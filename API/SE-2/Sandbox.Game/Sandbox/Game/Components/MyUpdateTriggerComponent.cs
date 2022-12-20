<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Linq;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Debris;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Components
{
	[MyComponentBuilder(typeof(MyObjectBuilder_UpdateTrigger), true)]
	public class MyUpdateTriggerComponent : MyTriggerComponent
	{
		private class Sandbox_Game_Components_MyUpdateTriggerComponent_003C_003EActor : IActivator, IActivator<MyUpdateTriggerComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyUpdateTriggerComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyUpdateTriggerComponent CreateInstance()
			{
				return new MyUpdateTriggerComponent();
			}

			MyUpdateTriggerComponent IActivator<MyUpdateTriggerComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private int m_size = 100;

		private Dictionary<MyEntity, MyEntityUpdateEnum> m_needsUpdate = new Dictionary<MyEntity, MyEntityUpdateEnum>();

		private bool m_isPirateStation;

		private MyObjectBuilder_CubeGrid m_serializedPirateStation;

		public int Size
		{
			get
			{
				return m_size;
			}
			set
			{
				m_size = value;
				if (base.Entity != null)
				{
					m_AABB.Inflate(value / 2);
				}
			}
		}

		public override string ComponentTypeDebugString => "Pirate update trigger";

		public MyUpdateTriggerComponent()
		{
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy = false)
		{
			MyObjectBuilder_UpdateTrigger obj = (MyObjectBuilder_UpdateTrigger)base.Serialize(copy);
			obj.Size = m_size;
			obj.IsPirateStation = m_isPirateStation;
			obj.SerializedPirateStation = m_serializedPirateStation;
			return obj;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			base.Deserialize(builder);
			MyObjectBuilder_UpdateTrigger myObjectBuilder_UpdateTrigger = (MyObjectBuilder_UpdateTrigger)builder;
			m_size = myObjectBuilder_UpdateTrigger.Size;
			m_isPirateStation = myObjectBuilder_UpdateTrigger.IsPirateStation;
			m_serializedPirateStation = myObjectBuilder_UpdateTrigger.SerializedPirateStation;
		}

		private void grid_OnBlockOwnershipChanged(MyCubeGrid obj)
		{
			bool flag = false;
			foreach (long bigOwner in obj.BigOwners)
			{
				MyFaction playerFaction = MySession.Static.Factions.GetPlayerFaction(bigOwner);
				if (playerFaction != null && !playerFaction.IsEveryoneNpc())
				{
					flag = true;
					break;
				}
			}
			foreach (long smallOwner in obj.SmallOwners)
			{
				MyFaction playerFaction2 = MySession.Static.Factions.GetPlayerFaction(smallOwner);
				if (playerFaction2 != null && !playerFaction2.IsEveryoneNpc())
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				obj.Components.Remove<MyUpdateTriggerComponent>();
				obj.OnBlockOwnershipChanged -= grid_OnBlockOwnershipChanged;
			}
		}

		public MyUpdateTriggerComponent(int triggerSize)
		{
			m_size = triggerSize;
		}

		protected override void UpdateInternal()
		{
			if (base.Entity.Physics == null && !(base.Entity is MyProxyAntenna))
			{
				return;
			}
			m_AABB = base.Entity.PositionComp.WorldAABB.Inflate(m_size / 2);
			bool flag = m_needsUpdate.Count != 0;
			for (int num = base.QueryResult.Count - 1; num >= 0; num--)
			{
				MyEntity myEntity = base.QueryResult[num];
				if (!myEntity.Closed && myEntity.PositionComp.WorldAABB.Intersects(m_AABB) && !(myEntity is MyMeteor))
				{
					break;
				}
				base.QueryResult.RemoveAtFast(num);
			}
			base.DoQuery = base.QueryResult.Count == 0;
			base.UpdateInternal();
			MyCubeGrid myCubeGrid;
			if (!m_isPirateStation && (myCubeGrid = base.Entity as MyCubeGrid) != null && myCubeGrid.DisplayName.Contains("Pirate Base"))
			{
				bool flag2 = false;
				MyPlayerCollection players = MySession.Static.Players;
				foreach (long smallOwner in myCubeGrid.SmallOwners)
				{
					if (smallOwner != 0L && !players.IdentityIsNpc(smallOwner))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					m_isPirateStation = true;
				}
			}
			if (base.QueryResult.Count == 0)
			{
				if (!flag)
				{
					if (m_isPirateStation)
					{
						DespawnPirateStation();
					}
					else
					{
						DisableRecursively((MyEntity)base.Entity);
					}
				}
			}
			else if (m_isPirateStation)
			{
				RespawnPirateStation();
			}
			else if (flag)
			{
				EnableRecursively((MyEntity)base.Entity);
				m_needsUpdate.Clear();
			}
		}

		private void RespawnPirateStation()
		{
			if (Sync.IsServer && !(base.Entity is MyCubeGrid))
			{
				if (m_serializedPirateStation != null)
				{
					MyEntities.CreateFromObjectBuilderParallel(m_serializedPirateStation, addToScene: true, null, null, null, null, checkPosition: false, fadeIn: true);
				}
				base.Entity.Close();
			}
		}

		private void DespawnPirateStation()
		{
			if (!Sync.IsServer)
			{
				return;
			}
			MyCubeGrid myCubeGrid;
			if ((myCubeGrid = base.Entity as MyCubeGrid) != null)
			{
				HashSet<MyDataBroadcaster> output = new HashSet<MyDataBroadcaster>();
				MyAntennaSystem.Static.GetEntityBroadcasters(myCubeGrid, ref output, 0L);
<<<<<<< HEAD
				IEnumerable<MyRadioBroadcaster> source = output.OfType<MyRadioBroadcaster>();
				if (source.Any())
				{
					MyRadioBroadcaster myRadioBroadcaster = source.MaxBy((MyRadioBroadcaster x) => x.BroadcastRadius);
=======
				IEnumerable<MyRadioBroadcaster> enumerable = Enumerable.OfType<MyRadioBroadcaster>((IEnumerable)output);
				if (Enumerable.Any<MyRadioBroadcaster>(enumerable))
				{
					MyRadioBroadcaster myRadioBroadcaster = enumerable.MaxBy((MyRadioBroadcaster x) => x.BroadcastRadius);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					MyObjectBuilder_ProxyAntenna myObjectBuilder_ProxyAntenna = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_ProxyAntenna>();
					myRadioBroadcaster.InitProxyObjectBuilder(myObjectBuilder_ProxyAntenna);
					MyObjectBuilder_CubeGrid serializedPirateStation = (MyObjectBuilder_CubeGrid)myCubeGrid.GetObjectBuilder();
					MyObjectBuilder_ComponentContainer myObjectBuilder_ComponentContainer = myCubeGrid.Components.Serialize();
					myObjectBuilder_ComponentContainer.Components.RemoveAll((MyObjectBuilder_ComponentContainer.ComponentData x) => !(x.Component is MyObjectBuilder_UpdateTrigger));
					((MyObjectBuilder_UpdateTrigger)myObjectBuilder_ComponentContainer.Components[0].Component).SerializedPirateStation = serializedPirateStation;
					myObjectBuilder_ProxyAntenna.ComponentContainer = myObjectBuilder_ComponentContainer;
					MySandboxGame.Static.Invoke("SpawnProxyPirate", myObjectBuilder_ProxyAntenna, delegate(object b)
					{
						MyEntities.CreateFromObjectBuilderAndAdd((MyObjectBuilder_EntityBase)b, fadeIn: false);
					});
					myCubeGrid.Close();
				}
			}
			else
			{
				_ = base.Entity is MyProxyAntenna;
			}
		}

		protected override bool QueryEvaluator(MyEntity entity)
		{
			if (entity.Physics == null || entity.Physics.IsStatic)
			{
				return false;
			}
			if (entity is MyFloatingObject || entity is MyDebrisBase)
			{
				return false;
			}
			if (entity == base.Entity.GetTopMostParent())
			{
				return false;
			}
			return true;
		}

		private void DisableRecursively(MyEntity entity)
		{
			Enabled = false;
			m_needsUpdate[entity] = entity.NeedsUpdate;
			entity.NeedsUpdate = MyEntityUpdateEnum.NONE;
			entity.Render.Visible = false;
			if (entity.Hierarchy == null)
			{
				return;
			}
			foreach (MyHierarchyComponentBase child in entity.Hierarchy.Children)
			{
				DisableRecursively((MyEntity)child.Entity);
			}
		}

		private void EnableRecursively(MyEntity entity)
		{
			Enabled = true;
			if (m_needsUpdate.ContainsKey(entity))
			{
				entity.NeedsUpdate = m_needsUpdate[entity];
			}
			entity.Render.Visible = true;
			if (entity.Hierarchy == null)
			{
				return;
			}
			foreach (MyHierarchyComponentBase child in entity.Hierarchy.Children)
			{
				EnableRecursively((MyEntity)child.Entity);
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			if (base.Entity != null && !base.Entity.MarkedForClose && base.QueryResult.Count != 0)
			{
				EnableRecursively((MyEntity)base.Entity);
				m_needsUpdate.Clear();
			}
			m_needsUpdate.Clear();
		}

		public override void OnAddedToScene()
		{
			base.OnAddedToScene();
			MyCubeGrid myCubeGrid;
			if ((myCubeGrid = base.Entity as MyCubeGrid) != null)
			{
				myCubeGrid.OnBlockOwnershipChanged += grid_OnBlockOwnershipChanged;
			}
		}

		public override void OnBeforeRemovedFromContainer()
		{
			base.OnBeforeRemovedFromContainer();
			MyCubeGrid myCubeGrid;
			if ((myCubeGrid = base.Entity as MyCubeGrid) != null)
			{
				myCubeGrid.OnBlockOwnershipChanged -= grid_OnBlockOwnershipChanged;
			}
		}
	}
}
