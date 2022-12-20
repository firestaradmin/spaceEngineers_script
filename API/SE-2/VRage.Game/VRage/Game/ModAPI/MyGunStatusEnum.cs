namespace VRage.Game.ModAPI
{
	/// <summary>
	/// When attempting to fire a gun, a status from this enum will be returned.
	/// </summary>
	public enum MyGunStatusEnum
	{
		/// <summary>
		/// Gun is capable of shooting
		/// </summary>
		OK,
		/// <summary>
		/// Gun is cooling down after previous shooting
		/// </summary>
		Cooldown,
		/// <summary>
		/// Gun does not have enough power to shoot
		/// </summary>
		OutOfPower,
		/// <summary>
		/// Gun is damaged beyond functionality
		/// </summary>
		NotFunctional,
		/// <summary>
		/// Gun does not have ammo
		/// </summary>
		OutOfAmmo,
		/// <summary>
		/// Gun is disabled by player
		/// </summary>
		Disabled,
		/// <summary>
		/// Any other reason not given here
		/// </summary>
		Failed,
		/// <summary>
		/// No gun was selected, so nothing could shoot
		/// </summary>
		NotSelected,
		/// <summary>
		/// Shooter does not not have access to the weapon
		/// </summary>
		AccessDenied,
		/// <summary>
		/// whole burst fired, re-press the trigger
		/// </summary>
		BurstLimit,
		/// <summary>
		/// Disabled in safezone
		/// </summary>
		SafeZoneDenied,
		/// <summary>
		/// Gun currently reloading
		/// </summary>
		Reloading,
		/// <summary>
		/// Gun is charging energy
		/// </summary>
		NotCharged
	}
}
