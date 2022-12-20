using System;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes piston block (mods interface)
	/// </summary>
	public interface IMyPistonBase : IMyMechanicalConnectionBlock, IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock, Sandbox.ModAPI.Ingame.IMyPistonBase
	{
		/// <summary>
		/// Called when piston reaching maximum or minimum extension
		/// Argument is true, when piston is extending
		/// Argument is false, when piston is retracting
		/// </summary>
		event Action<bool> LimitReached;

		/// <summary>
		/// Notifies when the top grid is attached or detached
		/// </summary>
		event Action<IMyPistonBase> AttachedEntityChanged;

		/// <summary>
		/// Attaches a specified nearby top part to the piston block
		/// </summary>
		/// <param name="top">Entity to attach</param>
		/// <param name="updateGroup">true to update grid groups</param>
		/// <remarks>The top to attach must already be in position before calling this method.</remarks>
		void Attach(IMyPistonTop top, bool updateGroup = true);
	}
}
