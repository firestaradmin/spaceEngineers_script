using System.Collections.Generic;

namespace VRageRender
{
	internal struct MeshPartId
	{
		public class MyMeshPartIdComparerType : IEqualityComparer<MeshPartId>
		{
			public bool Equals(MeshPartId left, MeshPartId right)
			{
				return left.Index == right.Index;
			}

			public int GetHashCode(MeshPartId meshPartId)
			{
				return meshPartId.Index;
			}
		}

		internal int Index;

		public static readonly MyMeshPartIdComparerType Comparer = new MyMeshPartIdComparerType();

		internal static readonly MeshPartId NULL = new MeshPartId
		{
			Index = -1
		};

		internal MyMeshPartInfo1 Info => MyMeshes.Parts.Data[Index];

		public static bool operator ==(MeshPartId x, MeshPartId y)
		{
			return x.Index == y.Index;
		}

		public static bool operator !=(MeshPartId x, MeshPartId y)
		{
			return x.Index != y.Index;
		}
<<<<<<< HEAD

		public override bool Equals(object obj)
		{
			object obj2;
			if ((obj2 = obj) is MeshPartId)
			{
				MeshPartId right = (MeshPartId)obj2;
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
	}
}
