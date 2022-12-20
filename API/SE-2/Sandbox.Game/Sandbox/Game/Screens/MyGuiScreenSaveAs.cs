using System;
using System.Collections.Generic;
using System.IO;
using ParallelTasks;
using Sandbox.Engine.Networking;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.GameServices;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenSaveAs : MyGuiScreenBase
	{
		private class SaveResult : IMyAsyncResult
		{
			public bool IsCompleted => Task.IsComplete;

			public CloudResult Result { get; private set; }

			public Task Task { get; private set; }

			public SaveResult(string saveDir, string sessionPath, MyWorldInfo copyFrom, bool isCloud)
			{
				SaveResult saveResult = this;
				Task = Parallel.Start(delegate
				{
					saveResult.Result = saveResult.SaveAsync(saveDir, sessionPath, copyFrom, isCloud);
				});
			}

			private CloudResult SaveAsync(string newSaveName, string sessionPath, MyWorldInfo copyFrom, bool isCloud)
			{
				string sessionSavesPath = MyLocalCache.GetSessionSavesPath(newSaveName, contentFolder: false, createIfNotExists: false, isCloud);
				if (isCloud)
				{
					while (true)
					{
						List<MyCloudFileInfo> cloudFiles = MyGameService.GetCloudFiles(sessionSavesPath);
						if (cloudFiles == null || cloudFiles.Count == 0)
						{
							break;
						}
						sessionSavesPath = MyLocalCache.GetSessionSavesPath(newSaveName + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), contentFolder: false, createIfNotExists: false, isCloud);
					}
					CloudResult cloudResult = MyCloudHelper.CopyFiles(sessionPath, sessionSavesPath);
					if (cloudResult == CloudResult.Ok)
					{
						ulong sizeInBytes;
						MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = MyLocalCache.LoadCheckpointFromCloud(sessionSavesPath, out sizeInBytes);
						myObjectBuilder_Checkpoint.SessionName = copyFrom.SessionName;
						myObjectBuilder_Checkpoint.WorkshopId = null;
						cloudResult = MyLocalCache.SaveCheckpointToCloud(myObjectBuilder_Checkpoint, sessionSavesPath);
					}
					return cloudResult;
				}
				try
				{
					while (Directory.Exists(sessionSavesPath))
					{
						sessionSavesPath = MyLocalCache.GetSessionSavesPath(newSaveName + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), contentFolder: false, createIfNotExists: false);
					}
					Directory.CreateDirectory(sessionSavesPath);
					MyUtils.CopyDirectory(sessionPath, sessionSavesPath);
					ulong sizeInBytes2;
					MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint2 = MyLocalCache.LoadCheckpoint(sessionSavesPath, out sizeInBytes2);
					myObjectBuilder_Checkpoint2.SessionName = copyFrom.SessionName;
					myObjectBuilder_Checkpoint2.WorkshopId = null;
					MyLocalCache.SaveCheckpoint(myObjectBuilder_Checkpoint2, sessionSavesPath);
				}
				catch (Exception)
				{
					return CloudResult.Failed;
				}
				return CloudResult.Ok;
			}
		}

		private MyGuiControlTextbox m_nameTextbox;

		private MyGuiControlButton m_okButton;

		private MyGuiControlButton m_cancelButton;

		private MyWorldInfo m_copyFrom;

		private List<string> m_existingSessionNames;

		private string m_sessionPath;

		private bool m_fromMainMenu;

		private bool m_isCloud;

		public event Action<CloudResult> SaveAsConfirm;

		public MyGuiScreenSaveAs(MyWorldInfo copyFrom, string sessionPath, List<string> existingSessionNames, bool isCloud)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(87f / 175f, 147f / 524f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			AddCaption(MyCommonTexts.ScreenCaptionSaveAs, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.78f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.78f / 2f, (0f - m_size.Value.Y) / 2f + 0.123f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList2);
			float y = -0.027f;
			m_nameTextbox = new MyGuiControlTextbox(new Vector2(0f, y), copyFrom.SessionName, 90);
			m_nameTextbox.Size = new Vector2(0.385f, 1f);
			m_okButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnOkButtonClick);
			m_cancelButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnCancelButtonClick);
			Vector2 vector = new Vector2(0.002f, m_size.Value.Y / 2f - 0.071f);
			Vector2 vector2 = new Vector2(0.018f, 0f);
			m_okButton.Position = vector - vector2;
			m_cancelButton.Position = vector + vector2;
			m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok));
			m_cancelButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
			m_nameTextbox.SetToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ToolTipWorldSettingsName), 5, 90));
			Controls.Add(m_nameTextbox);
			Controls.Add(m_okButton);
			Controls.Add(m_cancelButton);
			m_nameTextbox.MoveCarriageToEnd();
			m_copyFrom = copyFrom;
			m_sessionPath = sessionPath;
			m_existingSessionNames = existingSessionNames;
			base.CloseButtonEnabled = true;
<<<<<<< HEAD
			m_onEnterCallback = OnEnterPressed;
=======
			OnEnterCallback = OnEnterPressed;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_isCloud = isCloud;
			CreateGamepadHelp();
		}

		private void CreateGamepadHelp()
		{
			Vector2 minSizeGui = MyGuiControlButton.GetVisualStyle(MyGuiControlButtonStyleEnum.Default).NormalTexture.MinSizeGui;
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(m_okButton.Position.X - minSizeGui.X / 2f, m_okButton.Position.Y - minSizeGui.Y / 2f));
			myGuiControlLabel.Name = MyGuiScreenBase.GAMEPAD_HELP_LABEL_NAME;
			Controls.Add(myGuiControlLabel);
			base.GamepadHelpTextId = MySpaceTexts.SaveAs_Help_Screen;
		}

		public MyGuiScreenSaveAs(string sessionName)
			: base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(87f / 175f, 147f / 524f), isTopMostScreen: false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
			base.EnabledBackgroundFade = true;
			AddCaption(MyCommonTexts.ScreenCaptionSaveAs, null, new Vector2(0f, 0.003f));
			MyGuiControlSeparatorList myGuiControlSeparatorList = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.78f / 2f, m_size.Value.Y / 2f - 0.075f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList);
			MyGuiControlSeparatorList myGuiControlSeparatorList2 = new MyGuiControlSeparatorList();
			myGuiControlSeparatorList2.AddHorizontal(new Vector2(0f, 0f) - new Vector2(m_size.Value.X * 0.78f / 2f, (0f - m_size.Value.Y) / 2f + 0.122f), m_size.Value.X * 0.78f);
			Controls.Add(myGuiControlSeparatorList2);
			m_existingSessionNames = null;
			m_fromMainMenu = true;
			float y = -0.027f;
			m_nameTextbox = new MyGuiControlTextbox(new Vector2(0f, y), sessionName, 90);
			m_nameTextbox.Size = new Vector2(0.385f, 1f);
			m_okButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Ok), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnOkButtonClick);
			m_cancelButton = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM, null, MyTexts.Get(MyCommonTexts.Cancel), 0.8f, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, MyGuiControlHighlightType.WHEN_CURSOR_OVER, OnCancelButtonClick);
			Vector2 vector = new Vector2(0.002f, m_size.Value.Y / 2f - 0.045f);
			Vector2 vector2 = new Vector2(0.018f, 0f);
			m_okButton.Position = vector - vector2;
			m_cancelButton.Position = vector + vector2;
			m_okButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipNewsletter_Ok));
			m_cancelButton.SetToolTip(MyTexts.GetString(MySpaceTexts.ToolTipOptionsSpace_Cancel));
			m_nameTextbox.SetToolTip(string.Format(MyTexts.GetString(MyCommonTexts.ToolTipWorldSettingsName), 5, 90));
			Controls.Add(m_nameTextbox);
			Controls.Add(m_okButton);
			Controls.Add(m_cancelButton);
			m_nameTextbox.MoveCarriageToEnd();
			base.CloseButtonEnabled = true;
<<<<<<< HEAD
			m_onEnterCallback = OnEnterPressed;
=======
			OnEnterCallback = OnEnterPressed;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			CreateGamepadHelp();
		}

		public override void HandleUnhandledInput(bool receivedFocusInThisUpdate)
		{
			base.HandleUnhandledInput(receivedFocusInThisUpdate);
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_X))
			{
				TrySaveAs();
			}
		}

		public override bool Update(bool hasFocus)
		{
			bool result = base.Update(hasFocus);
			if (hasFocus)
			{
				m_okButton.Visible = !MyInput.Static.IsJoystickLastUsed;
				m_cancelButton.Visible = !MyInput.Static.IsJoystickLastUsed;
			}
			return result;
		}

		private void OnEnterPressed()
		{
			TrySaveAs();
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenSaveAs";
		}

		private void OnCancelButtonClick(MyGuiControlButton sender)
		{
			CloseScreen();
		}

		private void OnOkButtonClick(MyGuiControlButton sender)
		{
			TrySaveAs();
		}

		private bool TrySaveAs()
		{
			MyStringId? myStringId = null;
			if (m_nameTextbox.Text.ToString().Replace(':', '-').IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
			{
				myStringId = MyCommonTexts.ErrorNameInvalid;
			}
			else if (m_nameTextbox.Text.Length < 5)
			{
				myStringId = MyCommonTexts.ErrorNameTooShort;
			}
			else if (m_nameTextbox.Text.Length > 128)
			{
				myStringId = MyCommonTexts.ErrorNameTooLong;
			}
			if (m_existingSessionNames != null)
			{
				foreach (string existingSessionName in m_existingSessionNames)
				{
					if (existingSessionName == m_nameTextbox.Text)
					{
						myStringId = MyCommonTexts.ErrorNameAlreadyExists;
					}
				}
			}
			if (myStringId.HasValue)
			{
				MyGuiScreenMessageBox myGuiScreenMessageBox = MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(myStringId.Value), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError));
				myGuiScreenMessageBox.SkipTransition = true;
				myGuiScreenMessageBox.InstantClose = false;
				MyGuiSandbox.AddScreen(myGuiScreenMessageBox);
				return false;
			}
			if (m_fromMainMenu)
			{
				string text = MyUtils.StripInvalidChars(m_nameTextbox.Text);
				if (string.IsNullOrWhiteSpace(text))
				{
					text = MyLocalCache.GetSessionSavesPath(text + MyUtils.GetRandomInt(int.MaxValue).ToString("########"), contentFolder: false, createIfNotExists: false);
				}
				MyAsyncSaving.Start(null, text);
				MySession.Static.Name = m_nameTextbox.Text;
				CloseScreen();
				return true;
			}
			m_copyFrom.SessionName = m_nameTextbox.Text;
			MyGuiSandbox.AddScreen(new MyGuiScreenProgressAsync(MyCommonTexts.SavingPleaseWait, null, () => new SaveResult(MyUtils.StripInvalidChars(m_nameTextbox.Text), m_sessionPath, m_copyFrom, m_isCloud), delegate(IMyAsyncResult result, MyGuiScreenProgressAsync screen)
			{
				screen.CloseScreen();
				CloseScreen();
				this.SaveAsConfirm.InvokeIfNotNull(((SaveResult)result).Result);
			}));
			return true;
		}
	}
}
