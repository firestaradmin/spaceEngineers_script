using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes turret block (mods interface)
	/// </summary>
	public interface IMyLargeTurretBase : IMyUserControllableGun, IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUserControllableGun, Sandbox.ModAPI.Ingame.IMyLargeTurretBase, IMyCameraController, IMyTargetingCapableBlock
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets current turret target
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		VRage.ModAPI.IMyEntity Target { get; }

		/// <summary>
		/// Tracks target without position prediction
		/// </summary>
		/// <param name="entity"></param>
		void SetTarget(VRage.ModAPI.IMyEntity entity);

		/// <summary>
		/// Tracks entity with enabled position prediction
		/// </summary>
		/// <param name="entity"></param>
		void TrackTarget(VRage.ModAPI.IMyEntity entity);
	}
}
