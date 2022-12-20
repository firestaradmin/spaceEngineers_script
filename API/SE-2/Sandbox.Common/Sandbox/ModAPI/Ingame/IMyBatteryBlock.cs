using System;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes battery block (PB scripting interface)
	/// </summary>
	public interface IMyBatteryBlock : IMyPowerProducer, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets whether this battery block has any remaining capacity
		/// </summary>
		bool HasCapacityRemaining { get; }

		/// <summary>
		/// Gets current stored power
		/// </summary>
		float CurrentStoredPower { get; }

		/// <summary>
		/// Gets maximum stored power
		/// </summary>
		float MaxStoredPower { get; }

		/// <summary>
		/// Gets current power input
		/// </summary>
		float CurrentInput { get; }

		/// <summary>
		/// Gets current maximum power input
		/// </summary>
		float MaxInput { get; }

		/// <summary>
		/// Gets whether this battery block is charging
		/// </summary>
		bool IsCharging { get; }

		/// <summary>
		/// Gets or sets charge mode
		/// </summary>
=======
		bool HasCapacityRemaining { get; }

		float CurrentStoredPower { get; }

		float MaxStoredPower { get; }

		float CurrentInput { get; }

		float MaxInput { get; }

		bool IsCharging { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		ChargeMode ChargeMode { get; set; }

		[Obsolete("Use ChargeMode instead")]
		bool OnlyRecharge { get; set; }

		[Obsolete("Use ChargeMode instead")]
		bool OnlyDischarge { get; set; }

		[Obsolete("Semi-auto is no longer a valid mode, if you want to check for Auto instead, use ChargeMode")]
		bool SemiautoEnabled { get; set; }
	}
}
