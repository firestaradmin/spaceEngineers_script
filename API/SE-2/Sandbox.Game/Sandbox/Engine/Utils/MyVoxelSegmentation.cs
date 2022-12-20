<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Network;
using VRageMath;

namespace Sandbox.Engine.Utils
{
	public class MyVoxelSegmentation
	{
		[ProtoContract]
		public struct Segment
		{
			protected class Sandbox_Engine_Utils_MyVoxelSegmentation_003C_003ESegment_003C_003EMin_003C_003EAccessor : IMemberAccessor<Segment, Vector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Segment owner, in Vector3I value)
				{
					owner.Min = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Segment owner, out Vector3I value)
				{
					value = owner.Min;
				}
			}

			protected class Sandbox_Engine_Utils_MyVoxelSegmentation_003C_003ESegment_003C_003EMax_003C_003EAccessor : IMemberAccessor<Segment, Vector3I>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref Segment owner, in Vector3I value)
				{
					owner.Max = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref Segment owner, out Vector3I value)
				{
					value = owner.Max;
				}
			}

			private class Sandbox_Engine_Utils_MyVoxelSegmentation_003C_003ESegment_003C_003EActor : IActivator, IActivator<Segment>
			{
				private sealed override object CreateInstance()
				{
					return default(Segment);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override Segment CreateInstance()
				{
					return (Segment)(object)default(Segment);
				}

				Segment IActivator<Segment>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public Vector3I Min;

			[ProtoMember(4)]
			public Vector3I Max;

			public Vector3I Size => Max - Min + Vector3I.One;

			public int VoxelCount => Size.X * Size.Y * Size.Z;

			public Segment(Vector3I min, Vector3I max)
			{
				Min = min;
				Max = max;
			}

			public bool Contains(Segment b)
			{
				if (Vector3I.Min(b.Min, Min) == Min)
				{
					return Vector3I.Max(b.Max, Max) == Max;
				}
				return false;
			}

			public void Replace(IEnumerable<Vector3I> voxels)
			{
				Min = Vector3I.MaxValue;
				Max = Vector3I.MinValue;
				foreach (Vector3I voxel in voxels)
				{
					Min = Vector3I.Min(Min, voxel);
					Max = Vector3I.Max(Max, voxel);
				}
			}
		}

		private class SegmentSizeComparer : IComparer<Segment>
		{
			public int Compare(Segment x, Segment y)
			{
				return y.VoxelCount - x.VoxelCount;
			}
		}

		private class Vector3IComparer : IComparer<Vector3I>
		{
			public int Compare(Vector3I x, Vector3I y)
			{
				return x.CompareTo(y);
			}
		}

		private class Vector3IEqualityComparer : IEqualityComparer<Vector3I>
		{
			public bool Equals(Vector3I v1, Vector3I v2)
			{
				if (v1.X == v2.X && v1.Y == v2.Y)
				{
					return v1.Z == v2.Z;
				}
				return false;
			}

			public int GetHashCode(Vector3I obj)
			{
				return (((obj.X * 9767) ^ obj.Y) * 9767) ^ obj.Z;
			}
		}

		private class DescIntComparer : IComparer<int>
		{
			public int Compare(int x, int y)
			{
				return y - x;
			}
		}

		private HashSet<Vector3I> m_filledVoxels = new HashSet<Vector3I>((IEqualityComparer<Vector3I>)new Vector3IEqualityComparer());

		private HashSet<Vector3I> m_selectionList = new HashSet<Vector3I>((IEqualityComparer<Vector3I>)new Vector3IEqualityComparer());

		private List<Segment> m_segments = new List<Segment>();

		private List<Segment> m_tmpSegments = new List<Segment>();

		private HashSet<Vector3I> m_usedVoxels = new HashSet<Vector3I>();

		public int InputCount => m_filledVoxels.get_Count();

		public void ClearInput()
		{
			m_filledVoxels.Clear();
		}

		public void AddInput(Vector3I input)
		{
			m_filledVoxels.Add(input);
		}

		/// <summary>
		/// Creates segments from voxel data.
		/// </summary>        
		/// <param name="mergeIterations">Number of post-process merge iterations, one should be always sufficient, algorithm works pretty well with 0 too.</param>
		/// <param name="segmentationType"></param>
		/// <returns>List of segments.</returns>
		public List<Segment> FindSegments(MyVoxelSegmentationType segmentationType = MyVoxelSegmentationType.Optimized, int mergeIterations = 1)
		{
			m_segments.Clear();
			switch (segmentationType)
			{
			case MyVoxelSegmentationType.Simple:
				CreateSegmentsSimple();
				break;
			case MyVoxelSegmentationType.Simple2:
				CreateSegmentsSimple2();
				break;
			case MyVoxelSegmentationType.ExtraSimple:
				CreateSegmentsExtraSimple();
				break;
			default:
			{
				CreateSegments(segmentationType == MyVoxelSegmentationType.Fast);
				m_segments.Sort(new SegmentSizeComparer());
				RemoveFullyContainedOptimized();
				ClipSegments();
				for (int i = 0; i < mergeIterations; i++)
				{
					MergeSegments();
				}
				break;
			}
			}
			return m_segments;
		}

		private void CreateSegmentsExtraSimple()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			while (m_filledVoxels.get_Count() > 0)
			{
				Enumerator<Vector3I> enumerator = m_filledVoxels.GetEnumerator();
				enumerator.MoveNext();
				Vector3I start = enumerator.get_Current();
				Vector3I pos = start;
				ExpandX(ref start, ref pos);
				ExpandY(ref start, ref pos);
				ExpandZ(ref start, ref pos);
				m_segments.Add(new Segment(start, pos));
				for (int i = start.X; i <= pos.X; i++)
				{
					for (int j = start.Y; j <= pos.Y; j++)
					{
						for (int k = start.Z; k <= pos.Z; k++)
						{
							m_filledVoxels.Remove(new Vector3I(i, j, k));
						}
					}
				}
			}
		}

		private void MergeSegments()
		{
			for (int i = 0; i < m_segments.Count; i++)
			{
				int num = i + 1;
				while (num < m_segments.Count)
				{
					Segment value = m_segments[i];
					Segment segment = m_segments[num];
					int num2 = 0;
					if (value.Min.X == segment.Min.X && value.Max.X == segment.Max.X)
					{
						num2++;
					}
					if (value.Min.Y == segment.Min.Y && value.Max.Y == segment.Max.Y)
					{
						num2++;
					}
					if (value.Min.Z == segment.Min.Z && value.Max.Z == segment.Max.Z)
					{
						num2++;
					}
					if (num2 == 2 && (value.Min.X == segment.Max.X + 1 || value.Max.X + 1 == segment.Min.X || value.Min.Y == segment.Max.Y + 1 || value.Max.Y + 1 == segment.Min.Y || value.Min.Z == segment.Max.Z + 1 || value.Max.Z + 1 == segment.Min.Z))
					{
						value.Min = Vector3I.Min(value.Min, segment.Min);
						value.Max = Vector3I.Max(value.Max, segment.Max);
						m_segments[i] = value;
						m_segments.RemoveAt(num);
					}
					else
					{
						num++;
					}
				}
			}
		}

		private void ClipSegments()
		{
			for (int num = m_segments.Count - 1; num >= 0; num--)
			{
				m_filledVoxels.Clear();
				AddAllVoxels(m_segments[num].Min, m_segments[num].Max);
				for (int num2 = m_segments.Count - 1; num2 >= 0; num2--)
				{
					if (num != num2)
					{
						RemoveVoxels(m_segments[num2].Min, m_segments[num2].Max);
						if (m_filledVoxels.get_Count() == 0)
						{
							break;
						}
					}
				}
				if (m_filledVoxels.get_Count() == 0)
				{
					m_segments.RemoveAt(num);
				}
				else
				{
					Segment value = m_segments[num];
					value.Replace((IEnumerable<Vector3I>)m_filledVoxels);
					m_segments[num] = value;
				}
			}
		}

		private void AddAllVoxels(Vector3I from, Vector3I to)
		{
			for (int i = from.X; i <= to.X; i++)
			{
				for (int j = from.Y; j <= to.Y; j++)
				{
					for (int k = from.Z; k <= to.Z; k++)
					{
						m_filledVoxels.Add(new Vector3I(i, j, k));
					}
				}
			}
		}

		private void RemoveVoxels(Vector3I from, Vector3I to)
		{
			for (int i = from.X; i <= to.X; i++)
			{
				for (int j = from.Y; j <= to.Y; j++)
				{
					for (int k = from.Z; k <= to.Z; k++)
					{
						m_filledVoxels.Remove(new Vector3I(i, j, k));
					}
				}
			}
		}

		private void RemoveFullyContained()
		{
			for (int i = 0; i < m_segments.Count; i++)
			{
				int num = i + 1;
				while (num < m_segments.Count)
				{
					if (m_segments[i].Contains(m_segments[num]))
					{
						m_segments.RemoveAt(num);
					}
					else
					{
						num++;
					}
				}
			}
		}

		private void RemoveFullyContainedOptimized()
		{
			m_filledVoxels.Clear();
			m_tmpSegments.Clear();
			Vector3I vector3I = default(Vector3I);
			for (int i = 0; i < m_segments.Count; i++)
			{
				bool flag = false;
				Vector3I min = m_segments[i].Min;
				Vector3I max = m_segments[i].Max;
				vector3I.X = min.X;
				while (vector3I.X <= max.X)
				{
					vector3I.Y = min.Y;
					while (vector3I.Y <= max.Y)
					{
						vector3I.Z = min.Z;
						while (vector3I.Z <= max.Z)
						{
<<<<<<< HEAD
							flag = m_filledVoxels.Add(item) || flag;
							item.Z++;
=======
							flag = m_filledVoxels.Add(vector3I) || flag;
							vector3I.Z++;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						vector3I.Y++;
					}
					vector3I.X++;
				}
				if (flag)
				{
					m_tmpSegments.Add(m_segments[i]);
				}
			}
			List<Segment> segments = m_segments;
			m_segments = m_tmpSegments;
			m_tmpSegments = segments;
		}

		private void CreateSegments(bool fastMethod)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			m_usedVoxels.Clear();
			Enumerator<Vector3I> enumerator = m_filledVoxels.GetEnumerator();
			try
			{
<<<<<<< HEAD
				if (m_usedVoxels.Contains(filledVoxel))
				{
					continue;
				}
				Vector3I start = filledVoxel;
				Vector3I pos = filledVoxel;
				ExpandX(ref start, ref pos);
				ExpandY(ref start, ref pos);
				ExpandZ(ref start, ref pos);
				AddSegment(ref start, ref pos);
				if (fastMethod)
				{
					continue;
				}
				while (pos.X > start.X)
				{
					while (pos.Y > start.Y)
					{
						pos.Y--;
						pos.Z = start.Z;
						ExpandZ(ref start, ref pos);
						AddSegment(ref start, ref pos);
					}
					pos.X--;
					pos.Y = start.Y;
					pos.Z = start.Z;
					ExpandY(ref start, ref pos);
					ExpandZ(ref start, ref pos);
					AddSegment(ref start, ref pos);
=======
				while (enumerator.MoveNext())
				{
					Vector3I current = enumerator.get_Current();
					if (m_usedVoxels.Contains(current))
					{
						continue;
					}
					Vector3I start = current;
					Vector3I pos = current;
					ExpandX(ref start, ref pos);
					ExpandY(ref start, ref pos);
					ExpandZ(ref start, ref pos);
					AddSegment(ref start, ref pos);
					if (fastMethod)
					{
						continue;
					}
					while (pos.X > start.X)
					{
						while (pos.Y > start.Y)
						{
							pos.Y--;
							pos.Z = start.Z;
							ExpandZ(ref start, ref pos);
							AddSegment(ref start, ref pos);
						}
						pos.X--;
						pos.Y = start.Y;
						pos.Z = start.Z;
						ExpandY(ref start, ref pos);
						ExpandZ(ref start, ref pos);
						AddSegment(ref start, ref pos);
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private void AddSegment(ref Vector3I from, ref Vector3I to)
		{
			bool flag = false;
			Vector3I vector3I = default(Vector3I);
			vector3I.X = from.X;
			while (vector3I.X <= to.X)
			{
				vector3I.Y = from.Y;
				while (vector3I.Y <= to.Y)
				{
					vector3I.Z = from.Z;
					while (vector3I.Z <= to.Z)
					{
<<<<<<< HEAD
						flag = m_usedVoxels.Add(item) || flag;
						item.Z++;
=======
						flag = m_usedVoxels.Add(vector3I) || flag;
						vector3I.Z++;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					vector3I.Y++;
				}
				vector3I.X++;
			}
			if (flag)
			{
				m_segments.Add(new Segment(from, to));
			}
		}

		private Vector3I ShiftVector(Vector3I vec)
		{
			return new Vector3I(vec.Z, vec.X, vec.Y);
		}

		private bool AllFilled(Vector3I from, Vector3I to)
		{
			Vector3I vector3I = default(Vector3I);
			vector3I.X = to.X;
			while (vector3I.X >= from.X)
			{
				vector3I.Y = to.Y;
				while (vector3I.Y >= from.Y)
				{
					vector3I.Z = to.Z;
					while (vector3I.Z >= from.Z)
					{
						if (!m_filledVoxels.Contains(vector3I))
						{
							return false;
						}
						vector3I.Z--;
					}
					vector3I.Y--;
				}
				vector3I.X--;
			}
			return true;
		}

		private int Expand(Vector3I start, ref Vector3I pos, ref Vector3I expand)
		{
			int num = 0;
			while (AllFilled(start + expand, pos + expand))
			{
				start += expand;
				pos += expand;
				num++;
			}
			return num;
		}

		private int ExpandX(ref Vector3I start, ref Vector3I pos)
		{
			return Expand(start, ref pos, ref Vector3I.UnitX);
		}

		private int ExpandY(ref Vector3I start, ref Vector3I pos)
		{
			return Expand(start, ref pos, ref Vector3I.UnitY);
		}

		private int ExpandZ(ref Vector3I start, ref Vector3I pos)
		{
			return Expand(start, ref pos, ref Vector3I.UnitZ);
		}

		private void CreateSegmentsSimple2()
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			m_selectionList.Clear();
			Enumerator<Vector3I> enumerator = m_filledVoxels.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Vector3I current = enumerator.get_Current();
					m_selectionList.Add(current);
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			CreateSegmentsSimpleCore();
		}

		private void CreateSegmentsSimple()
		{
			HashSet<Vector3I> selectionList = m_selectionList;
			m_selectionList = m_filledVoxels;
			CreateSegmentsSimpleCore();
			m_selectionList = selectionList;
		}

		private void CreateSegmentsSimpleCore()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			while (m_selectionList.get_Count() > 0)
			{
				Enumerator<Vector3I> enumerator = m_selectionList.GetEnumerator();
				enumerator.MoveNext();
				bool flag = true;
				bool flag2 = true;
				bool flag3 = true;
				bool flag4 = true;
				bool flag5 = true;
				bool flag6 = true;
				Vector3I min = enumerator.get_Current();
				Vector3I max = min;
				m_filledVoxels.Remove(min);
				m_selectionList.Remove(min);
				while (flag || flag2 || flag3 || flag4 || flag5 || flag6)
				{
					if (flag)
					{
						flag = ExpandByOnePlusX(ref min, ref max);
					}
					if (flag4)
					{
						flag4 = ExpandByOneMinusX(ref min, ref max);
					}
					if (flag2)
					{
						flag2 = ExpandByOnePlusY(ref min, ref max);
					}
					if (flag5)
					{
						flag5 = ExpandByOneMinusY(ref min, ref max);
					}
					if (flag3)
					{
						flag3 = ExpandByOnePlusZ(ref min, ref max);
					}
					if (flag6)
					{
						flag6 = ExpandByOneMinusZ(ref min, ref max);
					}
				}
				m_segments.Add(new Segment(min, max));
			}
		}

		private bool ExpandByOnePlusX(ref Vector3I min, ref Vector3I max)
		{
			int x = max.X + 1;
			for (int i = min.Y; i <= max.Y; i++)
			{
				for (int j = min.Z; j <= max.Z; j++)
				{
					if (!m_filledVoxels.Contains(new Vector3I(x, i, j)))
					{
						return false;
					}
				}
			}
			max.X = x;
			for (int k = min.Y; k <= max.Y; k++)
			{
				for (int l = min.Z; l <= max.Z; l++)
				{
					m_selectionList.Remove(new Vector3I(x, k, l));
				}
			}
			return true;
		}

		private bool ExpandByOnePlusY(ref Vector3I min, ref Vector3I max)
		{
			int y = max.Y + 1;
			for (int i = min.X; i <= max.X; i++)
			{
				for (int j = min.Z; j <= max.Z; j++)
				{
					if (!m_filledVoxels.Contains(new Vector3I(i, y, j)))
					{
						return false;
					}
				}
			}
			max.Y = y;
			for (int k = min.X; k <= max.X; k++)
			{
				for (int l = min.Z; l <= max.Z; l++)
				{
					m_selectionList.Remove(new Vector3I(k, y, l));
				}
			}
			return true;
		}

		private bool ExpandByOnePlusZ(ref Vector3I min, ref Vector3I max)
		{
			int z = max.Z + 1;
			for (int i = min.X; i <= max.X; i++)
			{
				for (int j = min.Y; j <= max.Y; j++)
				{
					if (!m_filledVoxels.Contains(new Vector3I(i, j, z)))
					{
						return false;
					}
				}
			}
			max.Z = z;
			for (int k = min.X; k <= max.X; k++)
			{
				for (int l = min.Y; l <= max.Y; l++)
				{
					m_selectionList.Remove(new Vector3I(k, l, z));
				}
			}
			return true;
		}

		private bool ExpandByOneMinusX(ref Vector3I min, ref Vector3I max)
		{
			int x = min.X - 1;
			for (int i = min.Y; i <= max.Y; i++)
			{
				for (int j = min.Z; j <= max.Z; j++)
				{
					if (!m_filledVoxels.Contains(new Vector3I(x, i, j)))
					{
						return false;
					}
				}
			}
			min.X = x;
			for (int k = min.Y; k <= max.Y; k++)
			{
				for (int l = min.Z; l <= max.Z; l++)
				{
					m_selectionList.Remove(new Vector3I(x, k, l));
				}
			}
			return true;
		}

		private bool ExpandByOneMinusY(ref Vector3I min, ref Vector3I max)
		{
			int y = min.Y - 1;
			for (int i = min.X; i <= max.X; i++)
			{
				for (int j = min.Z; j <= max.Z; j++)
				{
					if (!m_filledVoxels.Contains(new Vector3I(i, y, j)))
					{
						return false;
					}
				}
			}
			min.Y = y;
			for (int k = min.X; k <= max.X; k++)
			{
				for (int l = min.Z; l <= max.Z; l++)
				{
					m_selectionList.Remove(new Vector3I(k, y, l));
				}
			}
			return true;
		}

		private bool ExpandByOneMinusZ(ref Vector3I min, ref Vector3I max)
		{
			int z = min.Z - 1;
			for (int i = min.X; i <= max.X; i++)
			{
				for (int j = min.Y; j <= max.Y; j++)
				{
					if (!m_filledVoxels.Contains(new Vector3I(i, j, z)))
					{
						return false;
					}
				}
			}
			min.Z = z;
			for (int k = min.X; k <= max.X; k++)
			{
				for (int l = min.Y; l <= max.Y; l++)
				{
					m_selectionList.Remove(new Vector3I(k, l, z));
				}
			}
			return true;
		}
	}
}
