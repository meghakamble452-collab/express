using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Bunifu.Framework.UI;

namespace APP_FRAMEWORK;

public class UpiBalnce : Form
{
    private IContainer components = null;
    private Label label1;
    private Button button1;
    private Label label2;
    private BunifuImageButton btnClose;

    public UpiBalnce()
    {
        InitializeComponent();
        ApplyByteNovaTheme();
    }

    private void ApplyByteNovaTheme()
    {
        BackColor = Color.FromArgb(3, 13, 29);
        ForeColor = Color.White;
        FormBorderStyle = FormBorderStyle.None;
        ClientSize = new Size(412, 153);

        label2.AutoSize = false;
        label2.SetBounds(18, 15, 335, 30);
        label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
        label2.ForeColor = Color.FromArgb(198, 244, 50);
        label2.BackColor = Color.Transparent;
        label2.TextAlign = ContentAlignment.MiddleCenter;

        label1.AutoSize = false;
        label1.SetBounds(18, 57, 376, 37);
        label1.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
        label1.ForeColor = Color.White;
        label1.BackColor = Color.FromArgb(8, 34, 58);
        label1.TextAlign = ContentAlignment.MiddleCenter;

        button1.SetBounds(153, 106, 106, 34);
        button1.BackColor = Color.FromArgb(0, 125, 175);
        button1.ForeColor = Color.White;
        button1.FlatStyle = FlatStyle.Flat;
        button1.FlatAppearance.BorderSize = 1;
        button1.FlatAppearance.BorderColor = Color.FromArgb(0, 225, 255);
        button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 150, 205);
        button1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

        btnClose.SetBounds(372, 7, 31, 27);
        btnClose.BackColor = Color.FromArgb(185, 38, 55);
        btnClose.SizeMode = PictureBoxSizeMode.Zoom;
        btnClose.Zoom = 0;
        btnClose.Cursor = Cursors.Hand;

        Paint += ByteNovaPaint;
    }

    private void ByteNovaPaint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (Pen outer = new Pen(Color.FromArgb(198, 244, 50), 1.5F))
            e.Graphics.DrawRectangle(outer, 2, 2, ClientSize.Width - 5, ClientSize.Height - 5);
        using (Pen inner = new Pen(Color.FromArgb(65, 80, 120, 255), 1F))
            e.Graphics.DrawRectangle(inner, 5, 5, ClientSize.Width - 11, ClientSize.Height - 11);
        using (Pen gold = new Pen(Color.FromArgb(255, 202, 62), 1F))
            e.Graphics.DrawLine(gold, 18, 49, ClientSize.Width - 18, 49);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        Close();
    }

    public void Form1_Load(string strin_1, string strin_2)
    {
        label1.Visible = true;
        label1.Text = strin_1;
        label2.Visible = true;
        label2.Text = strin_2;
        ShowDialog();
    }

    private void UpiBalnce_Load(object sender, EventArgs e)
    {
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(APP_FRAMEWORK.UpiBalnce));
        this.label1 = new System.Windows.Forms.Label();
        this.button1 = new System.Windows.Forms.Button();
        this.label2 = new System.Windows.Forms.Label();
        this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
        ((System.ComponentModel.ISupportInitialize)this.btnClose).BeginInit();
        base.SuspendLayout();
        this.label1.AutoSize = true;
        this.label1.Font = new System.Drawing.Font("Palatino Linotype", 13.8f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.label1.Location = new System.Drawing.Point(12, 74);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(78, 31);
        this.label1.TabIndex = 0;
        this.label1.Text = "label1";
        this.button1.BackColor = System.Drawing.Color.FromArgb(192, 255, 192);
        this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.button1.Location = new System.Drawing.Point(148, 110);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(106, 37);
        this.button1.TabIndex = 1;
        this.button1.Text = "OK";
        this.button1.UseVisualStyleBackColor = false;
        this.button1.Click += new System.EventHandler(button1_Click);
        this.label2.AutoSize = true;
        this.label2.Font = new System.Drawing.Font("Palatino Linotype", 13.8f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
        this.label2.Location = new System.Drawing.Point(131, 25);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(78, 31);
        this.label2.TabIndex = 2;
        this.label2.Text = "label2";
        this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        this.btnClose.BackColor = System.Drawing.Color.Transparent;
        this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        //this.btnClose.Image = (System.Drawing.Image)resources.GetObject("btnClose.Image");
        this.btnClose.ImageActive = null;
        this.btnClose.Location = new System.Drawing.Point(55, 15);
        this.btnClose.Margin = new System.Windows.Forms.Padding(4);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(70, 44);
        this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.btnClose.TabIndex = 3;
        this.btnClose.TabStop = false;
        this.btnClose.Zoom = 10;
        this.btnClose.Click += new System.EventHandler(button1_Click);
        base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.Color.FromArgb(192, 255, 255);
        base.ClientSize = new System.Drawing.Size(412, 153);
        base.Controls.Add(this.btnClose);
        base.Controls.Add(this.label2);
        base.Controls.Add(this.button1);
        base.Controls.Add(this.label1);
        base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        //base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
        base.Name = "UpiBalnce";
        base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "UpiBalnce Check";
        base.Load += new System.EventHandler(UpiBalnce_Load);
        ((System.ComponentModel.ISupportInitialize)this.btnClose).EndInit();
        base.ResumeLayout(false);
        base.PerformLayout();
    }
}
