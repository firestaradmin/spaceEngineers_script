namespace Sandbox.Game.Screens
{
	/// <summary>
	///     Server filter GUI will generate these options as checkboxes
	/// </summary>
	public enum MySpaceBoolOptionEnum : byte
	{
		Spectator = 0x80,
		CopyPaste,
		ThrusterDamage,
		PermanentDeath,
		Weapons,
		CargoShips,
		BlockDestruction,
		Scripts,
		Oxygen,
		ThirdPerson,
		Encounters,
		Airtightness,
		UnsupportedStations,
		VoxelDestruction,
		Drones,
		Wolves,
		Spiders,
		RespawnShips,
		ExternalServerManagement
	}
}
