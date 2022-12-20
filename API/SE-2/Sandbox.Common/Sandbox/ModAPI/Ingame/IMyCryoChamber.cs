using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes cryo chamber block (PB scripting interface)
	/// </summary>
	public interface IMyCryoChamber : IMyCockpit, IMyShipController, IMyTerminalBlock, IMyCubeBlock, IMyEntity, IMyTextSurfaceProvider
	{
	}
}
