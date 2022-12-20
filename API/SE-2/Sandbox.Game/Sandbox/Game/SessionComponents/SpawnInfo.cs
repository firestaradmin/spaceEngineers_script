using Sandbox.Game.Entities;

namespace Sandbox.Game.SessionComponents
{
	public struct SpawnInfo
	{
		/// <summary>
		/// Identity id the algorithm should look friends for
		/// </summary>
		public long IdentityId;

		/// <summary>
		/// Planet to spawn player on
		/// </summary>
		public MyPlanet Planet;

		/// <summary>
		/// Minimal clearance around the spawn area
		/// </summary>
		public float CollisionRadius;

		/// <summary>
		/// Suggested altitude above the planet (specified above) surface
		/// </summary>
		public float PlanetDeployAltitude;

		/// <summary>
		/// Minimal air density required at landing spot
		/// </summary>
		public float MinimalAirDensity;
	}
}
