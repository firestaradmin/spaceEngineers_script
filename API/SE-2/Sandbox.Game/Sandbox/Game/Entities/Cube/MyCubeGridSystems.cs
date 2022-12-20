using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sandbox.Engine.Platform;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Interfaces;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.GameSystems.Electricity;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using VRage;
using VRage.Game;
using VRage.Game.Entities;
<<<<<<< HEAD
using VRage.Game.ModAPI;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using VRage.Groups;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.Entities.Cube
{
	public class MyCubeGridSystems
	{
		private readonly MyCubeGrid m_cubeGrid;

		private Action<MyBlockGroup> m_terminalSystem_GroupAdded;

		private Action<MyBlockGroup> m_terminalSystem_GroupRemoved;

		private bool m_blocksRegistered;

		private readonly HashSet<MyResourceSinkComponent> m_tmpSinks = new HashSet<MyResourceSinkComponent>();

		public Action<long, bool, string> GridPowerStateChanged;

		public const int PowerStateRequestPlayerId_Trash = -1;

		public const int PowerStateRequestPlayerId_SpecialContent = -2;

		public MyGridResourceDistributorSystem ResourceDistributor { get; private set; }

		public MyGridTerminalSystem TerminalSystem { get; private set; }

		public MyGridConveyorSystem ConveyorSystem { get; private set; }

		public MyGridGyroSystem GyroSystem { get; private set; }

		public MyGridWeaponSystem WeaponSystem { get; private set; }

		public MyGridReflectorLightSystem ReflectorLightSystem { get; private set; }

		public MyGridRadioSystem RadioSystem { get; private set; }

		public MyGridWheelSystem WheelSystem { get; private set; }

		public MyGridLandingSystem LandingSystem { get; private set; }

		public MyGroupControlSystem ControlSystem { get; private set; }

		public MyGridCameraSystem CameraSystem { get; private set; }

		public MyShipSoundComponent ShipSoundComponent { get; private set; }

		public MyGridOreDetectorSystem OreDetectorSystem { get; private set; }

		public MyShipMiningSystem MiningSystem { get; private set; }

<<<<<<< HEAD
		/// <summary>
		/// Not null only if Oxygen option is enabled and called on hoster side
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyGridGasSystem GasSystem { get; private set; }

		public MyGridJumpDriveSystem JumpSystem { get; private set; }

		public MyGridEmissiveSystem EmissiveSystem { get; private set; }
<<<<<<< HEAD

		public MyTieredUpdateSystem TieredUpdateSystem { get; private set; }

		public MySolarOcclusionSystem SolarOcclusionSystem { get; private set; }

=======

		public MyTieredUpdateSystem TieredUpdateSystem { get; private set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		protected MyCubeGrid CubeGrid => m_cubeGrid;

		public bool NeedsPerFrameDraw => false;

		public MyCubeGridSystems(MyCubeGrid grid)
		{
			m_cubeGrid = grid;
			m_terminalSystem_GroupAdded = TerminalSystem_GroupAdded;
			m_terminalSystem_GroupRemoved = TerminalSystem_GroupRemoved;
			GyroSystem = new MyGridGyroSystem(m_cubeGrid);
			WeaponSystem = new MyGridWeaponSystem();
			ReflectorLightSystem = new MyGridReflectorLightSystem(m_cubeGrid);
			RadioSystem = new MyGridRadioSystem(m_cubeGrid);
			if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT)
			{
				WheelSystem = new MyGridWheelSystem(m_cubeGrid);
			}
			ConveyorSystem = new MyGridConveyorSystem(m_cubeGrid);
			LandingSystem = new MyGridLandingSystem();
			ControlSystem = new MyGroupControlSystem();
			CameraSystem = new MyGridCameraSystem(m_cubeGrid);
			OreDetectorSystem = new MyGridOreDetectorSystem(m_cubeGrid);
			if (Sync.IsServer && !grid.IsPreview)
			{
				TieredUpdateSystem = new MyTieredUpdateSystem(m_cubeGrid);
			}
<<<<<<< HEAD
			if (Sync.IsServer && !grid.IsPreview)
			{
				SolarOcclusionSystem = new MySolarOcclusionSystem(m_cubeGrid);
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (Sync.IsServer && MySession.Static.Settings.EnableOxygen && MySession.Static.Settings.EnableOxygenPressurization)
			{
				GasSystem = new MyGridGasSystem(m_cubeGrid);
			}
			if (MyPerGameSettings.EnableJumpDrive)
			{
				JumpSystem = new MyGridJumpDriveSystem(m_cubeGrid);
			}
			if (MyPerGameSettings.EnableShipSoundSystem && (MyFakes.ENABLE_NEW_SMALL_SHIP_SOUNDS || MyFakes.ENABLE_NEW_LARGE_SHIP_SOUNDS) && !Sandbox.Engine.Platform.Game.IsDedicated)
			{
				ShipSoundComponent = new MyShipSoundComponent();
			}
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				EmissiveSystem = new MyGridEmissiveSystem(m_cubeGrid);
			}
			m_blocksRegistered = true;
		}

		public virtual void Init(MyObjectBuilder_CubeGrid builder)
		{
			MyEntityThrustComponent myEntityThrustComponent = CubeGrid.Components.Get<MyEntityThrustComponent>();
			if (myEntityThrustComponent != null)
			{
				myEntityThrustComponent.DampenersEnabled = builder.DampenersEnabled;
			}
			if (WheelSystem != null)
			{
				WheelSystem.HandBrake = builder.Handbrake;
			}
			if (GasSystem != null && MySession.Static.Settings.EnableOxygen && MySession.Static.Settings.EnableOxygenPressurization)
			{
				GasSystem.Init(builder.OxygenRooms);
			}
			if (ShipSoundComponent != null)
			{
				CubeGrid.Components.Add(ShipSoundComponent);
				if (!ShipSoundComponent.InitComponent(m_cubeGrid))
				{
					ShipSoundComponent.DestroyComponent();
					ShipSoundComponent = null;
				}
			}
			if (MyPerGameSettings.EnableJumpDrive)
			{
				JumpSystem.Init(builder.JumpDriveDirection, builder.JumpRemainingTime);
			}
			CubeGrid.Components.Get<MyEntityThrustComponent>()?.MergeAllGroupsDirty();
		}

		public virtual void BeforeBlockDeserialization(MyObjectBuilder_CubeGrid builder)
		{
			ConveyorSystem.BeforeBlockDeserialization(builder.ConveyorLines);
		}

		public virtual void AfterBlockDeserialization()
		{
			ConveyorSystem.AfterBlockDeserialization();
			ConveyorSystem.ResourceSink.Update();
		}

		public void UpdateBeforeSimulation()
		{
		}

		public virtual void PrepareForDraw()
		{
		}

		public void UpdatePower()
		{
			if (ResourceDistributor != null)
			{
				ResourceDistributor.UpdateBeforeSimulation();
			}
		}

		public virtual void UpdateOnceBeforeFrame()
		{
			EmissiveSystem?.UpdateEmissivity();
		}

		public virtual void UpdateBeforeSimulation10()
		{
			if (CubeGrid.Closed)
			{
				MyLog.Default.Error("Updating closed Entity!");
				return;
			}
			CameraSystem.UpdateBeforeSimulation10();
			ConveyorSystem.UpdateBeforeSimulation10();
			MiningSystem?.UpdateBeforeSimulation10();
		}

		public virtual void UpdateBeforeSimulation100()
		{
			if (ControlSystem != null)
			{
				ControlSystem.UpdateBeforeSimulation100();
			}
			if (ShipSoundComponent != null)
			{
				ShipSoundComponent.Update100();
			}
			if (ResourceDistributor != null)
			{
				ResourceDistributor.UpdateBeforeSimulation100();
			}
			EmissiveSystem?.UpdateBeforeSimulation100();
		}

		public virtual void UpdateAfterSimulation()
		{
		}

		public virtual void UpdateAfterSimulation100()
		{
			ConveyorSystem.UpdateAfterSimulation100();
			TieredUpdateSystem?.UpdateAfterSimulation100();
		}

		public virtual void GetObjectBuilder(MyObjectBuilder_CubeGrid ob)
		{
			ob.DampenersEnabled = CubeGrid.Components.Get<MyEntityThrustComponent>()?.DampenersEnabled ?? true;
			ConveyorSystem.SerializeLines(ob.ConveyorLines);
			if (ob.ConveyorLines.Count == 0)
			{
				ob.ConveyorLines = null;
			}
			if (WheelSystem != null)
			{
				ob.Handbrake = WheelSystem.HandBrake;
			}
			if (GasSystem != null && MySession.Static.Settings.EnableOxygen && MySession.Static.Settings.EnableOxygenPressurization)
			{
				ob.OxygenRooms = GasSystem.GetOxygenAmount();
			}
			if (MyPerGameSettings.EnableJumpDrive)
			{
				ob.JumpDriveDirection = JumpSystem.GetJumpDriveDirection();
				ob.JumpRemainingTime = JumpSystem.GetRemainingJumpTime();
			}
		}

		public virtual void AddGroup(MyBlockGroup group)
		{
			if (TerminalSystem != null)
			{
				TerminalSystem.AddUpdateGroup(group, fireEvent: false);
			}
		}

		public virtual void RemoveGroup(MyBlockGroup group)
		{
			if (TerminalSystem != null)
			{
				TerminalSystem.RemoveGroup(group, fireEvent: false);
			}
		}

		public virtual void OnAddedToGroup(MyGridLogicalGroupData group)
		{
			TerminalSystem = group.TerminalSystem;
			ResourceDistributor = group.ResourceDistributor;
			WeaponSystem = group.WeaponSystem;
			if (string.IsNullOrEmpty(ResourceDistributor.DebugName))
			{
				ResourceDistributor.DebugName = m_cubeGrid.ToString();
			}
			m_cubeGrid.OnFatBlockAdded += ResourceDistributor.CubeGrid_OnFatBlockAddedOrRemoved;
			m_cubeGrid.OnFatBlockRemoved += ResourceDistributor.CubeGrid_OnFatBlockAddedOrRemoved;
			ResourceDistributor.AddSink(GyroSystem.ResourceSink);
			ResourceDistributor.AddSink(ConveyorSystem.ResourceSink);
			if (EmissiveSystem != null)
			{
				ResourceDistributor.AddSink(EmissiveSystem.ResourceSink);
			}
			ConveyorSystem.ResourceSink.IsPoweredChanged += ResourceDistributor.ConveyorSystem_OnPoweredChanged;
			foreach (MyBlockGroup blockGroup in m_cubeGrid.BlockGroups)
			{
				TerminalSystem.AddUpdateGroup(blockGroup, fireEvent: false);
			}
			TerminalSystem.GroupAdded += m_terminalSystem_GroupAdded;
			TerminalSystem.GroupRemoved += m_terminalSystem_GroupRemoved;
			foreach (MyCubeBlock fatBlock in m_cubeGrid.GetFatBlocks())
			{
				if (!fatBlock.MarkedForClose)
				{
					MyTerminalBlock myTerminalBlock = fatBlock as MyTerminalBlock;
					if (myTerminalBlock != null)
					{
						TerminalSystem.Add(myTerminalBlock);
					}
					MyResourceSourceComponent myResourceSourceComponent = fatBlock.Components.Get<MyResourceSourceComponent>();
					if (myResourceSourceComponent != null)
					{
						ResourceDistributor.AddSource(myResourceSourceComponent);
					}
					MyResourceSinkComponent myResourceSinkComponent = fatBlock.Components.Get<MyResourceSinkComponent>();
					if (myResourceSinkComponent != null)
					{
						ResourceDistributor.AddSink(myResourceSinkComponent);
					}
					IMyRechargeSocketOwner myRechargeSocketOwner = fatBlock as IMyRechargeSocketOwner;
					if (myRechargeSocketOwner != null)
					{
						myRechargeSocketOwner.RechargeSocket.ResourceDistributor = group.ResourceDistributor;
					}
					RegisterWeaponBlock(fatBlock);
				}
			}
<<<<<<< HEAD
			ResourceDistributor.OnPowerGenerationChanged += OnPowerGenerationChanged;
=======
			MyGridResourceDistributorSystem resourceDistributor = ResourceDistributor;
			resourceDistributor.OnPowerGenerationChanged = (Action<bool>)Delegate.Combine(resourceDistributor.OnPowerGenerationChanged, new Action<bool>(OnPowerGenerationChanged));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			TerminalSystem.BlockManipulationFinishedFunction();
			ResourceDistributor.UpdateBeforeSimulation();
			m_cubeGrid.Schedule(MyCubeGrid.UpdateQueue.OnceBeforeSimulation, delegate
			{
				ResourceDistributor.SetNeedRecomputeAll();
				ResourceDistributor.UpdateBeforeSimulation();
			});
		}

		public virtual void OnRemovedFromGroup(MyGridLogicalGroupData group)
		{
			if (m_blocksRegistered)
			{
				TerminalSystem.GroupAdded -= m_terminalSystem_GroupAdded;
				TerminalSystem.GroupRemoved -= m_terminalSystem_GroupRemoved;
				foreach (MyBlockGroup blockGroup in m_cubeGrid.BlockGroups)
				{
					TerminalSystem.RemoveGroup(blockGroup, fireEvent: false);
				}
				foreach (MyCubeBlock fatBlock in m_cubeGrid.GetFatBlocks())
				{
					MyTerminalBlock myTerminalBlock = fatBlock as MyTerminalBlock;
					if (myTerminalBlock != null)
					{
						TerminalSystem.Remove(myTerminalBlock);
					}
					MyResourceSourceComponent myResourceSourceComponent = fatBlock.Components.Get<MyResourceSourceComponent>();
					if (myResourceSourceComponent != null)
					{
						ResourceDistributor.RemoveSource(myResourceSourceComponent);
					}
					MyResourceSinkComponent myResourceSinkComponent = fatBlock.Components.Get<MyResourceSinkComponent>();
					if (myResourceSinkComponent != null)
					{
						ResourceDistributor.RemoveSink(myResourceSinkComponent, resetSinkInput: false, fatBlock.MarkedForClose);
					}
					IMyRechargeSocketOwner myRechargeSocketOwner = fatBlock as IMyRechargeSocketOwner;
					if (myRechargeSocketOwner != null)
					{
						myRechargeSocketOwner.RechargeSocket.ResourceDistributor = null;
					}
					UnregisterWeaponBlock(fatBlock);
				}
				TerminalSystem.BlockManipulationFinishedFunction();
			}
			ConveyorSystem.ResourceSink.IsPoweredChanged -= ResourceDistributor.ConveyorSystem_OnPoweredChanged;
			group.ResourceDistributor.RemoveSink(ConveyorSystem.ResourceSink, resetSinkInput: false);
			group.ResourceDistributor.RemoveSink(GyroSystem.ResourceSink, resetSinkInput: false);
			if (EmissiveSystem != null)
			{
				group.ResourceDistributor.RemoveSink(EmissiveSystem.ResourceSink, resetSinkInput: false);
			}
			group.ResourceDistributor.UpdateBeforeSimulation();
			m_cubeGrid.OnFatBlockAdded -= ResourceDistributor.CubeGrid_OnFatBlockAddedOrRemoved;
			m_cubeGrid.OnFatBlockRemoved -= ResourceDistributor.CubeGrid_OnFatBlockAddedOrRemoved;
<<<<<<< HEAD
			ResourceDistributor.OnPowerGenerationChanged -= OnPowerGenerationChanged;
=======
			MyGridResourceDistributorSystem resourceDistributor = ResourceDistributor;
			resourceDistributor.OnPowerGenerationChanged = (Action<bool>)Delegate.Remove(resourceDistributor.OnPowerGenerationChanged, new Action<bool>(OnPowerGenerationChanged));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			ResourceDistributor = null;
			TerminalSystem = null;
			WeaponSystem = null;
		}

		private void OnPowerGenerationChanged(bool powerIsGenerated)
		{
			if (MyVisualScriptLogicProvider.GridPowerGenerationStateChanged != null)
			{
				MyVisualScriptLogicProvider.GridPowerGenerationStateChanged(CubeGrid.EntityId, CubeGrid.Name, powerIsGenerated);
			}
			if (GridPowerStateChanged != null)
			{
				GridPowerStateChanged(CubeGrid.EntityId, powerIsGenerated, CubeGrid.Name);
			}
		}

		public void OnAddedToGroup(MyGridPhysicalGroupData group)
		{
			ControlSystem = group.ControlSystem;
			foreach (MyShipController fatBlock in m_cubeGrid.GetFatBlocks<MyShipController>())
			{
				if (fatBlock != null && (fatBlock.ControllerInfo.Controller != null || (fatBlock.Pilot != null && MySessionComponentReplay.Static.HasEntityReplayData(CubeGrid.EntityId))) && fatBlock.EnableShipControl && (!(fatBlock is MyCockpit) || fatBlock.IsMainCockpit || !fatBlock.CubeGrid.HasMainCockpit()))
				{
					ControlSystem.AddControllerBlock(fatBlock);
				}
			}
			ControlSystem.AddGrid(CubeGrid);
		}

		public void OnRemovedFromGroup(MyGridPhysicalGroupData group)
		{
			if (m_blocksRegistered)
			{
				foreach (MyShipController fatBlock in m_cubeGrid.GetFatBlocks<MyShipController>())
				{
					if (fatBlock != null && fatBlock.ControllerInfo.Controller != null && fatBlock.EnableShipControl && (!(fatBlock is MyCockpit) || fatBlock.IsMainCockpit || !fatBlock.CubeGrid.HasMainCockpit()))
					{
						ControlSystem.RemoveControllerBlock(fatBlock);
					}
				}
			}
			ControlSystem.RemoveGrid(CubeGrid);
			ControlSystem = null;
		}

		public virtual void BeforeGridClose()
		{
			ConveyorSystem.IsClosing = true;
			ReflectorLightSystem.IsClosing = true;
			RadioSystem.IsClosing = true;
			if (ShipSoundComponent != null)
			{
				ShipSoundComponent.DestroyComponent();
				ShipSoundComponent = null;
			}
			if (GasSystem != null)
			{
				GasSystem.OnGridClosing();
			}
		}

		public virtual void AfterGridClose()
		{
			ConveyorSystem.AfterGridClose();
			if (MyPerGameSettings.EnableJumpDrive)
			{
				JumpSystem.AfterGridClose();
			}
			m_blocksRegistered = false;
			if (ControlSystem != null)
			{
				ControlSystem.RemoveGrid(CubeGrid);
			}
			GasSystem = null;
		}

		public virtual void DebugDraw()
		{
			if (MyDebugDrawSettings.DEBUG_DRAW_GRID_TERMINAL_SYSTEMS)
			{
				MyRenderProxy.DebugDrawText3D(m_cubeGrid.WorldMatrix.Translation, TerminalSystem.GetHashCode().ToString(), Color.NavajoWhite, 1f, depthRead: false);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_CONVEYORS)
			{
				ConveyorSystem.DebugDraw(m_cubeGrid);
				ConveyorSystem.DebugDrawLinePackets();
			}
			if (GyroSystem != null && MyDebugDrawSettings.DEBUG_DRAW_GYROS)
			{
				GyroSystem.DebugDraw();
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_RESOURCE_RECEIVERS && ResourceDistributor != null)
			{
				ResourceDistributor.DebugDraw(m_cubeGrid);
			}
			if (MyDebugDrawSettings.DEBUG_DRAW_BLOCK_GROUPS && TerminalSystem != null)
			{
				TerminalSystem.DebugDraw(m_cubeGrid);
			}
			if (MySession.Static != null && GasSystem != null && MySession.Static.Settings.EnableOxygen && MySession.Static.Settings.EnableOxygenPressurization && MyDebugDrawSettings.DEBUG_DRAW_OXYGEN)
			{
				GasSystem.DebugDraw();
			}
			MiningSystem?.DebugDraw();
		}

		public virtual bool IsTrash()
		{
			if (ResourceDistributor.ResourceState != MyResourceStateEnum.NoPower)
			{
				return false;
			}
			if (ControlSystem.IsControlled)
			{
				return false;
			}
			return true;
		}

		public virtual void RegisterInSystems(MyCubeBlock block)
		{
			if (block.GetType() != typeof(MyCubeBlock))
			{
				if (ResourceDistributor != null)
				{
					MyResourceSourceComponent myResourceSourceComponent = block.Components.Get<MyResourceSourceComponent>();
					if (myResourceSourceComponent != null)
					{
						ResourceDistributor.AddSource(myResourceSourceComponent);
					}
					MyResourceSinkComponent myResourceSinkComponent = block.Components.Get<MyResourceSinkComponent>();
					if (!(block is MyThrust) && myResourceSinkComponent != null)
					{
						ResourceDistributor.AddSink(myResourceSinkComponent);
					}
					IMyRechargeSocketOwner myRechargeSocketOwner = block as IMyRechargeSocketOwner;
					if (myRechargeSocketOwner != null)
					{
						myRechargeSocketOwner.RechargeSocket.ResourceDistributor = ResourceDistributor;
					}
				}
				if (WeaponSystem != null)
				{
					RegisterWeaponBlock(block);
				}
				IMySolarOccludable occ;
				if (SolarOcclusionSystem != null && (occ = block as IMySolarOccludable) != null)
				{
					SolarOcclusionSystem.RegisterOccludable(occ);
				}
				if (TerminalSystem != null)
				{
					MyTerminalBlock myTerminalBlock = block as MyTerminalBlock;
					if (myTerminalBlock != null)
					{
						TerminalSystem.Add(myTerminalBlock);
					}
				}
				MyCubeBlock myCubeBlock = ((block != null && block.HasInventory) ? block : null);
				if (myCubeBlock != null)
				{
					ConveyorSystem.Add(myCubeBlock);
				}
				IMyConveyorEndpointBlock myConveyorEndpointBlock = block as IMyConveyorEndpointBlock;
				if (myConveyorEndpointBlock != null)
				{
					myConveyorEndpointBlock.InitializeConveyorEndpoint();
					ConveyorSystem.AddConveyorBlock(myConveyorEndpointBlock);
				}
				IMyConveyorSegmentBlock myConveyorSegmentBlock = block as IMyConveyorSegmentBlock;
				if (myConveyorSegmentBlock != null)
				{
					myConveyorSegmentBlock.InitializeConveyorSegment();
					ConveyorSystem.AddSegmentBlock(myConveyorSegmentBlock);
				}
				MyReflectorLight myReflectorLight = block as MyReflectorLight;
				if (myReflectorLight != null)
				{
					ReflectorLightSystem.Register(myReflectorLight);
				}
				if (block.Components.Contains(typeof(MyDataBroadcaster)))
				{
					MyDataBroadcaster broadcaster = block.Components.Get<MyDataBroadcaster>();
					RadioSystem.Register(broadcaster);
				}
				if (block.Components.Contains(typeof(MyDataReceiver)))
				{
					MyDataReceiver reciever = block.Components.Get<MyDataReceiver>();
					RadioSystem.Register(reciever);
				}
				if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT)
				{
					MyMotorSuspension myMotorSuspension = block as MyMotorSuspension;
					if (myMotorSuspension != null)
					{
						WheelSystem.Register(myMotorSuspension);
					}
				}
				IMyTieredUpdateBlock myTieredUpdateBlock = block as IMyTieredUpdateBlock;
				if (myTieredUpdateBlock != null && TieredUpdateSystem != null && myTieredUpdateBlock.IsTieredUpdateSupported)
				{
					TieredUpdateSystem.Register(myTieredUpdateBlock, block.EntityId);
				}
				IMyLandingGear myLandingGear = block as IMyLandingGear;
				if (myLandingGear != null)
				{
					LandingSystem.Register(myLandingGear);
				}
				MyGyro myGyro = block as MyGyro;
				if (myGyro != null)
				{
					GyroSystem.Register(myGyro);
				}
				MyCameraBlock myCameraBlock = block as MyCameraBlock;
				if (myCameraBlock != null)
				{
					CameraSystem.Register(myCameraBlock);
				}
				if (EmissiveSystem != null)
				{
					MyEmissiveBlock myEmissiveBlock = block as MyEmissiveBlock;
					if (myEmissiveBlock != null)
					{
						EmissiveSystem.Register(myEmissiveBlock);
					}
				}
			}
			block.OnRegisteredToGridSystems();
		}

		public virtual void UnregisterFromSystems(MyCubeBlock block)
		{
			if (block.GetType() != typeof(MyCubeBlock))
			{
				if (ResourceDistributor != null)
				{
					MyResourceSourceComponent myResourceSourceComponent = block.Components.Get<MyResourceSourceComponent>();
					if (myResourceSourceComponent != null)
					{
						ResourceDistributor.RemoveSource(myResourceSourceComponent);
					}
					MyResourceSinkComponent myResourceSinkComponent = block.Components.Get<MyResourceSinkComponent>();
					if (myResourceSinkComponent != null)
					{
						ResourceDistributor.RemoveSink(myResourceSinkComponent);
					}
					IMyRechargeSocketOwner myRechargeSocketOwner = block as IMyRechargeSocketOwner;
					if (myRechargeSocketOwner != null)
					{
						myRechargeSocketOwner.RechargeSocket.ResourceDistributor = null;
					}
				}
				if (WeaponSystem != null)
				{
					UnregisterWeaponBlock(block);
				}
				IMySolarOccludable occ;
				if (SolarOcclusionSystem != null && (occ = block as IMySolarOccludable) != null)
				{
					SolarOcclusionSystem.UnregisterOccludable(occ);
				}
				if (TerminalSystem != null)
				{
					MyTerminalBlock myTerminalBlock = block as MyTerminalBlock;
					if (myTerminalBlock != null)
					{
						TerminalSystem.Remove(myTerminalBlock);
					}
				}
				if (block.HasInventory)
				{
					ConveyorSystem.Remove(block);
				}
				IMyConveyorEndpointBlock myConveyorEndpointBlock = block as IMyConveyorEndpointBlock;
				if (myConveyorEndpointBlock != null)
				{
					ConveyorSystem.RemoveConveyorBlock(myConveyorEndpointBlock);
				}
				IMyConveyorSegmentBlock myConveyorSegmentBlock = block as IMyConveyorSegmentBlock;
				if (myConveyorSegmentBlock != null)
				{
					ConveyorSystem.RemoveSegmentBlock(myConveyorSegmentBlock);
				}
				MyReflectorLight myReflectorLight = block as MyReflectorLight;
				if (myReflectorLight != null)
				{
					ReflectorLightSystem.Unregister(myReflectorLight);
				}
				MyDataBroadcaster myDataBroadcaster = block.Components.Get<MyDataBroadcaster>();
				if (myDataBroadcaster != null)
				{
					RadioSystem.Unregister(myDataBroadcaster);
				}
				MyDataReceiver myDataReceiver = block.Components.Get<MyDataReceiver>();
				if (myDataReceiver != null)
				{
					RadioSystem.Unregister(myDataReceiver);
				}
				if (MyFakes.ENABLE_WHEEL_CONTROLS_IN_COCKPIT)
				{
					MyMotorSuspension myMotorSuspension = block as MyMotorSuspension;
					if (myMotorSuspension != null)
					{
						WheelSystem.Unregister(myMotorSuspension);
					}
				}
				IMyTieredUpdateBlock myTieredUpdateBlock = block as IMyTieredUpdateBlock;
				if (myTieredUpdateBlock != null && TieredUpdateSystem != null && myTieredUpdateBlock.IsTieredUpdateSupported)
				{
					TieredUpdateSystem.Unregister(myTieredUpdateBlock, block.EntityId);
				}
				IMyLandingGear myLandingGear = block as IMyLandingGear;
				if (myLandingGear != null)
				{
					LandingSystem.Unregister(myLandingGear);
				}
				MyGyro myGyro = block as MyGyro;
				if (myGyro != null)
				{
					GyroSystem.Unregister(myGyro);
				}
				MyCameraBlock myCameraBlock = block as MyCameraBlock;
				if (myCameraBlock != null)
				{
					CameraSystem.Unregister(myCameraBlock);
				}
				if (EmissiveSystem != null)
				{
					MyEmissiveBlock myEmissiveBlock = block as MyEmissiveBlock;
					if (myEmissiveBlock != null)
					{
						EmissiveSystem.Unregister(myEmissiveBlock);
					}
				}
			}
			block.OnUnregisteredFromGridSystems();
		}

		public void SyncObject_PowerProducerStateChanged(MyMultipleEnabledEnum enabledState, long playerId, bool localGridOnly = false)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			if (Sync.IsServer)
			{
				List<MyCubeGrid> list = new List<MyCubeGrid>();
				if (localGridOnly)
				{
<<<<<<< HEAD
					MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Group group = MyCubeGridGroups.Static.Mechanical.GetGroup(CubeGrid);
					if (group != null)
					{
						foreach (MyGroups<MyCubeGrid, MyGridMechanicalGroupData>.Node node in group.Nodes)
						{
							list.Add(node.NodeData);
						}
					}
				}
				else
				{
					MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Group group2 = MyCubeGridGroups.Static.Logical.GetGroup(CubeGrid);
					if (group2 != null)
					{
						foreach (MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node node2 in group2.Nodes)
						{
							list.Add(node2.NodeData);
						}
					}
				}
				foreach (MyCubeGrid item in list)
				{
					foreach (MyCubeBlock fatBlock in item.GetFatBlocks())
					{
						IMyPowerProducer myPowerProducer;
						if ((myPowerProducer = fatBlock as IMyPowerProducer) == null)
						{
							continue;
						}
						bool flag = false;
						if (playerId >= 0)
						{
							MyFunctionalBlock myFunctionalBlock = fatBlock as MyFunctionalBlock;
							if (myFunctionalBlock != null && myFunctionalBlock.HasPlayerAccess(playerId))
							{
								flag = true;
							}
						}
						else
						{
							switch (playerId)
							{
							case -1L:
								flag = true;
								break;
							case -2L:
							{
								string text = (fatBlock as MyTerminalBlock).CustomName.ToString();
								if (text == "Special Content Power" || text == "Special Content")
								{
									flag = true;
								}
								break;
							}
=======
					Enumerator<MyGroups<MyCubeGrid, MyGridLogicalGroupData>.Node> enumerator = group.Nodes.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							MyCubeGrid nodeData = enumerator.get_Current().NodeData;
							if (nodeData == null || nodeData.Physics == null || nodeData.Physics.Shape == null)
							{
								continue;
							}
							foreach (MyCubeBlock fatBlock in nodeData.GetFatBlocks())
							{
								IMyPowerProducer myPowerProducer = fatBlock as IMyPowerProducer;
								if (myPowerProducer == null)
								{
									continue;
								}
								bool flag = false;
								if (playerId >= 0)
								{
									MyFunctionalBlock myFunctionalBlock = fatBlock as MyFunctionalBlock;
									if (myFunctionalBlock != null && myFunctionalBlock.HasPlayerAccess(playerId))
									{
										flag = true;
									}
								}
								else
								{
									switch (playerId)
									{
									case -1L:
										flag = true;
										break;
									case -2L:
									{
										string text = (fatBlock as MyTerminalBlock).CustomName.ToString();
										if (text == "Special Content Power" || text == "Special Content")
										{
											flag = true;
										}
										break;
									}
									}
								}
								if (flag)
								{
									myPowerProducer.Enabled = enabledState == MyMultipleEnabledEnum.AllEnabled;
								}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
							}
						}
						if (flag)
						{
							myPowerProducer.Enabled = enabledState == MyMultipleEnabledEnum.AllEnabled;
						}
					}
<<<<<<< HEAD
					item.GridSystems?.ResourceDistributor?.ChangeSourcesState(MyResourceDistributorComponent.ElectricityId, enabledState, playerId, item);
=======
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			CubeGrid.ActivatePhysics();
		}

		private void TerminalSystem_GroupRemoved(MyBlockGroup group)
		{
			MyBlockGroup myBlockGroup = m_cubeGrid.BlockGroups.Find((MyBlockGroup x) => x.Name.CompareTo(group.Name) == 0);
			if (myBlockGroup != null)
			{
				myBlockGroup.Blocks.Clear();
				m_cubeGrid.BlockGroups.Remove(myBlockGroup);
				m_cubeGrid.ModifyGroup(myBlockGroup);
			}
		}

		private void TerminalSystem_GroupAdded(MyBlockGroup group)
		{
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			MyBlockGroup myBlockGroup = m_cubeGrid.BlockGroups.Find((MyBlockGroup x) => x.Name.CompareTo(group.Name) == 0);
<<<<<<< HEAD
			if (group.Blocks.FirstOrDefault((MyTerminalBlock x) => m_cubeGrid.GetFatBlocks().IndexOf(x) != -1) == null)
=======
			if (Enumerable.FirstOrDefault<MyTerminalBlock>((IEnumerable<MyTerminalBlock>)group.Blocks, (Func<MyTerminalBlock, bool>)((MyTerminalBlock x) => m_cubeGrid.GetFatBlocks().IndexOf(x) != -1)) == null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return;
			}
			if (myBlockGroup == null)
			{
				myBlockGroup = new MyBlockGroup();
				myBlockGroup.Name.AppendStringBuilder(group.Name);
				m_cubeGrid.BlockGroups.Add(myBlockGroup);
			}
			myBlockGroup.Blocks.Clear();
<<<<<<< HEAD
			foreach (MyTerminalBlock block in group.Blocks)
			{
				if (block.CubeGrid == m_cubeGrid)
				{
					myBlockGroup.Blocks.Add(block);
				}
			}
=======
			Enumerator<MyTerminalBlock> enumerator = group.Blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyTerminalBlock current = enumerator.get_Current();
					if (current.CubeGrid == m_cubeGrid)
					{
						myBlockGroup.Blocks.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_cubeGrid.ModifyGroup(myBlockGroup);
		}

		public virtual void OnBlockAdded(MySlimBlock block)
		{
			if (block.FatBlock is MyThrust)
			{
				if (ShipSoundComponent != null)
				{
					ShipSoundComponent.ShipHasChanged = true;
				}
				CubeGrid.Components.Get<MyEntityThrustComponent>()?.MarkDirty();
			}
			if (ConveyorSystem != null && (block.FatBlock is IMyConveyorEndpointBlock || block.FatBlock is IMyConveyorTube))
			{
				ConveyorSystem.UpdateLines();
			}
		}

		public virtual void OnBlockRemoved(MySlimBlock block)
		{
			if (block.FatBlock is MyThrust)
			{
				if (ShipSoundComponent != null)
				{
					ShipSoundComponent.ShipHasChanged = true;
				}
				CubeGrid.Components.Get<MyEntityThrustComponent>()?.MarkDirty();
			}
			if (ConveyorSystem != null && (block.FatBlock is IMyConveyorEndpointBlock || block.FatBlock is IMyConveyorTube))
			{
				ConveyorSystem.UpdateLines();
			}
		}

		public virtual void OnBlockIntegrityChanged(MySlimBlock block)
		{
		}

		public virtual void OnBlockOwnershipChanged(MyCubeGrid cubeGrid)
		{
			ConveyorSystem.FlagForRecomputation();
		}

		public MyShipMiningSystem GetOrCreateMiningSystem()
		{
			MiningSystem = MiningSystem ?? new MyShipMiningSystem(CubeGrid);
			return MiningSystem;
		}
<<<<<<< HEAD

		private void UnregisterWeaponBlock(MyCubeBlock block)
		{
			IMyGunObject<MyDeviceBase> myGunObject = block as IMyGunObject<MyDeviceBase>;
			if (myGunObject != null)
			{
				WeaponSystem.Unregister(myGunObject);
			}
			myGunObject = block.GameLogic?.GetAs<IMyGunObject<MyDeviceBase>>();
			if (myGunObject != null)
			{
				WeaponSystem.Unregister(myGunObject);
			}
			if (block.Components.TryGet<IMyGunObject<MyDeviceBase>>(out var component))
			{
				WeaponSystem.Unregister(component);
			}
		}

		private void RegisterWeaponBlock(MyCubeBlock block)
		{
			if (block.Components.TryGet<IMyGunObject<MyDeviceBase>>(out var component))
			{
				WeaponSystem.Register(component);
				return;
			}
			IMyGunObject<MyDeviceBase> myGunObject = block.GameLogic?.GetAs<IMyGunObject<MyDeviceBase>>();
			if (myGunObject != null)
			{
				WeaponSystem.Register(myGunObject);
				return;
			}
			myGunObject = block as IMyGunObject<MyDeviceBase>;
			if (myGunObject != null)
			{
				WeaponSystem.Register(myGunObject);
			}
		}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
