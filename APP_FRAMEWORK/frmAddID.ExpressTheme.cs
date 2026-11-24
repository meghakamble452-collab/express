using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace APP_FRAMEWORK;

public partial class frmAddID
{
    private bool exShell;
    private Label exHistTitle;
    private bool exCloseHot;
    private bool exCloseDown;

    private static GraphicsPath ExPath(Rectangle r, int rad)
    {
        GraphicsPath p = new GraphicsPath();
        int d = rad * 2;
        if (d > r.Width) d = r.Width;
        if (d > r.Height) d = r.Height;
        if (d <= 2) { p.AddRectangle(r); return p; }
        p.AddArc(r.X, r.Y, d, d, 180, 90);
        p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        p.CloseFigure();
        return p;
    }

    private static void ExRound(Control c, int rad)
    {
        if (c == null || c.Width < 2 || c.Height < 2) return;
        using (GraphicsPath p = ExPath(new Rectangle(0, 0, c.Width, c.Height), rad))
            c.Region = new Region(p);
    }

    internal void ApplyExpressTheme()
    {
        if (exShell) return;
        exShell = true;

        AutoScaleMode = AutoScaleMode.None;
        AutoSize = false;
        BackColor = Color.FromArgb(150, 186, 245);
        ForeColor = Color.FromArgb(18, 42, 74);
        MinimumSize = new Size(900, 560);

        pnlHeader.AutoSize = false;
        pnlHeader.BorderStyle = BorderStyle.None;
        pnlHeader.BackColor = Color.Transparent;
        pnlHeader.Paint -= pnlHeader_Paint;
        pnlHeader.Paint += ExHeaderPaint;

        formName.AutoSize = false;
        formName.BackColor = Color.Transparent;
        formName.ForeColor = Color.White;
        formName.Font = new Font("Segoe UI Semibold", 14f, FontStyle.Bold, GraphicsUnit.Pixel);
        formName.TextAlign = ContentAlignment.MiddleLeft;
        formName.Text = "EXPRESS   •   ADD IRCTC ID";

        btnClose.Image = null;
        btnClose.BackgroundImage = null;
        btnClose.BackColor = Color.Transparent;
        btnClose.Region = null;
        btnClose.Cursor = Cursors.Hand;
        btnClose.Paint -= ByteNovaClosePaint;
        btnClose.Paint -= ExClosePaint;
        btnClose.Paint += ExClosePaint;
        btnClose.MouseEnter += delegate { exCloseHot = true; btnClose.Invalidate(); };
        btnClose.MouseLeave += delegate { exCloseHot = false; exCloseDown = false; btnClose.Invalidate(); };
        btnClose.MouseDown += delegate { exCloseDown = true; btnClose.Invalidate(); };
        btnClose.MouseUp += delegate { exCloseDown = false; btnClose.Invalidate(); };

        // keep the button INSIDE the header bar so it sits on the header colour
        if (btnClose.Parent != pnlHeader) pnlHeader.Controls.Add(btnClose);

        PnlBase.AutoSize = false;
        PnlBase.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        PnlBase.Padding = new Padding(6);
        PnlBase.BackColor = Color.FromArgb(233, 241, 252);
        PnlBase.Paint -= ByteNovaBasePaint;
        PnlBase.Paint += ExBasePaint;

        tabIrctcId.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tabHistory.Dock = DockStyle.Fill;
        tabHistory.BackColor = Color.FromArgb(233, 241, 252);

        Resize += delegate { ExLayoutShell(); };
        ExStyleHistory();
        tabHistory.Resize += delegate { ExStyleHistory(); };
        ExLayoutShell();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ApplyExpressTheme();
        ExFit();
    }

    private void ExFit()
    {
        Rectangle wa = Screen.FromControl(this).WorkingArea;

        int w = Math.Min(1080, wa.Width - 80);
        int h = Math.Min(640, wa.Height - 60);
        if (w < 1000) w = Math.Min(1000, wa.Width - 16);
        if (h < 620) h = Math.Min(620, wa.Height - 16);

        ClientSize = new Size(w, h);
        Location = new Point(wa.Left + (wa.Width - Width) / 2,
                             wa.Top + (wa.Height - Height) / 2);
        ExLayoutShell();
    }

    private void ExLayoutShell()
    {
        if (!exShell || IsDisposed) return;

        int w = ClientSize.Width;
        int h = ClientSize.Height;
        if (w < 300 || h < 200) return;

        SuspendLayout();
        try
        {
            pnlHeader.Location = new Point(6, 6);
            pnlHeader.Size = new Size(w - 12, 40);

            formName.Location = new Point(16, 10);
            formName.Size = new Size(w - 150, 20);

            // bigger close button: 38x30, vertically centred in the 40px header, 6px from its right edge
            btnClose.Size = new Size(38, 30);
            btnClose.Location = new Point(pnlHeader.Width - 38 - 6, (pnlHeader.Height - 30) / 2);
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // body starts right under the header and uses every remaining pixel
            PnlBase.Location = new Point(6, 50);
            PnlBase.Size = new Size(w - 12, h - 56);

            ExRound(PnlBase, 11);

            pnlHeader.BringToFront();
            btnClose.BringToFront();
        }
        finally
        {
            ResumeLayout(false);
            PerformLayout();
            Invalidate();
        }
    }

    private void ExStyleHistory()
    {
        if (tabHistory == null) return;

        tabHistory.BackColor = Color.FromArgb(233, 241, 252);
        tabHistory.Padding = new Padding(8, 42, 8, 8);

        foreach (Control c in tabHistory.Controls)
        {
            Label lbl = c as Label;
            if (lbl != null)
            {
                if (lbl.Name == "label16") { lbl.Visible = false; continue; }

                if (lbl.Name == "label1")
                {
                    exHistTitle = lbl;
                    lbl.Dock = DockStyle.None;
                    lbl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                    lbl.AutoSize = false;
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.ForeColor = Color.White;
                    lbl.BackColor = Color.Transparent;
                    lbl.Font = new Font("Segoe UI Semibold", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
                    lbl.Paint -= ExHistTitlePaint;
                    lbl.Paint += ExHistTitlePaint;
                    continue;
                }

                if (lbl.Name == "lblMsg")
                {
                    lbl.ForeColor = Color.FromArgb(17, 40, 72);
                    lbl.Font = new Font("Segoe UI Semibold", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
                }
                continue;
            }

            Panel pnl = c as Panel;
            if (pnl != null && pnl.Name == "pnlloader")
            {
                pnl.BackColor = Color.White;
                pnl.BorderStyle = BorderStyle.None;
                continue;
            }

            DataGridView g = c as DataGridView;
            if (g != null)
            {
                g.Dock = DockStyle.Fill;
                g.EnableHeadersVisualStyles = false;
                g.BorderStyle = BorderStyle.None;
                g.BackgroundColor = Color.White;
                g.GridColor = Color.FromArgb(226, 236, 248);
                g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                g.RowHeadersVisible = false;
                g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                g.ColumnHeadersHeight = 34;
                g.RowTemplate.Height = 30;
                g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(226, 238, 252);
                g.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(17, 40, 72);
                g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(226, 238, 252);
                g.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 40, 72);
                g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
                g.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 4, 0);

                g.DefaultCellStyle.BackColor = Color.White;
                g.DefaultCellStyle.ForeColor = Color.FromArgb(24, 46, 74);
                g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(214, 234, 255);
                g.DefaultCellStyle.SelectionForeColor = Color.FromArgb(12, 34, 60);
                g.DefaultCellStyle.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
                g.DefaultCellStyle.Padding = new Padding(8, 0, 4, 0);
                g.RowsDefaultCellStyle.BackColor = Color.White;
                g.RowsDefaultCellStyle.ForeColor = Color.FromArgb(24, 46, 74);
                g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 251, 255);
                g.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(24, 46, 74);

                foreach (DataGridViewColumn col in g.Columns)
                {
                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    col.MinimumWidth = 62;
                    string n = col.Name.ToLower();
                    if (n.Contains("pnr") || n.Contains("from") || n.Contains("to") || n.Contains("train")) col.FillWeight = 72;
                    else if (n.Contains("date")) col.FillWeight = 110;
                    else col.FillWeight = 82;
                }
                g.Invalidate();
            }
        }

        if (exHistTitle != null)
        {
            exHistTitle.SetBounds(8, 6, Math.Max(60, tabHistory.ClientSize.Width - 16), 30);
            exHistTitle.BringToFront();
        }
    }

    private void ExHistTitlePaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null || c.Width < 4) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, c.Width, c.Height);

        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(74, 124, 216), Color.FromArgb(122, 166, 240),
            LinearGradientMode.Horizontal))
            e.Graphics.FillRectangle(bg, r);

        TextRenderer.DrawText(e.Graphics, c.Text, c.Font, r, Color.White,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }

    private void ExHeaderPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);

        using (GraphicsPath p = ExPath(r, 11))
        {
            using (LinearGradientBrush bg = new LinearGradientBrush(
                r, Color.FromArgb(74, 124, 216), Color.FromArgb(122, 166, 240),
                LinearGradientMode.Horizontal))
                e.Graphics.FillPath(bg, p);

            using (Pen pen = new Pen(Color.FromArgb(198, 244, 50), 1.6f))
                e.Graphics.DrawPath(pen, p);
        }

        using (SolidBrush lime = new SolidBrush(Color.FromArgb(198, 244, 50)))
            e.Graphics.FillRectangle(lime, 16, c.Height - 4, 52, 3);
    }

    private void ExBasePaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);

        using (GraphicsPath p = ExPath(r, 11))
        {
            using (SolidBrush bg = new SolidBrush(Color.FromArgb(238, 245, 253)))
                e.Graphics.FillPath(bg, p);
            using (Pen pen = new Pen(Color.FromArgb(150, 186, 245), 1.2f))
                e.Graphics.DrawPath(pen, p);
        }
    }

    private void ExClosePaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;

        // default: red with white X | hover: white with red X + red border | pressed: dark red, shifted down
        Color top = Color.FromArgb(239, 68, 68), bot = Color.FromArgb(220, 38, 38);
        Color edgeC = Color.FromArgb(185, 28, 28), glyph = Color.White;
        if (exCloseDown)
        {
            top = Color.FromArgb(170, 26, 26); bot = Color.FromArgb(140, 20, 20);
            edgeC = Color.FromArgb(127, 29, 29);
        }
        else if (exCloseHot)
        {
            top = Color.White; bot = Color.FromArgb(254, 242, 242);
            edgeC = Color.FromArgb(220, 38, 38); glyph = Color.FromArgb(220, 38, 38);
        }

        Rectangle r = new Rectangle(1, 1, c.Width - 3, c.Height - 4);
        if (exCloseDown) r.Offset(0, 1);
        if (r.Width < 4 || r.Height < 4) return;

        if (!exCloseDown)
            using (GraphicsPath sp = ExPath(new Rectangle(r.X, r.Y + 2, r.Width, r.Height), 8))
            using (SolidBrush sh = new SolidBrush(Color.FromArgb(55, 60, 10, 10)))
                g.FillPath(sh, sp);

        using (GraphicsPath p = ExPath(r, 8))
        {
            using (LinearGradientBrush bg = new LinearGradientBrush(r, top, bot, LinearGradientMode.Vertical))
                g.FillPath(bg, p);
            using (Pen edge = new Pen(edgeC, exCloseHot && !exCloseDown ? 1.6f : 1f))
                g.DrawPath(edge, p);
        }

        // square, centred X (same arm length both ways)
        float cx = r.X + r.Width / 2f, cy = r.Y + r.Height / 2f, a = 6f;
        using (Pen x = new Pen(glyph, 2.4f))
        {
            x.StartCap = LineCap.Round;
            x.EndCap = LineCap.Round;
            g.DrawLine(x, cx - a, cy - a, cx + a, cy + a);
            g.DrawLine(x, cx + a, cy - a, cx - a, cy + a);
        }
    }
}