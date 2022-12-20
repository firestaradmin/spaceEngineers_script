using System.Collections.Generic;

namespace VRageRender
{
	internal struct MeshSectionId
	{
		public class MyMeshSectionIdComparerType : IEqualityComparer<MeshSectionId>
		{
			public bool Equals(MeshSectionId left, MeshSectionId right)
			{
				return left.Index == right.Index;
			}

			public int GetHashCode(MeshSectionId sectionId)
			{
				return sectionId.Index;
			}
		}

		internal int Index;

		public static readonly MyMeshSectionIdComparerType Comparer = new MyMeshSectionIdComparerType();

		internal static readonly MeshSectionId NULL = new MeshSectionId
		{
			Index = -1
		};

		internal MyMeshSectionInfo1 Info => MyMeshes.Sections.Data[Index];

		public static bool operator ==(MeshSectionId x, MeshSectionId y)
		{
			return x.Index == y.Index;
		}

		public static bool operator !=(MeshSectionId x, MeshSectionId y)
		{
			return x.Index != y.Index;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is MeshSectionId))
			{
				return false;
			}
			MeshSectionId meshSectionId = (MeshSectionId)obj;
			return Index == meshSectionId.Index;
		}

		public override int GetHashCode()
		{
			return Index;
		}
	}
}
