using System;
<<<<<<< HEAD
using System.Collections.Generic;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Electricity;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Replication.ClientStates;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.SessionComponents.Clipboard;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
<<<<<<< HEAD
=======
using SpaceEngineers.Game.ModAPI.Ingame;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage;
using VRage.Audio;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.Gui;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Interfaces;
using VRage.Game.ObjectBuilders.Components;
using VRage.Groups;
using VRage.Input;
using VRage.Library.Utils;
using VRage.ModAPI;
using VRage.Network;
using VRage.ObjectBuilders;
using VRage.Serialization;
using VRage.Sync;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Entities
{
	[MyTerminalInterface(new Type[]
	{
		typeof(Sandbox.ModAPI.IMyShipController),
		typeof(Sandbox.ModAPI.Ingame.IMyShipController)
	})]
	public class MyShipController : MyTerminalBlock, IMyControllableEntity, VRage.Game.ModAPI.Interfaces.IMyControllableEntity, IMyRechargeSocketOwner, Sandbox.ModAPI.IMyShipController, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, VRage.ModAPI.IMyEntity, Sandbox.ModAPI.Ingame.IMyTerminalBlock, Sandbox.ModAPI.Ingame.IMyShipController, IMyTargetingCapableBlock, IMyShootOrigin, IMyPilotable
	{
		protected sealed class SyncMoveIndicator_003C_003EVRageMath_Vector3 : ICallSite<MyShipController, Vector3, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipController @this, in Vector3 moveIndicator, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SyncMoveIndicator(moveIndicator);
			}
		}

		protected sealed class sync_ControlledEntity_Used_003C_003E : ICallSite<MyShipController, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipController @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.sync_ControlledEntity_Used();
			}
		}

		protected sealed class OnSwitchHelmet_003C_003E : ICallSite<MyShipController, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipController @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSwitchHelmet();
			}
		}

		protected sealed class SwitchToWeaponMessage_003C_003ESystem_Nullable_00601_003CVRage_ObjectBuilders_SerializableDefinitionId_003E_0023VRage_ObjectBuilders_MyObjectBuilder_EntityBase_0023System_Int64 : ICallSite<MyShipController, SerializableDefinitionId?, MyObjectBuilder_EntityBase, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipController @this, in SerializableDefinitionId? weapon, in MyObjectBuilder_EntityBase weaponObjectBuilder, in long weaponEntityId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SwitchToWeaponMessage(weapon, weaponObjectBuilder, weaponEntityId);
			}
		}

		protected sealed class OnSwitchToWeaponFailure_003C_003ESystem_Nullable_00601_003CVRage_ObjectBuilders_SerializableDefinitionId_003E_0023VRage_ObjectBuilders_MyObjectBuilder_EntityBase_0023System_Int64 : ICallSite<MyShipController, SerializableDefinitionId?, MyObjectBuilder_EntityBase, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipController @this, in SerializableDefinitionId? weapon, in MyObjectBuilder_EntityBase weaponObjectBuilder, in long weaponEntityId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSwitchToWeaponFailure(weapon, weaponObjectBuilder, weaponEntityId);
			}
		}

		protected sealed class OnSwitchToWeaponSuccess_003C_003ESystem_Nullable_00601_003CVRage_ObjectBuilders_SerializableDefinitionId_003E_0023VRage_ObjectBuilders_MyObjectBuilder_EntityBase_0023System_Int64 : ICallSite<MyShipController, SerializableDefinitionId?, MyObjectBuilder_EntityBase, long, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipController @this, in SerializableDefinitionId? weapon, in MyObjectBuilder_EntityBase weaponObjectBuilder, in long weaponEntityId, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSwitchToWeaponSuccess(weapon, weaponObjectBuilder, weaponEntityId);
			}
		}

		protected sealed class OnSwitchAmmoMagazineRequest_003C_003E : ICallSite<MyShipController, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipController @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSwitchAmmoMagazineRequest();
			}
		}

		protected sealed class OnSwitchAmmoMagazineSuccess_003C_003E : ICallSite<MyShipController, DBNull, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipController @this, in DBNull arg1, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.OnSwitchAmmoMagazineSuccess();
			}
		}

		protected sealed class ShootBeginCallback_003C_003EVRage_Game_ModAPI_MyShootActionEnum : ICallSite<MyShipController, MyShootActionEnum, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipController @this, in MyShootActionEnum action, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ShootBeginCallback(action);
			}
		}

		protected sealed class ShootEndCallback_003C_003EVRage_Game_ModAPI_MyShootActionEnum : ICallSite<MyShipController, MyShootActionEnum, DBNull, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipController @this, in MyShootActionEnum action, in DBNull arg2, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.ShootEndCallback(action);
			}
		}

		protected sealed class SendToolbarItemRemoved_003C_003ESystem_Int32_0023System_Boolean : ICallSite<MyShipController, int, bool, DBNull, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipController @this, in int index, in bool isGamepad, in DBNull arg3, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SendToolbarItemRemoved(index, isGamepad);
			}
		}

		protected sealed class SendToolbarItemChanged_003C_003EVRage_Game_MyObjectBuilder_ToolbarItem_0023System_Int32_0023System_Boolean : ICallSite<MyShipController, MyObjectBuilder_ToolbarItem, int, bool, DBNull, DBNull, DBNull>
		{
			public sealed override void Invoke(in MyShipController @this, in MyObjectBuilder_ToolbarItem sentItem, in int index, in bool isGamepad, in DBNull arg4, in DBNull arg5, in DBNull arg6)
			{
				@this.SendToolbarItemChanged(sentItem, index, isGamepad);
			}
		}

		protected class m_controlThrusters_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType controlThrusters;
				ISyncType result = (controlThrusters = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyShipController)P_0).m_controlThrusters = (Sync<bool, SyncDirection.BothWays>)controlThrusters;
				return result;
			}
		}

		protected class m_controlWheels_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType controlWheels;
				ISyncType result = (controlWheels = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyShipController)P_0).m_controlWheels = (Sync<bool, SyncDirection.BothWays>)controlWheels;
				return result;
			}
		}

		protected class m_controlGyros_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType controlGyros;
				ISyncType result = (controlGyros = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyShipController)P_0).m_controlGyros = (Sync<bool, SyncDirection.BothWays>)controlGyros;
				return result;
			}
		}

		protected class m_clientState_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType clientState;
				ISyncType result = (clientState = new Sync<MyCockpitMoveState?, SyncDirection.BothWays>(P_1, P_2));
				((MyShipController)P_0).m_clientState = (Sync<MyCockpitMoveState?, SyncDirection.BothWays>)clientState;
				return result;
			}
		}

		protected class m_isMainCockpit_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isMainCockpit;
				ISyncType result = (isMainCockpit = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyShipController)P_0).m_isMainCockpit = (Sync<bool, SyncDirection.BothWays>)isMainCockpit;
				return result;
			}
		}

		protected class m_isTargetLockingEnabled_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType isTargetLockingEnabled;
				ISyncType result = (isTargetLockingEnabled = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyShipController)P_0).m_isTargetLockingEnabled = (Sync<bool, SyncDirection.BothWays>)isTargetLockingEnabled;
				return result;
			}
		}

		protected class m_horizonIndicatorEnabled_003C_003ESyncComposer : ISyncComposer
		{
			public sealed override ISyncType Compose(object P_0, int P_1, ISerializerInfo P_2)
			{
				ISyncType horizonIndicatorEnabled;
				ISyncType result = (horizonIndicatorEnabled = new Sync<bool, SyncDirection.BothWays>(P_1, P_2));
				((MyShipController)P_0).m_horizonIndicatorEnabled = (Sync<bool, SyncDirection.BothWays>)horizonIndicatorEnabled;
				return result;
			}
		}

		private class Sandbox_Game_Entities_MyShipController_003C_003EActor : IActivator, IActivator<MyShipController>
		{
			private sealed override object CreateInstance()
			{
				return new MyShipController();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyShipController CreateInstance()
			{
				return new MyShipController();
			}

			MyShipController IActivator<MyShipController>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyGridGyroSystem GridGyroSystem;

		public MyGridSelectionSystem GridSelectionSystem;

		public MyGridReflectorLightSystem GridReflectorLights;

		private readonly Sync<bool, SyncDirection.BothWays> m_controlThrusters;

		private readonly Sync<bool, SyncDirection.BothWays> m_controlWheels;

		private readonly Sync<bool, SyncDirection.BothWays> m_controlGyros;

		private readonly Sync<MyCockpitMoveState?, SyncDirection.BothWays> m_clientState;

		private bool m_reactorsSwitched = true;

		private bool m_mainCockpitOverwritten;

		protected MyRechargeSocket m_rechargeSocket;

		private MyHudNotification m_notificationLeave;

		private MyHudNotification m_notificationTerminal;

		private MyHudNotification m_landingGearsNotification;

		private MyHudNotification m_noWeaponNotification;

		private MyHudNotification m_weaponSelectedNotification;

		private MyHudNotification m_outOfAmmoNotification;

		private MyHudNotification m_weaponNotWorkingNotification;

		private MyHudNotification m_noControlNotification;

		private MyHudNotification m_connectorsNotification;

		private MyDLCs.MyDLC m_dlcNotificationDisplayed;

		protected bool m_enableFirstPerson;

		protected bool m_enableShipControl = true;

		protected bool m_enableBuilderCockpit;

		private static readonly float RollControlMultiplier = 0.2f;

		private bool m_forcedFPS;

		private MyDefinitionId? m_selectedGunId;

		private MyToolbar m_toolbar;

		private MyToolbar m_buildToolbar;

		private MyToolbar m_onLockedToolbar;

		public bool BuildingMode;

		[Obsolete]
		public bool hasPower;

		private readonly CachingList<MyGroupControlSystem> m_controlSystems = new CachingList<MyGroupControlSystem>();

		protected MyEntity3DSoundEmitter m_useSoundEmitter;

		protected MySoundPair m_baseIdleSound;

		protected MySoundPair GetOutOfCockpitSound = MySoundPair.Empty;

		protected MySoundPair GetInCockpitSound = MySoundPair.Empty;

		private Vector3 m_lastMoveIndicator;

		/// <summary>
		/// Raycaster used for showing block info when active.
		/// </summary>
		private MyCasterComponent m_raycaster;

		private int m_switchWeaponCounter;

		private readonly bool[] m_isShooting;

		private static bool m_shouldSetOtherToolbars;

		private bool m_syncing;

		public MyCharacter.MyTargetData TargetData;

		protected MyCharacter m_lastPilot;

		private bool m_isControlled;

		private readonly MyControllerInfo m_info = new MyControllerInfo();

		protected bool m_singleWeaponMode;

		protected Vector3 m_headLocalPosition;

		private readonly Sync<bool, SyncDirection.BothWays> m_isMainCockpit;

		private readonly Sync<bool, SyncDirection.BothWays> m_isTargetLockingEnabled;

		private readonly Sync<bool, SyncDirection.BothWays> m_horizonIndicatorEnabled;

		public MyResourceDistributorComponent GridResourceDistributor
		{
			get
			{
				if (base.CubeGrid == null)
				{
					return null;
				}
				return base.CubeGrid.GridSystems.ResourceDistributor;
			}
		}

		public MyGridWheelSystem GridWheels
		{
			get
			{
				if (base.CubeGrid == null)
				{
					return null;
				}
				return base.CubeGrid.GridSystems.WheelSystem;
			}
		}

		public MyEntityThrustComponent EntityThrustComponent
		{
			get
			{
				if (base.CubeGrid == null)
				{
					return null;
				}
				return base.CubeGrid.Components.Get<MyEntityThrustComponent>();
			}
		}

		protected virtual MyStringId LeaveNotificationHintText => MySpaceTexts.NotificationHintLeaveCockpit;

		public bool EnableShipControl => m_enableShipControl;

		public bool PlayDefaultUseSound => GetInCockpitSound == MySoundPair.Empty;

		private Vector3 MoveIndicator { get; set; }

		private Vector2 RotationIndicator { get; set; }

		private float RollIndicator { get; set; }

		public MyToolbar Toolbar
		{
			get
			{
				if (BuildingMode)
				{
					return m_buildToolbar;
				}
				return m_toolbar;
			}
		}

		public MyToolbar OnLockedToolbar => m_onLockedToolbar;

		private bool IsWaitingForWeaponSwitch => m_switchWeaponCounter != 0;

		public bool HasWheels
		{
			get
			{
				if (ControlWheels)
				{
					return GridWheels.WheelCount > 0;
				}
				return false;
			}
		}

		public MyGroups<MyCubeGrid, MyGridPhysicalGroupData> ControlGroup => MyCubeGridGroups.Static.Physical;

		public Vector3D ShootOrigin
		{
			get
			{
				if (!CanActiveToolShoot())
				{
					return base.PositionComp.WorldMatrixRef.Translation;
				}
				return GetActiveToolPosition();
			}
		}

		public virtual MyCharacter Pilot => null;

		protected virtual ControllerPriority Priority => ControllerPriority.Primary;

		public virtual bool CanHavePreviousControlledEntity => false;

		public virtual IMyControllableEntity PreviousControlledEntity
		{
			get
			{
				return null;
			}
			protected set
			{
			}
		}

		public bool PrimaryLookaround => !m_enableShipControl;

		public bool NeedsPerFrameUpdate
		{
			get
			{
				if (base.CubeGrid.GridSystems.ControlSystem != null)
				{
					return base.CubeGrid.GridSystems.ControlSystem.GetShipController() == this;
				}
				return false;
			}
		}

		public virtual bool ForceFirstPersonCamera
		{
			get
			{
				if (m_forcedFPS)
				{
					return m_enableFirstPerson;
				}
				return false;
			}
			set
			{
				if (m_forcedFPS != value)
				{
					m_forcedFPS = value;
					UpdateCameraAfterChange(resetHeadLocalAngle: false);
				}
			}
		}

		public virtual bool EnableFirstPersonView
		{
			get
			{
				return m_enableFirstPerson;
			}
			set
			{
				m_enableFirstPerson = value;
			}
		}

		public MySlimBlock RaycasterHitBlock => m_raycaster?.HitBlock;

		public MyEntity TopGrid => base.Parent;

		public MyEntity IsUsing => null;

		public override Vector3D LocationForHudMarker => base.LocationForHudMarker + 0.65 * (double)base.CubeGrid.GridSize * (double)BlockDefinition.Size.Y * base.PositionComp.WorldMatrixRef.Up;

		public new MyShipControllerDefinition BlockDefinition => base.BlockDefinition as MyShipControllerDefinition;

		public bool ControlThrusters
		{
			get
			{
				return m_controlThrusters;
			}
			set
			{
				m_controlThrusters.Value = value;
			}
		}

		public bool ControlWheels
		{
			get
			{
				return m_controlWheels;
			}
			set
			{
				m_controlWheels.Value = value;
			}
		}

		public bool ControlGyros
		{
			get
			{
				return m_controlGyros;
			}
			set
			{
				m_controlGyros.Value = value;
			}
		}

		public MyEntity Entity => this;

		public MyControllerInfo ControllerInfo => m_info;

		MyRechargeSocket IMyRechargeSocketOwner.RechargeSocket => m_rechargeSocket;

		public bool SingleWeaponMode
		{
			get
			{
				return m_singleWeaponMode;
			}
			private set
			{
				if (m_singleWeaponMode != value)
				{
					m_singleWeaponMode = value;
					if (m_selectedGunId.HasValue)
					{
						SwitchToWeapon(m_selectedGunId.Value);
					}
					else
					{
						SwitchToWeapon(null);
					}
				}
			}
		}

		public bool IsMainCockpit
		{
			get
			{
				return m_isMainCockpit;
			}
			set
			{
				m_isMainCockpit.Value = value;
			}
		}

		public bool IsTargetLockingEnabledValue
		{
			get
			{
				return m_isTargetLockingEnabled;
			}
			set
			{
				m_isTargetLockingEnabled.Value = value;
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyShipController.IsMainCockpit
		{
			get
			{
				return IsMainCockpit;
			}
			set
			{
				if (IsMainCockpitFree() && CanBeMainCockpit())
				{
					IsMainCockpit = value;
				}
			}
		}

		public bool HorizonIndicatorEnabled
		{
			get
			{
				if ((bool)m_horizonIndicatorEnabled)
				{
					return CanHaveHorizon();
				}
				return false;
			}
			set
			{
				m_horizonIndicatorEnabled.Value = value;
			}
		}

		public virtual MyToolbarType ToolbarType
		{
			get
			{
				if (!m_enableShipControl)
				{
					return MyToolbarType.Seat;
				}
				return MyToolbarType.Ship;
			}
		}

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ForceFirstPersonCamera
		{
			get
			{
				return ForceFirstPersonCamera;
			}
			set
			{
				ForceFirstPersonCamera = value;
			}
		}

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledThrusts => false;

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledDamping
		{
			get
			{
				if (EntityThrustComponent != null)
				{
					return EntityThrustComponent.DampenersEnabled;
				}
				return false;
			}
		}

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledLights => GridReflectorLights.ReflectorsEnabled == MyMultipleEnabledEnum.AllEnabled;

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledLeadingGears
		{
			get
			{
				MyMultipleEnabledEnum locked = base.CubeGrid.GridSystems.LandingSystem.Locked;
				if (locked != MyMultipleEnabledEnum.Mixed)
				{
					return locked == MyMultipleEnabledEnum.AllEnabled;
				}
				return true;
			}
		}

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledReactors => base.CubeGrid.IsPowered;

		bool IMyControllableEntity.EnabledBroadcasting => false;

		bool VRage.Game.ModAPI.Interfaces.IMyControllableEntity.EnabledHelmet => false;

		public virtual float HeadLocalXAngle { get; set; }

		public virtual float HeadLocalYAngle { get; set; }

		bool Sandbox.ModAPI.Ingame.IMyShipController.IsUnderControl => ControllerInfo.Controller != null;

		bool Sandbox.ModAPI.Ingame.IMyShipController.ControlWheels
		{
			get
			{
				return ControlWheels;
			}
			set
			{
				if (m_enableShipControl && GridWheels.WheelCount > 0 && IsMainCockpitFree())
				{
					ControlWheels = value;
				}
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyShipController.ControlThrusters
		{
			get
			{
				return ControlThrusters;
			}
			set
			{
				if (m_enableShipControl && IsMainCockpitFree())
				{
					ControlThrusters = value;
				}
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyShipController.HandBrake
		{
			get
			{
				return base.CubeGrid.GridSystems.WheelSystem.HandBrake;
			}
			set
			{
				if (m_enableShipControl && GridWheels.WheelCount > 0 && IsMainCockpitFree() && base.CubeGrid.GridSystems.WheelSystem.HandBrake != value)
				{
					SwitchHandbrake();
				}
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyShipController.DampenersOverride
		{
			get
			{
				if (EntityThrustComponent != null)
				{
					return EntityThrustComponent.DampenersEnabled;
				}
				return false;
			}
			set
			{
				if (m_enableShipControl)
				{
					base.CubeGrid.EnableDampingInternal(value, updateProxy: true);
				}
			}
		}

		Vector3D Sandbox.ModAPI.Ingame.IMyShipController.CenterOfMass => base.CubeGrid.Physics.CenterOfMassWorld;

		public MyStringId ControlContext => MySpaceBindingCreator.CX_SPACESHIP;

		public MyStringId AuxiliaryContext
		{
			get
			{
				if (MyCubeBuilder.Static.IsActivated)
				{
					return MySpaceBindingCreator.AX_BUILD;
				}
				if (MySessionComponentVoxelHand.Static.Enabled)
				{
					return MySpaceBindingCreator.AX_VOXEL;
				}
				if (MyClipboardComponent.Static.IsActive)
				{
					return MySpaceBindingCreator.AX_CLIPBOARD;
				}
				return MySpaceBindingCreator.AX_ACTIONS;
			}
		}

		public MyEntity RelativeDampeningEntity
		{
			get
			{
				return base.CubeGrid.GridSystems.ControlSystem.RelativeDampeningEntity;
			}
			set
			{
				base.CubeGrid.GridSystems.ControlSystem.RelativeDampeningEntity = value;
			}
		}

		public bool IsDefault3rdView => BlockDefinition.IsDefault3rdView;
<<<<<<< HEAD

		public MyDefinitionBase GetAmmoDefinition => GetSelectedAmmoDefinitionFromCubeBlock();

		public virtual bool CanHavePreviousCameraEntity => false;

		public virtual VRage.ModAPI.IMyEntity GetPreviousCameraEntity => null;

		public float MaxShootRange
		{
			get
			{
				MyAmmoDefinition selectedAmmoDefinitionFromCubeBlock = GetSelectedAmmoDefinitionFromCubeBlock();
				if (selectedAmmoDefinitionFromCubeBlock != null)
				{
					return selectedAmmoDefinitionFromCubeBlock.MaxTrajectory;
				}
				MyTargetFocusComponent targetFocusComp;
				if (Pilot != null && (targetFocusComp = Pilot.TargetFocusComp) != null)
				{
					return (float)targetFocusComp.FocusSearchMaxDistance;
				}
				return 0f;
			}
		}
=======

		VRage.ModAPI.IMyEntity VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Entity => Entity;

		public Vector3 LastMotionIndicator { get; set; }

		public Vector3 LastRotationIndicator { get; set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		VRage.ModAPI.IMyEntity VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Entity => Entity;

		public Vector3 LastMotionIndicator { get; set; }

		public Vector3 LastRotationIndicator { get; set; }

		IMyControllerInfo VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ControllerInfo => ControllerInfo;

		bool Sandbox.ModAPI.Ingame.IMyShipController.CanControlShip => EnableShipControl;

		bool Sandbox.ModAPI.Ingame.IMyShipController.HasWheels => GridWheels.WheelCount > 0;

		bool Sandbox.ModAPI.Ingame.IMyShipController.ShowHorizonIndicator
		{
			get
			{
				return HorizonIndicatorEnabled;
			}
			set
			{
				if (CanHaveHorizon())
				{
					HorizonIndicatorEnabled = value;
				}
			}
		}

		Vector3 Sandbox.ModAPI.Ingame.IMyShipController.MoveIndicator => MoveIndicator;

		Vector2 Sandbox.ModAPI.Ingame.IMyShipController.RotationIndicator => RotationIndicator;

		float Sandbox.ModAPI.Ingame.IMyShipController.RollIndicator => RollIndicator;

		bool Sandbox.ModAPI.IMyShipController.HasFirstPersonCamera => EnableFirstPersonView;

		IMyCharacter Sandbox.ModAPI.IMyShipController.Pilot => Pilot;

		IMyCharacter Sandbox.ModAPI.IMyShipController.LastPilot => m_lastPilot;

		bool Sandbox.ModAPI.IMyShipController.IsShooting => IsShooting();

		protected bool IsShooting(MyShootActionEnum action)
		{
			return m_isShooting[(uint)action];
		}

		public bool IsShooting()
		{
			MyShootActionEnum[] values = MyEnum<MyShootActionEnum>.Values;
			foreach (MyShootActionEnum myShootActionEnum in values)
			{
				if (m_isShooting[(uint)myShootActionEnum])
				{
					return true;
				}
			}
			return false;
		}

		public MyShipController()
		{
			CreateTerminalControls();
			m_isShooting = new bool[(uint)(MyEnum<MyShootActionEnum>.Range.Max + 1)];
			ControllerInfo.ControlAcquired += OnControlAcquired;
			ControllerInfo.ControlReleased += OnControlReleased;
			GridSelectionSystem = new MyGridSelectionSystem(this);
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
<<<<<<< HEAD
				m_useSoundEmitter = new MyEntity3DSoundEmitter(this, useStaticList: true);
=======
				m_soundEmitter = new MyEntity3DSoundEmitter(this, useStaticList: true);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			m_isMainCockpit.ValueChanged += delegate
			{
				MainCockpitChanged();
			};
			m_clientState.ValueChanged += ClientStateOnValueChanged;
			m_isTargetLockingEnabled.ValueChanged += OnTargetLockingEnabledChanged;
		}

		private void ClientStateOnValueChanged(SyncBase obj)
		{
			if (!Sync.IsServer || MyEventContext.Current.IsLocallyInvoked || (Pilot != null && Pilot.GetPlayerId(out var playerId) && playerId.SteamId == MyEventContext.Current.Sender.Value))
			{
				if (m_clientState.Value.HasValue)
				{
					MyCockpitMoveState value = m_clientState.Value.Value;
					MoveAndRotate(value.Move, value.Rotation, value.Roll);
				}
				else
				{
					MoveAndRotate(Vector3.Zero, Vector2.Zero, 0f);
				}
			}
		}

		protected override void CreateTerminalControls()
		{
			if (!MyTerminalControlFactory.AreControlsCreated<MyShipController>())
			{
				base.CreateTerminalControls();
				InitControlsStatic();
			}
		}

		private static void InitControlsStatic()
		{
			if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT)
			{
				MyTerminalControlCheckbox<MyShipController> obj = new MyTerminalControlCheckbox<MyShipController>("ControlThrusters", MySpaceTexts.TerminalControlPanel_Cockpit_ControlThrusters, MySpaceTexts.TerminalControlPanel_Cockpit_ControlThrusters)
				{
					Getter = (MyShipController x) => x.ControlThrusters,
					Setter = delegate(MyShipController x, bool v)
					{
						x.ControlThrusters = v;
					},
					Visible = (MyShipController x) => x.m_enableShipControl,
					Enabled = (MyShipController x) => x.IsMainCockpitFree()
				};
				MyTerminalAction<MyShipController> myTerminalAction = obj.EnableAction();
				if (myTerminalAction != null)
				{
					myTerminalAction.Enabled = (MyShipController x) => x.m_enableShipControl;
				}
				MyTerminalControlFactory.AddControl(obj);
				MyTerminalControlCheckbox<MyShipController> obj2 = new MyTerminalControlCheckbox<MyShipController>("ControlWheels", MySpaceTexts.TerminalControlPanel_Cockpit_ControlWheels, MySpaceTexts.TerminalControlPanel_Cockpit_ControlWheels)
				{
					Getter = (MyShipController x) => x.ControlWheels,
					Setter = delegate(MyShipController x, bool v)
					{
						x.ControlWheels = v;
					},
					Visible = (MyShipController x) => x.m_enableShipControl,
					Enabled = (MyShipController x) => x.GridWheels.WheelCount > 0 && x.IsMainCockpitFree()
				};
				myTerminalAction = obj2.EnableAction();
				if (myTerminalAction != null)
				{
					myTerminalAction.Enabled = (MyShipController x) => x.m_enableShipControl;
				}
				MyTerminalControlFactory.AddControl(obj2);
				MyTerminalControlCheckbox<MyShipController> obj3 = new MyTerminalControlCheckbox<MyShipController>("ControlGyros", MySpaceTexts.TerminalControlPanel_Cockpit_ControlGyros, MySpaceTexts.TerminalControlPanel_Cockpit_ControlGyros_Tooltip)
				{
					Getter = (MyShipController x) => x.ControlGyros,
					Setter = delegate(MyShipController x, bool v)
					{
						x.ControlGyros = v;
					},
					Visible = (MyShipController x) => x.m_enableShipControl,
					Enabled = (MyShipController x) => x.IsMainCockpitFree()
				};
				myTerminalAction = obj3.EnableAction();
				if (myTerminalAction != null)
				{
					myTerminalAction.Enabled = (MyShipController x) => x.m_enableShipControl;
				}
				MyTerminalControlFactory.AddControl(obj3);
				MyTerminalControlCheckbox<MyShipController> obj4 = new MyTerminalControlCheckbox<MyShipController>("HandBrake", MySpaceTexts.TerminalControlPanel_Cockpit_Handbrake, MySpaceTexts.ControlMenuItemLabel_Handbrake)
				{
<<<<<<< HEAD
					Getter = (MyShipController x) => x.CubeGrid.GridSystems.WheelSystem.IsParked,
=======
					Getter = (MyShipController x) => x.CubeGrid.GridSystems.WheelSystem.HandBrake,
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					Setter = delegate(MyShipController x, bool v)
					{
						x.SwitchHandbrake();
					},
					Visible = (MyShipController x) => x.m_enableShipControl,
					Enabled = (MyShipController x) => x.GridWheels.WheelCount > 0 && x.IsMainCockpitFree()
				};
				myTerminalAction = obj4.EnableAction();
				if (myTerminalAction != null)
				{
					myTerminalAction.Enabled = (MyShipController x) => x.m_enableShipControl;
				}
				MyTerminalControlFactory.AddControl(obj4);
				MyTerminalControlCheckbox<MyShipController> obj5 = new MyTerminalControlCheckbox<MyShipController>("Park", MySpaceTexts.TerminalControlPanel_Cockpit_Park, MySpaceTexts.ControlMenuItemLabel_Park)
				{
					Getter = (MyShipController x) => x.CubeGrid.IsParked,
					Setter = delegate(MyShipController x, bool v)
					{
						x.SwitchParkedStatus();
					},
					Visible = (MyShipController x) => x.m_enableShipControl,
					Enabled = (MyShipController x) => x.GridWheels.WheelCount > 0 && x.IsMainCockpitFree()
				};
				myTerminalAction = obj5.EnableAction();
				if (myTerminalAction != null)
				{
					myTerminalAction.Enabled = (MyShipController x) => x.m_enableShipControl;
				}
				MyTerminalControlFactory.AddControl(obj5);
			}
			if (MyFakes.ENABLE_DAMPENERS_OVERRIDE)
			{
				MyTerminalControlCheckbox<MyShipController> obj6 = new MyTerminalControlCheckbox<MyShipController>("DampenersOverride", MySpaceTexts.ControlName_InertialDampeners, MySpaceTexts.ControlName_InertialDampeners)
				{
					Getter = (MyShipController x) => x.EntityThrustComponent != null && x.EntityThrustComponent.DampenersEnabled,
					Setter = delegate(MyShipController x, bool v)
					{
						x.CubeGrid.EnableDampingInternal(v, updateProxy: true);
					},
					Visible = (MyShipController x) => x.m_enableShipControl
				};
				MyTerminalAction<MyShipController> myTerminalAction2 = obj6.EnableAction();
				if (myTerminalAction2 != null)
				{
					myTerminalAction2.Enabled = (MyShipController x) => x.m_enableShipControl;
				}
<<<<<<< HEAD
				obj6.Enabled = (MyShipController x) => x.IsMainCockpitFree();
				MyTerminalControlFactory.AddControl(obj6);
=======
				obj5.Enabled = (MyShipController x) => x.IsMainCockpitFree();
				MyTerminalControlFactory.AddControl(obj5);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			MyTerminalControlCheckbox<MyShipController> obj7 = new MyTerminalControlCheckbox<MyShipController>("HorizonIndicator", MySpaceTexts.TerminalControlPanel_Cockpit_HorizonIndicator, MySpaceTexts.TerminalControlPanel_Cockpit_HorizonIndicator)
			{
				Getter = (MyShipController x) => x.HorizonIndicatorEnabled,
				Setter = delegate(MyShipController x, bool v)
				{
					x.HorizonIndicatorEnabled = v;
				},
				Enabled = (MyShipController x) => true,
				Visible = (MyShipController x) => x.CanHaveHorizon()
			};
			obj7.EnableAction();
			MyTerminalControlFactory.AddControl(obj7);
			MyTerminalControlCheckbox<MyShipController> obj8 = new MyTerminalControlCheckbox<MyShipController>("MainCockpit", MySpaceTexts.TerminalControlPanel_Cockpit_MainCockpit, MySpaceTexts.TerminalControlPanel_Cockpit_MainCockpit)
			{
				Getter = (MyShipController x) => x.IsMainCockpit,
				Setter = delegate(MyShipController x, bool v)
				{
					x.IsMainCockpit = v;
				},
				Enabled = (MyShipController x) => x.IsMainCockpitFree(),
				Visible = (MyShipController x) => x.CanBeMainCockpit()
			};
			obj8.EnableAction();
			MyTerminalControlFactory.AddControl(obj8);
			MyTerminalControlCheckbox<MyShipController> obj9 = new MyTerminalControlCheckbox<MyShipController>("TargetLocking", MySpaceTexts.TerminalControlPanel_Cockpit_EnableTargetLocking, MySpaceTexts.TerminalControlPanel_Cockpit_EnableTargetLocking)
			{
				Getter = (MyShipController x) => x.IsTargetLockingEnabledValue,
				Setter = delegate(MyShipController x, bool v)
				{
					x.IsTargetLockingEnabledValue = v;
				},
				Enabled = (MyShipController x) => true,
				Visible = (MyShipController x) => x is MyCockpit && x.BlockDefinition.TargetLockingEnabled
			};
			obj9.EnableAction();
			MyTerminalControlFactory.AddControl(obj9);
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MyShipController>("OpenToolbar", MySpaceTexts.BlockPropertyTitle_CockpitOnLockedToolbarOpen, MySpaceTexts.BlockPropertyTitle_CockpitOnLockedToolbarOpen, delegate(MyShipController self)
			{
				if (MyGuiScreenToolbarConfigBase.Static == null)
				{
					m_shouldSetOtherToolbars = true;
					MyToolbarComponent.CurrentToolbar = self.OnLockedToolbar;
					MyGuiScreenBase myGuiScreenBase = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, 0, self, null);
					MyToolbarComponent.AutoUpdate = false;
					myGuiScreenBase.Closed += delegate
					{
						MyToolbarComponent.AutoUpdate = true;
					};
					MyGuiSandbox.AddScreen(myGuiScreenBase);
				}
			})
			{
				Tooltip = MySpaceTexts.BlockPropertyTooltip_CockpitOnLockedToolbarOpen
			});
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			base.SyncFlag = true;
			base.Init(objectBuilder, cubeGrid);
			MyDefinitionManager.Static.GetCubeBlockDefinition(objectBuilder.GetId());
			m_enableFirstPerson = BlockDefinition.EnableFirstPerson || !MySession.Static.Settings.Enable3rdPersonView;
			m_enableShipControl = BlockDefinition.EnableShipControl;
			m_enableBuilderCockpit = BlockDefinition.EnableBuilderCockpit;
			m_rechargeSocket = new MyRechargeSocket();
			MyObjectBuilder_ShipController myObjectBuilder_ShipController = (MyObjectBuilder_ShipController)objectBuilder;
			m_selectedGunId = myObjectBuilder_ShipController.SelectedGunId;
			m_controlThrusters.SetLocalValue(myObjectBuilder_ShipController.ControlThrusters);
			m_controlWheels.SetLocalValue(myObjectBuilder_ShipController.ControlWheels);
			m_controlGyros.SetLocalValue(myObjectBuilder_ShipController.ControlGyros);
			m_isTargetLockingEnabled.SetLocalValue(myObjectBuilder_ShipController.IsTargetLockingEnabled && BlockDefinition.TargetLockingEnabled);
			if (myObjectBuilder_ShipController.IsMainCockpit)
			{
				m_isMainCockpit.SetLocalValue(newValue: true);
			}
			m_horizonIndicatorEnabled.SetLocalValue(myObjectBuilder_ShipController.HorizonIndicatorEnabled);
			m_toolbar = new MyToolbar(ToolbarType);
			m_toolbar.Init(myObjectBuilder_ShipController.Toolbar, this);
			m_toolbar.ItemChanged += Toolbar_ItemChanged;
			m_buildToolbar = new MyToolbar(MyToolbarType.BuildCockpit);
			m_buildToolbar.Init(myObjectBuilder_ShipController.BuildToolbar, this);
			m_onLockedToolbar = new MyToolbar(MyToolbarType.ButtonPanel, 2, 1);
			m_onLockedToolbar.Init(myObjectBuilder_ShipController.OnLockedToolbar, this);
			m_onLockedToolbar.DrawNumbers = false;
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			m_baseIdleSound = BlockDefinition.PrimarySound;
			base.CubeGrid.OnGridSplit += CubeGrid_OnGridSplit;
			base.Components.ComponentAdded += OnComponentAdded;
			base.Components.ComponentRemoved += OnComponentRemoved;
			if (BlockDefinition.GetInSound != null && BlockDefinition.GetInSound.Length > 0)
			{
				GetInCockpitSound = new MySoundPair(BlockDefinition.GetInSound);
			}
			if (BlockDefinition.GetOutSound != null && BlockDefinition.GetOutSound.Length > 0)
			{
				GetOutOfCockpitSound = new MySoundPair(BlockDefinition.GetOutSound);
			}
			m_controlThrusters.ValueChanged += m_controlThrusters_ValueChanged;
		}

		private void m_controlThrusters_ValueChanged(SyncBase obj)
		{
			if (EntityThrustComponent != null && Sync.Players.HasExtendedControl(this, base.CubeGrid))
			{
				EntityThrustComponent.Enabled = m_controlThrusters;
			}
		}

		protected virtual void ComponentStack_IsFunctionalChanged()
		{
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_ShipController obj = (MyObjectBuilder_ShipController)base.GetObjectBuilderCubeBlock(copy);
			obj.SelectedGunId = m_selectedGunId;
			obj.UseSingleWeaponMode = m_singleWeaponMode;
			obj.ControlThrusters = m_controlThrusters;
			obj.ControlWheels = m_controlWheels;
			obj.ControlGyros = m_controlGyros;
			obj.Toolbar = m_toolbar.GetObjectBuilder();
			obj.BuildToolbar = m_buildToolbar.GetObjectBuilder();
			obj.OnLockedToolbar = m_onLockedToolbar.GetObjectBuilder();
			obj.IsMainCockpit = m_isMainCockpit;
			obj.HorizonIndicatorEnabled = HorizonIndicatorEnabled;
			obj.IsTargetLockingEnabled = m_isTargetLockingEnabled;
			return obj;
		}

		public virtual MatrixD GetHeadMatrix(bool includeY, bool includeX = true, bool forceBoneMatrix = false, bool forceHeadBone = false)
		{
			return base.PositionComp.WorldMatrixRef;
		}

		public override MatrixD GetViewMatrix()
		{
			MatrixD matrix = GetHeadMatrix(!ForceFirstPersonCamera, !ForceFirstPersonCamera);
			MatrixD.Invert(ref matrix, out var result);
			return result;
		}

		public void MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			MoveIndicator = moveIndicator;
			RotationIndicator = rotationIndicator;
			RollIndicator = rollIndicator;
			MyGroupControlSystem controlSystem = base.CubeGrid.GridSystems.ControlSystem;
			if (controlSystem == null || controlSystem.GetShipController() != this)
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

		public void WheelJump(bool controlPressed)
		{
			if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT && GridWheels != null && ControlWheels && m_enableShipControl && m_info.Controller != null && IsControllingCockpit())
			{
				base.CubeGrid.GridSystems.WheelSystem.UpdateJumpControlState(controlPressed, sync: true);
			}
		}

		public void MoveAndRotate()
		{
			if (base.Closed)
			{
				return;
			}
			MyGroupControlSystem controlSystem = base.CubeGrid.GridSystems.ControlSystem;
			if (controlSystem == null || (controlSystem.GetShipController() != null && controlSystem.GetShipController() != this))
			{
				return;
			}
			LastMotionIndicator = MoveIndicator;
			LastRotationIndicator = new Vector3(RotationIndicator, RollIndicator);
			if (MySessionComponentReplay.Static.IsEntityBeingRecorded(base.CubeGrid.EntityId))
			{
				PerFrameData perFrameData = default(PerFrameData);
				perFrameData.MovementData = new MovementData
				{
					MoveVector = MoveIndicator,
					RotateVector = new SerializableVector3(RotationIndicator.X, RotationIndicator.Y, RollIndicator)
				};
				PerFrameData data = perFrameData;
				MySessionComponentReplay.Static.ProvideEntityRecordData(base.CubeGrid.EntityId, data);
			}
			if (m_enableShipControl && MoveIndicator == Vector3.Zero && RotationIndicator == Vector2.Zero && RollIndicator == 0f)
			{
				ClearMovementControl();
			}
			else
			{
				if ((!IsMainCockpit && base.CubeGrid.HasMainCockpit() && !m_mainCockpitOverwritten) || (EntityThrustComponent == null && GridGyroSystem == null && GridWheels == null) || GridResourceDistributor == null)
				{
					return;
				}
				MyPlayer controllingPlayer = Sync.Players.GetControllingPlayer(base.CubeGrid);
				if ((!Sync.Players.HasExtendedControl(this, base.CubeGrid) && !MySessionComponentReplay.Static.IsEntityBeingReplayed(base.CubeGrid.EntityId) && (Pilot == null || controllingPlayer == null || controllingPlayer.Character != Pilot)) || !m_enableShipControl)
				{
					return;
				}
				if (!base.CubeGrid.Physics.RigidBody.IsActive)
				{
					base.CubeGrid.ActivatePhysics();
				}
				MyEntityThrustComponent entityThrustComponent = EntityThrustComponent;
				if (base.CubeGrid.GridSystems.ResourceDistributor.ResourceState != MyResourceStateEnum.NoPower)
				{
					base.Orientation.GetMatrix(out var result);
					if (entityThrustComponent != null)
					{
						entityThrustComponent.Enabled = m_controlThrusters;
						Vector3 vector = Vector3.Transform(MoveIndicator, result);
						entityThrustComponent.ControlThrust += vector;
					}
					if (GridGyroSystem != null && (bool)m_controlGyros)
					{
						Vector2 vector2 = RotationIndicator / 20f;
						vector2 = Vector2.ClampToSphere(vector2, 1f);
						float num = RollIndicator * RollControlMultiplier;
						Vector3 vector3 = Vector3.Transform(new Vector3(0f - vector2.X, 0f - vector2.Y, 0f - num), result);
						Vector3.ClampToSphere(vector3, 1f);
						GridGyroSystem.ControlTorque += vector3;
					}
					if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT && GridWheels != null && ControlWheels)
					{
						GridWheels.AngularVelocity = MoveIndicator;
					}
				}
			}
		}

		public void MoveAndRotateStopped()
		{
			ClearMovementControl();
		}

		public void ClearMovementControl()
		{
			if (base.CubeGrid.GridSystems.ControlSystem == null || base.CubeGrid.GridSystems.ControlSystem.GetShipController() != this)
			{
				return;
			}
			if (base.CubeGrid.GridSystems.ControlSystem != null && base.CubeGrid.GridSystems.ControlSystem.GetShipController() == this)
			{
				MoveIndicator = Vector3.Zero;
				RotationIndicator = Vector2.Zero;
				RollIndicator = 0f;
			}
			if (m_enableShipControl)
			{
				MyEntityThrustComponent entityThrustComponent = EntityThrustComponent;
				if (entityThrustComponent != null && !entityThrustComponent.AutopilotEnabled)
				{
					entityThrustComponent.ControlThrust = Vector3.Zero;
				}
				if (GridGyroSystem != null && !GridGyroSystem.AutopilotEnabled)
				{
					GridGyroSystem.ControlTorque = Vector3.Zero;
				}
				if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT && GridWheels != null)
				{
					GridWheels.AngularVelocity = Vector3.Zero;
				}
			}
		}

		public override void UpdatingStopped()
		{
			base.UpdatingStopped();
			ClearMovementControl();
		}

		public void UpdateControls()
		{
			MoveAndRotate();
		}

		public override void UpdateAfterSimulation()
		{
			base.UpdateAfterSimulation();
			if (base.CubeGrid.GridSystems.ControlSystem != null && (base.CubeGrid.GridSystems.ControlSystem.GetShipController() == this || base.CubeGrid.ControlledFromTurret))
			{
				if (EntityThrustComponent != null && !EntityThrustComponent.AutopilotEnabled)
				{
					EntityThrustComponent.ControlThrust = Vector3.Zero;
				}
				if (GridGyroSystem != null && !GridGyroSystem.AutopilotEnabled)
				{
					GridGyroSystem.ControlTorque = Vector3.Zero;
				}
				if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT && GridWheels != null)
				{
					GridWheels.AngularVelocity = Vector3.Zero;
				}
			}
			if (ControllerInfo.Controller != null && MySession.Static.LocalHumanPlayer != null && ControllerInfo.Controller == MySession.Static.LocalHumanPlayer.Controller)
			{
				MyEntityController controller = base.CubeGrid.GridSystems.ControlSystem.GetController();
				if (controller == ControllerInfo.Controller)
				{
					if (m_noControlNotification != null)
					{
						MyHud.Notifications.Remove(m_noControlNotification);
						m_noControlNotification = null;
					}
				}
				else if (m_noControlNotification == null && EnableShipControl)
				{
					if (controller == null && base.CubeGrid.GridSystems.ControlSystem.GetShipController() != null)
					{
						if (base.CubeGrid.GridSystems.ControlSystem.GetShipController().Priority == ControllerPriority.AutoPilot)
						{
							m_noControlNotification = new MyHudNotification(MySpaceTexts.Notification_NoControlAutoPilot, 0);
						}
						else
						{
							m_noControlNotification = new MyHudNotification(MySpaceTexts.Notification_NoControlLowerPriority, 0);
						}
					}
					else if (base.CubeGrid.HasMainCockpit() && !base.CubeGrid.IsMainCockpit(this))
					{
						m_noControlNotification = new MyHudNotification(MySpaceTexts.Notification_NoControlNotMain);
					}
					else if (controller != null && controller.ControlledEntity is MyCubeBlock && !base.CubeGrid.CubeBlocks.Contains((controller.ControlledEntity as MyCubeBlock).SlimBlock))
					{
						m_noControlNotification = new MyHudNotification(MySpaceTexts.Notification_NoControlOtherShip, 0);
					}
					else if (base.CubeGrid.IsStatic)
					{
						m_noControlNotification = new MyHudNotification(MySpaceTexts.Notification_NoControlStation, 0);
					}
					else
					{
						m_noControlNotification = new MyHudNotification(MySpaceTexts.Notification_NoControl, 0);
					}
					MyHud.Notifications.Add(m_noControlNotification);
				}
			}
			MyShootActionEnum[] values = MyEnum<MyShootActionEnum>.Values;
			foreach (MyShootActionEnum action in values)
			{
				if (IsShooting(action))
				{
					Shoot(action);
				}
			}
			if (CanBeMainCockpit())
			{
				if (base.CubeGrid.HasMainCockpit() && !base.CubeGrid.IsMainCockpit(this))
				{
					base.DetailedInfo.Clear();
					base.DetailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertiesText_MainCockpit));
					base.DetailedInfo.Append(": " + base.CubeGrid.MainCockpit.CustomName);
				}
				else
				{
					base.DetailedInfo.Clear();
				}
			}
			HandleBuldingMode();
		}

		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (Sync.MultiplayerActive && Sync.IsServer && m_lastMoveIndicator != MoveIndicator)
			{
				MyControllerInfo controllerInfo = ControllerInfo;
				int num;
				if (controllerInfo == null)
				{
<<<<<<< HEAD
					num = 0;
=======
					MyHud.Crosshair.ChangeDefaultSprite(MyHudTexturesEnum.Target_enemy, 0.01f);
					MyCubeBuilder.Static.Activate();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				else
				{
					MyEntityController controller = controllerInfo.Controller;
					if (controller == null)
					{
						num = 0;
					}
					else
					{
						MyPlayer player = controller.Player;
						if (player == null)
						{
							num = 0;
						}
						else
						{
							_ = player.Id;
							num = 1;
						}
					}
				}
				if (num != 0)
				{
					MyMultiplayer.RaiseEvent(this, (MyShipController x) => x.SyncMoveIndicator, MoveIndicator, new EndpointId(ControllerInfo.Controller.Player.Id.SteamId));
				}
			}
			m_lastMoveIndicator = MoveIndicator;
		}

<<<<<<< HEAD
		[Event(null, 873)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[BroadcastExcept]
		private void SyncMoveIndicator(Vector3 moveIndicator)
		{
			MoveIndicator = moveIndicator;
		}

		/// <summary>
		/// Handles logic related with 'building from cockpit'.
		/// </summary>
		private void HandleBuldingMode()
		{
			if (MySandboxGame.Config.ExperimentalMode && ((BuildingMode && !MySession.Static.IsCameraControlledObject()) || (MyInput.Static.IsNewKeyPressed(MyKeys.G) && MyInput.Static.IsAnyCtrlKeyPressed() && !MyInput.Static.IsAnyMousePressed() && m_enableBuilderCockpit && CanBeMainCockpit() && MySession.Static.IsCameraControlledObject() && MySession.Static.ControlledEntity == this)))
=======
		private void UpdateShipInfo()
		{
			if (!Sandbox.Engine.Platform.Game.IsDedicated && (MySession.Static.LocalHumanPlayer == null || ControllerInfo.Controller == MySession.Static.LocalHumanPlayer.Controller))
			{
				if (GridResourceDistributor != null)
				{
					MyHud.ShipInfo.FuelRemainingTime = GridResourceDistributor.RemainingFuelTimeByType(MyResourceDistributorComponent.ElectricityId);
					MyHud.ShipInfo.Reactors = GridResourceDistributor.MaxAvailableResourceByType(MyResourceDistributorComponent.ElectricityId);
					MyHud.ShipInfo.ResourceState = GridResourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId);
				}
				if (GridGyroSystem != null)
				{
					MyHud.ShipInfo.GyroCount = GridGyroSystem.GyroCount;
				}
				MyEntityThrustComponent entityThrustComponent = EntityThrustComponent;
				if (entityThrustComponent != null)
				{
					MyHud.ShipInfo.ThrustCount = entityThrustComponent.ThrustCount;
					MyHud.ShipInfo.DampenersEnabled = entityThrustComponent.DampenersEnabled;
				}
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
		}

		public override void UpdateBeforeSimulation10()
		{
			UpdateShipInfo10();
			base.UpdateBeforeSimulation10();
		}

		protected void UpdateShipInfo10()
		{
			if (Sync.IsDedicated || base.CubeGrid.GridSystems == null)
			{
				return;
			}
			MyGridResourceDistributorSystem resourceDistributor = base.CubeGrid.GridSystems.ResourceDistributor;
			hasPower = resourceDistributor != null && resourceDistributor.ResourceStateByType(MyResourceDistributorComponent.ElectricityId) != MyResourceStateEnum.NoPower;
			if (ControllerInfo == null || !ControllerInfo.IsLocallyHumanControlled())
			{
				return;
			}
			if (GridResourceDistributor != null)
			{
				MyHud.ShipInfo.PowerUsage = ((GridResourceDistributor.MaxAvailableResourceByType(MyResourceDistributorComponent.ElectricityId) != 0f) ? (GridResourceDistributor.TotalRequiredInputByType(MyResourceDistributorComponent.ElectricityId) / GridResourceDistributor.MaxAvailableResourceByType(MyResourceDistributorComponent.ElectricityId)) : 0f);
				MyHud.ShipInfo.NumberOfBatteries = GridResourceDistributor.GetSourceCount(MyResourceDistributorComponent.ElectricityId, MyStringHash.GetOrCompute("Battery"));
				GridResourceDistributor.UpdateHud(MyHud.SinkGroupInfo);
			}
			UpdateShipMass();
			if (base.Parent != null && base.Parent.Physics != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				BuildingMode = !BuildingMode;
				MyGuiAudio.PlaySound(MyGuiSounds.HudUse);
				Toolbar.Unselect();
				if (BuildingMode)
				{
					MyHud.Crosshair.ChangeDefaultSprite(MyHudTexturesEnum.Target_enemy, 0.01f);
					MyCubeBuilder.Static.Activate();
				}
				else
				{
					MyHud.Crosshair.ResetToDefault();
					MyCubeBuilder.Static.Deactivate();
				}
			}
		}

		public override void UpdateBeforeSimulation100()
		{
			if (m_useSoundEmitter != null)
			{
				m_useSoundEmitter.Update();
				UpdateSoundState();
			}
			base.UpdateBeforeSimulation100();
		}

		public static bool HasPriorityOver(MyShipController first, MyShipController second)
		{
			if (first.Priority < second.Priority)
			{
				return true;
			}
			if (first.Priority > second.Priority)
			{
				return false;
			}
			MyGridPhysics physics = first.CubeGrid.Physics;
			MyGridPhysics physics2 = second.CubeGrid.Physics;
			if (physics == null && physics2 == null)
			{
				return first.CubeGrid.BlocksCount > second.CubeGrid.BlocksCount;
			}
			if (physics != null && physics2 != null && physics.Shape.MassProperties.HasValue && physics2.Shape.MassProperties.HasValue)
			{
				float mass = physics.Shape.MassProperties.Value.Mass;
				float mass2 = physics2.Shape.MassProperties.Value.Mass;
				if (mass == mass2)
				{
					return first.EntityId > second.EntityId;
				}
				return mass > mass2;
			}
			return physics == null;
		}

		public void RefreshControlNotifications()
		{
			RemoveControlNotifications();
			string text = "[" + MyInput.Static.GetGameControl(MyControlsSpace.USE).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) + "]";
			m_notificationLeave = new MyHudNotification(LeaveNotificationHintText, 0);
			if (!MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed)
			{
				m_notificationLeave.SetTextFormatArguments(text, DisplayNameText);
			}
			else
			{
				m_notificationLeave.SetTextFormatArguments(MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_SPACESHIP, MyControlsSpace.USE), DisplayNameText);
			}
			m_notificationLeave.Level = MyNotificationLevel.Control;
			string text2 = "[" + MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL).GetControlButtonName(MyGuiInputDeviceEnum.Keyboard) + "]";
			if (!MyInput.Static.IsJoystickConnected() || !MyInput.Static.IsJoystickLastUsed)
			{
				m_notificationTerminal = new MyHudNotification(MySpaceTexts.NotificationHintOpenShipControlPanel, 0);
				m_notificationTerminal.SetTextFormatArguments(text2);
				m_notificationTerminal.Level = MyNotificationLevel.Control;
			}
			else
			{
				m_notificationTerminal = null;
			}
			MyHud.Notifications.Add(m_notificationLeave);
			if (m_notificationTerminal != null)
			{
				MyHud.Notifications.Add(m_notificationTerminal);
			}
		}

		public void RemoveControlNotifications()
		{
			if (m_notificationLeave != null)
			{
				MyHud.Notifications.Remove(m_notificationLeave);
			}
			if (m_notificationTerminal != null)
			{
				MyHud.Notifications.Remove(m_notificationTerminal);
			}
		}

		private void RefreshDLCNotification()
		{
			if (m_dlcNotificationDisplayed == null)
			{
				MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
				MyDLCs.MyDLC firstMissingDefinitionDLC = component.GetFirstMissingDefinitionDLC(BlockDefinition, Sync.MyId);
				if (firstMissingDefinitionDLC != null)
				{
					component.PushUsedUnownedDLC(firstMissingDefinitionDLC);
					m_dlcNotificationDisplayed = firstMissingDefinitionDLC;
				}
			}
		}

		private void RemoveDLCNotification()
		{
			if (m_dlcNotificationDisplayed != null)
			{
				MySession.Static.GetComponent<MySessionComponentDLC>().PopUsedUnownedDLC(m_dlcNotificationDisplayed);
				m_dlcNotificationDisplayed = null;
			}
		}

		private void OnComponentAdded(Type arg1, MyEntityComponentBase arg2)
		{
			if (arg1 == typeof(MyCasterComponent))
			{
				m_raycaster = arg2 as MyCasterComponent;
				base.PositionComp.OnPositionChanged += OnPositionChanged;
				OnPositionChanged(base.PositionComp);
			}
		}

		private void OnComponentRemoved(Type arg1, MyEntityComponentBase arg2)
		{
			if (arg1 == typeof(MyCasterComponent))
			{
				m_raycaster = null;
				base.PositionComp.OnPositionChanged -= OnPositionChanged;
			}
		}

		private void OnPositionChanged(MyPositionComponentBase obj)
		{
			MatrixD newTransform = obj.WorldMatrixRef;
			Vector3D vector3D2 = (newTransform.Translation = Vector3D.Transform(BlockDefinition.RaycastOffset, newTransform));
			if (m_raycaster != null)
			{
				m_raycaster.OnWorldPosChanged(ref newTransform);
			}
		}

		public override void OnAddedToScene(object source)
		{
			base.Render.NearFlag = false;
			base.OnAddedToScene(source);
			MyPlayerCollection.UpdateControl(base.CubeGrid);
		}

		public override void OnRemovedFromScene(object source)
		{
			m_controlSystems.ApplyChanges();
			base.OnRemovedFromScene(source);
		}

		protected virtual void OnControlAcquired_UpdateCamera()
		{
		}

		protected virtual bool IsCameraController()
		{
			return false;
		}

		private void OnControlEntityChanged(IMyControllableEntity oldControl, IMyControllableEntity newControl)
		{
			if (m_enableShipControl && oldControl != null && oldControl.Entity != null && newControl != null && newControl.Entity != null && base.CubeGrid.IsMainCockpit(oldControl.Entity as MyTerminalBlock))
			{
				MyEntity obj = ((oldControl.Entity.Parent == null) ? oldControl.Entity : oldControl.Entity.Parent);
				MyEntity myEntity = ((newControl.Entity.Parent == null) ? newControl.Entity : newControl.Entity.Parent);
				if (obj.EntityId == myEntity.EntityId)
				{
					ControlGroup.GetGroup(base.CubeGrid)?.GroupData.ControlSystem.AddControllerBlock(this);
					GridSelectionSystem.OnControlAcquired();
					m_mainCockpitOverwritten = true;
				}
			}
		}

		protected void OnControlAcquired(MyEntityController controller)
		{
			m_isControlled = true;
			controller.ControlledEntityChanged += OnControlEntityChanged;
			if (MySession.Static.LocalHumanPlayer == controller.Player || Sync.IsServer)
			{
				if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT && m_enableShipControl && (IsMainCockpit || !base.CubeGrid.HasMainCockpit()) && base.CubeGrid.GridSystems.ControlSystem != null && (base.CubeGrid.GridSystems.ControlSystem.GetShipController() == this || base.CubeGrid.GridSystems.ControlSystem.GetShipController() == null))
				{
					GridWheels.InitControl(controller.ControlledEntity as MyEntity);
				}
				if (MySession.Static.CameraController is MyEntity && IsCameraController() && MySession.Static.LocalHumanPlayer == controller.Player && !MySession.Static.GetComponent<MySessionComponentCutscenes>().IsCutsceneRunning)
				{
					MySession.Static.SetCameraController(MyCameraControllerEnum.Entity, this);
				}
				if (GridResourceDistributor != null)
				{
					GridResourceDistributor.ConveyorSystem_OnPoweredChanged();
				}
				if (EntityThrustComponent != null)
				{
					EntityThrustComponent.MarkDirty();
				}
				Static_CameraAttachedToChanged(null, null);
				if (MySession.Static.LocalHumanPlayer == controller.Player)
				{
					if (MySession.Static.Settings.RespawnShipDelete && base.CubeGrid.IsRespawnGrid)
					{
						MyHud.Notifications.Add(MyNotificationSingletons.RespawnShipWarning);
					}
					RefreshControlNotifications();
					RefreshDLCNotification();
					if (IsCameraController())
					{
						OnControlAcquired_UpdateCamera();
					}
					MyHud.HideAll();
					MyHud.Crosshair.ResetToDefault();
					MyHud.SinkGroupInfo.Visible = true;
					MyHud.GravityIndicator.Entity = this;
					MyHud.GravityIndicator.Show(null);
					MyHud.OreMarkers.Visible = true;
					MyHud.LargeTurretTargets.Visible = true;
				}
			}
			else
			{
				UpdateHudMarker();
			}
			if (m_enableShipControl && (IsMainCockpit || !base.CubeGrid.HasMainCockpit()))
			{
				ControlGroup.GetGroup(base.CubeGrid)?.GroupData.ControlSystem.AddControllerBlock(this);
				GridSelectionSystem.OnControlAcquired();
			}
			if (BuildingMode && MySession.Static.ControlledEntity is MyRemoteControl)
			{
				BuildingMode = false;
			}
			if (BuildingMode)
			{
				MyHud.Crosshair.ChangeDefaultSprite(MyHudTexturesEnum.Target_enemy, 0.01f);
			}
			else
			{
				MyHud.Crosshair.ResetToDefault();
			}
			MyEntityThrustComponent entityThrustComponent = EntityThrustComponent;
			if (controller == Sync.Players.GetEntityController(base.CubeGrid) && entityThrustComponent != null)
			{
				entityThrustComponent.Enabled = m_controlThrusters;
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_FRAME;
			if (Sync.IsServer || controller.Player == MySession.Static.LocalHumanPlayer)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_10TH_FRAME | MyEntityUpdateEnum.EACH_100TH_FRAME;
			}
		}

		protected virtual void OnControlReleased_UpdateCamera()
		{
		}

		protected virtual void OnControlReleased(MyEntityController controller)
		{
			m_isControlled = false;
			controller.ControlledEntityChanged -= OnControlEntityChanged;
			m_mainCockpitOverwritten = false;
			MyEntityThrustComponent entityThrustComponent = EntityThrustComponent;
			if (Sync.Players.GetEntityController(this) == controller && entityThrustComponent != null)
			{
				entityThrustComponent.Enabled = true;
			}
			if ((MySession.Static.LocalHumanPlayer == controller.Player || Sync.IsServer) && entityThrustComponent != null)
			{
				ClearMovementControl();
			}
			if (MySession.Static.LocalHumanPlayer == controller.Player)
			{
				OnControlReleased_UpdateCamera();
				ForceFirstPersonCamera = false;
				if (MyGuiScreenGamePlay.Static != null)
				{
					Static_CameraAttachedToChanged(null, null);
				}
				MyHud.Notifications.Remove(MyNotificationSingletons.RespawnShipWarning);
				RemoveControlNotifications();
				RemoveDLCNotification();
				MyHud.GravityIndicator.Hide();
				MyHud.Crosshair.HideDefaultSprite();
				MyHud.Crosshair.Recenter();
				MyHud.LargeTurretTargets.Visible = false;
				MyHud.Notifications.Remove(m_noControlNotification);
				m_noControlNotification = null;
			}
			else if (!MyFakes.ENABLE_RADIO_HUD)
			{
				MyHud.LocationMarkers.UnregisterMarker(this);
			}
			if (IsShooting())
			{
				EndShootAll();
			}
			if (m_enableShipControl && (IsMainCockpit || !base.CubeGrid.HasMainCockpit()))
			{
				if (GridSelectionSystem != null)
				{
					GridSelectionSystem.OnControlReleased();
				}
				ControlGroup.GetGroup(base.CubeGrid)?.GroupData.ControlSystem.RemoveControllerBlock(this);
			}
			if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT && m_enableShipControl && IsControllingCockpit())
			{
				GridWheels.ReleaseControl(controller.ControlledEntity as MyEntity);
			}
			m_clientState.Value = null;
		}

		public virtual void ForceReleaseControl()
		{
		}

		private void UpdateHudMarker()
		{
			if (!MyFakes.ENABLE_RADIO_HUD)
			{
				MyHud.LocationMarkers.RegisterMarker(this, new MyHudEntityParams
				{
					FlagsEnum = MyHudIndicatorFlagsEnum.SHOW_TEXT,
					Text = new StringBuilder(ControllerInfo.Controller.Player.DisplayName),
					ShouldDraw = MyHud.CheckShowPlayerNamesOnHud
				});
			}
		}

		protected virtual bool ShouldSit()
		{
			return !m_enableShipControl;
		}

		private void Static_CameraAttachedToChanged(IMyCameraController oldController, IMyCameraController newController)
		{
			if (MySession.Static.ControlledEntity == this && newController != MyThirdPersonSpectator.Static && newController != this)
			{
				EndShootAll();
			}
			UpdateCameraAfterChange();
		}

		protected virtual void UpdateCameraAfterChange(bool resetHeadLocalAngle = true)
		{
		}

		public void Shoot(MyShootActionEnum action)
		{
			if (m_enableShipControl && !IsWaitingForWeaponSwitch && GridSelectionSystem.CanShoot(action, out var _, out var _))
			{
				GridSelectionSystem.Shoot(action);
			}
		}

		public void Zoom(bool newKeyPress)
		{
		}

		public void Use()
		{
			if (GetOutOfCockpitSound == MySoundPair.Empty)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudUse);
			}
			RaiseControlledEntityUsed();
		}

		public void PlayUseSound(bool getIn)
		{
			if (m_useSoundEmitter != null)
			{
				m_useSoundEmitter.VolumeMultiplier = 1f;
				if (getIn)
				{
					m_useSoundEmitter.PlaySound(GetInCockpitSound, stopPrevious: false, skipIntro: false, MySession.Static.LocalCharacter != null && Pilot == MySession.Static.LocalCharacter);
				}
				else
				{
					m_useSoundEmitter.PlaySound(GetOutOfCockpitSound, stopPrevious: false, skipIntro: false, MySession.Static.LocalCharacter != null && m_lastPilot == MySession.Static.LocalCharacter);
				}
			}
		}

		public void RaiseControlledEntityUsed()
		{
			MyMultiplayer.RaiseEvent(this, (MyShipController x) => x.sync_ControlledEntity_Used);
		}

		public void UseContinues()
		{
		}

		public void UseFinished()
		{
		}

		public void PickUp()
		{
		}

		public void PickUpContinues()
		{
		}

		public void PickUpFinished()
		{
		}

		public void Crouch()
		{
		}

		public void Jump(Vector3 moveIndicator)
		{
		}

		public void SwitchWalk()
		{
		}

		public void Sprint(bool enabled)
		{
		}

		public void Up()
		{
		}

		public void Down()
		{
		}

		public virtual void ShowInventory()
		{
		}

		public virtual void ShowTerminal()
		{
		}

		public void SwitchBroadcasting()
		{
			if (!m_enableShipControl)
			{
				return;
			}
			if (base.CubeGrid.GridSystems.RadioSystem.AntennasBroadcasterEnabled == MyMultipleEnabledEnum.AllDisabled)
			{
				base.CubeGrid.GridSystems.RadioSystem.AntennasBroadcasterEnabled = MyMultipleEnabledEnum.AllEnabled;
				MyGuiAudio.PlaySound(MyGuiSounds.HudAntennaOn);
				return;
			}
			base.CubeGrid.GridSystems.RadioSystem.AntennasBroadcasterEnabled = MyMultipleEnabledEnum.AllDisabled;
			if (base.CubeGrid.GridSystems.RadioSystem.AntennasBroadcasterEnabled != 0)
			{
				MyGuiAudio.PlaySound(MyGuiSounds.HudAntennaOff);
			}
		}

		public void SwitchDamping()
		{
			if (m_enableShipControl && EntityThrustComponent != null)
			{
				base.CubeGrid.EnableDampingInternal(!EntityThrustComponent.DampenersEnabled, updateProxy: true);
				if (!EntityThrustComponent.DampenersEnabled)
				{
					RelativeDampeningEntity = null;
				}
			}
		}

		public virtual void SwitchThrusts()
		{
		}

		public void Die()
		{
		}

		public void SwitchLights()
		{
			if (m_enableShipControl)
			{
				if (GridReflectorLights.ReflectorsEnabled == MyMultipleEnabledEnum.AllDisabled)
				{
					GridReflectorLights.ReflectorsEnabled = MyMultipleEnabledEnum.AllEnabled;
				}
				else
				{
					GridReflectorLights.ReflectorsEnabled = MyMultipleEnabledEnum.AllDisabled;
				}
			}
		}

		public void SwitchHandbrake()
		{
			if (m_enableShipControl && (IsMainCockpit || !base.CubeGrid.HasMainCockpit()))
			{
				MyMultiplayer.RaiseEvent(base.CubeGrid, (MyCubeGrid x) => x.ToggleHandbrakeRequest);
			}
		}

		public void SwitchParkedStatus()
		{
			if (m_enableShipControl && (IsMainCockpit || !base.CubeGrid.HasMainCockpit()))
			{
				if (base.CubeGrid.IsParked)
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudBrakeOff);
				}
				else
				{
					MyGuiAudio.PlaySound(MyGuiSounds.HudBrakeOn);
				}
				MyMultiplayer.RaiseEvent(base.CubeGrid, (MyCubeGrid x) => x.ToggleParkStateRequest);
			}
		}

		public void SwitchLandingGears()
		{
			SwitchParkedStatus();
		}

		public void SwitchReactors()
		{
			if ((base.CubeGrid.MainCockpit != null && !IsMainCockpit) || !m_enableShipControl)
			{
				return;
			}
			base.CubeGrid.SwitchPower();
			MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group = MyCubeGridGroups.Static.Logical.GetGroup(base.CubeGrid);
			if (group != null)
			{
				foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node in group.Nodes)
				{
					if (node.NodeData.IsPowered != base.CubeGrid.IsPowered)
					{
						node.NodeData.SwitchPower();
					}
				}
			}
			if (base.CubeGrid.IsPowered)
			{
				base.CubeGrid.ChangePowerProducerState(MyMultipleEnabledEnum.AllEnabled, MySession.Static.LocalPlayerId);
			}
			else
			{
				base.CubeGrid.ChangePowerProducerState(MyMultipleEnabledEnum.AllDisabled, MySession.Static.LocalPlayerId);
			}
			if (!Sync.IsServer)
			{
				base.CubeGrid.ActivatePhysics();
			}
		}

		public void SwitchReactorsLocal()
		{
			if ((base.CubeGrid.MainCockpit != null && !IsMainCockpit) || !m_enableShipControl)
			{
				return;
			}
			base.CubeGrid.SwitchPower();
			MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Group group = MyCubeGridGroups.Static.Mechanical.GetGroup(base.CubeGrid);
			if (group != null)
			{
				foreach (MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node node in group.Nodes)
				{
					if (node.NodeData.IsPowered != base.CubeGrid.IsPowered)
					{
						node.NodeData.SwitchPower();
					}
				}
			}
			if (base.CubeGrid.IsPowered)
			{
				base.CubeGrid.ChangePowerProducerState(MyMultipleEnabledEnum.AllEnabled, MySession.Static.LocalPlayerId, localGridOnly: true);
			}
			else
			{
				base.CubeGrid.ChangePowerProducerState(MyMultipleEnabledEnum.AllDisabled, MySession.Static.LocalPlayerId, localGridOnly: true);
			}
			if (!Sync.IsServer)
			{
				base.CubeGrid.ActivatePhysics();
			}
		}

		public void DrawHud(IMyCameraController camera, long playerId)
		{
			if (camera is MySpectatorCameraController)
			{
				MyHud.Crosshair.Recenter();
				return;
			}
			if (GridSelectionSystem != null)
			{
				GridSelectionSystem.DrawHud(camera, playerId);
			}
			Vector3D worldPosition = base.PositionComp.GetPosition() + 1000.0 * base.PositionComp.WorldMatrixRef.Forward;
			Vector2 target = Vector2.Zero;
			if (MyHudCrosshair.GetProjectedVector(worldPosition, ref target))
			{
				MyHud.Crosshair.ChangePosition(target);
			}
			if (m_raycaster == null)
			{
				return;
			}
			MySlimBlock mySlimBlock = m_raycaster.HitBlock;
			if (mySlimBlock == null)
			{
				MyWelder.ProjectionRaycastData projectionRaycastData = FindProjectedBlock();
				if (projectionRaycastData.raycastResult == BuildCheckResult.OK)
				{
					mySlimBlock = projectionRaycastData.hitCube;
				}
			}
			if (mySlimBlock != null)
			{
				if (mySlimBlock.CubeGrid.Projector == null)
				{
					MySlimBlock.SetBlockComponents(MyHud.BlockInfo, mySlimBlock);
					MyHud.BlockInfo.BlockIntegrity = mySlimBlock.Integrity / mySlimBlock.MaxIntegrity;
				}
				else
				{
					MySlimBlock.SetBlockComponents(MyHud.BlockInfo, mySlimBlock.BlockDefinition);
					MyHud.BlockInfo.BlockIntegrity = 0.01f;
					MyHud.BlockInfo.MissingComponentIndex = 0;
				}
				MyHud.BlockInfo.DefinitionId = mySlimBlock.BlockDefinition.Id;
				MyHud.BlockInfo.BlockName = mySlimBlock.BlockDefinition.DisplayNameText;
				MyHud.BlockInfo.PCUCost = mySlimBlock.BlockDefinition.PCU;
				MyHud.BlockInfo.BlockIcons = mySlimBlock.BlockDefinition.Icons;
				MyHud.BlockInfo.CriticalIntegrity = mySlimBlock.BlockDefinition.CriticalIntegrityRatio;
				MyHud.BlockInfo.CriticalComponentIndex = mySlimBlock.BlockDefinition.CriticalGroup;
				MyHud.BlockInfo.OwnershipIntegrity = mySlimBlock.BlockDefinition.OwnershipIntegrityRatio;
				MyHud.BlockInfo.BlockBuiltBy = mySlimBlock.BuiltBy;
				MyHud.BlockInfo.GridSize = mySlimBlock.CubeGrid.GridSizeEnum;
			}
			else
			{
				MyHud.BlockInfo.DefinitionId = default(MyDefinitionId);
				MyHud.BlockInfo.MissingComponentIndex = -1;
				MyHud.BlockInfo.BlockName = m_raycaster.Caster.DrillDefinition.DisplayNameText;
				MyHud.BlockInfo.SetContextHelp(m_raycaster.Caster.DrillDefinition);
				MyHud.BlockInfo.PCUCost = 0;
				MyHud.BlockInfo.BlockIcons = m_raycaster.Caster.DrillDefinition.Icons;
				MyHud.BlockInfo.BlockIntegrity = 1f;
				MyHud.BlockInfo.CriticalIntegrity = 0f;
				MyHud.BlockInfo.CriticalComponentIndex = 0;
				MyHud.BlockInfo.OwnershipIntegrity = 0f;
				MyHud.BlockInfo.BlockBuiltBy = 0L;
				MyHud.BlockInfo.GridSize = MyCubeSize.Small;
				MyHud.BlockInfo.Components.Clear();
			}
		}

		public virtual bool IsLargeShip()
		{
			return true;
		}

		public bool CanSwitchToWeapon(MyDefinitionId? weapon)
		{
			if (!weapon.HasValue)
			{
				return true;
			}
			if (base.CubeGrid.GridSystems.WeaponSystem.GetGun(weapon.Value) == null)
			{
				return false;
			}
			return true;
		}

		public void SwitchToWeapon(MyDefinitionId weapon)
		{
			if (m_enableShipControl)
			{
				SwitchToWeaponInternal(weapon, updateSync: true);
			}
		}

		public void SwitchToWeapon(MyToolbarItemWeapon weapon)
		{
			if (m_enableShipControl)
			{
				SwitchToWeaponInternal(weapon?.Definition.Id, updateSync: true);
			}
		}

		private void SwitchToWeaponInternal(MyDefinitionId? weapon, bool updateSync)
		{
			if (MySessionComponentReplay.Static.IsEntityBeingRecorded(base.CubeGrid.EntityId))
			{
				PerFrameData perFrameData = default(PerFrameData);
				perFrameData.SwitchWeaponData = new SwitchWeaponData
				{
					WeaponDefinition = weapon
				};
				PerFrameData data = perFrameData;
				MySessionComponentReplay.Static.ProvideEntityRecordData(base.CubeGrid.EntityId, data);
			}
			if (updateSync)
			{
				RequestSwitchToWeapon(weapon, null, 0L);
				return;
			}
			StopCurrentWeaponShooting();
			if (weapon.HasValue)
			{
				SwitchToWeaponInternal(weapon);
				((Type)weapon.Value.TypeId).Name.Split(new char[1] { '_' });
			}
			else
			{
				m_selectedGunId = null;
				GridSelectionSystem.SwitchTo(null);
			}
		}

		private void SwitchToWeaponInternal(MyDefinitionId? gunId)
		{
			GridSelectionSystem.SwitchTo(gunId, m_singleWeaponMode);
			m_selectedGunId = gunId;
			if (ControllerInfo.IsLocallyHumanControlled())
			{
				if (m_weaponSelectedNotification == null)
				{
					m_weaponSelectedNotification = new MyHudNotification(MyCommonTexts.NotificationSwitchedToWeapon);
				}
<<<<<<< HEAD
				m_weaponSelectedNotification.SetTextFormatArguments(MyDefinitionManager.Static.GetDefinition(m_selectedGunId.Value).DisplayNameText);
=======
				m_weaponSelectedNotification.SetTextFormatArguments(MyDeviceBase.GetGunNotificationName(m_selectedGunId.Value));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (m_weaponSelectedNotification.Alive)
				{
					MyHud.Notifications.Update(m_weaponSelectedNotification);
					m_weaponSelectedNotification.ResetAliveTime();
				}
				else
				{
					MyHud.Notifications.Add(m_weaponSelectedNotification);
				}
			}
		}

		private void SwitchAmmoMagazineInternal(bool sync)
		{
			if (sync)
			{
				MyMultiplayer.RaiseEvent(this, (MyShipController x) => x.OnSwitchAmmoMagazineRequest);
			}
			else if (m_enableShipControl && !IsWaitingForWeaponSwitch)
			{
				GridSelectionSystem.SwitchAmmoMagazine();
			}
		}

		private void SwitchAmmoMagazineSuccess()
		{
			if (GridSelectionSystem.CanSwitchAmmoMagazine())
			{
				SwitchAmmoMagazineInternal(sync: false);
			}
		}

		private void ShowShootNotification(MyGunStatusEnum status, IMyGunObject<MyDeviceBase> weapon)
		{
			if (!ControllerInfo.IsLocallyHumanControlled())
			{
				return;
			}
			switch (status)
			{
			case MyGunStatusEnum.NotSelected:
				if (m_noWeaponNotification == null)
				{
					m_noWeaponNotification = new MyHudNotification(MyCommonTexts.NotificationNoWeaponSelected, 2000, "Red");
					MyHud.Notifications.Add(m_noWeaponNotification);
				}
				MyHud.Notifications.Add(m_noWeaponNotification);
				break;
			case MyGunStatusEnum.OutOfAmmo:
				if (m_outOfAmmoNotification == null)
				{
					m_outOfAmmoNotification = new MyHudNotification(MyCommonTexts.OutOfAmmo, 2000, "Red");
				}
				if (weapon is MyCubeBlock)
				{
					m_outOfAmmoNotification.SetTextFormatArguments((weapon as MyCubeBlock).DisplayNameText);
				}
				MyHud.Notifications.Add(m_outOfAmmoNotification);
				break;
			case MyGunStatusEnum.NotFunctional:
				if (m_weaponNotWorkingNotification == null)
				{
					m_weaponNotWorkingNotification = new MyHudNotification(MyCommonTexts.NotificationWeaponNotWorking, 2000, "Red");
				}
				if (weapon is MyCubeBlock)
				{
					m_weaponNotWorkingNotification.SetTextFormatArguments((weapon as MyCubeBlock).DisplayNameText);
				}
				MyHud.Notifications.Add(m_weaponNotWorkingNotification);
				break;
			case MyGunStatusEnum.Disabled:
			case MyGunStatusEnum.Failed:
				break;
			}
		}

		public override void OnRegisteredToGridSystems()
		{
			GridGyroSystem = base.CubeGrid.GridSystems.GyroSystem;
			GridReflectorLights = base.CubeGrid.GridSystems.ReflectorLightSystem;
			base.CubeGrid.AddedToLogicalGroup += CubeGrid_AddedToLogicalGroup;
			base.CubeGrid.RemovedFromLogicalGroup += CubeGrid_RemovedFromLogicalGroup;
			SetWeaponSystem(base.CubeGrid.GridSystems.WeaponSystem);
			base.OnRegisteredToGridSystems();
		}

		public override void OnUnregisteredFromGridSystems()
		{
			if (EntityThrustComponent != null)
			{
				ClearMovementControl();
			}
			base.CubeGrid.AddedToLogicalGroup -= CubeGrid_AddedToLogicalGroup;
			base.CubeGrid.RemovedFromLogicalGroup -= CubeGrid_RemovedFromLogicalGroup;
			CubeGrid_RemovedFromLogicalGroup();
			GridGyroSystem = null;
			GridReflectorLights = null;
			base.OnUnregisteredFromGridSystems();
		}

		private void CubeGrid_RemovedFromLogicalGroup()
		{
			GridSelectionSystem.WeaponSystem = null;
			GridSelectionSystem.SwitchTo(null);
		}

		private void CubeGrid_AddedToLogicalGroup(MyGridLogicalGroupData obj)
		{
			SetWeaponSystem(obj.WeaponSystem);
		}

		public void SetWeaponSystem(MyGridWeaponSystem weaponSystem)
		{
			GridSelectionSystem.WeaponSystem = weaponSystem;
			GridSelectionSystem.SwitchTo(m_selectedGunId, m_singleWeaponMode);
		}

		public override void UpdateVisual()
		{
			if (base.Render.NearFlag)
			{
				base.Render.ColorMaskHsv = SlimBlock.ColorMaskHSV;
			}
			else
			{
				base.UpdateVisual();
			}
		}

<<<<<<< HEAD
		[Event(null, 1963)]
=======
		[Event(null, 1925)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		protected void sync_ControlledEntity_Used()
		{
			OnControlledEntity_Used();
			if (GetOutOfCockpitSound != MySoundPair.Empty)
			{
				PlayUseSound(getIn: false);
			}
		}

<<<<<<< HEAD
		[Event(null, 1971)]
=======
		[Event(null, 1933)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnSwitchHelmet()
		{
			if (Pilot != null && Pilot.OxygenComponent != null)
			{
				Pilot.OxygenComponent.SwitchHelmet();
			}
		}

		protected virtual void OnControlledEntity_Used()
		{
		}

		private void SwitchToWeaponSuccess(MyDefinitionId? weapon, MyObjectBuilder_Base weaponObjectBuilder, long weaponEntityId)
		{
			SwitchToWeaponInternal(weapon, updateSync: false);
		}

		public void BeginShoot(MyShootActionEnum action)
		{
			if (!IsWaitingForWeaponSwitch)
			{
				if (MySessionComponentReplay.Static.IsEntityBeingRecorded(base.CubeGrid.EntityId))
				{
					PerFrameData perFrameData = default(PerFrameData);
					perFrameData.ShootData = new ShootData
					{
						Begin = true,
						ShootAction = (byte)action
					};
					PerFrameData data = perFrameData;
					MySessionComponentReplay.Static.ProvideEntityRecordData(base.CubeGrid.EntityId, data);
				}
				MyGunStatusEnum status = MyGunStatusEnum.OK;
				IMyGunObject<MyDeviceBase> FailedGun = null;
				GridSelectionSystem.CanShoot(action, out status, out FailedGun);
				if (status != 0)
				{
					ShowShootNotification(status, FailedGun);
				}
				BeginShootSync(action);
			}
		}

		protected void EndShootAll()
		{
			MyShootActionEnum[] values = MyEnum<MyShootActionEnum>.Values;
			foreach (MyShootActionEnum action in values)
			{
				if (IsShooting(action))
				{
					EndShoot(action);
				}
			}
		}

		private void StopCurrentWeaponShooting()
		{
			MyShootActionEnum[] values = MyEnum<MyShootActionEnum>.Values;
			foreach (MyShootActionEnum action in values)
			{
				if (IsShooting(action))
				{
					GridSelectionSystem.EndShoot(action);
				}
			}
		}

		public void EndShoot(MyShootActionEnum action)
		{
			if (MySessionComponentReplay.Static.IsEntityBeingRecorded(base.CubeGrid.EntityId))
			{
				PerFrameData perFrameData = default(PerFrameData);
				perFrameData.ShootData = new ShootData
				{
					Begin = false,
					ShootAction = (byte)action
				};
				PerFrameData data = perFrameData;
				MySessionComponentReplay.Static.ProvideEntityRecordData(base.CubeGrid.EntityId, data);
			}
			if (BuildingMode && Pilot != null)
			{
				Pilot.EndShoot(action);
			}
			EndShootSync(action);
		}

		public void OnBeginShoot(MyShootActionEnum action)
		{
			MyGunStatusEnum status = MyGunStatusEnum.OK;
			IMyGunObject<MyDeviceBase> FailedGun = null;
			if (!GridSelectionSystem.CanShoot(action, out status, out FailedGun) && status != 0 && status != MyGunStatusEnum.Cooldown)
			{
				ShootBeginFailed(action, status, FailedGun);
			}
			else
			{
				GridSelectionSystem.BeginShoot(action);
			}
		}

		public void OnEndShoot(MyShootActionEnum action)
		{
			GridSelectionSystem.EndShoot(action);
		}

		private void ShootBeginFailed(MyShootActionEnum action, MyGunStatusEnum status, IMyGunObject<MyDeviceBase> failedGun)
		{
			if (failedGun != null)
			{
				failedGun.BeginFailReaction(action, status);
				if (MySession.Static.ControlledEntity != null && base.CubeGrid == ((MyEntity)MySession.Static.ControlledEntity).GetTopMostParent())
				{
					failedGun.BeginFailReactionLocal(action, status);
				}
			}
		}

		protected override void Closing()
		{
			if (MyFakes.ENABLE_NEW_SOUNDS)
			{
				StopLoopSound();
			}
			IsMainCockpit = false;
			if (!base.CubeGrid.MarkedForClose)
			{
				base.CubeGrid.OnGridSplit -= CubeGrid_OnGridSplit;
			}
			if (m_useSoundEmitter != null)
			{
				m_useSoundEmitter.StopSound(forced: true);
				m_useSoundEmitter = null;
			}
			base.Closing();
		}

		protected virtual void UpdateSoundState()
		{
		}

		protected virtual void StartLoopSound()
		{
		}

		protected virtual void StopLoopSound()
		{
		}

		public void RemoveUsers(bool local)
		{
			if (local)
			{
				RemoveLocal();
			}
			else
			{
				RaiseControlledEntityUsed();
			}
		}

		protected virtual void RemoveLocal()
		{
		}

		internal void SwitchWeaponMode()
		{
			SingleWeaponMode = !SingleWeaponMode;
		}

		private void MainCockpitChanged()
		{
			if ((bool)m_isMainCockpit)
			{
				base.CubeGrid.SetMainCockpit(this);
			}
			else if (base.CubeGrid.IsMainCockpit(this))
			{
				base.CubeGrid.SetMainCockpit(null);
			}
		}

		protected virtual bool CanBeMainCockpit()
		{
			return false;
		}

		protected virtual bool CanHaveHorizon()
		{
			return BlockDefinition.EnableShipControl;
		}

		protected bool IsMainCockpitFree()
		{
			if (base.CubeGrid.HasMainCockpit())
			{
				return base.CubeGrid.IsMainCockpit(this);
			}
			return true;
		}

		protected bool IsControllingCockpit()
		{
			if (!IsMainCockpitFree())
			{
				return m_mainCockpitOverwritten;
			}
			return true;
		}

		MatrixD VRage.Game.ModAPI.Interfaces.IMyControllableEntity.GetHeadMatrix(bool includeY, bool includeX, bool forceHeadAnim, bool forceHeadBone)
		{
			return GetHeadMatrix(includeY, includeX, forceHeadAnim);
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.MoveAndRotate(Vector3 moveIndicator, Vector2 rotationIndicator, float rollIndicator)
		{
			MoveAndRotate(moveIndicator, rotationIndicator, rollIndicator);
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.MoveAndRotateStopped()
		{
			MoveAndRotateStopped();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Use()
		{
			Use();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.UseContinues()
		{
			UseContinues();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.PickUp()
		{
			PickUp();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.PickUpContinues()
		{
			PickUpContinues();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Jump(Vector3 moveIndicator)
		{
			Jump(moveIndicator);
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Up()
		{
			Up();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Crouch()
		{
			Crouch();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Down()
		{
			Down();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ShowInventory()
		{
			ShowInventory();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.ShowTerminal()
		{
			ShowTerminal();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchThrusts()
		{
			SwitchThrusts();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchDamping()
		{
			SwitchDamping();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchLights()
		{
			SwitchLights();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchLandingGears()
		{
			SwitchLandingGears();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchHandbrake()
		{
			SwitchHandbrake();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchReactors()
		{
			SwitchReactors();
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.SwitchHelmet()
		{
			if (Pilot != null && (Sync.IsServer || MySession.Static.LocalCharacter == Pilot))
			{
				MyMultiplayer.RaiseEvent(this, (MyShipController x) => x.OnSwitchHelmet);
			}
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.Die()
		{
			Die();
		}

		void IMyControllableEntity.SwitchAmmoMagazine()
		{
			if (m_enableShipControl && GridSelectionSystem.CanSwitchAmmoMagazine())
			{
				SwitchAmmoMagazineInternal(sync: true);
			}
		}

		bool IMyControllableEntity.CanSwitchAmmoMagazine()
		{
			if (m_selectedGunId.HasValue)
			{
				return GridSelectionSystem.CanSwitchAmmoMagazine();
			}
			return false;
		}

		private void CubeGrid_OnGridSplit(MyCubeGrid grid1, MyCubeGrid grid2)
		{
			CheckGridCokpit(grid1);
			CheckGridCokpit(grid2);
		}

		private bool HasCockpit(MyCubeGrid grid)
		{
			return grid.CubeBlocks.Contains(SlimBlock);
		}

		private void CheckGridCokpit(MyCubeGrid grid)
		{
			if (!HasCockpit(grid) && grid.IsMainCockpit(this) && base.CubeGrid != grid)
			{
				grid.SetMainCockpit(null);
			}
		}

		public MyEntityCameraSettings GetCameraEntitySettings()
		{
			return null;
		}

		private void RequestSwitchToWeapon(MyDefinitionId? weapon, MyObjectBuilder_EntityBase weaponObjectBuilder, long weaponEntityId)
		{
			if (!Sync.IsServer)
			{
				m_switchWeaponCounter++;
			}
			SerializableDefinitionId? arg = weapon;
			MyMultiplayer.RaiseEvent(this, (MyShipController x) => x.SwitchToWeaponMessage, arg, weaponObjectBuilder, weaponEntityId);
		}

<<<<<<< HEAD
		[Event(null, 2593)]
=======
		[Event(null, 2578)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void SwitchToWeaponMessage(SerializableDefinitionId? weapon, [Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof(MyObjectBuilderDynamicSerializer))] MyObjectBuilder_EntityBase weaponObjectBuilder, long weaponEntityId)
		{
			if (!CanSwitchToWeapon(weapon))
			{
				if (MyEventContext.Current.IsLocallyInvoked)
				{
					OnSwitchToWeaponFailure(weapon, weaponObjectBuilder, weaponEntityId);
					return;
				}
				MyMultiplayer.RaiseEvent(this, (MyShipController x) => x.OnSwitchToWeaponFailure, weapon, weaponObjectBuilder, weaponEntityId, MyEventContext.Current.Sender);
				return;
			}
			if (weaponObjectBuilder != null && weaponObjectBuilder.EntityId == 0L)
			{
				weaponObjectBuilder = (MyObjectBuilder_EntityBase)weaponObjectBuilder.Clone();
				weaponObjectBuilder.EntityId = ((weaponEntityId == 0L) ? MyEntityIdentifier.AllocateId() : weaponEntityId);
			}
			OnSwitchToWeaponSuccess(weapon, weaponObjectBuilder, weaponEntityId);
			MyMultiplayer.RaiseEvent(this, (MyShipController x) => x.OnSwitchToWeaponSuccess, weapon, weaponObjectBuilder, weaponEntityId);
		}

<<<<<<< HEAD
		[Event(null, 2620)]
=======
		[Event(null, 2605)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Client]
		private void OnSwitchToWeaponFailure(SerializableDefinitionId? weapon, [Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof(MyObjectBuilderDynamicSerializer))] MyObjectBuilder_EntityBase weaponObjectBuilder, long weaponEntityId)
		{
			if (!Sync.IsServer)
			{
				m_switchWeaponCounter--;
			}
		}

<<<<<<< HEAD
		[Event(null, 2629)]
=======
		[Event(null, 2614)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void OnSwitchToWeaponSuccess(SerializableDefinitionId? weapon, [Serialize(MyObjectFlags.DefaultZero | MyObjectFlags.Dynamic, DynamicSerializerType = typeof(MyObjectBuilderDynamicSerializer))] MyObjectBuilder_EntityBase weaponObjectBuilder, long weaponEntityId)
		{
			if (!Sync.IsServer && m_switchWeaponCounter > 0)
			{
				m_switchWeaponCounter--;
			}
			SwitchToWeaponSuccess(weapon, weaponObjectBuilder, weaponEntityId);
		}

<<<<<<< HEAD
		[Event(null, 2645)]
=======
		[Event(null, 2630)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnSwitchAmmoMagazineRequest()
		{
			if (((IMyControllableEntity)this).CanSwitchAmmoMagazine())
			{
				SwitchAmmoMagazineSuccess();
				MyMultiplayer.RaiseEvent(this, (MyShipController x) => x.OnSwitchAmmoMagazineSuccess);
			}
		}

<<<<<<< HEAD
		[Event(null, 2657)]
=======
		[Event(null, 2642)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Broadcast]
		private void OnSwitchAmmoMagazineSuccess()
		{
			SwitchAmmoMagazineSuccess();
		}

		public void BeginShootSync(MyShootActionEnum action = MyShootActionEnum.PrimaryAction)
		{
			StartShooting(action);
			MyMultiplayer.RaiseEvent(this, (MyShipController x) => x.ShootBeginCallback, action);
			if (MyFakes.SIMULATE_QUICK_TRIGGER)
			{
				EndShootInternal(action);
			}
		}

<<<<<<< HEAD
		[Event(null, 2673)]
=======
		[Event(null, 2658)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[BroadcastExcept]
		private void ShootBeginCallback(MyShootActionEnum action)
		{
			if (!Sync.IsServer || !MyEventContext.Current.IsLocallyInvoked)
			{
				StartShooting(action);
			}
		}

		private void StartShooting(MyShootActionEnum action)
		{
			m_isShooting[(uint)action] = true;
			OnBeginShoot(action);
		}

		private void StopShooting(MyShootActionEnum action)
		{
			m_isShooting[(uint)action] = false;
			OnEndShoot(action);
		}

		public void EndShootSync(MyShootActionEnum action = MyShootActionEnum.PrimaryAction)
		{
			if (!MyFakes.SIMULATE_QUICK_TRIGGER)
			{
				EndShootInternal(action);
			}
		}

		private void EndShootInternal(MyShootActionEnum action = MyShootActionEnum.PrimaryAction)
		{
			MyMultiplayer.RaiseEvent(this, (MyShipController x) => x.ShootEndCallback, action);
			StopShooting(action);
		}

<<<<<<< HEAD
		[Event(null, 2709)]
=======
		[Event(null, 2694)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[BroadcastExcept]
		private void ShootEndCallback(MyShootActionEnum action)
		{
			if (!Sync.IsServer || !MyEventContext.Current.IsLocallyInvoked)
			{
				StopShooting(action);
			}
		}

		private void Toolbar_ItemChanged(MyToolbar self, MyToolbar.IndexArgs index, bool isGamepad)
		{
			if (m_syncing)
			{
				return;
			}
			MyToolbarItem myToolbarItem = ((!isGamepad) ? self.GetItemAtIndex(index.ItemIndex) : self.GetItemAtLinearIndexGamepad(index.ItemIndex));
			if (myToolbarItem != null)
			{
				MyMultiplayer.RaiseEvent(this, (MyShipController x) => x.SendToolbarItemChanged, myToolbarItem.GetObjectBuilder(), index.ItemIndex, isGamepad);
			}
			else
			{
				MyMultiplayer.RaiseEvent(this, (MyShipController x) => x.SendToolbarItemRemoved, index.ItemIndex, isGamepad);
			}
		}

<<<<<<< HEAD
		[Event(null, 2744)]
=======
		[Event(null, 2729)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void SendToolbarItemRemoved(int index, bool isGamepad)
		{
			m_syncing = true;
			Toolbar.SetItemAtIndex(index, null, isGamepad);
			m_syncing = false;
		}

<<<<<<< HEAD
		[Event(null, 2752)]
=======
		[Event(null, 2737)]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void SendToolbarItemChanged([DynamicObjectBuilder(false)] MyObjectBuilder_ToolbarItem sentItem, int index, bool isGamepad)
		{
			m_syncing = true;
			MyToolbarItem item = null;
			if (sentItem != null)
			{
				item = MyToolbarItemFactory.CreateToolbarItem(sentItem);
			}
			Toolbar.SetItemAtIndex(index, item, isGamepad);
			m_syncing = false;
		}

		public MyGridClientState GetNetState()
		{
			MyGridClientState result = default(MyGridClientState);
			result.Move = MoveIndicator;
			result.Rotation = RotationIndicator;
			result.Roll = RollIndicator;
			return result;
		}

		public void SetNetState(MyGridClientState netState)
		{
			MoveAndRotate(netState.Move, netState.Rotation, netState.Roll);
		}

		public void RemoveControlSystem(MyGroupControlSystem controlSystem)
		{
			m_controlSystems.Remove(controlSystem);
		}

		public void AddControlSystem(MyGroupControlSystem controlSystem)
		{
			m_controlSystems.Add(controlSystem);
		}

		public bool ShouldEndShootingOnPause(MyShootActionEnum action)
		{
			return true;
		}

		/// <summary>
		/// Tries to set the brakes. May fail if this is not the main cockpit.
		/// </summary>
		/// <param name="enable">Should the brakes be enabled, or not?</param>
		/// <returns>True if the brakes were enabled, false otherwise.</returns>
		public bool TryEnableBrakes(bool enable)
		{
			if (!MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT)
			{
				return false;
			}
			if (GridWheels == null || !ControlWheels)
			{
				return false;
			}
			if (!EnableShipControl || (!IsMainCockpit && base.CubeGrid.HasMainCockpit()))
			{
				return false;
			}
			base.CubeGrid.GridSystems.WheelSystem.Brake = enable;
			return true;
		}

		public MyWelder.ProjectionRaycastData FindProjectedBlock()
		{
			if (m_raycaster != null)
			{
				return MyWelder.FindProjectedBlock(m_raycaster, 2f);
			}
			MyWelder.ProjectionRaycastData result = default(MyWelder.ProjectionRaycastData);
			result.raycastResult = BuildCheckResult.NotFound;
			return result;
		}

		private MyAmmoDefinition GetSelectedAmmoDefinitionFromCubeBlock()
		{
			MyToolbarItemWeapon myToolbarItemWeapon;
			if (Toolbar?.SelectedItem != null && GridSelectionSystem?.WeaponSystem != null && (myToolbarItemWeapon = Toolbar?.SelectedItem as MyToolbarItemWeapon) != null && GridSelectionSystem.WeaponSystem.HasGunsOfId(myToolbarItemWeapon.Definition.Id))
			{
				IMyGunObject<MyDeviceBase> gun = GridSelectionSystem.WeaponSystem.GetGun(myToolbarItemWeapon.Definition.Id);
				MyGunBase myGunBase;
				if (gun != null && (myGunBase = gun.GunBase as MyGunBase) != null && myGunBase.CurrentAmmoMagazineDefinition != null)
				{
					return MyDefinitionManager.Static.GetAmmoDefinition(myGunBase.CurrentAmmoMagazineDefinition.AmmoDefinitionId);
				}
			}
			return null;
		}

		public bool IsTargetLockingEnabled()
		{
			return IsTargetLockingEnabledValue;
		}

		private void OnTargetLockingEnabledChanged(SyncBase obj)
		{
			if (!m_isTargetLockingEnabled)
			{
				SetLockedTarget(null);
			}
		}

		public void SetLockedTarget(VRage.Game.ModAPI.IMyCubeGrid target)
		{
			if (target != null && (BlockDefinition?.TargetLockingEnabled ?? false))
			{
				TargetData = new MyCharacter.MyTargetData
				{
					TargetId = target.EntityId,
					IsTargetLocked = true,
					LockingProgress = 1f,
					ControlledBlockId = base.EntityId
				};
			}
			else
			{
				TargetData = MyCharacter.MyTargetData.EmptyTargetData;
			}
		}

		public MatrixD GetWorldMatrix()
		{
			return base.PositionComp.WorldMatrixRef;
		}

		public bool CanActiveToolShoot()
		{
			MyToolbarItemWeapon myToolbarItemWeapon = Toolbar?.SelectedItem as MyToolbarItemWeapon;
			if (myToolbarItemWeapon == null)
			{
				return false;
			}
			return myToolbarItemWeapon.AmmoCount >= 0;
		}

		public bool IsShipToolSelected()
		{
			if (GridSelectionSystem.CurrentGuns.Count > 0)
			{
				foreach (IMyGunObject<MyDeviceBase> currentGun in GridSelectionSystem.CurrentGuns)
				{
					MyCubeBlock myCubeBlock;
					if ((myCubeBlock = currentGun as MyCubeBlock) != null && !(myCubeBlock.BlockDefinition is MyShipToolDefinition))
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public Vector3D GetActiveToolPosition()
		{
			MyToolbarItemWeapon myToolbarItemWeapon;
			if (Toolbar?.SelectedItem != null && GridSelectionSystem?.WeaponSystem != null && (myToolbarItemWeapon = Toolbar?.SelectedItem as MyToolbarItemWeapon) != null && GridSelectionSystem.WeaponSystem.HasGunsOfId(myToolbarItemWeapon.Definition.Id))
			{
				HashSet<IMyGunObject<MyDeviceBase>> gunsById = GridSelectionSystem.WeaponSystem.GetGunsById(myToolbarItemWeapon.Definition.Id);
				Vector3D zero = Vector3D.Zero;
				foreach (IMyGunObject<MyDeviceBase> item in gunsById)
				{
					zero += item.GetMuzzlePosition();
				}
				return zero / gunsById.Count;
			}
			return base.PositionComp.WorldMatrixRef.Translation;
		}

		void VRage.Game.ModAPI.Interfaces.IMyControllableEntity.DrawHud(IMyCameraController camera, long playerId)
		{
			if (camera != null)
			{
				DrawHud(camera, playerId);
			}
		}

		public Vector3D GetNaturalGravity()
		{
			return MyGravityProviderSystem.CalculateNaturalGravityInPoint(base.WorldMatrix.Translation);
		}

		public Vector3D GetArtificialGravity()
		{
			return MyGravityProviderSystem.CalculateArtificialGravityInPoint(base.WorldMatrix.Translation);
		}

		public Vector3D GetTotalGravity()
		{
			return MyGravityProviderSystem.CalculateTotalGravityInPoint(base.WorldMatrix.Translation);
		}

		double Sandbox.ModAPI.Ingame.IMyShipController.GetShipSpeed()
		{
			MyPhysicsComponentBase myPhysicsComponentBase = ((base.Parent != null) ? base.Parent.Physics : null);
			return ((myPhysicsComponentBase == null) ? Vector3D.Zero : new Vector3D(myPhysicsComponentBase.LinearVelocity)).Length();
		}

		MyShipVelocities Sandbox.ModAPI.Ingame.IMyShipController.GetShipVelocities()
		{
			MyPhysicsComponentBase myPhysicsComponentBase = ((base.Parent != null) ? base.Parent.Physics : null);
			Vector3D linearVelocity = ((myPhysicsComponentBase == null) ? Vector3D.Zero : new Vector3D(myPhysicsComponentBase.LinearVelocity));
			Vector3D angularVelocity = ((myPhysicsComponentBase == null) ? Vector3D.Zero : new Vector3D(myPhysicsComponentBase.AngularVelocity));
			return new MyShipVelocities(linearVelocity, angularVelocity);
		}

		public MyShipMass CalculateShipMass()
		{
			float baseMass;
			float physicalMass;
			float currentMass = base.CubeGrid.GetCurrentMass(out baseMass, out physicalMass);
			return new MyShipMass(baseMass, currentMass, physicalMass);
		}

		bool Sandbox.ModAPI.Ingame.IMyShipController.TryGetPlanetElevation(MyPlanetElevation detail, out double elevation)
		{
			if (!MyGravityProviderSystem.IsPositionInNaturalGravity(base.PositionComp.GetPosition()))
			{
				elevation = double.PositiveInfinity;
				return false;
			}
			BoundingBoxD box = base.PositionComp.WorldAABB;
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(ref box);
			if (closestPlanet == null)
			{
				elevation = double.PositiveInfinity;
				return false;
			}
			switch (detail)
			{
			case MyPlanetElevation.Sealevel:
				elevation = (box.Center - closestPlanet.PositionComp.GetPosition()).Length() - (double)closestPlanet.AverageRadius;
				return true;
			case MyPlanetElevation.Surface:
			{
				Vector3D globalPos = base.CubeGrid.Physics.CenterOfMassWorld;
				Vector3D closestSurfacePointGlobal = closestPlanet.GetClosestSurfacePointGlobal(ref globalPos);
				elevation = Vector3D.Distance(closestSurfacePointGlobal, globalPos);
				return true;
			}
			default:
				throw new ArgumentOutOfRangeException("detail", detail, null);
			}
		}

		bool Sandbox.ModAPI.Ingame.IMyShipController.TryGetPlanetPosition(out Vector3D position)
		{
			if (!MyGravityProviderSystem.IsPositionInNaturalGravity(base.PositionComp.GetPosition()))
			{
				position = Vector3D.Zero;
				return false;
			}
			BoundingBoxD box = base.PositionComp.WorldAABB;
			MyPlanet closestPlanet = MyGamePruningStructure.GetClosestPlanet(ref box);
			if (closestPlanet == null)
			{
				position = Vector3D.Zero;
				return false;
			}
			position = closestPlanet.PositionComp.GetPosition();
			return true;
		}
	}
}
