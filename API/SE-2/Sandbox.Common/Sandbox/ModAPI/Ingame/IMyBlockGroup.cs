using System;
using System.Collections.Generic;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes terminal block group (PB scripting interface)
	/// </summary>
	public interface IMyBlockGroup
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets name of terminal blocks group 
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		string Name { get; }

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

		/// <summary>
		/// Get terminal blocks which assigned to this group, and matching type
		/// </summary>
		/// <typeparam name="T">Block must be assignable from T</typeparam>
		/// <param name="blocks">Buffer array that would receive results. It is cleared before call. Can be null</param>
		/// <param name="collect">if function returns true, block would be added to <paramref name="blocks" /></param>
		/// <exception cref="T:System.NullReferenceException">If function <paramref name="collect" /> returns true, and <paramref name="blocks" /> is null</exception>
		void GetBlocksOfType<T>(List<T> blocks, Func<T, bool> collect = null) where T : class;
	}
}
