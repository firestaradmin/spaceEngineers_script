using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	public interface IMyGyro : Sandbox.ModAPI.Ingame.IMyGyro, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyFunctionalBlock, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets or sets multiplier for max gyro strength  
		/// </summary>
		float GyroStrengthMultiplier { get; set; }

		/// <summary>
		/// Gets or sets multiplier gyro power consumption
		/// </summary>
=======
		float GyroStrengthMultiplier { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		float PowerConsumptionMultiplier { get; set; }
	}
}
