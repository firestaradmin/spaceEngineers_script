namespace Sandbox.ModAPI
{
	/// <summary>
	/// Enum defining which sound and effect would be played on explosion trigger
	/// </summary>
	public enum MyExplosionTypeEnum : byte
	{
		MISSILE_EXPLOSION,
		BOMB_EXPLOSION,
		AMMO_EXPLOSION,
		GRID_DEFORMATION,
		GRID_DESTRUCTION,
		WARHEAD_EXPLOSION_02,
		WARHEAD_EXPLOSION_15,
		WARHEAD_EXPLOSION_30,
		WARHEAD_EXPLOSION_50,
		CUSTOM,
		ProjectileExplosion
	}
}
