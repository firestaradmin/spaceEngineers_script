namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes search contract that player can take in contract block (mods interface)
	/// Player have to completely repair grid for getting reward
	/// </summary>
	public interface IMyContractSearch : IMyContract
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets grid that should be found
		/// </summary>
		long TargetGridId { get; }

		/// <summary>
		/// Gets radius in which would be target grid 
		/// </summary>
=======
		long TargetGridId { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		double SearchRadius { get; }
	}
}
