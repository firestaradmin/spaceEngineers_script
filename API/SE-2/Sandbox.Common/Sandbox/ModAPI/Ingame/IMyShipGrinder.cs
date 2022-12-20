using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes ship grinder block (PB scripting interface)
	/// </summary>
	public interface IMyShipGrinder : IMyShipToolBase, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
	}
}
