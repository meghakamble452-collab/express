using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;

namespace NEXUS.IRCommDLL;

internal class DevToolsKiller
{
	private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

	private const uint WM_CLOSE = 16u;

	private static Timer _timer;

	[DllImport("user32.dll")]
	private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

	[DllImport("user32.dll")]
	private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

	[DllImport("user32.dll")]
	private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

	public static void StartWatching(object j)
	{
		_timer = new Timer(500.0);
		_timer.Elapsed += delegate
		{
			CheckAndCloseDevTools();
		};
		_timer.Start();
	}

	private static void CheckAndCloseDevTools()
	{
		try
		{
			EnumWindows(delegate(IntPtr hWnd, IntPtr lParam)
			{
				StringBuilder stringBuilder = new StringBuilder(512);
				GetWindowText(hWnd, stringBuilder, stringBuilder.Capacity);
				string text = stringBuilder.ToString();
				if (!string.IsNullOrEmpty(text))
				{
					string value = "Developer Tools";
					if (!text.Contains(value))
					{
						string value2 = "DevTools";
						if (!text.Contains(value2))
						{
							string value3 = "Inspect";
							if (!text.Contains(value3))
							{
								goto IL_0078;
							}
						}
					}
					PostMessage(hWnd, 16u, IntPtr.Zero, IntPtr.Zero);
				}
				goto IL_0078;
				IL_0078:
				return true;
			}, IntPtr.Zero);
		}
		catch (Exception)
		{
		}
	}
}
