using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using SharpDX;
using VRage.Platform.Windows.Forms;

namespace VRage.Platform.Windows
{
	internal class MyCrashReporting : IMyCrashReporting
	{
		public bool IsCriticalMemory => false;

		public event Action<Exception> ExitingProcessOnCrash;

		public void MessageBoxModCrashForm(ref MyModCrashScreenTexts texts)
		{
			ModCrashedTheGameMesageBox.ShowDialog(ref texts);
		}

		public bool MessageBoxCrashForm(ref MyCrashScreenTexts texts, out string message, out string email)
		{
			return MyMessageBoxCrashForm.ShowDialog(ref texts, out message, out email);
		}

		public void SetNativeExceptionHandler(Action<IntPtr> handler)
		{
		}

		public ExceptionType GetExceptionType(Exception e)
		{
			if (e != null)
			{
				SharpDXException ex;
				if ((ex = e as SharpDXException) != null)
				{
					if (ex.Descriptor.NativeApiCode == "DXGI_ERROR_UNSUPPORTED")
					{
						return ExceptionType.UnsupportedGpu;
					}
					if (ex.ResultCode == Result.OutOfMemory)
					{
						return ExceptionType.OutOfMemory;
					}
					if (ex.ResultCode.Code == -2005532292)
					{
						return ExceptionType.OutOfVideoMemory;
					}
					SharpDXException ex2 = ex;
					if (ex2.ResultCode.Code == -2005270523 || ex2.ResultCode.Code == -2005270522 || ex2.ResultCode.Code == -2005270521)
					{
						return ExceptionType.DriverCrash;
					}
				}
				if (e is OutOfMemoryException)
				{
					return ExceptionType.OutOfMemory;
				}
				return GetExceptionType(e.InnerException);
			}
			return ExceptionType.Other;
		}

		public void PrepareCrashAnalyticsReporting(string logPath, bool GDPRConsent, CrashInfo info, bool isUnsupportedGpu)
		{
			string text = (isUnsupportedGpu ? "reporX" : "report");
			ProcessStartInfo processStartInfo = new ProcessStartInfo();
			processStartInfo.Arguments = "-" + text + " \"" + logPath + "\" \"" + info.GameName + "\" \"" + info.AppVersion + "\" \"" + info.AnalyticId + "\"";
			processStartInfo.FileName = Assembly.GetEntryAssembly().Location;
			processStartInfo.UseShellExecute = false;
			processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
			processStartInfo.RedirectStandardInput = true;
			Process.Start(processStartInfo)?.StandardInput.Close();
		}

		public bool ExtractCrashAnalyticsReport(string[] args, out string logPath, out CrashInfo info, out bool isUnsupportedGpu, out bool exitAfterReport)
		{
			exitAfterReport = true;
			for (int i = 0; i < 2; i++)
			{
				bool flag = i != 0;
				string value = ((i == 0) ? "-report" : "-reporX");
				int num = Array.IndexOf(args, value);
				if (num < 0)
				{
					continue;
				}
				isUnsupportedGpu = flag;
				logPath = ((num + 1 < args.Length) ? args[num + 1] : "");
				if (File.Exists(logPath))
				{
					using (StreamReader reader = new StreamReader(logPath))
					{
						info = CrashInfo.Read(reader);
					}
				}
				else
				{
					info = default(CrashInfo);
				}
				return true;
			}
			logPath = null;
			info = default(CrashInfo);
			isUnsupportedGpu = false;
			exitAfterReport = false;
			return false;
		}

		public void UpdateHangAnalytics(CrashInfo hangInfo, string logPath, bool GDPRConsent)
		{
		}

		public void CleanupCrashAnalytics()
		{
		}

		public void ExitProcessOnCrash(Exception exception)
		{
			this.ExitingProcessOnCrash?.Invoke(exception);
			Process.GetCurrentProcess().Kill();
		}

		[DllImport("dbghelp.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
		private static extern bool MiniDumpWriteDump(IntPtr hProcess, uint processId, SafeHandle hFile, uint dumpType, IntPtr expParam, IntPtr userStreamParam, IntPtr callbackParam);

		[DllImport("kernel32.dll", ExactSpelling = true)]
		private static extern uint GetCurrentThreadId();

		public unsafe void WriteMiniDump(string path, MyMiniDump.Options options, IntPtr exceptionPointers)
		{
			Process currentProcess = Process.GetCurrentProcess();
			IntPtr handle = currentProcess.Handle;
			uint id = (uint)currentProcess.Id;
			if (exceptionPointers == IntPtr.Zero)
			{
				exceptionPointers = Marshal.GetExceptionPointers();
			}
			MyMiniDump.ExceptionInformation exceptionInformation = default(MyMiniDump.ExceptionInformation);
			exceptionInformation.ThreadId = GetCurrentThreadId();
			exceptionInformation.ClientPointers = false;
			exceptionInformation.ExceptionPointers = exceptionPointers;
			using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
			{
				MiniDumpWriteDump(handle, id, fileStream.SafeFileHandle, (uint)options, (exceptionPointers == IntPtr.Zero) ? IntPtr.Zero : ((IntPtr)(&exceptionInformation)), IntPtr.Zero, IntPtr.Zero);
			}
		}

		public IEnumerable<string> AdditionalReportFiles()
		{
			return null;
		}
	}
}
