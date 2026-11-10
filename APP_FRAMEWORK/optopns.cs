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
    public partial class options : Form
    {
        public options()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmAboutUS frmAboutUS2 = new frmAboutUS();
            frmAboutUS2.Show();
            frmAboutUS2.Activate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmNotes frmNotes2 = new frmNotes(3, 0);
            frmNotes2.Activate();
            frmNotes2.Show();
        }

        private void btnFindTrain_Click(object sender, EventArgs e)
        {
            frmShowHistory frmShowHistory2 = new frmShowHistory();
            frmShowHistory2.Show();
            frmShowHistory2.Activate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new frmProxySetting().Show();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }
    }
}
