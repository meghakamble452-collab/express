using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace APP_FRAMEWORK
{
    public partial class frmMain
    {
        private static readonly MethodInfo ExOnClick =
            typeof(Control).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.NonPublic);

        private Panel exBar;
        private Panel exCanvas;
        private Panel exLogo;
        private Panel exModeBox;
        private Panel exClockPill;
        private Label exDateTxt;
        private Label exTimeTxt;
        private Label exTip;
        private Timer exTick;
        private Timer exRegion;
        private Timer exPopFix;
        private Control exDown;
        private int exTries;
        private bool exFit;
        private Font exTipFont;
        private Font exDateFont;
        private int exLastSec = -1;
        private string exLastTip;

        private Panel exInfo;
        private Panel exChId, exChVal, exChVer, exChPay;
        private Timer exValidityTick;
        private DateTime exValidityStart;
        private TimeSpan exValiditySpan;
        private static readonly string ExVersion = "0.0.1";

        private float exK = 1f;
        private int exBarH = 200;

        private readonly List<Control> exNav = new List<Control>();
        private readonly List<Panel> exSwitchRow = new List<Panel>();
        private readonly List<Control> exPills = new List<Control>();
        private readonly Dictionary<Panel, CheckBox> exSwitchMap = new Dictionary<Panel, CheckBox>();
        private readonly Dictionary<Panel, int> exBookIndex = new Dictionary<Panel, int>();
        private readonly Dictionary<Control, float> exHov = new Dictionary<Control, float>();
        private readonly Dictionary<string, Font> exFontCache = new Dictionary<string, Font>();

        private bool exDrag;
        private int exDragX, exDragY;

        private static readonly Color ExBlue = Color.FromArgb(59, 132, 247);
        private static readonly Color ExBlue2 = Color.FromArgb(148, 198, 253);
        private static readonly Color ExGreen = UiTheme.Green;
        private static readonly Color ExBoxBg = Color.FromArgb(88, 145, 230);

        private static readonly Color ExCUser = Color.FromArgb(99, 102, 241);
        private static readonly Color ExCIrctc = Color.FromArgb(124, 58, 237);
        private static readonly Color ExCBank = Color.FromArgb(13, 148, 136);
        private static readonly Color ExCNew = Color.FromArgb(22, 163, 74);
        private static readonly Color ExCProc = Color.FromArgb(37, 99, 235);
        private static readonly Color ExCProxy = Color.FromArgb(217, 119, 6);
        private static readonly Color ExCBypass = Color.FromArgb(192, 38, 211);
        private static readonly Color ExCBackup = Color.FromArgb(71, 85, 105);
        private static readonly Color ExCToken = Color.FromArgb(8, 145, 178);
        private static readonly Color ExCPower = Color.FromArgb(234, 108, 26);
        private static readonly Color ExCRed = Color.FromArgb(220, 38, 38);

        private static bool ExIsButton(Control c)
        {
            if (c is Button) return true;
            string n = c.GetType().Name;
            return n.Contains("FlatButton") || n.Contains("ImageButton");
        }

        private static int ExS(float v, float k)
        {
            return (int)Math.Round(v * k);
        }

        private Font ExCached(string fam, float px, FontStyle st, float k)
        {
            int size = Math.Max(9, (int)Math.Round(px * k));
            string key = fam + "|" + size + "|" + (int)st;
            Font f;
            if (exFontCache.TryGetValue(key, out f) && f != null) return f;
            f = new Font(fam, size, st, GraphicsUnit.Pixel);
            exFontCache[key] = f;
            return f;
        }

        private void ApplyExpressHeader()
        {
            exBar = ExFind(this, "bnCompactHeader") as Panel;
            if (exBar == null)
            {
                if (++exTries > 12) return;
                Timer t = new Timer();
                t.Interval = 150;
                t.Tick += (s, e) => { t.Stop(); t.Dispose(); ApplyExpressHeader(); };
                t.Start();
                return;
            }

            if (pnlHeader != null) pnlHeader.Visible = false;
            if (panel1 != null) panel1.Visible = false;

            int barTop = exBar.PointToScreen(Point.Empty).Y;
            foreach (Control c in AllChildren(this))
            {
                if (c is Label && !exBar.Contains(c) && c.PointToScreen(Point.Empty).Y < barTop + 22)
                    c.Visible = false;
            }

            List<Control> kids = new List<Control>();
            foreach (Control c in exBar.Controls) kids.Add(c);

            List<CheckBox> boxes = new List<CheckBox>();
            Control logo = null, resetBox = null, closeBox = null;

            foreach (Control c in kids)
            {
                if (c.Name == "bnCompactLogo") { logo = c; continue; }
                if (c.Name == "bnTopTimeBox") { continue; }
                if (c.Name == "bnTopResetBox") { resetBox = c; continue; }
                if (c.Name == "bnTopCloseBox") { closeBox = c; continue; }

                CheckBox cb = ExFindCheckBox(c);
                if (cb != null) { boxes.Add(cb); continue; }
            }

            foreach (Control c in kids) c.Visible = false;

            if (boxes.Count == 0) boxes = ExFallbackBoxes();

            ExFitWindow();

            exBar.Location = new Point(0, 0);
            exBar.Size = new Size(ClientSize.Width, ClientSize.Height);
            exBar.BackColor = UiTheme.Bg;
            exBar.BringToFront();

            exCanvas = new Panel();
            exCanvas.Dock = DockStyle.Fill;
            exCanvas.BackColor = UiTheme.Bg;
            exCanvas.Paint += ExCanvasPaint;
            exCanvas.MouseDown += ExCanvasDown;
            exCanvas.MouseMove += ExCanvasMove;
            exCanvas.MouseUp += (s, e) => exDrag = false;
            exBar.Controls.Add(exCanvas);
            exCanvas.BringToFront();

            exLogo = new Panel();
            exLogo.BackColor = Color.White;
            exLogo.Paint += ExLogoPaint;
            Image logoImg = null;
            if (logo is PictureBox pb) logoImg = pb.Image;
            if (logoImg != null) exLogo.Tag = logoImg;
            exCanvas.Controls.Add(exLogo);

            if (boxes.Count > 0)
            {
                Panel videoRow = ExBuildVideoSwitch(boxes[0]);
                if (videoRow != null) exSwitchRow.Add(videoRow);
            }

            if (boxes.Count > 1)
            {
                exModeBox = new Panel();
                exModeBox.BackColor = ExBoxBg;
                exModeBox.Paint += ExModeBoxPaint;
                exCanvas.Controls.Add(exModeBox);

                int keep = boxes.Count > 2 ? 2 : boxes.Count - 1;

                for (int i = 1; i < boxes.Count; i++)
                {
                    Panel row = ExBuildBookSwitch(boxes[i], i);
                    if (row == null) continue;
                    if (i != keep) { row.Visible = false; continue; }
                    row.Parent = exModeBox;
                    exSwitchRow.Add(row);
                }

                for (int i = 1; i < boxes.Count; i++)
                    if (i != keep) boxes[i].Checked = false;
                if (keep >= 1 && keep < boxes.Count) boxes[keep].Checked = true;
            }

            exInfo = new Panel();
            exInfo.BackColor = Color.Transparent;
            exCanvas.Controls.Add(exInfo);

            exChId = ExInfoChip("USER ID", Convert.ToString(Program.clientID), Color.White);
            exChVer = ExInfoChip("VERSION", ExVersion, Color.White);
            exChPay = ExInfoChip("PAYMENT", string.IsNullOrEmpty(Program.KeyStatus) ? "—" : Program.KeyStatus,
                Program.KeyStatus == "Paid" ? Color.FromArgb(198, 255, 220) : Color.FromArgb(255, 208, 176));
            exChVal = ExInfoChip("VALIDITY", "—", Color.White);

            exValidityStart = DateTime.Now;
            exValiditySpan = ExLeftSpan();
            exChVal.Tag = new object[] { "VALIDITY", ExFmtLeft(exValiditySpan), Color.White };

            exDateFont = ExCached("Segoe UI Semibold", 15f, FontStyle.Bold, exK);

            exClockPill = new Panel();
            exClockPill.BackColor = Color.White;
            exClockPill.Paint += ExClockPaint;
            exCanvas.Controls.Add(exClockPill);

            exDateTxt = new Label();
            exDateTxt.AutoSize = false;
            exDateTxt.BackColor = Color.White;
            exDateTxt.ForeColor = Color.FromArgb(20, 78, 190);
            exDateTxt.TextAlign = ContentAlignment.MiddleRight;
            exClockPill.Controls.Add(exDateTxt);

            exTimeTxt = new Label();
            exTimeTxt.AutoSize = false;
            exTimeTxt.BackColor = Color.White;
            exTimeTxt.ForeColor = Color.FromArgb(20, 78, 190);
            exTimeTxt.TextAlign = ContentAlignment.MiddleLeft;
            exClockPill.Controls.Add(exTimeTxt);

            Control resetHit = ExFindClickable(resetBox) ?? ExFind(this, "bnResetGlyph") ?? resetBox;
            if (resetHit != null)
            {
                Panel rb = ExPill("", 2, ExCPower, 9, 44, 38);
                rb.Click += (s, e) => ExClick(resetHit);
                exCanvas.Controls.Add(rb);
            }

            Control closeHit = ExFindClickable(closeBox) ?? closeBox ?? btnClose;
            if (closeHit != null)
            {
                Panel cbx = ExPill("", 3, ExCRed, 10, 44, 38);
                cbx.Click += (s, e) => ExClick(closeHit);
                exCanvas.Controls.Add(cbx);
            }

            string[,] nav =
            {
                { "bunifuFlatButton7", "IRCTC ID" },
                { "bunifuFlatButton1", "BANK" },
                { "bunifuFlatButton3", "NEW TICKET" },
                { "bunifuFlatButton4", "PROCESS_TICKET" },
                { "bunifuFlatButton2", "PROXY" },
                { "bunifuFlatButton6", "BYPASS" },
                { "bunifuFlatButton8", "BACKUP" }
            };
            Color[] navCol = { ExCIrctc, ExCBank, ExCNew, ExCProc, ExCProxy, ExCBypass, ExCBackup };
            int[] navIco = { 1, 2, 3, 4, 5, 6, 7 };

            for (int i = 0; i < nav.GetLength(0); i++)
            {
                Control src = ExFind(this, nav[i, 0]);
                if (src == null) src = ExFindByText(nav[i, 1]);
                if (src == null) continue;
                Control b = src;
                Panel p = ExPill(nav[i, 1], 0, navCol[i], navIco[i], 150, 44);
                p.Click += (s, e) => ExClick(b);
                exCanvas.Controls.Add(p);
                exNav.Add(p);
            }

            exTipFont = ExCached("Segoe UI Semibold", 11f, FontStyle.Bold, exK);

            exTip = new Label();
            exTip.AutoSize = false;
            exTip.Size = new Size(150, 24);
            exTip.Font = exTipFont;
            exTip.ForeColor = Color.White;
            exTip.BackColor = Color.FromArgb(15, 40, 78);
            exTip.TextAlign = ContentAlignment.MiddleCenter;
            exTip.Visible = false;
            exTip.Paint += ExTipPaint;
            exCanvas.Controls.Add(exTip);

            ExLayoutBar();

            exTick = new Timer();
            exTick.Interval = 16;
            exTick.Tick += ExTick;
            exTick.Start();

            exValidityTick = new Timer();
            exValidityTick.Interval = 1000;
            exValidityTick.Tick += ExValidityTickTick;
            exValidityTick.Start();

            exRegion = new Timer();
            exRegion.Interval = 40;
            int rt = 0;
            exRegion.Tick += (s, e) =>
            {
                rt++;
                ExApplyAllRegions();
                if (rt > 80) { exRegion.Stop(); exRegion.Dispose(); }
            };
            exRegion.Start();

            exCanvas.Resize += (s, e) => { ExLayoutBar(); ExApplyAllRegions(); };
            Resize += (s, e) => { ExFitWindow(); ExLayoutBar(); ExApplyAllRegions(); };
            Application.ThreadException -= ExLogThread;
            Application.ThreadException += ExLogThread;
            AppDomain.CurrentDomain.UnhandledException -= ExLogDomain;
            AppDomain.CurrentDomain.UnhandledException += ExLogDomain;
            FormClosed += (s, e) =>
            {
                if (exValidityTick != null) { exValidityTick.Stop(); exValidityTick.Dispose(); exValidityTick = null; }
            };
        }

        private static void ExLogText(string where, Exception ex)
        {
            try
            {
                System.IO.File.AppendAllText(
                    System.IO.Path.Combine(Application.StartupPath, "express_ui_error.log"),
                    DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "  [" + where + "]" +
                    Environment.NewLine + ex + Environment.NewLine +
                    "----------------------------------------" + Environment.NewLine);
            }
            catch { }
        }

        private void ExLogThread(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ExLogText("thread", e.Exception);
        }

        private void ExLogDomain(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null) ExLogText("domain", ex);
        }

        private void ExApplyAllRegions()
        {
            try
            {
                if (exCanvas == null) return;
                ExTameLeftovers();
                int rad = ExS(16, exK);
                ExRoundRegion(this, rad);
                ExRoundRegion(exCanvas, rad);
                if (exModeBox != null) ExRoundRegion(exModeBox, ExS(12, exK));
                if (exClockPill != null) ExRoundRegion(exClockPill, ExS(12, exK));
                foreach (Control c in exPills) ExRoundRegion(c, ExS(11, exK));
            }
            catch { }
        }

        private void ExTameLeftovers()
        {
            try
            {
                if (exBar.Anchor != (AnchorStyles.Top | AnchorStyles.Left))
                    exBar.Anchor = AnchorStyles.Top | AnchorStyles.Left;

                string[] names = { "bnTopTimeBox", "bnTopResetBox", "bnTopCloseBox", "bnCompactNavigation" };
                foreach (string n in names)
                {
                    Control c = ExFind(exBar, n);
                    if (c != null && c.Visible) c.Visible = false;
                }

                if (btnClose != null && btnClose.Visible) btnClose.Visible = false;
                if (_bnClockIcon != null && _bnClockIcon.Visible) _bnClockIcon.Visible = false;
                if (_bnClockTitle != null && _bnClockTitle.Visible) _bnClockTitle.Visible = false;
                if (_bnResetButton != null && _bnResetButton.Visible) _bnResetButton.Visible = false;
            }
            catch { }
        }

        // ==================================================================
        private void ExFitWindow()
        {
            if (exFit) return;
            exFit = true;
            try
            {
                Rectangle wa = Screen.FromControl(this).WorkingArea;
                if (wa.Width < 200 || wa.Height < 120) return;

                exK = Math.Max(0.70f, Math.Min(1.25f, wa.Width / 1920f));
                exBarH = ExS(200, exK);

                int needW = Math.Min(wa.Width - 20, ExS(1720, exK));
                int needH = exBarH;

                int w = Math.Max(ClientSize.Width, needW);
                int h = needH;
                if (w == ClientSize.Width && h == ClientSize.Height) return;

                int dH = h - ClientSize.Height;
                ClientSize = new Size(w, h);
                if (dH != 0) Top -= dH;

                if (Bottom > wa.Bottom) Top = wa.Bottom - Height;
                if (Top < wa.Top) Top = wa.Top;
            }
            catch { }
            finally { exFit = false; }
        }

        private void ExFixUserInfoPopup()
        {
            Panel pill = null;
            foreach (Control c in exPills)
                if (c.Tag is object[] t && (int)t[1] == 1) { pill = c as Panel; break; }
            if (pill == null) return;

            if (exPopFix != null) { exPopFix.Stop(); exPopFix.Dispose(); }
            exPopFix = new Timer();
            exPopFix.Interval = 16;
            int n = 0;
            exPopFix.Tick += (s, e) =>
            {
                try
                {
                    n++;
                    Form pop = ExFindInfoPopup();
                    if (pop == null || !pop.Visible || n > 70)
                    {
                        if (n > 70) { exPopFix.Stop(); exPopFix.Dispose(); }
                        return;
                    }
                    Rectangle wa = Screen.FromControl(this).WorkingArea;
                    Point p0 = pill.PointToScreen(Point.Empty);
                    int x = p0.X + (pill.Width - pop.Width) / 2;
                    x = Math.Max(wa.Left + 6, Math.Min(wa.Right - pop.Width - 6, x));
                    pop.Left = x;
                }
                catch { }
            };
            exPopFix.Start();
        }

        private Form ExFindInfoPopup()
        {
            foreach (Form f in Application.OpenForms)
            {
                if (ReferenceEquals(f, this)) continue;
                if (f.FormBorderStyle != FormBorderStyle.None) continue;
                if (f.Width < 290 || f.Width > 350) continue;
                if (f.Height < 130 || f.Height > 175) continue;
                return f;
            }
            return null;
        }

        private static Control ExFind(Control root, string name)
        {
            if (root.Name == name) return root;
            foreach (Control c in root.Controls)
            {
                Control r = ExFind(c, name);
                if (r != null) return r;
            }
            return null;
        }

        private Control ExFindByText(string keyword)
        {
            string k = keyword.Replace("&", "").Trim().ToLowerInvariant();
            foreach (Control c in AllChildren(this))
            {
                if (!ExIsButton(c)) continue;
                if (c.Text.Replace("&", "").Trim().ToLowerInvariant() == k) return c;
            }
            return null;
        }

        private static CheckBox ExFindCheckBox(Control root)
        {
            if (root is CheckBox cb) return cb;
            foreach (Control c in root.Controls)
            {
                CheckBox r = ExFindCheckBox(c);
                if (r != null) return r;
            }
            return null;
        }

        private static Control ExFindClickable(Control root)
        {
            if (root == null) return null;
            foreach (Control c in root.Controls)
            {
                if (ExIsButton(c)) return c;
                Control r = ExFindClickable(c);
                if (r != null) return r;
            }
            return null;
        }

        private static IEnumerable<Control> AllChildren(Control root)
        {
            foreach (Control c in root.Controls)
            {
                yield return c;
                foreach (Control k in AllChildren(c)) yield return k;
            }
        }

        private List<CheckBox> ExFallbackBoxes()
        {
            List<CheckBox> list = new List<CheckBox>();
            string[,] map =
            {
                { "checkBox1", "Video Mode" },
                { "chkADV",    "Booking Mode-1" },
                { "checkBox3", "Booking Mode-2" },
                { "checkBox5", "Booking Mode-3" }
            };
            for (int i = 0; i < map.GetLength(0); i++)
            {
                CheckBox cb = ExFind(this, map[i, 0]) as CheckBox;
                if (cb == null) continue;
                cb.Text = map[i, 1];
                list.Add(cb);
            }
            return list;
        }

        private static void ExClick(Control c)
        {
            if (c == null || ExOnClick == null) return;
            try { ExOnClick.Invoke(c, new object[] { EventArgs.Empty }); } catch { }
        }

        private static Color ExMix(Color a, Color b, float t)
        {
            t = Math.Max(0f, Math.Min(1f, t));
            return Color.FromArgb(
                (int)(a.R + (b.R - a.R) * t),
                (int)(a.G + (b.G - a.G) * t),
                (int)(a.B + (b.B - a.B) * t));
        }

        // ==================================================================
        private Panel ExBuildVideoSwitch(CheckBox cb)
        {
            Panel row = new Panel();
            row.Size = new Size(ExS(150, exK), ExS(46, exK));
            row.BackColor = Color.Transparent;
            row.Cursor = Cursors.Hand;
            row.Tag = cb;
            row.Click += (s, e) => { cb.Checked = !cb.Checked; };
            row.Paint += ExSwitchRowPaint;
            exCanvas.Controls.Add(row);

            Panel track = new Panel();
            track.Size = new Size(ExS(42, exK), ExS(22, exK));
            track.BackColor = Color.Transparent;
            track.Cursor = Cursors.Hand;
            track.Click += (s, e) => { cb.Checked = !cb.Checked; };
            track.Paint += ExSwitchPaint;
            row.Controls.Add(track);

            cb.CheckedChanged += (s, e) => { track.Invalidate(); row.Invalidate(); };
            exSwitchMap[row] = cb;
            return row;
        }

        private Panel ExBuildBookSwitch(CheckBox cb, int index)
        {
            Panel row = new Panel();
            row.Size = new Size(ExS(70, exK), ExS(20, exK));
            row.BackColor = ExBoxBg;
            row.Cursor = Cursors.Hand;
            row.Tag = cb;
            row.Click += (s, e) => { cb.Checked = !cb.Checked; };
            row.Paint += ExSwitchRowPaint;
            exCanvas.Controls.Add(row);

            Panel track = new Panel();
            track.Size = new Size(ExS(38, exK), ExS(20, exK));
            track.BackColor = ExBoxBg;
            track.Cursor = Cursors.Hand;
            track.Click += (s, e) => { cb.Checked = !cb.Checked; };
            track.Paint += ExSwitchPaint;
            row.Controls.Add(track);

            cb.CheckedChanged += (s, e) => { track.Invalidate(); row.Invalidate(); };
            exSwitchMap[row] = cb;
            exBookIndex[row] = index;
            return row;
        }


        private Panel ExInfoChip(string cap, string val, Color valCol)
        {
            Panel p = new Panel();
            p.BackColor = Color.Transparent;
            p.Tag = new object[] { cap, val, valCol };
            p.Paint += ExInfoChipPaint;
            exInfo.Controls.Add(p);
            return p;
        }

        private int ExChipW(Panel p)
        {
            object[] m = p.Tag as object[];
            if (m == null) return ExS(90, exK);
            Font fc = ExCached("Segoe UI Semibold", 9f, FontStyle.Bold, exK);
            Font fv = ExCached("Segoe UI Semibold", 10.5f, FontStyle.Bold, exK);
            return TextRenderer.MeasureText((string)m[0], fc).Width
                 + TextRenderer.MeasureText(Convert.ToString(m[1]), fv).Width
                 + ExS(36, exK);
        }

        private void ExInfoChipPaint(object sender, PaintEventArgs e)
        {
            try
            {
                Panel p = sender as Panel;
                if (p == null || p.Width < 10 || p.Height < 8) return;
                object[] m = p.Tag as object[];
                if (m == null) return;

                string cap = (string)m[0];
                string val = Convert.ToString(m[1]);
                Color vc = (Color)m[2];

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, p.Width - 1, p.Height - 1);

                using (GraphicsPath path = ExRounded(r, ExS(9, exK)))
                {
                    using (SolidBrush b = new SolidBrush(Color.FromArgb(46, 255, 255, 255)))
                        e.Graphics.FillPath(b, path);
                    using (Pen pen = new Pen(Color.FromArgb(70, 255, 255, 255), 1f))
                        e.Graphics.DrawPath(pen, path);
                }

                Font fc = ExCached("Segoe UI Semibold", 9f, FontStyle.Bold, exK);
                Font fv = ExCached("Segoe UI Semibold", 10.5f, FontStyle.Bold, exK);
                int cw = TextRenderer.MeasureText(cap, fc).Width;
                int pad = ExS(10, exK);

                TextRenderer.DrawText(e.Graphics, cap, fc,
                    new Rectangle(pad, 0, cw + 2, p.Height), Color.FromArgb(206, 232, 246, 255),
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

                TextRenderer.DrawText(e.Graphics, val, fv,
                    new Rectangle(pad + cw + ExS(7, exK), 0, Math.Max(8, p.Width - cw - pad - ExS(18, exK)), p.Height), vc,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
            }
            catch { }
        }

        private void ExValidityTickTick(object sender, EventArgs e)
        {
            try
            {
                if (exChVal == null || exChVal.IsDisposed) return;
                object[] m = exChVal.Tag as object[];
                if (m == null) return;

                TimeSpan left = exValiditySpan - (DateTime.Now - exValidityStart);
                if (left.TotalSeconds < 0) left = TimeSpan.Zero;

                string txt = ExFmtLeft(left);
                if (Equals(m[1], txt)) return;
                m[1] = txt;
                exChVal.Invalidate();
            }
            catch { }
        }

        private static TimeSpan ExLeftSpan()
        {
            string a = Program.availtime;
            if (!string.IsNullOrWhiteSpace(a))
            {
                TimeSpan t;
                if (TimeSpan.TryParse(a, out t) && t.TotalSeconds > 0) return t;
                double sec;
                if (double.TryParse(a.Trim(), System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture, out sec) && sec > 0)
                    return TimeSpan.FromSeconds(sec);
            }
            if (Program.ExpiryDay > 0) return TimeSpan.FromDays(Program.ExpiryDay);
            return TimeSpan.Zero;
        }

        private static string ExFmtLeft(TimeSpan t)
        {
            if (t.TotalSeconds <= 0) return "Expired";
            if (t.TotalDays >= 1) return ((int)t.TotalDays) + "d " + t.ToString(@"hh\:mm\:ss");
            if (t.TotalHours >= 1) return t.ToString(@"hh\:mm\:ss");
            if (t.TotalMinutes >= 1) return t.ToString(@"mm\:ss") + " min";
            return ((int)t.TotalSeconds) + " sec";
        }

        private Panel ExPill(string text, int kind, Color accent, int icon, int w, int h)
        {
            Panel p = new Panel();
            p.Size = new Size(ExS(w, exK), ExS(h, exK));
            p.BackColor = accent;
            p.Cursor = Cursors.Hand;
            p.Tag = new object[] { text, kind, accent, icon };
            p.Paint += ExPillPaint;
            exHov[p] = 0f;
            exPills.Add(p);
            ExRoundRegion(p, ExS(11, exK));
            return p;
        }

        // ==================================================================
        private void ExLayoutBar()
        {
            try
            {
                if (exCanvas == null) return;
                float k = exK;

                if (exBar.Location != Point.Empty) exBar.Location = Point.Empty;
                Size want = new Size(ClientSize.Width, ClientSize.Height);
                if (exBar.Width != want.Width || exBar.Height != want.Height) exBar.Size = want;

                int W = exCanvas.Width;
                if (W < 20 || exCanvas.Height < 20) return;

                if (exLogo != null) exLogo.Bounds = new Rectangle(ExS(14, k), ExS(16, k), ExS(56, k), ExS(56, k));

                int rowY = ExS(124, k);

                if (exSwitchRow.Count > 0 && exModeBox != null)
                {
                    exSwitchRow[0].Bounds = new Rectangle(ExS(16, k), rowY, ExS(150, k), ExS(46, k));
                    if (exSwitchRow[0].Controls.Count > 0)
                        exSwitchRow[0].Controls[0].Location = new Point(ExS(104, k), ExS(12, k));
                }

                if (exModeBox != null)
                {
                    int boxW = ExS(24, k) + (exSwitchRow.Count - 1) * ExS(74, k);
                    exModeBox.Bounds = new Rectangle(ExS(16, k) + ExS(150, k) + ExS(14, k), rowY - ExS(2, k), boxW, ExS(46, k));

                    for (int i = 1; i < exSwitchRow.Count; i++)
                    {
                        Panel r = exSwitchRow[i];
                        r.Bounds = new Rectangle(ExS(12, k) + (i - 1) * ExS(74, k), ExS(22, k), ExS(70, k), ExS(20, k));
                        if (r.Controls.Count > 0)
                        {
                            r.Controls[0].Size = new Size(ExS(38, k), ExS(20, k));
                            r.Controls[0].Location = new Point(ExS(18, k), 0);
                        }
                    }
                }

                if (exInfo != null)
                {
                    exInfo.Bounds = new Rectangle(ExS(16, k), ExS(86, k), Math.Max(40, W - ExS(32, k)), ExS(30, k));
                    int cxp = 0;
                    Panel[] chips = { exChId, exChVal, exChVer, exChPay };
                    foreach (Panel cp in chips)
                    {
                        if (cp == null) continue;
                        int cwp = ExChipW(cp);
                        cp.Bounds = new Rectangle(cxp, 0, cwp, ExS(30, k));
                        ExRoundRegion(cp, ExS(9, k));
                        cxp += cwp + ExS(6, k);
                    }
                }

                int pillH = ExS(44, k);
                int topY = ExS(24, k);

                int closeW = ExS(44, k);
                int gap = ExS(7, k);
                int right = W - ExS(18, k);

                int closeX = right - closeW;
                int powerX = closeX - gap - closeW;
                

                if (exClockPill != null)
                {
                    Font fd = ExCached("Segoe UI Semibold", 11f, FontStyle.Bold, k);
                    Font ft = ExCached("Segoe UI Semibold", 30f, FontStyle.Bold, k);
                    exDateTxt.Font = fd;
                    exTimeTxt.Font = ft;

                    int dw = TextRenderer.MeasureText("00-00-0000", fd).Width + ExS(4, k);
                    int tw = TextRenderer.MeasureText("00:00:00", ft).Width + ExS(6, k);
                    int ph = ExS(48, k);
                    int pw = dw + tw + ExS(24, k);
                    if (pw > W - ExS(60, k)) pw = W - ExS(60, k);

                    int px = (W - pw) / 2;
                    exClockPill.Bounds = new Rectangle(px, ExS(17, k), pw, ph);

                    int dx = ExS(12, k);
                    exDateTxt.Bounds = new Rectangle(dx, 0, dw, ph);
                    exTimeTxt.Bounds = new Rectangle(dx + dw + ExS(4, k), 0, Math.Max(20, pw - dx - dw - ExS(16, k)), ph);
                }

                foreach (Control c in exCanvas.Controls)
                {
                    if (!(c.Tag is object[] t)) continue;
                    int kind = (int)t[1];
                    if (kind == 1) c.Bounds = new Rectangle(userX, topY, userW, ExS(38, k));
                    if (kind == 2) c.Bounds = new Rectangle(powerX, topY, closeW, ExS(38, k));
                    if (kind == 3) c.Bounds = new Rectangle(closeX, topY, closeW, ExS(38, k));
                }

                int n = exNav.Count;
                if (n > 0)
                {
                    int startX = ExS(16, k) + ExS(150, k) + ExS(14, k) + (exModeBox != null ? exModeBox.Width : 0) + ExS(18, k);
                    int g2 = ExS(8, k);
                    int minW = ExS(140, k);
                    int avail = Math.Max(n * minW, W - startX - ExS(14, k));
                    int bw = Math.Max(minW, (avail - (n - 1) * g2) / n);
                    int x = startX;
                    foreach (Control b in exNav)
                    {
                        b.Bounds = new Rectangle(x, rowY - ExS(2, k), bw, pillH);
                        x += bw + g2;
                    }
                }

                ExApplyAllRegions();
                if (exTip != null) exTip.BringToFront();
                if (exClockPill != null) exClockPill.BringToFront();
            }
            catch { }
        }

        // ==================================================================
        private void ExTick(object sender, EventArgs e)
        {
            try
            {
                if (exCanvas == null || !Visible) return;
                if (exBar.Width != ClientSize.Width || exBar.Height != ClientSize.Height) ExLayoutBar();

                if (exDateTxt != null && exTimeTxt != null)
                {
                    DateTime now = DateTime.Now;
                    int stamp = (int)(now.Ticks / TimeSpan.TicksPerSecond);
                    if (stamp != exLastSec)
                    {
                        exLastSec = stamp;
                        string dPart = now.ToString("dd-MM-yyyy");
                        string tPart = now.ToString("HH:mm:ss");
                        if (exDateTxt.Text != dPart) exDateTxt.Text = dPart;
                        if (exTimeTxt.Text != tPart) exTimeTxt.Text = tPart;
                    }
                }

                Point p = exCanvas.PointToClient(Cursor.Position);
                Control under = null;
                if (exCanvas.ClientRectangle.Contains(p))
                {
                    foreach (Control c in exPills)
                    {
                        if (!c.Visible) continue;
                        if (c.Bounds.Contains(p)) { under = c; break; }
                    }
                }

                bool down = Control.MouseButtons == MouseButtons.Left;
                exDown = (under != null && down) ? under : null;

                foreach (Control c in exPills)
                {
                    float target = (c == under) ? 1f : 0f;
                    float v = exHov.ContainsKey(c) ? exHov[c] : 0f;
                    float diff = target - v;
                    if (Math.Abs(diff) < 0.01f) v = target; else v += diff * 0.22f;
                    if (v != exHov[c]) { exHov[c] = v; c.Invalidate(); }
                }

                string tip = null;
                int underKind = -1;

                if (under != null && under.Tag is object[] m)
                {
                    underKind = (int)m[1];
                    if (underKind == 2) tip = "POWER / RESTART";
                    else if (underKind == 3) tip = "CLOSE";
                    else tip = (string)m[0];
                }

                if (tip == null)
                {
                    if (exTip.Visible) exTip.Visible = false;
                }
                else
                {
                    if (tip != exLastTip)
                    {
                        exLastTip = tip;
                        Font f = exTipFont != null ? exTipFont : exTip.Font;
                        exTip.Size = new Size(Math.Max(ExS(96, exK), TextRenderer.MeasureText(tip, f).Width + ExS(22, exK)), ExS(24, exK));
                        exTip.Text = tip;
                    }

                    int tx = under.Left + under.Width / 2 - exTip.Width / 2;
                    tx = Math.Max(8, Math.Min(exCanvas.Width - exTip.Width - 8, tx));

                    int ty = underKind == 0
                        ? ExS(62, exK)
                        : Math.Max(4, under.Top - ExS(30, exK));

                    exTip.Location = new Point(tx, ty);
                    if (!exTip.Visible) { exTip.Visible = true; exTip.BringToFront(); }
                }
            }
            catch { }
        }

        private void ExTipPaint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, exTip.Width - 1, exTip.Height - 1);
                if (r.Width < 4 || r.Height < 4) return;
                using (GraphicsPath path = ExRounded(r, ExS(7, exK)))
                using (SolidBrush b = new SolidBrush(Color.FromArgb(15, 40, 78)))
                    e.Graphics.FillPath(b, path);
                TextRenderer.DrawText(e.Graphics, exTip.Text, exTip.Font, r, Color.White,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
            catch { }
        }

        // ==================================================================
        private void ExCanvasDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            exDrag = true;
            exDragX = Cursor.Position.X - FindForm().Left;
            exDragY = Cursor.Position.Y - FindForm().Top;
        }

        private void ExCanvasMove(object sender, MouseEventArgs e)
        {
            if (!exDrag) return;
            FindForm().Left = Cursor.Position.X - exDragX;
            FindForm().Top = Cursor.Position.Y - exDragY;
        }

        // ==================================================================
        private void ExCanvasPaint(object sender, PaintEventArgs e)
        {
            try
            {
                float k = exK;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                int w = exCanvas.Width;
                int h = exCanvas.Height;
                if (w < 12 || h < 12) return;

                using (SolidBrush bg = new SolidBrush(UiTheme.Bg))
                    e.Graphics.FillRectangle(bg, new Rectangle(0, 0, w, h));

                Rectangle r = new Rectangle(0, 0, w - 1, h - 1);
                int rad = ExS(16, k);

                using (GraphicsPath path = ExRounded(r, rad))
                using (LinearGradientBrush lb = new LinearGradientBrush(r, ExBlue, ExBlue2, 0f))
                    e.Graphics.FillPath(lb, path);

                using (GraphicsPath bp = ExRounded(new Rectangle(1, 1, w - 3, h - 3), rad))
                using (Pen outer = new Pen(Color.FromArgb(175, 255, 255, 255), 2f))
                    e.Graphics.DrawPath(outer, bp);

                using (GraphicsPath ip = ExRounded(new Rectangle(4, 4, w - 9, h - 9), Math.Max(4, rad - 3)))
                using (Pen inner = new Pen(Color.FromArgb(55, 12, 60, 130), 1.2f))
                    e.Graphics.DrawPath(inner, ip);

                Font fe = ExCached("Segoe UI", 26f, FontStyle.Bold | FontStyle.Italic, k);
                TextRenderer.DrawText(e.Graphics, "EXPRESS", fe,
                    new Rectangle(ExS(84, k), ExS(17, k), ExS(380, k), ExS(32, k)), Color.White,
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.NoClipping);

                Font fs = ExCached("Segoe UI Semibold", 10f, FontStyle.Bold, k);
                TextRenderer.DrawText(e.Graphics, "PREMIUM TATKAL SOFTWARE", fs,
                    new Rectangle(ExS(86, k), ExS(51, k), ExS(380, k), ExS(14, k)), Color.FromArgb(240, 248, 255),
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.NoClipping);

                using (Pen p = new Pen(Color.FromArgb(110, 255, 255, 255), 1f))
                    e.Graphics.DrawLine(p, ExS(16, k), ExS(80, k), w - ExS(16, k), ExS(80, k));
            }
            catch { }
        }

        private void ExLogoPaint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                Rectangle r = new Rectangle(0, 0, exLogo.Width, exLogo.Height);
                if (r.Width < 4 || r.Height < 4) return;

                using (GraphicsPath p = ExRounded(r, ExS(14, exK)))
                using (SolidBrush b = new SolidBrush(Color.White))
                    e.Graphics.FillPath(b, p);

                Image img = exLogo.Tag as Image;
                if (img != null)
                {
                    int box = ExS(40, exK);
                    float sc = Math.Min((float)box / img.Width, (float)box / img.Height);
                    int w = Math.Max(1, (int)(img.Width * sc));
                    int h = Math.Max(1, (int)(img.Height * sc));
                    e.Graphics.DrawImage(img, new Rectangle((exLogo.Width - w) / 2, (exLogo.Height - h) / 2, w, h));
                }
                else
                {
                    Font f = ExCached("Segoe UI Black", 24f, FontStyle.Bold, exK);
                    TextRenderer.DrawText(e.Graphics, "E", f, r, ExBlue,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }
            catch { }
        }

        private void ExModeBoxPaint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, exModeBox.Width - 1, exModeBox.Height - 1);
                if (r.Width < 4 || r.Height < 4) return;

                using (GraphicsPath p = ExRounded(r, ExS(12, exK)))
                {
                    using (SolidBrush b = new SolidBrush(ExBoxBg)) e.Graphics.FillPath(b, p);
                    using (Pen pen = new Pen(Color.FromArgb(120, 255, 255, 255), 1f)) e.Graphics.DrawPath(pen, p);
                }

                Font f = ExCached("Segoe UI Semibold", 10f, FontStyle.Bold, exK);
                TextRenderer.DrawText(e.Graphics, "BOOKING MODE", f,
                    new Rectangle(ExS(12, exK), ExS(5, exK), ExS(150, exK), ExS(14, exK)), Color.White,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
            catch { }
        }

        private void ExClockPaint(object sender, PaintEventArgs e)
        {
            try
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, exClockPill.Width - 1, exClockPill.Height - 1);
                if (r.Width < 6 || r.Height < 6) return;

                using (GraphicsPath sh = ExRounded(new Rectangle(r.X, r.Y + ExS(2, exK), r.Width, r.Height), ExS(12, exK)))
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(75, 8, 26, 58)))
                    e.Graphics.FillPath(sb, sh);

                using (GraphicsPath p = ExRounded(r, ExS(12, exK)))
                {
                    using (SolidBrush b = new SolidBrush(Color.White)) e.Graphics.FillPath(b, p);
                    using (Pen pen = new Pen(Color.White, 1.5f)) e.Graphics.DrawPath(pen, p);
                }
            }
            catch { }
        }

        private void ExSwitchRowPaint(object sender, PaintEventArgs e)
        {
            try
            {
                Panel row = (Panel)sender;
                CheckBox cb;
                if (!exSwitchMap.TryGetValue(row, out cb)) return;

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                if (exBookIndex.ContainsKey(row)) return;

                Font f = ExCached("Segoe UI", 12f, FontStyle.Regular, exK);
                TextRenderer.DrawText(e.Graphics, cb.Text, f,
                    new Rectangle(0, 0, ExS(100, exK), row.Height), Color.White,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            }
            catch { }
        }

        private void ExSwitchPaint(object sender, PaintEventArgs e)
        {
            try
            {
                Panel track = (Panel)sender;
                Panel row = track.Parent as Panel;
                if (row == null) return;

                CheckBox cb;
                if (!exSwitchMap.TryGetValue(row, out cb)) return;

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, track.Width - 1, track.Height - 1);
                if (r.Width < 4 || r.Height < 4) return;

                using (GraphicsPath p = ExRounded(r, r.Height / 2))
                {
                    Color off = Color.FromArgb(195, 255, 255, 255);
                    using (SolidBrush b = new SolidBrush(cb.Checked ? ExGreen : off))
                        e.Graphics.FillPath(b, p);

                    int kk = r.Height - ExS(4, exK);
                    int x = cb.Checked ? (r.Width - kk - ExS(2, exK)) : ExS(2, exK);
                    using (SolidBrush kb = new SolidBrush(Color.White))
                        e.Graphics.FillEllipse(kb, x, ExS(2, exK), kk, kk);
                }
            }
            catch { }
        }

        private void ExPillPaint(object sender, PaintEventArgs e)
        {
            try
            {
                Panel p = (Panel)sender;
                object[] meta = p.Tag as object[];
                if (meta == null) return;

                float k = exK;
                string text = (string)meta[0];
                int kind = (int)meta[1];
                Color accent = (Color)meta[2];
                int icon = (int)meta[3];

                if (p.Width < 8 || p.Height < 8) return;

                float h = exHov.ContainsKey(p) ? exHov[p] : 0f;
                bool dn = (exDown == p);
                bool hot = h > 0.02f;

                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                // hover par button 1px uthta hai, press par 2px dabta hai
                int dy = dn ? ExS(2, k) : (hot ? -1 : 0);
                Rectangle r = new Rectangle(1, 1 + dy, p.Width - 3, p.Height - 3 - dy);
                if (r.Width < 6 || r.Height < 6) return;
                int rad = ExS(10, k);

                // hover par shadow badi aur gehri
                int shA = dn ? 30 : (int)(50 + 60 * h);
                int shOff = dn ? ExS(1, k) : (int)(ExS(2, k) + ExS(4, k) * h);
                Rectangle sh2 = new Rectangle(r.X - 1, r.Y + shOff + ExS(1, k), r.Width + 2, r.Height);
                using (GraphicsPath sp2 = ExRounded(sh2, rad + 1))
                using (SolidBrush sb2 = new SolidBrush(Color.FromArgb(shA, 6, 22, 52)))
                    e.Graphics.FillPath(sb2, sp2);

                Rectangle sh1 = new Rectangle(r.X, r.Y + shOff, r.Width, r.Height);
                using (GraphicsPath sp1 = ExRounded(sh1, rad))
                using (SolidBrush sb1 = new SolidBrush(Color.FromArgb(Math.Min(255, shA + 30), 8, 26, 58)))
                    e.Graphics.FillPath(sb1, sp1);

                // hover par rang saaf halka, press par saaf gehra
                Color bg;
                if (dn) bg = ExMix(accent, Color.Black, 0.26f);
                else if (hot) bg = ExMix(accent, Color.White, 0.12f + 0.30f * h);
                else bg = accent;

                using (GraphicsPath path = ExRounded(r, rad))
                {
                    using (SolidBrush b = new SolidBrush(bg)) e.Graphics.FillPath(b, path);

                    // hover par safed rim, warna halki rim
                    Color rim = hot ? Color.FromArgb((int)(120 + 120 * h), 255, 255, 255)
                                    : Color.FromArgb(70, 255, 255, 255);
                    using (Pen pen = new Pen(rim, hot ? 1.8f : 1.1f))
                        e.Graphics.DrawPath(pen, path);

                    // andar ki gehari line (depth)
                    if (!dn)
                    {
                        using (GraphicsPath ip = ExRounded(new Rectangle(r.X + 1, r.Y + 1, r.Width - 2, r.Height - 2), Math.Max(3, rad - 2)))
                        using (Pen ipen = new Pen(Color.FromArgb(40, 8, 26, 58), 1f))
                            e.Graphics.DrawPath(ipen, ip);
                    }
                }

                Color fg = Color.White;

                if (kind == 2 || kind == 3 || text.Length == 0)
                {
                    Rectangle ir = new Rectangle(r.X + r.Width / 2 - ExS(10, k), r.Y + r.Height / 2 - ExS(10, k), ExS(20, k), ExS(20, k));
                    ExIcon(e.Graphics, ir, icon, fg, k);
                    return;
                }

                Rectangle ic = new Rectangle(r.X + ExS(12, k), r.Y + r.Height / 2 - ExS(9, k), ExS(18, k), ExS(18, k));
                ExIcon(e.Graphics, ic, icon, fg, k);

                Rectangle tr = new Rectangle(r.X + ExS(38, k), r.Y, r.Width - ExS(46, k), r.Height);
                if (tr.Width < 10) return;

                int size = Math.Max(9, (int)Math.Round(12 * k));
                Font f = ExCached("Segoe UI Semibold", size, FontStyle.Bold, 1f);
                while (size > 9 && TextRenderer.MeasureText(text, f).Width > tr.Width)
                {
                    size--;
                    f = ExCached("Segoe UI Semibold", size, FontStyle.Bold, 1f);
                }

                TextRenderer.DrawText(e.Graphics, text, f, tr, fg,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
            }
            catch { }
        }

        private static void ExIcon(Graphics g, Rectangle r, int id, Color col, float k)
        {
            int cx = r.X + r.Width / 2;
            int cy = r.Y + r.Height / 2;
            using (Pen p = new Pen(col, Math.Max(1.4f, 1.9f * k)))
            {
                p.StartCap = LineCap.Round;
                p.EndCap = LineCap.Round;
                p.LineJoin = LineJoin.Round;

                switch (id)
                {
                    case 1:
                        g.DrawRectangle(p, r.X + 1, r.Y + 4, r.Width - 2, r.Height - 8);
                        g.DrawEllipse(p, r.X + 4, r.Y + 7, 5, 5);
                        g.DrawLine(p, r.X + 11, r.Y + 9, r.Right - 3, r.Y + 9);
                        g.DrawLine(p, r.X + 11, r.Y + 13, r.Right - 5, r.Y + 13);
                        break;
                    case 2:
                        g.DrawLine(p, r.X + 1, r.Y + 6, cx, r.Y + 2);
                        g.DrawLine(p, cx, r.Y + 2, r.Right - 1, r.Y + 6);
                        g.DrawLine(p, r.X + 1, r.Y + 6, r.Right - 1, r.Y + 6);
                        g.DrawLine(p, r.X + 4, r.Y + 6, r.X + 4, r.Bottom - 3);
                        g.DrawLine(p, cx, r.Y + 6, cx, r.Bottom - 3);
                        g.DrawLine(p, r.Right - 4, r.Y + 6, r.Right - 4, r.Bottom - 3);
                        g.DrawLine(p, r.X + 1, r.Bottom - 2, r.Right - 1, r.Bottom - 2);
                        break;
                    case 3:
                        g.DrawLine(p, cx, r.Y + 3, cx, r.Bottom - 3);
                        g.DrawLine(p, r.X + 3, cy, r.Right - 3, cy);
                        break;
                    case 4:
                        g.DrawPolygon(p, new Point[] {
                            new Point(r.X + 4, r.Y + 2),
                            new Point(r.Right - 3, cy),
                            new Point(r.X + 4, r.Bottom - 2) });
                        break;
                    case 5:
                        g.DrawEllipse(p, r.X + 1, r.Y + 1, r.Width - 2, r.Height - 2);
                        g.DrawEllipse(p, cx - 4, r.Y + 1, 8, r.Height - 2);
                        g.DrawLine(p, r.X + 1, cy, r.Right - 1, cy);
                        break;
                    case 6:
                        g.DrawPolygon(p, new Point[] {
                            new Point(cx, r.Y + 1),
                            new Point(r.Right - 2, r.Y + 5),
                            new Point(r.Right - 3, r.Bottom - 6),
                            new Point(cx, r.Bottom - 1),
                            new Point(r.X + 3, r.Bottom - 6),
                            new Point(r.X + 2, r.Y + 5) });
                        break;
                    case 7:
                        g.DrawRectangle(p, r.X + 2, r.Y + 2, r.Width - 4, r.Height - 4);
                        g.DrawRectangle(p, r.X + 6, r.Y + 2, r.Width - 12, 5);
                        g.DrawRectangle(p, r.X + 5, r.Bottom - 7, r.Width - 10, 5);
                        break;
                    case 8:
                        g.DrawEllipse(p, r.X + 1, r.Y + 5, 7, 7);
                        g.DrawLine(p, r.X + 8, cy - 1, r.Right - 2, cy - 1);
                        g.DrawLine(p, r.Right - 5, cy - 1, r.Right - 5, cy + 4);
                        break;
                    case 9:
                        g.DrawArc(p, cx - 8, cy - 7, 16, 16, -62, 304);
                        g.DrawLine(p, cx, r.Y, cx, cy);
                        break;
                    case 10:
                        g.DrawLine(p, cx - 7, cy - 7, cx + 7, cy + 7);
                        g.DrawLine(p, cx + 7, cy - 7, cx - 7, cy + 7);
                        break;
                }
            }
        }

        private static GraphicsPath ExRounded(Rectangle r, int radius)
        {
            GraphicsPath p = new GraphicsPath();
            int d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            if (d <= 1) { p.AddRectangle(r); return p; }
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        private static void ExRoundRegion(Control c, int radius)
        {
            if (c.Width < 2 || c.Height < 2) return;
            using (GraphicsPath p = ExRounded(new Rectangle(0, 0, c.Width, c.Height), radius))
            {
                Region old = c.Region;
                c.Region = new Region(p);
                if (old != null) old.Dispose();
            }
        }
    }
}