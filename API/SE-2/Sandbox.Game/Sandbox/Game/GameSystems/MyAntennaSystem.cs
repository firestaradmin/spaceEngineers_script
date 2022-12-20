<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Groups;

namespace Sandbox.Game.GameSystems
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate, 588, typeof(MyObjectBuilder_AntennaSessionComponent), null, false)]
	public class MyAntennaSystem : MySessionComponentBase
	{
		public struct BroadcasterInfo
		{
			public long EntityId;

			public string Name;
		}

		public class BroadcasterInfoComparer : IEqualityComparer<BroadcasterInfo>
		{
			public bool Equals(BroadcasterInfo x, BroadcasterInfo y)
			{
				if (x.EntityId == y.EntityId)
				{
					return string.Equals(x.Name, y.Name);
				}
				return false;
			}

			public int GetHashCode(BroadcasterInfo obj)
			{
				int num = obj.EntityId.GetHashCode();
				if (obj.Name != null)
				{
					num = (num * 397) ^ obj.Name.GetHashCode();
				}
				return num;
			}
		}

		private static MyAntennaSystem m_static;

		private List<long> m_addedItems = new List<long>();

		private HashSet<BroadcasterInfo> m_output = new HashSet<BroadcasterInfo>((IEqualityComparer<BroadcasterInfo>)new BroadcasterInfoComparer());

		private HashSet<MyDataBroadcaster> m_tempPlayerRelayedBroadcasters = new HashSet<MyDataBroadcaster>();

		private List<MyDataBroadcaster> m_tempGridBroadcastersFromPlayer = new List<MyDataBroadcaster>();

		private HashSet<MyDataReceiver> m_tmpReceivers = new HashSet<MyDataReceiver>();

		private HashSet<MyDataBroadcaster> m_tmpBroadcasters = new HashSet<MyDataBroadcaster>();

		private HashSet<MyDataBroadcaster> m_tmpRelayedBroadcasters = new HashSet<MyDataBroadcaster>();

		private Dictionary<long, MyLaserBroadcaster> m_laserAntennas = new Dictionary<long, MyLaserBroadcaster>();

		private Dictionary<long, MyProxyAntenna> m_proxyAntennas = new Dictionary<long, MyProxyAntenna>();

		private Dictionary<long, HashSet<MyDataBroadcaster>> m_proxyGrids = new Dictionary<long, HashSet<MyDataBroadcaster>>();

		private HashSet<long> m_nearbyBroadcastersSearchExceptions = new HashSet<long>();

		public static MyAntennaSystem Static => m_static;

		public Dictionary<long, MyLaserBroadcaster> LaserAntennas => m_laserAntennas;

		public override void LoadData()
		{
			m_static = this;
			base.LoadData();
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			m_addedItems.Clear();
			m_addedItems = null;
			m_output.Clear();
			m_output = null;
			m_tempGridBroadcastersFromPlayer.Clear();
			m_tempGridBroadcastersFromPlayer = null;
			m_tempPlayerRelayedBroadcasters.Clear();
			m_tempPlayerRelayedBroadcasters = null;
			m_static = null;
		}

		public HashSet<BroadcasterInfo> GetConnectedGridsInfo(MyEntity interactedEntityRepresentative, MyPlayer player = null, bool mutual = true, bool accessible = false)
		{
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			m_output.Clear();
			if (player == null)
			{
				player = MySession.Static.LocalHumanPlayer;
				if (player == null)
				{
					return m_output;
				}
			}
			MyIdentity identity = player.Identity;
			m_tmpReceivers.Clear();
			m_tmpRelayedBroadcasters.Clear();
			if (interactedEntityRepresentative == null)
			{
				return m_output;
			}
			m_output.Add(new BroadcasterInfo
			{
				EntityId = interactedEntityRepresentative.EntityId,
				Name = interactedEntityRepresentative.DisplayName
			});
			GetAllRelayedBroadcasters(interactedEntityRepresentative, identity.IdentityId, mutual, m_tmpRelayedBroadcasters);
			Enumerator<MyDataBroadcaster> enumerator = m_tmpRelayedBroadcasters.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyDataBroadcaster current = enumerator.get_Current();
					if (!accessible || current.CanBeUsedByPlayer(identity.IdentityId))
					{
						m_output.Add(current.Info);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return m_output;
		}

		public MyEntity GetBroadcasterParentEntity(MyDataBroadcaster broadcaster)
		{
			if (broadcaster.Entity is MyCubeBlock)
			{
				return (broadcaster.Entity as MyCubeBlock).CubeGrid;
			}
			return broadcaster.Entity as MyEntity;
		}

		public MyCubeGrid GetLogicalGroupRepresentative(MyCubeGrid grid)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(grid);
			if (group == null || group.Nodes.Count == 0)
			{
				return grid;
			}
			MyCubeGrid nodeData = group.Nodes.First().NodeData;
			Enumerator<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node> enumerator = group.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node current = enumerator.get_Current();
					if (current.NodeData.GetBlocks().get_Count() > nodeData.GetBlocks().get_Count())
					{
						nodeData = current.NodeData;
					}
				}
				return nodeData;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void GetEntityBroadcasters(MyEntity entity, ref HashSet<MyDataBroadcaster> output, long playerId = 0L)
		{
			MyCharacter myCharacter = entity as MyCharacter;
			if (myCharacter != null)
			{
				output.Add((MyDataBroadcaster)myCharacter.RadioBroadcaster);
				MyCubeGrid myCubeGrid = myCharacter.GetTopMostParent() as MyCubeGrid;
				if (myCubeGrid != null)
				{
					GetCubeGridGroupBroadcasters(myCubeGrid, output, playerId);
				}
				return;
			}
			MyCubeBlock myCubeBlock = entity as MyCubeBlock;
			if (myCubeBlock != null)
			{
				GetCubeGridGroupBroadcasters(myCubeBlock.CubeGrid, output, playerId);
				return;
			}
			MyCubeGrid myCubeGrid2 = entity as MyCubeGrid;
			if (myCubeGrid2 != null)
			{
				GetCubeGridGroupBroadcasters(myCubeGrid2, output, playerId);
				return;
			}
			MyProxyAntenna myProxyAntenna = entity as MyProxyAntenna;
			if (myProxyAntenna != null)
			{
				GetProxyGridBroadcasters(myProxyAntenna, ref output, playerId);
			}
		}

		public static void GetCubeGridGroupBroadcasters(MyCubeGrid grid, HashSet<MyDataBroadcaster> output, long playerId = 0L)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(grid);
			Enumerator<MyDataBroadcaster> enumerator2;
			if (group != null)
			{
				Enumerator<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node> enumerator = group.m_members.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						enumerator2 = enumerator.get_Current().NodeData.GridSystems.RadioSystem.Broadcasters.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								MyDataBroadcaster current = enumerator2.get_Current();
								if (playerId == 0L || current.CanBeUsedByPlayer(playerId))
								{
									output.Add(current);
								}
							}
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
					}
				}
<<<<<<< HEAD
				return;
			}
			foreach (MyDataBroadcaster broadcaster2 in grid.GridSystems.RadioSystem.Broadcasters)
			{
				if (playerId == 0L || broadcaster2.CanBeUsedByPlayer(playerId))
				{
					output.Add(broadcaster2);
=======
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				return;
			}
			enumerator2 = grid.GridSystems.RadioSystem.Broadcasters.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					MyDataBroadcaster current2 = enumerator2.get_Current();
					if (playerId == 0L || current2.CanBeUsedByPlayer(playerId))
					{
						output.Add(current2);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
		}

		private void GetProxyGridBroadcasters(MyProxyAntenna proxy, ref HashSet<MyDataBroadcaster> output, long playerId = 0L)
		{
<<<<<<< HEAD
=======
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!m_proxyGrids.TryGetValue(proxy.Info.EntityId, out var value))
			{
				return;
			}
<<<<<<< HEAD
			foreach (MyDataBroadcaster item in value)
			{
				if (playerId == 0L || item.CanBeUsedByPlayer(playerId))
				{
					output.Add(item);
=======
			Enumerator<MyDataBroadcaster> enumerator = value.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyDataBroadcaster current = enumerator.get_Current();
					if (playerId == 0L || current.CanBeUsedByPlayer(playerId))
					{
						output.Add(current);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public void GetEntityReceivers(MyEntity entity, ref HashSet<MyDataReceiver> output, long playerId = 0L)
		{
			MyCharacter myCharacter = entity as MyCharacter;
			if (myCharacter != null)
			{
				output.Add((MyDataReceiver)myCharacter.RadioReceiver);
				MyCubeGrid myCubeGrid = myCharacter.GetTopMostParent() as MyCubeGrid;
				if (myCubeGrid != null)
				{
					GetCubeGridGroupReceivers(myCubeGrid, ref output, playerId);
				}
				return;
			}
			MyCubeBlock myCubeBlock = entity as MyCubeBlock;
			if (myCubeBlock != null)
			{
				GetCubeGridGroupReceivers(myCubeBlock.CubeGrid, ref output, playerId);
				return;
			}
			MyCubeGrid myCubeGrid2 = entity as MyCubeGrid;
			if (myCubeGrid2 != null)
			{
				GetCubeGridGroupReceivers(myCubeGrid2, ref output, playerId);
				return;
			}
			MyProxyAntenna myProxyAntenna = entity as MyProxyAntenna;
			if (myProxyAntenna != null)
			{
				GetProxyGridReceivers(myProxyAntenna, ref output, playerId);
			}
		}

		private void GetCubeGridGroupReceivers(MyCubeGrid grid, ref HashSet<MyDataReceiver> output, long playerId = 0L)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(grid);
			Enumerator<MyDataReceiver> enumerator2;
			if (group != null)
			{
				Enumerator<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node> enumerator = group.m_members.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						enumerator2 = enumerator.get_Current().NodeData.GridSystems.RadioSystem.Receivers.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								MyDataReceiver current = enumerator2.get_Current();
								if (playerId == 0L || current.CanBeUsedByPlayer(playerId))
								{
									output.Add(current);
								}
							}
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
						}
					}
				}
<<<<<<< HEAD
				return;
			}
			foreach (MyDataReceiver receiver2 in grid.GridSystems.RadioSystem.Receivers)
			{
				if (playerId == 0L || receiver2.CanBeUsedByPlayer(playerId))
				{
					output.Add(receiver2);
=======
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
				return;
			}
			enumerator2 = grid.GridSystems.RadioSystem.Receivers.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					MyDataReceiver current2 = enumerator2.get_Current();
					if (playerId == 0L || current2.CanBeUsedByPlayer(playerId))
					{
						output.Add(current2);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
		}

		private void GetProxyGridReceivers(MyProxyAntenna proxy, ref HashSet<MyDataReceiver> output, long playerId = 0L)
		{
<<<<<<< HEAD
=======
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!m_proxyGrids.TryGetValue(proxy.Info.EntityId, out var value))
			{
				return;
			}
<<<<<<< HEAD
			foreach (MyDataBroadcaster item in value)
			{
				if (item.Receiver != null && (playerId == 0L || item.CanBeUsedByPlayer(playerId)))
				{
					output.Add(item.Receiver);
=======
			Enumerator<MyDataBroadcaster> enumerator = value.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyDataBroadcaster current = enumerator.get_Current();
					if (current.Receiver != null && (playerId == 0L || current.CanBeUsedByPlayer(playerId)))
					{
						output.Add(current.Receiver);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		public HashSet<MyDataBroadcaster> GetAllRelayedBroadcasters(MyDataReceiver receiver, long identityId, bool mutual, HashSet<MyDataBroadcaster> output = null, bool resetExceptions = true)
		{
<<<<<<< HEAD
			if (resetExceptions)
			{
				m_nearbyBroadcastersSearchExceptions.Clear();
			}
			else if (!m_nearbyBroadcastersSearchExceptions.Contains(receiver.Entity.EntityId))
			{
				m_nearbyBroadcastersSearchExceptions.Add(receiver.Entity.EntityId);
			}
=======
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (output == null)
			{
				output = new HashSet<MyDataBroadcaster>();
			}
			Enumerator<MyDataBroadcaster> enumerator = receiver.BroadcastersInRange.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
<<<<<<< HEAD
					output.Add(item);
					if (item.Receiver != null && item.CanBeUsedByPlayer(identityId) && !m_nearbyBroadcastersSearchExceptions.Contains(item.Entity.EntityId))
					{
						GetAllRelayedBroadcasters(item.Receiver, identityId, mutual, output, resetExceptions: false);
=======
					MyDataBroadcaster current = enumerator.get_Current();
					if (!output.Contains(current) && !current.Closed && (!mutual || (current.Receiver != null && receiver.Broadcaster != null && current.Receiver.BroadcastersInRange.Contains(receiver.Broadcaster))))
					{
						output.Add(current);
						if (current.Receiver != null && current.CanBeUsedByPlayer(identityId))
						{
							GetAllRelayedBroadcasters(current.Receiver, identityId, mutual, output);
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				return output;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
<<<<<<< HEAD
			if (resetExceptions)
			{
				m_nearbyBroadcastersSearchExceptions.Clear();
			}
			return output;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public HashSet<MyDataBroadcaster> GetAllRelayedBroadcasters(MyEntity entity, long identityId, bool mutual = true, HashSet<MyDataBroadcaster> output = null, bool resetExceptions = true)
		{
<<<<<<< HEAD
			if (resetExceptions)
			{
				m_nearbyBroadcastersSearchExceptions.Clear();
			}
=======
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (output == null)
			{
				output = m_tmpBroadcasters;
				output.Clear();
			}
			m_tmpReceivers.Clear();
			GetEntityReceivers(entity, ref m_tmpReceivers, identityId);
			Enumerator<MyDataReceiver> enumerator = m_tmpReceivers.GetEnumerator();
			try
			{
<<<<<<< HEAD
				if (tmpReceiver.Entity != null && !tmpReceiver.Entity.Closed && !m_nearbyBroadcastersSearchExceptions.Contains(tmpReceiver.Entity.EntityId))
				{
					GetAllRelayedBroadcasters(tmpReceiver, identityId, mutual, output, resetExceptions);
				}
			}
			if (resetExceptions)
			{
				m_nearbyBroadcastersSearchExceptions.Clear();
=======
				while (enumerator.MoveNext())
				{
					MyDataReceiver current = enumerator.get_Current();
					GetAllRelayedBroadcasters(current, identityId, mutual, output);
				}
				return output;
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public bool CheckConnection(MyIdentity sender, MyIdentity receiver)
		{
			if (sender == receiver)
			{
				return true;
			}
			if (sender.Character == null || receiver.Character == null)
			{
				return false;
			}
			return CheckConnection(receiver.Character.RadioReceiver, sender.Character.RadioBroadcaster, receiver.IdentityId, mutual: false);
		}

		public bool CheckConnection(MyDataReceiver receiver, MyDataBroadcaster broadcaster, long playerIdentityId, bool mutual)
		{
			if (receiver == null || broadcaster == null)
			{
				return false;
			}
			return GetAllRelayedBroadcasters(receiver, playerIdentityId, mutual).Contains(broadcaster);
		}

		public bool CheckConnection(MyEntity receivingEntity, MyDataBroadcaster broadcaster, long playerIdentityId, bool mutual)
		{
			if (receivingEntity == null || broadcaster == null)
			{
				return false;
			}
			return GetAllRelayedBroadcasters(receivingEntity, playerIdentityId, mutual).Contains(broadcaster);
		}

		public bool CheckConnection(MyDataReceiver receiver, MyEntity broadcastingEntity, long playerIdentityId, bool mutual)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			if (receiver == null || broadcastingEntity == null)
			{
				return false;
			}
			m_tmpBroadcasters.Clear();
			m_tmpRelayedBroadcasters.Clear();
			GetAllRelayedBroadcasters(receiver, playerIdentityId, mutual, m_tmpRelayedBroadcasters);
			GetEntityBroadcasters(broadcastingEntity, ref m_tmpBroadcasters, playerIdentityId);
			Enumerator<MyDataBroadcaster> enumerator = m_tmpRelayedBroadcasters.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyDataBroadcaster current = enumerator.get_Current();
					if (m_tmpBroadcasters.Contains(current))
					{
						return true;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return false;
		}

		public bool CheckConnection(MyEntity broadcastingEntity, MyEntity receivingEntity, MyPlayer player, bool mutual = true)
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			MyCubeGrid myCubeGrid = broadcastingEntity as MyCubeGrid;
			if (myCubeGrid != null)
			{
				broadcastingEntity = GetLogicalGroupRepresentative(myCubeGrid);
			}
			MyCubeGrid myCubeGrid2 = receivingEntity as MyCubeGrid;
			if (myCubeGrid2 != null)
			{
				receivingEntity = GetLogicalGroupRepresentative(myCubeGrid2);
			}
			Enumerator<BroadcasterInfo> enumerator = GetConnectedGridsInfo(receivingEntity, player, mutual).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.get_Current().EntityId == broadcastingEntity.EntityId)
					{
						return true;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return false;
		}

		public void RegisterAntenna(MyDataBroadcaster broadcaster)
		{
			MyProxyAntenna value;
			if (broadcaster.Entity is MyProxyAntenna)
			{
				MyProxyAntenna myProxyAntenna = broadcaster.Entity as MyProxyAntenna;
				m_proxyAntennas[broadcaster.AntennaEntityId] = myProxyAntenna;
				RegisterProxyGrid(broadcaster);
				if (MyEntities.GetEntityById(broadcaster.AntennaEntityId) == null)
				{
					myProxyAntenna.Active = true;
				}
			}
			else if (m_proxyAntennas.TryGetValue(broadcaster.AntennaEntityId, out value))
			{
				value.Active = false;
			}
		}

		public void UnregisterAntenna(MyDataBroadcaster broadcaster)
		{
			MyProxyAntenna value;
			if (broadcaster.Entity is MyProxyAntenna)
			{
				MyProxyAntenna obj = broadcaster.Entity as MyProxyAntenna;
				m_proxyAntennas.Remove(broadcaster.AntennaEntityId);
				UnregisterProxyGrid(broadcaster);
				obj.Active = false;
			}
			else if (m_proxyAntennas.TryGetValue(broadcaster.AntennaEntityId, out value))
			{
				value.Active = true;
			}
		}

		private void RegisterProxyGrid(MyDataBroadcaster broadcaster)
		{
			if (!m_proxyGrids.TryGetValue(broadcaster.Info.EntityId, out var value))
			{
				value = new HashSet<MyDataBroadcaster>();
				m_proxyGrids.Add(broadcaster.Info.EntityId, value);
			}
			value.Add(broadcaster);
		}

		private void UnregisterProxyGrid(MyDataBroadcaster broadcaster)
		{
			if (m_proxyGrids.TryGetValue(broadcaster.Info.EntityId, out var value))
			{
				value.Remove(broadcaster);
				if (value.get_Count() == 0)
				{
					m_proxyGrids.Remove(broadcaster.Info.EntityId);
				}
			}
		}

		public void AddLaser(long id, MyLaserBroadcaster laser, bool register = true)
		{
			if (register)
			{
				RegisterAntenna(laser);
			}
			m_laserAntennas.Add(id, laser);
		}

		public void RemoveLaser(long id, bool register = true)
		{
			if (m_laserAntennas.TryGetValue(id, out var value))
			{
				m_laserAntennas.Remove(id);
				if (register)
				{
					UnregisterAntenna(value);
				}
			}
		}
	}
}
