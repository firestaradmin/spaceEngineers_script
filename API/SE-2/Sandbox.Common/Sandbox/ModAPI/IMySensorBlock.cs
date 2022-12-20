using System;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes sensor block (mods interface)
	/// </summary>
	public interface IMySensorBlock : IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMySensorBlock
	{
		/// <summary>
		/// Gets or sets the sensor mininum field as a Vector3(-L,-Bo,-F).
		/// </summary>
		/// <remarks>
		/// -X is Left
		/// -Y is Bottom
		/// -Z is Front
		/// </remarks>
		Vector3 FieldMin { get; set; }

		/// <summary>
		/// Gets or sets the sensor maximum field as a Vector3(R,T,Ba).
		/// </summary>
		/// <remarks>
		/// X is Right
		/// Y is Top
		/// Z is Back
		/// </remarks>
		Vector3 FieldMax { get; set; }

		/// <summary>
		/// Called when sensor state changer. (Found something)
		/// </summary>
		event Action<bool> StateChanged;
	}
}
