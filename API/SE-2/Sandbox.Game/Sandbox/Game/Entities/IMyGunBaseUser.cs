using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Entities
{
	public interface IMyGunBaseUser
	{
		MyEntity[] IgnoreEntities { get; }

		MyEntity Weapon { get; }

		MyEntity Owner { get; }

		IMyMissileGunObject Launcher { get; }

<<<<<<< HEAD
		/// <summary>
		/// The inventory in which the weapon searches for additional ammo
		/// </summary>
		MyInventory AmmoInventory { get; }

		/// <summary>
		/// The physical item that is being searched for in the weapon inventory. Can be ignored if WeaponInventory is null
		/// </summary>
		MyDefinitionId PhysicalItemId { get; }

		/// <summary>
		/// The inventory in which the weapon searches for it's object builder (e.g. an automatic rifle in character's inventory)
		/// Can be null if the object builder is not to be searched
		/// </summary>
=======
		MyInventory AmmoInventory { get; }

		MyDefinitionId PhysicalItemId { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		MyInventory WeaponInventory { get; }

		long OwnerId { get; }

		string ConstraintDisplayName { get; }
	}
}
