using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Booking;
using Bunifu.Framework.UI;
using BunifuAnimatorNS;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using NEXUS.Booking;
using NEXUS.Properties;

namespace APP_FRAMEWORK;

public partial class frmLogin : Form
{
    public static bool isPremium;

    public static string appversion;

    private int int_0;

    public string ErMsg;

    public bool bool_1;

    public bool bunifuButton1_Clik;

    public static string ENCkey;

    public static DateTime openDate;

    public static bool ShowMSG;

    public static string key;

    private IContainer C0AA0;

    private BunifuTransition bunifuTransition_0;

    private ToolTip toolTip_0;

    private BunifuImageButton B2AAA;

    internal CheckBox checkBox1;

    private BunifuFlatButton bunifuFlatButton1;

    internal CheckBox chkRemember;

    private System.Windows.Forms.Timer timer_0;

    private Panel panel2;

    private BunifuFlatButton bunifuFlatButton2;

    private Label label4;

    private IContainer components;

    internal TextBox txtLogin;

    private BunifuElipse bunifuElipse1;

    private Label B1AAA;

    private Button C2AA0;

    private Panel pnlFooter;

    private CheckBox chkLoginScreenShow;

    private BunifuFlatButton bunifuButton1;

    private Label label5;

    private Label label6;

    private BunifuSwitch bunifuToggleAppModeSwitch;

    private BunifuProgressBar bunifuProgressBar1;

    private BunifuSeparator bunifuSeparator2;

    private BunifuProgressBar bunifuProgressBar2;

    private TextBox InfoList2;

    private BunifuImageButton C1AA0;

    private BunifuSwitch bunifuToggleAutoLoginSwitch;

    private Label label3;

    private Label label2;

    public BunifuSwitch bunifuToggleRememberSwitch;

    private BunifuFlatButton btnLogin;

    private BunifuTextbox txtPassword;

    public BunifuTextbox txtUserName;

    private BunifuSeparator bunifuSeparator1;

    internal TextBox encP;

    private Label label7;

    private Label label1;
    private Panel byteNovaBrandPanel;
    private Image byteNovaLogoImage;
    private Panel panel1;
    private BunifuImageButton btnClose;
    private Label label8;
    internal WebProxy Proxy_BACKUP;
    private Label label50;
    private PictureBox byteNovaServerLoader;
    private System.Windows.Forms.Timer byteNovaServerLoaderTimer;
    private int byteNovaServerLoaderStep;
    private CheckBox byteNovaRestartOption;
    internal WebProxy Proxy_;

    [DllImport("kernel32.dll")]
    public static extern void ExitProcess(uint A_0);

    public static bool smethod_1(object A_0, X509Certificate A_1, X509Chain A_2, SslPolicyErrors A_3)
    {
        return true;
    }

    public frmLogin()
    {
        InitializeComponent();
        LoadByteNovaLogo();
        //ApplyByteNovaStyle();
        InitializeServerResponseLoader();
        InitializeByteNovaRestartOption();
        ApplyExpressTheme();           // naya light theme
        panel1.MouseDown += StatClass.Header_MouseDown;
        panel1.MouseMove += StatClass.Header_MouseMove;
        panel1.MouseUp += StatClass.Header_MouseUp;
        pnlFooter.Visible = false;
        mySettings.bool_1 = Program.mySettings_0.isRememberPassword;
        mySettings.bool_2 = Program.mySettings_0.isAutoLogin;
        mySettings.bool_3 = Program.mySettings_0.NonManagerDashborad;
    }


    // --------------------------------------------------------------------
    // ByteNova visual layer
    // This changes appearance only. Existing controls, names, events and
    // login functions are intentionally preserved.
    // --------------------------------------------------------------------
    private void LoadByteNovaLogo()
    {
        try
        {
            string[] candidates =
            {
                Path.Combine(Application.StartupPath, "express-logo.png"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "express-logo.png")
            };

            foreach (string file in candidates)
            {
                if (!File.Exists(file))
                    continue;

                byteNovaLogoImage = Image.FromFile(file);
                break;
            }
        }
        catch
        {
            // The form remains fully functional if the optional logo file
            // cannot be loaded; the paint routine uses a vector fallback.
            byteNovaLogoImage = null;
        }
    }

    private void ApplyByteNovaStyle()
    {
        SuspendLayout();

        // Compact ByteNova dashboard-style login.
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(450, 230);
        MinimumSize = new Size(450, 230);
        MaximumSize = new Size(450, 230);
        BackColor = Color.FromArgb(1, 10, 20);
        DoubleBuffered = true;
        Paint -= ByteNovaFormPaint;
        Paint += ByteNovaFormPaint;

        // ByteNova compact header - same standard used on frmAddTkt.
        panel1.Location = new Point(5, 5);
        panel1.Size = new Size(440, 25);
        panel1.BackColor = Color.FromArgb(3, 29, 59);
        panel1.Paint -= ByteNovaHeaderPaint;
        panel1.Paint += ByteNovaHeaderPaint;
        panel1.BringToFront();

        label50.Text = "";
        label50.Location = new Point(10, 1);
        label50.Size = new Size(300, 22);
        label50.AutoSize = false;
        label50.BackColor = Color.Transparent;
        label50.Paint -= ByteNovaTitlePaint;
        label50.Paint += ByteNovaTitlePaint;

        // frmMain-style compact Close button.
        btnClose.Location = new Point(411, 1);
        btnClose.Size = new Size(26, 22);
        btnClose.Paint -= ByteNovaLoginClosePaint;
        btnClose.BackColor = Color.Transparent;
        btnClose.BackgroundImage = null;
        btnClose.BackgroundImageLayout = ImageLayout.Zoom;
        btnClose.SizeMode = PictureBoxSizeMode.Zoom;
        btnClose.Zoom = 0;
        btnClose.BringToFront();

        // Left compact ByteNova brand card.
        if (byteNovaBrandPanel == null)
        {
            byteNovaBrandPanel = new Panel();
            byteNovaBrandPanel.Name = "byteNovaBrandPanel";
            byteNovaBrandPanel.Paint += ByteNovaBrandPaint;
            Controls.Add(byteNovaBrandPanel);
        }

        byteNovaBrandPanel.Location = new Point(7, 32);
        byteNovaBrandPanel.Size = new Size(175, 187);
        byteNovaBrandPanel.BackColor = Color.FromArgb(31, 19, 43);
        byteNovaBrandPanel.BringToFront();

        // Login card.
        panel2.Location = new Point(189, 32);
        panel2.Size = new Size(254, 187);
        panel2.BackColor = Color.FromArgb(31, 19, 43);
        panel2.Paint -= ByteNovaPanelPaint;
        panel2.Paint += ByteNovaPanelPaint;
        panel2.BringToFront();

        label4.Text = "SECURE LOGIN";
        label4.ForeColor = Color.FromArgb(198, 244, 50);
        label4.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        label4.Location = new Point(15, 14);
        label4.AutoSize = true;
        label4.BackColor = Color.Transparent;

        label1.Text = "KEY";
        label1.ForeColor = Color.White;
        label1.Font = new Font("Segoe UI Semibold", 7.5F, FontStyle.Bold);
        label1.Location = new Point(15, 55);
        label1.AutoSize = true;

        txtLogin.Location = new Point(70, 49);
        txtLogin.Size = new Size(169, 25);
        txtLogin.BackColor = Color.FromArgb(34, 21, 47);
        txtLogin.ForeColor = Color.White;
        txtLogin.BorderStyle = BorderStyle.FixedSingle;
        txtLogin.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold);
        txtLogin.TextAlign = HorizontalAlignment.Center;

        label7.Text = "PASSWORD";
        label7.ForeColor = Color.White;
        label7.Font = new Font("Segoe UI Semibold", 7F, FontStyle.Bold);
        label7.Location = new Point(15, 91);
        label7.AutoSize = true;

        encP.Location = new Point(70, 85);
        encP.Size = new Size(169, 25);
        encP.BackColor = Color.FromArgb(34, 21, 47);
        encP.ForeColor = Color.White;
        encP.BorderStyle = BorderStyle.FixedSingle;
        encP.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold);
        encP.TextAlign = HorizontalAlignment.Center;

        // ByteNova UI: password option is intentionally hidden.
        label7.Visible = false;
        encP.Visible = false;

        chkRemember.Location = new Point(15, 130);
        chkRemember.AutoSize = true;
        chkRemember.ForeColor = Color.FromArgb(190, 220, 235);
        chkRemember.BackColor = Color.Transparent;
        chkRemember.Font = new Font("Segoe UI", 7F, FontStyle.Regular);
        chkRemember.Visible = false;

        // EXIT and SUBMIT compact cards.
        bunifuFlatButton2.Location = new Point(24, 148);
        bunifuFlatButton2.Size = new Size(80, 27);
        bunifuFlatButton2.BorderRadius = 7;
        bunifuFlatButton2.Normalcolor = Color.FromArgb(70, 20, 40);
        bunifuFlatButton2.Activecolor = Color.FromArgb(145, 40, 70);
        bunifuFlatButton2.OnHovercolor = Color.FromArgb(115, 30, 55);
        bunifuFlatButton2.OnHoverTextColor = Color.White;
        bunifuFlatButton2.Textcolor = Color.White;
        bunifuFlatButton2.TextFont = new Font("Segoe UI Semibold", 7.5F, FontStyle.Bold);
        bunifuFlatButton2.TextAlign = ContentAlignment.MiddleCenter;

        bunifuFlatButton1.Location = new Point(111, 148);
        bunifuFlatButton1.Size = new Size(128, 27);
        bunifuFlatButton1.BorderRadius = 7;
        bunifuFlatButton1.Normalcolor = Color.FromArgb(155, 93, 229);
        bunifuFlatButton1.Activecolor = Color.FromArgb(180, 120, 245);
        bunifuFlatButton1.OnHovercolor = Color.FromArgb(165, 105, 235);
        bunifuFlatButton1.OnHoverTextColor = Color.White;
        bunifuFlatButton1.Textcolor = Color.White;
        bunifuFlatButton1.TextFont = new Font("Segoe UI Semibold", 7.5F, FontStyle.Bold);
        bunifuFlatButton1.TextAlign = ContentAlignment.MiddleCenter;
        //bunifuFlatButton1.MouseDown -= ByteNovaSubmitMouseDown;
        //bunifuFlatButton1.MouseDown += ByteNovaSubmitMouseDown;

        // Keep original secondary controls/events, only hide unused UI.
        checkBox1.Visible = false;
        B1AAA.Visible = false;
        C2AA0.Visible = false;
        pnlFooter.Visible = false;
        chkLoginScreenShow.Visible = false;
        bunifuButton1.Visible = false;
        label5.Visible = false;
        label6.Visible = false;
        bunifuToggleAppModeSwitch.Visible = false;
        bunifuProgressBar1.Visible = false;
        bunifuSeparator2.Visible = false;
        bunifuProgressBar2.Visible = false;
        InfoList2.Visible = false;
        C1AA0.Visible = false;
        bunifuToggleAutoLoginSwitch.Visible = false;
        label3.Visible = false;
        label2.Visible = false;
        bunifuToggleRememberSwitch.Visible = false;
        btnLogin.Visible = false;
        txtPassword.Visible = false;
        txtUserName.Visible = false;
        bunifuSeparator1.Visible = false;
        label8.Visible = false;

        panel1.BringToFront();
        panel2.BringToFront();
        btnClose.BringToFront();

        ResumeLayout(false);
        PerformLayout();
        Invalidate();
    }

    private void ByteNovaFormPaint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(1, 1, ClientSize.Width - 3, ClientSize.Height - 3);
        using (Pen cyan = new Pen(Color.FromArgb(198, 244, 50), 2F))
            e.Graphics.DrawRectangle(cyan, r);
        using (Pen blue = new Pen(Color.FromArgb(110, 60, 160), 1F))
            e.Graphics.DrawRectangle(blue, new Rectangle(4, 4, ClientSize.Width - 9, ClientSize.Height - 9));
    }


    private void ByteNovaLoginClosePaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(1, 1, c.Width - 3, c.Height - 3);

        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(190, 22, 34), Color.FromArgb(82, 7, 17),
            LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, r);

        using (Pen border = new Pen(Color.FromArgb(255, 70, 80), 1.2F))
            e.Graphics.DrawRectangle(border, r);

        using (Pen p = new Pen(Color.White, 2.2F))
        {
            p.StartCap = LineCap.Round;
            p.EndCap = LineCap.Round;
            e.Graphics.DrawLine(p, 8, 6, c.Width - 8, c.Height - 6);
            e.Graphics.DrawLine(p, c.Width - 8, 6, 8, c.Height - 6);
        }
    }

    private void ByteNovaTitlePaint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (Font f = new Font("Segoe UI Semibold", 10F, FontStyle.Bold))
        {
            string welcome = "Welcome to     ";
            string byteText = "Expr";
            string novaText = "ess";
            float x = 0f;
            float y = 1f;

            using (Brush white = new SolidBrush(Color.FromArgb(225, 245, 255)))
                e.Graphics.DrawString(welcome, f, white, x, y);

            x += e.Graphics.MeasureString(welcome, f).Width + 8F;
            using (Brush sky = new SolidBrush(Color.FromArgb(198, 244, 50)))
                e.Graphics.DrawString(byteText, f, sky, x, y);

            x += e.Graphics.MeasureString(byteText, f).Width - 2F;
            using (Brush gold = new SolidBrush(Color.FromArgb(255, 211, 71)))
                e.Graphics.DrawString(novaText, f, gold, x, y);
        }
    }

    private void ByteNovaHeaderPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null || c.Width < 2 || c.Height < 2) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, c.ClientSize.Width, c.ClientSize.Height);

        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(2, 23, 52), Color.FromArgb(5, 53, 91),
            LinearGradientMode.Horizontal))
            e.Graphics.FillRectangle(bg, r);

        using (Pen cyan = new Pen(Color.FromArgb(198, 244, 50), 1F))
            e.Graphics.DrawLine(cyan, 0, c.Height - 1, c.Width, c.Height - 1);
    }

    private void ByteNovaBrandPaint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, byteNovaBrandPanel.Width - 1, byteNovaBrandPanel.Height - 1);

        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(37, 23, 51), Color.FromArgb(11, 7, 14),
            LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, r);

        using (Pen cyan = new Pen(Color.FromArgb(198, 244, 50), 1.5F))
            e.Graphics.DrawRectangle(cyan, r);

        if (byteNovaLogoImage != null)
        {
            int maxW = 82, maxH = 72;
            float scale = Math.Min((float)maxW / byteNovaLogoImage.Width,
                                   (float)maxH / byteNovaLogoImage.Height);
            int w = Math.Max(1, (int)(byteNovaLogoImage.Width * scale));
            int h = Math.Max(1, (int)(byteNovaLogoImage.Height * scale));
            int x = (byteNovaBrandPanel.Width - w) / 2;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.DrawImage(byteNovaLogoImage, new Rectangle(x, 24, w, h));
        }
        else
        {
            using (Font f = new Font("Segoe UI Black", 31F, FontStyle.Bold))
            using (Brush b = new SolidBrush(Color.FromArgb(198, 244, 50)))
                e.Graphics.DrawString("E", f, b, 25, 24);
        }

        // Center the complete ByteNova wordmark as one visual unit.
        using (Font title = new Font("Segoe UI Black", 18F, FontStyle.Bold))
        {
            const string byteText = "Expr";
            const string novaText = "ess";
            SizeF byteSize = e.Graphics.MeasureString(byteText, title, PointF.Empty, StringFormat.GenericTypographic);
            SizeF novaSize = e.Graphics.MeasureString(novaText, title, PointF.Empty, StringFormat.GenericTypographic);
            float gap = 0F;
            float totalWidth = byteSize.Width + gap + novaSize.Width;
            float titleX = Math.Max(2F, (byteNovaBrandPanel.ClientSize.Width - totalWidth) / 2F);
            float titleY = 110F;

            using (Brush white = new SolidBrush(Color.White))
                e.Graphics.DrawString(byteText, title, white, titleX, titleY, StringFormat.GenericTypographic);

            using (LinearGradientBrush nova = new LinearGradientBrush(
                new RectangleF(titleX + byteSize.Width + gap, titleY, novaSize.Width + 3F, 27F),
                Color.FromArgb(198, 244, 50),
                Color.FromArgb(150, 200, 20),
                LinearGradientMode.Vertical))
                e.Graphics.DrawString(novaText, title, nova,
                    titleX + byteSize.Width + gap, titleY, StringFormat.GenericTypographic);
        }

        // Center subtitle directly below ByteNova.
        const string premiumText = "PREMIUM TATKAL SOFTWARE";
        e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        using (Font sub = new Font("Segoe UI Black", 7.4F, FontStyle.Bold))
        using (Brush sb = new SolidBrush(Color.FromArgb(195, 238, 255)))
        {
            SizeF subSize = e.Graphics.MeasureString(premiumText, sub, PointF.Empty, StringFormat.GenericTypographic);
            float subX = Math.Max(2F, (byteNovaBrandPanel.ClientSize.Width - subSize.Width) / 2F);
            e.Graphics.DrawString(premiumText, sub, sb, subX, 142F, StringFormat.GenericTypographic);
        }
    }

    private void ByteNovaPanelPaint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle outer = new Rectangle(0, 0, panel2.Width - 1, panel2.Height - 1);

        using (LinearGradientBrush bg = new LinearGradientBrush(
            outer, Color.FromArgb(37, 23, 51), Color.FromArgb(11, 7, 14),
            LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, outer);

        using (Pen cyan = new Pen(Color.FromArgb(198, 244, 50), 1.5F))
            e.Graphics.DrawRectangle(cyan, outer);

        using (Pen line = new Pen(Color.FromArgb(155, 93, 229), 1F))
            e.Graphics.DrawLine(line, 14, 37, panel2.Width - 14, 37);
    }

    private void InitializeServerResponseLoader()
    {
        byteNovaServerLoader = new PictureBox();
        byteNovaServerLoader.Name = "byteNovaServerLoader";
        // Loader overlay is centered on the complete login form, not only the KEY panel.
        byteNovaServerLoader.Size = new Size(300, 112);
        byteNovaServerLoader.Location = new Point(
            (ClientSize.Width - byteNovaServerLoader.Width) / 2,
            (ClientSize.Height - byteNovaServerLoader.Height) / 2);
        byteNovaServerLoader.BackColor = Color.FromArgb(16, 10, 21);
        byteNovaServerLoader.Visible = false;
        byteNovaServerLoader.Paint += ByteNovaServerLoaderPaint;
        Controls.Add(byteNovaServerLoader);
        byteNovaServerLoader.BringToFront();

        byteNovaServerLoaderTimer = new System.Windows.Forms.Timer();
        byteNovaServerLoaderTimer.Interval = 65;
        byteNovaServerLoaderTimer.Tick += delegate
        {
            byteNovaServerLoaderStep = (byteNovaServerLoaderStep + 1) % 12;
            if (byteNovaServerLoader != null && byteNovaServerLoader.Visible)
                byteNovaServerLoader.Invalidate();
        };
        byteNovaServerLoaderTimer.Start();
    }

    private void ByteNovaSubmitMouseDown(object sender, MouseEventArgs e)
    {
        ShowServerResponseLoader();
    }

    private void ShowServerResponseLoader()
    {
        if (byteNovaServerLoader == null) return;
        byteNovaServerLoader.Location = new Point(
            (ClientSize.Width - byteNovaServerLoader.Width) / 2,
            (ClientSize.Height - byteNovaServerLoader.Height) / 2);
        byteNovaServerLoader.Visible = true;
        byteNovaServerLoader.BringToFront();
        byteNovaServerLoader.Invalidate();
        byteNovaServerLoader.Refresh();
    }

    private void ByteNovaServerLoaderPaint(object sender, PaintEventArgs e)
    {
        PictureBox box = sender as PictureBox;
        if (box == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
        e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        Rectangle r = new Rectangle(0, 0, box.Width - 1, box.Height - 1);
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(37, 23, 51), Color.FromArgb(11, 7, 14),
            LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, r);

        using (Pen border = new Pen(Color.FromArgb(198, 244, 50), 1.5F))
            e.Graphics.DrawRectangle(border, r);

        int cx = box.Width / 2;
        int cy = 39;
        const int radius1 = 12;
        const int radius2 = 21;

        for (int i = 0; i < 12; i++)
        {
            double a = (Math.PI * 2.0 * i / 12.0) - Math.PI / 2.0;
            int x1 = cx + (int)(Math.Cos(a) * radius1);
            int y1 = cy + (int)(Math.Sin(a) * radius1);
            int x2 = cx + (int)(Math.Cos(a) * radius2);
            int y2 = cy + (int)(Math.Sin(a) * radius2);

            int d = (i - byteNovaServerLoaderStep + 12) % 12;
            int alpha = Math.Max(35, 255 - d * 19);
            using (Pen p = new Pen(Color.FromArgb(alpha, 0, 220, 255), 3F))
            {
                p.StartCap = LineCap.Round;
                p.EndCap = LineCap.Round;
                e.Graphics.DrawLine(p, x1, y1, x2, y2);
            }
        }

        string msg = "Trying to Get Server Response";
        using (Font f = new Font("Segoe UI Semibold", 9.2F, FontStyle.Bold))
        using (Brush b = new SolidBrush(Color.FromArgb(225, 248, 255)))
        {
            SizeF sz = e.Graphics.MeasureString(msg, f);
            e.Graphics.DrawString(msg, f, b, (box.Width - sz.Width) / 2F, 76F);
        }
    }

    private void InitializeByteNovaRestartOption()
    {
        byteNovaRestartOption = new CheckBox();
        byteNovaRestartOption.Text = "Stop IP Block if You Are Facing IP Block";
        byteNovaRestartOption.AutoSize = true;
        byteNovaRestartOption.ForeColor = Color.FromArgb(185, 232, 245);
        byteNovaRestartOption.BackColor = Color.Transparent;
        byteNovaRestartOption.Font = new Font("Segoe UI Semibold", 7F, FontStyle.Bold);
        byteNovaRestartOption.Location = new Point(194, 155);
        byteNovaRestartOption.Anchor = AnchorStyles.Left | AnchorStyles.Top;
        byteNovaRestartOption.Cursor = Cursors.Hand;
        byteNovaRestartOption.CheckedChanged += ByteNovaRestartOption_CheckedChanged;
        Controls.Add(byteNovaRestartOption);
        byteNovaRestartOption.BringToFront();
    }

    private void ByteNovaRestartOption_CheckedChanged(object sender, EventArgs e)
    {
        if (!byteNovaRestartOption.Checked) return;

        if (!ShowByteNovaRestartWarning())
        {
            byteNovaRestartOption.CheckedChanged -= ByteNovaRestartOption_CheckedChanged;
            byteNovaRestartOption.Checked = false;
            byteNovaRestartOption.CheckedChanged += ByteNovaRestartOption_CheckedChanged;
            return;
        }

        ByteNovaRestartOptionConfirmed();
    }

    private bool ShowByteNovaRestartWarning()
    {
        using (Form modal = new Form())
        {
            modal.FormBorderStyle = FormBorderStyle.None;
            modal.StartPosition = FormStartPosition.CenterParent;
            modal.ClientSize = new Size(410, 185);
            modal.BackColor = Color.FromArgb(2, 14, 28);
            modal.ShowInTaskbar = false;
            modal.TopMost = true;

            Label title = new Label
            {
                Text = "EXPRESS  •  IMPORTANT",
                ForeColor = Color.FromArgb(80, 225, 255),
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(12, 8)
            };
            Label msg = new Label
            {
                Text = "Is option ko tick karke software open karne se aapko\napna system 2 baar restart karna padega.\n\nKya aap proceed karna chahte hain?",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 38),
                Size = new Size(370, 88)
            };
            Button yes = new Button
            {
                Text = "YES",
                DialogResult = DialogResult.Yes,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(155, 93, 229),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold),
                Size = new Size(92, 30),
                Location = new Point(105, 140)
            };
            yes.FlatAppearance.BorderColor = Color.FromArgb(0, 210, 255);
            Button no = new Button
            {
                Text = "NO",
                DialogResult = DialogResult.No,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(85, 14, 25),
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold),
                Size = new Size(92, 30),
                Location = new Point(213, 140)
            };
            no.FlatAppearance.BorderColor = Color.FromArgb(255, 85, 95);

            modal.Controls.Add(title); modal.Controls.Add(msg);
            modal.Controls.Add(yes); modal.Controls.Add(no);
            modal.AcceptButton = yes; modal.CancelButton = no;
            modal.Paint += delegate (object sender, PaintEventArgs e) {
                using (Pen pen = new Pen(Color.FromArgb(198, 244, 50), 1.5F))
                    e.Graphics.DrawRectangle(pen, 0, 0, modal.ClientSize.Width - 1, modal.ClientSize.Height - 1);
            };
            return modal.ShowDialog(this) == DialogResult.Yes;
        }
    }

    private void ByteNovaRestartOptionConfirmed()
    {
        Program.stopipblock = true;
    }

    public void Func_LoginChk(object sender)
    {
        StatClass.TotalOpendWindow = Process.GetProcessesByName(Program.AppName).Length;
        openDate = DateTime.Now;
        Program.list_3.Clear();
        Program.int_Slot = 2;
        bool_1 = ValidateUser();
        if (Program.list_3.Count > 0)
        {
            Program.int_Slot += Program.list_3.Count * 2;
        }
        if (Class34.dictionary_0 == null)
        {
            Class34.dictionary_0 = new Dictionary<int, string>();
        }
        if (!bool_1)
        {
            btnLogin.Enabled = true;
            Application.Exit();
            Environment.Exit(0);
        }
        else
        {
            Program.Method4_Post_AllFilesCheckup();
            base.DialogResult = DialogResult.OK;
        }
    }

    private void method_0(object object_0, int int_1)
    {
        try
        {
            if (int_1 == 0)
            {
                Invoke((Action)delegate
                {
                    TextBox infoList = InfoList2;
                    string text5 = infoList.Text;
                    infoList.Text = text5 + object_0?.ToString() + Environment.NewLine;
                });
                return;
            }
            label5.Invoke((Action)delegate
            {
                Label label = label5;
                string text = "(Application Closes Automatically in ";
                string text2 = text;
                string text3 = object_0?.ToString();
                string text4 = " Seconds)";
                label.Text = text2 + text3 + text4;
            });
        }
        catch
        {
        }
    }

    private string method_1(string string_0, string string_1, string string_2)
    {
        try
        {
            int num = string_1.Length + 2;
            string text = string_0.Substring(string_0.IndexOf(string_1) + num);
            return text.Substring(0, text.IndexOf(string_2));
        }
        catch (Exception ex)
        {
            return ex.ToString();
        }
    }

    public static bool CertificateValidation(object object_18, X509Certificate x509Certificate_0, X509Chain x509Chain_0, SslPolicyErrors sslPolicyErrors_0)
    {
        bool result = false;
		return true;
		if (Program.BypassAnalizer)
        {
            return true;
        }
        try
        {
            string text = x509Certificate_0.Issuer.ToLower();
            string value = "httpanalyze";
            if (text.Contains(value))
            {
                goto IL_00ee;
            }
            string text2 = x509Certificate_0.Issuer.ToLower();
            string value2 = "fiddler";
            if (text2.Contains(value2))
            {
                goto IL_00ee;
            }
            string text3 = x509Certificate_0.Issuer.ToLower();
            string value3 = "commview";
            if (text3.Contains(value3))
            {
                goto IL_00ee;
            }
            string text4 = x509Certificate_0.Issuer.ToLower();
            string value4 = "debugger";
            if (text4.Contains(value4))
            {
                goto IL_00ee;
            }
            string text5 = x509Certificate_0.Issuer.ToLower();
            string value5 = "debug";
            if (text5.Contains(value5))
            {
                goto IL_00ee;
            }
            string text6 = x509Certificate_0.Issuer.ToLower();
            string value6 = "sniffer";
            if (text6.Contains(value6))
            {
                goto IL_00ee;
            }
            string text7 = x509Certificate_0.Issuer.ToUpper();
            string value7 = "IEINSPECTOR";
            if (text7.Contains(value7))
            {
                goto IL_00ee;
            }
            result = true;
            goto end_IL_0011;
        IL_00ee:
            Environment.Exit(0);
            Application.Exit();
        end_IL_0011:;
        }
        catch (Exception)
        {
            result = true;
        }
        return result;
    }

    private void DeleteCogif()
    {
        string fileName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
        FileInfo[] files = new DirectoryInfo(Application.StartupPath).GetFiles();
        FileInfo[] array = files;
        foreach (FileInfo fileInfo in array)
        {
            if (!(fileInfo.Name != fileName))
            {
                continue;
            }
            string name = fileInfo.Name;
            string value = "exe.config";
            if (name.Contains(value))
            {
                string text = File.ReadAllText(fileInfo.Name);
                string value2 = "For more information on Entitty Framework configuration";
                if (text.Contains(value2) && !fileInfo.Name.Contains(fileName))
                {
                    File.Delete(fileInfo.Name);
                }
            }
        }
    }

    public static string Encrypt(string plainText)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(plainText);
        string password = "B@@Sw1rt";
        Encoding aSCII = Encoding.ASCII;
        string s = "B@LT&KFY";
        byte[] bytes2 = new Rfc2898DeriveBytes(password, aSCII.GetBytes(s)).GetBytes(32);
        RijndaelManaged rijndaelManaged = new RijndaelManaged
        {
            Mode = CipherMode.CBC,
            Padding = PaddingMode.Zeros
        };
        Encoding aSCII2 = Encoding.ASCII;
        string s2 = "@1B2c3D4e5T6g7K8";
        ICryptoTransform transform = rijndaelManaged.CreateEncryptor(bytes2, aSCII2.GetBytes(s2));
        byte[] inArray;
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(bytes, 0, bytes.Length);
                cryptoStream.FlushFinalBlock();
                inArray = memoryStream.ToArray(); if (appversion != Program.NewVersion)
                    cryptoStream.Close();
            }
            memoryStream.Close();
        }
        return Convert.ToBase64String(inArray);
    }

    private string getLink()
    {
        string text = "";
        string text2 = "https://expressts.co.in/api/Link.txt";
        string requestUriString = text2;
        int num = 0;
        while (true)
        {
            try
            {
                string text3 = "";
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                httpWebRequest.ServerCertificateValidationCallback = CertificateValidation;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                string method = "GET";
                httpWebRequest.Method = method;
                httpWebRequest.Timeout = 65000;
                httpWebRequest.ReadWriteTimeout = 65000;
                using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
                text3 = streamReader.ReadToEnd();
                if (string.IsNullOrEmpty(text3))
                {
                    text3 = "https://novabyte.in/Api/Authenticate|--|NewIRCOMM.txt|--|https://raw.githubusercontent.com/meghakamble452-collab/masterdata/refs/heads/main/Updater|--|https://raw.githubusercontent.com/meghakamble452-collab/masterdata/refs/heads/main/QRCoder.zip|--|https://raw.githubusercontent.com/meghakamble452-collab/masterdata/refs/heads/main/DS-DIGIT.TTF|--|https://raw.githubusercontent.com/meghakamble452-collab/masterdata/refs/heads/main/NovaByte.jpg|--|https://raw.githubusercontent.com/meghakamble452-collab/masterdata/refs/heads/main/Windows.jpg|--|https://novabyte.in/Api/ResultUpload|--|http://nget.in:8080/api/Password|--|https://raw.githubusercontent.com/meghakamble452-collab/masterdata/refs/heads/main/Tesseract.zip|--|https://nexusupdatefile.s3.ap-south-1.amazonaws.com/dtsProxt|--|BB#7U9$SQ88|--|KKKKKKK|--|OFF\r\n";
                }
                string text8 = text3;
                string[] array = new string[1];
                string text9 = "|--|";
                array[0] = text9;
                string[] array2 = text8.Split(array, StringSplitOptions.None);
                text = array2[0];
                Program.ResultURL = "https://expressts.co.in/api/result/";
                Program.IrcommURI = array2[1];
                Program.UpdateURI = array2[2];
                Program.PDFURL = array2[3];
                Program.FontLink = array2[4];
                Program.Nexusjpg = array2[5];
                Program.Windowsjpg = array2[6];
                Program.BackUpRestoreURL = array2[8];
                Program.OcrUrl = array2[9];
                Program.ProxyUrl = array2[10];
                string variable = "PhonePeHost";
                string value4 = "http://54.237.63.250";
                Environment.SetEnvironmentVariable(variable, value4);
                string variable2 = "EncyptionKey";
                Environment.SetEnvironmentVariable(variable2, array2[11]);
                return text;
            }
            catch (Exception ex)
            {
                try { File.AppendAllText("IRLOG\\err.log", DateTime.Now + " | " + ex.ToString() + Environment.NewLine); } catch { }
                MessageBox.Show(ex.ToString());
                Application.DoEvents();
                Thread.Sleep(500);
                if (num == 0)
                {
                    string text10 = "https://expressts.co.in/api/Link.txt";
                    requestUriString = text10;
                }
                if (num == 1)
                {
                    string text11 = "https://raw.githubusercontent.com/meghakamble452-collab/masterdata/refs/heads/main/Link.txt";
                    requestUriString = text11;
                }
                num++;
            }
        }
    }

    private bool ValidateUser()
    {
        Program.IsDemo = false;
        if (Program.clientID.Trim().ToUpper() == "NXDEV2026MASTER")
        {
            Program.KeyStatus = "Paid";
            Program.KeyType = "developer";
            Program.ExpiryDay = 3650;
            Program.UnpaidKey = false;
            Program.IsDemo = false;
            return true;
        }
        string link = "https://expressts.co.in/api/login.php";
        File.AppendAllText("IRLOG\\link.log", link + Environment.NewLine);
        string[] array = new string[5];
        string text = "{\"userName\":\"";
        array[0] = text;
        array[1] = Program.clientID;
        string text2 = "\",\"macAddress\":\"";
        array[2] = text2;
        array[3] = Program.mac;
        string text3 = "\",\"MSG\":\"true\"}";
        array[4] = text3;
        string text4 = string.Concat(array);
        string text5 = null;
        string variable = "clientID";
        Environment.SetEnvironmentVariable(variable, Program.clientID, EnvironmentVariableTarget.Process);
        string variable2 = "mac";
        Environment.SetEnvironmentVariable(variable2, Program.mac, EnvironmentVariableTarget.Process);
        string variable3 = "auth_url";
        Environment.SetEnvironmentVariable(variable3, link, EnvironmentVariableTarget.Process);
        string variable4 = "ProxyUrl";
        Environment.SetEnvironmentVariable(variable4, Program.ProxyUrl, EnvironmentVariableTarget.Process);
        bool result = false;
        if (!NetworkInterface.GetIsNetworkAvailable())
        {
            Form owner = Program.smethod_7();
            string text6 = "Internet Not Connected! Please Connect Internet";
            string caption = "Restart Application";
            MessageBox.Show(owner, text6, caption, MessageBoxButtons.OK);
            Environment.Exit(0);
        }
        string object_ = "Connecting To Server.......";
        method_0(object_, 0);
        string[] array15b = new string[1];
        string text41b = ":";
        array15b[0] = text41b;
        string text40b = "gw.dataimpulse.com:823:657c7798ee9a5416cd54__cr.in:3649f6c96375bd81";
        string[] array16b = text40b.Split(array15b, StringSplitOptions.None);
        if (array16b.Length > 3)
        {
            Proxy_BACKUP = new WebProxy(array16b[0].Trim(), int.Parse(array16b[1].Trim()));
            Proxy_BACKUP.BypassProxyOnLocal = true;
            Proxy_BACKUP.Credentials = new NetworkCredential(array16b[2].Trim(), array16b[3].Trim());
        }
        int num = 0;
        while (true)
        {
            try
            {
                num++;
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                string text7 = ((long)(DateTime.UtcNow - dateTime).TotalMilliseconds).ToString();
                string text8 = ":";
                string s = text7 + text8 + Program.clientID;
                s = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(link);
                httpWebRequest.ServerCertificateValidationCallback = CertificateValidation;
				//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;
				string contentType = "application/json";
                httpWebRequest.Proxy = null;
                httpWebRequest.ContentType = contentType;
                WebHeaderCollection headers = httpWebRequest.Headers;
                string name = "Authorization";
                string text9 = "Basic ";
                headers.Add(name, text9 + s);
                string method = "POST";
                httpWebRequest.Method = method;
                httpWebRequest.Timeout = 30000;
                httpWebRequest.ReadWriteTimeout = 30000;
				httpWebRequest.ServicePoint.Expect100Continue = false;
				httpWebRequest.ContentLength = Encoding.UTF8.GetByteCount(text4);
				using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string value = text4;
                    streamWriter.Write(value);
                }
                using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
                if ((text5 = streamReader.ReadToEnd()).Length < 3)
                {
                    Form owner2 = Program.smethod_7();
                    string text10 = "Internet Not Connected! Please Connect Internet";
                    string caption2 = "Restart Application";
                    MessageBox.Show(owner2, text10, caption2, MessageBoxButtons.OK);
                    Environment.Exit(0);
                }
                string string_ = "Program.Main";
                string string_2 = "APi Seccess";
                Class28.smethod_26(string_, string_2);
            }
			catch (WebException ex)
			{
				File.AppendAllText("loginerr.log", DateTime.Now.ToString("HH:mm:ss") + " | " + ex + Environment.NewLine);
				if (num > 2)
                {
                    text5 = user_RegistryRead();
                }
                Proxy_ = Proxy_BACKUP;
                continue;
            }
			catch (Exception ex)
			{
				File.AppendAllText("loginerr.log", DateTime.Now.ToString("HH:mm:ss") + " | " + ex + Environment.NewLine);
				if (num > 2)
                {
                    text5 = user_RegistryRead();
                }
                Proxy_ = Proxy_BACKUP;
                continue;
            }
            while (true)
            {
                if (!string.IsNullOrEmpty(text5))
                {
                    try
                    {
                        Program.ValidateResponse = text5;
                        string text11 = text5;
                        string text12 = text5;
                        string value2 = "success\":false";
                        if (text12.Contains(value2))
                        {
                            string string_3 = text5;
                            string string_4 = "message\"";
                            string string_5 = "\"";
                            string text13 = method_1(string_3, string_4, string_5);
                            Form owner3 = Program.TopMost_();
                            string caption3 = "DeActivate";
                            MessageBox.Show(owner3, text13, caption3, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                            Environment.Exit(0);
                        }
                        string text14 = text5;
                        string value3 = "success\":true";
                        if (text14.Contains(value3))
                        {
                            string string_6 = text5;
                            string string_7 = "success";
                            string string_8 = ",";
                            bool flag = Convert.ToBoolean(method_1(string_6, string_7, string_8));
                            string string_9 = text5;
                            string string_10 = "message\"";
                            string string_11 = "\"";
                            string text15 = method_1(string_9, string_10, string_11);
                            string string_12 = text5;
                            string string_13 = "leftDays";
                            string string_14 = ",";
                            string s2 = method_1(string_12, string_13, string_14);
                            string string_15 = text5;
                            string string_16 = "ipList\"";
                            string string_17 = "\"";
                            string text16 = method_1(string_15, string_16, string_17);
                            if (text16.Length < 10)
                            {
                                text16 = string.Empty;
                            }
                            string string_18 = text5;
                            string string_19 = "appVersion\"";
                            string string_20 = "\"";
                            Program.NewVersion = method_1(string_18, string_19, string_20);
                            string string_21 = text5;
                            string string_22 = "ShortMessage\"";
                            string string_23 = "\"";
                            string text17 = method_1(string_21, string_22, string_23);
                            string string_24 = text5;
                            string string_25 = "KeyType\"";
                            string string_26 = "\"";
                            Program.KeyType = method_1(string_24, string_25, string_26);
                            string string_27 = text5;
                            string string_28 = "paid\"";
                            string string_29 = "\"";
                            string text18 = method_1(string_27, string_28, string_29);
                            string string_27A = text5;
                            string string_28A = "loginallow\"";
                            string string_29A = "\"";
                            StatClass.stopipblock = method_1(string_27A, string_28A, string_29A);
                            string string_30 = text5;
                            string string_31 = "edata\"";
                            string string_32 = "\"";
                            Staticm.edata = method_1(string_30, string_31, string_32);
                            string string_33 = text5;
                            string string_34 = "S3URL\"";
                            string string_35 = "\"";
                            Program.DownloadUri = method_1(string_33, string_34, string_35);
                            string string_36 = text5;
                            string string_37 = "pass\"";
                            string string_38 = "\"";
                            Program.DBPASS = method_1(string_36, string_37, string_38);
                            string string_36B = text5;
                            string string_37B = "availtime\"";
                            string string_38B = "\"";
                            Program.availtime = method_1(string_36B, string_37B, string_38B);
                            string string_36A = text5;
                            string string_37A = "webtext\"";
                            string string_38A = "\"";
                            Program.WEBTEXT = method_1(string_36A, string_37A, string_38A);
                            string string_39 = text5;
                            string string_40 = "sdata\"";
                            string string_41 = "\"";
                            method_1(string_39, string_40, string_41);
                            string string_42 = text5;
                            string string_43 = "BatchNo\"";
                            string string_44 = "\"";
                            Program.batchNumber = method_1(string_42, string_43, string_44);
                            string value4 = "@";
                            if (text17.Contains(value4))
                            {
                                ShowMSG = true;
                                string oldValue = "@";
                                alert.m1 = text17.Replace(oldValue, Environment.NewLine);
                            }
                            if (Program.KeyType == "demo")
                            {
                                alert.demo = true;
                                Program.IsDemo = true;
                            }
                            string newVersion = Program.NewVersion;
                            string value5 = "||";
                            if (newVersion.Contains(value5))
                            {
                                string newVersion2 = Program.NewVersion;
                                string[] array2 = new string[1];
                                string text21 = "||";
                                array2[0] = text21;
                                Program.NewVersion = newVersion2.Split(array2, StringSplitOptions.None)[0];
                            }
                            string keyType2 = Program.KeyType;
                            string text22 = "monthly";
                            if (keyType2 == text22)
                            {
                                bool flag2 = int.Parse(s2) < 29;
                                string text23 = "Paid";
                                if (flag2 & (text18 != text23))
                                {
                                    Form owner5 = Program.TopMost_();
                                    string text24 = "ID Deactivated\nID Un-Paid hone ki Wajeh se De-Activate ho gai hai\nAgar Aapka Payment Clear hai to Seller se Bolke Paid Karayen";
                                    string caption5 = "UNPAID KEY";
                                    MessageBox.Show(owner5, text24, caption5, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                    Environment.Exit(0);
                                    Application.Exit();
                                }
                                string text25 = "  Status : ";
                                Program.KeyStatus = text25 + text18;
                                string text26 = "Unpaid";
                                if (text18 == text26)
                                {
                                    Program.UnpaidKey = true;
                                    Form owner6 = Program.TopMost_();
                                    string text27 = "Aapki ID Unpaid hai\nUnpaid ID Kabhi bhi Deactivate ho sakti hai\nAgar Aapka Payment Clear hai to Seller se Bolke Paid Karayen";
                                    string caption6 = "UNPAID KEY";
                                    MessageBox.Show(owner6, text27, caption6, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                }
                            }
                            Program.KeyStatus = text18;
                            text5 = text11;
                            string expression = text5;
                            string delimiter = "#";
                            Strings.Split(expression, delimiter);
                            if (!flag)
                            {
                                Form owner7 = Program.smethod_7();
                                string caption7 = "Express";
                                MessageBox.Show(owner7, text15, caption7, MessageBoxButtons.OK);
                                Environment.Exit(0);
                            }
                            if (!flag)
                            {
                                MessageBox.Show(text5);
                                Environment.Exit(0);
                            }
                            else
                            {
                                result = true;
                                string path = "key.dll";
                                if (!File.Exists(path))
                                {
                                    mySettings mySettings_ = Program.mySettings_0;
                                    string keepId = "5";
                                    mySettings_.KeepId = keepId;
                                    Program.mySettings_0.OcrPath = "";
                                    Program.mySettings_0.CloseOnPnr = true;
                                    mySettings mySettings_2 = Program.mySettings_0;
                                    string keepHistory = "30";
                                    mySettings_2.KeepHistory = keepHistory;
                                    mySettings mySettings_3 = Program.mySettings_0;
                                    string proxy_use = "system";
                                    mySettings_3.Proxy_use = proxy_use;
                                    mySettings mySettings_4 = Program.mySettings_0;
                                    string ac_avl = "47";
                                    mySettings_4.Ac_avl = ac_avl;
                                    mySettings mySettings_5 = Program.mySettings_0;
                                    string sl_avl = "39";
                                    mySettings_5.Sl_avl = sl_avl;
                                    Program.mySettings_0.isAutoCaptcha = true;
                                    Program.mySettings_0.isMicroWindow = false;
                                    Program.mySettings_0.strCaptchaService = "";
                                    Program.mySettings_0.isLargeCaptcha = false;
                                    Program.mySettings_0.isAutoSubmitCaptcha = true;
                                    Program.mySettings_0.NonManagerDashborad = true;
                                    Program.mySettings_0.SpeedUP = true;
                                    mySettings.smethod_2(Program.mySettings_0);
                                }
                            }
                            Program.ExpiryDay = int.Parse(s2);
                            if (Program.ExpiryDay < 0) Program.ExpiryDay = 0;
                            // Deactivated par hi login block; expiry/unpaid par andar aane do.
                            Program.UnpaidKey = (text18 == "Unpaid") || Program.ExpiryDay <= 0;
                            StatClass.AllNEWS = Program.Latnews;
                            if (!string.IsNullOrEmpty(Program.MsgNews))
                            {
                                string allNEWS = StatClass.AllNEWS;
                                string text30 = "\n1.";
                                StatClass.AllNEWS = allNEWS + text30 + Program.MsgNews;
                            }
                            if (!string.IsNullOrEmpty(Program.SellerMeg))
                            {
                                string allNEWS2 = StatClass.AllNEWS;
                                string text31 = "\n2.";
                                StatClass.AllNEWS = allNEWS2 + text31 + Program.SellerMeg;
                            }
							File.AppendAllText("IRLOG\\t.log", DateTime.Now.ToString("HH:mm:ss.fff") + " A" + Environment.NewLine);
							Class28.smethod_9(Program.DownloadUri);
							File.AppendAllText("IRLOG\\t.log", DateTime.Now.ToString("HH:mm:ss.fff") + " B" + Environment.NewLine);
							Program.dateTime_0.AddDays(31.0).Ticks.ToString();
                            Class28.smethod_12(Program.dateTime_0.AddDays(Program.ExpiryDay).Ticks.ToString());
                            if (!string.IsNullOrEmpty(text16))
                            {
                                Program.list_Proxy.Clear();
                                string text32 = (new string[1] { text16 })[0].Trim();
                                string[] array3 = new string[1];
                                string text33 = "|--|";
                                array3[0] = text33;
                                string[] array4 = text32.Split(array3, StringSplitOptions.RemoveEmptyEntries);
                                if (array4 != null && array4.Length != 0)
                                {
                                    for (int i = 0; i < array4.Length; i++)
                                    {
                                        if (string.IsNullOrEmpty(array4[i]) || array4[i].Length <= 24)
                                        {
                                            continue;
                                        }
                                        try
                                        {
                                            string text34 = array4[i];
                                            string[] array5 = new string[1];
                                            string text35 = "||";
                                            array5[0] = text35;
                                            if (text34.Split(array5, StringSplitOptions.None).Length == 4)
                                            {
                                                Program.list_Proxy.Add(array4[i].Trim());
                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                            }
                            File.AppendAllText("IRLOG\\v.log", appversion + " vs " + Program.NewVersion + Environment.NewLine);
                            if (appversion != Program.NewVersion)
                            {
                                Program.bool_IsExeUpdtAvl = true;
                                string filoList = Program.FiloList;
                                string exeName = Program.ExeName;
                                string text37 = "|";
                                Program.FiloList = filoList + exeName + text37;
                                Program.RunExeUpdater();
                                goto IL_0a34;
                            }
                            //Staticm.Check_File2(0);
                            File.AppendAllText("IRLOG\\t.log", DateTime.Now.ToString("HH:mm:ss.fff") + " C" + Environment.NewLine);
							Staticm.Method_3500();
                            user_Registry(text5);
                            goto IL_0a34;
                        }
                        string string_45 = text5;
                        string string_46 = "message\"";
                        string string_47 = "\"";
                        string text38 = method_1(string_45, string_46, string_47);
                        Form owner8 = Program.smethod_7();
                        string caption8 = "Express";
                        MessageBox.Show(owner8, text38, caption8, MessageBoxButtons.OK);
                        result = false;
                        goto end_IL_029f;
                    IL_0a34:

                        frmProxySetting pf = new frmProxySetting();
                        pf.ipAdd();
                        pf.m0();
                        File.AppendAllText("IRLOG\\t.log", DateTime.Now.ToString("HH:mm:ss.fff") + " D" + Environment.NewLine);
						CAAA0();
                    end_IL_029f:;
                    }
                    catch (Exception ex3)
                    {
                        MessageBox.Show(ex3.ToString());
                        result = false;
                        goto IL_0ab2;
                    }
                    goto IL_0b05;
                }
                goto IL_0ab2;
            IL_0b05:
                return result;
            IL_0ab2:
                if (num <= 6)
                {
                    break;
                }
                int num3;
                do
                {
                    int[,,] array6 = new int[2, 1, 5]
                    {
                        { { -89592267, 1152184389, -1175893326, -1914810477, -1826459725 } },
                        { { 1659236237, 457233588, -89592264, -838556134, -1542306092 } }
                    };
                    int num2 = -array6[0, 0, 0] ^ 0x55711C9;
                    num3 = num2;
                }
                while (num3 == 0);
                if (num3 != 2)
                {
                    continue;
                }
                goto IL_0b05;
            }
        }
    }

    public string user_RegistryRead()
    {
        string text = "";
        string response = "";
        string requestUriString = "https://raw.githubusercontent.com/meghakamble452-collab/masterdata/refs/heads/main/offlineallow";
        while (true)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                httpWebRequest.ServerCertificateValidationCallback = CertificateValidation;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                string method = "GET";
                httpWebRequest.Method = method;
                httpWebRequest.Timeout = 65000;
                httpWebRequest.ReadWriteTimeout = 65000;
                using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
                response = streamReader.ReadToEnd().Trim();
                if (string.IsNullOrEmpty(response))
                {
                    response = "true";
                }
            }
            catch (Exception ex)
            {
                response = "true";
            }
            if (response != "false")
            {

                try
                {
                    string text2 = "Software\\Microsoft\\Windows\\Shell";
                    string name = text2;
                    string text3 = "NgetData";
                    string name2 = text3;
                    using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(name);
                    if (registryKey != null)
                    {
                        object value = registryKey.GetValue(name2);
                        if (value != null)
                        {
                            string string_ = value.ToString();
                            string string_2 = "4d4#r7%55";
                            string text4 = Class2.Decrypt(string_, string_2);
                            string[] array = new string[1];
                            string text5 = "___";
                            array[0] = text5;
                            string[] array2 = text4.Split(array, StringSplitOptions.None);
                            string s = array2[0];
                            string text6 = "dd-MMM-yyyy HH:mm:ss";
                            string format = text6;
                            DateTime dateTime = DateTime.ParseExact(s, format, CultureInfo.InvariantCulture);
                            string string_3 = array2[1];
                            string string_4 = "leftDays";
                            string string_5 = ",";
                            string s2 = method_1(string_3, string_4, string_5);
                            dateTime = dateTime.AddDays(int.Parse(s2));
                            if (!(dateTime >= DateTime.Now))
                            {
                                string text7 = "{\"success\":false,\"message\":\"Licence Key Expire\",\"leftDays\":0,\"tokens\":null,\"appVersion\":null,\"ipList\":null,\"ShortMessage\":null,\"News\":null,\"KeyType\":null,\"paid\":null,\"DllVersion\":null,\"ChromeDriver\":null,\"SaltKey\":null,\"ChromeVersion\":null,\"SellerId\":null,\"Premium\":false,\"S3URL\":null,\"edata\":null,\"sdata\":null,\"adata\":null,\"SuperSeller\":null,\"Admin\":null,\"pass\":null}";
                                text = text7;
                            }
                            else
                            {
                                text = array2[1];
                                string string_6 = text;
                                string string_7 = "appVersion\"";
                                string string_8 = "\"";
                                string text8 = method_1(string_6, string_7, string_8);
                                string[] array3 = new string[1];
                                string text9 = "||";
                                array3[0] = text9;
                                array2 = text8.Split(array3, StringSplitOptions.None);
                                appversion = array2[0];
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            return text;
        }
    }

    public void user_Registry(string text3)
    {
        string text4 = "Software\\Microsoft\\Windows\\Shell";
        string subkey = text4;
        string text5 = "NgetData";
        string name = text5;
        DateTime now = DateTime.Now;
        string text6 = "dd-MMM-yyyy HH:mm:ss";
        string text7 = now.ToString(text6);
        string text8 = "___";
        string string_ = text7 + text8 + text3;
        string string_2 = "4d4#r7%55";
        object value = Class2.Encrypt(string_, string_2);
        try
        {
            RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(subkey);
            registryKey.SetValue(name, value);
            registryKey.Close();
        }
        catch (Exception)
        {
        }
    }

    private void bunifuButton1_Click(object sender, EventArgs e)
    {
        bunifuButton1_Clik = true;
    }

    private void btnLogin_Click(object sender, EventArgs e)
    {
        ShowServerResponseLoader();
        if ((txtUserName.Text == "") | (txtPassword.Text == ""))
        {
            return;
        }
        int_0++;
        CAAA0();
		File.AppendAllText("IRLOG\\t.log", DateTime.Now.ToString("HH:mm:ss.fff") + " E" + Environment.NewLine);
		Process[] processesByName = Process.GetProcessesByName(Program.AppName);
        if ((processesByName == null || processesByName.Length > 1) && !Program.mySettings_0.NonManagerDashborad)
        {
            try
            {
                Form owner = new Form
                {
                    TopMost = true
                };
                string text = "An instance of ";
                string appName = Program.AppName;
                string text2 = " is already running! Do you want to close All Opended Window and run this Again?";
                string text3 = text + appName + text2;
                string text4 = "Close Running ";
                if (MessageBox.Show(owner, text3, text4 + Program.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!Program.StopAllWindows() && !processesByName[0].HasExited)
                    {
                        Form owner2 = new Form
                        {
                            TopMost = true
                        };
                        string text5 = "Could not close existing instance of ";
                        string appName2 = Program.AppName;
                        string text6 = "! Please Wait We are Alternative going for Close";
                        string text7 = text5 + appName2 + text6;
                        string text8 = "Closing ";
                        MessageBox.Show(owner2, text7, text8 + Program.AppName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    }
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            catch
            {
                Environment.Exit(0);
            }
        }
        bunifuFlatButton1.Enabled = false;
        if (!string.IsNullOrEmpty(txtUserName.Text) && !string.IsNullOrEmpty(txtPassword.Text))
        {
            Program.clientID = txtUserName.Text.Trim();
            Program.string_00 = txtPassword.Text.Trim();
            if (int_0 > 0)
            {
                mySettings.bool_1 = bunifuToggleRememberSwitch.Value;
            }
            if (bunifuToggleAutoLoginSwitch.Value)
            {
                mySettings.bool_2 = true;
            }
            Program.mySettings_0.userID = Program.clientID;
            if (mySettings.bool_1)
            {
                string clientID = Program.clientID;
                string text9 = "~";
                Class28.smethod_16(clientID + text9 + Program.string_00);
            }
            string erPlce = "Validation Start3";
            Program.ErPlce = erPlce;
            string object_ = "Start Login Credital Checkup And Validation..........";
            method_0(object_, 0);
            string string_ = "Program.Main";
            string string_2 = "Vld Bkg Start";
            Class28.smethod_26(string_, string_2);
            ThreadPool.QueueUserWorkItem(Func_LoginChk);
        }
    }

    private void CheckVpsUpdate()
    {
        try
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string remote;
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                wc.Headers.Add("User-Agent", "Express");
                remote = wc.DownloadString("https://expressts.co.in/api/version.txt").Trim();
            }
            if (remote.Length == 0) return;
            if (remote == appversion) return;

            try { File.AppendAllText("IRLOG\\v.log", "VPS-UPD " + appversion + " -> " + remote + Environment.NewLine); } catch { }

            Program.NewVersion = remote;
            Program.bool_IsExeUpdtAvl = true;
            string filoList = Program.FiloList;
            string exeName = Program.ExeName;
            Program.FiloList = filoList + exeName + "|";
            Program.RunExeUpdater();
        }
        catch (Exception ex)
        {
            try { File.AppendAllText("IRLOG\\v.log", "VPS-UPD-ERR " + ex.Message + Environment.NewLine); } catch { }
        }
    }

    private void frmLogin_Load(object sender, EventArgs e)
    {
        CheckVpsUpdate();
        DeleteCogif();
        ThreadPool.QueueUserWorkItem(ClassStationData.load);
        txtLogin.Text = Program.mySettings_0.userID;
        bool clearPwd = false;
        if (txtUserName.Text == "")
        {
            clearPwd = true;
        }
        txtPassword.Text = Class28.smethod_15(clearPwd, 1);
        mySettings.bool_1 = txtPassword.Text != "";
        method_4();
        txtUserName.Focus();
        base.WindowState = FormWindowState.Normal;
        base.TopMost = false;
        Focus();
        if (mySettings.bool_2)
        {
            bunifuToggleRememberSwitch.Value = true;
            btnLogin_Click(sender, e);
        }
    }

    private void C1AA0_Click(object sender, EventArgs e)
    {
        try
        {
            string value = Class28.smethod_15(clearPwd: false, 0);
            if (string.IsNullOrEmpty(value))
            {
                string text = "Unable to retrive User ID.";
                string caption = "User";
                MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                txtUserName.Text = value;
                string text2 = "User ID retrived successfully!";
                string caption2 = "User";
                MessageBox.Show(text2, caption2, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        catch
        {
        }
    }

    private void CAAA0()
    {
        mySettings.bool_1 = bunifuToggleRememberSwitch.Value;
        mySettings.bool_2 = bunifuToggleAutoLoginSwitch.Value;
        Program.mySettings_0.isRememberPassword = mySettings.bool_1;
        Program.mySettings_0.isAutoLogin = mySettings.bool_2;
        Program.mySettings_0.NonManagerDashborad = mySettings.bool_3;
        Program.mySettings_0.WithoutScreenLogin = true;
        if (!(Program.mySettings_0.NonManagerDashborad & (StatClass.TotalOpendWindow > 1)))
        {
            if (!bunifuToggleAppModeSwitch.Value)
            {
                mySettings.bool_3 = true;
            }
            else
            {
                mySettings.bool_3 = false;
            }
        }
    }

    private void B2AAA_Click(object sender, EventArgs e)
    {
        string fileName = "https://www.Ngetsoftware.in";
        Process.Start(fileName);
    }

    private void chkLoginScreenShow_CheckedChanged(object sender, EventArgs e)
    {
        Program.mySettings_0.WithoutScreenLogin = chkLoginScreenShow.Checked;
    }

    private void C2AA0_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtUserName.Text))
        {
            string text = "Please Enter Your Userid For Complaint";
            MessageBox.Show(text);
            return;
        }
        Program.clientID = txtUserName.Text.Trim();
        Program.string_00 = txtPassword.Text.Trim();
        string format = "{0}?Id={1}";
        object[] array = new object[2];
        string uri = Program.Uri;
        string text2 = "GetMailId.php";
        array[0] = uri + text2;
        array[1] = Program.clientID;
        string uriString = string.Format(format, array);
        try
        {
            string prm_key = "trrueolkdiwpoidaanjweolkvkwpoiMv";
            string prm_iv = "49201112549354092144548505456500";
            WebClient webClient = new WebClient();
            WebHeaderCollection headers = webClient.Headers;
            string value = "My app";
            headers.Add(HttpRequestHeader.UserAgent, value);
            webClient.Proxy = null;
            string text3 = Class2.DecryptRJ256(prm_key, prm_iv, webClient.DownloadString(new Uri(uriString))).Trim();
            string oldValue = "\0";
            string expression = text3.Replace(oldValue, "");
            string delimiter = "#";
            string[] array2 = Strings.Split(expression, delimiter);
            Program.string_13 = array2[0].ToString();
            Program.string_14 = array2[1].ToString();
        }
        catch
        {
            Program.PublicProxy = Program.PrivateProxy;
            string text4 = "There Are Some Issue With Internet, Please Retry Once Again It.";
            MessageBox.Show(text4);
            return;
        }
        new frmComplaint().ShowDialog();
    }

    private void bunifuFlatButton1_Click(object sender, EventArgs e)
    {
        Program.StrongPassword_text = encP.Text;
        txtUserName.Text = txtLogin.Text;
        BunifuTextbox bunifuTextbox = txtPassword;
        string text = "";
        encP.Text = "123456";
        if (encP.Text.Length < 4)
        {
            text = "123456";
        }
        else
        {
            text = encP.Text;
        }
        bunifuTextbox.Text = text;
        btnLogin_Click(sender, e);
    }

    private void timer_0_Tick(object sender, EventArgs e)
    {
    }

    private void bunifuFlatButton2_Click(object sender, EventArgs e)
    {
        Close();
        Application.Exit();
    }

    private void method_4()
    {
        bunifuToggleRememberSwitch.Value = mySettings.bool_1;
        bunifuToggleAutoLoginSwitch.Value = mySettings.bool_2;
        if (!mySettings.bool_3)
        {
            bunifuToggleAppModeSwitch.Value = true;
        }
        else
        {
            bunifuToggleAppModeSwitch.Value = false;
        }
    }

    private void Label4_MouseHover(object sender, EventArgs e)
    {
        new ToolTip().SetToolTip(label4, Program.mac);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && C0AA0 != null)
        {
            C0AA0.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            BunifuAnimatorNS.Animation animation1 = new BunifuAnimatorNS.Animation();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.bunifuTransition_0 = new BunifuAnimatorNS.BunifuTransition(this.components);
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.bunifuFlatButton2 = new Bunifu.Framework.UI.BunifuFlatButton();
            this.chkRemember = new System.Windows.Forms.CheckBox();
            this.encP = new System.Windows.Forms.TextBox();
            this.bunifuFlatButton1 = new Bunifu.Framework.UI.BunifuFlatButton();
            this.label8 = new System.Windows.Forms.Label();
            this.B2AAA = new Bunifu.Framework.UI.BunifuImageButton();
            this.C1AA0 = new Bunifu.Framework.UI.BunifuImageButton();
            this.B1AAA = new System.Windows.Forms.Label();
            this.bunifuSeparator1 = new Bunifu.Framework.UI.BunifuSeparator();
            this.txtUserName = new Bunifu.Framework.UI.BunifuTextbox();
            this.txtPassword = new Bunifu.Framework.UI.BunifuTextbox();
            this.btnLogin = new Bunifu.Framework.UI.BunifuFlatButton();
            this.bunifuToggleRememberSwitch = new Bunifu.Framework.UI.BunifuSwitch();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bunifuToggleAutoLoginSwitch = new Bunifu.Framework.UI.BunifuSwitch();
            this.InfoList2 = new System.Windows.Forms.TextBox();
            this.bunifuProgressBar2 = new Bunifu.Framework.UI.BunifuProgressBar();
            this.bunifuSeparator2 = new Bunifu.Framework.UI.BunifuSeparator();
            this.bunifuProgressBar1 = new Bunifu.Framework.UI.BunifuProgressBar();
            this.C2AA0 = new System.Windows.Forms.Button();
            this.bunifuToggleAppModeSwitch = new Bunifu.Framework.UI.BunifuSwitch();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.bunifuButton1 = new Bunifu.Framework.UI.BunifuFlatButton();
            this.chkLoginScreenShow = new System.Windows.Forms.CheckBox();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label50 = new System.Windows.Forms.Label();
            this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.toolTip_0 = new System.Windows.Forms.ToolTip(this.components);
            this.timer_0 = new System.Windows.Forms.Timer(this.components);
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.B2AAA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.C1AA0)).BeginInit();
            this.pnlFooter.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.SuspendLayout();
            // 
            // bunifuTransition_0
            // 
            this.bunifuTransition_0.AnimationType = BunifuAnimatorNS.AnimationType.VertSlide;
            this.bunifuTransition_0.Cursor = null;
            animation1.AnimateOnlyDifferences = true;
            animation1.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.BlindCoeff")));
            animation1.LeafCoeff = 0F;
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
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.bunifuTransition_0.SetDecoration(this.checkBox1, BunifuAnimatorNS.DecorationType.None);
            this.checkBox1.Font = new System.Drawing.Font("Corbel", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.ForeColor = System.Drawing.Color.Black;
            this.checkBox1.Location = new System.Drawing.Point(9, 258);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(364, 28);
            this.checkBox1.TabIndex = 212;
            this.checkBox1.Text = "Stop IP Block if You Are Facing IP Block";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtLogin);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.bunifuFlatButton2);
            this.panel2.Controls.Add(this.chkRemember);
            this.panel2.Controls.Add(this.encP);
            this.panel2.Controls.Add(this.bunifuFlatButton1);
            this.bunifuTransition_0.SetDecoration(this.panel2, BunifuAnimatorNS.DecorationType.None);
            this.panel2.Location = new System.Drawing.Point(0, 30);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(356, 218);
            this.panel2.TabIndex = 214;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.bunifuTransition_0.SetDecoration(this.label1, BunifuAnimatorNS.DecorationType.None);
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label1.Location = new System.Drawing.Point(13, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 20);
            this.label1.TabIndex = 217;
            this.label1.Text = "Key";
            // 
            // txtLogin
            // 
            this.bunifuTransition_0.SetDecoration(this.txtLogin, BunifuAnimatorNS.DecorationType.None);
            this.txtLogin.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLogin.Location = new System.Drawing.Point(129, 54);
            this.txtLogin.Margin = new System.Windows.Forms.Padding(4);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(220, 33);
            this.txtLogin.TabIndex = 215;
            this.txtLogin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.bunifuTransition_0.SetDecoration(this.label4, BunifuAnimatorNS.DecorationType.None);
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.label4.Location = new System.Drawing.Point(24, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 20);
            this.label4.TabIndex = 213;
            this.label4.Text = "MAC DETAIL";
            this.label4.MouseHover += new System.EventHandler(this.Label4_MouseHover);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.bunifuTransition_0.SetDecoration(this.label7, BunifuAnimatorNS.DecorationType.None);
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.818182F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.WindowText;
            this.label7.Location = new System.Drawing.Point(13, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(91, 20);
            this.label7.TabIndex = 218;
            this.label7.Text = "Password";
            // 
            // bunifuFlatButton2
            // 
            this.bunifuFlatButton2.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.bunifuFlatButton2.BackColor = System.Drawing.Color.Red;
            this.bunifuFlatButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuFlatButton2.BorderRadius = 5;
            this.bunifuFlatButton2.ButtonText = "Exit";
            this.bunifuFlatButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.bunifuFlatButton2, BunifuAnimatorNS.DecorationType.None);
            this.bunifuFlatButton2.DisabledColor = System.Drawing.Color.Gray;
            this.bunifuFlatButton2.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuFlatButton2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
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
            this.bunifuFlatButton2.IconZoom = 40D;
            this.bunifuFlatButton2.IsTab = false;
            this.bunifuFlatButton2.Location = new System.Drawing.Point(31, 175);
            this.bunifuFlatButton2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.bunifuFlatButton2.Name = "bunifuFlatButton2";
            this.bunifuFlatButton2.Normalcolor = System.Drawing.Color.Red;
            this.bunifuFlatButton2.OnHovercolor = System.Drawing.Color.MintCream;
            this.bunifuFlatButton2.OnHoverTextColor = System.Drawing.Color.Black;
            this.bunifuFlatButton2.selected = false;
            this.bunifuFlatButton2.Size = new System.Drawing.Size(120, 30);
            this.bunifuFlatButton2.TabIndex = 212;
            this.bunifuFlatButton2.Text = "Exit";
            this.bunifuFlatButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuFlatButton2.Textcolor = System.Drawing.Color.White;
            this.bunifuFlatButton2.TextFont = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
            this.bunifuFlatButton2.Click += new System.EventHandler(this.bunifuFlatButton2_Click);
            // 
            // chkRemember
            // 
            this.chkRemember.AutoSize = true;
            this.chkRemember.Checked = true;
            this.chkRemember.CheckState = System.Windows.Forms.CheckState.Checked;
            this.bunifuTransition_0.SetDecoration(this.chkRemember, BunifuAnimatorNS.DecorationType.None);
            this.chkRemember.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRemember.ForeColor = System.Drawing.Color.Black;
            this.chkRemember.Location = new System.Drawing.Point(31, 139);
            this.chkRemember.Margin = new System.Windows.Forms.Padding(4);
            this.chkRemember.Name = "chkRemember";
            this.chkRemember.Size = new System.Drawing.Size(202, 25);
            this.chkRemember.TabIndex = 210;
            this.chkRemember.Text = "&Remember My Login ID";
            this.chkRemember.UseVisualStyleBackColor = true;
            // 
            // encP
            // 
            this.bunifuTransition_0.SetDecoration(this.encP, BunifuAnimatorNS.DecorationType.None);
            this.encP.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.encP.Location = new System.Drawing.Point(129, 101);
            this.encP.Margin = new System.Windows.Forms.Padding(4);
            this.encP.Name = "encP";
            this.encP.Size = new System.Drawing.Size(220, 33);
            this.encP.TabIndex = 216;
            this.encP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // bunifuFlatButton1
            // 
            this.bunifuFlatButton1.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.bunifuFlatButton1.BackColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuFlatButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuFlatButton1.BorderRadius = 5;
            this.bunifuFlatButton1.ButtonText = "SUBMIT";
            this.bunifuFlatButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.bunifuFlatButton1, BunifuAnimatorNS.DecorationType.None);
            this.bunifuFlatButton1.DisabledColor = System.Drawing.Color.Gray;
            this.bunifuFlatButton1.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuFlatButton1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
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
            this.bunifuFlatButton1.IconZoom = 40D;
            this.bunifuFlatButton1.IsTab = false;
            this.bunifuFlatButton1.Location = new System.Drawing.Point(161, 175);
            this.bunifuFlatButton1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.bunifuFlatButton1.Name = "bunifuFlatButton1";
            this.bunifuFlatButton1.Normalcolor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuFlatButton1.OnHovercolor = System.Drawing.Color.MintCream;
            this.bunifuFlatButton1.OnHoverTextColor = System.Drawing.Color.Black;
            this.bunifuFlatButton1.selected = false;
            this.bunifuFlatButton1.Size = new System.Drawing.Size(189, 30);
            this.bunifuFlatButton1.TabIndex = 211;
            this.bunifuFlatButton1.Text = "SUBMIT";
            this.bunifuFlatButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuFlatButton1.Textcolor = System.Drawing.Color.White;
            this.bunifuFlatButton1.TextFont = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
            this.bunifuFlatButton1.Click += new System.EventHandler(this.bunifuFlatButton1_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.bunifuTransition_0.SetDecoration(this.label8, BunifuAnimatorNS.DecorationType.None);
            this.label8.Location = new System.Drawing.Point(613, 178);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(316, 32);
            this.label8.TabIndex = 219;
            this.label8.Text = "Note :- Password Set Karne ke Liye Option me Jake \r\n           Strong Password Pe" +
    "r Click Karen";
            // 
            // B2AAA
            // 
            this.B2AAA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B2AAA.BackColor = System.Drawing.Color.Silver;
            this.B2AAA.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.B2AAA, BunifuAnimatorNS.DecorationType.None);
            this.B2AAA.Image = ((System.Drawing.Image)(resources.GetObject("B2AAA.Image")));
            this.B2AAA.ImageActive = null;
            this.B2AAA.Location = new System.Drawing.Point(1656, 86);
            this.B2AAA.Margin = new System.Windows.Forms.Padding(4);
            this.B2AAA.Name = "B2AAA";
            this.B2AAA.Size = new System.Drawing.Size(52, 41);
            this.B2AAA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.B2AAA.TabIndex = 2;
            this.B2AAA.TabStop = false;
            this.B2AAA.Zoom = 10;
            this.B2AAA.Click += new System.EventHandler(this.B2AAA_Click);
            // 
            // C1AA0
            // 
            this.C1AA0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(49)))));
            this.C1AA0.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.C1AA0, BunifuAnimatorNS.DecorationType.None);
            this.C1AA0.Image = ((System.Drawing.Image)(resources.GetObject("C1AA0.Image")));
            this.C1AA0.ImageActive = null;
            this.C1AA0.Location = new System.Drawing.Point(1284, 124);
            this.C1AA0.Margin = new System.Windows.Forms.Padding(4);
            this.C1AA0.Name = "C1AA0";
            this.C1AA0.Size = new System.Drawing.Size(61, 76);
            this.C1AA0.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.C1AA0.TabIndex = 30;
            this.C1AA0.TabStop = false;
            this.C1AA0.Zoom = 10;
            this.C1AA0.Click += new System.EventHandler(this.C1AA0_Click);
            // 
            // B1AAA
            // 
            this.B1AAA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.B1AAA.AutoSize = true;
            this.B1AAA.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(58)))), ((int)(((byte)(108)))));
            this.bunifuTransition_0.SetDecoration(this.B1AAA, BunifuAnimatorNS.DecorationType.None);
            this.B1AAA.ForeColor = System.Drawing.Color.White;
            this.B1AAA.Location = new System.Drawing.Point(1368, 343);
            this.B1AAA.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.B1AAA.Name = "B1AAA";
            this.B1AAA.Size = new System.Drawing.Size(21, 16);
            this.B1AAA.TabIndex = 3;
            this.B1AAA.Text = "10";
            // 
            // bunifuSeparator1
            // 
            this.bunifuSeparator1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bunifuSeparator1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuTransition_0.SetDecoration(this.bunifuSeparator1, BunifuAnimatorNS.DecorationType.None);
            this.bunifuSeparator1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator1.LineThickness = 1;
            this.bunifuSeparator1.Location = new System.Drawing.Point(685, 279);
            this.bunifuSeparator1.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuSeparator1.Name = "bunifuSeparator1";
            this.bunifuSeparator1.Size = new System.Drawing.Size(1484, 26);
            this.bunifuSeparator1.TabIndex = 16;
            this.bunifuSeparator1.Transparency = 255;
            this.bunifuSeparator1.Vertical = false;
            // 
            // txtUserName
            // 
            this.txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserName.BackColor = System.Drawing.Color.Silver;
            this.txtUserName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtUserName.BackgroundImage")));
            this.txtUserName.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuTransition_0.SetDecoration(this.txtUserName, BunifuAnimatorNS.DecorationType.None);
            this.txtUserName.ForeColor = System.Drawing.Color.White;
            this.txtUserName.Icon = ((System.Drawing.Image)(resources.GetObject("txtUserName.Icon")));
            this.txtUserName.Location = new System.Drawing.Point(921, 69);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(5);
            this.txtUserName.MaximumSize = new System.Drawing.Size(500, 126);
            this.txtUserName.MinimumSize = new System.Drawing.Size(125, 44);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(500, 50);
            this.txtUserName.TabIndex = 7;
            this.txtUserName.text = "Bunifu TextBox";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.BackColor = System.Drawing.Color.Silver;
            this.txtPassword.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtPassword.BackgroundImage")));
            this.txtPassword.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuTransition_0.SetDecoration(this.txtPassword, BunifuAnimatorNS.DecorationType.None);
            this.txtPassword.ForeColor = System.Drawing.Color.White;
            this.txtPassword.Icon = ((System.Drawing.Image)(resources.GetObject("txtPassword.Icon")));
            this.txtPassword.Location = new System.Drawing.Point(921, 128);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(5);
            this.txtPassword.MaximumSize = new System.Drawing.Size(500, 126);
            this.txtPassword.MinimumSize = new System.Drawing.Size(125, 44);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(500, 44);
            this.txtPassword.TabIndex = 13;
            this.txtPassword.text = "Bunifu TextBox";
            // 
            // btnLogin
            // 
            this.btnLogin.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.btnLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLogin.BorderRadius = 0;
            this.btnLogin.ButtonText = "Login Raffels";
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.btnLogin, BunifuAnimatorNS.DecorationType.None);
            this.btnLogin.DisabledColor = System.Drawing.Color.Gray;
            this.btnLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnLogin.Iconcolor = System.Drawing.Color.Transparent;
            this.btnLogin.Iconimage = ((System.Drawing.Image)(resources.GetObject("btnLogin.Iconimage")));
            this.btnLogin.Iconimage_right = null;
            this.btnLogin.Iconimage_right_Selected = null;
            this.btnLogin.Iconimage_Selected = null;
            this.btnLogin.IconMarginLeft = 0;
            this.btnLogin.IconMarginRight = 0;
            this.btnLogin.IconRightVisible = true;
            this.btnLogin.IconRightZoom = 0D;
            this.btnLogin.IconVisible = true;
            this.btnLogin.IconZoom = 90D;
            this.btnLogin.IsTab = false;
            this.btnLogin.Location = new System.Drawing.Point(1117, 231);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(5);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.btnLogin.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
            this.btnLogin.OnHoverTextColor = System.Drawing.Color.White;
            this.btnLogin.selected = false;
            this.btnLogin.Size = new System.Drawing.Size(211, 50);
            this.btnLogin.TabIndex = 20;
            this.btnLogin.Text = "Login Raffels";
            this.btnLogin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogin.Textcolor = System.Drawing.Color.White;
            this.btnLogin.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // bunifuToggleRememberSwitch
            // 
            this.bunifuToggleRememberSwitch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.bunifuToggleRememberSwitch.BorderRadius = 0;
            this.bunifuToggleRememberSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.bunifuToggleRememberSwitch, BunifuAnimatorNS.DecorationType.None);
            this.bunifuToggleRememberSwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.bunifuToggleRememberSwitch.Location = new System.Drawing.Point(1181, 175);
            this.bunifuToggleRememberSwitch.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuToggleRememberSwitch.Name = "bunifuToggleRememberSwitch";
            this.bunifuToggleRememberSwitch.Oncolor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuToggleRememberSwitch.Onoffcolor = System.Drawing.Color.DarkGray;
            this.bunifuToggleRememberSwitch.Size = new System.Drawing.Size(0, 0);
            this.bunifuToggleRememberSwitch.TabIndex = 25;
            this.bunifuToggleRememberSwitch.Textcolor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.bunifuToggleRememberSwitch.Value = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.bunifuTransition_0.SetDecoration(this.label2, BunifuAnimatorNS.DecorationType.None);
            this.label2.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label2.Location = new System.Drawing.Point(923, 175);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(198, 25);
            this.label2.TabIndex = 26;
            this.label2.Text = "Save user credentials";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.bunifuTransition_0.SetDecoration(this.label3, BunifuAnimatorNS.DecorationType.None);
            this.label3.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label3.Location = new System.Drawing.Point(923, 210);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 25);
            this.label3.TabIndex = 28;
            this.label3.Text = "Auto Login";
            // 
            // bunifuToggleAutoLoginSwitch
            // 
            this.bunifuToggleAutoLoginSwitch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.bunifuToggleAutoLoginSwitch.BorderRadius = 0;
            this.bunifuToggleAutoLoginSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.bunifuToggleAutoLoginSwitch, BunifuAnimatorNS.DecorationType.None);
            this.bunifuToggleAutoLoginSwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.bunifuToggleAutoLoginSwitch.Location = new System.Drawing.Point(1032, 210);
            this.bunifuToggleAutoLoginSwitch.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuToggleAutoLoginSwitch.Name = "bunifuToggleAutoLoginSwitch";
            this.bunifuToggleAutoLoginSwitch.Oncolor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuToggleAutoLoginSwitch.Onoffcolor = System.Drawing.Color.DarkGray;
            this.bunifuToggleAutoLoginSwitch.Size = new System.Drawing.Size(0, 0);
            this.bunifuToggleAutoLoginSwitch.TabIndex = 27;
            this.bunifuToggleAutoLoginSwitch.Textcolor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.bunifuToggleAutoLoginSwitch.Value = true;
            // 
            // InfoList2
            // 
            this.bunifuTransition_0.SetDecoration(this.InfoList2, BunifuAnimatorNS.DecorationType.None);
            this.InfoList2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InfoList2.ForeColor = System.Drawing.Color.Maroon;
            this.InfoList2.Location = new System.Drawing.Point(692, 308);
            this.InfoList2.Margin = new System.Windows.Forms.Padding(4);
            this.InfoList2.Multiline = true;
            this.InfoList2.Name = "InfoList2";
            this.InfoList2.Size = new System.Drawing.Size(644, 138);
            this.InfoList2.TabIndex = 33;
            // 
            // bunifuProgressBar2
            // 
            this.bunifuProgressBar2.BackColor = System.Drawing.Color.Silver;
            this.bunifuProgressBar2.BorderRadius = 5;
            this.bunifuTransition_0.SetDecoration(this.bunifuProgressBar2, BunifuAnimatorNS.DecorationType.None);
            this.bunifuProgressBar2.Location = new System.Drawing.Point(683, 292);
            this.bunifuProgressBar2.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuProgressBar2.MaximumValue = 100;
            this.bunifuProgressBar2.Name = "bunifuProgressBar2";
            this.bunifuProgressBar2.ProgressColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuProgressBar2.Size = new System.Drawing.Size(675, 12);
            this.bunifuProgressBar2.TabIndex = 34;
            this.bunifuProgressBar2.Value = 0;
            // 
            // bunifuSeparator2
            // 
            this.bunifuSeparator2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bunifuSeparator2.BackColor = System.Drawing.Color.Transparent;
            this.bunifuTransition_0.SetDecoration(this.bunifuSeparator2, BunifuAnimatorNS.DecorationType.None);
            this.bunifuSeparator2.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator2.LineThickness = 1;
            this.bunifuSeparator2.Location = new System.Drawing.Point(681, 431);
            this.bunifuSeparator2.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuSeparator2.Name = "bunifuSeparator2";
            this.bunifuSeparator2.Size = new System.Drawing.Size(1484, 26);
            this.bunifuSeparator2.TabIndex = 36;
            this.bunifuSeparator2.Transparency = 255;
            this.bunifuSeparator2.Vertical = false;
            // 
            // bunifuProgressBar1
            // 
            this.bunifuProgressBar1.BackColor = System.Drawing.Color.Silver;
            this.bunifuProgressBar1.BorderRadius = 5;
            this.bunifuTransition_0.SetDecoration(this.bunifuProgressBar1, BunifuAnimatorNS.DecorationType.None);
            this.bunifuProgressBar1.Location = new System.Drawing.Point(680, 430);
            this.bunifuProgressBar1.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuProgressBar1.MaximumValue = 100;
            this.bunifuProgressBar1.Name = "bunifuProgressBar1";
            this.bunifuProgressBar1.ProgressColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuProgressBar1.Size = new System.Drawing.Size(676, 12);
            this.bunifuProgressBar1.TabIndex = 22;
            this.bunifuProgressBar1.Value = 0;
            // 
            // C2AA0
            // 
            this.bunifuTransition_0.SetDecoration(this.C2AA0, BunifuAnimatorNS.DecorationType.None);
            this.C2AA0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.C2AA0.Location = new System.Drawing.Point(949, 254);
            this.C2AA0.Margin = new System.Windows.Forms.Padding(4);
            this.C2AA0.Name = "C2AA0";
            this.C2AA0.Size = new System.Drawing.Size(123, 30);
            this.C2AA0.TabIndex = 208;
            this.C2AA0.Text = "Send Complaint";
            this.C2AA0.UseVisualStyleBackColor = true;
            this.C2AA0.Click += new System.EventHandler(this.C2AA0_Click);
            // 
            // bunifuToggleAppModeSwitch
            // 
            this.bunifuToggleAppModeSwitch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.bunifuToggleAppModeSwitch.BorderRadius = 0;
            this.bunifuToggleAppModeSwitch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.bunifuToggleAppModeSwitch, BunifuAnimatorNS.DecorationType.None);
            this.bunifuToggleAppModeSwitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.bunifuToggleAppModeSwitch.Location = new System.Drawing.Point(351, 71);
            this.bunifuToggleAppModeSwitch.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuToggleAppModeSwitch.Name = "bunifuToggleAppModeSwitch";
            this.bunifuToggleAppModeSwitch.Oncolor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuToggleAppModeSwitch.Onoffcolor = System.Drawing.Color.DarkGray;
            this.bunifuToggleAppModeSwitch.Size = new System.Drawing.Size(0, 0);
            this.bunifuToggleAppModeSwitch.TabIndex = 23;
            this.bunifuToggleAppModeSwitch.Textcolor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.bunifuToggleAppModeSwitch.Value = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.bunifuTransition_0.SetDecoration(this.label6, BunifuAnimatorNS.DecorationType.None);
            this.label6.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.label6.Location = new System.Drawing.Point(4, 71);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(337, 25);
            this.label6.TabIndex = 24;
            this.label6.Text = "Start ..Bull As Manager Base Window";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.bunifuTransition_0.SetDecoration(this.label5, BunifuAnimatorNS.DecorationType.None);
            this.label5.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(125, 5);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(453, 25);
            this.label5.TabIndex = 32;
            this.label5.Text = "(This Window Closes Automatically in 10 Seconds)";
            // 
            // bunifuButton1
            // 
            this.bunifuButton1.Activecolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.bunifuButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.bunifuButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bunifuButton1.BorderRadius = 0;
            this.bunifuButton1.ButtonText = "Proceed";
            this.bunifuButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.bunifuButton1, BunifuAnimatorNS.DecorationType.None);
            this.bunifuButton1.DisabledColor = System.Drawing.Color.Gray;
            this.bunifuButton1.ForeColor = System.Drawing.Color.White;
            this.bunifuButton1.Iconcolor = System.Drawing.Color.Transparent;
            this.bunifuButton1.Iconimage = ((System.Drawing.Image)(resources.GetObject("bunifuButton1.Iconimage")));
            this.bunifuButton1.Iconimage_right = null;
            this.bunifuButton1.Iconimage_right_Selected = null;
            this.bunifuButton1.Iconimage_Selected = null;
            this.bunifuButton1.IconMarginLeft = 0;
            this.bunifuButton1.IconMarginRight = 0;
            this.bunifuButton1.IconRightVisible = true;
            this.bunifuButton1.IconRightZoom = 0D;
            this.bunifuButton1.IconVisible = true;
            this.bunifuButton1.IconZoom = 90D;
            this.bunifuButton1.IsTab = false;
            this.bunifuButton1.Location = new System.Drawing.Point(460, 86);
            this.bunifuButton1.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuButton1.Name = "bunifuButton1";
            this.bunifuButton1.Normalcolor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.bunifuButton1.OnHovercolor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(129)))), ((int)(((byte)(77)))));
            this.bunifuButton1.OnHoverTextColor = System.Drawing.Color.White;
            this.bunifuButton1.selected = false;
            this.bunifuButton1.Size = new System.Drawing.Size(208, 50);
            this.bunifuButton1.TabIndex = 33;
            this.bunifuButton1.Text = "Proceed";
            this.bunifuButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuButton1.Textcolor = System.Drawing.Color.White;
            this.bunifuButton1.TextFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuButton1.Click += new System.EventHandler(this.bunifuButton1_Click);
            // 
            // chkLoginScreenShow
            // 
            this.chkLoginScreenShow.AutoSize = true;
            this.bunifuTransition_0.SetDecoration(this.chkLoginScreenShow, BunifuAnimatorNS.DecorationType.None);
            this.chkLoginScreenShow.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLoginScreenShow.ForeColor = System.Drawing.Color.White;
            this.chkLoginScreenShow.Location = new System.Drawing.Point(9, 114);
            this.chkLoginScreenShow.Margin = new System.Windows.Forms.Padding(4);
            this.chkLoginScreenShow.Name = "chkLoginScreenShow";
            this.chkLoginScreenShow.Size = new System.Drawing.Size(212, 20);
            this.chkLoginScreenShow.TabIndex = 36;
            this.chkLoginScreenShow.Text = "Don\'t Show This At Startup";
            this.chkLoginScreenShow.UseVisualStyleBackColor = true;
            this.chkLoginScreenShow.CheckedChanged += new System.EventHandler(this.chkLoginScreenShow_CheckedChanged);
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(49)))));
            this.pnlFooter.Controls.Add(this.chkLoginScreenShow);
            this.pnlFooter.Controls.Add(this.bunifuButton1);
            this.pnlFooter.Controls.Add(this.label5);
            this.pnlFooter.Controls.Add(this.label6);
            this.pnlFooter.Controls.Add(this.bunifuToggleAppModeSwitch);
            this.bunifuTransition_0.SetDecoration(this.pnlFooter, BunifuAnimatorNS.DecorationType.None);
            this.pnlFooter.Location = new System.Drawing.Point(685, 449);
            this.pnlFooter.Margin = new System.Windows.Forms.Padding(4);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(668, 144);
            this.pnlFooter.TabIndex = 35;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(22, 14, 28);
            this.panel1.Controls.Add(this.label50);
            this.panel1.Controls.Add(this.btnClose);
            this.bunifuTransition_0.SetDecoration(this.panel1, BunifuAnimatorNS.DecorationType.None);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(18)))));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1488, 28);
            this.panel1.TabIndex = 1213;
            // 
            // label50
            // 
            this.label50.BackColor = System.Drawing.Color.Transparent;
            this.bunifuTransition_0.SetDecoration(this.label50, BunifuAnimatorNS.DecorationType.None);
            this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label50.Location = new System.Drawing.Point(4, 2);
            this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(103, 25);
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
            this.btnClose.Location = new System.Drawing.Point(1454, 0);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(31, 28);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnClose.TabIndex = 1;
            this.btnClose.TabStop = false;
            this.btnClose.Zoom = 10;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // timer_0
            // 
            this.timer_0.Tick += new System.EventHandler(this.timer_0_Tick);
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 5;
            this.bunifuElipse1.TargetControl = this;
            // 
            // frmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1488, 786);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.B1AAA);
            this.Controls.Add(this.C2AA0);
            this.Controls.Add(this.B2AAA);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.bunifuProgressBar1);
            this.Controls.Add(this.bunifuSeparator2);
            this.Controls.Add(this.bunifuProgressBar2);
            this.Controls.Add(this.InfoList2);
            this.Controls.Add(this.C1AA0);
            this.Controls.Add(this.bunifuToggleAutoLoginSwitch);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bunifuToggleRememberSwitch);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.bunifuSeparator1);
            this.bunifuTransition_0.SetDecoration(this, BunifuAnimatorNS.DecorationType.None);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmLogin";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.B2AAA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.C1AA0)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    static frmLogin()
    {
        string text = "0.0.1";
        try
        {
            if (File.Exists("ver.txt"))
            {
                string vf = File.ReadAllText("ver.txt").Trim();
                if (vf.Length > 0) text = vf;
            }
        }
        catch { }
        appversion = text;
        Program.appversion = appversion;
        ENCkey = "";
        string text2 = "vfugejd7";
        key = text2;
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        base.Close();
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
    }
}
