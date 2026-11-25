using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace APP_FRAMEWORK;

internal class Class30
{
public static void StartSQLforDatabase()
{
string[] array = new string[14];
array[0] = "CREATE TABLE TBL_TKT(RID integer PRIMARY KEY AUTOINCREMENT,TKT_NAME varchar(20),SRC_STN varchar(30),DEST_STN varchar(30),BRD_STN varchar(30),TRAIN_NO varchar(20),TKT_DAY varchar(10),TKT_MONTH varchar(10),TKT_YEAR varchar(10),TKT_CLASS varchar(10),TKT_QUOTA varchar(10),TKT_TRNTYPE varchar(10),TKT_TRDAY varchar(10),TKT_ISADV varchar(10),TKT_ISCONFIRM varchar(10),TKT_ISAUTOUPGRD varchar(10),TKT_ISBEDROLL varchar(10),TKT_COACH varchar(10),TKT_MOBILE varchar(10),TKT_IRCTCID varchar(20),TKT_PWD varchar(20),TKT_SLOT varchar(20),TKT_PAYMENT varchar(50),TKT_BANKSAVEAS varchar(50),TKT_BANKDETAILS varchar(80),TKT_FAREDETAILS varchar(80),TR_LIST varchar(10000),Remarks varchar(50),TKT_ISRET varchar(10),TKT_ISPRI varchar(10),RSRC_STN varchar(30),RDEST_STN varchar(30),RBRD_STN varchar(30),RTRAIN_NO varchar(20),RTKT_DAY varchar(10),RTKT_MONTH varchar(10),RTKT_YEAR varchar(10),RTKT_CLASS varchar(10),RTKT_QUOTA varchar(10),RTKT_TRNTYPE varchar(10),RTKT_TRDAY varchar(10),RTR_LIST varchar(10000),TKT_STOPPAY varchar(10), WebLog varchar(30), AppLog varchar(30), Pnr varchar(15), PairId varchar(15), PairNo varchar(15), Create_Date DATETIME);";
array[1] = "CREATE TABLE TBL_PAX(RID integer PRIMARY KEY AUTOINCREMENT,PAX_NAME varchar(20),PAX_AGE varchar(5),PAX_SEX varchar(10),PAX_BIRTH varchar(20),PAX_FOOD varchar(20),PAX_IDTYPE varchar(30),PAX_IDNO varchar(40),PAX_CONC varchar(20),TKT_ID varchar(20),Remarks varchar(50),Create_Date varchar(50));";
array[2] = "CREATE TABLE TBL_USER(RID integer PRIMARY KEY AUTOINCREMENT,UserName varchar(50) NOT NULL, strPwd varchar(50) NOT NULL, UserState varchar(50), History varchar(1000), Remarks varchar(5000));";
array[3] = "CREATE TABLE TBL_BANK(RID integer PRIMARY KEY AUTOINCREMENT,BANK_NAME varchar(50),BANK_SAVEAS varchar(50),BANK_DETAILS varchar(50),BANK_ISDEBIT varchar(20), Remarks varchar(2000));";
array[4] = "CREATE TABLE TBL_HISTORY(RID integer PRIMARY KEY AUTOINCREMENT,TCNTRL_NAME varchar(20) NOT NULL, TKT_NAME varchar(50) NOT NULL, IRCTC_ID varchar(20),stnFrom varchar(20),stnTo varchar(20),Journey_Date varchar(20),BANK_NAME varchar(16),PNR varchar(12),AMT varchar(10), Remarks varchar(50),Create_Date varchar(50));";
array[5] = "CREATE TABLE TBL_STN(RID integer PRIMARY KEY AUTOINCREMENT,STNCD varchar(50) NOT NULL, STNNAME varchar(50) NOT NULL);";
array[6] = "CREATE TABLE tbl_RegUser(UserId varchar(30) PRIMARY KEY NOT NULL, strPwd varchar(50) NOT NULL, strMachine varchar(50) NOT NULL, strMachinePwd varchar(50), OpenedWindo varchar(5), NgetWindo varchar(5), AppWindo varchar(5));";
array[7] = "CREATE TABLE TBL_IP(IP varchar(50), Pnr1 varchar(50), Pnr2 varchar(50), Remarks varchar(50));";
array[8] = "CREATE TABLE TBL_LOG(RID integer PRIMARY KEY AUTOINCREMENT, TYPE varchar(10), LOG varchar(1000000), Remarks varchar(50));";
array[9] = "CREATE TABLE tbl_Train(TrainNo varchar(10), Traintype varchar(20));";
array[10] = "CREATE TABLE TBL_UPI(RID integer PRIMARY KEY AUTOINCREMENT, MobNo varchar(15), UPIAdd varchar(15), RegiInfo varchar(50000), LoginInfo varchar(100000), AccInfo varchar(100000), TrnsInfo varchar(100000), SessInfo varchar(100000), TimeStamp varchar(50));";
array[11] = "CREATE TABLE TBL_NPCI(RID integer PRIMARY KEY AUTOINCREMENT, MobNo varchar(15), UPIAdd varchar(15), RegiInfo varchar(50000), LoginInfo varchar(100000), AccInfo varchar(100000), TrnsInfo varchar(100000), SessInfo varchar(100000), TimeStamp varchar(50));";
array[12] = "CREATE TABLE TBL_SBIUPI(RID integer PRIMARY KEY AUTOINCREMENT, MobNo varchar(15), UPIAdd varchar(15), RegiInfo varchar(50000), LoginInfo varchar(100000), AccInfo varchar(100000), TrnsInfo varchar(100000), SessInfo varchar(100000), TimeStamp varchar(50));";
array[13] = "CREATE TABLE TBL_FREECHARGE(RID integer PRIMARY KEY AUTOINCREMENT, MobNo varchar(15), UPIAdd varchar(15), RegiInfo varchar(50000), LoginInfo varchar(100000), AccInfo varchar(100000), TrnsInfo varchar(100000), SessInfo varchar(100000), TimeStamp varchar(50));";

SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
sQLiteConnection.Open();
SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
for (int i = 0; i < array.Length; i++)
		{
			sQLiteCommand.CommandText = array[i];
			sQLiteCommand.ExecuteNonQuery();
		}
		sQLiteCommand.Dispose();
		if (sQLiteConnection.State == ConnectionState.Open)
		{
			sQLiteConnection.Close();
		}
	}

	public static void CreateNewTable()
	{
		try
		{
			string commandText = "CREATE TABLE IF NOT EXISTS AUTH_PASS(Username varchar(30), Password varchar(30));";
			SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
			sQLiteConnection.Open();
			SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
			sQLiteCommand.CommandText = commandText;
			sQLiteCommand.ExecuteNonQuery();
			sQLiteCommand.Dispose();
			if (sQLiteConnection.State == ConnectionState.Open)
			{
				sQLiteConnection.Close();
			}

			commandText = "CREATE TABLE IF NOT EXISTS TBL_PHONEPE(RID integer PRIMARY KEY AUTOINCREMENT, MobNo varchar(15), UPIAdd varchar(15), RegiInfo varchar(900000), LoginInfo varchar(900000), AccInfo varchar(900000), TrnsInfo varchar(900000), SessInfo varchar(900000), TimeStamp varchar(50));";
			sQLiteConnection = new SQLiteConnection(Class34.string_Database);
			sQLiteConnection.Open();
			sQLiteCommand = new SQLiteCommand(sQLiteConnection);
			sQLiteCommand.CommandText = commandText;
			sQLiteCommand.ExecuteNonQuery();
			sQLiteCommand.Dispose();
			if (sQLiteConnection.State == ConnectionState.Open)
			{
				sQLiteConnection.Close();
			}

			if (!string.IsNullOrEmpty(ReadData_checkUser()))
			{
				Program.StrongPassword = ReadDataTable();
				return;
			}

			sQLiteConnection = new SQLiteConnection(Class34.string_Database);
			sQLiteCommand = new SQLiteCommand();
			sQLiteConnection.Open();
			sQLiteCommand.Connection = sQLiteConnection;
			sQLiteCommand.CommandText = "INSERT INTO [AUTH_PASS]([Username],[Password]) VALUES('" + Program.clientID + "', '0')";
			sQLiteCommand.ExecuteNonQuery();
			sQLiteConnection.Close();
			sQLiteCommand.Dispose();
			Program.StrongPassword = "0";
		}
		catch
		{
		}
	}

	public static void UpdateStrongPassword(string pass)
	{
		SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
		SQLiteCommand sQLiteCommand = new SQLiteCommand();
		sQLiteConnection.Open();
		sQLiteCommand.Connection = sQLiteConnection;
		sQLiteCommand.CommandText = "UPDATE [AUTH_PASS] SET [Password] = '" + pass + "' WHERE [Username] = '" + Program.clientID + "'";
		sQLiteCommand.ExecuteNonQuery();
		sQLiteConnection.Close();
		sQLiteCommand.Dispose();
	}

	public static string ReadData_checkUser()
	{
		string result = "";
		DataTable dataTable = new DataTable();
		new SQLiteDataAdapter("SELECT * FROM AUTH_PASS", Class34.string_Database).Fill(dataTable);
		if (dataTable != null && dataTable.Rows.Count > 0)
{
foreach (DataRow row in dataTable.Rows)
{
result = row["Username"].ToString();
}
}
return result;
}

public static string ReadData_checkUser2()
{
string result = "";
DataTable dataTable = new DataTable();
new SQLiteDataAdapter("SELECT * FROM TBL_PHONEPE", Class34.string_Database).Fill(dataTable);
if (dataTable != null && dataTable.Rows.Count > 0)
{
foreach (DataRow row in dataTable.Rows)
{
result = row["Username"].ToString();
}
}
return result;
}

public static string ReadDataTable()
{
string result = "";
DataTable dataTable = new DataTable();
new SQLiteDataAdapter("SELECT * FROM AUTH_PASS WHERE Username LIKE '" + Program.clientID + "'", Class34.string_Database).Fill(dataTable);
if (dataTable != null && dataTable.Rows.Count > 0)
{
foreach (DataRow row in dataTable.Rows)
{
result = row["Password"].ToString();
}
}
return result;
}

public static void smethod_11()
{
try
{
SQLiteConnection sQLiteConnection;
while (true)
{
sQLiteConnection = new SQLiteConnection(Class34.string_Database);
sQLiteConnection.Open();
SQLiteCommand sQLiteCommand = new SQLiteCommand(sQLiteConnection);
try
{
sQLiteCommand.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='tbl_RegUser' LIMIT 1;";
object obj = sQLiteCommand.ExecuteScalar();
if (obj != null && !(obj is DBNull))
{
sQLiteCommand.CommandText = "SELECT UserId FROM tbl_RegUser LIMIT 1";
object obj2 = sQLiteCommand.ExecuteScalar();
if (obj2 == null || obj2 is DBNull)
{
sQLiteCommand.CommandText = "INSERT INTO tbl_RegUser(UserId, strPwd, strMachine, strMachinePwd) VALUES('" + Program.clientID + "', '" + Program.string_00 + "', '" + Program.mac + "', '" + Program.string_01 + "')";
sQLiteCommand.ExecuteNonQuery();
break;
}

sQLiteCommand.CommandText = "SELECT strPwd FROM tbl_RegUser LIMIT 1";
object obj3 = sQLiteCommand.ExecuteScalar();
if ((obj2.ToString().ToUpper() != Program.clientID.ToUpper()) & (obj3.ToString().ToUpper() != Program.string_00.ToUpper()))
{
sQLiteCommand.CommandText = "UPDATE tbl_RegUser SET UserId='" + Program.clientID + "', strPwd='" + Program.string_00 + "', strMachine='" + Program.mac + "', strMachinePwd='" + Program.string_01 + "'";
sQLiteCommand.ExecuteNonQuery();
Class28.smethod_15(clearPwd: true, 0);
}
break;
}
StartSQLforDatabase();
}
catch (Exception ex)
{
ex.Message.ToString().Contains("file is encrypted or is not a database");
ex.Message.ToString().Contains("no such table");
break;
}
finally
{
sQLiteCommand.Dispose();
}
}
if (sQLiteConnection.State == ConnectionState.Open)
{
sQLiteConnection.Close();
}
}
catch (Exception ex2)
{
Class28.smethod_26("SQLite", ex2.ToString());
}
}

private static void updatePWD(string pwd)
{
try
{
using SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
sQLiteConnection.Open();
using (SQLiteCommand sQLiteCommand = new SQLiteCommand("PRAGMA rekey='" + pwd + "'", sQLiteConnection))
{
sQLiteCommand.ExecuteNonQuery();
}
sQLiteConnection.Close();
}
catch (Exception)
{
}
}

public static void CleanFiles()
{
if (!Directory.Exists(Program.StartupPath))
{
return;
}
FileInfo[] files = new DirectoryInfo(Program.StartupPath).GetFiles();
foreach (FileInfo fileInfo in files)
{
if (fileInfo.Name == "Database.db" && !IsFileLocked(fileInfo))
{
// fileInfo.Delete();
}
}
}

public static bool IsFileLocked(FileInfo file)
{
FileStream fileStream = null;
try
{
fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
}
catch (IOException)
{
return true;
}
finally
{
fileStream?.Close();
}
return false;
}

public static void smethod_4()
{
if (string.IsNullOrEmpty(Program.DBPASS))
{
Program.DBPASS = "ExpressLocalKey";
}

string text2 = Program.StartupPath + "\\Database.db";
string oldPwd = "";
try
{
if (File.Exists("IRLOG//pwd"))
{
oldPwd = File.ReadAllText("IRLOG//pwd").Trim();
}
}
catch
{
}

Class34.string_Database = "Server=localhost; Data Source = " + text2 + "; Version = 3; Password= " + Program.string_1A + "; Compress=True;";

if (!string.IsNullOrEmpty(Program.DBPASS) && Program.DBPASS != Program.string_1A)
{
updatePWD(Program.DBPASS);
}
Program.string_1A = Program.DBPASS;

string path4 = "Your Id- " + Program.clientID + ".inf";
if (!File.Exists(path4))
{
StreamWriter streamWriter = new StreamWriter(path4);
streamWriter.Write(DateTime.Today.ToString("M/d/yyyy"));
streamWriter.Close();
}

Class34.string_Database = "Server=localhost; Data Source = " + text2 + "; Version = 3; Password= " + Program.string_1A + "; Compress=True;";

try
{
SQLiteConnection sQLiteConnection = new SQLiteConnection(Class34.string_Database);
sQLiteConnection.Open();
using (SQLiteCommand sQLiteCommand = new SQLiteCommand("PRAGMA schema_version;", sQLiteConnection))
{
sQLiteCommand.ExecuteScalar();
}
sQLiteConnection.Close();
new FileInfo(text2).Attributes = FileAttributes.Hidden;
EnsureAllTables(sQLiteConnection);

try
{
if (File.Exists("IRLOG//repair.flag"))
{
File.Delete("IRLOG//repair.flag");
}
}
catch
{
}
try
{
File.WriteAllText("IRLOG//pwd", Program.DBPASS);
}
catch
{
}
}
catch (Exception ex)
{
Class28.smethod_26("DBOpen", ex.ToString());

try
{
if (File.Exists("Database.db"))
{
File.Copy("Database.db", "Database.db.bak", true);
}
}
catch
{
}

bool ok = false;
if (!string.IsNullOrEmpty(oldPwd) && oldPwd != Program.DBPASS)
{
string oldCs = "Server=localhost; Data Source = " + text2 + "; Version = 3; Password= " + oldPwd + "; Compress=True;";
try
{
using (SQLiteConnection oldCon = new SQLiteConnection(oldCs))
{
oldCon.Open();
using (SQLiteCommand rk = new SQLiteCommand("PRAGMA rekey='" + Program.DBPASS + "'", oldCon))
{
rk.ExecuteNonQuery();
}
oldCon.Close();
}
Class34.string_Database = "Server=localhost; Data Source = " + text2 + "; Version = 3; Password= " + Program.DBPASS + "; Compress=True;";
using (SQLiteConnection testCon = new SQLiteConnection(Class34.string_Database))
{
testCon.Open();
ok = true;
}
}
catch
{
ok = false;
}
}

if (ok)
{
File.WriteAllText("IRLOG//pwd", Program.DBPASS);
}
else
{
try
{
if (File.Exists("Database.db"))
{
File.Move("Database.db", "Database_old_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".db");
}
if (File.Exists("IRLOG//pwd"))
{
File.Delete("IRLOG//pwd");
}
}
catch
{
}
}

if (!File.Exists("IRLOG//repair.flag"))
{
try
{
Directory.CreateDirectory("IRLOG");
File.WriteAllText("IRLOG//repair.flag", "1");
}
catch
{
}
try
{
Process.Start(Application.ExecutablePath);
}
catch
{
}
Environment.Exit(0);
}
}
}

public static void EnsureAllTables(SQLiteConnection con)
{
string[] array = new string[14];
array[0] = "CREATE TABLE TBL_TKT(RID integer PRIMARY KEY AUTOINCREMENT,TKT_NAME varchar(20),SRC_STN varchar(30),DEST_STN varchar(30),BRD_STN varchar(30),TRAIN_NO varchar(20),TKT_DAY varchar(10),TKT_MONTH varchar(10),TKT_YEAR varchar(10),TKT_CLASS varchar(10),TKT_QUOTA varchar(10),TKT_TRNTYPE varchar(10),TKT_TRDAY varchar(10),TKT_ISADV varchar(10),TKT_ISCONFIRM varchar(10),TKT_ISAUTOUPGRD varchar(10),TKT_ISBEDROLL varchar(10),TKT_COACH varchar(10),TKT_MOBILE varchar(10),TKT_IRCTCID varchar(20),TKT_PWD varchar(20),TKT_SLOT varchar(20),TKT_PAYMENT varchar(50),TKT_BANKSAVEAS varchar(50),TKT_BANKDETAILS varchar(80),TKT_FAREDETAILS varchar(80),TR_LIST varchar(10000),Remarks varchar(50),TKT_ISRET varchar(10),TKT_ISPRI varchar(10),RSRC_STN varchar(30),RDEST_STN varchar(30),RBRD_STN varchar(30),RTRAIN_NO varchar(20),RTKT_DAY varchar(10),RTKT_MONTH varchar(10),RTKT_YEAR varchar(10),RTKT_CLASS varchar(10),RTKT_QUOTA varchar(10),RTKT_TRNTYPE varchar(10),RTKT_TRDAY varchar(10),RTR_LIST varchar(10000),TKT_STOPPAY varchar(10),WebLog varchar(30),AppLog varchar(30),Pnr varchar(15),PairId varchar(15),PairNo varchar(15),Create_Date DATETIME);";
array[1] = "CREATE TABLE TBL_PAX(RID integer PRIMARY KEY AUTOINCREMENT,PAX_NAME varchar(20),PAX_AGE varchar(5),PAX_SEX varchar(10),PAX_BIRTH varchar(20),PAX_FOOD varchar(20),PAX_IDTYPE varchar(30),PAX_IDNO varchar(40),PAX_CONC varchar(20),TKT_ID varchar(20),Remarks varchar(50),Create_Date varchar(50));";
array[2] = "CREATE TABLE TBL_USER(RID integer PRIMARY KEY AUTOINCREMENT,UserName varchar(50) NOT NULL,strPwd varchar(50) NOT NULL,UserState varchar(50),History varchar(1000),Remarks varchar(5000));";
array[3] = "CREATE TABLE TBL_BANK(RID integer PRIMARY KEY AUTOINCREMENT,BANK_NAME varchar(50),BANK_SAVEAS varchar(50),BANK_DETAILS varchar(50),BANK_ISDEBIT varchar(20),Remarks varchar(2000));";
array[4] = "CREATE TABLE TBL_HISTORY(RID integer PRIMARY KEY AUTOINCREMENT,TCNTRL_NAME varchar(20) NOT NULL,TKT_NAME varchar(50) NOT NULL,IRCTC_ID varchar(20),stnFrom varchar(20),stnTo varchar(20),Journey_Date varchar(20),BANK_NAME varchar(16),PNR varchar(12),AMT varchar(10),Remarks varchar(50),Create_Date varchar(50));";
array[5] = "CREATE TABLE TBL_STN(RID integer PRIMARY KEY AUTOINCREMENT,STNCD varchar(50) NOT NULL,STNNAME varchar(50) NOT NULL);";
array[6] = "CREATE TABLE tbl_RegUser(UserId varchar(30) PRIMARY KEY NOT NULL,strPwd varchar(50) NOT NULL,strMachine varchar(50) NOT NULL,strMachinePwd varchar(50),OpenedWindo varchar(5),NgetWindo varchar(5),AppWindo varchar(5));";
array[7] = "CREATE TABLE TBL_IP(IP varchar(50),Pnr1 varchar(50),Pnr2 varchar(50),Remarks varchar(50));";
array[8] = "CREATE TABLE TBL_LOG(RID integer PRIMARY KEY AUTOINCREMENT,TYPE varchar(10),LOG varchar(1000000),Remarks varchar(50));";
array[9] = "CREATE TABLE tbl_Train(TrainNo varchar(10),Traintype varchar(20));";
array[10] = "CREATE TABLE TBL_UPI(RID integer PRIMARY KEY AUTOINCREMENT,MobNo varchar(15),UPIAdd varchar(15),RegiInfo varchar(50000),LoginInfo varchar(100000),AccInfo varchar(100000),TrnsInfo varchar(100000),SessInfo varchar(100000),TimeStamp varchar(50));";
array[11] = "CREATE TABLE TBL_NPCI(RID integer PRIMARY KEY AUTOINCREMENT,MobNo varchar(15),UPIAdd varchar(15),RegiInfo varchar(50000),LoginInfo varchar(100000),AccInfo varchar(100000),TrnsInfo varchar(100000),SessInfo varchar(100000),TimeStamp varchar(50));";
array[12] = "CREATE TABLE TBL_SBIUPI(RID integer PRIMARY KEY AUTOINCREMENT,MobNo varchar(15),UPIAdd varchar(15),RegiInfo varchar(50000),LoginInfo varchar(100000),AccInfo varchar(100000),TrnsInfo varchar(100000),SessInfo varchar(100000),TimeStamp varchar(50));";
array[13] = "CREATE TABLE TBL_FREECHARGE(RID integer PRIMARY KEY AUTOINCREMENT,MobNo varchar(15),UPIAdd varchar(15),RegiInfo varchar(50000),LoginInfo varchar(100000),AccInfo varchar(100000),TrnsInfo varchar(100000),SessInfo varchar(100000),TimeStamp varchar(50));";

con.Open();
using (SQLiteCommand sQLiteCommand = new SQLiteCommand(con))
{
foreach (string text15 in array)
{
string value = text15.Split(new string[] { "CREATE TABLE" }, StringSplitOptions.None)[1].Trim().Split('(')[0].Trim();
sQLiteCommand.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name=@name";
sQLiteCommand.Parameters.Clear();
sQLiteCommand.Parameters.AddWithValue("@name", value);
if (sQLiteCommand.ExecuteScalar() == null)
{
sQLiteCommand.CommandText = text15;
sQLiteCommand.Parameters.Clear();
sQLiteCommand.ExecuteNonQuery();
}
}
}
}
}