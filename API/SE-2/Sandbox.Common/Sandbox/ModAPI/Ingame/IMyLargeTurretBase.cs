using System.Collections.Generic;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	/// Describes turret block (PB scripting interface)
	/// </summary>
	public interface IMyLargeTurretBase : IMyUserControllableGun, IMyFunctionalBlock, IMyTerminalBlock, IMyCubeBlock, IMyEntity
	{
		/// <summary>
		/// Indicates whether a block is locally or remotely controlled.
		/// </summary>
		bool IsUnderControl { get; }

<<<<<<< HEAD
		/// <summary>
		/// Returns true if current player can control this block.
		/// Always return false on Dedicated Server
		/// </summary>
		bool CanControl { get; }

		/// <summary>
		/// Gets and Sets shooting range of the turret
		/// </summary>
		float Range { get; set; }

		/// <summary>
		/// Returns true if turret head looking at target
		/// </summary>
=======
		bool CanControl { get; }

		float Range { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		bool IsAimed { get; }

		/// <summary>
		/// Checks if the turret is locked onto a target
		/// </summary>
		bool HasTarget { get; }

		/// <summary>
		/// Gets / sets elevation of turret, this method is not synced, you need to sync elevation manually
		/// </summary>
		float Elevation { get; set; }

		/// <summary>
		/// Gets or sets azimuth of turret, this method is not synced, you need to sync azimuth manually
		/// </summary>
		float Azimuth { get; set; }

		/// <summary>
		/// Enable/disable idle rotation for turret, this method is not synced, you need to sync manually
		/// </summary>
		bool EnableIdleRotation { get; set; }

		/// <summary>
		/// Checks is AI is enabled for turret
		/// </summary>
		bool AIEnabled { get; }
<<<<<<< HEAD

		/// <summary>
		/// Gets/sets if the turret should target meteors.
		/// </summary>
		bool TargetMeteors { get; set; }

		/// <summary>
		/// Gets/sets if the turret should target missiles.
		/// </summary>
		bool TargetMissiles { get; set; }

		/// <summary>
		/// Gets/sets if the turret should target small grids.
		/// </summary>
		bool TargetSmallGrids { get; set; }

		/// <summary>
		/// Gets/sets if the turret should target large grids.
		/// </summary>
		bool TargetLargeGrids { get; set; }

		/// <summary>
		/// Gets/sets if the turret should target characters.
		/// </summary>
		bool TargetCharacters { get; set; }

		/// <summary>
		/// Gets/sets if the turret should target stations.
		/// </summary>
		bool TargetStations { get; set; }

		/// <summary>
		/// Gets/sets if the turret should target neutrals.
		/// </summary>
		bool TargetNeutrals { get; set; }

		/// <summary>
		/// Gets/sets if the turret should target enemies.
		/// </summary>
		bool TargetEnemies { get; set; }
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		/// <summary>
		/// Tracks given target with enabled position prediction
		/// </summary>
		/// <param name="pos">Position of turret</param>
		/// <param name="velocity">Velocity of target</param>
		void TrackTarget(Vector3D pos, Vector3 velocity);

		/// <summary>
		/// Set targets given position
		/// </summary>
		/// <param name="pos">Target world coordinates</param>
		void SetTarget(Vector3D pos);

		/// <summary>
		/// Method used to sync elevation of turret, you need to call it to sync elevation for other clients/server
		/// </summary>
		void SyncElevation();

		/// <summary>
		/// Method used to sync azimuth, you need to call it to sync azimuth for other clients/server
		/// </summary>
		void SyncAzimuth();

		/// <summary>
		/// Method used to sync idle rotation and elevation, you need to call it to sync rotation and elevation for other clients/server
		/// </summary>
		void SyncEnableIdleRotation();

		/// <summary>
		/// Resets targeting to default values
		/// </summary>
		void ResetTargetingToDefault();

		/// <summary>
		/// Gets the turret's current detected entity, if any
		/// </summary>
		/// <returns></returns>
		MyDetectedEntityInfo GetTargetedEntity();

		/// <summary>
		/// Gets all available targeting groups 
		/// </summary>
		List<string> GetTargetingGroups();

		/// <summary>
		/// Gets current targeting group
		/// </summary>
		string GetTargetingGroup();

		/// <summary>
		/// Sets current targeting group
		/// </summary>
		void SetTargetingGroup(string groupSubtypeId);

		/// <summary>
		/// Sets azimuth and elevation of the turret, this method is not synced, you need to sync it manually. Call SyncAzimuth or SyncElevation.
		/// </summary>
		/// <param name="azimuth">azimuth value</param>
		/// <param name="elevation">elevation value</param>
		void SetManualAzimuthAndElevation(float azimuth, float elevation);
	}
}
