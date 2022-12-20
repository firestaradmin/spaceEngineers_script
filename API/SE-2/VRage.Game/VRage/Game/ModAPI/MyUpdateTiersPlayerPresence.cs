namespace VRage.Game.ModAPI
{
	/// <summary>
	/// Describes tiers of player presence (if any player is nearby and grid is replicated to the client)
	/// </summary>
	public enum MyUpdateTiersPlayerPresence
	{
		/// <summary>
		/// Player is nearby, grid is replicated to the client
		/// </summary>
		Normal,
		/// <summary>
		/// Player is not nearby, grid is not replicated to the client. Update time can be slower.
		/// </summary>
		Tier1,
		/// <summary>
		/// Player is not nearby, grid is not replicated to the client. Update time can be even more slower.
		/// </summary>
		Tier2
	}
}
