using System;
using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes terminal system (PB scripting interface)
	/// </summary>
	public interface IMyGridTerminalSystem
	{
		/// <summary>
		/// Fills the provided list with all the blocks reachable by this grid terminal system. This means all blocks on the same
		/// grid, or connected via rotors, pistons or connectors.
		///
		/// </summary>
		/// <param name="blocks">A preallocated list to receive the blocks.</param>
		void GetBlocks(List<IMyTerminalBlock> blocks);

		/// <summary>
		/// Fills the provided list with the block groups reachable by this grid terminal system.
		/// </summary>
		/// <param name="blockGroups"></param>
		/// <param name="collect">Provide a filter method to determine if a given group should be added or not.</param>
		void GetBlockGroups(List<IMyBlockGroup> blockGroups, Func<IMyBlockGroup, bool> collect = null);

		/// <summary>
		/// Fills the provided list with the blocks reachable by this grid terminal system. This means all blocks on the same
		/// grid, or connected via rotors, pistons or connectors.
		/// </summary>
		/// <typeparam name="T">The type of blocks to retrieve.</typeparam>
		/// <param name="blocks">A preallocated list to receive the blocks.</param>
		/// <param name="collect">Provide a filter method to determine if a given group should be added or not.</param>
		void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null) where T : class;

		/// <summary>
		/// Fills the provided list with the blocks reachable by this grid terminal system. This means all blocks on the same
		/// grid, or connected via rotors, pistons or connectors.
		/// </summary>
		/// <typeparam name="T">The type of blocks to retrieve.</typeparam>
		/// <param name="blocks">A preallocated list to receive the blocks.</param>
		/// <param name="collect">Provide a filter method to determine if a given group should be added or not.</param>
		void GetBlocksOfType<T>(List<T> blocks, Func<T, bool> collect = null) where T : class;

		/// <summary>
		/// Fills the provided list with the blocks reachable by this grid terminal system. This means all blocks on the same
		/// grid, or connected via rotors, pistons or connectors. 
		/// </summary>
		/// <param name="name">The blocks must contain the given name in their name.</param>
		/// <param name="blocks">A preallocated list to receive the blocks.</param>
		/// <param name="collect">Provide a filter method to determine if a given group should be added or not.</param>
		void SearchBlocksOfName(string name, List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null);

		/// <summary>
		/// Returns the first block found with the given name.
		/// </summary>
		/// <param name="name">The block must contain the given name in their name.</param>
		/// <returns>First found block with <c>name</c> or <c>null</c> if no block with that name can be found</returns>
		IMyTerminalBlock GetBlockWithName(string name);

		/// <summary>
		/// Returns the first block group found with the given name. Will return <c>null</c> if no block group with that name
		/// can be found.
		/// </summary>
		/// <param name="name">The block group must contain the given name in their name.</param>
		/// <returns>First found block group with <c>name</c> or <c>null</c> if no block group with that name can be found</returns>
		IMyBlockGroup GetBlockGroupWithName(string name);

		/// <summary>
		///  Attempts to retrieve the block with the given entity ID.
		/// </summary>
		/// <param name="id">Entity Id</param>
		/// <returns>Block or <c>null</c> if no block with provided id found</returns>
		IMyTerminalBlock GetBlockWithId(long id);

		/// <summary>
		/// Checks if the grid terminal system can still access the given <see cref="T:Sandbox.ModAPI.Ingame.IMyTerminalBlock" />. A block is no longer
		/// accessible if it's destroyed, detached, it's ownership has changed or is otherwise disconnected from this grid terminal system.
		/// </summary>
		/// <param name="block">Block to test</param>
		/// <param name="scope">Type of access test</param>
		/// <returns></returns>
		bool CanAccess(IMyTerminalBlock block, MyTerminalAccessScope scope = MyTerminalAccessScope.All);

		/// <summary>
		/// Checks if the grid terminal system can still access the given <see cref="T:VRage.Game.ModAPI.Ingame.IMyCubeGrid" />. A grid is no longer accessible
		/// if it's destroyed, detached, it's ownership has changed or is otherwise disconnected from this grid terminal system.
		/// </summary>
		/// <param name="grid">Grid to check</param>
		/// <param name="scope">Type of access check</param>
		/// <returns>False if grid is <c>null</c>, closed or you can't access that grid</returns>
		bool CanAccess(IMyCubeGrid grid, MyTerminalAccessScope scope = MyTerminalAccessScope.All);
	}
}
