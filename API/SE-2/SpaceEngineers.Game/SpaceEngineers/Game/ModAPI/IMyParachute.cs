using System;
<<<<<<< HEAD
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ModAPI;
=======
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace SpaceEngineers.Game.ModAPI
{
	/// <summary>
	/// Describes Parachute block (mods interface)
	/// </summary>
	public interface IMyParachute : Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyParachute
	{
		/// <summary>
		/// Event that will trigger true if Chute is now deployed or false if Chute is now cut 
		/// </summary>
		/// <returns></returns>
		event Action<bool> ParachuteStateChanged;

		/// <summary>
		/// Event when doors of the block changed the state.
		/// </summary>        
		[Obsolete("Parachute block no longer has doors, so this even is just generic even when state changed. We will replace with new event in the future.")]
		event Action<bool> DoorStateChanged;
	}
}
