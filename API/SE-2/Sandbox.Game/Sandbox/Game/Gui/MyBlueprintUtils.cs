using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EmptyKeys.UserInterface.Mvvm;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.ViewModels;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.World;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.GUI
{
	public class MyBlueprintUtils
	{
		public static readonly string THUMB_IMAGE_NAME = "thumb.png";

		public static readonly string DEFAULT_SCRIPT_NAME = "Script";

		public static readonly string SCRIPT_EXTENSION = ".cs";

		public static readonly string BLUEPRINT_WORKSHOP_EXTENSION = ".sbb";

		public static readonly string BLUEPRINT_LOCAL_NAME = "bp.sbc";

		public static readonly string STEAM_THUMBNAIL_NAME = "Textures\\GUI\\Icons\\IngameProgrammingIcon.png";

		public static readonly string SCRIPTS_DIRECTORY = "IngameScripts";

		public static readonly string BLUEPRINT_DIRECTORY = "Blueprints";

		public static readonly string BLUEPRINT_DEFAULT_DIRECTORY = Path.Combine(MyFileSystem.ContentPath, "Data", "Blueprints");

		public static readonly string SCRIPT_FOLDER_LOCAL = Path.Combine(MyFileSystem.UserDataPath, SCRIPTS_DIRECTORY, "local");

		public static readonly string SCRIPT_FOLDER_TEMP = Path.Combine(MyFileSystem.UserDataPath, SCRIPTS_DIRECTORY, "temp");

		public static readonly string SCRIPT_FOLDER_WORKSHOP = Path.Combine(MyFileSystem.UserDataPath, SCRIPTS_DIRECTORY, "workshop");

		public static readonly string SCRIPT_IMAGE_FOLDER_WORKSHOP = Path.Combine(MyFileSystem.UserDataPath, "WorkshopBrowser");

		public static readonly string BLUEPRINT_FOLDER_LOCAL = Path.Combine(MyFileSystem.UserDataPath, BLUEPRINT_DIRECTORY, "local");

		public static readonly string BLUEPRINT_FOLDER_TEMP = Path.Combine(MyFileSystem.UserDataPath, BLUEPRINT_DIRECTORY, "temp");

		public static readonly string BLUEPRINT_FOLDER_WORKSHOP = Path.Combine(MyFileSystem.UserDataPath, BLUEPRINT_DIRECTORY, "workshop");

		public static readonly string BLUEPRINT_WORKSHOP_TEMP = Path.Combine(BLUEPRINT_FOLDER_WORKSHOP, "temp");

		private const int FILE_PATH_MAX_LENGTH = 260;

<<<<<<< HEAD
		private const int MAX_THUMBNAIL_SIZE = 1000000;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static MyObjectBuilder_Definitions LoadPrefab(string filePath)
		{
			MyObjectBuilder_Definitions objectBuilder = null;
			bool flag = false;
			string path = filePath + "B5";
			if (MyFileSystem.FileExists(path))
			{
				flag = MyObjectBuilderSerializer.DeserializePB<MyObjectBuilder_Definitions>(path, out objectBuilder);
				if (objectBuilder == null || objectBuilder.ShipBlueprints == null)
				{
					flag = MyObjectBuilderSerializer.DeserializeXML(filePath, out objectBuilder);
					if (objectBuilder != null)
					{
						MyObjectBuilderSerializer.SerializePB(path, compress: false, objectBuilder);
					}
				}
			}
			else if (MyFileSystem.FileExists(filePath))
			{
				flag = MyObjectBuilderSerializer.DeserializeXML(filePath, out objectBuilder);
				if (flag)
				{
					MyObjectBuilderSerializer.SerializePB(path, compress: false, objectBuilder);
				}
			}
			if (!flag)
			{
				return null;
			}
			return objectBuilder;
		}

		public static MyObjectBuilder_Definitions LoadPrefabFromCloud(MyBlueprintItemInfo info)
		{
			MyObjectBuilder_Definitions objectBuilder = null;
			if (!string.IsNullOrEmpty(info.CloudPathPB))
			{
				byte[] array = MyGameService.LoadFromCloud(info.CloudPathPB);
				if (array != null)
				{
					using (MemoryStream reader = new MemoryStream(array))
					{
						MyObjectBuilderSerializer.DeserializePB<MyObjectBuilder_Definitions>(reader, out objectBuilder);
						return objectBuilder;
					}
				}
			}
			else if (!string.IsNullOrEmpty(info.CloudPathXML))
			{
				byte[] array2 = MyGameService.LoadFromCloud(info.CloudPathXML);
				if (array2 != null)
				{
					using (MemoryStream stream = new MemoryStream(array2))
					{
						using Stream reader2 = stream.UnwrapGZip();
						MyObjectBuilderSerializer.DeserializeXML(reader2, out objectBuilder);
						return objectBuilder;
					}
				}
			}
			return objectBuilder;
		}

		public static bool CopyFileFromCloud(string pathFull, string pathRel)
		{
			byte[] array = MyGameService.LoadFromCloud(pathRel);
			if (array == null)
			{
				return false;
			}
			using (MemoryStream memoryStream = new MemoryStream(array))
			{
				memoryStream.Seek(0L, SeekOrigin.Begin);
				MyFileSystem.CreateDirectoryRecursive(Path.GetDirectoryName(pathFull));
				using FileStream fileStream = new FileStream(pathFull, FileMode.OpenOrCreate);
				memoryStream.CopyTo(fileStream);
				fileStream.Flush();
			}
			return true;
		}

		public static MyObjectBuilder_Definitions LoadWorkshopPrefab(string archive, ulong? publishedItemId, string publishedServiceName, bool isOldBlueprintScreen)
		{
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Expected O, but got Unknown
			if ((!File.Exists(archive) && !MyFileSystem.DirectoryExists(archive)) || !publishedItemId.HasValue)
			{
				return null;
			}
			MyWorkshopItem myWorkshopItem;
			if (isOldBlueprintScreen)
			{
				myWorkshopItem = MyGuiBlueprintScreen.m_subscribedItemsList.Find((MyWorkshopItem item) => item.Id == publishedItemId && item.ServiceName == publishedServiceName);
			}
			else
			{
				using (MyGuiBlueprintScreen_Reworked.SubscribedItemsLock.AcquireSharedUsing())
				{
					myWorkshopItem = MyGuiBlueprintScreen_Reworked.GetSubscribedItemsList(Content.Blueprint).Find((MyWorkshopItem item) => item.Id == publishedItemId && item.ServiceName == publishedServiceName);
				}
			}
			if (myWorkshopItem == null)
			{
				return null;
			}
			string text = Path.Combine(archive, BLUEPRINT_LOCAL_NAME);
			string text2 = text + "B5";
			if (!MyFileSystem.FileExists(text2) && publishedItemId.HasValue)
			{
				string text3 = Path.Combine(BLUEPRINT_WORKSHOP_TEMP, myWorkshopItem.ServiceName, publishedItemId.Value.ToString());
				MyFileSystem.EnsureDirectoryExists(text3);
				text2 = Path.Combine(text3, BLUEPRINT_LOCAL_NAME);
				text2 += "B5";
			}
			bool flag = false;
			MyObjectBuilder_Definitions objectBuilder = null;
			bool num = MyFileSystem.FileExists(text2);
			bool flag2 = MyFileSystem.FileExists(text);
			bool flag3 = false;
			if (num && flag2)
			{
				FileInfo val = new FileInfo(text2);
				FileInfo val2 = new FileInfo(text);
				if (((FileSystemInfo)val).get_LastWriteTimeUtc() >= ((FileSystemInfo)val2).get_LastWriteTimeUtc())
				{
					flag3 = true;
				}
			}
			if (flag3)
			{
				flag = MyObjectBuilderSerializer.DeserializePB<MyObjectBuilder_Definitions>(text2, out objectBuilder);
				if (objectBuilder == null || objectBuilder.ShipBlueprints == null)
				{
					flag = MyObjectBuilderSerializer.DeserializeXML(text, out objectBuilder);
				}
			}
			else if (flag2)
			{
				flag = MyObjectBuilderSerializer.DeserializeXML(text, out objectBuilder);
				if (flag && publishedItemId.HasValue)
				{
					MyObjectBuilderSerializer.SerializePB(text2, compress: false, objectBuilder);
				}
			}
			if (flag)
			{
				objectBuilder.ShipBlueprints[0].Description = myWorkshopItem.Description;
				objectBuilder.ShipBlueprints[0].CubeGrids[0].DisplayName = myWorkshopItem.Title;
				objectBuilder.ShipBlueprints[0].DLCs = new string[myWorkshopItem.DLCs.Count];
				for (int i = 0; i < myWorkshopItem.DLCs.Count; i++)
				{
					if (MyDLCs.TryGetDLC(myWorkshopItem.DLCs[i], out var dlc))
					{
						objectBuilder.ShipBlueprints[0].DLCs[i] = dlc.Name;
					}
				}
				return objectBuilder;
			}
			return null;
		}

		public static void PublishBlueprint(MyObjectBuilder_Definitions prefab, string blueprintName, string currentLocalDirectory, string sourceFile, MyBlueprintTypeEnum type)
		{
			string file = sourceFile ?? Path.Combine(BLUEPRINT_FOLDER_LOCAL, currentLocalDirectory, blueprintName);
			string title = prefab.ShipBlueprints[0].CubeGrids[0].DisplayName;
			string description = prefab.ShipBlueprints[0].Description;
			WorkshopId[] publishIds = prefab.ShipBlueprints[0].WorkshopIds;
<<<<<<< HEAD
			if (File.Exists(Path.Combine(file, "thumb.png")) && new FileInfo(Path.Combine(file, "thumb.png")).Length >= 1000000)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MySpaceTexts.MessageBoxTextThumbnailTooBig)));
				return;
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (publishIds == null)
			{
				publishIds = new WorkshopId[1]
				{
					new WorkshopId(prefab.ShipBlueprints[0].WorkshopId, MyGameService.GetDefaultUGC().ServiceName)
				};
			}
			MyCubeSize gridSize = prefab.ShipBlueprints[0].CubeGrids[0].GridSizeEnum;
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MySpaceTexts.PublishBlueprint_Caption), messageText: MyTexts.Get(MySpaceTexts.PublishBlueprint_Question), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum val)
			{
				if (val == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					Action<MyGuiScreenMessageBox.ResultEnum, string[], string[]> action = delegate(MyGuiScreenMessageBox.ResultEnum tagsResult, string[] outTags, string[] serviceNames)
					{
						if (tagsResult == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							publishIds = MyWorkshop.FilterWorkshopIds(publishIds, serviceNames);
<<<<<<< HEAD
							HashSet<uint> hashSet = new HashSet<uint>();
=======
							HashSet<uint> val2 = new HashSet<uint>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							MyObjectBuilder_ShipBlueprintDefinition[] shipBlueprints = prefab.ShipBlueprints;
							foreach (MyObjectBuilder_ShipBlueprintDefinition myObjectBuilder_ShipBlueprintDefinition in shipBlueprints)
							{
								if (myObjectBuilder_ShipBlueprintDefinition.DLCs != null)
								{
									string[] dLCs = myObjectBuilder_ShipBlueprintDefinition.DLCs;
									foreach (string text in dLCs)
									{
										uint result2;
										if (MyDLCs.TryGetDLC(text, out var dlc))
										{
											val2.Add(dlc.AppId);
										}
										else if (uint.TryParse(text, out result2))
										{
											val2.Add(result2);
										}
									}
								}
							}
							Array.Resize(ref outTags, outTags.Length + 1 + 1);
							outTags[outTags.Length - 1] = ((gridSize == MyCubeSize.Large) ? "large_grid" : "small_grid");
							if (false)
							{
								outTags[outTags.Length - 2] = MySteamConstants.TAG_EXPERIMENTAL;
							}
							else
							{
								outTags[outTags.Length - 2] = MySteamConstants.TAG_SAFE;
							}
<<<<<<< HEAD
							MyWorkshop.PublishBlueprintAsync(file, title, description, publishIds, outTags, hashSet.ToArray(), MyPublishedFileVisibility.Public, delegate(bool success, MyGameServiceCallResult result, string resultServiceName, MyWorkshopItem[] publishedItems)
=======
							MyWorkshop.PublishBlueprintAsync(file, title, description, publishIds, outTags, Enumerable.ToArray<uint>((IEnumerable<uint>)val2), MyPublishedFileVisibility.Public, delegate(bool success, MyGameServiceCallResult result, string resultServiceName, MyWorkshopItem[] publishedItems)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							{
								if (publishedItems.Length != 0)
								{
									WorkshopId[] workshopIds = publishedItems.ToWorkshopIds();
									prefab.ShipBlueprints[0].WorkshopId = 0uL;
									prefab.ShipBlueprints[0].WorkshopIds = workshopIds;
									SavePrefabToFile(prefab, blueprintName, currentLocalDirectory, replace: true, type, forceLocal: true);
								}
								MyWorkshop.ReportPublish(publishedItems, result, resultServiceName);
							});
						}
					};
					if (MyWorkshop.BlueprintCategories.Length != 0)
					{
						MyGuiSandbox.AddScreen(new MyGuiScreenWorkshopTags("blueprint", MyWorkshop.BlueprintCategories, null, action));
					}
					else
					{
						action(MyGuiScreenMessageBox.ResultEnum.YES, new string[1] { "blueprint" }, MyGameService.GetUGCNamesList());
					}
				}
			}));
		}

		public static void SavePrefabToFile(MyObjectBuilder_Definitions prefab, string name, string currentDirectory, bool replace = false, MyBlueprintTypeEnum type = MyBlueprintTypeEnum.LOCAL, bool forceLocal = false)
		{
			if (type == MyBlueprintTypeEnum.LOCAL && MySandboxGame.Config.EnableSteamCloud && MyGameService.IsActive && !forceLocal)
			{
				type = MyBlueprintTypeEnum.CLOUD;
			}
			string text = string.Empty;
			switch (type)
			{
			case MyBlueprintTypeEnum.CLOUD:
				text = Path.Combine("Blueprints/cloud", name);
				break;
			case MyBlueprintTypeEnum.LOCAL:
				text = Path.Combine(BLUEPRINT_FOLDER_LOCAL, currentDirectory, name);
				break;
			case MyBlueprintTypeEnum.WORKSHOP:
			case MyBlueprintTypeEnum.SHARED:
			case MyBlueprintTypeEnum.DEFAULT:
				text = Path.Combine(BLUEPRINT_WORKSHOP_TEMP, name);
				break;
			}
			string filePath = string.Empty;
			try
			{
				if (type == MyBlueprintTypeEnum.CLOUD)
				{
					filePath = Path.Combine(text, BLUEPRINT_LOCAL_NAME);
					SaveToCloud(prefab, filePath, replace);
				}
				else
				{
					SaveToDisk(prefab, name, replace, type, text, currentDirectory, ref filePath);
				}
			}
			catch (Exception ex)
			{
				MySandboxGame.Log.WriteLine($"Failed to write prefab at file {filePath}, message: {ex.Message}, stack:{ex.StackTrace}");
			}
		}

		public static void SaveToCloud(MyObjectBuilder_Definitions prefab, string filePath, bool replace, Action<string, CloudResult> onCompleted = null)
		{
			using MemoryStream memoryStream = new MemoryStream();
			bool num = MyObjectBuilderSerializer.SerializeXML(memoryStream, prefab, MyObjectBuilderSerializer.XmlCompression.Gzip);
			if (num)
			{
				byte[] buffer = memoryStream.ToArray();
				string filePathCorrect = filePath.Replace('\\', '/');
				MyGameService.SaveToCloudAsync(filePathCorrect, buffer, delegate(CloudResult result)
				{
<<<<<<< HEAD
					byte[] buffer = memoryStream.ToArray();
					string filePathCorrect = filePath.Replace('\\', '/');
					MyGameService.SaveToCloudAsync(filePathCorrect, buffer, delegate(CloudResult result)
					{
						if (result != 0)
						{
							onCompleted?.Invoke(filePath, result);
						}
						else
=======
					if (result != 0)
					{
						onCompleted?.Invoke(filePath, result);
					}
					else
					{
						using (MemoryStream memoryStream2 = new MemoryStream())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							if (MyObjectBuilderSerializer.SerializePB(memoryStream2, prefab))
							{
								byte[] buffer2 = memoryStream2.ToArray();
								filePathCorrect += "B5";
								CloudResult cloudResult = MyGameService.SaveToCloud(filePathCorrect, buffer2);
								if (cloudResult != 0)
								{
<<<<<<< HEAD
									byte[] buffer2 = memoryStream2.ToArray();
									filePathCorrect += "B5";
									CloudResult cloudResult = MyGameService.SaveToCloud(filePathCorrect, buffer2);
									if (cloudResult != 0)
									{
										onCompleted?.Invoke(filePath, cloudResult);
										return;
									}
=======
									onCompleted?.Invoke(filePath, cloudResult);
									return;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
								}
							}
							onCompleted?.Invoke(filePath, CloudResult.Ok);
						}
						onCompleted?.Invoke(filePath, CloudResult.Ok);
					}
				});
			}
			if (!num)
			{
				ShowBlueprintSaveError();
			}
		}

		public static CloudResult SaveToCloudFile(string pathFull, string pathRel)
		{
			try
			{
<<<<<<< HEAD
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (FileStream fileStream = new FileStream(pathFull, FileMode.Open, FileAccess.Read))
					{
						fileStream.CopyTo(memoryStream);
						byte[] buffer = memoryStream.ToArray();
						return MyGameService.SaveToCloud(pathRel.Replace('\\', '/'), buffer);
					}
				}
=======
				using MemoryStream memoryStream = new MemoryStream();
				using FileStream fileStream = new FileStream(pathFull, FileMode.Open, FileAccess.Read);
				fileStream.CopyTo(memoryStream);
				byte[] buffer = memoryStream.ToArray();
				return MyGameService.SaveToCloud(pathRel.Replace('\\', '/'), buffer);
			}
			catch (IOException)
			{
				return CloudResult.Failed;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch (IOException)
			{
				return CloudResult.Failed;
			}
		}

		private static void ShowBlueprintSaveError()
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: new StringBuilder("Error"), messageText: new StringBuilder("There was a problem with saving blueprint/script")));
		}

		public static void SaveToDisk(MyObjectBuilder_Definitions prefab, string name, bool replace, MyBlueprintTypeEnum type, string file, string currentDirectory, ref string filePath)
		{
			if (!replace)
			{
				int num = 1;
				while (MyFileSystem.DirectoryExists(file))
				{
					file = Path.Combine(BLUEPRINT_FOLDER_LOCAL, currentDirectory, name + "_" + num);
					num++;
				}
				if (num > 1)
				{
					name += new StringBuilder("_" + (num - 1));
				}
			}
			filePath = Path.Combine(file, BLUEPRINT_LOCAL_NAME);
			if (filePath.Length > 260)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat(MyTexts.GetString(MySpaceTexts.BlueprintScreen_FilePathTooLong_Description), filePath.Length, 260, filePath);
				StringBuilder messageCaption = MyTexts.Get(MySpaceTexts.BlueprintScreen_FilePathTooLong_Caption);
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, stringBuilder, messageCaption));
				return;
			}
			bool num2 = MyObjectBuilderSerializer.SerializeXML(filePath, compress: false, prefab);
			if (num2 && type == MyBlueprintTypeEnum.LOCAL)
			{
				MyObjectBuilderSerializer.SerializePB(filePath + "B5", compress: false, prefab);
			}
			if (!num2)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: new StringBuilder("Error"), messageText: new StringBuilder("There was a problem with saving blueprint")));
				if (Directory.Exists(file))
				{
					Directory.Delete(file, true);
				}
			}
		}

		public static int GetNumberOfBlocks(ref MyObjectBuilder_Definitions prefab)
		{
			int num = 0;
			MyObjectBuilder_CubeGrid[] cubeGrids = prefab.ShipBlueprints[0].CubeGrids;
			foreach (MyObjectBuilder_CubeGrid myObjectBuilder_CubeGrid in cubeGrids)
			{
				num += myObjectBuilder_CubeGrid.CubeBlocks.Count;
			}
			return num;
		}

		public static MyGuiControlButton CreateButton(MyGuiScreenDebugBase screen, float usableWidth, StringBuilder text, Action<MyGuiControlButton> onClick, bool enabled = true, MyStringId? tooltip = null, float textScale = 1f)
		{
			MyGuiControlButton myGuiControlButton = screen.AddButton(text, onClick);
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
			myGuiControlButton.TextScale = textScale;
			myGuiControlButton.Size = new Vector2(usableWidth, myGuiControlButton.Size.Y);
			myGuiControlButton.Position += new Vector2(-0.02f, 0f);
			myGuiControlButton.Enabled = enabled;
			if (tooltip.HasValue)
			{
				myGuiControlButton.SetToolTip(tooltip.Value);
			}
			return myGuiControlButton;
		}

		public static MyGuiControlButton CreateButtonString(MyGuiScreenDebugBase screen, float usableWidth, StringBuilder text, Action<MyGuiControlButton> onClick, bool enabled = true, string tooltip = null, float textScale = 1f)
		{
			MyGuiControlButton myGuiControlButton = screen.AddButton(text, onClick);
			myGuiControlButton.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
			myGuiControlButton.TextScale = textScale;
			myGuiControlButton.Size = new Vector2(usableWidth, myGuiControlButton.Size.Y);
			myGuiControlButton.Position += new Vector2(-0.02f, 0f);
			myGuiControlButton.Enabled = enabled;
			if (tooltip != null)
			{
				myGuiControlButton.SetToolTip(tooltip);
			}
			return myGuiControlButton;
		}

		public static void PublishScript(string localPath, MyBlueprintItemInfo script, Action OnPublished)
		{
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.LoadScreenButtonPublish), messageText: MyTexts.Get(MySpaceTexts.ProgrammableBlock_PublishScriptDialogText), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum val)
			{
				if (val == MyGuiScreenMessageBox.ResultEnum.YES)
				{
					MyGuiSandbox.AddScreen(new MyGuiScreenWorkshopTags("ingameScript", MyWorkshop.ScriptCategories, null, delegate(MyGuiScreenMessageBox.ResultEnum x, string[] y, string[] z)
					{
						OnPublishScriptTagsResult(localPath, script, OnPublished, x, y, z);
					}));
				}
			}));
		}

		private static void OnPublishScriptTagsResult(string localPath, MyBlueprintItemInfo script, Action OnPublished, MyGuiScreenMessageBox.ResultEnum tagScreenResult, string[] outTags, string[] serviceNames)
		{
			if (tagScreenResult != 0)
			{
				return;
			}
			WorkshopId[] workshopIds = MyWorkshop.FilterWorkshopIds(MyWorkshop.GetWorkshopIdFromLocalScript(script.Data.Name, new WorkshopId(script.Item?.Id ?? 0, script.Item?.ServiceName)), serviceNames);
			MyWorkshop.PublishIngameScriptAsync(localPath, script.Data.Name, script.Data.Description ?? "", workshopIds, outTags, MyPublishedFileVisibility.Public, delegate(bool success, MyGameServiceCallResult result, string resultServiceName, MyWorkshopItem[] publishedFiles)
			{
				if (publishedFiles.Length != 0)
				{
					MyWorkshop.GenerateModInfo(localPath, publishedFiles, Sync.MyId);
				}
				MyWorkshop.ReportPublish(publishedFiles, result, resultServiceName, OnPublished);
			});
		}

		public static bool IsItem_Blueprint(string path)
		{
			return File.Exists(path + "\\bp.sbc");
		}

		public static bool IsItem_Script(string path)
		{
			return File.Exists(path + "\\Script.cs");
		}

		private MyBlueprintUtils()
		{
		}

		public static void CreateModIoConsentScreen(Action onConsentAgree = null, Action onConsentOptOut = null)
		{
			MyModIoConsentViewModel viewModel = new MyModIoConsentViewModel(onConsentAgree, onConsentOptOut);
			ServiceManager.Instance.GetService<IMyGuiScreenFactoryService>().CreateScreen(viewModel);
		}

		public static void OpenBlueprintScreen(MyGridClipboard clipboard, bool allowCopyToClipboard, MyBlueprintAccessType accessType, Action<MyGuiBlueprintScreen_Reworked> onOpened)
		{
			MyGuiBlueprintScreen_Reworked myGuiBlueprintScreen_Reworked = MyGuiBlueprintScreen_Reworked.CreateBlueprintScreen(clipboard, allowCopyToClipboard, accessType);
			onOpened?.Invoke(myGuiBlueprintScreen_Reworked);
			MyGuiSandbox.AddScreen(myGuiBlueprintScreen_Reworked);
		}

		public static void OpenBlueprintScreen()
		{
			OpenBlueprintScreen(MyClipboardComponent.Static.Clipboard, MySession.Static.CreativeMode || MySession.Static.CreativeToolsEnabled(Sync.MyId), MyBlueprintAccessType.NORMAL, null);
		}

		public static void OpenScriptScreen(Action<string> scriptSelected, Func<string> getCode, Action workshopWindowClosed)
		{
			if (MyFakes.I_AM_READY_FOR_NEW_SCRIPT_SCREEN)
			{
				MyGuiSandbox.AddScreen(MyGuiBlueprintScreen_Reworked.CreateScriptScreen(scriptSelected, getCode, workshopWindowClosed));
			}
			else
			{
				MyGuiSandbox.AddScreen(new MyGuiIngameScriptsPage(scriptSelected, getCode, workshopWindowClosed));
			}
		}
	}
}
