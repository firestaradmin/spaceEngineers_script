using System;
using System.Collections.Generic;
using VRage.ModAPI;
using VRageMath;

namespace VRage.Game.ModAPI
{
	public interface IMyVoxelMaps
	{
		int VoxelMaterialCount { get; }

		void Clear();

		bool Exist(IMyVoxelBase voxelMap);

		IMyVoxelBase GetOverlappingWithSphere(ref BoundingSphereD sphere);

		IMyVoxelBase GetVoxelMapWhoseBoundingBoxIntersectsBox(ref BoundingBoxD boundingBox, IMyVoxelBase ignoreVoxelMap);

		void GetInstances(List<IMyVoxelBase> outInstances, Func<IMyVoxelBase, bool> collect = null);

		IMyStorage CreateStorage(Vector3I size);

		IMyStorage CreateStorage(byte[] data);

		IMyVoxelMap CreateVoxelMap(string storageName, IMyStorage storage, Vector3D position, long voxelMapId);

		/// <summary>
		/// Adds a prefab voxel to the game world.
		/// </summary>
		/// <param name="storageName">The name of which the voxel storage will be called within the world.</param>
		/// <param name="prefabVoxelMapName">The prefab voxel to add.</param>
		/// <param name="position">The Min corner position of the voxel within the world.</param>
		/// <returns>The newly added voxel map. Returns null if the prefabVoxelMapName does not exist.</returns>
		IMyVoxelMap CreateVoxelMapFromStorageName(string storageName, string prefabVoxelMapName, Vector3D position);

		IMyVoxelShapeBox GetBoxVoxelHand();

		IMyVoxelShapeCapsule GetCapsuleVoxelHand();

		IMyVoxelShapeSphere GetSphereVoxelHand();

		IMyVoxelShapeRamp GetRampVoxelHand();

		/// <summary>
		/// Will paint given material with given shape
		/// </summary>
		/// <param name="voxelMap"></param>
		/// <param name="voxelShape"></param>
		/// <param name="materialIdx"></param>
		void PaintInShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape, byte materialIdx);

		/// <summary>
		/// Will cut out given shape
		/// </summary>
		/// <param name="voxelMap"></param>
		/// <param name="voxelShape"></param>
		void CutOutShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape);

		/// <summary>
		/// Will fill given material with given shape
		/// </summary>
		/// <param name="voxelMap"></param>
		/// <param name="voxelShape"></param>
		/// <param name="materialIdx"></param>
		void FillInShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape, byte materialIdx);

		void RevertShape(IMyVoxelBase voxelMap, IMyVoxelShape voxelShape);

		void MakeCrater(IMyVoxelBase voxelMap, BoundingSphereD sphere, Vector3 direction, byte materialIdx);

		/// <summary>
		/// Creates a Planet Entity
		/// </summary>
		/// <param name="planetName">SubtypeId of the Planet (eg: Earthlike, Moon, Mars, etc)</param>
		/// <param name="size">Diameter (in meters) of the planet</param>
		/// <param name="seed">Voxel generation seed (similar to the slider you would set in Shift+F10 menu)</param>
		/// <param name="position">Position of where the planet is placed (placed using PositionLeftBottomCorner)</param>
		/// <returns>Planet as IMyVoxelBase</returns>
		IMyVoxelBase SpawnPlanet(string planetName, float size, int seed, Vector3D position);

		/// <summary>
		/// Creates an voxel map with some additional options.
		/// </summary>
		/// <param name="storageName">SubtypeId of a VoxelMap Definition</param>
		/// <param name="voxelMaterial">If provided with a Voxel Material SubtypeId, the entire voxel map will be converted to that material. Otherwise, if this value is Empty or Null, the default materials for the voxel map will be used.</param>
		/// <param name="matrix">World Matrix used for positioning voxel map</param>
		/// <param name="useVoxelOffset">If set to true, the voxel will be placed using the center of the voxel bounding box, instead of using PositionLeftBottomCorner</param>
		/// <returns>A voxel map (eg: asteroid) as IMyVoxelMap</returns>
		IMyVoxelMap CreatePredefinedVoxelMap(string storageName, string voxelMaterial, MatrixD matrix, bool useVoxelOffset);

		IMyVoxelMap CreateProceduralVoxelMap(int seed, float radius, MatrixD matrix);
	}
}
