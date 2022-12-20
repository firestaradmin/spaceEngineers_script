using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes jump drive block (PB scripting interface)
	/// </summary>
	public interface IMyJumpDrive : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets block stored power
		/// </summary>
		float CurrentStoredPower { get; }

		/// <summary>
		/// Gets block max stored power
		/// </summary>
		float MaxStoredPower { get; }

		/// <summary>
		/// Gets block status 
		/// </summary>
		MyJumpDriveStatus Status { get; }

		/// <summary>
		/// Gets or sets jump drive distance ratio, value from 0 to 1.
		/// </summary>
		float JumpDistanceRatio { get; set; }

		/// <summary>
		/// Gets or sets jump drive distance in meters.
		/// </summary>
		float JumpDistanceMeters { get; set; }

		/// <summary>
		/// Gets maximum jump drive distance.
		/// </summary>
		float MaxJumpDistanceMeters { get; }

		/// <summary>
		/// Gets minimum jump drive distance.
		/// </summary>
		float MinJumpDistanceMeters { get; }

		/// <summary>
		/// Turns on/off recharging
		/// </summary>
		bool Recharge { get; set; }
=======
		float CurrentStoredPower { get; }

		float MaxStoredPower { get; }

		MyJumpDriveStatus Status { get; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
