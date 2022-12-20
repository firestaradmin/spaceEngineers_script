<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using Sandbox.Engine.Utils;
using Sandbox.Game.Components;
using Sandbox.Game.EntityComponents;
using VRage;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation)]
	public class MySessionComponentTriggerSystem : MySessionComponentBase
	{
		private readonly Dictionary<MyEntity, CachingHashSet<MyTriggerComponent>> m_triggers = new Dictionary<MyEntity, CachingHashSet<MyTriggerComponent>>();

		private readonly FastResourceLock m_dictionaryLock = new FastResourceLock();

		public static MySessionComponentTriggerSystem Static;

		public override void LoadData()
		{
			base.LoadData();
			Static = this;
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Static = null;
		}

		public MyEntity GetTriggersEntity(string triggerName, out MyTriggerComponent foundTrigger)
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			foundTrigger = null;
			foreach (KeyValuePair<MyEntity, CachingHashSet<MyTriggerComponent>> trigger in m_triggers)
			{
				Enumerator<MyTriggerComponent> enumerator2 = trigger.Value.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						MyTriggerComponent current2 = enumerator2.get_Current();
						MyAreaTriggerComponent myAreaTriggerComponent = current2 as MyAreaTriggerComponent;
						if (myAreaTriggerComponent != null && myAreaTriggerComponent.Name == triggerName)
						{
							foundTrigger = current2;
							return trigger.Key;
						}
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
			}
			return null;
		}

		public void AddTrigger(MyTriggerComponent trigger)
		{
			if (Contains(trigger))
			{
				return;
			}
			using (m_dictionaryLock.AcquireExclusiveUsing())
			{
				if (m_triggers.TryGetValue((MyEntity)trigger.Entity, out var value))
				{
					value.Add(trigger);
					return;
				}
				m_triggers[(MyEntity)trigger.Entity] = new CachingHashSet<MyTriggerComponent> { trigger };
			}
		}

		public static void RemoveTrigger(MyEntity entity, MyTriggerComponent trigger)
		{
			if (Static != null)
			{
				Static.RemoveTriggerInternal(entity, trigger);
			}
		}

		private void RemoveTriggerInternal(MyEntity entity, MyTriggerComponent trigger)
		{
			using (m_dictionaryLock.AcquireExclusiveUsing())
			{
				if (m_triggers.TryGetValue(entity, out var value))
				{
					value.Remove(trigger);
					value.ApplyChanges();
					if (value.Count == 0)
					{
						m_triggers.Remove(entity);
					}
				}
			}
		}

		public override void UpdateAfterSimulation()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateAfterSimulation();
			using (m_dictionaryLock.AcquireSharedUsing())
			{
				foreach (CachingHashSet<MyTriggerComponent> value in m_triggers.Values)
				{
					value.ApplyChanges();
					Enumerator<MyTriggerComponent> enumerator2 = value.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							enumerator2.get_Current().Update();
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
			}
		}

		public override void Draw()
		{
<<<<<<< HEAD
			if (!MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER)
			{
				return;
			}
			using (m_dictionaryLock.AcquireSharedUsing())
			{
				foreach (CachingHashSet<MyTriggerComponent> value in m_triggers.Values)
				{
					foreach (MyTriggerComponent item in value)
					{
						item.DebugDraw();
=======
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			if (!MyDebugDrawSettings.DEBUG_DRAW_UPDATE_TRIGGER)
			{
				return;
			}
			using (m_dictionaryLock.AcquireSharedUsing())
			{
				foreach (CachingHashSet<MyTriggerComponent> value in m_triggers.Values)
				{
					Enumerator<MyTriggerComponent> enumerator2 = value.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							enumerator2.get_Current().DebugDraw();
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
			}
		}

		public bool IsAnyTriggerActive(MyEntity entity)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			using (m_dictionaryLock.AcquireSharedUsing())
			{
				if (m_triggers.ContainsKey(entity))
				{
					Enumerator<MyTriggerComponent> enumerator = m_triggers[entity].GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.get_Current().Enabled)
							{
								return true;
							}
						}
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					return m_triggers[entity].Count == 0;
				}
			}
			return true;
		}

		public bool Contains(MyTriggerComponent trigger)
		{
			using (m_dictionaryLock.AcquireSharedUsing())
			{
				foreach (CachingHashSet<MyTriggerComponent> value in m_triggers.Values)
				{
					if (value.Contains(trigger))
					{
						return true;
					}
				}
			}
			return false;
		}

		public List<MyTriggerComponent> GetIntersectingTriggers(Vector3D position)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			List<MyTriggerComponent> list = new List<MyTriggerComponent>();
			using (m_dictionaryLock.AcquireSharedUsing())
			{
				foreach (CachingHashSet<MyTriggerComponent> value in m_triggers.Values)
				{
					Enumerator<MyTriggerComponent> enumerator2 = value.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							MyTriggerComponent current = enumerator2.get_Current();
							if (current.Contains(position))
							{
								list.Add(current);
							}
						}
					}
					finally
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
				return list;
			}
		}

		public List<MyTriggerComponent> GetAllTriggers()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			List<MyTriggerComponent> list = new List<MyTriggerComponent>();
			using (m_dictionaryLock.AcquireSharedUsing())
			{
				foreach (CachingHashSet<MyTriggerComponent> value in m_triggers.Values)
				{
					value.ApplyChanges();
<<<<<<< HEAD
					foreach (MyTriggerComponent item in value)
=======
					Enumerator<MyTriggerComponent> enumerator2 = value.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							MyTriggerComponent current2 = enumerator2.get_Current();
							list.Add(current2);
						}
					}
					finally
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						((IDisposable)enumerator2).Dispose();
					}
				}
				return list;
			}
		}
	}
}
