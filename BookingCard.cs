using System;
using System.Drawing;
using System.Windows.Forms;

namespace ByteNova
{
    public partial class BookingCard : UserControl
    {
        private Booking _booking;

        public event EventHandler<Booking> PrintClicked;
        public event EventHandler<Booking> CancelClicked;
        public event EventHandler<Booking> DetailClicked;

        private Label lblPNR;
        private Label lblTrain;
        private Label lblJourney;
        private Label lblStatus;

        private Button btnPrint;
        private Button btnCancel;
        private Button btnDetail;

        public BookingCard()
        {
            InitializeComponent();
            CreateControls();
        }

        private void CreateControls()
        {
            this.Width = 750;
            this.Height = 120;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Margin = new Padding(5);

            lblPNR = new Label();
            lblPNR.Location = new Point(10, 10);
            lblPNR.AutoSize = true;

            lblTrain = new Label();
            lblTrain.Location = new Point(10, 35);
            lblTrain.AutoSize = true;

            lblJourney = new Label();
            lblJourney.Location = new Point(10, 60);
            lblJourney.AutoSize = true;

            lblStatus = new Label();
            lblStatus.Location = new Point(10, 85);
            lblStatus.AutoSize = true;

            btnPrint = new Button();
            btnPrint.Text = "Print";
            btnPrint.Size = new Size(80, 30);
            btnPrint.Location = new Point(500, 20);
            btnPrint.Click += BtnPrint_Click;

            btnCancel = new Button();
            btnCancel.Text = "Cancel";
            btnCancel.Size = new Size(80, 30);
            btnCancel.Location = new Point(590, 20);
            btnCancel.Click += BtnCancel_Click;

            btnDetail = new Button();
            btnDetail.Text = "Change Boarding Point";
            btnDetail.Size = new Size(180, 30);
            btnDetail.Location = new Point(500, 60);
            btnDetail.Click += BtnDetail_Click;

            Controls.Add(lblPNR);
            Controls.Add(lblTrain);
            Controls.Add(lblJourney);
            Controls.Add(lblStatus);
            Controls.Add(btnPrint);
            Controls.Add(btnCancel);
            Controls.Add(btnDetail);
        }

        public void SetData_1(Booking booking)
        {
            _booking = booking;

            lblPNR.Text = "PNR : " + booking.pnrNumber;
            lblTrain.Text = booking.trainNumber + " - " + booking.trainName;
            lblJourney.Text = "Journey : " + booking.journeyDate;
            lblStatus.Text = "Status : " + booking.reservationStatus;

            if (booking.reservationStatus != null &&
                booking.reservationStatus.ToUpper().Contains("CANCEL"))
            {
                lblStatus.ForeColor = Color.Red;
            }
            else
            {
                lblStatus.ForeColor = Color.Green;
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            PrintClicked?.Invoke(this, _booking);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            CancelClicked?.Invoke(this, _booking);
        }

        private void BtnDetail_Click(object sender, EventArgs e)
        {
            DetailClicked?.Invoke(this, _booking);
        }
        public void SetData(Booking booking)
        {
            _booking = booking;

            lblPNR.Text = "PNR : " + booking.pnrNumber;

            lblTrain.Text =
                booking.trainNumber + " - " + booking.trainName +
                Environment.NewLine +
                booking.fromStn + " ➜ " + booking.destStn +
                Environment.NewLine +
                "Class : " + booking.journeyClass +
                "    Quota : " + booking.journeyQuota;

            lblJourney.Text =
                "Journey : " +
                DateTime.Parse(booking.journeyDate).ToString("dd-MMM-yyyy") +
                "    Passenger : " + booking.noOfBkdPsgn;

            lblStatus.Text = booking.reservationStatus;

            if (booking.reservationStatus.ToUpper().Contains("BOOK"))
            {
                lblStatus.ForeColor = Color.Green;
            }
            else
            {
                lblStatus.ForeColor = Color.Red;
            }
        }

        private void BookingCard_Load(object sender, EventArgs e)
        {

        }
    }
}