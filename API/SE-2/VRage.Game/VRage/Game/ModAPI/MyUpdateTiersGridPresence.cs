namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes tiers of grid presence (if any other grid is nearby)
	/// </summary>
	public enum MyUpdateTiersGridPresence
	{
		/// <summary>
		/// There is grid nearby
		/// </summary>
		Normal,
		/// <summary>
		/// There is not grid nearby, updates can behave differently (slower or not running)
		/// </summary>
		Tier1
	}
}
