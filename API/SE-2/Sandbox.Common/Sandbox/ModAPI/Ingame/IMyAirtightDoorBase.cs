using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes interface for airtight doors (PB scripting interface)
	/// </summary>
	public interface IMyAirtightDoorBase : IMyDoor, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
	}
}
