namespace APP_FRAMEWORK
{
    partial class frmBypass
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBypass));
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.label50 = new System.Windows.Forms.Label();
            this.btnFindTrain = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(22, 14, 28);
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Controls.Add(this.label50);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(18)))));
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(333, 23);
            this.pnlHeader.TabIndex = 1211;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageActive = null;
            this.btnClose.Location = new System.Drawing.Point(309, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(21, 21);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.TabIndex = 1;
            this.btnClose.TabStop = false;
            this.btnClose.Zoom = 10;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.BackColor = System.Drawing.Color.Transparent;
            this.label50.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label50.Location = new System.Drawing.Point(3, 1);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(220, 20);
            this.label50.TabIndex = 0;
            this.label50.Text = "Sky UPI / OTP Bypass Setting";
            // 
            // btnFindTrain
            // 
            this.btnFindTrain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnFindTrain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFindTrain.FlatAppearance.BorderSize = 0;
            this.btnFindTrain.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFindTrain.Font = new System.Drawing.Font("Sitka Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFindTrain.ForeColor = System.Drawing.Color.Black;
            this.btnFindTrain.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFindTrain.Location = new System.Drawing.Point(2, 29);
            this.btnFindTrain.Name = "btnFindTrain";
            this.btnFindTrain.Size = new System.Drawing.Size(324, 25);
            this.btnFindTrain.TabIndex = 1212;
            this.btnFindTrain.Text = "FreeCharge UPI Bypass";
            this.btnFindTrain.UseVisualStyleBackColor = false;
            this.btnFindTrain.Click += new System.EventHandler(this.btnFindTrain_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Font = new System.Drawing.Font("Sitka Text", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(2, 60);
            this.button1.Name = "button1";
            this.button1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.button1.Size = new System.Drawing.Size(324, 25);
            this.button1.TabIndex = 1213;
            this.button1.Text = "SBI Net Banking OTP Bypass";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmBypass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(333, 100);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnFindTrain);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmBypass";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmBypass";
            this.Load += new System.EventHandler(this.frmBypass_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private Bunifu.Framework.UI.BunifuImageButton btnClose;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Button btnFindTrain;
        private System.Windows.Forms.Button button1;
    }
}