using System;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// ModAPI interface giving access to grid-group control system
	/// </summary>
	public interface IMyGridControlSystem
	{
		/// <summary>
		/// Gets whether grid-group is controlled
		/// </summary>
		bool IsControlled { get; }

		/// <summary>
		/// Called when <see cref="P:VRage.Game.ModAPI.IMyGridControlSystem.IsControlled" /> changed. Invoked with grid main grid that 
		/// </summary>
		event Action<bool, IMyCubeGrid> IsControlledChanged;
	}
}
