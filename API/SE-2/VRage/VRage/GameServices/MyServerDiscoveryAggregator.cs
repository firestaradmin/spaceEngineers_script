using System;
using System.Collections.Generic;
using VRage.Network;

namespace VRage.GameServices
{
	public class MyServerDiscoveryAggregator : IMyServerDiscovery
	{
		private const int MAX_INDEX = 65536;

		private readonly List<IMyServerDiscovery> m_serverDiscoveryList = new List<IMyServerDiscovery>();

		private int m_lanResponseCount;

		private MyMatchMakingServerResponse m_lanResponse;

		private int m_internetResponseCount;

		private MyMatchMakingServerResponse m_internetResponse;

		private int m_favoritesResponseCount;

		private MyMatchMakingServerResponse m_favoritesResponse;

		private int m_historyResponseCount;

		private MyMatchMakingServerResponse m_historyResponse;

		public string ServiceName => "null";

		public string ConnectionStringPrefix => "null://";

		/// LAN
		public bool LANSupport => !m_serverDiscoveryList.TrueForAll((IMyServerDiscovery x) => !x.LANSupport);

		/// INTERNET
		public bool DedicatedSupport => !m_serverDiscoveryList.TrueForAll((IMyServerDiscovery x) => !x.DedicatedSupport);

		/// <inheritdoc />
		public bool SupportsDirectServerSearch => m_serverDiscoveryList.TrueForAll((IMyServerDiscovery x) => x.SupportsDirectServerSearch);

		/// <inheritdoc />
		public MySupportedPropertyFilters SupportedSearchParameters => MySupportedPropertyFilters.Empty;

		/// FAVORITES
		public bool FavoritesSupport => !m_serverDiscoveryList.TrueForAll((IMyServerDiscovery x) => !x.FavoritesSupport);

		/// HISTORY
		public bool HistorySupport => !m_serverDiscoveryList.TrueForAll((IMyServerDiscovery x) => !x.HistorySupport);

		/// PING
		public bool PingSupport => !m_serverDiscoveryList.TrueForAll((IMyServerDiscovery x) => !x.PingSupport);

		/// OTHER
		public bool FriendSupport => !m_serverDiscoveryList.TrueForAll((IMyServerDiscovery x) => !x.FriendSupport);

		public bool GroupSupport => !m_serverDiscoveryList.TrueForAll((IMyServerDiscovery x) => !x.GroupSupport);

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

		/// INVITES
		public event MyServerChangeRequested OnServerChangeRequested;

		public void AddAggregate(IMyServerDiscovery aggregate)
		{
			aggregate.OnLANServerListResponded += OnLanServerList;
			aggregate.OnLANServersCompleteResponse += OnLanServersComplete;
			aggregate.OnDedicatedServerListResponded += OnDedicatedServerList;
			aggregate.OnDedicatedServersCompleteResponse += OnDedicatedServersComplete;
			aggregate.OnFavoritesServerListResponded += OnFavoritesServerList;
			aggregate.OnFavoritesServersCompleteResponse += OnFavoritesServersComplete;
			aggregate.OnHistoryServerListResponded += OnHistoryServerList;
			aggregate.OnHistoryServersCompleteResponse += OnHistoryServersComplete;
			aggregate.OnPingServerResponded += OnPingServer;
			aggregate.OnPingServerFailedToRespond += OnPingServerFailed;
			aggregate.OnServerChangeRequested += this.OnServerChangeRequested;
			m_serverDiscoveryList.Add(aggregate);
		}

		private int LocalToGlobalIndex(IMyServerDiscovery aggregate, int idx)
		{
			return idx + m_serverDiscoveryList.FindIndex((IMyServerDiscovery x) => x == aggregate) * 65536;
		}

		private int GlobalToLocalIndex(int idx)
		{
			return idx % 65536;
		}

		private IMyServerDiscovery GlobalToAggregate(int idx)
		{
			return m_serverDiscoveryList[idx / 65536];
		}

		private int GetProviderIndex(string connectionString)
		{
			int result = 0;
			for (int i = 0; i < m_serverDiscoveryList.Count; i++)
			{
				if (connectionString.StartsWith(m_serverDiscoveryList[i].ConnectionStringPrefix))
				{
					result = i;
					break;
				}
			}
			return result;
		}

		public IMyServerDiscovery FindAggregate(string connectionString)
		{
			return m_serverDiscoveryList[GetProviderIndex(connectionString)];
		}

		public MyGameServerItem GetLANServerDetails(int server)
		{
			return GlobalToAggregate(server).GetLANServerDetails(GlobalToLocalIndex(server));
		}

		public void RequestLANServerList()
		{
			m_lanResponseCount = 0;
			m_lanResponse = MyMatchMakingServerResponse.NoServersListedOnMasterServer;
			m_serverDiscoveryList.ForEach(delegate(IMyServerDiscovery x)
			{
				x.RequestLANServerList();
			});
		}

		public void CancelLANServersRequest()
		{
			m_serverDiscoveryList.ForEach(delegate(IMyServerDiscovery x)
			{
				x.CancelLANServersRequest();
			});
		}

		private void OnLanServerList(object sender, int idx)
		{
			this.OnLANServerListResponded?.Invoke(sender, LocalToGlobalIndex((IMyServerDiscovery)sender, idx));
		}

		private void OnLanServersComplete(object sender, MyMatchMakingServerResponse response)
		{
			m_lanResponseCount++;
			if (response < m_lanResponse)
			{
				m_lanResponse = response;
			}
			if (m_lanResponseCount == m_serverDiscoveryList.Count)
			{
				this.OnLANServersCompleteResponse?.Invoke(this, m_lanResponse);
			}
		}

		public MyGameServerItem GetDedicatedServerDetails(int serverIndex)
		{
			return GlobalToAggregate(serverIndex).GetDedicatedServerDetails(GlobalToLocalIndex(serverIndex));
		}

		/// <inheritdoc />
		public void RequestServerItems(string[] connectionStrings, MySessionSearchFilter filter, Action<IEnumerable<MyGameServerItem>> resultCallback)
		{
			throw new NotImplementedException();
		}

		public void RequestInternetServerList(MySessionSearchFilter filter)
		{
			m_internetResponseCount = 0;
			m_internetResponse = MyMatchMakingServerResponse.NoServersListedOnMasterServer;
			m_serverDiscoveryList.ForEach(delegate(IMyServerDiscovery x)
			{
				x.RequestInternetServerList(filter);
			});
		}

		public void CancelInternetServersRequest()
		{
			m_serverDiscoveryList.ForEach(delegate(IMyServerDiscovery x)
			{
				x.CancelInternetServersRequest();
			});
		}

		private void OnDedicatedServerList(object sender, int idx)
		{
			this.OnDedicatedServerListResponded?.Invoke(sender, LocalToGlobalIndex((IMyServerDiscovery)sender, idx));
		}

		private void OnDedicatedServersComplete(object sender, MyMatchMakingServerResponse response)
		{
			m_internetResponseCount++;
			if (response < m_internetResponse)
			{
				m_internetResponse = response;
			}
			if (m_internetResponseCount == m_serverDiscoveryList.Count)
			{
				this.OnDedicatedServersCompleteResponse?.Invoke(this, response);
			}
		}

		public MyGameServerItem GetFavoritesServerDetails(int server)
		{
			return GlobalToAggregate(server).GetFavoritesServerDetails(GlobalToLocalIndex(server));
		}

		public void RequestFavoritesServerList(MySessionSearchFilter filterOps)
		{
			m_favoritesResponseCount = 0;
			m_favoritesResponse = MyMatchMakingServerResponse.NoServersListedOnMasterServer;
			m_serverDiscoveryList.ForEach(delegate(IMyServerDiscovery x)
			{
				x.RequestFavoritesServerList(filterOps);
			});
		}

		public void CancelFavoritesServersRequest()
		{
			m_serverDiscoveryList.ForEach(delegate(IMyServerDiscovery x)
			{
				x.CancelFavoritesServersRequest();
			});
		}

		private void OnFavoritesServerList(object sender, int idx)
		{
			this.OnFavoritesServerListResponded?.Invoke(sender, LocalToGlobalIndex((IMyServerDiscovery)sender, idx));
		}

		private void OnFavoritesServersComplete(object sender, MyMatchMakingServerResponse response)
		{
			m_favoritesResponseCount++;
			if (response < m_favoritesResponse)
			{
				m_favoritesResponse = response;
			}
			if (m_favoritesResponseCount == m_serverDiscoveryList.Count)
			{
				this.OnFavoritesServersCompleteResponse?.Invoke(this, response);
			}
		}

		public void AddFavoriteGame(string connectionString)
		{
			m_serverDiscoveryList[GetProviderIndex(connectionString)].AddFavoriteGame(connectionString);
		}

		public void AddFavoriteGame(MyGameServerItem serverItem)
		{
			m_serverDiscoveryList[GetProviderIndex(serverItem.ConnectionString)].AddFavoriteGame(serverItem);
		}

		public void RemoveFavoriteGame(MyGameServerItem serverItem)
		{
			m_serverDiscoveryList[GetProviderIndex(serverItem.ConnectionString)].RemoveFavoriteGame(serverItem);
		}

		public MyGameServerItem GetHistoryServerDetails(int server)
		{
			return GlobalToAggregate(server).GetHistoryServerDetails(GlobalToLocalIndex(server));
		}

		public void RequestHistoryServerList(MySessionSearchFilter filterOps)
		{
			m_historyResponseCount = 0;
			m_historyResponse = MyMatchMakingServerResponse.NoServersListedOnMasterServer;
			m_serverDiscoveryList.ForEach(delegate(IMyServerDiscovery x)
			{
				x.RequestHistoryServerList(filterOps);
			});
		}

		public void CancelHistoryServersRequest()
		{
			m_serverDiscoveryList.ForEach(delegate(IMyServerDiscovery x)
			{
				x.CancelHistoryServersRequest();
			});
		}

		private void OnHistoryServerList(object sender, int idx)
		{
			this.OnHistoryServerListResponded?.Invoke(sender, LocalToGlobalIndex((IMyServerDiscovery)sender, idx));
		}

		private void OnHistoryServersComplete(object sender, MyMatchMakingServerResponse response)
		{
			m_historyResponseCount++;
			if (response < m_historyResponse)
			{
				m_historyResponse = response;
			}
			if (m_historyResponseCount == m_serverDiscoveryList.Count)
			{
				this.OnHistoryServersCompleteResponse?.Invoke(this, response);
			}
		}

		public void AddHistoryGame(MyGameServerItem serverItem)
		{
			m_serverDiscoveryList[GetProviderIndex(serverItem.ConnectionString)].AddHistoryGame(serverItem);
		}

		public void PingServer(string connectionString)
		{
			m_serverDiscoveryList[GetProviderIndex(connectionString)].PingServer(connectionString);
		}

		private void OnPingServer(object sender, MyGameServerItem e)
		{
			this.OnPingServerResponded?.Invoke(sender, e);
		}

		private void OnPingServerFailed(object sender, EventArgs e)
		{
			this.OnPingServerFailedToRespond?.Invoke(this, EventArgs.Empty);
		}

		public bool OnInvite(string dataProtocol)
		{
			bool ret = false;
			m_serverDiscoveryList.ForEach(delegate(IMyServerDiscovery x)
			{
				ret |= x.OnInvite(dataProtocol);
			});
			return ret;
		}

		/// SERVER INFO
		public void GetServerRules(MyGameServerItem serverItem, ServerRulesResponse completedAction, Action failedAction)
		{
			m_serverDiscoveryList[GetProviderIndex(serverItem.ConnectionString)].GetServerRules(serverItem, delegate(Dictionary<string, string> y)
			{
				completedAction(y);
			}, delegate
			{
				failedAction();
			});
		}

		public void GetPlayerDetails(MyGameServerItem serverItem, PlayerDetailsResponse completedAction, Action failedAction)
		{
			int rulesCounter = 0;
			bool completed = false;
			m_serverDiscoveryList[GetProviderIndex(serverItem.ConnectionString)].GetPlayerDetails(serverItem, delegate(Dictionary<string, float> y)
			{
				rulesCounter++;
				completed = true;
				completedAction(y);
			}, delegate
			{
				rulesCounter++;
				if (rulesCounter == m_serverDiscoveryList.Count && !completed)
				{
					failedAction();
				}
			});
		}

		/// CONNECTION
		public bool Connect(MyGameServerItem serverItem, Action<JoinResult> onDone)
		{
			return m_serverDiscoveryList[GetProviderIndex(serverItem.ConnectionString)].Connect(serverItem, onDone);
		}

		public void Disconnect()
		{
			m_serverDiscoveryList.ForEach(delegate(IMyServerDiscovery x)
			{
				x.Disconnect();
			});
		}

		public List<IMyServerDiscovery> GetAggregates()
		{
			return m_serverDiscoveryList;
		}
	}
}
