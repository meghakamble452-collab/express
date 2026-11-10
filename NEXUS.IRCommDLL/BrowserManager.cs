using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SeleniumUndetectedChromeDriver;

namespace NEXUS.IRCommDLL;

internal class BrowserManager
{
	public class CDPAttach
	{
		public static IWebDriver AttachToChrome(int port)
		{
			ChromeOptions chromeOptions = new ChromeOptions();
			string format = "127.0.0.1:{0}";
			chromeOptions.DebuggerAddress = string.Format(format, port);
			return new ChromeDriver(chromeOptions);
		}
	}

	private UndetectedChromeDriver driver;

	private string currentProfilePath;

	private int debugPort;

	private static readonly object BROWSER_LOCK = new object();

	private int GetFreePort()
	{
		try
		{
			TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 0);
			tcpListener.Start();
			int port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
			tcpListener.Stop();
			return port;
		}
		catch
		{
			return 9223;
		}
	}

	private void SurgicalCleanup(int port)
	{
		try
		{
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			string fileName = "cmd.exe";
			processStartInfo.FileName = fileName;
			string format = "/c for /f \"tokens=5\" %a in ('netstat -aon ^| findstr :{0}') do taskkill /F /PID %a";
			processStartInfo.Arguments = string.Format(format, port);
			processStartInfo.CreateNoWindow = true;
			processStartInfo.UseShellExecute = false;
			Process.Start(processStartInfo)?.WaitForExit();
		}
		catch
		{
		}
	}

	public IWebDriver AttachToChrome(int port)
	{
		ChromeOptions chromeOptions = new ChromeOptions();
		string format = "127.0.0.1:{0}";
		chromeOptions.DebuggerAddress = string.Format(format, port);
		IWebDriver webDriver = new ChromeDriver(chromeOptions);
		try
		{
			using WebClient webClient = new WebClient();
			string format2 = "http://127.0.0.1:{0}/json";
			webClient.DownloadString(string.Format(format2, port));
		}
		catch
		{
		}
		driver = (UndetectedChromeDriver)webDriver;
		return webDriver;
	}

	public bool LaunchBrowser(bool headless = false, bool useBrave = false)
	{
		lock (BROWSER_LOCK)
		{
			AttachToChrome(9222);
			driver.GetDevToolsSession();
			UndetectedChromeDriver undetectedChromeDriver = driver;
			string commandName = "Network.enable";
			undetectedChromeDriver.ExecuteCdpCommand(commandName, new Dictionary<string, object>());
			debugPort = GetFreePort();
			SurgicalCleanup(debugPort);
			string currentDirectory = Directory.GetCurrentDirectory();
			string path = "start";
			string text = Path.Combine(currentDirectory, path);
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string text2 = "session_";
			Guid guid = Guid.NewGuid();
			string text3 = "N";
			string text4 = text2 + guid.ToString(text3).Substring(0, 6);
			currentProfilePath = Path.Combine(text, text4);
			string format = "[Browser] Launching: {0} on Port {1}";
			Console.WriteLine(string.Format(format, text4, debugPort));
			ChromeOptions chromeOptions = new ChromeOptions();
			string text5 = "--user-data-dir=";
			chromeOptions.AddArgument(text5 + currentProfilePath);
			string format2 = "--remote-debugging-port={0}";
			chromeOptions.AddArgument(string.Format(format2, debugPort));
			string argument = "--no-first-run";
			chromeOptions.AddArgument(argument);
			string argument2 = "--no-sandbox";
			chromeOptions.AddArgument(argument2);
			string argument3 = "--disable-dev-shm-usage";
			chromeOptions.AddArgument(argument3);
			string argument4 = "--disable-web-security";
			chromeOptions.AddArgument(argument4);
			string argument5 = "--window-size=350,520";
			chromeOptions.AddArgument(argument5);
			string argument6 = "--password-store=basic";
			chromeOptions.AddArgument(argument6);
			if (headless)
			{
				string argument7 = "--headless=new";
				chromeOptions.AddArgument(argument7);
			}
			if (useBrave)
			{
				string text6 = "C:\\Program Files\\BraveSoftware\\Brave-Browser\\Application\\brave.exe";
				string text7 = text6;
				if (File.Exists(text7))
				{
					chromeOptions.BinaryLocation = text7;
				}
			}
			try
			{
				string startupPath = Application.StartupPath;
				string path2 = "chromedriver.exe";
				string driverExecutablePath = Path.Combine(startupPath, path2);
				driver = UndetectedChromeDriver.Create(chromeOptions, null, driverExecutablePath, null, 0, 0, headless: false, noSandbox: true, suppressWelcome: true, hideCommandPromptWindow: false, null, null, delegate(ChromeDriverService service)
				{
					service.HideCommandPromptWindow = true;
					service.SuppressInitialDiagnosticInformation = true;
				});
				int num = debugPort % 10 * 20;
				driver.Manage().Window.Position = new Point(100 + num, 50 + num);
				driver.Manage().Window.Size = new Size(450, 720);
				string format3 = "✅ Browser Ready | Port: {0}";
				Console.WriteLine(string.Format(format3, debugPort));
				OpenIRCTC();
				return true;
			}
			catch (Exception ex)
			{
				string text8 = "[Error] Launch Failed: ";
				Console.WriteLine(text8 + ex.Message);
				return false;
			}
		}
	}

	public bool OpenIRCTC()
	{
		try
		{
			INavigation navigation = driver.Navigate();
			string url = "https://www.irctc.co.in/nget/train-search";
			navigation.GoToUrl(url);
			return true;
		}
		catch
		{
			return false;
		}
	}

	public void Close()
	{
		try
		{
			driver?.Quit();
			string value = ">>> \ud83d\uded1 Browser Closed";
			Console.WriteLine(value);
			if (!string.IsNullOrEmpty(currentProfilePath) && Directory.Exists(currentProfilePath))
			{
				Thread.Sleep(1000);
				Directory.Delete(currentProfilePath, recursive: true);
				string text = ">>> \ud83e\uddf9 Temp Profile Deleted: ";
				Console.WriteLine(text + currentProfilePath);
			}
		}
		catch (Exception ex)
		{
			string text2 = ">>> ⚠\ufe0f Close Error: ";
			Console.WriteLine(text2 + ex.Message);
		}
	}
}
