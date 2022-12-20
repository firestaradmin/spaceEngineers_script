using VRage.Game.Entity;

namespace Sandbox.Game.Entities
{
	public struct MyParallelUpdateFlag
	{
		private bool m_needsUpdate;

		public MyParallelUpdateFlag(bool needsUpdate)
		{
			m_needsUpdate = needsUpdate;
		}

		public void Enable(MyEntity entity)
		{
			Set(entity, value: true);
		}

		public void Disable(MyEntity entity)
		{
			Set(entity, value: false);
		}

		public void Set(MyEntity entity, bool value)
		{
			if (value != m_needsUpdate)
			{
				m_needsUpdate = value;
				if (entity.InScene)
				{
					MyEntities.Orchestrator.EntityFlagsChanged(entity);
				}
			}
		}

		public MyParallelUpdateFlags GetFlags(MyEntity entity)
		{
			MyParallelUpdateFlags myParallelUpdateFlags = entity.NeedsUpdate.GetParallel();
			if (m_needsUpdate)
			{
				myParallelUpdateFlags |= MyParallelUpdateFlags.EACH_FRAME_PARALLEL;
			}
			return myParallelUpdateFlags;
		}
	}
}
