using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace APP_FRAMEWORK
{
    internal partial class frmBanks
    {
        private static readonly Color BKbg = Color.FromArgb(233, 241, 252);
        private static readonly Color BKcard = Color.White;
        private static readonly Color BKline = Color.FromArgb(198, 216, 242);
        private static readonly Color BKink = Color.FromArgb(17, 40, 72);
        private static readonly Color BKmuted = Color.FromArgb(104, 132, 168);
        private static readonly Color BKorange = Color.FromArgb(245, 146, 28);
        private static readonly Color BKhead = Color.FromArgb(12, 38, 72);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private Panel _bkHead;
        private bool _bkDone;

        private void ApplyBankTheme()
        {
            if (_bkDone) return;
            _bkDone = true;

            SuspendLayout();
            try
            {
                DoubleBuffered = true;
                BackColor = BKbg;
                FormBorderStyle = FormBorderStyle.None;
                StartPosition = FormStartPosition.CenterParent;
                Font = new Font("Segoe UI", 9f);

                foreach (Control c in Controls)
                    if (c.Top < 130) c.Top += 44;

                ClientSize = new Size(ClientSize.Width, ClientSize.Height + 44);

                _bkHead = new Panel();
                _bkHead.BackColor = BKhead;
                _bkHead.Bounds = new Rectangle(0, 0, ClientSize.Width, 42);
                _bkHead.Paint += BkHeadPaint;
                _bkHead.MouseDown += BkHeadDown;
                Controls.Add(_bkHead);
                _bkHead.BringToFront();

                Button x = new Button();
                x.Text = "";
                x.FlatStyle = FlatStyle.Flat;
                x.FlatAppearance.BorderSize = 0;
                x.BackColor = Color.FromArgb(220, 38, 38);
                x.Size = new Size(34, 26);
                x.Location = new Point(ClientSize.Width - 44, 8);
                x.Cursor = Cursors.Hand;
                x.Paint += BkXPaint;
                x.Click += delegate { Close(); };
                _bkHead.Controls.Add(x);

                BkDeep(this);
            }
            finally
            {
                ResumeLayout(false);
                PerformLayout();
            }

            BkRegion();
        }

        private void BkHeadDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }

        private void BkHeadPaint(object sender, PaintEventArgs e)
        {
            Rectangle r = new Rectangle(0, 0, _bkHead.Width, _bkHead.Height);
            using (LinearGradientBrush b = new LinearGradientBrush(r, Color.FromArgb(8, 26, 52), Color.FromArgb(16, 58, 104), 0f))
                e.Graphics.FillRectangle(b, r);

            using (SolidBrush lime = new SolidBrush(Color.FromArgb(198, 244, 50)))
                e.Graphics.FillRectangle(lime, 0, _bkHead.Height - 2, _bkHead.Width, 2);

            using (SolidBrush ink = new SolidBrush(Color.White))
            using (Font f = new Font("Segoe UI Semibold", 11f))
                e.Graphics.DrawString("PAYMENT DETAILS   •   BUILD " + Program.appversion, f, ink, 16f, 12f);
        }

        private void BkXPaint(object sender, PaintEventArgs e)
        {
            Control c = sender as Control;
            if (c == null) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (Pen p = new Pen(Color.White, 2f))
            {
                p.StartCap = LineCap.Round;
                p.EndCap = LineCap.Round;
                e.Graphics.DrawLine(p, 11, 8, c.Width - 11, c.Height - 8);
                e.Graphics.DrawLine(p, c.Width - 11, 8, 11, c.Height - 8);
            }
        }

        private void BkDeep(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c == _bkHead) continue;

                TextBox tb = c as TextBox;
                if (tb != null)
                {
                    tb.BorderStyle = BorderStyle.FixedSingle;
                    tb.BackColor = Color.White;
                    tb.ForeColor = BKink;
                    tb.Font = new Font("Segoe UI", 9.5f);
                }

                ComboBox cb = c as ComboBox;
                if (cb != null)
                {
                    cb.FlatStyle = FlatStyle.Flat;
                    cb.BackColor = Color.White;
                    cb.ForeColor = BKink;
                    cb.Font = new Font("Segoe UI", 9.5f);
                    cb.DrawMode = DrawMode.OwnerDrawFixed;
                    cb.ItemHeight = 24;
                    cb.DropDownHeight = 220;
                    cb.DrawItem += BkComboDraw;
                }

                ListBox lb = c as ListBox;
                if (lb != null)
                {
                    lb.BorderStyle = BorderStyle.None;
                    lb.BackColor = Color.White;
                    lb.ForeColor = BKink;
                    lb.Font = new Font("Segoe UI", 9.5f);
                }

                Label l = c as Label;
                if (l != null)
                {
                    l.ForeColor = BKink;
                    l.BackColor = Color.Transparent;
                }

                Button b = c as Button;
                if (b != null) BkBtn(b);

                Panel p = c as Panel;
                if (p != null)
                {
                    p.BackColor = BKcard;
                    p.Paint += BkCard;
                }

                GroupBox g = c as GroupBox;
                if (g != null) g.ForeColor = BKink;

                BkDeep(c);
            }
        }

        private void BkComboDraw(object sender, DrawItemEventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb == null || e.Index < 0) return;

            bool sel = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Rectangle r = e.Bounds;

            using (SolidBrush bg = new SolidBrush(sel ? Color.FromArgb(226, 238, 252) : Color.White))
                e.Graphics.FillRectangle(bg, r);

            if (sel)
                using (SolidBrush bar = new SolidBrush(BKorange))
                    e.Graphics.FillRectangle(bar, r.X, r.Y, 3, r.Height);

            string txt = Convert.ToString(cb.Items[e.Index]);
            TextRenderer.DrawText(e.Graphics, txt, cb.Font,
                new Rectangle(r.X + 8, r.Y, r.Width - 12, r.Height), BKink,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
        }

        private void BkBtn(Button b)
        {
            string t = (b.Text == null ? "" : b.Text).ToUpperInvariant();

            bool tab = t.Contains("BANKING") || t.Contains("CARD");

            if (tab)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 1;
                b.FlatAppearance.BorderColor = BKline;
                b.BackColor = BKcard;
                b.ForeColor = BKink;
                b.Font = new Font("Segoe UI Semibold", 10f);
            }
            else
            {
                Color bg = BKorange;
                if (t.Contains("DEL") || t.Contains("REMOVE") || t.Contains("CLEAR"))
                    bg = Color.FromArgb(220, 38, 38);
                else if (t.Contains("EDIT") || t.Contains("UPDATE"))
                    bg = Color.FromArgb(59, 132, 247);
                else if (t.Contains("CLOSE") || t.Contains("CANCEL"))
                    bg = Color.FromArgb(140, 158, 182);

                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 1;
                b.FlatAppearance.BorderColor = BkDark(bg);
                b.BackColor = bg;
                b.ForeColor = Color.White;
                b.Font = new Font("Segoe UI Semibold", 9.5f);
            }

            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;
        }

        private static Color BkDark(Color c)
        {
            return Color.FromArgb(
                Math.Max(0, c.R - 40),
                Math.Max(0, c.G - 40),
                Math.Max(0, c.B - 40));
        }

        private void BkCard(object sender, PaintEventArgs e)
        {
            Control c = sender as Control;
            if (c == null || c.Width < 40 || c.Height < 30) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);

            using (GraphicsPath path = BkRound(r, 10))
            {
                using (SolidBrush b = new SolidBrush(BKcard)) e.Graphics.FillPath(b, path);
                using (Pen pen = new Pen(BKline, 1f)) e.Graphics.DrawPath(pen, path);
            }
        }

        private void BkRegion()
        {
            using (GraphicsPath p = BkRound(new Rectangle(0, 0, Width, Height), 12))
                Region = new Region(p);
        }

        private static GraphicsPath BkRound(Rectangle r, int rad)
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
    }
}