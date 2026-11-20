using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace APP_FRAMEWORK
{
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
        private Panel exLine;
        private Label exTitle;
        private Label exSub;
        private Label exVersion;
        private Label exKeyCap;
        private Label exPlaceholder;
        private Label exRememberCap;
        private Label exAutoCap;
        private Label exHint;

        private Timer exAnim;
        private float exHoverSubmit, exHoverExit, exHoverClose;
        private bool exPressSubmit, exPressExit, exPressClose;
        private float exRemPos = 1f, exAutoPos = 1f;

        private float exK = 1f;
        private bool exDrag;
        private int exDragX, exDragY;

        private const int BASE_W = 540;
        private const int BASE_H = 470;
        private const int CORNER = 20;
        private const int BTN_RADIUS = 14;

        private static readonly Color ExBtnBlue = Color.FromArgb(11, 99, 206);
        private static readonly Color ExBtnBlueDown = Color.FromArgb(9, 78, 165);
        private static readonly Color ExBtnRed = Color.FromArgb(226, 72, 60);
        private static readonly Color ExBtnRedDown = Color.FromArgb(188, 48, 38);
        private static readonly Color ExCloseBase = Color.FromArgb(226, 62, 48);
        private static readonly Color ExCloseDown = Color.FromArgb(195, 58, 44);
        private static readonly Color ExFieldBg = Color.FromArgb(250, 252, 255);

        private readonly Dictionary<Control, Rectangle> exBaseRect = new Dictionary<Control, Rectangle>();

        private void ApplyExpressTheme()
        {
            SuspendLayout();

            using (Graphics g = CreateGraphics()) exK = g.DpiX / 96f;
            if (exK < 1f) exK = 1f;

            AutoScaleMode = AutoScaleMode.None;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(BASE_W, BASE_H);
            BackColor = UiTheme.Bg;
            ForeColor = UiTheme.Text;
            Font = UiTheme.FBase;
            DoubleBuffered = true;

            // Bunifu apna region laga ke hamare rounded corners ko square kar deta hai
            try { bunifuElipse1.TargetControl = null; } catch { }
            bunifuElipse1.ElipseRadius = (int)(CORNER * exK);

            Paint -= ExFormPaint;
            Paint += ExFormPaint;

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

            exHeader = new Panel();
            exHeader.Location = new Point(0, 0);
            exHeader.Size = new Size(BASE_W, 66);
            exHeader.BackColor = UiTheme.HeaderA;
            exHeader.Paint += ExHeaderPaint;
            exHeader.MouseDown += ExHeaderDown;
            exHeader.MouseMove += ExHeaderMove;
            exHeader.MouseUp += (s, e) => exDrag = false;
            exHeader.Click += (s, e) => ActiveControl = null;
            Controls.Add(exHeader);
            exHeader.BringToFront();

            exClose = new Panel();
            exClose.Size = new Size(36, 32);
            exClose.Location = new Point(BASE_W - 50, 17);
            exClose.BackColor = Color.Transparent;
            exClose.Cursor = Cursors.Hand;
            exClose.Paint += ExClosePaint;
            exClose.Click += (s, e) => Close();
            exHeader.Controls.Add(exClose);
            exClose.BringToFront();

            exCard = new Panel();
            exCard.Location = new Point(20, 78);
            exCard.Size = new Size(500, 380);
            exCard.BackColor = Color.White;
            exCard.Paint += ExCardPaint;
            exCard.Click += (s, e) => ActiveControl = null;
            Controls.Add(exCard);
            exCard.BringToFront();
            ExRoundControl(exCard, 18);

            exBrand = new Panel();
            exBrand.Location = new Point(18, 18);
            exBrand.Size = new Size(56, 56);
            exBrand.BackColor = Color.Transparent;
            exBrand.Paint += ExBrandPaint;
            exBrand.Click += (s, e) => ActiveControl = null;
            exCard.Controls.Add(exBrand);

            exTitle = ExLabel("EXPRESS", 88, 6, new Font("Segoe UI", 20F, FontStyle.Bold | FontStyle.Italic), UiTheme.HeaderA);
            exTitle.AutoSize = false;
            exTitle.Size = new Size(260, 42);
            exTitle.TextAlign = ContentAlignment.MiddleLeft;
            exSub = ExLabel("PREMIUM TATKAL SOFTWARE", 90, 48, new Font("Segoe UI Semibold", 7.5F, FontStyle.Bold), Color.FromArgb(112, 133, 160));

            exVersion = ExLabel("v" + appversion + "  •  SECURE", 332, 18, new Font("Segoe UI", 7.5F), Color.FromArgb(140, 158, 182));
            exVersion.AutoSize = false;
            exVersion.Width = 150;
            exVersion.Height = 14;
            exVersion.TextAlign = ContentAlignment.TopRight;

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

            exLine = new Panel();
            exLine.Location = new Point(18, 92);
            exLine.Size = new Size(464, 1);
            exLine.BackColor = UiTheme.Border;
            exLine.Visible = false;
            exCard.Controls.Add(exLine);

            exKeyCap = ExLabel("LICENCE KEY", 18, 102, new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold), Color.FromArgb(92, 113, 141));

            panel2.Parent = exCard;
            panel2.Location = new Point(18, 122);
            panel2.Size = new Size(464, 52);
            panel2.BackColor = Color.White;
            panel2.Paint -= ExFieldPaint;
            panel2.Paint += ExFieldPaint;
            panel2.Click += (s, e) => txtLogin.Focus();
            ExRoundControl(panel2, 12);

            txtLogin.Parent = panel2;
            txtLogin.Location = new Point(14, 14);
            txtLogin.Size = new Size(436, 24);
            txtLogin.BorderStyle = BorderStyle.None;
            txtLogin.BackColor = ExFieldBg;
            txtLogin.ForeColor = UiTheme.Text;
            txtLogin.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
            txtLogin.TextAlign = HorizontalAlignment.Left;
            txtLogin.HideSelection = false;
            txtLogin.Enter += (s, e) => { ExUpdatePlaceholder(); panel2.Invalidate(); };
            txtLogin.Leave += (s, e) => { ExUpdatePlaceholder(); panel2.Invalidate(); };
            txtLogin.TextChanged += (s, e) => ExUpdatePlaceholder();

            exPlaceholder = new Label();
            exPlaceholder.AutoSize = false;
            exPlaceholder.Size = new Size(320, 22);
            exPlaceholder.Location = new Point(32, 137);
            exPlaceholder.Text = "Enter your licence key";
            exPlaceholder.Font = new Font("Segoe UI Semibold", 12F);
            exPlaceholder.ForeColor = Color.FromArgb(158, 173, 192);
            exPlaceholder.BackColor = ExFieldBg;
            exPlaceholder.Cursor = Cursors.IBeam;
            exPlaceholder.UseCompatibleTextRendering = true;
            exPlaceholder.TextAlign = ContentAlignment.MiddleLeft;
            exPlaceholder.Click += (s, e) => txtLogin.Focus();
            exCard.Controls.Add(exPlaceholder);

            exRemember = ExToggle(new Point(18, 190), true, ExRememberToggle);
            exRememberCap = ExLabel("Remember My Key", 74, 191, new Font("Segoe UI", 9.5F), Color.FromArgb(62, 84, 112));
            exRememberCap.Cursor = Cursors.Hand;
            exRememberCap.Click += ExRememberToggle;

            exAuto = ExToggle(new Point(18, 220), true, ExAutoToggle);
            exAutoCap = ExLabel("Auto Login", 74, 221, new Font("Segoe UI", 9.5F), Color.FromArgb(62, 84, 112));
            exAutoCap.Cursor = Cursors.Hand;
            exAutoCap.Click += ExAutoToggle;

            byteNovaRestartOption.Parent = exCard;
            byteNovaRestartOption.Location = new Point(18, 252);
            byteNovaRestartOption.ForeColor = Color.FromArgb(74, 96, 124);
            byteNovaRestartOption.Font = new Font("Segoe UI", 9F);
            byteNovaRestartOption.BackColor = Color.Transparent;
            byteNovaRestartOption.BringToFront();

            bunifuFlatButton2.Parent = exCard;
            bunifuFlatButton2.Location = new Point(18, 288);
            bunifuFlatButton2.Size = new Size(150, 48);
            bunifuFlatButton2.BorderRadius = BTN_RADIUS;
            bunifuFlatButton2.BackColor = Color.Transparent;
            bunifuFlatButton2.Normalcolor = ExBtnRed;
            bunifuFlatButton2.Activecolor = ExBtnRed;
            bunifuFlatButton2.OnHovercolor = ExBtnRed;
            bunifuFlatButton2.Textcolor = Color.White;
            bunifuFlatButton2.OnHoverTextColor = Color.White;
            bunifuFlatButton2.Cursor = Cursors.Hand;
            bunifuFlatButton2.Text = "";
            bunifuFlatButton2.Paint += ExExitPaint;
            ExRoundControl(bunifuFlatButton2, BTN_RADIUS);

            bunifuFlatButton1.Parent = exCard;
            bunifuFlatButton1.Location = new Point(178, 288);
            bunifuFlatButton1.Size = new Size(300, 48);
            bunifuFlatButton1.BorderRadius = BTN_RADIUS;
            bunifuFlatButton1.BackColor = Color.Transparent;
            bunifuFlatButton1.Normalcolor = ExBtnBlue;
            bunifuFlatButton1.Activecolor = ExBtnBlue;
            bunifuFlatButton1.OnHovercolor = ExBtnBlue;
            bunifuFlatButton1.Textcolor = Color.White;
            bunifuFlatButton1.OnHoverTextColor = Color.White;
            bunifuFlatButton1.Cursor = Cursors.Hand;
            bunifuFlatButton1.Text = "";
            bunifuFlatButton1.Paint += ExSubmitPaint;
            ExRoundControl(bunifuFlatButton1, BTN_RADIUS);

            exHint = ExLabel("Server se validate hone ke baad software khulega.", 18, 344, new Font("Segoe UI", 8.5F), Color.FromArgb(122, 142, 166));

            if (byteNovaServerLoader != null)
            {
                byteNovaServerLoader.Paint -= ByteNovaServerLoaderPaint;
                byteNovaServerLoader.Paint += ExLoaderPaint;
                byteNovaServerLoader.BackColor = Color.White;
                byteNovaServerLoader.Size = new Size(330, 152);
            }

            if (exK > 1f)
            {
                ExScaleTree(this, exK);
                ClientSize = new Size((int)(BASE_W * exK), (int)(BASE_H * exK));
            }
            MinimumSize = ClientSize;
            MaximumSize = ClientSize;

            ExApplyRegion();
            ExRoundControl(panel2, 12);
            ExRoundControl(bunifuFlatButton1, BTN_RADIUS);
            ExRoundControl(bunifuFlatButton2, BTN_RADIUS);

            if (byteNovaServerLoader != null)
                byteNovaServerLoader.Location = new Point(
                    (ClientSize.Width - byteNovaServerLoader.Width) / 2,
                    (ClientSize.Height - byteNovaServerLoader.Height) / 2);

            exAnim = new Timer();
            exAnim.Interval = 16;
            exAnim.Tick += ExAnimTick;
            exAnim.Start();

            exRemPos = bunifuToggleRememberSwitch.Value ? 1f : 0f;
            exAutoPos = bunifuToggleAutoLoginSwitch.Value ? 1f : 0f;
            exRemember.Tag = bunifuToggleRememberSwitch.Value;
            exAuto.Tag = bunifuToggleAutoLoginSwitch.Value;

            exPlaceholder.BringToFront();
            ExUpdatePlaceholder();

            ResumeLayout(true);
            PerformLayout();
            Invalidate();
        }

        // ------------------------------------------------------------------
        private void ExUpdatePlaceholder()
        {
            if (exPlaceholder == null) return;
            exPlaceholder.Visible = txtLogin.Text.Length == 0 && !txtLogin.Focused;
        }

        private static GraphicsPath ExRound(Rectangle r, int radius)
        {
            GraphicsPath p = new GraphicsPath();
            int d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
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

            int d = (int)(CORNER * 2 * exK);
            IntPtr h = CreateRoundRectRgn(0, 0, Width + 1, Height + 1, d, d);
            SetWindowRgn(Handle, h, true);
        }

        private void ExScaleTree(Control parent, float k)
        {
            parent.SuspendLayout();
            foreach (Control c in parent.Controls)
            {
                c.Location = new Point((int)Math.Round(c.Left * k), (int)Math.Round(c.Top * k));
                if (!c.AutoSize)
                    c.Size = new Size((int)Math.Round(c.Width * k), (int)Math.Round(c.Height * k));
                try { c.Font = new Font(c.Font.FontFamily, Math.Max(6f, c.Font.Size * k), c.Font.Style); } catch { }
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

        // sirf bounds scale hote hain, fonts nahi - isliye koi vibration nahi
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
            l.UseCompatibleTextRendering = true;
            l.Click += (s, e) => ActiveControl = null;
            exCard.Controls.Add(l);
            l.BringToFront();
            return l;
        }

        private Panel ExToggle(Point p, bool state, EventHandler onClick)
        {
            Panel t = new Panel();
            t.Location = p;
            t.Size = new Size(46, 24);
            t.BackColor = Color.Transparent;
            t.Cursor = Cursors.Hand;
            t.Tag = state;
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
            if (!Visible) return;

            Point pc = Cursor.Position;
            Point inCard = exCard.PointToClient(pc);
            Point inHead = exHeader.PointToClient(pc);
            bool down = Control.MouseButtons == MouseButtons.Left;

            bool overSubmit = bunifuFlatButton1.Enabled && bunifuFlatButton1.Bounds.Contains(inCard);
            bool overExit = bunifuFlatButton2.Bounds.Contains(inCard);
            bool overClose = exClose.Bounds.Contains(inHead);

            float ns = ExApproach(exHoverSubmit, overSubmit ? 1f : 0f);
            bool ps = overSubmit && down;

            if (ns != exHoverSubmit || ps != exPressSubmit)
            {
                exHoverSubmit = ns;
                exPressSubmit = ps;
                bunifuFlatButton1.Normalcolor = ps ? ExBtnBlueDown : ExMix(ExBtnBlue, Color.White, ns * 0.9f);
                bunifuFlatButton1.Activecolor = bunifuFlatButton1.Normalcolor;
                bunifuFlatButton1.Invalidate();
            }

            float ne = ExApproach(exHoverExit, overExit ? 1f : 0f);
            bool pe = overExit && down;

            if (ne != exHoverExit || pe != exPressExit)
            {
                exHoverExit = ne;
                exPressExit = pe;
                bunifuFlatButton2.Normalcolor = pe ? ExBtnRedDown : ExMix(ExBtnRed, Color.White, ne * 0.9f);
                bunifuFlatButton2.Activecolor = bunifuFlatButton2.Normalcolor;
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

            bool remOn = (bool)exRemember.Tag;
            bool autoOn = (bool)exAuto.Tag;

            float nr = ExApproach(exRemPos, remOn ? 1f : 0f);
            if (nr != exRemPos) { exRemPos = nr; exRemember.Invalidate(); }

            float na = ExApproach(exAutoPos, autoOn ? 1f : 0f);
            if (na != exAutoPos) { exAutoPos = na; exAuto.Invalidate(); }
        }

        private static Color ExMix(Color a, Color b, float t)
        {
            t = Math.Max(0f, Math.Min(1f, t));
            return Color.FromArgb(
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t));
        }

        private static float ExS_DPI = 1f;

        private static int ExS(float v)
        {
            return (int)Math.Round(v * ExS_DPI);
        }

        // ------------------------------------------------------------------
        private void ExFormPaint(object sender, PaintEventArgs e)
        {
            ExS_DPI = exK;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            using (SolidBrush bg = new SolidBrush(UiTheme.Bg))
                e.Graphics.FillRectangle(bg, new Rectangle(0, 0, ClientSize.Width, ClientSize.Height));

            Rectangle b = new Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            using (GraphicsPath path = ExRound(b, ExS(CORNER)))
            {
                using (LinearGradientBrush lb = new LinearGradientBrush(b, UiTheme.HeaderA, UiTheme.Green, 20f))
                {
                    lb.InterpolationColors = new ColorBlend
                    {
                        Positions = new float[] { 0f, 0.55f, 1f },
                        Colors = new Color[] { Color.FromArgb(58, 123, 213), Color.FromArgb(90, 200, 120), Color.FromArgb(150, 120, 235) }
                    };
                    using (Pen pen = new Pen(lb, 2.2f * exK))
                        e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private void ExHeaderPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle r = new Rectangle(0, 0, exHeader.Width, exHeader.Height);
            using (GraphicsPath path = ExRound(new Rectangle(-ExS(CORNER), 0, r.Width + ExS(CORNER) * 2, r.Height + ExS(CORNER)), ExS(CORNER)))
            using (LinearGradientBrush bg = new LinearGradientBrush(r, UiTheme.HeaderA, UiTheme.HeaderB, 0f))
                e.Graphics.FillPath(bg, path);

            using (Font f = new Font("Segoe UI", 13F * exK, FontStyle.Bold | FontStyle.Italic))
                TextRenderer.DrawText(e.Graphics, "EXPRESS   |   KEY LOGIN", f,
                    new Point(ExS(20), ExS(21)), Color.White,
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
        }

        private void ExClosePaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int dy = exPressClose ? ExS(1) : 0;
            Rectangle r = new Rectangle(0, dy, exClose.Width - 1, exClose.Height - 1 - dy);

            Color bgCol = exPressClose
                ? ExCloseDown
                : ExMix(ExCloseBase, Color.White, exHoverClose * 0.92f);
            Color xCol = exPressClose
                ? Color.White
                : ExMix(Color.White, ExCloseDown, exHoverClose);

            using (GraphicsPath path = ExRound(r, ExS(9)))
            {
                using (SolidBrush bg = new SolidBrush(bgCol))
                    e.Graphics.FillPath(bg, path);

                if (exHoverClose > 0.05f)
                {
                    using (Pen border = new Pen(Color.FromArgb((int)(170 * exHoverClose), ExCloseBase), 1.3f * exK))
                        e.Graphics.DrawPath(border, path);
                }
            }

            using (Pen p = new Pen(xCol, 2.2f * exK))
            {
                p.StartCap = LineCap.Round;
                p.EndCap = LineCap.Round;
                int m = ExS(11);
                e.Graphics.DrawLine(p, m, ExS(10) + dy, exClose.Width - m, exClose.Height - ExS(10) + dy);
                e.Graphics.DrawLine(p, exClose.Width - m, ExS(10) + dy, m, exClose.Height - ExS(10) + dy);
            }
        }

        private void ExCardPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, exCard.Width - 1, exCard.Height - 1);
            using (GraphicsPath path = ExRound(r, ExS(18)))
            using (Pen pen = new Pen(UiTheme.Border, 1f * exK))
                e.Graphics.DrawPath(pen, path);
        }

        private void ExBrandPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Rectangle r = new Rectangle(0, 0, exBrand.Width, exBrand.Height);

            using (GraphicsPath path = ExRound(r, ExS(14)))
            using (LinearGradientBrush bg = new LinearGradientBrush(r, Color.FromArgb(232, 242, 255), Color.FromArgb(212, 232, 254), 90f))
                e.Graphics.FillPath(bg, path);

            if (byteNovaLogoImage != null)
            {
                int box = ExS(46);
                float scale = Math.Min((float)box / byteNovaLogoImage.Width, (float)box / byteNovaLogoImage.Height);
                int w = Math.Max(1, (int)(byteNovaLogoImage.Width * scale));
                int h = Math.Max(1, (int)(byteNovaLogoImage.Height * scale));
                e.Graphics.DrawImage(byteNovaLogoImage, new Rectangle((exBrand.Width - w) / 2, (exBrand.Height - h) / 2, w, h));
            }
            else
            {
                using (Font f = new Font("Segoe UI Black", 22F * exK, FontStyle.Bold))
                    TextRenderer.DrawText(e.Graphics, "E", f, new Rectangle(0, 0, exBrand.Width, exBrand.Height),
                        UiTheme.HeaderA, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private void ExFieldPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, panel2.Width - 1, panel2.Height - 1);
            bool focused = txtLogin.Focused;

            using (GraphicsPath path = ExRound(r, ExS(12)))
            {
                using (SolidBrush bg = new SolidBrush(ExFieldBg))
                    e.Graphics.FillPath(bg, path);

                if (focused)
                {
                    using (Pen glow = new Pen(Color.FromArgb(45, UiTheme.HeaderB), 5f * exK))
                        e.Graphics.DrawPath(glow, path);
                }
                using (Pen pen = new Pen(focused ? UiTheme.HeaderB : UiTheme.Border, 1.4f * exK))
                    e.Graphics.DrawPath(pen, path);
            }
        }

        private void ExTogglePaint(object sender, PaintEventArgs e)
        {
            Panel t = (Panel)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            float pos = (t == exRemember) ? exRemPos : exAutoPos;
            Rectangle r = new Rectangle(0, 0, t.Width - 1, t.Height - 1);

            using (GraphicsPath path = ExRound(r, t.Height / 2))
            {
                using (SolidBrush bg = new SolidBrush(ExMix(Color.FromArgb(203, 212, 224), UiTheme.Green, pos)))
                    e.Graphics.FillPath(bg, path);

                int knob = t.Height - ExS(6);
                int x = ExS(3) + (int)((t.Width - knob - ExS(6)) * pos);
                using (SolidBrush kn = new SolidBrush(Color.White))
                    e.Graphics.FillEllipse(kn, x, ExS(3), knob, knob);
            }
        }

        private void ExSubmitPaint(object sender, PaintEventArgs e)
        {
            Control c = (Control)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            float h = exHoverSubmit;
            int dy = exPressSubmit ? ExS(2) : 0;
            Rectangle r = new Rectangle(0, dy, c.Width - 1, c.Height - 1 - dy);

            Color top, bottom, fg;
            if (exPressSubmit)
            {
                top = bottom = ExBtnBlueDown;
                fg = Color.White;
            }
            else
            {
                top = ExMix(ExBtnBlue, Color.White, h * 0.9f);
                bottom = ExMix(UiTheme.HeaderB, Color.White, h * 0.9f);
                fg = ExMix(Color.White, ExBtnBlueDown, h);
            }

            using (GraphicsPath path = ExRound(r, ExS(BTN_RADIUS)))
            {
                using (LinearGradientBrush bg = new LinearGradientBrush(r, top, bottom, 0f))
                    e.Graphics.FillPath(bg, path);

                if (h > 0.04f && !exPressSubmit)
                {
                    using (Pen border = new Pen(Color.FromArgb((int)(190 * h), ExBtnBlue), 1.2f * exK))
                        e.Graphics.DrawPath(border, path);
                }

                using (Font f = new Font("Segoe UI Semibold", 10.5F * exK, FontStyle.Bold))
                    TextRenderer.DrawText(e.Graphics, "SUBMIT", f, r, fg,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private void ExExitPaint(object sender, PaintEventArgs e)
        {
            Control c = (Control)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            float h = exHoverExit;
            int dy = exPressExit ? ExS(2) : 0;
            Rectangle r = new Rectangle(0, dy, c.Width - 1, c.Height - 1 - dy);

            Color bgCol, fg;
            if (exPressExit)
            {
                bgCol = ExBtnRedDown;
                fg = Color.White;
            }
            else
            {
                bgCol = ExMix(ExBtnRed, Color.White, h * 0.9f);
                fg = ExMix(Color.White, ExBtnRedDown, h);
            }

            using (GraphicsPath path = ExRound(r, ExS(BTN_RADIUS)))
            {
                using (SolidBrush bg = new SolidBrush(bgCol))
                    e.Graphics.FillPath(bg, path);

                if (h > 0.04f && !exPressExit)
                {
                    using (Pen border = new Pen(Color.FromArgb((int)(190 * h), ExBtnRed), 1.2f * exK))
                        e.Graphics.DrawPath(border, path);
                }

                using (Font f = new Font("Segoe UI Semibold", 10.5F * exK, FontStyle.Bold))
                    TextRenderer.DrawText(e.Graphics, "EXIT", f, r, fg,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private void ExLoaderPaint(object sender, PaintEventArgs e)
        {
            PictureBox box = (PictureBox)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            Rectangle r = new Rectangle(0, 0, box.Width - 1, box.Height - 1);
            using (GraphicsPath path = ExRound(r, ExS(16)))
            {
                using (SolidBrush bg = new SolidBrush(Color.White))
                    e.Graphics.FillPath(bg, path);

                using (LinearGradientBrush lb = new LinearGradientBrush(r, UiTheme.HeaderA, UiTheme.Green, 20f))
                {
                    lb.InterpolationColors = new ColorBlend
                    {
                        Positions = new float[] { 0f, 0.55f, 1f },
                        Colors = new Color[] { Color.FromArgb(58, 123, 213), Color.FromArgb(90, 200, 120), Color.FromArgb(150, 120, 235) }
                    };
                    using (Pen pen = new Pen(lb, 2f * exK))
                        e.Graphics.DrawPath(pen, path);
                }
            }

            int bs = ExS(36);
            Rectangle badge = new Rectangle((box.Width - bs) / 2, ExS(16), bs, bs);
            using (GraphicsPath bp = ExRound(badge, ExS(10)))
            {
                using (LinearGradientBrush bg = new LinearGradientBrush(badge, Color.FromArgb(232, 242, 255), Color.FromArgb(212, 232, 254), 90f))
                    e.Graphics.FillPath(bg, bp);

                if (byteNovaLogoImage != null)
                {
                    int inner = bs - ExS(10);
                    float sc = Math.Min((float)inner / byteNovaLogoImage.Width, (float)inner / byteNovaLogoImage.Height);
                    int w = Math.Max(1, (int)(byteNovaLogoImage.Width * sc));
                    int h = Math.Max(1, (int)(byteNovaLogoImage.Height * sc));
                    e.Graphics.DrawImage(byteNovaLogoImage,
                        new Rectangle(badge.X + (bs - w) / 2, badge.Y + (bs - h) / 2, w, h));
                }
                else
                {
                    using (Font f = new Font("Segoe UI Black", 13F * exK, FontStyle.Bold))
                        TextRenderer.DrawText(e.Graphics, "E", f, badge, UiTheme.HeaderA,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }

            using (Font f = new Font("Segoe UI Semibold", 10F * exK, FontStyle.Bold))
                TextRenderer.DrawText(e.Graphics, "VALIDATING KEY", f,
                    new Rectangle(0, ExS(58), box.Width, ExS(18)), UiTheme.HeaderA,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            using (Font f = new Font("Segoe UI", 8F * exK))
                TextRenderer.DrawText(e.Graphics, "Please wait, server se response aa raha hai", f,
                    new Rectangle(0, ExS(78), box.Width, ExS(16)), Color.FromArgb(130, 148, 170),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            int tw = ExS(220);
            int th = ExS(6); bunifuElipse1.ElipseRadius = (int)(CORNER * exK);
            Rectangle track = new Rectangle((box.Width - tw) / 2, ExS(108), tw, th);

            using (GraphicsPath tp = ExRound(track, th / 2))
            using (SolidBrush tb = new SolidBrush(Color.FromArgb(228, 236, 246)))
                e.Graphics.FillPath(tb, tp);

            int sw = ExS(80);
            float pos = byteNovaServerLoaderStep / 11f;
            int sx = track.X + (int)((track.Width + sw) * pos) - sw;
            Rectangle seg = new Rectangle(sx, track.Y, sw, th);

            Region oldClip = e.Graphics.Clip;
            e.Graphics.SetClip(track);
            using (GraphicsPath sp = ExRound(seg, Math.Max(2, th / 2)))
            using (LinearGradientBrush sb = new LinearGradientBrush(seg, UiTheme.HeaderA, UiTheme.HeaderB, 0f))
                e.Graphics.FillPath(sb, sp);
            e.Graphics.Clip = oldClip;
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
            // kuch der tak region dobara lagate raho, taaki koi override na kar sake
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
            ExUpdatePlaceholder();

            // ---- sirf fade, size bilkul change nahi => koi vibration nahi ----
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
                    ExRoundControl(exCard, 18);
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
            if (exAnim != null) exAnim.Stop();
            base.OnFormClosed(e);
        }
    }
}