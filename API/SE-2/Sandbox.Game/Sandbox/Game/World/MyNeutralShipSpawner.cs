using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Utils;
using Sandbox.Game.AI.Autopilots;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Multiplayer;
using Sandbox.ModAPI.Ingame;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.World
{
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation, 500, typeof(MyObjectBuilder_NeutralShipSpawner), null, false)]
	internal class MyNeutralShipSpawner : MySessionComponentBase
	{
		public const float NEUTRAL_SHIP_SPAWN_DISTANCE = 8000f;

		public const float NEUTRAL_SHIP_FORBIDDEN_RADIUS = 2000f;

		public const float NEUTRAL_SHIP_DIRECTION_SPREAD = 0.5f;

		public const float NEUTRAL_SHIP_MINIMAL_ROUTE_LENGTH = 10000f;

		public const float NEUTRAL_SHIP_SPAWN_OFFSET = 500f;

		public static TimeSpan NEUTRAL_SHIP_RESCHEDULE_TIME = TimeSpan.FromSeconds(10.0);

		public static TimeSpan NEUTRAL_SHIP_MIN_TIME = TimeSpan.FromMinutes(13.0);

		public static TimeSpan NEUTRAL_SHIP_MAX_TIME = TimeSpan.FromMinutes(17.0);

		public static TimeSpan NEUTRAL_SHIP_LIFE_TIME = TimeSpan.FromMinutes(30.0);

		private const int EVENT_SPAWN_TRY_MAX = 3;

		private static List<MyPhysics.HitInfo> m_raycastHits = new List<MyPhysics.HitInfo>();

		private static List<float> m_spawnGroupCumulativeFrequencies = new List<float>();

		private static float m_spawnGroupTotalFrequencies = 0f;

		private static float[] m_upVecMultipliers = new float[4] { 1f, 1f, -1f, -1f };

		private static float[] m_rightVecMultipliers = new float[4] { 1f, -1f, -1f, 1f };

		private static List<MySpawnGroupDefinition> m_spawnGroups = new List<MySpawnGroupDefinition>();

		private static int m_eventSpawnTry = 0;

		private static List<MyEntity> m_tempFoundEnts = new List<MyEntity>();

		private static List<MyEntity> m_tempFoundEntsDespawn = new List<MyEntity>();

		private static List<Tuple<List<long>, TimeSpan>> m_shipsInProgress = new List<Tuple<List<long>, TimeSpan>>();

		public override bool IsRequiredByGame => MyPerGameSettings.Game == GameEnum.SE_GAME;

		public override void LoadData()
		{
			MySandboxGame.Log.WriteLine("Pre-loading neutral ship spawn groups...");
			foreach (MySpawnGroupDefinition spawnGroupDefinition in MyDefinitionManager.Static.GetSpawnGroupDefinitions())
			{
				if (spawnGroupDefinition.IsCargoShip)
				{
					m_spawnGroups.Add(spawnGroupDefinition);
				}
			}
			m_spawnGroupTotalFrequencies = 0f;
			m_spawnGroupCumulativeFrequencies.Clear();
			foreach (MySpawnGroupDefinition spawnGroup in m_spawnGroups)
			{
				m_spawnGroupTotalFrequencies += spawnGroup.Frequency;
				m_spawnGroupCumulativeFrequencies.Add(m_spawnGroupTotalFrequencies);
			}
			MySandboxGame.Log.WriteLine("End pre-loading neutral ship spawn groups.");
		}

		protected override void UnloadData()
		{
			m_spawnGroupTotalFrequencies = 0f;
			m_spawnGroupCumulativeFrequencies.Clear();
			for (int num = m_shipsInProgress.Count - 1; num >= 0; num--)
			{
				foreach (long item in m_shipsInProgress[num].Item1)
				{
					if (MyEntities.TryGetEntityById(item, out MyCubeGrid entity, allowClosed: false))
					{
						entity.OnGridChanged -= OnGridChanged;
						entity.OnBlockAdded -= OnBlockAddedRemovedOrChanged;
						entity.OnBlockRemoved -= OnBlockAddedRemovedOrChanged;
						entity.OnBlockIntegrityChanged -= OnBlockAddedRemovedOrChanged;
					}
				}
			}
			m_shipsInProgress.Clear();
			m_spawnGroups.Clear();
			m_raycastHits.Clear();
			Session = null;
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_NeutralShipSpawner myObjectBuilder_NeutralShipSpawner = base.GetObjectBuilder() as MyObjectBuilder_NeutralShipSpawner;
			myObjectBuilder_NeutralShipSpawner.ShipsInProgress = new List<MyObjectBuilder_NeutralShipSpawner.ShipTimePair>(m_shipsInProgress.Count);
			foreach (Tuple<List<long>, TimeSpan> item2 in m_shipsInProgress)
			{
				MyObjectBuilder_NeutralShipSpawner.ShipTimePair item = default(MyObjectBuilder_NeutralShipSpawner.ShipTimePair);
				item.EntityIds = new List<long>(item2.Item1.Count);
				item.TimeTicks = item2.Item2.Ticks;
				foreach (long item3 in item2.Item1)
				{
					item.EntityIds.Add(item3);
				}
				myObjectBuilder_NeutralShipSpawner.ShipsInProgress.Add(item);
			}
			return myObjectBuilder_NeutralShipSpawner;
		}

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			MyObjectBuilder_NeutralShipSpawner myObjectBuilder_NeutralShipSpawner = sessionComponent as MyObjectBuilder_NeutralShipSpawner;
			if (myObjectBuilder_NeutralShipSpawner.ShipsInProgress == null)
			{
				return;
			}
			m_shipsInProgress.Clear();
			foreach (MyObjectBuilder_NeutralShipSpawner.ShipTimePair item2 in myObjectBuilder_NeutralShipSpawner.ShipsInProgress)
			{
				List<long> list = new List<long>(item2.EntityIds.Count);
				foreach (long entityId in item2.EntityIds)
				{
					list.Add(entityId);
				}
				Tuple<List<long>, TimeSpan> item = new Tuple<List<long>, TimeSpan>(list, TimeSpan.FromTicks(item2.TimeTicks));
				m_shipsInProgress.Add(item);
			}
		}

		public override void BeforeStart()
		{
			base.BeforeStart();
			if (!Sync.IsServer)
			{
				return;
			}
			bool flag = MyFakes.ENABLE_CARGO_SHIPS && MySession.Static.CargoShipsEnabled;
			MyGlobalEventBase eventById = MyGlobalEvents.GetEventById(new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), "SpawnCargoShip"));
			if (eventById == null && flag)
			{
				MyGlobalEvents.AddGlobalEvent(MyGlobalEventFactory.CreateEvent(new MyDefinitionId(typeof(MyObjectBuilder_GlobalEventBase), "SpawnCargoShip")));
			}
			else if (eventById != null)
			{
				if (flag)
				{
					eventById.Enabled = true;
				}
				else
				{
					eventById.Enabled = false;
				}
			}
			for (int num = m_shipsInProgress.Count - 1; num >= 0; num--)
			{
				foreach (long item in m_shipsInProgress[num].Item1)
				{
					if (MyEntities.TryGetEntityById(item, out MyCubeGrid entity, allowClosed: false))
					{
						entity.OnGridChanged += OnGridChanged;
						entity.OnBlockAdded += OnBlockAddedRemovedOrChanged;
						entity.OnBlockRemoved += OnBlockAddedRemovedOrChanged;
						entity.OnBlockIntegrityChanged += OnBlockAddedRemovedOrChanged;
					}
				}
			}
		}

		public override void UpdateBeforeSimulation()
		{
			base.UpdateBeforeSimulation();
			for (int num = m_shipsInProgress.Count - 1; num >= 0; num--)
			{
				bool flag = false;
				foreach (long item in m_shipsInProgress[num].Item1)
				{
					if (MyEntities.TryGetEntityById(item, out MyCubeGrid entity, allowClosed: false) && entity != null)
					{
						MyRemoteControl firstBlockOfType = entity.GetFirstBlockOfType<MyRemoteControl>();
						if (firstBlockOfType != null && !firstBlockOfType.IsAutopilotEnabled())
						{
							flag = true;
							break;
						}
					}
				}
				if ((m_shipsInProgress[num].Item2 < MySession.Static.ElapsedGameTime || flag) && (!MyEntities.TryGetEntityById(m_shipsInProgress[num].Item1[0], out var entity2) || !IsPlayerNearby(entity2.PositionComp.GetPosition())))
				{
					foreach (long item2 in m_shipsInProgress[num].Item1)
					{
						if (MyEntities.TryGetEntityById(item2, out var entity3))
						{
							entity3.Close();
						}
					}
					m_shipsInProgress.RemoveAtFast(num);
				}
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_NEUTRAL_SHIPS)
			{
				Vector2 screenCoord = new Vector2(400f, 10f);
				for (int num2 = m_shipsInProgress.Count - 1; num2 >= 0; num2--)
				{
					Tuple<List<long>, TimeSpan> tuple = m_shipsInProgress[num2];
					long num3 = tuple.Item1[0];
					TimeSpan timeSpan = tuple.Item2 - MySession.Static.ElapsedGameTime;
					MyRenderProxy.DebugDrawText2D(screenCoord, "GridId: " + num3 + "TimeLeft: " + timeSpan.ToString(), Color.White, 0.5f);
					screenCoord.Y += 10f;
				}
			}
		}

		private static bool IsPlayerNearby(Vector3D nearbyPos)
		{
			using (MyUtils.ReuseCollection(ref m_tempFoundEntsDespawn))
			{
				BoundingSphereD sphere = new BoundingSphereD(nearbyPos, 2000.0);
				MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, m_tempFoundEntsDespawn, MyEntityQueryType.Dynamic);
				foreach (MyEntity item in m_tempFoundEntsDespawn)
				{
					if (item is MyCharacter)
					{
						return true;
					}
				}
				return false;
			}
		}

		private static MySpawnGroupDefinition PickRandomSpawnGroup()
		{
			if (m_spawnGroupCumulativeFrequencies.Count == 0)
			{
				return null;
			}
			float randomFloat = MyUtils.GetRandomFloat(0f, m_spawnGroupTotalFrequencies);
			int i;
			for (i = 0; i < m_spawnGroupCumulativeFrequencies.Count && (!(randomFloat <= m_spawnGroupCumulativeFrequencies[i]) || !m_spawnGroups[i].Enabled); i++)
			{
			}
			if (i >= m_spawnGroupCumulativeFrequencies.Count)
			{
				i = m_spawnGroupCumulativeFrequencies.Count - 1;
			}
			return m_spawnGroups[i];
		}

		private static void GetSafeBoundingBoxForPlayers(Vector3D start, double spawnDistance, out BoundingBoxD output)
		{
			double radius = 10.0;
			BoundingSphereD boundingSphereD = new BoundingSphereD(start, radius);
			ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
			bool flag = true;
			while (flag)
			{
				flag = false;
				foreach (MyPlayer item in onlinePlayers)
				{
					Vector3D position = item.GetPosition();
					double num = (boundingSphereD.Center - position).Length() - boundingSphereD.Radius;
					if (!(num <= 0.0) && !(num > spawnDistance * 2.0))
					{
						boundingSphereD.Include(new BoundingSphereD(position, radius));
						flag = true;
					}
				}
			}
			boundingSphereD.Radius += spawnDistance;
			output = new BoundingBoxD(boundingSphereD.Center - new Vector3D(boundingSphereD.Radius), boundingSphereD.Center + new Vector3D(boundingSphereD.Radius));
			List<MyEntity> entitiesInAABB = MyEntities.GetEntitiesInAABB(ref output);
			foreach (MyEntity item2 in entitiesInAABB)
			{
				if (item2 is MyCubeGrid)
				{
					MyCubeGrid myCubeGrid = item2 as MyCubeGrid;
					if (myCubeGrid.IsStatic)
					{
						Vector3D position2 = myCubeGrid.PositionComp.GetPosition();
						output.Include(new BoundingBoxD(position2 - spawnDistance, position2 + spawnDistance));
					}
				}
			}
			entitiesInAABB.Clear();
		}

		[MyGlobalEventHandler(typeof(MyObjectBuilder_GlobalEventBase), "SpawnCargoShip")]
		public static void OnGlobalSpawnEvent(object senderEvent)
		{
			MySpawnGroupDefinition mySpawnGroupDefinition = PickRandomSpawnGroup();
			if (mySpawnGroupDefinition == null)
			{
				return;
			}
			mySpawnGroupDefinition.ReloadPrefabs();
			SpawningOptions spawningOptions = SpawningOptions.None;
			long ownerId = 0L;
			bool visitStationIfPossible = false;
			if (mySpawnGroupDefinition.IsPirate)
			{
				ownerId = MyPirateAntennas.GetPiratesId();
			}
			else
			{
				spawningOptions |= SpawningOptions.SetNeutralOwner;
				visitStationIfPossible = true;
			}
			if (!MySession.Static.NPCBlockLimits.HasRemainingPCU)
			{
				MySandboxGame.Log.Log(MyLogSeverity.Info, "Pirate PCUs exhausted. Cargo ship will not spawn.");
				return;
			}
			double num = 8000.0;
			Vector3D vector3D = Vector3D.Zero;
			bool flag = MyEntities.IsWorldLimited();
			int num2 = 0;
			if (flag)
			{
				num = Math.Min(num, MyEntities.WorldSafeHalfExtent() - mySpawnGroupDefinition.SpawnRadius);
			}
			else
			{
				ICollection<MyPlayer> onlinePlayers = MySession.Static.Players.GetOnlinePlayers();
				num2 = Math.Max(0, onlinePlayers.Count - 1);
				int randomInt = MyUtils.GetRandomInt(0, num2);
				int num3 = 0;
				foreach (MyPlayer item in onlinePlayers)
				{
					if (num3 == randomInt)
					{
						if (item.Character != null)
						{
							vector3D = item.GetPosition();
						}
						break;
					}
					num3++;
				}
			}
			if (num < 0.0)
			{
				MySandboxGame.Log.WriteLine("Not enough space in the world to spawn such a huge spawn group!");
				return;
			}
			double num4 = 2000.0;
			BoundingBoxD spawnBox;
			if (flag)
			{
				spawnBox = new BoundingBoxD(vector3D - num, vector3D + num);
			}
			else
			{
				GetSafeBoundingBoxForPlayers(vector3D, num, out spawnBox);
				num4 += spawnBox.HalfExtents.Max() - 2000.0;
			}
			Vector3D? vector3D2 = null;
			for (int i = 0; i < 10; i++)
			{
				vector3D2 = MyEntities.TestPlaceInSpace(new Vector3D?(MyUtils.GetRandomBorderPosition(ref spawnBox)).Value, mySpawnGroupDefinition.SpawnRadius);
				if (vector3D2.HasValue)
				{
					break;
				}
			}
			if (!vector3D2.HasValue)
			{
				RetryEventWithMaxTry(senderEvent as MyGlobalEventBase);
				return;
			}
			_ = Vector3D.Zero;
<<<<<<< HEAD
			Vector3 direction = Vector3.One;
=======
			Vector3 direction = Vector3D.One;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			float num5 = (float)Math.Atan(num4 / (vector3D2.Value - spawnBox.Center).Length());
			direction = -Vector3.Normalize(vector3D2.Value);
			float randomFloat = MyUtils.GetRandomFloat(num5, num5 + 0.5f);
			float randomRadian = MyUtils.GetRandomRadian();
			Vector3 vector = Vector3.CalculatePerpendicularVector(direction);
			Vector3 vector2 = Vector3.Cross(direction, vector);
			vector *= (float)(Math.Sin(randomFloat) * Math.Cos(randomRadian));
			vector2 *= (float)(Math.Sin(randomFloat) * Math.Sin(randomRadian));
			direction = direction * (float)Math.Cos(randomFloat) + vector + vector2;
			double? num6 = new RayD(vector3D2.Value, direction).Intersects(spawnBox);
			Vector3D vector3D3 = ((num6.HasValue && !(num6.Value < 10000.0)) ? ((Vector3D)(direction * (float)num6.Value)) : ((Vector3D)(direction * 10000f)));
			_ = vector3D2.Value + vector3D3;
			Vector3 vector3 = Vector3.CalculatePerpendicularVector(direction);
			Vector3 vector4 = Vector3.Cross(direction, vector3);
			MatrixD matrix = MatrixD.CreateWorld(vector3D2.Value, direction, vector3);
			m_raycastHits.Clear();
			foreach (MySpawnGroupDefinition.SpawnGroupPrefab prefab in mySpawnGroupDefinition.Prefabs)
			{
				MyPrefabDefinition prefabDefinition = MyDefinitionManager.Static.GetPrefabDefinition(prefab.SubtypeId);
				Vector3D vector3D4 = Vector3.Transform(prefab.Position, matrix);
				Vector3D vector3D5 = vector3D4 + vector3D3;
				float num7 = prefabDefinition?.BoundingSphere.Radius ?? 10f;
				if (MyGravityProviderSystem.IsPositionInNaturalGravity(vector3D4, mySpawnGroupDefinition.SpawnRadius))
				{
					RetryEventWithMaxTry(senderEvent as MyGlobalEventBase);
					return;
				}
				if (MyGravityProviderSystem.IsPositionInNaturalGravity(vector3D5, mySpawnGroupDefinition.SpawnRadius))
				{
					RetryEventWithMaxTry(senderEvent as MyGlobalEventBase);
					return;
				}
				if (MyGravityProviderSystem.DoesTrajectoryIntersectNaturalGravity(vector3D4, vector3D5, mySpawnGroupDefinition.SpawnRadius + 500f))
				{
					RetryEventWithMaxTry(senderEvent as MyGlobalEventBase);
					return;
				}
				MyPhysics.CastRay(vector3D4, vector3D5, m_raycastHits, 24);
				if (m_raycastHits.Count > 0)
				{
					RetryEventWithMaxTry(senderEvent as MyGlobalEventBase);
					return;
				}
				for (int j = 0; j < 4; j++)
				{
					Vector3D vector3D6 = vector3 * m_upVecMultipliers[j] * num7 + vector4 * m_rightVecMultipliers[j] * num7;
					MyPhysics.CastRay(vector3D4 + vector3D6, vector3D5 + vector3D6, m_raycastHits, 24);
					if (m_raycastHits.Count > 0)
					{
						RetryEventWithMaxTry(senderEvent as MyGlobalEventBase);
						return;
					}
				}
			}
			foreach (MySpawnGroupDefinition.SpawnGroupPrefab shipPrefab in mySpawnGroupDefinition.Prefabs)
			{
				Vector3D vector3D7 = Vector3D.Transform((Vector3D)shipPrefab.Position, matrix);
				Vector3D shipDestination = vector3D7 + vector3D3;
				Vector3 up = Vector3.CalculatePerpendicularVector(-direction);
				List<MyCubeGrid> tmpGridList = new List<MyCubeGrid>();
				Stack<Action> val = new Stack<Action>();
				val.Push((Action)delegate
				{
					InitCargoShip(shipDestination, direction, spawnBox, visitStationIfPossible, shipPrefab, tmpGridList);
				});
				spawningOptions |= SpawningOptions.RotateFirstCockpitTowardsDirection | SpawningOptions.SpawnRandomCargo | SpawningOptions.DisableDampeners | SpawningOptions.SetAuthorship;
<<<<<<< HEAD
				MyPrefabManager.Static.SpawnPrefab(tmpGridList, shipPrefab.SubtypeId, vector3D7, direction, up, shipPrefab.Speed * direction, default(Vector3), shipPrefab.BeaconText, null, spawningOptions, ownerId, updateSync: false, stack);
=======
				MyPrefabManager.Static.SpawnPrefab(tmpGridList, shipPrefab.SubtypeId, vector3D7, direction, up, shipPrefab.Speed * direction, default(Vector3), shipPrefab.BeaconText, null, spawningOptions, ownerId, updateSync: false, val);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_eventSpawnTry = 0;
		}

		private static void InitCargoShip(Vector3D shipDestination, Vector3 direction, BoundingBoxD spawnBox, bool visitStationIfPossible, MySpawnGroupDefinition.SpawnGroupPrefab shipPrefab, List<MyCubeGrid> tmpGridList)
		{
			bool flag = false;
			if (tmpGridList == null || (tmpGridList != null && tmpGridList.Count == 0))
			{
				MyLog.Default.WriteLine("Cargo Ship failed to spawn - " + shipPrefab.SubtypeId + " ");
				return;
			}
			MyRemoteControl firstBlockOfType = tmpGridList[0].GetFirstBlockOfType<MyRemoteControl>();
			if (visitStationIfPossible && firstBlockOfType != null)
			{
				flag = SetVisitStationWaypoints(shipDestination, spawnBox, tmpGridList, firstBlockOfType, shipPrefab);
			}
			if (!flag && firstBlockOfType != null)
			{
				SetOneDestinationPoint(shipDestination, tmpGridList, firstBlockOfType, shipPrefab);
				flag = true;
			}
			if (!flag)
			{
				InitAutopilot(tmpGridList, shipDestination, direction, shipPrefab.SubtypeId);
			}
			else
			{
				foreach (MyCubeGrid tmpGrid in tmpGridList)
				{
					tmpGrid.OnGridChanged += OnGridChanged;
					tmpGrid.OnBlockAdded += OnBlockAddedRemovedOrChanged;
					tmpGrid.OnBlockRemoved += OnBlockAddedRemovedOrChanged;
					tmpGrid.OnBlockIntegrityChanged += OnBlockAddedRemovedOrChanged;
				}
			}
			foreach (MyCubeGrid tmpGrid2 in tmpGridList)
			{
				tmpGrid2.ActivatePhysics();
			}
		}

		private static void OnBlockAddedRemovedOrChanged(MySlimBlock block)
		{
			OnGridChanged(block.CubeGrid);
		}

		private static void OnGridChanged(MyCubeGrid grid)
		{
			for (int num = m_shipsInProgress.Count - 1; num >= 0; num--)
			{
				if (m_shipsInProgress[num].Item1.Contains(grid.EntityId))
				{
					m_shipsInProgress.RemoveAtFast(num);
					grid.OnGridChanged -= OnGridChanged;
					grid.OnBlockAdded -= OnBlockAddedRemovedOrChanged;
					grid.OnBlockRemoved -= OnBlockAddedRemovedOrChanged;
					grid.OnBlockIntegrityChanged -= OnBlockAddedRemovedOrChanged;
				}
			}
		}

		private static void SetOneDestinationPoint(Vector3D shipDestination, List<MyCubeGrid> tmpGridList, MyRemoteControl remoteBlock, MySpawnGroupDefinition.SpawnGroupPrefab shipPrefab)
		{
			remoteBlock.AddWaypoint(new MyWaypointInfo("endWaypoint", shipDestination));
			SetupRemoteBlock(remoteBlock, shipPrefab);
			TimeSpan item = MySession.Static.ElapsedGameTime + NEUTRAL_SHIP_LIFE_TIME;
			List<long> list = new List<long>(tmpGridList.Count);
			foreach (MyCubeGrid tmpGrid in tmpGridList)
			{
				list.Add(tmpGrid.EntityId);
			}
			Tuple<List<long>, TimeSpan> item2 = new Tuple<List<long>, TimeSpan>(list, item);
			m_shipsInProgress.Add(item2);
		}

		private static bool SetVisitStationWaypoints(Vector3D shipDestination, BoundingBoxD spawnBox, List<MyCubeGrid> tmpGridList, MyRemoteControl remoteBlock, MySpawnGroupDefinition.SpawnGroupPrefab shipPrefab)
		{
			tmpGridList[0].RecalculateOwners();
			long num = ((tmpGridList[0].BigOwners.Count > 0) ? tmpGridList[0].BigOwners[0] : 0);
			if (num != 0L)
			{
				using (MyUtils.ReuseCollection(ref m_tempFoundEnts))
				{
					BoundingBoxD boundingBoxD = spawnBox;
					boundingBoxD.Inflate(-400.0);
					MyGamePruningStructure.GetTopMostEntitiesInBox(ref spawnBox, m_tempFoundEnts, MyEntityQueryType.Static);
					foreach (MyEntity tempFoundEnt in m_tempFoundEnts)
					{
						MyCubeGrid myCubeGrid;
						if ((myCubeGrid = tempFoundEnt as MyCubeGrid) == null)
						{
							continue;
						}
						MyStoreBlock firstBlockOfType = myCubeGrid.GetFirstBlockOfType<MyStoreBlock>();
						if (firstBlockOfType == null || !firstBlockOfType.IsFunctional)
						{
							continue;
						}
						MatrixD worldMatrix = tempFoundEnt.WorldMatrix;
						if (MyGravityProviderSystem.IsPositionInNaturalGravity(worldMatrix.Translation) && (double)MyGravityProviderSystem.CalculateNaturalGravityInPoint(worldMatrix.Translation).LengthSquared() > 0.036)
						{
							continue;
						}
						long num2 = ((myCubeGrid.BigOwners.Count > 0) ? myCubeGrid.BigOwners[0] : 0);
						if (num2 == 0L)
						{
							continue;
						}
						IMyFaction myFaction = MySession.Static.Factions.TryGetPlayerFaction(num);
						if (myFaction == null)
						{
							continue;
						}
						IMyFaction myFaction2 = MySession.Static.Factions.TryGetPlayerFaction(num2);
						if (myFaction2 == null)
						{
							continue;
						}
						Tuple<MyRelationsBetweenFactions, int> relationBetweenFactions = MySession.Static.Factions.GetRelationBetweenFactions(myFaction.FactionId, myFaction2.FactionId);
						if (relationBetweenFactions.Item1 == MyRelationsBetweenFactions.Enemies || relationBetweenFactions.Item1 == MyRelationsBetweenFactions.Neutral)
						{
							continue;
						}
						float radius = tempFoundEnt.PositionComp.LocalVolume.Radius;
						Vector3D coords = worldMatrix.Translation + worldMatrix.Backward * 500.0;
						Vector3D vector3D = worldMatrix.Translation + worldMatrix.Backward * radius * 2.0 + worldMatrix.Left * radius * 1.5;
						Vector3D vector3D2 = vector3D + worldMatrix.Forward * radius * 2.0;
						Vector3D coords2 = vector3D2 + worldMatrix.Forward * 500.0;
						Vector3D vector3D3 = worldMatrix.Translation - tmpGridList[0].PositionComp.GetPosition();
						if (MyGravityProviderSystem.IsPositionInNaturalGravity(worldMatrix.Translation + vector3D3))
						{
							vector3D3 = tmpGridList[0].PositionComp.GetPosition() - worldMatrix.Translation;
							_ = worldMatrix.Translation + vector3D3;
						}
						Vector3D coords3 = worldMatrix.Translation + vector3D3;
						remoteBlock.AddWaypoint(new MyWaypointInfo("wp1", coords));
						remoteBlock.AddWaypoint(new MyWaypointInfo("wp2", vector3D));
						remoteBlock.AddWaypoint(new MyWaypointInfo("wp3", vector3D2));
						remoteBlock.AddWaypoint(new MyWaypointInfo("wp4", coords2));
						remoteBlock.AddWaypoint(new MyWaypointInfo("wp5", coords3));
						SetupRemoteBlock(remoteBlock, shipPrefab);
						TimeSpan item = MySession.Static.ElapsedGameTime + NEUTRAL_SHIP_LIFE_TIME;
						List<long> list = new List<long>(tmpGridList.Count);
						foreach (MyCubeGrid tmpGrid in tmpGridList)
						{
							list.Add(tmpGrid.EntityId);
						}
						Tuple<List<long>, TimeSpan> item2 = new Tuple<List<long>, TimeSpan>(list, item);
						m_shipsInProgress.Add(item2);
						return true;
					}
				}
			}
			return false;
		}

		private static void SetupRemoteBlock(MyRemoteControl remoteBlock, MySpawnGroupDefinition.SpawnGroupPrefab shipPrefab)
		{
			remoteBlock.SetCollisionAvoidance(enabled: true);
			remoteBlock.ChangeDirection(Base6Directions.Direction.Forward);
			remoteBlock.SetAutoPilotSpeedLimit(shipPrefab.Speed);
			remoteBlock.SetWaypointThresholdDistance(1f);
			remoteBlock.ChangeFlightMode(FlightMode.OneWay);
			remoteBlock.SetWaitForFreeWay(waitForFreeWay: true);
			remoteBlock.SetAutoPilotEnabled(enabled: true);
		}

		private static void InitAutopilot(List<MyCubeGrid> tmpGridList, Vector3D shipDestination, Vector3D direction, string prefabSubtype)
		{
			foreach (MyCubeGrid tmpGrid in tmpGridList)
			{
				MyCockpit firstBlockOfType = tmpGrid.GetFirstBlockOfType<MyCockpit>();
				if (firstBlockOfType != null)
				{
					MySimpleAutopilot newAutopilot = new MySimpleAutopilot(shipDestination, direction, tmpGridList.ToArray((MyCubeGrid x) => x.EntityId));
					firstBlockOfType.AttachAutopilot(newAutopilot);
					return;
				}
			}
			MyLog.Default.Error("Missing cockpit on spawngroup " + prefabSubtype);
			foreach (MyCubeGrid tmpGrid2 in tmpGridList)
			{
				tmpGrid2.Close();
			}
		}

		private static void RetryEventWithMaxTry(MyGlobalEventBase evt)
		{
			if (evt != null)
			{
				if (++m_eventSpawnTry <= 3)
				{
					MySandboxGame.Log.WriteLine("Could not spawn event. Try " + m_eventSpawnTry + " of " + 3);
					MyGlobalEvents.RescheduleEvent(evt, NEUTRAL_SHIP_RESCHEDULE_TIME);
				}
				else
				{
					m_eventSpawnTry = 0;
				}
			}
		}

		public bool IsEncounter(long entityId)
		{
			foreach (Tuple<List<long>, TimeSpan> item in m_shipsInProgress)
			{
				foreach (long item2 in item.Item1)
				{
					if (item2 == entityId)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
