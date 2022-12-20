using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;

namespace SpaceEngineers.Game.ModAPI.Ingame
{
	/// <summary>
	/// Describes any virtual mass block (PB Scripting interface)
	/// </summary>
	public interface IMyVirtualMass : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Gets virtual mass
		/// </summary>
		float VirtualMass { get; }
	}
}
