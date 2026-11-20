using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace APP_FRAMEWORK;

public partial class frmAddID
{
    private bool exShell;

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

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        AutoScaleMode = AutoScaleMode.None;
        BackColor = Color.FromArgb(1, 9, 18);
        ForeColor = Color.White;
        MinimumSize = new Size(1000, 620);

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
        btnClose.Paint -= ByteNovaClosePaint;
        btnClose.Paint += ExClosePaint;

        PnlBase.AutoSize = false;
        PnlBase.Padding = new Padding(6);
        PnlBase.BackColor = Color.FromArgb(3, 18, 32);
        PnlBase.Paint -= ByteNovaBasePaint;
        PnlBase.Paint += ExBasePaint;

        tabIrctcId.Dock = DockStyle.Fill;
        tabIrctcId.Anchor = AnchorStyles.None;
        tabHistory.Dock = DockStyle.Fill;
        tabHistory.Anchor = AnchorStyles.None;

        tabHistory.BackColor = Color.FromArgb(3, 18, 32);

        exShell = true;
        ExFit();
    }

    private void ExFit()
    {
        Rectangle wa = Screen.FromControl(this).WorkingArea;

        int w = Math.Min(1180, wa.Width - 80);
        int h = Math.Min(700, wa.Height - 60);
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
            pnlHeader.Size = new Size(w - 12, 34);

            formName.Location = new Point(14, 7);
            formName.Size = new Size(w - 130, 20);

            btnClose.Location = new Point(w - 40, 8);
            btnClose.Size = new Size(28, 22);

            PnlBase.Location = new Point(6, 44);
            PnlBase.Size = new Size(w - 12, h - 52);

            ExRound(this, 12);
            ExRound(PnlBase, 10);
            ExRound(btnClose, 6);

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

    private void ExHeaderPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);

        using (GraphicsPath p = ExPath(r, 10))
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(4, 42, 78), Color.FromArgb(2, 20, 38),
            LinearGradientMode.Horizontal))
        {
            e.Graphics.FillPath(bg, p);
            using (Pen pen = new Pen(Color.FromArgb(0, 120, 175), 1f))
                e.Graphics.DrawPath(pen, p);
        }

        using (SolidBrush lime = new SolidBrush(Color.FromArgb(198, 244, 50)))
            e.Graphics.FillRectangle(lime, 14, c.Height - 3, 46, 2);
    }

    private void ExBasePaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);

        using (GraphicsPath p = ExPath(r, 10))
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(5, 31, 51), Color.FromArgb(2, 13, 24),
            LinearGradientMode.Vertical))
        {
            e.Graphics.FillPath(bg, p);
            using (Pen pen = new Pen(Color.FromArgb(0, 135, 205), 1.2f))
                e.Graphics.DrawPath(pen, p);
        }
    }

    private void ExClosePaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(1, 1, c.Width - 3, c.Height - 3);

        using (GraphicsPath p = ExPath(r, 6))
        {
            using (LinearGradientBrush bg = new LinearGradientBrush(
                r, Color.FromArgb(196, 26, 40), Color.FromArgb(78, 6, 17),
                LinearGradientMode.Vertical))
                e.Graphics.FillPath(bg, p);
            using (Pen pen = new Pen(Color.FromArgb(255, 84, 94), 1.2f))
                e.Graphics.DrawPath(pen, p);
        }

        using (Pen x = new Pen(Color.White, 2.2f))
        {
            x.StartCap = LineCap.Round;
            x.EndCap = LineCap.Round;
            e.Graphics.DrawLine(x, 10, 7, c.Width - 10, c.Height - 7);
            e.Graphics.DrawLine(x, c.Width - 10, 7, 10, c.Height - 7);
        }
    }
}