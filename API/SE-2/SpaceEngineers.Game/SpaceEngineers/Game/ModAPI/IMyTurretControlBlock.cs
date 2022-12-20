using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace SpaceEngineers.Game.ModAPI
{
	/// <summary>
	/// Describes Turret Control block (mods interface)
	/// </summary>
	public interface IMyTurretControlBlock : SpaceEngineers.Game.ModAPI.Ingame.IMyTurretControlBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
	{
		VRage.ModAPI.IMyEntity Target { get; }

		/// <summary>
		/// Get the character that is currently piloting the controller.
		/// </summary>
		IMyCharacter Pilot { get; }

		/// <summary>
		/// Sets target without position prediction
		/// </summary>
		/// <param name="entity">target entity</param>
		void SetTarget(VRage.ModAPI.IMyEntity entity);

		/// <summary>
		/// Tracks entity with enabled position prediction
		/// </summary>
		/// <param name="entity">target entity</param>
		void TrackTarget(VRage.ModAPI.IMyEntity entity);
	}
}
