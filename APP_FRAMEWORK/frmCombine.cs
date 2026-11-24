using Bunifu.Framework.UI;
using BunifuAnimatorNS;
using NEXUS.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace APP_FRAMEWORK;

public class frmCombine : Form
{
    private IContainer components;

    private BunifuTransition bunifuTransition_0;

    private BunifuTransition bunifuTransition_1;

    private BunifuElipse bunifuElipse1;

    private Panel pnlHeader;

    private BunifuImageButton bunifuImageButton1;

    private BunifuImageButton btnClose;

    private Label label5;

    private BunifuThinButton2 bunifuThinButton23;

    private BunifuThinButton2 bunifuThinButton22;

    private BunifuThinButton2 bunifuThinButton21;

    private Panel BAAAA;

    private tabNetBanking CAAA0;

    private tabDebitCard tabDebitCard;

    private tabCreditCard tabCreditCard;

    public frmCombine()
    {
        InitializeComponent();
        ApplyByteNovaLatestDesign();
        pnlHeader.MouseDown += StatClass.Header_MouseDown;
        pnlHeader.MouseMove += StatClass.Header_MouseMove;
        pnlHeader.MouseUp += StatClass.Header_MouseUp;
    }

    private void method_0()
    {
        CAAA0.Visible = false;
        tabDebitCard.Visible = false;
        tabCreditCard.Visible = false;
        BAAAA.Visible = false;
    }

    private void BunifuThinButton21_Click(object sender, EventArgs e)
    {
        method_0();
        CAAA0.Visible = true;
        CAAA0.BringToFront();
        bunifuTransition_1.ShowSync(BAAAA);
    }

    private void BunifuThinButton22_Click(object sender, EventArgs e)
    {
        method_0();
        tabDebitCard.Visible = true;
        tabDebitCard.BringToFront();
        bunifuTransition_1.ShowSync(BAAAA);
    }

    private void BunifuThinButton23_Click(object sender, EventArgs e)
    {
        method_0();
        tabCreditCard.Visible = true;
        tabCreditCard.BringToFront();
        bunifuTransition_1.ShowSync(BAAAA);
    }

    private void FrmCombine_FormClosing(object sender, FormClosingEventArgs e)
    {
        StatClass.method_35();
    }

    private void BtnClose_Click(object sender, EventArgs e)
    {
        Close();
    }


    // =====================================================================
    //  ByteNova design — FORCED.
    //  tabNetBanking / tabDebitCard / tabCreditCard run their own styling when
    //  they load (and when they are shown), which used to undo this layout.
    //  So the layout of every tab is captured ONCE as a "plan" and re-applied
    //  after each tab loads, becomes visible or resizes — this design always wins.
    // =====================================================================

    private const int PAD = 18;          // inner margin of a tab
    private const int FIELD_H = 34;      // height of every input box
    private const int ROW_GAP = 10;      // vertical gap between two rows
    private const int INLINE_GAP = 10;   // gap between items in the same row
    private const int SAVE_H = 40;

    private static readonly Color CInk = Color.FromArgb(17, 40, 72);
    private static readonly Color CLabel = Color.FromArgb(28, 52, 88);
    private static readonly Color CCue = Color.FromArgb(150, 162, 180);
    private static readonly Color COrange = Color.FromArgb(245, 146, 28);
    private static readonly Color CLine = Color.FromArgb(198, 216, 242);

    private Panel _strip;
    private bool _shapeHooked;
    private BunifuThinButton2[] _pills;
    private bool _closeHot;
    private bool _closeDown;
    private bool _applying;
    private int _passes;
    private System.Windows.Forms.Timer _reapplyTimer;
    private readonly Dictionary<Control, TabPlan> _plans = new Dictionary<Control, TabPlan>();
    private readonly Dictionary<ComboBox, string> _comboCues = new Dictionary<ComboBox, string>();

    private class TabPlan
    {
        public Control Tab;
        public Control ListHost;
        public ListBox ListCtl;
        public Button Save;
        public List<List<Control>> Rows = new List<List<Control>>();
        public List<FieldBox> Boxes = new List<FieldBox>();
        public List<Control> OldPanels = new List<Control>();
        public int Needed;
    }

    private class Entry
    {
        public Control C;
        public int Cy;
        public int X;
    }

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

    private const int CB_SETCUEBANNER = 0x1703;

    private static Color Lighten(Color c, float f)
    {
        return Color.FromArgb(
            (int)Math.Min(255, c.R + (255 - c.R) * f),
            (int)Math.Min(255, c.G + (255 - c.G) * f),
            (int)Math.Min(255, c.B + (255 - c.B) * f));
    }

    private static Color Darken(Color c, float f)
    {
        return Color.FromArgb((int)(c.R * (1 - f)), (int)(c.G * (1 - f)), (int)(c.B * (1 - f)));
    }

    private void ApplyByteNovaLatestDesign()
    {
        SuspendLayout();
        try
        {
            BackColor = Color.FromArgb(233, 241, 252);
            ClientSize = new Size(880, 660);
            _pills = new BunifuThinButton2[3] { bunifuThinButton21, bunifuThinButton22, bunifuThinButton23 };

            pnlHeader.Dock = DockStyle.None;
            pnlHeader.SetBounds(0, 0, ClientSize.Width, 48);
            pnlHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlHeader.BackColor = Color.FromArgb(18, 76, 196);
            pnlHeader.Paint -= ByteNovaHeaderPaint;
            pnlHeader.Paint += ByteNovaHeaderPaint;

            label5.AutoSize = false;
            label5.SetBounds(20, 11, 540, 26);
            label5.Text = "PAYMENT DETAILS   •   BUILD " + Program.appversion;
            label5.Font = new Font("Segoe UI Semibold", 11.5F, FontStyle.Bold);
            label5.ForeColor = Color.White;
            label5.TextAlign = ContentAlignment.MiddleLeft;
            label5.BackColor = Color.Transparent;

            bunifuImageButton1.Visible = false;

            btnClose.SetBounds(ClientSize.Width - 46, 10, 30, 26);
            btnClose.BackColor = Color.Transparent;
            btnClose.Region = null;
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackgroundImage = null;
            btnClose.Image = null;
            btnClose.SizeMode = PictureBoxSizeMode.Zoom;
            btnClose.Zoom = 0;
            btnClose.Cursor = Cursors.Hand;
            btnClose.Paint -= ClosePaint;
            btnClose.Paint += ClosePaint;
            btnClose.MouseEnter -= CloseEnter;
            btnClose.MouseEnter += CloseEnter;
            btnClose.MouseLeave -= CloseLeave;
            btnClose.MouseLeave += CloseLeave;
            btnClose.MouseDown -= CloseDown;
            btnClose.MouseDown += CloseDown;
            btnClose.MouseUp -= CloseUp;
            btnClose.MouseUp += CloseUp;
            

            if (_strip == null)
            {
                _strip = new Panel();
                _strip.Name = "strip";
                _strip.Paint += StripPaint;
                Controls.Add(_strip);
            }
            _strip.SetBounds(0, 48, ClientSize.Width, 56);
            _strip.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _strip.BackColor = Color.FromArgb(214, 232, 252);

            for (int i = 0; i < 3; i++)
            {
                StylePill(_pills[i], i == 0 ? "NET BANKING" : (i == 1 ? "DEBIT CARD" : "CREDIT CARD"));
                _pills[i].Click -= PillClick;
                _pills[i].Click += PillClick;
                _pills[i].Tag = i;
            }

            int gap = 10;
            int left = 18;
            int pw = (ClientSize.Width - (left * 2) - (gap * 2)) / 3;
            bunifuThinButton21.SetBounds(left, 9, pw, 36);
            bunifuThinButton22.SetBounds(left + pw + gap, 9, pw, 36);
            bunifuThinButton23.SetBounds(left + ((pw + gap) * 2), 9, pw, 36);

            bunifuThinButton21.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            bunifuThinButton22.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            bunifuThinButton23.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            _strip.Controls.Add(bunifuThinButton21);
            _strip.Controls.Add(bunifuThinButton22);
            _strip.Controls.Add(bunifuThinButton23);

            LayoutBody();
            BAAAA.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            BAAAA.BackColor = Color.White;
            BAAAA.Padding = new Padding(16);
            BAAAA.Paint -= ByteNovaBodyPanelPaint;
            BAAAA.Paint += ByteNovaBodyPanelPaint;

            Control[] tabs = new Control[] { CAAA0, tabDebitCard, tabCreditCard };
            foreach (Control t in tabs)
            {
                t.Dock = DockStyle.Fill;
                t.BackColor = Color.White;
                BuildPlan(t);

                // Re-apply after the tab's own theme code has run.
                UserControl uc = t as UserControl;
                if (uc != null)
                {
                    uc.Load -= TabLoad;
                    uc.Load += TabLoad;
                }
                t.VisibleChanged -= TabVisibleChanged;
                t.VisibleChanged += TabVisibleChanged;
                t.Resize -= TabResize;
                t.Resize += TabResize;
            }

            bunifuElipse1.ElipseRadius = 14;
            ShapeForm();

            if (!_shapeHooked)
            {
                _shapeHooked = true;
                Resize += delegate { ShapeForm(); };
                Resize += delegate { LayoutBody(); };
            }
            Shown -= FormShown;
            Shown += FormShown;

            method_0();
            CAAA0.Visible = true;
            CAAA0.BringToFront();
            BAAAA.Visible = true;
            _strip.BringToFront();
            pnlHeader.BringToFront();
            MarkPills(0);

            ApplyAll();
        }
        finally
        {
            ResumeLayout(false);
            PerformLayout();
            Invalidate(true);
        }

        // once more now that the tabs have their real size
        ApplyAll();
    }

    private void LayoutBody()
    {
        if (BAAAA == null) return;
        BAAAA.SetBounds(18, 118, Math.Max(220, ClientSize.Width - 36), Math.Max(140, ClientSize.Height - 138));
    }

    // ---------- re-apply triggers ----------

    private void TabLoad(object sender, EventArgs e) { ScheduleReapply(); }

    private void TabVisibleChanged(object sender, EventArgs e)
    {
        Control t = sender as Control;
        if (t != null && t.Visible) ScheduleReapply();
    }

    private void TabResize(object sender, EventArgs e)
    {
        Control t = sender as Control;
        TabPlan p;
        if (t != null && _plans.TryGetValue(t, out p)) ApplyPlan(p);
    }

    private void FormShown(object sender, EventArgs e)
    {
        LayoutBody();
        ApplyAll();
        ScheduleReapply();
    }

    private void ScheduleReapply()
    {
        if (IsDisposed || Disposing) return;
        if (_reapplyTimer == null)
        {
            _reapplyTimer = components != null ? new System.Windows.Forms.Timer(components) : new System.Windows.Forms.Timer();
            _reapplyTimer.Tick += ReapplyTick;
        }
        _passes = 3;
        _reapplyTimer.Stop();
        _reapplyTimer.Interval = 40;
        _reapplyTimer.Start();
    }

    private void ReapplyTick(object sender, EventArgs e)
    {
        ApplyAll();
        _passes--;
        if (_passes <= 0) _reapplyTimer.Stop();
        else _reapplyTimer.Interval = 250;
    }

    private void ApplyAll()
    {
        foreach (TabPlan p in _plans.Values) ApplyPlan(p);
        FitFormHeight();
    }

    private void FitFormHeight()
    {
        int need = 0;
        foreach (TabPlan p in _plans.Values) need = Math.Max(need, p.Needed);
        if (need <= 0) return;

        // header 48 + strip 56 + gap = 118, body padding 16*2, bottom margin 20
        int want = 118 + 32 + need + 20;
        Rectangle wa = Screen.FromControl(this).WorkingArea;
        want = Math.Min(want, wa.Height - 20);
        if (want <= ClientSize.Height) return;

        ClientSize = new Size(ClientSize.Width, want);
        if (Visible && Bottom > wa.Bottom) Top = Math.Max(wa.Top, wa.Bottom - Height);
    }

    // ---------- close button ----------

    private void CloseEnter(object sender, EventArgs e) { _closeHot = true; btnClose.Invalidate(); }
    private void CloseLeave(object sender, EventArgs e) { _closeHot = false; _closeDown = false; btnClose.Invalidate(); }
    private void CloseDown(object sender, MouseEventArgs e) { _closeDown = true; btnClose.Invalidate(); }
    private void CloseUp(object sender, MouseEventArgs e) { _closeDown = false; btnClose.Invalidate(); }

    private void ClosePaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;

        Color top = Color.FromArgb(239, 68, 68), bot = Color.FromArgb(205, 32, 38);
        if (_closeDown) { top = Color.FromArgb(185, 28, 28); bot = Color.FromArgb(160, 20, 24); }
        else if (_closeHot) { top = Color.FromArgb(248, 100, 100); bot = Color.FromArgb(226, 48, 52); }

        Rectangle r = new Rectangle(1, 1, c.Width - 3, c.Height - 3);
        if (_closeDown) r.Offset(0, 1);
        if (r.Width < 4 || r.Height < 4) return;

        using (GraphicsPath p = RoundPath(r, 7))
        {
            using (LinearGradientBrush b = new LinearGradientBrush(r, top, bot, LinearGradientMode.Vertical))
                g.FillPath(b, p);
            using (Pen edge = new Pen(Color.FromArgb(90, 255, 255, 255), 1f))
                g.DrawPath(edge, p);
        }

        float cx = r.X + r.Width / 2f, cy = r.Y + r.Height / 2f, a = 4.5f;
        using (Pen x = new Pen(Color.White, 2f))
        {
            x.StartCap = LineCap.Round;
            x.EndCap = LineCap.Round;
            g.DrawLine(x, cx - a, cy - a, cx + a, cy + a);
            g.DrawLine(x, cx + a, cy - a, cx - a, cy + a);
        }
    }

    // ---------- input box (orange rounded border around a TextBox / ComboBox) ----------

    private class FieldBox : Panel
    {
        public bool Active;
        public bool Hover;
        public Control Field;
        public Label CueLabel;
        public string Cue;
        public string BaseLabel;
        public int ShareIndex;
        public int ShareCount = 1;

        public FieldBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                   | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle r = new Rectangle(1, 1, Width - 3, Height - 3);

            Color penCol = Color.FromArgb(232, 170, 96);
            float penW = 1.3f;
            if (Hover) { penCol = Color.FromArgb(245, 146, 28); penW = 1.6f; }
            if (Active) { penCol = Color.FromArgb(222, 118, 10); penW = 2f; }

            using (GraphicsPath p = RoundPath(r, 7))
            {
                using (SolidBrush b = new SolidBrush(Color.White)) e.Graphics.FillPath(b, p);
                using (Pen pen = new Pen(penCol, penW)) e.Graphics.DrawPath(pen, p);
            }
        }
    }

    private FieldBox WrapField(Control f, Control tab, Point tabPos)
    {
        FieldBox box = new FieldBox();
        box.Field = f;
        box.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        box.Location = new Point(tabPos.X - 5, tabPos.Y - 5);
        box.Size = new Size(f.Width + 10, FIELD_H);
        box.Visible = WantsVisible(f);

        if (f.Parent != null) f.Parent.Controls.Remove(f);
        f.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        f.Location = new Point(8, 6);
        box.Controls.Add(f);
        tab.Controls.Add(box);
        box.BringToFront();

        f.Enter += delegate { box.Active = true; UpdateCue(box); box.Invalidate(); };
        f.Leave += delegate { box.Active = false; UpdateCue(box); box.Invalidate(); };
        f.MouseEnter += delegate { box.Hover = true; box.Invalidate(); };
        f.MouseLeave += delegate { box.Hover = false; box.Invalidate(); };
        box.MouseEnter += delegate { box.Hover = true; box.Invalidate(); };
        box.MouseLeave += delegate { box.Hover = false; box.Invalidate(); };
        box.MouseDown += delegate { if (f.CanFocus) f.Focus(); };
        f.VisibleChanged += delegate { ScheduleReapply(); };

        TextBoxBase tb = f as TextBoxBase;
        if (tb != null)
        {
            // Our own placeholder: works even if visual styles are off and
            // survives the textbox handle being recreated by another theme.
            Label cue = new Label();
            cue.Tag = "exCue";
            cue.AutoSize = false;
            cue.UseMnemonic = false;
            cue.BackColor = Color.White;
            cue.ForeColor = CCue;
            cue.TextAlign = ContentAlignment.MiddleLeft;
            cue.Cursor = Cursors.IBeam;
            cue.Visible = false;
            cue.MouseDown += delegate { if (tb.CanFocus) tb.Focus(); };
            box.Controls.Add(cue);
            cue.BringToFront();
            box.CueLabel = cue;

            tb.TextChanged += delegate { UpdateCue(box); };
            tb.EnabledChanged += delegate { UpdateCue(box); };
        }

        ComboBox cb = f as ComboBox;
        if (cb != null)
        {
            cb.SelectedIndexChanged += delegate { ScheduleReapply(); };
            cb.HandleCreated += delegate { SetComboCue(cb); };
        }

        return box;
    }

    private void UpdateCue(FieldBox box)
    {
        if (box == null || box.CueLabel == null) return;
        TextBoxBase tb = box.Field as TextBoxBase;
        if (tb == null) return;
        box.CueLabel.Text = box.Cue ?? "";
        box.CueLabel.Visible = !string.IsNullOrEmpty(box.Cue) && tb.TextLength == 0 && !box.Active;
        if (box.CueLabel.Visible) box.CueLabel.BringToFront();
    }

    private void SetComboCue(ComboBox cb)
    {
        string cue;
        if (cb == null || !_comboCues.TryGetValue(cb, out cue) || string.IsNullOrEmpty(cue)) return;
        if (cb.DropDownStyle == ComboBoxStyle.DropDownList) { cb.Invalidate(); return; }
        if (!cb.IsHandleCreated) return;
        try { SendMessage(cb.Handle, CB_SETCUEBANNER, IntPtr.Zero, cue); } catch { }
    }

    private void LayoutBox(FieldBox box)
    {
        Control f = box.Field;
        if (f == null) return;
        int w = box.Width;

        if (f is ComboBox)
        {
            int h = f.Height;
            f.SetBounds(5, Math.Max(2, (FIELD_H - h) / 2), Math.Max(30, w - 10), h);
        }
        else
        {
            TextBoxBase tb = f as TextBoxBase;
            int h = f.Height;
            if (tb != null && tb.Multiline) h = FIELD_H - 10;
            h = Math.Min(h, FIELD_H - 6);
            f.SetBounds(11, Math.Max(3, (FIELD_H - h) / 2), Math.Max(30, w - 22), h);
        }

        if (box.CueLabel != null)
        {
            if (box.CueLabel.Font != f.Font) box.CueLabel.Font = f.Font;
            box.CueLabel.SetBounds(f.Left, 4, f.Width, FIELD_H - 8);
            UpdateCue(box);
        }
    }

    // ---------- plan: built once per tab ----------

    private void BuildPlan(Control tab)
    {
        if (tab == null || _plans.ContainsKey(tab)) return;

        TabPlan plan = new TabPlan();
        plan.Tab = tab;

        List<Control> all = new List<Control>();
        CollectAll(tab, all);

        foreach (Control c in all)
        {
            ListBox x = c as ListBox;
            if (x != null) { plan.ListCtl = x; break; }
        }
        if (plan.ListCtl != null && plan.ListCtl.Parent != null && plan.ListCtl.Parent != tab)
            plan.ListHost = plan.ListCtl.Parent;

        // Original label text (before shortening) is used for the placeholders.
        Dictionary<Control, string> original = new Dictionary<Control, string>();
        foreach (Control c in all) if (c is Label) original[c] = c.Text ?? "";

        ShortenLabels(tab);

        bool pwdSeen = false;
        foreach (Control c in all)
        {
            if (plan.ListHost != null && InsideOf(c, plan.ListHost)) continue;

            CheckBox chk = c as CheckBox;
            if (chk != null && WantsVisible(chk) && (chk.Text ?? "").ToUpperInvariant().Contains("SHOW"))
            {
                if (pwdSeen) chk.Visible = false;
                pwdSeen = true;
            }

            Button b = c as Button;
            if (b != null && plan.Save == null && (b.Text ?? "").ToUpperInvariant().Contains("SAVE"))
                plan.Save = b;
        }

        // Positions relative to the tab, taken BEFORE anything is moved.
        List<Control> items = new List<Control>();
        CollectMovables(tab, plan.ListHost, items);
        Dictionary<Control, Point> pos = new Dictionary<Control, Point>();
        foreach (Control c in items) pos[c] = TabPoint(c, tab);

        if (plan.ListHost != null && plan.ListHost.Parent != tab)
        {
            Point hp = TabPoint(plan.ListHost, tab);
            plan.ListHost.Parent.Controls.Remove(plan.ListHost);
            tab.Controls.Add(plan.ListHost);
            plan.ListHost.Location = hp;
        }

        if (plan.Save != null && plan.Save.Parent != tab)
        {
            Point sp = TabPoint(plan.Save, tab);
            plan.Save.Parent.Controls.Remove(plan.Save);
            tab.Controls.Add(plan.Save);
            plan.Save.Location = sp;
        }
        if (plan.Save != null) plan.Save.Tag = "exSave";

        foreach (Control c in tab.Controls)
        {
            if (c == plan.ListHost) continue;
            if (c is Panel || c is GroupBox) plan.OldPanels.Add(c);
        }

        List<Entry> entries = new List<Entry>();
        foreach (Control c in items)
        {
            Point p = pos[c];
            Entry en = new Entry();
            en.Cy = p.Y + c.Height / 2;
            en.X = p.X;

            if (c is TextBoxBase || c is ComboBox)
            {
                FieldBox box = WrapField(c, tab, p);
                plan.Boxes.Add(box);
                en.C = box;
            }
            else
            {
                if (c.Parent != tab)
                {
                    c.Parent.Controls.Remove(c);
                    tab.Controls.Add(c);
                }
                c.Location = p;
                c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                en.C = c;
            }
            entries.Add(en);
        }

        entries.Sort(delegate (Entry a, Entry b)
        {
            int r = a.Cy.CompareTo(b.Cy);
            return r != 0 ? r : a.X.CompareTo(b.X);
        });

        int i = 0;
        while (i < entries.Count)
        {
            int band = entries[i].Cy;
            List<Entry> row = new List<Entry>();
            while (i < entries.Count && entries[i].Cy - band <= 12) { row.Add(entries[i]); i++; }
            row.Sort(delegate (Entry a, Entry b) { return a.X.CompareTo(b.X); });

            List<Control> rc = new List<Control>();
            foreach (Entry en in row) rc.Add(en.C);
            plan.Rows.Add(rc);

            // Which label does each field belong to (nearest label to its left)?
            string current = null;
            List<FieldBox> group = new List<FieldBox>();
            foreach (Control c in rc)
            {
                Label l = c as Label;
                if (l != null)
                {
                    FinishGroup(group, current);
                    group = new List<FieldBox>();
                    string t;
                    if (!original.TryGetValue(l, out t)) t = l.Text;
                    current = CleanLabel(t);
                    continue;
                }
                FieldBox fb = c as FieldBox;
                if (fb != null) group.Add(fb);
            }
            FinishGroup(group, current);
        }

        _plans[tab] = plan;
        StyleDeep(tab);
    }

    private static void FinishGroup(List<FieldBox> group, string label)
    {
        for (int k = 0; k < group.Count; k++)
        {
            group[k].BaseLabel = string.IsNullOrEmpty(label) ? null : label;
            group[k].ShareIndex = k;
            group[k].ShareCount = group.Count;
        }
    }

    private static string CleanLabel(string t)
    {
        if (t == null) return null;
        t = t.Replace(":", "").Replace("*", "").Replace("&", "").Trim();
        while (t.Contains("  ")) t = t.Replace("  ", " ");
        return t.Length == 0 ? null : t;
    }

    // Placeholder text for a field.
    private string ResolveCue(FieldBox box)
    {
        Control f = box.Field;
        if (f == null) return "";
        bool combo = f is ComboBox;

        string guess = combo ? GuessFromItems((ComboBox)f) : null;
        string nm = PrettyName(f.Name);
        if (nm == null && !string.IsNullOrEmpty(f.AccessibleName)) nm = f.AccessibleName.Trim();

        string text;
        if (box.ShareCount > 1)
            text = guess ?? nm ?? (box.BaseLabel != null ? box.BaseLabel + " " + (box.ShareIndex + 1) : null);
        else
            text = box.BaseLabel ?? guess ?? nm;

        if (string.IsNullOrEmpty(text)) return combo ? "Select" : "Enter details";
        return (combo ? "Select " : "Enter ") + text;
    }

    private static string GuessFromItems(ComboBox cb)
    {
        if (cb == null || cb.Items.Count == 0) return null;
        int n = Math.Min(3, cb.Items.Count);
        bool years = true, months = true;
        for (int k = 0; k < n; k++)
        {
            string s = (Convert.ToString(cb.Items[k]) ?? "").Trim();
            int v;
            bool num = int.TryParse(s, out v);
            if (!(num && v >= 1900 && v <= 2200) && !(num && s.Length == 2 && cb.Items.Count < 40 && v > 12)) years = false;
            bool mon = (num && v >= 1 && v <= 12)
                || s.StartsWith("Jan", StringComparison.OrdinalIgnoreCase)
                || s.StartsWith("Feb", StringComparison.OrdinalIgnoreCase)
                || s.StartsWith("Mar", StringComparison.OrdinalIgnoreCase);
            if (!mon) months = false;
        }
        if (months && cb.Items.Count <= 13) return "Month";
        if (years) return "Year";
        return null;
    }

    // "txtLoginPwd" -> "Login Pwd". Returns null for meaningless names (textBox1, AAAB0 ...).
    private static string PrettyName(string n)
    {
        if (string.IsNullOrEmpty(n)) return null;
        int i = 0;
        while (i < n.Length && char.IsLower(n[i])) i++;
        if (i == 0 || i > 6 || i >= n.Length) return null;

        string s = n.Substring(i).TrimEnd('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '_');
        if (s.Length < 2 || s == "Box" || s == "TextBox" || s == "ComboBox") return null;

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int k = 0; k < s.Length; k++)
        {
            char ch = s[k];
            if (ch == '_') { sb.Append(' '); continue; }
            if (k > 0 && char.IsUpper(ch) && char.IsLower(s[k - 1])) sb.Append(' ');
            sb.Append(ch);
        }
        string r = sb.ToString().Trim();
        int lower = 0;
        foreach (char ch in r) if (char.IsLower(ch)) lower++;
        return lower == 0 ? null : r;   // all-caps fragments are obfuscated names
    }

    // ---------- plan: applied every time (this is what makes the design "forced") ----------

    private void ApplyPlan(TabPlan plan)
    {
        if (_applying || plan == null) return;
        Control tab = plan.Tab;
        if (tab == null || tab.IsDisposed || tab.ClientSize.Width < 200) return;

        _applying = true;
        tab.SuspendLayout();
        try
        {
            tab.BackColor = Color.White;
            StyleDeep(tab);

            // fields whose code-behind hid/showed them
            foreach (FieldBox box in plan.Boxes)
            {
                bool want = WantsVisible(box.Field);
                if (box.Visible != want) box.Visible = want;
                if (box.Field.Parent != box)
                {
                    if (box.Field.Parent != null) box.Field.Parent.Controls.Remove(box.Field);
                    box.Controls.Add(box.Field);
                }

                box.Cue = ResolveCue(box);
                ComboBox cb = box.Field as ComboBox;
                if (cb != null) { _comboCues[cb] = box.Cue; SetComboCue(cb); }
            }

            // old (now empty) container panels: no border, hide when empty
            foreach (Control c in plan.OldPanels)
            {
                if (c is FieldBox) continue;
                Panel p = c as Panel;
                if (p != null) p.BorderStyle = BorderStyle.None;
                bool keep = false;
                foreach (Control k in c.Controls) if (WantsVisible(k)) { keep = true; break; }
                if (c.Visible != keep) c.Visible = keep;
                c.SendToBack();
            }

            // saved-list card on the right
            int right = tab.ClientSize.Width - PAD;
            int leftLimit = right;
            if (plan.ListHost != null)
            {
                Control h = plan.ListHost;
                Panel hp = h as Panel;
                if (hp != null)
                {
                    hp.BorderStyle = BorderStyle.None;
                    hp.BackColor = Color.White;
                    hp.Paint -= CardPaint;
                    hp.Paint += CardPaint;
                }
                h.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                h.SetBounds(right - h.Width, PAD, h.Width, h.Height);
                h.BringToFront();
                h.Invalidate();
                leftLimit = h.Left - 22;
            }
            else if (plan.ListCtl != null && plan.ListCtl.Parent == tab)
            {
                leftLimit = plan.ListCtl.Left - 22;
            }
            if (leftLimit < PAD + 260) leftLimit = Math.Min(right, PAD + 260);

            // label column width
            int labelW = 0;
            foreach (List<Control> row in plan.Rows)
            {
                foreach (Control c in row)
                {
                    if (!WantsVisible(c)) continue;
                    Label l = c as Label;
                    if (l != null) labelW = Math.Max(labelW, TextW(l));
                    break;
                }
            }
            int fieldX = labelW > 0 ? PAD + Math.Max(96, labelW + 14) : PAD;

            int y = PAD;
            foreach (List<Control> row in plan.Rows)
            {
                List<Control> vis = new List<Control>();
                foreach (Control c in row) if (WantsVisible(c)) vis.Add(c);
                if (vis.Count == 0) continue;

                bool hasField = false;
                foreach (Control c in vis) if (c is FieldBox) { hasField = true; break; }

                int rowH = FIELD_H;
                if (!hasField)
                {
                    rowH = 22;
                    foreach (Control c in vis)
                        rowH = Math.Max(rowH, c is Label ? 22 : c.PreferredSize.Height);
                }

                int start = 0;
                Label lead = vis[0] as Label;
                if (lead != null)
                {
                    PlaceLabel(lead, PAD, y, fieldX - PAD - 6, rowH);
                    start = 1;
                }

                int fields = 0, fixedW = 0, n = vis.Count - start;
                for (int k = start; k < vis.Count; k++)
                {
                    if (vis[k] is FieldBox) fields++;
                    else fixedW += ItemW(vis[k]);
                }
                int avail = leftLimit - fieldX - fixedW - INLINE_GAP * Math.Max(0, n - 1);
                int w = fields > 0 ? Math.Max(60, avail / fields) : 0;

                int x = fieldX;
                for (int k = start; k < vis.Count; k++)
                {
                    Control c = vis[k];
                    FieldBox box = c as FieldBox;
                    if (box != null)
                    {
                        // last field takes the rounding remainder so the right edges line up
                        int bw = (k == vis.Count - 1 || IsLastField(vis, k)) ? Math.Max(60, leftLimit - x) : w;
                        box.SetBounds(x, y, bw, FIELD_H);
                        LayoutBox(box);
                        box.Invalidate();
                        x += bw + INLINE_GAP;
                    }
                    else if (c is Label)
                    {
                        int lw = ItemW(c);
                        PlaceLabel((Label)c, x, y, lw, rowH);
                        x += lw + INLINE_GAP;
                    }
                    else
                    {
                        int cw = ItemW(c);
                        int ch = Math.Max(18, c.PreferredSize.Height);
                        c.SetBounds(x, y + (rowH - ch) / 2, cw, ch);
                        x += cw + INLINE_GAP;
                    }
                    c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    c.BringToFront();
                }

                y += rowH + ROW_GAP;
            }

            if (plan.Save != null)
            {
                Button s = plan.Save;
                if (s.Parent != tab) { s.Parent.Controls.Remove(s); tab.Controls.Add(s); }
                int sy = Math.Max(y + 8, tab.ClientSize.Height - PAD - SAVE_H);
                s.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                s.SetBounds(PAD, sy, Math.Max(200, leftLimit - PAD), SAVE_H);
                SetFont(s, "Segoe UI Semibold", 10.5f, FontStyle.Bold);
                s.BringToFront();
                y += 8 + SAVE_H;
            }

            int need = y + PAD;
            if (plan.ListHost != null) need = Math.Max(need, plan.ListHost.Bottom + PAD);
            plan.Needed = need;
        }
        finally
        {
            tab.ResumeLayout(false);
            _applying = false;
            tab.Invalidate(true);
        }
    }

    private static bool IsLastField(List<Control> vis, int k)
    {
        for (int j = k + 1; j < vis.Count; j++) if (vis[j] is FieldBox) return false;
        // only stretch if nothing but labels/checkboxes follow (they're none in practice)
        return k == vis.Count - 1;
    }

    private static void PlaceLabel(Label l, int x, int y, int w, int h)
    {
        l.AutoSize = false;
        l.TextAlign = ContentAlignment.MiddleLeft;
        l.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        l.SetBounds(x, y, w, h);
    }

    private static int TextW(Label l)
    {
        return TextRenderer.MeasureText(l.Text ?? "", l.Font).Width + 4;
    }

    private static int ItemW(Control c)
    {
        Label l = c as Label;
        if (l != null) return TextW(l);
        if (c is CheckBox || c is RadioButton) return c.PreferredSize.Width + 4;
        return c.Width;
    }

    private void CardPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null || c.Width < 20 || c.Height < 20) return;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (GraphicsPath p = RoundPath(new Rectangle(0, 0, c.Width - 1, c.Height - 1), 10))
        using (Pen pen = new Pen(CLine, 1f))
            e.Graphics.DrawPath(pen, p);
    }

    // ---------- helpers ----------

    private static MethodInfo _getState;
    private static bool _getStateLooked;

    // The control's OWN Visible flag (Control.Visible is false while the form or
    // the tab is still hidden — that is why the old reflow did nothing).
    private static bool WantsVisible(Control c)
    {
        if (c == null) return false;
        if (!_getStateLooked)
        {
            _getStateLooked = true;
            foreach (MethodInfo m in typeof(Control).GetMethods(BindingFlags.Instance | BindingFlags.NonPublic))
            {
                if (m.Name == "GetState" && m.GetParameters().Length == 1 && m.ReturnType == typeof(bool))
                {
                    _getState = m;
                    break;
                }
            }
        }
        if (_getState != null)
        {
            try
            {
                Type pt = _getState.GetParameters()[0].ParameterType;
                object arg = pt.IsEnum ? Enum.ToObject(pt, 2) : (object)2;   // STATE_VISIBLE
                return (bool)_getState.Invoke(c, new object[] { arg });
            }
            catch { }
        }
        return c.Visible;
    }

    private static Point TabPoint(Control c, Control tab)
    {
        Point p = c.Location;
        Control par = c.Parent;
        while (par != null && par != tab)
        {
            p.Offset(par.Left, par.Top);
            par = par.Parent;
        }
        return p;
    }

    private bool InsideOf(Control c, Control box)
    {
        Control p = c.Parent;
        while (p != null) { if (p == box) return true; p = p.Parent; }
        return false;
    }

    private void CollectAll(Control p, List<Control> all)
    {
        foreach (Control c in p.Controls)
        {
            all.Add(c);
            CollectAll(c, all);
        }
    }

    private void CollectMovables(Control p, Control skip, List<Control> list)
    {
        foreach (Control c in p.Controls)
        {
            if (skip != null && (c == skip || InsideOf(c, skip))) continue;
            if (c is FieldBox) continue;
            if (c is TextBoxBase || c is ComboBox || c is Label || c is CheckBox || c is RadioButton) { list.Add(c); continue; }
            if (c is Button || c is ListBox || c is DataGridView) continue;
            if (c is Panel || c is GroupBox) CollectMovables(c, skip, list);
        }
    }

    private void ShortenLabels(Control par)
    {
        List<Control> all = new List<Control>();
        CollectAll(par, all);

        foreach (Control c in all)
        {
            Label l = c as Label;
            if (l == null || "exCue".Equals(l.Tag)) continue;

            string t = l.Text ?? "";
            if (t.Contains("Login Password")) l.Text = "Login Pwd :";
            else if (t.Contains("Valid Thru")) l.Text = "Expiry :";
            else if (t.Contains("Card 3D Password")) l.Text = "3D Password :";
            else if (t.Contains("Visa / Master Pin")) l.Text = "Visa Pin :";
            else if (t.Contains("Card ATM Pin")) l.Text = "ATM Pin :";
        }
    }

    private static void SetFont(Control c, string name, float size, FontStyle style)
    {
        Font f = c.Font;
        if (f != null && f.Name == name && Math.Abs(f.Size - size) < 0.01f && f.Style == style) return;
        c.Font = new Font(name, size, style);
    }

    private void PillClick(object sender, EventArgs e)
    {
        BunifuThinButton2 b = sender as BunifuThinButton2;
        if (b == null || !(b.Tag is int)) return;
        MarkPills((int)b.Tag);
    }

    private void MarkPills(int active)
    {
        for (int i = 0; i < _pills.Length; i++)
        {
            BunifuThinButton2 p = _pills[i];
            if (p == null) continue;

            if (i == active)
            {
                p.IdleFillColor = COrange;
                p.IdleForecolor = Color.White;
                p.IdleLineColor = Color.FromArgb(222, 126, 16);
                p.ActiveFillColor = Color.FromArgb(232, 132, 14);
                p.ActiveForecolor = Color.White;
            }
            else
            {
                p.IdleFillColor = Color.White;
                p.IdleForecolor = CInk;
                p.IdleLineColor = Color.FromArgb(150, 184, 226);
                p.ActiveFillColor = Color.FromArgb(240, 246, 255);
                p.ActiveForecolor = CInk;
            }
            p.Invalidate();
        }
    }

    private void ShapeForm()
    {
        if (Width < 40 || Height < 40) return;
        using (GraphicsPath p = RoundPath(new Rectangle(0, 0, Width, Height), 16))
            Region = new Region(p);
    }

    private static GraphicsPath RoundPath(Rectangle r, int rad)
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

    private void RoundControl(Control c, int rad)
    {
        if (c == null || c.Width < 2 || c.Height < 2) return;
        using (GraphicsPath p = RoundPath(new Rectangle(0, 0, c.Width, c.Height), rad))
            c.Region = new Region(p);
    }

    private void StylePill(BunifuThinButton2 b, string text)
    {
        if (b == null) return;
        b.ButtonText = text;
        b.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
        b.IdleCornerRadius = 8;
        b.ActiveCornerRadius = 8;
        b.IdleBorderThickness = 1;
        b.ActiveBorderThickness = 1;
        b.BackColor = Color.FromArgb(214, 232, 252);
        b.ForeColor = CInk;
        b.Cursor = Cursors.Hand;
    }

    private void StripPaint(object sender, PaintEventArgs e)
    {
        Panel p = sender as Panel;
        if (p == null) return;
        using (Pen line = new Pen(CLine, 1f))
            e.Graphics.DrawLine(line, 0, p.Height - 1, p.Width, p.Height - 1);
    }

    // Re-run on every apply: puts back anything another theme changed.
    private void StyleDeep(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            TextBoxBase tb = c as TextBoxBase;
            if (tb != null)
            {
                if (tb.BorderStyle != BorderStyle.None) tb.BorderStyle = BorderStyle.None;
                tb.BackColor = Color.White;
                tb.ForeColor = CInk;
                SetFont(tb, "Segoe UI", 10f, FontStyle.Regular);
            }

            ComboBox cb = c as ComboBox;
            if (cb != null) StyleCombo(cb);

            ListBox lb = c as ListBox;
            if (lb != null)
            {
                lb.BorderStyle = BorderStyle.FixedSingle;
                lb.BackColor = Color.FromArgb(250, 252, 255);
                lb.ForeColor = CInk;
                SetFont(lb, "Segoe UI", 10f, FontStyle.Regular);
            }

            Label l = c as Label;
            if (l != null && !"exCue".Equals(l.Tag))
            {
                l.ForeColor = CLabel;
                l.BackColor = Color.White;
                SetFont(l, "Segoe UI", 9.5f, FontStyle.Regular);
            }

            CheckBox chk = c as CheckBox;
            if (chk != null)
            {
                chk.ForeColor = CInk;
                chk.BackColor = Color.White;
                SetFont(chk, "Segoe UI", 9.5f, FontStyle.Regular);
            }

            RadioButton rb = c as RadioButton;
            if (rb != null)
            {
                rb.ForeColor = CInk;
                rb.BackColor = Color.White;
            }

            Button b = c as Button;
            if (b != null) StyleFlatButton(b);

            DataGridView d = c as DataGridView;
            if (d != null) StyleGrid(d);

            Panel p = c as Panel;
            if (p != null) p.BackColor = Color.White;

            GroupBox g = c as GroupBox;
            if (g != null)
            {
                g.ForeColor = CInk;
                g.BackColor = Color.White;
            }

            if (!(c is ComboBox) && !(c is TextBoxBase)) StyleDeep(c);
        }
    }

    private void StyleCombo(ComboBox cb)
    {
        ClearDrawItem(cb);
        cb.FlatStyle = FlatStyle.Flat;
        cb.BackColor = Color.White;
        cb.ForeColor = CInk;
        SetFont(cb, "Segoe UI", 10f, FontStyle.Regular);
        if (cb.DrawMode != DrawMode.OwnerDrawFixed) cb.DrawMode = DrawMode.OwnerDrawFixed;
        if (cb.ItemHeight != 22) cb.ItemHeight = 22;
        cb.MaxDropDownItems = 20;
        cb.IntegralHeight = false;
        cb.DropDownHeight = 340;
        cb.DrawItem += ComboDraw;
    }

    // Removes every DrawItem handler (ours + any other theme's) so only ours paints.
    private void ClearDrawItem(ComboBox cb)
    {
        cb.DrawItem -= ComboDraw;
        try
        {
            FieldInfo key = null;
            string[] names = new string[] { "EVENT_DRAWITEM", "s_drawItemEvent" };
            foreach (string n in names)
            {
                key = typeof(ComboBox).GetField(n, BindingFlags.Static | BindingFlags.NonPublic);
                if (key != null) break;
            }
            PropertyInfo evp = typeof(Component).GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
            if (key == null || evp == null) return;

            EventHandlerList list = evp.GetValue(cb, null) as EventHandlerList;
            object k = key.GetValue(null);
            if (list == null || k == null) return;
            Delegate d = list[k];
            if (d != null) list.RemoveHandler(k, d);
        }
        catch { }
    }

    private void ComboDraw(object sender, DrawItemEventArgs e)
    {
        ComboBox cb = sender as ComboBox;
        if (cb == null) return;
        Rectangle r = e.Bounds;

        if (e.Index < 0)
        {
            // nothing selected -> placeholder
            using (SolidBrush bg = new SolidBrush(Color.White)) e.Graphics.FillRectangle(bg, r);
            string cue;
            if (_comboCues.TryGetValue(cb, out cue) && !string.IsNullOrEmpty(cue))
                TextRenderer.DrawText(e.Graphics, cue, cb.Font,
                    new Rectangle(r.X + 6, r.Y, r.Width - 10, r.Height), CCue,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
            return;
        }

        bool closed = (e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;
        bool sel = !closed && (e.State & DrawItemState.Selected) == DrawItemState.Selected;

        using (SolidBrush bg = new SolidBrush(sel ? Color.FromArgb(255, 236, 214) : Color.White))
            e.Graphics.FillRectangle(bg, r);

        if (sel)
            using (SolidBrush bar = new SolidBrush(COrange))
                e.Graphics.FillRectangle(bar, r.X, r.Y, 3, r.Height);

        TextRenderer.DrawText(e.Graphics, cb.GetItemText(cb.Items[e.Index]), cb.Font,
            new Rectangle(r.X + (closed ? 6 : 10), r.Y, r.Width - 14, r.Height), CInk,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
    }

    private void StyleFlatButton(Button b)
    {
        string t = (b.Text ?? "").ToUpperInvariant();

        Color bg = COrange;
        if (t.Contains("DEL") || t.Contains("CLEAR") || t.Contains("REMOVE"))
            bg = Color.FromArgb(220, 38, 38);
        else if (t.Contains("EDIT") || t.Contains("UPDATE"))
            bg = Color.FromArgb(59, 132, 247);

        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.BackColor = bg;
        b.ForeColor = Color.White;
        if (!"exSave".Equals(b.Tag)) SetFont(b, "Segoe UI Semibold", 9.5f, FontStyle.Regular);
        b.UseVisualStyleBackColor = false;
        b.Cursor = Cursors.Hand;
        b.FlatAppearance.MouseOverBackColor = Lighten(bg, 0.16f);
        b.FlatAppearance.MouseDownBackColor = Darken(bg, 0.16f);
        RoundControl(b, 7);
        b.Resize -= ButtonRound;
        b.Resize += ButtonRound;
    }

    private void ButtonRound(object sender, EventArgs e)
    {
        RoundControl(sender as Control, 7);
    }

    private void StyleGrid(DataGridView d)
    {
        d.EnableHeadersVisualStyles = false;
        d.BackgroundColor = Color.White;
        d.BorderStyle = BorderStyle.None;
        d.GridColor = CLine;
        d.RowHeadersVisible = false;
        d.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(18, 76, 196);
        d.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        d.DefaultCellStyle.BackColor = Color.White;
        d.DefaultCellStyle.ForeColor = CInk;
        d.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 236, 214);
        d.DefaultCellStyle.SelectionForeColor = CInk;
    }

    private void ByteNovaHeaderPaint(object sender, PaintEventArgs e)
    {
        Rectangle r = pnlHeader.ClientRectangle;
        if (r.Width <= 0 || r.Height <= 0) return;

        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(18, 76, 196), Color.FromArgb(48, 140, 244),
            LinearGradientMode.Horizontal))
            e.Graphics.FillRectangle(bg, r);

        using (SolidBrush orange = new SolidBrush(COrange))
            e.Graphics.FillRectangle(orange, 0, r.Height - 3, r.Width, 3);
    }

    private void ByteNovaBodyPanelPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null || c.ClientSize.Width < 4 || c.ClientSize.Height < 4) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, c.ClientSize.Width - 1, c.ClientSize.Height - 1);

        using (GraphicsPath path = RoundPath(r, 12))
        {
            using (SolidBrush bg = new SolidBrush(Color.White)) e.Graphics.FillPath(bg, path);
            using (Pen p = new Pen(CLine, 1f)) e.Graphics.DrawPath(p, path);
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            BunifuAnimatorNS.Animation animation1 = new BunifuAnimatorNS.Animation();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCombine));
            BunifuAnimatorNS.Animation animation2 = new BunifuAnimatorNS.Animation();
            this.bunifuTransition_0 = new BunifuAnimatorNS.BunifuTransition(this.components);
            this.BAAAA = new System.Windows.Forms.Panel();
            this.CAAA0 = new APP_FRAMEWORK.tabNetBanking();
            this.tabDebitCard = new APP_FRAMEWORK.tabDebitCard();
            this.tabCreditCard = new APP_FRAMEWORK.tabCreditCard();
            this.bunifuThinButton23 = new Bunifu.Framework.UI.BunifuThinButton2();
            this.bunifuThinButton22 = new Bunifu.Framework.UI.BunifuThinButton2();
            this.bunifuThinButton21 = new Bunifu.Framework.UI.BunifuThinButton2();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.bunifuImageButton1 = new Bunifu.Framework.UI.BunifuImageButton();
            this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.label5 = new System.Windows.Forms.Label();
            this.bunifuTransition_1 = new BunifuAnimatorNS.BunifuTransition(this.components);
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.BAAAA.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.SuspendLayout();
            // 
            // bunifuTransition_0
            // 
            this.bunifuTransition_0.AnimationType = BunifuAnimatorNS.AnimationType.Leaf;
            this.bunifuTransition_0.Cursor = null;
            animation1.AnimateOnlyDifferences = true;
            animation1.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.BlindCoeff")));
            animation1.LeafCoeff = 1F;
            animation1.MaxTime = 1F;
            animation1.MinTime = 0F;
            animation1.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicCoeff")));
            animation1.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicShift")));
            animation1.MosaicSize = 0;
            animation1.Padding = new System.Windows.Forms.Padding(0);
            animation1.RotateCoeff = 0F;
            animation1.RotateLimit = 0F;
            animation1.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.ScaleCoeff")));
            animation1.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.SlideCoeff")));
            animation1.TimeCoeff = 0F;
            animation1.TransparencyCoeff = 0F;
            this.bunifuTransition_0.DefaultAnimation = animation1;
            // 
            // BAAAA
            // 
            this.BAAAA.BackColor = System.Drawing.Color.Transparent;
            this.BAAAA.Controls.Add(this.CAAA0);
            this.BAAAA.Controls.Add(this.tabDebitCard);
            this.BAAAA.Controls.Add(this.tabCreditCard);
            this.bunifuTransition_0.SetDecoration(this.BAAAA, BunifuAnimatorNS.DecorationType.None);
            this.BAAAA.Location = new System.Drawing.Point(9, 88);
            this.BAAAA.Name = "BAAAA";
            this.BAAAA.Size = new System.Drawing.Size(752, 324);
            this.BAAAA.TabIndex = 1217;
            // 
            // CAAA0
            // 
            this.CAAA0.BackColor = System.Drawing.Color.Transparent;
            this.bunifuTransition_0.SetDecoration(this.CAAA0, BunifuAnimatorNS.DecorationType.None);
            this.CAAA0.Location = new System.Drawing.Point(0, 0);
            this.CAAA0.Margin = new System.Windows.Forms.Padding(0);
            this.CAAA0.Name = "CAAA0";
            this.CAAA0.Size = new System.Drawing.Size(742, 310);
            this.CAAA0.TabIndex = 1;
            // 
            // tabDebitCard
            // 
            this.tabDebitCard.BackColor = System.Drawing.Color.Transparent;
            this.bunifuTransition_0.SetDecoration(this.tabDebitCard, BunifuAnimatorNS.DecorationType.None);
            this.tabDebitCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDebitCard.Location = new System.Drawing.Point(0, 0);
            this.tabDebitCard.Margin = new System.Windows.Forms.Padding(0);
            this.tabDebitCard.Name = "tabDebitCard";
            this.tabDebitCard.Size = new System.Drawing.Size(752, 324);
            this.tabDebitCard.TabIndex = 2;
            // 
            // tabCreditCard
            // 
            this.tabCreditCard.BackColor = System.Drawing.Color.Transparent;
            this.bunifuTransition_0.SetDecoration(this.tabCreditCard, BunifuAnimatorNS.DecorationType.None);
            this.tabCreditCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCreditCard.Location = new System.Drawing.Point(0, 0);
            this.tabCreditCard.Margin = new System.Windows.Forms.Padding(0);
            this.tabCreditCard.Name = "tabCreditCard";
            this.tabCreditCard.Size = new System.Drawing.Size(752, 324);
            this.tabCreditCard.TabIndex = 3;
            // 
            // bunifuThinButton23
            // 
            this.bunifuThinButton23.ActiveBorderThickness = 1;
            this.bunifuThinButton23.ActiveCornerRadius = 20;
            this.bunifuThinButton23.ActiveFillColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton23.ActiveForecolor = System.Drawing.Color.White;
            this.bunifuThinButton23.ActiveLineColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton23.BackColor = System.Drawing.Color.GhostWhite;
            this.bunifuThinButton23.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuThinButton23.BackgroundImage")));
            this.bunifuThinButton23.ButtonText = "Credit Card";
            this.bunifuThinButton23.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.bunifuThinButton23, BunifuAnimatorNS.DecorationType.None);
            this.bunifuThinButton23.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuThinButton23.ForeColor = System.Drawing.Color.DimGray;
            this.bunifuThinButton23.IdleBorderThickness = 2;
            this.bunifuThinButton23.IdleCornerRadius = 10;
            this.bunifuThinButton23.IdleFillColor = System.Drawing.Color.White;
            this.bunifuThinButton23.IdleForecolor = System.Drawing.Color.DimGray;
            this.bunifuThinButton23.IdleLineColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton23.Location = new System.Drawing.Point(508, 39);
            this.bunifuThinButton23.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuThinButton23.Name = "bunifuThinButton23";
            this.bunifuThinButton23.Size = new System.Drawing.Size(231, 41);
            this.bunifuThinButton23.TabIndex = 1216;
            this.bunifuThinButton23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuThinButton23.Click += new System.EventHandler(this.BunifuThinButton23_Click);
            // 
            // bunifuThinButton22
            // 
            this.bunifuThinButton22.ActiveBorderThickness = 1;
            this.bunifuThinButton22.ActiveCornerRadius = 20;
            this.bunifuThinButton22.ActiveFillColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton22.ActiveForecolor = System.Drawing.Color.White;
            this.bunifuThinButton22.ActiveLineColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton22.BackColor = System.Drawing.Color.GhostWhite;
            this.bunifuThinButton22.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuThinButton22.BackgroundImage")));
            this.bunifuThinButton22.ButtonText = "Debit Card";
            this.bunifuThinButton22.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.bunifuThinButton22, BunifuAnimatorNS.DecorationType.None);
            this.bunifuThinButton22.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuThinButton22.ForeColor = System.Drawing.Color.DimGray;
            this.bunifuThinButton22.IdleBorderThickness = 2;
            this.bunifuThinButton22.IdleCornerRadius = 10;
            this.bunifuThinButton22.IdleFillColor = System.Drawing.Color.White;
            this.bunifuThinButton22.IdleForecolor = System.Drawing.Color.DimGray;
            this.bunifuThinButton22.IdleLineColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton22.Location = new System.Drawing.Point(263, 39);
            this.bunifuThinButton22.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuThinButton22.Name = "bunifuThinButton22";
            this.bunifuThinButton22.Size = new System.Drawing.Size(231, 41);
            this.bunifuThinButton22.TabIndex = 1215;
            this.bunifuThinButton22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuThinButton22.Click += new System.EventHandler(this.BunifuThinButton22_Click);
            // 
            // bunifuThinButton21
            // 
            this.bunifuThinButton21.ActiveBorderThickness = 1;
            this.bunifuThinButton21.ActiveCornerRadius = 20;
            this.bunifuThinButton21.ActiveFillColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton21.ActiveForecolor = System.Drawing.Color.White;
            this.bunifuThinButton21.ActiveLineColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton21.BackColor = System.Drawing.Color.GhostWhite;
            this.bunifuThinButton21.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuThinButton21.BackgroundImage")));
            this.bunifuThinButton21.ButtonText = "Bank";
            this.bunifuThinButton21.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.bunifuThinButton21, BunifuAnimatorNS.DecorationType.None);
            this.bunifuThinButton21.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuThinButton21.ForeColor = System.Drawing.Color.DimGray;
            this.bunifuThinButton21.IdleBorderThickness = 2;
            this.bunifuThinButton21.IdleCornerRadius = 10;
            this.bunifuThinButton21.IdleFillColor = System.Drawing.Color.White;
            this.bunifuThinButton21.IdleForecolor = System.Drawing.Color.DimGray;
            this.bunifuThinButton21.IdleLineColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton21.Location = new System.Drawing.Point(14, 39);
            this.bunifuThinButton21.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuThinButton21.Name = "bunifuThinButton21";
            this.bunifuThinButton21.Size = new System.Drawing.Size(231, 41);
            this.bunifuThinButton21.TabIndex = 1214;
            this.bunifuThinButton21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuThinButton21.Click += new System.EventHandler(this.BunifuThinButton21_Click);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(22, 14, 28);
            this.pnlHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlHeader.Controls.Add(this.bunifuImageButton1);
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Controls.Add(this.label5);
            this.bunifuTransition_0.SetDecoration(this.pnlHeader, BunifuAnimatorNS.DecorationType.None);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(18)))));
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(753, 31);
            this.pnlHeader.TabIndex = 1210;
            // 
            // bunifuImageButton1
            // 
            this.bunifuImageButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bunifuImageButton1.BackColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuImageButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.bunifuImageButton1, BunifuAnimatorNS.DecorationType.None);
            this.bunifuImageButton1.Image = ((System.Drawing.Image)(resources.GetObject("bunifuImageButton1.Image")));
            this.bunifuImageButton1.ImageActive = null;
            this.bunifuImageButton1.Location = new System.Drawing.Point(-35, 44);
            this.bunifuImageButton1.Name = "bunifuImageButton1";
            this.bunifuImageButton1.Size = new System.Drawing.Size(26, 20);
            this.bunifuImageButton1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.bunifuImageButton1.TabIndex = 208;
            this.bunifuImageButton1.TabStop = false;
            this.bunifuImageButton1.Zoom = 10;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.btnClose, BunifuAnimatorNS.DecorationType.None);
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageActive = null;
            this.btnClose.Location = new System.Drawing.Point(719, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(31, 26);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.TabIndex = 1;
            this.btnClose.TabStop = false;
            this.btnClose.Zoom = 10;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.bunifuTransition_0.SetDecoration(this.label5, BunifuAnimatorNS.DecorationType.None);
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(3, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "Bank";
            // 
            // bunifuTransition_1
            // 
            this.bunifuTransition_1.AnimationType = BunifuAnimatorNS.AnimationType.VertSlide;
            this.bunifuTransition_1.Cursor = null;
            animation2.AnimateOnlyDifferences = true;
            animation2.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation2.BlindCoeff")));
            animation2.LeafCoeff = 0F;
            animation2.MaxTime = 1F;
            animation2.MinTime = 0F;
            animation2.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation2.MosaicCoeff")));
            animation2.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation2.MosaicShift")));
            animation2.MosaicSize = 0;
            animation2.Padding = new System.Windows.Forms.Padding(0);
            animation2.RotateCoeff = 0F;
            animation2.RotateLimit = 0F;
            animation2.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation2.ScaleCoeff")));
            animation2.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation2.SlideCoeff")));
            animation2.TimeCoeff = 0F;
            animation2.TransparencyCoeff = 0F;
            this.bunifuTransition_1.DefaultAnimation = animation2;
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 5;
            this.bunifuElipse1.TargetControl = this;
            // 
            // frmCombine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(753, 406);
            this.Controls.Add(this.BAAAA);
            this.Controls.Add(this.bunifuThinButton23);
            this.Controls.Add(this.bunifuThinButton22);
            this.Controls.Add(this.bunifuThinButton21);
            this.Controls.Add(this.pnlHeader);
            this.bunifuTransition_1.SetDecoration(this, BunifuAnimatorNS.DecorationType.None);
            this.bunifuTransition_0.SetDecoration(this, BunifuAnimatorNS.DecorationType.None);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCombine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmCombine";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmCombine_FormClosing);
            this.BAAAA.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.ResumeLayout(false);

    }
}
