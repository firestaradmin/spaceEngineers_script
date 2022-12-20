using System;
using Sandbox.Definitions;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Cube
{
	public class MyFourEdgeInfo
	{
		private struct Data
		{
			public Vector4 LocalOrthoMatrix;

			public MyCubeEdgeType EdgeType;

			private unsafe fixed uint m_data[4];

			private unsafe fixed byte m_data2[4];

			private MyStringHash m_edgeModel1;

			private MyStringHash m_edgeModel2;

			private MyStringHash m_edgeModel3;

			private MyStringHash m_edgeModel4;

			private MyStringHash m_skinSubtype1;

			private MyStringHash m_skinSubtype2;

			private MyStringHash m_skinSubtype3;

			private MyStringHash m_skinSubtype4;

			public unsafe bool Full
			{
				get
				{
					if (m_data[0] != 0 && m_data[1] != 0 && m_data[2] != 0)
					{
						return m_data[3] != 0;
					}
					return false;
				}
			}

			public unsafe bool Empty
			{
				get
				{
					fixed (uint* ptr = m_data)
					{
						ulong* ptr2 = (ulong*)ptr;
						if (*ptr2 == 0L)
						{
							return ptr2[1] == 0;
						}
						return false;
					}
				}
			}

			public unsafe int Count => ((m_data[0] != 0) ? 1 : 0) + ((m_data[1] != 0) ? 1 : 0) + ((m_data[2] != 0) ? 1 : 0) + ((m_data[3] != 0) ? 1 : 0);

			public unsafe int FirstAvailable
			{
				get
				{
					if (m_data[0] == 0)
					{
						if (m_data[1] == 0)
						{
							if (m_data[2] == 0)
							{
								if (m_data[3] == 0)
								{
									return -1;
								}
								return 3;
							}
							return 2;
						}
						return 1;
					}
					return 0;
				}
			}

			public unsafe uint Get(int index)
			{
				return m_data[index];
			}

			public unsafe void Get(int index, out Color color, out MyStringHash skinSubtypeId, out MyStringHash edgeModel, out Base27Directions.Direction normal0, out Base27Directions.Direction normal1)
			{
				color = new Color(m_data[index]);
				normal0 = (Base27Directions.Direction)color.A;
				normal1 = (Base27Directions.Direction)m_data2[index];
				fixed (MyStringHash* ptr = &m_edgeModel1)
				{
					edgeModel = ptr[index];
				}
				fixed (MyStringHash* ptr2 = &m_skinSubtype1)
				{
					skinSubtypeId = ptr2[index];
				}
			}

			public unsafe bool Set(int index, Color value, MyStringHash skinSubtype, MyStringHash edgeModel, Base27Directions.Direction normal0, Base27Directions.Direction normal1)
			{
				value.A = (byte)normal0;
				uint packedValue = value.PackedValue;
				bool result = false;
				if (m_data[index] != packedValue)
				{
					result = true;
					m_data[index] = packedValue;
				}
				m_data2[index] = (byte)normal1;
				fixed (MyStringHash* ptr = &m_edgeModel1)
				{
					ptr[index] = edgeModel;
				}
				fixed (MyStringHash* ptr2 = &m_skinSubtype1)
				{
					if (ptr2[index] != skinSubtype)
					{
						result = true;
						ptr2[index] = skinSubtype;
					}
				}
				return result;
			}

			public unsafe bool Reset(int index)
			{
				bool result = m_data[index] != 0;
				m_data[index] = 0u;
				fixed (MyStringHash* ptr = &m_edgeModel1)
				{
					ptr[index] = MyStringHash.NullOrEmpty;
				}
				fixed (MyStringHash* ptr = &m_skinSubtype1)
				{
					ptr[index] = MyStringHash.NullOrEmpty;
				}
				return result;
			}
		}

		public const int MaxInfoCount = 4;

		private Data m_data;

		public int Timestamp;

		public Vector4 LocalOrthoMatrix => m_data.LocalOrthoMatrix;

		public MyCubeEdgeType EdgeType => m_data.EdgeType;

		public bool Empty => m_data.Empty;

		public bool Full => m_data.Full;

		public int DebugCount => m_data.Count;

		public int FirstAvailable => m_data.FirstAvailable;

		public MyFourEdgeInfo(Vector4 localOrthoMatrix, MyCubeEdgeType edgeType)
		{
			m_data.LocalOrthoMatrix = localOrthoMatrix;
			m_data.EdgeType = edgeType;
		}

		public bool AddInstance(Vector3 blockPos, Color color, MyStringHash skinSubtype, MyStringHash edgeModel, Base27Directions.Direction normal0, Base27Directions.Direction normal1)
		{
			if (m_data.Set(GetIndex(ref blockPos), color, skinSubtype, edgeModel, normal0, normal1))
			{
				Timestamp++;
				return true;
			}
			return false;
		}

		public bool RemoveInstance(Vector3 blockPos)
		{
			if (m_data.Reset(GetIndex(ref blockPos)))
			{
				Timestamp++;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Based on block position and edge position, calculated block index from 0 to 4 (no more than for blocks shares edge)
		/// </summary>
		private int GetIndex(ref Vector3 blockPos)
		{
			Vector3 vector = blockPos - new Vector3(LocalOrthoMatrix);
			if (Math.Abs(vector.X) < 1E-05f)
			{
				return ((vector.Y > 0f) ? 1 : 0) + ((vector.Z > 0f) ? 2 : 0);
			}
			if (Math.Abs(vector.Y) < 1E-05f)
			{
				return ((vector.X > 0f) ? 1 : 0) + ((vector.Z > 0f) ? 2 : 0);
			}
			return ((vector.X > 0f) ? 1 : 0) + ((vector.Y > 0f) ? 2 : 0);
		}

		public bool GetNormalInfo(int index, out Color color, out MyStringHash skinSubtypeId, out MyStringHash edgeModel, out Base27Directions.Direction normal0, out Base27Directions.Direction normal1)
		{
			m_data.Get(index, out color, out skinSubtypeId, out edgeModel, out normal0, out normal1);
			color.A = 0;
			return normal0 != (Base27Directions.Direction)0;
		}
	}
}
