using System.Collections.Generic;

namespace VRageRender
{
	internal struct MyLodMesh
	{
		public class MyLodMeshComparerType : IEqualityComparer<MyLodMesh>
		{
			public bool Equals(MyLodMesh left, MyLodMesh right)
			{
				if (left.Mesh == right.Mesh)
				{
					return left.Lod == right.Lod;
				}
				return false;
			}

			public int GetHashCode(MyLodMesh lodMesh)
			{
				return (ushort)(MeshId.Comparer.GetHashCode() << 16) | (ushort)lodMesh.Lod;
			}
		}

		internal MeshId Mesh;

		internal int Lod;

		public static readonly MyLodMeshComparerType Comparer = new MyLodMeshComparerType();
	}
}
