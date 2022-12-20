using System;
using System.Collections.Generic;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// ModAPI interface giving access to grid-groups
	/// </summary>
	public interface IMyGridGroups
	{
		/// <summary>
<<<<<<< HEAD
		/// Called when new grid-group was created.
		/// Example 1: 1 large grid-group splitted into 2 parts.
		/// Example 2: new grid created
		/// </summary>
		event Action<IMyGridGroupData> OnGridGroupCreated;

		/// <summary>
		/// Called when grid-group was destroyed.
		/// Example 1: 2 small grids-groups joined into larger one.
		/// Example 2: grid, not connected to any other grid, was destroyed
		/// </summary>
		event Action<IMyGridGroupData> OnGridGroupDestroyed;

		/// <summary>
		/// OBSOLETE: Use GetGroup with passing your own collection, it is better for simulation speed. Returns all grids connected to the given grid by the specified link type. Array always contains node.
		/// </summary>
		/// <param name="node">One of grid group</param>
		/// <param name="type">Type of linking</param>
		/// <returns>New list of connected grids</returns>
=======
		/// OBSOLETE: Use GetGroup with passing your own collection, it is better for simulation speed. Returns all grids connected to the given grid by the specified link type. Array always contains node.
		/// </summary>
		/// <param name="node"></param>
		/// <param name="type"></param>
		/// <returns></returns>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Obsolete("Use GetGroup with passing your own collection, it is better for simulation speed", false)]
		List<IMyCubeGrid> GetGroup(IMyCubeGrid node, GridLinkTypeEnum type);

		/// <summary>
		/// Returns all grids connected to the given grid by the specified link type. Array always contains node.
		/// </summary>
<<<<<<< HEAD
		/// <param name="node">One of grid </param>
		/// <param name="type">Type of grid linking</param>
		/// <param name="collection">Collection where connected grids would be added</param>
=======
		/// <param name="node"></param>
		/// <param name="type"></param>
		/// <returns></returns>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		void GetGroup(IMyCubeGrid node, GridLinkTypeEnum type, ICollection<IMyCubeGrid> collection);

		/// <summary>
		/// Checks if two grids are connected by the given link type.
		/// </summary>
		/// <param name="grid1">Grid 1</param>
		/// <param name="grid2">Grid 2</param>
		/// <param name="type">Type of connection</param>
		/// <returns>True when 2 grids connected with specified grid linking</returns>
		bool HasConnection(IMyCubeGrid grid1, IMyCubeGrid grid2, GridLinkTypeEnum type);

		/// <summary>
		/// Gets grid-group for provided grid
		/// </summary>
		/// <param name="linking">Type of linking</param>
		/// <param name="grid">One part of grid-group</param>
		/// <returns>Grid group interface</returns>
		IMyGridGroupData GetGridGroup(GridLinkTypeEnum linking, IMyCubeGrid grid);

		/// <summary>
		/// Gets all grid groups, that exists in the world
		/// </summary>
		/// <param name="linking">Type of linking</param>
		/// <param name="grids">Collection would be filled with results</param>
		/// <typeparam name="T">Type of collection</typeparam>
		/// <returns>Provided collection</returns>
		T GetGridGroups<T>(GridLinkTypeEnum linking, T grids) where T : ICollection<IMyGridGroupData>;

		/// <summary>
		/// Generates <see cref="T:VRage.Game.ModAPI.MyGridGroupsDefaultEventHandler" /> each time grid group connected with provided linking is created
		/// </summary>
		/// <param name="type">Type of linking</param>
		/// <param name="creator">Function that creates grid group logic</param>
		/// <typeparam name="T">Type of grid group logic</typeparam>
		void AddGridGroupLogic<T>(GridLinkTypeEnum type, Func<IMyGridGroupData, T> creator) where T : MyGridGroupsDefaultEventHandler;
	}
}
