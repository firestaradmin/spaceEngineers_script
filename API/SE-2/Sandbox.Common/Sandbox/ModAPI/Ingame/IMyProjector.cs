using System;
using System.Collections.Generic;
using VRage.Game;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes projector block (PB scripting interface)
	/// </summary>
	public interface IMyProjector : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity, IMyTextSurfaceProvider
	{
		[Obsolete("Use ProjectionOffset vector instead.")]
		int ProjectionOffsetX { get; }

		[Obsolete("Use ProjectionOffset vector instead.")]
		int ProjectionOffsetY { get; }

		[Obsolete("Use ProjectionOffset vector instead.")]
		int ProjectionOffsetZ { get; }

		[Obsolete("Use ProjectionRotation vector instead.")]
		int ProjectionRotX { get; }

		[Obsolete("Use ProjectionRotation vector instead.")]
		int ProjectionRotY { get; }

		[Obsolete("Use ProjectionRotation vector instead.")]
		int ProjectionRotZ { get; }

		/// <summary>
		/// Checks if there is an active projection
		/// </summary>
		bool IsProjecting { get; }

		/// <summary>
		/// Gets total number of blocks in the projection
		/// </summary>
		int TotalBlocks { get; }

		/// <summary>
		/// Gets number of blocks left to be welded
		/// </summary>
		int RemainingBlocks { get; }

		/// <summary>
		/// Gets comprehensive list of blocks left to be welded
		/// </summary>
		Dictionary<MyDefinitionBase, int> RemainingBlocksPerType { get; }

		/// <summary>
		/// Get number of armor blocks left to be welded
		/// </summary>
		int RemainingArmorBlocks { get; }

		/// <summary>
		/// Get count of blocks which can be welded now
		/// </summary>
		int BuildableBlocksCount { get; }

<<<<<<< HEAD
		/// <summary>
		/// Gets or sets projection offset
		/// </summary>
		Vector3I ProjectionOffset { get; set; }

		/// <summary>
		/// Get or sets projection rotation. These values are not in degrees. 1 = 90 degrees, 2 = 180 degrees
		/// </summary>
		Vector3I ProjectionRotation { get; set; }
=======
		Vector3I ProjectionOffset { get; set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		/// <summary>
		/// Gets or set should projection show only buildable blocks
		/// </summary>
<<<<<<< HEAD
=======
		Vector3I ProjectionRotation { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		bool ShowOnlyBuildable { get; set; }

		/// <summary>
		/// Call this after setting ProjectionOffset and ProjectionRotation to update the projection
		/// </summary>
		void UpdateOffsetAndRotation();
	}
}
