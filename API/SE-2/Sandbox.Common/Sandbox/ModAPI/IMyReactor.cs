using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes reactor block (mods interface)
	/// </summary>
	public interface IMyReactor : Sandbox.ModAPI.Ingame.IMyReactor, Sandbox.ModAPI.Ingame.IMyPowerProducer, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyPowerProducer, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets or sets multiplier that increasing/decreasing amount of produced power keeping fuel consumption same. Default value = 1
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		float PowerOutputMultiplier { get; set; }
	}
}
