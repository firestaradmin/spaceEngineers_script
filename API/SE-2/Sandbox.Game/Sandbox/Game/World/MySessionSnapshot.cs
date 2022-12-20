using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.Multiplayer;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.Voxels;
using VRage.GameServices;
using VRageMath;

namespace Sandbox.Game.World
{
	public class MySessionSnapshot
	{
		private static FastResourceLock m_savingLock = new FastResourceLock();

		public string TargetDir;

		public string SavingDir;

		public MyObjectBuilder_Checkpoint CheckpointSnapshot;

		public MyObjectBuilder_Sector SectorSnapshot;

		public Task VicinityGatherTask;

		public const int MAX_WINDOWS_PATH = 260;

		public Dictionary<string, byte[]> CompressedVoxelSnapshots { get; set; }

		public Dictionary<string, byte[]> VoxelSnapshots { get; set; }

		public Dictionary<string, IMyStorage> VoxelStorageNameCache { get; set; }

		public ulong SavedSizeInBytes { get; private set; }

		public bool SavingSuccess { get; private set; }
<<<<<<< HEAD

		public bool TooLongPath { get; private set; }

		public CloudResult CloudResult { get; private set; }

=======

		public bool TooLongPath { get; private set; }

		public CloudResult CloudResult { get; private set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool Save(Func<bool> screenshotTaken, string thumbName)
		{
			VicinityGatherTask.WaitOrExecute();
			bool gAME_SAVES_TO_CLOUD = MyPlatformGameSettings.GAME_SAVES_TO_CLOUD;
			bool flag = true;
			using (m_savingLock.AcquireExclusiveUsing())
			{
				MySandboxGame.Log.WriteLine("Session snapshot save - START");
				using (MySandboxGame.Log.IndentUsing())
				{
					Directory.CreateDirectory(TargetDir);
					MySandboxGame.Log.WriteLine("Checking file access for files in target dir.");
					if (!CheckAccessToFiles())
					{
						SavingSuccess = false;
						return false;
					}
					string savingDir = SavingDir;
					if (Directory.Exists(savingDir))
					{
						Directory.Delete(savingDir, true);
					}
					Directory.CreateDirectory(savingDir);
					List<MyCloudFile> list = new List<MyCloudFile>();
					if (thumbName != null)
					{
						list.Add(new MyCloudFile(thumbName));
					}
					try
					{
						ulong sizeInBytes = 0uL;
						ulong sizeInBytes2 = 0uL;
						ulong num = 0uL;
						TooLongPath = false;
						flag = MyLocalCache.SaveSector(SectorSnapshot, SavingDir, Vector3I.Zero, out sizeInBytes, list) && MyLocalCache.SaveCheckpoint(CheckpointSnapshot, SavingDir, out sizeInBytes2, list);
						if (flag)
						{
							foreach (KeyValuePair<string, byte[]> voxelSnapshot in VoxelSnapshots)
							{
								if (Path.Combine(SavingDir, voxelSnapshot.Key).Length > 260)
								{
									TooLongPath = true;
									flag = false;
									break;
								}
								ulong size = 0uL;
								flag = flag && SaveVoxelSnapshot(voxelSnapshot.Key, voxelSnapshot.Value, compress: true, out size, list);
								if (flag)
								{
									num += size;
								}
							}
							VoxelSnapshots.Clear();
							VoxelStorageNameCache.Clear();
							foreach (KeyValuePair<string, byte[]> compressedVoxelSnapshot in CompressedVoxelSnapshots)
							{
								if (Path.Combine(SavingDir, compressedVoxelSnapshot.Key).Length > 260)
								{
									TooLongPath = true;
									flag = false;
									break;
								}
								ulong size2 = 0uL;
								flag = flag && SaveVoxelSnapshot(compressedVoxelSnapshot.Key, compressedVoxelSnapshot.Value, compress: false, out size2, list);
								if (flag)
								{
									num += size2;
								}
							}
							CompressedVoxelSnapshots.Clear();
						}
						if (flag && Sync.IsServer)
						{
							flag = MyLocalCache.SaveLastSessionInfo(TargetDir, isOnline: false, isLobby: false, MySession.Static.Name, null, 0);
						}
						if (flag)
						{
							SavedSizeInBytes = sizeInBytes + sizeInBytes2 + num;
							if (screenshotTaken != null)
							{
								while (!screenshotTaken())
								{
									Thread.Sleep(10);
								}
							}
							if (gAME_SAVES_TO_CLOUD)
							{
								string containerName = MyCloudHelper.LocalToCloudWorldPath(TargetDir);
								CloudResult = MyGameService.SaveToCloud(containerName, list);
								flag = CloudResult == CloudResult.Ok;
							}
						}
						if (flag)
						{
<<<<<<< HEAD
							HashSet<string> hashSet = new HashSet<string>();
=======
							HashSet<string> val = new HashSet<string>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							string[] files = Directory.GetFiles(savingDir);
							foreach (string obj in files)
							{
								string fileName = Path.GetFileName(obj);
<<<<<<< HEAD
								string destFileName = Path.Combine(TargetDir, fileName);
								File.Copy(obj, destFileName, overwrite: true);
								hashSet.Add(fileName);
=======
								string text = Path.Combine(TargetDir, fileName);
								File.Copy(obj, text, true);
								val.Add(fileName);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							}
							try
							{
								files = Directory.GetFiles(TargetDir);
<<<<<<< HEAD
								foreach (string path in files)
								{
									string fileName2 = Path.GetFileName(path);
									if (!hashSet.Contains(fileName2) && !(fileName2 == MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION))
									{
										File.Delete(path);
									}
								}
								Directory.Delete(savingDir, recursive: true);
=======
								foreach (string text2 in files)
								{
									string fileName2 = Path.GetFileName(text2);
									if (!val.Contains(fileName2) && !(fileName2 == MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION))
									{
										File.Delete(text2);
									}
								}
								Directory.Delete(savingDir, true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							}
							catch (Exception ex)
							{
								MySandboxGame.Log.WriteLine("There was an error while cleaning the snapshot.");
								MySandboxGame.Log.WriteLine(ex);
							}
							Backup(TargetDir, TargetDir);
						}
					}
					catch (Exception ex2)
					{
						MySandboxGame.Log.WriteLine("There was an error while saving snapshot.");
						MySandboxGame.Log.WriteLine(ex2);
						flag = false;
					}
					if (!flag)
					{
						try
<<<<<<< HEAD
						{
							if (Directory.Exists(savingDir))
							{
								Directory.Delete(savingDir, recursive: true);
							}
						}
						catch (Exception ex3)
						{
							MySandboxGame.Log.WriteLine("There was an error while cleaning snapshot.");
							MySandboxGame.Log.WriteLine(ex3);
						}
=======
						{
							if (Directory.Exists(savingDir))
							{
								Directory.Delete(savingDir, true);
							}
						}
						catch (Exception ex3)
						{
							MySandboxGame.Log.WriteLine("There was an error while cleaning snapshot.");
							MySandboxGame.Log.WriteLine(ex3);
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				MySandboxGame.Log.WriteLine("Session snapshot save - END");
			}
			SavingSuccess = flag;
			return flag;
		}

<<<<<<< HEAD
		/// <summary>
		/// Backup Saves to Backup folder of save game according to MySession.Static.MaxBackupSaves on advanced settings.
		/// If this is zero the backup functionality is disabled.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private void Backup(string targetDir, string backupDir)
		{
			if (MySession.Static.MaxBackupSaves > 0)
			{
				string path = DateTime.Now.ToString("yyyy-MM-dd HHmmss");
				string text = Path.Combine(backupDir, MyTextConstants.SESSION_SAVE_BACKUP_FOLDER, path);
				Directory.CreateDirectory(text);
				string[] files = Directory.GetFiles(targetDir);
				foreach (string text2 in files)
				{
					string text3 = Path.Combine(text, Path.GetFileName(text2));
					if (text3.Length < 260 && text2.Length < 260)
					{
						File.Copy(text2, text3, true);
					}
				}
				string[] directories = Directory.GetDirectories(Path.Combine(backupDir, MyTextConstants.SESSION_SAVE_BACKUP_FOLDER));
				if (!IsSorted(directories))
				{
					Array.Sort(directories);
				}
				if (directories.Length > MySession.Static.MaxBackupSaves)
				{
					int num = directories.Length - MySession.Static.MaxBackupSaves;
					for (int j = 0; j < num; j++)
					{
						Directory.Delete(directories[j], true);
					}
				}
			}
			else if (MySession.Static.MaxBackupSaves == 0 && Directory.Exists(Path.Combine(backupDir, MyTextConstants.SESSION_SAVE_BACKUP_FOLDER)))
			{
<<<<<<< HEAD
				Directory.Delete(Path.Combine(backupDir, MyTextConstants.SESSION_SAVE_BACKUP_FOLDER), recursive: true);
=======
				Directory.Delete(Path.Combine(backupDir, MyTextConstants.SESSION_SAVE_BACKUP_FOLDER), true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		/// <summary>
		/// Determines if string array is sorted from A -&gt; Z
		/// </summary>
		public static bool IsSorted(string[] arr)
		{
			for (int i = 1; i < arr.Length; i++)
			{
				if (arr[i - 1].CompareTo(arr[i]) > 0)
				{
					return false;
				}
			}
			return true;
		}

		private bool SaveVoxelSnapshot(string storageName, byte[] snapshotData, bool compress, out ulong size, List<MyCloudFile> fileList)
		{
<<<<<<< HEAD
=======
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			string path = storageName + ".vx2";
			string text = Path.Combine(SavingDir, path);
			fileList.Add(new MyCloudFile(text));
			try
			{
				if (compress)
				{
					using MemoryStream memoryStream = new MemoryStream(16384);
					GZipStream val = new GZipStream((Stream)memoryStream, (CompressionMode)1);
					try
					{
						((Stream)(object)val).Write(snapshotData, 0, snapshotData.Length);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					byte[] array = memoryStream.ToArray();
					File.WriteAllBytes(text, array);
					size = (ulong)array.Length;
					if (VoxelStorageNameCache != null)
					{
						IMyStorage value = null;
						if (VoxelStorageNameCache.TryGetValue(storageName, out value) && !value.Closed)
						{
<<<<<<< HEAD
							IMyStorage value = null;
							if (VoxelStorageNameCache.TryGetValue(storageName, out value) && !value.Closed)
							{
								value.SetDataCache(array, compressed: true);
							}
=======
							value.SetDataCache(array, compressed: true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
				}
				else
				{
					File.WriteAllBytes(text, snapshotData);
					size = (ulong)snapshotData.Length;
				}
			}
			catch (Exception ex)
			{
				MySandboxGame.Log.WriteLine($"Failed to write voxel file '{text}'");
				MySandboxGame.Log.WriteLine(ex);
				size = 0uL;
				return false;
			}
			return true;
		}

		private bool CheckAccessToFiles()
		{
			string[] files = Directory.GetFiles(TargetDir, "*", (SearchOption)0);
			foreach (string text in files)
			{
				if (!(text == MySession.Static.ThumbPath) && !MyFileSystem.CheckFileWriteAccess(text))
				{
					MySandboxGame.Log.WriteLine($"Couldn't access file '{Path.GetFileName(text)}'.");
					return false;
				}
			}
			return true;
		}

		public void SaveParallel(Func<bool> screenshotTaken, string screenshotPath, Action completionCallback = null)
		{
			Action action = delegate
			{
				Save(screenshotTaken, screenshotPath);
			};
			if (completionCallback != null)
			{
				Parallel.Start(action, completionCallback, WorkPriority.Low);
			}
			else
			{
				Parallel.Start(action, WorkPriority.Low);
			}
		}

		public static void WaitForSaving()
		{
			int num = 0;
			do
			{
				using (m_savingLock.AcquireExclusiveUsing())
				{
					num = m_savingLock.ExclusiveWaiters;
				}
			}
			while (num > 0);
		}
	}
}
