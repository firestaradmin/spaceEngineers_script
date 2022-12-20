using System.Collections.Generic;

namespace VRageRender
{
	internal struct MyMeshPart
	{
		public class MyMeshPartComparerType : IEqualityComparer<MyMeshPart>
		{
			public bool Equals(MyMeshPart left, MyMeshPart right)
			{
				if (left.Mesh == right.Mesh && left.Lod == right.Lod)
				{
					return left.Part == right.Part;
				}
				return false;
			}

			public int GetHashCode(MyMeshPart meshPart)
			{
				return (meshPart.Mesh.GetHashCode() << 20) | (meshPart.Lod << 10) | meshPart.Part;
			}
		}

		internal MeshId Mesh;

		internal int Lod;

		internal int Part;

		public static readonly MyMeshPartComparerType Comparer = new MyMeshPartComparerType();
	}
}
