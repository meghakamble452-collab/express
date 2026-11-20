using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace APP_FRAMEWORK
{
    public partial class frmAddID
    {
        private static readonly MethodInfo ExOnClick =
            typeof(Control).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic);

        private Panel exCard;
        private Panel exClose;
        private Timer exAnim;
        private float exHovClose;
        private bool exDownClose, exDrag;
        private int exDX, exDY;
        private readonly Dictionary<string, Font> exFonts = new Dictionary<string, Font>();

        private static readonly Color ExBlue = Color.FromArgb(41, 105, 240);
        private static readonly Color ExBlue2 = Color.FromArgb(118, 178, 252);
        private static readonly Color ExCardBg = Color.FromArgb(6, 22, 38);
        private static readonly Color ExInk = Color.FromArgb(228, 241, 252);
        private static readonly Color ExMuted = Color.FromArgb(176, 208, 230);
        private static readonly Color ExLine = Color.FromArgb(36, 78, 110);
        private static readonly Color ExGreen = Color.FromArgb(34, 197, 94);
        private static readonly Color ExRed = Color.FromArgb(220, 38, 38);
        private static readonly Color ExField = Color.FromArgb(11, 34, 53);

        private Font ExF(float px, FontStyle st, string fam = "Segoe UI")
        {
            int s = Math.Max(9, (int)Math.Round(px));
            string k = fam + "|" + s + "|" + (int)st;
            Font f;
            if (exFonts.TryGetValue(k, out f)) return f;
            f = new Font(fam, s, st, GraphicsUnit.Pixel);
            exFonts[k] = f;
            return f;
        }

        private static GraphicsPath ExRound(Rectangle r, int rad)
        {
            GraphicsPath p = new GraphicsPath();
            int d = Math.Min(rad * 2, Math.Min(r.Width, r.Height));
            if (d <= 1) { p.AddRectangle(r); return p; }
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        private static void ExRegion(Control c, int rad)
        {
            if (c.Width < 3 || c.Height < 3) return;
            using (GraphicsPath p = ExRound(new Rectangle(0, 0, c.Width, c.Height), rad))
            {
                Region old = c.Region;
                c.Region = new Region(p);
                if (old != null) old.Dispose();
            }
        }

        private void ExApplyRegion()
        {
            ExRegion(this, 18);
            if (exCard != null) ExRegion(exCard, 16);
            if (exClose != null) ExRegion(exClose, 9);
        }

        private void ApplyExpressTheme()
        {
            SuspendLayout();

            AutoScaleMode = AutoScaleMode.None;
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1180, 660);
            MinimumSize = ClientSize;
            MaximumSize = ClientSize;
            BackColor = Color.FromArgb(3, 12, 22);
            DoubleBuffered = true;

            Paint -= ExFormPaint;
            Paint += ExFormPaint;

            pnlHeader.AutoSize = false;
            pnlHeader.Dock = DockStyle.None;
            pnlHeader.BorderStyle = BorderStyle.None;
            pnlHeader.BackColor = Color.Transparent;
            pnlHeader.Paint -= pnlHeader_Paint;
            pnlHeader.Paint += ExHeaderPaint;
            pnlHeader.MouseDown += ExHeadDown;
            pnlHeader.MouseMove += ExHeadMove;
            pnlHeader.MouseUp += (s, e) => exDrag = false;

            formName.AutoSize = false;
            formName.BackColor = Color.Transparent;
            formName.ForeColor = Color.White;
            formName.Font = ExF(19f, FontStyle.Bold | FontStyle.Italic);
            formName.TextAlign = ContentAlignment.MiddleLeft;
            formName.Text = "EXPRESS    |    ADD IRCTC ID";

            btnClose.Visible = false;

            exClose = new Panel();
            exClose.BackColor = ExRed;
            exClose.Cursor = Cursors.Hand;
            exClose.Paint += ExClosePaint;
            exClose.Click += (s, e) =>
            {
                try { if (ExOnClick != null) ExOnClick.Invoke(btnClose, new object[] { EventArgs.Empty }); }
                catch { }
            };
            pnlHeader.Controls.Add(exClose);
            exClose.BringToFront();

            exCard = new Panel();
            exCard.BackColor = ExCardBg;
            exCard.Paint += ExCardPaint;
            Controls.Add(exCard);
            exCard.BringToFront();

            PnlBase.AutoSize = false;
            PnlBase.Parent = exCard;
            PnlBase.BackColor = ExCardBg;
            PnlBase.Paint -= ByteNovaBasePaint;
            PnlBase.Paint += (s, e) => { };

            tabIrctcId.BackColor = ExCardBg;
            tabHistory.BackColor = ExCardBg;

            ExThemeTree(tabIrctcId);
            ExThemeTree(tabHistory);

            CheckBox bulk = ExFindText(tabIrctcId, "Add Bulk ID");
            if (bulk != null)
            {
                bulk.CheckedChanged -= ExOnBulk;
                bulk.CheckedChanged += ExOnBulk;
            }

            pnlHeader.BringToFront();

            Resize += (s, e) => { ExLayoutAll(); ExApplyRegion(); };

            exAnim = new Timer();
            exAnim.Interval = 20;
            exAnim.Tick += ExAnimTick;
            exAnim.Start();

            ResumeLayout(true);
            PerformLayout();
            ExLayoutAll();
            ExApplyRegion();
            Invalidate();
        }

        private void ExOnBulk(object sender, EventArgs e)
        {
            BeginInvoke((Action)(() => ExLayoutAll()));
        }

        private CheckBox ExFindText(Control root, string text)
        {
            foreach (Control c in root.Controls)
            {
                CheckBox cb = c as CheckBox;
                if (cb != null && cb.Text == text) return cb;
                CheckBox r = ExFindText(c, text);
                if (r != null) return r;
            }
            return null;
        }

        // =================== layout ===================
        private void ExLayoutAll()
        {
            try
            {
                int W = ClientSize.Width;
                int H = ClientSize.Height;

                pnlHeader.Bounds = new Rectangle(0, 0, W, 58);
                formName.Bounds = new Rectangle(20, 14, W - 100, 30);
                exClose.Bounds = new Rectangle(W - 54, 14, 36, 30);

                exCard.Bounds = new Rectangle(12, 66, W - 24, H - 78);
                PnlBase.Bounds = new Rectangle(8, 8, exCard.Width - 16, exCard.Height - 16);
                tabIrctcId.Bounds = new Rectangle(0, 0, PnlBase.Width, PnlBase.Height);
                tabHistory.Bounds = new Rectangle(0, 0, PnlBase.Width, PnlBase.Height);

                ExApplyRegion();
                LayoutTabIrctcId(PnlBase.Width);
            }
            catch { }
        }

        private void LayoutTabIrctcId(int W)
        {
            GroupBox add = tabIrctcId.Controls["grpAddID"] as GroupBox;
            DataGridView grid = tabIrctcId.Controls["grdUsers"] as DataGridView;
            GroupBox search = tabIrctcId.Controls["groupBox1"] as GroupBox;

            int bulkW = 168;

            if (add != null)
            {
                add.Bounds = new Rectangle(0, 4, W, 96);

                Control userLbl = add.Controls["label37"];
                Control passLbl = add.Controls["lblPassword"];
                Control userBox = add.Controls["txtLogin"];
                Control passBox = add.Controls["txtPassword"];
                Control bulkChk = add.Controls["checkBox1"];
                Control addBtn = add.Controls["btnIDSave"];
                Control valChk = add.Controls["chkValidate"];
                Control showChk = add.Controls["chkShowPwd"];

                if (userLbl != null) userLbl.Bounds = new Rectangle(16, 30, 78, 26);
                if (userBox != null) userBox.Bounds = new Rectangle(98, 27, 210, 30);
                if (passLbl != null) passLbl.Bounds = new Rectangle(326, 30, 92, 26);
                if (passBox != null) passBox.Bounds = new Rectangle(422, 27, 210, 30);
                if (bulkChk != null) bulkChk.Bounds = new Rectangle(654, 30, 168, 24);
                if (addBtn != null) addBtn.Bounds = new Rectangle(W - 236, 24, 216, 38);

                if (valChk != null) valChk.Bounds = new Rectangle(16, 62, 336, 24);
                if (showChk != null) showChk.Bounds = new Rectangle(366, 62, 200, 24);
            }

            if (grid != null)
            {
                grid.Bounds = new Rectangle(0, 106, W - bulkW - 8, 200);
                grid.ScrollBars = ScrollBars.Both;

                int[] weights = { 120, 120, 90, 130, 90, 80, 90 };
                for (int i = 0; i < grid.Columns.Count && i < weights.Length; i++)
                    grid.Columns[i].FillWeight = weights[i];
            }

            Control bulkLbl = tabIrctcId.Controls["label6"];
            Control bulkBox = tabIrctcId.Controls["txtbulk"];
            Control pastBtn = tabIrctcId.Controls["button4"];

            if (bulkLbl != null) bulkLbl.Bounds = new Rectangle(W - bulkW, 78, bulkW - 64, 20);
            if (pastBtn != null) pastBtn.Bounds = new Rectangle(W - 62, 76, 62, 24);
            if (bulkBox != null) bulkBox.Bounds = new Rectangle(W - bulkW, 106, bulkW, 200);

            int y1 = 316;
            int y2 = 342;

            Place(tabIrctcId, "B1AAA", 0, y1, 70);
            Place(tabIrctcId, "lblTotal", 74, y1, 44);
            Place(tabIrctcId, "label10", 128, y1, 76);
            Place(tabIrctcId, "lblActive", 208, y1, 44);
            Place(tabIrctcId, "C3AA0", 262, y1, 78);
            Place(tabIrctcId, "lbl6", 344, y1, 44);
            Place(tabIrctcId, "label7", 398, y1, 80);
            Place(tabIrctcId, "lblDisableCount", 482, y1, 44);
            Place(tabIrctcId, "label9", 536, y1, 76);
            Place(tabIrctcId, "lblInValid", 616, y1, 44);

            Place(tabIrctcId, "label11", 0, y2, 96);
            Place(tabIrctcId, "lblUnVerified", 100, y2, 44);
            Place(tabIrctcId, "label1", 164, y2, 90);
            Place(tabIrctcId, "lblTobk", 258, y2, 44);

            int by = 372;
            Place(tabIrctcId, "button3", 0, by, 160);
            Place(tabIrctcId, "C1AA0", 170, by, 160);
            Place(tabIrctcId, "button1", 340, by, 200);
            Place(tabIrctcId, "button5", 550, by, 200);

            if (search != null)
            {
                search.Bounds = new Rectangle(0, 424, W, 146);

                Place(search, "RadioBdate", 16, 26, 110);
                Place(search, "B2AAA", 130, 24, 110);
                Place(search, "RadioJdate", 252, 26, 128);
                Place(search, "dtDate2", 384, 24, 110);
                Place(search, "RadioPnr", 506, 26, 72);
                Place(search, "txtPnr", 582, 24, 120);
                Place(search, "button2", 714, 24, 92);
                Place(search, "C2AA0", 814, 24, 76);

                Place(search, "label14", 16, 60, 190);
                Place(search, "CmbDays", 212, 58, 70);
                Place(search, "btnHideDay", 292, 58, 96);
                Place(search, "btnChangePwd", 400, 58, 150);
                Place(search, "btnChangeQn", 560, 58, 150);

                Control p13 = search.Controls["panel13"];
                if (p13 != null) p13.Bounds = new Rectangle(16, 96, W - 34, 42);
            }

            Control loader = tabIrctcId.Controls["pnlloader"];
            if (loader != null) loader.Bounds = new Rectangle((W - 433) / 2, 150, 433, 65);
            Control spin = tabIrctcId.Controls["pictureBox1"];
            if (spin != null) spin.Bounds = new Rectangle((W - 32) / 2, 118, 32, 32);
            Control msg = tabIrctcId.Controls["lblMsg"];
            if (msg != null) msg.Bounds = new Rectangle((W - 260) / 2, 96, 260, 20);
        }

        private static void Place(Control parent, string name, int x, int y, int w)
        {
            Control c = parent.Controls[name];
            if (c == null) return;
            int h = c.Height < 18 ? 22 : c.Height;
            c.Bounds = new Rectangle(x, y, w, h);
        }

        // =================== paint ===================
        private void ExFormPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1);

            using (GraphicsPath p = ExRound(r, 18))
            using (SolidBrush b = new SolidBrush(Color.FromArgb(3, 12, 22)))
                e.Graphics.FillPath(b, p);

            using (GraphicsPath bp = ExRound(new Rectangle(1, 1, ClientSize.Width - 3, ClientSize.Height - 3), 17))
            using (Pen pen = new Pen(Color.FromArgb(95, 165, 225), 1.5f))
                e.Graphics.DrawPath(pen, bp);
        }

        private void ExHeaderPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, pnlHeader.Width, pnlHeader.Height);
            using (GraphicsPath p = ExRound(new Rectangle(-20, 0, r.Width + 40, r.Height + 20), 18))
            using (LinearGradientBrush b = new LinearGradientBrush(r, ExBlue, ExBlue2, 0f))
                e.Graphics.FillPath(b, p);
        }

        private void ExCardPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, exCard.Width - 1, exCard.Height - 1);
            using (GraphicsPath p = ExRound(r, 16))
            {
                using (SolidBrush b = new SolidBrush(ExCardBg)) e.Graphics.FillPath(b, p);
                using (Pen pen = new Pen(ExLine, 1.2f)) e.Graphics.DrawPath(pen, p);
            }
        }

        private void ExClosePaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            int dy = exDownClose ? 2 : 0;
            Rectangle r = new Rectangle(0, dy, exClose.Width - 1, exClose.Height - 1 - dy);

            Color bg = exDownClose ? Color.FromArgb(170, 30, 26) : ExMix(ExRed, Color.White, exHovClose * 0.22f);

            using (GraphicsPath p = ExRound(r, 9))
            {
                using (SolidBrush b = new SolidBrush(bg)) e.Graphics.FillPath(b, p);
                using (Pen pen = new Pen(Color.FromArgb((int)(130 + 110 * exHovClose), 255, 255, 255), 1.4f))
                    e.Graphics.DrawPath(pen, p);
            }

            using (Pen x = new Pen(Color.White, 2.6f))
            {
                x.StartCap = LineCap.Round;
                x.EndCap = LineCap.Round;
                int cx = r.X + r.Width / 2, cy = r.Y + r.Height / 2, m = 7;
                e.Graphics.DrawLine(x, cx - m, cy - m, cx + m, cy + m);
                e.Graphics.DrawLine(x, cx + m, cy - m, cx - m, cy + m);
            }
        }

        private void ExAnimTick(object sender, EventArgs e)
        {
            if (exClose == null) return;
            Point p = exClose.PointToClient(Cursor.Position);
            bool over = exClose.Bounds.Contains(p);
            float t = over ? 1f : 0f;
            float d = t - exHovClose;
            if (Math.Abs(d) > 0.01f) { exHovClose += d * 0.22f; exClose.Invalidate(); }
            else if (exHovClose != t) { exHovClose = t; exClose.Invalidate(); }

            bool dn = over && Control.MouseButtons == MouseButtons.Left;
            if (dn != exDownClose) { exDownClose = dn; exClose.Invalidate(); }
        }

        private void ExHeadDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            exDrag = true;
            exDX = Cursor.Position.X - Left;
            exDY = Cursor.Position.Y - Top;
        }

        private void ExHeadMove(object sender, MouseEventArgs e)
        {
            if (!exDrag) return;
            Left = Cursor.Position.X - exDX;
            Top = Cursor.Position.Y - exDY;
        }

        private static Color ExMix(Color a, Color b, float t)
        {
            t = Math.Max(0f, Math.Min(1f, t));
            return Color.FromArgb(
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t));
        }

        // =================== theming ===================
        private void ExThemeTree(Control root)
        {
            foreach (Control c in root.Controls)
            {
                try
                {
                    GroupBox gb = c as GroupBox;
                    TextBox tb = c as TextBox;
                    CheckBox ck = c as CheckBox;
                    RadioButton rb = c as RadioButton;
                    Button bt = c as Button;
                    DataGridView dg = c as DataGridView;
                    Label lb = c as Label;
                    ComboBox cmb = c as ComboBox;
                    DateTimePicker dt = c as DateTimePicker;
                    RichTextBox rt = c as RichTextBox;

                    if (gb != null)
                    {
                        gb.BackColor = ExCardBg;
                        gb.ForeColor = Color.FromArgb(150, 215, 250);
                        gb.Font = ExF(12f, FontStyle.Bold);
                        gb.Paint += ExGroupPaint;
                    }
                    else if (dg != null) { ExGrid(dg); }
                    else if (tb != null)
                    {
                        tb.BorderStyle = BorderStyle.None;
                        tb.BackColor = ExField;
                        tb.ForeColor = Color.White;
                        tb.Font = ExF(14f, FontStyle.Regular);
                        ExRegion(tb, 6);
                    }
                    else if (rt != null)
                    {
                        rt.BorderStyle = BorderStyle.None;
                        rt.BackColor = ExField;
                        rt.ForeColor = Color.White;
                        rt.Font = ExF(13f, FontStyle.Regular);
                        ExRegion(rt, 8);
                    }
                    else if (ck != null)
                    {
                        ck.AutoSize = false;
                        ck.BackColor = Color.Transparent;
                        ck.ForeColor = ExInk;
                        ck.Font = ExF(14f, FontStyle.Regular);
                        ck.Size = new Size(TextRenderer.MeasureText(ck.Text, ck.Font).Width + 56, 24);
                        ck.Paint += ExCheckPaint;
                    }
                    else if (rb != null)
                    {
                        rb.AutoSize = false;
                        rb.BackColor = Color.Transparent;
                        rb.ForeColor = ExInk;
                        rb.Font = ExF(14f, FontStyle.Regular);
                        rb.Paint += ExRadioPaint;
                    }
                    else if (bt != null) { ExButton(bt); }
                    else if (cmb != null)
                    {
                        cmb.FlatStyle = FlatStyle.Flat;
                        cmb.BackColor = ExField;
                        cmb.ForeColor = Color.White;
                        cmb.Font = ExF(13f, FontStyle.Regular);
                        cmb.DrawMode = DrawMode.OwnerDrawFixed;
                        cmb.ItemHeight = 22;
                        cmb.DrawItem += ExComboDraw;
                        ExRegion(cmb, 6);
                    }
                    else if (dt != null)
                    {
                        dt.CalendarMonthBackground = ExField;
                        dt.CalendarTitleBackColor = ExBlue;
                        dt.CalendarTitleForeColor = Color.White;
                        dt.Font = ExF(13f, FontStyle.Regular);
                    }
                    else if (lb != null)
                    {
                        lb.BackColor = Color.Transparent;
                        lb.ForeColor = ExMuted;
                        lb.Font = ExF(13f, FontStyle.Regular);
                    }
                }
                catch { }

                if (c.HasChildren) ExThemeTree(c);
            }
        }

        private void ExGroupPaint(object sender, PaintEventArgs e)
        {
            GroupBox gb = sender as GroupBox;
            if (gb == null) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(1, 9, gb.Width - 3, gb.Height - 11);
            if (r.Width < 8 || r.Height < 8) return;

            using (GraphicsPath p = ExRound(r, 10))
            {
                using (SolidBrush b = new SolidBrush(Color.FromArgb(4, 17, 30))) e.Graphics.FillPath(b, p);
                using (Pen pen = new Pen(ExLine, 1.1f)) e.Graphics.DrawPath(pen, p);
            }

            SizeF ts = e.Graphics.MeasureString(gb.Text, gb.Font);
            using (SolidBrush er = new SolidBrush(ExCardBg))
                e.Graphics.FillRectangle(er, 12, 0, ts.Width + 8, 16);
            using (SolidBrush tbr = new SolidBrush(Color.FromArgb(150, 215, 250)))
                e.Graphics.DrawString(gb.Text, gb.Font, tbr, 15, 0);
        }

        private void ExCheckPaint(object sender, PaintEventArgs e)
        {
            CheckBox ck = (CheckBox)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle t = new Rectangle(ck.Width - 46, (ck.Height - 18) / 2, 40, 18);
            using (GraphicsPath p = ExRound(t, 9))
            {
                using (SolidBrush b = new SolidBrush(ck.Checked ? ExGreen : Color.FromArgb(52, 84, 110)))
                    e.Graphics.FillPath(b, p);

                int k = 14;
                int x = ck.Checked ? t.Right - k - 2 : t.X + 2;
                using (SolidBrush kb = new SolidBrush(Color.White))
                    e.Graphics.FillEllipse(kb, x, t.Y + 2, k, k);
            }

            TextRenderer.DrawText(e.Graphics, ck.Text, ck.Font,
                new Rectangle(0, 0, ck.Width - 52, ck.Height), ExInk,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private void ExRadioPaint(object sender, PaintEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int cy = rb.Height / 2;
            Rectangle dot = new Rectangle(0, cy - 8, 16, 16);

            using (Pen pen = new Pen(rb.Checked ? Color.FromArgb(120, 200, 255) : Color.FromArgb(70, 108, 138), 1.6f))
                e.Graphics.DrawEllipse(pen, dot);
            if (rb.Checked)
                using (SolidBrush b = new SolidBrush(Color.FromArgb(120, 200, 255)))
                    e.Graphics.FillEllipse(b, dot.X + 4, dot.Y + 4, 8, 8);

            TextRenderer.DrawText(e.Graphics, rb.Text, rb.Font,
                new Rectangle(22, 0, rb.Width - 22, rb.Height), ExInk,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private void ExComboDraw(object sender, DrawItemEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (e.Index < 0) return;
            bool sel = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            using (SolidBrush b = new SolidBrush(sel ? ExBlue : ExField))
                e.Graphics.FillRectangle(b, e.Bounds);
            TextRenderer.DrawText(e.Graphics, cb.Items[e.Index].ToString(), cb.Font,
                e.Bounds, Color.White, TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }

        private void ExButton(Button b)
        {
            string n = (b.Text ?? "").ToLowerInvariant();
            bool danger = n.Contains("delete") || n.Contains("clear");
            bool primary = n.Contains("add") || n.Contains("save") || n.Contains("search");

            b.UseVisualStyleBackColor = false;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = danger ? ExRed : (primary ? Color.FromArgb(130, 205, 255) : ExLine);
            b.FlatAppearance.MouseOverBackColor = danger
                ? Color.FromArgb(185, 46, 40)
                : (primary ? Color.FromArgb(34, 138, 218) : Color.FromArgb(22, 58, 86));
            b.FlatAppearance.MouseDownBackColor = danger
                ? Color.FromArgb(152, 33, 29)
                : Color.FromArgb(15, 98, 170);
            b.BackColor = danger ? ExRed : (primary ? Color.FromArgb(26, 110, 182) : Color.FromArgb(13, 42, 65));
            b.ForeColor = Color.White;
            b.Font = ExF(13f, FontStyle.Bold);
            b.Cursor = Cursors.Hand;
            ExRegion(b, 9);
        }

        private void ExGrid(DataGridView g)
        {
            g.EnableHeadersVisualStyles = false;
            g.BackgroundColor = Color.FromArgb(4, 17, 30);
            g.BorderStyle = BorderStyle.None;
            g.GridColor = Color.FromArgb(28, 62, 88);
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            g.RowHeadersVisible = false;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(12, 44, 68);
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(205, 240, 255);
            g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(12, 44, 68);
            g.ColumnHeadersDefaultCellStyle.Font = ExF(13f, FontStyle.Bold);
            g.ColumnHeadersHeight = 30;

            g.DefaultCellStyle.BackColor = Color.FromArgb(7, 25, 39);
            g.DefaultCellStyle.ForeColor = ExInk;
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(22, 96, 138);
            g.DefaultCellStyle.SelectionForeColor = Color.White;
            g.DefaultCellStyle.Font = ExF(13f, FontStyle.Regular);

            g.RowsDefaultCellStyle.BackColor = Color.FromArgb(7, 25, 39);
            g.RowsDefaultCellStyle.ForeColor = ExInk;
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(5, 20, 33);
            g.AlternatingRowsDefaultCellStyle.ForeColor = ExInk;
            g.RowTemplate.Height = 26;

            foreach (DataGridViewColumn col in g.Columns)
            {
                col.DefaultCellStyle.BackColor = Color.Empty;
                col.DefaultCellStyle.ForeColor = ExInk;
                col.DefaultCellStyle.SelectionBackColor = Color.FromArgb(22, 96, 138);
                col.DefaultCellStyle.SelectionForeColor = Color.White;

                DataGridViewLinkColumn lk = col as DataGridViewLinkColumn;
                if (lk != null)
                {
                    lk.LinkColor = Color.FromArgb(145, 222, 255);
                    lk.ActiveLinkColor = Color.White;
                    lk.VisitedLinkColor = Color.FromArgb(145, 222, 255);
                    lk.TrackVisitedState = false;
                    lk.DefaultCellStyle.ForeColor = Color.FromArgb(145, 222, 255);
                    lk.DefaultCellStyle.SelectionForeColor = Color.White;
                }
            }
            g.Invalidate();
        }
    }
}