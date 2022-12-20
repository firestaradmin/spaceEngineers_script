using System.Collections.Generic;
using System.Threading;
using Sandbox.Game.World;

namespace Sandbox.Game.Entities.Cube
{
	internal class MyCubeGridOwnershipManager
	{
		private MyCubeGrid m_grid;

		public Dictionary<long, int> PlayerOwnedBlocks;

		public Dictionary<long, int> PlayerOwnedValidBlocks;

		public List<long> BigOwners;

		public List<long> SmallOwners;

		public int MaxBlocks;

		public long gridEntityId;

		public bool m_needRecalculateOwners;

		public bool NeedRecalculateOwners
		{
			get
			{
				return m_needRecalculateOwners;
			}
			set
			{
				if (value != m_needRecalculateOwners)
				{
					m_needRecalculateOwners = value;
					if (value)
					{
						m_grid.Schedule(MyCubeGrid.UpdateQueue.OnceAfterSimulation, RecalculateOwnersThreadSafe, 20);
					}
				}
			}
		}

		private bool IsValidBlock(MyCubeBlock block)
		{
			return block.IsFunctional;
		}

		public void Init(MyCubeGrid grid)
		{
			m_grid = grid;
			PlayerOwnedBlocks = new Dictionary<long, int>();
			PlayerOwnedValidBlocks = new Dictionary<long, int>();
			BigOwners = new List<long>();
			SmallOwners = new List<long>();
			MaxBlocks = 0;
			gridEntityId = grid.EntityId;
			foreach (MyCubeBlock fatBlock in grid.GetFatBlocks())
			{
				long ownerId = fatBlock.OwnerId;
				if (ownerId == 0L)
				{
					continue;
				}
				if (!PlayerOwnedBlocks.ContainsKey(ownerId))
				{
					PlayerOwnedBlocks.Add(ownerId, 0);
				}
				PlayerOwnedBlocks[ownerId]++;
				if (IsValidBlock(fatBlock))
				{
					if (!PlayerOwnedValidBlocks.ContainsKey(ownerId))
					{
						PlayerOwnedValidBlocks.Add(ownerId, 0);
					}
					if (++PlayerOwnedValidBlocks[fatBlock.OwnerId] > MaxBlocks)
					{
						MaxBlocks = PlayerOwnedValidBlocks[ownerId];
					}
				}
			}
			NeedRecalculateOwners = true;
		}

		internal void RecalculateOwnersThreadSafe()
		{
<<<<<<< HEAD
			if (MyEntities.IsAsyncUpdateInProgress || Thread.CurrentThread == MySandboxGame.Static.UpdateThread)
=======
			if (MyEntities.IsAsyncUpdateInProgress || Thread.get_CurrentThread() == MySandboxGame.Static.UpdateThread)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (MyEntities.IsAsyncUpdateInProgress)
				{
					RecalculateOwnersInternal(updatePlayerGrids: false);
					MyEntities.InvokeLater(UpdatePlayerGrids);
				}
				else
				{
					RecalculateOwnersInternal();
				}
			}
		}

		private void RecalculateOwnersInternal(bool updatePlayerGrids = true)
		{
			NeedRecalculateOwners = false;
			MaxBlocks = 0;
			foreach (long key in PlayerOwnedValidBlocks.Keys)
			{
				if (PlayerOwnedValidBlocks[key] > MaxBlocks)
				{
					MaxBlocks = PlayerOwnedValidBlocks[key];
				}
			}
			BigOwners.Clear();
			foreach (long key2 in PlayerOwnedValidBlocks.Keys)
			{
				if (PlayerOwnedValidBlocks[key2] == MaxBlocks)
				{
					BigOwners.Add(key2);
				}
			}
			if (updatePlayerGrids)
			{
				UpdatePlayerGrids();
			}
			m_grid.NotifyBlockOwnershipChange();
		}

		private void UpdatePlayerGrids()
		{
			if (SmallOwners.Contains(MySession.Static.LocalPlayerId))
			{
				MySession.Static.LocalHumanPlayer.RemoveGrid(gridEntityId);
			}
			SmallOwners.Clear();
			foreach (long key in PlayerOwnedBlocks.Keys)
			{
				SmallOwners.Add(key);
				if (key == MySession.Static.LocalPlayerId)
				{
					MySession.Static.LocalHumanPlayer.AddGrid(gridEntityId);
				}
			}
		}

		public void ChangeBlockOwnership(MyCubeBlock block, long oldOwner, long newOwner)
		{
			DecreaseValue(ref PlayerOwnedBlocks, oldOwner);
			IncreaseValue(ref PlayerOwnedBlocks, newOwner);
			if (IsValidBlock(block))
			{
				DecreaseValue(ref PlayerOwnedValidBlocks, oldOwner);
				IncreaseValue(ref PlayerOwnedValidBlocks, newOwner);
			}
			NeedRecalculateOwners = true;
		}

		public void UpdateOnFunctionalChange(long ownerId, bool newFunctionalValue)
		{
			if (!newFunctionalValue)
			{
				DecreaseValue(ref PlayerOwnedValidBlocks, ownerId);
			}
			else
			{
				IncreaseValue(ref PlayerOwnedValidBlocks, ownerId);
			}
			NeedRecalculateOwners = true;
		}

		public void IncreaseValue(ref Dictionary<long, int> dict, long key)
		{
			if (key != 0L)
			{
				if (!dict.ContainsKey(key))
				{
					dict.Add(key, 0);
				}
				dict[key]++;
			}
		}

		public void DecreaseValue(ref Dictionary<long, int> dict, long key)
		{
			if (key != 0L && dict.ContainsKey(key))
			{
				dict[key]--;
				if (dict[key] == 0)
				{
					dict.Remove(key);
				}
			}
		}
	}
}
