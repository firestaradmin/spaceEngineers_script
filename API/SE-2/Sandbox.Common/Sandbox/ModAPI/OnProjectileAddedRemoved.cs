namespace Sandbox.ModAPI
{
	/// <summary>
	/// Index is used to get position of projectile
	/// Projectile can change it index, when it's index == projectilesCount - 1
	/// </summary>
	public delegate void OnProjectileAddedRemoved(ref MyProjectileInfo projectile, int index);
}
