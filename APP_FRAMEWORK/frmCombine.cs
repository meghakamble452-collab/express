using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Bunifu.Framework.UI;
using BunifuAnimatorNS;
using NEXUS.Properties;

namespace APP_FRAMEWORK;

public class frmCombine : Form
{
    private IContainer components;

    private BunifuTransition bunifuTransition_0;

    private BunifuTransition bunifuTransition_1;

    private BunifuElipse bunifuElipse1;

    private Panel pnlHeader;

    private BunifuImageButton bunifuImageButton1;

    private BunifuImageButton btnClose;

    private Label label5;

    private BunifuThinButton2 bunifuThinButton23;

    private BunifuThinButton2 bunifuThinButton22;

    private BunifuThinButton2 bunifuThinButton21;

    private Panel BAAAA;

    private tabNetBanking CAAA0;

    private tabDebitCard tabDebitCard;

    private tabCreditCard tabCreditCard;

    public frmCombine()
    {
        InitializeComponent();
        ApplyByteNovaLatestDesign();
        pnlHeader.MouseDown += StatClass.Header_MouseDown;
        pnlHeader.MouseMove += StatClass.Header_MouseMove;
        pnlHeader.MouseUp += StatClass.Header_MouseUp;
    }

    private void method_0()
    {
        CAAA0.Visible = false;
        tabDebitCard.Visible = false;
        tabCreditCard.Visible = false;
        BAAAA.Visible = false;
    }

    private void BunifuThinButton21_Click(object sender, EventArgs e)
    {
        method_0();
        CAAA0.Visible = true;
        CAAA0.BringToFront();
        bunifuTransition_1.ShowSync(BAAAA);
    }

    private void BunifuThinButton22_Click(object sender, EventArgs e)
    {
        method_0();
        tabDebitCard.Visible = true;
        tabDebitCard.BringToFront();
        bunifuTransition_1.ShowSync(BAAAA);
    }

    private void BunifuThinButton23_Click(object sender, EventArgs e)
    {
        method_0();
        tabCreditCard.Visible = true;
        tabCreditCard.BringToFront();
        bunifuTransition_1.ShowSync(BAAAA);
    }

    private void FrmCombine_FormClosing(object sender, FormClosingEventArgs e)
    {
        StatClass.method_35();
    }

    private void BtnClose_Click(object sender, EventArgs e)
    {
        Close();
    }


    private void ApplyByteNovaLatestDesign()
    {
        SuspendLayout();
        try
        {
            BackColor = Color.FromArgb(233, 241, 252);
            ClientSize = new Size(760, 470);

            pnlHeader.Dock = DockStyle.None;
            pnlHeader.SetBounds(0, 0, ClientSize.Width, 42);
            pnlHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlHeader.BackColor = Color.FromArgb(8, 26, 52);
            pnlHeader.Paint -= ByteNovaHeaderPaint;
            pnlHeader.Paint += ByteNovaHeaderPaint;

            label5.AutoSize = false;
            label5.SetBounds(16, 9, 480, 24);
            label5.Text = "PAYMENT DETAILS   \u2022   BUILD " + Program.appversion;
            label5.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            label5.ForeColor = Color.White;
            label5.TextAlign = ContentAlignment.MiddleLeft;
            label5.BackColor = Color.Transparent;

            bunifuImageButton1.Visible = false;

            btnClose.SetBounds(ClientSize.Width - 40, 8, 28, 26);
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.BackColor = Color.Transparent;
            btnClose.BackgroundImage = null;
            btnClose.BackgroundImageLayout = ImageLayout.Zoom;
            btnClose.SizeMode = PictureBoxSizeMode.Zoom;
            btnClose.Zoom = 0;

            StylePaymentButton(bunifuThinButton21, "NET BANKING", Color.FromArgb(0, 128, 154));
            StylePaymentButton(bunifuThinButton22, "DEBIT CARD", Color.FromArgb(50, 82, 180));
            StylePaymentButton(bunifuThinButton23, "CREDIT CARD", Color.FromArgb(104, 60, 180));

            int gap = 8;
            int left = 12;
            int buttonWidth = (ClientSize.Width - (left * 2) - (gap * 2)) / 3;
            bunifuThinButton21.SetBounds(left, 54, buttonWidth, 40);
            bunifuThinButton22.SetBounds(left + buttonWidth + gap, 54, buttonWidth, 40);
            bunifuThinButton23.SetBounds(left + ((buttonWidth + gap) * 2), 54, buttonWidth, 40);

            bunifuThinButton21.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            bunifuThinButton22.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            bunifuThinButton23.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            BAAAA.SetBounds(12, 104, ClientSize.Width - 24, ClientSize.Height - 116);
            BAAAA.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            BAAAA.BackColor = Color.White;
            BAAAA.Paint -= ByteNovaBodyPanelPaint;
            BAAAA.Paint += ByteNovaBodyPanelPaint;

            CAAA0.Dock = DockStyle.Fill;
            tabDebitCard.Dock = DockStyle.Fill;
            tabCreditCard.Dock = DockStyle.Fill;

            CAAA0.BackColor = Color.White;
            tabDebitCard.BackColor = Color.White;
            tabCreditCard.BackColor = Color.White;

            StyleDeep(CAAA0);
            StyleDeep(tabDebitCard);
            StyleDeep(tabCreditCard);

            bunifuElipse1.ElipseRadius = 6;

            method_0();
            CAAA0.Visible = true;
            CAAA0.BringToFront();
            BAAAA.Visible = true;
        }
        finally
        {
            ResumeLayout(false);
            PerformLayout();
            Invalidate(true);
        }
    }

    private void StyleDeep(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            TextBox tb = c as TextBox;
            if (tb != null)
            {
                tb.BorderStyle = BorderStyle.FixedSingle;
                tb.BackColor = Color.White;
                tb.ForeColor = Color.FromArgb(17, 40, 72);
                tb.Font = new Font("Segoe UI", 9.5f);
            }

            ComboBox cb = c as ComboBox;
            if (cb != null)
            {
                cb.FlatStyle = FlatStyle.Flat;
                cb.BackColor = Color.White;
                cb.ForeColor = Color.FromArgb(17, 40, 72);
                cb.Font = new Font("Segoe UI", 9.5f);
            }

            ListBox lb = c as ListBox;
            if (lb != null)
            {
                lb.BorderStyle = BorderStyle.FixedSingle;
                lb.BackColor = Color.White;
                lb.ForeColor = Color.FromArgb(17, 40, 72);
                lb.Font = new Font("Segoe UI", 9.5f);
            }

            Label l = c as Label;
            if (l != null)
            {
                l.ForeColor = Color.FromArgb(17, 40, 72);
                if (l.BackColor != Color.Transparent) l.BackColor = Color.White;
            }

            CheckBox chk = c as CheckBox;
            if (chk != null)
            {
                chk.ForeColor = Color.FromArgb(17, 40, 72);
                chk.BackColor = Color.White;
            }

            RadioButton rb = c as RadioButton;
            if (rb != null)
            {
                rb.ForeColor = Color.FromArgb(17, 40, 72);
                rb.BackColor = Color.White;
            }

            Button b = c as Button;
            if (b != null) StyleFlatButton(b);

            DataGridView d = c as DataGridView;
            if (d != null) StyleGrid(d);

            Panel p = c as Panel;
            if (p != null) p.BackColor = Color.White;

            GroupBox g = c as GroupBox;
            if (g != null)
            {
                g.ForeColor = Color.FromArgb(17, 40, 72);
                g.BackColor = Color.White;
            }

            StyleDeep(c);
        }
    }

    private void StyleFlatButton(Button b)
    {
        string t = (b.Text ?? "").ToUpperInvariant();

        Color bg = Color.FromArgb(245, 146, 28);
        if (t.Contains("DEL") || t.Contains("CLEAR") || t.Contains("REMOVE"))
            bg = Color.FromArgb(220, 38, 38);
        else if (t.Contains("EDIT") || t.Contains("UPDATE"))
            bg = Color.FromArgb(59, 132, 247);

        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.BackColor = bg;
        b.ForeColor = Color.White;
        b.Font = new Font("Segoe UI Semibold", 9f);
        b.UseVisualStyleBackColor = false;
        b.Cursor = Cursors.Hand;
    }

    private void StyleGrid(DataGridView d)
    {
        d.EnableHeadersVisualStyles = false;
        d.BackgroundColor = Color.White;
        d.BorderStyle = BorderStyle.None;
        d.GridColor = Color.FromArgb(198, 216, 242);
        d.RowHeadersVisible = false;
        d.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(12, 38, 72);
        d.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        d.DefaultCellStyle.BackColor = Color.White;
        d.DefaultCellStyle.ForeColor = Color.FromArgb(17, 40, 72);
        d.DefaultCellStyle.SelectionBackColor = Color.FromArgb(226, 238, 252);
        d.DefaultCellStyle.SelectionForeColor = Color.FromArgb(17, 40, 72);
    }

    private void StylePaymentButton(BunifuThinButton2 b, string text, Color accent)
    {
        if (b == null) return;

        b.ButtonText = text;
        b.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        b.IdleCornerRadius = 5;
        b.ActiveCornerRadius = 5;
        b.IdleBorderThickness = 1;
        b.ActiveBorderThickness = 1;

        b.IdleFillColor = Color.White;
        b.IdleForecolor = Color.FromArgb(17, 40, 72);
        b.IdleLineColor = accent;

        b.ActiveFillColor = accent;
        b.ActiveForecolor = Color.White;
        b.ActiveLineColor = Color.FromArgb(105, 225, 255);

        b.BackColor = Color.FromArgb(233, 241, 252);
        b.ForeColor = Color.FromArgb(17, 40, 72);
        b.Cursor = Cursors.Hand;
    }

    private void ByteNovaHeaderPaint(object sender, PaintEventArgs e)
    {
        Rectangle r = pnlHeader.ClientRectangle;
        if (r.Width <= 0 || r.Height <= 0) return;

        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(8, 26, 52), Color.FromArgb(16, 58, 104),
            LinearGradientMode.Horizontal))
            e.Graphics.FillRectangle(bg, r);

        using (SolidBrush lime = new SolidBrush(Color.FromArgb(198, 244, 50)))
            e.Graphics.FillRectangle(lime, 0, r.Height - 2, r.Width, 2);
    }

    private void ByteNovaBodyPanelPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null || c.ClientSize.Width < 2 || c.ClientSize.Height < 2) return;

        Rectangle r = new Rectangle(0, 0, c.ClientSize.Width - 1, c.ClientSize.Height - 1);
        using (SolidBrush bg = new SolidBrush(Color.White))
            e.Graphics.FillRectangle(bg, r);
        using (Pen p = new Pen(Color.FromArgb(198, 216, 242), 1F))
            e.Graphics.DrawRectangle(p, r);
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
            BunifuAnimatorNS.Animation animation1 = new BunifuAnimatorNS.Animation();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCombine));
            BunifuAnimatorNS.Animation animation2 = new BunifuAnimatorNS.Animation();
            this.bunifuTransition_0 = new BunifuAnimatorNS.BunifuTransition(this.components);
            this.BAAAA = new System.Windows.Forms.Panel();
            this.CAAA0 = new APP_FRAMEWORK.tabNetBanking();
            this.tabDebitCard = new APP_FRAMEWORK.tabDebitCard();
            this.tabCreditCard = new APP_FRAMEWORK.tabCreditCard();
            this.bunifuThinButton23 = new Bunifu.Framework.UI.BunifuThinButton2();
            this.bunifuThinButton22 = new Bunifu.Framework.UI.BunifuThinButton2();
            this.bunifuThinButton21 = new Bunifu.Framework.UI.BunifuThinButton2();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.bunifuImageButton1 = new Bunifu.Framework.UI.BunifuImageButton();
            this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.label5 = new System.Windows.Forms.Label();
            this.bunifuTransition_1 = new BunifuAnimatorNS.BunifuTransition(this.components);
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.BAAAA.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.SuspendLayout();
            // 
            // bunifuTransition_0
            // 
            this.bunifuTransition_0.AnimationType = BunifuAnimatorNS.AnimationType.Leaf;
            this.bunifuTransition_0.Cursor = null;
            animation1.AnimateOnlyDifferences = true;
            animation1.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.BlindCoeff")));
            animation1.LeafCoeff = 1F;
            animation1.MaxTime = 1F;
            animation1.MinTime = 0F;
            animation1.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicCoeff")));
            animation1.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation1.MosaicShift")));
            animation1.MosaicSize = 0;
            animation1.Padding = new System.Windows.Forms.Padding(0);
            animation1.RotateCoeff = 0F;
            animation1.RotateLimit = 0F;
            animation1.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.ScaleCoeff")));
            animation1.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation1.SlideCoeff")));
            animation1.TimeCoeff = 0F;
            animation1.TransparencyCoeff = 0F;
            this.bunifuTransition_0.DefaultAnimation = animation1;
            // 
            // BAAAA
            // 
            this.BAAAA.BackColor = System.Drawing.Color.Transparent;
            this.BAAAA.Controls.Add(this.CAAA0);
            this.BAAAA.Controls.Add(this.tabDebitCard);
            this.BAAAA.Controls.Add(this.tabCreditCard);
            this.bunifuTransition_0.SetDecoration(this.BAAAA, BunifuAnimatorNS.DecorationType.None);
            this.BAAAA.Location = new System.Drawing.Point(9, 88);
            this.BAAAA.Name = "BAAAA";
            this.BAAAA.Size = new System.Drawing.Size(752, 324);
            this.BAAAA.TabIndex = 1217;
            // 
            // CAAA0
            // 
            this.CAAA0.BackColor = System.Drawing.Color.Transparent;
            this.bunifuTransition_0.SetDecoration(this.CAAA0, BunifuAnimatorNS.DecorationType.None);
            this.CAAA0.Location = new System.Drawing.Point(0, 0);
            this.CAAA0.Margin = new System.Windows.Forms.Padding(0);
            this.CAAA0.Name = "CAAA0";
            this.CAAA0.Size = new System.Drawing.Size(742, 310);
            this.CAAA0.TabIndex = 1;
            // 
            // tabDebitCard
            // 
            this.tabDebitCard.BackColor = System.Drawing.Color.Transparent;
            this.bunifuTransition_0.SetDecoration(this.tabDebitCard, BunifuAnimatorNS.DecorationType.None);
            this.tabDebitCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDebitCard.Location = new System.Drawing.Point(0, 0);
            this.tabDebitCard.Margin = new System.Windows.Forms.Padding(0);
            this.tabDebitCard.Name = "tabDebitCard";
            this.tabDebitCard.Size = new System.Drawing.Size(752, 324);
            this.tabDebitCard.TabIndex = 2;
            // 
            // tabCreditCard
            // 
            this.tabCreditCard.BackColor = System.Drawing.Color.Transparent;
            this.bunifuTransition_0.SetDecoration(this.tabCreditCard, BunifuAnimatorNS.DecorationType.None);
            this.tabCreditCard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabCreditCard.Location = new System.Drawing.Point(0, 0);
            this.tabCreditCard.Margin = new System.Windows.Forms.Padding(0);
            this.tabCreditCard.Name = "tabCreditCard";
            this.tabCreditCard.Size = new System.Drawing.Size(752, 324);
            this.tabCreditCard.TabIndex = 3;
            // 
            // bunifuThinButton23
            // 
            this.bunifuThinButton23.ActiveBorderThickness = 1;
            this.bunifuThinButton23.ActiveCornerRadius = 20;
            this.bunifuThinButton23.ActiveFillColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton23.ActiveForecolor = System.Drawing.Color.White;
            this.bunifuThinButton23.ActiveLineColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton23.BackColor = System.Drawing.Color.GhostWhite;
            this.bunifuThinButton23.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuThinButton23.BackgroundImage")));
            this.bunifuThinButton23.ButtonText = "Credit Card";
            this.bunifuThinButton23.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.bunifuThinButton23, BunifuAnimatorNS.DecorationType.None);
            this.bunifuThinButton23.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuThinButton23.ForeColor = System.Drawing.Color.DimGray;
            this.bunifuThinButton23.IdleBorderThickness = 2;
            this.bunifuThinButton23.IdleCornerRadius = 10;
            this.bunifuThinButton23.IdleFillColor = System.Drawing.Color.White;
            this.bunifuThinButton23.IdleForecolor = System.Drawing.Color.DimGray;
            this.bunifuThinButton23.IdleLineColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton23.Location = new System.Drawing.Point(508, 39);
            this.bunifuThinButton23.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuThinButton23.Name = "bunifuThinButton23";
            this.bunifuThinButton23.Size = new System.Drawing.Size(231, 41);
            this.bunifuThinButton23.TabIndex = 1216;
            this.bunifuThinButton23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuThinButton23.Click += new System.EventHandler(this.BunifuThinButton23_Click);
            // 
            // bunifuThinButton22
            // 
            this.bunifuThinButton22.ActiveBorderThickness = 1;
            this.bunifuThinButton22.ActiveCornerRadius = 20;
            this.bunifuThinButton22.ActiveFillColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton22.ActiveForecolor = System.Drawing.Color.White;
            this.bunifuThinButton22.ActiveLineColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton22.BackColor = System.Drawing.Color.GhostWhite;
            this.bunifuThinButton22.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuThinButton22.BackgroundImage")));
            this.bunifuThinButton22.ButtonText = "Debit Card";
            this.bunifuThinButton22.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.bunifuThinButton22, BunifuAnimatorNS.DecorationType.None);
            this.bunifuThinButton22.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuThinButton22.ForeColor = System.Drawing.Color.DimGray;
            this.bunifuThinButton22.IdleBorderThickness = 2;
            this.bunifuThinButton22.IdleCornerRadius = 10;
            this.bunifuThinButton22.IdleFillColor = System.Drawing.Color.White;
            this.bunifuThinButton22.IdleForecolor = System.Drawing.Color.DimGray;
            this.bunifuThinButton22.IdleLineColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton22.Location = new System.Drawing.Point(263, 39);
            this.bunifuThinButton22.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuThinButton22.Name = "bunifuThinButton22";
            this.bunifuThinButton22.Size = new System.Drawing.Size(231, 41);
            this.bunifuThinButton22.TabIndex = 1215;
            this.bunifuThinButton22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuThinButton22.Click += new System.EventHandler(this.BunifuThinButton22_Click);
            // 
            // bunifuThinButton21
            // 
            this.bunifuThinButton21.ActiveBorderThickness = 1;
            this.bunifuThinButton21.ActiveCornerRadius = 20;
            this.bunifuThinButton21.ActiveFillColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton21.ActiveForecolor = System.Drawing.Color.White;
            this.bunifuThinButton21.ActiveLineColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton21.BackColor = System.Drawing.Color.GhostWhite;
            this.bunifuThinButton21.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bunifuThinButton21.BackgroundImage")));
            this.bunifuThinButton21.ButtonText = "Bank";
            this.bunifuThinButton21.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.bunifuThinButton21, BunifuAnimatorNS.DecorationType.None);
            this.bunifuThinButton21.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bunifuThinButton21.ForeColor = System.Drawing.Color.DimGray;
            this.bunifuThinButton21.IdleBorderThickness = 2;
            this.bunifuThinButton21.IdleCornerRadius = 10;
            this.bunifuThinButton21.IdleFillColor = System.Drawing.Color.White;
            this.bunifuThinButton21.IdleForecolor = System.Drawing.Color.DimGray;
            this.bunifuThinButton21.IdleLineColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuThinButton21.Location = new System.Drawing.Point(14, 39);
            this.bunifuThinButton21.Margin = new System.Windows.Forms.Padding(5);
            this.bunifuThinButton21.Name = "bunifuThinButton21";
            this.bunifuThinButton21.Size = new System.Drawing.Size(231, 41);
            this.bunifuThinButton21.TabIndex = 1214;
            this.bunifuThinButton21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.bunifuThinButton21.Click += new System.EventHandler(this.BunifuThinButton21_Click);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(22, 14, 28);
            this.pnlHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlHeader.Controls.Add(this.bunifuImageButton1);
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Controls.Add(this.label5);
            this.bunifuTransition_0.SetDecoration(this.pnlHeader, BunifuAnimatorNS.DecorationType.None);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(18)))));
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(753, 31);
            this.pnlHeader.TabIndex = 1210;
            // 
            // bunifuImageButton1
            // 
            this.bunifuImageButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bunifuImageButton1.BackColor = System.Drawing.Color.FromArgb(155, 93, 229);
            this.bunifuImageButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.bunifuImageButton1, BunifuAnimatorNS.DecorationType.None);
            this.bunifuImageButton1.Image = ((System.Drawing.Image)(resources.GetObject("bunifuImageButton1.Image")));
            this.bunifuImageButton1.ImageActive = null;
            this.bunifuImageButton1.Location = new System.Drawing.Point(-35, 44);
            this.bunifuImageButton1.Name = "bunifuImageButton1";
            this.bunifuImageButton1.Size = new System.Drawing.Size(26, 20);
            this.bunifuImageButton1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.bunifuImageButton1.TabIndex = 208;
            this.bunifuImageButton1.TabStop = false;
            this.bunifuImageButton1.Zoom = 10;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuTransition_0.SetDecoration(this.btnClose, BunifuAnimatorNS.DecorationType.None);
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageActive = null;
            this.btnClose.Location = new System.Drawing.Point(719, 2);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(31, 26);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.TabIndex = 1;
            this.btnClose.TabStop = false;
            this.btnClose.Zoom = 10;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.bunifuTransition_0.SetDecoration(this.label5, BunifuAnimatorNS.DecorationType.None);
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(3, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 20);
            this.label5.TabIndex = 0;
            this.label5.Text = "Bank";
            // 
            // bunifuTransition_1
            // 
            this.bunifuTransition_1.AnimationType = BunifuAnimatorNS.AnimationType.VertSlide;
            this.bunifuTransition_1.Cursor = null;
            animation2.AnimateOnlyDifferences = true;
            animation2.BlindCoeff = ((System.Drawing.PointF)(resources.GetObject("animation2.BlindCoeff")));
            animation2.LeafCoeff = 0F;
            animation2.MaxTime = 1F;
            animation2.MinTime = 0F;
            animation2.MosaicCoeff = ((System.Drawing.PointF)(resources.GetObject("animation2.MosaicCoeff")));
            animation2.MosaicShift = ((System.Drawing.PointF)(resources.GetObject("animation2.MosaicShift")));
            animation2.MosaicSize = 0;
            animation2.Padding = new System.Windows.Forms.Padding(0);
            animation2.RotateCoeff = 0F;
            animation2.RotateLimit = 0F;
            animation2.ScaleCoeff = ((System.Drawing.PointF)(resources.GetObject("animation2.ScaleCoeff")));
            animation2.SlideCoeff = ((System.Drawing.PointF)(resources.GetObject("animation2.SlideCoeff")));
            animation2.TimeCoeff = 0F;
            animation2.TransparencyCoeff = 0F;
            this.bunifuTransition_1.DefaultAnimation = animation2;
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 5;
            this.bunifuElipse1.TargetControl = this;
            // 
            // frmCombine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(753, 406);
            this.Controls.Add(this.BAAAA);
            this.Controls.Add(this.bunifuThinButton23);
            this.Controls.Add(this.bunifuThinButton22);
            this.Controls.Add(this.bunifuThinButton21);
            this.Controls.Add(this.pnlHeader);
            this.bunifuTransition_1.SetDecoration(this, BunifuAnimatorNS.DecorationType.None);
            this.bunifuTransition_0.SetDecoration(this, BunifuAnimatorNS.DecorationType.None);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCombine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmCombine";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmCombine_FormClosing);
            this.BAAAA.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.ResumeLayout(false);

    }
}
