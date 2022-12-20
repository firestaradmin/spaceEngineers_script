using System;
using System.Collections.Generic;
using Sandbox.Game.Entities;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
	public class MyWeldGroupData : IGroupData<MyEntity>
	{
		private MyGroups<MyEntity, MyWeldGroupData>.Group m_group;

		private MyEntity m_weldParent;

		public MyEntity Parent => m_weldParent;

		public void OnPreRelease()
		{
		}

		public void OnRelease()
		{
			m_group = null;
			m_weldParent = null;
		}

		public void OnNodeAdded<TGroupData>(MyEntity entity, TGroupData prevGroup) where TGroupData : IGroupData<MyEntity>, new()
		{
			if (entity.MarkedForClose)
			{
				return;
			}
			if (m_weldParent == null)
			{
				m_weldParent = entity;
			}
			else
			{
				MyPhysicsBody myPhysicsBody = m_weldParent.Physics as MyPhysicsBody;
				if (myPhysicsBody != null)
				{
					if (myPhysicsBody.IsStatic)
					{
						myPhysicsBody.Weld(entity.Physics as MyPhysicsBody);
					}
					else if (entity.Physics.IsStatic || (myPhysicsBody.RigidBody2 == null && entity.Physics.RigidBody2 != null))
					{
						ReplaceParent(entity);
					}
					else
					{
						myPhysicsBody.Weld(entity.Physics as MyPhysicsBody);
					}
				}
			}
			if (m_weldParent.Physics != null && m_weldParent.Physics.RigidBody != null)
			{
				m_weldParent.Physics.RigidBody.Activate();
			}
			m_weldParent.RaisePhysicsChanged();
		}

		public void OnPostNodeAdded<TGroupData>(MyEntity entity, TGroupData prevGroup) where TGroupData : IGroupData<MyEntity>, new()
		{
		}

		public void OnNodeRemoved<TGroupData>(MyEntity entity, TGroupData prevGroup) where TGroupData : IGroupData<MyEntity>, new()
		{
			if (m_weldParent == null)
			{
				return;
			}
			if (m_weldParent == entity)
			{
				if ((m_group.Nodes.Count != 1 || !m_group.Nodes.First().NodeData.MarkedForClose) && m_group.Nodes.Count > 0)
				{
					ReplaceParent(null);
				}
			}
			else if (m_weldParent.Physics != null && !entity.MarkedForClose)
			{
				(m_weldParent.Physics as MyPhysicsBody).Unweld(entity.Physics as MyPhysicsBody);
			}
			if (m_weldParent != null && m_weldParent.Physics != null && m_weldParent.Physics.RigidBody != null)
			{
				m_weldParent.Physics.RigidBody.Activate();
				m_weldParent.RaisePhysicsChanged();
			}
			entity.RaisePhysicsChanged();
		}

		public void OnPreNodeRemoved<TGroupData>(MyEntity entity, TGroupData prevGroup) where TGroupData : IGroupData<MyEntity>, new()
		{
		}

		private void ReplaceParent(MyEntity newParent)
		{
			m_weldParent = MyWeldingGroups.ReplaceParent(m_group, m_weldParent, newParent);
		}

		public void OnCreate<TGroupData>(MyGroups<MyEntity, TGroupData>.Group group) where TGroupData : IGroupData<MyEntity>, new()
		{
			m_group = group as MyGroups<MyEntity, MyWeldGroupData>.Group;
<<<<<<< HEAD
		}

		public void OnPostCreate()
		{
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		/// <summary>
		/// Call when node body changes quality type (dynamic/static/doubled)
		/// Checks if there isnt more suitable parent in the group
		/// i.e. group parent is dynamic and there is node that is static
		/// </summary>
		/// <param name="oldParent">WeldInfo.Parent or self if WeldInfo.Parent is null</param>
		/// <returns>parent changed</returns>
		public bool UpdateParent(MyEntity oldParent)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			MyPhysicsBody physicsBody = oldParent.GetPhysicsBody();
			if (physicsBody.WeldedRigidBody.IsFixed)
			{
				return false;
			}
			MyPhysicsBody myPhysicsBody = physicsBody;
			Enumerator<MyPhysicsBody> enumerator = physicsBody.WeldInfo.Children.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyPhysicsBody current = enumerator.get_Current();
					if (current.WeldedRigidBody.IsFixed)
					{
						myPhysicsBody = current;
						break;
					}
					if (!myPhysicsBody.Flags.HasFlag(RigidBodyFlag.RBF_DOUBLED_KINEMATIC) && current.Flags.HasFlag(RigidBodyFlag.RBF_DOUBLED_KINEMATIC))
					{
						myPhysicsBody = current;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			if (myPhysicsBody == physicsBody)
			{
				return false;
			}
			ReplaceParent((MyEntity)myPhysicsBody.Entity);
			myPhysicsBody.Weld(physicsBody);
			return true;
		}
	}
}
