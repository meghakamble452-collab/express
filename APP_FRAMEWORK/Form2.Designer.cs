namespace APP_FRAMEWORK
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.formName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(396, 49);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(8, 8);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Info;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button2.Location = new System.Drawing.Point(365, 84);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(180, 33);
            this.button2.TabIndex = 1;
            this.button2.Text = "Check IP Block";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.MistyRose;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Location = new System.Drawing.Point(365, 40);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(180, 35);
            this.button3.TabIndex = 2;
            this.button3.Text = "Check IP Block";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // pnlHeader
            // 
            this.pnlHeader.AutoSize = true;
            this.pnlHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(22, 14, 28);
            this.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Controls.Add(this.formName);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.ForeColor = System.Drawing.Color.Yellow;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Margin = new System.Windows.Forms.Padding(2);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(557, 25);
            this.pnlHeader.TabIndex = 131;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageActive = null;
            this.btnClose.Location = new System.Drawing.Point(531, -1);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(22, 22);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnClose.TabIndex = 25;
            this.btnClose.TabStop = false;
            this.btnClose.Zoom = 10;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // formName
            // 
            this.formName.AutoSize = true;
            this.formName.BackColor = System.Drawing.Color.Transparent;
            this.formName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.formName.ForeColor = System.Drawing.Color.Black;
            this.formName.Location = new System.Drawing.Point(3, 4);
            this.formName.Name = "formName";
            this.formName.Size = new System.Drawing.Size(141, 16);
            this.formName.TabIndex = 23;
            this.formName.Text = "Web Login Settings";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(171, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(178, 16);
            this.label1.TabIndex = 132;
            this.label1.Text = "Web-3 ( Microsoft Edge )";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(163, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(186, 16);
            this.label2.TabIndex = 133;
            this.label2.Text = "Web- 2 ( Google Chrome )";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(234, 138);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 16);
            this.label3.TabIndex = 135;
            this.label3.Text = "Web ( Brave )";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.Color.MistyRose;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button4.Location = new System.Drawing.Point(365, 129);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(180, 35);
            this.button4.TabIndex = 134;
            this.button4.Text = "Check IP Block";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(-2, 322);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(556, 97);
            this.label4.TabIndex = 136;
            this.label4.Text = resources.GetString("label4.Text");
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(154, 124);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 137;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(234, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 16);
            this.label5.TabIndex = 139;
            this.label5.Text = "Web ( Opera )";
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.MistyRose;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button5.Location = new System.Drawing.Point(365, 180);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(180, 35);
            this.button5.TabIndex = 138;
            this.button5.Text = "Check IP Block";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(234, 285);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 16);
            this.label6.TabIndex = 141;
            this.label6.Text = "APK Token Get";
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.MistyRose;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button6.Location = new System.Drawing.Point(365, 276);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(180, 35);
            this.button6.TabIndex = 140;
            this.button6.Text = "Get Token From Apk";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(4, 302);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(127, 16);
            this.label7.TabIndex = 142;
            this.label7.Text = "Token Count :-   0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(234, 238);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 16);
            this.label8.TabIndex = 144;
            this.label8.Text = "Web ( Comet )";
            // 
            // button7
            // 
            this.button7.BackColor = System.Drawing.Color.MistyRose;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button7.Location = new System.Drawing.Point(365, 229);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(180, 35);
            this.button7.TabIndex = 143;
            this.button7.Text = "Check IP Block";
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(0, 173);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(154, 124);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 145;
            this.pictureBox2.TabStop = false;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 419);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Panel pnlHeader;
        private Bunifu.Framework.UI.BunifuImageButton btnClose;
        internal System.Windows.Forms.Label formName;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label label2;
        internal System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button4;
        internal System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        internal System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button5;
        internal System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}