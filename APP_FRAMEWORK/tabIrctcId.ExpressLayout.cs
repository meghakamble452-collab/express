using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace APP_FRAMEWORK;

internal partial class tabIrctcId
{
    private static readonly Color ExBg = Color.FromArgb(3, 18, 32);
    private static readonly Color ExCard = Color.FromArgb(6, 30, 48);
    private static readonly Color ExFieldBg = Color.FromArgb(5, 26, 42);
    private static readonly Color ExLine = Color.FromArgb(28, 78, 106);
    private static readonly Color ExText = Color.FromArgb(216, 236, 246);
    private static readonly Color ExMuted = Color.FromArgb(148, 192, 212);
    private static readonly Color ExLime = Color.FromArgb(198, 244, 50);
    private static readonly Color ExCyan = Color.FromArgb(0, 225, 255);

    private bool exReady;
    private Dictionary<Button, bool> exHot;
    private HashSet<Button> exFill;

    private Panel exFId, exFPwd, exFMob, exFPass, exFPnr, exBulk;

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

    private static Color ExLight(Color c, int amt)
    {
        return Color.FromArgb(
            Math.Min(255, c.R + amt),
            Math.Min(255, c.G + amt),
            Math.Min(255, c.B + amt));
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ExPrep();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        ExLayout();
    }

    private void ExPut(Control c, int x, int y, int w, int h)
    {
        if (c == null) return;
        c.Location = new Point(x, y);
        c.Size = new Size(Math.Max(2, w), Math.Max(2, h));
    }

    // ---------------- styling ----------------

    private void ExPrep()
    {
        SuspendLayout();
        try
        {
            AutoScaleMode = AutoScaleMode.None;
            BackColor = ExBg;
            Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Pixel);

            exHot = new Dictionary<Button, bool>();
            exFill = new HashSet<Button>();

            grpAddID.Text = "ADD NEW IRCTC ID";
            groupBox1.Text = "SEARCH  &  BULK ACTIONS";

            ExGroup(grpAddID);
            ExGroup(groupBox1);

            exFId = ExField(grpAddID, txtLogin, 12, false);
            exFPwd = ExField(grpAddID, txtPassword, 12, false);
            exFMob = ExField(grpAddID, textmob, 12, false);
            exFPass = ExField(grpAddID, textmpass, 12, false);
            exFPnr = ExField(groupBox1, txtPnr, 12, false);
            exBulk = ExField(this, txtbulk, 10, true);

            Label[] labels = new Label[]
            {
                label37, lblPassword, label4, label5, label6, label7, label9,
                label10, label11, label1, label2, label3, B1AAA, B3AAA, C3AA0,
                lblTotal, lblActive, lbl6, lbl7, lblTobk, lblCurMonbk, lblMsg,
                lblUnVerified, lblInValid, lblDisableCount, label14
            };
            foreach (Label l in labels) ExLabel(l, ExMuted, 12f, false);

            ExLabel(label37, ExText, 12f, true);
            ExLabel(lblPassword, ExText, 12f, true);
            ExLabel(label4, ExText, 12f, true);
            ExLabel(label5, ExText, 12f, true);
            ExLabel(label14, ExText, 12f, true);
            ExLabel(label6, ExCyan, 12f, true);

            ExLabel(lblTotal, ExLime, 13f, true);
            ExLabel(lblActive, ExLime, 13f, true);
            ExLabel(lbl6, ExLime, 13f, true);
            ExLabel(lblDisableCount, ExLime, 13f, true);
            ExLabel(lblInValid, ExLime, 13f, true);
            ExLabel(lblUnVerified, ExLime, 13f, true);
            ExLabel(lblTobk, ExLime, 13f, true);
            ExLabel(lbl7, ExLime, 13f, true);
            ExLabel(lblCurMonbk, ExLime, 13f, true);
            ExLabel(B3AAA, ExLime, 13f, true);
            ExLabel(lblMsg, ExCyan, 12f, true);

            ExCheck(chkValidate);
            ExCheck(chkShowPwd);
            ExCheck(checkBox1);
            ExCheck(RadioBdate);
            ExCheck(RadioJdate);
            ExCheck(RadioPnr);

            txtLogin.MaxLength = 50;
            txtbulk.BorderStyle = BorderStyle.None;
            txtbulk.BackColor = ExFieldBg;
            txtbulk.ForeColor = Color.White;
            txtbulk.Font = new Font("Consolas", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
            txtbulk.ScrollBars = RichTextBoxScrollBars.Vertical;

            btnIDSave.Text = "ADD IRCTC ID";
            button3.Text = "CHANGE PASS";
            C1AA0.Text = "SYNC ALL IDS";
            button1.Text = "MANAGE BOOKINGS";
            button5.Text = "CREATE NEW ID";
            button2.Text = "SEARCH";
            C2AA0.Text = "CLEAR";
            btnChangePwd.Text = "CHANGE ALL PWD";
            btnChangeQn.Text = "CHANGE ALL ANS";
            btnHideDay.Text = "SAVE DAYS";
            button4.Text = "PASTE";

            ExBtn(btnIDSave, ExLime, true);
            ExBtn(button5, Color.FromArgb(0, 150, 200), false);
            ExBtn(button3, Color.FromArgb(255, 152, 0), false);
            ExBtn(C1AA0, Color.FromArgb(0, 170, 220), false);
            ExBtn(button1, Color.FromArgb(120, 90, 255), false);
            ExBtn(button2, Color.FromArgb(0, 190, 160), false);
            ExBtn(C2AA0, Color.FromArgb(150, 150, 160), false);
            ExBtn(btnChangePwd, Color.FromArgb(0, 150, 200), false);
            ExBtn(btnChangeQn, Color.FromArgb(0, 150, 200), false);
            ExBtn(btnHideDay, Color.FromArgb(0, 150, 200), false);
            ExBtn(button4, Color.FromArgb(0, 150, 200), false);

            CmbDays.FlatStyle = FlatStyle.Flat;
            CmbDays.BackColor = ExFieldBg;
            CmbDays.ForeColor = Color.White;
            CmbDays.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Pixel);

            ExDate(B2AAA);
            ExDate(dtDate2);

            panel13.BackColor = Color.FromArgb(4, 24, 40);
            panel13.Paint -= ByteNovaPanelPaint;
            panel13.Paint += ExPanelPaint;

            pnlloader.BackColor = Color.FromArgb(4, 26, 44);
            pnlloader.Paint -= ByteNovaPanelPaint;
            pnlloader.Paint += ExPanelPaint;

            grdUsers.BorderStyle = BorderStyle.None;
        }
        finally
        {
            ResumeLayout(false);
            PerformLayout();
        }

        exReady = true;
        ExLayout();
    }

    private Panel ExField(Control parent, Control box, int pad, bool fill)
    {
        if (box == null) return null;

        Panel p = new Panel();
        p.BackColor = ExFieldBg;
        p.Paint += ExFieldPaint;
        parent.Controls.Add(p);
        p.BringToFront();

        box.Parent = p;
        box.BorderStyle = BorderStyle.None;
        box.BackColor = ExFieldBg;
        box.ForeColor = Color.White;
        if (box is TextBox)
            box.Font = new Font("Segoe UI Semibold", 13f, FontStyle.Regular, GraphicsUnit.Pixel);

        p.Tag = fill;
        return p;
    }

    private void ExFieldPaint(object sender, PaintEventArgs e)
    {
        Panel p = sender as Panel;
        if (p == null || p.Width < 4) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, p.Width - 1, p.Height - 1);

        using (GraphicsPath path = ExPath(r, 8))
        {
            using (SolidBrush b = new SolidBrush(ExFieldBg)) e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(ExLine, 1f)) e.Graphics.DrawPath(pen, path);
        }

        using (SolidBrush accent = new SolidBrush(Color.FromArgb(0, 168, 215)))
            e.Graphics.FillRectangle(accent, 1, 10, 3, Math.Max(4, p.Height - 20));
    }

    private void ExPanelPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);
        using (GraphicsPath path = ExPath(r, 8))
        {
            using (SolidBrush b = new SolidBrush(c.BackColor)) e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(Color.FromArgb(0, 120, 175), 1f)) e.Graphics.DrawPath(pen, path);
        }
    }

    private void ExGroup(GroupBox g)
    {
        if (g == null) return;
        g.BackColor = ExBg;
        g.ForeColor = ExLime;
        g.Font = new Font("Segoe UI Semibold", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
        g.Paint -= ByteNovaGroupPaint;
        g.Paint += ExGroupPaint;
    }

    private void ExGroupPaint(object sender, PaintEventArgs e)
    {
        GroupBox g = sender as GroupBox;
        if (g == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 8, g.Width - 1, g.Height - 9);

        using (GraphicsPath path = ExPath(r, 10))
        {
            using (SolidBrush b = new SolidBrush(Color.FromArgb(4, 22, 37)))
                e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(Color.FromArgb(0, 118, 168), 1f))
                e.Graphics.DrawPath(pen, path);
        }

        SizeF t = e.Graphics.MeasureString(g.Text, g.Font);
        using (SolidBrush erase = new SolidBrush(ExBg))
            e.Graphics.FillRectangle(erase, 12, 0, t.Width + 10, 17);
        using (SolidBrush txt = new SolidBrush(ExLime))
            e.Graphics.DrawString(g.Text, g.Font, txt, 16, 1);
    }

    private void ExLabel(Label l, Color c, float px, bool bold)
    {
        if (l == null) return;
        l.AutoSize = false;
        l.BackColor = Color.Transparent;
        l.ForeColor = c;
        l.TextAlign = ContentAlignment.MiddleLeft;
        l.Font = new Font("Segoe UI", px,
            bold ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Pixel);
    }

    private void ExCheck(ButtonBase b)
    {
        if (b == null) return;
        b.AutoSize = false;
        b.BackColor = Color.Transparent;
        b.ForeColor = ExText;
        b.FlatStyle = FlatStyle.Flat;
        b.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
        b.Cursor = Cursors.Hand;
    }

    private void ExDate(DateTimePicker d)
    {
        if (d == null) return;
        d.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
        d.CalendarForeColor = Color.White;
        d.CalendarMonthBackground = ExFieldBg;
        d.CalendarTitleBackColor = Color.FromArgb(0, 95, 135);
        d.CalendarTitleForeColor = Color.White;
    }

    private void ExBtn(Button b, Color accent, bool filled)
    {
        if (b == null) return;

        b.UseVisualStyleBackColor = false;
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.FlatAppearance.MouseOverBackColor = Color.Transparent;
        b.FlatAppearance.MouseDownBackColor = Color.Transparent;
        b.BackColor = filled ? accent : ExCard;
        b.ForeColor = filled ? Color.FromArgb(6, 20, 28) : Color.White;
        b.Font = new Font("Segoe UI Semibold", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
        b.Cursor = Cursors.Hand;
        b.Tag = accent;

        if (filled) exFill.Add(b);

        b.Paint -= ExBtnPaint;
        b.Paint += ExBtnPaint;
        b.MouseEnter -= ExBtnEnter;
        b.MouseEnter += ExBtnEnter;
        b.MouseLeave -= ExBtnLeave;
        b.MouseLeave += ExBtnLeave;
    }

    private void ExBtnEnter(object sender, EventArgs e)
    {
        Button b = sender as Button;
        if (b == null) return;
        exHot[b] = true;
        b.Invalidate();
    }

    private void ExBtnLeave(object sender, EventArgs e)
    {
        Button b = sender as Button;
        if (b == null) return;
        exHot[b] = false;
        b.Invalidate();
    }

    private void ExBtnPaint(object sender, PaintEventArgs e)
    {
        Button b = sender as Button;
        if (b == null || b.Width < 4) return;

        Color accent = (b.Tag is Color) ? (Color)b.Tag : Color.FromArgb(0, 150, 200);
        bool hot = exHot.ContainsKey(b) && exHot[b];
        bool filled = exFill.Contains(b);

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, b.Width - 1, b.Height - 1);

        Color fill = filled ? (hot ? ExLight(accent, 30) : accent)
                            : (hot ? ExLight(ExCard, 24) : ExCard);

        using (GraphicsPath path = ExPath(r, 8))
        {
            using (SolidBrush br = new SolidBrush(fill)) e.Graphics.FillPath(br, path);
            using (Pen pen = new Pen(hot ? Color.White : accent, 1.2f))
                e.Graphics.DrawPath(pen, path);
        }

        TextRenderer.DrawText(e.Graphics, b.Text, b.Font, r, b.ForeColor,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding | TextFormatFlags.WordBreak);
    }

    // ---------------- layout ----------------

    private void ExLayout()
    {
        if (!exReady || IsDisposed) return;

        int W = ClientSize.Width;
        int H = ClientSize.Height;
        if (W < 500 || H < 400) return;

        const int M = 14;
        int right = W - M;
        int innerW = right - M;

        int bulkW = 240;
        int bulkX = right - bulkW;
        int gridW = bulkX - 12 - M;

        int footY = H - 54;
        int grpY = footY - 8 - 124;
        int gridY = 170;
        int gridH = grpY - 8 - gridY;

        SuspendLayout();
        try
        {
            // ----- row: add id -----
            ExPut(grpAddID, M, 12, innerW, 150);
            ExRound(grpAddID, 10);

            int gx = 14;
            int gw = innerW - 28;

            ExPut(label37, gx, 28, 80, 22);
            ExPut(exFId, gx + 84, 22, 200, 38);
            ExPut(txtLogin, 12, 9, 176, 20);

            ExPut(lblPassword, gx + 306, 28, 96, 22);
            ExPut(exFPwd, gx + 406, 22, 220, 38);
            ExPut(txtPassword, 12, 9, 196, 20);

            ExPut(label4, gx, 74, 80, 22);
            ExPut(exFMob, gx + 84, 68, 200, 38);
            ExPut(textmob, 12, 9, 176, 20);

            ExPut(label5, gx + 306, 74, 110, 22);
            ExPut(exFPass, gx + 420, 68, 186, 38);
            ExPut(textmpass, 12, 9, 162, 20);

            label5.Visible = textmpass.Visible;
            exFPass.Visible = textmpass.Visible;

            ExPut(chkValidate, gx, 116, 300, 22);
            ExPut(chkShowPwd, gx + 316, 116, 140, 22);
            ExPut(checkBox1, gx + 470, 116, 130, 22);

            ExPut(btnIDSave, gw - 170 + 14, 26, 170, 46);
            ExRound(btnIDSave, 9);
            ExPut(button5, gw - 160 + 14, 84, 160, 30);
            ExRound(button5, 8);

            // ----- grid -----
            ExPut(grdUsers, M, gridY, gridW, gridH);
            ExRound(grdUsers, 8);

            // ----- bulk column -----
            ExPut(label6, bulkX, 148, bulkW, 18);
            ExPut(exBulk, bulkX, gridY, bulkW, gridH - 30);
            ExRound(exBulk, 8);
            ExPut(txtbulk, 12, 10, bulkW - 24, gridH - 50);

            ExPut(button4, bulkX, gridY + gridH - 26, 70, 24);
            ExRound(button4, 7);

            // ----- search group -----
            ExPut(groupBox1, M, grpY, innerW, 124);
            ExRound(groupBox1, 10);

            ExPut(btnChangePwd, 14, 26, 180, 30);
            ExRound(btnChangePwd, 8);
            ExPut(btnChangeQn, 204, 26, 180, 30);
            ExRound(btnChangeQn, 8);
            ExPut(label14, 396, 30, 168, 22);
            ExPut(CmbDays, 568, 28, 70, 26);
            ExPut(btnHideDay, 648, 27, 112, 28);
            ExRound(btnHideDay, 8);

            ExPut(RadioBdate, 14, 66, 120, 22);
            ExPut(B2AAA, 140, 64, 120, 24);
            ExPut(RadioJdate, 272, 66, 140, 22);
            ExPut(dtDate2, 418, 64, 120, 24);
            ExPut(RadioPnr, 550, 66, 80, 22);
            ExPut(exFPnr, 636, 58, 140, 36);
            ExPut(txtPnr, 12, 8, 116, 20);
            ExPut(button2, 790, 62, 100, 30);
            ExRound(button2, 8);
            ExPut(C2AA0, 900, 62, 80, 30);
            ExRound(C2AA0, 8);

            ExPut(panel13, 14, 98, gw, 20);
            ExPut(label3, 4, 2, 180, 16);
            ExPut(lblCurMonbk, 190, 2, 40, 16);
            ExPut(label2, 240, 2, 140, 16);
            ExPut(B3AAA, 386, 2, 200, 16);
            ExPut(lbl7, 592, 2, 40, 16);

            // ----- counters -----
            int r1 = H - 52;
            int r2 = H - 26;

            ExPut(B1AAA, 14, r1, 52, 20);
            ExPut(lblTotal, 68, r1, 26, 20);
            ExPut(label10, 100, r1, 56, 20);
            ExPut(lblActive, 158, r1, 26, 20);
            ExPut(C3AA0, 190, r1, 56, 20);
            ExPut(lbl6, 248, r1, 26, 20);
            ExPut(label7, 280, r1, 62, 20);
            ExPut(lblDisableCount, 344, r1, 26, 20);

            ExPut(label9, 14, r2, 56, 20);
            ExPut(lblInValid, 72, r2, 26, 20);
            ExPut(label11, 104, r2, 76, 20);
            ExPut(lblUnVerified, 182, r2, 26, 20);
            ExPut(label1, 214, r2, 76, 20);
            ExPut(lblTobk, 292, r2, 26, 20);

            ExPut(lblMsg, 330, r2, 200, 20);
            ExPut(pictureBox1, 534, r2 - 8, 32, 32);

            // ----- footer buttons -----
            ExPut(button3, W - 419, H - 52, 110, 44);
            ExRound(button3, 9);
            ExPut(C1AA0, W - 301, H - 52, 115, 44);
            ExRound(C1AA0, 9);
            ExPut(button1, W - 178, H - 52, 150, 44);
            ExRound(button1, 9);

            // ----- loader overlay -----
            ExPut(pnlloader, M + (gridW / 2) - 216, gridY + (gridH / 2) - 32, 433, 65);
            ExRound(pnlloader, 10);
            pnlloader.BringToFront();

            panel13.BringToFront();
        }
        finally
        {
            ResumeLayout(false);
            PerformLayout();
            Invalidate(true);
        }
    }
}