using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using VRage.FileSystem;
using VRage.GameServices;
using VRage.Mod.Io.Data;
using VRage.Utils;

namespace VRage.Mod.Io
{
	internal static class MyModIoCache
	{
		private class MyDownloadInfo
		{
			public string Url;

			public ulong Size;

			public ulong Position;

			public string FilePath;

			public string TempFilePath;

			public DateTime DateAdded;

			public event Action<MyGameServiceCallResult> Response;

			public void InvokeResponse(MyGameServiceCallResult result)
			{
				this.Response.InvokeIfNotNull(result);
			}
		}

		private static readonly ConcurrentDictionary<string, MyDownloadInfo> m_downloadInfos = new ConcurrentDictionary<string, MyDownloadInfo>();

		public static MyWorkshopItemState GetItemState(Modfile modFile)
		{
<<<<<<< HEAD
=======
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyWorkshopItemState myWorkshopItemState = MyWorkshopItemState.None;
			if (m_downloadInfos.ContainsKey(modFile.download.binary_url))
			{
				myWorkshopItemState |= MyWorkshopItemState.Downloading | MyWorkshopItemState.DownloadPending;
			}
			string filePath = GetFilePath(modFile);
			if (File.Exists(filePath))
			{
				myWorkshopItemState |= MyWorkshopItemState.Installed;
<<<<<<< HEAD
				if (File.GetCreationTimeUtc(filePath).ToUnixTimestamp() != modFile.date_added || new FileInfo(filePath).Length != modFile.filesize)
=======
				if (File.GetCreationTimeUtc(filePath).ToUnixTimestamp() != modFile.date_added || new FileInfo(filePath).get_Length() != modFile.filesize)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					myWorkshopItemState |= MyWorkshopItemState.NeedsUpdate;
				}
			}
			return myWorkshopItemState;
		}

		public static MyWorkshopItemState GetItemState(ulong modId, string url, uint timeStamp)
		{
<<<<<<< HEAD
=======
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyWorkshopItemState myWorkshopItemState = MyWorkshopItemState.None;
			if (m_downloadInfos.ContainsKey(url))
			{
				myWorkshopItemState |= MyWorkshopItemState.Downloading | MyWorkshopItemState.DownloadPending;
			}
			string itemFilePath = GetItemFilePath(modId, Path.GetExtension(url));
			if (File.Exists(itemFilePath))
			{
				myWorkshopItemState |= MyWorkshopItemState.Installed;
<<<<<<< HEAD
				if (File.GetCreationTimeUtc(itemFilePath).ToUnixTimestamp() != timeStamp || new FileInfo(itemFilePath).Length == 0L)
=======
				if (File.GetCreationTimeUtc(itemFilePath).ToUnixTimestamp() != timeStamp || new FileInfo(itemFilePath).get_Length() == 0L)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					myWorkshopItemState |= MyWorkshopItemState.NeedsUpdate;
				}
			}
			return myWorkshopItemState;
		}

		public static string GetItemFilePath(ulong modId, string url)
		{
			return GetFilePath(modId + Path.GetExtension(url));
		}

		public static bool GetItemInstallInfo(Modfile modFile, out ulong size, out string folder, out uint timeStamp)
		{
<<<<<<< HEAD
			if (GetItemState(modFile).HasFlag(MyWorkshopItemState.Installed))
			{
				string filePath = GetFilePath(modFile);
				FileInfo fileInfo = new FileInfo(filePath);
				size = (ulong)fileInfo.Length;
				folder = filePath;
				timeStamp = fileInfo.CreationTimeUtc.ToUnixTimestamp();
=======
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			if (GetItemState(modFile).HasFlag(MyWorkshopItemState.Installed))
			{
				string filePath = GetFilePath(modFile);
				FileInfo val = new FileInfo(filePath);
				size = (ulong)val.get_Length();
				folder = filePath;
				timeStamp = ((FileSystemInfo)val).get_CreationTimeUtc().ToUnixTimestamp();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return true;
			}
			size = 0uL;
			folder = "";
			timeStamp = 0u;
			return false;
		}

		private static string GetFilePath(Modfile modFile)
		{
			return GetFilePath(modFile.mod_id + ".zip");
		}

		private static string GetFilePath(string fileName)
		{
			return Path.Combine(MyFileSystem.CachePath, fileName);
		}

		private static void DownloadItem(string fileName, string url, uint timeStamp, ulong fileSize, Action<MyGameServiceCallResult> response)
		{
<<<<<<< HEAD
			if (m_downloadInfos.TryGetValue(url, out var value))
			{
				value.Response += response;
=======
			MyDownloadInfo myDownloadInfo = default(MyDownloadInfo);
			if (m_downloadInfos.TryGetValue(url, ref myDownloadInfo))
			{
				myDownloadInfo.Response += response;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return;
			}
			string filePath = GetFilePath(fileName);
			string text = filePath + ".download";
			try
			{
				Directory.CreateDirectory(MyFileSystem.CachePath);
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
				response.InvokeIfNotNull(MyGameServiceCallResult.AccessDenied);
				return;
			}
			MyDownloadInfo info = new MyDownloadInfo
			{
				Url = url,
				Size = fileSize,
				TempFilePath = text,
				FilePath = filePath,
				DateAdded = timeStamp.ToDateTimeFromUnixTimestamp()
			};
			info.Response += response;
			m_downloadInfos.TryAdd(url, info);
			MyModIo.DownloadFile(url, text, delegate(MyGameServiceCallResult x)
			{
				OnDownloadDone(x, info);
			}, delegate(ulong x)
			{
				OnDownloadProgress(x, info);
			});
		}

		public static void DownloadItem(ulong modId, string url, uint timeStamp, Action<MyGameServiceCallResult> response)
		{
			DownloadItem(GetItemFilePath(modId, url), url, timeStamp, 0uL, response);
		}

		public static void DownloadItem(Modfile modFile, Action<MyGameServiceCallResult> response)
		{
			DownloadItem(modFile.mod_id + ".zip", modFile.download.binary_url, (uint)modFile.date_added, (ulong)modFile.filesize, response);
		}

		private static void OnDownloadProgress(ulong downloadedSize, MyDownloadInfo info)
		{
			info.Position += downloadedSize;
		}

		private static void OnDownloadDone(MyGameServiceCallResult result, MyDownloadInfo info)
		{
			try
			{
				if (File.Exists(info.FilePath))
				{
					File.Delete(info.FilePath);
				}
				File.Move(info.TempFilePath, info.FilePath);
				File.SetCreationTimeUtc(info.FilePath, info.DateAdded);
			}
			catch
			{
				result = MyGameServiceCallResult.AccessDenied;
			}
<<<<<<< HEAD
			m_downloadInfos.Remove(info.Url);
=======
			m_downloadInfos.Remove<string, MyDownloadInfo>(info.Url);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			info.InvokeResponse(result);
		}

		public static void GetItemDownloadInfo(string url, out ulong bytesDownloaded, out ulong bytesTotal)
		{
<<<<<<< HEAD
			if (m_downloadInfos.TryGetValue(url, out var value))
			{
				bytesDownloaded = value.Position;
				bytesTotal = value.Size;
=======
			MyDownloadInfo myDownloadInfo = default(MyDownloadInfo);
			if (m_downloadInfos.TryGetValue(url, ref myDownloadInfo))
			{
				bytesDownloaded = myDownloadInfo.Position;
				bytesTotal = myDownloadInfo.Size;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				bytesDownloaded = 0uL;
				bytesTotal = 0uL;
			}
		}
	}
}
