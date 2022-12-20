using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 887)]
	public class MySessionComponentTeamBalancer : MySessionComponentBase
	{
		private bool m_initialized;

		private bool m_enabled;

		private Dictionary<long, MyFaction> m_factions = new Dictionary<long, MyFaction>();

		public bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				if (m_enabled != value && Sync.IsServer)
				{
					m_enabled = value;
					if (m_enabled && !m_initialized)
					{
						Initialize();
					}
				}
			}
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			if (MySession.Static.Settings.EnableTeamBalancing && Sync.IsServer)
			{
				Enabled = true;
			}
		}

		private void Initialize()
		{
			m_initialized = true;
			m_factions.Clear();
			GatherFactions();
			MyVisualScriptLogicProvider.PlayerSpawned = (SingleKeyPlayerEvent)Delegate.Combine(MyVisualScriptLogicProvider.PlayerSpawned, new SingleKeyPlayerEvent(OnPlayerSpawned));
		}

		protected override void UnloadData()
		{
			MyVisualScriptLogicProvider.PlayerSpawned = (SingleKeyPlayerEvent)Delegate.Remove(MyVisualScriptLogicProvider.PlayerSpawned, new SingleKeyPlayerEvent(OnPlayerSpawned));
		}

		private void OnPlayerSpawned(long playerId)
		{
			if (MySession.Static.Factions.TryGetPlayerFaction(playerId) != null)
			{
				return;
			}
			MyFaction emptiestFaction = GetEmptiestFaction();
			if (emptiestFaction != null)
			{
				MyFactionCollection.SendJoinRequest(emptiestFaction.FactionId, playerId);
				if (MyVisualScriptLogicProvider.TeamBalancerPlayerSorted != null)
				{
					MyVisualScriptLogicProvider.TeamBalancerPlayerSorted(playerId, emptiestFaction.Tag);
				}
			}
		}

		private MyFaction GetEmptiestFaction()
		{
			MyFaction result = null;
			int num = int.MaxValue;
			foreach (KeyValuePair<long, MyFaction> faction in m_factions)
			{
				if (faction.Value.Members.Count < num)
				{
					num = faction.Value.Members.Count;
					result = faction.Value;
				}
			}
			return result;
		}

		private void GatherFactions()
		{
			MyFactionCollection factions = MySession.Static.Factions;
			foreach (KeyValuePair<long, IMyFaction> faction in factions.Factions)
			{
				MyFaction myFaction = faction.Value as MyFaction;
				if (myFaction != null && !factions.IsNpcFaction(myFaction.FactionId) && MyDefinitionManager.Static.TryGetFactionDefinition(myFaction.Tag) == null && (myFaction.FactionType == MyFactionTypes.PlayerMade || myFaction.FactionType == MyFactionTypes.None))
				{
					myFaction.AutoAcceptMember = true;
					m_factions.Add(myFaction.FactionId, myFaction);
				}
			}
		}
	}
}
