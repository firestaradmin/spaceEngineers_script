using VRage;
using VRageMath;
using VRageRender.Voxels;

namespace VRageRender
{
	public static class MeshIdExtensions
	{
		internal static int GetIndexCount(this MeshId meshId)
		{
			return MyMeshes.GetLodMesh(meshId, 0).Info.IndicesNum;
		}

		internal static BoundingBox? GetBoundingBox(this MeshId meshId, int lod)
		{
			return MyMeshes.GetLodMesh(meshId, lod).Info.BoundingBox;
		}

		internal static void AssignLodMeshToProxy(this MeshId meshId, MyRenderableProxy proxy)
		{
			proxy.Mesh = MyMeshes.GetLodMesh(meshId, 0);
		}

		internal static MyMeshBuffers GetMeshBuffers(this MeshId meshId)
		{
			return MyMeshes.GetLodMesh(meshId, 0).Buffers;
		}

		internal static bool IsLoaded(this MeshId mesh, int lod = 0)
		{
			return MyMeshes.GetLodMesh(mesh, 0).Buffers.VB0 != null;
		}

		internal static bool ShouldHaveFoliage(this MeshId meshId, bool justGrass)
		{
			int partsNum = MyMeshes.GetLodMesh(meshId, 0).Info.PartsNum;
			bool flag = false;
			for (int i = 0; i < partsNum; i++)
			{
				if (flag)
				{
					break;
				}
				MyVoxelMaterialTriple materialTriple = MyMeshes.GetVoxelPart(meshId, i).Info.MaterialTriple;
				flag = ShouldHaveFoliage(materialTriple.I0, justGrass) || ShouldHaveFoliage(materialTriple.I1, justGrass) || ShouldHaveFoliage(materialTriple.I2, justGrass);
			}
			return flag;
		}

		private static bool ShouldHaveFoliage(byte materialIdx, bool justGrass)
		{
			if (materialIdx < MyVoxelMaterials.Table.Length && MyVoxelMaterials.Table[materialIdx].HasFoliage)
			{
				if (justGrass)
				{
					return MyVoxelMaterials.Table[materialIdx].Foliage.BoxedValue.Type == MyFoliageType.Grass;
				}
				return true;
			}
			return false;
		}
	}
}
