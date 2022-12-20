using Sandbox.Engine.Voxels;
using VRage.Game;
using VRage.Voxels;
using VRageMath;

namespace Sandbox.Game.Entities
{
	public static class VoxelBaseExtensions
	{
		public static MyVoxelMaterialDefinition GetMaterialAt(this MyVoxelBase self, ref Vector3D worldPosition)
		{
			if (self.Storage == null)
			{
				return null;
			}
			MyVoxelCoordSystems.WorldPositionToLocalPosition(worldPosition, self.PositionComp.WorldMatrixRef, self.PositionComp.WorldMatrixInvScaled, self.SizeInMetresHalf, out var localPosition);
			Vector3I voxelCoords = new Vector3I(localPosition / 1f) + self.StorageMin;
			return self.Storage.GetMaterialAt(ref voxelCoords);
		}
	}
}
