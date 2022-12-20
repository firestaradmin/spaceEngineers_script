using System.Collections.Generic;
using System.Reflection;
using Sandbox.Game.Screens.Helpers.RadialMenuActions;
using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.Game.Screens.Helpers
{
	internal static class MyRadialMenuItemFactory
	{
		private static MyObjectFactory<MyRadialMenuItemDescriptor, MyRadialMenuItem> m_objectFactory;

		private static Dictionary<MySystemAction, IMyRadialMenuSystemAction> m_systemActions;

		static MyRadialMenuItemFactory()
		{
			m_objectFactory = new MyObjectFactory<MyRadialMenuItemDescriptor, MyRadialMenuItem>();
			m_objectFactory.RegisterFromAssembly(Assembly.GetAssembly(typeof(MyRadialMenuItem)));
			m_systemActions = new Dictionary<MySystemAction, IMyRadialMenuSystemAction>();
			m_systemActions.Add(MySystemAction.ActiveContractsScreen, new MyActionActiveContractsScreen());
			m_systemActions.Add(MySystemAction.AdminMenu, new MyActionAdminMenu());
			m_systemActions.Add(MySystemAction.BlueprintsScreen, new MyActionBlueprintScreen());
			m_systemActions.Add(MySystemAction.Chat, new MyActionChat());
			m_systemActions.Add(MySystemAction.ColorPicker, new MyActionColorPicker());
			m_systemActions.Add(MySystemAction.ColorTool, new MyActionColorTool());
			m_systemActions.Add(MySystemAction.CopyGrid, new MyActionCopyGrid());
			m_systemActions.Add(MySystemAction.CreateBlueprint, new MyActionCreateBlueprint());
			m_systemActions.Add(MySystemAction.CutGrid, new MyActionCutGrid());
			m_systemActions.Add(MySystemAction.OpenInventory, new MyActionOpenInventory());
			m_systemActions.Add(MySystemAction.PasteGrid, new MyActionPasteGrid());
			m_systemActions.Add(MySystemAction.PlacementMode, new MyActionPlacementMode());
			m_systemActions.Add(MySystemAction.ReloadSession, new MyActionReloadSession());
			m_systemActions.Add(MySystemAction.Respawn, new MyActionRespawn());
			m_systemActions.Add(MySystemAction.ShowHelpScreen, new MyActionShowHelpScreen());
			m_systemActions.Add(MySystemAction.ShowPlayers, new MyActionShowPlayers());
			m_systemActions.Add(MySystemAction.ShowProgressionTree, new MyActionShowProgressionTree());
			m_systemActions.Add(MySystemAction.SpawnMenu, new MyActionSpawnMenu());
			m_systemActions.Add(MySystemAction.SwitchCamera, new MyActionSwitchCamera());
			m_systemActions.Add(MySystemAction.SymmetrySetup, new MyActionSymmetrySetup());
			m_systemActions.Add(MySystemAction.ToggleAutoRotation, new MyActionToggleAutoRotation());
			m_systemActions.Add(MySystemAction.ToggleBroadcasting, new MyActionToggleBroadcasting());
			m_systemActions.Add(MySystemAction.ToggleConnectors, new MyActionToggleConnectors());
			m_systemActions.Add(MySystemAction.ToggleDampeners, new MyActionToggleDampeners());
			m_systemActions.Add(MySystemAction.ToggleHud, new MyActionToggleHud());
			m_systemActions.Add(MySystemAction.ToggleLights, new MyActionToggleLights());
			m_systemActions.Add(MySystemAction.ToggleMultiBlock, new MyActionToggleMultiBlock());
			m_systemActions.Add(MySystemAction.TogglePower, new MyActionTogglePower());
			m_systemActions.Add(MySystemAction.ToggleSignals, new MyActionToggleSignals());
			m_systemActions.Add(MySystemAction.ToggleSymmetry, new MyActionToggleSymmetry());
			m_systemActions.Add(MySystemAction.ToggleVisor, new MyActionToggleVisor());
			m_systemActions.Add(MySystemAction.Unequip, new MyActionUnequip());
			m_systemActions.Add(MySystemAction.ViewMode, new MyActionViewMode());
			m_systemActions.Add(MySystemAction.ToggleHandbrake, new MyActionToggleHandbrake());
		}

		public static MyRadialMenuItem CreateRadialMenuItem(MyObjectBuilder_RadialMenuItem data)
		{
			MyRadialMenuItem myRadialMenuItem = m_objectFactory.CreateInstance(data.TypeId);
			myRadialMenuItem.Init(data);
			return myRadialMenuItem;
		}

		internal static IMyRadialMenuSystemAction GetSystemMenuAction(MySystemAction actionKey)
		{
			if (!m_systemActions.TryGetValue(actionKey, out var value))
			{
				return null;
			}
			return value;
		}
	}
}
