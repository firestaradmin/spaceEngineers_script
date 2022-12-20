using System.Collections.Generic;

namespace VRageRender
{
	internal struct VoxelPartId
	{
		public class MyVoxelPartIdComparerType : IEqualityComparer<VoxelPartId>
		{
			public bool Equals(VoxelPartId left, VoxelPartId right)
			{
				return left.Index == right.Index;
			}

			public int GetHashCode(VoxelPartId voxelPartId)
			{
				return voxelPartId.Index;
			}
		}

		internal int Index;

		public static readonly MyVoxelPartIdComparerType Comparer = new MyVoxelPartIdComparerType();

		internal static readonly VoxelPartId NULL = new VoxelPartId
		{
			Index = -1
		};

		internal MyVoxelPartInfo1 Info => MyMeshes.VoxelParts.Data[Index];

		public static bool operator ==(VoxelPartId x, VoxelPartId y)
		{
			return x.Index == y.Index;
		}

		public static bool operator !=(VoxelPartId x, VoxelPartId y)
		{
			return x.Index != y.Index;
		}
<<<<<<< HEAD

		public override bool Equals(object obj)
		{
			object obj2;
			if ((obj2 = obj) is VoxelPartId)
			{
				VoxelPartId right = (VoxelPartId)obj2;
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
