using System;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes motor rotor (mods interface)
	/// </summary>
	public interface IMyMotorRotor : IMyAttachableTopBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyAttachableTopBlock, Sandbox.ModAPI.Ingame.IMyMotorRotor
	{
		/// <summary>
		/// Gets the attached stator/suspension block
		/// </summary>
		[Obsolete("Use IMyAttachableTopBlock.Base")]
		IMyMotorBase Stator { get; }

		/// <summary>
		/// Gets the attached stator/suspension block
		/// </summary>
		new IMyMotorBase Base { get; }
	}
}
