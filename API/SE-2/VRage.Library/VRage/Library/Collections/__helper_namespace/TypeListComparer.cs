using System.Collections.Generic;

namespace VRage.Library.Collections.__helper_namespace
{
	internal class TypeListComparer : IEqualityComparer<ITypeList>
	{
		public bool Equals(ITypeList x, ITypeList y)
		{
			if (x.Count == y.Count)
			{
				for (int i = 0; i < x.Count; i++)
				{
					if (x[i] != y[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public int GetHashCode(ITypeList obj)
		{
			return obj.HashCode;
		}
	}
}
