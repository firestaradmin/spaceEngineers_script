using System;

namespace Sandbox.Game.Entities
{
<<<<<<< HEAD
	/// <summary>
	/// Extension methods for the <see cref="T:Sandbox.Game.Entities.MyCubeGrid.UpdateQueue" /> enum.
	/// </summary>
	public static class MyCubeGridUpdateQueueExtensions
	{
		/// <summary>
		/// Whether a given update queue will be executed once.
		/// </summary>
		/// <param name="queue"></param>
		/// <returns></returns>
		public static bool IsExecutedOnce(this MyCubeGrid.UpdateQueue queue)
		{
			switch (queue)
			{
			case MyCubeGrid.UpdateQueue.BeforeSimulation:
				return false;
			case MyCubeGrid.UpdateQueue.AfterSimulation:
				return false;
			case MyCubeGrid.UpdateQueue.OnceBeforeSimulation:
				return true;
			case MyCubeGrid.UpdateQueue.OnceAfterSimulation:
				return true;
			default:
				throw new ArgumentOutOfRangeException("queue", queue, null);
			}
		}

		/// <summary>
		/// Whether a given update queue is executed before simulation.
		/// </summary>
		/// <param name="queue"></param>
		/// <returns></returns>
		public static bool IsBeforeSimulation(this MyCubeGrid.UpdateQueue queue)
		{
			switch (queue)
			{
			case MyCubeGrid.UpdateQueue.BeforeSimulation:
				return true;
			case MyCubeGrid.UpdateQueue.AfterSimulation:
				return false;
			case MyCubeGrid.UpdateQueue.OnceBeforeSimulation:
				return true;
			case MyCubeGrid.UpdateQueue.OnceAfterSimulation:
				return false;
			default:
				throw new ArgumentOutOfRangeException("queue", queue, null);
			}
		}

		/// <summary>
		/// Whether a given update queue is executed after simulation.
		/// </summary>
		/// <param name="queue"></param>
		/// <returns></returns>
		public static bool IsAfterSimulation(this MyCubeGrid.UpdateQueue queue)
		{
			switch (queue)
			{
			case MyCubeGrid.UpdateQueue.BeforeSimulation:
				return false;
			case MyCubeGrid.UpdateQueue.AfterSimulation:
				return true;
			case MyCubeGrid.UpdateQueue.OnceBeforeSimulation:
				return false;
			case MyCubeGrid.UpdateQueue.OnceAfterSimulation:
				return true;
			default:
				throw new ArgumentOutOfRangeException("queue", queue, null);
			}
=======
	public static class MyCubeGridUpdateQueueExtensions
	{
		public static bool IsExecutedOnce(this MyCubeGrid.UpdateQueue queue)
		{
			return queue switch
			{
				MyCubeGrid.UpdateQueue.BeforeSimulation => false, 
				MyCubeGrid.UpdateQueue.AfterSimulation => false, 
				MyCubeGrid.UpdateQueue.OnceBeforeSimulation => true, 
				MyCubeGrid.UpdateQueue.OnceAfterSimulation => true, 
				_ => throw new ArgumentOutOfRangeException("queue", queue, null), 
			};
		}

		public static bool IsBeforeSimulation(this MyCubeGrid.UpdateQueue queue)
		{
			return queue switch
			{
				MyCubeGrid.UpdateQueue.BeforeSimulation => true, 
				MyCubeGrid.UpdateQueue.AfterSimulation => false, 
				MyCubeGrid.UpdateQueue.OnceBeforeSimulation => true, 
				MyCubeGrid.UpdateQueue.OnceAfterSimulation => false, 
				_ => throw new ArgumentOutOfRangeException("queue", queue, null), 
			};
		}

		public static bool IsAfterSimulation(this MyCubeGrid.UpdateQueue queue)
		{
			return queue switch
			{
				MyCubeGrid.UpdateQueue.BeforeSimulation => false, 
				MyCubeGrid.UpdateQueue.AfterSimulation => true, 
				MyCubeGrid.UpdateQueue.OnceBeforeSimulation => false, 
				MyCubeGrid.UpdateQueue.OnceAfterSimulation => true, 
				_ => throw new ArgumentOutOfRangeException("queue", queue, null), 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
