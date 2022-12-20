using VRageMath;

namespace VRage.Game.ModAPI.Interfaces
{
	/// <summary>
	/// Stores information about user input
	/// </summary>
	public struct MoveInformation
	{
		/// <summary>
		/// Gets user input (W/A/S/D Space/C)
		/// </summary>
		public Vector3 MoveIndicator;

		/// <summary>
		/// Gets user input Mouse (X/Y)
		/// </summary>
		public Vector2 RotationIndicator;

		/// <summary>
		/// Gets user input (Q/E)
		/// </summary>
		public float RollIndicator;
	}
}
