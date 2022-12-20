namespace VRage.Game.ModAPI
{
	/// <summary>
	/// This delegate is used to handle damage before it's applied to an object.  This returns a modified damage that is used in DoDamage.  Return damage if no change.
	/// </summary>
	/// <param name="target">The object that is damaged</param>
	/// <param name="info"></param>
	/// <returns>Modified damage.  Return damage parameter if damage is not modified.</returns>
	public delegate void BeforeDamageApplied(object target, ref MyDamageInformation info);
}
