namespace IRCommDLL
{
    partial class QrPay
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QrPay));
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.tktname = new System.Windows.Forms.Label();
            this.btnCancel = new Bunifu.Framework.UI.BunifuImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(1, 274);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(282, 16);
            this.label3.TabIndex = 1211;
            this.label3.Text = "Scan For Token App";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(2, 58);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(281, 213);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1210;
            this.pictureBox1.TabStop = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(-1, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(236, 34);
            this.label2.TabIndex = 1212;
            this.label2.Text = "Payment Sucess hone par QR auto \r\n                close ho jayega.";
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(22, 14, 28);
            this.pnlHeader.Controls.Add(this.tktname);
            this.pnlHeader.Controls.Add(this.btnCancel);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.ForeColor = System.Drawing.Color.White;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(283, 15);
            this.pnlHeader.TabIndex = 1215;
            // 
            // tktname
            // 
            this.tktname.AutoSize = true;
            this.tktname.BackColor = System.Drawing.Color.Transparent;
            this.tktname.ForeColor = System.Drawing.Color.Black;
            this.tktname.Location = new System.Drawing.Point(-1, 2);
            this.tktname.Name = "tktname";
            this.tktname.Size = new System.Drawing.Size(45, 13);
            this.tktname.TabIndex = 23;
            this.tktname.Text = "tktname";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImageActive = null;
            this.btnCancel.Location = new System.Drawing.Point(268, 0);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(15, 15);
            this.btnCancel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnCancel.TabIndex = 1187;
            this.btnCancel.TabStop = false;
            this.btnCancel.Zoom = 10;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // QrPay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 293);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "QrPay";
            this.Text = "QrPay";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.QrPay_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlHeader;
        internal System.Windows.Forms.Label tktname;
        private Bunifu.Framework.UI.BunifuImageButton btnCancel;
    }
}