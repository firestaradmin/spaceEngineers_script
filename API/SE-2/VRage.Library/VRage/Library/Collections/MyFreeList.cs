using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VRage.Library.Collections
{
	public class MyFreeList<TItem>
	{
		private TItem[] m_list;

		private int m_size;

		private readonly Queue<int> m_freePositions;

		private readonly TItem m_default;

		public ref TItem this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return ref m_list[index];
			}
		}

		/// <summary>
		/// Length of the internal buffer occupied by a mix of free and empty nodes, data past this length is all free.
		/// </summary>
		public int UsedLength => m_size;

		/// <summary>
		/// Total number of allocated positions.
		/// </summary>
<<<<<<< HEAD
		public int Count => m_size - m_freePositions.Count;
=======
		public int Count => m_size - m_freePositions.get_Count();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public int Capacity => m_list.Length;

		public MyFreeList(int capacity = 16, TItem defaultValue = default(TItem))
		{
			m_list = new TItem[16];
			m_freePositions = new Queue<int>(capacity / 2);
			m_default = defaultValue;
		}

		public int Allocate()
		{
<<<<<<< HEAD
			if (m_freePositions.Count > 0)
=======
			if (m_freePositions.get_Count() > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return m_freePositions.Dequeue();
			}
			if (m_size == m_list.Length)
			{
				Array.Resize(ref m_list, m_list.Length << 1);
			}
			return m_size++;
		}

		public int Allocate(TItem value)
		{
			int num = Allocate();
			m_list[num] = value;
			return num;
		}

		public void Free(int position)
		{
			m_list[position] = m_default;
			if (position == m_size)
			{
				m_size--;
			}
			else
			{
				m_freePositions.Enqueue(position);
			}
		}

		public TItem[] GetInternalArray()
		{
			return m_list;
		}

		public bool KeyValid(int key)
		{
			return (uint)key < m_size;
		}

		public void Clear()
		{
			for (int i = 0; i < m_size; i++)
			{
				m_list[i] = default(TItem);
			}
			m_size = 0;
			m_freePositions.Clear();
		}
	}
}
