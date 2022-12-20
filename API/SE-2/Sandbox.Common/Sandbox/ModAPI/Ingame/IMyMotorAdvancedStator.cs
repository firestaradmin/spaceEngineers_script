using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes advanced motor stator (PB scripting interface)
	/// </summary>
	public interface IMyMotorAdvancedStator : IMyMotorStator, IMyMotorBase, IMyMechanicalConnectionBlock, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
	}
}
