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
    private static readonly Color ExFieldOff = Color.FromArgb(246, 249, 253);
    private static readonly Color ExLine = Color.FromArgb(198, 216, 242);
    private static readonly Color ExText = Color.FromArgb(17, 40, 72);
    private static readonly Color ExMuted = Color.FromArgb(104, 127, 158);
    private static readonly Color ExLime = Color.FromArgb(198, 244, 50);
    private static readonly Color ExCyan = Color.FromArgb(0, 132, 190);
    private static readonly Color ExInk = Color.FromArgb(12, 32, 58);

    private bool exBuilt;
    private bool exFormScaled;
    private float exS = 1f;

    private readonly HashSet<Button> exFill = new HashSet<Button>();
    private readonly HashSet<Control> exHover = new HashSet<Control>();
    private readonly HashSet<Control> exPress = new HashSet<Control>();
    private readonly HashSet<Control> exFocus = new HashSet<Control>();
    private readonly Dictionary<Control, float> exGlow = new Dictionary<Control, float>();
    private readonly Dictionary<Control, float> exGlowTo = new Dictionary<Control, float>();
    private Timer exTicker;

    private sealed class ExTag { public Control Box; public string Glyph; }

    private Panel exFId, exFPwd, exFMob, exFPass, exFPnr, exBulk, exRange, exFDate1, exFDate2, exDrop;
    private Label exArrow;
    private readonly List<Label> exPresets = new List<Label>();

    private readonly List<Label[]> exStats = new List<Label[]>();
    private readonly Dictionary<Label, Color> exStatColor = new Dictionary<Label, Color>();
    private readonly Dictionary<Label, string> exStatIcon = new Dictionary<Label, string>();

    private ToolStripDropDown exMenu;
    private Panel exMenuList;
    private int exMenuHover = -1;

    private ToolStripDropDown exCal;
    private Panel exCalPanel;
    private DateTimePicker exCalTarget;
    private DateTime exCalMonth = DateTime.Today;
    private int exCalHover = -1;

    // ---------------- dpi scale ----------------

    private int P(float v)
    {
        int r = (int)Math.Round(v * exS);
        return r < 1 ? 1 : r;
    }

    private Font ExF(float px, FontStyle st) =>
        new Font("Segoe UI", px * exS, st, GraphicsUnit.Pixel);

    private Font ExFS(float px, FontStyle st) =>
        new Font("Segoe UI Semibold", px * exS, st, GraphicsUnit.Pixel);

    // ---------------- geometry ----------------

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

    private static Color ExLight(Color c, int a) =>
        Color.FromArgb(Math.Min(255, c.R + a), Math.Min(255, c.G + a), Math.Min(255, c.B + a));

    private static Color ExDark(Color c, int a) =>
        Color.FromArgb(Math.Max(0, c.R - a), Math.Max(0, c.G - a), Math.Max(0, c.B - a));

    private static Color ExBlend(Color a, Color b, float t)
    {
        if (t < 0f) t = 0f; if (t > 1f) t = 1f;
        return Color.FromArgb((int)(a.R + (b.R - a.R) * t),
            (int)(a.G + (b.G - a.G) * t), (int)(a.B + (b.B - a.B) * t));
    }

    private const TextFormatFlags TF_MEASURE = TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix;

    private int ExTextW(Control c)
    {
        if (c == null || string.IsNullOrEmpty(c.Text)) return 0;
        using (Graphics g = CreateGraphics())
            return TextRenderer.MeasureText(g, c.Text, c.Font, new Size(4000, 60), TF_MEASURE).Width + P(6);
    }

    // ---------------- animation ----------------

    private void ExSync(Control c)
    {
        if (c == null) return;
        float t = 0f;
        if (exHover.Contains(c)) t += 0.50f;
        if (exPress.Contains(c)) t += 0.35f;
        if (exFocus.Contains(c)) t += 1.00f;
        ExAnim(c, Math.Min(1.6f, t));
    }

    private void ExAnim(Control c, float target)
    {
        if (c == null || c.IsDisposed) return;
        exGlowTo[c] = target;
        if (!exGlow.ContainsKey(c)) exGlow[c] = 0f;
        if (exTicker == null) { exTicker = new Timer { Interval = 16 }; exTicker.Tick += ExTick; }
        if (!exTicker.Enabled) exTicker.Start();
    }

    private void ExTick(object sender, EventArgs e)
    {
        bool busy = false;
        List<Control> keys = new List<Control>(exGlowTo.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            Control c = keys[i];
            if (c == null || c.IsDisposed) { exGlow.Remove(c); exGlowTo.Remove(c); continue; }
            float cur = exGlow.ContainsKey(c) ? exGlow[c] : 0f;
            float to = exGlowTo[c];
            float d = to - cur;
            if (Math.Abs(d) < 0.02f) cur = to;
            else { cur += d * 0.32f; busy = true; }
            exGlow[c] = cur;
            c.Invalidate();
        }
        if (!busy && exTicker != null) exTicker.Stop();
    }

    protected override void OnHandleDestroyed(EventArgs e)
    {
        if (exTicker != null) { exTicker.Stop(); exTicker.Dispose(); exTicker = null; }
        if (exCal != null) exCal.Close();
        if (exMenu != null) exMenu.Close();
        base.OnHandleDestroyed(e);
    }

    protected override void OnLoad(EventArgs e) { base.OnLoad(e); ExPrep(); }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        if (exBuilt) ExLayout();
    }

    private void ExPut(Control c, int x, int y, int w, int h)
    {
        if (c == null) return;
        c.Location = new Point(x, y);
        if (!c.AutoSize) c.Size = new Size(Math.Max(2, w), Math.Max(2, h));
    }

    private void ExScaleForm()
    {
        if (exFormScaled) return;
        exFormScaled = true;
        if (exS < 1.05f) return;

        Form f = FindForm();
        if (f == null) return;

        try
        {
            f.MinimumSize = new Size(P(f.MinimumSize.Width), P(f.MinimumSize.Height));
            f.ClientSize = new Size(P(f.ClientSize.Width), P(f.ClientSize.Height));
            Screen sc = Screen.FromControl(f);
            f.Location = new Point(
                sc.WorkingArea.Left + (sc.WorkingArea.Width - f.Width) / 2,
                sc.WorkingArea.Top + (sc.WorkingArea.Height - f.Height) / 2);
        }
        catch { }
    }

    // ---------------- build ----------------

    private void ExPrep()
    {
        if (exBuilt) return;

        try { using (Graphics g = CreateGraphics()) exS = g.DpiX / 96f; }
        catch { exS = 1f; }
        if (exS < 1f) exS = 1f;

        SuspendLayout();
        try
        {
            AutoScaleMode = AutoScaleMode.None;
            BackColor = ExBg;
            ForeColor = ExText;
            Font = ExF(12f, FontStyle.Regular);

            Paint -= ByteNovaControlPaint;
            Paint += ExBgPaint;

            grpAddID.Text = "ADD NEW IRCTC ID";
            groupBox1.Text = "SEARCH  &  BULK ACTIONS";
            ExGroup(grpAddID);
            ExGroup(groupBox1);

            exFId = ExField(grpAddID, txtLogin, "user");
            exFPwd = ExField(grpAddID, txtPassword, "lock");
            exFMob = ExField(grpAddID, textmob, "phone");
            exFPass = ExField(grpAddID, textmpass, "mail");
            exBulk = ExField(this, txtbulk, null);

            exRange = new Panel { BackColor = ExCard, Cursor = Cursors.Hand };
            exRange.Paint += ExRangePaint;
            groupBox1.Controls.Add(exRange);

            exFDate1 = ExField(exRange, B2AAA, "cal");
            exFDate2 = ExField(exRange, dtDate2, "cal");

            exArrow = new Label
            {
                AutoSize = false,
                BackColor = Color.Transparent,
                ForeColor = ExMuted,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = ExFS(12f, FontStyle.Bold),
                Text = "\u2192"
            };
            exRange.Controls.Add(exArrow);

            ExPreset("Today", ExPresetToday);
            ExPreset("7D", ExPreset7);
            ExPreset("30D", ExPreset30);
            ExPreset("This Month", ExPresetMonth);
            ExPreset("Clear", ExPresetClear);

            exFPnr = ExField(groupBox1, txtPnr, "search");

            CmbDays.Visible = false;
            if (string.IsNullOrEmpty(CmbDays.Text) && CmbDays.Items.Count > 0)
                CmbDays.SelectedIndex = 0;

            exDrop = new Panel { BackColor = ExFieldBg, Cursor = Cursors.Hand };
            exDrop.Paint += ExDropPaint;
            exDrop.Click += delegate { ExDropOpen(); };
            exDrop.MouseEnter += delegate { exHover.Add(exDrop); ExSync(exDrop); };
            exDrop.MouseLeave += delegate { exHover.Remove(exDrop); ExSync(exDrop); };
            groupBox1.Controls.Add(exDrop);

            CmbDays.TextChanged += delegate { if (exDrop != null) exDrop.Invalidate(); };
            CmbDays.SelectedIndexChanged += delegate { if (exDrop != null) exDrop.Invalidate(); };

            Label[] muted =
            {
                label37, lblPassword, label4, label5, label6, label7, label9,
                label10, label11, label1, label2, label3, B3AAA, C3AA0,
                lblTobk, lblCurMonbk, lblMsg, lblUnVerified, lblInValid, lblDisableCount, label14
            };
            foreach (Label l in muted) ExLabel(l, ExMuted, 12f, false);

            ExLabel(label37, ExText, 12f, true);
            ExLabel(lblPassword, ExText, 12f, true);
            ExLabel(label4, ExText, 12f, true);
            ExLabel(label5, ExText, 12f, true);
            ExLabel(label14, ExText, 12f, true);
            ExLabel(label6, ExCyan, 12f, true);
            ExLabel(lblMsg, ExCyan, 12f, true);

            ExStat(B1AAA, lblTotal, Color.FromArgb(0, 132, 190), "grid");
            ExStat(label10, lblActive, Color.FromArgb(34, 170, 92), "check");
            ExStat(C3AA0, lbl6, Color.FromArgb(240, 150, 30), "minus");
            ExStat(label7, lblDisableCount, Color.FromArgb(226, 72, 60), "slash");
            ExStat(label9, lblInValid, Color.FromArgb(226, 72, 60), "x");
            ExStat(label11, lblUnVerified, Color.FromArgb(214, 148, 20), "dot");
            ExStat(label1, lblTobk, Color.FromArgb(124, 82, 232), "ticket");

            ExChip(lblCurMonbk);
            ExChip(lbl7);

            ExCheck(chkValidate);
            ExCheck(chkShowPwd);
            ExCheck(checkBox1);

            RadioBdate.Text = "By Book Date";
            RadioJdate.Text = "By Journey Date";
            RadioPnr.Text = "By PNR";
            ExRadio(RadioBdate);
            ExRadio(RadioJdate);
            ExRadio(RadioPnr);

            RadioBdate.CheckedChanged += delegate { ExModeSync(); };
            RadioJdate.CheckedChanged += delegate { ExModeSync(); };
            RadioPnr.CheckedChanged += delegate { ExModeSync(); };

            txtbulk.BorderStyle = BorderStyle.None;
            txtbulk.BackColor = ExFieldBg;
            txtbulk.ForeColor = ExText;
            txtbulk.Font = new Font("Consolas", 12.5f * exS, FontStyle.Regular, GraphicsUnit.Pixel);
            txtbulk.ScrollBars = RichTextBoxScrollBars.Vertical;

            foreach (DateTimePicker d in new DateTimePicker[] { B2AAA, dtDate2 })
            {
                d.Font = ExFS(12.5f, FontStyle.Regular);
                try
                {
                    d.MinDate = new DateTime(2000, 1, 1);
                    d.MaxDate = DateTime.Today.AddYears(3);
                }
                catch { }
                if (d.Value.Year < 2021) { try { d.Value = DateTime.Today; } catch { } }
                d.Visible = false;
            }

            btnIDSave.Text = "ADD IRCTC ID";
            button5.Text = "CREATE NEW ID";
            button3.Text = "CHANGE PASS";
            C1AA0.Text = "SYNC ALL IDS";
            button1.Text = "MANAGE BOOKINGS";
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

            panel13.BackColor = ExCard;
            panel13.Paint -= ByteNovaPanelPaint;
            panel13.Paint += ExCardPaint;

            pnlloader.BackColor = ExCard;
            pnlloader.ForeColor = ExText;
            pnlloader.Paint -= ByteNovaPanelPaint;
            pnlloader.Paint += ExCardPaint;

            ExGrid();
            ExModeSync();
        }
        finally
        {
            ResumeLayout(false);
            PerformLayout();
        }

        exBuilt = true;
        ExLayout();
        BeginInvoke(new MethodInvoker(delegate { ExScaleForm(); ExLayout(); }));
    }

    private void ExModeSync()
    {
        bool byPnr = RadioPnr.Checked;
        if (B2AAA != null) B2AAA.Enabled = !byPnr;
        if (dtDate2 != null) dtDate2.Enabled = !byPnr;
        if (txtPnr != null) txtPnr.Enabled = byPnr;
        if (exFDate1 != null) exFDate1.Invalidate();
        if (exFDate2 != null) exFDate2.Invalidate();
        if (exFPnr != null) exFPnr.Invalidate();
    }

    // ---------------- presets ----------------

    private void ExPreset(string text, EventHandler onClick)
    {
        Label c = new Label
        {
            AutoSize = false,
            BackColor = Color.Transparent,
            ForeColor = ExMuted,
            TextAlign = ContentAlignment.MiddleCenter,
            Font = ExFS(11.5f, FontStyle.Bold),
            Text = text,
            Cursor = Cursors.Hand
        };
        c.Paint += ExPresetPaint;
        c.MouseEnter += delegate { exHover.Add(c); ExSync(c); };
        c.MouseLeave += delegate { exHover.Remove(c); ExSync(c); };
        c.Click += onClick;
        exRange.Controls.Add(c);
        exPresets.Add(c);
    }

    private void ExPresetToday(object s, EventArgs e) { ExSetRange(DateTime.Today, DateTime.Today); }
    private void ExPreset7(object s, EventArgs e) { ExSetRange(DateTime.Today.AddDays(-6), DateTime.Today); }
    private void ExPreset30(object s, EventArgs e) { ExSetRange(DateTime.Today.AddDays(-29), DateTime.Today); }
    private void ExPresetMonth(object s, EventArgs e)
    {
        DateTime n = DateTime.Today;
        ExSetRange(new DateTime(n.Year, n.Month, 1), n);
    }
    private void ExPresetClear(object s, EventArgs e)
    {
        ExSetRange(DateTime.Today.AddDays(-30), DateTime.Today);
        if (txtPnr != null) txtPnr.Clear();
    }

    private void ExSetRange(DateTime a, DateTime b)
    {
        try
        {
            if (B2AAA != null && B2AAA.Enabled) B2AAA.Value = a;
            if (dtDate2 != null && dtDate2.Enabled) dtDate2.Value = b;
        }
        catch { }
        if (exFDate1 != null) exFDate1.Invalidate();
        if (exFDate2 != null) exFDate2.Invalidate();
    }

    // ---------------- fields ----------------

    private Panel ExField(Control parent, Control box, string glyph)
    {
        if (box == null) return null;

        Panel p = new Panel { BackColor = ExFieldBg, Cursor = Cursors.IBeam };
        p.Tag = new ExTag { Box = box, Glyph = glyph };
        p.Paint += ExFieldPaint;
        p.Click += ExFieldClick;
        p.MouseEnter += delegate { exHover.Add(p); ExSync(p); };
        p.MouseLeave += delegate { exHover.Remove(p); ExSync(p); };
        parent.Controls.Add(p);

        box.Parent = p;
        box.BackColor = ExFieldBg;
        box.ForeColor = ExText;

        TextBox tb = box as TextBox;
        if (tb != null)
        {
            tb.BorderStyle = BorderStyle.None;
            tb.Font = ExFS(13f, FontStyle.Regular);
            tb.Enter += delegate { exFocus.Add(p); ExSync(p); };
            tb.Leave += delegate { exFocus.Remove(p); ExSync(p); };
        }

        RichTextBox rtb = box as RichTextBox;
        if (rtb != null)
        {
            rtb.BorderStyle = BorderStyle.None;
            rtb.Enter += delegate { exFocus.Add(p); ExSync(p); };
            rtb.Leave += delegate { exFocus.Remove(p); ExSync(p); };
        }

        DateTimePicker dt = box as DateTimePicker;
        if (dt != null)
        {
            p.Cursor = Cursors.Hand;
            dt.ValueChanged += delegate { p.Invalidate(); };
            dt.Enter += delegate { exFocus.Add(p); ExSync(p); };
            dt.Leave += delegate { exFocus.Remove(p); ExSync(p); };
        }

        return p;
    }

    private void ExFieldClick(object sender, EventArgs e)
    {
        Panel p = sender as Panel;
        if (p == null) return;
        ExTag t = p.Tag as ExTag;
        if (t == null || t.Box == null || !t.Box.Enabled) return;

        DateTimePicker dt = t.Box as DateTimePicker;
        if (dt != null) { ExCalOpen(dt, p); return; }
        t.Box.Focus();
    }

    private void ExGlyphSearch(Graphics g, int x, int y, Color c)
    {
        int s = P(1.8f);
        using (Pen p = new Pen(c, s))
        {
            p.StartCap = LineCap.Round; p.EndCap = LineCap.Round;
            g.DrawEllipse(p, x, y, P(9), P(9));
            g.DrawLine(p, x + P(8), y + P(8), x + P(13), y + P(13));
        }
    }

    private void ExGlyphCal(Graphics g, int x, int y, Color c)
    {
        using (Pen p = new Pen(c, P(1.5f)))
        {
            Rectangle r = new Rectangle(x, y + P(2), P(12), P(11));
            using (GraphicsPath path = ExPath(r, P(3))) g.DrawPath(p, path);
            g.DrawLine(p, x + 1, y + P(6), x + P(11), y + P(6));
            g.DrawLine(p, x + P(3), y, x + P(3), y + P(3));
            g.DrawLine(p, x + P(9), y, x + P(9), y + P(3));
        }
    }

    // ---------------- calendar ----------------

    private int CalPad { get { return P(10); } }
    private int CalCW { get { return P(32); } }
    private int CalCH { get { return P(30); } }
    private int CalHead { get { return P(66); } }

    private void ExCalOpen(DateTimePicker dt, Control anchor)
    {
        if (exCal != null) { ExCalClose(); return; }

        exCalTarget = dt;
        exCalMonth = dt.Value;
        exCalHover = -1;

        int w = CalPad * 2 + 7 * CalCW;
        int h = CalHead + 6 * CalCH + CalPad;

        exCalPanel = new Panel { BackColor = ExCard, Size = new Size(w, h) };
        exCalPanel.Paint += ExCalPaint;
        exCalPanel.MouseMove += ExCalMove;
        exCalPanel.MouseLeave += delegate { exCalHover = -1; exCalPanel.Invalidate(); };
        exCalPanel.MouseClick += ExCalClick;
        exCalPanel.MouseWheel += ExCalWheel;

        ToolStripControlHost host = new ToolStripControlHost(exCalPanel);
        host.Margin = Padding.Empty;
        host.Padding = Padding.Empty;

        exCal = new ToolStripDropDown
        {
            AutoSize = true,
            Padding = Padding.Empty,
            BackColor = ExCard,
            DropShadowEnabled = true
        };
        exCal.Items.Add(host);
        exCal.Closed += delegate
        {
            exCal = null; exCalPanel = null; exCalTarget = null; exCalHover = -1;
            if (exFDate1 != null) exFDate1.Invalidate();
            if (exFDate2 != null) exFDate2.Invalidate();
        };

        exCal.Show(anchor, new Point(0, anchor.Height + P(4)));
    }

    private void ExCalClose() { if (exCal != null) exCal.Close(); }

    private void ExCalWheel(object sender, MouseEventArgs e)
    {
        exCalMonth = exCalMonth.AddMonths(e.Delta > 0 ? -1 : 1);
        if (exCalPanel != null) exCalPanel.Invalidate();
    }

    private void ExCalMove(object sender, MouseEventArgs e)
    {
        if (exCalPanel == null) return;
        int idx = ExCalIndex(e.X, e.Y);
        if (idx != exCalHover) { exCalHover = idx; exCalPanel.Invalidate(); }
    }

    private int ExCalIndex(int x, int y)
    {
        int col = (x - CalPad) / CalCW;
        int row = (y - CalHead) / CalCH;
        if (col < 0 || col > 6 || row < 0 || row > 5) return -1;
        return row * 7 + col;
    }

    private void ExCalClick(object sender, MouseEventArgs e)
    {
        if (exCalPanel == null) return;

        if (e.Y >= P(8) && e.Y <= P(40))
        {
            if (e.X >= CalPad && e.X <= CalPad + P(28))
            { exCalMonth = exCalMonth.AddMonths(-1); exCalPanel.Invalidate(); return; }
            if (e.X >= exCalPanel.Width - CalPad - P(28) && e.X <= exCalPanel.Width - CalPad)
            { exCalMonth = exCalMonth.AddMonths(1); exCalPanel.Invalidate(); return; }
        }

        int idx = ExCalIndex(e.X, e.Y);
        if (idx < 0 || exCalTarget == null) return;

        DateTime first = new DateTime(exCalMonth.Year, exCalMonth.Month, 1);
        DateTime day = first.AddDays(idx - (int)first.DayOfWeek);
        try { exCalTarget.Value = day; } catch { }
        ExCalClose();
    }

    private void ExCalPaint(object sender, PaintEventArgs e)
    {
        Panel p = sender as Panel;
        if (p == null) return;
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle r = new Rectangle(0, 0, p.Width - 1, p.Height - 1);
        using (GraphicsPath path = ExPath(r, P(12)))
        {
            using (SolidBrush b = new SolidBrush(ExCard)) g.FillPath(b, path);
            using (Pen pen = new Pen(ExLine, P(1.2f))) g.DrawPath(pen, path);
        }

        using (Pen ch = new Pen(ExMuted, P(1.8f)))
        {
            ch.StartCap = LineCap.Round; ch.EndCap = LineCap.Round;
            g.DrawLine(ch, CalPad + P(8), P(24), CalPad + P(14), P(18));
            g.DrawLine(ch, CalPad + P(14), P(18), CalPad + P(20), P(24));
            int nx = p.Width - CalPad - P(20);
            g.DrawLine(ch, nx, P(18), nx + P(6), P(24));
            g.DrawLine(ch, nx + P(6), P(24), nx + P(12), P(18));
        }

        TextRenderer.DrawText(g, exCalMonth.ToString("MMMM yyyy"), ExFS(13f, FontStyle.Bold),
            new Rectangle(CalPad + P(30), P(12), p.Width - CalPad * 2 - P(60), P(22)), ExInk,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

        string[] wd = { "S", "M", "T", "W", "T", "F", "S" };
        for (int i = 0; i < 7; i++)
            TextRenderer.DrawText(g, wd[i], ExFS(11f, FontStyle.Bold),
                new Rectangle(CalPad + i * CalCW, P(42), CalCW, P(20)), ExMuted,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

        DateTime first = new DateTime(exCalMonth.Year, exCalMonth.Month, 1);
        int offset = (int)first.DayOfWeek;
        DateTime today = DateTime.Today;
        DateTime sel = exCalTarget != null ? exCalTarget.Value.Date : DateTime.MinValue;

        for (int i = 0; i < 42; i++)
        {
            int col = i % 7, row = i / 7;
            Rectangle cell = new Rectangle(CalPad + col * CalCW, CalHead + row * CalCH, CalCW, CalCH);
            DateTime day = first.AddDays(i - offset);

            bool outMonth = day.Month != exCalMonth.Month;
            bool isSel = day.Date == sel;
            bool isToday = day.Date == today;
            bool hot = i == exCalHover;

            if (isSel || hot)
            {
                Rectangle cr = Rectangle.Inflate(cell, -P(3), -P(3));
                using (GraphicsPath cp = ExPath(cr, P(8)))
                using (SolidBrush cb = new SolidBrush(isSel ? ExLime : Color.FromArgb(234, 245, 255)))
                    g.FillPath(cb, cp);
            }

            if (isToday && !isSel)
            {
                Rectangle cr = Rectangle.Inflate(cell, -P(3), -P(3));
                using (GraphicsPath cp = ExPath(cr, P(8)))
                using (Pen tp = new Pen(ExCyan, P(1.3f))) g.DrawPath(tp, cp);
            }

            Color tc = isSel ? ExInk : (outMonth ? Color.FromArgb(178, 192, 210) : ExText);
            TextRenderer.DrawText(g, day.Day.ToString(),
                ExFS(12f, isSel ? FontStyle.Bold : FontStyle.Regular),
                cell, tc, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }
    }

    // ---------------- paints ----------------

    private void ExBgPaint(object sender, PaintEventArgs e)
    {
        Rectangle r = new Rectangle(0, 0, Width, Height);
        if (r.Width < 2 || r.Height < 2) return;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(242, 247, 255), Color.FromArgb(216, 231, 250), LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, r);
    }

    private void ExFieldPaint(object sender, PaintEventArgs e)
    {
        Panel p = sender as Panel;
        if (p == null || p.Width < 8) return;
        ExTag t = p.Tag as ExTag;
        bool off = (t != null && t.Box != null && !t.Box.Enabled);
        float g = exGlow.ContainsKey(p) ? exGlow[p] : 0f;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, p.Width - 1, p.Height - 1);

        using (GraphicsPath path = ExPath(r, P(10)))
        {
            using (SolidBrush b = new SolidBrush(off ? ExFieldOff : ExFieldBg)) e.Graphics.FillPath(b, path);
            if (g > 0.03f && !off)
            {
                Rectangle ri = Rectangle.Inflate(r, -P(2), -P(2));
                using (GraphicsPath gp = ExPath(ri, P(9)))
                using (Pen glow = new Pen(Color.FromArgb((int)(48 * Math.Min(1f, g)), ExCyan), P(3.2f)))
                    e.Graphics.DrawPath(glow, gp);
            }
            using (Pen pen = new Pen(off ? ExLine : ExBlend(ExLine, ExCyan, Math.Min(1f, g)),
                P(1f) + Math.Min(1f, g) * P(0.9f)))
                e.Graphics.DrawPath(pen, path);
        }

        Color gc = off ? Color.FromArgb(170, 186, 208) : ExBlend(ExMuted, ExCyan, Math.Min(1f, g));
        int gy = (p.Height - P(14)) / 2;
        if (t != null && t.Glyph == "search") ExGlyphSearch(e.Graphics, P(13), gy, gc);
        else if (t != null && t.Glyph == "cal") ExGlyphCal(e.Graphics, P(12), gy, gc);
        else if (t != null && t.Glyph == "user")
            using (Pen pen = new Pen(gc, P(1.4f)))
            {
                e.Graphics.DrawEllipse(pen, P(13), gy, P(9), P(9));
                e.Graphics.DrawArc(pen, P(9), gy + P(11), P(17), P(12), 200, 140);
            }
        else if (t != null && t.Glyph == "lock")
            using (Pen pen = new Pen(gc, P(1.4f)))
            {
                e.Graphics.DrawArc(pen, P(12), gy - P(1), P(11), P(11), 180, 180);
                using (GraphicsPath path = ExPath(new Rectangle(P(10), gy + P(5), P(15), P(11)), P(3)))
                    e.Graphics.DrawPath(pen, path);
            }
        else if (t != null && t.Glyph == "phone")
            using (Pen pen = new Pen(gc, P(1.4f)))
            using (GraphicsPath path = ExPath(new Rectangle(P(13), gy - P(1), P(10), P(17)), P(3)))
                e.Graphics.DrawPath(pen, path);
        else if (t != null && t.Glyph == "mail")
            using (Pen pen = new Pen(gc, P(1.4f)))
            {
                using (GraphicsPath path = ExPath(new Rectangle(P(11), gy, P(16), P(12)), P(2)))
                    e.Graphics.DrawPath(pen, path);
                e.Graphics.DrawLine(pen, P(11), gy + 1, P(19), gy + P(6));
                e.Graphics.DrawLine(pen, P(27), gy + 1, P(19), gy + P(6));
            }

        DateTimePicker dt = t != null ? t.Box as DateTimePicker : null;
        if (dt != null)
            TextRenderer.DrawText(e.Graphics, dt.Value.ToString("dd-MMM-yyyy"), ExFS(12.5f, FontStyle.Regular),
                new Rectangle(P(32), 0, p.Width - P(40), p.Height), off ? ExMuted : ExText,
                TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
    }

    private void ExRangePaint(object sender, PaintEventArgs e)
    {
        Panel p = sender as Panel;
        if (p == null || p.Width < 8) return;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, p.Width - 1, p.Height - 1);
        using (GraphicsPath path = ExPath(r, P(11)))
        {
            using (SolidBrush b = new SolidBrush(ExCard)) e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(ExLine, P(1.1f))) e.Graphics.DrawPath(pen, path);
        }
        if (exPresets.Count > 0 && exPresets[0].Visible)
            using (Pen sep = new Pen(ExLine, P(1f)))
                e.Graphics.DrawLine(sep, P(14), p.Height - P(30), p.Width - P(14), p.Height - P(30));
    }

    private void ExDropPaint(object sender, PaintEventArgs e)
    {
        Panel p = sender as Panel;
        if (p == null || p.Width < 8) return;
        float g = exGlow.ContainsKey(p) ? exGlow[p] : 0f;
        bool open = exMenu != null;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, p.Width - 1, p.Height - 1);

        using (GraphicsPath path = ExPath(r, P(9)))
        {
            Color fill = open ? Color.FromArgb(232, 244, 255)
                       : ExBlend(ExFieldBg, Color.FromArgb(232, 244, 255), Math.Min(1f, g));
            using (SolidBrush b = new SolidBrush(fill)) e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(ExBlend(ExLine, ExCyan, Math.Min(1f, g)), P(1f) + Math.Min(1f, g) * P(0.8f)))
                e.Graphics.DrawPath(pen, path);
        }

        string val = CmbDays.Text;
        if (string.IsNullOrEmpty(val) && CmbDays.Items.Count > 0) val = Convert.ToString(CmbDays.Items[0]);
        TextRenderer.DrawText(e.Graphics, val, ExFS(12.5f, FontStyle.Bold),
            new Rectangle(0, 0, p.Width - P(20), p.Height), open ? ExCyan : ExText,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

        int cx = p.Width - P(15), cy = p.Height / 2 - (open ? 1 : P(2));
        using (Pen chev = new Pen(open ? ExCyan : ExMuted, P(1.7f)))
        {
            chev.StartCap = LineCap.Round; chev.EndCap = LineCap.Round;
            if (open)
            {
                e.Graphics.DrawLine(chev, cx - P(4), cy + P(2), cx, cy - P(2));
                e.Graphics.DrawLine(chev, cx, cy - P(2), cx + P(4), cy + P(2));
            }
            else
            {
                e.Graphics.DrawLine(chev, cx - P(4), cy - P(2), cx, cy + P(2));
                e.Graphics.DrawLine(chev, cx, cy + P(2), cx + P(4), cy - P(2));
            }
        }
    }

    private void ExMenuPaint(object sender, PaintEventArgs e)
    {
        Panel p = sender as Panel;
        if (p == null) return;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, p.Width - 1, p.Height - 1);

        using (GraphicsPath path = ExPath(r, P(9)))
        {
            using (SolidBrush b = new SolidBrush(ExCard)) e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(ExLine, P(1.2f))) e.Graphics.DrawPath(pen, path);
        }

        int rowH = P(30);
        Font f = ExFS(12.5f, FontStyle.Regular);
        for (int i = 0; i < CmbDays.Items.Count; i++)
        {
            Rectangle row = new Rectangle(P(6), P(5) + i * rowH, p.Width - P(12), rowH);
            bool sel = CmbDays.SelectedIndex == i;
            if (i == exMenuHover || sel)
                using (GraphicsPath rp = ExPath(row, P(7)))
                using (SolidBrush rb = new SolidBrush(sel ? ExLime : Color.FromArgb(234, 245, 255)))
                    e.Graphics.FillPath(rb, rp);

            TextRenderer.DrawText(e.Graphics, Convert.ToString(CmbDays.Items[i]), f,
                new Rectangle(row.X + P(10), row.Y, row.Width - P(20), row.Height),
                sel ? ExInk : ExText, TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }
    }

    private void ExMenuMove(object sender, MouseEventArgs e)
    {
        if (exMenuList == null) return;
        int idx = (e.Y - P(5)) / P(30);
        if (idx < 0 || idx >= CmbDays.Items.Count) idx = -1;
        if (idx != exMenuHover) { exMenuHover = idx; exMenuList.Invalidate(); }
    }

    private void ExMenuClick(object sender, MouseEventArgs e)
    {
        int idx = (e.Y - P(5)) / P(30);
        if (idx < 0 || idx >= CmbDays.Items.Count) return;
        CmbDays.SelectedIndex = idx;
        CmbDays.Text = Convert.ToString(CmbDays.Items[idx]);
        if (exMenu != null) exMenu.Close();
        if (exDrop != null) exDrop.Invalidate();
    }

    private void ExDropOpen()
    {
        if (exMenu != null) { exMenu.Close(); return; }
        int n = CmbDays.Items.Count;
        if (n == 0) return;

        int w = Math.Max(P(120), exDrop.Width + P(20));
        exMenuHover = -1;

        exMenuList = new Panel { BackColor = ExCard, Size = new Size(w, n * P(30) + P(10)) };
        exMenuList.Paint += ExMenuPaint;
        exMenuList.MouseMove += ExMenuMove;
        exMenuList.MouseLeave += delegate { exMenuHover = -1; exMenuList.Invalidate(); };
        exMenuList.MouseClick += ExMenuClick;

        ToolStripControlHost host = new ToolStripControlHost(exMenuList);
        host.Margin = Padding.Empty;
        host.Padding = Padding.Empty;

        exMenu = new ToolStripDropDown
        {
            AutoSize = true,
            Padding = Padding.Empty,
            BackColor = ExCard,
            DropShadowEnabled = true
        };
        exMenu.Items.Add(host);
        exMenu.Closed += delegate { exMenu = null; exMenuList = null; exMenuHover = -1; if (exDrop != null) exDrop.Invalidate(); };
        exMenu.Show(exDrop, new Point(0, exDrop.Height + P(4)));
        exDrop.Invalidate();
    }

    private void ExCardPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null || c.Width < 6) return;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);
        using (GraphicsPath path = ExPath(r, P(10)))
        {
            using (SolidBrush b = new SolidBrush(ExCard)) e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(ExLine, P(1.2f))) e.Graphics.DrawPath(pen, path);
        }
    }

    private void ExGroup(GroupBox g)
    {
        if (g == null) return;
        g.BackColor = ExBg;
        g.ForeColor = ExText;
        g.Font = ExFS(12f, FontStyle.Bold);
        g.Paint -= ByteNovaGroupPaint;
        g.Paint += ExGroupPaint;
    }

    private void ExGroupPaint(object sender, PaintEventArgs e)
    {
        GroupBox g = sender as GroupBox;
        if (g == null || g.Width < 20) return;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, g.Width - 1, g.Height - 1);

        using (GraphicsPath path = ExPath(r, P(11)))
        {
            using (SolidBrush b = new SolidBrush(ExCard)) e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(ExLine, P(1.2f))) e.Graphics.DrawPath(pen, path);
        }

        using (SolidBrush lime = new SolidBrush(ExLime))
            e.Graphics.FillRectangle(lime, P(18), P(14), P(3), P(13));

        TextRenderer.DrawText(e.Graphics, g.Text, g.Font, new Point(P(28), P(13)), ExInk,
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
    }

    private void ExLabel(Label l, Color c, float px, bool bold)
    {
        if (l == null) return;
        l.AutoSize = false;
        l.BackColor = Color.Transparent;
        l.ForeColor = c;
        l.TextAlign = ContentAlignment.MiddleLeft;
        l.Font = ExF(px, bold ? FontStyle.Bold : FontStyle.Regular);
    }

    // ---------------- stat badges ----------------

    private void ExStat(Label cap, Label val, Color accent, string icon)
    {
        if (cap == null || val == null) return;

        cap.AutoSize = false;
        cap.BackColor = Color.Transparent;
        cap.ForeColor = ExMuted;
        cap.TextAlign = ContentAlignment.MiddleLeft;
        cap.Font = ExF(11.5f, FontStyle.Regular);
        cap.Paint -= ExStatCapPaint;
        cap.Paint += ExStatCapPaint;

        val.AutoSize = false;
        val.BackColor = Color.Transparent;
        val.ForeColor = accent;
        val.TextAlign = ContentAlignment.MiddleCenter;
        val.Font = ExFS(12f, FontStyle.Bold);
        val.Paint -= ExStatValPaint;
        val.Paint += ExStatValPaint;

        exStats.Add(new Label[] { cap, val });
        exStatColor[cap] = accent;
        exStatIcon[cap] = icon;
    }

    private void ExStatCapPaint(object sender, PaintEventArgs e)
    {
        Label l = sender as Label;
        if (l == null || l.Width < 8) return;

        Color accent;
        string icon;
        if (!exStatColor.TryGetValue(l, out accent)) accent = ExCyan;
        if (!exStatIcon.TryGetValue(l, out icon)) icon = "dot";

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, l.Width - 1, l.Height - 1);

        using (GraphicsPath path = ExPath(r, r.Height / 2))
        {
            using (SolidBrush b = new SolidBrush(ExBlend(accent, Color.White, 0.90f))) e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(ExBlend(accent, Color.White, 0.62f), P(1f))) e.Graphics.DrawPath(pen, path);
        }

        ExStatIcon(e.Graphics, icon, new Rectangle(P(9), (l.Height - P(11)) / 2, P(11), P(11)), accent);

        string txt = l.Text.Trim().TrimEnd(':').Trim();
        TextRenderer.DrawText(e.Graphics, txt, l.Font, new Rectangle(P(25), 0, l.Width - P(30), l.Height), ExInk,
            TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix |
            TextFormatFlags.EndEllipsis);
    }

    private void ExStatValPaint(object sender, PaintEventArgs e)
    {
        Label l = sender as Label;
        if (l == null || l.Width < 6) return;

        Color accent = ExCyan;
        for (int i = 0; i < exStats.Count; i++)
            if (exStats[i][1] == l) accent = exStatColor[exStats[i][0]];

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, l.Width - 1, l.Height - 1);

        using (GraphicsPath path = ExPath(r, P(7)))
        using (LinearGradientBrush gb = new LinearGradientBrush(
            r, ExLight(accent, 20), accent, LinearGradientMode.Vertical))
            e.Graphics.FillPath(gb, path);

        TextRenderer.DrawText(e.Graphics, l.Text, l.Font, r, Color.White,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }

    private void ExStatIcon(Graphics g, string kind, Rectangle r, Color c)
    {
        using (Pen p = new Pen(c, P(1.4f)))
        {
            p.StartCap = LineCap.Round; p.EndCap = LineCap.Round;
            switch (kind)
            {
                case "check":
                    g.DrawLine(p, r.X + 1, r.Y + P(6), r.X + P(4), r.Y + P(9));
                    g.DrawLine(p, r.X + P(4), r.Y + P(9), r.X + P(10), r.Y + P(2));
                    break;
                case "minus":
                    g.DrawLine(p, r.X + 1, r.Y + P(6), r.X + P(10), r.Y + P(6));
                    break;
                case "slash":
                    g.DrawEllipse(p, r.X, r.Y + 1, P(10), P(10));
                    g.DrawLine(p, r.X + P(2), r.Y + P(9), r.X + P(9), r.Y + P(2));
                    break;
                case "x":
                    g.DrawLine(p, r.X + 1, r.Y + 1, r.X + P(10), r.Y + P(10));
                    g.DrawLine(p, r.X + P(10), r.Y + 1, r.X + 1, r.Y + P(10));
                    break;
                case "ticket":
                    using (GraphicsPath tp = ExPath(new Rectangle(r.X, r.Y + P(2), P(11), P(8)), P(2))) g.DrawPath(p, tp);
                    break;
                case "grid":
                    for (int i = 0; i < 2; i++)
                        for (int j = 0; j < 2; j++)
                            g.DrawRectangle(p, r.X + j * P(6), r.Y + 1 + i * P(6), P(4), P(4));
                    break;
                default:
                    g.DrawEllipse(p, r.X + P(3), r.Y + P(2), P(5), P(5));
                    g.DrawLine(p, r.X + P(5), r.Y + P(8), r.X + P(5), r.Y + P(9));
                    break;
            }
        }
    }

    // ---------------- chips / checks / radios ----------------

    private void ExChip(Label l)
    {
        if (l == null) return;
        l.AutoSize = false;
        l.BackColor = Color.White;
        l.ForeColor = ExText;
        l.TextAlign = ContentAlignment.MiddleCenter;
        l.Font = ExFS(11f, FontStyle.Bold);
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
            using (Pen pen = new Pen(ExLine, P(1.1f))) e.Graphics.DrawPath(pen, path);
        }
        TextRenderer.DrawText(e.Graphics, l.Text, l.Font, r, ExText,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }

    private void ExPresetPaint(object sender, PaintEventArgs e)
    {
        Label l = sender as Label;
        if (l == null || l.Width < 6) return;
        float g = exGlow.ContainsKey(l) ? exGlow[l] : 0f;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, l.Width - 1, l.Height - 1);

        using (GraphicsPath path = ExPath(r, r.Height / 2))
        {
            using (SolidBrush b = new SolidBrush(ExBlend(Color.FromArgb(244, 249, 255), ExCyan, Math.Min(1f, g))))
                e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(ExBlend(ExLine, ExCyan, Math.Min(1f, g)), P(1.1f)))
                e.Graphics.DrawPath(pen, path);
        }

        TextRenderer.DrawText(e.Graphics, l.Text, l.Font, r, g > 0.4f ? Color.White : ExMuted,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }

    private void ExCheck(CheckBox c)
    {
        if (c == null) return;
        c.AutoSize = false;
        c.BackColor = Color.Transparent;
        c.ForeColor = ExText;
        c.FlatStyle = FlatStyle.Flat;
        c.Font = ExF(12f, FontStyle.Regular);
        c.Cursor = Cursors.Hand;
        c.Paint -= ExCheckPaint;
        c.Paint += ExCheckPaint;
        c.MouseEnter += delegate { exHover.Add(c); ExSync(c); };
        c.MouseLeave += delegate { exHover.Remove(c); ExSync(c); };
        c.CheckedChanged += delegate { c.Invalidate(); };
    }

    private void ExCheckPaint(object sender, PaintEventArgs e)
    {
        CheckBox c = sender as CheckBox;
        if (c == null) return;
        float g = exGlow.ContainsKey(c) ? exGlow[c] : 0f;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle box = new Rectangle(1, (c.Height - P(15)) / 2, P(15), P(15));

        using (GraphicsPath path = ExPath(box, P(4)))
        {
            Color fill = c.Checked ? ExLime : ExBlend(Color.White, Color.FromArgb(228, 240, 254), Math.Min(1f, g));
            using (SolidBrush b = new SolidBrush(fill)) e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(c.Checked ? Color.FromArgb(140, 190, 30)
                : ExBlend(ExLine, ExCyan, Math.Min(1f, g)), P(1.2f)))
                e.Graphics.DrawPath(pen, path);
        }

        if (c.Checked)
            using (Pen tick = new Pen(Color.FromArgb(20, 45, 25), P(1.9f)))
            {
                tick.StartCap = LineCap.Round; tick.EndCap = LineCap.Round;
                e.Graphics.DrawLine(tick, box.X + P(4), box.Y + P(8), box.X + P(6), box.Y + P(11));
                e.Graphics.DrawLine(tick, box.X + P(6), box.Y + P(11), box.X + P(11), box.Y + P(4));
            }

        TextRenderer.DrawText(e.Graphics, c.Text, c.Font,
            new Rectangle(P(22), 0, c.Width - P(24), c.Height), ExText,
            TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
    }

    private void ExRadio(RadioButton r)
    {
        if (r == null) return;
        r.AutoSize = false;
        r.BackColor = Color.Transparent;
        r.ForeColor = ExText;
        r.Font = ExFS(12f, FontStyle.Bold);
        r.Cursor = Cursors.Hand;
        r.Paint -= ExRadioPaint;
        r.Paint += ExRadioPaint;
        r.MouseEnter += delegate { exHover.Add(r); ExSync(r); };
        r.MouseLeave += delegate { exHover.Remove(r); ExSync(r); };
        r.CheckedChanged += delegate { r.Invalidate(); if (r.Parent != null) r.Parent.Invalidate(); };
    }

    private void ExRadioPaint(object sender, PaintEventArgs e)
    {
        RadioButton r = sender as RadioButton;
        if (r == null || r.Width < 8) return;
        float g = exGlow.ContainsKey(r) ? exGlow[r] : 0f;
        bool on = r.Checked;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle box = new Rectangle(0, 0, r.Width - 1, r.Height - 1);

        using (GraphicsPath path = ExPath(box, r.Height / 2))
        {
            Color fill = on ? ExLime : ExBlend(Color.White, Color.FromArgb(228, 240, 254), Math.Min(1f, g));
            using (SolidBrush b = new SolidBrush(fill)) e.Graphics.FillPath(b, path);
            using (Pen pen = new Pen(on ? Color.FromArgb(140, 190, 30)
                : ExBlend(ExLine, ExCyan, Math.Min(1f, g)), P(1.2f)))
                e.Graphics.DrawPath(pen, path);
        }

        int cy = r.Height / 2;
        using (SolidBrush dot = new SolidBrush(on ? Color.FromArgb(20, 45, 25) : Color.FromArgb(150, 175, 205)))
            e.Graphics.FillEllipse(dot, P(9), cy - P(4), P(8), P(8));

        TextRenderer.DrawText(e.Graphics, r.Text, r.Font,
            new Rectangle(P(23), 0, r.Width - P(29), r.Height),
            on ? Color.FromArgb(16, 42, 22) : ExText,
            TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
    }

    private void ExGrid()
    {
        grdUsers.EnableHeadersVisualStyles = false;
        grdUsers.BorderStyle = BorderStyle.None;
        grdUsers.BackgroundColor = Color.White;
        grdUsers.GridColor = Color.FromArgb(226, 236, 248);
        grdUsers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grdUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grdUsers.RowHeadersVisible = false;
        grdUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grdUsers.ColumnHeadersHeight = P(34);
        grdUsers.RowTemplate.Height = P(30);
        grdUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grdUsers.SelectionMode = DataGridViewSelectionMode.CellSelect;

        grdUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(226, 238, 252);
        grdUsers.ColumnHeadersDefaultCellStyle.ForeColor = ExText;
        grdUsers.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(226, 238, 252);
        grdUsers.ColumnHeadersDefaultCellStyle.SelectionForeColor = ExText;
        grdUsers.ColumnHeadersDefaultCellStyle.Font = ExFS(12f, FontStyle.Bold);
        grdUsers.ColumnHeadersDefaultCellStyle.Padding = new Padding(P(8), 0, P(4), 0);

        grdUsers.DefaultCellStyle.BackColor = Color.White;
        grdUsers.DefaultCellStyle.ForeColor = Color.FromArgb(24, 46, 74);
        grdUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(214, 234, 255);
        grdUsers.DefaultCellStyle.SelectionForeColor = Color.FromArgb(12, 34, 60);
        grdUsers.DefaultCellStyle.Font = ExF(12f, FontStyle.Regular);
        grdUsers.DefaultCellStyle.Padding = new Padding(P(8), 0, P(4), 0);

        grdUsers.RowsDefaultCellStyle.BackColor = Color.White;
        grdUsers.RowsDefaultCellStyle.ForeColor = Color.FromArgb(24, 46, 74);
        grdUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 251, 255);
        grdUsers.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(24, 46, 74);

        foreach (DataGridViewColumn col in grdUsers.Columns)
        {
            col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            col.MinimumWidth = P(58);
            string n = col.Name.ToLower();
            if (n.Contains("id")) col.FillWeight = 120;
            else if (n.Contains("pwd")) col.FillWeight = 130;
            else if (n.Contains("check")) col.FillWeight = 100;
            else col.FillWeight = 78;

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

    // ---------------- buttons ----------------

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
        b.Font = ExFS(12f, FontStyle.Bold);
        b.Cursor = Cursors.Hand;
        b.Tag = accent;

        if (filled) exFill.Add(b);

        b.Paint -= ExBtnPaint;
        b.Paint += ExBtnPaint;
        b.MouseEnter += delegate { exHover.Add(b); ExSync(b); };
        b.MouseLeave += delegate { exHover.Remove(b); exPress.Remove(b); ExSync(b); };
        b.MouseDown += delegate { exPress.Add(b); ExSync(b); };
        b.MouseUp += delegate { exPress.Remove(b); ExSync(b); };
    }

    private void ExBtnPaint(object sender, PaintEventArgs e)
    {
        Button b = sender as Button;
        if (b == null || b.Width < 6) return;

        Color accent = (b.Tag is Color) ? (Color)b.Tag : Color.FromArgb(60, 140, 240);
        bool filled = exFill.Contains(b);
        bool down = exPress.Contains(b);
        float g = exGlow.ContainsKey(b) ? exGlow[b] : 0f;
        float h = Math.Min(1f, g / 0.5f);

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, b.Width - 1, b.Height - 1);
        if (down) r = Rectangle.Inflate(r, -P(2), -P(2));

        Color fill;
        if (filled) fill = down ? ExDark(accent, 42) : ExBlend(accent, Color.White, h * 0.35f);
        else fill = down ? ExBlend(Color.White, accent, 0.30f) : ExBlend(Color.White, accent, h * 0.14f);

        Color border = down ? ExDark(accent, 22) : ExBlend(accent, Color.White, 0.25f * (1f - h));
        Color text = filled ? (down ? Color.White : Color.FromArgb(16, 40, 20)) : (down ? Color.White : accent);

        if (h > 0.05f && !down)
        {
            Rectangle ri = Rectangle.Inflate(r, -P(1), -P(1));
            using (GraphicsPath gp = ExPath(ri, P(9)))
            using (Pen glow = new Pen(Color.FromArgb((int)(70 * h), accent), P(3f)))
                e.Graphics.DrawPath(glow, gp);
        }

        using (GraphicsPath path = ExPath(r, P(9)))
        {
            using (LinearGradientBrush gb = new LinearGradientBrush(
                r, ExLight(fill, 8), fill, LinearGradientMode.Vertical))
                e.Graphics.FillPath(gb, path);
            using (Pen pen = new Pen(border, down ? P(1.6f) : P(1.3f)))
                e.Graphics.DrawPath(pen, path);
        }

        Rectangle tr = r;
        if (down) tr.Offset(0, P(1));

        TextRenderer.DrawText(e.Graphics, b.Text, b.Font, tr, text,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
            TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.WordBreak);
    }

    // ---------------- layout ----------------

    private void ExLayout()
    {
        if (!exBuilt || IsDisposed) return;

        int W = ClientSize.Width;
        int H = ClientSize.Height;
        if (W < P(620) || H < P(430)) return;

        int M = P(16);
        int GX = P(18);
        int ROW = P(40);

        int right = W - M;
        int innerW = right - M;
        int gw = innerW - GX * 2;

        bool compact = H < P(640);

        int footH = P(78);
        int footY = H - footH - P(6);
        int searchH = compact ? P(176) : P(202);
        int searchY = footY - P(8) - searchH;

        int topY = P(10);
        int topH = P(168);
        int gridY = topY + topH + P(10);
        int gridH = searchY - P(10) - gridY;
        if (gridH < P(96)) gridH = P(96);

        int bulkW = P(240);
        int bulkX = right - bulkW;
        int gridW = bulkX - P(12) - M;

        SuspendLayout();
        try
        {
            ExPut(grpAddID, M, topY, innerW, topH);
            ExRound(grpAddID, P(11));

            int row1 = P(34), row2 = row1 + ROW + P(4), rowCk = row2 + ROW + P(8);

            int btnW = P(190);
            int btnX = M + innerW - GX - btnW;
            int maxEnd = btnX - M - P(18);

            int l1 = ExTextW(label37) + P(8), l2 = ExTextW(lblPassword) + P(8);
            int f1 = P(200), f2 = P(230);
            int need = GX + l1 + P(8) + f1 + P(26) + l2 + P(8) + f2;
            if (need > maxEnd - GX) f2 = Math.Max(P(140), f2 - (need - (maxEnd - GX)));

            int x = GX;
            ExPut(label37, x, row1 + P(9), l1, P(22)); x += l1 + P(8);
            ExPut(exFId, x, row1, f1, ROW); ExPut(txtLogin, P(40), P(9), f1 - P(54), P(22)); x += f1 + P(26);
            ExPut(lblPassword, x, row1 + P(9), l2, P(22)); x += l2 + P(8);
            ExPut(exFPwd, x, row1, f2, ROW); ExPut(txtPassword, P(40), P(9), f2 - P(54), P(22));

            int l3 = ExTextW(label4) + P(8), l4 = ExTextW(label5) + P(8);
            int f3 = P(200), f4 = P(210);
            int need2 = GX + l3 + P(8) + f3 + P(26) + l4 + P(8) + f4;
            if (need2 > maxEnd - GX) f4 = Math.Max(P(130), f4 - (need2 - (maxEnd - GX)));

            x = GX;
            ExPut(label4, x, row2 + P(9), l3, P(22)); x += l3 + P(8);
            ExPut(exFMob, x, row2, f3, ROW); ExPut(textmob, P(40), P(9), f3 - P(54), P(22)); x += f3 + P(26);
            ExPut(label5, x, row2 + P(9), l4, P(22)); x += l4 + P(8);
            ExPut(exFPass, x, row2, f4, ROW); ExPut(textmpass, P(40), P(9), f4 - P(54), P(22));

            label5.Visible = textmpass.Visible;
            exFPass.Visible = textmpass.Visible;

            int ckX = GX;
            int ck1 = ExTextW(chkValidate) + P(24);
            ExPut(chkValidate, ckX, rowCk, ck1, P(24)); ckX += ck1 + P(30);
            int ck2 = ExTextW(chkShowPwd) + P(24);
            ExPut(chkShowPwd, ckX, rowCk, ck2, P(24)); ckX += ck2 + P(30);
            ExPut(checkBox1, ckX, rowCk, ExTextW(checkBox1) + P(24), P(24));

            ExPut(btnIDSave, btnX, row1, btnW, P(46));
            ExRound(btnIDSave, P(10));
            ExPut(button5, btnX, row2, btnW, P(32));
            ExRound(button5, P(9));

            ExPut(label6, M, gridY, P(160), P(16));
            ExPut(grdUsers, M, gridY + P(18), gridW, gridH - P(18));
            ExRound(grdUsers, P(9));

            int bulkY = gridY + P(18);
            int bulkH = gridH - P(18);
            ExPut(exBulk, bulkX, bulkY, bulkW, bulkH);
            ExRound(exBulk, P(9));
            ExPut(txtbulk, P(12), P(10), bulkW - P(24), bulkH - P(46));
            ExPut(button4, bulkX + bulkW - P(80), bulkY + bulkH - P(30), P(68), P(24));
            ExRound(button4, P(7));

            ExPut(groupBox1, M, searchY, innerW, searchH);
            ExRound(groupBox1, P(11));

            int s1 = P(32), s2 = s1 + P(34), s3 = s2 + P(32);
            int rcH = compact ? P(40) : P(66);
            int s4 = s3 + rcH + P(8);

            x = GX;
            ExPut(btnChangePwd, x, s1, P(158), P(30)); ExRound(btnChangePwd, P(8)); x += P(166);
            ExPut(btnChangeQn, x, s1, P(158), P(30)); ExRound(btnChangeQn, P(8)); x += P(176);

            int l14 = ExTextW(label14) + P(8);
            ExPut(label14, x, s1 + P(4), l14, P(22)); x += l14 + P(10);

            ExPut(exDrop, x, s1, P(92), P(30)); ExRound(exDrop, P(9)); x += P(102);
            ExPut(btnHideDay, x, s1, P(104), P(30)); ExRound(btnHideDay, P(8));

            x = GX;
            int rb1 = ExTextW(RadioBdate) + P(24);
            ExPut(RadioBdate, x, s2, rb1, P(26)); x += rb1 + P(16);
            int rb2 = ExTextW(RadioJdate) + P(24);
            ExPut(RadioJdate, x, s2, rb2, P(26)); x += rb2 + P(16);
            ExPut(RadioPnr, x, s2, ExTextW(RadioPnr) + P(24), P(26));

            int rangeW = Math.Min(P(520), gw - P(400));
            if (rangeW < P(300)) rangeW = Math.Max(P(260), gw - P(330));
            int searchFldW = gw - rangeW - P(110) - P(90) - P(30);
            if (searchFldW < P(140)) searchFldW = P(140);

            ExPut(exRange, GX, s3, rangeW, rcH);
            ExRound(exRange, P(11));

            int pad = P(12), gap = P(28);
            int fw = (rangeW - pad * 2 - gap) / 2;
            int fh = P(34);
            int fy = (rcH >= P(60)) ? P(8) : (rcH - fh) / 2;

            ExPut(exFDate1, pad, fy, fw, fh); ExRound(exFDate1, P(9));
            ExPut(B2AAA, P(32), (fh - P(24)) / 2, fw - P(42), P(24));

            ExPut(exArrow, pad + fw + P(2), fy + P(8), P(24), P(18));

            ExPut(exFDate2, pad + fw + gap, fy, fw, fh); ExRound(exFDate2, P(9));
            ExPut(dtDate2, P(32), (fh - P(24)) / 2, fw - P(42), P(24));

            int px = pad;
            int py = fy + fh + P(6);
            bool showPresets = !compact && rcH >= P(60);
            foreach (Label pl in exPresets)
            {
                pl.Visible = showPresets;
                if (!showPresets) continue;
                using (Graphics gg = CreateGraphics())
                {
                    int pw = TextRenderer.MeasureText(gg, pl.Text, pl.Font, new Size(2000, 60), TF_MEASURE).Width + P(22);
                    ExPut(pl, px, py, pw, P(22));
                    ExRound(pl, P(11));
                    px += pw + P(8);
                }
            }

            int sx = GX + rangeW + P(14);
            ExPut(exFPnr, sx, s3, searchFldW, P(40)); ExRound(exFPnr, P(9));
            ExPut(txtPnr, P(40), P(9), searchFldW - P(54), P(22));

            ExPut(button2, sx + searchFldW + P(12), s3, P(104), P(40)); ExRound(button2, P(9));
            ExPut(C2AA0, sx + searchFldW + P(124), s3, P(84), P(40)); ExRound(C2AA0, P(9));

            ExPut(panel13, GX, s4, gw, P(26));
            ExRound(panel13, P(8));
            ExPut(label3, P(10), P(4), P(152), P(18));
            ExPut(lblCurMonbk, P(168), P(4), P(32), P(18));
            ExPut(label2, P(206), P(4), P(112), P(18));
            ExPut(B3AAA, P(322), P(4), P(216), P(18));
            ExPut(lbl7, P(544), P(4), P(32), P(18));

            int sxBadge = M;
            int syBadge = footY + P(6);
            for (int i = 0; i < exStats.Count; i++)
            {
                Label cap = exStats[i][0];
                Label val = exStats[i][1];

                int cw;
                using (Graphics gg = CreateGraphics())
                    cw = TextRenderer.MeasureText(gg, cap.Text.Trim().TrimEnd(':'), cap.Font,
                        new Size(2000, 60), TF_MEASURE).Width + P(36);
                if (cw < P(70)) cw = P(70);

                if (sxBadge + cw + P(40) > W - P(430)) { sxBadge = M; syBadge += P(32); }

                ExPut(cap, sxBadge, syBadge, cw, P(26));
                ExRound(cap, P(13));
                ExPut(val, sxBadge + cw + P(4), syBadge + P(2), P(30), P(22));
                ExRound(val, P(7));
                sxBadge += cw + P(4) + P(30) + P(8);
            }

            ExPut(lblMsg, sxBadge, syBadge + P(4), P(170), P(18));
            ExPut(pictureBox1, sxBadge + P(176), syBadge + P(1), P(24), P(24));

            ExPut(button3, W - P(419), footY + P(6), P(110), P(40)); ExRound(button3, P(10));
            ExPut(C1AA0, W - P(301), footY + P(6), P(115), P(40)); ExRound(C1AA0, P(10));
            ExPut(button1, W - P(178), footY + P(6), P(150), P(40)); ExRound(button1, P(10));

            ExPut(pnlloader, M + (gridW / 2) - P(216), gridY + (gridH / 2) - P(32), P(433), P(65));
            ExRound(pnlloader, P(11));

            if (txtLogin != null) txtLogin.BringToFront();
            if (txtPassword != null) txtPassword.BringToFront();
            if (textmob != null) textmob.BringToFront();
            if (textmpass != null) textmpass.BringToFront();
            if (txtPnr != null) txtPnr.BringToFront();
            if (txtbulk != null) txtbulk.BringToFront();
            if (exArrow != null) exArrow.BringToFront();

            panel13.BringToFront();
            pnlloader.BringToFront();
        }
        finally
        {
            ResumeLayout(false);
            PerformLayout();
            Invalidate(true);
        }
    }
}