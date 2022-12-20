using System.Collections.Generic;

namespace VRageRender
{
	public struct LodMeshId
	{
		public class MyLodMeshIdComparerType : IEqualityComparer<LodMeshId>
		{
			public bool Equals(LodMeshId left, LodMeshId right)
			{
				return left.Index == right.Index;
			}

			public int GetHashCode(LodMeshId lodMeshId)
			{
				return lodMeshId.Index;
			}
		}

		internal int Index;

		internal static readonly MyLodMeshIdComparerType Comparer = new MyLodMeshIdComparerType();

		internal static readonly LodMeshId NULL = new LodMeshId
		{
			Index = -1
		};

		internal MyLodMeshInfo Info => MyMeshes.LodMeshInfos.Data[Index];

		internal MyMeshBuffers Buffers
		{
			get
			{
				if (Index == -1)
				{
					return MyMeshBuffers.Empty;
				}
				return MyMeshes.LodMeshBuffers[Index];
			}
		}

		internal VertexLayoutId VertexLayout => MyMeshes.LodMeshInfos.Data[Index].Data.VertexLayout;

		public static bool operator ==(LodMeshId x, LodMeshId y)
		{
			return x.Index == y.Index;
		}

		public static bool operator !=(LodMeshId x, LodMeshId y)
		{
			return x.Index != y.Index;
		}
<<<<<<< HEAD

		public override bool Equals(object obj)
		{
			object obj2;
			if ((obj2 = obj) is LodMeshId)
			{
				LodMeshId right = (LodMeshId)obj2;
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
