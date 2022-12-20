using System;
using System.Collections.Generic;
using Sandbox;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.AI;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.AI.Bot;
using VRage.Game.ObjectBuilders.Components;
using VRage.Utils;
using VRageMath;
using VRageMath.Spatial;
using VRageRender;

namespace SpaceEngineers.Game.AI
{
	[MySessionComponentDescriptor(MyUpdateOrder.AfterSimulation, 502, typeof(MyObjectBuilder_SpaceFaunaComponent), null, false)]
	public class MySpaceFaunaComponent : MySessionComponentBase
	{
		private class PlanetAIInfo
		{
			public readonly MyPlanet Planet;

			public int BotNumber;

			public PlanetAIInfo(MyPlanet planet)
			{
				Planet = planet;
				BotNumber = 0;
			}
		}

		private class SpawnInfo
		{
			public int SpawnTime;

			public int AbandonTime;

			public Vector3D Position;

			public readonly MyPlanet Planet;

			public bool SpawnDone;

			public SpawnInfo(Vector3D position, int gameTime, MyPlanet planet)
			{
				MyPlanetAnimalSpawnInfo dayOrNightAnimalSpawnInfo = MySpaceBotFactory.GetDayOrNightAnimalSpawnInfo(planet, position);
				SpawnTime = gameTime + MyUtils.GetRandomInt(dayOrNightAnimalSpawnInfo.SpawnDelayMin, dayOrNightAnimalSpawnInfo.SpawnDelayMax);
				AbandonTime = gameTime + 45000;
				Position = position;
				Planet = planet;
				SpawnDone = false;
			}

			public SpawnInfo(MyObjectBuilder_SpaceFaunaComponent.SpawnInfo info, int currentTime)
			{
				SpawnTime = currentTime + info.SpawnTime;
				AbandonTime = currentTime + info.SpawnTime;
				Position = new Vector3D(info.X, info.Y, info.Z);
				Planet = MyGamePruningStructure.GetClosestPlanet(Position);
				SpawnDone = false;
			}

			public bool ShouldSpawn(int currentTime)
			{
				return SpawnTime - currentTime < 0;
			}

			public bool IsAbandoned(int currentTime)
			{
				return AbandonTime - currentTime < 0;
			}

			public void UpdateAbandoned(int currentTime)
			{
				AbandonTime = currentTime + 45000;
			}
		}

		private class SpawnTimeoutInfo
		{
			public int TimeoutTime;

			public Vector3D Position;

			public readonly MyPlanetAnimalSpawnInfo AnimalSpawnInfo;

			public SpawnTimeoutInfo(Vector3D position, int currentTime)
			{
				TimeoutTime = currentTime;
				Position = position;
				MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(Position);
				AnimalSpawnInfo = MySpaceBotFactory.GetDayOrNightAnimalSpawnInfo(closestPlanet, Position);
				if (AnimalSpawnInfo == null)
				{
					TimeoutTime = currentTime;
				}
			}

			public SpawnTimeoutInfo(MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo info, int currentTime)
			{
				TimeoutTime = currentTime + info.Timeout;
				Position = new Vector3D(info.X, info.Y, info.Z);
				MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(Position);
				AnimalSpawnInfo = MySpaceBotFactory.GetDayOrNightAnimalSpawnInfo(closestPlanet, Position);
				if (AnimalSpawnInfo == null)
				{
					TimeoutTime = currentTime;
				}
			}

			internal void AddKillTimeout()
			{
				if (AnimalSpawnInfo != null)
				{
					TimeoutTime += AnimalSpawnInfo.KillDelay;
				}
			}

			internal bool IsTimedOut(int currentTime)
			{
				return TimeoutTime - currentTime < 0;
			}
		}

		private const string WOLF_SUBTYPE_ID = "Wolf";

		private const int UPDATE_DELAY = 120;

		private const int CLEAN_DELAY = 2400;

		private const int ABANDON_DELAY = 45000;

		private const float DESPAWN_DIST = 1000f;

		private const float SPHERE_SPAWN_DIST = 150f;

		private const float PROXIMITY_DIST = 50f;

		private const float TIMEOUT_DIST = 150f;

<<<<<<< HEAD
=======
		private const int MAX_BOTS_PER_PLANET = 10;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private int m_waitForUpdate = 120;

		private int m_waitForClean = 2400;

		private Action<MyCharacter> m_botCharacterDied;

		private readonly Dictionary<long, PlanetAIInfo> m_planets = new Dictionary<long, PlanetAIInfo>();

		private readonly List<Vector3D> m_tmpPlayerPositions = new List<Vector3D>();

		private readonly MyVector3DGrid<SpawnInfo> m_spawnInfoGrid = new MyVector3DGrid<SpawnInfo>(150.0);

		private readonly List<SpawnInfo> m_allSpawnInfos = new List<SpawnInfo>();

		private readonly MyVector3DGrid<SpawnTimeoutInfo> m_timeoutInfoGrid = new MyVector3DGrid<SpawnTimeoutInfo>(150.0);

		private readonly List<SpawnTimeoutInfo> m_allTimeoutInfos = new List<SpawnTimeoutInfo>();

		private MyObjectBuilder_SpaceFaunaComponent m_obForLoading;

		public override Type[] Dependencies => new Type[1] { typeof(MyAIComponent) };

		public override bool IsRequiredByGame
		{
			get
			{
				if (MyPerGameSettings.Game == GameEnum.SE_GAME)
				{
					return MyPerGameSettings.EnableAi;
				}
				return false;
			}
		}

		static MySpaceFaunaComponent()
		{
		}

		public override void LoadData()
		{
			base.LoadData();
			if (Sync.IsServer)
			{
				MyEntities.OnEntityAdd += EntityAdded;
				MyEntities.OnEntityRemove += EntityRemoved;
				MyAIComponent.Static.BotCreatedEvent += OnBotCreatedEvent;
				m_botCharacterDied = BotCharacterDied;
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			if (Sync.IsServer)
			{
				MyEntities.OnEntityAdd -= EntityAdded;
				MyEntities.OnEntityRemove -= EntityRemoved;
				MyAIComponent.Static.BotCreatedEvent -= OnBotCreatedEvent;
				m_botCharacterDied = null;
				m_planets.Clear();
			}
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			m_obForLoading = sessionComponent as MyObjectBuilder_SpaceFaunaComponent;
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_SpaceFaunaComponent myObjectBuilder_SpaceFaunaComponent = base.GetObjectBuilder() as MyObjectBuilder_SpaceFaunaComponent;
			int totalGamePlayTimeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			int num = 0;
			foreach (SpawnInfo allSpawnInfo in m_allSpawnInfos)
			{
				if (!allSpawnInfo.SpawnDone)
				{
					num++;
				}
			}
			myObjectBuilder_SpaceFaunaComponent.SpawnInfos.Capacity = num;
			foreach (SpawnInfo allSpawnInfo2 in m_allSpawnInfos)
			{
				if (!allSpawnInfo2.SpawnDone)
				{
					MyObjectBuilder_SpaceFaunaComponent.SpawnInfo item = new MyObjectBuilder_SpaceFaunaComponent.SpawnInfo
					{
						X = allSpawnInfo2.Position.X,
						Y = allSpawnInfo2.Position.Y,
						Z = allSpawnInfo2.Position.Z,
						AbandonTime = Math.Max(0, allSpawnInfo2.AbandonTime - totalGamePlayTimeInMilliseconds),
						SpawnTime = Math.Max(0, allSpawnInfo2.SpawnTime - totalGamePlayTimeInMilliseconds)
					};
					myObjectBuilder_SpaceFaunaComponent.SpawnInfos.Add(item);
				}
			}
			myObjectBuilder_SpaceFaunaComponent.TimeoutInfos.Capacity = m_allTimeoutInfos.Count;
			foreach (SpawnTimeoutInfo allTimeoutInfo in m_allTimeoutInfos)
			{
				MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo item2 = new MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo
				{
					X = allTimeoutInfo.Position.X,
					Y = allTimeoutInfo.Position.Y,
					Z = allTimeoutInfo.Position.Z,
					Timeout = Math.Max(0, allTimeoutInfo.TimeoutTime - totalGamePlayTimeInMilliseconds)
				};
				myObjectBuilder_SpaceFaunaComponent.TimeoutInfos.Add(item2);
			}
			return myObjectBuilder_SpaceFaunaComponent;
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			if (m_obForLoading == null)
			{
				return;
			}
			int totalGamePlayTimeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			m_allSpawnInfos.Capacity = m_obForLoading.SpawnInfos.Count;
			foreach (MyObjectBuilder_SpaceFaunaComponent.SpawnInfo spawnInfo2 in m_obForLoading.SpawnInfos)
			{
				SpawnInfo spawnInfo = new SpawnInfo(spawnInfo2, totalGamePlayTimeInMilliseconds);
				m_allSpawnInfos.Add(spawnInfo);
				m_spawnInfoGrid.AddPoint(ref spawnInfo.Position, spawnInfo);
			}
			m_allTimeoutInfos.Capacity = m_obForLoading.TimeoutInfos.Count;
			foreach (MyObjectBuilder_SpaceFaunaComponent.TimeoutInfo timeoutInfo in m_obForLoading.TimeoutInfos)
			{
				SpawnTimeoutInfo spawnTimeoutInfo = new SpawnTimeoutInfo(timeoutInfo, totalGamePlayTimeInMilliseconds);
				if (spawnTimeoutInfo.AnimalSpawnInfo != null)
				{
					m_allTimeoutInfos.Add(spawnTimeoutInfo);
					m_timeoutInfoGrid.AddPoint(ref spawnTimeoutInfo.Position, spawnTimeoutInfo);
				}
			}
			m_obForLoading = null;
		}

		private void EntityAdded(MyEntity entity)
		{
			MyPlanet planet;
			if ((planet = entity as MyPlanet) != null && PlanetHasFauna(planet))
			{
				m_planets.Add(entity.EntityId, new PlanetAIInfo(planet));
			}
		}

		private void EntityRemoved(MyEntity entity)
		{
			if (entity is MyPlanet)
			{
				m_planets.Remove(entity.EntityId);
			}
		}

		private bool PlanetHasFauna(MyPlanet planet)
		{
			if (planet.Generator.AnimalSpawnInfo?.Animals != null)
			{
				return planet.Generator.AnimalSpawnInfo.Animals.Length != 0;
			}
			return false;
		}

		private void SpawnBot(SpawnInfo spawnInfo, MyPlanet planet, MyPlanetAnimalSpawnInfo animalSpawnInfo)
		{
<<<<<<< HEAD
			if (!m_planets.TryGetValue(planet.EntityId, out var value) || value.BotNumber >= planet.Generator.MaxBotCount)
=======
			if (!m_planets.TryGetValue(planet.EntityId, out var value) || value.BotNumber >= 10)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			double minRadius = animalSpawnInfo.SpawnDistMin;
			double maxRadius = animalSpawnInfo.SpawnDistMax;
			Vector3D center = spawnInfo.Position;
			Vector3D vector3D = MyGravityProviderSystem.CalculateNaturalGravityInPoint(center);
			if (vector3D == Vector3D.Zero)
			{
				vector3D = Vector3D.Up;
			}
			vector3D.Normalize();
			Vector3D tangent = Vector3D.CalculatePerpendicularVector(vector3D);
			Vector3D bitangent = Vector3D.Cross(vector3D, tangent);
			tangent.Normalize();
			bitangent.Normalize();
			Vector3D globalPos = MyUtils.GetRandomDiscPosition(ref center, minRadius, maxRadius, ref tangent, ref bitangent);
			globalPos = planet.GetClosestSurfacePointGlobal(ref globalPos);
			Vector3D? vector3D2 = MyEntities.FindFreePlace(globalPos, 2f);
			if (vector3D2.HasValue)
			{
				globalPos = vector3D2.Value;
			}
			planet.CorrectSpawnLocation(ref globalPos, 2.0);
			MyAgentDefinition myAgentDefinition;
			if ((myAgentDefinition = GetAnimalDefinition(animalSpawnInfo) as MyAgentDefinition) != null)
			{
				if (myAgentDefinition.Id.SubtypeName == "Wolf" && MySession.Static.EnableWolfs)
				{
					MyAIComponent.Static.SpawnNewBot(myAgentDefinition, globalPos);
				}
				else if (myAgentDefinition.Id.SubtypeName != "Wolf" && MySession.Static.EnableSpiders)
				{
					MyAIComponent.Static.SpawnNewBot(myAgentDefinition, globalPos);
				}
			}
		}

		private void OnBotCreatedEvent(int botSerialNum, MyBotDefinition botDefinition)
		{
			MyAgentDefinition myAgentDefinition;
			if ((myAgentDefinition = botDefinition as MyAgentDefinition) != null && myAgentDefinition.FactionTag == "SPID" && Sync.Players.TryGetPlayerById(new MyPlayer.PlayerId(Sync.MyId, botSerialNum), out var player))
			{
				player.Controller.ControlledEntityChanged += OnBotControlledEntityChanged;
				MyCharacter myCharacter;
				if ((myCharacter = player.Controller.ControlledEntity as MyCharacter) != null)
				{
					myCharacter.CharacterDied += BotCharacterDied;
				}
			}
		}

		private void OnBotControlledEntityChanged(IMyControllableEntity oldControllable, IMyControllableEntity newControllable)
		{
			MyCharacter myCharacter;
			if ((myCharacter = oldControllable as MyCharacter) != null)
			{
				myCharacter.CharacterDied -= BotCharacterDied;
			}
			MyCharacter myCharacter2;
			if ((myCharacter2 = newControllable as MyCharacter) != null)
			{
				myCharacter2.CharacterDied += BotCharacterDied;
			}
		}

		private void BotCharacterDied(MyCharacter obj)
		{
			Vector3D point = obj.PositionComp.GetPosition();
			obj.CharacterDied -= BotCharacterDied;
			int num = 0;
			MyVector3DGrid<SpawnTimeoutInfo>.Enumerator pointsCloserThan = m_timeoutInfoGrid.GetPointsCloserThan(ref point, 150.0);
			while (pointsCloserThan.MoveNext())
			{
				num++;
				pointsCloserThan.Current.AddKillTimeout();
			}
			if (num == 0)
			{
				int totalGamePlayTimeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				SpawnTimeoutInfo spawnTimeoutInfo = new SpawnTimeoutInfo(point, totalGamePlayTimeInMilliseconds);
				spawnTimeoutInfo.AddKillTimeout();
				m_timeoutInfoGrid.AddPoint(ref point, spawnTimeoutInfo);
				m_allTimeoutInfos.Add(spawnTimeoutInfo);
			}
		}

		private static MyBotDefinition GetAnimalDefinition(MyPlanetAnimalSpawnInfo animalSpawnInfo)
		{
			int randomInt = MyUtils.GetRandomInt(0, animalSpawnInfo.Animals.Length);
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_AnimalBot), animalSpawnInfo.Animals[randomInt].AnimalType);
			return MyDefinitionManager.Static.GetBotDefinition(id) as MyAgentDefinition;
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (!Sync.IsServer)
			{
				return;
			}
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && MyDebugDrawSettings.DEBUG_DRAW_FAUNA_COMPONENT)
			{
				DebugDraw();
			}
			m_waitForUpdate--;
			if (m_waitForUpdate > 0)
			{
				return;
			}
			m_waitForUpdate = 120;
			ICollection<MyPlayer> onlinePlayers = Sync.Players.GetOnlinePlayers();
			m_tmpPlayerPositions.Capacity = Math.Max(m_tmpPlayerPositions.Capacity, onlinePlayers.Count);
			m_tmpPlayerPositions.Clear();
			foreach (KeyValuePair<long, PlanetAIInfo> planet in m_planets)
			{
				planet.Value.BotNumber = 0;
			}
			foreach (MyPlayer item in onlinePlayers)
			{
				if (item.Id.SerialId == 0)
				{
					if (item.Controller.ControlledEntity != null)
					{
						Vector3D position = item.GetPosition();
						m_tmpPlayerPositions.Add(position);
					}
				}
				else if (item.Controller.ControlledEntity != null)
				{
					MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(item.GetPosition());
					if (closestPlanet != null && m_planets.TryGetValue(closestPlanet.EntityId, out var value))
					{
						value.BotNumber++;
					}
				}
			}
			int totalGamePlayTimeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			if (MyFakes.SPAWN_SPACE_FAUNA_IN_CREATIVE)
			{
				foreach (MyPlayer item2 in onlinePlayers)
				{
					if (item2.Controller.ControlledEntity == null)
					{
						continue;
					}
					Vector3D globalPos = item2.GetPosition();
					MyPlanet closestPlanet2 = MyGamePruningStructure.GetClosestPlanet(globalPos);
					if (closestPlanet2 == null || !PlanetHasFauna(closestPlanet2) || !m_planets.TryGetValue(closestPlanet2.EntityId, out var value2))
					{
						continue;
					}
					if (item2.Id.SerialId == 0)
					{
<<<<<<< HEAD
						if ((closestPlanet2.GetClosestSurfacePointGlobal(ref globalPos) - globalPos).LengthSquared() >= 2500.0 || value2.BotNumber >= closestPlanet2.Generator.MaxBotCount)
=======
						if ((closestPlanet2.GetClosestSurfacePointGlobal(ref globalPos) - globalPos).LengthSquared() >= 2500.0 || value2.BotNumber >= 10)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						{
							continue;
						}
						int num = 0;
						MyVector3DGrid<SpawnInfo>.Enumerator pointsCloserThan = m_spawnInfoGrid.GetPointsCloserThan(ref globalPos, 150.0);
						while (pointsCloserThan.MoveNext())
						{
							SpawnInfo current3 = pointsCloserThan.Current;
							num++;
							if (current3.SpawnDone)
							{
								continue;
							}
							if (current3.ShouldSpawn(totalGamePlayTimeInMilliseconds))
							{
								current3.SpawnDone = true;
								MyVector3DGrid<SpawnTimeoutInfo>.Enumerator pointsCloserThan2 = m_timeoutInfoGrid.GetPointsCloserThan(ref globalPos, 150.0);
								bool flag = false;
								while (pointsCloserThan2.MoveNext())
								{
									if (!pointsCloserThan2.Current.IsTimedOut(totalGamePlayTimeInMilliseconds))
									{
										flag = true;
										break;
									}
								}
								if (flag)
								{
									continue;
								}
								MyPlanetAnimalSpawnInfo dayOrNightAnimalSpawnInfo = MySpaceBotFactory.GetDayOrNightAnimalSpawnInfo(closestPlanet2, current3.Position);
								if (dayOrNightAnimalSpawnInfo != null)
								{
									int randomInt = MyUtils.GetRandomInt(dayOrNightAnimalSpawnInfo.WaveCountMin, dayOrNightAnimalSpawnInfo.WaveCountMax);
									for (int i = 0; i < randomInt; i++)
									{
										SpawnBot(current3, closestPlanet2, dayOrNightAnimalSpawnInfo);
									}
								}
							}
							else
							{
								current3.UpdateAbandoned(totalGamePlayTimeInMilliseconds);
							}
						}
						if (num == 0)
						{
							SpawnInfo spawnInfo = new SpawnInfo(globalPos, totalGamePlayTimeInMilliseconds, closestPlanet2);
							m_spawnInfoGrid.AddPoint(ref globalPos, spawnInfo);
							m_allSpawnInfos.Add(spawnInfo);
						}
						continue;
					}
					double num2 = double.MaxValue;
					foreach (Vector3D tmpPlayerPosition in m_tmpPlayerPositions)
					{
						num2 = Math.Min(Vector3D.DistanceSquared(globalPos, tmpPlayerPosition), num2);
					}
					if (num2 > 1000000.0)
					{
						MyAIComponent.Static.RemoveBot(item2.Id.SerialId, removeCharacter: true);
					}
				}
			}
			m_tmpPlayerPositions.Clear();
			m_waitForClean -= 120;
			if (m_waitForClean > 0)
			{
				return;
			}
			MyAIComponent.Static.CleanUnusedIdentities();
			m_waitForClean = 2400;
			for (int j = 0; j < m_allSpawnInfos.Count; j++)
			{
				SpawnInfo spawnInfo2 = m_allSpawnInfos[j];
				if (spawnInfo2.IsAbandoned(totalGamePlayTimeInMilliseconds) || spawnInfo2.SpawnDone)
				{
					m_allSpawnInfos.RemoveAtFast(j);
					Vector3D point = spawnInfo2.Position;
					m_spawnInfoGrid.RemovePoint(ref point);
					j--;
				}
			}
			for (int k = 0; k < m_allTimeoutInfos.Count; k++)
			{
				SpawnTimeoutInfo spawnTimeoutInfo = m_allTimeoutInfos[k];
				if (spawnTimeoutInfo.IsTimedOut(totalGamePlayTimeInMilliseconds))
				{
					m_allTimeoutInfos.RemoveAtFast(k);
					Vector3D point2 = spawnTimeoutInfo.Position;
					m_timeoutInfoGrid.RemovePoint(ref point2);
					k--;
				}
			}
		}

		private void EraseAllInfos()
		{
			foreach (SpawnInfo allSpawnInfo in m_allSpawnInfos)
			{
				allSpawnInfo.SpawnTime = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			}
			foreach (SpawnTimeoutInfo allTimeoutInfo in m_allTimeoutInfos)
			{
				m_timeoutInfoGrid.RemovePoint(ref allTimeoutInfo.Position);
			}
			m_allTimeoutInfos.Clear();
		}

		public void DebugDraw()
		{
			int num = 0;
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, (float)num++ * 13f), "Cleanup in " + m_waitForClean, Color.Red, 0.5f);
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, (float)num++ * 13f), "Planet infos:", Color.GreenYellow, 0.5f);
			foreach (KeyValuePair<long, PlanetAIInfo> planet in m_planets)
			{
				MyRenderProxy.DebugDrawText2D(new Vector2(0f, (float)num++ * 13f), "  Name: " + planet.Value.Planet.Generator.FolderName + ", Id: " + planet.Key + ", Bots: " + planet.Value.BotNumber, Color.LightYellow, 0.5f);
			}
			MyRenderProxy.DebugDrawText2D(new Vector2(0f, (float)num++ * 13f), "Num. of spawn infos: " + m_allSpawnInfos.Count + "/" + m_timeoutInfoGrid.Count, Color.GreenYellow, 0.5f);
			int totalGamePlayTimeInMilliseconds = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			foreach (SpawnInfo allSpawnInfo in m_allSpawnInfos)
			{
				Vector3D position = allSpawnInfo.Position;
				Vector3 vector = allSpawnInfo.Planet.PositionComp.GetPosition() - position;
				vector.Normalize();
				int num2 = Math.Max(0, (allSpawnInfo.SpawnTime - totalGamePlayTimeInMilliseconds) / 1000);
				int num3 = Math.Max(0, (allSpawnInfo.AbandonTime - totalGamePlayTimeInMilliseconds) / 1000);
				if (num2 != 0 && num3 != 0)
				{
					MyRenderProxy.DebugDrawSphere(position, 150f, Color.Yellow, 1f, depthRead: false);
					MyRenderProxy.DebugDrawText3D(position, "Spawning in: " + num2, Color.Yellow, 0.5f, depthRead: false);
					MyRenderProxy.DebugDrawText3D(position - vector * 0.5f, "Abandoned in: " + num3, Color.Yellow, 0.5f, depthRead: false);
				}
			}
			foreach (SpawnTimeoutInfo allTimeoutInfo in m_allTimeoutInfos)
			{
				Vector3D position2 = allTimeoutInfo.Position;
				int num4 = Math.Max(0, (allTimeoutInfo.TimeoutTime - totalGamePlayTimeInMilliseconds) / 1000);
				MyRenderProxy.DebugDrawSphere(position2, 150f, Color.Blue, 1f, depthRead: false);
				MyRenderProxy.DebugDrawText3D(position2, "Timeout: " + num4, Color.Blue, 0.5f, depthRead: false);
			}
		}
	}
}
