using System.Collections.Generic;

namespace VRageRender
{
	internal struct MyMeshSection
	{
		public class MyMeshSectionComparerType : IEqualityComparer<MyMeshSection>
		{
			public bool Equals(MyMeshSection left, MyMeshSection right)
			{
				if (left.Mesh == right.Mesh && left.Lod == right.Lod)
				{
					return left.Section == right.Section;
				}
				return false;
			}

			public int GetHashCode(MyMeshSection section)
			{
				return (section.Mesh.GetHashCode() << 20) | (section.Lod << 10) | section.Section.GetHashCode();
			}
		}

		internal MeshId Mesh;

		internal int Lod;

		internal string Section;

		public static readonly MyMeshSectionComparerType Comparer = new MyMeshSectionComparerType();
	}
}
