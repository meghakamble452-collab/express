using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using APP_FRAMEWORK;
using Bunifu.Framework.UI;
using Newtonsoft.Json;
using NEXUS.Properties;

namespace NEXUS.APP_FRAMEWORK;

public class frmToken : Form
{
	private string jsonFilePath = "SensorList.json";

	private IContainer components;

	private Button button1;

	private BunifuElipse bunifuElipse1;

	private Label label1;

	private Panel pnlHeader;

	private BunifuImageButton btnClose;

	private Label B2AAA;

	public frmToken()
	{
		InitializeComponent();
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		Close();
	}
    private string ReadOTP()
    {
        string empty = string.Empty;
        string text = string.Empty;
        string text2 = "https://novabyte.in/Api/sensor";
        string requestUriString = text2;
        string[] array = new string[5];
        string text3 = "{\"userId\":\"";
        array[0] = text3;
        array[1] = Program.clientID;
        string text4 = "\", \"LastChar\":\"";
        array[2] = text4;
        string text5 = "0";
        array[3] = text5;
        string text6 = "\"}";
        array[4] = text6;
        string text7 = string.Concat(array);
        while (true)
        {
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                string contentType = "application/json";
                httpWebRequest.ContentType = contentType;
                string method = "POST";
                httpWebRequest.Method = method;
                httpWebRequest.Timeout = 10000;
                httpWebRequest.ReadWriteTimeout = 10000;
                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string value = text7;
                    streamWriter.Write(value);
                }
                using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
                empty = streamReader.ReadToEnd();
                string text8 = empty;
                string value2 = "6,a,";
                if (text8.Contains(value2))
                {
                    text = empty;
                    ProcessAndSaveSensorData(text);
                }
            }
            catch (WebException ex)
            {
                Application.DoEvents();
                Thread.Sleep(500);
                continue;
            }
            break;
        }
        return text;
    }

    private string ReadOTPAaa()
	{
		string empty = string.Empty;
		string text = string.Empty;
		string text2 = "https://novabyte.in/Api/sensor";
		string requestUriString = text2;
		string[] array = new string[5];
		string text3 = "{\"userId\":\"";
		array[0] = text3;
		array[1] = Program.clientID;
		string text4 = "\", \"LastChar\":\"";
		array[2] = text4;
		string text5 = "0";
		array[3] = text5;
		string text6 = "\"}";
		array[4] = text6;
		string text7 = string.Concat(array);
		while (true)
		{
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
				string contentType = "application/json";
				httpWebRequest.ContentType = contentType;
				string method = "POST";
				httpWebRequest.Method = method;
				httpWebRequest.Timeout = 10000;
				httpWebRequest.ReadWriteTimeout = 10000;
				using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
				{
					string value = text7;
					streamWriter.Write(value);
				}
				using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
				empty = streamReader.ReadToEnd();
				string text8 = empty;
				string value2 = "6,a,";
				if (text8.Contains(value2))
				{
					text = empty;
					ProcessAndSaveSensorData(text);
				}
			}
			catch (WebException)
			{
				Application.DoEvents();
				Thread.Sleep(500);
				continue;
			}
			break;
		}
		return text;
	}

	private void ProcessAndSaveSensorData(string rawApiResponse)
	{
		try
		{
			List<string> list = JsonConvert.DeserializeObject<List<string>>(rawApiResponse);
			if (list == null || list.Count == 0)
			{
				return;
			}
			List<string> list2 = new List<string>();
			if (File.Exists(jsonFilePath))
			{
				list2 = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(jsonFilePath)) ?? new List<string>();
			}
			bool flag = false;
			foreach (string item in list)
			{
				string value = "6,a,";
				if (item.StartsWith(value) && !list2.Contains(item))
				{
					list2.Add(item);
					flag = true;
					string value2 = "New Sensor Data Found and Added.";
					Console.WriteLine(value2);
				}
			}
			if (flag)
			{
				string contents = JsonConvert.SerializeObject(list2, Formatting.Indented);
				File.WriteAllText(jsonFilePath, contents);
			}
			else
			{
				string value3 = "No new unique data to add.";
				Console.WriteLine(value3);
			}
		}
		catch (Exception ex)
		{
			string text = "Error processing data: ";
			Console.WriteLine(text + ex.Message);
		}
	}

	private void SaveToLocalJson(string rawData)
	{
		try
		{
			List<string> list = ((!File.Exists(jsonFilePath)) ? new List<string>() : (JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(jsonFilePath)) ?? new List<string>()));
			if (!list.Contains(rawData))
			{
				list.Add(rawData);
				string contents = JsonConvert.SerializeObject(list, Formatting.Indented);
				File.WriteAllText(jsonFilePath, contents);
				string value = "New data added successfully.";
				Console.WriteLine(value);
			}
			else
			{
				string value2 = "Data already exists in file. Skipping...";
				Console.WriteLine(value2);
			}
		}
		catch (Exception ex)
		{
			string text = "Error: ";
			Console.WriteLine(text + ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		ReadOTP();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmToken));
            this.button1 = new System.Windows.Forms.Button();
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.B2AAA = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(64, 62);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "Check Token";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 5;
            this.bunifuElipse1.TargetControl = this;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(38, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Token Count :-   0";
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(22, 14, 28);
            this.pnlHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Controls.Add(this.B2AAA);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(18)))));
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(223, 16);
            this.pnlHeader.TabIndex = 1212;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageActive = null;
            this.btnClose.Location = new System.Drawing.Point(209, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(14, 16);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.TabIndex = 1;
            this.btnClose.TabStop = false;
            this.btnClose.Zoom = 10;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // B2AAA
            // 
            this.B2AAA.AutoSize = true;
            this.B2AAA.BackColor = System.Drawing.Color.Transparent;
            this.B2AAA.Dock = System.Windows.Forms.DockStyle.Left;
            this.B2AAA.Enabled = false;
            this.B2AAA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.B2AAA.ForeColor = System.Drawing.Color.Black;
            this.B2AAA.Location = new System.Drawing.Point(0, 0);
            this.B2AAA.Name = "B2AAA";
            this.B2AAA.Size = new System.Drawing.Size(44, 13);
            this.B2AAA.TabIndex = 0;
            this.B2AAA.Text = "TOKEN";
            // 
            // frmToken
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(223, 102);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmToken";
            this.Text = "frmToken";
            this.Load += new System.EventHandler(this.frmToken_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

	}
    private void Refreshtoken()
    {
        string text = "SensorList.json";
        string path = text;
        string result = string.Empty;
        try
        {
            if (!File.Exists(path))
            {
                string value = "File nahi mili!";
                Console.WriteLine(value);
            }
            List<string> list = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(path)) ?? new List<string>();
            if (list.Count <= 0)
            {
                string value2 = "JSON file khali hai!";
                Console.WriteLine(value2);
            }
            label1.Text = "Token Count :-  " + list.Count.ToString();
        }
        catch { }
    }

    private void frmToken_Load(object sender, EventArgs e)
    {
        Refreshtoken();
    }
}
