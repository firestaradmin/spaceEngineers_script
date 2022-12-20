using System;
using System.Collections.Generic;
using System.Linq;

namespace VRage.Library.Collections.__helper_namespace
{
	/// <summary>
	/// Basic type list implementation.
	/// </summary>
	internal class TypeList : List<Type>, ITypeList
	{
		private int m_cachedHashCode;

		public int HashCode => m_cachedHashCode;

		public void UpdateHashCode()
		{
			m_cachedHashCode = this.ComputeHashCode();
		}

		public TypeList GetSolidified()
		{
			return this;
		}

		public override string ToString()
		{
<<<<<<< HEAD
			return string.Join(", ", this.Select((Type x) => x.Name));
=======
			return string.Join(", ", Enumerable.Select<Type, string>((IEnumerable<Type>)this, (Func<Type, string>)((Type x) => x.Name)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
