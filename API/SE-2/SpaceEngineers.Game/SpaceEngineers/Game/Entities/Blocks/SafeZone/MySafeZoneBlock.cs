using System;
using System.Collections.Generic;
using System.Text;
using ObjectBuilders.SafeZone;
using Sandbox;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Engine.Platform;
using Sandbox.Game;
using Sandbox.Game.Components;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.EntityComponents;
using Sandbox.Game.GameSystems.Conveyors;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using SpaceEngineers.Game.Definitions.SafeZone;
using SpaceEngineers.Game.ModAPI;
using SpaceEngineers.Game.ModAPI.Ingame;
using VRage;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.Entity;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Components;
using VRage.Game.ObjectBuilders.Components.Beacon;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace SpaceEngineers.Game.Entities.Blocks.SafeZone
{
	[MyCubeBlockType(typeof(MyObjectBuilder_SafeZoneBlock))]
	[MyTerminalInterface(new Type[]
	{
		typeof(SpaceEngineers.Game.ModAPI.IMySafeZoneBlock),
		typeof(SpaceEngineers.Game.ModAPI.Ingame.IMySafeZoneBlock)
	})]
	public class MySafeZoneBlock : MyFunctionalBlock, IMyConveyorEndpointBlock, SpaceEngineers.Game.ModAPI.IMySafeZoneBlock, Sandbox.ModAPI.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyFunctionalBlock, Sandbox.ModAPI.Ingame.IMyTerminalBlock, VRage.Game.ModAPI.Ingame.IMyCubeBlock, VRage.Game.ModAPI.Ingame.IMyEntity, Sandbox.ModAPI.IMyTerminalBlock, VRage.Game.ModAPI.IMyCubeBlock, VRage.ModAPI.IMyEntity, SpaceEngineers.Game.ModAPI.Ingame.IMySafeZoneBlock, IMyMultiTextPanelComponentOwner, IMyTextPanelComponentOwner, Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider
	{
		/// <summary>
		/// Manager (entity component) that handles all operations on safezone
		/// </summary>
		private MySafeZoneComponent m_safeZoneManager;

		/// <summary>
		/// Conveyor end point used for injecting zone chips to the inventory.
		/// </summary>
		private MyMultilineConveyorEndpoint m_conveyorEndpoint;

		private bool m_immediatelyEnableSafezone;

		protected MySoundPair m_processSound = new MySoundPair();

		protected bool m_isSoundRunning;

		private MySessionComponentDLC m_dlcComponent;

		private bool m_readyToRecieveEvents;

		internal MySafeZoneBlockDefinition Definition => (MySafeZoneBlockDefinition)base.BlockDefinition;

		public IMyConveyorEndpoint ConveyorEndpoint => m_conveyorEndpoint;

		public MySafeZoneBlock()
		{
			base.Render = new MyRenderComponentScreenAreas(this);
		}

		public override void Init(MyObjectBuilder_CubeBlock objectBuilder, MyCubeGrid cubeGrid)
		{
			MyObjectBuilder_SafeZoneBlock myObjectBuilder_SafeZoneBlock = objectBuilder as MyObjectBuilder_SafeZoneBlock;
			m_safeZoneManager = new MySafeZoneComponent();
			base.Components.Add(m_safeZoneManager);
			m_safeZoneManager.Init(this, myObjectBuilder_SafeZoneBlock.SafeZoneId);
			m_dlcComponent = MySession.Static.GetComponent<MySessionComponentDLC>();
			MyResourceSinkComponent myResourceSinkComponent = new MyResourceSinkComponent();
<<<<<<< HEAD
			myResourceSinkComponent.Init(MyStringHash.GetOrCompute(Definition.ResourceSinkGroup), Definition.MaxSafeZonePowerDrainkW, UpdatePowerInput, this);
=======
			myResourceSinkComponent.Init(MyStringHash.GetOrCompute(Definition.ResourceSinkGroup), Definition.MaxSafeZonePowerDrainkW, UpdatePowerInput);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			base.ResourceSink = myResourceSinkComponent;
			base.ResourceSink.IsPoweredChanged += Receiver_IsPoweredChanged;
			base.ResourceSink.Update();
			CheckObjectBuilders(objectBuilder as MyObjectBuilder_SafeZoneBlock, Definition);
			base.Init(objectBuilder, cubeGrid);
			m_processSound = base.BlockDefinition.ActionSound;
<<<<<<< HEAD
			UpdateEffectsAndText();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		private void CheckObjectBuilders(MyObjectBuilder_SafeZoneBlock builder, MySafeZoneBlockDefinition definition)
		{
			if (builder == null || definition == null || builder.ComponentContainer == null)
			{
				return;
			}
			foreach (MyObjectBuilder_ComponentContainer.ComponentData component in builder.ComponentContainer.Components)
=======
			if (Definition.ScreenAreas != null && Definition.ScreenAreas.Count > 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MyObjectBuilder_SafeZoneComponent myObjectBuilder_SafeZoneComponent;
				MyObjectBuilder_SafeZone myObjectBuilder_SafeZone;
				if ((myObjectBuilder_SafeZoneComponent = component.Component as MyObjectBuilder_SafeZoneComponent) != null && myObjectBuilder_SafeZoneComponent.SafeZoneOb != null && (myObjectBuilder_SafeZone = myObjectBuilder_SafeZoneComponent.SafeZoneOb as MyObjectBuilder_SafeZone) != null)
				{
					Vector3 size = myObjectBuilder_SafeZone.Size;
					size.X = MathHelper.Clamp(size.X, definition.MinSafeZoneRadius, 2f * definition.MaxSafeZoneRadius);
					size.Y = MathHelper.Clamp(size.Y, definition.MinSafeZoneRadius, 2f * definition.MaxSafeZoneRadius);
					size.Z = MathHelper.Clamp(size.Z, definition.MinSafeZoneRadius, 2f * definition.MaxSafeZoneRadius);
					myObjectBuilder_SafeZone.Size = size;
					myObjectBuilder_SafeZone.Radius = MathHelper.Clamp(myObjectBuilder_SafeZone.Radius, definition.MinSafeZoneRadius, definition.MaxSafeZoneRadius);
				}
			}
<<<<<<< HEAD
=======
			UpdateEffectsAndText();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void CheckObjectBuilders(MyObjectBuilder_SafeZoneBlock builder, MySafeZoneBlockDefinition definition)
		{
			if (builder == null || definition == null || builder.ComponentContainer == null)
			{
				return;
			}
			foreach (MyObjectBuilder_ComponentContainer.ComponentData component in builder.ComponentContainer.Components)
			{
				MyObjectBuilder_SafeZoneComponent myObjectBuilder_SafeZoneComponent;
				MyObjectBuilder_SafeZone myObjectBuilder_SafeZone;
				if ((myObjectBuilder_SafeZoneComponent = component.Component as MyObjectBuilder_SafeZoneComponent) != null && myObjectBuilder_SafeZoneComponent.SafeZoneOb != null && (myObjectBuilder_SafeZone = myObjectBuilder_SafeZoneComponent.SafeZoneOb as MyObjectBuilder_SafeZone) != null)
				{
					Vector3 size = myObjectBuilder_SafeZone.Size;
					size.X = MathHelper.Clamp(size.X, definition.MinSafeZoneRadius, 2f * definition.MaxSafeZoneRadius);
					size.Y = MathHelper.Clamp(size.Y, definition.MinSafeZoneRadius, 2f * definition.MaxSafeZoneRadius);
					size.Z = MathHelper.Clamp(size.Z, definition.MinSafeZoneRadius, 2f * definition.MaxSafeZoneRadius);
					myObjectBuilder_SafeZone.Size = size;
					myObjectBuilder_SafeZone.Radius = MathHelper.Clamp(myObjectBuilder_SafeZone.Radius, definition.MinSafeZoneRadius, definition.MaxSafeZoneRadius);
				}
			}
		}

		public override void UpdateOnceBeforeFrame()
		{
			base.UpdateOnceBeforeFrame();
			base.ResourceSink.Update();
			UpdateEffectsAndText();
			m_readyToRecieveEvents = true;
		}

		protected override void CreateTerminalControls()
		{
			if (MyTerminalControlFactory.AreControlsCreated<MySafeZoneBlock>())
			{
<<<<<<< HEAD
				base.CreateTerminalControls();
				InitControlsStatic();
			}
		}

		private static void InitControlsStatic()
		{
			MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MySafeZoneBlock>
			{
				Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlLabel<MySafeZoneBlock>(MySpaceTexts.TerminalSafeZoneNeedsStation));
			MyTerminalControlOnOffSwitch<MySafeZoneBlock> safeZoneOnOff = new MyTerminalControlOnOffSwitch<MySafeZoneBlock>("SafeZoneCreate", MySpaceTexts.Beacon_SafeZone_Desc, default(MyStringId), MySpaceTexts.Beacon_SafeZone_On, MySpaceTexts.Beacon_SafeZone_Off);
			safeZoneOnOff.Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0;
			safeZoneOnOff.Setter = delegate(MySafeZoneBlock safezone, bool isturnOn)
			{
				safezone.OnSafezoneCreateRemove(isturnOn);
			};
			safeZoneOnOff.Enabled = (MySafeZoneBlock safezone) => !safezone.m_safeZoneManager.WaitingResponse && safezone.Enabled && safezone.IsFunctional && safezone.CubeGrid.IsStatic;
			safeZoneOnOff.Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large;
			safeZoneOnOff.DynamicTooltipGetter = (MySafeZoneBlock safezone) => safezone.OnGetTooltip();
=======
				return;
			}
			base.CreateTerminalControls();
			MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MySafeZoneBlock>
			{
				Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlLabel<MySafeZoneBlock>(MySpaceTexts.TerminalSafeZoneNeedsStation));
			MyTerminalControlOnOffSwitch<MySafeZoneBlock> safeZoneOnOff = new MyTerminalControlOnOffSwitch<MySafeZoneBlock>("SafeZoneCreate", MySpaceTexts.Beacon_SafeZone_Desc, default(MyStringId), MySpaceTexts.Beacon_SafeZone_On, MySpaceTexts.Beacon_SafeZone_Off);
			safeZoneOnOff.Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0;
			safeZoneOnOff.Setter = delegate(MySafeZoneBlock beacon, bool isturnOn)
			{
				beacon.OnSafezoneCreateRemove(isturnOn);
			};
			safeZoneOnOff.Enabled = (MySafeZoneBlock beacon) => !beacon.m_safeZoneManager.WaitingResponse && beacon.Enabled && beacon.IsFunctional && beacon.CubeGrid.IsStatic;
			safeZoneOnOff.Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large;
			safeZoneOnOff.DynamicTooltipGetter = (MySafeZoneBlock beacon) => beacon.OnGetTooltip();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyTerminalControlFactory.AddControl(safeZoneOnOff);
			MyTerminalControlCombobox<MySafeZoneBlock> myTerminalControlCombobox = new MyTerminalControlCombobox<MySafeZoneBlock>("SafeZoneShapeCombo", MySpaceTexts.SafeZone_SelectZoneShape, MySpaceTexts.Beacon_SafeZone_Shape_TTIP);
			MyTerminalControlComboBoxItem sphere;
			MyTerminalControlComboBoxItem myTerminalControlComboBoxItem = (sphere = new MyTerminalControlComboBoxItem
			{
				Key = 0L,
				Value = MySpaceTexts.SafeZone_Spherical
			});
			MyTerminalControlComboBoxItem box;
			myTerminalControlComboBoxItem = (box = new MyTerminalControlComboBoxItem
			{
				Key = 1L,
				Value = MySpaceTexts.SafeZone_Cubical
			});
			myTerminalControlCombobox.ComboBoxContent = delegate(List<MyTerminalControlComboBoxItem> list)
			{
				list.Add(sphere);
				list.Add(box);
			};
<<<<<<< HEAD
			myTerminalControlCombobox.Setter = delegate(MySafeZoneBlock safezone, long key)
			{
				safezone.m_safeZoneManager.OnSafeZoneShapeChanged((MySafeZoneShape)key);
			};
			myTerminalControlCombobox.Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSafeZoneShape();
			myTerminalControlCombobox.Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large;
			myTerminalControlCombobox.Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0;
			MyTerminalControlFactory.AddControl(myTerminalControlCombobox);
			MyTerminalControlSlider<MySafeZoneBlock> myTerminalControlSlider = new MyTerminalControlSlider<MySafeZoneBlock>("SafeZoneSlider", MySpaceTexts.Beacon_SafeZone_RangeSlider, MySpaceTexts.Beacon_SafeZone_RangeSlider_TTIP);
			myTerminalControlSlider.SetLogLimits((MySafeZoneBlock safezone) => safezone.Definition.MinSafeZoneRadius, (MySafeZoneBlock safezone) => safezone.Definition.MaxSafeZoneRadius);
			myTerminalControlSlider.DefaultValueGetter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetRadius();
			myTerminalControlSlider.Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetRadius();
			myTerminalControlSlider.Setter = delegate(MySafeZoneBlock safezone, float radius)
			{
				safezone.m_safeZoneManager.SetRadius(radius);
			};
			myTerminalControlSlider.Writer = delegate(MySafeZoneBlock safezone, StringBuilder result)
			{
				result.AppendDecimal(safezone.m_safeZoneManager.GetRadius(), 0).Append(" m");
			};
			myTerminalControlSlider.Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0;
			myTerminalControlSlider.Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large && safezone.m_safeZoneManager.GetSafeZoneShape() == 0;
			MyTerminalControlFactory.AddControl(myTerminalControlSlider);
			MyTerminalControlSlider<MySafeZoneBlock> myTerminalControlSlider2 = new MyTerminalControlSlider<MySafeZoneBlock>("SafeZoneXSlider", MySpaceTexts.SafeZone_Size_X, MySpaceTexts.Beacon_SafeZone_RangeSlider_TTIP);
			myTerminalControlSlider2.SetLogLimits((MySafeZoneBlock safezone) => safezone.Definition.MinSafeZoneRadius, (MySafeZoneBlock safezone) => safezone.Definition.MaxSafeZoneRadius * 2f);
			myTerminalControlSlider2.DefaultValueGetter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSize().X;
			myTerminalControlSlider2.Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSize().X;
			myTerminalControlSlider2.Setter = delegate(MySafeZoneBlock safezone, float value)
			{
				safezone.m_safeZoneManager.SetSize(MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.X, value);
			};
			myTerminalControlSlider2.Writer = delegate(MySafeZoneBlock safezone, StringBuilder result)
			{
				result.AppendDecimal(safezone.m_safeZoneManager.GetSize().X, 0).Append(" m");
			};
			myTerminalControlSlider2.Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0;
			myTerminalControlSlider2.Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large && safezone.m_safeZoneManager.GetSafeZoneShape() == 1;
			myTerminalControlSlider2.EnableActions();
			MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
			MyTerminalControlSlider<MySafeZoneBlock> myTerminalControlSlider3 = new MyTerminalControlSlider<MySafeZoneBlock>("SafeZoneYSlider", MySpaceTexts.SafeZone_Size_Y, MySpaceTexts.Beacon_SafeZone_RangeSlider_TTIP);
			myTerminalControlSlider3.SetLogLimits((MySafeZoneBlock safezone) => safezone.Definition.MinSafeZoneRadius, (MySafeZoneBlock safezone) => safezone.Definition.MaxSafeZoneRadius * 2f);
			myTerminalControlSlider3.DefaultValueGetter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSize().Y;
			myTerminalControlSlider3.Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSize().Y;
			myTerminalControlSlider3.Setter = delegate(MySafeZoneBlock safezone, float value)
			{
				safezone.m_safeZoneManager.SetSize(MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.Y, value);
			};
			myTerminalControlSlider3.Writer = delegate(MySafeZoneBlock safezone, StringBuilder result)
			{
				result.AppendDecimal(safezone.m_safeZoneManager.GetSize().Y, 0).Append(" m");
			};
			myTerminalControlSlider3.Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0;
			myTerminalControlSlider3.Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large && safezone.m_safeZoneManager.GetSafeZoneShape() == 1;
			MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
			MyTerminalControlSlider<MySafeZoneBlock> myTerminalControlSlider4 = new MyTerminalControlSlider<MySafeZoneBlock>("SafeZoneZSlider", MySpaceTexts.SafeZone_Size_Z, MySpaceTexts.Beacon_SafeZone_RangeSlider_TTIP);
			myTerminalControlSlider4.SetLogLimits((MySafeZoneBlock safezone) => safezone.Definition.MinSafeZoneRadius, (MySafeZoneBlock safezone) => safezone.Definition.MaxSafeZoneRadius * 2f);
			myTerminalControlSlider4.DefaultValueGetter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSize().Z;
			myTerminalControlSlider4.Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSize().Z;
			myTerminalControlSlider4.Setter = delegate(MySafeZoneBlock safezone, float value)
			{
				safezone.m_safeZoneManager.SetSize(MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.Z, value);
			};
			myTerminalControlSlider4.Writer = delegate(MySafeZoneBlock safezone, StringBuilder result)
			{
				result.AppendDecimal(safezone.m_safeZoneManager.GetSize().Z, 0).Append(" m");
			};
			myTerminalControlSlider4.Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0;
			myTerminalControlSlider4.Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large && safezone.m_safeZoneManager.GetSafeZoneShape() == 1;
=======
			myTerminalControlCombobox.Setter = delegate(MySafeZoneBlock beacon, long key)
			{
				beacon.m_safeZoneManager.OnSafeZoneShapeChanged((MySafeZoneShape)key);
			};
			myTerminalControlCombobox.Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSafeZoneShape();
			myTerminalControlCombobox.Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large;
			myTerminalControlCombobox.Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0;
			MyTerminalControlFactory.AddControl(myTerminalControlCombobox);
			MyTerminalControlSlider<MySafeZoneBlock> myTerminalControlSlider = new MyTerminalControlSlider<MySafeZoneBlock>("SafeZoneSlider", MySpaceTexts.Beacon_SafeZone_RangeSlider, MySpaceTexts.Beacon_SafeZone_RangeSlider_TTIP);
			myTerminalControlSlider.SetLogLimits((MySafeZoneBlock beacon) => beacon.Definition.MinSafeZoneRadius, (MySafeZoneBlock beacon) => beacon.Definition.MaxSafeZoneRadius);
			myTerminalControlSlider.DefaultValueGetter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetRadius();
			myTerminalControlSlider.Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetRadius();
			myTerminalControlSlider.Setter = delegate(MySafeZoneBlock beacon, float radius)
			{
				beacon.m_safeZoneManager.SetRadius(radius);
			};
			myTerminalControlSlider.Writer = delegate(MySafeZoneBlock beacon, StringBuilder result)
			{
				result.AppendDecimal(beacon.m_safeZoneManager.GetRadius(), 0).Append(" m");
			};
			myTerminalControlSlider.Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0;
			myTerminalControlSlider.Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large && beacon.m_safeZoneManager.GetSafeZoneShape() == 0;
			MyTerminalControlFactory.AddControl(myTerminalControlSlider);
			MyTerminalControlSlider<MySafeZoneBlock> myTerminalControlSlider2 = new MyTerminalControlSlider<MySafeZoneBlock>("SafeZoneXSlider", MySpaceTexts.SafeZone_Size_X, MySpaceTexts.Beacon_SafeZone_RangeSlider_TTIP);
			myTerminalControlSlider2.SetLogLimits((MySafeZoneBlock beacon) => beacon.Definition.MinSafeZoneRadius, (MySafeZoneBlock beacon) => beacon.Definition.MaxSafeZoneRadius * 2f);
			myTerminalControlSlider2.DefaultValueGetter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSize().X;
			myTerminalControlSlider2.Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSize().X;
			myTerminalControlSlider2.Setter = delegate(MySafeZoneBlock beacon, float value)
			{
				beacon.m_safeZoneManager.SetSize(MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.X, value);
			};
			myTerminalControlSlider2.Writer = delegate(MySafeZoneBlock beacon, StringBuilder result)
			{
				result.AppendDecimal(beacon.m_safeZoneManager.GetSize().X, 0).Append(" m");
			};
			myTerminalControlSlider2.Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0;
			myTerminalControlSlider2.Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large && beacon.m_safeZoneManager.GetSafeZoneShape() == 1;
			myTerminalControlSlider2.EnableActions();
			MyTerminalControlFactory.AddControl(myTerminalControlSlider2);
			MyTerminalControlSlider<MySafeZoneBlock> myTerminalControlSlider3 = new MyTerminalControlSlider<MySafeZoneBlock>("SafeZoneYSlider", MySpaceTexts.SafeZone_Size_Y, MySpaceTexts.Beacon_SafeZone_RangeSlider_TTIP);
			myTerminalControlSlider3.SetLogLimits((MySafeZoneBlock beacon) => beacon.Definition.MinSafeZoneRadius, (MySafeZoneBlock beacon) => beacon.Definition.MaxSafeZoneRadius * 2f);
			myTerminalControlSlider3.DefaultValueGetter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSize().Y;
			myTerminalControlSlider3.Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSize().Y;
			myTerminalControlSlider3.Setter = delegate(MySafeZoneBlock beacon, float value)
			{
				beacon.m_safeZoneManager.SetSize(MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.Y, value);
			};
			myTerminalControlSlider3.Writer = delegate(MySafeZoneBlock beacon, StringBuilder result)
			{
				result.AppendDecimal(beacon.m_safeZoneManager.GetSize().Y, 0).Append(" m");
			};
			myTerminalControlSlider3.Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0;
			myTerminalControlSlider3.Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large && beacon.m_safeZoneManager.GetSafeZoneShape() == 1;
			MyTerminalControlFactory.AddControl(myTerminalControlSlider3);
			MyTerminalControlSlider<MySafeZoneBlock> myTerminalControlSlider4 = new MyTerminalControlSlider<MySafeZoneBlock>("SafeZoneZSlider", MySpaceTexts.SafeZone_Size_Z, MySpaceTexts.Beacon_SafeZone_RangeSlider_TTIP);
			myTerminalControlSlider4.SetLogLimits((MySafeZoneBlock beacon) => beacon.Definition.MinSafeZoneRadius, (MySafeZoneBlock beacon) => beacon.Definition.MaxSafeZoneRadius * 2f);
			myTerminalControlSlider4.DefaultValueGetter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSize().Z;
			myTerminalControlSlider4.Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSize().Z;
			myTerminalControlSlider4.Setter = delegate(MySafeZoneBlock beacon, float value)
			{
				beacon.m_safeZoneManager.SetSize(MyGuiScreenAdminMenu.MyZoneAxisTypeEnum.Z, value);
			};
			myTerminalControlSlider4.Writer = delegate(MySafeZoneBlock beacon, StringBuilder result)
			{
				result.AppendDecimal(beacon.m_safeZoneManager.GetSize().Z, 0).Append(" m");
			};
			myTerminalControlSlider4.Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0;
			myTerminalControlSlider4.Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large && beacon.m_safeZoneManager.GetSafeZoneShape() == 1;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyTerminalControlFactory.AddControl(myTerminalControlSlider4);
			MyTerminalControlFactory.AddControl(new MyTerminalControlButton<MySafeZoneBlock>("SafeZoneFilterBtn", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_ConfigureFilter, MySpaceTexts.Beacon_SafeZone_FilterBtn_TTIP, delegate(MySafeZoneBlock x)
			{
				x.OnSafeZoneFilterBtnPressed();
			})
			{
<<<<<<< HEAD
				Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneDamageCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDamage, MySpaceTexts.Beacon_SafeZone_AllowDmg_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock safezone, bool isChecked)
				{
					safezone.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Damage, isChecked);
				},
				Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Damage),
				Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneShootingCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowShooting, MySpaceTexts.Beacon_SafeZone_AllowShoot_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock safezone, bool isChecked)
				{
					safezone.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Shooting, isChecked);
				},
				Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Shooting),
				Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneDrillingCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDrilling, MySpaceTexts.Beacon_SafeZone_AllowDrill_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock safezone, bool isChecked)
				{
					safezone.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Drilling, isChecked);
				},
				Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Drilling),
				Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneWeldingCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowWelding, MySpaceTexts.Beacon_SafeZone_AllowWeld_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock safezone, bool isChecked)
				{
					safezone.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Welding, isChecked);
				},
				Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Welding),
				Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneGrindingCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowGrinding, MySpaceTexts.Beacon_SafeZone_AllowGrind_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock safezone, bool isChecked)
				{
					safezone.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Grinding, isChecked);
				},
				Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Grinding),
				Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneBuildingCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowBuilding, MySpaceTexts.Beacon_SafeZone_AllowBuild_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock safezone, bool isChecked)
				{
					safezone.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Building, isChecked);
				},
				Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Building),
				Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneBuildingProjectionCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowBuildingProjections, MySpaceTexts.Beacon_SafeZone_AllowBuild_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock safezone, bool isChecked)
				{
					safezone.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.BuildingProjections, isChecked);
				},
				Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.BuildingProjections),
				Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneVoxelHandCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowVoxelHands, MySpaceTexts.Beacon_SafeZone_AllowVoxel_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock safezone, bool isChecked)
				{
					safezone.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.VoxelHand, isChecked);
				},
				Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.VoxelHand),
				Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneLandingGearCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowLandingGear, MySpaceTexts.Beacon_SafeZone_AllowLandingGear_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock safezone, bool isChecked)
				{
					safezone.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.LandingGearLock, isChecked);
				},
				Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.LandingGearLock),
				Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneConvertToStationCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowConvertToStation, MySpaceTexts.Beacon_SafeZone_AllowConvertToStation_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock safezone, bool isChecked)
				{
					safezone.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.ConvertToStation, isChecked);
				},
				Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.ConvertToStation),
				Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlColor<MySafeZoneBlock> safeZoneColor = new MyTerminalControlColor<MySafeZoneBlock>("SafeZoneColor", MySpaceTexts.ScreenAdmin_Safezone_ColorLabel);
			safeZoneColor.Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0;
			safeZoneColor.Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large;
			safeZoneColor.DynamicTooltipGetter = (MySafeZoneBlock safezone) => safezone.OnGetColorTooltip();
			safeZoneColor.Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetColor();
			safeZoneColor.Setter = delegate(MySafeZoneBlock safezone, Color color)
			{
				safezone.m_safeZoneManager.SetColor(color);
			};
			MyTerminalControlFactory.AddControl(safeZoneColor);
			MyTerminalControlCombobox<MySafeZoneBlock> safeZoneTextureCombo = new MyTerminalControlCombobox<MySafeZoneBlock>("SafeZoneTextureCombo", MySpaceTexts.SafeZone_Texture, MySpaceTexts.SafeZone_Texture_TTIP);
			safeZoneTextureCombo.ComboBoxContentWithBlock = delegate(MySafeZoneBlock safezone, ICollection<MyTerminalControlComboBoxItem> list)
			{
				safezone.GetTexturesList(list);
			};
			safeZoneTextureCombo.Setter = delegate(MySafeZoneBlock safezone, long key)
			{
				safezone.OnTextureSelected(key);
			};
			safeZoneTextureCombo.Getter = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.GetTexture();
			safeZoneTextureCombo.Visible = (MySafeZoneBlock safezone) => safezone.Definition.CubeSize == MyCubeSize.Large;
			safeZoneTextureCombo.Enabled = (MySafeZoneBlock safezone) => safezone.m_safeZoneManager.SafeZoneEntityId != 0L && safezone.m_dlcComponent.HasDLC("Economy", MySession.Static.LocalHumanPlayer.Id.SteamId);
			safeZoneTextureCombo.DynamicTooltipGetter = (MySafeZoneBlock safezone) => safezone.OnGetTextureTooltip();
			MyTerminalControlFactory.AddControl(safeZoneTextureCombo);
			MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MySafeZoneBlock>());
=======
				Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneDamageCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDamage, MySpaceTexts.Beacon_SafeZone_AllowDmg_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock beacon, bool isChecked)
				{
					beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Damage, isChecked);
				},
				Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Damage),
				Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneShootingCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowShooting, MySpaceTexts.Beacon_SafeZone_AllowShoot_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock beacon, bool isChecked)
				{
					beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Shooting, isChecked);
				},
				Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Shooting),
				Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneDrillingCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowDrilling, MySpaceTexts.Beacon_SafeZone_AllowDrill_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock beacon, bool isChecked)
				{
					beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Drilling, isChecked);
				},
				Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Drilling),
				Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneWeldingCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowWelding, MySpaceTexts.Beacon_SafeZone_AllowWeld_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock beacon, bool isChecked)
				{
					beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Welding, isChecked);
				},
				Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Welding),
				Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneGrindingCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowGrinding, MySpaceTexts.Beacon_SafeZone_AllowGrind_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock beacon, bool isChecked)
				{
					beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Grinding, isChecked);
				},
				Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Grinding),
				Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneBuildingCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowBuilding, MySpaceTexts.Beacon_SafeZone_AllowBuild_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock beacon, bool isChecked)
				{
					beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.Building, isChecked);
				},
				Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.Building),
				Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneBuildingProjectionCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowBuildingProjections, MySpaceTexts.Beacon_SafeZone_AllowBuild_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock beacon, bool isChecked)
				{
					beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.BuildingProjections, isChecked);
				},
				Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.BuildingProjections),
				Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneVoxelHandCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowVoxelHands, MySpaceTexts.Beacon_SafeZone_AllowVoxel_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock beacon, bool isChecked)
				{
					beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.VoxelHand, isChecked);
				},
				Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.VoxelHand),
				Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneLandingGearCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowLandingGear, MySpaceTexts.Beacon_SafeZone_AllowLandingGear_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock beacon, bool isChecked)
				{
					beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.LandingGearLock, isChecked);
				},
				Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.LandingGearLock),
				Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlFactory.AddControl(new MyTerminalControlCheckbox<MySafeZoneBlock>("SafeZoneConvertToStationCb", MySpaceTexts.ScreenDebugAdminMenu_SafeZones_AllowConvertToStation, MySpaceTexts.Beacon_SafeZone_AllowConvertToStation_TTIP, null, null, justify: true)
			{
				Setter = delegate(MySafeZoneBlock beacon, bool isChecked)
				{
					beacon.m_safeZoneManager.OnSafeZoneSettingChanged(MySafeZoneAction.ConvertToStation, isChecked);
				},
				Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetSafeZoneSetting(MySafeZoneAction.ConvertToStation),
				Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large,
				Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0
			});
			MyTerminalControlColor<MySafeZoneBlock> safeZoneColor = new MyTerminalControlColor<MySafeZoneBlock>("SafeZoneColor", MySpaceTexts.ScreenAdmin_Safezone_ColorLabel);
			safeZoneColor.Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0;
			safeZoneColor.Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large;
			safeZoneColor.DynamicTooltipGetter = (MySafeZoneBlock beacon) => beacon.OnGetColorTooltip();
			safeZoneColor.Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetColor();
			safeZoneColor.Setter = delegate(MySafeZoneBlock beacon, Color color)
			{
				beacon.m_safeZoneManager.SetColor(color);
			};
			MyTerminalControlFactory.AddControl(safeZoneColor);
			MyTerminalControlCombobox<MySafeZoneBlock> safeZoneTextureCombo = new MyTerminalControlCombobox<MySafeZoneBlock>("SafeZoneTextureCombo", MySpaceTexts.SafeZone_Texture, MySpaceTexts.SafeZone_Texture_TTIP);
			safeZoneTextureCombo.ComboBoxContentWithBlock = delegate(MySafeZoneBlock beacon, ICollection<MyTerminalControlComboBoxItem> list)
			{
				beacon.GetTexturesList(list);
			};
			safeZoneTextureCombo.Setter = delegate(MySafeZoneBlock beacon, long key)
			{
				beacon.OnTextureSelected(key);
			};
			safeZoneTextureCombo.Getter = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.GetTexture();
			safeZoneTextureCombo.Visible = (MySafeZoneBlock beacon) => beacon.Definition.CubeSize == MyCubeSize.Large;
			safeZoneTextureCombo.Enabled = (MySafeZoneBlock beacon) => beacon.m_safeZoneManager.SafeZoneEntityId != 0L && m_dlcComponent.HasDLC("Economy", MySession.Static.LocalHumanPlayer.Id.SteamId);
			safeZoneTextureCombo.DynamicTooltipGetter = (MySafeZoneBlock beacon) => beacon.OnGetTextureTooltip();
			MyTerminalControlFactory.AddControl(safeZoneTextureCombo);
			MyTerminalControlFactory.AddControl(new MyTerminalControlSeparator<MySafeZoneBlock>());
			MyMultiTextPanelComponent.CreateTerminalControls<MySafeZoneBlock>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!Sandbox.Engine.Platform.Game.IsDedicated)
			{
				MySandboxGame.Static.Invoke(delegate
				{
					safeZoneOnOff.GetGuiControl().ShowTooltipWhenDisabled = true;
					safeZoneColor.GetGuiControl().ShowTooltipWhenDisabled = true;
					safeZoneTextureCombo.GetGuiControl().ShowTooltipWhenDisabled = true;
				}, "SafeZoneBlock - TerminalControl");
			}
		}

		private void OnTextureSelected(long key)
		{
			IEnumerable<MySafeZoneTexturesDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MySafeZoneTexturesDefinition>();
			if (allDefinitions == null)
			{
				MyLog.Default.Error("Textures definition for safe zone are missing. Without it, safezone wont work propertly.");
				return;
			}
			MyStringHash myStringHash = MyStringHash.TryGet((int)key);
			bool flag = false;
			foreach (MySafeZoneTexturesDefinition item in allDefinitions)
			{
				if (item.DisplayTextId == myStringHash)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				MyLog.Default.Error("Safe zone texture not found.");
			}
			else
			{
				m_safeZoneManager.SetTexture(myStringHash);
			}
		}

		private void GetTexturesList(ICollection<MyTerminalControlComboBoxItem> list)
		{
			IEnumerable<MySafeZoneTexturesDefinition> allDefinitions = MyDefinitionManager.Static.GetAllDefinitions<MySafeZoneTexturesDefinition>();
			if (allDefinitions == null)
			{
				MyLog.Default.Error("Textures definition for safe zone are missing. Without it, safezone wont work propertly.");
				return;
			}
			foreach (MySafeZoneTexturesDefinition item2 in allDefinitions)
			{
				MyTerminalControlComboBoxItem myTerminalControlComboBoxItem = default(MyTerminalControlComboBoxItem);
				myTerminalControlComboBoxItem.Key = (int)item2.DisplayTextId;
				myTerminalControlComboBoxItem.Value = MyStringId.GetOrCompute(item2.DisplayTextId.String);
				MyTerminalControlComboBoxItem item = myTerminalControlComboBoxItem;
				list.Add(item);
			}
		}

		private void OnZonechipContentsChanged(MyInventoryBase obj)
		{
			m_safeZoneManager.SetActivate_Server(activate: true);
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			UpdateEffectsAndText();
		}

		private string OnGetTooltip()
		{
			return new StringBuilder().AppendFormat(MySpaceTexts.Beacon_SafeZone_ToolTip, Definition.SafeZoneActivationTimeS, Definition.SafeZoneUpkeep, Definition.SafeZoneUpkeepTimeM, (Definition.SafeZoneActivationTimeS == 0) ? "" : MyTexts.GetString(MySpaceTexts.Beacon_SafeZone_ToolTip_PluralSuffix_Activation), (Definition.SafeZoneUpkeep == 0) ? "" : MyTexts.GetString(MySpaceTexts.Beacon_SafeZone_ToolTip_PluralSuffix_ZoneChips), (Definition.SafeZoneUpkeepTimeM == 0) ? "" : MyTexts.GetString(MySpaceTexts.Beacon_SafeZone_ToolTip_PluralSuffix_Minutes)).ToString();
		}

		private string OnGetColorTooltip()
		{
			return MyTexts.GetString(MySpaceTexts.SafeZone_Color_TTP);
		}

		private string OnGetTextureTooltip()
		{
			if (!m_dlcComponent.HasDLC("Economy", MySession.Static.LocalHumanPlayer.Id.SteamId))
			{
				return MyTexts.GetString(MySpaceTexts.SafeZone_Texture_DLCReq_TTIP);
			}
			return MyTexts.GetString(MySpaceTexts.SafeZone_Texture_TTIP);
		}

		public override MyObjectBuilder_CubeBlock GetObjectBuilderCubeBlock(bool copy = false)
		{
			MyObjectBuilder_SafeZoneBlock myObjectBuilder_SafeZoneBlock = base.GetObjectBuilderCubeBlock(copy) as MyObjectBuilder_SafeZoneBlock;
			if (!copy)
			{
				myObjectBuilder_SafeZoneBlock.SafeZoneId = m_safeZoneManager.SafeZoneEntityId;
			}
			return myObjectBuilder_SafeZoneBlock;
		}

		public override void OnAddedToScene(object source)
		{
			base.OnAddedToScene(source);
			if (this.GetInventory() != null)
			{
				this.GetInventory().ContentsChanged += OnZonechipContentsChanged;
			}
			MySessionComponentSafeZones.OnSafeZoneUpdated += OnSafeZoneUpdated;
			base.CubeGrid.OnStaticChanged += OnIsStaticChanged;
			base.IsWorkingChanged += OnIsWorkingChanged;
			m_safeZoneManager.SafeZoneChanged += OnSafeZoneChanged;
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			if (Sync.IsServer && m_immediatelyEnableSafezone)
			{
				m_safeZoneManager.SafeZoneCreate_Server(skipActivationTimer: true);
				m_immediatelyEnableSafezone = false;
			}
		}

		public override void OnAddedToScene(object source)
		{
<<<<<<< HEAD
			base.OnRemovedFromScene(source);
			if (this.GetInventory() != null)
			{
				this.GetInventory().ContentsChanged -= OnZonechipContentsChanged;
			}
=======
			base.OnAddedToScene(source);
			if (this.GetInventory() != null)
			{
				this.GetInventory().ContentsChanged += OnZonechipContentsChanged;
			}
			MySessionComponentSafeZones.OnSafeZoneUpdated += OnSafeZoneUpdated;
			base.CubeGrid.OnStaticChanged += OnIsStaticChanged;
			base.IsWorkingChanged += OnIsWorkingChanged;
			m_safeZoneManager.SafeZoneChanged += OnSafeZoneChanged;
			SlimBlock.ComponentStack.IsFunctionalChanged += ComponentStack_IsFunctionalChanged;
			if (m_multiPanel != null && m_multiPanel.SurfaceCount > 0)
			{
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
			if (m_multiPanel != null)
			{
				m_multiPanel.AddToScene();
			}
		}

		public override void OnRemovedFromScene(object source)
		{
			base.OnRemovedFromScene(source);
			if (this.GetInventory() != null)
			{
				this.GetInventory().ContentsChanged -= OnZonechipContentsChanged;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			base.ResourceSink.IsPoweredChanged -= Receiver_IsPoweredChanged;
			MySessionComponentSafeZones.OnSafeZoneUpdated -= OnSafeZoneUpdated;
			base.CubeGrid.OnStaticChanged -= OnIsStaticChanged;
			base.IsWorkingChanged -= OnIsWorkingChanged;
			m_safeZoneManager.SafeZoneChanged -= OnSafeZoneChanged;
			SlimBlock.ComponentStack.IsFunctionalChanged -= ComponentStack_IsFunctionalChanged;
<<<<<<< HEAD
			if (base.CubeGrid.IsBlockTrasferInProgress && m_safeZoneManager.IsSafeZoneEnabled())
			{
				m_immediatelyEnableSafezone = true;
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (Sync.IsServer)
			{
				m_safeZoneManager.SafeZoneRemove_Server();
			}
		}

		private void ComponentStack_IsFunctionalChanged()
		{
			base.ResourceSink.Update();
			UpdateEffectsAndText();
		}

		private void OnSafezoneCreateRemove(bool turnOnSafeZone)
		{
<<<<<<< HEAD
			MyCubeGrid cubeGrid = base.CubeGrid;
			if (cubeGrid != null && cubeGrid.IsStatic)
			{
				m_safeZoneManager.OnSafezoneCreateRemove_Request(turnOnSafeZone);
				base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
				base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			}
=======
			m_safeZoneManager.OnSafezoneCreateRemove_Request(turnOnSafeZone);
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private float UpdatePowerInput()
		{
			float powerDrain = m_safeZoneManager.GetPowerDrain();
			if (!Enabled || !base.IsFunctional)
			{
				return 0f;
			}
			return powerDrain;
		}

		private void OnSafeZoneChanged()
		{
			base.ResourceSink.Update();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			UpdateEffectsAndText();
		}

		private void OnSafeZoneUpdated(MySafeZone obj)
		{
			if (m_safeZoneManager != null && m_safeZoneManager.SafeZoneEntityId == obj.EntityId)
			{
				OnSafeZoneChanged();
			}
		}

		private void Receiver_IsPoweredChanged()
		{
			UpdateIsWorking();
			UpdateEffectsAndText();
		}

		private void OnIsStaticChanged(MyCubeGrid grid, bool isStatic)
		{
			if (!isStatic && Sync.IsServer)
			{
				m_safeZoneManager.SafeZoneRemove_Server();
			}
			RaisePropertiesChanged();
		}

		private void OnIsWorkingChanged(MyCubeBlock obj)
		{
			if (base.CubeGrid == null || base.CubeGrid.MarkedForClose || !base.CubeGrid.IsStatic)
			{
				return;
			}
			if (Sync.IsServer && m_readyToRecieveEvents)
			{
				if (base.IsWorking)
				{
					m_safeZoneManager.SafeZoneCreate_Server();
				}
				else
				{
					m_safeZoneManager.SafeZoneRemove_Server();
				}
			}
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override void UpdateAfterSimulation100()
		{
			base.UpdateAfterSimulation100();
			bool flag = false;
			if (base.IsWorking && base.IsFunctional)
			{
				flag = m_safeZoneManager.Update();
				if (flag)
				{
					UpdateEffectsAndText();
				}
			}
			if (!flag && base.IsFunctional && !base.HasDamageEffect)
			{
				base.NeedsUpdate &= ~MyEntityUpdateEnum.EACH_100TH_FRAME;
			}
		}

		protected override bool CheckIsWorking()
		{
			if (base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId))
			{
				return base.CheckIsWorking();
			}
			return false;
		}

		protected override void OnEnabledChanged()
		{
			base.ResourceSink.Update();
			base.OnEnabledChanged();
			base.NeedsUpdate |= MyEntityUpdateEnum.EACH_100TH_FRAME;
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
			m_isSoundRunning = false;
			UpdateEffectsAndText();
		}

		protected override void UpdateDetailedInfo(StringBuilder detailedInfo)
		{
			base.UpdateDetailedInfo(detailedInfo);
			detailedInfo.AppendStringBuilder(MyTexts.Get(MyCommonTexts.BlockPropertiesText_Type));
			detailedInfo.Append(base.BlockDefinition.DisplayNameText);
			detailedInfo.Append("\n");
			detailedInfo.AppendStringBuilder(MyTexts.Get(MySpaceTexts.BlockPropertyProperties_CurrentInput));
			MyValueFormatter.AppendWorkInBestUnit(base.ResourceSink.IsPoweredByType(MyResourceDistributorComponent.ElectricityId) ? base.ResourceSink.RequiredInputByType(MyResourceDistributorComponent.ElectricityId) : 0f, detailedInfo);
			detailedInfo.Append("\n");
			m_safeZoneManager.SetTextInfo(detailedInfo);
		}

		private void UpdateEffectsAndText()
		{
			if (m_soundEmitter != null)
			{
				bool flag = m_safeZoneManager.IsSafeZoneInWorld();
				if (flag != m_isSoundRunning)
				{
					if (flag)
					{
						m_soundEmitter.PlaySound(m_processSound, stopPrevious: true);
					}
					else
					{
						m_soundEmitter.StopSound(forced: false);
					}
					m_isSoundRunning = flag;
				}
			}
			SetDetailedInfoDirty();
			RaisePropertiesChanged();
		}

		private void OnSafeZoneFilterBtnPressed()
		{
			m_safeZoneManager.OnSafeZoneFilterBtnPressed();
		}

		public void InitializeConveyorEndpoint()
		{
			m_conveyorEndpoint = new MyMultilineConveyorEndpoint(this);
		}

		public bool AllowSelfPulling()
		{
			return true;
		}

		public PullInformation GetPullInformation()
		{
			MyInventory inventory = this.GetInventory();
			if (inventory == null)
			{
				return null;
			}
			return new PullInformation
			{
				OwnerID = base.OwnerId,
				Inventory = inventory,
				Constraint = inventory.Constraint
			};
		}

		public PullInformation GetPushInformation()
		{
			MyInventory inventory = this.GetInventory();
			if (inventory == null)
			{
				return null;
			}
			return new PullInformation
			{
				OwnerID = base.OwnerId,
				Inventory = inventory,
				Constraint = inventory.Constraint
			};
		}

<<<<<<< HEAD
=======
		public override void UpdateAfterSimulation10()
		{
			base.UpdateAfterSimulation10();
			if (m_multiPanel != null)
			{
				m_multiPanel.UpdateAfterSimulation(base.IsWorking);
			}
		}

		private void UpdateScreen()
		{
			m_multiPanel?.UpdateScreen(base.IsWorking);
		}

		private void SendAddImagesToSelectionRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MySafeZoneBlock x) => x.OnSelectImageRequest, panelIndex, selection);
		}

		private void SendRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			MyMultiplayer.RaiseEvent(this, (MySafeZoneBlock x) => x.OnRemoveSelectedImageRequest, panelIndex, selection);
		}

		private void ChangeTextRequest(int panelIndex, string text)
		{
			MyMultiplayer.RaiseEvent(this, (MySafeZoneBlock x) => x.OnChangeTextRequest, panelIndex, text);
		}

		[Event(null, 713)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnChangeTextRequest(int panelIndex, [Nullable] string text)
		{
			m_multiPanel?.ChangeText(panelIndex, text);
		}

		private void UpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			if (Sync.IsServer)
			{
				MyMultiplayer.RaiseEvent(this, (MySafeZoneBlock x) => x.OnUpdateSpriteCollection, panelIndex, sprites);
			}
		}

		[Event(null, 729)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		[DistanceRadius(32f)]
		private void OnUpdateSpriteCollection(int panelIndex, MySerializableSpriteCollection sprites)
		{
			m_multiPanel?.UpdateSpriteCollection(panelIndex, sprites);
		}

		Sandbox.ModAPI.Ingame.IMyTextSurface Sandbox.ModAPI.Ingame.IMyTextSurfaceProvider.GetSurface(int index)
		{
			if (m_multiPanel == null)
			{
				return null;
			}
			return m_multiPanel.GetSurface(index);
		}

		[Event(null, 746)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnRemoveSelectedImageRequest(int panelIndex, int[] selection)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.RemoveItems(panelIndex, selection);
			}
		}

		[Event(null, 755)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		private void OnSelectImageRequest(int panelIndex, int[] selection)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.SelectItems(panelIndex, selection);
			}
		}

		void IMyMultiTextPanelComponentOwner.SelectPanel(List<MyGuiControlListbox.Item> panelItems)
		{
			if (m_multiPanel != null)
			{
				m_multiPanel.SelectPanel((int)panelItems[0].UserData);
			}
			RaisePropertiesChanged();
		}

		public void OpenWindow(bool isEditable, bool sync, bool isPublic)
		{
			if (sync)
			{
				SendChangeOpenMessage(isOpen: true, isEditable, Sync.MyId, isPublic);
				return;
			}
			CreateTextBox(isEditable, new StringBuilder(PanelComponent.Text.ToString()), isPublic);
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = MyGuiScreenGamePlay.ActiveGameplayScreen;
			MyScreenManager.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = m_textBoxMultiPanel);
		}

		private void CreateTextBox(bool isEditable, StringBuilder description, bool isPublic)
		{
			string displayNameText = DisplayNameText;
			string displayName = PanelComponent.DisplayName;
			string description2 = description.ToString();
			bool editable = isEditable;
			m_textBoxMultiPanel = new MyGuiScreenTextPanel(displayNameText, "", displayName, description2, OnClosedPanelTextBox, null, null, editable);
		}

		public void OnClosedPanelTextBox(ResultEnum result)
		{
			if (m_textBoxMultiPanel != null)
			{
				if (m_textBoxMultiPanel.Description.Text.Length > 100000)
				{
					MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.YES_NO, callback: OnClosedPanelMessageBox, messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextTooLongText)));
				}
				else
				{
					CloseWindow(isPublic: true);
				}
			}
		}

		public void OnClosedPanelMessageBox(MyGuiScreenMessageBox.ResultEnum result)
		{
			if (result == MyGuiScreenMessageBox.ResultEnum.YES)
			{
				m_textBoxMultiPanel.Description.Text.Remove(100000, m_textBoxMultiPanel.Description.Text.Length - 100000);
				CloseWindow(isPublic: true);
			}
			else
			{
				CreateTextBox(isEditable: true, m_textBoxMultiPanel.Description.Text, isPublic: true);
				MyScreenManager.AddScreen(m_textBoxMultiPanel);
			}
		}

		[Event(null, 835)]
		[Reliable]
		[Broadcast]
		private void OnChangeOpenSuccess(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			OnChangeOpen(isOpen, editable, user, isPublic);
		}

		private void SendChangeOpenMessage(bool isOpen, bool editable = false, ulong user = 0uL, bool isPublic = false)
		{
			MyMultiplayer.RaiseEvent(this, (MySafeZoneBlock x) => x.OnChangeOpenRequest, isOpen, editable, user, isPublic);
		}

		[Event(null, 846)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		private void OnChangeOpenRequest(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			if (!(Sync.IsServer && IsTextPanelOpen && isOpen))
			{
				OnChangeOpen(isOpen, editable, user, isPublic);
				MyMultiplayer.RaiseEvent(this, (MySafeZoneBlock x) => x.OnChangeOpenSuccess, isOpen, editable, user, isPublic);
			}
		}

		private void OnChangeOpen(bool isOpen, bool editable, ulong user, bool isPublic)
		{
			IsTextPanelOpen = isOpen;
			if (!Sandbox.Engine.Platform.Game.IsDedicated && user == Sync.MyId && isOpen)
			{
				OpenWindow(editable, sync: false, isPublic);
			}
		}

		private void CloseWindow(bool isPublic)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiScreenGamePlay.TmpGameplayScreenHolder;
			MyGuiScreenGamePlay.TmpGameplayScreenHolder = null;
			Enumerator<MySlimBlock> enumerator = base.CubeGrid.CubeBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MySlimBlock current = enumerator.get_Current();
					if (current.FatBlock != null && current.FatBlock.EntityId == base.EntityId)
					{
						SendChangeDescriptionMessage(m_textBoxMultiPanel.Description.Text, isPublic);
						SendChangeOpenMessage(isOpen: false, editable: false, 0uL);
						break;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
		}

		private void SendChangeDescriptionMessage(StringBuilder description, bool isPublic)
		{
			if (base.CubeGrid.IsPreview || !base.CubeGrid.SyncFlag)
			{
				PanelComponent.Text = description;
			}
			else if (description.CompareTo(PanelComponent.Text) != 0)
			{
				MyMultiplayer.RaiseEvent(this, (MySafeZoneBlock x) => x.OnChangeDescription, description.ToString(), isPublic);
			}
		}

		[Event(null, 914)]
		[Reliable]
		[Server(ValidationType.Access | ValidationType.Ownership)]
		[Broadcast]
		public void OnChangeDescription(string description, bool isPublic)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Clear().Append(description);
			PanelComponent.Text = stringBuilder;
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override void UpdateVisual()
		{
			base.UpdateVisual();
			base.NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
		}

		public override void OnRemovedByCubeBuilder()
		{
			ReleaseInventory(this.GetInventory());
			base.OnRemovedByCubeBuilder();
		}

		public override void OnDestroy()
		{
			ReleaseInventory(this.GetInventory(), damageContent: true);
			base.OnDestroy();
		}

		void SpaceEngineers.Game.ModAPI.IMySafeZoneBlock.EnableSafeZone(bool turnOn)
		{
			if (base.CubeGrid.IsStatic)
			{
				OnSafezoneCreateRemove(turnOn);
			}
		}

		bool SpaceEngineers.Game.ModAPI.IMySafeZoneBlock.IsSafeZoneEnabled()
		{
			return m_safeZoneManager.SafeZoneEntityId != 0;
		}
	}
}
