namespace VRage.Game
{
	/// <summary>
	/// Describes faction type.
	/// </summary>
	public enum MyFactionTypes
	{
		/// <summary>
		/// Backward compatibility. In many cases works like <see cref="F:VRage.Game.MyFactionTypes.PlayerMade" />, but in some differs.
		/// </summary>
		None,
		/// <summary>
		/// Faction that was created by player
		/// </summary>
		PlayerMade,
		/// <summary>
		/// NPC miner faction
		/// </summary>
		Miner,
		/// <summary>
		/// NPC trader faction
		/// </summary>
		Trader,
		/// <summary>
		/// NPC trader faction
		/// </summary>
		Builder
	}
}
