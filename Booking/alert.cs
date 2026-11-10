using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net;
using System.Windows.Forms;
using Bunifu.Framework.UI;

namespace Booking;

public class alert : Form
{
    public static string m1;

    public static string m2;

    public static string m3;

    public static bool demo;

    private IContainer icontainer_0;

    private BunifuElipse bunifuElipse_0;

    private Label label1;

    private Panel panel1;

    private Label BAAAA;
    private Label label2;
    private PictureBox pictureBox2;
    private PictureBox pictureBox1;
    private Label label3;
    private BunifuImageButton btnClose;
    private BackgroundWorker backgroundWorker1;
    private IContainer components;

    public alert()
    {
        InitializeComponent();
        ApplyByteNovaLatestUI();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        base.DialogResult = DialogResult.OK;
        Close();
    }

    public void show_(object j)
    {
        ApplyAlertValue();
        ShowDialog();
    }

    private void alert_Load(object sender, EventArgs e)
    {
        ApplyAlertValue();
    }

    private void ApplyAlertValue()
    {
        string value = m1;

        if (string.IsNullOrWhiteSpace(value))
            value = m2;

        if (string.IsNullOrWhiteSpace(value))
            value = m3;

        if (!string.IsNullOrWhiteSpace(value))
        {
            BAAAA.BackColor = Color.FromArgb(4, 24, 40);
            BAAAA.ForeColor = Color.FromArgb(240, 249, 255);
            BAAAA.Text = FormatByteNovaMessageRows(value);
            BAAAA.Visible = true;
            BAAAA.BringToFront();
            BAAAA.Invalidate();
            BAAAA.Update();
        }
    }

    private void ApplyByteNovaLatestUI()
    {
        SuspendLayout();

        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(760, 480);
        MinimumSize = new Size(760, 480);
        MaximumSize = new Size(760, 480);
        BackColor = Color.FromArgb(1, 9, 18);
        ForeColor = Color.White;
        DoubleBuffered = true;

        Paint -= ByteNovaAlertPaint;
        Paint += ByteNovaAlertPaint;

        // Premium compact ByteNova header.
        label1.AutoSize = false;
        label1.Location = new Point(16, 2);
        label1.Size = new Size(650, 25);
        label1.Text = "Express  •  IMPORTANT MESSAGE";
        label1.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        label1.ForeColor = Color.White;
        label1.BackColor = Color.Transparent;
        label1.TextAlign = ContentAlignment.MiddleLeft;

        // frmMain-style compact Close button.
        btnClose.Location = new Point(721, 3);
        btnClose.Size = new Size(27, 23);
        btnClose.Paint -= ByteNovaAlertClosePaint;
        btnClose.BackColor = Color.Transparent;
        btnClose.BackgroundImage = null;
        btnClose.BackgroundImageLayout = ImageLayout.Zoom;
        btnClose.SizeMode = PictureBoxSizeMode.Zoom;
        btnClose.Zoom = 0;
        btnClose.BringToFront();

        // Large HD message card.
        BAAAA.Location = new Point(14, 38);
        BAAAA.Size = new Size(732, 284);
        BAAAA.BackColor = Color.FromArgb(4, 24, 40);
        BAAAA.ForeColor = Color.FromArgb(240, 249, 255);
        BAAAA.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        BAAAA.TextAlign = ContentAlignment.TopLeft;
        BAAAA.Padding = new Padding(24, 22, 24, 22);
        BAAAA.Paint -= ByteNovaMessagePaint;
        BAAAA.Paint += ByteNovaMessagePaint;

        // Full-width social / QR card.
        panel1.Dock = DockStyle.None;
        panel1.Location = new Point(14, 334);
        panel1.Size = new Size(732, 132);
        panel1.BackColor = Color.FromArgb(3, 18, 32);
        panel1.Paint -= ByteNovaSocialPanelPaint;
        panel1.Paint += ByteNovaSocialPanelPaint;

        label2.Location = new Point(18, 23);
        label2.Size = new Size(210, 58);
        label2.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        label2.ForeColor = Color.FromArgb(180, 235, 255);
        label2.BackColor = Color.Transparent;
        label2.Text = "JOIN OFFICIAL\r\nWHATSAPP CHANNEL";
        label2.TextAlign = ContentAlignment.MiddleCenter;

        pictureBox1.Location = new Point(235, 10);
        pictureBox1.Size = new Size(110, 110);
        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox1.BackColor = Color.White;

        label3.Location = new Point(385, 23);
        label3.Size = new Size(210, 58);
        label3.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        label3.ForeColor = Color.FromArgb(180, 235, 255);
        label3.BackColor = Color.Transparent;
        label3.Text = "JOIN OFFICIAL\r\nTELEGRAM CHANNEL";
        label3.TextAlign = ContentAlignment.MiddleCenter;

        pictureBox2.Location = new Point(608, 10);
        pictureBox2.Size = new Size(110, 110);
        pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
        pictureBox2.BackColor = Color.White;

        label1.MouseDown -= ByteNovaAlertDrag_MouseDown;
        label1.MouseDown += ByteNovaAlertDrag_MouseDown;

        ResumeLayout(false);
        PerformLayout();
        Invalidate();
    }

    private void ByteNovaAlertPaint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
        e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        Rectangle all = new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
        using (LinearGradientBrush bg = new LinearGradientBrush(
            all, Color.FromArgb(3, 21, 39), Color.FromArgb(1, 7, 16),
            LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, all);

        Rectangle header = new Rectangle(1, 1, ClientSize.Width - 3, 29);
        using (LinearGradientBrush h = new LinearGradientBrush(
            header, Color.FromArgb(2, 31, 58), Color.FromArgb(42, 35, 105),
            LinearGradientMode.Horizontal))
        {
            ColorBlend blend = new ColorBlend();
            blend.Positions = new float[] { 0F, .32F, .68F, 1F };
            blend.Colors = new Color[]
            {
                Color.FromArgb(2, 20, 42),
                Color.FromArgb(0, 100, 150),
                Color.FromArgb(20, 82, 165),
                Color.FromArgb(55, 35, 115)
            };
            h.InterpolationColors = blend;
            e.Graphics.FillRectangle(h, header);
        }

        using (Pen cyan = new Pen(Color.FromArgb(0, 215, 255), 1.7F))
            e.Graphics.DrawRectangle(cyan, all);
        using (Pen line = new Pen(Color.FromArgb(198, 244, 50), 1F))
            e.Graphics.DrawLine(line, 1, 29, ClientSize.Width - 2, 29);
    }

    private string FormatByteNovaMessageRows(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        string normalized = value
            .Replace("\r\n", "\n")
            .Replace("\r", "\n");

        string[] rows = normalized.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        System.Text.StringBuilder result = new System.Text.StringBuilder();

        foreach (string row in rows)
        {
            string line = row.Trim();
            if (line.Length == 0) continue;

            if (result.Length > 0)
                result.Append(Environment.NewLine);

            result.Append(line);
        }

        return result.ToString();
    }

    private void ByteNovaMessagePaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        e.Graphics.TextRenderingHint =
            System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        Rectangle r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);

        using (Brush bg = new SolidBrush(Color.FromArgb(4, 24, 40)))
            e.Graphics.FillRectangle(bg, r);

        using (Pen p = new Pen(Color.FromArgb(0, 190, 245), 1.4F))
            e.Graphics.DrawRectangle(p, r);

        string text = c.Text ?? string.Empty;
        string[] sourceRows = text
            .Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        Color[] rowColors =
        {
            Color.FromArgb(70, 225, 255),
            Color.FromArgb(255, 215, 80),
            Color.FromArgb(120, 255, 170),
            Color.FromArgb(255, 135, 190),
            Color.FromArgb(180, 150, 255),
            Color.FromArgb(255, 170, 90),
            Color.FromArgb(120, 205, 255)
        };

        int left = c.Padding.Left;
        int top = c.Padding.Top;
        int right = c.Width - c.Padding.Right;
        int bottom = c.Height - c.Padding.Bottom;

        // Use a normal Unicode finger that renders reliably in Segoe UI Symbol.
        const string finger = "\u261E"; // ☞
        using (Font fingerFont = new Font("Segoe UI Symbol", 13F, FontStyle.Regular))
        {
            Size fingerSize = TextRenderer.MeasureText(
                finger, fingerFont, new Size(int.MaxValue, int.MaxValue),
                TextFormatFlags.NoPadding);

            int fingerGap = 7;
            int textLeft = left + fingerSize.Width + fingerGap;
            int textWidth = Math.Max(20, right - textLeft);

            float y = top;

            for (int rowIndex = 0; rowIndex < sourceRows.Length; rowIndex++)
            {
                string line = sourceRows[rowIndex].Trim();

                // Remove old emoji/symbol prefix if already stored in BAAAA.Text.
                if (line.StartsWith("👉"))
                    line = line.Substring("👉".Length).TrimStart();
                else if (line.StartsWith("☞"))
                    line = line.Substring("☞".Length).TrimStart();

                if (line.Length == 0)
                    continue;

                Color color = rowColors[rowIndex % rowColors.Length];

                // Measure with WordBreak so long messages automatically wrap.
                Size proposed = new Size(textWidth, int.MaxValue);
                Size measured = TextRenderer.MeasureText(
                    line,
                    c.Font,
                    proposed,
                    TextFormatFlags.WordBreak |
                    TextFormatFlags.TextBoxControl |
                    TextFormatFlags.NoPadding);

                int rowHeight = Math.Max(
                    measured.Height,
                    fingerSize.Height);

                if (y + rowHeight > bottom)
                    break;

                // Finger is drawn separately, so it can never disappear during wrapping.
                TextRenderer.DrawText(
                    e.Graphics,
                    finger,
                    fingerFont,
                    new Rectangle(left, (int)y, fingerSize.Width + 3, rowHeight),
                    color,
                    TextFormatFlags.Left |
                    TextFormatFlags.Top |
                    TextFormatFlags.NoPadding);

                // Text wraps inside the available width. Wrapped continuation
                // stays aligned after the finger instead of returning under it.
                TextRenderer.DrawText(
                    e.Graphics,
                    line,
                    c.Font,
                    new Rectangle(textLeft, (int)y, textWidth, rowHeight),
                    color,
                    TextFormatFlags.Left |
                    TextFormatFlags.Top |
                    TextFormatFlags.WordBreak |
                    TextFormatFlags.TextBoxControl |
                    TextFormatFlags.NoPadding);

                y += rowHeight + 9F;
            }
        }
    }

    private void ByteNovaSocialPanelPaint(object sender, PaintEventArgs e)
    {
        Control c = (Control)sender;
        Rectangle r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(5, 31, 51), Color.FromArgb(2, 15, 27),
            LinearGradientMode.Horizontal))
            e.Graphics.FillRectangle(bg, r);
        using (Pen p = new Pen(Color.FromArgb(0, 151, 220), 1.2F))
            e.Graphics.DrawRectangle(p, r);
    }

    private void ByteNovaAlertClosePaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(1, 1, c.Width - 3, c.Height - 3);
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(45, 205, 255), Color.FromArgb(0, 125, 190),
            LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, r);
        using (Pen border = new Pen(Color.FromArgb(145, 240, 255), 1.2F))
            e.Graphics.DrawRectangle(border, r);
        using (Pen p = new Pen(Color.White, 2.2F))
        {
            p.StartCap = LineCap.Round;
            p.EndCap = LineCap.Round;
            e.Graphics.DrawLine(p, 8, 7, c.Width - 8, c.Height - 7);
            e.Graphics.DrawLine(p, c.Width - 8, 7, 8, c.Height - 7);
        }
    }

    private void ByteNovaAlertDrag_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left) return;
        NativeMethods.ReleaseCapture();
        NativeMethods.SendMessage(Handle, 0xA1, 0x2, 0);
    }

    private static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool ReleaseCapture();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }

    protected override void Dispose(bool disposing)
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(alert));
            this.bunifuElipse_0 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BAAAA = new System.Windows.Forms.Label();
            this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.SuspendLayout();
            // 
            // bunifuElipse_0
            // 
            this.bunifuElipse_0.ElipseRadius = 5;
            this.bunifuElipse_0.TargetControl = this;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.PowderBlue;
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 439);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(732, 124);
            this.panel1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Mongolian Baiti", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(395, 39);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(189, 35);
            this.label3.TabIndex = 4;
            this.label3.Text = "Scan Qr For Join Our Official Teligram Chanel";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Mongolian Baiti", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(20, 39);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(209, 35);
            this.label2.TabIndex = 3;
            this.label2.Text = "Scan Qr For Join Our Official Whatsapp Chanel";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(589, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(140, 121);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(235, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(137, 118);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Mongolian Baiti", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(205, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(312, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "Express Important Message";
            // 
            // BAAAA
            // 
            this.BAAAA.BackColor = System.Drawing.Color.Azure;
            this.BAAAA.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BAAAA.ForeColor = System.Drawing.Color.Black;
            this.BAAAA.Location = new System.Drawing.Point(0, 32);
            this.BAAAA.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.BAAAA.Name = "BAAAA";
            this.BAAAA.Size = new System.Drawing.Size(730, 406);
            this.BAAAA.TabIndex = 3;
            this.BAAAA.Text = "Bhim UPI ke Server me Proble he, Jis ke wajeh se Kabhi Kabhi Reject From Bank ka " +
    "Error Aata he, Yeh Software ki Problem Nahi he, Aap kisi Other Gatway ka use kar" +
    "en taki booking fail na ho";
            this.BAAAA.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageActive = null;
            this.btnClose.Location = new System.Drawing.Point(707, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(23, 23);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnClose.TabIndex = 4;
            this.btnClose.TabStop = false;
            this.btnClose.Zoom = 10;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // alert
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(732, 563);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.BAAAA);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "alert";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "alert";
            this.Load += new System.EventHandler(this.alert_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        base.DialogResult = DialogResult.OK;
        Close();
    }

    private void linkLabel1_LinkClicked_2(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start("");
    }
}
