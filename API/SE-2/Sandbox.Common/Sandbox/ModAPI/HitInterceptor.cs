namespace Sandbox.ModAPI
{
	/// <summary>
	/// Provide information about projectiles hit.
	/// </summary>
	public delegate void HitInterceptor(ref MyProjectileInfo projectile, ref MyProjectileHitInfo info);
}
