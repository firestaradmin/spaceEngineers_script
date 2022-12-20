namespace VRage.Game.ModAPI
{
	/// <summary>
	/// ModAPI interface giving access to grid-group weapon system
	/// </summary>
	public interface IMyGridWeaponSystem
	{
		/// <summary>
		/// Get first found gun object with specified definition id.
		/// </summary>
		/// <param name="defId"></param>
		/// <returns></returns>
		IMyGunObject<MyDeviceBase> GetGun(MyDefinitionId defId);

		/// <summary>
		/// Registers gun in weapon system. Required for being able to use it as a ship tool or weapon: `Left mouse hold to shoot`
		/// </summary>
		/// <param name="gun">Interface representing gun</param>
		void Register(IMyGunObject<MyDeviceBase> gun);

		/// <summary>
		/// Unregisters gun in weapon system. 
		/// </summary>
		/// <param name="gun"></param>
		void Unregister(IMyGunObject<MyDeviceBase> gun);
	}
}
