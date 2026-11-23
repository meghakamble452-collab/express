using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace APP_FRAMEWORK
{
    internal partial class frmBanks
    {
        private static readonly Color TBg = Color.FromArgb(233, 241, 252);
        private static readonly Color TCard = Color.White;
        private static readonly Color TLine = Color.FromArgb(198, 216, 242);
        private static readonly Color TInk = Color.FromArgb(17, 40, 72);
        private static readonly Color TOrange = Color.FromArgb(245, 146, 28);
        private static readonly Color THead = Color.FromArgb(12, 38, 72);

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        private Panel _tHead;
        private bool _tDone;

        private void ApplyBankTheme()
        {
            if (_tDone) return;
            _tDone = true;

            SuspendLayout();
            try
            {
                BackColor = TBg;
                FormBorderStyle = FormBorderStyle.None;
                StartPosition = FormStartPosition.CenterParent;
                Font = new Font("Segoe UI", 8.6f);
                DoubleBuffered = true;

                foreach (Control c in Controls)
                {
                    if (c.Top < 120) c.Top += 44;
                }

                ClientSize = new Size(ClientSize.Width, ClientSize.Height + 44);

                _tHead = new Panel();
                _tHead.BackColor = THead;
                _tHead.Bounds = new Rectangle(0, 0, ClientSize.Width, 40);
                _tHead.Paint += TBankHeadPaint;
                _tHead.MouseDown += TBankHeadDown;
                Controls.Add(_tHead);
                _tHead.BringToFront();

                Button x = new Button();
                x.Text = "X";
                x.FlatStyle = FlatStyle.Flat;
                x.FlatAppearance.BorderSize = 0;
                x.BackColor = Color.FromArgb(220, 38, 38);
                x.ForeColor = Color.White;
                x.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold);
                x.Size = new Size(34, 26);
                x.Location = new Point(ClientSize.Width - 44, 7);
                x.Cursor = Cursors.Hand;
                x.Click += delegate { Close(); };
                _tHead.Controls.Add(x);

                TBankDeep(this);
            }
            finally
            {
                ResumeLayout(false);
                PerformLayout();
            }

            TBankRegion();
        }

        private void TBankHeadDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, 0xA1, 0x2, 0);
            }
        }

        private void TBankHeadPaint(object sender, PaintEventArgs e)
        {
            Rectangle r = new Rectangle(0, 0, _tHead.Width, _tHead.Height);

            using (LinearGradientBrush b = new LinearGradientBrush(r, Color.FromArgb(8, 26, 52), Color.FromArgb(16, 58, 104), 0f))
                e.Graphics.FillRectangle(b, r);

            using (SolidBrush lime = new SolidBrush(Color.FromArgb(198, 244, 50)))
                e.Graphics.FillRectangle(lime, 0, _tHead.Height - 2, _tHead.Width, 2);

            using (SolidBrush ink = new SolidBrush(Color.White))
            using (Font f = new Font("Segoe UI Semibold", 11f))
                e.Graphics.DrawString("PAYMENT DETAILS", f, ink, 16f, 11f);
        }

        private void TBankDeep(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c == _tHead) continue;

                TextBox tb = c as TextBox;
                if (tb != null)
                {
                    tb.BorderStyle = BorderStyle.FixedSingle;
                    tb.BackColor = Color.White;
                    tb.ForeColor = TInk;
                    tb.Font = new Font("Segoe UI", 9f);
                }

                ComboBox cb = c as ComboBox;
                if (cb != null)
                {
                    cb.FlatStyle = FlatStyle.Flat;
                    cb.BackColor = Color.White;
                    cb.ForeColor = TInk;
                    cb.Font = new Font("Segoe UI", 9f);
                }

                ListBox lb = c as ListBox;
                if (lb != null)
                {
                    lb.BorderStyle = BorderStyle.None;
                    lb.BackColor = Color.White;
                    lb.ForeColor = TInk;
                    lb.Font = new Font("Segoe UI", 9f);
                }

                Label l = c as Label;
                if (l != null) l.ForeColor = TInk;

                Button b = c as Button;
                if (b != null) TBankBtn(b);

                Panel p = c as Panel;
                if (p != null)
                {
                    p.BackColor = TCard;
                    p.Paint += TBankCard;
                }

                GroupBox g = c as GroupBox;
                if (g != null) g.ForeColor = TInk;

                TBankDeep(c);
            }
        }

        private void TBankBtn(Button b)
        {
            string t = (b.Text == null ? "" : b.Text).ToUpperInvariant();
            Color bg = TOrange;

            if (t.Contains("DEL") || t.Contains("REMOVE") || t.Contains("CLEAR"))
                bg = Color.FromArgb(226, 88, 88);
            else if (t.Contains("CLOSE") || t.Contains("CANCEL"))
                bg = Color.FromArgb(140, 158, 182);

            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = bg;
            b.ForeColor = Color.White;
            b.Font = new Font("Segoe UI Semibold", 9f);
            b.Cursor = Cursors.Hand;
        }

        private void TBankCard(object sender, PaintEventArgs e)
        {
            Control c = sender as Control;
            if (c == null || c.Width < 12 || c.Height < 12) return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);

            using (GraphicsPath path = TBround(r, 10))
            {
                using (SolidBrush b = new SolidBrush(TCard)) e.Graphics.FillPath(b, path);
                using (Pen pen = new Pen(TLine, 1f)) e.Graphics.DrawPath(pen, path);
            }
        }

        private void TBankRegion()
        {
            using (GraphicsPath p = TBround(new Rectangle(0, 0, Width, Height), 12))
                Region = new Region(p);
        }

        private static GraphicsPath TBround(Rectangle r, int rad)
        {
            GraphicsPath p = new GraphicsPath();

            if (rad <= 0)
            {
                p.AddRectangle(r);
                return p;
            }

            int d = rad * 2;
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }
    }
}