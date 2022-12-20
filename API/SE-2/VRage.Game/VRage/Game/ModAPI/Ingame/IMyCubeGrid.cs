using VRageMath;

namespace VRage.Game.ModAPI.Ingame
{
	/// <summary>
	/// Grid interface
	/// </summary>
	public interface IMyCubeGrid : IMyEntity
	{
		/// <summary>
		/// Getter and setter for display name of the grid (as seen in Info terminal tab)
		/// </summary>
		string CustomName { get; set; }

		/// <summary>
		/// Getter of grid size in meters
		/// </summary>
		float GridSize { get; }

		/// <summary>
		/// Getter for grid size enum
		/// </summary>
		MyCubeSize GridSizeEnum { get; }

		/// <summary>
		/// Determines if the grid is static (unmoveable)
		/// </summary>
		bool IsStatic { get; }

		/// <summary>
		/// Maximum coordinates of blocks in grid
		/// </summary>
		Vector3I Max { get; }

		/// <summary>
		/// Minimum coordinates of blocks in grid
		/// </summary>
		Vector3I Min { get; }

		/// <summary>
		/// Returns true if there is any block occupying given position
		/// </summary>
		/// <returns>True if block exists</returns>
		bool CubeExists(Vector3I pos);

		/// <summary>
		/// Get cube block at given position
		/// </summary>
		/// <param name="pos">Block position</param>
		/// <returns>Block or null if none is present at given position</returns>
		IMySlimBlock GetCubeBlock(Vector3I pos);

		/// <summary>
		/// Converts grid coordinates to world space
		/// </summary>
		/// <returns>Vector in world coordinates</returns>
		Vector3D GridIntegerToWorld(Vector3I gridCoords);

		/// <summary>
		/// Converts world coordinates to grid space cell coordinates
		/// </summary>
		/// <returns>Vector in world grid coordinates</returns>
		Vector3I WorldToGridInteger(Vector3D coords);

		/// <summary>
		/// <para>
		/// Determines whether this grid is <see cref="F:VRage.Game.ModAPI.GridLinkTypeEnum.Mechanical" /> connected to the other. This is any grid connected
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
		/// <returns>True if grid connected with <see cref="F:VRage.Game.ModAPI.GridLinkTypeEnum.Mechanical" /> linking</returns>
		bool IsSameConstructAs(IMyCubeGrid other);
	}
}
