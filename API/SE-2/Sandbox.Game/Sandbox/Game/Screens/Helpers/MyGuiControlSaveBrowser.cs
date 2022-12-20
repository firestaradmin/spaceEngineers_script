using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sandbox.Engine.Networking;
using Sandbox.Game.GUI;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.Gui.DirectoryBrowser;
using VRage;
using VRage.FileSystem;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGuiControlSaveBrowser : MyGuiControlDirectoryBrowser
	{
		private readonly List<FileInfo> m_saveEntriesToCreate = new List<FileInfo>();

		private readonly Dictionary<string, MyWorldInfo> m_loadedWorldsByFilePaths = new Dictionary<string, MyWorldInfo>();

		public string SearchTextFilter;

		public bool InBackupsFolder { get; private set; }

		private bool IsCurrentDirectoryCloud
		{
			get
			{
				if (MyPlatformGameSettings.GAME_SAVES_TO_CLOUD)
				{
					return !InBackupsFolder;
				}
				return false;
			}
		}

		public MyGuiControlSaveBrowser()
			: base(MyFileSystem.SavesPath, MyFileSystem.SavesPath)
		{
			SetColumnName(1, MyTexts.Get(MyCommonTexts.Date));
			SetColumnComparison(1, delegate(Cell cellA, Cell cellB)
			{
				if (cellA == null)
				{
					return -1;
				}
				if (cellB == null)
				{
					return -1;
				}
				object userData = cellA.UserData;
				FileInfo val = (FileInfo)((userData is FileInfo) ? userData : null);
				object userData2 = cellB.UserData;
				FileInfo val2 = (FileInfo)((userData2 is FileInfo) ? userData2 : null);
				if (val == val2)
				{
					if (val == null)
					{
						return 0;
					}
				}
				else
				{
					if (val == null)
					{
						return -1;
					}
					if (val2 == null)
					{
						return 1;
					}
				}
				return m_loadedWorldsByFilePaths[val.get_DirectoryName()].LastSaveTime.CompareTo(m_loadedWorldsByFilePaths[val2.get_DirectoryName()].LastSaveTime);
			});
		}

		public DirectoryInfo GetDirectory(Row row)
		{
			if (row == null)
			{
				return null;
			}
			object userData = row.UserData;
			return (DirectoryInfo)((userData is DirectoryInfo) ? userData : null);
		}

		public void GetSave(Row row, out MySaveInfo info)
		{
			info = default(MySaveInfo);
			if (row == null)
			{
				return;
			}
			MyCloudFile myCloudFile;
			MySaveInfo mySaveInfo;
<<<<<<< HEAD
			FileInfo fileInfo;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if ((myCloudFile = row.UserData as MyCloudFile) != null)
			{
				if (m_loadedWorldsByFilePaths.TryGetValue(myCloudFile.CloudName, out var value))
				{
					mySaveInfo = new MySaveInfo
					{
						Valid = true,
						Name = myCloudFile.CloudName,
						WorldInfo = value,
						IsCloud = true
					};
					info = mySaveInfo;
				}
<<<<<<< HEAD
			}
			else if ((fileInfo = row.UserData as FileInfo) != null)
			{
				string directoryName = Path.GetDirectoryName(fileInfo.FullName);
=======
				return;
			}
			object userData = row.UserData;
			FileInfo val;
			if ((val = (FileInfo)((userData is FileInfo) ? userData : null)) != null)
			{
				string directoryName = Path.GetDirectoryName(((FileSystemInfo)val).get_FullName());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (m_loadedWorldsByFilePaths.TryGetValue(directoryName, out var value2))
				{
					mySaveInfo = new MySaveInfo
					{
						Valid = true,
						Name = directoryName,
						WorldInfo = value2
					};
					info = mySaveInfo;
				}
			}
		}

		public void AccessBackups()
		{
<<<<<<< HEAD
=======
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			GetSave(base.SelectedRow, out var info);
			if (!info.Valid)
			{
				return;
			}
<<<<<<< HEAD
			DirectoryInfo directoryInfo = ((!info.IsCloud) ? new DirectoryInfo(info.Name) : new DirectoryInfo(MyCloudHelper.CloudToLocalWorldPath(info.Name)));
			DirectoryInfo directoryInfo2 = null;
			if (directoryInfo.Exists)
			{
				directoryInfo2 = directoryInfo.GetDirectories().FirstOrDefault((DirectoryInfo dir) => dir.Name.StartsWith("Backup"));
			}
			if (directoryInfo2 == null)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.SaveBrowserMissingBackup), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
				return;
			}
			InBackupsFolder = true;
			base.CurrentDirectory = directoryInfo2.FullName;
=======
			DirectoryInfo val = ((!info.IsCloud) ? new DirectoryInfo(info.Name) : new DirectoryInfo(MyCloudHelper.CloudToLocalWorldPath(info.Name)));
			DirectoryInfo val2 = null;
			if (((FileSystemInfo)val).get_Exists())
			{
				val2 = Enumerable.FirstOrDefault<DirectoryInfo>((IEnumerable<DirectoryInfo>)val.GetDirectories(), (Func<DirectoryInfo, bool>)((DirectoryInfo dir) => ((FileSystemInfo)dir).get_Name().StartsWith("Backup")));
			}
			if (val2 == null)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.SaveBrowserMissingBackup), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
				return;
			}
			InBackupsFolder = true;
			base.CurrentDirectory = ((FileSystemInfo)val2).get_FullName();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		protected override void AddFolderRow(DirectoryInfo dir)
		{
			if (!SearchFilterTest(((FileSystemInfo)dir).get_Name()))
			{
				return;
			}
			FileInfo[] files = dir.GetFiles();
			bool flag = false;
			FileInfo[] array = files;
			foreach (FileInfo val in array)
			{
				if (((FileSystemInfo)val).get_Name() == "Sandbox.sbc")
				{
					if (m_loadedWorldsByFilePaths.ContainsKey(val.get_DirectoryName()))
					{
						m_saveEntriesToCreate.Add(val);
					}
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				base.AddFolderRow(dir);
			}
		}

		public override void Refresh()
		{
			RefreshTheWorldInfos();
		}

		public void ForceRefresh()
		{
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, StartLoadingWorldInfos, OnLoadingFinished));
		}

		public void RefreshAfterLoaded()
		{
			if (IsCurrentDirectoryCloud)
			{
				Clear();
<<<<<<< HEAD
				List<string> list = m_loadedWorldsByFilePaths.Keys.ToList();
=======
				List<string> list = Enumerable.ToList<string>((IEnumerable<string>)m_loadedWorldsByFilePaths.Keys);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				list.Sort((string fileA, string fileB) => m_loadedWorldsByFilePaths[fileB].LastSaveTime.CompareTo(m_loadedWorldsByFilePaths[fileA].LastSaveTime));
				foreach (string item in list)
				{
					AddSavedGame(item, new MyCloudFile(item));
				}
				ScrollToSelection();
				return;
			}
			base.Refresh();
<<<<<<< HEAD
			m_saveEntriesToCreate.Sort((FileInfo fileA, FileInfo fileB) => m_loadedWorldsByFilePaths[fileB.DirectoryName].LastSaveTime.CompareTo(m_loadedWorldsByFilePaths[fileA.DirectoryName].LastSaveTime));
			foreach (FileInfo item2 in m_saveEntriesToCreate)
			{
				AddSavedGame(item2.DirectoryName, item2);
=======
			m_saveEntriesToCreate.Sort((FileInfo fileA, FileInfo fileB) => m_loadedWorldsByFilePaths[fileB.get_DirectoryName()].LastSaveTime.CompareTo(m_loadedWorldsByFilePaths[fileA.get_DirectoryName()].LastSaveTime));
			foreach (FileInfo item2 in m_saveEntriesToCreate)
			{
				AddSavedGame(item2.get_DirectoryName(), item2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_saveEntriesToCreate.Clear();
		}

		private void AddSavedGame(string saveName, object userData)
		{
			MyWorldInfo myWorldInfo = m_loadedWorldsByFilePaths[saveName];
			if (SearchFilterTest(myWorldInfo.SessionName))
			{
				Row row = new Row(userData);
				Cell cell = new Cell(myWorldInfo.SessionName, userData, null, null, base.FileCellIconTexture, base.FileCellIconAlign);
				if (myWorldInfo.IsCorrupted)
				{
					cell.TextColor = Color.Red;
				}
				row.AddCell(cell);
				row.AddCell(new Cell(myWorldInfo.LastSaveTime.ToString("g"), userData));
				row.AddCell(new Cell(MyValueFormatter.GetFormattedFileSizeInMB(myWorldInfo.StorageSize), userData));
				Add(row);
			}
		}

		private void RefreshTheWorldInfos()
		{
			m_loadedWorldsByFilePaths.Clear();
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.LoadingPleaseWait, null, StartLoadingWorldInfos, OnLoadingFinished));
		}

		private bool SearchFilterTest(string testString)
		{
			if (SearchTextFilter != null && SearchTextFilter.Length != 0)
			{
				string[] array = SearchTextFilter.Split(new char[1] { ' ' });
				string text = testString.ToLower();
				string[] array2 = array;
				foreach (string text2 in array2)
				{
					if (!text.Contains(text2.ToLower()))
					{
						return false;
					}
				}
			}
			return true;
		}

		private IMyAsyncResult StartLoadingWorldInfos()
		{
			if (IsCurrentDirectoryCloud)
			{
				return new MyLoadWorldInfoListFromCloudResult(new List<string> { GetLocalPath() });
			}
			return new MyLoadWorldInfoListResult(new List<string> { base.CurrentDirectory });
		}

		private void OnLoadingFinished(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
		{
			MyLoadListResult myLoadListResult = (MyLoadListResult)result;
			m_loadedWorldsByFilePaths.Clear();
			foreach (Tuple<string, MyWorldInfo> availableSafe in myLoadListResult.AvailableSaves)
			{
				m_loadedWorldsByFilePaths[availableSafe.Item1] = availableSafe.Item2;
			}
			if (myLoadListResult.ContainsCorruptedWorlds)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.SomeWorldFilesCouldNotBeLoaded), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
			}
			RefreshAfterLoaded();
			screen.CloseScreen();
		}

		protected override void OnBackDoubleclicked()
		{
			if (((FileSystemInfo)m_currentDir).get_Name().StartsWith("Backup"))
			{
				base.CurrentDirectory = ((FileSystemInfo)m_currentDir.get_Parent().get_Parent()).get_FullName();
				InBackupsFolder = false;
				base.IgnoreFirstRowForSort = false;
			}
			else
			{
				base.OnBackDoubleclicked();
			}
		}
	}
}
