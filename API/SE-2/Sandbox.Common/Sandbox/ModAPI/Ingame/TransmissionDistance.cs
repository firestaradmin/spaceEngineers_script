namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Specifies how far should be broadcasted message.
	/// </summary>
	public enum TransmissionDistance
	{
		/// <summary>
		/// All PBs on single ship
		/// For more info on constructs see <see cref="M:Sandbox.ModAPI.Ingame.IMyTerminalBlock.IsSameConstructAs(Sandbox.ModAPI.Ingame.IMyTerminalBlock)" />
		/// </summary>
		CurrentConstruct = 0,
		/// <summary>
		/// All PBs on physically connected ships
		/// For more info on constructs see <see cref="M:Sandbox.ModAPI.Ingame.IMyTerminalBlock.IsSameConstructAs(Sandbox.ModAPI.Ingame.IMyTerminalBlock)" />
		/// </summary>
		ConnectedConstructs = 1,
		/// <summary>
		/// All PBs connected over antenna relay
		/// </summary>
		AntennaRelay = 2,
		/// <summary>
		/// Max Distance 
		/// </summary>
		TransmissionDistanceMax = 2
	}
}
