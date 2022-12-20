using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes Camera block (PB scripting interface)
	/// </summary>
	public interface IMyCameraBlock : IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Gets whether this camera is currently in use by at least one player.
		/// </summary>
		bool IsActive { get; }

		/// <summary>
		/// Gets the maximum distance that this camera can scan, based on the time since the last scan.
		/// </summary>
		double AvailableScanRange { get; }

		/// <summary>
		/// Gets or Sets whether the raycast is enabled.
		/// When this is true, the available raycast distance will count up, and power usage is increased.
		/// </summary>
		bool EnableRaycast { get; set; }

		/// <summary>
		/// Gets the maximum positive angle you can apply for pitch and yaw.
		/// </summary>
		float RaycastConeLimit { get; }

		/// <summary>
		/// Gets the maximum distance you can request a raycast. -1 means infinite.
		/// </summary>
		double RaycastDistanceLimit { get; }

		/// <summary>
		/// Gets the raycast time multiplier that converts time in milliseconds to available raycast distance in meters.
		/// </summary>
		float RaycastTimeMultiplier { get; }

		/// <summary>
		/// Does a raycast in the direction the camera is facing.
		/// </summary>
		/// <param name="distance">distance in meters</param>
		/// <param name="pitch">pitch in degrees</param>
		/// <param name="yaw">yaw in degrees</param>
		/// <returns>Empty if distance or angle are out of bounds.</returns>
		MyDetectedEntityInfo Raycast(double distance, float pitch = 0f, float yaw = 0f);

		/// <summary>
		/// Does a raycast to the specific target point.         
		/// </summary>
		/// <param name="targetPos">target position</param>
		/// <returns>Empty if distance or angle are out of bounds.</returns>
		MyDetectedEntityInfo Raycast(Vector3D targetPos);

		/// <summary>
		/// Does a raycast in the given direction (in camera local space).        
		/// </summary>
		/// <param name="distance">distance in meters</param>
		/// <param name="targetDirection">direction of the target</param>
		/// <returns>Empty if distance or angle are out of bounds.</returns>
		MyDetectedEntityInfo Raycast(double distance, Vector3D targetDirection);

		/// <summary>
		/// Checks if the camera can scan the given distance.
		/// </summary>
		/// <param name="distance">distance in meters</param>
		/// <returns>true if camera can scan</returns>
		bool CanScan(double distance);

		/// <summary>
		/// Checks if the camera can scan to the given direction and distance (in camera local space).
		/// </summary>
		/// <param name="distance">distance in meters</param>
		/// <param name="direction">direction to the target</param>
		/// <returns>true if camera can scan</returns>
		bool CanScan(double distance, Vector3D direction);

		/// <summary>
		/// Checks if the camera can scan to the given target
		/// </summary>
		/// <param name="target">target position</param>
		/// <returns>true if camera can scan</returns>
		bool CanScan(Vector3D target);

		/// <summary>
		/// Calculates time until scan
		/// </summary>
		/// <param name="distance">distance in meters</param>
		/// <returns>number of milliseconds until the camera can do a raycast of the given distance</returns>
		int TimeUntilScan(double distance);
	}
}
