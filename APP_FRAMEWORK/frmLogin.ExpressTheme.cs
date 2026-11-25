using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace APP_FRAMEWORK
{
    // Express KEY LOGIN - visual layer only.
    // No login / validation / settings logic lives here; it only restyles the existing
    // controls and mirrors their values (toggles write the same Bunifu switches as before).
    public partial class frmLogin
    {
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateRoundRectRgn(int l, int t, int r, int b, int w, int h);

        [DllImport("user32.dll")]
        private static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool redraw);

        private Panel exHeader;
        private Panel exClose;
        private Panel exCard;
        private Panel exBrand;
        private Panel exRemember;
        private Panel exAuto;
        private Panel exFloat;          // animated floating label of the licence key field
        private Label exTitle;
        private Label exSub;
        private Label exRememberCap;
        private Label exAutoCap;

        private Timer exAnim;
        private float exHoverSubmit, exHoverExit, exHoverClose, exHoverStop;
        private bool exPressSubmit, exPressExit, exPressClose;
        private float exRemPos = 1f, exAutoPos = 1f;
        private float exFloatT, exFocusT, exFieldHover;
        private Bitmap exLoaderBg;

        private float exK = 1f;
        private bool exDrag;
        private int exDragX, exDragY;

        // ---- layout (96 dpi units; everything is multiplied by exK on high-dpi) ----
        private const int BASE_W = 340;
        private const int BASE_H = 290;
        private const int HEAD_H = 38;
        private const int CORNER = 14;
        private const int BTN_RADIUS = 9;
        private const int CARD_X = 10, CARD_Y = 48, CARD_W = 320, CARD_H = 232;
        // content edges inside the card: x 16 .. 356 (buttons and the field border share them)
        private const int EDGE_L = 12, EDGE_R = 308;
        // panel2 carries a 3px glow margin on the sides and a 9px notch band on top for the floated label
        private const int FIELD_PAD = 3, FIELD_NOTCH = 16;
        private const int FIELD_X = EDGE_L - FIELD_PAD, FIELD_Y = 62, FIELD_W = EDGE_R - EDGE_L + FIELD_PAD * 2, FIELD_H = 59;
        private const int FIELD_TEXT_X = 36;
        private static readonly Color ExKeyText = Color.FromArgb(214, 96, 4);

        private static readonly Color ExBlue = Color.FromArgb(29, 98, 216);
        private static readonly Color ExBlueDeep = Color.FromArgb(16, 64, 160);
        private static readonly Color ExBlueSoft = Color.FromArgb(37, 99, 235);
        private static readonly Color ExSaffron = Color.FromArgb(245, 146, 28);
        private static readonly Color ExSaffronL = Color.FromArgb(248, 158, 44);
        private static readonly Color ExSaffronD = Color.FromArgb(214, 96, 4);
        private static readonly Color ExRed = Color.FromArgb(220, 38, 38);
        private static readonly Color ExRedDeep = Color.FromArgb(153, 27, 27);
        private static readonly Color ExPageBg = Color.FromArgb(236, 242, 251);
        private static readonly Color ExInk = Color.FromArgb(30, 41, 59);
        private static readonly Color ExSlate = Color.FromArgb(100, 116, 139);
        private static readonly Color ExLine = Color.FromArgb(214, 224, 240);
        private static readonly Color ExFieldBg = Color.White;

        private readonly Dictionary<Control, Rectangle> exBaseRect = new Dictionary<Control, Rectangle>();

        private void ApplyExpressTheme()
        {
            SuspendLayout();

            using (Graphics g = CreateGraphics()) exK = g.DpiX / 96f;
            if (exK < 1f) exK = 1f;
            ExS_DPI = 1f;   // controls are laid out at 96 dpi first, then the whole tree is scaled once

            AutoScaleMode = AutoScaleMode.None;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(BASE_W, BASE_H);
            BackColor = ExPageBg;
            ForeColor = ExInk;
            Font = UiTheme.FBase;
            DoubleBuffered = true;

            // Bunifu would square our rounded corners with its own region
            try { bunifuElipse1.TargetControl = null; } catch { }
            bunifuElipse1.ElipseRadius = (int)(CORNER * exK);

            Paint -= ExFormPaint;
            Paint += ExFormPaint;

            // legacy surfaces stay in the form (logic uses them) but are not shown
            panel1.Visible = false;
            pnlFooter.Visible = false;
            checkBox1.Visible = false;
            B1AAA.Visible = false;
            B2AAA.Visible = false;
            C1AA0.Visible = false;
            C2AA0.Visible = false;
            btnLogin.Visible = false;
            txtUserName.Visible = false;
            txtPassword.Visible = false;
            encP.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            chkRemember.Visible = false;
            chkLoginScreenShow.Visible = false;
            bunifuButton1.Visible = false;
            bunifuToggleAppModeSwitch.Visible = false;
            bunifuToggleRememberSwitch.Visible = false;
            bunifuToggleAutoLoginSwitch.Visible = false;
            bunifuProgressBar1.Visible = false;
            bunifuProgressBar2.Visible = false;
            bunifuSeparator1.Visible = false;
            bunifuSeparator2.Visible = false;
            InfoList2.Visible = false;

            // ---------------- header ----------------
            exHeader = new Panel();
            exHeader.Location = new Point(0, 0);
            exHeader.Size = new Size(BASE_W, HEAD_H);
            exHeader.BackColor = ExBlue;
            ExBuffer(exHeader);
            exHeader.Paint += ExHeaderPaint;
            exHeader.MouseDown += ExHeaderDown;
            exHeader.MouseMove += ExHeaderMove;
            exHeader.MouseUp += (s, e) => exDrag = false;
            exHeader.Click += (s, e) => ActiveControl = null;
            Controls.Add(exHeader);
            exHeader.BringToFront();

            exClose = new Panel();
            exClose.Size = new Size(26, 22);
            exClose.Location = new Point(BASE_W - 10 - 26, (HEAD_H - 3 - 22) / 2);
            exClose.BackColor = Color.Transparent;
            exClose.Cursor = Cursors.Hand;
            ExBuffer(exClose);
            exClose.Paint += ExClosePaint;
            exClose.Click += (s, e) => Close();
            exHeader.Controls.Add(exClose);
            exClose.BringToFront();

            // ---------------- card ----------------
            exCard = new Panel();
            exCard.Location = new Point(CARD_X, CARD_Y);
            exCard.Size = new Size(CARD_W, CARD_H);
            exCard.BackColor = ExPageBg;          // the white rounded card is painted (smooth corners)
            ExBuffer(exCard);
            exCard.Paint += ExCardPaint;
            exCard.Click += (s, e) => ActiveControl = null;
            Controls.Add(exCard);
            exCard.BringToFront();

            exBrand = new Panel();
            exBrand.Location = new Point(EDGE_L, 12);
            exBrand.Size = new Size(38, 38);
            exBrand.BackColor = Color.Transparent;
            ExBuffer(exBrand);
            exBrand.Paint += ExBrandPaint;
            exBrand.Click += (s, e) => ActiveControl = null;
            exCard.Controls.Add(exBrand);

            // the only "EXPRESS" on the popup - next to the logo
            exTitle = ExLabel("EXPRESS", 68, 17, new Font("Segoe UI", 14F, FontStyle.Bold | FontStyle.Italic), ExBlue);
            exTitle.AutoSize = false;
            exTitle.Size = new Size(150, 26);
            exTitle.TextAlign = ContentAlignment.MiddleLeft;
            exSub = ExLabel("PREMIUM TATKAL SOFTWARE", 70, 42, new Font("Segoe UI Semibold", 6.75F), ExSlate);
            // wordmark + tagline are painted on the card (path text, measured - never clipped at any DPI)
            exTitle.Visible = false;
            exSub.Visible = false;

            // MAC DETAIL - kept (object + MouseHover handler) but hidden, as before the redesign
            label4.Parent = exCard;
            label4.Text = "MAC DETAIL";
            label4.AutoSize = false;
            label4.Location = new Point(332, 34);
            label4.Size = new Size(150, 14);
            label4.TextAlign = ContentAlignment.TopRight;
            label4.Font = new Font("Segoe UI", 7.5F);
            label4.ForeColor = Color.FromArgb(140, 158, 182);
            label4.BackColor = Color.Transparent;
            label4.Cursor = Cursors.Help;
            label4.BringToFront();
            label4.Visible = false;

            // ---------------- licence key field (floating label) ----------------
            panel2.Parent = exCard;
            panel2.Location = new Point(FIELD_X, FIELD_Y);
            panel2.Size = new Size(FIELD_W, FIELD_H);
            panel2.BackColor = Color.White;       // card colour; the field itself is painted rounded
            panel2.Region = null;
            ExBuffer(panel2);
            panel2.Paint -= ExFieldPaint;
            panel2.Paint += ExFieldPaint;
            panel2.Cursor = Cursors.IBeam;

            // The designer's old "Key" caption (label1) lives inside panel2 at (13,63). In the new field it
            // sat across the box's bottom-left border as a white strip - hide it (purely decorative, no logic).
            label1.Visible = false;
            panel2.Click += (s, e) => txtLogin.Focus();
            panel2.MouseEnter += (s, e) => exFieldHover = 1f;
            panel2.MouseLeave += (s, e) => exFieldHover = 0f;

            txtLogin.Parent = panel2;
            txtLogin.BorderStyle = BorderStyle.None;
            txtLogin.BackColor = ExFieldBg;
            txtLogin.ForeColor = ExKeyText;                  // typed key in bhagwa
            txtLogin.Font = ExF0("Segoe UI Semibold", 8.5F);
            // vertically centred inside the painted box (box = FIELD_NOTCH .. FIELD_H - FIELD_PAD)
            int boxMid = (FIELD_NOTCH + (FIELD_H - FIELD_PAD)) / 2;
            txtLogin.Location = new Point(FIELD_TEXT_X, boxMid - txtLogin.PreferredHeight / 2);
            txtLogin.Size = new Size(FIELD_W - FIELD_TEXT_X - 14, txtLogin.PreferredHeight);
            txtLogin.TextAlign = HorizontalAlignment.Left;
            txtLogin.HideSelection = false;
            txtLogin.Enter += (s, e) => ExInvalidateField();
            txtLogin.Leave += (s, e) => ExInvalidateField();
            // empty + unfocused: a faint placeholder inside the box (native cue banner - it is the
            // textbox's own hint text, so it can never overlap typed text)
            txtLogin.HandleCreated += (s, e) => ExSetCue();
            if (txtLogin.IsHandleCreated) ExSetCue();
            txtLogin.MouseEnter += (s, e) => exFieldHover = 1f;
            txtLogin.MouseLeave += (s, e) => exFieldHover = 0f;
            txtLogin.BackColorChanged += (s, e) => { if (txtLogin.BackColor != ExFieldBg) txtLogin.BackColor = ExFieldBg; };

            exFloat = new Panel();
            exFloat.BackColor = ExFieldBg;
            exFloat.Cursor = Cursors.IBeam;
            ExBuffer(exFloat);
            exFloat.Paint += ExFloatPaint;
            exFloat.Click += (s, e) => txtLogin.Focus();
            exFloat.MouseEnter += (s, e) => exFieldHover = 1f;
            exFloat.MouseLeave += (s, e) => exFieldHover = 0f;
            panel2.Controls.Add(exFloat);
            exFloat.BringToFront();

            // ---------------- toggles (mirror the Bunifu switches the logic reads) ----------------
            exRemember = ExToggle(new Point(EDGE_L, 132), true, ExRememberToggle);
            exRememberCap = ExLabel("Remember My Key", EDGE_L + 38, 133, ExF0("Segoe UI", 8F), ExInk);
            exRememberCap.Cursor = Cursors.Hand;
            exRememberCap.Click += ExRememberToggle;

            exAuto = ExToggle(new Point(166, 132), true, ExAutoToggle);
            exAutoCap = ExLabel("Auto Login", 166 + 38, 133, ExF0("Segoe UI", 8F), ExInk);
            exAutoCap.Cursor = Cursors.Hand;
            exAutoCap.Click += ExAutoToggle;

            // "Stop IP Block" - kept (object + CheckedChanged logic) but hidden, as before the redesign
            byteNovaRestartOption.Parent = exCard;
            byteNovaRestartOption.Location = new Point(18, 252);
            byteNovaRestartOption.ForeColor = Color.FromArgb(74, 96, 124);
            byteNovaRestartOption.Font = new Font("Segoe UI", 9F);
            byteNovaRestartOption.BackColor = Color.Transparent;
            byteNovaRestartOption.BringToFront();
            byteNovaRestartOption.Visible = false;

            // ---------------- buttons (same left/right edges as the field) ----------------
            ExStyleButton(bunifuFlatButton2, new Point(EDGE_L, 164), new Size(92, 34), Color.White, ExExitPaint);
            ExStyleButton(bunifuFlatButton1, new Point(EDGE_L + 92 + 8, 164), new Size(EDGE_R - (EDGE_L + 92 + 8), 34), ExBlue, ExSubmitPaint);

            // ---------------- validating overlay ----------------
            if (byteNovaServerLoader != null)
            {
                byteNovaServerLoader.Paint -= ByteNovaServerLoaderPaint;
                byteNovaServerLoader.Paint -= ExLoaderPaint;
                byteNovaServerLoader.Paint += ExLoaderPaint;
                byteNovaServerLoader.BackColor = ExPageBg;
                byteNovaServerLoader.Size = new Size(BASE_W, BASE_H - HEAD_H);
                byteNovaServerLoader.Cursor = Cursors.WaitCursor;
                byteNovaServerLoader.VisibleChanged -= ExLoaderVisibleChanged;
                byteNovaServerLoader.VisibleChanged += ExLoaderVisibleChanged;
                byteNovaServerLoader.Click -= ExLoaderClick;
                byteNovaServerLoader.Click += ExLoaderClick;
            }

            if (exK > 1f)
            {
                ExScaleTree(this, exK);
                ClientSize = new Size((int)(BASE_W * exK), (int)(BASE_H * exK));
            }
            ExS_DPI = exK;
            MinimumSize = ClientSize;
            MaximumSize = ClientSize;
            exHeader.Width = ClientSize.Width;

            ExApplyRegion();

            exAnim = new Timer();
            exAnim.Interval = 16;
            exAnim.Tick += ExAnimTick;
            exAnim.Start();

            ExSyncToggles();
            exFloatT = ExFloatTarget();
            ExLayoutFloat();

            ResumeLayout(true);
            PerformLayout();
            Invalidate(true);
        }

        // ------------------------------------------------------------------
        // helpers
        // ------------------------------------------------------------------
        private static void ExBuffer(Control c)
        {
            try
            {
                typeof(Control).GetProperty("DoubleBuffered",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                    .SetValue(c, true, null);
            }
            catch { }
        }

        private void ExStyleButton(Bunifu.Framework.UI.BunifuFlatButton b, Point at, Size size, Color baseCol, PaintEventHandler paint)
        {
            b.Parent = exCard;
            b.Location = at;
            b.Size = size;
            b.BorderRadius = BTN_RADIUS;
            b.BackColor = Color.White;
            b.Normalcolor = baseCol;
            b.Activecolor = baseCol;
            b.OnHovercolor = baseCol;
            b.Textcolor = Color.White;
            b.OnHoverTextColor = Color.White;
            b.Cursor = Cursors.Hand;
            b.Text = "";
            b.Region = null;
            b.Paint -= paint;
            b.Paint += paint;
            b.BringToFront();
        }

        private void ExSyncToggles()
        {
            if (exRemember == null || exAuto == null) return;
            exRemember.Tag = bunifuToggleRememberSwitch.Value;
            exAuto.Tag = bunifuToggleAutoLoginSwitch.Value;
            exRemPos = bunifuToggleRememberSwitch.Value ? 1f : 0f;
            exAutoPos = bunifuToggleAutoLoginSwitch.Value ? 1f : 0f;
            exRemember.Invalidate();
            exAuto.Invalidate();
        }

        // 1 = raised (focused), 0 = sitting on the top border (unfocused, with or without text)
        private float ExFloatTarget()
        {
            return txtLogin.Focused ? 1f : 0f;
        }

        // Pixel-unit fonts: exK is applied exactly once, so Windows DPI can't double-size and clip them.
        private Font ExF(string family, float pt, FontStyle style = FontStyle.Regular)
        {
            return new Font(family, pt * 96f / 72f * exK, style, GraphicsUnit.Pixel);
        }

        // 96-dpi pixel font for controls (ExScaleTree scales pixel fonts by exK afterwards)
        private static Font ExF0(string family, float pt, FontStyle style = FontStyle.Regular)
        {
            return new Font(family, pt * 96f / 72f, style, GraphicsUnit.Pixel);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

        private void ExSetCue()
        {
            try { SendMessage(txtLogin.Handle, 0x1501 /* EM_SETCUEBANNER */, IntPtr.Zero, "Enter your licence key"); } catch { }
        }

        // Floating label:
        //   unfocused -> a notch ON the top border line (never inside the box)
        //   focused   -> rises fully ABOVE the box as a small caption
        private void ExLayoutFloat()
        {
            if (exFloat == null) return;
            float t = ExEase(exFloatT);
            float lineY = FIELD_NOTCH;                                   // top border of the box
            float onH = 12f, onY = lineY - onH / 2f, onX = FIELD_PAD + 9f;
            float upH = 13f, upY = lineY - upH - 3f, upX = FIELD_PAD + 1f;
            float w;
            using (Font uf = ExF0("Segoe UI", 7.5F, FontStyle.Bold))
                w = TextRenderer.MeasureText("LICENCE KEY", uf, Size.Empty, TextFormatFlags.NoPadding).Width + 12f;
            float x = onX + (upX - onX) * t;
            float y = onY + (upY - onY) * t;
            float h = onH + (upH - onH) * t;
            Rectangle want = new Rectangle(ExS(x), ExS(y), ExS(w), ExS(h));
            if (exFloat.Bounds != want) exFloat.Bounds = want;
            exFloat.Invalidate();
        }

        private static float ExEase(float t)
        {
            t = Math.Max(0f, Math.Min(1f, t));
            return 1f - (1f - t) * (1f - t) * (1f - t);   // ease-out cubic
        }

        private static GraphicsPath ExRound(Rectangle r, int radius)
        {
            return ExRoundF(new RectangleF(r.X, r.Y, r.Width, r.Height), radius);
        }

        private static GraphicsPath ExRoundF(RectangleF r, float radius)
        {
            GraphicsPath p = new GraphicsPath();
            float d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            if (d <= 1) { p.AddRectangle(r); return p; }
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        private void ExRoundControl(Control c, int radius)
        {
            if (c.Width < 2 || c.Height < 2) return;
            using (GraphicsPath p = ExRound(new Rectangle(0, 0, c.Width, c.Height), (int)(radius * exK)))
            {
                Region old = c.Region;
                c.Region = new Region(p);
                if (old != null) old.Dispose();
            }
        }

        private void ExApplyRegion()
        {
            if (!IsHandleCreated) return;
            if (Width < 4 || Height < 4) return;

            // Only (re)apply when the size really changed. SetWindowRgn(redraw) repaints the whole window,
            // incl. the TextBox, and was re-asserted every 16 ms for 1.5 s after Show - that kills the caret blink.
            if (exRegionSize == Size && exRegionHandle == Handle) return;
            exRegionSize = Size;
            exRegionHandle = Handle;

            // Windows 11: let DWM round the window - real anti-aliased corners, no Region at all.
            if (ExTryDwmRound())
            {
                SetWindowRgn(Handle, IntPtr.Zero, true);
                return;
            }

            // Older Windows: a Region can't anti-alias, so the frame painter fills the pixels outside the
            // rounded path with the border colour and strokes the AA border exactly on the edge.
            int d = (int)Math.Round(ExCornerR() * 2);
            IntPtr h = CreateRoundRectRgn(0, 0, Width + 1, Height + 1, d, d);
            SetWindowRgn(Handle, h, true);
        }

        private Size exRegionSize;
        private IntPtr exRegionHandle;

        // Repaint the field chrome but never the TextBox area (repainting it hides / restarts the caret).
        private void ExInvalidateField()
        {
            if (panel2 == null || panel2.IsDisposed) return;
            using (Region rg = new Region(panel2.ClientRectangle))
            {
                if (txtLogin != null && txtLogin.Parent == panel2) rg.Exclude(txtLogin.Bounds);
                panel2.Invalidate(rg, false);
            }
        }

        private void ExScaleTree(Control parent, float k)
        {
            parent.SuspendLayout();
            foreach (Control c in parent.Controls)
            {
                c.Location = new Point((int)Math.Round(c.Left * k), (int)Math.Round(c.Top * k));
                if (!c.AutoSize)
                    c.Size = new Size((int)Math.Round(c.Width * k), (int)Math.Round(c.Height * k));
                // only pixel-unit fonts are scaled; point fonts already follow Windows DPI (scaling them again double-sizes and clips)
                if (c.Font.Unit == GraphicsUnit.Pixel)
                    try { c.Font = new Font(c.Font.FontFamily, Math.Max(6f, c.Font.Size * k), c.Font.Style, GraphicsUnit.Pixel); } catch { }
                ExScaleTree(c, k);
            }
            parent.ResumeLayout(false);
        }

        private void ExCaptureBase(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                exBaseRect[c] = c.Bounds;
                ExCaptureBase(c);
            }
        }

        // only bounds scale, fonts do not - no vibration
        private void ExApplyScale(Control parent, float s, Point parentCenter)
        {
            foreach (Control c in parent.Controls)
            {
                if (!exBaseRect.ContainsKey(c)) continue;

                Rectangle b = exBaseRect[c];
                int cx = b.X + b.Width / 2;
                int cy = b.Y + b.Height / 2;

                int nx = parentCenter.X + (int)((cx - parentCenter.X) * s);
                int ny = parentCenter.Y + (int)((cy - parentCenter.Y) * s);
                int nw = Math.Max(1, (int)(b.Width * s));
                int nh = Math.Max(1, (int)(b.Height * s));

                c.Bounds = new Rectangle(nx - nw / 2, ny - nh / 2, nw, nh);
                ExApplyScale(c, s, new Point(nx, ny));
            }
        }

        private Label ExLabel(string text, int x, int y, Font font, Color color)
        {
            Label l = new Label();
            l.Text = text;
            l.AutoSize = true;
            l.Location = new Point(x, y);
            l.Font = font;
            l.ForeColor = color;
            l.BackColor = Color.Transparent;
            l.UseCompatibleTextRendering = false;
            l.Click += (s, e) => ActiveControl = null;
            exCard.Controls.Add(l);
            l.BringToFront();
            return l;
        }

        private Panel ExToggle(Point p, bool state, EventHandler onClick)
        {
            Panel t = new Panel();
            t.Location = p;
            t.Size = new Size(32, 18);
            t.BackColor = Color.White;
            t.Cursor = Cursors.Hand;
            t.Tag = state;
            ExBuffer(t);
            t.Paint += ExTogglePaint;
            t.Click += onClick;
            exCard.Controls.Add(t);
            t.BringToFront();
            return t;
        }

        private void ExRememberToggle(object sender, EventArgs e)
        {
            bool on = !(bool)exRemember.Tag;
            exRemember.Tag = on;
            bunifuToggleRememberSwitch.Value = on;
            exRemember.Invalidate();
        }

        private void ExAutoToggle(object sender, EventArgs e)
        {
            bool on = !(bool)exAuto.Tag;
            exAuto.Tag = on;
            bunifuToggleAutoLoginSwitch.Value = on;
            exAuto.Invalidate();
        }

        private void ExHeaderDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            exDrag = true;
            exDragX = Cursor.Position.X - Left;
            exDragY = Cursor.Position.Y - Top;
        }

        private void ExHeaderMove(object sender, MouseEventArgs e)
        {
            if (!exDrag) return;
            Left = Cursor.Position.X - exDragX;
            Top = Cursor.Position.Y - exDragY;
        }

        private static float ExApproach(float value, float target)
        {
            float diff = target - value;
            if (Math.Abs(diff) < 0.02f) return target;
            return value + diff * 0.22f;
        }

        private void ExAnimTick(object sender, EventArgs e)
        {
            if (!Visible || IsDisposed) return;

            Point pc = Cursor.Position;
            Point inCard = exCard.PointToClient(pc);
            Point inHead = exHeader.PointToClient(pc);
            bool down = Control.MouseButtons == MouseButtons.Left;
            bool loaderUp = byteNovaServerLoader != null && byteNovaServerLoader.Visible;

            bool overSubmit = !loaderUp && bunifuFlatButton1.Enabled && bunifuFlatButton1.Bounds.Contains(inCard);
            bool overExit = !loaderUp && bunifuFlatButton2.Bounds.Contains(inCard);
            bool overClose = exClose.Bounds.Contains(inHead);

            float ns = ExApproach(exHoverSubmit, overSubmit ? 1f : 0f);
            bool ps = overSubmit && down;
            if (ns != exHoverSubmit || ps != exPressSubmit)
            {
                exHoverSubmit = ns;
                exPressSubmit = ps;
                bunifuFlatButton1.Invalidate();
            }

            float ne = ExApproach(exHoverExit, overExit ? 1f : 0f);
            bool pe = overExit && down;
            if (ne != exHoverExit || pe != exPressExit)
            {
                exHoverExit = ne;
                exPressExit = pe;
                bunifuFlatButton2.Invalidate();
            }

            float nc = ExApproach(exHoverClose, overClose ? 1f : 0f);
            bool pcl = overClose && down;
            if (nc != exHoverClose || pcl != exPressClose)
            {
                exHoverClose = nc;
                exPressClose = pcl;
                exClose.Invalidate();
            }

            // keep the painted toggles in step with the real switches (method_4 sets them on Load)
            if ((bool)exRemember.Tag != bunifuToggleRememberSwitch.Value) exRemember.Tag = bunifuToggleRememberSwitch.Value;
            if ((bool)exAuto.Tag != bunifuToggleAutoLoginSwitch.Value) exAuto.Tag = bunifuToggleAutoLoginSwitch.Value;

            float nr = ExApproach(exRemPos, (bool)exRemember.Tag ? 1f : 0f);
            if (nr != exRemPos) { exRemPos = nr; exRemember.Invalidate(); }

            float na = ExApproach(exAutoPos, (bool)exAuto.Tag ? 1f : 0f);
            if (na != exAutoPos) { exAutoPos = na; exAuto.Invalidate(); }

            // floating label + focus ring
            float ft = ExApproach(exFloatT, ExFloatTarget());
            float fo = ExApproach(exFocusT, txtLogin.Focused ? 1f : 0f);
            if (ft != exFloatT) { exFloatT = ft; ExLayoutFloat(); }
            // only while the focus animation is running; once settled nothing near the textbox is repainted
            if (fo != exFocusT) { exFocusT = fo; ExInvalidateField(); exFloat.Invalidate(); }

            if (loaderUp) byteNovaServerLoader.Invalidate(ExLoaderCardRect(byteNovaServerLoader));
        }

        private static Color ExMix(Color a, Color b, float t)
        {
            t = Math.Max(0f, Math.Min(1f, t));
            return Color.FromArgb(
                (int)(a.A + (b.A - a.A) * t),
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t));
        }

        private static float ExS_DPI = 1f;

        private static int ExS(float v)
        {
            return (int)Math.Round(v * ExS_DPI);
        }

        private static void ExText(Graphics g, string text, Font f, Rectangle r, Color c, TextFormatFlags flags)
        {
            TextRenderer.DrawText(g, text, f, r, c, flags | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
        }

        // Lucide-style icons on a 24x24 grid (2px stroke, round caps / joins).
        private static void ExIcon(Graphics g, string kind, RectangleF r, Color c, float stroke)
        {
            GraphicsState st = g.Save();
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TranslateTransform(r.X, r.Y);
            g.ScaleTransform(r.Width / 24f, r.Height / 24f);
            float w = stroke * 24f / r.Width;
            using (Pen p = new Pen(c, w))
            {
                p.StartCap = LineCap.Round; p.EndCap = LineCap.Round; p.LineJoin = LineJoin.Round;
                switch (kind)
                {
                    case "key":
                        g.DrawEllipse(p, 2.5f, 11.5f, 9f, 9f);
                        g.DrawLine(p, 10.4f, 13.6f, 21f, 3f);
                        g.DrawLine(p, 15.5f, 8.5f, 18.5f, 11.5f);
                        g.DrawLine(p, 18.5f, 5.5f, 21f, 8f);
                        break;
                    case "shield":
                        using (GraphicsPath sp = new GraphicsPath())
                        {
                            sp.AddLine(12f, 2.5f, 20f, 5.5f);
                            sp.AddLine(20f, 5.5f, 20f, 12f);
                            sp.AddBezier(20f, 12f, 20f, 17f, 16f, 20f, 12f, 22f);
                            sp.AddBezier(12f, 22f, 8f, 20f, 4f, 17f, 4f, 12f);
                            sp.AddLine(4f, 12f, 4f, 5.5f);
                            sp.CloseFigure();
                            g.DrawPath(p, sp);
                        }
                        g.DrawLines(p, new[] { new PointF(9f, 12f), new PointF(11f, 14f), new PointF(15f, 10f) });
                        break;
                    case "login":
                        g.DrawLines(p, new[] { new PointF(15f, 3f), new PointF(19f, 3f), new PointF(21f, 5f), new PointF(21f, 19f), new PointF(19f, 21f), new PointF(15f, 21f) });
                        g.DrawLines(p, new[] { new PointF(10f, 7f), new PointF(15f, 12f), new PointF(10f, 17f) });
                        g.DrawLine(p, 15f, 12f, 3f, 12f);
                        break;
                    case "power":
                        g.DrawArc(p, 3f, 4f, 18f, 18f, -50f, 280f);
                        g.DrawLine(p, 12f, 2f, 12f, 12f);
                        break;
                    case "x":
                        g.DrawLine(p, 6f, 6f, 18f, 18f);
                        g.DrawLine(p, 18f, 6f, 6f, 18f);
                        break;
                    case "check":
                        g.DrawLines(p, new[] { new PointF(5f, 12.5f), new PointF(10f, 17.5f), new PointF(19f, 7f) });
                        break;
                    case "info":
                        g.DrawEllipse(p, 2f, 2f, 20f, 20f);
                        g.DrawLine(p, 12f, 16f, 12f, 11.5f);
                        g.DrawLine(p, 12f, 8f, 12.01f, 8f);
                        break;
                    case "wifi":
                        g.DrawArc(p, 2f, 7f, 20f, 20f, 225f, 90f);
                        g.DrawArc(p, 6f, 11f, 12f, 12f, 225f, 90f);
                        g.DrawLine(p, 12f, 20f, 12.01f, 20f);
                        break;
                }
            }
            g.Restore(st);
        }

        // ------------------------------------------------------------------
        // painting
        // ------------------------------------------------------------------
        private void ExFormPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (SolidBrush bg = new SolidBrush(ExPageBg))
                g.FillRectangle(bg, new Rectangle(0, 0, ClientSize.Width, ClientSize.Height));

            ExPaintFrame(g, Point.Empty);
        }

        // ------------------------------------------------------------------
        // window frame: smooth rounded corners + 3px bhagwa gradient border
        // ------------------------------------------------------------------
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int value, int size);

        [StructLayout(LayoutKind.Sequential)]
        private struct ExOsVersion
        {
            public int Size, Major, Minor, Build, Platform;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string Csd;
        }

        [DllImport("ntdll.dll")]
        private static extern int RtlGetVersion(ref ExOsVersion v);

        private bool exDwmTried, exDwmRound;

        private bool ExTryDwmRound()
        {
            if (exDwmTried) return exDwmRound;
            exDwmTried = true;
            try
            {
                ExOsVersion v = new ExOsVersion();
                v.Size = Marshal.SizeOf(typeof(ExOsVersion));
                if (RtlGetVersion(ref v) != 0 || v.Major < 10 || v.Build < 22000) return exDwmRound = false;
                int pref = 2;   // DWMWCP_ROUND
                exDwmRound = DwmSetWindowAttribute(Handle, 33 /* DWMWA_WINDOW_CORNER_PREFERENCE */, ref pref, sizeof(int)) == 0;
            }
            catch { exDwmRound = false; }
            if (exDwmRound) Invalidate(true);   // repaint the frame with the DWM radius
            return exDwmRound;
        }

        // DWM uses an 8px (logical) radius on Windows 11; our own Region uses CORNER.
        private float ExCornerR()
        {
            return (exDwmRound ? 8f : CORNER) * exK;
        }

        private const float FRAME_W = 3f;

        // Paints the outer frame in form coordinates; `offset` is the top-left of the surface being painted
        // (form = 0,0; header / overlay pass their Location so the frame is continuous over them).
        private void ExPaintFrame(Graphics g, Point offset)
        {
            int W = ClientSize.Width, H = ClientSize.Height;
            if (W < 4 || H < 4) return;
            GraphicsState st = g.Save();
            g.TranslateTransform(-offset.X, -offset.Y);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float R = ExCornerR();
            float bw = FRAME_W * exK;
            Rectangle all = new Rectangle(0, 0, W, H);
            using (LinearGradientBrush br = new LinearGradientBrush(all, ExSaffronD, ExSaffronL, 35f))
            {
                // 1) every pixel outside the rounded edge gets the border colour -> no white specks / stair steps
                using (GraphicsPath edge = ExRoundF(new RectangleF(0, 0, W, H), R))
                using (Region outside = new Region(all))
                {
                    outside.Exclude(edge);
                    g.FillRegion(br, outside);
                }
                // 2) anti-aliased border exactly on the window edge, following the corners
                using (GraphicsPath ring = ExRoundF(new RectangleF(bw / 2f, bw / 2f, W - bw, H - bw), Math.Max(1f, R - bw / 2f)))
                using (Pen pen = new Pen(br, bw))
                {
                    pen.LineJoin = LineJoin.Round;
                    g.DrawPath(pen, ring);
                }
            }
            g.Restore(st);
        }

        private void ExHeaderPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, exHeader.Width, exHeader.Height);

            using (LinearGradientBrush bg = new LinearGradientBrush(r, ExBlueDeep, ExBlueSoft, 0f))
            {
                bg.InterpolationColors = new ColorBlend
                {
                    Positions = new[] { 0f, 0.6f, 1f },
                    Colors = new[] { ExBlueDeep, ExBlue, Color.FromArgb(59, 130, 246) }
                };
                g.FillRectangle(bg, r);
            }
            using (SolidBrush gloss = new SolidBrush(Color.FromArgb(22, 255, 255, 255)))
                g.FillRectangle(gloss, 0, 0, r.Width, (r.Height - ExS(3)) / 2);

            // bhagwa underline
            Rectangle ul = new Rectangle(0, r.Height - ExS(3), r.Width, ExS(3));
            using (LinearGradientBrush ob = new LinearGradientBrush(new Rectangle(ul.X, ul.Y, Math.Max(1, ul.Width), Math.Max(1, ul.Height)), ExSaffronD, ExSaffronL, 0f))
                g.FillRectangle(ob, ul);

            // key badge + "Key Login" (the brand name appears only once, beside the logo in the card)
            int h = r.Height - ExS(3);
            Rectangle badge = new Rectangle(ExS(10), (h - ExS(22)) / 2, ExS(22), ExS(22));
            using (SolidBrush sh = new SolidBrush(Color.FromArgb(45, 0, 0, 0)))
                g.FillEllipse(sh, badge.X, badge.Y + ExS(1), badge.Width, badge.Height);
            using (SolidBrush wb = new SolidBrush(Color.White))
                g.FillEllipse(wb, badge);
            ExIcon(g, "key", new RectangleF(badge.X + ExS(5), badge.Y + ExS(5), ExS(12), ExS(12)), ExSaffronD, 1.8f * exK);

            using (Font tf = ExF("Segoe UI Semibold", 9F))
            {
                Size ts = TextRenderer.MeasureText(g, "Key Login", tf, Size.Empty, TextFormatFlags.NoPadding);
                ExText(g, "Key Login", tf, new Rectangle(badge.Right + ExS(8), (h - ts.Height) / 2, ts.Width + ExS(4), ts.Height),
                    Color.White, TextFormatFlags.Left);
            }

            // the header is a full-width child: repaint the window frame over it so its top corners stay smooth
            ExPaintFrame(g, exHeader.Location);
        }

        private void ExClosePaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // repaint the header gradient under the button (transparent panel)
            Rectangle pr = new Rectangle(-exClose.Left, -exClose.Top, Math.Max(1, exHeader.Width), Math.Max(1, exHeader.Height));
            using (LinearGradientBrush bg = new LinearGradientBrush(pr, ExBlueDeep, Color.FromArgb(59, 130, 246), 0f))
            {
                bg.InterpolationColors = new ColorBlend
                {
                    Positions = new[] { 0f, 0.6f, 1f },
                    Colors = new[] { ExBlueDeep, ExBlue, Color.FromArgb(59, 130, 246) }
                };
                g.FillRectangle(bg, 0, 0, exClose.Width, exClose.Height);
            }

            int dy = exPressClose ? ExS(1) : 0;
            RectangleF r = new RectangleF(0.5f, 0.5f + dy, exClose.Width - 2f, exClose.Height - 2f - ExS(1));

            // default red / hover white + red X + red border / pressed dark red
            Color fill = exPressClose ? ExRedDeep : ExMix(ExRed, Color.White, exHoverClose);
            Color line = exPressClose ? Color.FromArgb(127, 29, 29) : ExMix(Color.FromArgb(185, 28, 28), ExRed, exHoverClose);
            Color glyph = exPressClose ? Color.White : ExMix(Color.White, ExRed, exHoverClose);

            if (!exPressClose)
                using (GraphicsPath sp = ExRoundF(new RectangleF(r.X, r.Y + ExS(1.5f), r.Width, r.Height), ExS(8)))
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(60, 0, 0, 0)))
                    g.FillPath(sb, sp);
            using (GraphicsPath path = ExRoundF(r, ExS(8)))
            {
                using (SolidBrush b = new SolidBrush(fill)) g.FillPath(b, path);
                using (Pen p = new Pen(line, 1.2f * exK)) g.DrawPath(p, path);
            }
            float s = ExS(16);
            ExIcon(g, "x", new RectangleF(r.X + (r.Width - s) / 2f, r.Y + (r.Height - s) / 2f, s, s), glyph, 2.4f * exK);
        }

        private void ExCardPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            RectangleF r = new RectangleF(0.5f, 0.5f, exCard.Width - 2f, exCard.Height - 2f - ExS(2));

            using (GraphicsPath sh = ExRoundF(new RectangleF(r.X, r.Y + ExS(2), r.Width, r.Height), ExS(12)))
            using (SolidBrush sb = new SolidBrush(Color.FromArgb(214, 225, 243)))
                g.FillPath(sb, sh);
            using (GraphicsPath path = ExRoundF(r, ExS(12)))
            {
                using (SolidBrush b = new SolidBrush(Color.White)) g.FillPath(b, path);
                using (Pen pen = new Pen(ExLine, 1f)) g.DrawPath(pen, path);
            }

            ExPaintWordmark(g);

            // "SECURE" badge (shield-check) top-right
            using (Font bf = ExF("Segoe UI", 6.5F, FontStyle.Bold))
            {
                Size ts = TextRenderer.MeasureText(g, "SECURE", bf, Size.Empty, TextFormatFlags.NoPadding);
                int bw = ts.Width + ExS(24);
                Rectangle badge = new Rectangle(ExS(EDGE_R) - bw, ExS(14), bw, ExS(18));
                using (GraphicsPath bp = ExRound(badge, badge.Height / 2))
                {
                    using (SolidBrush b = new SolidBrush(Color.FromArgb(220, 252, 231))) g.FillPath(b, bp);
                    using (Pen p = new Pen(Color.FromArgb(134, 239, 172), 1f)) g.DrawPath(p, bp);
                }
                ExIcon(g, "shield", new RectangleF(badge.X + ExS(6), badge.Y + ExS(4), ExS(10), ExS(10)), Color.FromArgb(21, 128, 61), 1.4f * exK);
                ExText(g, "SECURE", bf, new Rectangle(badge.X + ExS(19), badge.Y, ts.Width + ExS(4), badge.Height),
                    Color.FromArgb(21, 128, 61), TextFormatFlags.Left | TextFormatFlags.VerticalCenter);

                using (Font vf = ExF("Segoe UI", 6.5F))
                    ExText(g, "v" + Program.appversion, vf, new Rectangle(badge.Right - ExS(80), badge.Bottom + ExS(4), ExS(80) - ExS(6), ExS(14)),
                        ExSlate, TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
            }

            // thin divider between brand and form
            using (Pen p = new Pen(Color.FromArgb(234, 239, 247), 1f))
                g.DrawLine(p, ExS(EDGE_L), ExS(58), ExS(EDGE_R), ExS(58));

            // footer hint with info icon (centred, quiet)
            using (Font hf = ExF("Segoe UI", 7F))
            {
                string hint = "Server se validate hone ke baad software khulega.";
                Size hs = TextRenderer.MeasureText(g, hint, hf, Size.Empty, TextFormatFlags.NoPadding);
                int iw = ExS(11), gap = ExS(5);
                int fx = (exCard.Width - (iw + gap + hs.Width)) / 2;
                int fy = ExS(206);
                ExIcon(g, "info", new RectangleF(fx, fy + (ExS(18) - iw) / 2f, iw, iw), ExSaffron, 1.5f * exK);
                ExText(g, hint, hf, new Rectangle(fx + iw + gap, fy, hs.Width + ExS(4), ExS(18)), ExSlate,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
            }
        }

        // EXPRESS wordmark: built as a GraphicsPath in pixels and positioned from its real bounds,
        // so ascent/descent are always fully inside the card at 100 / 125 / 150 % DPI.
        private void ExPaintWordmark(Graphics g)
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            float left = exBrand.Right + ExS(10);
            float top = exBrand.Top + ExS(1);

            using (GraphicsPath word = new GraphicsPath())
            using (FontFamily ff = new FontFamily("Segoe UI"))
            {
                FontStyle st = ff.IsStyleAvailable(FontStyle.Bold | FontStyle.Italic) ? FontStyle.Bold | FontStyle.Italic : FontStyle.Bold;
                word.AddString("EXPRESS", ff, (int)st, ExS(21), PointF.Empty, StringFormat.GenericTypographic);
                RectangleF wb = word.GetBounds();
                using (Matrix m = new Matrix())
                {
                    m.Translate(left - wb.X, top - wb.Y);
                    word.Transform(m);
                }
                wb = word.GetBounds();

                // soft bhagwa glow + blue gradient fill
                using (Pen glow = new Pen(Color.FromArgb(38, ExSaffron), ExS(3)))
                {
                    glow.LineJoin = LineJoin.Round;
                    g.DrawPath(glow, word);
                }
                using (LinearGradientBrush wbr = new LinearGradientBrush(
                    new RectangleF(wb.X, wb.Y - 1, Math.Max(1, wb.Width), wb.Height + 2), ExBlueDeep, ExBlueSoft, 90f))
                    g.FillPath(wbr, word);

                // bhagwa accent underline
                float uy = wb.Bottom + ExS(4);
                RectangleF ul = new RectangleF(wb.X + ExS(1), uy, ExS(26), ExS(2.5f));
                using (GraphicsPath up = ExRoundF(ul, ul.Height / 2f))
                using (LinearGradientBrush ub = new LinearGradientBrush(new RectangleF(ul.X - 1, ul.Y, ul.Width + 2, ul.Height), ExSaffronD, ExSaffronL, 0f))
                    g.FillPath(ub, up);

                // tagline, measured so it is never clipped
                using (Font tf = ExF("Segoe UI Semibold", 6F))
                {
                    Size ts = TextRenderer.MeasureText(g, "PREMIUM TATKAL SOFTWARE", tf, Size.Empty, TextFormatFlags.NoPadding);
                    ExText(g, "PREMIUM TATKAL SOFTWARE", tf,
                        new Rectangle((int)wb.X + ExS(1), (int)ul.Bottom + ExS(3), ts.Width + ExS(4), ts.Height + 2),
                        ExSlate, TextFormatFlags.Left);
                }
            }
        }

        private void ExBrandPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            RectangleF r = new RectangleF(1f, 1f, exBrand.Width - 3f, exBrand.Height - 3f);

            using (GraphicsPath path = ExRoundF(r, ExS(11)))
            {
                using (LinearGradientBrush bg = new LinearGradientBrush(r, Color.FromArgb(255, 247, 237), Color.FromArgb(255, 237, 213), 90f))
                    g.FillPath(bg, path);
                using (LinearGradientBrush rim = new LinearGradientBrush(r, ExSaffronL, ExSaffronD, 45f))
                using (Pen p = new Pen(rim, 1.6f * exK))
                    g.DrawPath(p, path);
            }

            if (byteNovaLogoImage != null)
            {
                int box = exBrand.Width - ExS(12);
                float scale = Math.Min((float)box / byteNovaLogoImage.Width, (float)box / byteNovaLogoImage.Height);
                int w = Math.Max(1, (int)(byteNovaLogoImage.Width * scale));
                int h = Math.Max(1, (int)(byteNovaLogoImage.Height * scale));
                g.DrawImage(byteNovaLogoImage, new Rectangle((exBrand.Width - w) / 2, (exBrand.Height - h) / 2, w, h));
            }
            else
            {
                using (Font f = ExF("Segoe UI Black", 15F, FontStyle.Bold))
                    ExText(g, "E", f, new Rectangle(0, 0, exBrand.Width, exBrand.Height), ExSaffronD,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private void ExFieldPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            float f = exFocusT;
            // outlined box: exactly EDGE_L..EDGE_R in card space, top border leaves room for the notch label
            RectangleF r = new RectangleF(ExS(FIELD_PAD) + 0.5f, ExS(FIELD_NOTCH) + 0.5f,
                panel2.Width - ExS(FIELD_PAD * 2) - 1f, panel2.Height - ExS(FIELD_NOTCH) - ExS(FIELD_PAD) - 1f);

            // focus glow ring
            if (f > 0.01f)
                using (GraphicsPath gp = ExRoundF(RectangleF.Inflate(r, ExS(2.5f), ExS(2.5f)), ExS(11)))
                using (SolidBrush gb = new SolidBrush(Color.FromArgb((int)(52 * f), ExBlueSoft)))
                    g.FillPath(gb, gp);

            using (GraphicsPath path = ExRoundF(r, ExS(9)))
            {
                using (SolidBrush bg = new SolidBrush(ExFieldBg)) g.FillPath(bg, path);
                Color idle = exFieldHover > 0f ? Color.FromArgb(148, 163, 184) : Color.FromArgb(203, 213, 225);
                using (Pen pen = new Pen(ExMix(idle, ExBlueSoft, f), (1.2f + 0.6f * f) * exK))
                    g.DrawPath(pen, path);
            }

            // bhagwa accent that grows along the bottom while focused
            if (f > 0.01f)
            {
                // kept strictly inside the rounded border: clipped to the box path, inset from the corners
                float lw = (r.Width - ExS(28)) * ExEase(f);
                float lx = r.X + (r.Width - lw) / 2f;
                using (GraphicsPath box = ExRoundF(RectangleF.Inflate(r, -ExS(1), -ExS(1)), ExS(8)))
                using (GraphicsPath lp = ExRoundF(new RectangleF(lx, r.Bottom - ExS(1) - ExS(2.5f), lw, ExS(2.5f)), ExS(1.2f)))
                using (LinearGradientBrush lb = new LinearGradientBrush(new RectangleF(lx - 1, r.Bottom - ExS(4), lw + 2, ExS(4)), ExSaffronD, ExSaffronL, 0f))
                {
                    Region old = g.Clip;
                    g.SetClip(box, CombineMode.Intersect);
                    g.FillPath(lb, lp);
                    g.Clip = old;
                }
            }

            // icon centred vertically in the box, centred in the column left of the text
            Color ic = ExMix(Color.FromArgb(120, 136, 160), ExSaffronD, f);
            float isz = ExS(14);
            float colL = r.X, colR = ExS(FIELD_TEXT_X) - ExS(4);
            ExIcon(g, "key", new RectangleF(colL + (colR - colL - isz) / 2f, r.Y + (r.Height - isz) / 2f, isz, isz), ic, 1.9f * exK);
        }

        private void ExFloatPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(ExFieldBg);   // white: on the border it is the notch, raised it sits on the white card
            float t = ExEase(exFloatT);
            float pt = 7F + (7.5F - 7F) * t;
            Color col = ExMix(ExSlate, ExBlueSoft, t);                 // slate on the border, blue when raised
            float pad = 4f * (1f - t);                                  // notch padding fades out when raised
            using (Font f = ExF("Segoe UI", pt, FontStyle.Bold))
                ExText(g, "LICENCE KEY", f, new Rectangle(ExS(pad), 0, exFloat.Width - ExS(pad), exFloat.Height), col,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private void ExTogglePaint(object sender, PaintEventArgs e)
        {
            Panel t = (Panel)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            float pos = (t == exRemember) ? exRemPos : exAutoPos;
            RectangleF r = new RectangleF(0.5f, 0.5f, t.Width - 2f, t.Height - 2f);

            using (GraphicsPath path = ExRoundF(r, r.Height / 2f))
            {
                using (LinearGradientBrush on = new LinearGradientBrush(r, ExSaffronD, ExSaffronL, 0f))
                using (SolidBrush off = new SolidBrush(Color.FromArgb(226, 232, 240)))
                {
                    g.FillPath(off, path);
                    if (pos > 0.01f)
                    {
                        Region old = g.Clip;
                        g.SetClip(new RectangleF(r.X, r.Y, r.Width * pos + 1, r.Height + 1), CombineMode.Intersect);
                        g.FillPath(on, path);
                        g.Clip = old;
                    }
                }
                using (Pen border = new Pen(ExMix(Color.FromArgb(203, 213, 225), ExSaffronD, pos), 1f))
                    g.DrawPath(border, path);
            }

            float knob = r.Height - ExS(5);
            float x = r.X + ExS(2.5f) + (r.Width - knob - ExS(5)) * pos;
            float y = r.Y + ExS(2.5f);
            using (SolidBrush sh = new SolidBrush(Color.FromArgb(55, 15, 23, 42)))
                g.FillEllipse(sh, x, y + ExS(1), knob, knob);
            using (SolidBrush kn = new SolidBrush(ExMix(Color.FromArgb(148, 163, 184), Color.White, pos)))
                g.FillEllipse(kn, x, y, knob, knob);
        }

        private void ExStopPaint(object sender, PaintEventArgs e)
        {
            CheckBox c = (CheckBox)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            float bs = ExS(16);
            RectangleF box = new RectangleF(ExS(1), (c.Height - bs) / 2f, bs, bs);
            using (GraphicsPath bp = ExRoundF(box, ExS(4)))
            {
                if (c.Checked)
                {
                    using (LinearGradientBrush b = new LinearGradientBrush(box, ExSaffronL, ExSaffronD, 90f)) g.FillPath(b, bp);
                }
                else
                {
                    using (SolidBrush b = new SolidBrush(exHoverStop > 0f ? Color.FromArgb(255, 247, 237) : Color.White)) g.FillPath(b, bp);
                    using (Pen p = new Pen(exHoverStop > 0f ? ExSaffron : Color.FromArgb(148, 163, 184), 1.3f * exK)) g.DrawPath(p, bp);
                }
            }
            if (c.Checked)
                ExIcon(g, "check", new RectangleF(box.X + ExS(2), box.Y + ExS(2), bs - ExS(4), bs - ExS(4)), Color.White, 2.2f * exK);

            ExText(g, c.Text, c.Font, new Rectangle((int)box.Right + ExS(8), 0, c.Width - (int)box.Right - ExS(8), c.Height),
                c.Checked ? ExSaffronD : ExInk, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private void ExSubmitPaint(object sender, PaintEventArgs e)
        {
            Control c = (Control)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            bool on = c.Enabled;
            float h = exHoverSubmit;
            int dy = exPressSubmit ? ExS(1) : 0;
            RectangleF r = new RectangleF(0.5f, 0.5f + dy, c.Width - 2f, c.Height - 2f - ExS(2));

            Color a = ExMix(ExBlueDeep, Color.FromArgb(37, 99, 235), h * 0.6f);
            Color b = ExMix(ExBlueSoft, Color.FromArgb(96, 165, 250), h * 0.5f);
            if (exPressSubmit) { a = ExMix(ExBlueDeep, Color.Black, 0.15f); b = ExBlueDeep; }
            if (!on) { a = Color.FromArgb(148, 175, 222); b = Color.FromArgb(170, 194, 232); }

            if (on && !exPressSubmit)
                using (GraphicsPath sp = ExRoundF(new RectangleF(r.X, r.Y + ExS(2), r.Width, r.Height), ExS(BTN_RADIUS)))
                using (SolidBrush sb = new SolidBrush(Color.FromArgb((int)(45 + 50 * h), 29, 78, 216)))
                    g.FillPath(sb, sp);

            using (GraphicsPath path = ExRoundF(r, ExS(BTN_RADIUS)))
            {
                using (LinearGradientBrush bg = new LinearGradientBrush(r, a, b, 0f)) g.FillPath(bg, path);
                Region old = g.Clip;
                g.SetClip(path, CombineMode.Intersect);
                using (SolidBrush gl = new SolidBrush(Color.FromArgb(28, 255, 255, 255)))
                    g.FillRectangle(gl, r.X, r.Y, r.Width, r.Height / 2f);
                // bhagwa base line on hover
                if (on && h > 0.02f)
                    using (LinearGradientBrush ob = new LinearGradientBrush(new RectangleF(r.X, r.Bottom - ExS(3), r.Width, ExS(3) + 1), ExSaffronD, ExSaffronL, 0f))
                        g.FillRectangle(ob, r.X, r.Bottom - ExS(3) * h, r.Width, ExS(3) * h);
                g.Clip = old;
            }

            using (Font f = ExF("Segoe UI Semibold", 9F))
            {
                string text = "SUBMIT";
                Size ts = TextRenderer.MeasureText(g, text, f, Size.Empty, TextFormatFlags.NoPadding);
                float isz = ExS(15);
                float gw = ts.Width + ExS(10) + isz;
                float x = r.X + (r.Width - gw) / 2f + ExS(3) * h;   // content nudges right on hover
                float cy = r.Y + r.Height / 2f;
                ExText(g, text, f, new Rectangle((int)x, (int)(cy - ts.Height / 2f) - 1, ts.Width + ExS(4), ts.Height + 2), Color.White, TextFormatFlags.Left);
                ExIcon(g, "login", new RectangleF(x + ts.Width + ExS(10), cy - isz / 2f, isz, isz), Color.White, 2.1f * exK);
            }
        }

        private void ExExitPaint(object sender, PaintEventArgs e)
        {
            Control c = (Control)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            float h = exHoverExit;
            int dy = exPressExit ? ExS(1) : 0;
            RectangleF r = new RectangleF(0.5f, 0.5f + dy, c.Width - 2f, c.Height - 2f - ExS(2));

            Color fill = exPressExit ? ExRed : ExMix(Color.White, Color.FromArgb(254, 226, 226), h);
            Color line = exPressExit ? ExRedDeep : ExMix(Color.FromArgb(252, 165, 165), ExRed, h);
            Color fg = exPressExit ? Color.White : ExMix(Color.FromArgb(220, 38, 38), Color.FromArgb(185, 28, 28), h);

            using (GraphicsPath path = ExRoundF(r, ExS(BTN_RADIUS)))
            {
                using (SolidBrush b = new SolidBrush(fill)) g.FillPath(b, path);
                using (Pen p = new Pen(line, 1.3f * exK)) g.DrawPath(p, path);
            }

            using (Font f = ExF("Segoe UI Semibold", 9F))
            {
                string text = "EXIT";
                Size ts = TextRenderer.MeasureText(g, text, f, Size.Empty, TextFormatFlags.NoPadding);
                float isz = ExS(14);
                float gw = isz + ExS(8) + ts.Width;
                float x = r.X + (r.Width - gw) / 2f;
                float cy = r.Y + r.Height / 2f;
                ExIcon(g, "power", new RectangleF(x, cy - isz / 2f, isz, isz), fg, 2.1f * exK);
                ExText(g, text, f, new Rectangle((int)(x + isz + ExS(8)), (int)(cy - ts.Height / 2f) - 1, ts.Width + ExS(4), ts.Height + 2), fg, TextFormatFlags.Left);
            }
        }

        // ------------------------------------------------------------------
        // validating overlay (frosted card over the blurred, dimmed form body)
        // ------------------------------------------------------------------
        private void ExLoaderVisibleChanged(object sender, EventArgs e)
        {
            PictureBox box = byteNovaServerLoader;
            if (box == null) return;
            if (box.Visible)
            {
                // the logic centres it; the overlay covers the whole body under the header
                int top = exHeader != null ? exHeader.Height : 0;
                box.Bounds = new Rectangle(0, top, ClientSize.Width, ClientSize.Height - top);
                ExCaptureLoaderBg(box);
                box.BringToFront();
            }
            else if (exLoaderBg != null)
            {
                exLoaderBg.Dispose();
                exLoaderBg = null;
            }
        }

        // Validation really starts once SUBMIT gets disabled; before that (e.g. empty key) the overlay can be dismissed.
        private void ExLoaderClick(object sender, EventArgs e)
        {
            PictureBox box = byteNovaServerLoader;
            if (box == null || !bunifuFlatButton1.Enabled) return;
            Point p = box.PointToClient(Cursor.Position);
            if (!ExLoaderCardRect(box).Contains(p)) box.Visible = false;
        }

        private void ExCaptureLoaderBg(PictureBox box)
        {
            try
            {
                if (exLoaderBg != null) { exLoaderBg.Dispose(); exLoaderBg = null; }
                int w = Math.Max(1, box.Width), h = Math.Max(1, box.Height);
                using (Bitmap full = new Bitmap(w, h))
                {
                    using (Graphics g = Graphics.FromImage(full)) g.Clear(ExPageBg);
                    Rectangle cr = new Rectangle(exCard.Left - box.Left, exCard.Top - box.Top, exCard.Width, exCard.Height);
                    exCard.DrawToBitmap(full, cr);

                    // cheap blur: downscale then upscale
                    int sw = Math.Max(1, w / 6), shh = Math.Max(1, h / 6);
                    using (Bitmap small = new Bitmap(sw, shh))
                    {
                        using (Graphics g = Graphics.FromImage(small))
                        {
                            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                            g.DrawImage(full, new Rectangle(0, 0, sw, shh));
                        }
                        exLoaderBg = new Bitmap(w, h);
                        using (Graphics g = Graphics.FromImage(exLoaderBg))
                        {
                            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                            g.PixelOffsetMode = PixelOffsetMode.Half;
                            g.DrawImage(small, new Rectangle(0, 0, w, h));
                        }
                    }
                }
            }
            catch
            {
                exLoaderBg = null;
            }
        }

        private Rectangle ExLoaderCardRect(Control box)
        {
            int cw = Math.Min(box.Width - ExS(28), ExS(312));
            int ch = ExS(176);
            return new Rectangle((box.Width - cw) / 2, (box.Height - ch) / 2, cw, ch);
        }

        private string ExLoaderStatusLine()
        {
            try
            {
                string all = InfoList2 != null ? InfoList2.Text : "";
                if (string.IsNullOrWhiteSpace(all)) return "";
                string[] lines = all.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                return lines.Length > 0 ? lines[lines.Length - 1].Trim().TrimEnd('.') : "";
            }
            catch { return ""; }
        }

        private void ExLoaderPaint(object sender, PaintEventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            if (exLoaderBg != null) g.DrawImageUnscaled(exLoaderBg, 0, 0);
            else g.Clear(ExPageBg);
            using (SolidBrush dim = new SolidBrush(Color.FromArgb(120, 15, 30, 62)))
                g.FillRectangle(dim, 0, 0, box.Width, box.Height);

            float time = Environment.TickCount / 1000f;
            Rectangle card = ExLoaderCardRect(box);
            RectangleF cr = new RectangleF(card.X + 0.5f, card.Y + 0.5f, card.Width - 1f, card.Height - 1f);

            // frosted card with soft shadow
            using (GraphicsPath sp = ExRoundF(new RectangleF(cr.X, cr.Y + ExS(4), cr.Width, cr.Height), ExS(18)))
            using (SolidBrush sb = new SolidBrush(Color.FromArgb(70, 2, 6, 23)))
                g.FillPath(sb, sp);
            using (GraphicsPath path = ExRoundF(cr, ExS(18)))
            {
                using (SolidBrush b = new SolidBrush(Color.FromArgb(248, 255, 255, 255))) g.FillPath(b, path);
                using (Pen p = new Pen(Color.FromArgb(220, 228, 240), 1f)) g.DrawPath(p, path);

                // top gradient strip blue -> bhagwa
                Region old = g.Clip;
                g.SetClip(path, CombineMode.Intersect);
                using (LinearGradientBrush tb = new LinearGradientBrush(new RectangleF(cr.X, cr.Y, cr.Width, ExS(4)), ExBlue, ExSaffron, 0f))
                    g.FillRectangle(tb, cr.X, cr.Y, cr.Width, ExS(4));
                g.Clip = old;
            }

            // badge with expanding "radar" pulse rings (no spinner)
            float bcx = cr.X + cr.Width / 2f, bcy = cr.Y + ExS(40);
            float br = ExS(20);
            for (int i = 0; i < 2; i++)
            {
                float ph = ((time * 0.9f) + i * 0.5f) % 1f;
                float rr = br + ExS(14) * ph;
                int alpha = (int)(110 * (1f - ph));
                using (Pen ring = new Pen(Color.FromArgb(alpha, ExSaffron), 2f * exK))
                    g.DrawEllipse(ring, bcx - rr, bcy - rr, rr * 2, rr * 2);
            }
            RectangleF bb = new RectangleF(bcx - br, bcy - br, br * 2, br * 2);
            using (LinearGradientBrush bg = new LinearGradientBrush(bb, ExBlue, ExBlueSoft, 90f))
                g.FillEllipse(bg, bb);
            ExIcon(g, "shield", new RectangleF(bcx - ExS(11), bcy - ExS(11), ExS(22), ExS(22)), Color.White, 2f * exK);

            // title + bouncing dots
            using (Font tf = ExF("Segoe UI Semibold", 12F))
            {
                string title = "Validating key";
                Size ts = TextRenderer.MeasureText(g, title, tf, Size.Empty, TextFormatFlags.NoPadding);
                float dotsW = ExS(24);
                float tx = bcx - (ts.Width + ExS(6) + dotsW) / 2f;
                float ty = cr.Y + ExS(70);
                ExText(g, title, tf, new Rectangle((int)tx, (int)ty, ts.Width + ExS(4), ts.Height + 2), Color.FromArgb(15, 42, 92), TextFormatFlags.Left);
                float dx = tx + ts.Width + ExS(6);
                float baseY = ty + ts.Height * 0.72f;
                for (int i = 0; i < 3; i++)
                {
                    float ph = (float)Math.Sin((time * 7.0) - i * 0.8);
                    float lift = Math.Max(0f, ph) * ExS(4);
                    Color dc = i == 1 ? ExSaffron : ExBlueSoft;
                    float d = ExS(5);
                    using (SolidBrush db = new SolidBrush(Color.FromArgb(150 + (int)(105 * Math.Max(0f, ph)), dc)))
                        g.FillEllipse(db, dx + i * ExS(8), baseY - lift - d / 2f, d, d);
                }
            }

            using (Font mf = ExF("Segoe UI", 8.75F))
                ExText(g, "Please wait, server se response aa raha hai", mf,
                    new Rectangle(card.X + ExS(14), card.Y + ExS(98), card.Width - ExS(28), ExS(18)),
                    ExSlate, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            string status = ExLoaderStatusLine();
            if (status.Length > 0)
                using (Font sf = ExF("Segoe UI", 7.75F))
                    ExText(g, status, sf,
                        new Rectangle(card.X + ExS(14), card.Y + ExS(118), card.Width - ExS(28), ExS(16)),
                        ExBlueSoft, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);

            // indeterminate progress: two gradient segments sliding at different speeds
            int tw = card.Width - ExS(64);
            RectangleF track = new RectangleF(card.X + (card.Width - tw) / 2f, card.Bottom - ExS(26), tw, ExS(6));
            using (GraphicsPath tp = ExRoundF(track, track.Height / 2f))
            {
                using (SolidBrush tb = new SolidBrush(Color.FromArgb(226, 232, 240))) g.FillPath(tb, tp);
                Region old = g.Clip;
                g.SetClip(tp, CombineMode.Intersect);
                float p1 = (time * 0.55f) % 1f;
                float p2 = (time * 0.55f + 0.55f) % 1f;
                ExSeg(g, track, ExEase(p1) * 1.4f - 0.35f, 0.38f);
                ExSeg(g, track, (p2 * p2) * 1.5f - 0.25f, 0.18f);
                g.Clip = old;
            }

            // overlay spans the full width: keep the window frame continuous over it
            ExPaintFrame(g, box.Location);
        }

        private static void ExSeg(Graphics g, RectangleF track, float startFrac, float lenFrac)
        {
            float x = track.X + track.Width * startFrac;
            float w = track.Width * lenFrac;
            if (x > track.Right || x + w < track.X) return;
            RectangleF s = new RectangleF(x, track.Y, w, track.Height);
            using (GraphicsPath sp = ExRoundF(s, track.Height / 2f))
            using (LinearGradientBrush sb = new LinearGradientBrush(new RectangleF(s.X - 1, s.Y, s.Width + 2, s.Height), ExBlueSoft, ExSaffron, 0f))
                g.FillPath(sb, sp);
        }

        // ------------------------------------------------------------------
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AutoScaleMode = AutoScaleMode.None;

            Size s = new Size((int)(BASE_W * exK), (int)(BASE_H * exK));
            MinimumSize = s;
            MaximumSize = s;
            ClientSize = s;
            CenterToScreen();
            ExApplyRegion();

            // frmLogin_Load (method_4) has set the real switch values by now
            ExSyncToggles();
            exFloatT = ExFloatTarget();
            ExLayoutFloat();

            exBaseRect.Clear();
            ExCaptureBase(this);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ExApplyRegion();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            ExApplyRegion();
            // re-assert the rounded region for a moment so nothing can override it
            int reassert = 0;
            Timer fix = new Timer();
            fix.Interval = 16;
            fix.Tick += (s, ev) =>
            {
                reassert++;
                ExApplyRegion();
                if (reassert >= 90)
                {
                    fix.Stop();
                    fix.Dispose();
                }
            };
            fix.Start();

            // fade in only (size never changes => no vibration)
            Opacity = 0;
            int step = 0;
            const int total = 8;
            Timer fade = new Timer();
            fade.Interval = 16;
            fade.Tick += (s, ev) =>
            {
                step++;
                Opacity = Math.Min(1.0, step / (double)total);
                if (step >= total)
                {
                    fade.Stop();
                    fade.Dispose();
                    Opacity = 1;
                    ExApplyRegion();
                }
            };
            fade.Start();

            BeginInvoke((Action)(() =>
            {
                txtLogin.Focus();
                txtLogin.Select(txtLogin.Text.Length, 0);
            }));
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (exAnim != null) { exAnim.Stop(); exAnim.Dispose(); exAnim = null; }
            if (exLoaderBg != null) { exLoaderBg.Dispose(); exLoaderBg = null; }
            base.OnFormClosed(e);
        }
    }
}
