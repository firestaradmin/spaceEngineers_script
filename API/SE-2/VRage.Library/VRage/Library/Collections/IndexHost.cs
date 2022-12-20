using System;
using System.Collections.Generic;
using VRage.Library.Collections.__helper_namespace;

namespace VRage.Library.Collections
{
	/// <summary>
	/// Host of static references to indexers.
	/// </summary>
	public class IndexHost
	{
		private static readonly ComponentIndex NullIndex = new ComponentIndex(new TypeList());

		private readonly Dictionary<ITypeList, WeakReference> m_indexes;

		public IndexHost()
		{
			m_indexes = new Dictionary<ITypeList, WeakReference>(new TypeListComparer());
			m_indexes[NullIndex.Types] = new WeakReference(NullIndex);
		}

		private ComponentIndex GetForTypes(ITypeList types)
		{
			ComponentIndex result;
			if (!m_indexes.TryGetValue(types, out var value) || (result = (ComponentIndex)value.Target) == null)
			{
				if (value == null)
				{
					value = new WeakReference(null);
				}
				TypeList solidified = types.GetSolidified();
				result = (ComponentIndex)(value.Target = new ComponentIndex(solidified));
				m_indexes[solidified] = value;
			}
			return result;
		}

		public ComponentIndex GetAfterInsert(ComponentIndex current, Type newType, out int insertionPoint)
		{
			insertionPoint = ~current.Types.BinarySearch(newType, TypeComparer.Instance);
			ITypeList types = current.Types.With(insertionPoint, newType);
			return GetForTypes(types);
		}

		public ComponentIndex GetAfterRemove(ComponentIndex current, Type oldType, out int removalPoint)
		{
			removalPoint = current.Index[oldType];
			ITypeList types = current.Types.Without(removalPoint);
			return GetForTypes(types);
		}

		public ComponentIndex GetEmptyComponentIndex()
		{
			return NullIndex;
		}
	}
}
