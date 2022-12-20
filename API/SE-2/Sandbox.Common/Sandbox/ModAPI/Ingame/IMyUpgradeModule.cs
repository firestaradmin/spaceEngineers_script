using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes upgrade module block (PB scripting interface)
	/// </summary>
	public interface IMyUpgradeModule : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Gets number of upgrade effects this block has
		/// </summary>
		uint UpgradeCount { get; }

		/// <summary>
		/// Gets number of blocks this block is connected to
		/// </summary>
		uint Connections { get; }

		/// <summary>
		/// Gets list of upgrades from this block, see <see cref="T:VRage.Game.ObjectBuilders.Definitions.MyUpgradeModuleInfo">MyUpgradeModuleInfo</see> for details
		/// </summary>
		void GetUpgradeList(out List<MyUpgradeModuleInfo> upgrades);
	}
}
