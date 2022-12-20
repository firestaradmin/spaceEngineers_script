using System;

namespace Sandbox.Game.Entities
{
<<<<<<< HEAD
	/// <summary>
	/// Parallel update flags.
	/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	[Flags]
	public enum MyParallelUpdateFlags
	{
		NONE = 0x0,
		EACH_FRAME = 0x1,
		EACH_10TH_FRAME = 0x2,
		EACH_100TH_FRAME = 0x4,
		BEFORE_NEXT_FRAME = 0x8,
		SIMULATE = 0x10,
<<<<<<< HEAD
		/// <summary>
		/// Notifies that this entity wants to be updated in parallel each frame.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		EACH_FRAME_PARALLEL = 0x20
	}
}
