namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes repair contract that player can take in contract block (mods interface)
	/// Player have to completely repair grid for getting reward
	/// </summary>
	public interface IMyContractRepair : IMyContract
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets grid id, that should be repaired
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		long GridId { get; }
	}
}
