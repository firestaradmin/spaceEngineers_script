using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Sandbox.Engine.Analytics;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Networking;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Game.ObjectBuilders;
using VRage.GameServices;
using VRage.Network;
using VRage.Profiler;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
	public static class MyJoinGameHelper
	{
		private static MyGuiScreenProgress m_progress;

		private static bool JoinGameTest(IMyLobby lobby)
		{
			if (!lobby.IsValid)
			{
				return false;
			}
			if (!MyMultiplayerLobby.IsLobbyCorrectVersion(lobby))
			{
				string @string = MyTexts.GetString(MyCommonTexts.MultiplayerError_IncorrectVersion);
				string arg = MyBuildNumbers.ConvertBuildNumberFromIntToString(MyFinalBuildConstants.APP_VERSION);
				int lobbyAppVersion = MyMultiplayerLobby.GetLobbyAppVersion(lobby);
				if (lobbyAppVersion != 0)
				{
					string arg2 = MyBuildNumbers.ConvertBuildNumberFromIntToString(lobbyAppVersion);
					MyGuiSandbox.Show(new StringBuilder(string.Format(@string, arg, arg2)));
				}
				return false;
			}
			if (MyFakes.ENABLE_MP_DATA_HASHES && !MyMultiplayerLobby.HasSameData(lobby))
			{
				MyGuiSandbox.Show(MyCommonTexts.MultiplayerError_DifferentData);
				MySandboxGame.Log.WriteLine("Different game data when connecting to server. Local hash: " + MyDataIntegrityChecker.GetHashBase64() + ", server hash: " + MyMultiplayerLobby.GetDataHash(lobby));
				return false;
			}
			return true;
		}

		public static void JoinGame(IMyLobby lobby, bool requestData = true)
		{
			if (MySession.Static != null)
			{
				MySession.Static.Unload();
				MySession.Static = null;
			}
			if (requestData && string.IsNullOrEmpty(lobby.GetData("appVersion")))
			{
				MyLobbyHelper myLobbyHelper = new MyLobbyHelper(lobby);
				myLobbyHelper.OnSuccess += delegate(IMyLobby l, bool isSuccess)
				{
					if (!isSuccess)
					{
						JoinGame(lobby.LobbyId);
					}
					JoinGame(l, requestData: false);
				};
				if (myLobbyHelper.RequestData())
				{
					return;
				}
			}
			if (JoinGameTest(lobby))
			{
				JoinGame(lobby.LobbyId);
			}
		}

		public static void JoinGame(MyGameServerItem server, Dictionary<string, string> rules, bool enableGuiBackgroundFade = true, Action failedToJoin = null)
		{
			if (MySession.Static != null)
			{
				MySession.Static.Unload();
				MySession.Static = null;
			}
			MySpaceAnalytics.Instance.SetWorldEntry(MyWorldEntryEnum.Join);
			if (server.ServerVersion != (int)MyFinalBuildConstants.APP_VERSION)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat(MyTexts.GetString(MyCommonTexts.MultiplayerError_IncorrectVersion), MyFinalBuildConstants.APP_VERSION, server.ServerVersion);
				MyGuiSandbox.Show(stringBuilder, MyCommonTexts.MessageBoxCaptionError);
				failedToJoin?.Invoke();
				return;
			}
			if (MyFakes.ENABLE_MP_DATA_HASHES)
			{
				string gameTagByPrefix = server.GetGameTagByPrefix("datahash");
				if (gameTagByPrefix != "" && gameTagByPrefix != MyDataIntegrityChecker.GetHashBase64())
				{
					MyGuiSandbox.Show(MyCommonTexts.MultiplayerError_DifferentData);
					MySandboxGame.Log.WriteLine("Different game data when connecting to server. Local hash: " + MyDataIntegrityChecker.GetHashBase64() + ", server hash: " + gameTagByPrefix);
					failedToJoin?.Invoke();
					return;
				}
			}
			MyCachedServerItem myCachedServerItem = new MyCachedServerItem(server);
			myCachedServerItem.Rules = rules;
			if (rules != null)
			{
				myCachedServerItem.DeserializeSettings();
				if (!string.IsNullOrEmpty(GetNonConsentedServiceNameInMyCachedServerItem(myCachedServerItem)))
<<<<<<< HEAD
				{
					MySessionLoader.ShowUGCConsentNotAcceptedWarning(GetNonConsentedServiceNameInMyCachedServerItem(myCachedServerItem));
					foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
					{
						if (screen is MyGuiScreenProgress)
						{
							screen.CloseScreenNow();
=======
				{
					MySessionLoader.ShowUGCConsentNotAcceptedWarning(GetNonConsentedServiceNameInMyCachedServerItem(myCachedServerItem));
					foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
					{
						if (screen is MyGuiScreenProgress)
						{
							screen.CloseScreenNow();
						}
					}
					failedToJoin?.Invoke();
					return;
				}
				MyGameService.AddHistoryGame(server);
				MyMultiplayerClient multiplayer = new MyMultiplayerClient(server, new MySyncLayer(new MyTransportLayer(2)));
				if (!string.IsNullOrEmpty(GetNonConsentedServiceNameInMyMultiplayerClient(multiplayer)))
				{
					MySessionLoader.ShowUGCConsentNotAcceptedWarning(GetNonConsentedServiceNameInMyMultiplayerClient(multiplayer));
					foreach (MyGuiScreenBase screen2 in MyScreenManager.Screens)
					{
						if (screen2 is MyGuiScreenProgress)
						{
							screen2.CloseScreenNow();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
					failedToJoin?.Invoke();
					return;
				}
<<<<<<< HEAD
				MyGameService.AddHistoryGame(server);
				MyMultiplayerClient multiplayer = new MyMultiplayerClient(server, new MySyncLayer(new MyTransportLayer(2)));
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				multiplayer.ExperimentalMode = MySandboxGame.Config.ExperimentalMode;
				MyMultiplayer.Static = multiplayer;
				StringBuilder text = MyTexts.Get(MyCommonTexts.DialogTextJoiningWorld);
				MyGuiScreenProgress progress = new MyGuiScreenProgress(text, MyCommonTexts.Cancel, isTopMostScreen: false, enableGuiBackgroundFade);
				MyGuiSandbox.AddScreen(progress);
				progress.ProgressCancelled += delegate
				{
					multiplayer.Dispose();
					MySessionLoader.UnloadAndExitToMenu();
					if (MyMultiplayer.Static != null)
					{
						MyMultiplayer.Static.Dispose();
					}
				};
				MyMultiplayerClient myMultiplayerClient = multiplayer;
				myMultiplayerClient.OnJoin = (Action)Delegate.Combine(myMultiplayerClient.OnJoin, (Action)delegate
				{
					OnJoin(progress, success: true, null, MyLobbyStatusCode.Success, multiplayer);
				});
				Action<string> onProfilerCommandExecuted = delegate(string desc)
				{
					MyHudNotification notification = new MyHudNotification(MyStringId.GetOrCompute(desc));
					MyHud.Notifications.Add(notification);
					MyLog.Default.WriteLine(desc);
				};
				VRage.Profiler.MyRenderProfiler.GetProfilerFromServer = delegate
				{
					onProfilerCommandExecuted("Command executed: Download profiler");
					MyMultiplayer.Static.ProfilerDone = onProfilerCommandExecuted;
					MyMultiplayer.Static.DownloadProfiler();
				};
				MyRenderProfiler.ServerInvoke = delegate(RenderProfilerCommand cmd, int payload)
				{
					onProfilerCommandExecuted("Command executed: " + cmd);
					MyMultiplayer.RaiseStaticEvent((IMyEventOwner _) => MyRenderProfiler.OnCommandReceived, cmd, payload);
				};
			}
			else
			{
				failedToJoin?.Invoke();
			}
		}

		public static void JoinGame(MyGameServerItem server, bool enableGuiBackgroundFade = true, Action failedToJoin = null)
		{
			MyGameService.ServerDiscovery.GetServerRules(server, delegate(Dictionary<string, string> rules)
<<<<<<< HEAD
			{
				JoinGame(server, rules, enableGuiBackgroundFade, failedToJoin);
			}, delegate
			{
				JoinGame(server, null, enableGuiBackgroundFade, failedToJoin);
			});
		}

		public static void JoinServer(string address)
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && MySandboxGame.Static != null)
			{
				MySandboxGame.Static.Invoke(delegate
				{
					MySessionLoader.UnloadAndExitToMenu();
					MyGameService.OnPingServerResponded += MySandboxGame.Static.ServerResponded;
					MyGameService.OnPingServerFailedToRespond += MySandboxGame.Static.ServerFailedToRespond;
					MyGameService.PingServer(address);
				}, "UnloadAndExitToMenu");
			}
		}

		private static string GetNonConsentedServiceNameInMyMultiplayerClient(MyMultiplayerBase mClient)
		{
			string result = "";
			if (mClient != null && !Sync.IsDedicated)
			{
				using (List<MyObjectBuilder_Checkpoint.ModItem>.Enumerator enumerator = mClient.Mods.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						MyObjectBuilder_Checkpoint.ModItem current = enumerator.Current;
						IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(current.PublishedServiceName);
						if (aggregate == null)
						{
							return result;
						}
						if (!(aggregate.ServiceName == current.PublishedServiceName))
						{
							return result;
						}
						if (!aggregate.IsConsentGiven)
						{
							return aggregate.ServiceName;
						}
						return result;
					}
					return result;
				}
			}
			return result;
		}

		private static string GetNonConsentedServiceNameInMyCachedServerItem(MyCachedServerItem csi)
		{
			string result = "";
			if (csi != null && csi.UsedServices != null && csi.UsedServices.Count > 0 && !Sync.IsDedicated)
			{
				using (List<string>.Enumerator enumerator = csi.UsedServices.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(current);
=======
			{
				JoinGame(server, rules, enableGuiBackgroundFade, failedToJoin);
			}, delegate
			{
				JoinGame(server, null, enableGuiBackgroundFade, failedToJoin);
			});
		}

		private static string GetNonConsentedServiceNameInMyMultiplayerClient(MyMultiplayerBase mClient)
		{
			string result = "";
			if (mClient != null && !Sync.IsDedicated)
			{
				using (List<MyObjectBuilder_Checkpoint.ModItem>.Enumerator enumerator = mClient.Mods.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						MyObjectBuilder_Checkpoint.ModItem current = enumerator.Current;
						IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(current.PublishedServiceName);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						if (aggregate == null)
						{
							return result;
						}
<<<<<<< HEAD
						if (!(aggregate.ServiceName == current))
=======
						if (!(aggregate.ServiceName == current.PublishedServiceName))
						{
							return result;
						}
						if (!aggregate.IsConsentGiven)
						{
							return aggregate.ServiceName;
						}
						return result;
					}
					return result;
				}
			}
			return result;
		}

		private static string GetNonConsentedServiceNameInMyCachedServerItem(MyCachedServerItem csi)
		{
			string result = "";
			if (csi != null && csi.Mods != null && !Sync.IsDedicated)
			{
				using (List<WorkshopId>.Enumerator enumerator = csi.Mods.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						WorkshopId current = enumerator.Current;
						IMyUGCService aggregate = MyGameService.WorkshopService.GetAggregate(current.ServiceName);
						if (aggregate == null)
						{
							return result;
						}
						if (!(aggregate.ServiceName == current.ServiceName))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							return result;
						}
						if (!aggregate.IsConsentGiven)
						{
							return aggregate.ServiceName;
						}
						return result;
					}
					return result;
				}
			}
			return result;
		}

		public static void JoinGame(ulong lobbyId)
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
							JoinGameInternal(lobbyId);
							break;
						case PermissionResult.Error:
							MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, default(MyStringId), MyMessageBoxStyleEnum.Info);
							break;
						}
					});
					break;
				case PermissionResult.Error:
					MyGuiSandbox.Show(MyCommonTexts.XBoxPermission_MultiplayerError, default(MyStringId), MyMessageBoxStyleEnum.Info);
					break;
				}
			});
		}

		private static void JoinGameInternal(ulong lobbyId)
		{
			StringBuilder text = MyTexts.Get(MyCommonTexts.DialogTextJoiningWorld);
			MyGuiScreenProgress progress = new MyGuiScreenProgress(text, MyCommonTexts.Cancel);
			MyGuiSandbox.AddScreen(progress);
			progress.ProgressCancelled += delegate
			{
				MySessionLoader.UnloadAndExitToMenu();
			};
			MyLog.Default.WriteLine("Joining lobby: " + lobbyId);
			MyMultiplayerJoinResult result = MyMultiplayer.JoinLobby(lobbyId);
			result.JoinDone += delegate(bool success, IMyLobby lobby, MyLobbyStatusCode response, MyMultiplayerBase multiplayer)
			{
				if (multiplayer != null && multiplayer != null && !string.IsNullOrEmpty(GetNonConsentedServiceNameInMyMultiplayerClient(multiplayer)))
				{
					MySessionLoader.ShowUGCConsentNotAcceptedWarning(GetNonConsentedServiceNameInMyMultiplayerClient(multiplayer));
					foreach (MyGuiScreenBase screen in MyScreenManager.Screens)
					{
						if (screen is MyGuiScreenProgress)
						{
							screen.CloseScreenNow();
						}
					}
				}
				else
				{
					OnJoin(progress, success, lobby, response, multiplayer);
				}
			};
			progress.ProgressCancelled += delegate
			{
				result.Cancel();
			};
		}

		public static void OnJoin(MyGuiScreenProgress progress, bool success, IMyLobby lobby, MyLobbyStatusCode response, MyMultiplayerBase multiplayer)
		{
			MyLog.Default.WriteLine($"Lobby join response: {success}, enter state: {response}");
			if (success && response == MyLobbyStatusCode.Success && multiplayer.GetOwner() != Sync.MyId)
			{
				DownloadWorld(progress, multiplayer);
			}
			else
			{
				OnJoinFailed(progress, multiplayer, response);
			}
		}

		private static void DownloadWorld(MyGuiScreenProgress progress, MyMultiplayerBase multiplayer)
		{
			m_progress = progress;
			progress.Text = MyTexts.Get(MyCommonTexts.MultiplayerStateConnectingToServer);
			MyLog.Default.WriteLine("World requested");
			Stopwatch worldRequestTime = Stopwatch.StartNew();
			ulong serverId = multiplayer.GetOwner();
			bool connected = false;
			progress.Tick += delegate
			{
				MyP2PSessionState state = default(MyP2PSessionState);
				if (MyGameService.Peer2Peer != null)
				{
					MyGameService.Peer2Peer.GetSessionState(multiplayer.ServerId, ref state);
				}
				if (!connected && state.ConnectionActive)
				{
					MyLog.Default.WriteLine("World requested - connection alive");
					connected = true;
					progress.Text = MyTexts.Get(MyCommonTexts.MultiplayerStateWaitingForServer);
				}
				if (serverId != multiplayer.GetOwner())
				{
					MyLog.Default.WriteLine("World requested - failed, version mismatch");
					progress.Cancel();
					MyGuiSandbox.Show(MyCommonTexts.MultiplayerErrorServerHasLeft);
					multiplayer.Dispose();
				}
				else
				{
					bool flag = MyScreenManager.IsScreenOfTypeOpen(typeof(MyGuiScreenDownloadMods));
<<<<<<< HEAD
					if (!flag && !worldRequestTime.IsRunning)
					{
						worldRequestTime.Start();
					}
					else if (flag && worldRequestTime.IsRunning)
					{
						worldRequestTime.Stop();
					}
					if (worldRequestTime.IsRunning && worldRequestTime.Elapsed.TotalSeconds > 40.0)
=======
					if (!flag && !worldRequestTime.get_IsRunning())
					{
						worldRequestTime.Start();
					}
					else if (flag && worldRequestTime.get_IsRunning())
					{
						worldRequestTime.Stop();
					}
					if (worldRequestTime.get_IsRunning() && worldRequestTime.get_Elapsed().TotalSeconds > 40.0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						MyLog.Default.WriteLine("World requested - failed, server changed");
						progress.Cancel();
						MyGuiSandbox.Show(MyCommonTexts.MultiplaterJoin_ServerIsNotResponding);
						multiplayer.Dispose();
					}
				}
			};
			multiplayer.DownloadWorld(MyFinalBuildConstants.APP_VERSION);
		}

		public static StringBuilder GetErrorMessage(MyLobbyStatusCode response)
		{
			return new StringBuilder(string.Format(MyTexts.GetString(response switch
			{
				MyLobbyStatusCode.Error => MyCommonTexts.LobbyError, 
				MyLobbyStatusCode.Full => MyCommonTexts.LobbyFull, 
				MyLobbyStatusCode.Banned => MyCommonTexts.LobbyBanned, 
				MyLobbyStatusCode.Cancelled => MyCommonTexts.LobbyCancelled, 
				MyLobbyStatusCode.ClanDisabled => MyCommonTexts.LobbyClanDisabled, 
				MyLobbyStatusCode.CommunityBan => MyCommonTexts.LobbyCommunityBan, 
				MyLobbyStatusCode.ConnectionProblems => MyCommonTexts.LobbyConnectionProblems, 
				MyLobbyStatusCode.DoesntExist => MyCommonTexts.LobbyDoesntExist, 
				MyLobbyStatusCode.FriendsOnly => MyCommonTexts.OnlyFriendsCanJoinThisGame, 
				MyLobbyStatusCode.InvalidPasscode => MyCommonTexts.LobbyInvalidPasscode, 
				MyLobbyStatusCode.Limited => MyCommonTexts.LobbyLimited, 
				MyLobbyStatusCode.LostInternetConnection => MyCommonTexts.LobbyLostInternetConnection, 
				MyLobbyStatusCode.MemberBlockedYou => MyCommonTexts.LobbyMemberBlockedYou, 
				MyLobbyStatusCode.NoDirectConnections => MyCommonTexts.LobbyNoDirectConnections, 
				MyLobbyStatusCode.NotAllowed => MyCommonTexts.LobbyNotAllowed, 
				MyLobbyStatusCode.ServiceUnavailable => MyCommonTexts.LobbyServiceUnavailable, 
				MyLobbyStatusCode.UserMultiplayerRestricted => MyCommonTexts.LobbyUserMultiplayerRestricted, 
				MyLobbyStatusCode.VersionMismatch => MyCommonTexts.LobbyVersionMismatch, 
				MyLobbyStatusCode.YouBlockedMember => MyCommonTexts.LobbyYouBlockedMember, 
				MyLobbyStatusCode.NoUser => MyCommonTexts.LobbyNoUser, 
				_ => MyCommonTexts.LobbyError, 
			}), MySession.GameServiceName));
		}

		private static void OnJoinFailed(MyGuiScreenProgress progress, MyMultiplayerBase multiplayer, MyLobbyStatusCode response)
		{
			multiplayer?.Dispose();
			progress.Cancel();
			if (response != MyLobbyStatusCode.Success)
			{
				StringBuilder errorMessage = GetErrorMessage(response);
				MyLog.Default.WriteLine(string.Concat("OnJoinFailed: ", response, " / ", errorMessage));
				MyGuiSandbox.Show(errorMessage);
			}
		}

		private static void CheckDx11AndJoin(MyObjectBuilder_World world, MyMultiplayerBase multiplayer)
		{
			if (multiplayer.Scenario)
			{
				MySessionLoader.LoadMultiplayerScenarioWorld(world, multiplayer);
			}
			else
			{
				MySessionLoader.LoadMultiplayerSession(world, multiplayer);
			}
		}

		public static void OnDX11SwitchRequestAnswer(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				MySandboxGame.Config.GraphicsRenderer = MySandboxGame.DirectX11RendererKey;
				MySandboxGame.Config.Save();
				MyGuiSandbox.BackToMainMenu();
				StringBuilder messageText = MyTexts.Get(MySpaceTexts.QuickstartDX11PleaseRestartGame);
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageText, MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
			}
			else
			{
				StringBuilder messageText2 = MyTexts.Get(MySpaceTexts.QuickstartSelectDifferent);
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageText2, MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
			}
		}

		public static void WorldReceived(MyObjectBuilder_World world, MyMultiplayerBase multiplayer)
		{
			if (world == null)
			{
				MyLog.Default.WriteLine("World requested - failed, version mismatch");
				m_progress.Cancel();
				m_progress = null;
				MyGuiSandbox.Show(MyCommonTexts.MultiplayerErrorAppVersionMismatch);
				multiplayer.Dispose();
				return;
			}
			if (world?.Checkpoint?.Settings != null && !MySandboxGame.Config.ExperimentalMode)
			{
				bool num = world.Checkpoint.Settings.IsSettingsExperimental(remote: true) || (world.Checkpoint.Mods != null && world.Checkpoint.Mods.Count != 0);
				bool flag = (world.Checkpoint.SessionComponents.Find((MyObjectBuilder_SessionComponent x) => x is MyObjectBuilder_CampaignSessionComponent) as MyObjectBuilder_CampaignSessionComponent)?.IsVanilla ?? false;
				if (num && !flag)
				{
					MySessionLoader.UnloadAndExitToMenu();
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendFormat(MyCommonTexts.DialogTextJoinWorldFailed, MyTexts.GetString(MyCommonTexts.MultiplayerErrorExperimental));
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, stringBuilder, MyTexts.Get(MyCommonTexts.MessageBoxCaptionError)));
					return;
				}
			}
			m_progress = null;
			CheckDx11AndJoin(world, multiplayer);
		}
	}
}
