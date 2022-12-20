using VRage.ObjectBuilders;

namespace VRage.Game.ModAPI.Ingame
{
	public interface IMyInventoryItem
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets or sets amount of items
		/// </summary>
		MyFixedPoint Amount { get; set; }

		/// <summary>
		/// Gets or sets scale of Floating object
		/// </summary>
		float Scale { get; set; }

		/// <summary>
		/// Gets or sets content of inventory item. Cast it to <see cref="T:VRage.Game.MyObjectBuilder_PhysicalObject" />
		/// </summary>
		MyObjectBuilder_Base Content { get; set; }

		/// <summary>
		/// Item Id. Used to determine exact stack 
		/// </summary>
=======
		MyFixedPoint Amount { get; set; }

		float Scale { get; set; }

		MyObjectBuilder_Base Content { get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		uint ItemId { get; set; }
	}
}
