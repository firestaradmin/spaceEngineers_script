using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.FileSystem;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenLoadSandbox : MyGuiScreenBase
	{
		public static readonly string CONST_BACKUP = "//Backup";

		public static bool ENABLE_SCENARIO_EDIT = false;

		private MyGuiControlSaveBrowser m_saveBrowser;

		private MyGuiControlButton m_continueLastSave;

		private MyGuiControlButton m_loadButton;

		private MyGuiControlButton m_editButton;

		private MyGuiControlButton m_saveButton;

		private MyGuiControlButton m_deleteButton;

		private MyGuiControlButton m_publishButton;

		private MyGuiControlButton m_backupsButton;

		private int m_selectedRow;

		private int m_lastSelectedRow;

		private bool m_rowAutoSelect = true;

		private MyGuiControlRotatingWheel m_loadingWheel;

		private MyGuiControlImage m_levelImage;

		private bool m_parallelLoadIsRunning;

		private MyGuiControlSearchBox m_searchBox;

		private bool m_isEditable;

		public MyGuiScreenLoadSandbox()
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.924f, 0.97f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			AddCaption(MyCommonTexts.ScreenMenuButtonLoadGame, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.872f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.859f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			Vector2 start = new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.872f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f);
			myGuiControlSeparatorList2.AddHorizontal(start, m_size.Value.X * 0.859f);
			Controls.Add(myGuiControlSeparatorList2);
			MyGuiControlSeparatorList myGuiControlSeparatorList3 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList3.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.87f / 2f, m_size.Value.Y / 2f - 0.25f), m_size.Value.X * 0.188f);
			Controls.Add(myGuiControlSeparatorList3);
			Vector2 vector = new Vector2(-0.401f, -0.39f);
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			m_searchBox = new MyGuiControlSearchBox(vector + new Vector2(minSizeGui.X * 1.1f - 0.004f, 0.017f));
			m_searchBox.OnTextChanged += OnSearchTextChange;
			m_searchBox.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
			m_searchBox.Size = new Vector2(1075f / MyGuiConstants.GUI_OPTIMAL_SIZE.X * 0.898f, m_searchBox.Size.Y);
			Controls.Add(m_searchBox);
			m_saveBrowser = new MyGuiControlSaveBrowser();
			m_saveBrowser.Position = vector + new Vector2(minSizeGui.X * 1.1f - 0.004f, 0.055f);
			m_saveBrowser.Size = new Vector2(1075f / MyGuiConstants.GUI_OPTIMAL_SIZE.X * 0.898f, 0.15f);
			m_saveBrowser.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_saveBrowser.VisibleRowsCount = 19;
			m_saveBrowser.ItemSelected += OnTableItemSelected;
			m_saveBrowser.BrowserItemConfirmed += OnTableItemConfirmedOrDoubleClick;
			m_saveBrowser.SelectedRowIndex = 0;
			m_saveBrowser.GamepadHelpTextId = MySpaceTexts.LoadScreen_Help_Load;
			m_saveBrowser.UpdateTableSortHelpText();
			Controls.Add(m_saveBrowser);
			Vector2 vector2 = vector + minSizeGui * 0.5f;
			vector2.Y += 0.002f;
			Vector2 mENU_BUTTONS_POSITION_DELTA = MyGuiConstants.MENU_BUTTONS_POSITION_DELTA;
			vector2.Y += 0.192f;
			m_editButton = MakeButton(vector2 + mENU_BUTTONS_POSITION_DELTA * -0.25f, MyCommonTexts.LoadScreenButtonEditSettings, OnEditClick);
			m_editButton.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipLoadGame_EditSettings));
			Controls.Add(m_editButton);
			m_publishButton = MakeButton(vector2 + mENU_BUTTONS_POSITION_DELTA * 0.75f, MyCommonTexts.LoadScreenButtonPublish, OnPublishClick);
			m_publishButton.SetToolTip(MyCommonTexts.LoadScreenButtonTooltipPublish);
			Controls.Add(m_publishButton);
			m_backupsButton = MakeButton(vector2 + mENU_BUTTONS_POSITION_DELTA * 1.75f, MyCommonTexts.LoadScreenButtonBackups, OnBackupsButtonClick);
			m_backupsButton.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipLoadGame_Backups));
			Controls.Add(m_backupsButton);
			m_saveButton = MakeButton(vector2 + mENU_BUTTONS_POSITION_DELTA * 2.75f, MyCommonTexts.LoadScreenButtonSaveAs, OnSaveAsClick);
			m_saveButton.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipLoadGame_SaveAs));
			Controls.Add(m_saveButton);
			m_deleteButton = MakeButton(vector2 + mENU_BUTTONS_POSITION_DELTA * 3.75f, MyCommonTexts.LoadScreenButtonDelete, OnDeleteClick);
			m_deleteButton.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipLoadGame_Delete));
			Controls.Add(m_deleteButton);
			Vector2 position = vector2 + mENU_BUTTONS_POSITION_DELTA * -3.65f;
			position.X -= m_publishButton.Size.X / 2f + 0.002f;
			m_levelImage = new MyGuiControlImage
			{
				Size = new Vector2(m_publishButton.Size.X, m_publishButton.Size.X / 4f * 3f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Position = position,
				BorderEnabled = true,
				BorderSize = 1,
				BorderColor = new Vector4(0.235f, 0.274f, 0.314f, 1f)
			};
			m_levelImage.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
			Controls.Add(m_levelImage);
			m_loadButton = MakeButton(new Vector2(0f, 0f) - new Vector2(-0.307f, (0f - m_size.Value.Y) / 2f + 0.071f), MyCommonTexts.LoadScreenButtonLoad, OnLoadClick);
			m_loadButton.SetTooltip(MyTexts.GetString(MySpaceTexts.ToolTipLoadGame_Load));
			Controls.Add(m_loadButton);
			m_loadingWheel = new MyGuiControlRotatingWheel(m_loadButton.Position + new Vector2(0.273f, -0.008f), MyGuiConstants.ROTATING_WHEEL_COLOR, 0.22f);
			Controls.Add(m_loadingWheel);
			m_loadingWheel.Visible = false;
			m_loadButton.DrawCrossTextureWhenDisabled = false;
			m_editButton.DrawCrossTextureWhenDisabled = false;
			m_deleteButton.DrawCrossTextureWhenDisabled = false;
			m_saveButton.DrawCrossTextureWhenDisabled = false;
			m_publishButton.DrawCrossTextureWhenDisabled = false;
			SetButtonsVisibility(!MyInput.Static.IsJoystickLastUsed);
			if (MyPlatformGameSettings.GAME_SAVES_TO_CLOUD)
			{
				Controls.Add(new MySCloudStorageQuotaBar(new Vector2(-0.3145f, 0.3175f)));
			}
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(start.X, m_loadButton.Position.Y));
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			base.GamepadHelpTextId = MySpaceTexts.LoadScreen_Help_Screen;
			base.CloseButtonEnabled = true;
			base.FocusedControl = m_searchBox.TextBox;
			m_saveBrowser.Sort(switchSort: false);
<<<<<<< HEAD
			m_levelImage.SetTexture();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void DebugOverrideAutosaveCheckboxIsCheckChanged(MyGuiControlCheckbox checkbox)
		{
			MySandboxGame.Config.DebugOverrideAutosave = checkbox.IsChecked;
			MySandboxGame.Config.Save();
		}

		private void DebugWorldCheckboxIsCheckChanged(MyGuiControlCheckbox checkbox)
		{
			string topMostAndCurrentDir = (checkbox.IsChecked ? Path.Combine(MyFileSystem.ContentPath, "Worlds") : MyFileSystem.SavesPath);
			m_saveBrowser.SetTopMostAndCurrentDir(topMostAndCurrentDir);
			m_saveBrowser.Refresh();
		}

		private void OnBackupsButtonClick(MyGuiControlButton myGuiControlButton)
		{
			m_saveBrowser.AccessBackups();
		}

		private void OnTableItemConfirmedOrDoubleClick(MyGuiControlTable table, MyGuiControlTable.EventArgs args)
		{
			LoadSandbox();
		}

		private MyGuiControlButton MakeButton(Vector2 position, MyStringId text, Action<MyGuiControlButton> onClick)
		{
			return new MyGuiControlButton(position, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, null, MyTexts.Get(text), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, onClick);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenLoadSandbox";
		}

		private void OnSearchTextChange(string text)
		{
			m_saveBrowser.SearchTextFilter = text;
			m_saveBrowser.RefreshAfterLoaded();
		}

		private void OnLoadClick(MyGuiControlButton sender)
		{
			LoadSandbox();
		}

		private void OnBackClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}

		private void DeleteThumbCache()
<<<<<<< HEAD
		{
			string path = Path.Combine(MyFileSystem.TempPath, "thumbs");
			if (Directory.Exists(path))
			{
				try
				{
					Directory.Delete(path, recursive: true);
				}
				catch
				{
				}
			}
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
=======
		{
			string text = Path.Combine(MyFileSystem.TempPath, "thumbs");
			if (Directory.Exists(text))
			{
				try
				{
					Directory.Delete(text, true);
				}
				catch
				{
				}
			}
		}

		public override bool CloseScreen(bool isUnloading = false)
		{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			DeleteThumbCache();
			return base.CloseScreen(isUnloading);
		}

		private void OnEditClick(MyGuiControlButton sender)
		{
			MyGuiControlTable.Row selectedRow = m_saveBrowser.SelectedRow;
			if (selectedRow == null)
			{
				return;
			}
			m_saveBrowser.GetSave(selectedRow, out var info);
			if (info.Valid && !info.WorldInfo.IsCorrupted)
			{
				ulong sizeInBytes;
				MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = ((!info.IsCloud) ? MyLocalCache.LoadCheckpoint(info.Name, out sizeInBytes) : MyLocalCache.LoadCheckpointFromCloud(info.Name, out sizeInBytes));
				if (myObjectBuilder_Checkpoint != null)
				{
					if (!m_isEditable)
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MySpaceTexts.WorldFileCouldNotBeEdited)));
						return;
					}
					MyGuiScreenBase myGuiScreenBase = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.EditWorldSettingsScreen, myObjectBuilder_Checkpoint, info.Name, true, true, true, info.IsCloud);
					myGuiScreenBase.Closed += delegate
					{
						m_saveBrowser.ForceRefresh();
					};
					MyGuiSandbox.AddScreen(myGuiScreenBase);
					m_rowAutoSelect = true;
					return;
				}
			}
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.WorldFileCouldNotBeLoaded)));
		}

		private void OnSaveAsClick(MyGuiControlButton sender)
		{
			MyGuiControlTable.Row selectedRow = m_saveBrowser.SelectedRow;
			if (selectedRow != null)
			{
				m_saveBrowser.GetSave(selectedRow, out var info);
				if (info.Valid)
				{
					MyGuiScreenSaveAs myGuiScreenSaveAs = new MyGuiScreenSaveAs(info.WorldInfo, info.Name, null, info.IsCloud);
					myGuiScreenSaveAs.SaveAsConfirm += OnSaveAsConfirm;
					MyGuiSandbox.AddScreen(myGuiScreenSaveAs);
				}
			}
		}

		private void OnSaveAsConfirm(CloudResult result)
		{
			if (MyCloudHelper.IsError(result, out var errorMessage))
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(errorMessage), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
			}
			else
			{
				m_saveBrowser.ForceRefresh();
			}
		}

		private void OnDeleteClick(MyGuiControlButton sender)
		{
			if (m_saveBrowser.SelectedRows.Count > 0)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, new StringBuilder(MyTexts.GetString(MyCommonTexts.MessageBoxTextAreYouSureYouWantToDeleteMultipleSaves)), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, OnDeleteConfirmMultiple));
				return;
			}
			MyGuiControlTable.Row selectedRow = m_saveBrowser.SelectedRow;
			if (selectedRow == null)
			{
				return;
			}
			m_saveBrowser.GetSave(selectedRow, out var info);
			if (info.Valid)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextAreYouSureYouWantToDeleteSave, info.WorldInfo.SessionName), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, OnDeleteConfirm));
				return;
			}
			DirectoryInfo directory = m_saveBrowser.GetDirectory(selectedRow);
			if (directory != null)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, new StringBuilder().AppendFormat(MyCommonTexts.MessageBoxTextAreYouSureYouWantToDeleteSave, ((FileSystemInfo)directory).get_Name()), MyTexts.Get(MyCommonTexts.MessageBoxCaptionPleaseConfirm), null, null, null, null, OnDeleteConfirm));
			}
		}

		private void OnDeleteConfirm(MyGuiScreenMessageBox.ResultEnum callbackReturn)
		{
			if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MyGuiControlTable.Row selectedRow = m_saveBrowser.SelectedRow;
				if (selectedRow == null)
				{
					return;
				}
				m_saveBrowser.GetSave(selectedRow, out var info);
				if (info.Valid)
				{
					try
					{
						if (info.IsCloud)
						{
							MyCloudHelper.Delete(info.Name);
<<<<<<< HEAD
							Directory.Delete(MyCloudHelper.CloudToLocalWorldPath(info.Name), recursive: true);
						}
						else
						{
							Directory.Delete(info.Name, recursive: true);
=======
							Directory.Delete(MyCloudHelper.CloudToLocalWorldPath(info.Name), true);
						}
						else
						{
							Directory.Delete(info.Name, true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						m_saveBrowser.RemoveSelectedRow();
						m_saveBrowser.SelectedRowIndex = m_selectedRow;
						m_saveBrowser.Refresh();
						m_levelImage.SetTexture();
					}
					catch (Exception)
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.SessionDeleteFailed)));
					}
				}
				else
				{
					try
					{
						DirectoryInfo directory = m_saveBrowser.GetDirectory(selectedRow);
						if (directory != null)
						{
							directory.Delete(true);
							m_saveBrowser.Refresh();
						}
					}
					catch (Exception)
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.SessionDeleteFailed)));
					}
				}
			}
			m_rowAutoSelect = true;
		}

		private void OnDeleteConfirmMultiple(MyGuiScreenMessageBox.ResultEnum callbackReturn)
		{
			if (callbackReturn == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				foreach (MyGuiControlTable.Row selectedRow in m_saveBrowser.SelectedRows)
				{
					if (selectedRow == null)
					{
						return;
					}
					m_saveBrowser.GetSave(selectedRow, out var info);
					if (info.Valid)
					{
						try
						{
							if (info.IsCloud)
							{
								MyCloudHelper.Delete(info.Name);
								Directory.Delete(MyCloudHelper.CloudToLocalWorldPath(info.Name), recursive: true);
							}
							else
							{
								Directory.Delete(info.Name, recursive: true);
							}
							m_saveBrowser.Remove(selectedRow);
							m_levelImage.SetTexture();
						}
						catch (Exception)
						{
							MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.SessionDeleteFailed)));
						}
					}
					else
					{
						try
						{
							m_saveBrowser.GetDirectory(selectedRow)?.Delete(recursive: true);
						}
						catch (Exception)
						{
							MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.SessionDeleteFailed)));
						}
					}
				}
			}
			m_saveBrowser.SelectedRowsIndexes = new List<int>();
			m_saveBrowser.Refresh();
			m_rowAutoSelect = true;
		}

		private void OnContinueLastGameClick(MyGuiControlButton sender)
		{
			MySessionLoader.LoadLastSession();
			m_continueLastSave.Enabled = false;
		}

		private void OnPublishClick(MyGuiControlButton sender)
		{
			MyGuiControlTable.Row selectedRow = m_saveBrowser.SelectedRow;
			if (selectedRow != null)
			{
				m_saveBrowser.GetSave(selectedRow, out var info);
				if (info.Valid)
				{
					Publish(info.Name, info.WorldInfo, info.IsCloud);
				}
			}
		}

		public static void Publish(string sessionPath, MyWorldInfo worlInfo, bool isCloud)
		{
			WorkshopId[] workshopIds = worlInfo.WorkshopIds;
			if (workshopIds != null && workshopIds.Length != 0)
			{
				StringBuilder messageText = new StringBuilder(MyTexts.GetString(MyCommonTexts.MessageBoxTextDoYouWishToUpdateWorld));
				MyStringId messageBoxCaptionDoYouWishToUpdateWorld = MyCommonTexts.MessageBoxCaptionDoYouWishToUpdateWorld;
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, messageText, MyTexts.Get(messageBoxCaptionDoYouWishToUpdateWorld), null, null, null, null, delegate(MyGuiScreenMessageBox.ResultEnum val)
				{
					if (val == MyGuiScreenMessageBox.ResultEnum.YES)
					{
						UploadToWorkshop(sessionPath, worlInfo, isCloud);
					}
				}));
			}
			else
			{
				UploadToWorkshop(sessionPath, worlInfo, isCloud);
			}
		}

		private static void UploadToWorkshop(string sessionPath, MyWorldInfo worldInfo, bool isCloud)
		{
			Action<MyGuiScreenMessageBox.ResultEnum, string[], string[]> action = delegate(MyGuiScreenMessageBox.ResultEnum tagsResult, string[] outTags, string[] serviceNames)
			{
				if (tagsResult == MyGuiScreenMessageBox.ResultEnum.YES && serviceNames != null && serviceNames.Length != 0)
				{
					string originalSessionPath = sessionPath;
					if (isCloud)
					{
						string text = Path.Combine(MyFileSystem.TempPath, Path.GetFileName(Path.GetDirectoryName(sessionPath)));
						if (!MyCloudHelper.ExtractFilesTo(sessionPath, text, unpack: true))
						{
							MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.MessageBoxTextCloudExtractError), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWorldPublishFailed)));
							return;
						}
						sessionPath = text;
					}
					ulong sizeInBytes;
					MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out sizeInBytes);
					if (checkpoint == null)
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.MessageBoxTextLoadWorldError), MyTexts.Get(MyCommonTexts.MessageBoxCaptionWorldPublishFailed)));
					}
					else
					{
						Array.Resize(ref outTags, 1 + ((outTags != null) ? outTags.Length : 0));
						if (checkpoint.Settings.IsSettingsExperimental(remote: true))
<<<<<<< HEAD
						{
							outTags[outTags.Length - 1] = MySteamConstants.TAG_EXPERIMENTAL;
						}
						else
						{
							outTags[outTags.Length - 1] = MySteamConstants.TAG_SAFE;
						}
						WorkshopId[] array = MyWorkshop.FilterWorkshopIds(worldInfo.WorkshopIds, serviceNames);
						WorkshopId[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
=======
						{
							outTags[outTags.Length - 1] = MySteamConstants.TAG_EXPERIMENTAL;
						}
						else
						{
							outTags[outTags.Length - 1] = MySteamConstants.TAG_SAFE;
						}
						WorkshopId[] array = MyWorkshop.FilterWorkshopIds(worldInfo.WorkshopIds, serviceNames);
						WorkshopId[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							WorkshopId workshopId = array2[i];
							IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(workshopId.ServiceName);
							if (aggregate != null && !aggregate.IsConsentGiven)
							{
								MySessionLoader.ShowUGCConsentNeededForThisServiceWarning();
								return;
							}
						}
						MyWorkshop.PublishWorldAsync(sessionPath, worldInfo.SessionName, worldInfo.Description, array, outTags, MyPublishedFileVisibility.Public, delegate(bool success, MyGameServiceCallResult result, string resultServiceName, MyWorkshopItem[] publishedFiles)
						{
							if (publishedFiles != null && publishedFiles.Length != 0)
							{
								worldInfo.WorkshopIds = publishedFiles.ToWorkshopIds();
								checkpoint.WorkshopId = publishedFiles[0].Id;
								checkpoint.WorkshopServiceName = publishedFiles[0].ServiceName;
								if (publishedFiles.Length > 1)
								{
									checkpoint.WorkshopId1 = publishedFiles[1].Id;
									checkpoint.WorkshopServiceName1 = publishedFiles[1].ServiceName;
								}
								if (isCloud)
								{
									MyLocalCache.SaveCheckpointToCloud(checkpoint, originalSessionPath);
								}
								else
								{
									MyLocalCache.SaveCheckpoint(checkpoint, sessionPath);
								}
							}
							MyWorkshop.ReportPublish(publishedFiles, result, resultServiceName);
						});
					}
				}
			};
			if (MyWorkshop.WorldCategories.Length != 0)
			{
				MyGuiSandbox.AddScreen(new MyGuiScreenWorkshopTags("world", MyWorkshop.WorldCategories, null, action));
				return;
			}
			action(MyGuiScreenMessageBox.ResultEnum.YES, new string[1] { "world" }, MyGameService.GetUGCNamesList());
		}

		private void OnTableItemSelected(MyGuiControlTable sender, MyGuiControlTable.EventArgs eventArgs)
		{
			sender.CanHaveFocus = true;
			base.FocusedControl = sender;
			m_selectedRow = eventArgs.RowIndex;
			m_lastSelectedRow = m_selectedRow;
			LoadImagePreview();
		}

		private void LoadImagePreview()
		{
<<<<<<< HEAD
			MyGuiControlTable.Row row = ((m_saveBrowser.SelectedRows == null || m_saveBrowser.SelectedRows.Count <= 0) ? null : m_saveBrowser.SelectedRows[m_saveBrowser.SelectedRows.Count - 1]);
			if (row != null)
			{
				m_saveBrowser.GetSave(row, out var info);
				if (!info.Valid || info.WorldInfo.IsCorrupted)
				{
					m_levelImage.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
					return;
				}
				if (info.IsCloud)
				{
					string thumbDir = Path.Combine(MyFileSystem.TempPath, "thumbs/");
					string thumbImage = Path.Combine(thumbDir, info.Name.GetHashCode().ToString());
					thumbImage += ".jpg";
					if (File.Exists(thumbImage))
					{
						m_levelImage.SetTexture(thumbImage);
						return;
					}
					MyGameService.LoadFromCloudAsync(MyCloudHelper.Combine(info.Name, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION), delegate(byte[] data)
					{
						if (data != null)
						{
							try
							{
								Directory.CreateDirectory(thumbDir);
								File.WriteAllBytes(thumbImage, data);
								MyRenderProxy.UnloadTexture(thumbImage);
								m_levelImage.SetTexture(thumbImage);
								return;
							}
							catch
							{
							}
						}
						m_levelImage.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
					});
					return;
				}
				string name = info.Name;
				if (Directory.Exists(name + CONST_BACKUP))
				{
					string[] directories = Directory.GetDirectories(name + CONST_BACKUP);
					if (directories.Any())
					{
						string text = Path.Combine(directories.Last(), MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION);
						if (File.Exists(text) && new FileInfo(text).Length > 0)
						{
							m_levelImage.SetTexture(Path.Combine(Directory.GetDirectories(name + CONST_BACKUP).Last().ToString(), MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION));
							return;
						}
					}
				}
				string text2 = Path.Combine(name, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION);
				if (File.Exists(text2) && new FileInfo(text2).Length > 0)
				{
					m_levelImage.SetTexture();
					m_levelImage.SetTexture(Path.Combine(name, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION));
				}
				else
				{
					m_levelImage.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
				}
			}
			else
			{
				m_levelImage.SetTexture();
			}
		}

		private void LoadSandbox()
		{
			if (!m_parallelLoadIsRunning)
			{
				m_parallelLoadIsRunning = true;
				MyGuiScreenProgress progressScreen = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgressScreen_LoadingWorld));
				MyScreenManager.AddScreen(progressScreen);
				Parallel.StartBackground(delegate
				{
					LoadSandboxInternal();
				}, delegate
				{
					progressScreen.CloseScreen();
					m_parallelLoadIsRunning = false;
				});
			}
		}

		private void LoadSandboxInternal()
		{
=======
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyGuiControlTable.Row selectedRow = m_saveBrowser.SelectedRow;
			if (selectedRow == null)
			{
				return;
			}
<<<<<<< HEAD
			m_saveBrowser.GetSave(selectedRow, out var save);
			if (m_saveBrowser.GetDirectory(selectedRow) != null)
=======
			m_saveBrowser.GetSave(selectedRow, out var info);
			if (!info.Valid || info.WorldInfo.IsCorrupted)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
<<<<<<< HEAD
			if (!save.Valid || save.WorldInfo.IsCorrupted)
			{
				MySandboxGame.Static.Invoke(delegate
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.WorldFileCouldNotBeLoaded)));
				}, "New Game screen");
				return;
			}
			ulong sizeInBytes;
			MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = ((!save.IsCloud) ? MyLocalCache.LoadCheckpoint(save.Name, out sizeInBytes) : MyLocalCache.LoadCheckpointFromCloud(save.Name, out sizeInBytes));
			if (MySessionLoader.HasOnlyModsFromConsentedUGCs(myObjectBuilder_Checkpoint))
			{
				MyStringId errorMessage;
				if (myObjectBuilder_Checkpoint != null && myObjectBuilder_Checkpoint.OnlineMode != 0)
				{
					MyGameService.Service.RequestPermissions(Permissions.Multiplayer, attemptResolution: true, delegate(PermissionResult granted)
					{
						switch (granted)
						{
						case PermissionResult.Granted:
							MyGameService.Service.RequestPermissions(Permissions.UGC, attemptResolution: true, delegate(PermissionResult ugcGranted)
							{
								switch (ugcGranted)
								{
								case PermissionResult.Granted:
									BackupAndLoadSandbox(ref save);
									break;
								case PermissionResult.Error:
									MySandboxGame.Static.Invoke(delegate
									{
										MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, default(MyStringId), MyMessageBoxStyleEnum.Info);
									}, "New Game screen");
									break;
								}
							});
							break;
						case PermissionResult.Error:
							MySandboxGame.Static.Invoke(delegate
							{
								MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, default(MyStringId), MyMessageBoxStyleEnum.Info);
							}, "New Game screen");
							break;
						}
					});
				}
				else if (MyCloudHelper.IsError(BackupAndLoadSandbox(ref save), out errorMessage, MySpaceTexts.WorldSettings_Error_SavingFailed))
				{
					MySandboxGame.Static.Invoke(delegate
					{
						MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(errorMessage), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
						myGuiScreenMessageBox.SkipTransition = true;
						myGuiScreenMessageBox.InstantClose = false;
						MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
					}, "New Game screen");
				}
=======
			if (info.IsCloud)
			{
				string thumbDir = Path.Combine(MyFileSystem.TempPath, "thumbs/");
				string thumbImage = Path.Combine(thumbDir, info.Name.GetHashCode().ToString());
				thumbImage += ".jpg";
				if (File.Exists(thumbImage))
				{
					m_levelImage.SetTexture(thumbImage);
					return;
				}
				MyGameService.LoadFromCloudAsync(MyCloudHelper.Combine(info.Name, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION), delegate(byte[] data)
				{
					if (data != null)
					{
						try
						{
							Directory.CreateDirectory(thumbDir);
							File.WriteAllBytes(thumbImage, data);
							MyRenderProxy.UnloadTexture(thumbImage);
							m_levelImage.SetTexture(thumbImage);
							return;
						}
						catch
						{
						}
					}
					m_levelImage.SetTexture("Textures\\GUI\\Screens\\image_background.dds");
				});
				return;
			}
			string name = info.Name;
			if (Directory.Exists(name + CONST_BACKUP))
			{
				string[] directories = Directory.GetDirectories(name + CONST_BACKUP);
				if (Enumerable.Any<string>((IEnumerable<string>)directories))
				{
					string text = Path.Combine(Enumerable.Last<string>((IEnumerable<string>)directories), MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION);
					if (File.Exists(text) && new FileInfo(text).get_Length() > 0)
					{
						m_levelImage.SetTexture(Path.Combine(Enumerable.Last<string>((IEnumerable<string>)Directory.GetDirectories(name + CONST_BACKUP)).ToString(), MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION));
						return;
					}
				}
			}
			string text2 = Path.Combine(name, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION);
			if (File.Exists(text2) && new FileInfo(text2).get_Length() > 0)
			{
				m_levelImage.SetTexture();
				m_levelImage.SetTexture(Path.Combine(name, MyTextConstants.SESSION_THUMB_NAME_AND_EXTENSION));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				MySessionLoader.ShowUGCConsentNotAcceptedWarning(MySessionLoader.GetNonConsentedServiceNameInCheckpoint(myObjectBuilder_Checkpoint));
			}
		}

		private CloudResult BackupAndLoadSandbox(ref MySaveInfo saveInfo)
		{
<<<<<<< HEAD
			DeleteThumbCache();
			bool gAME_SAVES_TO_CLOUD = MyPlatformGameSettings.GAME_SAVES_TO_CLOUD;
			MyLog.Default.WriteLine("LoadSandbox() - Start");
			string saveFilePath = saveInfo.Name;
			if (m_saveBrowser.InBackupsFolder)
			{
				if (gAME_SAVES_TO_CLOUD)
				{
					CloudResult cloudResult = CopyBackupToCloud(saveFilePath);
					if (cloudResult != 0)
					{
						return cloudResult;
					}
				}
=======
			if (!m_parallelLoadIsRunning)
			{
				m_parallelLoadIsRunning = true;
				MyGuiScreenProgress progressScreen = new MyGuiScreenProgress(MyTexts.Get(MySpaceTexts.ProgressScreen_LoadingWorld));
				MyScreenManager.AddScreen(progressScreen);
				Parallel.StartBackground(delegate
				{
					LoadSandboxInternal();
				}, delegate
				{
					progressScreen.CloseScreen();
					m_parallelLoadIsRunning = false;
				});
			}
		}

		private void LoadSandboxInternal()
		{
			MyGuiControlTable.Row selectedRow = m_saveBrowser.SelectedRow;
			if (selectedRow == null)
			{
				return;
			}
			m_saveBrowser.GetSave(selectedRow, out var save);
			if (m_saveBrowser.GetDirectory(selectedRow) != null)
			{
				return;
			}
			if (!save.Valid || save.WorldInfo.IsCorrupted)
			{
				MySandboxGame.Static.Invoke(delegate
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.WorldFileCouldNotBeLoaded)));
				}, "New Game screen");
				return;
			}
			ulong sizeInBytes;
			MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = ((!save.IsCloud) ? MyLocalCache.LoadCheckpoint(save.Name, out sizeInBytes) : MyLocalCache.LoadCheckpointFromCloud(save.Name, out sizeInBytes));
			if (MySessionLoader.HasOnlyModsFromConsentedUGCs(myObjectBuilder_Checkpoint))
			{
				MyStringId errorMessage;
				if (myObjectBuilder_Checkpoint != null && myObjectBuilder_Checkpoint.OnlineMode != 0)
				{
					MyGameService.Service.RequestPermissions(Permissions.Multiplayer, attemptResolution: true, delegate(PermissionResult granted)
					{
						switch (granted)
						{
						case PermissionResult.Granted:
							MyGameService.Service.RequestPermissions(Permissions.UGC, attemptResolution: true, delegate(PermissionResult ugcGranted)
							{
								switch (ugcGranted)
								{
								case PermissionResult.Granted:
									BackupAndLoadSandbox(ref save);
									break;
								case PermissionResult.Error:
									MySandboxGame.Static.Invoke(delegate
									{
										MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, default(MyStringId), MyMessageBoxStyleEnum.Info);
									}, "New Game screen");
									break;
								}
							});
							break;
						case PermissionResult.Error:
							MySandboxGame.Static.Invoke(delegate
							{
								MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, default(MyStringId), MyMessageBoxStyleEnum.Info);
							}, "New Game screen");
							break;
						}
					});
				}
				else if (MyCloudHelper.IsError(BackupAndLoadSandbox(ref save), out errorMessage, MySpaceTexts.WorldSettings_Error_SavingFailed))
				{
					MySandboxGame.Static.Invoke(delegate
					{
						MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(errorMessage), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
						myGuiScreenMessageBox.SkipTransition = true;
						myGuiScreenMessageBox.InstantClose = false;
						MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
					}, "New Game screen");
				}
			}
			else
			{
				MySessionLoader.ShowUGCConsentNotAcceptedWarning(MySessionLoader.GetNonConsentedServiceNameInCheckpoint(myObjectBuilder_Checkpoint));
			}
		}

		private CloudResult BackupAndLoadSandbox(ref MySaveInfo saveInfo)
		{
			DeleteThumbCache();
			bool gAME_SAVES_TO_CLOUD = MyPlatformGameSettings.GAME_SAVES_TO_CLOUD;
			MyLog.Default.WriteLine("LoadSandbox() - Start");
			string saveFilePath = saveInfo.Name;
			if (m_saveBrowser.InBackupsFolder)
			{
				if (gAME_SAVES_TO_CLOUD)
				{
					CloudResult cloudResult = CopyBackupToCloud(saveFilePath);
					if (cloudResult != 0)
					{
						return cloudResult;
					}
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				CopyBackupUpALevel(ref saveFilePath, saveInfo.WorldInfo);
			}
			else if (saveInfo.IsCloud)
			{
				string text = MyCloudHelper.CloudToLocalWorldPath(saveFilePath);
				if (!MyCloudHelper.ExtractFilesTo(saveFilePath, text, unpack: false))
				{
					return CloudResult.Failed;
				}
				saveFilePath = text;
			}
			MySessionLoader.LoadSingleplayerSession(saveFilePath);
			MyLog.Default.WriteLine("LoadSandbox() - End");
			return CloudResult.Ok;
		}

		private CloudResult CopyBackupToCloud(string saveFilePath)
		{
<<<<<<< HEAD
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(saveFilePath);
				return MyCloudHelper.UploadFiles(MyCloudHelper.LocalToCloudWorldPath(directoryInfo.Parent.Parent.FullName + "\\"), directoryInfo, compress: false);
=======
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			try
			{
				DirectoryInfo val = new DirectoryInfo(saveFilePath);
				return MyCloudHelper.UploadFiles(MyCloudHelper.LocalToCloudWorldPath(((FileSystemInfo)val.get_Parent().get_Parent()).get_FullName() + "\\"), val, compress: false);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch
			{
				return CloudResult.Failed;
			}
		}

		private void CopyBackupUpALevel(ref string saveFilePath, MyWorldInfo worldInfo)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			DirectoryInfo val = new DirectoryInfo(saveFilePath);
			DirectoryInfo targetDirectory = val.get_Parent().get_Parent();
			targetDirectory.GetFiles().ForEach(delegate(FileInfo file)
			{
				((FileSystemInfo)file).Delete();
			});
			val.GetFiles().ForEach(delegate(FileInfo file)
			{
				file.CopyTo(Path.Combine(((FileSystemInfo)targetDirectory).get_FullName(), ((FileSystemInfo)file).get_Name()));
			});
			saveFilePath = ((FileSystemInfo)targetDirectory).get_FullName();
		}

		public override bool Update(bool hasFocus)
		{
			if (m_saveBrowser != null)
			{
				if (hasFocus && m_saveBrowser.RowsCount != 0 && m_rowAutoSelect)
				{
					if (m_lastSelectedRow < m_saveBrowser.RowsCount)
					{
						m_saveBrowser.SelectedRow = m_saveBrowser.GetRow(m_lastSelectedRow);
						m_selectedRow = m_lastSelectedRow;
					}
					else
					{
						m_saveBrowser.SelectedRow = m_saveBrowser.GetRow(0);
						m_selectedRow = (m_lastSelectedRow = 0);
					}
					m_rowAutoSelect = false;
					m_saveBrowser.ScrollToSelection();
					LoadImagePreview();
				}
				m_saveBrowser.GetSave(m_saveBrowser.SelectedRow, out var info);
				if (!info.Valid || (m_saveBrowser.SelectedRowsIndexes != null && m_saveBrowser.SelectedRowsIndexes.Count > 1))
				{
					m_loadButton.Enabled = false;
					m_editButton.Enabled = false;
					m_saveButton.Enabled = false;
					m_publishButton.Enabled = false;
					m_backupsButton.Enabled = false;
				}
				else
				{
					m_loadButton.Enabled = true;
					m_editButton.Enabled = true;
					m_saveButton.Enabled = true;
					m_publishButton.Enabled = MyFakes.ENABLE_WORKSHOP_PUBLISH && !info.WorldInfo.IsCampaign;
					m_backupsButton.Enabled = true;
					m_isEditable = !info.WorldInfo.IsCampaign || ENABLE_SCENARIO_EDIT || MySandboxGame.Config.ExperimentalMode;
				}
				m_deleteButton.Enabled = m_saveBrowser.SelectedRows != null;
			}
<<<<<<< HEAD
			if (hasFocus)
=======
			m_saveBrowser.GetSave(m_saveBrowser.SelectedRow, out var info);
			if (info.Valid)
			{
				m_loadButton.Enabled = true;
				m_editButton.Enabled = true;
				m_saveButton.Enabled = true;
				m_publishButton.Enabled = MyFakes.ENABLE_WORKSHOP_PUBLISH && !info.WorldInfo.IsCampaign;
				m_backupsButton.Enabled = true;
				m_isEditable = !info.WorldInfo.IsCampaign || ENABLE_SCENARIO_EDIT || MySandboxGame.Config.ExperimentalMode;
			}
			else
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
				{
					OnDeleteClick(null);
				}
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.VIEW))
				{
					OnBackupsButtonClick(null);
				}
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MAIN_MENU))
				{
					OnPublishClick(null);
				}
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON))
				{
					OnSaveAsClick(null);
				}
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
				{
					OnEditClick(null);
				}
				SetButtonsVisibility(!MyInput.Static.IsJoystickLastUsed);
			}
<<<<<<< HEAD
=======
			m_deleteButton.Enabled = m_saveBrowser.SelectedRow != null;
			if (hasFocus)
			{
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_Y))
				{
					OnDeleteClick(null);
				}
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.VIEW))
				{
					OnBackupsButtonClick(null);
				}
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.MAIN_MENU))
				{
					OnPublishClick(null);
				}
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON))
				{
					OnSaveAsClick(null);
				}
				if (MyControllerHelper.IsControl(MySpaceBindingCreator.CX_GUI, MyControlsGUI.BUTTON_X))
				{
					OnEditClick(null);
				}
				SetButtonsVisibility(!MyInput.Static.IsJoystickLastUsed);
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return base.Update(hasFocus);
		}

		private void SetButtonsVisibility(bool visible)
		{
			m_loadButton.Visible = visible;
			m_deleteButton.Visible = visible;
			m_editButton.Visible = visible;
			m_backupsButton.Visible = visible;
			m_publishButton.Visible = visible;
			m_saveButton.Visible = visible;
		}
	}
}
