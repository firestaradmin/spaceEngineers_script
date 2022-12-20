using VRage.Game;
using VRageMath;

namespace Sandbox.ModAPI
{
	/// <summary>
	/// Interface describing part of weapon block logic
	/// Used in detection if target is visible
	/// </summary>
	public interface IMyShootOrigin
	{
		/// <summary>
		/// Gets shot starting position
		/// </summary>
		Vector3D ShootOrigin { get; }

		/// <summary>
		/// Gets ammo definition
		/// </summary>
		MyDefinitionBase GetAmmoDefinition { get; }

		/// <summary>
		/// Gets weapon max shoot range
		/// </summary>
		float MaxShootRange { get; }
	}
}
