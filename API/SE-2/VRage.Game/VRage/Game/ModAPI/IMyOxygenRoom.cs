using VRage.Collections;
using VRageMath;

namespace VRage.Game.ModAPI
{
	public interface IMyOxygenRoom
	{
		bool IsAirtight { get; }

		bool IsDirty { get; }

		float EnvironmentOxygen { get; }

		float OxygenAmount { get; }

		int BlockCount { get; }

		Vector3I StartingPosition { get; }

		/// <summary>
		/// HashSet of all the airtight positions in the room (Reference, not a copy!)
		/// </summary>
		HashSetReader<Vector3I> Blocks { get; }

		/// <summary>
		/// Returns the percentage of oxygen in the room compared to the maximum possible oxygen
		/// </summary>
		float OxygenLevel(float gridSize);

		/// <summary>
		/// Returns the volume of oxygen that is not in the room (m^3)
		/// </summary>
		float MissingOxygen(float gridSize);

		/// <summary>
		/// Gets the maximum volume of oxygen in the room (m^3)
		/// </summary>
		float MaxOxygen(float gridSize);
	}
}
