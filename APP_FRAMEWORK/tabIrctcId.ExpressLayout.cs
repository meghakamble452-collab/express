using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace APP_FRAMEWORK;

internal partial class tabIrctcId
{
    private static readonly Color ExBg = Color.FromArgb(233, 241, 252);
    private static readonly Color ExCard = Color.White;
    private static readonly Color ExFieldBg = Color.White;
    private static readonly Color ExLine = Color.FromArgb(196, 214, 240);
    private static readonly Color ExText = Color.FromArgb(18, 42, 74);
    private static readonly Color ExMuted = Color.FromArgb(96, 120, 152);
    private static readonly Color ExLime = Color.FromArgb(198, 244, 50);
    private static readonly Color ExCyan = Color.FromArgb(0, 132, 190);

    private bool exReady;
    private Dictionary<Button, bool> exHot;
    private HashSet<Button> exFill;
    private HashSet<Button> exDown;
    private HashSet<Control> exFocus;
    private HashSet<RadioButton> exHotRb;
    private HashSet<CheckBox> exHotCb;

    private Panel exFId, exFPwd, exFMob, exFPass, exFPnr, exBulk, exRange, exSearchIcon;
    private Label exArrow;

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

    private static Color ExDark(Color c, int amt)
    {
        return Color.FromArgb(
            Math.Max(0, c.R - amt),
            Math.Max(0, c.G - amt),
            Math.Max(0, c.B - amt));
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

    // ------------------------- styling -------------------------

    private void ExPrep()
    {
        SuspendLayout();
        try
        {
            AutoScaleMode = AutoScaleMode.None;
            BackColor = ExBg;
            ForeColor = ExText;
            Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Pixel);

            exHot = new Dictionary<Button, bool>();
            exFill = new HashSet<Button>();
            exDown = new HashSet<Button>();
            exFocus = new HashSet<Control>();
            exHotRb = new HashSet<RadioButton>();
            exHotCb = new HashSet<CheckBox>();

            Paint -= ByteNovaControlPaint;
            Paint += ExBgPaint;

            grpAddID.Text = "ADD NEW IRCTC ID";
            groupBox1.Text = "SEARCH  &&  BULK ACTIONS";

            ExGroup(grpAddID);
            ExGroup(groupBox1);

            exFId = ExField(grpAddID, txtLogin);
            exFPwd = ExField(grpAddID, txtPassword);
            exFMob = ExField(grpAddID, textmob);
            exFPass = ExField(grpAddID, textmpass);
            exBulk = ExField(this, txtbulk);

            exRange = new Panel();
            exRange.BackColor = ExFieldBg;
            exRange.Paint += ExFieldPaint;
            groupBox1.Controls.Add(exRange);
            B2AAA.Parent = exRange;
            dtDate2.Parent = exRange;

            exArrow = new Label();
            exArrow.AutoSize = false;
            exArrow.BackColor = Color.Transparent;
            exArrow.ForeColor = ExMuted;
            exArrow.TextAlign = ContentAlignment.MiddleCenter;
            exArrow.Font = new Font("Segoe UI Semibold", 13f, FontStyle.Bold, GraphicsUnit.Pixel);
            exArrow.Text = "\u2192";
            groupBox1.Controls.Add(exArrow);

            exSearchIcon = new Panel();
            exSearchIcon.BackColor = ExFieldBg;
            exSearchIcon.Paint += ExSearchIconPaint;
            groupBox1.Controls.Add(exSearchIcon);

            exFPnr = ExField(groupBox1, txtPnr);

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

            ExLabel(label3, ExMuted, 11f, false);
            ExLabel(label2, ExMuted, 11f, false);
            ExLabel(B3AAA, ExMuted, 11f, false);
            ExLabel(label7, ExMuted, 11f, false);
            ExLabel(label9, ExMuted, 11f, false);
            ExLabel(label11, ExMuted, 11f, false);
            ExLabel(label1, ExMuted, 11f, false);
            ExLabel(label10, ExMuted, 11f, false);
            ExLabel(C3AA0, ExMuted, 11f, false);
            ExLabel(B1AAA, ExMuted, 11f, false);

            ExChip(lblTotal);
            ExChip(lblActive);
            ExChip(lbl6);
            ExChip(lblDisableCount);
            ExChip(lblInValid);
            ExChip(lblUnVerified);
            ExChip(lblTobk);
            ExChip(lbl7);
            ExChip(lblCurMonbk);
            ExLabel(lblMsg, ExCyan, 12f, true);

            ExCheck(chkValidate);
            ExCheck(chkShowPwd);
            ExCheck(checkBox1);

            RadioBdate.Text = "By Book Date";
            RadioJdate.Text = "By Journey Date";
            RadioPnr.Text = "By PNR";
            ExRadio(RadioBdate);
            ExRadio(RadioJdate);
            ExRadio(RadioPnr);

            txtbulk.BorderStyle = BorderStyle.None;
            txtbulk.BackColor = ExFieldBg;
            txtbulk.ForeColor = ExText;
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
            ExBtn(button5, Color.FromArgb(60, 140, 240), false);
            ExBtn(button3, Color.FromArgb(240, 150, 30), false);
            ExBtn(C1AA0, Color.FromArgb(20, 170, 160), false);
            ExBtn(button1, Color.FromArgb(124, 82, 232), false);
            ExBtn(button2, ExLime, true);
            ExBtn(C2AA0, Color.FromArgb(150, 165, 185), false);
            ExBtn(btnChangePwd, Color.FromArgb(60, 140, 240), false);
            ExBtn(btnChangeQn, Color.FromArgb(60, 140, 240), false);
            ExBtn(btnHideDay, Color.FromArgb(60, 140, 240), false);
            ExBtn(button4, Color.FromArgb(60, 140, 240), false);

            CmbDays.FlatStyle = FlatStyle.Standard;
            CmbDays.BackColor = Color.White;
            CmbDays.ForeColor = ExText;
            CmbDays.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Pixel);

            ExDate(B2AAA);
            ExDate(dtDate2);

            panel13.BackColor = ExCard;
            panel13.Paint -= ByteNovaPanelPaint;
            panel13.Paint += ExCardPaint;

            pnlloader.BackColor = ExCard;
            pnlloader.ForeColor = ExText;
            pnlloader.Paint -= ByteNovaPanelPaint;
            pnlloader.Paint += ExCardPaint;

            ExGrid();
        }
        finally
        {
            ResumeLayout(false);
            PerformLayout();
        }

        exReady = true;
        ExLayout();
    }

    private Panel ExField(Control parent, Control box)
    {
        if (box == null) return null;

        Panel p = new Panel();
        p.BackColor = ExFieldBg;
        p.Paint += ExFieldPaint;
        parent.Controls.Add(p);
        p.BringToFront();

        box.Parent = p;
        box.BackColor = ExFieldBg;
        box.ForeColor = ExText;

        TextBox tb = box as TextBox;
        if (tb != null)
        {
            tb.BorderStyle = BorderStyle.None;
            tb.Font = new Font("Segoe UI Semibold", 13f, FontStyle.Regular, GraphicsUnit.Pixel);
            tb.Enter += ExFocusIn;
            tb.Leave += ExFocusOut;
        }

        RichTextBox rtb = box as RichTextBox;
        if (rtb != null)
        {
            rtb.BorderStyle = BorderStyle.None;
            rtb.Enter += ExFocusIn;
            rtb.Leave += ExFocusOut;
        }

        p.Tag = box;
        return p;
    }

    private void ExFocusIn(object sender, EventArgs e)
    {
        Control c = sender as Control;
        if (c == null || c.Parent == null) return;
        exFocus.Add(c.Parent);
        c.Parent.Invalidate();
    }

    private void ExFocusOut(object sender, EventArgs e)
    {
        Control c = sender as Control;
        if (c == null || c.Parent == null) return;
        exFocus.Remove(c.Parent);
        c.Parent.Invalidate();
    }

    private void ExBgPaint(object sender, PaintEventArgs e)
    {
        Rectangle r = new Rectangle(0, 0, Width, Height);
        if (r.Width < 2 || r.Height < 2) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(240, 246, 254), Color.FromArgb(214, 230, 250),
            LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, r);
    }

    private void ExFieldPaint(object sender, PaintEventArgs e)
    {
        Control p = sender as Control;
        if (p == null || p.Width < 6) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, p.Width - 1, p.Height - 1);
        bool hot = exFocus.Contains(p);

        using (GraphicsPath path = ExPath(r, 9))
        {
            using (SolidBrush b = new SolidBrush(ExFieldBg)) e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(hot ? Color.FromArgb(0, 132, 190) : ExLine, hot ? 1.6f : 1f))
                e.Graphics.DrawPath(pen, path);
        }

        using (SolidBrush accent = new SolidBrush(hot ? ExLime : Color.FromArgb(120, 165, 225)))
            e.Graphics.FillRectangle(accent, 1, 11, 3, Math.Max(4, p.Height - 22));
    }

    private void ExSearchIconPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (Pen p = new Pen(Color.FromArgb(0, 132, 190), 1.8f))
        {
            p.StartCap = LineCap.Round;
            p.EndCap = LineCap.Round;
            e.Graphics.DrawEllipse(p, 5, 5, 9, 9);
            e.Graphics.DrawLine(p, 13, 14, 18, 19);
        }
    }

    private void ExCardPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null || c.Width < 6) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);

        using (GraphicsPath path = ExPath(r, 10))
        {
            using (SolidBrush b = new SolidBrush(ExCard)) e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(ExLine, 1.2f)) e.Graphics.DrawPath(pen, path);
        }
    }

    private void ExGroup(GroupBox g)
    {
        if (g == null) return;
        g.BackColor = ExBg;
        g.ForeColor = ExText;
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

        using (GraphicsPath path = ExPath(r, 11))
        {
            using (SolidBrush b = new SolidBrush(ExCard)) e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(Color.FromArgb(160, 192, 232), 1.2f))
                e.Graphics.DrawPath(pen, path);
        }

        SizeF t = e.Graphics.MeasureString(g.Text, g.Font);
        using (SolidBrush erase = new SolidBrush(ExBg))
            e.Graphics.FillRectangle(erase, 12, 0, t.Width + 30, 17);
        using (SolidBrush lime = new SolidBrush(ExLime))
            e.Graphics.FillRectangle(lime, 18, 5, 3, 9);
        using (SolidBrush txt = new SolidBrush(ExText))
            e.Graphics.DrawString(g.Text, g.Font, txt, 30, 1);
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

    private void ExChip(Label l)
    {
        if (l == null) return;
        l.AutoSize = false;
        l.BackColor = Color.White;
        l.ForeColor = Color.FromArgb(18, 42, 74);
        l.TextAlign = ContentAlignment.MiddleCenter;
        l.Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold, GraphicsUnit.Pixel);
        l.Paint -= ExChipPaint;
        l.Paint += ExChipPaint;
    }

    private void ExChipPaint(object sender, PaintEventArgs e)
    {
        Label l = sender as Label;
        if (l == null || l.Width < 6) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, l.Width - 1, l.Height - 1);

        using (GraphicsPath path = ExPath(r, r.Height / 2))
        {
            using (SolidBrush b = new SolidBrush(Color.White)) e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(Color.FromArgb(160, 192, 232), 1.2f))
                e.Graphics.DrawPath(pen, path);
        }

        TextRenderer.DrawText(e.Graphics, l.Text, l.Font, r, Color.FromArgb(18, 42, 74),
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }

    private void ExCheck(CheckBox c)
    {
        if (c == null) return;
        c.AutoSize = false;
        c.BackColor = Color.Transparent;
        c.ForeColor = ExText;
        c.FlatStyle = FlatStyle.Flat;
        c.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
        c.Cursor = Cursors.Hand;
        c.Paint -= ExCheckPaint;
        c.Paint += ExCheckPaint;
        c.MouseEnter -= ExCheckIn;
        c.MouseEnter += ExCheckIn;
        c.MouseLeave -= ExCheckOut;
        c.MouseLeave += ExCheckOut;
        c.CheckedChanged -= ExCheckChanged;
        c.CheckedChanged += ExCheckChanged;
    }

    private void ExCheckIn(object sender, EventArgs e)
    {
        CheckBox c = sender as CheckBox;
        if (c == null) return;
        exHotCb.Add(c);
        c.Invalidate();
    }

    private void ExCheckOut(object sender, EventArgs e)
    {
        CheckBox c = sender as CheckBox;
        if (c == null) return;
        exHotCb.Remove(c);
        c.Invalidate();
    }

    private void ExCheckChanged(object sender, EventArgs e)
    {
        CheckBox c = sender as CheckBox;
        if (c != null) c.Invalidate();
    }

    private void ExCheckPaint(object sender, PaintEventArgs e)
    {
        CheckBox c = sender as CheckBox;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        bool hot = exHotCb.Contains(c);
        Rectangle box = new Rectangle(1, (c.Height - 15) / 2, 15, 15);

        using (GraphicsPath path = ExPath(box, 4))
        {
            using (SolidBrush b = new SolidBrush(c.Checked ? ExLime : (hot ? Color.FromArgb(226, 238, 252) : Color.White)))
                e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(c.Checked ? Color.FromArgb(140, 190, 30) : ExLine, 1.2f))
                e.Graphics.DrawPath(pen, path);
        }

        if (c.Checked)
        {
            using (Pen tick = new Pen(Color.FromArgb(20, 45, 25), 1.9f))
            {
                tick.StartCap = LineCap.Round;
                tick.EndCap = LineCap.Round;
                e.Graphics.DrawLine(tick, box.X + 4, box.Y + 8, box.X + 6, box.Y + 11);
                e.Graphics.DrawLine(tick, box.X + 6, box.Y + 11, box.X + 11, box.Y + 4);
            }
        }

        TextRenderer.DrawText(e.Graphics, c.Text, c.Font,
            new Rectangle(21, 0, c.Width - 23, c.Height), ExText,
            TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
    }

    private void ExRadio(RadioButton r)
    {
        if (r == null) return;
        r.AutoSize = false;
        r.BackColor = Color.Transparent;
        r.ForeColor = ExText;
        r.Font = new Font("Segoe UI Semibold", 12f, FontStyle.Bold, GraphicsUnit.Pixel);
        r.Cursor = Cursors.Hand;
        r.Paint -= ExRadioPaint;
        r.Paint += ExRadioPaint;
        r.MouseEnter -= ExRadioIn;
        r.MouseEnter += ExRadioIn;
        r.MouseLeave -= ExRadioOut;
        r.MouseLeave += ExRadioOut;
        r.CheckedChanged -= ExRadioChanged;
        r.CheckedChanged += ExRadioChanged;
    }

    private void ExRadioIn(object sender, EventArgs e)
    {
        RadioButton r = sender as RadioButton;
        if (r == null) return;
        exHotRb.Add(r);
        r.Invalidate();
    }

    private void ExRadioOut(object sender, EventArgs e)
    {
        RadioButton r = sender as RadioButton;
        if (r == null) return;
        exHotRb.Remove(r);
        r.Invalidate();
    }

    private void ExRadioChanged(object sender, EventArgs e)
    {
        RadioButton r = sender as RadioButton;
        if (r == null) return;
        r.Invalidate();
        if (r.Parent != null) r.Parent.Invalidate();
    }

    private void ExRadioPaint(object sender, PaintEventArgs e)
    {
        RadioButton r = sender as RadioButton;
        if (r == null || r.Width < 8) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        bool on = r.Checked;
        bool hot = exHotRb.Contains(r);
        Rectangle box = new Rectangle(0, 0, r.Width - 1, r.Height - 1);

        Color fill = on ? ExLime : (hot ? Color.FromArgb(226, 238, 252) : Color.White);

        using (GraphicsPath path = ExPath(box, r.Height / 2))
        {
            using (SolidBrush b = new SolidBrush(fill)) e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(on ? Color.FromArgb(140, 190, 30) : ExLine, 1.2f))
                e.Graphics.DrawPath(pen, path);
        }

        int cy = r.Height / 2;
        using (SolidBrush dot = new SolidBrush(on ? Color.FromArgb(20, 45, 25) : Color.FromArgb(150, 175, 205)))
            e.Graphics.FillEllipse(dot, 9, cy - 4, 8, 8);

        TextRenderer.DrawText(e.Graphics, r.Text, r.Font,
            new Rectangle(23, 0, r.Width - 29, r.Height),
            on ? Color.FromArgb(18, 42, 22) : ExText,
            TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
    }

    private void ExDate(DateTimePicker d)
    {
        if (d == null) return;
        d.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Pixel);
        d.CalendarForeColor = ExText;
        d.CalendarMonthBackground = Color.White;
        d.CalendarTitleBackColor = Color.FromArgb(60, 130, 220);
        d.CalendarTitleForeColor = Color.White;
    }

    private void ExGrid()
    {
        grdUsers.EnableHeadersVisualStyles = false;
        grdUsers.BorderStyle = BorderStyle.None;
        grdUsers.BackgroundColor = Color.White;
        grdUsers.GridColor = Color.FromArgb(222, 233, 246);
        grdUsers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grdUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grdUsers.ColumnHeadersHeight = 30;
        grdUsers.RowTemplate.Height = 26;

        grdUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(226, 238, 252);
        grdUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(18, 42, 74);
        grdUsers.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(226, 238, 252);
        grdUsers.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(18, 42, 74);
        grdUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 12f, FontStyle.Bold, GraphicsUnit.Pixel);

        grdUsers.DefaultCellStyle.BackColor = Color.White;
        grdUsers.DefaultCellStyle.ForeColor = Color.FromArgb(24, 46, 74);
        grdUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(214, 234, 255);
        grdUsers.DefaultCellStyle.SelectionForeColor = Color.FromArgb(12, 34, 60);
        grdUsers.DefaultCellStyle.Font = new Font("Segoe UI", 12f, FontStyle.Regular, GraphicsUnit.Pixel);

        grdUsers.RowsDefaultCellStyle.BackColor = Color.White;
        grdUsers.RowsDefaultCellStyle.ForeColor = Color.FromArgb(24, 46, 74);
        grdUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 251, 255);
        grdUsers.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(24, 46, 74);

        foreach (DataGridViewColumn col in grdUsers.Columns)
        {
            col.DefaultCellStyle.BackColor = Color.Empty;
            col.DefaultCellStyle.ForeColor = Color.FromArgb(24, 46, 74);
            col.DefaultCellStyle.SelectionBackColor = Color.FromArgb(214, 234, 255);
            col.DefaultCellStyle.SelectionForeColor = Color.FromArgb(12, 34, 60);

            DataGridViewLinkColumn link = col as DataGridViewLinkColumn;
            if (link != null)
            {
                link.LinkColor = Color.FromArgb(20, 110, 190);
                link.ActiveLinkColor = Color.FromArgb(124, 82, 232);
                link.VisitedLinkColor = Color.FromArgb(20, 110, 190);
                link.TrackVisitedState = false;
                link.DefaultCellStyle.ForeColor = Color.FromArgb(20, 110, 190);
                link.DefaultCellStyle.SelectionForeColor = Color.FromArgb(124, 82, 232);
            }
        }

        grdUsers.Invalidate();
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
        b.ForeColor = filled ? Color.FromArgb(16, 40, 20) : ExText;
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
        b.MouseDown -= ExBtnDown;
        b.MouseDown += ExBtnDown;
        b.MouseUp -= ExBtnUp;
        b.MouseUp += ExBtnUp;
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
        exDown.Remove(b);
        b.Invalidate();
    }

    private void ExBtnDown(object sender, MouseEventArgs e)
    {
        Button b = sender as Button;
        if (b == null) return;
        exDown.Add(b);
        b.Invalidate();
    }

    private void ExBtnUp(object sender, MouseEventArgs e)
    {
        Button b = sender as Button;
        if (b == null) return;
        exDown.Remove(b);
        b.Invalidate();
    }

    private void ExBtnPaint(object sender, PaintEventArgs e)
    {
        Button b = sender as Button;
        if (b == null || b.Width < 6) return;

        Color accent = (b.Tag is Color) ? (Color)b.Tag : Color.FromArgb(60, 140, 240);
        bool hot = exHot.ContainsKey(b) && exHot[b];
        bool down = exDown.Contains(b);
        bool filled = exFill.Contains(b);

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, b.Width - 1, b.Height - 1);

        Color fill;
        if (filled)
            fill = down ? ExDark(accent, 34) : (hot ? ExLight(accent, 26) : accent);
        else
            fill = down ? Color.FromArgb(210, 230, 250) : (hot ? Color.FromArgb(232, 242, 254) : Color.White);

        Color border = down ? ExDark(accent, 10) : accent;
        Color text = filled ? Color.FromArgb(16, 40, 20) : (down ? ExDark(accent, 40) : accent);

        using (GraphicsPath path = ExPath(r, 9))
        {
            using (LinearGradientBrush gb = new LinearGradientBrush(
                r, ExLight(fill, 10), fill, LinearGradientMode.Vertical))
                e.Graphics.FillPath(gb, path);
            using (Pen pen = new Pen(border, 1.3f))
                e.Graphics.DrawPath(pen, path);
        }

        Rectangle tr = r;
        if (down) tr.Offset(0, 1);

        TextRenderer.DrawText(e.Graphics, b.Text, b.Font, tr, text,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding | TextFormatFlags.WordBreak);
    }

    // ------------------------- layout -------------------------

    private void ExLayout()
    {
        if (!exReady || IsDisposed) return;

        int W = ClientSize.Width;
        int H = ClientSize.Height;
        if (W < 500 || H < 420) return;

        const int M = 14;
        int right = W - M;
        int innerW = right - M;

        int bulkW = 236;
        int bulkX = right - bulkW;
        int gridW = bulkX - 12 - M;

        int footY = H - 52;
        int grpY = footY - 8 - 178;
        int gridY = 168;
        int gridH = grpY - 8 - gridY;
        int gx = 14;
        int gw = innerW - 28;

        SuspendLayout();
        try
        {
            ExPut(grpAddID, M, 10, innerW, 150);
            ExRound(grpAddID, 11);

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

            ExPut(chkValidate, gx, 118, 280, 24);
            ExPut(chkShowPwd, gx + 292, 118, 140, 24);
            ExPut(checkBox1, gx + 444, 118, 130, 24);

            ExPut(btnIDSave, gx + gw - 170, 26, 170, 46);
            ExRound(btnIDSave, 10);
            ExPut(button5, gx + gw - 160, 80, 160, 30);
            ExRound(button5, 8);

            ExPut(grdUsers, M, gridY, gridW, gridH);
            ExRound(grdUsers, 9);

            ExPut(label6, bulkX, 146, bulkW, 18);
            ExPut(exBulk, bulkX, gridY, bulkW, gridH - 30);
            ExRound(exBulk, 9);
            ExPut(txtbulk, 12, 10, bulkW - 24, gridH - 50);

            ExPut(button4, bulkX, gridY + gridH - 26, 70, 24);
            ExRound(button4, 7);

            // search group
            ExPut(groupBox1, M, grpY, innerW, 178);
            ExRound(groupBox1, 11);

            ExPut(btnChangePwd, gx, 26, 180, 30);
            ExRound(btnChangePwd, 8);
            ExPut(btnChangeQn, gx + 190, 26, 180, 30);
            ExRound(btnChangeQn, 8);
            ExPut(label14, gx + 382, 30, 160, 22);
            ExPut(CmbDays, gx + 548, 28, 90, 26);
            ExPut(btnHideDay, gx + 648, 27, 112, 28);
            ExRound(btnHideDay, 8);

            ExPut(RadioBdate, gx, 62, 132, 26);
            ExPut(RadioJdate, gx + 140, 62, 152, 26);
            ExPut(RadioPnr, gx + 300, 62, 98, 26);

            ExPut(exRange, gx, 96, 420, 38);
            ExRound(exRange, 9);
            ExPut(B2AAA, 14, 8, 118, 22);
            ExPut(exArrow, 136, 8, 24, 22);
            ExPut(dtDate2, 164, 8, 118, 22);

            ExPut(exFPnr, gx + 432, 96, 204, 38);
            ExRound(exFPnr, 9);
            ExPut(txtPnr, 38, 9, 154, 20);
            ExPut(exSearchIcon, gx + 442, 103, 24, 24);

            ExPut(button2, gx + 648, 96, 110, 38);
            ExRound(button2, 9);
            ExPut(C2AA0, gx + 768, 96, 90, 38);
            ExRound(C2AA0, 9);

            ExPut(panel13, gx, 146, gw, 24);
            ExRound(panel13, 8);
            ExPut(label3, 10, 3, 168, 18);
            ExPut(lblCurMonbk, 182, 3, 32, 18);
            ExPut(label2, 220, 3, 138, 18);
            ExPut(B3AAA, 366, 3, 232, 18);
            ExPut(lbl7, 604, 3, 32, 18);

            // footer counters
            int r1 = H - 46;
            int r2 = H - 24;

            ExPut(B1AAA, 14, r1, 44, 18);
            ExPut(lblTotal, 60, r1, 30, 18);
            ExPut(label10, 100, r1, 50, 18);
            ExPut(lblActive, 152, r1, 30, 18);
            ExPut(C3AA0, 192, r1, 54, 18);
            ExPut(lbl6, 248, r1, 30, 18);
            ExPut(label7, 288, r1, 56, 18);
            ExPut(lblDisableCount, 346, r1, 30, 18);

            ExPut(label9, 14, r2, 50, 18);
            ExPut(lblInValid, 66, r2, 30, 18);
            ExPut(label11, 106, r2, 76, 18);
            ExPut(lblUnVerified, 184, r2, 30, 18);
            ExPut(label1, 224, r2, 62, 18);
            ExPut(lblTobk, 288, r2, 30, 18);

            ExPut(lblMsg, 328, r2, 190, 18);
            ExPut(pictureBox1, 528, r2 - 8, 28, 28);

            ExPut(button3, W - 419, H - 48, 110, 40);
            ExRound(button3, 10);
            ExPut(C1AA0, W - 301, H - 48, 115, 40);
            ExRound(C1AA0, 10);
            ExPut(button1, W - 178, H - 48, 150, 40);
            ExRound(button1, 10);

            ExPut(pnlloader, M + (gridW / 2) - 216, gridY + (gridH / 2) - 32, 433, 65);
            ExRound(pnlloader, 11);
            pnlloader.BringToFront();

            panel13.BringToFront();
            exSearchIcon.BringToFront();
            exRange.BringToFront();
            exArrow.BringToFront();
        }
        finally
        {
            ResumeLayout(false);
            PerformLayout();
            Invalidate(true);
        }
    }
}