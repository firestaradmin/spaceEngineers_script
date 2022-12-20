using System;
using System.Collections.Generic;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	public class MyStationCellGenerator : MyProceduralWorldModule
	{
		private HashSet<MyStation> m_spawnInProgress = new HashSet<MyStation>();

		private HashSet<MyStation> m_removeRequested = new HashSet<MyStation>();

		public MyStationCellGenerator(double cellSize, int radiusMultiplier, int seed, double density, MyProceduralWorldModule parent = null)
			: base(cellSize, radiusMultiplier, seed, density, parent)
		{
		}

		protected override MyProceduralCell GenerateProceduralCell(ref Vector3I cellId)
		{
			MyProceduralCell myProceduralCell = new MyProceduralCell(cellId, CELL_SIZE);
			bool flag = false;
			foreach (KeyValuePair<long, MyFaction> faction in MySession.Static.Factions)
			{
				foreach (MyStation station in faction.Value.Stations)
				{
					if (myProceduralCell.BoundingVolume.Contains(station.Position) == ContainmentType.Contains)
					{
						double stationSpawnDistance = GetStationSpawnDistance(station.Type);
						MyObjectSeed myObjectSeed = new MyObjectSeed(myProceduralCell, station.Position, stationSpawnDistance);
						myObjectSeed.UserData = station;
						myObjectSeed.Params.Type = MyObjectSeedType.Station;
						myObjectSeed.Params.Generated = station.StationEntityId != 0;
						myProceduralCell.AddObject(myObjectSeed);
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return null;
			}
			return myProceduralCell;
		}

		private double GetStationSpawnDistance(MyStationTypeEnum stationType)
		{
			MyDefinitionId subtypeId = default(MyDefinitionId);
			switch (stationType)
			{
			case MyStationTypeEnum.MiningStation:
				subtypeId = MyStationGenerator.MINING_STATIONS_ID;
				break;
			case MyStationTypeEnum.OrbitalStation:
				subtypeId = MyStationGenerator.ORBITAL_STATIONS_ID;
				break;
			case MyStationTypeEnum.Outpost:
				subtypeId = MyStationGenerator.OUTPOST_STATIONS_ID;
				break;
			case MyStationTypeEnum.SpaceStation:
				subtypeId = MyStationGenerator.SPACE_STATIONS_ID;
				break;
			default:
				MyLog.Default.Error($"Stations list for type {stationType} not defined. Go to Economy_Stations.sbc to add definition.");
				break;
			}
			MyStationsListDefinition definition = MyDefinitionManager.Static.GetDefinition<MyStationsListDefinition>(subtypeId);
			if (definition == null)
			{
				return CELL_SIZE;
			}
			return definition.SpawnDistance;
		}

		public override void GenerateObjects(List<MyObjectSeed> list, HashSet<MyObjectSeedParams> existingObjectsSeeds)
		{
			foreach (MyObjectSeed seed in list)
			{
				MyStation station = seed.UserData as MyStation;
				if (station.StationEntityId != 0L)
				{
<<<<<<< HEAD
					if (Sync.IsServer)
					{
						MySession.Static.GetComponent<MySessionComponentEconomy>()?.AddStationGrid(station.StationEntityId);
					}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					continue;
				}
				IMyFaction faction = MySession.Static.Factions.TryGetFactionById(station.FactionId);
				if (faction == null || m_spawnInProgress.Contains(station))
				{
					continue;
				}
				MySafeZone safeZone = station.CreateSafeZone(faction);
				safeZone.AccessTypeGrids = MySafeZoneAccess.Blacklist;
				safeZone.AccessTypeFloatingObjects = MySafeZoneAccess.Blacklist;
				safeZone.AccessTypePlayers = MySafeZoneAccess.Blacklist;
				safeZone.AccessTypeFactions = MySafeZoneAccess.Blacklist;
<<<<<<< HEAD
				string text3 = (safeZone.DisplayName = (safeZone.Name = string.Format(MyTexts.GetString(MySpaceTexts.SafeZone_Name_Station), faction.Tag, station.Id)));
=======
				safeZone.DisplayName = (safeZone.Name = string.Format(MyTexts.GetString(MySpaceTexts.SafeZone_Name_Station), faction.Tag, station.Id));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MySpawnPrefabProperties spawnProperties = new MySpawnPrefabProperties
				{
					Position = station.Position,
					Forward = station.Forward,
					Up = station.Up,
					PrefabName = station.PrefabName,
					OwnerId = faction.FounderId,
					Color = faction.CustomColor,
					SpawningOptions = (SpawningOptions.SetAuthorship | SpawningOptions.ReplaceColor | SpawningOptions.UseOnlyWorldMatrix),
					UpdateSync = true
				};
				m_spawnInProgress.Add(station);
				seed.Params.Generated = true;
				BoundingSphereD boundingSphere = new BoundingSphereD(station.Position, safeZone.Radius);
				ClearToken<MyEntity> clearToken = MyEntities.GetTopMostEntitiesInSphere(ref boundingSphere).GetClearToken();
				try
				{
					foreach (MyEntity item in clearToken.List)
					{
						if (item is MyFloatingObject)
						{
							item.Close();
						}
					}
				}
				finally
<<<<<<< HEAD
				{
					((IDisposable)clearToken).Dispose();
				}
				MyPrefabManager.Static.SpawnPrefabInternal(spawnProperties, delegate
				{
=======
				{
					((IDisposable)clearToken).Dispose();
				}
				MyPrefabManager.Static.SpawnPrefabInternal(spawnProperties, (Action)delegate
				{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_spawnInProgress.Remove(station);
					if (spawnProperties.ResultList != null && spawnProperties.ResultList.Count != 0 && spawnProperties.ResultList.Count <= 1)
					{
						MyCubeGrid myCubeGrid = spawnProperties.ResultList[0];
						if (m_removeRequested.Contains(station))
						{
							RemoveStationGrid(station, myCubeGrid);
							m_removeRequested.Remove(station);
						}
						else
						{
							station.StationEntityId = myCubeGrid.EntityId;
							myCubeGrid.IsGenerated = true;
<<<<<<< HEAD
							string text6 = (myCubeGrid.DisplayName = (myCubeGrid.Name = string.Format(MyTexts.GetString(MySpaceTexts.Grid_Name_Station), faction.Tag, station.Type.ToString(), station.Id)));
=======
							myCubeGrid.DisplayName = (myCubeGrid.Name = string.Format(MyTexts.GetString(MySpaceTexts.Grid_Name_Station), faction.Tag, station.Type.ToString(), station.Id));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							station.ResourcesGenerator.UpdateStation(myCubeGrid);
							station.StationGridSpawned();
							if (Sync.IsServer)
							{
								MySession.Static.GetComponent<MySessionComponentEconomy>()?.AddStationGrid(myCubeGrid.EntityId);
								MyPlanetEnvironmentSessionComponent component = MySession.Static.GetComponent<MyPlanetEnvironmentSessionComponent>();
								if (component != null)
								{
									component.ClearEnvironmentItems(worldBBox: new BoundingBoxD(station.Position - safeZone.Radius, station.Position + safeZone.Radius), entity: safeZone);
								}
							}
						}
					}
<<<<<<< HEAD
				}, delegate
=======
				}, (Action)delegate
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					station.StationEntityId = 0L;
					m_spawnInProgress.Remove(station);
					seed.Params.Generated = false;
				});
			}
		}

		protected override void CloseObjectSeed(MyObjectSeed objectSeed)
		{
			MyStation myStation = objectSeed.UserData as MyStation;
			if (!MyEntities.TryGetEntityById(myStation.SafeZoneEntityId, out MySafeZone entity, allowClosed: false))
			{
				return;
			}
			if (m_spawnInProgress.Contains(myStation))
			{
				m_removeRequested.Add(myStation);
				entity?.Close();
				return;
			}
			entity.Close();
			myStation.SafeZoneEntityId = 0L;
			objectSeed.Params.Generated = false;
			if (myStation.StationEntityId != 0L)
			{
				if (!MyEntities.TryGetEntityById(myStation.StationEntityId, out MyCubeGrid entity2, allowClosed: false))
				{
					MySession.Static.GetComponent<MySessionComponentEconomy>()?.RemoveStationGrid(myStation.StationEntityId);
					myStation.StationEntityId = 0L;
				}
				else
				{
					RemoveStationGrid(myStation, entity2);
				}
			}
		}

		private void RemoveStationGrid(MyStation station, MyCubeGrid stationGrid)
		{
			stationGrid.Close();
			MySession.Static.GetComponent<MySessionComponentEconomy>()?.RemoveStationGrid(station.StationEntityId);
			station.StationEntityId = 0L;
			station.ResourcesGenerator.ClearBlocksCache();
		}

		public override void ReclaimObject(object reclaimedObject)
		{
		}
	}
}
