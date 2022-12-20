using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Screens.Helpers
{
	public static class MyAsyncSaving
	{
		private static Action m_callbackOnFinished;

		private static int m_inProgressCount;

		private static bool m_screenshotTaken;

		private static bool m_saveErrorIsShown;

		public static bool InProgress => m_inProgressCount > 0;

		private static void PushInProgress()
		{
			m_inProgressCount++;
		}

		private static void PopInProgress()
		{
			m_inProgressCount--;
		}

		public static void Start(Action callbackOnFinished = null, string customName = null)
		{
			PushInProgress();
			m_callbackOnFinished = callbackOnFinished;
			OnSnapshotDone(MySession.Static.Save(out var snapshot, customName), snapshot);
		}

		public static void DelayedSaveAfterLoad(string saveName)
		{
			MySession.Static.AddUpdateCallback(new MyUpdateCallback(delegate
			{
				MyGuiScreenGamePlay firstScreenOfType = MyScreenManager.GetFirstScreenOfType<MyGuiScreenGamePlay>();
				if (firstScreenOfType == null || firstScreenOfType.State != MyGuiScreenState.OPENED)
				{
					return false;
				}
				if (MyHud.ScreenEffects.IsBlackscreenFadeInProgress())
				{
					MyHudScreenEffects screenEffects = MyHud.ScreenEffects;
					screenEffects.OnBlackscreenFadeFinishedCallback = (Action)Delegate.Combine(screenEffects.OnBlackscreenFadeFinishedCallback, (Action)delegate
					{
						Start(null, saveName);
					});
				}
				else
				{
					Start(null, saveName);
				}
				return true;
			}));
		}

		private static void OnSnapshotDone(bool snapshotSuccess, MySessionSnapshot snapshot)
		{
			if (snapshotSuccess)
			{
				Func<bool> screenshotTaken = null;
				string text = null;
				if (!Sandbox.Engine.Platform.Game.IsDedicated && !MySandboxGame.Config.SyncRendering)
				{
					text = MySession.Static.ThumbPath;
					try
					{
						if (File.Exists(text))
						{
							File.Delete(text);
						}
						m_screenshotTaken = false;
						MySandboxGame.Static.OnScreenshotTaken += OnScreenshotTaken;
						MyRenderProxy.TakeScreenshot(new Vector2(0.5f, 0.5f), text, debug: false, ignoreSprites: true, showNotification: false);
						screenshotTaken = () => m_screenshotTaken;
					}
					catch (Exception ex)
					{
						MySandboxGame.Log.WriteLine("Could not take session thumb screenshot. Exception:");
						MySandboxGame.Log.WriteLine(ex);
					}
				}
				snapshot.SaveParallel(screenshotTaken, text, delegate
				{
					SaveFinished(snapshot);
				});
			}
			else
			{
<<<<<<< HEAD
				MyLog.Default.WriteLine("OnSnapshotDone: failed to save the world " + Environment.StackTrace);
=======
				MyLog.Default.WriteLine("OnSnapshotDone: failed to save the world " + Environment.get_StackTrace());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!Sandbox.Engine.Platform.Game.IsDedicated)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.WorldNotSaved), MySession.Static.Name), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
				}
				PopInProgress();
			}
			if (m_callbackOnFinished != null)
			{
				m_callbackOnFinished();
			}
			m_callbackOnFinished = null;
		}

		private static void OnScreenshotTaken(object sender, EventArgs e)
		{
			MySandboxGame.Static.OnScreenshotTaken -= OnScreenshotTaken;
			m_screenshotTaken = true;
		}

		private static void SaveFinished(MySessionSnapshot snapshot)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && MySession.Static != null)
			{
				if (snapshot.SavingSuccess)
				{
					IEnumerable<MyCharacter> savedCharacters = MySession.Static.SavedCharacters;
					if (savedCharacters != null)
					{
						foreach (MyCharacter item in savedCharacters)
						{
							if (item.Definition.UsableByPlayer)
							{
								MyLocalCache.SaveInventoryConfig(item);
							}
						}
					}
					MyHudNotification myHudNotification = new MyHudNotification(MyCommonTexts.WorldSaved);
					myHudNotification.SetTextFormatArguments(MySession.Static.Name);
					MyHud.Notifications.Add(myHudNotification);
				}
				else
				{
<<<<<<< HEAD
					MyLog.Default.WriteLine("SaveFinished: failed to save the world " + Environment.StackTrace);
=======
					MyLog.Default.WriteLine("SaveFinished: failed to save the world " + Environment.get_StackTrace());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (!m_saveErrorIsShown)
					{
						m_saveErrorIsShown = true;
						MyStringId id = ((!snapshot.TooLongPath) ? MyCloudHelper.GetErrorMessage(snapshot.CloudResult, MyCommonTexts.WorldNotSaved) : MyCloudHelper.GetErrorMessage(snapshot.CloudResult, MyCommonTexts.WorldNotSavedPathTooLong));
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, new StringBuilder().AppendFormat(MyTexts.GetString(id), MySession.Static.Name), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), null, null, null, null, null, 0, MyGuiScreenMessageBox.ResultEnum.YES, canHideOthers: true, null, useOpacity: true, null, focusable: true, canBeHidden: false, delegate
						{
							m_saveErrorIsShown = false;
						}));
					}
				}
			}
			PopInProgress();
		}
	}
}
