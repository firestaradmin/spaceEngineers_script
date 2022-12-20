using System;
using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Declares grid interface. (mods interface)
	/// Grid - an entity that consist of <see cref="T:VRage.Game.ModAPI.IMySlimBlock" />.
	/// Blocks like rotor, piston, hinge, motor suspension, on their creation creates top part, that belongs to another grid.
	/// Player created ships, can consist of many grids.
	/// </summary>
	public interface IMyCubeGrid : VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.Game.ModAPI.Ingame.IMyCubeGrid
	{
		/// <summary>
		/// List of players with majority of blocks on grid
		/// </summary>
		List<long> BigOwners { get; }

		/// <summary>
		/// List of players with any blocks on grid
		/// </summary>
		List<long> SmallOwners { get; }

		/// <summary>
		/// Gets or sets if this grid is a respawn grid (can be cleaned up automatically when player leaves)
		/// </summary>
		bool IsRespawnGrid { get; set; }

		/// <summary>
		/// Gets or sets if the grid is static (station)
		/// </summary>
		/// <remarks>Be careful not to set it on stations which are embedded in voxels!</remarks>
		new bool IsStatic { get; set; }

		/// <summary>
		/// Display name of the grid (as seen in Info terminal tab)
		/// </summary>
		new string CustomName { get; set; }

		/// <summary>
		/// Gets or sets X-Axis build symmetry plane
		/// </summary>
		Vector3I? XSymmetryPlane { get; set; }

		/// <summary>
		/// Gets or sets Y-Axis build symmetry plane
		/// </summary>
		Vector3I? YSymmetryPlane { get; set; }

		/// <summary>
		/// Gets or sets Z-Axis build symmetry plane
		/// </summary>
		Vector3I? ZSymmetryPlane { get; set; }

		/// <summary>
		/// Gets or sets if the symmetry plane is offset from the block center
		/// </summary>
		/// <remarks>True if symmetry plane is at block border; false if center of block</remarks>
		bool XSymmetryOdd { get; set; }

		/// <summary>
		/// Gets or sets if the symmetry plane is offset from the block center
		/// </summary>
		/// <remarks>True if symmetry plane is at block border; false if center of block</remarks>
		bool YSymmetryOdd { get; set; }

		/// <summary>
		/// Gets or sets if the symmetry plane is offset from the block center
		/// </summary>
		/// <remarks>True if symmetry plane is at block border; false if center of block</remarks>
		bool ZSymmetryOdd { get; set; }

		/// <summary>
		/// Gets grid presence tier
		/// </summary>
		MyUpdateTiersGridPresence GridPresenceTier { get; }

		/// <summary>
		/// Gets player presence tier
		/// </summary>
		MyUpdateTiersPlayerPresence PlayerPresenceTier { get; }
<<<<<<< HEAD

		/// <summary>
		/// Gets grid-group weapon system
		/// </summary>
		IMyGridWeaponSystem WeaponSystem { get; }

		/// <summary>
		/// Gets grid-group control system
		/// </summary>
		IMyGridControlSystem ControlSystem { get; }

		/// <summary>
		/// Gets grid-group gas system
		/// </summary>
		IMyGridGasSystem GasSystem { get; }

		/// <summary>
		/// Gets grid-group jump system
		/// </summary>
		IMyGridJumpDriveSystem JumpSystem { get; }

		/// <summary>
		/// Gets grid-group resource distributor
		/// </summary>
		IMyResourceDistributorComponent ResourceDistributor { get; }

		/// <summary>
		/// Gets grid-group conveyor system
		/// </summary>
		IMyGridConveyorSystem ConveyorSystem { get; }
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		/// <summary>
		/// Called when a block is added to the grid
		/// </summary>
		event Action<IMySlimBlock> OnBlockAdded;

		/// <summary>
		/// Called when a block is removed from the grid
		/// </summary>
		event Action<IMySlimBlock> OnBlockRemoved;

		/// <summary>
		/// Called when a block on the grid changes owner
		/// </summary>
		event Action<IMyCubeGrid> OnBlockOwnershipChanged;

		/// <summary>
		/// Called when a grid is taken control of by a player
		/// </summary>
		event Action<IMyCubeGrid> OnGridChanged;

		/// <summary>
		/// Triggered when grid is split
		/// </summary>
		event Action<IMyCubeGrid, IMyCubeGrid> OnGridSplit;

		/// <summary>
		/// Triggered when grid changes to or from static (station)
		/// </summary>
		event Action<IMyCubeGrid, bool> OnIsStaticChanged;

		/// <summary>
		/// Triggered when block integrity changes (construction)
		/// </summary>
		event Action<IMySlimBlock> OnBlockIntegrityChanged;

		/// <summary>
		/// Triggered when grid presence tier is changed
		/// </summary>
		event Action<IMyCubeGrid> GridPresenceTierChanged;

		/// <summary>
		/// Triggered when player presence tier is changed
		/// </summary>
		event Action<IMyCubeGrid> PlayerPresenceTierChanged;

		/// <summary>
<<<<<<< HEAD
		/// Called, when 2 grids are merged with merge block. First grid - grid that will stay, second - will be merged into first, and deleted.
		/// Called for both grids
		/// </summary>
		event Action<IMyCubeGrid, IMyCubeGrid> OnGridMerge;

		/// <summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		/// Applies random deformation to given block
		/// </summary>
		/// <param name="block">block to be deformed</param>
		void ApplyDestructionDeformation(IMySlimBlock block);

		/// <summary>
		/// Changes owner of all blocks on grid
		/// Call only on server!
		/// </summary>
		/// <param name="playerId">new owner id (IdentityId)</param>
		/// <param name="shareMode">new share mode</param>
		void ChangeGridOwnership(long playerId, MyOwnershipShareModeEnum shareMode);

		/// <summary>
		/// Clears symmetry planes
		/// </summary>
		void ClearSymmetries();

		/// <summary>
		/// Sets given color mask to range of blocks
		/// </summary>
		/// <param name="min">Starting coordinates of colored area</param>
		/// <param name="max">End coordinates of colored area</param>
		/// <param name="newHSV">new color mask (Saturation and Value are offsets)</param>
		void ColorBlocks(Vector3I min, Vector3I max, Vector3 newHSV);

		/// <summary>
		/// Sets given skin to range of blocks
		/// </summary>
		/// <param name="min">Starting coordinates of skinned area</param>
		/// <param name="max">End coordinates of skinned area</param>
		/// <param name="newHSV">new color mask (Saturation and Value are offsets)</param>
		/// <param name="newSkin">subtype of the new skin</param>
		void SkinBlocks(Vector3I min, Vector3I max, Vector3? newHSV, string newSkin);

		/// <summary>
		/// Converts station to ship
		/// </summary>
		[Obsolete("Use IMyCubeGrid.Static instead.")]
		void OnConvertToDynamic();

		/// <summary>
		/// Clamps fractional grid position to nearest cell (prefers neighboring occupied cell before empty) 
		/// </summary>
		/// <param name="cube">Return value</param>
		/// <param name="fractionalGridPosition">Fractional position in grid space</param>
		void FixTargetCube(out Vector3I cube, Vector3 fractionalGridPosition);

		/// <summary>
		/// Gets position of closest cell corner
		/// </summary>
		/// <param name="gridPos">Cell coordinates</param>
		/// <param name="position">Position to find nearest corner to. Grid space</param>
		/// <returns>Fractional position of corner in grid space</returns>
		Vector3 GetClosestCorner(Vector3I gridPos, Vector3 position);

		/// <summary>
		/// Get cube block at given position
		/// </summary>
		/// <param name="pos">Block position</param>
		/// <returns>Block or null if none is present at given position</returns>
		new IMySlimBlock GetCubeBlock(Vector3I pos);

		/// <summary>
		/// Returns point of intersection with line
		/// </summary>
		/// <param name="line">Intersecting line</param>
		/// <param name="distance">Distance of intersection</param>
		/// <param name="intersectedBlock">Returns intersected block, or null if there was no intersection</param>
		/// <returns>Point of intersection</returns>
		Vector3D? GetLineIntersectionExactAll(ref LineD line, out double distance, out IMySlimBlock intersectedBlock);

		/// <summary>
		/// Same as GetLineIntersectionExactAll just without intersected block
		/// </summary>
		/// <param name="line">Line, that should intersect any block</param>
		/// <param name="position">Position of block, intersected with line would be returned into that variable</param>
		/// <param name="distanceSquared">Squared distance of intersection would be returned into that variable</param>
		bool GetLineIntersectionExactGrid(ref LineD line, ref Vector3I position, ref double distanceSquared);

		/// <summary>
		/// Finds out if given area has any neighboring block
		/// Checking only cube sides. Example: for vectors Min (0,0,0) and Max (10,10,10), Space (1,1,1)-(9,9,9) won't be checked
		/// </summary>
		/// <param name="min">Minimal position (in block coordinates)</param>
		/// <param name="max">Maximum position (in block coordinates)</param>
		/// <returns>True if given area has at least one block</returns>
		bool IsTouchingAnyNeighbor(Vector3I min, Vector3I max);

		/// <summary>
		/// Determines if merge between grids is possible with given offset
		/// </summary>
		/// <param name="gridToMerge"></param>
		/// <param name="gridOffset">offset to merged grid (in grid space)</param>
		/// <returns>True whether grids could be merged</returns>
		/// <remarks>Can be extremely slow on large grids, that can't be merged</remarks>
		bool CanMergeCubes(IMyCubeGrid gridToMerge, Vector3I gridOffset);

		/// <summary>
		/// Transformation matrix that has to be applied to grid blocks to correctly merge it
		/// used because ie. ships can be turned 90 degrees along X axis when being merged
		/// </summary>
		/// <param name="gridToMerge"></param>
		/// <param name="gridOffset"></param>
		/// <returns></returns>
		MatrixI CalculateMergeTransform(IMyCubeGrid gridToMerge, Vector3I gridOffset);

		/// <summary>
		/// Merge used by merge blocks
		/// </summary>
		/// <param name="gridToMerge"></param>
		/// <param name="gridOffset">Block position </param>
		/// <returns></returns>
		IMyCubeGrid MergeGrid_MergeBlock(IMyCubeGrid gridToMerge, Vector3I gridOffset);

		/// <summary>
		/// Obtains first block position intersected with line.
		/// </summary>
		/// <param name="worldStart">Start position in world coordinates</param>
		/// <param name="worldEnd">End position in world coordinates</param>
		/// <returns>Position of found block or null</returns>
		Vector3I? RayCastBlocks(Vector3D worldStart, Vector3D worldEnd);

		/// <summary>
		/// Obtains positions of grid cells <b>regardless</b> of whether these cells are taken up by blocks or not.
		/// </summary>
		/// <param name="worldStart">Start position in world coordinates</param>
		/// <param name="worldEnd">End position in world coordinates</param>
		/// <param name="outHitPositions">List of found block positions</param>
		/// <param name="gridSizeInflate">If not null, would allow find blocks out </param>
		/// <param name="havokWorld">Obsolete, not used.</param>
		void RayCastCells(Vector3D worldStart, Vector3D worldEnd, List<Vector3I> outHitPositions, Vector3I? gridSizeInflate = null, bool havokWorld = false);

		/// <summary>
		/// Remove block at given position
		/// </summary>
		/// <param name="position">Position of block (in block coordinates)</param>
		void RazeBlock(Vector3I position);

		/// <summary>
		/// Remove blocks in given area
		/// </summary>
		/// <param name="pos">Starting position</param>
		/// <param name="size">Area extents</param>
		void RazeBlocks(ref Vector3I pos, ref Vector3UByte size);

		/// <summary>
		/// Remove blocks at given positions
		/// </summary>
		void RazeBlocks(List<Vector3I> locations);

		/// <summary>
		/// Removes given block
		/// </summary>
		/// <param name="block">Block, that you want to remove</param>
		/// <param name="updatePhysics">Update grid physics</param>
		void RemoveBlock(IMySlimBlock block, bool updatePhysics = false);

		/// <summary>
		/// Removes block and deformates neighboring blocks
		/// </summary>
		/// <param name="block">Block, that you want to remove</param>
		void RemoveDestroyedBlock(IMySlimBlock block);

		/// <summary>
		/// Refreshes block neighbors (checks connections)
		/// </summary>
		/// <param name="block">Block, whose neighbours need to be updated</param>
		void UpdateBlockNeighbours(IMySlimBlock block);

		/// <summary>
		/// Returns blocks in grid
		/// </summary>
		/// <param name="blocks">List of returned blocks. Can be null, if function always returns false</param>
		/// <param name="collect">Filter - function called on each block telling if it should be added to result. When it is null, all blocks are added</param>
		void GetBlocks(List<IMySlimBlock> blocks, Func<IMySlimBlock, bool> collect = null);

		/// <summary>
		/// Returns blocks intersects with given sphere (world space)
		/// </summary>
		/// <returns>List of blocks, that intersects with given sphere</returns>
		List<IMySlimBlock> GetBlocksInsideSphere(ref BoundingSphereD sphere);

		/// <summary>
		/// Called when functional block lost or gained owner.
		/// Triggers, grid ownership recalculation 
		/// </summary>
		/// <param name="ownerId">New or previous block owner</param>
		/// <param name="isFunctional">New functional state of block.</param>
		void UpdateOwnership(long ownerId, bool isFunctional);

		/// <summary>
		/// Add a cubeblock to the grid
		/// </summary>
		/// <param name="objectBuilder">Object builder of cube to add</param>
		/// <param name="testMerge">test for grid merging</param>
		/// <returns>Created block</returns>
		IMySlimBlock AddBlock(MyObjectBuilder_CubeBlock objectBuilder, bool testMerge);

		/// <summary>
		/// Checks if removing a block will cause the grid to split
		/// </summary>
		/// <param name="testBlock"></param>
		/// <returns>True if removing block, would split grid</returns>
		bool WillRemoveBlockSplitGrid(IMySlimBlock testBlock);

		/// <summary>
		/// Tests if a cubeblock can be added at the specific location
		/// </summary>
		/// <param name="pos">Position where you want add cube</param>
		/// <returns><b>true</b> if block can be added</returns>
		bool CanAddCube(Vector3I pos);

		/// <summary>
		/// Test if the range of positions are not occupied by any blocks
		/// </summary>
		/// <param name="min">Start position</param>
		/// <param name="max">End position</param>
		/// <returns><b>true</b> if blocks can be added in that range</returns>
		bool CanAddCubes(Vector3I min, Vector3I max);

		/// <summary>
		/// Split grid along a plane
		/// </summary>
		/// <param name="plane"></param>
		/// <returns>Splitted grid</returns>
		IMyCubeGrid SplitByPlane(PlaneD plane);

		/// <summary>
		/// Split grid
		/// </summary>
		/// <param name="blocks">List of blocks to split into new grid</param>
		/// <param name="sync">Pass <b>true</b> if on server to sync this to clients.</param>
		/// <returns>New grid</returns>
		/// <remarks>To sync to clients, this must be called on the server with sync = true.</remarks>
		IMyCubeGrid Split(List<IMySlimBlock> blocks, bool sync = true);

		/// <summary>
		/// Determines whether this grid is in the same logical group as the other, meaning they're connected
		/// either mechanically or via blocks like connectors. Be aware that using merge blocks combines grids into one, so this function
		/// will not filter out grids connected that way.
		/// </summary>
		/// <param name="other">Other grid</param>
		/// <returns>If grids connected logically</returns>
		bool IsInSameLogicalGroupAs(IMyCubeGrid other);

		/// <summary>
		/// <para>
		/// Determines whether this grid is mechanically connected to the other. This is any grid connected
		/// with rotors or pistons or other mechanical devices, but not things like connectors. This will in most
		/// cases constitute your complete construct.
		/// </para>
		/// <para>
		/// Be aware that using merge blocks combines grids into one, so this function will not filter out grids
		/// connected that way. Also be aware that detaching the heads of pistons and rotors will cause this
		/// connection to change.
		/// </para>
		/// </summary>
		/// <param name="other">Other grid</param>
		/// <returns>If grids connected mechanically</returns>
		bool IsSameConstructAs(IMyCubeGrid other);

		/// <summary>
		/// Is room at specified position airtight
		/// </summary>
		/// <param name="vector3I">position</param>
		/// <returns>true if airtight</returns>
		bool IsRoomAtPositionAirtight(Vector3I vector3I);

		/// <summary>
		/// Gets grid group of grids connected by provided link type
		/// </summary>
		/// <param name="linkTypeEnum">Type of grid connection</param>
		/// <returns>Grid group data</returns>
		/// <seealso cref="T:VRage.Game.ModAPI.IMyGridGroupData" />
		IMyGridGroupData GetGridGroup(GridLinkTypeEnum linkTypeEnum);

		/// <summary>
		/// Get all blocks in grid, inherit from this specific type
		/// </summary>
		/// <typeparam name="T">Blocks must be inherit from this type T</typeparam>
		/// <returns>Enumerable of blocks</returns>
		IEnumerable<T> GetFatBlocks<T>() where T : class, IMyCubeBlock;

		bool ApplyDeformation(float deformationOffset, float softAreaPlanar, float softAreaVertical, Vector3 localPos, Vector3 localNormal, MyStringHash damageType, out int blocksDestroyedByThisCp, float offsetThreshold = 0f, float lowerRatioLimit = 0f, long attackerId = 0L);
	}
}
