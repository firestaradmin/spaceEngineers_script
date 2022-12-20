using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes block, which has upgrade effects (PB scripting interface)
	/// </summary>
	public interface IMyUpgradableBlock : IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Gets number of upgrades applied
		/// </summary>
		uint UpgradeCount { get; }

		/// <summary>
		/// Get list of upgrades. Read only.
		/// </summary>
		/// <param name="upgrades">String - upgrade type, float - effect value as float (1 = 100%)</param>
		void GetUpgrades(out Dictionary<string, float> upgrades);
	}
}
