using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes reloadable missile block (PB scripting interface)
	/// </summary>
	public interface IMySmallMissileLauncherReload : IMySmallMissileLauncher, IMyUserControllableGun, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
	}
}
