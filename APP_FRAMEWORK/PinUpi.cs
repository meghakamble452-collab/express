using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace APP_FRAMEWORK;

public class PinUpi : Form
{
    internal string bvalue;
    private IContainer components;
    private Label label1;
    private TextBox textBox1;
    private Button button1;
    private Panel pnlHeader;
    private Label label5;
    private Bunifu.Framework.UI.BunifuImageButton bunifuImageButton1;
    private Label label2;

    public PinUpi()
    {
        InitializeComponent();
        ApplyByteNovaTheme();
    }

    private void ApplyByteNovaTheme()
    {
        BackColor = Color.FromArgb(3, 13, 29);
        ForeColor = Color.White;
        FormBorderStyle = FormBorderStyle.None;
        ClientSize = new Size(453, 164);

        pnlHeader.Dock = DockStyle.None;
        pnlHeader.SetBounds(4, 4, 445, 30);
        pnlHeader.BackColor = Color.FromArgb(5, 29, 56);

        label5.AutoSize = false;
        label5.SetBounds(12, 4, 380, 22);
        label5.Text = "UPI PIN  _  EXPRESS";
        label5.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        label5.ForeColor = Color.White;
        label5.BackColor = Color.Transparent;
        label5.TextAlign = ContentAlignment.MiddleLeft;

        bunifuImageButton1.SetBounds(414, 3, 27, 24);
        bunifuImageButton1.BackColor = Color.FromArgb(185, 38, 55);
        bunifuImageButton1.Zoom = 0;

        label2.SetBounds(135, 42, 180, 23);
        label2.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
        label2.ForeColor = Color.FromArgb(255, 202, 62);
        label2.BackColor = Color.Transparent;
        label2.TextAlign = ContentAlignment.MiddleCenter;

        label1.AutoSize = false;
        label1.SetBounds(18, 75, 200, 27);
        label1.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
        label1.ForeColor = Color.FromArgb(225, 240, 250);
        label1.BackColor = Color.Transparent;
        label1.TextAlign = ContentAlignment.MiddleRight;

        textBox1.SetBounds(226, 75, 180, 27);
        textBox1.BackColor = Color.FromArgb(9, 39, 65);
        textBox1.ForeColor = Color.White;
        textBox1.BorderStyle = BorderStyle.FixedSingle;
        textBox1.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        textBox1.TextAlign = HorizontalAlignment.Center;
        textBox1.UseSystemPasswordChar = true;

        button1.SetBounds(173, 116, 108, 34);
        button1.BackColor = Color.FromArgb(0, 125, 175);
        button1.ForeColor = Color.White;
        button1.FlatStyle = FlatStyle.Flat;
        button1.FlatAppearance.BorderSize = 1;
        button1.FlatAppearance.BorderColor = Color.FromArgb(0, 225, 255);
        button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 150, 205);
        button1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

        Paint += ByteNovaPaint;
    }

    private void ByteNovaPaint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (Pen p = new Pen(Color.FromArgb(198, 244, 50), 1.5F))
            e.Graphics.DrawRectangle(p, 2, 2, ClientSize.Width - 5, ClientSize.Height - 5);
        using (Pen p = new Pen(Color.FromArgb(55, 80, 120, 255), 1F))
            e.Graphics.DrawRectangle(p, 5, 5, ClientSize.Width - 11, ClientSize.Height - 11);
        using (Pen p = new Pen(Color.FromArgb(255, 202, 62), 1F))
            e.Graphics.DrawLine(p, 18, 68, ClientSize.Width - 18, 68);
    }

    private void label1_Click(object sender, EventArgs e) { }

    private void button1_Click(object sender, EventArgs e)
    {
        bvalue = textBox1.Text.Trim();
        Close();
    }

    private void label2_Click(object sender, EventArgs e) { }

    public void Form1_Load(string strin_1, string strin_2)
    {
        label1.Visible = true;
        label1.Text = "Enter Upi Pin For==> " + strin_1;
        label2.Text = "A/C-" + strin_2;
        ShowDialog();
    }

    private void textBox1_TextChanged(object sender, EventArgs e) { }
    private void PinUpi_Load(object sender, EventArgs e) { }

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PinUpi));
        this.label1 = new System.Windows.Forms.Label();
        this.textBox1 = new System.Windows.Forms.TextBox();
        this.button1 = new System.Windows.Forms.Button();
        this.label2 = new System.Windows.Forms.Label();
        this.pnlHeader = new System.Windows.Forms.Panel();
        this.label5 = new System.Windows.Forms.Label();
        this.bunifuImageButton1 = new Bunifu.Framework.UI.BunifuImageButton();
        this.pnlHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton1)).BeginInit();
        this.SuspendLayout();
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(12, 77);
        this.label1.Name = "label1";
        this.label1.Text = "Enter Upi Pin";
        this.label1.Click += new System.EventHandler(this.label1_Click);
        this.textBox1.Location = new System.Drawing.Point(133, 74);
        this.textBox1.Name = "textBox1";
        this.textBox1.Size = new System.Drawing.Size(142, 26);
        this.textBox1.TabIndex = 1;
        this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
        this.button1.Location = new System.Drawing.Point(133, 113);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(142, 40);
        this.button1.TabIndex = 3;
        this.button1.Text = "OK";
        this.button1.Click += new System.EventHandler(this.button1_Click);
        this.label2.Location = new System.Drawing.Point(137, 37);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(125, 25);
        this.label2.Text = "Enter Upi Pin";
        this.pnlHeader.Controls.Add(this.label5);
        this.pnlHeader.Controls.Add(this.bunifuImageButton1);
        this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlHeader.Name = "pnlHeader";
        this.pnlHeader.Size = new System.Drawing.Size(453, 22);
        this.label5.AutoSize = true;
        this.label5.Location = new System.Drawing.Point(127, 3);
        this.label5.Name = "label5";
        this.label5.Text = "SBI Upi Bypass_Express";
        this.bunifuImageButton1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        this.bunifuImageButton1.Cursor = System.Windows.Forms.Cursors.Hand;
        this.bunifuImageButton1.Image = ((System.Drawing.Image)(resources.GetObject("bunifuImageButton1.Image")));
        this.bunifuImageButton1.ImageActive = null;
        this.bunifuImageButton1.Location = new System.Drawing.Point(431, 0);
        this.bunifuImageButton1.Name = "bunifuImageButton1";
        this.bunifuImageButton1.Size = new System.Drawing.Size(22, 22);
        this.bunifuImageButton1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.bunifuImageButton1.TabStop = false;
        this.bunifuImageButton1.Click += new System.EventHandler(this.bunifuImageButton1_Click);
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(453, 164);
        this.Controls.Add(this.pnlHeader);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.textBox1);
        this.Controls.Add(this.label1);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.Name = "PinUpi";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "PinUpi";
        this.Load += new System.EventHandler(this.PinUpi_Load);
        this.pnlHeader.ResumeLayout(false);
        this.pnlHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton1)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private void bunifuImageButton1_Click(object sender, EventArgs e)
    {
        base.Close();
    }
}
