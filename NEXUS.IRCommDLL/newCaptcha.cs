using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Windows.Forms;
using APP_FRAMEWORK;
using Bunifu.Framework.UI;
using Ionic.Zip;
using NEXUS.Properties;

namespace NEXUS.IRCommDLL;

public class newCaptcha : Form
{
    private delegate string Delegate2(byte[] byte_0, int int_0);

    internal clsMain clsMain1;

    private Bitmap bitmap_0;

    private string string_0;

    private string base64inage;

    private bool bool_9;

    private bool bool_8;

    protected static readonly int int_5;

    private static readonly IntPtr intptr_1;

    private StringBuilder stringBuilder_0 = new StringBuilder(256);

    protected static readonly string string_15;

    private object lockObject = new object();

    private Control control_0;

    private IntPtr intptr_0 = IntPtr.Zero;

    private string string_5;

    private CookieContainer cookieContainer_0;

    private string string_6;

    private clsMain class28_0;

    private string string_7;

    private IContainer components;

    private BunifuElipse bunifuElipse_0;

    private Panel PanelHelder;

    private BunifuImageButton btnClose;

    internal Label tktname;

    private PictureBox picOriginal;

    private new Button Close;

    private Button btnSubmit;

    internal TextBox txtImageData;

    private Button btnReferesh;

    [DllImport("kernel32.dll")]
    internal static extern void un(uint uint_0);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool SetWindowPos(IntPtr intptr_5, IntPtr intptr_6, int int_6, int int_7, int int_8, int int_9, uint uint_3);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    internal static extern IntPtr GetFocus();

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool SetForegroundWindow(IntPtr intptr_5);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetParent(IntPtr intptr_5, IntPtr intptr_6);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetClassName(IntPtr intptr_5, StringBuilder stringBuilder_1, int int_6);

    public newCaptcha()
    {
        InitializeComponent();
        ApplyByteNovaTheme();
    }

    internal newCaptcha(clsMain class28_1, CookieContainer cookieContainer_1, string string_16, string string_17)
    {
        InitializeComponent();
        ApplyByteNovaTheme();
        Control.CheckForIllegalCrossThreadCalls = false;
        string_5 = string_16;
        cookieContainer_0 = cookieContainer_1;
        string_6 = string_17;
        class28_0 = class28_1;
        string text = "";
        string_7 = text;
        tktname.Text = class28_0.TktName_;
    }


    private void ApplyByteNovaTheme()
    {
        SuspendLayout();

        // Same compact footprint as the approved TCntrl.
        ClientSize = new Size(330, 176);
        BackColor = Color.FromArgb(3, 13, 29);
        ForeColor = Color.White;
        FormBorderStyle = FormBorderStyle.None;
        bunifuElipse_0.ElipseRadius = 9;

        PanelHelder.Dock = DockStyle.None;
        PanelHelder.SetBounds(4, 4, 322, 25);
        PanelHelder.BackColor = Color.FromArgb(3, 20, 40);
        PanelHelder.ForeColor = Color.White;
        PanelHelder.Paint -= ByteNovaHeaderPaint;
        PanelHelder.Paint += ByteNovaHeaderPaint;

        tktname.Dock = DockStyle.None;
        tktname.AutoSize = false;
        tktname.SetBounds(9, 2, 270, 21);
        tktname.BackColor = Color.Transparent;
        tktname.ForeColor = Color.White;
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
        btnClose.Click -= Close_Click;
        btnClose.Click += Close_Click;
        btnClose.BringToFront();

        picOriginal.Dock = DockStyle.None;
        picOriginal.SetBounds(8, 35, 314, 64);
        picOriginal.BackColor = Color.FromArgb(8, 34, 58);
        picOriginal.BorderStyle = BorderStyle.None;
        picOriginal.SizeMode = PictureBoxSizeMode.Zoom;

        txtImageData.SetBounds(8, 106, 314, 25);
        txtImageData.BackColor = Color.FromArgb(8, 34, 58);
        txtImageData.ForeColor = Color.White;
        txtImageData.BorderStyle = BorderStyle.FixedSingle;
        txtImageData.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        txtImageData.TextAlign = HorizontalAlignment.Center;

        Close.SetBounds(8, 139, 92, 29);
        StyleByteNovaButton(Close, Color.FromArgb(180, 38, 55));
        Close.Text = "CANCEL";

        btnReferesh.SetBounds(106, 139, 104, 29);
        StyleByteNovaButton(btnReferesh, Color.FromArgb(88, 52, 180));
        btnReferesh.Text = "REFRESH";
        btnReferesh.Click -= btnReferesh_Click;
        btnReferesh.Click += btnReferesh_Click;

        btnSubmit.SetBounds(216, 139, 106, 29);
        StyleByteNovaButton(btnSubmit, Color.FromArgb(0, 126, 176));
        btnSubmit.Text = "SUBMIT";

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
        b.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
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

        DrawByteNovaCard(e.Graphics, new Rectangle(5, 32, 320, 70), Color.FromArgb(198, 244, 50));
        DrawByteNovaCard(e.Graphics, new Rectangle(5, 103, 320, 31), Color.FromArgb(74, 105, 255));
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

    internal Image method_7(string string_16)
    {
        byte[] array = Convert.FromBase64String(string_16);
        using MemoryStream stream = new MemoryStream(array, 0, array.Length);
        return Image.FromStream(stream, useEmbeddedColorManagement: true);
    }

    internal void method_4(Control control_1, Rectangle rectangle_0)
    {
        _ = control_1.Controls[0];
        Control control = control_1.Parent;
        base.Width = rectangle_0.Width;
        base.StartPosition = FormStartPosition.Manual;
        base.Location = control.Location;
        base.Left += 2;
        base.Height += -6;
        base.TopMost = true;
    }

    private void method_3()
    {
        try
        {
            intptr_0 = GetForegroundWindow();
            if (intptr_0 != IntPtr.Zero)
            {
                GetClassName(intptr_0, stringBuilder_0, stringBuilder_0.Capacity);
                control_0 = Control.FromHandle(intptr_0);
            }
        }
        catch
        {
            intptr_0 = IntPtr.Zero;
        }
    }

    internal void method_2(Control control_1, Rectangle rectangle_0, string string_16)
    {
        try
        {
            string_0 = string_16;
            base64inage = string_16;
            method_4(control_1, rectangle_0);
        }
        catch (Exception)
        {
        }
        try
        {
            if (class28_0.isFinalCaptcha)
            {
                bitmap_0 = new Bitmap(method_7(string_0));
                method_3();
                SetWindowPos(base.Handle, intptr_1, 0, 0, 0, 0, 3u);
                method_6(null);
                btnSubmit.Enabled = true;
                Button button = btnSubmit;
                string text = "Submit";
                button.Text = text;
                ShowDialog();
            }
            else
            {
                bitmap_0 = new Bitmap(method_7(string_0));
                method_3();
                SetWindowPos(base.Handle, intptr_1, 0, 0, 0, 0, 3u);
                btnReferesh_Click(null, null);
                btnSubmit.Enabled = true;
                Button button2 = btnSubmit;
                string text2 = "Submit";
                button2.Text = text2;
                base.TopMost = true;
                ShowDialog();
            }
        }
        catch (ThreadAbortException)
        {
            Thread.ResetAbort();
        }
        catch
        {
        }
    }

    private void btnReferesh_Click(object sender, EventArgs e)
    {
        try
        {
            bitmap_0 = null;
            Thread thread = new Thread(method_6);
            thread.IsBackground = true;
            thread.Start(0);
        }
        catch
        {
        }
    }

    private void method_6(object object_1)
    {
        try
        {
            DateTime.Now.AddMilliseconds(400.0);
            Application.DoEvents();
            string loginType = class28_0.LoginType;
            string text = "WEB";
            Bitmap bitmap = ((!(loginType == text)) ? new Bitmap(method_7(string_0)) : new Bitmap(method_7(string_0)));
            try
            {
                picOriginal.Width = base.Width - 4;
                picOriginal.BackColor = Color.FromArgb(4, 65, 76);
                picOriginal.Image = bitmap;
                if (bitmap.Height == 20 && bitmap.Width == 308)
                {
                    picOriginal.Size = new Size(517, 45);
                    picOriginal.Left = -280;
                }
                picOriginal.SizeMode = PictureBoxSizeMode.Zoom;
                picOriginal.Refresh();
                clsMain clsMain = class28_0;
                string text2 = "Captcha Received =";
                clsMain.Lock(text2 + string_0);
                btnReferesh.Enabled = true;
            }
            catch
            {
            }
            try
            {
                if (bitmap != null)
                {
                    method_0();
                    _ = (Bitmap)bitmap.Clone();
                    string object_2 = "";
                    lock (lockObject)
                    {
                        if (Program.AutofillOCR)
                        {
                            object_2 = SolveCaptcha(base64inage);
                        }
                    }
                    if (Program.AutoFillAPI)
                    {
                        object_2 = CallAPI(base64inage);
                    }
                    method_27(object_2);
                }
            }
            catch (Exception)
            {
            }
        }
        catch
        {
        }
        if (Program.autoSubmitCaptcha)
        {
            btnSubmit_Click(this, null);
        }
        bitmap_0 = null;
    }

    private string CallAPI(string base64)
    {
        string text = string.Empty;
        try
        {
            string requestUriString = "https://api.apitruecaptcha.org/one/gettext";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
            string contentType = "application/json";
            httpWebRequest.ContentType = contentType;
            string method = "POST";
            httpWebRequest.Method = method;
            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string userid = "mlifekhan94@gmail.com";
                string apikey = "fzg43RPRAfZKz6z3VePn";
                string value = JsonSerializer.Serialize(new
                {
                    userid = userid,
                    apikey = apikey,
                    data = base64
                });
                streamWriter.Write(value);
            }
            using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
            string text2 = streamReader.ReadToEnd();
            string value2 = "result";
            string text3 = text2.Substring(text2.IndexOf(value2) + 10);
            string value3 = "\"";
            text = text3.Substring(0, text3.IndexOf(value3));
            Console.WriteLine(text);
        }
        catch (Exception)
        {
        }
        return text;
    }

    private void method_27(object object_1)
    {
        try
        {
            if (txtImageData.InvokeRequired)
            {
                txtImageData.Invoke(new ParameterizedThreadStart(method_27), object_1);
            }
            else if (object_1.ToString().Length > 2)
            {
                txtImageData.Text = object_1.ToString();
            }
        }
        catch
        {
        }
    }

    private void method_31(object object_1)
    {
        try
        {
            string pks = frmCaptcha.pks;
            if (!Directory.Exists(pks))
            {
                Directory.CreateDirectory(pks);
            }
            bool flag = false;
            string path = "CaptchaRecog_CShape.exe";
            if (!File.Exists(Path.Combine(pks, path)))
            {
                flag = true;
            }
            string path2 = "spiky5.dll";
            string text = Path.Combine(pks, path2);
            string path3 = "spiky6.zip";
            string text2 = Path.Combine(pks, path3);
            if (!flag)
            {
                return;
            }
            File.Delete(text);
            File.Delete(text2);
            if (!File.Exists(text))
            {
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                string name = "IRCommDLL.include.CaptchaRecog_CShape.dll";
                Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(name);
                if (manifestResourceStream != null)
                {
                    FileStream fileStream = new FileStream(text, FileMode.CreateNew);
                    for (int i = 0; i < manifestResourceStream.Length; i++)
                    {
                        fileStream.WriteByte((byte)manifestResourceStream.ReadByte());
                    }
                    fileStream.Close();
                    new FileInfo(text).Attributes = FileAttributes.Hidden;
                }
            }
            if (!File.Exists(text))
            {
                return;
            }
            if (!File.Exists(text2))
            {
                File.Copy(text, text2, overwrite: true);
            }
            File.Delete(text);
            try
            {
                ZipFile zipFile = ZipFile.Read(text2);
                try
                {
                    string password = "DM04AP16MS3";
                    zipFile.Password = password;
                    zipFile.ExtractAll(pks);
                }
                finally
                {
                    zipFile?.Dispose();
                }
                File.Delete(text2);
            }
            catch (Exception ex)
            {
                File.Delete(text2);
                MessageBox.Show(ex.ToString());
            }
        }
        catch
        {
        }
    }

    public static void smethod_12(object object_1)
    {
        try
        {
            string pks = frmCaptcha.pks;
            bool flag = false;
            string path = "XCaptchaRecog.exe";
            if (!File.Exists(Path.Combine(pks, path)))
            {
                flag = true;
            }
            string path2 = "spiky5.dll";
            string text = Path.Combine(pks, path2);
            string path3 = "spiky6.zip";
            string text2 = Path.Combine(pks, path3);
            if (!flag)
            {
                return;
            }
            if (File.Exists(pks))
            {
                File.Delete(text);
            }
            if (!File.Exists(text))
            {
                Assembly executingAssembly = Assembly.GetExecutingAssembly();
                string name = "NEXUS.Extention.XCaptchaRecog.dll";
                Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(name);
                if (manifestResourceStream != null)
                {
                    FileStream fileStream = new FileStream(text, FileMode.CreateNew);
                    for (int i = 0; i < manifestResourceStream.Length; i++)
                    {
                        fileStream.WriteByte((byte)manifestResourceStream.ReadByte());
                    }
                    fileStream.Close();
                    new FileInfo(text).Attributes = FileAttributes.Hidden;
                }
            }
            if (!File.Exists(text))
            {
                return;
            }
            if (!File.Exists(text2))
            {
                File.Copy(text, text2, overwrite: true);
            }
            File.Delete(text);
            try
            {
                ZipFile zipFile = ZipFile.Read(text2);
                try
                {
                    zipFile.ExtractAll(pks);
                }
                finally
                {
                    zipFile?.Dispose();
                }
                File.Delete(text2);
            }
            catch (Exception ex)
            {
                File.Delete(text2);
                MessageBox.Show(ex.ToString());
            }
        }
        catch
        {
        }
    }

    internal string SolveCaptcha(string bs64)
    {
        method_31(0);
        string text = string.Empty;
        try
        {
            string text2 = "|ce56gjr678i79k";
            string arguments = bs64 + text2;
            Process process = new Process();
            ProcessStartInfo startInfo = process.StartInfo;
            string pks = frmCaptcha.pks;
            string path = "ByteNovaocr//XCaptchaRecog.exe";
            startInfo.FileName = Path.Combine(pks, path);
            process.StartInfo.Arguments = arguments;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            StreamReader standardOutput = process.StandardOutput;
            process.WaitForExit(3500);
            text = standardOutput.ReadLine();
            string text3 = text;
            string value = ",";
            if (text3.Contains(value))
            {
                string text4 = text;
                string value2 = ")";
                if (text4.Contains(value2))
                {
                    text = text.Trim('(', ')').Split(',')[0].Trim();
                }
            }
            if (!process.HasExited)
            {
                process.Kill();
            }
        }
        catch (Exception)
        {
        }
        if (string.IsNullOrEmpty(text))
        {
            try
            {
                string pks2 = frmCaptcha.pks;
                string path2 = "XCaptchaRecog.exe";
                string path3 = Path.Combine(pks2, path2);
                if (File.Exists(path3))
                {
                    File.Delete(path3);
                    smethod_12(0);
                }
            }
            catch
            {
            }
        }
        return text;
    }

    private void method_0()
    {
        txtImageData.Text = "";
        txtImageData.Focus();
        string variable = "AutofillCp";
        bool.TryParse(Environment.GetEnvironmentVariable(variable, EnvironmentVariableTarget.Process), out clsMain.bool_16);
        string variable2 = "AutoSubmitCp";
        bool.TryParse(Environment.GetEnvironmentVariable(variable2, EnvironmentVariableTarget.Process), out clsMain.bool_17);
    }

    private void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(txtImageData.Text.Trim()) && txtImageData.Text.Trim().Length >= 2)
        {
            base.DialogResult = DialogResult.OK;
            Close();
        }
    }

    private void Close_Click(object sender, EventArgs e)
    {
        base.DialogResult = DialogResult.Cancel;
        Close();
    }

    internal string Base64Encode(string plainText)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(newCaptcha));
        this.bunifuElipse_0 = new Bunifu.Framework.UI.BunifuElipse(this.components);
        this.PanelHelder = new System.Windows.Forms.Panel();
        this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
        this.tktname = new System.Windows.Forms.Label();
        this.btnSubmit = new System.Windows.Forms.Button();
        this.Close = new System.Windows.Forms.Button();
        this.picOriginal = new System.Windows.Forms.PictureBox();
        this.txtImageData = new System.Windows.Forms.TextBox();
        this.btnReferesh = new System.Windows.Forms.Button();
        this.PanelHelder.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).BeginInit();
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
        this.PanelHelder.TabIndex = 130;
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
        // btnSubmit
        // 
        this.btnSubmit.Location = new System.Drawing.Point(104, 121);
        this.btnSubmit.Name = "btnSubmit";
        this.btnSubmit.Size = new System.Drawing.Size(109, 23);
        this.btnSubmit.TabIndex = 131;
        this.btnSubmit.Text = "Submit";
        this.btnSubmit.UseVisualStyleBackColor = true;
        this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
        // 
        // Close
        // 
        this.Close.Location = new System.Drawing.Point(12, 121);
        this.Close.Name = "Close";
        this.Close.Size = new System.Drawing.Size(75, 23);
        this.Close.TabIndex = 132;
        this.Close.Text = "Cancell";
        this.Close.UseVisualStyleBackColor = true;
        this.Close.Click += new System.EventHandler(this.Close_Click);
        // 
        // picOriginal
        // 
        this.picOriginal.Dock = System.Windows.Forms.DockStyle.Top;
        this.picOriginal.Location = new System.Drawing.Point(0, 15);
        this.picOriginal.Name = "picOriginal";
        this.picOriginal.Size = new System.Drawing.Size(225, 65);
        this.picOriginal.TabIndex = 133;
        this.picOriginal.TabStop = false;
        // 
        // txtImageData
        // 
        this.txtImageData.Location = new System.Drawing.Point(60, 87);
        this.txtImageData.Name = "txtImageData";
        this.txtImageData.Size = new System.Drawing.Size(100, 20);
        this.txtImageData.TabIndex = 134;
        // 
        // btnReferesh
        // 
        this.btnReferesh.Location = new System.Drawing.Point(314, 85);
        this.btnReferesh.Name = "btnReferesh";
        this.btnReferesh.Size = new System.Drawing.Size(75, 23);
        this.btnReferesh.TabIndex = 135;
        this.btnReferesh.Text = "button1";
        this.btnReferesh.UseVisualStyleBackColor = true;
        // 
        // newCaptcha
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(225, 156);
        this.Controls.Add(this.btnReferesh);
        this.Controls.Add(this.txtImageData);
        this.Controls.Add(this.picOriginal);
        this.Controls.Add(this.Close);
        this.Controls.Add(this.btnSubmit);
        this.Controls.Add(this.PanelHelder);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.Name = "newCaptcha";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.Text = "newCaptcha";
        this.TopMost = true;
        this.PanelHelder.ResumeLayout(false);
        this.PanelHelder.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.picOriginal)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    static newCaptcha()
    {
        int_5 = 180;
        intptr_1 = new IntPtr(-1);
        string text = "api.cheapcaptcha.com";
        string_15 = text;
    }
}
