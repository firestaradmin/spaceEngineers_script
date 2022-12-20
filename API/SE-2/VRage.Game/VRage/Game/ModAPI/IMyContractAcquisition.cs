namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes acquisition contract that player can take in contract block (mods interface)
	/// </summary>
	public interface IMyContractAcquisition : IMyContract
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets id of block, that should receive goods
		/// </summary>
		long EndBlockId { get; }

		/// <summary>
		/// Gets id of item that should be delivered
		/// </summary>
		MyDefinitionId ItemTypeId { get; }

		/// <summary>
		/// Gets amount of items that should be delivered
		/// </summary>
=======
		long EndBlockId { get; }

		MyDefinitionId ItemTypeId { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		int ItemAmount { get; }
	}
}
