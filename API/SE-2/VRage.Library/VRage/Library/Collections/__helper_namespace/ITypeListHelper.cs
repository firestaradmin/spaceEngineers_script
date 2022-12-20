using System;

namespace VRage.Library.Collections.__helper_namespace
{
	internal static class ITypeListHelper
	{
		[ThreadStatic]
		private static TypeListWith m_withInstance;

		[ThreadStatic]
		private static TypeListWithout m_withoutInstance;

		public static int ComputeHashCode(this ITypeList self)
		{
			int num = 757602046;
			for (int i = 0; i < self.Count; i++)
			{
				num *= 377;
				num += self[i].GetHashCode();
			}
			return num;
		}

		public static ITypeList With(this TypeList self, int index, Type type)
		{
			TypeListWith typeListWith = m_withInstance ?? (m_withInstance = new TypeListWith());
			typeListWith.Set(self, type, index);
			return typeListWith;
		}

		public static ITypeList Without(this TypeList self, int index)
		{
			TypeListWithout typeListWithout = m_withoutInstance ?? (m_withoutInstance = new TypeListWithout());
			typeListWithout.Set(self, index);
			return typeListWithout;
		}
	}
}
