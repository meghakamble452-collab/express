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
                using (Font lf = new Font("Segoe UI", 9F)) TextRenderer.DrawText(g, "Loading QR...", lf, code, qMuted,
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

        // ---------------- payment info (right-hand panel) ----------------
        // label1.Text is set by the logic ("Pay Using QR :- <upi>  Amt=  <amount>");
        // the label paints it as a vertical info panel: Scan & Pay / PAY TO / AMOUNT / hint.
        label1.AutoSize = false;
        label1.BackColor = Color.White;
        label1.ForeColor = qText;
        label1.Font = new Font("Segoe UI Semibold", 9F);
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
            TextFormatFlags one = TextFormatFlags.Left | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix;

            Func<string, float, FontStyle, int, float, Font> fit = (text, start, style, maxW, minSz) =>
            {
                float sz = start;
                while (sz > minSz)
                {
                    using (Font f = new Font("Segoe UI", sz, style))
                        if (TextRenderer.MeasureText(g, text, f, Size.Empty, TextFormatFlags.NoPadding).Width <= maxW) break;
                    sz -= 0.5f;
                }
                return new Font("Segoe UI", sz, style);
            };
            // character wrap (UPI ids have no spaces); prefers breaking before "@"
            Func<string, Font, int, int, System.Collections.Generic.List<string>> wrap = (text, f, maxW, maxLines) =>
            {
                var lines = new System.Collections.Generic.List<string>();
                string rest = text;
                while (rest.Length > 0 && lines.Count < maxLines)
                {
                    int cut = rest.Length;
                    while (cut > 1 && TextRenderer.MeasureText(g, rest.Substring(0, cut), f, Size.Empty, TextFormatFlags.NoPadding).Width > maxW) cut--;
                    if (cut < rest.Length)
                    {
                        int at = rest.LastIndexOf('@', Math.Max(0, cut - 1));
                        if (at > cut / 2) cut = at;
                    }
                    lines.Add(rest.Substring(0, cut));
                    rest = rest.Substring(cut);
                }
                if (rest.Length > 0 && lines.Count > 0) lines[lines.Count - 1] += rest;   // drawn with EndEllipsis as a last resort
                return lines;
            };

            int y = 4;
            // title row: scan icon + "Scan & Pay"
            using (Pen ip = new Pen(Color.FromArgb(214, 96, 4), 1.8F))
            {
                ip.StartCap = LineCap.Round; ip.EndCap = LineCap.Round; ip.LineJoin = LineJoin.Round;
                float ix = x0, iy = y + 3, sz = 14, c = 4.5f;
                g.DrawLines(ip, new[] { new PointF(ix, iy + c), new PointF(ix, iy), new PointF(ix + c, iy) });
                g.DrawLines(ip, new[] { new PointF(ix + sz - c, iy), new PointF(ix + sz, iy), new PointF(ix + sz, iy + c) });
                g.DrawLines(ip, new[] { new PointF(ix + sz, iy + sz - c), new PointF(ix + sz, iy + sz), new PointF(ix + sz - c, iy + sz) });
                g.DrawLines(ip, new[] { new PointF(ix + c, iy + sz), new PointF(ix, iy + sz), new PointF(ix, iy + sz - c) });
                g.DrawLine(ip, ix + 2.5f, iy + sz / 2, ix + sz - 2.5f, iy + sz / 2);
            }
            using (Font tf = fit("Scan & Pay", 12F, FontStyle.Bold, cw - 22, 9F))
                TextRenderer.DrawText(g, "Scan & Pay", tf, new Rectangle(x0 + 22, y, cw - 22, 22), qNavy, one | TextFormatFlags.VerticalCenter);
            y += 24;
            using (Font hf = new Font("Segoe UI", 7.75F))
            {
                string hint = "Open any UPI app and scan the QR";
                TextFormatFlags wb = TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix;
                int hh = Math.Min(30, TextRenderer.MeasureText(g, hint, hf, new Size(cw, 0), wb).Height);
                TextRenderer.DrawText(g, hint, hf, new Rectangle(x0, y, cw, hh + 1), qMuted, wb);
                y += hh;
            }
            y += 4;
            using (Pen sep = new Pen(Color.FromArgb(226, 232, 240)))
                g.DrawLine(sep, x0, y, x0 + cw, y);
            y += 7;

            if (ai < 0)
            {
                // unknown text format: show it whole, wrapped
                using (Font f = new Font("Segoe UI Semibold", 9F))
                    TextRenderer.DrawText(g, raw, f, new Rectangle(x0, y, cw, H - y - 4), qText,
                        TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix);
                return;
            }

            string left = raw.Substring(0, ai);
            int ci = left.IndexOf(":-");
            string payTo = (ci >= 0 ? left.Substring(ci + 2) : left).Trim();
            string amtText = ((char)0x20B9).ToString() + " " + raw.Substring(ai + 4).Trim();   // rupee sign

            using (Font cap = new Font("Segoe UI", 7F, FontStyle.Bold))
            {
                // AMOUNT block is anchored to the bottom so it is always fully visible
                int amtH;
                using (Font af = fit(amtText, 20F, FontStyle.Bold, cw, 9F))
                {
                    amtH = TextRenderer.MeasureText(g, amtText, af, Size.Empty, TextFormatFlags.NoPadding).Height;
                    int ay = H - 4 - amtH;
                    TextRenderer.DrawText(g, "AMOUNT", cap, new Rectangle(x0, ay - 15, cw, 13), qMuted, one);
                    TextRenderer.DrawText(g, amtText, af, new Rectangle(x0, ay, cw, amtH + 2), Color.FromArgb(214, 96, 4), one);
                    amtH += 15;
                }

                // PAY TO fills the space between the divider and the amount
                TextRenderer.DrawText(g, "PAY TO", cap, new Rectangle(x0, y, cw, 13), qMuted, one);
                y += 15;
                int room = Math.Max(14, H - 4 - amtH - 6 - y);
                using (Font vf = fit(payTo, 10F, FontStyle.Bold, cw, 8F))
                {
                    if (TextRenderer.MeasureText(g, payTo, vf, Size.Empty, TextFormatFlags.NoPadding).Width <= cw)
                        TextRenderer.DrawText(g, payTo, vf, new Rectangle(x0, y, cw, 20), qNavy, one);
                    else
                        using (Font wf = new Font("Segoe UI", 8.5F, FontStyle.Bold))
                        {
                            int lh = TextRenderer.MeasureText(g, "Ag", wf, Size.Empty, TextFormatFlags.NoPadding).Height + 1;
                            var lines = wrap(payTo, wf, cw, Math.Max(1, Math.Min(4, room / lh)));
                            foreach (string l in lines)
                            {
                                TextRenderer.DrawText(g, l, wf, new Rectangle(x0, y, cw, lh), qNavy, one | TextFormatFlags.EndEllipsis);
                                y += lh;
                            }
                        }
                }
            }
        };

        // ---------------- close (X) button in the header ----------------
        // Always visible (harmless). It HIDES the popup instead of closing it: the owner code
        // keeps this instance and calls method_1 -> method_0 -> Show() for the next QR, so a
        // disposed form would make the next QR silently never appear.
        int xState = 0;                                     // 0 normal, 1 hover, 2 pressed
        PictureBox xBtn = new PictureBox                    // PictureBox is double-buffered: no flicker
        {
            Size = new Size(26, 20),
            BackColor = Color.FromArgb(248, 158, 44),
            Cursor = Cursors.Hand,
            TabStop = false
        };
        xBtn.Paint += (s, xe) =>
        {
            Graphics g = xe.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            int fw = ClientSize.Width;
            // repaint the header gradient + gloss underneath so the button blends into the ribbon
            using (LinearGradientBrush hb = new LinearGradientBrush(new Rectangle(-xBtn.Left, -xBtn.Top, Math.Max(1, fw), 26),
                Color.FromArgb(214, 96, 4), Color.FromArgb(248, 158, 44), LinearGradientMode.Horizontal))
                g.FillRectangle(hb, 0, 0, xBtn.Width, xBtn.Height);
            using (SolidBrush gloss = new SolidBrush(Color.FromArgb(22, 255, 255, 255)))
                g.FillRectangle(gloss, 0, 0, xBtn.Width, Math.Max(0, 13 - xBtn.Top));

            RectangleF r = new RectangleF(0.5f, xState == 2 ? 1.5f : 0.5f, xBtn.Width - 1.5f, xBtn.Height - 2f);
            Color fill = xState == 2 ? Color.FromArgb(153, 27, 27) : xState == 1 ? Color.White : Color.FromArgb(220, 38, 38);
            Color line = xState == 2 ? Color.FromArgb(127, 29, 29) : xState == 1 ? Color.FromArgb(220, 38, 38) : Color.FromArgb(185, 28, 28);
            Color glyph = xState == 1 ? Color.FromArgb(220, 38, 38) : Color.White;
            float rad = 5f, d = rad * 2;
            using (GraphicsPath p = new GraphicsPath())
            {
                p.AddArc(r.X, r.Y, d, d, 180, 90);
                p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
                p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
                p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
                p.CloseFigure();
                using (SolidBrush fb = new SolidBrush(fill)) g.FillPath(fb, p);
                using (Pen lp = new Pen(line, 1F)) g.DrawPath(lp, p);
            }
            float cx = r.X + r.Width / 2f, cy = r.Y + r.Height / 2f, k = 4f;
            using (Pen xp = new Pen(glyph, 2F))
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
        xBtn.Click += (s, xe) =>
        {
            xState = 0;
            Hide();                                         // the next QR (method_0 -> Show) brings it back
        };
        Controls.Add(xBtn);
        xBtn.BringToFront();

        // ---------------- adaptive horizontal layout ----------------
        // slim header | QR card (square, as tall as possible) on the left | info panel on the right
        Action qLayout = () =>
        {
            if (IsDisposed) return;
            int W = ClientSize.Width, H = ClientSize.Height;
            const int hdr = 26, top = 3, bottom = 2, pad = 6;       // minimal clean gaps
            int side = Math.Max(100, H - hdr - top - bottom);         // white QR card = full available height
            side = Math.Min(side, Math.Max(100, W - pad * 3 - 140));   // info panel keeps >= ~140px
            pictureBox1.SetBounds(pad + 3, hdr + top + 3, side - 6, side - 6);
            int ix = pad + side + pad;
            label1.SetBounds(ix + 8, hdr + top + 6, Math.Max(60, W - ix - pad - 14), side - 12);
            xBtn.SetBounds(W - 26 - 5, (hdr - 3 - 20) / 2 + 1, 26, 20);   // right end of the header
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
        const int hdrH = 26;
        Rectangle header = new Rectangle(0, 0, W, hdrH);
        using (LinearGradientBrush hb = new LinearGradientBrush(header,
            Color.FromArgb(214, 96, 4), Color.FromArgb(248, 158, 44), LinearGradientMode.Horizontal))
            g.FillRectangle(hb, header);
        using (SolidBrush gloss = new SolidBrush(Color.FromArgb(22, 255, 255, 255)))
            g.FillRectangle(gloss, 0, 0, W, hdrH / 2);
        using (SolidBrush ul = new SolidBrush(navy))
            g.FillRectangle(ul, 0, hdrH - 3, W, 3);

        // QR badge (Lucide "qr-code"-ish) + title
        RectangleF badge = new RectangleF(7, 3, 18, 18);
        using (SolidBrush wb = new SolidBrush(Color.White)) g.FillEllipse(wb, badge);
        using (SolidBrush ib = new SolidBrush(Color.FromArgb(214, 96, 4)))
        {
            float bx = badge.X + 4.5f, by = badge.Y + 4.5f;
            g.FillRectangle(ib, bx, by, 3.5f, 3.5f); g.FillRectangle(ib, bx + 5.5f, by, 3.5f, 3.5f);
            g.FillRectangle(ib, bx, by + 5.5f, 3.5f, 3.5f); g.FillRectangle(ib, bx + 6, by + 6, 2.5f, 2.5f);
        }
        using (Font tf = new Font("Segoe UI Semibold", 9.5F))
            TextRenderer.DrawText(g, "Payment QR", tf, new Rectangle(31, 0, W - 110, hdrH - 3), Color.White,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);

        // UPI pill on the right of the header
        using (Font pf = new Font("Segoe UI", 7.5F, FontStyle.Bold))
        {
            Size ps = TextRenderer.MeasureText(g, "UPI", pf, Size.Empty, TextFormatFlags.NoPadding);
            RectangleF pill = new RectangleF(W - ps.Width - 24 - 32, (hdrH - 3 - 16) / 2f, ps.Width + 16, 16);
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