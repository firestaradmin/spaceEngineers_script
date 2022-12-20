using System;
using VRage.Game.ModAPI.Ingame;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes functional block (block with Enabled/Disabled toggle) (PB scripting interface)
	/// </summary>
	public interface IMyFunctionalBlock : IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Represents terminal gui toggle. Gets or sets if block is Enabled 
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		bool Enabled { get; set; }

		[Obsolete("Use the setter of Enabled")]
		void RequestEnable(bool enable);
	}
}
