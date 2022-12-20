using System;
using System.Collections.Generic;

namespace VRage
{
	public interface IMyCrashReporting
	{
		bool IsCriticalMemory { get; }

		event Action<Exception> ExitingProcessOnCrash;

		ExceptionType GetExceptionType(Exception e);

		void WriteMiniDump(string dumpPath, MyMiniDump.Options dumpFlags, IntPtr exceptionPointers);

		void SetNativeExceptionHandler(Action<IntPtr> handler);

		void PrepareCrashAnalyticsReporting(string logPath, bool GDPRConsent, CrashInfo info, bool isUnsupportedGpu);

		bool ExtractCrashAnalyticsReport(string[] args, out string logPath, out CrashInfo info, out bool isUnsupportedGpu, out bool exitAfterReport);

		void UpdateHangAnalytics(CrashInfo hangInfo, string logPath, bool GDPRConsent);

		void CleanupCrashAnalytics();

		bool MessageBoxCrashForm(ref MyCrashScreenTexts texts, out string message, out string email);

		void MessageBoxModCrashForm(ref MyModCrashScreenTexts texts);

		void ExitProcessOnCrash(Exception exception);

		IEnumerable<string> AdditionalReportFiles();
	}
}
