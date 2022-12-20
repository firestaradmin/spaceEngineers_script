using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes remote control block (PB scripting interface)
	/// </summary>
	public interface IMyRemoteControl : IMyShipController, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Determines whether the autopilot is currently enabled.
		/// </summary>
		bool IsAutoPilotEnabled { get; }

		/// <summary>
		/// Gets or sets the autopilot speed limit
		/// </summary>
		float SpeedLimit { get; set; }

		/// <summary>
		/// Gets or sets the current flight mode
		/// </summary>
		FlightMode FlightMode { get; set; }

		/// <summary>
		/// Gets or sets the current flight direction
		/// </summary>
		Base6Directions.Direction Direction { get; set; }

		/// <summary>
		/// Gets the current target waypoint
		/// </summary>
		MyWaypointInfo CurrentWaypoint { get; }

		/// <summary>
		/// if true, if collision avoidance is on, autopilot will wait until path is clear to move forward.
		/// </summary>
		bool WaitForFreeWay { get; set; }

		/// <summary>
		/// Gets the nearest player's position. Will only work if the remote control belongs to an NPC
		/// </summary>
		/// <param name="playerPosition"></param>
		/// <returns>True if was able to get player position</returns>
		bool GetNearestPlayer(out Vector3D playerPosition);

		/// <summary>
		/// Removes all existing waypoints.
		/// </summary>
		void ClearWaypoints();

		/// <summary>
		/// Gets basic information about the currently configured waypoints.
		/// </summary>
		/// <param name="waypoints">Buffer array, results would be added into it</param>
		void GetWaypointInfo(List<MyWaypointInfo> waypoints);

		/// <summary>
		/// Adds a new waypoint.
		/// </summary>
		/// <param name="coords">World position of waypoint</param>
		/// <param name="name">Name of waypoint</param>
		void AddWaypoint(Vector3D coords, string name);

		/// <summary>
		/// Adds a new waypoint.
		/// </summary>
		/// <param name="coords">Waypoint info</param>
		void AddWaypoint(MyWaypointInfo coords);

		/// <summary>
		/// Enables or disables the autopilot.
		/// </summary>
		/// <param name="enabled">Should be autopilot enabled or not</param>
		void SetAutoPilotEnabled(bool enabled);

		/// <summary>
		/// Enables or disables collision avoidance.
		/// </summary>
		/// <param name="enabled">Should be collision avoidance enabled or not</param>
		void SetCollisionAvoidance(bool enabled);

		/// <summary>
		/// Enables or disables docking mode.
		/// </summary>
		/// <param name="enabled">Should be docking mode enabled or not</param>
		void SetDockingMode(bool enabled);
	}
}
