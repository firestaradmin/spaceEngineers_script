using System;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes interface that provides information about current control state for controllable entities such as turret, cockpit, character ...
	/// </summary>
	/// <see cref="T:VRage.Game.ModAPI.Interfaces.IMyControllableEntity" />
	/// <see cref="T:VRage.Game.ModAPI.IMyEntityController" />
	public interface IMyControllerInfo
	{
		/// <summary>
		/// Gets the controller for the entity
		/// </summary>
		IMyEntityController Controller { get; }

		/// <summary>
		/// Gets the controlling entity id (eg. IMyPlayer.IdentityId)
		/// </summary>
		long ControllingIdentityId { get; }

		/// <summary>
		/// Called when the entity gains a controller
		/// </summary>
		event Action<IMyEntityController> ControlAcquired;

		/// <summary>
		/// Called when the entity loses a controller
		/// </summary>
		event Action<IMyEntityController> ControlReleased;

		/// <summary>
		/// Returns true if the local player is controlling the entity
		/// </summary>
		/// <returns></returns>
		bool IsLocallyControlled();

		/// <summary>
		/// Returns true if the local human player is controlling the entity
		/// </summary>
		/// <returns></returns>
		bool IsLocallyHumanControlled();

		/// <summary>
		/// Returns true if the entity is remotely controlled
		/// </summary>
		/// <returns></returns>
		bool IsRemotelyControlled();
	}
}
