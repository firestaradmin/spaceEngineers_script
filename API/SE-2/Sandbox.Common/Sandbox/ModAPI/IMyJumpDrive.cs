using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes jump drive block (mods interface)
	/// </summary>
	public interface IMyJumpDrive : IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyJumpDrive
	{
		/// <summary>
		/// Gets or sets the stored charge.
		/// </summary>
		new float CurrentStoredPower { get; set; }

		/// <summary>
		/// Requests the jump drive to make a jump.
		/// MODDERS: Use IMyCubeGrid.JumpSystems for more options.
		/// </summary>
		/// <param name="usePilot">Pass <b>true</b> to use the controlling player as a direction reference, <b>false</b> to use the default ship controller. Default <b>true</b>.
		/// </param>
		void Jump(bool usePilot = true);
	}
}
