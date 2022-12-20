using System;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Sandbox.Definitions;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Utils;
using Sandbox.Game.Audio;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Multiplayer;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game;
using VRage.Game.ObjectBuilders;
using VRage.GameServices;
using VRage.Utils;

namespace Sandbox.Game.World
{
	/// <summary>
	/// This class is a Loading helper toolbox that holds syntax
	/// that was split all across the GUI screens.
	/// </summary>
	public static class MySessionLoader
	{
		public static MyWorkshopItem LastLoadedSessionWorkshopItem;
<<<<<<< HEAD

		public static event Action ScenarioWorldLoaded;

		/// <summary>
		/// Starts new session and unloads outdated if theres any.
		/// </summary>
		/// <param name="sessionName">Created session name.</param>
		/// <param name="settings">Session settings OB.</param>
		/// <param name="mods">Mod selection.</param>
		/// <param name="scenarioDefinition">World generator argument.</param>
		/// <param name="asteroidAmount">Hostility settings.</param>
		/// <param name="description">Session description.</param>
		/// <param name="passwd">Session password.</param>
=======

		public static event Action BattleWorldLoaded;

		public static event Action ScenarioWorldLoaded;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static void StartNewSession(string sessionName, MyObjectBuilder_SessionSettings settings, List<MyObjectBuilder_Checkpoint.ModItem> mods, MyScenarioDefinition scenarioDefinition = null, int asteroidAmount = 0, string description = "", string passwd = "")
		{
			MyLog.Default.WriteLine("StartNewSandbox - Start");
			if (!MyWorkshop.CheckLocalModsAllowed(mods, settings.OnlineMode == MyOnlineModeEnum.OFFLINE))
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextLocalModsDisabledInMultiplayer)));
				MyLog.Default.WriteLine("LoadSession() - End");
				return;
			}
			MyWorkshop.DownloadModsAsync(mods, delegate(bool success)
			{
				if (success || (settings.OnlineMode == MyOnlineModeEnum.OFFLINE && MyWorkshop.CanRunOffline(mods)))
				{
					MyScreenManager.RemoveAllScreensExcept(null);
					if (asteroidAmount < 0)
					{
						MyWorldGenerator.SetProceduralSettings(asteroidAmount, settings);
						asteroidAmount = 0;
					}
					MySpaceAnalytics.Instance.SetWorldEntry(MyWorldEntryEnum.Custom);
					StartLoading(delegate
					{
						MySpaceAnalytics.Instance.SetWorldEntry(MyWorldEntryEnum.Custom);
						MySession.Start(sessionName, description, passwd, settings, mods, new MyWorldGenerator.Args
						{
							AsteroidAmount = asteroidAmount,
							Scenario = scenarioDefinition
						});
					});
				}
				else if (MyGameService.IsOnline)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed)));
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.DialogTextDownloadModsFailedSteamOffline), MySession.GameServiceName))));
				}
				MyLog.Default.WriteLine("StartNewSandbox - End");
			});
		}

		public static void LoadLastSession()
		{
			string lastSessionPath = MyLocalCache.GetLastSessionPath();
			bool flag = false;
			if (lastSessionPath != null && MyPlatformGameSettings.GAME_SAVES_TO_CLOUD)
			{
				flag = !MyCloudHelper.ExtractFilesTo(MyCloudHelper.LocalToCloudWorldPath(lastSessionPath + "/"), lastSessionPath, unpack: false);
			}
			if (lastSessionPath == null || flag || !MyFileSystem.DirectoryExists(lastSessionPath))
			{
				MySandboxGame.Static.Invoke(delegate
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, MyTexts.Get(MyCommonTexts.MessageBoxLastSessionNotFound), MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
				}, "New Game screen");
			}
			else
			{
				LoadSingleplayerSession(lastSessionPath);
			}
		}

		public static void LoadMultiplayerSession(MyObjectBuilder_World world, MyMultiplayerBase multiplayerSession)
		{
			MyLog.Default.WriteLine("LoadSession() - Start");
			if (!MyWorkshop.CheckLocalModsAllowed(world.Checkpoint.Mods, allowLocalMods: false))
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextLocalModsDisabledInMultiplayer)));
				MyLog.Default.WriteLine("LoadSession() - End");
				return;
			}
			MyWorkshop.DownloadModsAsync(world.Checkpoint.Mods, delegate(bool success)
			{
				if (success)
				{
					MyScreenManager.CloseAllScreensNowExcept(null);
					MyGuiSandbox.Update(16);
					if (MySession.Static != null)
					{
						MySession.Static.Unload();
						MySession.Static = null;
					}
					StartLoading(delegate
					{
						MySession.LoadMultiplayer(world, multiplayerSession);
					});
				}
				else
				{
					multiplayerSession.Dispose();
					UnloadAndExitToMenu();
					if (MyGameService.IsOnline)
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed)));
					}
					else
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.DialogTextDownloadModsFailedSteamOffline), MySession.GameServiceName))));
					}
				}
				MyLog.Default.WriteLine("LoadSession() - End");
			}, delegate
			{
				multiplayerSession.Dispose();
				UnloadAndExitToMenu();
			});
		}

		public static bool LoadDedicatedSession(string sessionPath, MyWorkshop.CancelToken cancelToken, Action afterLoad = null)
		{
			ulong sizeInBytes;
			MyObjectBuilder_Checkpoint myObjectBuilder_Checkpoint = MyLocalCache.LoadCheckpoint(sessionPath, out sizeInBytes);
			if (!HasOnlyModsFromConsentedUGCs(myObjectBuilder_Checkpoint))
			{
				ShowUGCConsentNotAcceptedWarning(GetNonConsentedServiceNameInCheckpoint(myObjectBuilder_Checkpoint));
				MyLog.Default.WriteLineAndConsole("LoadCheckpoint failed. No UGC consent.");
				MySandboxGame.Static.Exit();
				return false;
			}
			if (MySession.IsCompatibleVersion(myObjectBuilder_Checkpoint))
			{
				if (MyWorkshop.DownloadWorldModsBlocking(myObjectBuilder_Checkpoint.Mods, cancelToken).Success)
				{
					MySpaceAnalytics.Instance.SetWorldEntry(MyWorldEntryEnum.Load);
					MySession.Load(sessionPath, myObjectBuilder_Checkpoint, sizeInBytes);
					afterLoad?.Invoke();
					MySession.Static.StartServer(MyMultiplayer.Static);
					return true;
				}
				MyLog.Default.WriteLineAndConsole("Unable to download mods");
				MySandboxGame.Static.Exit();
				return false;
			}
			MyLog.Default.WriteLineAndConsole(MyTexts.Get(MyCommonTexts.DialogTextIncompatibleWorldVersion).ToString());
			MySandboxGame.Static.Exit();
			return false;
		}

		public static void LoadMultiplayerScenarioWorld(MyObjectBuilder_World world, MyMultiplayerBase multiplayerSession)
		{
			MyLog.Default.WriteLine("LoadMultiplayerScenarioWorld() - Start");
			if (!MyWorkshop.CheckLocalModsAllowed(world.Checkpoint.Mods, allowLocalMods: false))
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextLocalModsDisabledInMultiplayer), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate
				{
					UnloadAndExitToMenu();
				}));
				MyLog.Default.WriteLine("LoadMultiplayerScenarioWorld() - End");
				return;
			}
			MyWorkshop.DownloadModsAsync(world.Checkpoint.Mods, delegate(bool success)
			{
				if (success)
				{
					MyScreenManager.CloseAllScreensNowExcept(null);
					MyGuiSandbox.Update(16);
					StartLoading(delegate
					{
						MySession.Static.LoadMultiplayerWorld(world, multiplayerSession);
						if (MySessionLoader.ScenarioWorldLoaded != null)
						{
							MySessionLoader.ScenarioWorldLoaded();
						}
					});
				}
				else
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate
					{
						MySandboxGame.Static.Invoke(UnloadAndExitToMenu, "UnloadAndExitToMenu");
					}));
				}
				MyLog.Default.WriteLine("LoadMultiplayerScenarioWorld() - End");
			}, delegate
			{
				UnloadAndExitToMenu();
			});
		}

		private static void CheckDx11AndLoad(MyObjectBuilder_Checkpoint checkpoint, string sessionPath, ulong checkpointSizeInBytes, Action afterLoad = null)
		{
			LoadSingleplayerSession(checkpoint, sessionPath, checkpointSizeInBytes, afterLoad);
		}

		public static void LoadSingleplayerSession(string sessionDirectory, Action afterLoad = null, string contextName = null, MyOnlineModeEnum? onlineMode = null, int maxPlayers = 0, string forceSessionName = null)
		{
			MyLog.Default.WriteLine("LoadSession() - Start");
			MyLog.Default.WriteLine(sessionDirectory);
			ulong sizeInBytes = 0uL;
			MyLocalCache.LoadWorldConfiguration(sessionDirectory, out sizeInBytes);
			ulong checkpointSizeInBytes;
			MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(sessionDirectory, out checkpointSizeInBytes, null, onlineMode);
			if (checkpoint == null)
			{
				MyLog.Default.WriteLine(MyTexts.Get(MyCommonTexts.WorldFileIsCorruptedAndCouldNotBeLoaded).ToString());
				MySandboxGame.Static.Invoke(delegate
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.WorldFileIsCorruptedAndCouldNotBeLoaded)));
				}, "New Game screen");
				MyLog.Default.WriteLine("LoadSession() - End");
				return;
			}
			if (forceSessionName != null)
			{
				checkpoint.SessionName = forceSessionName;
			}
			checkpoint.CustomLoadingScreenText = MyStatControlText.SubstituteTexts(checkpoint.CustomLoadingScreenText, contextName);
			if (onlineMode.HasValue)
			{
				checkpoint.MaxPlayers = (short)maxPlayers;
<<<<<<< HEAD
			}
			if (HasOnlyModsFromConsentedUGCs(checkpoint))
			{
				if (checkpoint.OnlineMode != 0)
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
									Run();
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
				else
				{
					Run();
				}
			}
			else
			{
				ShowUGCConsentNotAcceptedWarning(GetNonConsentedServiceNameInCheckpoint(checkpoint));
			}
			void Run()
			{
				CheckDx11AndLoad(checkpoint, sessionDirectory, checkpointSizeInBytes, afterLoad);
			}
		}

		public static bool HasOnlyModsFromConsentedUGCs(MyObjectBuilder_Checkpoint checkpoint)
		{
			bool result = true;
			if (checkpoint != null && !Sync.IsDedicated)
			{
				foreach (MyObjectBuilder_Checkpoint.ModItem mod in checkpoint.Mods)
				{
					IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(mod.PublishedServiceName);
					if (aggregate != null && aggregate.ServiceName == mod.PublishedServiceName && !aggregate.IsConsentGiven)
					{
						result = false;
					}
				}
				return result;
			}
			return result;
		}

		public static string GetNonConsentedServiceNameInCheckpoint(MyObjectBuilder_Checkpoint checkpoint)
		{
			string result = "";
			if (checkpoint != null && !Sync.IsDedicated)
			{
				foreach (MyObjectBuilder_Checkpoint.ModItem mod in checkpoint.Mods)
				{
					IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(mod.PublishedServiceName);
					if (aggregate != null && aggregate.ServiceName == mod.PublishedServiceName && !aggregate.IsConsentGiven)
					{
						result = aggregate.ServiceName;
					}
				}
				return result;
			}
=======
			}
			if (HasOnlyModsFromConsentedUGCs(checkpoint))
			{
				if (checkpoint.OnlineMode != 0)
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
									Run();
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
				else
				{
					Run();
				}
			}
			else
			{
				ShowUGCConsentNotAcceptedWarning(GetNonConsentedServiceNameInCheckpoint(checkpoint));
			}
			void Run()
			{
				CheckDx11AndLoad(checkpoint, sessionDirectory, checkpointSizeInBytes, afterLoad);
			}
		}

		public static bool HasOnlyModsFromConsentedUGCs(MyObjectBuilder_Checkpoint checkpoint)
		{
			bool result = true;
			if (checkpoint != null && !Sync.IsDedicated)
			{
				foreach (MyObjectBuilder_Checkpoint.ModItem mod in checkpoint.Mods)
				{
					IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(mod.PublishedServiceName);
					if (aggregate != null && aggregate.ServiceName == mod.PublishedServiceName && !aggregate.IsConsentGiven)
					{
						result = false;
					}
				}
				return result;
			}
			return result;
		}

		public static string GetNonConsentedServiceNameInCheckpoint(MyObjectBuilder_Checkpoint checkpoint)
		{
			string result = "";
			if (checkpoint != null && !Sync.IsDedicated)
			{
				foreach (MyObjectBuilder_Checkpoint.ModItem mod in checkpoint.Mods)
				{
					IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(mod.PublishedServiceName);
					if (aggregate != null && aggregate.ServiceName == mod.PublishedServiceName && !aggregate.IsConsentGiven)
					{
						result = aggregate.ServiceName;
					}
				}
				return result;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return result;
		}

		public static void ShowUGCConsentNotAcceptedWarning(string serviceName)
		{
			StringBuilder msgText = new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.WorldFileIsContainsModsFromNotConsentedUGCAndCouldNotBeLoaded), serviceName);
			MyLog.Default.WriteLine(msgText.ToString());
			MySandboxGame.Static.Invoke(delegate
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: msgText));
			}, "New Game screen");
			MyLog.Default.WriteLine("LoadSession() - End");
		}

		public static void ShowUGCConsentNeededForThisServiceWarning()
		{
			MyLog.Default.WriteLine(new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.NotAbleToUseBecauseNotConsentedToUGC)).ToString());
			MySandboxGame.Static.Invoke(delegate
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: new StringBuilder().AppendFormat(MyTexts.GetString(MyCommonTexts.NotAbleToUseBecauseNotConsentedToUGC))));
			}, "New Game screen");
			MyLog.Default.WriteLine("LoadSession() - End");
		}

		private static string GetCustomLoadingScreenImagePath(string relativePath)
		{
			if (string.IsNullOrEmpty(relativePath))
			{
				return null;
			}
			string text = Path.Combine(MyFileSystem.SavesPath, relativePath);
			if (!MyFileSystem.FileExists(text))
			{
				text = Path.Combine(MyFileSystem.ContentPath, relativePath);
			}
			if (!MyFileSystem.FileExists(text))
			{
				text = Path.Combine(MyFileSystem.ModsPath, relativePath);
			}
			if (!MyFileSystem.FileExists(text))
			{
				text = null;
			}
			return text;
		}

		public static void LoadSingleplayerSession(MyObjectBuilder_Checkpoint checkpoint, string sessionPath, ulong checkpointSizeInBytes, Action afterLoad = null)
		{
			if (!MySession.IsCompatibleVersion(checkpoint))
			{
				MyLog.Default.WriteLine(MyTexts.Get(MyCommonTexts.DialogTextIncompatibleWorldVersionWarning).ToString());
				MySandboxGame.Static.Invoke(delegate
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.YES_NO, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextIncompatibleWorldVersionWarning), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate(MyGuiScreenMessageBox.ResultEnum result)
					{
						if (result == MyGuiScreenMessageBox.ResultEnum.YES)
						{
							LoadSingleplayerSessionInternal(checkpoint, sessionPath, checkpointSizeInBytes, afterLoad);
						}
						else
						{
<<<<<<< HEAD
							if (!MyScreenManager.IsAnyScreenOpening() && !MyScreenManager.IsScreenOfTypeOpen(MyPerGameSettings.GUI.MainMenu))
							{
								MyGuiSandbox.BackToMainMenu();
							}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							MyLog.Default.WriteLine("LoadSession() - Cancelled");
						}
					}));
				}, "LoadSingleplayerSession failed");
			}
			else
			{
				LoadSingleplayerSessionInternal(checkpoint, sessionPath, checkpointSizeInBytes, afterLoad);
			}
		}

		private static void LoadSingleplayerSessionInternal(MyObjectBuilder_Checkpoint checkpoint, string sessionPath, ulong checkpointSizeInBytes, Action afterLoad = null)
		{
			bool flag = false;
<<<<<<< HEAD
			MyObjectBuilder_CampaignSessionComponent myObjectBuilder_CampaignSessionComponent = checkpoint.SessionComponents.OfType<MyObjectBuilder_CampaignSessionComponent>().FirstOrDefault();
=======
			MyObjectBuilder_CampaignSessionComponent myObjectBuilder_CampaignSessionComponent = Enumerable.FirstOrDefault<MyObjectBuilder_CampaignSessionComponent>(Enumerable.OfType<MyObjectBuilder_CampaignSessionComponent>((IEnumerable)checkpoint.SessionComponents));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (myObjectBuilder_CampaignSessionComponent != null)
			{
				flag |= MyCampaignManager.Static != null && MyCampaignManager.Static.IsCampaign(myObjectBuilder_CampaignSessionComponent);
			}
			bool experimentalMode = checkpoint.Settings.ExperimentalMode;
			MyObjectBuilder_SessionSettings.ExperimentalReason experimentalReason = checkpoint.Settings.GetExperimentalReason(remote: false);
			MyLog.Default.WriteLineAndConsole("CheckPoint Experimental mode: " + (experimentalMode ? "Yes" : "No"));
			MyLog.Default.WriteLineAndConsole("CheckPoint Experimental mode reason: " + experimentalReason);
			if (!flag && (experimentalReason != (MyObjectBuilder_SessionSettings.ExperimentalReason)0L || experimentalMode) && !MySandboxGame.Config.ExperimentalMode)
			{
				ShowLoadingError(MyCommonTexts.SaveGameErrorExperimental);
				return;
			}
			if (!MyWorkshop.CheckLocalModsAllowed(checkpoint.Mods, checkpoint.Settings.OnlineMode == MyOnlineModeEnum.OFFLINE))
			{
				MySandboxGame.Static.Invoke(delegate
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextLocalModsDisabledInMultiplayer)));
				}, "New Game screen");
				MyLog.Default.WriteLine("LoadSession() - End");
				return;
			}
			MyWorkshop.DownloadModsAsync(checkpoint.Mods, delegate(bool success)
			{
				MySandboxGame.Static.Invoke(delegate
				{
					DownloadModsDone(success);
				}, "MySessionLoader::DownloadModsDone");
				MyLog.Default.WriteLine("LoadSession() - End");
			}, UnloadAndExitToMenu);
			void DownloadModsDone(bool success)
			{
				if (success || (checkpoint.Settings.OnlineMode == MyOnlineModeEnum.OFFLINE && MyWorkshop.CanRunOffline(checkpoint.Mods)))
				{
					MyScreenManager.CloseAllScreensNowExcept(null);
					MyGuiSandbox.Update(16);
					string customLoadingScreenPath = GetCustomLoadingScreenImagePath(checkpoint.CustomLoadingScreenImage);
					StartLoading(delegate
					{
						if (MySession.Static != null)
						{
							MySession.Static.Unload();
							MySession.Static = null;
						}
						MySpaceAnalytics.Instance.SetWorldEntry(MyWorldEntryEnum.Load);
						MySession.Load(sessionPath, checkpoint, checkpointSizeInBytes, saveLastStates: true, allowXml: false);
						if (afterLoad != null)
						{
							afterLoad();
						}
					}, delegate
					{
						StartLoading(delegate
						{
							if (MySession.Static != null)
							{
								MySession.Static.Unload();
								MySession.Static = null;
							}
							MySpaceAnalytics.Instance.SetWorldEntry(MyWorldEntryEnum.Load);
							MySession.Load(sessionPath, checkpoint, checkpointSizeInBytes);
							if (afterLoad != null)
							{
								afterLoad();
							}
						}, null, customLoadingScreenPath, checkpoint.CustomLoadingScreenText);
					}, customLoadingScreenPath, checkpoint.CustomLoadingScreenText);
				}
				else
				{
					MyLog.Default.WriteLine(MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed).ToString());
					if (MyGameService.IsOnline)
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.DialogTextDownloadModsFailed), okButtonText: null, cancelButtonText: null, yesButtonText: null, noButtonText: null, callback: delegate
						{
							UnloadAndExitToMenu();
						}));
					}
					else
					{
						MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: new StringBuilder(string.Format(MyTexts.GetString(MyCommonTexts.DialogTextDownloadModsFailedSteamOffline), MySession.GameServiceName))));
					}
				}
			}
<<<<<<< HEAD
			void ShowLoadingError(MyStringId errorMessage)
=======
			static void ShowLoadingError(MyStringId errorMessage)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyLog.Default.WriteLine(MyTexts.GetString(errorMessage));
				MySandboxGame.Static.Invoke(delegate
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(errorMessage)));
				}, "SessionLoadingError");
			}
		}

		public static void StartLoading(Action loadingAction, Action loadingActionXMLAllowed = null, string customLoadingBackground = null, string customLoadingtext = null)
		{
			if (MySpaceAnalytics.Instance != null)
			{
				MySpaceAnalytics.Instance.StoreWorldLoadingStartTime();
			}
			MyGuiScreenGamePlay newGameplayScreen = new MyGuiScreenGamePlay();
			MyGuiScreenGamePlay myGuiScreenGamePlay = newGameplayScreen;
			myGuiScreenGamePlay.OnLoadingAction = (Action)Delegate.Combine(myGuiScreenGamePlay.OnLoadingAction, loadingAction);
			MyGuiScreenLoading myGuiScreenLoading = new MyGuiScreenLoading(newGameplayScreen, MyGuiScreenGamePlay.Static, customLoadingBackground, customLoadingtext);
			myGuiScreenLoading.OnScreenLoadingFinished += delegate
			{
				if (MySession.Static != null)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.HUDScreen));
					newGameplayScreen.LoadingDone = true;
				}
			};
			myGuiScreenLoading.OnLoadingXMLAllowed = loadingActionXMLAllowed;
			MyGuiSandbox.AddScreen(myGuiScreenLoading);
		}

		public static void Unload()
		{
			try
			{
				try
				{
					if (MySession.Static != null)
					{
						MySession.Static.Unload();
						return;
					}
					MyScreenManager.CloseAllScreensNowExcept(null, isUnloading: true);
					MyGuiSandbox.Update(16);
				}
				finally
				{
					MySession.Static = null;
					try
					{
						if (MyMusicController.Static != null)
						{
							MyMusicController.Static.Unload();
							MyAudio.Static.MusicAllowed = true;
							MyAudio.Static.Mute = false;
						}
					}
					finally
					{
						MyMusicController.Static = null;
						if (MyMultiplayer.Static != null)
						{
							MyMultiplayer.Static.Dispose();
						}
					}
				}
			}
			catch (Exception ex)
			{
				MySession.Static = null;
				MySandboxGame.Log.WriteLine("ERROR: Failed to cleanly unload session:");
<<<<<<< HEAD
				MySandboxGame.Log.WriteLine(Environment.StackTrace);
=======
				MySandboxGame.Log.WriteLine(Environment.get_StackTrace());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MySandboxGame.Log.WriteLine(ex);
			}
		}

		public static void UnloadAndExitToMenu()
		{
			Unload();
			MyGuiSandbox.AddScreen(MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.MainMenu));
		}

		public static void LoadInventoryScene()
		{
			if (MyGameService.IsActive && MyFakes.ENABLE_MAIN_MENU_INVENTORY_SCENE)
			{
				string text = Path.Combine(MyFileSystem.ContentPath, "InventoryScenes\\Inventory-9");
				DictionaryValuesReader<MyDefinitionId, MyMainMenuInventorySceneDefinition> mainMenuInventoryScenes = MyDefinitionManager.Static.GetMainMenuInventoryScenes();
				if (mainMenuInventoryScenes.Count > 0)
				{
<<<<<<< HEAD
					List<MyMainMenuInventorySceneDefinition> list = mainMenuInventoryScenes.ToList();
=======
					List<MyMainMenuInventorySceneDefinition> list = Enumerable.ToList<MyMainMenuInventorySceneDefinition>((IEnumerable<MyMainMenuInventorySceneDefinition>)mainMenuInventoryScenes);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					int randomInt = MyUtils.GetRandomInt(list.Count);
					text = Path.Combine(MyFileSystem.ContentPath, list[randomInt].SceneDirectory);
				}
				ulong sizeInBytes;
				MyObjectBuilder_Checkpoint checkpoint = MyLocalCache.LoadCheckpoint(text, out sizeInBytes);
				MySession.Load(text, checkpoint, sizeInBytes, saveLastStates: false);
			}
		}

		public static void ExitGame()
		{
			if (MySpaceAnalytics.Instance != null)
			{
				MySpaceAnalytics.Instance.ReportSessionEnd("Exit to Windows");
			}
			MyScreenManager.CloseAllScreensNowExcept(null);
			MySandboxGame.ExitThreadSafe();
		}
	}
}
