using VRageMath;

namespace VRage.Game.ModAPI
{
	public abstract class MyDeviceBase
	{
		/// <summary>
		/// Reference to the inventory item that this device originated from.
		/// Can be used to update the inventory item (when ammo changes etc...)
		/// </summary>
		public uint? InventoryItemId { get; set; }

		public void Init(MyObjectBuilder_DeviceBase objectBuilder)
		{
			InventoryItemId = objectBuilder.InventoryItemId;
		}

		public abstract Vector3D GetMuzzleLocalPosition();

		public abstract Vector3D GetMuzzleWorldPosition();

		public abstract bool CanSwitchAmmoMagazine();

		public abstract bool SwitchToNextAmmoMagazine();

		public abstract bool SwitchAmmoMagazineToNextAvailable();
	}
}
