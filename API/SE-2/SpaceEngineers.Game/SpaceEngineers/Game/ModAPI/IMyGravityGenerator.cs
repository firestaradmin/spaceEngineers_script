using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRageMath;

namespace SpaceEngineers.Game.ModAPI
{
	/// <summary>
	/// Describes Gravity generator block (mods interface)
	/// </summary>
	public interface IMyGravityGenerator : IMyGravityGeneratorBase, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGeneratorBase, IMyGravityProvider, SpaceEngineers.Game.ModAPI.Ingame.IMyGravityGenerator
	{
		/// <summary>
		/// Gets or sets the gravity field as a Vector3(W,H,D).
		/// </summary>
		/// <remarks>
		/// X is Width
		/// Y is Height
		/// Z is Depth
		/// This is not clamped like the Ingame one is.
		/// </remarks>
		new Vector3 FieldSize { get; set; }
	}
}
