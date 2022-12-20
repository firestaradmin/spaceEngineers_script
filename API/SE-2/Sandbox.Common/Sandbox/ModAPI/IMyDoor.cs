using System;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes door block (mods interface)
	/// </summary>
	public interface IMyDoor : IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyDoor
	{
<<<<<<< HEAD
		/// <summary>
		/// Returns whether door is fully closed. (Animations are stopped)
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		bool IsFullyClosed { get; }

		/// <summary>
		/// Called when door changes state 
		/// </summary>
		event Action<bool> DoorStateChanged;

		/// <summary>
		/// Called when door changes opened state 
		/// </summary>
		event Action<IMyDoor, bool> OnDoorStateChanged;
	}
}
