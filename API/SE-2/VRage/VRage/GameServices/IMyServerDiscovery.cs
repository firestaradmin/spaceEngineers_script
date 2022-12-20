using System;
using System.Collections.Generic;
using VRage.Network;

namespace VRage.GameServices
{
	public interface IMyServerDiscovery
	{
		string ServiceName { get; }

		string ConnectionStringPrefix { get; }

		bool LANSupport { get; }

		bool DedicatedSupport { get; }

		bool FavoritesSupport { get; }

		bool HistorySupport { get; }

		bool FriendSupport { get; }

		bool PingSupport { get; }

		bool GroupSupport { get; }

		bool SupportsDirectServerSearch { get; }

		/// <summary>
		/// Returns a set of supported search queries.
		/// </summary>
		MySupportedPropertyFilters SupportedSearchParameters { get; }

		event EventHandler<int> OnLANServerListResponded;

		event EventHandler<MyMatchMakingServerResponse> OnLANServersCompleteResponse;

		event EventHandler<int> OnDedicatedServerListResponded;

		event EventHandler<MyMatchMakingServerResponse> OnDedicatedServersCompleteResponse;

		event EventHandler<int> OnFavoritesServerListResponded;

		event EventHandler<MyMatchMakingServerResponse> OnFavoritesServersCompleteResponse;

		event EventHandler<int> OnHistoryServerListResponded;

		event EventHandler<MyMatchMakingServerResponse> OnHistoryServersCompleteResponse;

		event EventHandler<MyGameServerItem> OnPingServerResponded;

		event EventHandler OnPingServerFailedToRespond;

		event MyServerChangeRequested OnServerChangeRequested;

		bool OnInvite(string dataProtocol);

		void RequestFavoritesServerList(MySessionSearchFilter filterOps);

		void CancelFavoritesServersRequest();

		MyGameServerItem GetFavoritesServerDetails(int server);

		MyGameServerItem GetDedicatedServerDetails(int serverIndex);

		/// <summary>
		/// Query the game servers for a set of connection strings.
		/// </summary>
		/// <param name="connectionStrings"></param>
		/// <param name="filter"></param>
		/// <param name="resultCallback"></param>
		void RequestServerItems(string[] connectionStrings, MySessionSearchFilter filter, Action<IEnumerable<MyGameServerItem>> resultCallback);

		void RequestInternetServerList(MySessionSearchFilter filter);

		void CancelInternetServersRequest();

		MyGameServerItem GetHistoryServerDetails(int server);

		void RequestHistoryServerList(MySessionSearchFilter filterOps);

		void CancelHistoryServersRequest();

		MyGameServerItem GetLANServerDetails(int server);

		void RequestLANServerList();

		void CancelLANServersRequest();

		void AddFavoriteGame(MyGameServerItem serverItem);

		void AddFavoriteGame(string connectionString);

		void RemoveFavoriteGame(MyGameServerItem serverItem);

		void AddHistoryGame(MyGameServerItem serverItem);

		void PingServer(string connectionString);

		void GetServerRules(MyGameServerItem serverItem, ServerRulesResponse completedAction, Action failedAction);

		void GetPlayerDetails(MyGameServerItem serverItem, PlayerDetailsResponse completedAction, Action failedAction);

		bool Connect(MyGameServerItem serverItem, Action<JoinResult> onDone);

		void Disconnect();
	}
}
