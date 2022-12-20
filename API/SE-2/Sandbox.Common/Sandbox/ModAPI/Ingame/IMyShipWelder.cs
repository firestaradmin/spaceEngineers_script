using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes ship welder interface (PB scripting interface)
	/// </summary>
	public interface IMyShipWelder : IMyShipToolBase, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Gets and sets whether this welder can help other welders
		/// </summary>
		bool HelpOthers { get; set; }
	}
}
