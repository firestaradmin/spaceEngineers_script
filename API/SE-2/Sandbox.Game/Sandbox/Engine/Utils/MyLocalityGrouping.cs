using System;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Engine.Utils
{
	/// <summary>
	/// Use this class to prevent multiple instances close to each other at the same time.
	/// Call add instance to test whether instance can be added.
	/// </summary>
	public class MyLocalityGrouping
	{
		public enum GroupingMode
		{
			ContainsCenter,
			Overlaps
		}

		private struct InstanceInfo
		{
			public Vector3 Position;

			public float Radius;

			public int EndTimeMs;
		}

		private class InstanceInfoComparer : IComparer<InstanceInfo>
		{
			public int Compare(InstanceInfo x, InstanceInfo y)
			{
				return x.EndTimeMs - y.EndTimeMs;
			}
		}

		public GroupingMode Mode;

		private SortedSet<InstanceInfo> m_instances = new SortedSet<InstanceInfo>((IComparer<InstanceInfo>)new InstanceInfoComparer());

		private int TimeMs => MySandboxGame.TotalGamePlayTimeInMilliseconds;

		public MyLocalityGrouping(GroupingMode mode)
		{
			Mode = mode;
		}

		/// <summary>
		/// This is currently O(n), when it's not enough, bounding volume tree or KD-tree will be used.
		/// </summary>
		public bool AddInstance(TimeSpan lifeTime, Vector3 position, float radius, bool removeOld = true)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (removeOld)
			{
				RemoveOld();
			}
			Enumerator<InstanceInfo> enumerator = m_instances.GetEnumerator();
			try
			{
<<<<<<< HEAD
				float num = ((Mode == GroupingMode.ContainsCenter) ? Math.Max(radius, instance.Radius) : (radius + instance.Radius));
				if (Vector3.DistanceSquared(position, instance.Position) < num * num)
=======
				while (enumerator.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					InstanceInfo current = enumerator.get_Current();
					float num = ((Mode == GroupingMode.ContainsCenter) ? Math.Max(radius, current.Radius) : (radius + current.Radius));
					if (Vector3.DistanceSquared(position, current.Position) < num * num)
					{
						return false;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			m_instances.Add(new InstanceInfo
			{
				EndTimeMs = TimeMs + (int)lifeTime.TotalMilliseconds,
				Position = position,
				Radius = radius
			});
			return true;
		}

		/// <summary>
		/// This is O(r) where r is number of removed elements
		/// </summary>
		public void RemoveOld()
		{
			int timeMs = TimeMs;
			while (m_instances.get_Count() > 0 && m_instances.get_Min().EndTimeMs < timeMs)
			{
				m_instances.Remove(m_instances.get_Min());
			}
		}

		public void Clear()
		{
			m_instances.Clear();
		}
	}
}
