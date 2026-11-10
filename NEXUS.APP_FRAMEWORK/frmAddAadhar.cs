using System;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using APP_FRAMEWORK;
using Bunifu.Framework.UI;
using NEXUS.Properties;

namespace NEXUS.APP_FRAMEWORK;

public class frmAddAadhar : Form
{
	private IContainer components;

	private BunifuElipse bunifuElipse1;

	private Panel pnlHeader;

	private BunifuImageButton btnClose;

	internal Label formName;

	private Button button1;

	private Label label2;

	private Label label1;

	private TextBox textBox1;

	private ComboBox comboBox1;

	private Label label3;

	private PictureBox pictureBox1;

	private Label label4;

	public frmAddAadhar()
	{
		InitializeComponent();
		pnlHeader.MouseDown += StatClass.Header_MouseDown;
		pnlHeader.MouseMove += StatClass.Header_MouseMove;
		pnlHeader.MouseUp += StatClass.Header_MouseUp;
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void frmAddAadhar_Load(object sender, EventArgs e)
	{
		string path = "AADHAR";
		if (!Directory.Exists(path))
		{
			string path2 = "AADHAR";
			Directory.CreateDirectory(path2);
		}
		comboBox1.Items.Clear();
		ComboBox.ObjectCollection items = comboBox1.Items;
		string item = "Select ID";
		items.Add(item);
		DataTable dataTable = new DataTable();
		string commandText = "SELECT * FROM TBL_USER";
		new SQLiteDataAdapter(commandText, Class34.string_Database).Fill(dataTable);
		int num = 0;
		if (dataTable != null && dataTable.Rows.Count > 0)
		{
			foreach (DataRow row in dataTable.Rows)
			{
				num++;
				try
				{
					string columnName = "UserName";
					string item2 = row[columnName].ToString();
					comboBox1.Items.Add(item2);
				}
				catch
				{
				}
			}
		}
		comboBox1.SelectedIndex = 0;
	}

	private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
	{
		if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
		{
			e.Handled = true;
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		if (comboBox1.SelectedIndex <= 0)
		{
			return;
		}
		string itemText = comboBox1.GetItemText(comboBox1.SelectedItem);
		if (textBox1.Text.Length == 4)
		{
			string path = "AADHAR";
			string path2 = Path.Combine(path, itemText);
			if (File.Exists(path2))
			{
				File.Delete(path2);
			}
			File.WriteAllText(path2, textBox1.Text);
			Form owner = new Form
			{
				TopMost = true
			};
			string text = "AADHAR Last 4 Digit Saved";
			string caption = "Success";
			MessageBox.Show(owner, text, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			comboBox1.SelectedIndex = 0;
			textBox1.Text = string.Empty;
		}
	}

	private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (comboBox1.SelectedIndex <= 0)
		{
			return;
		}
		string itemText = comboBox1.GetItemText(comboBox1.SelectedItem);
		string path = "AADHAR";
		string path2 = Path.Combine(path, itemText);
		if (File.Exists(path2))
		{
			string text = File.ReadAllText(path2);
			if (text.Length > 3)
			{
				textBox1.Text = text;
			}
			else
			{
				textBox1.Text = string.Empty;
			}
		}
		else
		{
			textBox1.Text = string.Empty;
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
		this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
		this.comboBox1 = new System.Windows.Forms.ComboBox();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.pnlHeader = new System.Windows.Forms.Panel();
		this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
		this.formName = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.pnlHeader.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnClose).BeginInit();
		base.SuspendLayout();
		this.bunifuElipse1.ElipseRadius = 5;
		this.bunifuElipse1.TargetControl = this;
		this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.comboBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.comboBox1.FormattingEnabled = true;
		this.comboBox1.Location = new System.Drawing.Point(108, 39);
		this.comboBox1.Name = "comboBox1";
		this.comboBox1.Size = new System.Drawing.Size(143, 21);
		this.comboBox1.TabIndex = 132;
		this.comboBox1.SelectedIndexChanged += new System.EventHandler(comboBox1_SelectedIndexChanged);
		this.textBox1.Location = new System.Drawing.Point(173, 71);
		this.textBox1.MaxLength = 4;
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(78, 20);
		this.textBox1.TabIndex = 133;
		this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(textBox1_KeyPress);
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(3, 42);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(86, 13);
		this.label1.TabIndex = 134;
		this.label1.Text = "Select IRCTC ID";
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(3, 74);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(97, 13);
		this.label2.TabIndex = 135;
		this.label2.Text = "Last 4 Digit Aadhar";
		this.button1.Location = new System.Drawing.Point(187, 97);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(64, 34);
		this.button1.TabIndex = 136;
		this.button1.Text = "SAVE";
		this.button1.UseVisualStyleBackColor = true;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.label3.Location = new System.Drawing.Point(3, 103);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(184, 30);
		this.label3.TabIndex = 137;
		this.label3.Text = "Note ! IRCTC ID Select karke AADHAR ka Last 4 Digit Enter kaen";
		this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
		this.pictureBox1.Location = new System.Drawing.Point(26, 137);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(202, 191);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.pictureBox1.TabIndex = 138;
		this.pictureBox1.TabStop = false;
		this.pnlHeader.AutoSize = true;
		this.pnlHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
		this.pnlHeader.BackColor = System.Drawing.SystemColors.ControlDarkDark;
//		this.pnlHeader.BackgroundImage = NEXUS.Properties.Resources.titlebar;
		this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.pnlHeader.Controls.Add(this.btnClose);
		this.pnlHeader.Controls.Add(this.formName);
		this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlHeader.ForeColor = System.Drawing.Color.Yellow;
		this.pnlHeader.Location = new System.Drawing.Point(0, 0);
		this.pnlHeader.Margin = new System.Windows.Forms.Padding(2);
		this.pnlHeader.Name = "pnlHeader";
		this.pnlHeader.Size = new System.Drawing.Size(265, 23);
		this.pnlHeader.TabIndex = 131;
		this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnClose.BackColor = System.Drawing.Color.Transparent;
		this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
//		this.btnClose.Image = NEXUS.Properties.Resources.controlbutton_close;
		this.btnClose.ImageActive = null;
		this.btnClose.Location = new System.Drawing.Point(239, -1);
		this.btnClose.Margin = new System.Windows.Forms.Padding(2);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(20, 20);
		this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.btnClose.TabIndex = 25;
		this.btnClose.TabStop = false;
		this.btnClose.Zoom = 10;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		this.formName.AutoSize = true;
		this.formName.BackColor = System.Drawing.Color.Transparent;
		this.formName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.formName.ForeColor = System.Drawing.Color.Black;
		this.formName.Location = new System.Drawing.Point(57, 0);
		this.formName.Name = "formName";
		this.formName.Size = new System.Drawing.Size(94, 16);
		this.formName.TabIndex = 23;
		this.formName.Text = "ADD AAdhar";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(2, 331);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(259, 13);
		this.label4.TabIndex = 139;
		this.label4.Text = "OTP Reader Download karne ke liye QR Scan Karen";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(265, 349);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.pictureBox1);
		base.Controls.Add(this.label3);
		base.Controls.Add(this.button1);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.textBox1);
		base.Controls.Add(this.comboBox1);
		base.Controls.Add(this.pnlHeader);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Name = "frmAddAadhar";
		base.ShowIcon = false;
		base.ShowInTaskbar = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "frmAddAadhar";
		base.TopMost = true;
		base.Load += new System.EventHandler(frmAddAadhar_Load);
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.pnlHeader.ResumeLayout(false);
		this.pnlHeader.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnClose).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
