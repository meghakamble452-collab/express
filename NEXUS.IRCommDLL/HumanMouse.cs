using Microsoft.Playwright;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumUndetectedChromeDriver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace NEXUS.IRCommDLL;

internal class HumanMouse
{
	public static class WinKeys
	{
		public struct INPUT
		{
			public uint type;

			public InputUnion u;
		}

		[StructLayout(LayoutKind.Explicit)]
		public struct InputUnion
		{
			[FieldOffset(0)]
			public KEYBDINPUT ki;
		}

		public struct KEYBDINPUT
		{
			public ushort wVk;

			public ushort wScan;

			public uint dwFlags;

			public uint time;

			public IntPtr dwExtraInfo;
		}

		public const uint INPUT_KEYBOARD = 1u;

		public const uint KEYEVENTF_KEYUP = 2u;

		public const uint KEYEVENTF_UNICODE = 4u;

		[DllImport("user32.dll")]
		public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);
	}

	public static class WinMouse
	{
		private const uint LEFTDOWN = 2u;

		private const uint LEFTUP = 4u;

		[DllImport("user32.dll")]
		public static extern bool SetCursorPos(int x, int y);

		[DllImport("user32.dll")]
		private static extern void mouse_event(uint flags, uint dx, uint dy, uint data, UIntPtr extraInfo);

		public static void LeftClick(int x, int y)
		{
			SetCursorPos(x, y);
			Thread.Sleep(20);
			mouse_event(2u, (uint)x, (uint)y, 0u, UIntPtr.Zero);
			Thread.Sleep(30);
			mouse_event(4u, (uint)x, (uint)y, 0u, UIntPtr.Zero);
		}
	}

	private struct RECT
	{
		private int Left;

		private int Top;

		private int Right;

		private int Bottom;
	}

	public class MouseConfig
	{
		public int MoveSpeed = 350;

		public int Jitter = 1;

		public int ShakeRadius = 2;

		public int ShakeSteps = 3;

		public int MinClickDelay = 60;

		public int MaxClickDelay = 150;

		public int MinBetweenClick = 40;

		public int MaxBetweenClick = 120;

		public int DragSpeed = 600;

		public bool UseBezier = true;

		public int BezierMaxOffset = 80;
	}

	private struct POINT
	{
		public int X;

		public int Y;
	}

	private const int MOUSEEVENTF_WHEEL = 2048;

	private const int KEYEVENTF_KEYDOWN = 0;

	private const int KEYEVENTF_KEYUP = 2;

	private MouseConfig Config = new MouseConfig();

	private const int LEFTDOWN = 2;

	private const int LEFTUP = 4;

	private const int RIGHTDOWN = 8;

	private const int RIGHTUP = 16;

	private static readonly Random rnd = new Random();

	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

	[DllImport("user32.dll", CharSet = CharSet.Auto)]
	private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

	[DllImport("user32.dll")]
	private static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

	[DllImport("user32.dll")]
	private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

	public static void TypeUnicode(string text)
	{
		foreach (char wScan in text)
		{
			WinKeys.INPUT[] array = new WinKeys.INPUT[2];
			array[0].type = 1u;
			array[0].u.ki.wVk = 0;
			array[0].u.ki.wScan = wScan;
			array[0].u.ki.dwFlags = 4u;
			array[0].u.ki.time = 0u;
			array[0].u.ki.dwExtraInfo = IntPtr.Zero;
			array[1].type = 1u;
			array[1].u.ki.wVk = 0;
			array[1].u.ki.wScan = wScan;
			array[1].u.ki.dwFlags = 6u;
			array[1].u.ki.time = 0u;
			array[1].u.ki.dwExtraInfo = IntPtr.Zero;
			WinKeys.SendInput(2u, array, Marshal.SizeOf(typeof(WinKeys.INPUT)));
			Thread.Sleep(40);
		}
	}

	public static void PressTab()
	{
		WinKeys.INPUT[] array = new WinKeys.INPUT[2];
		array[0].type = 1u;
		array[0].u.ki.wVk = 9;
		array[1].type = 1u;
		array[1].u.ki.wVk = 9;
		array[1].u.ki.dwFlags = 2u;
		WinKeys.SendInput(2u, array, Marshal.SizeOf(typeof(WinKeys.INPUT)));
	}

	internal void OriginAutoCompleteTypeAndSelect(IWebDriver driver, string text, string field)
	{
		IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
		IWebElement webElement = driver.FindElement(By.CssSelector(field));
		string script = "\r\n        var el = arguments[0];\r\n        var r = el.getBoundingClientRect();\r\n        var sx = window.screenX + r.left + r.width/2;\r\n        var sy = window.screenY + (window.outerHeight - window.innerHeight) + r.top + r.height/2;\r\n        return [sx, sy];\r\n    ";
		ReadOnlyCollection<object> readOnlyCollection = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script, webElement);
		int x = Convert.ToInt32(readOnlyCollection[0]);
		int y = Convert.ToInt32(readOnlyCollection[1]);
		WinMouse.LeftClick(x, y);
		Thread.Sleep(150);
		TypeUnicode(text);
		Thread.Sleep(800);
		PressTab();
		Thread.Sleep(150);
	}

	internal void FillCaptchaWinApi(IWebDriver driver, string captchaText)
	{
		IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
		string cssSelectorToFind = "input[formcontrolname='captcha']";
		IWebElement webElement = driver.FindElement(By.CssSelector(cssSelectorToFind));
		string script = "\r\n        var r = arguments[0].getBoundingClientRect();\r\n        var sx = window.screenX + r.left + r.width/2;\r\n        var sy = window.screenY + (window.outerHeight - window.innerHeight) + r.top + r.height/2;\r\n        return [sx, sy];\r\n    ";
		ReadOnlyCollection<object> readOnlyCollection = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script, webElement);
		int x = Convert.ToInt32(readOnlyCollection[0]);
		int y = Convert.ToInt32(readOnlyCollection[1]);
		WinMouse.LeftClick(x, y);
		Thread.Sleep(150);
		TypeUnicode(captchaText);
		PressEnter();
	}

	public static void PressEnter()
	{
		WinKeys.INPUT[] array = new WinKeys.INPUT[2];
		array[0].type = 1u;
		array[0].u.ki.wVk = 13;
		array[0].u.ki.wScan = 0;
		array[0].u.ki.dwFlags = 0u;
		array[0].u.ki.time = 0u;
		array[0].u.ki.dwExtraInfo = IntPtr.Zero;
		array[1].type = 1u;
		array[1].u.ki.wVk = 13;
		array[1].u.ki.wScan = 0;
		array[1].u.ki.dwFlags = 2u;
		array[1].u.ki.time = 0u;
		array[1].u.ki.dwExtraInfo = IntPtr.Zero;
		WinKeys.SendInput(2u, array, Marshal.SizeOf(typeof(WinKeys.INPUT)));
	}

	private void KeyDown(byte key)
	{
		keybd_event(key, 0, 0u, UIntPtr.Zero);
	}

	private void KeyUp(byte key)
	{
		keybd_event(key, 0, 2u, UIntPtr.Zero);
	}

	private void CtrlV()
	{
		KeyDown(17);
		Thread.Sleep(30);
		KeyDown(86);
		Thread.Sleep(50);
		KeyUp(86);
		Thread.Sleep(20);
		KeyUp(17);
	}

	private void ScrollWheel(int amount)
	{
		mouse_event(2048, 0, 0, amount, 0);
	}

	internal void MoveCursorThenSlowScrollToTop(IWebDriver driver, By locator, int topMargin = 0)
	{
		IWebElement webElement = driver.FindElement(locator);
		IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
		string script = "\r\n        var el = arguments[0];\r\n        var r  = el.getBoundingClientRect();\r\n\r\n        var sx = window.screenX + r.left + (r.width / 2);\r\n        var sy = window.screenY \r\n                 + (window.outerHeight - window.innerHeight) \r\n                 + r.top + (r.height / 2);\r\n        return [sx, sy];\r\n    ";
		ReadOnlyCollection<object> readOnlyCollection = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script, webElement);
		int targetX = Convert.ToInt32(readOnlyCollection[0]);
		int targetY = Convert.ToInt32(readOnlyCollection[1]);
		MoveSmooth(targetX, targetY);
		Shake();
		Thread.Sleep(120);
		string script2 = "\r\n        var r = arguments[0].getBoundingClientRect();\r\n        return [r.top];\r\n    ";
		double num = Convert.ToDouble(((ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script2, webElement))[0]);
		while (num > (double)(topMargin + 5))
		{
			int num2 = new Random().Next(20, 35);
			string format = "window.scrollBy(0, {0});";
			javaScriptExecutor.ExecuteScript(string.Format(format, num2));
			Thread.Sleep(30);
			string script3 = "\r\n            var r = arguments[0].getBoundingClientRect();\r\n            return [r.top];\r\n        ";
			num = Convert.ToDouble(((ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script3, webElement))[0]);
		}
	}

	internal void MoveMouseToElementAndScrollToEnd(IWebDriver driver, By locator)
	{
		IWebElement webElement = driver.FindElement(locator);
		IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
		string script = "\r\n        var el = arguments[0];\r\n        var r  = el.getBoundingClientRect();\r\n        return [\r\n            window.screenX + r.left + (r.width / 2),\r\n            window.screenY + (window.outerHeight - window.innerHeight) + r.top + (r.height / 2)\r\n        ];\r\n    ";
		ReadOnlyCollection<object> readOnlyCollection = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script, webElement);
		int targetX = Convert.ToInt32(readOnlyCollection[0]);
		int targetY = Convert.ToInt32(readOnlyCollection[1]);
		MoveSmooth(targetX, targetY);
		Shake();
		for (int i = 0; i < 15; i++)
		{
			ScrollWheel(-180);
			Thread.Sleep(80);
		}
	}

	internal void ClickAndPaste(IWebDriver driver, string textToPaste, By locator)
	{
		IWebElement webElement = driver.FindElement(locator);
		IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
		string script = "\r\n        var el = arguments[0];\r\n        var r  = el.getBoundingClientRect();\r\n        return [\r\n            window.screenX + r.left + (r.width / 2),\r\n            window.screenY + (window.outerHeight - window.innerHeight) + r.top + (r.height / 2)\r\n        ];\r\n    ";
		ReadOnlyCollection<object> readOnlyCollection = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script, webElement);
		int num = Convert.ToInt32(readOnlyCollection[0]);
		int num2 = Convert.ToInt32(readOnlyCollection[1]);
		MoveSmooth(num, num2);
		LeftClick2(num, num2);
		Thread.Sleep(150);
		Clipboard.SetText(textToPaste);
		CtrlV();
	}

	internal void ClickElementHumanRandom2(IWebDriver driver, By locator)
	{
		Config.MoveSpeed = 1300;
		Config.BezierMaxOffset = 60;
		Config.Jitter = 1;
		Config.ShakeRadius = 1;
		Config.DragSpeed = 1700;
		Config.UseBezier = true;
		IWebElement webElement = driver.FindElement(locator);
		IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
		string script = "\r\n        var el = arguments[0];\r\n        var r = el.getBoundingClientRect();\r\n        return [r.left, r.top, r.width, r.height];\r\n    ";
		ReadOnlyCollection<object> readOnlyCollection = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script, webElement);
		Convert.ToDouble(readOnlyCollection[0]);
		Convert.ToDouble(readOnlyCollection[1]);
		double num = Convert.ToDouble(readOnlyCollection[2]);
		double num2 = Convert.ToDouble(readOnlyCollection[3]);
		Random random = new Random();
		int num3 = random.Next((int)(num * 0.2), (int)(num * 0.8));
		int num4 = random.Next((int)(num2 * 0.2), (int)(num2 * 0.8));
		string script2 = "\r\n        var el = arguments[0];\r\n        var r = el.getBoundingClientRect();\r\n        var sx = window.screenX + r.left;\r\n        var sy = window.screenY + (window.outerHeight - window.innerHeight) + r.top;\r\n        return [sx, sy];\r\n    ";
		ReadOnlyCollection<object> readOnlyCollection2 = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script2, webElement);
		int num5 = Convert.ToInt32(readOnlyCollection2[0]);
		int num6 = Convert.ToInt32(readOnlyCollection2[1]);
		int num7 = num5 + num3;
		int num8 = num6 + num4;
		MoveSmooth(num7, num8 - 50);
		LeftClick(num7, num8 - 50);
	}

	internal void ClickElementHumanRandom(IWebDriver driver, By locator)
	{
		Config.MoveSpeed = 550;
		Config.BezierMaxOffset = 120;
		Config.Jitter = 2;
		Config.ShakeRadius = 3;
		Config.DragSpeed = 900;
		Config.UseBezier = true;
		IWebElement webElement = driver.FindElement(locator);
		IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
		string script = "\r\n        var el = arguments[0];\r\n        var r = el.getBoundingClientRect();\r\n        return [r.left, r.top, r.width, r.height];\r\n    ";
		ReadOnlyCollection<object> readOnlyCollection = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script, webElement);
		Convert.ToDouble(readOnlyCollection[0]);
		Convert.ToDouble(readOnlyCollection[1]);
		double num = Convert.ToDouble(readOnlyCollection[2]);
		double num2 = Convert.ToDouble(readOnlyCollection[3]);
		Random random = new Random();
		int num3 = random.Next((int)(num * 0.2), (int)(num * 0.8));
		int num4 = random.Next((int)(num2 * 0.2), (int)(num2 * 0.8));
		string script2 = "\r\n        var el = arguments[0];\r\n        var r = el.getBoundingClientRect();\r\n        var sx = window.screenX + r.left;\r\n        var sy = window.screenY + (window.outerHeight - window.innerHeight) + r.top;\r\n        return [sx, sy];\r\n    ";
		ReadOnlyCollection<object> readOnlyCollection2 = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script2, webElement);
		int num5 = Convert.ToInt32(readOnlyCollection2[0]);
		int num6 = Convert.ToInt32(readOnlyCollection2[1]);
		int num7 = num5 + num3;
		int num8 = num6 + num4;
		LeftClick(num7 + 5, num8 - 5);
	}

	private void Fast_LeftClick(int x, int y)
	{
		Thread.Sleep(rnd.Next(50, 10));
		mouse_event(2, x, y, 0, 0);
		Thread.Sleep(rnd.Next(50, 10));
		mouse_event(4, x, y, 0, 0);
	}

	internal void MoveSmoothFast(int x, int y)
	{
		int num = Cursor.Position.X;
		int num2 = Cursor.Position.Y;
		int num3 = 12;
		double num4 = (double)(x - num) / 12.0;
		double num5 = (double)(y - num2) / 12.0;
		for (int i = 0; i < num3; i++)
		{
			num = (int)((double)num + num4);
			num2 = (int)((double)num2 + num5);
			Cursor.Position = new Point(num, num2);
			Thread.Sleep(1);
		}
		Cursor.Position = new Point(x, y);
	}
    internal void ClickElementHuman_Play_brave(IPage page, string profileKeyword, By locator)
    {
        string selector = locator.ToString().Replace("By.CssSelector: ", "").Trim();
        var element = page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
        if (element == null)
        {
            Console.WriteLine("Element not found: " + selector);
            return;
        }
        element.ScrollIntoViewIfNeededAsync().GetAwaiter().GetResult();
        var box = element.BoundingBoxAsync().GetAwaiter().GetResult();
        if (box == null)
        {
            Console.WriteLine("Bounding box not found");
            return;
        }
        Random rnd = new Random();
        double targetX = box.X + box.Width / 2;
        double targetY = box.Y + box.Height / 2;
        targetX += rnd.Next(-3, 3);
        targetY += rnd.Next(-3, 3);
        LeftClick((int)targetX, (int)targetY);
        //page.Mouse.MoveAsync((float)targetX, (float)targetY).GetAwaiter().GetResult();
        //Thread.Sleep(rnd.Next(80, 180));
        //page.Mouse.ClickAsync((float)targetX, (float)targetY).GetAwaiter().GetResult();
    }
    internal void ClickElementHuman_Play(IPage page, string profileKeyword, By locator)
    {
        string text = locator.ToString();
        string oldValue = "By.CssSelector: ";
        string selector = text.Replace(oldValue, "");
        Config.MoveSpeed = 550;
        Config.BezierMaxOffset = 120;
        Config.Jitter = 2;
        Config.ShakeRadius = 3;
        Config.DragSpeed = 900;
        Config.UseBezier = true;
        IElementHandle result = page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
        string expression = "\r\n        (el) => {\r\n            var r = el.getBoundingClientRect();\r\n\r\n            var sx = window.screenX + r.left + (r.width / 2);\r\n\r\n            var sy = window.screenY \r\n                    + (window.outerHeight - window.innerHeight)\r\n                    + r.top + (r.height / 2);\r\n\r\n            return [sx, sy];\r\n        }\r\n    ";
        double[] result2 = page.EvaluateAsync<double[]>(expression, result).GetAwaiter().GetResult();
        int num = (int)Math.Round(result2[0]);
        int num2 = (int)Math.Round(result2[1]);
        LeftClick(num + 10, num2 - 5);
    }

    internal void ClickElementHuman_Play_opera(IPage page, string profileKeyword, By locator)
	{
		string text = locator.ToString();
		string oldValue = "By.CssSelector: ";
		string selector = text.Replace(oldValue, "");
		Config.MoveSpeed = 550;
		Config.BezierMaxOffset = 120;
		Config.Jitter = 2;
		Config.ShakeRadius = 3;
		Config.DragSpeed = 900;
		Config.UseBezier = true;
		IElementHandle result = page.QuerySelectorAsync(selector).GetAwaiter().GetResult();
		string expression = "\r\n        (el) => {\r\n            var r = el.getBoundingClientRect();\r\n\r\n            var sx = window.screenX + r.left + (r.width / 2);\r\n\r\n            var sy = window.screenY \r\n                    + (window.outerHeight - window.innerHeight)\r\n                    + r.top + (r.height / 2);\r\n\r\n            return [sx, sy];\r\n        }\r\n    ";
		double[] result2 = page.EvaluateAsync<double[]>(expression, result).GetAwaiter().GetResult();
		int num = (int)Math.Round(result2[0]);
		int num2 = (int)Math.Round(result2[1]);
		LeftClick(num + 60, num2 - 5);
	}

	internal void ClickElementHuman_old(IWebDriver driver, string profileKeyword, By locator)
	{
		Config.MoveSpeed = 550;
		Config.BezierMaxOffset = 120;
		Config.Jitter = 2;
		Config.ShakeRadius = 3;
		Config.DragSpeed = 900;
		Config.UseBezier = true;
		IWebElement webElement = driver.FindElement(locator);
		IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
		string script = "\r\n            var el   = arguments[0];\r\n            var r    = el.getBoundingClientRect();\r\n            var sx   = window.screenX + r.left + (r.width  / 2);\r\n            var sy   = window.screenY \r\n                       + (window.outerHeight - window.innerHeight) \r\n                       + r.top + (r.height / 2);\r\n            return [sx, sy];\r\n        ";
		ReadOnlyCollection<object> readOnlyCollection = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script, webElement);
		int num = (int)Math.Round(Convert.ToDouble(readOnlyCollection[0]));
		int num2 = (int)Math.Round(Convert.ToDouble(readOnlyCollection[1]));
		LeftClick(num + 10, num2 - 5);
	}
    internal void HumanMoveToPosition(int startX, int startY, int endX, int endY)
    {
        Random rnd = new Random();
        int steps = rnd.Next(25, 45);
        double t;
        int cp1X = startX + rnd.Next(-100, 100);
        int cp1Y = startY + rnd.Next(-100, 100);
        int cp2X = endX + rnd.Next(-100, 100);
        int cp2Y = endY + rnd.Next(-100, 100);
        for (int i = 0; i <= steps; i++)
        {
            t = (double)i / steps;
            double x = Math.Pow(1 - t, 3) * startX +
                       3 * Math.Pow(1 - t, 2) * t * cp1X +
                       3 * (1 - t) * Math.Pow(t, 2) * cp2X +
                       Math.Pow(t, 3) * endX;

            double y = Math.Pow(1 - t, 3) * startY +
                       3 * Math.Pow(1 - t, 2) * t * cp1Y +
                       3 * (1 - t) * Math.Pow(t, 2) * cp2Y +
                       Math.Pow(t, 3) * endY;
            int jitterX = rnd.Next(-2, 3);
            int jitterY = rnd.Next(-2, 3);
            SetCursorPos((int)x + jitterX, (int)y + jitterY);
            Thread.Sleep(rnd.Next(5, 15));
        }
        SetCursorPos(endX, endY);
    }
    internal void ClickElementHuman(IWebDriver driver, string profileKeyword, By locator)
    {
        int maxRetries = 3;
        Random rnd = new Random();

        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                IWebElement element = driver.FindElement(locator);
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

                // Scroll to center
                js.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", element);
                Thread.Sleep(rnd.Next(300, 600));

                // Get coordinates
                var rect = (ReadOnlyCollection<object>)js.ExecuteScript(@"
                var r = arguments[0].getBoundingClientRect();
                return [r.left + r.width/2, r.top + r.height/2];
            ", element);

                int x = (int)Math.Round(Convert.ToDouble(rect[0]));
                int y = (int)Math.Round(Convert.ToDouble(rect[1]));

                // Convert to screen position
                var screen = (ReadOnlyCollection<object>)js.ExecuteScript(@"
                return [window.screenX, window.screenY, window.outerHeight - window.innerHeight];
            ");

                int screenX = (int)Math.Round(Convert.ToDouble(screen[0]));
                int screenY = (int)Math.Round(Convert.ToDouble(screen[1]));
                int panelHeight = (int)Math.Round(Convert.ToDouble(screen[2]));

                int finalX = screenX + x + rnd.Next(-5, 8);
                int finalY = screenY + panelHeight + y + rnd.Next(-5, 8);

                // Current cursor
                GetCursorPos(out var start);

                // Move mouse
                HumanMoveToPosition(start.X, start.Y, finalX, finalY);

                Thread.Sleep(rnd.Next(100, 250));

                // Click
                LeftClick(finalX, finalY);

                Thread.Sleep(rnd.Next(400, 700));

                // ✅ Verify click (focus / active element check)
                bool isClicked = (bool)js.ExecuteScript(@"
                return document.activeElement === arguments[0];
            ", element);

                if (isClicked)
                    return; // success → exit method

            }
            catch (Exception)
            {
                // ignore and retry
            }

            // Retry delay (human hesitation)
            Thread.Sleep(rnd.Next(500, 1000));
        }

        // ❌ Final fallback (JS click)
        try
        {
            IWebElement element = driver.FindElement(locator);
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", element);
        }
        catch { }
    }
    internal string ClassCodeToText(string code)
    {
        code = code.ToUpperInvariant().Trim();
        string result = "";
        switch (code)
        {
            case "2S":
                result = "Second Sitting (2S)";
                break;
            case "CC":
                result = "AC Chair Car (CC)";
                break;
            case "2A":
                result = "AC 2 Tier (2A)";
                break;
            case "3A":
                result = "AC 3 Tier (3A)";
                break;
            case "SL":
                result = "Sleeper (SL)";
                break;
        }
        return result;
    }
    internal void HumanClickButtonFast(IWebDriver driver, By locator, int timeoutSeconds = 20)
    {
        Random random = new Random();
        WebDriverWait webDriverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
        IWebElement webElement2 = webDriverWait.Until(delegate (IWebDriver drv)
        {
            try
            {
                IWebElement webElement = drv.FindElement(locator);
                return (webElement.Displayed && webElement.Enabled) ? webElement : null;
            }
            catch
            {
                return (IWebElement?)null;
            }
        });
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({behavior:'smooth', block:'center'});", webElement2);
        Thread.Sleep(random.Next(20, 100));
        Actions actions = new Actions(driver);
        actions.MoveToElement(webElement2, random.Next(3, 10), random.Next(3, 10)).Pause(TimeSpan.FromMilliseconds(random.Next(20, 100))).Click()
            .Pause(TimeSpan.FromMilliseconds(random.Next(10, 150)))
            .Perform();
    }

    internal void HumanClickButton(IWebDriver driver, By locator, int timeoutSeconds = 20)
    {
        Random random = new Random();
        WebDriverWait webDriverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
        IWebElement webElement2 = webDriverWait.Until(delegate (IWebDriver drv)
        {
            try
            {
                IWebElement webElement = drv.FindElement(locator);
                return (webElement.Displayed && webElement.Enabled) ? webElement : null;
            }
            catch
            {
                return (IWebElement?)null;
            }
        });
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({behavior:'smooth', block:'center'});", webElement2);
        Thread.Sleep(random.Next(400, 800));
        Actions actions = new Actions(driver);
        actions.MoveToElement(webElement2, random.Next(3, 10), random.Next(3, 10)).Pause(TimeSpan.FromMilliseconds(random.Next(200, 500))).Click()
            .Pause(TimeSpan.FromMilliseconds(random.Next(150, 350)))
            .Perform();
    }
    internal string FormatIrctcTileLabel(DateTime dt)
    {
        return dt.ToString("ddd, dd MMM", CultureInfo.InvariantCulture);
    }
    private bool TryClickDateTile(UndetectedChromeDriver driver, IWebElement trainBlock, string label, int maxMoves)
    {
        if (ClickVisibleTile(driver, trainBlock, label))
        {
            return true;
        }
        IWebElement webElement = trainBlock.FindElements(By.CssSelector(".fa.fa-chevron-right")).FirstOrDefault();
        IWebElement webElement2 = trainBlock.FindElements(By.CssSelector(".fa.fa-chevron-left")).FirstOrDefault();
        int num = 0;
        while (true)
        {
            if (num < maxMoves && webElement != null)
            {
                HumanClickButton(driver, By.XPath(".//app-train-avl-enq[.//*[(contains(normalize-space(),'Runs On') or contains(normalize-space(),'Train Schedule'))]]//span[contains(@class,'fa-chevron-right')]"));
                Thread.Sleep(150);
                if (ClickVisibleTile(driver, trainBlock, label))
                {
                    break;
                }
                num++;
                continue;
            }
            int num2 = 0;
            while (true)
            {
                if (num2 < maxMoves && webElement2 != null)
                {
                    HumanClickButton(driver, By.XPath(".//app-train-avl-enq[.//*[(contains(normalize-space(),'Runs On') or contains(normalize-space(),'Train Schedule'))]]//span[contains(@class,'fa-chevron-left')]"));
                    Thread.Sleep(150);
                    if (ClickVisibleTile(driver, trainBlock, label))
                    {
                        break;
                    }
                    num2++;
                    continue;
                }
                return false;
            }
            return true;
        }
        return true;
    }
    private bool ClickVisibleTile(UndetectedChromeDriver driver, IWebElement trainBlock, string label)
    {
        string text = ".//div[contains(@class,'pre-avl')]//strong[translate(normalize-space(.), '\u00a0', ' ') = '" + label + "']";
        HumanClickButton(driver, By.XPath(text + "/ancestor::div[contains(@class,'pre-avl')]"));
        return true;
    }
    internal void BookTrainClassOnDate(UndetectedChromeDriver driver, string trainNo, string classCode, DateTime journeyDate, bool bookOnly, int timeoutSec = 20)
    {
        try
        {
            WebDriverWait webDriverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSec));
            IWebElement train = webDriverWait.Until((IWebDriver d) => d.FindElements(By.CssSelector("app-train-avl-enq")).FirstOrDefault((IWebElement x) => (x.Text ?? "").Contains(trainNo)));
            if (train == null)
            {
                throw new Exception("Train " + trainNo + " not found.");
            }
            string text = ClassCodeToText(classCode);
            if (!bookOnly)
            {
                if (train.FindElements(By.XPath(".//strong[normalize-space(text())='" + text + "']")).FirstOrDefault() == null)
                {
                    throw new Exception("Class " + classCode + " not found for train " + trainNo);
                }
                HumanClickButton(driver, By.XPath(".//app-train-avl-enq[.//*[contains(normalize-space(),'" + trainNo + "')]]//strong[normalize-space(text())='" + text + "']"));
            }
            webDriverWait.Until((IWebDriver d) => train.FindElements(By.CssSelector("div[style*='overflow-x'] table")).FirstOrDefault());
            string text2 = FormatIrctcTileLabel(journeyDate);
            if (!TryClickDateTile(driver, train, text2, 12))
            {
                throw new Exception("Date tile '" + text2 + "' not found for train " + trainNo + ".");
            }
            if (webDriverWait.Until(delegate
            {
                try
                {
                    return train.FindElements(By.XPath(".//button[contains(.,'Book Now')]")).FirstOrDefault((IWebElement b) => b.Displayed && b.Enabled && !(b.GetAttribute("class") ?? "").Contains("disable-book"));
                }
                catch
                {
                    return (IWebElement?)null;
                }
            }) == null)
            {
                throw new Exception("Book Now button not enabled after selecting date.");
            }
            HumanClickButtonFast(driver, By.XPath(".//app-train-avl-enq[.//*[contains(normalize-space(),'" + trainNo + "')]]//button[contains(.,'Book Now')]"));
        }
        catch (Exception)
        {
        }
    }

    internal bool ClickElementHuman_availxpath(
        IWebDriver driver,
        string profileKeyword,
        string xpath,
        int maxRetry = 3)
    {
        Config.MoveSpeed = 550;
        Config.BezierMaxOffset = 120;
        Config.Jitter = 2;
        Config.ShakeRadius = 3;
        Config.DragSpeed = 900;
        Config.UseBezier = true;

        for (int i = 0; i < maxRetry; i++)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

                IWebElement element = wait.Until(d =>
                {
                    try
                    {
                        IWebElement e = d.FindElement(By.XPath(xpath));
                        return (e.Displayed && e.Enabled) ? e : null;
                    }
                    catch
                    {
                        return null;
                    }
                });

                if (element == null)
                {
                    Thread.Sleep(300);
                    continue;
                }

                ((IJavaScriptExecutor)driver).ExecuteScript(
                    "arguments[0].scrollIntoView({block:'center', inline:'center'});",
                    element);

                Thread.Sleep(500);

                var result =
                    (ReadOnlyCollection<object>)((IJavaScriptExecutor)driver)
                    .ExecuteScript(@"
                    var el = arguments[0];
                    var r = el.getBoundingClientRect();

                    var screenX = window.screenX + r.left + (r.width / 2);
                    var screenY = window.screenY +
                                  (window.outerHeight - window.innerHeight) +
                                  r.top + (r.height / 2);

                    return [screenX, screenY];
                ", element);

                int screenX = Convert.ToInt32(Convert.ToDouble(result[0]));
                int screenY = Convert.ToInt32(Convert.ToDouble(result[1]));

                Console.WriteLine("XPath : " + xpath);
                Console.WriteLine($"Mouse Click => X:{screenX} Y:{screenY}");

                LeftClick(screenX, screenY);

                return true;
            }
            catch (StaleElementReferenceException)
            {
                Thread.Sleep(300);
            }
            catch (NoSuchElementException)
            {
                Thread.Sleep(300);
            }
            catch (InvalidSelectorException ex)
            {
                Console.WriteLine("Invalid XPath:");
                Console.WriteLine(xpath);
                Console.WriteLine(ex.ToString());
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Thread.Sleep(500);
            }
        }

        return false;
    }
    internal void TypeLikeHuman(IWebDriver driver, By by, string text, int timeoutSeconds = 40)
    {
        Random random = new Random();
        WebDriverWait webDriverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
        IWebElement webElement = webDriverWait.Until(ExpectedConditions.ElementIsVisible(by));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'smooth', block: 'center'});", webElement);
        Thread.Sleep(random.Next(800, 1200));
        string script = "\r\n        var el = arguments[0];\r\n        var rect = el.getBoundingClientRect();\r\n        var startX = Math.random() * window.innerWidth;\r\n        var startY = Math.random() * window.innerHeight;\r\n        var endX = rect.left + (Math.random() * rect.width);\r\n        var endY = rect.top + (Math.random() * rect.height);\r\n\r\n        function moveMouse(tx, ty) {\r\n            el.dispatchEvent(new MouseEvent('mousemove', {\r\n                bubbles: true, clientX: tx, clientY: ty\r\n            }));\r\n        }\r\n\r\n        var steps = 25;\r\n        for (var i = 0; i <= steps; i++) {\r\n            var t = i / steps;\r\n            // Quadratic Bezier Formula with a bit of random 'jitter'\r\n            var x = Math.pow(1-t, 2) * startX + 2*(1-t)*t*(startX + 100) + Math.pow(t, 2) * endX + (Math.random() * 2);\r\n            var y = Math.pow(1-t, 2) * startY + 2*(1-t)*t*(startY - 100) + Math.pow(t, 2) * endY + (Math.random() * 2);\r\n            setTimeout(moveMouse, i * 10, x, y);\r\n        }\r\n        \r\n        setTimeout(() => {\r\n            el.dispatchEvent(new MouseEvent('mousedown', { bubbles: true, clientX: endX, clientY: endY }));\r\n            el.dispatchEvent(new MouseEvent('mouseup', { bubbles: true, clientX: endX, clientY: endY }));\r\n            el.click();\r\n            el.focus();\r\n        }, steps * 11);\r\n    ";
        ((IJavaScriptExecutor)driver).ExecuteScript(script, webElement);
        Thread.Sleep(random.Next(600, 1000));
        webElement.SendKeys(OpenQA.Selenium.Keys.Control + "a");
        Thread.Sleep(random.Next(150, 300));
        webElement.SendKeys(OpenQA.Selenium.Keys.Backspace);
        Thread.Sleep(random.Next(400, 700));
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (random.Next(1, 100) < 3)
            {
                webElement.SendKeys("q");
                Thread.Sleep(random.Next(100, 250));
                webElement.SendKeys(OpenQA.Selenium.Keys.Backspace);
                Thread.Sleep(random.Next(150, 300));
            }
            webElement.SendKeys(c.ToString());
            int num = random.Next(80, 250);
            if (char.IsWhiteSpace(c))
            {
                num += random.Next(100, 300);
            }
            Thread.Sleep(num);
        }
    }
    internal int GetSelectedIndex(IWebElement selectEl)
    {
        IWebElement selected = selectEl.FindElements(By.CssSelector("option:checked")).FirstOrDefault();
        if (selected == null)
        {
            return 0;
        }
        return selectEl.FindElements(By.TagName("option")).ToList().FindIndex((IWebElement o) => o.Equals(selected));
    }
    internal int IndexOfOption(IWebElement selectEl, string visibleText)
    {
        List<IWebElement> list = selectEl.FindElements(By.TagName("option")).ToList();
        int num = 0;
        while (true)
        {
            if (num < list.Count)
            {
                if ((list[num].Text ?? "").Trim().Equals(visibleText, StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                num++;
                continue;
            }
            return -1;
        }
        return num;
    }

    internal void SelectGender_Humanish2(UndetectedChromeDriver driver, int index1Based, string genderText, int timeoutSec = 8)
    {
        if (index1Based < 1)
        {
            index1Based = 1;
        }
        switch (genderText)
        {
            case "T":
                genderText = "Transgender";
                break;
            case "F":
                genderText = "Female";
                break;
            case "M":
                genderText = "Male";
                break;
        }
        WebDriverWait webDriverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSec));
        By locator = By.XPath($"(//app-passenger//select[@formcontrolname='passengerGender'])[{index1Based}]");
        IWebElement webElement = webDriverWait.Until(delegate (IWebDriver d)
        {
            ReadOnlyCollection<IWebElement> readOnlyCollection = d.FindElements(By.CssSelector("select[formcontrolname='passengerGender']"));
            return (readOnlyCollection == null || readOnlyCollection.Count < index1Based) ? null : readOnlyCollection[index1Based - 1];
        });
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'})", new object[1] { webElement });
        try
        {
            HumanClickButton(driver, locator);
        }
        catch
        {
        }
        try
        {
            new Actions(driver).MoveToElement(webElement, 2, Math.Max(2, webElement.Size.Height / 2 - 2)).Pause(TimeSpan.FromMilliseconds(120 + new Random().Next(120))).Click()
                .Perform();
        }
        catch
        {
        }
        Thread.Sleep(50 + new Random().Next(80));
        string text = (genderText ?? "").Trim().ToLower();
        int num = -1;
        if (1 == 0)
        {
        }
        int num2 = text switch
        {
            "transgender" => IndexOfOption(webElement, "Transgender"),
            "female" => IndexOfOption(webElement, "Female"),
            "male" => IndexOfOption(webElement, "Male"),
            _ => IndexOfOption(webElement, genderText),
        };
        if (1 == 0)
        {
        }
        num = num2;
        if (num >= 0)
        {
            int selectedIndex = GetSelectedIndex(webElement);
            int num3 = num - selectedIndex;
            if (num3 == 0)
            {
                webElement.SendKeys(OpenQA.Selenium.Keys.Enter);
            }
            else
            {
                string text2 = ((num3 > 0) ? OpenQA.Selenium.Keys.ArrowDown : OpenQA.Selenium.Keys.ArrowUp);
                num3 = Math.Abs(num3);
                for (int i = 0; i < num3; i++)
                {
                    webElement.SendKeys(text2);
                    Thread.Sleep(60 + new Random().Next(70));
                }
                webElement.SendKeys(OpenQA.Selenium.Keys.Enter);
            }
        }
        if (!IsSelectedText(webElement, genderText))
        {
            TryCdpClickCenter(driver, webElement);
            Thread.Sleep(150);
            webElement.FindElements(By.TagName("option")).FirstOrDefault((IWebElement o) => (o.Text ?? "").Trim().Equals(genderText, StringComparison.OrdinalIgnoreCase))?.Click();
        }
        if (!IsSelectedText(webElement, genderText))
        {
            string text3 = null;
            if (1 == 0)
            {
            }
            string text4 = text switch
            {
                "transgender" => "T",
                "female" => "F",
                "male" => "M",
                _ => FindOptionValue(webElement, genderText),
            };
            if (1 == 0)
            {
            }
            text3 = text4;
            if (text3 != null)
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("\r\n                var sel=arguments[0], val=arguments[1];\r\n                sel.value = val;\r\n                sel.dispatchEvent(new Event('input',{bubbles:true}));\r\n                sel.dispatchEvent(new Event('change',{bubbles:true}));\r\n            ", new object[2] { webElement, text3 });
            }
        }
        if (!IsSelectedText(webElement, genderText))
        {
            throw new Exception("Gender select failed for row " + index1Based + ": '" + genderText + "'.");
        }
    }
    internal void TryCdpClickCenter(UndetectedChromeDriver driver, IWebElement el)
    {
        try
        {
            IDictionary dictionary = (IDictionary)((IJavaScriptExecutor)driver).ExecuteScript("\r\n            var r = arguments[0].getBoundingClientRect();\r\n            return {left:r.left, top:r.top, width:r.width, height:r.height};\r\n        ", new object[1] { el });
            double num = Convert.ToDouble(dictionary["left"]);
            double num2 = Convert.ToDouble(dictionary["top"]);
            double num3 = Convert.ToDouble(dictionary["width"]);
            double num4 = Convert.ToDouble(dictionary["height"]);
            int num5 = (int)(num + num3 / 2.0);
            int num6 = (int)(num2 + num4 / 2.0);
            driver.ExecuteCdpCommand("Input.dispatchMouseEvent", new Dictionary<string, object>
            {
                ["type"] = "mouseMoved",
                ["x"] = num5,
                ["y"] = num6,
                ["buttons"] = 1
            });
            driver.ExecuteCdpCommand("Input.dispatchMouseEvent", new Dictionary<string, object>
            {
                ["type"] = "mousePressed",
                ["x"] = num5,
                ["y"] = num6,
                ["button"] = "left",
                ["clickCount"] = 1
            });
            driver.ExecuteCdpCommand("Input.dispatchMouseEvent", new Dictionary<string, object>
            {
                ["type"] = "mouseReleased",
                ["x"] = num5,
                ["y"] = num6,
                ["button"] = "left",
                ["clickCount"] = 1
            });
        }
        catch
        {
        }
    }
    internal string FindOptionValue(IWebElement selectEl, string visibleText)
    {
        return selectEl.FindElements(By.TagName("option")).FirstOrDefault((IWebElement o) => (o.Text ?? "").Trim().Equals(visibleText, StringComparison.OrdinalIgnoreCase))?.GetAttribute("value");
    }
    internal bool IsSelectedText(IWebElement selectEl, string expectedText)
    {
        IWebElement webElement = selectEl.FindElements(By.CssSelector("option:checked")).FirstOrDefault();
        if (webElement == null)
        {
            return false;
        }
        return (webElement.Text ?? "").Trim().Equals(expectedText.Trim(), StringComparison.OrdinalIgnoreCase);
    }
    internal bool HumanLikeRadioByFormControl(IWebDriver driver, string formControlName, string value)
    {
        string script = "\r\n    (function(){\r\n      var host = document.querySelector('p-radiobutton[formcontrolname=\"__FC__\"] input[type=\"radio\"][value=\"__VAL__\"]');\r\n      if(!host){\r\n        // in some templates, p-radiobutton is sibling; broaden search:\r\n        host = Array.from(document.querySelectorAll('p-radiobutton[formcontrolname=\"__FC__\"]'))\r\n               .map(function(pr){ return pr.querySelector('input[type=\"radio\"][value=\"__VAL__\"]'); })\r\n               .filter(Boolean)[0] || null;\r\n      }\r\n      if(!host) return 'NF'; // not found\r\n\r\n      var box = host.closest('.ui-radiobutton');\r\n      if(!box){ return 'NOBOX'; }\r\n\r\n      var uiBox = box.querySelector('.ui-radiobutton-box') || box;\r\n\r\n      // if already selected, done\r\n      if(host.checked) return 'OK';\r\n\r\n      // bring into view\r\n      try { uiBox.scrollIntoView({block:'center'}); } catch(e){}\r\n\r\n      // fire a human-ish pointer sequence on the visible box\r\n      var r = uiBox.getBoundingClientRect();\r\n      var x = Math.floor(r.left + r.width  * (0.40 + Math.random()*0.2));\r\n      var y = Math.floor(r.top  + r.height * (0.40 + Math.random()*0.2));\r\n      function fire(type, delay){\r\n        setTimeout(function(){\r\n          var evt = new MouseEvent(type,{bubbles:true,cancelable:true,clientX:x,clientY:y,view:window,button:0});\r\n          uiBox.dispatchEvent(evt);\r\n        }, delay);\r\n      }\r\n      var base = 40 + Math.floor(Math.random()*90);\r\n      fire('mousemove', base);\r\n      fire('mouseover', base+60);\r\n      fire('mousemove', base+120);\r\n      fire('mousedown', base+180);\r\n      fire('mouseup',   base+220);\r\n      fire('click',     base+260);\r\n\r\n      // verify after a short delay; if not selected, force-set + Angular events\r\n      setTimeout(function(){\r\n        if(!host.checked){\r\n          try { uiBox.click(); } catch(e){}\r\n        }\r\n        setTimeout(function(){\r\n          if(!host.checked){\r\n            host.checked = true;\r\n            host.dispatchEvent(new Event('input', {bubbles:true}));\r\n            host.dispatchEvent(new Event('change',{bubbles:true}));\r\n            // nudge UI box so PrimeNG updates classes/aria\r\n            try { uiBox.dispatchEvent(new Event('click', {bubbles:true})); } catch(e){}\r\n          }\r\n        }, 120);\r\n      }, 140);\r\n\r\n      return 'PEND';\r\n    })();\r\n    ".Replace("__FC__", formControlName).Replace("__VAL__", value);
        string text = (driver as IJavaScriptExecutor).ExecuteScript(script) as string;
        if (!(text == "OK"))
        {
            return text == "PEND";
        }
        return true;
    }
    internal void HumanTickPaymentRadio(UndetectedChromeDriver driver)
    {
        IWebElement webElement = new WebDriverWait(driver, TimeSpan.FromSeconds(10.0)).Until(ExpectedConditions.ElementIsVisible(By.CssSelector("label[for='2'] div[role='radio']")));
        ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({behavior:'smooth', block:'center'});", new object[1] { webElement });
        Thread.Sleep(new Random().Next(200, 400));
        Size size = webElement.Size;
        Point location = webElement.Location;
        int num = location.X - new Random().Next(20, 50);
        int num2 = location.Y - new Random().Next(10, 30);
        int num3 = location.X + size.Width / 2;
        int num4 = location.Y + size.Height / 2;
        Actions actions = new Actions(driver);
        int num5 = new Random().Next(7, 14);
        for (int i = 0; i <= num5; i++)
        {
            int num6 = num + (int)((double)((num3 - num) * i) / (double)num5);
            int num7 = num2 + (int)((double)((num4 - num2) * i) / (double)num5);
            actions.MoveByOffset(num6 - ((i == 0) ? num : num6), num7 - ((i == 0) ? num2 : num7)).Perform();
            Thread.Sleep(new Random().Next(15, 40));
        }
        actions.MoveToElement(webElement).Perform();
        Thread.Sleep(new Random().Next(80, 180));
        actions.Click().Perform();
    }

    internal void FillPassengerBasic(
        UndetectedChromeDriver driver,
        int slotIndex,
        string name,
        string age,
        string gender)
    {
        string baseXpath =
            $"(//app-passenger//div[contains(@class,'container-fluid')])[{slotIndex}]";

        WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

        // Name Field
        IWebElement nameBox = wait.Until(d =>
        {
            try
            {
                var e = d.FindElement(By.XPath(
                    baseXpath +
                    "//p-autocomplete[@formcontrolname='passengerName']//input"));

                return (e.Displayed && e.Enabled) ? e : null;
            }
            catch
            {
                return null;
            }
        });

        ((IJavaScriptExecutor)driver).ExecuteScript(
            "arguments[0].scrollIntoView({block:'center'});",
            nameBox);

        Thread.Sleep(200);

        TypeLikeHuman(
            driver,
            By.XPath(baseXpath +
            "//p-autocomplete[@formcontrolname='passengerName']//input"),
            name);

        Thread.Sleep(200);

        // Age Field
        IWebElement ageBox = wait.Until(d =>
        {
            try
            {
                var e = d.FindElement(By.XPath(
                    baseXpath +
                    "//input[@formcontrolname='passengerAge']"));

                return (e.Displayed && e.Enabled) ? e : null;
            }
            catch
            {
                return null;
            }
        });

        ((IJavaScriptExecutor)driver).ExecuteScript(
            "arguments[0].scrollIntoView({block:'center'});",
            ageBox);

        Thread.Sleep(150);

        TypeLikeHuman(
            driver,
            By.XPath(baseXpath +
            "//input[@formcontrolname='passengerAge']"),
            age);

        Thread.Sleep(200);

        // Gender
        SelectGender_Humanish2(driver, slotIndex, gender);
    }
    private bool IsClickSuccessful(IWebDriver driver)
    {
        return !driver.Url.Contains("train-search");
    }
    internal bool HumanClickWithRetry(IWebDriver driver, By by, int maxRetry = 3, int timeoutSeconds = 20)
    {
        WebDriverWait webDriverWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutSeconds));
        Random random = new Random();
        for (int i = 1; i <= maxRetry; i++)
        {
            try
            {
                IWebElement webElement = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(by));
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView({block:'center'});", webElement);
                Thread.Sleep(random.Next(200, 400));
                webElement.Click();
                Thread.Sleep(random.Next(500, 900));
                if (IsClickSuccessful(driver))
                {
                    return true;
                }
            }
            catch
            {
            }
            Thread.Sleep(1000);
            if (i != maxRetry)
            {
                continue;
            }
            try
            {
                IWebElement webElement2 = driver.FindElement(by);
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", webElement2);
                Thread.Sleep(800);
                if (IsClickSuccessful(driver))
                {
                    return true;
                }
            }
            catch
            {
            }
        }
        return false;
    }

    internal void ClickElementHuman_ooo(IWebDriver driver, string profileKeyword, By locator)
    {
        Config.MoveSpeed = 550;
        Config.BezierMaxOffset = 120;
        Config.Jitter = 2;
        Config.ShakeRadius = 3;
        Config.DragSpeed = 900;
        Config.UseBezier = true;
        IWebElement webElement = driver.FindElement(locator);
        IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
        string script = "\r\n            var el   = arguments[0];\r\n            var r    = el.getBoundingClientRect();\r\n            var sx   = window.screenX + r.left + (r.width  / 2);\r\n            var sy   = window.screenY \r\n                       + (window.outerHeight - window.innerHeight) \r\n                       + r.top + (r.height / 2);\r\n            return [sx, sy];\r\n        ";
        ReadOnlyCollection<object> readOnlyCollection = (ReadOnlyCollection<object>)javaScriptExecutor.ExecuteScript(script, webElement);
        int num = (int)Math.Round(Convert.ToDouble(readOnlyCollection[0]));
        int num2 = (int)Math.Round(Convert.ToDouble(readOnlyCollection[1]));
        LeftClick(num + 10, num2 - 5);
    }
    [DllImport("user32.dll")]
	private static extern bool SetCursorPos(int X, int Y);

	[DllImport("user32.dll")]
	private static extern bool GetCursorPos(out POINT lpPoint);

	[DllImport("user32.dll")]
	private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int extraInfo);

	private static (int x, int y) GetPosition()
	{
		GetCursorPos(out var lpPoint);
		return (x: lpPoint.X, y: lpPoint.Y);
	}

	private static (double x, double y) CubicBezier(double t, (double x, double y) p0, (double x, double y) c1, (double x, double y) c2, (double x, double y) p3)
	{
		double num = 1.0 - t;
		double num2 = t * t;
		double num3 = num * num;
		double num4 = num3 * num;
		double num5 = num2 * t;
		double item = num4 * p0.x + 3.0 * num3 * t * c1.x + 3.0 * num * num2 * c2.x + num5 * p3.x;
		double item2 = num4 * p0.y + 3.0 * num3 * t * c1.y + 3.0 * num * num2 * c2.y + num5 * p3.y;
		return (x: item, y: item2);
	}

	private void MoveSmooth(int targetX, int targetY)
	{
		GetCursorPos(out var lpPoint);
		int x = lpPoint.X;
		int y = lpPoint.Y;
		int num = Config.MoveSpeed;
		if (num < 80)
		{
			num = 80;
		}
		int num2 = rnd.Next(6, 12);
		int num3 = Math.Max(1, num / num2);
		(double, double) p = (x, y);
		(double, double) p2 = (targetX, targetY);
		(double, double) c;
		(double, double) c2;
		if (!Config.UseBezier)
		{
			c = (p.Item1 + (p2.Item1 - p.Item1) / 3.0, p.Item2 + (p2.Item2 - p.Item2) / 3.0);
			c2 = (p.Item1 + 2.0 * (p2.Item1 - p.Item1) / 3.0, p.Item2 + 2.0 * (p2.Item2 - p.Item2) / 3.0);
		}
		else
		{
			double num4 = p2.Item1 - p.Item1;
			double num5 = p2.Item2 - p.Item2;
			double num6 = Math.Sqrt(num4 * num4 + num5 * num5);
			double num7 = 0.0;
			double num8 = 0.0;
			if (num6 > 0.001)
			{
				num7 = (0.0 - num5) / num6;
				num8 = num4 / num6;
			}
			double num9 = rnd.Next(-Config.BezierMaxOffset, Config.BezierMaxOffset + 1);
			double num10 = rnd.Next(-Config.BezierMaxOffset, Config.BezierMaxOffset + 1);
			c = (p.Item1 + num4 / 3.0 + num7 * num9, p.Item2 + num5 / 3.0 + num8 * num9);
			c2 = (p.Item1 + 2.0 * num4 / 3.0 + num7 * num10, p.Item2 + 2.0 * num5 / 3.0 + num8 * num10);
		}
		for (int i = 1; i <= num3; i++)
		{
			double num11 = (double)i / (double)num3;
			(double, double) tuple = CubicBezier(num11 * num11 * (3.0 - 2.0 * num11), p, c, c2, p2);
			double item = tuple.Item1;
			double item2 = tuple.Item2;
			int num12 = (int)Math.Round(item);
			int num13 = (int)Math.Round(item2);
			int x2 = num12 + rnd.Next(-Config.Jitter, Config.Jitter + 1);
			num13 += rnd.Next(-Config.Jitter, Config.Jitter + 1);
			SetCursorPos(x2, num13);
			Thread.Sleep(num2);
		}
		SetCursorPos(targetX, targetY);
	}

	private void Shake()
	{
		GetCursorPos(out var lpPoint);
		int x = lpPoint.X;
		int y = lpPoint.Y;
		int num = 1;
		int num2 = 2;
		for (int i = 0; i < num; i++)
		{
			int x2 = x + rnd.Next(-num2, num2 + 1);
			int y2 = y + rnd.Next(-num2, num2 + 1);
			SetCursorPos(x2, y2);
			Thread.Sleep(rnd.Next(120, 200));
		}
		SetCursorPos(x, y);
		Thread.Sleep(rnd.Next(80, 140));
	}

	private void Shake2()
	{
		GetCursorPos(out var lpPoint);
		int x = lpPoint.X;
		int y = lpPoint.Y;
		for (int i = 0; i < Config.ShakeSteps; i++)
		{
			int x2 = x + rnd.Next(-Config.ShakeRadius, Config.ShakeRadius + 1);
			int y2 = y + rnd.Next(-Config.ShakeRadius, Config.ShakeRadius + 1);
			SetCursorPos(x2, y2);
			Thread.Sleep(rnd.Next(20, 60));
		}
		SetCursorPos(x, y);
	}

	private void LeftClick(int x, int y)
	{
		MoveSmooth(x, y);
		Thread.Sleep(rnd.Next(Config.MinClickDelay, Config.MaxClickDelay));
		mouse_event(2, x, y, 0, 0);
		Thread.Sleep(rnd.Next(Config.MinBetweenClick, Config.MaxBetweenClick));
		mouse_event(4, x, y, 0, 0);
	}

	private void LeftClick2(int x, int y)
	{
		Thread.Sleep(rnd.Next(Config.MinClickDelay, Config.MaxClickDelay));
		mouse_event(2, x, y, 0, 0);
		Thread.Sleep(rnd.Next(Config.MinBetweenClick, Config.MaxBetweenClick));
		mouse_event(4, x, y, 0, 0);
	}

	private void LeftDoubleClick(int x, int y)
	{
		LeftClick(x, y);
		Thread.Sleep(rnd.Next(80, 200));
		LeftClick(x, y);
	}

	private void RightClick(int x, int y)
	{
		MoveSmooth(x, y);
		Shake();
		Thread.Sleep(rnd.Next(Config.MinClickDelay, Config.MaxClickDelay));
		mouse_event(8, x, y, 0, 0);
		Thread.Sleep(rnd.Next(Config.MinBetweenClick, Config.MaxBetweenClick));
		mouse_event(16, x, y, 0, 0);
	}

	private void Drag(int startX, int startY, int endX, int endY)
	{
		MoveSmooth(startX, startY);
		Thread.Sleep(rnd.Next(60, 150));
		mouse_event(2, startX, startY, 0, 0);
		Thread.Sleep(rnd.Next(80, 150));
		int num = Config.DragSpeed;
		if (num < 100)
		{
			num = 100;
		}
		int num2 = rnd.Next(8, 15);
		int num3 = Math.Max(1, num / num2);
		GetCursorPos(out var lpPoint);
		int x = lpPoint.X;
		int y = lpPoint.Y;
		for (int i = 1; i <= num3; i++)
		{
			double num4 = (double)i / (double)num3;
			double num5 = num4 * num4 * (3.0 - 2.0 * num4);
			int x2 = (int)((double)x + (double)(endX - x) * num5);
			int y2 = (int)((double)y + (double)(endY - y) * num5);
			SetCursorPos(x2, y2);
			Thread.Sleep(num2);
		}
		SetCursorPos(endX, endY);
		Thread.Sleep(rnd.Next(60, 150));
		mouse_event(4, endX, endY, 0, 0);
	}
}
