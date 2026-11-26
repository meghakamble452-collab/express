using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Bunifu.Framework.UI;
using NEXUS.Properties;

internal class frmPayment : Form
{
    private clsPostData class42_0;

    private const uint uint_0 = 1u;

    private const uint uint_1 = 2u;

    internal const uint uint_2 = 3u;

    internal static readonly IntPtr intptr_0 = new IntPtr(-1);

    private Dictionary<string, string> dictionary_0 = new Dictionary<string, string>();

    internal string string_0 = "";

    internal string string_1 = "";

    internal string string_2 = "";

    private string string_3;

    internal bool bool_0;

    internal bool bool_2;

    internal bool bool_3;

    internal bool bool_4;

    internal bool bool_7;

    internal bool bool_Passenger;

    internal bool bool_Alert;

    private string tktname_0;

    private IContainer icontainer_0;

    internal Label lblError;

    internal Label tktname;

    private BunifuImageButton btnCancel;

    private Panel pnlHeader;

    public Button btnSubmit;

    public Button btnNxt;

    private BunifuElipse bunifuElipse_0;

    private IContainer components;

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(IntPtr intptr_1, IntPtr intptr_2, int int_0, int int_1, int int_2, int int_3, uint uint_3);

    public frmPayment()
    {
        InitializeComponent();
        string text = "frmPayment-";
        base.Name = text + Thread.CurrentThread.Name;
        btnNxt.Visible = false;
        ApplyByteNovaTheme();
    }


    // =====================================================================
    // Express theme (light + bhagwa) - visual layer only.
    // Payment / dialog / loop logic below is untouched.
    // =====================================================================
    private static readonly Color XP_BG = Color.FromArgb(236, 242, 251);
    private static readonly Color XP_ORANGE = Color.FromArgb(245, 146, 28);
    private static readonly Color XP_HEAD_L = Color.FromArgb(214, 96, 4);
    private static readonly Color XP_HEAD_R = Color.FromArgb(248, 158, 44);
    private static readonly Color XP_NAVY = Color.FromArgb(15, 42, 92);
    private static readonly Color XP_TEXT = Color.FromArgb(30, 41, 59);
    private static readonly Color XP_MUTED = Color.FromArgb(100, 116, 139);
    private static readonly Color XP_FRAME = Color.FromArgb(178, 196, 226);
    private static readonly Color XP_CARD_LINE = Color.FromArgb(220, 228, 241);

    // Base layout in 96-dpi pixels; scaled by xpK so nothing is cut at 125-150 % scaling.
    private const int XP_W = 400, XP_HEAD = 42, XP_M = 12, XP_BTN_H = 38, XP_CARD_MIN = 60;
    private float xpK = 1f;
    private Font xpFTitle, xpFMsg, xpFBtn, xpFHint;
    private Rectangle xpCard;
    private bool xpCloseHot, xpCloseDown;
    private int xpBtnState;            // 0 normal, 1 hover, 2 pressed
    private bool xpLaying;

    private int XpS(float v) { return (int)Math.Round(v * xpK); }

    private void ApplyByteNovaTheme()
    {
        SuspendLayout();

        try
        {
            using (Graphics sg = Graphics.FromHwnd(IntPtr.Zero))
                xpK = Math.Max(1f, sg.DpiX / 96f);
        }
        catch { xpK = 1f; }

        // pixel-unit fonts: their size follows xpK exactly like the layout does
        xpFTitle = new Font("Segoe UI Semibold", 14f * xpK, FontStyle.Regular, GraphicsUnit.Pixel);
        xpFMsg = new Font("Segoe UI Semibold", 13.5f * xpK, FontStyle.Regular, GraphicsUnit.Pixel);
        xpFBtn = new Font("Segoe UI Semibold", 14f * xpK, FontStyle.Regular, GraphicsUnit.Pixel);
        xpFHint = new Font("Segoe UI", 12.5f * xpK, FontStyle.Italic, GraphicsUnit.Pixel);

        BackColor = XP_BG;
        ForeColor = XP_TEXT;
        FormBorderStyle = FormBorderStyle.None;
        DoubleBuffered = true;
        bunifuElipse_0.ElipseRadius = XpS(10);
        XpBuffer(pnlHeader);

        // ---- header ribbon ----
        pnlHeader.Dock = DockStyle.None;
        pnlHeader.BackColor = XP_HEAD_L;
        pnlHeader.Paint -= ByteNovaHeaderPaint;
        pnlHeader.Paint += ByteNovaHeaderPaint;

        tktname.AutoSize = false;
        tktname.AutoEllipsis = true;
        tktname.ForeColor = Color.White;
        tktname.BackColor = Color.Transparent;
        tktname.Font = xpFTitle;
        tktname.TextAlign = ContentAlignment.MiddleLeft;
        tktname.UseCompatibleTextRendering = false;

        btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        btnCancel.BackColor = Color.Transparent;
        btnCancel.Image = null;
        btnCancel.ImageActive = null;
        btnCancel.Zoom = 0;
        btnCancel.Cursor = Cursors.Hand;
        btnCancel.Paint -= ByteNovaClosePaint;
        btnCancel.Paint += ByteNovaClosePaint;
        btnCancel.MouseEnter += delegate { xpCloseHot = true; btnCancel.Invalidate(); };
        btnCancel.MouseLeave += delegate { xpCloseHot = false; xpCloseDown = false; btnCancel.Invalidate(); };
        btnCancel.MouseDown += delegate { xpCloseDown = true; btnCancel.Invalidate(); };
        btnCancel.MouseUp += delegate { xpCloseDown = false; if (!btnCancel.IsDisposed) btnCancel.Invalidate(); };
        // close shown as the corner sticker; it calls the same btnCancel_Click (nothing reads btnCancel.Visible)
        btnCancel.Visible = false;
        StInitSticker(delegate { btnCancel_Click(btnCancel, EventArgs.Empty); });

        // ---- message ----
        lblError.AutoSize = false;
        lblError.BackColor = Color.Transparent;
        lblError.ForeColor = XP_TEXT;
        lblError.Font = xpFMsg;
        lblError.TextAlign = ContentAlignment.MiddleCenter;
        lblError.UseCompatibleTextRendering = false;
        lblError.VisibleChanged -= ByteNovaLblErrorVisibleChanged;
        lblError.VisibleChanged += ByteNovaLblErrorVisibleChanged;
        lblError.TextChanged -= XpMessageChanged;
        lblError.TextChanged += XpMessageChanged;
        if (lblError.Visible) lblError.BringToFront();

        // ---- primary button (text comes from the logic: Process Payment / Submit Passenger / Close) ----
        StyleByteNovaButton(btnSubmit, XP_BG);

        // Keep the existing hidden Next control and its original functionality.
        btnNxt.FlatStyle = FlatStyle.Flat;
        btnNxt.FlatAppearance.BorderSize = 1;
        btnNxt.FlatAppearance.BorderColor = XP_ORANGE;
        btnNxt.BackColor = XP_HEAD_L;
        btnNxt.ForeColor = Color.White;

        Paint -= ByteNovaFormPaint;
        Paint += ByteNovaFormPaint;

        XpLayout();
        ResumeLayout(false);
        Invalidate(true);
    }

    // frmPayment_Load (alert mode) resets a few bounds; lay the themed design out again after it.
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (!IsDisposed) XpLayout();
    }

    private static void XpBuffer(Control c)
    {
        try
        {
            typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(c, true, null);
        }
        catch { }
    }

    private void XpMessageChanged(object sender, EventArgs e)
    {
        XpLayout();
        Invalidate();
    }

    // Fit exactly on the ticket window this popup opens over (same size + position).
    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        if (!IsDisposed) XpMatchHost();
    }

    private Size xpHostSize = Size.Empty;

    private void XpMatchHost()
    {
        try
        {
            Point me = Location;
            Point mid = new Point(Left + Width / 2, Top + Height / 2);
            Form host = null;

            Form own = Owner;
            if (own != null && own != this && own.Visible &&
                (own.Bounds.Contains(me) || own.Bounds.Contains(mid)))
                host = own;

            if (host == null)
            {
                long best = long.MaxValue;
                bool bestIsTicket = false;
                List<Form> forms = new List<Form>();
                foreach (Form of in Application.OpenForms) forms.Add(of);
                foreach (Form f in forms)
                {
                    if (f == null || f == this || f.IsDisposed || !f.Visible || f is frmPayment) continue;
                    Rectangle b = f.Bounds;
                    if (b.Width < 50 || b.Height < 50) continue;
                    if (!b.Contains(me) && !b.Contains(mid)) continue;
                    bool isTicket = f.GetType().Name == "frmProcessTicket";
                    long area = (long)b.Width * b.Height;
                    if ((isTicket && !bestIsTicket) || (isTicket == bestIsTicket && area < best))
                    {
                        host = f;
                        best = area;
                        bestIsTicket = isTicket;
                    }
                }
            }

            if (host == null) return;          // nothing found: keep the default size/position
            Rectangle hb = host.Bounds;
            xpHostSize = hb.Size;
            Bounds = hb;
            XpLayout();
            Invalidate(true);
        }
        catch { }
    }

    private void XpLayout()
    {
        if (xpLaying || xpFMsg == null) return;
        xpLaying = true;
        try
        {
            int m = XpS(XP_M), head = XpS(XP_HEAD), btnH = XpS(XP_BTN_H);
            string msg = lblError.Text ?? "";
            bool hasMsg = msg.Trim().Length > 0;
            const TextFormatFlags wrap = TextFormatFlags.WordBreak | TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPadding;
            int w, h;

            if (!xpHostSize.IsEmpty)
            {
                // sized to the ticket window behind: card fills the middle, button at the bottom
                w = xpHostSize.Width;
                h = xpHostSize.Height;
                ClientSize = new Size(w, h);
                int btnTop = h - m - btnH;
                xpCard = new Rectangle(m, head + m, w - 2 * m, Math.Max(XpS(40), btnTop - XpS(10) - (head + m)));

                // shrink the message font only if the text would not fit the card
                int tw = xpCard.Width - XpS(64), th = xpCard.Height - XpS(12);
                float px = 15f;
                Font f = null;
                while (true)
                {
                    if (f != null && f != xpFMsg) f.Dispose();
                    f = new Font("Segoe UI Semibold", px * xpK, FontStyle.Regular, GraphicsUnit.Pixel);
                    if (!hasMsg || px <= 10.5f ||
                        TextRenderer.MeasureText(msg, f, new Size(tw, int.MaxValue), wrap).Height <= th)
                        break;
                    px -= 0.5f;
                }
                Font oldMsg = xpFMsg;
                xpFMsg = f;
                lblError.Font = f;
                if (oldMsg != null && oldMsg != f) oldMsg.Dispose();
            }
            else
            {
                w = XpS(XP_W);
                // message card grows with the text so nothing is ever cut
                int textW = w - 2 * m - XpS(64);
                int textH = 0;
                if (hasMsg)
                    textH = TextRenderer.MeasureText(msg, xpFMsg, new Size(textW, int.MaxValue), wrap).Height;
                int cardH = Math.Max(XpS(XP_CARD_MIN), textH + XpS(24));
                xpCard = new Rectangle(m, head + m, w - 2 * m, cardH);
                h = xpCard.Bottom + XpS(10) + btnH + m;
                ClientSize = new Size(w, h);
            }
            int btnY = h - m - btnH;

            pnlHeader.SetBounds(0, 0, w, head);
            // close = round red badge in the top-right corner: 30px disc, 3px from the right edge,
            // vertically centred in the band above the 3px underline (header band 39 >= 36)
            int bb = XpS(36), ch = XpS(26);
            btnCancel.SetBounds(w - bb, Math.Max(0, (head - XpS(3) - bb) / 2), bb, bb);
            tktname.SetBounds(XpS(46), 0, btnCancel.Left - XpS(52), head - XpS(3));
            btnNxt.SetBounds(btnCancel.Left - XpS(42), (head - XpS(3) - ch) / 2, XpS(36), ch);
            btnCancel.BringToFront();

            lblError.SetBounds(xpCard.X + XpS(52), xpCard.Y + XpS(6), xpCard.Width - XpS(60), xpCard.Height - XpS(12));
            if (lblError.Visible) lblError.BringToFront();

            btnSubmit.SetBounds(m, btnY, w - 2 * m, btnH);
        }
        finally { xpLaying = false; }
    }

    private void ByteNovaLblErrorVisibleChanged(object sender, EventArgs e)
    {
        if (lblError != null && lblError.Visible)
        {
            lblError.BringToFront();
            lblError.Refresh();
        }
        Invalidate();
    }

    private void StyleByteNovaButton(Button b, Color back)
    {
        if (b == null) return;
        b.BackColor = back;
        b.ForeColor = Color.White;
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.FlatAppearance.MouseOverBackColor = back;
        b.FlatAppearance.MouseDownBackColor = back;
        b.Font = xpFBtn ?? b.Font;
        b.UseVisualStyleBackColor = false;
        b.Cursor = Cursors.Hand;
        b.Paint -= XpButtonPaint;
        b.Paint += XpButtonPaint;
        b.MouseEnter += delegate { xpBtnState = 1; b.Invalidate(); };
        b.MouseLeave += delegate { xpBtnState = 0; b.Invalidate(); };
        b.MouseDown += delegate (object s, MouseEventArgs e) { if (e.Button == MouseButtons.Left) { xpBtnState = 2; b.Invalidate(); } };
        b.MouseUp += delegate
        {
            if (b.IsDisposed) return;
            xpBtnState = b.ClientRectangle.Contains(b.PointToClient(MousePosition)) ? 1 : 0;
            b.Invalidate();
        };
        b.TextChanged += delegate { b.Invalidate(); };
    }

    // ---------------- painting ----------------

    private static GraphicsPath XpRound(RectangleF r, float rad)
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

    private static void XpFill(Graphics g, RectangleF r, float rad, Color fill, Color border)
    {
        if (r.Width < 1 || r.Height < 1) return;
        using (GraphicsPath p = XpRound(r, rad))
        {
            using (SolidBrush b = new SolidBrush(fill)) g.FillPath(b, p);
            if (border.A > 0) using (Pen pen = new Pen(border, 1F)) g.DrawPath(pen, p);
        }
    }

    private static Color XpMix(Color a, Color b, float t)
    {
        return Color.FromArgb(a.A,
            (int)(a.R + (b.R - a.R) * t),
            (int)(a.G + (b.G - a.G) * t),
            (int)(a.B + (b.B - a.B) * t));
    }

    private bool XpIsWarning()
    {
        string t = (lblError.Text ?? "").ToUpperInvariant();
        return t.Contains("HIGHER") || t.Contains("ALREADY");
    }

    private void ByteNovaHeaderPaint(object sender, PaintEventArgs e)
    {
        Rectangle r = pnlHeader.ClientRectangle;
        if (r.Width <= 0 || r.Height <= 0) return;
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using (LinearGradientBrush br = new LinearGradientBrush(r, XP_HEAD_L, XP_HEAD_R, LinearGradientMode.Horizontal))
            g.FillRectangle(br, r);
        using (SolidBrush shine = new SolidBrush(Color.FromArgb(20, 255, 255, 255)))
            g.FillRectangle(shine, 0, 0, r.Width, (r.Height - XpS(3)) / 2);
        using (SolidBrush navy = new SolidBrush(XP_NAVY))
            g.FillRectangle(navy, 0, r.Height - XpS(3), r.Width, XpS(3));

        // white badge with a Lucide credit-card icon
        int hb = r.Height - XpS(3);
        float bd = XpS(28);
        RectangleF badge = new RectangleF(XpS(10), (hb - bd) / 2f, bd, bd);
        using (SolidBrush sh = new SolidBrush(Color.FromArgb(45, 0, 0, 0)))
            g.FillEllipse(sh, badge.X, badge.Y + 1, badge.Width, badge.Height);
        using (SolidBrush wb = new SolidBrush(Color.White))
            g.FillEllipse(wb, badge);
        XpIcon(g, "card", RectangleF.Inflate(badge, -XpS(7), -XpS(7)), XP_HEAD_L);
    }

    // ===== Close "sticker" (same as the ticket popup): a round red X badge whose centre sits on the
    // window's top-right corner, about half outside it. WinForms can't paint outside a form, so it is a
    // tiny owned layered window (WS_EX_LAYERED | WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE) with per-pixel alpha.
    // Its click runs exactly the existing close path; the in-header close control is only hidden.
    private StickerWindow stSticker;

    private void StInitSticker(MethodInvoker click)
    {
        if (stSticker != null) return;
        stSticker = new StickerWindow(this, click);
        EventHandler sync = delegate { StSyncSticker(); };
        Move += sync;
        Resize += sync;
        LocationChanged += sync;
        SizeChanged += sync;
        VisibleChanged += sync;
        Activated += sync;
        Shown += sync;
        HandleDestroyed += delegate { if (stSticker != null) stSticker.DropHandle(); };   // recreated on next sync
        FormClosed += delegate { StKillSticker(); };
        Disposed += delegate { StKillSticker(); };
    }

    private void StSyncSticker()
    {
        if (stSticker == null || IsDisposed || Disposing || !IsHandleCreated) return;
        stSticker.Sync(Visible && WindowState != FormWindowState.Minimized);
    }

    private void StKillSticker()
    {
        if (stSticker == null) return;
        stSticker.Dispose();
        stSticker = null;
    }

    private sealed class StickerWindow : NativeWindow, IDisposable
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

        public StickerWindow(Form owner, MethodInvoker click)
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
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
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
                x.StartCap = LineCap.Round;
                x.EndCap = LineCap.Round;
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
                        owner.BeginInvoke(click);   // exactly the existing btnClose click path
                    m.Result = IntPtr.Zero;
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }

    private void ByteNovaClosePaint(object sender, PaintEventArgs e)
    {
        Control c = btnCancel;
        Rectangle r0 = c.ClientRectangle;
        if (r0.Width <= 0 || r0.Height <= 0) return;
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // repaint the ribbon underneath so the old image never shows through
        Control p = c.Parent;
        int pw = p != null ? Math.Max(1, p.Width) : c.Width;
        int ph = p != null ? Math.Max(1, p.Height) : c.Height;
        using (LinearGradientBrush bg = new LinearGradientBrush(new Rectangle(-c.Left, -c.Top, pw, ph), XP_HEAD_L, XP_HEAD_R, LinearGradientMode.Horizontal))
            g.FillRectangle(bg, r0);
        using (SolidBrush shine = new SolidBrush(Color.FromArgb(20, 255, 255, 255)))
            g.FillRectangle(shine, 0, 0, c.Width, Math.Max(0, (ph - XpS(3)) / 2 - c.Top));
        int uy = ph - XpS(3) - c.Top;
        if (uy < c.Height)
            using (SolidBrush nb = new SolidBrush(XP_NAVY))
                g.FillRectangle(nb, 0, uy, c.Width, XpS(3));

        // Round red badge patched on the corner: shadow, white ring, red disc, bold white X.
        // Hover: grows 2px, white disc + red X + red ring. Pressed: dark red, 1px down.
        bool hot = xpCloseHot && !xpCloseDown, down = xpCloseDown;
        float k = xpK;
        float d = (30f + (hot ? 2f : 0f)) * k;
        float cx = Math.Min(c.Width, c.Height) / 2f, cy = cx + (down ? k : 0f);
        RectangleF disc = new RectangleF(cx - d / 2f, cy - d / 2f, d, d);
        if (!down)
        {
            using (SolidBrush s1 = new SolidBrush(Color.FromArgb(34, 0, 0, 0)))
                g.FillEllipse(s1, disc.X - 0.5f * k, disc.Y + 2.4f * k, disc.Width + k, disc.Height + 0.4f * k);
            using (SolidBrush s2 = new SolidBrush(Color.FromArgb(46, 0, 0, 0)))
                g.FillEllipse(s2, disc.X + 0.5f * k, disc.Y + 1.4f * k, disc.Width - k, disc.Height - 0.4f * k);
        }
        Color red = Color.FromArgb(220, 38, 38);
        Color ring = hot ? red : Color.White;
        Color fill = down ? Color.FromArgb(153, 27, 27) : hot ? Color.White : red;
        Color glyph = hot ? red : Color.White;
        using (SolidBrush rb = new SolidBrush(ring)) g.FillEllipse(rb, disc);
        RectangleF inner = RectangleF.Inflate(disc, -2f * k, -2f * k);
        using (SolidBrush fb = new SolidBrush(fill)) g.FillEllipse(fb, inner);
        if (!hot && !down)
            using (SolidBrush gl = new SolidBrush(Color.FromArgb(38, 255, 255, 255)))
                g.FillEllipse(gl, inner.X + inner.Width * 0.18f, inner.Y + 1.5f * k, inner.Width * 0.64f, inner.Height * 0.42f);
        float a = d * 0.17f;
        using (Pen x = new Pen(glyph, 2.6f * k))
        {
            x.StartCap = LineCap.Round;
            x.EndCap = LineCap.Round;
            g.DrawLine(x, cx - a, cy - a, cx + a, cy + a);
            g.DrawLine(x, cx + a, cy - a, cx - a, cy + a);
        }
    }

    private void ByteNovaFormPaint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(XP_BG);

        bool warn = XpIsWarning();
        Color accent = warn ? Color.FromArgb(217, 119, 6) : XP_ORANGE;
        Color cardFill = warn ? Color.FromArgb(255, 251, 235) : Color.White;
        Color cardLine = warn ? Color.FromArgb(253, 211, 77) : XP_CARD_LINE;

        RectangleF card = new RectangleF(xpCard.X, xpCard.Y, xpCard.Width - 1, xpCard.Height - 1);
        XpFill(g, new RectangleF(card.X, card.Y + 1.5f, card.Width, card.Height), XpS(10), Color.FromArgb(215, 226, 243), Color.Empty);
        XpFill(g, card, XpS(10), cardFill, cardLine);
        XpFill(g, new RectangleF(card.X + 1, card.Y + XpS(10), XpS(3), card.Height - XpS(20)), 1.5f * xpK, accent, Color.Empty);

        // icon tile: alert-triangle for warnings, info otherwise
        float tile = XpS(32);
        RectangleF it = new RectangleF(card.X + XpS(12), card.Y + (card.Height - tile) / 2f, tile, tile);
        XpFill(g, it, XpS(8), Color.FromArgb(warn ? 60 : 36, accent), Color.Empty);
        XpIcon(g, warn ? "alert" : "info", RectangleF.Inflate(it, -XpS(7), -XpS(7)), accent);

        // empty message: a calm painted hint (the label text itself is untouched)
        if (!lblError.Visible || (lblError.Text ?? "").Trim().Length == 0)
        {
            Rectangle hr = new Rectangle(xpCard.X + XpS(52), xpCard.Y, xpCard.Width - XpS(60), xpCard.Height);
            TextRenderer.DrawText(g, "Ready to process payment", xpFHint, hr, XP_MUTED,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);
        }

        using (Pen outer = new Pen(XP_FRAME, 1.2F))
            g.DrawRectangle(outer, 0.6f, 0.6f, ClientSize.Width - 1.2f, ClientSize.Height - 1.2f);
    }

    private void XpButtonPaint(object sender, PaintEventArgs e)
    {
        Button b = sender as Button;
        if (b == null) return;
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(XP_BG);

        int st = xpBtnState;
        bool on = b.Enabled;
        RectangleF r = new RectangleF(0.5f, st == 2 ? 1.5f : 0.5f, b.Width - 2, b.Height - 3);
        Color top = Color.FromArgb(252, 170, 66), bot = Color.FromArgb(232, 112, 4);
        if (st == 1) { top = XpMix(top, Color.White, 0.14f); bot = XpMix(bot, Color.White, 0.08f); }
        if (st == 2) { top = XpMix(top, Color.Black, 0.12f); bot = XpMix(bot, Color.Black, 0.16f); }
        if (!on) { top = XpMix(top, Color.White, 0.55f); bot = XpMix(bot, Color.White, 0.55f); }
        if (st != 2 && on)
            XpFill(g, new RectangleF(r.X, r.Y + 2, r.Width, r.Height), XpS(9), Color.FromArgb(st == 1 ? 90 : 60, 194, 84, 0), Color.Empty);
        using (GraphicsPath path = XpRound(r, XpS(9)))
        using (LinearGradientBrush bg = new LinearGradientBrush(r, top, bot, LinearGradientMode.Vertical))
        {
            g.FillPath(bg, path);
            Region old = g.Clip;
            g.SetClip(path, CombineMode.Intersect);
            using (SolidBrush gloss = new SolidBrush(Color.FromArgb(st == 2 ? 16 : 38, 255, 255, 255)))
                g.FillRectangle(gloss, r.X, r.Y, r.Width, r.Height / 2);
            if (st == 1)
                using (SolidBrush nb = new SolidBrush(XP_NAVY))
                    g.FillRectangle(nb, r.X, r.Bottom - XpS(3), r.Width, XpS(3));
            g.Clip = old;
        }

        string text = b.Text ?? "";
        string up = text.ToUpperInvariant();
        string icon = up.Contains("CLOSE") ? "x" : up.Contains("PASSENGER") ? "user" : "card";
        Size ts = TextRenderer.MeasureText(g, text, xpFBtn, Size.Empty, TextFormatFlags.NoPadding);
        float isz = XpS(18), gap = XpS(9);
        float gw = isz + gap + ts.Width + XpS(14);   // + room for the arrow
        float x = r.X + (r.Width - gw) / 2f + (st == 1 ? XpS(2) : 0);
        float cy = r.Y + r.Height / 2f;
        XpIcon(g, icon, new RectangleF(x, cy - isz / 2f, isz, isz), Color.White);
        TextRenderer.DrawText(g, text, xpFBtn,
            new Rectangle((int)(x + isz + gap), (int)(cy - ts.Height / 2f) - 1, ts.Width + 4, ts.Height + 2),
            Color.White, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        if (icon == "card")
            XpIcon(g, "arrow", new RectangleF(x + isz + gap + ts.Width + XpS(6), cy - XpS(5), XpS(10), XpS(10)), Color.White);
    }

    // Lucide-style line icons (2px stroke, round caps/joins)
    private void XpIcon(Graphics g, string kind, RectangleF r, Color c)
    {
        SmoothingMode old = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        float x = r.X, y = r.Y, w = r.Width, h = r.Height;
        using (Pen p = new Pen(c, Math.Max(1.5f, 1.9f * xpK)))
        using (SolidBrush b = new SolidBrush(c))
        {
            p.StartCap = LineCap.Round; p.EndCap = LineCap.Round; p.LineJoin = LineJoin.Round;
            switch (kind)
            {
                case "card":
                    using (GraphicsPath cp = XpRound(new RectangleF(x, y + h * 0.18f, w, h * 0.66f), w * 0.12f))
                        g.DrawPath(p, cp);
                    g.DrawLine(p, x, y + h * 0.4f, x + w, y + h * 0.4f);
                    g.DrawLine(p, x + w * 0.16f, y + h * 0.66f, x + w * 0.36f, y + h * 0.66f);
                    break;
                case "arrow":
                    g.DrawLine(p, x, y + h / 2, x + w, y + h / 2);
                    g.DrawLines(p, new[] { new PointF(x + w * 0.55f, y), new PointF(x + w, y + h / 2), new PointF(x + w * 0.55f, y + h) });
                    break;
                case "info":
                    g.DrawEllipse(p, x, y, w, h);
                    g.DrawLine(p, x + w / 2, y + h * 0.46f, x + w / 2, y + h * 0.72f);
                    g.FillEllipse(b, x + w / 2 - 1.3f * xpK, y + h * 0.26f - 1.3f * xpK, 2.6f * xpK, 2.6f * xpK);
                    break;
                case "alert":
                    using (GraphicsPath tp = new GraphicsPath())
                    {
                        tp.AddLines(new[] { new PointF(x + w / 2, y + h * 0.04f), new PointF(x + w, y + h * 0.92f), new PointF(x, y + h * 0.92f) });
                        tp.CloseFigure();
                        g.DrawPath(p, tp);
                    }
                    g.DrawLine(p, x + w / 2, y + h * 0.38f, x + w / 2, y + h * 0.6f);
                    g.FillEllipse(b, x + w / 2 - 1.3f * xpK, y + h * 0.74f - 1.3f * xpK, 2.6f * xpK, 2.6f * xpK);
                    break;
                case "user":
                    g.DrawEllipse(p, x + w * 0.3f, y, w * 0.4f, h * 0.42f);
                    g.DrawArc(p, x + w * 0.08f, y + h * 0.52f, w * 0.84f, h * 0.9f, 180, 180);
                    break;
                case "x":
                    g.DrawEllipse(p, x, y, w, h);
                    g.DrawLine(p, x + w * 0.34f, y + h * 0.34f, x + w * 0.66f, y + h * 0.66f);
                    g.DrawLine(p, x + w * 0.66f, y + h * 0.34f, x + w * 0.34f, y + h * 0.66f);
                    break;
            }
        }
        g.SmoothingMode = old;
    }


    internal void method_0(Control control_0, Rectangle rectangle_0)
    {
        _ = control_0.Controls[0];
        Control control = control_0.Parent;
        base.StartPosition = FormStartPosition.Manual;
        base.Location = control.Location;
        base.TopMost = true;
        base.Left += 2;
    }

    internal void method_1_0(Control control_0, Rectangle rectangle_0, clsPostData class42_1, string string_5, string Tkt_Name_)
    {
        tktname_0 = Tkt_Name_;
        class42_0 = class42_1;
        string_3 = string_5;
        method_0(control_0, rectangle_0);
        lblError.Text = string_1;
        lblError.Visible = true;
        lblError.BringToFront();
        lblError.Refresh();
        try
        {
            SetWindowPos(base.Handle, intptr_0, 0, 0, 0, 0, 3u);
            ThreadPool.QueueUserWorkItem(Loop);
            ShowDialog();
        }
        catch (ThreadAbortException)
        {
            Thread.ResetAbort();
        }
        catch (Exception)
        {
        }
    }

    private void Loop(object j)
    {
        try
        {
            while (true)
            {
                if (clsMain.list_BookingFail.Contains(tktname_0))
                {
                    clsMain.list_BookingFail.Remove(tktname_0);
                    base.DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    Application.DoEvents();
                    Thread.Sleep(333);
                }
            }
        }
        catch
        {
        }
    }

    internal void method_1(Control control_0, Rectangle rectangle_0, clsPostData class42_1, string string_5)
    {
        class42_0 = class42_1;
        string_3 = string_5;
        method_0(control_0, rectangle_0);
        lblError.Text = string_1;
        lblError.Visible = true;
        lblError.BringToFront();
        lblError.Refresh();
        try
        {
            SetWindowPos(base.Handle, intptr_0, 0, 0, 0, 0, 3u);
            ShowDialog();
        }
        catch (ThreadAbortException)
        {
            Thread.ResetAbort();
        }
        catch (Exception)
        {
        }
    }

    private void frmPayment_Load(object sender, EventArgs e)
    {
        if (bool_Passenger)
        {
            Button button = btnSubmit;
            string text = "Submit Passenger";
            button.Text = text;
        }
        if (bool_Alert)
        {
            base.Size = new Size(330, 126);
            btnSubmit.SetBounds(9, 87, 312, 31);
            lblError.SetBounds(9, 34, 312, 48);
            Button button2 = btnSubmit;
            string text2 = "Close";
            button2.Text = text2;
        }
    }

    private void btnNxt_Click(object sender, EventArgs e)
    {
        base.DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        base.DialogResult = DialogResult.Cancel;
        Close();
    }

    private void lblError_Click(object sender, EventArgs e)
    {
        string text = lblError.Text;
        string caption = "Info";
        MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    protected new virtual void Dispose(bool disposing)
    {
        if (disposing && icontainer_0 != null)
        {
            icontainer_0.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPayment));
        this.lblError = new System.Windows.Forms.Label();
        this.tktname = new System.Windows.Forms.Label();
        this.btnSubmit = new System.Windows.Forms.Button();
        this.pnlHeader = new System.Windows.Forms.Panel();
        this.btnNxt = new System.Windows.Forms.Button();
        this.btnCancel = new Bunifu.Framework.UI.BunifuImageButton();
        this.bunifuElipse_0 = new Bunifu.Framework.UI.BunifuElipse(this.components);
        this.pnlHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
        this.SuspendLayout();
        // 
        // lblError
        // 
        this.lblError.Cursor = System.Windows.Forms.Cursors.Hand;
        this.lblError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
        this.lblError.ForeColor = System.Drawing.Color.Black;
        this.lblError.Location = new System.Drawing.Point(-1, 16);
        this.lblError.Name = "lblError";
        this.lblError.Size = new System.Drawing.Size(222, 28);
        this.lblError.TabIndex = 18;
        this.lblError.Text = "Select another Payment";
        this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.lblError.Visible = false;
        this.lblError.Click += new System.EventHandler(this.lblError_Click);
        // 
        // tktname
        // 
        this.tktname.AutoSize = true;
        this.tktname.BackColor = System.Drawing.Color.Transparent;
        this.tktname.ForeColor = System.Drawing.Color.Black;
        this.tktname.Location = new System.Drawing.Point(-1, 2);
        this.tktname.Name = "tktname";
        this.tktname.Size = new System.Drawing.Size(45, 13);
        this.tktname.TabIndex = 23;
        this.tktname.Text = "tktname";
        // 
        // btnSubmit
        // 
        this.btnSubmit.BackColor = System.Drawing.Color.DarkGreen;
        this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold);
        this.btnSubmit.ForeColor = System.Drawing.Color.White;
        this.btnSubmit.Location = new System.Drawing.Point(10, 46);
        this.btnSubmit.Margin = new System.Windows.Forms.Padding(2);
        this.btnSubmit.Name = "btnSubmit";
        this.btnSubmit.Size = new System.Drawing.Size(200, 23);
        this.btnSubmit.TabIndex = 1194;
        this.btnSubmit.Text = "Process Payment";
        this.btnSubmit.UseVisualStyleBackColor = false;
        this.btnSubmit.Click += new System.EventHandler(this.btnNxt_Click);
        // 
        // pnlHeader
        // 
        this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(22, 14, 28);
        this.pnlHeader.Controls.Add(this.tktname);
        this.pnlHeader.Controls.Add(this.btnNxt);
        this.pnlHeader.Controls.Add(this.btnCancel);
        this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlHeader.ForeColor = System.Drawing.Color.White;
        this.pnlHeader.Location = new System.Drawing.Point(0, 0);
        this.pnlHeader.Name = "pnlHeader";
        this.pnlHeader.Size = new System.Drawing.Size(221, 15);
        this.pnlHeader.TabIndex = 1188;
        // 
        // btnNxt
        // 
        this.btnNxt.BackColor = System.Drawing.Color.Coral;
        this.btnNxt.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnNxt.Location = new System.Drawing.Point(248, 0);
        this.btnNxt.Name = "btnNxt";
        this.btnNxt.Size = new System.Drawing.Size(41, 21);
        this.btnNxt.TabIndex = 1195;
        this.btnNxt.Text = "Next";
        this.btnNxt.UseVisualStyleBackColor = false;
        this.btnNxt.Click += new System.EventHandler(this.btnNxt_Click);
        // 
        // btnCancel
        // 
        this.btnCancel.BackColor = System.Drawing.Color.Transparent;
        this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
        this.btnCancel.ImageActive = null;
        this.btnCancel.Location = new System.Drawing.Point(204, -1);
        this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(15, 15);
        this.btnCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.btnCancel.TabIndex = 1187;
        this.btnCancel.TabStop = false;
        this.btnCancel.Zoom = 10;
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
        // 
        // bunifuElipse_0
        // 
        this.bunifuElipse_0.ElipseRadius = 5;
        this.bunifuElipse_0.TargetControl = this;
        // 
        // frmPayment
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
        this.BackColor = System.Drawing.Color.White;
        this.ClientSize = new System.Drawing.Size(221, 81);
        this.Controls.Add(this.btnSubmit);
        this.Controls.Add(this.pnlHeader);
        this.Controls.Add(this.lblError);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.Name = "frmPayment";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.Text = "frmPayment";
        this.TopMost = true;
        this.Load += new System.EventHandler(this.frmPayment_Load);
        this.pnlHeader.ResumeLayout(false);
        this.pnlHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
        this.ResumeLayout(false);

    }
}
