using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Ionic.Zip;

namespace APP_FRAMEWORK;

internal class frmDownloasData : Form
{
    private string[] onnxParts = new string[3] { "https://raw.githubusercontent.com/bytenova-dot/masterdata/refs/heads/main/model_int8.onnx.part0", "https://raw.githubusercontent.com/bytenova-dot/masterdata/refs/heads/main/model_int8.onnx.part1", "https://raw.githubusercontent.com/bytenova-dot/masterdata/refs/heads/main/model_int8.onnx.part2" };

    private int currentPart;

    private string[] downloadedParts;

    private string URLL;

    private string FileName_;

    private string string_0;

    private string string_1;

    private int int_0;

    private int int_1;

    private Stopwatch stopwatch_0 = new Stopwatch();

    private IContainer icontainer_0;

    private IDisposable idisposable_0;

    private Label labelDownloaded;

    private Label lblstatus;

    private ProgressBar progressBar1;

    private Label B0AAA;

    private Label C0AA0;

    private Button button1;

    private IContainer components;

    private System.Windows.Forms.Timer timer_0;

    private Panel bnHeader;
    private Label bnTitle;
    private Label bnSubtitle;
    private Panel bnProgressTrack;
    private Panel bnProgressFill;

    public frmDownloasData()
    {
        InitializeComponent();
        ApplyByteNovaTheme();
        ServicePointManager.Expect100Continue = false;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
    }

    private void ApplyByteNovaTheme()
    {
        SuspendLayout();

        ClientSize = new Size(520, 176);
        BackColor = Color.FromArgb(14, 9, 18);
        ForeColor = Color.White;
        FormBorderStyle = FormBorderStyle.None;
        Padding = new Padding(1);

        bnHeader = new Panel();
        bnHeader.Name = "bnHeader";
        bnHeader.SetBounds(6, 6, 508, 47);
        bnHeader.BackColor = Color.FromArgb(37, 23, 51);
        bnHeader.Paint += ByteNovaHeaderPaint;

        bnTitle = new Label();
        bnTitle.AutoSize = false;
        bnTitle.SetBounds(15, 7, 300, 19);
        bnTitle.Text = "EXPRESS  •  DOWNLOAD CENTER";
        bnTitle.ForeColor = Color.White;
        bnTitle.BackColor = Color.Transparent;
        bnTitle.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);

        bnSubtitle = new Label();
        bnSubtitle.AutoSize = false;
        bnSubtitle.SetBounds(15, 26, 350, 15);
        bnSubtitle.Text = "SECURE FILE DELIVERY  /  PLEASE WAIT";
        bnSubtitle.ForeColor = Color.FromArgb(121, 211, 255);
        bnSubtitle.BackColor = Color.Transparent;
        bnSubtitle.Font = new Font("Segoe UI", 7.5F, FontStyle.Regular);

        bnHeader.Controls.Add(bnTitle);
        bnHeader.Controls.Add(bnSubtitle);
        Controls.Add(bnHeader);
        bnHeader.BringToFront();

        lblstatus.AutoSize = false;
        lblstatus.SetBounds(16, 62, 488, 23);
        lblstatus.TextAlign = ContentAlignment.MiddleLeft;
        lblstatus.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
        lblstatus.ForeColor = Color.FromArgb(229, 239, 249);
        lblstatus.BackColor = Color.Transparent;

        // Replace the stock visual progress bar with a clean ByteNova progress rail.
        progressBar1.Visible = false;
        bnProgressTrack = new Panel();
        bnProgressTrack.SetBounds(16, 92, 488, 12);
        bnProgressTrack.BackColor = Color.FromArgb(48, 30, 66);
        bnProgressTrack.Paint += ByteNovaProgressTrackPaint;

        bnProgressFill = new Panel();
        bnProgressFill.SetBounds(2, 2, 0, 8);
        bnProgressFill.BackColor = Color.FromArgb(198, 244, 50);
        bnProgressTrack.Controls.Add(bnProgressFill);
        Controls.Add(bnProgressTrack);

        B0AAA.AutoSize = false;
        B0AAA.SetBounds(16, 113, 205, 24);
        B0AAA.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        B0AAA.ForeColor = Color.FromArgb(190, 208, 224);
        B0AAA.BackColor = Color.Transparent;
        B0AAA.TextAlign = ContentAlignment.MiddleLeft;

        C0AA0.AutoSize = false;
        C0AA0.SetBounds(220, 113, 155, 24);
        C0AA0.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        C0AA0.ForeColor = Color.FromArgb(255, 202, 76);
        C0AA0.BackColor = Color.Transparent;
        C0AA0.TextAlign = ContentAlignment.MiddleCenter;

        labelDownloaded.AutoSize = false;
        labelDownloaded.SetBounds(391, 109, 113, 31);
        labelDownloaded.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold);
        labelDownloaded.ForeColor = Color.FromArgb(198, 244, 50);
        labelDownloaded.BackColor = Color.Transparent;
        labelDownloaded.TextAlign = ContentAlignment.MiddleRight;

        // Completion button is used programmatically; keep it out of the UI.
        button1.Visible = false;

        Paint += ByteNovaFormPaint;

        ResumeLayout(false);
        Invalidate();
    }

    private void UpdateByteNovaProgress(int percentage)
    {
        if (bnProgressTrack == null || bnProgressFill == null) return;
        int usable = Math.Max(0, bnProgressTrack.ClientSize.Width - 4);
        int width = (int)Math.Round(usable * Math.Max(0, Math.Min(100, percentage)) / 100.0);
        bnProgressFill.Width = width;
    }

    private void ByteNovaHeaderPaint(object sender, PaintEventArgs e)
    {
        Rectangle r = bnHeader.ClientRectangle;
        if (r.Width <= 0 || r.Height <= 0) return;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (LinearGradientBrush br = new LinearGradientBrush(
            r, Color.FromArgb(3, 29, 59), Color.FromArgb(45, 30, 91),
            LinearGradientMode.Horizontal))
            e.Graphics.FillRectangle(br, r);
        using (Pen p = new Pen(Color.FromArgb(198, 244, 50), 1F))
            e.Graphics.DrawLine(p, 0, r.Height - 1, r.Width, r.Height - 1);
    }

    private void ByteNovaProgressTrackPaint(object sender, PaintEventArgs e)
    {
        Rectangle r = bnProgressTrack.ClientRectangle;
        if (r.Width <= 1 || r.Height <= 1) return;
        using (Pen p = new Pen(Color.FromArgb(39, 79, 108), 1F))
            e.Graphics.DrawRectangle(p, 0, 0, r.Width - 1, r.Height - 1);
    }

    private void ByteNovaFormPaint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (Pen outer = new Pen(Color.FromArgb(198, 244, 50), 1.2F))
            e.Graphics.DrawRectangle(outer, 1, 1, ClientSize.Width - 3, ClientSize.Height - 3);
    }

    private void Form_2_Load(object sender, EventArgs e)
    {
        string chrome = Path.Combine(Environment.CurrentDirectory, "chromedriver.exe");

        if (FileName_ != null &&
            (FileName_.Contains("model_int8.onnx") || FileName_.Contains("XCaptchaRecog.exe")))
        {
            BeginInvoke((Action)delegate { base.DialogResult = DialogResult.OK; Close(); });
            return;
        }

        if (FileName_ != null && FileName_.Contains("chromedriver") && File.Exists(chrome))
        {
            BeginInvoke((Action)delegate { base.DialogResult = DialogResult.OK; Close(); });
            return;
        }

        string fileName_ = FileName_;
        if (fileName_.Contains("XCaptchaRecog.exe")) { method_00(URLL, FileName_); return; }
        if (fileName_.Contains("model_int8.onnx")) { method_0011(URLL, FileName_); }
        else { method_0(URLL, FileName_); }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        base.DialogResult = DialogResult.OK;
        Close();
    }

    private void timer_0_Tick(object sender, EventArgs e)
    {
    }

    public void startDownload(string pUri, string pFile)
    {
        URLL = pUri;
        FileName_ = pFile;
        ShowDialog();
    }

    private void method_0011(string unused1, string unused2)
    {
        try
        {
            downloadedParts = new string[onnxParts.Length];
            currentPart = 0;
            Thread thread = new Thread((ThreadStart)delegate
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                DownloadNextPart();
            });
            thread.IsBackground = true;
            thread.Start();
        }
        catch
        {
        }
    }

    private void DownloadNextPart()
    {
        if (currentPart >= onnxParts.Length)
        {
            CombineOnnxParts();
            return;
        }
        string text = onnxParts[currentPart];
        string currentDirectory = Environment.CurrentDirectory;
        string format = "model_int8.onnx.part{0}";
        string text2 = Path.Combine(currentDirectory, string.Format(format, currentPart));
        downloadedParts[currentPart] = text2;
        WebClient webClient = new WebClient();
        webClient.DownloadProgressChanged += method_1;
        webClient.DownloadFileCompleted += method_22;
        FileName_ = text2;
        URLL = text;
        stopwatch_0.Restart();
        webClient.DownloadFileAsync(new Uri(text), text2);
    }

    private void method_22(object sender, AsyncCompletedEventArgs e)
    {
        stopwatch_0.Stop();
        if (e.Error == null && !e.Cancelled)
        {
            currentPart++;
            DownloadNextPart();
        }
    }

    private void CombineOnnxParts()
    {
        string currentDirectory = Environment.CurrentDirectory;
        string path = "model_int8.onnx";
        string[] array;
        using (FileStream fileStream = new FileStream(Path.Combine(currentDirectory, path), FileMode.Create))
        {
            array = downloadedParts;
            for (int i = 0; i < array.Length; i++)
            {
                byte[] array2 = File.ReadAllBytes(array[i]);
                fileStream.Write(array2, 0, array2.Length);
            }
        }
        array = downloadedParts;
        for (int j = 0; j < array.Length; j++)
        {
            File.Delete(array[j]);
        }
        BeginInvoke((Action)delegate
        {
            button1.PerformClick();
        });
    }

    private void method_0(string string_2, string string_3)
    {
        URLL = string_2;
        string_0 = string_3;
        int_1++;
        try
        {
            new Thread((ThreadStart)delegate
            {
                ServicePointManager.Expect100Continue = false;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;
                WebClient webClient = new WebClient();
                webClient.DownloadProgressChanged += method_1;
                webClient.DownloadFileCompleted += method_2;
                webClient.DownloadFileAsync(new Uri(URLL), FileName_);
                stopwatch_0.Start();
            }).Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }

    private void method_1(object sender, DownloadProgressChangedEventArgs e)
    {
        try
        {
            BeginInvoke((Action)delegate
            {
                Label c0AA2 = C0AA0;
                string format2 = "{0} Kbps";
                double num2 = (double)e.BytesReceived / 1024.0 / stopwatch_0.Elapsed.TotalSeconds;
                string text8 = "0.00";
                c0AA2.Text = string.Format(format2, num2.ToString(text8));
                Label b0AAA2 = B0AAA;
                string text9 = "File Size _  ";
                num2 = (double)e.TotalBytesToReceive / 1024.0 / 1024.0;
                string text10 = "0.00";
                string text11 = num2.ToString(text10);
                string text12 = " / MB";
                b0AAA2.Text = text9 + text11 + text12;
                Label label2 = labelDownloaded;
                string text13 = e.ProgressPercentage.ToString();
                string text14 = "%";
                label2.Text = text13 + text14;
                progressBar1.Value = e.ProgressPercentage;
                UpdateByteNovaProgress(e.ProgressPercentage);
            });
        }
        catch (Exception)
        {
        }
        try
        {
            BeginInvoke((Action)delegate
            {
                Label c0AA = C0AA0;
                string format = "{0} Kbps";
                double num = (double)e.BytesReceived / 1024.0 / stopwatch_0.Elapsed.TotalSeconds;
                string text = "0.00";
                c0AA.Text = string.Format(format, num.ToString(text));
                Label b0AAA = B0AAA;
                string text2 = "File Size _  ";
                num = (double)e.TotalBytesToReceive / 1024.0 / 1024.0;
                string text3 = "0.00";
                string text4 = num.ToString(text3);
                string text5 = " / MB";
                b0AAA.Text = text2 + text4 + text5;
                Label label = labelDownloaded;
                string text6 = e.ProgressPercentage.ToString();
                string text7 = "%";
                label.Text = text6 + text7;
                progressBar1.Value = e.ProgressPercentage;
                UpdateByteNovaProgress(e.ProgressPercentage);
            });
        }
        catch (Exception)
        {
        }
    }

    private void method_00(string string_2, string string_3)
    {
        URLL = string_2;
        string_0 = string_3;
        int_1++;
        try
        {
            new Thread((ThreadStart)delegate
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string path = "Microsoft";
                string path2 = "ByteNovaocr";
                string text = Path.Combine(folderPath, path, path2);
                Directory.CreateDirectory(text);
                string path3 = "XCaptchaRecog.exe";
                FileName_ = Path.Combine(text, path3);
                WebClient webClient = new WebClient();
                webClient.DownloadProgressChanged += method_1;
                webClient.DownloadFileCompleted += method_2;
                webClient.DownloadFileAsync(new Uri(URLL), FileName_);
                stopwatch_0.Start();
            }).Start();
        }
        catch
        {
        }
    }

    private void method_2(object sender, AsyncCompletedEventArgs e)
    {
        if (e.Error == null && !e.Cancelled)
        {
            try
            {
                string text = string_0;
                string value = "XCaptchaRecog.exe";
                if (!text.Contains(value))
                {
                    string text2 = string_0;
                    string value2 = "chromedriver-win64.zip";
                    if (text2.Contains(value2))
                    {
                        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                        string text3 = "/chromedriver-win64.zip";
                        ExtractChromeDriverToCurrentFolder(baseDirectory + text3);
                    }
                    else
                    {
                        using (ZipFile zipFile = ZipFile.Read(FileName_))
                        {
                            zipFile.ExtractAll(Directory.GetCurrentDirectory(), ExtractExistingFileAction.OverwriteSilently);
                        }
                        File.Delete(FileName_);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                string text4 = "Could not delete ZIP!";
                MessageBox.Show(text4);
                Environment.Exit(1);
            }
        }
        BeginInvoke((Action)delegate
        {
            button1.PerformClick();
        });
    }

    public static string ExtractChromeDriverToCurrentFolder(string zipPath)
    {
        try
        {
            if (!File.Exists(zipPath))
            {
                string message = "Zip file not found.";
                throw new FileNotFoundException(message, zipPath);
            }
            string directoryName = Path.GetDirectoryName(zipPath);
            string path = "tmp_extract";
            string path2 = Path.Combine(directoryName, path);
            if (Directory.Exists(path2))
            {
                Directory.Delete(path2, recursive: true);
            }
            Directory.CreateDirectory(path2);
            using (ZipFile zipFile = ZipFile.Read(zipPath))
            {
                zipFile.ExtractAll(path2, ExtractExistingFileAction.OverwriteSilently);
            }
            string searchPattern = "chromedriver.exe";
            string[] files = Directory.GetFiles(path2, searchPattern, SearchOption.AllDirectories);
            if (files.Length == 0)
            {
                string message2 = "chromedriver.exe not found inside the ZIP.";
                throw new Exception(message2);
            }
            string sourceFileName = files[0];
            string path3 = "chromedriver.exe";
            string text = Path.Combine(directoryName, path3);
            if (File.Exists(text))
            {
                File.Delete(text);
            }
            File.Move(sourceFileName, text);
            File.Delete(zipPath);
            Directory.Delete(path2, recursive: true);
            return text;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private bool method_3(string string_2)
    {
        bool result = false;
        try
        {
            if (File.Exists(string_2))
            {
                File.Delete(string_2);
                result = true;
            }
        }
        catch
        {
            result = false;
        }
        return result;
    }

    private bool method_4(string string_2)
    {
        bool result = false;
        try
        {
            if (File.Exists(string_2))
            {
                if (Directory.Exists(string_2))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(string_2);
                    FileInfo[] files = directoryInfo.GetFiles();
                    for (int i = 0; i < files.Length; i++)
                    {
                        files[i].Delete();
                    }
                    DirectoryInfo[] directories = directoryInfo.GetDirectories();
                    for (int j = 0; j < directories.Length; j++)
                    {
                        directories[j].Delete(recursive: true);
                    }
                }
                result = true;
            }
        }
        catch
        {
            result = false;
        }
        return result;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && idisposable_0 != null)
        {
            idisposable_0.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDownloasData));
        this.lblstatus = new System.Windows.Forms.Label();
        this.labelDownloaded = new System.Windows.Forms.Label();
        this.progressBar1 = new System.Windows.Forms.ProgressBar();
        this.B0AAA = new System.Windows.Forms.Label();
        this.C0AA0 = new System.Windows.Forms.Label();
        this.button1 = new System.Windows.Forms.Button();
        this.timer_0 = new System.Windows.Forms.Timer(this.components);
        this.SuspendLayout();
        // 
        // lblstatus
        // 
        this.lblstatus.AutoSize = true;
        this.lblstatus.BackColor = System.Drawing.Color.Transparent;
        this.lblstatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblstatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        this.lblstatus.Location = new System.Drawing.Point(76, 306);
        this.lblstatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.lblstatus.Name = "lblstatus";
        this.lblstatus.Size = new System.Drawing.Size(389, 24);
        this.lblstatus.TabIndex = 183;
        this.lblstatus.Text = "Downloading External File Please Wait...";
        // 
        // labelDownloaded
        // 
        this.labelDownloaded.AutoSize = true;
        this.labelDownloaded.BackColor = System.Drawing.Color.Transparent;
        this.labelDownloaded.Font = new System.Drawing.Font("DS-Digital", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.labelDownloaded.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        this.labelDownloaded.Location = new System.Drawing.Point(393, 20);
        this.labelDownloaded.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.labelDownloaded.Name = "labelDownloaded";
        this.labelDownloaded.Size = new System.Drawing.Size(43, 21);
        this.labelDownloaded.TabIndex = 181;
        this.labelDownloaded.Text = "00%";
        // 
        // progressBar1
        // 
        this.progressBar1.Location = new System.Drawing.Point(-3, -1);
        this.progressBar1.Margin = new System.Windows.Forms.Padding(2);
        this.progressBar1.Name = "progressBar1";
        this.progressBar1.Size = new System.Drawing.Size(486, 18);
        this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
        this.progressBar1.TabIndex = 179;
        // 
        // B0AAA
        // 
        this.B0AAA.AutoSize = true;
        this.B0AAA.BackColor = System.Drawing.Color.Transparent;
        this.B0AAA.Font = new System.Drawing.Font("DS-Digital", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.B0AAA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        this.B0AAA.Location = new System.Drawing.Point(-1, 22);
        this.B0AAA.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.B0AAA.Name = "B0AAA";
        this.B0AAA.Size = new System.Drawing.Size(20, 21);
        this.B0AAA.TabIndex = 182;
        this.B0AAA.Text = "0";
        // 
        // C0AA0
        // 
        this.C0AA0.AutoSize = true;
        this.C0AA0.BackColor = System.Drawing.Color.Transparent;
        this.C0AA0.Font = new System.Drawing.Font("DS-Digital", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.C0AA0.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        this.C0AA0.Location = new System.Drawing.Point(219, 20);
        this.C0AA0.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.C0AA0.Name = "C0AA0";
        this.C0AA0.Size = new System.Drawing.Size(20, 21);
        this.C0AA0.TabIndex = 180;
        this.C0AA0.Text = "0";
        // 
        // button1
        // 
        this.button1.Location = new System.Drawing.Point(407, 259);
        this.button1.Margin = new System.Windows.Forms.Padding(2);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(82, 19);
        this.button1.TabIndex = 178;
        this.button1.Text = "button1";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.button1_Click);
        // 
        // timer_0
        // 
        this.timer_0.Tick += new System.EventHandler(this.timer_0_Tick);
        // 
        // frmDownloasData
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.FromArgb(14, 9, 18);
        this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.ClientSize = new System.Drawing.Size(520, 176);
        this.ControlBox = false;
        this.Controls.Add(this.labelDownloaded);
        this.Controls.Add(this.C0AA0);
        this.Controls.Add(this.B0AAA);
        this.Controls.Add(this.lblstatus);
        this.Controls.Add(this.progressBar1);
        this.Controls.Add(this.button1);
        this.ForeColor = System.Drawing.Color.Red;
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "frmDownloasData";
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "File Downloading";
        this.TopMost = true;
        this.Load += new System.EventHandler(this.Form_2_Load);
        this.ResumeLayout(false);
        this.PerformLayout();

    }
}
