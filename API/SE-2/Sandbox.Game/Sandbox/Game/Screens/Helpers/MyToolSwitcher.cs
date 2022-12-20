using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	public class MyToolSwitcher : MySessionComponentBase
	{
		public enum ToolType
		{
			Drill,
			Welder,
			Grinder,
			Weapon
		}

		private static readonly MyTimeSpan CONFIG_PRESS_LENGTH = MyTimeSpan.FromMilliseconds(660.0);

		private List<MyHandDrillDefinition> m_availableDrills = new List<MyHandDrillDefinition>();

		private List<MyWelderDefinition> m_availableWelders = new List<MyWelderDefinition>();

		private List<MyAngleGrinderDefinition> m_availableGrinders = new List<MyAngleGrinderDefinition>();

		private List<MyPhysicalItemDefinition> m_availableWeapons = new List<MyPhysicalItemDefinition>();

		private MyStringId m_lastShipControl = MyStringId.NullOrEmpty;

		private MyTimeSpan m_lastShipControlPressed = MyTimeSpan.Zero;

		private MyDefinitionId? m_lastWeaponId;

		public bool SwitchingEnabled { get; set; }

		public event Action<bool> ToolSwitched;

		public event Action ToolsRefreshed;

		public void RefreshAvailableTools()
		{
			m_availableDrills.Clear();
			m_availableWelders.Clear();
			m_availableGrinders.Clear();
			m_availableWeapons.Clear();
			foreach (MyPhysicalItemDefinition weaponDefinition in MyDefinitionManager.Static.GetWeaponDefinitions())
			{
				if (weaponDefinition.Public && ((MySession.Static.LocalCharacter?.FindWeaponItemByDefinition(weaponDefinition.Id).HasValue ?? false) || MySession.Static.CreativeMode))
				{
					MyHandItemDefinition myHandItemDefinition = MyDefinitionManager.Static.TryGetHandItemForPhysicalItem(weaponDefinition.Id);
					MyHandDrillDefinition item;
					MyWelderDefinition item2;
					MyAngleGrinderDefinition item3;
					if ((item = myHandItemDefinition as MyHandDrillDefinition) != null)
					{
						m_availableDrills.Add(item);
					}
					else if ((item2 = myHandItemDefinition as MyWelderDefinition) != null)
					{
						m_availableWelders.Add(item2);
					}
					else if ((item3 = myHandItemDefinition as MyAngleGrinderDefinition) != null)
					{
						m_availableGrinders.Add(item3);
					}
					else
					{
						m_availableWeapons.Add(weaponDefinition);
					}
				}
			}
			m_availableDrills.SortNoAlloc((MyHandDrillDefinition x, MyHandDrillDefinition y) => y.SpeedMultiplier.CompareTo(x.DistanceMultiplier));
			m_availableWelders.SortNoAlloc((MyWelderDefinition x, MyWelderDefinition y) => y.SpeedMultiplier.CompareTo(x.SpeedMultiplier));
			m_availableGrinders.SortNoAlloc((MyAngleGrinderDefinition x, MyAngleGrinderDefinition y) => y.SpeedMultiplier.CompareTo(x.SpeedMultiplier));
			this.ToolsRefreshed?.Invoke();
		}

		public override void HandleInput()
		{
			_ = MySession.Static.LocalCharacter?.CurrentWeapon;
			if (MyScreenManager.GetScreenWithFocus() != MyGuiScreenGamePlay.Static)
			{
				return;
			}
			MyStringId myStringId = MySession.Static.ControlledEntity?.AuxiliaryContext ?? MyStringId.NullOrEmpty;
			if (myStringId != MySpaceBindingCreator.AX_TOOLS && myStringId != MySpaceBindingCreator.AX_ACTIONS)
			{
				return;
			}
			if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.SLOT0))
			{
				MySession.Static.LocalCharacter?.SwitchToWeapon((MyToolbarItemWeapon)null);
				return;
			}
			RefreshAvailableTools();
			if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.TOOL_UP))
			{
				SwitchToDrill();
			}
			if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.TOOL_LEFT))
			{
				SwitchToGrinder();
			}
			if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.TOOL_RIGHT))
			{
				SwitchToWelder();
			}
			if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.TOOL_DOWN))
			{
				SwitchToWeapon();
			}
			MyTimeSpan myTimeSpan = MyTimeSpan.FromMilliseconds(MySession.Static.ElapsedGameTime.TotalMilliseconds);
			if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.ACTION_UP))
			{
				m_lastShipControl = MyControlsSpace.ACTION_UP;
				m_lastShipControlPressed = myTimeSpan;
			}
			else if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.ACTION_UP, MyControlStateType.NEW_RELEASED))
			{
				ActivateGamepadToolbar(0);
			}
			else if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.ACTION_UP, MyControlStateType.PRESSED) && m_lastShipControl == MyControlsSpace.ACTION_UP && myTimeSpan - m_lastShipControlPressed > CONFIG_PRESS_LENGTH)
			{
				OpenToolbarConfig(0);
			}
			if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.ACTION_LEFT))
			{
				m_lastShipControl = MyControlsSpace.ACTION_LEFT;
				m_lastShipControlPressed = myTimeSpan;
			}
			else if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.ACTION_LEFT, MyControlStateType.NEW_RELEASED))
			{
				ActivateGamepadToolbar(1);
			}
			else if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.ACTION_LEFT, MyControlStateType.PRESSED) && m_lastShipControl == MyControlsSpace.ACTION_LEFT && myTimeSpan - m_lastShipControlPressed > CONFIG_PRESS_LENGTH)
			{
				OpenToolbarConfig(1);
			}
			if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.ACTION_RIGHT))
			{
				m_lastShipControl = MyControlsSpace.ACTION_RIGHT;
				m_lastShipControlPressed = myTimeSpan;
			}
			else if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.ACTION_RIGHT, MyControlStateType.NEW_RELEASED))
			{
				ActivateGamepadToolbar(2);
			}
			else if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.ACTION_RIGHT, MyControlStateType.PRESSED) && m_lastShipControl == MyControlsSpace.ACTION_RIGHT && myTimeSpan - m_lastShipControlPressed > CONFIG_PRESS_LENGTH)
			{
				OpenToolbarConfig(2);
			}
			if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.ACTION_DOWN))
			{
				m_lastShipControl = MyControlsSpace.ACTION_DOWN;
				m_lastShipControlPressed = myTimeSpan;
			}
			else if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.ACTION_DOWN, MyControlStateType.NEW_RELEASED))
			{
				ActivateGamepadToolbar(3);
			}
			else if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.ACTION_DOWN, MyControlStateType.PRESSED) && m_lastShipControl == MyControlsSpace.ACTION_DOWN && myTimeSpan - m_lastShipControlPressed > CONFIG_PRESS_LENGTH)
			{
				OpenToolbarConfig(3);
			}
			if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.TOOLBAR_PREVIOUS))
			{
				MyToolbarComponent.CurrentToolbar.PageDownGamepad();
			}
			if (MyControllerHelper.IsControl(myStringId, MyControlsSpace.TOOLBAR_NEXT))
			{
				MyToolbarComponent.CurrentToolbar.PageUpGamepad();
			}
			base.HandleInput();
		}

		private void OpenToolbarConfig(int id)
		{
			if (!(MySession.Static.ControlledEntity is MyCryoChamber))
			{
				int num = ComputeToolbarId(id);
				MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, 0, MySession.Static.ControlledEntity as MyShipController, num));
			}
		}

		private void ActivateGamepadToolbar(int id)
		{
			int index = ComputeToolbarId(id);
			MyToolbarComponent.CurrentToolbar.ActivateGamepadItemAtIndex(index);
		}

		private int ComputeToolbarId(int slot)
		{
			return MyToolbarComponent.CurrentToolbar.SlotToIndexGamepad(slot);
		}

		public void SwitchToDrill()
		{
			SwitchToTool(m_availableDrills);
		}

		public void SwitchToWelder()
		{
			SwitchToTool(m_availableWelders);
		}

		public void SwitchToGrinder()
		{
			SwitchToTool(m_availableGrinders);
		}

		public void SwitchToWeapon()
		{
			SwitchToTool(m_availableWeapons, weapon: true);
		}

		public MyDefinitionId? GetCurrentOrNextTool(ToolType type)
		{
			return type switch
			{
				ToolType.Drill => GetTool(m_availableDrills, next: false), 
				ToolType.Welder => GetTool(m_availableWelders, next: false), 
				ToolType.Grinder => GetTool(m_availableGrinders, next: false), 
				ToolType.Weapon => GetTool(m_availableWeapons, next: false, weapon: true), 
				_ => null, 
			};
		}

		private MyDefinitionId? GetTool<T>(List<T> type, bool next, bool weapon = false) where T : MyDefinitionBase
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter == null || type.Count == 0)
			{
				return null;
			}
			IMyHandheldGunObject<MyDeviceBase> currentWeapon = localCharacter.CurrentWeapon;
			MyDefinitionId currentTool = default(MyDefinitionId);
			int num = -1;
			if (weapon)
			{
				if (currentWeapon == null || type.FindIndex((T x) => x.Id == MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId).Id) == -1)
				{
					if (m_lastWeaponId.HasValue)
					{
						currentTool = m_lastWeaponId.Value;
						num = Math.Max(type.FindIndex((T x) => x.Id == currentTool), 0);
					}
					else
					{
						num = 0;
					}
				}
				else
				{
					num = type.FindIndex((T x) => x.Id == MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId).Id);
					if (next || num == -1)
					{
						num = (num + 1) % type.Count;
					}
				}
			}
			else
			{
				currentTool = ((currentWeapon != null) ? currentWeapon.DefinitionId : default(MyDefinitionId));
				num = type.FindIndex((T x) => x.Id == currentTool);
				if (next || num == -1)
				{
					num = (num + 1) % type.Count;
				}
			}
			return type[num].Id;
		}

		private void SwitchToTool<T>(List<T> type, bool weapon = false) where T : MyDefinitionBase
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			if (localCharacter == null)
<<<<<<< HEAD
			{
				return;
			}
			RefreshAvailableTools();
			if (type.Count != 0)
			{
				MyDefinitionId? tool = GetTool(type, next: true, weapon);
				localCharacter.SwitchToWeapon(tool);
				if (weapon)
				{
					m_lastWeaponId = tool;
				}
				this.ToolSwitched?.Invoke(obj: false);
=======
			{
				return;
			}
			RefreshAvailableTools();
			if (type.Count != 0)
			{
				MyDefinitionId? tool = GetTool(type, next: true, weapon);
				localCharacter.SwitchToWeapon(tool);
				if (weapon)
				{
					m_lastWeaponId = tool;
				}
				this.ToolSwitched?.Invoke(obj: false);
			}
		}

		internal bool IsEquipped(ToolType tool)
		{
			switch (tool)
			{
			case ToolType.Drill:
			{
				MyCharacter localCharacter3 = MySession.Static.LocalCharacter;
				if (localCharacter3 == null || localCharacter3.CurrentWeapon == null || m_availableDrills.Count <= 0)
				{
					return false;
				}
				MyDefinitionId toolId3 = localCharacter3.CurrentWeapon.DefinitionId;
				return m_availableDrills.FindIndex((MyHandDrillDefinition x) => x.Id == toolId3) != -1;
			}
			case ToolType.Welder:
			{
				MyCharacter localCharacter4 = MySession.Static.LocalCharacter;
				if (localCharacter4 == null || localCharacter4.CurrentWeapon == null || m_availableDrills.Count <= 0)
				{
					return false;
				}
				MyDefinitionId toolId4 = localCharacter4.CurrentWeapon.DefinitionId;
				return m_availableWelders.FindIndex((MyWelderDefinition x) => x.Id == toolId4) != -1;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			case ToolType.Grinder:
			{
				MyCharacter localCharacter2 = MySession.Static.LocalCharacter;
				if (localCharacter2 == null || localCharacter2.CurrentWeapon == null || m_availableDrills.Count <= 0)
				{
					return false;
				}
				MyDefinitionId toolId2 = localCharacter2.CurrentWeapon.DefinitionId;
				return m_availableGrinders.FindIndex((MyAngleGrinderDefinition x) => x.Id == toolId2) != -1;
			}
			case ToolType.Weapon:
			{
				MyCharacter localCharacter = MySession.Static.LocalCharacter;
				if (localCharacter == null || localCharacter.CurrentWeapon == null || m_availableDrills.Count <= 0)
				{
					return false;
				}
				MyDefinitionId toolId = MyDefinitionManager.Static.GetPhysicalItemForHandItem(localCharacter.CurrentWeapon.DefinitionId).Id;
				return m_availableWeapons.FindIndex((MyPhysicalItemDefinition x) => x.Id == toolId) != -1;
			}
			default:
				return false;
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Session = null;
		}

		internal bool IsEquipped(ToolType tool)
		{
			switch (tool)
			{
			case ToolType.Drill:
			{
				MyCharacter localCharacter3 = MySession.Static.LocalCharacter;
				if (localCharacter3 == null || localCharacter3.CurrentWeapon == null || m_availableDrills.Count <= 0)
				{
					return false;
				}
				MyDefinitionId toolId3 = localCharacter3.CurrentWeapon.DefinitionId;
				return m_availableDrills.FindIndex((MyHandDrillDefinition x) => x.Id == toolId3) != -1;
			}
			case ToolType.Welder:
			{
				MyCharacter localCharacter4 = MySession.Static.LocalCharacter;
				if (localCharacter4 == null || localCharacter4.CurrentWeapon == null || m_availableDrills.Count <= 0)
				{
					return false;
				}
				MyDefinitionId toolId4 = localCharacter4.CurrentWeapon.DefinitionId;
				return m_availableWelders.FindIndex((MyWelderDefinition x) => x.Id == toolId4) != -1;
			}
			case ToolType.Grinder:
			{
				MyCharacter localCharacter2 = MySession.Static.LocalCharacter;
				if (localCharacter2 == null || localCharacter2.CurrentWeapon == null || m_availableDrills.Count <= 0)
				{
					return false;
				}
				MyDefinitionId toolId2 = localCharacter2.CurrentWeapon.DefinitionId;
				return m_availableGrinders.FindIndex((MyAngleGrinderDefinition x) => x.Id == toolId2) != -1;
			}
			case ToolType.Weapon:
			{
				MyCharacter localCharacter = MySession.Static.LocalCharacter;
				if (localCharacter == null || localCharacter.CurrentWeapon == null || m_availableDrills.Count <= 0)
				{
					return false;
				}
				MyDefinitionId toolId = MyDefinitionManager.Static.GetPhysicalItemForHandItem(localCharacter.CurrentWeapon.DefinitionId).Id;
				return m_availableWeapons.FindIndex((MyPhysicalItemDefinition x) => x.Id == toolId) != -1;
			}
			default:
				return false;
			}
		}

		protected override void UnloadData()
		{
			base.UnloadData();
			Session = null;
		}
	}
}
