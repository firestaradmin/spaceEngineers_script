using VRage.Game;

namespace Sandbox.Game.GameSystems.Conveyors
{
	public class PullInformation
	{
<<<<<<< HEAD
		/// <summary>
		/// Inventory of the block
		/// </summary>
		public MyInventory Inventory { get; set; }

		/// <summary>
		/// Owner of the block
		/// </summary>
		public long OwnerID { get; set; }

		/// <summary>
		/// Inventory constraint in case this block pulls/pushes multiple items
		/// </summary>
		public MyInventoryConstraint Constraint { get; set; }

		/// <summary>
		/// Item definition in case this block only pulls/pushes 1 specific item
		/// </summary>
=======
		public MyInventory Inventory { get; set; }

		public long OwnerID { get; set; }

		public MyInventoryConstraint Constraint { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyDefinitionId ItemDefinition { get; set; }
	}
}
