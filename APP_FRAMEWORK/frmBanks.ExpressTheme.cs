using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace APP_FRAMEWORK;

internal partial class frmBanks
{
    private static readonly Color TBg = Color.FromArgb(233, 241, 252);
    private static readonly Color TCard = Color.White;
    private static readonly Color TLine = Color.FromArgb(198, 216, 242);
    private static readonly Color TInk = Color.FromArgb(17, 40, 72);
    private static readonly Color TOrange = Color.FromArgb(245, 146, 28);
    private static readonly Color THead = Color.FromArgb(12, 38, 72);

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

            foreach (Control c in Controls)
                if (c.Top < 120) c.Top += 44;
            ClientSize = new Size(ClientSize.Width, ClientSize.Height + 44);

            _tHead = new Panel { BackColor = THead, Bounds = new Rectangle(0, 0, ClientSize.Width, 40) };
            _tHead.Paint += HeadPaint;
            _tHead.MouseDown += (s, e) => { }
            Controls.Add(_tHead);
            _tHead.BringToFront();

            Button x = new Button
            {
                Text = "✕",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(220, 38, 38),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Bounds = new Rectangle(ClientSize.Width - 42, 7, 30, 26),
                Cursor = Cursors.Hand
            };
            x.FlatAppearance.BorderSize = 0;
            x.Click += (s, e) => Close();
            _tHead.Controls.Add(x);

            Style(this);
        }
        finally { ResumeLayout(false); }
    }

    private void HeadPaint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (LinearGradientBrush g = new LinearGradientBrush(_tHead.ClientRectangle,
            Color.FromArgb(8, 26, 52), Color.FromArgb(16, 58, 104), 0f))
            e.Graphics.FillRectangle(g, _tHead.ClientRectangle);
        using (Pen lime = new Pen(Color.FromArgb(198, 244, 50), 2f))
            e.Graphics.DrawLine(lime, 0, _tHead.Height - 2, _tHead.Width, _tHead.Height - 2);
        using (Font f = new Font("Segoe UI Semibold", 11f, FontStyle.Bold))
        using (SolidBrush b = new SolidBrush(Color.White))
            e.Graphics.DrawString("PAYMENT DETAILS", f, b, 14, 9);
    }

    private void Style(Control root)
    {
        foreach (Control c in root.Controls)
        {
            TextBox tb = c as TextBox;
            if (tb != null)
            {
                tb.BorderStyle = BorderStyle.FixedSingle;
                tb.BackColor = Color.White;
                tb.ForeColor = TInk;
                tb.Font = new Font("Segoe UI", 9.2f);
                continue;
            }

            ComboBox cb = c as ComboBox;
            if (cb != null)
            {
                cb.FlatStyle = FlatStyle.Flat;
                cb.BackColor = Color.White;
                cb.ForeColor = TInk;
                cb.Font = new Font("Segoe UI", 9.2f);
                continue;
            }

            ListBox lb = c as ListBox;
            if (lb != null)
            {
                lb.BorderStyle = BorderStyle.None;
                lb.BackColor = Color.White;
                lb.ForeColor = TInk;
                lb.Font = new Font("Segoe UI", 8.8f);
                lb.ItemHeight = 20;
                lb.DrawMode = DrawMode.OwnerDrawFixed;
                lb.DrawItem += (s, e) =>
                {
                    if (e.Index < 0) return;
                    bool sel = (e.State & DrawItemState.Selected) != 0;
                    using (SolidBrush b = new SolidBrush(sel ? Color.FromArgb(214, 234, 255) : Color.White))
                        e.Graphics.FillRectangle(b, e.Bounds);
                    TextRenderer.DrawText(e.Graphics, lb.Items[e.Index].ToString(), lb.Font,
                        new Rectangle(e.Bounds.X + 6, e.Bounds.Y, e.Bounds.Width - 8, e.Bounds.Height), TInk,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
                };
                continue;
            }

            Button bt = c as Button;
            if (bt != null)
            {
                string t = bt.Text.Replace("&", "").ToLower();
                bool danger = t.Contains("delete");
                bool primary = t.Contains("save") || t.Contains("edit");
                bt.FlatStyle = FlatStyle.Flat;
                bt.FlatAppearance.BorderSize = 0;
                bt.BackColor = danger ? Color.FromArgb(226, 72, 60)
                              : primary ? TOrange : Color.FromArgb(96, 118, 148);
                bt.ForeColor = Color.White;
                bt.Font = new Font("Segoe UI Semibold", 8.6f, FontStyle.Bold);
                bt.Cursor = Cursors.Hand;
                continue;
            }

            Panel p = c as Panel;
            if (p != null && p != _tHead)
            {
                p.BackColor = TCard;
                p.Paint -= CardPaint;
                p.Paint += CardPaint;
                Style(p);
                continue;
            }

            Label l = c as Label;
            if (l != null)
            {
                l.ForeColor = TInk;
                l.Font = new Font("Segoe UI Semibold", 8.4f, FontStyle.Bold);
                l.BackColor = Color.Transparent;
                continue;
            }

            GroupBox gb = c as GroupBox;
            if (gb != null)
            {
                gb.ForeColor = TInk;
                gb.BackColor = TCard;
                Style(gb);
                continue;
            }

            Style(c);
        }
    }

    private void CardPaint(object sender, PaintEventArgs e)
    {
        Panel p = sender as Panel;
        if (p == null || p.Width < 8) return;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, p.Width - 1, p.Height - 1);
        using (GraphicsPath path = Round(r, 8))
        {
            using (SolidBrush b = new SolidBrush(TCard)) e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(TLine, 1.1f)) e.Graphics.DrawPath(pen, path);
        }
    }

    private static GraphicsPath Round(Rectangle r, int rad)
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