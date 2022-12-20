using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes gyroscope block (PB scripting interface)
	/// </summary>
	public interface IMyGyro : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Get or set gyroscope power in 
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		float GyroPower { get; set; }

		bool GyroOverride { get; set; }

		float Yaw { get; set; }

		float Pitch { get; set; }

		float Roll { get; set; }
	}
}
