using VRage.Game;
using VRage.Game.ModAPI;

namespace VRage.ModAPI
{
	/// <summary>
	/// ModAPI interface giving access to grid-group conveyor system
	/// </summary>
	public interface IMyGridConveyorSystem
	{
		/// <summary>
		/// Implements pull item with possible optional remove. Computation part of this method is done in parallel, so if you call it on new conveyor network, it will not pull anything for the first time.
		/// So the best approach is to call it in some steps, so it does not matter that you don't get result instantly. Be careful not to call it every frame as it can degrade performance.
		/// </summary>
		/// <param name="itemDefinitionId">Item id</param>
		/// <param name="amount">Amount to transfer</param>
		/// <param name="startingBlock">starting block</param>
		/// <param name="destinationInventory">destination inventory</param>
		/// <param name="remove">if true item is removed from inventories instead of transfer</param>        
		/// <returns>amount of item pulled</returns>
		MyFixedPoint PullItem(MyDefinitionId itemDefinitionId, MyFixedPoint? amount, IMyEntity startingBlock, IMyInventory destinationInventory, bool remove);

		/// <summary>
		/// Implements push item from one source block. Item will be generated from source.
		/// </summary>
		/// <param name="sourceBlock">Source block</param>
		/// <param name="itemDefinitionId">Item type to be transferred</param>
		/// <param name="amount">Amount of items to transfer</param>
		/// <param name="transferredAmount">Amount of items that was transferred</param>
		/// <param name="partialPush">If true, items fill be pushed even though not all can fit the conveyor system. Items that can't fit will be thrown away. If false, items will be pushed into system only when all of them fits.</param>        
		/// <returns>Returns info whether all items could fit in target network or not.</returns>
		bool PushGenerateItem(MyDefinitionId itemDefinitionId, MyFixedPoint? amount, out MyFixedPoint transferredAmount, IMyEntity sourceBlock, bool partialPush);
	}
}
