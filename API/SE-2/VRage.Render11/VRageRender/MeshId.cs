using System.Collections.Generic;

namespace VRageRender
{
	public struct MeshId
	{
		public class MyMeshIdComparerType : IEqualityComparer<MeshId>
		{
			public bool Equals(MeshId left, MeshId right)
			{
				return left.Index == right.Index;
			}

			public int GetHashCode(MeshId meshId)
			{
				return meshId.Index;
			}
		}

		internal int Index;

		public static readonly MyMeshIdComparerType Comparer = new MyMeshIdComparerType();

		internal static readonly MeshId NULL = new MeshId
		{
			Index = -1
		};

		internal MyMeshInfo Info => MyMeshes.MeshInfos.Data[Index];

		public static bool operator ==(MeshId x, MeshId y)
		{
			return x.Index == y.Index;
		}

		public static bool operator !=(MeshId x, MeshId y)
		{
			return x.Index != y.Index;
		}
<<<<<<< HEAD

		public override bool Equals(object obj)
		{
			object obj2;
			if ((obj2 = obj) is MeshId)
			{
				MeshId right = (MeshId)obj2;
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
