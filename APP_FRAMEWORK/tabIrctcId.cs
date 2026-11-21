using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Bunifu.Framework.UI;
using IRCommDLL;
using Microsoft.VisualBasic;
using NEXUS;

namespace APP_FRAMEWORK;

internal partial class tabIrctcId : UserControl
{
    private mCOMM mcommInstance;

    private bool bool_0;

    private DataTable dataTable_0;

    private IDisposable idisposable_0;

    private GroupBox grpAddID;

    private Label lblTotal;

    private Label lblActive;

    private Label label2;

    private Label B1AAA;

    private Button C1AA0;

    internal TextBox txtPnr;

    private Button btnIDSave;

    internal TextBox txtPassword;

    internal CheckBox chkValidate;

    internal Label lblPassword;

    private DataGridView grdUsers;

    private ToolTip toolTip_0;

    private IContainer icontainer_0;

    internal CheckBox chkShowPwd;

    private Button btnChangePwd;

    private DateTimePicker dtDate2;

    internal TextBox txtLogin;

    internal Label label37;

    private Button button2;

    private RadioButton RadioPnr;

    private RadioButton RadioJdate;

    private RadioButton RadioBdate;

    private DateTimePicker B2AAA;

    private Button C2AA0;

    private Button btnChangeQn;

    private Button button5;

    private GroupBox groupBox1;

    private Panel panel13;

    private Label lblCurMonbk;

    private Label lblTobk;

    private Label label3;

    private Label label1;

    private Label lbl7;

    private Label lbl6;

    private Label B3AAA;

    private Label C3AA0;

    private Button btnHideDay;

    private ComboBox CmbDays;

    private Label label14;

    private Panel pnlloader;

    private PictureBox pictureBox1;

    private Label lblMsg;

    private BackgroundWorker backgroundWorker_0;

    private BackgroundWorker backgroundWorker_1;

    private BackgroundWorker backgroundWorker_2;

    private BunifuElipse bunifuElipse_0;

    internal TextBox textmob;

    internal Label label4;

    internal TextBox textmpass;

    internal Label label5;

    private DataGridViewTextBoxColumn B4AAA;

    private DataGridViewTextBoxColumn C4AA0;

    private BackgroundWorker bgwkverify_;

    private Button button3;

    private DataGridViewTextBoxColumn clmID;

    private DataGridViewTextBoxColumn clmPwd;

    private DataGridViewTextBoxColumn clmMTotal;

    private DataGridViewLinkColumn clmCheck;

    private DataGridViewLinkColumn clmAvtive;

    private DataGridViewLinkColumn clmDelete;

    private DataGridViewLinkColumn ClmHis;

    private CheckBox checkBox1;

    private Label label6;

    private RichTextBox txtbulk;

    private Button button4;

    private Label lblDisableCount;

    private Label label7;

    private Label lblUnVerified;

    private Label label11;

    private Label lblInValid;

    private Label label9;

    private Label label10;
    private Button button1;
    private IContainer components;

    public tabIrctcId()
    {
        try
        {
            InitializeComponent();
            method_1();
            //ApplyByteNovaLatestUI();
        }
        catch (Exception)
        {
        }
    }

    private void method_0(object sender, EventArgs e)
    {
        try
        {
            CmbDays.Text = Program.mySettings_0.KeepId;
            method_1();
        }
        catch (Exception)
        {
        }
    }

    private void btnIDSave_Click_1(object sender, EventArgs e)
    {
        if (!checkBox1.Checked)
        {
            if (string.IsNullOrEmpty(txtLogin.Text) || string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(textmob.Text))
            {
                goto IL_0336;
            }
            if (Program.dictionary_0.ContainsKey(txtLogin.Text.Trim()))
            {
                string text = "User already exist!";
                MessageBox.Show(text);
                return;
            }
            bool flag;
            if (!chkValidate.Checked)
            {
                flag = true;
            }
            else
            {
                Cursor = Cursors.WaitCursor;
                flag = true;
                Cursor = Cursors.Default;
            }
            if (flag)
            {
                DataRow dataRow = dataTable_0.NewRow();
                dataRow[0] = txtLogin.Text.Trim();
                dataRow[1] = txtPassword.Text.Trim();
                string value = "0";
                dataRow[2] = value;
                dataRow[3] = textmob.Text;
                string value2 = "Active";
                dataRow[4] = value2;
                string value3 = "Delete";
                dataRow[5] = value3;
                string value4 = "View";
                dataRow[6] = value4;
                dataTable_0.Rows.Add(dataRow);
                dataTable_0.AcceptChanges();
                Program.dictionary_0.Add(txtLogin.Text.Trim(), txtPassword.Text.Trim());
                Program.dictionary_01.Add(txtLogin.Text.Trim(), txtPassword.Text.Trim());
                try
                {
                    SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
                    SQLiteCommand sQLiteCommand = new SQLiteCommand();
                    sQLiteConnection.Open();
                    sQLiteCommand.Connection = sQLiteConnection;
                    string[] array = new string[7];
                    string text2 = "INSERT INTO [TBL_USER]([UserName],[strPwd],[UserState]) VALUES('";
                    array[0] = text2;
                    array[1] = dataRow[0].ToString();
                    string text3 = "', '";
                    array[2] = text3;
                    array[3] = dataRow[1].ToString();
                    string text4 = "', 'Active||";
                    array[4] = text4;
                    array[5] = textmob.Text;
                    string text5 = "')";
                    array[6] = text5;
                    sQLiteCommand.CommandText = string.Concat(array);
                    sQLiteCommand.ExecuteNonQuery();
                    sQLiteConnection.Close();
                    sQLiteCommand.Dispose();
                    bool_0 = true;
                    string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    string path = "idlog";
                    string text6 = Path.Combine(folderPath, path);
                    if (!Directory.Exists(text6))
                    {
                        Directory.CreateDirectory(text6);
                    }
                    string path2 = Path.Combine(text6, txtLogin.Text.Trim());
                    string text7 = textmob.Text.Trim();
                    string text8 = "||";
                    File.WriteAllText(path2, text7 + text8 + textmpass.Text.Trim());
                }
                catch (Exception)
                {
                }
            }
        }
        else
        {
            if (string.IsNullOrEmpty(txtbulk.Text) || string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(textmob.Text))
            {
                goto IL_0336;
            }
            RichTextBox richTextBox = txtbulk;
            string text9 = txtbulk.Text;
            string oldValue = " ";
            richTextBox.Text = text9.Replace(oldValue, "");
            string[] array2 = txtbulk.Text.Split(new char[1] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string[] array3 = array2;
            foreach (string text10 in array3)
            {
                if (Program.dictionary_0.ContainsKey(text10.Trim()))
                {
                    continue;
                }
                if (chkValidate.Checked)
                {
                    Cursor = Cursors.WaitCursor;
                    Cursor = Cursors.Default;
                }
                DataRow dataRow2 = dataTable_0.NewRow();
                dataRow2[0] = text10.Trim();
                dataRow2[1] = txtPassword.Text.Trim();
                string value5 = "0";
                dataRow2[2] = value5;
                dataRow2[3] = textmob.Text;
                string value6 = "Active";
                dataRow2[4] = value6;
                string value7 = "Delete";
                dataRow2[5] = value7;
                string value8 = "View";
                dataRow2[6] = value8;
                dataTable_0.Rows.Add(dataRow2);
                dataTable_0.AcceptChanges();
                Program.dictionary_0.Add(text10.Trim(), txtPassword.Text.Trim());
                Program.dictionary_01.Add(text10.Trim(), txtPassword.Text.Trim());
                try
                {
                    SQLiteConnection sQLiteConnection2 = new SQLiteConnection(Class34.string_Database);
                    SQLiteCommand sQLiteCommand2 = new SQLiteCommand();
                    sQLiteConnection2.Open();
                    sQLiteCommand2.Connection = sQLiteConnection2;
                    string[] array4 = new string[7];
                    string text11 = "INSERT INTO [TBL_USER]([UserName],[strPwd],[UserState]) VALUES('";
                    array4[0] = text11;
                    array4[1] = dataRow2[0].ToString();
                    string text12 = "', '";
                    array4[2] = text12;
                    array4[3] = dataRow2[1].ToString();
                    string text13 = "', 'Active||";
                    array4[4] = text13;
                    array4[5] = textmob.Text;
                    string text14 = "')";
                    array4[6] = text14;
                    sQLiteCommand2.CommandText = string.Concat(array4);
                    sQLiteCommand2.ExecuteNonQuery();
                    sQLiteConnection2.Close();
                    sQLiteCommand2.Dispose();
                    bool_0 = true;
                    string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    string path3 = "idlog";
                    string text15 = Path.Combine(folderPath2, path3);
                    if (!Directory.Exists(text15))
                    {
                        Directory.CreateDirectory(text15);
                    }
                    string path4 = Path.Combine(text15, text10.Trim());
                    string text16 = textmob.Text.Trim();
                    string text17 = "||";
                    File.WriteAllText(path4, text16 + text17 + textmpass.Text.Trim());
                }
                catch (Exception)
                {
                }
            }
        }
        goto IL_0620;
    IL_0620:
        txtLogin.Focus();
        return;
    IL_0336:
        string text18 = "Fill All Data Properly. Please try again.";
        string caption = "ERROR";
        MessageBox.Show(text18, caption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
        goto IL_0620;
    }

    private void method_1()
    {
        try
        {
            txtPassword.PasswordChar = ((!chkShowPwd.Checked) ? '*' : '\0');
            txtLogin.Focus();
            ToolTip toolTip = toolTip_0;
            TextBox control = txtLogin;
            string caption = "Type your IRCTC login name here.";
            toolTip.SetToolTip(control, caption);
            ToolTip toolTip2 = toolTip_0;
            string text = "Don't use this option between 8:00AM to 10:10AM, otherwise leave this option checked.";
            toolTip2.Show(text, chkValidate, 2000, chkValidate.Location.X, chkValidate.Location.Y);
            string query = "SELECT * FROM [TBL_USER]";
            method_IrctcId2(query, "");
        }
        catch (Exception)
        {
        }
    }

    internal void method_IrctcId2(string Query, string His)
    {
        try
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            string tableName = "tbl_Ir_Users";
            dataTable_0 = new DataTable(tableName);
            DataColumnCollection columns = dataTable_0.Columns;
            string columnName = "userid";
            columns.Add(new DataColumn(columnName));
            DataColumnCollection columns2 = dataTable_0.Columns;
            string columnName2 = "Pwd";
            columns2.Add(new DataColumn(columnName2));
            DataColumnCollection columns3 = dataTable_0.Columns;
            string columnName3 = "currtotal";
            columns3.Add(new DataColumn(columnName3));
            DataColumnCollection columns4 = dataTable_0.Columns;
            string columnName4 = "sync";
            columns4.Add(new DataColumn(columnName4));
            DataColumnCollection columns5 = dataTable_0.Columns;
            string columnName5 = "state";
            columns5.Add(new DataColumn(columnName5));
            DataColumnCollection columns6 = dataTable_0.Columns;
            string columnName6 = "delete";
            columns6.Add(new DataColumn(columnName6));
            DataColumnCollection columns7 = dataTable_0.Columns;
            string columnName7 = "history";
            columns7.Add(new DataColumn(columnName7));
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            int num9 = 0;
            using (SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database))
            {
                try
                {
                    SQLiteDataAdapter sQLiteDataAdapter = new SQLiteDataAdapter(Query, Class34.string_Database);
                    DataTable dataTable = new DataTable();
                    sQLiteDataAdapter.Fill(dataTable);
                    num6 = dataTable.Rows.Count;
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        sQLiteConnection.Open();
                        new SQLiteCommand().Connection = sQLiteConnection;
                        foreach (DataRow row in dataTable.Rows)
                        {
                            DataRow dataRow2 = dataTable_0.NewRow();
                            dataRow2[0] = row[1];
                            dataRow2[1] = row[2];
                            string value = "error";
                            dataRow2[2] = value;
                            string columnName8 = "UserState";
                            string text = row[columnName8].ToString();
                            string[] array = new string[1];
                            string text2 = "||";
                            array[0] = text2;
                            string[] array2 = text.Split(array, StringSplitOptions.None);
                            dataRow2[3] = array2[1];
                            dataRow2[4] = array2[0];
                            string value2 = "Delete";
                            dataRow2[5] = value2;
                            string value3 = "View";
                            dataRow2[6] = value3;
                            string text3 = array2[0];
                            string value4 = "Disabled";
                            bool flag = text3.Contains(value4);
                            string text4 = array2[0];
                            string value5 = "Suspend";
                            if (flag | text4.Contains(value5))
                            {
                                num3++;
                            }
                            else
                            {
                                string text5 = array2[0];
                                string value6 = "Invalid";
                                if (!text5.Contains(value6))
                                {
                                    string text6 = array2[0];
                                    string value7 = "InCompleteProfile";
                                    if (text6.Contains(value7))
                                    {
                                        num5++;
                                    }
                                    else
                                    {
                                        string text7 = array2[0];
                                        string value8 = "Deactive";
                                        if (!text7.Contains(value8))
                                        {
                                            num++;
                                        }
                                        else
                                        {
                                            num2++;
                                        }
                                    }
                                }
                                else
                                {
                                    num4++;
                                }
                            }
                            Dictionary<string, string> dictionary_ = Program.dictionary_0;
                            string columnName9 = "userid";
                            if (!dictionary_.ContainsKey(dataRow2[columnName9].ToString()))
                            {
                                string columnName10 = "state";
                                string text8 = dataRow2[columnName10].ToString();
                                string value9 = "Active";
                                if (text8.Contains(value9))
                                {
                                    Dictionary<string, string> dictionary_2 = Program.dictionary_0;
                                    string columnName11 = "userid";
                                    string key = dataRow2[columnName11].ToString();
                                    string columnName12 = "Pwd";
                                    dictionary_2.Add(key, dataRow2[columnName12].ToString());
                                    Dictionary<string, string> list_SelectedIRCTCid = Program.list_SelectedIRCTCid;
                                    string columnName13 = "userid";
                                    string key2 = dataRow2[columnName13].ToString();
                                    string value10 = "free";
                                    list_SelectedIRCTCid.Add(key2, value10);
                                }
                            }
                            if (!chkShowPwd.Checked)
                            {
                                string value11 = "**********";
                                dataRow2[1] = value11;
                            }
                            dataTable_0.Rows.Add(dataRow2);
                        }
                        sQLiteConnection.Close();
                    }
                }
                catch
                {
                }
                if (His == "")
                {
                    lblTotal.Text = num6.ToString();
                    lblActive.Text = num.ToString();
                    lbl7.Text = num7.ToString();
                    lblTobk.Text = num8.ToString();
                    lblCurMonbk.Text = num9.ToString();
                    lbl6.Text = num2.ToString();
                    lblDisableCount.Text = num3.ToString();
                    lblInValid.Text = num4.ToString();
                    lblUnVerified.Text = num5.ToString();
                }
            }
            dataTable_0.AcceptChanges();
            grdUsers.DataSource = dataTable_0;
        }
        catch (Exception)
        {
        }
    }

    private void grdUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
    {
        try
        {
            int rowIndex = e.RowIndex;
            int columnIndex = e.ColumnIndex;
            DataRow dataRow = dataTable_0.Rows[rowIndex];
            string columnName = "userid";
            string text = dataRow[columnName].ToString();
            DataRow dataRow2 = dataTable_0.Rows[rowIndex];
            string columnName2 = "pwd";
            string text2 = dataRow2[columnName2].ToString();
            try
            {
                text2 = Program.dictionary_01[text];
            }
            catch
            {
                DataRow dataRow3 = dataTable_0.Rows[rowIndex];
                string columnName3 = "state";
                string text3 = dataRow3[columnName3].ToString();
                string value = "Invalid";
                if (text3.Contains(value))
                {
                    string text4 = "The Selected User Id Is Deactivated";
                    MessageBox.Show(text4);
                }
            }
            txtLogin.Text = text;
            txtPassword.Text = text2;
            switch (columnIndex)
            {
                case 0:
                case 1:
                    switch (columnIndex)
                    {
                        case 1:
                            {
                                DataRow dataRow13 = dataTable_0.Rows[rowIndex];
                                string columnName13 = "pwd";
                                text = dataRow13[columnName13].ToString();
                                break;
                            }
                        case 0:
                            {
                                DataRow dataRow12 = dataTable_0.Rows[rowIndex];
                                string columnName12 = "userid";
                                text = dataRow12[columnName12].ToString();
                                break;
                            }
                    }
                    Clipboard.SetText(text);
                    break;
                case 3:
                    {
                        lblMsg.Visible = true;
                        Label label = lblMsg;
                        string text22 = "Updating Id Records - ";
                        label.Text = text22 + text;
                        pictureBox1.Visible = true;
                        WaitCallback callBack = method_4;
                        string[] array3 = new string[5]
                        {
                    rowIndex.ToString(),
                    null,
                    null,
                    null,
                    null
                        };
                        string text23 = "||";
                        array3[1] = text23;
                        array3[2] = text;
                        string text24 = "||";
                        array3[3] = text24;
                        array3[4] = text2;
                        ThreadPool.QueueUserWorkItem(callBack, string.Concat(array3));
                        break;
                    }
                case 4:
                    {
                        DataRow dataRow4 = dataTable_0.Rows[rowIndex];
                        string columnName4 = "sync";
                        string text9 = dataRow4[columnName4].ToString();
                        DataRow dataRow5 = dataTable_0.Rows[rowIndex];
                        string columnName5 = "state";
                        string text10 = dataRow5[columnName5].ToString();
                        string text11 = "Active";
                        string commandText;
                        if (text10 == text11)
                        {
                            Program.dictionary_0.Remove(text);
                            Program.dictionary_01.Remove(text);
                            Dictionary<string, string> list_SelectedIRCTCid = Program.list_SelectedIRCTCid;
                            DataRow dataRow6 = dataTable_0.Rows[rowIndex];
                            string columnName6 = "userid";
                            list_SelectedIRCTCid.Remove(dataRow6[columnName6].ToString());
                            DataRow dataRow7 = dataTable_0.Rows[rowIndex];
                            string columnName7 = "state";
                            string value2 = "Deactive";
                            dataRow7[columnName7] = value2;
                            string[] array = new string[5];
                            string text12 = "UPDATE [TBL_USER] SET [UserState]='Deactive||";
                            array[0] = text12;
                            array[1] = text9;
                            string text13 = "' where UserName LIKE '";
                            array[2] = text13;
                            array[3] = text;
                            string text14 = "'";
                            array[4] = text14;
                            commandText = string.Concat(array);
                        }
                        else
                        {
                            if (!Program.dictionary_0.ContainsKey(text))
                            {
                                Program.dictionary_0.Add(text, text2);
                            }
                            Dictionary<string, string> list_SelectedIRCTCid2 = Program.list_SelectedIRCTCid;
                            DataRow dataRow8 = dataTable_0.Rows[rowIndex];
                            string columnName8 = "userid";
                            string key = dataRow8[columnName8].ToString();
                            string value3 = "free";
                            list_SelectedIRCTCid2.Add(key, value3);
                            DataRow dataRow9 = dataTable_0.Rows[rowIndex];
                            string columnName9 = "state";
                            string value4 = "Active";
                            dataRow9[columnName9] = value4;
                            string[] array2 = new string[5];
                            string text15 = "UPDATE [TBL_USER] SET [UserState]='Active||";
                            array2[0] = text15;
                            array2[1] = text9;
                            string text16 = "' where UserName LIKE '";
                            array2[2] = text16;
                            array2[3] = text;
                            string text17 = "'";
                            array2[4] = text17;
                            commandText = string.Concat(array2);
                        }
                        SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
                        SQLiteCommand sQLiteCommand = new SQLiteCommand();
                        sQLiteConnection.Open();
                        sQLiteCommand.Connection = sQLiteConnection;
                        sQLiteCommand.CommandText = commandText;
                        sQLiteCommand.ExecuteNonQuery();
                        sQLiteConnection.Close();
                        sQLiteCommand.Dispose();
                        break;
                    }
                case 5:
                    {
                        string text18 = "Do you really want to delete user?";
                        string caption = "Confirm";
                        if (MessageBox.Show(text18, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            Dictionary<string, string> dictionary_ = Program.dictionary_0;
                            DataRow dataRow10 = dataTable_0.Rows[rowIndex];
                            string columnName10 = "userid";
                            dictionary_.Remove(dataRow10[columnName10].ToString());
                            Dictionary<string, string> dictionary_2 = Program.dictionary_01;
                            DataRow dataRow11 = dataTable_0.Rows[rowIndex];
                            string columnName11 = "userid";
                            dictionary_2.Remove(dataRow11[columnName11].ToString());
                            try
                            {
                                SQLiteConnection sQLiteConnection2 = new SQLiteConnection(Class34.string_Database);
                                SQLiteCommand sQLiteCommand2 = new SQLiteCommand();
                                sQLiteConnection2.Open();
                                sQLiteCommand2.Connection = sQLiteConnection2;
                                string text19 = "DELETE FROM [TBL_USER] WHERE [UserName] = '";
                                string text20 = text;
                                string text21 = "'";
                                sQLiteCommand2.CommandText = text19 + text20 + text21;
                                sQLiteCommand2.ExecuteNonQuery();
                                sQLiteConnection2.Close();
                                sQLiteCommand2.Dispose();
                            }
                            catch (Exception)
                            {
                            }
                            dataTable_0.Rows.RemoveAt(rowIndex);
                        }
                        break;
                    }
                case 6:
                    {
                        Form form = ((Control)sender).Parent.FindForm();
                        string name = form.Name;
                        string text5 = "frmCombineCreditials";
                        if (!(name == text5))
                        {
                            frmAddID frmAddID2 = (frmAddID)form;
                            tabHistory tabHistory2 = frmAddID2.tabHistory;
                            string text6 = "SELECT * FROM [TBL_USER] WHERE [UserName] = '";
                            string text7 = text;
                            string text8 = "'";
                            tabHistory2.method_IrctcId3(text6 + text7 + text8, text, text2);
                            frmAddID2.labe6_MouseUp(sender, new EventArgs());
                        }
                        break;
                    }
                case 2:
                    break;
            }
        }
        catch (Exception)
        {
        }
    }

    private void chkShowPwd_CheckedChanged(object sender, EventArgs e)
    {
        method_1();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        if (RadioBdate.Checked)
        {
            DateTime value = B2AAA.Value;
            string text = "yyyy-MM-dd";
            string text2 = value.ToString(text);
            string text3 = "SELECT * FROM [TBL_USER] Where History LIKE '%";
            string text4 = "T%'";
            method_IrctcId2(text3 + text2 + text4, text2);
        }
        if (RadioJdate.Checked)
        {
            DateTime value2 = dtDate2.Value;
            string text5 = "yyyy-MM-dd";
            string text6 = value2.ToString(text5);
            string text7 = "SELECT * FROM [TBL_USER] Where History LIKE '%";
            string text8 = "B%'";
            method_IrctcId2(text7 + text6 + text8, text6);
        }
        if (RadioPnr.Checked)
        {
            string text9 = "SELECT * FROM [TBL_USER] Where History LIKE '%";
            string text10 = txtPnr.Text;
            string text11 = "%'";
            string query = text9 + text10 + text11;
            string his = "1";
            method_IrctcId2(query, his);
        }
    }

    private void C2AA0_Click(object sender, EventArgs e)
    {
        B2AAA.Enabled = false;
        dtDate2.Enabled = false;
        txtPnr.Enabled = false;
        method_1();
    }

    private void C1AA0_Click_1(object sender, EventArgs e)
    {
        lblMsg.Visible = true;
        pictureBox1.Visible = true;
        backgroundWorker_0.RunWorkerAsync();
    }

    private void btnChangePwd_Click(object sender, EventArgs e)
    {
        backgroundWorker_1.RunWorkerAsync();
    }

    private void BAAAA(object sender, EventArgs e)
    {
        backgroundWorker_2.RunWorkerAsync();
    }


    private void method_2(object object_0)
    {
        try
        {
            if (pnlloader.InvokeRequired)
            {
                pnlloader.Invoke(new ParameterizedThreadStart(method_2), object_0);
            }
            else
            {
                pnlloader.Visible = bool.Parse(object_0.ToString());
            }
            Application.DoEvents();
            Thread.Sleep(500);
        }
        catch (Exception)
        {
        }
    }

    private void method_3(object object_0)
    {
        try
        {
            if (lblMsg.InvokeRequired)
            {
                lblMsg.Invoke(new ParameterizedThreadStart(method_3), object_0);
            }
            else
            {
                lblMsg.Text = object_0.ToString();
            }
            Application.DoEvents();
        }
        catch (Exception)
        {
        }
    }

    private void method_4(object object_0)
    {
        try
        {
            string text = "ProfileHistory";
            string errMessage = text;
            string text2 = object_0.ToString();
            string[] array = new string[1];
            string text3 = "||";
            array[0] = text3;
            string[] array2 = text2.Split(array, StringSplitOptions.None);
            int index = int.Parse(array2[0]);
            string text4 = array2[1];
            string text5 = array2[2];
            method_2(true);
            try
            {
                string[] array3 = Program.mcommInstance_.isLV(text4.Trim(), text5.Trim(), ref errMessage).Split('^')[1].Split('#');
                DataRow dataRow = dataTable_0.Rows[index];
                string columnName = "currtotal";
                string[] array4 = new string[5]
                {
                    array3[0],
                    null,
                    null,
                    null,
                    null
                };
                string text6 = "-";
                array4[1] = text6;
                array4[2] = array3[1];
                string text7 = "-";
                array4[3] = text7;
                array4[4] = array3[2];
                dataRow[columnName] = string.Concat(array4);
            }
            catch
            {
            }
            method_2(false);
            lblMsg.Visible = false;
            pictureBox1.Visible = false;
        }
        catch (Exception)
        {
        }
    }

    private void backgroundWorker_0_DoWork4(object sender, DoWorkEventArgs e)
    {
        try
        {
            method_2(true);
            DataTable dataTable = new DataTable();
            string commandText = "SELECT * FROM TBL_USER";
            new SQLiteDataAdapter(commandText, Class34.string_Database).Fill(dataTable);
            int num = 0;
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    try
                    {
                        string columnName = "UserName";
                        string text = row[columnName].ToString();
                        string columnName2 = "strPwd";
                        string pwd = row[columnName2].ToString();
                        string text2 = "ProfileHistory";
                        string errMessage = text2;
                        string text3 = "UpdatingRecords: ";
                        method_3(text3 + text);
                        string[] array = Program.mcommInstance_.isLV(text, pwd, ref errMessage).Split('^')[1].Split('#');
                        DataRow dataRow2 = dataTable_0.Rows[num];
                        string columnName3 = "currtotal";
                        string[] array2 = new string[5]
                        {
                            array[0],
                            null,
                            null,
                            null,
                            null
                        };
                        string text4 = "-";
                        array2[1] = text4;
                        array2[2] = array[1];
                        string text5 = "-";
                        array2[3] = text5;
                        array2[4] = array[2];
                        dataRow2[columnName3] = string.Concat(array2);
                    }
                    catch
                    {
                        try
                        {
                            DataRow dataRow3 = dataTable_0.Rows[num];
                            string columnName4 = "currtotal";
                            string value = "error";
                            dataRow3[columnName4] = value;
                        }
                        catch
                        {
                        }
                    }
                    num++;
                }
            }
            string object_ = "Process Complete";
            method_3(object_);
            method_2(false);
        }
        catch (Exception)
        {
        }
    }

    private void backgroundWorker_0_DoWork(object sender, DoWorkEventArgs e)
    {
        try
        {
            mcommInstance = new IRComm();
            mcommInstance.SetCon(Class34.string_Database);
            method_2(true);
            DataTable dataTable = new DataTable();
            string commandText = "SELECT * FROM TBL_USER";
            new SQLiteDataAdapter(commandText, Class34.string_Database).Fill(dataTable);
            int num = 0;
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    try
                    {
                        string columnName = "UserName";
                        string text = row[columnName].ToString();
                        string columnName2 = "strPwd";
                        string pwd = row[columnName2].ToString();
                        string text2 = "ProfileHistory";
                        string errMessage = text2;
                        string text3 = "UpdatingRecords: ";
                        method_3(text3 + text);
                        string[] array = mcommInstance.isLV(text, pwd, ref errMessage).Split('^')[1].Split('#');
                        DataRow dataRow2 = dataTable_0.Rows[num];
                        string columnName3 = "currtotal";
                        string[] array2 = new string[5]
                        {
                            array[0],
                            null,
                            null,
                            null,
                            null
                        };
                        string text4 = "-";
                        array2[1] = text4;
                        array2[2] = array[1];
                        string text5 = "-";
                        array2[3] = text5;
                        array2[4] = array[2];
                        dataRow2[columnName3] = string.Concat(array2);
                    }
                    catch
                    {
                        try
                        {
                            DataRow dataRow3 = dataTable_0.Rows[num];
                            string columnName4 = "currtotal";
                            string value = "error";
                            dataRow3[columnName4] = value;
                        }
                        catch
                        {
                        }
                    }
                    num++;
                }
            }
            string object_ = "Process Complete";
            method_3(object_);
            method_2(false);
        }
        catch (Exception)
        {
        }
    }

    private void backgroundWorker_0_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        lblMsg.Visible = false;
        pictureBox1.Visible = false;
        method_1();
    }

    private void backgroundWorker_1_DoWork(object sender, DoWorkEventArgs e)
    {
        try
        {
            method_2(true);
            DataTable dataTable = new DataTable();
            string commandText = "SELECT * FROM TBL_USER";
            new SQLiteDataAdapter(commandText, Class34.string_Database).Fill(dataTable);
            int i = 0;
            string prompt = "Enter Your Specific Password\r\nPlease Enter Atleast 1 Lower, 1 Upper, 1 Numeric";
            string text = Interaction.InputBox(prompt);
            bool flag = text.Any(char.IsUpper) && text.Any(char.IsLower) && text.Any(char.IsDigit);
            if (text.Length >= 8 && flag)
            {
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    lblMsg.Visible = true;
                    {
                        IEnumerator enumerator = dataTable.Rows.GetEnumerator();
                        try
                        {
                            for (; enumerator.MoveNext(); i++)
                            {
                                DataRow dataRow = (DataRow)enumerator.Current;
                                string text2 = "Updating Id Records: ";
                                string columnName = "UserName";
                                method_3(text2 + dataRow[columnName].ToString());
                                try
                                {
                                    string columnName2 = "UserName";
                                    string userID = dataRow[columnName2].ToString();
                                    string columnName3 = "strPwd";
                                    string pwd = dataRow[columnName3].ToString();
                                    string text3 = "Password:";
                                    string errMessage = text3 + text;
                                    if (bool.Parse(Program.mcommInstance_.isLV(userID, pwd, ref errMessage)))
                                    {
                                        DataRow dataRow2 = dataTable_0.Rows[i];
                                        string columnName4 = "Pwd";
                                        dataRow2[columnName4] = text;
                                        continue;
                                    }
                                    string text4 = "Please Enter Atleast 1 Lower, 1 Upper, 1 Numeric";
                                    MessageBox.Show(text4);
                                }
                                catch
                                {
                                    continue;
                                }
                                break;
                            }
                        }
                        finally
                        {
                            IDisposable disposable = enumerator as IDisposable;
                            if (disposable != null)
                            {
                                disposable.Dispose();
                            }
                        }
                    }
                    lblMsg.Visible = false;
                }
                method_2(false);
                Form owner = new Form
                {
                    TopMost = true
                };
                string text5 = "IRCTC Password Update Successful\r\nNeed to Restart Application";
                string caption = "Success";
                MessageBox.Show(owner, text5, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Environment.Exit(0);
            }
            else
            {
                string text6 = "Password Should be Min 8 Character Long\r\nPlease Enter Atleast 1 Lower, 1 Upper, 1 Numeric";
                MessageBox.Show(text6);
            }
        }
        catch (Exception)
        {
        }
    }

    private void B0AAA(object sender, RunWorkerCompletedEventArgs e)
    {
        method_1();
    }

    private void C0AA0(object sender, DoWorkEventArgs e)
    {
        method_2(true);
        DataTable dataTable = new DataTable();
        string commandText = "SELECT * FROM TBL_USER WHERE UserState LIKE 'Active||%'";
        new SQLiteDataAdapter(commandText, Class34.string_Database).Fill(dataTable);
        int num = 0;
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                num++;
                string text = "Updating Id Records: ";
                method_3(text + num);
                try
                {
                    string columnName = "UserName";
                    string userID = row[columnName].ToString();
                    string columnName2 = "strPwd";
                    string pwd = row[columnName2].ToString();
                    string text2 = "Answer:";
                    string prompt = "Enter Your Specific Password";
                    string errMessage = text2 + Interaction.InputBox(prompt);
                    string text3 = Program.mcommInstance_.isLV(userID, pwd, ref errMessage);
                    string text4 = "Sucess";
                    _ = text3 == text4;
                }
                catch
                {
                }
            }
        }
        method_2(false);
    }

    private void backgroundWorker_2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        method_1();
    }

    private void textmob_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
        {
            e.Handled = true;
        }
    }


    // -----------------------------------------------------------------
    // ByteNova Latest UI
    // Visual-only layer: existing IRCTC ID logic and event handlers remain intact.
    // -----------------------------------------------------------------
    private void ApplyByteNovaLatestUI()
    {
        SuspendLayout();

        BackColor = Color.FromArgb(3, 18, 32);
        ForeColor = Color.White;
        Font = new Font("Segoe UI", 8.25F, FontStyle.Regular);

        StyleByteNovaGroup(grpAddID);
        StyleByteNovaGroup(groupBox1);

        StyleByteNovaGrid(grdUsers);

        StyleByteNovaTextBox(txtLogin);
        StyleByteNovaTextBox(txtPassword);
        StyleByteNovaTextBox(textmob);
        StyleByteNovaTextBox(textmpass);
        StyleByteNovaTextBox(txtPnr);

        txtbulk.BackColor = Color.FromArgb(8, 28, 44);
        txtbulk.ForeColor = Color.White;
        txtbulk.BorderStyle = BorderStyle.FixedSingle;
        txtbulk.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular);

        StyleByteNovaButton(btnIDSave, true);
        StyleByteNovaButton(C1AA0, false);
        StyleByteNovaButton(btnChangePwd, false);
        StyleByteNovaButton(btnChangeQn, false);
        StyleByteNovaButton(btnHideDay, false);
        StyleByteNovaButton(button2, true);
        StyleByteNovaButton(C2AA0, false);
        StyleByteNovaButton(button3, false);
        StyleByteNovaButton(button4, false);
        StyleByteNovaButton(button5, false);
        StyleByteNovaButton(button1, false);

        StyleByteNovaDate(B2AAA);
        StyleByteNovaDate(dtDate2);

        CmbDays.BackColor = Color.FromArgb(8, 28, 44);
        CmbDays.ForeColor = Color.White;
        CmbDays.FlatStyle = FlatStyle.Flat;
        CmbDays.Font = new Font("Segoe UI", 8F, FontStyle.Regular);

        StyleByteNovaLabel(label37);
        StyleByteNovaLabel(lblPassword);
        StyleByteNovaLabel(label4);
        StyleByteNovaLabel(label5);
        StyleByteNovaLabel(label14);
        StyleByteNovaLabel(label6);
        StyleByteNovaLabel(label7);
        StyleByteNovaLabel(label9);
        StyleByteNovaLabel(label10);
        StyleByteNovaLabel(label11);
        StyleByteNovaLabel(label2);
        StyleByteNovaLabel(label3);
        StyleByteNovaLabel(label1);
        StyleByteNovaLabel(B1AAA);
        StyleByteNovaLabel(B3AAA);
        StyleByteNovaLabel(C3AA0);

        StyleByteNovaCounter(lblTotal);
        StyleByteNovaCounter(lblActive);
        StyleByteNovaCounter(lbl7);
        StyleByteNovaCounter(lbl6);
        StyleByteNovaCounter(lblTobk);
        StyleByteNovaCounter(lblCurMonbk);
        StyleByteNovaCounter(lblDisableCount);
        StyleByteNovaCounter(lblInValid);
        StyleByteNovaCounter(lblUnVerified);

        StyleByteNovaCheck(chkValidate);
        StyleByteNovaCheck(chkShowPwd);
        StyleByteNovaCheck(checkBox1);

        StyleByteNovaRadio(RadioBdate);
        StyleByteNovaRadio(RadioJdate);
        StyleByteNovaRadio(RadioPnr);

        panel13.BackColor = Color.FromArgb(5, 25, 42);
        panel13.Paint -= ByteNovaPanelPaint;
        panel13.Paint += ByteNovaPanelPaint;

        pnlloader.BackColor = Color.FromArgb(3, 18, 32);
        lblMsg.ForeColor = Color.FromArgb(0, 225, 255);
        lblMsg.BackColor = Color.Transparent;
        lblMsg.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);

        Paint -= ByteNovaControlPaint;
        Paint += ByteNovaControlPaint;

        ResumeLayout(false);
        PerformLayout();
        Invalidate(true);
    }

    private void StyleByteNovaGrid(DataGridView grid)
    {
        grid.EnableHeadersVisualStyles = false;
        grid.BackgroundColor = Color.FromArgb(4, 20, 34);
        grid.BorderStyle = BorderStyle.None;
        grid.GridColor = Color.FromArgb(32, 66, 83);
        grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        grid.RowHeadersVisible = false;

        grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(5, 37, 59);
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(210, 245, 255);
        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(5, 37, 59);
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
        grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
        grid.ColumnHeadersHeight = 27;

        // Keep ALL values visible even when the cell is not selected.
        grid.DefaultCellStyle.BackColor = Color.FromArgb(7, 25, 39);
        grid.DefaultCellStyle.ForeColor = Color.FromArgb(230, 241, 248);
        grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(12, 74, 102);
        grid.DefaultCellStyle.SelectionForeColor = Color.White;
        grid.DefaultCellStyle.Font = new Font("Segoe UI", 8F, FontStyle.Regular);

        grid.RowsDefaultCellStyle.BackColor = Color.FromArgb(7, 25, 39);
        grid.RowsDefaultCellStyle.ForeColor = Color.FromArgb(230, 241, 248);
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(5, 31, 47);
        grid.AlternatingRowsDefaultCellStyle.ForeColor = Color.FromArgb(230, 241, 248);

        grid.RowTemplate.Height = 24;
        grid.SelectionMode = DataGridViewSelectionMode.CellSelect;

        foreach (DataGridViewColumn col in grid.Columns)
        {
            col.DefaultCellStyle.BackColor = Color.Empty;
            col.DefaultCellStyle.ForeColor = Color.FromArgb(230, 241, 248);
            col.DefaultCellStyle.SelectionBackColor = Color.FromArgb(12, 74, 102);
            col.DefaultCellStyle.SelectionForeColor = Color.White;

            DataGridViewLinkColumn link = col as DataGridViewLinkColumn;
            if (link != null)
            {
                link.LinkColor = Color.FromArgb(198, 244, 50);
                link.ActiveLinkColor = Color.White;
                link.VisitedLinkColor = Color.FromArgb(198, 244, 50);
                link.TrackVisitedState = false;
                link.DefaultCellStyle.ForeColor = Color.FromArgb(198, 244, 50);
                link.DefaultCellStyle.SelectionForeColor = Color.White;
            }
        }

        grid.Invalidate();
    }

    private void StyleByteNovaTextBox(TextBox box)
    {
        box.BackColor = Color.FromArgb(8, 28, 44);
        box.ForeColor = Color.White;
        box.BorderStyle = BorderStyle.FixedSingle;
        box.Font = new Font("Segoe UI Semibold", 8.25F, FontStyle.Regular);
    }

    private void StyleByteNovaButton(Button button, bool primary)
    {
        if (button == null) return;
        button.UseVisualStyleBackColor = false;
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.BorderColor = primary
            ? Color.FromArgb(198, 244, 50)
            : Color.FromArgb(35, 125, 165);
        button.FlatAppearance.MouseOverBackColor = Color.FromArgb(10, 80, 110);
        button.FlatAppearance.MouseDownBackColor = Color.FromArgb(6, 55, 78);
        button.BackColor = primary
            ? Color.FromArgb(0, 111, 158)
            : Color.FromArgb(5, 40, 62);
        button.ForeColor = Color.White;
        button.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
    }

    private void StyleByteNovaGroup(GroupBox group)
    {
        if (group == null) return;
        group.BackColor = Color.FromArgb(3, 18, 32);
        group.ForeColor = Color.FromArgb(198, 244, 50);
        group.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
        group.Paint -= ByteNovaGroupPaint;
        group.Paint += ByteNovaGroupPaint;
    }

    private void StyleByteNovaDate(DateTimePicker picker)
    {
        picker.CalendarForeColor = Color.White;
        picker.CalendarMonthBackground = Color.FromArgb(8, 28, 44);
        picker.CalendarTitleBackColor = Color.FromArgb(0, 95, 135);
        picker.CalendarTitleForeColor = Color.White;
        picker.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
    }

    private void StyleByteNovaLabel(Label label)
    {
        if (label == null) return;
        label.ForeColor = Color.FromArgb(190, 225, 240);
        label.BackColor = Color.Transparent;
        label.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
    }

    private void StyleByteNovaCounter(Label label)
    {
        if (label == null) return;
        label.ForeColor = Color.FromArgb(0, 235, 255);
        label.BackColor = Color.Transparent;
        label.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
    }

    private void StyleByteNovaCheck(CheckBox check)
    {
        if (check == null) return;
        check.ForeColor = Color.FromArgb(215, 238, 248);
        check.BackColor = Color.Transparent;
        check.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
    }

    private void StyleByteNovaRadio(RadioButton radio)
    {
        if (radio == null) return;
        radio.ForeColor = Color.FromArgb(215, 238, 248);
        radio.BackColor = Color.Transparent;
        radio.Font = new Font("Segoe UI", 8F, FontStyle.Regular);
    }

    private void ByteNovaControlPaint(object sender, PaintEventArgs e)
    {
        Rectangle r = new Rectangle(0, 0, Width - 1, Height - 1);
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r,
            Color.FromArgb(4, 25, 43),
            Color.FromArgb(1, 10, 20),
            LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, r);

        using (Pen p = new Pen(Color.FromArgb(0, 170, 220), 1F))
            e.Graphics.DrawRectangle(p, r);
    }

    private void ByteNovaPanelPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;
        Rectangle r = new Rectangle(0, 0, c.Width - 1, c.Height - 1);
        using (Pen p = new Pen(Color.FromArgb(0, 120, 175), 1F))
            e.Graphics.DrawRectangle(p, r);
    }

    private void ByteNovaGroupPaint(object sender, PaintEventArgs e)
    {
        GroupBox group = sender as GroupBox;
        if (group == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(1, 7, group.Width - 3, group.Height - 9);
        using (Pen p = new Pen(Color.FromArgb(0, 115, 165), 1F))
            e.Graphics.DrawRectangle(p, r);

        SizeF textSize = e.Graphics.MeasureString(group.Text, group.Font);
        using (Brush erase = new SolidBrush(group.BackColor))
            e.Graphics.FillRectangle(erase, 8, 0, textSize.Width + 8, 16);
        using (Brush textBrush = new SolidBrush(Color.FromArgb(0, 225, 255)))
            e.Graphics.DrawString(group.Text, group.Font, textBrush, 11, 0);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && idisposable_0 != null)
        {
            idisposable_0.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
        this.RadioBdate = new System.Windows.Forms.RadioButton();
        this.B2AAA = new System.Windows.Forms.DateTimePicker();
        this.RadioPnr = new System.Windows.Forms.RadioButton();
        this.RadioJdate = new System.Windows.Forms.RadioButton();
        this.C2AA0 = new System.Windows.Forms.Button();
        this.button2 = new System.Windows.Forms.Button();
        this.txtPnr = new System.Windows.Forms.TextBox();
        this.dtDate2 = new System.Windows.Forms.DateTimePicker();
        this.grdUsers = new System.Windows.Forms.DataGridView();
        this.clmID = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.clmPwd = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.clmMTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.clmCheck = new System.Windows.Forms.DataGridViewLinkColumn();
        this.clmAvtive = new System.Windows.Forms.DataGridViewLinkColumn();
        this.clmDelete = new System.Windows.Forms.DataGridViewLinkColumn();
        this.ClmHis = new System.Windows.Forms.DataGridViewLinkColumn();
        this.grpAddID = new System.Windows.Forms.GroupBox();
        this.checkBox1 = new System.Windows.Forms.CheckBox();
        this.textmob = new System.Windows.Forms.TextBox();
        this.label4 = new System.Windows.Forms.Label();
        this.textmpass = new System.Windows.Forms.TextBox();
        this.label5 = new System.Windows.Forms.Label();
        this.txtLogin = new System.Windows.Forms.TextBox();
        this.label37 = new System.Windows.Forms.Label();
        this.chkShowPwd = new System.Windows.Forms.CheckBox();
        this.btnIDSave = new System.Windows.Forms.Button();
        this.txtPassword = new System.Windows.Forms.TextBox();
        this.lblPassword = new System.Windows.Forms.Label();
        this.chkValidate = new System.Windows.Forms.CheckBox();
        this.lblMsg = new System.Windows.Forms.Label();
        this.button5 = new System.Windows.Forms.Button();
        this.btnHideDay = new System.Windows.Forms.Button();
        this.CmbDays = new System.Windows.Forms.ComboBox();
        this.label14 = new System.Windows.Forms.Label();
        this.btnChangeQn = new System.Windows.Forms.Button();
        this.btnChangePwd = new System.Windows.Forms.Button();
        this.C1AA0 = new System.Windows.Forms.Button();
        this.B1AAA = new System.Windows.Forms.Label();
        this.lblTotal = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.lblActive = new System.Windows.Forms.Label();
        this.toolTip_0 = new System.Windows.Forms.ToolTip(this.components);
        this.groupBox1 = new System.Windows.Forms.GroupBox();
        this.panel13 = new System.Windows.Forms.Panel();
        this.lbl7 = new System.Windows.Forms.Label();
        this.B3AAA = new System.Windows.Forms.Label();
        this.lblCurMonbk = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.lbl6 = new System.Windows.Forms.Label();
        this.C3AA0 = new System.Windows.Forms.Label();
        this.lblTobk = new System.Windows.Forms.Label();
        this.label1 = new System.Windows.Forms.Label();
        this.pnlloader = new System.Windows.Forms.Panel();
        this.backgroundWorker_0 = new System.ComponentModel.BackgroundWorker();
        this.backgroundWorker_1 = new System.ComponentModel.BackgroundWorker();
        this.backgroundWorker_2 = new System.ComponentModel.BackgroundWorker();
        this.pictureBox1 = new System.Windows.Forms.PictureBox();
        this.bgwkverify_ = new System.ComponentModel.BackgroundWorker();
        this.button3 = new System.Windows.Forms.Button();
        this.txtbulk = new System.Windows.Forms.RichTextBox();
        this.label6 = new System.Windows.Forms.Label();
        this.button4 = new System.Windows.Forms.Button();
        this.label7 = new System.Windows.Forms.Label();
        this.lblDisableCount = new System.Windows.Forms.Label();
        this.lblInValid = new System.Windows.Forms.Label();
        this.label9 = new System.Windows.Forms.Label();
        this.lblUnVerified = new System.Windows.Forms.Label();
        this.label11 = new System.Windows.Forms.Label();
        this.label10 = new System.Windows.Forms.Label();
        this.button1 = new System.Windows.Forms.Button();
        ((System.ComponentModel.ISupportInitialize)(this.grdUsers)).BeginInit();
        this.grpAddID.SuspendLayout();
        this.groupBox1.SuspendLayout();
        this.panel13.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
        this.SuspendLayout();
        // 
        // RadioBdate
        // 
        this.RadioBdate.AutoSize = true;
        this.RadioBdate.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.RadioBdate.ForeColor = System.Drawing.Color.White;
        this.RadioBdate.Location = new System.Drawing.Point(2, 46);
        this.RadioBdate.Name = "RadioBdate";
        this.RadioBdate.Size = new System.Drawing.Size(101, 19);
        this.RadioBdate.TabIndex = 34;
        this.RadioBdate.TabStop = true;
        this.RadioBdate.Text = "ByBookDate:";
        this.RadioBdate.UseVisualStyleBackColor = true;
        // 
        // B2AAA
        // 
        this.B2AAA.Checked = false;
        this.B2AAA.CustomFormat = "dd-MMM-yyyy";
        this.B2AAA.Enabled = false;
        this.B2AAA.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.B2AAA.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
        this.B2AAA.Location = new System.Drawing.Point(105, 46);
        this.B2AAA.MaxDate = new System.DateTime(2020, 12, 31, 0, 0, 0, 0);
        this.B2AAA.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
        this.B2AAA.Name = "B2AAA";
        this.B2AAA.Size = new System.Drawing.Size(103, 20);
        this.B2AAA.TabIndex = 33;
        this.B2AAA.Value = new System.DateTime(2020, 12, 31, 0, 0, 0, 0);
        // 
        // RadioPnr
        // 
        this.RadioPnr.AutoSize = true;
        this.RadioPnr.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.RadioPnr.ForeColor = System.Drawing.Color.White;
        this.RadioPnr.Location = new System.Drawing.Point(433, 48);
        this.RadioPnr.Name = "RadioPnr";
        this.RadioPnr.Size = new System.Drawing.Size(64, 19);
        this.RadioPnr.TabIndex = 32;
        this.RadioPnr.TabStop = true;
        this.RadioPnr.Text = "ByPnr:";
        this.RadioPnr.UseVisualStyleBackColor = true;
        // 
        // RadioJdate
        // 
        this.RadioJdate.AutoSize = true;
        this.RadioJdate.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.RadioJdate.ForeColor = System.Drawing.Color.White;
        this.RadioJdate.Location = new System.Drawing.Point(213, 47);
        this.RadioJdate.Name = "RadioJdate";
        this.RadioJdate.Size = new System.Drawing.Size(120, 19);
        this.RadioJdate.TabIndex = 31;
        this.RadioJdate.TabStop = true;
        this.RadioJdate.Text = "ByJourneyDate: ";
        this.RadioJdate.UseVisualStyleBackColor = true;
        // 
        // C2AA0
        // 
        this.C2AA0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(146)))));
        this.C2AA0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.C2AA0.Font = new System.Drawing.Font("Corbel", 11.25F);
        this.C2AA0.ForeColor = System.Drawing.Color.White;
        this.C2AA0.Location = new System.Drawing.Point(696, 44);
        this.C2AA0.Name = "C2AA0";
        this.C2AA0.Size = new System.Drawing.Size(54, 24);
        this.C2AA0.TabIndex = 30;
        this.C2AA0.Text = "Clear";
        this.C2AA0.UseVisualStyleBackColor = false;
        // 
        // button2
        // 
        this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(146)))));
        this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.button2.Font = new System.Drawing.Font("Corbel", 11.25F);
        this.button2.ForeColor = System.Drawing.Color.White;
        this.button2.Location = new System.Drawing.Point(611, 44);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(80, 24);
        this.button2.TabIndex = 30;
        this.button2.Text = "Search";
        this.button2.UseVisualStyleBackColor = false;
        // 
        // txtPnr
        // 
        this.txtPnr.Enabled = false;
        this.txtPnr.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtPnr.Location = new System.Drawing.Point(501, 46);
        this.txtPnr.MaxLength = 50;
        this.txtPnr.Name = "txtPnr";
        this.txtPnr.Size = new System.Drawing.Size(104, 21);
        this.txtPnr.TabIndex = 2;
        // 
        // dtDate2
        // 
        this.dtDate2.Checked = false;
        this.dtDate2.CustomFormat = "dd-MMM-yyyy";
        this.dtDate2.Enabled = false;
        this.dtDate2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.dtDate2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
        this.dtDate2.Location = new System.Drawing.Point(322, 46);
        this.dtDate2.MaxDate = new System.DateTime(2020, 12, 31, 0, 0, 0, 0);
        this.dtDate2.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
        this.dtDate2.Name = "dtDate2";
        this.dtDate2.Size = new System.Drawing.Size(105, 20);
        this.dtDate2.TabIndex = 27;
        this.dtDate2.Value = new System.DateTime(2020, 12, 31, 0, 0, 0, 0);
        // 
        // grdUsers
        // 
        this.grdUsers.AllowUserToAddRows = false;
        this.grdUsers.AllowUserToDeleteRows = false;
        this.grdUsers.AllowUserToResizeColumns = false;
        this.grdUsers.AllowUserToResizeRows = false;
        this.grdUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
        dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
        dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
        dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        this.grdUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        this.grdUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.grdUsers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.clmID,
            this.clmPwd,
            this.clmMTotal,
            this.clmCheck,
            this.clmAvtive,
            this.clmDelete,
            this.ClmHis});
        dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
        dataGridViewCellStyle6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
        dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
        this.grdUsers.DefaultCellStyle = dataGridViewCellStyle6;
        this.grdUsers.GridColor = System.Drawing.SystemColors.Control;
        this.grdUsers.Location = new System.Drawing.Point(0, 49);
        this.grdUsers.MultiSelect = false;
        this.grdUsers.Name = "grdUsers";
        this.grdUsers.ReadOnly = true;
        this.grdUsers.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
        this.grdUsers.RowHeadersVisible = false;
        this.grdUsers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.grdUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
        this.grdUsers.Size = new System.Drawing.Size(757, 285);
        this.grdUsers.TabIndex = 18;
        this.grdUsers.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdUsers_CellContentClick);
        // 
        // clmID
        // 
        this.clmID.DataPropertyName = "userid";
        dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
        dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
        this.clmID.DefaultCellStyle = dataGridViewCellStyle2;
        this.clmID.FillWeight = 125F;
        this.clmID.HeaderText = "User ID";
        this.clmID.MaxInputLength = 20;
        this.clmID.Name = "clmID";
        this.clmID.ReadOnly = true;
        this.clmID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
        // 
        // clmPwd
        // 
        this.clmPwd.DataPropertyName = "Pwd";
        this.clmPwd.HeaderText = "Password";
        this.clmPwd.Name = "clmPwd";
        this.clmPwd.ReadOnly = true;
        // 
        // clmMTotal
        // 
        this.clmMTotal.DataPropertyName = "currtotal";
        this.clmMTotal.HeaderText = "Total Pnr";
        this.clmMTotal.Name = "clmMTotal";
        this.clmMTotal.ReadOnly = true;
        this.clmMTotal.Resizable = System.Windows.Forms.DataGridViewTriState.True;
        // 
        // clmCheck
        // 
        this.clmCheck.DataPropertyName = "sync";
        dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
        this.clmCheck.DefaultCellStyle = dataGridViewCellStyle3;
        this.clmCheck.HeaderText = "Mobile num";
        this.clmCheck.Name = "clmCheck";
        this.clmCheck.ReadOnly = true;
        this.clmCheck.Resizable = System.Windows.Forms.DataGridViewTriState.False;
        this.clmCheck.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
        // 
        // clmAvtive
        // 
        this.clmAvtive.DataPropertyName = "state";
        dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
        this.clmAvtive.DefaultCellStyle = dataGridViewCellStyle4;
        this.clmAvtive.HeaderText = "Action";
        this.clmAvtive.Name = "clmAvtive";
        this.clmAvtive.ReadOnly = true;
        this.clmAvtive.Resizable = System.Windows.Forms.DataGridViewTriState.False;
        this.clmAvtive.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
        this.clmAvtive.TrackVisitedState = false;
        // 
        // clmDelete
        // 
        this.clmDelete.DataPropertyName = "delete";
        dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.White;
        this.clmDelete.DefaultCellStyle = dataGridViewCellStyle5;
        this.clmDelete.FillWeight = 80F;
        this.clmDelete.HeaderText = "Delete";
        this.clmDelete.Name = "clmDelete";
        this.clmDelete.ReadOnly = true;
        this.clmDelete.Resizable = System.Windows.Forms.DataGridViewTriState.False;
        this.clmDelete.Text = "Delete";
        this.clmDelete.TrackVisitedState = false;
        // 
        // ClmHis
        // 
        this.ClmHis.DataPropertyName = "history";
        this.ClmHis.HeaderText = "History";
        this.ClmHis.Name = "ClmHis";
        this.ClmHis.ReadOnly = true;
        this.ClmHis.Resizable = System.Windows.Forms.DataGridViewTriState.True;
        this.ClmHis.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
        // 
        // grpAddID
        // 
        this.grpAddID.Controls.Add(this.checkBox1);
        this.grpAddID.Controls.Add(this.textmob);
        this.grpAddID.Controls.Add(this.label4);
        this.grpAddID.Controls.Add(this.textmpass);
        this.grpAddID.Controls.Add(this.label5);
        this.grpAddID.Controls.Add(this.txtLogin);
        this.grpAddID.Controls.Add(this.label37);
        this.grpAddID.Controls.Add(this.chkShowPwd);
        this.grpAddID.Controls.Add(this.btnIDSave);
        this.grpAddID.Controls.Add(this.txtPassword);
        this.grpAddID.Controls.Add(this.lblPassword);
        this.grpAddID.Controls.Add(this.chkValidate);
        this.grpAddID.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.grpAddID.Location = new System.Drawing.Point(0, 5);
        this.grpAddID.Name = "grpAddID";
        this.grpAddID.Size = new System.Drawing.Size(757, 46);
        this.grpAddID.TabIndex = 1;
        this.grpAddID.TabStop = false;
        // 
        // checkBox1
        // 
        this.checkBox1.AutoSize = true;
        this.checkBox1.Location = new System.Drawing.Point(213, 17);
        this.checkBox1.Name = "checkBox1";
        this.checkBox1.Size = new System.Drawing.Size(89, 19);
        this.checkBox1.TabIndex = 136;
        this.checkBox1.Text = "Add Bulk ID";
        this.checkBox1.UseVisualStyleBackColor = true;
        this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
        // 
        // textmob
        // 
        this.textmob.Enabled = false;
        this.textmob.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.textmob.Location = new System.Drawing.Point(862, 29);
        this.textmob.MaxLength = 10;
        this.textmob.Name = "textmob";
        this.textmob.Size = new System.Drawing.Size(115, 22);
        this.textmob.TabIndex = 27;
        this.textmob.Text = "9820000000";
        // 
        // label4
        // 
        this.label4.Location = new System.Drawing.Point(798, 30);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(71, 26);
        this.label4.TabIndex = 29;
        this.label4.Text = "Mobile No: ";
        this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // textmpass
        // 
        this.textmpass.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.textmpass.Location = new System.Drawing.Point(1191, 29);
        this.textmpass.MaxLength = 100;
        this.textmpass.Name = "textmpass";
        this.textmpass.Size = new System.Drawing.Size(139, 22);
        this.textmpass.TabIndex = 28;
        this.textmpass.Visible = false;
        // 
        // label5
        // 
        this.label5.Location = new System.Drawing.Point(1071, 30);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(114, 26);
        this.label5.TabIndex = 26;
        this.label5.Text = "Email Password: ";
        this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        this.label5.Visible = false;
        // 
        // txtLogin
        // 
        this.txtLogin.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtLogin.Location = new System.Drawing.Point(59, 14);
        this.txtLogin.MaxLength = 50;
        this.txtLogin.Name = "txtLogin";
        this.txtLogin.Size = new System.Drawing.Size(139, 22);
        this.txtLogin.TabIndex = 7;
        // 
        // label37
        // 
        this.label37.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.label37.ForeColor = System.Drawing.Color.Black;
        this.label37.Location = new System.Drawing.Point(3, 11);
        this.label37.Name = "label37";
        this.label37.Size = new System.Drawing.Size(93, 26);
        this.label37.TabIndex = 15;
        this.label37.Text = "User ID: ";
        this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // chkShowPwd
        // 
        this.chkShowPwd.AutoSize = true;
        this.chkShowPwd.Checked = true;
        this.chkShowPwd.CheckState = System.Windows.Forms.CheckState.Checked;
        this.chkShowPwd.ForeColor = System.Drawing.Color.Black;
        this.chkShowPwd.Location = new System.Drawing.Point(313, 122);
        this.chkShowPwd.Name = "chkShowPwd";
        this.chkShowPwd.Size = new System.Drawing.Size(116, 19);
        this.chkShowPwd.TabIndex = 11;
        this.chkShowPwd.Text = "&Show Password";
        this.chkShowPwd.UseVisualStyleBackColor = true;
        // 
        // btnIDSave
        // 
        this.btnIDSave.BackColor = System.Drawing.SystemColors.ActiveCaption;
        this.btnIDSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnIDSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
        this.btnIDSave.Location = new System.Drawing.Point(546, 14);
        this.btnIDSave.Name = "btnIDSave";
        this.btnIDSave.Size = new System.Drawing.Size(204, 26);
        this.btnIDSave.TabIndex = 9;
        this.btnIDSave.Text = "Add IRCTC";
        this.btnIDSave.UseVisualStyleBackColor = false;
        this.btnIDSave.Click += new System.EventHandler(this.btnIDSave_Click_1);
        // 
        // txtPassword
        // 
        this.txtPassword.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.txtPassword.Location = new System.Drawing.Point(388, 14);
        this.txtPassword.MaxLength = 50;
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.PasswordChar = '*';
        this.txtPassword.Size = new System.Drawing.Size(139, 22);
        this.txtPassword.TabIndex = 8;
        // 
        // lblPassword
        // 
        this.lblPassword.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.lblPassword.ForeColor = System.Drawing.Color.Black;
        this.lblPassword.Location = new System.Drawing.Point(313, 11);
        this.lblPassword.Name = "lblPassword";
        this.lblPassword.Size = new System.Drawing.Size(114, 26);
        this.lblPassword.TabIndex = 3;
        this.lblPassword.Text = "Password: ";
        this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // chkValidate
        // 
        this.chkValidate.AutoSize = true;
        this.chkValidate.ForeColor = System.Drawing.Color.Black;
        this.chkValidate.Location = new System.Drawing.Point(6, 122);
        this.chkValidate.Name = "chkValidate";
        this.chkValidate.Size = new System.Drawing.Size(270, 19);
        this.chkValidate.TabIndex = 10;
        this.chkValidate.Text = "&Validate from IRCTC (Strongly Recomended)";
        this.chkValidate.UseVisualStyleBackColor = true;
        // 
        // lblMsg
        // 
        this.lblMsg.AutoSize = true;
        this.lblMsg.Location = new System.Drawing.Point(328, 398);
        this.lblMsg.Name = "lblMsg";
        this.lblMsg.Size = new System.Drawing.Size(141, 13);
        this.lblMsg.TabIndex = 25;
        this.lblMsg.Text = "Updating Id Records 0 of 10";
        this.lblMsg.Visible = false;
        // 
        // button5
        // 
        this.button5.BackColor = System.Drawing.Color.Transparent;
        this.button5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.button5.Location = new System.Drawing.Point(812, 653);
        this.button5.Name = "button5";
        this.button5.Size = new System.Drawing.Size(128, 26);
        this.button5.TabIndex = 12;
        this.button5.Text = "Create New Irctc-ID";
        this.button5.UseVisualStyleBackColor = false;
        // 
        // btnHideDay
        // 
        this.btnHideDay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(146)))));
        this.btnHideDay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnHideDay.Font = new System.Drawing.Font("Corbel", 11.25F);
        this.btnHideDay.ForeColor = System.Drawing.Color.White;
        this.btnHideDay.Location = new System.Drawing.Point(662, 15);
        this.btnHideDay.Name = "btnHideDay";
        this.btnHideDay.Size = new System.Drawing.Size(88, 23);
        this.btnHideDay.TabIndex = 17;
        this.btnHideDay.Text = "Save Days";
        this.btnHideDay.UseVisualStyleBackColor = false;
        // 
        // CmbDays
        // 
        this.CmbDays.FormattingEnabled = true;
        this.CmbDays.Items.AddRange(new object[] {
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
            "10"});
        this.CmbDays.Location = new System.Drawing.Point(574, 15);
        this.CmbDays.Name = "CmbDays";
        this.CmbDays.Size = new System.Drawing.Size(63, 23);
        this.CmbDays.TabIndex = 16;
        // 
        // label14
        // 
        this.label14.AutoSize = true;
        this.label14.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.label14.ForeColor = System.Drawing.Color.White;
        this.label14.Location = new System.Drawing.Point(430, 18);
        this.label14.Name = "label14";
        this.label14.Size = new System.Drawing.Size(143, 15);
        this.label14.TabIndex = 1177;
        this.label14.Text = "Hide IRCTC Id for Days :";
        // 
        // btnChangeQn
        // 
        this.btnChangeQn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(146)))));
        this.btnChangeQn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnChangeQn.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
        this.btnChangeQn.ForeColor = System.Drawing.Color.White;
        this.btnChangeQn.Location = new System.Drawing.Point(286, 13);
        this.btnChangeQn.Name = "btnChangeQn";
        this.btnChangeQn.Size = new System.Drawing.Size(128, 24);
        this.btnChangeQn.TabIndex = 15;
        this.btnChangeQn.Text = "Change ALL ID Ans";
        this.btnChangeQn.UseVisualStyleBackColor = false;
        // 
        // btnChangePwd
        // 
        this.btnChangePwd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(49)))), ((int)(((byte)(146)))));
        this.btnChangePwd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnChangePwd.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
        this.btnChangePwd.ForeColor = System.Drawing.Color.White;
        this.btnChangePwd.Location = new System.Drawing.Point(152, 13);
        this.btnChangePwd.Name = "btnChangePwd";
        this.btnChangePwd.Size = new System.Drawing.Size(128, 24);
        this.btnChangePwd.TabIndex = 14;
        this.btnChangePwd.Text = "Change ALL ID PWD";
        this.btnChangePwd.UseVisualStyleBackColor = false;
        // 
        // C1AA0
        // 
        this.C1AA0.BackColor = System.Drawing.SystemColors.ButtonShadow;
        this.C1AA0.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.C1AA0.Location = new System.Drawing.Point(537, 393);
        this.C1AA0.Name = "C1AA0";
        this.C1AA0.Size = new System.Drawing.Size(67, 45);
        this.C1AA0.TabIndex = 13;
        this.C1AA0.Text = "Sync ALL ID\'S";
        this.C1AA0.UseVisualStyleBackColor = false;
        this.C1AA0.Click += new System.EventHandler(this.C1AA0_Click_1);
        // 
        // B1AAA
        // 
        this.B1AAA.AutoSize = true;
        this.B1AAA.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.B1AAA.Location = new System.Drawing.Point(3, 397);
        this.B1AAA.Name = "B1AAA";
        this.B1AAA.Size = new System.Drawing.Size(64, 15);
        this.B1AAA.TabIndex = 9;
        this.B1AAA.Text = "Total IDs: ";
        // 
        // lblTotal
        // 
        this.lblTotal.AutoSize = true;
        this.lblTotal.BackColor = System.Drawing.Color.Black;
        this.lblTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
        this.lblTotal.ForeColor = System.Drawing.Color.Yellow;
        this.lblTotal.Location = new System.Drawing.Point(68, 397);
        this.lblTotal.Name = "lblTotal";
        this.lblTotal.Size = new System.Drawing.Size(17, 16);
        this.lblTotal.TabIndex = 13;
        this.lblTotal.Text = "T";
        this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.label2.Location = new System.Drawing.Point(259, 24);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(99, 15);
        this.label2.TabIndex = 10;
        this.label2.Text = "Total Active IDs:";
        // 
        // lblActive
        // 
        this.lblActive.AutoSize = true;
        this.lblActive.BackColor = System.Drawing.Color.Black;
        this.lblActive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
        this.lblActive.ForeColor = System.Drawing.Color.Yellow;
        this.lblActive.Location = new System.Drawing.Point(233, 397);
        this.lblActive.Name = "lblActive";
        this.lblActive.Size = new System.Drawing.Size(17, 16);
        this.lblActive.TabIndex = 12;
        this.lblActive.Text = "T";
        this.lblActive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // groupBox1
        // 
        this.groupBox1.Controls.Add(this.btnHideDay);
        this.groupBox1.Controls.Add(this.dtDate2);
        this.groupBox1.Controls.Add(this.CmbDays);
        this.groupBox1.Controls.Add(this.C2AA0);
        this.groupBox1.Controls.Add(this.label14);
        this.groupBox1.Controls.Add(this.RadioBdate);
        this.groupBox1.Controls.Add(this.button2);
        this.groupBox1.Controls.Add(this.btnChangeQn);
        this.groupBox1.Controls.Add(this.panel13);
        this.groupBox1.Controls.Add(this.txtPnr);
        this.groupBox1.Controls.Add(this.B2AAA);
        this.groupBox1.Controls.Add(this.RadioPnr);
        this.groupBox1.Controls.Add(this.RadioJdate);
        this.groupBox1.Controls.Add(this.btnChangePwd);
        this.groupBox1.Font = new System.Drawing.Font("Arial", 9F);
        this.groupBox1.Location = new System.Drawing.Point(801, 528);
        this.groupBox1.Name = "groupBox1";
        this.groupBox1.Size = new System.Drawing.Size(757, 119);
        this.groupBox1.TabIndex = 36;
        this.groupBox1.TabStop = false;
        this.groupBox1.Text = "Search PNR";
        // 
        // panel13
        // 
        this.panel13.Controls.Add(this.lbl7);
        this.panel13.Controls.Add(this.B3AAA);
        this.panel13.Controls.Add(this.lblCurMonbk);
        this.panel13.Controls.Add(this.label3);
        this.panel13.Controls.Add(this.label2);
        this.panel13.Location = new System.Drawing.Point(2, 72);
        this.panel13.Name = "panel13";
        this.panel13.Size = new System.Drawing.Size(750, 42);
        this.panel13.TabIndex = 24;
        // 
        // lbl7
        // 
        this.lbl7.AutoSize = true;
        this.lbl7.BackColor = System.Drawing.Color.White;
        this.lbl7.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.lbl7.ForeColor = System.Drawing.Color.Red;
        this.lbl7.Location = new System.Drawing.Point(718, 24);
        this.lbl7.Name = "lbl7";
        this.lbl7.Size = new System.Drawing.Size(14, 15);
        this.lbl7.TabIndex = 41;
        this.lbl7.Text = "T";
        this.lbl7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // B3AAA
        // 
        this.B3AAA.AutoSize = true;
        this.B3AAA.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.B3AAA.Location = new System.Drawing.Point(477, 24);
        this.B3AAA.Name = "B3AAA";
        this.B3AAA.Size = new System.Drawing.Size(205, 15);
        this.B3AAA.TabIndex = 38;
        this.B3AAA.Text = "Total Upcoming Journey Bookings: ";
        // 
        // lblCurMonbk
        // 
        this.lblCurMonbk.AutoSize = true;
        this.lblCurMonbk.BackColor = System.Drawing.Color.White;
        this.lblCurMonbk.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.lblCurMonbk.ForeColor = System.Drawing.Color.Red;
        this.lblCurMonbk.Location = new System.Drawing.Point(362, 5);
        this.lblCurMonbk.Name = "lblCurMonbk";
        this.lblCurMonbk.Size = new System.Drawing.Size(14, 15);
        this.lblCurMonbk.TabIndex = 37;
        this.lblCurMonbk.Text = "T";
        this.lblCurMonbk.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.label3.Location = new System.Drawing.Point(200, 4);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(153, 15);
        this.label3.TabIndex = 35;
        this.label3.Text = "Current Month Bookings: ";
        // 
        // lbl6
        // 
        this.lbl6.AutoSize = true;
        this.lbl6.BackColor = System.Drawing.Color.Black;
        this.lbl6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
        this.lbl6.ForeColor = System.Drawing.Color.Yellow;
        this.lbl6.Location = new System.Drawing.Point(68, 423);
        this.lbl6.Name = "lbl6";
        this.lbl6.Size = new System.Drawing.Size(17, 16);
        this.lbl6.TabIndex = 40;
        this.lbl6.Text = "T";
        this.lbl6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // C3AA0
        // 
        this.C3AA0.AutoSize = true;
        this.C3AA0.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.C3AA0.Location = new System.Drawing.Point(3, 423);
        this.C3AA0.Name = "C3AA0";
        this.C3AA0.Size = new System.Drawing.Size(66, 15);
        this.C3AA0.TabIndex = 39;
        this.C3AA0.Text = "DeActive: ";
        // 
        // lblTobk
        // 
        this.lblTobk.AutoSize = true;
        this.lblTobk.BackColor = System.Drawing.Color.Black;
        this.lblTobk.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.lblTobk.ForeColor = System.Drawing.Color.Yellow;
        this.lblTobk.Location = new System.Drawing.Point(162, 467);
        this.lblTobk.Name = "lblTobk";
        this.lblTobk.Size = new System.Drawing.Size(14, 15);
        this.lblTobk.TabIndex = 36;
        this.lblTobk.Text = "T";
        this.lblTobk.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.label1.Location = new System.Drawing.Point(75, 468);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(81, 15);
        this.label1.TabIndex = 35;
        this.label1.Text = "Total Booked";
        // 
        // pnlloader
        // 
        this.pnlloader.BackColor = System.Drawing.SystemColors.Control;
        this.pnlloader.Location = new System.Drawing.Point(236, 438);
        this.pnlloader.Name = "pnlloader";
        this.pnlloader.Size = new System.Drawing.Size(433, 65);
        this.pnlloader.TabIndex = 133;
        this.pnlloader.Visible = false;
        // 
        // backgroundWorker_0
        // 
        this.backgroundWorker_0.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_0_DoWork);
        // 
        // backgroundWorker_1
        // 
        this.backgroundWorker_1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_1_DoWork);
        // 
        // bunifuElipse_0
        // 
        // 
        // pictureBox1
        // 
        this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
        this.pictureBox1.Location = new System.Drawing.Point(475, 387);
        this.pictureBox1.Name = "pictureBox1";
        this.pictureBox1.Size = new System.Drawing.Size(32, 32);
        this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
        this.pictureBox1.TabIndex = 24;
        this.pictureBox1.TabStop = false;
        this.pictureBox1.Visible = false;
        // 
        // button3
        // 
        this.button3.BackColor = System.Drawing.SystemColors.ButtonShadow;
        this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.button3.Location = new System.Drawing.Point(526, 376);
        this.button3.Name = "button3";
        this.button3.Size = new System.Drawing.Size(78, 54);
        this.button3.TabIndex = 135;
        this.button3.Text = "Change PASS";
        this.button3.UseVisualStyleBackColor = false;
        this.button3.Click += new System.EventHandler(this.btnChangePwd_Click);
        // 
        // txtbulk
        // 
        this.txtbulk.Location = new System.Drawing.Point(757, 50);
        this.txtbulk.Name = "txtbulk";
        this.txtbulk.Size = new System.Drawing.Size(141, 284);
        this.txtbulk.TabIndex = 136;
        this.txtbulk.Text = "";
        // 
        // label6
        // 
        this.label6.AutoSize = true;
        this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label6.ForeColor = System.Drawing.Color.Blue;
        this.label6.Location = new System.Drawing.Point(756, 34);
        this.label6.Name = "label6";
        this.label6.Size = new System.Drawing.Size(89, 13);
        this.label6.TabIndex = 137;
        this.label6.Text = "Past ID\'s Here";
        this.label6.Visible = false;
        // 
        // button4
        // 
        this.button4.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.button4.Location = new System.Drawing.Point(858, 30);
        this.button4.Name = "button4";
        this.button4.Size = new System.Drawing.Size(40, 20);
        this.button4.TabIndex = 138;
        this.button4.Text = "PAST";
        this.button4.UseVisualStyleBackColor = false;
        this.button4.Click += new System.EventHandler(this.button4_Click);
        // 
        // label7
        // 
        this.label7.AutoSize = true;
        this.label7.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.label7.Location = new System.Drawing.Point(129, 423);
        this.label7.Name = "label7";
        this.label7.Size = new System.Drawing.Size(78, 15);
        this.label7.TabIndex = 139;
        this.label7.Text = "Disable IDs: ";
        // 
        // lblDisableCount
        // 
        this.lblDisableCount.AutoSize = true;
        this.lblDisableCount.BackColor = System.Drawing.Color.Black;
        this.lblDisableCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
        this.lblDisableCount.ForeColor = System.Drawing.Color.Yellow;
        this.lblDisableCount.Location = new System.Drawing.Point(211, 423);
        this.lblDisableCount.Name = "lblDisableCount";
        this.lblDisableCount.Size = new System.Drawing.Size(17, 16);
        this.lblDisableCount.TabIndex = 140;
        this.lblDisableCount.Text = "T";
        this.lblDisableCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // lblInValid
        // 
        this.lblInValid.AutoSize = true;
        this.lblInValid.BackColor = System.Drawing.Color.Black;
        this.lblInValid.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
        this.lblInValid.ForeColor = System.Drawing.Color.Yellow;
        this.lblInValid.Location = new System.Drawing.Point(339, 423);
        this.lblInValid.Name = "lblInValid";
        this.lblInValid.Size = new System.Drawing.Size(17, 16);
        this.lblInValid.TabIndex = 142;
        this.lblInValid.Text = "T";
        this.lblInValid.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // label9
        // 
        this.label9.AutoSize = true;
        this.label9.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.label9.Location = new System.Drawing.Point(259, 423);
        this.label9.Name = "label9";
        this.label9.Size = new System.Drawing.Size(75, 15);
        this.label9.TabIndex = 141;
        this.label9.Text = "InValid IDs: ";
        // 
        // lblUnVerified
        // 
        this.lblUnVerified.AutoSize = true;
        this.lblUnVerified.BackColor = System.Drawing.Color.Black;
        this.lblUnVerified.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblUnVerified.ForeColor = System.Drawing.Color.Yellow;
        this.lblUnVerified.Location = new System.Drawing.Point(493, 423);
        this.lblUnVerified.Name = "lblUnVerified";
        this.lblUnVerified.Size = new System.Drawing.Size(17, 16);
        this.lblUnVerified.TabIndex = 144;
        this.lblUnVerified.Text = "T";
        this.lblUnVerified.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // label11
        // 
        this.label11.AutoSize = true;
        this.label11.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.label11.Location = new System.Drawing.Point(400, 423);
        this.label11.Name = "label11";
        this.label11.Size = new System.Drawing.Size(95, 15);
        this.label11.TabIndex = 143;
        this.label11.Text = "UnVerified IDs: ";
        // 
        // label10
        // 
        this.label10.AutoSize = true;
        this.label10.Font = new System.Drawing.Font("Corbel", 9.75F, System.Drawing.FontStyle.Bold);
        this.label10.Location = new System.Drawing.Point(153, 397);
        this.label10.Name = "label10";
        this.label10.Size = new System.Drawing.Size(72, 15);
        this.label10.TabIndex = 145;
        this.label10.Text = "Active IDs: ";
        // 
        // button1
        // 
        this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold);
        this.button1.Location = new System.Drawing.Point(610, 393);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(143, 45);
        this.button1.TabIndex = 146;
        this.button1.Text = "Manage Bookings";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += new System.EventHandler(this.button1_Click);
        // 
        // tabIrctcId
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
        this.Controls.Add(this.button1);
        this.Controls.Add(this.label10);
        this.Controls.Add(this.lblUnVerified);
        this.Controls.Add(this.label11);
        this.Controls.Add(this.lblInValid);
        this.Controls.Add(this.lblActive);
        this.Controls.Add(this.label9);
        this.Controls.Add(this.lblDisableCount);
        this.Controls.Add(this.label7);
        this.Controls.Add(this.button4);
        this.Controls.Add(this.label6);
        this.Controls.Add(this.txtbulk);
        this.Controls.Add(this.button3);
        this.Controls.Add(this.pictureBox1);
        this.Controls.Add(this.grpAddID);
        this.Controls.Add(this.lbl6);
        this.Controls.Add(this.button5);
        this.Controls.Add(this.pnlloader);
        this.Controls.Add(this.B1AAA);
        this.Controls.Add(this.lblTotal);
        this.Controls.Add(this.lblMsg);
        this.Controls.Add(this.C3AA0);
        this.Controls.Add(this.groupBox1);
        this.Controls.Add(this.grdUsers);
        this.Controls.Add(this.lblTobk);
        this.Controls.Add(this.C1AA0);
        this.Controls.Add(this.label1);
        this.Name = "tabIrctcId";
        this.Size = new System.Drawing.Size(890, 337);
        ((System.ComponentModel.ISupportInitialize)(this.grdUsers)).EndInit();
        this.grpAddID.ResumeLayout(false);
        this.grpAddID.PerformLayout();
        this.groupBox1.ResumeLayout(false);
        this.groupBox1.PerformLayout();
        this.panel13.ResumeLayout(false);
        this.panel13.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    private void button1_Click_1(object sender, EventArgs e)
    {
        lblMsg.Visible = true;
        pictureBox1.Visible = true;
        bgwkverify_.RunWorkerAsync();
    }

    private void bgwkverify__DoWork(object sender, DoWorkEventArgs e)
    {
        method_2(true);
        DataTable dataTable = new DataTable();
        string commandText = "SELECT * FROM TBL_USER";
        new SQLiteDataAdapter(commandText, Class34.string_Database).Fill(dataTable);
        int num = 0;
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                num++;
                try
                {
                    string columnName = "UserName";
                    string text = row[columnName].ToString();
                    string columnName2 = "strPwd";
                    row[columnName2].ToString();
                    string text2 = "verifi_iid";
                    string errMessage = text2;
                    string columnName3 = "UserState";
                    string text3 = row[columnName3].ToString();
                    string text4 = "Veryfying ID: ";
                    method_3(text4 + text);
                    string text5 = "InCompleteProfile";
                    if (text3 == text5)
                    {
                        mCOMM mcommInstance_ = Program.mcommInstance_;
                        string pwd = "verifi_iid";
                        mcommInstance_.isLV(text, pwd, ref errMessage);
                        string text6 = errMessage;
                        string text7 = "true";
                        if (text6 != text7)
                        {
                            string text8 = "Verify Failed!";
                            MessageBox.Show(text8);
                            goto IL_0159;
                        }
                    }
                }
                catch
                {
                }
            }
        }
        string object_ = "Done";
        method_3(object_);
        goto IL_0159;
    IL_0159:
        method_2(false);
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBox1.Checked)
        {
            label6.Visible = true;
            txtbulk.Enabled = true;
            txtLogin.Enabled = false;
            
        }
        else
        {
            txtbulk.Enabled = false;
            label6.Visible = false;
            txtLogin.Enabled = true;
            
        }
    }

    private void button4_Click(object sender, EventArgs e)
    {
        if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
        {
            Clipboard.GetText();
            txtbulk.Paste();
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (txtLogin.Text.Length > 3)
        {
            string errMessage = "Histry_PrintTicket";
            string text60 = Program.mcommInstance_.isLV(txtLogin.Text.Trim(), txtPassword.Text.Trim(), ref errMessage);
        }
        else
        {
            MessageBox.Show("First Select Irctc Id Then Click On Manage Bookings Button");
        }
    }
}
