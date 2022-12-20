using System;
using System.Collections.Generic;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.EntityComponents
{
	[MyComponentBuilder(typeof(MyObjectBuilder_AreaTrigger), true)]
	public class MyAreaTriggerComponent : MyTriggerComponent
	{
		private class Sandbox_Game_EntityComponents_MyAreaTriggerComponent_003C_003EActor : IActivator, IActivator<MyAreaTriggerComponent>
		{
			private sealed override object CreateInstance()
			{
				return new MyAreaTriggerComponent();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyAreaTriggerComponent CreateInstance()
			{
				return new MyAreaTriggerComponent();
			}

			MyAreaTriggerComponent IActivator<MyAreaTriggerComponent>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private readonly HashSet<MyEntity> m_prevEntities = new HashSet<MyEntity>();
<<<<<<< HEAD

		private readonly List<MyEntity> m_entitiesToRemove = new List<MyEntity>();

=======

		private readonly List<MyEntity> m_entitiesToRemove = new List<MyEntity>();

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private readonly HashSet<MyCharacter> m_prevPlayers = new HashSet<MyCharacter>();

		private readonly HashSet<MyCharacter> m_currentPlayers = new HashSet<MyCharacter>();

		private readonly List<MyCharacter> m_playersToRemove = new List<MyCharacter>();

		public Action<long, string> EntityEntered;

		public string Name { get; set; }

		public double Radius
		{
			get
			{
				return m_boundingSphere.Radius;
			}
			set
			{
				m_boundingSphere.Radius = value;
				m_AABB.Min = new Vector3D((0.0 - value) / 2.0);
				m_AABB.Max = new Vector3D(value / 2.0);
				m_orientedBoundingBox.HalfExtent = new Vector3D(value / 2.0);
			}
		}

		public double SizeX
		{
			get
			{
				return m_orientedBoundingBox.HalfExtent.X * 2.0;
			}
			set
			{
				m_boundingSphere.Radius = value / 2.0;
				m_AABB.Min.X = (0.0 - value) / 2.0;
				m_AABB.Max.X = value / 2.0;
				m_orientedBoundingBox.HalfExtent.X = value / 2.0;
			}
		}

		public double SizeY
		{
			get
			{
				return m_orientedBoundingBox.HalfExtent.Y * 2.0;
			}
			set
			{
				m_boundingSphere.Radius = value / 2.0;
				m_AABB.Min.Y = (0.0 - value) / 2.0;
				m_AABB.Max.Y = value / 2.0;
				m_orientedBoundingBox.HalfExtent.Y = value / 2.0;
			}
		}

		public double SizeZ
		{
			get
			{
				return m_orientedBoundingBox.HalfExtent.Z * 2.0;
			}
			set
			{
				m_boundingSphere.Radius = value / 2.0;
				m_AABB.Min.Z = (0.0 - value) / 2.0;
				m_AABB.Max.Z = value / 2.0;
				m_orientedBoundingBox.HalfExtent.Z = value / 2.0;
			}
		}

		public MyAreaTriggerComponent()
			: this(string.Empty)
		{
		}

		public MyAreaTriggerComponent(string name)
			: base(TriggerType.Sphere, 20u)
		{
			Name = name;
		}

		protected override void UpdateInternal()
		{
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateInternal();
			m_currentPlayers.Clear();
			foreach (MyEntity item in base.QueryResult)
			{
				MyCharacter myCharacter = item as MyCharacter;
				if (myCharacter != null && !myCharacter.IsBot && myCharacter.ControlSteamId != 0L)
				{
					m_currentPlayers.Add(myCharacter);
				}
				MyCubeGrid myCubeGrid = item as MyCubeGrid;
				if (myCubeGrid == null || MySession.Static == null)
				{
					continue;
				}
				foreach (MyPlayer onlinePlayer in MySession.Static.Players.GetOnlinePlayers())
<<<<<<< HEAD
				{
					if (onlinePlayer.Character != null && onlinePlayer.Character.Parent is MyCockpit && ((MyCockpit)onlinePlayer.Character.Parent).CubeGrid.EntityId == myCubeGrid.EntityId)
					{
						m_currentPlayers.Add(onlinePlayer.Character);
					}
				}
			}
			foreach (MyEntity prevEntity in m_prevEntities)
			{
				if (!base.QueryResult.Contains(prevEntity))
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					if (onlinePlayer.Character != null && onlinePlayer.Character.Parent is MyCockpit && ((MyCockpit)onlinePlayer.Character.Parent).CubeGrid.EntityId == myCubeGrid.EntityId)
					{
<<<<<<< HEAD
						MyVisualScriptLogicProvider.AreaTrigger_EntityLeft(Name, prevEntity.EntityId, prevEntity.Name);
					}
					m_entitiesToRemove.Add(prevEntity);
				}
			}
=======
						m_currentPlayers.Add(onlinePlayer.Character);
					}
				}
			}
			Enumerator<MyEntity> enumerator3 = m_prevEntities.GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					MyEntity current3 = enumerator3.get_Current();
					if (!base.QueryResult.Contains(current3))
					{
						if (MyVisualScriptLogicProvider.AreaTrigger_EntityLeft != null)
						{
							MyVisualScriptLogicProvider.AreaTrigger_EntityLeft(Name, current3.EntityId, current3.Name);
						}
						m_entitiesToRemove.Add(current3);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator3).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (MyEntity item2 in m_entitiesToRemove)
			{
				m_prevEntities.Remove(item2);
			}
<<<<<<< HEAD
			foreach (MyCharacter prevPlayer in m_prevPlayers)
			{
				if (m_currentPlayers.Contains(prevPlayer))
				{
					continue;
				}
				if (MyVisualScriptLogicProvider.AreaTrigger_Left != null && !prevPlayer.Closed)
				{
					MyIdentity identity = prevPlayer.GetIdentity();
					if (identity != null)
					{
						MyVisualScriptLogicProvider.AreaTrigger_Left(Name, identity.IdentityId);
					}
=======
			Enumerator<MyCharacter> enumerator4 = m_prevPlayers.GetEnumerator();
			try
			{
				while (enumerator4.MoveNext())
				{
					MyCharacter current5 = enumerator4.get_Current();
					if (m_currentPlayers.Contains(current5))
					{
						continue;
					}
					if (MyVisualScriptLogicProvider.AreaTrigger_Left != null && !current5.Closed)
					{
						MyIdentity identity = current5.GetIdentity();
						if (identity != null)
						{
							MyVisualScriptLogicProvider.AreaTrigger_Left(Name, identity.IdentityId);
						}
					}
					m_playersToRemove.Add(current5);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				m_playersToRemove.Add(prevPlayer);
			}
<<<<<<< HEAD
=======
			finally
			{
				((IDisposable)enumerator4).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (MyCharacter item3 in m_playersToRemove)
			{
				m_prevPlayers.Remove(item3);
			}
			m_entitiesToRemove.Clear();
			m_playersToRemove.Clear();
			foreach (MyEntity item4 in base.QueryResult)
			{
				if (m_prevEntities.Add(item4))
				{
					if (MyVisualScriptLogicProvider.AreaTrigger_EntityEntered != null)
					{
						MyVisualScriptLogicProvider.AreaTrigger_EntityEntered(Name, item4.EntityId, item4.Name);
					}
					if (EntityEntered != null)
					{
						EntityEntered(item4.EntityId, item4.Name);
					}
				}
			}
<<<<<<< HEAD
			foreach (MyCharacter currentPlayer in m_currentPlayers)
			{
				if (m_prevPlayers.Add(currentPlayer) && MyVisualScriptLogicProvider.AreaTrigger_Entered != null)
				{
					MyIdentity identity2 = currentPlayer.GetIdentity();
					if (identity2 != null)
					{
						MyVisualScriptLogicProvider.AreaTrigger_Entered(Name, identity2.IdentityId);
=======
			enumerator4 = m_currentPlayers.GetEnumerator();
			try
			{
				while (enumerator4.MoveNext())
				{
					MyCharacter current8 = enumerator4.get_Current();
					if (m_prevPlayers.Add(current8) && MyVisualScriptLogicProvider.AreaTrigger_Entered != null)
					{
						MyIdentity identity2 = current8.GetIdentity();
						if (identity2 != null)
						{
							MyVisualScriptLogicProvider.AreaTrigger_Entered(Name, identity2.IdentityId);
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			finally
			{
				((IDisposable)enumerator4).Dispose();
			}
		}

		protected override bool QueryEvaluator(MyEntity entity)
		{
			if (entity is MyCharacter)
			{
				return true;
			}
			if (entity is MyCubeGrid)
			{
				return true;
			}
			if (entity is MyFloatingObject)
			{
				return true;
			}
			return false;
		}

		public override MyObjectBuilder_ComponentBase Serialize(bool copy)
		{
			MyObjectBuilder_AreaTrigger obj = base.Serialize(copy) as MyObjectBuilder_AreaTrigger;
			obj.Name = Name;
			return obj;
		}

		public override void Deserialize(MyObjectBuilder_ComponentBase builder)
		{
			base.Deserialize(builder);
			MyObjectBuilder_AreaTrigger myObjectBuilder_AreaTrigger = (MyObjectBuilder_AreaTrigger)builder;
			Name = myObjectBuilder_AreaTrigger.Name;
		}

		public override bool IsSerialized()
		{
			return true;
		}

		public override void DebugDraw()
		{
			base.DebugDraw();
		}
	}
}
