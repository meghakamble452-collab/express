using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Bunifu.Framework.UI;
using NEXUS.Properties;

namespace APP_FRAMEWORK;

public partial class frmAddID : Form
{
    private IContainer components;

    private BunifuElipse bunifuElipse1;

    private Panel pnlHeader;

    private BunifuImageButton btnClose;

    internal Label formName;

    private Panel PnlBase;

    internal tabHistory tabHistory;

    internal tabIrctcId tabIrctcId;

    public frmAddID()
    {
        InitializeComponent();
        ApplyByteNovaLatestUI();
        ApplyExpressTheme();
        pnlHeader.MouseDown += StatClass.Header_MouseDown;
        pnlHeader.MouseMove += StatClass.Header_MouseMove;
        pnlHeader.MouseUp += StatClass.Header_MouseUp;
        base.Width = 764;
        base.Height = 393;
    }

    private void BtnClose_Click(object sender, EventArgs e)
    {
        if (tabHistory.Visible)
        {
            labe6_MouseUp2(sender, new EventArgs());
        }
        else
        {
            Close();
        }
    }

    private void FrmAddID_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    private void method_0()
    {
        tabIrctcId.Visible = false;
        tabHistory.Visible = false;
    }

    private void Label6_MouseUp(object sender, MouseEventArgs e)
    {
        method_0();
        tabHistory.Visible = true;
        tabHistory.BringToFront();
    }

    internal void labe6_MouseUp(object sender, EventArgs eventArgs)
    {
        method_0();
        tabHistory.Visible = true;
        tabHistory.BringToFront();
    }

    internal void labe6_MouseUp2(object sender, EventArgs eventArgs)
    {
        method_0();
        tabIrctcId.Visible = true;
        tabIrctcId.BringToFront();
    }

    private void frmAddID_Load(object sender, EventArgs e)
    {
        tabHistory.Visible = false;
    }


    // ---------------------------------------------------------------
    // ByteNova Latest UI - visual redesign only.
    // Existing tab/history/close behavior is preserved.
    // ---------------------------------------------------------------
    private void ApplyByteNovaLatestUI()
    {
        SuspendLayout();

        AutoSize = false;
        AutoSizeMode = AutoSizeMode.GrowOnly;
        FormBorderStyle = FormBorderStyle.None;
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(764, 393);
        MinimumSize = new Size(764, 393);
        BackColor = Color.FromArgb(1, 9, 18);
        ForeColor = Color.White;
        DoubleBuffered = true;
        Paint -= ByteNovaFormPaint;
        Paint += ByteNovaFormPaint;

        // frmMain-style compact ByteNova header.
        pnlHeader.AutoSize = false;
        pnlHeader.Dock = DockStyle.None;
        pnlHeader.Location = new Point(5, 5);
        pnlHeader.Size = new Size(754, 23);
        pnlHeader.BorderStyle = BorderStyle.None;
        pnlHeader.BackColor = Color.Transparent;
        pnlHeader.Paint -= pnlHeader_Paint;
        pnlHeader.Paint += pnlHeader_Paint;

        formName.AutoSize = false;
        formName.Location = new Point(9, 1);
        formName.Size = new Size(300, 21);
        formName.Text = "Express  •  ADD IRCTC ID";
        formName.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        formName.ForeColor = Color.White;
        formName.TextAlign = ContentAlignment.MiddleLeft;
        formName.BackColor = Color.Transparent;

        // frmMain-style Close button.
        btnClose.Location = new Point(727, 1);
        btnClose.Size = new Size(22, 21);
        btnClose.Paint -= ByteNovaClosePaint;
        btnClose.BackColor = Color.Transparent;
        btnClose.BackgroundImage = null;
        btnClose.BackgroundImageLayout = ImageLayout.Zoom;
        btnClose.SizeMode = PictureBoxSizeMode.Zoom;
        btnClose.Zoom = 0;
        btnClose.BringToFront();

        // Main content shell.
        PnlBase.AutoSize = false;
        PnlBase.Location = new Point(5, 33);
        PnlBase.Size = new Size(754, 355);
        PnlBase.BackColor = Color.FromArgb(3, 18, 32);
        PnlBase.Paint -= ByteNovaBasePaint;
        PnlBase.Paint += ByteNovaBasePaint;

        // Preserve both existing UserControls, only resize/reposition them.
        tabIrctcId.Location = new Point(6, 6);
        tabIrctcId.Size = new Size(742, 343);
        tabIrctcId.BackColor = SystemColors.Control;
        tabIrctcId.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        tabHistory.Location = new Point(6, 6);
        tabHistory.Size = new Size(742, 343);
        tabHistory.BackColor = SystemColors.ActiveCaption;
        tabHistory.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        // Keep all legacy grid values permanently readable.
        FixGridReadability(tabIrctcId);
        FixGridReadability(tabHistory);

        pnlHeader.BringToFront();
        btnClose.BringToFront();

        ResumeLayout(false);
        PerformLayout();
        Invalidate();
    }


    private void FixGridReadability(Control root)
    {
        foreach (Control child in root.Controls)
        {
            DataGridView grid = child as DataGridView;
            if (grid != null)
            {
                grid.EnableHeadersVisualStyles = false;
                grid.BackgroundColor = Color.White;
                grid.GridColor = Color.FromArgb(215, 220, 225);

                grid.DefaultCellStyle.BackColor = Color.White;
                grid.DefaultCellStyle.ForeColor = Color.Black;
                grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(205, 235, 250);
                grid.DefaultCellStyle.SelectionForeColor = Color.Black;

                grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 250, 252);
                grid.AlternatingRowsDefaultCellStyle.ForeColor = Color.Black;

                grid.RowsDefaultCellStyle.BackColor = Color.White;
                grid.RowsDefaultCellStyle.ForeColor = Color.Black;

                grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 242, 247);
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(235, 242, 247);
                grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;

                grid.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(235, 242, 247);
                grid.RowHeadersDefaultCellStyle.ForeColor = Color.Black;

                // Link columns also remain visible without clicking.
                foreach (DataGridViewColumn col in grid.Columns)
                {
                    DataGridViewLinkColumn link = col as DataGridViewLinkColumn;
                    if (link != null)
                    {
                        link.LinkColor = Color.Blue;
                        link.ActiveLinkColor = Color.FromArgb(0, 90, 180);
                        link.VisitedLinkColor = Color.Blue;
                        link.DefaultCellStyle.ForeColor = Color.Blue;
                        link.DefaultCellStyle.BackColor = Color.White;
                        link.DefaultCellStyle.SelectionForeColor = Color.Blue;
                    }
                }

                grid.Invalidate();
            }

            if (child.HasChildren)
                FixGridReadability(child);
        }
    }

    private void ByteNovaFormPaint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(1, 1, ClientSize.Width - 3, ClientSize.Height - 3);

        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(31, 19, 43), Color.FromArgb(1, 7, 15),
            LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, r);

        using (Pen outer = new Pen(Color.FromArgb(198, 244, 50), 1.6F))
            e.Graphics.DrawRectangle(outer, r);

        using (Pen inner = new Pen(Color.FromArgb(0, 80, 145), 1F))
            e.Graphics.DrawRectangle(inner,
                new Rectangle(4, 4, ClientSize.Width - 9, ClientSize.Height - 9));
    }

    private void ByteNovaBasePaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);

        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(5, 31, 51), Color.FromArgb(2, 14, 26),
            LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, r);

        using (Pen p = new Pen(Color.FromArgb(0, 135, 205), 1.2F))
            e.Graphics.DrawRectangle(p, r);
    }

    private void ByteNovaClosePaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(1, 1, c.Width - 3, c.Height - 3);

        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(190, 22, 34), Color.FromArgb(75, 6, 17),
            LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, r);

        using (Pen border = new Pen(Color.FromArgb(255, 75, 85), 1.2F))
            e.Graphics.DrawRectangle(border, r);

        using (Pen p = new Pen(Color.White, 2.2F))
        {
            p.StartCap = LineCap.Round;
            p.EndCap = LineCap.Round;
            e.Graphics.DrawLine(p, 10, 7, c.Width - 10, c.Height - 7);
            e.Graphics.DrawLine(p, c.Width - 10, 7, 10, c.Height - 7);
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddID));
        this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
        this.pnlHeader = new System.Windows.Forms.Panel();
        this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
        this.formName = new System.Windows.Forms.Label();
        this.PnlBase = new System.Windows.Forms.Panel();
        this.tabIrctcId = new APP_FRAMEWORK.tabIrctcId();
        this.tabHistory = new APP_FRAMEWORK.tabHistory();
        this.pnlHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
        this.PnlBase.SuspendLayout();
        this.SuspendLayout();
        // 
        // bunifuElipse1
        // 
        this.bunifuElipse1.ElipseRadius = 5;
        this.bunifuElipse1.TargetControl = this;
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
        this.pnlHeader.Size = new System.Drawing.Size(764, 25);
        this.pnlHeader.TabIndex = 130;
        this.pnlHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeader_Paint);
        // 
        // btnClose
        // 
        this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnClose.BackColor = System.Drawing.Color.Transparent;
        this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
        this.btnClose.ImageActive = null;
        this.btnClose.Location = new System.Drawing.Point(738, -1);
        this.btnClose.Margin = new System.Windows.Forms.Padding(2);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(22, 22);
        this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.btnClose.TabIndex = 25;
        this.btnClose.TabStop = false;
        this.btnClose.Zoom = 10;
        this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
        // 
        // formName
        // 
        this.formName.AutoSize = true;
        this.formName.BackColor = System.Drawing.Color.Transparent;
        this.formName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.formName.ForeColor = System.Drawing.Color.Black;
        this.formName.Location = new System.Drawing.Point(323, 3);
        this.formName.Name = "formName";
        this.formName.Size = new System.Drawing.Size(107, 16);
        this.formName.TabIndex = 23;
        this.formName.Text = "ADD IRCTC ID";
        // 
        // PnlBase
        // 
        this.PnlBase.AutoSize = true;
        this.PnlBase.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.PnlBase.Controls.Add(this.tabIrctcId);
        this.PnlBase.Controls.Add(this.tabHistory);
        this.PnlBase.Location = new System.Drawing.Point(2, 25);
        this.PnlBase.Name = "PnlBase";
        this.PnlBase.Size = new System.Drawing.Size(763, 367);
        this.PnlBase.TabIndex = 131;
        // 
        // tabIrctcId
        // 
        this.tabIrctcId.BackColor = System.Drawing.SystemColors.Control;
        this.tabIrctcId.Location = new System.Drawing.Point(2, 0);
        this.tabIrctcId.Name = "tabIrctcId";
        this.tabIrctcId.Size = new System.Drawing.Size(758, 364);
        this.tabIrctcId.TabIndex = 1;
        // 
        // tabHistory
        // 
        this.tabHistory.BackColor = System.Drawing.SystemColors.ActiveCaption;
        this.tabHistory.Location = new System.Drawing.Point(2, 0);
        this.tabHistory.Name = "tabHistory";
        this.tabHistory.Size = new System.Drawing.Size(758, 358);
        this.tabHistory.TabIndex = 0;
        // 
        // frmAddID
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.AutoSize = true;
        this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
        this.BackColor = System.Drawing.SystemColors.ActiveCaption;
        this.ClientSize = new System.Drawing.Size(764, 393);
        this.Controls.Add(this.PnlBase);
        this.Controls.Add(this.pnlHeader);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.Name = "frmAddID";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Add IRCTC ID";
        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmAddID_FormClosing);
        this.Load += new System.EventHandler(this.frmAddID_Load);
        this.pnlHeader.ResumeLayout(false);
        this.pnlHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
        this.PnlBase.ResumeLayout(false);
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    private void pnlHeader_Paint(object sender, PaintEventArgs e)
    {
        Rectangle r = pnlHeader.ClientRectangle;
        if (r.Width < 2 || r.Height < 2) return;

        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(2, 23, 52), Color.FromArgb(5, 53, 91),
            LinearGradientMode.Horizontal))
            e.Graphics.FillRectangle(bg, r);

        using (Pen cyan = new Pen(Color.FromArgb(198, 244, 50), 1.5F))
            e.Graphics.DrawLine(cyan, 0, r.Height - 1, r.Width, r.Height - 1);
    }
}
