using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using OpenQA.Selenium;

namespace NEXUS.IRCommDLL;

internal class WindowsApi
{
	public struct RECT
	{
		public int Left;

		public int Top;

		public int Right;

		public int Bottom;
	}

	private const int MOUSEEVENTF_LEFTDOWN = 2;

	private const int MOUSEEVENTF_LEFTUP = 4;

	[DllImport("user32.dll")]
	private static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

	[DllImport("user32.dll")]
	private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int extraInfo);

	[DllImport("user32.dll")]
	private static extern bool SetCursorPos(int X, int Y);

	internal Point GetAccurateScreenCoordinates(IWebDriver driver, IWebElement element)
	{
		IJavaScriptExecutor javaScriptExecutor = (IJavaScriptExecutor)driver;
		Point location = element.Location;
		Size size = element.Size;
		string script = "return window.pageXOffset;";
		_ = (long)javaScriptExecutor.ExecuteScript(script);
		string script2 = "return window.pageYOffset;";
		_ = (long)javaScriptExecutor.ExecuteScript(script2);
		string processName = "chrome";
		GetWindowRect(Process.GetProcessesByName(processName)[0].MainWindowHandle, out var rect);
		int left = rect.Left;
		int top = rect.Top;
		int num = location.X + size.Width / 2;
		int num2 = location.Y + size.Height / 2;
		int x = left + num;
		int y = top + num2 + 85;
		return new Point(x, y);
	}

	internal void DrawDebugDot(int x, int y)
	{
		using Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
		graphics.FillEllipse(Brushes.Red, x - 5, y - 5, 10, 10);
	}

	internal void ClickWithDebugDot(int x, int y)
	{
		SetCursorPos(x, y);
		DrawDebugDot(x, y);
		Thread.Sleep(200);
		mouse_event(2, x, y, 0, 0);
		Thread.Sleep(50);
		mouse_event(4, x, y, 0, 0);
	}
}
