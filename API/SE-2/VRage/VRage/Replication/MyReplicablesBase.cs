using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using VRage.Collections;
using VRage.Network;
using VRageMath;

namespace VRage.Replication
{
	public abstract class MyReplicablesBase
	{
		private static readonly HashSet<IMyReplicable> m_empty = new HashSet<IMyReplicable>();

		private readonly Stack<HashSet<IMyReplicable>> m_hashSetPool = new Stack<HashSet<IMyReplicable>>();

		private readonly ConcurrentDictionary<IMyReplicable, HashSet<IMyReplicable>> m_parentToChildren = new ConcurrentDictionary<IMyReplicable, HashSet<IMyReplicable>>();

		private readonly ConcurrentDictionary<IMyReplicable, IMyReplicable> m_childToParent = new ConcurrentDictionary<IMyReplicable, IMyReplicable>();

		private readonly Thread m_mainThread;

		public event Action<IMyReplicable> OnChildAdded;

		protected MyReplicablesBase(Thread mainThread)
		{
			m_mainThread = mainThread;
		}

		public void GetAllChildren(IMyReplicable replicable, List<IMyReplicable> resultList)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<IMyReplicable> enumerator = GetChildren(replicable).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IMyReplicable current = enumerator.get_Current();
					resultList.Add(current);
					GetAllChildren(current, resultList);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		/// <summary>
		/// Sets or resets replicable (updates child status and parent).
		/// Returns true if replicable is root, otherwise false.
		/// </summary>
		public void Add(IMyReplicable replicable, out IMyReplicable parent)
		{
			if (replicable.HasToBeChild && TryGetParent(replicable, out parent))
			{
				AddChild(replicable, parent);
			}
			else if (!replicable.HasToBeChild)
			{
				parent = null;
				AddRoot(replicable);
			}
			else
			{
				parent = null;
			}
		}

		/// <summary>
		/// Removes replicable with all children, children of children, etc.
		/// </summary>
		public void RemoveHierarchy(IMyReplicable replicable)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			HashSet<IMyReplicable> valueOrDefault = m_parentToChildren.GetValueOrDefault<IMyReplicable, HashSet<IMyReplicable>>(replicable, m_empty);
			while (valueOrDefault.get_Count() > 0)
			{
				Enumerator<IMyReplicable> enumerator = valueOrDefault.GetEnumerator();
				enumerator.MoveNext();
				RemoveHierarchy(enumerator.get_Current());
			}
			Remove(replicable);
		}

		private HashSet<IMyReplicable> Obtain()
		{
			if (m_hashSetPool.get_Count() <= 0)
			{
				return new HashSet<IMyReplicable>();
			}
			return m_hashSetPool.Pop();
		}

		public HashSetReader<IMyReplicable> GetChildren(IMyReplicable replicable)
		{
			return m_parentToChildren.GetValueOrDefault<IMyReplicable, HashSet<IMyReplicable>>(replicable, m_empty);
		}

		private static bool TryGetParent(IMyReplicable replicable, out IMyReplicable parent)
		{
			parent = replicable.GetParent();
			return parent != null;
		}

		/// <summary>
		/// Refreshes replicable, updates it's child status and parent.
		/// </summary>
		public void Refresh(IMyReplicable replicable)
		{
<<<<<<< HEAD
			IMyReplicable value2;
			if (replicable.HasToBeChild && TryGetParent(replicable, out var parent))
			{
				if (m_childToParent.TryGetValue(replicable, out var value))
=======
			IMyReplicable parent2 = default(IMyReplicable);
			if (replicable.HasToBeChild && TryGetParent(replicable, out var parent))
			{
				IMyReplicable myReplicable = default(IMyReplicable);
				if (m_childToParent.TryGetValue(replicable, ref myReplicable))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					if (myReplicable != parent)
					{
						RemoveChild(replicable, myReplicable);
						AddChild(replicable, parent);
					}
				}
				else
				{
					RemoveRoot(replicable);
					AddChild(replicable, parent);
				}
			}
			else if (m_childToParent.TryGetValue(replicable, ref parent2))
			{
				RemoveChild(replicable, parent2);
				AddRoot(replicable);
			}
		}

		/// <summary>
		/// Removes replicable, children should be already removed
		/// </summary>
		private void Remove(IMyReplicable replicable)
		{
<<<<<<< HEAD
			if (m_childToParent.TryGetValue(replicable, out var value))
=======
			IMyReplicable parent = default(IMyReplicable);
			if (m_childToParent.TryGetValue(replicable, ref parent))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				RemoveChild(replicable, parent);
			}
			RemoveRoot(replicable);
		}

		protected virtual void AddChild(IMyReplicable replicable, IMyReplicable parent)
		{
<<<<<<< HEAD
			if (!m_parentToChildren.TryGetValue(parent, out var value))
=======
			HashSet<IMyReplicable> val = default(HashSet<IMyReplicable>);
			if (!m_parentToChildren.TryGetValue(parent, ref val))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				val = Obtain();
				m_parentToChildren.set_Item(parent, val);
			}
<<<<<<< HEAD
			value.Add(replicable);
			m_childToParent[replicable] = parent;
=======
			val.Add(replicable);
			m_childToParent.set_Item(replicable, parent);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			this.OnChildAdded.InvokeIfNotNull(replicable);
		}

		protected virtual void RemoveChild(IMyReplicable replicable, IMyReplicable parent)
		{
			m_childToParent.Remove<IMyReplicable, IMyReplicable>(replicable);
			HashSet<IMyReplicable> val = m_parentToChildren.get_Item(parent);
			val.Remove(replicable);
			if (val.get_Count() == 0)
			{
				m_parentToChildren.Remove<IMyReplicable, HashSet<IMyReplicable>>(parent);
				m_hashSetPool.Push(val);
			}
		}

		public abstract void IterateRoots(Action<IMyReplicable> p);

		public abstract void GetReplicablesInBox(BoundingBoxD aabb, List<IMyReplicable> list);

		protected abstract void AddRoot(IMyReplicable replicable);

		protected abstract void RemoveRoot(IMyReplicable replicable);

		[Conditional("DEBUG")]
		protected void CheckThread()
		{
		}
	}
}
