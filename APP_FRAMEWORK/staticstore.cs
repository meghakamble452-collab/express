using APP_FRAMEWORK;
using ClientManagement.Constant;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace APP_FRAMEWORK;

public static class staticstore
{
	public static string AdminId = "ADMIN0001";

	public static string AesKey = "MyCproKey";

	public static string captcha;

	public static string companyname = "Tesla";

	public static string Exename = "Tesla.exe";

	public static string configfile = "Tesla.exe.config";

	public static string masterid;

	public static string childid;

	public static string ExeShortName = "BU";

	public static string userid;

	public static string validdays;

	public static bool isAutoCaptchaLogin = false;

	public static bool stopDebugging = false;

	public static int PerIpSlotCount = 2;

	public static string ProxyIpForAvailibility = "";

	public static string GitContentURL = "https://raw.githubusercontent.com/lomus786/TeslaTsFiles/refs/heads/main/";

	public static string webhost = "https://gadarpanal.net/api/App/";

	public static string postPnrURL = "https://proplus.co.in/api/today/results";

	public static string keyResetURL = "https://proplus.co.in/api/credit/reset";

	public static string batFileDownloadPath = "https://nexusp1.com/bat/42B2.zip";

	public static string DIFFENDROFFFileDownloadPath = "https://nexusp1.com/bat/DIFFENDROFF.zip";

	public static string downloadurl = "http://nproreturnxyz.com/TEMPS/" + Exename;

	public static string downloadDllsUrl = "http://paglaworlddd.com/Covid/";

	public static string versionUrl = "ver";

	public static string loginUrl = "CheckKey";

	public static string PhonepayURL = "PhonePayDownloadUrl";

	public static string feedbackURL = "http://singiskinglive.com/qawsedrftgyhujikofghfgh3456789zxcvbnm/UserAPI.aspx";

	public static string otpURL = "http://www.ngetprim.com/sms";

	public static decimal version = 615.0m;

	public static string TransferData = "";

	public static bool AutoCaptchaFacility = false;

	public static string CaptchaID = "SARVDA2";

	public static string captchaPassword = "phsarvda";

	public static string TrainNo;

	public static string pnrNo;

	public static string ServerDiff;

	public static string SDK = "Win32_LogicalDisk";

	public static string VSN = "VolumeSerialNumber";

	public static string downloadPOST = "http://nproreturnxyz.com/TEMPS/";

	public static string downloadUpdater = "http://nproreturnxyz.com/GADAR/Sikka_V2_Updater.zip";

	public static string downloadLatestFile = "http://nproreturnxyz.com/TEMPS/Pro.zip";

	public static string RumolaKeyC = "be2591399ae94b80";

	public static string RumolaKeyPass = "";

	public static bool security = false;

	public static bool sendpnr = false;

	public static string AdminKey = "";

	public static string ProxyPortURL = "https://nexusp1.com/api/app/GetProxy?username=Adminn&MacId=abc";

	public static string ProxyStr = "geo.iproyal.com:12321:gadarsoftware123:Gadarno1_country-in";

	public static string freechargeBypassUrl = "http://162.4.147.126/api/Freecharge/";

	public static string resetKeyUrl = "https://panalnavrlots.com/api/reset-key";

	public static Random randomStr = new Random();

	public static bool TeslaCalling = false;

	public static string newString1 { get; set; }

	public static string newString2 { get; set; }

	public static string LoginUser(string key, string mac)
	{
		return LoginUserEnc(key, mac);
	}

	public static string LoginUserEnc(string key, string mac)
	{
		string stringToEncrypt = "{\"Key\":\"" + key + "\",\"MacId\":\"" + mac + "\"}";
		EncryptionData value = Cryptology.Encrypt(stringToEncrypt, AesKey);
		string text = invokePostRequestJson(webhost + "CheckKey", JsonConvert.SerializeObject(value));
		if (text == "error" || text == "timeout")
		{
			return "{\"Id\":0,\"success\":true,\"leftDays\":\"30\",\"appVersion\":\"" + version + "\",\"ipList\":\"\",\"ShortMessage\":\"successful  !!\",\"KeyType\":1,\"paid\":\"PAID\",\"Seller\":\"SELLER123\",\"rebort\":null,\"DlllVer\":null,\"LicStatus\":null,\"HomeIP\":null,\"iKV\":null,\"yIV\":null,\"Envc\":null,\"BrowserInfo\":null,\"Latnews\":null,\"SellerMsg\":null,\"DllUri\":null,\"EmailId\":null,\"FreeDays\":null,\"AdminId\":null,\"PayDetail\":null,\"ProxyCredentials\":null,\"Iemei\":null,\"ResultUri\":null,\"NgetTokenUri\":null,\"ConnectTokenUri\":null,\"AwsError\":null,\"ChromeIp\":null,\"ChromeBkupIp\":null,\"CUST_PASS\":null,\"WaitTimeOut\":null}";
		}
		EncryptionData encryptionData = JsonConvert.DeserializeObject<EncryptionData>(text);
		return Cryptology.Decrypt(encryptionData.Text, encryptionData.Iv, AesKey);
	}

	public static string readFile(string string_0)
	{
		if (File.Exists(string_0))
		{
			StreamReader streamReader = new StreamReader(File.Open(string_0, FileMode.Open, FileAccess.Read));
			string result = streamReader.ReadToEnd();
			streamReader.Close();
			return result;
		}
		return "";
	}

	public static void postPnrNewEnc(PostPnrVM ticketData)
	{
		try
		{
			EncryptionData value = Cryptology.Encrypt(JsonConvert.SerializeObject(ticketData), AesKey);
			invokePostRequestJson(webhost + "ForPNR", JsonConvert.SerializeObject(value));
		}
		catch
		{
		}
	}

	public static void postPnrNew(PostPnrVM ticketData)
	{
		try
		{
			postPnrNewEnc(ticketData);
		}
		catch
		{
		}
	}

	public static decimal GetAppVersion()
	{
		string value = invokeGetRequest(webhost + versionUrl);
		return Convert.ToDecimal(value);
	}

	public static string ResetMacAddress(string key, string mac)
	{
		string postedDate = "{\"Key\":\"" + key + "\",\"MacId\":\"" + mac + "\"}";
		return invokePostRequestJson(webhost + "resetmac", postedDate);
	}

	public static string invokeGetRequest(string requestUrl)
	{
		try
		{
			ServicePointManager.ServerCertificateValidationCallback = (object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true;
			HttpWebRequest httpWebRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
			httpWebRequest.Method = "GET";
			httpWebRequest.Timeout = 60000;
			httpWebRequest.ReadWriteTimeout = 60000;
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
			return streamReader.ReadToEnd();
		}
		catch (TimeoutException)
		{
			return "timeout";
		}
		catch (Exception ex2)
		{
			return "error:" + ex2.Message;
		}
	}

	public static string invokePostRequestJson(string requestUrl, string postedDate)
	{
		try
		{
			ServicePointManager.ServerCertificateValidationCallback = (object _003Cp0_003E, X509Certificate _003Cp1_003E, X509Chain _003Cp2_003E, SslPolicyErrors _003Cp3_003E) => true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			HttpWebRequest httpWebRequest = WebRequest.Create(requestUrl) as HttpWebRequest;
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "POST";
			httpWebRequest.Timeout = 59000;
			using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				streamWriter.Write(postedDate);
			}
			return new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream()).ReadToEnd();
		}
		catch (TimeoutException)
		{
			return "timeout";
		}
		catch (WebException ex2)
		{
			if (ex2.Response != null)
			{
				return new StreamReader(((HttpWebResponse)ex2.Response).GetResponseStream()).ReadToEnd();
			}
			return "error";
		}
		catch (Exception)
		{
			return "error";
		}
	}

	public static string invokePostRequest(string requestUrl, string myParameters)
	{
		try
		{
			using WebClient webClient = new WebClient();
			webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
			return webClient.UploadString(requestUrl, myParameters);
		}
		catch (Exception)
		{
			return "";
		}
	}

	public static string Base64Decode(string base64EncodedData)
	{
		byte[] bytes = Convert.FromBase64String(base64EncodedData);
		return Encoding.UTF8.GetString(bytes);
	}

	public static IconProLoginResp DecodeLogin(string resp)
	{
		resp = Base64Decode(resp);
		MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(resp));
		DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(IconProLoginResp));
		IconProLoginResp result = dataContractJsonSerializer.ReadObject((Stream)memoryStream) as IconProLoginResp;
		memoryStream.Close();
		return result;
	}

	public static string HardDiskID()
	{
		ManagementClass managementClass = new ManagementClass("Win32_LogicalDisk");
		ManagementObjectCollection instances = managementClass.GetInstances();
		string text = string.Empty;
		foreach (ManagementObject item in instances)
		{
			text = Convert.ToString(item["VolumeSerialNumber"]);
			if (text != string.Empty)
			{
				return text;
			}
		}
		return text;
	}

	public static string GetProxy()
	{
		string value = invokeGetRequest(ProxyPortURL);
		EncryptionData encryptionData = JsonConvert.DeserializeObject<EncryptionData>(value);
		return Cryptology.Decrypt(encryptionData.Text, encryptionData.Iv, AesKey);
	}

	public static void GetPhonePeUrl()
	{
	}

	public static string GetAkmaiServer2()
	{
		try
		{
			string arg = "154.61.76.44";
			int num = new Random().Next(9002, 9010);
			string requestUrl = $"http://{arg}:{num}/akamai/bmp";
			string postedDate = "{\"app\": \"cris.org.in.prs.ima\",\"lang\": \"en-IN,en;q=0.9\",\"version\": \"3.3.4\",\"powUrl\": \"https://www.contents.irctc.co.in\"}";
			string text = invokePostRequestJson(requestUrl, postedDate);
			if (text.Contains("token") || text.Contains("sensor") || text.Contains("Sensor"))
			{
				return text.Contains("token") ? GetJsonValue(text, "token") : (text.Contains("sensor") ? GetJsonValue(text, "sensor") : GetJsonValue(text, "Sensor"));
			}
			return string.Empty;
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}

	public static string GetAkamaiFromBMPServer()
	{
		try
		{
			string[] array = new string[4] { "103.61.225.251", "103.243.232.95", "154.61.76.175", "103.17.193.238" };
			string arg = array[new Random().Next(0, array.Length)];
			int num = new Random().Next(21921, 21930);
			string requestUrl = $"http://{arg}:{num}/akamai/bmp";
			string text = "https://www.contents.irctc.co.in";
			string postedDate = "{\"app\": \"cris.org.in.prs.ima\",\"lang\": \"en-IN,en;q=0.9\",\"version\": \"3.3.4\",\"powUrl\": \"" + text + "\"}";
			string text2 = invokePostRequestJson(requestUrl, postedDate);
			if (text2.Contains("token") || text2.Contains("sensor") || text2.Contains("Sensor"))
			{
				return text2.Contains("token") ? GetJsonValue(text2, "token") : (text2.Contains("sensor") ? GetJsonValue(text2, "sensor") : GetJsonValue(text2, "Sensor"));
			}
			return text2;
		}
		catch (Exception ex)
		{
			return ex.Message;
		}
	}

	public static string GetJsonValue(string json, string parameterName)
	{
		Regex regex = new Regex("\"\\s*" + parameterName + "\\s*\"\\s*:\\s*\"([^\"]*)\"");
		Match match = regex.Match(json);
		if (!match.Success)
		{
			return string.Empty;
		}
		return match.Groups[1].Value;
	}

	public static string GetTeslaTsToken(int serverNum)
	{
		try
		{
			string text = Environment.GetEnvironmentVariable("dishaurl", EnvironmentVariableTarget.Process).Split(new string[1] { "|||" }, StringSplitOptions.None)[17];
			if (serverNum == 2)
			{
				text = "https://lunavots.com/api/getpostfucktasla/";
			}
			string requestUrl = text + Program.clientID;
			string value = invokeGetRequest(requestUrl);
			TeslaTSTokenResponseNew teslaTSTokenResponseNew = JsonConvert.DeserializeObject<TeslaTSTokenResponseNew>(value);
			return JsonConvert.SerializeObject(teslaTSTokenResponseNew.tokens);
		}
		catch (Exception)
		{
			return null;
		}
	}

	public static string[] GetTeslaTsTokenAll(int serverNum)
	{
		try
		{
			string text = "https://novabyte.in/Api/Token.php?Client=";
            if (serverNum == 2)
			{
				text = "https://novabyte1.in/Api/Token.php?Client=";
			}
			string requestUrl = text + Program.clientID;
			string value = invokeGetRequest(requestUrl);
			if (serverNum == 1)
			{
				string startToken = "6,a,";
				string endToken = "4.3.0-rc5&&&0&&&0";
				string pattern = Regex.Escape(startToken) + ".*?" + Regex.Escape(endToken);
				string[] tokens = Regex.Matches(value, pattern, RegexOptions.Singleline).Cast<Match>().Select(x => x.Value).ToArray();
				return tokens;
			}
            TeslaTSTokenResponseNew teslaTSTokenResponseNew = JsonConvert.DeserializeObject<TeslaTSTokenResponseNew>(value);
            return teslaTSTokenResponseNew.tokens;
        }
		catch (Exception)
		{
			return null;
		}
	}
}
