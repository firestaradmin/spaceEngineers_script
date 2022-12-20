using System;
using System.Collections.Generic;
using Steamworks;
using VRage.GameServices;

namespace VRage.Steam
{
	internal class MatchmakingRulesResponse
	{
		private ServerRulesResponse m_completedAction;

		private Action m_failedAction;

		private Dictionary<string, string> m_rules;

		public ISteamMatchmakingRulesResponse RulesResponse { get; set; }

		public HServerQuery Query { get; set; }

		public bool Failed { get; set; }

		public bool IsCompleted { get; set; }

		public MatchmakingRulesResponse(ServerRulesResponse completedAction, Action failedAction)
		{
			m_completedAction = completedAction;
			m_failedAction = failedAction;
			m_rules = new Dictionary<string, string>();
			RulesResponse = new ISteamMatchmakingRulesResponse(OnRulesResponded, OnRulesFailedToRespond, OnRulesRefreshComplete);
		}

		private void OnRulesRefreshComplete()
		{
			m_completedAction(m_rules);
			m_rules.Clear();
			m_completedAction = null;
			m_failedAction = null;
			RulesResponse = null;
			IsCompleted = true;
		}

		private void OnRulesFailedToRespond()
		{
			m_failedAction();
			Failed = true;
		}

		private void OnRulesResponded(string pchRule, string pchValue)
		{
			m_rules.Add(pchRule, pchValue);
		}
	}
}
