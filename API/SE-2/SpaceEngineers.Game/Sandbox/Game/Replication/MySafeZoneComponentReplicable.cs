using System;
using System.Collections.Generic;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Multiplayer;
using SpaceEngineers.Game.Entities.Blocks.SafeZone;
using VRage.Game.Entity;
using VRage.Library.Collections;
using VRage.Network;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Replication
{
	public class MySafeZoneComponentReplicable : MyExternalReplicableEvent<MySafeZoneComponent>
	{
		private readonly Action<MyEntity> m_destroyEntity;

		private long m_entityId;

		private MySafeZoneComponent SafeZoneManager => base.Instance;

		public override bool IsValid
		{
			get
			{
				if (SafeZoneManager != null && SafeZoneManager.Entity != null)
				{
					return !SafeZoneManager.Entity.MarkedForClose;
				}
				return false;
			}
		}

		public override bool HasToBeChild => true;

		public MySafeZoneComponentReplicable()
		{
			m_destroyEntity = delegate
			{
				RaiseDestroyed();
			};
		}

		protected override void OnHook()
		{
			base.OnHook();
			if (SafeZoneManager != null)
			{
				((MyEntity)SafeZoneManager.Entity).OnClose += m_destroyEntity;
				SafeZoneManager.BeforeRemovedFromContainer += delegate
				{
					OnRemovedFromContainer();
				};
				MyCubeBlock myCubeBlock = SafeZoneManager.Entity as MyCubeBlock;
				if (myCubeBlock != null)
				{
					myCubeBlock.SlimBlock.CubeGridChanged += OnBlockCubeGridChanged;
				}
			}
		}

		private void OnBlockCubeGridChanged(MySlimBlock slimBlock, MyCubeGrid grid)
		{
			if (Sync.IsServer)
			{
				(MyMultiplayer.ReplicationLayer as MyReplicationLayer).RefreshReplicableHierarchy(this);
			}
		}

		public override IMyReplicable GetParent()
		{
			if (SafeZoneManager.Entity is MyCubeBlock)
			{
				return MyExternalReplicable.FindByObject(SafeZoneManager.Entity as MyCubeBlock);
			}
			return null;
		}

		public override bool OnSave(BitStream stream, Endpoint clientEndpoint)
		{
			long value = SafeZoneManager.Entity.EntityId;
			MySerializer.Write(stream, ref value);
			return true;
		}

		protected override void OnLoad(BitStream stream, Action<MySafeZoneComponent> loadingDoneHandler)
		{
			if (stream != null)
			{
				MySerializer.CreateAndRead<long>(stream, out m_entityId);
			}
			MyEntities.CallAsync(delegate
			{
				loadingDoneHandler(FindComponent());
			});
		}

		private MySafeZoneComponent FindComponent()
		{
			MySafeZoneComponent component = null;
			if (MyEntities.TryGetEntityById(m_entityId, out var entity) && entity != null && entity.Components.TryGet<MySafeZoneComponent>(out component))
			{
				return component;
			}
			return null;
		}

		public override void OnDestroyClient()
		{
			if (SafeZoneManager != null && SafeZoneManager.Entity != null)
			{
				((MyEntity)SafeZoneManager.Entity).OnClose -= m_destroyEntity;
			}
		}

		public override void GetStateGroups(List<IMyStateGroup> resultList)
		{
		}

		private void OnRemovedFromContainer()
		{
			if (SafeZoneManager != null && SafeZoneManager.Entity != null)
			{
				((MyEntity)SafeZoneManager.Entity).OnClose -= m_destroyEntity;
				MyCubeBlock myCubeBlock = SafeZoneManager.Entity as MyCubeBlock;
				if (myCubeBlock != null)
				{
					myCubeBlock.SlimBlock.CubeGridChanged -= OnBlockCubeGridChanged;
				}
				RaiseDestroyed();
			}
		}

		public override BoundingBoxD GetAABB()
		{
			return BoundingBoxD.CreateInvalid();
		}
	}
}
