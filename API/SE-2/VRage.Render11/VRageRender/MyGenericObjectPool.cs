using System;
using System.Collections.Generic;
using VRage.Collections;

namespace VRageRender
{
	/// <summary>
	/// A copy of MyObjectsPool that handles types a little different for the MyObjectPoolManager
	/// </summary>
	internal class MyGenericObjectPool
	{
		private readonly Type m_storedType;

		private Queue<IPooledObject> m_unused;

		private HashSet<IPooledObject> m_active;

		private HashSet<IPooledObject> m_marked;

		private int m_baseCapacity;

		public QueueReader<IPooledObject> Unused => new QueueReader<IPooledObject>(m_unused);

		public HashSetReader<IPooledObject> Active => new HashSetReader<IPooledObject>(m_active);

<<<<<<< HEAD
		public int ActiveCount => m_active.Count;

		public int BaseCapacity => m_baseCapacity;

		public int Capacity => m_unused.Count + m_active.Count;
=======
		public int ActiveCount => m_active.get_Count();

		public int BaseCapacity => m_baseCapacity;

		public int Capacity => m_unused.get_Count() + m_active.get_Count();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private MyGenericObjectPool(Type storedType)
		{
		}

		public MyGenericObjectPool(int baseCapacity, Type storedTypeOverride)
		{
			m_storedType = storedTypeOverride;
			Construct(baseCapacity);
		}

		private void Construct(int baseCapacity)
		{
			m_baseCapacity = baseCapacity;
			m_unused = new Queue<IPooledObject>(m_baseCapacity);
			m_active = new HashSet<IPooledObject>();
			m_marked = new HashSet<IPooledObject>();
			for (int i = 0; i < m_baseCapacity; i++)
			{
				m_unused.Enqueue((IPooledObject)Activator.CreateInstance(m_storedType));
			}
		}

		/// <summary>
		/// Returns true when new item was allocated
		/// </summary>
		public bool AllocateOrCreate(out IPooledObject item)
		{
<<<<<<< HEAD
			bool num = m_unused.Count == 0;
=======
			bool num = m_unused.get_Count() == 0;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (num)
			{
				item = (IPooledObject)Activator.CreateInstance(m_storedType);
			}
			else
			{
				item = m_unused.Dequeue();
			}
			m_active.Add(item);
			return num;
		}

		public void Deallocate(IPooledObject item)
		{
			m_active.Remove(item);
			m_unused.Enqueue(item);
		}

		public void MarkForDeallocate(IPooledObject item)
		{
			m_marked.Add(item);
		}

		public void DeallocateAllMarked()
		{
<<<<<<< HEAD
			foreach (IPooledObject item in m_marked)
			{
				Deallocate(item);
=======
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<IPooledObject> enumerator = m_marked.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IPooledObject current = enumerator.get_Current();
					Deallocate(current);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_marked.Clear();
		}

		public void DeallocateAll()
		{
<<<<<<< HEAD
			foreach (IPooledObject item in m_active)
			{
				m_unused.Enqueue(item);
=======
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<IPooledObject> enumerator = m_active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IPooledObject current = enumerator.get_Current();
					m_unused.Enqueue(current);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_active.Clear();
			m_marked.Clear();
		}

		public void TrimToBaseCapacity()
		{
<<<<<<< HEAD
			while (Capacity > BaseCapacity && m_unused.Count > 0)
=======
			while (Capacity > BaseCapacity && m_unused.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_unused.Dequeue();
			}
			m_unused.TrimExcess();
			m_active.TrimExcess();
			m_marked.TrimExcess();
		}
	}
}
