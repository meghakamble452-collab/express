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
using Bunifu.Framework.UI;
using IRCommDLL;

public class frmOTP : Form
{
    private string data_;

    internal string string_0;

    internal string string_1;

    private bool bool_0;

    private bool bool_1;

    private static readonly IntPtr intptr_0 = new IntPtr(-1);

    private IContainer icontainer_0;

    private BunifuElipse bunifuElipse_0;

    private Label label1;

    private TextBox textotp;

    private Button Submit;

    private Panel pnlHeader;

    internal Label tktname;

    private BunifuImageButton btnCancel;

    private IContainer components;

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

    public frmOTP()
    {
        InitializeComponent();
        ApplyByteNovaTheme();
    }

    internal void method_0(Control control_0, Rectangle rectangle_0)
    {
        _ = control_0.Controls[0];
        base.Size = new Size(330, 118);
        Control control = control_0.Parent;
        base.StartPosition = FormStartPosition.Manual;
        base.Location = control.Location;
        base.TopMost = true;
    }

    internal void method_1(Control control_0, Rectangle rectangle_0, bool bool_2, string string_2, string string_3)
    {
        try
        {
            string text = string_3.Split('_')[0];
            string value = "AIRTELMONEY";
            if (!text.Contains(value))
            {
                string value2 = "PAYTM";
                if (!text.Contains(value2))
                {
                    string value3 = "iMudra";
                    if (!text.Contains(value3))
                    {
                        string variable = "clientID";
                        string_1 = Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Process);
                        goto IL_0078;
                    }
                }
            }
            clsMain.dictionary_7.TryGetValue(string_3, out string_1);
            goto IL_0078;
        IL_0078:
            if (!bool_2)
            {
                Label label = label1;
                string text2 = "Enter OTP From Mobile";
                label.Text = text2;
            }
            else
            {
                Label label2 = label1;
                string text3 = "Getting OTP ! Please Wait...";
                label2.Text = text3;
                string value4 = "IRCTC Login OTP";
                if (string_2.Contains(value4))
                {
                    ThreadPool.QueueUserWorkItem(method_30, string_3);
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(method_3, string_3);
                }
            }
            tktname.Text = string_2;
            method_0(control_0, rectangle_0);
            SetWindowPos(base.Handle, intptr_0, 0, 0, 0, 0, 3u);
            ShowDialog();
        }
        catch (ThreadAbortException)
        {
            Thread.ResetAbort();
        }
    }

    internal void method_2(Control control_0, Rectangle rectangle_0, string string_2)
    {
        try
        {
            tktname.Text = string_2;
            Label label = label1;
            string text = "Enter ADHAR OTP";
            label.Text = text;
            method_0(control_0, rectangle_0);
            SetWindowPos(base.Handle, intptr_0, 0, 0, 0, 0, 3u);
            ShowDialog();
        }
        catch (ThreadAbortException)
        {
            Thread.ResetAbort();
        }
    }

    private void method_30(object object_0)
    {
        string text = data_;
        string[] array = new string[1];
        string text2 = "|";
        array[0] = text2;
        string[] array2 = text.Split(array, StringSplitOptions.None);
        string text3 = array2[0];
        string value = "@gmail.com";
        if (!text3.Contains(value))
        {
            MailNet2 mailNet = new MailNet2();
            textotp.Text = mailNet.FetchEmailsByAccount(array2[0], array2[1]);
        }
        else
        {
            MailNet mailNet2 = new MailNet();
            textotp.Text = mailNet2.getotp(array2[0], array2[1]);
        }
        Submit_Click(null, null);
    }

    private void method_3(object object_0)
    {
        try
        {
            string text = "";
            string text2 = object_0.ToString();
            string[] array = new string[1];
            string text3 = "_";
            array[0] = text3;
            string[] array2 = text2.Split(array, StringSplitOptions.None);
            string text4 = array2[0];
            string text5 = "MOBIKWIK";
            if (text4 == text5)
            {
                string text6 = "MOBIKW";
                text = text6;
            }
            else
            {
                string text7 = array2[0];
                string text8 = "iMudra";
                if (text7 == text8)
                {
                    string text9 = "IRCTCm";
                    text = text9;
                }
                else
                {
                    string text10 = array2[0];
                    string text11 = "OLAMONEY";
                    if (!(text10 == text11))
                    {
                        string text12 = array2[0];
                        string text13 = "JIOMONEY";
                        if (!(text12 == text13))
                        {
                            string text14 = array2[0];
                            string text15 = "AIRTELMONEY";
                            if (!(text14 == text15))
                            {
                                string text16 = array2[0];
                                string text17 = "IRCTC";
                                if (!(text16 == text17))
                                {
                                    string text18 = array2[0];
                                    string text19 = "PAYTM";
                                    if (!(text18 == text19))
                                    {
                                        string text20 = array2[0];
                                        string text21 = "SBI";
                                        if (text20 == text21)
                                        {
                                            string text22 = "SBIINB";
                                            text = text22;
                                        }
                                        else
                                        {
                                            string text23 = array2[0];
                                            string text24 = "IRCTC Login OTP";
                                            if (text23 == text24)
                                            {
                                                MailNet2 mailNet = new MailNet2();
                                                textotp.Text = mailNet.FetchEmailsByAccount("", "");
                                                Submit_Click(null, null);
                                                return;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string text25 = "iPaytm";
                                        text = text25;
                                    }
                                }
                                else
                                {
                                    string text26 = "IRCTCi";
                                    text = text26;
                                }
                            }
                            else
                            {
                                string text27 = "AIRBNK";
                                text = text27;
                            }
                        }
                        else
                        {
                            string text28 = "JIOMNY";
                            text = text28;
                        }
                    }
                    else
                    {
                        string text29 = "OLAMNY";
                        text = text29;
                    }
                }
            }
            string variable = "mac";
            string environmentVariable = Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Process);
            string variable2 = "clientID";
            string environmentVariable2 = Environment.GetEnvironmentVariable(variable2, EnvironmentVariableTarget.Process);
            string[] array3 = new string[6];
            string text30 = "https://novabyte.in/Api/msg_read.php?username=";
            array3[0] = text30;
            array3[1] = environmentVariable2;
            string text31 = "&mac=";
            array3[2] = text31;
            array3[3] = environmentVariable;
            string text32 = "&mobile=9999999999&bank=";
            array3[4] = text32;
            array3[5] = text;
            string address = string.Concat(array3);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            while (true)
            {
                if (bool_1)
                {
                    return;
                }
                WebClient webClient = new WebClient();
                WebHeaderCollection headers = webClient.Headers;
                string value = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/66.0.3359.181 Safari/537.36";
                headers.Add(HttpRequestHeader.UserAgent, value);
                string text33 = new StreamReader(webClient.OpenRead(address)).ReadToEnd();
                string value2 = "OTP";
                if (text33.Contains(value2) && text33.Length > 20)
                {
                    string text34 = text;
                    switch (Class46.smethod_0(text34))
                    {
                        case 534156456u:
                            {
                                string text38 = "AIRBNK";
                                if (text34 == text38)
                                {
                                    textotp.Text = method_4(text33, 6);
                                }
                                break;
                            }
                        case 158865325u:
                            {
                                string text43 = "JIOMNY";
                                if (text34 == text43)
                                {
                                    string value6 = "OTP is";
                                    string text44 = text33.Substring(text33.IndexOf(value6) + 7);
                                    TextBox textBox2 = textotp;
                                    string value7 = ".";
                                    textBox2.Text = text44.Substring(0, text44.IndexOf(value7));
                                }
                                break;
                            }
                        case 1678934222u:
                            {
                                string text47 = "Global";
                                if (text34 == text47)
                                {
                                    text33.Substring(0, 6);
                                    textotp.Text = text33.Substring(0, 6);
                                }
                                break;
                            }
                        case 1135293582u:
                            {
                                string text39 = "SBIINB";
                                if (text34 == text39)
                                {
                                    string value4 = "BOOKING is ";
                                    string text40 = text33.Substring(text33.IndexOf(value4) + 11);
                                    textotp.Text = text40.Substring(0, 8);
                                }
                                break;
                            }
                        case 2152469133u:
                            {
                                string text37 = "IRCTCm";
                                if (text34 == text37)
                                {
                                    textotp.Text = method_4(text33, 6);
                                }
                                break;
                            }
                        case 2085358657u:
                            {
                                string text45 = "IRCTCi";
                                if (text34 == text45)
                                {
                                    string value8 = "LOGIN : ";
                                    string text46 = text33.Substring(text33.IndexOf(value8) + 8);
                                    textotp.Text = text46.Substring(0, 6);
                                }
                                break;
                            }
                        case 3669580657u:
                            {
                                string text41 = "OLAMNY";
                                if (text34 == text41)
                                {
                                    string value5 = "OTP. \n\n";
                                    string text42 = text33.Substring(text33.IndexOf(value5) + 7);
                                    textotp.Text = text42.Substring(0, 6);
                                }
                                break;
                            }
                        case 2999904165u:
                            {
                                string text35 = "iPaytm";
                                if (text34 == text35)
                                {
                                    string value3 = "Updated Balance";
                                    if (!text33.Contains(value3))
                                    {
                                        textotp.Text = method_4(text33, 6);
                                        break;
                                    }
                                    TextBox textBox = textotp;
                                    string text36 = "00000";
                                    textBox.Text = text36;
                                }
                                break;
                            }
                    }
                }
                if (textotp.Text.Length > 5)
                {
                    break;
                }
                Thread.Sleep(1000);
            }
            Submit_Click(null, null);
        }
        catch (Exception)
        {
        }
    }

    private void ApplyByteNovaTheme()
    {
        SuspendLayout();

        ClientSize = new Size(330, 118);
        BackColor = Color.FromArgb(3, 13, 29);
        ForeColor = Color.White;
        FormBorderStyle = FormBorderStyle.None;
        bunifuElipse_0.ElipseRadius = 9;

        // ByteNova compact header - same visual family as frmCaptcha.
        pnlHeader.Dock = DockStyle.None;
        pnlHeader.SetBounds(4, 4, 322, 25);
        pnlHeader.BackColor = Color.FromArgb(3, 20, 40);
        pnlHeader.Paint -= ByteNovaHeaderPaint;
        pnlHeader.Paint += ByteNovaHeaderPaint;

        tktname.AutoSize = false;
        tktname.SetBounds(9, 2, 270, 21);
        tktname.ForeColor = Color.White;
        tktname.BackColor = Color.Transparent;
        tktname.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
        tktname.TextAlign = ContentAlignment.MiddleLeft;

        btnCancel.SetBounds(293, 1, 26, 22);
        btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancel.BackColor = Color.Transparent;
        btnCancel.Image = null;
        btnCancel.ImageActive = null;
        btnCancel.Zoom = 0;
        btnCancel.Paint -= ByteNovaClosePaint;
        btnCancel.Paint += ByteNovaClosePaint;
        btnCancel.BringToFront();

        label1.AutoSize = false;
        label1.SetBounds(10, 37, 310, 20);
        label1.ForeColor = Color.White;
        label1.BackColor = Color.Transparent;
        label1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        label1.TextAlign = ContentAlignment.MiddleLeft;

        textotp.SetBounds(10, 66, 200, 31);
        textotp.BackColor = Color.White;
        textotp.ForeColor = Color.Black;
        textotp.BorderStyle = BorderStyle.FixedSingle;
        textotp.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        textotp.TextAlign = HorizontalAlignment.Center;

        Submit.SetBounds(218, 65, 102, 33);
        Submit.BackColor = Color.FromArgb(0, 126, 176);
        Submit.ForeColor = Color.White;
        Submit.FlatStyle = FlatStyle.Flat;
        Submit.FlatAppearance.BorderSize = 1;
        Submit.FlatAppearance.BorderColor = Color.FromArgb(198, 244, 50);
        Submit.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 150, 205);
        Submit.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        Submit.UseVisualStyleBackColor = false;

        Paint -= ByteNovaFormPaint;
        Paint += ByteNovaFormPaint;

        ResumeLayout(false);
        Invalidate();
    }

    private void ByteNovaHeaderPaint(object sender, PaintEventArgs e)
    {
        Rectangle r = pnlHeader.ClientRectangle;
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
        Rectangle r = btnCancel.ClientRectangle;
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
    }

    private void frmOTP_Load(object sender, EventArgs e)
    {
    }

    private string method_4(string string_2, int int_0)
    {
        string result = "";
        try
        {
            string pattern = "\\D+";
            string[] array = Regex.Split(string_2, pattern);
            string[] array2 = array;
            foreach (string text in array2)
            {
                if (!string.IsNullOrEmpty(text) && text.Length == int_0)
                {
                    result = text;
                }
            }
            return result;
        }
        catch
        {
            return result;
        }
    }

    private void Submit_Click(object sender, EventArgs e)
    {
        if (bool_0)
        {
            bool_1 = true;
            string_0 = textotp.Text.Trim();
            base.DialogResult = DialogResult.OK;
            Close();
        }
        else if (textotp.Text.Length >= 5)
        {
            bool_1 = true;
            string_0 = textotp.Text.Trim();
            base.DialogResult = DialogResult.OK;
            Close();
        }
        else
        {
            Label label = label1;
            string text = "Enter Valid OTP";
            label.Text = text;
            textotp.Text = null;
            textotp.Focus();
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        bool_1 = true;
        base.DialogResult = DialogResult.Cancel;
        Close();
    }

    private void textotp_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!bool_0)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOTP));
        this.bunifuElipse_0 = new Bunifu.Framework.UI.BunifuElipse(this.components);
        this.Submit = new System.Windows.Forms.Button();
        this.textotp = new System.Windows.Forms.TextBox();
        this.label1 = new System.Windows.Forms.Label();
        this.pnlHeader = new System.Windows.Forms.Panel();
        this.tktname = new System.Windows.Forms.Label();
        this.btnCancel = new Bunifu.Framework.UI.BunifuImageButton();
        this.pnlHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
        this.SuspendLayout();
        // 
        // bunifuElipse_0
        // 
        this.bunifuElipse_0.ElipseRadius = 5;
        this.bunifuElipse_0.TargetControl = this;
        // 
        // Submit
        // 
        this.Submit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
        this.Submit.Location = new System.Drawing.Point(109, 43);
        this.Submit.Margin = new System.Windows.Forms.Padding(2);
        this.Submit.Name = "Submit";
        this.Submit.Size = new System.Drawing.Size(70, 23);
        this.Submit.TabIndex = 0;
        this.Submit.Text = "Submit";
        this.Submit.UseVisualStyleBackColor = true;
        this.Submit.Click += new System.EventHandler(this.Submit_Click);
        // 
        // textotp
        // 
        this.textotp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.textotp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.textotp.Location = new System.Drawing.Point(6, 44);
        this.textotp.Margin = new System.Windows.Forms.Padding(2);
        this.textotp.Name = "textotp";
        this.textotp.Size = new System.Drawing.Size(90, 23);
        this.textotp.TabIndex = 1;
        this.textotp.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textotp_KeyPress);
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label1.ForeColor = System.Drawing.Color.Black;
        this.label1.Location = new System.Drawing.Point(4, 23);
        this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(41, 13);
        this.label1.TabIndex = 2;
        this.label1.Text = "label1";
        // 
        // pnlHeader
        // 
        this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(22, 14, 28);
        this.pnlHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.pnlHeader.Controls.Add(this.tktname);
        this.pnlHeader.Controls.Add(this.btnCancel);
        this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlHeader.Location = new System.Drawing.Point(0, 0);
        this.pnlHeader.Margin = new System.Windows.Forms.Padding(2);
        this.pnlHeader.Name = "pnlHeader";
        this.pnlHeader.Size = new System.Drawing.Size(182, 18);
        this.pnlHeader.TabIndex = 1191;
        // 
        // tktname
        // 
        this.tktname.AutoSize = true;
        this.tktname.BackColor = System.Drawing.Color.Transparent;
        this.tktname.ForeColor = System.Drawing.Color.White;
        this.tktname.Location = new System.Drawing.Point(-1, 1);
        this.tktname.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.tktname.Name = "tktname";
        this.tktname.Size = new System.Drawing.Size(45, 13);
        this.tktname.TabIndex = 1189;
        this.tktname.Text = "tktname";
        // 
        // btnCancel
        // 
        this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnCancel.BackColor = System.Drawing.Color.Transparent;
        this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
        this.btnCancel.ImageActive = null;
        this.btnCancel.Location = new System.Drawing.Point(163, -1);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(15, 15);
        this.btnCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.btnCancel.TabIndex = 1188;
        this.btnCancel.TabStop = false;
        this.btnCancel.Zoom = 10;
        this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
        // 
        // frmOTP
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
        this.BackColor = System.Drawing.Color.White;
        this.ClientSize = new System.Drawing.Size(330, 118);
        this.Controls.Add(this.pnlHeader);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.textotp);
        this.Controls.Add(this.Submit);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.Margin = new System.Windows.Forms.Padding(2);
        this.Name = "frmOTP";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.Text = "frmOTP";
        this.TopMost = true;
        this.Load += new System.EventHandler(this.frmOTP_Load);
        this.pnlHeader.ResumeLayout(false);
        this.pnlHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }
}
