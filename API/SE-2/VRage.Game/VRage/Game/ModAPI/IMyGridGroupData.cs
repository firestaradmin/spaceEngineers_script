using System;
using System.Collections.Generic;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// A class that presenting connection between grids 
	/// WARNING: you must not keep link to instance or you have to remove link when event OnReleased is fired
	/// Use Get/SetVariable to store data in GridGroups. Variables are cleared after OnRelease is fired
	/// </summary>
	public interface IMyGridGroupData
	{
		/// <summary>
		/// Get connection type
		/// </summary>
		GridLinkTypeEnum LinkType { get; }

		/// <summary>
		/// First MyGridGroupData(this) - where grid would be added
		/// Second MyGridGroupData(Nullable) - previous grid group of grid 
		/// </summary>
		event Action<IMyGridGroupData, IMyCubeGrid, IMyGridGroupData> OnGridAdded;

		/// <summary>
		/// First MyGridGroupData(this) - from where grid was removed
		/// Second MyGridGroupData(Nullable) - where grid group would be added
		///
		/// Called after Keen OnAdded logic, like MyGridLogicalGroupData.OnNodeAdded
		/// </summary>
		event Action<IMyGridGroupData, IMyCubeGrid, IMyGridGroupData> OnGridRemoved;

		/// <summary>
		/// You must clean your subscriptions here. Instances of IMyGridGroupData are re-used in ObjectPool.
		/// At the time event is called it has no grids attached to it.
		/// </summary>
		event Action<IMyGridGroupData> OnReleased;

		/// <summary>
		/// Gets grids in this grid-group
		/// </summary>
		/// <param name="grids">Collection, that would receive grids</param>
		/// <typeparam name="T">Generic type of collection</typeparam>
		/// <returns>Grids stored in provided collection</returns>
		T GetGrids<T>(T grids) where T : ICollection<IMyCubeGrid>;

		/// <summary>
		/// Gets memory-stored variable
		/// </summary>
		/// <param name="key">Key to access variable in dictionary</param>
		/// <param name="variable">Variable that stored by key, or default value for type T (null)</param>
		/// <typeparam name="T">Type of stored value</typeparam>
		/// <returns>True, when variable found, false when not</returns>
		/// <remarks>Strongly recommended to define GUID in sbc file, but that is not required</remarks>
		bool TryGetVariable<T>(Guid key, out T variable);

		/// <summary>
		/// Gets memory-stored variable
		/// </summary>
		/// <param name="key">Key to access variable in dictionary</param>
		/// <typeparam name="T">Type of stored value</typeparam>
		/// <returns>Variable that stored by key, or default value for type T (null)</returns>
		/// <remarks>Strongly recommended to define GUID in sbc file, but that is not required</remarks>
		T GetVariable<T>(Guid key);

		/// <summary>
		/// Sets memory-stored variable
		/// </summary>
		/// <param name="key">Key to access variable in dictionary</param>
		/// <param name="data">Stored variable</param>
		/// <remarks>Strongly recommended to define GUID in sbc file, but that is not required</remarks>
		void SetVariable(Guid key, object data);

		/// <summary>
		/// Removes stored variable
		/// </summary>
		/// <param name="key">Key to access variable in dictionary</param>
		/// <returns>True, if removed</returns>
		bool RemoveVariable(Guid key);
	}
}
