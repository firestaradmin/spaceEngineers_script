using System;
using System.Collections.Generic;
using Steamworks;
using VRage.GameServices;

namespace VRage.Steam
{
	internal class MatchmakingPlayersResponse
	{
		private PlayerDetailsResponse m_completedAction;

		private Action m_failedAction;

		private Dictionary<string, float> m_players;

		public ISteamMatchmakingPlayersResponse RulesResponse { get; set; }

		public HServerQuery Query { get; set; }

		public bool Failed { get; set; }

		public bool IsCompleted { get; set; }

		public MatchmakingPlayersResponse(PlayerDetailsResponse completedAction, Action failedAction)
		{
			m_completedAction = completedAction;
			m_failedAction = failedAction;
			m_players = new Dictionary<string, float>();
			RulesResponse = new ISteamMatchmakingPlayersResponse(OnAddPlayerToList, OnPlayersFailedToRespond, onPlayersRefreshComplete);
		}

		private void onPlayersRefreshComplete()
		{
			m_completedAction(m_players);
			m_completedAction = null;
			m_failedAction = null;
			RulesResponse = null;
			IsCompleted = true;
		}

		private void OnPlayersFailedToRespond()
		{
			m_failedAction();
			Failed = true;
		}

		private void OnAddPlayerToList(string pchName, int nScore, float flTimePlayed)
		{
			m_players[pchName] = flTimePlayed;
		}
	}
}
