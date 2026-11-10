using System;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Threading;

namespace NEXUS.IRCommDLL;

public class EdgeTaskbarHider
{
	internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

	private const int GWL_EXSTYLE = -20;

	private const int WS_EX_TOOLWINDOW = 128;

	private const int WS_EX_APPWINDOW = 262144;

	private const uint SWP_NOSIZE = 1u;

	private const uint SWP_NOMOVE = 2u;

	private const uint SWP_NOZORDER = 4u;

	private const uint SWP_FRAMECHANGED = 32u;

	[DllImport("user32.dll")]
	private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

	[DllImport("user32.dll")]
	private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

	[DllImport("user32.dll")]
	private static extern bool IsWindowVisible(IntPtr hWnd);

	[DllImport("user32.dll")]
	private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

	[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
	private static extern int GetWindowLong32(IntPtr hWnd, int nIndex);

	[DllImport("user32.dll")]
	private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

	[DllImport("user32.dll", EntryPoint = "SetWindowLong")]
	private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

	[DllImport("user32.dll", SetLastError = true)]
	private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

	private static int GetExStyle(IntPtr hWnd)
	{
		if (IntPtr.Size != 8)
		{
			while (true)
			{
				int[,,,,] array = new int[2, 5, 1, 1, 1]
				{
					{
						{ { { -1273447715 } } },
						{ { { 807851596 } } },
						{ { { -169110706 } } },
						{ { { 631805353 } } },
						{ { { 1865765628 } } }
					},
					{
						{ { { 463815544 } } },
						{ { { -1069621240 } } },
						{ { { 470888829 } } },
						{ { { -1800982978 } } },
						{ { { 2005285674 } } }
					}
				};
				switch (array[1, 2, 0, 0, 0] * -1258757585)
				{
				case 703623154:
					continue;
				case 703623155:
					return GetWindowLong32(hWnd, -20);
				}
				break;
			}
		}
		return (int)GetWindowLongPtr(hWnd, -20);
	}

	private static void SetExStyle(IntPtr hWnd, int exStyle)
	{
		if (IntPtr.Size == 8)
		{
			SetWindowLongPtr(hWnd, -20, new IntPtr(exStyle));
		}
		else
		{
			SetWindowLong32(hWnd, -20, exStyle);
		}
	}
    public static Process FindChromeProcessByCmd(string userDataDir)
    {
        string query =
            "SELECT ProcessId, CommandLine " +
            "FROM Win32_Process " +
            "WHERE Name='chrome.exe'";

        using var searcher = new ManagementObjectSearcher(query);

        foreach (ManagementObject item in searcher.Get())
        {
            string commandLine =
                item["CommandLine"]?.ToString() ?? string.Empty;

            if (commandLine.IndexOf(
                    userDataDir,
                    StringComparison.OrdinalIgnoreCase) >= 0)
            {
                int pid = Convert.ToInt32(item["ProcessId"]);

                try
                {
                    return Process.GetProcessById(pid);
                }
                catch
                {
                    // Process exit ho gaya ho to ignore karein
                }
            }
        }

        return null;
    }
    public static IntPtr FindChromeWindowByUserDataDir(
        string userDataDir,
        int waitMs = 5000)
    {
        int endTime = Environment.TickCount + waitMs;

        while (Environment.TickCount < endTime)
        {
            Process process = FindChromeProcessByCmd(userDataDir);

            if (process != null)
            {
                IntPtr hwnd = GetTopLevelWindowForPid(process.Id);

                if (hwnd != IntPtr.Zero)
                {
                    return hwnd;
                }
            }

            Thread.Sleep(150);
        }

        return IntPtr.Zero;
    }
    public static IntPtr FindEdgeWindowByUserDataDir(string userDataDir, int waitMs = 5000)
	{
		int num = Environment.TickCount + waitMs;
		IntPtr topLevelWindowForPid;
		while (true)
		{
			if (Environment.TickCount < num)
			{
				Process process = FindEdgeProcessByCmd(userDataDir);
				if (process != null)
				{
					topLevelWindowForPid = GetTopLevelWindowForPid(process.Id);
					if (topLevelWindowForPid != IntPtr.Zero)
					{
						break;
					}
				}
				Thread.Sleep(150);
				continue;
			}
			return IntPtr.Zero;
		}
		return topLevelWindowForPid;
	}
    public static IntPtr FindBraveWindowByUserDataDir(string userDataDir, int waitMs = 5000)
    {
        int endTime = Environment.TickCount + waitMs;
        IntPtr hwnd;

        while (true)
        {
            if (Environment.TickCount < endTime)
            {
                Process process = FindBraveProcessByCmd(userDataDir);

                if (process != null)
                {
                    hwnd = GetTopLevelWindowForPid(process.Id);

                    if (hwnd != IntPtr.Zero)
                        return hwnd;
                }

                Thread.Sleep(150);
                continue;
            }

            return IntPtr.Zero;
        }
    }
    public static Process FindBraveProcessByCmd(string userDataDir)
    {
        foreach (var process in Process.GetProcessesByName("brave"))
        {
            try
            {
                string cmd = GetCommandLine(process);

                if (!string.IsNullOrEmpty(cmd) && cmd.Contains(userDataDir))
                {
                    return process;
                }
            }
            catch
            {
                // ignore access denied processes
            }
        }

        return null;
    }
    public static string GetCommandLine(Process process)
    {
        try
        {
            using (var searcher = new System.Management.ManagementObjectSearcher(
                $"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {process.Id}"))
            {
                foreach (System.Management.ManagementObject obj in searcher.Get())
                {
                    return obj["CommandLine"]?.ToString();
                }
            }
        }
        catch { }

        return null;
    }
    private static Process FindEdgeProcessByCmd(string containsPath)
	{
		string queryString = "SELECT ProcessId, CommandLine FROM Win32_Process WHERE Name='msedge.exe'";
		using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(queryString))
		{
			foreach (ManagementObject item in managementObjectSearcher.Get())
			{
				string propertyName = "CommandLine";
				if ((item[propertyName] ?? "").ToString().IndexOf(containsPath, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					string propertyName2 = "ProcessId";
					int processId = Convert.ToInt32(item[propertyName2]);
					try
					{
						return Process.GetProcessById(processId);
					}
					catch
					{
					}
				}
			}
		}
		return null;
	}

	private static IntPtr GetTopLevelWindowForPid(int pid)
	{
		IntPtr found = IntPtr.Zero;
		EnumWindows(delegate(IntPtr h, IntPtr l)
		{
			if (!IsWindowVisible(h))
			{
				return true;
			}
			GetWindowThreadProcessId(h, out var lpdwProcessId);
			if (lpdwProcessId == pid)
			{
				found = h;
				return false;
			}
			return true;
		}, IntPtr.Zero);
		return found;
	}

	public static bool HideFromTaskbar(IntPtr hWnd)
	{
		if (hWnd == IntPtr.Zero)
		{
			return false;
		}
		int exStyle = GetExStyle(hWnd);
		exStyle &= -262145;
		exStyle |= 0x80;
		SetExStyle(hWnd, exStyle);
		return SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, 39u);
	}

	public static bool ShowOnTaskbar(IntPtr hWnd)
	{
		if (hWnd == IntPtr.Zero)
		{
			return false;
		}
		int exStyle = GetExStyle(hWnd);
		exStyle |= 0x40000;
		exStyle &= -129;
		SetExStyle(hWnd, exStyle);
		return SetWindowPos(hWnd, IntPtr.Zero, 0, 0, 0, 0, 39u);
	}
}
