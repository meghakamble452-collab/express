using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace IRCommDLL
{
    public partial class QrPay : Form
    {
        private clsPostData class42_0;
        internal clsMain clsMain;
        internal string string_1;
        internal string string_2;
        internal string string_3;
        internal string string_0;

        internal bool bool_3;
        internal bool bool_0;

        internal static readonly IntPtr intptr_0 = new IntPtr(-1); // TOPMOST

        // ✅ FIXED: Proper WinAPI import
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags
        );

        public QrPay()
        {
            InitializeComponent();
            base.Name = "frmPayment-" + Thread.CurrentThread.ManagedThreadId;
        }
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
                    val = Math.Max(workingArea.Left, bounds2.Left - (bounds2.Right - workingArea.Right));
                }
                if (bounds2.Top < workingArea.Top)
                {
                    val2 = workingArea.Top;
                }
                if (bounds2.Bottom > workingArea.Bottom)
                {
                    val2 = Math.Max(workingArea.Top, bounds2.Top - (bounds2.Bottom - workingArea.Bottom));
                }
                val = Math.Min(Math.Max(val, bounds.Left), bounds.Right - bounds2.Width);
                val2 = Math.Min(Math.Max(val2, bounds.Top), bounds.Bottom - bounds2.Height);
                if (val != bounds2.Left || val2 != bounds2.Top)
                {
                    form.Location = new Point(val, val2);
                }
            }
        }
        internal void method_0QR(Control control_0, Rectangle rectangle_0)
        {
            try
            {
                _ = Screen.PrimaryScreen.WorkingArea;
                _ = control_0.Controls[0];
                Control control = control_0.Parent;
                base.StartPosition = FormStartPosition.Manual;
                base.Location = control.Location;
                EnsureShowsTaskbar(this);
                _ = control.Location.Y;
                SetWindowPos(base.Handle, intptr_0, 0, 0, 0, 0, 3u);
                _ = base.Top;
                _ = base.Height;
                base.TopMost = true;
                Show();
                pictureBox1.BringToFront();
                Activate();
            }
            catch (Exception)
            {
            }
        }
        internal void method_0AA(Control control_0, Rectangle rectangle_0)
        {
            try
            {
                if (control_0 == null || control_0.Parent == null)
                    return;
                Control parent = control_0.Parent;
                base.StartPosition = FormStartPosition.Manual;
                base.Location = parent.Location;
                base.TopMost = true;
                base.Left += 2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("method_0 error: " + ex.Message);
            }
        }
        internal void method_0(Control control_0, Rectangle rectangle_0)
        {
            Rectangle workingArea = Screen.GetWorkingArea(this);
            base.Left = 0;
            _ = control_0.Controls[0];
            Control control = control_0.Parent;
            base.StartPosition = FormStartPosition.Manual;
            Control parent = control_0.Parent;
            int x = parent.Location.X; 
            int y = parent.Location.Y;
            int space = workingArea.Height;
            if (space > base.Height)
            {
                base.Location = new Point(x, y);
            }
            else
            {
                base.Location = new Point(x, y);
            }
            base.TopMost = true;
            base.Left += 2;
        }
        internal void method_0yy(Control control_0, Rectangle rectangle_0)
        {
            try
            {
                if (control_0 == null || control_0.Parent == null)
                    return;

                Control parent = control_0.Parent;

                // Get correct screen position
                Point parentScreen = parent.PointToScreen(Point.Empty);

                this.StartPosition = FormStartPosition.Manual;

                // Decide position (above or below control)
                int x = parentScreen.X;
                int y;

                // Screen working area
                Rectangle workingArea = Screen.GetWorkingArea(parent);

                // Agar neeche space hai to below show karo
                if (parentScreen.Y + parent.Height + this.Height < workingArea.Bottom)
                {
                    y = parentScreen.Y + parent.Height; // below
                }
                else
                {
                    y = parentScreen.Y - this.Height; // above
                }

                this.Location = new Point(x, y);

                // Optional (use carefully)
                this.TopMost = true;

                // Show safely
                if (!this.Visible)
                    this.Show();

                this.BringToFront();
                this.Activate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
        internal void method_1AA(object string_5)
        {
            string_3 = string_5.ToString();
            string text = string_3;
            string[] array = new string[1];
            string text2 = "|||";
            array[0] = text2;
            string[] array2 = text.Split(array, StringSplitOptions.None);
            pictureBox1.Image = LoadImage(array2[0]);
            string text3 = "Pay Using QR :- ";
            string text4 = array2[1];
            string text5 = "  Amt=  ";
            label3.Text = text3 + text4 + text5 + array2[2];
            this.tktname.Text = array2[2];
            method_0(clsMain.control_0, clsMain.rectangle_0);
        }
        internal DialogResult method_1(Control control_0, Rectangle rectangle_0, clsPostData class42_1, string state)
        {
            class42_0 = class42_1;
            if (this.InvokeRequired)
            {
                return (DialogResult)this.Invoke(new Func<DialogResult>(() =>
                    method_1(control_0, rectangle_0, class42_1, state)
                ));
            }

            method_0(control_0, rectangle_0);

            try
            {
                const uint SWP_NOSIZE = 0x0001;
                const uint SWP_NOMOVE = 0x0002;
                const uint FLAGS = SWP_NOMOVE | SWP_NOSIZE;

                SetWindowPos(this.Handle, intptr_0, 0, 0, 0, 0, FLAGS);

                if (string.IsNullOrEmpty(state))
                {
                    MessageBox.Show("State empty hai");
                    return DialogResult.Cancel;
                }

                string[] array = state.Split(new string[] { "|||" }, StringSplitOptions.None);

                if (array.Length < 3)
                {
                    MessageBox.Show("Invalid QR data format");
                    return DialogResult.Cancel;
                }

                string_0 = array[0];
                string amount = array[2];
                string text = array[1];
                this.tktname.Text = text;
                var img = LoadImage(string_0);
                if (img == null)
                {
                    MessageBox.Show("QR Image load nahi hua");
                    return DialogResult.Cancel;
                }

                pictureBox1.Image = img;
                label3.Text = "Pay Using QR :- " + amount;
                return this.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("method_1 error: " + ex.Message);
                return DialogResult.Cancel;
            }
        }
        public Image LoadImage(string base64)
        {
            try
            {
                if (string.IsNullOrEmpty(base64))
                    return null;

                byte[] bytes = Convert.FromBase64String(base64);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    return Image.FromStream(ms);
                }
            }
            catch
            {
                return null;
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void QrPay_Load_1(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.BringToFront();
            this.Activate();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }
    }
}