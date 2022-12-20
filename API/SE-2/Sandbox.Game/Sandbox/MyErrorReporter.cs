using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using LitJson;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Http;
using VRage.Utils;
using VRageRender;

namespace Sandbox
{
	public class MyErrorReporter
	{
		public struct SessionMetadata
		{
			public static readonly SessionMetadata DEFAULT = new SessionMetadata
			{
				UniqueUserIdentifier = Guid.NewGuid().ToString(),
				SessionId = string.Empty
			};

			public string UniqueUserIdentifier;

			public string SessionId;
		}

		private class MyCrashInfo
		{
			public string ReportVersion = "1.0";

			public string UniqueUserIdentifier = string.Empty;

			public long SessionID;

			public string GameID = string.Empty;

			public string GameVersion = string.Empty;

			public string Feedback = string.Empty;

			public string Email = string.Empty;

			public string ReportType = string.Empty;

			public bool IsOfficial;

			public string BranchName = string.Empty;

			public bool OOM;

			public bool IsGPU;

			public bool IsNative;

			public bool IsTask;

			public bool IsHang;

			public long ProcessRunTime;

			public int PCUCount;
		}

		public static string SUPPORT_EMAIL = "support@keenswh.com";

		public static string MESSAGE_BOX_CAPTION = "{LOCG:Error_Message_Caption}";

		public static string APP_ALREADY_RUNNING = "{LOCG:Error_AlreadyRunning}";

		public static string APP_ERROR_CAPTION = "{LOCG:Error_Error_Caption}";

		public static string APP_LOG_REPORT_FAILED = "{LOCG:Error_Failed}";

		public static string APP_LOG_REPORT_THANK_YOU = "{LOCG:Error_ThankYou}";

		public static string APP_ERROR_MESSAGE = "{LOCG:Error_Error_Message}";

		public static string APP_ERROR_MESSAGE_DX11_NOT_AVAILABLE = "{LOCG:Error_DX11}";

		public static string APP_ERROR_MESSAGE_LOW_GPU = "{LOCG:Error_GPU_Low}";

		public static string APP_ERROR_MESSAGE_NOT_DX11_GPU = "{LOCG:Error_GPU_NotDX11}";

		public static string APP_ERROR_MESSAGE_DRIVER_NOT_INSTALLED = "{LOCG:Error_GPU_Drivers}";

		public static string APP_WARNING_MESSAGE_OLD_DRIVER = "{LOCG:Error_GPU_OldDriver}";

		public static string APP_WARNING_MESSAGE_UNSUPPORTED_GPU = "{LOCG:Error_GPU_Unsupported}";

		public static string APP_ERROR_OUT_OF_MEMORY = "{LOCG:Error_OutOfMemmory}";

		public static string APP_ERROR_OUT_OF_VIDEO_MEMORY = "{LOCG:Error_GPU_OutOfMemory}";

		private static bool AllowSendDialog(string gameName, string logfile, string errorMessage)
		{
			return MyMessageBox.Show(string.Format(errorMessage, gameName, logfile), gameName, MessageBoxOptions.IconExclamation | MessageBoxOptions.YesNo) == MessageBoxResult.Yes;
		}

		public static void ReportRendererCrash(string logfile, string gameName, string minimumRequirementsPage, MyRenderExceptionEnum type)
		{
			MyMessageBox.Show(string.Format(type switch
			{
				MyRenderExceptionEnum.GpuNotSupported => MyTexts.SubstituteTexts(APP_ERROR_MESSAGE_LOW_GPU).ToString().Replace("\\n", "\r\n"), 
				MyRenderExceptionEnum.DriverNotInstalled => MyTexts.SubstituteTexts(APP_ERROR_MESSAGE_DRIVER_NOT_INSTALLED).ToString().Replace("\\n", "\r\n"), 
				_ => MyTexts.SubstituteTexts(APP_ERROR_MESSAGE_LOW_GPU).ToString().Replace("\\n", "\r\n"), 
			}, logfile, gameName, minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
		}

		public static MessageBoxResult ReportOldDrivers(string gameName, string cardName, string driverUpdateLink)
		{
			return MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(APP_WARNING_MESSAGE_OLD_DRIVER).ToString().Replace("\\n", "\r\n"), gameName, cardName, driverUpdateLink), gameName, MessageBoxOptions.YesNoCancel | MessageBoxOptions.IconExclamation);
		}

		public static void ReportNotCompatibleGPU(string gameName, string logfile, string minimumRequirementsPage)
		{
			MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(APP_WARNING_MESSAGE_UNSUPPORTED_GPU).ToString().Replace("\\n", "\r\n"), logfile, gameName, minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
		}

		public static void ReportNotDX11GPUCrash(string gameName, string logfile, string minimumRequirementsPage)
		{
			MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(APP_ERROR_MESSAGE_NOT_DX11_GPU).ToString().Replace("\\n", "\r\n"), logfile, gameName, minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
		}

		public static void ReportGpuUnderMinimumCrash(string gameName, string logfile, string minimumRequirementsPage)
		{
			MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(APP_ERROR_MESSAGE_LOW_GPU).ToString().Replace("\\n", "\r\n"), logfile, gameName, minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
		}

		public static void ReportOutOfMemory(string gameName, string logfile, string minimumRequirementsPage)
		{
			MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(APP_ERROR_OUT_OF_MEMORY).ToString().Replace("\\n", "\r\n"), logfile, gameName, minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
		}

		public static void ReportOutOfVideoMemory(string gameName, string logfile, string minimumRequirementsPage)
		{
			MyMessageBox.Show(string.Format(MyTexts.SubstituteTexts(APP_ERROR_OUT_OF_VIDEO_MEMORY).ToString().Replace("\\n", "\r\n"), logfile, gameName, minimumRequirementsPage), gameName, MessageBoxOptions.IconExclamation);
		}

		private static void MessageBox(string caption, string text)
		{
			MyMessageBox.Show(text, caption, MessageBoxOptions.OkOnly);
		}

		private static bool DisplayCommonError(string logContent)
		{
			ErrorInfo[] infos = MyErrorTexts.Infos;
			foreach (ErrorInfo errorInfo in infos)
			{
				if (logContent.Contains(errorInfo.Match))
				{
					MessageBox(errorInfo.Caption, errorInfo.Message);
					return true;
				}
			}
			return false;
		}

		private static bool LoadAndDisplayCommonError(string logName)
		{
			try
			{
				if (logName != null && File.Exists(logName))
				{
					using (FileStream stream = File.Open(logName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					{
						using StreamReader streamReader = new StreamReader(stream);
						return DisplayCommonError(streamReader.ReadToEnd());
					}
				}
			}
			catch
			{
			}
			return false;
		}

		private static void ReportInternal(string logName, string id, CrashInfo info, string email, string feedback)
		{
			if (TrySendReport(logName, id, email, feedback, info, "crash"))
			{
				MessageBox(info.GameName, MyTexts.SubstituteTexts(APP_LOG_REPORT_THANK_YOU).Replace("\\n", "\r\n"));
			}
			else
			{
				MessageBox(string.Format(MyTexts.SubstituteTexts(APP_ERROR_CAPTION).Replace("\\n", "\r\n"), info.GameName), string.Format(MyTexts.SubstituteTexts(APP_LOG_REPORT_FAILED).Replace("\\n", "\r\n"), info.GameName, logName, MyTexts.SubstituteTexts(SUPPORT_EMAIL)));
			}
		}

		private static HashSet<string> EnumerateLogs()
		{
<<<<<<< HEAD
			HashSet<string> hashSet = new HashSet<string>();
=======
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			HashSet<string> val = new HashSet<string>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			IEnumerable<string> enumerable = MyVRage.Platform.CrashReporting.AdditionalReportFiles();
			if (enumerable != null)
			{
				foreach (string item in enumerable)
				{
<<<<<<< HEAD
					hashSet.Add(item);
				}
			}
			FileInfo fileInfo = null;
			foreach (FileInfo item2 in new DirectoryInfo(MyFileSystem.UserDataPath).EnumerateFiles("*.log"))
			{
				if (item2.Name.StartsWith("VRageRender") && item2.Exists && (fileInfo == null || fileInfo.LastWriteTime < item2.LastWriteTime))
				{
					fileInfo = item2;
				}
				hashSet.Add(item2.FullName);
			}
			if (fileInfo != null)
			{
				hashSet.Remove(fileInfo.FullName);
			}
			hashSet.Remove(MySandboxGame.Log.GetFilePath());
			return hashSet;
		}

		public static void ReportNotInteractive(string logName, string id, bool includeAdditionalLogs, IEnumerable<string> additionalFiles, bool isCrash, string email, string feedback, CrashInfo info)
		{
			HashSet<string> hashSet = (includeAdditionalLogs ? EnumerateLogs() : null);
			if (additionalFiles != null)
			{
				if (hashSet == null)
				{
					hashSet = new HashSet<string>();
				}
				foreach (string additionalFile in additionalFiles)
				{
					hashSet.Add(additionalFile);
				}
			}
			TrySendReport(logName, id, email, feedback, info, isCrash ? "crash" : "log", hashSet);
		}

=======
					val.Add(item);
				}
			}
			FileInfo val2 = null;
			foreach (FileInfo item2 in new DirectoryInfo(MyFileSystem.UserDataPath).EnumerateFiles("*.log"))
			{
				if (((FileSystemInfo)item2).get_Name().StartsWith("VRageRender") && ((FileSystemInfo)item2).get_Exists() && (val2 == null || ((FileSystemInfo)val2).get_LastWriteTime() < ((FileSystemInfo)item2).get_LastWriteTime()))
				{
					val2 = item2;
				}
				val.Add(((FileSystemInfo)item2).get_FullName());
			}
			if (val2 != null)
			{
				val.Remove(((FileSystemInfo)val2).get_FullName());
			}
			val.Remove(MySandboxGame.Log.GetFilePath());
			return val;
		}

		public static void ReportNotInteractive(string logName, string id, bool includeAdditionalLogs, IEnumerable<string> additionalFiles, bool isCrash, string email, string feedback, CrashInfo info)
		{
			HashSet<string> val = (includeAdditionalLogs ? EnumerateLogs() : null);
			if (additionalFiles != null)
			{
				if (val == null)
				{
					val = new HashSet<string>();
				}
				foreach (string additionalFile in additionalFiles)
				{
					val.Add(additionalFile);
				}
			}
			TrySendReport(logName, id, email, feedback, info, isCrash ? "crash" : "log", (IEnumerable<string>)val);
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static void ReportGeneral(string logName, string id, CrashInfo info)
		{
			if (!string.IsNullOrWhiteSpace(logName) && !LoadAndDisplayCommonError(logName))
			{
				MyCrashScreenTexts myCrashScreenTexts;
				MyCrashScreenTexts texts;
				if (MyTexts.Exists(MyCoreTexts.CrashScreen_MainText))
				{
					myCrashScreenTexts = default(MyCrashScreenTexts);
					myCrashScreenTexts.GameName = info.GameName;
					myCrashScreenTexts.LogName = logName;
					myCrashScreenTexts.MainText = MyTexts.Get(MyCoreTexts.CrashScreen_MainText).ToString();
					myCrashScreenTexts.Log = MyTexts.Get(MyCoreTexts.CrashScreen_Log).ToString();
					myCrashScreenTexts.EmailText = string.Format(MyTexts.Get(MyCoreTexts.CrashScreen_EmailText).ToString(), MyGameService.Service?.ServiceName ?? "Steam");
					myCrashScreenTexts.Email = MyTexts.Get(MyCoreTexts.CrashScreen_Email).ToString();
					myCrashScreenTexts.Detail = MyTexts.Get(MyCoreTexts.CrashScreen_Detail).ToString();
					myCrashScreenTexts.Yes = MyTexts.Get(MyCoreTexts.CrashScreen_Yes).ToString();
					texts = myCrashScreenTexts;
				}
				else
				{
					myCrashScreenTexts = default(MyCrashScreenTexts);
					myCrashScreenTexts.GameName = info.GameName;
					myCrashScreenTexts.LogName = logName;
					myCrashScreenTexts.MainText = "Space Engineers had a problem and crashed! We apologize for the inconvenience. Please click Send Log if you would like to help us analyze and fix the problem. For more information, check the log below";
					myCrashScreenTexts.Log = "log";
					myCrashScreenTexts.EmailText = "Additionally, you can send us your email in case a member of our support staff needs more information about this error. \r\n \r\n If you would not mind being contacted about this issue please provide your e-mail address below. By sending the log, I grant my consent to the processing of my personal data (E-mail, Steam ID and IP address) to Keen SWH LTD. United Kingdom and it subsidiaries, in order for these data to be processed for the purpose of tracking the crash and requesting feedback with the intent to improve the game performance. I grant this consent for an indefinite term until my express revocation thereof. I confirm that I have been informed that the provision of these data is voluntary, and that I have the right to request their deletion. Registration is non-transferable. More information about the processing of my personal data in the scope required by legal regulations, in particular Regulation (EU) 2016/679 of the European Parliament and of the Council, can be found as of 25 May 2018 here. \r\n";
					myCrashScreenTexts.Email = "Email (optional)";
					myCrashScreenTexts.Detail = "To help us resolve the problem, please provide a description of what you were doing when it occurred (optional)";
					myCrashScreenTexts.Yes = "Send Log";
					texts = myCrashScreenTexts;
				}
				if (MyVRage.Platform.CrashReporting.MessageBoxCrashForm(ref texts, out var message, out var email))
				{
					ReportInternal(logName, id, info, email, message);
				}
			}
		}

		public static void Report(string logName, string id, CrashInfo info, string errorMessage)
		{
			if (!LoadAndDisplayCommonError(logName) && AllowSendDialog(info.GameName, logName, errorMessage) && logName != null)
			{
				ReportInternal(logName, id, info, string.Empty, string.Empty);
			}
		}

		public static void ReportAppAlreadyRunning(string gameName)
		{
			MyVRage.Platform.Windows.MessageBox(string.Format(MyTexts.SubstituteTexts(APP_ALREADY_RUNNING).Replace("\\n", "\r\n"), gameName), string.Format(MyTexts.SubstituteTexts(MESSAGE_BOX_CAPTION).Replace("\\n", "\r\n"), gameName), MessageBoxOptions.OkOnly);
		}

		private static bool TrySendReport(string logName, string gameId, string email, string feedback, CrashInfo info, string reportType, IEnumerable<string> files = null)
		{
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrWhiteSpace(email))
			{
				stringBuilder.AppendLine("Email: " + email);
			}
			if (!string.IsNullOrWhiteSpace(feedback))
			{
				stringBuilder.AppendLine("Feedback: " + feedback);
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine();
			}
			SessionMetadata metadata = SessionMetadata.DEFAULT;
			Tuple<string, string> tuple = null;
			try
			{
				string text = stringBuilder.ToString();
				if (logName != null && File.Exists(logName))
				{
					using FileStream stream = File.Open(logName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					using StreamReader streamReader = new StreamReader(stream);
					text += streamReader.ReadToEnd();
				}
				string text2 = Path.GetFileName(logName);
				int num = text2.IndexOf("_");
				if (num >= 0)
				{
					text2 = text2.Substring(0, num);
					text2 += ".log";
				}
				tuple = new Tuple<string, string>(text2, text);
				TryExtractMetadataFromLog(tuple, ref metadata);
			}
			catch
			{
				return false;
			}
			try
			{
				string text3 = stringBuilder.ToString();
<<<<<<< HEAD
				FileInfo fileInfo = null;
				foreach (FileInfo item in new DirectoryInfo(Path.GetDirectoryName(logName)).EnumerateFiles("VRageRender*.log"))
				{
					if (item.Exists && (fileInfo == null || fileInfo.LastWriteTime < item.LastWriteTime))
					{
						fileInfo = item;
					}
				}
				if (fileInfo != null)
				{
					using (FileStream stream2 = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					{
						using (StreamReader streamReader2 = new StreamReader(stream2))
						{
							text3 += streamReader2.ReadToEnd();
=======
				FileInfo val = null;
				foreach (FileInfo item in new DirectoryInfo(Path.GetDirectoryName(logName)).EnumerateFiles("VRageRender*.log"))
				{
					if (((FileSystemInfo)item).get_Exists() && (val == null || ((FileSystemInfo)val).get_LastWriteTime() < ((FileSystemInfo)item).get_LastWriteTime()))
					{
						val = item;
					}
				}
				if (val != null)
				{
					using FileStream stream2 = val.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					using StreamReader streamReader2 = new StreamReader(stream2);
					text3 += streamReader2.ReadToEnd();
				}
				dictionary["VRageRender.log"] = Encoding.UTF8.GetBytes(text3);
			}
			catch
			{
			}
			try
			{
				string text4 = Path.Combine(Path.GetDirectoryName(logName), MyPerGameSettings.BasicGameInfo.ApplicationName + ".cfg");
				string text5 = stringBuilder.ToString();
				if (File.Exists(text4))
				{
					using (FileStream stream3 = File.Open(text4, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					{
						using StreamReader streamReader3 = new StreamReader(stream3);
						text5 += streamReader3.ReadToEnd();
					}
					dictionary[Path.GetFileName(text4)] = Encoding.UTF8.GetBytes(text5);
				}
			}
			catch
			{
			}
			try
			{
				if (files != null)
				{
					using MemoryStream memoryStream = new MemoryStream();
					bool flag = false;
					using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
					{
						foreach (string file in files)
						{
							flag = true;
							using Stream destination = zipArchive.CreateEntry(Path.GetFileName(file)).Open();
							using FileStream fileStream = File.Open(file, FileMode.Open);
							fileStream.CopyTo(destination);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					if (flag)
					{
						byte[] array2 = (dictionary["AdditionalFiles.zip"] = memoryStream.ToArray());
					}
				}
<<<<<<< HEAD
				dictionary["VRageRender.log"] = Encoding.UTF8.GetBytes(text3);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch
			{
			}
			try
			{
				string path = Path.Combine(Path.GetDirectoryName(logName), MyPerGameSettings.BasicGameInfo.ApplicationName + ".cfg");
				string text4 = stringBuilder.ToString();
				if (File.Exists(path))
				{
					using (FileStream stream3 = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					{
						using (StreamReader streamReader3 = new StreamReader(stream3))
						{
							text4 += streamReader3.ReadToEnd();
						}
					}
					dictionary[Path.GetFileName(path)] = Encoding.UTF8.GetBytes(text4);
				}
			}
			catch
			{
			}
			try
			{
				if (files != null)
				{
					using MemoryStream memoryStream2 = new MemoryStream();
					bool flag2 = false;
					using (ZipArchive zipArchive2 = new ZipArchive(memoryStream2, ZipArchiveMode.Create, leaveOpen: true))
					{
						foreach (string item2 in MyMiniDump.FindActiveDumps(Path.GetDirectoryName(logName)))
						{
<<<<<<< HEAD
							foreach (string file in files)
							{
								flag = true;
								using (Stream destination = zipArchive.CreateEntry(Path.GetFileName(file)).Open())
								{
									using (FileStream fileStream = File.Open(file, FileMode.Open))
									{
										fileStream.CopyTo(destination);
									}
								}
							}
						}
						if (flag)
						{
							byte[] array2 = (dictionary["AdditionalFiles.zip"] = memoryStream.ToArray());
						}
					}
				}
			}
			catch
			{
			}
			if (MyFakes.ENABLE_MINIDUMP_SENDING)
			{
				try
				{
					using (MemoryStream memoryStream2 = new MemoryStream())
					{
						bool flag2 = false;
						using (ZipArchive zipArchive2 = new ZipArchive(memoryStream2, ZipArchiveMode.Create, leaveOpen: true))
						{
							foreach (string item2 in MyMiniDump.FindActiveDumps(Path.GetDirectoryName(logName)))
							{
								flag2 = true;
								using (Stream destination2 = zipArchive2.CreateEntry(Path.GetFileName(item2)).Open())
								{
									using (FileStream fileStream2 = File.Open(item2, FileMode.Open))
									{
										fileStream2.CopyTo(destination2);
									}
								}
							}
						}
						if (flag2)
						{
							byte[] array4 = (dictionary["Minidumps.zip"] = memoryStream2.ToArray());
=======
							flag2 = true;
							using Stream destination2 = zipArchive2.CreateEntry(Path.GetFileName(item2)).Open();
							using FileStream fileStream2 = File.Open(item2, FileMode.Open);
							fileStream2.CopyTo(destination2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					if (flag2)
					{
						byte[] array4 = (dictionary["Minidumps.zip"] = memoryStream2.ToArray());
					}
				}
				catch
				{
				}
			}
			return TrySendToOpicka(ref metadata, gameId, tuple, dictionary, email, feedback, info, reportType);
		}

		private static void TryExtractMetadataFromLog(Tuple<string, string> mainLog, ref SessionMetadata metadata)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			string uniqueUserIdentifier = metadata.UniqueUserIdentifier;
			string sessionId = metadata.SessionId;
			try
			{
				StringReader val = new StringReader(mainLog.Item2);
				try
				{
					for (string text = ((TextReader)(object)val).ReadLine(); text != null; text = ((TextReader)(object)val).ReadLine())
					{
						int num = text.IndexOf("Analytics uuid:");
						if (num >= 0)
						{
							uniqueUserIdentifier = text.Substring(num + "Analytics uuid:".Length).Trim();
						}
						num = text.IndexOf("Analytics session:");
						if (num >= 0)
						{
							sessionId = text.Substring(num + "Analytics session:".Length).Trim();
						}
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch
			{
			}
			metadata.UniqueUserIdentifier = uniqueUserIdentifier;
			metadata.SessionId = sessionId;
		}

		/// <summary>
		/// Submit crash reports to Hetzner, using the old reporting format.
		/// </summary>
		/// <returns>True if sent successfully, false otherwise.</returns>
		private static bool TrySendToHetzner(ref SessionMetadata metadata, string gameId, Tuple<string, string> log, Dictionary<string, byte[]> additionalFiles, string email, string feedback)
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Invalid comparison between Unknown and I4
			if (log == null || string.IsNullOrWhiteSpace(log.Item1) || string.IsNullOrWhiteSpace(log.Item2) || string.IsNullOrWhiteSpace(gameId))
			{
				return false;
			}
			string value = "";
			if (additionalFiles.TryGetValue("VRageRender.log", out var value2))
			{
				value = Encoding.UTF8.GetString(value2);
			}
			additionalFiles.TryGetValue("Minidumps.zip", out var value3);
			try
			{
				string url = "https://minerwars.keenswh.com/SubmitLog.aspx?id=" + gameId;
				HttpStatusCode val = (HttpStatusCode)0;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
					binaryWriter.Write(log.Item2);
					binaryWriter.Write(value);
					if (MyFakes.ENABLE_MINIDUMP_SENDING && value3 != null)
					{
<<<<<<< HEAD
						binaryWriter.Write(log.Item2);
						binaryWriter.Write(value);
						if (MyFakes.ENABLE_MINIDUMP_SENDING && value3 != null)
						{
							binaryWriter.Write(value3.Length);
							binaryWriter.Write(value3);
						}
						HttpData[] parameters = new HttpData[2]
						{
							new HttpData("Content-Type", "application/octet-stream", HttpDataType.HttpHeader),
							new HttpData("application/octet-stream", memoryStream.ToArray(), HttpDataType.RequestBody)
						};
						httpStatusCode = MyVRage.Platform.Http.SendRequest(url, parameters, HttpMethod.POST, out var _);
=======
						binaryWriter.Write(value3.Length);
						binaryWriter.Write(value3);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					HttpData[] parameters = new HttpData[2]
					{
						new HttpData("Content-Type", "application/octet-stream", HttpDataType.HttpHeader),
						new HttpData("application/octet-stream", memoryStream.ToArray(), HttpDataType.RequestBody)
					};
					val = MyVRage.Platform.Http.SendRequest(url, parameters, HttpMethod.POST, out var _);
				}
				return (int)val == 200;
			}
			catch
			{
			}
			return false;
		}

<<<<<<< HEAD
		/// <summary>
		/// Submit crash reports to Opicka, using the new reporting format.
		/// </summary>
		/// <returns>True if sent successfully, false otherwise.</returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static bool TrySendToOpicka(ref SessionMetadata metadata, string id, Tuple<string, string> log, Dictionary<string, byte[]> additionalFiles, string email, string feedback, CrashInfo generalInfo, string reportType)
		{
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Invalid comparison between Unknown and I4
			if (log == null || string.IsNullOrWhiteSpace(log.Item1) || string.IsNullOrWhiteSpace(log.Item2) || string.IsNullOrWhiteSpace(id))
			{
				return false;
			}
			try
			{
				string value = JsonMapper.ToJson(new MyCrashInfo
				{
					UniqueUserIdentifier = metadata.UniqueUserIdentifier,
					SessionID = 0L,
					GameID = id,
					GameVersion = generalInfo.AppVersion,
					Email = email,
					Feedback = feedback,
					ReportType = reportType,
					IsHang = generalInfo.IsHang,
					OOM = generalInfo.IsOutOfMemory,
					IsNative = generalInfo.IsNative,
					IsGPU = generalInfo.IsGPU,
					IsTask = generalInfo.IsTask,
					PCUCount = generalInfo.PCUCount,
					ProcessRunTime = generalInfo.ProcessRunTime,
					IsOfficial = true,
					BranchName = MyGameService.BranchName
				});
				HttpStatusCode val = (HttpStatusCode)0;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
					binaryWriter.Write("metadata");
					binaryWriter.Write(value);
					binaryWriter.Write("log");
					binaryWriter.Write(log.Item1);
					binaryWriter.Write(log.Item2);
					foreach (KeyValuePair<string, byte[]> additionalFile in additionalFiles)
					{
<<<<<<< HEAD
						binaryWriter.Write("metadata");
						binaryWriter.Write(value);
						binaryWriter.Write("log");
						binaryWriter.Write(log.Item1);
						binaryWriter.Write(log.Item2);
						foreach (KeyValuePair<string, byte[]> additionalFile in additionalFiles)
						{
							binaryWriter.Write("file");
							binaryWriter.Write(additionalFile.Key);
							binaryWriter.Write(additionalFile.Value.Length);
							binaryWriter.Write(additionalFile.Value);
						}
						HttpData[] parameters = new HttpData[2]
						{
							new HttpData("Content-Type", "application/octet-stream", HttpDataType.HttpHeader),
							new HttpData("application/octet-stream", memoryStream.ToArray(), HttpDataType.RequestBody)
						};
						httpStatusCode = MyVRage.Platform.Http.SendRequest("https://crashlogs.keenswh.com/api/Report", parameters, HttpMethod.POST, out var _);
=======
						binaryWriter.Write("file");
						binaryWriter.Write(additionalFile.Key);
						binaryWriter.Write(additionalFile.Value.Length);
						binaryWriter.Write(additionalFile.Value);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					HttpData[] parameters = new HttpData[2]
					{
						new HttpData("Content-Type", "application/octet-stream", HttpDataType.HttpHeader),
						new HttpData("application/octet-stream", memoryStream.ToArray(), HttpDataType.RequestBody)
					};
					val = MyVRage.Platform.Http.SendRequest("https://crashlogs.keenswh.com/api/Report", parameters, HttpMethod.POST, out var _);
				}
				return (int)val == 200;
			}
			catch
			{
			}
			return false;
		}

		public static CrashInfo BuildCrashInfo()
		{
			MySession @static = MySession.Static;
			MyVRage.Platform.System.GetGCMemory(out var allocated, out var used);
			CrashInfo result = new CrashInfo(MyFinalBuildConstants.APP_VERSION_STRING.ToString(), MyPerGameSettings.BasicGameInfo.GameName, MyPerGameSettings.BasicGameInfo.AnalyticId);
			result.GCMemory = (long)used;
			result.GCMemoryAllocated = (long)allocated;
			result.HWAvailableMemory = (long)MyVRage.Platform.System.RAMCounter;
			result.ProcessPrivateMemory = MyVRage.Platform.System.ProcessPrivateMemory / 1024 / 1024;
			result.PCUCount = @static?.TotalSessionPCU ?? 0;
			result.IsExperimental = @static?.IsSettingsExperimental() ?? false;
<<<<<<< HEAD
			result.ProcessRunTime = (long)(DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;
=======
			result.ProcessRunTime = (long)(DateTime.Now - Process.GetCurrentProcess().get_StartTime()).TotalSeconds;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return result;
		}

		public static void UpdateHangAnalytics()
		{
			CrashInfo hangInfo = BuildCrashInfo();
			hangInfo.IsHang = true;
			bool gDPRConsent = MySandboxGame.Config?.GDPRConsent.GetValueOrDefault(false) ?? true;
			MyVRage.Platform.CrashReporting.UpdateHangAnalytics(hangInfo, MyLog.Default.GetFilePath(), gDPRConsent);
		}
	}
}
