using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Bunifu.Framework.UI;
using NEXUS.Properties;

namespace APP_FRAMEWORK;

internal class frmManageTicket : Form
{
    private int B0AAA;

    private int C0AA0;

    private int int_0;

    private int int_1;

    private IDisposable idisposable_0;

    private Panel panel1;

    internal Button btnSubmitAll;

    internal Button btnSubmit;

    internal Button btnDelete;

    internal Button btnDeleteAll;

    internal Button btnEdit;

    internal GroupBox grpOpen;

    private Label label2;

    private Label lblCount;

    internal Button btnNew;

    private CheckBox chkAutoWEB;

    private ComboBox cboCredit;

    private Label label34;

    private RadioButton C1AA0;

    private RadioButton radioVPair;

    private ComboBox cboSeconds;

    private ComboBox cboMinutes;

    private Label lblDot;

    private ComboBox cboHours;

    private Label lblDott;

    private DataGridView grdUsers;

    private BackgroundWorker backgroundWorker_0;

    private Panel pnlHeader;

    private BunifuImageButton btnClose;

    private Label B2AAA;

    private BunifuElipse C2AA0;

    private Label label5;

    private IContainer components;

    private CheckBox chkAutoAPP;

    private DataGridViewTextBoxColumn clmSN;

    private DataGridViewTextBoxColumn clmNm;

    private DataGridViewTextBoxColumn clmFrm;

    private DataGridViewTextBoxColumn clmTo;

    private DataGridViewTextBoxColumn clmDoj;

    private DataGridViewTextBoxColumn clmQt;

    private DataGridViewTextBoxColumn clmcl;

    private DataGridViewComboBoxColumn Slot;

    private DataGridViewComboBoxColumn ClmN;

    private DataGridViewComboBoxColumn ClmA;

    private DataGridViewButtonColumn clmAvtive;

    private DataGridViewButtonColumn Login;

    private DataGridViewButtonColumn clmDelete;

    private DataGridViewButtonColumn ClmHis;

    public frmManageTicket()
    {
        StatClass.Tktlist_0 = new List<string>();
        InitializeComponent();
        ApplyByteNovaNovaStyle();
        pnlHeader.MouseDown += StatClass.Header_MouseDown;
        pnlHeader.MouseMove += StatClass.Header_MouseMove;
        pnlHeader.MouseUp += StatClass.Header_MouseUp;
        frmBackup.ImportTicket();
    }

    private void frmManageTicket_Load(object sender, EventArgs e)
    {
        StatClass.firstpair = false;
        StatClass.WebLogin = false;
        StatClass.ApptLogin = false;
        StatClass.int_TotalNoOfPair = 0;
        _ = Screen.PrimaryScreen.WorkingArea.Height;
        _ = Screen.PrimaryScreen.WorkingArea.Width;
        B0AAA = 1;
        C0AA0 = 0;
        method_0();
        cboCredit.SelectedIndex = 0;
        cboHours.SelectedIndex = 0;
        cboMinutes.SelectedIndex = 0;
        cboSeconds.SelectedIndex = 0;
        if (StatClass.HPair)
        {
            C1AA0.Checked = StatClass.HPair;
        }
        else if (StatClass.VPair)
        {
            radioVPair.Checked = StatClass.VPair;
        }
    }

    private void method_0()
    {
        int num = 0;
        int num2 = 0;
        string text = "SELECT TKT_NAME ,SRC_STN,DEST_STN,TKT_SLOT,((CASE WHEN length(TKT_DAY) = 2 then TKT_DAY else '0' || TKT_DAY end) || '-' || (CASE WHEN length(TKT_MONTH) = 2 then TKT_MONTH else '0' || TKT_MONTH end) || '-' || TKT_YEAR) as DOJ,  TKT_QUOTA,  TKT_CLASS, TRAIN_NO FROM TBL_TKT ORDER BY TKT_YEAR, cast(TKT_MONTH as int), cast(TKT_DAY as int)";
        string commandText = text;
        try
        {
            string keyStatus = Program.KeyStatus;
            string text2 = "Paid";
            bool flag = keyStatus == text2;
            string keyType = Program.KeyType;
            string text3 = "demo";
            int num3 = ((!(flag & (keyType != text3))) ? 12 : 20);
            string[] array = new string[num3 + 1];
            for (int i = 0; i <= num3; i++)
            {
                array[i] = i.ToString();
            }
            string[] array2 = new string[Program.int_Slot];
            int num4 = 0;
            for (int j = 1; j <= Program.int_Slot; j++)
            {
                int num5 = num4;
                string text4 = "Slot-";
                array2[num5] = text4 + j;
                num4++;
            }
            DataTable dataTable = new DataTable();
            SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(commandText, Class34.string_Database);
            sQLiteDataAdapter.Fill(dataTable);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    num2++;
                    grdUsers.Rows.Add(num2.ToString(), row[0].ToString(), row[1].ToString(), row[2].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString());
                    string text5 = row[3].ToString();
                    string text6 = text5;
                    string text7 = "Select_Auto Slot";
                    int index;
                    if (text6 == text7)
                    {
                        index = 0;
                    }
                    else
                    {
                        string text8 = text5;
                        string oldValue = "Slot-";
                        text5 = text8.Replace(oldValue, "");
                        if (string.IsNullOrEmpty(text5))
                        {
                            string text9 = "1";
                            text5 = text9;
                        }
                        index = int.Parse(text5);
                        index = ((index <= array2.Length) ? (index - 1) : 0);
                    }
                    DataGridViewComboBoxColumn dataGridViewComboBoxColumn = (DataGridViewComboBoxColumn)grdUsers.Columns[7];
                    dataGridViewComboBoxColumn.Items.Clear();
                    DataGridViewComboBoxCell.ObjectCollection items = dataGridViewComboBoxColumn.Items;
                    object[] array3 = array2;
                    object[] array4 = array3;
                    object[] items2 = array4;
                    items.AddRange(items2);
                    DataGridViewColumnCollection columns = grdUsers.Columns;
                    string columnName = "ClmN";
                    DataGridViewComboBoxColumn dataGridViewComboBoxColumn2 = (DataGridViewComboBoxColumn)columns[columnName];
                    dataGridViewComboBoxColumn2.Items.Clear();
                    DataGridViewComboBoxCell.ObjectCollection items3 = dataGridViewComboBoxColumn2.Items;
                    array3 = array;
                    array4 = array3;
                    items2 = array4;
                    items3.AddRange(items2);
                    DataGridViewColumnCollection columns2 = grdUsers.Columns;
                    string columnName2 = "ClmA";
                    DataGridViewComboBoxColumn dataGridViewComboBoxColumn3 = (DataGridViewComboBoxColumn)columns2[columnName2];
                    dataGridViewComboBoxColumn3.Items.Clear();
                    DataGridViewComboBoxCell.ObjectCollection items4 = dataGridViewComboBoxColumn3.Items;
                    array3 = array;
                    array4 = array3;
                    items2 = array4;
                    items4.AddRange(items2);
                    DataGridViewCell dataGridViewCell = grdUsers.Rows[num].Cells[10];
                    string value = "Open";
                    dataGridViewCell.Value = value;
                    DataGridViewCell dataGridViewCell2 = grdUsers.Rows[num].Cells[11];
                    string value2 = "Login";
                    dataGridViewCell2.Value = value2;
                    DataGridViewCell dataGridViewCell3 = grdUsers.Rows[num].Cells[12];
                    string value3 = "Edit";
                    dataGridViewCell3.Value = value3;
                    DataGridViewCell dataGridViewCell4 = grdUsers.Rows[num].Cells[13];
                    string value4 = "Delete";
                    dataGridViewCell4.Value = value4;
                    grdUsers.Rows[num].Cells[7].Value = (grdUsers.Rows[num].Cells[7] as DataGridViewComboBoxCell).Items[index];
                    DataGridViewCellCollection cells = grdUsers.Rows[num].Cells;
                    string columnName3 = "ClmN";
                    DataGridViewCell dataGridViewCell5 = cells[columnName3];
                    DataGridViewCellCollection cells2 = grdUsers.Rows[num].Cells;
                    string columnName4 = "ClmN";
                    dataGridViewCell5.Value = (cells2[columnName4] as DataGridViewComboBoxCell).Items[B0AAA];
                    DataGridViewCellCollection cells3 = grdUsers.Rows[num].Cells;
                    string columnName5 = "ClmA";
                    DataGridViewCell dataGridViewCell6 = cells3[columnName5];
                    DataGridViewCellCollection cells4 = grdUsers.Rows[num].Cells;
                    string columnName6 = "ClmA";
                    dataGridViewCell6.Value = (cells4[columnName6] as DataGridViewComboBoxCell).Items[C0AA0];
                    num++;
                }
            }
            else
            {
                string text10 = "No Saved Ticket Found!";
                MessageBox.Show(text10);
                base.DialogResult = DialogResult.Cancel;
                btnNew.PerformClick();
            }
            sQLiteDataAdapter.Dispose();
            int_0 = num2;
            Label label = lblCount;
            int count = dataTable.Rows.Count;
            string text11 = "00";
            label.Text = count.ToString(text11);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message.ToString());
        }
    }

    private void btnSubmit_Click(object sender, EventArgs e)
    {
        int index = int_1;
        string tktname_ = grdUsers.Rows[index].Cells[1].Value.ToString();
        frmProcessTicket existing = frmProcessTicket.FindOpen(tktname_);
        if (existing != null)
        {
            if (!existing.Visible || existing.WindowState == FormWindowState.Minimized)
            {
                existing.Show();
                if (existing.WindowState == FormWindowState.Minimized)
                    existing.WindowState = FormWindowState.Normal;
                existing.BringToFront();
                existing.Activate();
                return;
            }
            MessageBox.Show(Program.TopMost_(), "Ticket Already Open", tktname_, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
        }
        frmProcessTicket.Forget(tktname_);
        StatClass.Tktlist_0.Clear();
        DataGridViewCellCollection cells = grdUsers.Rows[index].Cells;
        string columnName = "Slot";
        string text2 = cells[columnName].Value.ToString();
        string oldValue = "Slot-";
        string text3 = text2.Replace(oldValue, "");
        StatClass.Slot_ = int.Parse(text3);
        DataGridViewCellCollection cells2 = grdUsers.Rows[index].Cells;
        string columnName2 = "ClmA";
        StatClass.appPair = int.Parse(cells2[columnName2].Value.ToString());
        DataGridViewCellCollection cells3 = grdUsers.Rows[index].Cells;
        string columnName3 = "ClmN";
        StatClass.webPair = int.Parse(cells3[columnName3].Value.ToString());
        int num = StatClass.appPair + StatClass.webPair;
        string keyStatus = Program.KeyStatus;
        string text4 = "Paid";
        bool flag = keyStatus == text4;
        string keyType = Program.KeyType;
        string text5 = "demo";
        if (flag & (keyType != text5))
        {
            StatClass.int_MaxPair = Program.paidpair + 1;
        }
        else
        {
            StatClass.int_MaxPair = Program.unpaidpair + 1;
        }
        if (num >= StatClass.int_MaxPair)
        {
            if (StatClass.int_MaxPair != 12)
            {
                Form owner2 = Program.TopMost_();
                string text6 = "You cannot Open more Than ";
                string text7 = (StatClass.int_MaxPair - 1).ToString();
                string text8 = " Pairs at a time.";
                string text9 = text6 + text7 + text8;
                string caption = "Limitation";
                MessageBox.Show(owner2, text9, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                Form owner3 = Program.TopMost_();
                string text10 = "You cannot Open more Than ";
                string text11 = (StatClass.int_MaxPair - 1).ToString();
                string text12 = " Pairs at a time on UnPaid and Demo ID";
                string text13 = text10 + text11 + text12;
                string caption2 = "UnPaid Limitation";
                MessageBox.Show(owner3, text13, caption2, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return;
        }
        string[] array = new string[15]
        {
            grdUsers.Rows[index].Cells[1].Value.ToString(),
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null
        };
        string text14 = "^#^";
        array[1] = text14;
        DataGridViewCellCollection cells4 = grdUsers.Rows[index].Cells;
        string columnName4 = "ClmN";
        array[2] = cells4[columnName4].Value?.ToString();
        string text15 = "^#^";
        array[3] = text15;
        DataGridViewCellCollection cells5 = grdUsers.Rows[index].Cells;
        string columnName5 = "ClmA";
        array[4] = cells5[columnName5].Value?.ToString();
        string text16 = "^#^";
        array[5] = text16;
        array[6] = chkAutoWEB.Checked.ToString();
        string text17 = "^#^";
        array[7] = text17;
        array[8] = cboHours.SelectedItem?.ToString();
        string text18 = ":";
        array[9] = text18;
        array[10] = cboMinutes.SelectedItem?.ToString();
        string text19 = ":";
        array[11] = text19;
        array[12] = cboSeconds.SelectedItem?.ToString();
        string text20 = "^#^";
        array[13] = text20;
        array[14] = text3.ToString();
        string item = string.Concat(array);
        StatClass.Tktlist_0.Add(item);
        base.DialogResult = DialogResult.OK;
        Close();
    }

    private void btnSubmitAll_Click(object sender, EventArgs e)
    {
        StatClass.Tktlist_0.Clear();
        int num = 0;
        foreach (object item2 in (IEnumerable)grdUsers.Rows)
        {
            int index = ((DataGridViewBand)item2).Index;
            string tktname_ = grdUsers.Rows[index].Cells[1].Value.ToString();
            Class28.dictionary_8.Any((KeyValuePair<string, int> tr) => tr.Key.Equals(tktname_, StringComparison.CurrentCultureIgnoreCase));
            DataGridViewCellCollection cells = grdUsers.Rows[index].Cells;
            string columnName = "Slot";
            string text = cells[columnName].Value.ToString();
            string oldValue = "Slot-";
            StatClass.Slot_ = int.Parse(text.Replace(oldValue, ""));
            DataGridViewCellCollection cells2 = grdUsers.Rows[index].Cells;
            string columnName2 = "ClmA";
            StatClass.appPair = int.Parse(cells2[columnName2].Value.ToString());
            DataGridViewCellCollection cells3 = grdUsers.Rows[index].Cells;
            string columnName3 = "ClmN";
            StatClass.webPair = int.Parse(cells3[columnName3].Value.ToString());
            int num2 = StatClass.appPair + StatClass.webPair;
            string keyStatus = Program.KeyStatus;
            string text2 = "Paid";
            bool flag = keyStatus == text2;
            string keyType = Program.KeyType;
            string text3 = "demo";
            if (!(flag & (keyType != text3)))
            {
                StatClass.int_MaxPair = Program.unpaidpair;
            }
            else
            {
                StatClass.int_MaxPair = Program.paidpair;
            }
            if (num < StatClass.int_MaxPair)
            {
                num++;
                DataGridViewCellCollection cells4 = grdUsers.Rows[index].Cells;
                string columnName4 = "ClmA";
                num2 = int.Parse(cells4[columnName4].Value.ToString());
                if (num2 > 1)
                {
                    DataGridViewCellCollection cells5 = grdUsers.Rows[index].Cells;
                    string columnName5 = "ClmN";
                    cells5[columnName5].Value = 0;
                }
                DataGridViewCellCollection cells6 = grdUsers.Rows[index].Cells;
                string columnName6 = "ClmN";
                int num3 = int.Parse(cells6[columnName6].Value.ToString());
                if (num3 > 1)
                {
                    DataGridViewCellCollection cells7 = grdUsers.Rows[index].Cells;
                    string columnName7 = "ClmA";
                    cells7[columnName7].Value = 0;
                }
                if (num3 == 1 && num2 == 1)
                {
                    DataGridViewCellCollection cells8 = grdUsers.Rows[index].Cells;
                    string columnName8 = "ClmA";
                    cells8[columnName8].Value = 0;
                }
                if (num2 < StatClass.int_MaxPair)
                {
                    string[] array = new string[13]
                    {
                        grdUsers.Rows[index].Cells[1].Value.ToString(),
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null,
                        null
                    };
                    string text4 = "^#^";
                    array[1] = text4;
                    DataGridViewCellCollection cells9 = grdUsers.Rows[index].Cells;
                    string columnName9 = "ClmN";
                    array[2] = cells9[columnName9].Value?.ToString();
                    string text5 = "^#^";
                    array[3] = text5;
                    DataGridViewCellCollection cells10 = grdUsers.Rows[index].Cells;
                    string columnName10 = "ClmA";
                    array[4] = cells10[columnName10].Value?.ToString();
                    string text6 = "^#^";
                    array[5] = text6;
                    array[6] = chkAutoWEB.Checked.ToString();
                    string text7 = "^#^";
                    array[7] = text7;
                    array[8] = cboHours.SelectedItem?.ToString();
                    string text8 = ":";
                    array[9] = text8;
                    array[10] = cboMinutes.SelectedItem?.ToString();
                    string text9 = ":";
                    array[11] = text9;
                    array[12] = cboSeconds.SelectedItem?.ToString();
                    string item = string.Concat(array);
                    StatClass.Tktlist_0.Add(item);
                }
            }
        }
        base.DialogResult = DialogResult.OK;
        Close();
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        Form owner = Program.TopMost_();
        string text = "Do you really want to Delete Selected Tickets";
        string caption = "Confirm";
        if (MessageBox.Show(owner, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            method_1(int_1, bool_0: true);
            int_0--;
            lblCount.Text = int_0.ToString();
            if (int_0 == 0)
            {
                SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
                SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
                sQLiteConnection.Open();
                string commandText = "DELETE FROM TBL_TKT; VACUUM; UPDATE SQLITE_SEQUENCE SET SEQ= '0' WHERE NAME='TBL_TKT';";
                sQLiteCommand.CommandText = commandText;
                sQLiteCommand.ExecuteNonQuery();
                string commandText2 = "DELETE FROM TBL_PAX; VACUUM; UPDATE SQLITE_SEQUENCE SET SEQ= '0' WHERE NAME='TBL_PAX';";
                sQLiteCommand.CommandText = commandText2;
                sQLiteCommand.ExecuteNonQuery();
                sQLiteConnection.Close();
                sQLiteCommand.Dispose();
            }
        }
    }

    private void method_1(int int_2, bool bool_0)
    {
        try
        {
            string text = grdUsers.Rows[int_2].Cells[1].Value.ToString();
            SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
            SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
            sQLiteConnection.Open();
            string text2 = "DELETE FROM TBL_PAX WHERE TKT_ID = '";
            string text3 = "'";
            sQLiteCommand.CommandText = text2 + text + text3;
            int num = sQLiteCommand.ExecuteNonQuery();
            string text4 = "DELETE FROM TBL_TKT WHERE TKT_NAME = '";
            string text5 = "'";
            sQLiteCommand.CommandText = text4 + text + text5;
            int num2 = sQLiteCommand.ExecuteNonQuery();
            sQLiteConnection.Close();
            sQLiteCommand.Dispose();
            if (bool_0)
            {
                Form owner = Program.TopMost_();
                string text6 = num2.ToString();
                string text7 = " Ticket with ";
                string text8 = num.ToString();
                string text9 = " Passengers have been Deleted Successfully!";
                string text10 = text6 + text7 + text8 + text9;
                string caption = "Delete";
                MessageBox.Show(owner, text10, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                grdUsers.Rows.RemoveAt(int_2);
            }
        }
        catch
        {
            Form owner2 = Program.TopMost_();
            string text11 = "Error Occured while Deleting the Ticket";
            MessageBox.Show(owner2, text11);
        }
    }

    private void btnDeleteAll_Click(object sender, EventArgs e)
    {
        Form owner = Program.TopMost_();
        string text = "Do you really want to Delete All Saved Tickets";
        string caption = "Confirm";
        if (MessageBox.Show(owner, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        {
            return;
        }
        foreach (DataGridViewBand item in (IEnumerable)grdUsers.Rows)
        {
            method_1(item.Index, bool_0: false);
        }
        int_0 = 0;
        lblCount.Text = int_0.ToString();
        if (int_0 == 0)
        {
            SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
            SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
            sQLiteConnection.Open();
            string commandText = "DELETE FROM TBL_TKT; VACUUM; UPDATE SQLITE_SEQUENCE SET SEQ= '0' WHERE NAME='TBL_TKT';";
            sQLiteCommand.CommandText = commandText;
            sQLiteCommand.ExecuteNonQuery();
            string commandText2 = "DELETE FROM TBL_PAX; VACUUM; UPDATE SQLITE_SEQUENCE SET SEQ= '0' WHERE NAME='TBL_PAX';";
            sQLiteCommand.CommandText = commandText2;
            sQLiteCommand.ExecuteNonQuery();
            sQLiteConnection.Close();
            sQLiteCommand.Dispose();
        }
        Form owner2 = Program.TopMost_();
        string text2 = "All Ticket have been Deleted Successfully!";
        string caption2 = "Delete";
        MessageBox.Show(owner2, text2, caption2, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        grdUsers.DataSource = null;
        grdUsers.Refresh();
        base.DialogResult = DialogResult.Cancel;
        Close();
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
        try
        {
            Class48 class48_ = new Class48(grdUsers.Rows[int_1].Cells[1].Value.ToString());
            frmAddTkt frmAddTkt2 = new frmAddTkt
            {
                dictionary_1 = Program.dictionary_0
            };
            string text = "Edit Ticket";
            frmAddTkt2.Text = text;
            frmAddTkt2.bool_0 = true;
            frmAddTkt2.EditLoad(class48_);
            Dispose();
            frmAddTkt2.ShowDialog();
            frmAddTkt2.Dispose();
        }
        catch
        {
        }
    }

    private void btnNew_Click(object sender, EventArgs e)
    {
        frmAddTkt frmAddTkt2 = new frmAddTkt
        {
            dictionary_1 = Program.dictionary_0
        };
        Dispose();
        frmAddTkt2.ShowDialog();
        frmAddTkt2.Dispose();
    }

    private void radioVPair_CheckedChanged(object sender, EventArgs e)
    {
        StatClass.HPair = false;
        StatClass.VPair = radioVPair.Checked;
    }

    private void C1AA0_CheckedChanged(object sender, EventArgs e)
    {
        StatClass.VPair = false;
        StatClass.HPair = C1AA0.Checked;
    }

    private void grdUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        int rowIndex = e.RowIndex;
        int columnIndex = e.ColumnIndex;
        int_1 = rowIndex;
        switch (columnIndex)
        {
            case 10:
                btnSubmit_Click(null, null);
                break;
            case 11:
                chkAutoWEB.Checked = true;
                btnSubmit_Click(null, null);
                break;
            case 12:
                btnEdit_Click(sender, EventArgs.Empty);
                break;
            case 13:
                btnDelete_Click(sender, EventArgs.Empty);
                break;
        }
    }

    private void cboCredit_SelectedIndexChanged(object sender, EventArgs e)
    {
        int num = int.Parse(cboCredit.SelectedItem.ToString());
        B0AAA = num - (C0AA0 = num / 2);
        for (int i = 0; i < int_0; i++)
        {
            DataGridViewCellCollection cells = grdUsers.Rows[i].Cells;
            string columnName = "ClmN";
            DataGridViewCell dataGridViewCell = cells[columnName];
            DataGridViewCellCollection cells2 = grdUsers.Rows[i].Cells;
            string columnName2 = "ClmN";
            dataGridViewCell.Value = (cells2[columnName2] as DataGridViewComboBoxCell).Items[B0AAA];
            DataGridViewCellCollection cells3 = grdUsers.Rows[i].Cells;
            string columnName3 = "ClmA";
            DataGridViewCell dataGridViewCell2 = cells3[columnName3];
            DataGridViewCellCollection cells4 = grdUsers.Rows[i].Cells;
            string columnName4 = "ClmA";
            dataGridViewCell2.Value = (cells4[columnName4] as DataGridViewComboBoxCell).Items[C0AA0];
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void C1AA0_Click(object sender, EventArgs e)
    {
        if (StatClass.HPair)
        {
            mySettings mySettings_ = Program.mySettings_0;
            string formAlign = "Hpair";
            mySettings_.FormAlign = formAlign;
            if (Program.mySettings_0.NonManagerDashborad)
            {
                mySettings.smethod_2(Program.mySettings_0);
            }
        }
    }

    private void radioVPair_Click(object sender, EventArgs e)
    {
        if (StatClass.VPair)
        {
            mySettings mySettings_ = Program.mySettings_0;
            string formAlign = "Vpair";
            mySettings_.FormAlign = formAlign;
            if (Program.mySettings_0.NonManagerDashborad)
            {
                mySettings.smethod_2(Program.mySettings_0);
            }
        }
    }

    private void chkAuto_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void ChkAutoAPP_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void GrdUsers_CellEnter(object sender, DataGridViewCellEventArgs e)
    {
        bool flag = e.RowIndex != -1 && e.ColumnIndex != -1;
        DataGridView dataGridView = sender as DataGridView;
        if (dataGridView.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn && flag)
        {
            dataGridView.BeginEdit(selectAll: true);
            ((ComboBox)dataGridView.EditingControl).DroppedDown = true;
        }
    }

    private void GrdUsers_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
        grdUsers.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && idisposable_0 != null)
        {
            idisposable_0.Dispose();
        }
        base.Dispose(disposing);
    }


    // ===== Express theme (light) - visual layer only. Ticket / database / click logic above is untouched. =====
    private static readonly Color MT_BG = Color.FromArgb(233, 241, 252);
    private static readonly Color MT_ORANGE = Color.FromArgb(245, 146, 28);
    private static readonly Color MT_BLUE = Color.FromArgb(37, 99, 235);
    private static readonly Color MT_TEXT = Color.FromArgb(30, 41, 59);
    private static readonly Color MT_MUTED = Color.FromArgb(100, 116, 139);
    private static readonly Color MT_LINE = Color.FromArgb(230, 236, 245);
    private static readonly Color MT_BORDER = Color.FromArgb(203, 213, 225);
    private static readonly Color MT_HEAD_L = Color.FromArgb(14, 52, 120);
    private static readonly Color MT_HEAD_R = Color.FromArgb(30, 96, 196);

    private const int MT_W = 980, MT_H = 476, MT_HEAD = 60, MT_M = 14;
    private static readonly string[] MT_HEADS = { "#", "TICKET", "FROM", "TO", "JOURNEY", "QUOTA", "CLASS", "SLOT", "WEB", "APP" };
    private static readonly int[] MT_COLW = { 40, 104, 62, 62, 86, 50, 52, 84, 60, 60, 70, 70, 62, 72 };

    private readonly Font mtFHead = new Font("Segoe UI", 8F, FontStyle.Bold);
    private readonly Font mtFCell = new Font("Segoe UI", 9F);
    private readonly Font mtFSemi = new Font("Segoe UI Semibold", 9F);
    private readonly Font mtFBold = new Font("Segoe UI", 9F, FontStyle.Bold);
    private readonly Font mtFPill = new Font("Segoe UI", 8F, FontStyle.Bold);
    private readonly Font mtFTitle = new Font("Segoe UI Semibold", 13F);
    private readonly Font mtFSub = new Font("Segoe UI", 8.5F);
    private readonly Font mtFBtn = new Font("Segoe UI Semibold", 9.5F);

    private Rectangle mtCard;
    private int mtHotRow = -1, mtHotCol = -1, mtDownRow = -1, mtDownCol = -1;
    private bool mtCloseHot, mtCloseDown;
    private ExpressCloseSticker closeSticker;
    private readonly Dictionary<Button, int> mtBtnState = new Dictionary<Button, int>();

    private void ApplyByteNovaNovaStyle()
    {
        SuspendLayout();

        FormBorderStyle = FormBorderStyle.None;
        DoubleBuffered = true;
        BackColor = MT_BG;
        ForeColor = MT_TEXT;
        Padding = new Padding(0);
        ClientSize = new Size(MT_W, MT_H);

        // ---- header ----
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Height = MT_HEAD;
        pnlHeader.BackColor = MT_HEAD_L;
        MtBuffer(pnlHeader);
        pnlHeader.Paint -= MtHeaderPaint;
        pnlHeader.Paint += MtHeaderPaint;
        B2AAA.Visible = false;              // title is painted
        lblCount.Visible = false;           // count is painted as a pill (Text still updated by the code)
        lblCount.TextChanged -= MtCountChanged;
        lblCount.TextChanged += MtCountChanged;

        btnClose.Zoom = 0;
        btnClose.BackColor = Color.Transparent;
        btnClose.Cursor = Cursors.Hand;
        btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        btnClose.Size = new Size(CB_BOX, CB_BOX);    // round red badge patched on the top-right corner
        pnlHeader.Resize -= MtPlaceClose;
        pnlHeader.Resize += MtPlaceClose;
        MtPlaceClose(null, EventArgs.Empty);
        btnClose.Paint -= MtClosePaint;
        btnClose.Paint += MtClosePaint;
        btnClose.MouseEnter += delegate { mtCloseHot = true; btnClose.Invalidate(); };
        btnClose.MouseLeave += delegate { mtCloseHot = false; mtCloseDown = false; btnClose.Invalidate(); };
        btnClose.MouseDown += delegate { mtCloseDown = true; btnClose.Invalidate(); };
        btnClose.MouseUp += delegate { mtCloseDown = false; if (!btnClose.IsDisposed) btnClose.Invalidate(); };
        btnClose.BringToFront();

        // Close is shown as a "sticker" badge on the window's top-right corner (half outside the popup).
        // The in-header button is only hidden; the sticker raises its own Click (same handlers, same sender).
        btnClose.Visible = false;
        if (closeSticker == null)
            closeSticker = ExpressCloseSticker.Attach(this, delegate { if (!btnClose.IsDisposed) InvokeOnClick(btnClose, EventArgs.Empty); });

        // ---- table card ----
        mtCard = new Rectangle(MT_M, MT_HEAD + 12, MT_W - MT_M * 2, MT_H - MT_HEAD - 12 - 70);
        grdUsers.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        grdUsers.SetBounds(mtCard.X + 8, mtCard.Y + 8, mtCard.Width - 16, mtCard.Height - 16);
        MtBuffer(grdUsers);
        grdUsers.BorderStyle = BorderStyle.None;
        grdUsers.BackgroundColor = Color.White;
        grdUsers.GridColor = MT_LINE;
        grdUsers.CellBorderStyle = DataGridViewCellBorderStyle.None;
        grdUsers.EnableHeadersVisualStyles = false;
        grdUsers.RowHeadersVisible = false;
        grdUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grdUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        grdUsers.ColumnHeadersHeight = 38;
        grdUsers.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        grdUsers.RowTemplate.Height = 42;
        grdUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        grdUsers.ScrollBars = ScrollBars.Vertical;
        grdUsers.SelectionMode = DataGridViewSelectionMode.CellSelect;

        grdUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 252);
        grdUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(51, 65, 85);
        grdUsers.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 245, 252);
        grdUsers.ColumnHeadersDefaultCellStyle.Font = mtFHead;
        grdUsers.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

        grdUsers.DefaultCellStyle.BackColor = Color.White;
        grdUsers.DefaultCellStyle.ForeColor = MT_TEXT;
        grdUsers.DefaultCellStyle.SelectionBackColor = Color.White;
        grdUsers.DefaultCellStyle.SelectionForeColor = MT_TEXT;
        grdUsers.DefaultCellStyle.Font = mtFCell;
        grdUsers.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
        grdUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 251, 255);
        grdUsers.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(249, 251, 255);
        grdUsers.AlternatingRowsDefaultCellStyle.SelectionForeColor = MT_TEXT;

        for (int i = 0; i < grdUsers.Columns.Count; i++)
        {
            DataGridViewColumn col = grdUsers.Columns[i];
            col.AutoSizeMode = (i == grdUsers.Columns.Count - 1)
                ? DataGridViewAutoSizeColumnMode.Fill
                : DataGridViewAutoSizeColumnMode.None;
            col.MinimumWidth = 8;
            if (i < MT_COLW.Length) col.Width = MT_COLW[i];
            col.SortMode = DataGridViewColumnSortMode.NotSortable;

            DataGridViewComboBoxColumn cc = col as DataGridViewComboBoxColumn;
            if (cc != null)
            {
                cc.FlatStyle = FlatStyle.Flat;
                cc.DefaultCellStyle.BackColor = Color.White;
                cc.DefaultCellStyle.ForeColor = MT_TEXT;
                cc.DefaultCellStyle.SelectionBackColor = Color.White;
                cc.DefaultCellStyle.SelectionForeColor = MT_TEXT;
                cc.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
            DataGridViewButtonColumn bc = col as DataGridViewButtonColumn;
            if (bc != null) bc.FlatStyle = FlatStyle.Flat;
        }
        if (grdUsers.Columns.Count > 0) grdUsers.Columns[grdUsers.Columns.Count - 1].MinimumWidth = 60;

        grdUsers.CellPainting -= MtCellPaint;
        grdUsers.CellPainting += MtCellPaint;
        grdUsers.Paint -= MtGridPaint;
        grdUsers.Paint += MtGridPaint;
        grdUsers.CellMouseEnter += MtCellHotEnter;
        grdUsers.CellMouseLeave += MtCellHotLeave;
        grdUsers.MouseLeave += delegate { int r = mtHotRow; mtHotRow = mtHotCol = -1; MtInvRow(r); };
        grdUsers.CellMouseDown += MtCellDown;
        grdUsers.CellMouseUp += MtCellUp;
        grdUsers.CurrentCellChanged += delegate { if (!grdUsers.IsDisposed) grdUsers.Invalidate(); };
        grdUsers.EditingControlShowing += MtEditingShowing;

        // ---- footer buttons ----
        MtFooterButton(btnDeleteAll, new Rectangle(MT_W - MT_M - 184 - 12 - 132, mtCard.Bottom + 17, 132, 36));
        MtFooterButton(btnSubmitAll, new Rectangle(MT_W - MT_M - 184, mtCard.Bottom + 17, 184, 36));

        // Old helper buttons stay hidden (they are called from the table's buttons).
        if (btnSubmit != null) btnSubmit.Visible = false;
        if (btnEdit != null) btnEdit.Visible = false;
        if (btnDelete != null) btnDelete.Visible = false;

        // These were outside the old (small) form; keep them out of sight in the bigger one too.
        // Moved, not hidden, so their existing behaviour (Checked, PerformClick, etc.) stays the same.
        foreach (Control c in new Control[] { label5, chkAutoWEB, chkAutoAPP, grpOpen, panel1, label2 })
            if (c != null) c.Location = new Point(MT_W + 60, c.Top);

        Paint -= MtFormPaint;
        Paint += MtFormPaint;

        ResumeLayout(false);
        PerformLayout();
        Invalidate(true);
    }

    private static void MtBuffer(Control c)
    {
        try
        {
            typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(c, true, null);
        }
        catch { }
    }

    // Keep the close button in the header's top-right corner, whatever the header's real width is.
    private void MtPlaceClose(object sender, EventArgs e)
    {
        if (btnClose == null || btnClose.IsDisposed || pnlHeader == null) return;
        // badge centre sits ~19px in from the top-right corner (patch on the corner), fully inside the header
        int w = pnlHeader.Width > 0 ? pnlHeader.Width : ClientSize.Width;
        btnClose.Size = new Size(CB_BOX, CB_BOX);
        btnClose.Location = new Point(Math.Max(0, w - CB_BOX), CB_TOP);
        btnClose.BringToFront();
    }

    private void MtCountChanged(object sender, EventArgs e)
    {
        if (!pnlHeader.IsDisposed) pnlHeader.Invalidate();
    }

    private void MtFooterButton(Button b, Rectangle r)
    {
        if (b == null) return;
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.FlatAppearance.MouseOverBackColor = MT_BG;
        b.FlatAppearance.MouseDownBackColor = MT_BG;
        b.BackColor = MT_BG;
        b.UseVisualStyleBackColor = false;
        b.Cursor = Cursors.Hand;
        b.Bounds = r;
        b.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        mtBtnState[b] = 0;
        b.Paint -= MtFooterPaint;
        b.Paint += MtFooterPaint;
        b.MouseEnter += delegate { mtBtnState[b] = 1; b.Invalidate(); };
        b.MouseLeave += delegate { mtBtnState[b] = 0; b.Invalidate(); };
        b.MouseDown += delegate (object s, MouseEventArgs e) { if (e.Button == MouseButtons.Left) { mtBtnState[b] = 2; b.Invalidate(); } };
        b.MouseUp += delegate { if (b.IsDisposed) return; mtBtnState[b] = b.ClientRectangle.Contains(b.PointToClient(MousePosition)) ? 1 : 0; b.Invalidate(); };
    }

    // ---------------- painting ----------------

    private static GraphicsPath MtRound(RectangleF r, float rad)
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

    private static void MtFill(Graphics g, RectangleF r, float rad, Color fill, Color border)
    {
        using (GraphicsPath p = MtRound(r, rad))
        {
            using (SolidBrush b = new SolidBrush(fill)) g.FillPath(b, p);
            if (border.A > 0) using (Pen pen = new Pen(border, 1F)) g.DrawPath(pen, p);
        }
    }

    private static Color MtMix(Color a, Color b, float t)
    {
        return Color.FromArgb(
            (int)(a.R + (b.R - a.R) * t),
            (int)(a.G + (b.G - a.G) * t),
            (int)(a.B + (b.B - a.B) * t));
    }

    private void MtFormPaint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(MT_BG);

        // table card with a soft shadow
        RectangleF sh = new RectangleF(mtCard.X, mtCard.Y + 2, mtCard.Width - 1, mtCard.Height - 1);
        MtFill(g, sh, 12, Color.FromArgb(214, 225, 242), Color.Empty);
        MtFill(g, new RectangleF(mtCard.X, mtCard.Y, mtCard.Width - 1, mtCard.Height - 1), 12, Color.White, Color.FromArgb(214, 224, 240));

        // footer help text
        int y = mtCard.Bottom + 17;
        Rectangle ic = new Rectangle(MT_M + 4, y + 3, 14, 14);
        MtIcon(g, "info", ic, MT_BLUE);
        TextRenderer.DrawText(g, "Choose the Slot and how many Web / App pairs to open, then press Open or Login.",
            mtFSub, new Rectangle(ic.Right + 8, y, 560, 20), MT_TEXT,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
        TextRenderer.DrawText(g, "Delete removes the ticket and its passengers permanently.",
            mtFSub, new Rectangle(ic.Right + 8, y + 18, 560, 18), MT_MUTED,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);

        using (Pen p = new Pen(Color.FromArgb(178, 196, 226), 1F))
            g.DrawRectangle(p, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
    }

    private void MtHeaderPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null || c.Width < 4 || c.Height < 4) return;
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle r = new Rectangle(0, 0, c.Width, c.Height);
        using (LinearGradientBrush bg = new LinearGradientBrush(r, MT_HEAD_L, MT_HEAD_R, LinearGradientMode.Horizontal))
            g.FillRectangle(bg, r);
        using (SolidBrush o = new SolidBrush(MT_ORANGE))
            g.FillRectangle(o, 0, c.Height - 3, c.Width, 3);

        int h = c.Height - 3;
        Rectangle ib = new Rectangle(MT_M + 2, (h - 38) / 2, 38, 38);
        MtFill(g, ib, 10, Color.FromArgb(40, 255, 255, 255), Color.FromArgb(70, 255, 255, 255));
        MtIcon(g, "ticket", new Rectangle(ib.X + 9, ib.Y + 11, 20, 16), MT_ORANGE);

        int tx = ib.Right + 12;
        string title = "Saved Tickets";
        Size ts = TextRenderer.MeasureText(g, title, mtFTitle, Size.Empty, TextFormatFlags.NoPadding);
        TextRenderer.DrawText(g, title, mtFTitle, new Point(tx, ib.Y - 1), Color.White, TextFormatFlags.NoPadding);

        string cnt = (lblCount != null ? lblCount.Text : "0").Trim();
        int n;
        string pill = cnt + (int.TryParse(cnt, out n) && n == 1 ? " Ticket" : " Tickets");
        Size ps = TextRenderer.MeasureText(g, pill, mtFPill, Size.Empty, TextFormatFlags.NoPadding);
        Rectangle pr = new Rectangle(tx + ts.Width + 10, ib.Y + 2, ps.Width + 18, 20);
        MtFill(g, pr, 10, MT_ORANGE, Color.Empty);
        TextRenderer.DrawText(g, pill, mtFPill, pr, Color.White,
            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

        TextRenderer.DrawText(g, "Open, login, edit or delete any saved ticket", mtFSub,
            new Point(tx + 1, ib.Y + 23), Color.FromArgb(205, 220, 247), TextFormatFlags.NoPadding);
    }

    private void MtClosePaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // repaint the header gradient underneath so the original image never shows
        Control p = c.Parent;
        int pw = p != null ? Math.Max(1, p.Width) : c.Width;
        int ph = p != null ? Math.Max(1, p.Height) : c.Height;
        using (LinearGradientBrush bg = new LinearGradientBrush(
            new Rectangle(-c.Left, -c.Top, pw, ph), MT_HEAD_L, MT_HEAD_R, LinearGradientMode.Horizontal))
            g.FillRectangle(bg, 0, 0, c.Width, c.Height);

        CbDrawCloseBadge(g, mtCloseHot, mtCloseDown);
    }

    // ---- round red close badge, patched on the popup's top-right corner ----
    // Control is CB_BOX square; circle (30px, 32px on hover) centred at (CB_C, CB_C) leaves room for the shadow.
    private const int CB_BOX = 36, CB_TOP = 2;
    private const float CB_C = 17f;

    private static void CbDrawCloseBadge(Graphics g, bool hot, bool down)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        float R = hot && !down ? 16f : 15f;
        float cx = CB_C, cy = CB_C + (down ? 1f : 0f);

        if (!down)
        {
            using (SolidBrush s1 = new SolidBrush(Color.FromArgb(28, 0, 0, 0)))
                g.FillEllipse(s1, cx - R - 1.5f, cy - R + 0.5f, (R + 1.5f) * 2, (R + 1.5f) * 2);
            using (SolidBrush s2 = new SolidBrush(Color.FromArgb(55, 60, 0, 0)))
                g.FillEllipse(s2, cx - R, cy - R + 1.6f, R * 2, R * 2);
        }

        RectangleF disc = new RectangleF(cx - R, cy - R, R * 2, R * 2);
        Color ring = hot && !down ? Color.FromArgb(220, 38, 38) : Color.White;
        Color glyph = hot && !down ? Color.FromArgb(220, 38, 38) : Color.White;
        if (down)
            using (SolidBrush b = new SolidBrush(Color.FromArgb(153, 27, 27))) g.FillEllipse(b, disc);
        else if (hot)
            using (SolidBrush b = new SolidBrush(Color.White)) g.FillEllipse(b, disc);
        else
            using (LinearGradientBrush b = new LinearGradientBrush(disc, Color.FromArgb(239, 68, 68), Color.FromArgb(220, 38, 38), LinearGradientMode.Vertical))
                g.FillEllipse(b, disc);

        using (Pen p = new Pen(ring, 2f))
            g.DrawEllipse(p, cx - R + 1f, cy - R + 1f, R * 2 - 2f, R * 2 - 2f);

        float a = R * 0.34f;
        using (Pen x = new Pen(glyph, 2.6f))
        {
            x.StartCap = LineCap.Round;
            x.EndCap = LineCap.Round;
            g.DrawLine(x, cx - a, cy - a, cx + a, cy + a);
            g.DrawLine(x, cx + a, cy - a, cx - a, cy + a);
        }
    }

    private void MtFooterPaint(object sender, PaintEventArgs e)
    {
        Button b = sender as Button;
        if (b == null) return;
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(MT_BG);

        int st;
        if (!mtBtnState.TryGetValue(b, out st)) st = 0;
        bool primary = b == btnSubmitAll;
        RectangleF r = new RectangleF(0.5f, st == 2 ? 1.5f : 0.5f, b.Width - 2, b.Height - 3);

        Color fg;
        string icon, text;
        if (primary)
        {
            Color top = Color.FromArgb(251, 166, 60), bot = Color.FromArgb(234, 124, 10);
            if (st == 1) { top = MtMix(top, Color.White, 0.12f); bot = MtMix(bot, Color.White, 0.08f); }
            if (st == 2) { top = MtMix(top, Color.Black, 0.10f); bot = MtMix(bot, Color.Black, 0.12f); }
            if (st != 2) MtFill(g, new RectangleF(r.X, r.Y + 2, r.Width, r.Height), 8, Color.FromArgb(60, 194, 100, 8), Color.Empty);
            using (GraphicsPath p = MtRound(r, 8))
            using (LinearGradientBrush lb = new LinearGradientBrush(r, top, bot, LinearGradientMode.Vertical))
            {
                g.FillPath(lb, p);
                using (Pen pen = new Pen(Color.FromArgb(214, 110, 6))) g.DrawPath(pen, p);
            }
            fg = Color.White; icon = "playall"; text = "Open All Tickets";
        }
        else
        {
            Color fill = st == 2 ? Color.FromArgb(254, 226, 226) : st == 1 ? Color.FromArgb(254, 242, 242) : Color.White;
            MtFill(g, r, 8, fill, st == 0 ? Color.FromArgb(252, 165, 165) : Color.FromArgb(239, 68, 68));
            fg = Color.FromArgb(220, 38, 38); icon = "trash"; text = "Delete All";
        }

        Size ts = TextRenderer.MeasureText(g, text, mtFBtn, Size.Empty, TextFormatFlags.NoPadding);
        int gw = 14 + 8 + ts.Width;
        int x = (int)(r.X + (r.Width - gw) / 2);
        int cy = (int)(r.Y + r.Height / 2);
        MtIcon(g, icon, new Rectangle(x, cy - 7, 14, 14), fg);
        TextRenderer.DrawText(g, text, mtFBtn, new Rectangle(x + 22, cy - ts.Height / 2 - 1, ts.Width + 4, ts.Height + 2), fg,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }

    private void MtGridPaint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // one "ACTIONS" caption over the four button columns
        if (grdUsers.ColumnCount > 13 && grdUsers.ColumnHeadersVisible)
        {
            Rectangle a = grdUsers.GetColumnDisplayRectangle(10, false);
            Rectangle b = grdUsers.GetColumnDisplayRectangle(13, false);
            if (a.Width > 0 && b.Width > 0)
            {
                Rectangle hr = new Rectangle(a.X, 0, b.Right - a.X, grdUsers.ColumnHeadersHeight - 2);
                TextRenderer.DrawText(g, "ACTIONS", mtFHead, hr, Color.FromArgb(51, 65, 85),
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
            }
        }

        if (grdUsers.Rows.Count == 0)
        {
            int top = grdUsers.ColumnHeadersHeight;
            Rectangle body = new Rectangle(0, top, grdUsers.Width, Math.Max(10, grdUsers.Height - top));
            Rectangle ic = new Rectangle(body.X + body.Width / 2 - 16, body.Y + body.Height / 2 - 34, 32, 26);
            MtIcon(g, "ticket", ic, Color.FromArgb(148, 163, 184));
            TextRenderer.DrawText(g, "No saved tickets", mtFBold,
                new Rectangle(body.X, ic.Bottom + 8, body.Width, 20), MT_TEXT,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPadding);
            TextRenderer.DrawText(g, "Saved tickets will appear here", mtFSub,
                new Rectangle(body.X, ic.Bottom + 28, body.Width, 18), MT_MUTED,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPadding);
        }
    }

    private void MtCellPaint(object sender, DataGridViewCellPaintingEventArgs e)
    {
        if (e.ColumnIndex < 0) return;
        Graphics g = e.Graphics;
        Rectangle cb = e.CellBounds;
        if (cb.Width < 2 || cb.Height < 2) return;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        int col = e.ColumnIndex, row = e.RowIndex;
        const TextFormatFlags BASE = TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
                                     TextFormatFlags.NoPadding | TextFormatFlags.SingleLine;

        // ---- column header ----
        if (row < 0)
        {
            using (LinearGradientBrush bg = new LinearGradientBrush(cb, Color.FromArgb(247, 250, 255),
                Color.FromArgb(233, 240, 251), LinearGradientMode.Vertical))
                g.FillRectangle(bg, cb);
            using (SolidBrush o = new SolidBrush(MT_ORANGE))
                g.FillRectangle(o, cb.X, cb.Bottom - 2, cb.Width, 2);
            if (col < MT_HEADS.Length)
            {
                Rectangle tr = col == 1
                    ? new Rectangle(cb.X + 10, cb.Y, cb.Width - 12, cb.Height - 2)
                    : new Rectangle(cb.X, cb.Y, cb.Width, cb.Height - 2);
                TextRenderer.DrawText(g, MT_HEADS[col], mtFHead, tr, Color.FromArgb(51, 65, 85),
                    BASE | (col == 1 ? TextFormatFlags.Left : TextFormatFlags.HorizontalCenter));
            }
            e.Handled = true;
            return;
        }
        if (row >= grdUsers.RowCount) return;

        // ---- row background ----
        bool cur = grdUsers.CurrentCell != null && grdUsers.CurrentCell.RowIndex == row;
        bool hot = row == mtHotRow;
        Color bgc = cur ? Color.FromArgb(255, 247, 237)
                  : hot ? Color.FromArgb(240, 246, 255)
                  : (row % 2 == 1 ? Color.FromArgb(249, 251, 255) : Color.White);
        using (SolidBrush b = new SolidBrush(bgc)) g.FillRectangle(b, cb);
        using (Pen ln = new Pen(MT_LINE)) g.DrawLine(ln, cb.X, cb.Bottom - 1, cb.Right, cb.Bottom - 1);
        if (cur && col == 0)
            using (SolidBrush o = new SolidBrush(MT_ORANGE)) g.FillRectangle(o, cb.X, cb.Y, 3, cb.Height - 1);

        string v = Convert.ToString(e.FormattedValue);
        int midY = cb.Y + cb.Height / 2;

        switch (col)
        {
            case 0: // serial number badge
            {
                Rectangle c = new Rectangle(cb.X + (cb.Width - 24) / 2, midY - 12, 24, 24);
                using (SolidBrush b = new SolidBrush(cur ? MT_ORANGE : Color.FromArgb(224, 234, 252)))
                    g.FillEllipse(b, c);
                TextRenderer.DrawText(g, v, mtFPill, c, cur ? Color.White : Color.FromArgb(29, 78, 216),
                    BASE | TextFormatFlags.HorizontalCenter);
                break;
            }
            case 1: // ticket name
                TextRenderer.DrawText(g, v, mtFBold, new Rectangle(cb.X + 10, cb.Y, cb.Width - 14, cb.Height), MT_TEXT,
                    BASE | TextFormatFlags.Left);
                break;
            case 2:
            case 3: // stations
                TextRenderer.DrawText(g, v, mtFSemi, new Rectangle(cb.X + 4, cb.Y, cb.Width - 12, cb.Height),
                    Color.FromArgb(30, 64, 175), BASE | TextFormatFlags.HorizontalCenter);
                if (col == 2) MtIcon(g, "arrow", new Rectangle(cb.Right - 10, midY - 4, 8, 8), Color.FromArgb(148, 163, 184));
                break;
            case 4: // journey date
                MtIcon(g, "calendar", new Rectangle(cb.X + 6, midY - 6, 12, 12), MT_ORANGE);
                TextRenderer.DrawText(g, v, mtFCell, new Rectangle(cb.X + 21, cb.Y, cb.Width - 22, cb.Height), MT_TEXT,
                    BASE | TextFormatFlags.Left);
                break;
            case 5: // quota
            case 6: // class
            {
                Color pf, pt;
                string q = v.Trim().ToUpperInvariant();
                if (col == 6) { pf = Color.FromArgb(224, 231, 255); pt = Color.FromArgb(55, 48, 163); }
                else if (q == "TQ" || q == "PT" || q.Contains("TATKAL")) { pf = Color.FromArgb(255, 237, 213); pt = Color.FromArgb(194, 65, 12); }
                else if (q == "GN" || q.Contains("GENERAL")) { pf = Color.FromArgb(219, 234, 254); pt = Color.FromArgb(29, 78, 216); }
                else { pf = Color.FromArgb(226, 232, 240); pt = Color.FromArgb(51, 65, 85); }
                if (v.Length == 0) break;
                Size ts = TextRenderer.MeasureText(g, v, mtFPill, Size.Empty, TextFormatFlags.NoPadding);
                int w = Math.Min(cb.Width - 8, ts.Width + 16);
                Rectangle pr = new Rectangle(cb.X + (cb.Width - w) / 2, midY - 11, w, 22);
                MtFill(g, pr, 11, pf, Color.Empty);
                TextRenderer.DrawText(g, v, mtFPill, pr, pt, BASE | TextFormatFlags.HorizontalCenter);
                break;
            }
            case 7:
            case 8:
            case 9: // slot / web / app pickers
            {
                bool hc = hot && mtHotCol == col;
                Rectangle br = new Rectangle(cb.X + 4, midY - 13, cb.Width - 9, 26);
                MtFill(g, br, 6, Color.White, hc ? MT_BLUE : MT_BORDER);
                int tx = br.X + 8;
                if (col != 7)
                {
                    MtIcon(g, col == 8 ? "globe" : "phone", new Rectangle(br.X + 6, midY - 5, 10, 10),
                        col == 8 ? MT_BLUE : MT_ORANGE);
                    tx = br.X + 20;
                }
                Color tc = col == 7 ? Color.FromArgb(29, 78, 216) : (v == "0" ? Color.FromArgb(148, 163, 184) : MT_TEXT);
                TextRenderer.DrawText(g, v, col == 7 ? mtFSemi : mtFBold,
                    new Rectangle(tx, br.Y, Math.Max(4, br.Right - 14 - tx), br.Height), tc, BASE | TextFormatFlags.Left);
                MtIcon(g, "chevron", new Rectangle(br.Right - 12, midY - 3, 8, 6), hc ? MT_BLUE : MT_MUTED);
                break;
            }
            default: // action buttons
                if (col >= 10 && col <= 13) MtActionPaint(g, cb, row, col);
                break;
        }
        e.Handled = true;
    }

    private void MtActionPaint(Graphics g, Rectangle cb, int row, int col)
    {
        bool hot = row == mtHotRow && col == mtHotCol;
        bool down = row == mtDownRow && col == mtDownCol;
        string text, icon;
        Color fill, border, fg;
        switch (col)
        {
            case 10:
                text = "Open"; icon = "play"; fg = Color.White;
                fill = MT_BLUE; border = Color.FromArgb(29, 78, 216);
                break;
            case 11:
                text = "Login"; icon = "login"; fg = Color.White;
                fill = MT_ORANGE; border = Color.FromArgb(221, 120, 8);
                break;
            case 12:
                text = "Edit"; icon = "edit"; fg = Color.FromArgb(29, 78, 216);
                fill = Color.White; border = Color.FromArgb(147, 197, 253);
                break;
            default:
                text = "Delete"; icon = "trash"; fg = Color.FromArgb(220, 38, 38);
                fill = Color.FromArgb(254, 242, 242); border = Color.FromArgb(252, 165, 165);
                break;
        }
        bool solid = col <= 11;
        if (down) { fill = solid ? MtMix(fill, Color.Black, 0.15f) : MtMix(fill, border, 0.45f); }
        else if (hot) { fill = solid ? MtMix(fill, Color.White, 0.14f) : MtMix(fill, border, 0.22f); border = solid ? border : fg; }

        RectangleF r = new RectangleF(cb.X + 4, cb.Y + 7 + (down ? 1 : 0), cb.Width - 9, cb.Height - 15);
        MtFill(g, r, 7, fill, border);

        Size ts = TextRenderer.MeasureText(g, text, mtFPill, Size.Empty, TextFormatFlags.NoPadding);
        int gw = 11 + 5 + ts.Width;
        bool withIcon = gw <= r.Width - 6;
        if (!withIcon) gw = ts.Width;
        int x = (int)(r.X + (r.Width - gw) / 2);
        int cy = (int)(r.Y + r.Height / 2);
        if (withIcon)
        {
            MtIcon(g, icon, new Rectangle(x, cy - 5, 11, 11), fg);
            x += 16;
        }
        TextRenderer.DrawText(g, text, mtFPill, new Rectangle(x, cy - ts.Height / 2 - 1, ts.Width + 4, ts.Height + 2), fg,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }

    private static void MtIcon(Graphics g, string kind, Rectangle r, Color c)
    {
        SmoothingMode old = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        float x = r.X, y = r.Y, w = r.Width, h = r.Height;
        using (Pen p = new Pen(c, 1.6F))
        using (SolidBrush b = new SolidBrush(c))
        {
            p.StartCap = LineCap.Round;
            p.EndCap = LineCap.Round;
            p.LineJoin = LineJoin.Round;
            switch (kind)
            {
                case "play":
                    g.FillPolygon(b, new[] { new PointF(x + w * 0.2f, y), new PointF(x + w, y + h / 2), new PointF(x + w * 0.2f, y + h) });
                    break;
                case "playall":
                    g.FillPolygon(b, new[] { new PointF(x, y + 1), new PointF(x + w * 0.55f, y + h / 2), new PointF(x, y + h - 1) });
                    g.FillPolygon(b, new[] { new PointF(x + w * 0.45f, y + 1), new PointF(x + w, y + h / 2), new PointF(x + w * 0.45f, y + h - 1) });
                    break;
                case "login":
                    g.DrawLines(p, new[] { new PointF(x + w * 0.55f, y), new PointF(x + w, y), new PointF(x + w, y + h), new PointF(x + w * 0.55f, y + h) });
                    g.DrawLine(p, x, y + h / 2, x + w * 0.68f, y + h / 2);
                    g.DrawLines(p, new[] { new PointF(x + w * 0.42f, y + h * 0.24f), new PointF(x + w * 0.68f, y + h / 2), new PointF(x + w * 0.42f, y + h * 0.76f) });
                    break;
                case "edit":
                    using (Pen t = new Pen(c, 2.4F))
                        g.DrawLine(t, x + w * 0.22f, y + h * 0.78f, x + w * 0.9f, y + h * 0.1f);
                    g.FillPolygon(b, new[] { new PointF(x + w * 0.05f, y + h * 0.95f), new PointF(x + w * 0.12f, y + h * 0.62f), new PointF(x + w * 0.38f, y + h * 0.88f) });
                    break;
                case "trash":
                    g.DrawLine(p, x, y + h * 0.2f, x + w, y + h * 0.2f);
                    g.DrawLine(p, x + w * 0.35f, y, x + w * 0.65f, y);
                    g.DrawPolygon(p, new[] { new PointF(x + w * 0.14f, y + h * 0.3f), new PointF(x + w * 0.86f, y + h * 0.3f), new PointF(x + w * 0.76f, y + h), new PointF(x + w * 0.24f, y + h) });
                    using (Pen t = new Pen(c, 1.1F))
                    {
                        g.DrawLine(t, x + w * 0.42f, y + h * 0.45f, x + w * 0.42f, y + h * 0.85f);
                        g.DrawLine(t, x + w * 0.58f, y + h * 0.45f, x + w * 0.58f, y + h * 0.85f);
                    }
                    break;
                case "ticket":
                {
                    using (GraphicsPath tp = MtRound(new RectangleF(x, y, w, h), 3))
                    using (Pen t = new Pen(c, 1.8F))
                        g.DrawPath(t, tp);
                    float nr = h * 0.18f;
                    g.DrawArc(p, x - nr, y + h / 2 - nr, nr * 2, nr * 2, -90, 180);
                    g.DrawArc(p, x + w - nr, y + h / 2 - nr, nr * 2, nr * 2, 90, 180);
                    using (Pen d = new Pen(c, 1.2F))
                    {
                        d.DashStyle = DashStyle.Dash;
                        g.DrawLine(d, x + w * 0.66f, y + 2, x + w * 0.66f, y + h - 2);
                    }
                    g.DrawLine(p, x + w * 0.2f, y + h * 0.38f, x + w * 0.5f, y + h * 0.38f);
                    g.DrawLine(p, x + w * 0.2f, y + h * 0.64f, x + w * 0.42f, y + h * 0.64f);
                    break;
                }
                case "calendar":
                    using (GraphicsPath cp = MtRound(new RectangleF(x, y + 1.5f, w, h - 1.5f), 2))
                        g.DrawPath(p, cp);
                    g.DrawLine(p, x, y + h * 0.42f, x + w, y + h * 0.42f);
                    g.DrawLine(p, x + w * 0.3f, y, x + w * 0.3f, y + 3);
                    g.DrawLine(p, x + w * 0.7f, y, x + w * 0.7f, y + 3);
                    break;
                case "globe":
                    using (Pen t = new Pen(c, 1.2F))
                    {
                        g.DrawEllipse(t, x, y, w, h);
                        g.DrawEllipse(t, x + w * 0.28f, y, w * 0.44f, h);
                        g.DrawLine(t, x, y + h / 2, x + w, y + h / 2);
                    }
                    break;
                case "phone":
                    using (Pen t = new Pen(c, 1.3F))
                    using (GraphicsPath pp = MtRound(new RectangleF(x + w * 0.2f, y, w * 0.6f, h), 1.5f))
                        g.DrawPath(t, pp);
                    g.FillEllipse(b, x + w / 2 - 1, y + h - 3, 2, 2);
                    break;
                case "arrow":
                    g.DrawLines(p, new[] { new PointF(x + w * 0.3f, y), new PointF(x + w * 0.8f, y + h / 2), new PointF(x + w * 0.3f, y + h) });
                    break;
                case "chevron":
                    g.DrawLines(p, new[] { new PointF(x, y), new PointF(x + w / 2, y + h), new PointF(x + w, y) });
                    break;
                case "info":
                    g.FillEllipse(b, x, y, w, h);
                    using (Pen t = new Pen(Color.White, 1.8F))
                    {
                        t.StartCap = LineCap.Round; t.EndCap = LineCap.Round;
                        g.DrawLine(t, x + w / 2, y + h * 0.45f, x + w / 2, y + h * 0.75f);
                    }
                    using (SolidBrush wb = new SolidBrush(Color.White))
                        g.FillEllipse(wb, x + w / 2 - 1.1f, y + h * 0.2f, 2.2f, 2.2f);
                    break;
            }
        }
        g.SmoothingMode = old;
    }

    // ---------------- hover / press ----------------

    private void MtInvRow(int r)
    {
        if (grdUsers == null || grdUsers.IsDisposed) return;
        if (r >= 0 && r < grdUsers.RowCount) grdUsers.InvalidateRow(r);
    }

    private void MtCellHotEnter(object sender, DataGridViewCellEventArgs e)
    {
        int old = mtHotRow;
        mtHotRow = e.RowIndex;
        mtHotCol = e.ColumnIndex;
        if (old != mtHotRow) MtInvRow(old);
        MtInvRow(mtHotRow);
        grdUsers.Cursor = (e.RowIndex >= 0 && e.ColumnIndex >= 7) ? Cursors.Hand : Cursors.Default;
    }

    private void MtCellHotLeave(object sender, DataGridViewCellEventArgs e)
    {
        int old = mtHotRow;
        mtHotRow = mtHotCol = -1;
        mtDownRow = mtDownCol = -1;
        MtInvRow(old);
    }

    private void MtCellDown(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.Button != MouseButtons.Left || e.RowIndex < 0 || e.ColumnIndex < 10 || e.ColumnIndex > 13) return;
        mtDownRow = e.RowIndex;
        mtDownCol = e.ColumnIndex;
        MtInvRow(mtDownRow);
    }

    private void MtCellUp(object sender, DataGridViewCellMouseEventArgs e)
    {
        int old = mtDownRow;
        mtDownRow = mtDownCol = -1;
        MtInvRow(old);
    }

    private void MtEditingShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
        ComboBox cb = e.Control as ComboBox;
        if (cb == null) return;
        cb.FlatStyle = FlatStyle.Flat;
        cb.BackColor = Color.White;
        cb.ForeColor = MT_TEXT;
        cb.Font = mtFCell;
    }

    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManageTicket));
            this.panel1 = new System.Windows.Forms.Panel();
            this.grdUsers = new System.Windows.Forms.DataGridView();
            this.clmSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmNm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmFrm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmDoj = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmQt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clmcl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Slot = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ClmN = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ClmA = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.clmAvtive = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Login = new System.Windows.Forms.DataGridViewButtonColumn();
            this.clmDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.ClmHis = new System.Windows.Forms.DataGridViewButtonColumn();
            this.cboCredit = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.lblDott = new System.Windows.Forms.Label();
            this.cboHours = new System.Windows.Forms.ComboBox();
            this.chkAutoWEB = new System.Windows.Forms.CheckBox();
            this.cboSeconds = new System.Windows.Forms.ComboBox();
            this.cboMinutes = new System.Windows.Forms.ComboBox();
            this.lblDot = new System.Windows.Forms.Label();
            this.radioVPair = new System.Windows.Forms.RadioButton();
            this.C1AA0 = new System.Windows.Forms.RadioButton();
            this.btnSubmitAll = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.grpOpen = new System.Windows.Forms.GroupBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.backgroundWorker_0 = new System.ComponentModel.BackgroundWorker();
            this.C2AA0 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.chkAutoAPP = new System.Windows.Forms.CheckBox();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.lblCount = new System.Windows.Forms.Label();
            this.B2AAA = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdUsers)).BeginInit();
            this.grpOpen.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gray;
            this.panel1.Location = new System.Drawing.Point(1050, 135);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(38, 147);
            this.panel1.TabIndex = 0;
            // 
            // grdUsers
            // 
            this.grdUsers.AllowUserToAddRows = false;
            this.grdUsers.AllowUserToDeleteRows = false;
            this.grdUsers.AllowUserToResizeColumns = false;
            this.grdUsers.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Aqua;
            this.grdUsers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grdUsers.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.DarkTurquoise;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Corbel", 10.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.DarkTurquoise;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grdUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmSN,
            this.clmNm,
            this.clmFrm,
            this.clmTo,
            this.clmDoj,
            this.clmQt,
            this.clmcl,
            this.Slot,
            this.ClmN,
            this.ClmA,
            this.clmAvtive,
            this.Login,
            this.clmDelete,
            this.ClmHis});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.Azure;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdUsers.DefaultCellStyle = dataGridViewCellStyle7;
            this.grdUsers.Location = new System.Drawing.Point(2, 22);
            this.grdUsers.MultiSelect = false;
            this.grdUsers.Name = "grdUsers";
            this.grdUsers.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.grdUsers.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FloralWhite;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.InfoText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdUsers.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.grdUsers.RowHeadersVisible = false;
            this.grdUsers.RowHeadersWidth = 66;
            this.grdUsers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.grdUsers.Size = new System.Drawing.Size(725, 220);
            this.grdUsers.TabIndex = 1214;
            this.grdUsers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdUsers_CellContentClick);
            this.grdUsers.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.GrdUsers_CellEnter);
            this.grdUsers.CurrentCellDirtyStateChanged += new System.EventHandler(this.GrdUsers_CurrentCellDirtyStateChanged);
            // 
            // clmSN
            // 
            this.clmSN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.clmSN.DataPropertyName = "0";
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            this.clmSN.DefaultCellStyle = dataGridViewCellStyle3;
            this.clmSN.FillWeight = 10F;
            this.clmSN.Frozen = true;
            this.clmSN.HeaderText = "S/n";
            this.clmSN.MaxInputLength = 20;
            this.clmSN.MinimumWidth = 8;
            this.clmSN.Name = "clmSN";
            this.clmSN.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.clmSN.Width = 11;
            // 
            // clmNm
            // 
            this.clmNm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.clmNm.DataPropertyName = "1";
            this.clmNm.FillWeight = 70F;
            this.clmNm.Frozen = true;
            this.clmNm.HeaderText = "Name";
            this.clmNm.MinimumWidth = 8;
            this.clmNm.Name = "clmNm";
            this.clmNm.Width = 77;
            // 
            // clmFrm
            // 
            this.clmFrm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.clmFrm.DataPropertyName = "2";
            this.clmFrm.FillWeight = 40F;
            this.clmFrm.Frozen = true;
            this.clmFrm.HeaderText = "From";
            this.clmFrm.MinimumWidth = 8;
            this.clmFrm.Name = "clmFrm";
            this.clmFrm.Width = 44;
            // 
            // clmTo
            // 
            this.clmTo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.clmTo.DataPropertyName = "3";
            this.clmTo.FillWeight = 40F;
            this.clmTo.Frozen = true;
            this.clmTo.HeaderText = "To";
            this.clmTo.MinimumWidth = 8;
            this.clmTo.Name = "clmTo";
            this.clmTo.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.clmTo.Width = 45;
            // 
            // clmDoj
            // 
            this.clmDoj.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.clmDoj.DataPropertyName = "4";
            this.clmDoj.FillWeight = 70F;
            this.clmDoj.Frozen = true;
            this.clmDoj.HeaderText = "Date";
            this.clmDoj.MinimumWidth = 8;
            this.clmDoj.Name = "clmDoj";
            this.clmDoj.Width = 77;
            // 
            // clmQt
            // 
            this.clmQt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.clmQt.DataPropertyName = "5";
            this.clmQt.FillWeight = 30F;
            this.clmQt.HeaderText = "QT";
            this.clmQt.MinimumWidth = 8;
            this.clmQt.Name = "clmQt";
            this.clmQt.Width = 33;
            // 
            // clmcl
            // 
            this.clmcl.DataPropertyName = "6";
            this.clmcl.FillWeight = 40F;
            this.clmcl.HeaderText = "CLS";
            this.clmcl.MinimumWidth = 40;
            this.clmcl.Name = "clmcl";
            // 
            // Slot
            // 
            this.Slot.DataPropertyName = "7";
            this.Slot.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.Slot.FillWeight = 50F;
            this.Slot.HeaderText = "SLOT";
            this.Slot.MaxDropDownItems = 20;
            this.Slot.MinimumWidth = 60;
            this.Slot.Name = "Slot";
            // 
            // ClmN
            // 
            this.ClmN.DataPropertyName = "8";
            this.ClmN.FillWeight = 30F;
            this.ClmN.HeaderText = "Web";
            this.ClmN.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.ClmN.MaxDropDownItems = 21;
            this.ClmN.MinimumWidth = 40;
            this.ClmN.Name = "ClmN";
            this.ClmN.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ClmA
            // 
            this.ClmA.DataPropertyName = "9";
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.Turquoise;
            this.ClmA.DefaultCellStyle = dataGridViewCellStyle4;
            this.ClmA.FillWeight = 30F;
            this.ClmA.HeaderText = "App";
            this.ClmA.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20"});
            this.ClmA.MaxDropDownItems = 21;
            this.ClmA.MinimumWidth = 40;
            this.ClmA.Name = "ClmA";
            this.ClmA.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // clmAvtive
            // 
            this.clmAvtive.DataPropertyName = "10";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White;
            this.clmAvtive.DefaultCellStyle = dataGridViewCellStyle5;
            this.clmAvtive.FillWeight = 60F;
            this.clmAvtive.HeaderText = "";
            this.clmAvtive.MinimumWidth = 8;
            this.clmAvtive.Name = "clmAvtive";
            this.clmAvtive.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Login
            // 
            this.Login.DataPropertyName = "11";
            this.Login.FillWeight = 60F;
            this.Login.HeaderText = "";
            this.Login.MinimumWidth = 8;
            this.Login.Name = "Login";
            // 
            // clmDelete
            // 
            this.clmDelete.DataPropertyName = "12";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.White;
            this.clmDelete.DefaultCellStyle = dataGridViewCellStyle6;
            this.clmDelete.FillWeight = 60F;
            this.clmDelete.HeaderText = "";
            this.clmDelete.MinimumWidth = 8;
            this.clmDelete.Name = "clmDelete";
            this.clmDelete.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ClmHis
            // 
            this.ClmHis.DataPropertyName = "13";
            this.ClmHis.FillWeight = 60F;
            this.ClmHis.HeaderText = "";
            this.ClmHis.MinimumWidth = 8;
            this.ClmHis.Name = "ClmHis";
            this.ClmHis.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.ClmHis.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // cboCredit
            // 
            this.cboCredit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCredit.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboCredit.FormattingEnabled = true;
            this.cboCredit.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30"});
            this.cboCredit.Location = new System.Drawing.Point(937, 55);
            this.cboCredit.Name = "cboCredit";
            this.cboCredit.Size = new System.Drawing.Size(40, 23);
            this.cboCredit.TabIndex = 11;
            this.cboCredit.Visible = false;
            this.cboCredit.SelectedIndexChanged += new System.EventHandler(this.cboCredit_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(872, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 19);
            this.label2.TabIndex = 10;
            this.label2.Text = "Tickets : ";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Georgia", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.Color.Black;
            this.label34.Location = new System.Drawing.Point(856, 59);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(74, 16);
            this.label34.TabIndex = 10;
            this.label34.Text = "Set Login";
            this.label34.Visible = false;
            // 
            // lblDott
            // 
            this.lblDott.AutoSize = true;
            this.lblDott.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDott.Location = new System.Drawing.Point(895, 89);
            this.lblDott.Name = "lblDott";
            this.lblDott.Size = new System.Drawing.Size(15, 23);
            this.lblDott.TabIndex = 1213;
            this.lblDott.Text = ".";
            this.lblDott.Visible = false;
            // 
            // cboHours
            // 
            this.cboHours.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHours.Enabled = false;
            this.cboHours.FormattingEnabled = true;
            this.cboHours.Items.AddRange(new object[] {
            "03",
            "07",
            "09",
            "10"});
            this.cboHours.Location = new System.Drawing.Point(859, 91);
            this.cboHours.Name = "cboHours";
            this.cboHours.Size = new System.Drawing.Size(36, 21);
            this.cboHours.TabIndex = 1212;
            this.cboHours.Visible = false;
            // 
            // chkAutoWEB
            // 
            this.chkAutoWEB.AutoSize = true;
            this.chkAutoWEB.ForeColor = System.Drawing.Color.Black;
            this.chkAutoWEB.Location = new System.Drawing.Point(92, 397);
            this.chkAutoWEB.Name = "chkAutoWEB";
            this.chkAutoWEB.Size = new System.Drawing.Size(110, 17);
            this.chkAutoWEB.TabIndex = 9;
            this.chkAutoWEB.Text = "Auto Login NGET";
            this.chkAutoWEB.UseVisualStyleBackColor = false;
            this.chkAutoWEB.CheckedChanged += new System.EventHandler(this.chkAuto_CheckedChanged);
            // 
            // cboSeconds
            // 
            this.cboSeconds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSeconds.Enabled = false;
            this.cboSeconds.FormattingEnabled = true;
            this.cboSeconds.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06"});
            this.cboSeconds.Location = new System.Drawing.Point(961, 91);
            this.cboSeconds.Name = "cboSeconds";
            this.cboSeconds.Size = new System.Drawing.Size(36, 21);
            this.cboSeconds.TabIndex = 1211;
            this.cboSeconds.Visible = false;
            // 
            // cboMinutes
            // 
            this.cboMinutes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMinutes.Enabled = false;
            this.cboMinutes.FormattingEnabled = true;
            this.cboMinutes.Items.AddRange(new object[] {
            "56",
            "57",
            "58",
            "59"});
            this.cboMinutes.Location = new System.Drawing.Point(910, 91);
            this.cboMinutes.Name = "cboMinutes";
            this.cboMinutes.Size = new System.Drawing.Size(36, 21);
            this.cboMinutes.TabIndex = 1209;
            this.cboMinutes.Visible = false;
            // 
            // lblDot
            // 
            this.lblDot.AutoSize = true;
            this.lblDot.Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDot.Location = new System.Drawing.Point(946, 89);
            this.lblDot.Name = "lblDot";
            this.lblDot.Size = new System.Drawing.Size(15, 23);
            this.lblDot.TabIndex = 1210;
            this.lblDot.Text = ".";
            this.lblDot.Visible = false;
            // 
            // radioVPair
            // 
            this.radioVPair.AutoSize = true;
            this.radioVPair.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioVPair.ForeColor = System.Drawing.Color.Black;
            this.radioVPair.Location = new System.Drawing.Point(143, 14);
            this.radioVPair.Name = "radioVPair";
            this.radioVPair.Size = new System.Drawing.Size(64, 20);
            this.radioVPair.TabIndex = 1207;
            this.radioVPair.Text = "V Pair";
            this.radioVPair.UseVisualStyleBackColor = false;
            this.radioVPair.CheckedChanged += new System.EventHandler(this.radioVPair_CheckedChanged);
            this.radioVPair.Click += new System.EventHandler(this.radioVPair_Click);
            // 
            // C1AA0
            // 
            this.C1AA0.AutoSize = true;
            this.C1AA0.Checked = true;
            this.C1AA0.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.C1AA0.ForeColor = System.Drawing.Color.Black;
            this.C1AA0.Location = new System.Drawing.Point(207, 14);
            this.C1AA0.Name = "C1AA0";
            this.C1AA0.Size = new System.Drawing.Size(64, 20);
            this.C1AA0.TabIndex = 1208;
            this.C1AA0.TabStop = true;
            this.C1AA0.Text = "H Pair";
            this.C1AA0.UseVisualStyleBackColor = false;
            this.C1AA0.CheckedChanged += new System.EventHandler(this.C1AA0_CheckedChanged);
            this.C1AA0.Click += new System.EventHandler(this.C1AA0_Click);
            // 
            // btnSubmitAll
            // 
            this.btnSubmitAll.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSubmitAll.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSubmitAll.FlatAppearance.BorderSize = 2;
            this.btnSubmitAll.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            this.btnSubmitAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Lime;
            this.btnSubmitAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSubmitAll.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmitAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSubmitAll.Location = new System.Drawing.Point(546, 248);
            this.btnSubmitAll.Name = "btnSubmitAll";
            this.btnSubmitAll.Size = new System.Drawing.Size(174, 25);
            this.btnSubmitAll.TabIndex = 1;
            this.btnSubmitAll.Text = "Open All Ticket";
            this.btnSubmitAll.UseVisualStyleBackColor = false;
            this.btnSubmitAll.Click += new System.EventHandler(this.btnSubmitAll_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.Transparent;
            this.btnSubmit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSubmit.Location = new System.Drawing.Point(876, 195);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(98, 25);
            this.btnSubmit.TabIndex = 0;
            this.btnSubmit.Text = "      Open Selected";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // grpOpen
            // 
            this.grpOpen.BackColor = System.Drawing.Color.Honeydew;
            this.grpOpen.Controls.Add(this.btnNew);
            this.grpOpen.Controls.Add(this.C1AA0);
            this.grpOpen.Controls.Add(this.radioVPair);
            this.grpOpen.Location = new System.Drawing.Point(92, 450);
            this.grpOpen.Name = "grpOpen";
            this.grpOpen.Size = new System.Drawing.Size(665, 40);
            this.grpOpen.TabIndex = 8;
            this.grpOpen.TabStop = false;
            // 
            // btnNew
            // 
            this.btnNew.BackColor = System.Drawing.Color.Transparent;
            this.btnNew.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNew.Location = new System.Drawing.Point(36, 12);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(98, 25);
            this.btnNew.TabIndex = 12;
            this.btnNew.Text = "Save Ticket";
            this.btnNew.UseVisualStyleBackColor = false;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.BackColor = System.Drawing.Color.Red;
            this.btnDeleteAll.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnDeleteAll.FlatAppearance.BorderSize = 2;
            this.btnDeleteAll.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnDeleteAll.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.btnDeleteAll.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDeleteAll.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteAll.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnDeleteAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteAll.Location = new System.Drawing.Point(441, 248);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(98, 25);
            this.btnDeleteAll.TabIndex = 1;
            this.btnDeleteAll.Text = "    Delete All";
            this.btnDeleteAll.UseVisualStyleBackColor = false;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.Transparent;
            this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEdit.Location = new System.Drawing.Point(899, 164);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(98, 25);
            this.btnEdit.TabIndex = 0;
            this.btnEdit.Text = "    Edit";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.Transparent;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(656, 419);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(98, 25);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "     Delete Selected";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // C2AA0
            // 
            this.C2AA0.ElipseRadius = 5;
            this.C2AA0.TargetControl = this;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 305);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(287, 17);
            this.label5.TabIndex = 1214;
            this.label5.Text = "Jitna Bhi Pair Open Karna Hai Type Kar den";
            // 
            // chkAutoAPP
            // 
            this.chkAutoAPP.AutoSize = true;
            this.chkAutoAPP.ForeColor = System.Drawing.Color.Black;
            this.chkAutoAPP.Location = new System.Drawing.Point(227, 397);
            this.chkAutoAPP.Name = "chkAutoAPP";
            this.chkAutoAPP.Size = new System.Drawing.Size(101, 17);
            this.chkAutoAPP.TabIndex = 1215;
            this.chkAutoAPP.Text = "Auto Login APP";
            this.chkAutoAPP.UseVisualStyleBackColor = false;
            this.chkAutoAPP.CheckedChanged += new System.EventHandler(this.ChkAutoAPP_CheckedChanged);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(22, 14, 28);
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Controls.Add(this.lblCount);
            this.pnlHeader.Controls.Add(this.B2AAA);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(18)))));
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(729, 22);
            this.pnlHeader.TabIndex = 1208;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageActive = null;
            this.btnClose.Location = new System.Drawing.Point(702, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(26, 20);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.TabIndex = 1;
            this.btnClose.TabStop = false;
            this.btnClose.Zoom = 10;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.BackColor = System.Drawing.Color.Transparent;
            this.lblCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblCount.Location = new System.Drawing.Point(116, 4);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(26, 17);
            this.lblCount.TabIndex = 11;
            this.lblCount.Text = "04";
            // 
            // B2AAA
            // 
            this.B2AAA.AutoSize = true;
            this.B2AAA.BackColor = System.Drawing.Color.Transparent;
            this.B2AAA.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.B2AAA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.B2AAA.Location = new System.Drawing.Point(5, 3);
            this.B2AAA.Name = "B2AAA";
            this.B2AAA.Size = new System.Drawing.Size(94, 17);
            this.B2AAA.TabIndex = 0;
            this.B2AAA.Text = "Total Ticket";
            // 
            // frmManageTicket
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(729, 280);
            this.Controls.Add(this.chkAutoAPP);
            this.Controls.Add(this.btnDeleteAll);
            this.Controls.Add(this.grdUsers);
            this.Controls.Add(this.chkAutoWEB);
            this.Controls.Add(this.btnSubmitAll);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.cboCredit);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.label34);
            this.Controls.Add(this.lblDott);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.grpOpen);
            this.Controls.Add(this.cboHours);
            this.Controls.Add(this.cboMinutes);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cboSeconds);
            this.Controls.Add(this.lblDot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmManageTicket";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmManageTicket_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdUsers)).EndInit();
            this.grpOpen.ResumeLayout(false);
            this.grpOpen.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }
}

// ===== Express close "sticker" (shared by the Express popups; visual only) =====
// A round red X badge whose centre sits ON the popup's top-right corner (about half of it outside the
// window). WinForms cannot paint outside a form, so it is a tiny owned layered window
// (WS_EX_LAYERED | WS_EX_TOOLWINDOW | WS_EX_NOACTIVATE) drawn with per-pixel alpha.
// Its click runs the popup's existing close path (the caller passes it), so closing behaves exactly the same.
internal sealed class ExpressCloseSticker : NativeWindow, IDisposable
{
    /// <summary>Creates the sticker for owner and keeps it glued to the owner's top-right corner.</summary>
    public static ExpressCloseSticker Attach(Form owner, MethodInvoker click)
    {
        ExpressCloseSticker s = new ExpressCloseSticker(owner, click);
        EventHandler sync = delegate { s.SyncWithOwner(); };
        owner.Move += sync;
        owner.Resize += sync;
        owner.LocationChanged += sync;
        owner.SizeChanged += sync;
        owner.VisibleChanged += sync;
        owner.Activated += sync;
        owner.Shown += sync;
        owner.HandleDestroyed += delegate { s.DropHandle(); };   // recreated on the next sync
        owner.FormClosed += delegate { s.Dispose(); };
        owner.Disposed += delegate { s.Dispose(); };
        if (owner.IsHandleCreated && owner.Visible) s.SyncWithOwner();
        return s;
    }

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

    private ExpressCloseSticker(Form owner, MethodInvoker click)
    {
        this.owner = owner;
        this.click = click;
    }

    private void SyncWithOwner()
    {
        if (dead || owner == null || owner.IsDisposed || owner.Disposing || !owner.IsHandleCreated) return;
        Sync(owner.Visible && owner.WindowState != FormWindowState.Minimized);
    }

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
                    owner.BeginInvoke(click);   // exactly the popup's existing close path
                m.Result = IntPtr.Zero;
                return;
            }
        }
        base.WndProc(ref m);
    }
}
