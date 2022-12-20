using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes rotor,piston or wheel suspension attachable block (PB scripting interface)
	/// </summary>
	public interface IMyAttachableTopBlock : IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Gets whether the top part is attached to a base block
		/// </summary>
		bool IsAttached { get; }

		/// <summary>
		/// Gets the attached base block
		/// </summary>
		IMyMechanicalConnectionBlock Base { get; }
	}
}
