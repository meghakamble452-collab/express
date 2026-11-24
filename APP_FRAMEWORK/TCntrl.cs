using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using Booking;
using Newtonsoft.Json;
using NEXUS;

namespace APP_FRAMEWORK;

internal class TCntrl : UserControl
{
    // ByteNova: native disabled WinForms ComboBox paints a system white face.
    // These visual covers keep the real ComboBox disabled/functional state untouched,
    // while showing the same value on a Navy Blue face.
    private readonly Dictionary<ComboBox, Label> _byteNovaDisabledComboCovers =
        new Dictionary<ComboBox, Label>();

    private delegate void ChangeTextsSafe(string length);

    public string[,] Traindata = new string[51, 12];

    public int x;

    public int y;

    public Size PairSize;

    public Size lblGatewaySize;

    public Size Panel1Size;

    public Size baseSize;

    public Point A;

    public Point B;

    public Point C;

    public Point D;

    public Point E;

    public Point F;

    public Point G;

    public Point H;

    private string string_1;

    public static bool isok;

    private EventHandler eventHandler_0;

    private EventHandler eventHandler_1;

    private EventHandler eventHandler_2;

    private EventHandler eventHandler_3;

    private EventHandler B2AAA;

    internal Class48 class48_0;

    internal string string_0;

    internal string LoginSite;

    internal string fare_str;

    internal string Actualfare;

    internal bool bool_0;

    internal bool LoginProceed;

    internal int int_1;

    internal int int_2;

    internal int WindoNo;

    internal int int_3;

    internal int TrDayDiff;

    internal long ftime;

    internal long long_3;

    private static List<string> C2AA0 = new List<string>();

    private static object object_0 = new object();

    private string string_2;

    internal bool bool_1;

    private Container container_0;

    internal Label lblJourney;

    private Panel TopPanel;

    internal ComboBox cboID;

    internal ComboBox cboSlot;

    internal Button btnEdit;

    internal Button BtnOpenPair;

    internal ComboBox cboGateway;

    internal CheckBox chkConfirmation;

    internal ComboBox cboQuota;

    public System.Windows.Forms.Timer timer_0;

    private ToolTip toolTip_0;

    private IContainer icontainer_0;

    internal Label lblStatus;

    internal Label lblClass;

    public Label lblTktName;

    internal ComboBox cboTrain;

    private Panel HeaderPanel1;

    internal bool IsOpenningTicket;

    public Button btnCancel;

    private Panel B3AAA;

    private Label lbl_Month;

    private Label lbl_Date;

    private LinkLabel WLlinkLabel;

    public Panel PanelPnr;

    public Panel Panel1;

    internal Label label5;

    internal Label lbl_Detail;

    internal Label lbl_status;

    internal Label lbl_slot;

    public Label lblAvailability;

    public LinkLabel EditlinkLabel;

    public Label lblTime;

    public Label lblPnr;

    public Label label13;

    public Label label11;

    internal Label lblDate;

    internal ComboBox cboClass;

    private Panel panel2;

    public Label lblInfo;

    public bool GNBooking;

    public bool ACBooking;

    public bool SLBooking;

    public bool bool_ISADVV;

    internal bool ChkAutoLogin;

    internal bool MicroWindow;

    internal Button AppLogin_;

    internal bool Applogin;
    internal bool mixlogin;

    internal bool web3;

    internal bool Dishalogin;

    internal bool Betalogin;

    internal int loginType;

    internal bool WithCapchtaInputlogin;

    internal bool WithOtpInputlogin;

    internal bool autologin;

    public Button btnSlotNo;

    public Button btnMinize;

    private Panel Panel5;

    internal Label lblTrainNo;

    internal Panel BottomPanel;

    private System.Windows.Forms.Timer timer_1;
    private bool _byteNovaStatusBlinkOn;

    private IContainer components;

    private Label lbl_brand;

    public Button WebLogin;

    internal Button button1;

    internal CheckBox check_avl;

    private PictureBox pictureBox2;

    internal Label lbl_time;

    private Label lbl_bnk;

    private bool blb;

    private Label lblBrand;

    internal Button btnDish;

    private string CLc;

    internal Button btnWeb3;

    internal Panel pnlBrand;

    internal Label lblBank;

    internal Label lblLoginType;

    internal ComboBox fake_bank;

    internal ComboBox fake_id;
    private ToolTip toolTip1;
    internal Button button2;
    private string eWallet_irctc;

    internal event EventHandler Event_0
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        add
        {
            eventHandler_0 = (EventHandler)Delegate.Combine(eventHandler_0, value);
        }
        remove
        {
            eventHandler_0 = (EventHandler)Delegate.Remove(eventHandler_0, (EventHandler)Delegate.Combine(eventHandler_0, value));
        }
    }

    internal event EventHandler Event_1
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        add
        {
            eventHandler_1 = (EventHandler)Delegate.Combine(eventHandler_1, value);
        }
        remove
        {
            eventHandler_1 = (EventHandler)Delegate.Remove(eventHandler_1, (EventHandler)Delegate.Combine(eventHandler_1, value));
        }
    }

    internal event EventHandler Event_2
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        add
        {
            eventHandler_2 = (EventHandler)Delegate.Combine(eventHandler_2, value);
        }
        remove
        {
            eventHandler_2 = (EventHandler)Delegate.Remove(eventHandler_2, (EventHandler)Delegate.Combine(eventHandler_2, value));
        }
    }

    internal event EventHandler Event_3
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        add
        {
            eventHandler_3 = (EventHandler)Delegate.Combine(eventHandler_3, value);
        }
        remove
        {
            eventHandler_3 = (EventHandler)Delegate.Remove(eventHandler_3, (EventHandler)Delegate.Combine(eventHandler_3, value));
        }
    }

    internal event EventHandler Event_4
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        add
        {
            B2AAA = (EventHandler)Delegate.Combine(B2AAA, value);
        }
        remove
        {
            B2AAA = (EventHandler)Delegate.Remove(B2AAA, (EventHandler)Delegate.Combine(B2AAA, value));
        }
    }

    // Notifies frmProcessTicket which login/browser UI was selected.
    public event Action<string> LoginHeaderIconChanged;

    // Notifies frmProcessTicket whenever Train Combo selection changes.
    public event Action<string> TrainHeaderChanged;

    private void NotifyHeaderLoginIcon(string loginType)
    {
        EventHandler dummy = null; // keeps this method compatible with older project style
        if (LoginHeaderIconChanged != null)
            LoginHeaderIconChanged(loginType);
    }

    public TCntrl()
    {
        string_0 = "";
        string_2 = "";
        InitializeComponent();

        // Keep frmProcessTicket header Train No synchronized with cboTrain.
        cboTrain.SelectedIndexChanged += ByteNovaTrainHeader_SelectedIndexChanged;

        ApplyByteNovaModernStyle();
        ApplyByteNovaGoldenDetailLabels();
        ApplyByteNovaDisabledComboTextToAll();
        ApplyApprovedLargeWindowLayout();
        A = cboID.Location;
        B = cboSlot.Location;
        C = cboQuota.Location;
        D = cboTrain.Location;
        E = HeaderPanel1.Location;
        F = cboGateway.Location;
        G = chkConfirmation.Location;
        _ = base.Width;
        _ = base.Height;
        _ = Screen.PrimaryScreen.WorkingArea.Width;
        _ = Screen.PrimaryScreen.WorkingArea.Height;
        x = Screen.PrimaryScreen.WorkingArea.Width - 1366;
        y = Screen.PrimaryScreen.WorkingArea.Height - 768;
        y = Screen.PrimaryScreen.WorkingArea.Height - 728;
        MicroWindow = StatClass.MicroWindow;
        string text = "Ready";
        StatusChange(0, text);
        cboGateway.Items.Clear();
        ComboBox.ObjectCollection items = cboGateway.Items;
        string item = "Select-Bank-XXXX";
        items.Add(item);
        Program.dictionary_00.Clear();
        DataTable dataTable = new DataTable();
        string commandText = "SELECT BANK_SAVEAS, BANK_DETAILS from TBL_BANK";
        new SQLiteDataAdapter(commandText, Class34.string_Database).Fill(dataTable);
        if (dataTable != null && dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                string key = row[0].ToString();
                string value = row[1].ToString();
                Program.dictionary_00.Add(key, value);
            }
        }
        foreach (string key2 in Program.dictionary_00.Keys)
        {
            cboGateway.Items.Add(key2);
        }
        cboGateway.SelectedIndex = 0;
        cboID.Items.Clear();
        ComboBox.ObjectCollection items2 = cboID.Items;
        string item2 = "Select-ID-XXXX";
        items2.Add(item2);
        foreach (string key3 in Program.dictionary_0.Keys)
        {
            cboID.Items.Add(key3);
        }
        cboID.SelectedIndex = 0;
        cboSlot.Items.Clear();
        ComboBox.ObjectCollection items3 = cboSlot.Items;
        string item3 = "Select-Slot-XXXX";
        items3.Add(item3);
        for (int i = 1; i <= Program.int_Slot; i++)
        {
            ComboBox.ObjectCollection items4 = cboSlot.Items;
            string text2 = "Slot-";
            items4.Add(text2 + i);
        }
        string loginSite = "1";
        LoginSite = loginSite;
        cboSlot.SelectedIndex = 0;
        PairSize = BtnOpenPair.Size;
        lblGatewaySize = cboGateway.Size;
        Panel1Size = Panel1.Size;
        baseSize = base.Size;
    }

    internal TCntrl(Class48 class48_1)
        : this()
    {
        method_0(class48_1);
    }

    private void ByteNovaTrainHeader_SelectedIndexChanged(object sender, EventArgs e)
    {
        string trainNo = (cboTrain != null) ? cboTrain.Text : "";
        if (!string.IsNullOrWhiteSpace(trainNo) && TrainHeaderChanged != null)
            TrainHeaderChanged(trainNo);
    }

    private void ResetByteNovaSuccessDesign()
    {
        _byteNovaFinalSuccessDesign = false;
        _byteNovaFinalSuccessText = "";
        _byteNovaLoginCompleted = false;
        Invalidate();
    }

    public void TCntrll()
    {
        if (Class45.ManagerDashborad)
        {
            if (MicroWindow)
            {
                method_2();
                MicroWindow = false;
            }
            else
            {
                method_3();
                MicroWindow = true;
            }
        }
        else if (!StatClass.MicroWindow)
        {
            method_3();
            MicroWindow = true;
        }
        else
        {
            method_2();
            MicroWindow = false;
        }
        if (bool_1)
        {
            btnEdit.Visible = false;
            EditlinkLabel.Visible = false;
            BtnOpenPair.Visible = false;
        }
    }

    internal void method_0(Class48 class48_1)
    {
        try
        {
            class48_0 = class48_1;
            DateTime dateTime = new DateTime(int.Parse(class48_1.string_6), int.Parse(class48_1.string_5), int.Parse(class48_1.string_4));
            if (class48_1.string_0 == class48_1.string_3)
            {
                Label label = lblJourney;
                string text = class48_1.string_0;
                string text2 = "-";
                label.Text = text + text2 + class48_1.string_1;
            }
            else
            {
                Label label2 = lblJourney;
                string[] array = new string[5] { class48_1.string_0, null, null, null, null };
                string text3 = "-";
                array[1] = text3;
                array[2] = class48_1.string_3;
                string text4 = "-";
                array[3] = text4;
                array[4] = class48_1.string_1;
                label2.Text = string.Concat(array);
            }
            Label label3 = lblDate;
            string text5 = "dd/MM";
            label3.Text = dateTime.ToString(text5);
            lblClass.Text = class48_1.string_7;
            cboClass.Text = class48_1.string_7;
            string_1 = class48_1.string_12;
            cboQuota.SelectedItem = class48_1.string_8;
            cboTrain.SelectedItem = class48_1.string_2;
            cboGateway.SelectedItem = class48_1.string_23;
            lblTrainNo.Text = class48_1.string_2;
            lblJourney.Text = class48_1.string_0 + "_" + class48_1.string_1;
            foreach (KeyValuePair<string, string> item in Program.list_SelectedIRCTCid)
            {
                string value = item.Value;
                string text6 = "free";
                if (value == text6)
                {
                    cboID.SelectedItem = item.Key;
                    Dictionary<string, string> list_SelectedIRCTCid = Program.list_SelectedIRCTCid;
                    string key = item.Key;
                    string value2 = "busy";
                    list_SelectedIRCTCid[key] = value2;
                    break;
                }
            }
            IsOpenningTicket = class48_1.bool_8;
            string string_ = class48_1.string_25;
            string[] array2 = new string[1];
            string text7 = "|";
            array2[0] = text7;
            string[] array3 = string_.Split(array2, StringSplitOptions.RemoveEmptyEntries);
            if (class48_1.bool_6)
            {
                chkConfirmation.Checked = true;
            }
            fare_str = array3[0].Trim();
            Actualfare = array3[0].Trim();
            Label label4 = lblAvailability;
            string text8 = label4.Text;
            string text9 = "|";
            label4.Text = text8 + text9 + fare_str;
            string string_2 = class48_1.string_8;
            string text10 = "PT";
            if (string_2 == text10)
            {
                int_2 = int.Parse(array3[0].Trim());
            }
            Class34.list_0.Contains(class48_1.string_2);
            ToolTip toolTip = toolTip_0;
            string text11 = "Ticket: ";
            toolTip.ToolTipTitle = text11 + class48_1.string_12;
            if (class48_1.bool_3)
            {
                LinkLabel wLlinkLabel = WLlinkLabel;
                string text12 = "WL Stop";
                wLlinkLabel.Text = text12;
                WLlinkLabel.VisitedLinkColor = Color.Red;
            }
            else
            {
                LinkLabel wLlinkLabel2 = WLlinkLabel;
                string text13 = "WL Allow";
                wLlinkLabel2.Text = text13;
                WLlinkLabel.VisitedLinkColor = Color.Blue;
            }
            if (!StatClass.MicroWindow)
            {
                method_2();
            }
            else
            {
                method_3();
            }
            Label label5 = lbl_Detail;
            string text14 = "DEOS --> NDLS 15:30-06:45";
            label5.Text = text14;
            Label label6 = lbl_Detail;
            string[] array4 = new string[10];
            string text15 = "Train: ";
            array4[0] = text15;
            array4[1] = class48_1.string_2;
            string text16 = "\r\nQuota: ";
            array4[2] = text16;
            array4[3] = class48_1.string_8;
            string text17 = "\r\nClass: ";
            array4[4] = text17;
            array4[5] = class48_1.string_7;
            string text18 = "\r\nSrc: ";
            array4[6] = text18;
            array4[7] = class48_1.string_0;
            string text19 = "\r\nDest: ";
            array4[8] = text19;
            array4[9] = class48_1.string_1;
            label6.Text = string.Concat(array4);
            Label label7 = this.label5;
            string text20 = "VAISHALI EXP (18201)";
            label7.Text = text20;
            Label label8 = this.label5;
            string text21 = "Exp Train: (";
            string text22 = class48_1.string_2;
            string text23 = ")_";
            label8.Text = text21 + text22 + text23;
            Label label9 = lbl_Date;
            string text24 = "dd";
            label9.Text = dateTime.ToString(text24);
            Label label10 = lbl_Month;
            string text25 = "MMM yyyy";
            label10.Text = dateTime.ToString(text25);
            Label label11 = lbl_slot;
            string text26 = "SLOT - ";
            label11.Text = text26 + (cboSlot.SelectedIndex + 1);
            lbl_bnk.Text = GetBankName(cboGateway.SelectedItem.ToString());
            Label label12 = lblStatus;
            string text27 = "Ready-{ Bkg Fare Limit: ";
            string text28 = fare_str;
            string text29 = " } ";
            label12.Text = text27 + text28 + text29;
            Refresh();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }

    private string GetBankName(string str)
    {
        string result = "";
        try
        {
            string value = "_";
            if (str.Contains(value))
            {
                string value2 = "_";
                string text = str.Substring(str.IndexOf(value2) + 1).Trim();
                string value3 = "_";
                string text2 = str.Substring(0, str.IndexOf(value3)).Trim();
                string text3 = text;
                string value4 = "__";
                if (text3.Contains(value4))
                {
                    string text4 = text2;
                    string text5 = "_";
                    string text6 = text;
                    string text7 = text;
                    string value5 = "__";
                    text2 = text4 + text5 + text6.Substring(0, text7.IndexOf(value5)).Trim();
                    string text8 = text;
                    string text9 = text;
                    string value6 = "__";
                    text = text8.Substring(text9.IndexOf(value6) + 2).Trim();
                    string text10 = text;
                    string value7 = "_";
                    if (text10.Contains(value7))
                    {
                        string text11 = text2;
                        string text12 = "__";
                        string text13 = text;
                        string text14 = text;
                        string value8 = "_";
                        text2 = text11 + text12 + text13.Substring(0, text14.IndexOf(value8)).Trim();
                        result = text2;
                        return result;
                    }
                    return result;
                }
                string text15 = text;
                string value9 = "_";
                if (!text15.Contains(value9))
                {
                    result = text2;
                    return result;
                }
                string text16 = text2;
                string text17 = "_";
                string text18 = text;
                string text19 = text;
                string value10 = "_";
                text2 = text16 + text17 + text18.Substring(0, text19.IndexOf(value10)).Trim();
                result = text2;
                return result;
            }
            return result;
        }
        catch
        {
            return result;
        }
    }

    private void method_2()
    {
        cboSlot.Visible = true;
        if (class48_0 != null)
            lblJourney.Text = class48_0.string_0 + "_" + class48_0.string_1;
        RestoreApprovedGeometry();
    }


    private static Image _bnBrandLogo;
    private static bool _bnBrandLogoTried;

    private static Image LoadBrandLogo()
    {
        if (_bnBrandLogoTried) return _bnBrandLogo;
        _bnBrandLogoTried = true;
        try
        {
            string[] paths =
            {
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "express-logo.png"),
            Path.Combine(Application.StartupPath, "express-logo.png")
        };
            foreach (string p in paths)
            {
                if (!File.Exists(p)) continue;
                using (FileStream fs = new FileStream(p, FileMode.Open, FileAccess.Read))
                using (Image tmp = Image.FromStream(fs))
                    _bnBrandLogo = new Bitmap(tmp);
                break;
            }
        }
        catch { }
        return _bnBrandLogo;
    }

    private void method_3()
    {
        cboSlot.Visible = true;
        if (class48_0 != null)
            lblJourney.Text = class48_0.string_0 + "_" + class48_0.string_1;
        RestoreApprovedGeometry();
    }

    internal void StatusChange(int int_4, string string_2)
    {
        try
        {
            if (int_4 != 900)
            {
                switch (int_4)
                {
                    case 999:
                        {
                            string text7 = string_2;
                            string value2 = "AVAILABLE";
                            if (text7.Contains(value2))
                            {
                                string text8 = string_2;
                                string oldValue2 = "AVAILABLE";
                                string newValue2 = "AVL";
                                string_2 = text8.Replace(oldValue2, newValue2);
                            }
                            string text9 = string_2;
                            string[] array3 = new string[1];
                            string text10 = "$";
                            array3[0] = text10;
                            string[] array4 = text9.Split(array3, StringSplitOptions.RemoveEmptyEntries);
                            string_2 = array4[0].Trim();
                            Label label2 = lblAvailability;
                            string text11 = array4[2].Trim();
                            string text12 = " |  ";
                            label2.Text = text11 + text12 + array4[1].Trim();
                            lblInfo.Text = lblAvailability.Text;
                            break;
                        }
                    case 959:
                        {
                            string text = string_2;
                            string value = "AVAILABLE";
                            if (text.Contains(value))
                            {
                                string text2 = string_2;
                                string oldValue = "AVAILABLE";
                                string newValue = "AVL";
                                string_2 = text2.Replace(oldValue, newValue);
                            }
                            string text3 = string_2;
                            string[] array = new string[1];
                            string text4 = "$";
                            array[0] = text4;
                            string[] array2 = text3.Split(array, StringSplitOptions.None);
                            string_2 = array2[0].Trim();
                            Label label = lblAvailability;
                            string text5 = array2[1].Trim();
                            string text6 = " |  ";
                            label.Text = text5 + text6 + fare_str;
                            lblInfo.Text = lblAvailability.Text;
                            break;
                        }
                }
                if (int_4 > 200)
                {
                    if (int_4 == 202)
                    {
                        _byteNovaStatusBlinkOn = true;
                        timer_1.Enabled = true;
                        Invalidate();
                        Update();
                    }
                    if (int_4 == 203)
                    {
                        timer_1.Enabled = false;
                        _byteNovaStatusBlinkOn = false;
                        Invalidate();
                        Update();
                    }
                    if (int_4 == 204)
                    {
                        timer_1.Enabled = false;
                        _byteNovaStatusBlinkOn = false;
                        Invalidate();
                        Update();
                    }
                    if (int_4 == 253)
                    {
                        cboGateway.Enabled = true;
                    }
                }
                else if (int_4 == 5)
                {
                    lblBrand.Location = new Point(56, 39);
                    cboID.Left = 2;
                    timer_0.Enabled = false;
                    timer_1.Enabled = false;
                    string obj = "0";
                    string bank = cboGateway.Text;
                    selectredbank = bank.Substring(0, bank.IndexOf("_")).Trim();
                    // Restore the ByteNova reference design before CloseAfter.
                    RestoreByteNovaReferenceDesignBeforeClose(string_2);

                    // Paint the restored reference state before the close routine.
                    Application.DoEvents();
                    CloseAfter(obj);
                }
                if (!string.IsNullOrEmpty(string_2))
                {
                    Label label3 = lblStatus;
                    string text13 = " ";
                    label3.Text = text13 + string_2;
                    lblStatus.ForeColor = Color.White;
                }
                if ((uint)(int_4 - 2) <= 3u)
                {
                    timer_0.Enabled = false;
                    timer_1.Enabled = false;
                }
            }
            else
            {
                int_3 = int.Parse(string_2);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }

    private void set_pic(string str)
    {
        try
        {
            string[] array = new string[1];
            string text = "T";
            array[0] = text;
            string[] array2 = str.Split(array, StringSplitOptions.None);
            lbl_time.Text = array2[1];
            BottomPanel.Visible = false;
            pictureBox2.Top = 0;
        }
        catch
        {
        }
    }

    internal void ChnageColour_()
    {
        if (Applogin)
        {
            BackColor = Color.GhostWhite;
            pnlBrand.BackColor = Color.GhostWhite;
            lblBrand.BackColor = Color.GhostWhite;
            lblBank.BackColor = Color.GhostWhite;
            lblLoginType.BackColor = Color.GhostWhite;
            BottomPanel.BackColor = Color.Thistle;
            lblStatus.BackColor = Color.FromArgb(3, 29, 59);
            lblStatus.ForeColor = Color.White;
        }
        else if (web3)
        {
            BackColor = Color.Thistle;
            pnlBrand.BackColor = Color.Thistle;
            lblBrand.BackColor = Color.Thistle;
            lblBank.BackColor = Color.Thistle;
            lblLoginType.BackColor = Color.Thistle;
            BottomPanel.BackColor = Color.Thistle;
            lblStatus.BackColor = Color.FromArgb(3, 29, 59);
            lblStatus.ForeColor = Color.White;
        }
        else
        {
            BackColor = SystemColors.ButtonHighlight;
            pnlBrand.BackColor = SystemColors.ButtonHighlight;
            lblBrand.BackColor = SystemColors.ButtonHighlight;
            lblBank.BackColor = SystemColors.ButtonHighlight;
            lblLoginType.BackColor = SystemColors.ButtonHighlight;
            BottomPanel.BackColor = SystemColors.ButtonHighlight;
            lblStatus.BackColor = Color.FromArgb(3, 29, 59);
            lblStatus.ForeColor = Color.White;
        }
    }

    private void timer_1_Tick(object sender, EventArgs e)
    {
        if (!timer_1.Enabled)
            return;

        _byteNovaStatusBlinkOn = !_byteNovaStatusBlinkOn;

        // The visible ByteNova status strip is painted by ByteNovaReferencePaint,
        // therefore repaint the whole TCntrl instead of only lblStatus.
        Invalidate(new Rectangle(7, 7, 316, 27));
        Update();
    }

    private bool _byteNovaFinalSuccessDesign;
    private string _byteNovaFinalSuccessText = "";

    private void RestoreByteNovaReferenceDesignBeforeClose(string finalStatus)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<string>(RestoreByteNovaReferenceDesignBeforeClose), finalStatus);
            return;
        }

        // IMPORTANT: this is a separate FINAL/SUCCESS visual state.
        // Re-calling ShowByteNovaAfterLoginPanel() did not visibly change anything because
        // that was already the current state.
        _byteNovaFinalSuccessText = finalStatus ?? "";
        _byteNovaFinalSuccessDesign = true;

        SuspendLayout();
        Size = new Size(330, 176);

        // Hide only while the final success/reference surface is being painted.
        Control[] finalHidden =
        {
            cboID, cboSlot, cboGateway, cboQuota, cboTrain, cboClass,
            BtnOpenPair, chkConfirmation, check_avl,
            WebLogin, btnDish, btnWeb3, AppLogin_,
            lblAvailability, lblTime, lblJourney, lblTrainNo, lblClass, lblDate
        };
        foreach (Control c in finalHidden)
            if (c != null) c.Visible = false;

        if (BottomPanel != null) BottomPanel.Visible = false;

        ResumeLayout(true);
        PerformLayout();
        Invalidate(true);
        Refresh();

        // Force parent/window composition to repaint too.
        if (Parent != null)
        {
            Parent.Invalidate(true);
            Parent.Update();
        }

        Application.DoEvents();
    }

    public void CloseAfter(object obj)
    {
        try
        {
            if (StatClass.CloseOnPnr)
            {
                if (Class45.ManagerDashborad)
                {
                    ThreadPool.QueueUserWorkItem(CloseOtherThanPnrBookWindow, null);
                    string text = "Nget_PnrFinish-";
                    string name = base.Name;
                    string text2 = ",";
                    string text3 = text2;
                    Clipboard.SetText(text + name + text3 + lblTktName);
                }
                else
                {
                    ThreadPool.QueueUserWorkItem(CloseOtherThanPnrBookWindow, null);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }

    private void TCntrl_Load_1(object sender, EventArgs e)
    {
        string variable = "S_Window";
        if (Convert.ToBoolean(Environment.GetEnvironmentVariable(variable)))
        {
            base.Height = 60;
            AppLogin_.Top = 36;
            AppLogin_.BringToFront();
            WebLogin.Top = 36;
            WebLogin.BringToFront();
            BottomPanel.BringToFront();
        }
        lbl_time.Parent = pictureBox2;
        lbl_time.BackColor = Color.Transparent;
        lbl_time.Location = new Point(84, 22);
        lbl_status.Parent = pictureBox2;
        lbl_status.BackColor = Color.Transparent;
        lbl_status.Location = new Point(71, 48);
        lbl_brand.Parent = pictureBox2;
        lbl_brand.BackColor = Color.Transparent;
        lbl_brand.Location = new Point(86, 70);
        lbl_bnk.Parent = pictureBox2;
        lbl_bnk.BackColor = Color.Transparent;
        lbl_bnk.Location = new Point(2, 3);
        lbl_Detail.Parent = pictureBox2;
        lbl_Detail.BackColor = Color.Transparent;
        lbl_Detail.Location = new Point(2, 18);
        lbl_slot.Parent = pictureBox2;
        lbl_slot.BackColor = Color.Transparent;
        lbl_slot.Location = new Point(2, 93);
        lbl_Date.Parent = pictureBox2;
        lbl_Date.BackColor = Color.Transparent;
        lbl_Date.Location = new Point(178, 70);
        lbl_Month.Parent = pictureBox2;
        lbl_Month.BackColor = Color.Transparent;
        lbl_Month.Location = new Point(161, 90);
        int_1 = Program.int_4;
        Label label = lblStatus;
        string text = "Ready-{ Bkg Fare Limit: ";
        string text2 = fare_str;
        string text3 = " } ";
        label.Text = text + text2 + text3;
        AddTRaininCombo();
        cboTrain.SelectedItem = class48_0.string_2;
        if (StatClass.Slot_ > 0)
        {
            cboSlot.SelectedIndex = StatClass.Slot_;
        }
        if (StatClass.BoolIsNgetCust)
        {
            WebLogin.Enabled = true;
        }
        if (StatClass.WebLogin)
        {
            WebLogin.PerformClick();
        }
        if (StatClass.ApptLogin)
        {
            AppLogin_.PerformClick();
        }
        BottomPanel.Width = base.Width;
    }

    private static bool ByteNovaValidityOk()
    {
        if (!Program.UnpaidKey && Program.ExpiryDay > 0) return true;

        string msg = "Aapki validity khatam ho chuki hai, isliye ticket book nahi ho sakta.\r\n\r\n"
                   + "Booking ke liye apna plan renew karayein aur phir software restart karein.";
        Form owner = Program.TopMost_();
        MessageBox.Show(owner, msg, "Validity Expired", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    private void WebLogin_Click(object sender, EventArgs e)
    {
        ResetByteNovaSuccessDesign();
        if (!ByteNovaValidityOk()) return;
        string str = cboGateway.SelectedItem.ToString();
        string app = "WEB";
        if (checkPaymet(str, app))
        {
            Betalogin = true;
            Dishalogin = false;
            Applogin = false;
            loginType = 1;
            ChnageColour_();
            Label label = lbl_status;
            string text2 = "SUCCESS-WEB";
            label.Text = text2;
            NotifyHeaderLoginIcon("Chrome");
            method_4();
        }
        else
        {
            StatClass.WebLogin = false;
            StatClass.DishaLogin = false;
            string text3 = "Selected Payment Gateway is Not Working\r\nPlease Choose Another Payment Option";
            MessageBox.Show(text3);
        }
    }

    private void AppLogin_Click(object sender, EventArgs e)
    {
        ResetByteNovaSuccessDesign();
        if (!ByteNovaValidityOk()) return;
        bool flag = false;
        string text = "SensorList.json";
        string path = text;
        string str = cboGateway.SelectedItem.ToString();
        string app = "APP";
        if (!checkPaymet(str, app))
        {
            StatClass.ApptLogin = false;
            StatClass.DishaLogin = false;
            string text2 = "Selected Payment Gateway is Not Working\r\nPlease Choose Another Payment Option";
            MessageBox.Show(text2);
            return;
        }
        Betalogin = false;
        Dishalogin = false;
        Applogin = true;
        loginType = 2;
        //ChnageColour_();
        Label label = lbl_status;
        string text3 = "SUCCESS-APP";
        label.Text = text3;
        NotifyHeaderLoginIcon("APP");
        method_4();
    }

    private void btnDish_Click(object sender, EventArgs e)
    {
        ResetByteNovaSuccessDesign();
        if (!ByteNovaValidityOk()) return;
        string str = cboGateway.SelectedItem.ToString();
        string app = "Disha";
        if (checkPaymet(str, app))
        {
            Betalogin = false;
            Applogin = false;
            Dishalogin = true;
            loginType = 3;
            //ChnageColour_();
            Label label = lbl_status;
            string text2 = "SUCCESS-Disha";
            label.Text = text2;
            NotifyHeaderLoginIcon("Edge");
            method_4();
        }
        else
        {
            StatClass.ApptLogin = false;
            StatClass.WebLogin = false;
            string text3 = "Selected Payment Gateway is Not Working\r\nPlease Choose Another Payment Option";
            MessageBox.Show(text3);
        }
    }

    private void btnWeb3_Click(object sender, EventArgs e)
    {
        ResetByteNovaSuccessDesign();
        if (!ByteNovaValidityOk()) return;
        string str = cboGateway.SelectedItem.ToString();
        string app = "web3";
        if (!checkPaymet(str, app))
        {
            StatClass.ApptLogin = false;
            StatClass.WebLogin = false;
            string text2 = "Selected Payment Gateway is Not Working\r\nPlease Choose Another Payment Option";
            MessageBox.Show(text2);
            return;
        }
        Betalogin = false;
        Applogin = false;
        Dishalogin = false;
        web3 = true;
        loginType = 4;
        ChnageColour_();
        Label label = lbl_status;
        string text3 = "SUCCESS-APP";
        label.Text = text3;
        NotifyHeaderLoginIcon("Brave");
        method_4();
    }

    private bool checkPaymet(string str, string app)
    {
        return true;
        if (app == "Mix")
        {
            return true;
        }
        if (app == "APP")
        {
            return true;
        }
        string text = str;
        string[] array = new string[1];
        string text2 = "_";
        array[0] = text2;
        string[] array2 = text.Split(array, StringSplitOptions.None);
        string text3 = array2[1];
        string text4 = "WALLET";
        if (text3 == text4)
        {
            string text5 = cboID.Text;
            if (array2[2] != text5)
            {
                Label label = lblStatus;
                string text6 = "Select Same IRCTC and Wallet";
                label.Text = text6;
                return false;
            }
        }
        str = array2[0];
        if (array2.Length > 2)
        {
            string text7 = array2[0];
            string text8 = "_";
            str = text7 + text8 + array2[1];
        }
        bool result = false;
        try
        {
            string text9 = "APP";
            if (app == text9)
            {
                string text10 = str;
                string text11 = "SBI";
                bool flag = text10 == text11;
                string text12 = str;
                string text13 = "HDFC";
                bool flag2 = flag | (text12 == text13);
                string text14 = str;
                string text15 = "PHONEPE";
                bool flag3 = flag2 | (text14 == text15);
                string text16 = str;
                string text17 = "NPCI";
                bool flag4 = flag3 | (text16 == text17);
                string text18 = str;
                string text19 = "ADVANCE_FAIL";
                bool flag5 = flag4 | (text18 == text19);
                string text20 = str;
                string text21 = "ADVANCE_PAY";
                bool flag6 = flag5 | (text20 == text21);
                string text22 = str;
                string text23 = "FREECHARGE";
                bool flag7 = flag6 | (text22 == text23);
                string text24 = str;
                string text25 = "AUTOPAY_UPI";
                bool flag8 = flag7 | (text24 == text25);
                string text26 = str;
                string text27 = "BHIMUPI";
                bool flag9 = flag8 | (text26 == text27);
                string text28 = str;
                string text29 = "PAYZAP_DC";
                bool flag10 = flag9 | (text28 == text29);
                string text30 = str;
                string text31 = "PAYTM";
                bool flag11 = flag10 | (text30 == text31);
                string text32 = str;
                string text33 = "HDFC_DC";
                bool flag12 = flag11 | (text32 == text33);
                string text34 = str;
                string text35 = "AIRTELMONEY";
                result = flag12 | (text34 == text35);
                string text36 = str;
                string text37 = "NSDL_DC";
                result = flag12 | (text36 == text37);
            }
            else
            {
                string text36 = "WEB";
                if (!(app == text36))
                {
                    string text37 = str;
                    string text38 = "NSDL_DC";
                    bool flag13 = text37 == text38;
                    string text39 = str;
                    string text40 = "ICICIVERTUAL_DC";
                    bool flag14 = flag13 | (text39 == text40);
                    string text41 = str;
                    string text42 = "PHONEPE-QR";
                    bool flag15 = flag14 | (text41 == text42);
                    string text43 = str;
                    string text44 = "iPay-QR";
                    bool flag16 = flag15 | (text43 == text44);
                    string text45 = str;
                    string text46 = "PayTM-QR";
                    bool flag17 = flag16 | (text45 == text46);
                    string text47 = str;
                    string text48 = "IRCTC_WALLET";
                    bool flag18 = flag17 | (text47 == text48);
                    string text49 = str;
                    string text50 = "SBI";
                    bool flag19 = flag18 | (text49 == text50);
                    string text51 = str;
                    string text52 = "HDFC";
                    bool flag20 = flag19 | (text51 == text52);
                    string text53 = str;
                    string text54 = "IPAYUPI";
                    bool flag21 = flag20 | (text53 == text54);
                    string text55 = str;
                    string text56 = "SBIUPI";
                    bool flag22 = flag21 | (text55 == text56);
                    string text57 = str;
                    string text58 = "PHONEPE";
                    bool flag23 = flag22 | (text57 == text58);
                    string text59 = str;
                    string text60 = "NPCI";
                    bool flag24 = flag23 | (text59 == text60);
                    string text61 = str;
                    string text62 = "ADVANCE_FAIL";
                    bool flag25 = flag24 | (text61 == text62);
                    string text63 = str;
                    string text64 = "ADVANCE_PAY";
                    bool flag26 = flag25 | (text63 == text64);
                    string text65 = str;
                    string text66 = "FREECHARGE";
                    bool flag27 = flag26 | (text65 == text66);
                    string text67 = str;
                    string text68 = "AUTOPAY_UPI";
                    bool flag28 = flag27 | (text67 == text68);
                    string text69 = str;
                    string text70 = "BHIMUPI";
                    bool flag29 = flag28 | (text69 == text70);
                    string text71 = str;
                    string text72 = "PAYZAP_DC";
                    bool flag30 = flag29 | (text71 == text72);
                    string text73 = str;
                    string text74 = "HDFC_DC";
                    bool flag31 = flag30 | (text73 == text74);
                    string text75 = str;
                    string text76 = "PAYTM";
                    bool flag32 = flag31 | (text75 == text76);
                    string text77 = str;
                    string text78 = "AIRTELMONEY";
                    result = flag32 | (text77 == text78);
                }
                else
                {
                    string text79 = str;
                    string text80 = "NSDL_DC";
                    bool flag33 = text79 == text80;
                    string text81 = str;
                    string text82 = "ICICIVERTUAL_DC";
                    bool flag34 = flag33 | (text81 == text82);
                    string text83 = str;
                    string text84 = "PHONEPE-QR";
                    bool flag35 = flag34 | (text83 == text84);
                    string text85 = str;
                    string text86 = "iPay-QR";
                    bool flag36 = flag35 | (text85 == text86);
                    string text87 = str;
                    string text88 = "PayTM-QR";
                    bool flag37 = flag36 | (text87 == text88);
                    string text89 = str;
                    string text90 = "IRCTC_WALLET";
                    bool flag38 = flag37 | (text89 == text90);
                    string text91 = str;
                    string text92 = "SBI";
                    bool flag39 = flag38 | (text91 == text92);
                    string text93 = str;
                    string text94 = "HDFC";
                    bool flag40 = flag39 | (text93 == text94);
                    string text95 = str;
                    string text96 = "IPAYUPI";
                    bool flag41 = flag40 | (text95 == text96);
                    string text97 = str;
                    string text98 = "SBIUPI";
                    bool flag42 = flag41 | (text97 == text98);
                    string text99 = str;
                    string text100 = "PHONEPE";
                    bool flag43 = flag42 | (text99 == text100);
                    string text101 = str;
                    string text102 = "NPCI";
                    bool flag44 = flag43 | (text101 == text102);
                    string text103 = str;
                    string text104 = "ADVANCE_FAIL";
                    bool flag45 = flag44 | (text103 == text104);
                    string text105 = str;
                    string text106 = "ADVANCE_PAY";
                    bool flag46 = flag45 | (text105 == text106);
                    string text107 = str;
                    string text108 = "FREECHARGE";
                    bool flag47 = flag46 | (text107 == text108);
                    string text109 = str;
                    string text110 = "AUTOPAY_UPI";
                    bool flag48 = flag47 | (text109 == text110);
                    string text111 = str;
                    string text112 = "BHIMUPI";
                    bool flag49 = flag48 | (text111 == text112);
                    string text113 = str;
                    string text114 = "PAYZAP_DC";
                    bool flag50 = flag49 | (text113 == text114);
                    string text115 = str;
                    string text116 = "HDFC_DC";
                    bool flag51 = flag50 | (text115 == text116);
                    string text117 = str;
                    string text118 = "PAYTM";
                    bool flag52 = flag51 | (text117 == text118);
                    string text119 = str;
                    string text120 = "AIRTELMONEY";
                    result = flag52 | (text119 == text120);
                }
            }
        }
        catch
        {
        }
        return result;
    }

    private void method_4()
    {
        if (Applogin)
        {
            Betalogin = false;
            Applogin = true;
            WithCapchtaInputlogin = true;
            WithOtpInputlogin = false;
        }
        else if (Betalogin)
        {
            Betalogin = true;
            Applogin = false;
            WithCapchtaInputlogin = true;
            WithOtpInputlogin = false;
        }
        else
        {
            Betalogin = false;
            Applogin = false;
            WithCapchtaInputlogin = true;
            WithOtpInputlogin = false;
        }
        lblAvailability.Visible = true;
        method_5();

        // ByteNova compact UI: after successful login show the real bottom status panel.
        ShowByteNovaAfterLoginPanel();

        // Login ke baad disabled combo text ke saath Journey VALUE bhi bright yellow/gold.
        if (lblJourney != null)
        {
            lblJourney.ForeColor = Color.FromArgb(255, 215, 0);
            lblJourney.Invalidate();
        }
    }

    private void method_5()
    {
        if (cboSlot.SelectedIndex <= 0)
        {
            cboSlot.SelectedIndex = 1;
        }
        else if (cboID.SelectedIndex <= 0)
        {
            if (StatClass.WebLogin)
            {
                StatClass.WebLogin = false;
            }
            if (StatClass.ApptLogin)
            {
                StatClass.ApptLogin = false;
            }
            string text = "Please select valid IRCTC ID for Booking.";
            string caption = "IRCTC ID";
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
        else if (cboQuota.SelectedIndex >= 0)
        {
            if (cboGateway.SelectedIndex < 0)
            {
                if (StatClass.WebLogin)
                {
                    StatClass.WebLogin = false;
                }
                if (StatClass.ApptLogin)
                {
                    StatClass.ApptLogin = false;
                }
                string text2 = "Please select valid Bank for Booking.";
                string caption2 = "Bank";
                MessageBox.Show(text2, caption2, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                bool_0 = Control.ModifierKeys == Keys.Control;
                if (eventHandler_0 != null)
                {
                    eventHandler_0(this, null);
                }
            }
        }
        else
        {
            if (StatClass.WebLogin)
            {
                StatClass.WebLogin = false;
            }
            if (StatClass.ApptLogin)
            {
                StatClass.ApptLogin = false;
            }
            string text3 = "Please select valid Quota for Booking.";
            string caption3 = "Quota";
            MessageBox.Show(text3, caption3, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }
    }

    private void btnEdit_Click(object sender, EventArgs e)
    {
        string text = btnEdit.Text;
        string text2 = "Info";
        if (text == text2)
        {
            frmAddTkt frmAddTkt2 = new frmAddTkt
            {
                bool_1 = true,
                dictionary_1 = Program.dictionary_0
            };
            string text3 = "Ticket Information";
            frmAddTkt2.Text = text3;
            frmAddTkt2.bool_0 = true;
            frmAddTkt2.EditLoad(class48_0);
            frmAddTkt2.ShowDialog();
        }
        else if (eventHandler_2 != null)
        {
            eventHandler_2(this, e);
        }
    }

    private void BtnCancel_Click_1(object sender, EventArgs e)
    {
        if (eventHandler_1 != null)
        {
            eventHandler_1(this, e);
        }
    }

    private void BAAAA(object sender, EventArgs e)
    {
        string text = lblStatus.Text;
        string value = "Open PNR:";
        if (text.Contains(value))
        {
            try
            {
                string text2 = lblStatus.Text;
                string text3 = lblStatus.Text;
                string value2 = ":";
                text2.Substring(text3.IndexOf(value2) + 1).Trim();
                string text4 = lblPnr.Text;
                string startupPath = Program.StartupPath;
                string path = "PNR";
                string text5 = Path.Combine(startupPath, path);
                string text6 = "\\";
                string path2 = text5 + text6;
                DateTime now = DateTime.Now;
                string text7 = "dd-MMM";
                string text8 = Path.Combine(path2, now.ToString(text7));
                string text9 = "\\";
                string path3 = text8 + text9;
                string[] array = new string[8]
                {
                    cboID.SelectedItem.ToString(),
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null
                };
                string text10 = "_";
                array[1] = text10;
                array[2] = text4;
                string text11 = "_";
                array[3] = text11;
                array[4] = class48_0.string_0;
                string text12 = "_";
                array[5] = text12;
                array[6] = class48_0.string_1;
                string text13 = ".htm";
                array[7] = text13;
                Path.Combine(path3, string.Concat(array));
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string path4 = "Nget_Log";
                string path5 = Path.Combine(folderPath, path4);
                string[] array2 = new string[7];
                string text14 = "ERS_";
                array2[0] = text14;
                array2[1] = text4;
                string text15 = "_";
                array2[2] = text15;
                array2[3] = class48_0.string_0;
                string text16 = "_";
                array2[4] = text16;
                array2[5] = class48_0.string_1;
                string text17 = ".htm";
                array2[6] = text17;
                Process.Start(Path.Combine(path5, string.Concat(array2)));
                return;
            }
            catch
            {
                return;
            }
        }
        string text18 = lblStatus.Text;
        string value3 = "Ready-{ Bkg Cap";
        if (text18.Contains(value3))
        {
            try
            {
                string string_ = class48_0.string_25;
                string[] array3 = new string[1];
                string text19 = "|";
                array3[0] = text19;
                string[] array4 = string_.Split(array3, StringSplitOptions.RemoveEmptyEntries);
                int num = int.Parse(array4[0].Trim());
                int num2 = int.Parse(array4[1].Trim());
                frmOTP frmOTP2 = new frmOTP(bool_2: false, "");
                frmOTP2.StartPosition = FormStartPosition.Manual;
                frmOTP2.Location = PointToScreen(Point.Empty);
                frmOTP2.Location = PointToScreen(base.Location);
                frmOTP2.bool_1 = false;
                frmOTP2.int_0 = int_1;
                string string_2 = class48_0.string_8;
                string text20 = "PT";
                if (string_2 == text20)
                {
                    int_2 = num2;
                    frmOTP2.int_2 = int_2;
                }
                if (frmOTP2.ShowDialog() == DialogResult.OK)
                {
                    int_1 = frmOTP2.int_0;
                    string string_3 = class48_0.string_8;
                    string text21 = "PT";
                    if (string_3 == text21)
                    {
                        int_2 = frmOTP2.int_2;
                        int num3 = int_2;
                        Class48 @class = class48_0;
                        string text22 = num.ToString();
                        string text23 = "|";
                        @class.string_25 = text22 + text23 + num3;
                        fare_str = class48_0.string_25;
                        Label label = lblAvailability;
                        string text24 = label.Text;
                        string text25 = "|";
                        label.Text = text24 + text25 + fare_str;
                    }
                    Label label2 = lblStatus;
                    string[] array5 = new string[5];
                    string text26 = "Ready-{ Bkg Cap: ";
                    array5[0] = text26;
                    array5[1] = (int_1 / 1000).ToString();
                    string text27 = ".";
                    array5[2] = text27;
                    array5[3] = (int_1 % 1000 / 100).ToString();
                    string text28 = " } Fare Limit";
                    array5[4] = text28;
                    label2.Text = string.Concat(array5);
                }
                frmOTP2.Dispose();
                return;
            }
            catch
            {
                return;
            }
        }
        string text29 = lblStatus.Text;
        string caption = "Info";
        MessageBox.Show(text29, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
    }

    public void RefreshSeats(object object_0)
    {
        try
        {
            cboQuota.SelectedItem.ToString();
            string from = class48_0.string_0;
            string to = class48_0.string_1;
            string trainNum = class48_0.string_2;
            string[] array = new string[5] { class48_0.string_4, null, null, null, null };
            string text = "/";
            array[1] = text;
            array[2] = class48_0.string_5;
            string text2 = "/";
            array[3] = text2;
            array[4] = class48_0.string_6;
            string s = string.Concat(array);
            string format = "d/M/yyyy";
            method_6(Class47.method_2A(from, to, trainNum, DateTime.ParseExact(s, format, CultureInfo.InvariantCulture), class48_0.string_7, cboQuota.SelectedItem.ToString()).Split('~')[0]);
        }
        catch
        {
        }
    }

    private void method_6(object object_1)
    {
        if (base.InvokeRequired)
        {
            Invoke(new ParameterizedThreadStart(method_6), object_1);
            return;
        }
        Label label = lblAvailability;
        string text = object_1.ToString();
        string text2 = "|";
        label.Text = text + text2 + fare_str;
    }

    private void lblAvailability_Click(object sender, EventArgs e)
    {
        Label label = lblAvailability;
        string text = "Getting..";
        label.Text = text;
        ThreadPool.QueueUserWorkItem(RefreshSeats);
    }

    private void timer_0_Tick(object sender, EventArgs e)
    {
        Label label = lblTime;
        DateTime dateTime = DateTime.Now.AddMilliseconds(int_3);
        string text = "hh:mm:ss";
        label.Text = dateTime.ToString(text);
    }

    private void cboID_DrawItem(object sender, DrawItemEventArgs e)
    {
        e.DrawBackground();
        string text = ((ComboBox)sender).Items[e.Index].ToString();
        Brush brush = ((!C2AA0.Contains(text)) ? Brushes.Black : Brushes.Red);
        e.Graphics.DrawString(text, ((Control)sender).Font, brush, e.Bounds.X, e.Bounds.Y);
        e.DrawFocusRectangle();
    }

    private void cboID_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lock (object_0)
            {
                if (string_2.Length > 1)
                {
                    Dictionary<string, string> list_SelectedIRCTCid = Program.list_SelectedIRCTCid;
                    string key = string_2;
                    string value = "free";
                    list_SelectedIRCTCid[key] = value;
                    C2AA0.Remove(string_2);
                }
                Dictionary<string, string> list_SelectedIRCTCid2 = Program.list_SelectedIRCTCid;
                string key2 = cboID.SelectedItem.ToString();
                string value2 = "busy";
                list_SelectedIRCTCid2[key2] = value2;
                C2AA0.Add(cboID.SelectedItem.ToString());
                string_2 = cboID.SelectedItem.ToString();
            }
            cboID.Refresh();
        }
        catch
        {
        }
    }


    // ByteNova modern skin.
    // IMPORTANT: no new feature/control is added; only existing controls are restyled.
    private void RestoreApprovedGeometry()
    {
        if (!_byteNovaLargeLayoutApplied) return;
        Size = new Size(330, 176);
        HideLegacyStatusOverlayControls();
        if (lblClass != null) { lblClass.Visible = false; lblClass.SendToBack(); }
        if (lblDate != null) { lblDate.Visible = false; lblDate.SendToBack(); }
        if (lblTktName != null) lblTktName.Visible = false;
        if (lblInfo != null) lblInfo.Visible = false;
        if (label5 != null) label5.Visible = false;
        if (lblBrand != null) lblBrand.Visible = false;
        if (lblClass != null) { lblClass.Visible = false; lblClass.SendToBack(); }
        if (cboTrain != null) cboTrain.SetBounds(197, 41, 64, 21);
        if (cboQuota != null) { cboQuota.SetBounds(265, 41, 58, 21); cboQuota.Visible = true; cboQuota.BringToFront(); }
        if (cboID != null) cboID.SetBounds(70, 76, 87, 22);
        if (cboSlot != null) cboSlot.SetBounds(201, 76, 58, 22);
        if (BtnOpenPair != null) BtnOpenPair.SetBounds(264, 75, 59, 24);
        if (chkConfirmation != null) chkConfirmation.SetBounds(12, 108, 54, 22);
        if (cboGateway != null) cboGateway.SetBounds(131, 108, 189, 22);

        if (_byteNovaLoginCompleted)
        {
            ShowByteNovaAfterLoginPanel();
        }
        else
        {
            LayoutReferenceButton(WebLogin, 6, 139, 77, 32, "Chrome", "C", Color.FromArgb(12, 88, 190));
            LayoutReferenceButton(btnDish, 87, 139, 77, 32, "Edge", "E", Color.FromArgb(72, 44, 170));
            LayoutReferenceButton(btnWeb3, 168, 139, 77, 32, "Brave", "B", Color.FromArgb(178, 98, 10));
            LayoutReferenceButton(AppLogin_, 249, 139, 75, 32, "APP", "A", Color.FromArgb(0, 126, 100));
        }
        ApplyByteNovaDisabledComboTextToAll();
        Invalidate();
    }

    private bool _byteNovaLargeLayoutApplied;
    private bool _byteNovaLoginCompleted;
    private string selectredbank;

    private void ApplyApprovedLargeWindowLayout()
    {
        if (lblStatus != null)
        {
            lblStatus.ForeColor = Color.White;
            lblStatus.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
        }
        if (chkConfirmation != null) chkConfirmation.Text = "Payment Stop";
        if (_byteNovaLargeLayoutApplied) return;
        _byteNovaLargeLayoutApplied = true;

        SuspendLayout();
        AutoScaleMode = AutoScaleMode.None;
        Size = new Size(330, 176);
        MinimumSize = new Size(330, 176);
        BackColor = Color.FromArgb(1, 13, 31);

        // Re-parent only the existing functional controls. No option/event is deleted.
        Control[] keep =
        {
            cboID, cboSlot, cboGateway, cboQuota, cboTrain, cboClass, fake_bank, fake_id,
            BtnOpenPair, btnSlotNo, chkConfirmation, check_avl,
            WebLogin, btnDish, btnWeb3, AppLogin_,
            btnEdit, btnCancel, btnMinize, button1, button2,
            lblJourney, lblTrainNo, lblClass, lblDate, lblTktName
        };
        foreach (Control c in keep)
            if (c != null && c.Parent != this) Controls.Add(c);

        // Legacy containers remain in the class for all original logic, but the approved
        // compact card is drawn directly so nested designer geometry cannot overlap it.
        Panel[] oldPanels = { TopPanel, HeaderPanel1, Panel1, PanelPnr, Panel5, panel2, pnlBrand, B3AAA, BottomPanel };
        foreach (Panel p in oldPanels) if (p != null) p.Visible = false;
        if (pictureBox2 != null) pictureBox2.Visible = false;

        // Prevent old designer header/title controls from drawing over the new header.
        if (lblTktName != null) lblTktName.Visible = false;
        if (lblInfo != null) lblInfo.Visible = false;
        if (label5 != null) label5.Visible = false;
        if (lblBrand != null) lblBrand.Visible = false;
        if (lbl_brand != null) lbl_brand.Visible = false;
        if (lbl_Detail != null) lbl_Detail.Visible = false;

        // Values shown in the reference card.
        SetCompactValue(lblJourney, 70, 41, 82, 20, ContentAlignment.MiddleLeft);
        if (lblTrainNo != null) lblTrainNo.Visible = false;

        // Journey shows FROM_TO; Train No uses the existing functional train ComboBox.
        if (lblJourney != null && class48_0 != null)
        {
            lblJourney.Text = class48_0.string_0 + "_" + class48_0.string_1;
            lblJourney.Visible = true;
            lblJourney.BringToFront();
        }
        if (lblTrainNo != null) lblTrainNo.Visible = false;
        if (cboTrain != null) { cboTrain.SetBounds(197, 41, 64, 21); cboTrain.Visible = true; cboTrain.BringToFront(); }
        if (cboClass != null) { cboClass.SetBounds(0, 0, 1, 1); }   // retained; compact detail value is painted
        if (cboQuota != null) { cboQuota.SetBounds(265, 41, 58, 21); cboQuota.Visible = true; cboQuota.BringToFront(); }

        // IRCTC ID + Slot + Pair.
        if (cboID != null) { cboID.SetBounds(70, 76, 87, 22); cboID.Visible = true; cboID.BringToFront(); }
        if (cboSlot != null) { cboSlot.SetBounds(201, 76, 58, 22); cboSlot.Visible = true; cboSlot.BringToFront(); }
        if (BtnOpenPair != null)
        {
            BtnOpenPair.SetBounds(264, 75, 59, 24);
            BtnOpenPair.Text = "";
            BtnOpenPair.Visible = true;
            BtnOpenPair.UseVisualStyleBackColor = false;
            BtnOpenPair.FlatStyle = FlatStyle.Flat;
            BtnOpenPair.FlatAppearance.BorderSize = 1;
            BtnOpenPair.FlatAppearance.BorderColor = Color.FromArgb(0, 235, 195);
            BtnOpenPair.BackColor = Color.FromArgb(0, 145, 118);
            BtnOpenPair.Paint -= ByteNovaPairButtonPaint;
            BtnOpenPair.Paint += ByteNovaPairButtonPaint;
            ApplyRoundedRegion(BtnOpenPair, 6);
            BtnOpenPair.BringToFront();
        }

        // Stop + payment gateway.
        if (chkConfirmation != null)
        {
            chkConfirmation.SetBounds(12, 108, 54, 22);
            chkConfirmation.Text = "Payment Stop";
            chkConfirmation.Visible = true;
            chkConfirmation.BringToFront();
        }
        if (cboGateway != null)
        {
            cboGateway.SetBounds(131, 108, 189, 22);
            cboGateway.Visible = true;
            cboGateway.BringToFront();
        }

        // Keep secondary options alive without allowing legacy geometry to cover the card.
        // These legacy surfaces were appearing above the Ready/status strip.
        // Controls remain in the class; only their duplicate visual surface is hidden.
        if (check_avl != null) { check_avl.Visible = false; check_avl.TabStop = false; }
        if (fake_id != null) { fake_id.Visible = false; fake_id.TabStop = false; }
        if (fake_bank != null) { fake_bank.Visible = false; fake_bank.TabStop = false; }
        if (btnSlotNo != null) { btnSlotNo.Visible = false; btnSlotNo.TabStop = false; }
        if (btnEdit != null) { btnEdit.Visible = false; btnEdit.TabStop = false; }
        if (btnCancel != null) { btnCancel.Visible = false; btnCancel.TabStop = false; }
        if (btnMinize != null) { btnMinize.Visible = false; btnMinize.TabStop = false; }
        if (button1 != null) { button1.Visible = false; button1.TabStop = false; }
        if (button2 != null) { button2.Visible = false; button2.TabStop = false; }

        HideLegacyStatusOverlayControls();
        if (lblDate != null) { lblDate.Visible = false; lblDate.SendToBack(); }
        if (lblClass != null) { lblClass.Visible = false; lblClass.SendToBack(); }
        if (BtnOpenPair != null)
        {
            BtnOpenPair.Text = "";
            BtnOpenPair.Paint -= ByteNovaPairButtonPaint;
            BtnOpenPair.Paint += ByteNovaPairButtonPaint;
            BtnOpenPair.Invalidate();
        }

        LayoutReferenceButton(WebLogin, 6, 139, 77, 32, "Chrome", "C", Color.FromArgb(12, 88, 190));
        LayoutReferenceButton(btnDish, 87, 139, 77, 32, "Edge", "E", Color.FromArgb(72, 44, 170));
        LayoutReferenceButton(btnWeb3, 168, 139, 77, 32, "Brave", "B", Color.FromArgb(178, 98, 10));
        LayoutReferenceButton(AppLogin_, 249, 139, 75, 32, "APP", "A", Color.FromArgb(0, 126, 100));

        ApplyByteNovaDisabledComboTextToAll();

        Paint -= ByteNovaReferencePaint;
        Paint += ByteNovaReferencePaint;

        if (lblStatus != null)
        {
            lblStatus.TextChanged -= ByteNovaStatusTextChanged;
            lblStatus.TextChanged += ByteNovaStatusTextChanged;
        }

        ResumeLayout(true);
        PerformLayout();
        LockByteNovaCompactLayout();
        Invalidate();
    }

    private void HideLegacyStatusOverlayControls()
    {
        // Duplicate legacy labels that are no longer used by the painted compact status strip.
        Label[] duplicateLabels =
        {
            lblInfo, lbl_status, lbl_slot,
            lbl_Detail, lbl_brand, lblBrand, lbl_time, lbl_bnk,
            lblPnr, lbl_Month, lbl_Date, lblDate, label5
        };

        foreach (Label l in duplicateLabels)
        {
            if (l == null) continue;
            l.Visible = false;
            l.SendToBack();
        }

        if (!_byteNovaLoginCompleted)
        {
            if (lblAvailability != null) { lblAvailability.Visible = false; lblAvailability.SendToBack(); }
            if (lblTime != null) { lblTime.Visible = false; lblTime.SendToBack(); }
        }

        // Remove the extra physical date label (e.g. the circled "08-11").
        // The date is still painted once at the right side of the Ready/status strip.
        string displayedDate = lblDate != null ? lblDate.Text : "";
        foreach (Control c in Controls)
        {
            Label l = c as Label;
            if (l == null || l == lblJourney || l == lblTrainNo) continue;
            if (!string.IsNullOrWhiteSpace(displayedDate) &&
                string.Equals(l.Text.Trim(), displayedDate.Trim(), StringComparison.OrdinalIgnoreCase))
            {
                l.Visible = false;
                l.SendToBack();
            }
        }

        // Fast Availability checkbox is intentionally not shown in this compact reference UI.
        // Its object/event wiring remains untouched.
        if (check_avl != null)
        {
            check_avl.Visible = false;
            check_avl.SendToBack();
        }

        // Prevent hidden legacy action buttons from leaking into the status/header area.
        Button[] legacyButtons = { btnSlotNo, btnEdit, btnCancel, btnMinize, button1, button2 };
        foreach (Button b in legacyButtons)
        {
            if (b == null) continue;
            b.Visible = false;
            b.SendToBack();
        }
    }

    private void ByteNovaPairButtonPaint(object sender, PaintEventArgs e)
    {
        Button b = sender as Button;
        if (b == null) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (Brush bg = new SolidBrush(Color.FromArgb(0, 145, 118)))
            e.Graphics.FillRectangle(bg, b.ClientRectangle);

        // Proper chain-link icon.
        using (Pen p = new Pen(Color.White, 1.5F))
        {
            p.StartCap = LineCap.Round;
            p.EndCap = LineCap.Round;
            RectangleF leftLink = new RectangleF(8, (b.Height - 10) / 2f, 12, 8);
            RectangleF rightLink = new RectangleF(16, (b.Height - 10) / 2f, 12, 8);
            e.Graphics.DrawArc(p, leftLink, 45, 270);
            e.Graphics.DrawArc(p, rightLink, 225, 270);
            e.Graphics.DrawLine(p, 16, b.Height / 2f, 21, b.Height / 2f);
        }

        Rectangle textRect = new Rectangle(30, 0, Math.Max(1, b.Width - 32), b.Height);
        using (Font f = new Font("Segoe UI Semibold", 7F, FontStyle.Bold))
            TextRenderer.DrawText(e.Graphics, "Pair", f, textRect, Color.White,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.NoPadding);
    }

    private void LockByteNovaCompactLayout()
    {
        if (!_byteNovaLargeLayoutApplied) return;

        if (_byteNovaLoginCompleted)
        {
            ShowByteNovaAfterLoginPanel();
            return;
        }

        SuspendLayout();

        // Initial compact view: browser/login buttons are visible.
        if (BottomPanel != null)
        {
            BottomPanel.Dock = DockStyle.None;
            BottomPanel.Visible = false;
            BottomPanel.SendToBack();
        }

        Size = new Size(330, 176);
        LayoutReferenceButton(WebLogin, 6, 139, 77, 32, "Chrome", "C", Color.FromArgb(12, 88, 190));
        LayoutReferenceButton(btnDish, 87, 139, 77, 32, "Edge", "E", Color.FromArgb(72, 44, 170));
        LayoutReferenceButton(btnWeb3, 168, 139, 77, 32, "Brave", "B", Color.FromArgb(178, 98, 10));
        LayoutReferenceButton(AppLogin_, 249, 139, 75, 32, "APP", "A", Color.FromArgb(0, 126, 100));

        if (lblAvailability != null) lblAvailability.Visible = false;
        if (lblTime != null) lblTime.Visible = false;

        HideLegacyStatusOverlayControls();

        ResumeLayout(true);
        Invalidate();
    }

    private void ShowByteNovaAfterLoginPanel()
    {
        if (BottomPanel != null)
        {
            // Square panel: remove any legacy rounded/curved Region.
            BottomPanel.Region = null;
            BottomPanel.BackColor = Color.FromArgb(18, 52, 82);
        }

        // Bottom status text: larger + bold.
        if (lblAvailability != null)
        {
            lblAvailability.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            lblAvailability.ForeColor = Color.FromArgb(220, 245, 255);
        }
        if (lblTime != null)
        {
            lblTime.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTime.ForeColor = Color.FromArgb(220, 245, 255);
        }

        // Keep BottomPanel visually merged with the main ByteNova window.
        if (BottomPanel != null)
            BottomPanel.BackColor = Color.FromArgb(18, 52, 82);

        if (!_byteNovaLargeLayoutApplied) return;

        _byteNovaLoginCompleted = true;

        if (InvokeRequired)
        {
            BeginInvoke(new MethodInvoker(ShowByteNovaAfterLoginPanel));
            return;
        }

        SuspendLayout();

        // Login completed: hide all four login buttons.
        Button[] loginButtons = { WebLogin, btnDish, btnWeb3, AppLogin_ };
        foreach (Button b in loginButtons)
        {
            if (b == null) continue;
            b.Visible = false;
            b.Enabled = false;
            b.SendToBack();
        }

        // Existing BottomPanel becomes the bottom status area.
        if (BottomPanel != null)
        {
            BottomPanel.Dock = DockStyle.None;
            BottomPanel.Visible = true;
            BottomPanel.BackColor = Color.FromArgb(22, 20, 58);
            // Screenshot target: fill the complete lower free area edge-to-edge.
            // Start immediately after the Stop/Gateway row and extend to the client bottom.
            int bottomTop = 134;
            int bottomHeight = Math.Max(1, ClientSize.Height - bottomTop);
            BottomPanel.Margin = Padding.Empty;
            BottomPanel.Padding = Padding.Empty;
            BottomPanel.Dock = DockStyle.None;
            // Match the complete TCntrl/base window width exactly after login.
            int baseWindowWidth = base.ClientSize.Width;
            BottomPanel.SetBounds(0, bottomTop, baseWindowWidth, bottomHeight);
            BottomPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            BottomPanel.Visible = true;
            BottomPanel.BringToFront();
        }

        // Show existing availability + time labels inside two frmMain-style curved cards.
        if (lblAvailability != null)
        {
            if (lblAvailability.Parent != BottomPanel && BottomPanel != null)
                BottomPanel.Controls.Add(lblAvailability);

            lblAvailability.AutoSize = false;
            int cardH = BottomPanel != null ? Math.Max(24, BottomPanel.ClientSize.Height - 8) : 28;
            int timeWidth = 108;
            int gap = 8;
            int availabilityWidth = BottomPanel != null
                ? Math.Max(100, BottomPanel.ClientSize.Width - timeWidth - gap - 16)
                : 214;
            lblAvailability.SetBounds(8, 4, availabilityWidth, cardH);
            lblAvailability.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lblAvailability.TextAlign = ContentAlignment.MiddleLeft;
            // Leave room for the availability icon painted inside the curved card.
            lblAvailability.Padding = new Padding(30, 0, 5, 0);
            lblAvailability.BackColor = Color.Transparent;
            lblAvailability.ForeColor = Color.FromArgb(55, 245, 180);
            lblAvailability.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblAvailability.Visible = true;
            lblAvailability.BringToFront();
        }

        if (lblTime != null)
        {
            if (lblTime.Parent != BottomPanel && BottomPanel != null)
                BottomPanel.Controls.Add(lblTime);

            lblTime.AutoSize = false;
            int timeWidth = 108;
            int cardH = BottomPanel != null ? Math.Max(24, BottomPanel.ClientSize.Height - 8) : 28;
            int timeX = BottomPanel != null ? Math.Max(0, BottomPanel.ClientSize.Width - timeWidth - 8) : 224;
            lblTime.SetBounds(timeX, 4, timeWidth, cardH);
            lblTime.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            lblTime.TextAlign = ContentAlignment.MiddleCenter;
            // Leave room for the clock icon painted inside the right curved card.
            lblTime.Padding = new Padding(24, 0, 8, 0);
            lblTime.BackColor = Color.Transparent;
            lblTime.ForeColor = Color.White;
            lblTime.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTime.Visible = true;
            lblTime.BringToFront();
        }

        if (BottomPanel != null)
        {
            BottomPanel.Paint -= ByteNovaBottomStatusPaint;
            BottomPanel.Paint += ByteNovaBottomStatusPaint;
            BottomPanel.Invalidate();
        }

        ResumeLayout(true);
        Invalidate();
    }

    private void ByteNovaBottomStatusPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null || c.Width < 2 || c.Height < 2) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        using (SolidBrush bg = new SolidBrush(Color.FromArgb(18, 52, 82)))
            e.Graphics.FillRectangle(bg, c.ClientRectangle);

        // frmMain-style curved card behind Get Availability.
        if (lblAvailability != null && lblAvailability.Visible)
        {
            DrawByteNovaBottomCard(e.Graphics, lblAvailability.Bounds);
            DrawByteNovaAvailabilityIcon(e.Graphics, lblAvailability.Bounds);
        }

        // frmMain-style curved card behind time.
        if (lblTime != null && lblTime.Visible)
        {
            DrawByteNovaBottomCard(e.Graphics, lblTime.Bounds);
            DrawByteNovaBottomClockIcon(e.Graphics, lblTime.Bounds);
        }

        Rectangle border = new Rectangle(0, 0, Math.Max(0, c.ClientSize.Width - 1), Math.Max(0, c.ClientSize.Height - 1));
        using (Pen p = new Pen(Color.FromArgb(91, 211, 255), 1F))
            e.Graphics.DrawRectangle(p, border);
    }

    private void DrawByteNovaAvailabilityIcon(Graphics g, Rectangle bounds)
    {
        // Compact railway/seat-style status icon before the availability value.
        int size = Math.Min(16, Math.Max(12, bounds.Height - 12));
        int x = bounds.X + 9;
        int y = bounds.Y + (bounds.Height - size) / 2;
        Rectangle icon = new Rectangle(x, y, size, size);

        using (Pen p = new Pen(Color.FromArgb(55, 245, 180), 1.5F))
        using (SolidBrush dot = new SolidBrush(Color.FromArgb(55, 245, 180)))
        {
            p.StartCap = LineCap.Round;
            p.EndCap = LineCap.Round;

            // Small ticket/availability symbol.
            Rectangle ticket = new Rectangle(icon.X + 1, icon.Y + 3, icon.Width - 2, icon.Height - 6);
            g.DrawRectangle(p, ticket);
            g.FillEllipse(dot, icon.X + 4, icon.Y + 6, 2, 2);
            g.DrawLine(p, icon.X + 8, icon.Y + 7, icon.Right - 3, icon.Y + 7);
            g.DrawLine(p, icon.X + 4, icon.Y + 11, icon.Right - 3, icon.Y + 11);
        }
    }

    private void DrawByteNovaBottomClockIcon(Graphics g, Rectangle bounds)
    {
        // Compact cyan clock icon before the time value.
        int size = Math.Min(16, Math.Max(12, bounds.Height - 12));
        int x = bounds.X + 8;
        int y = bounds.Y + (bounds.Height - size) / 2;
        Rectangle clock = new Rectangle(x, y, size, size);

        using (Pen p = new Pen(Color.FromArgb(91, 211, 255), 1.6F))
        {
            p.StartCap = LineCap.Round;
            p.EndCap = LineCap.Round;

            g.DrawEllipse(p, clock);

            int cx = clock.X + clock.Width / 2;
            int cy = clock.Y + clock.Height / 2;

            g.DrawLine(p, cx, cy, cx, clock.Y + 4);
            g.DrawLine(p, cx, cy, clock.Right - 4, cy + 2);
        }
    }

    private void DrawByteNovaBottomCard(Graphics g, Rectangle bounds)
    {
        Rectangle r = new Rectangle(bounds.X, bounds.Y, Math.Max(1, bounds.Width - 1), Math.Max(1, bounds.Height - 1));
        int radius = Math.Min(10, Math.Max(5, r.Height / 2));

        using (GraphicsPath path = ByteNovaBottomRoundedRect(r, radius))
        using (LinearGradientBrush fill = new LinearGradientBrush(
            r,
            Color.FromArgb(7, 40, 68),
            Color.FromArgb(5, 24, 48),
            LinearGradientMode.Vertical))
        using (Pen border = new Pen(Color.FromArgb(198, 244, 50), 1.4F))
        {
            g.FillPath(fill, path);
            g.DrawPath(border, path);
        }
    }

    private GraphicsPath ByteNovaBottomRoundedRect(Rectangle r, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        int d = radius * 2;
        path.AddArc(r.Left, r.Top, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Top, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    private void ByteNovaStatusTextChanged(object sender, EventArgs e)
    {
        // StatusChange() updates lblStatus.Text. Repaint our custom status strip immediately.
        if (InvokeRequired)
        {
            BeginInvoke(new MethodInvoker(delegate { Invalidate(); Update(); }));
            return;
        }

        Invalidate(new Rectangle(7, 7, 316, 27));
        Update();
    }

    private void SetCompactValue(Label label, int x, int y, int w, int h, ContentAlignment align)
    {
        if (label == null) return;
        label.AutoSize = false;
        label.SetBounds(x, y, w, h);
        label.TextAlign = align;
        label.BackColor = Color.Transparent;
        label.ForeColor = Color.FromArgb(225, 235, 255);
        label.Font = new Font("Segoe UI Semibold", 7.5F, FontStyle.Bold);
        label.Visible = true;
        label.BringToFront();
    }

    private void LayoutReferenceButton(Button b, int x, int y, int w, int h, string text, string glyph, Color accent)
    {
        if (b == null) return;
        if (b.Parent != this) Controls.Add(b);

        b.AutoSize = false;
        b.SetBounds(x, y, w, h);
        b.Visible = true;
        b.UseVisualStyleBackColor = false;
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 1;
        b.FlatAppearance.BorderColor = ControlPaint.Light(accent, .25F);
        b.BackColor = accent;
        b.ForeColor = Color.White;
        b.Font = new Font("Segoe UI Semibold", 7F, FontStyle.Bold);
        b.Image = null;
        b.Text = "";
        b.Tag = new object[] { text, glyph, accent };

        b.Paint -= ByteNovaActionButtonPaint;
        b.Paint += ByteNovaActionButtonPaint;

        ApplyRoundedRegion(b, 7);
        b.BringToFront();
        b.Invalidate();
    }

    private void ByteNovaActionButtonPaint(object sender, PaintEventArgs e)
    {
        Button b = sender as Button;
        if (b == null || !(b.Tag is object[] data) || data.Length < 3) return;

        string text = Convert.ToString(data[0]);
        Color accent = (Color)data[2];

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle r = new Rectangle(1, 1, b.Width - 3, b.Height - 3);

        // Soft glow
        using (GraphicsPath glowPath = ByteNovaButtonRound(r, 7))
        using (Pen glow = new Pen(Color.FromArgb(70, 0, 200, 255), 3F))
            e.Graphics.DrawPath(glow, glowPath);

        // Gradient face
        using (GraphicsPath path = ByteNovaButtonRound(r, 6))
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, ControlPaint.Light(accent, 0.30F), ControlPaint.Dark(accent, 0.25F),
            LinearGradientMode.Vertical))
        using (Pen border = new Pen(Color.FromArgb(198, 244, 50), 1.1F))
        {
            e.Graphics.FillPath(bg, path);
            e.Graphics.DrawPath(border, path);

            // Top shine
            Rectangle shine = new Rectangle(r.X + 2, r.Y + 2, r.Width - 4, Math.Max(2, r.Height / 2));
            using (LinearGradientBrush sb = new LinearGradientBrush(
                shine, Color.FromArgb(70, 255, 255, 255), Color.FromArgb(0, 255, 255, 255),
                LinearGradientMode.Vertical))
            using (GraphicsPath sp = ByteNovaButtonRound(shine, 5))
                e.Graphics.FillPath(sb, sp);
        }

        Rectangle iconRect = new Rectangle(9, (b.Height - 18) / 2, 18, 18);
        DrawAppIcon(e.Graphics, iconRect, text);

        Rectangle textRect = new Rectangle(31, 0, Math.Max(1, b.Width - 34), b.Height);
        using (Font tf = new Font("Segoe UI Semibold", 7.2F, FontStyle.Bold))
            TextRenderer.DrawText(e.Graphics, text, tf, textRect, Color.White,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
    }

    private GraphicsPath ByteNovaButtonRound(Rectangle r, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        int d = radius * 2;
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    private void DrawAppIcon(Graphics g, Rectangle r, string app)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;

        if (string.Equals(app, "Chrome", StringComparison.OrdinalIgnoreCase))
        {
            // Chrome-style multicolour wheel.
            using (Brush red = new SolidBrush(Color.FromArgb(235, 70, 55)))
                g.FillPie(red, r, 210, 120);
            using (Brush yellow = new SolidBrush(Color.FromArgb(250, 190, 35)))
                g.FillPie(yellow, r, 330, 120);
            using (Brush green = new SolidBrush(Color.FromArgb(35, 175, 80)))
                g.FillPie(green, r, 90, 120);
            Rectangle c = new Rectangle(r.X + 5, r.Y + 5, r.Width - 10, r.Height - 10);
            using (Brush blue = new SolidBrush(Color.FromArgb(35, 120, 225))) g.FillEllipse(blue, c);
            using (Pen white = new Pen(Color.White, 1F)) g.DrawEllipse(white, c);
            return;
        }

        if (string.Equals(app, "Edge", StringComparison.OrdinalIgnoreCase))
        {
            using (Brush cyan = new SolidBrush(Color.FromArgb(0, 210, 220))) g.FillEllipse(cyan, r);
            Rectangle inner = new Rectangle(r.X + 4, r.Y + 4, r.Width - 8, r.Height - 8);
            using (Brush blue = new SolidBrush(Color.FromArgb(35, 75, 220))) g.FillPie(blue, inner, 205, 260);
            using (Font f = new Font("Segoe UI Semibold", 9F, FontStyle.Bold))
            using (Brush w = new SolidBrush(Color.White))
                g.DrawString("e", f, w, r.X + 5, r.Y + 1);
            return;
        }

        if (string.Equals(app, "Brave", StringComparison.OrdinalIgnoreCase))
        {
            using (Brush orange = new SolidBrush(Color.FromArgb(255, 105, 30))) g.FillEllipse(orange, r);
            using (Font f = new Font("Segoe UI Semibold", 9F, FontStyle.Bold))
            using (Brush w = new SolidBrush(Color.White))
                g.DrawString("B", f, w, r.X + 5, r.Y + 1);
            return;
        }

        if (string.Equals(app, "APP", StringComparison.OrdinalIgnoreCase))
        {
            // Android-style robot.
            using (Pen p = new Pen(Color.White, 1.5F))
            {
                g.DrawArc(p, r.X + 4, r.Y + 5, 12, 10, 180, 180);
                g.DrawLine(p, r.X + 6, r.Y + 6, r.X + 4, r.Y + 3);
                g.DrawLine(p, r.X + 14, r.Y + 6, r.X + 16, r.Y + 3);
                g.DrawRectangle(p, r.X + 5, r.Y + 10, 10, 7);
                g.DrawLine(p, r.X + 7, r.Y + 17, r.X + 7, r.Y + 20);
                g.DrawLine(p, r.X + 13, r.Y + 17, r.X + 13, r.Y + 20);
            }
            using (Brush w = new SolidBrush(Color.White))
            {
                g.FillEllipse(w, r.X + 7, r.Y + 8, 2, 2);
                g.FillEllipse(w, r.X + 12, r.Y + 8, 2, 2);
            }
            return;
        }

        using (Brush dark = new SolidBrush(Color.FromArgb(35, 0, 0, 0))) g.FillEllipse(dark, r);
        using (Pen white = new Pen(Color.White, 1.2F)) g.DrawEllipse(white, r);
    }

    private void ByteNovaReferencePaint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        if (_byteNovaFinalSuccessDesign)
        {
            DrawByteNovaFinalSuccessReference(g);
            return;
        }

        // No header here. frmProcessTicket owns the window header.
        Rectangle outer = new Rectangle(2, 2, Width - 5, Height - 5);
        using (Pen glow = new Pen(Color.FromArgb(0, 195, 255), 1.5F))
            g.DrawRectangle(glow, outer);
        using (Pen inner = new Pen(Color.FromArgb(55, 75, 120, 255), 1F))
            g.DrawRectangle(inner, new Rectangle(5, 5, Width - 11, Height - 11));

        // Status strip starts at the top of TCntrl.
        // IMPORTANT: this is the status surface the user actually sees.
        bool statusBlinking = timer_1 != null && timer_1.Enabled;
        Color statusTop = statusBlinking
            ? (_byteNovaStatusBlinkOn ? Color.FromArgb(0, 128, 205) : Color.FromArgb(190, 140, 10))
            : Color.FromArgb(0, 118, 168);
        Color statusBottom = statusBlinking
            ? (_byteNovaStatusBlinkOn ? Color.FromArgb(0, 62, 120) : Color.FromArgb(120, 82, 5))
            : Color.FromArgb(1, 52, 88);
        Color statusTextColor = Color.White;
        Color statusDotColor = Color.FromArgb(0, 245, 185);

        Rectangle statusBlinkRect = new Rectangle(7, 7, 316, 27);
        using (LinearGradientBrush statusBg = new LinearGradientBrush(
            statusBlinkRect, statusTop, statusBottom, LinearGradientMode.Vertical))
            g.FillRectangle(statusBg, statusBlinkRect);
        using (Pen statusBorder = new Pen(Color.FromArgb(198, 244, 50), 1.2F))
            g.DrawRectangle(statusBorder, statusBlinkRect);
        using (Pen innerLine = new Pen(Color.FromArgb(90, 150, 240, 255), 1F))
            g.DrawRectangle(innerLine, new Rectangle(9, 9, 312, 23));

        using (Brush dot = new SolidBrush(statusDotColor))
            g.FillEllipse(dot, 15, 17, 9, 9);
        using (Font f = new Font("Segoe UI Semibold", 10F, FontStyle.Bold))
        using (Brush b = new SolidBrush(statusTextColor))
        {
            string liveStatus = (lblStatus != null && !string.IsNullOrWhiteSpace(lblStatus.Text))
                ? lblStatus.Text.Trim()
                : "Ready [ Bkg Fare Limit : 0 ]";
            g.DrawString(liveStatus, f, b, new RectangleF(30, 12, 215, 16));
        }

        DrawReferenceCard(g, new Rectangle(7, 37, 316, 29), Color.FromArgb(20, 90, 170));
        DrawMiniIcon(g, 14, 44, "⌖");
        DrawCaption(g, "Journey", 31, 45);
        DrawCaption(g, "Train No.", 140, 45);
        DrawMiniIcon(g, 184, 44, "#");

        DrawReferenceCard(g, new Rectangle(7, 70, 316, 33), Color.FromArgb(20, 90, 170));
        DrawMiniIcon(g, 14, 78, "I");
        DrawCaption(g, "IRCTC ID", 31, 79);
        DrawMiniIcon(g, 168, 78, "S");
        DrawCaption(g, "Slot", 184, 79);

        DrawReferenceCard(g, new Rectangle(7, 105, 316, 28), Color.FromArgb(20, 90, 170));
    }

    private void DrawByteNovaFinalSuccessReference(Graphics g)
    {
        Rectangle all = ClientRectangle;
        using (LinearGradientBrush bg = new LinearGradientBrush(
            all, Color.FromArgb(4, 13, 30), Color.FromArgb(1, 31, 58),
            LinearGradientMode.Vertical))
            g.FillRectangle(bg, all);
        // Reference-style FINAL StatusChange(5) status strip.
        Rectangle statusOuter = new Rectangle(5, 5, 320, 38);
        using (Pen cyanOuter = new Pen(Color.FromArgb(198, 244, 50), 1.4F))
            g.DrawRectangle(cyanOuter, statusOuter);

        Rectangle statusCard = new Rectangle(9, 12, 312, 37);
        using (GraphicsPath statusPath = new GraphicsPath())
        {
            int radius = 14;
            int d = radius * 2;
            statusPath.AddArc(statusCard.X, statusCard.Y, d, d, 90, 180);
            statusPath.AddArc(statusCard.Right - d, statusCard.Y, d, d, 270, 180);
            statusPath.CloseFigure();
            using (SolidBrush black = new SolidBrush(Color.FromArgb(4, 5, 7)))
                g.FillPath(black, statusPath);
            using (Pen goldBorder = new Pen(Color.FromArgb(255, 214, 45), 2.4F))
                g.DrawPath(goldBorder, statusPath);
        }

        string success = string.IsNullOrWhiteSpace(_byteNovaFinalSuccessText)
            ? ""
            : _byteNovaFinalSuccessText.Trim();

        // True geometric centering inside the golden curved box.
        RectangleF statusTextRect = new RectangleF(17F, 9F, 194F, 37F);
        using (Font statusFont = new Font("Arial Black", 10.5F, FontStyle.Bold))
        using (Brush statusBrush = new SolidBrush(Color.FromArgb(255, 218, 65)))
        using (StringFormat statusFormat = new StringFormat())
        {
            statusFormat.Alignment = StringAlignment.Near;
            statusFormat.LineAlignment = StringAlignment.Center;
            statusFormat.FormatFlags = StringFormatFlags.NoWrap;
            statusFormat.Trimming = StringTrimming.EllipsisCharacter;
            g.DrawString(success, statusFont, statusBrush, statusTextRect, statusFormat);
        }

        using (Pen sep = new Pen(Color.FromArgb(255, 218, 65), 1.4F))
            g.DrawLine(sep, 214, 10, 214, 40);

        Rectangle clockRect = new Rectangle(217, 16, 20, 20);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using (Pen clockPen = new Pen(Color.FromArgb(255, 218, 65), 2.2F))
        {
            g.DrawEllipse(clockPen, clockRect);
            int cx = clockRect.X + clockRect.Width / 2;
            int cy = clockRect.Y + clockRect.Height / 2;
            g.DrawLine(clockPen, cx, cy, cx, clockRect.Y + 5);
            g.DrawLine(clockPen, cx, cy, clockRect.Right - 5, cy + 3);
        }

        string statusTime = (lblTime != null && !string.IsNullOrWhiteSpace(lblTime.Text))
            ? lblTime.Text.Trim()
            : DateTime.Now.ToString("HH:mm:ss");

        using (Font timeFont = new Font("DS-Digital", 14.0F, FontStyle.Bold))
            TextRenderer.DrawText(g, statusTime, timeFont,
                new Rectangle(244, 9, 70, 37), Color.FromArgb(255, 218, 65),
                TextFormatFlags.Right | TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);

        DrawOldByteNovaCompactBrand(g, new Rectangle(0, 47, 326, 62));

        // Travel detail stays directly above the bottom panel.
        Rectangle detail = new Rectangle(5, 116, 320, 25);
        using (Brush white = new SolidBrush(Color.FromArgb(242, 246, 250)))
            g.FillRectangle(white, detail);
        using (Pen gold = new Pen(Color.FromArgb(255, 200, 45), 1.4F))
            g.DrawRectangle(gold, detail);

        // Reference-image style journey icons + subtle separators.
        DrawByteNovaTrainIconDark(g, new Rectangle(9, 120, 17, 17));
        DrawByteNovaTicketIcon(g, new Rectangle(107, 120, 17, 17));
        DrawByteNovaLocationIcon(g, new Rectangle(183, 120, 17, 17));
        DrawByteNovaCalendarIcon(g, new Rectangle(244, 120, 17, 17));
        using (Pen sepDetail = new Pen(Color.FromArgb(145, 155, 165), 1F))
        {
            g.DrawLine(sepDetail, 102, 120, 102, 138);
            g.DrawLine(sepDetail, 178, 120, 178, 138);
            g.DrawLine(sepDetail, 238, 120, 238, 138);
        }

        string journey = lblJourney != null ? lblJourney.Text : "";
        string train = cboTrain != null ? cboTrain.Text : "";
        string cls = cboClass != null ? cboClass.Text : "";
        string date = lblDate != null ? lblDate.Text : "";

        using (Font big = new Font("Arial Black", 7.5F, FontStyle.Bold))
        {
            TextRenderer.DrawText(g, journey, big, new Rectangle(29, 118, 70, 20), Color.FromArgb(5, 25, 55),
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            TextRenderer.DrawText(g, train, big, new Rectangle(127, 118, 49, 20), Color.FromArgb(5, 25, 55),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            TextRenderer.DrawText(g, cls, big, new Rectangle(202, 118, 34, 20), Color.FromArgb(5, 25, 55),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
            TextRenderer.DrawText(g, date, big, new Rectangle(264, 118, 56, 20), Color.FromArgb(5, 25, 55),
                TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        // Reference-image style bottom information panel with icons.
        Rectangle bottomBox = new Rectangle(5, 144, 320, 27);
        DrawByteNovaFinalBottomPanel(g, bottomBox);

        string avl = lblAvailability != null ? lblAvailability.Text : "";
        string tm = lblTime != null ? lblTime.Text : "";

        DrawByteNovaTrainIcon(g, new Rectangle(10, 149, 17, 17));
        using (Pen divider = new Pen(Color.FromArgb(198, 244, 50), 1F))
        {
            g.DrawLine(divider, 32, 149, 32, 166);
            g.DrawLine(divider, 231, 149, 231, 166);
        }

        using (Font avlFont = new Font("Arial", 8.2F, FontStyle.Bold))
            TextRenderer.DrawText(g, avl, avlFont,
                new Rectangle(38, 146, 132, 22), Color.White,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);

        // Show the selected bank value immediately before the bottom "|" divider.
        string bottomSelectedBank = selectredbank ?? "";
        if (string.IsNullOrWhiteSpace(bottomSelectedBank) && cboGateway != null)
        {
            string gatewayText = cboGateway.Text ?? "";
            int bankSep = gatewayText.IndexOf("_");
            bottomSelectedBank = (bankSep > 0 ? gatewayText.Substring(0, bankSep) : gatewayText).Trim();
            if (bottomSelectedBank.StartsWith("Select-", StringComparison.OrdinalIgnoreCase))
                bottomSelectedBank = "";
        }

        using (Font bankFont = new Font("Arial", 7.6F, FontStyle.Bold))
            TextRenderer.DrawText(g, bottomSelectedBank, bankFont,
                new Rectangle(172, 146, 55, 22), Color.FromArgb(255, 215, 0),
                TextFormatFlags.Right | TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);

        DrawByteNovaClockIcon(g, new Rectangle(236, 149, 17, 17));
        using (Font timeFont = new Font("Arial", 9.4F, FontStyle.Bold))
            TextRenderer.DrawText(g, tm, timeFont,
                new Rectangle(255, 146, 63, 22), Color.White,
                TextFormatFlags.Right | TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPadding);
    }

    private void DrawOldByteNovaCompactBrand(Graphics g, Rectangle area)
    {
        const string logoBase64 = "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";
        int logoWidth = 62;
        int logoHeight = 62; // only logo height reduced slightly
        int logoX = area.X;
        int logoY = area.Y + 1;

        try
        {
            Image fileLogo = LoadBrandLogo();
            if (fileLogo != null)
            {
                g.DrawImage(fileLogo, new Rectangle(logoX, logoY, logoWidth, logoHeight));
            }
            else
            {
                byte[] bytes = Convert.FromBase64String(logoBase64);
                using (MemoryStream ms = new MemoryStream(bytes))
                using (Image logo = Image.FromStream(ms))
                    g.DrawImage(logo, new Rectangle(logoX, logoY, logoWidth, logoHeight));
            }
        }
        catch { }

        int textX = logoX + logoWidth + 8;
        int textWidth = Math.Max(1, area.Right - textX);

        // BYTENOVA is intentionally rendered as ONE continuous word.
        // Drawing BYTE/NOVA separately can introduce a visual gap because of text measurement.
        // A single DrawString call guarantees there is no inserted space between BYTE and NOVA.
        // Reference branding: mixed-case ByteNova, heavy bold, full golden fill.
        using (Font brandFont = new Font("Arial Black", 32F, FontStyle.Bold))
        using (StringFormat brandFormat = new StringFormat(StringFormat.GenericTypographic))
        using (Brush brandBrush = new LinearGradientBrush(
            new Rectangle(textX, area.Y - 2, Math.Max(1, textWidth), 53),
            Color.FromArgb(255, 247, 150),
            Color.FromArgb(255, 174, 0),
            LinearGradientMode.Vertical))
        {
            brandFormat.FormatFlags = StringFormatFlags.NoWrap;
            brandFormat.Trimming = StringTrimming.None;

            g.DrawString(
                "Express",
                brandFont,
                brandBrush,
                new PointF(textX, area.Y - 12),
                brandFormat);
        }

        // Gold, widely-spaced subtitle like the supplied reference image.
        using (Font subFont = new Font("Arial Black", 7F, FontStyle.Bold))
            TextRenderer.DrawText(
                g, "P R E M I U M   T A T K A L   S O F T W A R E",
                subFont,
                new Rectangle(textX, area.Y + 51, textWidth, 11),
                Color.FromArgb(255, 215, 85),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPadding | TextFormatFlags.SingleLine);

    }

    private void DrawByteNovaFinalStatusCard(Graphics g, Rectangle r)
    {
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(5, 45, 92), Color.FromArgb(1, 18, 48),
            LinearGradientMode.Vertical))
            g.FillRectangle(bg, r);

        using (Pen outer = new Pen(Color.FromArgb(0, 225, 255), 1.8F))
            g.DrawRectangle(outer, r);

        Rectangle inner = new Rectangle(r.X + 2, r.Y + 2,
            Math.Max(1, r.Width - 4), Math.Max(1, r.Height - 4));
        using (Pen glow = new Pen(Color.FromArgb(90, 0, 150, 255), 1F))
            g.DrawRectangle(glow, inner);

        Rectangle shineRect = new Rectangle(r.X + 2, r.Y + 2,
            Math.Max(1, r.Width - 4), Math.Max(1, r.Height / 2 - 1));
        using (LinearGradientBrush shine = new LinearGradientBrush(
            shineRect, Color.FromArgb(42, 45, 135, 225), Color.FromArgb(0, 10, 40, 80),
            LinearGradientMode.Vertical))
            g.FillRectangle(shine, shineRect);
    }

    private void DrawByteNovaChromeStatusIcon(Graphics g, Rectangle r)
    {
        SmoothingMode old = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle pieRect = new Rectangle(r.X, r.Y, r.Width, r.Height);
        using (Brush red = new SolidBrush(Color.FromArgb(255, 210, 45))) g.FillPie(red, pieRect, 210F, 120F);
        using (Brush yellow = new SolidBrush(Color.FromArgb(251, 188, 5))) g.FillPie(yellow, pieRect, 330F, 120F);
        using (Brush green = new SolidBrush(Color.FromArgb(52, 168, 83))) g.FillPie(green, pieRect, 90F, 120F);
        Rectangle core = new Rectangle(
            r.X + (int)(r.Width * 0.28F),
            r.Y + (int)(r.Height * 0.28F),
            Math.Max(1, (int)(r.Width * 0.44F)),
            Math.Max(1, (int)(r.Height * 0.44F)));
        using (Brush blue = new SolidBrush(Color.FromArgb(66, 133, 244))) g.FillEllipse(blue, core);
        using (Pen white = new Pen(Color.White, 1F)) g.DrawEllipse(white, core);
        g.SmoothingMode = old;
    }

    private void DrawByteNovaSuccessBadge(Graphics g, Rectangle r)
    {
        SmoothingMode old = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle glow = new Rectangle(r.X - 2, r.Y - 2, r.Width + 4, r.Height + 4);
        using (Brush gb = new SolidBrush(Color.FromArgb(55, 35, 255, 95))) g.FillEllipse(gb, glow);
        using (LinearGradientBrush bg = new LinearGradientBrush(r, Color.FromArgb(0, 115, 35), Color.FromArgb(0, 50, 20), LinearGradientMode.Vertical)) g.FillEllipse(bg, r);
        using (Pen ring = new Pen(Color.FromArgb(70, 255, 105), 2F)) g.DrawEllipse(ring, r);
        using (Pen tick = new Pen(Color.FromArgb(120, 255, 145), 2.5F))
        {
            tick.StartCap = LineCap.Round; tick.EndCap = LineCap.Round;
            g.DrawLines(tick, new[] { new Point(r.X + 6, r.Y + 10), new Point(r.X + 10, r.Y + 14), new Point(r.X + 18, r.Y + 6) });
        }
        g.SmoothingMode = old;
    }

    private void DrawByteNovaTrainIconDark(Graphics g, Rectangle r)
    {
        SmoothingMode old = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        Color c = Color.FromArgb(5, 30, 65);
        using (Pen p = new Pen(c, 1.7F))
        using (Brush b = new SolidBrush(c))
        {
            Rectangle body = new Rectangle(r.X + 3, r.Y + 1, r.Width - 6, r.Height - 5);
            g.DrawRectangle(p, body);
            g.DrawLine(p, r.X + 5, r.Y + 5, r.Right - 5, r.Y + 5);
            g.FillEllipse(b, r.X + 5, r.Bottom - 5, 3, 3);
            g.FillEllipse(b, r.Right - 8, r.Bottom - 5, 3, 3);
        }
        g.SmoothingMode = old;
    }

    private void DrawByteNovaTicketIcon(Graphics g, Rectangle r)
    {
        SmoothingMode old = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using (Pen p = new Pen(Color.FromArgb(5, 30, 65), 2F))
        {
            g.DrawLine(p, r.X + 3, r.Y + 12, r.X + 12, r.Y + 3);
            g.DrawLine(p, r.X + 6, r.Y + 15, r.X + 15, r.Y + 6);
            g.DrawLine(p, r.X + 3, r.Y + 12, r.X + 6, r.Y + 15);
            g.DrawLine(p, r.X + 12, r.Y + 3, r.X + 15, r.Y + 6);
            g.DrawLine(p, r.X + 7, r.Y + 8, r.X + 10, r.Y + 11);
        }
        g.SmoothingMode = old;
    }

    private void DrawByteNovaLocationIcon(Graphics g, Rectangle r)
    {
        SmoothingMode old = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        Color c = Color.FromArgb(5, 30, 65);
        using (Pen p = new Pen(c, 1.8F))
        using (Brush b = new SolidBrush(c))
        {
            g.DrawEllipse(p, r.X + 3, r.Y + 1, 11, 11);
            g.FillEllipse(b, r.X + 7, r.Y + 5, 3, 3);
            g.DrawLine(p, r.X + 5, r.Y + 10, r.X + 8, r.Bottom - 1);
            g.DrawLine(p, r.X + 12, r.Y + 10, r.X + 8, r.Bottom - 1);
        }
        g.SmoothingMode = old;
    }

    private void DrawByteNovaCalendarIcon(Graphics g, Rectangle r)
    {
        Color c = Color.FromArgb(5, 30, 65);
        using (Pen p = new Pen(c, 1.6F))
        using (Brush b = new SolidBrush(c))
        {
            g.DrawRectangle(p, r.X + 2, r.Y + 3, r.Width - 4, r.Height - 5);
            g.DrawLine(p, r.X + 2, r.Y + 7, r.Right - 2, r.Y + 7);
            g.FillRectangle(b, r.X + 5, r.Y + 1, 2, 5);
            g.FillRectangle(b, r.Right - 7, r.Y + 1, 2, 5);
            g.FillRectangle(b, r.X + 5, r.Y + 10, 2, 2);
            g.FillRectangle(b, r.X + 9, r.Y + 10, 2, 2);
        }
    }

    private void DrawByteNovaFinalBottomPanel(Graphics g, Rectangle r)
    {
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(6, 55, 100), Color.FromArgb(1, 18, 45),
            LinearGradientMode.Vertical))
            g.FillRectangle(bg, r);

        using (Pen glow = new Pen(Color.FromArgb(0, 225, 255), 1.8F))
            g.DrawRectangle(glow, r);

        Rectangle inner = new Rectangle(r.X + 2, r.Y + 2,
            Math.Max(1, r.Width - 4), Math.Max(1, r.Height - 4));
        using (Pen innerPen = new Pen(Color.FromArgb(85, 80, 175, 255), 1F))
            g.DrawRectangle(innerPen, inner);
    }

    private void DrawByteNovaTrainIcon(Graphics g, Rectangle r)
    {
        SmoothingMode old = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (Pen p = new Pen(Color.White, 1.7F))
        using (Brush b = new SolidBrush(Color.White))
        {
            Rectangle body = new Rectangle(r.X + 3, r.Y + 1, r.Width - 6, r.Height - 5);
            g.DrawRectangle(p, body);
            g.DrawLine(p, r.X + 5, r.Y + 5, r.Right - 5, r.Y + 5);
            g.FillEllipse(b, r.X + 5, r.Bottom - 5, 3, 3);
            g.FillEllipse(b, r.Right - 8, r.Bottom - 5, 3, 3);
            g.DrawLine(p, r.X + 5, r.Bottom - 1, r.X + 8, r.Bottom - 5);
            g.DrawLine(p, r.Right - 5, r.Bottom - 1, r.Right - 8, r.Bottom - 5);
        }

        g.SmoothingMode = old;
    }

    private void DrawByteNovaClockIcon(Graphics g, Rectangle r)
    {
        SmoothingMode old = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        using (Pen p = new Pen(Color.White, 1.8F))
        {
            g.DrawEllipse(p, r);
            Point c = new Point(r.X + r.Width / 2, r.Y + r.Height / 2);
            g.DrawLine(p, c.X, c.Y, c.X, r.Y + 4);
            g.DrawLine(p, c.X, c.Y, r.Right - 4, c.Y + 2);
        }

        g.SmoothingMode = old;
    }

    private void DrawReferenceCard(Graphics g, Rectangle r, Color edge)
    {
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(7, 40, 68), Color.FromArgb(2, 17, 36),
            LinearGradientMode.Vertical))
            g.FillRectangle(bg, r);

        using (Pen p = new Pen(Color.FromArgb(26, 105, 160), 1F))
            g.DrawRectangle(p, r);

        using (Pen accent = new Pen(Color.FromArgb(198, 244, 50), 2F))
            g.DrawLine(accent, r.X, r.Y + 2, r.X, r.Bottom - 2);
    }

    private void DrawCaption(Graphics g, string text, float x, float y)
    {
        // Journey / Train No. / IRCTC ID / Slot captions are custom-painted here.
        // Use ByteNova bright gold so the captions stay clearly visible.
        using (Font f = new Font("Segoe UI Semibold", 6.2F, FontStyle.Bold))
        using (Brush b = new SolidBrush(Color.FromArgb(255, 211, 71)))
            g.DrawString(text, f, b, x, y);
    }

    private void DrawMiniIcon(Graphics g, int x, int y, string glyph)
    {
        Rectangle r = new Rectangle(x, y, 14, 14);
        using (Brush bg = new SolidBrush(Color.FromArgb(12, 52, 92)))
            g.FillEllipse(bg, r);
        using (Pen p = new Pen(Color.FromArgb(185, 225, 255), 1.1F))
        {
            if (glyph == "⌖") // Journey pin
            {
                g.DrawEllipse(p, x + 4, y + 3, 6, 6);
                g.DrawLine(p, x + 7, y + 9, x + 7, y + 12);
            }
            else if (glyph == "#") // Train
            {
                g.DrawRectangle(p, x + 4, y + 3, 7, 7);
                g.DrawLine(p, x + 5, y + 11, x + 3, y + 13);
                g.DrawLine(p, x + 10, y + 11, x + 12, y + 13);
            }
            else if (glyph == "I") // User / IRCTC ID
            {
                g.DrawEllipse(p, x + 5, y + 3, 4, 4);
                g.DrawArc(p, x + 3, y + 7, 8, 6, 190, 160);
            }
            else // Slot / clock
            {
                g.DrawEllipse(p, x + 3, y + 3, 8, 8);
                g.DrawLine(p, x + 7, y + 4, x + 7, y + 7);
                g.DrawLine(p, x + 7, y + 7, x + 10, y + 8);
            }
        }
    }

    private void ApplyReadableFontRecursive(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            if (c is Button)
                c.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            else if (c is ComboBox)
                c.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            else if (c is Label && c != lblStatus)
                c.Font = new Font("Segoe UI", 8.5F, c.Font.Bold ? FontStyle.Bold : FontStyle.Regular);

            if (c.HasChildren)
                ApplyReadableFontRecursive(c);
        }

        if (lblStatus != null)
            lblStatus.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
    }

    // ByteNova: keep ComboBox text clearly visible after login/disable.
    // Disabled combos are painted with yellow text; enabled combos keep their normal text color.
    private void ApplyByteNovaDisabledComboText(ComboBox combo)
    {
        if (combo == null) return;

        combo.DrawMode = DrawMode.OwnerDrawFixed;
        combo.DrawItem -= ByteNovaCombo_DrawItem;
        combo.DrawItem += ByteNovaCombo_DrawItem;
        combo.EnabledChanged -= ByteNovaCombo_EnabledChanged;
        combo.EnabledChanged += ByteNovaCombo_EnabledChanged;
        combo.SelectedIndexChanged -= ByteNovaComboVisualChanged;
        combo.SelectedIndexChanged += ByteNovaComboVisualChanged;
        combo.TextChanged -= ByteNovaComboVisualChanged;
        combo.TextChanged += ByteNovaComboVisualChanged;
        combo.LocationChanged -= ByteNovaComboVisualChanged;
        combo.LocationChanged += ByteNovaComboVisualChanged;
        combo.SizeChanged -= ByteNovaComboVisualChanged;
        combo.SizeChanged += ByteNovaComboVisualChanged;
        combo.VisibleChanged -= ByteNovaComboVisualChanged;
        combo.VisibleChanged += ByteNovaComboVisualChanged;

        combo.BackColor = Color.FromArgb(3, 29, 59);
        combo.ForeColor = Color.White;
        combo.FlatStyle = FlatStyle.Flat;

        UpdateByteNovaDisabledComboCover(combo);
    }

    private void ByteNovaCombo_EnabledChanged(object sender, EventArgs e)
    {
        ComboBox combo = sender as ComboBox;
        if (combo == null) return;
        combo.Invalidate();
        UpdateByteNovaDisabledComboCover(combo);
    }

    private void ByteNovaComboVisualChanged(object sender, EventArgs e)
    {
        ComboBox combo = sender as ComboBox;
        if (combo != null)
            UpdateByteNovaDisabledComboCover(combo);
    }

    private void UpdateByteNovaDisabledComboCover(ComboBox combo)
    {
        if (combo == null || combo.Parent == null) return;

        Label cover;
        if (!_byteNovaDisabledComboCovers.TryGetValue(combo, out cover) || cover == null || cover.IsDisposed)
        {
            cover = new Label();
            cover.AutoSize = false;
            cover.BackColor = Color.FromArgb(3, 29, 59);
            cover.ForeColor = Color.FromArgb(255, 215, 0);
            cover.Font = combo.Font;
            cover.TextAlign = ContentAlignment.MiddleLeft;
            cover.Padding = new Padding(4, 0, 18, 0);
            cover.Paint += ByteNovaDisabledComboCoverPaint;
            cover.Tag = combo;
            _byteNovaDisabledComboCovers[combo] = cover;
        }

        if (cover.Parent != combo.Parent)
        {
            if (cover.Parent != null) cover.Parent.Controls.Remove(cover);
            combo.Parent.Controls.Add(cover);
        }

        cover.Bounds = combo.Bounds;
        cover.Font = combo.Font;
        cover.Text = combo.Text;
        cover.Visible = combo.Visible && !combo.Enabled;

        if (cover.Visible)
        {
            cover.BringToFront();
            cover.Invalidate();
        }
        else
        {
            combo.BringToFront();
        }
    }

    private void ByteNovaDisabledComboCoverPaint(object sender, PaintEventArgs e)
    {
        Label cover = sender as Label;
        if (cover == null || cover.Width < 2 || cover.Height < 2) return;

        Rectangle r = new Rectangle(0, 0, cover.Width - 1, cover.Height - 1);

        // Completely cover the native disabled white ComboBox surface.
        using (Brush bg = new SolidBrush(Color.FromArgb(3, 29, 59)))
            e.Graphics.FillRectangle(bg, r);

        using (Pen border = new Pen(Color.FromArgb(3, 29, 59), 2F))
            e.Graphics.DrawRectangle(border, r);

        // Small ByteNova dropdown arrow; no white system arrow/background.
        Point[] arrow =
        {
            new Point(Math.Max(2, cover.Width - 13), Math.Max(3, cover.Height / 2 - 2)),
            new Point(Math.Max(2, cover.Width - 5),  Math.Max(3, cover.Height / 2 - 2)),
            new Point(Math.Max(2, cover.Width - 9),  Math.Min(cover.Height - 3, cover.Height / 2 + 3))
        };
        using (Brush arrowBrush = new SolidBrush(Color.FromArgb(255, 215, 0)))
            e.Graphics.FillPolygon(arrowBrush, arrow);

        string text = cover.Text ?? "";
        Rectangle textRect = new Rectangle(4, 0, Math.Max(1, cover.Width - 20), cover.Height);
        TextRenderer.DrawText(
            e.Graphics, text, cover.Font, textRect,
            Color.FromArgb(255, 215, 0),
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }

    private void ByteNovaCombo_DrawItem(object sender, DrawItemEventArgs e)
    {
        ComboBox combo = sender as ComboBox;
        if (combo == null || e.Index < 0) return;

        // Always paint the item/display area Navy Blue so Windows never leaves it white.
        using (Brush bg = new SolidBrush(Color.FromArgb(3, 29, 59)))
            e.Graphics.FillRectangle(bg, e.Bounds);

        string itemText = combo.GetItemText(combo.Items[e.Index]);
        Color textColor = combo.Enabled
            ? Color.White
            : Color.FromArgb(255, 215, 0);

        using (Brush b = new SolidBrush(textColor))
        {
            Rectangle r = e.Bounds;
            r.X += 3;
            e.Graphics.DrawString(itemText, combo.Font, b, r);
        }
    }

    private void ApplyByteNovaDisabledComboTextToAll()
    {
        ApplyByteNovaDisabledComboText(cboID);
        ApplyByteNovaDisabledComboText(cboSlot);
        ApplyByteNovaDisabledComboText(cboGateway);
        ApplyByteNovaDisabledComboText(cboQuota);
        ApplyByteNovaDisabledComboText(cboTrain);
        ApplyByteNovaDisabledComboText(cboClass);
    }

    private void ApplyByteNovaGoldenDetailLabels()
    {
        Color gold = Color.FromArgb(255, 211, 71);

        // Force the four caption labels highlighted in the supplied screenshot.
        if (label11 != null) label11.ForeColor = gold;
        if (label13 != null) label13.ForeColor = gold;

        // Requested captions only: Journey, Train No, IRCTC ID and Slot.
        foreach (Control c in Controls)
            ApplyByteNovaGoldenDetailLabelsRecursive(c, gold);
    }

    private void ApplyByteNovaGoldenDetailLabelsRecursive(Control parent, Color gold)
    {
        foreach (Control c in parent.Controls)
        {
            Label lbl = c as Label;
            if (lbl != null)
            {
                string t = (lbl.Text ?? "").Trim().ToUpperInvariant();
                bool requestedCaption =
                    t == "JOURNEY" || t == "TRAIN NO" || t == "TRAIN NO." ||
                    t == "IRCTC ID" || t == "SLOT" ||
                    lbl.Name.Equals("label11", StringComparison.OrdinalIgnoreCase) ||
                    lbl.Name.Equals("label13", StringComparison.OrdinalIgnoreCase);

                if (requestedCaption)
                    lbl.ForeColor = gold;
            }

            if (c.HasChildren)
                ApplyByteNovaGoldenDetailLabelsRecursive(c, gold);
        }
    }

    private void ApplyByteNovaModernStyle()
    {
        SuspendLayout();
        DoubleBuffered = true;
        BackColor = Color.FromArgb(2, 12, 23);
        ForeColor = Color.White;

        StyleModernPanel(TopPanel, true);
        StyleModernPanel(HeaderPanel1, true);
        StyleModernPanel(BottomPanel, false);
        StyleModernPanel(Panel1, false);
        StyleModernPanel(PanelPnr, false);
        StyleModernPanel(Panel5, false);
        StyleModernPanel(panel2, false);
        StyleModernPanel(pnlBrand, false);
        StyleModernPanel(B3AAA, false);

        ComboBox[] combos = { cboID, cboSlot, cboGateway, cboQuota, cboTrain, cboClass, fake_bank, fake_id };
        foreach (ComboBox c in combos)
        {
            if (c == null) continue;
            c.BackColor = Color.FromArgb(7, 27, 45);
            c.ForeColor = Color.White;
            c.FlatStyle = FlatStyle.Flat;
            c.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Regular);
        }

        // Existing buttons only - use restrained different accent colors for quick recognition.
        StyleModernButton(btnEdit, Color.FromArgb(0, 145, 205));   // blue
        StyleModernButton(BtnOpenPair, Color.FromArgb(0, 170, 135));   // teal
        StyleModernButton(btnCancel, Color.FromArgb(185, 38, 58));   // red
        StyleModernButton(AppLogin_, Color.FromArgb(0, 145, 105));   // green
        StyleModernButton(WebLogin, Color.FromArgb(20, 125, 235));  // Chrome blue
        StyleModernButton(button1, Color.FromArgb(0, 175, 145));   // Pair teal
        StyleModernButton(btnDish, Color.FromArgb(120, 55, 210));  // Edge violet
        StyleModernButton(btnWeb3, Color.FromArgb(225, 125, 15));  // Brave amber
        StyleModernButton(button2, Color.FromArgb(115, 65, 175));  // purple
        StyleModernButton(btnSlotNo, Color.FromArgb(25, 115, 165));  // steel blue
        StyleModernButton(btnMinize, Color.FromArgb(70, 85, 110));   // slate

        // Browser names requested by user. Existing buttons and click handlers remain unchanged.
        if (WebLogin != null) WebLogin.Text = "Chrome";
        if (btnDish != null) btnDish.Text = "Edge";
        if (btnWeb3 != null) btnWeb3.Text = "Brave";

        ApplyExistingButtonIcon(WebLogin, "C");
        ApplyExistingButtonIcon(btnDish, "E");
        ApplyExistingButtonIcon(btnWeb3, "B");
        ApplyExistingButtonIcon(AppLogin_, "A");
        ApplyExistingButtonIcon(BtnOpenPair, "↗");
        ApplyExistingButtonIcon(button1, "↗");
        ApplyExistingButtonIcon(btnEdit, "✎");
        ApplyExistingButtonIcon(btnCancel, "×");
        ApplyExistingButtonIcon(btnMinize, "−");

        Label[] labels = { lblJourney, lblStatus, lblClass, lblTktName, lbl_Month, lbl_Date,
                           label5, lbl_Detail, lbl_status, lbl_slot, lblAvailability, lblTime,
                           lblPnr, label13, label11, lblDate, lblInfo, lblTrainNo, lbl_brand,
                           lbl_time, lbl_bnk, lblBrand, lblBank, lblLoginType };
        foreach (Label l in labels)
        {
            if (l == null) continue;
            l.ForeColor = Color.FromArgb(230, 245, 255);
            l.Font = new Font("Segoe UI", Math.Max(7.5F, l.Font.Size), l.Font.Style);
        }

        if (lblTktName != null)
        {
            lblTktName.ForeColor = Color.White;
            lblTktName.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
        }
        if (lblStatus != null)
        {
            lblStatus.BackColor = Color.FromArgb(3, 29, 59);
            lblStatus.ForeColor = Color.White;
        }
        if (lblAvailability != null)
        {
            lblAvailability.ForeColor = Color.FromArgb(75, 235, 175);
            lblAvailability.Font = new Font("Segoe UI Semibold", Math.Max(8F, lblAvailability.Font.Size), FontStyle.Bold);
        }

        CheckBox[] checks = { chkConfirmation, check_avl };
        foreach (CheckBox c in checks)
        {
            if (c == null) continue;
            c.ForeColor = Color.FromArgb(225, 242, 250);
            c.BackColor = Color.Transparent;
            c.FlatStyle = FlatStyle.Flat;
        }

        // Keep chkConfirmation as a normal Windows checkbox so its tick is visible/clickable.
        if (chkConfirmation != null)
        {
            chkConfirmation.FlatStyle = FlatStyle.Standard;
            chkConfirmation.UseVisualStyleBackColor = true;
            chkConfirmation.AutoCheck = true;
            chkConfirmation.Enabled = true;
        }

        StyleModernLink(WLlinkLabel);
        StyleModernLink(EditlinkLabel);

        Paint -= ByteNovaModernControlPaint;
        Paint += ByteNovaModernControlPaint;

        // Rounded neon-glass silhouette like the approved image, using existing controls only.
        ApplyRoundedRegion(TopPanel, 10);
        ApplyRoundedRegion(HeaderPanel1, 10);
        ApplyRoundedRegion(BottomPanel, 10);
        ApplyRoundedRegion(Panel1, 8);
        ApplyRoundedRegion(PanelPnr, 8);
        ApplyRoundedRegion(Panel5, 8);
        ApplyRoundedRegion(panel2, 8);
        ApplyRoundedRegion(pnlBrand, 8);
        ApplyRoundedRegion(B3AAA, 8);

        Button[] roundedButtons = { btnEdit, BtnOpenPair, btnCancel, AppLogin_, WebLogin, button1,
                                    btnDish, btnWeb3, button2, btnSlotNo, btnMinize };
        foreach (Button b in roundedButtons) ApplyRoundedRegion(b, 7);

        // Restructure only the existing controls: cleaner spacing and hierarchy.
        RestructureExistingLayout();

        ResumeLayout(false);
        Invalidate(true);
    }

    private void RestructureExistingLayout()
    {
        // Redesign using ONLY existing controls. No option is removed or added.
        Panel[] sections = { TopPanel, HeaderPanel1, BottomPanel, Panel1, PanelPnr, Panel5, panel2, pnlBrand, B3AAA };
        foreach (Panel section in sections)
        {
            if (section == null) continue;
            section.Padding = new Padding(4);
            section.Margin = new Padding(2);
        }

        // Main ticket title becomes the strongest visual element.
        if (lblTktName != null)
        {
            lblTktName.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold);
            lblTktName.ForeColor = Color.White;
        }

        if (lblJourney != null)
        {
            lblJourney.Font = new Font("Segoe UI Semibold", 8.5F, FontStyle.Bold);
            lblJourney.ForeColor = Color.FromArgb(115, 225, 255);
        }

        // Status and availability work as compact information strips.
        if (lblStatus != null)
        {
            lblStatus.BackColor = Color.FromArgb(3, 29, 59);
            lblStatus.ForeColor = Color.White;
            lblStatus.Padding = new Padding(4, 0, 4, 0);
        }

        if (lblAvailability != null)
        {
            lblAvailability.BackColor = Color.FromArgb(3, 34, 38);
            lblAvailability.ForeColor = Color.FromArgb(100, 245, 175);
            lblAvailability.Padding = new Padding(3, 0, 3, 0);
        }

        // Consistent compact action cards.
        Button[] actions = { btnEdit, BtnOpenPair, btnCancel, AppLogin_, WebLogin, button1,
                             btnDish, btnWeb3, button2, btnSlotNo, btnMinize };
        foreach (Button b in actions)
        {
            if (b == null) continue;
            b.Margin = new Padding(2);
            b.TextAlign = ContentAlignment.MiddleCenter;
            b.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
        }

        // Requested browser names; original click handlers stay attached.
        if (WebLogin != null) WebLogin.Text = "Chrome";
        if (btnDish != null) btnDish.Text = "Edge";
        if (btnWeb3 != null) btnWeb3.Text = "Brave";

        // Approved mockup hierarchy: Journey first, IRCTC ID second.
        // Existing combo boxes remain untouched.
        if (label5 != null) label5.Text = "Journey";
        if (lblBrand != null) lblBrand.Text = "IRCTC ID";

        // Add lightweight drawn icons to EXISTING buttons only.
        ApplyExistingButtonIcon(WebLogin, "C");      // Chrome
        ApplyExistingButtonIcon(btnDish, "E");       // Edge
        ApplyExistingButtonIcon(btnWeb3, "B");       // Brave
        ApplyExistingButtonIcon(AppLogin_, "A");     // App
        ApplyExistingButtonIcon(BtnOpenPair, "↗");   // Pair/link
        ApplyExistingButtonIcon(button1, "↗");
        ApplyExistingButtonIcon(btnEdit, "✎");
        ApplyExistingButtonIcon(btnCancel, "×");
        ApplyExistingButtonIcon(btnMinize, "−");
        ApplyExistingButtonIcon(btnSlotNo, "S");
        ApplyExistingButtonIcon(button2, "●");
    }

    private void ApplyExistingButtonIcon(Button button, string glyph)
    {
        if (button == null) return;

        Bitmap bmp = new Bitmap(18, 18);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.Clear(Color.Transparent);

            using (Pen ring = new Pen(Color.FromArgb(235, 255, 255, 255), 1.4F))
                g.DrawEllipse(ring, 1.5F, 1.5F, 14.5F, 14.5F);

            using (Font f = new Font("Segoe UI Symbol", 8F, FontStyle.Bold, GraphicsUnit.Point))
            using (Brush b = new SolidBrush(Color.White))
            {
                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.LineAlignment = StringAlignment.Center;
                g.DrawString(glyph, f, b, new RectangleF(1, 1, 16, 16), sf);
            }
        }

        button.Image = bmp;
        button.ImageAlign = ContentAlignment.MiddleLeft;
        button.TextImageRelation = TextImageRelation.ImageBeforeText;
        button.Padding = new Padding(4, 0, 4, 0);
    }

    private void ApplyRoundedRegion(Control control, int radius)
    {
        if (control == null || control.Width < 4 || control.Height < 4) return;

        int d = Math.Max(2, radius * 2);
        Rectangle bounds = new Rectangle(0, 0, control.Width, control.Height);
        using (GraphicsPath path = new GraphicsPath())
        {
            path.AddArc(bounds.Left, bounds.Top, d, d, 180, 90);
            path.AddArc(bounds.Right - d - 1, bounds.Top, d, d, 270, 90);
            path.AddArc(bounds.Right - d - 1, bounds.Bottom - d - 1, d, d, 0, 90);
            path.AddArc(bounds.Left, bounds.Bottom - d - 1, d, d, 90, 90);
            path.CloseFigure();
            control.Region = new Region(path);
        }
    }

    private void StyleModernPanel(Panel panel, bool strong)
    {
        if (panel == null) return;
        panel.BackColor = strong ? Color.FromArgb(3, 27, 47) : Color.FromArgb(3, 19, 33);
        panel.Paint -= ByteNovaModernPanelPaint;
        panel.Paint += ByteNovaModernPanelPaint;
    }

    private void StyleModernButton(Button button, Color accent)
    {
        if (button == null) return;

        // ONE solid color per button. No two-tone / gradient-like appearance.
        button.UseVisualStyleBackColor = false;
        button.FlatStyle = FlatStyle.Flat;
        button.BackColor = accent;
        button.ForeColor = Color.White;

        // Border stays in the same color family instead of a second bright color.
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.BorderColor = ControlPaint.Dark(accent, 0.08F);

        // Hover/click only change brightness temporarily.
        button.FlatAppearance.MouseOverBackColor = ControlPaint.Light(accent, 0.10F);
        button.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(accent, 0.12F);

        button.Font = new Font("Segoe UI Semibold", 8F, FontStyle.Bold);
        button.Cursor = Cursors.Hand;
        button.TextAlign = ContentAlignment.MiddleCenter;
    }

    private void StyleModernLink(LinkLabel link)
    {
        if (link == null) return;
        link.BackColor = Color.Transparent;
        link.LinkColor = Color.FromArgb(50, 220, 255);
        link.ActiveLinkColor = Color.White;
        link.VisitedLinkColor = Color.FromArgb(50, 220, 255);
    }

    private void ByteNovaModernControlPaint(object sender, PaintEventArgs e)
    {
        if (Width < 2 || Height < 2) return;
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, Width - 1, Height - 1);

        using (LinearGradientBrush bg = new LinearGradientBrush(
            r, Color.FromArgb(4, 28, 47), Color.FromArgb(1, 9, 18), LinearGradientMode.Vertical))
            e.Graphics.FillRectangle(bg, r);

        using (Pen glow = new Pen(Color.FromArgb(198, 244, 50), 1F))
            e.Graphics.DrawRectangle(glow, r);
    }

    private void ByteNovaModernPanelPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null || c.Width < 4 || c.Height < 4) return;

        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(1, 1, c.Width - 3, c.Height - 3);

        // New flatter glass-card design: navy -> indigo depth.
        using (LinearGradientBrush bg = new LinearGradientBrush(
            r,
            Color.FromArgb(7, 39, 61),
            Color.FromArgb(12, 17, 39),
            LinearGradientMode.Horizontal))
        {
            ColorBlend blend = new ColorBlend();
            blend.Positions = new float[] { 0F, .55F, 1F };
            blend.Colors = new Color[]
            {
                Color.FromArgb(6, 40, 62),
                Color.FromArgb(3, 23, 40),
                Color.FromArgb(18, 17, 48)
            };
            bg.InterpolationColors = blend;
            e.Graphics.FillRectangle(bg, r);
        }

        // Clean cyan outline with a restrained violet inner edge.
        using (Pen outer = new Pen(Color.FromArgb(0, 190, 230), 1F))
            e.Graphics.DrawRectangle(outer, r);

        Rectangle inner = new Rectangle(3, 3, Math.Max(1, c.Width - 7), Math.Max(1, c.Height - 7));
        using (Pen p = new Pen(Color.FromArgb(55, 125, 95, 210), 1F))
            e.Graphics.DrawRectangle(p, inner);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && container_0 != null)
        {
            container_0.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.cboGateway = new System.Windows.Forms.ComboBox();
        this.cboID = new System.Windows.Forms.ComboBox();
        this.btnEdit = new System.Windows.Forms.Button();
        this.chkConfirmation = new System.Windows.Forms.CheckBox();
        this.cboSlot = new System.Windows.Forms.ComboBox();
        this.BtnOpenPair = new System.Windows.Forms.Button();
        this.lblJourney = new System.Windows.Forms.Label();
        this.cboQuota = new System.Windows.Forms.ComboBox();
        this.lblStatus = new System.Windows.Forms.Label();
        this.TopPanel = new System.Windows.Forms.Panel();
        this.btnCancel = new System.Windows.Forms.Button();
        this.lblTktName = new System.Windows.Forms.Label();
        this.timer_0 = new System.Windows.Forms.Timer(this.components);
        this.toolTip_0 = new System.Windows.Forms.ToolTip(this.components);
        this.panel2 = new System.Windows.Forms.Panel();
        this.lblInfo = new System.Windows.Forms.Label();
        this.check_avl = new System.Windows.Forms.CheckBox();
        this.HeaderPanel1 = new System.Windows.Forms.Panel();
        this.Panel5 = new System.Windows.Forms.Panel();
        this.btnSlotNo = new System.Windows.Forms.Button();
        this.btnMinize = new System.Windows.Forms.Button();
        this.WLlinkLabel = new System.Windows.Forms.LinkLabel();
        this.EditlinkLabel = new System.Windows.Forms.LinkLabel();
        this.lblClass = new System.Windows.Forms.Label();
        this.cboTrain = new System.Windows.Forms.ComboBox();
        this.Panel1 = new System.Windows.Forms.Panel();
        this.cboClass = new System.Windows.Forms.ComboBox();
        this.label13 = new System.Windows.Forms.Label();
        this.label11 = new System.Windows.Forms.Label();
        this.lblDate = new System.Windows.Forms.Label();
        this.PanelPnr = new System.Windows.Forms.Panel();
        this.label5 = new System.Windows.Forms.Label();
        this.lbl_Detail = new System.Windows.Forms.Label();
        this.lbl_Date = new System.Windows.Forms.Label();
        this.lbl_Month = new System.Windows.Forms.Label();
        this.lbl_status = new System.Windows.Forms.Label();
        this.lbl_brand = new System.Windows.Forms.Label();
        this.lbl_time = new System.Windows.Forms.Label();
        this.B3AAA = new System.Windows.Forms.Panel();
        this.lbl_slot = new System.Windows.Forms.Label();
        this.lblPnr = new System.Windows.Forms.Label();
        this.AppLogin_ = new System.Windows.Forms.Button();
        this.lblTrainNo = new System.Windows.Forms.Label();
        this.timer_1 = new System.Windows.Forms.Timer(this.components);
        this.WebLogin = new System.Windows.Forms.Button();
        this.button1 = new System.Windows.Forms.Button();
        this.lbl_bnk = new System.Windows.Forms.Label();
        this.lblBrand = new System.Windows.Forms.Label();
        this.btnDish = new System.Windows.Forms.Button();
        this.btnWeb3 = new System.Windows.Forms.Button();
        this.fake_bank = new System.Windows.Forms.ComboBox();
        this.fake_id = new System.Windows.Forms.ComboBox();
        this.pnlBrand = new System.Windows.Forms.Panel();
        this.lblLoginType = new System.Windows.Forms.Label();
        this.lblBank = new System.Windows.Forms.Label();
        this.BottomPanel = new System.Windows.Forms.Panel();
        this.lblTime = new System.Windows.Forms.Label();
        this.lblAvailability = new System.Windows.Forms.Label();
        this.pictureBox2 = new System.Windows.Forms.PictureBox();
        this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
        this.button2 = new System.Windows.Forms.Button();
        this.TopPanel.SuspendLayout();
        this.HeaderPanel1.SuspendLayout();
        this.Panel1.SuspendLayout();
        this.PanelPnr.SuspendLayout();
        this.pnlBrand.SuspendLayout();
        this.BottomPanel.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
        this.SuspendLayout();
        // 
        // cboGateway
        // 
        this.cboGateway.BackColor = System.Drawing.Color.Gainsboro;
        this.cboGateway.DropDownHeight = 250;
        this.cboGateway.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cboGateway.DropDownWidth = 200;
        this.cboGateway.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.cboGateway.ForeColor = System.Drawing.Color.Black;
        this.cboGateway.FormattingEnabled = true;
        this.cboGateway.IntegralHeight = false;
        this.cboGateway.Items.AddRange(new object[] {
            "-Select-",
            "NB BANK NAME - USERNAME",
            "DC BANK NAME - HOLDERNAME (LAST 4 DIGIT OF CARD)",
            "CC BANK NAME - HOLDERNAME (LAST 4 DIGIT OF CARD)",
            "VC BANK NAME - HOLDERNAME (LAST 4 DIGIT OF CARD)"});
        this.cboGateway.Location = new System.Drawing.Point(56, 63);
        this.cboGateway.Margin = new System.Windows.Forms.Padding(2);
        this.cboGateway.Name = "cboGateway";
        this.cboGateway.Size = new System.Drawing.Size(162, 22);
        this.cboGateway.TabIndex = 12;
        this.cboGateway.SelectedIndexChanged += new System.EventHandler(this.cboGateway_SelectedIndexChanged);
        this.cboGateway.MouseLeave += new System.EventHandler(this.cboGateway_MouseLeave);
        this.cboGateway.MouseMove += new System.Windows.Forms.MouseEventHandler(this.cboGateway_MouseMove);
        // 
        // cboID
        // 
        this.cboID.BackColor = System.Drawing.SystemColors.Window;
        this.cboID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cboID.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.cboID.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.cboID.ForeColor = System.Drawing.Color.Black;
        this.cboID.FormattingEnabled = true;
        this.cboID.Items.AddRange(new object[] {
            "-Select-",
            "A1A2A3",
            "B1B2B3",
            "C1C2C3",
            "D1D2D3",
            "E1E2E3",
            "F1F2F3",
            "G1G2G3"});
        this.cboID.Location = new System.Drawing.Point(0, 39);
        this.cboID.Name = "cboID";
        this.cboID.Size = new System.Drawing.Size(107, 21);
        this.cboID.TabIndex = 10;
        this.cboID.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboID_DrawItem);
        this.cboID.SelectedIndexChanged += new System.EventHandler(this.cboID_SelectedIndexChanged);
        this.cboID.MouseLeave += new System.EventHandler(this.cboID_MouseLeave);
        this.cboID.MouseMove += new System.Windows.Forms.MouseEventHandler(this.cboID_MouseMove);
        // 
        // btnEdit
        // 
        this.btnEdit.BackColor = System.Drawing.SystemColors.Control;
        this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
        this.btnEdit.ForeColor = System.Drawing.Color.MidnightBlue;
        this.btnEdit.Location = new System.Drawing.Point(144, 210);
        this.btnEdit.Name = "btnEdit";
        this.btnEdit.Size = new System.Drawing.Size(72, 23);
        this.btnEdit.TabIndex = 7;
        this.btnEdit.Text = "Edit";
        this.btnEdit.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        this.btnEdit.UseVisualStyleBackColor = false;
        this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
        // 
        // chkConfirmation
        // 
        this.chkConfirmation.AutoSize = true;
        this.chkConfirmation.Font = new System.Drawing.Font("Corbel", 9F);
        this.chkConfirmation.ForeColor = System.Drawing.Color.Black;
        this.chkConfirmation.Location = new System.Drawing.Point(3, 65);
        this.chkConfirmation.Name = "chkConfirmation";
        this.chkConfirmation.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.chkConfirmation.Size = new System.Drawing.Size(96, 18);
        this.chkConfirmation.TabIndex = 22;
        this.chkConfirmation.Text = "Payment Stop";
        this.chkConfirmation.UseVisualStyleBackColor = true;
        this.chkConfirmation.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chkConfirmation_MouseClick);
        // 
        // cboSlot
        // 
        this.cboSlot.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cboSlot.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.cboSlot.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.cboSlot.ForeColor = System.Drawing.Color.Black;
        this.cboSlot.FormattingEnabled = true;
        this.cboSlot.Items.AddRange(new object[] {
            "-Select-",
            "Tkt No 1",
            "Tkt No 2",
            "Tkt No 3",
            "Tkt No 4",
            "Tkt No 5",
            "Tkt No 6"});
        this.cboSlot.Location = new System.Drawing.Point(112, 39);
        this.cboSlot.Name = "cboSlot";
        this.cboSlot.Size = new System.Drawing.Size(51, 21);
        this.cboSlot.TabIndex = 8;
        this.cboSlot.SelectedIndexChanged += new System.EventHandler(this.cboSlot_SelectedIndexChanged);
        // 
        // BtnOpenPair
        // 
        this.BtnOpenPair.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.BtnOpenPair.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
        this.BtnOpenPair.ForeColor = System.Drawing.Color.Black;
        this.BtnOpenPair.Location = new System.Drawing.Point(166, 39);
        this.BtnOpenPair.Name = "BtnOpenPair";
        this.BtnOpenPair.Size = new System.Drawing.Size(53, 20);
        this.BtnOpenPair.TabIndex = 5;
        this.BtnOpenPair.Text = "Pair";
        this.BtnOpenPair.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        this.BtnOpenPair.UseVisualStyleBackColor = false;
        this.BtnOpenPair.Click += new System.EventHandler(this.BtnOpenPair_Click);
        // 
        // lblJourney
        // 
        this.lblJourney.AutoSize = true;
        this.lblJourney.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblJourney.ForeColor = System.Drawing.Color.Black;
        this.lblJourney.Location = new System.Drawing.Point(-1, 22);
        this.lblJourney.Name = "lblJourney";
        this.lblJourney.Size = new System.Drawing.Size(71, 14);
        this.lblJourney.TabIndex = 2;
        this.lblJourney.Text = "NZM_ NDLS";
        this.lblJourney.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // cboQuota
        // 
        this.cboQuota.BackColor = System.Drawing.SystemColors.Control;
        this.cboQuota.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cboQuota.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.cboQuota.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.cboQuota.ForeColor = System.Drawing.Color.Black;
        this.cboQuota.FormattingEnabled = true;
        this.cboQuota.Items.AddRange(new object[] {
            "GN",
            "TQ",
            "PT",
            "LD"});
        this.cboQuota.Location = new System.Drawing.Point(69, 212);
        this.cboQuota.Name = "cboQuota";
        this.cboQuota.Size = new System.Drawing.Size(67, 21);
        this.cboQuota.TabIndex = 10;
        this.cboQuota.SelectedIndexChanged += new System.EventHandler(this.CboQuota_SelectedIndexChanged);
        // 
        // lblStatus
        // 
        this.lblStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(78)))), ((int)(((byte)(66)))));
        this.lblStatus.Cursor = System.Windows.Forms.Cursors.Hand;
        this.lblStatus.Dock = System.Windows.Forms.DockStyle.Top;
        this.lblStatus.Font = new System.Drawing.Font("MS UI Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(3)))), ((int)(((byte)(29)))), ((int)(((byte)(59)))));
        this.lblStatus.Location = new System.Drawing.Point(0, 0);
        this.lblStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(644, 19);
        this.lblStatus.TabIndex = 0;
        this.lblStatus.Text = "Status Bar";
        this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        this.lblStatus.Click += new System.EventHandler(this.lblStatus_Click);
        // 
        // TopPanel
        // 
        this.TopPanel.Controls.Add(this.lblStatus);
        this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
        this.TopPanel.Location = new System.Drawing.Point(0, 0);
        this.TopPanel.Name = "TopPanel";
        this.TopPanel.Size = new System.Drawing.Size(644, 21);
        this.TopPanel.TabIndex = 28;
        // 
        // btnCancel
        // 
        this.btnCancel.AutoSize = true;
        this.btnCancel.BackColor = System.Drawing.Color.LightGray;
        this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.btnCancel.FlatAppearance.BorderSize = 0;
        this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
        this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnCancel.ForeColor = System.Drawing.Color.Black;
        this.btnCancel.Location = new System.Drawing.Point(621, 39);
        this.btnCancel.Name = "btnCancel";
        this.btnCancel.Size = new System.Drawing.Size(105, 23);
        this.btnCancel.TabIndex = 1180;
        this.btnCancel.Text = "Cancel";
        this.btnCancel.UseVisualStyleBackColor = false;
        this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click_1);
        // 
        // lblTktName
        // 
        this.lblTktName.AutoSize = true;
        this.lblTktName.Font = new System.Drawing.Font("Verdana", 6F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblTktName.ForeColor = System.Drawing.Color.Silver;
        this.lblTktName.Location = new System.Drawing.Point(971, 231);
        this.lblTktName.Name = "lblTktName";
        this.lblTktName.Size = new System.Drawing.Size(35, 10);
        this.lblTktName.TabIndex = 14;
        this.lblTktName.Text = "11012";
        // 
        // timer_0
        // 
        this.timer_0.Interval = 500;
        this.timer_0.Tick += new System.EventHandler(this.timer_0_Tick);
        // 
        // toolTip_0
        // 
        this.toolTip_0.AutoPopDelay = 2000;
        this.toolTip_0.InitialDelay = 500;
        this.toolTip_0.IsBalloon = true;
        this.toolTip_0.ReshowDelay = 100;
        this.toolTip_0.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
        this.toolTip_0.ToolTipTitle = "Ticket Window";
        // 
        // panel2
        // 
        this.panel2.BackColor = System.Drawing.Color.Maroon;
        this.panel2.Cursor = System.Windows.Forms.Cursors.Hand;
        this.panel2.ForeColor = System.Drawing.Color.White;
        this.panel2.Location = new System.Drawing.Point(469, 179);
        this.panel2.Margin = new System.Windows.Forms.Padding(2);
        this.panel2.Name = "panel2";
        this.panel2.Size = new System.Drawing.Size(189, 33);
        this.panel2.TabIndex = 1182;
        // 
        // lblInfo
        // 
        this.lblInfo.AutoSize = true;
        this.lblInfo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblInfo.ForeColor = System.Drawing.Color.White;
        this.lblInfo.Location = new System.Drawing.Point(410, 245);
        this.lblInfo.Name = "lblInfo";
        this.lblInfo.Size = new System.Drawing.Size(101, 14);
        this.lblInfo.TabIndex = 2;
        this.lblInfo.Text = "SHE -> SRP -> SRP";
        // 
        // check_avl
        // 
        this.check_avl.AutoSize = true;
        this.check_avl.Font = new System.Drawing.Font("Corbel", 7.8F);
        this.check_avl.Location = new System.Drawing.Point(590, 92);
        this.check_avl.Margin = new System.Windows.Forms.Padding(2);
        this.check_avl.Name = "check_avl";
        this.check_avl.Size = new System.Drawing.Size(99, 17);
        this.check_avl.TabIndex = 1192;
        this.check_avl.Text = "Fast Availability";
        this.check_avl.UseVisualStyleBackColor = true;
        // 
        // HeaderPanel1
        // 
        this.HeaderPanel1.BackColor = System.Drawing.Color.Thistle;
        this.HeaderPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.HeaderPanel1.Controls.Add(this.Panel5);
        this.HeaderPanel1.Controls.Add(this.btnSlotNo);
        this.HeaderPanel1.Location = new System.Drawing.Point(571, 245);
        this.HeaderPanel1.Margin = new System.Windows.Forms.Padding(2);
        this.HeaderPanel1.Name = "HeaderPanel1";
        this.HeaderPanel1.Size = new System.Drawing.Size(219, 23);
        this.HeaderPanel1.TabIndex = 129;
        // 
        // Panel5
        // 
        this.Panel5.BackColor = System.Drawing.Color.Thistle;
        this.Panel5.Location = new System.Drawing.Point(35, 0);
        this.Panel5.Name = "Panel5";
        this.Panel5.Size = new System.Drawing.Size(145, 21);
        this.Panel5.TabIndex = 1195;
        // 
        // btnSlotNo
        // 
        this.btnSlotNo.BackColor = System.Drawing.Color.MediumTurquoise;
        this.btnSlotNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnSlotNo.ForeColor = System.Drawing.Color.Black;
        this.btnSlotNo.Location = new System.Drawing.Point(181, 0);
        this.btnSlotNo.Name = "btnSlotNo";
        this.btnSlotNo.Size = new System.Drawing.Size(36, 21);
        this.btnSlotNo.TabIndex = 1193;
        this.btnSlotNo.Text = "1";
        this.btnSlotNo.UseVisualStyleBackColor = false;
        // 
        // btnMinize
        // 
        this.btnMinize.BackColor = System.Drawing.Color.Lavender;
        this.btnMinize.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.btnMinize.Location = new System.Drawing.Point(179, 1);
        this.btnMinize.Name = "btnMinize";
        this.btnMinize.Size = new System.Drawing.Size(36, 21);
        this.btnMinize.TabIndex = 1193;
        this.btnMinize.Text = "Xx";
        this.btnMinize.UseVisualStyleBackColor = false;
        // 
        // WLlinkLabel
        // 
        this.WLlinkLabel.ActiveLinkColor = System.Drawing.Color.Aqua;
        this.WLlinkLabel.AutoSize = true;
        this.WLlinkLabel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.WLlinkLabel.ForeColor = System.Drawing.Color.Cornsilk;
        this.WLlinkLabel.LinkColor = System.Drawing.Color.DarkGreen;
        this.WLlinkLabel.Location = new System.Drawing.Point(162, 25);
        this.WLlinkLabel.Name = "WLlinkLabel";
        this.WLlinkLabel.Size = new System.Drawing.Size(52, 14);
        this.WLlinkLabel.TabIndex = 1186;
        this.WLlinkLabel.TabStop = true;
        this.WLlinkLabel.Text = "WL Stop";
        this.WLlinkLabel.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        // 
        // EditlinkLabel
        // 
        this.EditlinkLabel.ActiveLinkColor = System.Drawing.Color.Aqua;
        this.EditlinkLabel.AutoSize = true;
        this.EditlinkLabel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.EditlinkLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        this.EditlinkLabel.LinkColor = System.Drawing.Color.Green;
        this.EditlinkLabel.Location = new System.Drawing.Point(155, 44);
        this.EditlinkLabel.Name = "EditlinkLabel";
        this.EditlinkLabel.Size = new System.Drawing.Size(59, 14);
        this.EditlinkLabel.TabIndex = 1187;
        this.EditlinkLabel.TabStop = true;
        this.EditlinkLabel.Text = "Edit Form";
        this.EditlinkLabel.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
        // 
        // lblClass
        // 
        this.lblClass.AutoSize = true;
        this.lblClass.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblClass.ForeColor = System.Drawing.Color.Black;
        this.lblClass.Location = new System.Drawing.Point(131, 22);
        this.lblClass.Name = "lblClass";
        this.lblClass.Size = new System.Drawing.Size(40, 14);
        this.lblClass.TabIndex = 30;
        this.lblClass.Text = "SL:GN";
        this.lblClass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // cboTrain
        // 
        this.cboTrain.BackColor = System.Drawing.Color.AntiqueWhite;
        this.cboTrain.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cboTrain.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.cboTrain.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.cboTrain.ForeColor = System.Drawing.Color.Black;
        this.cboTrain.FormattingEnabled = true;
        this.cboTrain.Items.AddRange(new object[] {
            "Train No"});
        this.cboTrain.Location = new System.Drawing.Point(115, 2);
        this.cboTrain.Name = "cboTrain";
        this.cboTrain.Size = new System.Drawing.Size(64, 19);
        this.cboTrain.TabIndex = 31;
        // 
        // Panel1
        // 
        this.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        this.Panel1.Controls.Add(this.cboClass);
        this.Panel1.Controls.Add(this.btnMinize);
        this.Panel1.Controls.Add(this.label13);
        this.Panel1.Controls.Add(this.label11);
        this.Panel1.Controls.Add(this.EditlinkLabel);
        this.Panel1.Controls.Add(this.WLlinkLabel);
        this.Panel1.Controls.Add(this.cboTrain);
        this.Panel1.Location = new System.Drawing.Point(674, 153);
        this.Panel1.Name = "Panel1";
        this.Panel1.Size = new System.Drawing.Size(219, 62);
        this.Panel1.TabIndex = 131;
        // 
        // cboClass
        // 
        this.cboClass.BackColor = System.Drawing.Color.White;
        this.cboClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cboClass.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.cboClass.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.cboClass.ForeColor = System.Drawing.Color.Black;
        this.cboClass.FormattingEnabled = true;
        this.cboClass.Items.AddRange(new object[] {
            "1A",
            "2A",
            "3A",
            "CC",
            "SL",
            "2S",
            "3E",
            "EA",
            "EC",
            "EV"});
        this.cboClass.Location = new System.Drawing.Point(41, 3);
        this.cboClass.Name = "cboClass";
        this.cboClass.Size = new System.Drawing.Size(39, 19);
        this.cboClass.TabIndex = 10;
        // 
        // label13
        // 
        this.label13.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label13.ForeColor = System.Drawing.Color.DarkGreen;
        this.label13.Location = new System.Drawing.Point(2, 24);
        this.label13.Name = "label13";
        this.label13.Size = new System.Drawing.Size(32, 16);
        this.label13.TabIndex = 1183;
        this.label13.Text = "Cls :";
        // 
        // label11
        // 
        this.label11.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.label11.ForeColor = System.Drawing.Color.DarkGreen;
        this.label11.Location = new System.Drawing.Point(81, 23);
        this.label11.Name = "label11";
        this.label11.Size = new System.Drawing.Size(27, 16);
        this.label11.TabIndex = 1183;
        this.label11.Text = "Qt :";
        // 
        // lblDate
        // 
        this.lblDate.AutoSize = true;
        this.lblDate.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblDate.ForeColor = System.Drawing.Color.Black;
        this.lblDate.Location = new System.Drawing.Point(178, 22);
        this.lblDate.Name = "lblDate";
        this.lblDate.Size = new System.Drawing.Size(40, 14);
        this.lblDate.TabIndex = 36;
        this.lblDate.Text = "00/00";
        this.lblDate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // PanelPnr
        // 
        this.PanelPnr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(21)))), ((int)(((byte)(22)))));
        this.PanelPnr.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        this.PanelPnr.Controls.Add(this.label5);
        this.PanelPnr.Location = new System.Drawing.Point(30, 437);
        this.PanelPnr.Name = "PanelPnr";
        this.PanelPnr.Size = new System.Drawing.Size(241, 113);
        this.PanelPnr.TabIndex = 1181;
        // 
        // label5
        // 
        this.label5.AutoSize = true;
        this.label5.BackColor = System.Drawing.Color.Transparent;
        this.label5.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold);
        this.label5.ForeColor = System.Drawing.Color.White;
        this.label5.Location = new System.Drawing.Point(134, 96);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(104, 14);
        this.label5.TabIndex = 35;
        this.label5.Text = "DEOS_NDLS- 12/22";
        // 
        // lbl_Detail
        // 
        this.lbl_Detail.BackColor = System.Drawing.Color.Black;
        this.lbl_Detail.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lbl_Detail.ForeColor = System.Drawing.Color.Yellow;
        this.lbl_Detail.Location = new System.Drawing.Point(33, 339);
        this.lbl_Detail.Name = "lbl_Detail";
        this.lbl_Detail.Size = new System.Drawing.Size(83, 76);
        this.lbl_Detail.TabIndex = 34;
        this.lbl_Detail.Text = "Train: 12553\r\nQuota: TQ\r\nClass: SL\r\nSrc: DEOS\r\nDest: NDLS";
        // 
        // lbl_Date
        // 
        this.lbl_Date.AutoSize = true;
        this.lbl_Date.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        this.lbl_Date.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lbl_Date.ForeColor = System.Drawing.Color.Yellow;
        this.lbl_Date.Location = new System.Drawing.Point(203, 388);
        this.lbl_Date.Name = "lbl_Date";
        this.lbl_Date.Size = new System.Drawing.Size(30, 23);
        this.lbl_Date.TabIndex = 31;
        this.lbl_Date.Text = "05";
        // 
        // lbl_Month
        // 
        this.lbl_Month.AutoSize = true;
        this.lbl_Month.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
        this.lbl_Month.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lbl_Month.ForeColor = System.Drawing.Color.Yellow;
        this.lbl_Month.Location = new System.Drawing.Point(186, 408);
        this.lbl_Month.Name = "lbl_Month";
        this.lbl_Month.Size = new System.Drawing.Size(60, 18);
        this.lbl_Month.TabIndex = 32;
        this.lbl_Month.Text = "Jun 2018";
        // 
        // lbl_status
        // 
        this.lbl_status.AutoSize = true;
        this.lbl_status.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
        this.lbl_status.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lbl_status.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(173)))), ((int)(((byte)(211)))));
        this.lbl_status.Location = new System.Drawing.Point(97, 365);
        this.lbl_status.Name = "lbl_status";
        this.lbl_status.Size = new System.Drawing.Size(85, 15);
        this.lbl_status.TabIndex = 33;
        this.lbl_status.Text = "SUCCESS -WEB";
        // 
        // lbl_brand
        // 
        this.lbl_brand.AutoSize = true;
        this.lbl_brand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
        this.lbl_brand.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lbl_brand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(173)))), ((int)(((byte)(211)))));
        this.lbl_brand.Location = new System.Drawing.Point(113, 386);
        this.lbl_brand.Name = "lbl_brand";
        this.lbl_brand.Size = new System.Drawing.Size(76, 19);
        this.lbl_brand.TabIndex = 34;
        this.lbl_brand.Text = "Express";
        // 
        // lbl_time
        // 
        this.lbl_time.AutoSize = true;
        this.lbl_time.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
        this.lbl_time.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lbl_time.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(173)))), ((int)(((byte)(211)))));
        this.lbl_time.Location = new System.Drawing.Point(112, 340);
        this.lbl_time.Name = "lbl_time";
        this.lbl_time.Size = new System.Drawing.Size(58, 18);
        this.lbl_time.TabIndex = 40;
        this.lbl_time.Text = "10:00:55";
        // 
        // B3AAA
        // 
        this.B3AAA.BackColor = System.Drawing.Color.Transparent;
        this.B3AAA.Location = new System.Drawing.Point(774, 421);
        this.B3AAA.Name = "B3AAA";
        this.B3AAA.Size = new System.Drawing.Size(219, 109);
        this.B3AAA.TabIndex = 37;
        // 
        // lbl_slot
        // 
        this.lbl_slot.AutoSize = true;
        this.lbl_slot.BackColor = System.Drawing.Color.Black;
        this.lbl_slot.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold);
        this.lbl_slot.ForeColor = System.Drawing.Color.Yellow;
        this.lbl_slot.Location = new System.Drawing.Point(33, 416);
        this.lbl_slot.Name = "lbl_slot";
        this.lbl_slot.Size = new System.Drawing.Size(51, 15);
        this.lbl_slot.TabIndex = 38;
        this.lbl_slot.Text = "SLOT - 1";
        // 
        // lblPnr
        // 
        this.lblPnr.AutoSize = true;
        this.lblPnr.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblPnr.ForeColor = System.Drawing.Color.Black;
        this.lblPnr.Location = new System.Drawing.Point(1005, 261);
        this.lblPnr.Name = "lblPnr";
        this.lblPnr.Size = new System.Drawing.Size(67, 14);
        this.lblPnr.TabIndex = 1182;
        this.lblPnr.Text = "6715568899";
        // 
        // AppLogin_
        // 
        this.AppLogin_.BackColor = System.Drawing.SystemColors.ActiveBorder;
        this.AppLogin_.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        this.AppLogin_.Cursor = System.Windows.Forms.Cursors.Default;
        this.AppLogin_.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.AppLogin_.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
        this.AppLogin_.ForeColor = System.Drawing.Color.Black;
        this.AppLogin_.Location = new System.Drawing.Point(0, 209);
        this.AppLogin_.Margin = new System.Windows.Forms.Padding(2);
        this.AppLogin_.Name = "AppLogin_";
        this.AppLogin_.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.AppLogin_.Size = new System.Drawing.Size(64, 24);
        this.AppLogin_.TabIndex = 1188;
        this.AppLogin_.Text = "APP";
        this.AppLogin_.UseVisualStyleBackColor = false;
        this.AppLogin_.Click += new System.EventHandler(this.AppLogin_Click);
        // 
        // lblTrainNo
        // 
        this.lblTrainNo.AutoSize = true;
        this.lblTrainNo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblTrainNo.ForeColor = System.Drawing.Color.Black;
        this.lblTrainNo.Location = new System.Drawing.Point(79, 22);
        this.lblTrainNo.Name = "lblTrainNo";
        this.lblTrainNo.Size = new System.Drawing.Size(42, 14);
        this.lblTrainNo.TabIndex = 1189;
        this.lblTrainNo.Text = "00234";
        this.lblTrainNo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // timer_1
        // 
        this.timer_1.Interval = 500;
        this.timer_1.Tick += new System.EventHandler(this.timer_1_Tick);
        // 
        // WebLogin
        // 
        this.WebLogin.BackColor = System.Drawing.Color.PaleTurquoise;
        this.WebLogin.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        this.WebLogin.Cursor = System.Windows.Forms.Cursors.Default;
        this.WebLogin.FlatAppearance.BorderColor = System.Drawing.Color.Yellow;
        this.WebLogin.FlatAppearance.BorderSize = 0;
        this.WebLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
        this.WebLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
        this.WebLogin.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.WebLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.WebLogin.ForeColor = System.Drawing.Color.Black;
        this.WebLogin.Location = new System.Drawing.Point(2, 87);
        this.WebLogin.Margin = new System.Windows.Forms.Padding(2);
        this.WebLogin.Name = "WebLogin";
        this.WebLogin.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.WebLogin.Size = new System.Drawing.Size(56, 23);
        this.WebLogin.TabIndex = 1188;
        this.WebLogin.Text = "Web-1";
        this.WebLogin.UseVisualStyleBackColor = false;
        this.WebLogin.Click += new System.EventHandler(this.WebLogin_Click);
        // 
        // button1
        // 
        this.button1.BackColor = System.Drawing.SystemColors.ActiveCaption;
        this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
        this.button1.ForeColor = System.Drawing.Color.Black;
        this.button1.Location = new System.Drawing.Point(774, 59);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(54, 23);
        this.button1.TabIndex = 1193;
        this.button1.Text = "Pair";
        this.button1.UseVisualStyleBackColor = false;
        this.button1.Click += new System.EventHandler(this.button1_Click_1);
        // 
        // lbl_bnk
        // 
        this.lbl_bnk.AutoSize = true;
        this.lbl_bnk.BackColor = System.Drawing.Color.Transparent;
        this.lbl_bnk.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lbl_bnk.ForeColor = System.Drawing.Color.Black;
        this.lbl_bnk.Location = new System.Drawing.Point(32, 321);
        this.lbl_bnk.Name = "lbl_bnk";
        this.lbl_bnk.Size = new System.Drawing.Size(105, 19);
        this.lbl_bnk.TabIndex = 39;
        this.lbl_bnk.Text = "Bnk : PHONPE";
        // 
        // lblBrand
        // 
        this.lblBrand.BackColor = System.Drawing.Color.Transparent;
        this.lblBrand.Dock = System.Windows.Forms.DockStyle.Top;
        this.lblBrand.Font = new System.Drawing.Font("Verdana", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblBrand.ForeColor = System.Drawing.Color.Gray;
        this.lblBrand.Location = new System.Drawing.Point(0, 0);
        this.lblBrand.Name = "lblBrand";
        this.lblBrand.Size = new System.Drawing.Size(268, 45);
        this.lblBrand.TabIndex = 1195;
        this.lblBrand.Text = "  Express  ";
        // 
        // btnDish
        // 
        this.btnDish.BackColor = System.Drawing.SystemColors.ActiveCaption;
        this.btnDish.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        this.btnDish.Cursor = System.Windows.Forms.Cursors.Default;
        this.btnDish.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.btnDish.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
        this.btnDish.ForeColor = System.Drawing.Color.Black;
        this.btnDish.Location = new System.Drawing.Point(60, 87);
        this.btnDish.Margin = new System.Windows.Forms.Padding(2);
        this.btnDish.Name = "btnDish";
        this.btnDish.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.btnDish.Size = new System.Drawing.Size(56, 23);
        this.btnDish.TabIndex = 1188;
        this.btnDish.Text = "Web-2";
        this.btnDish.UseVisualStyleBackColor = false;
        this.btnDish.Click += new System.EventHandler(this.btnDish_Click);
        // 
        // btnWeb3
        // 
        this.btnWeb3.BackColor = System.Drawing.Color.Thistle;
        this.btnWeb3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        this.btnWeb3.Cursor = System.Windows.Forms.Cursors.Default;
        this.btnWeb3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.btnWeb3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
        this.btnWeb3.ForeColor = System.Drawing.Color.Black;
        this.btnWeb3.Location = new System.Drawing.Point(118, 87);
        this.btnWeb3.Margin = new System.Windows.Forms.Padding(2);
        this.btnWeb3.Name = "btnWeb3";
        this.btnWeb3.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.btnWeb3.Size = new System.Drawing.Size(56, 23);
        this.btnWeb3.TabIndex = 1197;
        this.btnWeb3.Text = "Web-3";
        this.btnWeb3.UseVisualStyleBackColor = false;
        this.btnWeb3.Click += new System.EventHandler(this.btnWeb3_Click);
        // 
        // fake_bank
        // 
        this.fake_bank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.fake_bank.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.fake_bank.Font = new System.Drawing.Font("Verdana", 9F);
        this.fake_bank.FormattingEnabled = true;
        this.fake_bank.IntegralHeight = false;
        this.fake_bank.Location = new System.Drawing.Point(322, 61);
        this.fake_bank.Margin = new System.Windows.Forms.Padding(2);
        this.fake_bank.Name = "fake_bank";
        this.fake_bank.Size = new System.Drawing.Size(216, 22);
        this.fake_bank.TabIndex = 1190;
        // 
        // fake_id
        // 
        this.fake_id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.fake_id.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.fake_id.Font = new System.Drawing.Font("Verdana", 9F);
        this.fake_id.FormattingEnabled = true;
        this.fake_id.Items.AddRange(new object[] {
            "XXXXXXX"});
        this.fake_id.Location = new System.Drawing.Point(322, 39);
        this.fake_id.Name = "fake_id";
        this.fake_id.Size = new System.Drawing.Size(92, 22);
        this.fake_id.TabIndex = 1191;
        // 
        // pnlBrand
        // 
        this.pnlBrand.BackColor = System.Drawing.Color.WhiteSmoke;
        this.pnlBrand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.pnlBrand.Controls.Add(this.lblLoginType);
        this.pnlBrand.Controls.Add(this.lblBank);
        this.pnlBrand.Controls.Add(this.lblBrand);
        this.pnlBrand.Location = new System.Drawing.Point(297, 39);
        this.pnlBrand.Name = "pnlBrand";
        this.pnlBrand.Size = new System.Drawing.Size(268, 73);
        this.pnlBrand.TabIndex = 1198;
        // 
        // lblLoginType
        // 
        this.lblLoginType.AutoSize = true;
        this.lblLoginType.BackColor = System.Drawing.Color.Transparent;
        this.lblLoginType.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblLoginType.ForeColor = System.Drawing.Color.Gray;
        this.lblLoginType.Location = new System.Drawing.Point(161, 53);
        this.lblLoginType.Name = "lblLoginType";
        this.lblLoginType.Size = new System.Drawing.Size(54, 16);
        this.lblLoginType.TabIndex = 1197;
        this.lblLoginType.Text = "WEB-2";
        // 
        // lblBank
        // 
        this.lblBank.AutoSize = true;
        this.lblBank.BackColor = System.Drawing.Color.Transparent;
        this.lblBank.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblBank.ForeColor = System.Drawing.Color.Gray;
        this.lblBank.Location = new System.Drawing.Point(3, 53);
        this.lblBank.Name = "lblBank";
        this.lblBank.Size = new System.Drawing.Size(110, 16);
        this.lblBank.TabIndex = 1196;
        this.lblBank.Text = "FREECHARGE";
        // 
        // BottomPanel
        // 
        this.BottomPanel.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
        this.BottomPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.BottomPanel.Controls.Add(this.lblTime);
        this.BottomPanel.Controls.Add(this.lblAvailability);
        this.BottomPanel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
        this.BottomPanel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
        this.BottomPanel.Location = new System.Drawing.Point(3, 114);
        this.BottomPanel.Margin = new System.Windows.Forms.Padding(2);
        this.BottomPanel.Name = "BottomPanel";
        this.BottomPanel.Size = new System.Drawing.Size(368, 40);
        this.BottomPanel.TabIndex = 1188;
        this.BottomPanel.Visible = false;
        // 
        // lblTime
        // 
        this.lblTime.AutoSize = true;
        this.lblTime.BackColor = System.Drawing.Color.Transparent;
        this.lblTime.Dock = System.Windows.Forms.DockStyle.Right;
        this.lblTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblTime.ForeColor = System.Drawing.Color.Black;
        this.lblTime.Location = new System.Drawing.Point(272, 0);
        this.lblTime.Margin = new System.Windows.Forms.Padding(0);
        this.lblTime.Name = "lblTime";
        this.lblTime.Size = new System.Drawing.Size(96, 25);
        this.lblTime.TabIndex = 13;
        this.lblTime.Text = "00:00:00";
        this.lblTime.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        // 
        // lblAvailability
        // 
        this.lblAvailability.AutoSize = true;
        this.lblAvailability.BackColor = System.Drawing.Color.Transparent;
        this.lblAvailability.Cursor = System.Windows.Forms.Cursors.Hand;
        this.lblAvailability.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        this.lblAvailability.ForeColor = System.Drawing.Color.Black;
        this.lblAvailability.Location = new System.Drawing.Point(0, 2);
        this.lblAvailability.Name = "lblAvailability";
        this.lblAvailability.Size = new System.Drawing.Size(99, 15);
        this.lblAvailability.TabIndex = 1;
        this.lblAvailability.Text = "Get Availability";
        this.lblAvailability.Visible = false;
        // 
        // pictureBox2
        // 
        this.pictureBox2.Location = new System.Drawing.Point(27, 321);
        this.pictureBox2.Name = "pictureBox2";
        this.pictureBox2.Size = new System.Drawing.Size(223, 110);
        this.pictureBox2.TabIndex = 1194;
        this.pictureBox2.TabStop = false;
        // 
        // button2
        // 
        this.button2.BackColor = System.Drawing.Color.GhostWhite;
        this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
        this.button2.Cursor = System.Windows.Forms.Cursors.Default;
        this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
        this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
        this.button2.ForeColor = System.Drawing.Color.Black;
        this.button2.Location = new System.Drawing.Point(176, 87);
        this.button2.Margin = new System.Windows.Forms.Padding(2);
        this.button2.Name = "button2";
        this.button2.RightToLeft = System.Windows.Forms.RightToLeft.No;
        this.button2.Size = new System.Drawing.Size(44, 23);
        this.button2.TabIndex = 1199;
        this.button2.Text = "App";
        this.button2.UseVisualStyleBackColor = false;
        this.button2.Click += new System.EventHandler(this.button2_Click);
        // 
        // TCntrl
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
        this.BackColor = System.Drawing.Color.FloralWhite;
        this.Controls.Add(this.button2);
        this.Controls.Add(this.pnlBrand);
        this.Controls.Add(this.BottomPanel);
        this.Controls.Add(this.button1);
        this.Controls.Add(this.check_avl);
        this.Controls.Add(this.btnCancel);
        this.Controls.Add(this.TopPanel);
        this.Controls.Add(this.lblTrainNo);
        this.Controls.Add(this.lblClass);
        this.Controls.Add(this.lblJourney);
        this.Controls.Add(this.lblDate);
        this.Controls.Add(this.lbl_bnk);
        this.Controls.Add(this.lbl_slot);
        this.Controls.Add(this.lbl_Month);
        this.Controls.Add(this.lbl_Date);
        this.Controls.Add(this.lbl_Detail);
        this.Controls.Add(this.PanelPnr);
        this.Controls.Add(this.lbl_brand);
        this.Controls.Add(this.lbl_status);
        this.Controls.Add(this.lbl_time);
        this.Controls.Add(this.pictureBox2);
        this.Controls.Add(this.WebLogin);
        this.Controls.Add(this.panel2);
        this.Controls.Add(this.HeaderPanel1);
        this.Controls.Add(this.lblPnr);
        this.Controls.Add(this.Panel1);
        this.Controls.Add(this.lblTktName);
        this.Controls.Add(this.lblInfo);
        this.Controls.Add(this.B3AAA);
        this.Controls.Add(this.fake_bank);
        this.Controls.Add(this.fake_id);
        this.Controls.Add(this.btnEdit);
        this.Controls.Add(this.BtnOpenPair);
        this.Controls.Add(this.btnWeb3);
        this.Controls.Add(this.btnDish);
        this.Controls.Add(this.AppLogin_);
        this.Controls.Add(this.cboQuota);
        this.Controls.Add(this.chkConfirmation);
        this.Controls.Add(this.cboID);
        this.Controls.Add(this.cboSlot);
        this.Controls.Add(this.cboGateway);
        this.Margin = new System.Windows.Forms.Padding(2);
        this.Name = "TCntrl";
        this.Size = new System.Drawing.Size(644, 154);
        this.Load += new System.EventHandler(this.TCntrl_Load_1);
        this.TopPanel.ResumeLayout(false);
        this.HeaderPanel1.ResumeLayout(false);
        this.Panel1.ResumeLayout(false);
        this.Panel1.PerformLayout();
        this.PanelPnr.ResumeLayout(false);
        this.PanelPnr.PerformLayout();
        this.pnlBrand.ResumeLayout(false);
        this.pnlBrand.PerformLayout();
        this.BottomPanel.ResumeLayout(false);
        this.BottomPanel.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    private void cboGateway_MouseMove(object sender, MouseEventArgs e)
    {
        if (StatClass.VideoMode)
        {
            cboGateway.ForeColor = Color.Black;
        }
    }

    public static bool checkDotNet()
    {
        bool result = false;
        try
        {
            string text = string.Empty;
            string text2 = "dotnet --list-runtimes";
            string value = text2;
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            string fileName = "cmd.exe";
            processStartInfo.FileName = fileName;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.CreateNoWindow = true;
            ProcessStartInfo startInfo = processStartInfo;
            using (Process process = new Process
            {
                StartInfo = startInfo
            })
            {
                process.Start();
                process.StandardInput.WriteLine(value);
                process.StandardInput.Flush();
                process.StandardInput.Close();
                text = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
            }
            string text3 = text;
            string value2 = "Microsoft.AspNetCore.App";
            bool flag = text3.Contains(value2);
            string text4 = text;
            string value3 = "Microsoft.NETCore.App";
            /*
			if (!(flag | text4.Contains(value3)))
			{
				Form owner = new Form
				{
					TopMost = true
				};
				string text5 = "Please Install .Net FrameWork 6\r\nits Mandatory for PhonePe Bypass\r\nAfter Installed Restart Your Computer";
				string caption = "DotNet";
				MessageBox.Show(owner, text5, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				string text6 = "Do You Want to Download .Net FrameWork 6.0.27";
				string caption2 = "DotNet";
				if (MessageBox.Show(text6, caption2, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					string fileName2 = "https://download.visualstudio.microsoft.com/download/pr/37d12f81-b83d-43d8-91be-7c547a2abfe2/4f6bed6c672458dafd805b007509d9ad/dotnet-sdk-6.0.419-win-x64.exe";
					Process.Start(fileName2);
				}
			}
			else
			{
				result = true;
			}*/
        }
        catch
        {
            result = true;
        }
        return result;
    }

    private void cboGateway_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cboGateway.SelectedIndex == 0)
        {
            return;
        }
        string text = cboGateway.SelectedItem.ToString();
        string value = "@freecharge";
        if (text.Contains(value))
        {
            checkDotNet();
        }
        string value2 = "########";
        if (text.Contains(value2))
        {
            return;
        }
        string text2 = Program.dictionary_00[text];
        try
        {
            string value3 = "SBI_";
            if (text.Contains(value3))
            {
                string value4 = "_DC";
                if (!text.Contains(value4))
                {
                    goto IL_0093;
                }
                string value5 = "_CC";
                if (!text.Contains(value5))
                {
                    goto IL_0093;
                }
            }
            else
            {
                string value6 = "HDFC_";
                if (!text.Contains(value6))
                {
                    string value7 = "ICICI_";
                    if (text.Contains(value7))
                    {
                        string value8 = "_DC";
                        if (!text.Contains(value8))
                        {
                            goto IL_0116;
                        }
                        string value9 = "_CC";
                        if (!text.Contains(value9))
                        {
                            goto IL_0116;
                        }
                    }
                    else
                    {
                        string value10 = "IRCTC_WALLET";
                        if (text.Contains(value10))
                        {
                            string[] array = new string[1];
                            string text3 = "_";
                            array[0] = text3;
                            string[] array2 = text.Split(array, StringSplitOptions.None);
                            eWallet_irctc = array2[1];
                        }
                        else
                        {
                            string value11 = "YES_";
                            if (text.Contains(value11))
                            {
                                string value12 = "_DC";
                                if (!text.Contains(value12))
                                {
                                    goto IL_01da;
                                }
                                string value13 = "_CC";
                                if (!text.Contains(value13))
                                {
                                    goto IL_01da;
                                }
                            }
                            else
                            {
                                string value14 = "PAYTM_";
                                if (!text.Contains(value14))
                                {
                                    string value15 = "E-WALLET_";
                                    if (text.Contains(value15))
                                    {
                                        goto IL_025b;
                                    }
                                    string value16 = "LOYALTY_";
                                    if (text.Contains(value16))
                                    {
                                        goto IL_025b;
                                    }
                                }
                                else
                                {
                                    string value17 = "_DC";
                                    if (!text.Contains(value17))
                                    {
                                        goto IL_02ba;
                                    }
                                    string value18 = "_CC";
                                    if (!text.Contains(value18))
                                    {
                                        goto IL_02ba;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    string value19 = "_DC";
                    if (!text.Contains(value19))
                    {
                        goto IL_0309;
                    }
                    string value20 = "_CC";
                    if (!text.Contains(value20))
                    {
                        goto IL_0309;
                    }
                }
            }
            goto end_IL_0057;
        IL_025b:
            if (!text.Contains(this.string_2))
            {
                string text4 = "Please Select Valid Irctc Account for use this Payment Getways.!";
                MessageBox.Show(text4);
                cboGateway.SelectedIndex = 0;
                return;
            }
            goto end_IL_0057;
        IL_02ba:
            string[] array3 = new string[1];
            string text5 = "||";
            array3[0] = text5;
            if (text2.Split(array3, StringSplitOptions.None).Length <= 3)
            {
            }
            goto end_IL_0057;
        IL_01da:
            string[] array4 = new string[1];
            string text6 = "||";
            array4[0] = text6;
            if (text2.Split(array4, StringSplitOptions.None).Length < 3)
            {
                string text7 = "Please Bypass YES Bank First Then Use it, Other Wise Your Booking May Fail.!";
                MessageBox.Show(text7);
                cboGateway.SelectedIndex = 0;
                return;
            }
            goto end_IL_0057;
        IL_0309:
            string[] array5 = new string[1];
            string text8 = "||";
            array5[0] = text8;
            if (text2.Split(array5, StringSplitOptions.None).Length < 3)
            {
                string text9 = "Please Bypass HDFC Bank First Then Use it, Other Wise Your Booking May Fail.!";
                MessageBox.Show(text9);
                cboGateway.SelectedIndex = 0;
                return;
            }
            goto end_IL_0057;
        IL_0093:
            string[] array6 = new string[1];
            string text10 = "||";
            array6[0] = text10;
            if (text2.Split(array6, StringSplitOptions.None).Length < 4)
            {
            }
            goto end_IL_0057;
        IL_0116:
            string[] array7 = new string[1];
            string text11 = "||";
            array7[0] = text11;
            if (text2.Split(array7, StringSplitOptions.None).Length < 3)
            {
                string text12 = "Please Bypass ICICI Bank First Then Use it, Other Wise Your Booking May Fail.!";
                MessageBox.Show(text12);
                cboGateway.SelectedIndex = 0;
                return;
            }
        end_IL_0057:;
        }
        catch
        {
        }
        if (class48_0.string_23 != text)
        {
            class48_0.string_23 = text;
            string text13 = class48_0.string_23;
            string string_ = class48_0.string_233;
            if (!string.IsNullOrEmpty(string_))
            {
                string text14 = text13;
                string text15 = "-+-";
                text13 = text14 + text15 + string_;
            }
            class48_0.string_24 = text2;
            string string_2 = class48_0.string_24;
            string string_3 = class48_0.string_244;
            if (!string.IsNullOrEmpty(string_3))
            {
                string text16 = "-+-";
                _ = string_2 + text16 + string_3;
            }
            method_7(text13, string_3, lblTktName.Text);
        }
        Label label = lbl_bnk;
        string text17 = "Bnk: ";
        label.Text = text17 + GetBankName(cboGateway.SelectedItem.ToString());
    }

    private void method_7(string string_3, string string_4, string string_5)
    {
        try
        {
            string[] array = new string[7];
            string text = "UPDATE [TBL_TKT] SET [TKT_BANKSAVEAS] = '";
            array[0] = text;
            array[1] = string_3;
            string text2 = "', [TKT_BANKDETAILS] = '";
            array[2] = text2;
            array[3] = string_4;
            string text3 = "' WHERE [TKT_NAME] = '";
            array[4] = text3;
            array[5] = string_5;
            string text4 = "'";
            array[6] = text4;
            Staticm.smethod_UpdateFieldValue(string.Concat(array));
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }

    private void B0AAA(object sender, EventArgs e)
    {
        int num = 0;
        try
        {
            if (string.IsNullOrEmpty(Traindata[0, 0]))
            {
                Invoke(new ChangeTextsSafe(ClearTextinCombo), "");
                Invoke(new ChangeTextsSafe(AddTextinCombo), class48_0.string_2);
                return;
            }
            while (cboTrain.SelectedItem.ToString() != Traindata[num, 0])
            {
                num++;
                if (string.IsNullOrEmpty(Traindata[num, 0]) || Traindata[num, 0] == null)
                {
                    break;
                }
            }
            class48_0.string_2a = Traindata[num, 0];
            class48_0.string_0a = Traindata[num, 2];
            class48_0.string_1a = Traindata[num, 3];
            class48_0.string_0 = class48_0.string_0a;
            class48_0.string_1 = class48_0.string_1a;
            class48_0.string_2 = class48_0.string_2a;
            if (class48_0.string_2 != class48_0.string_2a)
            {
                if (class48_0.string_0 != class48_0.string_3)
                {
                    class48_0.string_3 = class48_0.string_0a;
                }
                if (class48_0.string_0 == class48_0.string_3)
                {
                    Label label = lblJourney;
                    string text = class48_0.string_0;
                    string text2 = "_";
                    label.Text = text + text2 + class48_0.string_1;
                }
                else
                {
                    Label label2 = lblJourney;
                    string text3 = class48_0.string_0;
                    string text4 = "_";
                    label2.Text = text3 + text4 + class48_0.string_1;
                }
            }
        }
        catch
        {
        }
    }

    public bool AddTRaininCombo()
    {
        try
        {
            Invoke(new ChangeTextsSafe(ClearTextinCombo), "");
            string[] array = class48_0.string_38.Split('^');
            for (int i = 0; i < array.Length - 1; i++)
            {
                string[] array2 = array[i].Split('~');
                for (int j = 0; j < array2.Length - 1; j++)
                {
                    Traindata[i, j] = array2[j];
                }
            }
            if (string.IsNullOrEmpty(Traindata[0, 0]))
            {
                return false;
            }
            int num = 0;
            do
            {
                Invoke(new ChangeTextsSafe(AddTextinCombo), Traindata[num, 0]);
                num++;
            }
            while (!string.IsNullOrEmpty(Traindata[num, 0]) && Traindata[num, 0] != "");
        }
        catch
        {
        }
        return false;
    }

    public void AddTextinCombo(string Trn)
    {
        cboTrain.Items.Add(Trn);
    }

    public void ClearTextinCombo(string Trn)
    {
        cboTrain.Items.Clear();
    }

    private void C0AA0(object sender, MouseEventArgs e)
    {
        if (StatClass.VideoMode)
        {
            cboGateway.ForeColor = Color.Black;
        }
    }

    private void cboID_MouseMove(object sender, MouseEventArgs e)
    {
        if (StatClass.VideoMode)
        {
            cboID.ForeColor = Color.Black;
        }
    }

    private void cboSlot_MouseMove(object sender, MouseEventArgs e)
    {
        if (StatClass.VideoMode)
        {
            cboSlot.ForeColor = Color.Black;
        }
    }

    private void cboID_MouseLeave(object sender, EventArgs e)
    {
        if (StatClass.VideoMode)
        {
            cboID.ForeColor = Color.LightGray;
        }
    }

    private void cboSlot_MouseLeave(object sender, EventArgs e)
    {
        if (StatClass.VideoMode)
        {
            cboID.ForeColor = Color.LightGray;
        }
    }

    private void cboGateway_MouseLeave(object sender, EventArgs e)
    {
        if (StatClass.VideoMode)
        {
            cboID.ForeColor = Color.LightGray;
        }
    }

    public void CloseOtherThanPnrBookWindow(object sender)
    {
        try
        {
            string name = base.Name;
            string text = lblTktName.Text;
            _ = chkConfirmation.Checked;
            List<Form> list = new List<Form>();
            foreach (Form openForm in Application.OpenForms)
            {
                list.Add(openForm);
            }
            foreach (Form item in list)
            {
                try
                {
                    string name2 = item.Name;
                    string text2 = "frmProcessTicket";
                    if (!(name2 == text2))
                    {
                        continue;
                    }
                    _ = item.Controls.Count;
                    ControlCollection controls = item.Controls;
                    string key = "pnl";
                    Control control = controls[key];
                    int count = control.Controls.Count;
                    for (int i = 0; i < count; i++)
                    {
                        Control control2 = control.Controls[i];
                        string name3 = control2.Name;
                        ControlCollection controls2 = control2.Controls;
                        string key2 = "lblTktName";
                        string text3 = ((Label)controls2[key2]).Text;
                        _ = control2.Controls.Count;
                        if (name != name3 && text == text3)
                        {
                            ControlCollection controls3 = control2.Controls;
                            string key3 = "btnCancel";
                            ((Button)controls3[key3]).PerformClick();
                            Application.DoEvents();
                            Thread.Sleep(400);
                        }
                    }
                }
                catch
                {
                }
            }
        }
        catch
        {
        }
    }

    private void chkConfirmation_MouseClick(object sender, MouseEventArgs e)
    {
        if (BtnOpenPair.Visible)
        {
            if (!Class45.ManagerDashborad)
            {
                method_8(sender);
                string[] array = new string[6];
                string text = "Nget_chkConfirmation-";
                array[0] = text;
                array[1] = base.Name;
                string text2 = ",";
                array[2] = text2;
                array[3] = lblTktName.Text;
                string text3 = ",";
                array[4] = text3;
                array[5] = chkConfirmation.Checked.ToString();
                Clipboard.SetText(string.Concat(array));
            }
            else
            {
                method_8(sender);
            }
        }
    }

    private void chkConfirmation_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (BtnOpenPair.Visible)
        {
            if (!Class45.ManagerDashborad)
            {
                string[] array = new string[6];
                string text = "Nget_chkConfirmation-";
                array[0] = text;
                array[1] = base.Name;
                string text2 = ",";
                array[2] = text2;
                array[3] = lblTktName.Text;
                string text3 = ",";
                array[4] = text3;
                array[5] = chkConfirmation.Checked.ToString();
                Clipboard.SetText(string.Concat(array));
            }
            else
            {
                method_8(sender);
            }
        }
    }

    private void method_8(object object_1)
    {
        string name = base.Name;
        string text = lblTktName.Text;
        bool @checked = chkConfirmation.Checked;
        List<Form> list = new List<Form>();
        foreach (Form openForm in Application.OpenForms)
        {
            list.Add(openForm);
        }
        foreach (Form item in list)
        {
            string name2 = item.Name;
            string text2 = "frmProcessTicket";
            if (!(name2 == text2))
            {
                continue;
            }
            try
            {
                _ = item.Controls.Count;
                ControlCollection controls = item.Controls;
                string key = "pnl";
                Control control = controls[key];
                int count = control.Controls.Count;
                for (int i = 0; i < count; i++)
                {
                    Control control2 = control.Controls[i];
                    string name3 = control2.Name;
                    ControlCollection controls2 = control2.Controls;
                    string key2 = "lblTktName";
                    string text3 = controls2[key2].Text;
                    _ = control2.Controls.Count;
                    if (name != name3 && text == text3)
                    {
                        ControlCollection controls3 = control2.Controls;
                        string key3 = "cboGateway";
                        ((ComboBox)controls3[key3]).SelectedItem.ToString();
                        ControlCollection controls4 = control2.Controls;
                        string key4 = "chkConfirmation";
                        ((CheckBox)controls4[key4]).Checked = @checked;
                    }
                }
            }
            catch
            {
            }
        }
    }

    private void lblAvailability_MouseClick0(object sender, MouseEventArgs e)
    {
        if (BtnOpenPair.Visible)
        {
            if (!Class45.ManagerDashborad)
            {
                method_9(sender);
                string[] array = new string[6];
                string text = "Nget_check_avl-";
                array[0] = text;
                array[1] = base.Name;
                string text2 = ",";
                array[2] = text2;
                array[3] = lblTktName.Text;
                string text3 = ",";
                array[4] = text3;
                array[5] = check_avl.Checked.ToString();
                Clipboard.SetText(string.Concat(array));
            }
            else
            {
                method_9(sender);
            }
        }
    }

    private void method_9(object object_1)
    {
        string name = base.Name;
        string text = lblTktName.Text;
        bool @checked = check_avl.Checked;
        List<Form> list = new List<Form>();
        foreach (Form openForm in Application.OpenForms)
        {
            list.Add(openForm);
        }
        foreach (Form item in list)
        {
            string name2 = item.Name;
            string text2 = "frmProcessTicket";
            if (!(name2 == text2))
            {
                continue;
            }
            try
            {
                _ = item.Controls.Count;
                ControlCollection controls = item.Controls;
                string key = "pnl";
                Control control = controls[key];
                int count = control.Controls.Count;
                for (int i = 0; i < count; i++)
                {
                    Control control2 = control.Controls[i];
                    string name3 = control2.Name;
                    ControlCollection controls2 = control2.Controls;
                    string key2 = "lblTktName";
                    string text3 = controls2[key2].Text;
                    _ = control2.Controls.Count;
                    if (name != name3 && text == text3)
                    {
                        ControlCollection controls3 = control2.Controls;
                        string key3 = "cboGateway";
                        ((ComboBox)controls3[key3]).SelectedItem.ToString();
                        ControlCollection controls4 = control2.Controls;
                        string key4 = "check_avl";
                        ((CheckBox)controls4[key4]).Checked = @checked;
                    }
                }
            }
            catch
            {
            }
        }
    }

    private void B1AAA(object sender, EventArgs e)
    {
        btnEdit.PerformClick();
    }

    private void C1AA0(object sender, LinkLabelLinkClickedEventArgs e)
    {
        string text = WLlinkLabel.Text;
        string text2 = "WL Allow";
        bool flag;
        if (text == text2)
        {
            flag = true;
            LinkLabel wLlinkLabel = WLlinkLabel;
            string text3 = "WL Stop";
            wLlinkLabel.Text = text3;
        }
        else
        {
            flag = false;
            LinkLabel wLlinkLabel2 = WLlinkLabel;
            string text4 = "WL Allow";
            wLlinkLabel2.Text = text4;
        }
        string[] array = new string[5];
        string text5 = "UPDATE [TBL_TKT] SET [TKT_ISCONFIRM] = '";
        array[0] = text5;
        array[1] = flag.ToString();
        string text6 = "' WHERE [TKT_NAME] = '";
        array[2] = text6;
        array[3] = base.Name;
        string text7 = "'";
        array[4] = text7;
        Staticm.smethod_UpdateFieldValue(string.Concat(array));
        mCOMM mcommInstance_ = Program.mcommInstance_;
        string name = base.Name;
        string text8 = " WL_Status";
        mcommInstance_.SetData(name + text8, WLlinkLabel.Text);
    }

    private void EditlinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        btnEdit.PerformClick();
    }

    private void btnMinize_Click(object sender, EventArgs e)
    {
        if (B2AAA != null)
        {
            B2AAA(this, e);
        }
    }

    private void cboSlot_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string text = cboSlot.SelectedItem.ToString();
            string value = "XX";
            if (!text.Contains(value))
            {
                Button button = btnSlotNo;
                string text2 = "T-";
                string value2 = "-";
                button.Text = text2 + text.Substring(text.IndexOf(value2) + 1);
            }
        }
        catch
        {
            cboSlot.SelectedIndex = 1;
        }
    }

    private void btnSlotNo_Click(object sender, EventArgs e)
    {
        try
        {
            string text = btnSlotNo.Text;
            string oldValue = "T-";
            int num = int.Parse(text.Replace(oldValue, ""));
            string text2;
            if (num + 1 <= Program.int_Slot)
            {
                text2 = (num + 1).ToString();
            }
            else
            {
                string text3 = "1";
                text2 = text3;
            }
            string text4 = text2;
            Button button = btnSlotNo;
            string text5 = "T-";
            button.Text = text5 + text4;
            cboSlot.SelectedIndex = int.Parse(text4);
        }
        catch
        {
        }
    }

    private void BtnOpenPair_Click(object sender, EventArgs e)
    {
        blb = true;
        if (eventHandler_3 != null)
        {
            eventHandler_3(this, e);
        }
    }

    private void CboQuota_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(CLc))
        {
            CLc = lblClass.Text;
        }
        Label label = lblClass;
        string cLc = CLc;
        string text = ":";
        label.Text = cLc + text + cboQuota.SelectedItem.ToString();
    }

    private void button1_Click_1(object sender, EventArgs e)
    {
        if (eventHandler_3 != null)
        {
            eventHandler_3(this, e);
        }
    }

    private void lblStatus_Click(object sender, EventArgs e)
    {
        MessageBox.Show(lblStatus.Text);
    }

    private void button2_Click(object sender, EventArgs e)
    {
        ResetByteNovaSuccessDesign();
        string str = cboGateway.SelectedItem.ToString();
        string app = "APP";
        if (button2.Text == "Mix")
        {
            app = "Mix";
        }
        if (!checkPaymet(str, app))
        {
            StatClass.ApptLogin = false;
            StatClass.WebLogin = false;
            string text2 = "Selected Payment Gateway is Not Working\r\nPlease Choose Another Payment Option";
            MessageBox.Show(text2);
            return;
        }
        Betalogin = false;
        Dishalogin = false;
        web3 = false;
        Applogin = true;
        loginType = 5;
        if (app == "Mix")
        {
            Applogin = false;
            mixlogin = true;
            loginType = 6;
        }
        ChnageColour_();
        Label label = lbl_status;
        string text3 = "SUCCESS-APP";
        label.Text = text3;
        method_4();
    }
}
