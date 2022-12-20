using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes block, which has upgrade effects from block <see cref="T:Sandbox.ModAPI.Ingame.IMyUpgradableBlock" /> (PB scripting interface)
	/// </summary>
	public interface IMyUpgradableBlock : VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyUpgradableBlock
	{
	}
}
