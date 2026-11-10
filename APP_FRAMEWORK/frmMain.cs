using Booking;
using Bunifu.Framework.UI;
using BunifuAnimatorNS;
using ByteNova;
using Ionic.Zip;
using Microsoft.Playwright;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Newtonsoft.Json;
using NEXUS;
using NEXUS.APP_FRAMEWORK;
using NEXUS.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APP_FRAMEWORK;

public class frmMain : Form
{
    internal delegate void Delegate12(int statusCode, string cntrlName);

    public static Point App_Loc;

    private int int_0;

    private static List<TCntrl> list_0;

    public static bool bool_0;

    public static bool bool_news;

    public static bool chAuto;

    public static int int_3;

    public static int int_4;

    public static int PWid;

    public static int PHei;

    public static int PWid2;

    private bool bool_1;

    private string string_0;

    private bool mProcess;

    private bool isChrome;

    private bool isExtensionActive;

    private HttpListener _cookieListener;

    private bool chBeta;

    private IContainer B3AAA;

    private Button C3AA0;

    private Label lblUsrMsg;

    private BunifuTransition bunifuTransition_0;

    private BunifuElipse bunifuElipse_0;

    private BunifuDragControl bunifuDragControl_0;

    private ImageList imageList_0;

    private Panel pnlHeader;

    private BunifuImageButton btnClose;

    private BunifuTransition bunifuTransition_1;

    private BunifuCustomLabel lbl_Date;

    private MenuStrip menuStrip1;

    private ToolStripMenuItem B4AAA;

    private ToolStripMenuItem C4AA0;

    private ToolStripMenuItem aboutUSToolStripMenuItem;

    public ToolStripMenuItem AutoFillCaptchaToolStripMenuItem;

    public ToolStripMenuItem AutoSubmitCaptchaAfterFill;

    private ToolStripMenuItem googleOCRToolStripMenuItem;

    private ToolStripMenuItem showHistoryToolStripMenuItem;

    private System.Windows.Forms.Timer timer_0;

    private ToolStripMenuItem extractReCaptchaExtensionToolStripMenuItem;

    private ToolStripMenuItem clearCacheToolStripMenuItem;

    private ToolStripMenuItem B6AAA;

    private ToolStripMenuItem C6AA0;

    private ToolStripMenuItem uPIPayByOtherIDToolStripMenuItem;

    private IContainer components;

    private BunifuFlatButton bunifuFlatButton3;

    private BunifuFlatButton bunifuFlatButton4;

    private BunifuFlatButton bunifuFlatButton5;

    private BunifuFlatButton bunifuFlatButton6;

    private BunifuFlatButton bunifuFlatButton7;

    private BunifuFlatButton bunifuFlatButton2;

    private ToolStripMenuItem toolStripSpeedUP;

    private CheckBox chkADV;

    private ToolStripMenuItem enableStrongPasswordToolStripMenuItem;

    private ToolStripMenuItem autoChangeDestinationStationToolStripMenuItem;

    private ToolStripMenuItem autoChangeSourceStationToolStripMenuItem;

    private ToolStripMenuItem reInstallChromeToolStripMenuItem;

    private ToolStripMenuItem solveToolStripMenuItem;

    internal ToolStripMenuItem fixIPBlockManuallyClickSIGNINButtonToolStripMenuItem;

    public ToolStripMenuItem autoFillCaptchaByAPIToolStripMenuItem;

    private ToolStripMenuItem showPassengerCaptchaWindowToolStripMenuItem;
    private BunifuFlatButton bunifuFlatButton1;
    private Panel panel1;
    private BunifuFlatButton bunifuFlatButton8;
    private BunifuFlatButton _bnTokenButton;
    private CheckBox checkBox1;
    private CheckBox checkBox2;
    private BunifuCustomLabel bunifuCustomLabel3;
    private BunifuCustomLabel bunifuCustomLabel4;
    private BunifuCustomLabel bunifuCustomLabel2;
    private Panel panel3;
    private Panel panel2;
    private BunifuCustomLabel bunifuCustomLabel6;
    private BunifuCustomLabel bunifuCustomLabel5;
    private Panel panel4;
    private CheckBox checkBox3;
    private CheckBox checkBox4;
    private CheckBox checkBox5;
    private CheckBox checkBox6;
    private CheckBox checkBox8;
    private CheckBox checkBox7;
    private CheckBox checkBox9;
    private LinkLabel linkLabel1;
    private CheckBox checkBox10;
    private Label label50;

    [DllImport("kernel32.dll")]
    internal static extern void ExitProcess(uint uint_0);

    public frmMain()
    {
        InitializeComponent();
        ApplyByteNovaMainStyle();
        StatClass.TotalOpendWindow--;
        panel1.MouseDown += StatClass.Header_MouseDown;
        panel1.MouseMove += StatClass.Header_MouseMove;
        panel1.MouseUp += StatClass.Header_MouseUp;
        Control.CheckForIllegalCrossThreadCalls = false;
        Class28.bool_1 = true;
        Program.mySettings_0.isNewWindowPair = true;
        try
        {
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
        }
        catch
        {
        }
        string variable = "Userid";
        Environment.SetEnvironmentVariable(variable, Program.clientID, EnvironmentVariableTarget.Process);
        string variable2 = "BtechDegree";
        string value = "Nget";
        Environment.SetEnvironmentVariable(variable2, value, EnvironmentVariableTarget.Process);
        string variable3 = "BankRejectFix";
        string value2 = "False";
        Environment.SetEnvironmentVariable(variable3, value2, EnvironmentVariableTarget.Process);
        StatClass.TotalOpendTicket = 0;
    }

    static frmMain()
    {
        Class28.int_0 = 0;
        int_3 = 0;
        Class28.int_4 = 0;
        list_0 = new List<TCntrl>();
        Class28.list_2 = new List<string>();
        Class28.dictionary_0 = new Dictionary<int, int>();
        Class28.dictionary_8 = new Dictionary<string, int>();
        Class28.dictionary_9 = new Dictionary<string, int>();
    }

    public void CheckForAutoFill(object object_0)
    {
        lblUsrMsg.Text = Program.SellerMeg;
        mySettings mySettings_ = Program.mySettings_0;
        string formAlign = "Hpair";
        mySettings_.FormAlign = formAlign;
        string formAlign2 = Program.mySettings_0.FormAlign;
        string text = "Hpair";
        if (!(formAlign2 == text))
        {
            StatClass.VPair = true;
        }
        else
        {
            StatClass.HPair = true;
        }
        if (Program.mySettings_0.isAutoCaptcha)
        {
            AutoFillCaptchaToolStripMenuItem.PerformClick();
            if (Program.mySettings_0.isAutoSubmitCaptcha && Program.mySettings_0.isAutoSubmitCaptcha)
            {
                AutoSubmitCaptchaAfterFill.PerformClick();
            }
        }
        Program.mySettings_0.isMicroWindow = true;
        string variable = "LargeCp";
        string environmentVariable = Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Process);
        string text2 = "True";
        if (environmentVariable == text2)
        {
            Program.mySettings_0.isLargeCaptcha = true;
        }
    }

    public void CheckForAutoFilll(object object_0)
    {
        string query = "DELETE from [TBL_LOG]";
        Staticm.smethod_UpdateFieldValue(query);
    }


    // --------------------------------------------------------------------
    // ByteNova Modern Dashboard UI
    // VISUAL ONLY: existing events, handlers and business logic are kept.
    // --------------------------------------------------------------------

    private Panel _bnSidebar;
    private Panel _bnTopBar;
    private Panel _bnContent;
    private Panel _bnStatusCard;
    private Panel _bnSecureCard;
    private Panel _bnQuickCard;
    private PictureBox _bnLogo;
    private Label _bnBrand;
    private Label _bnSubtitle;
    private Label _bnPageTitle;
    private Label _bnPageSub;
    private Label _bnClockTitle;
    private Label _bnLicenseTitle;
    private Label _bnLicenseValue;
    private Label _bnValidityTitle;
    private Label _bnValidityValue;
    private Label _bnPaidTitle;
    private Label _bnPaidValue;
    private Label _bnSecureTitle;
    private Label _bnSecureSub;
    private Label _bnVersion;
    private Label _bnClockIcon;
    private Label _bnSecureIcon;
    private Label _bnResetIcon;
    private Label _bnNavTitle;
    private Label _bnNavHint;

    // Borderless-form dragging state.
    private bool _bnDragging;
    private Point _bnDragCursorStart;
    private Point _bnDragFormStart;
    private Button _bnResetButton;

    private Form _bnUserInfoPopup;
    private System.Windows.Forms.Timer _bnUserInfoTimer;
    private System.Windows.Forms.Timer _bnUserInfoAutoCloseTimer;
    private int _bnUserInfoTargetY;

    private static readonly Color BN_BG = Color.FromArgb(9, 14, 24);
    private static readonly Color BN_SIDEBAR = Color.FromArgb(13, 21, 34);
    private static readonly Color BN_PANEL = Color.FromArgb(18, 29, 46);
    private static readonly Color BN_PANEL2 = Color.FromArgb(22, 35, 55);
    private static readonly Color BN_CYAN = Color.FromArgb(0, 190, 255);
    private static readonly Color BN_SKY = Color.FromArgb(118, 220, 255);
    private static readonly Color BN_GOLD = Color.FromArgb(255, 194, 71);
    private static readonly Color BN_WHITE = Color.FromArgb(242, 247, 253);
    private static readonly Color BN_MUTED = Color.FromArgb(151, 171, 193);
    private static readonly Color BN_GREEN = Color.FromArgb(55, 218, 119);
    private static readonly Color BN_RED = Color.FromArgb(232, 73, 91);

    private void ApplyByteNovaMainStyle()
    {
        SuspendLayout();

        FormBorderStyle = FormBorderStyle.None;
        BackColor = BN_BG;
        ForeColor = BN_WHITE;
        int byteNovaCheckBox1Width = (checkBox1 != null) ? checkBox1.PreferredSize.Width : 0;
        MinimumSize = new Size(920 + byteNovaCheckBox1Width, 84);
        int byteNovaExtraWidth = (checkBox1 != null) ? checkBox1.PreferredSize.Width : 0;
        ClientSize = new Size(1040 + byteNovaExtraWidth, 88);
        StartPosition = FormStartPosition.Manual;
        ShowInTaskbar = true;

        panel1.Visible = false;
        pnlHeader.Visible = false;
        lblUsrMsg.Visible = false;
        C3AA0.Visible = false;
        menuStrip1.Visible = false;

        BuildByteNovaDashboard();

        // Allow frmMain to be moved with LEFT mouse drag from almost anywhere.
        EnableByteNovaWholeFormDragging(this);

        ResumeLayout(false);
        PerformLayout();
    }

    private void BuildByteNovaDashboard()
    {
        BuildByteNovaCompactDashboard();
        return;

#pragma warning disable CS0162
        Controls.Clear();

        // Keep all designer controls alive for their existing event handlers.
        Controls.Add(panel1);
        Controls.Add(pnlHeader);
        Controls.Add(lblUsrMsg);
        Controls.Add(C3AA0);
        Controls.Add(menuStrip1);
        Controls.Add(lbl_Date);
        Controls.Add(btnClose);
        Controls.Add(linkLabel1);

        // Main shell.
        _bnSidebar = new Panel
        {
            Name = "bnSidebar",
            Dock = DockStyle.Left,
            Width = 230,
            BackColor = BN_SIDEBAR,
            Padding = new Padding(16, 18, 16, 16)
        };
        _bnSidebar.Paint += ByteNovaSidebarPaint;
        Controls.Add(_bnSidebar);

        _bnTopBar = new Panel
        {
            Name = "bnTopBar",
            Dock = DockStyle.Top,
            Height = 92,
            BackColor = BN_PANEL
        };
        _bnTopBar.Paint += ByteNovaTopBarPaint;
        Controls.Add(_bnTopBar);

        _bnContent = new Panel
        {
            Name = "bnContent",
            Dock = DockStyle.Fill,
            BackColor = BN_BG,
            Padding = new Padding(28, 24, 28, 24)
        };
        Controls.Add(_bnContent);

        // Sidebar brand.
        _bnLogo = new PictureBox
        {
            Name = "bnLogo",
            Location = new Point(20, 20),
            Size = new Size(62, 62),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent
        };
        TryLoadByteNovaLogo();
        _bnSidebar.Controls.Add(_bnLogo);

        _bnBrand = MakeLabel("Express", new Point(91, 20), new Size(125, 34),
            new Font("Segoe UI Semibold", 20F, FontStyle.Bold), BN_WHITE);
        _bnSubtitle = MakeLabel("IRCTC ASSISTANT", new Point(93, 51), new Size(120, 20),
            new Font("Segoe UI", 8F, FontStyle.Bold), BN_SKY);
        _bnSidebar.Controls.Add(_bnBrand);
        _bnSidebar.Controls.Add(_bnSubtitle);

        _bnNavTitle = MakeLabel("MAIN MENU", new Point(20, 102), new Size(180, 22),
            new Font("Segoe UI Semibold", 6F, FontStyle.Bold), BN_MUTED);
        _bnSidebar.Controls.Add(_bnNavTitle);

        // Existing buttons are reused; only their visual placement is changed.
        SetupNavButton(bunifuFlatButton7, "IRCTC ID", 132);
        SetupNavButton(bunifuFlatButton1, "BANK", 194);
        SetupNavButton(bunifuFlatButton3, "NEW TICKET", 256);
        SetupNavButton(bunifuFlatButton4, "MANAGE TICKET", 318);
        SetupNavButton(bunifuFlatButton2, "PROXY", 380);
        SetupNavButton(bunifuFlatButton6, "OTP / TOOLS", 442);
        SetupNavButton(bunifuFlatButton8, "BACKUP", 504);

        _bnNavHint = MakeLabel("Secure workspace", new Point(20, 650), new Size(180, 22),
            new Font("Segoe UI", 8F, FontStyle.Regular), BN_MUTED);
        _bnSidebar.Controls.Add(_bnNavHint);

        // Top bar title.
        _bnPageTitle = MakeLabel("Control Center", new Point(28, 15), new Size(330, 34),
            new Font("Segoe UI Semibold", 21F, FontStyle.Bold), BN_WHITE);
        _bnPageSub = MakeLabel("Manage your booking tools from one place", new Point(30, 51), new Size(380, 24),
            new Font("Segoe UI", 9.5F, FontStyle.Regular), BN_MUTED);
        _bnTopBar.Controls.Add(_bnPageTitle);
        _bnTopBar.Controls.Add(_bnPageSub);

        // Live clock on top right.
        _bnClockIcon = MakeLabel("◷", new Point(ClientSize.Width - 360, 20), new Size(42, 48),
            new Font("Segoe UI Symbol", 28F), BN_CYAN);
        _bnClockTitle = MakeLabel("LIVE TIME", new Point(ClientSize.Width - 312, 17), new Size(120, 20),
            new Font("Segoe UI Semibold", 8F, FontStyle.Bold), BN_SKY);
        _bnTopBar.Controls.Add(_bnClockIcon);
        _bnTopBar.Controls.Add(_bnClockTitle);

        lbl_Date.Visible = true;
        lbl_Date.AutoSize = false;
        lbl_Date.Location = new Point(ClientSize.Width - 315, 37);
        lbl_Date.Size = new Size(205, 35);
        lbl_Date.TextAlign = ContentAlignment.MiddleLeft;
        lbl_Date.Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);
        lbl_Date.ForeColor = BN_WHITE;
        lbl_Date.BackColor = Color.Transparent;
        _bnTopBar.Controls.Add(lbl_Date);

        // Close button.
        btnClose.Visible = true;
        btnClose.Location = new Point(ClientSize.Width - 55, 20);
        btnClose.Size = new Size(38, 52);
        btnClose.SizeMode = PictureBoxSizeMode.Zoom;
        btnClose.BackColor = BN_RED;
        btnClose.Paint -= ByteNovaClosePaint;
        btnClose.Paint += ByteNovaClosePaint;
        _bnTopBar.Controls.Add(btnClose);
        btnClose.BringToFront();

        // Content cards.
        _bnStatusCard = MakeCard("ACCOUNT STATUS", new Point(28, 92), new Size(380, 170));
        _bnStatusCard.Paint += ByteNovaCardPaint;
        _bnContent.Controls.Add(_bnStatusCard);

        _bnLicenseTitle = MakeLabel("LICENSE KEY", new Point(24, 25), new Size(130, 22),
            new Font("Segoe UI Semibold", 6F, FontStyle.Bold), BN_MUTED);
        _bnLicenseValue = MakeLabel("ACTIVE", new Point(24, 47), new Size(320, 30),
            new Font("Segoe UI Semibold", 16F, FontStyle.Bold), BN_GOLD);
        _bnValidityTitle = MakeLabel("KEY VALIDITY", new Point(24, 82), new Size(130, 22),
            new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold), BN_MUTED);
        _bnValidityValue = MakeLabel("VALID", new Point(24, 104), new Size(150, 28),
            new Font("Segoe UI Semibold", 13F, FontStyle.Bold), BN_GREEN);
        _bnPaidTitle = MakeLabel("PAYMENT STATUS", new Point(190, 82), new Size(150, 22),
            new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold), BN_MUTED);
        _bnPaidValue = MakeLabel("PAID", new Point(190, 104), new Size(150, 28),
            new Font("Segoe UI Semibold", 13F, FontStyle.Bold), BN_GREEN);
        _bnStatusCard.Controls.Add(_bnLicenseTitle);
        _bnStatusCard.Controls.Add(_bnLicenseValue);
        _bnStatusCard.Controls.Add(_bnValidityTitle);
        _bnStatusCard.Controls.Add(_bnValidityValue);
        _bnStatusCard.Controls.Add(_bnPaidTitle);
        _bnStatusCard.Controls.Add(_bnPaidValue);

        _bnSecureCard = MakeCard("SYSTEM SECURITY", new Point(432, 92), new Size(300, 170));
        _bnSecureCard.Paint += ByteNovaCardPaint;
        _bnContent.Controls.Add(_bnSecureCard);

        _bnSecureIcon = MakeLabel("✓", new Point(24, 35), new Size(60, 60),
            new Font("Segoe UI Semibold", 34F, FontStyle.Bold), BN_GREEN);
        _bnSecureTitle = MakeLabel("Secure", new Point(92, 32), new Size(170, 32),
            new Font("Segoe UI Semibold", 18F, FontStyle.Bold), BN_WHITE);
        _bnSecureSub = MakeLabel("All Systems Operational", new Point(92, 66), new Size(185, 42),
            new Font("Segoe UI", 9F, FontStyle.Regular), BN_MUTED);
        _bnSecureCard.Controls.Add(_bnSecureIcon);
        _bnSecureCard.Controls.Add(_bnSecureTitle);
        _bnSecureCard.Controls.Add(_bnSecureSub);

        _bnQuickCard = MakeCard("QUICK ACTION", new Point(756, 92), new Size(300, 170));
        _bnQuickCard.Paint += ByteNovaCardPaint;
        _bnContent.Controls.Add(_bnQuickCard);
        Label quick = MakeLabel("Reset booking workspace", new Point(24, 36), new Size(245, 28),
            new Font("Segoe UI Semibold", 12F, FontStyle.Bold), BN_WHITE);
        Label quickSub = MakeLabel("Clear active state and start fresh", new Point(24, 69), new Size(245, 42),
            new Font("Segoe UI", 8.5F), BN_MUTED);
        _bnQuickCard.Controls.Add(quick);
        _bnQuickCard.Controls.Add(quickSub);

        // Existing reset action is retained.
        linkLabel1.Visible = false;
        linkLabel1.Text = "RESET";
        linkLabel1.LinkColor = BN_WHITE;
        linkLabel1.ActiveLinkColor = Color.White;
        linkLabel1.VisitedLinkColor = BN_WHITE;
        linkLabel1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        linkLabel1.TextAlign = ContentAlignment.MiddleCenter;
        linkLabel1.Location = new Point(24, 118);
        linkLabel1.Size = new Size(110, 34);
        linkLabel1.BackColor = Color.FromArgb(22, 124, 82);
        _bnQuickCard.Controls.Add(linkLabel1);
        linkLabel1.BringToFront();

        _bnResetIcon = MakeLabel("↻", new Point(142, 116), new Size(40, 38),
            new Font("Segoe UI Symbol", 22F, FontStyle.Bold), BN_GOLD);
        _bnQuickCard.Controls.Add(_bnResetIcon);

        _bnVersion = MakeLabel("Version : Latest", new Point(28, 286), new Size(180, 24),
            new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold), BN_MUTED);
        _bnContent.Controls.Add(_bnVersion);

        // Keep legacy controls alive but out of the redesigned shell.
        bunifuCustomLabel3.Visible = false;
        bunifuCustomLabel4.Visible = false;
        bunifuCustomLabel5.Visible = false;
        bunifuCustomLabel6.Visible = false;
        bunifuCustomLabel2.Visible = false;
        pnlHeader.Visible = false;
        lblUsrMsg.Visible = false;
        menuStrip1.Visible = false;

        Resize += ByteNovaMain_Resize;
    }

    private Panel MakeCard(string title, Point location, Size size)
    {
        Panel p = new Panel
        {
            Name = "bnCard_" + title.Replace(" ", "_"),
            Location = location,
            Size = size,
            BackColor = BN_PANEL,
            Anchor = AnchorStyles.Top | AnchorStyles.Left
        };
        Label t = MakeLabel(title, new Point(24, 12), new Size(size.Width - 48, 20),
            new Font("Segoe UI Semibold", 8F, FontStyle.Bold), BN_CYAN);
        p.Controls.Add(t);
        return p;
    }

    private Label MakeLabel(string text, Point location, Size size, Font font, Color color)
    {
        return new Label
        {
            Text = text,
            Location = location,
            Size = size,
            AutoSize = false,
            Font = font,
            ForeColor = color,
            BackColor = Color.Transparent,
            TextAlign = ContentAlignment.MiddleLeft
        };
    }

    private void SetupNavButton(BunifuFlatButton button, string text, int y)
    {
        button.Parent = _bnSidebar;
        button.Visible = true;
        button.Location = new Point(16, y);
        button.Size = new Size(198, 54);
        button.BorderRadius = 9;
        button.Normalcolor = BN_SIDEBAR;
        button.Activecolor = Color.FromArgb(0, 105, 170);
        button.OnHovercolor = Color.FromArgb(0, 126, 195);
        button.Textcolor = BN_WHITE;
        button.OnHoverTextColor = Color.White;
        button.TextFont = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
        button.TextAlign = ContentAlignment.MiddleLeft;
        button.ButtonText = "   " + text;
        button.Text = "   " + text;
        button.BringToFront();
    }

    private void TryLoadByteNovaLogo()
    {
        try
        {
            string[] paths =
            {
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "express-logo.png"),
            Path.Combine(Application.StartupPath, "express-logo.png")
        };
            foreach (string path in paths)
            {
                if (!File.Exists(path)) continue;
                _bnLogo.Image = Image.FromFile(path);
                return;
            }
        }
        catch { }

        Bitmap bmp = new Bitmap(62, 62);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(BN_SIDEBAR);
            using (Pen p = new Pen(Color.FromArgb(198, 244, 50), 3))
                g.DrawEllipse(p, 5, 5, 52, 52);
            using (Font f = new Font("Segoe UI Black", 29F, FontStyle.Bold))
            using (Brush b = new SolidBrush(Color.FromArgb(198, 244, 50)))
                g.DrawString("E", f, b, 17, 12);
        }
        _bnLogo.Image = bmp;
    }

    private void ByteNovaSidebarPaint(object sender, PaintEventArgs e)
    {
        Control c = (Control)sender;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (Pen p = new Pen(Color.FromArgb(32, 62, 88), 1.5F))
            e.Graphics.DrawLine(p, c.Width - 1, 0, c.Width - 1, c.Height);
        using (Pen p = new Pen(BN_CYAN, 2))
            e.Graphics.DrawLine(p, 18, 88, 62, 88);
    }

    private void ByteNovaTopBarPaint(object sender, PaintEventArgs e)
    {
        Control c = (Control)sender;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (LinearGradientBrush bg = new LinearGradientBrush(
            new Rectangle(0, 0, c.Width, c.Height), BN_PANEL2, BN_PANEL,
            LinearGradientMode.Horizontal))
            e.Graphics.FillRectangle(bg, 0, 0, c.Width, c.Height);
        using (Pen p = new Pen(Color.FromArgb(35, 61, 87), 1))
            e.Graphics.DrawLine(p, 0, c.Height - 1, c.Width, c.Height - 1);
    }

    private void ByteNovaCardPaint(object sender, PaintEventArgs e)
    {
        Control c = (Control)sender;
        Rectangle r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (LinearGradientBrush bg = new LinearGradientBrush(r, BN_PANEL2, BN_PANEL,
            LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, r);
        using (Pen p = new Pen(Color.FromArgb(38, 69, 96), 1.2F))
            DrawRoundedRectangle(e.Graphics, p, r, 12);
        using (Pen p = new Pen(BN_CYAN, 2))
            e.Graphics.DrawLine(p, 24, 35, 62, 35);
    }




    private void ByteNovaResetButton_Click(object sender, EventArgs e)
    {
        // Use the application's existing Reset logic.
        linkLabel1_LinkClicked(linkLabel1, null);
    }

    private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        int d = radius * 2;
        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    private void ByteNovaResetPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle r = new Rectangle(1, 1, c.Width - 3, c.Height - 3);
        using (GraphicsPath path = CreateRoundedRectanglePath(r, 10))
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(4, 108, 61), Color.FromArgb(2, 54, 39),
            LinearGradientMode.Vertical))
        using (Pen border = new Pen(Color.FromArgb(0, 230, 128), 1.4F))
        {
            e.Graphics.FillPath(bg, path);
            e.Graphics.DrawPath(border, path);
        }

        using (Font iconFont = new Font("Segoe UI Symbol", 14F, FontStyle.Bold))
        using (Font textFont = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold))
        using (Brush white = new SolidBrush(Color.White))
        {
            StringFormat center = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            e.Graphics.DrawString("↻", iconFont, white,
                new RectangleF(0, 0, c.Width, 17), center);
            e.Graphics.DrawString("RESET", textFont, white,
                new RectangleF(0, 16, c.Width, 12), center);
        }
    }

    private void ByteNovaClosePaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle r = new Rectangle(1, 1, c.Width - 3, c.Height - 3);
        using (GraphicsPath path = CreateRoundedRectanglePath(r, 10))
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(190, 22, 34), Color.FromArgb(82, 7, 17),
            LinearGradientMode.Vertical))
        using (Pen border = new Pen(Color.FromArgb(255, 70, 80), 1.4F))
        {
            e.Graphics.FillPath(bg, path);
            e.Graphics.DrawPath(border, path);
        }

        using (Pen p = new Pen(Color.White, 3.2F))
        {
            p.StartCap = LineCap.Round;
            p.EndCap = LineCap.Round;
            int m = 9;
            e.Graphics.DrawLine(p, m, 6, c.Width - m, c.Height - 6);
            e.Graphics.DrawLine(p, c.Width - m, 6, m, c.Height - 6);
        }
    }

    private void DrawRoundedRectangle(Graphics g, Pen pen, Rectangle rect, int radius)
    {
        int d = radius * 2;
        using (GraphicsPath path = new GraphicsPath())
        {
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            g.DrawPath(pen, path);
        }
    }

    private void ByteNovaMain_Resize(object sender, EventArgs e)
    {
        if (_bnTopBar == null || _bnContent == null) return;
        ApplyCompactResponsiveLayout();
        UpdateByteNovaCompactClock();
        BuildByteNovaTopUtilityBoxes();
    }

    private void ApplyCompactResponsiveLayout()
    {
        if (_bnTopBar == null || _bnContent == null) return;

        int w = Math.Max(906, ClientSize.Width - 14);
        _bnTopBar.Location = new Point(7, 7);
        _bnTopBar.Size = new Size(w, 74);

        int right = _bnTopBar.ClientSize.Width - 8;

        // Close is managed only by BuildByteNovaTopUtilityBoxes().

        if (linkLabel1 != null)
            linkLabel1.Visible = false;

        if (_bnResetButton != null)
        {
            _bnResetButton.Size = new Size(78, 70);
            _bnResetButton.Location = new Point(right - 78, 8);
            right -= 84;
        }

        if (_bnSecureCard != null)
        {
            _bnSecureCard.Size = new Size(78, 70);
            _bnSecureCard.Location = new Point(right - 78, 8);
            right -= 86;
        }

        if (_bnStatusCard != null)
        {
            _bnStatusCard.Size = new Size(230, 70);
            _bnStatusCard.Location = new Point(right - 230, 8);
        }

        if (_bnClockIcon != null)
        {
            if (_bnClockIcon.Parent != null && _bnClockIcon.Parent.Name == "bnTopTimeBox")
                _bnClockIcon.Location = new Point(6, -3);
            else
                _bnClockIcon.Location = new Point(275, 0);
        }

        if (_bnClockTitle != null)
        {
            if (_bnClockTitle.Parent != null && _bnClockTitle.Parent.Name == "bnTopTimeBox")
            {
                _bnClockTitle.Location = new Point(40, 0);
                _bnClockTitle.Size = new Size(120, 25);
                _bnClockTitle.Font = new Font("Segoe UI Semibold", 7.2F, FontStyle.Bold);
                _bnClockTitle.ForeColor = Color.White;
                _bnClockTitle.Visible = true;
            }
            else
            {
                _bnClockTitle.Location = new Point(313, 18);
                _bnClockTitle.Size = new Size(275, 28);
                _bnClockTitle.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            }
        }

        if (_bnPageSub != null)
            _bnPageSub.Visible = false;

        Panel nav = _bnTopBar.Controls["bnCompactNavigation"] as Panel;
        if (nav != null)
        {
            // Start the buttons curved box directly below Booking Mode-1.
            Panel mode1Box = _bnTopBar.Controls["bnModeBox_chkADV"] as Panel;
            int navLeft = (mode1Box != null) ? mode1Box.Left : 272;

            nav.Location = new Point(navLeft, 39);
            nav.Size = new Size(Math.Max(400, _bnTopBar.ClientSize.Width - navLeft - 7), 32);
        }

        LayoutCompactNavButtons();

        if (_bnStatusCard != null) _bnStatusCard.Visible = false;
        if (_bnSecureCard != null) _bnSecureCard.Visible = false;
        if (_bnResetButton != null && _bnResetButton.Parent == null) _bnResetButton.Visible = true;
        if (btnClose != null)
        {
            btnClose.Visible = true;
            btnClose.BringToFront();
        }
    }




    // --------------------------------------------------------------------
    // Compact bottom control bar - designed to match the supplied reference.
    // The original application controls/events are reused; only the layout is changed.
    // --------------------------------------------------------------------
    private void BuildByteNovaCompactDashboard()
    {
        SuspendLayout();
        Controls.Clear();

        Controls.Add(panel1);
        Controls.Add(pnlHeader);
        Controls.Add(lblUsrMsg);
        Controls.Add(C3AA0);
        Controls.Add(menuStrip1);
        Controls.Add(lbl_Date);
        Controls.Add(btnClose);
        Controls.Add(linkLabel1);

        panel1.Visible = false;
        pnlHeader.Visible = false;
        lblUsrMsg.Visible = false;
        C3AA0.Visible = false;
        menuStrip1.Visible = false;
        lbl_Date.Visible = false;

        _bnContent = new Panel
        {
            Name = "bnCompactRoot",
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(1, 10, 20),
            Margin = Padding.Empty
        };
        _bnContent.Paint += ByteNovaCompactRootPaint;
        Controls.Add(_bnContent);

        _bnTopBar = new Panel
        {
            Name = "bnCompactHeader",
            Location = new Point(7, 7),
            Size = new Size(ClientSize.Width - 14, 74),
            BackColor = Color.Transparent,
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
        };
        _bnTopBar.Paint += ByteNovaCompactHeaderPaint;
        _bnContent.Controls.Add(_bnTopBar);

        // Borderless form can be moved by dragging the header.
        _bnTopBar.MouseDown += ByteNovaDrag_MouseDown;
        _bnTopBar.MouseMove += ByteNovaDrag_MouseMove;
        _bnTopBar.MouseUp += ByteNovaDrag_MouseUp;

        _bnLogo = new PictureBox
        {
            Name = "bnCompactLogo",
            Location = new Point(3, 4),
            Size = new Size(68, 64),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.Transparent
        };
        TryLoadByteNovaLogo();
        _bnTopBar.Controls.Add(_bnLogo);

        _bnBrand = MakeLabel("", new Point(73, 13), new Size(165, 34),
            new Font("Segoe UI Semibold", 19F, FontStyle.Bold), BN_WHITE);
        _bnBrand.Paint += ByteNovaCompactBrandPaint;
        _bnSubtitle = MakeLabel("P R E M I U M   T A T K A L   S O F T W A R E",
            new Point(73, 48), new Size(180, 16),
            new Font("Segoe UI", 5.2F, FontStyle.Bold), BN_MUTED);
        _bnTopBar.Controls.Add(_bnBrand);
        _bnTopBar.Controls.Add(_bnSubtitle);

        _bnClockIcon = MakeLabel("◷", new Point(285, 0), new Size(34, 38),
            new Font("Segoe UI Symbol", 21F), BN_CYAN);
        _bnClockTitle = MakeLabel("00:00:00", new Point(313, 10), new Size(125, 34),
            new Font("Segoe UI Semibold", 17F, FontStyle.Bold), BN_WHITE);
        _bnPageSub = MakeLabel("", new Point(315, 46), new Size(130, 18),
            new Font("Segoe UI", 7F, FontStyle.Bold), BN_SKY);
        _bnTopBar.Controls.Add(_bnClockIcon);
        _bnTopBar.Controls.Add(_bnClockTitle);
        _bnTopBar.Controls.Add(_bnPageSub);

        _bnStatusCard = new Panel
        {
            Name = "bnCompactLicense",
            Size = new Size(180, 30),
            BackColor = Color.FromArgb(5, 28, 47),
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        _bnStatusCard.Paint += ByteNovaCompactStatusPaint;
        _bnTopBar.Controls.Add(_bnStatusCard);

        _bnLicenseTitle = MakeLabel("🔑 LICENSE KEY :", new Point(6, 0), new Size(88, 10),
            new Font("Segoe UI Semibold", 6.7F, FontStyle.Bold), BN_WHITE);
        _bnLicenseValue = MakeLabel("ACTIVE", new Point(98, 0), new Size(75, 10),
            new Font("Segoe UI Semibold", 7F, FontStyle.Bold), BN_WHITE);
        _bnValidityTitle = MakeLabel("▣ KEY VALIDITY :", new Point(6, 10), new Size(88, 10),
            new Font("Segoe UI Semibold", 6.7F, FontStyle.Bold), BN_WHITE);
        _bnValidityValue = MakeLabel("VALID", new Point(98, 10), new Size(75, 10),
            new Font("Segoe UI Semibold", 7F, FontStyle.Bold), BN_GREEN);
        _bnPaidTitle = MakeLabel("▤ PAID STATUS :", new Point(6, 20), new Size(88, 9),
            new Font("Segoe UI Semibold", 6.7F, FontStyle.Bold), BN_WHITE);
        _bnPaidValue = MakeLabel("PAID", new Point(98, 20), new Size(75, 9),
            new Font("Segoe UI Semibold", 7F, FontStyle.Bold), BN_GREEN);

        _bnStatusCard.Controls.Add(_bnLicenseTitle);
        _bnStatusCard.Controls.Add(_bnLicenseValue);
        _bnStatusCard.Controls.Add(_bnValidityTitle);
        _bnStatusCard.Controls.Add(_bnValidityValue);
        _bnStatusCard.Controls.Add(_bnPaidTitle);
        _bnStatusCard.Controls.Add(_bnPaidValue);

        // Secure, Reset and Close all use the same compact card size.
        _bnSecureCard = new Panel
        {
            Name = "bnCompactSecurity",
            Size = new Size(78, 70),
            BackColor = Color.FromArgb(2, 48, 43),
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        _bnSecureCard.Paint += ByteNovaCompactSecurityPaint;
        _bnTopBar.Controls.Add(_bnSecureCard);

        _bnSecureIcon = MakeLabel("♢", new Point(3, 4), new Size(16, 20),
            new Font("Segoe UI Symbol", 10F, FontStyle.Bold), BN_GREEN);
        _bnSecureTitle = MakeLabel("Secure", new Point(17, 6), new Size(29, 16),
            new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold), BN_WHITE);
        _bnSecureTitle.TextAlign = ContentAlignment.MiddleCenter;
        _bnSecureSub = MakeLabel("", new Point(0, 0), new Size(1, 1),
            new Font("Segoe UI", 5.2F), BN_WHITE);
        _bnSecureSub.TextAlign = ContentAlignment.MiddleCenter;
        _bnSecureCard.Controls.Add(_bnSecureIcon);
        _bnSecureCard.Controls.Add(_bnSecureTitle);
        _bnSecureCard.Controls.Add(_bnSecureSub);

        // RESET - real Button (not LinkLabel).
        linkLabel1.Visible = false;

        _bnResetButton = new Button
        {
            Name = "bnResetButton",
            Text = "",
            Size = new Size(78, 70),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(3, 78, 49),
            ForeColor = Color.White,
            Cursor = Cursors.Hand,
            TabStop = false,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        _bnResetButton.FlatAppearance.BorderSize = 0;
        _bnResetButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(4, 95, 56);
        _bnResetButton.FlatAppearance.MouseDownBackColor = Color.FromArgb(2, 65, 42);
        _bnResetButton.Paint += ByteNovaResetPaint;
        _bnResetButton.Click += ByteNovaResetButton_Click;
        _bnTopBar.Controls.Add(_bnResetButton);

        // CLOSE card - same dimensions as Secure and Reset.
        btnClose.Visible = true;
        btnClose.Size = new Size(78, 82);
        btnClose.BackColor = Color.FromArgb(82, 8, 18);
        btnClose.SizeMode = PictureBoxSizeMode.Zoom;
        btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClose.Paint -= ByteNovaClosePaint;
        btnClose.Paint += ByteNovaClosePaint;
        _bnTopBar.Controls.Add(btnClose);

        Panel nav = new Panel
        {
            Name = "bnCompactNavigation",
            Location = new Point(272, 69),
            Size = new Size(Math.Max(400, _bnTopBar.ClientSize.Width - 280), 32),
            BackColor = Color.FromArgb(31, 19, 43),
            Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
        };
        nav.Paint += ByteNovaCompactNavPaint;
        _bnTopBar.Controls.Add(nav);

        SetupCompactButton(bunifuFlatButton7, "IRCTC_ID", 0, 0);
        SetupCompactButton(bunifuFlatButton1, "BANK", 0, 0);
        SetupCompactButton(bunifuFlatButton3, "NEW_TICKET", 0, 0);
        SetupCompactButton(bunifuFlatButton4, "PROCESS_TICKET", 0, 0);
        SetupCompactButton(bunifuFlatButton2, "PROXY", 0, 0);
        SetupCompactButton(bunifuFlatButton6, "BYPASS", 0, 0);
        SetupCompactButton(bunifuFlatButton8, "BACKUP", 0, 0);

        // TOKEN button - opens the existing Token_count form.
        _bnTokenButton = new BunifuFlatButton();
        _bnTokenButton.Name = "bnTokenButton";
        _bnTokenButton.Click += ByteNovaTokenButton_Click;
        SetupCompactButton(_bnTokenButton, "TOKEN", 0, 0);
        // TOKEN button: text only, no icon.
        _bnTokenButton.Iconimage = null;
        _bnTokenButton.IconVisible = false;
        _bnTokenButton.IconZoom = 0;
        _bnTokenButton.TextAlign = ContentAlignment.MiddleCenter;
        SetCompactButtonShade(_bnTokenButton, Color.FromArgb(20, 37, 58), Color.FromArgb(0, 126, 195));
        bunifuFlatButton5.Visible = false;

        // Subtle individual shades for each navigation button.
        // Kept intentionally light so the ByteNova dark theme remains consistent.
        SetCompactButtonShade(bunifuFlatButton7, Color.FromArgb(5, 38, 59), Color.FromArgb(0, 101, 150));
        SetCompactButtonShade(bunifuFlatButton1, Color.FromArgb(16, 34, 61), Color.FromArgb(39, 91, 157));
        SetCompactButtonShade(bunifuFlatButton3, Color.FromArgb(36, 31, 57), Color.FromArgb(91, 69, 148));
        SetCompactButtonShade(bunifuFlatButton4, Color.FromArgb(18, 44, 58), Color.FromArgb(0, 116, 145));
        SetCompactButtonShade(bunifuFlatButton2, Color.FromArgb(43, 34, 48), Color.FromArgb(126, 74, 125));
        SetCompactButtonShade(bunifuFlatButton6, Color.FromArgb(46, 39, 25), Color.FromArgb(139, 103, 38));
        SetCompactButtonShade(bunifuFlatButton8, Color.FromArgb(23, 42, 43), Color.FromArgb(34, 118, 105));

        _bnVersion = null;

        bunifuCustomLabel3.Visible = false;
        bunifuCustomLabel4.Visible = false;
        bunifuCustomLabel5.Visible = false;
        bunifuCustomLabel6.Visible = false;
        bunifuCustomLabel2.Visible = false;

        Resize -= ByteNovaMain_Resize;
        Resize += ByteNovaMain_Resize;

        PositionByteNovaCompact();
        ApplyCompactResponsiveLayout();
        UpdateByteNovaCompactClock();
        BuildByteNovaTopUtilityBoxes();

        // Requested compact header: hide License, Secure and Reset only.
        if (_bnStatusCard != null) _bnStatusCard.Visible = false;
        if (_bnSecureCard != null) _bnSecureCard.Visible = false;
        if (_bnResetButton != null && _bnResetButton.Parent == null) _bnResetButton.Visible = true;

        ResumeLayout(false);
    }


    private void BuildByteNovaTopUtilityBoxes()
    {
        if (_bnTopBar == null) return;

        Panel timeBox = _bnTopBar.Controls["bnTopTimeBox"] as Panel;
        if (timeBox == null)
        {
            timeBox = new Panel
            {
                Name = "bnTopTimeBox",
                BackColor = Color.FromArgb(31, 19, 43),
                Size = new Size(165, 32),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            timeBox.Paint += ByteNovaCompactNavPaint;
            _bnTopBar.Controls.Add(timeBox);
        }

        Panel resetBox = _bnTopBar.Controls["bnTopResetBox"] as Panel;
        if (resetBox == null)
        {
            resetBox = new Panel
            {
                Name = "bnTopResetBox",
                BackColor = Color.FromArgb(31, 19, 43),
                Size = new Size(38, 32),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            resetBox.Paint += ByteNovaCompactNavPaint;
            _bnTopBar.Controls.Add(resetBox);
        }

        Panel closeBox = _bnTopBar.Controls["bnTopCloseBox"] as Panel;
        if (closeBox == null)
        {
            closeBox = new Panel
            {
                Name = "bnTopCloseBox",
                BackColor = Color.FromArgb(31, 19, 43),
                Size = new Size(38, 32),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            closeBox.Paint += ByteNovaCompactNavPaint;
            _bnTopBar.Controls.Add(closeBox);
        }

        timeBox.Location = new Point(_bnTopBar.ClientSize.Width - 251, 4);
        resetBox.Location = new Point(_bnTopBar.ClientSize.Width - 84, 4);
        closeBox.Location = new Point(_bnTopBar.ClientSize.Width - 43, 4);

        // Requested top options before the Time curved box.
        // Hide checkBox8/checkBox10; keep their existing logic untouched.
        if (checkBox8 != null) checkBox8.Visible = false;
        if (checkBox10 != null) checkBox10.Visible = false;

        // Each booking-mode checkbox gets its own separate curved box.
        CheckBox[] topChecks = new CheckBox[] { checkBox5, checkBox3, chkADV, checkBox1 };
        int boxRight = timeBox.Left - 6;

        foreach (CheckBox cb in topChecks)
        {
            if (cb == null) continue;

            cb.AutoSize = true;
            cb.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);

            int boxWidth = cb.PreferredSize.Width + 18;
            Panel modeBox = _bnTopBar.Controls["bnModeBox_" + cb.Name] as Panel;
            if (modeBox == null)
            {
                modeBox = new Panel
                {
                    Name = "bnModeBox_" + cb.Name,
                    BackColor = Color.FromArgb(31, 19, 43),
                    Size = new Size(boxWidth, 32),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };
                modeBox.Paint += ByteNovaCompactNavPaint;
                _bnTopBar.Controls.Add(modeBox);
            }

            modeBox.Size = new Size(boxWidth, 32);
            modeBox.Location = new Point(boxRight - boxWidth, 4);
            boxRight = modeBox.Left - 4;

            cb.Parent = modeBox;
            cb.BackColor = Color.Transparent;
            cb.ForeColor = Color.White;
            cb.UseVisualStyleBackColor = false;
            cb.Visible = true;

            if (cb == checkBox1 && checkBox9 != null)
            {
                int rowH = Math.Max(cb.PreferredSize.Height, checkBox9.PreferredSize.Height);

                // checkBox1 + checkBox9 curved box spans the full compact header height.
                modeBox.Size = new Size(boxWidth, Math.Max(1, _bnTopBar.ClientSize.Height - 5));
                modeBox.Location = new Point(modeBox.Left, 0);

                int totalRowsH = rowH * 2;
                int firstY = Math.Max(-4, ((_bnTopBar.ClientSize.Height - totalRowsH) / 2) - 11);

                cb.Location = new Point(9, firstY);

                checkBox9.Parent = modeBox;
                checkBox9.AutoSize = true;
                checkBox9.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
                checkBox9.BackColor = Color.Transparent;
                checkBox9.ForeColor = Color.White;
                checkBox9.UseVisualStyleBackColor = false;
                checkBox9.Visible = true;
                checkBox9.Location = new Point(9, firstY + rowH);
                checkBox9.BringToFront();

                Label userInfo = modeBox.Controls["bnUserInfoLabel"] as Label;
                if (userInfo == null)
                {
                    userInfo = new Label
                    {
                        Name = "bnUserInfoLabel",
                        Text = "USER INFO",
                        AutoSize = false,
                        BackColor = Color.Transparent,
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI Black", 8.2F, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter,
                        Cursor = Cursors.Hand
                    };
                    userInfo.Click += ByteNovaUserInfo_Click;
                    userInfo.Paint += ByteNovaUserInfoButtonPaint;
                    modeBox.Controls.Add(userInfo);
                }

                userInfo.SetBounds(5, Math.Max(1, modeBox.ClientSize.Height - 23),
                    Math.Max(1, modeBox.ClientSize.Width - 10), 20);
                userInfo.BackColor = Color.Transparent;
                userInfo.BringToFront();
            }
            else
            {
                cb.Location = new Point(9, Math.Max(5, (modeBox.ClientSize.Height - cb.PreferredSize.Height) / 2));
            }

            cb.BringToFront();
            modeBox.BringToFront();
        }

        if (_bnClockIcon != null)
        {
            _bnClockIcon.Parent = timeBox;
            _bnClockIcon.Location = new Point(6, -3);
            _bnClockIcon.Size = new Size(30, 24);
            _bnClockIcon.TextAlign = ContentAlignment.MiddleCenter;
            _bnClockIcon.BringToFront();
        }

        if (_bnClockTitle != null)
        {
            _bnClockTitle.Parent = timeBox;
            _bnClockTitle.Location = new Point(40, 0);
            _bnClockTitle.Size = new Size(120, 25);
            _bnClockTitle.Font = new Font("Segoe UI Semibold", 7.2F, FontStyle.Bold);
            _bnClockTitle.ForeColor = Color.White;
            _bnClockTitle.Visible = true;
            _bnClockTitle.TextAlign = ContentAlignment.MiddleLeft;
            _bnClockTitle.BringToFront();
        }

        if (_bnPageSub != null)
            _bnPageSub.Visible = false;

        if (_bnResetButton != null)
        {
            // Preserve the original Reset control and its existing click handler,
            // but use a simple overlay glyph so the icon always renders.
            _bnResetButton.Parent = resetBox;
            _bnResetButton.Visible = false;

            Label resetGlyph = resetBox.Controls["bnResetGlyph"] as Label;
            if (resetGlyph == null)
            {
                resetGlyph = new Label
                {
                    Name = "bnResetGlyph",
                    Text = "↻",
                    AutoSize = false,
                    BackColor = Color.Transparent,
                    ForeColor = Color.FromArgb(0, 210, 255),
                    Font = new Font("Segoe UI Symbol", 15F, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Cursor = Cursors.Hand
                };
                // Call the existing Reset logic directly.
                // PerformClick() does not reliably fire while the legacy button is hidden.
                resetGlyph.Click += delegate
                {
                    ByteNovaResetButton_Click(resetGlyph, EventArgs.Empty);
                };
                resetBox.Controls.Add(resetGlyph);
            }

            resetGlyph.SetBounds(2, 1, resetBox.ClientSize.Width - 4, resetBox.ClientSize.Height - 2);
            resetGlyph.Visible = true;
            resetGlyph.BringToFront();
        }

        if (btnClose != null)
        {
            btnClose.Parent = closeBox;
            btnClose.Visible = true;
            btnClose.Size = new Size(22, 22);
            btnClose.Location = new Point(8, 5);
            btnClose.Anchor = AnchorStyles.None;

            // Keep the original Close image clean and remove the old painted/red background.
            btnClose.Paint -= ByteNovaClosePaint;
            btnClose.BackColor = Color.Transparent;
            btnClose.BackgroundImage = null;
            btnClose.BackgroundImageLayout = ImageLayout.Zoom;
            btnClose.SizeMode = PictureBoxSizeMode.Zoom;
            btnClose.Zoom = 0;

            btnClose.BringToFront();
        }

        timeBox.BringToFront();
        resetBox.BringToFront();
        closeBox.BringToFront();
    }

    private void ByteNovaUserInfoButtonPaint(object sender, PaintEventArgs e)
    {
        Label c = sender as Label;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(1, 1, c.Width - 3, c.Height - 3);

        using (GraphicsPath path = CreateRoundedRectanglePath(r, 7))
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(0, 119, 181), Color.FromArgb(3, 55, 91),
            LinearGradientMode.Vertical))
        using (Pen border = new Pen(Color.FromArgb(0, 215, 255), 1.3F))
        using (Brush textBrush = new SolidBrush(Color.White))
        using (Font textFont = new Font("Segoe UI Black", 8.2F, FontStyle.Bold))
        {
            e.Graphics.FillPath(bg, path);
            e.Graphics.DrawPath(border, path);

            using (StringFormat sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                e.Graphics.DrawString("USER INFO", textFont, textBrush, r, sf);
            }
        }
    }


    private void ByteNovaUserInfo_Click(object sender, EventArgs e)
    {
        EnsureByteNovaUserInfoPopup();

        if (_bnUserInfoPopup.Visible)
        {
            _bnUserInfoTimer.Stop();
            if (_bnUserInfoAutoCloseTimer != null)
                _bnUserInfoAutoCloseTimer.Stop();
            _bnUserInfoPopup.Hide();
            return;
        }

        Label idValue = _bnUserInfoPopup.Controls["bnUserIdValue"] as Label;
        Label validityValue = _bnUserInfoPopup.Controls["bnValidityValue"] as Label;
        Label appVersionValue = _bnUserInfoPopup.Controls["bnAppVersionValue"] as Label;
        Label paymentStatusValue = _bnUserInfoPopup.Controls["bnPaymentStatusValue"] as Label;

        if (idValue != null)
            idValue.Text = Convert.ToString(Program.clientID);

        if (validityValue != null)
            validityValue.Text = Program.ExpiryDay.ToString() + " Days";

        if (appVersionValue != null)
            appVersionValue.Text = Convert.ToString(Program.appversion);

        if (paymentStatusValue != null)
            paymentStatusValue.Text = Convert.ToString(Program.KeyStatus);

        // Open exactly above the USER INFO label / checkBox1 curved box.
        Control userInfoLabel = sender as Control;
        Point labelScreen = (userInfoLabel != null)
            ? userInfoLabel.PointToScreen(Point.Empty)
            : this.PointToScreen(new Point(0, 0));

        int x = labelScreen.X + ((userInfoLabel != null ? userInfoLabel.Width : 0) - _bnUserInfoPopup.Width) / 2;

        // Start touching frmMain at the USER INFO position, then slowly slide upward.
        int startY = this.Top;
        _bnUserInfoPopup.Location = new Point(x, startY);
        _bnUserInfoTargetY = this.Top - _bnUserInfoPopup.Height + 2;

        _bnUserInfoPopup.Show(this);
        _bnUserInfoPopup.BringToFront();
        _bnUserInfoTimer.Start();

        // Auto-close USER INFO exactly 20 seconds after opening.
        if (_bnUserInfoAutoCloseTimer != null)
        {
            _bnUserInfoAutoCloseTimer.Stop();
            _bnUserInfoAutoCloseTimer.Start();
        }
    }

    private void EnsureByteNovaUserInfoPopup()
    {
        if (_bnUserInfoPopup != null && !_bnUserInfoPopup.IsDisposed)
            return;

        _bnUserInfoPopup = new Form
        {
            FormBorderStyle = FormBorderStyle.None,
            ShowInTaskbar = false,
            StartPosition = FormStartPosition.Manual,
            Size = new Size(315, 151),
            BackColor = Color.FromArgb(31, 19, 43),
            TopMost = this.TopMost
        };
        _bnUserInfoPopup.Paint += ByteNovaUserInfoPopupPaint;

        Label title = MakeByteNovaUserInfoLabel("USER INFO", new Point(14, 8),
            new Size(285, 24), Color.FromArgb(0, 210, 255), 11F);

        Label idTitle = MakeByteNovaUserInfoLabel("CLIENT USER ID", new Point(14, 38),
            new Size(140, 22), Color.FromArgb(151, 171, 193), 9F);

        Label idValue = MakeByteNovaUserInfoLabel("", new Point(158, 38),
            new Size(140, 22), Color.White, 10F);
        idValue.Name = "bnUserIdValue";

        Label validityTitle = MakeByteNovaUserInfoLabel("VALIDITY", new Point(14, 65),
            new Size(140, 22), Color.FromArgb(151, 171, 193), 9F);

        Label validityValue = MakeByteNovaUserInfoLabel("", new Point(158, 65),
            new Size(140, 22), Color.FromArgb(255, 211, 71), 10F);
        validityValue.Name = "bnValidityValue";

        Label appVersionTitle = MakeByteNovaUserInfoLabel("VERSION", new Point(14, 92),
            new Size(140, 22), Color.FromArgb(151, 171, 193), 9F);

        Label appVersionValue = MakeByteNovaUserInfoLabel("", new Point(158, 92),
            new Size(140, 22), Color.FromArgb(118, 220, 255), 10F);
        appVersionValue.Name = "bnAppVersionValue";

        Label paymentStatusTitle = MakeByteNovaUserInfoLabel("PAYMENT STATUS", new Point(14, 119),
            new Size(140, 22), Color.FromArgb(151, 171, 193), 9F);

        Label paymentStatusValue = MakeByteNovaUserInfoLabel("", new Point(158, 119),
            new Size(140, 22), Color.FromArgb(55, 218, 119), 10F);
        paymentStatusValue.Name = "bnPaymentStatusValue";

        _bnUserInfoPopup.Controls.Add(title);
        _bnUserInfoPopup.Controls.Add(idTitle);
        _bnUserInfoPopup.Controls.Add(idValue);
        _bnUserInfoPopup.Controls.Add(validityTitle);
        _bnUserInfoPopup.Controls.Add(validityValue);
        _bnUserInfoPopup.Controls.Add(appVersionTitle);
        _bnUserInfoPopup.Controls.Add(appVersionValue);
        _bnUserInfoPopup.Controls.Add(paymentStatusTitle);
        _bnUserInfoPopup.Controls.Add(paymentStatusValue);

        _bnUserInfoTimer = new System.Windows.Forms.Timer();
        _bnUserInfoTimer.Interval = 20;
        _bnUserInfoTimer.Tick += ByteNovaUserInfoTimer_Tick;

        _bnUserInfoAutoCloseTimer = new System.Windows.Forms.Timer();
        _bnUserInfoAutoCloseTimer.Interval = 20000;
        _bnUserInfoAutoCloseTimer.Tick += ByteNovaUserInfoAutoCloseTimer_Tick;
    }

    private Label MakeByteNovaUserInfoLabel(string text, Point location, Size size, Color color, float fontSize)
    {
        return new Label
        {
            Text = text,
            Location = location,
            Size = size,
            AutoSize = false,
            BackColor = Color.Transparent,
            ForeColor = color,
            Font = new Font("Segoe UI Black", fontSize, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };
    }

    private void ByteNovaUserInfoAutoCloseTimer_Tick(object sender, EventArgs e)
    {
        if (_bnUserInfoAutoCloseTimer != null)
            _bnUserInfoAutoCloseTimer.Stop();

        if (_bnUserInfoTimer != null)
            _bnUserInfoTimer.Stop();

        if (_bnUserInfoPopup != null &&
            !_bnUserInfoPopup.IsDisposed &&
            _bnUserInfoPopup.Visible)
        {
            _bnUserInfoPopup.Hide();
        }
    }


    private void ByteNovaUserInfoTimer_Tick(object sender, EventArgs e)
    {
        if (_bnUserInfoPopup == null || _bnUserInfoPopup.IsDisposed || !_bnUserInfoPopup.Visible)
        {
            _bnUserInfoTimer.Stop();
            return;
        }

        int nextY = _bnUserInfoPopup.Top - 4;
        if (nextY <= _bnUserInfoTargetY)
        {
            _bnUserInfoPopup.Top = _bnUserInfoTargetY;
            _bnUserInfoTimer.Stop();
        }
        else
        {
            _bnUserInfoPopup.Top = nextY;
        }
    }

    private void ByteNovaUserInfoPopupPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

        Rectangle outer = new Rectangle(1, 1, c.Width - 3, c.Height - 3);
        Rectangle inner = new Rectangle(4, 4, c.Width - 9, c.Height - 9);

        // Soft cyan outer glow.
        using (GraphicsPath glowPath = CreateRoundedRectanglePath(outer, 15))
        using (Pen glow = new Pen(Color.FromArgb(90, 0, 205, 255), 3.2F))
            e.Graphics.DrawPath(glow, glowPath);

        // Premium dark navy glass-like card.
        using (GraphicsPath cardPath = CreateRoundedRectanglePath(inner, 13))
        using (LinearGradientBrush bg = new LinearGradientBrush(
            inner, Color.FromArgb(10, 57, 87), Color.FromArgb(16, 10, 21),
            LinearGradientMode.Vertical))
        using (Pen border = new Pen(Color.FromArgb(0, 215, 255), 1.5F))
        {
            e.Graphics.FillPath(bg, cardPath);
            e.Graphics.DrawPath(border, cardPath);
        }

        // Subtle top highlight and gold separator.
        using (Pen highlight = new Pen(Color.FromArgb(100, 118, 220, 255), 1F))
            e.Graphics.DrawLine(highlight, 17, 7, c.Width - 17, 7);

        using (Pen gold = new Pen(Color.FromArgb(255, 211, 71), 1.4F))
            e.Graphics.DrawLine(gold, 15, 28, c.Width - 15, 28);
    }


    private void SetupCompactButton(BunifuFlatButton button, string text, int x, int y)
    {
        button.AutoSize = false;

        Panel nav = _bnTopBar.Controls["bnCompactNavigation"] as Panel;
        if (nav == null) return;

        button.Parent = nav;
        button.Visible = true;
        button.Location = new Point(x, y);
        button.Size = new Size(90, 22);
        button.BorderRadius = 4;
        button.Normalcolor = Color.FromArgb(4, 25, 43);
        button.Activecolor = Color.FromArgb(0, 76, 130);
        button.OnHovercolor = Color.FromArgb(0, 96, 158);
        button.Textcolor = Color.White;
        button.OnHoverTextColor = Color.White;
        button.TextFont = new Font("Segoe UI Semibold", 7.6F, FontStyle.Bold);
        button.TextAlign = ContentAlignment.MiddleCenter;
        button.ButtonText = text;
        button.Text = text;
        button.BringToFront();
    }

    private void ByteNovaTokenButton_Click(object sender, EventArgs e)
    {
        new Token_count().ShowDialog();
    }

    private void SetCompactButtonShade(BunifuFlatButton button, Color normal, Color hover)
    {
        if (button == null) return;

        button.Normalcolor = normal;
        button.Activecolor = hover;
        button.OnHovercolor = hover;
        button.Textcolor = Color.White;
        button.OnHoverTextColor = Color.White;
    }


    private void LayoutCompactNavButtons()
    {
        if (_bnContent == null) return;

        Panel nav = _bnTopBar.Controls["bnCompactNavigation"] as Panel;
        if (nav == null) return;

        BunifuFlatButton[] buttons =
        {
            bunifuFlatButton7, bunifuFlatButton1, bunifuFlatButton3, bunifuFlatButton4,
            bunifuFlatButton2, bunifuFlatButton6, bunifuFlatButton8, _bnTokenButton
        };

        const int pad = 6;
        const int gap = 2;
        int bh = 22;

        // PROCESS_TICKET gets extra width so its caption stays comfortably on one line.
        const int processExtra = 34;
        int totalGap = gap * (buttons.Length - 1);
        int usable = nav.ClientSize.Width - (pad * 2) - totalGap - processExtra;
        int bw = Math.Max(70, usable / buttons.Length);

        int x = pad;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].Parent = nav;

            int currentWidth = (buttons[i] == bunifuFlatButton4)
                ? bw + processExtra
                : bw;

            buttons[i].Location = new Point(x, Math.Max(2, (nav.ClientSize.Height - bh) / 2));
            buttons[i].Size = new Size(currentWidth, bh);
            buttons[i].TextAlign = ContentAlignment.MiddleCenter;
            buttons[i].BringToFront();

            x += currentWidth + gap;
        }
    }

    private void ByteNovaCompactRootPaint(object sender, PaintEventArgs e)
    {
        Control c = (Control)sender;
        if (c.Width < 10 || c.Height < 10) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        using (LinearGradientBrush bg = new LinearGradientBrush(
            c.ClientRectangle,
            Color.FromArgb(22, 14, 28),
            Color.FromArgb(1, 7, 15),
            LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, c.ClientRectangle);

        using (Pen outerGlow = new Pen(Color.FromArgb(198, 244, 50), 2.2F))
            DrawRoundedRectangle(e.Graphics, outerGlow,
                new Rectangle(2, 2, c.Width - 5, c.Height - 5), 18);

    }

    private void ByteNovaCompactBrandPaint(object sender, PaintEventArgs e)
    {
        Label brand = sender as Label;
        if (brand == null) return;

        e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

        using (Font f = new Font("Segoe UI Black", 19F, FontStyle.Bold))
        using (StringFormat sf = (StringFormat)StringFormat.GenericTypographic.Clone())
        {
            sf.FormatFlags |= StringFormatFlags.NoClip;

            const string byteText = "Expr";
            const string novaText = "ess";

            // HD white "Byte" with a very soft highlight.
            using (Brush byteShadow = new SolidBrush(Color.FromArgb(85, 170, 205, 235)))
                e.Graphics.DrawString(byteText, f, byteShadow, 0.7F, 1.0F, sf);
            using (LinearGradientBrush byteBrush = new LinearGradientBrush(
                new RectangleF(0F, 0F, 76F, 31F),
                Color.White,
                Color.FromArgb(225, 236, 248),
                LinearGradientMode.Vertical))
                e.Graphics.DrawString(byteText, f, byteBrush, 0F, 0F, sf);

            SizeF byteSize = e.Graphics.MeasureString(byteText, f, PointF.Empty, sf);

            // Exact 2px gap between Byte and Nova.
            float novaX = byteSize.Width + 0F;
            RectangleF novaRect = new RectangleF(novaX, 0F, 82F, 32F);

            // Subtle blue glow behind Nova.
            using (Brush glow = new SolidBrush(Color.FromArgb(70, 150, 200, 20)))
                e.Graphics.DrawString(novaText, f, glow, novaX + 0.8F, 1.0F, sf);

            // Highlighted HD Nova: bright sky-blue top -> richer/darker blue bottom.
            using (LinearGradientBrush novaBrush = new LinearGradientBrush(
                novaRect,
                Color.FromArgb(198, 244, 50),
                Color.FromArgb(150, 200, 20),
                LinearGradientMode.Vertical))
                e.Graphics.DrawString(novaText, f, novaBrush, novaX, 0F, sf);

            // Fine top shine for the glossy/highlighted reference effect.
            using (LinearGradientBrush shine = new LinearGradientBrush(
                new RectangleF(novaX, 0F, 82F, 15F),
                Color.FromArgb(190, 205, 250, 255),
                Color.FromArgb(0, 205, 250, 255),
                LinearGradientMode.Vertical))
                e.Graphics.DrawString(novaText, f, shine, novaX, -0.35F, sf);
        }
    }

    private void ByteNovaCompactHeaderPaint(object sender, PaintEventArgs e)
    {
        Control c = (Control)sender;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (LinearGradientBrush bg = new LinearGradientBrush(
            c.ClientRectangle, Color.FromArgb(7, 33, 56), Color.FromArgb(3, 16, 29), LinearGradientMode.Horizontal))
            e.Graphics.FillRectangle(bg, c.ClientRectangle);
        using (Pen p = new Pen(Color.FromArgb(0, 87, 145), 1F))
            e.Graphics.DrawLine(p, 0, c.Height - 1, c.Width, c.Height - 1);
    }

    private void ByteNovaCompactStatusPaint(object sender, PaintEventArgs e)
    {
        Control c = (Control)sender;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (LinearGradientBrush bg = new LinearGradientBrush(
            c.ClientRectangle, Color.FromArgb(7, 30, 49), Color.FromArgb(4, 18, 31), LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, c.ClientRectangle);
        using (Pen p = new Pen(Color.FromArgb(0, 102, 173), 1.3F))
            DrawRoundedRectangle(e.Graphics, p, new Rectangle(0, 0, c.Width - 1, c.Height - 1), 16);
    }

    private void ByteNovaCompactSecurityPaint(object sender, PaintEventArgs e)
    {
        Control c = (Control)sender;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (LinearGradientBrush bg = new LinearGradientBrush(
            c.ClientRectangle, Color.FromArgb(7, 46, 42), Color.FromArgb(3, 24, 28), LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, c.ClientRectangle);
        using (Pen p = new Pen(Color.FromArgb(44, 213, 128), 1.2F))
            DrawRoundedRectangle(e.Graphics, p, new Rectangle(0, 0, c.Width - 1, c.Height - 1), 16);
    }

    private void ByteNovaCompactNavPaint(object sender, PaintEventArgs e)
    {
        Control c = (Control)sender;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (LinearGradientBrush bg = new LinearGradientBrush(
            c.ClientRectangle, Color.FromArgb(6, 31, 52), Color.FromArgb(3, 15, 27), LinearGradientMode.Horizontal))
            e.Graphics.FillRectangle(bg, c.ClientRectangle);
        using (Pen p = new Pen(Color.FromArgb(0, 129, 213), 1.5F))
            DrawRoundedRectangle(e.Graphics, p, new Rectangle(0, 0, c.Width - 1, c.Height - 1), 14);
    }


    private void EnableByteNovaWholeFormDragging(Control root)
    {
        if (root == null) return;

        // Do not steal mouse input from controls that need normal clicking/editing.
        bool interactive =
            root is ButtonBase ||
            root is TextBoxBase ||
            root is ComboBox ||
            root is ListBox ||
            root is ListView ||
            root is TreeView ||
            root is NumericUpDown ||
            root is DateTimePicker ||
            root is LinkLabel ||
            root is MenuStrip ||
            root is ToolStrip;

        // USER INFO is intentionally clickable, so keep its click behavior.
        if (root.Name == "bnUserInfoLabel")
            interactive = true;

        if (!interactive)
        {
            root.MouseDown -= ByteNovaDrag_MouseDown;
            root.MouseMove -= ByteNovaDrag_MouseMove;
            root.MouseUp -= ByteNovaDrag_MouseUp;

            root.MouseDown += ByteNovaDrag_MouseDown;
            root.MouseMove += ByteNovaDrag_MouseMove;
            root.MouseUp += ByteNovaDrag_MouseUp;
        }

        foreach (Control child in root.Controls)
            EnableByteNovaWholeFormDragging(child);
    }


    private void ByteNovaDrag_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;

        _bnDragging = true;
        _bnDragCursorStart = Cursor.Position;
        _bnDragFormStart = Location;

        Control c = sender as Control;
        if (c != null) c.Capture = true;
    }

    private void ByteNovaDrag_MouseMove(object sender, MouseEventArgs e)
    {
        if (!_bnDragging || e.Button != MouseButtons.Left) return;

        Point current = Cursor.Position;
        int dx = current.X - _bnDragCursorStart.X;
        int dy = current.Y - _bnDragCursorStart.Y;

        Location = new Point(
            _bnDragFormStart.X + dx,
            _bnDragFormStart.Y + dy);

        App_Loc = Location;
    }

    private void ByteNovaDrag_MouseUp(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;

        _bnDragging = false;
        Control c = sender as Control;
        if (c != null) c.Capture = false;

        App_Loc = Location;
    }

    private void PositionByteNovaCompact()
    {
        try
        {
            Rectangle area = Screen.FromControl(this).WorkingArea;

            int width = (int)(area.Width * 0.62);
            width = Math.Max(920, width);
            width = Math.Min(width, 1120);
            width = Math.Min(width, area.Width - 18);

            int height = 88;

            Width = width;
            Height = height;
            Left = area.Left + Math.Max(0, (area.Width - Width) / 2);
            Top = area.Bottom - Height;
        }
        catch { }
    }

    private void UpdateByteNovaCompactClock()
    {
        DateTime now = DateTime.Now;
        if (_bnClockTitle != null)
            _bnClockTitle.Text = now.ToString("dd MMM yyyy  |  HH:mm:ss");
        if (_bnPageSub != null)
            _bnPageSub.Text = "";

        if (_bnPaidValue != null)
        {
            string status = Convert.ToString(Program.KeyStatus);
            _bnPaidValue.Text = string.IsNullOrWhiteSpace(status) ? "PAID" : status.ToUpperInvariant();
            _bnPaidValue.ForeColor = status.Equals("Paid", StringComparison.OrdinalIgnoreCase)
                ? BN_GREEN : BN_GOLD;
        }
    }

    private void frmMain_Load(object sender, EventArgs e)
    {
        string erPlce = "Loading Main Windows";
        Program.ErPlce = erPlce;
        string string_ = "Program.Main";
        Class28.smethod_26(string_, Program.ErPlce);
        Rectangle workingArea = Screen.GetWorkingArea(this);
        int_0 = workingArea.Width / 2;
        pnlHeader.Width = base.Width;
        base.Left = workingArea.Left + Math.Max(0, (workingArea.Width - base.Width) / 2);
        BunifuCustomLabel bunifuCustomLabel = lbl_Date;
        DateTime now = DateTime.Now;
        string text = "dd-MM-yyyy HH:mm:ss";
        bunifuCustomLabel.Text = now.ToString(text);
        base.Top = workingArea.Bottom - base.Height;
        PositionByteNovaCompact();
        ApplyCompactResponsiveLayout();
        App_Loc = base.Location;
        StatClass.TopMargin = PHei;
        StatClass.LeftMargin = 0;
        Staticm.AfterDllDownload(0);
        CheckForAutoFill("");
        ThreadPool.QueueUserWorkItem(CheckForAutoFilll, "");
        ThreadPool.QueueUserWorkItem(method_1);
        uPIPayByOtherIDToolStripMenuItem.PerformClick();
        Program.ExpiryDay++;
        Label label = bunifuCustomLabel5;
        string[] array = new string[5];
        array[1] = Program.mySettings_0.userID;
        label.Text = string.Concat(array);
        if (Program.mySettings_0.CloseOnPnr)
        {
            checkBox1.Checked = true;
            StatClass.CloseOnPnr = true;
        }
        bunifuCustomLabel2.Text = Program.ExpiryDay.ToString() + " Days";
        if (Program.KeyStatus == "Paid")
        {
            bunifuCustomLabel3.Text = "Recived";
        }
        else
        {
            bunifuCustomLabel3.Text = "UNPAID";
        }
        timer_0.Enabled = true;
        string path = "IRLOG";
        if (!Directory.Exists(path))
        {
            string path2 = "IRLOG";
            Directory.CreateDirectory(path2);
        }
        StatClass.operalogin = false;
        string strongPassword = Program.StrongPassword;
        string text4 = "0";
        if (strongPassword != text4)
        {
            enableStrongPasswordToolStripMenuItem.Checked = true;
        }
        StatClass.VideoMode = false;
        checkBox1.Checked = false;
        checkBox6.Checked = true;
        checkBox8.Checked = true;
        checkBox10.Checked = true;
        checkBox9.Checked = false;
        if (Program.WEBTEXT == "WEBMODE-1")
        {
            chkADV.Checked = true;
        }
        if (Program.WEBTEXT == "WEBMODE-2")
        {
            checkBox3.Checked = true;
        }
        if (Program.WEBTEXT == "WEBMODE-3")
        {
            checkBox5.Checked = true;
        }
    }

    public static void IntervalInDays(int hour, int min, double interval, Action task)
    {
        interval *= 24.0;
        SchedulerService.Instance.ScheduleTask(hour, min, interval, task);
    }

    private void timer_0_Tick(object sender, EventArgs e)
    {
        BunifuCustomLabel bunifuCustomLabel = lbl_Date;
        int minut = DateTime.Now.Minute;
        if (minut > 45)
        {
            if (linkLabel1.Enabled = true)
            {
                linkLabel1.Enabled = false;
            }
        }
        DateTime now = DateTime.Now;
        string text = "dd-MM-yyyy HH:mm:ss";
        bunifuCustomLabel.Text = now.ToString(text);

        // Keep the new ByteNova clock/date updated every second as well.
        UpdateByteNovaCompactClock();
    }

    private void method_0()
    {
        string text = "dd-MMM-yyyy";
        string value = text;
        RegistryKey currentUser = Registry.CurrentUser;
        string name = "Control Panel\\International";
        RegistryKey registryKey = currentUser.OpenSubKey(name, writable: true);
        string name2 = "sShortDate";
        registryKey.SetValue(name2, value);
        registryKey.Close();
    }

    private void method_1(object object_0)
    {
        try
        {
            method_0();
            string systemTimeZone = "India Standard Time";
            SetSystemTimeZone(systemTimeZone);
            string errMessage = "";
            mCOMM mcommInstance_ = Program.mcommInstance_;
            string userID = "update_time";
            string pwd = "update_time";
            mcommInstance_.isLV(userID, pwd, ref errMessage);
            mCOMM mcommInstance_2 = Program.mcommInstance_;
            string key = "test";
            mcommInstance_2.GetData(key);
        }
        catch (Exception)
        {
        }
    }

    public void SetSystemTimeZone(string timeZoneId)
    {
        string text = "";
        try
        {
            text = TimeZone.CurrentTimeZone.StandardName;
        }
        catch
        {
        }
        if (!(text != timeZoneId))
        {
            return;
        }
        try
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            string fileName = "tzutil.exe";
            processStartInfo.FileName = fileName;
            string text2 = "/s \"";
            string text3 = "\"";
            processStartInfo.Arguments = text2 + timeZoneId + text3;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            Process process = Process.Start(processStartInfo);
            if (process != null)
            {
                process.WaitForExit();
                TimeZoneInfo.ClearCachedData();
            }
        }
        catch
        {
        }
    }

    private string method_14()
    {
        DateTime utcNow = DateTime.UtcNow;
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return ((long)(utcNow - dateTime).TotalMilliseconds).ToString();
    }

    private void Update_Time00()
    {
        try
        {
            string requestUriString = "https://www.contents.irctc.co.in/eticketing/protected/mapps1/loginCaptcha?nlpCaptchaException=nil";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
            httpWebRequest.ServerCertificateValidationCallback = frmLogin.CertificateValidation;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            string userAgent = "IRCTC Rail Connect/3.1.64iOS";
            httpWebRequest.UserAgent = userAgent;
            string contentType = "application/json; charset=utf-8";
            httpWebRequest.ContentType = contentType;
            string accept = "application/json";
            httpWebRequest.Accept = accept;
            WebHeaderCollection headers = httpWebRequest.Headers;
            string name = "accept-language";
            string value = "en-IN,en;q=0.9";
            headers.Add(name, value);
            WebHeaderCollection headers2 = httpWebRequest.Headers;
            string name2 = "bmirak";
            string value2 = "iosbm";
            headers2.Add(name2, value2);
            WebHeaderCollection headers3 = httpWebRequest.Headers;
            string name3 = "mup";
            string value3 = "C40E54B7-A374-4EF7-B184-73F6905B4E6C";
            headers3.Add(name3, value3);
            WebHeaderCollection headers4 = httpWebRequest.Headers;
            string name4 = "greq";
            string value4 = "irC40E54B76=A3744=4EF70=B1841=73F6905B4E6C3";
            headers4.Add(name4, value4);
            WebHeaderCollection headers5 = httpWebRequest.Headers;
            string name5 = "Content-Language";
            string value5 = "en";
            headers5.Add(name5, value5);
            string method = "GET";
            httpWebRequest.Method = method;
            httpWebRequest.Timeout = 10000;
            httpWebRequest.ReadWriteTimeout = 10000;
            using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
            string text = streamReader.ReadToEnd();
            string value6 = "timeStamp";
            string text2 = text.Substring(text.IndexOf(value6) + 23);
            string text3 = text2;
            string text4 = text2;
            string value7 = "\"";
            text2 = text3.Substring(0, text4.IndexOf(value7));
            _ = new TimeSpan(DateTime.Parse(text2).Ticks - DateTime.Now.Ticks).TotalMilliseconds;
        }
        catch (Exception)
        {
        }
    }

    internal static void smethod_0(object sender, EventArgs e)
    {
        try
        {
            frmProcessTicket frmProcessTicket2 = (frmProcessTicket)sender;
            if (frmProcessTicket2.Tag != null)
            {
                int key = (int)frmProcessTicket2.Tag;
                if (Class28.dictionary_0[key] <= 0)
                {
                    Interlocked.Decrement(ref Class28.int_4);
                    Class28.dictionary_0.Remove(key);
                }
            }
        }
        catch
        {
        }
    }

    private bool method_2(string string_1)
    {
        try
        {
            int num = frmProcessTicket.WindowCount();
            if (num < Program.WindowCount)
            {
                if (Class28.int_4 >= Program.int_2)
                {
                    Form owner = Program.TopMost_();
                    string text = "Sorry, you do not have sufficient credits!";
                    string caption = "Limitation";
                    MessageBox.Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return false;
                }
                string delimiter = "^#^";
                string[] array = Strings.Split(string_1, delimiter);
                if (array.Length > 4)
                {
                    StatClass.lblTktName = array[0];
                    StatClass.int_TotalNoOfPair = int.Parse(array[1]);
                    StatClass.int_NoOfConnect = int.Parse(array[2]);
                    chAuto = bool.Parse(array[3]);
                    StatClass.TimeStr = array[4];
                }
                if (Class28.dictionary_9.ContainsKey(StatClass.lblTktName))
                {
                    int value = (int_3 = Class28.dictionary_9[StatClass.lblTktName] + 1);
                    Class28.dictionary_9[StatClass.lblTktName] = value;
                    int_4 = Class28.dictionary_8[StatClass.lblTktName];
                }
                else
                {
                    Interlocked.Increment(ref StatClass.TotalOpendTicket);
                    int_4 = StatClass.TotalOpendTicket;
                    Class28.dictionary_8.Add(StatClass.lblTktName, StatClass.TotalOpendTicket);
                    int_3 = 0;
                    Class28.dictionary_9.Add(StatClass.lblTktName, int_3);
                }
                Color color_ = Color.FromArgb(255, 255, 255);
                switch (StatClass.TotalOpendTicket)
                {
                    case 11:
                        color_ = Color.Azure;
                        break;
                    case 10:
                        color_ = Color.Turquoise;
                        break;
                }
                if (Class28.int_4 + 1 > Program.int_Slot)
                {
                }
                frmProcessTicket frmProcessTicket2 = new frmProcessTicket(string_1, color_, bool_1: false, int_4, int_3);
                frmProcessTicket2.FormClosed += smethod_0;
                frmProcessTicket2.Show();
                frmProcessTicket2.Activate();
                frmProcessTicket2.ThreadProcUnsafe(0);
                Interlocked.Increment(ref Class28.int_4);
                return true;
            }
            Form owner2 = Program.TopMost_();
            string text2 = "You cannot Open more Than ";
            string text3 = num.ToString();
            string text4 = " Tickets Window at same time.";
            string text5 = text2 + text3 + text4;
            string caption2 = "Express Limitation";
            MessageBox.Show(owner2, text5, caption2, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private void bunifuFlatButton5_Click(object sender, EventArgs e)
    {
        if (Program.UnpaidKey)
        {
            string text = "Booking Karne ke Liye ID PAID karayen";
            string caption = "ID UnPaid";
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
        else if (!StatClass.ResetReq)
        {
            if (StatClass.IsReadydll)
            {
                frmManageTicket frmManageTicket2 = new frmManageTicket();
                try
                {
                    if (frmManageTicket2.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }
                    foreach (string item in StatClass.Tktlist_0)
                    {
                        if (!method_2(item))
                        {
                            break;
                        }
                    }
                    return;
                }
                catch
                {
                    return;
                }
                finally
                {
                    frmManageTicket2.Dispose();
                }
            }
            string text2 = "Some Files Have Been Downloading, Wait Some Time.\r\nPlease Wait We Are Checking Your System File And Updating is Pending.";
            string caption2 = "Express Manager";
            MessageBox.Show(text2, caption2, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else
        {
            string text3 = "Restart Application Before Open Ticket";
            string caption3 = "Restart Required";
            MessageBox.Show(text3, caption3, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
    }

    private void bunifuFlatButton7_Click(object sender, EventArgs e)
    {
        Form2 frmAboutUS2 = new Form2();
        frmAboutUS2.Show();
        frmAboutUS2.Activate();
    }

    private void bunifuFlatButton8_Click(object sender, EventArgs e)
    {
        new Freecharge().ShowDialog();
    }

    private void frmMain_KeyDown(object sender, KeyEventArgs e)
    {
        string text = e.KeyCode.ToString();
        string text2 = "F6";
        if (!(text == text2))
        {
            string text3 = e.KeyCode.ToString();
            string text4 = "F7";
            if (text3 == text4)
            {
                BunifuFlatButton3_Click(sender, e);
                return;
            }
            string text5 = e.KeyCode.ToString();
            string text6 = "F8";
            if (text5 == text6)
            {
                bunifuFlatButton7_Click(sender, e);
                return;
            }
            string text7 = e.KeyCode.ToString();
            string text8 = "F9";
            if (text7 == text8)
            {
                bunifuFlatButton8_Click(sender, e);
                return;
            }
            string text9 = e.KeyCode.ToString();
            string text10 = "F10";
            if (text9 == text10)
            {
                C3AA0.PerformClick();
            }
        }
        else
        {
            bunifuFlatButton5_Click(sender, e);
        }
    }

    public void Disalbed_Form_Click(bool Cond)
    {
    }

    private void AutoFillCaptchaToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Program.mySettings_0.isAutoCaptcha = AutoFillCaptchaToolStripMenuItem.Checked;
        Program.AutofillOCR = AutoFillCaptchaToolStripMenuItem.Checked;
        mCOMM mcommInstance_ = Program.mcommInstance_;
        string key = "AutofillCp";
        mcommInstance_.SetData(key, AutoFillCaptchaToolStripMenuItem.Checked.ToString());
        string variable = "AutofillCp";
        Environment.SetEnvironmentVariable(variable, AutoFillCaptchaToolStripMenuItem.Checked.ToString(), EnvironmentVariableTarget.Process);
        if (Program.AutofillOCR)
        {
            Program.AutoFillAPI = false;
            autoFillCaptchaByAPIToolStripMenuItem.Checked = false;
        }
    }

    private void AutoSubmitCaptchaAfterFill_Click(object sender, EventArgs e)
    {
        Program.mySettings_0.isAutoSubmitCaptcha = AutoSubmitCaptchaAfterFill.Checked;
        Program.autoSubmitCaptcha = AutoSubmitCaptchaAfterFill.Checked;
        mCOMM mcommInstance_ = Program.mcommInstance_;
        string key = "AutoSubmitCp";
        mcommInstance_.SetData(key, AutoSubmitCaptchaAfterFill.Checked.ToString());
        string variable = "AutoSubmitCp";
        Environment.SetEnvironmentVariable(variable, AutoSubmitCaptchaAfterFill.Checked.ToString(), EnvironmentVariableTarget.Process);
    }

    private void autoFillCaptchaByAPIToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Program.AutoFillAPI = autoFillCaptchaByAPIToolStripMenuItem.Checked;
        if (Program.AutoFillAPI)
        {
            Program.AutofillOCR = false;
            AutoFillCaptchaToolStripMenuItem.Checked = false;
        }
    }


    private void C0AA0(object sender, EventArgs e)
    {
        frmNotes frmNotes2 = new frmNotes(3, 0);
        frmNotes2.Activate();
        frmNotes2.Show();
    }

    private void googleOCRToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
    {
        if (googleOCRToolStripMenuItem.Checked)
        {
            string variable = "OCR";
            string value = "0";
            Environment.SetEnvironmentVariable(variable, value, EnvironmentVariableTarget.Process);
        }
    }

    private void googleOCRToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (!googleOCRToolStripMenuItem.Checked)
        {
            return;
        }
        string c_name = "Microsoft Visual C++";
        Program.bool_VSC = Program.checkInstalled(c_name);
        if (!Program.bool_VSC)
        {
            googleOCRToolStripMenuItem.Checked = false;
            Form owner = new Form
            {
                TopMost = true
            };
            string text = "Google OCR use Karne Ke Liye Kuch File Aapke System me Zaroori hoti hai, Agar Aapne Abhi Tak File Download Karke Install Nahi ki hai to YES Button Ko Click karke Download Karke Install Kariye\r\nAgar Aapne File Install Kar di hai to NO Button Click Kariye";
            string caption = "nlpCaptcha OCR Extention";
            if (MessageBox.Show(owner, text, caption, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string fileName = "https://download.microsoft.com/download/6/A/A/6AA4EDFF-645B-48C5-81CC-ED5963AEAD48/vc_redist.x86.exe";
                Process.Start(fileName);
            }
        }
    }

    private void showHistoryToolStripMenuItem_Click(object sender, EventArgs e)
    {
        frmShowHistory frmShowHistory2 = new frmShowHistory();
        frmShowHistory2.Show();
        frmShowHistory2.Activate();
    }

    private void B6AAA_Click(object sender, EventArgs e)
    {
        ThreadPool.QueueUserWorkItem(method_3);
    }

    private void method_3(object object_0)
    {
        string path = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";
        if (File.Exists(path))
        {
            new Stopwatch().Start();
            mCOMM mcommInstance_ = Program.mcommInstance_;
            string key = "check_reCaptcha";
            mcommInstance_.GetData(key);
        }
    }

    private void C6AA0_Click(object sender, EventArgs e)
    {
    }

    public void method_4(object object_0)
    {
        string text = "C:\\Program Files\\Google\\Chrome Beta\\Application\\chrome.exe";
        string text2 = text;
        if (!File.Exists(text2))
        {
            Form owner = new Form
            {
                TopMost = true
            };
            string text3 = "Please Install Chromium Web Browser";
            string caption = "Chromium";
            MessageBox.Show(owner, text3, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
        }
        Form owner2 = new Form
        {
            TopMost = true
        };
        string text4 = "Do you Want to Open IRCTC Home Page on Chromium for reCaptcha Check";
        string caption2 = "reCaptcha Checking";
        DialogResult dialogResult = MessageBox.Show(owner2, text4, caption2, MessageBoxButtons.YesNo);
        if (dialogResult != DialogResult.Yes)
        {
            return;
        }
        string processName = "chrome";
        Process[] processesByName = Process.GetProcessesByName(processName);
        for (int i = 0; i < processesByName.Length; i++)
        {
            processesByName[i].Kill();
        }
        Thread.Sleep(500);
        string arguments = "https://www.irctc.co.in/nget/assets/images/secondry-logo.png";
        Process.Start(text2, arguments);
        Form owner3 = new Form
        {
            TopMost = true
        };
        string text5 = "Click on YES Button After Load IRCTC on Chromium";
        string caption3 = "Wait Untill Page Load";
        MessageBox.Show(owner3, text5, caption3, MessageBoxButtons.YesNo);
        if (dialogResult == DialogResult.Yes)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            mCOMM mcommInstance_ = Program.mcommInstance_;
            string key = "check_reCaptcha2";
            string data = mcommInstance_.GetData(key);
            stopwatch.Stop();
            double totalMilliseconds = stopwatch.Elapsed.TotalMilliseconds;
            string text6 = "Solve";
            if (data == text6)
            {
                Form owner4 = new Form
                {
                    TopMost = true
                };
                string text7 = "reCaptcha Solved Successfully, you can Proceed for Booking";
                string caption4 = "Success";
                MessageBox.Show(owner4, text7, caption4, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Form owner5 = new Form
                {
                    TopMost = true
                };
                string text8 = "reCaptcha Solving Time :- ";
                string text9 = totalMilliseconds.ToString();
                string text10 = "   Milliseconds";
                string text11 = text8 + text9 + text10;
                string caption5 = "Time";
                MessageBox.Show(owner5, text11, caption5, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                Form owner6 = new Form
                {
                    TopMost = true
                };
                string text12 = "reCaptcha Fail, Please Reinstall XAMPP and SQL Connector and Install reCaptcha Solver Extesion on chrome";
                string caption6 = "Fail";
                MessageBox.Show(owner6, text12, caption6, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
    }

    private void B1AAA(object sender, EventArgs e)
    {
        string variable = "UPI_AUTO";
        Environment.SetEnvironmentVariable(variable, Convert.ToBoolean(uPIPayByOtherIDToolStripMenuItem.CheckState).ToString());
    }


    private void B2AAA(object sender, EventArgs e)
    {
        frmAboutUS frmAboutUS2 = new frmAboutUS();
        frmAboutUS2.Show();
        frmAboutUS2.Activate();
    }

    private void C2AA0(object sender, EventArgs e)
    {
        StatClass.extract_extension(0);
        StatClass.extract_extension_Chromium(0);
    }

    private void clearCacheToolStripMenuItem_Click(object sender, EventArgs e)
    {
        try
        {
            Class28.smethod_31();
            if (Program.bool_8)
            {
                string text = "Optimization Successful!";
                string caption = "Optimized";
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                string text2 = "Optimization not Required or Failed.";
                string caption2 = "Optimized";
                MessageBox.Show(text2, caption2, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        catch
        {
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        string text = "Kya AAP ByteNova Software Close Karna Chahte Ho.\r\n\r\nPress Yes To Close Othwise Press No?";
        string caption = "Exit";
        if (MessageBox.Show(text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            string exitNAME = "2nget";
            Program.exitNAME = exitNAME;
            string text2 = "nget";
            string_0 = text2;
            Environment.Exit(0);
        }
    }

    private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
    {
        try
        {
            mySettings.smethod_2(Program.mySettings_0);
            string text = string_0;
            string text2 = "nget";
            if (text != text2)
            {
                e.Cancel = true;
            }
            string text3 = e.CloseReason.ToString();
            string text4 = "\t\t     ";
            string string_ = text3 + text4 + sender.ToString();
            string path = "Recog_CShape.exe";
            if (File.Exists(path))
            {
                string path2 = "Recog_CShape.exe";
                File.Delete(path2);
            }
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.StartupPath);
            string searchPattern = "*.txt";
            FileInfo[] files = directoryInfo.GetFiles(searchPattern);
            FileInfo[] array = files;
            foreach (FileInfo fileInfo in array)
            {
                File.Delete(Path.Combine(fileInfo.DirectoryName, fileInfo.Name));
            }
            string string_2 = "frmMain_FormClosing";
            Class28.smethod_26(string_2, string_);
        }
        catch
        {
        }
    }

    private void bunifuFlatButton1_Click(object sender, EventArgs e)
    {
        new frmProxySetting().Show();


        //options frmAboutUS2 = new options();
        //frmAboutUS2.Show();
        //frmAboutUS2.Activate();
    }

    private void BunifuFlatButton2_Click(object sender, EventArgs e)
    {
        new frmAddID().Show();
    }

    private void BunifuFlatButton3_Click(object sender, EventArgs e)
    {
        frmAddTkt frmAddTkt2 = new frmAddTkt();
        frmAddTkt2.dictionary_1 = Program.dictionary_0;
        frmAddTkt2.ShowDialog();
        frmAddTkt2.Dispose();
    }

    private void NewTicketToolStripMenuItem_Click(object sender, EventArgs e)
    {
        frmAddTkt frmAddTkt2 = new frmAddTkt();
        frmAddTkt2.dictionary_1 = Program.dictionary_0;
        frmAddTkt2.ShowDialog();
        frmAddTkt2.Dispose();
    }

    private void AddPaymentOptionToolStripMenuItem_Click(object sender, EventArgs e)
    {
        new frmCombine().ShowDialog();
    }

    private void AddIRCTCIDToolStripMenuItem_Click(object sender, EventArgs e)
    {
        new frmAddID().Show();
    }

    private void SBIOTPByPassToolStripMenuItem_Click(object sender, EventArgs e)
    {
        new frmNewSBI().Show();
    }

    private void HDFCOTPBYPassToolStripMenuItem_Click(object sender, EventArgs e)
    {
        new frmByPassHDFC().Show();
    }

    private void ProxyIPSettingToolStripMenuItem_Click(object sender, EventArgs e)
    {
        new frmProxySetting().Show();
    }

    private void PayTMOTPByPassToolStripMenuItem_Click(object sender, EventArgs e)
    {
        new frmPaytmB().Show();
    }

    private void ToolStripSpeedUP_Click(object sender, EventArgs e)
    {
        string variable = "S_Window";
        Environment.SetEnvironmentVariable(variable, Convert.ToBoolean(toolStripSpeedUP.CheckState).ToString());
    }

    private void chkADV_CheckedChanged(object sender, EventArgs e)
    {
        if (isUpdating) return;
        isUpdating = true;
        if (chkADV.Checked)
            checkBox3.Checked = false;
        checkBox5.Checked = false;
        StatClass.bookingmode3 = false;
        StatClass.bookingmode2 = false;
        StatClass.bookingmode1 = chkADV.Checked;
        isUpdating = false;
    }

    private void enableStrongPasswordToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (!enableStrongPasswordToolStripMenuItem.Checked)
        {
            string prompt = "Enter Strong Password";
            string title = "Password";
            string text = Interaction.InputBox(prompt, title);
            string prompt2 = "Re-Enter Strong Password";
            string title2 = "Password";
            string text2 = Interaction.InputBox(prompt2, title2);
            if (text != text2)
            {
                string text3 = "Password Mismatch";
                MessageBox.Show(text3);
                enableStrongPasswordToolStripMenuItem.Checked = false;
            }
            else
            {
                Program.StrongPassword = text;
                Class30.UpdateStrongPassword(Program.StrongPassword);
                enableStrongPasswordToolStripMenuItem.Checked = true;
                string text4 = "Strong Password Set Successfully";
                MessageBox.Show(text4);
            }
        }
        else
        {
            string text5 = "Do You Want to Remove Strong Password";
            string caption = "Password";
            if (MessageBox.Show(text5, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string strongPassword = "0";
                Program.StrongPassword = strongPassword;
                string pass = "0";
                Class30.UpdateStrongPassword(pass);
                enableStrongPasswordToolStripMenuItem.Checked = false;
                string text6 = "Strong Password Removed Successfully";
                MessageBox.Show(text6);
            }
        }
    }

    private void freeChargeUPIByPassToolStripMenuItem_Click(object sender, EventArgs e)
    {
        new Freecharge_htm().ShowDialog();
    }

    private void aXISBhimUPIAddByPassToolStripMenuItem_Click(object sender, EventArgs e)
    {
        mCOMM mcommInstance_ = Program.mcommInstance_;
        string key = "StartAxisBhim";
        mcommInstance_.SetData(key, "");
    }

    private void phonePeUPIByPassToolStripMenuItem1_Click(object sender, EventArgs e)
    {
        mCOMM mcommInstance_ = Program.mcommInstance_;
        string key = "StartPhonePe";
        mcommInstance_.SetData(key, "");
    }

    private void autoChangeDestinationStationToolStripMenuItem_Click(object sender, EventArgs e)
    {
        string value = autoChangeDestinationStationToolStripMenuItem.Checked.ToString();
        string variable = "SwitchDEST";
        Environment.SetEnvironmentVariable(variable, value);
    }

    private void autoChangeSourceStationToolStripMenuItem_Click(object sender, EventArgs e)
    {
        string variable = "SwitchSRC";
        Environment.SetEnvironmentVariable(variable, autoChangeSourceStationToolStripMenuItem.Checked.ToString());
    }

    private void nPCIUPIByPassToolStripMenuItem_Click(object sender, EventArgs e)
    {
        new frmNPCI().ShowDialog();
    }

    private void reInstallChromeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        ChromeResetRunner.Run();
    }

    private void sBIUPIByPassToolStripMenuItem_Click(object sender, EventArgs e)
    {
        new SbiUpiByPass().ShowDialog();
    }

    private void solveToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Program.PaymentIPBloack = solveToolStripMenuItem.Checked;
    }

    private void fixIPBlockManuallyClickSIGNINButtonToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Program.FixIPBloack = fixIPBlockManuallyClickSIGNINButtonToolStripMenuItem.Checked;
    }

    private void aDDAadhar4DigitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        new frmAddAadhar().ShowDialog();
    }

    private void showPassengerCaptchaWindowToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Program.showCaptcha = showPassengerCaptchaWindowToolStripMenuItem.Checked;
    }

    private void checkTokenToolStripMenuItem_Click(object sender, EventArgs e)
    {
        frmToken frmToken = new frmToken();
        frmToken.Show();
        frmToken.Activate();
    }

    internal async Task StartEdgeAndConnect()
    {
        isChrome = false;
        try
        {
            int port = 9234;
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                string fileName = "cmd.exe";
                processStartInfo.FileName = fileName;
                string arguments = "/c taskkill /F /IM msedge.exe";
                processStartInfo.Arguments = arguments;
                processStartInfo.CreateNoWindow = true;
                processStartInfo.UseShellExecute = false;
                ProcessStartInfo kill = processStartInfo;
                await Task.Run(delegate
                {
                    Process.Start(kill)?.WaitForExit();
                });
            }
            catch
            {
            }
            await Task.Delay(200);
            string text = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe";
            string text2 = text;
            string format = "--remote-debugging-port={0} ";
            string obj2 = string.Format(format, port);
            string text3 = "--remote-debugging-address=127.0.0.1 --no-first-run --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --disable-features=msEdgeSync ";
            string text4 = obj2 + text3;
            ProcessStartInfo processStartInfo2 = new ProcessStartInfo();
            string fileName2 = "cmd.exe";
            processStartInfo2.FileName = fileName2;
            string text5 = "/c start \"\" \"";
            string text6 = "\" ";
            processStartInfo2.Arguments = text5 + text2 + text6 + text4;
            processStartInfo2.CreateNoWindow = true;
            processStartInfo2.UseShellExecute = false;
            Process.Start(processStartInfo2);
            await Task.Delay(5000);
            IBrowserType chromium = (await Playwright.CreateAsync()).Chromium;
            string format2 = "http://127.0.0.1:{0}";
            IBrowserContext browserContext = (await chromium.ConnectOverCDPAsync(string.Format(format2, port))).Contexts[0];
            IPage page2 = ((browserContext.Pages.Count <= 0) ? (await browserContext.NewPageAsync()) : browserContext.Pages[0]);
            IPage page = page2;
            CaptureIRCTCRequests(page);
            string url = "https://www.irctc.co.in/nget/train-search";
            await page.GotoAsync(url, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded
            });
            while (!mProcess)
            {
                await Task.Delay(1000);
            }
            Program.BookingAllowEdge = true;
            if (page != null)
            {
                await page.CloseAsync();
            }
            Form owner = new Form
            {
                TopMost = true
            };
            string text7 = "IP Block Fix Successfully, Ab Aap Booking Process kar sakte hain";
            string caption = "Express";
            MessageBox.Show(owner, text7, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        catch (Exception)
        {
        }
    }

    internal void CaptureIRCTCRequests(IPage page)
    {
        page.Response += async delegate (object sender, IResponse response)
        {
            string method = response.Request.Method;
            string text = "OPTIONS";
            if (!(method == text))
            {
                string url = response.Url;
                string value = "insurenceApplicable=";
                if (url.Contains(value))
                {
                    try
                    {
                        string text2 = "Check_IP_Block";
                        string fileName = text2;
                        if (isChrome)
                        {
                            string text3 = "Check_IP_Block_Chrome";
                            fileName = text3;
                        }
                        IBrowserContext context = page.Context;
                        IReadOnlyList<BrowserContextCookiesResult> allCookies = await context.CookiesAsync();
                        await context.CookiesAsync();
                        string input = JsonConvert.SerializeObject(allCookies.Where(delegate (BrowserContextCookiesResult c)
                        {
                            string domain = c.Domain;
                            string value2 = "irctc.co.in";
                            return domain.Contains(value2);
                        }).ToList(), Formatting.Indented);
                        string text4 = "ec3104c5318c8f0753fdc13faa540020cae645c4b43f3795a0f2f9fc94328326";
                        string symmetricKey = text4;
                        string contents = Staticm.EncryptAES(input, symmetricKey);
                        File.WriteAllText(fileName, contents);
                        Program.BookingAllowEdge = true;
                        mProcess = true;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        };
    }

    private async void checkIPBlockToolStripMenuItem_Click_1(object sender, EventArgs e)
    {
        string path = "Check_IP_Block";
        if (File.Exists(path))
        {
            Form owner = new Form
            {
                TopMost = true
            };
            string text = "Aapka Pehle ka IP Block Fix Save hai\nIP Block Fix karne ki Zaroorat nahi hai\nAgar New IP Block Fix chahte hain to YES Button Click Karen";
            string caption = "Express";
            if (MessageBox.Show(owner, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                await StartEdgeAndConnect();
            }
        }
        else
        {
            await StartEdgeAndConnect();
        }
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing && B3AAA != null)
        {
            B3AAA.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        BunifuAnimatorNS.Animation animation3 = new BunifuAnimatorNS.Animation();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
        BunifuAnimatorNS.Animation animation4 = new BunifuAnimatorNS.Animation();
        this.lblUsrMsg = new System.Windows.Forms.Label();
        this.bunifuElipse_0 = new Bunifu.Framework.UI.BunifuElipse(this.components);
        this.bunifuTransition_0 = new BunifuAnimatorNS.BunifuTransition(this.components);
        this.C3AA0 = new System.Windows.Forms.Button();
        this.pnlHeader = new System.Windows.Forms.Panel();
        this.checkBox10 = new System.Windows.Forms.CheckBox();
        this.linkLabel1 = new System.Windows.Forms.LinkLabel();
        this.checkBox9 = new System.Windows.Forms.CheckBox();
        this.checkBox8 = new System.Windows.Forms.CheckBox();
        this.checkBox7 = new System.Windows.Forms.CheckBox();
        this.checkBox5 = new System.Windows.Forms.CheckBox();
        this.checkBox4 = new System.Windows.Forms.CheckBox();
        this.bunifuCustomLabel2 = new Bunifu.Framework.UI.BunifuCustomLabel();
        this.bunifuCustomLabel5 = new Bunifu.Framework.UI.BunifuCustomLabel();
        this.checkBox6 = new System.Windows.Forms.CheckBox();
        this.bunifuCustomLabel6 = new Bunifu.Framework.UI.BunifuCustomLabel();
        this.checkBox3 = new System.Windows.Forms.CheckBox();
        this.panel4 = new System.Windows.Forms.Panel();
        this.panel3 = new System.Windows.Forms.Panel();
        this.bunifuFlatButton5 = new Bunifu.Framework.UI.BunifuFlatButton();
        this.panel2 = new System.Windows.Forms.Panel();
        this.checkBox2 = new System.Windows.Forms.CheckBox();
        this.checkBox1 = new System.Windows.Forms.CheckBox();
        this.bunifuFlatButton8 = new Bunifu.Framework.UI.BunifuFlatButton();
        this.bunifuFlatButton1 = new Bunifu.Framework.UI.BunifuFlatButton();
        this.chkADV = new System.Windows.Forms.CheckBox();
        this.menuStrip1 = new System.Windows.Forms.MenuStrip();
        this.B4AAA = new System.Windows.Forms.ToolStripMenuItem();
        this.autoFillCaptchaByAPIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.AutoFillCaptchaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.AutoSubmitCaptchaAfterFill = new System.Windows.Forms.ToolStripMenuItem();
        this.showPassengerCaptchaWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.C4AA0 = new System.Windows.Forms.ToolStripMenuItem();
        this.aboutUSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.googleOCRToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.showHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.extractReCaptchaExtensionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.clearCacheToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.B6AAA = new System.Windows.Forms.ToolStripMenuItem();
        this.C6AA0 = new System.Windows.Forms.ToolStripMenuItem();
        this.uPIPayByOtherIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripSpeedUP = new System.Windows.Forms.ToolStripMenuItem();
        this.enableStrongPasswordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.autoChangeDestinationStationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.autoChangeSourceStationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.reInstallChromeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.solveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.fixIPBlockManuallyClickSIGNINButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        this.bunifuFlatButton2 = new Bunifu.Framework.UI.BunifuFlatButton();
        this.bunifuFlatButton7 = new Bunifu.Framework.UI.BunifuFlatButton();
        this.bunifuFlatButton6 = new Bunifu.Framework.UI.BunifuFlatButton();
        this.bunifuFlatButton4 = new Bunifu.Framework.UI.BunifuFlatButton();
        this.bunifuFlatButton3 = new Bunifu.Framework.UI.BunifuFlatButton();
        this.bunifuCustomLabel3 = new Bunifu.Framework.UI.BunifuCustomLabel();
        this.bunifuCustomLabel4 = new Bunifu.Framework.UI.BunifuCustomLabel();
        this.panel1 = new System.Windows.Forms.Panel();
        this.lbl_Date = new Bunifu.Framework.UI.BunifuCustomLabel();
        this.label50 = new System.Windows.Forms.Label();
        this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
        this.bunifuDragControl_0 = new Bunifu.Framework.UI.BunifuDragControl(this.components);
        this.imageList_0 = new System.Windows.Forms.ImageList(this.components);
        this.bunifuTransition_1 = new BunifuAnimatorNS.BunifuTransition(this.components);
        this.timer_0 = new System.Windows.Forms.Timer(this.components);
        this.pnlHeader.SuspendLayout();
        this.menuStrip1.SuspendLayout();
        this.panel1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
        this.SuspendLayout();
        // 
        // lblUsrMsg
        // 
        this.lblUsrMsg.AutoSize = true;
        this.lblUsrMsg.BackColor = System.Drawing.Color.Transparent;
        this.lblUsrMsg.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblUsrMsg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        this.lblUsrMsg.Location = new System.Drawing.Point(183, 714);
        this.lblUsrMsg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        this.lblUsrMsg.Name = "lblUsrMsg";
        this.lblUsrMsg.Size = new System.Drawing.Size(380, 16);
        this.lblUsrMsg.TabIndex = 1173;
        this.lblUsrMsg.Text = "Dear Users More Features And Updates Coming Soon.........";
        // 
        // bunifuElipse_0
        // 
        this.bunifuElipse_0.ElipseRadius = 5;
        this.bunifuElipse_0.TargetControl = this;
        // 
        // bunifuTransition_0
        // 
        this.bunifuTransition_0.AnimationType = BunifuAnimatorNS.AnimationType.VertSlide;
        this.bunifuTransition_0.Cursor = null;
        animation3.AnimateOnlyDifferences = true;
        animation3.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation3.BlindCoeff")));
        animation3.LeafCoeff = 0F;
        animation3.MaxTime = 1F;
        animation3.MinTime = 0F;
        animation3.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation3.MosaicCoeff")));
        animation3.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation3.MosaicShift")));
        animation3.MosaicSize = 0;
        animation3.Padding = new System.Windows.Forms.Padding(0);
        animation3.RotateCoeff = 0F;
        animation3.RotateLimit = 0F;
        animation3.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation3.ScaleCoeff")));
        animation3.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation3.SlideCoeff")));
        animation3.TimeCoeff = 0F;
        animation3.TransparencyCoeff = 0F;
        this.bunifuTransition_0.DefaultAnimation = animation3;
        // 
        // C3AA0
        // 
        this.C3AA0.BackColor = System.Drawing.Color.Transparent;
        this.bunifuTransition_0.SetDecoration(this.C3AA0, BunifuAnimatorNS.DecorationType.None);
        this.C3AA0.FlatAppearance.BorderSize = 0;
        this.C3AA0.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
        this.C3AA0.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
        this.C3AA0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.C3AA0.ForeColor = System.Drawing.Color.Black;
        this.C3AA0.Location = new System.Drawing.Point(195, 650);
        this.C3AA0.Name = "C3AA0";
        this.C3AA0.Size = new System.Drawing.Size(11, 17);
        this.C3AA0.TabIndex = 161;
        this.C3AA0.Text = "F10";
        this.C3AA0.UseVisualStyleBackColor = false;
        // 
        // pnlHeader
        // 
        this.pnlHeader.BackColor = System.Drawing.Color.Snow;
        this.pnlHeader.Controls.Add(this.checkBox10);
        this.pnlHeader.Controls.Add(this.linkLabel1);
        this.pnlHeader.Controls.Add(this.checkBox9);
        this.pnlHeader.Controls.Add(this.checkBox8);
        this.pnlHeader.Controls.Add(this.checkBox7);
        this.pnlHeader.Controls.Add(this.checkBox5);
        this.pnlHeader.Controls.Add(this.checkBox4);
        this.pnlHeader.Controls.Add(this.bunifuCustomLabel2);
        this.pnlHeader.Controls.Add(this.bunifuCustomLabel5);
        this.pnlHeader.Controls.Add(this.checkBox6);
        this.pnlHeader.Controls.Add(this.bunifuCustomLabel6);
        this.pnlHeader.Controls.Add(this.checkBox3);
        this.pnlHeader.Controls.Add(this.panel4);
        this.pnlHeader.Controls.Add(this.panel3);
        this.pnlHeader.Controls.Add(this.bunifuFlatButton5);
        this.pnlHeader.Controls.Add(this.panel2);
        this.pnlHeader.Controls.Add(this.checkBox2);
        this.pnlHeader.Controls.Add(this.checkBox1);
        this.pnlHeader.Controls.Add(this.bunifuFlatButton8);
        this.pnlHeader.Controls.Add(this.bunifuFlatButton1);
        this.pnlHeader.Controls.Add(this.chkADV);
        this.pnlHeader.Controls.Add(this.menuStrip1);
        this.pnlHeader.Controls.Add(this.bunifuFlatButton2);
        this.pnlHeader.Controls.Add(this.bunifuFlatButton7);
        this.pnlHeader.Controls.Add(this.bunifuFlatButton6);
        this.pnlHeader.Controls.Add(this.bunifuFlatButton4);
        this.pnlHeader.Controls.Add(this.bunifuFlatButton3);
        this.bunifuTransition_0.SetDecoration(this.pnlHeader, BunifuAnimatorNS.DecorationType.None);
        this.pnlHeader.Location = new System.Drawing.Point(-1, 24);
        this.pnlHeader.Margin = new System.Windows.Forms.Padding(2);
        this.pnlHeader.Name = "pnlHeader";
        this.pnlHeader.Size = new System.Drawing.Size(795, 199);
        this.pnlHeader.TabIndex = 1207;
        this.pnlHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeader_Paint);
        // 
        // checkBox10
        // 
        this.checkBox10.AutoSize = true;
        this.bunifuTransition_0.SetDecoration(this.checkBox10, BunifuAnimatorNS.DecorationType.None);
        this.checkBox10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.checkBox10.ForeColor = System.Drawing.Color.Black;
        this.checkBox10.Location = new System.Drawing.Point(279, 87);
        this.checkBox10.Name = "checkBox10";
        this.checkBox10.Size = new System.Drawing.Size(111, 19);
        this.checkBox10.TabIndex = 1260;
        this.checkBox10.Text = "Stop IP Block";
        this.checkBox10.UseVisualStyleBackColor = true;
        this.checkBox10.CheckedChanged += new System.EventHandler(this.checkBox10_CheckedChanged_1);
        // 
        // linkLabel1
        // 
        this.linkLabel1.AutoSize = true;
        this.bunifuTransition_0.SetDecoration(this.linkLabel1, BunifuAnimatorNS.DecorationType.None);
        this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.linkLabel1.Location = new System.Drawing.Point(515, 59);
        this.linkLabel1.Name = "linkLabel1";
        this.linkLabel1.Size = new System.Drawing.Size(57, 20);
        this.linkLabel1.TabIndex = 1259;
        this.linkLabel1.TabStop = true;
        this.linkLabel1.Text = "Reset";
        this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
        // 
        // checkBox9
        // 
        this.checkBox9.AutoSize = true;
        this.bunifuTransition_0.SetDecoration(this.checkBox9, BunifuAnimatorNS.DecorationType.None);
        this.checkBox9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.checkBox9.ForeColor = System.Drawing.Color.Black;
        this.checkBox9.Location = new System.Drawing.Point(452, 61);
        this.checkBox9.Name = "checkBox9";
        this.checkBox9.Size = new System.Drawing.Size(107, 19);
        this.checkBox9.TabIndex = 1258;
        this.checkBox9.Text = "Beta Version";
        this.checkBox9.UseVisualStyleBackColor = true;
        this.checkBox9.CheckedChanged += new System.EventHandler(this.checkBox9_CheckedChanged);
        // 
        // checkBox8
        // 
        this.checkBox8.AutoSize = true;
        this.bunifuTransition_0.SetDecoration(this.checkBox8, BunifuAnimatorNS.DecorationType.None);
        this.checkBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.checkBox8.ForeColor = System.Drawing.Color.Black;
        this.checkBox8.Location = new System.Drawing.Point(169, 87);
        this.checkBox8.Name = "checkBox8";
        this.checkBox8.Size = new System.Drawing.Size(104, 19);
        this.checkBox8.TabIndex = 1257;
        this.checkBox8.Text = "Direct Login";
        this.checkBox8.UseVisualStyleBackColor = true;
        this.checkBox8.CheckedChanged += new System.EventHandler(this.checkBox8_CheckedChanged);
        // 
        // checkBox7
        // 
        this.checkBox7.AutoSize = true;
        this.bunifuTransition_0.SetDecoration(this.checkBox7, BunifuAnimatorNS.DecorationType.None);
        this.checkBox7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.checkBox7.ForeColor = System.Drawing.Color.Black;
        this.checkBox7.Location = new System.Drawing.Point(376, 60);
        this.checkBox7.Name = "checkBox7";
        this.checkBox7.Size = new System.Drawing.Size(65, 19);
        this.checkBox7.TabIndex = 1216;
        this.checkBox7.Text = "APP-2";
        this.checkBox7.UseVisualStyleBackColor = true;
        this.checkBox7.CheckedChanged += new System.EventHandler(this.checkBox7_CheckedChanged);
        // 
        // checkBox5
        // 
        this.checkBox5.AutoSize = true;
        this.bunifuTransition_0.SetDecoration(this.checkBox5, BunifuAnimatorNS.DecorationType.None);
        this.checkBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.checkBox5.ForeColor = System.Drawing.Color.Black;
        this.checkBox5.Location = new System.Drawing.Point(203, 60);
        this.checkBox5.Name = "checkBox5";
        this.checkBox5.Size = new System.Drawing.Size(131, 19);
        this.checkBox5.TabIndex = 1256;
        this.checkBox5.Text = "Booking Mode-3";
        this.checkBox5.UseVisualStyleBackColor = true;
        this.checkBox5.CheckedChanged += new System.EventHandler(this.checkBox5_CheckedChanged);
        // 
        // checkBox4
        // 
        this.checkBox4.AutoSize = true;
        this.bunifuTransition_0.SetDecoration(this.checkBox4, BunifuAnimatorNS.DecorationType.None);
        this.checkBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.checkBox4.ForeColor = System.Drawing.Color.Black;
        this.checkBox4.Location = new System.Drawing.Point(3, 86);
        this.checkBox4.Name = "checkBox4";
        this.checkBox4.Size = new System.Drawing.Size(76, 19);
        this.checkBox4.TabIndex = 1255;
        this.checkBox4.Text = "Alt Avail";
        this.checkBox4.UseVisualStyleBackColor = true;
        this.checkBox4.CheckedChanged += new System.EventHandler(this.checkBox4_CheckedChanged);
        // 
        // bunifuCustomLabel2
        // 
        this.bunifuCustomLabel2.Anchor = System.Windows.Forms.AnchorStyles.Right;
        this.bunifuCustomLabel2.AutoSize = true;
        this.bunifuCustomLabel2.BackColor = System.Drawing.Color.Transparent;
        this.bunifuTransition_0.SetDecoration(this.bunifuCustomLabel2, BunifuAnimatorNS.DecorationType.None);
        this.bunifuCustomLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
        this.bunifuCustomLabel2.ForeColor = System.Drawing.Color.Red;
        this.bunifuCustomLabel2.Location = new System.Drawing.Point(748, 126);
        this.bunifuCustomLabel2.Name = "bunifuCustomLabel2";
        this.bunifuCustomLabel2.Size = new System.Drawing.Size(40, 24);
        this.bunifuCustomLabel2.TabIndex = 1217;
        this.bunifuCustomLabel2.Text = "310";
        this.bunifuCustomLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // bunifuCustomLabel5
        // 
        this.bunifuCustomLabel5.Anchor = System.Windows.Forms.AnchorStyles.Right;
        this.bunifuCustomLabel5.AutoSize = true;
        this.bunifuCustomLabel5.BackColor = System.Drawing.Color.Transparent;
        this.bunifuTransition_0.SetDecoration(this.bunifuCustomLabel5, BunifuAnimatorNS.DecorationType.None);
        this.bunifuCustomLabel5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.bunifuCustomLabel5.ForeColor = System.Drawing.Color.Red;
        this.bunifuCustomLabel5.Location = new System.Drawing.Point(657, 126);
        this.bunifuCustomLabel5.Name = "bunifuCustomLabel5";
        this.bunifuCustomLabel5.Size = new System.Drawing.Size(80, 24);
        this.bunifuCustomLabel5.TabIndex = 1218;
        this.bunifuCustomLabel5.Text = "XXXXX";
        this.bunifuCustomLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // checkBox6
        // 
        this.checkBox6.AutoSize = true;
        this.bunifuTransition_0.SetDecoration(this.checkBox6, BunifuAnimatorNS.DecorationType.None);
        this.checkBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.checkBox6.ForeColor = System.Drawing.Color.Black;
        this.checkBox6.Location = new System.Drawing.Point(310, 60);
        this.checkBox6.Name = "checkBox6";
        this.checkBox6.Size = new System.Drawing.Size(65, 19);
        this.checkBox6.TabIndex = 1215;
        this.checkBox6.Text = "APP-1";
        this.checkBox6.UseVisualStyleBackColor = true;
        this.checkBox6.CheckedChanged += new System.EventHandler(this.checkBox6_CheckedChanged);
        // 
        // bunifuCustomLabel6
        // 
        this.bunifuCustomLabel6.Anchor = System.Windows.Forms.AnchorStyles.Right;
        this.bunifuCustomLabel6.AutoSize = true;
        this.bunifuCustomLabel6.BackColor = System.Drawing.Color.Transparent;
        this.bunifuTransition_0.SetDecoration(this.bunifuCustomLabel6, BunifuAnimatorNS.DecorationType.None);
        this.bunifuCustomLabel6.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
        this.bunifuCustomLabel6.ForeColor = System.Drawing.Color.Navy;
        this.bunifuCustomLabel6.Location = new System.Drawing.Point(609, 126);
        this.bunifuCustomLabel6.Name = "bunifuCustomLabel6";
        this.bunifuCustomLabel6.Size = new System.Drawing.Size(52, 24);
        this.bunifuCustomLabel6.TabIndex = 1251;
        this.bunifuCustomLabel6.Text = "Key :";
        this.bunifuCustomLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // checkBox3
        // 
        this.checkBox3.AutoSize = true;
        this.bunifuTransition_0.SetDecoration(this.checkBox3, BunifuAnimatorNS.DecorationType.None);
        this.checkBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.checkBox3.ForeColor = System.Drawing.Color.Black;
        this.checkBox3.Location = new System.Drawing.Point(104, 60);
        this.checkBox3.Name = "checkBox3";
        this.checkBox3.Size = new System.Drawing.Size(131, 19);
        this.checkBox3.TabIndex = 1254;
        this.checkBox3.Text = "Booking Mode-2";
        this.checkBox3.UseVisualStyleBackColor = true;
        this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
        // 
        // panel4
        // 
        this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        this.bunifuTransition_0.SetDecoration(this.panel4, BunifuAnimatorNS.DecorationType.None);
        this.panel4.Location = new System.Drawing.Point(-12, 54);
        this.panel4.Name = "panel4";
        this.panel4.Size = new System.Drawing.Size(590, 2);
        this.panel4.TabIndex = 1253;
        // 
        // panel3
        // 
        this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        this.bunifuTransition_0.SetDecoration(this.panel3, BunifuAnimatorNS.DecorationType.None);
        this.panel3.Location = new System.Drawing.Point(-13, 81);
        this.panel3.Name = "panel3";
        this.panel3.Size = new System.Drawing.Size(590, 2);
        this.panel3.TabIndex = 1250;
        // 
        // bunifuFlatButton5
        // 
        this.bunifuFlatButton5.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.bunifuFlatButton5.BorderRadius = 0;
        this.bunifuFlatButton5.ButtonText = "Ip Block Check && Token";
        this.bunifuFlatButton5.Cursor = System.Windows.Forms.Cursors.Hand;
        this.bunifuTransition_0.SetDecoration(this.bunifuFlatButton5, BunifuAnimatorNS.DecorationType.None);
        this.bunifuFlatButton5.DisabledColor = System.Drawing.Color.Gray;
        this.bunifuFlatButton5.Iconcolor = System.Drawing.Color.Transparent;
        this.bunifuFlatButton5.Iconimage = null;
        this.bunifuFlatButton5.Iconimage_right = null;
        this.bunifuFlatButton5.Iconimage_right_Selected = null;
        this.bunifuFlatButton5.Iconimage_Selected = null;
        this.bunifuFlatButton5.IconMarginLeft = 0;
        this.bunifuFlatButton5.IconMarginRight = 0;
        this.bunifuFlatButton5.IconRightVisible = true;
        this.bunifuFlatButton5.IconRightZoom = 0D;
        this.bunifuFlatButton5.IconVisible = true;
        this.bunifuFlatButton5.IconZoom = 90D;
        this.bunifuFlatButton5.IsTab = false;
        this.bunifuFlatButton5.Location = new System.Drawing.Point(431, 29);
        this.bunifuFlatButton5.Margin = new System.Windows.Forms.Padding(5);
        this.bunifuFlatButton5.Name = "bunifuFlatButton5";
        this.bunifuFlatButton5.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton5.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
        this.bunifuFlatButton5.OnHoverTextColor = System.Drawing.Color.White;
        this.bunifuFlatButton5.selected = false;
        this.bunifuFlatButton5.Size = new System.Drawing.Size(145, 23);
        this.bunifuFlatButton5.TabIndex = 1206;
        this.bunifuFlatButton5.Text = "Ip Block Check && Token";
        this.bunifuFlatButton5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.bunifuFlatButton5.Textcolor = System.Drawing.Color.White;
        this.bunifuFlatButton5.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.bunifuFlatButton5.Click += new System.EventHandler(this.bunifuFlatButton7_Click);
        // 
        // panel2
        // 
        this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        this.bunifuTransition_0.SetDecoration(this.panel2, BunifuAnimatorNS.DecorationType.None);
        this.panel2.Location = new System.Drawing.Point(-21, 111);
        this.panel2.Name = "panel2";
        this.panel2.Size = new System.Drawing.Size(610, 2);
        this.panel2.TabIndex = 1249;
        // 
        // checkBox2
        // 
        this.checkBox2.AutoSize = true;
        this.bunifuTransition_0.SetDecoration(this.checkBox2, BunifuAnimatorNS.DecorationType.None);
        this.checkBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.checkBox2.ForeColor = System.Drawing.Color.Black;
        this.checkBox2.Location = new System.Drawing.Point(113, 137);
        this.checkBox2.Name = "checkBox2";
        this.checkBox2.Size = new System.Drawing.Size(94, 19);
        this.checkBox2.TabIndex = 1215;
        this.checkBox2.Text = "Auto Close";
        this.checkBox2.UseVisualStyleBackColor = true;
        this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
        // 
        // checkBox1
        // 
        this.checkBox1.AutoSize = true;
        this.bunifuTransition_0.SetDecoration(this.checkBox1, BunifuAnimatorNS.DecorationType.None);
        this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.checkBox1.ForeColor = System.Drawing.Color.Black;
        this.checkBox1.Location = new System.Drawing.Point(86, 86);
        this.checkBox1.Name = "checkBox1";
        this.checkBox1.Size = new System.Drawing.Size(102, 19);
        this.checkBox1.TabIndex = 1214;
        this.checkBox1.Text = "Vedio Mode";
        this.checkBox1.UseVisualStyleBackColor = true;
        this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
        // 
        // bunifuFlatButton8
        // 
        this.bunifuFlatButton8.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.bunifuFlatButton8.BorderRadius = 0;
        this.bunifuFlatButton8.ButtonText = "BackUp && Restore";
        this.bunifuFlatButton8.Cursor = System.Windows.Forms.Cursors.Hand;
        this.bunifuTransition_0.SetDecoration(this.bunifuFlatButton8, BunifuAnimatorNS.DecorationType.None);
        this.bunifuFlatButton8.DisabledColor = System.Drawing.Color.Gray;
        this.bunifuFlatButton8.Iconcolor = System.Drawing.Color.Transparent;
        this.bunifuFlatButton8.Iconimage = null;
        this.bunifuFlatButton8.Iconimage_right = null;
        this.bunifuFlatButton8.Iconimage_right_Selected = null;
        this.bunifuFlatButton8.Iconimage_Selected = null;
        this.bunifuFlatButton8.IconMarginLeft = 0;
        this.bunifuFlatButton8.IconMarginRight = 0;
        this.bunifuFlatButton8.IconRightVisible = true;
        this.bunifuFlatButton8.IconRightZoom = 0D;
        this.bunifuFlatButton8.IconVisible = true;
        this.bunifuFlatButton8.IconZoom = 90D;
        this.bunifuFlatButton8.IsTab = false;
        this.bunifuFlatButton8.Location = new System.Drawing.Point(289, 29);
        this.bunifuFlatButton8.Margin = new System.Windows.Forms.Padding(5);
        this.bunifuFlatButton8.Name = "bunifuFlatButton8";
        this.bunifuFlatButton8.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton8.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
        this.bunifuFlatButton8.OnHoverTextColor = System.Drawing.Color.White;
        this.bunifuFlatButton8.selected = false;
        this.bunifuFlatButton8.Size = new System.Drawing.Size(140, 23);
        this.bunifuFlatButton8.TabIndex = 1213;
        this.bunifuFlatButton8.Text = "BackUp && Restore";
        this.bunifuFlatButton8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.bunifuFlatButton8.Textcolor = System.Drawing.Color.White;
        this.bunifuFlatButton8.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.bunifuFlatButton8.Click += new System.EventHandler(this.bunifuFlatButton8_Click_1);
        // 
        // bunifuFlatButton1
        // 
        this.bunifuFlatButton1.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.bunifuFlatButton1.BorderRadius = 0;
        this.bunifuFlatButton1.ButtonText = "Add Bank";
        this.bunifuFlatButton1.Cursor = System.Windows.Forms.Cursors.Hand;
        this.bunifuTransition_0.SetDecoration(this.bunifuFlatButton1, BunifuAnimatorNS.DecorationType.None);
        this.bunifuFlatButton1.DisabledColor = System.Drawing.Color.Gray;
        this.bunifuFlatButton1.Iconcolor = System.Drawing.Color.Transparent;
        this.bunifuFlatButton1.Iconimage = null;
        this.bunifuFlatButton1.Iconimage_right = null;
        this.bunifuFlatButton1.Iconimage_right_Selected = null;
        this.bunifuFlatButton1.Iconimage_Selected = null;
        this.bunifuFlatButton1.IconMarginLeft = 0;
        this.bunifuFlatButton1.IconMarginRight = 0;
        this.bunifuFlatButton1.IconRightVisible = true;
        this.bunifuFlatButton1.IconRightZoom = 0D;
        this.bunifuFlatButton1.IconVisible = true;
        this.bunifuFlatButton1.IconZoom = 90D;
        this.bunifuFlatButton1.IsTab = false;
        this.bunifuFlatButton1.Location = new System.Drawing.Point(3, 29);
        this.bunifuFlatButton1.Margin = new System.Windows.Forms.Padding(5);
        this.bunifuFlatButton1.Name = "bunifuFlatButton1";
        this.bunifuFlatButton1.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton1.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
        this.bunifuFlatButton1.OnHoverTextColor = System.Drawing.Color.White;
        this.bunifuFlatButton1.selected = false;
        this.bunifuFlatButton1.Size = new System.Drawing.Size(140, 23);
        this.bunifuFlatButton1.TabIndex = 1211;
        this.bunifuFlatButton1.Text = "Add Bank";
        this.bunifuFlatButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.bunifuFlatButton1.Textcolor = System.Drawing.Color.White;
        this.bunifuFlatButton1.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.bunifuFlatButton1.Click += new System.EventHandler(this.bunifuFlatButton1_Click_1);
        // 
        // chkADV
        // 
        this.chkADV.AutoSize = true;
        this.bunifuTransition_0.SetDecoration(this.chkADV, BunifuAnimatorNS.DecorationType.None);
        this.chkADV.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.chkADV.ForeColor = System.Drawing.Color.Black;
        this.chkADV.Location = new System.Drawing.Point(3, 60);
        this.chkADV.Name = "chkADV";
        this.chkADV.Size = new System.Drawing.Size(131, 19);
        this.chkADV.TabIndex = 1210;
        this.chkADV.Text = "Booking Mode-1";
        this.chkADV.UseVisualStyleBackColor = true;
        this.chkADV.CheckedChanged += new System.EventHandler(this.chkADV_CheckedChanged);
        // 
        // menuStrip1
        // 
        this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
        this.bunifuTransition_0.SetDecoration(this.menuStrip1, BunifuAnimatorNS.DecorationType.None);
        this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
        this.menuStrip1.Font = new System.Drawing.Font("Cambria", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
        this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.B4AAA});
        this.menuStrip1.Location = new System.Drawing.Point(378, 249);
        this.menuStrip1.Name = "menuStrip1";
        this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
        this.menuStrip1.Size = new System.Drawing.Size(85, 24);
        this.menuStrip1.TabIndex = 166;
        this.menuStrip1.Text = "menuStrip1";
        // 
        // B4AAA
        // 
        this.B4AAA.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoFillCaptchaByAPIToolStripMenuItem,
            this.AutoFillCaptchaToolStripMenuItem,
            this.AutoSubmitCaptchaAfterFill,
            this.showPassengerCaptchaWindowToolStripMenuItem,
            this.C4AA0,
            this.aboutUSToolStripMenuItem,
            this.googleOCRToolStripMenuItem,
            this.showHistoryToolStripMenuItem,
            this.extractReCaptchaExtensionToolStripMenuItem,
            this.clearCacheToolStripMenuItem,
            this.B6AAA,
            this.C6AA0,
            this.uPIPayByOtherIDToolStripMenuItem,
            this.toolStripSpeedUP,
            this.enableStrongPasswordToolStripMenuItem,
            this.autoChangeDestinationStationToolStripMenuItem,
            this.autoChangeSourceStationToolStripMenuItem,
            this.reInstallChromeToolStripMenuItem,
            this.solveToolStripMenuItem,
            this.fixIPBlockManuallyClickSIGNINButtonToolStripMenuItem});
        this.B4AAA.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.B4AAA.Name = "B4AAA";
        this.B4AAA.Size = new System.Drawing.Size(79, 20);
        this.B4AAA.Text = "Option\'s";
        // 
        // autoFillCaptchaByAPIToolStripMenuItem
        // 
        this.autoFillCaptchaByAPIToolStripMenuItem.CheckOnClick = true;
        this.autoFillCaptchaByAPIToolStripMenuItem.Name = "autoFillCaptchaByAPIToolStripMenuItem";
        this.autoFillCaptchaByAPIToolStripMenuItem.Size = new System.Drawing.Size(407, 22);
        this.autoFillCaptchaByAPIToolStripMenuItem.Text = "Auto Fill Captcha by API";
        this.autoFillCaptchaByAPIToolStripMenuItem.Click += new System.EventHandler(this.autoFillCaptchaByAPIToolStripMenuItem_Click);
        // 
        // AutoFillCaptchaToolStripMenuItem
        // 
        this.AutoFillCaptchaToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
        this.AutoFillCaptchaToolStripMenuItem.CheckOnClick = true;
        this.AutoFillCaptchaToolStripMenuItem.Name = "AutoFillCaptchaToolStripMenuItem";
        this.AutoFillCaptchaToolStripMenuItem.Size = new System.Drawing.Size(407, 22);
        this.AutoFillCaptchaToolStripMenuItem.Text = "Auto Fill Captcha by OCR";
        this.AutoFillCaptchaToolStripMenuItem.Click += new System.EventHandler(this.AutoFillCaptchaToolStripMenuItem_Click);
        // 
        // AutoSubmitCaptchaAfterFill
        // 
        this.AutoSubmitCaptchaAfterFill.CheckOnClick = true;
        this.AutoSubmitCaptchaAfterFill.Name = "AutoSubmitCaptchaAfterFill";
        this.AutoSubmitCaptchaAfterFill.Size = new System.Drawing.Size(407, 22);
        this.AutoSubmitCaptchaAfterFill.Text = "AutoSubmit Captcha After Fill";
        this.AutoSubmitCaptchaAfterFill.Click += new System.EventHandler(this.AutoSubmitCaptchaAfterFill_Click);
        // 
        // showPassengerCaptchaWindowToolStripMenuItem
        // 
        this.showPassengerCaptchaWindowToolStripMenuItem.CheckOnClick = true;
        this.showPassengerCaptchaWindowToolStripMenuItem.Name = "showPassengerCaptchaWindowToolStripMenuItem";
        this.showPassengerCaptchaWindowToolStripMenuItem.Size = new System.Drawing.Size(407, 22);
        this.showPassengerCaptchaWindowToolStripMenuItem.Text = "Show Passenger Captcha Window";
        this.showPassengerCaptchaWindowToolStripMenuItem.Click += new System.EventHandler(this.showPassengerCaptchaWindowToolStripMenuItem_Click);
        // 
        // C4AA0
        // 
        this.C4AA0.Name = "C4AA0";
        this.C4AA0.Size = new System.Drawing.Size(407, 22);
        this.C4AA0.Text = "Show NEWS";
        this.C4AA0.Click += new System.EventHandler(this.C0AA0);
        // 
        // aboutUSToolStripMenuItem
        // 
        this.aboutUSToolStripMenuItem.Name = "aboutUSToolStripMenuItem";
        this.aboutUSToolStripMenuItem.Size = new System.Drawing.Size(407, 22);
        this.aboutUSToolStripMenuItem.Text = "Key Info";
        this.aboutUSToolStripMenuItem.Click += new System.EventHandler(this.B2AAA);
        // 
        // googleOCRToolStripMenuItem
        // 
        this.googleOCRToolStripMenuItem.CheckOnClick = true;
        this.googleOCRToolStripMenuItem.Name = "googleOCRToolStripMenuItem";
        this.googleOCRToolStripMenuItem.Size = new System.Drawing.Size(407, 22);
        this.googleOCRToolStripMenuItem.Text = "Google OCR";
        this.googleOCRToolStripMenuItem.Visible = false;
        this.googleOCRToolStripMenuItem.CheckedChanged += new System.EventHandler(this.googleOCRToolStripMenuItem_CheckedChanged);
        this.googleOCRToolStripMenuItem.Click += new System.EventHandler(this.googleOCRToolStripMenuItem_Click);
        // 
        // showHistoryToolStripMenuItem
        // 
        this.showHistoryToolStripMenuItem.Name = "showHistoryToolStripMenuItem";
        this.showHistoryToolStripMenuItem.Size = new System.Drawing.Size(407, 22);
        this.showHistoryToolStripMenuItem.Text = "Show History";
        this.showHistoryToolStripMenuItem.Click += new System.EventHandler(this.showHistoryToolStripMenuItem_Click);
        // 
        // extractReCaptchaExtensionToolStripMenuItem
        // 
        this.extractReCaptchaExtensionToolStripMenuItem.Name = "extractReCaptchaExtensionToolStripMenuItem";
        this.extractReCaptchaExtensionToolStripMenuItem.Size = new System.Drawing.Size(407, 22);
        this.extractReCaptchaExtensionToolStripMenuItem.Text = "Extract reCaptcha Extension";
        this.extractReCaptchaExtensionToolStripMenuItem.Click += new System.EventHandler(this.C2AA0);
        // 
        // clearCacheToolStripMenuItem
        // 
        this.clearCacheToolStripMenuItem.Name = "clearCacheToolStripMenuItem";
        this.clearCacheToolStripMenuItem.Size = new System.Drawing.Size(407, 22);
        this.clearCacheToolStripMenuItem.Text = "Clear Cache";
        this.clearCacheToolStripMenuItem.Click += new System.EventHandler(this.clearCacheToolStripMenuItem_Click);
        // 
        // B6AAA
        // 
        this.B6AAA.Name = "B6AAA";
        this.B6AAA.Size = new System.Drawing.Size(407, 22);
        this.B6AAA.Text = "Check reCaptcha Connection";
        this.B6AAA.Visible = false;
        this.B6AAA.Click += new System.EventHandler(this.B6AAA_Click);
        // 
        // C6AA0
        // 
        this.C6AA0.Name = "C6AA0";
        this.C6AA0.Size = new System.Drawing.Size(407, 22);
        this.C6AA0.Text = "Check reCaptcha Connection Chrome Beta";
        this.C6AA0.Visible = false;
        this.C6AA0.Click += new System.EventHandler(this.C6AA0_Click);
        // 
        // uPIPayByOtherIDToolStripMenuItem
        // 
        this.uPIPayByOtherIDToolStripMenuItem.CheckOnClick = true;
        this.uPIPayByOtherIDToolStripMenuItem.Name = "uPIPayByOtherIDToolStripMenuItem";
        this.uPIPayByOtherIDToolStripMenuItem.Size = new System.Drawing.Size(407, 22);
        this.uPIPayByOtherIDToolStripMenuItem.Text = "BHIMUPI Direct Pay";
        this.uPIPayByOtherIDToolStripMenuItem.Visible = false;
        this.uPIPayByOtherIDToolStripMenuItem.Click += new System.EventHandler(this.B1AAA);
        // 
        // toolStripSpeedUP
        // 
        this.toolStripSpeedUP.CheckOnClick = true;
        this.toolStripSpeedUP.Name = "toolStripSpeedUP";
        this.toolStripSpeedUP.Size = new System.Drawing.Size(407, 22);
        this.toolStripSpeedUP.Text = "Small Booking Window";
        this.toolStripSpeedUP.Click += new System.EventHandler(this.ToolStripSpeedUP_Click);
        // 
        // enableStrongPasswordToolStripMenuItem
        // 
        this.enableStrongPasswordToolStripMenuItem.Name = "enableStrongPasswordToolStripMenuItem";
        this.enableStrongPasswordToolStripMenuItem.Size = new System.Drawing.Size(407, 22);
        this.enableStrongPasswordToolStripMenuItem.Text = "Enable Strong Password";
        this.enableStrongPasswordToolStripMenuItem.Click += new System.EventHandler(this.enableStrongPasswordToolStripMenuItem_Click);
        // 
        // autoChangeDestinationStationToolStripMenuItem
        // 
        this.autoChangeDestinationStationToolStripMenuItem.CheckOnClick = true;
        this.autoChangeDestinationStationToolStripMenuItem.Name = "autoChangeDestinationStationToolStripMenuItem";
        this.autoChangeDestinationStationToolStripMenuItem.Size = new System.Drawing.Size(407, 22);
        this.autoChangeDestinationStationToolStripMenuItem.Text = "Auto Change Destination Station";
        this.autoChangeDestinationStationToolStripMenuItem.Click += new System.EventHandler(this.autoChangeDestinationStationToolStripMenuItem_Click);
        // 
        // autoChangeSourceStationToolStripMenuItem
        // 
        this.autoChangeSourceStationToolStripMenuItem.CheckOnClick = true;
        this.autoChangeSourceStationToolStripMenuItem.Name = "autoChangeSourceStationToolStripMenuItem";
        this.autoChangeSourceStationToolStripMenuItem.Size = new System.Drawing.Size(407, 22);
        this.autoChangeSourceStationToolStripMenuItem.Text = "Auto Change Source Station";
        this.autoChangeSourceStationToolStripMenuItem.Click += new System.EventHandler(this.autoChangeSourceStationToolStripMenuItem_Click);
        // 
        // reInstallChromeToolStripMenuItem
        // 
        this.reInstallChromeToolStripMenuItem.Name = "reInstallChromeToolStripMenuItem";
        this.reInstallChromeToolStripMenuItem.Size = new System.Drawing.Size(407, 22);
        this.reInstallChromeToolStripMenuItem.Text = "Re-Install Chrome";
        this.reInstallChromeToolStripMenuItem.Click += new System.EventHandler(this.reInstallChromeToolStripMenuItem_Click);
        // 
        // solveToolStripMenuItem
        // 
        this.solveToolStripMenuItem.CheckOnClick = true;
        this.solveToolStripMenuItem.Name = "solveToolStripMenuItem";
        this.solveToolStripMenuItem.Size = new System.Drawing.Size(407, 22);
        this.solveToolStripMenuItem.Text = "Solve IP Block on Payment";
        this.solveToolStripMenuItem.Click += new System.EventHandler(this.solveToolStripMenuItem_Click);
        // 
        // fixIPBlockManuallyClickSIGNINButtonToolStripMenuItem
        // 
        this.fixIPBlockManuallyClickSIGNINButtonToolStripMenuItem.CheckOnClick = true;
        this.fixIPBlockManuallyClickSIGNINButtonToolStripMenuItem.Name = "fixIPBlockManuallyClickSIGNINButtonToolStripMenuItem";
        this.fixIPBlockManuallyClickSIGNINButtonToolStripMenuItem.Size = new System.Drawing.Size(407, 22);
        this.fixIPBlockManuallyClickSIGNINButtonToolStripMenuItem.Text = "Fix IP Block ( Manually Click SIGN-IN Button )";
        this.fixIPBlockManuallyClickSIGNINButtonToolStripMenuItem.Click += new System.EventHandler(this.fixIPBlockManuallyClickSIGNINButtonToolStripMenuItem_Click);
        // 
        // bunifuFlatButton2
        // 
        this.bunifuFlatButton2.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.bunifuFlatButton2.BorderRadius = 0;
        this.bunifuFlatButton2.ButtonText = "Histry && Proxy";
        this.bunifuFlatButton2.Cursor = System.Windows.Forms.Cursors.Hand;
        this.bunifuTransition_0.SetDecoration(this.bunifuFlatButton2, BunifuAnimatorNS.DecorationType.None);
        this.bunifuFlatButton2.DisabledColor = System.Drawing.Color.Gray;
        this.bunifuFlatButton2.Iconcolor = System.Drawing.Color.Transparent;
        this.bunifuFlatButton2.Iconimage = null;
        this.bunifuFlatButton2.Iconimage_right = null;
        this.bunifuFlatButton2.Iconimage_right_Selected = null;
        this.bunifuFlatButton2.Iconimage_Selected = null;
        this.bunifuFlatButton2.IconMarginLeft = 0;
        this.bunifuFlatButton2.IconMarginRight = 0;
        this.bunifuFlatButton2.IconRightVisible = true;
        this.bunifuFlatButton2.IconRightZoom = 0D;
        this.bunifuFlatButton2.IconVisible = true;
        this.bunifuFlatButton2.IconZoom = 90D;
        this.bunifuFlatButton2.IsTab = false;
        this.bunifuFlatButton2.Location = new System.Drawing.Point(431, 3);
        this.bunifuFlatButton2.Margin = new System.Windows.Forms.Padding(5);
        this.bunifuFlatButton2.Name = "bunifuFlatButton2";
        this.bunifuFlatButton2.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton2.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
        this.bunifuFlatButton2.OnHoverTextColor = System.Drawing.Color.White;
        this.bunifuFlatButton2.selected = false;
        this.bunifuFlatButton2.Size = new System.Drawing.Size(145, 22);
        this.bunifuFlatButton2.TabIndex = 1209;
        this.bunifuFlatButton2.Text = "Histry && Proxy";
        this.bunifuFlatButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.bunifuFlatButton2.Textcolor = System.Drawing.Color.White;
        this.bunifuFlatButton2.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.bunifuFlatButton2.Click += new System.EventHandler(this.bunifuFlatButton1_Click);
        // 
        // bunifuFlatButton7
        // 
        this.bunifuFlatButton7.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.bunifuFlatButton7.BorderRadius = 0;
        this.bunifuFlatButton7.ButtonText = "Add IRCTC";
        this.bunifuFlatButton7.Cursor = System.Windows.Forms.Cursors.Hand;
        this.bunifuTransition_0.SetDecoration(this.bunifuFlatButton7, BunifuAnimatorNS.DecorationType.None);
        this.bunifuFlatButton7.DisabledColor = System.Drawing.Color.Gray;
        this.bunifuFlatButton7.Iconcolor = System.Drawing.Color.Transparent;
        this.bunifuFlatButton7.Iconimage = null;
        this.bunifuFlatButton7.Iconimage_right = null;
        this.bunifuFlatButton7.Iconimage_right_Selected = null;
        this.bunifuFlatButton7.Iconimage_Selected = null;
        this.bunifuFlatButton7.IconMarginLeft = 0;
        this.bunifuFlatButton7.IconMarginRight = 0;
        this.bunifuFlatButton7.IconRightVisible = true;
        this.bunifuFlatButton7.IconRightZoom = 0D;
        this.bunifuFlatButton7.IconVisible = true;
        this.bunifuFlatButton7.IconZoom = 90D;
        this.bunifuFlatButton7.IsTab = false;
        this.bunifuFlatButton7.Location = new System.Drawing.Point(3, 3);
        this.bunifuFlatButton7.Margin = new System.Windows.Forms.Padding(5);
        this.bunifuFlatButton7.Name = "bunifuFlatButton7";
        this.bunifuFlatButton7.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton7.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
        this.bunifuFlatButton7.OnHoverTextColor = System.Drawing.Color.White;
        this.bunifuFlatButton7.selected = false;
        this.bunifuFlatButton7.Size = new System.Drawing.Size(140, 23);
        this.bunifuFlatButton7.TabIndex = 1208;
        this.bunifuFlatButton7.Text = "Add IRCTC";
        this.bunifuFlatButton7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.bunifuFlatButton7.Textcolor = System.Drawing.Color.White;
        this.bunifuFlatButton7.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.bunifuFlatButton7.Click += new System.EventHandler(this.BunifuFlatButton2_Click);
        // 
        // bunifuFlatButton6
        // 
        this.bunifuFlatButton6.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.bunifuFlatButton6.BorderRadius = 0;
        this.bunifuFlatButton6.ButtonText = "   OTP-ByPASS";
        this.bunifuFlatButton6.Cursor = System.Windows.Forms.Cursors.Hand;
        this.bunifuTransition_0.SetDecoration(this.bunifuFlatButton6, BunifuAnimatorNS.DecorationType.None);
        this.bunifuFlatButton6.DisabledColor = System.Drawing.Color.Gray;
        this.bunifuFlatButton6.Iconcolor = System.Drawing.Color.Transparent;
        this.bunifuFlatButton6.Iconimage = null;
        this.bunifuFlatButton6.Iconimage_right = null;
        this.bunifuFlatButton6.Iconimage_right_Selected = null;
        this.bunifuFlatButton6.Iconimage_Selected = null;
        this.bunifuFlatButton6.IconMarginLeft = 0;
        this.bunifuFlatButton6.IconMarginRight = 0;
        this.bunifuFlatButton6.IconRightVisible = true;
        this.bunifuFlatButton6.IconRightZoom = 0D;
        this.bunifuFlatButton6.IconVisible = true;
        this.bunifuFlatButton6.IconZoom = 90D;
        this.bunifuFlatButton6.IsTab = false;
        this.bunifuFlatButton6.Location = new System.Drawing.Point(146, 30);
        this.bunifuFlatButton6.Margin = new System.Windows.Forms.Padding(5);
        this.bunifuFlatButton6.Name = "bunifuFlatButton6";
        this.bunifuFlatButton6.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton6.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
        this.bunifuFlatButton6.OnHoverTextColor = System.Drawing.Color.White;
        this.bunifuFlatButton6.selected = false;
        this.bunifuFlatButton6.Size = new System.Drawing.Size(140, 23);
        this.bunifuFlatButton6.TabIndex = 1207;
        this.bunifuFlatButton6.Text = "   OTP-ByPASS";
        this.bunifuFlatButton6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.bunifuFlatButton6.Textcolor = System.Drawing.Color.White;
        this.bunifuFlatButton6.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.bunifuFlatButton6.Click += new System.EventHandler(this.bunifuFlatButton8_Click);
        // 
        // bunifuFlatButton4
        // 
        this.bunifuFlatButton4.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.bunifuFlatButton4.BorderRadius = 0;
        this.bunifuFlatButton4.ButtonText = " OpenTicket";
        this.bunifuFlatButton4.Cursor = System.Windows.Forms.Cursors.Hand;
        this.bunifuTransition_0.SetDecoration(this.bunifuFlatButton4, BunifuAnimatorNS.DecorationType.None);
        this.bunifuFlatButton4.DisabledColor = System.Drawing.Color.Gray;
        this.bunifuFlatButton4.Iconcolor = System.Drawing.Color.Transparent;
        this.bunifuFlatButton4.Iconimage = null;
        this.bunifuFlatButton4.Iconimage_right = null;
        this.bunifuFlatButton4.Iconimage_right_Selected = null;
        this.bunifuFlatButton4.Iconimage_Selected = null;
        this.bunifuFlatButton4.IconMarginLeft = 0;
        this.bunifuFlatButton4.IconMarginRight = 0;
        this.bunifuFlatButton4.IconRightVisible = true;
        this.bunifuFlatButton4.IconRightZoom = 0D;
        this.bunifuFlatButton4.IconVisible = true;
        this.bunifuFlatButton4.IconZoom = 90D;
        this.bunifuFlatButton4.IsTab = false;
        this.bunifuFlatButton4.Location = new System.Drawing.Point(289, 3);
        this.bunifuFlatButton4.Margin = new System.Windows.Forms.Padding(5);
        this.bunifuFlatButton4.Name = "bunifuFlatButton4";
        this.bunifuFlatButton4.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton4.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
        this.bunifuFlatButton4.OnHoverTextColor = System.Drawing.Color.White;
        this.bunifuFlatButton4.selected = false;
        this.bunifuFlatButton4.Size = new System.Drawing.Size(140, 23);
        this.bunifuFlatButton4.TabIndex = 1205;
        this.bunifuFlatButton4.Text = " OpenTicket";
        this.bunifuFlatButton4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.bunifuFlatButton4.Textcolor = System.Drawing.Color.White;
        this.bunifuFlatButton4.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.bunifuFlatButton4.Click += new System.EventHandler(this.bunifuFlatButton5_Click);
        // 
        // bunifuFlatButton3
        // 
        this.bunifuFlatButton3.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.bunifuFlatButton3.BorderRadius = 0;
        this.bunifuFlatButton3.ButtonText = "   New Ticket";
        this.bunifuFlatButton3.Cursor = System.Windows.Forms.Cursors.Hand;
        this.bunifuTransition_0.SetDecoration(this.bunifuFlatButton3, BunifuAnimatorNS.DecorationType.None);
        this.bunifuFlatButton3.DisabledColor = System.Drawing.Color.Gray;
        this.bunifuFlatButton3.Iconcolor = System.Drawing.Color.Transparent;
        this.bunifuFlatButton3.Iconimage = null;
        this.bunifuFlatButton3.Iconimage_right = null;
        this.bunifuFlatButton3.Iconimage_right_Selected = null;
        this.bunifuFlatButton3.Iconimage_Selected = null;
        this.bunifuFlatButton3.IconMarginLeft = 0;
        this.bunifuFlatButton3.IconMarginRight = 0;
        this.bunifuFlatButton3.IconRightVisible = true;
        this.bunifuFlatButton3.IconRightZoom = 0D;
        this.bunifuFlatButton3.IconVisible = true;
        this.bunifuFlatButton3.IconZoom = 90D;
        this.bunifuFlatButton3.IsTab = false;
        this.bunifuFlatButton3.Location = new System.Drawing.Point(146, 3);
        this.bunifuFlatButton3.Margin = new System.Windows.Forms.Padding(5);
        this.bunifuFlatButton3.Name = "bunifuFlatButton3";
        this.bunifuFlatButton3.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
        this.bunifuFlatButton3.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
        this.bunifuFlatButton3.OnHoverTextColor = System.Drawing.Color.White;
        this.bunifuFlatButton3.selected = false;
        this.bunifuFlatButton3.Size = new System.Drawing.Size(140, 23);
        this.bunifuFlatButton3.TabIndex = 1204;
        this.bunifuFlatButton3.Text = "   New Ticket";
        this.bunifuFlatButton3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.bunifuFlatButton3.Textcolor = System.Drawing.Color.White;
        this.bunifuFlatButton3.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.bunifuFlatButton3.Click += new System.EventHandler(this.BunifuFlatButton3_Click);
        // 
        // bunifuCustomLabel3
        // 
        this.bunifuCustomLabel3.Anchor = System.Windows.Forms.AnchorStyles.Right;
        this.bunifuCustomLabel3.AutoSize = true;
        this.bunifuCustomLabel3.BackColor = System.Drawing.Color.Transparent;
        this.bunifuTransition_0.SetDecoration(this.bunifuCustomLabel3, BunifuAnimatorNS.DecorationType.None);
        this.bunifuCustomLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
        this.bunifuCustomLabel3.ForeColor = System.Drawing.Color.Green;
        this.bunifuCustomLabel3.Location = new System.Drawing.Point(466, 0);
        this.bunifuCustomLabel3.Name = "bunifuCustomLabel3";
        this.bunifuCustomLabel3.Size = new System.Drawing.Size(86, 24);
        this.bunifuCustomLabel3.TabIndex = 1219;
        this.bunifuCustomLabel3.Text = "Recived";
        this.bunifuCustomLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // bunifuCustomLabel4
        // 
        this.bunifuCustomLabel4.Anchor = System.Windows.Forms.AnchorStyles.Right;
        this.bunifuCustomLabel4.AutoSize = true;
        this.bunifuCustomLabel4.BackColor = System.Drawing.Color.Transparent;
        this.bunifuTransition_0.SetDecoration(this.bunifuCustomLabel4, BunifuAnimatorNS.DecorationType.None);
        this.bunifuCustomLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
        this.bunifuCustomLabel4.ForeColor = System.Drawing.Color.White;
        this.bunifuCustomLabel4.Location = new System.Drawing.Point(378, -1);
        this.bunifuCustomLabel4.Name = "bunifuCustomLabel4";
        this.bunifuCustomLabel4.Size = new System.Drawing.Size(93, 24);
        this.bunifuCustomLabel4.TabIndex = 1218;
        this.bunifuCustomLabel4.Text = "Payment :";
        this.bunifuCustomLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // panel1
        // 
        this.panel1.BackColor = System.Drawing.Color.FromArgb(22, 14, 28);
        this.panel1.Controls.Add(this.lbl_Date);
        this.panel1.Controls.Add(this.label50);
        this.panel1.Controls.Add(this.btnClose);
        this.panel1.Controls.Add(this.bunifuCustomLabel3);
        this.panel1.Controls.Add(this.bunifuCustomLabel4);
        this.bunifuTransition_0.SetDecoration(this.panel1, BunifuAnimatorNS.DecorationType.None);
        this.panel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(18)))));
        this.panel1.Location = new System.Drawing.Point(-3, 0);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(582, 23);
        this.panel1.TabIndex = 1212;
        // 
        // lbl_Date
        // 
        this.lbl_Date.Anchor = System.Windows.Forms.AnchorStyles.Right;
        this.lbl_Date.AutoSize = true;
        this.lbl_Date.BackColor = System.Drawing.Color.Transparent;
        this.bunifuTransition_0.SetDecoration(this.lbl_Date, BunifuAnimatorNS.DecorationType.None);
        this.lbl_Date.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lbl_Date.ForeColor = System.Drawing.Color.Black;
        this.lbl_Date.Location = new System.Drawing.Point(195, 178);
        this.lbl_Date.Name = "lbl_Date";
        this.lbl_Date.Size = new System.Drawing.Size(177, 24);
        this.lbl_Date.TabIndex = 169;
        this.lbl_Date.Text = "22-09-2022 22:23:55";
        this.lbl_Date.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // label50
        // 
        this.label50.AutoSize = true;
        this.label50.BackColor = System.Drawing.Color.Transparent;
        this.bunifuTransition_0.SetDecoration(this.label50, BunifuAnimatorNS.DecorationType.None);
        this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label50.Location = new System.Drawing.Point(3, 2);
        this.label50.Name = "label50";
        this.label50.Size = new System.Drawing.Size(77, 20);
        this.label50.TabIndex = 0;
        this.label50.Text = "Express";
        // 
        // btnClose
        // 
        this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
        this.btnClose.BackColor = System.Drawing.Color.Transparent;
        this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
        this.bunifuTransition_0.SetDecoration(this.btnClose, BunifuAnimatorNS.DecorationType.None);
        this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
        this.btnClose.ImageActive = null;
        this.btnClose.Location = new System.Drawing.Point(557, 0);
        this.btnClose.Margin = new System.Windows.Forms.Padding(2);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(23, 23);
        this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.btnClose.TabIndex = 1;
        this.btnClose.TabStop = false;
        this.btnClose.Zoom = 10;
        this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
        // 
        // bunifuDragControl_0
        // 
        this.bunifuDragControl_0.Fixed = true;
        this.bunifuDragControl_0.Horizontal = true;
        this.bunifuDragControl_0.TargetControl = this.pnlHeader;
        this.bunifuDragControl_0.Vertical = true;
        // 
        // imageList_0
        // 
        this.imageList_0.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
        this.imageList_0.ImageSize = new System.Drawing.Size(16, 16);
        this.imageList_0.TransparentColor = System.Drawing.Color.Transparent;
        // 
        // bunifuTransition_1
        // 
        this.bunifuTransition_1.AnimationType = BunifuAnimatorNS.AnimationType.HorizSlide;
        this.bunifuTransition_1.Cursor = null;
        animation4.AnimateOnlyDifferences = true;
        animation4.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation4.BlindCoeff")));
        animation4.LeafCoeff = 0F;
        animation4.MaxTime = 1F;
        animation4.MinTime = 0F;
        animation4.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation4.MosaicCoeff")));
        animation4.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation4.MosaicShift")));
        animation4.MosaicSize = 0;
        animation4.Padding = new System.Windows.Forms.Padding(0);
        animation4.RotateCoeff = 0F;
        animation4.RotateLimit = 0F;
        animation4.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation4.ScaleCoeff")));
        animation4.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation4.SlideCoeff")));
        animation4.TimeCoeff = 0F;
        animation4.TransparencyCoeff = 0F;
        this.bunifuTransition_1.DefaultAnimation = animation4;
        // 
        // timer_0
        // 
        this.timer_0.Interval = 1000;
        this.timer_0.Tick += new System.EventHandler(this.timer_0_Tick);
        // 
        // frmMain
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
        this.ClientSize = new System.Drawing.Size(857, 353);
        this.Controls.Add(this.pnlHeader);
        this.Controls.Add(this.C3AA0);
        this.Controls.Add(this.lblUsrMsg);
        this.Controls.Add(this.panel1);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.KeyPreview = true;
        this.MaximizeBox = false;
        this.MinimumSize = new System.Drawing.Size(10, 3);
        this.Name = "frmMain";
        this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
        this.Text = "Express";
        this.TopMost = true;
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
        this.Load += new System.EventHandler(this.frmMain_Load);
        this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyDown);
        this.pnlHeader.ResumeLayout(false);
        this.pnlHeader.PerformLayout();
        this.menuStrip1.ResumeLayout(false);
        this.menuStrip1.PerformLayout();
        this.panel1.ResumeLayout(false);
        this.panel1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    private void pnlHeader_Paint(object sender, PaintEventArgs e)
    {

    }

    private void bunifuFlatButton1_Click_1(object sender, EventArgs e)
    {
        new frmCombine().ShowDialog();
    }

    private void bunifuFlatButton8_Click_1(object sender, EventArgs e)
    {
        new frmBackup(base.Icon).ShowDialog();
    }
    bool isUpdating = false;
    bool isUpdating1 = false;

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBox1.Checked)
        {
            StatClass.VideoMode = true;
        }
        else
        {
            StatClass.VideoMode = false;
        }
    }

    private void checkBox2_CheckedChanged(object sender, EventArgs e)
    {
        Program.mySettings_0.CloseOnPnr = false;
        StatClass.CloseOnPnr = Program.mySettings_0.CloseOnPnr;
        checkBox2.Checked = !checkBox2.Checked;
        if (checkBox2.Checked)
        {
            Program.mySettings_0.CloseOnPnr = true;
            StatClass.CloseOnPnr = Program.mySettings_0.CloseOnPnr;
        }
    }

    private void bunifuImageButton1_Click(object sender, EventArgs e)
    {

    }

    private void checkBox3_CheckedChanged(object sender, EventArgs e)
    {
        if (isUpdating) return;
        isUpdating = true;

        if (checkBox3.Checked)
            chkADV.Checked = false;
        checkBox5.Checked = false;
        StatClass.bookingmode1 = false;
        StatClass.bookingmode3 = false;
        StatClass.bookingmode2 = checkBox3.Checked;
        isUpdating = false;
    }

    private void checkBox4_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBox4.Checked)
        {
            StatClass.manualclick = true;
        }
        else
        {
            StatClass.manualclick = false;
        }
    }

    private void checkBox5_CheckedChanged(object sender, EventArgs e)
    {
        if (isUpdating) return;
        isUpdating = true;

        if (checkBox5.Checked)
            chkADV.Checked = false;
        checkBox3.Checked = false;
        StatClass.bookingmode1 = false;
        StatClass.bookingmode2 = false;
        StatClass.bookingmode3 = checkBox5.Checked;
        isUpdating = false;
    }

    private void checkBox6_CheckedChanged(object sender, EventArgs e)
    {
        if (isUpdating1) return;
        isUpdating1 = true;
        if (checkBox6.Checked)
            checkBox7.Checked = false;
        StatClass.app1 = checkBox6.Checked;
        isUpdating1 = false;
    }

    private void checkBox7_CheckedChanged(object sender, EventArgs e)
    {
        if (isUpdating1) return;
        isUpdating1 = true;
        if (checkBox7.Checked)
            checkBox6.Checked = false;
        StatClass.app1 = checkBox6.Checked;
        isUpdating1 = false;
    }

    private void checkBox8_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBox8.Checked)
        {
            StatClass.directlogin = checkBox8.Checked;
        }
        else
        {
            StatClass.directlogin = false;
        }
    }

    private void checkBox9_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBox9.Checked)
        {
            StatClass.unverifiedbook = true;
        }
        else
        {
            StatClass.unverifiedbook = false;
        }
    }
    public static string smethod_000(string string_1)
    {
        string text = "";
        try
        {
            WebClient webClient = new WebClient();
            webClient.Proxy = null;
            webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.2.10) Gecko/20100914 Firefox/3.6.10 ( .NET CLR 3.5.30729; .NET4.0C)");
            return webClient.DownloadString(string_1);
        }
        catch (Exception)
        {
            return "";
        }
    }
    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        string text = "https://novabyte.in/Api/ResetUser.php?";
        string text4 = "Id=" + Program.clientID + "&Mac=" + Program.mac;
        string text5 = smethod_000(text + text4);
        if (text5.Contains("Success"))
        {
            Application.Exit();
            Environment.Exit(0);
        }
    }

    private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
    }

    private void checkBox10_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void checkBox10_CheckedChanged_1(object sender, EventArgs e)
    {
        if (checkBox10.Checked)
        {
            StatClass.stopipblocknew = checkBox8.Checked;
        }
        else
        {
            StatClass.stopipblocknew = false;
        }
    }

    internal static async Task RunEdgeIPCheck()
    {
        throw new NotImplementedException();
    }

    internal static void RunDownloadExtension()
    {
        throw new NotImplementedException();
    }
}
