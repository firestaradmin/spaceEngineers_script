using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes Warhead block (PB scripting interface)
	/// </summary>
	public interface IMyWarhead : IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets whether this warhead block is counting down
		/// </summary>
		bool IsCountingDown { get; }

		/// <summary>
		/// Gets actual detonation time [s]
		/// </summary>
		float DetonationTime { get; set; }

		/// <summary>
		/// Gets or sets whether this warhead block is armed
		/// </summary>
=======
		bool IsCountingDown { get; }

		float DetonationTime { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		bool IsArmed { get; set; }

		/// <summary>
		/// Start the countdown
		/// </summary>
		/// <returns>true if countdown was started, false if countdown can not start (block not functional) or countdown already running</returns>
		bool StartCountdown();

		/// <summary>
		/// Stops the countdown
		/// </summary>
		/// <returns>true if countdown was stopped, false if countdown can not stop (block not functional) or countdown is not running</returns>
		bool StopCountdown();

		/// <summary>
		/// Detonates the warhead
		/// </summary>
		void Detonate();
	}
}
