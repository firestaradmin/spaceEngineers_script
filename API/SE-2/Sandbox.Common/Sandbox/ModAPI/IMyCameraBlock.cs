using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes Camera block (mods interface)
	/// </summary>
	public interface IMyCameraBlock : IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyCameraBlock, IMyCameraController
	{
		/// <summary>
		/// Gets whether this camera is in use locally by the <i>current</i> player.
		/// To determine if <i>any</i> player is controlling the camera, instead use <see cref="P:Sandbox.ModAPI.Ingame.IMyCameraBlock.IsActive" />.
		/// </summary>
		bool IsActiveLocal { get; }
	}
}
