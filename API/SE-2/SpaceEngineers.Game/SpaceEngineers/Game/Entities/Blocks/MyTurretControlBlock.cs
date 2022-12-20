using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.Entities.UseObject;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.ClientStates;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.Entity.UseObject;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Groups;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilder;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks
{
	[MyCubeBlockType(typeof(MyObjectBuilder_TurretControlBlock))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMyTurretControlBlock),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMyTurretControlBlock)
	})]
	public class MyTurretControlBlock : MyFunctionalBlock, Sandbox.Game.Entities.IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity, IMyUsableEntity, IMyTurretTerminalControlReceiver, IMyTargetingReceiver, IMyShootOrigin, IMyTargetingCapableBlock, IMyPilotable, SpaceEngineers.Game.ModAPI.IMyTurretControlBlock, SpaceEngineers.Game.ModAPI.Ingame.IMyTurretControlBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity
	{
		private class BlockComparer : IComparer<MyCubeBlock>
		{
			public int Compare(MyCubeBlock x, MyCubeBlock y)
			{
				bool flag = x is IMyGunObject<MyGunBase>;
				bool flag2 = y is IMyGunObject<MyGunBase>;
				if (flag != flag2)
				{
					if (flag)
					{
						return -1;
					}
					return 1;
				}
				if (flag)
				{
					return x.DisplayNameText.CompareTo(y.DisplayNameText);
				}
				bool flag3 = x is IMyGunObject<MyToolBase>;
				bool flag4 = y is IMyGunObject<MyToolBase>;
				if (flag3 != flag4)
				{
					if (flag3)
					{
						return -1;
					}
					return 1;
				}
				return x.DisplayNameText.CompareTo(y.DisplayNameText);
			}
		}

		private enum MyDirectionSource
		{
			Weapon = 3,
			Tool = 2,
			Camera = 1,
			None = 0
		}

		protected sealed class SendToolbarItemChanged_003C_003ESandbox_Game_Entities_Blocks_ToolbarItem_0023System_Int32 : ICallSite<MyTurretControlBlock, ToolbarItem, int, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTurretControlBlock @this, in ToolbarItem sentItem, in int index, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SendToolbarItemChanged(sentItem, index);
			}
		}

		protected sealed class ToolUnselectionRequest_003C_003ESystem_Collections_Generic_List_00601_003CSystem_Int64_003E : ICallSite<MyTurretControlBlock, List<long>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTurretControlBlock @this, in List<long> toSync, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ToolUnselectionRequest(toSync);
			}
		}

		protected sealed class ToolSelectionRequest_003C_003ESystem_Collections_Generic_List_00601_003CSystem_Int64_003E : ICallSite<MyTurretControlBlock, List<long>, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTurretControlBlock @this, in List<long> toSync, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ToolSelectionRequest(toSync);
			}
		}

		protected sealed class RequestUseMessage_003C_003EVRage_Game_Entity_UseObject_UseActionEnum_0023System_Int64 : ICallSite<MyTurretControlBlock, UseActionEnum, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTurretControlBlock @this, in UseActionEnum useAction, in long usedById, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RequestUseMessage(useAction, usedById);
			}
		}

		protected sealed class UseSuccessCallback_003C_003EVRage_Game_Entity_UseObject_UseActionEnum_0023System_Int64_0023VRage_Game_Entity_UseObject_UseActionResult : ICallSite<MyTurretControlBlock, UseActionEnum, long, UseActionResult, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTurretControlBlock @this, in UseActionEnum useAction, in long usedById, in UseActionResult useResult, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.UseSuccessCallback(useAction, usedById, useResult);
			}
		}

		protected sealed class RequestRelease_003C_003ESystem_Boolean : ICallSite<MyTurretControlBlock, bool, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTurretControlBlock @this, in bool previousClosed, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.RequestRelease(previousClosed);
			}
		}

		protected sealed class UseFailureCallback_003C_003EVRage_Game_Entity_UseObject_UseActionEnum_0023System_Int64_0023VRage_Game_Entity_UseObject_UseActionResult : ICallSite<MyTurretControlBlock, UseActionEnum, long, UseActionResult, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTurretControlBlock @this, in UseActionEnum useAction, in long usedById, in UseActionResult useResult, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.UseFailureCallback(useAction, usedById, useResult);
			}
		}

		protected sealed class sync_ControlledEntity_Used_003C_003E : ICallSite<MyTurretControlBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTurretControlBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.sync_ControlledEntity_Used();
			}
		}

		protected sealed class ForgetTargetServer_003C_003E : ICallSite<MyTurretControlBlock, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTurretControlBlock @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ForgetTargetServer();
			}
		}

		protected sealed class CopyTargetServer_003C_003ESystem_Int64_0023System_Int64 : ICallSite<MyTurretControlBlock, long, long, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyTurretControlBlock @this, in long characterEntityId, in long cockpitEntityId, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.CopyTargetServer(characterEntityId, cockpitEntityId);
			}
		}

		protected class m_boundAzimuth_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType boundAzimuth;
				ISyncType result = (boundAzimuth = new Sync<long, SyncDirection.BothWays>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_boundAzimuth = (Sync<long, SyncDirection.BothWays>)boundAzimuth;
				return result;
			}
		}

		protected class m_boundElevation_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType boundElevation;
				ISyncType result = (boundElevation = new Sync<long, SyncDirection.BothWays>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_boundElevation = (Sync<long, SyncDirection.BothWays>)boundElevation;
				return result;
			}
		}

		protected class m_boundCamera_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType boundCamera;
				ISyncType result = (boundCamera = new Sync<long, SyncDirection.BothWays>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_boundCamera = (Sync<long, SyncDirection.BothWays>)boundCamera;
				return result;
			}
		}

		protected class m_isCorrectTurret_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isCorrectTurret;
				ISyncType result = (isCorrectTurret = new Sync<bool, SyncDirection.FromServer>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_isCorrectTurret = (Sync<bool, SyncDirection.FromServer>)isCorrectTurret;
				return result;
			}
		}

		protected class m_shootingRange_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType shootingRange;
				ISyncType result = (shootingRange = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_shootingRange = (Sync<float, SyncDirection.BothWays>)shootingRange;
				return result;
			}
		}

		protected class m_targetingGroup_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType targetingGroup;
				ISyncType result = (targetingGroup = new Sync<MyStringHash, SyncDirection.BothWays>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_targetingGroup = (Sync<MyStringHash, SyncDirection.BothWays>)targetingGroup;
				return result;
			}
		}

		protected class m_targetSync_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType targetSync;
				ISyncType result = (targetSync = new Sync<MyLargeTurretTargetingSystem.CurrentTargetSync, SyncDirection.FromServer>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_targetSync = (Sync<MyLargeTurretTargetingSystem.CurrentTargetSync, SyncDirection.FromServer>)targetSync;
				return result;
			}
		}

		protected class m_lockedTarget_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType lockedTarget;
				ISyncType result = (lockedTarget = new Sync<long, SyncDirection.FromServer>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_lockedTarget = (Sync<long, SyncDirection.FromServer>)lockedTarget;
				return result;
			}
		}

		protected class m_directionBlockId_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType directionBlockId;
				ISyncType result = (directionBlockId = new Sync<long, SyncDirection.FromServer>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_directionBlockId = (Sync<long, SyncDirection.FromServer>)directionBlockId;
				return result;
			}
		}

		protected class m_areWeaponsMismatched_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType areWeaponsMismatched;
				ISyncType result = (areWeaponsMismatched = new Sync<bool, SyncDirection.FromServer>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_areWeaponsMismatched = (Sync<bool, SyncDirection.FromServer>)areWeaponsMismatched;
				return result;
			}
		}

		protected class m_velocityMultiplierAzimuth_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType velocityMultiplierAzimuth;
				ISyncType result = (velocityMultiplierAzimuth = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_velocityMultiplierAzimuth = (Sync<float, SyncDirection.BothWays>)velocityMultiplierAzimuth;
				return result;
			}
		}

		protected class m_velocityMultiplierElevation_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType velocityMultiplierElevation;
				ISyncType result = (velocityMultiplierElevation = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_velocityMultiplierElevation = (Sync<float, SyncDirection.BothWays>)velocityMultiplierElevation;
				return result;
			}
		}

		protected class m_targetLocking_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType targetLocking;
				ISyncType result = (targetLocking = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_targetLocking = (Sync<bool, SyncDirection.BothWays>)targetLocking;
				return result;
			}
		}

		protected class m_angleDeviation_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType angleDeviation;
				ISyncType result = (angleDeviation = new Sync<float, SyncDirection.BothWays>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_angleDeviation = (Sync<float, SyncDirection.BothWays>)angleDeviation;
				return result;
			}
		}

		protected class m_allowAI_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType allowAI;
				ISyncType result = (allowAI = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_allowAI = (Sync<bool, SyncDirection.BothWays>)allowAI;
				return result;
			}
		}

		protected class m_targetFlags_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType targetFlags;
				ISyncType result = (targetFlags = new Sync<MyTurretTargetFlags, SyncDirection.BothWays>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_targetFlags = (Sync<MyTurretTargetFlags, SyncDirection.BothWays>)targetFlags;
				return result;
			}
		}

		protected class m_clientState_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType clientState;
				ISyncType result = (clientState = new Sync<MyCockpitMoveState?, SyncDirection.BothWays>(P_1, P_2));
				((MyTurretControlBlock)P_0).m_clientState = (Sync<MyCockpitMoveState?, SyncDirection.BothWays>)clientState;
				return result;
			}
		}

		private static bool DISPLAY_EXTENDED_DETAILS = false;

		private static float STUCK_FRAME_LIMIT = 5f;

		private static int BLACKLIST_TIME = 60;

		private static float STUCK_LIMIT_CHECK = 1E-05f;

		/// <summary>
		/// Signifies how many frames, before the correct angle would be achieved, slowdown of rotor should start.
		/// </summary>
		public static float AIM_SLOWDOWN_THRESHOLD = 2f;

		private static List<MyToolbar> m_openedToolbars;

		private static bool m_shouldSetOtherToolbars;

		private bool m_syncing;

		private float m_maxRangeMeter = 800f;

		private Sync<long, SyncDirection.BothWays> m_boundAzimuth;

		private Sync<long, SyncDirection.BothWays> m_boundElevation;

		private Sync<long, SyncDirection.BothWays> m_boundCamera;

		private readonly Sync<bool, SyncDirection.FromServer> m_isCorrectTurret;

		private readonly Sync<float, SyncDirection.BothWays> m_shootingRange;

		private readonly Sync<MyStringHash, SyncDirection.BothWays> m_targetingGroup;

		private readonly Sync<MyLargeTurretTargetingSystem.CurrentTargetSync, SyncDirection.FromServer> m_targetSync;

		private Sync<long, SyncDirection.FromServer> m_lockedTarget;

		private Sync<long, SyncDirection.FromServer> m_directionBlockId;

		private Sync<bool, SyncDirection.FromServer> m_areWeaponsMismatched;

		private MyHudNotification m_outOfRangeNotification;

		private MyHudNotification m_noTargetNotification;

		private MyHudNotification m_lockingInProgressNotification;

		private MyMotorStator m_azimuthor;

		private MyMotorStator m_elevator;

		private MyCameraBlock m_camera;

		private Sync<float, SyncDirection.BothWays> m_velocityMultiplierAzimuth;

		private Sync<float, SyncDirection.BothWays> m_velocityMultiplierElevation;

		private Sync<bool, SyncDirection.BothWays> m_targetLocking;

		private Sync<float, SyncDirection.BothWays> m_angleDeviation;

		private Sync<bool, SyncDirection.BothWays> m_allowAI;

		private Dictionary<long, MyFunctionalBlock> m_boundTools = new Dictionary<long, MyFunctionalBlock>();

		private List<MyGuiControlListbox.Item> m_selectedToolsToAdd = new List<MyGuiControlListbox.Item>();

		private List<MyGuiControlListbox.Item> m_selectedToolsToRemove = new List<MyGuiControlListbox.Item>();

		private List<MyFunctionalBlock> m_cachedToolBlocks = new List<MyFunctionalBlock>();

		private List<MyCameraBlock> m_cachedCameraBlocks = new List<MyCameraBlock>();

		private List<MyMotorStator> m_cachedRotorBlocks = new List<MyMotorStator>();

		private long m_lastCacheTime = -1L;

		private float m_lastRotationHorizontal;

		private float m_lastRotationVertical;

		private MyLargeTurretTargetingSystem m_targetingSystem;

		private float m_forcedTargetRange = 5000f;

		private float m_searchingRange = 800f;

		private float m_angleAzi;

		private float m_angleEle;

		private MyControllerInfo m_controllerInfo = new MyControllerInfo();

		private bool m_isAimed;

		private bool m_isShooting;

		/// <summary>
		/// Some weapons/tools were added while shooting was in progress, Reset all weapons/tool.
		/// </summary>
		private bool m_shouldRefreshShooting;

		private MyCubeBlock m_directionBlock;

		private MyDirectionSource m_directionSource;

		private bool m_workingFlagCombination = true;

		private bool m_isAimingToolbarActive;

		private long? m_possessedCamera;

		private bool m_initialize = true;

		private bool m_resubscribeToLogicalGroup = true;

		private bool m_recheckBlockConnections;

		private bool m_resetIgnoredGrids;

		private MyEntity m_stuckTarget;

		private int m_stuckCounter;

		private MyCharacter m_cockpitPilot;

		private Sandbox.Game.Entities.IMyControllableEntity m_previousControlledEntity;

		private long? m_savedPreviousControlledEntityId;

		private readonly Sync<MyTurretTargetFlags, SyncDirection.BothWays> m_targetFlags;

		private MyToolbar m_toolbar;

		private List<ToolbarItem> m_items;

		private readonly Sync<MyCockpitMoveState?, SyncDirection.BothWays> m_clientState;

		public Sync<MyLargeTurretTargetingSystem.CurrentTargetSync, SyncDirection.FromServer> TargetSync => m_targetSync;

		public bool IsTargetLocked => m_lockedTarget.Value != 0;

		public new MyTurretControlBlockDefinition BlockDefinition => base.BlockDefinition as MyTurretControlBlockDefinition;

		private List<MyFunctionalBlock> ToolBlocks
		{
			get
			{
				if (MySession.Static.GameplayFrameCounter - m_lastCacheTime > 100)
				{
					CacheBlocks();
				}
				return m_cachedToolBlocks;
			}
		}

		private List<MyCameraBlock> CameraBlocks
		{
			get
			{
				if (MySession.Static.GameplayFrameCounter - m_lastCacheTime > 100)
				{
					CacheBlocks();
				}
				return m_cachedCameraBlocks;
			}
		}

		private List<MyMotorStator> RotorBlocks
		{
			get
			{
				if (MySession.Static.GameplayFrameCounter - m_lastCacheTime > 100)
				{
					CacheBlocks();
				}
				return m_cachedRotorBlocks;
			}
		}

		private MyMotorStator Azimuthor
		{
			get
			{
				return m_azimuthor;
			}
			set
			{
				if (m_azimuthor != value)
				{
					if (Sync.IsServer && m_azimuthor != null)
					{
						m_azimuthor.RemovedFromScene -= BlockRemovedAzimuthor;
						m_azimuthor.OnClose -= BlockRemovedAzimuthor;
					}
					m_azimuthor = value;
					if (Sync.IsServer && m_azimuthor != null)
					{
						m_azimuthor.RemovedFromScene += BlockRemovedAzimuthor;
						m_azimuthor.OnClose += BlockRemovedAzimuthor;
					}
					ResetTargetingIgnoredGrids();
				}
			}
		}

		private MyMotorStator Elevator
		{
			get
			{
				return m_elevator;
			}
			set
			{
				if (m_elevator != value)
				{
					if (Sync.IsServer && m_elevator != null)
					{
						m_elevator.RemovedFromScene -= BlockRemovedElevator;
						m_elevator.OnClose -= BlockRemovedElevator;
					}
					m_elevator = value;
					if (Sync.IsServer && m_elevator != null)
					{
						m_elevator.RemovedFromScene += BlockRemovedElevator;
						m_elevator.OnClose += BlockRemovedElevator;
					}
					ResetTargetingIgnoredGrids();
				}
			}
		}

		private MyCameraBlock Camera
		{
			get
			{
				return m_camera;
			}
			set
			{
				if (m_camera != value)
				{
					if (Sync.IsServer && m_camera != null)
					{
						m_camera.RemovedFromScene -= BlockRemovedCamera;
						m_camera.OnClose -= BlockRemovedCamera;
					}
					m_camera = value;
					if (Sync.IsServer && m_camera != null)
					{
						m_camera.RemovedFromScene += BlockRemovedCamera;
						m_camera.OnClose += BlockRemovedCamera;
					}
				}
			}
		}

		public float AngleDeviation
		{
			get
			{
				return MathHelper.ToDegrees(m_angleDeviation);
			}
			set
			{
				float num = MathHelper.ToRadians(value);
				if ((float)m_angleDeviation != num)
				{
					m_angleDeviation.Value = num;
				}
			}
		}

		public MyControllerInfo ControllerInfo => m_controllerInfo;

		public MyEntity Entity => this;

		VRage.ModAPI.IMyEntity VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Entity => this;

		public float HeadLocalXAngle { get; set; }

		public float HeadLocalYAngle { get; set; }

		public bool EnabledBroadcasting => false;

		public MyToolbarType ToolbarType => MyToolbarType.None;

		public MyStringId ControlContext => MySpaceBindingCreator.CX_SPACESHIP;

		public MyStringId AuxiliaryContext => MySpaceBindingCreator.AX_ACTIONS;

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

		public MyToolbar Toolbar => null;

		public MyEntity RelativeDampeningEntity { get; set; }

		IMyControllerInfo VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ControllerInfo => ControllerInfo;

		public bool ForceFirstPersonCamera { get; set; }

		public Vector3 LastMotionIndicator => Vector3.Zero;

		public Vector3 LastRotationIndicator { get; set; }

		public bool EnabledThrusts => false;

		public bool EnabledDamping => false;

		public bool EnabledLights => false;

		public bool EnabledLeadingGears => false;

		public bool EnabledReactors => false;

		public bool EnabledHelmet => false;

		public bool PrimaryLookaround => false;

		public bool EnableIdleRotation
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool IsControlled => PreviousControlledEntity != null;

		public bool IsAimed
		{
			get
			{
				return m_isAimed;
			}
			protected set
			{
				if (m_isAimed == value)
				{
					return;
				}
				m_isAimed = value;
				if (m_isAimed)
				{
					m_toolbar.UpdateItem(0);
					if (Sync.IsServer)
					{
						m_toolbar.ActivateItemAtSlot(0, checkIfWantsToBeActivated: false, playActivationSound: false);
					}
				}
				else
				{
					m_toolbar.UpdateItem(1);
					if (Sync.IsServer)
					{
						m_toolbar.ActivateItemAtSlot(1, checkIfWantsToBeActivated: false, playActivationSound: false);
					}
				}
			}
		}

		public bool IsAimingToolbarActive
		{
			get
			{
				return m_isAimingToolbarActive;
			}
			set
			{
				if (m_isAimingToolbarActive == value)
				{
					return;
				}
				m_isAimingToolbarActive = value;
				if (m_isAimingToolbarActive)
				{
					m_toolbar.UpdateItem(0);
					if (Sync.IsServer)
					{
						m_toolbar.ActivateItemAtSlot(0, checkIfWantsToBeActivated: false, playActivationSound: false);
					}
				}
				else
				{
					m_toolbar.UpdateItem(1);
					if (Sync.IsServer)
					{
						m_toolbar.ActivateItemAtSlot(1, checkIfWantsToBeActivated: false, playActivationSound: false);
					}
				}
			}
		}

		public MyEntity Target => m_targetingSystem.Target;

		public float VelocityMultiplierAzimuthRpm
		{
			get
			{
				return (float)m_velocityMultiplierAzimuth * GetAzimuthSpeedMax(this);
			}
			set
			{
				m_velocityMultiplierAzimuth.Value = value / GetAzimuthSpeedMax(this);
			}
		}

		public float VelocityMultiplierElevationRpm
		{
			get
			{
				return (float)m_velocityMultiplierElevation * GetElevatorSpeedMax(this);
			}
			set
			{
				m_velocityMultiplierElevation.Value = value / GetElevatorSpeedMax(this);
			}
		}

		public bool IsPlayerControlled
		{
			get
			{
				if (Sync.Players.GetControllingPlayer(this) != null)
				{
					return true;
				}
				return false;
			}
		}

		public MyCharacter Pilot
		{
			get
			{
				MyCharacter myCharacter = PreviousControlledEntity as MyCharacter;
				if (myCharacter != null)
				{
					return myCharacter;
				}
				return m_cockpitPilot;
			}
		}

		public virtual Sandbox.Game.Entities.IMyControllableEntity PreviousControlledEntity
		{
			get
			{
				if (m_savedPreviousControlledEntityId.HasValue && TryFindSavedEntity())
				{
					m_savedPreviousControlledEntityId = null;
				}
				return m_previousControlledEntity;
			}
			private set
			{
				if (value == m_previousControlledEntity)
				{
					return;
				}
				if (m_previousControlledEntity != null)
				{
					RemovePreviousControllerEvents();
					if (m_cockpitPilot != null)
					{
						m_cockpitPilot.OnMarkForClose -= Entity_OnPreviousMarkForClose;
					}
				}
				m_previousControlledEntity = value;
				if (m_previousControlledEntity == null)
				{
					return;
				}
				AddPreviousControllerEvents();
				if (PreviousControlledEntity is MyCockpit)
				{
					m_cockpitPilot = (PreviousControlledEntity as MyCockpit).Pilot;
					if (m_cockpitPilot != null)
					{
						m_cockpitPilot.OnMarkForClose += Entity_OnPreviousMarkForClose;
					}
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

		public bool AIEnabled
		{
			get
			{
				return m_allowAI;
			}
			set
			{
				m_allowAI.Value = value;
			}
		}

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

		public long OwnerIdentityId => base.OwnerId;

		public Vector3D EntityPosition => base.PositionComp.GetPosition();

		public Vector3D ShootDirection
		{
			get
			{
				if (m_directionSource == MyDirectionSource.Weapon)
				{
					return ((IMyGunObject<MyGunBase>)m_directionBlock).GetShootDirection();
				}
				if (m_directionSource > MyDirectionSource.None)
				{
					return m_directionBlock.WorldMatrix.Forward;
				}
				return base.PositionComp.WorldMatrix.Forward;
			}
		}

		public Vector3D ShootOrigin
		{
			get
			{
				if (m_directionSource == MyDirectionSource.Weapon)
				{
					return ((IMyGunObject<MyGunBase>)m_directionBlock).GetMuzzlePosition();
				}
				if (m_directionSource > MyDirectionSource.None)
				{
					return m_directionBlock.PositionComp.GetPosition();
				}
				return base.PositionComp.GetPosition();
			}
		}

		public MyGridTargeting GridTargeting => base.CubeGrid.Components.Get<MyGridTargeting>();

		public float MechanicalDamage => 0f;

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

		public float MinRange => 4f;

		public float MaxRange => m_maxRangeMeter;

		public MyDefinitionBase GetAmmoDefinition
		{
			get
			{
				IMyGunObject<MyGunBase> myGunObject;
				if (m_directionBlock != null && (myGunObject = m_directionBlock as IMyGunObject<MyGunBase>) != null)
				{
					return myGunObject.GunBase.CurrentAmmoDefinition;
				}
				return null;
			}
		}

		public MyStringHash TargetingGroup => m_targetingGroup.Value;

		public bool CanHavePreviousControlledEntity => true;

		public bool CanHavePreviousCameraEntity => true;

		public VRage.ModAPI.IMyEntity GetPreviousCameraEntity => PreviousControlledEntity as VRage.ModAPI.IMyEntity;

		public float MaxShootRange => GetMaxAmmoRange();

		VRage.ModAPI.IMyEntity SpaceEngineers.Game.ModAPI.IMyTurretControlBlock.Target => this;

		IMyCharacter SpaceEngineers.Game.ModAPI.IMyTurretControlBlock.Pilot
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool IsUnderControl => ControllerInfo.Controller != null;

		public float Range
		{
			get
			{
				return ShootRangeSimple;
			}
			set
			{
				ShootRangeSimple = value;
			}
		}

		public bool HasTarget => Target != null;

		public Sandbox.ModAPI.Ingame.IMyMotorStator AzimuthRotor
		{
			get
			{
				return m_azimuthor;
			}
			set
			{
				if (m_boundAzimuth.Value != value.EntityId)
				{
					m_boundAzimuth.Value = value.EntityId;
				}
			}
		}

		public Sandbox.ModAPI.Ingame.IMyMotorStator ElevationRotor
		{
			get
			{
				return m_elevator;
			}
			set
			{
				if (m_boundElevation.Value != value.EntityId)
				{
					m_boundElevation.Value = value.EntityId;
				}
			}
		}

		Sandbox.ModAPI.Ingame.IMyCameraBlock SpaceEngineers.Game.ModAPI.Ingame.IMyTurretControlBlock.Camera
		{
			get
			{
				return m_camera;
			}
			set
			{
				if (m_boundCamera.Value != value.EntityId)
				{
					m_boundCamera.Value = value.EntityId;
				}
			}
		}

		public Vector2 RotationIndicator { get; set; }

		public Vector3 MoveIndicator { get; set; }

		public float RollIndicator { get; set; }

		public MyTurretTargetingOptions HiddenTargetingOptions => (MyTurretTargetingOptions)0;

		private void BlockRemovedAzimuthor(object obj)
		{
			if (Azimuthor == obj)
			{
				Azimuthor = null;
				m_boundAzimuth.Value = 0L;
			}
			CheckTurretCorrectness();
		}

		private void BlockRemovedElevator(object obj)
		{
			if (Elevator == obj)
			{
				Elevator = null;
				m_boundElevation.Value = 0L;
			}
			CheckTurretCorrectness();
		}

		private void BlockRemovedCamera(object obj)
		{
			if (Camera == obj)
			{
				Camera = null;
				m_boundCamera.Value = 0L;
			}
			CheckTurretCorrectness();
		}

		public MyTurretControlBlock()
		{
			m_targetSync.AlwaysReject();
			InitializeTargetingSystem();
			base.Render = new MyRenderComponentScreenAreas(this);
			m_shootingRange.ValueChanged += delegate
			{
				ShootingRangeChanged();
			};
			m_isCorrectTurret.ValueChanged += delegate
			{
				TurretCorrectnessChanged();
			};
			m_lockedTarget.ValueChanged += OnLockedTargetChanged;
			m_allowAI.ValueChanged += OnAllowAIChanged;
			m_clientState.ValueChanged += ClientStateOnValueChanged;
			m_outOfRangeNotification = new MyHudNotification(MyCommonTexts.TargetOutOfRange, 1000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
			m_noTargetNotification = new MyHudNotification(MyCommonTexts.NoTargetLocked, 1000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
			m_lockingInProgressNotification = new MyHudNotification(MyCommonTexts.LockingInProgress, 1000, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, 0, MyNotificationLevel.Important);
		}

		private void ClientStateOnValueChanged(SyncBase obj)
		{
			if (m_clientState.Value.HasValue)
			{
				MyCockpitMoveState value = m_clientState.Value.Value;
				MoveIndicator = value.Move;
				RotationIndicator = value.Rotation;
				RollIndicator = value.Roll;
			}
			else
			{
				MoveIndicator = Vector3.Zero;
				RotationIndicator = Vector2.Zero;
				RollIndicator = 0f;
			}
		}

		private void OnAllowAIChanged(SyncBase obj)
		{
			if (!Sync.IsServer)
			{
				return;
			}
			CheckNeedsUpdate();
			if (!m_allowAI && ControllerInfo != null)
			{
				if (m_azimuthor != null)
				{
					m_azimuthor.TargetVelocity.Value = 0f;
				}
				if (m_elevator != null)
				{
					m_elevator.TargetVelocity.Value = 0f;
				}
				EndShoot(MyShootActionEnum.PrimaryAction);
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (Sync.IsServer)
			{
				base.CubeGrid.OnBlockRemoved += OnBlockRemoved;
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			if (Sync.IsServer)
			{
				base.CubeGrid.OnBlockRemoved -= OnBlockRemoved;
			}
		}

		private void OnBlockRemoved(MySlimBlock obj)
		{
			MyCubeBlock fatBlock = obj.FatBlock;
			if (fatBlock == null)
			{
				return;
			}
			if ((long)m_boundAzimuth == fatBlock.EntityId)
			{
				m_boundAzimuth.Value = 0L;
			}
			else if ((long)m_boundElevation == fatBlock.EntityId)
			{
				m_boundElevation.Value = 0L;
			}
			else if ((long)m_boundCamera == fatBlock.EntityId)
			{
				m_boundElevation.Value = 0L;
			}
			else if (fatBlock is MyFunctionalBlock)
			{
				if (m_boundTools.ContainsKey(fatBlock.EntityId))
				{
					SyncToolUnselection(new List<long> { fatBlock.EntityId });
				}
			}
			else if (Sync.IsServer)
			{
				MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(base.CubeGrid);
				if (group != null)
				{
					group.GroupData.OnGridRemoved -= GridRemovedFromGroupCallback;
				}
			}
		}

		private void OnLockedTargetChanged(SyncBase obj)
		{
			m_targetingSystem.ForgetGridTarget();
			if (m_lockedTarget.Value != 0L)
			{
				m_forcedTargetRange = GetMaxAmmoRange();
				MyEntity entityById = MyEntities.GetEntityById(m_lockedTarget.Value);
				if (!Sync.IsDedicated && entityById != null && (entityById.PositionComp.GetPosition() - base.PositionComp.GetPosition()).LengthSquared() > (double)(m_forcedTargetRange * m_forcedTargetRange))
				{
					MyHud.Notifications.Add(m_outOfRangeNotification);
				}
			}
		}

		private float GetMaxAmmoRange()
		{
			IMyGunObject<MyGunBase> myGunObject;
			if ((myGunObject = m_directionBlock as IMyGunObject<MyGunBase>) != null)
			{
				return myGunObject.GunBase.CurrentAmmoDefinition.MaxTrajectory;
			}
			MyCharacter myCharacter = MySession.Static.Players.GetControllingPlayer(this)?.Character;
			if (myCharacter != null)
			{
				return (float)myCharacter.TargetFocusComp.FocusSearchMaxDistance;
			}
			return MaxRange;
		}

		private void TurretCorrectnessChanged()
		{
			if (Sync.IsServer)
			{
				CheckNeedsUpdate();
				if (!m_isCorrectTurret)
				{
					IsAimingToolbarActive = false;
				}
				SetDetailedInfoDirty();
			}
			else
			{
				SetDetailedInfoDirty();
			}
		}

		private void ResetTargetingIgnoredGrids()
		{
			if (Sync.IsServer)
			{
				m_targetingSystem.ClearReverseRaycastIgnoredEntities();
				if (m_azimuthor != null && m_azimuthor.TopBlock != null)
				{
					MyCubeGrid cubeGrid = m_azimuthor.TopBlock.CubeGrid;
					m_targetingSystem.AddReverseRaycastIgnoredEntity(cubeGrid.EntityId, cubeGrid);
				}
				else if ((long)m_boundAzimuth != 0L)
				{
					m_resetIgnoredGrids |= true;
					base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				}
				if (m_elevator != null && m_elevator.TopBlock != null)
				{
					MyCubeGrid cubeGrid2 = m_elevator.TopBlock.CubeGrid;
					m_targetingSystem.AddReverseRaycastIgnoredEntity(cubeGrid2.EntityId, cubeGrid2);
				}
				else if ((long)m_boundElevation != 0L)
				{
					m_resetIgnoredGrids |= true;
					base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
				}
			}
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

		private void CheckTurretCorrectness()
		{
			if (Sync.IsServer)
			{
				ResetDirectionSource();
				if (Azimuthor == null && Elevator == null)
				{
					m_isCorrectTurret.Value = false;
				}
				else if (m_boundTools.Count <= 0 && m_camera == null)
				{
					m_isCorrectTurret.Value = false;
				}
				else if (!m_isCorrectTurret)
				{
					m_isCorrectTurret.Value = true;
				}
			}
		}

		private void ShootingRangeChanged()
		{
			m_searchingRange = m_shootingRange;
			if (Sync.IsServer && base.IsWorking && (bool)m_isCorrectTurret)
			{
				m_targetingSystem.CheckAndSelectNearTargetsParallel();
			}
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
			myResourceSinkComponent.Init(BlockDefinition.ResourceSinkGroup, BlockDefinition.PowerInputIdle, () => (!Enabled || !base.IsFunctional) ? 0f : base.ResourceSink.MaxRequiredInputByType(MyResourceDistributorComponent.ElectricityId), this);
			myResourceSinkComponent.IsPoweredChanged += PowerReceiver_IsPoweredChanged;
			base.ResourceSink = myResourceSinkComponent;
			base.Init(objectBuilder, cubeGrid);
			m_items = new List<ToolbarItem>(2);
			for (int i = 0; i < 2; i++)
			{
				m_items.Add(new ToolbarItem
				{
					EntityID = 0L
				});
			}
			m_toolbar = new MyToolbar(MyToolbarType.ButtonPanel, 2, 1);
			m_toolbar.DrawNumbers = false;
			MyObjectBuilder_TurretControlBlock myObjectBuilder_TurretControlBlock;
			if ((myObjectBuilder_TurretControlBlock = objectBuilder as MyObjectBuilder_TurretControlBlock) != null)
			{
				m_shootingRange.ValidateRange(0f, BlockDefinition.MaxRangeMeters);
				m_shootingRange.SetLocalValue(Math.Min(BlockDefinition.MaxRangeMeters, Math.Max(0f, myObjectBuilder_TurretControlBlock.Range)));
				if (myObjectBuilder_TurretControlBlock.AzimuthId > 0)
				{
					m_boundAzimuth.SetLocalValue(myObjectBuilder_TurretControlBlock.AzimuthId);
				}
				if (myObjectBuilder_TurretControlBlock.ElevationId > 0)
				{
					m_boundElevation.SetLocalValue(myObjectBuilder_TurretControlBlock.ElevationId);
				}
				if (myObjectBuilder_TurretControlBlock.CameraId > 0)
				{
					m_boundCamera.SetLocalValue(myObjectBuilder_TurretControlBlock.CameraId);
				}
				if (myObjectBuilder_TurretControlBlock.ToolIds.Count > 0)
				{
					foreach (long toolId in myObjectBuilder_TurretControlBlock.ToolIds)
					{
						m_boundTools.Add(toolId, null);
					}
				}
				m_velocityMultiplierAzimuth.ValidateRange(-1f, 1f);
				m_velocityMultiplierAzimuth.SetLocalValue(MyMath.Clamp(myObjectBuilder_TurretControlBlock.VelocityMultiplierAzimuth, -1f, 1f));
				m_velocityMultiplierElevation.ValidateRange(-1f, 1f);
				m_velocityMultiplierElevation.SetLocalValue(MyMath.Clamp(myObjectBuilder_TurretControlBlock.VelocityMultiplierElevation, -1f, 1f));
				m_savedPreviousControlledEntityId = myObjectBuilder_TurretControlBlock.PreviousControlledEntityId;
				TargetLocking = myObjectBuilder_TurretControlBlock.TargetLocking;
				m_targetingGroup.Value = myObjectBuilder_TurretControlBlock.TargetingGroup;
				m_angleDeviation.ValidateRange(0f, (float)Math.PI / 2f);
				m_angleDeviation.Value = myObjectBuilder_TurretControlBlock.AngleDeviation;
				m_areWeaponsMismatched.SetLocalValue(myObjectBuilder_TurretControlBlock.AreWeaponsMismatched);
				m_allowAI.Value = myObjectBuilder_TurretControlBlock.AllowAI;
				m_directionBlockId.SetLocalValue(myObjectBuilder_TurretControlBlock.DirectionBlockId);
				m_toolbar.Init(myObjectBuilder_TurretControlBlock.Toolbar, this);
				for (int j = 0; j < 2; j++)
				{
					MyToolbarItem itemAtIndex = m_toolbar.GetItemAtIndex(j);
					if (itemAtIndex != null)
					{
						m_items.RemoveAt(j);
						m_items.Insert(j, ToolbarItem.FromItem(itemAtIndex));
					}
				}
				m_toolbar.ItemChanged += Toolbar_ItemChanged;
				if (Sync.IsServer)
				{
					m_isCorrectTurret.SetLocalValue(newValue: false);
				}
				else
				{
					m_isCorrectTurret.SetLocalValue(myObjectBuilder_TurretControlBlock.IsCorrect);
				}
				TargetMeteors = myObjectBuilder_TurretControlBlock.Flags.TargetMeteors;
				TargetMissiles = myObjectBuilder_TurretControlBlock.Flags.TargetMissiles;
				TargetCharacters = myObjectBuilder_TurretControlBlock.Flags.TargetCharacters;
				TargetSmallGrids = myObjectBuilder_TurretControlBlock.Flags.TargetSmallGrids;
				TargetLargeGrids = myObjectBuilder_TurretControlBlock.Flags.TargetLargeGrids;
				TargetStations = myObjectBuilder_TurretControlBlock.Flags.TargetStations;
				TargetNeutrals = myObjectBuilder_TurretControlBlock.Flags.TargetNeutrals;
				TargetFriends = myObjectBuilder_TurretControlBlock.Flags.TargetFriends;
				TargetEnemies = myObjectBuilder_TurretControlBlock.Flags.TargetEnemies;
				SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
				base.ResourceSink.Update();
			}
			if (BlockDefinition != null)
			{
				m_maxRangeMeter = BlockDefinition.MaxRangeMeters;
			}
			m_boundAzimuth.ValueChanged += ValueChangedAzimuth;
			m_boundElevation.ValueChanged += ValueChangedElevation;
			m_boundCamera.ValueChanged += ValueChangedCamera;
			m_velocityMultiplierAzimuth.ValueChanged += ValueChangedVelocityAz;
			m_velocityMultiplierElevation.ValueChanged += ValueChangedVelocityEl;
			m_boundAzimuth.Validate = ValidateAzimuth;
			m_boundElevation.Validate = ValidateElevation;
			m_boundCamera.Validate = ValidateCamera;
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			m_initialize = true;
		}

		private void PowerReceiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		private void Toolbar_ItemChanged(MyToolbar self, MyToolbar.IndexArgs index, bool isGamepad)
		{
			if (m_syncing)
			{
				return;
			}
			ToolbarItem toolbarItem = ToolbarItem.FromItem(self.GetItemAtIndex(index.ItemIndex));
			ToolbarItem toolbarItem2 = m_items[index.ItemIndex];
			if ((toolbarItem.EntityID == 0L && toolbarItem2.EntityID == 0L) || (toolbarItem.EntityID != 0L && toolbarItem2.EntityID != 0L && toolbarItem.Equals(toolbarItem2)))
			{
				return;
			}
			m_items.RemoveAt(index.ItemIndex);
			m_items.Insert(index.ItemIndex, toolbarItem);
			MyMultiplayer.RaiseEvent(this, (MyTurretControlBlock x) => x.SendToolbarItemChanged, toolbarItem, index.ItemIndex);
			if (!m_shouldSetOtherToolbars)
			{
				return;
			}
			m_shouldSetOtherToolbars = false;
			foreach (MyToolbar openedToolbar in m_openedToolbars)
			{
				if (openedToolbar != self)
				{
					openedToolbar.SetItemAtIndex(index.ItemIndex, self.GetItemAtIndex(index.ItemIndex));
				}
			}
			m_shouldSetOtherToolbars = true;
		}

		private bool ValidateAzimuth(long newValue)
		{
			if (!Sync.IsServer || newValue == 0L)
			{
				return true;
			}
			MyMotorStator block = MyEntities.GetEntityById(newValue) as MyMotorStator;
			return ValidateBlock(block);
		}

		private bool ValidateElevation(long newValue)
		{
			if (!Sync.IsServer || newValue == 0L)
			{
				return true;
			}
			MyMotorStator block = MyEntities.GetEntityById(newValue) as MyMotorStator;
			return ValidateBlock(block);
		}

		private bool ValidateCamera(long newValue)
		{
			if (!Sync.IsServer || newValue == 0L)
			{
				return true;
			}
			MyCameraBlock block = MyEntities.GetEntityById(newValue) as MyCameraBlock;
			return ValidateBlock(block);
		}

		private void ValueChangedVelocityAz(SyncBase obj)
		{
			if ((float)m_velocityMultiplierAzimuth < -1f || (float)m_velocityMultiplierAzimuth > 1f)
			{
				m_velocityMultiplierAzimuth.Value = MyMath.Clamp(m_velocityMultiplierAzimuth, -1f, 1f);
			}
		}

		private void ValueChangedVelocityEl(SyncBase obj)
		{
			if ((float)m_velocityMultiplierElevation < -1f || (float)m_velocityMultiplierElevation > 1f)
			{
				m_velocityMultiplierElevation.Value = MyMath.Clamp(m_velocityMultiplierElevation, -1f, 1f);
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			if (base.CubeGrid.IsPreview || base.CubeGrid.Physics == null)
			{
				return;
			}
			if (m_initialize)
			{
				RecacheTools();
				RecacheAzimuth();
				RecacheElevation();
				RecacheCamera();
				if (m_savedPreviousControlledEntityId.HasValue)
				{
					MySession.Static.Players.UpdatePlayerControllers(base.EntityId);
					if (m_savedPreviousControlledEntityId.HasValue)
					{
						TryFindSavedEntity();
						m_savedPreviousControlledEntityId = null;
					}
				}
				if (Sync.IsServer)
				{
					CheckTurretCorrectness();
				}
				m_initialize = false;
			}
			if (m_resubscribeToLogicalGroup)
			{
				if (Sync.IsServer)
				{
					MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(base.CubeGrid);
					if (group != null)
					{
						group.GroupData.OnGridRemoved += GridRemovedFromGroupCallback;
					}
				}
				m_resubscribeToLogicalGroup = false;
			}
			if (m_recheckBlockConnections)
			{
				CheckBlockConnections();
				m_recheckBlockConnections = false;
			}
			if (m_resetIgnoredGrids)
			{
				m_resetIgnoredGrids = false;
				ResetTargetingIgnoredGrids();
			}
		}

		private void GridRemovedFromGroupCallback(IMyGridGroupData arg1, VRage.Game.ModAPI.IMyCubeGrid arg2, IMyGridGroupData arg3)
		{
			MyCubeGrid myCubeGrid = arg2 as MyCubeGrid;
			if (base.CubeGrid == myCubeGrid)
			{
				if (arg1 != null)
				{
					arg1.OnGridRemoved -= GridRemovedFromGroupCallback;
				}
				if (arg3 != null)
				{
					arg3.OnGridRemoved += GridRemovedFromGroupCallback;
				}
			}
			m_recheckBlockConnections = true;
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_TurretControlBlock myObjectBuilder_TurretControlBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_TurretControlBlock;
			myObjectBuilder_TurretControlBlock.AzimuthId = m_boundAzimuth.Value;
			myObjectBuilder_TurretControlBlock.ElevationId = m_boundElevation.Value;
			myObjectBuilder_TurretControlBlock.CameraId = m_boundCamera.Value;
			myObjectBuilder_TurretControlBlock.ToolIds = new MySerializableList<long>();
			foreach (KeyValuePair<long, MyFunctionalBlock> boundTool in m_boundTools)
			{
				myObjectBuilder_TurretControlBlock.ToolIds.Add(boundTool.Key);
			}
			if (PreviousControlledEntity != null)
			{
				myObjectBuilder_TurretControlBlock.PreviousControlledEntityId = PreviousControlledEntity.Entity.EntityId;
			}
			myObjectBuilder_TurretControlBlock.VelocityMultiplierAzimuth = m_velocityMultiplierAzimuth;
			myObjectBuilder_TurretControlBlock.VelocityMultiplierElevation = m_velocityMultiplierElevation;
			myObjectBuilder_TurretControlBlock.Range = m_shootingRange;
			myObjectBuilder_TurretControlBlock.IsCorrect = m_isCorrectTurret;
			myObjectBuilder_TurretControlBlock.TargetLocking = TargetLocking;
			myObjectBuilder_TurretControlBlock.AngleDeviation = m_angleDeviation.Value;
			myObjectBuilder_TurretControlBlock.DirectionBlockId = m_directionBlockId.Value;
			myObjectBuilder_TurretControlBlock.AreWeaponsMismatched = m_areWeaponsMismatched;
			myObjectBuilder_TurretControlBlock.AllowAI = m_allowAI;
			myObjectBuilder_TurretControlBlock.Toolbar = m_toolbar.GetObjectBuilder();
			myObjectBuilder_TurretControlBlock.Flags.TargetMeteors = TargetMeteors;
			myObjectBuilder_TurretControlBlock.Flags.TargetMissiles = TargetMissiles;
			myObjectBuilder_TurretControlBlock.Flags.TargetCharacters = TargetCharacters;
			myObjectBuilder_TurretControlBlock.Flags.TargetSmallGrids = TargetSmallGrids;
			myObjectBuilder_TurretControlBlock.Flags.TargetLargeGrids = TargetLargeGrids;
			myObjectBuilder_TurretControlBlock.Flags.TargetStations = TargetStations;
			myObjectBuilder_TurretControlBlock.Flags.TargetNeutrals = TargetNeutrals;
			myObjectBuilder_TurretControlBlock.Flags.TargetFriends = TargetFriends;
			myObjectBuilder_TurretControlBlock.Flags.TargetEnemies = TargetEnemies;
			myObjectBuilder_TurretControlBlock.TargetingGroup = m_targetingGroup.Value;
			return myObjectBuilder_TurretControlBlock;
		}

		private bool TryFindSavedEntity()
		{
			if (ControllerInfo.Controller != null && MyEntities.TryGetEntityById(m_savedPreviousControlledEntityId.Value, out var entity))
			{
				PreviousControlledEntity = (Sandbox.Game.Entities.IMyControllableEntity)entity;
				if (m_previousControlledEntity is MyCockpit)
				{
					m_cockpitPilot = (m_previousControlledEntity as MyCockpit).Pilot;
				}
				return true;
			}
			return false;
		}

		private void ValueChangedAzimuth(SyncBase obj)
		{
			Azimuthor = null;
			RecacheAzimuth();
			SetDetailedInfoDirty();
		}

		private void ValueChangedElevation(SyncBase obj)
		{
			Elevator = null;
			RecacheElevation();
			SetDetailedInfoDirty();
		}

		private void ValueChangedCamera(SyncBase obj)
		{
			Camera = null;
			RecacheCamera();
			if (Sync.IsServer && ((m_camera != null && m_directionSource < MyDirectionSource.Camera) || (m_camera == null && m_directionSource == MyDirectionSource.Camera)))
			{
				CheckTurretCorrectness();
			}
			SetDetailedInfoDirty();
		}

		private void RecacheAzimuth()
		{
			MyMotorStator myMotorStator = MyEntities.GetEntityById(m_boundAzimuth.Value) as MyMotorStator;
			if (myMotorStator != null)
			{
				Azimuthor = myMotorStator;
			}
		}

		private void RecacheElevation()
		{
			MyMotorStator myMotorStator = MyEntities.GetEntityById(m_boundElevation.Value) as MyMotorStator;
			if (myMotorStator != null)
			{
				Elevator = myMotorStator;
			}
		}

		private void RecacheCamera()
		{
			MyCameraBlock myCameraBlock = MyEntities.GetEntityById(m_boundCamera.Value) as MyCameraBlock;
			if (myCameraBlock != null)
			{
				Camera = myCameraBlock;
			}
		}

		private void RecacheTools()
		{
			List<Tuple<long, MyFunctionalBlock>> list = new List<Tuple<long, MyFunctionalBlock>>();
			List<long> list2 = new List<long>();
			foreach (long key in m_boundTools.Keys)
			{
				if (m_boundTools[key] == null)
				{
					MyFunctionalBlock myFunctionalBlock = MyEntities.GetEntityById(key) as MyFunctionalBlock;
					if (myFunctionalBlock != null)
					{
						list.Add(new Tuple<long, MyFunctionalBlock>(key, myFunctionalBlock));
					}
					else
					{
						list2.Add(key);
					}
				}
			}
			foreach (long item in list2)
			{
				m_boundTools.Remove(item);
			}
			foreach (Tuple<long, MyFunctionalBlock> item2 in list)
			{
				m_boundTools[item2.Item1] = item2.Item2;
				if (Sync.IsServer)
				{
					item2.Item2.RemovedFromScene += BlockRemovedTool;
					item2.Item2.OnClose += BlockRemovedTool;
				}
			}
		}

		private void BlockRemovedTool(MyEntity tool)
		{
			SyncToolUnselection(new List<long> { tool.EntityId });
			tool.RemovedFromScene -= BlockRemovedTool;
			tool.OnClose -= BlockRemovedTool;
			CheckTurretCorrectness();
		}

		private void CacheBlocks()
		{
			m_cachedToolBlocks.Clear();
			m_cachedCameraBlocks.Clear();
			m_cachedRotorBlocks.Clear();
			foreach (MyCubeBlock fatBlock in base.CubeGrid.GetFatBlocks())
			{
				CacheProcessBlock(fatBlock);
			}
			MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(base.CubeGrid);
			if (group == null)
			{
				return;
			}
			foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in group.Nodes)
			{
				if (node.NodeData == base.CubeGrid)
				{
					continue;
				}
				foreach (MyCubeBlock fatBlock2 in node.NodeData.GetFatBlocks())
				{
					CacheProcessBlock(fatBlock2);
				}
			}
		}

		private void CacheProcessBlock(MyCubeBlock block)
		{
			MyCameraBlock item;
			MyMotorStator item2;
			MyFunctionalBlock item3;
			if ((item = block as MyCameraBlock) != null)
			{
				m_cachedCameraBlocks.Add(item);
			}
			else if ((item2 = block as MyMotorStator) != null)
			{
				m_cachedRotorBlocks.Add(item2);
			}
			else if ((item3 = block as MyFunctionalBlock) != null && (block is IMyGunObject<MyToolBase> || (block is IMyGunObject<MyGunBase> && !(block is MyLargeTurretBase))))
			{
				m_cachedToolBlocks.Add(item3);
			}
		}

		protected override void OnStopWorking()
		{
			base.OnStopWorking();
			m_targetingSystem.ResetTarget();
			if (Sync.IsServer)
			{
				IsAimingToolbarActive = false;
				if (m_azimuthor != null)
				{
					m_azimuthor.TargetVelocity.Value = 0f;
				}
				if (m_elevator != null)
				{
					m_elevator.TargetVelocity.Value = 0f;
				}
				EndShoot(MyShootActionEnum.PrimaryAction);
			}
		}

		private void OnTargetFlagChanged()
		{
			MyTurretTargetFlags myTurretTargetFlags = TargetFlags & ~MyTurretTargetFlags.NotNeutrals;
			m_workingFlagCombination = myTurretTargetFlags != (MyTurretTargetFlags)0;
			m_targetingSystem.OnTargetFlagChanged();
		}

		protected override void Closing()
		{
			base.Closing();
			ReleaseControl();
			try
			{
				m_targetingSystem.FinishTasks();
			}
			catch
			{
			}
			m_targetingSystem.ResetTarget();
		}

		protected override void CreateTerminalControls()
		{
			if (MyTerminalControlFactory.AreControlsCreated<MyTurretControlBlock>())
			{
				return;
			}
			base.CreateTerminalControls();
			m_openedToolbars = new List<MyToolbar>();
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyTurretControlBlock>("Open Toolbar", MySpaceTexts.BlockPropertyTitle_SensorToolbarOpen, MySpaceTexts.BlockPropertyDescription_SensorToolbarOpen, delegate(MyTurretControlBlock self)
			{
				m_openedToolbars.Add(self.m_toolbar);
				if (MyGuiScreenToolbarConfigBase.Static == null)
				{
					m_shouldSetOtherToolbars = true;
					MyToolbarComponent.CurrentToolbar = self.m_toolbar;
					MyGuiScreenBase myGuiScreenBase = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, 0, self, null);
					MyToolbarComponent.AutoUpdate = false;
					myGuiScreenBase.Closed += delegate
					{
						MyToolbarComponent.AutoUpdate = true;
						m_openedToolbars.Clear();
					};
					MyGuiSandbox.AddScreen(myGuiScreenBase);
				}
			}));
			MyTerminalControlFactory.AddControl(new MyTerminalControlCombobox<MyTurretControlBlock>("RotorAzimuth", MySpaceTexts.BlockPropertyTitle_AssignRotorAzimuth, MySpaceTexts.Blank)
			{
				ComboBoxContentWithBlock = delegate(MyTurretControlBlock x, ICollection<MyTerminalControlComboBoxItem> list)
				{
					x.FillComboboxRotors(list);
				},
				Getter = (MyTurretControlBlock x) => x.m_boundAzimuth,
				Setter = delegate(MyTurretControlBlock x, long y)
				{
					x.m_boundAzimuth.Value = y;
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCombobox<MyTurretControlBlock>("RotorElevation", MySpaceTexts.BlockPropertyTitle_AssignRotorElevation, MySpaceTexts.Blank)
			{
				ComboBoxContentWithBlock = delegate(MyTurretControlBlock x, ICollection<MyTerminalControlComboBoxItem> list)
				{
					x.FillComboboxRotors(list);
				},
				Getter = (MyTurretControlBlock x) => x.m_boundElevation,
				Setter = delegate(MyTurretControlBlock x, long y)
				{
					x.m_boundElevation.Value = y;
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCombobox<MyTurretControlBlock>("CameraList", MySpaceTexts.BlockPropertyTitle_AssignedCamera, MySpaceTexts.Blank)
			{
				ComboBoxContentWithBlock = delegate(MyTurretControlBlock x, ICollection<MyTerminalControlComboBoxItem> list)
				{
					x.FillComboboxCameras(list);
				},
				Getter = (MyTurretControlBlock x) => x.m_boundCamera,
				Setter = delegate(MyTurretControlBlock x, long y)
				{
					x.m_boundCamera.Value = y;
				}
			});
			MyTerminalControlSlider<MyTurretControlBlock> myTerminalControlSlider = new MyTerminalControlSlider<MyTurretControlBlock>("MultiplierAz", MySpaceTexts.BlockPropertyTitle_MultiplierAzimuth, MySpaceTexts.BlockPropertyDescription_MultiplierAzimuth);
			myTerminalControlSlider.SetLimits(GetAzimuthSpeedMin, GetAzimuthSpeedMax);
			myTerminalControlSlider.DefaultValueGetter = (MyTurretControlBlock block) => 10f;
			myTerminalControlSlider.Getter = (MyTurretControlBlock x) => x.VelocityMultiplierAzimuthRpm;
			myTerminalControlSlider.Setter = delegate(MyTurretControlBlock x, float v)
			{
				x.VelocityMultiplierAzimuthRpm = v;
			};
			myTerminalControlSlider.Writer = delegate(MyTurretControlBlock x, StringBuilder result)
			{
				result.Append(MyValueFormatter.GetFormatedFloat(x.VelocityMultiplierAzimuthRpm, 2));
			};
			myTerminalControlSlider.EnableActions();
			MyTerminalControlFactory.AddControl(myTerminalControlSlider);
			MyTerminalControlSlider<MyTurretControlBlock> myTerminalControlSlider2 = new MyTerminalControlSlider<MyTurretControlBlock>("MultiplierEl", MySpaceTexts.BlockPropertyTitle_MultiplierElevation, MySpaceTexts.BlockPropertyDescription_MultiplierElevation);
			myTerminalControlSlider2.SetLimits(GetElevatorSpeedMin, GetElevatorSpeedMax);
			myTerminalControlSlider2.DefaultValueGetter = (MyTurretControlBlock block) => 10f;
			myTerminalControlSlider2.Getter = (MyTurretControlBlock x) => x.VelocityMultiplierElevationRpm;
			myTerminalControlSlider2.Setter = delegate(MyTurretControlBlock x, float v)
			{
				x.VelocityMultiplierElevationRpm = v;
			};
			myTerminalControlSlider2.Writer = delegate(MyTurretControlBlock x, StringBuilder result)
			{
				result.Append(MyValueFormatter.GetFormatedFloat(x.VelocityMultiplierElevationRpm, 2));
			};
			myTerminalControlSlider2.EnableActions();
			MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
			MyTerminalControlSlider<MyTurretControlBlock> myTerminalControlSlider3 = new MyTerminalControlSlider<MyTurretControlBlock>("AngleDeviation", MySpaceTexts.BlockPropertyTitle_AngleDeviation, MySpaceTexts.BlockPropertyDescription_AngleDeviation);
			myTerminalControlSlider3.SetLimits(0.05f, 90f);
			myTerminalControlSlider3.DefaultValueGetter = (MyTurretControlBlock block) => 5f;
			myTerminalControlSlider3.Getter = (MyTurretControlBlock x) => x.AngleDeviation;
			myTerminalControlSlider3.Setter = delegate(MyTurretControlBlock x, float v)
			{
				x.AngleDeviation = v;
			};
			myTerminalControlSlider3.Writer = delegate(MyTurretControlBlock x, StringBuilder result)
			{
				result.Append(MyValueFormatter.GetFormatedFloat(x.AngleDeviation, 2));
			};
			myTerminalControlSlider3.EnableActions();
			MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
			MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<MyTurretControlBlock>("ToolList", MySpaceTexts.BlockPropertyTitle_ToolList, MySpaceTexts.Blank, multiSelect: true)
			{
				ListContent = delegate(MyTurretControlBlock x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2, ICollection<MyGuiControlListbox.Item> focusedItem)
				{
					x.FillToolList(list1, list2);
				},
				ItemSelected = delegate(MyTurretControlBlock x, List<MyGuiControlListbox.Item> y)
				{
					x.ChangeToolsToSelect(y);
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyTurretControlBlock>("AddSelectedTool", MySpaceTexts.BlockPropertyTitle_AddTool, MySpaceTexts.Blank, delegate(MyTurretControlBlock x)
			{
				x.SendToolSelectionPressed();
			})
			{
				Visible = (MyTurretControlBlock x) => true,
				Enabled = (MyTurretControlBlock x) => true
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlListbox<MyTurretControlBlock>("SelectedToolsList", MySpaceTexts.BlockPropertyTitle_SelectedToolList, MySpaceTexts.Blank, multiSelect: true)
			{
				ItemSelected = delegate(MyTurretControlBlock x, List<MyGuiControlListbox.Item> y)
				{
					x.ChangeToolsToUnselect(y);
				},
				ListContent = delegate(MyTurretControlBlock x, ICollection<MyGuiControlListbox.Item> list1, ICollection<MyGuiControlListbox.Item> list2, ICollection<MyGuiControlListbox.Item> focusedItem)
				{
					x.FillSelectedToolList(list1, list2);
				}
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyTurretControlBlock>("RemoveSelectedTool", MySpaceTexts.BlockPropertyTitle_RemoveTool, MySpaceTexts.Blank, delegate(MyTurretControlBlock x)
			{
				x.SendToolUnselectionPressed();
			})
			{
				Visible = (MyTurretControlBlock x) => true,
				Enabled = (MyTurretControlBlock x) => true
			});
			MyTerminalControlOnOffSwitch<MyTurretControlBlock> obj = new MyTerminalControlOnOffSwitch<MyTurretControlBlock>("AI", MySpaceTexts.BlockPropertyTitle_EnableAI)
			{
				Getter = (MyTurretControlBlock x) => x.AIEnabled,
				Setter = delegate(MyTurretControlBlock x, bool v)
				{
					x.AIEnabled = v;
				},
				Visible = (MyTurretControlBlock t) => t.IsTurretTerminalVisible()
			};
			obj.EnableToggleAction();
			obj.EnableOnOffActions();
			MyTerminalControlFactory.AddControl(obj);
			InitializeTargetingSystem();
			m_targetingSystem.InjectTerminalControls(this, allowIdleMovement: false);
		}

		[Event(null, 1916)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void SendToolbarItemChanged(ToolbarItem sentItem, int index)
		{
			m_syncing = true;
			MyToolbarItem item = null;
			if (sentItem.EntityID != 0L)
			{
				item = ToolbarItem.ToItem(sentItem);
			}
			m_toolbar.SetItemAtIndex(index, item);
			m_syncing = false;
		}

		private static float GetAzimuthSpeedMax(MyTurretControlBlock block)
		{
			if (block.m_azimuthor != null)
			{
				return block.m_azimuthor.MaxRotorAngularVelocity * (30f / (float)Math.PI);
			}
			return 30f;
		}

		private static float GetAzimuthSpeedMin(MyTurretControlBlock block)
		{
			if (block.m_azimuthor != null)
			{
				return (0f - block.m_azimuthor.MaxRotorAngularVelocity) * (30f / (float)Math.PI);
			}
			return -30f;
		}

		private static float GetElevatorSpeedMax(MyTurretControlBlock block)
		{
			if (block.m_elevator != null)
			{
				return block.m_elevator.MaxRotorAngularVelocity * (30f / (float)Math.PI);
			}
			return 30f;
		}

		private static float GetElevatorSpeedMin(MyTurretControlBlock block)
		{
			if (block.m_elevator != null)
			{
				return (0f - block.m_elevator.MaxRotorAngularVelocity) * (30f / (float)Math.PI);
			}
			return -30f;
		}

		private void FillComboboxCameras(ICollection<MyTerminalControlComboBoxItem> items)
		{
			MyTerminalControlComboBoxItem item = new MyTerminalControlComboBoxItem
			{
				Key = 0L,
				Value = MyCommonTexts.ScreenGraphicsOptions_AntiAliasing_None
			};
			items.Add(item);
			foreach (MyCameraBlock cameraBlock in CameraBlocks)
			{
				item = new MyTerminalControlComboBoxItem
				{
					Key = cameraBlock.EntityId,
					Value = MyStringId.GetOrCompute(cameraBlock.CustomName.ToString())
				};
				items.Add(item);
			}
		}

		private void FillComboboxRotors(ICollection<MyTerminalControlComboBoxItem> items)
		{
			MyTerminalControlComboBoxItem item = new MyTerminalControlComboBoxItem
			{
				Key = 0L,
				Value = MyCommonTexts.ScreenGraphicsOptions_AntiAliasing_None
			};
			items.Add(item);
			foreach (MyMotorStator rotorBlock in RotorBlocks)
			{
				item = new MyTerminalControlComboBoxItem
				{
					Key = rotorBlock.EntityId,
					Value = MyStringId.GetOrCompute(rotorBlock.CustomName.ToString())
				};
				items.Add(item);
			}
		}

		private void CheckWeaponMissMatch()
		{
			if (m_boundTools.Count == 0 || m_directionSource != MyDirectionSource.Weapon)
			{
				m_areWeaponsMismatched.Value = false;
				return;
			}
			MyDefinitionId id = (m_directionBlock as IMyGunObject<MyGunBase>).GunBase.CurrentAmmoDefinition.Id;
			foreach (KeyValuePair<long, MyFunctionalBlock> boundTool in m_boundTools)
			{
				IMyGunObject<MyGunBase> myGunObject;
				if ((myGunObject = boundTool.Value as IMyGunObject<MyGunBase>) != null && id != myGunObject.GunBase.CurrentAmmoDefinition.Id)
				{
					m_areWeaponsMismatched.Value = true;
					return;
				}
			}
			m_areWeaponsMismatched.Value = false;
		}

		private bool ValidateBlock(MyFunctionalBlock block)
		{
			if (block == null)
			{
				return false;
			}
			if (!base.CubeGrid.IsInSameLogicalGroupAs(block.CubeGrid))
			{
				return false;
			}
			return true;
		}

		private void ResetDirectionSource()
		{
			if (Sync.IsServer)
			{
				if (m_boundTools.Count > 0)
				{
					m_directionBlock = GetSortedFirst(m_boundTools);
					m_directionSource = MyDirectionSource.Tool;
					m_directionBlockId.Value = m_directionBlock.EntityId;
				}
				else if (m_camera != null)
				{
					m_directionBlock = m_camera;
					m_directionSource = MyDirectionSource.Camera;
					m_directionBlockId.Value = m_directionBlock.EntityId;
				}
				else
				{
					m_directionSource = MyDirectionSource.None;
				}
			}
		}

		private MyCubeBlock GetSortedFirst(Dictionary<long, MyFunctionalBlock> tools)
		{
			if (tools.Count == 1)
			{
				return tools[tools.Keys.First()];
			}
			List<MyCubeBlock> list = new List<MyCubeBlock>();
			foreach (KeyValuePair<long, MyFunctionalBlock> tool in tools)
			{
				if (tool.Value != null)
				{
					list.Add(tool.Value);
				}
			}
			list.Sort(new BlockComparer());
			return list[0];
		}

		private void SendToolUnselectionPressed()
		{
			List<long> list = new List<long>();
			foreach (MyGuiControlListbox.Item item in m_selectedToolsToRemove)
			{
				long num = (long)item.UserData;
				if (m_boundTools.ContainsKey(num))
				{
					list.Add(num);
				}
			}
			if (list.Count > 0)
			{
				SyncToolUnselection(list);
			}
		}

		private void SendToolSelectionPressed()
		{
			List<long> list = new List<long>();
			foreach (MyGuiControlListbox.Item item in m_selectedToolsToAdd)
			{
				long num = (long)item.UserData;
				if (!m_boundTools.ContainsKey(num))
				{
					list.Add(num);
				}
			}
			if (list.Count > 0)
			{
				SyncToolSelection(list);
			}
		}

		private void SyncToolUnselection(List<long> toSync)
		{
			MyMultiplayer.RaiseEvent(this, (MyTurretControlBlock x) => ToolUnselectionRequest, toSync);
		}

		private void SyncToolSelection(List<long> toSync)
		{
			MyMultiplayer.RaiseEvent(this, (MyTurretControlBlock x) => ToolSelectionRequest, toSync);
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		[Event(null, 2111)]
		[Reliable]
		[Server]
		[Broadcast]
		private void ToolUnselectionRequest(List<long> toSync)
		{
			foreach (long item in toSync)
			{
				if (m_boundTools.ContainsKey(item))
				{
					m_boundTools.Remove(item);
				}
			}
			if (Sync.IsServer)
			{
				CheckTurretCorrectness();
			}
			RaisePropertiesChanged();
		}

		[Event(null, 2130)]
		[Reliable]
		[Server]
		[Broadcast]
		private void ToolSelectionRequest(List<long> toSync)
		{
			foreach (long item in toSync)
			{
				if (!m_boundTools.ContainsKey(item))
				{
					MyFunctionalBlock myFunctionalBlock = MyEntities.GetEntityById(item) as MyFunctionalBlock;
					if (!Sync.IsServer || ValidateBlock(myFunctionalBlock))
					{
						m_boundTools.Add(item, myFunctionalBlock);
					}
				}
			}
			if (Sync.IsServer)
			{
				if (m_isShooting)
				{
					m_shouldRefreshShooting = true;
				}
				CheckTurretCorrectness();
			}
			RaisePropertiesChanged();
		}

		public void FillToolList(ICollection<MyGuiControlListbox.Item> listBoxContent, ICollection<MyGuiControlListbox.Item> listBoxSelectedItems)
		{
			List<MyFunctionalBlock> toolBlocks = ToolBlocks;
			toolBlocks.SortNoAlloc((MyFunctionalBlock x, MyFunctionalBlock y) => string.Compare(x.DisplayNameText, y.DisplayNameText, StringComparison.InvariantCultureIgnoreCase));
			foreach (MyFunctionalBlock item2 in toolBlocks)
			{
				MyFunctionalBlock current;
				if ((current = item2) != null)
				{
					MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder().Append(MyStringId.GetOrCompute(current.CustomName.ToString())), null, null, current.EntityId);
					listBoxContent.Add(item);
				}
			}
		}

		public void FillSelectedToolList(ICollection<MyGuiControlListbox.Item> listBoxContent, ICollection<MyGuiControlListbox.Item> listBoxSelectedItems)
		{
			foreach (KeyValuePair<long, MyFunctionalBlock> boundTool in m_boundTools)
			{
				MyFunctionalBlock value;
				if ((value = boundTool.Value) != null)
				{
					MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder().Append(MyStringId.GetOrCompute(value.CustomName.ToString())), null, null, value.EntityId);
					listBoxContent.Add(item);
				}
			}
		}

		private void ChangeToolsToSelect(List<MyGuiControlListbox.Item> list)
		{
			m_selectedToolsToAdd.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				m_selectedToolsToAdd.Add(list[i]);
			}
		}

		private void ChangeToolsToUnselect(List<MyGuiControlListbox.Item> list)
		{
			m_selectedToolsToRemove.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				m_selectedToolsToRemove.Add(list[i]);
			}
		}

		public bool CanControl()
		{
			if (!base.IsWorking)
			{
				return false;
			}
			if (IsPlayerControlled)
			{
				return false;
			}
			MyCockpit myCockpit = MySession.Static.ControlledEntity as MyCockpit;
			if (myCockpit != null)
			{
				if (myCockpit is MyCryoChamber)
				{
					return false;
				}
				return MyAntennaSystem.Static.CheckConnection(myCockpit.CubeGrid, base.CubeGrid, myCockpit.ControllerInfo.Controller.Player);
			}
			MyCharacter myCharacter = MySession.Static.ControlledEntity as MyCharacter;
			if (myCharacter != null)
			{
				return MyAntennaSystem.Static.CheckConnection(myCharacter, base.CubeGrid, myCharacter.ControllerInfo.Controller.Player);
			}
			return false;
		}

		public void RequestControl()
		{
			if (MyFakes.ENABLE_TURRET_CONTROL && CanControl())
			{
				if (MyGuiScreenTerminal.IsOpen)
				{
					MyGuiScreenTerminal.Hide();
				}
				MySession.Static.GameFocusManager.Clear();
				MyMultiplayer.RaiseEvent(this, (MyTurretControlBlock x) => x.RequestUseMessage, UseActionEnum.Manipulate, MySession.Static.ControlledEntity.Entity.EntityId);
			}
		}

		[Event(null, 2257)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void RequestUseMessage(UseActionEnum useAction, long usedById)
		{
			MyEntity entity;
			bool num = MyEntities.TryGetEntityById<MyEntity>(usedById, out entity, allowClosed: false);
			Sandbox.Game.Entities.IMyControllableEntity user = entity as Sandbox.Game.Entities.IMyControllableEntity;
			UseActionResult useActionResult = UseActionResult.OK;
			if (num && (useActionResult = ((IMyUsableEntity)this).CanUse(useAction, user)) == UseActionResult.OK)
			{
				MyMultiplayer.RaiseEvent(this, (MyTurretControlBlock x) => x.UseSuccessCallback, useAction, usedById, useActionResult);
				UseSuccessCallback(useAction, usedById, useActionResult);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyTurretControlBlock x) => x.UseFailureCallback, useAction, usedById, useActionResult, MyEventContext.Current.Sender);
			}
		}

		[Event(null, 2279)]
		[Reliable]
		[Broadcast]
		private void UseSuccessCallback(UseActionEnum useAction, long usedById, UseActionResult useResult)
		{
			if (MyEntities.TryGetEntityById<MyEntity>(usedById, out MyEntity entity, allowClosed: false))
			{
				Sandbox.Game.Entities.IMyControllableEntity myControllableEntity = entity as Sandbox.Game.Entities.IMyControllableEntity;
				if (myControllableEntity != null)
				{
					MyRelationsBetweenPlayerAndBlock relations = MyRelationsBetweenPlayerAndBlock.NoOwnership;
					if (this != null && myControllableEntity.ControllerInfo.Controller != null)
					{
						relations = GetUserRelationToOwner(myControllableEntity.ControllerInfo.Controller.Player.Identity.IdentityId, MyRelationsBetweenPlayerAndBlock.NoOwnership);
					}
					if (relations.IsFriendly() || MySession.Static.AdminSettings.HasFlag(AdminSettingsEnum.UseTerminals))
					{
						UseSuccess(useAction, myControllableEntity);
					}
					else
					{
						UseFailed(useAction, useResult, myControllableEntity);
					}
				}
			}
			if (MySession.Static.ControlledEntity != null && MySession.Static.ControlledEntity.Entity == this)
			{
				BlockPossessionStarted();
			}
		}

		private void BlockPossessionStarted()
		{
			if (Sync.IsServer)
			{
				m_targetingSystem.CheckOtherTargets = false;
				if (m_azimuthor != null)
				{
					m_azimuthor.TargetVelocity.Value = 0f;
				}
				if (m_elevator != null)
				{
					m_elevator.TargetVelocity.Value = 0f;
				}
				EndShoot(MyShootActionEnum.PrimaryAction);
			}
			if (m_boundCamera.Value != 0L)
			{
				if (m_camera == null)
				{
					RecacheCamera();
				}
				if (m_camera != null)
				{
					m_possessedCamera = m_camera.EntityId;
					m_camera.RequestSetView();
				}
			}
		}

		private void BlockPossessionEnded()
		{
			if (m_possessedCamera.HasValue && base.CubeGrid.GridSystems.CameraSystem.CurrentCamera != null && base.CubeGrid.GridSystems.CameraSystem.CurrentCamera.EntityId == m_possessedCamera.Value)
			{
				base.CubeGrid.GridSystems.CameraSystem.ResetCamera();
			}
			if (Sync.IsServer)
			{
				m_targetingSystem.ResetTargetParams();
			}
		}

		private void AddPreviousControllerEvents()
		{
			m_previousControlledEntity.Entity.OnMarkForClose += Entity_OnPreviousMarkForClose;
			MyTerminalBlock myTerminalBlock = m_previousControlledEntity.Entity as MyTerminalBlock;
			if (myTerminalBlock != null)
			{
				myTerminalBlock.IsWorkingChanged += PreviousCubeBlock_IsWorkingChanged;
			}
		}

		private void RemovePreviousControllerEvents()
		{
			m_previousControlledEntity.Entity.OnMarkForClose -= Entity_OnPreviousMarkForClose;
			MyTerminalBlock myTerminalBlock = m_previousControlledEntity.Entity as MyTerminalBlock;
			if (myTerminalBlock != null)
			{
				myTerminalBlock.IsWorkingChanged -= PreviousCubeBlock_IsWorkingChanged;
			}
		}

		private void Entity_OnPreviousMarkForClose(MyEntity obj)
		{
			ReleaseControl(previousClosed: true);
		}

		private void PreviousCubeBlock_IsWorkingChanged(MyCubeBlock obj)
		{
			if (!obj.IsWorking && !obj.Closed && !obj.MarkedForClose)
			{
				ReleaseControl();
			}
		}

		public override void OnRegisteredToGridSystems()
		{
			base.OnRegisteredToGridSystems();
		}

		public override void OnUnregisteredFromGridSystems()
		{
			base.OnUnregisteredFromGridSystems();
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MyTurretControlBlock x) => x.RequestRelease, arg2: false);
			}
		}

		[Event(null, 2398)]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void RequestRelease(bool previousClosed)
		{
			ReleaseControl();
		}

		private void ReleaseControl(bool previousClosed = false)
		{
			if (!IsPlayerControlled)
			{
				return;
			}
			if (Sync.IsServer)
			{
				EndShoot(MyShootActionEnum.PrimaryAction);
			}
			MyCockpit myCockpit = PreviousControlledEntity as MyCockpit;
			if (myCockpit != null && (previousClosed || myCockpit.Pilot == null || myCockpit.MarkedForClose || myCockpit.Closed))
			{
				ReturnControl(m_cockpitPilot);
				return;
			}
			MyCharacter myCharacter = PreviousControlledEntity as MyCharacter;
			if (myCharacter != null)
			{
				myCharacter.CurrentRemoteControl = null;
			}
			ReturnControl(PreviousControlledEntity);
		}

		private void ReturnControl(Sandbox.Game.Entities.IMyControllableEntity nextControllableEntity)
		{
			if (ControllerInfo.Controller != null)
			{
				this.SwitchControl(nextControllableEntity);
				BlockPossessionEnded();
			}
			PreviousControlledEntity = null;
			CheckNeedsUpdate();
		}

		private void CheckNeedsUpdate()
		{
			if (ControllerInfo.Controller != null)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			}
			else
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
			}
			if ((bool)m_isCorrectTurret && ControllerInfo.Controller == null && AIEnabled)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			}
			else
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_FRAME;
			}
		}

		private void UseSuccess(UseActionEnum action, Sandbox.Game.Entities.IMyControllableEntity user)
		{
			AcquireControl(user);
		}

		private void AcquireControl(Sandbox.Game.Entities.IMyControllableEntity previousControlledEntity)
		{
			PreviousControlledEntity = previousControlledEntity;
			if (previousControlledEntity.ControllerInfo.Controller != null)
			{
				previousControlledEntity.SwitchControl(this);
			}
			MyShipController myShipController = PreviousControlledEntity as MyShipController;
			if (myShipController != null)
			{
				m_cockpitPilot = myShipController.Pilot;
				if (m_cockpitPilot != null)
				{
					m_cockpitPilot.CurrentRemoteControl = this;
				}
			}
			else
			{
				MyCharacter myCharacter = PreviousControlledEntity as MyCharacter;
				if (myCharacter != null)
				{
					myCharacter.CurrentRemoteControl = this;
				}
			}
			CheckNeedsUpdate();
		}

		private void UseFailed(UseActionEnum action, UseActionResult actionResult, Sandbox.Game.Entities.IMyControllableEntity user)
		{
			if (user != null && user.ControllerInfo.IsLocallyHumanControlled())
			{
				switch (actionResult)
				{
				case UseActionResult.UsedBySomeoneElse:
					MyHud.Notifications.Add(new MyHudNotification(MyCommonTexts.AlreadyUsedBySomebodyElse, 2500, "Red"));
					break;
				case UseActionResult.MissingDLC:
					MySession.Static.CheckDLCAndNotify(BlockDefinition);
					break;
				default:
					MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
					break;
				}
			}
		}

		[Event(null, 2518)]
		[Reliable]
		[Client]
		private void UseFailureCallback(UseActionEnum useAction, long usedById, UseActionResult useResult)
		{
			MyEntities.TryGetEntityById<MyEntity>(usedById, out MyEntity entity, allowClosed: false);
			Sandbox.Game.Entities.IMyControllableEntity user = entity as Sandbox.Game.Entities.IMyControllableEntity;
			UseFailed(useAction, useResult, user);
		}

		public void BeginShoot(MyShootActionEnum action)
		{
			if (action != 0)
			{
				return;
			}
			long num = 0L;
			foreach (KeyValuePair<long, MyFunctionalBlock> boundTool in m_boundTools)
			{
				if (boundTool.Value == null)
				{
					num = boundTool.Key;
					break;
				}
				MyUserControllableGun myUserControllableGun;
				if ((myUserControllableGun = boundTool.Value as MyUserControllableGun) != null)
				{
					myUserControllableGun.SetShooting(shooting: true);
				}
				else if (boundTool.Value != null)
				{
					boundTool.Value.Enabled = true;
				}
			}
			if (num == 0L)
			{
				return;
			}
			RecacheTools();
			foreach (KeyValuePair<long, MyFunctionalBlock> boundTool2 in m_boundTools)
			{
				if (boundTool2.Key == num)
				{
					num = 0L;
				}
				if (num == 0L)
				{
					MyUserControllableGun myUserControllableGun2;
					if ((myUserControllableGun2 = boundTool2.Value as MyUserControllableGun) != null)
					{
						myUserControllableGun2.SetShooting(shooting: true);
					}
					else if (boundTool2.Value != null)
					{
						boundTool2.Value.Enabled = true;
					}
				}
			}
		}

		public void EndShoot(MyShootActionEnum action)
		{
			foreach (KeyValuePair<long, MyFunctionalBlock> boundTool in m_boundTools)
			{
				MyUserControllableGun myUserControllableGun;
				if ((myUserControllableGun = boundTool.Value as MyUserControllableGun) != null)
				{
					myUserControllableGun.SetShooting(shooting: false);
				}
				else
				{
					boundTool.Value.Enabled = false;
				}
			}
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

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			MyEntity entityById = MyEntities.GetEntityById(m_directionBlockId);
			detailedInfo.Append(string.Format(MyTexts.GetString(MySpaceTexts.TurretControlBlock_Detail_Status)));
			if (m_boundTools.Count != 0 || (long)m_boundCamera > 0)
			{
				detailedInfo.Append(string.Format(MyTexts.GetString(MySpaceTexts.TurretControlBlock_Detail_Status_Norm), (entityById != null) ? entityById.DisplayNameText : MyTexts.GetString(MySpaceTexts.TurretControlBlock_Detail_Status_Def)));
			}
			if ((long)m_boundAzimuth <= 0 && (long)m_boundElevation <= 0)
			{
				detailedInfo.Append(MyTexts.GetString(MySpaceTexts.TurretControlBlock_Detail_Status_Error1));
			}
			if (m_boundTools.Count == 0 && (long)m_boundCamera <= 0)
			{
				detailedInfo.Append(MyTexts.GetString(MySpaceTexts.TurretControlBlock_Detail_Status_Error2));
			}
			if ((bool)m_areWeaponsMismatched)
			{
				detailedInfo.Append(MyTexts.GetString(MySpaceTexts.TurretControlBlock_Detail_Status_Warn));
			}
			if (!DISPLAY_EXTENDED_DETAILS)
			{
				return;
			}
			if (m_camera != null)
			{
				detailedInfo.Append(string.Format(MyTexts.GetString(MySpaceTexts.TurretControlBlock_Detail_Camera), m_camera.DisplayNameText));
			}
			if (m_azimuthor != null)
			{
				detailedInfo.Append(string.Format(MyTexts.GetString(MySpaceTexts.TurretControlBlock_Detail_Azi), m_azimuthor.DisplayNameText));
			}
			if (m_elevator != null)
			{
				detailedInfo.Append(string.Format(MyTexts.GetString(MySpaceTexts.TurretControlBlock_Detail_Elev), m_elevator.DisplayNameText));
			}
			if (m_boundTools.Count <= 0)
			{
				return;
			}
			detailedInfo.Append(MyTexts.GetString(MySpaceTexts.TurretControlBlock_Detail_Tool));
			foreach (KeyValuePair<long, MyFunctionalBlock> boundTool in m_boundTools)
			{
				if (boundTool.Value != null)
				{
					detailedInfo.Append(string.Format(MyTexts.GetString(MySpaceTexts.TurretControlBlock_Detail_Format), boundTool.Value.DisplayNameText));
				}
			}
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
			if (m_previousControlledEntity != null)
			{
				return m_previousControlledEntity.GetHeadMatrix(includeY, includeX, forceHeadAnim);
			}
			return MatrixD.Identity;
		}

		public void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			if (Azimuthor != null)
			{
				float num = rotationIndicator.Y / BlockDefinition.PlayerInputDivider;
				if (num != m_lastRotationHorizontal)
				{
					float num2 = m_velocityMultiplierAzimuth;
					float maxRotorAngularVelocity = Azimuthor.MaxRotorAngularVelocity;
					float num3 = MyMath.Clamp(num * num2, -1f, 1f);
					Azimuthor.TargetVelocity.Value = num3 * maxRotorAngularVelocity;
					m_lastRotationHorizontal = num;
				}
			}
			else if ((long)m_boundAzimuth != 0L)
			{
				RecacheAzimuth();
			}
			if (Elevator != null)
			{
				float num4 = rotationIndicator.X / BlockDefinition.PlayerInputDivider;
				if (num4 != m_lastRotationVertical)
				{
					float num5 = m_velocityMultiplierElevation;
					float maxRotorAngularVelocity2 = Elevator.MaxRotorAngularVelocity;
					float num6 = MyMath.Clamp(num4 * num5, -1f, 1f);
					Elevator.TargetVelocity.Value = num6 * maxRotorAngularVelocity2;
					m_lastRotationVertical = num4;
				}
			}
			else if ((long)m_boundElevation != 0L)
			{
				RecacheElevation();
			}
			MoveAndRotateSync(moveIndicator, rotationIndicator, rollIndicator);
		}

		public void MoveAndRotateSync(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			MoveIndicator = moveIndicator;
			RotationIndicator = rotationIndicator;
			RollIndicator = rollIndicator;
			if (ControllerInfo.Controller != null)
			{
				if (MoveIndicator == Vector3.Zero && RotationIndicator == Vector2.Zero && RollIndicator == 0f)
				{
					m_clientState.Value = null;
					return;
				}
				m_clientState.Value = new MyCockpitMoveState
				{
					Move = MoveIndicator,
					Rotation = RotationIndicator,
					Roll = RollIndicator
				};
			}
		}

		public void MoveAndRotateStopped()
		{
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			CheckBlockConnections();
		}

		public void CheckBlockConnections()
		{
			if (!Sync.IsServer)
			{
				return;
			}
			if ((long)m_boundAzimuth != 0L && (Azimuthor == null || Azimuthor.Closed || !base.CubeGrid.IsInSameLogicalGroupAs(Azimuthor.CubeGrid)))
			{
				m_boundAzimuth.Value = 0L;
			}
			if ((long)m_boundElevation != 0L && (Elevator == null || Elevator.Closed || !base.CubeGrid.IsInSameLogicalGroupAs(Elevator.CubeGrid)))
			{
				m_boundElevation.Value = 0L;
			}
			if ((long)m_boundCamera != 0L && (m_camera == null || m_camera.Closed || !base.CubeGrid.IsInSameLogicalGroupAs(m_camera.CubeGrid)))
			{
				m_boundCamera.Value = 0L;
			}
			List<long> list = new List<long>();
			foreach (KeyValuePair<long, MyFunctionalBlock> boundTool in m_boundTools)
			{
				MyFunctionalBlock value = boundTool.Value;
				if (value == null || value.Closed || !base.CubeGrid.IsInSameLogicalGroupAs(value.CubeGrid))
				{
					list.Add(boundTool.Key);
				}
			}
			if (list.Count > 0)
			{
				SyncToolUnselection(list);
			}
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			base.Render.IsVisible();
			if (!base.IsWorking || !Enabled)
			{
				return;
			}
			if (!IsPlayerControlled && (bool)m_isCorrectTurret)
			{
				CheckBeingStuck();
				UpdateAiWeapon();
			}
			if (m_azimuthor != null)
			{
				m_angleAzi = m_azimuthor.GetAngle();
			}
			if (m_elevator != null)
			{
				m_angleEle = m_elevator.GetAngle();
			}
			m_targetingSystem.UpdateVisibilityCache();
			if (IsControlled)
			{
				if (!IsInRangeAndPlayerHasAccess())
				{
					ReleaseControl();
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
			}
			else if (Sync.IsServer && !m_targetingSystem.TargetPrediction.IsTargetPositionManual && m_workingFlagCombination)
			{
				m_targetingSystem.CheckAndSelectNearTargetsParallel();
			}
		}

		private void CheckBeingStuck()
		{
			if (IsAimed)
			{
				m_stuckCounter = 0;
				m_stuckTarget = null;
				return;
			}
			bool? flag = null;
			bool? flag2 = null;
			if (m_azimuthor != null)
			{
				float num = Math.Abs(2f * (m_angleAzi - m_azimuthor.GetAngle()) / 60f);
				flag = ((!(num < STUCK_LIMIT_CHECK) || !(Math.Abs(m_azimuthor.TargetVelocity.Value) > num)) ? new bool?(false) : new bool?(true));
			}
			if (m_elevator != null)
			{
				float num2 = m_angleEle - m_elevator.GetAngle();
				flag2 = ((!(num2 < STUCK_LIMIT_CHECK) || !(Math.Abs(m_elevator.TargetVelocity.Value) > num2)) ? new bool?(false) : new bool?(true));
			}
			bool flag3 = true;
			if (flag.HasValue)
			{
				flag3 &= flag.Value;
			}
			if (flag2.HasValue)
			{
				flag3 &= flag2.Value;
			}
			if (Target != null && flag3)
			{
				if (m_stuckTarget == Target)
				{
					m_stuckCounter++;
					if ((float)m_stuckCounter >= STUCK_FRAME_LIMIT)
					{
						m_targetingSystem.BlacklistTarget(m_stuckTarget, BLACKLIST_TIME);
						m_stuckCounter = 0;
						m_targetingSystem.ResetTarget();
					}
				}
				else
				{
					m_stuckTarget = Target;
					m_stuckCounter = 0;
				}
			}
			else
			{
				m_stuckCounter = 0;
				m_stuckTarget = null;
			}
		}

		private bool IsInRangeAndPlayerHasAccess()
		{
			if (ControllerInfo.Controller == null)
			{
				return false;
			}
			MyTerminalBlock myTerminalBlock = PreviousControlledEntity as MyTerminalBlock;
			if (myTerminalBlock == null)
			{
				MyCharacter myCharacter = PreviousControlledEntity as MyCharacter;
				if (myCharacter != null)
				{
					return MyAntennaSystem.Static.CheckConnection(myCharacter, base.CubeGrid, ControllerInfo.Controller.Player);
				}
				return true;
			}
			MyCubeGrid cubeGrid = myTerminalBlock.SlimBlock.CubeGrid;
			return MyAntennaSystem.Static.CheckConnection(cubeGrid, base.CubeGrid, ControllerInfo.Controller.Player);
		}

		private void UpdateAiWeapon()
		{
			if (!Sync.IsServer)
			{
				return;
			}
			if (m_targetingSystem.GetTargetDistanceSquared() < (double)SearchRangeSquared || m_targetingSystem.TargetPrediction.IsTargetPositionManual)
			{
				bool flag = (Target != null || m_targetingSystem.TargetPrediction.IsTargetPositionManual) && (m_targetingSystem.IsTargetVisible(Target) || m_targetingSystem.TargetPrediction.IsTargetPositionManual) && RotationAndElevation();
				bool flag2 = m_targetingSystem.IsValidTarget(Target);
				UpdateShooting(flag && !m_targetingSystem.IsPotentialTarget && m_targetingSystem.IsTarget(Target) && flag2);
				if (!flag2)
				{
					m_targetingSystem.ResetTarget();
				}
				if (!flag && Target == null)
				{
					if (m_azimuthor != null)
					{
						m_azimuthor.TargetVelocity.Value = 0f;
					}
					if (m_elevator != null)
					{
						m_elevator.TargetVelocity.Value = 0f;
					}
					EndShoot(MyShootActionEnum.PrimaryAction);
				}
			}
			else
			{
				if (m_isShooting)
				{
					UpdateShooting(!m_targetingSystem.IsPotentialTarget);
				}
				if (Target == null)
				{
					if (m_azimuthor != null)
					{
						m_azimuthor.TargetVelocity.Value = 0f;
					}
					if (m_elevator != null)
					{
						m_elevator.TargetVelocity.Value = 0f;
					}
					EndShoot(MyShootActionEnum.PrimaryAction);
				}
			}
			if (IsAimed && Target != null)
			{
				IsAimingToolbarActive = true;
			}
			else
			{
				IsAimingToolbarActive = false;
			}
		}

		private void UpdateShooting(bool shouldShoot)
		{
			if (m_shouldRefreshShooting && shouldShoot && m_isShooting)
			{
				BeginShoot(MyShootActionEnum.PrimaryAction);
			}
			if (shouldShoot != m_isShooting)
			{
				m_isShooting = shouldShoot;
				if (shouldShoot)
				{
					BeginShoot(MyShootActionEnum.PrimaryAction);
				}
				else
				{
					EndShoot(MyShootActionEnum.PrimaryAction);
				}
			}
		}

		public bool RotationAndElevation()
		{
			Vector3 vector = Vector3.Zero;
			if (Target != null)
			{
				bool usePrediction = m_targetingSystem.TargetPrediction.UsePrediction;
				m_targetingSystem.TargetPrediction.UsePrediction = m_targetingSystem.TargetPrediction.IsFastPrediction;
				Vector3D aimPosition = m_targetingSystem.AimPosition;
				m_targetingSystem.TargetPrediction.UsePrediction = usePrediction;
				vector = LookAt(aimPosition);
			}
			SetRotorAngleAndVelocity(Azimuthor, vector.Y, m_velocityMultiplierAzimuth, out var _);
			SetRotorAngleAndVelocity(Elevator, vector.X, m_velocityMultiplierElevation, out var _);
			float num = 0f;
			if (Target != null)
			{
				num = Vector3.Dot(ShootDirection, Vector3.Normalize((Vector3)(m_targetingSystem.AimPosition - ShootOrigin)));
			}
			if ((double)num < Math.Cos((float)m_angleDeviation))
			{
				IsAimed = false;
			}
			else
			{
				IsAimed = true;
			}
			return IsAimed;
		}

		private float SetRotorAngleAndVelocity(MyMotorStator rotor, float targetAngle, float speedMultiplier, out float angleDifference)
		{
			if (rotor == null)
			{
				angleDifference = float.MaxValue;
				return 0f;
			}
			float num = MathHelper.WrapAngle(rotor.GetAngle());
			_ = rotor.MaxRotorAngularVelocity;
			float num2 = rotor.MaxRotorAngularVelocity * Math.Abs(speedMultiplier);
			float num3 = 0f;
			float value;
			float value2;
			if (targetAngle < num)
			{
				value = MathHelper.WrapAngle(targetAngle - num);
				value2 = MathHelper.WrapAngle(targetAngle + (float)Math.PI * 2f - num);
			}
			else
			{
				value = MathHelper.WrapAngle(targetAngle + (float)Math.PI * 2f - num);
				value2 = MathHelper.WrapAngle(targetAngle - num);
			}
			if (Math.Abs(value) < Math.Abs(value2))
			{
				angleDifference = Math.Abs(value);
				num3 = Math.Sign(value);
			}
			else
			{
				angleDifference = Math.Abs(value2);
				num3 = Math.Sign(value2);
			}
			float num4 = 1f;
			float num5 = num2 / 60f;
			if (angleDifference < AIM_SLOWDOWN_THRESHOLD * num5)
			{
				num4 = MyMath.Clamp(angleDifference / num5, 0f, 1f);
			}
			float num6 = num2 * num3 * num4;
			rotor.TargetVelocity.Value = num6;
			return num6;
		}

		public void Use()
		{
			MyGuiAudio.PlaySound(MyGuiSounds.HudUse);
			MyMultiplayer.RaiseEvent(this, (MyTurretControlBlock x) => x.sync_ControlledEntity_Used);
		}

		[Event(null, 3140)]
		[Reliable]
		[Broadcast]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void sync_ControlledEntity_Used()
		{
			ReleaseControl();
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

		public void ShowInventory()
		{
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
		}

		public void SwitchLights()
		{
		}

		public void SwitchLandingGears()
		{
		}

		public void SwitchHandbrake()
		{
		}

		public void SwitchReactors()
		{
		}

		public void SwitchReactorsLocal()
		{
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

		public UseActionResult CanUse(UseActionEnum actionEnum, Sandbox.Game.Entities.IMyControllableEntity user)
		{
			if (base.IsWorking)
			{
				if (IsPlayerControlled)
				{
					return UseActionResult.UsedBySomeoneElse;
				}
				return UseActionResult.OK;
			}
			return UseActionResult.AccessDenied;
		}

		public void RemoveUsers(bool local)
		{
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
			Vector3 vector = ShootDirection;
			Vector3D shootOrigin = ShootOrigin;
			if (object.Equals(shootOrigin, Target))
			{
				return new Vector3((Elevator != null) ? Elevator.GetAngle() : 0f, (Azimuthor != null) ? Azimuthor.GetAngle() : 0f, 0f);
			}
			Vector3 vector2 = Vector3D.Normalize(target - shootOrigin);
			Vector3 result = new Vector3(0f, 0f, 0f);
			if (Azimuthor != null)
			{
				_ = Azimuthor.MaxRotorAngularVelocity;
				Vector3 vector3 = Azimuthor.PositionComp.WorldMatrix.Up;
				Azimuthor.PositionComp.GetPosition();
				if (Math.Abs(Vector3D.Dot(vector, vector3)) > 0.9999)
				{
					Azimuthor.TargetVelocity.Value = 0f;
				}
				else
				{
					Vector3 vector4 = Vector3.Normalize(vector - vector3 * Vector3.Dot(vector3, vector));
					Vector3 vector5 = Vector3.Normalize(vector2 - vector3 * Vector3.Dot(vector3, vector2));
					Vector3 vector6 = Vector3D.Cross(vector3, vector4);
					float num = ((!(Vector3.Dot(vector5, vector6) > 0f)) ? 1f : (-1f));
					float num2 = MyMath.Clamp(Vector3.Dot(vector5, vector4), -1f, 1f);
					float num3 = num * (float)Math.Acos(num2);
					float angle = Azimuthor.GetAngle();
					result.Y = MathHelper.WrapAngle(angle + num3);
				}
			}
			if (Elevator != null)
			{
				_ = Elevator.MaxRotorAngularVelocity;
				Vector3 vector7 = Elevator.PositionComp.WorldMatrix.Up;
				Elevator.PositionComp.GetPosition();
				if (!(Math.Abs(Vector3D.Dot(vector, vector7)) > 0.9999))
				{
					Vector3 vector8 = Vector3.Normalize(vector - vector7 * Vector3.Dot(vector7, vector));
					Vector3 vector9 = Vector3.Normalize(vector2 - vector7 * Vector3.Dot(vector7, vector2));
					Vector3 vector10 = Vector3D.Cross(vector7, vector8);
					float num4 = ((!(Vector3.Dot(vector9, vector10) > 0f)) ? 1f : (-1f));
					float num5 = MyMath.Clamp(Vector3.Dot(vector9, vector8), -1f, 1f);
					float num6 = num4 * (float)Math.Acos(num5);
					float angle2 = Elevator.GetAngle();
					result.X = MathHelper.WrapAngle(angle2 + num6);
				}
			}
			return result;
		}

		public bool IsTargetInView(Vector3D predPos)
		{
			if (!m_isCorrectTurret)
			{
				return false;
			}
			Vector3 lookAtPositionEuler = LookAt(predPos);
			return IsInRange(ref lookAtPositionEuler);
		}

		public bool IsInRange(ref Vector3 lookAtPositionEuler)
		{
			return true;
		}

		public MyRelationsBetweenPlayerAndBlock GetUserRelationToOwner(long targetIidentityId)
		{
			return GetUserRelationToOwner(targetIidentityId, MyRelationsBetweenPlayerAndBlock.NoOwnership);
		}

		public bool IsTurretTerminalVisible()
		{
			return m_isCorrectTurret;
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
			if (m_directionBlock != null)
			{
				return m_directionBlock.PositionComp.WorldMatrix;
			}
			return base.PositionComp.WorldMatrix;
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
			return ShootOrigin;
		}

		public void SetTargetingMode(MyTargetingGroupDefinition definition)
		{
			m_targetingGroup.Value = ((definition == null) ? MyStringHash.NullOrEmpty : MyStringHash.GetOrCompute(definition.Id.SubtypeName));
		}

		public void ForgetTarget()
		{
			MyMultiplayer.RaiseEvent(this, (MyTurretControlBlock x) => x.ForgetTargetServer);
		}

		[Event(null, 3452)]
		[Reliable]
		[Server]
		public void ForgetTargetServer()
		{
			m_lockedTarget.Value = 0L;
		}

		public void CopyTarget()
		{
			if (MySession.Static.LocalCharacter == null)
			{
				return;
			}
			MyShipController myShipController = MySession.Static.LocalCharacter.Parent as MyShipController;
			if (myShipController == null && MySession.Static.ControlledEntity != null)
			{
				myShipController = MySession.Static.ControlledEntity.Entity as MyShipController;
			}
			MyTargetLockingComponent targetLockingComp = MySession.Static.LocalCharacter.TargetLockingComp;
			if (targetLockingComp == null)
			{
				return;
			}
			if (targetLockingComp.Target == null)
			{
				MyHud.Notifications.Add(m_noTargetNotification);
				return;
			}
			if (!targetLockingComp.IsTargetLocked)
			{
				MyHud.Notifications.Add(m_lockingInProgressNotification);
				return;
			}
			MyMultiplayer.RaiseEvent(this, (MyTurretControlBlock x) => x.CopyTargetServer, MySession.Static.LocalCharacter.EntityId, myShipController.EntityId);
		}

		[Event(null, 3489)]
		[Reliable]
		[Server]
		public void CopyTargetServer(long characterEntityId, long cockpitEntityId)
		{
			MyCharacter myCharacter = MyEntities.GetEntityById(characterEntityId) as MyCharacter;
			MyShipController myShipController = MyEntities.GetEntityById(cockpitEntityId) as MyShipController;
			if (myCharacter == null || myShipController == null || myCharacter.GetPlayerIdentityId() != MySession.Static.Players.TryGetIdentityId(MyEventContext.Current.Sender.Value) || myShipController.Pilot == null || myShipController.Pilot != myCharacter)
			{
				return;
			}
			MyTargetLockingComponent targetLockingComp = myCharacter.TargetLockingComp;
			if (targetLockingComp != null)
			{
				MyCubeGrid target = targetLockingComp.Target;
				if (target != null && targetLockingComp.IsTargetLocked)
				{
					m_lockedTarget.Value = target.EntityId;
				}
			}
		}

		public void SetTarget(VRage.ModAPI.IMyEntity entity)
		{
			MyEntity entity2;
			if ((entity2 = entity as MyEntity) != null)
			{
				m_targetingSystem.ForceTarget(entity2, usePrediction: false);
			}
		}

		public void TrackTarget(VRage.ModAPI.IMyEntity entity)
		{
			MyEntity entity2;
			if ((entity2 = entity as MyEntity) != null)
			{
				m_targetingSystem.ForceTarget(entity2, usePrediction: true);
			}
		}

		public MyDetectedEntityInfo GetTargetedEntity()
		{
			return MyDetectedEntityInfoHelper.Create(Target, base.OwnerId, Target?.PositionComp.WorldAABB.Center);
		}

		public List<string> GetTargetingGroups()
		{
			List<MyTargetingGroupDefinition> targetingGroupDefinitions = MyDefinitionManager.Static.GetTargetingGroupDefinitions();
			List<string> list = new List<string>();
			foreach (MyTargetingGroupDefinition item in targetingGroupDefinitions)
			{
				list.Add(item.Id.SubtypeName);
			}
			return list;
		}

		public string GetTargetingGroup()
		{
			string @string = m_targetingGroup.Value.String;
			if (string.IsNullOrEmpty(@string))
			{
				return null;
			}
			return @string;
		}

		public void SetTargetingGroup(string groupSubtypeId)
		{
			m_targetingGroup.Value = MyStringHash.Get(groupSubtypeId);
		}

		public void GetTools(List<Sandbox.ModAPI.Ingame.IMyFunctionalBlock> tools)
		{
			if (tools == null)
			{
				return;
			}
			tools.Clear();
			foreach (KeyValuePair<long, MyFunctionalBlock> boundTool in m_boundTools)
			{
				tools.Add(boundTool.Value);
			}
		}

		public void AddTools(List<Sandbox.ModAPI.Ingame.IMyFunctionalBlock> tools)
		{
			if (tools == null)
			{
				return;
			}
			List<long> list = new List<long>();
			foreach (Sandbox.ModAPI.Ingame.IMyFunctionalBlock tool in tools)
			{
				if (tool != null && !m_boundTools.ContainsKey(tool.EntityId))
				{
					list.Add(tool.EntityId);
				}
			}
			if (list.Count > 0)
			{
				SyncToolSelection(list);
			}
		}

		public void RemoveTools(List<Sandbox.ModAPI.Ingame.IMyFunctionalBlock> tools)
		{
			if (tools == null)
			{
				return;
			}
			List<long> list = new List<long>();
			foreach (Sandbox.ModAPI.Ingame.IMyFunctionalBlock tool in tools)
			{
				if (tool != null && m_boundTools.ContainsKey(tool.EntityId))
				{
					list.Add(tool.EntityId);
				}
			}
			if (list.Count > 0)
			{
				SyncToolUnselection(list);
			}
		}

		public void AddTool(Sandbox.ModAPI.Ingame.IMyFunctionalBlock tool)
		{
			if (tool != null && !m_boundTools.ContainsKey(tool.EntityId))
			{
				SyncToolSelection(new List<long> { tool.EntityId });
			}
		}

		public void RemoveTool(Sandbox.ModAPI.Ingame.IMyFunctionalBlock tool)
		{
			if (tool != null && m_boundTools.ContainsKey(tool.EntityId))
			{
				SyncToolUnselection(new List<long> { tool.EntityId });
			}
		}

		public void ClearTools()
		{
			List<long> list = new List<long>();
			foreach (KeyValuePair<long, MyFunctionalBlock> boundTool in m_boundTools)
			{
				list.Add(boundTool.Key);
			}
			if (list.Count > 0)
			{
				SyncToolUnselection(list);
			}
		}

		public Vector3 GetShootDirection()
		{
			return ShootDirection;
		}

		public Sandbox.ModAPI.Ingame.IMyTerminalBlock GetDirectionSource()
		{
			return m_directionBlock as Sandbox.ModAPI.Ingame.IMyTerminalBlock;
		}
	}
}
