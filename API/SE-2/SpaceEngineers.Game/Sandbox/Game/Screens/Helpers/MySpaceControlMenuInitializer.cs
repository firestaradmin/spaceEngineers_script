using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Entities.Character.Components;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using SpaceEngineers.Game.Entities.Cube;
using SpaceEngineers.Game.Entities.UseObjects;

namespace Sandbox.Game.Screens.Helpers
{
	public class MySpaceControlMenuInitializer : IMyControlMenuInitializer
	{
		private MyGuiScreenControlMenu m_controlMenu;

		private MyControllableEntityControlHelper m_lightsControlHelper;

		private MyControllableEntityControlHelper m_helmetControlHelper;

		private MyControllableEntityControlHelper m_dampingControlHelper;

		private MyControllableEntityControlHelper m_broadcastingControlHelper;

		private MyControllableEntityControlHelper m_reactorsControlHelper;

		private MyControllableEntityControlHelper m_jetpackControlHelper;

		private MyControllableEntityControlHelper m_buildModeControlHelper;

		private MyLandingGearControlHelper m_landingGearsControlHelper;

		private MyQuickLoadControlHelper m_quickLoadControlHelper;

		private MyHudToggleControlHelper m_hudToggleControlHelper;

		private MyCameraModeControlHelper m_cameraModeControlHelper;

		private MyShowTerminalControlHelper m_showTerminalControlHelper;

		private MyShowBuildScreenControlHelper m_showBuildScreenControlHelper;

		private MyColorPickerControlHelper m_colorPickerControlHelper;

		private MySuicideControlHelper m_suicideControlHelper;

		private MyUseTerminalControlHelper m_terminalControlHelper;

		private MyEnableStationRotationControlHelper m_enableStationRotationControlHelper;

		private MyConnectorControlHelper m_connectorControlHelper;

		private MyBlueprintMenuControlHelper m_blueprintControlHelper;

		private MyInventoryMenuControlHelper m_inventoryControlHelper;

		private MyPlayersMenuControlHelper m_playersControlHelper;

		private MyHelpMenuControlHelper m_helpControlHelper;

		private MySpawnMenuControlHelper m_spawnControlHelper;

		private MyAdminMenuControlHelper m_adminControlHelper;

		private bool IsControlMenuInitialized => m_controlMenu != null;

		public MySpaceControlMenuInitializer()
		{
			m_lightsControlHelper = new MyControllableEntityControlHelper(MyControlsSpace.HEADLIGHTS, delegate(IMyControllableEntity x)
			{
				x.SwitchLights();
			}, (IMyControllableEntity x) => x.EnabledLights, MySpaceTexts.ControlMenuItemLabel_Lights);
			m_helmetControlHelper = new MyControllableEntityControlHelper(MyControlsSpace.HELMET, delegate(IMyControllableEntity x)
			{
				x.SwitchHelmet();
			}, (IMyControllableEntity x) => x.EnabledHelmet, MySpaceTexts.ControlMenuItemLabel_Helmet);
			m_dampingControlHelper = new MyControllableEntityControlHelper(MyControlsSpace.DAMPING, delegate(IMyControllableEntity x)
			{
				x.SwitchDamping();
			}, (IMyControllableEntity x) => x.EnabledDamping, MySpaceTexts.ControlMenuItemLabel_Dampeners);
			m_broadcastingControlHelper = new MyControllableEntityControlHelper(MyControlsSpace.BROADCASTING, delegate(IMyControllableEntity x)
			{
				x.SwitchBroadcasting();
			}, (IMyControllableEntity x) => x.EnabledBroadcasting, MySpaceTexts.ControlMenuItemLabel_Broadcasting);
			m_landingGearsControlHelper = new MyLandingGearControlHelper();
			m_connectorControlHelper = new MyConnectorControlHelper();
			m_reactorsControlHelper = new MyControllableEntityControlHelper(MyControlsSpace.TOGGLE_REACTORS, delegate(IMyControllableEntity x)
			{
				x.SwitchReactors();
			}, (IMyControllableEntity x) => x.EnabledReactors, MySpaceTexts.ControlMenuItemLabel_Reactors);
			m_jetpackControlHelper = new MyControllableEntityControlHelper(MyControlsSpace.THRUSTS, delegate(IMyControllableEntity x)
			{
				x.SwitchThrusts();
			}, (IMyControllableEntity x) => x.EnabledThrusts, MySpaceTexts.ControlMenuItemLabel_Jetpack);
			m_buildModeControlHelper = new MyControllableEntityControlHelper(MyControlsSpace.BUILD_MODE, delegate
			{
				MyCubeBuilder.Static.IsBuildMode = !MyCubeBuilder.Static.IsBuildMode;
			}, (IMyControllableEntity x) => MyCubeBuilder.Static.IsBuildMode, MySpaceTexts.ControlMenuItemLabel_BuildMode);
			m_quickLoadControlHelper = new MyQuickLoadControlHelper();
			m_hudToggleControlHelper = new MyHudToggleControlHelper();
			m_cameraModeControlHelper = new MyCameraModeControlHelper();
			m_showTerminalControlHelper = new MyShowTerminalControlHelper();
			m_showBuildScreenControlHelper = new MyShowBuildScreenControlHelper();
			m_colorPickerControlHelper = new MyColorPickerControlHelper();
			m_suicideControlHelper = new MySuicideControlHelper();
			m_terminalControlHelper = new MyUseTerminalControlHelper();
			m_blueprintControlHelper = new MyBlueprintMenuControlHelper();
			m_inventoryControlHelper = new MyInventoryMenuControlHelper();
			m_playersControlHelper = new MyPlayersMenuControlHelper();
			m_helpControlHelper = new MyHelpMenuControlHelper();
			m_spawnControlHelper = new MySpawnMenuControlHelper();
			m_adminControlHelper = new MyAdminMenuControlHelper();
			m_enableStationRotationControlHelper = new MyEnableStationRotationControlHelper();
		}

		public void OpenControlMenu(IMyControllableEntity controlledEntity)
		{
			m_controlMenu = null;
			if (controlledEntity is MyCharacter)
			{
				SetupCharacterScreen(controlledEntity as MyCharacter);
			}
			else if (controlledEntity is MyShipController)
			{
				SetupSpaceshipScreen(controlledEntity as MyShipController);
			}
			if (IsControlMenuInitialized)
			{
				m_controlMenu.RecreateControls(constructor: false);
				MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = m_controlMenu);
			}
		}

		private void SetupCharacterScreen(MyCharacter character)
		{
			m_lightsControlHelper.SetEntity(character);
			m_dampingControlHelper.SetEntity(character);
			m_broadcastingControlHelper.SetEntity(character);
			m_helmetControlHelper.SetEntity(character);
			m_jetpackControlHelper.SetEntity(character);
			m_showBuildScreenControlHelper.SetEntity(character);
			m_showTerminalControlHelper.SetEntity(character);
			m_suicideControlHelper.SetCharacter(character);
			m_terminalControlHelper.SetCharacter(character);
			m_buildModeControlHelper.SetEntity(character);
			m_inventoryControlHelper.SetEntity(character);
			m_controlMenu = new MyGuiScreenControlMenu();
			m_controlMenu.AddItem(m_showTerminalControlHelper);
			m_controlMenu.AddItem(m_showBuildScreenControlHelper);
			m_controlMenu.AddItem(m_inventoryControlHelper);
			m_controlMenu.AddItem(m_buildModeControlHelper);
			m_controlMenu.AddItem(m_hudToggleControlHelper);
			m_controlMenu.AddItem(m_jetpackControlHelper);
			m_controlMenu.AddItem(m_lightsControlHelper);
			m_controlMenu.AddItem(m_dampingControlHelper);
			m_controlMenu.AddItem(m_helmetControlHelper);
			m_controlMenu.AddItem(m_broadcastingControlHelper);
			m_controlMenu.AddItem(m_cameraModeControlHelper);
			m_controlMenu.AddItem(m_quickLoadControlHelper);
			m_controlMenu.AddItem(m_colorPickerControlHelper);
			m_controlMenu.AddItem(m_helpControlHelper);
			if (MyMultiplayer.Static != null)
			{
				m_controlMenu.AddItem(m_playersControlHelper);
			}
			m_controlMenu.AddItem(m_blueprintControlHelper);
			m_controlMenu.AddItem(m_spawnControlHelper);
			if (MySession.Static.IsUserModerator(Sync.MyId))
			{
				m_controlMenu.AddItem(m_adminControlHelper);
			}
			AddUseObjectControl(character);
			if (MySession.Static.SurvivalMode)
			{
				m_controlMenu.AddItem(m_suicideControlHelper);
			}
		}

		private void SetupSpaceshipScreen(MyShipController ship)
		{
			m_lightsControlHelper.SetEntity(ship);
			m_dampingControlHelper.SetEntity(ship);
			m_landingGearsControlHelper.SetEntity(ship);
			m_connectorControlHelper.SetEntity(ship);
			m_reactorsControlHelper.SetEntity(ship);
			m_showBuildScreenControlHelper.SetEntity(ship);
			m_showTerminalControlHelper.SetEntity(ship);
			m_buildModeControlHelper.SetEntity(ship);
			m_inventoryControlHelper.SetEntity(ship);
			m_controlMenu = new MyGuiScreenControlMenu();
			m_controlMenu.AddItem(m_showTerminalControlHelper);
			m_controlMenu.AddItem(m_showBuildScreenControlHelper);
			m_controlMenu.AddItem(m_inventoryControlHelper);
			m_controlMenu.AddItem(m_quickLoadControlHelper);
			m_controlMenu.AddItem(m_hudToggleControlHelper);
			m_controlMenu.AddItem(m_lightsControlHelper);
			m_controlMenu.AddItem(m_dampingControlHelper);
			m_controlMenu.AddItem(m_landingGearsControlHelper);
			m_controlMenu.AddItem(m_connectorControlHelper);
			m_controlMenu.AddItem(m_reactorsControlHelper);
			m_controlMenu.AddItem(m_cameraModeControlHelper);
			m_controlMenu.AddItem(m_helpControlHelper);
			if (MyMultiplayer.Static != null)
			{
				m_controlMenu.AddItem(m_playersControlHelper);
			}
			m_controlMenu.AddItem(m_blueprintControlHelper);
			m_controlMenu.AddItem(m_spawnControlHelper);
			if (MySession.Static.IsUserModerator(Sync.MyId))
			{
				m_controlMenu.AddItem(m_adminControlHelper);
			}
		}

		private void AddUseObjectControl(MyCharacter character)
		{
			MyCharacterDetectorComponent myCharacterDetectorComponent = character.Components.Get<MyCharacterDetectorComponent>();
			if (myCharacterDetectorComponent != null)
			{
				if (myCharacterDetectorComponent.UseObject is MyUseObjectDoorTerminal || myCharacterDetectorComponent.UseObject is MyUseObjectTerminal || myCharacterDetectorComponent.UseObject is MyUseObjectTextPanel)
				{
					m_terminalControlHelper.SetLabel(MySpaceTexts.ControlMenuItemLabel_ShowControlPanel);
					m_controlMenu.AddItem(m_terminalControlHelper);
				}
				else if (myCharacterDetectorComponent.UseObject is MyUseObjectInventory)
				{
					m_terminalControlHelper.SetLabel(MySpaceTexts.ControlMenuItemLabel_OpenInventory);
					m_controlMenu.AddItem(m_terminalControlHelper);
				}
				else if (myCharacterDetectorComponent.UseObject is MyUseObjectPanelButton)
				{
					m_terminalControlHelper.SetLabel(MySpaceTexts.ControlMenuItemLabel_SetupButtons);
					m_controlMenu.AddItem(m_terminalControlHelper);
				}
			}
		}
	}
}
