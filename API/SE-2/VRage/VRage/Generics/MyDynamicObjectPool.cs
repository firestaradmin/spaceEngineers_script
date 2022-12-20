using System;
using System.Collections.Generic;

namespace VRage.Generics
{
	/// <summary>
	/// Dynamic object pool. It allocates a new instance when necessary.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class MyDynamicObjectPool<T> where T : class, new()
	{
		private readonly Stack<T> m_poolStack;

		private readonly Action<T> m_deallocator;
<<<<<<< HEAD

		public int Count => m_poolStack.Count;

=======

		public int Count => m_poolStack.get_Count();

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyDynamicObjectPool(int capacity, Action<T> deallocator = null)
		{
			m_deallocator = deallocator ?? ((Action<T>)delegate
			{
			});
			m_poolStack = new Stack<T>(capacity);
			Preallocate(capacity);
		}

		private void Preallocate(int count)
		{
			for (int i = 0; i < count; i++)
			{
				T val = new T();
				m_poolStack.Push(val);
			}
		}

		public T Allocate()
		{
			if (m_poolStack.get_Count() == 0)
			{
				Preallocate(1);
			}
			return m_poolStack.Pop();
		}

		public void Deallocate(T item)
		{
			m_poolStack.Push(item);
		}

		public void TrimToSize(int size)
		{
			while (m_poolStack.get_Count() > size)
			{
				m_poolStack.Pop();
			}
			m_poolStack.TrimExcess();
		}

		/// <summary>
		/// Suppress finalization of all items buffered in the pool.
		///
		/// This should only be called if the elements of the pool have some form of leak detecting finalizer.
		/// </summary>
		public void SuppressFinalize()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<T> enumerator = m_poolStack.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GC.SuppressFinalize(enumerator.get_Current());
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}
	}
}
