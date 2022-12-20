namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes bounty contract that player can take in contract block (mods interface)
	/// Player should kill other player
	/// </summary>
	public interface IMyContractBounty : IMyContract
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets IdentityId of target, that should be killed
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		long TargetIdentityId { get; }
	}
}
