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
        if (Class28.dictionary_8.Any((KeyValuePair<string, int> tr) => tr.Key.Equals(tktname_, StringComparison.CurrentCultureIgnoreCase)))
        {
            Form owner = Program.TopMost_();
            string text = "Ticket Already Open";
            MessageBox.Show(owner, text, tktname_, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            return;
        }
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
                string text7 = Program.paidpair.ToString();
                string text8 = " Pairs at a time.";
                string text9 = text6 + text7 + text8;
                string caption = "Limitation";
                MessageBox.Show(owner2, text9, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                Form owner3 = Program.TopMost_();
                string text10 = "You cannot Open more Than ";
                string text11 = Program.paidpair.ToString();
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


    // ByteNova Nova visual layer. Existing ticket/database/business handlers remain unchanged.
    private void ApplyByteNovaNovaStyle()
    {
        SuspendLayout();

        BackColor = Color.FromArgb(2, 13, 27);
        ForeColor = Color.FromArgb(232, 247, 255);
        DoubleBuffered = true;

        if (pnlHeader != null)
        {
            pnlHeader.BackColor = Color.FromArgb(3, 29, 59);
            pnlHeader.Paint -= ByteNovaManageHeaderPaint;
            pnlHeader.Paint += ByteNovaManageHeaderPaint;
        }

        if (B2AAA != null)
        {
            B2AAA.ForeColor = Color.White;
            B2AAA.BackColor = Color.Transparent;
            B2AAA.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        }

        if (btnClose != null)
        {
            btnClose.BackColor = Color.FromArgb(5, 43, 77);
            btnClose.Zoom = 0;
            btnClose.Cursor = Cursors.Hand;
            btnClose.BringToFront();
        }

        if (grdUsers != null)
        {
            grdUsers.EnableHeadersVisualStyles = false;
            grdUsers.BackgroundColor = Color.FromArgb(31, 19, 43);
            grdUsers.BorderStyle = BorderStyle.None;
            grdUsers.GridColor = Color.FromArgb(28, 83, 112);
            grdUsers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grdUsers.RowHeadersVisible = false;
            grdUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grdUsers.ColumnHeadersHeight = 30;
            grdUsers.RowTemplate.Height = 28;

            grdUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(5, 49, 80);
            grdUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(100, 225, 255);
            grdUsers.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(5, 49, 80);
            grdUsers.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
            grdUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            grdUsers.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grdUsers.DefaultCellStyle.BackColor = Color.FromArgb(7, 29, 48);
            grdUsers.DefaultCellStyle.ForeColor = Color.White;
            grdUsers.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 105, 155);
            grdUsers.DefaultCellStyle.SelectionForeColor = Color.White;
            grdUsers.DefaultCellStyle.Font = new Font("Segoe UI", 8.2F, FontStyle.Regular);
            grdUsers.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            grdUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(9, 37, 59);
            grdUsers.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            grdUsers.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 105, 155);
            grdUsers.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;

            foreach (DataGridViewColumn col in grdUsers.Columns)
            {
                DataGridViewButtonColumn bc = col as DataGridViewButtonColumn;
                if (bc != null)
                {
                    bc.FlatStyle = FlatStyle.Flat;
                    bc.DefaultCellStyle.BackColor = Color.FromArgb(8, 57, 88);
                    bc.DefaultCellStyle.ForeColor = Color.White;
                    bc.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 135, 185);
                    bc.DefaultCellStyle.SelectionForeColor = Color.White;
                    bc.DefaultCellStyle.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
                }

                DataGridViewComboBoxColumn cc = col as DataGridViewComboBoxColumn;
                if (cc != null)
                {
                    cc.FlatStyle = FlatStyle.Flat;
                    cc.DefaultCellStyle.BackColor = Color.White;
                    cc.DefaultCellStyle.ForeColor = Color.Black;
                    cc.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 245, 255);
                    cc.DefaultCellStyle.SelectionForeColor = Color.Black;
                }
            }
        }

        if (grpOpen != null)
        {
            grpOpen.BackColor = Color.FromArgb(5, 27, 47);
            grpOpen.ForeColor = Color.FromArgb(80, 220, 255);
            grpOpen.Paint -= ByteNovaManageGroupPaint;
            grpOpen.Paint += ByteNovaManageGroupPaint;
        }

        ByteNovaManageButton(btnNew, Color.FromArgb(0, 110, 165), Color.FromArgb(198, 244, 50));
        ByteNovaManageButton(btnSubmitAll, Color.FromArgb(0, 118, 150), Color.FromArgb(65, 235, 225));
        ByteNovaManageButton(btnDeleteAll, Color.FromArgb(125, 38, 58), Color.FromArgb(255, 100, 125));

        // Helper buttons remain available to their existing handlers but do not disturb the compact layout.
        if (btnSubmit != null) btnSubmit.Visible = false;
        if (btnEdit != null) btnEdit.Visible = false;
        if (btnDelete != null) btnDelete.Visible = false;

        ByteNovaManageInput(cboCredit);
        ByteNovaManageInput(cboHours);
        ByteNovaManageInput(cboMinutes);
        ByteNovaManageInput(cboSeconds);

        ByteNovaManageCheck(chkAutoWEB);
        ByteNovaManageCheck(chkAutoAPP);
        ByteNovaManageRadio(radioVPair);
        ByteNovaManageRadio(C1AA0);

        if (lblCount != null)
        {
            lblCount.ForeColor = Color.FromArgb(80, 230, 255);
            lblCount.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
            lblCount.BackColor = Color.Transparent;
        }
        if (label2 != null)
        {
            label2.ForeColor = Color.FromArgb(220, 240, 250);
            label2.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            label2.BackColor = Color.Transparent;
        }

        Paint -= ByteNovaManageFormPaint;
        Paint += ByteNovaManageFormPaint;

        ResumeLayout(false);
        PerformLayout();
    }

    private void ByteNovaManageButton(Button b, Color back, Color border)
    {
        if (b == null) return;
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 1;
        b.FlatAppearance.BorderColor = border;
        b.BackColor = back;
        b.ForeColor = Color.White;
        b.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
        b.UseVisualStyleBackColor = false;
        b.Cursor = Cursors.Hand;
    }

    private void ByteNovaManageInput(ComboBox c)
    {
        if (c == null) return;
        c.FlatStyle = FlatStyle.Flat;
        c.BackColor = Color.White;
        c.ForeColor = Color.Black;
        c.Font = new Font("Segoe UI", 8.5F);
    }

    private void ByteNovaManageCheck(CheckBox c)
    {
        if (c == null) return;
        c.BackColor = Color.Transparent;
        c.ForeColor = Color.White;
        c.FlatStyle = FlatStyle.Standard;
        c.UseVisualStyleBackColor = true;
    }

    private void ByteNovaManageRadio(RadioButton r)
    {
        if (r == null) return;
        r.BackColor = Color.Transparent;
        r.ForeColor = Color.White;
        r.FlatStyle = FlatStyle.Standard;
        r.UseVisualStyleBackColor = true;
        r.AutoCheck = true;
    }

    private void ByteNovaManageHeaderPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null || c.Width < 2 || c.Height < 2) return;
        Rectangle r = c.ClientRectangle;
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(2, 23, 52), Color.FromArgb(5, 53, 91),
            LinearGradientMode.Horizontal))
            e.Graphics.FillRectangle(bg, r);
        using (Pen p = new Pen(Color.FromArgb(198, 244, 50), 1.5F))
            e.Graphics.DrawLine(p, 0, r.Height - 1, r.Width, r.Height - 1);
    }

    private void ByteNovaManageGroupPaint(object sender, PaintEventArgs e)
    {
        GroupBox g = sender as GroupBox;
        if (g == null || g.Width < 2 || g.Height < 2) return;
        Rectangle r = new Rectangle(0, 0, g.Width - 1, g.Height - 1);
        using (Pen p = new Pen(Color.FromArgb(0, 150, 205), 1F))
            e.Graphics.DrawRectangle(p, r);
    }

    private void ByteNovaManageFormPaint(object sender, PaintEventArgs e)
    {
        if (ClientSize.Width < 2 || ClientSize.Height < 2) return;
        using (Pen p = new Pen(Color.FromArgb(198, 244, 50), 1F))
            e.Graphics.DrawRectangle(p, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
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
