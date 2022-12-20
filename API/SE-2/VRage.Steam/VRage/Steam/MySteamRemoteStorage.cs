using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Steamworks;
using VRage.GameServices;

namespace VRage.Steam
{
	internal class MySteamRemoteStorage
	{
		private class ReadInfo
		{
			public int Size { get; set; }

			public bool IsReadCompleted { get; set; }

			public byte[] Buffer { get; set; }

			public string FileName { get; set; }

			public bool IsResultOk { get; set; }

			public Action<bool> CompletedAction { get; set; }
		}

		private List<CallResult<RemoteStorageFileWriteAsyncComplete_t>> m_fileWriteComplete;

		private List<CallResult<RemoteStorageFileReadAsyncComplete_t>> m_fileReadComplete;

		private ConcurrentDictionary<string, MyCloudFileInfo> m_filesInfo;

		private ConcurrentDictionary<ulong, ReadInfo> m_readsInfo;

		private ConcurrentDictionary<string, ReadInfo> m_completeReadInfo;

		private Action<CloudResult> m_saveCompleteAction;

		public MySteamRemoteStorage()
		{
			m_fileWriteComplete = new List<CallResult<RemoteStorageFileWriteAsyncComplete_t>> { CallResult<RemoteStorageFileWriteAsyncComplete_t>.Create(OnFileWriteAsyncComplete) };
			m_fileReadComplete = new List<CallResult<RemoteStorageFileReadAsyncComplete_t>> { CallResult<RemoteStorageFileReadAsyncComplete_t>.Create(OnFileReadAsyncComplete) };
			m_filesInfo = new ConcurrentDictionary<string, MyCloudFileInfo>();
			m_readsInfo = new ConcurrentDictionary<ulong, ReadInfo>();
			m_completeReadInfo = new ConcurrentDictionary<string, ReadInfo>();
			LoadFilesInfo();
		}

		public void UnregisterCallbacks()
		{
			foreach (CallResult<RemoteStorageFileWriteAsyncComplete_t> item in m_fileWriteComplete)
			{
				item.Dispose();
			}
			foreach (CallResult<RemoteStorageFileReadAsyncComplete_t> item2 in m_fileReadComplete)
			{
				item2.Dispose();
			}
			m_filesInfo.Clear();
		}

		public void SaveBufferAsync(string fileName, byte[] buffer, Action<CloudResult> completedAction)
		{
			CloudResult cloudResult = IsCorrectSize((ulong)buffer.Length);
			if (cloudResult != 0)
			{
				completedAction?.Invoke(cloudResult);
				return;
			}
			m_filesInfo.Remove(fileName);
			SteamAPICall_t result = SteamRemoteStorage.FileWriteAsync(fileName, buffer, (uint)buffer.Length);
			SetCallResult(m_fileWriteComplete, OnFileWriteAsyncComplete, result);
			m_saveCompleteAction = completedAction;
		}

		private static ulong GetAvailableQuota()
		{
			SteamRemoteStorage.GetQuota(out var _, out var puAvailableBytes);
			return puAvailableBytes;
		}

		public static CloudResult IsCorrectSize(ulong size)
		{
			if (size > 104857600)
			{
				return CloudResult.Failed;
			}
			if (size > GetAvailableQuota())
			{
				return CloudResult.QuotaExceeded;
			}
			return CloudResult.Ok;
		}

		private void OnFileWriteAsyncComplete(RemoteStorageFileWriteAsyncComplete_t param, bool bIOFailure)
		{
			if (m_saveCompleteAction != null)
			{
				CloudResult obj;
				switch (param.m_eResult)
				{
				case EResult.k_EResultOK:
					obj = CloudResult.Ok;
					break;
				case EResult.k_EResultLimitExceeded:
				case EResult.k_EResultAccountLimitExceeded:
					obj = CloudResult.QuotaExceeded;
					break;
				default:
					obj = CloudResult.Failed;
					break;
				}
				m_saveCompleteAction(obj);
			}
		}

		public CloudResult SaveBuffer(string fileName, byte[] buffer)
		{
			CloudResult cloudResult = IsCorrectSize((ulong)buffer.Length);
			if (cloudResult != 0)
			{
				return cloudResult;
			}
			SteamRemoteStorage.GetQuota(out var _, out var puAvailableBytes);
			if ((ulong)buffer.Length > puAvailableBytes)
			{
				return CloudResult.QuotaExceeded;
			}
			bool num = SteamRemoteStorage.FileWrite(fileName, buffer, buffer.Length);
			m_filesInfo.Remove(fileName);
			if (!num)
			{
				return CloudResult.Failed;
			}
			return CloudResult.Ok;
		}

		public byte[] LoadBuffer(string fileName)
		{
			if (!m_filesInfo.TryGetValue(fileName, out var value))
			{
				int fileSize = SteamRemoteStorage.GetFileSize(fileName);
				if (fileSize == 0)
				{
					return null;
				}
				long fileTimestamp = SteamRemoteStorage.GetFileTimestamp(fileName);
				value = new MyCloudFileInfo(fileName, fileName, fileSize, fileTimestamp);
				m_filesInfo.TryAdd(fileName, value);
			}
			byte[] array = new byte[value.Size];
			if (SteamRemoteStorage.FileRead(fileName, array, value.Size) != value.Size)
			{
				return null;
			}
			return array;
		}

		public bool LoadBufferAsync(string fileName, Action<bool> completedAction)
		{
			if (!m_filesInfo.TryGetValue(fileName, out var value))
			{
				int fileSize = SteamRemoteStorage.GetFileSize(fileName);
				if (fileSize == 0)
				{
					return false;
				}
				long fileTimestamp = SteamRemoteStorage.GetFileTimestamp(fileName);
				value = new MyCloudFileInfo(fileName, fileName, fileSize, fileTimestamp);
				m_filesInfo.TryAdd(fileName, value);
			}
			SteamAPICall_t result = SteamRemoteStorage.FileReadAsync(fileName, 0u, (uint)value.Size);
			if (result.m_SteamAPICall == 0L)
			{
				return false;
			}
			SetCallResult(m_fileReadComplete, OnFileReadAsyncComplete, result);
			m_readsInfo.TryAdd(result.m_SteamAPICall, new ReadInfo
			{
				Size = value.Size,
				FileName = fileName,
				CompletedAction = completedAction
			});
			return true;
		}

		private void OnFileReadAsyncComplete(RemoteStorageFileReadAsyncComplete_t param, bool bIOFailure)
		{
			ReadInfo value = null;
			if (m_readsInfo.TryGetValue(param.m_hFileReadAsync.m_SteamAPICall, out value))
			{
				if (param.m_eResult == EResult.k_EResultOK && value.Size == param.m_cubRead)
				{
					byte[] array = new byte[param.m_cubRead];
					bool isResultOk = SteamRemoteStorage.FileReadAsyncComplete(param.m_hFileReadAsync, array, param.m_cubRead);
					value.Buffer = array;
					value.IsResultOk = isResultOk;
				}
				value.IsReadCompleted = true;
				m_completeReadInfo.TryAdd(value.FileName, value);
				m_readsInfo.TryRemove(param.m_hFileReadAsync.m_SteamAPICall, out value);
				if (value.CompletedAction != null)
				{
					value.CompletedAction(value.IsResultOk);
				}
			}
		}

		private void SetCallResult<T>(List<CallResult<T>> callList, Action<T, bool> onComplete, SteamAPICall_t result)
		{
			foreach (CallResult<T> call in callList)
			{
				if (!call.IsActive())
				{
					call.Set(result);
					return;
				}
			}
			CallResult<T> callResult = CallResult<T>.Create(delegate(T U, bool x)
			{
				onComplete(U, x);
			});
			callResult.Set(result);
			callList.Add(callResult);
		}

		public List<MyCloudFileInfo> GetCloudFiles(string directoryFilter)
		{
			LoadFilesInfo();
			List<MyCloudFileInfo> list = new List<MyCloudFileInfo>();
			foreach (MyCloudFileInfo value in m_filesInfo.Values)
			{
				if (value.Name.StartsWith(directoryFilter))
				{
					list.Add(value);
				}
			}
			return list;
		}

		private void LoadFilesInfo()
		{
			m_filesInfo.Clear();
			int fileCount = SteamRemoteStorage.GetFileCount();
			for (int i = 0; i < fileCount; i++)
			{
				int pnFileSizeInBytes = 0;
				string fileNameAndSize = SteamRemoteStorage.GetFileNameAndSize(i, out pnFileSizeInBytes);
				long fileTimestamp = SteamRemoteStorage.GetFileTimestamp(fileNameAndSize);
				m_filesInfo.TryAdd(fileNameAndSize, new MyCloudFileInfo(fileNameAndSize, fileNameAndSize, pnFileSizeInBytes, fileTimestamp));
			}
		}

		public bool IsReadCompleted(string fileName)
		{
			if (!m_completeReadInfo.TryGetValue(fileName, out var value))
			{
				return false;
			}
			return value.IsReadCompleted;
		}

		public byte[] GetFileBuffer(string fileName)
		{
			if (!m_completeReadInfo.TryGetValue(fileName, out var value))
			{
				return null;
			}
			m_completeReadInfo.TryRemove(fileName, out value);
			return value.Buffer;
		}

		public bool DeleteFile(string fileName)
		{
			bool num = SteamRemoteStorage.FileDelete(fileName);
			if (num)
			{
				m_filesInfo.TryRemove(fileName, out var _);
			}
			return num;
		}
	}
}
