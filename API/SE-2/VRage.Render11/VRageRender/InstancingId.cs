using System.Collections.Generic;
using System.Text;
using VRage.Render11.Resources;

namespace VRageRender
{
	internal struct InstancingId
	{
		public class MyInstancingIdComparerType : IEqualityComparer<InstancingId>
		{
			public bool Equals(InstancingId left, InstancingId right)
			{
				return left.Index == right.Index;
			}

			public int GetHashCode(InstancingId instancingId)
			{
				return instancingId.Index;
			}
		}

		internal int Index;

		internal static readonly InstancingId NULL = new InstancingId
		{
			Index = -1
		};

		public static readonly MyInstancingIdComparerType Comparer = new MyInstancingIdComparerType();

		private static StringBuilder m_stringHelper = new StringBuilder();

		internal MyInstancingInfo Info => MyInstancing.Instancings.Data[Index];

		internal IVertexBuffer VB => MyInstancing.Data[Index].VB;

		public static bool operator ==(InstancingId x, InstancingId y)
		{
			return x.Index == y.Index;
		}

		public static bool operator !=(InstancingId x, InstancingId y)
		{
			return x.Index != y.Index;
		}

<<<<<<< HEAD
		public override bool Equals(object obj)
		{
			object obj2;
			if ((obj2 = obj) is InstancingId)
			{
				InstancingId right = (InstancingId)obj2;
				return Comparer.Equals(this, right);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Comparer.GetHashCode();
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override string ToString()
		{
			m_stringHelper.Clear();
			return m_stringHelper.AppendInt32(Index).ToString();
		}
	}
}
