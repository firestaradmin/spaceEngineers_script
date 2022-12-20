using System;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes functional block (block with Enabled/Disabled toggle) (mods interface)
	/// </summary>
	public interface IMyFunctionalBlock : Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
	{
		/// <summary>
		/// Returns true if timer was created. If the block does not use timer, this will be false.
		/// </summary>
		bool IsUpdateTimerCreated { get; }

		/// <summary>
		/// Returns true if timer is enabled and ticking.
		/// </summary>
		bool IsUpdateTimerEnabled { get; }

<<<<<<< HEAD
		/// <summary>
		/// Called when Enabled changed
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		event Action<IMyTerminalBlock> EnabledChanged;

		/// <summary>
		/// Triggered when update timer ticks, event is fired after the block does all gameplay stuff
		/// </summary>
		event Action<IMyFunctionalBlock> UpdateTimerTriggered;

		/// <summary>
		/// Gets number of frames from the last trigger of the update timer
		/// </summary>
		/// <returns>number of frames</returns>
		uint GetFramesFromLastTrigger();
	}
}
