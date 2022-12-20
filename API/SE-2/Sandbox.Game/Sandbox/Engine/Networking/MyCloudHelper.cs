using System;
using System.Collections.Generic;
using System.IO;
using Sandbox.Game.Localization;
using VRage.FileSystem;
using VRage.GameServices;
using VRage.Utils;
using VRageRender;

namespace Sandbox.Engine.Networking
{
	public static class MyCloudHelper
	{
		public const string BLUEPRINT_CLOUD_DIRECTORY = "Blueprints/cloud";

		public const string SCRIPT_CLOUD_DIRECTORY = "Scripts/cloud";

		public const string WORLD_CLOUD_DIRECTORY = "Worlds/cloud";

		public const string CONFIG_CLOUD_DIRECTORY = "Config/cloud";

		public const string LAST_SESSION_CLOUD_DIRECTORY = "Session/cloud";

		private static string[] m_validPrefixes = new string[4] { "Blueprints/cloud", "Worlds/cloud", "Config/cloud", "Session/cloud" };

		public static void FixStorage()
		{
			List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles("");
			if (cloudFiles == null)
			{
				return;
			}
			bool flag = false;
			foreach (MyCloudFileInfo item in cloudFiles)
			{
				string[] array = item.Name.Split(new char[1] { '/' });
				if (array.Length < 2 || array[1] != "cloud")
				{
					if (!flag)
					{
						MyLog.Default.WriteLine("Invalid cloud filenames: (will be removed)");
					}
					MyLog.Default.WriteLine(item.Name);
					MyGameService.DeleteFromCloud(item.ContainerName);
					flag = true;
				}
			}
		}

		public static CloudResult CopyFiles(string oldSessionPath, string newSessionPath)
		{
			try
			{
				List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(oldSessionPath);
				if (cloudFiles == null || cloudFiles.Count == 0)
				{
					return CloudResult.Failed;
				}
				List<MyCloudFile> list = new List<MyCloudFile>();
				foreach (MyCloudFileInfo item in cloudFiles)
				{
					byte[] array = MyGameService.LoadFromCloud(item.Name);
					if (array != null)
					{
						string fileName = Path.GetFileName(item.Name);
						string text = Path.Combine(MyFileSystem.TempPath, fileName);
						File.WriteAllBytes(text, array);
						list.Add(new MyCloudFile(text));
					}
				}
				return MyGameService.SaveToCloud(newSessionPath, list);
			}
			catch
			{
				return CloudResult.Failed;
			}
		}

		public static bool Delete(string fileName)
		{
			return MyGameService.DeleteFromCloud(fileName);
		}

		public static CloudResult UploadFiles(string cloudPath, string sourcePath, bool compress)
		{
<<<<<<< HEAD
=======
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return UploadFiles(cloudPath, new DirectoryInfo(sourcePath), compress);
		}

		public static CloudResult UploadFiles(string cloudPath, DirectoryInfo sourceDirectory, bool compress)
		{
			Delete(cloudPath);
			List<MyCloudFile> list = new List<MyCloudFile>();
			FileInfo[] files = sourceDirectory.GetFiles();
<<<<<<< HEAD
			foreach (FileInfo fileInfo in files)
			{
				string extension = Path.GetExtension(fileInfo.FullName);
				list.Add(new MyCloudFile(fileInfo.FullName, compress && extension.ToLower() != ".vx2"));
=======
			foreach (FileInfo val in files)
			{
				string extension = Path.GetExtension(((FileSystemInfo)val).get_FullName());
				list.Add(new MyCloudFile(((FileSystemInfo)val).get_FullName(), compress && extension.ToLower() != ".vx2"));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return MyGameService.SaveToCloud(cloudPath, list);
		}

		public static bool ExtractFilesTo(string cloudPath, string filePath, bool unpack)
		{
			try
			{
				List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(cloudPath);
				if (cloudFiles == null || cloudFiles.Count == 0)
				{
					return false;
				}
				if (Directory.Exists(filePath))
				{
					string[] files = Directory.GetFiles(filePath);
					for (int i = 0; i < files.Length; i++)
					{
						File.Delete(files[i]);
					}
				}
				Directory.CreateDirectory(filePath);
				foreach (MyCloudFileInfo item in cloudFiles)
				{
					byte[] array = MyGameService.LoadFromCloud(item.Name);
					if (array == null)
					{
						continue;
					}
					string fileName = Path.GetFileName(item.Name);
					string text = Path.Combine(filePath, fileName);
					if (unpack)
					{
<<<<<<< HEAD
						using (MemoryStream stream = new MemoryStream(array))
						{
							using (Stream stream2 = stream.UnwrapGZip())
							{
								using (FileStream destination = File.OpenWrite(text))
								{
									stream2.CopyTo(destination);
								}
							}
						}
=======
						using MemoryStream stream = new MemoryStream(array);
						using Stream stream2 = stream.UnwrapGZip();
						using FileStream destination = File.OpenWrite(text);
						stream2.CopyTo(destination);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					else
					{
						File.WriteAllBytes(text, array);
					}
					string text2 = Path.GetExtension(text).ToLower();
					if (text2 == ".jpg" || text2 == ".png")
					{
						MyRenderProxy.UnloadTexture(text);
					}
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		public static string LocalToCloudWorldPath(string localPath)
		{
			if (localPath.StartsWith(MyFileSystem.SavesPath))
			{
				return MyLocalCache.GetSessionSavesPath(localPath.Substring(MyFileSystem.SavesPath.Length).Trim('/', '\\').Replace('\\', ' ')
					.Replace('/', ' '), contentFolder: false, createIfNotExists: false, isCloud: true);
			}
			return MyLocalCache.GetSessionSavesPath(Path.GetFileName(Path.GetDirectoryName(localPath)), contentFolder: false, createIfNotExists: false, isCloud: true);
		}

		public static string CloudToLocalWorldPath(string cloudPath)
		{
			return MyLocalCache.GetSessionSavesPath(Path.GetFileName(Path.GetDirectoryName(cloudPath)), contentFolder: false);
		}

		public static string Combine(string container, string file)
		{
			if (container.EndsWith("/"))
			{
				return container + file;
			}
			return container + "/" + file;
		}

		public static ulong GetStorageSize(string containerName)
		{
			List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(containerName);
			ulong num = 0uL;
			foreach (MyCloudFileInfo item in cloudFiles)
			{
				num += (ulong)item.Size;
			}
			return num;
		}

		public static bool IsError(CloudResult error, out MyStringId errorMessage, MyStringId? defaultErrorMessage = null)
		{
			if (error == CloudResult.Ok)
			{
				errorMessage = MyStringId.NullOrEmpty;
				return false;
			}
			errorMessage = GetErrorMessage(error, defaultErrorMessage);
			return true;
		}

		public static MyStringId GetErrorMessage(CloudResult error, MyStringId? defaultErrorMessage = null)
		{
			switch (error)
			{
			default:
				return defaultErrorMessage ?? MySpaceTexts.MessageBoxWorldOperation_Error;
			case CloudResult.QuotaExceeded:
			case CloudResult.OutOfLocalStorage:
				return MySpaceTexts.MessageBoxWorldOperation_Quota;
			case CloudResult.SynchronizationFailure:
				return MySpaceTexts.MessageBoxWorldOperation_CloudSynchronization;
			}
		}

		public static string ChangeContainerName(string containerPath, string newName)
		{
			containerPath = containerPath.Trim(new char[1] { '/' });
			int num = containerPath.LastIndexOf('/');
			containerPath = ((num == -1) ? newName : (containerPath.Substring(0, num) + "/" + newName));
			return containerPath;
		}
	}
}
