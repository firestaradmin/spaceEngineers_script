using Sandbox.Game.Entities;

namespace Sandbox.Game.GameSystems
{
	public abstract class MyUpdateableGridSystem
	{
		private bool m_scheduled;

		protected MyCubeGrid Grid { get; private set; }

		public abstract MyCubeGrid.UpdateQueue Queue { get; }

		public virtual int UpdatePriority => int.MaxValue;

		public virtual bool UpdateInParallel => true;

<<<<<<< HEAD
		/// <inheritdoc />
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		protected MyUpdateableGridSystem(MyCubeGrid grid)
		{
			Grid = grid;
		}

		protected abstract void Update();

		protected void Schedule()
		{
			if (!m_scheduled)
			{
				Grid.Schedule(Queue, Update, UpdatePriority);
				if (!Queue.IsExecutedOnce())
				{
					m_scheduled = true;
				}
			}
		}

		protected void DeSchedule()
		{
			Grid.DeSchedule(Queue, Update);
			m_scheduled = false;
		}
	}
}
