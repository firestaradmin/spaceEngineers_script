using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame;

namespace SpaceEngineers.Game.ModAPI.Ingame
{
	/// <summary>
	/// Describes Artificial Mass block (PB scripting interface)
	/// </summary>
	public interface IMyArtificialMassBlock : IMyVirtualMass, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
	}
}
