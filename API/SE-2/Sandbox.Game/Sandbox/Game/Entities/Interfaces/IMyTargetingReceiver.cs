using System;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Weapons;
using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities.Interfaces
{
	public interface IMyTargetingReceiver : IMyShootOrigin
	{
		bool TargetMissiles { get; set; }

		bool TargetMeteors { get; set; }

		bool TargetCharacters { get; set; }

		bool TargetNeutrals { get; set; }

		bool TargetEnemies { get; set; }

		bool TargetFriends { get; set; }

		bool TargetSmallGrids { get; set; }

		bool TargetLargeGrids { get; set; }

		bool TargetStations { get; set; }

		bool IsTargetLocked { get; }

		long OwnerIdentityId { get; }

		Vector3D EntityPosition { get; }

		Vector3D ShootDirection { get; }

		float SearchRange { get; }

		float ShootRangeSimple { get; set; }

		Sync<MyLargeTurretTargetingSystem.CurrentTargetSync, SyncDirection.FromServer> TargetSync { get; }

		MyGridTargeting GridTargeting { get; }

		MyEntity Entity { get; }

		float MechanicalDamage { get; }

		float SearchRangeSquared { get; }

		float ShootRangeSquared { get; }

		MyTurretTargetingOptions HiddenTargetingOptions { get; }

		void AddPropertiesChangedCallback(Action<MyTerminalBlock> callback);

		void RemovePropertiesChangedCallback(Action<MyTerminalBlock> callback);

		MyStringHash GetTargetingGroupHash();

		bool HasLocalPlayerAccess();

		MyEntity GetTargetingParent();

		bool IsConnected(MyCubeGrid grid);

		Vector3 LookAt(Vector3D target);

		bool IsTargetInView(Vector3D predPos);

		MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long targetIidentityId);
	}
}
