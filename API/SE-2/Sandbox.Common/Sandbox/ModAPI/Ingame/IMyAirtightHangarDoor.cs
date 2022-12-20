using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes hangar door block (PB scripting interface)
	/// </summary>
	public interface IMyAirtightHangarDoor : IMyAirtightDoorBase, IMyDoor, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
	}
}
