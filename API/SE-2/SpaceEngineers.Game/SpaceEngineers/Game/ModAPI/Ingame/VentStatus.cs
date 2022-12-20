namespace SpaceEngineers.Game.ModAPI.Ingame
{
	/// <summary>
	/// Describes status for vent block
	/// </summary>
	public enum VentStatus
	{
		/// <summary>
		/// Air vent is depressurized
		/// </summary>
		Depressurized,
		/// <summary>
		/// Air vent is in process of depressurizing
		/// </summary>
		Depressurizing,
		/// <summary>
		/// Air vent is pressurized
		/// </summary>
		Pressurized,
		/// <summary>
		/// Air vent is in process of pressurizing
		/// </summary>
		Pressurizing
	}
}
