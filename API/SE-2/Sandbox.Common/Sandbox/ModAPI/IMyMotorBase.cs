using System;
using Sandbox.ModAPI.Ingame;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.ModAPI;
using VRageMath;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Describes base class for motor, advanced rotor and motor suspension block (mods interface)
	/// </summary>
	public interface IMyMotorBase : IMyMechanicalConnectionBlock, IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock, Sandbox.ModAPI.Ingame.IMyMotorBase
	{
		/// <summary>
		/// Gets the maximum angular velocity this rotor is capable of.
		/// </summary>
		float MaxRotorAngularVelocity { get; }

		/// <summary>
		/// Gets the current angular velocity.
		/// </summary>
		Vector3 RotorAngularVelocity { get; }

		/// <summary>
		/// Gets the grid attached to the rotor part
		/// </summary>
		[Obsolete("Use IMyMechanicalConnectionBlock.TopGrid")]
		VRage.Game.ModAPI.IMyCubeGrid RotorGrid { get; }

		/// <summary>
		/// Gets the attached rotor part entity
		/// </summary>
		[Obsolete("Use IMyMechanicalConnectionBlock.Top")]
		VRage.Game.ModAPI.IMyCubeBlock Rotor { get; }

		/// <summary>
		/// Gets the dummy position, to aid in attachment
		/// </summary>
		/// <remarks>Gets the location where the top rotor piece will attach.</remarks>
		Vector3 DummyPosition { get; }

		/// <summary>
		/// When the rotor head is attached or detached from the base
		/// </summary>
		/// <remarks>This event can be called in three states:
		/// <list type="number">
		/// <item>Stator is detached from rotor</item>
		/// <item>Stator is looking for rotor attachment</item>
		/// <item>Stator is attached to rotor</item>
		/// </list>
		/// The looking and attached states will both return <b>true</b> for <see cref="P:Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.IsAttached">IsAttached</see>.
		/// To determine which state it is, use the <see cref="P:Sandbox.ModAPI.Ingame.IMyMechanicalConnectionBlock.PendingAttachment">PendingAttachment</see> property: <b>true</b> means awaiting attachment, <b>false</b> means the rotor is attached.
		/// </remarks>
		event Action<IMyMotorBase> AttachedEntityChanged;

		/// <summary>
		/// Attaches a specified nearby rotor/wheel to the stator/suspension block
		/// </summary>
		/// <param name="rotor">Entity to attach</param>
		/// <param name="updateGroup">true to update grid groups</param>
		/// <remarks>The rotor to attach must already be in position before calling this method.</remarks>
		void Attach(IMyMotorRotor rotor, bool updateGroup = true);
	}
}
