using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using APP_FRAMEWORK;
using Bunifu.Framework.UI;
using NEXUS.Properties;
using SeleniumUndetectedChromeDriver;

namespace NEXUS.IRCommDLL;

public class frmAdharOTP : Form
{
    private static readonly IntPtr intptr_0 = new IntPtr(-1);

    internal string string_0;

    internal UndetectedChromeDriver driver;

    internal clsMain clsMain1;

    private string otp_;

    private int counter = 90;

    internal string Aadharnumber;

    private IContainer components;

    private BunifuElipse bunifuElipse_0;

    private Panel PanelHelder;

    private BunifuImageButton btnClose;

    internal Label tktname;

    private Button Submit;

    private TextBox textotp;

    private Label label1;

    private System.Windows.Forms.Timer timer1;

    private Button btnResend;

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetWindowPos(IntPtr intptr_1, IntPtr intptr_2, int int_0, int int_1, int int_2, int int_3, uint uint_0);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    internal static extern IntPtr GetFocus();

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool SetForegroundWindow(IntPtr intptr_1);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetParent(IntPtr intptr_1, IntPtr intptr_2);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetClassName(IntPtr intptr_1, StringBuilder stringBuilder_0, int int_0);

    public frmAdharOTP()
    {
        InitializeComponent();
        ApplyByteNovaTheme();
    }


    private void ApplyByteNovaTheme()
    {
        SuspendLayout();

        ClientSize = new Size(330, 176);
        BackColor = Color.FromArgb(3, 13, 29);
        ForeColor = Color.White;
        FormBorderStyle = FormBorderStyle.None;
        bunifuElipse_0.ElipseRadius = 9;

        PanelHelder.Dock = DockStyle.None;
        PanelHelder.SetBounds(4, 4, 322, 25);
        PanelHelder.BackColor = Color.FromArgb(3, 20, 40);
        PanelHelder.Paint -= ByteNovaHeaderPaint;
        PanelHelder.Paint += ByteNovaHeaderPaint;

        tktname.Dock = DockStyle.None;
        tktname.AutoSize = false;
        tktname.SetBounds(9, 2, 270, 21);
        tktname.ForeColor = Color.White;
        tktname.BackColor = Color.Transparent;
        tktname.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
        tktname.TextAlign = ContentAlignment.MiddleLeft;

        btnClose.SetBounds(293, 1, 26, 22);
        btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClose.BackColor = Color.Transparent;
        btnClose.Image = null;
        btnClose.ImageActive = null;
        btnClose.Zoom = 0;
        btnClose.Paint -= ByteNovaClosePaint;
        btnClose.Paint += ByteNovaClosePaint;
        btnClose.BringToFront();

        label1.AutoSize = false;
        label1.SetBounds(10, 42, 128, 28);
        label1.TextAlign = ContentAlignment.MiddleLeft;
        label1.ForeColor = Color.FromArgb(95, 230, 255);
        label1.BackColor = Color.Transparent;
        label1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

        textotp.SetBounds(145, 42, 175, 28);
        textotp.BackColor = Color.FromArgb(8, 34, 58);
        textotp.ForeColor = Color.White;
        textotp.BorderStyle = BorderStyle.FixedSingle;
        textotp.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        textotp.TextAlign = HorizontalAlignment.Center;

        Submit.SetBounds(216, 80, 104, 31);
        StyleByteNovaButton(Submit, Color.FromArgb(0, 126, 176));
        Submit.Text = "SUBMIT OTP";

        btnResend.SetBounds(10, 120, 310, 36);
        StyleByteNovaButton(btnResend, Color.FromArgb(92, 52, 178));
        btnResend.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

        Paint -= ByteNovaFormPaint;
        Paint += ByteNovaFormPaint;

        ResumeLayout(false);
        Invalidate();
    }

    private void StyleByteNovaButton(Button b, Color back)
    {
        if (b == null) return;
        b.BackColor = back;
        b.ForeColor = Color.White;
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 1;
        b.FlatAppearance.BorderColor = Color.FromArgb(198, 244, 50);
        b.FlatAppearance.MouseOverBackColor = ControlPaint.Light(back, .08F);
        b.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
        b.UseVisualStyleBackColor = false;
    }

    private void ByteNovaHeaderPaint(object sender, PaintEventArgs e)
    {
        Rectangle r = PanelHelder.ClientRectangle;
        if (r.Width <= 0 || r.Height <= 0) return;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (LinearGradientBrush br = new LinearGradientBrush(
            r, Color.FromArgb(3, 27, 52), Color.FromArgb(64, 35, 116),
            LinearGradientMode.Horizontal))
            e.Graphics.FillRectangle(br, r);
        using (Pen p = new Pen(Color.FromArgb(198, 244, 50), 1F))
            e.Graphics.DrawRectangle(p, 0, 0, r.Width - 1, r.Height - 1);
    }

    private void ByteNovaClosePaint(object sender, PaintEventArgs e)
    {
        Rectangle r = btnClose.ClientRectangle;
        if (r.Width <= 0 || r.Height <= 0) return;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (LinearGradientBrush br = new LinearGradientBrush(
            r, Color.FromArgb(205, 32, 48), Color.FromArgb(105, 8, 24),
            LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(br, r);
        using (Pen p = new Pen(Color.White, 1.8F))
        {
            e.Graphics.DrawLine(p, 8, 6, r.Width - 8, r.Height - 6);
            e.Graphics.DrawLine(p, r.Width - 8, 6, 8, r.Height - 6);
        }
    }

    private void ByteNovaFormPaint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (Pen outer = new Pen(Color.FromArgb(198, 244, 50), 1.2F))
            e.Graphics.DrawRectangle(outer, 1, 1, ClientSize.Width - 3, ClientSize.Height - 3);

        DrawByteNovaCard(e.Graphics, new Rectangle(6, 35, 318, 40), Color.FromArgb(198, 244, 50));
        DrawByteNovaCard(e.Graphics, new Rectangle(6, 116, 318, 44), Color.FromArgb(86, 92, 255));
    }

    private void DrawByteNovaCard(Graphics g, Rectangle r, Color borderColor)
    {
        using (GraphicsPath path = ByteNovaRoundedRect(r, 8))
        using (SolidBrush fill = new SolidBrush(Color.FromArgb(7, 35, 61)))
        using (Pen border = new Pen(borderColor, 1F))
        {
            g.FillPath(fill, path);
            g.DrawPath(border, path);
        }
    }

    private GraphicsPath ByteNovaRoundedRect(Rectangle r, int radius)
    {
        GraphicsPath p = new GraphicsPath();
        int d = radius * 2;
        p.AddArc(r.Left, r.Top, d, d, 180, 90);
        p.AddArc(r.Right - d, r.Top, d, d, 270, 90);
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        p.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
        p.CloseFigure();
        return p;
    }

    internal void method_0(Control control_0, Rectangle rectangle_0)
    {
        _ = control_0.Controls[0];
        base.Size = new Size(330, 176);
        Control control = control_0.Parent;
        base.StartPosition = FormStartPosition.Manual;
        base.Location = control.Location;
        base.TopMost = true;
    }

    private void ReadOTP(object j)
    {
        int i = 0;
        string text = "https://novabyte.in/Api/getuserotp";
        string requestUriString = text;
        string[] array = new string[5];
        string text2 = "{\"Edata\":\"";
        array[0] = text2;
        array[1] = Staticm.edata;
        string text3 = "\", \"LastChar\":\"";
        array[2] = text3;
        array[3] = Aadharnumber;
        string text4 = "\"}";
        array[4] = text4;
        string text5 = string.Concat(array);
        while (true)
        {
            try
            {
                i++;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                string contentType = "application/json";
                httpWebRequest.ContentType = contentType;
                string method = "POST";
                httpWebRequest.Method = method;
                httpWebRequest.Timeout = 60000;
                httpWebRequest.ReadWriteTimeout = 60000;
                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string value = text5;
                    streamWriter.Write(value);
                }
                using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
                string input = streamReader.ReadToEnd();
                string pattern = "\\b\\d{6}\\b";
                MatchCollection matchCollection = Regex.Matches(input, pattern);
                if (matchCollection.Count > 0)
                {
                    string_0 = matchCollection[0].Value;
                    break;
                }
                if (i > 20)
                {
                    string_0 = "";
                    break;
                }
                Application.DoEvents();
                Thread.Sleep(500);
            }
            catch (WebException)
            {
                Application.DoEvents();
                Thread.Sleep(500);
            }
        }
        if (string_0.Length == 6)
        {
            BeginInvoke((Action)delegate
            {
                VerifyOTP();
            });
        }
    }

    private void VerifyOTP()
    {
        textotp.Text = string_0;
        if (string_0.Length == 6)
        {
            base.DialogResult = DialogResult.OK;
            Close();
        }
        else
        {
            string text = "Enter Correct OTP";
            MessageBox.Show(text);
        }
    }

    internal void method_2(Control control_0, Rectangle rectangle_0, string string_2)
    {
        try
        {
            Label label = label1;
            string text = "Fetching OTP...";
            label.Text = text;
            ThreadPool.QueueUserWorkItem(ReadOTP);
            tktname.Text = string_2;
            method_0(control_0, rectangle_0);
            SetWindowPos(base.Handle, intptr_0, 0, 0, 0, 0, 3u);
            StartCountdown();
            base.TopMost = true;
            ShowDialog();
        }
        catch (ThreadAbortException)
        {
            Thread.ResetAbort();
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        base.DialogResult = DialogResult.Cancel;
        Close();
    }

    private void StartCountdown()
    {
        counter = 90;
        btnResend.Enabled = false;
        Button button = btnResend;
        string text = "Resend OTP in ";
        string text2 = counter.ToString();
        string text3 = " Second";
        button.Text = text + text2 + text3;
        timer1.Interval = 1000;
        timer1.Start();
    }

    private void Submit_Click(object sender, EventArgs e)
    {
        string_0 = textotp.Text.Trim();
        if (string_0.Length == 6)
        {
            base.DialogResult = DialogResult.OK;
            Close();
        }
        else
        {
            string text = "Enter Correct OTP";
            MessageBox.Show(text);
        }
    }

    private void textotp_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
        {
            e.Handled = true;
        }
    }

    private void btnResend_Click(object sender, EventArgs e)
    {
        if (!clsMain1.ResendOTP())
        {
            string text = "ReSend Faield Retry";
            MessageBox.Show(text);
        }
        else
        {
            StartCountdown();
        }
    }

    private void timer1_Tick_1(object sender, EventArgs e)
    {
        counter--;
        if (counter > 0)
        {
            Button button = btnResend;
            string text = "Resend OTP in ";
            string text2 = counter.ToString();
            string text3 = " Second";
            button.Text = text + text2 + text3;
        }
        else
        {
            timer1.Stop();
            btnResend.Enabled = true;
            Button button2 = btnResend;
            string text4 = "ReSend OTP";
            button2.Text = text4;
        }
    }

    private void textotp_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Return)
        {
            string_0 = textotp.Text.Trim();
            if (string_0.Length != 6)
            {
                string text = "Enter Correct OTP";
                MessageBox.Show(text);
            }
            else
            {
                base.DialogResult = DialogResult.OK;
                Close();
            }
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAdharOTP));
        this.bunifuElipse_0 = new Bunifu.Framework.UI.BunifuElipse(this.components);
        this.PanelHelder = new System.Windows.Forms.Panel();
        this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
        this.tktname = new System.Windows.Forms.Label();
        this.label1 = new System.Windows.Forms.Label();
        this.textotp = new System.Windows.Forms.TextBox();
        this.Submit = new System.Windows.Forms.Button();
        this.timer1 = new System.Windows.Forms.Timer(this.components);
        this.btnResend = new System.Windows.Forms.Button();
        this.PanelHelder.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
        this.SuspendLayout();
        // 
        // bunifuElipse_0
        // 
        this.bunifuElipse_0.ElipseRadius = 5;
        this.bunifuElipse_0.TargetControl = this;
        // 
        // PanelHelder
        // 
        this.PanelHelder.BackColor = System.Drawing.Color.DarkGray;
        this.PanelHelder.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.PanelHelder.Controls.Add(this.btnClose);
        this.PanelHelder.Controls.Add(this.tktname);
        this.PanelHelder.Dock = System.Windows.Forms.DockStyle.Top;
        this.PanelHelder.ForeColor = System.Drawing.Color.Yellow;
        this.PanelHelder.Location = new System.Drawing.Point(0, 0);
        this.PanelHelder.Margin = new System.Windows.Forms.Padding(2);
        this.PanelHelder.Name = "PanelHelder";
        this.PanelHelder.Size = new System.Drawing.Size(225, 15);
        this.PanelHelder.TabIndex = 129;
        // 
        // btnClose
        // 
        this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnClose.BackColor = System.Drawing.Color.Transparent;
        this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
        this.btnClose.ImageActive = null;
        this.btnClose.Location = new System.Drawing.Point(208, 0);
        this.btnClose.Margin = new System.Windows.Forms.Padding(2);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(15, 15);
        this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.btnClose.TabIndex = 25;
        this.btnClose.TabStop = false;
        this.btnClose.Zoom = 10;
        this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
        // 
        // tktname
        // 
        this.tktname.AutoSize = true;
        this.tktname.BackColor = System.Drawing.Color.Transparent;
        this.tktname.Dock = System.Windows.Forms.DockStyle.Left;
        this.tktname.ForeColor = System.Drawing.Color.Black;
        this.tktname.Location = new System.Drawing.Point(0, 0);
        this.tktname.Name = "tktname";
        this.tktname.Size = new System.Drawing.Size(56, 13);
        this.tktname.TabIndex = 23;
        this.tktname.Text = "Tikt Name";
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(8, 28);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(105, 13);
        this.label1.TabIndex = 130;
        this.label1.Text = "Enter AADHAR OTP";
        // 
        // textotp
        // 
        this.textotp.Location = new System.Drawing.Point(128, 24);
        this.textotp.Name = "textotp";
        this.textotp.Size = new System.Drawing.Size(85, 20);
        this.textotp.TabIndex = 131;
        this.textotp.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textotp_KeyDown);
        this.textotp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textotp_KeyPress);
        // 
        // Submit
        // 
        this.Submit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.Submit.Location = new System.Drawing.Point(128, 51);
        this.Submit.Name = "Submit";
        this.Submit.Size = new System.Drawing.Size(85, 23);
        this.Submit.TabIndex = 132;
        this.Submit.Text = "Submit OTP";
        this.Submit.UseVisualStyleBackColor = true;
        this.Submit.Click += new System.EventHandler(this.Submit_Click);
        // 
        // timer1
        // 
        this.timer1.Tick += new System.EventHandler(this.timer1_Tick_1);
        // 
        // btnResend
        // 
        this.btnResend.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        this.btnResend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnResend.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnResend.ForeColor = System.Drawing.Color.White;
        this.btnResend.Location = new System.Drawing.Point(11, 90);
        this.btnResend.Name = "btnResend";
        this.btnResend.Size = new System.Drawing.Size(201, 26);
        this.btnResend.TabIndex = 133;
        this.btnResend.Text = "ReSend OTP";
        this.btnResend.UseVisualStyleBackColor = false;
        this.btnResend.Click += new System.EventHandler(this.btnResend_Click);
        // 
        // frmAdharOTP
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(225, 156);
        this.Controls.Add(this.btnResend);
        this.Controls.Add(this.Submit);
        this.Controls.Add(this.textotp);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.PanelHelder);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.Name = "frmAdharOTP";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.Text = "frmAdharOTP";
        this.TopMost = true;
        this.PanelHelder.ResumeLayout(false);
        this.PanelHelder.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }
}
