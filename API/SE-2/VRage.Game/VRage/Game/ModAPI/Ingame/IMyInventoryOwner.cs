namespace VRage.Game.ModAPI.Ingame
{
	/// <summary>
	/// Describes interface of object that has inventory (PB scripting interface)
	/// </summary>
	public interface IMyInventoryOwner
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets amount of inventories
		/// </summary>
		int InventoryCount { get; }

		/// <summary>
		/// Gets EntityId, which it belongs to
		/// </summary>
		long EntityId { get; }

		/// <summary>
		/// Gets or sets if that inventory can interact with 
		/// </summary>
		bool UseConveyorSystem { get; set; }

		/// <summary>
		/// Gets whether has inventory
		/// </summary>
=======
		int InventoryCount { get; }

		long EntityId { get; }

		bool UseConveyorSystem { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		bool HasInventory { get; }

		/// <summary>
		/// Gets inventory by index
		/// </summary>
		/// <param name="index">Index of inventory, should be less than <see cref="P:VRage.Game.ModAPI.Ingame.IMyInventoryOwner.InventoryCount" /></param>
		/// <returns></returns>
		IMyInventory GetInventory(int index);
	}
}
