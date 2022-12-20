using System;
using System.Text;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.World.Generator;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.SessionComponents
{
	public abstract class MyRespawnComponentBase : MySessionComponentBase
	{
<<<<<<< HEAD
		/// <summary>
		/// Indicates if permadeath ownership warning should be shown.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		protected static bool ShowPermaWarning { get; set; }

		public static event Action<MyPlayer> RespawnRequested;

		public abstract void InitFromCheckpoint(MyObjectBuilder_Checkpoint checkpoint);

		public abstract void SaveToCheckpoint(MyObjectBuilder_Checkpoint checkpoint);

		public abstract bool HandleRespawnRequest(bool joinGame, bool newIdentity, long respawnEntityId, string respawnShipId, MyPlayer.PlayerId playerId, Vector3D? spawnPosition, Vector3? direction, Vector3? up, SerializableDefinitionId? botDefinitionId, bool realPlayer, string modelName, Color color);

		public abstract MyIdentity CreateNewIdentity(string identityName, MyPlayer.PlayerId playerId, string modelName, bool initialPlayer = false);

		public abstract void AfterRemovePlayer(MyPlayer player);

		public abstract void SetupCharacterDefault(MyPlayer player, MyWorldGenerator.Args args);

		public abstract bool IsInRespawnScreen();

		public abstract void CloseRespawnScreen();

		public abstract void CloseRespawnScreenNow();

		public abstract void SetNoRespawnText(StringBuilder text, int timeSec);

		public abstract void SetupCharacterFromStarts(MyPlayer player, MyWorldGeneratorStartingStateBase[] playerStarts, MyWorldGenerator.Args args);

		public void ResetPlayerIdentity(MyPlayer player, string modelName, Color color)
		{
			if (player.Identity != null && MySession.Static.Settings.PermanentDeath.Value)
			{
				if (!player.Identity.IsDead)
				{
					Sync.Players.KillPlayer(player);
				}
				IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(player.Identity.IdentityId);
				if (myFaction != null)
				{
					MyFactionCollection.KickMember(myFaction.FactionId, player.Identity.IdentityId);
				}
				MySession.Static.ChatSystem.ChatHistory.ClearNonGlobalHistory();
				MyIdentity myIdentity2 = (player.Identity = Sync.Players.CreateNewIdentity(player.DisplayName, modelName, color));
			}
		}

		protected static void NotifyRespawnRequested(MyPlayer player)
		{
			if (MyRespawnComponentBase.RespawnRequested != null)
			{
				MyRespawnComponentBase.RespawnRequested(player);
			}
		}

		public static Vector3D? FindPositionAbovePlanet(Vector3D friendPosition, ref SpawnInfo info, bool testFreeZone, int distanceIteration, int maxDistanceIterations, float? optimalSpawnDistance = null, MyEntity ignoreEntity = null)
		{
			MyPlanet planet = info.Planet;
			float collisionRadius = info.CollisionRadius;
			Vector3D center = planet.PositionComp.WorldAABB.Center;
			Vector3D axis = Vector3D.Normalize(friendPosition - center);
			float num = ((!optimalSpawnDistance.HasValue) ? MySession.Static.Settings.OptimalSpawnDistance : optimalSpawnDistance.Value);
			float num2 = num * 0.9f;
			for (int i = 0; i < 20; i++)
			{
				Vector3 vector = MyUtils.GetRandomPerpendicularVector(ref axis);
				float num3 = num * (MyUtils.GetRandomFloat(1.05f, 1.15f) + (float)distanceIteration * 0.05f);
				Vector3D globalPos = friendPosition + vector * num3;
				globalPos = planet.GetClosestSurfacePointGlobal(ref globalPos);
				if (testFreeZone && info.MinimalAirDensity > 0f && planet.GetAirDensity(globalPos) < info.MinimalAirDensity)
				{
					continue;
				}
				Vector3 axis2 = Vector3D.Normalize(globalPos - center);
				vector = MyUtils.GetRandomPerpendicularVector(in axis2);
				bool flag = true;
<<<<<<< HEAD
				Vector3 vector2 = vector * collisionRadius;
				Vector3 vector3 = Vector3.Cross(vector2, axis2);
				MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(globalPos, new Vector3D(collisionRadius * 2f, Math.Min(10f, collisionRadius * 0.5f), collisionRadius * 2f), Quaternion.CreateFromForwardUp(vector, axis2));
=======
				Vector3 vector = (Vector3)randomPerpendicularVector * collisionRadius;
				Vector3 vector2 = Vector3.Cross(vector, axis2);
				MyOrientedBoundingBoxD myOrientedBoundingBoxD = new MyOrientedBoundingBoxD(globalPos, new Vector3D(collisionRadius * 2f, Math.Min(10f, collisionRadius * 0.5f), collisionRadius * 2f), Quaternion.CreateFromForwardUp(randomPerpendicularVector, axis2));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				int num4 = -1;
				for (int j = 0; j < 4; j++)
				{
					num4 = -num4;
					int num5 = ((j <= 1) ? 1 : (-1));
<<<<<<< HEAD
					Vector3D point = planet.GetClosestSurfacePointGlobal(globalPos + vector2 * num4 + vector3 * num5);
=======
					Vector3D point = planet.GetClosestSurfacePointGlobal(globalPos + vector * num4 + vector2 * num5);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (!myOrientedBoundingBoxD.Contains(ref point))
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					continue;
				}
				if (testFreeZone && !MyProceduralWorldModule.IsZoneFree(new BoundingSphereD(globalPos, num2)))
				{
					distanceIteration++;
					if (distanceIteration > maxDistanceIterations)
					{
						break;
					}
					continue;
				}
				Vector3D vector3D = Vector3D.Normalize(globalPos - center);
				Vector3D? vector3D2 = MyEntities.FindFreePlace(globalPos + vector3D * info.PlanetDeployAltitude, collisionRadius, 20, 5, 1f, ignoreEntity);
				if (vector3D2.HasValue)
				{
					return vector3D2.Value;
				}
			}
			return null;
		}
	}
}
