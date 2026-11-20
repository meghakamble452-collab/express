using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace APP_FRAMEWORK
{
    public partial class frmMain
    {
        private static readonly MethodInfo ExOnClick =
            typeof(Control).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic);

        private Panel exBar;
        private Panel exCanvas;
        private Panel exLogo;
        private Label exDate;
        private Timer exTick;
        private Control exHot;
        private Control exDown;
        private int exTries;

        private const int ExBarH = 106;

        private readonly List<Control> exNav = new List<Control>();
        private readonly List<Panel> exSwitchRow = new List<Panel>();
        private readonly Dictionary<Panel, CheckBox> exSwitchMap = new Dictionary<Panel, CheckBox>();

        private static readonly Color ExInk = Color.FromArgb(15, 35, 62);
        private static readonly Color ExSoft = Color.FromArgb(74, 96, 124);
        private static readonly Color ExLine = Color.FromArgb(214, 226, 240);
        private static readonly Color ExBlue = Color.FromArgb(11, 99, 206);
        private static readonly Color ExRed = Color.FromArgb(226, 72, 60);
        private static readonly Color ExGreen = Color.FromArgb(41, 186, 76);
        private static readonly Color ExTint = Color.FromArgb(232, 241, 254);

        private static bool ExIsButton(Control c)
        {
            if (c is Button) return true;
            string n = c.GetType().Name;
            return n.Contains("FlatButton") || n.Contains("ImageButton");
        }

        private void ApplyExpressHeader()
        {
            exBar = ExFind(this, "bnCompactHeader") as Panel;
            if (exBar == null)
            {
                if (++exTries > 12) return;
                Timer t = new Timer();
                t.Interval = 150;
                t.Tick += (s, e) => { t.Stop(); t.Dispose(); ApplyExpressHeader(); };
                t.Start();
                return;
            }

            if (pnlHeader != null) pnlHeader.Visible = false;
            if (panel1 != null) panel1.Visible = false;

            int barTop = exBar.PointToScreen(Point.Empty).Y;
            foreach (Control c in AllChildren(this))
            {
                if (c is Label && !exBar.Contains(c) && c.PointToScreen(Point.Empty).Y < barTop + 22)
                    c.Visible = false;
            }

            List<Control> kids = new List<Control>();
            foreach (Control c in exBar.Controls) kids.Add(c);

            List<CheckBox> boxes = new List<CheckBox>();
            Control logo = null, resetBox = null, closeBox = null;

            foreach (Control c in kids)
            {
                if (c.Name == "bnCompactLogo") { logo = c; continue; }
                if (c.Name == "bnTopTimeBox") { continue; }
                if (c.Name == "bnTopResetBox") { resetBox = c; continue; }
                if (c.Name == "bnTopCloseBox") { closeBox = c; continue; }

                CheckBox cb = ExFindCheckBox(c);
                if (cb != null) { boxes.Add(cb); continue; }
            }

            foreach (Control c in kids) c.Visible = false;

            if (boxes.Count == 0) boxes = ExFallbackBoxes();

            exBar.Height = ExBarH;
            exBar.BackColor = Color.White;
            exBar.BringToFront();

            exCanvas = new Panel();
            exCanvas.Dock = DockStyle.Fill;
            exCanvas.BackColor = Color.White;
            exCanvas.Paint += ExCanvasPaint;
            exBar.Controls.Add(exCanvas);
            exCanvas.BringToFront();
            ExRoundRegion(exCanvas, 16);

            exLogo = new Panel();
            exLogo.Size = new Size(32, 32);
            exLogo.BackColor = Color.White;
            exLogo.Paint += ExLogoPaint;
            Image logoImg = null;
            if (logo is PictureBox pb) logoImg = pb.Image;
            if (logoImg != null) exLogo.Tag = logoImg;
            exCanvas.Controls.Add(exLogo);

            foreach (CheckBox cb in boxes)
            {
                Panel row = ExBuildSwitch(cb);
                if (row != null) exSwitchRow.Add(row);
            }

            Control userBtn = ExFindByText("USER INFO");
            if (userBtn != null)
            {
                Panel ui = ExPill("USER INFO", ExBlue, true, 96, 20);
                ui.Click += (s, e) => ExClick(userBtn);
                exCanvas.Controls.Add(ui);
            }

            exDate = new Label();
            exDate.AutoSize = false;
            exDate.Size = new Size(150, 22);
            exDate.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
            exDate.ForeColor = ExBlue;
            exDate.BackColor = Color.FromArgb(236, 244, 254);
            exDate.TextAlign = ContentAlignment.MiddleCenter;
            exDate.Paint += ExDatePaint;
            exCanvas.Controls.Add(exDate);

            Control resetHit = ExFindClickable(resetBox) ?? resetBox;
            if (resetHit != null)
            {
                Panel rb = ExPill("\u21BB", ExBlue, false, 28, 22);
                rb.Click += (s, e) => ExClick(resetHit);
                exCanvas.Controls.Add(rb);
            }

            Control closeHit = ExFindClickable(closeBox) ?? closeBox ?? btnClose;
            if (closeHit != null)
            {
                Panel cbx = ExPill("\u2715", ExRed, false, 28, 22);
                cbx.Click += (s, e) => ExClick(closeHit);
                exCanvas.Controls.Add(cbx);
            }

            string[] navKw = { "IRCTC", "BANK", "New Ticket", "OpenTicket", "Histry", "OTP", "BackUp", "Ip Block" };
            foreach (string kw in navKw)
            {
                Control src = ExFindByText(kw);
                if (src == null) continue;
                Control b = src;
                Panel p = ExPill(ExNavText(b.Text), ExBlue, false, 100, 22);
                p.Click += (s, e) => ExClick(b);
                exCanvas.Controls.Add(p);
                exNav.Add(p);
            }

            ExLayoutBar();

            exTick = new Timer();
            exTick.Interval = 20;
            exTick.Tick += ExTick;
            exTick.Start();

            exCanvas.Resize += (s, e) => { ExLayoutBar(); ExRoundRegion(exCanvas, 16); };
        }

        // ------------------------------------------------------------------
        private static Control ExFind(Control root, string name)
        {
            if (root.Name == name) return root;
            foreach (Control c in root.Controls)
            {
                Control r = ExFind(c, name);
                if (r != null) return r;
            }
            return null;
        }

        private static CheckBox ExFindCheckBox(Control root)
        {
            if (root is CheckBox cb) return cb;
            foreach (Control c in root.Controls)
            {
                CheckBox r = ExFindCheckBox(c);
                if (r != null) return r;
            }
            return null;
        }

        private static Control ExFindClickable(Control root)
        {
            if (root == null) return null;
            foreach (Control c in root.Controls)
            {
                if (ExIsButton(c)) return c;
                Control r = ExFindClickable(c);
                if (r != null) return r;
            }
            return null;
        }

        private Control ExFindByText(string keyword)
        {
            string k = keyword.Replace("&", "").Trim().ToLowerInvariant();
            foreach (Control c in AllChildren(this))
            {
                if (!ExIsButton(c)) continue;
                if (c.Text.Replace("&", "").Trim().ToLowerInvariant().Contains(k)) return c;
            }
            return null;
        }

        private static IEnumerable<Control> AllChildren(Control root)
        {
            foreach (Control c in root.Controls)
            {
                yield return c;
                foreach (Control k in AllChildren(c)) yield return k;
            }
        }

        private List<CheckBox> ExFallbackBoxes()
        {
            List<CheckBox> list = new List<CheckBox>();
            string[,] map =
            {
                { "checkBox1", "Video Mode" },
                { "checkBox9", "Beta Version" },
                { "chkADV",    "Booking Mode-1" },
                { "checkBox3", "Booking Mode-2" },
                { "checkBox5", "Booking Mode-3" }
            };
            for (int i = 0; i < map.GetLength(0); i++)
            {
                CheckBox cb = ExFind(this, map[i, 0]) as CheckBox;
                if (cb == null) continue;
                cb.Text = map[i, 1];
                list.Add(cb);
            }
            return list;
        }

        private static string ExNavText(string raw)
        {
            string t = (raw ?? "").Replace("&", "").Trim().ToUpperInvariant();
            if (t.Contains("IRCTC")) return "IRCTC ID";
            if (t.Contains("BANK")) return "BANK";
            if (t.Contains("NEW TICKET")) return "NEW TICKET";
            if (t.Contains("OPENTICKET")) return "PROCESS TICKET";
            if (t.Contains("HISTRY")) return "PROXY";
            if (t.Contains("OTP")) return "BYPASS";
            if (t.Contains("BACKUP")) return "BACKUP";
            if (t.Contains("IP BLOCK")) return "TOKEN";
            return t;
        }

        private static void ExClick(Control c)
        {
            if (c == null || ExOnClick == null) return;
            try { ExOnClick.Invoke(c, new object[] { EventArgs.Empty }); } catch { }
        }

        // ------------------------------------------------------------------
        private Panel ExBuildSwitch(CheckBox cb)
        {
            Panel row = new Panel();
            row.Size = new Size(148, 24);
            row.BackColor = Color.White;
            row.Cursor = Cursors.Hand;
            row.Tag = cb;
            row.Click += (s, e) => { cb.Checked = !cb.Checked; };
            row.Paint += ExSwitchRowPaint;
            exCanvas.Controls.Add(row);

            Panel track = new Panel();
            track.Size = new Size(30, 16);
            track.Location = new Point(114, 4);
            track.BackColor = Color.White;
            track.Cursor = Cursors.Hand;
            track.Click += (s, e) => { cb.Checked = !cb.Checked; };
            track.Paint += ExSwitchPaint;
            row.Controls.Add(track);

            cb.CheckedChanged += (s, e) => { track.Invalidate(); row.Invalidate(); };

            exSwitchMap[row] = cb;
            return row;
        }

        private Panel ExPill(string text, Color accent, bool outline, int w, int h)
        {
            Panel p = new Panel();
            p.Size = new Size(w, h);
            p.BackColor = Color.White;
            p.Cursor = Cursors.Hand;
            p.Tag = new object[] { text, accent, outline };
            p.Paint += ExPillPaint;
            return p;
        }

        // ------------------------------------------------------------------
        private void ExLayoutBar()
        {
            if (exCanvas == null) return;
            if (exBar.Height != ExBarH) exBar.Height = ExBarH;
            int W = exCanvas.Width;

            if (exLogo != null) exLogo.Bounds = new Rectangle(10, 10, 32, 32);

            for (int i = 0; i < exSwitchRow.Count; i++)
                exSwitchRow[i].Location = new Point(10 + i * 152, 52);

            if (exDate != null) exDate.Bounds = new Rectangle(W - 238, 12, 150, 22);

            foreach (Control c in exCanvas.Controls)
            {
                if (!(c.Tag is object[] t)) continue;
                string s = (string)t[0];
                if (s == "USER INFO") c.Bounds = new Rectangle(50, 36, 96, 20);
                if (s == "\u21BB") c.Bounds = new Rectangle(W - 82, 12, 28, 22);
                if (s == "\u2715") c.Bounds = new Rectangle(W - 50, 12, 28, 22);
            }

            int n = exNav.Count;
            if (n > 0)
            {
                int gap = 6;
                int avail = W - 20 - (n - 1) * gap;
                int bw = Math.Max(60, avail / n);
                int x = 10;
                foreach (Control b in exNav)
                {
                    b.Bounds = new Rectangle(x, 78, bw, 22);
                    x += bw + gap;
                }
            }
        }

        // ------------------------------------------------------------------
        private void ExTick(object sender, EventArgs e)
        {
            if (exCanvas == null || !Visible) return;
            if (exBar.Height != ExBarH) { exBar.Height = ExBarH; ExLayoutBar(); }

            if (exDate != null && lbl_Date != null && exDate.Text != lbl_Date.Text)
                exDate.Text = lbl_Date.Text;

            Point p = exCanvas.PointToClient(Cursor.Position);
            Control under = null;

            if (exCanvas.ClientRectangle.Contains(p))
            {
                foreach (Control c in exCanvas.Controls)
                {
                    if (!c.Visible || !(c.Tag is object[])) continue;
                    if (c.Bounds.Contains(p)) { under = c; break; }
                }
            }

            if (under != exHot)
            {
                Control old = exHot;
                exHot = under;
                if (old != null) old.Invalidate();
                if (under != null) under.Invalidate();
            }

            Control down = (under != null && Control.MouseButtons == MouseButtons.Left) ? under : null;
            if (down != exDown)
            {
                exDown = down;
                if (exHot != null) exHot.Invalidate();
            }
        }

        // ------------------------------------------------------------------
        private void ExCanvasPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle r = new Rectangle(0, 0, exCanvas.Width, exCanvas.Height);
            using (SolidBrush bg = new SolidBrush(Color.White))
                e.Graphics.FillRectangle(bg, r);

            Rectangle b = new Rectangle(0, 0, exCanvas.Width - 1, exCanvas.Height - 1);
            using (GraphicsPath path = ExRounded(b, 16))
            {
                using (LinearGradientBrush lb = new LinearGradientBrush(b, ExBlue, ExGreen, 20f))
                {
                    lb.InterpolationColors = new ColorBlend
                    {
                        Positions = new float[] { 0f, 0.55f, 1f },
                        Colors = new Color[] { Color.FromArgb(58, 123, 213), Color.FromArgb(90, 200, 120), Color.FromArgb(150, 120, 235) }
                    };
                    using (Pen pen = new Pen(lb, 1.8f)) e.Graphics.DrawPath(pen, path);
                }
            }

            using (Font f = new Font("Segoe UI", 12F, FontStyle.Bold | FontStyle.Italic))
                TextRenderer.DrawText(e.Graphics, "EXPRESS", f,
                    new Rectangle(50, 8, 220, 20), ExBlue,
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);

            using (Font f = new Font("Segoe UI Semibold", 5.6F, FontStyle.Bold))
                TextRenderer.DrawText(e.Graphics, "PREMIUM TATKAL SOFTWARE", f,
                    new Rectangle(51, 27, 220, 12), Color.FromArgb(130, 148, 170),
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);

            using (Pen p = new Pen(Color.FromArgb(232, 238, 246), 1f))
            {
                e.Graphics.DrawLine(p, 10, 44, exCanvas.Width - 10, 44);
                e.Graphics.DrawLine(p, 10, 80, exCanvas.Width - 10, 80);
            }
        }

        private void ExLogoPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Rectangle r = new Rectangle(0, 0, exLogo.Width, exLogo.Height);

            using (GraphicsPath p = ExRounded(r, 9))
            using (LinearGradientBrush b = new LinearGradientBrush(r, Color.FromArgb(232, 242, 255), Color.FromArgb(210, 231, 254), 90f))
                e.Graphics.FillPath(b, p);

            Image img = exLogo.Tag as Image;
            if (img != null)
            {
                int box = 24;
                float sc = Math.Min((float)box / img.Width, (float)box / img.Height);
                int w = Math.Max(1, (int)(img.Width * sc));
                int h = Math.Max(1, (int)(img.Height * sc));
                e.Graphics.DrawImage(img, new Rectangle((exLogo.Width - w) / 2, (exLogo.Height - h) / 2, w, h));
            }
            else
            {
                using (Font f = new Font("Segoe UI Black", 12F, FontStyle.Bold))
                    TextRenderer.DrawText(e.Graphics, "E", f, r, ExBlue,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }

        private void ExDatePaint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, exDate.Width - 1, exDate.Height - 1);
            using (GraphicsPath p = ExRounded(r, 8))
            {
                using (SolidBrush b = new SolidBrush(Color.FromArgb(236, 244, 254))) e.Graphics.FillPath(b, p);
                using (Pen pen = new Pen(Color.FromArgb(198, 219, 244), 1f)) e.Graphics.DrawPath(pen, p);
            }
            TextRenderer.DrawText(e.Graphics, exDate.Text, exDate.Font, r, ExBlue,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private void ExSwitchRowPaint(object sender, PaintEventArgs e)
        {
            Panel row = (Panel)sender;
            CheckBox cb;
            if (!exSwitchMap.TryGetValue(row, out cb)) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            using (Font f = new Font("Segoe UI", 6.8F))
                TextRenderer.DrawText(e.Graphics, cb.Text, f,
                    new Rectangle(0, 0, 110, row.Height), ExSoft,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }

        private void ExSwitchPaint(object sender, PaintEventArgs e)
        {
            Panel track = (Panel)sender;
            Panel row = track.Parent as Panel;
            if (row == null) return;

            CheckBox cb;
            if (!exSwitchMap.TryGetValue(row, out cb)) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, track.Width - 1, track.Height - 1);

            using (GraphicsPath p = ExRounded(r, r.Height / 2))
            {
                using (SolidBrush b = new SolidBrush(cb.Checked ? ExGreen : Color.FromArgb(205, 214, 226)))
                    e.Graphics.FillPath(b, p);

                int k = r.Height - 4;
                int x = cb.Checked ? (r.Width - k - 2) : 2;
                using (SolidBrush kb = new SolidBrush(Color.White))
                    e.Graphics.FillEllipse(kb, x, 2, k, k);
            }
        }

        private void ExPillPaint(object sender, PaintEventArgs e)
        {
            Panel p = (Panel)sender;
            object[] meta = p.Tag as object[];
            if (meta == null) return;

            string text = (string)meta[0];
            Color accent = (Color)meta[1];
            bool outline = (bool)meta[2];

            bool hot = (exHot == p);
            bool dn = (exDown == p);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            Rectangle r = new Rectangle(0, dn ? 1 : 0, p.Width - 1, p.Height - 1);

            Color bg = dn ? accent : (hot ? ExTint : Color.White);
            Color fg = dn ? Color.White : (hot || outline ? accent : ExInk);

            using (GraphicsPath path = ExRounded(r, 7))
            {
                using (SolidBrush b = new SolidBrush(bg)) e.Graphics.FillPath(b, path);
                using (Pen pen = new Pen(hot || dn || outline ? accent : ExLine, 1f))
                    e.Graphics.DrawPath(pen, path);
            }

            using (Font f = new Font("Segoe UI Semibold", text.Length <= 2 ? 10F : 7.5F, FontStyle.Bold))
                TextRenderer.DrawText(e.Graphics, text, f, r, fg,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        private static GraphicsPath ExRounded(Rectangle r, int radius)
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

        private static void ExRoundRegion(Control c, int radius)
        {
            if (c.Width < 2 || c.Height < 2) return;
            using (GraphicsPath p = ExRounded(new Rectangle(0, 0, c.Width, c.Height), radius))
            {
                Region old = c.Region;
                c.Region = new Region(p);
                if (old != null) old.Dispose();
            }
        }
    }
}