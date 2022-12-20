using VRage.ModAPI;

namespace Sandbox.Game.Entities
{
<<<<<<< HEAD
	/// <summary>
	/// Extension methods for the <see cref="T:Sandbox.Game.Entities.MyParallelUpdateFlags" /> enum.
	/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	public static class MyParallelUpdateFlagsExtensions
	{
		public static MyParallelUpdateFlags GetParallel(this MyEntityUpdateEnum @enum)
		{
			return (MyParallelUpdateFlags)@enum;
		}
	}
}
