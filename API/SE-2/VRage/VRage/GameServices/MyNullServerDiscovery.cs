using System;
using System.Collections.Generic;
using VRage.Network;

namespace VRage.GameServices
{
	public class MyNullServerDiscovery : IMyServerDiscovery
	{
		public string ServiceName => "null";

		public string ConnectionStringPrefix => "null://";

		public bool LANSupport => false;

		public bool DedicatedSupport => false;

		public bool FavoritesSupport => false;

		public bool HistorySupport => false;

		public bool FriendSupport => false;

		public bool PingSupport => false;

		public bool GroupSupport => false;

		/// <inheritdoc />
		public bool SupportsDirectServerSearch => false;

		/// <inheritdoc />
		public MySupportedPropertyFilters SupportedSearchParameters => MySupportedPropertyFilters.Empty;

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

		public MyGameServerItem GetFavoritesServerDetails(int server)
		{
			return null;
		}

		public bool OnInvite(string dataProtocol)
		{
			return false;
		}

		public void RequestFavoritesServerList(MySessionSearchFilter filterOps)
		{
		}

		public void CancelFavoritesServersRequest()
		{
		}

		public MyGameServerItem GetDedicatedServerDetails(int serverIndex)
		{
			return null;
		}

		/// <inheritdoc />
		public void RequestServerItems(string[] connectionStrings, MySessionSearchFilter filter, Action<IEnumerable<MyGameServerItem>> resultCallback)
		{
		}

		public void RequestInternetServerList(MySessionSearchFilter filter)
		{
		}

		public void CancelInternetServersRequest()
		{
		}

		public MyGameServerItem GetHistoryServerDetails(int server)
		{
			return null;
		}

		public void RequestHistoryServerList(MySessionSearchFilter filterOps)
		{
		}

		public void CancelHistoryServersRequest()
		{
		}

		public MyGameServerItem GetLANServerDetails(int server)
		{
			return null;
		}

		public void RequestLANServerList()
		{
		}

		public void CancelLANServersRequest()
		{
		}

		public void PingServer(string connectionString)
		{
		}

		public void GetServerRules(MyGameServerItem serverItem, ServerRulesResponse completedAction, Action failedAction)
		{
		}

		public void GetPlayerDetails(MyGameServerItem serverItem, PlayerDetailsResponse completedAction, Action failedAction)
		{
		}

		public bool Connect(MyGameServerItem serverItem, Action<JoinResult> onDone)
		{
			onDone.InvokeIfNotNull(JoinResult.OK);
			return true;
		}

		public void Disconnect()
		{
		}

		public void AddFavoriteGame(string connectionString)
		{
		}

		public void AddFavoriteGame(MyGameServerItem serverItem)
		{
		}

		public void RemoveFavoriteGame(MyGameServerItem serverItem)
		{
		}

		public void AddHistoryGame(MyGameServerItem serverItem)
		{
		}

		protected virtual void OnOnLanServerListResponded(int e)
		{
			this.OnLANServerListResponded?.Invoke(this, e);
		}

		protected virtual void OnOnLanServersCompleteResponse(MyMatchMakingServerResponse e)
		{
			this.OnLANServersCompleteResponse?.Invoke(this, e);
		}

		protected virtual void OnOnDedicatedServerListResponded(int e)
		{
			this.OnDedicatedServerListResponded?.Invoke(this, e);
		}

		protected virtual void OnOnDedicatedServersCompleteResponse(MyMatchMakingServerResponse e)
		{
			this.OnDedicatedServersCompleteResponse?.Invoke(this, e);
		}

		protected virtual void OnOnFavoritesServerListResponded(int e)
		{
			this.OnFavoritesServerListResponded?.Invoke(this, e);
		}

		protected virtual void OnOnFavoritesServersCompleteResponse(MyMatchMakingServerResponse e)
		{
			this.OnFavoritesServersCompleteResponse?.Invoke(this, e);
		}

		protected virtual void OnOnHistoryServerListResponded(int e)
		{
			this.OnHistoryServerListResponded?.Invoke(this, e);
		}

		protected virtual void OnOnHistoryServersCompleteResponse(MyMatchMakingServerResponse e)
		{
			this.OnHistoryServersCompleteResponse?.Invoke(this, e);
		}

		protected virtual void OnOnPingServerResponded(MyGameServerItem e)
		{
			this.OnPingServerResponded?.Invoke(this, e);
		}

		protected virtual void OnOnPingServerFailedToRespond()
		{
			this.OnPingServerFailedToRespond?.Invoke(this, EventArgs.Empty);
		}

		protected virtual void OnOnServerChangeRequested(string server, string password)
		{
			this.OnServerChangeRequested?.Invoke(server, password);
		}
	}
}
