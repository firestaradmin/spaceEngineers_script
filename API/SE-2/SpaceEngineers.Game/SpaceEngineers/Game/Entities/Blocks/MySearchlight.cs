using System;
using System.Runtime.CompilerServices;
using System.Text;
using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.Entities.UseObject;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Lights;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication;
using Sandbox.Game.Replication.ClientStates;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Game.Utils;
using VRage.Input;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Network;
using VRage.Sync;
using VRage.Utils;
using VRageMath;
using VRageRender.Import;
using VRageRender.Lights;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_Searchlight))]
	public class MySearchlight : MyFunctionalBlock, IMyUsableEntity, IMyParallelUpdateable, VRage.ModAPI.IMyEntity, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.Game.Entities.IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity, IMyTurretTerminalControlReceiver, IMyTargetingReceiver, IMyShootOrigin, IMyCameraController, IMyTurretControllerControllable, IMyLightingLogicSync, IMyTargetingCapableBlock
	{
		[Serializable]
		private struct SyncRotationAndElevation
		{
			protected class SpaceEngineers_Game_Entities_Blocks_MySearchlight_003C_003ESyncRotationAndElevation_003C_003ERotation_003C_003EAccessor : IMemberAccessor<SyncRotationAndElevation, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SyncRotationAndElevation owner, in float value)
				{
					owner.Rotation = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SyncRotationAndElevation owner, out float value)
				{
					value = owner.Rotation;
				}
			}

			protected class SpaceEngineers_Game_Entities_Blocks_MySearchlight_003C_003ESyncRotationAndElevation_003C_003EElevation_003C_003EAccessor : IMemberAccessor<SyncRotationAndElevation, float>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref SyncRotationAndElevation owner, in float value)
				{
					owner.Elevation = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref SyncRotationAndElevation owner, out float value)
				{
					value = owner.Elevation;
				}
			}

			public float Rotation;

			public float Elevation;
		}

		protected sealed class sync_ControlledEntity_Used_003C_003E : ICallSite<MySearchlight, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MySearchlight @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.sync_ControlledEntity_Used();
			}
		}

		protected class m_rotationAndElevationSync_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType rotationAndElevationSync;
				ISyncType result = (rotationAndElevationSync = new Sync<SyncRotationAndElevation, SyncDirection.BothWays>(P_1, P_2));
				((MySearchlight)P_0).m_rotationAndElevationSync = (Sync<SyncRotationAndElevation, SyncDirection.BothWays>)rotationAndElevationSync;
				return result;
			}
		}

		protected class m_lockedTarget_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType lockedTarget;
				ISyncType result = (lockedTarget = new Sync<long, SyncDirection.FromServer>(P_1, P_2));
				((MySearchlight)P_0).m_lockedTarget = (Sync<long, SyncDirection.FromServer>)lockedTarget;
				return result;
			}
		}

		protected class m_targetFlags_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType targetFlags;
				ISyncType result = (targetFlags = new Sync<MyTurretTargetFlags, SyncDirection.BothWays>(P_1, P_2));
				((MySearchlight)P_0).m_targetFlags = (Sync<MyTurretTargetFlags, SyncDirection.BothWays>)targetFlags;
				return result;
			}
		}

		protected class m_targetSync_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType targetSync;
				ISyncType result = (targetSync = new Sync<MyLargeTurretTargetingSystem.CurrentTargetSync, SyncDirection.FromServer>(P_1, P_2));
				((MySearchlight)P_0).m_targetSync = (Sync<MyLargeTurretTargetingSystem.CurrentTargetSync, SyncDirection.FromServer>)targetSync;
				return result;
			}
		}

		protected class m_targetingGroup_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType targetingGroup;
				ISyncType result = (targetingGroup = new Sync<MyStringHash, SyncDirection.BothWays>(P_1, P_2));
				((MySearchlight)P_0).m_targetingGroup = (Sync<MyStringHash, SyncDirection.BothWays>)targetingGroup;
				return result;
			}
		}

		protected class m_shootingRange_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType shootingRange;
				ISyncType result = (shootingRange = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MySearchlight)P_0).m_shootingRange = (Sync<float, SyncDirection.BothWays>)shootingRange;
				return result;
			}
		}

		protected class m_enableIdleRotation_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType enableIdleRotation;
				ISyncType result = (enableIdleRotation = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MySearchlight)P_0).m_enableIdleRotation = (Sync<bool, SyncDirection.BothWays>)enableIdleRotation;
				return result;
			}
		}

		protected class m_targetLocking_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType targetLocking;
				ISyncType result = (targetLocking = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MySearchlight)P_0).m_targetLocking = (Sync<bool, SyncDirection.BothWays>)targetLocking;
				return result;
			}
		}

		protected class _003CBlinkIntervalSecondsSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType _003CBlinkIntervalSecondsSync_003Ek__BackingField;
				ISyncType result = (_003CBlinkIntervalSecondsSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MySearchlight)P_0).BlinkIntervalSecondsSync = (Sync<float, SyncDirection.BothWays>)_003CBlinkIntervalSecondsSync_003Ek__BackingField;
				return result;
			}
		}

		protected class _003CBlinkLengthSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType _003CBlinkLengthSync_003Ek__BackingField;
				ISyncType result = (_003CBlinkLengthSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MySearchlight)P_0).BlinkLengthSync = (Sync<float, SyncDirection.BothWays>)_003CBlinkLengthSync_003Ek__BackingField;
				return result;
			}
		}

		protected class _003CBlinkOffsetSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType _003CBlinkOffsetSync_003Ek__BackingField;
				ISyncType result = (_003CBlinkOffsetSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MySearchlight)P_0).BlinkOffsetSync = (Sync<float, SyncDirection.BothWays>)_003CBlinkOffsetSync_003Ek__BackingField;
				return result;
			}
		}

		protected class _003CIntensitySync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType _003CIntensitySync_003Ek__BackingField;
				ISyncType result = (_003CIntensitySync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MySearchlight)P_0).IntensitySync = (Sync<float, SyncDirection.BothWays>)_003CIntensitySync_003Ek__BackingField;
				return result;
			}
		}

		protected class _003CLightColorSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType _003CLightColorSync_003Ek__BackingField;
				ISyncType result = (_003CLightColorSync_003Ek__BackingField = new Sync<Color, SyncDirection.BothWays>(P_1, P_2));
				((MySearchlight)P_0).LightColorSync = (Sync<Color, SyncDirection.BothWays>)_003CLightColorSync_003Ek__BackingField;
				return result;
			}
		}

		protected class _003CLightRadiusSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType _003CLightRadiusSync_003Ek__BackingField;
				ISyncType result = (_003CLightRadiusSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MySearchlight)P_0).LightRadiusSync = (Sync<float, SyncDirection.BothWays>)_003CLightRadiusSync_003Ek__BackingField;
				return result;
			}
		}

		protected class _003CLightFalloffSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType _003CLightFalloffSync_003Ek__BackingField;
				ISyncType result = (_003CLightFalloffSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MySearchlight)P_0).LightFalloffSync = (Sync<float, SyncDirection.BothWays>)_003CLightFalloffSync_003Ek__BackingField;
				return result;
			}
		}

		protected class _003CLightOffsetSync_003Ek__BackingField_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType _003CLightOffsetSync_003Ek__BackingField;
				ISyncType result = (_003CLightOffsetSync_003Ek__BackingField = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MySearchlight)P_0).LightOffsetSync = (Sync<float, SyncDirection.BothWays>)_003CLightOffsetSync_003Ek__BackingField;
				return result;
			}
		}

		protected class m_rotationSpeedForSubparts_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType rotationSpeedForSubparts;
				ISyncType result = (rotationSpeedForSubparts = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MySearchlight)P_0).m_rotationSpeedForSubparts = (Sync<float, SyncDirection.BothWays>)rotationSpeedForSubparts;
				return result;
			}
		}

		private MyTurretController m_turretController;

		private readonly Sync<SyncRotationAndElevation, SyncDirection.BothWays> m_rotationAndElevationSync;

		private float m_rotation;

		private float m_elevation;

		private bool m_isMoving = true;

		private bool m_transformDirty = true;

		protected MyEntity m_base1;

		protected MyEntity m_base2;

		private MyReplicationServer m_replicableServer;

		private IMyReplicable m_blocksReplicable;

		private MyLargeTurretTargetingSystem m_targetingSystem;

		private Sync<long, SyncDirection.FromServer> m_lockedTarget;

		private readonly Sync<MyTurretTargetFlags, SyncDirection.BothWays> m_targetFlags;

		private MyHudNotification m_outOfRangeNotification;

		private MyHudNotification m_noTargetNotification;

		private MyHudNotification m_lockingInProgressNotification;

		private long m_targetToSet;

		private bool m_previousIdleRotationState = true;

		private bool m_gunIdleElevationAzimuthUnknown = true;

		private bool m_workingFlagCombination = true;

		private bool m_playAimingSound;

		private bool m_isInRandomRotationDistance;

		private bool m_randomIsMoving;

		private double m_laserLength = 100.0;

		protected Vector3D m_hitPosition;

		private MyEntity3DSoundEmitter m_soundEmitterForRotation;

		protected MySoundPair m_rotatingCueEnum = new MySoundPair();

		private bool m_isAimed;

		private float m_gunIdleElevation;

		private float m_gunIdleAzimuth;

		private MatrixD m_lastTestedGridWM;

		private float m_minElevationRadians;

		private float m_maxElevationRadians = (float)Math.PI * 2f;

		private float m_minAzimuthRadians;

		private float m_maxAzimuthRadians = (float)Math.PI * 2f;

		private float m_minSinElevationRadians = -1f;

		private float m_maxSinElevationRadians = 1f;

		private float m_randomStandbyRotation;

		private float m_randomStandbyElevation;

		protected int m_randomStandbyChange_ms;

		protected int m_randomStandbyChangeConst_ms;

		protected float m_rotationSpeed;

		protected float m_elevationSpeed;

		private float m_fov;

		private float m_targetFov;

		private const float MIN_FOV = 1E-05f;

		private const float MAX_FOV = (float)Math.PI * 179f / 180f;

		private static float m_minFov;

		private static float m_maxFov;

		public static float GAMEPAD_ZOOM_SPEED = 0.02f;

		private static readonly float ROTATION_MULTIPLIER_NORMAL = 1f;

		private static readonly float ROTATION_MULTIPLIER_ZOOMED = 0.3f;

		private const float DEFAULT_MIN_RANGE = 4f;

		private const float DEFAULT_MAX_RANGE = 800f;

		private float m_maxRangeMeter = 800f;

		public const float MAX_DISTANCE_FOR_RANDOM_ROTATING_LARGESHIP_GUNS = 600f;

		private static uint TIMER_NORMAL_IN_FRAMES = 10u;

		private static uint TIMER_TIER1_IN_FRAMES = 0u;

		private const double MIN_AMING_DISTANCE_SQ = 10.0;

		private MyToolbar m_toolbar;

		private readonly Sync<MyLargeTurretTargetingSystem.CurrentTargetSync, SyncDirection.FromServer> m_targetSync;

		private readonly Sync<MyStringHash, SyncDirection.BothWays> m_targetingGroup;

		private readonly Sync<float, SyncDirection.BothWays> m_shootingRange;

		private float m_searchingRange = 800f;

		private float m_forcedTargetRange = 5000f;

		private readonly Sync<bool, SyncDirection.BothWays> m_enableIdleRotation;

		private MyDefinitionId m_defId;

		private const int MAX_LIGHT_UPDATE_DISTANCE = 5000;

		private const int NUM_DECIMALS = 1;

		private MyLightingLogic m_lightingLogic;

		private MyFlareDefinition m_flare;

		private static readonly Color COLOR_OFF = new Color(30, 30, 30);

		private bool m_wasWorking = true;

		private Matrix m_rotationMatrix = Matrix.Identity;

		private readonly Sync<bool, SyncDirection.BothWays> m_targetLocking;

		private MyParallelUpdateFlag m_parallelFlag;

		private readonly Sync<float, SyncDirection.BothWays> m_rotationSpeedForSubparts;

		private MatrixD LightSourceWorldMatrix
		{
			get
			{
				if (LightSourceDummy != null)
				{
					MatrixD worldMatrix = m_base2.WorldMatrix;
					Matrix m = Matrix.Normalize(LightSourceDummy.Matrix);
					return MatrixD.Multiply(m, worldMatrix);
				}
				return base.WorldMatrix;
			}
		}

		public bool IsAimed
		{
			get
			{
				return m_isAimed;
			}
			protected set
			{
				if (m_isAimed != value)
				{
					m_isAimed = value;
				}
			}
		}

		public bool TargetFriends
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.TargetFriends) != 0;
			}
			set
			{
				if (value)
				{
					TargetFlags |= MyTurretTargetFlags.TargetFriends;
				}
				else
				{
					TargetFlags &= ~MyTurretTargetFlags.TargetFriends;
				}
			}
		}

		public MyTurretTargetFlags TargetFlags
		{
			get
			{
				return m_targetFlags;
			}
			set
			{
				m_targetFlags.Value = value;
			}
		}

		private bool AiEnabled
		{
			get
			{
				if (BlockDefinition != null)
				{
					return BlockDefinition.AiEnabled;
				}
				return true;
			}
		}

		public Color Color
		{
			get
			{
				return m_lightingLogic.Color;
			}
			set
			{
				m_lightingLogic.Color = value;
			}
		}

		public Vector4 LightColorDef => (IsLargeLight ? new Color(255, 255, 222) : new Color(206, 235, 255)).ToVector4();

		protected bool SupportsFalloff => false;

		private float GlareQuerySizeDef => base.CubeGrid.GridScale * (IsLargeLight ? 0.5f : 0.1f);

		private float Rotation
		{
			get
			{
				return m_rotation;
			}
			set
			{
				if (m_rotation != value)
				{
					m_rotation = value;
					m_transformDirty = true;
				}
			}
		}

		private float Elevation
		{
			get
			{
				return m_elevation;
			}
			set
			{
				if (m_elevation != value)
				{
					m_elevation = value;
					m_transformDirty = true;
				}
			}
		}

		public MyEntity Target => m_targetingSystem.Target;

		public new MySearchlightDefinition BlockDefinition => base.BlockDefinition as MySearchlightDefinition;

		public MyModelDummy CameraDummy { get; private set; }

		public MyModelDummy LightSourceDummy { get; private set; }

		IMyControllerInfo VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ControllerInfo => ControllerInfo;

		public bool TargetMeteors
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.Asteroids) != 0;
			}
			set
			{
				if (value)
				{
					TargetFlags |= MyTurretTargetFlags.Asteroids;
				}
				else
				{
					TargetFlags &= ~MyTurretTargetFlags.Asteroids;
				}
			}
		}

		public bool TargetMissiles
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.Missiles) != 0;
			}
			set
			{
				if (value)
				{
					TargetFlags |= MyTurretTargetFlags.Missiles;
				}
				else
				{
					TargetFlags &= ~MyTurretTargetFlags.Missiles;
				}
			}
		}

		public bool TargetSmallGrids
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.SmallShips) != 0;
			}
			set
			{
				if (value)
				{
					TargetFlags |= MyTurretTargetFlags.SmallShips;
				}
				else
				{
					TargetFlags &= ~MyTurretTargetFlags.SmallShips;
				}
			}
		}

		public bool TargetLargeGrids
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.LargeShips) != 0;
			}
			set
			{
				if (value)
				{
					TargetFlags |= MyTurretTargetFlags.LargeShips;
				}
				else
				{
					TargetFlags &= ~MyTurretTargetFlags.LargeShips;
				}
			}
		}

		public bool TargetCharacters
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.Players) != 0;
			}
			set
			{
				if (value)
				{
					TargetFlags |= MyTurretTargetFlags.Players;
				}
				else
				{
					TargetFlags &= ~MyTurretTargetFlags.Players;
				}
			}
		}

		public bool TargetStations
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.Stations) != 0;
			}
			set
			{
				if (value)
				{
					TargetFlags |= MyTurretTargetFlags.Stations;
				}
				else
				{
					TargetFlags &= ~MyTurretTargetFlags.Stations;
				}
			}
		}

		public bool TargetNeutrals
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.NotNeutrals) == 0;
			}
			set
			{
				if (value)
				{
					TargetFlags &= ~MyTurretTargetFlags.NotNeutrals;
				}
				else
				{
					TargetFlags |= MyTurretTargetFlags.NotNeutrals;
				}
			}
		}

		public bool TargetEnemies
		{
			get
			{
				return (TargetFlags & MyTurretTargetFlags.NotEnemies) == 0;
			}
			set
			{
				if (value)
				{
					TargetFlags &= ~MyTurretTargetFlags.NotEnemies;
				}
				else
				{
					TargetFlags |= MyTurretTargetFlags.NotEnemies;
				}
			}
		}

		public bool IsTargetLocked => m_lockedTarget.Value != 0;

		public bool IsLargeLight { get; private set; }

		public float CurrentLightPower => m_lightingLogic.CurrentLightPower;

		public float Intensity => m_lightingLogic.Intensity;

		public float ReflectorRadius => m_lightingLogic.ReflectorRadius;

		public MyBounds ReflectorRadiusBounds => m_lightingLogic.ReflectorRadiusBounds;

		public bool IsReflectorEnabled
		{
			get
			{
				if (m_lightingLogic.Lights.Count > 0 && m_lightingLogic.Lights[0].ReflectorOn)
				{
					return !m_turretController.IsControlledByLocalPlayer;
				}
				return false;
			}
		}

		public string ReflectorConeMaterial => BlockDefinition.ReflectorConeMaterial;

		public long OwnerIdentityId => base.OwnerId;

		public Vector3D EntityPosition => base.PositionComp.GetPosition();

		public Vector3D ShootDirection => LightSourceWorldMatrix.Forward;

		public float SearchRange
		{
			get
			{
				if (m_targetingSystem.IsTargetForced)
				{
					return m_forcedTargetRange;
				}
				return m_searchingRange;
			}
		}

		public float ShootRangeSimple
		{
			get
			{
				return m_shootingRange;
			}
			set
			{
				m_shootingRange.Value = value;
			}
		}

		protected float ForwardCameraOffset => BlockDefinition.ForwardCameraOffset;

		protected float UpCameraOffset => BlockDefinition.UpCameraOffset;

		public Sync<MyLargeTurretTargetingSystem.CurrentTargetSync, SyncDirection.FromServer> TargetSync => m_targetSync;

		public MyGridTargeting GridTargeting => base.CubeGrid.Components.Get<MyGridTargeting>();

		bool IMyCameraController.AllowCubeBuilding => false;

		public MyEntity Entity => this;

		public float MechanicalDamage { get; }

		public float ShootRange
		{
			get
			{
				if (m_targetingSystem.IsTargetForced)
				{
					return m_forcedTargetRange;
				}
				return m_shootingRange;
			}
		}

		public float SearchRangeSquared
		{
			get
			{
				float searchRange = SearchRange;
				return searchRange * searchRange;
			}
		}

		public float ShootRangeSquared
		{
			get
			{
				float shootRange = ShootRange;
				return shootRange * shootRange;
			}
		}

		public MyTurretTargetingOptions HiddenTargetingOptions => (MyTurretTargetingOptions)0;

		public float HeadLocalXAngle { get; set; }

		public float HeadLocalYAngle { get; set; }

		public MyToolbarType ToolbarType => MyToolbarType.LargeCockpit;

		public MyStringId ControlContext => MySpaceBindingCreator.CX_SPACESHIP;

		public MyStringId AuxiliaryContext => MySpaceBindingCreator.AX_ACTIONS;

		public MyToolbar Toolbar => m_toolbar;

		public MyEntity RelativeDampeningEntity { get; set; }

		public MyControllerInfo ControllerInfo => m_turretController.ControllerInfo;

		VRage.ModAPI.IMyEntity VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Entity => Entity;

		bool IMyCameraController.IsInFirstPersonView
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public bool ForceFirstPersonCamera { get; set; }

		bool IMyCameraController.EnableFirstPersonView
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public Vector3 LastMotionIndicator => Vector3.Zero;

		public Vector3 LastRotationIndicator { get; set; }

		public bool EnabledThrusts => false;

		public bool EnabledDamping => false;

		public bool EnabledLights => false;

		public bool EnabledLeadingGears => false;

		public bool EnabledReactors => false;

		public bool EnabledBroadcasting => false;

		public bool EnabledHelmet => false;

		public bool PrimaryLookaround => false;

		private bool NeedsPerFrameUpdate
		{
			get
			{
				if (!MyFakes.OPTIMIZE_GRID_UPDATES)
				{
					return true;
				}
				float randomStandbyRotation = m_randomStandbyRotation;
				float randomStandbyElevation = m_randomStandbyElevation;
				float num = randomStandbyRotation - Rotation;
				float num2 = randomStandbyElevation - Elevation;
				if ((!m_turretController.IsControlledByLocalPlayer || MySession.Static.CameraController != this) && !m_transformDirty && !m_isMoving && !(num * num > 9.99999944E-11f))
				{
					return num2 * num2 > 9.99999944E-11f;
				}
				return true;
			}
		}

		protected virtual bool NeedPerFrameUpdate => m_lightingLogic.NeedPerFrameUpdate | (m_lightingLogic.HasSubPartLights && (float)m_rotationSpeedForSubparts > 0f && m_lightingLogic.CurrentLightPower > 0f);

		public MyParallelUpdateFlags UpdateFlags => m_parallelFlag.GetFlags(this);

		public Vector3D ShootOrigin => LightSourceWorldMatrix.Translation;

		public MyDefinitionBase GetAmmoDefinition { get; }

		public MyStringHash TargetingGroup { get; }

		public bool EnableIdleRotation
		{
			get
			{
				return m_enableIdleRotation;
			}
			set
			{
				m_enableIdleRotation.Value = value;
			}
		}

		public float MinRange => 4f;

		public float MaxRange => m_maxRangeMeter;

		public bool TargetLocking
		{
			get
			{
				return m_targetLocking;
			}
			set
			{
				m_targetLocking.Value = value;
			}
		}

		public MyTurretController TurretController => m_turretController;

		public Sync<float, SyncDirection.BothWays> BlinkIntervalSecondsSync { get; }

		public Sync<float, SyncDirection.BothWays> BlinkLengthSync { get; }

		public Sync<float, SyncDirection.BothWays> BlinkOffsetSync { get; }

		public Sync<float, SyncDirection.BothWays> IntensitySync { get; }

		public Sync<Color, SyncDirection.BothWays> LightColorSync { get; }

		public Sync<float, SyncDirection.BothWays> LightRadiusSync { get; }

		public Sync<float, SyncDirection.BothWays> LightFalloffSync { get; }

		public Sync<float, SyncDirection.BothWays> LightOffsetSync { get; }

		public float MaxShootRange => 800f;

		public Vector3 GetShootDirection()
		{
			return base.WorldMatrix.Forward;
		}

		private new MatrixD GetViewMatrix()
		{
			RotateModels();
			MatrixD matrix = m_base2.WorldMatrix;
			if (CameraDummy != null)
			{
				Matrix m = Matrix.Normalize(CameraDummy.Matrix);
				matrix = MatrixD.Multiply(m, matrix);
			}
			else
			{
				matrix.Translation += matrix.Forward * ForwardCameraOffset;
				matrix.Translation += matrix.Up * UpCameraOffset;
			}
			MatrixD.Invert(ref matrix, out var result);
			return result;
		}

		public void ControlCamera(MyCamera currentCamera)
		{
			currentCamera.SetViewMatrix(GetViewMatrix());
		}

		public void Rotate(Vector2 rotationIndicator, float rollIndicator)
		{
		}

		public void RotateStopped()
		{
		}

		public void OnAssumeControl(IMyCameraController previousCameraController)
		{
			SetCameraOverlay();
		}

		public void OnReleaseControl(IMyCameraController newCameraController)
		{
			RemoveCameraOverlay();
		}

		public bool HandleUse()
		{
			if (MySession.Static.LocalCharacter != null)
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, MySession.Static.LocalCharacter);
				m_targetFov = m_maxFov;
				SetFov(m_maxFov);
			}
			return false;
		}

		public bool HandlePickUp()
		{
			return false;
		}

		public void AddPropertiesChangedCallback(Action<MyTerminalBlock> callback)
		{
			base.PropertiesChanged += callback;
		}

		public void RemovePropertiesChangedCallback(Action<MyTerminalBlock> callback)
		{
			base.PropertiesChanged -= callback;
		}

		public MyStringHash GetTargetingGroupHash()
		{
			return m_targetingGroup.Value;
		}

		public MyEntity GetTargetingParent()
		{
			return GetTopMostParent();
		}

		public bool IsConnected(MyCubeGrid grid)
		{
			return grid.GridSystems.TerminalSystem == base.CubeGrid.GridSystems.TerminalSystem;
		}

		public Vector3 LookAt(Vector3D target)
		{
			Vector3D translation = LightSourceWorldMatrix.Translation;
			Vector3.GetAzimuthAndElevation(Vector3.Normalize(Vector3D.TransformNormal(target - translation, base.PositionComp.WorldMatrixInvScaled)), out var azimuth, out var elevation);
			if (m_gunIdleElevationAzimuthUnknown)
			{
				Vector3.GetAzimuthAndElevation((LightSourceDummy != null) ? LightSourceDummy.Matrix.Forward : ((Vector3)MatrixD.Identity.Forward), out m_gunIdleAzimuth, out m_gunIdleElevation);
				m_gunIdleElevationAzimuthUnknown = false;
			}
			return new Vector3(elevation - m_gunIdleElevation, MathHelper.WrapAngle(azimuth - m_gunIdleAzimuth), 0f);
		}

		public bool IsTargetInView(Vector3D predPos)
		{
			Vector3 lookAtPositionEuler = LookAt(predPos);
			return IsInRange(ref lookAtPositionEuler);
		}

		public bool IsInRange(ref Vector3 lookAtPositionEuler)
		{
			float y = lookAtPositionEuler.Y;
			float x = lookAtPositionEuler.X;
			if (y > m_minAzimuthRadians && y < m_maxAzimuthRadians && x > m_minElevationRadians)
			{
				return x < m_maxElevationRadians;
			}
			return false;
		}

		public MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long targetIidentityId)
		{
			return GetUserRelationToOwner(targetIidentityId, MyRelationsBetweenPlayerAndBlock.NoOwnership);
		}

		public void BeginShoot(MyShootActionEnum action)
		{
		}

		public void EndShoot(MyShootActionEnum action)
		{
		}

		public bool ShouldEndShootingOnPause(MyShootActionEnum action)
		{
			return true;
		}

		public void OnBeginShoot(MyShootActionEnum action)
		{
		}

		public void OnEndShoot(MyShootActionEnum action)
		{
		}

		public void UseFinished()
		{
		}

		public void PickUpFinished()
		{
		}

		public void Sprint(bool enabled)
		{
		}

		public void SwitchToWeapon(MyDefinitionId weaponDefinition)
		{
		}

		public void SwitchToWeapon(MyToolbarItemWeapon weapon)
		{
		}

		public bool CanSwitchToWeapon(MyDefinitionId? weaponDefinition)
		{
			return false;
		}

		public void SwitchAmmoMagazine()
		{
		}

		public bool CanSwitchAmmoMagazine()
		{
			return false;
		}

		public void SwitchBroadcasting()
		{
		}

		public MyEntityCameraSettings GetCameraEntitySettings()
		{
			return null;
		}

		public MatrixD GetHeadMatrix(bool includeY, bool includeX = true, bool forceHeadAnim = false, bool forceHeadBone = false)
		{
			return GetViewMatrix();
		}

		public void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			MoveAndRotate(moveIndicator, rotationIndicator, rollIndicator, overrideControlCheck: false, MyInput.Static.IsAnyAltKeyPressed());
		}

		public void MoveAndRotateStopped()
		{
			RotateModels();
		}

		public void Use()
		{
			MyGuiAudio.PlaySound(MyGuiSounds.HudUse);
			MyMultiplayer.RaiseEvent(this, (MySearchlight x) => x.sync_ControlledEntity_Used);
		}

		public void UseContinues()
		{
		}

		public void PickUp()
		{
		}

		public void PickUpContinues()
		{
		}

		public void Jump(Vector3 moveindicator = default(Vector3))
		{
		}

		public void SwitchWalk()
		{
		}

		public void Up()
		{
		}

		public void Crouch()
		{
		}

		public void Down()
		{
		}

		public override bool GetTimerEnabledState()
		{
			return Enabled;
		}

		public void ShowInventory()
		{
			MyCharacter user = m_turretController.GetUser();
			if (user != null)
			{
				MyGuiScreenTerminal.Show(MyTerminalPageEnum.Inventory, user, this);
			}
		}

		public void ShowTerminal()
		{
			MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, MySession.Static.LocalCharacter, this);
		}

		public void SwitchThrusts()
		{
		}

		public void SwitchDamping()
		{
			MyShipController myShipController = m_turretController.PreviousControlledEntity as MyShipController;
			if (myShipController != null && base.CubeGrid.ControlledFromTurret)
			{
				myShipController.SwitchDamping();
			}
		}

		public void SwitchLights()
		{
			MyShipController myShipController = m_turretController.PreviousControlledEntity as MyShipController;
			if (myShipController != null && base.CubeGrid.ControlledFromTurret)
			{
				myShipController.SwitchLights();
			}
			Enabled = !Enabled;
		}

		public void SwitchLandingGears()
		{
			MyShipController myShipController = m_turretController.PreviousControlledEntity as MyShipController;
			if (myShipController != null && base.CubeGrid.ControlledFromTurret)
			{
				myShipController.SwitchLandingGears();
			}
		}

		public void SwitchHandbrake()
		{
			MyShipController myShipController = m_turretController.PreviousControlledEntity as MyShipController;
			if (myShipController != null && base.CubeGrid.ControlledFromTurret)
			{
				myShipController.SwitchHandbrake();
			}
		}

		public void SwitchReactors()
		{
			MyShipController myShipController = m_turretController.PreviousControlledEntity as MyShipController;
			if (myShipController != null && base.CubeGrid.ControlledFromTurret)
			{
				myShipController.SwitchReactors();
			}
		}

		public void SwitchReactorsLocal()
		{
			MyShipController myShipController;
			if ((myShipController = m_turretController.PreviousControlledEntity as MyShipController) != null && base.CubeGrid.ControlledFromTurret)
			{
				myShipController.SwitchReactorsLocal();
			}
		}

		public void SwitchHelmet()
		{
		}

		public void DrawHud(IMyCameraController camera, long playerId)
		{
		}

		public void Die()
		{
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			MyObjectBuilder_Searchlight myObjectBuilder_Searchlight = (MyObjectBuilder_Searchlight)objectBuilder;
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.RequiredPowerInput, () => (!Enabled || !base.IsFunctional) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), this);
			myResourceSinkComponent.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink = myResourceSinkComponent;
			m_lightingLogic = new MyLightingLogic(this, BlockDefinition, this);
			m_lightingLogic.OnPropertiesChanged += base.RaisePropertiesChanged;
			m_lightingLogic.OnUpdateEnabled += UpdateEnabled;
			m_lightingLogic.OnIntensityUpdated += UpdateIntensity;
			m_lightingLogic.OnInitLight += InitLight;
			m_lightingLogic.OnEmissivityUpdated += UpdateEmissivity;
			m_lightingLogic.OnRadiusUpdated += UpdateRadius;
			CreateTerminalControls();
			base.Init(objectBuilder, cubeGrid);
			m_randomStandbyChangeConst_ms = MyUtils.GetRandomInt(3500, 4500);
			m_rotatingCueEnum.Init("WepTurretGatlingRotate");
			IsLargeLight = cubeGrid.GridSizeEnum == MyCubeSize.Large;
			m_lightingLogic.IsReflector = true;
			m_rotationSpeedForSubparts.ValidateRange(BlockDefinition.RotationSpeedBounds);
			m_rotationSpeedForSubparts.SetLocalValue(BlockDefinition.RotationSpeedBounds.Clamp((myObjectBuilder_Searchlight.RotationSpeed == -1f) ? BlockDefinition.RotationSpeedBounds.Default : myObjectBuilder_Searchlight.RotationSpeed));
			m_lightingLogic.Color = ((myObjectBuilder_Searchlight.ColorAlpha == -1f) ? LightColorDef : new Vector4(myObjectBuilder_Searchlight.ColorRed, myObjectBuilder_Searchlight.ColorGreen, myObjectBuilder_Searchlight.ColorBlue, myObjectBuilder_Searchlight.ColorAlpha));
			LightRadiusSync.ValidateRange(m_lightingLogic.IsReflector ? BlockDefinition.LightReflectorRadius : BlockDefinition.LightRadius);
			LightFalloffSync.ValidateRange(BlockDefinition.LightFalloff);
			m_lightingLogic.Radius = m_lightingLogic.RadiusBounds.Clamp((myObjectBuilder_Searchlight.Radius == -1f) ? m_lightingLogic.RadiusBounds.Default : myObjectBuilder_Searchlight.Radius);
			m_lightingLogic.ReflectorRadius = m_lightingLogic.ReflectorRadiusBounds.Clamp((myObjectBuilder_Searchlight.ReflectorRadius == -1f) ? m_lightingLogic.ReflectorRadiusBounds.Default : myObjectBuilder_Searchlight.ReflectorRadius);
			m_lightingLogic.Falloff = m_lightingLogic.FalloffBounds.Clamp((myObjectBuilder_Searchlight.Falloff == -1f) ? m_lightingLogic.FalloffBounds.Default : myObjectBuilder_Searchlight.Falloff);
			BlinkIntervalSecondsSync.ValidateRange(BlockDefinition.BlinkIntervalSeconds);
			BlinkIntervalSecondsSync.SetLocalValue(m_lightingLogic.BlinkIntervalSecondsBounds.Clamp((myObjectBuilder_Searchlight.BlinkIntervalSeconds == -1f) ? m_lightingLogic.BlinkIntervalSecondsBounds.Default : myObjectBuilder_Searchlight.BlinkIntervalSeconds));
			BlinkLengthSync.ValidateRange(BlockDefinition.BlinkLenght);
			BlinkLengthSync.SetLocalValue(m_lightingLogic.BlinkLengthBounds.Clamp((myObjectBuilder_Searchlight.BlinkLenght == -1f) ? m_lightingLogic.BlinkLengthBounds.Default : myObjectBuilder_Searchlight.BlinkLenght));
			BlinkOffsetSync.ValidateRange(BlockDefinition.BlinkOffset);
			BlinkOffsetSync.SetLocalValue(m_lightingLogic.BlinkOffsetBounds.Clamp((myObjectBuilder_Searchlight.BlinkOffset == -1f) ? m_lightingLogic.BlinkOffsetBounds.Default : myObjectBuilder_Searchlight.BlinkOffset));
			IntensitySync.ValidateRange(BlockDefinition.LightIntensity);
			IntensitySync.SetLocalValue(m_lightingLogic.IntensityBounds.Clamp((myObjectBuilder_Searchlight.Intensity == -1f) ? m_lightingLogic.IntensityBounds.Default : myObjectBuilder_Searchlight.Intensity));
			LightOffsetSync.ValidateRange(BlockDefinition.LightOffset);
			LightOffsetSync.SetLocalValue(m_lightingLogic.OffsetBounds.Clamp((myObjectBuilder_Searchlight.Offset == -1f) ? m_lightingLogic.OffsetBounds.Default : myObjectBuilder_Searchlight.Offset));
			m_lightingLogic.Initialize();
			m_defId = myObjectBuilder_Searchlight.GetId();
			m_turretController.SavedPreviousControlledEntityId = myObjectBuilder_Searchlight.PreviousControlledEntityId;
			Rotation = myObjectBuilder_Searchlight.Rotation;
			Elevation = myObjectBuilder_Searchlight.Elevation;
			m_shootingRange.ValidateRange(0f, BlockDefinition.MaxRangeMeters);
			m_shootingRange.SetLocalValue(Math.Min(BlockDefinition.MaxRangeMeters, Math.Max(0f, myObjectBuilder_Searchlight.Range)));
			AdjustSearchingRange();
			TargetMeteors = myObjectBuilder_Searchlight.Flags.TargetMeteors;
			TargetMissiles = myObjectBuilder_Searchlight.Flags.TargetMissiles;
			TargetCharacters = myObjectBuilder_Searchlight.Flags.TargetCharacters;
			TargetSmallGrids = myObjectBuilder_Searchlight.Flags.TargetSmallGrids;
			TargetLargeGrids = myObjectBuilder_Searchlight.Flags.TargetLargeGrids;
			TargetStations = myObjectBuilder_Searchlight.Flags.TargetStations;
			TargetNeutrals = myObjectBuilder_Searchlight.Flags.TargetNeutrals;
			TargetFriends = myObjectBuilder_Searchlight.Flags.TargetFriends;
			TargetEnemies = myObjectBuilder_Searchlight.Flags.TargetEnemies;
			OnTargetFlagChanged();
			TargetLocking = myObjectBuilder_Searchlight.TargetLocking;
			m_targetingGroup.Value = myObjectBuilder_Searchlight.TargetingGroup;
			m_targetToSet = myObjectBuilder_Searchlight.Target;
			m_targetingSystem.IsPotentialTarget = myObjectBuilder_Searchlight.IsPotentialTarget;
			m_maxRangeMeter = BlockDefinition.MaxRangeMeters;
			m_minElevationRadians = MathHelper.ToRadians(NormalizeAngle(BlockDefinition.MinElevationDegrees));
			m_maxElevationRadians = MathHelper.ToRadians(NormalizeAngle(BlockDefinition.MaxElevationDegrees));
			if (m_minElevationRadians > m_maxElevationRadians)
			{
				m_minElevationRadians -= (float)Math.PI * 2f;
			}
			m_minSinElevationRadians = (float)Math.Sin(m_minElevationRadians);
			m_maxSinElevationRadians = (float)Math.Sin(m_maxElevationRadians);
			m_minAzimuthRadians = MathHelper.ToRadians(NormalizeAngle(BlockDefinition.MinAzimuthDegrees));
			m_maxAzimuthRadians = MathHelper.ToRadians(NormalizeAngle(BlockDefinition.MaxAzimuthDegrees));
			if (m_minAzimuthRadians > m_maxAzimuthRadians)
			{
				m_minAzimuthRadians -= (float)Math.PI * 2f;
			}
			m_rotationSpeed = BlockDefinition.RotationSpeed;
			m_elevationSpeed = BlockDefinition.ElevationSpeed;
			m_enableIdleRotation.Value = BlockDefinition.IdleRotation;
			ClampRotationAndElevation();
			m_minFov = BlockDefinition.MinFov;
			m_maxFov = BlockDefinition.MaxFov;
			m_fov = BlockDefinition.MaxFov;
			m_targetFov = BlockDefinition.MaxFov;
			m_enableIdleRotation.Value &= myObjectBuilder_Searchlight.EnableIdleRotation;
			m_previousIdleRotationState = myObjectBuilder_Searchlight.PreviousIdleRotationState;
			m_gunIdleElevationAzimuthUnknown = true;
			m_targetFlags.ValueChanged += delegate
			{
				OnTargetFlagChanged();
			};
			base.InvalidateOnMove = false;
			CreateUpdateTimer(GetTimerTime(0), MyTimerTypes.Frame10);
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME | MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			base.ResourceSink.Update();
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStackIsFunctionalChanged;
			base.IsWorkingChanged += CubeBlockOnWorkingChanged;
			MyCubeGrid cubeGrid2 = base.CubeGrid;
			cubeGrid2.IsPreviewChanged = (Action<bool>)Delegate.Combine(cubeGrid2.IsPreviewChanged, new Action<bool>(OnIsPreviewChanged));
			MyRenderComponentSearchlight obj = base.Render as MyRenderComponentSearchlight;
			obj.Lights = m_lightingLogic.Lights;
			obj.LightShaftOffset = BlockDefinition.LightShaftOffset;
			m_gunIdleElevationAzimuthUnknown = true;
			InitializeTargetingSystem();
			m_targetSync.AlwaysReject();
			m_lockedTarget.ValueChanged += OnLockedTargetChanged;
			m_outOfRangeNotification = new MyHudNotification(MyCommonTexts.TargetOutOfRange, 1000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
			m_noTargetNotification = new MyHudNotification(MyCommonTexts.NoTargetLocked, 1000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
			m_lockingInProgressNotification = new MyHudNotification(MyCommonTexts.LockingInProgress, 1000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
			m_shootingRange.ValueChanged += delegate
			{
				ShootingRangeChanged();
			};
		}

		public MySearchlight()
		{
			m_turretController = new MyTurretController(this);
			m_turretController.OnControlAcquired += AcquireControl;
			m_turretController.OnControlReleased += ReleaseControl;
			m_turretController.OnRotationUpdate += RotateModels;
			m_turretController.OnMoveAndRotationUpdate += MoveAndRotate;
			m_turretController.OnCameraOverlayUpdate += SetCameraOverlay;
			m_rotationAndElevationSync.ValueChanged += delegate
			{
				RotationAndElevationSync();
			};
			m_soundEmitter = new MyEntity3DSoundEmitter(this, useStaticList: true);
			m_soundEmitterForRotation = new MyEntity3DSoundEmitter(this, useStaticList: true);
			ControllerInfo.ControlReleased += OnControlReleased;
			if (!Sync.IsDedicated)
			{
				m_parallelFlag.Enable(this);
			}
			base.Render = new MyRenderComponentSearchlight();
			m_rotationSpeedForSubparts.ValueChanged += delegate
			{
				RotationSpeedChanged();
			};
			m_randomStandbyChange_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		private void OnTargetFlagChanged()
		{
			MyTurretTargetFlags myTurretTargetFlags = TargetFlags & ~MyTurretTargetFlags.NotNeutrals;
			m_workingFlagCombination = myTurretTargetFlags != (MyTurretTargetFlags)0;
			m_targetingSystem.OnTargetFlagChanged();
		}

		private void RotationSpeedChanged()
		{
			m_rotationMatrix = Matrix.CreateRotationZ(m_rotationSpeedForSubparts);
		}

		private void ComponentStackIsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		private void CubeBlockOnWorkingChanged(MyCubeBlock block)
		{
			m_lightingLogic.IsPositionDirty = true;
		}

		private void OnIsPreviewChanged(bool isPreview)
		{
			MySandboxGame.Static.Invoke(delegate
			{
				if (!base.MarkedForClose)
				{
					MyCubeGrid cubeGrid = base.CubeGrid;
					if (cubeGrid != null && !cubeGrid.MarkedForClose)
					{
						UpdateVisual();
					}
				}
			}, "LightPreviewUpdate");
		}

		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		private void InitializeTargetingSystem()
		{
			if (m_targetingSystem == null)
			{
				m_targetSync.ValueChanged += delegate
				{
					m_targetingSystem.TargetChanged();
				};
				m_targetingSystem = new MyLargeTurretTargetingSystem(this);
			}
		}

		private void UpdateAiWeapon()
		{
			if (!Sync.IsServer)
			{
				return;
			}
			m_targetingSystem.TargetPrediction.UsePrediction = true;
			if (m_targetingSystem.GetTargetDistanceSquared() < (double)SearchRangeSquared || m_targetingSystem.TargetPrediction.IsTargetPositionManual)
			{
				if ((Target != null || m_targetingSystem.TargetPrediction.IsTargetPositionManual) && (m_targetingSystem.IsTargetVisible(Target) || m_targetingSystem.TargetPrediction.IsTargetPositionManual) && RotationAndElevation())
				{
					_ = !m_targetingSystem.TargetPrediction.IsTargetPositionManual;
				}
				else
					_ = 0;
				if (!m_targetingSystem.IsValidTarget(Target))
				{
					m_targetingSystem.ResetTarget();
				}
			}
			else if (m_isInRandomRotationDistance)
			{
				ResetRandomAiming();
				RandomMovement();
			}
			else
			{
				StopAimingSound();
			}
		}

		internal void StopAimingSound()
		{
			if (m_soundEmitterForRotation != null && (m_soundEmitterForRotation.SoundId == m_rotatingCueEnum.Arcade || m_soundEmitterForRotation.SoundId == m_rotatingCueEnum.Realistic))
			{
				m_soundEmitterForRotation.StopSound(forced: false);
			}
		}

		private void RandomMovement()
		{
			if (!m_enableIdleRotation)
			{
				return;
			}
			float randomStandbyRotation = m_randomStandbyRotation;
			float randomStandbyElevation = m_randomStandbyElevation;
			float num = m_rotationSpeed * 16f;
			bool flag = false;
			float num2 = randomStandbyRotation - Rotation;
			if (num2 * num2 > 9.99999944E-11f)
			{
				Rotation += MathHelper.Clamp(num2, 0f - num, num);
				flag = true;
			}
			if (true)
			{
				num = m_elevationSpeed * 16f;
				num2 = randomStandbyElevation - Elevation;
				if (num2 * num2 > 9.99999944E-11f)
				{
					Elevation += MathHelper.Clamp(num2, 0f - num, num);
					flag = true;
				}
			}
			m_playAimingSound = flag;
			ClampRotationAndElevation();
			if (m_randomIsMoving)
			{
				if (!flag)
				{
					SetupSearchRaycast();
					m_randomIsMoving = false;
				}
			}
			else if (flag)
			{
				m_randomIsMoving = true;
			}
		}

		private void OnStopAI()
		{
			if (m_soundEmitter != null)
			{
				if (m_soundEmitter.IsPlaying)
				{
					m_soundEmitter.StopSound(forced: true);
				}
				if (m_soundEmitterForRotation.IsPlaying)
				{
					m_soundEmitterForRotation.StopSound(forced: true);
				}
			}
		}

		private void SetupSearchRaycast()
		{
			MatrixD lightSourceWorldMatrix = LightSourceWorldMatrix;
			Vector3D hitPosition = lightSourceWorldMatrix.Translation + lightSourceWorldMatrix.Forward * m_searchingRange;
			m_laserLength = m_searchingRange;
			m_hitPosition = hitPosition;
		}

		private void ResetRandomAiming()
		{
			if (MySandboxGame.TotalGamePlayTimeInMilliseconds - m_randomStandbyChange_ms > m_randomStandbyChangeConst_ms)
			{
				m_randomStandbyRotation = MyUtils.GetRandomFloat(-3.141593f, 3.141593f);
				m_randomStandbyElevation = MyUtils.GetRandomFloat(0f, (float)Math.E * 449f / 777f);
				m_randomStandbyChange_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		public bool RotationAndElevation()
		{
			bool flag = false;
			if (Target != null && Vector3D.DistanceSquared(Target.PositionComp.GetPosition(), base.PositionComp.GetPosition()) < 10.0)
			{
				return true;
			}
			Vector3 vector = Vector3.Zero;
			if (Target != null || m_targetingSystem.TargetPrediction.IsTargetPositionManual)
			{
				bool usePrediction = m_targetingSystem.TargetPrediction.UsePrediction;
				m_targetingSystem.TargetPrediction.UsePrediction = m_targetingSystem.TargetPrediction.IsFastPrediction;
				Vector3D aimPosition = m_targetingSystem.AimPosition;
				m_targetingSystem.TargetPrediction.UsePrediction = usePrediction;
				vector = LookAt(aimPosition);
			}
			float y = vector.Y;
			float x = vector.X;
			float num = m_rotationSpeed * 16f;
			float num2 = MathHelper.WrapAngle(y - Rotation);
			Rotation += MathHelper.Clamp(num2, 0f - num, num);
			flag = num2 * num2 > 9.99999944E-11f;
			if (Rotation > 3.141593f)
			{
				Rotation -= (float)Math.PI * 2f;
			}
			else if (Rotation < -3.141593f)
			{
				Rotation += (float)Math.PI * 2f;
			}
			num = m_elevationSpeed * 16f;
			float num3 = x - Elevation;
			Elevation += MathHelper.Clamp(num3, 0f - num, num);
			flag = (m_playAimingSound = flag || num3 * num3 > 9.99999944E-11f);
			ClampRotationAndElevation();
			RotateModels();
			if (Target != null || m_targetingSystem.TargetPrediction.IsTargetPositionManual)
			{
				float num4 = Math.Abs(y - Rotation);
				float num5 = Math.Abs(x - Elevation);
				IsAimed = num4 <= float.Epsilon && num5 <= 0.01f;
			}
			else
			{
				IsAimed = false;
			}
			return IsAimed;
		}

		private void OnLockedTargetChanged(SyncBase obj)
		{
			m_targetingSystem.ForgetGridTarget();
			if (m_lockedTarget.Value != 0L)
			{
				m_forcedTargetRange = m_maxRangeMeter;
				MyEntity entityById = MyEntities.GetEntityById(m_lockedTarget.Value);
				if (!Sync.IsDedicated && entityById != null && (entityById.PositionComp.GetPosition() - base.PositionComp.GetPosition()).LengthSquared() > (double)(m_forcedTargetRange * m_forcedTargetRange))
				{
					MyHud.Notifications.Add(m_outOfRangeNotification);
				}
			}
		}

		private void ShootingRangeChanged()
		{
			AdjustSearchingRange();
			if (Sync.IsServer && base.IsWorking && AiEnabled && MySession.Static.CreativeMode)
			{
				m_targetingSystem.CheckAndSelectNearTargetsParallel();
			}
		}

		private void AdjustSearchingRange()
		{
			m_searchingRange = m_shootingRange;
		}

		[Event(null, 1535)]
		[Reliable]
		[Broadcast]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		public void sync_ControlledEntity_Used()
		{
			ReleaseControl();
		}

		protected override uint GetDefaultTimeForUpdateTimer(int index)
		{
			switch (index)
			{
			case 0:
				return TIMER_NORMAL_IN_FRAMES;
			case 1:
				return TIMER_TIER1_IN_FRAMES;
			default:
				return 0u;
			}
		}

		public override void DoUpdateTimerTick()
		{
			base.DoUpdateTimerTick();
			if (base.Render.IsVisible() && base.IsWorking && Enabled)
			{
				m_isInRandomRotationDistance = false;
				if (!m_turretController.IsPlayerControlled && AiEnabled)
				{
					UpdateAiWeapon();
					m_isInRandomRotationDistance = MySector.MainCamera.GetDistanceFromPoint(base.PositionComp.GetPosition()) <= 600.0;
					if (m_isInRandomRotationDistance)
					{
						base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
					}
				}
				m_targetingSystem.UpdateVisibilityCache();
				if (m_turretController.IsControlled)
				{
					if (!m_turretController.IsInRangeAndPlayerHasAccess())
					{
						ReleaseControl();
						if (MyGuiScreenTerminal.IsOpen && MyGuiScreenTerminal.InteractedEntity == this)
						{
							MyGuiScreenTerminal.Hide();
						}
					}
					else
					{
						m_turretController.GetFirstRadioReceiver()?.UpdateHud(showMyself: true);
						if (m_turretController.IsControlledByLocalPlayer && MyGuiScreenHudSpace.Static != null)
						{
							MyGuiScreenHudSpace.Static.SetToolbarVisible(visible: false);
						}
					}
				}
				else if ((long)m_lockedTarget != 0L)
				{
					MyEntity entityById = MyEntities.GetEntityById(m_lockedTarget.Value);
					MyCubeGrid entity;
					if (entityById != null && !m_targetingSystem.CheckForcedTarget(entityById) && (entity = entityById as MyCubeGrid) != null)
					{
						m_targetingSystem.ForceGridTarget(entity, m_forcedTargetRange);
					}
					if (Sync.IsServer && !m_targetingSystem.TargetPrediction.IsTargetPositionManual && m_workingFlagCombination)
					{
						m_targetingSystem.CheckAndSelectNearTargetsParallel();
					}
					if (m_randomIsMoving || (Target != null && m_targetingSystem.IsPotentialTarget))
					{
						SetupSearchRaycast();
					}
				}
				else
				{
					if (Sync.IsServer && !m_targetingSystem.TargetPrediction.IsTargetPositionManual && m_workingFlagCombination)
					{
						m_targetingSystem.CheckAndSelectNearTargetsParallel();
					}
					if (m_randomIsMoving || (Target != null && m_targetingSystem.IsPotentialTarget))
					{
						SetupSearchRaycast();
					}
				}
				if (m_playAimingSound)
				{
					PlayAimingSound();
				}
				else
				{
					StopAimingSound();
				}
			}
			m_isMoving = false;
			if (base.CubeGrid?.Physics != null)
			{
				MatrixD worldMatrix = base.CubeGrid.WorldMatrix;
				if (!worldMatrix.EqualsFast(ref m_lastTestedGridWM))
				{
					m_isMoving = true;
					m_lastTestedGridWM = worldMatrix;
					base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
				}
			}
		}

		private void PlayAimingSound()
		{
			if (m_soundEmitterForRotation != null && !m_soundEmitterForRotation.IsPlaying)
			{
				m_soundEmitterForRotation.PlaySound(m_rotatingCueEnum, stopPrevious: true);
			}
		}

		protected void GetCameraDummy()
		{
			if (m_base2.Model != null && m_base2.Model.Dummies.ContainsKey("camera"))
			{
				CameraDummy = m_base2.Model.Dummies["camera"];
			}
		}

		private void GetLightSourceDummy()
		{
			if (m_base2.Model != null && m_base2.Model.Dummies.ContainsKey(BlockDefinition.LightDummyName))
			{
				LightSourceDummy = m_base2.Model.Dummies[BlockDefinition.LightDummyName];
			}
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_Searchlight myObjectBuilder_Searchlight = (MyObjectBuilder_Searchlight)base.GetObjectBuilderCubeBlock(copy);
			Vector4 vector = m_lightingLogic.Color.ToVector4();
			myObjectBuilder_Searchlight.ColorRed = vector.X;
			myObjectBuilder_Searchlight.ColorGreen = vector.Y;
			myObjectBuilder_Searchlight.ColorBlue = vector.Z;
			myObjectBuilder_Searchlight.ColorAlpha = vector.W;
			myObjectBuilder_Searchlight.Radius = m_lightingLogic.Radius;
			myObjectBuilder_Searchlight.ReflectorRadius = m_lightingLogic.ReflectorRadius;
			myObjectBuilder_Searchlight.Falloff = m_lightingLogic.Falloff;
			myObjectBuilder_Searchlight.Intensity = IntensitySync;
			myObjectBuilder_Searchlight.BlinkIntervalSeconds = BlinkIntervalSecondsSync;
			myObjectBuilder_Searchlight.BlinkLenght = BlinkLengthSync;
			myObjectBuilder_Searchlight.BlinkOffset = BlinkOffsetSync;
			myObjectBuilder_Searchlight.Offset = LightOffsetSync;
			myObjectBuilder_Searchlight.RotationSpeed = m_rotationSpeedForSubparts;
			myObjectBuilder_Searchlight.Range = m_shootingRange;
			myObjectBuilder_Searchlight.Target = ((Target != null) ? Target.EntityId : 0);
			myObjectBuilder_Searchlight.IsPotentialTarget = m_targetingSystem.IsPotentialTarget;
			myObjectBuilder_Searchlight.Flags.TargetMeteors = TargetMeteors;
			myObjectBuilder_Searchlight.Flags.TargetMissiles = TargetMissiles;
			myObjectBuilder_Searchlight.EnableIdleRotation = EnableIdleRotation;
			myObjectBuilder_Searchlight.TargetCharacters = TargetCharacters;
			myObjectBuilder_Searchlight.Flags.TargetSmallGrids = TargetSmallGrids;
			myObjectBuilder_Searchlight.Flags.TargetLargeGrids = TargetLargeGrids;
			myObjectBuilder_Searchlight.Flags.TargetStations = TargetStations;
			myObjectBuilder_Searchlight.Flags.TargetNeutrals = TargetNeutrals;
			myObjectBuilder_Searchlight.Flags.TargetFriends = TargetFriends;
			myObjectBuilder_Searchlight.Flags.TargetCharacters = TargetCharacters;
			myObjectBuilder_Searchlight.Flags.TargetEnemies = TargetEnemies;
			myObjectBuilder_Searchlight.TargetLocking = TargetLocking;
			myObjectBuilder_Searchlight.TargetingGroup = GetTargetingGroupHash();
			myObjectBuilder_Searchlight.PreviousIdleRotationState = m_previousIdleRotationState;
			if (m_turretController.PreviousControlledEntity != null)
			{
				myObjectBuilder_Searchlight.PreviousControlledEntityId = m_turretController.PreviousControlledEntity.Entity.EntityId;
				myObjectBuilder_Searchlight.Rotation = Rotation;
				myObjectBuilder_Searchlight.Elevation = Elevation;
			}
			return myObjectBuilder_Searchlight;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			if (MyMultiplayer.Static != null && Sync.IsServer)
			{
				m_replicableServer = MyMultiplayer.Static.ReplicationLayer as MyReplicationServer;
				m_blocksReplicable = MyExternalReplicable.FindByObject(this);
			}
			if (base.CubeGrid != null)
			{
				MyCubeGrid cubeGrid = base.CubeGrid;
				cubeGrid.IsPreviewChanged = (Action<bool>)Delegate.Combine(cubeGrid.IsPreviewChanged, new Action<bool>(PreviewChangedCallback));
				PreviewChangedCallback(base.CubeGrid.IsPreview);
			}
			m_lightingLogic.OnAddedToScene();
		}

		public override void OnRemovedFromScene(object source)
		{
			if (base.CubeGrid != null)
			{
				MyCubeGrid cubeGrid = base.CubeGrid;
				cubeGrid.IsPreviewChanged = (Action<bool>)Delegate.Remove(cubeGrid.IsPreviewChanged, new Action<bool>(PreviewChangedCallback));
			}
			base.OnRemovedFromScene(source);
		}

		private void PreviewChangedCallback(bool isPreview)
		{
			base.IsPreview = isPreview;
		}

		public override void OnModelChange()
		{
			base.OnModelChange();
			if (base.IsBuilt)
			{
				if (!base.Subparts.TryGetValue("InteriorTurretBase1", out var value))
				{
					return;
				}
				m_base1 = value;
				if (!m_base1.Subparts.TryGetValue("InteriorTurretBase2", out var value2))
				{
					return;
				}
				m_base2 = value2;
				GetCameraDummy();
				GetLightSourceDummy();
				m_lightingLogic.OnModelChange();
			}
			else
			{
				m_base1 = null;
				m_base2 = null;
			}
			ResetRotation();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override void UpdateVisual()
		{
			base.UpdateVisual();
			m_lightingLogic.UpdateVisual();
			UpdateEmissivity(force: true);
		}

		protected void ResetRotation()
		{
			Rotation = 0f;
			Elevation = 0f;
			ClampRotationAndElevation();
			m_randomStandbyElevation = 0f;
			m_randomStandbyRotation = 0f;
			m_randomStandbyChange_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		protected override void CreateTerminalControls()
		{
			if (m_targetingSystem == null)
			{
				InitializeTargetingSystem();
			}
			if (!MyTerminalControlFactory.AreControlsCreated<MySearchlight>())
			{
				base.CreateTerminalControls();
				m_targetingSystem.InjectTerminalControls(this, allowIdleMovement: true);
				MyTerminalControlFactory.AddControl(new MyTerminalControlColor<MySearchlight>("Color", MySpaceTexts.BlockPropertyTitle_LightColor)
				{
					Getter = (MySearchlight x) => x.m_lightingLogic.Color,
					Setter = delegate(MySearchlight x, Color v)
					{
						x.LightColorSync.Value = v;
					}
				});
				MyTerminalControlSlider<MySearchlight> myTerminalControlSlider = new MyTerminalControlSlider<MySearchlight>("Radius", MySpaceTexts.BlockPropertyTitle_LightRadius, MySpaceTexts.BlockPropertyDescription_LightRadius);
				myTerminalControlSlider.SetLimits((MySearchlight x) => (!x.m_lightingLogic.IsReflector) ? x.m_lightingLogic.RadiusBounds.Min : x.m_lightingLogic.ReflectorRadiusBounds.Min, (MySearchlight x) => (!x.m_lightingLogic.IsReflector) ? x.m_lightingLogic.RadiusBounds.Max : x.m_lightingLogic.ReflectorRadiusBounds.Max);
				myTerminalControlSlider.DefaultValueGetter = (MySearchlight x) => (!x.m_lightingLogic.IsReflector) ? x.m_lightingLogic.RadiusBounds.Default : x.m_lightingLogic.ReflectorRadiusBounds.Default;
				myTerminalControlSlider.Getter = (MySearchlight x) => (!x.m_lightingLogic.IsReflector) ? x.m_lightingLogic.Radius : x.m_lightingLogic.ReflectorRadius;
				myTerminalControlSlider.Setter = delegate(MySearchlight x, float v)
				{
					x.LightRadiusSync.Value = v;
				};
				myTerminalControlSlider.Writer = delegate(MySearchlight x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_lightingLogic.IsReflector ? x.m_lightingLogic.ReflectorRadius : x.m_lightingLogic.Radius, 1)).Append(" m");
				};
				myTerminalControlSlider.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider);
				MyTerminalControlSlider<MySearchlight> myTerminalControlSlider2 = new MyTerminalControlSlider<MySearchlight>("Falloff", MySpaceTexts.BlockPropertyTitle_LightFalloff, MySpaceTexts.BlockPropertyDescription_LightFalloff);
				myTerminalControlSlider2.SetLimits((MySearchlight x) => x.m_lightingLogic.FalloffBounds.Min, (MySearchlight x) => x.m_lightingLogic.FalloffBounds.Max);
				myTerminalControlSlider2.DefaultValueGetter = (MySearchlight x) => x.m_lightingLogic.FalloffBounds.Default;
				myTerminalControlSlider2.Getter = (MySearchlight x) => x.m_lightingLogic.Falloff;
				myTerminalControlSlider2.Setter = delegate(MySearchlight x, float v)
				{
					x.LightFalloffSync.Value = v;
				};
				myTerminalControlSlider2.Writer = delegate(MySearchlight x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_lightingLogic.Falloff, 1));
				};
				myTerminalControlSlider2.Visible = (MySearchlight x) => x.SupportsFalloff;
				myTerminalControlSlider2.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
				MyTerminalControlSlider<MySearchlight> myTerminalControlSlider3 = new MyTerminalControlSlider<MySearchlight>("Intensity", MySpaceTexts.BlockPropertyTitle_LightIntensity, MySpaceTexts.BlockPropertyDescription_LightIntensity);
				myTerminalControlSlider3.SetLimits((MySearchlight x) => x.m_lightingLogic.IntensityBounds.Min, (MySearchlight x) => x.m_lightingLogic.IntensityBounds.Max);
				myTerminalControlSlider3.DefaultValueGetter = (MySearchlight x) => x.m_lightingLogic.IntensityBounds.Default;
				myTerminalControlSlider3.Getter = (MySearchlight x) => x.m_lightingLogic.Intensity;
				myTerminalControlSlider3.Setter = delegate(MySearchlight x, float v)
				{
					x.m_lightingLogic.Intensity = v;
				};
				myTerminalControlSlider3.Writer = delegate(MySearchlight x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_lightingLogic.Intensity, 1));
				};
				myTerminalControlSlider3.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
				MyTerminalControlSlider<MySearchlight> myTerminalControlSlider4 = new MyTerminalControlSlider<MySearchlight>("Offset", MySpaceTexts.BlockPropertyTitle_LightOffset, MySpaceTexts.BlockPropertyDescription_LightOffset);
				myTerminalControlSlider4.SetLimits((MySearchlight x) => x.m_lightingLogic.OffsetBounds.Min, (MySearchlight x) => x.m_lightingLogic.OffsetBounds.Max);
				myTerminalControlSlider4.DefaultValueGetter = (MySearchlight x) => x.m_lightingLogic.OffsetBounds.Default;
				myTerminalControlSlider4.Getter = (MySearchlight x) => x.m_lightingLogic.Offset;
				myTerminalControlSlider4.Setter = delegate(MySearchlight x, float v)
				{
					x.LightOffsetSync.Value = v;
				};
				myTerminalControlSlider4.Writer = delegate(MySearchlight x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_lightingLogic.Offset, 1));
				};
				myTerminalControlSlider4.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider4);
				MyTerminalControlSlider<MySearchlight> myTerminalControlSlider5 = new MyTerminalControlSlider<MySearchlight>("Blink Interval", MySpaceTexts.BlockPropertyTitle_LightBlinkInterval, MySpaceTexts.BlockPropertyDescription_LightBlinkInterval);
				myTerminalControlSlider5.SetLimits((MySearchlight x) => x.m_lightingLogic.BlinkIntervalSecondsBounds.Min, (MySearchlight x) => x.m_lightingLogic.BlinkIntervalSecondsBounds.Max);
				myTerminalControlSlider5.DefaultValueGetter = (MySearchlight x) => x.m_lightingLogic.BlinkIntervalSecondsBounds.Default;
				myTerminalControlSlider5.Getter = (MySearchlight x) => x.m_lightingLogic.BlinkIntervalSeconds;
				myTerminalControlSlider5.Setter = delegate(MySearchlight x, float v)
				{
					x.m_lightingLogic.BlinkIntervalSeconds = v;
				};
				myTerminalControlSlider5.Writer = delegate(MySearchlight x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_lightingLogic.BlinkIntervalSeconds, 1)).Append(" s");
				};
				myTerminalControlSlider5.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider5);
				MyTerminalControlSlider<MySearchlight> myTerminalControlSlider6 = new MyTerminalControlSlider<MySearchlight>("Blink Lenght", MySpaceTexts.BlockPropertyTitle_LightBlinkLenght, MySpaceTexts.BlockPropertyDescription_LightBlinkLenght, isAutoscaleEnabled: true, isAutoEllipsisEnabled: true);
				myTerminalControlSlider6.SetLimits((MySearchlight x) => x.m_lightingLogic.BlinkLengthBounds.Min, (MySearchlight x) => x.m_lightingLogic.BlinkLengthBounds.Max);
				myTerminalControlSlider6.DefaultValueGetter = (MySearchlight x) => x.m_lightingLogic.BlinkLengthBounds.Default;
				myTerminalControlSlider6.Getter = (MySearchlight x) => x.m_lightingLogic.BlinkLength;
				myTerminalControlSlider6.Setter = delegate(MySearchlight x, float v)
				{
					x.m_lightingLogic.BlinkLength = v;
				};
				myTerminalControlSlider6.Writer = delegate(MySearchlight x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_lightingLogic.BlinkLength, 1)).Append(" %");
				};
				myTerminalControlSlider6.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider6);
				MyTerminalControlSlider<MySearchlight> myTerminalControlSlider7 = new MyTerminalControlSlider<MySearchlight>("Blink Offset", MySpaceTexts.BlockPropertyTitle_LightBlinkOffset, MySpaceTexts.BlockPropertyDescription_LightBlinkOffset, isAutoscaleEnabled: true, isAutoEllipsisEnabled: true);
				myTerminalControlSlider7.SetLimits((MySearchlight x) => x.m_lightingLogic.BlinkOffsetBounds.Min, (MySearchlight x) => x.m_lightingLogic.BlinkOffsetBounds.Max);
				myTerminalControlSlider7.DefaultValueGetter = (MySearchlight x) => x.m_lightingLogic.BlinkOffsetBounds.Default;
				myTerminalControlSlider7.Getter = (MySearchlight x) => x.m_lightingLogic.BlinkOffset;
				myTerminalControlSlider7.Setter = delegate(MySearchlight x, float v)
				{
					x.m_lightingLogic.BlinkOffset = v;
				};
				myTerminalControlSlider7.Writer = delegate(MySearchlight x, StringBuilder result)
				{
					result.Append(MyValueFormatter.GetFormatedFloat(x.m_lightingLogic.BlinkOffset, 1)).Append(" %");
				};
				myTerminalControlSlider7.EnableActions();
				MyTerminalControlFactory.AddControl(myTerminalControlSlider7);
			}
		}

		public override void SerializeControls(BitStream stream)
		{
			m_turretController.SerializeControls(stream);
		}

		public override void DeserializeControls(BitStream stream, bool outOfOrder)
		{
			m_turretController.DeserializeControls(stream, outOfOrder);
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (m_targetToSet != 0L && base.IsWorking)
			{
				MyEntity entity = null;
				if (MyEntities.TryGetEntityById(m_targetToSet, out entity))
				{
					m_targetingSystem.ResetTarget();
				}
			}
			m_turretController.UpdatePlayerControllers();
			MyCubeGrid cubeGrid = base.CubeGrid;
			MyCubeGridRenderCell orAddCell = cubeGrid.RenderData.GetOrAddCell(base.Position * cubeGrid.GridSize);
			if (orAddCell.ParentCullObject == uint.MaxValue)
			{
				orAddCell.RebuildInstanceParts(cubeGrid.Render.GetRenderFlags());
			}
			if (m_base1 != null)
			{
				m_base1.Render.SetParent(0, orAddCell.ParentCullObject);
				m_base1.NeedsWorldMatrix = false;
				m_base1.InvalidateOnMove = false;
			}
			if (m_base2 != null)
			{
				m_base2.Render.SetParent(0, orAddCell.ParentCullObject);
				m_base2.NeedsWorldMatrix = false;
				m_base2.InvalidateOnMove = false;
			}
			RotateModels();
			m_lightingLogic.UpdateOnceBeforeFrame();
			if (m_lightingLogic.NeedPerFrameUpdate)
			{
				m_parallelFlag.Enable(this);
			}
		}

		public virtual void UpdateBeforeSimulationParallel()
		{
			if (m_turretController.IsControlledByLocalPlayer && MySession.Static.CameraController == this)
			{
				if (MyInput.Static.DeltaMouseScrollWheelValue() != 0 && MyGuiScreenToolbarConfigBase.Static == null && !MyGuiScreenTerminal.IsOpen)
				{
					ChangeZoom(MyInput.Static.DeltaMouseScrollWheelValue());
				}
				MyStringId context = MySession.Static.ControlledEntity?.ControlContext ?? MyStringId.NullOrEmpty;
				if (MyControllerHelper.IsControl(context, MyControlsSpace.CAMERA_ZOOM_IN, MyControlStateType.PRESSED))
				{
					ChangeZoomPrecise(0f - GAMEPAD_ZOOM_SPEED);
				}
				else if (MyControllerHelper.IsControl(context, MyControlsSpace.CAMERA_ZOOM_OUT, MyControlStateType.PRESSED))
				{
					ChangeZoomPrecise(GAMEPAD_ZOOM_SPEED);
				}
			}
		}

		public virtual void UpdateAfterSimulationParallel()
		{
			base.UpdateAfterSimulation();
			if (!Sync.IsDedicated)
			{
				RotateModels();
			}
			if (!((MySector.MainCamera.Position - base.PositionComp.GetPosition()).AbsMax() > 5000.0))
			{
				if (LightSourceDummy != null && m_base2 != null)
				{
					MatrixD worldMatrix = m_base2.WorldMatrix;
					Matrix m = Matrix.Normalize(LightSourceDummy.Matrix);
					worldMatrix = MatrixD.Multiply(m, worldMatrix);
					m_lightingLogic.IsPositionDirty = true;
					m_lightingLogic.UpdateAfterSimulation(worldMatrix.Translation, MatrixD.CreateFromYawPitchRoll(Rotation, Elevation, 0.0));
				}
				else
				{
					m_lightingLogic.UpdateAfterSimulation();
				}
				if (Sync.IsServer && base.IsWorking && !base.CubeGrid.IsPreview && Target != null)
				{
					m_rotationAndElevationSync.Value = new SyncRotationAndElevation
					{
						Rotation = Rotation,
						Elevation = Elevation
					};
				}
			}
		}

		public override void UpdateAfterSimulation100()
		{
			if ((MySector.MainCamera.Position - base.PositionComp.GetPosition()).AbsMax() > 5000.0)
			{
				m_parallelFlag.Disable(this);
				return;
			}
			bool needPerFrameUpdate = m_lightingLogic.NeedPerFrameUpdate;
			m_parallelFlag.Set(this, needPerFrameUpdate);
			m_lightingLogic.UpdateLightProperties();
		}

		public sealed override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (base.IsWorking && base.Parent.Physics != null && base.Parent.Physics.Enabled)
			{
				if (m_turretController.IsControlledByLocalPlayer && MySession.Static.CameraController == this)
				{
					m_fov = MathHelper.Lerp(m_fov, m_targetFov, 0.5f);
					SetFov(m_fov);
				}
				if (base.Render.IsVisible() && base.IsWorking && !m_turretController.IsPlayerControlled && AiEnabled)
				{
					UpdateAiWeapon();
				}
				if (!NeedsPerFrameUpdate)
				{
					base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
				}
			}
		}

		public override void DisableUpdates()
		{
			base.DisableUpdates();
			m_parallelFlag.Disable(this);
		}

		public void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator, bool overrideControlCheck, bool rotateShip)
		{
			LastRotationIndicator = new Vector3(rotationIndicator, rollIndicator);
			float rotationMultiplier = GetRotationMultiplier();
			m_turretController.LastNetMoveState = new MyGridClientState
			{
				Move = moveIndicator,
				Rotation = rotationIndicator * rotationMultiplier,
				Roll = rollIndicator * rotationMultiplier
			};
			m_turretController.LastNetRotateShip = rotateShip;
			bool flag = false;
			MyShipController myShipController = m_turretController.PreviousControlledEntity as MyShipController;
			if (myShipController != null)
			{
				bool flag2 = true;
				if (!overrideControlCheck && base.CubeGrid.HasMainCockpit() && (myShipController.Pilot == null || myShipController.Pilot != MySession.Static.LocalCharacter))
				{
					flag2 = false;
				}
				if (flag2 && (overrideControlCheck || myShipController.HasLocalPlayerAccess()))
				{
					m_turretController.LastNetCanControl = true;
					if (rotateShip)
					{
						myShipController.MoveAndRotate(moveIndicator, rotationIndicator * rotationMultiplier, rollIndicator * rotationMultiplier);
						flag = true;
					}
					else
					{
						myShipController.MoveAndRotate(moveIndicator, Vector2.Zero, rollIndicator * rotationMultiplier);
					}
					myShipController.MoveAndRotate();
					base.CubeGrid.ControlledFromTurret = true;
				}
				else
				{
					m_turretController.LastNetCanControl = false;
				}
			}
			if (!flag && (rotationIndicator.X != 0f || rotationIndicator.Y != 0f) && base.SyncObject != null)
			{
				float num = 0.05f * m_rotationSpeed * 16f;
				Rotation -= rotationIndicator.Y * num * rotationMultiplier;
				Elevation -= rotationIndicator.X * num * rotationMultiplier;
				Elevation = MathHelper.Clamp(Elevation, m_minElevationRadians, 1.5533427f);
				RotateModels();
				m_rotationAndElevationSync.Value = new SyncRotationAndElevation
				{
					Rotation = Rotation,
					Elevation = Elevation
				};
			}
		}

		public bool WasControllingCockpitWhenSaved()
		{
			if (m_turretController.SavedPreviousControlledEntityId.HasValue && MyEntities.TryGetEntityById(m_turretController.SavedPreviousControlledEntityId.Value, out var entity))
			{
				return entity is MyCockpit;
			}
			return false;
		}

		private void RotationAndElevationSync()
		{
			if (!m_turretController.IsControlledByLocalPlayer)
			{
				UpdateRotationAndElevation(m_rotationAndElevationSync.Value.Rotation, m_rotationAndElevationSync.Value.Elevation);
			}
		}

		public void UpdateRotationAndElevation(float newRotation, float newElevation)
		{
			Rotation = newRotation;
			Elevation = newElevation;
			RotateModels();
		}

		private float NormalizeAngle(int angle)
		{
			int num = angle % 360;
			if (num == 0 && angle != 0)
			{
				return 360f;
			}
			return num;
		}

		private float GetRotationMultiplier()
		{
			float zoomNormalized = GetZoomNormalized();
			return zoomNormalized * ROTATION_MULTIPLIER_NORMAL + (1f - zoomNormalized) * ROTATION_MULTIPLIER_ZOOMED;
		}

		private float GetZoomNormalized()
		{
			return (m_targetFov - m_minFov) / (m_maxFov - m_minFov);
		}

		private float ClampRotation(float value)
		{
			if (IsRotationLimited())
			{
				value = Math.Min(m_maxAzimuthRadians, Math.Max(m_minAzimuthRadians, value));
			}
			return value;
		}

		private bool IsRotationLimited()
		{
			return (double)Math.Abs(m_maxAzimuthRadians - m_minAzimuthRadians - (float)Math.PI * 2f) > 0.01;
		}

		private float ClampElevation(float value)
		{
			if (IsElevationLimited())
			{
				value = Math.Min(m_maxElevationRadians, Math.Max(m_minElevationRadians, value));
			}
			return value;
		}

		private bool IsElevationLimited()
		{
			return (double)Math.Abs(m_maxElevationRadians - m_minElevationRadians - (float)Math.PI * 2f) > 0.01;
		}

		private void ClampRotationAndElevation()
		{
			Rotation = ClampRotation(Rotation);
			Elevation = ClampElevation(Elevation);
		}

		private void RotateModels()
		{
			if (m_base1 == null || m_base2 == null || !m_base1.Render.IsChild(0))
			{
				m_transformDirty = false;
				return;
			}
			if (m_transformDirty && !m_isMoving && m_replicableServer != null && Sync.IsDedicated && !m_replicableServer.IsReplicated(m_blocksReplicable) && Target == null)
			{
				m_transformDirty = false;
				return;
			}
			ClampRotationAndElevation();
			Matrix.CreateRotationY(Rotation, out var result);
			result.Translation = m_base1.PositionComp.LocalMatrixRef.Translation;
			Matrix matrix = base.PositionComp.LocalMatrixRef;
			Matrix.Multiply(ref result, ref matrix, out var result2);
			m_base1.PositionComp.SetLocalMatrix(ref result, m_base1.Physics, updateWorld: false, ref result2, forceUpdateRender: true);
			Matrix.CreateRotationX(Elevation, out var result3);
			result3.Translation = m_base2.PositionComp.LocalMatrixRef.Translation;
			Matrix.Multiply(ref result3, ref result2, out var result4);
			m_base2.PositionComp.SetLocalMatrix(ref result3, m_base2.Physics, updateWorld: true, ref result4, forceUpdateRender: true);
			m_transformDirty = false;
		}

		private void AcquireControl(Sandbox.Game.Entities.IMyControllableEntity previousControlledEntity)
		{
			if (previousControlledEntity.ControllerInfo.Controller != null)
			{
				previousControlledEntity.SwitchControl(this);
			}
			if (m_turretController.IsControlledByLocalPlayer)
			{
				MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, this);
				m_targetFov = m_maxFov;
				SetFov(m_maxFov);
			}
			MyCharacter myCharacter = m_turretController.PreviousControlledEntity as MyCharacter;
			if (myCharacter != null)
			{
				myCharacter.CurrentRemoteControl = this;
			}
			OnStopAI();
			m_targetingSystem.ResetTarget();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
		}

		public void ForceReleaseControl()
		{
			ReleaseControl();
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			if (base.IsWorking)
			{
				m_randomStandbyChange_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
			}
			else
			{
				OnStopWorking();
			}
		}

		public override void OnCubeGridChanged(MyCubeGrid oldGrid)
		{
			base.OnCubeGridChanged(oldGrid);
			m_lightingLogic.IsPositionDirty = true;
		}

		private void UpdateIntensity()
		{
			float num = m_lightingLogic.CurrentLightPower * m_lightingLogic.Intensity;
			foreach (MyLight light in m_lightingLogic.Lights)
			{
				light.ReflectorIntensity = num * 8f;
				light.Intensity = num * 0.3f;
				float num2 = num / m_lightingLogic.IntensityBounds.Max;
				float num3 = m_flare.Intensity * num;
				if (num3 < m_flare.Intensity)
				{
					num3 = m_flare.Intensity;
				}
				light.GlareIntensity = num3;
				float num4 = num2 / 2f + 0.5f;
				light.GlareSize = m_flare.Size * num4;
				m_lightingLogic.BulbColor = m_lightingLogic.ComputeBulbColor();
			}
		}

		private void UpdateEnabled(bool state)
		{
			if (m_lightingLogic.Lights == null)
			{
				return;
			}
			bool flag = state && base.CubeGrid.Projector == null;
			foreach (MyLight light in m_lightingLogic.Lights)
			{
				light.ReflectorOn = flag;
				light.LightOn = flag;
				light.GlareOn = flag;
			}
		}

		private void InitLight(MyLight light, Vector4 color, float radius, float falloff)
		{
			light.Start(color, base.CubeGrid.GridScale * radius, DisplayNameText);
			light.ReflectorOn = true;
			light.LightType = MyLightType.SPOTLIGHT;
			light.ReflectorTexture = BlockDefinition.ReflectorTexture;
			light.Falloff = 0.3f;
			light.GlossFactor = 0f;
			light.ReflectorGlossFactor = 1f;
			light.ReflectorFalloff = 0.5f;
			light.GlareOn = light.LightOn;
			light.GlareQuerySize = GlareQuerySizeDef;
			light.GlareType = MyGlareTypeEnum.Directional;
			MyDefinitionId id = new MyDefinitionId(typeof(MyObjectBuilder_FlareDefinition), BlockDefinition.Flare);
			m_flare = (MyDefinitionManager.Static.GetDefinition(id) as MyFlareDefinition) ?? new MyFlareDefinition();
			light.GlareSize = m_flare.Size;
			light.SubGlares = m_flare.SubGlares;
			UpdateIntensity();
			base.Render.NeedsDrawFromParent = true;
		}

		private void UpdateRadius(float value)
		{
			m_lightingLogic.Radius = 10f * (m_lightingLogic.ReflectorRadius / m_lightingLogic.ReflectorRadiusBounds.Max);
		}

		private void UpdateEmissivity(bool force = false)
		{
			bool flag = m_lightingLogic.Lights.Count > 0 && m_lightingLogic.Lights[0].ReflectorOn;
			if (m_lightingLogic.Lights != null && (m_wasWorking != (base.IsWorking && flag) || force))
			{
				m_wasWorking = base.IsWorking && flag;
				m_lightingLogic.IsEmissiveMaterialDirty = true;
				m_lightingLogic.UpdateEmissiveMaterial();
			}
		}

		private void ReleaseControl(bool previousClosed = false)
		{
			if (m_turretController.IsControlledByLocalPlayer && MyGuiScreenHudSpace.Static != null)
			{
				MyGuiScreenHudSpace.Static.SetToolbarVisible(visible: true);
			}
			if (!m_turretController.IsPlayerControlled)
			{
				return;
			}
			MyCockpit myCockpit = m_turretController.PreviousControlledEntity as MyCockpit;
			if (myCockpit != null && (previousClosed || myCockpit.Pilot == null || myCockpit.MarkedForClose || myCockpit.Closed))
			{
				ReturnControl(m_turretController.CockpitPilot);
				return;
			}
			MyCharacter myCharacter = m_turretController.PreviousControlledEntity as MyCharacter;
			if (myCharacter != null)
			{
				myCharacter.CurrentRemoteControl = null;
			}
			base.CubeGrid.ControlledFromTurret = false;
			ReturnControl(m_turretController.PreviousControlledEntity);
		}

		private void ReturnControl(Sandbox.Game.Entities.IMyControllableEntity nextControllableEntity)
		{
			if (ControllerInfo.Controller != null)
			{
				this.SwitchControl(nextControllableEntity);
			}
			m_turretController.PreviousControlledEntity = null;
			m_randomStandbyElevation = Elevation;
			m_randomStandbyRotation = Rotation;
			m_randomStandbyChange_ms = MySandboxGame.TotalGamePlayTimeInMilliseconds;
		}

		private void OnControlReleased(MyEntityController controller)
		{
			RemoveCameraOverlay();
		}

		private void SetCameraOverlay()
		{
			if (m_turretController.IsControlledByLocalPlayer)
			{
				base.CubeGrid.GridSystems.CameraSystem.ResetCurrentCamera();
				if (BlockDefinition != null && BlockDefinition.OverlayTexture != null)
				{
					MyHudCameraOverlay.TextureName = BlockDefinition.OverlayTexture;
					MyHudCameraOverlay.Enabled = true;
				}
				else
				{
					MyHudCameraOverlay.Enabled = false;
				}
			}
		}

		private void RemoveCameraOverlay()
		{
			if (m_turretController.IsControlledByLocalPlayer)
			{
				if (MyGuiScreenHudSpace.Static != null)
				{
					MyGuiScreenHudSpace.Static.SetToolbarVisible(visible: true);
				}
				MyHudCameraOverlay.Enabled = false;
				m_turretController.GetFirstRadioReceiver()?.Clear();
				ExitView();
			}
		}

		public void ExitView()
		{
			MySector.MainCamera.FieldOfView = MySandboxGame.Config.FieldOfView;
		}

		protected override void Closing()
		{
			m_turretController.OnControlAcquired -= AcquireControl;
			m_turretController.OnControlReleased -= ReleaseControl;
			m_turretController.OnRotationUpdate -= RotateModels;
			m_turretController.OnMoveAndRotationUpdate -= MoveAndRotate;
			m_turretController.OnCameraOverlayUpdate -= SetCameraOverlay;
			m_lightingLogic.CloseLights();
			m_lightingLogic.OnPropertiesChanged -= base.RaisePropertiesChanged;
			m_lightingLogic.OnUpdateEnabled -= UpdateEnabled;
			m_lightingLogic.OnIntensityUpdated -= UpdateIntensity;
			m_lightingLogic.OnInitLight -= InitLight;
			m_lightingLogic.OnEmissivityUpdated -= UpdateEmissivity;
			m_lightingLogic.OnRadiusUpdated -= UpdateRadius;
			try
			{
				m_targetingSystem.FinishTasks();
			}
			catch
			{
			}
			base.ResourceSink.IsPoweredChanged -= Receiver_IsPoweredChanged;
			base.Closing();
			ReleaseControl();
			m_targetingSystem.ResetTarget();
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink != null && base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		protected override void OnStartWorking()
		{
			base.OnStartWorking();
			m_lightingLogic.IsEmissiveMaterialDirty = true;
			m_parallelFlag.Enable(this);
		}

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
			if (m_turretController.IsControlled)
			{
				ReleaseControl();
			}
			m_targetingSystem.ResetTarget();
			m_lightingLogic.IsEmissiveMaterialDirty = true;
			m_parallelFlag.Enable(this);
		}

		private void ChangeZoom(int deltaZoom)
		{
			if (deltaZoom > 0)
			{
				m_targetFov -= 0.15f;
				if (m_targetFov < m_minFov)
				{
					m_targetFov = m_minFov;
				}
			}
			else
			{
				m_targetFov += 0.15f;
				if (m_targetFov > m_maxFov)
				{
					m_targetFov = m_maxFov;
				}
			}
			SetFov(m_fov);
		}

		private void ChangeZoomPrecise(float deltaZoom)
		{
			m_targetFov += deltaZoom;
			if (deltaZoom < 0f)
			{
				if (m_targetFov < m_minFov)
				{
					m_targetFov = m_minFov;
				}
			}
			else if (m_targetFov > m_maxFov)
			{
				m_targetFov = m_maxFov;
			}
			SetFov(m_fov);
		}

		private static void SetFov(float fov)
		{
			fov = MathHelper.Clamp(fov, 1E-05f, (float)Math.PI * 179f / 180f);
			MySector.MainCamera.FieldOfView = fov;
		}

		public UseActionResult CanUse(UseActionEnum actionEnum, Sandbox.Game.Entities.IMyControllableEntity user)
		{
			return m_turretController.CanUse(actionEnum, user);
		}

		public void RemoveUsers(bool local)
		{
		}

		public bool CanControl()
		{
			return m_turretController.CanControl();
		}

		public void RequestControl()
		{
			m_turretController.RequestControl();
		}

		public bool IsTurretTerminalVisible()
		{
			return true;
		}

		public void ForgetTarget()
		{
		}

		public void CopyTarget()
		{
		}

		public void SetTargetingMode(MyTargetingGroupDefinition definition)
		{
		}

		public MatrixD? GetOverridingFocusMatrix()
		{
			return null;
		}

		public bool IsTargetLockingEnabled()
		{
			return TargetLocking;
		}

		public void SetLockedTarget(VRage.Game.ModAPI.IMyCubeGrid target)
		{
		}

		public MatrixD GetWorldMatrix()
		{
			return LightSourceWorldMatrix;
		}

		public bool CanActiveToolShoot()
		{
			return true;
		}

		public bool IsShipToolSelected()
		{
			return false;
		}

		public Vector3D GetActiveToolPosition()
		{
			return LightSourceWorldMatrix.Translation;
		}
	}
}
