using System;
using VRage.Collections;
using VRage.Network;

namespace ParallelTasks
{
	internal class FutureWork<T> : AbstractWork, IPrioritizedWork, IWork
	{
		private class ParallelTasks_FutureWork_00601_003C_003EActor : IActivator, IActivator<FutureWork<T>>
		{
			private sealed override object CreateInstance()
			{
				return new FutureWork<T>();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override FutureWork<T> CreateInstance()
			{
				return new FutureWork<T>();
			}

			FutureWork<T> IActivator<FutureWork<T>>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		private static readonly MyConcurrentPool<FutureWork<T>> m_workPool = new MyConcurrentPool<FutureWork<T>>(10, null, 1000);

		internal int ID;

		internal Func<T> Function { get; set; }

		internal T Result { get; set; }

		public WorkPriority Priority { get; set; }

		public override void DoWork(WorkData workData = null)
		{
			Result = Function();
		}

		public static FutureWork<T> GetInstance()
		{
			return m_workPool.Get();
		}

		public void ReturnToPool()
		{
			if (ID != 0)
			{
				Result = default(T);
				Function = null;
				m_workPool.Return(this);
			}
		}
	}
}
