using System;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;

namespace SpaceEngineers.Game.ModAPI.Ingame
{
	/// <summary>
	/// Describes Air Vent block (PB scripting interface)
	/// </summary>
	public interface IMyAirVent : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Gets whether this vent can pressurize. If so room is airtight.        
		/// </summary>
		bool CanPressurize { get; }

		/// <summary>
		/// Gets whether this vet is depressurizing
		/// </summary>
		[Obsolete("IsDepressurizing is deprecated, please use Depressurize instead.")]
		bool IsDepressurizing { get; }

		/// <summary>
		/// Gets whether this vent is in depressurize state
		/// </summary>
		bool Depressurize { get; set; }

		/// <summary>
		/// Gets the current air vent status
		/// </summary>
		VentStatus Status { get; }

		/// <summary>
		/// Gets whether this vent has pressurization enabled.
		/// </summary>
		bool PressurizationEnabled { get; }

		/// <summary>
		/// Gets whether this vent can be pressurized
		/// </summary>
		/// <returns>true if containing room is airtight</returns>
		[Obsolete("IsPressurized() is deprecated, please use CanPressurize instead.")]
		bool IsPressurized();

		/// <summary>
		/// Gets Oxygen level in room
		/// </summary>
		/// <returns>Oxygen fill level as float (0.5 = 50%)</returns>
		float GetOxygenLevel();
	}
}
