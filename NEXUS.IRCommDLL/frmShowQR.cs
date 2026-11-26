using Bunifu.Framework.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEXUS.IRCommDLL;

public class frmShowQR : Form
{
    private clsPostData class42_0;
    internal clsMain clsMain;

    private const uint uint_0 = 1u;
    private const uint uint_1 = 2u;
    internal const uint uint_2 = 3u;

    internal static readonly IntPtr intptr_0 = new IntPtr(-1);

    private Dictionary<string, string> dictionary_0 =
        new Dictionary<string, string>();

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

    private IContainer components;
    private BunifuElipse bunifuElipse1;
    private PictureBox pictureBox1;
    private Label label1;

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(
        IntPtr intptr_1,
        IntPtr intptr_2,
        int int_0,
        int int_1,
        int int_2,
        int int_3,
        uint uint_3);

    public frmShowQR()
    {
        InitializeComponent();
        ApplyByteNovaTheme();
    }

    // =========================================================
    // EXPRESS THEME
    // =========================================================
    private void ApplyByteNovaTheme()
    {
        // Express theme: light page, bhagwa header, white card, pixel-perfect QR.
        SuspendLayout();

        Color qBg = Color.FromArgb(236, 242, 251);
        Color qOrange = Color.FromArgb(245, 146, 28);
        Color qNavy = Color.FromArgb(15, 42, 92);
        Color qText = Color.FromArgb(30, 41, 59);
        Color qMuted = Color.FromArgb(100, 116, 139);

        BackColor = qBg;
        ForeColor = qText;
        FormBorderStyle = FormBorderStyle.None;
        DoubleBuffered = true;
        // Default size of the ticket window this popup covers; once shown it takes that
        // window's exact bounds (see qMatch below) and the layout adapts to ClientSize.
        ClientSize = new Size(465, 290);
        bunifuElipse1.ElipseRadius = 10;

        // ---------------- QR area ----------------
        // The PictureBox covers the QR card's inner square (bounds set by qLayout). Its own
        // image drawing is replaced in the Paint handler below by a pixel-perfect render.
        pictureBox1.Dock = DockStyle.None;
        pictureBox1.BackColor = Color.White;
        pictureBox1.BorderStyle = BorderStyle.None;
        pictureBox1.SizeMode = PictureBoxSizeMode.Normal;   // no bilinear scaling by the PictureBox

        Image qSrc = null;          // image the cache was built from
        Bitmap qCrisp = null;       // rebuilt QR: 1 colour per module, integer scale
        int qPhase = 0;             // scan-marker animation phase
        int qModPx = 0;             // module size (px) of the rebuilt QR
        int qBuiltFor = -1;         // box size the cache was built for
        float qMarkX = 9;           // x of the left scan marker (for cheap invalidation)

        // Builds a perfectly sharp QR bitmap from whatever image the server sent.
        // Detects the module grid (finder pattern = 7 modules) and re-draws every module
        // as a solid square at an integer scale, with a 4-module white quiet zone.
        Func<Image, int, Bitmap> buildCrisp = (img, avail) =>
        {
            if (img == null || img.Width < 21 || img.Height < 21) return null;
            using (Bitmap src = new Bitmap(img.Width, img.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (Graphics sg = Graphics.FromImage(src))
                {
                    sg.Clear(Color.White);
                    sg.InterpolationMode = InterpolationMode.NearestNeighbor;
                    sg.PixelOffsetMode = PixelOffsetMode.Half;
                    sg.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
                }
                int w = src.Width, h = src.Height;
                int[] px = new int[w * h];
                System.Drawing.Imaging.BitmapData bd = src.LockBits(new Rectangle(0, 0, w, h),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                try { Marshal.Copy(bd.Scan0, px, 0, px.Length); }
                finally { src.UnlockBits(bd); }

                Func<int, int, bool> dark = (x, y) =>
                {
                    int c = px[y * w + x];
                    int r = (c >> 16) & 255, gg = (c >> 8) & 255, b = c & 255;
                    return (r * 299 + gg * 587 + b * 114) / 1000 < 128;
                };

                int minX = w, minY = h, maxX = -1, maxY = -1;
                for (int y = 0; y < h; y++)
                    for (int x = 0; x < w; x++)
                        if (dark(x, y))
                        {
                            if (x < minX) minX = x;
                            if (x > maxX) maxX = x;
                            if (y < minY) minY = y;
                            if (y > maxY) maxY = y;
                        }
                if (maxX < 0) return null;

                // top-left finder pattern: its top row is 7 dark modules
                int run = 0;
                // (checked on the first few rows: anti-aliased corners can make the very first row lighter)
                for (int yy = minY; yy <= Math.Min(maxY, minY + 3); yy++)
                {
                    int sx0 = minX;
                    while (sx0 <= maxX && !dark(sx0, yy)) sx0++;
                    int r0 = 0;
                    while (sx0 + r0 <= maxX && dark(sx0 + r0, yy)) r0++;
                    if (r0 > run) run = r0;
                }
                double mod = run / 7.0;
                if (mod < 1) return null;
                double bw = maxX - minX + 1, bh = maxY - minY + 1;
                int n = (int)Math.Round(bw / mod);
                if (n < 21 || n > 177 || (n - 17) % 4 != 0) return null;
                if (Math.Abs(bw / mod - n) > 1.0 || Math.Abs(bw - bh) > 1.5 * mod) return null;   // rough shape check; the strict per-module check follows

                // ---- read the module grid with a strict self-check ----
                // Each module is sampled on a 3x3 grid inside its central half. A module is
                // "ambiguous" if its samples disagree or their average is mid-grey (64..192).
                Func<int, int, int> lum = (x, y) =>
                {
                    int c = px[y * w + x];
                    return (((c >> 16) & 255) * 299 + ((c >> 8) & 255) * 587 + (c & 255) * 114) / 1000;
                };
                double mw = bw / n, mh = bh / n;
                bool[,] grid = new bool[n, n];
                int ambiguous = 0;
                for (int j = 0; j < n; j++)
                    for (int i = 0; i < n; i++)
                    {
                        int dk = 0, cnt = 0; long sum = 0;
                        for (int oy = -1; oy <= 1; oy++)
                            for (int ox = -1; ox <= 1; ox++)
                            {
                                int sx = Math.Max(0, Math.Min(w - 1, (int)(minX + (i + 0.5 + ox * 0.25) * mw)));
                                int sy = Math.Max(0, Math.Min(h - 1, (int)(minY + (j + 0.5 + oy * 0.25) * mh)));
                                int l = lum(sx, sy);
                                sum += l; cnt++;
                                if (l < 128) dk++;
                            }
                        double avg = (double)sum / cnt;
                        grid[i, j] = dk * 2 > cnt;
                        if ((dk != 0 && dk != cnt) || (avg > 64 && avg < 192)) ambiguous++;
                    }
                if (ambiguous > n * n * 0.02) return null;          // > 2% unsure -> use the original image

                // all three finder patterns must match exactly, including their white separators
                Func<int, int, bool> finderOk = (fx, fy) =>
                {
                    for (int y = fy - 1; y <= fy + 7; y++)
                        for (int x = fx - 1; x <= fx + 7; x++)
                        {
                            if (x < 0 || y < 0 || x >= n || y >= n) continue;
                            int u = x - fx, v = y - fy;
                            bool inside = u >= 0 && u <= 6 && v >= 0 && v <= 6;
                            bool want = inside && (u == 0 || u == 6 || v == 0 || v == 6 || (u >= 2 && u <= 4 && v >= 2 && v <= 4));
                            if (grid[x, y] != want) return false;
                        }
                    return true;
                };
                if (!finderOk(0, 0) || !finderOk(n - 7, 0) || !finderOk(0, n - 7)) return null;

                // `avail` = full white square (QR box + the white card padding around it);
                // the code plus a 4-module quiet zone on every side must fit inside it.
                int total = n + 8;                                   // quiet zone: 4 modules each side
                int scale = Math.Max(1, avail / total);
                qModPx = scale;
                Bitmap outB = new Bitmap(total * scale, total * scale, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics og = Graphics.FromImage(outB))
                {
                    og.Clear(Color.White);
                    og.SmoothingMode = SmoothingMode.None;
                    for (int j = 0; j < n; j++)
                        for (int i = 0; i < n; i++)
                            if (grid[i, j])
                                og.FillRectangle(Brushes.Black, (i + 4) * scale, (j + 4) * scale, scale, scale);
                }
                return outB;
            }
        };

        pictureBox1.Paint += (s, pe) =>
        {
            Graphics g = pe.Graphics;
            int W = pictureBox1.Width, H = pictureBox1.Height;
            g.Clear(Color.White);

            // The whole white card counts as the quiet zone: the PictureBox is inset 3px in the
            // white card, so the white square available to the code is the box + 3px each side.
            int side = Math.Min(W, H);
            int avail = side + 6;

            Image img = pictureBox1.Image;
            if (!ReferenceEquals(img, qSrc) || qBuiltFor != avail)
            {
                if (qCrisp != null) { qCrisp.Dispose(); qCrisp = null; }
                qSrc = img;
                qBuiltFor = avail;
                qModPx = 0;
                try { qCrisp = buildCrisp(img, avail); } catch { qCrisp = null; }
            }

            Rectangle code;
            g.SmoothingMode = SmoothingMode.None;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            if (qCrisp != null && qModPx > 0)
            {
                // centred; the bitmap's own white border (4 modules) may overlap the white card padding
                int bx = (W - qCrisp.Width) / 2, by = (H - qCrisp.Height) / 2;
                g.DrawImageUnscaled(qCrisp, bx, by);
                int q4 = 4 * qModPx;
                code = new Rectangle(bx + q4, by + q4, qCrisp.Width - 2 * q4, qCrisp.Height - 2 * q4);
            }
            else if (img != null)
            {
                // Fallback (not verified as a plain QR): integer upscale if it fits, otherwise fit,
                // always nearest-neighbour so edges stay hard. The image keeps its own margin.
                int k = Math.Max(img.Width, img.Height) <= side ? side / Math.Max(img.Width, img.Height) : 0;
                Size sz = k >= 1
                    ? new Size(img.Width * k, img.Height * k)
                    : new Size((int)(img.Width * (double)side / Math.Max(img.Width, img.Height)),
                               (int)(img.Height * (double)side / Math.Max(img.Width, img.Height)));
                code = new Rectangle((W - sz.Width) / 2, (H - sz.Height) / 2, sz.Width, sz.Height);
                g.DrawImage(img, code);
            }
            else
            {
                code = new Rectangle(0, 0, W, H);
                using (Font lf = new Font("Segoe UI", 9f * 4f / 3f, FontStyle.Regular, GraphicsUnit.Pixel)) TextRenderer.DrawText(g, "Loading QR...", lf, code, qMuted,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            // Corner brackets are drawn on the white card's padding ring (ByteNovaPaint), so the
            // code keeps its full quiet zone. Here only the animated side markers are drawn, and
            // only when there is spare white so their inner edge stays >= 3 modules from the code.
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.Default;
            float gapPx = qModPx > 0 ? 3 * qModPx : 6;
            float b0 = code.Left - gapPx - 2f;
            if (b0 < 2.5f) { qMarkX = -1; return; }                 // no spare white: keep the code clean
            float bl = Math.Max(12f, Math.Min(26f, qModPx > 0 ? 3.5f * qModPx : side / 9f));
            RectangleF fr = new RectangleF(b0, b0, W - 1 - b0 * 2, H - 1 - b0 * 2);
            qMarkX = b0;

            // animated scan markers that glide up and down on the LEFT and RIGHT margins only
            double t = (Math.Sin(qPhase * 0.06) + 1) / 2;                 // 0..1
            float mh = Math.Min(28f, Math.Max(12f, fr.Height - bl * 2 - 16));
            float my = (float)(fr.Top + bl + 4 + mh / 2 + t * Math.Max(0, fr.Height - bl * 2 - 8 - mh));
            using (LinearGradientBrush mk = new LinearGradientBrush(new RectangleF(0, my - mh / 2 - 1, 6, mh + 2),
                Color.FromArgb(0, qOrange), qOrange, LinearGradientMode.Vertical))
            {
                ColorBlend cb = new ColorBlend();
                cb.Positions = new[] { 0f, 0.5f, 1f };
                cb.Colors = new[] { Color.FromArgb(0, qOrange), qOrange, Color.FromArgb(0, qOrange) };
                mk.InterpolationColors = cb;
                g.FillRectangle(mk, fr.Left - 1.5f, my - mh / 2, 3, mh);
                g.FillRectangle(mk, fr.Right - 1.5f, my - mh / 2, 3, mh);
            }
        };

        // smooth, low-cost animation; stops when the window closes
        Timer qTimer = new Timer { Interval = 33 };
        qTimer.Tick += (s, te) =>
        {
            if (IsDisposed || !Visible) return;
            qPhase++;
            int W = pictureBox1.Width, H = pictureBox1.Height;
            if (qMarkX < 0) return;                                        // markers not shown at this size
            int mx = (int)qMarkX + 4;                                      // only the marker strips repaint
            pictureBox1.Invalidate(new Rectangle(Math.Max(0, mx - 8), 0, 8, H));
            pictureBox1.Invalidate(new Rectangle(Math.Max(0, W - mx), 0, 8, H));
        };
        qTimer.Start();
        FormClosed += (s, fe) => { qTimer.Stop(); qTimer.Dispose(); };
        Disposed += (s, de) => { qTimer.Stop(); if (qCrisp != null) { qCrisp.Dispose(); qCrisp = null; } };

        // ---------------- corner close badge geometry (shared by layout, paint and hit-test) ----------------
        // A ~30px (at 96 DPI) red circle sitting on the top-right corner, centre ~(W-19, 19).
        float qDpiK = 1f;
        try { using (Graphics sg = Graphics.FromHwnd(IntPtr.Zero)) qDpiK = Math.Max(1f, sg.DpiX / 96f); } catch { }
        float qBadgeR = 15f * qDpiK;
        // ---------------- "sticker" close badge: Win32 binding ----------------
        // The sticker is an owned layered window with per-pixel alpha (UpdateLayeredWindow), like the
        // ticket popup's. DllImports can't be declared outside these two methods, so the eight Win32
        // calls are bound at runtime with Reflection.Emit (DefinePInvokeMethod) and cached per AppDomain.
        // If that is unavailable for any reason, the in-header badge below is used instead.
        Type qNative = null;
        try
        {
            qNative = AppDomain.CurrentDomain.GetData("Express.QrStickerNative") as Type;
            if (qNative == null)
            {
                var an = new System.Reflection.AssemblyName("Express.QrStickerNative");
                var ab = AppDomain.CurrentDomain.DefineDynamicAssembly(an, System.Reflection.Emit.AssemblyBuilderAccess.Run);
                var mb = ab.DefineDynamicModule("Express.QrStickerNative");
                var tb = mb.DefineType("QrStickerNative",
                    System.Reflection.TypeAttributes.Public | System.Reflection.TypeAttributes.Abstract | System.Reflection.TypeAttributes.Sealed);
                Action<string, string, Type, Type[]> pinv = (dll, name, ret, args) =>
                {
                    var m = tb.DefinePInvokeMethod(name, dll,
                        System.Reflection.MethodAttributes.Public | System.Reflection.MethodAttributes.Static | System.Reflection.MethodAttributes.PinvokeImpl,
                        System.Reflection.CallingConventions.Standard, ret, args,
                        CallingConvention.Winapi, CharSet.Unicode);
                    m.SetImplementationFlags(m.GetMethodImplementationFlags() | System.Reflection.MethodImplAttributes.PreserveSig);
                };
                Type ip = typeof(IntPtr), rp = typeof(Point).MakeByRefType(), rs = typeof(Size).MakeByRefType(), ri = typeof(int).MakeByRefType();
                pinv("user32.dll", "UpdateLayeredWindow", typeof(bool), new[] { ip, ip, rp, rs, ip, rp, typeof(int), ri, typeof(int) });
                pinv("user32.dll", "GetWindowLongW", typeof(int), new[] { ip, typeof(int) });
                pinv("user32.dll", "SetWindowLongW", typeof(int), new[] { ip, typeof(int), typeof(int) });
                pinv("user32.dll", "ShowWindow", typeof(bool), new[] { ip, typeof(int) });
                pinv("gdi32.dll", "CreateCompatibleDC", ip, new[] { ip });
                pinv("gdi32.dll", "SelectObject", ip, new[] { ip, ip });
                pinv("gdi32.dll", "DeleteDC", typeof(bool), new[] { ip });
                pinv("gdi32.dll", "DeleteObject", typeof(bool), new[] { ip });
                qNative = tb.CreateType();
                AppDomain.CurrentDomain.SetData("Express.QrStickerNative", qNative);
            }
        }
        catch { qNative = null; }
        bool qSticker = qNative != null;
        Func<string, object[], object> qCall = (name, args) => qNative.GetMethod(name).Invoke(null, args);

        // The header is tall enough to hold the badge: diameter + 6px (36px at 100%).
        int qHdrH = qSticker ? 26 : (int)Math.Ceiling(qBadgeR * 2f + 6f);   // sticker sits outside: slim header again
        // Badge flush in the top-right corner: ~3px from the top and right edges, centred in the header.
        Func<PointF> qBadgeCentre = () => new PointF(ClientSize.Width - 3f - qBadgeR, qHdrH / 2f);
        Func<Rectangle> qBadgeBox = () =>
        {
            PointF c = qBadgeCentre();
            int half = (int)Math.Ceiling(qBadgeR + 4f);                 // hover growth + shadow
            int x = (int)Math.Floor(c.X - half), y = (int)Math.Floor(c.Y - half);
            int w = half * 2 + 1, h = half * 2 + 1;
            x = Math.Max(0, Math.Min(x, ClientSize.Width - w));          // must stay inside the window
            y = Math.Max(0, Math.Min(y, ClientSize.Height - h));
            return new Rectangle(x, y, w, h);
        };

        // ---------------- payment info (right-hand panel) ----------------
        // label1.Text is set by the logic ("Pay Using QR :- <upi>  Amt=  <amount>");
        // the label paints it as a vertical info panel: Scan & Pay / PAY TO / AMOUNT / hint.
        label1.AutoSize = false;
        label1.BackColor = Color.White;
        label1.ForeColor = qText;
        label1.Font = new Font("Segoe UI Semibold", 9f * 4f / 3f, FontStyle.Regular, GraphicsUnit.Pixel);
        label1.TextAlign = ContentAlignment.MiddleCenter;
        label1.UseMnemonic = false;
        label1.Paint += (s, le) =>
        {
            Graphics g = le.Graphics;
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            string raw = label1.Text ?? "";
            if (raw == "label1") raw = "";
            int ai = raw.IndexOf("Amt=", StringComparison.OrdinalIgnoreCase);
            int W = label1.Width, H = label1.Height, x0 = 6, cw = Math.Max(20, W - 12);

            // Design unit: label1.Font is 12px (9pt at 96 DPI); every size below is relative to it,
            // in pixels, so the layout is DPI-independent and fully measured.
            float u = Math.Max(0.5f, label1.Font.Size / 12f);
            TextFormatFlags ONE = TextFormatFlags.Left | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine |
                                  TextFormatFlags.NoPrefix | TextFormatFlags.NoClipping;
            Func<float, FontStyle, Font> F = (pt, st) => new Font("Segoe UI", pt * u * 4f / 3f, st, GraphicsUnit.Pixel);
            Func<string, Font, Size> M = (t, f) => TextRenderer.MeasureText(g, string.IsNullOrEmpty(t) ? " " : t, f, Size.Empty,
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);

            // The corner close badge may hang over the panel's top-right: rows in that band get less width.
            Rectangle bb = qBadgeBox();
            bb.Offset(-label1.Left, -label1.Top);
            bb.Inflate(4, 2);
            Func<int, int, int> widthAt = (top, h) =>
                (top < bb.Bottom && top + h > bb.Top && bb.Left < x0 + cw) ? Math.Max(24, bb.Left - x0) : cw;

            // Greedy word wrap; a word wider than the line is split by characters (prefers a break
            // before '@'). Never ellipsis, never drops characters.
            Func<string, Font, int, List<string>> wrap = (text, f, maxW) =>
            {
                List<string> lines = new List<string>();
                string cur = "";
                foreach (string word in (text ?? "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string trial = cur.Length == 0 ? word : cur + " " + word;
                    if (M(trial, f).Width <= maxW) { cur = trial; continue; }
                    if (cur.Length > 0) { lines.Add(cur); cur = ""; }
                    string w = word;
                    while (w.Length > 1 && M(w, f).Width > maxW)
                    {
                        int cut = w.Length - 1;
                        while (cut > 1 && M(w.Substring(0, cut), f).Width > maxW) cut--;
                        int at = w.LastIndexOf('@', cut - 1);
                        if (at > cut / 2) cut = at;
                        lines.Add(w.Substring(0, cut));
                        w = w.Substring(cut);
                    }
                    cur = w;
                }
                if (cur.Length > 0) lines.Add(cur);
                if (lines.Count == 0) lines.Add("");
                return lines;
            };
            Func<List<string>, Font, int> blockH = (ls, f) => { int hh = 0; foreach (string l in ls) hh += M(l, f).Height; return hh; };
            Action<List<string>, Font, int, Color> drawBlock = (ls, f, yy, col) =>
            {
                foreach (string l in ls)
                {
                    int lh = M(l, f).Height;
                    TextRenderer.DrawText(g, l, f, new Rectangle(x0, yy, cw + 4, lh + 1), col, ONE);
                    yy += lh;
                }
            };

            if (ai < 0)
            {
                // unknown text format: show it all, wrapped, shrinking until it fits
                float fs = 9f;
                while (true)
                {
                    using (Font f = F(fs, FontStyle.Bold))
                    {
                        List<string> ls = wrap(raw, f, cw);
                        if (blockH(ls, f) <= H - 8 || fs <= 6.5f) { drawBlock(ls, f, 4, qText); break; }
                    }
                    fs -= 0.5f;
                }
                return;
            }

            string left = raw.Substring(0, ai);
            int ci = left.IndexOf(":-");
            string payTo = (ci >= 0 ? left.Substring(ci + 2) : left).Trim();
            string amtText = ((char)0x20B9).ToString() + " " + raw.Substring(ai + 4).Trim();   // rupee sign
            const string title = "Scan & Pay", hint = "Open any UPI app and scan the QR";

            // sizes (pt at 96 DPI, scaled by u); shrink order: title -> hint -> pay-to -> drop hint -> amount
            float tS = 12f, hS = 7.75f, pS = 10f, cS = 7f, aS = 20f;
            bool showHint = true;
            while (aS > 10f) { using (Font af = F(aS, FontStyle.Bold)) if (M(amtText, af).Width <= cw) break; aS -= 1f; }

            for (int guard = 0; guard < 80; guard++)
            {
                using (Font tf = F(tS, FontStyle.Bold))
                using (Font hf = F(hS, FontStyle.Regular))
                using (Font cf = F(cS, FontStyle.Bold))
                using (Font pf = F(pS, FontStyle.Bold))
                using (Font af = F(aS, FontStyle.Bold))
                {
                    int capH = M("PAY TO", cf).Height;
                    int iconW = (int)(22 * u);
                    int y = 4;
                    int tH = Math.Max((int)(16 * u), M(title, tf).Height);
                    int tW = widthAt(y, tH) - iconW;
                    bool titleFits = M(title, tf).Width <= tW;
                    y += tH + (int)(3 * u);
                    List<string> hl = showHint ? wrap(hint, hf, widthAt(y, 2 * M("Ag", hf).Height)) : new List<string>();
                    int hintH = showHint ? blockH(hl, hf) : 0;
                    y += hintH + (int)(6 * u);
                    int divY = y;
                    y += 1 + (int)(6 * u);
                    int payCapY = y;
                    y += capH + (int)(2 * u);
                    List<string> pl = wrap(payTo, pf, widthAt(y, 2 * M("Ag", pf).Height));
                    int payY = y;
                    y += blockH(pl, pf);
                    int amtH = M(amtText, af).Height;
                    int need = y + (int)(8 * u) + capH + (int)(2 * u) + amtH + 4;

                    bool fits = need <= H && titleFits && (!showHint || hl.Count <= 3);
                    bool canShrink = tS > 9f || hS > 6.5f || pS > 7.5f || showHint || aS > 10f;
                    if (fits || !canShrink)
                    {
                        // ---- draw ----
                        using (Pen ip = new Pen(Color.FromArgb(214, 96, 4), 1.8F * u))
                        {
                            ip.StartCap = LineCap.Round; ip.EndCap = LineCap.Round; ip.LineJoin = LineJoin.Round;
                            float sz = 14 * u, c = 4.5f * u, ix = x0, iy = 4 + (tH - sz) / 2f;
                            g.DrawLines(ip, new[] { new PointF(ix, iy + c), new PointF(ix, iy), new PointF(ix + c, iy) });
                            g.DrawLines(ip, new[] { new PointF(ix + sz - c, iy), new PointF(ix + sz, iy), new PointF(ix + sz, iy + c) });
                            g.DrawLines(ip, new[] { new PointF(ix + sz, iy + sz - c), new PointF(ix + sz, iy + sz), new PointF(ix + sz - c, iy + sz) });
                            g.DrawLines(ip, new[] { new PointF(ix + c, iy + sz), new PointF(ix, iy + sz), new PointF(ix, iy + sz - c) });
                            g.DrawLine(ip, ix + 2.5f * u, iy + sz / 2, ix + sz - 2.5f * u, iy + sz / 2);
                        }
                        int titleTextH = M(title, tf).Height;
                        TextRenderer.DrawText(g, title, tf, new Rectangle(x0 + iconW, 4 + (tH - titleTextH) / 2, cw, titleTextH + 1), qNavy, ONE);
                        if (showHint) drawBlock(hl, hf, 4 + tH + (int)(3 * u), qMuted);
                        using (Pen sep = new Pen(Color.FromArgb(226, 232, 240)))
                            g.DrawLine(sep, x0, divY, x0 + cw, divY);
                        TextRenderer.DrawText(g, "PAY TO", cf, new Rectangle(x0, payCapY, cw, capH + 1), qMuted, ONE);
                        drawBlock(pl, pf, payY, qNavy);
                        // AMOUNT anchored to the bottom, always complete
                        int ay = Math.Max(payY + blockH(pl, pf) + (int)(4 * u) + capH, H - 4 - amtH);
                        TextRenderer.DrawText(g, "AMOUNT", cf, new Rectangle(x0, ay - capH - (int)(2 * u), cw, capH + 1), qMuted, ONE);
                        TextRenderer.DrawText(g, amtText, af, new Rectangle(x0, ay, cw + 4, amtH + 1), Color.FromArgb(214, 96, 4), ONE);
                        break;
                    }
                }
                if (tS > 9f) tS -= 0.5f;
                else if (hS > 6.5f) hS -= 0.25f;
                else if (pS > 7.5f) pS -= 0.5f;
                else if (showHint) showHint = false;          // secondary text goes before anything is cut
                else if (aS > 10f) aS -= 1f;
            }
        };

        // ---------------- close (X) button in the header ----------------
        // Always visible (harmless). It HIDES the popup instead of closing it: the owner code
        // keeps this instance and calls method_1 -> method_0 -> Show() for the next QR, so a
        // disposed form would make the next QR silently never appear.
        int xState = 0;                                     // 0 normal, 1 hover, 2 pressed
        // Corner "patch" badge. Transparent PictureBox: WinForms paints the form (header ribbon,
        // cards) behind it, so there are no square edges around the circle. PictureBox is
        // double-buffered, so hover/press don't flicker.
        PictureBox xBtn = new PictureBox
        {
            BackColor = Color.Transparent,
            Cursor = Cursors.Hand,
            TabStop = false
        };
        xBtn.Paint += (s, xe) =>
        {
            Graphics g = xe.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            PointF c0 = qBadgeCentre();
            float r = qBadgeR + (xState == 1 ? 1f : 0f);                  // hover: grows ~2px
            float cx = c0.X - xBtn.Left, cy = c0.Y - xBtn.Top + (xState == 2 ? 1f : 0f);

            // soft drop shadow
            if (xState != 2)
            {
                using (SolidBrush sh = new SolidBrush(Color.FromArgb(28, 0, 0, 0)))
                    g.FillEllipse(sh, cx - r - 1f, cy - r + 0.5f, (r + 1f) * 2, (r + 1f) * 2);
                using (SolidBrush sh = new SolidBrush(Color.FromArgb(46, 0, 0, 0)))
                    g.FillEllipse(sh, cx - r, cy - r + 1.8f, r * 2, r * 2);
            }

            Color ring = xState == 1 ? Color.FromArgb(220, 38, 38) : Color.White;
            Color fill = xState == 2 ? Color.FromArgb(153, 27, 27)
                       : xState == 1 ? Color.White
                       : Color.FromArgb(220, 38, 38);
            Color glyph = xState == 1 ? Color.FromArgb(220, 38, 38) : Color.White;
            float ringW = 2f * qDpiK;
            using (SolidBrush rb = new SolidBrush(ring)) g.FillEllipse(rb, cx - r, cy - r, r * 2, r * 2);
            using (SolidBrush fb = new SolidBrush(fill)) g.FillEllipse(fb, cx - r + ringW, cy - r + ringW, (r - ringW) * 2, (r - ringW) * 2);

            float k = r * 0.34f;
            using (Pen xp = new Pen(glyph, 2.6f * qDpiK))
            {
                xp.StartCap = LineCap.Round; xp.EndCap = LineCap.Round;
                g.DrawLine(xp, cx - k, cy - k, cx + k, cy + k);
                g.DrawLine(xp, cx + k, cy - k, cx - k, cy + k);
            }
        };
        xBtn.MouseEnter += (s, xe) => { xState = 1; xBtn.Invalidate(); };
        xBtn.MouseLeave += (s, xe) => { xState = 0; xBtn.Invalidate(); };
        xBtn.MouseDown += (s, xe) => { if (xe.Button == MouseButtons.Left) { xState = 2; xBtn.Invalidate(); } };
        xBtn.MouseUp += (s, xe) =>
        {
            if (xBtn.IsDisposed) return;
            xState = xBtn.ClientRectangle.Contains(xe.Location) ? 1 : 0;
            xBtn.Invalidate();
        };
        // ---- what closing means, and why it is robust ----
        // Root causes found for "X does nothing" in the real app:
        //  (1) while any ShowDialog runs in the app, WinForms disables every other top-level
        //      window of the thread -> this popup receives NO mouse messages at all;
        //  (2) the app re-sends the QR (method_1 -> method_0 -> Show/Activate), which re-shows
        //      the popup right after the user hid it.
        // Fixes: close on MouseUp (not Click); a fallback that reads the real mouse state from
        // the UI timer (works even while the window is disabled); and the same QR is not
        // re-shown after the user hid it (a different QR shows normally).
        string qHiddenSig = null;
        Func<string> qSig = () =>
        {
            long hsh = 17;
            Image im = pictureBox1.Image;
            if (im != null)
            {
                try
                {
                    using (Bitmap sb = new Bitmap(im, new Size(40, 40)))
                        for (int yy = 0; yy < 40; yy++)
                            for (int xx = 0; xx < 40; xx++)
                                hsh = unchecked(hsh * 31 + sb.GetPixel(xx, yy).ToArgb());
                }
                catch { }
            }
            return (label1.Text ?? "") + "|" + hsh;
        };
        int qStState = 0;                                   // sticker: 0 normal, 1 hover, 2 pressed
        bool xPrevDown = false, xArmed = false;             // polling state (reset whenever the popup hides)
        Action qUserClose = () =>
        {
            if (IsDisposed || !Visible) return;
            xState = 0;
            qStState = 0;
            xPrevDown = false; xArmed = false;
            qHiddenSig = qSig();
            Hide();                                         // never Close(): the owner reuses this instance
        };
        xBtn.MouseUp += (s, xe) =>
        {
            if (xe.Button == MouseButtons.Left && Rectangle.Inflate(xBtn.ClientRectangle, 4, 4).Contains(xe.Location))
                qUserClose();
        };
        VisibleChanged += (s, ve) =>
        {
            if (!Visible || qHiddenSig == null || IsDisposed) return;
            if (qSig() == qHiddenSig)
                BeginInvoke((Action)(() => { if (!IsDisposed) Hide(); }));   // same QR again: stay closed
            else
                qHiddenSig = null;                                           // new QR: show normally
        };

        // ---------------- "sticker" close badge: owned layered window on the top-right corner ----------------
        // 32px red disc (96 DPI) with a white ring, soft shadow and white X, centred at
        // (Right - 8, Top + 8): about half of it sits outside the popup. Per-pixel alpha gives perfectly
        // smooth edges over anything behind it. WS_EX_NOACTIVATE: clicking it never steals focus.
        Form qSt = null;
        bool qStShown = false;
        float qStR = 16f * qDpiK;
        int qStS = (int)Math.Ceiling(qStR * 2f + 12f * qDpiK);           // disc + hover growth + shadow
        Func<Point> qStCentre = () => new Point(Right - 8, Top + 8);       // screen pixels, like the ticket popup
        Action qStRender = () =>
        {
            if (!qSticker || qSt == null || qSt.IsDisposed || !qSt.IsHandleCreated) return;
            try
            {
                using (Bitmap bmp = new Bitmap(qStS, qStS, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        g.Clear(Color.Transparent);
                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        float r = qStR + (qStState == 1 ? 1f : 0f);
                        float cx = qStS / 2f, cy = qStS / 2f - 1.5f * qDpiK + (qStState == 2 ? 1f : 0f);
                        if (qStState != 2)
                            for (int i = 4; i >= 1; i--)                              // soft drop shadow
                                using (SolidBrush sh = new SolidBrush(Color.FromArgb(14 + (4 - i) * 10, 0, 0, 0)))
                                {
                                    float rr = r + i * 0.7f * qDpiK;
                                    g.FillEllipse(sh, cx - rr, cy - rr + 2f * qDpiK, rr * 2, rr * 2);
                                }
                        Color ring = qStState == 1 ? Color.FromArgb(220, 38, 38) : Color.White;
                        Color fill = qStState == 2 ? Color.FromArgb(153, 27, 27)
                                   : qStState == 1 ? Color.White
                                   : Color.FromArgb(220, 38, 38);
                        Color glyph = qStState == 1 ? Color.FromArgb(220, 38, 38) : Color.White;
                        float ringW = 2f * qDpiK;
                        using (SolidBrush rb = new SolidBrush(ring)) g.FillEllipse(rb, cx - r, cy - r, r * 2, r * 2);
                        using (SolidBrush fb = new SolidBrush(fill)) g.FillEllipse(fb, cx - r + ringW, cy - r + ringW, (r - ringW) * 2, (r - ringW) * 2);
                        float k = r * 0.33f;
                        using (Pen xp = new Pen(glyph, 2.6f * qDpiK))
                        {
                            xp.StartCap = LineCap.Round; xp.EndCap = LineCap.Round;
                            g.DrawLine(xp, cx - k, cy - k, cx + k, cy + k);
                            g.DrawLine(xp, cx + k, cy - k, cx - k, cy + k);
                        }
                    }
                    // make sure the window is layered + non-activating (WinForms may create or refresh the
                    // handle before/after our HandleCreated hook, e.g. when Owner is assigned)
                    int exs = (int)qCall("GetWindowLongW", new object[] { qSt.Handle, -20 });
                    const int want = 0x80000 | 0x80 | 0x8000000;           // LAYERED | TOOLWINDOW | NOACTIVATE
                    if ((exs & want) != want) qCall("SetWindowLongW", new object[] { qSt.Handle, -20, exs | want });
                    using (Graphics sg = Graphics.FromHwnd(IntPtr.Zero))
                    {
                        IntPtr sdc = sg.GetHdc();
                        IntPtr mem = (IntPtr)qCall("CreateCompatibleDC", new object[] { sdc });
                        IntPtr hbmp = bmp.GetHbitmap(Color.FromArgb(0));           // 32bpp premultiplied
                        IntPtr old = (IntPtr)qCall("SelectObject", new object[] { mem, hbmp });
                        Point c = qStCentre();
                        object[] a = { qSt.Handle, sdc, new Point(c.X - qStS / 2, c.Y - qStS / 2), new Size(qStS, qStS),
                                       mem, new Point(0, 0), 0, 0x01FF0000, 2 };      // AC_SRC_OVER, alpha 255, AC_SRC_ALPHA; ULW_ALPHA
                        qCall("UpdateLayeredWindow", a);
                        qCall("SelectObject", new object[] { mem, old });
                        qCall("DeleteObject", new object[] { hbmp });
                        qCall("DeleteDC", new object[] { mem });
                        sg.ReleaseHdc(sdc);
                    }
                }
            }
            catch { }
        };
        Action qStSync = () =>
        {
            if (!qSticker || qSt == null || qSt.IsDisposed) return;
            try
            {
                bool want = Visible && !IsDisposed && WindowState != FormWindowState.Minimized;
                IntPtr h = qSt.Handle;
                if (want)
                {
                    qStRender();
                    if (!qStShown) { qCall("ShowWindow", new object[] { h, 4 }); qStShown = true; }   // SW_SHOWNOACTIVATE
                }
                else if (qStShown) { qCall("ShowWindow", new object[] { h, 0 }); qStShown = false; qStState = 0; }
            }
            catch { }
        };
        if (qSticker)
        {
            try
            {
                qSt = new Form
                {
                    FormBorderStyle = FormBorderStyle.None,
                    ShowInTaskbar = false,
                    StartPosition = FormStartPosition.Manual,
                    ControlBox = false, MinimizeBox = false, MaximizeBox = false,
                    Text = "",
                    TopMost = true,
                    Cursor = Cursors.Hand,
                    Size = new Size(qStS, qStS)
                };
                qSt.Owner = this;                                                // stays above the popup, goes with it
                qSt.HandleCreated += (s, he) =>
                {
                    try
                    {
                        int ex = (int)qCall("GetWindowLongW", new object[] { qSt.Handle, -20 });
                        qCall("SetWindowLongW", new object[] { qSt.Handle, -20, ex | 0x80000 | 0x80 | 0x8000000 });   // LAYERED | TOOLWINDOW | NOACTIVATE
                    }
                    catch { }
                };
                Func<Point, bool> onDisc = p =>
                {
                    float dx = p.X - qStS / 2f, dy = p.Y - (qStS / 2f - 1.5f * qDpiK);
                    return dx * dx + dy * dy <= (qStR + 2f) * (qStR + 2f);
                };
                qSt.MouseMove += (s, me) => { int ns = onDisc(me.Location) ? (me.Button == MouseButtons.Left ? 2 : 1) : 0; if (ns != qStState) { qStState = ns; qStRender(); } };
                qSt.MouseLeave += (s, me) => { if (qStState != 0) { qStState = 0; qStRender(); } };
                qSt.MouseDown += (s, me) => { if (me.Button == MouseButtons.Left && onDisc(me.Location)) { qStState = 2; qStRender(); } };
                qSt.MouseUp += (s, me) => { if (me.Button == MouseButtons.Left && onDisc(me.Location)) qUserClose(); };
                IntPtr forceHandle = qSt.Handle;                                 // create now (ex-style applied)
                // follow the popup: position, visibility; destroyed with it
                Move += (s, e2) => qStSync();
                Resize += (s, e2) => qStSync();
                VisibleChanged += (s, e2) => qStSync();
                Shown += (s, e2) => qStSync();
                Disposed += (s, e2) => { try { if (qSt != null && !qSt.IsDisposed) qSt.Dispose(); } catch { } };
            }
            catch { qSticker = false; if (qSt != null) { try { qSt.Dispose(); } catch { } qSt = null; } }
        }

        // ---------------- polling fallback (works even while a modal dialog disables our windows) ----------------
        qTimer.Tick += (s, te) =>
        {
            if (IsDisposed || !Visible || xBtn.IsDisposed) { xPrevDown = false; xArmed = false; return; }
            Point ms = Control.MousePosition;                                   // screen pixels
            PointF bc; float br;
            if (qSticker && qSt != null && !qSt.IsDisposed) { Point c = qStCentre(); bc = new PointF(c.X, c.Y - 1.5f * qDpiK); br = qStR; }
            else { PointF lc = qBadgeCentre(); Point sp = PointToScreen(Point.Empty); bc = new PointF(sp.X + lc.X, sp.Y + lc.Y); br = qBadgeR; }
            float ddx = ms.X - bc.X, ddy = ms.Y - bc.Y;
            bool inside = ddx * ddx + ddy * ddy <= (br + 2f) * (br + 2f);       // circle hit-test
            bool down = (Control.MouseButtons & MouseButtons.Left) != 0;
            if (down && !xPrevDown) xArmed = inside;                             // press must start on the X
            if (!down && xPrevDown && xArmed && inside) { xArmed = false; xPrevDown = false; qUserClose(); return; }
            if (!down) xArmed = false;
            xPrevDown = down;
            int ns = inside ? (down && xArmed ? 2 : 1) : 0;
            if (qSticker && qSt != null) { if (ns != qStState) { qStState = ns; qStRender(); } }
            else if (ns != xState) { xState = ns; xBtn.Invalidate(); }
        };
        Controls.Add(xBtn);
        xBtn.BringToFront();

        // ---------------- adaptive horizontal layout ----------------
        // slim header | QR card (square, as tall as possible) on the left | info panel on the right
        Action qLayout = () =>
        {
            if (IsDisposed) return;
            int W = ClientSize.Width, H = ClientSize.Height;
            int hdr = qHdrH; const int top = 3, bottom = 2, pad = 6; // header holds the badge; minimal clean gaps
            int side = Math.Max(100, H - hdr - top - bottom);         // white QR card = full available height
            side = Math.Min(side, Math.Max(100, W - pad * 3 - 140));   // info panel keeps >= ~140px
            pictureBox1.SetBounds(pad + 3, hdr + top + 3, side - 6, side - 6);
            int ix = pad + side + pad;
            label1.SetBounds(ix + 8, hdr + top + 6, Math.Max(60, W - ix - pad - 14), side - 12);
            if (qSticker) xBtn.Visible = false;                         // the corner sticker replaces the in-header badge
            else { xBtn.Visible = true; xBtn.Bounds = qBadgeBox(); }      // fallback: corner patch badge in the header
            xBtn.BringToFront();
            Invalidate();
        };
        qLayout();
        Resize += (s, re) => qLayout();

        // ---------------- sit exactly on the window behind ----------------
        // method_0 places this form at the covered window's location; once shown, take that
        // window's exact bounds (Owner first, else the smallest visible form under us).
        bool qMatching = false;
        Action qMatch = () =>
        {
            if (qMatching || IsDisposed || !Visible) return;
            qMatching = true;
            try
            {
                Form target = (Owner != null && Owner.Visible) ? Owner : null;
                if (target == null)
                {
                    Point loc = Location;
                    Point ctr = new Point(Left + Width / 2, Top + Height / 2);
                    long best = long.MaxValue;
                    foreach (Form f in Application.OpenForms)
                    {
                        if (f == this || f.IsDisposed || !f.Visible || f.WindowState == FormWindowState.Minimized) continue;
                        Rectangle b = f.Bounds;
                        if (b.Width < 200 || b.Height < 150) continue;
                        if (b.Location == loc) { target = f; break; }
                        if (b.Contains(loc) || b.Contains(ctr))
                        {
                            long a = (long)b.Width * b.Height;
                            if (a < best) { best = a; target = f; }
                        }
                    }
                }
                if (target != null && Bounds != target.Bounds) Bounds = target.Bounds;
            }
            catch { }
            finally { qMatching = false; }
        };
        Shown += (s, se) => qMatch();
        Activated += (s, ae) => qMatch();

        // Paint custom Express UI
        Paint += ByteNovaPaint;

        ResumeLayout(false);
        Invalidate(true);
    }

    // =========================================================
    // CUSTOM EXPRESS PAINT
    // =========================================================
    private void ByteNovaPaint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        Color bg = Color.FromArgb(236, 242, 251);
        Color orange = Color.FromArgb(245, 146, 28);
        Color navy = Color.FromArgb(15, 42, 92);
        Color muted = Color.FromArgb(100, 116, 139);
        int W = ClientSize.Width, H = ClientSize.Height;

        g.Clear(bg);

        Func<RectangleF, float, GraphicsPath> round = (r, rad) =>
        {
            GraphicsPath p = new GraphicsPath();
            float d = Math.Min(rad * 2, Math.Min(r.Width, r.Height));
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        };
        Action<RectangleF, float, Color, Color> card = (r, rad, fill, line) =>
        {
            using (GraphicsPath sp = round(new RectangleF(r.X, r.Y + 2, r.Width, r.Height), rad))
            using (SolidBrush sb = new SolidBrush(Color.FromArgb(40, 30, 64, 120)))
                g.FillPath(sb, sp);
            using (GraphicsPath p = round(r, rad))
            {
                using (SolidBrush fb = new SolidBrush(fill)) g.FillPath(fb, p);
                if (line.A > 0) using (Pen pen = new Pen(line)) g.DrawPath(pen, p);
            }
        };

        // ---------------- slim header ribbon (bhagwa + navy underline) ----------------
        float hk = 1f;
        try { using (Graphics sg = Graphics.FromHwnd(IntPtr.Zero)) hk = Math.Max(1f, sg.DpiX / 96f); } catch { }
        int hdrH = Math.Max(20, pictureBox1.Top - 6);                  // same header height the layout used
        Rectangle header = new Rectangle(0, 0, W, hdrH);
        using (LinearGradientBrush hb = new LinearGradientBrush(header,
            Color.FromArgb(214, 96, 4), Color.FromArgb(248, 158, 44), LinearGradientMode.Horizontal))
            g.FillRectangle(hb, header);
        using (SolidBrush gloss = new SolidBrush(Color.FromArgb(22, 255, 255, 255)))
            g.FillRectangle(gloss, 0, 0, W, hdrH / 2);
        using (SolidBrush ul = new SolidBrush(navy))
            g.FillRectangle(ul, 0, hdrH - 3, W, 3);

        // QR badge (Lucide "qr-code"-ish) + title
        RectangleF badge = new RectangleF(7, (hdrH - 3 - 18) / 2f, 18, 18);
        using (SolidBrush wb = new SolidBrush(Color.White)) g.FillEllipse(wb, badge);
        using (SolidBrush ib = new SolidBrush(Color.FromArgb(214, 96, 4)))
        {
            float bx = badge.X + 4.5f, by = badge.Y + 4.5f;
            g.FillRectangle(ib, bx, by, 3.5f, 3.5f); g.FillRectangle(ib, bx + 5.5f, by, 3.5f, 3.5f);
            g.FillRectangle(ib, bx, by + 5.5f, 3.5f, 3.5f); g.FillRectangle(ib, bx + 6, by + 6, 2.5f, 2.5f);
        }
        using (Font tf = new Font("Segoe UI Semibold", 9.5f * 4f / 3f, FontStyle.Regular, GraphicsUnit.Pixel))
            TextRenderer.DrawText(g, "Payment QR", tf, new Rectangle(31, 0, W - 110, hdrH - 3), Color.White,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);

        // UPI pill on the right of the header
        using (Font pf = new Font("Segoe UI", (7.5f) * 4f / 3f, FontStyle.Bold, GraphicsUnit.Pixel))
        {
            Size ps = TextRenderer.MeasureText(g, "UPI", pf, Size.Empty, TextFormatFlags.NoPadding);
            float bk = 1f;
            try { using (Graphics sg = Graphics.FromHwnd(IntPtr.Zero)) bk = Math.Max(1f, sg.DpiX / 96f); } catch { }
            bool headerBadge = false;
            foreach (Control hc in Controls) if (hc is PictureBox && hc != pictureBox1 && hc.Visible) headerBadge = true;
            float pillRight = headerBadge ? W - 3f - 30f * bk - 10f : W - 30f * bk;   // clear of the in-header badge, or of the corner sticker
            RectangleF pill = new RectangleF(pillRight - (ps.Width + 16), (hdrH - 3 - 16) / 2f, ps.Width + 16, 16);
            using (GraphicsPath pp = round(pill, 8f))
            {
                using (SolidBrush b = new SolidBrush(Color.FromArgb(48, 255, 255, 255))) g.FillPath(b, pp);
                using (Pen p = new Pen(Color.FromArgb(110, 255, 255, 255))) g.DrawPath(p, pp);
            }
            TextRenderer.DrawText(g, "UPI", pf, Rectangle.Round(pill), Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        // ---------------- QR card (frames pictureBox1, left) ----------------
        Rectangle pbR = pictureBox1.Bounds;
        RectangleF qrCard = new RectangleF(pbR.X - 3, pbR.Y - 3, pbR.Width + 6, pbR.Height + 6);
        card(qrCard, 12, Color.White, Color.FromArgb(220, 228, 241));

        // bhagwa corner brackets hugging the card corners, inside its 3px white padding ring.
        // The code is centred with >= 4 modules of white to the card edge, so a <= 3px bracket
        // stays >= 3 modules away from it (modules are >= 3px).
        {
            float inset = 1.5f, rr = 12f - inset;
            RectangleF c = RectangleF.Inflate(qrCard, -inset, -inset);
            float bl = Math.Max(16f, Math.Min(34f, c.Width / 7f));
            using (Pen bp = new Pen(orange, 2.4F))
            {
                bp.StartCap = LineCap.Round; bp.EndCap = LineCap.Round; bp.LineJoin = LineJoin.Round;
                using (GraphicsPath p = new GraphicsPath())
                {
                    p.AddLine(c.Left, c.Top + bl, c.Left, c.Top + rr); p.AddArc(c.Left, c.Top, rr * 2, rr * 2, 180, 90); p.AddLine(c.Left + rr, c.Top, c.Left + bl, c.Top);
                    g.DrawPath(bp, p);
                }
                using (GraphicsPath p = new GraphicsPath())
                {
                    p.AddLine(c.Right - bl, c.Top, c.Right - rr, c.Top); p.AddArc(c.Right - rr * 2, c.Top, rr * 2, rr * 2, 270, 90); p.AddLine(c.Right, c.Top + rr, c.Right, c.Top + bl);
                    g.DrawPath(bp, p);
                }
                using (GraphicsPath p = new GraphicsPath())
                {
                    p.AddLine(c.Right, c.Bottom - bl, c.Right, c.Bottom - rr); p.AddArc(c.Right - rr * 2, c.Bottom - rr * 2, rr * 2, rr * 2, 0, 90); p.AddLine(c.Right - rr, c.Bottom, c.Right - bl, c.Bottom);
                    g.DrawPath(bp, p);
                }
                using (GraphicsPath p = new GraphicsPath())
                {
                    p.AddLine(c.Left + bl, c.Bottom, c.Left + rr, c.Bottom); p.AddArc(c.Left, c.Bottom - rr * 2, rr * 2, rr * 2, 90, 90); p.AddLine(c.Left, c.Bottom - rr, c.Left, c.Bottom - bl);
                    g.DrawPath(bp, p);
                }
            }
        }

        // ---------------- info card (frames label1, right) ----------------
        Rectangle lbR = label1.Bounds;
        RectangleF info = new RectangleF(lbR.X - 8, qrCard.Y, lbR.Width + 14, qrCard.Height);
        card(info, 12, Color.White, Color.FromArgb(253, 186, 116));
        using (SolidBrush acc = new SolidBrush(orange))
            g.FillRectangle(acc, info.X + 1, info.Y + 14, 3, info.Height - 28);

        // smooth outer border (masks the rounded window edge)
        using (Pen fr = new Pen(Color.FromArgb(178, 196, 226), 1.4F))
        using (GraphicsPath op = round(new RectangleF(0.7f, 0.7f, W - 1.4f, H - 1.4f), 10))
            g.DrawPath(fr, op);

        _ = muted;
    }

    // =========================================================
    // ROUNDED RECTANGLE
    // =========================================================
    private GraphicsPath RoundedRect(Rectangle r, int radius)
    {
        GraphicsPath p = new GraphicsPath();

        int d = radius * 2;

        p.AddArc(
            r.Left,
            r.Top,
            d,
            d,
            180,
            90);

        p.AddArc(
            r.Right - d,
            r.Top,
            d,
            d,
            270,
            90);

        p.AddArc(
            r.Right - d,
            r.Bottom - d,
            d,
            d,
            0,
            90);

        p.AddArc(
            r.Left,
            r.Bottom - d,
            d,
            d,
            90,
            90);

        p.CloseFigure();

        return p;
    }

    // =========================================================
    // LOAD BASE64 QR IMAGE
    // =========================================================
    public Image LoadImage(string strin_1)
    {
        using MemoryStream stream =
            new MemoryStream(Convert.FromBase64String(strin_1));

        return Image.FromStream(stream);
    }

    // =========================================================
    // MAKE SURE FORM IS VISIBLE ON SCREEN
    // =========================================================
    public static void EnsureShowsTaskbar(Form form)
    {
        if (form != null && !form.IsDisposed)
        {
            Screen screen = Screen.FromControl(form);

            Rectangle bounds = screen.Bounds;
            Rectangle workingArea = screen.WorkingArea;
            Rectangle bounds2 = form.Bounds;

            int val = bounds2.Left;
            int val2 = bounds2.Top;

            if (bounds2.Left < workingArea.Left)
            {
                val = workingArea.Left;
            }

            if (bounds2.Right > workingArea.Right)
            {
                val = Math.Max(
                    workingArea.Left,
                    bounds2.Left -
                    (bounds2.Right - workingArea.Right));
            }

            if (bounds2.Top < workingArea.Top)
            {
                val2 = workingArea.Top;
            }

            if (bounds2.Bottom > workingArea.Bottom)
            {
                val2 = Math.Max(
                    workingArea.Top,
                    bounds2.Top -
                    (bounds2.Bottom - workingArea.Bottom));
            }

            val = Math.Min(
                Math.Max(val, bounds.Left),
                bounds.Right - bounds2.Width);

            val2 = Math.Min(
                Math.Max(val2, bounds.Top),
                bounds.Bottom - bounds2.Height);

            if (val != bounds2.Left ||
                val2 != bounds2.Top)
            {
                form.Location = new Point(val, val2);
            }
        }
    }

    // =========================================================
    // SHOW QR FORM
    // =========================================================
    internal void method_0(
        Control control_0,
        Rectangle rectangle_0)
    {
        try
        {
            _ = Screen.PrimaryScreen.WorkingArea;
            _ = control_0.Controls[0];

            Control control = control_0.Parent;

            StartPosition = FormStartPosition.Manual;

            Location = control.Location;

            EnsureShowsTaskbar(this);

            _ = control.Location.Y;

            SetWindowPos(
                Handle,
                intptr_0,
                0,
                0,
                0,
                0,
                3u);

            _ = Top;
            _ = Height;

            TopMost = true;

            Show();

            pictureBox1.BringToFront();

            Activate();
        }
        catch (Exception)
        {
        }
    }

    // =========================================================
    // RECEIVE QR DATA
    // =========================================================
    internal void method_1(object string_5)
    {
        string_3 = string_5.ToString();

        string text = string_3;

        string[] array = new string[1];

        string text2 = "|||";

        array[0] = text2;

        string[] array2 =
            text.Split(array, StringSplitOptions.None);

        pictureBox1.Image = LoadImage(array2[0]);

        Label label = label1;

        string text3 = "Pay Using QR :- ";
        string text4 = array2[1];
        string text5 = "  Amt=  ";

        label.Text =
            text3 +
            text4 +
            text5 +
            array2[2];

        method_0(
            clsMain.control_0,
            clsMain.rectangle_0);
    }

    // =========================================================
    // DISPOSE
    // =========================================================
    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    // =========================================================
    // INITIALIZE COMPONENT
    // =========================================================
    private void InitializeComponent()
    {
        this.components =
            new System.ComponentModel.Container();

        this.bunifuElipse1 =
            new Bunifu.Framework.UI.BunifuElipse(
                this.components);

        this.pictureBox1 =
            new System.Windows.Forms.PictureBox();

        this.label1 =
            new System.Windows.Forms.Label();

        ((System.ComponentModel.ISupportInitialize)
            (this.pictureBox1)).BeginInit();

        this.SuspendLayout();

        // 
        // bunifuElipse1
        // 
        this.bunifuElipse1.ElipseRadius = 5;
        this.bunifuElipse1.TargetControl = this;

        // 
        // pictureBox1
        // 
        this.pictureBox1.BackColor =
            System.Drawing.SystemColors.ButtonHighlight;

        this.pictureBox1.BorderStyle =
            System.Windows.Forms.BorderStyle.FixedSingle;

        this.pictureBox1.Dock =
            System.Windows.Forms.DockStyle.Top;

        this.pictureBox1.Location =
            new System.Drawing.Point(0, 0);

        this.pictureBox1.Name = "pictureBox1";

        this.pictureBox1.Size =
            new System.Drawing.Size(221, 221);

        this.pictureBox1.SizeMode =
            System.Windows.Forms.PictureBoxSizeMode.Zoom;

        this.pictureBox1.TabIndex = 1217;
        this.pictureBox1.TabStop = false;

        // 
        // label1
        // 
        this.label1.AutoSize = true;

        this.label1.Location =
            new System.Drawing.Point(12, 235);

        this.label1.Name = "label1";

        this.label1.Size =
            new System.Drawing.Size(35, 13);

        this.label1.TabIndex = 1218;

        this.label1.Text = "label1";

        // 
        // frmShowQR
        // 
        this.AutoScaleDimensions =
            new System.Drawing.SizeF(6F, 13F);

        this.AutoScaleMode =
            System.Windows.Forms.AutoScaleMode.Font;

        // Designer size.
        // ApplyByteNovaTheme() will finally make it 265 x 300.
        this.ClientSize =
            new System.Drawing.Size(265, 300);

        this.Controls.Add(this.label1);
        this.Controls.Add(this.pictureBox1);

        this.FormBorderStyle =
            System.Windows.Forms.FormBorderStyle.None;

        this.Name = "frmShowQR";

        this.Text = "Payment QR";

        this.TopMost = true;

        ((System.ComponentModel.ISupportInitialize)
            (this.pictureBox1)).EndInit();

        this.ResumeLayout(false);
        this.PerformLayout();
    }
}