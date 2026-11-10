using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Resources;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu.Framework.UI;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace APP_FRAMEWORK;

public class Token_count : Form
{
    public class DataObject
    {
        public List<string> data { get; set; } = new List<string>();

    }

    public class Root_
    {
        public int id { get; set; }

        public string username { get; set; }

        public string data { get; set; }
    }

    public class Rootsensordata
    {
        public List<string> data { get; set; }
    }

    public class Rootnew
    {
        public List<string> sensors { get; set; }

        public long timestamp { get; set; }

        public string userId { get; set; }
    }

    public class Root
    {
        public List<string> data { get; set; }
    }

    private string sensordatajson;

    private static readonly byte[] Key = Encoding.UTF8.GetBytes("a4e9f3d2b7c6a1d5");

    private IContainer components = null;

    private Button button1;

    private Label label1;

    private BackgroundWorker backgroundWorker1;

    private Label label2;

    private Label label3;

    private Label label4;

    private LinkLabel linkLabel1;

    private LinkLabel linkLabel2;

    private Button button2;

    private LinkLabel linkLabel3;

    private Button button3;

    private BunifuImageButton btnClose;

    private BunifuImageButton bunifuImageButton1;

    private BunifuImageButton bunifuImageButton2;

    private Panel panelHeader;

    private Label lblTitle;

    private Label lblSubTitle;

    private Label lblTotalTokens;

    private GroupBox grpServer1;

    private Label lblTokens1;

    private GroupBox groupBox1;

    private Label lblTokens2;

    private GroupBox grpApk;

    private Button btnDownloadApk;

    private GroupBox grpInfo;

    private Label lblInfo;

    private Panel panel1;

    private GroupBox groupBox2;

    private Label label6;

    private Label label7;

    private BunifuImageButton bunifuImageButton3;

    private GroupBox groupBox3;

    private Button button4;

    private BunifuImageButton bunifuImageButton4;

    private Button button5;

    private Label label8;

    private Button button6;

    private Label label5;

    private Label lblHeaderUserId;

    public Token_count()
    {
        InitializeComponent();
        StyleForm();
        ApplyCompactFormSize();
        ApplyProxySettingHeaderStyle();
        string environmentVariable = Environment.GetEnvironmentVariable("clientID", EnvironmentVariableTarget.Process);
        label3.Text = "User ID: " + environmentVariable;
        label3.Visible = false; // User ID is displayed only in the ByteNova header.
    }

    private void ApplyCompactFormSize()
    {
        // Keep the existing Token dashboard design/logic, but display it at a compact size.
        const float compactScale = 0.80f;
        SuspendLayout();
        Scale(new SizeF(compactScale, compactScale));
        ClientSize = new Size(800, 544);
        ResumeLayout(true);
    }

    private void ApplyProxySettingHeaderStyle()
    {
        // Match frmProxySetting ByteNova header/close styling only.
        // Token controls and business logic remain unchanged.
        panelHeader.BackColor = Color.FromArgb(3, 29, 59);
        panelHeader.SetBounds(0, 0, ClientSize.Width, 23);
        panelHeader.BringToFront();

        // Move the borderless form by holding the RIGHT mouse button on the header.
        Point byteNovaRightDragOffset = Point.Empty;
        panelHeader.MouseDown += delegate (object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                byteNovaRightDragOffset = new Point(e.X, e.Y);
        };
        panelHeader.MouseMove += delegate (object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Left = Cursor.Position.X - byteNovaRightDragOffset.X;
                Top = Cursor.Position.Y - byteNovaRightDragOffset.Y;
            }
        };

        lblTitle.AutoSize = false;
        lblTitle.SetBounds(9, 1, Math.Max(120, ClientSize.Width - 300), 21);
        lblTitle.Text = "TOKEN  _  EXPRESS";
        lblTitle.ForeColor = Color.White;
        lblTitle.BackColor = Color.Transparent;
        lblTitle.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
        lblTitle.TextAlign = ContentAlignment.MiddleLeft;
        lblTitle.BringToFront();

        // frmProxySetting uses a single compact title row.
        lblSubTitle.Visible = false;

        // Show the current User ID directly in the ByteNova header.
        if (lblHeaderUserId == null)
        {
            lblHeaderUserId = new Label();
            lblHeaderUserId.Name = "lblHeaderUserId";
            panelHeader.Controls.Add(lblHeaderUserId);
        }
        string headerUserId = Environment.GetEnvironmentVariable("clientID", EnvironmentVariableTarget.Process) ?? "";
        lblHeaderUserId.Parent = panelHeader;
        lblHeaderUserId.AutoSize = false;
        lblHeaderUserId.SetBounds(Math.Max(250, ClientSize.Width - 285), 1, 250, 21);
        lblHeaderUserId.Text = "USER ID : " + headerUserId;
        lblHeaderUserId.ForeColor = Color.FromArgb(255, 211, 71);
        lblHeaderUserId.BackColor = Color.Transparent;
        lblHeaderUserId.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
        lblHeaderUserId.TextAlign = ContentAlignment.MiddleRight;
        lblHeaderUserId.BringToFront();

        bunifuImageButton3.Parent = panelHeader;
        bunifuImageButton3.SetBounds(Math.Max(0, ClientSize.Width - 24), 1, 21, 21);
        bunifuImageButton3.BackColor = Color.Transparent;
        bunifuImageButton3.BackgroundImage = null;
        bunifuImageButton3.BackgroundImageLayout = ImageLayout.Zoom;
        bunifuImageButton3.SizeMode = PictureBoxSizeMode.Zoom;
        bunifuImageButton3.Zoom = 0;
        bunifuImageButton3.BringToFront();

        panelHeader.Paint += ByteNovaProxyHeaderPaint;
        bunifuImageButton3.Paint += ByteNovaProxyClosePaint;

        // Remove the old upper App/tab strip and start the main content
        // directly below the compact ByteNova header.
        panel1.Visible = false;

        Control mainContent = null;
        foreach (Control c in Controls)
        {
            if (c == panelHeader || c == panel1 || !c.Visible) continue;
            if (mainContent == null || c.Top > panelHeader.Bottom && c.Width > mainContent.Width)
                mainContent = c;
        }

        if (mainContent != null && mainContent.Top > panelHeader.Bottom)
        {
            int shiftUp = mainContent.Top - panelHeader.Bottom;
            foreach (Control c in Controls)
            {
                if (c == panelHeader || c == panel1) continue;
                if (c.Top >= mainContent.Top)
                    c.Top -= shiftUp;
            }
            ClientSize = new Size(ClientSize.Width, Math.Max(120, ClientSize.Height - shiftUp));
        }

        // Remove unused space below the dashboard and keep the main tab area fitted.
        TabControl compactTabs = Controls.OfType<TabControl>().FirstOrDefault();
        if (compactTabs != null)
        {
            compactTabs.Top = panelHeader.Bottom;
            compactTabs.Height = 432;
            compactTabs.Width = ClientSize.Width;
        }
        ClientSize = new Size(ClientSize.Width, 455);
        panelHeader.Width = ClientSize.Width;
        bunifuImageButton3.Left = ClientSize.Width - 24;
        if (lblHeaderUserId != null)
            lblHeaderUserId.Left = Math.Max(250, ClientSize.Width - 285);
    }

    private void ByteNovaProxyHeaderPaint(object sender, PaintEventArgs e)
    {
        Rectangle r = panelHeader.ClientRectangle;
        if (r.Width < 2 || r.Height < 2) return;

        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(2, 23, 52), Color.FromArgb(5, 53, 91),
            LinearGradientMode.Horizontal))
            e.Graphics.FillRectangle(bg, r);

        using (Pen cyan = new Pen(Color.FromArgb(198, 244, 50), 1.5F))
            e.Graphics.DrawLine(cyan, 0, r.Height - 1, r.Width, r.Height - 1);
    }

    private void ByteNovaProxyClosePaint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0,
            Math.Max(1, bunifuImageButton3.Width - 1),
            Math.Max(1, bunifuImageButton3.Height - 1));

        using (SolidBrush bg = new SolidBrush(Color.FromArgb(4, 35, 61)))
            e.Graphics.FillRectangle(bg, r);
        using (Pen border = new Pen(Color.FromArgb(198, 244, 50), 1F))
            e.Graphics.DrawRectangle(border, r);
        using (Pen x = new Pen(Color.FromArgb(120, 230, 255), 1.6F))
        {
            e.Graphics.DrawLine(x, 6, 6, bunifuImageButton3.Width - 7, bunifuImageButton3.Height - 7);
            e.Graphics.DrawLine(x, bunifuImageButton3.Width - 7, 6, 6, bunifuImageButton3.Height - 7);
        }
    }

    private void StyleForm()
    {
        BackColor = Color.FromArgb(1, 10, 20);
        ForeColor = Color.FromArgb(225, 242, 252);
        base.FormBorderStyle = FormBorderStyle.None;
        base.StartPosition = FormStartPosition.CenterScreen;
        base.ClientSize = new Size(1000, 680);
        base.Paint += delegate (object sender, PaintEventArgs e)
        {
            using Pen pen4 = new Pen(Color.FromArgb(198, 244, 50), 1.5f);
            e.Graphics.DrawRectangle(pen4, 0, 0, base.Width - 1, base.Height - 1);
        };
        panelHeader.BackColor = Color.FromArgb(3, 29, 59);
        panelHeader.Width = base.Width;
        panelHeader.Height = 65;
        panelHeader.Location = new Point(0, 0);
        panelHeader.Paint += delegate (object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (LinearGradientBrush headerBg = new LinearGradientBrush(panelHeader.ClientRectangle, Color.FromArgb(2, 23, 52), Color.FromArgb(5, 53, 91), LinearGradientMode.Horizontal))
                e.Graphics.FillRectangle(headerBg, panelHeader.ClientRectangle);
            using (Pen headerLine = new Pen(Color.FromArgb(198, 244, 50), 1.5f))
                e.Graphics.DrawLine(headerLine, 0, panelHeader.Height - 1, panelHeader.Width, panelHeader.Height - 1);
            using (GraphicsPath graphicsPath4 = new GraphicsPath())
            {
                graphicsPath4.AddArc(16, 12, 10, 10, 180f, 90f);
                graphicsPath4.AddArc(46, 12, 10, 10, 270f, 90f);
                graphicsPath4.AddArc(46, 42, 10, 10, 0f, 90f);
                graphicsPath4.AddArc(16, 42, 10, 10, 90f, 90f);
                graphicsPath4.CloseAllFigures();
                using SolidBrush brush3 = new SolidBrush(Color.FromArgb(0, 125, 205));
                e.Graphics.FillPath(brush3, graphicsPath4);
            }
            using Font font2 = new Font("Segoe UI Emoji", 14f);
            TextRenderer.DrawText(e.Graphics, "\ud83d\udee1\ufe0f", font2, new Rectangle(16, 12, 40, 40), Color.FromArgb(31, 19, 43), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        };
        Point dragOffset = Point.Empty;
        panelHeader.MouseDown += delegate (object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragOffset = new Point(e.X, e.Y);
            }
        };
        panelHeader.MouseMove += delegate (object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                base.Left = Cursor.Position.X - dragOffset.X;
                base.Top = Cursor.Position.Y - dragOffset.Y;
            }
        };
        lblTitle.Font = new Font("Segoe UI", 15f, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(31, 19, 43);
        lblTitle.Location = new Point(68, 10);
        lblTitle.Text = "Express  •  TOKENS";
        lblTitle.BackColor = Color.Transparent;
        lblSubTitle.Font = new Font("Segoe UI", 8.5f, FontStyle.Regular);
        lblSubTitle.ForeColor = Color.FromArgb(155, 225, 248);
        lblSubTitle.Location = new Point(69, 36);
        lblSubTitle.Text = "PREMIUM TATKAL SOFTWARE  •  TOKEN DASHBOARD";
        lblSubTitle.BackColor = Color.Transparent;
        try
        {
            bunifuImageButton3.BackColor = Color.FromArgb(4, 35, 61);
            bunifuImageButton3.Size = new Size(26, 26);
            bunifuImageButton3.Location = new Point(base.Width - 42, 19);
            bunifuImageButton3.Paint += delegate (object sender, PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (GraphicsPath graphicsPath3 = new GraphicsPath())
                {
                    graphicsPath3.AddEllipse(0, 0, bunifuImageButton3.Width - 1, bunifuImageButton3.Height - 1);
                    using SolidBrush brush2 = new SolidBrush(Color.FromArgb(4, 35, 61));
                    e.Graphics.FillPath(brush2, graphicsPath3);
                }
                using Font font = new Font("Segoe UI", 9f, FontStyle.Bold);
                TextRenderer.DrawText(e.Graphics, "✕", font, bunifuImageButton3.ClientRectangle, Color.FromArgb(120, 230, 255), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            };
        }
        catch
        {
        }
        panel1.BackColor = Color.FromArgb(31, 19, 43);
        panel1.Location = new Point(0, 65);
        panel1.Size = new Size(base.Width, 50);
        panel1.Controls.Clear();
        TabControl tabControlMain = new TabControl();
        tabControlMain.Name = "tabControlMain";
        tabControlMain.SizeMode = TabSizeMode.Fixed;
        tabControlMain.ItemSize = new Size(0, 1);
        tabControlMain.Location = new Point(0, 115);
        tabControlMain.Size = new Size(base.Width, 565);
        base.Controls.Add(tabControlMain);
        TabPage tabPage = new TabPage("Web")
        {
            BackColor = Color.FromArgb(1, 10, 20)
        };
        TabPage tabPage2 = new TabPage("Web 2")
        {
            BackColor = Color.FromArgb(1, 10, 20)
        };
        TabPage tabPage3 = new TabPage("App")
        {
            BackColor = Color.FromArgb(1, 10, 20)
        };
        tabControlMain.TabPages.AddRange(new TabPage[1] { tabPage3 });
        Button btnTabWeb = new Button();
        Button btnTabWeb2 = new Button();
        Button btnTabApp = new Button();
        StyleTabButton(btnTabApp, "\ud83d\udcf1  App");
        btnTabApp.Location = new Point(20, 8);
        btnTabApp.Size = new Size(120, 34);
        btnTabApp.Click += delegate
        {
            SelectTab(0, btnTabWeb, btnTabWeb2, btnTabApp, tabControlMain);
            panel1.Invalidate();
        };
        panel1.Controls.Add(btnTabApp);
        StyleTabButton(btnTabWeb, "\ud83c\udf10  Web");
        btnTabWeb.Location = new Point(150, 8);
        btnTabWeb.Size = new Size(120, 34);
        btnTabWeb.Click += delegate
        {
            SelectTab(1, btnTabWeb, btnTabWeb2, btnTabApp, tabControlMain);
            panel1.Invalidate();
        };
        StyleTabButton(btnTabWeb2, "\ud83c\udf10  Web 2");
        btnTabWeb2.Location = new Point(280, 8);
        btnTabWeb2.Size = new Size(120, 34);
        btnTabWeb2.Click += delegate
        {
            SelectTab(2, btnTabWeb, btnTabWeb2, btnTabApp, tabControlMain);
            panel1.Invalidate();
        };
        panel1.Paint += delegate (object sender, PaintEventArgs e)
        {
            using (Pen pen3 = new Pen(Color.FromArgb(155, 93, 229), 1f))
            {
                e.Graphics.DrawLine(pen3, 0, panel1.Height - 1, panel1.Width, panel1.Height - 1);
            }
            using SolidBrush brush = new SolidBrush(Color.FromArgb(198, 244, 50));
            switch (tabControlMain.SelectedIndex)
            {
                case 0:
                    e.Graphics.FillRectangle(brush, 20, panel1.Height - 3, 120, 3);
                    break;
                case 1:
                    e.Graphics.FillRectangle(brush, 150, panel1.Height - 3, 120, 3);
                    break;
                case 2:
                    e.Graphics.FillRectangle(brush, 280, panel1.Height - 3, 120, 3);
                    break;
            }
        };
        Label badgeLabel = new Label
        {
            Name = "lblChromeInstallBadge",
            Text = "Checking..."
        };
        Label badgeLabel2 = new Label
        {
            Name = "lblChromeTokenBadge",
            Text = "Checking..."
        };
        Panel value = CreateStatusCard("Gadar Browser Checker", "Found in chrome (Default) · via Secure Preferences", badgeLabel, "\ud83e\udde9", new Point(20, 20));
        Panel value2 = CreateStatusCard("Current tokens", "Check check_ip_block_chrome file status", badgeLabel2, "\ud83d\udee1\ufe0f", new Point(520, 20));
        tabPage.Controls.Add(value);
        tabPage.Controls.Add(value2);
        Panel panel = new Panel
        {
            Size = new Size(960, 350),
            Location = new Point(20, 120),
            BackColor = Color.FromArgb(31, 19, 43)
        };
        StyleCardPanel(panel);
        tabPage.Controls.Add(panel);
        Label value3 = new Label
        {
            Text = "\ud83d\udccb   INSTRUCTIONS",
            Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(255, 211, 71),
            Location = new Point(20, 15),
            Size = new Size(200, 25),
            BackColor = Color.FromArgb(31, 19, 43)
        };
        panel.Controls.Add(value3);
        AddNumberedStep(panel, "1", "Check IP karne se pehle yeh ensure kar lijiye ki aap Chrome browser use kar rahe hain aur usme aapka Gmail account login hona zaruri hai. Is baat ka khas dhyan rakhein.", new Point(20, 140), new Size(620, 38));
        AddNumberedStep(panel, "2", "Token generate karne ke baad IRCTC ka page open hoga. Uske baad aapko Login se lekar Payment tak ka pura process manually complete karna hai.", new Point(20, 185), new Size(620, 38));
        Label value4 = new Label
        {
            Location = new Point(20, 235),
            Size = new Size(620, 1),
            BackColor = Color.FromArgb(37, 23, 51)
        };
        panel.Controls.Add(value4);
        Label value5 = new Label
        {
            Text = "\ud83d\udca1   DHYAN RAKHEIN:",
            Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(255, 211, 71),
            Location = new Point(20, 245),
            Size = new Size(200, 20),
            BackColor = Color.FromArgb(31, 19, 43)
        };
        panel.Controls.Add(value5);
        AddCheckmarkItem(panel, "Jis Route ka Tatkal ticket book kar rahe hain, usi route ka selection karein.", new Point(20, 275), new Size(600, 18));
        AddCheckmarkItem(panel, "Passenger Details bhi wahi rakhein jinke liye booking karni hai.", new Point(20, 295), new Size(600, 18));
        AddCheckmarkItem(panel, "Payment Gateway bhi wahi select karein jo actual booking me use karenge.", new Point(20, 315), new Size(600, 18));
        AddCheckmarkItem(panel, "Iske baad Check IP process complete karein.", new Point(20, 335), new Size(600, 18));
        PictureBox pictureBox = new PictureBox
        {
            Size = new Size(240, 240),
            Location = new Point(680, 50),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.FromArgb(31, 19, 43)
        };
        try
        {
            pictureBox.Image = Image.FromFile("clipboard_shield.png");
        }
        catch
        {
        }
        panel.Controls.Add(pictureBox);
        if (bunifuImageButton4.Parent != this)
        {
            bunifuImageButton4.Parent?.Controls.Remove(bunifuImageButton4);
            base.Controls.Add(bunifuImageButton4);
        }
        bunifuImageButton4.Visible = false;
        bunifuImageButton4.Location = new Point(-2000, -2000);
        Button btnChromeGen = new Button();
        StyleButton(btnChromeGen, "\ud83d\ude80   Generate Tokens", Color.FromArgb(198, 244, 50), Color.FromArgb(255, 211, 71));
        btnChromeGen.Location = new Point(20, 490);
        btnChromeGen.Size = new Size(360, 48);
        tabPage.Controls.Add(btnChromeGen);
        Button button = new Button();
        StyleButton(button, "\ud83d\udd04   Clear", Color.FromArgb(37, 23, 51), Color.FromArgb(155, 93, 229));
        button.ForeColor = Color.FromArgb(190, 220, 235);
        button.Location = new Point(400, 490);
        button.Size = new Size(200, 48);
        button.Click += delegate
        {
            this.button3.PerformClick();
            UpdateTokenStatus();
        };
        tabPage.Controls.Add(button);
        Button btnDownloadChromeExt = new Button();
        StyleButton(btnDownloadChromeExt, "\ud83d\udce5   Download extension", Color.FromArgb(31, 19, 43), Color.FromArgb(37, 23, 51));
        btnDownloadChromeExt.ForeColor = Color.FromArgb(198, 244, 50);
        btnDownloadChromeExt.Paint += delegate (object sender, PaintEventArgs e)
        {
            using Pen pen2 = new Pen(Color.FromArgb(198, 244, 50), 1.5f);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using GraphicsPath graphicsPath2 = new GraphicsPath();
            graphicsPath2.AddArc(0, 0, 12, 12, 180f, 90f);
            graphicsPath2.AddArc(btnDownloadChromeExt.Width - 12 - 1, 0, 12, 12, 270f, 90f);
            graphicsPath2.AddArc(btnDownloadChromeExt.Width - 12 - 1, btnDownloadChromeExt.Height - 12 - 1, 12, 12, 0f, 90f);
            graphicsPath2.AddArc(0, btnDownloadChromeExt.Height - 12 - 1, 12, 12, 90f, 90f);
            graphicsPath2.CloseAllFigures();
            e.Graphics.DrawPath(pen2, graphicsPath2);
        };
        btnDownloadChromeExt.Location = new Point(620, 490);
        btnDownloadChromeExt.Size = new Size(340, 48);
        btnDownloadChromeExt.Click += delegate
        {
        };
        tabPage.Controls.Add(btnDownloadChromeExt);
        Label badgeLabel3 = new Label
        {
            Name = "lblEdgeInstallBadge",
            Text = "Checking..."
        };
        Label badgeLabel4 = new Label
        {
            Name = "lblEdgeTokenBadge",
            Text = "Checking..."
        };
        Panel value6 = CreateStatusCard("Gadar Browser Checker", "Found in Edge (Default) · via Secure Preferences", badgeLabel3, "\ud83e\udde9", new Point(20, 20));
        Panel value7 = CreateStatusCard("Current tokens", "Check check_ip_block_edge file status", badgeLabel4, "\ud83d\udee1\ufe0f", new Point(520, 20));
        tabPage2.Controls.Add(value6);
        tabPage2.Controls.Add(value7);
        Panel panel2 = new Panel
        {
            Size = new Size(960, 350),
            Location = new Point(20, 120),
            BackColor = Color.FromArgb(31, 19, 43)
        };
        StyleCardPanel(panel2);
        tabPage2.Controls.Add(panel2);
        Label value8 = new Label
        {
            Text = "\ud83d\udccb   INSTRUCTIONS",
            Font = new Font("Segoe UI Semibold", 10.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(255, 211, 71),
            Location = new Point(20, 15),
            Size = new Size(200, 25),
            BackColor = Color.FromArgb(31, 19, 43)
        };
        panel2.Controls.Add(value8);
        AddNumberedStep(panel2, "1", "Check IP karne se pehle yeh ensure kar lijiye ki aap Edge browser use kar rahe hain aur usme aapka Gmail account login hona zaruri hai. Is baat ka khas dhyan rakhein.", new Point(20, 140), new Size(620, 38));
        AddNumberedStep(panel2, "2", "Token generate karne ke baad IRCTC ka page open hoga. Uske baad aapko Login se lekar Payment tak ka pura process manually complete karna hai.", new Point(20, 185), new Size(620, 38));
        Label value9 = new Label
        {
            Location = new Point(20, 235),
            Size = new Size(620, 1),
            BackColor = Color.FromArgb(37, 23, 51)
        };
        panel2.Controls.Add(value9);
        Label value10 = new Label
        {
            Text = "\ud83d\udca1   DHYAN RAKHEIN:",
            Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(255, 211, 71),
            Location = new Point(20, 245),
            Size = new Size(200, 20),
            BackColor = Color.FromArgb(31, 19, 43)
        };
        panel2.Controls.Add(value10);
        AddCheckmarkItem(panel2, "Jis Route ka Tatkal ticket book kar rahe hain, usi route ka selection karein.", new Point(20, 275), new Size(600, 18));
        AddCheckmarkItem(panel2, "Passenger Details bhi wahi rakhein jinke liye booking karni hai.", new Point(20, 295), new Size(600, 18));
        AddCheckmarkItem(panel2, "Payment Gateway bhi wahi select karein jo actual booking me use karenge.", new Point(20, 315), new Size(600, 18));
        AddCheckmarkItem(panel2, "Iske baad Check IP process complete karein.", new Point(20, 335), new Size(600, 18));
        PictureBox pictureBox2 = new PictureBox
        {
            Size = new Size(240, 240),
            Location = new Point(680, 50),
            SizeMode = PictureBoxSizeMode.Zoom,
            BackColor = Color.FromArgb(31, 19, 43)
        };
        try
        {
            pictureBox2.Image = Image.FromFile("clipboard_shield.png");
        }
        catch
        {
        }
        panel2.Controls.Add(pictureBox2);
        if (bunifuImageButton2.Parent != this)
        {
            bunifuImageButton2.Parent?.Controls.Remove(bunifuImageButton2);
            base.Controls.Add(bunifuImageButton2);
        }
        bunifuImageButton2.Visible = false;
        bunifuImageButton2.Location = new Point(-2000, -2000);
        Button btnEdgeGen = new Button();
        StyleButton(btnEdgeGen, "\ud83d\ude80   Generate Tokens", Color.FromArgb(198, 244, 50), Color.FromArgb(255, 211, 71));
        btnEdgeGen.Location = new Point(20, 490);
        btnEdgeGen.Size = new Size(360, 48);
        btnEdgeGen.Click += async delegate
        {
            btnEdgeGen.Enabled = false;
            btnEdgeGen.Text = "Opening Edge...";
            await frmMain.RunEdgeIPCheck();
            await Task.Delay(2000);
            btnEdgeGen.Text = "\ud83d\ude80   Generate Tokens";
            btnEdgeGen.Enabled = true;
            UpdateTokenStatus();
        };
        tabPage2.Controls.Add(btnEdgeGen);
        Button button2 = new Button();
        StyleButton(button2, "\ud83d\udd04   Clear", Color.FromArgb(37, 23, 51), Color.FromArgb(155, 93, 229));
        button2.ForeColor = Color.FromArgb(190, 220, 235);
        button2.Location = new Point(400, 490);
        button2.Size = new Size(200, 48);
        button2.Click += delegate
        {
            this.button3.PerformClick();
            UpdateTokenStatus();
        };
        tabPage2.Controls.Add(button2);
        Button btnDownloadEdgeExt = new Button();
        StyleButton(btnDownloadEdgeExt, "\ud83d\udce5   Download extension", Color.FromArgb(31, 19, 43), Color.FromArgb(37, 23, 51));
        btnDownloadEdgeExt.ForeColor = Color.FromArgb(198, 244, 50);
        btnDownloadEdgeExt.Paint += delegate (object sender, PaintEventArgs e)
        {
            using Pen pen = new Pen(Color.FromArgb(198, 244, 50), 1.5f);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddArc(0, 0, 12, 12, 180f, 90f);
            graphicsPath.AddArc(btnDownloadEdgeExt.Width - 12 - 1, 0, 12, 12, 270f, 90f);
            graphicsPath.AddArc(btnDownloadEdgeExt.Width - 12 - 1, btnDownloadEdgeExt.Height - 12 - 1, 12, 12, 0f, 90f);
            graphicsPath.AddArc(0, btnDownloadEdgeExt.Height - 12 - 1, 12, 12, 90f, 90f);
            graphicsPath.CloseAllFigures();
            e.Graphics.DrawPath(pen, graphicsPath);
        };
        btnDownloadEdgeExt.Location = new Point(620, 490);
        btnDownloadEdgeExt.Size = new Size(340, 48);
        btnDownloadEdgeExt.Click += delegate
        {
            frmMain.RunDownloadExtension();
        };
        tabPage2.Controls.Add(btnDownloadEdgeExt);
        Panel panel3 = new Panel
        {
            Size = new Size(310, 520),
            Location = new Point(20, 20),
            BackColor = Color.FromArgb(31, 19, 43)
        };
        StyleCardPanel(panel3);
        tabPage3.Controls.Add(panel3);
        Label value11 = new Label
        {
            Text = "\ud83d\udd11",
            Font = new Font("Segoe UI Emoji", 11f),
            Location = new Point(20, 18),
            Size = new Size(22, 22),
            BackColor = Color.FromArgb(31, 19, 43)
        };
        panel3.Controls.Add(value11);
        if (lblTokens1.Parent != panel3)
        {
            lblTokens1.Parent?.Controls.Remove(lblTokens1);
            panel3.Controls.Add(lblTokens1);
        }
        lblTokens1.Font = new Font("Segoe UI Semibold", 11.5f, FontStyle.Bold);
        lblTokens1.ForeColor = Color.FromArgb(198, 244, 50);
        lblTokens1.BackColor = Color.FromArgb(31, 19, 43);
        lblTokens1.Location = new Point(46, 16);
        lblTokens1.Size = new Size(200, 24);
        lblTokens1.TextAlign = ContentAlignment.MiddleLeft;
        if (btnClose.Parent != panel3)
        {
            btnClose.Parent?.Controls.Remove(btnClose);
            panel3.Controls.Add(btnClose);
        }
        btnClose.Location = new Point(60, 52);
        btnClose.Size = new Size(190, 190);
        btnClose.Visible = true;
        if (label1.Parent != panel3)
        {
            label1.Parent?.Controls.Remove(label1);
            panel3.Controls.Add(label1);
        }
        label1.Location = new Point(10, 248);
        label1.Size = new Size(290, 24);
        label1.Font = new Font("Segoe UI Semibold", 7.5f, FontStyle.Italic);
        label1.ForeColor = Color.FromArgb(170, 210, 230);
        label1.TextAlign = ContentAlignment.MiddleCenter;
        label1.BackColor = Color.FromArgb(31, 19, 43);
        label1.Visible = true;
        if (label2.Parent != this)
        {
            label2.Parent?.Controls.Remove(label2);
            base.Controls.Add(label2);
        }
        label2.Location = new Point(-2000, -2000);
        label2.Visible = false;
        if (btnDownloadApk.Parent != panel3)
        {
            btnDownloadApk.Parent?.Controls.Remove(btnDownloadApk);
            panel3.Controls.Add(btnDownloadApk);
        }
        StyleButton(btnDownloadApk, "\ud83d\udce5   Download Normal APK v4.3.14", Color.FromArgb(0, 125, 205), Color.FromArgb(0, 160, 230));
        btnDownloadApk.Location = new Point(20, 275);
        btnDownloadApk.Size = new Size(270, 42);
        if (button1.Parent != panel3)
        {
            button1.Parent?.Controls.Remove(button1);
            panel3.Controls.Add(button1);
        }
        StyleButton(button1, "\ud83d\udee2\ufe0f   Find Tokens on Server 1", Color.FromArgb(16, 124, 65), Color.FromArgb(13, 107, 56));
        button1.Location = new Point(20, 327);
        button1.Size = new Size(270, 42);
        Panel value12 = CreateTipCallout("Ensure your internet connection is stable\nbefore generating tokens.", new Point(20, 382), new Size(270, 72));
        panel3.Controls.Add(value12);
        Button button3 = new Button();
        StyleButton(button3, "\ud83d\uddd1\ufe0f   Clear Tokens", Color.FromArgb(220, 38, 38), Color.FromArgb(185, 28, 28));
        button3.Location = new Point(20, 462);
        button3.Size = new Size(270, 42);
        button3.Click += delegate
        {
            this.button3.PerformClick();
            UpdateTokenStatus();
        };
        panel3.Controls.Add(button3);
        Panel panel4 = new Panel
        {
            Size = new Size(610, 520),
            Location = new Point(350, 20),
            BackColor = Color.FromArgb(31, 19, 43)
        };
        StyleCardPanel(panel4);
        tabPage3.Controls.Add(panel4);
        Label value13 = new Label
        {
            Text = "\ud83d\udccb   APP Login – Important Instructions:",
            Font = new Font("Segoe UI Semibold", 11f, FontStyle.Bold),
            ForeColor = Color.FromArgb(198, 244, 50),
            Location = new Point(20, 18),
            Size = new Size(400, 25),
            BackColor = Color.FromArgb(31, 19, 43)
        };
        panel4.Controls.Add(value13);
        AddAppInstructionItem(panel4, "Hamesha Token banane se pehle apna Internet (Wi-Fi/Mobile Data) restart karein.", new Point(20, 60), new Size(570, 36));
        AddAppInstructionItem(panel4, "Token banane ke time hamesha ek New ID se login karke hi Token generate karein.", new Point(20, 110), new Size(570, 36));
        AddAppInstructionItem(panel4, "Token generate ho jane ke baad usi ID ko Web Login me use kar sakte hain, lekin App Login me dobara use nahi kar sakte. Is baat ka hamesha dhyan rakhein.", new Point(20, 160), new Size(570, 50));
        AddAppInstructionItem(panel4, "Jo ID AC Quota me use ho chuki hai, usi ID ko SL Quota me use nahi kar sakte.", new Point(20, 225), new Size(570, 36));
        AddAppInstructionItem(panel4, "Jo ID Opening me use ho chuki hai, usi ID ko baad me AC ya SL kisi bhi Quota me use nahi kar sakte.", new Point(20, 275), new Size(570, 36));
        AddAppInstructionItem(panel4, "Hamesha latest Token APK hi use karein. APK ko Scan karke Download karein aur uske baad hi use karein.", new Point(20, 325), new Size(570, 36));
        Panel value14 = CreateWarningCallout("Important:\nUpar diye gaye sabhi points ka dhyan rakhein.", new Point(20, 390), new Size(570, 95));
        panel4.Controls.Add(value14);
        groupBox3.Visible = false;
        groupBox1.Visible = false;
        grpServer1.Visible = false;
        label1.Visible = false;
        label2.Visible = false;
        label5.Visible = false;
        label7.Visible = false;
        groupBox2.Visible = false;
        grpApk.Visible = false;
        grpInfo.Visible = false;
        panel1.Visible = true;
        lblTotalTokens.Visible = false;
        SelectTab(0, btnTabWeb, btnTabWeb2, btnTabApp, tabControlMain);
        UpdateTokenStatus();
    }

    private void StyleTabButton(Button btn, string text)
    {
        btn.Text = text;
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.BackColor = Color.FromArgb(31, 19, 43);
        btn.ForeColor = Color.FromArgb(170, 210, 230);
        btn.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Regular);
        btn.Cursor = Cursors.Hand;
    }

    private void StyleCardPanel(Panel panel)
    {
        panel.BackColor = Color.FromArgb(31, 19, 43);
        panel.Paint += delegate (object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddArc(0, 0, 12, 12, 180f, 90f);
            graphicsPath.AddArc(panel.Width - 12 - 1, 0, 12, 12, 270f, 90f);
            graphicsPath.AddArc(panel.Width - 12 - 1, panel.Height - 12 - 1, 12, 12, 0f, 90f);
            graphicsPath.AddArc(0, panel.Height - 12 - 1, 12, 12, 90f, 90f);
            graphicsPath.CloseAllFigures();
            using Pen pen = new Pen(Color.FromArgb(0, 155, 220), 1.2f);
            e.Graphics.DrawPath(pen, graphicsPath);
        };
    }

    private void MakeRounded(Control ctrl, int radius, Color backColor, Color? borderColor = null)
    {
        ctrl.Paint += delegate (object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (SolidBrush brush = new SolidBrush(ctrl.Parent.BackColor))
            {
                e.Graphics.FillRectangle(brush, ctrl.ClientRectangle);
            }
            using (GraphicsPath graphicsPath = new GraphicsPath())
            {
                graphicsPath.AddArc(0, 0, radius, radius, 180f, 90f);
                graphicsPath.AddArc(ctrl.Width - radius - 1, 0, radius, radius, 270f, 90f);
                graphicsPath.AddArc(ctrl.Width - radius - 1, ctrl.Height - radius - 1, radius, radius, 0f, 90f);
                graphicsPath.AddArc(0, ctrl.Height - radius - 1, radius, radius, 90f, 90f);
                graphicsPath.CloseAllFigures();
                using (SolidBrush brush2 = new SolidBrush(backColor))
                {
                    e.Graphics.FillPath(brush2, graphicsPath);
                }
                if (borderColor.HasValue)
                {
                    using Pen pen = new Pen(borderColor.Value, 1.5f);
                    e.Graphics.DrawPath(pen, graphicsPath);
                }
            }
            TextRenderer.DrawText(e.Graphics, ctrl.Text, ctrl.Font, ctrl.ClientRectangle, ctrl.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        };
    }

    private void SelectTab(int index, Button btnWeb, Button btnWeb2, Button btnApp, TabControl tabCtrl)
    {
        tabCtrl.SelectedIndex = index;
        Color color = Color.FromArgb(198, 244, 50);
        Color color2 = Color.FromArgb(170, 210, 230);
        btnWeb.ForeColor = ((index == 0) ? color : color2);
        btnWeb.Font = new Font("Segoe UI Semibold", 9.5f, (index == 0) ? FontStyle.Bold : FontStyle.Regular);
        btnWeb2.ForeColor = ((index == 1) ? color : color2);
        btnWeb2.Font = new Font("Segoe UI Semibold", 9.5f, (index == 1) ? FontStyle.Bold : FontStyle.Regular);
        btnApp.ForeColor = ((index == 2) ? color : color2);
        btnApp.Font = new Font("Segoe UI Semibold", 9.5f, (index == 2) ? FontStyle.Bold : FontStyle.Regular);
    }

    private void UpdateTokenStatus()
    {
        try
        {
            Label label = base.Controls.Find("lblChromeInstallBadge", searchAllChildren: true).FirstOrDefault() as Label;
            Label label2 = base.Controls.Find("lblChromeTokenBadge", searchAllChildren: true).FirstOrDefault() as Label;
            Label label3 = base.Controls.Find("lblEdgeInstallBadge", searchAllChildren: true).FirstOrDefault() as Label;
            Label label4 = base.Controls.Find("lblEdgeTokenBadge", searchAllChildren: true).FirstOrDefault() as Label;
            bool flag = File.Exists("C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe") || File.Exists("C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe");
            if (label != null)
            {
                label.Text = (flag ? "INSTALLED ✓" : "NOT FOUND");
                label.ForeColor = (flag ? Color.FromArgb(3, 84, 63) : Color.FromArgb(155, 28, 28));
                MakeRounded(label, 12, flag ? Color.FromArgb(222, 247, 236) : Color.FromArgb(253, 232, 232));
            }
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GADAR", "RuntimeData", "Check_IP_Block_Chrome_Browser");
            bool flag2 = File.Exists(path);
            if (label2 != null)
            {
                label2.Text = (flag2 ? "GENERATED ✓" : "NOT GENERATED");
                label2.ForeColor = (flag2 ? Color.FromArgb(3, 84, 63) : Color.FromArgb(155, 28, 28));
                MakeRounded(label2, 12, flag2 ? Color.FromArgb(222, 247, 236) : Color.FromArgb(253, 232, 232));
            }
            bool flag3 = File.Exists("C:\\Program Files\\Microsoft\\Edge\\Application\\msedge.exe") || File.Exists("C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe");
            if (label3 != null)
            {
                label3.Text = (flag3 ? "INSTALLED ✓" : "NOT FOUND");
                label3.ForeColor = (flag3 ? Color.FromArgb(3, 84, 63) : Color.FromArgb(155, 28, 28));
                MakeRounded(label3, 12, flag3 ? Color.FromArgb(222, 247, 236) : Color.FromArgb(253, 232, 232));
            }
            string path2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GADAR", "RuntimeData", "Check_IP_Block_EdgeBrowser");
            bool flag4 = File.Exists(path2);
            if (label4 != null)
            {
                label4.Text = (flag4 ? "GENERATED ✓" : "NOT GENERATED");
                label4.ForeColor = (flag4 ? Color.FromArgb(3, 84, 63) : Color.FromArgb(155, 28, 28));
                MakeRounded(label4, 12, flag4 ? Color.FromArgb(222, 247, 236) : Color.FromArgb(253, 232, 232));
            }
        }
        catch
        {
        }
    }

    private Panel CreateStatusCard(string title, string subtitle, Label badgeLabel, string iconEmoji, Point location)
    {
        Panel panel = new Panel();
        panel.BackColor = Color.FromArgb(31, 19, 43);
        panel.Size = new Size(460, 80);
        panel.Location = location;
        StyleCardPanel(panel);
        Color color = Color.FromArgb(37, 23, 51);
        Color foreColor = Color.FromArgb(0, 160, 230);
        if (iconEmoji == "\ud83e\udde9" || iconEmoji == "⚙\ufe0f" || iconEmoji == "\ud83c\udf10")
        {
            color = Color.FromArgb(4, 35, 61);
            foreColor = Color.FromArgb(198, 244, 50);
            iconEmoji = "⚙\ufe0f";
        }
        else if (iconEmoji == "\ud83d\udee1\ufe0f" || iconEmoji == "✅")
        {
            color = Color.FromArgb(222, 247, 236);
            foreColor = Color.FromArgb(3, 84, 63);
            iconEmoji = "\ud83d\udee1\ufe0f";
        }
        Panel iconCircle = new Panel();
        iconCircle.Size = new Size(48, 48);
        iconCircle.Location = new Point(16, 16);
        iconCircle.BackColor = Color.FromArgb(31, 19, 43);
        Color tempBg = color;
        iconCircle.Paint += delegate (object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddEllipse(0, 0, iconCircle.Width - 1, iconCircle.Height - 1);
            using SolidBrush brush = new SolidBrush(tempBg);
            e.Graphics.FillPath(brush, graphicsPath);
        };
        Label value = new Label
        {
            Text = iconEmoji,
            Font = new Font("Segoe UI Emoji", 13f),
            ForeColor = foreColor,
            Size = new Size(32, 32),
            Location = new Point(8, 8),
            BackColor = Color.Transparent,
            TextAlign = ContentAlignment.MiddleCenter
        };
        iconCircle.Controls.Add(value);
        panel.Controls.Add(iconCircle);
        Label value2 = new Label
        {
            Text = title,
            Font = new Font("Segoe UI Semibold", 10f, FontStyle.Bold),
            ForeColor = Color.FromArgb(198, 244, 50),
            Location = new Point(76, 18),
            Size = new Size(220, 20),
            BackColor = Color.FromArgb(31, 19, 43),
            AutoEllipsis = true
        };
        panel.Controls.Add(value2);
        Label value3 = new Label
        {
            Text = subtitle,
            Font = new Font("Segoe UI", 8f),
            ForeColor = Color.FromArgb(170, 210, 230),
            Location = new Point(76, 40),
            Size = new Size(240, 32),
            BackColor = Color.FromArgb(31, 19, 43),
            AutoEllipsis = true
        };
        panel.Controls.Add(value3);
        badgeLabel.Location = new Point(330, 25);
        badgeLabel.Size = new Size(110, 30);
        badgeLabel.TextAlign = ContentAlignment.MiddleCenter;
        badgeLabel.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
        panel.Controls.Add(badgeLabel);
        return panel;
    }

    private void AddNumberedStep(Panel parent, string number, string text, Point location, Size size)
    {
        Panel numCircle = new Panel();
        numCircle.Size = new Size(22, 22);
        numCircle.Location = new Point(location.X, location.Y + 2);
        numCircle.BackColor = Color.FromArgb(31, 19, 43);
        numCircle.Paint += delegate (object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath graphicsPath = new GraphicsPath())
            {
                graphicsPath.AddEllipse(0, 0, numCircle.Width - 1, numCircle.Height - 1);
                using SolidBrush brush = new SolidBrush(Color.FromArgb(198, 244, 50));
                e.Graphics.FillPath(brush, graphicsPath);
            }
            using Font font = new Font("Segoe UI", 8f, FontStyle.Bold);
            TextRenderer.DrawText(e.Graphics, number, font, numCircle.ClientRectangle, Color.FromArgb(31, 19, 43), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        };
        parent.Controls.Add(numCircle);
        Label value = new Label
        {
            Text = text,
            Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(198, 244, 50),
            Location = new Point(location.X + 32, location.Y),
            Size = new Size(size.Width - 32, size.Height),
            BackColor = Color.FromArgb(31, 19, 43),
            FlatStyle = FlatStyle.System
        };
        parent.Controls.Add(value);
    }

    private void AddCheckmarkItem(Panel parent, string text, Point location, Size size)
    {
        Panel checkCircle = new Panel();
        checkCircle.Size = new Size(18, 18);
        checkCircle.Location = new Point(location.X, location.Y + 1);
        checkCircle.BackColor = Color.FromArgb(31, 19, 43);
        checkCircle.Paint += delegate (object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath graphicsPath = new GraphicsPath())
            {
                graphicsPath.AddEllipse(0, 0, checkCircle.Width - 1, checkCircle.Height - 1);
                using SolidBrush brush = new SolidBrush(Color.FromArgb(198, 244, 50));
                e.Graphics.FillPath(brush, graphicsPath);
            }
            using Font font = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            TextRenderer.DrawText(e.Graphics, "✓", font, checkCircle.ClientRectangle, Color.FromArgb(31, 19, 43), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        };
        parent.Controls.Add(checkCircle);
        Label value = new Label
        {
            Text = text,
            Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
            ForeColor = Color.FromArgb(225, 242, 252),
            Location = new Point(location.X + 24, location.Y),
            Size = new Size(size.Width - 24, size.Height),
            BackColor = Color.FromArgb(31, 19, 43),
            FlatStyle = FlatStyle.System
        };
        parent.Controls.Add(value);
    }

    private void AddAppInstructionItem(Panel parent, string text, Point location, Size size)
    {
        Panel checkCircle = new Panel();
        checkCircle.Size = new Size(18, 18);
        checkCircle.Location = new Point(location.X, location.Y + 2);
        checkCircle.BackColor = Color.FromArgb(31, 19, 43);
        checkCircle.Paint += delegate (object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath graphicsPath = new GraphicsPath())
            {
                graphicsPath.AddEllipse(0, 0, checkCircle.Width - 1, checkCircle.Height - 1);
                using SolidBrush brush = new SolidBrush(Color.FromArgb(198, 244, 50));
                e.Graphics.FillPath(brush, graphicsPath);
            }
            using Font font = new Font("Segoe UI", 7.5f, FontStyle.Bold);
            TextRenderer.DrawText(e.Graphics, "✓", font, checkCircle.ClientRectangle, Color.FromArgb(31, 19, 43), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        };
        parent.Controls.Add(checkCircle);
        Label value = new Label
        {
            Text = text,
            Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
            ForeColor = Color.FromArgb(225, 242, 252),
            Location = new Point(location.X + 28, location.Y),
            Size = new Size(size.Width - 28, size.Height),
            BackColor = Color.FromArgb(31, 19, 43),
            FlatStyle = FlatStyle.System
        };
        parent.Controls.Add(value);
    }

    private Panel CreateWarningCallout(string text, Point location, Size size)
    {
        Panel panel = new Panel();
        panel.Size = size;
        panel.Location = location;
        panel.BackColor = Color.FromArgb(48, 38, 10);
        panel.Paint += delegate (object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddArc(0, 0, 10, 10, 180f, 90f);
            graphicsPath.AddArc(panel.Width - 10 - 1, 0, 10, 10, 270f, 90f);
            graphicsPath.AddArc(panel.Width - 10 - 1, panel.Height - 10 - 1, 10, 10, 0f, 90f);
            graphicsPath.AddArc(0, panel.Height - 10 - 1, 10, 10, 90f, 90f);
            graphicsPath.CloseAllFigures();
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(48, 38, 10)))
            {
                e.Graphics.FillPath(brush, graphicsPath);
            }
            using Pen pen = new Pen(Color.FromArgb(255, 211, 71), 1f);
            e.Graphics.DrawPath(pen, graphicsPath);
        };
        Label value = new Label
        {
            Text = "⚠\ufe0f",
            Font = new Font("Segoe UI Emoji", 14f),
            Size = new Size(30, 30),
            Location = new Point(15, (size.Height - 30) / 2),
            BackColor = Color.FromArgb(48, 38, 10),
            TextAlign = ContentAlignment.MiddleCenter
        };
        panel.Controls.Add(value);
        Label value2 = new Label
        {
            Text = text,
            Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
            ForeColor = Color.FromArgb(255, 220, 90),
            Location = new Point(55, 12),
            Size = new Size(size.Width - 70, size.Height - 24),
            BackColor = Color.FromArgb(48, 38, 10)
        };
        panel.Controls.Add(value2);
        return panel;
    }

    private Panel CreateTipCallout(string text, Point location, Size size)
    {
        Panel panel = new Panel();
        panel.Size = size;
        panel.Location = location;
        panel.BackColor = Color.FromArgb(4, 35, 61);
        panel.Paint += delegate (object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddArc(0, 0, 10, 10, 180f, 90f);
            graphicsPath.AddArc(panel.Width - 10 - 1, 0, 10, 10, 270f, 90f);
            graphicsPath.AddArc(panel.Width - 10 - 1, panel.Height - 10 - 1, 10, 10, 0f, 90f);
            graphicsPath.AddArc(0, panel.Height - 10 - 1, 10, 10, 90f, 90f);
            graphicsPath.CloseAllFigures();
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(4, 35, 61)))
            {
                e.Graphics.FillPath(brush, graphicsPath);
            }
            using Pen pen = new Pen(Color.FromArgb(198, 244, 50), 1f);
            e.Graphics.DrawPath(pen, graphicsPath);
        };
        Label value = new Label
        {
            Text = "ℹ\ufe0f",
            Font = new Font("Segoe UI Emoji", 11f),
            Size = new Size(20, 20),
            Location = new Point(12, 10),
            BackColor = Color.FromArgb(4, 35, 61),
            TextAlign = ContentAlignment.MiddleCenter
        };
        panel.Controls.Add(value);
        Label value2 = new Label
        {
            Text = "TIP",
            Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
            ForeColor = Color.FromArgb(255, 211, 71),
            Location = new Point(36, 12),
            Size = new Size(50, 16),
            BackColor = Color.FromArgb(4, 35, 61)
        };
        panel.Controls.Add(value2);
        Label value3 = new Label
        {
            Text = text,
            Font = new Font("Segoe UI", 7.5f, FontStyle.Regular),
            ForeColor = Color.FromArgb(190, 220, 235),
            Location = new Point(36, 32),
            Size = new Size(size.Width - 48, size.Height - 38),
            BackColor = Color.FromArgb(4, 35, 61)
        };
        panel.Controls.Add(value3);
        return panel;
    }

    private void StyleButton(Button btn, string text, Color normal, Color hover)
    {
        btn.Text = text;
        btn.Font = new Font("Segoe UI Semibold", 9.5f, FontStyle.Bold);
        btn.ForeColor = Color.FromArgb(31, 19, 43);
        btn.BackColor = normal;
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = hover;
        btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(3, 29, 59);
        btn.Cursor = Cursors.Hand;
        btn.Paint += delegate (object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (GraphicsPath graphicsPath = new GraphicsPath())
            {
                graphicsPath.AddArc(0, 0, 12, 12, 180f, 90f);
                graphicsPath.AddArc(btn.Width - 12 - 1, 0, 12, 12, 270f, 90f);
                graphicsPath.AddArc(btn.Width - 12 - 1, btn.Height - 12 - 1, 12, 12, 0f, 90f);
                graphicsPath.AddArc(0, btn.Height - 12 - 1, 12, 12, 90f, 90f);
                graphicsPath.CloseAllFigures();
                using SolidBrush brush = new SolidBrush(btn.BackColor);
                e.Graphics.FillPath(brush, graphicsPath);
            }
            TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, btn.ClientRectangle, btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        };
    }

    public static string Decryptsensor(string cipherTextBase64, string keys)
    {
        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(keys);
            byte[] array = Convert.FromBase64String(cipherTextBase64);
            if (array.Length >= 16)
            {
                byte[] array2 = new byte[16];
                Array.Copy(array, 0, array2, 0, 16);
                byte[] array3 = new byte[array.Length - 16];
                Array.Copy(array, 16, array3, 0, array3.Length);
                using Aes aes = Aes.Create();
                aes.Key = bytes;
                aes.IV = array2;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;
                using ICryptoTransform cryptoTransform = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] bytes2 = cryptoTransform.TransformFinalBlock(array3, 0, array3.Length);
                return Encoding.UTF8.GetString(bytes2);
            }
            throw new Exception("Ciphertext too short");
        }
        catch (Exception)
        {
            try
            {
                keys = "a4e9f3d2b7c6a1d5";
                byte[] bytes3 = Encoding.UTF8.GetBytes(keys);
                byte[] array4 = Convert.FromBase64String(cipherTextBase64);
                if (array4.Length < 16)
                {
                    throw new Exception("Ciphertext too short");
                }
                byte[] array5 = new byte[16];
                Array.Copy(array4, 0, array5, 0, 16);
                byte[] array6 = new byte[array4.Length - 16];
                Array.Copy(array4, 16, array6, 0, array6.Length);
                using Aes aes2 = Aes.Create();
                aes2.Key = bytes3;
                aes2.IV = array5;
                aes2.Mode = CipherMode.CBC;
                aes2.Padding = PaddingMode.PKCS7;
                using ICryptoTransform cryptoTransform2 = aes2.CreateDecryptor(aes2.Key, aes2.IV);
                byte[] bytes4 = cryptoTransform2.TransformFinalBlock(array6, 0, array6.Length);
                return Encoding.UTF8.GetString(bytes4);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }

    private string getnews()
    {
        string text = Environment.GetEnvironmentVariable("awsurl", EnvironmentVariableTarget.Process) + "APKURL.txt";
        string text2 = StatClass.SendRequest(text, null);
        if (text2.Contains("failed to do request"))
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Headers.Add(HttpRequestHeader.UserAgent, "My appp");
                text2 = webClient.DownloadString(new Uri(text));
                webClient.Dispose();
            }
            catch
            {
                return "RW2IUW2NV6TEI7YG3CIMONH33UDKYMZJT2H2VQBYP4PUN7ZNX4TUI37Z5DPFS3HUR42ZERIMZIVX7MQKGIJQLVUIFSADKYBJH4QQODYKV5CSHCQXWEIAYZRODTFO3K5LPGUXR6V3XJTNXIXDHM64B7U3VSYDCMP7QDYPYGGK4YLS5YSHJ2TGZHWYQ47B6ZM7DL32C2Y5BHRPVTWFYBE64QY2WS5VHPV6IT2NPBWPISNLTI2AVFW2CWQICHNCKZ46MMIKEXB6K3JSWNJTFCZMID2CTY";
            }
        }
        return text2;
    }

    private void button1_Click_(object sender, EventArgs e)
    {
        label2.Visible = false;
        button1.Visible = false;
        label1.Visible = true;
        string environmentVariable = Environment.GetEnvironmentVariable("clientID", EnvironmentVariableTarget.Process);
        Encryptsensor(environmentVariable);
        string text = string.Empty;
        string text2 = getnews();
        try
        {
            label1.Text = "Logging into the server...";
            string[] array = text2.Split(new string[1] { "##" }, StringSplitOptions.None);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(array[new Random().Next(array.Length)] + environmentVariable);
            httpWebRequest.Method = "GET";
            httpWebRequest.Accept = "application/json";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64: x64)";
            httpWebRequest.Timeout = 160000;
            httpWebRequest.ReadWriteTimeout = 160000;
            httpWebRequest.ServerCertificateValidationCallback = smethod_1SS;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {
                using StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                text = streamReader.ReadToEnd();
            }
            sensordatajson = text;
            StatClass.sensorjson = sensordatajson;
            if (text.Contains("default-mobile"))
            {
                label1.Text = "Mobile me apk ko background se close kare, then open kare aur token generate kare fir yaha par check kare.";
                SetTokenCount(0, 1);
                return;
            }
            if (text.Contains("[]"))
            {
                label1.Text = "0 Token found, please generate and retry.";
                button1.Visible = true;
                SetTokenCount(0, 1);
                return;
            }
            Rootnew rootnew = JsonConvert.DeserializeObject<Rootnew>(sensordatajson);
            int valueOrDefault = (rootnew?.sensors?.Count).GetValueOrDefault();
            if (valueOrDefault > 0)
            {
                label1.Text = $"Found {valueOrDefault} Token(s). Please proceed with Pair login.";
                SetTokenCount(valueOrDefault, 1);
                string cacheDirectory = getCacheDirectory();
                string path = Path.Combine(cacheDirectory, "VALL.dll");
                Directory.CreateDirectory(cacheDirectory);
                using StreamWriter streamWriter = new StreamWriter(path, append: true);
                foreach (string sensor in rootnew.sensors)
                {
                    streamWriter.WriteLine(sensor);
                }
            }
            else
            {
                label1.Text = "You have 0 Token against this User ID. Please regenerate and retry.";
                button1.Visible = true;
                SetTokenCount(0, 1);
            }
        }
        catch (Exception)
        {
            label1.Text = "Token fetch failed. Don't worry, login can proceed using server tokens.";
            SetTokenCount(0, 1);
        }
        button1.Visible = true;
    }

    private static string getCacheDirectory()
    {
        string text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft", "Windows", "Caches");
        try
        {
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
                new DirectoryInfo(text).Attributes |= FileAttributes.Hidden | FileAttributes.System;
            }
        }
        catch
        {
        }
        return text;
    }

    public static int GetSavedTokenCount()
    {
        try
        {
            string cacheDirectory = getCacheDirectory();
            string path = Path.Combine(cacheDirectory, "VALL.dll");
            if (File.Exists(path))
            {
                return File.ReadAllLines(path).Length;
            }
        }
        catch
        {
        }
        return 0;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        button1.Visible = false;
        GetTokenFromServer(1);
        button1.Visible = true;
    }

    private void GetTokenFromServer(int serverNum)
    {
        label2.Visible = false;
        label1.Visible = true;
        List<string> list = new List<string>();
        try
        {
            bool flag = true;
            while (true)
            {
                if (flag)
                {
                    label1.Text = "Logging into the server...";
                    string[] teslaTsTokenAll = staticstore.GetTeslaTsTokenAll(serverNum);
                    if (teslaTsTokenAll == null)
                    {
                        break;
                    }
                    if (teslaTsTokenAll.Length != 0)
                    {
                        list.AddRange(teslaTsTokenAll.ToList());
                        flag = MessageBox.Show("⚠\ufe0f Agar Apko Aur Token Chahiye To Yes Kre Nhi to Jita Token Mila He Uta Hi He To No Kre. \" +\r\n    \"Uske baad aapka total token count show ho jayega.", "\ud83d\udd14 Alert: Dhyan se padhein", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
                        continue;
                    }
                    label1.Text = "0 Token found, please generate and retry.";
                    button1.Visible = true;
                    SetTokenCount(0, serverNum);
                    return;
                }
                int count = list.Count;
                if (count > 0)
                {
                    label1.Text = $"Found {count} Token(s). Please proceed with Pair login.";
                    SetTokenCount(count, serverNum);
                    string cacheDirectory = getCacheDirectory();
                    string path = Path.Combine(cacheDirectory, "VALL.dll");
                    Directory.CreateDirectory(cacheDirectory);
                    using StreamWriter streamWriter = new StreamWriter(path, append: true);
                    foreach (string item in list)
                    {
                        streamWriter.WriteLine(item);
                    }
                    return;
                }
                label1.Text = "You have 0 Token against this User ID. Please regenerate and retry.";
                SetTokenCount(0, serverNum);
                return;
            }
            label1.Text = "Mobile me apk ko background se close kare, then open kare aur token generate kare fir yaha par check kare.";
            SetTokenCount(0, serverNum);
        }
        catch (Exception)
        {
            label1.Text = "Token fetch failed. Plz try Again and check if your GADAR ID is valid and active.";
            SetTokenCount(0, serverNum);
        }
    }

    private void SetTokenCount(int count, int serverNum)
    {
        Environment.SetEnvironmentVariable("avltoken", count.ToString(), EnvironmentVariableTarget.Process);
        TokenCountState.SetCount(count);
        label2.Visible = true;
        label2.Text = $"Available Token is {count}";
        sensordatajson = ((count > 0) ? sensordatajson : null);
        StatClass.sensorjson = sensordatajson;
        lblTotalTokens.Text = "Total Tokens: " + count;
        if (serverNum == 1)
        {
            lblTokens1.Text = "Tokens: " + count;
        }
        else
        {
            lblTokens2.Text = "Tokens: " + count;
        }
    }

    private void SaveSensorData(string json)
    {
        try
        {
            string cacheDirectory = getCacheDirectory();
            if (!Directory.Exists(cacheDirectory))
            {
                Directory.CreateDirectory(cacheDirectory);
            }
            File.AppendAllText(Path.Combine(cacheDirectory, "VALL.dll"), json + "||||||||||");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error saving sensor data: " + ex.Message);
        }
    }

    private void button1_Click_LLLL(object sender, EventArgs e)
    {
        label2.Visible = false;
        button1.Visible = false;
        label1.Visible = true;
        int num = 0;
        string empty = string.Empty;
        string text = "";
        int num2 = 0;
        string environmentVariable = Environment.GetEnvironmentVariable("clientID", EnvironmentVariableTarget.Process);
        Encryptsensor(environmentVariable);
        try
        {
            label1.Text = "Loggin in to the server..";
            num2++;
            text = "https://auth5.taslats.com/api/v1/post-username";
            string[] array = "https://auth5.taslavalidate.com/sensor/##https://auth6.taslavalidate.com/sensor/##https://auth6.taslavalidate.com/sensor/##https://auth5.taslavalidate.com/sensor/##https://auth6.taslavalidate.com/sensor/".Split(new string[1] { "##" }, StringSplitOptions.None);
            if (array.Length != 0)
            {
                text = array[new Random().Next(array.Length - 1)].ToString();
            }
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(text + environmentVariable);
            httpWebRequest.ServerCertificateValidationCallback = smethod_1SS;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "application/json, text/plain, */*";
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64: x64) AppleWebKit/537.36 (KHTML, like Geck0) Chrome/131.0.0.0 Safari/537.36";
            httpWebRequest.Timeout = 160000;
            httpWebRequest.Referer = "taslats.com";
            httpWebRequest.ReadWriteTimeout = 160000;
            httpWebRequest.Method = "GET";
            using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
            empty = (sensordatajson = streamReader.ReadToEnd());
            try
            {
                string text2 = "";
                new DataObject();
                if (empty.Contains("default-mobile"))
                {
                    label1.Text = "Please mobile me apk ko background se close kare, then open kare aur token generate kare fir yaha par check kare";
                    Environment.SetEnvironmentVariable("avltoken", "0".ToString().Trim(), EnvironmentVariableTarget.Process);
                    label2.Text = "Available Token is " + 0.ToString().Trim();
                    sensordatajson = null;
                    StatClass.sensorjson = sensordatajson;
                }
                if (!empty.Contains("[]"))
                {
                    Rootnew rootnew = JsonConvert.DeserializeObject<Rootnew>(sensordatajson);
                    num = rootnew.sensors.Count;
                    if (rootnew.sensors == null || rootnew.sensors.Count <= 0)
                    {
                        Environment.SetEnvironmentVariable("avltoken", 0.ToString().Trim(), EnvironmentVariableTarget.Process);
                        label1.Text = "You have 0 Token agains this user Id please generate token again and click button again.\r\ndont worry you can do all logins APP , APP2 and Super from our server stored tokens";
                        sensordatajson = null;
                        StatClass.sensorjson = sensordatajson;
                    }
                    if (text2.Contains("default-mobile"))
                    {
                        label1.Text = "Please mobile me apk ko background se close kare\r\nthen open kare aur token generate kare fir yaha par check kare";
                        Environment.SetEnvironmentVariable("avltoken", "0".ToString().Trim(), EnvironmentVariableTarget.Process);
                        label2.Text = "Available Token is " + 0.ToString().Trim();
                        sensordatajson = null;
                        StatClass.sensorjson = sensordatajson;
                        button1.Visible = true;
                        return;
                    }
                    int count = rootnew.sensors.Count;
                    label1.Text = $"Found {count} Token Assigned your Software. Please proceed with Pair login.";
                    Environment.SetEnvironmentVariable("avltoken", count.ToString().Trim(), EnvironmentVariableTarget.Process);
                    StatClass.sensorjson = sensordatajson;
                    try
                    {
                        string sensorjson = StatClass.sensorjson;
                        string cacheDirectory = getCacheDirectory();
                        if (!Directory.Exists(cacheDirectory))
                        {
                            Directory.CreateDirectory(cacheDirectory);
                        }
                        string text3 = Path.Combine(cacheDirectory, "VALL.dll");
                        string text4 = "||||||||||";
                        try
                        {
                            File.AppendAllText(text3, sensorjson + text4);
                            Console.WriteLine("Sensor data appended to: " + text3);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error writing to file: " + ex.Message);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            try
            {
                if (empty.Contains("[]"))
                {
                    Environment.SetEnvironmentVariable("avltoken", 0.ToString().Trim(), EnvironmentVariableTarget.Process);
                    label1.Text = "You have 0 Token agains this user Id please generate token again and click button again.\r\ndont worry you can do all logins APP , APP2 and Super from our server stored tokens";
                    sensordatajson = null;
                    StatClass.sensorjson = sensordatajson;
                }
                if (empty.Contains("default-mobile"))
                {
                    label1.Text = "Please mobile me apk ko background se close kare, then open kare aur token generate kare fir yaha par check kare";
                    Environment.SetEnvironmentVariable("avltoken", "0".ToString().Trim(), EnvironmentVariableTarget.Process);
                    label2.Text = "Available Token is " + 0.ToString().Trim();
                    sensordatajson = null;
                    StatClass.sensorjson = sensordatajson;
                }
                else if (empty.Contains("6,a"))
                {
                    label2.Visible = true;
                    label2.Text = "Available Token is " + num.ToString().Trim() + " Please dont open more than " + num.ToString().Trim() + " Windows for all Forms";
                }
            }
            catch (Exception)
            {
                label1.Text = "0 Token found please retry, dont worry you can do all logins APP , APP2 and Super from our server stored tokens.";
                Environment.SetEnvironmentVariable("avltoken", 0.ToString().Trim(), EnvironmentVariableTarget.Process);
                label2.Text = "Available Token is " + 0.ToString().Trim();
                sensordatajson = null;
                StatClass.sensorjson = sensordatajson;
            }
        }
        catch (Exception)
        {
            label1.Text = "0 Token found please retry, Dont worry bina token bhi aap all logins bindass kar sakte hai from our server token.";
            Environment.SetEnvironmentVariable("avltoken", 0.ToString().Trim(), EnvironmentVariableTarget.Process);
            label2.Text = "Available Token is " + 0.ToString().Trim();
            sensordatajson = null;
            StatClass.sensorjson = sensordatajson;
        }
        button1.Visible = true;
    }

    private void button1_Click_OLD(object sender, EventArgs e)
    {
        label2.Visible = false;
        button1.Visible = false;
        label1.Visible = true;
        int num = 0;
        string empty = string.Empty;
        string text = "";
        int num2 = 0;
        string environmentVariable = Environment.GetEnvironmentVariable("clientID", EnvironmentVariableTarget.Process);
        Encryptsensor(environmentVariable);
        try
        {
            label1.Text = "Loggin in to the server..";
            num2++;
            text = "https://auth5.taslats.com/api/v1/post-username";
            string[] array = "https://auth5.taslats.com/sensor/##https://auth6.taslats.com/sensor/##https://auth6.taslats.com/sensor/##https://auth5.taslats.com/sensor/##https://auth6.taslats.com/sensor/".Split(new string[1] { "##" }, StringSplitOptions.None);
            if (array.Length != 0)
            {
                text = array[new Random().Next(array.Length - 1)].ToString();
            }
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(text + environmentVariable);
            httpWebRequest.ServerCertificateValidationCallback = smethod_1SS;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "application/json, text/plain, */*";
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64: x64) AppleWebKit/537.36 (KHTML, like Geck0) Chrome/131.0.0.0 Safari/537.36";
            httpWebRequest.Timeout = 160000;
            httpWebRequest.Referer = "taslats.com";
            httpWebRequest.ReadWriteTimeout = 160000;
            httpWebRequest.Method = "GET";
            using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
            empty = (sensordatajson = streamReader.ReadToEnd());
            try
            {
                string text2 = "";
                DataObject dataObject = new DataObject();
                if (empty.Contains("default-mobile"))
                {
                    label1.Text = "Please mobile me apk ko background se close kare, then open kare aur token generate kare fir yaha par check kare";
                    Environment.SetEnvironmentVariable("avltoken", "0".ToString().Trim(), EnvironmentVariableTarget.Process);
                    label2.Text = "Available Token is " + 0.ToString().Trim();
                    sensordatajson = null;
                    StatClass.sensorjson = sensordatajson;
                }
                if (!empty.Contains("[]"))
                {
                    List<Root_> list = JsonConvert.DeserializeObject<List<Root_>>(sensordatajson);
                    num = list.Count;
                    if (num > 0)
                    {
                        for (int i = 0; i <= num - 1; i++)
                        {
                            new Random().Next(list.Count);
                            text2 = Decryptsensor(list[i].data, "398a748baa05b5a5");
                            foreach (string datum in JsonConvert.DeserializeObject<Root>(text2).data)
                            {
                                dataObject.data.Add(datum.ToString());
                            }
                        }
                    }
                    if (text2.Contains("default-mobile"))
                    {
                        label1.Text = "Please mobile me apk ko background se close kare\r\nthen open kare aur token generate kare fir yaha par check kare";
                        Environment.SetEnvironmentVariable("avltoken", "0".ToString().Trim(), EnvironmentVariableTarget.Process);
                        label2.Text = "Available Token is " + 0.ToString().Trim();
                        sensordatajson = null;
                        StatClass.sensorjson = sensordatajson;
                        button1.Visible = true;
                        return;
                    }
                    int count = dataObject.data.Count;
                    label1.Text = $"Found {count} Token Assigned your Software. Please proceed with Pair login.";
                    Environment.SetEnvironmentVariable("avltoken", count.ToString().Trim(), EnvironmentVariableTarget.Process);
                    StatClass.sensorjson = JsonConvert.SerializeObject(dataObject, Formatting.Indented);
                    try
                    {
                        string sensorjson = StatClass.sensorjson;
                        string cacheDirectory = getCacheDirectory();
                        if (!Directory.Exists(cacheDirectory))
                        {
                            Directory.CreateDirectory(cacheDirectory);
                        }
                        string text3 = Path.Combine(cacheDirectory, "VALL.dll");
                        string text4 = "||||||||||";
                        try
                        {
                            File.AppendAllText(text3, sensorjson + text4);
                            Console.WriteLine("Sensor data appended to: " + text3);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error writing to file: " + ex.Message);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
            try
            {
                if (empty.Contains("[]"))
                {
                    Environment.SetEnvironmentVariable("avltoken", 0.ToString().Trim(), EnvironmentVariableTarget.Process);
                    label1.Text = "You have 0 Token agains this user Id please generate token again and click button again.\r\ndont worry you can do all logins APP , APP2 and Super from our server stored tokens";
                    sensordatajson = null;
                    StatClass.sensorjson = sensordatajson;
                }
                if (!empty.Contains("default-mobile"))
                {
                    if (empty.Contains("6,a"))
                    {
                        label2.Visible = true;
                        label2.Text = "Available Token is " + num.ToString().Trim() + " Please dont open more than " + num.ToString().Trim() + " Windows for all Forms";
                    }
                }
                else
                {
                    label1.Text = "Please mobile me apk ko background se close kare, then open kare aur token generate kare fir yaha par check kare";
                    Environment.SetEnvironmentVariable("avltoken", "0".ToString().Trim(), EnvironmentVariableTarget.Process);
                    label2.Text = "Available Token is " + 0.ToString().Trim();
                    sensordatajson = null;
                    StatClass.sensorjson = sensordatajson;
                }
            }
            catch (Exception)
            {
                label1.Text = "0 Token found please retry, dont worry you can do all logins APP , APP2 and Super from our server stored tokens.";
                Environment.SetEnvironmentVariable("avltoken", 0.ToString().Trim(), EnvironmentVariableTarget.Process);
                label2.Text = "Available Token is " + 0.ToString().Trim();
                sensordatajson = null;
                StatClass.sensorjson = sensordatajson;
            }
        }
        catch (Exception)
        {
            label1.Text = "0 Token found please retry, Dont worry bina token bhi aap all logins bindass kar sakte hai from our server token.";
            Environment.SetEnvironmentVariable("avltoken", 0.ToString().Trim(), EnvironmentVariableTarget.Process);
            label2.Text = "Available Token is " + 0.ToString().Trim();
            sensordatajson = null;
            StatClass.sensorjson = sensordatajson;
        }
        button1.Visible = true;
    }

    private void button1_Click_OLYD(object sender, EventArgs e)
    {
        label2.Visible = false;
        button1.Visible = false;
        label1.Visible = true;
        int num = 0;
        string empty = string.Empty;
        string text = "";
        int num2 = 0;
        string environmentVariable = Environment.GetEnvironmentVariable("clientID", EnvironmentVariableTarget.Process);
        string value = Encryptsensor(environmentVariable);
        try
        {
            label1.Text = "Loggin in to the server..";
            num2++;
            text = "http://sensortokendata.com:8080/api/v1/get-sensor-by-username?username=ADMIN0001";
            if ("https://auth5.taslats.com/api/v1/post-username##https://auth6.taslats.com/api/v1/post-username##https://auth6.taslats.com/api/v1/post-username##https://auth5.taslats.com/api/v1/post-username##https://auth6.taslats.com/api/v1/post-username".Split(new string[1] { "##" }, StringSplitOptions.None).Length != 0)
            {
                new Random();
            }
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(text);
            httpWebRequest.ServerCertificateValidationCallback = smethod_1SS;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "application/json, text/plain, */*";
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64: x64) AppleWebKit/537.36 (KHTML, like Geck0) Chrome/131.0.0.0 Safari/537.36";
            httpWebRequest.Timeout = 160000;
            httpWebRequest.Headers.Add("api-key", "da3016ac-29c5-4d0c-8311-f7908a20e7a9");
            httpWebRequest.Headers.Add("username", value);
            httpWebRequest.ReadWriteTimeout = 160000;
            _ = "{\r\n\"usernamee\": \"" + environmentVariable + "\"\r\n}";
            httpWebRequest.Method = "GET";
            using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
            empty = (sensordatajson = streamReader.ReadToEnd());
            try
            {
                if (!empty.Contains("[]"))
                {
                    empty = (sensordatajson = Decryptsensor(empty, "398a748baa05b5a5"));
                    List<Root_> list = JsonConvert.DeserializeObject<List<Root_>>(empty);
                    Random random = new Random();
                    int count = list.Count;
                    num = count;
                    label1.Text = "Found " + count.ToString().Trim() + "Token Assigned your Software Plz proceed with Pair login";
                    Environment.SetEnvironmentVariable("avltoken", count.ToString().Trim(), EnvironmentVariableTarget.Process);
                    Root_ root_ = list[random.Next(list.Count)];
                    StatClass.sensorjson = null;
                    StatClass.sensorjson = sensordatajson;
                    _ = root_.data;
                }
            }
            catch
            {
            }
            try
            {
                if (empty.Contains("[]"))
                {
                    Environment.SetEnvironmentVariable("avltoken", 0.ToString().Trim(), EnvironmentVariableTarget.Process);
                    label1.Text = "You have 0 Token agains this user Id please generate token again and click button again.\r\ndont worry you can do all logins APP , APP2 and Super from our server stored tokens";
                    sensordatajson = null;
                    StatClass.sensorjson = sensordatajson;
                }
                if (!empty.Contains("default-mobile"))
                {
                    if (empty.Contains("6,a"))
                    {
                        label2.Visible = true;
                        label2.Text = "Available Token is " + num.ToString().Trim() + " Please dont open more than " + num.ToString().Trim() + " Windows for all Forms";
                    }
                }
                else
                {
                    label1.Text = "Please mobile me apk ko background se close kare, then open kare aur token generate kare fir yaha par check kare";
                    Environment.SetEnvironmentVariable("avltoken", "0".ToString().Trim(), EnvironmentVariableTarget.Process);
                    label2.Text = "Available Token is " + 0.ToString().Trim();
                    sensordatajson = null;
                    StatClass.sensorjson = sensordatajson;
                }
            }
            catch (Exception)
            {
                label1.Text = "0 Token found please retry, dont worry you can do all logins APP , APP2 and Super from our server stored tokens.";
                Environment.SetEnvironmentVariable("avltoken", 0.ToString().Trim(), EnvironmentVariableTarget.Process);
                label2.Text = "Available Token is " + 0.ToString().Trim();
                sensordatajson = null;
                StatClass.sensorjson = sensordatajson;
            }
        }
        catch (Exception)
        {
            Environment.SetEnvironmentVariable("avltoken", 0.ToString().Trim(), EnvironmentVariableTarget.Process);
            sensordatajson = null;
        }
        button1.Visible = true;
    }

    public static string Encryptsensor(string plainText)
    {
        try
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();
            byte[] iV = aes.IV;
            using ICryptoTransform cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            byte[] array = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
            byte[] array2 = new byte[iV.Length + array.Length];
            Array.Copy(iV, 0, array2, 0, iV.Length);
            Array.Copy(array, 0, array2, iV.Length, array.Length);
            return Convert.ToBase64String(array2);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during encryption: " + ex.Message);
            return null;
        }
    }

    public static bool smethod_1SS(object A_0, X509Certificate A_1, X509Chain A_2, SslPolicyErrors A_3)
    {
        if (!Program.BypassAnalizer)
        {
            try
            {
                if (A_0 is HttpWebRequest httpWebRequest && httpWebRequest.Address.ToString().ToLower().Contains("https://www.google.co.in") && !A_1.Issuer.StartsWith("CN=Google"))
                {
                    MessageBox.Show(new Form
                    {
                        TopMost = true
                    }, "Security Failed for your S/w You tried to use cracking tools. Your ID has been logged.\r\n\r\nPlease format your PC or use new PC", "Security", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    try
                    {
                        RegistryKey registryKey = Registry.LocalMachine.CreateSubKey("Software\\Wow6432Node\\Default");
                        registryKey.SetValue("Found", "Defaulter");
                        registryKey.Close();
                    }
                    catch
                    {
                    }
                    Application.Exit();
                    return false;
                }
                if (A_1.Issuer.ToLower().Contains("httpanalyze") || A_1.Issuer.ToLower().Contains("fiddler") || A_1.Issuer.ToLower().Contains("commview") || A_1.Issuer.ToLower().Contains("debugger") || A_1.Issuer.ToLower().Contains("debug") || A_1.Issuer.ToLower().Contains("sniffer") || A_1.Issuer.ToUpper().Contains("IEINSPECTOR"))
                {
                    MessageBox.Show(new Form
                    {
                        TopMost = true
                    }, "Security Failed for S/w. Your ID has been logged.\r\n\r\nPlease contact Your Admin.", "Security", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    Application.Exit();
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }
        return true;
    }

    private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
    {
        int num = 0;
        string empty = string.Empty;
        string text = "";
        int num2 = 0;
        string environmentVariable = Environment.GetEnvironmentVariable("clientID", EnvironmentVariableTarget.Process);
        string value = Encryptsensor(environmentVariable);
        try
        {
            label1.Text = "Loggin in to the server..";
            num2++;
            text = "https://auth5.taslats.com/api/v1/post-username";
            string[] array = "https://auth5.taslats.com/api/v1/post-username##https://auth6.taslats.com/api/v1/post-username##https://auth6.taslats.com/api/v1/post-username".Split(new string[1] { "##" }, StringSplitOptions.None);
            if (array.Length != 0)
            {
                text = array[new Random().Next(array.Length - 1)].ToString();
            }
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(text);
            httpWebRequest.ServerCertificateValidationCallback = smethod_1SS;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Accept = "application/json, text/plain, */*";
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64: x64) AppleWebKit/537.36 (KHTML, like Geck0) Chrome/131.0.0.0 Safari/537.36";
            httpWebRequest.Timeout = 160000;
            httpWebRequest.Headers.Add("username", value);
            httpWebRequest.ReadWriteTimeout = 160000;
            string value2 = "{\r\n\"usernamee\": \"" + environmentVariable + "\"\r\n}";
            httpWebRequest.Method = "POST";
            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(value2);
            }
            using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
            empty = (sensordatajson = streamReader.ReadToEnd());
            try
            {
                if (!empty.Contains("[]"))
                {
                    List<Root_> list = JsonConvert.DeserializeObject<List<Root_>>(sensordatajson);
                    Random random = new Random();
                    int count = list.Count;
                    num = count;
                    label1.Text = "Found " + count.ToString().Trim() + "Token Assigning it your Software";
                    Environment.SetEnvironmentVariable("avltoken", count.ToString().Trim(), EnvironmentVariableTarget.Process);
                    Root_ root_ = list[random.Next(list.Count)];
                    StatClass.sensorjson = null;
                    StatClass.sensorjson = sensordatajson;
                    empty = Decryptsensor(root_.data, "398a748baa05b5a5");
                }
            }
            catch
            {
            }
            try
            {
                if (empty.Contains("[]"))
                {
                    Environment.SetEnvironmentVariable("avltoken", 0.ToString().Trim(), EnvironmentVariableTarget.Process);
                    label1.Text = "You have 0 Token agains this user Id please generate token again and click button again.\r\ndont worry you can do all logins APP , APP2 and Super from our server stored tokens";
                    sensordatajson = null;
                    StatClass.sensorjson = sensordatajson;
                }
                if (empty.Contains("default-mobile"))
                {
                    label1.Text = "Please close app from background regenrate token and click check token again";
                    Environment.SetEnvironmentVariable("avltoken", "0".ToString().Trim(), EnvironmentVariableTarget.Process);
                    label2.Text = "Available Token is " + 0.ToString().Trim();
                    sensordatajson = null;
                    StatClass.sensorjson = sensordatajson;
                }
                else if (empty.Contains("6,a"))
                {
                    label2.Visible = true;
                    label2.Text = "Available Token is " + num.ToString().Trim() + " Please dont open more than " + num.ToString().Trim() + " Windows for all fomrs";
                }
            }
            catch (Exception)
            {
                label1.Text = "0 Token found dont worry you can do all logins APP , APP2 and Super from our server stored tokens.";
                Environment.SetEnvironmentVariable("avltoken", 0.ToString().Trim(), EnvironmentVariableTarget.Process);
                label2.Text = "Available Token is " + 0.ToString().Trim();
                sensordatajson = null;
                StatClass.sensorjson = sensordatajson;
            }
        }
        catch (Exception)
        {
            Environment.SetEnvironmentVariable("avltoken", 0.ToString().Trim(), EnvironmentVariableTarget.Process);
            sensordatajson = null;
        }
    }

    private void label2_Click(object sender, EventArgs e)
    {
    }

    private void button2_Click(object sender, EventArgs e)
    {
        Process.Start(Environment.GetEnvironmentVariable("awsurl", EnvironmentVariableTarget.Process) + "IRCTC_CRIS.apk");
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start(Environment.GetEnvironmentVariable("https://tokenserverapk.s3.eu-north-1.amazonaws.com/Irctc_Token_4.2.56.apk"));
    }

    private void Token_count_Load(object sender, EventArgs e)
    {
        try
        {
            string cacheDirectory = getCacheDirectory();
            string path = Path.Combine(cacheDirectory, "VALL.dll");
            Directory.CreateDirectory(cacheDirectory);
            if (!File.Exists("cleartoken"))
            {
                File.WriteAllText(path, "");
                File.WriteAllText("cleartoken", "clear");
            }
            if (File.Exists(path))
            {
                string[] array = File.ReadAllLines(path);
                SetTokenCount(array.Length, 1);
            }
        }
        catch
        {
        }
    }

    private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start(Environment.GetEnvironmentVariable("awsurl", EnvironmentVariableTarget.Process) + "Chocolate_Fudge_39.apk");
    }

    private void label5_Click(object sender, EventArgs e)
    {
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(APP_FRAMEWORK.Token_count));
        this.button1 = new System.Windows.Forms.Button();
        this.label1 = new System.Windows.Forms.Label();
        this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
        this.label2 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.label4 = new System.Windows.Forms.Label();
        this.linkLabel1 = new System.Windows.Forms.LinkLabel();
        this.linkLabel2 = new System.Windows.Forms.LinkLabel();
        this.label5 = new System.Windows.Forms.Label();
        this.button2 = new System.Windows.Forms.Button();
        this.linkLabel3 = new System.Windows.Forms.LinkLabel();
        this.button3 = new System.Windows.Forms.Button();
        this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
        this.bunifuImageButton1 = new Bunifu.Framework.UI.BunifuImageButton();
        this.bunifuImageButton2 = new Bunifu.Framework.UI.BunifuImageButton();
        this.panelHeader = new System.Windows.Forms.Panel();
        this.bunifuImageButton3 = new Bunifu.Framework.UI.BunifuImageButton();
        this.lblTitle = new System.Windows.Forms.Label();
        this.lblTotalTokens = new System.Windows.Forms.Label();
        this.lblSubTitle = new System.Windows.Forms.Label();
        this.grpServer1 = new System.Windows.Forms.GroupBox();
        this.lblTokens1 = new System.Windows.Forms.Label();
        this.btnDownloadApk = new System.Windows.Forms.Button();
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.button5 = new System.Windows.Forms.Button();
        this.lblTokens2 = new System.Windows.Forms.Label();
        this.grpApk = new System.Windows.Forms.GroupBox();
        this.grpInfo = new System.Windows.Forms.GroupBox();
        this.lblInfo = new System.Windows.Forms.Label();
        this.panel1 = new System.Windows.Forms.Panel();
        this.label8 = new System.Windows.Forms.Label();
        this.groupBox2 = new System.Windows.Forms.GroupBox();
        this.label6 = new System.Windows.Forms.Label();
        this.label7 = new System.Windows.Forms.Label();
        this.groupBox3 = new System.Windows.Forms.GroupBox();
        this.button6 = new System.Windows.Forms.Button();
        this.button4 = new System.Windows.Forms.Button();
        this.bunifuImageButton4 = new Bunifu.Framework.UI.BunifuImageButton();
        ((System.ComponentModel.ISupportInitialize)this.btnClose).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.bunifuImageButton1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)this.bunifuImageButton2).BeginInit();
        this.panelHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.bunifuImageButton3).BeginInit();
        this.grpServer1.SuspendLayout();
        this.groupBox1.SuspendLayout();
        this.grpApk.SuspendLayout();
        this.grpInfo.SuspendLayout();
        this.panel1.SuspendLayout();
        this.groupBox2.SuspendLayout();
        this.groupBox3.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.bunifuImageButton4).BeginInit();
        base.SuspendLayout();
        this.button1.BackColor = System.Drawing.Color.Chocolate;
        this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.button1.Font = new System.Drawing.Font("Microsoft Tai Le", 7.8f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.button1.ForeColor = System.Drawing.Color.Snow;
        this.button1.Location = new System.Drawing.Point(17, 246);
        this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(263, 30);
        this.button1.TabIndex = 0;
        this.button1.Text = "Find Tokens on Server 1";
        this.button1.UseVisualStyleBackColor = false;
        this.button1.Click += new System.EventHandler(button1_Click);
        this.label1.AutoSize = true;
        this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.label1.Location = new System.Drawing.Point(985, 654);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(291, 30);
        this.label1.TabIndex = 1;
        this.label1.Text = "Fetching Token from Server";
        this.label1.Visible = false;
        this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
        this.label2.AutoSize = true;
        this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.label2.Font = new System.Drawing.Font("Microsoft Tai Le", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.label2.Location = new System.Drawing.Point(985, 637);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(758, 30);
        this.label2.TabIndex = 2;
        this.label2.Text = "Fetching token from server… please avoid opening more than 5 windows";
        this.label2.Visible = false;
        this.label2.Click += new System.EventHandler(label2_Click);
        this.label3.AutoSize = true;
        this.label3.BackColor = System.Drawing.Color.FromArgb(31, 19, 43);
        this.label3.Font = new System.Drawing.Font("Microsoft Tai Le", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.label3.Location = new System.Drawing.Point(6, 73);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(196, 19);
        this.label3.TabIndex = 3;
        this.label3.Text = "USER ID: User1234567890";
        this.label4.AutoSize = true;
        this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.label4.Font = new System.Drawing.Font("Microsoft Tai Le", 10.2f, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
        this.label4.Location = new System.Drawing.Point(772, 626);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(864, 44);
        this.label4.TabIndex = 4;
        this.label4.Text = "Plz Click Chk Token button after you generated token from the apk.If token not comes in one click try 1-2 times \r\nand wait for response.Token show jane par dobara check na kare direct form login kare.";
        this.label4.Visible = false;
        this.linkLabel1.AutoSize = true;
        this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.linkLabel1.Location = new System.Drawing.Point(1027, 680);
        this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        this.linkLabel1.Name = "linkLabel1";
        this.linkLabel1.Size = new System.Drawing.Size(308, 25);
        this.linkLabel1.TabIndex = 1216;
        this.linkLabel1.TabStop = true;
        this.linkLabel1.Text = "Download Normal APK v4.2.56";
        this.linkLabel1.Visible = false;
        this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel1_LinkClicked);
        this.linkLabel2.AutoSize = true;
        this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.linkLabel2.Location = new System.Drawing.Point(801, 619);
        this.linkLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        this.linkLabel2.Name = "linkLabel2";
        this.linkLabel2.Size = new System.Drawing.Size(582, 25);
        this.linkLabel2.TabIndex = 1217;
        this.linkLabel2.TabStop = true;
        this.linkLabel2.Text = "APK Number :- 2 Auto clear data on close from background";
        this.linkLabel2.Visible = false;
        this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel2_LinkClicked);
        this.label5.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
        this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.8f, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
        this.label5.Location = new System.Drawing.Point(424, 151);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(1005, 126);
        this.label5.TabIndex = 1218;
        this.label5.Visible = false;
        this.label5.Click += new System.EventHandler(label5_Click);
        this.button2.BackColor = System.Drawing.Color.Chocolate;
        this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.button2.Font = new System.Drawing.Font("Microsoft Tai Le", 7.8f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.button2.ForeColor = System.Drawing.Color.Snow;
        this.button2.Location = new System.Drawing.Point(853, 104);
        this.button2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(253, 30);
        this.button2.TabIndex = 1219;
        this.button2.Text = "Find Tokens on Server 2";
        this.button2.UseVisualStyleBackColor = false;
        this.button2.Visible = false;
        this.button2.Click += new System.EventHandler(button2_Click_1);
        this.linkLabel3.AutoSize = true;
        this.linkLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.linkLabel3.Location = new System.Drawing.Point(978, 724);
        this.linkLabel3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
        this.linkLabel3.Name = "linkLabel3";
        this.linkLabel3.Size = new System.Drawing.Size(408, 25);
        this.linkLabel3.TabIndex = 1220;
        this.linkLabel3.TabStop = true;
        this.linkLabel3.Text = "Download Install Official APK for server 2";
        this.linkLabel3.Visible = false;
        this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(linkLabel3_LinkClicked);
        this.button3.BackColor = System.Drawing.Color.FromArgb(223, 230, 232);
        this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.button3.Font = new System.Drawing.Font("Microsoft Tai Le", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.button3.ForeColor = System.Drawing.Color.DarkSlateGray;
        this.button3.Location = new System.Drawing.Point(942, 2);
        this.button3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.button3.Name = "button3";
        this.button3.Size = new System.Drawing.Size(179, 27);
        this.button3.TabIndex = 1221;
        this.button3.Text = "Clear Old Tokens";
        this.button3.UseVisualStyleBackColor = false;
        this.button3.Click += new System.EventHandler(button3_Click);
        this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
        this.btnClose.BackColor = System.Drawing.Color.Transparent;
        this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
        //this.btnClose.Image = (System.Drawing.Image)resources.GetObject("btnClose.Image");
        this.btnClose.ImageActive = null;
        this.btnClose.Location = new System.Drawing.Point(55, 24);
        this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(178, 160);
        this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.btnClose.TabIndex = 1222;
        this.btnClose.TabStop = false;
        this.btnClose.Zoom = 10;
        this.bunifuImageButton1.Anchor = System.Windows.Forms.AnchorStyles.Right;
        this.bunifuImageButton1.BackColor = System.Drawing.Color.Transparent;
        this.bunifuImageButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        this.bunifuImageButton1.Cursor = System.Windows.Forms.Cursors.Hand;
        //this.bunifuImageButton1.Image = (System.Drawing.Image)resources.GetObject("bunifuImageButton1.Image");
        this.bunifuImageButton1.ImageActive = null;
        this.bunifuImageButton1.Location = new System.Drawing.Point(60, 22);
        this.bunifuImageButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.bunifuImageButton1.Name = "bunifuImageButton1";
        this.bunifuImageButton1.Size = new System.Drawing.Size(178, 160);
        this.bunifuImageButton1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.bunifuImageButton1.TabIndex = 1223;
        this.bunifuImageButton1.TabStop = false;
        this.bunifuImageButton1.Zoom = 10;
        this.bunifuImageButton2.Anchor = System.Windows.Forms.AnchorStyles.Right;
        this.bunifuImageButton2.BackColor = System.Drawing.Color.Transparent;
        this.bunifuImageButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        this.bunifuImageButton2.Cursor = System.Windows.Forms.Cursors.Hand;
        //this.bunifuImageButton2.Image = (System.Drawing.Image)resources.GetObject("bunifuImageButton2.Image");
        this.bunifuImageButton2.ImageActive = null;
        this.bunifuImageButton2.Location = new System.Drawing.Point(9, 19);
        this.bunifuImageButton2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.bunifuImageButton2.Name = "bunifuImageButton2";
        this.bunifuImageButton2.Size = new System.Drawing.Size(147, 108);
        this.bunifuImageButton2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.bunifuImageButton2.TabIndex = 1225;
        this.bunifuImageButton2.TabStop = false;
        this.bunifuImageButton2.Zoom = 10;
        this.panelHeader.BackColor = System.Drawing.Color.FromArgb(31, 19, 43);
        this.panelHeader.Controls.Add(this.bunifuImageButton3);
        this.panelHeader.Controls.Add(this.lblTitle);
        this.panelHeader.Controls.Add(this.lblTotalTokens);
        this.panelHeader.Controls.Add(this.lblSubTitle);
        this.panelHeader.Location = new System.Drawing.Point(-1, 2);
        this.panelHeader.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.panelHeader.Name = "panelHeader";
        this.panelHeader.Size = new System.Drawing.Size(1143, 63);
        this.panelHeader.TabIndex = 1227;
        this.bunifuImageButton3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        this.bunifuImageButton3.BackColor = System.Drawing.Color.Transparent;
        this.bunifuImageButton3.Cursor = System.Windows.Forms.Cursors.Hand;
        //		this.bunifuImageButton3.Image = (System.Drawing.Bitmap)new System.Resources.ResourceManager("APP_FRAMEWORK.frmAddID", typeof(APP_FRAMEWORK.Token_count).Assembly).GetObject("btnClose.Image");
        this.bunifuImageButton3.ImageActive = null;
        this.bunifuImageButton3.Location = new System.Drawing.Point(1099, 5);
        this.bunifuImageButton3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
        this.bunifuImageButton3.Name = "bunifuImageButton3";
        this.bunifuImageButton3.Size = new System.Drawing.Size(34, 24);
        this.bunifuImageButton3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.bunifuImageButton3.TabIndex = 1222;
        this.bunifuImageButton3.TabStop = false;
        this.bunifuImageButton3.Zoom = 10;
        this.bunifuImageButton3.Click += new System.EventHandler(bunifuImageButton3_Click);
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16f, System.Drawing.FontStyle.Bold);
        this.lblTitle.Location = new System.Drawing.Point(3, 0);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(328, 37);
        this.lblTitle.TabIndex = 4;
        this.lblTitle.Text = "TOKEN CONTROL PANEL";
        this.lblTotalTokens.AutoSize = true;
        this.lblTotalTokens.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.lblTotalTokens.Location = new System.Drawing.Point(838, 3);
        this.lblTotalTokens.Name = "lblTotalTokens";
        this.lblTotalTokens.Size = new System.Drawing.Size(138, 28);
        this.lblTotalTokens.TabIndex = 6;
        this.lblTotalTokens.Text = "Total Tokens: 0";
        this.lblSubTitle.AutoSize = true;
        this.lblSubTitle.Location = new System.Drawing.Point(12, 39);
        this.lblSubTitle.Name = "lblSubTitle";
        this.lblSubTitle.Size = new System.Drawing.Size(180, 16);
        this.lblSubTitle.TabIndex = 5;
        this.lblSubTitle.Text = "Live APK Testing Dashboard";
        this.grpServer1.BackColor = System.Drawing.Color.FromArgb(31, 19, 43);
        this.grpServer1.Controls.Add(this.lblTokens1);
        this.grpServer1.Controls.Add(this.btnDownloadApk);
        this.grpServer1.Controls.Add(this.btnClose);
        this.grpServer1.Controls.Add(this.button1);
        this.grpServer1.Location = new System.Drawing.Point(406, 92);
        this.grpServer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.grpServer1.Name = "grpServer1";
        this.grpServer1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.grpServer1.Size = new System.Drawing.Size(300, 282);
        this.grpServer1.TabIndex = 1228;
        this.grpServer1.TabStop = false;
        this.grpServer1.Text = "SERVER 1";
        this.lblTokens1.AutoSize = true;
        this.lblTokens1.Font = new System.Drawing.Font("Segoe UI", 11f, System.Drawing.FontStyle.Bold);
        this.lblTokens1.Location = new System.Drawing.Point(97, 186);
        this.lblTokens1.Name = "lblTokens1";
        this.lblTokens1.Size = new System.Drawing.Size(95, 25);
        this.lblTokens1.TabIndex = 1;
        this.lblTokens1.Text = "Tokens: 0";
        this.btnDownloadApk.BackColor = System.Drawing.Color.DarkViolet;
        this.btnDownloadApk.ForeColor = System.Drawing.Color.FromArgb(31, 19, 43);
        this.btnDownloadApk.Location = new System.Drawing.Point(17, 212);
        this.btnDownloadApk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.btnDownloadApk.Name = "btnDownloadApk";
        this.btnDownloadApk.Size = new System.Drawing.Size(263, 30);
        this.btnDownloadApk.TabIndex = 1;
        this.btnDownloadApk.Text = "Download Normal APK v4.3.14\r\n";
        this.btnDownloadApk.UseVisualStyleBackColor = false;
        this.btnDownloadApk.Click += new System.EventHandler(btnDownloadApk_Click);
        this.groupBox1.BackColor = System.Drawing.Color.FromArgb(31, 19, 43);
        this.groupBox1.Controls.Add(this.button5);
        this.groupBox1.Controls.Add(this.lblTokens2);
        this.groupBox1.Controls.Add(this.bunifuImageButton1);
        this.groupBox1.Location = new System.Drawing.Point(842, 95);
        this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.groupBox1.Size = new System.Drawing.Size(290, 282);
        this.groupBox1.TabIndex = 1229;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "APP login setup download hear";
        this.button5.BackColor = System.Drawing.Color.DarkViolet;
        this.button5.ForeColor = System.Drawing.Color.FromArgb(31, 19, 43);
        this.button5.Location = new System.Drawing.Point(17, 212);
        this.button5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.button5.Name = "button5";
        this.button5.Size = new System.Drawing.Size(253, 61);
        this.button5.TabIndex = 1226;
        this.button5.Text = "APP login software download";
        this.button5.UseVisualStyleBackColor = false;
        this.button5.Click += new System.EventHandler(button5_Click);
        this.lblTokens2.AutoSize = true;
        this.lblTokens2.Font = new System.Drawing.Font("Segoe UI", 11f, System.Drawing.FontStyle.Bold);
        this.lblTokens2.Location = new System.Drawing.Point(98, 182);
        this.lblTokens2.Name = "lblTokens2";
        this.lblTokens2.Size = new System.Drawing.Size(95, 25);
        this.lblTokens2.TabIndex = 1;
        this.lblTokens2.Text = "Tokens: 0";
        this.lblTokens2.Visible = false;
        this.grpApk.Controls.Add(this.bunifuImageButton2);
        this.grpApk.Location = new System.Drawing.Point(37, 667);
        this.grpApk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.grpApk.Name = "grpApk";
        this.grpApk.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.grpApk.Size = new System.Drawing.Size(446, 144);
        this.grpApk.TabIndex = 1230;
        this.grpApk.TabStop = false;
        this.grpApk.Text = "OFFICIAL TESTING APK";
        this.grpInfo.Controls.Add(this.lblInfo);
        this.grpInfo.Controls.Add(this.label5);
        this.grpInfo.Location = new System.Drawing.Point(489, 674);
        this.grpInfo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.grpInfo.Name = "grpInfo";
        this.grpInfo.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.grpInfo.Size = new System.Drawing.Size(427, 230);
        this.grpInfo.TabIndex = 1231;
        this.grpInfo.TabStop = false;
        this.grpInfo.Text = "IMPORTANT INFORMATION";
        this.lblInfo.Location = new System.Drawing.Point(8, 25);
        this.lblInfo.Name = "lblInfo";
        this.lblInfo.Size = new System.Drawing.Size(391, 150);
        this.lblInfo.TabIndex = 0;
        //this.lblInfo.Text = resources.GetString("lblInfo.Text");
        this.panel1.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
        this.panel1.Controls.Add(this.label8);
        this.panel1.Controls.Add(this.button3);
        this.panel1.Controls.Add(this.button2);
        this.panel1.Font = new System.Drawing.Font("Microsoft Tai Le", 7.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.panel1.Location = new System.Drawing.Point(6, 385);
        this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.panel1.Name = "panel1";
        this.panel1.Size = new System.Drawing.Size(1126, 147);
        this.panel1.TabIndex = 1232;
        this.label8.AutoSize = true;
        this.label8.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.label8.ForeColor = System.Drawing.Color.FromArgb(128, 64, 0);
        this.label8.Location = new System.Drawing.Point(6, 16);
        this.label8.Name = "label8";
        this.label8.Size = new System.Drawing.Size(536, 100);
        this.label8.TabIndex = 2;
        //this.label8.Text = resources.GetString("label8.Text");
        this.groupBox2.Controls.Add(this.label6);
        this.groupBox2.Controls.Add(this.label7);
        this.groupBox2.Location = new System.Drawing.Point(37, 825);
        this.groupBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.groupBox2.Name = "groupBox2";
        this.groupBox2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.groupBox2.Size = new System.Drawing.Size(374, 81);
        this.groupBox2.TabIndex = 1232;
        this.groupBox2.TabStop = false;
        this.groupBox2.Text = "IMPORTANT INFORMATION";
        this.label6.Location = new System.Drawing.Point(8, 25);
        this.label6.Name = "label6";
        this.label6.Size = new System.Drawing.Size(355, 54);
        this.label6.TabIndex = 0;
        this.label6.Text = "• Yeh APK sirf testing ke liye diya gaya hai.\r\n• Agar aap isko properly use karte hain, to IP block issue kam hoga.";
        this.label7.BackColor = System.Drawing.Color.FromArgb(255, 255, 192);
        this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.label7.Font = new System.Drawing.Font("Microsoft YaHei UI", 10.8f, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
        this.label7.Location = new System.Drawing.Point(424, 151);
        this.label7.Name = "label7";
        this.label7.Size = new System.Drawing.Size(1005, 126);
        this.label7.TabIndex = 1218;
        this.label7.Visible = false;
        this.groupBox3.BackColor = System.Drawing.Color.FromArgb(31, 19, 43);
        this.groupBox3.Controls.Add(this.button6);
        this.groupBox3.Controls.Add(this.button4);
        this.groupBox3.Controls.Add(this.bunifuImageButton4);
        this.groupBox3.Location = new System.Drawing.Point(10, 103);
        this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.groupBox3.Name = "groupBox3";
        this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.groupBox3.Size = new System.Drawing.Size(267, 277);
        this.groupBox3.TabIndex = 1231;
        this.groupBox3.TabStop = false;
        this.groupBox3.Text = "OFFICIAL Premium APK v4.3.14";
        this.button6.BackColor = System.Drawing.Color.Crimson;
        this.button6.ForeColor = System.Drawing.Color.FromArgb(31, 19, 43);
        this.button6.Location = new System.Drawing.Point(0, 241);
        this.button6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.button6.Name = "button6";
        this.button6.Size = new System.Drawing.Size(257, 30);
        this.button6.TabIndex = 1226;
        this.button6.Text = "See Video, How to use APK";
        this.button6.UseVisualStyleBackColor = false;
        this.button6.Click += new System.EventHandler(button6_Click);
        this.button4.BackColor = System.Drawing.Color.FromArgb(155, 93, 229);
        this.button4.ForeColor = System.Drawing.Color.FromArgb(31, 19, 43);
        this.button4.Location = new System.Drawing.Point(1, 206);
        this.button4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.button4.Name = "button4";
        this.button4.Size = new System.Drawing.Size(257, 30);
        this.button4.TabIndex = 1;
        this.button4.Text = "Download Premium APK v4.3.14\r\n";
        this.button4.UseVisualStyleBackColor = false;
        this.button4.Click += new System.EventHandler(button4_Click);
        this.bunifuImageButton4.Anchor = System.Windows.Forms.AnchorStyles.Right;
        this.bunifuImageButton4.BackColor = System.Drawing.Color.Transparent;
        this.bunifuImageButton4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        this.bunifuImageButton4.Cursor = System.Windows.Forms.Cursors.Hand;
        //this.bunifuImageButton4.Image = (System.Drawing.Image)resources.GetObject("bunifuImageButton4.Image");
        this.bunifuImageButton4.ImageActive = null;
        this.bunifuImageButton4.Location = new System.Drawing.Point(32, 40);
        this.bunifuImageButton4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        this.bunifuImageButton4.Name = "bunifuImageButton4";
        this.bunifuImageButton4.Size = new System.Drawing.Size(188, 150);
        this.bunifuImageButton4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.bunifuImageButton4.TabIndex = 1225;
        this.bunifuImageButton4.TabStop = false;
        this.bunifuImageButton4.Zoom = 10;
        base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        base.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.BackColor = System.Drawing.Color.FromArgb(31, 19, 43);
        base.ClientSize = new System.Drawing.Size(1144, 542);
        base.ControlBox = false;
        base.Controls.Add(this.groupBox2);
        base.Controls.Add(this.groupBox3);
        base.Controls.Add(this.grpApk);
        base.Controls.Add(this.panel1);
        base.Controls.Add(this.grpInfo);
        base.Controls.Add(this.groupBox1);
        base.Controls.Add(this.linkLabel1);
        base.Controls.Add(this.grpServer1);
        base.Controls.Add(this.panelHeader);
        base.Controls.Add(this.label3);
        base.Controls.Add(this.linkLabel3);
        base.Controls.Add(this.linkLabel2);
        base.Controls.Add(this.label4);
        base.Controls.Add(this.label2);
        base.Controls.Add(this.label1);
        base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        base.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
        base.Name = "Token_count";
        base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Token Control Panel";
        base.Load += new System.EventHandler(Token_count_Load);
        ((System.ComponentModel.ISupportInitialize)this.btnClose).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.bunifuImageButton1).EndInit();
        ((System.ComponentModel.ISupportInitialize)this.bunifuImageButton2).EndInit();
        this.panelHeader.ResumeLayout(false);
        this.panelHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)this.bunifuImageButton3).EndInit();
        this.grpServer1.ResumeLayout(false);
        this.grpServer1.PerformLayout();
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        this.grpApk.ResumeLayout(false);
        this.grpInfo.ResumeLayout(false);
        this.panel1.ResumeLayout(false);
        this.panel1.PerformLayout();
        this.groupBox2.ResumeLayout(false);
        this.groupBox3.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)this.bunifuImageButton4).EndInit();
        base.ResumeLayout(false);
        base.PerformLayout();
    }

    private void button2_Click_1(object sender, EventArgs e)
    {
        button2.Visible = false;
        GetTokenFromServer(2);
        button2.Visible = true;
    }

    private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start(Environment.GetEnvironmentVariable("awsurl", EnvironmentVariableTarget.Process) + "IRCTC.apk");
    }

    private void button3_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Old Token deleted");
        string cacheDirectory = getCacheDirectory();
        string path = Path.Combine(cacheDirectory, "VALL.dll");
        if (File.Exists(path))
        {
            File.WriteAllText(path, "");
        }
        MessageBox.Show("Old Token deleted");
    }

    private void bunifuImageButton3_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void button4_Click(object sender, EventArgs e)
    {
        Process.Start("http://162.4.147.126/ByteNova.apk");
    }

    private void btnDownloadApk_Click(object sender, EventArgs e)
    {
        Process.Start("http://162.4.147.126/ByteNova.apk");
    }

    private void button5_Click(object sender, EventArgs e)
    {
        Process.Start("http://162.4.147.126/ByteNova.apk");
    }

    private void button6_Click(object sender, EventArgs e)
    {
        Process.Start("");
    }
}
