using Bunifu.Framework.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEXUS.IRCommDLL;

public class frmShowQR : Form
{
    private clsPostData class42_0;
    internal clsMain clsMain;

    private const uint uint_0 = 1u;
    private const uint uint_1 = 2u;
    internal const uint uint_2 = 3u;

    internal static readonly IntPtr intptr_0 = new IntPtr(-1);

    private Dictionary<string, string> dictionary_0 =
        new Dictionary<string, string>();

    internal string string_0 = "";
    internal string string_1 = "";
    internal string string_2 = "";

    private string string_3;

    internal bool bool_0;
    internal bool bool_2;
    internal bool bool_3;
    internal bool bool_4;
    internal bool bool_7;
    internal bool bool_Passenger;
    internal bool bool_Alert;

    private string tktname_0;

    private IContainer components;
    private BunifuElipse bunifuElipse1;
    private PictureBox pictureBox1;
    private Label label1;

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(
        IntPtr intptr_1,
        IntPtr intptr_2,
        int int_0,
        int int_1,
        int int_2,
        int int_3,
        uint uint_3);

    public frmShowQR()
    {
        InitializeComponent();
        ApplyByteNovaTheme();
    }

    // =========================================================
    // EXPRESS THEME
    // =========================================================
    private void ApplyByteNovaTheme()
    {
        SuspendLayout();

        BackColor = Color.FromArgb(3, 13, 29);
        ForeColor = Color.White;

        FormBorderStyle = FormBorderStyle.None;

        // OLD WIDTH = 245
        // NEW WIDTH = 265 (+20px)
        ClientSize = new Size(310, 300);

        bunifuElipse1.ElipseRadius = 10;

        // =====================================================
        // QR PICTURE BOX
        // =====================================================
        pictureBox1.Dock = DockStyle.None;

        // Left/Right 12px margin
        pictureBox1.SetBounds(
            12,
            40,
            ClientSize.Width - 24,
            221);

        pictureBox1.BackColor = Color.White;
        pictureBox1.BorderStyle = BorderStyle.None;
        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

        // =====================================================
        // PAYMENT INFORMATION LABEL
        // =====================================================
        label1.AutoSize = false;

        label1.SetBounds(
            12,
            268,
            ClientSize.Width - 24,
            23);

        label1.BackColor = Color.Transparent;
        label1.ForeColor = Color.FromArgb(255, 215, 80);

        label1.Font = new Font(
            "Segoe UI Semibold",
            8.5F,
            FontStyle.Bold);

        label1.TextAlign = ContentAlignment.MiddleCenter;

        // Paint custom Byte Nova UI
        Paint += ByteNovaPaint;

        ResumeLayout(false);
        Invalidate();
    }

    // =========================================================
    // CUSTOM EXPRESS PAINT
    // =========================================================
    private void ByteNovaPaint(object sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        // =====================================================
        // HEADER
        // =====================================================
        Rectangle header = new Rectangle(
            4,
            4,
            ClientSize.Width - 8,
            29);

        using (LinearGradientBrush hb = new LinearGradientBrush(
            header,
            Color.FromArgb(3, 38, 70),
            Color.FromArgb(53, 34, 112),
            LinearGradientMode.Horizontal))
        {
            e.Graphics.FillRectangle(hb, header);
        }

        // =====================================================
        // OUTER CYAN BORDER
        // =====================================================
        using (Pen cyan = new Pen(
            Color.FromArgb(0, 210, 255),
            1.4F))
        {
            e.Graphics.DrawRectangle(
                cyan,
                2,
                2,
                ClientSize.Width - 5,
                ClientSize.Height - 5);
        }

        // =====================================================
        // INNER BORDER
        // =====================================================
        using (Pen inner = new Pen(
            Color.FromArgb(65, 90, 140, 255),
            1F))
        {
            e.Graphics.DrawRectangle(
                inner,
                5,
                5,
                ClientSize.Width - 11,
                ClientSize.Height - 11);
        }

        // =====================================================
        // HEADER TITLE
        // =====================================================
        using (Font titleFont = new Font(
            "Segoe UI Semibold",
            9F,
            FontStyle.Bold))
        using (Brush white = new SolidBrush(Color.White))
        {
            e.Graphics.DrawString(
                "PAYMENT QR  _  EXPRESS",
                titleFont,
                white,
                12,
                10);
        }

        // =====================================================
        // QR CARD
        // Dynamic width according to form
        // =====================================================
        Rectangle qrCard = new Rectangle(
            9,
            37,
            ClientSize.Width - 18,
            227);

        using (GraphicsPath path = RoundedRect(qrCard, 9))
        using (SolidBrush fill =
               new SolidBrush(Color.FromArgb(7, 35, 61)))
        using (Pen border =
               new Pen(Color.FromArgb(0, 210, 255), 1.2F))
        {
            e.Graphics.FillPath(fill, path);
            e.Graphics.DrawPath(border, path);
        }

        // =====================================================
        // PAYMENT INFO CARD
        // Dynamic width according to form
        // =====================================================
        Rectangle infoCard = new Rectangle(
            9,
            266,
            ClientSize.Width - 18,
            27);

        using (GraphicsPath path = RoundedRect(infoCard, 8))
        using (SolidBrush fill =
               new SolidBrush(Color.FromArgb(7, 35, 61)))
        using (Pen border =
               new Pen(Color.FromArgb(255, 202, 62), 1F))
        {
            e.Graphics.FillPath(fill, path);
            e.Graphics.DrawPath(border, path);
        }
    }

    // =========================================================
    // ROUNDED RECTANGLE
    // =========================================================
    private GraphicsPath RoundedRect(Rectangle r, int radius)
    {
        GraphicsPath p = new GraphicsPath();

        int d = radius * 2;

        p.AddArc(
            r.Left,
            r.Top,
            d,
            d,
            180,
            90);

        p.AddArc(
            r.Right - d,
            r.Top,
            d,
            d,
            270,
            90);

        p.AddArc(
            r.Right - d,
            r.Bottom - d,
            d,
            d,
            0,
            90);

        p.AddArc(
            r.Left,
            r.Bottom - d,
            d,
            d,
            90,
            90);

        p.CloseFigure();

        return p;
    }

    // =========================================================
    // LOAD BASE64 QR IMAGE
    // =========================================================
    public Image LoadImage(string strin_1)
    {
        using MemoryStream stream =
            new MemoryStream(Convert.FromBase64String(strin_1));

        return Image.FromStream(stream);
    }

    // =========================================================
    // MAKE SURE FORM IS VISIBLE ON SCREEN
    // =========================================================
    public static void EnsureShowsTaskbar(Form form)
    {
        if (form != null && !form.IsDisposed)
        {
            Screen screen = Screen.FromControl(form);

            Rectangle bounds = screen.Bounds;
            Rectangle workingArea = screen.WorkingArea;
            Rectangle bounds2 = form.Bounds;

            int val = bounds2.Left;
            int val2 = bounds2.Top;

            if (bounds2.Left < workingArea.Left)
            {
                val = workingArea.Left;
            }

            if (bounds2.Right > workingArea.Right)
            {
                val = Math.Max(
                    workingArea.Left,
                    bounds2.Left -
                    (bounds2.Right - workingArea.Right));
            }

            if (bounds2.Top < workingArea.Top)
            {
                val2 = workingArea.Top;
            }

            if (bounds2.Bottom > workingArea.Bottom)
            {
                val2 = Math.Max(
                    workingArea.Top,
                    bounds2.Top -
                    (bounds2.Bottom - workingArea.Bottom));
            }

            val = Math.Min(
                Math.Max(val, bounds.Left),
                bounds.Right - bounds2.Width);

            val2 = Math.Min(
                Math.Max(val2, bounds.Top),
                bounds.Bottom - bounds2.Height);

            if (val != bounds2.Left ||
                val2 != bounds2.Top)
            {
                form.Location = new Point(val, val2);
            }
        }
    }

    // =========================================================
    // SHOW QR FORM
    // =========================================================
    internal void method_0(
        Control control_0,
        Rectangle rectangle_0)
    {
        try
        {
            _ = Screen.PrimaryScreen.WorkingArea;
            _ = control_0.Controls[0];

            Control control = control_0.Parent;

            StartPosition = FormStartPosition.Manual;

            Location = control.Location;

            EnsureShowsTaskbar(this);

            _ = control.Location.Y;

            SetWindowPos(
                Handle,
                intptr_0,
                0,
                0,
                0,
                0,
                3u);

            _ = Top;
            _ = Height;

            TopMost = true;

            Show();

            pictureBox1.BringToFront();

            Activate();
        }
        catch (Exception)
        {
        }
    }

    // =========================================================
    // RECEIVE QR DATA
    // =========================================================
    internal void method_1(object string_5)
    {
        string_3 = string_5.ToString();

        string text = string_3;

        string[] array = new string[1];

        string text2 = "|||";

        array[0] = text2;

        string[] array2 =
            text.Split(array, StringSplitOptions.None);

        pictureBox1.Image = LoadImage(array2[0]);

        Label label = label1;

        string text3 = "Pay Using QR :- ";
        string text4 = array2[1];
        string text5 = "  Amt=  ";

        label.Text =
            text3 +
            text4 +
            text5 +
            array2[2];

        method_0(
            clsMain.control_0,
            clsMain.rectangle_0);
    }

    // =========================================================
    // DISPOSE
    // =========================================================
    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    // =========================================================
    // INITIALIZE COMPONENT
    // =========================================================
    private void InitializeComponent()
    {
        this.components =
            new System.ComponentModel.Container();

        this.bunifuElipse1 =
            new Bunifu.Framework.UI.BunifuElipse(
                this.components);

        this.pictureBox1 =
            new System.Windows.Forms.PictureBox();

        this.label1 =
            new System.Windows.Forms.Label();

        ((System.ComponentModel.ISupportInitialize)
            (this.pictureBox1)).BeginInit();

        this.SuspendLayout();

        // 
        // bunifuElipse1
        // 
        this.bunifuElipse1.ElipseRadius = 5;
        this.bunifuElipse1.TargetControl = this;

        // 
        // pictureBox1
        // 
        this.pictureBox1.BackColor =
            System.Drawing.SystemColors.ButtonHighlight;

        this.pictureBox1.BorderStyle =
            System.Windows.Forms.BorderStyle.FixedSingle;

        this.pictureBox1.Dock =
            System.Windows.Forms.DockStyle.Top;

        this.pictureBox1.Location =
            new System.Drawing.Point(0, 0);

        this.pictureBox1.Name = "pictureBox1";

        this.pictureBox1.Size =
            new System.Drawing.Size(221, 221);

        this.pictureBox1.SizeMode =
            System.Windows.Forms.PictureBoxSizeMode.Zoom;

        this.pictureBox1.TabIndex = 1217;
        this.pictureBox1.TabStop = false;

        // 
        // label1
        // 
        this.label1.AutoSize = true;

        this.label1.Location =
            new System.Drawing.Point(12, 235);

        this.label1.Name = "label1";

        this.label1.Size =
            new System.Drawing.Size(35, 13);

        this.label1.TabIndex = 1218;

        this.label1.Text = "label1";

        // 
        // frmShowQR
        // 
        this.AutoScaleDimensions =
            new System.Drawing.SizeF(6F, 13F);

        this.AutoScaleMode =
            System.Windows.Forms.AutoScaleMode.Font;

        // Designer size.
        // ApplyByteNovaTheme() will finally make it 265 x 300.
        this.ClientSize =
            new System.Drawing.Size(265, 300);

        this.Controls.Add(this.label1);
        this.Controls.Add(this.pictureBox1);

        this.FormBorderStyle =
            System.Windows.Forms.FormBorderStyle.None;

        this.Name = "frmShowQR";

        this.Text = "Payment QR";

        this.TopMost = true;

        ((System.ComponentModel.ISupportInitialize)
            (this.pictureBox1)).EndInit();

        this.ResumeLayout(false);
        this.PerformLayout();
    }
}