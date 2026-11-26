using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Bunifu.Framework.UI;

namespace Booking;

public class frmMessege : Form
{
	private IContainer icontainer_0;

	private BunifuElipse bunifuElipse_0;

	private Label label2;

	private Label label1;

	private Label label3;

	private Button OK;

	private Panel panel1;

	private IContainer components;

	public frmMessege()
	{
		InitializeComponent();
		ApplyExpressSuccessTheme();
	}

	internal void mm()
	{
		base.TopMost = true;
		ShowDialog();
	}

	private void OK_Click(object sender, EventArgs e)
	{
		base.DialogResult = DialogResult.OK;
	}

	// =====================================================================
	// Express theme - booking success popup (visual layer only).
	// label1 / label2 / label3 / OK keep their names, texts and handlers;
	// their content is painted by the cards below.
	// =====================================================================
	private static readonly Color XM_BG = Color.FromArgb(236, 242, 251);
	private static readonly Color XM_GREEN = Color.FromArgb(22, 163, 74);
	private static readonly Color XM_GREEN_D = Color.FromArgb(21, 128, 61);
	private static readonly Color XM_ORANGE = Color.FromArgb(245, 146, 28);
	private static readonly Color XM_ORANGE_D = Color.FromArgb(214, 96, 4);
	private static readonly Color XM_NAVY = Color.FromArgb(15, 42, 92);
	private static readonly Color XM_TEXT = Color.FromArgb(30, 41, 59);
	private static readonly Color XM_MUTED = Color.FromArgb(100, 116, 139);
	private static readonly Color XM_LINE = Color.FromArgb(220, 228, 241);

	private const int XM_W = 640, XM_HEAD = 78, XM_FOOT = 72, XM_PAD = 16;

	private readonly Font xmFTitle = new Font("Segoe UI Semibold", 15f);
	private readonly Font xmFSub = new Font("Segoe UI", 9.5f);
	private readonly Font xmFCap = new Font("Segoe UI Semibold", 8.5f);
	private readonly Font xmFVal = new Font("Segoe UI Semibold", 10.5f);
	private readonly Font xmFPnr = new Font("Segoe UI", 15f, FontStyle.Bold);
	private readonly Font xmFPara = new Font("Segoe UI", 10.5f);
	private readonly Font xmFBtn = new Font("Segoe UI Semibold", 10.5f);
	private readonly Font xmFFoot = new Font("Segoe UI", 8.5f, FontStyle.Italic);

	private Panel xmHead;
	private Panel xmScroll;
	private XmSurface xmBody;
	private Button xmClose;
	private float xmS = 1f;
	private int xmOkState, xmCloseState;

	private sealed class XmSurface : Panel
	{
		public XmSurface()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
			         ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
		}
	}

	private int Xs(float v) { return (int)Math.Round(v * xmS); }

	private void ApplyExpressSuccessTheme()
	{
		SuspendLayout();
		try
		{
			using (Graphics g = CreateGraphics()) xmS = Math.Max(1f, g.DpiX / 96f);
		}
		catch { xmS = 1f; }

		AutoScaleMode = AutoScaleMode.None;          // layout is computed here in real pixels
		FormBorderStyle = FormBorderStyle.None;
		BackColor = XM_BG;
		DoubleBuffered = true;
		Padding = new Padding(1);

		// designer labels keep their text (callers may set it); the cards paint it
		label1.Visible = false;
		label2.Visible = false;
		label3.Visible = false;
		label1.TextChanged += delegate { XmRelayout(); };
		label2.TextChanged += delegate { XmRelayout(); };
		label3.TextChanged += delegate { if (panel1 != null) panel1.Invalidate(); };

		// ---- header ----
		xmHead = new XmSurface { Dock = DockStyle.Top, Height = Xs(XM_HEAD) };
		xmHead.Paint += XmHeadPaint;
		xmHead.MouseDown += XmDragDown;
		xmClose = new Button
		{
			FlatStyle = FlatStyle.Flat,
			Size = new Size(Xs(36), Xs(36)),   // round corner badge + shadow room
			Cursor = Cursors.Hand,
			TabStop = false,
			Text = "",
			BackColor = XM_GREEN
		};
		xmClose.FlatAppearance.BorderSize = 0;
		xmClose.FlatAppearance.MouseOverBackColor = XM_GREEN;
		xmClose.FlatAppearance.MouseDownBackColor = XM_GREEN;
		xmClose.Paint += XmClosePaint;
		XmHover(xmClose, s => xmCloseState = s);
		xmClose.Click += delegate { OK.PerformClick(); };   // same result as the OK button
		xmHead.Controls.Add(xmClose);
		// close shown as the corner sticker; it runs the same path as xmClose (OK.PerformClick)
		xmClose.Visible = false;
		StInitSticker(delegate { OK.PerformClick(); });
		xmHead.Resize += delegate
		{
			// flush in the header's top-right corner: 30px disc 3px from the top and right edges
			xmClose.Location = new Point(xmHead.Width - xmClose.Width, 0);
			xmClose.BringToFront();
		};

		// ---- footer (existing panel1 with the OK button) ----
		panel1.Dock = DockStyle.Bottom;
		panel1.Height = Xs(XM_FOOT);
		panel1.BackColor = XM_BG;
		typeof(Control).GetProperty("DoubleBuffered",
			System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
			.SetValue(panel1, true, null);
		panel1.Paint += XmFootPaint;
		panel1.Resize += delegate { XmPlaceOk(); };

		OK.FlatStyle = FlatStyle.Flat;
		OK.FlatAppearance.BorderSize = 0;
		OK.FlatAppearance.MouseOverBackColor = XM_BG;
		OK.FlatAppearance.MouseDownBackColor = XM_BG;
		OK.BackColor = XM_BG;
		OK.UseVisualStyleBackColor = false;
		OK.Cursor = Cursors.Hand;
		OK.Size = new Size(Xs(150), Xs(42));
		OK.Anchor = AnchorStyles.None;
		OK.Paint += XmOkPaint;
		XmHover(OK, s => xmOkState = s);

		// ---- body: scrollable surface holding the message card ----
		xmScroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = XM_BG };
		xmBody = new XmSurface { Location = Point.Empty, BackColor = XM_BG };
		xmBody.Paint += XmBodyPaint;
		xmScroll.Controls.Add(xmBody);
		xmScroll.Resize += delegate { XmRelayout(); };

		Controls.Add(xmScroll);
		Controls.Add(xmHead);
		xmScroll.BringToFront();      // fill takes the space between header and footer
		panel1.SendToBack();
		xmHead.SendToBack();

		Paint += XmFramePaint;
		ClientSize = new Size(Xs(XM_W), Xs(XM_HEAD + XM_FOOT + 200));
		ResumeLayout(true);
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		XmRelayout();
		XmPlaceOk();
		CenterToScreen();
	}

	private void XmDragDown(object sender, MouseEventArgs e)
	{
		if (e.Button != MouseButtons.Left) return;
		Point start = Cursor.Position, origin = Location;
		Control c = (Control)sender;
		MouseEventHandler move = null;
		MouseEventHandler up = null;
		move = delegate { Location = new Point(origin.X + Cursor.Position.X - start.X, origin.Y + Cursor.Position.Y - start.Y); };
		up = delegate { c.MouseMove -= move; c.MouseUp -= up; };
		c.MouseMove += move;
		c.MouseUp += up;
	}

	private void XmHover(Control c, Action<int> set)
	{
		c.MouseEnter += delegate { set(1); c.Invalidate(); };
		c.MouseLeave += delegate { set(0); c.Invalidate(); };
		c.MouseDown += delegate (object s, MouseEventArgs e) { if (e.Button == MouseButtons.Left) { set(2); c.Invalidate(); } };
		c.MouseUp += delegate
		{
			if (c.IsDisposed) return;
			set(c.ClientRectangle.Contains(c.PointToClient(Cursor.Position)) ? 1 : 0);
			c.Invalidate();
		};
	}

	private void XmPlaceOk()
	{
		if (OK == null || panel1 == null) return;
		OK.Location = new Point(panel1.Width - OK.Width - Xs(XM_PAD + 2), (panel1.Height - OK.Height) / 2);
	}

	// ---------- message model: "Key : Value" lines become rows, everything else a paragraph ----------

	private sealed class XmLine
	{
		public string Key, Value, Para;
		public bool Divider, Gap;
	}

	private static List<XmLine> XmParse(string text)
	{
		List<XmLine> list = new List<XmLine>();
		string[] lines = (text ?? "").Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
		foreach (string raw in lines)
		{
			string l = raw.Trim();
			if (l.Length == 0)
			{
				if (list.Count > 0 && !list[list.Count - 1].Gap) list.Add(new XmLine { Gap = true });
				continue;
			}
			bool divider = l.Length >= 3;
			foreach (char ch in l) if ("-=_*~.".IndexOf(ch) < 0) { divider = false; break; }
			if (divider) { list.Add(new XmLine { Divider = true }); continue; }

			int i = l.IndexOf(':');
			// a colon between two digits is a time (e.g. 00:00:07), not a "Key : Value" separator
			bool timeColon = i > 0 && i < l.Length - 1 && char.IsDigit(l[i - 1]) && char.IsDigit(l[i + 1]);
			if (i > 0 && i <= 28 && i < l.Length - 1 && !timeColon)
			{
				string k = l.Substring(0, i).Trim().TrimEnd('-', '=', '>').Trim();
				string v = l.Substring(i + 1).Trim().TrimStart('-', '=', '>').Trim();
				if (k.Length > 0 && v.Length > 0 && !k.Contains("http"))
				{
					list.Add(new XmLine { Key = k, Value = v });
					continue;
				}
			}
			list.Add(new XmLine { Para = l });
		}
		while (list.Count > 0 && list[list.Count - 1].Gap) list.RemoveAt(list.Count - 1);
		return list;
	}

	private const TextFormatFlags XM_WRAP = TextFormatFlags.WordBreak | TextFormatFlags.NoPrefix |
	                                        TextFormatFlags.Left | TextFormatFlags.TextBoxControl | TextFormatFlags.NoPadding;

	// Lays out (and optionally paints) the card content; returns the card's inner height.
	private int XmFlow(Graphics g, int x, int y, int w, bool paint)
	{
		List<XmLine> lines = XmParse(label2.Text);
		int keyW = Xs(150), y0 = y;
		if (lines.Count == 0)
		{
			string none = "Your ticket has been booked.";
			Size s = TextRenderer.MeasureText(g, none, xmFPara, new Size(w, 0), XM_WRAP);
			if (paint) TextRenderer.DrawText(g, none, xmFPara, new Rectangle(x, y, w, s.Height), XM_TEXT, XM_WRAP);
			return s.Height;
		}
		bool firstRow = true;
		foreach (XmLine ln in lines)
		{
			if (ln.Gap) { y += Xs(6); continue; }
			if (ln.Divider)
			{
				if (paint) using (Pen p = new Pen(XM_LINE)) g.DrawLine(p, x, y + Xs(6), x + w, y + Xs(6));
				y += Xs(13);
				continue;
			}
			if (ln.Para != null)
			{
				Size s = TextRenderer.MeasureText(g, ln.Para, xmFPara, new Size(w, 0), XM_WRAP);
				if (paint) TextRenderer.DrawText(g, ln.Para, xmFPara, new Rectangle(x, y, w, s.Height + 2), XM_TEXT, XM_WRAP);
				y += s.Height + Xs(6);
				firstRow = true;
				continue;
			}

			bool pnr = ln.Key.IndexOf("PNR", StringComparison.OrdinalIgnoreCase) >= 0;
			Font vf = pnr ? xmFPnr : xmFVal;
			int vw = w - keyW - Xs(12);
			Size ks = TextRenderer.MeasureText(g, ln.Key.ToUpperInvariant(), xmFCap, new Size(keyW - Xs(18), 0), XM_WRAP);
			Size vs = TextRenderer.MeasureText(g, ln.Value, vf, new Size(vw, 0), XM_WRAP);
			int rowH = Math.Max(ks.Height, vs.Height) + Xs(14);
			if (paint)
			{
				if (!firstRow) using (Pen p = new Pen(Color.FromArgb(236, 241, 248))) g.DrawLine(p, x, y, x + w, y);
				if (pnr)
					XmFill(g, new RectangleF(x - Xs(6), y + Xs(3), w + Xs(12), rowH - Xs(6)), Xs(8),
						Color.FromArgb(255, 247, 237), Color.FromArgb(253, 186, 116));
				int cy = y + rowH / 2;
				using (SolidBrush dot = new SolidBrush(pnr ? XM_ORANGE : Color.FromArgb(148, 163, 184)))
					g.FillEllipse(dot, x + Xs(2), cy - Xs(3), Xs(6), Xs(6));
				TextRenderer.DrawText(g, ln.Key.ToUpperInvariant(), xmFCap,
					new Rectangle(x + Xs(14), y + (rowH - ks.Height) / 2, keyW - Xs(18), ks.Height + 2),
					pnr ? XM_ORANGE_D : XM_MUTED, XM_WRAP);
				TextRenderer.DrawText(g, ln.Value, vf,
					new Rectangle(x + keyW, y + (rowH - vs.Height) / 2, vw, vs.Height + 2),
					pnr ? XM_ORANGE_D : XM_NAVY, XM_WRAP);
			}
			y += rowH;
			firstRow = false;
		}
		return y - y0;
	}

	private void XmRelayout()
	{
		if (xmBody == null || xmScroll == null || label2 == null || xmInLayout) return;
		xmInLayout = true;
		try
		{
			Rectangle wa = Screen.FromControl(this).WorkingArea;
			int chrome = Xs(XM_HEAD) + Xs(XM_FOOT) + 2;
			int maxBody = Math.Max(Xs(160), wa.Height - Xs(40) - chrome);
			int fullW = Xs(XM_W) - 2;                                  // form client width minus 1px frame each side

			int bodyH = XmBodyHeight(fullW);
			bool scroll = bodyH > maxBody;
			int bodyW = fullW - (scroll ? SystemInformation.VerticalScrollBarWidth : 0);
			if (scroll) bodyH = XmBodyHeight(bodyW);                    // re-flow for the narrower width

			// grow the window to fit, never beyond the screen; beyond that the body scrolls
			Size want = new Size(Xs(XM_W), chrome + Math.Min(bodyH, maxBody));
			if (ClientSize != want) ClientSize = want;
			xmBody.Size = new Size(bodyW, bodyH);
			xmBody.Invalidate();
			if (xmHead != null) xmHead.Invalidate();
		}
		finally { xmInLayout = false; }
	}

	private bool xmInLayout;

	private int XmBodyHeight(int bodyW)
	{
		int innerW = bodyW - Xs(XM_PAD) * 2 - 1 - Xs(22) * 2;
		int contentH;
		using (Graphics g = CreateGraphics()) contentH = XmFlow(g, 0, 0, innerW, false);
		return Xs(XM_PAD) + Xs(46) + contentH + Xs(18) + Xs(XM_PAD);
	}

	// ---------- painting ----------

	private static GraphicsPath XmRound(RectangleF r, float rad)
	{
		GraphicsPath p = new GraphicsPath();
		float d = Math.Min(rad * 2, Math.Min(r.Width, r.Height));
		if (d < 1) { p.AddRectangle(r); return p; }
		p.AddArc(r.X, r.Y, d, d, 180, 90);
		p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
		p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
		p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
		p.CloseFigure();
		return p;
	}

	private static void XmFill(Graphics g, RectangleF r, float rad, Color fill, Color border)
	{
		if (r.Width < 1 || r.Height < 1) return;
		using (GraphicsPath p = XmRound(r, rad))
		{
			using (SolidBrush b = new SolidBrush(fill)) g.FillPath(b, p);
			if (border.A > 0) using (Pen pen = new Pen(border, 1f)) g.DrawPath(pen, p);
		}
	}

	private Pen XmPen(Color c, float w)
	{
		Pen p = new Pen(c, w * xmS);
		p.StartCap = LineCap.Round; p.EndCap = LineCap.Round; p.LineJoin = LineJoin.Round;
		return p;
	}

	private void XmFramePaint(object sender, PaintEventArgs e)
	{
		using (Pen p = new Pen(Color.FromArgb(178, 196, 226)))
			e.Graphics.DrawRectangle(p, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
	}

	private void XmHeadPaint(object sender, PaintEventArgs e)
	{
		Control c = (Control)sender;
		Graphics g = e.Graphics;
		g.SmoothingMode = SmoothingMode.AntiAlias;
		Rectangle r = c.ClientRectangle;
		using (LinearGradientBrush b = new LinearGradientBrush(r, Color.FromArgb(21, 128, 61), Color.FromArgb(34, 197, 94), LinearGradientMode.Horizontal))
			g.FillRectangle(b, r);
		using (SolidBrush shine = new SolidBrush(Color.FromArgb(22, 255, 255, 255)))
			g.FillRectangle(shine, 0, 0, r.Width, r.Height / 2);
		// bhagwa underline
		using (LinearGradientBrush ob = new LinearGradientBrush(new Rectangle(0, r.Height - Xs(4), Math.Max(1, r.Width), Xs(4)),
			XM_ORANGE_D, Color.FromArgb(248, 158, 44), LinearGradientMode.Horizontal))
			g.FillRectangle(ob, 0, r.Height - Xs(4), r.Width, Xs(4));

		// check-circle on a white disc with a soft halo
		int d = Xs(46), cx = Xs(XM_PAD) + d / 2, cy = (r.Height - Xs(4)) / 2;
		using (SolidBrush halo = new SolidBrush(Color.FromArgb(50, 255, 255, 255)))
			g.FillEllipse(halo, cx - d / 2 - Xs(4), cy - d / 2 - Xs(4), d + Xs(8), d + Xs(8));
		using (SolidBrush wb = new SolidBrush(Color.White))
			g.FillEllipse(wb, cx - d / 2, cy - d / 2, d, d);
		using (Pen p = XmPen(XM_GREEN, 2.4f))
		{
			float rr = d * 0.30f;
			g.DrawEllipse(p, cx - rr, cy - rr, rr * 2, rr * 2);
			g.DrawLines(p, new[] { new PointF(cx - rr * 0.45f, cy + rr * 0.02f), new PointF(cx - rr * 0.08f, cy + rr * 0.38f), new PointF(cx + rr * 0.5f, cy - rr * 0.3f) });
		}

		int tx = cx + d / 2 + Xs(14);
		int right = (xmClose != null ? xmClose.Left : r.Width) - Xs(10);
		string sub = (label1.Text ?? "").Trim();
		Size ts = TextRenderer.MeasureText(g, "Ticket Booked Successfully", xmFTitle, Size.Empty, TextFormatFlags.NoPadding);
		int ty = sub.Length > 0 ? cy - ts.Height + Xs(2) : cy - ts.Height / 2;
		TextRenderer.DrawText(g, "Ticket Booked Successfully", xmFTitle, new Rectangle(tx, ty, Math.Max(10, right - tx), ts.Height + 2),
			Color.White, TextFormatFlags.Left | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine);
		if (sub.Length > 0)
			TextRenderer.DrawText(g, sub, xmFSub, new Rectangle(tx + 1, cy + Xs(3), Math.Max(10, right - tx), Xs(20)),
				Color.FromArgb(220, 252, 231), TextFormatFlags.Left | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.NoPrefix);
	}

	private void XmBodyPaint(object sender, PaintEventArgs e)
	{
		Graphics g = e.Graphics;
		g.SmoothingMode = SmoothingMode.AntiAlias;
		g.Clear(XM_BG);
		int pad = Xs(XM_PAD);
		RectangleF card = new RectangleF(pad, pad, xmBody.Width - pad * 2 - 1, xmBody.Height - pad * 2);
		XmFill(g, new RectangleF(card.X, card.Y + 2, card.Width, card.Height), Xs(12), Color.FromArgb(214, 225, 242), Color.Empty);
		XmFill(g, card, Xs(12), Color.White, XM_LINE);

		// card title: ticket icon + "BOOKING DETAILS" with bhagwa bar
		int x = (int)card.X + Xs(22), y = (int)card.Y + Xs(14);
		XmFill(g, new RectangleF(card.X + 1, card.Y + Xs(14), Xs(4), Xs(20)), Xs(2), XM_ORANGE, Color.Empty);
		using (Pen p = XmPen(XM_ORANGE, 1.8f))
		{
			RectangleF t = new RectangleF(x, y + Xs(3), Xs(20), Xs(14));
			using (GraphicsPath tp = XmRound(t, Xs(3))) g.DrawPath(p, tp);
			g.DrawLine(p, t.X + t.Width * 0.62f, t.Y + Xs(3), t.X + t.Width * 0.62f, t.Bottom - Xs(3));
		}
		TextRenderer.DrawText(g, "BOOKING DETAILS", xmFCap, new Point(x + Xs(28), y + Xs(2)), XM_NAVY, TextFormatFlags.NoPadding);
		using (Pen p = new Pen(XM_LINE)) g.DrawLine(p, x, y + Xs(28), card.Right - Xs(22), y + Xs(28));

		XmFlow(g, x, (int)card.Y + Xs(46), (int)card.Width - Xs(22) * 2, true);
	}

	private void XmFootPaint(object sender, PaintEventArgs e)
	{
		Control c = (Control)sender;
		Graphics g = e.Graphics;
		g.SmoothingMode = SmoothingMode.AntiAlias;
		g.Clear(XM_BG);
		using (Pen p = new Pen(XM_LINE)) g.DrawLine(p, Xs(XM_PAD), 0, c.Width - Xs(XM_PAD), 0);
		// shield-check + regards line (label3's text)
		int cy = c.Height / 2, x = Xs(XM_PAD + 4);
		using (Pen p = XmPen(XM_GREEN, 1.8f))
		{
			float w = Xs(16), h = Xs(18), sx = x, sy = cy - h / 2;
			g.DrawLines(p, new[] { new PointF(sx + w / 2, sy), new PointF(sx + w, sy + h * 0.2f), new PointF(sx + w, sy + h * 0.5f),
				new PointF(sx + w / 2, sy + h), new PointF(sx, sy + h * 0.5f), new PointF(sx, sy + h * 0.2f), new PointF(sx + w / 2, sy) });
			g.DrawLines(p, new[] { new PointF(sx + w * 0.3f, sy + h * 0.5f), new PointF(sx + w * 0.46f, sy + h * 0.64f), new PointF(sx + w * 0.72f, sy + h * 0.36f) });
		}
		int right = (OK != null ? OK.Left : c.Width) - Xs(12);
		TextRenderer.DrawText(g, label3.Text ?? "", xmFFoot, new Rectangle(x + Xs(24), 0, Math.Max(10, right - x - Xs(24)), c.Height),
			XM_MUTED, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix | TextFormatFlags.WordBreak);
	}

	private void XmOkPaint(object sender, PaintEventArgs e)
	{
		Button b = (Button)sender;
		Graphics g = e.Graphics;
		g.SmoothingMode = SmoothingMode.AntiAlias;
		g.Clear(XM_BG);
		int st = xmOkState;
		RectangleF r = new RectangleF(0.5f, st == 2 ? 2f : 0.5f, b.Width - 2, b.Height - 3);
		Color top = Color.FromArgb(251, 166, 60), bot = XM_ORANGE_D;
		if (st == 1) { top = Color.FromArgb(253, 186, 100); bot = Color.FromArgb(234, 118, 14); }
		if (st == 2) { top = Color.FromArgb(234, 124, 10); bot = Color.FromArgb(180, 74, 4); }
		if (st != 2) XmFill(g, new RectangleF(r.X, r.Y + 2, r.Width, r.Height), Xs(10), Color.FromArgb(70, 194, 84, 0), Color.Empty);
		using (GraphicsPath p = XmRound(r, Xs(10)))
		using (LinearGradientBrush lb = new LinearGradientBrush(r, top, bot, LinearGradientMode.Vertical))
			g.FillPath(lb, p);
		if (st == 1) XmFill(g, r, Xs(10), Color.Transparent, XM_NAVY);

		string text = string.IsNullOrEmpty(b.Text) ? "OK" : b.Text;
		Size ts = TextRenderer.MeasureText(g, text, xmFBtn, Size.Empty, TextFormatFlags.NoPadding);
		int iw = Xs(16), gw = iw + Xs(8) + ts.Width;
		int x = (int)(r.X + (r.Width - gw) / 2), cy = (int)(r.Y + r.Height / 2);
		using (Pen p = XmPen(Color.White, 2.2f))
			g.DrawLines(p, new[] { new PointF(x + iw * 0.1f, cy), new PointF(x + iw * 0.4f, cy + iw * 0.3f), new PointF(x + iw, cy - iw * 0.32f) });
		TextRenderer.DrawText(g, text, xmFBtn, new Point(x + iw + Xs(8), cy - ts.Height / 2), Color.White, TextFormatFlags.NoPadding);
	}

    // ===== Close "sticker" (same as the ticket popup): a round red X badge whose centre sits on the
    // window's top-right corner, about half outside it. WinForms can't paint outside a form, so it is a
    // tiny owned layered window (WS_EX_LAYERED | WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE) with per-pixel alpha.
    // Its click runs exactly the existing close path; the in-header close control is only hidden.
    private StickerWindow stSticker;

    private void StInitSticker(MethodInvoker click)
    {
        if (stSticker != null) return;
        stSticker = new StickerWindow(this, click);
        EventHandler sync = delegate { StSyncSticker(); };
        Move += sync;
        Resize += sync;
        LocationChanged += sync;
        SizeChanged += sync;
        VisibleChanged += sync;
        Activated += sync;
        Shown += sync;
        HandleDestroyed += delegate { if (stSticker != null) stSticker.DropHandle(); };   // recreated on next sync
        FormClosed += delegate { StKillSticker(); };
        Disposed += delegate { StKillSticker(); };
    }

    private void StSyncSticker()
    {
        if (stSticker == null || IsDisposed || Disposing || !IsHandleCreated) return;
        stSticker.Sync(Visible && WindowState != FormWindowState.Minimized);
    }

    private void StKillSticker()
    {
        if (stSticker == null) return;
        stSticker.Dispose();
        stSticker = null;
    }

    private sealed class StickerWindow : NativeWindow, IDisposable
    {
        private const int WS_POPUP = unchecked((int)0x80000000);
        private const int WS_EX_LAYERED = 0x80000, WS_EX_TOOLWINDOW = 0x80, WS_EX_NOACTIVATE = 0x8000000;
        private const int SW_HIDE = 0, SW_SHOWNOACTIVATE = 4;

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct XPoint { public int X, Y; }
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct XSize { public int CX, CY; }
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
        private struct XBlend { public byte Op, Flags, Alpha, Format; }
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct XTrack { public int cbSize, dwFlags; public IntPtr hwnd; public int dwHoverTime; }

        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern IntPtr GetDC(IntPtr h);
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern int ReleaseDC(IntPtr h, IntPtr dc);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")] private static extern IntPtr CreateCompatibleDC(IntPtr dc);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")] private static extern bool DeleteDC(IntPtr dc);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")] private static extern IntPtr SelectObject(IntPtr dc, IntPtr o);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")] private static extern bool DeleteObject(IntPtr o);
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern bool UpdateLayeredWindow(IntPtr h, IntPtr dst, ref XPoint pt, ref XSize sz, IntPtr src, ref XPoint srcPt, int key, ref XBlend blend, int flags);
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern bool ShowWindow(IntPtr h, int cmd);
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern bool IsWindowVisible(IntPtr h);
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern IntPtr SetCapture(IntPtr h);
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern bool ReleaseCapture();
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern IntPtr SetCursor(IntPtr c);
        [System.Runtime.InteropServices.DllImport("user32.dll")] private static extern bool TrackMouseEvent(ref XTrack t);

        private readonly Form owner;
        private readonly MethodInvoker click;
        private bool hot, down, tracking, dead;
        private float k = 1f;
        private int size;

        public StickerWindow(Form owner, MethodInvoker click)
        {
            this.owner = owner;
            this.click = click;
        }

        public bool Hot { get { return hot; } set { hot = value; Render(); } }
        public IntPtr Hwnd { get { return Handle; } }

        public void Sync(bool show)
        {
            if (dead) return;
            if (!show)
            {
                if (Handle != IntPtr.Zero) ShowWindow(Handle, SW_HIDE);
                hot = down = false;
                return;
            }
            if (Handle == IntPtr.Zero)
            {
                try { using (Graphics g = Graphics.FromHwnd(owner.Handle)) k = Math.Max(1f, g.DpiX / 96f); }
                catch { k = 1f; }
                size = (int)Math.Ceiling(46f * k);          // 32px disc + hover growth + shadow
                CreateParams cp = new CreateParams();
                cp.Caption = "";
                cp.X = 0; cp.Y = 0; cp.Width = size; cp.Height = size;
                cp.Style = WS_POPUP;
                cp.ExStyle = WS_EX_LAYERED | WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE;
                cp.Parent = owner.Handle;                     // = owner for a popup: stays above it, hides/minimises with it
                CreateHandle(cp);
            }
            Render();
            if (!IsWindowVisible(Handle)) ShowWindow(Handle, SW_SHOWNOACTIVATE);
        }

        public void DropHandle()
        {
            if (Handle != IntPtr.Zero) DestroyHandle();
        }

        public void Dispose()
        {
            dead = true;
            DropHandle();
        }

        // Badge centre sits on the owner's top-right corner, (Right - 8, Top + 8) in screen pixels.
        private Point Origin()
        {
            Rectangle ob = owner.Bounds;
            int cx = ob.Right - (int)Math.Round(8f * k), cy = ob.Top + (int)Math.Round(8f * k);
            return new Point(cx - size / 2, cy - size / 2);
        }

        private void Render()
        {
            if (dead || Handle == IntPtr.Zero || size <= 0) return;
            using (Bitmap bmp = new Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.Clear(Color.Transparent);
                    Draw(g);
                }
                IntPtr screen = GetDC(IntPtr.Zero);
                IntPtr mem = CreateCompatibleDC(screen);
                IntPtr hbmp = bmp.GetHbitmap(Color.FromArgb(0));   // 32bpp, premultiplied for UpdateLayeredWindow
                IntPtr old = SelectObject(mem, hbmp);
                try
                {
                    Point o = Origin();
                    XPoint dst = new XPoint { X = o.X, Y = o.Y };
                    XPoint src = new XPoint();
                    XSize sz = new XSize { CX = size, CY = size };
                    XBlend bl = new XBlend { Op = 0, Flags = 0, Alpha = 255, Format = 1 };   // AC_SRC_OVER, AC_SRC_ALPHA
                    UpdateLayeredWindow(Handle, screen, ref dst, ref sz, mem, ref src, 0, ref bl, 2);  // ULW_ALPHA
                }
                finally
                {
                    SelectObject(mem, old);
                    DeleteObject(hbmp);
                    DeleteDC(mem);
                    ReleaseDC(IntPtr.Zero, screen);
                }
            }
        }

        private float Radius() { return (16f + (hot && !down ? 1f : 0f)) * k; }

        private bool InDisc(int x, int y)
        {
            float c = size / 2f, r = Radius() + 1f * k;
            float dx = x - c, dy = y - c;
            return dx * dx + dy * dy <= r * r;
        }

        private void Draw(Graphics g)
        {
            bool h = hot && !down;
            float r = Radius();
            float cx = size / 2f, cy = size / 2f + (down ? k : 0f);
            RectangleF disc = new RectangleF(cx - r, cy - r, r * 2, r * 2);
            if (!down)
            {
                using (SolidBrush s1 = new SolidBrush(Color.FromArgb(40, 0, 0, 0)))
                    g.FillEllipse(s1, disc.X - 0.8f * k, disc.Y + 2.6f * k, disc.Width + 1.6f * k, disc.Height + 0.6f * k);
                using (SolidBrush s2 = new SolidBrush(Color.FromArgb(55, 0, 0, 0)))
                    g.FillEllipse(s2, disc.X + 0.4f * k, disc.Y + 1.5f * k, disc.Width - 0.8f * k, disc.Height - 0.4f * k);
            }
            Color red = Color.FromArgb(220, 38, 38);
            using (SolidBrush rb = new SolidBrush(h ? red : Color.White)) g.FillEllipse(rb, disc);
            RectangleF inner = RectangleF.Inflate(disc, -2f * k, -2f * k);
            using (SolidBrush fb = new SolidBrush(down ? Color.FromArgb(153, 27, 27) : h ? Color.White : red)) g.FillEllipse(fb, inner);
            if (!h && !down)
                using (SolidBrush gl = new SolidBrush(Color.FromArgb(40, 255, 255, 255)))
                    g.FillEllipse(gl, inner.X + inner.Width * 0.18f, inner.Y + 1.5f * k, inner.Width * 0.64f, inner.Height * 0.42f);
            float a = r * 0.34f;
            using (Pen x = new Pen(h ? red : Color.White, 2.8f * k))
            {
                x.StartCap = LineCap.Round;
                x.EndCap = LineCap.Round;
                g.DrawLine(x, cx - a, cy - a, cx + a, cy + a);
                g.DrawLine(x, cx + a, cy - a, cx - a, cy + a);
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x0021:   // WM_MOUSEACTIVATE -> never take focus from the popup
                    m.Result = (IntPtr)3;   // MA_NOACTIVATE
                    return;
                case 0x0020:   // WM_SETCURSOR
                    SetCursor(Cursors.Hand.Handle);
                    m.Result = (IntPtr)1;
                    return;
                case 0x0084:   // WM_NCHITTEST: only the disc is clickable, the rest is see-through
                {
                    long lp = m.LParam.ToInt64();
                    int sx = (short)(lp & 0xFFFF), sy = (short)((lp >> 16) & 0xFFFF);
                    Point o = Origin();
                    m.Result = InDisc(sx - o.X, sy - o.Y) ? (IntPtr)1 : (IntPtr)(-1);   // HTCLIENT / HTTRANSPARENT
                    return;
                }
                case 0x0200:   // WM_MOUSEMOVE
                    if (!tracking)
                    {
                        XTrack t = new XTrack { cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(XTrack)), dwFlags = 2, hwnd = Handle };
                        tracking = TrackMouseEvent(ref t);
                    }
                    if (!hot) { hot = true; Render(); }
                    m.Result = IntPtr.Zero;
                    return;
                case 0x02A3:   // WM_MOUSELEAVE
                    tracking = false;
                    if (hot || down) { hot = false; down = false; Render(); }
                    return;
                case 0x0201:   // WM_LBUTTONDOWN
                    down = true;
                    SetCapture(Handle);
                    Render();
                    m.Result = IntPtr.Zero;
                    return;
                case 0x0202:   // WM_LBUTTONUP
                {
                    bool was = down;
                    down = false;
                    ReleaseCapture();
                    long lp = m.LParam.ToInt64();
                    int x = (short)(lp & 0xFFFF), y = (short)((lp >> 16) & 0xFFFF);
                    bool inside = InDisc(x, y);
                    hot = inside;
                    Render();
                    if (was && inside && !dead && owner != null && !owner.IsDisposed && owner.IsHandleCreated)
                        owner.BeginInvoke(click);   // exactly the existing btnClose click path
                    m.Result = IntPtr.Zero;
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }

	private void XmClosePaint(object sender, PaintEventArgs e)
	{
		Control c = (Control)sender;
		Graphics g = e.Graphics;
		g.SmoothingMode = SmoothingMode.AntiAlias;
		// repaint the header gradient behind so the corners blend
		Control parent = c.Parent;
		Rectangle pr = new Rectangle(-c.Left, -c.Top, parent != null ? Math.Max(1, parent.Width) : c.Width, parent != null ? Math.Max(1, parent.Height) : c.Height);
		using (LinearGradientBrush b = new LinearGradientBrush(pr, Color.FromArgb(21, 128, 61), Color.FromArgb(34, 197, 94), LinearGradientMode.Horizontal))
			g.FillRectangle(b, 0, 0, c.Width, c.Height);
		int ph = parent != null ? parent.Height : c.Height;
		using (SolidBrush shine = new SolidBrush(Color.FromArgb(22, 255, 255, 255)))
			g.FillRectangle(shine, 0, 0, c.Width, Math.Max(0, ph / 2 - c.Top));

		// Round red badge patched on the corner: shadow, white ring, red disc, bold white X.
		// Hover: grows 2px, white disc + red X + red ring. Pressed: dark red, 1px down.
		int st = xmCloseState;
		bool hot = st == 1, down = st == 2;
		float k = xmS;
		float d = (30f + (hot ? 2f : 0f)) * k;
		float cx = Math.Min(c.Width, c.Height) / 2f, cy = cx + (down ? k : 0f);
		RectangleF disc = new RectangleF(cx - d / 2f, cy - d / 2f, d, d);
		if (!down)
		{
			using (SolidBrush s1 = new SolidBrush(Color.FromArgb(34, 0, 0, 0)))
				g.FillEllipse(s1, disc.X - 0.5f * k, disc.Y + 2.4f * k, disc.Width + k, disc.Height + 0.4f * k);
			using (SolidBrush s2 = new SolidBrush(Color.FromArgb(46, 0, 0, 0)))
				g.FillEllipse(s2, disc.X + 0.5f * k, disc.Y + 1.4f * k, disc.Width - k, disc.Height - 0.4f * k);
		}
		Color red = Color.FromArgb(220, 38, 38);
		using (SolidBrush rb = new SolidBrush(hot ? red : Color.White)) g.FillEllipse(rb, disc);
		RectangleF inner = RectangleF.Inflate(disc, -2f * k, -2f * k);
		using (SolidBrush fb = new SolidBrush(down ? Color.FromArgb(153, 27, 27) : hot ? Color.White : red)) g.FillEllipse(fb, inner);
		if (!hot && !down)
			using (SolidBrush gl = new SolidBrush(Color.FromArgb(38, 255, 255, 255)))
				g.FillEllipse(gl, inner.X + inner.Width * 0.18f, inner.Y + 1.5f * k, inner.Width * 0.64f, inner.Height * 0.42f);
		float a = d * 0.17f;
		using (Pen p = new Pen(hot ? red : Color.White, 2.6f * k))
		{
			p.StartCap = LineCap.Round;
			p.EndCap = LineCap.Round;
			g.DrawLine(p, cx - a, cy - a, cx + a, cy + a);
			g.DrawLine(p, cx + a, cy - a, cx - a, cy + a);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && icontainer_0 != null)
		{
			icontainer_0.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		this.bunifuElipse_0 = new Bunifu.Framework.UI.BunifuElipse(this.components);
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.OK = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.bunifuElipse_0.ElipseRadius = 5;
		this.bunifuElipse_0.TargetControl = this;
		this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(254, 12);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(315, 36);
		this.label1.TabIndex = 1;
		this.label1.Text = "Important information";
		this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(15, 65);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(805, 304);
		this.label2.TabIndex = 1;
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.OK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.OK.Location = new System.Drawing.Point(644, 37);
		this.OK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.OK.Name = "OK";
		this.OK.Size = new System.Drawing.Size(182, 50);
		this.OK.TabIndex = 2;
		this.OK.Text = "OK";
		this.OK.UseVisualStyleBackColor = true;
		this.OK.Click += new System.EventHandler(OK_Click);
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(289, 51);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(209, 20);
		this.label3.TabIndex = 3;
		this.label3.Text = "Regard's *** Nget Admin***";
		this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
		this.panel1.Controls.Add(this.OK);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Location = new System.Drawing.Point(-6, 373);
		this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(844, 104);
		this.panel1.TabIndex = 4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(154f, 154f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		base.ClientSize = new System.Drawing.Size(835, 476);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
		base.Name = "frmMessege";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "frmMessege";
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
