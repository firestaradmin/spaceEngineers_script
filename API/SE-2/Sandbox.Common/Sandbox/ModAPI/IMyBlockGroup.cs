using System;
using System.Collections.Generic;
using Sandbox.ModAPI.Ingame;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes terminal block group (mods interface)
	/// </summary>
	public interface IMyBlockGroup : Sandbox.ModAPI.Ingame.IMyBlockGroup
	{
		/// <summary>
		/// Get terminal blocks which assigned to this group
		/// </summary>
		/// <param name="blocks">buffer array</param>
		/// <param name="collect">if function returns true, block would be added to <paramref name="blocks" /></param>
		void GetBlocks(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null);

		/// <summary>
		/// Get terminal blocks which assigned to this group, and matching type 
		/// </summary>
		/// <typeparam name="T">Block must be assignable from T</typeparam>
		/// <param name="blocks">buffer array</param>
		/// <param name="collect">if function returns true, block would be added to <paramref name="blocks" /></param>
		/// <exception cref="T:System.NullReferenceException">If function <paramref name="collect" /> returns true, and <paramref name="blocks" /> is null</exception>
		void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null) where T : class;
	}
}
