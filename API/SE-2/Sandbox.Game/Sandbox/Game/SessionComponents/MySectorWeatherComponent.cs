using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Physics;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Planet;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Game.WorldEnvironment;
using Sandbox.ModAPI;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.SessionComponents
{
	[StaticEventOwner]
	[MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation | MyUpdateOrder.AfterSimulation, 555, typeof(MyObjectBuilder_SectorWeatherComponent), null, false)]
	public class MySectorWeatherComponent : MySessionComponentBase, IMyWeatherEffects
	{
		private class EffectLightning
		{
			public MyObjectBuilder_WeatherLightning ObjectBuilder;

			public bool Initialized;

			public MyEntity3DSoundEmitter BoltEmitter = new MyEntity3DSoundEmitter(null);

			public MySoundPair LightningSound = new MySoundPair();

			public void Init()
			{
				if (ObjectBuilder == null)
				{
					return;
				}
				if (!Sync.IsDedicated)
				{
					LightningSound.Init(ObjectBuilder.Sound, useLog: false);
					BoltEmitter.SetPosition(ObjectBuilder.Position);
					BoltEmitter.PlaySound(LightningSound);
					if (MySession.Static != null && MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.ReverbDetectorComp != null)
					{
						BoltEmitter.VolumeMultiplier = MyUtils.GetRandomFloat(100f, 150f) / 100f * Math.Min((25f - (float)(MySession.Static.LocalCharacter.ReverbDetectorComp.Grids - 10)) / 25f, 1f);
					}
				}
				if (Sync.IsServer && ObjectBuilder.ExplosionRadius != 0f)
				{
					MyExplosionTypeEnum explosionType = MyExplosionTypeEnum.CUSTOM;
					MyEntity closestPlanet = MyGamePruningStructure.GetClosestPlanet(ObjectBuilder.Position);
					MyExplosionInfo myExplosionInfo = default(MyExplosionInfo);
					myExplosionInfo.PlayerDamage = 0f;
					myExplosionInfo.Damage = ObjectBuilder.Damage;
					myExplosionInfo.ExplosionType = explosionType;
					myExplosionInfo.ExplosionSphere = new BoundingSphereD(ObjectBuilder.Position, ObjectBuilder.ExplosionRadius);
					myExplosionInfo.LifespanMiliseconds = 700;
					myExplosionInfo.ParticleScale = 1f;
					myExplosionInfo.Direction = Vector3.Up;
					myExplosionInfo.AffectVoxels = false;
					myExplosionInfo.KeepAffectedBlocks = true;
					myExplosionInfo.VoxelExplosionCenter = ObjectBuilder.Position;
					myExplosionInfo.ExplosionFlags = MyExplosionFlags.CREATE_DEBRIS | MyExplosionFlags.APPLY_FORCE_AND_DAMAGE | MyExplosionFlags.CREATE_DECALS | MyExplosionFlags.APPLY_DEFORMATION;
					myExplosionInfo.VoxelCutoutScale = 0f;
					myExplosionInfo.PlaySound = true;
					myExplosionInfo.ApplyForceAndDamage = true;
					myExplosionInfo.ObjectsRemoveDelayInMiliseconds = 40;
					myExplosionInfo.StrengthImpulse = ObjectBuilder.BoltImpulseMultiplier;
					myExplosionInfo.OwnerEntity = ((closestPlanet != null) ? closestPlanet : null);
					myExplosionInfo.OriginEntity = closestPlanet?.EntityId ?? 0;
					MyExplosionInfo explosionInfo = myExplosionInfo;
					MyExplosions.AddExplosion(ref explosionInfo);
				}
				Initialized = true;
			}
		}

		protected sealed class RequestLightningServer_003C_003EVRageMath_Vector3D_0023VRageMath_Vector3D : ICallSite<IMyEventOwner, Vector3D, Vector3D, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in Vector3D cameraTranslation, in Vector3D hitPosition, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestLightningServer(cameraTranslation, hitPosition);
			}
		}

		protected sealed class UpdateWeathersOnClients_003C_003EVRage_Game_MyObjectBuilder_WeatherPlanetData_003C_0023_003E : ICallSite<IMyEventOwner, MyObjectBuilder_WeatherPlanetData[], DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyObjectBuilder_WeatherPlanetData[] planetData, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UpdateWeathersOnClients(planetData);
			}
		}

		protected sealed class UpdateLightningsOnClients_003C_003EVRage_Game_MyObjectBuilder_WeatherLightning_003C_0023_003E : ICallSite<IMyEventOwner, MyObjectBuilder_WeatherLightning[], DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyObjectBuilder_WeatherLightning[] lightnings, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UpdateLightningsOnClients(lightnings);
			}
		}

		protected sealed class RequestWeathersUpdate_Implementation_003C_003E : ICallSite<IMyEventOwner, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				RequestWeathersUpdate_Implementation();
			}
		}

		protected sealed class UpdateWeathersOnClient_003C_003EVRage_Game_MyObjectBuilder_WeatherPlanetData_003C_0023_003E : ICallSite<IMyEventOwner, MyObjectBuilder_WeatherPlanetData[], DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in IMyEventOwner _003Cstatic_003E, in MyObjectBuilder_WeatherPlanetData[] planetData, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				UpdateWeathersOnClient(planetData);
			}
		}

		public const float ExtremeFreeze = 0f;

		public const float Freeze = 0.25f;

		public const float Cozy = 0.5f;

		public const float Hot = 0.75f;

		public const float ExtremeHot = 1f;

		private static MySectorWeatherComponent Static;

		private float m_speed;

		private Vector3 m_sunRotationAxis;

		private Vector3 m_baseSunDirection;

		private Vector3 m_sunDirectionNormalized;

		public static Func<Vector3D, Vector3> CalculateGravityInPoint;

		private const int UPDATE_DELAY = 60;

		private int m_updateCounter = -1;

		private const int MAX_DECOY_RADIUS = 50;

		private readonly float REALISTIC_SOUND_MULTIPLIER_WITH_HELMET = 0.3f;

		private List<MyEntity> m_nearbyEntities = new List<MyEntity>();

		private MyWeatherEffectDefinition m_defaultWeather = new MyWeatherEffectDefinition();

		private MyWeatherEffectDefinition m_sourceWeather;

		private MyWeatherEffectDefinition m_targetWeather;

		private MyWeatherEffectDefinition m_currentWeather;

		private float m_targetVolume;

		private float m_volumeTransitionSpeed = 0.05f;

		private float m_nightValue;

		private float m_currentTransition = 1f;

		private readonly float m_transitionSpeed = 0.001f;

		private Vector3 m_gravityVector;

		private List<MyParticleEffect> m_particleEffects = new List<MyParticleEffect>(256);

		private int m_particleEffectIndex;

		private Vector3D[] m_particleSpread = new Vector3D[5];

		private float m_weatherIntensity;

		private float m_originAltitude;

		private float m_surfaceAltitude;

		private MyPlanet m_closestPlanet;

		private bool m_insideVoxel;

		private bool m_insideGrid;

		private bool m_inCockpit;

		private bool m_insideClosedCockpit;

		private IMySourceVoice m_ambientSound;

		private string m_currentSound = "";

		private List<MyObjectBuilder_WeatherPlanetData> m_weatherPlanetData = new List<MyObjectBuilder_WeatherPlanetData>();

		private List<EffectLightning> m_lightnings = new List<EffectLightning>();

		private static readonly MyStringId m_lightningMaterial = MyStringId.GetOrCompute("WeaponLaser");

		private List<MyEntity> m_foundEntities = new List<MyEntity>();

		private List<MyCubeGrid> m_foundGrids = new List<MyCubeGrid>();

		private List<MyPlayer> m_foundPlayers = new List<MyPlayer>();

		private List<MyEntity> m_decoyGrids = new List<MyEntity>();

		public float RotationInterval
		{
			get
			{
				return m_speed;
			}
			set
			{
				m_speed = value;
			}
		}

		public float? FogMultiplierOverride { get; set; }

		public float? FogDensityOverride { get; set; }

		public Vector3? FogColorOverride { get; set; }

		public float? FogSkyboxOverride { get; set; }

		public float? FogAtmoOverride { get; set; }

		public MatrixD? ParticleDirectionOverride { get; set; }

		public Vector3? ParticleVelocityOverride { get; set; }

		public float? SunIntensityOverride { get; set; }

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			Static = this;
			MyObjectBuilder_SectorWeatherComponent myObjectBuilder_SectorWeatherComponent = (MyObjectBuilder_SectorWeatherComponent)sessionComponent;
			m_speed = 60f * MySession.Static.Settings.SunRotationIntervalMinutes;
			if (!myObjectBuilder_SectorWeatherComponent.BaseSunDirection.IsZero)
			{
				m_baseSunDirection = myObjectBuilder_SectorWeatherComponent.BaseSunDirection;
			}
			if (!myObjectBuilder_SectorWeatherComponent.SunDirectionNormalized.IsZero)
			{
				m_sunDirectionNormalized = myObjectBuilder_SectorWeatherComponent.SunDirectionNormalized;
			}
			else
			{
				m_sunDirectionNormalized = CalculateSunDirection();
			}
			base.UpdateOnPause = true;
			InitWeather(myObjectBuilder_SectorWeatherComponent);
		}

		public override void BeforeStart()
		{
			UpdateSunProperties();
		}

		private void UpdateSunProperties()
		{
			if ((double)(Math.Abs(m_baseSunDirection.X) + Math.Abs(m_baseSunDirection.Y) + Math.Abs(m_baseSunDirection.Z)) < 0.001)
			{
				m_baseSunDirection = MySector.SunProperties.BaseSunDirectionNormalized;
				m_sunRotationAxis = MySector.SunProperties.SunRotationAxis;
				if (MySession.Static.Settings.EnableSunRotation && m_speed > 0f && MySession.Static.ElapsedGameTime.Ticks != 0L)
				{
					float angle = -6.283186f * (float)(MySession.Static.ElapsedGameTime.TotalSeconds / (double)m_speed);
					Vector3 baseSunDirection = Vector3.Transform(m_baseSunDirection, Matrix.CreateFromAxisAngle(m_sunRotationAxis, angle));
					baseSunDirection.Normalize();
					m_baseSunDirection = baseSunDirection;
				}
			}
			else
			{
				m_sunRotationAxis = MySector.SunProperties.SunRotationAxis;
			}
			if (MySession.Static.Settings.EnableSunRotation)
			{
				MySector.SunProperties.SunDirectionNormalized = (m_sunDirectionNormalized = CalculateSunDirection());
			}
			else
			{
				MySector.SunProperties.SunDirectionNormalized = m_sunDirectionNormalized;
			}
		}

		public override MyObjectBuilder_SessionComponent GetObjectBuilder()
		{
			MyObjectBuilder_SectorWeatherComponent obj = (MyObjectBuilder_SectorWeatherComponent)base.GetObjectBuilder();
			obj.BaseSunDirection = m_baseSunDirection;
			obj.SunDirectionNormalized = m_sunDirectionNormalized;
			obj.WeatherPlanetData = m_weatherPlanetData.ToArray();
			return obj;
		}

		public override void UpdateBeforeSimulation()
		{
			if (MySession.Static.Settings.EnableSunRotation)
			{
				Vector3 sunDirectionNormalized = CalculateSunDirection();
				MySector.SunProperties.SunDirectionNormalized = (m_sunDirectionNormalized = sunDirectionNormalized);
			}
		}

		private Vector3 CalculateSunDirection()
		{
			float angle = ((m_speed > 0f) ? (6.283186f * (float)(MySession.Static.ElapsedGameTime.TotalSeconds / (double)m_speed)) : 0f);
			Vector3 result = Vector3.Transform(m_baseSunDirection, Matrix.CreateFromAxisAngle(m_sunRotationAxis, angle));
			result.Normalize();
			return result;
		}

		/// <summary>
		/// Calculate and return temperature in any point in the world
		/// </summary>
		/// <param name="worldPoint"></param>
		/// <returns>0 = absolute zero, maximum freeze. 1 = inferno, maximum heat. 0.5f = cosy living condition</returns>
		public static float GetTemperatureInPoint(Vector3D worldPoint)
		{
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(worldPoint);
			if (closestPlanet == null)
			{
				return 0f;
			}
			float oxygenInPoint = MyOxygenProviderSystem.GetOxygenInPoint(worldPoint);
			if (oxygenInPoint < 0.01f)
			{
				return 0f;
			}
			oxygenInPoint = MathHelper.Saturate(oxygenInPoint / 0.6f);
			float num = (float)Vector3D.Distance(closestPlanet.PositionComp.GetPosition(), worldPoint) / closestPlanet.AverageRadius;
			float num2 = (Vector3.Dot(-MySector.SunProperties.SunDirectionNormalized, Vector3.Normalize(worldPoint - closestPlanet.PositionComp.GetPosition())) + 1f) / 2f;
			num2 = 1f - (float)Math.Pow(1f - num2, 0.5);
			MyTemperatureLevel level = MyTemperatureLevel.Cozy;
			if (closestPlanet.Generator != null)
			{
				level = closestPlanet.Generator.DefaultSurfaceTemperature;
			}
			float value = LevelToTemperature(level) * MySession.Static.GetComponent<MySectorWeatherComponent>().GetTemperatureMultiplier(worldPoint);
			float value2 = MathHelper.Lerp(value, MathHelper.Min(value, 0.25f), num2);
			float num3 = 0f;
			if (num < 1f)
			{
				float num4 = 0.8f;
				float amount = MathHelper.Saturate(num / num4);
				return MathHelper.Lerp(1f, value2, amount);
			}
			return MathHelper.Lerp(0f, value2, oxygenInPoint);
		}

		public static MyTemperatureLevel TemperatureToLevel(float temperature)
		{
			if (temperature < 0.125f)
			{
				return MyTemperatureLevel.ExtremeFreeze;
			}
			if (temperature < 0.375f)
			{
				return MyTemperatureLevel.Freeze;
			}
			if (temperature < 0.625f)
			{
				return MyTemperatureLevel.Cozy;
			}
			if (temperature < 0.875f)
			{
				return MyTemperatureLevel.Hot;
			}
			return MyTemperatureLevel.ExtremeHot;
		}

		public static float LevelToTemperature(MyTemperatureLevel level)
		{
<<<<<<< HEAD
			switch (level)
			{
			case MyTemperatureLevel.ExtremeFreeze:
				return 0f;
			case MyTemperatureLevel.Freeze:
				return 0.25f;
			case MyTemperatureLevel.Cozy:
				return 0.5f;
			case MyTemperatureLevel.Hot:
				return 0.75f;
			case MyTemperatureLevel.ExtremeHot:
				return 1f;
			default:
				return 0.5f;
			}
=======
			return level switch
			{
				MyTemperatureLevel.ExtremeFreeze => 0f, 
				MyTemperatureLevel.Freeze => 0.25f, 
				MyTemperatureLevel.Cozy => 0.5f, 
				MyTemperatureLevel.Hot => 0.75f, 
				MyTemperatureLevel.ExtremeHot => 1f, 
				_ => 0.5f, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static bool IsOnDarkSide(Vector3D point)
		{
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(point);
			if (closestPlanet == null)
			{
				return false;
			}
			return IsThereNight(closestPlanet, ref point);
		}

		public static bool IsThereNight(MyPlanet planet, ref Vector3D position)
		{
			Vector3D value = position - planet.PositionComp.GetPosition();
			if ((float)value.Length() > planet.MaximumRadius * 1.1f)
			{
				return false;
			}
			Vector3 vector = Vector3.Normalize(value);
			return Vector3.Dot(MySector.DirectionToSunNormalized, vector) < -0.1f;
		}

		private void InitWeather(MyObjectBuilder_SectorWeatherComponent ob)
		{
			if (ob.WeatherPlanetData != null)
			{
<<<<<<< HEAD
				m_weatherPlanetData = ob.WeatherPlanetData.ToList();
=======
				m_weatherPlanetData = Enumerable.ToList<MyObjectBuilder_WeatherPlanetData>((IEnumerable<MyObjectBuilder_WeatherPlanetData>)ob.WeatherPlanetData);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			ResetWeather(instant: false);
		}

		public void CreateRandomWeather(MyPlanet planet, bool verbose = false)
		{
			if (planet == null)
			{
				if (verbose)
				{
					MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.Get(MyCommonTexts.ChatCommand_Texts_NoPlanet).ToString(), Color.Red);
				}
				return;
			}
			if (planet.Generator.WeatherGenerators == null || planet.Generator.WeatherGenerators.Count == 0)
			{
				if (verbose)
				{
					MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.Get(MyCommonTexts.ChatCommand_Texts_NoWeatherSystem).ToString(), Color.Red);
				}
				return;
			}
			if (planet.Generator.GlobalWeather)
			{
				if (GetWeather(planet.PositionComp.GetPosition()) != null || planet.Generator.WeatherGenerators[0] == null)
				{
					return;
				}
				List<int> list = new List<int>();
				for (int i = 0; i < planet.Generator.WeatherGenerators[0].Weathers.Count; i++)
				{
					for (int j = 0; j < planet.Generator.WeatherGenerators[0].Weathers[i].Weight; j++)
					{
						list.Add(i);
					}
				}
				if (list.Count > 0)
				{
					int randomInt = MyUtils.GetRandomInt(list.Count);
					int randomInt2 = MyUtils.GetRandomInt(planet.Generator.WeatherGenerators[0].Weathers[list[randomInt]].MinLength, planet.Generator.WeatherGenerators[0].Weathers[list[randomInt]].MaxLength);
					SetWeather(planet.Generator.WeatherGenerators[0].Weathers[list[randomInt]].Name, planet.AtmosphereRadius, planet.PositionComp.GetPosition(), verbose: false, Vector3.Zero, randomInt2);
					if (verbose)
					{
						MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.Get(MyCommonTexts.ChatCommand_Texts_RandomWeather).ToString(), Color.Red);
					}
				}
				return;
			}
			foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
			{
				if (onlinePlayer == null || MyGamePruningStructure.GetClosestPlanet(onlinePlayer.GetPosition())?.EntityId != planet.EntityId)
				{
					continue;
				}
				Vector3D worldPosition = planet.GetClosestSurfacePointGlobal(onlinePlayer.GetPosition());
				if (!GetWeather(worldPosition, out var _))
				{
<<<<<<< HEAD
					Vector3D axis = Vector3D.Normalize(planet.PositionComp.GetPosition() - worldPosition);
					Vector3D randomPerpendicularVector = MyUtils.GetRandomPerpendicularVector(ref axis);
=======
					Vector3 axis = Vector3D.Normalize(planet.PositionComp.GetPosition() - worldPosition);
					Vector3D vector3D = MyUtils.GetRandomPerpendicularVector(in axis);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					MyVoxelMaterialDefinition materialAt = planet.GetMaterialAt(ref worldPosition);
					if (materialAt == null || materialAt.MaterialTypeName == null)
					{
						continue;
					}
					foreach (MyWeatherGeneratorSettings weatherGenerator in planet.Generator.WeatherGenerators)
					{
						if (!weatherGenerator.Voxel.Equals(materialAt.MaterialTypeName))
						{
							continue;
						}
						List<int> list2 = new List<int>();
						for (int k = 0; k < weatherGenerator.Weathers.Count; k++)
						{
							for (int l = 0; l < weatherGenerator.Weathers[k].Weight; l++)
							{
								list2.Add(k);
							}
						}
						if (list2.Count <= 0)
						{
							continue;
						}
						int randomInt3 = MyUtils.GetRandomInt(list2.Count);
						int randomInt4 = MyUtils.GetRandomInt(weatherGenerator.Weathers[list2[randomInt3]].MinLength, weatherGenerator.Weathers[list2[randomInt3]].MaxLength);
						int spawnOffset = weatherGenerator.Weathers[list2[randomInt3]].SpawnOffset;
						float num = (float)(75.0 / 668.0 * (double)planet.AtmosphereRadius);
<<<<<<< HEAD
						if (!GetWeather(worldPosition - randomPerpendicularVector * ((float)spawnOffset + num), out var _) && !GetWeather(worldPosition + randomPerpendicularVector * ((float)spawnOffset + num), out var _))
						{
							worldPosition -= randomPerpendicularVector * ((float)spawnOffset + num);
							SetWeather(weatherGenerator.Weathers[list2[randomInt3]].Name, num, worldPosition, verbose: false, randomPerpendicularVector * (2f * ((float)spawnOffset + num) / (float)randomInt4), randomInt4);
=======
						if (!GetWeather(worldPosition - vector3D * ((float)spawnOffset + num), out var _) && !GetWeather(worldPosition + vector3D * ((float)spawnOffset + num), out var _))
						{
							worldPosition -= vector3D * ((float)spawnOffset + num);
							SetWeather(weatherGenerator.Weathers[list2[randomInt3]].Name, num, worldPosition, verbose: false, vector3D * (2f * ((float)spawnOffset + num) / (float)randomInt4), randomInt4);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							if (verbose)
							{
								MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.Get(MyCommonTexts.ChatCommand_Texts_RandomWeather).ToString(), Color.Red);
							}
						}
					}
				}
				else if (verbose)
				{
					MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.Get(MyCommonTexts.ChatCommand_Texts_WeatherIncoming).ToString(), Color.Red);
				}
			}
		}

		public void CreateRandomLightning(MyObjectBuilder_WeatherEffect weatherEffect, MyObjectBuilder_WeatherLightning lightningBuilder, bool doHitGrid, bool doHitPlayer)
		{
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(weatherEffect.Position);
			MyDefinitionManager.Static.GetWeatherEffect(weatherEffect.Weather);
			Vector3D? hitPosition = null;
			bool doDamage = true;
			if (doHitGrid)
			{
				BoundingSphereD sphere = new BoundingSphereD(weatherEffect.Position, weatherEffect.Radius);
				MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, m_foundEntities);
				foreach (MyEntity foundEntity in m_foundEntities)
				{
					if (foundEntity != null && foundEntity is MyCubeGrid)
					{
						MyCubeGrid myCubeGrid = foundEntity as MyCubeGrid;
						if (!myCubeGrid.Immune && !myCubeGrid.IsRespawnGrid && !myCubeGrid.IsPreview && !myCubeGrid.IsRespawnGrid && !myCubeGrid.MarkedForClose && myCubeGrid.InScene && !myCubeGrid.Closed)
						{
							m_foundGrids.Add(myCubeGrid);
						}
					}
				}
				if (m_foundGrids.Count > 0)
				{
					int randomInt = MyUtils.GetRandomInt(0, m_foundGrids.Count);
					MyCubeGrid myCubeGrid2 = m_foundGrids[randomInt];
<<<<<<< HEAD
					int count = myCubeGrid2.CubeBlocks.Count;
=======
					int count = myCubeGrid2.CubeBlocks.get_Count();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (count > 0)
					{
						int randomInt2 = MyUtils.GetRandomInt(0, count);
						if (!myCubeGrid2.Immune && !myCubeGrid2.IsRespawnGrid && !myCubeGrid2.IsPreview && !myCubeGrid2.IsRespawnGrid && !myCubeGrid2.MarkedForClose && myCubeGrid2.InScene && !myCubeGrid2.Closed)
						{
<<<<<<< HEAD
							hitPosition = m_foundGrids[randomInt].CubeBlocks.ElementAt(randomInt2).WorldAABB.Center;
=======
							hitPosition = Enumerable.ElementAt<MySlimBlock>((IEnumerable<MySlimBlock>)m_foundGrids[randomInt].CubeBlocks, randomInt2).WorldAABB.Center;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
					}
				}
				m_foundEntities.Clear();
				m_foundGrids.Clear();
			}
			if (doHitPlayer && !hitPosition.HasValue)
			{
				foreach (MyPlayer onlinePlayer in Sync.Players.GetOnlinePlayers())
				{
					if ((!MySession.Static.IsUserAdmin(onlinePlayer.Id.SteamId) || onlinePlayer.Id.SteamId == 0L || !MySession.Static.RemoteAdminSettings.TryGetValue(onlinePlayer.Id.SteamId, out var value) || !value.HasFlag(AdminSettingsEnum.Untargetable)) && InsideWeather(onlinePlayer.GetPosition(), weatherEffect))
					{
						m_foundPlayers.Add(onlinePlayer);
					}
				}
				if (m_foundPlayers.Count > 0)
				{
					int randomInt3 = MyUtils.GetRandomInt(0, m_foundPlayers.Count);
					hitPosition = m_foundPlayers[randomInt3].GetPosition();
				}
				m_foundPlayers.Clear();
			}
			if (!hitPosition.HasValue)
			{
				hitPosition = weatherEffect.Position + MyUtils.GetRandomVector3Normalized() * weatherEffect.Radius;
				doDamage = false;
			}
			if (hitPosition.HasValue && closestPlanet != null)
			{
				ProcessDecoys(ref hitPosition, closestPlanet);
				MyPhysics.HitInfo? hitInfo = MyPhysics.CastRay(hitPosition.Value - 50f * Vector3.Normalize(closestPlanet.PositionComp.GetPosition() - hitPosition.Value), hitPosition.Value, 15);
				if (hitInfo.HasValue)
				{
					CreateLightning(hitInfo.Value.Position, lightningBuilder, doDamage);
				}
				else
				{
					CreateLightning(hitPosition.Value, lightningBuilder, doDamage);
				}
			}
		}

		internal void RequestLightning(Vector3D cameraTranslation, Vector3D hitPosition)
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestLightningServer, cameraTranslation, hitPosition);
		}

<<<<<<< HEAD
		[Event(null, 368)]
=======
		[Event(null, 367)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void RequestLightningServer(Vector3D cameraTranslation, Vector3D hitPosition)
		{
			if (MySession.Static.GetUserPromoteLevel(MyEventContext.Current.Sender.Value) < MyPromoteLevel.Admin)
			{
				MyEventContext.ValidationFailed();
				return;
			}
			MyObjectBuilder_WeatherLightning lightning = new MyObjectBuilder_WeatherLightning();
			string weather = MySession.Static.GetComponent<MySectorWeatherComponent>().GetWeather(cameraTranslation);
			if (weather != null)
			{
				MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather);
				if (weatherEffect != null && weatherEffect.Lightning != null)
				{
					lightning = MyDefinitionManager.Static.GetWeatherEffect(weather).Lightning;
				}
			}
			MySession.Static.GetComponent<MySectorWeatherComponent>().CreateLightning(hitPosition, lightning);
		}

		private bool ProcessDecoys(ref Vector3D? hitPosition, MyPlanet planet)
		{
			if (planet == null)
			{
				return false;
			}
			m_decoyGrids.Clear();
			MyPlanetEnvironmentComponent myPlanetEnvironmentComponent = planet.Components.Get<MyPlanetEnvironmentComponent>();
			if (myPlanetEnvironmentComponent != null)
			{
				MyEnvironmentSector sectorForPosition = myPlanetEnvironmentComponent.GetSectorForPosition(hitPosition.Value);
				if (sectorForPosition != null && sectorForPosition.DataView != null)
				{
					for (int i = 0; i < sectorForPosition.DataView.Items.Count; i++)
					{
						ItemInfo itemInfo = sectorForPosition.DataView.Items[i];
						if (sectorForPosition.EnvironmentDefinition.Items.TryGetValue(itemInfo.DefinitionIndex, out var value) && value.Type.Name == "Tree")
						{
							Vector3D vector3D = itemInfo.Position + sectorForPosition.SectorCenter;
							if (Vector3D.DistanceSquared(vector3D, hitPosition.Value) < 400.0)
							{
								hitPosition = vector3D;
								return true;
							}
						}
					}
				}
			}
			BoundingSphereD sphere = new BoundingSphereD(hitPosition.Value, 50.0);
			MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, m_decoyGrids, MyEntityQueryType.Static);
			bool flag = AreDecoysWithinRadius(ref sphere, planet, ref hitPosition);
			if (!flag)
			{
				MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, m_decoyGrids, MyEntityQueryType.Dynamic);
				flag = AreDecoysWithinRadius(ref sphere, planet, ref hitPosition);
			}
			if (!flag)
			{
				foreach (MyEntity decoyGrid in m_decoyGrids)
				{
					if (decoyGrid != null)
					{
						MyCubeGrid myCubeGrid = decoyGrid as MyCubeGrid;
						if (myCubeGrid != null)
						{
							foreach (MyCubeBlock fatBlock in myCubeGrid.GetFatBlocks())
							{
								if (!planet.IsUnderGround(fatBlock.PositionComp.GetPosition()))
								{
									MyDecoy myDecoy = fatBlock as MyDecoy;
									if (myDecoy != null && myDecoy.IsWorking && Vector3D.Distance(myDecoy.PositionComp.GetPosition(), hitPosition.Value) < 50.0)
									{
										hitPosition = myDecoy.PositionComp.GetPosition();
										flag = true;
										break;
									}
									MyRadioAntenna myRadioAntenna = fatBlock as MyRadioAntenna;
									if (myRadioAntenna != null && myRadioAntenna.IsWorking && Vector3D.Distance(myRadioAntenna.PositionComp.GetPosition(), hitPosition.Value) < (double)myRadioAntenna.GetRodRadius())
									{
										hitPosition = myRadioAntenna.PositionComp.GetPosition();
										flag = true;
										break;
									}
								}
							}
						}
					}
				}
				return flag;
			}
			return flag;
		}

		public bool AreDecoysWithinRadius(ref BoundingSphereD localSphere, MyPlanet planet, ref Vector3D? hitPosition)
		{
<<<<<<< HEAD
=======
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref localSphere, m_decoyGrids, MyEntityQueryType.Static);
			foreach (MyEntity decoyGrid in m_decoyGrids)
			{
				MyCubeGrid myCubeGrid;
				if (decoyGrid == null || (myCubeGrid = decoyGrid as MyCubeGrid) == null || !myCubeGrid.Decoys.IsValid)
				{
					continue;
				}
<<<<<<< HEAD
				foreach (MyDecoy decoy in myCubeGrid.Decoys)
				{
					if (!planet.IsUnderGround(decoy.PositionComp.GetPosition()) && decoy.IsWorking && Vector3D.Distance(decoy.PositionComp.GetPosition(), hitPosition.Value) < (double)decoy.GetSafetyRodRadius())
					{
						hitPosition = decoy.PositionComp.GetPosition();
						return true;
					}
				}
=======
				Enumerator<MyDecoy> enumerator2 = myCubeGrid.Decoys.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						MyDecoy current2 = enumerator2.get_Current();
						if (!planet.IsUnderGround(current2.PositionComp.GetPosition()) && current2.IsWorking && Vector3D.Distance(current2.PositionComp.GetPosition(), hitPosition.Value) < (double)current2.GetSafetyRodRadius())
						{
							hitPosition = current2.PositionComp.GetPosition();
							return true;
						}
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return false;
		}

		public void CreateLightning(Vector3D position, MyObjectBuilder_WeatherLightning lightning, bool doDamage = true)
		{
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(position);
			Vector3D? hitPosition = position;
			ProcessDecoys(ref hitPosition, closestPlanet);
			MyObjectBuilder_WeatherLightning myObjectBuilder_WeatherLightning = ((lightning != null) ? ((MyObjectBuilder_WeatherLightning)MyObjectBuilderSerializer.Clone(lightning)) : new MyObjectBuilder_WeatherLightning());
			if (!doDamage)
			{
				myObjectBuilder_WeatherLightning.ExplosionRadius = 0f;
			}
			myObjectBuilder_WeatherLightning.Position = hitPosition.Value;
<<<<<<< HEAD
			if (!closestPlanet.IsUnderGround(myObjectBuilder_WeatherLightning.Position))
			{
				m_lightnings.Add(new EffectLightning
				{
					ObjectBuilder = myObjectBuilder_WeatherLightning
				});
				SyncLightning();
			}
=======
			m_lightnings.Add(new EffectLightning
			{
				ObjectBuilder = myObjectBuilder_WeatherLightning
			});
			SyncLightning();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public string GetWeather(Vector3D position)
		{
			foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetDatum in m_weatherPlanetData)
			{
				foreach (MyObjectBuilder_WeatherEffect weather in weatherPlanetDatum.Weathers)
				{
					if (InsideWeather(position, weather))
					{
						return weather.Weather;
					}
				}
			}
			return "Clear";
		}

		public bool GetWeather(Vector3D position, out MyObjectBuilder_WeatherEffect weatherEffect)
		{
			if (m_weatherPlanetData != null)
			{
				foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetDatum in m_weatherPlanetData)
				{
					if (weatherPlanetDatum == null || weatherPlanetDatum.Weathers == null)
					{
						continue;
					}
					foreach (MyObjectBuilder_WeatherEffect weather in weatherPlanetDatum.Weathers)
					{
						if (weather != null)
						{
							Vector3D linePointA = weather.StartPoint;
							Vector3D linePointB = weather.EndPoint;
							if (linePointA != linePointB && Vector3D.Distance(position, MyUtils.GetClosestPointOnLine(ref linePointA, ref linePointB, ref position)) < (double)weather.Radius)
							{
								weatherEffect = weather;
								return true;
							}
							if (linePointA == linePointB && Vector3D.Distance(position, weather.Position) < (double)weather.Radius)
							{
								weatherEffect = weather;
								return true;
							}
						}
					}
				}
			}
			weatherEffect = null;
			return false;
		}

		public List<MyObjectBuilder_WeatherPlanetData> GetWeatherPlanetData()
		{
			return m_weatherPlanetData;
		}

		public bool ReplaceWeather(string weatherEffect, Vector3D? weatherPosition)
		{
			if (string.IsNullOrEmpty(weatherEffect))
			{
				return false;
			}
			foreach (MyWeatherEffectDefinition weatherDefinition in MyDefinitionManager.Static.GetWeatherDefinitions())
			{
				if (weatherDefinition.Public && weatherDefinition.Id.SubtypeName.ToLower() == weatherEffect.ToLower())
				{
					weatherEffect = weatherDefinition.Id.SubtypeName;
					break;
				}
			}
			if (!weatherPosition.HasValue)
			{
				weatherPosition = MySector.MainCamera.Position;
			}
			for (int i = 0; i < m_weatherPlanetData.Count; i++)
			{
				for (int num = m_weatherPlanetData[i].Weathers.Count - 1; num > -1; num--)
				{
					if (Vector3D.Distance(weatherPosition.Value, m_weatherPlanetData[i].Weathers[num].Position) <= (double)m_weatherPlanetData[i].Weathers[num].Radius)
					{
						m_weatherPlanetData[i].Weathers[num].Weather = weatherEffect;
						SyncWeathers();
						return true;
					}
				}
			}
			return false;
		}

		public bool SetWeather(string weatherEffect, float radius, Vector3D? weatherPosition, bool verbose, Vector3D velocity, int length = 0, float intensity = 1f)
		{
			if (string.IsNullOrEmpty(weatherEffect))
			{
				return false;
			}
			string text = null;
			foreach (MyWeatherEffectDefinition weatherDefinition in MyDefinitionManager.Static.GetWeatherDefinitions())
			{
				if (weatherDefinition.Public && weatherDefinition.Id.SubtypeName.ToLower() == weatherEffect.ToLower())
				{
					text = weatherDefinition.Id.SubtypeName;
					break;
				}
			}
			MyPlanet closestPlanet = m_closestPlanet;
			if (!weatherPosition.HasValue)
			{
				if (m_closestPlanet == null)
				{
					if (verbose)
					{
						MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_NoPlanet), Color.Red);
					}
					return false;
				}
				Vector3D globalPos = MySector.MainCamera.WorldMatrix.Translation;
				weatherPosition = m_closestPlanet.GetClosestSurfacePointGlobal(ref globalPos);
			}
			else
			{
				closestPlanet = MyGamePruningStructure.GetClosestPlanet(weatherPosition.Value);
			}
			if (closestPlanet == null)
			{
				if (verbose)
				{
					MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_NoPlanet), Color.Red);
				}
				return false;
			}
			if (weatherPosition.HasValue && weatherEffect.ToLower().Equals("clear"))
			{
				if (RemoveWeather(weatherPosition.Value))
				{
					if (verbose)
					{
						MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_RemovedWeather), Color.Red);
					}
					return true;
				}
				if (verbose)
				{
					MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_NoWeather), Color.Red);
				}
				return true;
			}
<<<<<<< HEAD
			if (text != null && weatherPosition.HasValue)
=======
			if (text != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				radius = ((radius != 0f) ? Math.Abs(radius) : ((float)Math.Max(75.0 / 668.0 * (double)closestPlanet.AtmosphereRadius, 5000.0)));
				BoundingSphereD boundingSphereD = new BoundingSphereD(weatherPosition.Value, radius);
				for (int i = 0; i < m_weatherPlanetData.Count; i++)
				{
					if (m_weatherPlanetData[i].PlanetId != closestPlanet.EntityId)
					{
						continue;
					}
					for (int num = m_weatherPlanetData[i].Weathers.Count - 1; num > -1; num--)
					{
						BoundingSphereD sphere = new BoundingSphereD(m_weatherPlanetData[i].Weathers[num].Position, m_weatherPlanetData[i].Weathers[num].Radius);
						if (boundingSphereD.Intersects(sphere))
						{
							m_weatherPlanetData[i].Weathers.RemoveAtFast(num);
						}
					}
				}
				MyObjectBuilder_WeatherEffect myObjectBuilder_WeatherEffect = new MyObjectBuilder_WeatherEffect
				{
					Position = weatherPosition.Value,
					Velocity = velocity,
					Weather = text,
					Radius = radius,
					MaxLife = (int)((float)length / 0.0166666675f),
					Intensity = intensity,
					StartPoint = weatherPosition.Value
				};
				for (int j = 0; j < m_weatherPlanetData.Count; j++)
				{
					if (m_weatherPlanetData[j].PlanetId == closestPlanet.EntityId)
					{
						m_weatherPlanetData[j].Weathers.Add(myObjectBuilder_WeatherEffect);
						break;
					}
				}
				if (Vector3.Distance(MySector.MainCamera.WorldMatrix.Translation, myObjectBuilder_WeatherEffect.Position) < myObjectBuilder_WeatherEffect.Radius && verbose)
				{
					MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), string.Format(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_SetWeather), text), Color.Red);
				}
				SyncWeathers();
				return true;
			}
			if (verbose)
			{
				MyHud.Chat.ShowMessage(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_Author), string.Format(MyTexts.GetString(MyCommonTexts.ChatCommand_Texts_NoWeatherFound), weatherEffect), Color.Red);
			}
			return false;
		}

		public bool RemoveWeather(Vector3D position)
		{
			if (m_weatherPlanetData != null)
			{
				foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetDatum in m_weatherPlanetData)
				{
					if (weatherPlanetDatum.Weathers == null)
					{
						continue;
					}
					for (int i = 0; i < weatherPlanetDatum.Weathers.Count; i++)
					{
						if (InsideWeather(position, weatherPlanetDatum.Weathers[i]))
						{
							weatherPlanetDatum.Weathers.RemoveAt(i);
							SyncWeathers();
							return true;
						}
					}
				}
			}
			return false;
		}

		public void RemoveWeather(MyObjectBuilder_WeatherEffect weatherEffect)
		{
			if (m_weatherPlanetData == null)
			{
				return;
			}
			foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetDatum in m_weatherPlanetData)
			{
				if (weatherPlanetDatum.Weathers == null)
				{
					continue;
				}
				for (int i = 0; i < weatherPlanetDatum.Weathers.Count; i++)
				{
					if (weatherPlanetDatum.Weathers[i] == weatherEffect)
					{
						weatherPlanetDatum.Weathers.RemoveAt(i);
						SyncWeathers();
						return;
					}
				}
			}
		}

		public bool InsideWeather(Vector3D position, MyObjectBuilder_WeatherEffect weather)
		{
			if (Vector3D.Distance(position, weather.Position) < (double)weather.Radius)
			{
				return true;
			}
			return false;
		}

		private float AdjustIntensity(float weatherIntensity)
		{
			if (m_closestPlanet == null)
			{
				return weatherIntensity;
			}
			if (m_surfaceAltitude < 0f)
			{
				float num = 2f * (MathHelper.Clamp(0f - m_surfaceAltitude, 0f, 100f) / 100f);
				weatherIntensity *= 1f - Math.Min(num * num, 1f);
			}
			else
			{
				float num = MathHelper.Clamp(m_surfaceAltitude * m_surfaceAltitude * 0.04f / m_closestPlanet.AtmosphereRadius - 0.25f, 0f, 1f);
				weatherIntensity *= 1f - num;
			}
			return weatherIntensity;
		}

		public float GetWeatherIntensity(Vector3D position)
		{
			foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetDatum in m_weatherPlanetData)
			{
				foreach (MyObjectBuilder_WeatherEffect weather in weatherPlanetDatum.Weathers)
				{
					if (InsideWeather(position, weather))
					{
						float num = Vector3.Distance(position, weather.Position) / weather.Radius;
						float weatherIntensity = (1f - num * num * num) * weather.Intensity;
						return AdjustIntensity(weatherIntensity);
					}
				}
			}
			return 0f;
		}

		public float GetWeatherIntensity(Vector3D position, MyObjectBuilder_WeatherEffect weatherEffect)
		{
			if (InsideWeather(position, weatherEffect))
			{
				float num = Vector3.Distance(position, weatherEffect.Position) / weatherEffect.Radius;
				float weatherIntensity = (1f - num * num * num) * weatherEffect.Intensity;
				return AdjustIntensity(weatherIntensity);
			}
			return 0f;
		}

		public float GetOxygenMultiplier(Vector3D position)
		{
			foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetDatum in m_weatherPlanetData)
			{
				foreach (MyObjectBuilder_WeatherEffect weather in weatherPlanetDatum.Weathers)
				{
					if (weather.Weather != null && Vector3.Distance(position, weather.Position) < weather.Radius)
					{
						float num = Vector3.Distance(position, weather.Position) / weather.Radius;
						MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather.Weather);
						if (weatherEffect != null)
						{
							return MathHelper.Lerp(1f, weatherEffect.OxygenLevelModifier, 1f - num * num * num);
						}
					}
				}
			}
			return 1f;
		}

		public float GetOxygenMultiplier(Vector3D position, MyObjectBuilder_WeatherEffect weatherEffect)
		{
			if (Vector3.Distance(position, weatherEffect.Position) < weatherEffect.Radius)
			{
				float num = Vector3.Distance(position, weatherEffect.Position) / weatherEffect.Radius;
				MyWeatherEffectDefinition weatherEffect2 = MyDefinitionManager.Static.GetWeatherEffect(weatherEffect.Weather);
				if (weatherEffect2 != null)
				{
					return MathHelper.Lerp(1f, weatherEffect2.OxygenLevelModifier, 1f - num * num * num);
				}
			}
			return 1f;
		}

		public float GetSolarMultiplier(Vector3D position)
		{
			foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetDatum in m_weatherPlanetData)
			{
				foreach (MyObjectBuilder_WeatherEffect weather in weatherPlanetDatum.Weathers)
				{
					if (Vector3.Distance(position, weather.Position) < weather.Radius)
					{
						float num = Vector3.Distance(position, weather.Position) / weather.Radius;
						MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather.Weather);
						if (weatherEffect != null)
						{
							return MathHelper.Lerp(1f, weatherEffect.SolarOutputModifier, 1f - num * num * num);
						}
					}
				}
			}
			return 1f;
		}

		public float GetSolarMultiplier(Vector3D position, MyObjectBuilder_WeatherEffect weather)
		{
			if (Vector3.Distance(position, weather.Position) < weather.Radius)
			{
				float num = Vector3.Distance(position, weather.Position) / weather.Radius;
				MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather.Weather);
				if (weatherEffect != null)
				{
					return MathHelper.Lerp(1f, weatherEffect.SolarOutputModifier, 1f - num * num * num);
				}
			}
			return 1f;
		}

		public float GetTemperatureMultiplier(Vector3D position)
		{
			foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetDatum in m_weatherPlanetData)
			{
				foreach (MyObjectBuilder_WeatherEffect weather in weatherPlanetDatum.Weathers)
				{
					if (Vector3.Distance(position, weather.Position) < weather.Radius)
					{
						float num = Vector3.Distance(position, weather.Position) / weather.Radius;
						MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather.Weather);
						if (weatherEffect != null)
						{
							return MathHelper.Lerp(1f, weatherEffect.TemperatureModifier, 1f - num * num * num);
						}
					}
				}
			}
			return 1f;
		}

		public float GetTemperatureMultiplier(Vector3D position, MyObjectBuilder_WeatherEffect weather)
		{
			if (Vector3.Distance(position, weather.Position) < weather.Radius)
			{
				float num = Vector3.Distance(position, weather.Position) / weather.Radius;
				MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather.Weather);
				if (weatherEffect != null)
				{
					return MathHelper.Lerp(1f, weatherEffect.TemperatureModifier, 1f - num * num * num);
				}
			}
			return 1f;
		}

		public float GetWindMultiplier(Vector3D position)
		{
			foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetDatum in m_weatherPlanetData)
			{
				foreach (MyObjectBuilder_WeatherEffect weather in weatherPlanetDatum.Weathers)
				{
					if (Vector3.Distance(position, weather.Position) < weather.Radius)
					{
						float num = Vector3.Distance(position, weather.Position) / weather.Radius;
						MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather.Weather);
						if (weatherEffect != null)
						{
							return MathHelper.Lerp(1f, weatherEffect.WindOutputModifier, 1f - num * num * num);
						}
					}
				}
			}
			return 1f;
		}

		public float GetWindMultiplier(Vector3D position, MyObjectBuilder_WeatherEffect weather)
		{
			if (Vector3.Distance(position, weather.Position) < weather.Radius)
			{
				float num = Vector3.Distance(position, weather.Position) / weather.Radius;
				MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(weather.Weather);
				if (weatherEffect != null)
				{
					return MathHelper.Lerp(1f, weatherEffect.WindOutputModifier, 1f - num * num * num);
				}
			}
			return 1f;
		}

		public override void UpdateAfterSimulation()
		{
			if (m_updateCounter == -1 && !Sync.IsServer)
			{
				RequestWeathersUpdate();
				m_updateCounter = 0;
			}
			if (m_updateCounter == 0)
			{
				UpdateAfterSimulationDelay();
			}
			m_updateCounter++;
			if (m_updateCounter > 60)
			{
				m_updateCounter = 0;
			}
			if (!MySandboxGame.IsPaused)
			{
				UpdatePlanetDataClient();
				UpdatePlanetDataServer();
			}
			for (int i = 0; i < m_lightnings.Count; i++)
			{
				if (!m_lightnings[i].Initialized)
				{
					m_lightnings[i].Init();
				}
			}
			if (!Sync.IsDedicated && Session != null)
			{
				if (m_closestPlanet != null && MySector.MainCamera != null)
				{
					m_originAltitude = (float)(MySector.MainCamera.Position - m_closestPlanet.PositionComp.GetPosition()).Length();
					m_surfaceAltitude = (float)((double)m_originAltitude - (m_closestPlanet.GetClosestSurfacePointGlobal(MySector.MainCamera.WorldMatrix.Translation) - m_closestPlanet.PositionComp.GetPosition()).Length());
				}
				else
				{
					m_originAltitude = 0f;
					m_surfaceAltitude = 0f;
				}
			}
			UpdateDefaultWeather();
			if (m_currentWeather != null && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				ApplyCurrentWeather();
				ApplySound();
				ApplyParticle();
			}
			UpdateTargetWeather();
		}

		private void UpdateAfterSimulationDelay()
		{
			if (MySector.MainCamera == null)
			{
				return;
			}
			bool flag = false;
			if (MySession.Static != null)
			{
				if (!Sync.IsServer)
				{
					foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetDatum in m_weatherPlanetData)
					{
						weatherPlanetDatum.NextWeather -= 60;
					}
				}
				foreach (MyVoxelBase instance in MySession.Static.VoxelMaps.Instances)
				{
					MyPlanet myPlanet = instance as MyPlanet;
					if (myPlanet == null)
					{
						continue;
					}
					bool flag2 = false;
					foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetDatum2 in m_weatherPlanetData)
					{
						if (weatherPlanetDatum2.PlanetId == myPlanet.EntityId)
						{
							flag2 = true;
							break;
						}
					}
					if (!flag2)
					{
						MyObjectBuilder_WeatherPlanetData item = new MyObjectBuilder_WeatherPlanetData
						{
							PlanetId = myPlanet.EntityId,
							NextWeather = (int)(MyUtils.GetRandomFloat(myPlanet.Generator.WeatherFrequencyMin, myPlanet.Generator.WeatherFrequencyMax) / 0.0166666675f)
						};
						m_weatherPlanetData.Add(item);
						flag = true;
					}
				}
				for (int num = m_weatherPlanetData.Count - 1; num > -1; num--)
				{
					bool flag3 = false;
					foreach (MyVoxelBase instance2 in MySession.Static.VoxelMaps.Instances)
					{
						MyPlanet myPlanet2 = instance2 as MyPlanet;
						if (myPlanet2 != null && m_weatherPlanetData[num].PlanetId == myPlanet2.EntityId)
						{
							flag3 = true;
							break;
						}
					}
					if (!flag3)
					{
						m_weatherPlanetData.RemoveAtFast(num);
						flag = true;
					}
				}
			}
			if (flag && Sync.IsServer)
			{
				SyncWeathers();
			}
			BoundingSphereD sphere = new BoundingSphereD(MySector.MainCamera.WorldMatrix.Translation, 100.0);
			m_nearbyEntities.Clear();
			MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref sphere, m_nearbyEntities);
			UpdateLocalData();
		}

		private void UpdateLocalData()
		{
			if (Sync.IsDedicated || MySession.Static == null)
			{
				return;
			}
			if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.ReverbDetectorComp != null)
			{
				m_insideVoxel = MySession.Static.LocalCharacter.ReverbDetectorComp.Voxels > 20;
				bool flag = MySession.Static.LocalCharacter.ReverbDetectorComp.Grids > 20;
				bool flag2 = false;
				if (Session.Player != null && Session.Player.Controller != null && Session.Player.Controller.ControlledEntity != null)
				{
					flag2 = Session.Player.Controller.ControlledEntity.Entity is IMyCockpit;
				}
				if (m_insideGrid != flag || m_inCockpit != flag2)
				{
					m_insideGrid = flag;
					m_inCockpit = flag2;
					foreach (MyParticleEffect particleEffect in m_particleEffects)
					{
						if (particleEffect != null)
						{
							UpdateCameraSoftRadiusMultiplier(particleEffect);
						}
					}
				}
			}
			if (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity != null && MySession.Static.ControlledEntity.Entity is MyCockpit)
			{
				m_insideClosedCockpit = (MySession.Static.ControlledEntity.Entity as MyCockpit).BlockDefinition.IsPressurized;
			}
			else
			{
				m_insideClosedCockpit = false;
			}
			m_closestPlanet = MyGamePruningStructure.GetClosestPlanet(MySector.MainCamera.Position);
			if (m_closestPlanet != null)
			{
				m_gravityVector = Vector3D.Normalize(m_closestPlanet.PositionComp.GetPosition() - MySector.MainCamera.Position);
			}
			else
			{
				m_gravityVector = Vector3.Zero;
			}
		}

		private void UpdatePlanetDataClient()
		{
			for (int num = m_lightnings.Count - 1; num > -1; num--)
			{
				m_lightnings[num].ObjectBuilder.Life++;
				if (m_lightnings[num].ObjectBuilder.Life > m_lightnings[num].ObjectBuilder.MaxLife)
				{
					m_lightnings.RemoveAtFast(num);
				}
			}
			for (int i = 0; i < m_weatherPlanetData.Count; i++)
			{
				MyObjectBuilder_WeatherPlanetData myObjectBuilder_WeatherPlanetData = m_weatherPlanetData[i];
				if (myObjectBuilder_WeatherPlanetData == null)
				{
					continue;
				}
				for (int j = 0; j < myObjectBuilder_WeatherPlanetData.Weathers.Count; j++)
				{
					MyObjectBuilder_WeatherEffect myObjectBuilder_WeatherEffect = myObjectBuilder_WeatherPlanetData.Weathers[j];
					if (myObjectBuilder_WeatherEffect.Velocity != Vector3D.Zero)
					{
						myObjectBuilder_WeatherEffect.Position += myObjectBuilder_WeatherEffect.Velocity * 0.01666666753590107;
					}
					if (myObjectBuilder_WeatherEffect.MaxLife > 0)
					{
						myObjectBuilder_WeatherEffect.Life++;
						if (myObjectBuilder_WeatherEffect.Life <= 1000)
						{
							myObjectBuilder_WeatherEffect.Intensity = Math.Min((float)myObjectBuilder_WeatherEffect.Life / 1000f, 1f);
						}
						else if (myObjectBuilder_WeatherEffect.Life >= myObjectBuilder_WeatherEffect.MaxLife - 1000)
						{
							myObjectBuilder_WeatherEffect.Intensity = (float)(myObjectBuilder_WeatherEffect.MaxLife - myObjectBuilder_WeatherEffect.Life) / 1000f;
						}
					}
				}
			}
		}

		private void UpdatePlanetDataServer()
		{
			if (!Sync.IsServer || MySession.Static == null || !MySession.Static.Settings.WeatherSystem)
			{
				return;
			}
			for (int i = 0; i < m_weatherPlanetData.Count; i++)
			{
				MyObjectBuilder_WeatherPlanetData myObjectBuilder_WeatherPlanetData = m_weatherPlanetData[i];
				if (myObjectBuilder_WeatherPlanetData == null)
				{
					continue;
				}
				for (int j = 0; j < myObjectBuilder_WeatherPlanetData.Weathers.Count; j++)
				{
					MyObjectBuilder_WeatherEffect myObjectBuilder_WeatherEffect = myObjectBuilder_WeatherPlanetData.Weathers[j];
					MyWeatherEffectDefinition weatherEffect = MyDefinitionManager.Static.GetWeatherEffect(myObjectBuilder_WeatherEffect.Weather);
					if (weatherEffect != null && weatherEffect.Lightning != null)
					{
						myObjectBuilder_WeatherEffect.NextLightning--;
						if (myObjectBuilder_WeatherEffect.NextLightning == 0)
						{
							CreateRandomLightning(myObjectBuilder_WeatherEffect, weatherEffect.Lightning, doHitGrid: false, doHitPlayer: false);
						}
						if (myObjectBuilder_WeatherEffect.NextLightning <= 0)
						{
							float randomFloat = MyUtils.GetRandomFloat(weatherEffect.LightningIntervalMin, weatherEffect.LightningIntervalMax);
							myObjectBuilder_WeatherEffect.NextLightning = (int)(randomFloat / 0.0166666675f);
						}
						myObjectBuilder_WeatherEffect.NextLightningCharacter--;
						if (myObjectBuilder_WeatherEffect.NextLightningCharacter == 0)
						{
							CreateRandomLightning(myObjectBuilder_WeatherEffect, weatherEffect.Lightning, doHitGrid: false, doHitPlayer: true);
						}
						if (myObjectBuilder_WeatherEffect.NextLightningCharacter <= 0)
						{
							float randomFloat2 = MyUtils.GetRandomFloat(weatherEffect.LightningCharacterHitIntervalMin, weatherEffect.LightningCharacterHitIntervalMax);
							myObjectBuilder_WeatherEffect.NextLightningCharacter = (int)(randomFloat2 / 0.0166666675f);
						}
						myObjectBuilder_WeatherEffect.NextLightningGrid--;
						if (myObjectBuilder_WeatherEffect.NextLightningGrid == 0)
						{
							CreateRandomLightning(myObjectBuilder_WeatherEffect, weatherEffect.Lightning, doHitGrid: true, doHitPlayer: false);
						}
						if (myObjectBuilder_WeatherEffect.NextLightningGrid <= 0)
						{
							float randomFloat3 = MyUtils.GetRandomFloat(weatherEffect.LightningGridHitIntervalMin, weatherEffect.LightningGridHitIntervalMax);
							myObjectBuilder_WeatherEffect.NextLightningGrid = (int)(randomFloat3 / 0.0166666675f);
						}
					}
				}
			}
			for (int k = 0; k < m_weatherPlanetData.Count; k++)
			{
				MyObjectBuilder_WeatherPlanetData myObjectBuilder_WeatherPlanetData2 = m_weatherPlanetData[k];
				if (myObjectBuilder_WeatherPlanetData2 == null)
				{
					continue;
				}
				MyPlanet myPlanet = (MyPlanet)MyEntities.GetEntityById(myObjectBuilder_WeatherPlanetData2.PlanetId);
				if (myPlanet != null && myPlanet.Generator != null && myPlanet.Generator.WeatherGenerators != null)
				{
					myObjectBuilder_WeatherPlanetData2.NextWeather--;
					if (myObjectBuilder_WeatherPlanetData2.NextWeather <= 0)
					{
						CreateRandomWeather(myPlanet);
						myObjectBuilder_WeatherPlanetData2.NextWeather = (int)(MyUtils.GetRandomFloat(myPlanet.Generator.WeatherFrequencyMin, myPlanet.Generator.WeatherFrequencyMax) / 0.0166666675f);
					}
				}
				for (int l = 0; l < myObjectBuilder_WeatherPlanetData2.Weathers.Count; l++)
				{
					MyObjectBuilder_WeatherEffect myObjectBuilder_WeatherEffect2 = myObjectBuilder_WeatherPlanetData2.Weathers[l];
					if (myObjectBuilder_WeatherEffect2.Life > myObjectBuilder_WeatherEffect2.MaxLife || myObjectBuilder_WeatherEffect2.Weather.Equals("Clear"))
					{
						RemoveWeather(myObjectBuilder_WeatherEffect2.Position);
					}
				}
			}
		}

		private void UpdateTargetWeather()
		{
			if (MySector.MainCamera == null)
			{
				return;
			}
			if (m_closestPlanet != null)
			{
				MyObjectBuilder_WeatherEffect weatherEffect = null;
				if (GetWeather(MySector.MainCamera.Position, out weatherEffect))
				{
					m_nightValue = NightValue(m_closestPlanet, MySector.MainCamera.Position);
					m_weatherIntensity = GetWeatherIntensity(MySector.MainCamera.Position);
					if (m_targetWeather == null || m_targetWeather.Id.SubtypeName != weatherEffect.Weather)
					{
						SetTargetWeather(MyDefinitionManager.Static.GetWeatherEffect(weatherEffect.Weather), instant: true);
					}
				}
				else if (m_targetWeather != null && m_targetWeather.Id.SubtypeName != "Default")
				{
					ResetWeather(instant: true);
				}
			}
			else if (m_targetWeather != null && m_targetWeather.Id.SubtypeName != "Default")
			{
				ResetWeather(instant: true);
			}
		}

		private void UpdateCameraSoftRadiusMultiplier(MyParticleEffect particleEffect)
		{
			if (!m_insideGrid)
			{
				if (m_inCockpit)
				{
					if (Session.Player != null && Session.Player.Controller != null && Session.Player.Controller.ControlledEntity != null && Session.Player.Controller.ControlledEntity.Entity is IMyCockpit)
					{
						particleEffect.CameraSoftRadiusMultiplier = ((IMyCockpit)Session.Player.Controller.ControlledEntity.Entity).LocalVolume.Radius;
					}
				}
				else
				{
					particleEffect.CameraSoftRadiusMultiplier = 1f;
				}
			}
			if (m_insideGrid || m_inCockpit)
			{
				particleEffect.SoftParticleDistanceScaleMultiplier = 1000f;
			}
			else
			{
				particleEffect.SoftParticleDistanceScaleMultiplier = 1f;
			}
		}

		public static float NightValue(MyPlanet planet, Vector3D position)
		{
			Vector3D value = position - planet.PositionComp.GetPosition();
			if ((float)value.Length() > planet.MaximumRadius * 1.1f)
			{
				return 0f;
			}
			Vector3 vector = Vector3.Normalize(value);
			if (Vector3.Dot(MySector.DirectionToSunNormalized, vector) < 0f)
			{
				return Math.Abs(Vector3.Dot(MySector.DirectionToSunNormalized, vector));
			}
			return 0f;
		}

		protected override void UnloadData()
		{
			if (m_ambientSound != null && m_ambientSound.IsPlaying)
			{
				m_ambientSound.Stop(force: true);
			}
			ResetWeather(instant: true);
			Static = null;
		}

		private void UpdateDefaultWeather()
		{
			if (MySector.EnvironmentDefinition != null)
			{
				m_defaultWeather.FogProperties = MySector.EnvironmentDefinition.FogProperties;
				m_defaultWeather.SunColor = MySector.EnvironmentDefinition.SunProperties.EnvironmentLight.SunColor;
				m_defaultWeather.SunSpecularColor = MySector.EnvironmentDefinition.SunProperties.EnvironmentLight.SunSpecularColor;
				m_defaultWeather.SunIntensity = MySector.EnvironmentDefinition.SunProperties.SunIntensity;
				m_defaultWeather.ShadowFadeout = MySector.EnvironmentDefinition.SunProperties.EnvironmentLight.ShadowFadeoutMultiplier;
			}
			else
			{
				m_defaultWeather.FogProperties = MyFogProperties.Default;
				m_defaultWeather.SunColor = MyEnvironmentLightData.Default.SunColor;
				m_defaultWeather.SunSpecularColor = MyEnvironmentLightData.Default.SunSpecularColor;
				m_defaultWeather.SunIntensity = MySunProperties.Default.SunIntensity;
				m_defaultWeather.ShadowFadeout = MyEnvironmentLightData.Default.ShadowFadeoutMultiplier;
			}
		}

		public override void Draw()
		{
			if (m_defaultWeather == null)
			{
				return;
			}
			if (MyDebugDrawSettings.ENABLE_DEBUG_DRAW && Sync.IsServer)
			{
				foreach (MyObjectBuilder_WeatherPlanetData weatherPlanetDatum in m_weatherPlanetData)
				{
					foreach (MyObjectBuilder_WeatherEffect weather in weatherPlanetDatum.Weathers)
					{
						MyRenderProxy.DebugDrawSphere(weather.Position, weather.Radius, Color.Red, 1f, depthRead: true, smooth: false, cull: false);
						MyRenderProxy.DebugDrawLine3D(weather.StartPoint, weather.EndPoint, Color.Green, Color.Yellow, depthRead: true);
					}
				}
			}
			if (MySandboxGame.IsPaused)
			{
				return;
			}
			for (int i = 0; i < m_lightnings.Count; i++)
			{
				Vector3D vector3D = m_lightnings[i].ObjectBuilder.Position;
				for (int j = 1; j <= m_lightnings[i].ObjectBuilder.BoltParts; j++)
				{
					float num = (float)j / (float)(int)m_lightnings[i].ObjectBuilder.BoltParts;
					Vector3D vector3D2 = vector3D - m_gravityVector * (m_lightnings[i].ObjectBuilder.BoltLength / (float)(int)m_lightnings[i].ObjectBuilder.BoltParts) + MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(0f, m_lightnings[i].ObjectBuilder.BoltVariation);
					MySimpleObjectDraw.DrawLine(vector3D, vector3D2, m_lightningMaterial, ref m_lightnings[i].ObjectBuilder.Color, num * m_lightnings[i].ObjectBuilder.BoltRadius);
					if (j > 2)
					{
						for (int k = 0; k < MyUtils.GetRandomInt(0, 2); k++)
						{
							MySimpleObjectDraw.DrawLine(vector3D, vector3D + m_gravityVector * (m_lightnings[i].ObjectBuilder.BoltLength / (float)(int)m_lightnings[i].ObjectBuilder.BoltParts / 2f) + MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(0f, m_lightnings[i].ObjectBuilder.BoltVariation / 2), m_lightningMaterial, ref m_lightnings[i].ObjectBuilder.Color, num * m_lightnings[i].ObjectBuilder.BoltRadius / 2f);
						}
					}
					vector3D = vector3D2;
				}
			}
		}

		private void ApplyCurrentWeather()
		{
			if (m_targetWeather == null)
			{
				m_targetWeather = m_defaultWeather;
			}
			m_sourceWeather.Lerp(m_defaultWeather, m_sourceWeather, m_currentTransition);
			m_sourceWeather.Lerp(m_targetWeather, m_currentWeather, m_currentTransition * m_weatherIntensity);
			m_currentTransition += m_transitionSpeed;
			m_currentTransition = MathHelper.Clamp(m_currentTransition, 0f, 1f);
			MySector.FogProperties.FogMultiplier = (FogMultiplierOverride.HasValue ? FogMultiplierOverride.Value : m_currentWeather.FogProperties.FogMultiplier);
			MySector.FogProperties.FogDensity = (FogDensityOverride.HasValue ? FogDensityOverride.Value : m_currentWeather.FogProperties.FogDensity);
			MySector.FogProperties.FogColor = (FogColorOverride.HasValue ? FogColorOverride.Value : (m_currentWeather.FogProperties.FogColor * (1f - m_nightValue * 0.5f)));
			MySector.FogProperties.FogSkybox = (FogSkyboxOverride.HasValue ? FogSkyboxOverride.Value : m_currentWeather.FogProperties.FogSkybox);
			MySector.FogProperties.FogAtmo = (FogAtmoOverride.HasValue ? FogAtmoOverride.Value : m_currentWeather.FogProperties.FogAtmo);
			MySector.SunProperties.EnvironmentLight.SunColor = m_currentWeather.SunColor;
			MySector.SunProperties.EnvironmentLight.SunSpecularColor = m_currentWeather.SunSpecularColor;
			MySector.SunProperties.SunIntensity = (SunIntensityOverride.HasValue ? SunIntensityOverride.Value : m_currentWeather.SunIntensity);
			float windStrength = MyRenderProxy.Settings.WindStrength;
			float num = MyRenderSettings.Default.WindStrength * m_currentWeather.FoliageWindModifier;
			if (Math.Abs(windStrength - num) > 0.01f)
			{
				MyRenderProxy.Settings.WindStrength = num;
				MyRenderProxy.SetSettingsDirty();
			}
			if (MySector.EnvironmentDefinition != null)
			{
				MySector.SunProperties.EnvironmentLight.ShadowFadeoutMultiplier = MySector.EnvironmentDefinition.SunProperties.EnvironmentLight.ShadowFadeoutMultiplier;
			}
		}

		private void SetTargetWeather(MyWeatherEffectDefinition targetWeather, bool instant = false)
		{
			if (m_currentWeather == null)
			{
				m_currentWeather = new MyWeatherEffectDefinition();
				m_defaultWeather.Lerp(m_defaultWeather, m_currentWeather, 1f);
			}
			if (m_sourceWeather == null)
			{
				m_sourceWeather = new MyWeatherEffectDefinition();
			}
			m_currentWeather.Lerp(m_currentWeather, m_sourceWeather, 1f);
			m_targetWeather = targetWeather;
			if (!instant)
			{
				m_currentTransition = 0f;
			}
			else
			{
				m_currentTransition = 1f;
			}
		}

		private void ApplySound()
		{
			string text = null;
			text = m_currentWeather.AmbientSound;
			if (string.IsNullOrEmpty(text))
			{
				if (m_ambientSound != null && m_ambientSound.IsPlaying)
				{
					m_ambientSound.Stop(force: true);
					m_currentSound = "";
				}
				return;
			}
			if (m_ambientSound != null && m_ambientSound.IsPlaying && !m_currentSound.Equals(text))
			{
				m_ambientSound.Stop(force: true);
			}
			if (m_ambientSound == null || !m_ambientSound.IsPlaying || !m_currentSound.Equals(text))
			{
				MyCueId cueId = new MyCueId(MyStringHash.GetOrCompute(text));
				m_ambientSound = MyAudio.Static.PlaySound(cueId);
				if (m_ambientSound == null)
				{
					_ = MyAudio.Static.CanPlay;
					return;
				}
				m_ambientSound.SetVolume(0f);
				m_currentSound = text;
			}
			if (m_ambientSound != null && m_ambientSound.IsPlaying)
			{
				if (m_insideClosedCockpit)
				{
					m_targetVolume = MathHelper.Clamp(m_currentWeather.AmbientVolume * m_currentTransition * m_weatherIntensity * 0.3f, 0f, m_currentWeather.AmbientVolume);
				}
				else if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.ReverbDetectorComp != null)
				{
					m_targetVolume = MathHelper.Clamp(m_currentWeather.AmbientVolume * m_currentTransition * m_weatherIntensity * Math.Min((25f - (float)(MySession.Static.LocalCharacter.ReverbDetectorComp.Grids - 10)) / 25f, 1f), 0f, m_currentWeather.AmbientVolume);
				}
				if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.OxygenComponent.HelmetEnabled && MySession.Static.Settings.RealisticSound)
				{
					m_targetVolume *= REALISTIC_SOUND_MULTIPLIER_WITH_HELMET;
				}
				m_ambientSound.SetVolume(MathHelper.Lerp(m_ambientSound.Volume, m_targetVolume, m_volumeTransitionSpeed));
			}
			m_currentSound = text;
		}

		private void ApplyParticle()
		{
			if (m_currentWeather == null || Session == null || Sync.IsDedicated || Session == null || MySector.MainCamera == null)
			{
				return;
			}
			int num = (int)Math.Round((float)m_currentWeather.ParticleCount * m_weatherIntensity);
			MatrixD worldMatrix = ((!ParticleDirectionOverride.HasValue) ? MatrixD.CreateFromDir(m_gravityVector) : ParticleDirectionOverride.Value);
			IMyCockpit myCockpit = null;
			Vector3D position = MySector.MainCamera.Position;
			if (m_particleEffects == null || m_particleEffects.Count != m_currentWeather.ParticleCount)
			{
				ResetParticles(instant: false);
				if (m_particleEffectIndex >= m_currentWeather.ParticleCount)
				{
					m_particleEffectIndex = 0;
				}
			}
			if (m_particleEffectIndex < num && m_particleEffectIndex < m_particleEffects.Count)
			{
				Vector3D zero = Vector3D.Zero;
				MyParticleEffect effect = m_particleEffects[m_particleEffectIndex];
				if (effect == null || effect.IsStopped || effect.GetName() != m_currentWeather.EffectName)
				{
					if (effect != null && !effect.IsStopped)
					{
						effect.Stop(instant: false);
					}
					MyParticlesManager.TryCreateParticleEffect(m_currentWeather.EffectName, out effect);
					m_particleEffects[m_particleEffectIndex] = effect;
					if (effect != null)
					{
						UpdateCameraSoftRadiusMultiplier(effect);
					}
				}
				if (effect != null)
				{
					Vector3 vector = MyUtils.GetRandomVector3Normalized() * MyUtils.GetRandomFloat(0f, m_currentWeather.ParticleRadius);
					zero = ((myCockpit != null) ? (myCockpit.CubeGrid.Physics.LinearVelocity + position + vector) : ((Session.Player != null && Session.Player.Character != null) ? (Session.Player.Character.Physics.LinearVelocity + position + vector) : (position + vector)));
					if (IsPositionSafe(ref zero))
					{
						worldMatrix.Translation = zero;
						effect.WorldMatrix = worldMatrix;
						effect.UserScale = m_currentWeather.ParticleScale;
						if (ParticleVelocityOverride.HasValue)
						{
							effect.Velocity = ParticleVelocityOverride.Value;
						}
						else
						{
							effect.Velocity = Vector3.Zero;
						}
					}
				}
			}
			else if (m_particleEffectIndex < m_particleEffects.Count && m_particleEffects[m_particleEffectIndex] != null)
			{
				m_particleEffects[m_particleEffectIndex].Stop(instant: false);
				m_particleEffects[m_particleEffectIndex] = null;
			}
			m_particleEffectIndex++;
			if (m_particleEffectIndex >= m_particleEffects.Count)
			{
				m_particleEffectIndex = 0;
			}
		}

		private bool IsPositionSafe(ref Vector3D position)
		{
			if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.ReverbDetectorComp != null)
			{
				_ = MySession.Static.LocalCharacter.ReverbDetectorComp.Voxels;
			}
			if (MySession.Static.LocalCharacter != null && MySession.Static.LocalCharacter.ReverbDetectorComp != null)
			{
				_ = MySession.Static.LocalCharacter.ReverbDetectorComp.Grids;
			}
			for (int i = 0; i < m_nearbyEntities.Count; i++)
			{
				if (m_nearbyEntities[i] == null)
				{
					continue;
				}
				if (m_nearbyEntities[i] is MyVoxelPhysics)
				{
					Vector3D from = position;
					Vector3D to = position + 30f * m_gravityVector;
					LineD line = new LineD(from, to);
					if (m_insideVoxel && (m_nearbyEntities[i] as MyVoxelPhysics).RootVoxel.GetIntersectionWithLine(ref line, out var t, IntersectionFlags.ALL_TRIANGLES))
					{
						return false;
					}
					from = position;
					to = position - 30f * m_gravityVector;
					line = new LineD(from, to);
					if ((m_nearbyEntities[i] as MyVoxelPhysics).RootVoxel.GetIntersectionWithLine(ref line, out t, IntersectionFlags.ALL_TRIANGLES))
					{
						return false;
					}
				}
				if (!(m_nearbyEntities[i] is MyCubeGrid) || ((MyCubeGrid)m_nearbyEntities[i]).GridSizeEnum != 0)
				{
					continue;
				}
				MatrixD matrixD = MatrixD.CreateFromDir(m_gravityVector);
				float num = 5f;
				m_particleSpread[0] = position;
				m_particleSpread[1] = position + num * matrixD.Left;
				m_particleSpread[2] = position + num * matrixD.Right;
				m_particleSpread[3] = position + num * matrixD.Up;
				m_particleSpread[4] = position + num * matrixD.Down;
				Vector3D[] particleSpread;
				if (m_insideGrid)
				{
					particleSpread = m_particleSpread;
					foreach (Vector3D vector3D in particleSpread)
					{
						Vector3D from2 = vector3D;
						Vector3D to2 = vector3D + 30f * m_gravityVector;
						LineD lineD = new LineD(from2, to2);
						if ((m_nearbyEntities[i] as MyCubeGrid).RayCastBlocks(lineD.From, lineD.To).HasValue)
						{
							Vector3D vector3D2 = vector3D - 28f * m_gravityVector;
							from2 = vector3D2;
							to2 = vector3D;
							if ((m_nearbyEntities[i] as MyCubeGrid).RayCastBlocks(from2, to2).HasValue)
							{
								return false;
							}
							position = vector3D2;
							break;
						}
					}
				}
				particleSpread = m_particleSpread;
				for (int j = 0; j < particleSpread.Length; j++)
				{
					Vector3D from3;
					Vector3D to3 = (from3 = particleSpread[j]) - 30f * m_gravityVector;
					LineD lineD2 = new LineD(from3, to3);
					if ((m_nearbyEntities[i] as MyCubeGrid).RayCastBlocks(lineD2.From, lineD2.To).HasValue)
					{
						return false;
					}
				}
			}
			return true;
		}

		private void ResetWeather(bool instant)
		{
			if (m_defaultWeather != null)
			{
				SetTargetWeather(m_defaultWeather, instant);
				ApplyCurrentWeather();
				ResetParticles(instant);
			}
		}

		private void ResetParticles(bool instant)
		{
			if (m_currentWeather.ParticleCount < 0)
			{
				return;
			}
			if (m_currentWeather.ParticleCount > m_particleEffects.Count)
			{
				m_particleEffects.AddRange(new MyParticleEffect[m_currentWeather.ParticleCount - m_particleEffects.Count]);
			}
			else
			{
				if (m_currentWeather.ParticleCount >= m_particleEffects.Count)
				{
					return;
				}
				for (int i = m_currentWeather.ParticleCount; i < m_particleEffects.Count; i++)
				{
					if (m_particleEffects[i] != null)
					{
						m_particleEffects[i].Stop(instant);
					}
				}
				m_particleEffects.RemoveRange(m_currentWeather.ParticleCount, m_particleEffects.Count - m_currentWeather.ParticleCount);
			}
		}

		public bool IsPositionAirtight(Vector3D position, out MyCubeGrid grid)
		{
			for (int i = 0; i < m_nearbyEntities.Count; i++)
			{
				MyCubeGrid myCubeGrid = null;
				if (m_nearbyEntities[i] != null)
				{
					myCubeGrid = m_nearbyEntities[i] as MyCubeGrid;
				}
				if (myCubeGrid != null && myCubeGrid.GridSizeEnum == MyCubeSize.Large)
				{
					Vector3I pos = myCubeGrid.WorldToGridInteger(position);
					if (myCubeGrid.IsRoomAtPositionAirtight(pos))
					{
						grid = myCubeGrid;
						return true;
					}
				}
			}
			grid = null;
			return false;
		}

		public bool IsPositionAirtight(Vector3D position)
		{
			for (int i = 0; i < m_nearbyEntities.Count; i++)
			{
				MyCubeGrid myCubeGrid = null;
				if (m_nearbyEntities[i] != null)
				{
					myCubeGrid = m_nearbyEntities[i] as MyCubeGrid;
				}
				if (myCubeGrid != null && myCubeGrid.IsRoomAtPositionAirtight(myCubeGrid.WorldToGridInteger(position)))
				{
					return true;
				}
			}
			return false;
		}

		private void SyncWeathers()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateWeathersOnClients, m_weatherPlanetData.ToArray());
		}

<<<<<<< HEAD
		[Event(null, 1877)]
=======
		[Event(null, 1872)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void UpdateWeathersOnClients(MyObjectBuilder_WeatherPlanetData[] planetData)
		{
			if (MySession.Static != null && MyMultiplayer.Static != null && Static != null)
			{
				if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
				{
					MyEventContext.ValidationFailed();
					(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
				}
				else
				{
					Static.m_weatherPlanetData.Clear();
					Static.m_weatherPlanetData.AddArray(planetData);
				}
			}
		}

		private void SyncLightning()
		{
<<<<<<< HEAD
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateLightningsOnClients, m_lightnings.Select((EffectLightning x) => x.ObjectBuilder).ToArray());
		}

		[Event(null, 1900)]
=======
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => UpdateLightningsOnClients, Enumerable.ToArray<MyObjectBuilder_WeatherLightning>(Enumerable.Select<EffectLightning, MyObjectBuilder_WeatherLightning>((IEnumerable<EffectLightning>)m_lightnings, (Func<EffectLightning, MyObjectBuilder_WeatherLightning>)((EffectLightning x) => x.ObjectBuilder))));
		}

		[Event(null, 1895)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		[Broadcast]
		private static void UpdateLightningsOnClients(MyObjectBuilder_WeatherLightning[] lightnings)
		{
			if (!MyEventContext.Current.IsLocallyInvoked && !MySession.Static.IsUserSpaceMaster(MyEventContext.Current.Sender.Value))
			{
				MyEventContext.ValidationFailed();
				(MyMultiplayer.Static as MyMultiplayerServerBase).ValidationFailed(MyEventContext.Current.Sender.Value);
			}
			else if (Static != null)
			{
				Static.m_lightnings.Clear();
				foreach (MyObjectBuilder_WeatherLightning objectBuilder in lightnings)
				{
					EffectLightning item = new EffectLightning
					{
						ObjectBuilder = objectBuilder
					};
					Static.m_lightnings.Add(item);
				}
			}
		}

		public void RequestWeathersUpdate()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner x) => RequestWeathersUpdate_Implementation);
		}

<<<<<<< HEAD
		[Event(null, 1931)]
=======
		[Event(null, 1926)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server]
		private static void RequestWeathersUpdate_Implementation()
		{
			MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => UpdateWeathersOnClient, Static.m_weatherPlanetData.ToArray(), MyEventContext.Current.Sender);
		}

<<<<<<< HEAD
		[Event(null, 1937)]
=======
		[Event(null, 1932)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private static void UpdateWeathersOnClient(MyObjectBuilder_WeatherPlanetData[] planetData)
		{
			if (Static != null)
			{
				Static.m_weatherPlanetData.Clear();
				Static.m_weatherPlanetData.AddArray(planetData);
			}
		}
	}
}
