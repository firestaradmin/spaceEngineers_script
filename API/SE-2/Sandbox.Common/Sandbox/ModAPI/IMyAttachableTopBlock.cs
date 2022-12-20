using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes rotor,piston or wheel suspension attachable block (mods interface)
	/// </summary>
	public interface IMyAttachableTopBlock : VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyAttachableTopBlock
	{
		/// <summary>
		/// Gets the attached base block
		/// </summary>
		new IMyMechanicalConnectionBlock Base { get; }
	}
}
