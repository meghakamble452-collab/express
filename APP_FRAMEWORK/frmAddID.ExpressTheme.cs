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

    internal void ApplyExpressTheme()
    {
        if (exShell) return;
        exShell = true;

        AutoScaleMode = AutoScaleMode.None;
        AutoSize = false;
        BackColor = Color.FromArgb(150, 186, 245);
        ForeColor = Color.FromArgb(18, 42, 74);
        MinimumSize = new Size(1040, 680);

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
        PnlBase.BackColor = Color.FromArgb(233, 241, 252);
        PnlBase.Paint -= ByteNovaBasePaint;
        PnlBase.Paint += ExBasePaint;

        tabIrctcId.Dock = DockStyle.Fill;
        tabIrctcId.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        tabHistory.Dock = DockStyle.Fill;
        tabHistory.BackColor = Color.FromArgb(233, 241, 252);

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
            pnlHeader.Location = new Point(8, 8);
            pnlHeader.Size = new Size(w - 16, 34);

            formName.Location = new Point(16, 7);
            formName.Size = new Size(w - 140, 20);

            btnClose.Location = new Point(w - 44, 10);
            btnClose.Size = new Size(28, 22);

            PnlBase.Location = new Point(8, 48);
            PnlBase.Size = new Size(w - 16, h - 58);

            
            ExRound(PnlBase, 11);
            ExRound(btnClose, 7);

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

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(1, 1, c.Width - 3, c.Height - 3);

        using (GraphicsPath p = ExPath(r, 7))
        {
            using (LinearGradientBrush bg = new LinearGradientBrush(
                r, Color.FromArgb(236, 62, 76), Color.FromArgb(190, 32, 46),
                LinearGradientMode.Vertical))
                e.Graphics.FillPath(bg, p);
            using (Pen pen = new Pen(Color.White, 1.2f))
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