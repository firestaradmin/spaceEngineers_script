using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ObjectBuilder;
using VRage.GameServices;
using VRage.Utils;
using VRageMath;

namespace Sandbox
{
	/// <summary>
	/// Serves as the main wrapper class that will be used for the program initialization.
	/// The individual games should use the services of this class and should not need to do things "on their own"
	/// </summary>
	public class MyCommonProgramStartup
	{
		private string[] m_args;

		private MyBasicGameInfo GameInfo => MyPerGameSettings.BasicGameInfo;

		public static void MessageBoxWrapper(string caption, string text)
		{
			MyVRage.Platform.Windows.MessageBox(text, caption, MessageBoxOptions.OkOnly);
		}

		public MyCommonProgramStartup(string[] args)
		{
			int? gameVersion = GameInfo.GameVersion;
			MyFinalBuildConstants.APP_VERSION = (gameVersion.HasValue ? ((MyVersion)gameVersion.GetValueOrDefault()) : null);
			m_args = args;
		}

		public bool PerformReporting(out CrashInfo crashInfo)
		{
			crashInfo = default(CrashInfo);
			try
			{
				if (MyVRage.Platform.CrashReporting.ExtractCrashAnalyticsReport(m_args, out var logPath, out crashInfo, out var isUnsupportedGpu, out var exitAfterReport))
				{
					if (isUnsupportedGpu)
					{
						string errorMessage = string.Format(MyTexts.SubstituteTexts(MyErrorReporter.APP_ERROR_MESSAGE_DX11_NOT_AVAILABLE).Replace("\\n", "\r\n"), m_args[1], m_args[2], GameInfo.MinimumRequirementsWeb);
						MyErrorReporter.Report(logPath, GameInfo.GameAcronym, crashInfo, errorMessage);
					}
					else
					{
						MyErrorReporter.ReportGeneral(logPath, GameInfo.GameAcronym, crashInfo);
					}
					return exitAfterReport;
				}
			}
			catch (Exception)
			{
			}
			return false;
		}

		public void PerformNotInteractiveReport(bool includeAdditionalLogs, IEnumerable<string> additionalFiles)
		{
			CrashInfo info = MyErrorReporter.BuildCrashInfo();
			MyErrorReporter.ReportNotInteractive(MyLog.Default.GetFilePath(), GameInfo.GameAcronym, includeAdditionalLogs, additionalFiles, isCrash: false, string.Empty, string.Empty, info);
		}

		public void PerformAutoconnect()
		{
			if (!MyFakes.ENABLE_CONNECT_COMMAND_LINE || !m_args.Contains("+connect"))
			{
				return;
			}
			int num = Array.IndexOf(m_args, "+connect");
			if (num + 1 < m_args.Length)
			{
				Sandbox.Engine.Platform.Game.ConnectToServer = m_args[num + 1];
				if (!string.IsNullOrEmpty(Sandbox.Engine.Platform.Game.ConnectToServer))
				{
					Console.WriteLine(GameInfo.GameName + " " + MyFinalBuildConstants.APP_VERSION_STRING);
					bool enabled = MyObfuscation.Enabled;
					Console.WriteLine("Obfuscated: " + enabled + ", Platform: " + (Environment.Is64BitProcess ? " 64-bit" : " 32-bit"));
					Console.WriteLine("Connecting to: " + m_args[num + 1]);
				}
			}
		}

		public bool PerformColdStart()
		{
<<<<<<< HEAD
			string path = Path.Combine(MyFileSystem.UserDataPath, "ColdStart.txt");
			if (m_args.Contains("-coldstart") || !File.Exists(path))
			{
				if (MyFakes.ENABLE_COLDSTART_ASSEMBLY_LOADING)
				{
					MyGlobalTypeMetadata.Static.Init(loadSerializersAsync: false);
					Parallel.Scheduler = new PrioritizedScheduler(1, amd: false);
					int num = -1;
					List<string> list = new List<string>();
					Queue<AssemblyName> queue = new Queue<AssemblyName>();
					queue.Enqueue(Assembly.GetEntryAssembly().GetName());
					while (queue.Count > 0)
					{
						AssemblyName assemblyName = queue.Dequeue();
						if (list.Contains(assemblyName.FullName))
						{
							continue;
						}
						list.Add(assemblyName.FullName);
						try
						{
							Assembly assembly = Assembly.Load(assemblyName);
							PreloadTypesFrom(assembly);
							num++;
							AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
							foreach (AssemblyName item in referencedAssemblies)
							{
								queue.Enqueue(item);
							}
						}
						catch (Exception)
=======
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Expected O, but got Unknown
			if (m_args.Contains("-coldstart"))
			{
				MyGlobalTypeMetadata.Static.Init(loadSerializersAsync: false);
				Parallel.Scheduler = new PrioritizedScheduler(1, amd: false);
				int num = -1;
				List<string> list = new List<string>();
				Queue<AssemblyName> val = new Queue<AssemblyName>();
				val.Enqueue(Assembly.GetEntryAssembly().GetName());
				while (val.get_Count() > 0)
				{
					AssemblyName assemblyName = val.Dequeue();
					if (list.Contains(assemblyName.FullName))
					{
						continue;
					}
					list.Add(assemblyName.FullName);
					try
					{
						Assembly assembly = Assembly.Load(assemblyName);
						PreloadTypesFrom(assembly);
						num++;
						AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
						foreach (AssemblyName assemblyName2 in referencedAssemblies)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							val.Enqueue(assemblyName2);
						}
					}
					catch (Exception)
					{
					}
				}
				if (MyFakes.ENABLE_NGEN && !File.Exists(path))
				{
					ProcessStartInfo val2 = new ProcessStartInfo(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "ngen"));
					val2.set_Verb("runas");
					ProcessStartInfo val3 = val2;
					val3.set_Arguments("install SpaceEngineers.exe /silent /nologo");
					val3.set_WindowStyle((ProcessWindowStyle)1);
					try
					{
						Process.Start(val3).WaitForExit();
					}
					catch (Exception ex2)
					{
						MyLog.Default.WriteLine("NGEN failed: " + ex2);
					}
					File.Create(path);
				}
				return true;
			}
			return false;
		}

		public bool VerboseNetworkLogging()
		{
			return m_args.Contains("-verboseNetworkLogging");
		}

		public bool UseEOS()
		{
			return m_args.Contains("-eos");
		}

		public bool IsRenderUpdateSyncEnabled()
		{
			return m_args.Contains("-render_sync");
		}

		public bool IsVideoRecordingEnabled()
		{
			return m_args.Contains("-video_record");
		}

		public bool IsGenerateDx11MipCache()
		{
			return m_args.Contains("-generateDx11MipCache");
		}

		private static void PreloadTypesFrom(Assembly assembly)
		{
			if (assembly != null)
			{
				ForceStaticCtor(Enumerable.ToArray<Type>(Enumerable.Where<Type>((IEnumerable<Type>)assembly.GetTypes(), (Func<Type, bool>)((Type type) => Attribute.IsDefined(type, typeof(PreloadRequiredAttribute))))));
			}
		}

		public static void ForceStaticCtor(Type[] types)
		{
			for (int i = 0; i < types.Length; i++)
			{
				RuntimeHelpers.RunClassConstructor(types[i].TypeHandle);
			}
		}

		/// <summary>
		/// Determines the application data path to use for configuration, save games and other dynamic data.
		/// </summary>        
		/// <returns></returns>
		public string GetAppDataPath()
		{
			string result = null;
			int num = Array.IndexOf(m_args, "-appdata") + 1;
			if (num != 0 && m_args.Length > num)
			{
				string text = m_args[num];
				if (!text.StartsWith("-"))
				{
					result = Path.GetFullPath(Environment.ExpandEnvironmentVariables(text));
				}
			}
			return result;
		}

		public void InitSplashScreen()
		{
			if (MyFakes.ENABLE_SPLASHSCREEN && !m_args.Contains("-nosplash"))
			{
				MyVRage.Platform.Windows.ShowSplashScreen(GameInfo.SplashScreenImage, new Vector2(0.7f, 0.7f));
			}
		}

		public void DisposeSplashScreen()
		{
			MyVRage.Platform.Windows.HideSplashScreen();
		}

		public bool Check64Bit()
		{
<<<<<<< HEAD
			if (!Environment.Is64BitProcess && AssemblyExtensions.TryGetArchitecture("SteamSDK.dll") == ProcessorArchitecture.Amd64)
			{
				string text = GameInfo.GameName + " cannot be started in 64-bit mode, ";
				text = text + "because 64-bit version of .NET framework is not available or is broken." + Environment.NewLine + Environment.NewLine;
				text = text + "Do you want to open website with more information about this particular issue?" + Environment.NewLine + Environment.NewLine;
				text = text + "Press Yes to open website with info" + Environment.NewLine;
				text = text + "Press No to run in 32-bit mode (smaller potential of " + GameInfo.GameName + "!)" + Environment.NewLine;
=======
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Expected O, but got Unknown
			if (!MyEnvironment.Is64BitProcess && AssemblyExtensions.TryGetArchitecture("SteamSDK.dll") == ProcessorArchitecture.Amd64)
			{
				string text = GameInfo.GameName + " cannot be started in 64-bit mode, ";
				text = text + "because 64-bit version of .NET framework is not available or is broken." + MyEnvironment.NewLine + MyEnvironment.NewLine;
				text = text + "Do you want to open website with more information about this particular issue?" + MyEnvironment.NewLine + MyEnvironment.NewLine;
				text = text + "Press Yes to open website with info" + MyEnvironment.NewLine;
				text = text + "Press No to run in 32-bit mode (smaller potential of " + GameInfo.GameName + "!)" + MyEnvironment.NewLine;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				text += "Press Cancel to close this dialog";
				switch (MyMessageBox.Show(text, ".NET Framework 64-bit error", MessageBoxOptions.YesNoCancel))
				{
				case MessageBoxResult.Yes:
					MyVRage.Platform.System.OpenUrl("http://www.spaceengineersgame.com/64-bit-start-up-issue.html");
					break;
				case MessageBoxResult.No:
				{
					string location = Assembly.GetEntryAssembly().Location;
<<<<<<< HEAD
					string text2 = Path.Combine(new FileInfo(location).Directory.Parent.FullName, "Bin", Path.GetFileName(location));
					Process.Start(new ProcessStartInfo
					{
						FileName = text2,
						WorkingDirectory = Path.GetDirectoryName(text2),
						Arguments = "-fallback",
						UseShellExecute = false,
						WindowStyle = ProcessWindowStyle.Normal
					});
=======
					string text2 = Path.Combine(((FileSystemInfo)new FileInfo(location).get_Directory().get_Parent()).get_FullName(), "Bin", Path.GetFileName(location));
					ProcessStartInfo val = new ProcessStartInfo();
					val.set_FileName(text2);
					val.set_WorkingDirectory(Path.GetDirectoryName(text2));
					val.set_Arguments("-fallback");
					val.set_UseShellExecute(false);
					val.set_WindowStyle((ProcessWindowStyle)0);
					Process.Start(val);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					break;
				}
				}
				return false;
			}
			return true;
		}

		public bool CheckSteamRunning()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				if (MyGameService.IsActive)
				{
					MyGameService.SetNotificationPosition(NotificationPosition.TopLeft);
					MySandboxGame.Log.WriteLineAndConsole("Service.IsActive: " + MyGameService.IsActive);
					MySandboxGame.Log.WriteLineAndConsole("Service.IsOnline: " + MyGameService.IsOnline);
					MySandboxGame.Log.WriteLineAndConsole("Service.OwnsGame: " + MyGameService.OwnsGame);
					MySandboxGame.Log.WriteLineAndConsole("Service.UserId: " + MyGameService.UserId);
					MySandboxGame.Log.WriteLineAndConsole("Service.UserName: " + MyGameService.UserName);
					MySandboxGame.Log.WriteLineAndConsole("Service.Branch: " + MyGameService.BranchName);
					MySandboxGame.Log.WriteLineAndConsole("Build date: " + MySandboxGame.BuildDateTime.ToString("yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture));
					MySandboxGame.Log.WriteLineAndConsole("Build version: " + MySandboxGame.BuildVersion.ToString());
				}
				else if ((!MyGameService.IsActive || !MyGameService.OwnsGame) && !MyFakes.ENABLE_RUN_WITHOUT_STEAM)
				{
<<<<<<< HEAD
					MessageBoxWrapper(MyGameService.Service.ServiceName + " is not running!", "Please run this game from " + MyGameService.Service.ServiceName + "." + Environment.NewLine + "(restart Steam if already running)");
=======
					MessageBoxWrapper(MyGameService.Service.ServiceName + " is not running!", "Please run this game from " + MyGameService.Service.ServiceName + "." + MyEnvironment.NewLine + "(restart Steam if already running)");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					return false;
				}
			}
			return true;
		}
	}
}
