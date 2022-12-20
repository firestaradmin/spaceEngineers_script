using System;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes contracts that player can take in contract block (mods interface)
	/// </summary>
	public interface IMyContract
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets id of block, that created contract 
		/// </summary>
		long StartBlockId { get; }

		/// <summary>
		/// Gets reward for completion of this contract
		/// </summary>
		int MoneyReward { get; }

		/// <summary>
		/// Gets money that player would loose if he won't complete contract
		/// </summary>
		int Collateral { get; }

		/// <summary>
		/// Gets duration of contract in minutes
		/// </summary>
		int Duration { get; }

		/// <summary>
		/// Gets or sets function that is triggered when contract is acquired by player. IdentityId is passed as an argument 
		/// </summary>
		Action<long> OnContractAcquired { get; set; }

		/// <summary>
		/// Gets or sets function that is triggered when contract is successfully completed 
		/// </summary>
		Action OnContractSucceeded { get; set; }

		/// <summary>
		/// Gets or sets function that is triggered when contract is failed
		/// </summary>
=======
		long StartBlockId { get; }

		int MoneyReward { get; }

		int Collateral { get; }

		int Duration { get; }

		Action<long> OnContractAcquired { get; set; }

		Action OnContractSucceeded { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		Action OnContractFailed { get; set; }
	}
}
