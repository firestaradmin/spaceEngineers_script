using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using VRage.Library.Collections;
using VRage.Library.Threading;

namespace VRage.Collections
{
	/// <summary>
	/// List wrapper that allows for addition and removal even during enumeration.
	/// Done by caching changes and allowing explicit application using Apply* methods.
	///
	/// This version has individual locks for cached and non-cached versions, allowing
	/// each to be managed efficiently even across multiple threads
	/// </summary>
	public class ConcurrentCachingLinkedList<T> : IEnumerable<T>, IEnumerable
	{
		private readonly LinkedList<T> m_list = new LinkedList<T>();

		private readonly List<T> m_toAdd = new List<T>();

		private readonly List<T> m_toRemove = new List<T>();

		private readonly SpinLockRef m_listLock = new SpinLockRef();

		private readonly SpinLockRef m_cacheLock = new SpinLockRef();

		private bool m_dirty;

		private Action<T, LinkedListNode<T>> m_setter;

		private Func<T, LinkedListNode<T>> m_getter;

		public int Count => m_list.Count;

		public bool IsEmpty
		{
			get
			{
				if (m_list.Count == 0 && m_toAdd.Count == 0)
				{
					return m_toRemove.Count == 0;
				}
				return false;
			}
		}

		public ConcurrentCachingLinkedList(Action<T, LinkedListNode<T>> setter, Func<T, LinkedListNode<T>> getter)
		{
			m_setter = setter;
			m_getter = getter;
		}

		public void Add(T entity)
		{
			using (m_cacheLock.Acquire())
			{
				if (m_toRemove.Contains(entity))
				{
					m_toRemove.Remove(entity);
					return;
				}
				m_toAdd.Add(entity);
				m_dirty = true;
			}
		}

		public void Remove(T entity, bool immediate = false)
		{
			using (m_cacheLock.Acquire())
			{
				if (!m_toAdd.Remove(entity))
				{
					m_toRemove.Add(entity);
				}
			}
			if (immediate)
			{
				using (m_listLock.Acquire())
				{
					using (m_cacheLock.Acquire())
					{
						LinkedListNode<T> node = m_getter(entity);
						m_list.Remove(node);
						m_toRemove.Remove(entity);
					}
				}
			}
			else
			{
				m_dirty = true;
			}
		}

		public void ClearList()
		{
			using (m_listLock.Acquire())
			{
				m_list.Clear();
			}
		}

		public void ClearImmediate()
		{
			using (m_listLock.Acquire())
			{
				using (m_cacheLock.Acquire())
				{
					m_toAdd.Clear();
					m_toRemove.Clear();
					m_list.Clear();
					m_dirty = false;
				}
			}
		}

		public void ApplyChanges()
		{
			if (m_dirty)
			{
				m_dirty = false;
				ApplyAdditions();
				ApplyRemovals();
			}
		}

		public void ApplyAdditions()
		{
			using (m_listLock.Acquire())
			{
				using (m_cacheLock.Acquire())
				{
					foreach (T item in m_toAdd)
					{
						LinkedListNode<T> arg = m_list.AddLast(item);
						m_setter(item, arg);
					}
					m_toAdd.Clear();
				}
			}
		}

		public void ApplyRemovals()
		{
			using (m_listLock.Acquire())
			{
				using (m_cacheLock.Acquire())
				{
					foreach (T item in m_toRemove)
					{
						LinkedListNode<T> linkedListNode = m_getter(item);
						if (linkedListNode != null && linkedListNode.List == m_list)
						{
							m_list.Remove(linkedListNode);
						}
					}
					m_toRemove.Clear();
				}
			}
		}

		public void Invoke(Action action)
		{
			using (m_listLock.Acquire())
			{
				action();
			}
		}

		public ConcurrentEnumerator<SpinLockRef.Token, T, LinkedList<T>.Enumerator> GetEnumerator()
		{
			return ConcurrentEnumerator.Create<SpinLockRef.Token, T, LinkedList<T>.Enumerator>(m_listLock.Acquire(), m_list.GetEnumerator());
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		[Conditional("DEBUG")]
		public void DebugCheckEmpty()
		{
		}

		public override string ToString()
		{
			return $"Count = {m_list.Count}; ToAdd = {m_toAdd.Count}; ToRemove = {m_toRemove.Count}";
		}
	}
}
