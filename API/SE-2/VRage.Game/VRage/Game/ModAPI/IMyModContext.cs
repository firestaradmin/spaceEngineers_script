namespace VRage.Game.ModAPI
{
	public interface IMyModContext
	{
		string ModName { get; }

		string ModId { get; }

		string ModServiceName { get; }

		string ModPath { get; }

		string ModPathData { get; }

		bool IsBaseGame { get; }
<<<<<<< HEAD

		MyObjectBuilder_Checkpoint.ModItem ModItem { get; }
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
