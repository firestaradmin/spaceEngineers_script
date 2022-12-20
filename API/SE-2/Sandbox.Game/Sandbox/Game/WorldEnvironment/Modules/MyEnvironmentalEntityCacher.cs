using System.Collections.Generic;
using Sandbox.Game.World;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.Entity;

namespace Sandbox.Game.WorldEnvironment.Modules
{
	/// <summary>
	/// The environmental entity cacher will keep entity references for some time and then close them.
	///
	/// This is useful when multiple sector lods support entities because the entity would be deleted
	/// and then re-created during the transition.
	/// </summary>
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class MyEnvironmentalEntityCacher : MySessionComponentBase
	{
		private struct EntityReference
		{
			public MyEntity Entity;
		}

		private const long EntityPreserveTime = 1000L;

		private HashSet<long> m_index;

		private MyBinaryStructHeap<long, EntityReference> m_entities;

		public void QueueEntity(MyEntity entity)
		{
			long num = Time();
			num += 1000;
			m_entities.Insert(new EntityReference
			{
				Entity = entity
			}, num);
			m_index.Add(entity.EntityId);
			if (base.UpdateOrder == MyUpdateOrder.NoUpdate)
			{
				SetUpdateOrder(MyUpdateOrder.AfterSimulation);
			}
		}

		public MyEntity GetEntity(long entityId)
		{
			if (m_index.Remove(entityId))
			{
				return m_entities.Remove(entityId).Entity;
			}
			return null;
		}

		public override void UpdateAfterSimulation()
		{
			long num = Time();
			while (m_entities.Count > 0 && m_entities.MinKey() < num)
			{
				m_index.Remove(m_entities.RemoveMin().Entity.EntityId);
			}
			if (m_entities.Count == 0)
			{
				SetUpdateOrder(MyUpdateOrder.NoUpdate);
			}
		}

		/// <summary>
		/// Get the current game time in milliseconds.
		/// </summary>
		/// <returns></returns>
		private static long Time()
		{
			return MySession.Static.ElapsedGameTime.Ticks / 10000;
		}
	}
}
