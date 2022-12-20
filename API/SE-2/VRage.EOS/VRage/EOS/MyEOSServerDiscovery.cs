using System;
using System.Collections.Generic;
using System.Linq;
using Epic.OnlineServices;
using Epic.OnlineServices.Connect;
using Epic.OnlineServices.Lobby;
using VRage.GameServices;
using VRage.Network;

namespace VRage.EOS
{
	internal sealed class MyEOSServerDiscovery : IMyServerDiscovery
	{
		private enum ConnectionState
		{
			Disconnected,
			Disconnecting,
			Connecting,
			Connected
		}

		private struct MyFilter
		{
			public string Key;

			public string Value;
		}

		private readonly MyEOSNetworking m_networking;

		private readonly LobbyInterface m_lobby;

		private readonly MyEOSLobbyList<MyGameServerItem> m_favoriteServers;

		private readonly MyEOSLobbyList<MyGameServerItem> m_historyServers;

		private string m_connectedLobbyId;

		private (MyGameServerItem serverItem, Action<JoinResult> onDone)? m_nextConnect;

		private ConnectionState m_connectionState;

		private readonly object m_stateLock = new object();

		private MyGameServerItem m_currentConnectingItem;

		private readonly List<MyGameServerItem> m_internetLobbySearchResults = new List<MyGameServerItem>();

		private bool m_internetLobbySearchInProgress;

		private readonly HashSet<MyGameServerItem> m_lastServerItemRequest = new HashSet<MyGameServerItem>();

		public string ServiceName => "EOS";

		public string ConnectionStringPrefix => "eos://";

		public ProductUserId LocalUserId => m_networking.Users.ProductUserId;

		public bool DedicatedSupport => true;

		public bool SupportsDirectServerSearch => true;

		public MySupportedPropertyFilters SupportedSearchParameters { get; } = new MySupportedPropertyFilters(new(string, MySearchConditionFlags)[5]
		{
			("SERVER_PROP_NAMES", MySearchConditionFlags.Contains),
			("SERVER_PROP_DATA", MySearchConditionFlags.Equal | MySearchConditionFlags.Contains),
			("SERVER_PROP_PLAYER_COUNT", MySearchConditionFlags.GreaterOrEqual | MySearchConditionFlags.LesserOrEqual),
			("SERVER_PROP_TAGS", MySearchConditionFlags.Equal | MySearchConditionFlags.Contains),
			("SERVER_CPROP_", MySearchConditionFlags.Equal | MySearchConditionFlags.Contains)
		});


		public bool LANSupport => false;

		public bool FriendSupport => true;

		public bool PingSupport => false;

		public bool GroupSupport => false;

		public bool FavoritesSupport => true;

		public bool HistorySupport => true;

		public event MyServerChangeRequested OnServerChangeRequested;

		public event EventHandler<int> OnDedicatedServerListResponded;

		public event EventHandler<MyMatchMakingServerResponse> OnDedicatedServersCompleteResponse;

		public event EventHandler<int> OnLANServerListResponded;

		public event EventHandler<MyMatchMakingServerResponse> OnLANServersCompleteResponse;

		public event EventHandler<MyGameServerItem> OnPingServerResponded;

		public event EventHandler OnPingServerFailedToRespond;

		public event EventHandler<int> OnFavoritesServerListResponded;

		public event EventHandler<MyMatchMakingServerResponse> OnFavoritesServersCompleteResponse;

		public event EventHandler<int> OnHistoryServerListResponded;

		public event EventHandler<MyMatchMakingServerResponse> OnHistoryServersCompleteResponse;

		public MyEOSServerDiscovery(MyEOSNetworking networking)
		{
			m_networking = networking;
			m_lobby = m_networking.Platform.GetLobbyInterface();
			m_networking.Service.OnUserChanged += OnUserChanged;
			MyVRage.Platform.Render.OnSuspending += RenderOnOnSuspending;
			m_lobby.AddNotifyLobbyMemberStatusReceived(new AddNotifyLobbyMemberStatusReceivedOptions(), null, LobbyMemberStatusChanged);
			m_favoriteServers = new MyEOSLobbyList<MyGameServerItem>(m_networking, "eos/cloud/favoriteServers.xml", GetGameServerItemFromLobbyHandle, RaiseOnFavoriteListResponded, DisposeGameServer);
			m_historyServers = new MyEOSLobbyList<MyGameServerItem>(m_networking, "eos/cloud/historyServers.xml", GetGameServerItemFromLobbyHandle, RaiseOnHistoryListResponded, DisposeGameServer, 100);
		}

		private void RenderOnOnSuspending()
		{
			ClearCachedGameServers();
		}

		private void LobbyMemberStatusChanged(LobbyMemberStatusReceivedCallbackInfo data)
		{
			if (m_connectedLobbyId == null || data.LobbyId != m_connectedLobbyId)
			{
				m_lobby.LeaveLobby(new LeaveLobbyOptions
				{
					LobbyId = data.LobbyId,
					LocalUserId = LocalUserId
				}, null, delegate
				{
				});
				return;
			}
			ProductUserId serverUserId = m_networking.EOSPeer2Peer.ServerUserId;
			if ((data.TargetUserId != serverUserId && data.CurrentStatus == LobbyMemberStatus.Promoted) || (data.TargetUserId == serverUserId && !data.CurrentStatus.IsConnected()))
			{
				m_networking.Log("Server disconnected from lobby. Leaving...");
				if (serverUserId != null)
				{
					m_networking.EOSPeer2Peer.RaiseConnectionFailed(serverUserId, "Server left lobby.");
				}
			}
		}

		public bool OnInvite(string dataProtocol)
		{
			string text = m_networking.EOSPlatform.RetrieveConnectionStringFromSession(dataProtocol);
			if (!string.IsNullOrEmpty(text))
			{
				this.OnServerChangeRequested?.Invoke(text, "");
				return true;
			}
			return false;
		}

		public void GetServerRules(MyGameServerItem serverItem, ServerRulesResponse completedAction, Action failedAction)
		{
			m_networking.InvokeOnNetworkThread(delegate
			{
				LobbyDetails lobbyDetails = (LobbyDetails)serverItem.LobbyHandle;
				if (lobbyDetails == null)
				{
					if (m_internetLobbySearchResults.Contains(serverItem))
					{
						m_networking.Error($"Lobby handle for {serverItem} is already disposed while the user asks for rules.");
					}
					m_networking.InvokeOnMainThread(failedAction);
				}
				else
				{
					Dictionary<string, string> dict = new Dictionary<string, string>();
					uint attributeCount = lobbyDetails.GetAttributeCount(new LobbyDetailsGetAttributeCountOptions());
					for (int i = 0; i < attributeCount; i++)
					{
						lobbyDetails.CopyAttributeByIndex(new LobbyDetailsCopyAttributeByIndexOptions
						{
							AttrIndex = (uint)i
						}, out var outAttribute);
						if (outAttribute.Data.Key.StartsWith("RULE_"))
						{
							dict.Add(outAttribute.Data.Key.Substring("RULE_".Length).ToLower(), outAttribute.Data.Value.AsUtf8);
						}
					}
					m_networking.InvokeOnMainThread(delegate
					{
						completedAction(dict);
					});
				}
			});
		}

		public bool Connect(MyGameServerItem serverItem, Action<JoinResult> onDone)
		{
			lock (m_stateLock)
			{
				if (m_connectionState == ConnectionState.Connecting || m_connectionState == ConnectionState.Connected || m_nextConnect.HasValue)
				{
					return false;
				}
				MyServiceManager.Instance.AddService((IMyNetworking)m_networking);
				if (m_connectionState == ConnectionState.Disconnecting)
				{
					m_nextConnect = new(MyGameServerItem, Action<JoinResult>)?((serverItem, onDone));
				}
				else
				{
					m_connectionState = ConnectionState.Connecting;
					m_networking.InvokeOnNetworkThread(delegate
					{
						ConnectInternal(serverItem, onDone);
					});
				}
				return true;
			}
		}

		private void ConnectInternal(MyGameServerItem serverItem, Action<JoinResult> onDone)
		{
			DateTime networkOrLocalTimeUTC = GetNetworkOrLocalTimeUTC();
			if (new TimeSpan(Math.Abs((networkOrLocalTimeUTC - DateTime.UtcNow).Ticks)) > TimeSpan.FromHours(3.0))
			{
				OnConnectDone(serverItem, Result.AccessDenied, onDone, JoinResult.IncorrectTime);
				return;
			}
			LobbyDetails lobbyDetails = (LobbyDetails)serverItem.LobbyHandle;
			m_currentConnectingItem = serverItem;
			if (lobbyDetails == null)
			{
				FindGameServer(serverItem.ConnectionString, delegate(MyGameServerItem item)
				{
					if (item == null)
					{
						OnConnectDone(serverItem, Result.NotFound, onDone);
					}
					else
					{
						ConnectInternal(item, onDone);
					}
				});
				return;
			}
			m_lobby.JoinLobby(new JoinLobbyOptions
			{
				LocalUserId = LocalUserId,
				LobbyDetailsHandle = lobbyDetails
			}, null, delegate(JoinLobbyCallbackInfo joinResult)
			{
				if (joinResult.ResultCode == Result.Success)
				{
					UpdateLobbyModificationOptions options = new UpdateLobbyModificationOptions
					{
						LobbyId = joinResult.LobbyId,
						LocalUserId = LocalUserId
					};
					if (m_lobby.UpdateLobbyModification(options, out var lobbyModificationHandle) != 0)
					{
						m_lobby.LeaveLobby(new LeaveLobbyOptions
						{
							LobbyId = joinResult.LobbyId,
							LocalUserId = LocalUserId
						}, null, delegate
						{
						});
						OnConnectDone(serverItem, Result.NotFound, onDone);
					}
					lobbyModificationHandle.AddMemberAttribute(new LobbyModificationAddMemberAttributeOptions
					{
						Attribute = new AttributeData
						{
							Key = "MEMBER_SERVICE_KIND",
							Value = (long)m_networking.Service.GetServiceKind()
						}
					});
					lobbyModificationHandle.AddMemberAttribute(new LobbyModificationAddMemberAttributeOptions
					{
						Attribute = new AttributeData
						{
							Key = "MEMBER_SERVICE_ID",
							Value = m_networking.Service.UserId.ToString()
						}
					});
					m_lobby.CopyLobbyDetailsHandle(new CopyLobbyDetailsHandleOptions
					{
						LocalUserId = LocalUserId,
						LobbyId = joinResult.LobbyId
					}, out var outLobbyDetailsHandle);
					Epic.OnlineServices.Lobby.Attribute outAttribute = null;
					for (uint num = 0u; num < outLobbyDetailsHandle.GetMemberCount(new LobbyDetailsGetMemberCountOptions()); num++)
					{
						outLobbyDetailsHandle.CopyMemberAttributeByKey(new LobbyDetailsCopyMemberAttributeByKeyOptions
						{
							TargetUserId = outLobbyDetailsHandle.GetMemberByIndex(new LobbyDetailsGetMemberByIndexOptions
							{
								MemberIndex = num
							}),
							AttrKey = m_networking.EOSPlatform.SessionNameAttributeKey
						}, out outAttribute);
						if (outAttribute != null && !string.IsNullOrEmpty(outAttribute.Data.Value.AsUtf8))
						{
							break;
						}
					}
					string sessionName = outAttribute?.Data.Value.AsUtf8 ?? string.Empty;
					if (m_networking.EOSPlatform.CreateOrJoinSession(serverItem.ConnectionString, ref sessionName))
					{
						lobbyModificationHandle.AddMemberAttribute(new LobbyModificationAddMemberAttributeOptions
						{
							Visibility = LobbyAttributeVisibility.Public,
							Attribute = new AttributeData
							{
								Key = m_networking.EOSPlatform.SessionNameAttributeKey,
								Value = new AttributeDataValue
								{
									AsUtf8 = sessionName
								}
							}
						});
					}
					outLobbyDetailsHandle.Release();
					UpdateLobbyOptions options2 = new UpdateLobbyOptions
					{
						LobbyModificationHandle = lobbyModificationHandle
					};
					m_lobby.UpdateLobby(options2, null, delegate(UpdateLobbyCallbackInfo info)
					{
						lobbyModificationHandle.Release();
						OnConnectDone(serverItem, info.ResultCode, onDone);
					});
				}
				else if (joinResult.ResultCode == Result.LobbyLobbyAlreadyExists)
				{
					OnConnectDone(serverItem, Result.Success, onDone);
				}
				else
				{
					OnConnectDone(null, joinResult.ResultCode, onDone);
				}
			});
		}

		private void OnConnectDone(MyGameServerItem serverItem, Result result, Action<JoinResult> onDone, JoinResult? actualJoinResult = null)
		{
			m_connectedLobbyId = null;
			if (result == Result.Success)
			{
				result = ((LobbyDetails)serverItem.LobbyHandle).CopyInfo(new LobbyDetailsCopyInfoOptions(), out var outLobbyDetailsInfo);
				if (result == Result.Success)
				{
					m_networking.EOSPeer2Peer.SetHost(outLobbyDetailsInfo.LobbyOwnerUserId, serverItem.SteamID);
					m_connectedLobbyId = outLobbyDetailsInfo.LobbyId;
				}
			}
			JoinResult joinResult = ((result != 0) ? JoinResult.UserNotConnected : JoinResult.OK);
			if (!actualJoinResult.HasValue)
			{
				switch (result)
				{
				case Result.NotFound:
					joinResult = JoinResult.NotFound;
					break;
				case Result.LobbyTooManyPlayers:
					joinResult = JoinResult.ServerFull;
					break;
				case Result.LobbyHostAtCapacity:
					joinResult = JoinResult.ServerFull;
					break;
				case Result.LobbyNoPermission:
					joinResult = JoinResult.NoLicenseOrExpired;
					break;
				case Result.LobbyNotAllowed:
					joinResult = JoinResult.NoLicenseOrExpired;
					break;
				case Result.LobbyInviteFailed:
					joinResult = JoinResult.NoLicenseOrExpired;
					break;
				case Result.LobbyInviteNotFound:
					joinResult = JoinResult.NoLicenseOrExpired;
					break;
				}
			}
			else
			{
				joinResult = actualJoinResult.Value;
			}
			lock (m_stateLock)
			{
				if (m_connectedLobbyId == null)
				{
					m_connectionState = ConnectionState.Disconnected;
					m_networking.InvokeOnMainThread(delegate
					{
						onDone(joinResult);
					});
				}
				else if (m_connectionState == ConnectionState.Disconnecting)
				{
					DisconnectInternal();
				}
				else
				{
					m_connectionState = ConnectionState.Connected;
					m_networking.InvokeOnMainThread(delegate
					{
						onDone(joinResult);
					});
				}
			}
			m_currentConnectingItem = null;
			if (serverItem != null)
			{
				DisposeGameServer(serverItem);
			}
		}

		public void Disconnect()
		{
			lock (m_stateLock)
			{
				if (m_connectionState == ConnectionState.Connected || m_connectionState == ConnectionState.Connecting)
				{
					if (m_connectionState == ConnectionState.Connected)
					{
						m_networking.EOSPeer2Peer.SetDisconnecting();
						m_networking.InvokeOnNetworkThread(DisconnectInternal, TimeSpan.FromSeconds(5.0));
					}
					m_connectionState = ConnectionState.Disconnecting;
				}
			}
		}

		private void DisconnectInternal()
		{
			m_lobby.LeaveLobby(new LeaveLobbyOptions
			{
				LobbyId = m_connectedLobbyId,
				LocalUserId = LocalUserId
			}, null, delegate
			{
			});
			m_connectedLobbyId = null;
			m_networking.EOSPeer2Peer.ClearHost();
			lock (m_stateLock)
			{
				if (m_connectionState == ConnectionState.Disconnecting && !m_nextConnect.HasValue)
				{
					m_networking.InvokeOnMainThread(delegate
					{
						lock (m_stateLock)
						{
							if (m_connectionState == ConnectionState.Disconnected && MyServiceManager.Instance.GetService<IMyNetworking>() is MyEOSNetworking)
							{
								MyServiceManager.Instance.RemoveService<IMyNetworking>();
							}
						}
					});
				}
				m_networking.EOSPlatform.DisconnectSession();
				m_connectionState = ConnectionState.Disconnected;
				if (m_nextConnect.HasValue)
				{
					(MyGameServerItem serverItem, Action<JoinResult> onDone) conn = m_nextConnect.Value;
					m_nextConnect = null;
					m_networking.InvokeOnMainThread(delegate
					{
						Connect(conn.serverItem, conn.onDone);
					});
				}
			}
		}

		private List<MyFilter> GetFilters(string filterOps)
		{
			List<MyFilter> list = new List<MyFilter>();
			if (string.IsNullOrEmpty(filterOps))
			{
				return list;
			}
			string[] array = filterOps.Split(new char[1] { ';' });
			MyFilter item = default(MyFilter);
			foreach (string text in array)
			{
				int num = text.IndexOf(':');
				if (num < 0)
				{
					m_networking.Error("Wrong Filter Argument: " + text);
					continue;
				}
				item.Key = text.Substring(0, num).Trim();
				item.Value = text.Substring(num + 1).Trim();
				list.Add(item);
			}
			return list;
		}

		public void RequestServerItems(string[] connectionStrings, MySessionSearchFilter filter, Action<IEnumerable<MyGameServerItem>> resultCallback)
		{
			for (int i = 0; i < connectionStrings.Length; i++)
			{
				connectionStrings[i] = MyEOSNetworking.ParseConnectionString(connectionStrings[i]);
			}
			m_networking.InvokeOnNetworkThread(delegate
			{
				RequestServerItemsInternal(connectionStrings, filter, resultCallback);
			});
		}

		private void RequestServerItemsInternal(string[] connectionStrings, MySessionSearchFilter filter, Action<IEnumerable<MyGameServerItem>> resultCallback)
		{
			foreach (MyGameServerItem item in m_lastServerItemRequest)
			{
				if (item != null)
				{
					DisposeGameServer(item);
				}
			}
			m_lastServerItemRequest.Clear();
			new MyLobbySearch<MyGameServerItem>(m_networking, GetGameServerItemFromLobbyHandle).Search(connectionStrings, filter, delegate(Result result, (string ConnectingString, MyGameServerItem Item)[] servers)
			{
				m_lastServerItemRequest.UnionWith(from x in servers
					select x.Item into x
					where x != null
					select x);
				MyGameServerItem[] results = m_lastServerItemRequest.ToArray();
				m_networking.InvokeOnMainThread(delegate
				{
					resultCallback(results);
				});
			});
		}

		public void RequestInternetServerList(MySessionSearchFilter filter)
		{
			m_networking.InvokeOnNetworkThread(delegate
			{
				RequestInternetServerListInternal(filter);
			});
		}

		private void RequestInternetServerListInternal(MySessionSearchFilter filter)
		{
			if (m_internetLobbySearchInProgress)
			{
				m_networking.Log("DS Search already in progress.");
				return;
			}
			m_internetLobbySearchInProgress = true;
			CreateLobbySearchOptions options = new CreateLobbySearchOptions
			{
				MaxResults = 200u
			};
			LobbySearch internetLobbySearch;
			Result result = m_lobby.CreateLobbySearch(options, out internetLobbySearch);
			m_networking.ApplySearchFilter(internetLobbySearch, filter);
			LobbySearchFindOptions options2 = new LobbySearchFindOptions
			{
				LocalUserId = LocalUserId
			};
			m_networking.Log("DS Search started.");
			internetLobbySearch.Find(options2, null, delegate(LobbySearchFindCallbackInfo info)
			{
				if (info.ResultCode == Result.Success)
				{
					uint count = 0u;
					lock (m_internetLobbySearchResults)
					{
						foreach (MyGameServerItem internetLobbySearchResult in m_internetLobbySearchResults)
						{
							DisposeGameServer(internetLobbySearchResult);
						}
						m_internetLobbySearchResults.Clear();
						count = internetLobbySearch.GetSearchResultCount(new LobbySearchGetSearchResultCountOptions());
						m_networking.Log($"DS Search results: {count}");
						for (int i = 0; i < count; i++)
						{
							result = internetLobbySearch.CopySearchResultByIndex(new LobbySearchCopySearchResultByIndexOptions
							{
								LobbyIndex = (uint)i
							}, out var outLobbyDetailsHandle);
							result = outLobbyDetailsHandle.CopyInfo(new LobbyDetailsCopyInfoOptions(), out var outLobbyDetailsInfo);
							if (!(outLobbyDetailsInfo.LobbyOwnerUserId == null) && outLobbyDetailsInfo.LobbyOwnerUserId.IsValid())
							{
								outLobbyDetailsHandle.CopyAttributeByKey(new LobbyDetailsCopyAttributeByKeyOptions
								{
									AttrKey = "OWNER_EOS_ID"
								}, out var outAttribute);
								if (!(outAttribute?.Data.Value.AsUtf8 != outLobbyDetailsInfo.LobbyOwnerUserId.GetIdString()))
								{
									m_internetLobbySearchResults.Add(GetGameServerItemFromLobbyHandle(outLobbyDetailsHandle));
									int idx = m_internetLobbySearchResults.Count - 1;
									m_networking.InvokeOnMainThread(delegate
									{
										this.OnDedicatedServerListResponded?.Invoke(this, idx);
									});
								}
							}
						}
					}
					m_networking.InvokeOnMainThread(delegate
					{
						this.OnDedicatedServersCompleteResponse?.Invoke(this, (count == 0) ? MyMatchMakingServerResponse.NoServersListedOnMasterServer : MyMatchMakingServerResponse.ServerResponded);
						m_internetLobbySearchInProgress = false;
					});
				}
				else
				{
					m_networking.Log($"DS Search failed: {info.ResultCode}");
					m_networking.InvokeOnMainThread(delegate
					{
						this.OnDedicatedServersCompleteResponse?.Invoke(this, MyMatchMakingServerResponse.ServerFailedToRespond);
						m_internetLobbySearchInProgress = false;
					});
				}
				internetLobbySearch?.Release();
			});
		}

		public void CancelInternetServersRequest()
		{
		}

		public MyGameServerItem GetDedicatedServerDetails(int serverIndex)
		{
			lock (m_internetLobbySearchResults)
			{
				return m_internetLobbySearchResults[serverIndex];
			}
		}

		internal MyGameServerItem GetGameServerItemFromLobbyHandle(LobbyDetails handle, LobbyDetailsInfo details = null)
		{
			if (details == null)
			{
				handle.CopyInfo(new LobbyDetailsCopyInfoOptions(), out details);
			}
			handle.CopyAttributeByKey(new LobbyDetailsCopyAttributeByKeyOptions
			{
				AttrKey = "GAME_DESCRIPTION"
			}, out var outAttribute);
			handle.CopyAttributeByKey(new LobbyDetailsCopyAttributeByKeyOptions
			{
				AttrKey = "MOD_DIR"
			}, out var outAttribute2);
			handle.CopyAttributeByKey(new LobbyDetailsCopyAttributeByKeyOptions
			{
				AttrKey = "TAGS"
			}, out var outAttribute3);
			handle.CopyAttributeByKey(new LobbyDetailsCopyAttributeByKeyOptions
			{
				AttrKey = "MAP_NAME"
			}, out var outAttribute4);
			handle.CopyAttributeByKey(new LobbyDetailsCopyAttributeByKeyOptions
			{
				AttrKey = "SERVER_NAME"
			}, out var outAttribute5);
			handle.CopyAttributeByKey(new LobbyDetailsCopyAttributeByKeyOptions
			{
				AttrKey = "VERSION"
			}, out var outAttribute6);
			handle.CopyAttributeByKey(new LobbyDetailsCopyAttributeByKeyOptions
			{
				AttrKey = "IS_PASSWORD_PROTECTED"
			}, out var outAttribute7);
			handle.CopyAttributeByKey(new LobbyDetailsCopyAttributeByKeyOptions
			{
				AttrKey = "OWNER_SERVICE_ID"
			}, out var outAttribute8);
			MyGameServerItem myGameServerItem = new MyGameServerItem
			{
				AppID = m_networking.Service.AppId,
				GameDescription = outAttribute?.Data.Value.AsUtf8,
				GameDir = outAttribute2?.Data.Value.AsUtf8,
				GameTags = outAttribute3?.Data.Value.AsUtf8,
				HadSuccessfulResponse = true,
				Map = outAttribute4?.Data.Value.AsUtf8,
				MaxPlayers = (int)(details.MaxMembers - 1),
				Name = outAttribute5?.Data.Value.AsUtf8,
				Password = (outAttribute7?.Data.Value.AsUtf8.ToLower() == "true"),
				Ping = -1,
				Players = (int)(details.MaxMembers - details.AvailableSlots - 1),
				Secure = true,
				ServerVersion = int.Parse(outAttribute6?.Data.Value.AsUtf8 ?? "0"),
				SteamID = ulong.Parse(outAttribute8?.Data.Value.AsUtf8 ?? "0"),
				LobbyHandle = handle,
				ConnectionString = "eos://" + details.LobbyOwnerUserId.GetIdString()
			};
			details.LobbyOwnerUserId.GetIdString();
			if (!string.IsNullOrEmpty(myGameServerItem.GameTags))
			{
				myGameServerItem.GameTagList.AddRange(myGameServerItem.GameTags.Split(new char[1] { ' ' }));
			}
			return myGameServerItem;
		}

		public void RequestLANServerList()
		{
			this.OnLANServersCompleteResponse?.Invoke(this, MyMatchMakingServerResponse.NoServersListedOnMasterServer);
		}

		public void CancelLANServersRequest()
		{
		}

		public MyGameServerItem GetLANServerDetails(int server)
		{
			return null;
		}

		public void PingServer(string connectionString)
		{
			m_networking.InvokeOnNetworkThread(delegate
			{
				m_networking.SearchForLobby(connectionString, delegate(LobbyDetails detailsHandle, LobbyDetailsInfo details)
				{
					if (detailsHandle != null)
					{
						RaiseOnPingServerResponded(GetGameServerItemFromLobbyHandle(detailsHandle, details));
					}
					else
					{
						RaiseOnPingServerFailedToRespond();
					}
				});
			});
		}

		public void GetPlayerDetails(MyGameServerItem serverItem, PlayerDetailsResponse completedAction, Action failedAction)
		{
			m_networking.InvokeOnNetworkThread(delegate
			{
				GetServerDetailsInternal(serverItem, completedAction, failedAction);
			});
		}

		private void GetServerDetailsInternal(MyGameServerItem serverItem, PlayerDetailsResponse completedAction, Action failedAction)
		{
			LobbyDetails lobbyDetails = (LobbyDetails)serverItem.LobbyHandle;
			uint memberCount = lobbyDetails.GetMemberCount(new LobbyDetailsGetMemberCountOptions());
			Dictionary<string, float> results = new Dictionary<string, float>();
			if (memberCount <= 1)
			{
				m_networking.InvokeOnMainThread(delegate
				{
					completedAction(new Dictionary<string, float>());
				});
				return;
			}
			ConnectInterface connect = m_networking.Platform.GetConnectInterface();
			ProductUserId lobbyOwner = lobbyDetails.GetLobbyOwner(new LobbyDetailsGetLobbyOwnerOptions());
			int num = 0;
			ProductUserId[] uids = new ProductUserId[memberCount - 1];
			for (int i = 0; i < memberCount; i++)
			{
				ProductUserId memberByIndex = lobbyDetails.GetMemberByIndex(new LobbyDetailsGetMemberByIndexOptions
				{
					MemberIndex = (uint)i
				});
				if (!(memberByIndex == lobbyOwner))
				{
					uids[num++] = memberByIndex;
				}
			}
			connect.QueryProductUserIdMappings(new QueryProductUserIdMappingsOptions
			{
				LocalUserId = LocalUserId,
				ProductUserIds = uids
			}, null, delegate(QueryProductUserIdMappingsCallbackInfo data)
			{
				if (data.ResultCode != 0)
				{
					m_networking.InvokeOnMainThread(failedAction);
				}
				else
				{
					for (int j = 0; j < uids.Length; j++)
					{
						if (connect.CopyProductUserInfo(new CopyProductUserInfoOptions
						{
							TargetUserId = uids[j]
						}, out var outExternalAccountInfo) == Result.Success)
						{
							results[outExternalAccountInfo.DisplayName] = -1f;
						}
					}
					m_networking.InvokeOnMainThread(delegate
					{
						completedAction(results);
					});
				}
			});
		}

		private void OnUserChanged(bool differentUserLoggedIn)
		{
			ClearCachedGameServers();
		}

		private void RaiseOnPingServerResponded(MyGameServerItem e)
		{
			m_networking.InvokeOnMainThread(delegate
			{
				this.OnPingServerResponded?.Invoke(this, e);
			});
		}

		private void RaiseOnPingServerFailedToRespond()
		{
			m_networking.InvokeOnMainThread(delegate
			{
				this.OnPingServerFailedToRespond?.Invoke(this, EventArgs.Empty);
			});
		}

		public MyGameServerItem GetFavoritesServerDetails(int server)
		{
			return m_favoriteServers.Get(server);
		}

		public void RequestFavoritesServerList(MySessionSearchFilter filterOps)
		{
			if (!m_favoriteServers.IsRequesting)
			{
				m_networking.InvokeOnNetworkThread(delegate
				{
					m_favoriteServers.QueryServerList(filterOps);
				});
			}
		}

		public void CancelFavoritesServersRequest()
		{
		}

		public void AddFavoriteGame(string connectionString)
		{
			m_networking.InvokeOnNetworkThread(delegate
			{
				m_favoriteServers.AddServer(connectionString);
			});
		}

		public void AddFavoriteGame(MyGameServerItem serverItem)
		{
			m_networking.InvokeOnNetworkThread(delegate
			{
				m_favoriteServers.AddServer(serverItem.ConnectionString, serverItem);
			});
		}

		public void RemoveFavoriteGame(MyGameServerItem serverItem)
		{
			m_networking.InvokeOnNetworkThread(delegate
			{
				m_favoriteServers.RemoveServer(serverItem.ConnectionString, serverItem);
			});
		}

		public void RequestHistoryServerList(MySessionSearchFilter filterOps)
		{
			m_networking.InvokeOnNetworkThread(delegate
			{
				m_historyServers.QueryServerList(filterOps);
			});
		}

		public void CancelHistoryServersRequest()
		{
		}

		public void AddHistoryGame(MyGameServerItem serverItem)
		{
			m_networking.InvokeOnNetworkThread(delegate
			{
				m_historyServers.AddServer(serverItem.ConnectionString, serverItem);
			});
		}

		public MyGameServerItem GetHistoryServerDetails(int server)
		{
			return m_historyServers.Get(server);
		}

		private void FindGameServer(string connectionString, Action<MyGameServerItem> onResult)
		{
			m_networking.SearchForLobby(connectionString, delegate(LobbyDetails detailsHandle, LobbyDetailsInfo details)
			{
				if (detailsHandle != null)
				{
					onResult(GetGameServerItemFromLobbyHandle(detailsHandle, details));
				}
				else
				{
					onResult(null);
				}
			});
		}

		private void DisposeGameServer(MyGameServerItem item)
		{
			LobbyDetails lobbyDetails;
			if (!m_internetLobbySearchResults.Contains(item) && !m_lastServerItemRequest.Contains(item) && m_currentConnectingItem != item && (object)(lobbyDetails = item.LobbyHandle as LobbyDetails) != null)
			{
				lobbyDetails.Release();
				item.LobbyHandle = null;
			}
		}

		private void ClearCachedGameServers()
		{
			lock (m_internetLobbySearchResults)
			{
				m_internetLobbySearchResults.ForEach(DisposeGameServer);
				m_internetLobbySearchResults.Clear();
			}
			m_favoriteServers.Clear();
			m_historyServers.Clear();
		}

		private void RaiseOnFavoriteListResponded()
		{
			m_networking.InvokeOnMainThread(delegate
			{
				int itemCount = m_favoriteServers.ItemCount;
				for (int i = 0; i < itemCount; i++)
				{
					this.OnFavoritesServerListResponded?.Invoke(this, i);
				}
				this.OnFavoritesServersCompleteResponse?.Invoke(this, MyMatchMakingServerResponse.ServerResponded);
			});
		}

		private void RaiseOnHistoryListResponded()
		{
			m_networking.InvokeOnMainThread(delegate
			{
				int itemCount = m_historyServers.ItemCount;
				for (int i = 0; i < itemCount; i++)
				{
					this.OnHistoryServerListResponded?.Invoke(this, i);
				}
				this.OnHistoryServersCompleteResponse?.Invoke(this, MyMatchMakingServerResponse.ServerResponded);
			});
		}

		private DateTime GetNetworkOrLocalTimeUTC()
		{
			try
			{
				return MyVRage.Platform.System.GetNetworkTimeUTC();
			}
			catch (Exception ex)
			{
				m_networking.Log("Cannot query network time: " + ex.Message);
				return DateTime.UtcNow;
			}
		}
	}
}
