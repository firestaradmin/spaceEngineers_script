using VRage.Game.Components;
using VRageMath;

namespace VRage.Game.ModAPI.Ingame
{
	/// <summary>
	/// Interface for all entities. (PB scripting interface)
	/// </summary>
	public interface IMyEntity
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets blocks component logic container
		/// </summary>
		MyEntityComponentContainer Components { get; }

		/// <summary>
		/// Id of entity
		/// </summary>
		long EntityId { get; }

		/// <summary>
		/// Some entities can have uniq name, and game can find them by name <see cref="M:VRage.ModAPI.IMyEntities.TryGetEntityByName(System.String,VRage.ModAPI.IMyEntity@)" />
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Gets user friendly name of entity. May be null
		/// For block terminal name use <see cref="P:VRage.Game.ModAPI.Ingame.IMyCubeBlock.DisplayNameText" />
		/// </summary>
=======
		MyEntityComponentContainer Components { get; }

		long EntityId { get; }

		string Name { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		string DisplayName { get; }

		/// <summary>
		/// Returns true if this entity has got at least one inventory. 
		/// Note that one aggregate inventory can contain zero simple inventories =&gt; zero will be returned even if GetInventory() != null.
		/// </summary>
		bool HasInventory { get; }

		/// <summary>
		/// Returns the count of the number of inventories this entity has.
		/// </summary>
		int InventoryCount { get; }

<<<<<<< HEAD
		/// <summary>
		/// True if the block has been removed from the world.
		/// </summary>
		bool Closed { get; }

		/// <summary>
		/// Gets world axis-aligned bounding box
		/// </summary>
		BoundingBoxD WorldAABB { get; }

		/// <summary>
		/// Gets world axis-aligned bounding box
		/// </summary>
		BoundingBoxD WorldAABBHr { get; }

		/// <summary>
		/// Gets world matrix of this entity
		/// </summary>
		MatrixD WorldMatrix { get; }

		/// <summary>
		/// Gets bounding sphere of this entity
		/// </summary>
		BoundingSphereD WorldVolume { get; }

		/// <summary>
		/// Gets bounding sphere of this entity
		/// </summary>
=======
		BoundingBoxD WorldAABB { get; }

		BoundingBoxD WorldAABBHr { get; }

		MatrixD WorldMatrix { get; }

		BoundingSphereD WorldVolume { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		BoundingSphereD WorldVolumeHr { get; }

		/// <summary>
		/// Simply get the MyInventoryBase component stored in this entity.
		/// </summary>
		/// <returns>Inventory</returns>
		IMyInventory GetInventory();

		/// <summary>
		/// Search for inventory component with maching index.
		/// </summary>
		IMyInventory GetInventory(int index);

		/// <summary>
		/// Gets position in world coordinates
		/// </summary>
		/// <returns>Position</returns>
		Vector3D GetPosition();
	}
}
