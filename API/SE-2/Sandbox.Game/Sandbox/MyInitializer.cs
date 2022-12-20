using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Havok;
using ParallelTasks;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.World;
using VRage;
using VRage.Common.Utils;
using VRage.Cryptography;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageRender;

namespace Sandbox
{
	public static class MyInitializer
	{
<<<<<<< HEAD
		/// <summary>
		/// <see cref="T:Sandbox.MyInitializer.MyOutOfMemoryException" /> doesn't trigger special behaviour of <see cref="T:System.OutOfMemoryException" />
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private class MyOutOfMemoryException : Exception
		{
			public override string StackTrace => string.Empty;

			public MyOutOfMemoryException()
				: base("Game is at critically low memory")
			{
			}
		}

		private class MyNativeException : Exception
		{
			private StackTrace m_trace = new StackTrace(1, fNeedFileInfo: false);

<<<<<<< HEAD
			/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public override string StackTrace => m_trace.ToString();

			public MyNativeException()
				: base("Exception in unmanaged code.")
			{
			}

<<<<<<< HEAD
			/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public override string ToString()
			{
				return StackTrace;
			}
		}

		private static string m_appName;

		private static HashSet<string> m_ignoreList = new HashSet<string>();

		private static object m_exceptionSyncRoot = new object();

		private static IMyCrashReporting ErrorPlatform => MyVRage.Platform.CrashReporting;

		private static void ChecksumFailed(string filename, string hash)
		{
			if (!m_ignoreList.Contains(filename))
			{
				m_ignoreList.Add(filename);
				MySandboxGame.Log.WriteLine("Error: checksum of file '" + filename + "' failed " + hash);
			}
		}

		private static void ChecksumNotFound(IFileVerifier verifier, string filename)
		{
			MyChecksumVerifier myChecksumVerifier = (MyChecksumVerifier)verifier;
			if (!m_ignoreList.Contains(filename) && filename.StartsWith(myChecksumVerifier.BaseChecksumDir, StringComparison.InvariantCultureIgnoreCase) && filename.Substring(Math.Min(filename.Length, myChecksumVerifier.BaseChecksumDir.Length + 1)).StartsWith("Data", StringComparison.InvariantCultureIgnoreCase))
			{
				MySandboxGame.Log.WriteLine("Error: no checksum found for file '" + filename + "'");
				m_ignoreList.Add(filename);
			}
		}

		public static void InvokeBeforeRun(uint appId, string appName, string userDataPath, bool addDateToLog = false, int maxLogAge = -1, Action onConfigChangedCallback = null)
		{
			m_appName = appName;
			InitFileSystem(userDataPath);
			MySandboxGame.Log.InitWithDate(appName, MyFinalBuildConstants.APP_VERSION_STRING, maxLogAge);
			MyLog.Default = MySandboxGame.Log;
			InitExceptionHandling();
			string rootPath = MyFileSystem.RootPath;
			bool flag = SteamHelpers.IsSteamPath(rootPath);
			bool flag2 = SteamHelpers.IsAppManifestPresent(rootPath, appId);
			Sandbox.Engine.Platform.Game.IsPirated = !flag && !flag2;
			MySandboxGame.Log.WriteLineAndConsole(string.Format("Is official: {0} {1}{2}{3}", true, MyObfuscation.Enabled ? "[O]" : "[NO]", flag ? "[IS]" : "[NIS]", flag2 ? "[AMP]" : "[NAMP]"));
			MySandboxGame.Log.WriteLine("Branch / Sandbox: " + MyGameService.BranchName);
<<<<<<< HEAD
			MySandboxGame.Log.WriteLineAndConsole("Environment.ProcessorCount: " + Environment.ProcessorCount);
			MySandboxGame.Log.WriteLineAndConsole("Environment.OSVersion: " + MyVRage.Platform.System.GetOsName());
			MySandboxGame.Log.WriteLineAndConsole("Environment.CommandLine: " + Environment.CommandLine);
			MySandboxGame.Log.WriteLineAndConsole("Environment.Is64BitProcess: " + Environment.Is64BitProcess);
			MySandboxGame.Log.WriteLineAndConsole("Environment.Is64BitOperatingSystem: " + Environment.Is64BitOperatingSystem);
			MySandboxGame.Log.WriteLineAndConsole("Environment.Version: " + RuntimeInformation.FrameworkDescription);
			MySandboxGame.Log.WriteLineAndConsole("Environment.CurrentDirectory: " + Environment.CurrentDirectory);
=======
			MySandboxGame.Log.WriteLineAndConsole("Environment.ProcessorCount: " + MyEnvironment.ProcessorCount);
			MySandboxGame.Log.WriteLineAndConsole("Environment.OSVersion: " + MyVRage.Platform.System.GetOsName());
			MySandboxGame.Log.WriteLineAndConsole("Environment.CommandLine: " + Environment.get_CommandLine());
			MySandboxGame.Log.WriteLineAndConsole("Environment.Is64BitProcess: " + MyEnvironment.Is64BitProcess);
			MySandboxGame.Log.WriteLineAndConsole("Environment.Is64BitOperatingSystem: " + Environment.get_Is64BitOperatingSystem());
			MySandboxGame.Log.WriteLineAndConsole("Environment.Version: " + RuntimeInformation.FrameworkDescription);
			MySandboxGame.Log.WriteLineAndConsole("Environment.CurrentDirectory: " + Environment.get_CurrentDirectory());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MySandboxGame.Log.WriteLineAndConsole("CPU Info: " + MyVRage.Platform.System.GetInfoCPU(out var frequency, out var _));
			MySandboxGame.CPUFrequency = frequency;
			MySandboxGame.Log.WriteLine("IntPtr.Size: " + IntPtr.Size);
			MySandboxGame.Log.WriteLine("Default Culture: " + CultureInfo.CurrentCulture.Name);
			MySandboxGame.Log.WriteLine("Default UI Culture: " + CultureInfo.CurrentUICulture.Name);
			MyVRage.Platform.System.LogRuntimeInfo(MySandboxGame.Log.WriteLineAndConsole);
			MySandboxGame.Config = new MyConfig(appName + ".cfg");
			if (onConfigChangedCallback != null)
			{
				MySandboxGame.Config.OnSettingChanged += onConfigChangedCallback;
			}
			MySandboxGame.Config.Load();
		}

		public static void InitFileSystem(string userDataPath)
		{
			MyFileSystem.Init(Path.Combine(MyFileSystem.RootPath, "Content"), userDataPath);
		}

		public static void InitExceptionHandling()
		{
			HkCrashHunter.Init(() => MySession.Static?.GameplayFrameCounter ?? (-1));
<<<<<<< HEAD
			AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
=======
			AppDomain.get_CurrentDomain().add_UnhandledException((UnhandledExceptionEventHandler)UnhandledExceptionHandler);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ErrorPlatform.SetNativeExceptionHandler(delegate(IntPtr x)
			{
				ProcessUnhandledException(new MyNativeException(), x);
			});
<<<<<<< HEAD
			if (string.IsNullOrEmpty(Thread.CurrentThread.Name))
			{
				Thread.CurrentThread.Name = "Main thread";
			}
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
=======
			if (string.IsNullOrEmpty(Thread.get_CurrentThread().get_Name()))
			{
				Thread.get_CurrentThread().set_Name("Main thread");
			}
			Thread.get_CurrentThread().set_CurrentCulture(CultureInfo.InvariantCulture);
			Thread.get_CurrentThread().set_CurrentUICulture(CultureInfo.InvariantCulture);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (MyFakes.ENABLE_MINIDUMP_SENDING && MyFileSystem.IsInitialized)
			{
				if (MyFakes.COLLECT_SUSPEND_DUMPS)
				{
					MyVRage.Platform.Render.OnSuspending += MyMiniDump.CollectStateDump;
				}
				MyMiniDump.CleanupOldDumps();
			}
			ErrorPlatform.CleanupCrashAnalytics();
			MyErrorReporter.UpdateHangAnalytics();
		}

		public static void InvokeAfterRun()
		{
			MySandboxGame.Log.Close();
		}

		public static void InitCheckSum()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				string text = Path.Combine(MyFileSystem.ContentPath, "checksum.xml");
				if (!File.Exists(text))
				{
					MySandboxGame.Log.WriteLine("Checksum file is missing, game will run as usual but file integrity won't be verified");
					return;
				}
<<<<<<< HEAD
				using (FileStream fileStream = File.OpenRead(path))
				{
					MyChecksums myChecksums = (MyChecksums)new XmlSerializer(typeof(MyChecksums)).Deserialize(fileStream);
					MyChecksumVerifier myChecksumVerifier = new MyChecksumVerifier(myChecksums, MyFileSystem.ContentPath);
					myChecksumVerifier.ChecksumFailed += ChecksumFailed;
					myChecksumVerifier.ChecksumNotFound += ChecksumNotFound;
					fileStream.Position = 0L;
					SHA256 sHA = MySHA256.Create();
					sHA.Initialize();
					byte[] inArray = sHA.ComputeHash(fileStream);
					string text = "BgIAAACkAABSU0ExAAQAAAEAAQClSibD83Y6Akok8tAtkbMz4IpueWFra0QkkKcodwe2pV/RJAfyq5mLUGsF3JdTdu3VWn93VM+ZpL9CcMKS8HaaHmBZJn7k2yxNvU4SD+8PhiZ87iPqpkN2V+rz9nyPWTHDTgadYMmenKk2r7w4oYOooo5WXdkTVjAD50MroAONuQ==";
					MySandboxGame.Log.WriteLine("Checksum file hash: " + Convert.ToBase64String(inArray));
					MySandboxGame.Log.WriteLine($"Checksum public key valid: {myChecksums.PublicKey == text}, Key: {myChecksums.PublicKey}");
					MyFileSystem.FileVerifier = myChecksumVerifier;
				}
=======
				using FileStream fileStream = File.OpenRead(text);
				MyChecksums myChecksums = (MyChecksums)new XmlSerializer(typeof(MyChecksums)).Deserialize((Stream)fileStream);
				MyChecksumVerifier myChecksumVerifier = new MyChecksumVerifier(myChecksums, MyFileSystem.ContentPath);
				myChecksumVerifier.ChecksumFailed += ChecksumFailed;
				myChecksumVerifier.ChecksumNotFound += ChecksumNotFound;
				fileStream.Position = 0L;
				SHA256 obj = MySHA256.Create();
				((HashAlgorithm)obj).Initialize();
				byte[] inArray = ((HashAlgorithm)obj).ComputeHash((Stream)fileStream);
				string text2 = "BgIAAACkAABSU0ExAAQAAAEAAQClSibD83Y6Akok8tAtkbMz4IpueWFra0QkkKcodwe2pV/RJAfyq5mLUGsF3JdTdu3VWn93VM+ZpL9CcMKS8HaaHmBZJn7k2yxNvU4SD+8PhiZ87iPqpkN2V+rz9nyPWTHDTgadYMmenKk2r7w4oYOooo5WXdkTVjAD50MroAONuQ==";
				MySandboxGame.Log.WriteLine("Checksum file hash: " + Convert.ToBase64String(inArray));
				MySandboxGame.Log.WriteLine($"Checksum public key valid: {myChecksums.PublicKey == text2}, Key: {myChecksums.PublicKey}");
				MyFileSystem.FileVerifier = myChecksumVerifier;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch
			{
			}
		}

		/// <summary>
		/// This handler gets called when unhandled exception occurs in main thread or any other background thread.
		/// We display our own error message and prevent displaying windows standard crash message because I have discovered that
		/// if error occurs during XNA loading, no message box is displayed ever. Game just turns off. So this is a solution, user
		/// sees the same error message every time.
		/// But I have discovered that it's sometimes not called when CLR throws OutOfMemoryException. But sometimes it is!!!
		/// I assume there are other fatal exception types that won't be handled here: stack-something or engine-fatal-i-dont-know...
		/// Possible explanation of this mystery: OutOfMemoryException is so fatal that CRL just shut-downs the application so we can't write to log or display messagebox.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="args">The <see cref="T:System.UnhandledExceptionEventArgs" /> instance containing the event data.</param>
		[HandleProcessCorruptedStateExceptions]
		[SecurityCritical]
		private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
		{
			ProcessUnhandledException(args.ExceptionObject as Exception, IntPtr.Zero);
		}

		private static void ProcessUnhandledException(Exception exception, IntPtr exceptionPointers)
		{
			lock (m_exceptionSyncRoot)
			{
				bool flag = ErrorPlatform.IsCriticalMemory || IsOom(exception);
				if (flag)
				{
					MyOutOfMemoryException ex = new MyOutOfMemoryException();
					LogException(ex);
					if (exception != null)
					{
						MySandboxGame.Log.AppendToClosedLog("Exception source:");
						MySandboxGame.Log.AppendToClosedLog(Convert.ToBase64String(Encoding.UTF8.GetBytes(exception.ToString())));
					}
					exception = ex;
				}
				else
				{
					LogException(exception);
				}
				MySandboxGame.Log.AppendToClosedLog("Showing message");
				if (!Sandbox.Engine.Platform.Game.IsDedicated || MyPerGameSettings.SendLogToKeen)
				{
					OnCrash(MySandboxGame.Log.GetFilePath(), MyPerGameSettings.GameName, MyPerGameSettings.MinimumRequirementsPage, MyPerGameSettings.RequiresDX11, exception, exceptionPointers, flag);
				}
				MySandboxGame.Log.Close();
				MySimpleProfiler.LogPerformanceTestResults();
				if (!Debugger.IsAttached)
				{
					AppDomain.get_CurrentDomain().remove_UnhandledException((UnhandledExceptionEventHandler)UnhandledExceptionHandler);
					ErrorPlatform.SetNativeExceptionHandler(null);
					ErrorPlatform.ExitProcessOnCrash(exception);
				}
			}
<<<<<<< HEAD
			bool IsOom(Exception e)
=======
			static bool IsOom(Exception e)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (!(e is OutOfMemoryException) && ErrorPlatform.GetExceptionType(e) != ExceptionType.OutOfMemory && (e.InnerException == null || !IsOom(e.InnerException)))
				{
					TaskException ex6;
					if ((ex6 = e as TaskException) != null)
					{
<<<<<<< HEAD
						return ex6.InnerExceptions.Any(IsOom);
=======
						return Enumerable.Any<Exception>((IEnumerable<Exception>)ex6.InnerExceptions, (Func<Exception, bool>)IsOom);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					return false;
				}
				return true;
			}
<<<<<<< HEAD
			void LogException(Exception e)
=======
			static void LogException(Exception e)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (e != null)
				{
					TaskException ex2;
					if ((ex2 = e as TaskException) != null)
					{
						TaskException ex3 = ex2;
						Exception[] innerExceptions = ex3.InnerExceptions;
						for (int i = 0; i < innerExceptions.Length; i++)
						{
							LogException(innerExceptions[i]);
						}
<<<<<<< HEAD
						MyVRage.Platform.System.LogToExternalDebugger("Task Exception Stack:" + Environment.NewLine + ex3.StackTrace);
						MySandboxGame.Log.AppendToClosedLog("Task Exception Stack:" + Environment.NewLine + ex3.StackTrace);
=======
						MyVRage.Platform.System.LogToExternalDebugger("Task Exception Stack:" + Environment.get_NewLine() + ex3.StackTrace);
						MySandboxGame.Log.AppendToClosedLog("Task Exception Stack:" + Environment.get_NewLine() + ex3.StackTrace);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						return;
					}
					MyNativeException ex4;
					if ((ex4 = e as MyNativeException) != null)
					{
						MyNativeException ex5 = ex4;
<<<<<<< HEAD
						MyVRage.Platform.System.LogToExternalDebugger("Unhandled native exception: " + Environment.NewLine + ex5);
						MySandboxGame.Log.AppendToClosedLog("Native exception occured: " + Environment.NewLine + ex5);
=======
						MyVRage.Platform.System.LogToExternalDebugger("Unhandled native exception: " + Environment.get_NewLine() + ex5);
						MySandboxGame.Log.AppendToClosedLog("Native exception occured: " + Environment.get_NewLine() + ex5);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						return;
					}
				}
				if (e != null)
				{
<<<<<<< HEAD
					MyVRage.Platform.System.LogToExternalDebugger("Unhandled managed exception: " + Environment.NewLine + e);
=======
					MyVRage.Platform.System.LogToExternalDebugger("Unhandled managed exception: " + Environment.get_NewLine() + e);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					MySandboxGame.Log.AppendToClosedLog(e);
					HandleSpecialExceptions(e);
				}
			}
		}

		private static void HandleSpecialExceptions(Exception exception)
		{
			if (exception == null)
			{
				return;
			}
			ReflectionTypeLoadException ex;
			if ((ex = exception as ReflectionTypeLoadException) == null)
			{
				OutOfMemoryException ex2;
				if ((ex2 = exception as OutOfMemoryException) != null)
				{
					OutOfMemoryException e = ex2;
					MySandboxGame.Log.AppendToClosedLog("Handling out of memory exception... " + MySandboxGame.Config);
					if (MySandboxGame.Config.LowMemSwitchToLow == MyConfig.LowMemSwitch.ARMED && !MySandboxGame.Config.IsSetToLowQuality())
					{
						MySandboxGame.Log.AppendToClosedLog("Creating switch to low request");
						MySandboxGame.Config.LowMemSwitchToLow = MyConfig.LowMemSwitch.TRIGGERED;
						MySandboxGame.Config.Save();
						MySandboxGame.Log.AppendToClosedLog("Switch to low request created");
					}
					MySandboxGame.Log.AppendToClosedLog(e);
				}
			}
			else
			{
				Exception[] loaderExceptions = ex.LoaderExceptions;
				foreach (Exception e2 in loaderExceptions)
				{
					MySandboxGame.Log.AppendToClosedLog(e2);
				}
			}
			HandleSpecialExceptions(exception.InnerException);
		}

		private static bool IsModCrash(Exception e)
		{
			return e is ModCrashedException;
		}

		private static void OnCrash(string logPath, string gameName, string minimumRequirementsPage, bool requiresDX11, Exception exception, IntPtr exceptionPointers, bool oom)
		{
			try
			{
				ExceptionType exceptionType = ErrorPlatform.GetExceptionType(exception);
				if (MyVideoSettingsManager.GpuUnderMinimum)
				{
					MyErrorReporter.ReportGpuUnderMinimumCrash(gameName, logPath, minimumRequirementsPage);
					return;
				}
				if (!Sandbox.Engine.Platform.Game.IsDedicated && exceptionType == ExceptionType.OutOfMemory)
				{
					MyErrorReporter.ReportOutOfMemory(gameName, logPath, minimumRequirementsPage);
					return;
				}
				if (!Sandbox.Engine.Platform.Game.IsDedicated && exceptionType == ExceptionType.OutOfVideoMemory)
				{
					MyErrorReporter.ReportOutOfVideoMemory(gameName, logPath, minimumRequirementsPage);
					return;
				}
				bool flag = false;
				if (exception != null && exception.Data.Contains("Silent"))
				{
					flag = Convert.ToBoolean(exception.Data["Silent"]);
				}
				if (MyFakes.ENABLE_MINIDUMP_SENDING && exceptionType != ExceptionType.OutOfMemory)
				{
					MyMiniDump.CollectCrashDump(exceptionPointers);
				}
				if (!flag && !Debugger.IsAttached)
				{
					if (IsModCrash(exception))
					{
						ModCrashedException ex = (ModCrashedException)exception;
						MyModCrashScreenTexts myModCrashScreenTexts = default(MyModCrashScreenTexts);
						myModCrashScreenTexts.ModName = ex.ModContext.ModName;
						myModCrashScreenTexts.ModId = ex.ModContext.ModId;
						myModCrashScreenTexts.ModServiceName = ex.ModContext.ModServiceName;
						myModCrashScreenTexts.LogPath = logPath;
						myModCrashScreenTexts.Close = MyTexts.GetString(MyCommonTexts.Close);
						myModCrashScreenTexts.Text = MyTexts.GetString(MyCommonTexts.ModCrashedTheGame);
						myModCrashScreenTexts.Info = MyTexts.GetString(MyCommonTexts.ModCrashedTheGameInfo);
						MyModCrashScreenTexts texts = myModCrashScreenTexts;
						ErrorPlatform.MessageBoxModCrashForm(ref texts);
					}
					else
					{
						CrashInfo crashInfo = GetCrashInfo(exception, oom, exceptionType);
						MySandboxGame.Log.AppendToClosedLog("\n" + crashInfo);
						CLoseLog(MySandboxGame.Log);
						CLoseLog(MyRenderProxy.Log);
						bool gDPRConsent = MySandboxGame.Config?.GDPRConsent.GetValueOrDefault(false) ?? true;
						ErrorPlatform.PrepareCrashAnalyticsReporting(logPath, gDPRConsent, crashInfo, requiresDX11 && exceptionType == ExceptionType.UnsupportedGpu);
					}
				}
			}
			catch (Exception ex2)
			{
				MyLog.Default.WriteLineAndConsole("Exception while reporting crash.");
				MyLog.Default.WriteLineAndConsole(ex2.ToString());
			}
			finally
			{
				if (!Debugger.IsAttached)
				{
					try
					{
						if (MySpaceAnalytics.Instance != null)
						{
							MySpaceAnalytics.Instance.ReportSessionEndByCrash(exception);
						}
					}
					catch
					{
					}
				}
			}
			static void CLoseLog(MyLog log)
			{
				try
				{
					log.Flush();
					log.Close();
				}
				catch
				{
				}
			}
		}

		private static CrashInfo GetCrashInfo(Exception exception, bool oom, ExceptionType et)
		{
			CrashInfo result = MyErrorReporter.BuildCrashInfo();
			result.IsGPU = et == ExceptionType.DriverCrash;
			result.IsOutOfMemory = oom;
			result.IsNative = exception is MyNativeException;
			result.IsTask = exception is TaskException;
			return result;
		}
	}
}
