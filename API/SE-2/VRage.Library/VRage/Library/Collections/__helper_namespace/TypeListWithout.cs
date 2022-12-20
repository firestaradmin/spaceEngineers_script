using System;
<<<<<<< HEAD
=======
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Linq;

namespace VRage.Library.Collections.__helper_namespace
{
	/// <summary>
	/// Type list with a single element added.
	/// </summary>
	internal class TypeListWithout : ITypeList
	{
		private TypeList m_list;

		private int m_index;

		public int Count => m_list.Count - 1;

		public Type this[int index]
		{
			get
			{
				if (index < m_index)
				{
					return m_list[index];
				}
				return m_list[index + 1];
			}
		}

		public int HashCode { get; private set; }

		public void Set(TypeList source, int removalIndex)
		{
			m_list = source;
			m_index = removalIndex;
			HashCode = this.ComputeHashCode();
		}

		public TypeList GetSolidified()
		{
			TypeList typeList = new TypeList();
			typeList.Capacity = Count;
<<<<<<< HEAD
			typeList.AddRange(m_list.Take(m_index));
			typeList.AddRange(m_list.Skip(m_index + 1));
=======
			typeList.AddRange(Enumerable.Take<Type>((IEnumerable<Type>)m_list, m_index));
			typeList.AddRange(Enumerable.Skip<Type>((IEnumerable<Type>)m_list, m_index + 1));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			typeList.UpdateHashCode();
			return typeList;
		}
	}
}
