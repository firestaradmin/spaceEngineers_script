using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage.Game.Entities;
using VRage.Game.ModAPI;

namespace Sandbox.Game.GameSystems
{
	public class MyTieredUpdateSystem : MyUpdateableGridSystem
	{
		private readonly int TIER2_PLAYER_PRESENCE_TIME_FRAMES = 36000;

		private ConcurrentDictionary<long, IMyTieredUpdateBlock> m_blocks = new ConcurrentDictionary<long, IMyTieredUpdateBlock>();

		private bool m_isReplicated;

		private bool? m_isGridPresent;

		private int m_playerPresenceTierTimer;

		public override MyCubeGrid.UpdateQueue Queue => MyCubeGrid.UpdateQueue.OnceBeforeSimulation;

		public MyTieredUpdateSystem(MyCubeGrid grid)
			: base(grid)
		{
			MyCubeGrid grid2 = base.Grid;
			grid2.ReplicationStarted = (Action)Delegate.Combine(grid2.ReplicationStarted, new Action(ReplicationStarted));
			MyCubeGrid grid3 = base.Grid;
			grid3.ReplicationEnded = (Action)Delegate.Combine(grid3.ReplicationEnded, new Action(ReplicationEnded));
			MyCubeGrid grid4 = base.Grid;
			grid4.GridPresenceUpdate = (Action<bool>)Delegate.Combine(grid4.GridPresenceUpdate, new Action<bool>(GridPresenceUpdate));
			Schedule();
		}

		private void GridPresenceUpdate(bool isAnyGridPresent)
		{
			if (Sync.IsServer && (!m_isGridPresent.HasValue || isAnyGridPresent != m_isGridPresent))
			{
				if (isAnyGridPresent)
				{
					m_isGridPresent = true;
					ChangeGridTier(MyUpdateTiersGridPresence.Normal);
				}
				else
				{
					m_isGridPresent = false;
					ChangeGridTier(MyUpdateTiersGridPresence.Tier1);
				}
			}
		}

		private void ChangeGridTier(MyUpdateTiersGridPresence newTier)
		{
			if (!Sync.IsServer || base.Grid.GridPresenceTier == newTier)
			{
				return;
			}
			base.Grid.GridPresenceTier = newTier;
<<<<<<< HEAD
			foreach (IMyTieredUpdateBlock value in m_blocks.Values)
=======
			foreach (IMyTieredUpdateBlock value in m_blocks.get_Values())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				value.ChangeTier();
			}
		}

		private void ReplicationEnded()
		{
			ChangePlayerTier(MyUpdateTiersPlayerPresence.Tier1);
			m_playerPresenceTierTimer = TIER2_PLAYER_PRESENCE_TIME_FRAMES;
		}

		private void ReplicationStarted()
		{
			ChangePlayerTier(MyUpdateTiersPlayerPresence.Normal);
		}

		private void ChangePlayerTier(MyUpdateTiersPlayerPresence newTier)
		{
			if (base.Grid.PlayerPresenceTier == newTier)
			{
				return;
			}
			base.Grid.PlayerPresenceTier = newTier;
<<<<<<< HEAD
			foreach (IMyTieredUpdateBlock value in m_blocks.Values)
=======
			foreach (IMyTieredUpdateBlock value in m_blocks.get_Values())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				value.ChangeTier();
			}
		}

		internal void Register(IMyTieredUpdateBlock tieredBlock, long id)
		{
			if (!m_blocks.ContainsKey(id))
			{
				m_blocks.TryAdd(id, tieredBlock);
<<<<<<< HEAD
				if (m_blocks.Count == 1)
=======
				if (m_blocks.get_Count() == 1)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MySession.Static.GetComponent<MySessionComponentSmartUpdater>()?.RegisterToUpdater(base.Grid);
				}
				tieredBlock.ChangeTier();
			}
		}

		internal void Unregister(IMyTieredUpdateBlock tieredBlock, long id)
		{
			if (m_blocks.ContainsKey(id))
			{
<<<<<<< HEAD
				m_blocks.Remove(id);
				if (m_blocks.Count == 0)
=======
				m_blocks.Remove<long, IMyTieredUpdateBlock>(id);
				if (m_blocks.get_Count() == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					MySession.Static.GetComponent<MySessionComponentSmartUpdater>()?.UnregisterFromUpdater(base.Grid);
				}
			}
		}

<<<<<<< HEAD
		/// <summary>
		/// Called OnceBeforeSimulation
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		protected override void Update()
		{
			m_playerPresenceTierTimer = 0;
			if (base.Grid.PlayerPresenceTier == MyUpdateTiersPlayerPresence.Normal && MySession.Static.Players.GetOnlinePlayerCount() == 0)
			{
				ChangePlayerTier(MyUpdateTiersPlayerPresence.Tier1);
				m_playerPresenceTierTimer = TIER2_PLAYER_PRESENCE_TIME_FRAMES;
			}
		}

		internal void UpdateAfterSimulation100()
		{
			if (base.Grid.PlayerPresenceTier == MyUpdateTiersPlayerPresence.Tier1 && m_playerPresenceTierTimer > 0)
			{
				m_playerPresenceTierTimer -= 100;
				if (m_playerPresenceTierTimer <= 0)
				{
					ChangePlayerTier(MyUpdateTiersPlayerPresence.Tier2);
				}
			}
		}
	}
}
