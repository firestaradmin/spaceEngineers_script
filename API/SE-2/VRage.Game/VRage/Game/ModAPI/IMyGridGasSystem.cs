using System;
using System.Collections.Generic;
using VRageMath;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// ModAPI interface giving access to grid-group gas system
	/// </summary>
	public interface IMyGridGasSystem
	{
		/// <summary>
		/// Returns true if the grid is currently recalculating airtightness (It's multithreaded)
		/// </summary>
		bool IsProcessingData { get; }

		/// <summary>
		/// Called when ProcessingData becomes false
		/// </summary>
		Action OnProcessingDataComplete { get; set; }

		/// <summary>
		/// Called when ProcessingData becomes true
		/// </summary>
		Action OnProcessingDataStart { get; set; }

		/// <summary>
		/// Gets all oxygen rooms on the grid
		/// </summary>
		bool GetRooms(List<IMyOxygenRoom> list);

		/// <summary>
		/// Returns an oxygen room at the position if there is one
		/// </summary>
		IMyOxygenRoom GetOxygenRoomForCubeGridPosition(ref Vector3I gridPosition);

		/// <summary>
		/// Returns an oxygen block at the position if there is one
		/// </summary>
		IMyOxygenBlock GetOxygenBlock(Vector3D worldPosition);
	}
}
