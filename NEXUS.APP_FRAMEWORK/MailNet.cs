using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using APP_FRAMEWORK;
using Bunifu.Framework.UI;
using NEXUS.Properties;

namespace NEXUS.APP_FRAMEWORK;

public class MailNet : Form
{
	private string id;

	private IContainer components;

	private Button button1;

	private Label label1;

	private Label label2;

	private TextBox txtpass;

	private BunifuElipse bunifuElipse_0;

	private Panel pnlHeader;

	private BunifuImageButton btnClose;

	private Label B2AAA;

	private TextBox txtmail;

	internal ComboBox cboID;

	private Label lblMail;

	public MailNet()
	{
		InitializeComponent();
		pnlHeader.MouseDown += StatClass.Header_MouseDown;
		pnlHeader.MouseMove += StatClass.Header_MouseMove;
		pnlHeader.MouseUp += StatClass.Header_MouseUp;
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.Cancel;
	}

	internal void show_(string irctc)
	{
		id = irctc;
		ShowDialog();
	}

	private void button1_Click(object sender, EventArgs e)
	{
		string text = txtmail.Text.Trim();
		string text2 = txtpass.Text.Trim();
		if (!(!string.IsNullOrEmpty(text) & !string.IsNullOrEmpty(text2)))
		{
			string text3 = "Please Enter Mail ID and Password";
			MessageBox.Show(text3);
			return;
		}
		string value = "@outlook.com";
		bool flag = text.Contains(value);
		string value2 = "@hotmail.com";
		if (!(flag | text.Contains(value2)))
		{
			string text4 = "Please Enter Mail ID outlook.com or hotmail.com";
			MessageBox.Show(text4);
			return;
		}
		if (!new MailNet2().FetchEmailsByAccount(text, text2))
		{
			string text5 = "Please Enter Valid Email";
			MessageBox.Show(text5);
			return;
		}
		string text6 = "|";
		string contents = text + text6 + text2;
		File.WriteAllText(id, contents);
		base.DialogResult = DialogResult.OK;
	}

	private void MailNet_Load(object sender, EventArgs e)
	{
		cboID.Items.Clear();
		ComboBox.ObjectCollection items = cboID.Items;
		string item = "Select-ID-XXXX";
		items.Add(item);
		foreach (string key in Program.dictionary_0.Keys)
		{
			cboID.Items.Add(key);
		}
		cboID.SelectedIndex = 0;
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
		this.button1 = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtpass = new System.Windows.Forms.TextBox();
		this.bunifuElipse_0 = new Bunifu.Framework.UI.BunifuElipse(this.components);
		this.pnlHeader = new System.Windows.Forms.Panel();
		this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
		this.B2AAA = new System.Windows.Forms.Label();
		this.txtmail = new System.Windows.Forms.TextBox();
		this.cboID = new System.Windows.Forms.ComboBox();
		this.lblMail = new System.Windows.Forms.Label();
		this.pnlHeader.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnClose).BeginInit();
		base.SuspendLayout();
		this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.Location = new System.Drawing.Point(171, 81);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(75, 23);
		this.button1.TabIndex = 0;
		this.button1.Text = "SAVE";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(3, 29);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(105, 16);
		this.label1.TabIndex = 1;
		this.label1.Text = "Select IRCTC ID";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(3, 56);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(68, 16);
		this.label2.TabIndex = 1;
		this.label2.Text = "Password";
		this.txtpass.Location = new System.Drawing.Point(150, 55);
		this.txtpass.Name = "txtpass";
		this.txtpass.Size = new System.Drawing.Size(96, 20);
		this.txtpass.TabIndex = 2;
		this.bunifuElipse_0.ElipseRadius = 5;
		this.bunifuElipse_0.TargetControl = this;
		this.pnlHeader.BackColor = System.Drawing.SystemColors.ControlDarkDark;
//		this.pnlHeader.BackgroundImage = NEXUS.Properties.Resources.Topbar__0;
		this.pnlHeader.Controls.Add(this.btnClose);
		this.pnlHeader.Controls.Add(this.B2AAA);
		this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlHeader.ForeColor = System.Drawing.Color.FromArgb(17, 17, 18);
		this.pnlHeader.Location = new System.Drawing.Point(0, 0);
		this.pnlHeader.Name = "pnlHeader";
		this.pnlHeader.Size = new System.Drawing.Size(258, 22);
		this.pnlHeader.TabIndex = 1211;
		this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnClose.BackColor = System.Drawing.Color.Transparent;
		this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
//		this.btnClose.Image = NEXUS.Properties.Resources.power_button;
		this.btnClose.ImageActive = null;
		this.btnClose.Location = new System.Drawing.Point(232, 0);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(26, 20);
		this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.btnClose.TabIndex = 1;
		this.btnClose.TabStop = false;
		this.btnClose.Zoom = 10;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		this.B2AAA.AutoSize = true;
		this.B2AAA.BackColor = System.Drawing.Color.Transparent;
		this.B2AAA.Enabled = false;
		this.B2AAA.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.B2AAA.ForeColor = System.Drawing.Color.White;
		this.B2AAA.Location = new System.Drawing.Point(3, 2);
		this.B2AAA.Name = "B2AAA";
		this.B2AAA.Size = new System.Drawing.Size(80, 17);
		this.B2AAA.TabIndex = 0;
		this.B2AAA.Text = "Add Email";
		this.txtmail.Location = new System.Drawing.Point(49, 180);
		this.txtmail.Name = "txtmail";
		this.txtmail.Size = new System.Drawing.Size(197, 20);
		this.txtmail.TabIndex = 2;
		this.cboID.BackColor = System.Drawing.SystemColors.Window;
		this.cboID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboID.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.cboID.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cboID.ForeColor = System.Drawing.Color.Black;
		this.cboID.FormattingEnabled = true;
        this.cboID.Items.AddRange(new object[8] { "-Select-", "A1A2A3", "B1B2B3", "C1C2C3", "D1D2D3", "E1E2E3", "F1F2F3", "G1G2G3" });
		this.cboID.Location = new System.Drawing.Point(114, 28);
		this.cboID.Name = "cboID";
		this.cboID.Size = new System.Drawing.Size(132, 21);
		this.cboID.TabIndex = 1212;
		this.lblMail.AutoSize = true;
		this.lblMail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblMail.Location = new System.Drawing.Point(3, 84);
		this.lblMail.Name = "lblMail";
		this.lblMail.Size = new System.Drawing.Size(68, 16);
		this.lblMail.TabIndex = 1;
		this.lblMail.Text = "Password";
		this.lblMail.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(258, 212);
		base.Controls.Add(this.cboID);
		base.Controls.Add(this.pnlHeader);
		base.Controls.Add(this.txtpass);
		base.Controls.Add(this.txtmail);
		base.Controls.Add(this.lblMail);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.button1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "MailNet";
		base.ShowIcon = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "MailNet";
		base.TopMost = true;
		base.Load += new System.EventHandler(MailNet_Load);
		this.pnlHeader.ResumeLayout(false);
		this.pnlHeader.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnClose).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
