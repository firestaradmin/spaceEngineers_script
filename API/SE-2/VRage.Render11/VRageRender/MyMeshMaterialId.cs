using System.Collections.Generic;

namespace VRageRender
{
	public struct MyMeshMaterialId
	{
		public class CustomMergingEqualityComparer : IEqualityComparer<MyMeshMaterialId>
		{
			public bool Equals(MyMeshMaterialId x, MyMeshMaterialId y)
			{
				MyMeshMaterialInfo info = x.Info;
				MyMeshMaterialInfo info2 = y.Info;
				if (info.ColorMetal_Texture == info2.ColorMetal_Texture && info.NormalGloss_Texture == info2.NormalGloss_Texture && info.Alphamask_Texture == info2.Alphamask_Texture && info.Extensions_Texture == info2.Extensions_Texture && info.TextureTypes == info2.TextureTypes && info.Technique == info2.Technique)
				{
					return info.Facing == info2.Facing;
				}
				return false;
			}

			public int GetHashCode(MyMeshMaterialId obj)
			{
				return 1;
			}
		}

<<<<<<< HEAD
		public static CustomMergingEqualityComparer Comparer = new CustomMergingEqualityComparer();

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		internal int Index;

		internal static readonly MyMeshMaterialId NULL = new MyMeshMaterialId
		{
			Index = -1
		};

		internal MyMeshMaterialInfo Info => MyMeshMaterials1.Table[Index];

		public static bool operator ==(MyMeshMaterialId x, MyMeshMaterialId y)
		{
			return x.Index == y.Index;
		}

		public static bool operator !=(MyMeshMaterialId x, MyMeshMaterialId y)
		{
			return x.Index != y.Index;
		}
<<<<<<< HEAD

		public override bool Equals(object obj)
		{
			object obj2;
			if ((obj2 = obj) is MyMeshMaterialId)
			{
				MyMeshMaterialId myMeshMaterialId = (MyMeshMaterialId)obj2;
				return Index == myMeshMaterialId.Index;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Index.GetHashCode();
		}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
