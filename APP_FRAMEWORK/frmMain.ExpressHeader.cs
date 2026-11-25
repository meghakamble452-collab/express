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
        private Label exClockCap;
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
        private int exBarH = 150;

        private readonly List<Control> exNav = new List<Control>();
        private readonly List<Panel> exSwitchRow = new List<Panel>();
        private readonly List<Control> exPills = new List<Control>();
        private readonly Dictionary<Panel, CheckBox> exSwitchMap = new Dictionary<Panel, CheckBox>();
        private readonly Dictionary<Panel, int> exBookIndex = new Dictionary<Panel, int>();
        private readonly Dictionary<Control, float> exHov = new Dictionary<Control, float>();
        private readonly Dictionary<string, Font> exFontCache = new Dictionary<string, Font>();

        private bool exDrag;
        private int exDragX, exDragY;

        // Express header palette: light white -> soft blue body, dark ink text, bhagwa (saffron) highlights.
        private static readonly Color ExBlue = Color.FromArgb(37, 99, 235);
        private static readonly Color ExNavyA = Color.FromArgb(10, 22, 52);
        private static readonly Color ExNavyB = Color.FromArgb(24, 50, 112);
        private static readonly Color ExBgA = Color.FromArgb(255, 255, 255);
        private static readonly Color ExBgB = Color.FromArgb(221, 233, 251);
        private static readonly Color ExEdge = Color.FromArgb(168, 188, 222);
        private static readonly Color ExInk = Color.FromArgb(15, 34, 78);
        private static readonly Color ExSaffron = Color.FromArgb(249, 140, 22);
        private static readonly Color ExSaffronL = Color.FromArgb(253, 186, 92);
        private static readonly Color ExSaffronD = Color.FromArgb(214, 96, 4);
        private static readonly Color ExMuted = Color.FromArgb(100, 116, 139);
        private static readonly Color ExGreen = Color.FromArgb(22, 163, 74);
        private static readonly Color ExBoxBg = Color.White;

        private static readonly Color ExCUser = Color.FromArgb(99, 102, 241);
        private static readonly Color ExCIrctc = Color.FromArgb(124, 58, 237);
        private static readonly Color ExCBank = Color.FromArgb(13, 148, 136);
        private static readonly Color ExCNew = Color.FromArgb(234, 108, 10);
        private static readonly Color ExCProc = Color.FromArgb(37, 99, 235);
        private static readonly Color ExCProxy = Color.FromArgb(8, 145, 178);
        private static readonly Color ExCBypass = Color.FromArgb(219, 39, 119);
        private static readonly Color ExCBackup = Color.FromArgb(71, 85, 105);
        private static readonly Color ExCToken = Color.FromArgb(8, 145, 178);
        private static readonly Color ExCPower = Color.FromArgb(234, 108, 10);
        private static readonly Color ExCRed = Color.FromArgb(220, 38, 38);
        private static readonly Color ExKeyCol = Color.FromArgb(255, 226, 148);
        private int exLastHalf = -1;
        private int exValLevel;   // VALIDITY wording detail chosen by the layout (0 = full words)

        private static void ExBuffer(Control c)
        {
            try
            {
                typeof(Control).GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(c, true, null);
            }
            catch { }
        }

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
            ExBuffer(exCanvas);
            exCanvas.Paint += ExCanvasPaint;
            exCanvas.MouseDown += ExCanvasDown;
            exCanvas.MouseMove += ExCanvasMove;
            exCanvas.MouseUp += (s, e) => exDrag = false;
            exBar.Controls.Add(exCanvas);
            exCanvas.BringToFront();

            exLogo = new Panel();
            exLogo.BackColor = Color.Transparent;   // rounded tile is painted with AA; no square corners
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
                exModeBox.BackColor = Color.Transparent;
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
            ExBuffer(exInfo);
            exInfo.Paint += ExInfoPaint;
            exCanvas.Controls.Add(exInfo);

            exChId = ExInfoChip("USER ID", Convert.ToString(Program.clientID) + (Program.IsDemo ? "   •   DEMO" : ""), Color.White);
            exChVer = ExInfoChip("VERSION", Program.appversion, Color.White);
            exChPay = ExInfoChip("PAYMENT", string.IsNullOrEmpty(Program.KeyStatus) ? "—" : Program.KeyStatus,
                Program.KeyStatus == "Paid" ? Color.FromArgb(198, 255, 220) : Color.FromArgb(255, 208, 176));
            exChVal = ExInfoChip("VALIDITY", "—", Color.White);

            exValidityStart = DateTime.Now;
            exValiditySpan = ExLeftSpan();
            exChVal.Tag = new object[] { "VALIDITY", ExFmtLeft(exValiditySpan), Color.White };

            exDateFont = ExCached("Segoe UI Semibold", 15f, FontStyle.Bold, exK);

            exClockPill = new Panel();
            exClockPill.BackColor = Color.Transparent;
            ExBuffer(exClockPill);
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

            exClockCap = new Label();
            exClockCap.AutoSize = false;
            exClockCap.BackColor = Color.White;
            exClockCap.ForeColor = Color.FromArgb(104, 132, 168);
            exClockCap.TextAlign = ContentAlignment.MiddleLeft;
            exClockCap.Text = "Current Time";
            exClockPill.Controls.Add(exClockCap);

            // The LED clock is painted directly (7-segment digits); the text labels stay as value holders only.
            exDateTxt.Visible = false;
            exTimeTxt.Visible = false;
            exClockCap.Visible = false;

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
                ExRoundRegion(this, rad);   // the window itself still needs a shape; its edge is masked by an AA border in ExCanvasPaint
                // Everything inside is painted with anti-aliased rounded shapes on transparent panels -> no Region staircase.
                ExClearRegion(exCanvas);
                ExClearRegion(exModeBox);
                ExClearRegion(exClockPill);
                foreach (Control c in exPills) ExClearRegion(c);
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
                exBarH = ExS(134, exK);   // nav buttons end a hair above the bottom edge

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
            ExBuffer(p);
            p.Paint += ExInfoChipPaint;
            exInfo.Controls.Add(p);
            return p;
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
            if (t.TotalDays >= 1) return ((int)t.TotalDays) + "d : " + t.ToString(@"hh\:mm\:ss");
            if (t.TotalHours >= 1) return t.ToString(@"hh\:mm\:ss");
            if (t.TotalMinutes >= 1) return t.ToString(@"mm\:ss") + " min";
            return ((int)t.TotalSeconds) + " sec";
        }

        private Panel ExPill(string text, int kind, Color accent, int icon, int w, int h)
        {
            Panel p = new Panel();
            p.Size = new Size(ExS(w, exK), ExS(h, exK));
            p.BackColor = Color.Transparent;   // colour comes from Tag; parent shows through the AA corners
            p.Cursor = Cursors.Hand;
            p.Tag = new object[] { text, kind, accent, icon };
            p.Paint += ExPillPaint;
            ExBuffer(p);
            exHov[p] = 0f;
            exPills.Add(p);
            // no Region: the transparent panel lets the anti-aliased rounded face show smooth corners
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

                if (exLogo != null) exLogo.Bounds = new Rectangle(ExS(14, k), ExS(13, k), ExS(56, k), ExS(56, k));

                int rowY = ExS(86, k);
                int rowH = ExS(44, k);

                if (exSwitchRow.Count > 0)
                {
                    exSwitchRow[0].Bounds = new Rectangle(ExS(16, k), rowY, ExS(150, k), rowH);
                    if (exSwitchRow[0].Controls.Count > 0)
                    {
                        exSwitchRow[0].Controls[0].Size = new Size(ExS(44, k), ExS(24, k));
                        exSwitchRow[0].Controls[0].Location = new Point(ExS(100, k), (rowH - ExS(24, k)) / 2);
                    }
                }

                if (exModeBox != null)
                {
                    int boxW = ExS(24, k) + Math.Max(1, exSwitchRow.Count - 1) * ExS(74, k);
                    int capW = TextRenderer.MeasureText("BOOKING MODE", ExCached("Segoe UI", 10f, FontStyle.Bold, k)).Width + ExS(22, k);
                    boxW = Math.Max(boxW, capW);
                    exModeBox.Bounds = new Rectangle(ExS(16, k) + ExS(150, k) + ExS(14, k), rowY, boxW, rowH);

                    for (int i = 1; i < exSwitchRow.Count; i++)
                    {
                        Panel r = exSwitchRow[i];
                        int rowsW = (exSwitchRow.Count - 1) * ExS(74, k) - ExS(4, k);
                        int ox = Math.Max(ExS(6, k), (boxW - rowsW) / 2);
                        r.Bounds = new Rectangle(ox + (i - 1) * ExS(74, k), ExS(19, k), ExS(70, k), ExS(22, k));
                        if (r.BackColor != Color.Transparent) r.BackColor = Color.Transparent;
                        if (r.Controls.Count > 0)
                        {
                            if (r.Controls[0].BackColor != Color.Transparent) r.Controls[0].BackColor = Color.Transparent;
                            r.Controls[0].Size = new Size(ExS(42, k), ExS(22, k));
                            r.Controls[0].Location = new Point(ExS(14, k), 0);
                        }
                    }
                }

                int topY = ExS(21, k);
                int btnH = ExS(40, k);
                int closeW = ExS(46, k);
                int gap = ExS(8, k);
                int right = W - ExS(18, k);

                int closeX = right - closeW;
                int powerX = closeX - gap - closeW;
                int userW = ExS(124, k);

                bool hasUser = false;
                foreach (Control c in exCanvas.Controls)
                {
                    if (c.Tag is object[] tt && (int)tt[1] == 1) { hasUser = true; break; }
                }

                int userX = hasUser ? (powerX - gap - ExS(24, k) - gap - userW) : powerX;
                int infoRight = hasUser ? userX : powerX;

                int clockX = ExS(318, k);

                if (exClockPill != null)
                    exClockPill.Bounds = new Rectangle(clockX, ExS(8, k), ExClockWidth(k), ExS(66, k));

                // Clock sits on the LEFT (right after the brand); user info is right-aligned just before power/close.
                int minInfoX = clockX + (exClockPill != null ? exClockPill.Width : 0) + ExS(18, k);

                if (exInfo != null)
                {
                    Panel[] chips = { exChId, exChVal, exChVer, exChPay };
                    int lead = ExS(46, k), chipGap = ExS(7, k), tail = ExS(9, k);
                    int infoEnd = infoRight - ExS(14, k);

                    // pick the most detailed VALIDITY wording that still fits (full words -> Yrs/Mos -> D/h/m/s -> no secs)
                    int infoWant = 0;
                    for (exValLevel = 0; exValLevel <= 3; exValLevel++)
                    {
                        infoWant = lead + tail;
                        int cnt = 0;
                        foreach (Panel cp in chips) if (cp != null) { infoWant += ExChipW(cp); cnt++; }
                        if (cnt > 1) infoWant += (cnt - 1) * chipGap;
                        if (infoEnd - infoWant >= minInfoX) break;
                    }
                    if (exValLevel > 3) exValLevel = 3;

                    int infoX = Math.Max(minInfoX, infoEnd - infoWant);
                    int maxRel = Math.Max(ExS(160, k), infoEnd - infoX);

                    int cxp = lead;
                    int chipTop = ExS(7, k);
                    int chipH = ExS(52, k);
                    foreach (Panel cp in chips)
                    {
                        if (cp == null) continue;
                        int cwp = ExChipW(cp);
                        if (cxp + cwp > maxRel - tail)
                            cwp = Math.Max(ExS(52, k), maxRel - tail - cxp);
                        cp.Bounds = new Rectangle(cxp, chipTop, cwp, chipH);
                        cp.Region = null;   // smooth AA corners are painted; no jagged Region
                        cxp += cwp + chipGap;
                    }

                    int infoW = Math.Min(cxp - chipGap + tail, maxRel);
                    exInfo.Bounds = new Rectangle(infoX, ExS(8, k), Math.Max(ExS(160, k), infoW), ExS(66, k));
                }

                foreach (Control c in exCanvas.Controls)
                {
                    if (!(c.Tag is object[] t)) continue;
                    int kind = (int)t[1];
                    if (kind == 1) c.Bounds = new Rectangle(userX, topY, userW, btnH);
                    if (kind == 2) c.Bounds = new Rectangle(powerX, topY, closeW, btnH);
                    if (kind == 3) c.Bounds = new Rectangle(closeX, topY, closeW, btnH);
                }

                int n = exNav.Count;
                if (n > 0)
                {
                    int startX = ExS(16, k) + ExS(150, k) + ExS(14, k) + (exModeBox != null ? exModeBox.Width : 0) + ExS(16, k);
                    int g2 = ExS(8, k);
                    int minW = ExS(140, k);
                    int avail = Math.Max(n * minW, W - startX - ExS(16, k));
                    int bw = Math.Max(minW, (avail - (n - 1) * g2) / n);
                    int x = startX;
                    foreach (Control b in exNav)
                    {
                        b.Bounds = new Rectangle(x, rowY, bw, rowH);
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

                    // LED clock repaints every half second (digits + blinking colon)
                    int half = (int)(now.Ticks / (TimeSpan.TicksPerSecond / 2));
                    if (half != exLastHalf && exClockPill != null)
                    {
                        exLastHalf = half;
                        exClockPill.Invalidate();
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
                {
                    using (SolidBrush b = new SolidBrush(Color.FromArgb(15, 23, 42)))
                        e.Graphics.FillPath(b, path);
                    using (Pen pen = new Pen(ExSaffron, 1f))
                        e.Graphics.DrawPath(pen, path);
                }
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
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                int w = exCanvas.Width;
                int h = exCanvas.Height;
                if (w < 12 || h < 12) return;

                // Corner area: same colour as the border, so the window's own (Region) staircase
                // blends into the anti-aliased outline drawn below instead of showing steps.
                using (SolidBrush bg = new SolidBrush(ExEdge))
                    g.FillRectangle(bg, new Rectangle(0, 0, w, h));

                Rectangle r = new Rectangle(0, 0, w - 1, h - 1);
                int rad = ExS(16, k);

                // light body: white -> soft blue
                using (GraphicsPath path = ExRounded(r, rad))
                {
                    using (LinearGradientBrush lb = new LinearGradientBrush(r, ExBgA, ExBgB, 0f))
                    {
                        ColorBlend cbg = new ColorBlend();
                        cbg.Positions = new[] { 0f, 0.45f, 1f };
                        cbg.Colors = new[] { ExBgA, Color.FromArgb(240, 245, 253), ExBgB };
                        lb.InterpolationColors = cbg;
                        g.FillPath(lb, path);
                    }

                    Region old = g.Clip;
                    g.SetClip(path, CombineMode.Intersect);
                    // warm bhagwa glow behind the brand
                    using (GraphicsPath gp = new GraphicsPath())
                    {
                        gp.AddEllipse(-ExS(120, k), -ExS(110, k), ExS(520, k), ExS(260, k));
                        using (PathGradientBrush pg = new PathGradientBrush(gp))
                        {
                            pg.CenterColor = Color.FromArgb(46, ExSaffron);
                            pg.SurroundColors = new[] { Color.FromArgb(0, ExSaffron) };
                            g.FillPath(pg, gp);
                        }
                    }
                    // soft top sheen
                    using (LinearGradientBrush sh = new LinearGradientBrush(new Rectangle(0, 0, w, ExS(80, k)),
                        Color.FromArgb(120, 255, 255, 255), Color.FromArgb(0, 255, 255, 255), 90f))
                        g.FillRectangle(sh, 0, 0, w, ExS(80, k));
                    // bhagwa top edge
                    using (LinearGradientBrush tb = new LinearGradientBrush(new Rectangle(0, 0, w, 3), ExSaffronD, ExSaffronL, 0f))
                        g.FillRectangle(tb, 0, 0, w, ExS(3, k));
                    g.Clip = old;
                }

                // anti-aliased outline exactly on the window edge (masks the Region stair-steps)
                using (GraphicsPath bp = ExRoundF(new RectangleF(0.5f, 0.5f, w - 1.5f, h - 1.5f), rad))
                using (Pen outer = new Pen(ExEdge, 1.6f))
                    g.DrawPath(outer, bp);
                using (GraphicsPath ip = ExRoundF(new RectangleF(2f, 2f, w - 4.5f, h - 4.5f), Math.Max(4, rad - 2)))
                using (Pen inner = new Pen(Color.FromArgb(200, 255, 255, 255), 1f))
                    g.DrawPath(inner, ip);

                Font fe = ExCached("Segoe UI", 27f, FontStyle.Bold | FontStyle.Italic, k);
                TextRenderer.DrawText(g, "EXPRESS", fe,
                    new Rectangle(ExS(82, k), ExS(13, k), ExS(220, k), ExS(36, k)), ExInk,
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.NoClipping);

                Font fs = ExCached("Segoe UI Semibold", 10.5f, FontStyle.Bold, k);
                TextRenderer.DrawText(g, "PREMIUM  TATKAL  SOFTWARE", fs,
                    new Rectangle(ExS(84, k), ExS(51, k), ExS(230, k), ExS(16, k)), ExSaffronD,
                    TextFormatFlags.NoPadding | TextFormatFlags.SingleLine | TextFormatFlags.NoClipping);

                // divider: bhagwa fading out to both sides
                Rectangle dv = new Rectangle(ExS(16, k), ExS(79, k), w - ExS(32, k), 1);
                using (LinearGradientBrush db = new LinearGradientBrush(new Rectangle(dv.X, dv.Y, dv.Width, 2),
                    Color.FromArgb(0, ExSaffron), Color.FromArgb(0, ExSaffron), 0f))
                {
                    ColorBlend cb = new ColorBlend();
                    cb.Positions = new[] { 0f, 0.25f, 0.75f, 1f };
                    cb.Colors = new[] { Color.FromArgb(190, ExSaffron), Color.FromArgb(110, ExEdge),
                                        Color.FromArgb(80, ExEdge), Color.FromArgb(0, ExEdge) };
                    db.InterpolationColors = cb;
                    g.FillRectangle(db, dv);
                }
            }
            catch { }
        }

        private void ExLogoPaint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                Rectangle r = new Rectangle(0, 0, exLogo.Width, exLogo.Height);
                if (r.Width < 4 || r.Height < 4) return;

                using (GraphicsPath p = ExRoundF(new RectangleF(1, 2, r.Width - 2, r.Height - 3), ExS(14, exK)))
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(40, 30, 64, 120)))
                    g.FillPath(sb, p);
                using (GraphicsPath p = ExRoundF(new RectangleF(1, 1, r.Width - 2.5f, r.Height - 3.5f), ExS(14, exK)))
                {
                    using (SolidBrush b = new SolidBrush(Color.White)) g.FillPath(b, p);
                    using (Pen pen = new Pen(Color.FromArgb(200, ExSaffron), 1.6f)) g.DrawPath(pen, p);
                }

                Image img = exLogo.Tag as Image;
                if (img != null)
                {
                    int box = ExS(40, exK);
                    float sc = Math.Min((float)box / img.Width, (float)box / img.Height);
                    int w = Math.Max(1, (int)(img.Width * sc));
                    int h = Math.Max(1, (int)(img.Height * sc));
                    g.DrawImage(img, new Rectangle((exLogo.Width - w) / 2, (exLogo.Height - h) / 2, w, h));
                }
                else
                {
                    Font f = ExCached("Segoe UI Black", 26f, FontStyle.Bold, exK);
                    TextRenderer.DrawText(g, "E", f, r, ExSaffronD,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
            }
            catch { }
        }

        private void ExModeBoxPaint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, exModeBox.Width - 1, exModeBox.Height - 1);
                if (r.Width < 4 || r.Height < 4) return;

                using (GraphicsPath sp = ExRoundF(new RectangleF(0.5f, 2f, r.Width - 1f, r.Height - 2f), ExS(12, exK)))
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(34, 30, 64, 120))) g.FillPath(sb, sp);
                using (GraphicsPath p = ExRoundF(new RectangleF(0.5f, 0.5f, r.Width - 1f, r.Height - 2f), ExS(12, exK)))
                {
                    using (SolidBrush b = new SolidBrush(ExBoxBg)) g.FillPath(b, p);
                    using (Pen pen = new Pen(Color.FromArgb(170, ExSaffron), 1.2f)) g.DrawPath(pen, p);
                }

                Font f = ExCached("Segoe UI", 10f, FontStyle.Bold, exK);
                TextRenderer.DrawText(g, "BOOKING MODE", f,
                    new Rectangle(ExS(12, exK), ExS(4, exK), r.Width - ExS(16, exK), ExS(14, exK)), ExSaffronD,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
            }
            catch { }
        }

        // ================== 3D LED wall clock (bhagwa 7-segment) ==================

        private int ExClockWidth(float k)
        {
            return ExS(12, k) + ExS(58, k) + ExS(12, k)
                 + 2 * ExS(22, k) + ExS(6, k) + ExS(14, k) + 2 * ExS(22, k) + ExS(6, k)
                 + ExS(10, k) + 2 * ExS(13, k) + ExS(4, k) + ExS(14, k);
        }

        private void ExClockPaint(object sender, PaintEventArgs e)
        {
            try
            {
                float k = exK;
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                int W = exClockPill.Width, H = exClockPill.Height;
                if (W < 20 || H < 20) return;

                DateTime now = DateTime.Now;
                int rad = ExS(12, k);
                Rectangle body = new Rectangle(0, 0, W - 1, H - 1);

                // bezel: glossy black case with a bhagwa rim
                using (GraphicsPath p = ExRounded(body, rad))
                {
                    using (LinearGradientBrush b = new LinearGradientBrush(body, Color.FromArgb(62, 62, 70), Color.FromArgb(8, 8, 11), 90f))
                        g.FillPath(b, p);
                    using (LinearGradientBrush rim = new LinearGradientBrush(body, ExSaffronL, ExSaffronD, 90f))
                    using (Pen pen = new Pen(rim, 1.6f))
                        g.DrawPath(pen, p);
                }

                // recessed LED screen
                int ins = ExS(4, k);
                Rectangle scr = new Rectangle(ins, ins, W - 1 - ins * 2, H - 1 - ins * 2);
                using (GraphicsPath sp = ExRounded(scr, Math.Max(3, rad - ins)))
                {
                    using (LinearGradientBrush sb = new LinearGradientBrush(scr, Color.FromArgb(4, 4, 6), Color.FromArgb(22, 14, 8), 90f))
                        g.FillPath(sb, sp);
                    using (Pen inner = new Pen(Color.FromArgb(150, 0, 0, 0), 1f))
                        g.DrawPath(inner, sp);
                }

                // left column: weekday / date / year
                int x = ExS(12, k);
                int colW = ExS(58, k);
                Font fDay = ExCached("Segoe UI", 13f, FontStyle.Bold, k);
                Font fDate = ExCached("Segoe UI Semibold", 11f, FontStyle.Bold, k);
                Font fYear = ExCached("Segoe UI", 9.5f, FontStyle.Bold, k);
                string day = now.ToString("ddd", System.Globalization.CultureInfo.InvariantCulture).ToUpperInvariant();
                string dm = now.ToString("dd MMM", System.Globalization.CultureInfo.InvariantCulture).ToUpperInvariant();
                TextRenderer.DrawText(g, day, fDay, new Rectangle(x, ExS(9, k), colW, ExS(18, k)), Color.FromArgb(255, 190, 80),
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                TextRenderer.DrawText(g, dm, fDate, new Rectangle(x, ExS(27, k), colW, ExS(16, k)), Color.FromArgb(255, 150, 40),
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                TextRenderer.DrawText(g, now.Year.ToString(), fYear, new Rectangle(x, ExS(43, k), colW, ExS(14, k)), Color.FromArgb(170, 110, 40),
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                x += colW;

                using (Pen sep = new Pen(Color.FromArgb(70, ExSaffron), 1f))
                    g.DrawLine(sep, x + ExS(5, k), ExS(12, k), x + ExS(5, k), H - ExS(12, k));
                x += ExS(12, k);

                // HH:MM big, SS small (24h, same as the old clock)
                float bw = ExS(22, k), bh = ExS(40, k), bt = Math.Max(3f, 5f * k);
                float sw = ExS(13, k), sh = ExS(23, k), st = Math.Max(2f, 3.2f * k);
                float top = (H - bh) / 2f;
                bool colonOn = now.Millisecond < 500;

                string hh = now.ToString("HH"), mm = now.ToString("mm"), ss = now.ToString("ss");
                ExSegDigit(g, new RectangleF(x, top, bw, bh), hh[0] - '0', bt);
                x += (int)bw + ExS(6, k);
                ExSegDigit(g, new RectangleF(x, top, bw, bh), hh[1] - '0', bt);
                x += (int)bw;

                // colon
                float cx = x + ExS(7, k);
                float dot = Math.Max(3f, 5f * k);
                ExSegDot(g, new RectangleF(cx - dot / 2 + 1, top + bh * 0.30f - dot / 2, dot, dot), colonOn);
                ExSegDot(g, new RectangleF(cx - dot / 2 - 1, top + bh * 0.70f - dot / 2, dot, dot), colonOn);
                x += ExS(14, k);

                ExSegDigit(g, new RectangleF(x, top, bw, bh), mm[0] - '0', bt);
                x += (int)bw + ExS(6, k);
                ExSegDigit(g, new RectangleF(x, top, bw, bh), mm[1] - '0', bt);
                x += (int)bw + ExS(10, k);

                // seconds: smaller, bottom aligned, with a tiny caption above
                float stop = top + bh - sh;
                Font fCap = ExCached("Segoe UI", 8.5f, FontStyle.Bold, k);
                TextRenderer.DrawText(g, "SEC", fCap, new Rectangle(x, (int)top - 1, (int)(sw * 2 + ExS(4, k)), ExS(12, k)),
                    Color.FromArgb(190, 120, 40), TextFormatFlags.HorizontalCenter | TextFormatFlags.Top | TextFormatFlags.NoPadding);
                ExSegDigit(g, new RectangleF(x, stop, sw, sh), ss[0] - '0', st);
                x += (int)sw + ExS(4, k);
                ExSegDigit(g, new RectangleF(x, stop, sw, sh), ss[1] - '0', st);

                // glass reflection over the whole face
                using (GraphicsPath gl = new GraphicsPath())
                {
                    gl.AddBezier(scr.X, scr.Y + scr.Height * 0.55f, scr.X + scr.Width * 0.35f, scr.Y + scr.Height * 0.25f,
                                 scr.X + scr.Width * 0.7f, scr.Y + scr.Height * 0.2f, scr.Right, scr.Y + scr.Height * 0.12f);
                    gl.AddLine(scr.Right, scr.Y + scr.Height * 0.12f, scr.Right, scr.Y);
                    gl.AddLine(scr.Right, scr.Y, scr.X, scr.Y);
                    gl.CloseFigure();
                    Region old = g.Clip;
                    using (GraphicsPath sp = ExRounded(scr, Math.Max(3, rad - ins)))
                        g.SetClip(sp, CombineMode.Intersect);
                    using (LinearGradientBrush gb = new LinearGradientBrush(scr, Color.FromArgb(34, 255, 255, 255), Color.FromArgb(0, 255, 255, 255), 90f))
                        g.FillPath(gb, gl);
                    g.Clip = old;
                }
            }
            catch { }
        }

        private static readonly int[] ExSegMask = { 0x3F, 0x06, 0x5B, 0x4F, 0x66, 0x6D, 0x7D, 0x07, 0x7F, 0x6F };

        // One 7-segment digit (slightly italic like a real LED clock), lit segments glow bhagwa.
        private static void ExSegDigit(Graphics g, RectangleF r, int d, float t)
        {
            int mask = (d >= 0 && d <= 9) ? ExSegMask[d] : 0;
            float gp = Math.Max(0.6f, t * 0.16f);
            float x0 = r.X, y0 = r.Y, w = r.Width, h = r.Height, half = h / 2f;
            const float slant = 0.10f;

            Func<float, float, PointF> P = (px, py) => new PointF(px + (r.Bottom - py) * slant, py);

            PointF[][] seg = new PointF[7][];
            seg[0] = ExHSeg(P, x0 + t / 2 + gp, x0 + w - t / 2 - gp, y0 + t / 2, t);            // a
            seg[1] = ExVSeg(P, x0 + w - t / 2, y0 + t / 2 + gp, y0 + half - gp, t);             // b
            seg[2] = ExVSeg(P, x0 + w - t / 2, y0 + half + gp, y0 + h - t / 2 - gp, t);        // c
            seg[3] = ExHSeg(P, x0 + t / 2 + gp, x0 + w - t / 2 - gp, y0 + h - t / 2, t);        // d
            seg[4] = ExVSeg(P, x0 + t / 2, y0 + half + gp, y0 + h - t / 2 - gp, t);             // e
            seg[5] = ExVSeg(P, x0 + t / 2, y0 + t / 2 + gp, y0 + half - gp, t);                 // f
            seg[6] = ExHSeg(P, x0 + t / 2 + gp, x0 + w - t / 2 - gp, y0 + half, t);             // g

            using (SolidBrush off = new SolidBrush(Color.FromArgb(46, 255, 120, 20)))
            using (Pen glow1 = new Pen(Color.FromArgb(38, 255, 140, 20), t * 1.7f))
            using (Pen glow2 = new Pen(Color.FromArgb(70, 255, 150, 30), t * 0.9f))
            using (LinearGradientBrush lit = new LinearGradientBrush(new RectangleF(r.X, r.Y - 1, r.Width + h * slant, r.Height + 2),
                       Color.FromArgb(255, 214, 120), Color.FromArgb(255, 122, 0), 90f))
            {
                glow1.LineJoin = LineJoin.Round; glow2.LineJoin = LineJoin.Round;
                for (int i = 0; i < 7; i++)
                {
                    if ((mask & (1 << i)) == 0) { g.FillPolygon(off, seg[i]); continue; }
                    g.DrawPolygon(glow1, seg[i]);
                    g.DrawPolygon(glow2, seg[i]);
                }
                for (int i = 0; i < 7; i++)
                    if ((mask & (1 << i)) != 0) g.FillPolygon(lit, seg[i]);
            }
        }

        private static PointF[] ExHSeg(Func<float, float, PointF> P, float xa, float xb, float yc, float t)
        {
            float q = t / 2f;
            return new[] { P(xa, yc), P(xa + q, yc - q), P(xb - q, yc - q), P(xb, yc), P(xb - q, yc + q), P(xa + q, yc + q) };
        }

        private static PointF[] ExVSeg(Func<float, float, PointF> P, float xc, float ya, float yb, float t)
        {
            float q = t / 2f;
            return new[] { P(xc, ya), P(xc + q, ya + q), P(xc + q, yb - q), P(xc, yb), P(xc - q, yb - q), P(xc - q, ya + q) };
        }

        private static void ExSegDot(Graphics g, RectangleF r, bool on)
        {
            if (on)
            {
                using (SolidBrush glow = new SolidBrush(Color.FromArgb(60, 255, 140, 20)))
                    g.FillEllipse(glow, RectangleF.Inflate(r, r.Width * 0.6f, r.Height * 0.6f));
                using (LinearGradientBrush b = new LinearGradientBrush(RectangleF.Inflate(r, 0, 1), Color.FromArgb(255, 214, 120), Color.FromArgb(255, 122, 0), 90f))
                    g.FillEllipse(b, r);
            }
            else
            {
                using (SolidBrush b = new SolidBrush(Color.FromArgb(46, 255, 120, 20)))
                    g.FillEllipse(b, r);
            }
        }

        // ================== user info ==================

        private void ExInfoPaint(object sender, PaintEventArgs e)
        {
            try
            {
                Panel p = sender as Panel;
                if (p == null || p.Width < 20 || p.Height < 10) return;
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, p.Width - 1, p.Height - 1);

                using (GraphicsPath sp = ExRoundF(new RectangleF(0.5f, 2f, r.Width - 0.5f, r.Height - 1.5f), ExS(12, exK)))
                using (SolidBrush sb = new SolidBrush(Color.FromArgb(30, 30, 64, 120))) g.FillPath(sb, sp);
                using (GraphicsPath path = ExRoundF(new RectangleF(0.5f, 0.5f, r.Width - 0.5f, r.Height - 1.5f), ExS(12, exK)))
                {
                    using (SolidBrush b = new SolidBrush(Color.FromArgb(235, 255, 255, 255)))
                        g.FillPath(b, path);
                    using (Pen pen = new Pen(Color.FromArgb(150, ExSaffron), 1.2f))
                        g.DrawPath(pen, path);
                }

                // soft bhagwa glow around the VALIDITY tile
                if (exChVal != null && exChVal.Parent == p && exChVal.Visible)
                {
                    Rectangle vb = exChVal.Bounds;
                    for (int i = 4; i >= 1; i--)
                    {
                        RectangleF gr = new RectangleF(vb.X - i, vb.Y - i + 1, vb.Width + i * 2 - 1, vb.Height + i * 2 - 1);
                        using (GraphicsPath gp = ExRoundF(gr, ExS(10, exK) + i))
                        using (SolidBrush gb = new SolidBrush(Color.FromArgb(18 + (4 - i) * 10, ExSaffron)))
                            g.FillPath(gb, gp);
                    }
                }

                // account badge
                int d = ExS(30, exK);
                Rectangle bd = new Rectangle(ExS(9, exK), (p.Height - d) / 2, d, d);
                using (LinearGradientBrush bb = new LinearGradientBrush(bd, ExSaffronL, ExSaffronD, 90f))
                    g.FillEllipse(bb, bd);
                ExIcon(g, Rectangle.Inflate(bd, -ExS(6, exK), -ExS(6, exK)), 11, Color.White, exK);
            }
            catch { }
        }

        private static bool ExIsPaid(string v)
        {
            return string.Equals((v ?? "").Trim(), "Paid", StringComparison.OrdinalIgnoreCase);
        }

        private int ExChipW(Panel p)
        {
            object[] m = p.Tag as object[];
            if (m == null) return ExS(90, exK);
            string cap = (string)m[0];
            string val = Convert.ToString(m[1]);
            Font fc = ExCached("Segoe UI", 10f, FontStyle.Bold, exK);
            Font fv = ExCached("Segoe UI Semibold", 17f, FontStyle.Bold, exK);
            Font fb = ExCached("Segoe UI", 12f, FontStyle.Bold, exK);
            int capW = ExS(17, exK) + TextRenderer.MeasureText(cap, fc, Size.Empty, TextFormatFlags.NoPadding).Width;
            int valW = cap == "PAYMENT"
                ? ExS(34, exK) + TextRenderer.MeasureText(val.ToUpperInvariant(), fb, Size.Empty, TextFormatFlags.NoPadding).Width
                : TextRenderer.MeasureText(val, fv, Size.Empty, TextFormatFlags.NoPadding).Width;
            // VALIDITY uses the two-line human format; its width is reserved with "8" digits so the tick never jitters
            if (cap == "VALIDITY") return ExValidityW(val, exValLevel);
            return Math.Max(capW, valW) + ExS(24, exK);
        }

        private void ExInfoChipPaint(object sender, PaintEventArgs e)
        {
            try
            {
                Panel p = sender as Panel;
                if (p == null || p.Width < 10 || p.Height < 8) return;
                object[] m = p.Tag as object[];
                if (m == null) return;

                float k = exK;
                string cap = (string)m[0];
                string val = Convert.ToString(m[1]);
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Rectangle r = new Rectangle(0, 0, p.Width - 1, p.Height - 1);
                bool validity = cap == "VALIDITY";
                bool payment = cap == "PAYMENT";

                using (GraphicsPath path = ExRoundF(new RectangleF(0.5f, 0.5f, p.Width - 1.5f, p.Height - 1.5f), ExS(10, k)))
                {
                    if (validity)
                    {
                        // highlighted: bhagwa gradient tile with a glossy top
                        using (LinearGradientBrush b = new LinearGradientBrush(r, Color.FromArgb(251, 160, 48), ExSaffronD, 90f))
                            g.FillPath(b, path);
                        Region oc = g.Clip;
                        g.SetClip(path, CombineMode.Intersect);
                        using (SolidBrush gl = new SolidBrush(Color.FromArgb(36, 255, 255, 255)))
                            g.FillRectangle(gl, 0, 0, p.Width, p.Height / 2);
                        g.Clip = oc;
                        using (Pen pen = new Pen(Color.FromArgb(194, 84, 4), 1f)) g.DrawPath(pen, path);
                    }
                    else
                    {
                        using (SolidBrush b = new SolidBrush(Color.FromArgb(244, 248, 254))) g.FillPath(b, path);
                        using (Pen pen = new Pen(Color.FromArgb(208, 220, 238), 1f)) g.DrawPath(pen, path);
                    }
                }

                if (validity) { ExPaintValidity(g, p, val); return; }

                int pad = ExS(12, k);
                int icon = cap == "USER ID" ? 1 : validity ? 12 : cap == "VERSION" ? 13 : 14;
                Color capCol = validity ? Color.FromArgb(255, 241, 222) : ExMuted;
                Rectangle ir = new Rectangle(pad, ExS(8, k), ExS(13, k), ExS(13, k));
                ExIcon(g, ir, icon, validity ? Color.White : ExSaffronD, k);

                Font fc = ExCached("Segoe UI", 10f, FontStyle.Bold, k);
                TextRenderer.DrawText(g, cap, fc, new Rectangle(pad + ExS(17, k), ExS(6, k), p.Width - pad * 2, ExS(17, k)), capCol,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);

                Rectangle vr = new Rectangle(pad, ExS(24, k), p.Width - pad * 2 + 2, ExS(24, k));
                if (payment)
                {
                    bool paid = ExIsPaid(val);
                    string up = string.IsNullOrWhiteSpace(val) || val == "—" ? "—" : val.Trim().ToUpperInvariant();
                    bool demo = up.Contains("DEMO") || up.Contains("TRIAL");
                    Color bc = paid ? ExGreen : demo ? Color.FromArgb(217, 119, 6) : ExCRed;
                    Font fb = ExCached("Segoe UI", 12f, FontStyle.Bold, k);
                    int tw = TextRenderer.MeasureText(up, fb, Size.Empty, TextFormatFlags.NoPadding).Width;
                    Rectangle pill = new Rectangle(pad, vr.Y + 1, Math.Min(vr.Width, tw + ExS(32, k)), ExS(22, k));
                    using (GraphicsPath pp = ExRounded(pill, pill.Height / 2))
                    {
                        using (LinearGradientBrush pb = new LinearGradientBrush(pill, ExMix(bc, Color.White, 0.18f), bc, 90f))
                            g.FillPath(pb, pp);
                        using (Pen pen = new Pen(Color.FromArgb(150, 255, 255, 255), 1f)) g.DrawPath(pen, pp);
                    }
                    int cd = ExS(14, k);
                    Rectangle ci = new Rectangle(pill.X + ExS(5, k), pill.Y + (pill.Height - cd) / 2, cd, cd);
                    using (SolidBrush wb = new SolidBrush(Color.White)) g.FillEllipse(wb, ci);
                    ExIcon(g, Rectangle.Inflate(ci, -ExS(2, k), -ExS(2, k)), paid ? 15 : 16, bc, k);
                    TextRenderer.DrawText(g, up, fb, new Rectangle(ci.Right + ExS(5, k), pill.Y, pill.Right - ci.Right - ExS(7, k), pill.Height),
                        Color.White, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                }
                else
                {
                    Font fv = ExCached("Segoe UI Semibold", 17f, FontStyle.Bold, k);
                    Color vc = validity ? Color.White : cap == "USER ID" ? ExInk : Color.FromArgb(194, 65, 12);
                    TextRenderer.DrawText(g, val, fv, vr, vc,
                        TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding |
                        TextFormatFlags.SingleLine | TextFormatFlags.EndEllipsis);
                }
            }
            catch { }
        }

        // ================== VALIDITY: human calendar format (display only) ==================
        // The value string still comes from the existing ExFmtLeft / ExValidityTickTick logic ("Nd : hh:mm:ss",
        // "hh:mm:ss", "mm:ss min", "N sec", "Expired"); it is only parsed and re-presented here.

        private static TimeSpan? ExParseLeft(string v)
        {
            v = (v ?? "").Trim();
            if (v.Length == 0 || v == "—") return null;
            if (v.StartsWith("Exp", StringComparison.OrdinalIgnoreCase)) return TimeSpan.Zero;
            try
            {
                System.Text.RegularExpressions.Match m;
                m = System.Text.RegularExpressions.Regex.Match(v, @"^(\d+)\s*d\s*:\s*(\d+):(\d+):(\d+)$");
                if (m.Success) return new TimeSpan(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value));
                m = System.Text.RegularExpressions.Regex.Match(v, @"^(\d+):(\d+):(\d+)$");
                if (m.Success) return new TimeSpan(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value));
                m = System.Text.RegularExpressions.Regex.Match(v, @"^(\d+):(\d+)\s*min$");
                if (m.Success) return new TimeSpan(0, int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value));
                m = System.Text.RegularExpressions.Regex.Match(v, @"^(\d+)\s*sec$");
                if (m.Success) return TimeSpan.FromSeconds(int.Parse(m.Groups[1].Value));
            }
            catch { }
            return null;
        }

        // Calendar difference from now: add whole years, then whole months; the rest is days + time.
        private static void ExCalParts(TimeSpan span, out int y, out int mo, out int d, out int h, out int mi, out int s)
        {
            DateTime now = DateTime.Now;
            DateTime exp = now + span;
            y = 0;
            while (y < 200 && now.AddYears(y + 1) <= exp) y++;
            DateTime cur = now.AddYears(y);
            mo = 0;
            while (mo < 12 && cur.AddMonths(mo + 1) <= exp) mo++;
            cur = cur.AddMonths(mo);
            TimeSpan rest = exp - cur;
            if (rest < TimeSpan.Zero) rest = TimeSpan.Zero;
            d = rest.Days; h = rest.Hours; mi = rest.Minutes; s = rest.Seconds;
        }

        // token kinds: 0 = big number (time), 1 = big-row unit, 2 = big-row separator,
        //              4 = small number (days part), 5 = small unit, 6 = small separator
        private static void ExTokS(List<KeyValuePair<string, int>> t, int n, string one, string many)
        {
            t.Add(new KeyValuePair<string, int>(n.ToString(), 4));
            t.Add(new KeyValuePair<string, int>(n == 1 ? one : many, 5));
        }

        private const string ExValCaption = "VALIDITY LEFT";

        // Value row: small calendar part first ("16 Days" / "9 Yrs 11 Mos · 27 Days"), then the BIG time.
        // level 0 = units on the time (23 Hr : 59 Min : 19 Sec), 1 = big "23:59:19", 2 = compact date "9Y 11M 27D", 3 = no seconds.
        private static List<KeyValuePair<string, int>> ExValidityLine(string val, int level, bool widest)
        {
            List<KeyValuePair<string, int>> t = new List<KeyValuePair<string, int>>();
            TimeSpan? sp = ExParseLeft(val);
            if (sp == null) { t.Add(new KeyValuePair<string, int>(string.IsNullOrEmpty(val) ? "—" : val, 0)); return t; }
            if (sp.Value <= TimeSpan.Zero) { t.Add(new KeyValuePair<string, int>("Expired", 0)); return t; }

            int y, mo, d, h, mi, s;
            ExCalParts(sp.Value, out y, out mo, out d, out h, out mi, out s);

            if (level <= 1)
            {
                if (y > 0) ExTokS(t, y, "Yr", "Yrs");
                if (y > 0 || mo > 0) ExTokS(t, mo, "Mo", "Mos");
                if (y > 0 || mo > 0) t.Add(new KeyValuePair<string, int>("·", 6));
                if (y > 0 || mo > 0 || d > 0) ExTokS(t, d, "Day", "Days");
            }
            else
            {
                if (y > 0) { t.Add(new KeyValuePair<string, int>(y.ToString(), 4)); t.Add(new KeyValuePair<string, int>("Y", 5)); }
                if (y > 0 || mo > 0) { t.Add(new KeyValuePair<string, int>(mo.ToString(), 4)); t.Add(new KeyValuePair<string, int>("M", 5)); }
                if (y > 0 || mo > 0 || d > 0) { t.Add(new KeyValuePair<string, int>(d.ToString(), 4)); t.Add(new KeyValuePair<string, int>("D", 5)); }
            }

            if (level == 0)
            {
                t.Add(new KeyValuePair<string, int>(h.ToString("00"), 0)); t.Add(new KeyValuePair<string, int>("Hr", 1));
                t.Add(new KeyValuePair<string, int>(":", 2));
                t.Add(new KeyValuePair<string, int>(mi.ToString("00"), 0)); t.Add(new KeyValuePair<string, int>("Min", 1));
                t.Add(new KeyValuePair<string, int>(":", 2));
                t.Add(new KeyValuePair<string, int>(s.ToString("00"), 0)); t.Add(new KeyValuePair<string, int>("Sec", 1));
            }
            else
            {
                t.Add(new KeyValuePair<string, int>(h.ToString("00"), 0));
                t.Add(new KeyValuePair<string, int>(":", 2));
                t.Add(new KeyValuePair<string, int>(mi.ToString("00"), 0));
                if (level <= 2)
                {
                    t.Add(new KeyValuePair<string, int>(":", 2));
                    t.Add(new KeyValuePair<string, int>(s.ToString("00"), 0));
                }
            }

            if (widest)   // reserve width with "8" digits so the per-second tick never changes the width
            {
                for (int i = 0; i < t.Count; i++)
                {
                    if (t[i].Value != 0 && t[i].Value != 4) continue;
                    string w = System.Text.RegularExpressions.Regex.Replace(t[i].Key, "[0-9]", "8");
                    if (w.Length < 2 && char.IsDigit(t[i].Key, 0)) w = "88";
                    t[i] = new KeyValuePair<string, int>(w, t[i].Value);
                }
            }
            return t;
        }

        private static string ExValidityTill(string val)
        {
            TimeSpan? sp = ExParseLeft(val);
            if (sp == null || sp.Value <= TimeSpan.Zero) return null;
            return "till " + (DateTime.Now + sp.Value).ToString("dd MMM yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }

        // Whole-pixel GDI fonts (ExCached rounds to integer px) in heavy weights for crisp ClearType.
        private Font ExValFont(int kind)
        {
            switch (kind)
            {
                case 0: return ExCached("Segoe UI", 18f, FontStyle.Bold, exK);           // big time digits
                case 2: return ExCached("Segoe UI", 15f, FontStyle.Bold, exK);           // big ":"
                case 4: return ExCached("Segoe UI", 12f, FontStyle.Bold, exK);           // days part digits
                default: return ExCached("Segoe UI Semibold", 10f, FontStyle.Regular, exK); // units / small separators
            }
        }

        private int ExTokGap(int a, int b)
        {
            bool aSmall = a >= 4, bSmall = b >= 4;
            if (aSmall && !bSmall) return ExS(10, exK);                 // days part -> big time
            if ((a == 0 && b == 1) || (a == 4 && b == 5)) return ExS(2, exK);
            if (a == 2 || b == 2 || a == 6 || b == 6) return ExS(4, exK);
            return ExS(5, exK);
        }

        private int ExTokW(List<KeyValuePair<string, int>> t)
        {
            int w = 0;
            for (int i = 0; i < t.Count; i++)
            {
                w += TextRenderer.MeasureText(t[i].Key, ExValFont(t[i].Value), Size.Empty, TextFormatFlags.NoPadding).Width;
                if (i < t.Count - 1) w += ExTokGap(t[i].Value, t[i + 1].Value);
            }
            return w;
        }

        private int ExValCapW(string val, int level)
        {
            Font fc = ExCached("Segoe UI", 10f, FontStyle.Bold, exK);
            int w = ExS(17, exK) + TextRenderer.MeasureText(ExValCaption, fc, Size.Empty, TextFormatFlags.NoPadding).Width;
            string till = level <= 1 ? ExValidityTill(val) : null;
            if (till != null)
                w += ExS(14, exK) + TextRenderer.MeasureText(System.Text.RegularExpressions.Regex.Replace(till, "[0-9]", "8"),
                    ExCached("Segoe UI Semibold", 10f, FontStyle.Regular, exK), Size.Empty, TextFormatFlags.NoPadding).Width;
            return w;
        }

        private int ExValidityW(string val, int level)
        {
            int lineW = ExTokW(ExValidityLine(val, level, true));
            return ExS(12, exK) + Math.Max(ExValCapW(val, level), lineW) + ExS(14, exK);
        }

        private void ExPaintValidity(Graphics g, Panel p, string val)
        {
            float k = exK;
            int level = Math.Max(0, Math.Min(3, exValLevel));
            while (level < 3 && ExValidityW(val, level) > p.Width + 1) level++;

            // TextRenderer below draws straight onto this control's GDI surface: no clip, no transform active.
            g.ResetClip();
            g.ResetTransform();

            int pad = ExS(12, k);

            // caption row (same position as USER ID / VERSION / PAYMENT)
            ExIcon(g, new Rectangle(pad, ExS(8, k), ExS(13, k), ExS(13, k)), 12, Color.White, k);
            Font fc = ExCached("Segoe UI", 10f, FontStyle.Bold, k);
            int capW = TextRenderer.MeasureText(ExValCaption, fc, Size.Empty, TextFormatFlags.NoPadding).Width;
            TextRenderer.DrawText(g, ExValCaption, fc, new Rectangle(pad + ExS(17, k), ExS(6, k), capW + 4, ExS(17, k)),
                Color.FromArgb(255, 241, 222), TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);

            string till = level <= 1 ? ExValidityTill(val) : null;
            if (till != null)
            {
                Font ft = ExCached("Segoe UI Semibold", 10f, FontStyle.Regular, k);
                int tw = TextRenderer.MeasureText(till, ft, Size.Empty, TextFormatFlags.NoPadding).Width;
                int tx = p.Width - pad - tw;
                if (tx > pad + ExS(17, k) + capW + ExS(10, k))
                    TextRenderer.DrawText(g, till, ft, new Rectangle(tx, ExS(6, k), tw + 2, ExS(17, k)),
                        Color.FromArgb(255, 222, 180), TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
            }

            // value row: small days part, then the BIG time; everything bottom-aligned on one baseline band
            List<KeyValuePair<string, int>> t = ExValidityLine(val, level, false);
            int x = pad;
            Rectangle band = new Rectangle(pad, ExS(22, k), p.Width - pad, ExS(26, k));
            for (int i = 0; i < t.Count; i++)
            {
                Font f = ExValFont(t[i].Value);
                int w = TextRenderer.MeasureText(t[i].Key, f, Size.Empty, TextFormatFlags.NoPadding).Width;
                Color c;
                switch (t[i].Value)
                {
                    case 0: c = Color.White; break;
                    case 1: c = Color.FromArgb(255, 226, 190); break;
                    case 2: c = Color.FromArgb(255, 214, 170); break;
                    case 4: c = Color.FromArgb(255, 241, 222); break;
                    default: c = Color.FromArgb(255, 214, 170); break;
                }
                TextRenderer.DrawText(g, t[i].Key, f, new Rectangle(x, band.Y, w + 4, band.Height), c,
                    TextFormatFlags.Left | TextFormatFlags.Bottom | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
                x += w;
                if (i < t.Count - 1) x += ExTokGap(t[i].Value, t[i + 1].Value);
            }
        }

        // ================== switches ==================

        private void ExSwitchRowPaint(object sender, PaintEventArgs e)
        {
            try
            {
                Panel row = (Panel)sender;
                CheckBox cb;
                if (!exSwitchMap.TryGetValue(row, out cb)) return;
                if (exBookIndex.ContainsKey(row)) return;

                Font f = ExCached("Segoe UI Semibold", 13f, FontStyle.Bold, exK);
                TextRenderer.DrawText(e.Graphics, cb.Text, f,
                    new Rectangle(0, 0, ExS(96, exK), row.Height), ExInk,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
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

                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                RectangleF r = new RectangleF(0.5f, 0.5f, track.Width - 2f, track.Height - 2f);
                if (r.Width < 4 || r.Height < 4) return;
                bool on = cb.Checked;

                using (GraphicsPath p = ExRoundF(r, r.Height / 2f))
                {
                    if (on)
                    {
                        using (LinearGradientBrush b = new LinearGradientBrush(r, ExSaffronL, ExSaffronD, 90f))
                            g.FillPath(b, p);
                        using (Pen pen = new Pen(ExSaffronD, 1f)) g.DrawPath(pen, p);
                    }
                    else
                    {
                        // OFF: grey track + slate knob, clearly visible on the light bar
                        using (SolidBrush b = new SolidBrush(Color.FromArgb(214, 222, 234))) g.FillPath(b, p);
                        using (Pen pen = new Pen(Color.FromArgb(148, 163, 184), 1f)) g.DrawPath(pen, p);
                    }
                }

                float kd = r.Height - ExS(6, exK);
                float kx = on ? r.Right - kd - ExS(3, exK) : r.X + ExS(3, exK);
                float ky = r.Y + (r.Height - kd) / 2f;
                using (SolidBrush shd = new SolidBrush(Color.FromArgb(60, 0, 0, 0)))
                    g.FillEllipse(shd, kx, ky + 1, kd, kd);
                using (SolidBrush kb = new SolidBrush(on ? Color.White : Color.FromArgb(100, 116, 139)))
                    g.FillEllipse(kb, kx, ky, kd, kd);
            }
            catch { }
        }

        // ================== buttons ==================

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

                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;


                int dy = dn ? 1 : 0;
                Rectangle r = new Rectangle(0, dy, p.Width - 1, p.Height - 3);
                if (r.Width < 6 || r.Height < 6) return;
                int rad = ExS(10, k);
                RectangleF rf = new RectangleF(0.5f, dy + 0.5f, p.Width - 1.5f, p.Height - 3.5f);

                // soft drop shadow (inside the panel bounds, so the transparent corners stay clean)
                if (!dn)
                    using (GraphicsPath sp = ExRoundF(new RectangleF(rf.X, rf.Y + 2f, rf.Width, rf.Height), rad))
                    using (SolidBrush sb = new SolidBrush(Color.FromArgb(hot ? 80 : 50, ExMix(accent, Color.Black, 0.45f))))
                        g.FillPath(sb, sp);

                Color top, bot;
                if (dn) { top = ExMix(accent, Color.Black, 0.18f); bot = ExMix(accent, Color.Black, 0.32f); }
                else
                {
                    top = ExMix(accent, Color.White, 0.20f + 0.14f * h);
                    bot = ExMix(accent, Color.Black, 0.12f - 0.08f * h);
                }

                using (GraphicsPath path = ExRoundF(rf, rad))
                {
                    using (LinearGradientBrush b = new LinearGradientBrush(r, top, bot, 90f))
                        g.FillPath(b, path);

                    Region old = g.Clip;
                    g.SetClip(path, CombineMode.Intersect);
                    // glossy top half
                    using (SolidBrush gl = new SolidBrush(Color.FromArgb(dn ? 10 : 28, 255, 255, 255)))
                        g.FillRectangle(gl, r.X, r.Y, r.Width, r.Height / 2);
                    // hover: bhagwa underline (close keeps a white one)
                    if (hot && !dn)
                    {
                        Color ul = kind == 3 ? Color.White : (kind == 2 ? Color.FromArgb(255, 237, 213) : ExSaffronL);
                        using (SolidBrush u = new SolidBrush(Color.FromArgb((int)(255 * h), ul)))
                            g.FillRectangle(u, r.X, r.Bottom - ExS(3, k), r.Width, ExS(3, k));
                    }
                    g.Clip = old;

                    Color rim = hot ? Color.FromArgb((int)(110 + 120 * h), 255, 255, 255) : Color.FromArgb(60, 255, 255, 255);
                    using (Pen pen = new Pen(rim, hot ? 1.5f : 1f))
                        g.DrawPath(pen, path);
                }

                Color fg = Color.White;

                if (kind == 2 || kind == 3 || text.Length == 0)
                {
                    int isz = ExS(24, k);
                    Rectangle ir = new Rectangle(r.X + (r.Width - isz) / 2, r.Y + (r.Height - isz) / 2, isz, isz);
                    ExIcon(g, ir, icon, fg, k);
                    return;
                }

                int icoSz = ExS(19, k);
                Font f = null;
                int size = Math.Max(9, (int)Math.Round(13.5f * k));
                f = ExCached("Segoe UI Semibold", size, FontStyle.Bold, 1f);
                int maxText = r.Width - icoSz - ExS(30, k);
                while (size > 9 && TextRenderer.MeasureText(text, f, Size.Empty, TextFormatFlags.NoPadding).Width > maxText)
                {
                    size--;
                    f = ExCached("Segoe UI Semibold", size, FontStyle.Bold, 1f);
                }
                int tw = Math.Min(maxText, TextRenderer.MeasureText(text, f, Size.Empty, TextFormatFlags.NoPadding).Width);
                int gw = icoSz + ExS(9, k) + tw;
                int x = r.X + Math.Max(ExS(10, k), (r.Width - gw) / 2);
                Rectangle ic = new Rectangle(x, r.Y + (r.Height - icoSz) / 2, icoSz, icoSz);
                ExIcon(g, ic, icon, fg, k);

                Rectangle tr = new Rectangle(ic.Right + ExS(9, k), r.Y, Math.Max(10, tw + 4), r.Height);
                TextRenderer.DrawText(g, text, f, tr, fg,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);
            }
            catch { }
        }

        // Lucide-style line icons, drawn on a 24-unit grid scaled to the target rectangle (2-unit stroke, round caps).
        private static void ExIcon(Graphics g, Rectangle r, int id, Color col, float k)
        {
            SmoothingMode oldSm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            float s = Math.Min(r.Width, r.Height) / 24f;
            float ox = r.X + (r.Width - 24 * s) / 2f, oy = r.Y + (r.Height - 24 * s) / 2f;
            Func<float, float, PointF> P = (x, y) => new PointF(ox + x * s, oy + y * s);
            Func<float, float, float, float, RectangleF> R = (x, y, w, h) => new RectangleF(ox + x * s, oy + y * s, w * s, h * s);

            using (Pen p = new Pen(col, Math.Max(1.3f, 2f * s)))
            using (SolidBrush b = new SolidBrush(col))
            {
                p.StartCap = LineCap.Round;
                p.EndCap = LineCap.Round;
                p.LineJoin = LineJoin.Round;

                switch (id)
                {
                    case 1: // id-card
                        using (GraphicsPath gp = ExRoundF(R(2, 5, 20, 14), 2 * s)) g.DrawPath(p, gp);
                        g.DrawEllipse(p, R(6, 8.5f, 5, 5));
                        g.DrawLine(p, P(14, 10), P(18, 10));
                        g.DrawLine(p, P(14, 14), P(17, 14));
                        g.DrawArc(p, R(5, 14.5f, 7, 5), 200, 140);
                        break;
                    case 2: // landmark (bank)
                        g.DrawLine(p, P(3, 22), P(21, 22));
                        g.DrawLine(p, P(6, 18), P(6, 11));
                        g.DrawLine(p, P(10, 18), P(10, 11));
                        g.DrawLine(p, P(14, 18), P(14, 11));
                        g.DrawLine(p, P(18, 18), P(18, 11));
                        g.DrawPolygon(p, new[] { P(12, 2), P(20, 7), P(4, 7) });
                        break;
                    case 3: // ticket-plus
                        using (GraphicsPath gp = new GraphicsPath())
                        {
                            gp.AddLine(P(2, 9), P(2, 7));
                            gp.AddArc(R(2, 5, 4, 4), 180, 90);
                            gp.AddLine(P(4, 5), P(20, 5));
                            gp.AddArc(R(18, 5, 4, 4), 270, 90);
                            gp.AddLine(P(22, 7), P(22, 9));
                            gp.AddArc(R(19, 9, 6, 6), 270, -180);
                            gp.AddLine(P(22, 15), P(22, 17));
                            gp.AddArc(R(18, 15, 4, 4), 0, 90);
                            gp.AddLine(P(20, 19), P(4, 19));
                            gp.AddArc(R(2, 15, 4, 4), 90, 90);
                            gp.AddLine(P(2, 17), P(2, 15));
                            gp.AddArc(R(-1, 9, 6, 6), 90, -180);
                            gp.CloseFigure();
                            g.DrawPath(p, gp);
                        }
                        g.DrawLine(p, P(9, 12), P(15, 12));
                        g.DrawLine(p, P(12, 9), P(12, 15));
                        break;
                    case 4: // circle-play
                        g.DrawEllipse(p, R(2, 2, 20, 20));
                        g.DrawPolygon(p, new[] { P(10, 8), P(16, 12), P(10, 16) });
                        break;
                    case 5: // globe
                        g.DrawEllipse(p, R(2, 2, 20, 20));
                        g.DrawEllipse(p, R(7.5f, 2, 9, 20));
                        g.DrawLine(p, P(2, 12), P(22, 12));
                        break;
                    case 6: // shield-check
                        using (GraphicsPath gp = new GraphicsPath())
                        {
                            gp.AddBezier(P(12, 22), P(17, 20), P(20, 16.5f), P(20, 13));
                            gp.AddLine(P(20, 13), P(20, 5.5f));
                            gp.AddLine(P(20, 5.5f), P(12, 2));
                            gp.AddLine(P(12, 2), P(4, 5.5f));
                            gp.AddLine(P(4, 5.5f), P(4, 13));
                            gp.AddBezier(P(4, 13), P(4, 16.5f), P(7, 20), P(12, 22));
                            gp.CloseFigure();
                            g.DrawPath(p, gp);
                        }
                        g.DrawLines(p, new[] { P(9, 12), P(11, 14), P(15, 10) });
                        break;
                    case 7: // database (backup)
                        g.DrawEllipse(p, R(3, 2, 18, 6));
                        g.DrawLine(p, P(3, 5), P(3, 19));
                        g.DrawLine(p, P(21, 5), P(21, 19));
                        g.DrawArc(p, R(3, 9, 18, 6), 0, 180);
                        g.DrawArc(p, R(3, 16, 18, 6), 0, 180);
                        break;
                    case 8: // key
                        g.DrawEllipse(p, R(2, 13, 8, 8));
                        g.DrawLine(p, P(8.6f, 14.4f), P(21, 2));
                        g.DrawLine(p, P(15.5f, 7.5f), P(19, 11));
                        g.DrawLine(p, P(18.5f, 4.5f), P(21, 7));
                        break;
                    case 9: // power
                        g.DrawLine(p, P(12, 2), P(12, 12));
                        g.DrawArc(p, R(3, 4, 18, 18), -50, 280);
                        break;
                    case 10: // x (a touch bolder so the close button reads instantly)
                        using (Pen t = new Pen(col, Math.Max(1.8f, 2.6f * s)))
                        {
                            t.StartCap = LineCap.Round; t.EndCap = LineCap.Round;
                            g.DrawLine(t, P(6, 6), P(18, 18));
                            g.DrawLine(t, P(18, 6), P(6, 18));
                        }
                        break;
                    case 11: // user
                        g.DrawEllipse(p, R(8, 3, 8, 8));
                        g.DrawArc(p, R(4, 14, 16, 14), 180, 180);
                        break;
                    case 12: // timer
                        g.DrawLine(p, P(10, 2), P(14, 2));
                        g.DrawLine(p, P(12, 14), P(15, 11));
                        g.DrawEllipse(p, R(4, 6, 16, 16));
                        break;
                    case 13: // tag
                        g.DrawPolygon(p, new[] { P(2.5f, 2.5f), P(12, 2.5f), P(21.5f, 12), P(12, 21.5f), P(2.5f, 12) });
                        g.FillEllipse(b, R(6, 6, 3, 3));
                        break;
                    case 14: // credit-card
                        using (GraphicsPath gp = ExRoundF(R(2, 5, 20, 14), 2 * s)) g.DrawPath(p, gp);
                        g.DrawLine(p, P(2, 10), P(22, 10));
                        break;
                    case 15: // check
                        using (Pen t = new Pen(col, Math.Max(1.6f, 3f * s)))
                        {
                            t.StartCap = LineCap.Round; t.EndCap = LineCap.Round; t.LineJoin = LineJoin.Round;
                            g.DrawLines(t, new[] { P(20, 6), P(9, 17), P(4, 12) });
                        }
                        break;
                    case 16: // alert
                        using (Pen t = new Pen(col, Math.Max(1.6f, 3f * s)))
                        {
                            t.StartCap = LineCap.Round; t.EndCap = LineCap.Round;
                            g.DrawLine(t, P(12, 5), P(12, 13));
                        }
                        g.FillEllipse(b, R(10.2f, 16.5f, 3.6f, 3.6f));
                        break;
                }
            }
            g.SmoothingMode = oldSm;
        }

        private static GraphicsPath ExRoundF(RectangleF r, float radius)
        {
            GraphicsPath p = new GraphicsPath();
            float d = Math.Min(radius * 2, Math.Min(r.Width, r.Height));
            if (d <= 1) { p.AddRectangle(r); return p; }
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
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

        private static void ExClearRegion(Control c)
        {
            if (c == null || c.Region == null) return;
            Region old = c.Region;
            c.Region = null;
            old.Dispose();
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