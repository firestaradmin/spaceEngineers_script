using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Steamworks;
using VRage.GameServices;
using VRage.Network;

namespace VRage.Steam
{
	internal class MySteamServerDiscovery : IMyServerDiscovery, IDisposable
	{
		private const string CONNECT_STRING_PREFIX = "steam://";

		private static readonly object m_ruleResponseLockObject = new object();

		private static readonly object m_playerResponseLockObject = new object();

		private readonly List<MatchmakingRulesResponse> m_ruleResponses = new List<MatchmakingRulesResponse>();

		private readonly List<MatchmakingPlayersResponse> m_playerResponses = new List<MatchmakingPlayersResponse>();

		private readonly ISteamMatchmakingServerListResponse m_lanRequestServerResponse;

		private HServerListRequest? m_lanRequest;

		private readonly ISteamMatchmakingServerListResponse m_dedicatedRequestServerResponse;

		private HServerListRequest? m_dedicatedRequest;

		private readonly ISteamMatchmakingServerListResponse m_favoritesRequestServerResponse;

		private HServerListRequest? m_favoritesRequest;

		private readonly ISteamMatchmakingServerListResponse m_historyRequestServerResponse;

		private HServerListRequest? m_historyRequest;

		private readonly ISteamMatchmakingPingResponse m_pingServerResponse;

		private readonly MySteamNetworking m_steamNetworking;

		private readonly Callback<GameServerChangeRequested_t> m_serverChangeRequested;

		public string ServiceName => "Steam";

		public string ConnectionStringPrefix => "steam://";

		public bool LANSupport => true;

		public bool FriendSupport => true;

		public bool PingSupport => true;

		public bool GroupSupport => true;

		public bool DedicatedSupport => true;

		public bool FavoritesSupport => true;

		public bool HistorySupport => true;

		public bool SupportsDirectServerSearch => false;

		public MySupportedPropertyFilters SupportedSearchParameters { get; } = new MySupportedPropertyFilters(new(string, MySearchConditionFlags)[1] { ("SERVER_PROP_DATA", MySearchConditionFlags.Equal) });


		public event EventHandler<int> OnLANServerListResponded;

		public event EventHandler<MyMatchMakingServerResponse> OnLANServersCompleteResponse;

		public event EventHandler<int> OnDedicatedServerListResponded;

		public event EventHandler<MyMatchMakingServerResponse> OnDedicatedServersCompleteResponse;

		public event EventHandler<int> OnFavoritesServerListResponded;

		public event EventHandler<MyMatchMakingServerResponse> OnFavoritesServersCompleteResponse;

		public event EventHandler<int> OnHistoryServerListResponded;

		public event EventHandler<MyMatchMakingServerResponse> OnHistoryServersCompleteResponse;

		public event EventHandler<MyGameServerItem> OnPingServerResponded;

		public event EventHandler OnPingServerFailedToRespond;

		public event MyServerChangeRequested OnServerChangeRequested;

		public MySteamServerDiscovery(MySteamNetworking steamNetworking)
		{
			m_steamNetworking = steamNetworking;
			m_lanRequestServerResponse = new ISteamMatchmakingServerListResponse(OnRequestServerResponded, OnServerRequestFailed, OnServerRequestComplete);
			m_dedicatedRequestServerResponse = new ISteamMatchmakingServerListResponse(OnRequestServerResponded, OnServerRequestFailed, OnServerRequestComplete);
			m_favoritesRequestServerResponse = new ISteamMatchmakingServerListResponse(OnRequestServerResponded, OnServerRequestFailed, OnServerRequestComplete);
			m_historyRequestServerResponse = new ISteamMatchmakingServerListResponse(OnRequestServerResponded, OnServerRequestFailed, OnServerRequestComplete);
			m_pingServerResponse = new ISteamMatchmakingPingResponse(OnPingRequestServerResponded, OnPingRequestServerFailed);
			m_serverChangeRequested = Callback<GameServerChangeRequested_t>.Create(HandleServerChange);
		}

		public void Dispose()
		{
			m_serverChangeRequested?.Unregister();
		}

		private void OnRequestServerResponded(HServerListRequest hRequest, int server)
		{
			HServerListRequest value = hRequest;
			HServerListRequest? lanRequest = m_lanRequest;
			if (value == lanRequest)
			{
				this.OnLANServerListResponded?.Invoke(this, server);
			}
			value = hRequest;
			lanRequest = m_favoritesRequest;
			if (value == lanRequest)
			{
				this.OnFavoritesServerListResponded?.Invoke(this, server);
			}
			value = hRequest;
			lanRequest = m_historyRequest;
			if (value == lanRequest)
			{
				this.OnHistoryServerListResponded?.Invoke(this, server);
			}
			value = hRequest;
			lanRequest = m_dedicatedRequest;
			if (value == lanRequest)
			{
				this.OnDedicatedServerListResponded?.Invoke(this, server);
			}
		}

		private void OnServerRequestFailed(HServerListRequest hRequest, int server)
		{
		}

		private void OnServerRequestComplete(HServerListRequest hRequest, EMatchMakingServerResponse response)
		{
			HServerListRequest value = hRequest;
			HServerListRequest? lanRequest = m_lanRequest;
			if (value == lanRequest)
			{
				this.OnLANServersCompleteResponse?.Invoke(this, (MyMatchMakingServerResponse)response);
				m_lanRequest = null;
			}
			value = hRequest;
			lanRequest = m_favoritesRequest;
			if (value == lanRequest)
			{
				this.OnFavoritesServersCompleteResponse?.Invoke(this, (MyMatchMakingServerResponse)response);
				m_favoritesRequest = null;
			}
			value = hRequest;
			lanRequest = m_historyRequest;
			if (value == lanRequest)
			{
				this.OnHistoryServersCompleteResponse?.Invoke(this, (MyMatchMakingServerResponse)response);
				m_historyRequest = null;
			}
			value = hRequest;
			lanRequest = m_dedicatedRequest;
			if (value == lanRequest)
			{
				this.OnDedicatedServersCompleteResponse?.Invoke(this, (MyMatchMakingServerResponse)response);
				m_dedicatedRequest = null;
			}
			SteamMatchmakingServers.ReleaseRequest(hRequest);
		}

		private List<MatchMakingKeyValuePair_t> GetFilters(MySessionSearchFilter filterOps)
		{
			List<MatchMakingKeyValuePair_t> list = new List<MatchMakingKeyValuePair_t>();
			MatchMakingKeyValuePair_t item = new MatchMakingKeyValuePair_t
			{
				m_szKey = "secure",
				m_szValue = "1"
			};
			list.Add(item);
			item = new MatchMakingKeyValuePair_t
			{
				m_szKey = "gamedir",
				m_szValue = m_steamNetworking.ProductName
			};
			list.Add(item);
			MySessionSearchFilter.Query query = filterOps.Queries.FirstOrDefault((MySessionSearchFilter.Query x) => x.Property == "SERVER_PROP_DATA");
			if (query.Property != null)
			{
				item = new MatchMakingKeyValuePair_t
				{
					m_szKey = "gamedataand",
					m_szValue = query.Value
				};
				list.Add(item);
			}
			return list;
		}

		private static MyGameServerItem GetGameServer(gameserveritem_t serverItem)
		{
			MyGameServerItem myGameServerItem = new MyGameServerItem
			{
				AppID = serverItem.m_nAppID,
				BotPlayers = serverItem.m_nBotPlayers,
				DoNotRefresh = serverItem.m_bDoNotRefresh,
				GameDescription = serverItem.GetGameDescription(),
				GameDir = serverItem.GetGameDir(),
				GameTags = serverItem.GetGameTags(),
				HadSuccessfulResponse = serverItem.m_bHadSuccessfulResponse,
				Map = serverItem.GetMap(),
				MaxPlayers = serverItem.m_nMaxPlayers,
				Name = serverItem.GetServerName(),
				Password = serverItem.m_bPassword,
				Ping = serverItem.m_nPing,
				Players = serverItem.m_nPlayers,
				Secure = serverItem.m_bSecure,
				ServerVersion = serverItem.m_nServerVersion,
				SteamID = serverItem.m_steamID.m_SteamID,
				TimeLastPlayed = serverItem.m_ulTimeLastPlayed
			};
			if (!string.IsNullOrEmpty(myGameServerItem.GameTags))
			{
				myGameServerItem.GameTagList.AddRange(myGameServerItem.GameTags.Split(new char[1] { ' ' }));
			}
			IPAddress address = IPAddressExtensions.FromIPv4NetworkOrder(serverItem.m_NetAdr.GetIP());
			ushort connectionPort = serverItem.m_NetAdr.GetConnectionPort();
			IPEndPoint iPEndPoint = new IPEndPoint(address, connectionPort);
			myGameServerItem.ConnectionString = "steam://" + iPEndPoint;
			return myGameServerItem;
		}

		public bool OnInvite(string dataProtocol)
		{
			return false;
		}

		public void RequestFavoritesServerList(MySessionSearchFilter filterOps)
		{
			List<MatchMakingKeyValuePair_t> filters = GetFilters(filterOps);
			m_favoritesRequest = SteamMatchmakingServers.RequestFavoritesServerList(SteamUtils.GetAppID(), filters.ToArray(), (uint)filters.Count, m_favoritesRequestServerResponse);
		}

		public void CancelFavoritesServersRequest()
		{
			if (m_favoritesRequest.HasValue)
			{
				SteamMatchmakingServers.CancelQuery(m_favoritesRequest.Value);
			}
		}

		public MyGameServerItem GetFavoritesServerDetails(int server)
		{
			if (!m_favoritesRequest.HasValue)
			{
				return null;
			}
			return GetGameServer(SteamMatchmakingServers.GetServerDetails(m_favoritesRequest.Value, server));
		}

		public MyGameServerItem GetDedicatedServerDetails(int serverIndex)
		{
			if (!m_dedicatedRequest.HasValue)
			{
				return null;
			}
			return GetGameServer(SteamMatchmakingServers.GetServerDetails(m_dedicatedRequest.Value, serverIndex));
		}

		public void RequestServerItems(string[] connectionStrings, MySessionSearchFilter filter, Action<IEnumerable<MyGameServerItem>> resultCallback)
		{
			throw new NotImplementedException();
		}

		public void RequestInternetServerList(MySessionSearchFilter filter)
		{
			List<MatchMakingKeyValuePair_t> filters = GetFilters(filter);
			m_dedicatedRequest = SteamMatchmakingServers.RequestInternetServerList(SteamUtils.GetAppID(), filters.ToArray(), (uint)filters.Count, m_dedicatedRequestServerResponse);
		}

		public void CancelInternetServersRequest()
		{
			if (m_dedicatedRequest.HasValue)
			{
				SteamMatchmakingServers.CancelQuery(m_dedicatedRequest.Value);
			}
		}

		public MyGameServerItem GetHistoryServerDetails(int server)
		{
			if (!m_historyRequest.HasValue)
			{
				return null;
			}
			return GetGameServer(SteamMatchmakingServers.GetServerDetails(m_historyRequest.Value, server));
		}

		public void RequestHistoryServerList(MySessionSearchFilter filterOps)
		{
			List<MatchMakingKeyValuePair_t> filters = GetFilters(filterOps);
			m_historyRequest = SteamMatchmakingServers.RequestHistoryServerList(SteamUtils.GetAppID(), filters.ToArray(), (uint)filters.Count, m_historyRequestServerResponse);
		}

		public void CancelHistoryServersRequest()
		{
			if (m_historyRequest.HasValue)
			{
				SteamMatchmakingServers.CancelQuery(m_historyRequest.Value);
			}
		}

		public MyGameServerItem GetLANServerDetails(int server)
		{
			if (!m_lanRequest.HasValue)
			{
				return null;
			}
			return GetGameServer(SteamMatchmakingServers.GetServerDetails(m_lanRequest.Value, server));
		}

		public void RequestLANServerList()
		{
			m_lanRequest = SteamMatchmakingServers.RequestLANServerList(SteamUtils.GetAppID(), m_lanRequestServerResponse);
		}

		public void CancelLANServersRequest()
		{
			if (m_lanRequest.HasValue)
			{
				SteamMatchmakingServers.CancelQuery(m_lanRequest.Value);
			}
		}

		public bool IsSteamPrefix(string prefix)
		{
			if (!string.IsNullOrEmpty(prefix))
			{
				return prefix == "steam://";
			}
			return true;
		}

		public void AddFavoriteGame(string connectionString)
		{
			if (IPAddressExtensions.TryParseEndpoint(connectionString, out var prefix, out var result) && IsSteamPrefix(prefix))
			{
				uint rTime32LastPlayedOnServer = DateTime.UtcNow.ToUnixTimestamp();
				SteamMatchmaking.AddFavoriteGame((AppId_t)m_steamNetworking.Service.AppId, result.Address.ToIPv4NetworkOrder(), (ushort)result.Port, (ushort)result.Port, 1u, rTime32LastPlayedOnServer);
			}
		}

		public void AddFavoriteGame(MyGameServerItem serverItem)
		{
			AddFavoriteGame(serverItem.ConnectionString);
		}

		public void RemoveFavoriteGame(MyGameServerItem serverItem)
		{
			if (IPAddressExtensions.TryParseEndpoint(serverItem.ConnectionString, out var prefix, out var result) && IsSteamPrefix(prefix))
			{
				SteamMatchmaking.RemoveFavoriteGame((AppId_t)m_steamNetworking.Service.AppId, result.Address.ToIPv4NetworkOrder(), (ushort)result.Port, (ushort)result.Port, 1u);
			}
		}

		public void AddHistoryGame(MyGameServerItem serverItem)
		{
			if (IPAddressExtensions.TryParseEndpoint(serverItem.ConnectionString, out var prefix, out var result) && IsSteamPrefix(prefix))
			{
				uint rTime32LastPlayedOnServer = DateTime.UtcNow.ToUnixTimestamp();
				SteamMatchmaking.AddFavoriteGame((AppId_t)m_steamNetworking.Service.AppId, result.Address.ToIPv4NetworkOrder(), (ushort)result.Port, (ushort)result.Port, 2u, rTime32LastPlayedOnServer);
			}
		}

		private void OnPingRequestServerResponded(gameserveritem_t server)
		{
			if (this.OnPingServerResponded != null)
			{
				MyGameServerItem gameServer = GetGameServer(server);
				this.OnPingServerResponded(this, gameServer);
			}
		}

		private void OnPingRequestServerFailed()
		{
			this.OnPingServerFailedToRespond?.Invoke(this, null);
		}

		public void PingServer(string connectionString)
		{
			if (!IPAddressExtensions.TryParseEndpoint(connectionString, out var prefix, out var result) || !IsSteamPrefix(prefix))
			{
				OnPingRequestServerFailed();
			}
			else
			{
				SteamMatchmakingServers.PingServer(result.Address.ToIPv4NetworkOrder(), (ushort)result.Port, m_pingServerResponse);
			}
		}

		public void GetServerRules(MyGameServerItem serverItem, ServerRulesResponse completedAction, Action failedAction)
		{
			if (!IPAddressExtensions.TryParseEndpoint(serverItem.ConnectionString, out var prefix, out var result) || !IsSteamPrefix(prefix))
			{
				failedAction();
				return;
			}
			MatchmakingRulesResponse matchmakingRulesResponse = new MatchmakingRulesResponse(completedAction, failedAction);
			lock (m_ruleResponseLockObject)
			{
				m_ruleResponses.Add(matchmakingRulesResponse);
			}
			HServerQuery hServerQuery2 = (matchmakingRulesResponse.Query = SteamMatchmakingServers.ServerRules(result.Address.ToIPv4NetworkOrder(), (ushort)result.Port, matchmakingRulesResponse.RulesResponse));
		}

		public void GetPlayerDetails(MyGameServerItem serverItem, PlayerDetailsResponse completedAction, Action failedAction)
		{
			if (!IPAddressExtensions.TryParseEndpoint(serverItem.ConnectionString, out var prefix, out var result) || !IsSteamPrefix(prefix))
			{
				failedAction();
				return;
			}
			MatchmakingPlayersResponse matchmakingPlayersResponse = new MatchmakingPlayersResponse(completedAction, failedAction);
			lock (m_playerResponseLockObject)
			{
				m_playerResponses.Add(matchmakingPlayersResponse);
			}
			HServerQuery hServerQuery2 = (matchmakingPlayersResponse.Query = SteamMatchmakingServers.PlayerDetails(result.Address.ToIPv4NetworkOrder(), (ushort)result.Port, matchmakingPlayersResponse.RulesResponse));
		}

		private void HandleServerChange(GameServerChangeRequested_t param)
		{
			this.OnServerChangeRequested?.Invoke(param.m_rgchServer, param.m_rgchPassword);
		}

		public bool Connect(MyGameServerItem serverItem, Action<JoinResult> onDone)
		{
			if (!IPAddressExtensions.TryParseEndpoint(serverItem.ConnectionString, out var prefix, out var _) || !IsSteamPrefix(prefix))
			{
				return false;
			}
			MyServiceManager.Instance.AddService((IMyNetworking)m_steamNetworking);
			onDone.InvokeIfNotNull(JoinResult.OK);
			return true;
		}

		public void Disconnect()
		{
		}

		public void Update()
		{
			lock (m_ruleResponseLockObject)
			{
				foreach (MatchmakingRulesResponse item in m_ruleResponses.Where((MatchmakingRulesResponse r) => r.IsCompleted || r.Failed).ToList())
				{
					m_ruleResponses.Remove(item);
				}
			}
			lock (m_playerResponseLockObject)
			{
				foreach (MatchmakingPlayersResponse item2 in m_playerResponses.Where((MatchmakingPlayersResponse r) => r.IsCompleted || r.Failed).ToList())
				{
					m_playerResponses.Remove(item2);
				}
			}
		}
	}
}
