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
        ClientSize = new Size(320, 422);
        bunifuElipse1.ElipseRadius = 12;

        // ---------------- QR area ----------------
        // The PictureBox covers the QR card's inner square. Its own image drawing is
        // replaced in the Paint handler below by a pixel-perfect, nearest-neighbour render.
        pictureBox1.Dock = DockStyle.None;
        pictureBox1.SetBounds(22, 64, 276, 276);
        pictureBox1.BackColor = Color.White;
        pictureBox1.BorderStyle = BorderStyle.None;
        pictureBox1.SizeMode = PictureBoxSizeMode.Normal;   // no bilinear scaling by the PictureBox

        Image qSrc = null;          // image the cache was built from
        Bitmap qCrisp = null;       // rebuilt QR: 1 colour per module, integer scale
        int qPhase = 0;             // scan-marker animation phase

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
                while (minX + run <= maxX && dark(minX + run, minY)) run++;
                double mod = run / 7.0;
                if (mod < 1) return null;
                double bw = maxX - minX + 1, bh = maxY - minY + 1;
                int n = (int)Math.Round(bw / mod);
                if (n < 21 || n > 177 || (n - 17) % 4 != 0) return null;
                if (Math.Abs(bw / mod - n) > 0.45 || Math.Abs(bh / mod - n) > 0.6) return null;

                int total = n + 8;                                   // quiet zone: 4 modules each side
                int scale = Math.Max(1, avail / total);
                Bitmap outB = new Bitmap(total * scale, total * scale, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics og = Graphics.FromImage(outB))
                {
                    og.Clear(Color.White);
                    og.SmoothingMode = SmoothingMode.None;
                    double mh = bh / n;
                    for (int j = 0; j < n; j++)
                        for (int i = 0; i < n; i++)
                        {
                            int sx = Math.Min(w - 1, (int)(minX + (i + 0.5) * mod));
                            int sy = Math.Min(h - 1, (int)(minY + (j + 0.5) * mh));
                            if (dark(sx, sy))
                                og.FillRectangle(Brushes.Black, (i + 4) * scale, (j + 4) * scale, scale, scale);
                        }
                }
                return outB;
            }
        };

        pictureBox1.Paint += (s, pe) =>
        {
            Graphics g = pe.Graphics;
            int W = pictureBox1.Width, H = pictureBox1.Height;
            g.Clear(Color.White);

            const int frame = 14;                                     // room for brackets / scan markers
            Rectangle zone = new Rectangle(frame, frame, W - frame * 2, H - frame * 2);
            int avail = Math.Min(zone.Width, zone.Height);

            Image img = pictureBox1.Image;
            if (!ReferenceEquals(img, qSrc))
            {
                if (qCrisp != null) { qCrisp.Dispose(); qCrisp = null; }
                qSrc = img;
                try { qCrisp = buildCrisp(img, avail); } catch { qCrisp = null; }
            }

            Rectangle qr;
            g.SmoothingMode = SmoothingMode.None;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            if (qCrisp != null)
            {
                qr = new Rectangle(zone.X + (zone.Width - qCrisp.Width) / 2, zone.Y + (zone.Height - qCrisp.Height) / 2,
                    qCrisp.Width, qCrisp.Height);
                g.DrawImageUnscaled(qCrisp, qr.X, qr.Y);
            }
            else if (img != null)
            {
                // Fallback (not a plain QR image): integer upscale if it fits, otherwise fit,
                // always nearest-neighbour so edges stay hard.
                int k = Math.Max(img.Width, img.Height) <= avail ? avail / Math.Max(img.Width, img.Height) : 0;
                Size sz = k >= 1
                    ? new Size(img.Width * k, img.Height * k)
                    : new Size((int)(img.Width * (double)avail / Math.Max(img.Width, img.Height)),
                               (int)(img.Height * (double)avail / Math.Max(img.Width, img.Height)));
                qr = new Rectangle(zone.X + (zone.Width - sz.Width) / 2, zone.Y + (zone.Height - sz.Height) / 2, sz.Width, sz.Height);
                g.DrawImage(img, qr);
            }
            else
            {
                qr = zone;
                using (Font lf = new Font("Segoe UI", 9F)) TextRenderer.DrawText(g, "Loading QR...", lf, zone, qMuted,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            // bhagwa corner brackets OUTSIDE the code (in the frame margin)
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.Default;
            float b0 = 3.5f, bl = 26f;
            RectangleF fr = new RectangleF(b0, b0, W - 1 - b0 * 2, H - 1 - b0 * 2);
            using (Pen bp = new Pen(qOrange, 3.2F))
            {
                bp.StartCap = LineCap.Round; bp.EndCap = LineCap.Round; bp.LineJoin = LineJoin.Round;
                g.DrawLines(bp, new[] { new PointF(fr.Left, fr.Top + bl), new PointF(fr.Left, fr.Top), new PointF(fr.Left + bl, fr.Top) });
                g.DrawLines(bp, new[] { new PointF(fr.Right - bl, fr.Top), new PointF(fr.Right, fr.Top), new PointF(fr.Right, fr.Top + bl) });
                g.DrawLines(bp, new[] { new PointF(fr.Right, fr.Bottom - bl), new PointF(fr.Right, fr.Bottom), new PointF(fr.Right - bl, fr.Bottom) });
                g.DrawLines(bp, new[] { new PointF(fr.Left + bl, fr.Bottom), new PointF(fr.Left, fr.Bottom), new PointF(fr.Left, fr.Bottom - bl) });
            }

            // animated scan markers that glide up and down on the LEFT and RIGHT margins only
            double t = (Math.Sin(qPhase * 0.06) + 1) / 2;                 // 0..1
            float my = (float)(fr.Top + bl + 6 + t * (fr.Height - bl * 2 - 12));
            using (LinearGradientBrush mk = new LinearGradientBrush(new RectangleF(0, my - 14, 6, 28),
                Color.FromArgb(0, qOrange), qOrange, LinearGradientMode.Vertical))
            {
                ColorBlend cb = new ColorBlend();
                cb.Positions = new[] { 0f, 0.5f, 1f };
                cb.Colors = new[] { Color.FromArgb(0, qOrange), qOrange, Color.FromArgb(0, qOrange) };
                mk.InterpolationColors = cb;
                g.FillRectangle(mk, fr.Left - 1.5f, my - 14, 3, 28);
                g.FillRectangle(mk, fr.Right - 1.5f, my - 14, 3, 28);
            }
        };

        // smooth, low-cost animation; stops when the window closes
        Timer qTimer = new Timer { Interval = 33 };
        qTimer.Tick += (s, te) =>
        {
            if (IsDisposed || !Visible) return;
            qPhase++;
            int W = pictureBox1.Width, H = pictureBox1.Height;
            pictureBox1.Invalidate(new Rectangle(0, 0, 9, H));            // only the side margins repaint
            pictureBox1.Invalidate(new Rectangle(W - 9, 0, 9, H));
        };
        qTimer.Start();
        FormClosed += (s, fe) => { qTimer.Stop(); qTimer.Dispose(); };
        Disposed += (s, de) => { qTimer.Stop(); if (qCrisp != null) { qCrisp.Dispose(); qCrisp = null; } };

        // ---------------- payment info ----------------
        // label1.Text is set by the logic ("Pay Using QR :- <upi>  Amt=  <amount>");
        // it is drawn as a clean two-column card: PAY TO | AMOUNT.
        label1.AutoSize = false;
        label1.SetBounds(22, 358, 276, 50);
        label1.BackColor = Color.White;
        label1.ForeColor = qText;
        label1.Font = new Font("Segoe UI Semibold", 9F);
        label1.TextAlign = ContentAlignment.MiddleCenter;
        label1.UseMnemonic = false;
        label1.Paint += (s, le) =>
        {
            Graphics g = le.Graphics;
            g.Clear(Color.White);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            string raw = label1.Text ?? "";
            int ai = raw.IndexOf("Amt=", StringComparison.OrdinalIgnoreCase);
            int W = label1.Width, H = label1.Height;

            Func<string, float, FontStyle, int, Font> fit = (text, start, style, maxW) =>
            {
                float sz = start;
                while (sz > 7f)
                {
                    using (Font f = new Font("Segoe UI", sz, style))
                        if (TextRenderer.MeasureText(g, text, f, Size.Empty, TextFormatFlags.NoPadding).Width <= maxW) break;
                    sz -= 0.5f;
                }
                return new Font("Segoe UI", sz, style);
            };

            if (ai < 0 || raw.Trim().Length == 0 || raw == "label1")
            {
                using (Font f = new Font("Segoe UI Semibold", 9F))
                    TextRenderer.DrawText(g, raw == "label1" ? "" : raw, f, new Rectangle(4, 0, W - 8, H), qText,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
                return;
            }

            string left = raw.Substring(0, ai);
            int ci = left.IndexOf(":-");
            string payTo = (ci >= 0 ? left.Substring(ci + 2) : left).Trim();
            string amt = raw.Substring(ai + 4).Trim();

            // amount column (right)
            string amtText = ((char)0x20B9).ToString() + " " + amt;   // rupee sign
            int amtW;
            using (Font af0 = new Font("Segoe UI", 14F, FontStyle.Bold))
                amtW = Math.Min(120, TextRenderer.MeasureText(g, amtText, af0, Size.Empty, TextFormatFlags.NoPadding).Width + 6);
            int colR = W - amtW - 4;
            using (Font cap = new Font("Segoe UI", 7F, FontStyle.Bold))
            {
                TextRenderer.DrawText(g, "PAY TO", cap, new Rectangle(8, 6, colR - 16, 14), qMuted,
                    TextFormatFlags.Left | TextFormatFlags.NoPadding);
                TextRenderer.DrawText(g, "AMOUNT", cap, new Rectangle(colR, 6, amtW, 14), qMuted,
                    TextFormatFlags.Right | TextFormatFlags.NoPadding);
            }
            // UPI ids have no spaces, so wrap by characters (max 2 lines) instead of cutting them.
            int maxW = colR - 16;
            using (Font vf = fit(payTo, 9.5f, FontStyle.Bold, maxW))
            {
                if (TextRenderer.MeasureText(g, payTo, vf, Size.Empty, TextFormatFlags.NoPadding).Width <= maxW)
                {
                    TextRenderer.DrawText(g, payTo, vf, new Rectangle(8, 21, maxW + 2, 20), qNavy,
                        TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                }
                else
                {
                    using (Font wf = new Font("Segoe UI", 8F, FontStyle.Bold))
                    {
                        int cut = payTo.Length;
                        while (cut > 1 && TextRenderer.MeasureText(g, payTo.Substring(0, cut), wf, Size.Empty, TextFormatFlags.NoPadding).Width > maxW)
                            cut--;
                        int at = payTo.LastIndexOf('@', Math.Max(0, cut - 1));
                        if (at > cut / 2) cut = at;                       // prefer breaking before "@bank"
                        string l1 = payTo.Substring(0, cut), l2 = payTo.Substring(cut);
                        TextRenderer.DrawText(g, l1, wf, new Rectangle(8, 19, maxW + 2, 15), qNavy,
                            TextFormatFlags.Left | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                        TextRenderer.DrawText(g, l2, wf, new Rectangle(8, 33, maxW + 2, 15), qNavy,
                            TextFormatFlags.Left | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
                    }
                }
            }
            using (Font af = fit(amtText, 14F, FontStyle.Bold, amtW))
                TextRenderer.DrawText(g, amtText, af, new Rectangle(colR, 18, amtW, H - 20), Color.FromArgb(214, 96, 4),
                    TextFormatFlags.Right | TextFormatFlags.Top | TextFormatFlags.NoPadding);
            using (Pen sep = new Pen(Color.FromArgb(226, 232, 240)))
                g.DrawLine(sep, colR - 6, 8, colR - 6, H - 8);
        };

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

        // ---------------- header ribbon (bhagwa + navy underline) ----------------
        Rectangle header = new Rectangle(0, 0, W, 48);
        using (LinearGradientBrush hb = new LinearGradientBrush(header,
            Color.FromArgb(214, 96, 4), Color.FromArgb(248, 158, 44), LinearGradientMode.Horizontal))
            g.FillRectangle(hb, header);
        using (SolidBrush gloss = new SolidBrush(Color.FromArgb(22, 255, 255, 255)))
            g.FillRectangle(gloss, 0, 0, W, 22);
        using (SolidBrush ul = new SolidBrush(navy))
            g.FillRectangle(ul, 0, 45, W, 3);

        // scan icon (Lucide "scan-line") on a white badge
        RectangleF badge = new RectangleF(12, 8, 30, 30);
        using (SolidBrush sh = new SolidBrush(Color.FromArgb(50, 0, 0, 0))) g.FillEllipse(sh, badge.X, badge.Y + 1, badge.Width, badge.Height);
        using (SolidBrush wb = new SolidBrush(Color.White)) g.FillEllipse(wb, badge);
        using (Pen ip = new Pen(Color.FromArgb(214, 96, 4), 1.8F))
        {
            ip.StartCap = LineCap.Round; ip.EndCap = LineCap.Round; ip.LineJoin = LineJoin.Round;
            float x = badge.X + 8, y = badge.Y + 8, s = 14, c = 4.5f;
            g.DrawLines(ip, new[] { new PointF(x, y + c), new PointF(x, y), new PointF(x + c, y) });
            g.DrawLines(ip, new[] { new PointF(x + s - c, y), new PointF(x + s, y), new PointF(x + s, y + c) });
            g.DrawLines(ip, new[] { new PointF(x + s, y + s - c), new PointF(x + s, y + s), new PointF(x + s - c, y + s) });
            g.DrawLines(ip, new[] { new PointF(x + c, y + s), new PointF(x, y + s), new PointF(x, y + s - c) });
            g.DrawLine(ip, x + 2.5f, y + s / 2, x + s - 2.5f, y + s / 2);
        }
        using (Font tf = new Font("Segoe UI Semibold", 11.5F))
            TextRenderer.DrawText(g, "Scan & Pay", tf, new Point(50, 6), Color.White, TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix);
        using (Font sf = new Font("Segoe UI", 7.75F))
            TextRenderer.DrawText(g, "Open any UPI app and scan this code", sf, new Point(51, 27),
                Color.FromArgb(255, 240, 222), TextFormatFlags.NoPadding);

        // UPI pill on the right of the header
        using (Font pf = new Font("Segoe UI", 7.5F, FontStyle.Bold))
        {
            Size ps = TextRenderer.MeasureText(g, "UPI", pf, Size.Empty, TextFormatFlags.NoPadding);
            RectangleF pill = new RectangleF(W - ps.Width - 32, 14, ps.Width + 20, 19);
            using (GraphicsPath pp = round(pill, 9.5f))
            {
                using (SolidBrush b = new SolidBrush(Color.FromArgb(48, 255, 255, 255))) g.FillPath(b, pp);
                using (Pen p = new Pen(Color.FromArgb(110, 255, 255, 255))) g.DrawPath(p, pp);
            }
            TextRenderer.DrawText(g, "UPI", pf, Rectangle.Round(pill), Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }

        // ---------------- QR card ----------------
        // pictureBox1 sits at (22,64,276,276) inside this card.
        RectangleF qrCard = new RectangleF(12, 56, W - 24, 292);
        card(qrCard, 14, Color.White, Color.FromArgb(220, 228, 241));
        using (SolidBrush acc = new SolidBrush(orange))
            g.FillRectangle(acc, qrCard.X + 1, qrCard.Y + 18, 3, qrCard.Height - 36);

        // ---------------- payment info card ----------------
        // label1 sits at (22,358,276,50); the card frames it.
        RectangleF info = new RectangleF(12, 354, W - 24, 58);
        card(info, 12, Color.White, Color.FromArgb(253, 186, 116));
        using (SolidBrush acc = new SolidBrush(orange))
            g.FillRectangle(acc, info.X + 1, info.Y + 12, 3, info.Height - 24);

        // smooth outer border (masks the rounded window edge)
        using (Pen fr = new Pen(Color.FromArgb(178, 196, 226), 1.4F))
        using (GraphicsPath op = round(new RectangleF(0.7f, 0.7f, W - 1.4f, H - 1.4f), 12))
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