<<<<<<< HEAD
=======
using System;
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Threading;
using Sandbox.Game.Entities;
using VRage.Game.Entity;
using VRage.Groups;

namespace Sandbox.Engine.Physics
{
	public class MyWeldingGroups : MyGroups<MyEntity, MyWeldGroupData>, IMySceneComponent
	{
		private static MyWeldingGroups m_static;

		public static MyWeldingGroups Static => m_static;

		public void Load()
		{
			m_static = this;
			base.SupportsOphrans = true;
		}

		public void Unload()
		{
			m_static = null;
		}

		/// <summary>
		/// Replace common parent in weld group
		/// old parent is not considered part of weld group anymore (isnt welded to new parent)
		/// </summary>
		/// <param name="group">weld group</param>
		/// <param name="oldParent">old parent</param>
		/// <param name="newParent">new parent (can be null)</param>
		/// <returns>chosen new parent</returns>
		public static MyEntity ReplaceParent(Group group, MyEntity oldParent, MyEntity newParent)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<Node> enumerator;
			if (oldParent != null && oldParent.Physics != null)
			{
				oldParent.GetPhysicsBody().UnweldAll(insertInWorld: false);
			}
			else
			{
				if (group == null)
				{
					return oldParent;
				}
				enumerator = group.Nodes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Node current = enumerator.get_Current();
						if (!current.NodeData.MarkedForClose)
						{
							current.NodeData.GetPhysicsBody().Unweld(insertInWorld: false);
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			if (group == null)
			{
				return oldParent;
			}
			if (newParent == null)
			{
				enumerator = group.Nodes.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Node current2 = enumerator.get_Current();
						if (!current2.NodeData.MarkedForClose && current2.NodeData != oldParent)
						{
							if (current2.NodeData.Physics.IsStatic)
							{
								newParent = current2.NodeData;
								break;
							}
							if (current2.NodeData.Physics.RigidBody2 != null)
							{
								newParent = current2.NodeData;
							}
						}
					}
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
			}
			enumerator = group.Nodes.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Node current3 = enumerator.get_Current();
					if (!current3.NodeData.MarkedForClose && newParent != current3.NodeData)
					{
						if (newParent == null)
						{
							newParent = current3.NodeData;
						}
						else
						{
							newParent.GetPhysicsBody().Weld(current3.NodeData.Physics, recreateShape: false);
						}
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			if (newParent != null && !newParent.Physics.IsInWorld)
			{
				newParent.Physics.Activate();
			}
			return newParent;
		}

		public override void CreateLink(long linkId, MyEntity parentNode, MyEntity childNode)
		{
			if (MySandboxGame.Static.UpdateThread == Thread.get_CurrentThread())
			{
				base.CreateLink(linkId, parentNode, childNode);
			}
		}

		public bool IsEntityParent(MyEntity entity)
		{
			Group group = GetGroup(entity);
			if (group == null)
			{
				return true;
			}
			return entity == group.GroupData.Parent;
		}

		public MyWeldingGroups()
			: base(supportOphrans: false, (MajorGroupComparer)null)
		{
		}
	}
}
