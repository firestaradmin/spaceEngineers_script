using System;
using System.Collections.Generic;

namespace VRage.Library.Collections.Comparers
{
	public sealed class IntPtrComparer : EqualityComparer<IntPtr>
	{
		public static readonly IntPtrComparer Instance = new IntPtrComparer();

		public override bool Equals(IntPtr x, IntPtr y)
		{
			return x == y;
		}

		public override int GetHashCode(IntPtr obj)
		{
			return obj.GetHashCode();
		}
	}
}
