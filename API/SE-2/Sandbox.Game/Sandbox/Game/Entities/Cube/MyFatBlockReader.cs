using System;
using System.Collections;
using System.Collections.Generic;
using VRage.Game.ModAPI;

namespace Sandbox.Game.Entities.Cube
{
	public struct MyFatBlockReader<TBlock> : IEnumerator<TBlock>, IEnumerator, IDisposable, IEnumerable<TBlock>, IEnumerable where TBlock : class, IMyCubeBlock
	{
		private Enumerator<MySlimBlock> m_enumerator;

<<<<<<< HEAD
		public TBlock Current => m_enumerator.Current.FatBlock as TBlock;
=======
		public TBlock Current => (TBlock)m_enumerator.get_Current().FatBlock;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		object IEnumerator.Current => Current;

		public MyFatBlockReader(MyCubeGrid grid)
			: this(grid.GetBlocks().GetEnumerator())
		{
		}//IL_0007: Unknown result type (might be due to invalid IL or missing references)


		public MyFatBlockReader(HashSet<MySlimBlock> set)
			: this(set.GetEnumerator())
		{
		}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


		public MyFatBlockReader(Enumerator<MySlimBlock> enumerator)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			m_enumerator = enumerator;
		}

		public void Dispose()
		{
			m_enumerator.Dispose();
		}

		public bool MoveNext()
		{
			while (m_enumerator.MoveNext())
			{
				if (m_enumerator.get_Current().FatBlock as TBlock != null)
				{
					return true;
				}
			}
			return false;
		}

		public void Reset()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			IEnumerator<MySlimBlock> enumerator = (IEnumerator<MySlimBlock>)(object)m_enumerator;
			enumerator.Reset();
			m_enumerator = (Enumerator<MySlimBlock>)(object)enumerator;
		}

		public IEnumerator<TBlock> GetEnumerator()
		{
			return this;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this;
		}
	}
}
