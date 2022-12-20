using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes ship controller block (cockpit/remote control/cryopod) (mods interface)
	/// </summary>
	public interface IMyShipController : IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyShipController, IMyControllableEntity, IMyTargetingCapableBlock
	{
		/// <summary>
		/// Gets if this ship controller contains a first-person camera view.
		/// </summary>
		bool HasFirstPersonCamera { get; }

		/// <summary>
		/// Get the last character that was piloting the controller.
		/// </summary>
		IMyCharacter LastPilot { get; }

		/// <summary>
		/// Get the character that is currently piloting the controller.
		/// </summary>
		IMyCharacter Pilot { get; }

		/// <summary>
		/// Gets if the ship is shooting selected weapons.
		/// </summary>
		bool IsShooting { get; }

		/// <summary>
		/// If player gets into this cockpit for the first time, the camera is in 3rd view
		/// </summary>
<<<<<<< HEAD
=======
		/// <remarks>Set by MoveAndRotate, regardless if a movement happened.</remarks>
		new Vector3 MoveIndicator { get; }

		/// <summary>
		/// Gets the current rotation direction indicator
		/// </summary>
		/// <remarks>Set by MoveAndRotate, regardless if a movement happened.</remarks>
		new Vector2 RotationIndicator { get; }

		/// <summary>
		/// Gets the current roll direction indicator
		/// </summary>
		/// <remarks>Set by MoveAndRotate, regardless if a movement happened.</remarks>
		new float RollIndicator { get; }

		/// <summary>
		/// If player gets into this cockpit for the first time, the camera is in 3rd view
		/// </summary>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		bool IsDefault3rdView { get; }
	}
}
