using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes gun block (PB scripting interface)
	/// </summary>
	public interface IMyUserControllableGun : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets whether block is shooting
		/// </summary>
		bool IsShooting { get; }

		/// <summary>
		/// Represents terminal gui toggle element "Shoot". Not same as <see cref="P:Sandbox.ModAPI.Ingame.IMyUserControllableGun.IsShooting" />
		/// </summary>
		bool Shoot { get; set; }

		/// <summary>
		/// Triggers a single shot.
		/// </summary>
		void ShootOnce();
=======
		bool IsShooting { get; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
