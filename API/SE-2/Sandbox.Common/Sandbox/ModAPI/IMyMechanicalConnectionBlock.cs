using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes blocks linked with <see cref="F:VRage.Game.ModAPI.GridLinkTypeEnum.Mechanical" /> connection (mods interface)
	/// </summary>
	public interface IMyMechanicalConnectionBlock : IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock
	{
		/// <summary>
		/// Gets the grid of the attached top part
		/// </summary>
		new VRage.Game.ModAPI.IMyCubeGrid TopGrid { get; }

		/// <summary>
		/// Gets the attached top part entity
		/// </summary>
		new IMyAttachableTopBlock Top { get; }

		/// <summary>
		/// Attaches the specified top part to the base
		/// </summary>
		/// <param name="top"></param>
		/// <param name="updateGroup">true to update grid groups</param>
		void Attach(IMyAttachableTopBlock top, bool updateGroup = true);
	}
}
