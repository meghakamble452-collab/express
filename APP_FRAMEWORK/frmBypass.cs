using NEXUS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace APP_FRAMEWORK
{
    public partial class frmBypass : Form
    {
        public frmBypass()
        {
            InitializeComponent();
            pnlHeader.MouseDown += StatClass.Header_MouseDown;
            pnlHeader.MouseMove += StatClass.Header_MouseMove;
            pnlHeader.MouseUp += StatClass.Header_MouseUp;
        }

        private void btnFindTrain_Click(object sender, EventArgs e)
        {
            new Freecharge_htm().ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new frmSBI().Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void frmBypass_Load(object sender, EventArgs e)
        {

        }
    }
}
