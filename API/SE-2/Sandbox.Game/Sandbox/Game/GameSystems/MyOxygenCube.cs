using System.Collections.Concurrent;
using System.Collections.Generic;
using VRageMath;

namespace Sandbox.Game.GameSystems
{
	public class MyOxygenCube
	{
		private class Cell
		{
			public MyOxygenBlock[] Data;
		}

		private readonly Vector3I m_defaultCellSize = new Vector3I(10, 10, 10);

		private readonly Vector3I m_defaultBaseOffset = new Vector3I(5, 5, 5);

		private Vector3I m_cellSize;

		private Vector3I m_baseOffset;

		private ConcurrentDictionary<Vector3I, MyOxygenBlock[]> m_dictionary;

		public MyOxygenBlock this[int x, int y, int z]
		{
			get
			{
				TryGetValue(new Vector3I(x, y, z), out var value);
				return value;
			}
			set
			{
				Add(new Vector3I(x, y, z), value);
			}
		}

		public MyOxygenCube()
		{
			m_cellSize = m_defaultCellSize;
			m_baseOffset = m_defaultBaseOffset;
			m_dictionary = new ConcurrentDictionary<Vector3I, MyOxygenBlock[]>((IEqualityComparer<Vector3I>)new Vector3I.EqualityComparer());
		}

		public MyOxygenCube(Vector3I cellSize)
		{
			m_cellSize = cellSize;
			m_baseOffset = cellSize / 2;
			m_dictionary = new ConcurrentDictionary<Vector3I, MyOxygenBlock[]>((IEqualityComparer<Vector3I>)new Vector3I.EqualityComparer());
		}

		public void Add(Vector3I key, MyOxygenBlock value)
		{
			GetCellPosition(key, out var cellPosition);
<<<<<<< HEAD
			if (!m_dictionary.TryGetValue(cellPosition, out var value2))
=======
			MyOxygenBlock[] array = default(MyOxygenBlock[]);
			if (!m_dictionary.TryGetValue(cellPosition, ref array))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				array = new MyOxygenBlock[m_cellSize.Volume()];
				m_dictionary.TryAdd(cellPosition, array);
			}
			Vector3I vector3I = key - cellPosition;
			int num = vector3I.X + vector3I.Y * m_cellSize.X + vector3I.Z * m_cellSize.X * m_cellSize.Y;
			array[num] = value;
		}

		private void GetCellPosition(Vector3I key, out Vector3I cellPosition)
		{
			if (-m_baseOffset.X > key.X)
			{
				key.X -= m_cellSize.X - 1;
			}
			if (-m_baseOffset.Y > key.Y)
			{
				key.Y -= m_cellSize.Y - 1;
			}
			if (-m_baseOffset.Z > key.Z)
			{
				key.Z -= m_cellSize.Z - 1;
			}
			Vector3I vector3I = (key + m_baseOffset) / m_cellSize;
			cellPosition = m_baseOffset + (vector3I - 1) * m_cellSize;
		}

		public bool TryGetValue(Vector3I key, out MyOxygenBlock value)
		{
			GetCellPosition(key, out var cellPosition);
<<<<<<< HEAD
			if (!m_dictionary.TryGetValue(cellPosition, out var value2))
=======
			MyOxygenBlock[] array = default(MyOxygenBlock[]);
			if (!m_dictionary.TryGetValue(cellPosition, ref array))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				value = null;
				return false;
			}
			Vector3I vector3I = key - cellPosition;
			int num = vector3I.X + vector3I.Y * m_cellSize.X + vector3I.Z * m_cellSize.X * m_cellSize.Y;
			value = array[num];
			if (value == null)
			{
				return false;
			}
			return true;
		}
	}
}
