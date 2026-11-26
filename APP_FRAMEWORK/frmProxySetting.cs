using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;
using Bunifu.Framework.UI;
using NEXUS.Properties;

namespace APP_FRAMEWORK;

public class frmProxySetting : Form
{
    internal static List<string> list_IP = new List<string>();

    public static string ipfile;

    internal bool check_;

    internal bool checkProxy;

    public static string mip;

    public static string fileTxt;

    private IContainer components;

    private BunifuElipse bunifuElipse1;

    private Label label10;

    private GroupBox groupBox1;

    private Label label9;

    private Button btn_save;

    private Label label8;

    private Label label7;

    private Label label6;

    private Label label5;

    private TextBox txt_ipPass;

    private TextBox txt_ipUser;

    private TextBox txt_port;

    private TextBox txt_ip;

    private CheckBox checkPrivate;

    private CheckBox checkSystem;

    private CheckBox checkAdmin;

    private Button button1;

    private Label label2;

    private GroupBox groupBox3;

    private GroupBox groupBox2;

    private Panel pnlHeader;

    internal Label formName;

    private GroupBox groupBox4;

    private Label label11;

    private Label label4;

    private Label label3;

    private Label label1;

    private TextBox txt_SLTime;

    private TextBox txt_AcTime;

    private System.Windows.Forms.Timer timer1;

    private Label label12;

    private Label label14;

    private Label label13;

    private Label label15;

    private GroupBox groupBox5;

    private Button btn_delete;

    private ComboBox comboIP;

    private Button btn_check;

    private Panel panel1;

    private GroupBox groupBox6;

    private Button button2;

    private TextBox text_limit;
    private BunifuImageButton btnClose;
    private Label label16;

    public static WebProxy Proxy_ { get; private set; }

    public frmProxySetting()
    {
        InitializeComponent();
        ApplyByteNovaLatestTheme();
        pnlHeader.MouseDown += StatClass.Header_MouseDown;
        pnlHeader.MouseMove += StatClass.Header_MouseMove;
        pnlHeader.MouseUp += StatClass.Header_MouseUp;
    }


    // =====================================================================
    // Express theme (light + bhagwa) - visual layer only.
    // Proxy save / check / delete / limit logic below is untouched.
    // =====================================================================
    private static readonly Color PX_BG = Color.FromArgb(236, 242, 251);
    private static readonly Color PX_ORANGE = Color.FromArgb(245, 146, 28);
    private static readonly Color PX_BLUE = Color.FromArgb(37, 99, 235);
    private static readonly Color PX_NAVY = Color.FromArgb(15, 42, 92);
    private static readonly Color PX_TEXT = Color.FromArgb(30, 41, 59);
    private static readonly Color PX_MUTED = Color.FromArgb(100, 116, 139);
    private static readonly Color PX_RED = Color.FromArgb(220, 38, 38);
    private static readonly Color PX_CARD_LINE = Color.FromArgb(220, 228, 241);
    private static readonly Color PX_FIELD_LINE = Color.FromArgb(203, 213, 225);
    private static readonly Color PX_FRAME = Color.FromArgb(178, 196, 226);
    private static readonly Color PX_HEAD_L = Color.FromArgb(214, 96, 4);
    private static readonly Color PX_HEAD_R = Color.FromArgb(248, 158, 44);

    private const int PX_W = 380, PX_HEAD = 42, PX_FIELD_H = 32, PX_ARROW_W = 22;

    private readonly Font pxFTitle = new Font("Segoe UI Semibold", 11F);
    private readonly Font pxFCard = new Font("Segoe UI", 9F, FontStyle.Bold);
    private readonly Font pxFCaption = new Font("Segoe UI Semibold", 8.25F);
    private readonly Font pxFField = new Font("Segoe UI", 9.75F);
    private readonly Font pxFFieldBold = new Font("Segoe UI Semibold", 9.75F);
    private readonly Font pxFBtn = new Font("Segoe UI Semibold", 9.25F);
    private readonly Font pxFLabel = new Font("Segoe UI Semibold", 9.75F);

    private readonly Dictionary<Control, int> pxState = new Dictionary<Control, int>();   // 0 normal, 1 hover, 2 pressed
    private readonly Dictionary<GroupBox, string[]> pxCards = new Dictionary<GroupBox, string[]>();
    private Control pxHotField;
    private bool pxCloseHot, pxCloseDown;

    [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

    private void ApplyByteNovaLatestTheme()
    {
        SuspendLayout();

        BackColor = PX_BG;
        ForeColor = PX_TEXT;
        TransparencyKey = Color.Empty;
        FormBorderStyle = FormBorderStyle.None;
        DoubleBuffered = true;

        // ---------- header ribbon ----------
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Height = PX_HEAD;
        pnlHeader.BackColor = PX_HEAD_L;
        pnlHeader.BorderStyle = BorderStyle.None;
        PxBuffer(pnlHeader);
        pnlHeader.Paint -= ByteNovaProxyHeaderPaint;
        pnlHeader.Paint += ByteNovaProxyHeaderPaint;

        formName.Enabled = true;
        formName.AutoSize = false;
        formName.Text = "Proxy Setting";
        formName.ForeColor = Color.White;
        formName.BackColor = Color.Transparent;
        formName.Font = pxFTitle;
        formName.UseCompatibleTextRendering = false;
        formName.TextAlign = ContentAlignment.MiddleLeft;
        formName.SetBounds(50, 0, 220, PX_HEAD - 3);

        btnClose.BackColor = Color.Transparent;
        btnClose.BackgroundImage = null;
        btnClose.Zoom = 0;
        btnClose.Cursor = Cursors.Hand;
        btnClose.Size = new Size(36, 37);   // round corner badge (30px circle + hover growth + shadow)
        btnClose.Paint -= PxClosePaint;
        btnClose.Paint += PxClosePaint;
        btnClose.MouseEnter += delegate { pxCloseHot = true; btnClose.Invalidate(); };
        btnClose.MouseLeave += delegate { pxCloseHot = false; pxCloseDown = false; btnClose.Invalidate(); };
        btnClose.MouseDown += delegate { pxCloseDown = true; btnClose.Invalidate(); };
        btnClose.MouseUp += delegate { pxCloseDown = false; if (!btnClose.IsDisposed) btnClose.Invalidate(); };

        // Close is shown as a "sticker" badge on the window's top-right corner (half outside the popup).
        // The in-header button is only hidden; its Click handler is what the sticker calls.
        btnClose.Visible = false;
        PxInitSticker(delegate { btnClose_Click_1(btnClose, EventArgs.Empty); });

        // ---------- cards (the existing group boxes) ----------
        int x = 12, w = PX_W - 24, y = PX_HEAD + 12;

        // PRIVATE IP : IP + Port / Username + Password / Save
        PxCard(groupBox3, "PRIVATE IP", "Add your own proxy server", "server");
        groupBox3.SetBounds(x, y, w, 196);
        int inL = 16, inR = w - 16, gap = 10;
        int col2 = inR - 96;                                   // Port column
        PxCaption(label5, "IP Address", inL, 44);
        PxField(txt_ip, inL, 62, col2 - gap - inL, "e.g. 103.21.244.10");
        PxCaption(label6, "Port", col2, 44);
        PxField(txt_port, col2, 62, inR - col2, "8080");
        int half = (inR - inL - gap) / 2;
        PxCaption(label7, "Username", inL, 100);
        PxField(txt_ipUser, inL, 118, half, "Proxy username");
        PxCaption(label8, "Password", inL + half + gap, 100);
        PxField(txt_ipPass, inL + half + gap, 118, inR - (inL + half + gap), "Proxy password");
        PxButton(btn_save, inR - 150, 152, 150, 34);
        y += 196 + 12;

        // PROXY IP : select + check + delete
        PxCard(groupBox5, "PROXY IP", "Saved proxies", "globe");
        groupBox5.SetBounds(x, y, w, 96);
        PxButton(btn_delete, inR - 84, 48, 84, 34);
        PxButton(btn_check, inR - 84 - 8 - 88, 48, 88, 34);
        PxCombo(comboIP, inL, 48, (inR - 84 - 8 - 88 - 10) - inL);
        y += 96 + 12;

        // divider panel is only decoration in the old skin
        panel1.Visible = false;

        // PROXY LIMIT : label + value + save
        PxCard(groupBox6, "PROXY LIMIT", "Pairs allowed on one proxy", "gauge");
        groupBox6.SetBounds(x, y, w, 96);
        label16.AutoSize = false;
        label16.BackColor = Color.Transparent;
        label16.ForeColor = PX_TEXT;
        label16.Font = pxFLabel;
        label16.UseCompatibleTextRendering = false;
        label16.TextAlign = ContentAlignment.MiddleLeft;
        label16.SetBounds(inL, 48, 150, PX_FIELD_H);
        PxButton(button2, inR - 96, 48, 96, 34);
        PxField(text_limit, inR - 96 - 10 - 76, 49, 76, "1");
        text_limit.TextAlign = HorizontalAlignment.Center;
        y += 96 + 14;

        ClientSize = new Size(PX_W, y);

        // Off-form legacy groups / labels: keep them where the designer put them (outside the card area).
        foreach (Control c in new Control[] { groupBox1, groupBox2, groupBox4, label10, label12, label14, checkPrivate })
            if (c != null && c.Left < PX_W + 20) c.Left = PX_W + 200 + c.Left;

        Paint -= PxFormPaint;
        Paint += PxFormPaint;

        ResumeLayout(false);
        PerformLayout();
        PxPlaceClose();
        pnlHeader.Resize += delegate { PxPlaceClose(); };
        Invalidate(true);
    }

    // ---------------- building blocks ----------------

    private static void PxBuffer(Control c)
    {
        try
        {
            typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(c, true, null);
        }
        catch { }
    }

    private void PxPlaceClose()
    {
        if (btnClose == null || btnClose.IsDisposed) return;
        // badge centre ~ (W - 19, 19): sits on the header's top-right corner like a patch
        btnClose.Location = new Point(Math.Max(0, pnlHeader.Width - btnClose.Width - 1), 1);
        btnClose.BringToFront();
    }

    // Round red "X" badge (shared look across popups). Background is painted by the caller.
    private static void PxCloseBadge(Graphics g, Control c, bool hot, bool down, Color red)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        // match the header's top-half shine so no square edge shows around the badge
        if (c.Parent != null)
        {
            int shineH = (c.Parent.Height - 3) / 2 - c.Top;
            if (shineH > 0)
                using (SolidBrush shine = new SolidBrush(Color.FromArgb(20, 255, 255, 255)))
                    g.FillRectangle(shine, 0, 0, c.Width, shineH);
        }
        float d = 30f + (hot && !down ? 2f : 0f);
        float cx = c.Width / 2f, cy = (c.Height - 2) / 2f + (down ? 1f : 0f);
        RectangleF o = new RectangleF(cx - d / 2f, cy - d / 2f, d, d);
        if (!down)
        {
            using (SolidBrush s1 = new SolidBrush(Color.FromArgb(38, 0, 0, 0)))
                g.FillEllipse(s1, o.X - 0.5f, o.Y + 1.5f, o.Width + 1f, o.Height + 1f);
            using (SolidBrush s2 = new SolidBrush(Color.FromArgb(48, 0, 0, 0)))
                g.FillEllipse(s2, o.X + 0.5f, o.Y + 1.5f, o.Width - 1f, o.Height - 0.5f);
        }
        Color fill = down ? Color.FromArgb(153, 27, 27) : hot ? Color.White : red;
        Color ring = hot && !down ? red : Color.White;
        Color xc = hot && !down ? red : Color.White;
        using (SolidBrush b = new SolidBrush(fill)) g.FillEllipse(b, o);
        using (Pen p = new Pen(ring, 2f)) g.DrawEllipse(p, RectangleF.Inflate(o, -1f, -1f));
        float h = d * 0.19f;
        using (Pen x = new Pen(xc, 2.6f))
        {
            x.StartCap = LineCap.Round; x.EndCap = LineCap.Round;
            g.DrawLine(x, cx - h, cy - h, cx + h, cy + h);
            g.DrawLine(x, cx + h, cy - h, cx - h, cy + h);
        }
    }

    private void PxCard(GroupBox g, string title, string sub, string icon)
    {
        if (g == null) return;
        g.FlatStyle = FlatStyle.Standard;
        g.BackColor = PX_BG;
        g.ForeColor = PX_TEXT;
        g.Font = pxFCard;
        PxBuffer(g);
        pxCards[g] = new[] { title, sub, icon };
        g.Paint -= PxCardPaint;
        g.Paint += PxCardPaint;
        g.MouseDown -= PxCardMouseDown;
        g.MouseDown += PxCardMouseDown;
        g.MouseMove -= PxCardMouseMove;
        g.MouseMove += PxCardMouseMove;
        g.MouseLeave -= PxCardMouseLeave;
        g.MouseLeave += PxCardMouseLeave;
    }

    private void PxCaption(Label l, string text, int x, int y)
    {
        if (l == null) return;
        l.AutoSize = false;
        l.Text = text;
        l.BackColor = Color.Transparent;
        l.ForeColor = PX_MUTED;
        l.Font = pxFCaption;
        l.UseCompatibleTextRendering = false;
        l.TextAlign = ContentAlignment.MiddleLeft;
        l.SetBounds(x, y, 140, 16);
    }

    // TextBox without border, sitting inside a rounded box painted by the card.
    private void PxField(TextBox t, int x, int y, int w, string cue)
    {
        if (t == null) return;
        t.BorderStyle = BorderStyle.None;
        t.BackColor = Color.White;
        t.ForeColor = PX_TEXT;
        t.Font = pxFField;
        int th = t.PreferredHeight;
        t.SetBounds(x + 10, y + (PX_FIELD_H - th) / 2, Math.Max(10, w - 20), th);
        t.Tag = new Rectangle(x, y, w, PX_FIELD_H);
        if (!pxState.ContainsKey(t))
        {
            pxState[t] = 0;
            t.GotFocus += delegate { PxInvField(t); };
            t.LostFocus += delegate { PxInvField(t); };
            t.MouseEnter += delegate { pxHotField = t; PxInvField(t); };
            t.MouseLeave += delegate { if (pxHotField == t) { pxHotField = null; PxInvField(t); } };
            t.HandleCreated += delegate { PxCue(t, cue); };
            // a parent BackColor change must not turn the field grey
            t.BackColorChanged += delegate { if (t.BackColor != Color.White) t.BackColor = Color.White; };
        }
        if (t.IsHandleCreated) PxCue(t, cue);
    }

    private static void PxCue(TextBox t, string cue)
    {
        try { SendMessage(t.Handle, 0x1501 /* EM_SETCUEBANNER */, (IntPtr)1, cue); } catch { }
    }

    private void PxInvField(Control c)
    {
        if (c == null || c.Parent == null || c.Parent.IsDisposed || !(c.Tag is Rectangle)) return;
        c.Parent.Invalidate(Rectangle.Inflate((Rectangle)c.Tag, 3, 3));
    }

    private void PxCombo(ComboBox c, int x, int y, int w)
    {
        if (c == null) return;
        c.FlatStyle = FlatStyle.Flat;
        c.BackColor = Color.White;
        c.ForeColor = PX_TEXT;
        c.Font = pxFField;
        c.DrawMode = DrawMode.OwnerDrawVariable;
        c.ItemHeight = 18;
        c.MaxDropDownItems = 10;
        c.Cursor = Cursors.Hand;
        int ch = c.Height;
        c.SetBounds(x + 4, y + (PX_FIELD_H - ch) / 2, w - 6, ch);
        c.Tag = new Rectangle(x, y, w, PX_FIELD_H);
        PxComboRegion(c);
        if (!pxState.ContainsKey(c))
        {
            pxState[c] = 0;
            c.DrawItem += PxComboDrawItem;
            c.MeasureItem += delegate (object s, MeasureItemEventArgs e) { e.ItemHeight = 26; };
            c.SizeChanged += delegate { PxComboRegion(c); };
            c.GotFocus += delegate { PxInvField(c); };
            c.LostFocus += delegate { PxInvField(c); };
            c.MouseEnter += delegate { pxHotField = c; PxInvField(c); };
            c.MouseLeave += delegate { if (pxHotField == c) { pxHotField = null; PxInvField(c); } };
            c.DropDown += delegate { PxFitDropDown(c); PxInvField(c); };
            c.DropDownClosed += delegate { PxInvField(c); };
            c.SelectedIndexChanged += delegate { PxInvField(c); };
        }
    }

    private void PxComboRegion(ComboBox c)
    {
        if (c.Width > PX_ARROW_W + 6 && c.Height > 4)
            c.Region = new Region(new Rectangle(1, 1, c.Width - PX_ARROW_W - 1, c.Height - 2));
    }

    private void PxFitDropDown(ComboBox c)
    {
        int widest = 0;
        try
        {
            foreach (object it in c.Items)
                widest = Math.Max(widest, TextRenderer.MeasureText(c.GetItemText(it), pxFFieldBold).Width);
        }
        catch { }
        Rectangle box = c.Tag is Rectangle ? (Rectangle)c.Tag : c.Bounds;
        int want = widest + 16 + 30 + SystemInformation.VerticalScrollBarWidth;
        c.DropDownWidth = Math.Min(480, Math.Max(box.Width, want));
    }

    private void PxButton(Button b, int x, int y, int w, int h)
    {
        if (b == null) return;
        b.UseVisualStyleBackColor = false;
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.FlatAppearance.MouseOverBackColor = Color.White;
        b.FlatAppearance.MouseDownBackColor = Color.White;
        b.BackColor = Color.White;
        b.Font = pxFBtn;
        b.Cursor = Cursors.Hand;
        b.Region = null;
        b.SetBounds(x, y, w, h);
        if (!pxState.ContainsKey(b))
        {
            pxState[b] = 0;
            b.MouseEnter += delegate { pxState[b] = 1; b.Invalidate(); };
            b.MouseLeave += delegate { pxState[b] = 0; b.Invalidate(); };
            b.MouseDown += delegate (object s, MouseEventArgs e) { if (e.Button == MouseButtons.Left) { pxState[b] = 2; b.Invalidate(); } };
            b.MouseUp += delegate
            {
                if (b.IsDisposed) return;
                pxState[b] = b.ClientRectangle.Contains(b.PointToClient(MousePosition)) ? 1 : 0;
                b.Invalidate();
            };
            b.Paint += PxButtonPaint;
        }
    }

    private int PxSt(Control c)
    {
        int s;
        return c != null && pxState.TryGetValue(c, out s) ? s : 0;
    }

    // ---------------- painting ----------------

    private static GraphicsPath PxRound(RectangleF r, float rad)
    {
        GraphicsPath p = new GraphicsPath();
        float d = Math.Min(rad * 2, Math.Min(r.Width, r.Height));
        if (d < 1) { p.AddRectangle(r); return p; }
        p.AddArc(r.X, r.Y, d, d, 180, 90);
        p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        p.CloseFigure();
        return p;
    }

    private static void PxFill(Graphics g, RectangleF r, float rad, Color fill, Color border)
    {
        if (r.Width < 1 || r.Height < 1) return;
        using (GraphicsPath p = PxRound(r, rad))
        {
            using (SolidBrush b = new SolidBrush(fill)) g.FillPath(b, p);
            if (border.A > 0) using (Pen pen = new Pen(border, 1F)) g.DrawPath(pen, p);
        }
    }

    private static Color PxMix(Color a, Color b, float t)
    {
        return Color.FromArgb(a.A,
            (int)(a.R + (b.R - a.R) * t), (int)(a.G + (b.G - a.G) * t), (int)(a.B + (b.B - a.B) * t));
    }

    private void PxFormPaint(object sender, PaintEventArgs e)
    {
        if (ClientSize.Width < 4 || ClientSize.Height < 4) return;
        e.Graphics.Clear(PX_BG);
        using (Pen p = new Pen(PX_FRAME))
            e.Graphics.DrawRectangle(p, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
    }

    private void ByteNovaProxyHeaderPaint(object sender, PaintEventArgs e)
    {
        Rectangle r = pnlHeader.ClientRectangle;
        if (r.Width < 2 || r.Height < 4) return;
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (LinearGradientBrush bg = new LinearGradientBrush(r, PX_HEAD_L, PX_HEAD_R, LinearGradientMode.Horizontal))
            g.FillRectangle(bg, r);
        using (SolidBrush shine = new SolidBrush(Color.FromArgb(20, 255, 255, 255)))
            g.FillRectangle(shine, 0, 0, r.Width, (r.Height - 3) / 2);
        using (SolidBrush navy = new SolidBrush(PX_NAVY))
            g.FillRectangle(navy, 0, r.Height - 3, r.Width, 3);

        // icon badge
        int h = r.Height - 3;
        Rectangle badge = new Rectangle(12, (h - 28) / 2, 28, 28);
        using (SolidBrush sh = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
            g.FillEllipse(sh, badge.X, badge.Y + 1, badge.Width, badge.Height);
        using (SolidBrush wb = new SolidBrush(Color.White))
            g.FillEllipse(wb, badge);
        PxIcon(g, "shield", new RectangleF(badge.X + 7, badge.Y + 6, 14, 16), PX_HEAD_L);
    }


    // ===== close "sticker" (same approach as frmProcessTicket) =====
    // A round red X badge whose centre sits ON the popup's top-right corner (about half of it outside
    // the window). WinForms can't paint outside a form, so it is a tiny owned layered window
    // (WS_EX_LAYERED | WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE) drawn with per-pixel alpha.
    // Its click calls the existing close Click handler, so the close behaviour is unchanged.
    private PxSticker pxSticker;

    private void PxInitSticker(MethodInvoker click)
    {
        if (pxSticker != null) return;
        pxSticker = new PxSticker(this, click);
        EventHandler sync = delegate { PxSyncSticker(); };
        Move += sync;
        Resize += sync;
        LocationChanged += sync;
        SizeChanged += sync;
        VisibleChanged += sync;
        Activated += sync;
        Shown += sync;
        HandleDestroyed += delegate { if (pxSticker != null) pxSticker.DropHandle(); };   // recreated on next sync
        FormClosed += delegate { PxKillSticker(); };
        Disposed += delegate { PxKillSticker(); };
    }

    private void PxSyncSticker()
    {
        if (pxSticker == null || IsDisposed || Disposing || !IsHandleCreated) return;
        pxSticker.Sync(Visible && WindowState != FormWindowState.Minimized);
    }

    private void PxKillSticker()
    {
        if (pxSticker == null) return;
        pxSticker.Dispose();
        pxSticker = null;
    }

    private sealed class PxSticker : NativeWindow, IDisposable
    {
        private const int WS_POPUP = unchecked((int)0x80000000);
        private const int WS_EX_LAYERED = 0x80000, WS_EX_TOOLWINDOW = 0x80, WS_EX_NOACTIVATE = 0x8000000;
        private const int SW_HIDE = 0, SW_SHOWNOACTIVATE = 4;

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct XPoint { public int X, Y; }
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct XSize { public int CX, CY; }
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
        private struct XBlend { public byte Op, Flags, Alpha, Format; }
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct XTrack { public int cbSize, dwFlags; public IntPtr hwnd; public int dwHoverTime; }

        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern IntPtr GetDC(IntPtr h);
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern int ReleaseDC(IntPtr h, IntPtr dc);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")] private static extern IntPtr CreateCompatibleDC(IntPtr dc);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")] private static extern bool DeleteDC(IntPtr dc);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")] private static extern IntPtr SelectObject(IntPtr dc, IntPtr o);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")] private static extern bool DeleteObject(IntPtr o);
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern bool UpdateLayeredWindow(IntPtr h, IntPtr dst, ref XPoint pt, ref XSize sz, IntPtr src, ref XPoint srcPt, int key, ref XBlend blend, int flags);
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern bool ShowWindow(IntPtr h, int cmd);
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern bool IsWindowVisible(IntPtr h);
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern IntPtr SetCapture(IntPtr h);
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern bool ReleaseCapture();
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern IntPtr SetCursor(IntPtr c);
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern bool TrackMouseEvent(ref XTrack t);

        private readonly Form owner;
        private readonly MethodInvoker click;
        private bool hot, down, tracking, dead;
        private float k = 1f;
        private int size;

        public PxSticker(Form owner, MethodInvoker click)
        {
            this.owner = owner;
            this.click = click;
        }

        public bool Hot { get { return hot; } set { hot = value; Render(); } }
        public IntPtr Hwnd { get { return Handle; } }

        public void Sync(bool show)
        {
            if (dead) return;
            if (!show)
            {
                if (Handle != IntPtr.Zero) ShowWindow(Handle, SW_HIDE);
                hot = down = false;
                return;
            }
            if (Handle == IntPtr.Zero)
            {
                try { using (Graphics g = Graphics.FromHwnd(owner.Handle)) k = Math.Max(1f, g.DpiX / 96f); }
                catch { k = 1f; }
                size = (int)Math.Ceiling(46f * k);          // 32px disc + hover growth + shadow
                CreateParams cp = new CreateParams();
                cp.Caption = "";
                cp.X = 0; cp.Y = 0; cp.Width = size; cp.Height = size;
                cp.Style = WS_POPUP;
                cp.ExStyle = WS_EX_LAYERED | WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE;
                cp.Parent = owner.Handle;                     // = owner for a popup: stays above it, hides/minimises with it
                CreateHandle(cp);
            }
            Render();
            if (!IsWindowVisible(Handle)) ShowWindow(Handle, SW_SHOWNOACTIVATE);
        }

        public void DropHandle()
        {
            if (Handle != IntPtr.Zero) DestroyHandle();
        }

        public void Dispose()
        {
            dead = true;
            DropHandle();
        }

        // Badge centre sits on the owner's top-right corner, (Right - 8, Top + 8) in screen pixels.
        private Point Origin()
        {
            Rectangle ob = owner.Bounds;
            int cx = ob.Right - (int)Math.Round(8f * k), cy = ob.Top + (int)Math.Round(8f * k);
            return new Point(cx - size / 2, cy - size / 2);
        }

        private void Render()
        {
            if (dead || Handle == IntPtr.Zero || size <= 0) return;
            using (Bitmap bmp = new Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.Clear(Color.Transparent);
                    Draw(g);
                }
                IntPtr screen = GetDC(IntPtr.Zero);
                IntPtr mem = CreateCompatibleDC(screen);
                IntPtr hbmp = bmp.GetHbitmap(Color.FromArgb(0));   // 32bpp, premultiplied for UpdateLayeredWindow
                IntPtr old = SelectObject(mem, hbmp);
                try
                {
                    Point o = Origin();
                    XPoint dst = new XPoint { X = o.X, Y = o.Y };
                    XPoint src = new XPoint();
                    XSize sz = new XSize { CX = size, CY = size };
                    XBlend bl = new XBlend { Op = 0, Flags = 0, Alpha = 255, Format = 1 };   // AC_SRC_OVER, AC_SRC_ALPHA
                    UpdateLayeredWindow(Handle, screen, ref dst, ref sz, mem, ref src, 0, ref bl, 2);  // ULW_ALPHA
                }
                finally
                {
                    SelectObject(mem, old);
                    DeleteObject(hbmp);
                    DeleteDC(mem);
                    ReleaseDC(IntPtr.Zero, screen);
                }
            }
        }

        private float Radius() { return (16f + (hot && !down ? 1f : 0f)) * k; }

        private bool InDisc(int x, int y)
        {
            float c = size / 2f, r = Radius() + 1f * k;
            float dx = x - c, dy = y - c;
            return dx * dx + dy * dy <= r * r;
        }

        private void Draw(Graphics g)
        {
            bool h = hot && !down;
            float r = Radius();
            float cx = size / 2f, cy = size / 2f + (down ? k : 0f);
            RectangleF disc = new RectangleF(cx - r, cy - r, r * 2, r * 2);
            if (!down)
            {
                using (SolidBrush s1 = new SolidBrush(Color.FromArgb(40, 0, 0, 0)))
                    g.FillEllipse(s1, disc.X - 0.8f * k, disc.Y + 2.6f * k, disc.Width + 1.6f * k, disc.Height + 0.6f * k);
                using (SolidBrush s2 = new SolidBrush(Color.FromArgb(55, 0, 0, 0)))
                    g.FillEllipse(s2, disc.X + 0.4f * k, disc.Y + 1.5f * k, disc.Width - 0.8f * k, disc.Height - 0.4f * k);
            }
            Color red = Color.FromArgb(220, 38, 38);
            using (SolidBrush rb = new SolidBrush(h ? red : Color.White)) g.FillEllipse(rb, disc);
            RectangleF inner = RectangleF.Inflate(disc, -2f * k, -2f * k);
            using (SolidBrush fb = new SolidBrush(down ? Color.FromArgb(153, 27, 27) : h ? Color.White : red)) g.FillEllipse(fb, inner);
            if (!h && !down)
                using (SolidBrush gl = new SolidBrush(Color.FromArgb(40, 255, 255, 255)))
                    g.FillEllipse(gl, inner.X + inner.Width * 0.18f, inner.Y + 1.5f * k, inner.Width * 0.64f, inner.Height * 0.42f);
            float a = r * 0.34f;
            using (Pen x = new Pen(h ? red : Color.White, 2.8f * k))
            {
                x.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                x.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                g.DrawLine(x, cx - a, cy - a, cx + a, cy + a);
                g.DrawLine(x, cx + a, cy - a, cx - a, cy + a);
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0021:   // WM_MOUSEACTIVATE -> never take focus from the popup
                    m.Result = (IntPtr)3;   // MA_NOACTIVATE
                    return;
                case 0x0020:   // WM_SETCURSOR
                    SetCursor(Cursors.Hand.Handle);
                    m.Result = (IntPtr)1;
                    return;
                case 0x0084:   // WM_NCHITTEST: only the disc is clickable, the rest is see-through
                {
                    long lp = m.LParam.ToInt64();
                    int sx = (short)(lp & 0xFFFF), sy = (short)((lp >> 16) & 0xFFFF);
                    Point o = Origin();
                    m.Result = InDisc(sx - o.X, sy - o.Y) ? (IntPtr)1 : (IntPtr)(-1);   // HTCLIENT / HTTRANSPARENT
                    return;
                }
                case 0x0200:   // WM_MOUSEMOVE
                    if (!tracking)
                    {
                        XTrack t = new XTrack { cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(XTrack)), dwFlags = 2, hwnd = Handle };
                        tracking = TrackMouseEvent(ref t);
                    }
                    if (!hot) { hot = true; Render(); }
                    m.Result = IntPtr.Zero;
                    return;
                case 0x02A3:   // WM_MOUSELEAVE
                    tracking = false;
                    if (hot || down) { hot = false; down = false; Render(); }
                    return;
                case 0x0201:   // WM_LBUTTONDOWN
                    down = true;
                    SetCapture(Handle);
                    Render();
                    m.Result = IntPtr.Zero;
                    return;
                case 0x0202:   // WM_LBUTTONUP
                {
                    bool was = down;
                    down = false;
                    ReleaseCapture();
                    long lp = m.LParam.ToInt64();
                    int x = (short)(lp & 0xFFFF), y = (short)((lp >> 16) & 0xFFFF);
                    bool inside = InDisc(x, y);
                    hot = inside;
                    Render();
                    if (was && inside && !dead && owner != null && !owner.IsDisposed && owner.IsHandleCreated)
                        owner.BeginInvoke(click);   // exactly the existing close click path
                    m.Result = IntPtr.Zero;
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }
    private void PxClosePaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // repaint the ribbon underneath so the old image never shows
        Control p = c.Parent;
        int pw = p != null ? Math.Max(1, p.Width) : c.Width;
        int ph = p != null ? Math.Max(1, p.Height) : c.Height;
        using (LinearGradientBrush bg = new LinearGradientBrush(new Rectangle(-c.Left, -c.Top, pw, ph), PX_HEAD_L, PX_HEAD_R, LinearGradientMode.Horizontal))
            g.FillRectangle(bg, 0, 0, c.Width, c.Height);

        // default: red with white X / hover: white with red X + red border / pressed: dark red
        PxCloseBadge(g, c, pxCloseHot, pxCloseDown, PX_RED);
    }

    private void PxCardPaint(object sender, PaintEventArgs e)
    {
        GroupBox gb = sender as GroupBox;
        if (gb == null || gb.Width < 10 || gb.Height < 10) return;
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(PX_BG);   // hides the native group-box frame + caption

        RectangleF card = new RectangleF(0.5f, 0.5f, gb.Width - 2, gb.Height - 3);
        PxFill(g, new RectangleF(card.X, card.Y + 2, card.Width, card.Height), 10, Color.FromArgb(214, 225, 242), Color.Empty);
        PxFill(g, card, 10, Color.White, PX_CARD_LINE);
        PxFill(g, new RectangleF(1, 12, 3.5f, 22), 1.5f, PX_ORANGE, Color.Empty);

        string[] info;
        if (pxCards.TryGetValue(gb, out info))
        {
            Rectangle ib = new Rectangle(14, 10, 26, 26);
            PxFill(g, ib, 7, Color.FromArgb(255, 237, 213), Color.Empty);
            PxIcon(g, info[2], new RectangleF(ib.X + 6, ib.Y + 6, 14, 14), Color.FromArgb(214, 96, 4));
            TextRenderer.DrawText(g, info[0], pxFCard, new Point(ib.Right + 8, 9), PX_NAVY, TextFormatFlags.NoPadding);
            TextRenderer.DrawText(g, info[1], pxFCaption, new Point(ib.Right + 8, 25), PX_MUTED, TextFormatFlags.NoPadding);
        }

        // field + dropdown boxes for the children that carry a box rectangle in Tag
        foreach (Control c in gb.Controls)
        {
            if (!c.Visible || !(c.Tag is Rectangle)) continue;
            Rectangle b = (Rectangle)c.Tag;
            bool focus = c.Focused || (c is ComboBox && ((ComboBox)c).DroppedDown);
            bool hot = c == pxHotField;
            Color line = focus ? PX_BLUE : hot ? PX_ORANGE : PX_FIELD_LINE;
            if (focus)
                PxFill(g, new RectangleF(b.X - 2.5f, b.Y - 2.5f, b.Width + 4, b.Height + 4), 9, Color.FromArgb(40, PX_BLUE), Color.Empty);
            PxFill(g, new RectangleF(b.X, b.Y, b.Width - 1, b.Height - 1), 7, c.Enabled ? Color.White : Color.FromArgb(241, 245, 249), line);

            ComboBox cb = c as ComboBox;
            if (cb != null)
            {
                int zx = b.Right - PX_ARROW_W - 2;
                using (Pen sep = new Pen(Color.FromArgb(232, 237, 245)))
                    g.DrawLine(sep, zx, b.Y + 7, zx, b.Bottom - 8);
                float ax = zx + PX_ARROW_W / 2f + 1, ay = b.Y + b.Height / 2f;
                bool open = cb.DroppedDown;
                using (Pen p = new Pen(focus ? PX_BLUE : hot ? PX_ORANGE : PX_MUTED, 1.8F))
                {
                    p.StartCap = LineCap.Round; p.EndCap = LineCap.Round; p.LineJoin = LineJoin.Round;
                    if (open) g.DrawLines(p, new[] { new PointF(ax - 4, ay + 2), new PointF(ax, ay - 2), new PointF(ax + 4, ay + 2) });
                    else g.DrawLines(p, new[] { new PointF(ax - 4, ay - 2), new PointF(ax, ay + 2), new PointF(ax + 4, ay - 2) });
                }
            }
        }
    }

    private Control PxFieldAt(GroupBox gb, Point p)
    {
        foreach (Control c in gb.Controls)
            if (c.Visible && c.Enabled && c.Tag is Rectangle && ((Rectangle)c.Tag).Contains(p)) return c;
        return null;
    }

    private void PxCardMouseDown(object sender, MouseEventArgs e)
    {
        GroupBox gb = sender as GroupBox;
        if (gb == null || e.Button != MouseButtons.Left) return;
        Control c = PxFieldAt(gb, e.Location);
        if (c == null) return;
        c.Focus();
        ComboBox cb = c as ComboBox;
        if (cb != null) cb.DroppedDown = true;
    }

    private void PxCardMouseMove(object sender, MouseEventArgs e)
    {
        GroupBox gb = sender as GroupBox;
        if (gb == null) return;
        Control c = PxFieldAt(gb, e.Location);
        if (c != pxHotField)
        {
            Control old = pxHotField;
            pxHotField = c;
            PxInvField(old);
            PxInvField(c);
        }
        gb.Cursor = c is ComboBox ? Cursors.Hand : c != null ? Cursors.IBeam : Cursors.Default;
    }

    private void PxCardMouseLeave(object sender, EventArgs e)
    {
        GroupBox gb = sender as GroupBox;
        if (pxHotField == null || gb == null || pxHotField.Parent != gb) return;
        if (pxHotField.ClientRectangle.Contains(pxHotField.PointToClient(MousePosition))) return;
        Control old = pxHotField;
        pxHotField = null;
        PxInvField(old);
    }

    private void PxComboDrawItem(object sender, DrawItemEventArgs e)
    {
        ComboBox c = sender as ComboBox;
        if (c == null || e.Index < 0) return;
        Graphics g = e.Graphics;
        bool edit = (e.State & DrawItemState.ComboBoxEdit) != 0;
        bool sel = !edit && (e.State & DrawItemState.Selected) != 0;
        bool current = !edit && e.Index == c.SelectedIndex;
        string text = c.GetItemText(c.Items[e.Index]);
        bool placeholder = e.Index == 0 && text.StartsWith("Select", StringComparison.OrdinalIgnoreCase);

        using (SolidBrush b = new SolidBrush(Color.White)) g.FillRectangle(b, e.Bounds);

        if (edit)
        {
            TextRenderer.DrawText(g, text, placeholder ? pxFField : pxFFieldBold,
                new Rectangle(e.Bounds.X + 6, e.Bounds.Y, e.Bounds.Width - 8, e.Bounds.Height),
                placeholder ? Color.FromArgb(148, 163, 184) : PX_TEXT,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
            return;
        }

        g.SmoothingMode = SmoothingMode.AntiAlias;
        if (sel)
        {
            PxFill(g, new RectangleF(e.Bounds.X + 3, e.Bounds.Y + 2, e.Bounds.Width - 7, e.Bounds.Height - 4), 6, Color.FromArgb(255, 237, 213), Color.Empty);
            PxFill(g, new RectangleF(e.Bounds.X + 3, e.Bounds.Y + 6, 3, e.Bounds.Height - 12), 1.5f, PX_ORANGE, Color.Empty);
        }
        Color tc = placeholder ? Color.FromArgb(148, 163, 184) : sel ? Color.FromArgb(154, 52, 18) : current ? PX_BLUE : PX_TEXT;
        TextRenderer.DrawText(g, text, sel || current ? pxFFieldBold : pxFField,
            new Rectangle(e.Bounds.X + 14, e.Bounds.Y, e.Bounds.Width - 34, e.Bounds.Height), tc,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
            TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        if (current && !placeholder)
            using (Pen t = new Pen(sel ? PX_ORANGE : PX_BLUE, 1.8F))
            {
                t.StartCap = LineCap.Round; t.EndCap = LineCap.Round;
                float cx = e.Bounds.Right - 15, cy = e.Bounds.Y + e.Bounds.Height / 2f;
                g.DrawLines(t, new[] { new PointF(cx - 4, cy), new PointF(cx - 1, cy + 3), new PointF(cx + 4, cy - 3) });
            }
    }

    private void PxButtonPaint(object sender, PaintEventArgs e)
    {
        Button b = sender as Button;
        if (b == null) return;
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Color.White);    // buttons sit on white cards

        string label, icon;
        int kind;   // 0 = bhagwa primary, 1 = blue, 2 = red outline
        if (b == btn_check) { label = "Check"; icon = "check"; kind = 1; }
        else if (b == btn_delete) { label = "Delete"; icon = "trash"; kind = 2; }
        else if (b == btn_save) { label = "Save Proxy"; icon = "save"; kind = 0; }
        else { label = "Save"; icon = "save"; kind = 0; }

        int st = PxSt(b);
        bool on = b.Enabled;
        RectangleF r = new RectangleF(0.5f, st == 2 ? 1.5f : 0.5f, b.Width - 2, b.Height - 3);
        Color fg;
        if (kind == 2)
        {
            Color fill = st == 2 ? Color.FromArgb(254, 202, 202) : st == 1 ? Color.FromArgb(254, 226, 226) : Color.FromArgb(254, 242, 242);
            PxFill(g, r, 8, fill, st == 0 ? Color.FromArgb(252, 165, 165) : PX_RED);
            fg = on ? Color.FromArgb(200, 30, 30) : Color.FromArgb(248, 180, 180);
        }
        else
        {
            Color top = kind == 0 ? Color.FromArgb(252, 170, 66) : Color.FromArgb(72, 130, 245);
            Color bot = kind == 0 ? Color.FromArgb(236, 122, 8) : Color.FromArgb(29, 78, 216);
            if (st == 1) { top = PxMix(top, Color.White, 0.15f); bot = PxMix(bot, Color.White, 0.10f); }
            if (st == 2) { top = PxMix(top, Color.Black, 0.12f); bot = PxMix(bot, Color.Black, 0.15f); }
            if (!on) { top = PxMix(top, Color.White, 0.55f); bot = PxMix(bot, Color.White, 0.55f); }
            if (st != 2 && on)
                PxFill(g, new RectangleF(r.X, r.Y + 1.5f, r.Width, r.Height), 8, Color.FromArgb(st == 1 ? 80 : 45, bot), Color.Empty);
            using (GraphicsPath p = PxRound(r, 8))
            using (LinearGradientBrush lb = new LinearGradientBrush(r, top, bot, LinearGradientMode.Vertical))
                g.FillPath(lb, p);
            fg = Color.White;
        }

        Size ts = TextRenderer.MeasureText(g, label, pxFBtn, Size.Empty, TextFormatFlags.NoPadding);
        int gw = 14 + 7 + ts.Width;
        bool withIcon = gw <= r.Width - 10;
        if (!withIcon) gw = ts.Width;
        int x = (int)(r.X + (r.Width - gw) / 2);
        int cy = (int)(r.Y + r.Height / 2);
        if (withIcon)
        {
            PxIcon(g, icon, new RectangleF(x, cy - 7, 14, 14), fg);
            x += 21;
        }
        TextRenderer.DrawText(g, label, pxFBtn, new Rectangle(x, cy - ts.Height / 2 - 1, ts.Width + 4, ts.Height + 2), fg,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }

    private static void PxIcon(Graphics g, string kind, RectangleF r, Color c)
    {
        SmoothingMode old = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        float x = r.X, y = r.Y, w = r.Width, h = r.Height;
        using (Pen p = new Pen(c, 1.6F))
        using (SolidBrush b = new SolidBrush(c))
        {
            p.StartCap = LineCap.Round; p.EndCap = LineCap.Round; p.LineJoin = LineJoin.Round;
            switch (kind)
            {
                case "shield":
                    g.FillPolygon(b, new[]
                    {
                        new PointF(x + w / 2, y), new PointF(x + w, y + h * 0.18f), new PointF(x + w * 0.9f, y + h * 0.62f),
                        new PointF(x + w / 2, y + h), new PointF(x + w * 0.1f, y + h * 0.62f), new PointF(x, y + h * 0.18f)
                    });
                    using (Pen t = new Pen(Color.White, 1.6F))
                    {
                        t.StartCap = LineCap.Round; t.EndCap = LineCap.Round;
                        g.DrawLines(t, new[] { new PointF(x + w * 0.3f, y + h * 0.5f), new PointF(x + w * 0.46f, y + h * 0.65f), new PointF(x + w * 0.72f, y + h * 0.36f) });
                    }
                    break;
                case "server":
                    using (GraphicsPath a = PxRound(new RectangleF(x, y, w, h * 0.42f), 2)) g.DrawPath(p, a);
                    using (GraphicsPath a = PxRound(new RectangleF(x, y + h * 0.58f, w, h * 0.42f), 2)) g.DrawPath(p, a);
                    g.FillEllipse(b, x + w * 0.72f, y + h * 0.15f, 2.4f, 2.4f);
                    g.FillEllipse(b, x + w * 0.72f, y + h * 0.73f, 2.4f, 2.4f);
                    break;
                case "globe":
                    g.DrawEllipse(p, x, y, w, h);
                    g.DrawEllipse(p, x + w * 0.28f, y, w * 0.44f, h);
                    g.DrawLine(p, x, y + h / 2, x + w, y + h / 2);
                    break;
                case "gauge":
                    g.DrawArc(p, x, y + h * 0.1f, w, h * 1.2f, 180, 180);
                    g.DrawLine(p, x + w / 2, y + h * 0.7f, x + w * 0.78f, y + h * 0.3f);
                    g.FillEllipse(b, x + w / 2 - 2, y + h * 0.7f - 2, 4, 4);
                    break;
                case "check":
                    g.DrawEllipse(p, x, y, w, h);
                    g.DrawLines(p, new[] { new PointF(x + w * 0.28f, y + h * 0.52f), new PointF(x + w * 0.44f, y + h * 0.68f), new PointF(x + w * 0.74f, y + h * 0.36f) });
                    break;
                case "trash":
                    g.DrawLine(p, x, y + h * 0.2f, x + w, y + h * 0.2f);
                    g.DrawLine(p, x + w * 0.35f, y, x + w * 0.65f, y);
                    g.DrawPolygon(p, new[] { new PointF(x + w * 0.14f, y + h * 0.3f), new PointF(x + w * 0.86f, y + h * 0.3f), new PointF(x + w * 0.76f, y + h), new PointF(x + w * 0.24f, y + h) });
                    break;
                case "save":
                    using (GraphicsPath a = PxRound(new RectangleF(x, y, w, h), 2)) g.DrawPath(p, a);
                    g.DrawRectangle(p, x + w * 0.25f, y, w * 0.5f, h * 0.32f);
                    g.FillRectangle(b, x + w * 0.25f, y + h * 0.6f, w * 0.5f, h * 0.4f);
                    break;
            }
        }
        g.SmoothingMode = old;
    }


    private void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            string str = "";
            string[] array = new string[7] { txt_ip.Text, null, null, null, null, null, null };
            string text = "||";
            array[1] = text;
            array[2] = txt_port.Text;
            string text2 = "||";
            array[3] = text2;
            array[4] = txt_ipUser.Text;
            string text3 = "||";
            array[5] = text3;
            array[6] = txt_ipPass.Text;
            string parameter = string.Concat(array);
            if (string.IsNullOrEmpty(txt_ip.Text) | string.IsNullOrEmpty(txt_port.Text) | string.IsNullOrEmpty(txt_ipUser.Text) | string.IsNullOrEmpty(txt_ipPass.Text))
            {
                Form owner = new Form
                {
                    TopMost = true
                };
                string text4 = "Please Enter Details Propperly";
                string caption = "Information";
                MessageBox.Show(owner, text4, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (list_IP.FirstOrDefault((string stringToCheck) => stringToCheck.Contains(parameter)) == null)
            {
                list_IP.Add(parameter);
                string text6 = null;
                foreach (string item in list_IP)
                {
                    if (!string.IsNullOrEmpty(text6))
                    {
                        string text7 = text6;
                        string text8 = "####";
                        text6 = text7 + text8 + item;
                    }
                    else
                    {
                        text6 = item;
                    }
                }
                string path = "IRLOG//IP.Proxy";
                File.WriteAllText(path, text6);
                txt_ip.Text = null;
                txt_port.Text = null;
                txt_ipUser.Text = null;
                txt_ipPass.Text = null;
            }
            ipAdd();
                Form owner4 = new Form
                {
                    TopMost = true
                };
                string text12 = "Save SuccessFully";
                string caption4 = "Information";
                MessageBox.Show(owner4, text12, caption4, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        catch
        {
        }
    }

    private void FrmProxySetting_Load(object sender, EventArgs e)
    {
        try
        {
            text_limit.Text = Program.mySettings_0.perIPlimit;
            label9.Text = mip;
            if (string.IsNullOrEmpty(Program.mySettings_0.Ac_avl) || string.IsNullOrEmpty(Program.mySettings_0.Sl_avl))
            {
                TextBox textBox = txt_AcTime;
                string text = "53";
                textBox.Text = text;
                TextBox textBox2 = txt_SLTime;
                string text2 = "47";
                textBox2.Text = text2;
            }
            else
            {
                txt_AcTime.Text = Program.mySettings_0.Ac_avl;
                txt_SLTime.Text = Program.mySettings_0.Sl_avl;
            }
            ipAdd();
        }
        catch
        {
        }
    }

    public void ipAdd()
    {
        comboIP.Items.Clear();
        ComboBox.ObjectCollection items = comboIP.Items;
        string item2 = "Select Proxy";
        items.Add(item2);
        comboIP.SelectedIndex = 0;
        string text = "IRLOG//IP.Proxy";
        ipfile = text;
        if (File.Exists(ipfile))
        {
            fileTxt = File.ReadAllText(ipfile);
            string text2 = fileTxt;
            string[] array = new string[1];
            string text3 = "####";
            array[0] = text3;
            string[] array2 = text2.Split(array, StringSplitOptions.None);
            string[] array3 = array2;
            foreach (string item in array3)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    if (Program.list_Proxy.FirstOrDefault((string stringToCheck) => stringToCheck.Contains(item)) == null)
                    {
                        Program.int_Slot += 2;
                        Program.list_Proxy.Add(item);
                    }
                    if (list_IP.FirstOrDefault((string stringToCheck) => stringToCheck.Contains(item)) == null)
                    {
                        list_IP.Add(item);
                    }
                    if (!comboIP.Items.Contains(item))
                    {
                        string text4 = item;
                        string[] array4 = new string[1];
                        string text5 = "||";
                        array4[0] = text5;
                        string[] array5 = text4.Split(array4, StringSplitOptions.None);
                        comboIP.Items.Add(array5[0]);
                    }
                }
            }
        }
        Program.int_Slot = 2 + Program.list_Proxy.Count * 2;
    }

    internal void m0()
    {
        Thread thread = new Thread(GetLocalIPAddress);
        thread.IsBackground = true;
        thread.Start(0);
    }

    private void CheckAdmin_CheckedChanged(object sender, EventArgs e)
    {
        if (checkAdmin.Checked)
        {
            mySettings mySettings_ = Program.mySettings_0;
            string proxy_use = "admin";
            mySettings_.Proxy_use = proxy_use;
            checkPrivate.Checked = false;
            checkSystem.Checked = false;
        }
    }

    private void CheckSystem_CheckedChanged(object sender, EventArgs e)
    {
        if (checkSystem.Checked)
        {
            mySettings mySettings_ = Program.mySettings_0;
            string proxy_use = "system";
            mySettings_.Proxy_use = proxy_use;
            checkPrivate.Checked = false;
            checkAdmin.Checked = false;
        }
    }

    private void CheckPrivate_CheckedChanged(object sender, EventArgs e)
    {
        if (checkPrivate.Checked)
        {
            mySettings mySettings_ = Program.mySettings_0;
            string proxy_use = "private";
            mySettings_.Proxy_use = proxy_use;
            checkSystem.Checked = false;
            checkAdmin.Checked = false;
        }
    }

    internal void GetLocalIPAddress(object j)
    {
        string variable = "PrivateProxy";
        Environment.SetEnvironmentVariable(variable, "", EnvironmentVariableTarget.Process);
        string text = null;
        while (true)
        {
            try
            {
                string requestUriString = "http://checkip.dyndns.org";
                text = new StreamReader(WebRequest.Create(requestUriString).GetResponse().GetResponseStream()).ReadToEnd().Trim().Split(':')[1].Substring(1).Split('<')[0];
            }
            catch
            {
                continue;
            }
            break;
        }
        mip = text;
        label9.Text = mip;
        Program.bool_SystemIP = true;
        try
        {
            string requestUriString2 = "https://www.irctc.co.in/nget/train-search";
            WebRequest webRequest = WebRequest.Create(requestUriString2);
            webRequest.Proxy = Proxy_;
            webRequest.Timeout = 11000;
            string text2 = new StreamReader(webRequest.GetResponse().GetResponseStream()).ReadToEnd().Trim();
            string value = "IRCTC Next Generation eTicketing System";
            if (text2.Contains(value))
            {
                Label label = label12;
                string text3 = "IRCTC Server is Working on Your IP Adress";
                label.Text = text3;
                label12.BackColor = Color.LightCyan;
                label12.ForeColor = Color.Black;
            }
        }
        catch
        {
            Program.bool_SystemIP = false;
        }
        int num = 1;
        foreach (string item in Program.list_Proxy)
        {
            num++;
            string[] array = new string[1];
            string text4 = "||";
            array[0] = text4;
            string[] array2 = item.Split(array, StringSplitOptions.None);
            Proxy_ = new WebProxy(array2[0].Trim(), int.Parse(array2[1].Trim()));
            Proxy_.BypassProxyOnLocal = true;
            Proxy_.Credentials = new NetworkCredential(array2[2].Trim(), array2[3].Trim());
            try
            {
                string requestUriString3 = "https://www.irctc.co.in/nget/train-search";
                WebRequest webRequest2 = WebRequest.Create(requestUriString3);
                webRequest2.Proxy = Proxy_;
                webRequest2.Timeout = 11000;
                string text5 = new StreamReader(webRequest2.GetResponse().GetResponseStream()).ReadToEnd().Trim();
                string value2 = "IRCTC Next Generation eTicketing System";
                if (text5.Contains(value2))
                {
                    string variable2 = "PrivateProxy";
                    Environment.SetEnvironmentVariable(variable2, item.ToString(), EnvironmentVariableTarget.Process);
                    if (txt_ip.Text == array2[0])
                    {
                        checkPrivate.Checked = true;
                    }
                    Program.int_WorkingIP = num * 2;
                    File.AppendAllText("IRLOG\\t.log", DateTime.Now.ToString("HH:mm:ss.fff") + " PX-OK " + array2[0] + Environment.NewLine);
                    Program.PrivateProxy = Proxy_;
                    label12.BackColor = Color.LightCyan;
                    label12.ForeColor = Color.Black;
                    Label label2 = label12;
                    string text6 = "IRCTC Server is Working on Your IP Adress";
                    label2.Text = text6;
                    break;
                }
            }
            catch
            {
                if (txt_ip.Text == array2[0])
                {
                    checkPrivate.Checked = false;
                }
                label12.BackColor = Color.Red;
                label12.ForeColor = Color.White;
                Label label3 = label12;
                string text7 = "IRCTC Server is Not Working on Your IP Adress";
                label3.Text = text7;
            }
        }
    }

    private static bool CheckCertificate(object object_18, X509Certificate x509Certificate_0, X509Chain x509Chain_0, SslPolicyErrors sslPolicyErrors_0)
    {
        return true;
    }

    private bool Check_IP(object object_0, ref string str0)
    {
        bool result = false;
        string text = object_0.ToString();
        string[] array = new string[1];
        string text2 = "||";
        array[0] = text2;
        string[] array2 = text.Split(array, StringSplitOptions.None);
        string text3 = "Not Working";
        str0 = text3;
        try
        {
            if (array2.Length > 3)
            {
                Proxy_ = new WebProxy(array2[0].Trim(), int.Parse(array2[1].Trim()));
                Proxy_.BypassProxyOnLocal = true;
                Proxy_.Credentials = new NetworkCredential(array2[2].Trim(), array2[3].Trim());
            }
            string requestUriString = "https://www.irctc.co.in/nget/train-search";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            httpWebRequest.ServerCertificateValidationCallback = CheckCertificate;
            ServicePointManager.Expect100Continue = false;
            httpWebRequest.ProtocolVersion = HttpVersion.Version11;
            httpWebRequest.Proxy = Proxy_;
            httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            HttpRequestCachePolicy cachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromDays(0.0));
            httpWebRequest.CachePolicy = cachePolicy;
            httpWebRequest.KeepAlive = true;
            string connection = "keepalive";
            httpWebRequest.Connection = connection;
            httpWebRequest.Timeout = 10000;
            httpWebRequest.ReadWriteTimeout = 10000;
            string accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, */*";
            httpWebRequest.Accept = accept;
            string contentType = "application/x-www-form-urlencoded";
            httpWebRequest.ContentType = contentType;
            httpWebRequest.AllowAutoRedirect = true;
            string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";
            httpWebRequest.UserAgent = userAgent;
            WebHeaderCollection headers = httpWebRequest.Headers;
            string name = "Accept-Language";
            string value = "en-US,en;q=0.9,hi;q=0.8";
            headers.Add(name, value);
            WebHeaderCollection headers2 = httpWebRequest.Headers;
            string name2 = "Origin";
            string value2 = "https://www.irctc.co.in";
            headers2.Add(name2, value2);
            WebHeaderCollection headers3 = httpWebRequest.Headers;
            string name3 = "bmirak";
            string value3 = "webbm";
            headers3.Add(name3, value3);
            string text4 = new StreamReader(httpWebRequest.GetResponse().GetResponseStream()).ReadToEnd().Trim();
            string value4 = "nget";
            if (text4.Contains(value4))
            {
                string text5 = "Proxy is Working";
                str0 = text5;
                result = true;
            }
        }
        catch (Exception ex)
        {
            string text6 = "Not Working";
            str0 = text6;
            string message = ex.Message;
            string value5 = "Authentication Required";
            if (message.Contains(value5))
            {
                string text7 = "Invalid Proxy Username or Password";
                str0 = text7;
            }
        }
        return result;
    }

    internal void AddSlot()
    {
        if (!string.IsNullOrEmpty(Program.mySettings_0.PrivateIP))
        {
            string text = Program.mySettings_0.PrivateIP.ToString();
            string[] array2 = new string[1];
            string text2 = ":";
            array2[0] = text2;
            string[] array = text.Split(array2, StringSplitOptions.None);
            if (Program.list_Proxy.FirstOrDefault((string stringToCheck) => stringToCheck.Contains(array[0])) == null)
            {
                Program.int_Slot += 2;
                List<string> list_Proxy = Program.list_Proxy;
                string text3 = Program.mySettings_0.PrivateIP.ToString();
                string oldValue = ":";
                string newValue = "||";
                list_Proxy.Add(text3.Replace(oldValue, newValue));
                string variable = "PrivateProxy";
                string text4 = Program.mySettings_0.PrivateIP.ToString();
                string oldValue2 = ":";
                string newValue2 = "||";
                Environment.SetEnvironmentVariable(variable, text4.Replace(oldValue2, newValue2), EnvironmentVariableTarget.Process);
            }
        }
    }

    public void check_0(object j)
    {
        try
        {
            bool flag = false;
            string requestUriString = "https://www.irctc.co.in/nget/train-search";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
            httpWebRequest.Proxy = Proxy_;
            WebHeaderCollection headers = httpWebRequest.Headers;
            string name = "bmirak";
            string value = "webbm";
            headers.Add(name, value);
            httpWebRequest.Timeout = 8000;
            httpWebRequest.ReadWriteTimeout = 8000;
            using (StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream()))
            {
                streamReader.ReadToEnd();
                flag = true;
            }
            if (!flag & !string.IsNullOrEmpty(Program.mySettings_0.PrivateIP))
            {
                Class47.prx0 = new WebProxy();
                string text = Program.mySettings_0.PrivateIP.ToString();
                string[] array = new string[1];
                string text2 = ":";
                array[0] = text2;
                string[] array2 = text.Split(array, StringSplitOptions.None);
                Class47.prx0 = new WebProxy(array2[0].Trim(), int.Parse(array2[1].Trim()));
                Class47.prx0.BypassProxyOnLocal = true;
                Class47.prx0.Credentials = new NetworkCredential(array2[2].Trim(), array2[3].Trim());
            }
        }
        catch
        {
        }
    }

    private void Txt_ip_Leave(object sender, EventArgs e)
    {
        string text = txt_ip.Text;
        string value = ":";
        if (text.Contains(value))
        {
            string text2 = (new string[1] { txt_ip.Text })[0].Trim();
            string[] array = new string[1];
            string text3 = ":";
            array[0] = text3;
            string[] array2 = text2.Split(array, StringSplitOptions.RemoveEmptyEntries);
            txt_ip.Text = array2[0];
            txt_port.Text = array2[1];
            txt_ipUser.Text = array2[2];
            txt_ipPass.Text = array2[3];
        }
    }

    private void BtnClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void FrmProxySetting_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    private void Txt_AcTime_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
        {
            e.Handled = true;
        }
    }

    private void Txt_AcTime_Leave(object sender, EventArgs e)
    {
        TextBox textBox = sender as TextBox;
        int.TryParse(textBox.Text, out var result);
        if (result > 55 && textBox.Text != "")
        {
            string text = "55";
            textBox.Text = text;
        }
    }

    private void Timer1_Tick(object sender, EventArgs e)
    {
    }

    private void button3_Click(object sender, EventArgs e)
    {
        Form owner = new Form
        {
            TopMost = true
        };
        string text = "Do You Want to Delete All Proxy";
        string caption = "Delete Proxy";
        if (MessageBox.Show(owner, text, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
        {
            string path = "IRLOG//IP.Proxy";
            File.Delete(path);
            ipAdd();
            Form owner2 = new Form
            {
                TopMost = true
            };
            string text2 = "All Proxy Deleted Successfully";
            string caption2 = "Delete";
            MessageBox.Show(owner2, text2, caption2, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }

    private void writeFile(string str)
    {
        string text = null;
        string item = list_IP.FirstOrDefault((string stringToCheck) => stringToCheck.Contains(str));
        list_IP.Remove(item);
        Program.list_Proxy.Remove(item);
        Program.int_Slot -= 2;
        foreach (string item2 in list_IP)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = item2;
                continue;
            }
            string text2 = text;
            string text3 = "####";
            text = text2 + text3 + item2;
        }
        string path = "IRLOG//IP.Proxy";
        File.WriteAllText(path, text);
        base.Height = 117;
        ipAdd();
        Program.int_Slot = 2 + Program.list_Proxy.Count * 2;
    }

    private void btn_delete_Click(object sender, EventArgs e)
    {
        Form owner = new Form
        {
            TopMost = true
        };
        string text = "Do You Want to Delete Selected Proxy";
        string caption = "Delete Proxy";
        if (MessageBox.Show(owner, text, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK || comboIP.SelectedIndex <= 0)
        {
            return;
        }
        string txt = comboIP.SelectedItem.ToString();
        list_IP.RemoveAll((string item) => item.Contains(txt));
        string text2 = null;
        foreach (string item in list_IP)
        {
            if (!string.IsNullOrEmpty(text2))
            {
                string text3 = text2;
                string text4 = "####";
                text2 = text3 + text4 + item;
            }
            else
            {
                text2 = item;
            }
        }
        string path = "IRLOG//IP.Proxy";
        File.WriteAllText(path, text2);
        Form owner2 = new Form
        {
            TopMost = true
        };
        string text5 = "Proxy Deleted Successfully";
        string caption2 = "Delete";
        MessageBox.Show(owner2, text5, caption2, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        ipAdd();
    }

    private void btn_check_Click(object sender, EventArgs e)
    {
        if (comboIP.SelectedIndex > 0)
        {
            string str = "";
            string object_ = list_IP[comboIP.SelectedIndex - 1];
            Check_IP(object_, ref str);
            Form owner = new Form
            {
                TopMost = true
            };
            string text = str;
            string caption = "Information";
            MessageBox.Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }

    private void button2_Click_1(object sender, EventArgs e)
    {
        Program.mySettings_0.perIPlimit = text_limit.Text.Trim();
        string variable = "perIPlimit";
        Environment.SetEnvironmentVariable(variable, Program.mySettings_0.perIPlimit);
        Form owner = new Form
        {
            TopMost = true
        };
        string text = "Save Successfully";
        string caption = "Success";
        MessageBox.Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    private void text_limit_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        {
            e.Handled = true;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProxySetting));
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.checkAdmin = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.checkSystem = new System.Windows.Forms.CheckBox();
            this.checkPrivate = new System.Windows.Forms.CheckBox();
            this.txt_ip = new System.Windows.Forms.TextBox();
            this.txt_port = new System.Windows.Forms.TextBox();
            this.txt_ipUser = new System.Windows.Forms.TextBox();
            this.txt_ipPass = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btn_save = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.formName = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_SLTime = new System.Windows.Forms.TextBox();
            this.txt_AcTime = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label12 = new System.Windows.Forms.Label();
            this.comboIP = new System.Windows.Forms.ComboBox();
            this.btn_check = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.text_limit = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 5;
            this.bunifuElipse1.TargetControl = this;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.checkAdmin);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(610, 213);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 46);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IP BY ADMIN";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(131, 21);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(67, 13);
            this.label15.TabIndex = 19;
            this.label15.Text = "Not Working";
            this.label15.Visible = false;
            // 
            // checkAdmin
            // 
            this.checkAdmin.AutoSize = true;
            this.checkAdmin.Location = new System.Drawing.Point(298, 21);
            this.checkAdmin.Name = "checkAdmin";
            this.checkAdmin.Size = new System.Drawing.Size(15, 14);
            this.checkAdmin.TabIndex = 5;
            this.checkAdmin.UseVisualStyleBackColor = true;
            this.checkAdmin.Visible = false;
            this.checkAdmin.CheckedChanged += new System.EventHandler(this.CheckAdmin_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(207, 15);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Check";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "202.103.94.108";
            // 
            // checkSystem
            // 
            this.checkSystem.AutoSize = true;
            this.checkSystem.Location = new System.Drawing.Point(298, 23);
            this.checkSystem.Name = "checkSystem";
            this.checkSystem.Size = new System.Drawing.Size(15, 14);
            this.checkSystem.TabIndex = 6;
            this.checkSystem.UseVisualStyleBackColor = true;
            this.checkSystem.Visible = false;
            this.checkSystem.CheckedChanged += new System.EventHandler(this.CheckSystem_CheckedChanged);
            // 
            // checkPrivate
            // 
            this.checkPrivate.AutoSize = true;
            this.checkPrivate.Location = new System.Drawing.Point(749, 186);
            this.checkPrivate.Name = "checkPrivate";
            this.checkPrivate.Size = new System.Drawing.Size(15, 14);
            this.checkPrivate.TabIndex = 7;
            this.checkPrivate.UseVisualStyleBackColor = true;
            this.checkPrivate.Visible = false;
            this.checkPrivate.CheckedChanged += new System.EventHandler(this.CheckPrivate_CheckedChanged);
            // 
            // txt_ip
            // 
            this.txt_ip.Location = new System.Drawing.Point(8, 41);
            this.txt_ip.Name = "txt_ip";
            this.txt_ip.Size = new System.Drawing.Size(85, 20);
            this.txt_ip.TabIndex = 8;
            this.txt_ip.Leave += new System.EventHandler(this.Txt_ip_Leave);
            // 
            // txt_port
            // 
            this.txt_port.Location = new System.Drawing.Point(100, 41);
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(30, 20);
            this.txt_port.TabIndex = 9;
            // 
            // txt_ipUser
            // 
            this.txt_ipUser.Location = new System.Drawing.Point(136, 41);
            this.txt_ipUser.Name = "txt_ipUser";
            this.txt_ipUser.Size = new System.Drawing.Size(69, 20);
            this.txt_ipUser.TabIndex = 10;
            // 
            // txt_ipPass
            // 
            this.txt_ipPass.Location = new System.Drawing.Point(211, 41);
            this.txt_ipPass.Name = "txt_ipPass";
            this.txt_ipPass.Size = new System.Drawing.Size(69, 20);
            this.txt_ipPass.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(47, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "IP";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(101, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(26, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Port";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(143, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(55, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Username";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(218, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Password";
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(146, 67);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(132, 23);
            this.btn_save.TabIndex = 16;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.Button2_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Checking";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label10.Location = new System.Drawing.Point(6, 445);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Note :- ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.checkSystem);
            this.groupBox2.Location = new System.Drawing.Point(610, 283);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(320, 51);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SYSTEM IP";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(178, 23);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 13);
            this.label13.TabIndex = 18;
            this.label13.Text = "Not Working";
            this.label13.Visible = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btn_save);
            this.groupBox3.Controls.Add(this.txt_ip);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txt_port);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txt_ipUser);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txt_ipPass);
            this.groupBox3.Location = new System.Drawing.Point(0, 22);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(289, 96);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "PRIVATE IP";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(843, 283);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(67, 13);
            this.label14.TabIndex = 19;
            this.label14.Text = "Not Working";
            this.label14.Visible = false;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(22, 14, 28);
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Controls.Add(this.formName);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(2);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(295, 22);
            this.pnlHeader.TabIndex = 129;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageActive = null;
            this.btnClose.Location = new System.Drawing.Point(271, -2);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(23, 23);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnClose.TabIndex = 24;
            this.btnClose.TabStop = false;
            this.btnClose.Zoom = 10;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click_1);
            // 
            // formName
            // 
            this.formName.AutoSize = true;
            this.formName.BackColor = System.Drawing.Color.Transparent;
            this.formName.Enabled = false;
            this.formName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.formName.ForeColor = System.Drawing.Color.Black;
            this.formName.Location = new System.Drawing.Point(2, 2);
            this.formName.Name = "formName";
            this.formName.Size = new System.Drawing.Size(116, 16);
            this.formName.TabIndex = 23;
            this.formName.Text = "Proxy IP Setting";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.txt_SLTime);
            this.groupBox4.Controls.Add(this.txt_AcTime);
            this.groupBox4.Location = new System.Drawing.Point(503, 31);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(320, 80);
            this.groupBox4.TabIndex = 130;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Availablity TIME";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(188, 48);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 16);
            this.label11.TabIndex = 17;
            this.label11.Text = "10  :  59  :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(188, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 16);
            this.label4.TabIndex = 16;
            this.label4.Text = "09  :  59  :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "SL Availablity Start AT :-";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "AC Availablity Start AT :-";
            // 
            // txt_SLTime
            // 
            this.txt_SLTime.Location = new System.Drawing.Point(265, 46);
            this.txt_SLTime.MaxLength = 2;
            this.txt_SLTime.Name = "txt_SLTime";
            this.txt_SLTime.Size = new System.Drawing.Size(28, 20);
            this.txt_SLTime.TabIndex = 1;
            this.txt_SLTime.Text = "39";
            this.txt_SLTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Txt_AcTime_KeyPress);
            this.txt_SLTime.Leave += new System.EventHandler(this.Txt_AcTime_Leave);
            // 
            // txt_AcTime
            // 
            this.txt_AcTime.Location = new System.Drawing.Point(265, 14);
            this.txt_AcTime.MaxLength = 2;
            this.txt_AcTime.Name = "txt_AcTime";
            this.txt_AcTime.Size = new System.Drawing.Size(28, 20);
            this.txt_AcTime.TabIndex = 0;
            this.txt_AcTime.Text = "47";
            this.txt_AcTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Txt_AcTime_KeyPress);
            this.txt_AcTime.Leave += new System.EventHandler(this.Txt_AcTime_Leave);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(607, 317);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(333, 20);
            this.label12.TabIndex = 131;
            this.label12.Text = "Checking";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // comboIP
            // 
            this.comboIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboIP.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboIP.FormattingEnabled = true;
            this.comboIP.Location = new System.Drawing.Point(4, 25);
            this.comboIP.Margin = new System.Windows.Forms.Padding(2);
            this.comboIP.Name = "comboIP";
            this.comboIP.Size = new System.Drawing.Size(126, 21);
            this.comboIP.TabIndex = 139;
            // 
            // btn_check
            // 
            this.btn_check.Location = new System.Drawing.Point(136, 23);
            this.btn_check.Name = "btn_check";
            this.btn_check.Size = new System.Drawing.Size(59, 23);
            this.btn_check.TabIndex = 140;
            this.btn_check.Text = "Check";
            this.btn_check.UseVisualStyleBackColor = true;
            this.btn_check.Click += new System.EventHandler(this.btn_check_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.Location = new System.Drawing.Point(200, 22);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(79, 23);
            this.btn_delete.TabIndex = 141;
            this.btn_delete.Text = "Delete";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.comboIP);
            this.groupBox5.Controls.Add(this.btn_delete);
            this.groupBox5.Controls.Add(this.btn_check);
            this.groupBox5.Location = new System.Drawing.Point(0, 123);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox5.Size = new System.Drawing.Size(289, 54);
            this.groupBox5.TabIndex = 139;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Proxy IP";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.button2);
            this.groupBox6.Controls.Add(this.text_limit);
            this.groupBox6.Controls.Add(this.label16);
            this.groupBox6.Location = new System.Drawing.Point(0, 219);
            this.groupBox6.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox6.Size = new System.Drawing.Size(289, 57);
            this.groupBox6.TabIndex = 140;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Se Proxy Limit";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(211, 15);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(67, 25);
            this.button2.TabIndex = 17;
            this.button2.Text = "SAVE";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // text_limit
            // 
            this.text_limit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.text_limit.Location = new System.Drawing.Point(162, 18);
            this.text_limit.Name = "text_limit";
            this.text_limit.Size = new System.Drawing.Size(45, 22);
            this.text_limit.TabIndex = 16;
            this.text_limit.Text = "1";
            this.text_limit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.text_limit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.text_limit_KeyPress);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(5, 20);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(151, 17);
            this.label16.TabIndex = 15;
            this.label16.Text = "Per Proxy Pair Limit";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel1.Location = new System.Drawing.Point(-4, 209);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(318, 6);
            this.panel1.TabIndex = 141;
            // 
            // frmProxySetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 282);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.checkPrivate);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmProxySetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmProxySetting";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmProxySetting_FormClosing);
            this.Load += new System.EventHandler(this.FrmProxySetting_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    private void btnClose_Click_1(object sender, EventArgs e)
    {
        base.Close();
    }
}
