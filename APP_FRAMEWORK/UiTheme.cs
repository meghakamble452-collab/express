using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace APP_FRAMEWORK;

internal static class UiTheme
{
    private const float S = 1.28f;

    public static readonly Color Bg = Color.FromArgb(244, 248, 253);
    public static readonly Color Card = Color.White;
    public static readonly Color Border = Color.FromArgb(214, 226, 242);
    public static readonly Color HeaderA = Color.FromArgb(18, 76, 196);
    public static readonly Color HeaderB = Color.FromArgb(48, 140, 244);
    public static readonly Color Green = Color.FromArgb(41, 186, 76);
    public static readonly Color Red = Color.FromArgb(226, 72, 60);
    public static readonly Color Text = Color.FromArgb(22, 36, 54);

    public static readonly Font FBase = new Font("Segoe UI", 11F);
    public static readonly Font FBold = new Font("Segoe UI Semibold", 10.5F, FontStyle.Bold);
    public static readonly Font FTitle = new Font("Segoe UI Semibold", 15F, FontStyle.Bold);

    public static void Apply(Form f)
    {
        f.SuspendLayout();
        HideOldHeader(f);
        f.FormBorderStyle = FormBorderStyle.None;
        f.BackColor = Bg;
        f.ForeColor = Text;
        f.Font = FBase;
        f.ClientSize = new Size((int)(f.ClientSize.Width * S), (int)(f.ClientSize.Height * S));
        f.Padding = new Padding(14);
        foreach (Control c in f.Controls) Walk(c);
        f.ResumeLayout(true);
    }

    private static void HideOldHeader(Form f)
    {
        foreach (Control c in f.Controls)
            if (c is Panel p && p.Name != "uiHeader" && p.Dock == DockStyle.Top && p.Height <= 48)
                p.Visible = false;
    }

    private static void Scale(Control c)
    {
        if (c.Dock == DockStyle.None || c.Dock == DockStyle.Left || c.Dock == DockStyle.Right)
        {
            c.Left = (int)(c.Left * S);
            c.Width = (int)(c.Width * S);
        }
        c.Top = (int)(c.Top * S);
        c.Height = (int)(c.Height * S);
        try { c.Font = new Font(c.Font.FontFamily, Math.Max(9.5f, c.Font.Size * S)); } catch { }
    }

    private static void Walk(Control c)
    {
        Scale(c);

        if (c is DataGridView d) { StyleGrid(d); return; }

        if (c is GroupBox g)
        {
            g.BackColor = Card;
            g.ForeColor = HeaderA;
            g.Font = FBold;
            DrawBorder(g, Border);
            Round(g, 12);
        }
        else if (c is Panel p)
        {
            p.BackColor = (p.Parent is Form) ? Bg : Card;
            Round(p, 12);
        }
        else if (c is Button b) StyleButton(b);
        else if (c is TextBox t)
        {
            t.BorderStyle = BorderStyle.FixedSingle;
            t.BackColor = Color.White;
            t.ForeColor = Text;
            t.Font = FBase;
        }
        else if (c is ComboBox cb)
        {
            cb.FlatStyle = FlatStyle.Flat;
            cb.BackColor = Color.White;
            cb.ForeColor = Text;
            cb.Font = FBase;
        }
        else if (c is RadioButton rb) { rb.ForeColor = Text; rb.BackColor = Color.Transparent; }
        else if (c is CheckBox ch) { ch.ForeColor = Text; ch.BackColor = Color.Transparent; }
        else if (c is Label l) { l.ForeColor = Text; l.BackColor = Color.Transparent; }

        foreach (Control k in c.Controls) Walk(k);
    }

    private static void StyleButton(Button b)
    {
        string n = (b.Name + " " + b.Text).ToLower();
        bool danger = n.Contains("close") || n.Contains("exit") || n.Contains("cancel");
        bool primary = n.Contains("process") || n.Contains("book") || n.Contains("save")
                    || n.Contains("add") || n.Contains("search") || n.Contains("fare") || n.Contains("get");

        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 1;
        b.UseVisualStyleBackColor = false;
        b.Font = FBold;
        b.ForeColor = Color.White;
        b.Cursor = Cursors.Hand;
        b.Height = Math.Max(b.Height, 40);

        if (danger) { b.BackColor = Red; b.FlatAppearance.BorderColor = Red; }
        else if (primary) { b.BackColor = Green; b.FlatAppearance.BorderColor = Green; }
        else { b.BackColor = Card; b.ForeColor = HeaderA; b.FlatAppearance.BorderColor = Border; }

        Color baseCol = b.BackColor;
        b.FlatAppearance.MouseOverBackColor = (primary || danger) ? Lighten(baseCol, 0.16f) : Color.FromArgb(238, 245, 254);
        b.FlatAppearance.MouseDownBackColor = (primary || danger) ? Darken(baseCol, 0.16f) : Color.FromArgb(222, 235, 250);

        Round(b, 10);
    }

    private static void StyleGrid(DataGridView d)
    {
        d.EnableHeadersVisualStyles = false;
        d.BackgroundColor = Card;
        d.BorderStyle = BorderStyle.None;
        d.GridColor = Border;
        d.RowHeadersVisible = false;
        d.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        d.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        d.ColumnHeadersDefaultCellStyle.BackColor = HeaderA;
        d.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        d.ColumnHeadersDefaultCellStyle.Font = FBold;
        d.ColumnHeadersDefaultCellStyle.SelectionBackColor = HeaderA;
        d.ColumnHeadersHeight = 40;
        d.DefaultCellStyle.BackColor = Color.White;
        d.DefaultCellStyle.ForeColor = Text;
        d.DefaultCellStyle.Font = FBase;
        d.DefaultCellStyle.SelectionBackColor = Color.FromArgb(214, 232, 252);
        d.DefaultCellStyle.SelectionForeColor = Text;
        d.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 251, 255);
        d.RowTemplate.Height = 36;
        d.AllowUserToResizeRows = false;

        foreach (DataGridViewColumn col in d.Columns)
        {
            if (col is DataGridViewCheckBoxColumn)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                col.Width = 52;
            }
            else
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                col.FillWeight = 100;
            }
        }
    }

    public static void Round(Control c, int r)
    {
        if (c.Width < 2 || c.Height < 2) return;
        var p = new GraphicsPath();
        var b = new Rectangle(0, 0, c.Width, c.Height);
        int dd = Math.Min(r * 2, Math.Min(b.Width, b.Height));
        p.AddArc(b.X, b.Y, dd, dd, 180, 90);
        p.AddArc(b.Right - dd, b.Y, dd, dd, 270, 90);
        p.AddArc(b.Right - dd, b.Bottom - dd, dd, dd, 0, 90);
        p.AddArc(b.X, b.Bottom - dd, dd, dd, 90, 90);
        p.CloseFigure();
        c.Region = new Region(p);
    }

    private static void DrawBorder(Control c, Color col)
    {
        c.Paint += (s, e) =>
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var p = new Pen(col, 1.2F))
                e.Graphics.DrawRectangle(p, 1, 1, c.Width - 3, c.Height - 3);
        };
    }

    private static Color Lighten(Color c, float f) => Color.FromArgb(c.A,
        (int)Math.Min(255, c.R + (255 - c.R) * f),
        (int)Math.Min(255, c.G + (255 - c.G) * f),
        (int)Math.Min(255, c.B + (255 - c.B) * f));

    private static Color Darken(Color c, float f) => Color.FromArgb(c.A,
        (int)(c.R * (1 - f)), (int)(c.G * (1 - f)), (int)(c.B * (1 - f)));

    public static void Header(Form f, string title)
    {
        var bar = new Panel { Dock = DockStyle.Top, Height = 66, Name = "uiHeader" };
        bool drag = false; int px = 0, py = 0;

        bar.Paint += (s, e) =>
        {
            using (var br = new LinearGradientBrush(bar.ClientRectangle, HeaderA, HeaderB, LinearGradientMode.Horizontal))
                e.Graphics.FillRectangle(br, bar.ClientRectangle);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawString(title, FTitle, Brushes.White, 24, 19);
        };
        bar.MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { drag = true; px = Cursor.Position.X - f.Left; py = Cursor.Position.Y - f.Top; } };
        bar.MouseMove += (s, e) => { if (drag) { f.Left = Cursor.Position.X - px; f.Top = Cursor.Position.Y - py; } };
        bar.MouseUp += (s, e) => drag = false;

        var x = new Button
        {
            Name = "uiHeaderClose",
            Text = "\u2715",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = Color.White,
            BackColor = Red,
            FlatStyle = FlatStyle.Flat,
            Size = new Size(44, 36),
            Cursor = Cursors.Hand,
            TabStop = false
        };
        x.FlatAppearance.BorderSize = 0;
        x.FlatAppearance.MouseOverBackColor = Lighten(Red, 0.2f);
        x.FlatAppearance.MouseDownBackColor = Darken(Red, 0.2f);
        x.Click += (s, e) => f.Close();

        bar.Controls.Add(x);
        x.Location = new Point(bar.Width - 58, 15);
        bar.Resize += (s, e) => x.Location = new Point(bar.Width - 58, 15);

        f.Controls.Add(bar);
        f.Controls.SetChildIndex(bar, 0);
    }
}