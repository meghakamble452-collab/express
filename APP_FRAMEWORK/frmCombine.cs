using Bunifu.Framework.UI;
using BunifuAnimatorNS;
using NEXUS.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
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


    private Panel _strip;
    private bool _shapeHooked;
    private BunifuThinButton2[] _pills;
    private bool _closeHot;
    private bool _closeDown;

    [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

    private const int EM_SETCUEBANNER = 0x1501;

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
            label5.Text = "PAYMENT DETAILS   \u2022   BUILD " + Program.appversion;
            label5.Font = new Font("Segoe UI Semibold", 11.5F, FontStyle.Bold);
            label5.ForeColor = Color.White;
            label5.TextAlign = ContentAlignment.MiddleLeft;
            label5.BackColor = Color.Transparent;

            bunifuImageButton1.Visible = false;

            btnClose.SetBounds(ClientSize.Width - 48, 10, 32, 28);
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
            RoundControl(btnClose, 8);

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

            CAAA0.Dock = DockStyle.Fill;
            tabDebitCard.Dock = DockStyle.Fill;
            tabCreditCard.Dock = DockStyle.Fill;

            CAAA0.BackColor = Color.White;
            tabDebitCard.BackColor = Color.White;
            tabCreditCard.BackColor = Color.White;

            StyleDeep(CAAA0);
            StyleDeep(tabDebitCard);
            StyleDeep(tabCreditCard);

            WrapAllFields(CAAA0);
            WrapAllFields(tabDebitCard);
            WrapAllFields(tabCreditCard);

            SetupTab(CAAA0);
            SetupTab(tabDebitCard);
            SetupTab(tabCreditCard);

            bunifuElipse1.ElipseRadius = 14;
            ShapeForm();

            if (!_shapeHooked)
            {
                _shapeHooked = true;
                Resize += delegate { ShapeForm(); };
                Resize += delegate { LayoutBody(); };
            }

            method_0();
            CAAA0.Visible = true;
            CAAA0.BringToFront();
            BAAAA.Visible = true;
            _strip.BringToFront();
            pnlHeader.BringToFront();
            MarkPills(0);
        }
        finally
        {
            ResumeLayout(false);
            PerformLayout();
            Invalidate(true);
        }
    }

    private void LayoutBody()
    {
        if (BAAAA == null) return;
        BAAAA.SetBounds(18, 118, Math.Max(220, ClientSize.Width - 36), Math.Max(140, ClientSize.Height - 138));
    }

    private void CloseEnter(object sender, EventArgs e) { _closeHot = true; btnClose.Invalidate(); }
    private void CloseLeave(object sender, EventArgs e) { _closeHot = false; _closeDown = false; btnClose.Invalidate(); }
    private void CloseDown(object sender, MouseEventArgs e) { _closeDown = true; btnClose.Invalidate(); }
    private void CloseUp(object sender, MouseEventArgs e) { _closeDown = false; btnClose.Invalidate(); }

    private void ClosePaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        Color bg = Color.FromArgb(220, 38, 38);
        if (_closeDown) bg = Color.FromArgb(170, 26, 26);
        else if (_closeHot) bg = Color.FromArgb(240, 78, 78);

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (SolidBrush b = new SolidBrush(bg))
        using (GraphicsPath p = RoundPath(new Rectangle(0, 0, c.Width - 1, c.Height - 1), 8))
            e.Graphics.FillPath(b, p);

        using (Pen pen = new Pen(Color.White, 2.2f))
        {
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            int m = 10;
            e.Graphics.DrawLine(pen, m, m, c.Width - m, c.Height - m);
            e.Graphics.DrawLine(pen, c.Width - m, m, m, c.Height - m);
        }
    }

    private class FieldBox : Panel
    {
        public bool Active;
        public bool Hover;

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
            Rectangle r = new Rectangle(0, 0, Width - 1, Height - 1);

            Color penCol = Color.FromArgb(232, 152, 44);
            float penW = 1.4f;
            if (Hover) { penCol = Color.FromArgb(245, 146, 28); penW = 1.8f; }
            if (Active) { penCol = Color.FromArgb(222, 118, 10); penW = 2.1f; }

            using (GraphicsPath p = RoundPath(r, 6))
            {
                using (SolidBrush b = new SolidBrush(Color.White)) e.Graphics.FillPath(b, p);
                using (Pen pen = new Pen(penCol, penW)) e.Graphics.DrawPath(pen, p);
            }
        }
    }

    private void CollectFields(Control p, System.Collections.Generic.List<Control> hits)
    {
        foreach (Control c in p.Controls)
        {
            if (c is TextBox || c is ComboBox) { hits.Add(c); continue; }
            if (c is Label || c is Button || c is CheckBox || c is RadioButton
                || c is ListBox || c is DataGridView) continue;
            CollectFields(c, hits);
        }
    }

    private void WrapAllFields(Control tab)
    {
        System.Collections.Generic.List<Control> hits = new System.Collections.Generic.List<Control>();
        CollectFields(tab, hits);

        foreach (Control f in hits)
        {
            Control par = f.Parent;
            if (par == null) continue;

            FieldBox box = new FieldBox();
            box.Anchor = f.Anchor;
            box.SetBounds(f.Left - 5, f.Top - 5, f.Width + 10, f.Height + 10);
            par.Controls.Add(box);
            box.BringToFront();

            f.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            f.SetBounds(5, 5, f.Width, f.Height);
            box.Controls.Add(f);

            f.Enter += delegate { box.Active = true; box.Invalidate(); };
            f.Leave += delegate { box.Active = false; box.Invalidate(); };
            f.MouseEnter += delegate { box.Hover = true; box.Invalidate(); };
            f.MouseLeave += delegate { box.Hover = false; box.Invalidate(); };
            box.MouseEnter += delegate { box.Hover = true; box.Invalidate(); };
            box.MouseLeave += delegate { box.Hover = false; box.Invalidate(); };
        }
    }

    private bool HasListBox(Control c)
    {
        foreach (Control k in c.Controls)
        {
            if (k is ListBox) return true;
            if (HasListBox(k)) return true;
        }
        return false;
    }

    private bool InsideOf(Control c, Control box)
    {
        Control p = c.Parent;
        while (p != null) { if (p == box) return true; p = p.Parent; }
        return false;
    }

    private void CollectMovables(Control p, Control skip, System.Collections.Generic.List<Control> list)
    {
        foreach (Control c in p.Controls)
        {
            if (c == skip) continue;

            if (c is FieldBox) { list.Add(c); continue; }
            if (c is TextBox || c is ComboBox || c is Label || c is CheckBox) { list.Add(c); continue; }
            if (c is Button || c is ListBox || c is DataGridView) continue;

            if (c is Panel)
            {
                CollectMovables(c, skip, list);
                continue;
            }
        }
    }

    private void MoveTo(Control c, Control dest)
    {
        if (c == null || dest == null || c.Parent == dest || c.Parent == null) return;
        Point sp = c.Parent.PointToScreen(c.Location);
        c.Parent.Controls.Remove(c);
        dest.Controls.Add(c);
        c.BringToFront();
        c.Location = dest.PointToClient(sp);
        c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
    }

    private void ShortenLabels(Control par)
    {
        System.Collections.Generic.List<Control> all = new System.Collections.Generic.List<Control>();
        CollectAll(par, all);

        foreach (Control c in all)
        {
            Label l = c as Label;
            if (l == null) continue;

            string t = l.Text ?? "";
            if (t.Contains("Login Password")) l.Text = "Login Pwd :";
            else if (t.Contains("Valid Thru")) l.Text = "Expiry :";
            else if (t.Contains("Card 3D Password")) l.Text = "3D Password :";
            else if (t.Contains("Visa / Master Pin")) l.Text = "Visa Pin :";
            else if (t.Contains("Card ATM Pin")) l.Text = "ATM Pin :";

            l.AutoSize = true;
            l.Font = new Font("Segoe UI", 9f);
        }
    }

    private void SetupTab(Control tab)
    {
        if (tab == null) return;

        Panel listPanel = null;
        foreach (Control c in tab.Controls)
        {
            Panel p = c as Panel;
            if (p != null && !(p is FieldBox) && HasListBox(p)) { listPanel = p; break; }
        }

        int leftLimit = listPanel != null ? listPanel.Left - 14 : tab.ClientSize.Width - 240;

        ShortenLabels(tab);

        Button save = null;
        CheckBox pwd = null;
        bool pwdDone = false;

        System.Collections.Generic.List<Control> all = new System.Collections.Generic.List<Control>();
        CollectAll(tab, all);

        foreach (Control c in all)
        {
            if (listPanel != null && InsideOf(c, listPanel)) continue;

            CheckBox chk = c as CheckBox;
            if (chk != null && (chk.Text ?? "").ToUpperInvariant().Contains("SHOW"))
            {
                if (!pwdDone) { pwd = chk; pwdDone = true; }
                else chk.Visible = false;
            }

            Button b = c as Button;
            if (b != null && (b.Text ?? "").ToUpperInvariant().Contains("SAVE")) save = b;
        }

        System.Collections.Generic.List<Control> movables = new System.Collections.Generic.List<Control>();
        CollectMovables(tab, listPanel, movables);

        foreach (Control c in movables) MoveTo(c, tab);

        foreach (Control c in movables)
        {
            TextBox tb = c as TextBox;
            ComboBox cb = c as ComboBox;
            if (tb == null && cb == null) continue;

            Control f = (Control)tb ?? cb;
            int maxW = leftLimit - 24 - f.Left;
            if (maxW < 100) maxW = 100;

            if (f.Width > maxW) f.Width = maxW;
            if (f.Width < 130) f.Width = Math.Min(200, maxW);

            if (f.Parent is FieldBox) f.Parent.Width = f.Width + 10;
        }

        foreach (Control c in tab.Controls)
        {
            Panel p = c as Panel;
            if (p == null || p == listPanel || p is FieldBox) continue;
            p.Visible = false;
        }

        foreach (Control c in movables) c.BringToFront();

        int endY = Reflow(tab);

        if (save != null)
        {
            if (save.Parent != tab) MoveTo(save, tab);
            save.SetBounds(16, endY + 2, Math.Max(200, leftLimit - 30), 36);
            save.BringToFront();
        }

        AddCues(tab);
        tab.Invalidate();
    }

    private void CollectAll(Control p, System.Collections.Generic.List<Control> all)
    {
        foreach (Control c in p.Controls)
        {
            all.Add(c);
            CollectAll(c, all);
        }
    }

    private int Reflow(Control tab)
    {
        System.Collections.Generic.List<Control> items = new System.Collections.Generic.List<Control>();

        foreach (Control c in tab.Controls)
        {
            if (!c.Visible) continue;
            if (c is Button) continue;
            if (c is Panel && !(c is FieldBox)) continue;
            items.Add(c);
        }

        items.Sort(delegate (Control a, Control b) { return a.Top.CompareTo(b.Top); });

        System.Collections.Generic.List<System.Collections.Generic.List<Control>> rows =
            new System.Collections.Generic.List<System.Collections.Generic.List<Control>>();

        int i = 0;
        while (i < items.Count)
        {
            int bandTop = items[i].Top;
            System.Collections.Generic.List<Control> row = new System.Collections.Generic.List<Control>();
            while (i < items.Count && items[i].Top - bandTop <= 14) { row.Add(items[i]); i++; }
            rows.Add(row);
        }

        int sum = 0;
        foreach (System.Collections.Generic.List<Control> r in rows)
        {
            int hh = 0;
            foreach (Control c in r) hh = Math.Max(hh, c.Height);
            sum += hh;
        }

        int avail = tab.ClientSize.Height - 34 - 50;
        int gap = 14;

        if (rows.Count > 1)
        {
            int free = avail - sum;
            gap = Math.Max(10, Math.Min(26, free / rows.Count));
        }

        int y = 16;
        foreach (System.Collections.Generic.List<Control> r in rows)
        {
            int bandTop = r[0].Top;
            int hh = 0;
            foreach (Control c in r) hh = Math.Max(hh, c.Height);

            int delta = y - bandTop;
            foreach (Control c in r) c.Top += delta;

            y += hh + gap;
        }

        return y;
    }

    private void AddCues(Control par)
    {
        foreach (Control c in par.Controls)
        {
            TextBox tb = c as TextBox;
            if (tb == null) continue;

            string cue = null;
            int bestLeft = -1;

            foreach (Control l in par.Controls)
            {
                Label lab = l as Label;
                if (lab == null) continue;
                if (Math.Abs(lab.Top - tb.Top) > 16) continue;
                if (lab.Left >= tb.Left) continue;
                if (lab.Left <= bestLeft) continue;
                bestLeft = lab.Left;
                cue = (lab.Text ?? "").Replace(":", "").Replace("*", "").Trim();
            }

            if (string.IsNullOrEmpty(cue)) continue;
            try { SendMessage(tb.Handle, EM_SETCUEBANNER, (IntPtr)1, cue); } catch { }
        }
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
                p.IdleFillColor = Color.FromArgb(245, 146, 28);
                p.IdleForecolor = Color.White;
                p.IdleLineColor = Color.FromArgb(222, 126, 16);
                p.ActiveFillColor = Color.FromArgb(232, 132, 14);
                p.ActiveForecolor = Color.White;
            }
            else
            {
                p.IdleFillColor = Color.White;
                p.IdleForecolor = Color.FromArgb(17, 40, 72);
                p.IdleLineColor = Color.FromArgb(150, 184, 226);
                p.ActiveFillColor = Color.FromArgb(240, 246, 255);
                p.ActiveForecolor = Color.FromArgb(17, 40, 72);
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
        b.ForeColor = Color.FromArgb(17, 40, 72);
        b.Cursor = Cursors.Hand;
    }

    private void StripPaint(object sender, PaintEventArgs e)
    {
        Panel p = sender as Panel;
        if (p == null) return;
        using (Pen line = new Pen(Color.FromArgb(198, 216, 242), 1f))
            e.Graphics.DrawLine(line, 0, p.Height - 1, p.Width, p.Height - 1);
    }

    private void StyleDeep(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            TextBox tb = c as TextBox;
            if (tb != null)
            {
                tb.BorderStyle = BorderStyle.None;
                tb.BackColor = Color.White;
                tb.ForeColor = Color.FromArgb(17, 40, 72);
                tb.Font = new Font("Segoe UI", 10f);
            }

            ComboBox cb = c as ComboBox;
            if (cb != null) StyleCombo(cb);

            ListBox lb = c as ListBox;
            if (lb != null)
            {
                lb.BorderStyle = BorderStyle.FixedSingle;
                lb.BackColor = Color.FromArgb(250, 252, 255);
                lb.ForeColor = Color.FromArgb(17, 40, 72);
                lb.Font = new Font("Segoe UI", 10f);
            }

            Label l = c as Label;
            if (l != null)
            {
                l.ForeColor = Color.FromArgb(28, 52, 88);
                if (l.BackColor != Color.Transparent) l.BackColor = Color.White;
            }

            CheckBox chk = c as CheckBox;
            if (chk != null)
            {
                chk.ForeColor = Color.FromArgb(17, 40, 72);
                chk.BackColor = Color.White;
                chk.Font = new Font("Segoe UI", 9f);
            }

            RadioButton rb = c as RadioButton;
            if (rb != null)
            {
                rb.ForeColor = Color.FromArgb(17, 40, 72);
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
                g.ForeColor = Color.FromArgb(17, 40, 72);
                g.BackColor = Color.White;
            }

            StyleDeep(c);
        }
    }

    private void StyleCombo(ComboBox cb)
    {
        cb.FlatStyle = FlatStyle.Flat;
        cb.BackColor = Color.White;
        cb.ForeColor = Color.FromArgb(17, 40, 72);
        cb.Font = new Font("Segoe UI", 10f);
        cb.DrawMode = DrawMode.OwnerDrawFixed;
        cb.ItemHeight = 26;
        cb.MaxDropDownItems = 20;
        cb.IntegralHeight = false;
        cb.DropDownHeight = 340;
        cb.DrawItem -= ComboDraw;
        cb.DrawItem += ComboDraw;
    }

    private void ComboDraw(object sender, DrawItemEventArgs e)
    {
        ComboBox cb = sender as ComboBox;
        if (cb == null || e.Index < 0) return;

        bool closed = (e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;
        bool sel = !closed && (e.State & DrawItemState.Selected) == DrawItemState.Selected;
        Rectangle r = e.Bounds;

        using (SolidBrush bg = new SolidBrush(sel ? Color.FromArgb(255, 236, 214) : Color.White))
            e.Graphics.FillRectangle(bg, r);

        if (sel)
            using (SolidBrush bar = new SolidBrush(Color.FromArgb(245, 146, 28)))
                e.Graphics.FillRectangle(bar, r.X, r.Y, 3, r.Height);

        TextRenderer.DrawText(e.Graphics, Convert.ToString(cb.Items[e.Index]), cb.Font,
            new Rectangle(r.X + 10, r.Y, r.Width - 14, r.Height), Color.FromArgb(17, 40, 72),
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
    }

    private void StyleFlatButton(Button b)
    {
        string t = (b.Text ?? "").ToUpperInvariant();

        Color bg = Color.FromArgb(245, 146, 28);
        if (t.Contains("DEL") || t.Contains("CLEAR") || t.Contains("REMOVE"))
            bg = Color.FromArgb(220, 38, 38);
        else if (t.Contains("EDIT") || t.Contains("UPDATE"))
            bg = Color.FromArgb(59, 132, 247);

        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.BackColor = bg;
        b.ForeColor = Color.White;
        b.Font = new Font("Segoe UI Semibold", 9.5f);
        b.UseVisualStyleBackColor = false;
        b.Cursor = Cursors.Hand;
        b.FlatAppearance.MouseOverBackColor = Lighten(bg, 0.16f);
        b.FlatAppearance.MouseDownBackColor = Darken(bg, 0.16f);
        RoundControl(b, 7);
        b.Resize += delegate { RoundControl(b, 7); };
    }

    private void StyleGrid(DataGridView d)
    {
        d.EnableHeadersVisualStyles = false;
        d.BackgroundColor = Color.White;
        d.BorderStyle = BorderStyle.None;
        d.GridColor = Color.FromArgb(198, 216, 242);
        d.RowHeadersVisible = false;
        d.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(18, 76, 196);
        d.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        d.DefaultCellStyle.BackColor = Color.White;
        d.DefaultCellStyle.ForeColor = Color.FromArgb(17, 40, 72);
        d.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 236, 214);
        d.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 40, 72);
    }

    private void ByteNovaHeaderPaint(object sender, PaintEventArgs e)
    {
        Rectangle r = pnlHeader.ClientRectangle;
        if (r.Width <= 0 || r.Height <= 0) return;

        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(18, 76, 196), Color.FromArgb(48, 140, 244),
            LinearGradientMode.Horizontal))
            e.Graphics.FillRectangle(bg, r);

        using (SolidBrush orange = new SolidBrush(Color.FromArgb(245, 146, 28)))
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
            using (Pen p = new Pen(Color.FromArgb(198, 216, 242), 1f)) e.Graphics.DrawPath(p, path);
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
