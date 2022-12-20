using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using VRage.Collections;

namespace ParallelTasks
{
	/// <summary>
	/// A thread safe, non-blocking, object pool.
	/// </summary>
	/// <typeparam name="T">The type of item to store. Must be a class with a parameterless constructor.</typeparam>
	public class Pool<T> : Singleton<Pool<T>> where T : class, new()
	{
		private readonly ConcurrentDictionary<Thread, MyConcurrentQueue<T>> m_instances;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:ParallelTasks.Pool`1" /> class.
		/// </summary>
		public Pool()
		{
<<<<<<< HEAD
			m_instances = new ConcurrentDictionary<Thread, MyConcurrentQueue<T>>(Environment.ProcessorCount, Environment.ProcessorCount);
=======
			m_instances = (ConcurrentDictionary<Thread, MyConcurrentQueue<T>>)(object)new ConcurrentDictionary<Thread, MyConcurrentQueue<Thread>>(MyEnvironment.ProcessorCount, MyEnvironment.ProcessorCount);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		/// <summary>
		/// Gets an instance from the pool.
		/// </summary>
		/// <returns>An instance of <typeparamref name="T" />.</returns>
		public unsafe T Get(Thread thread)
		{
<<<<<<< HEAD
			if (!m_instances.TryGetValue(thread, out var value))
=======
			MyConcurrentQueue<T> myConcurrentQueue = default(MyConcurrentQueue<T>);
			if (!((ConcurrentDictionary<Thread, MyConcurrentQueue<Thread>>)(object)m_instances).TryGetValue(thread, ref *(MyConcurrentQueue<Thread>*)(&myConcurrentQueue)))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				myConcurrentQueue = new MyConcurrentQueue<T>();
				bool flag = ((ConcurrentDictionary<Thread, MyConcurrentQueue<Thread>>)(object)m_instances).TryAdd(thread, (MyConcurrentQueue<Thread>)(object)myConcurrentQueue);
			}
<<<<<<< HEAD
			if (!value.TryDequeue(out var result))
=======
			if (!myConcurrentQueue.TryDequeue(out var result))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return new T();
			}
			return result;
		}

		/// <summary>
		/// Returns an instance to the pool, so it is available for re-use.
		/// It is advised that the item is reset to a default state before being returned.
		/// </summary>
		/// <param name="thread"></param>
		/// <param name="instance">The instance to return to the pool.</param>
		public void Return(Thread thread, T instance)
		{
			MyConcurrentQueue<T> myConcurrentQueue = ((ConcurrentDictionary<Thread, MyConcurrentQueue<Thread>>)(object)m_instances).get_Item(thread);
			myConcurrentQueue.Enqueue(instance);
		}

		public void Clean()
		{
<<<<<<< HEAD
			foreach (KeyValuePair<Thread, MyConcurrentQueue<T>> instance in m_instances)
			{
				instance.Value.Clear();
=======
			foreach (KeyValuePair<Thread, MyConcurrentQueue<T>> item in (ConcurrentDictionary<Thread, MyConcurrentQueue<Thread>>)(object)m_instances)
			{
				item.Value.Clear();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}
	}
}
