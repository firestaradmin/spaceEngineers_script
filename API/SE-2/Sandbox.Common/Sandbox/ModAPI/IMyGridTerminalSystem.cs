using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace Sandbox.ModAPI
{
	public interface IMyGridTerminalSystem : Sandbox.ModAPI.Ingame.IMyGridTerminalSystem
	{
		/// <summary>
		/// Called when new block group was added. Warning, on grid disconnects, you would need to unsubscribe and subscribe to new TerminalSystem 
		/// </summary>
		event Action<IMyBlockGroup> GroupAdded;

		/// <summary>
		/// Called when new block group was removed. Warning, on grid disconnects, you would need to unsubscribe and subscribe to new TerminalSystem 
		/// </summary>
		event Action<IMyBlockGroup> GroupRemoved;

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
		void GetBlockGroups(List<IMyBlockGroup> blockGroups);

		/// <summary>
		/// Fills the provided list with the blocks reachable by this grid terminal system. This means all blocks on the same
		/// grid, or connected via rotors, pistons or connectors.
		/// </summary>
		/// <typeparam name="T">The type of blocks to retrieve.</typeparam>
		/// <param name="blocks">A preallocated list to receive the blocks.</param>
		/// <param name="collect">Provide a filter method to determine if a given group should be added or not.</param>
		void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null);

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
		new IMyTerminalBlock GetBlockWithName(string name);

		/// <summary>
		/// Returns the first block group found with the given name. Will return <c>null</c> if no block group with that name
		/// can be found.
		/// </summary>
		/// <param name="name">The block group must contain the given name in their name.</param>
		/// <returns>First found block group with <c>name</c> or <c>null</c> if no block group with that name can be found</returns>
		new IMyBlockGroup GetBlockGroupWithName(string name);
	}
}
