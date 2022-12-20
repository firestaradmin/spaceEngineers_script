using System;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.AdminMenu
{
	internal class MyTrashVoxelTabContainer : MyTabContainer
	{
		private MyGuiControlTextbox m_textboxVoxelPlayerDistanceTrash;

		private MyGuiControlTextbox m_textboxVoxelGridDistanceTrash;

		private MyGuiControlTextbox m_textboxVoxelAgeTrash;

		private MyGuiControlCheckbox m_checkboxRevertMaterials;

		private MyGuiControlCheckbox m_checkboxRevertAsteroids;

		private MyGuiControlCheckbox m_checkboxRevertBoulders;

		private MyGuiControlCheckbox m_checkboxRevertFloatingPreset;

		private MyGuiControlCheckbox m_checkboxIgnoreNPCGrids;

		public MyTrashVoxelTabContainer(MyGuiScreenBase parentScreen)
			: base(parentScreen)
		{
			base.Control.Size = new Vector2(base.Control.Size.X, 0.4f);
			Vector2 currentPosition = -base.Control.Size * 0.5f;
			Vector2? size = parentScreen.GetSize();
			CreateVoxelTrashCheckBoxes(ref currentPosition);
			currentPosition.Y += 0.045f;
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = currentPosition,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromPlayer)
			};
			control.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromPlayer_Tooltip));
			base.Control.Controls.Add(control);
			m_textboxVoxelPlayerDistanceTrash = AddTextbox(ref currentPosition, MySession.Static.Settings.VoxelPlayerDistanceThreshold.ToString(), null, MyTabContainer.LABEL_COLOR, 0.9f, MyGuiControlTextboxType.DigitsOnly, null, "Debug", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, addToControls: false, 0m);
			base.Control.Controls.Add(m_textboxVoxelPlayerDistanceTrash);
			m_textboxVoxelPlayerDistanceTrash.Size = new Vector2(0.07f, m_textboxVoxelPlayerDistanceTrash.Size.Y);
			m_textboxVoxelPlayerDistanceTrash.PositionX = currentPosition.X + size.Value.X - m_textboxVoxelPlayerDistanceTrash.Size.X - 0.045f;
			m_textboxVoxelPlayerDistanceTrash.PositionY = currentPosition.Y;
			m_textboxVoxelPlayerDistanceTrash.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromPlayer_Tooltip));
			currentPosition.Y += 0.045f;
			MyGuiControlLabel control2 = new MyGuiControlLabel
			{
				Position = currentPosition,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromGrid)
			};
			control2.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromGrid_Tooltip));
			base.Control.Controls.Add(control2);
			m_textboxVoxelGridDistanceTrash = AddTextbox(ref currentPosition, MySession.Static.Settings.VoxelGridDistanceThreshold.ToString(), null, MyTabContainer.LABEL_COLOR, 0.9f, MyGuiControlTextboxType.DigitsOnly, null, "Debug", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, addToControls: false, 0m);
			base.Control.Controls.Add(m_textboxVoxelGridDistanceTrash);
			m_textboxVoxelGridDistanceTrash.Size = new Vector2(0.07f, m_textboxVoxelGridDistanceTrash.Size.Y);
			m_textboxVoxelGridDistanceTrash.PositionX = currentPosition.X + size.Value.X - m_textboxVoxelGridDistanceTrash.Size.X - 0.045f;
			m_textboxVoxelGridDistanceTrash.PositionY = currentPosition.Y;
			m_textboxVoxelGridDistanceTrash.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelDistanceFromGrid_Tooltip));
			currentPosition.Y += 0.045f;
			MyGuiControlLabel control3 = new MyGuiControlLabel
			{
				Position = currentPosition,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelAge)
			};
			control3.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelAge_Tooltip));
			base.Control.Controls.Add(control3);
			m_textboxVoxelAgeTrash = AddTextbox(ref currentPosition, MySession.Static.Settings.VoxelAgeThreshold.ToString(), null, MyTabContainer.LABEL_COLOR, 0.9f, MyGuiControlTextboxType.DigitsOnly, null, "Debug", MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP, addToControls: false, 0m);
			base.Control.Controls.Add(m_textboxVoxelAgeTrash);
			m_textboxVoxelAgeTrash.Size = new Vector2(0.07f, m_textboxVoxelAgeTrash.Size.Y);
			m_textboxVoxelAgeTrash.PositionX = currentPosition.X + size.Value.X - m_textboxVoxelAgeTrash.Size.X - 0.045f;
			m_textboxVoxelAgeTrash.PositionY = currentPosition.Y;
			m_textboxVoxelAgeTrash.SetTooltip(MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_VoxelAge_Tooltip));
			currentPosition.Y += 0.045f;
			float num = 0.14f;
			Vector2 currentPosition2 = currentPosition + new Vector2(num * 0.5f, 0f);
			MyGuiControlButton control4 = CreateDebugButton(ref currentPosition2, num, (!MySession.Static.Settings.VoxelTrashRemovalEnabled) ? MyCommonTexts.ScreenDebugAdminMenu_ResumeTrashButton : MyCommonTexts.ScreenDebugAdminMenu_PauseTrashButton, OnTrashVoxelButtonClicked, enabled: true, MyCommonTexts.ScreenDebugAdminMenu_PauseTrashVoxelButtonTooltip, increaseSpacing: false, addToControls: false);
			base.Control.Controls.Add(control4);
		}

		protected virtual void CreateVoxelTrashCheckBoxes(ref Vector2 currentPosition)
		{
			MyTrashRemovalFlags myTrashRemovalFlags = MyTrashRemovalFlags.RevertMaterials;
			MyGuiControlLabel control = new MyGuiControlLabel
			{
				Position = currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MySessionComponentTrash.GetName(myTrashRemovalFlags)
			};
			m_checkboxRevertMaterials = new MyGuiControlCheckbox(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_checkboxRevertMaterials.IsChecked = (MySession.Static.Settings.TrashFlags & myTrashRemovalFlags) == myTrashRemovalFlags;
			m_checkboxRevertMaterials.UserData = myTrashRemovalFlags;
			base.Control.Controls.Add(m_checkboxRevertMaterials);
			base.Control.Controls.Add(control);
			currentPosition.Y += 0.045f;
			MyTrashRemovalFlags myTrashRemovalFlags2 = MyTrashRemovalFlags.RevertAsteroids;
			control = new MyGuiControlLabel
			{
				Position = currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MySessionComponentTrash.GetName(myTrashRemovalFlags2)
			};
			m_checkboxRevertAsteroids = new MyGuiControlCheckbox(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_checkboxRevertAsteroids.IsChecked = (MySession.Static.Settings.TrashFlags & myTrashRemovalFlags2) == myTrashRemovalFlags2;
			m_checkboxRevertAsteroids.UserData = myTrashRemovalFlags2;
			base.Control.Controls.Add(m_checkboxRevertAsteroids);
			base.Control.Controls.Add(control);
			currentPosition.Y += 0.045f;
			MyTrashRemovalFlags myTrashRemovalFlags3 = MyTrashRemovalFlags.RevertBoulders;
			control = new MyGuiControlLabel
			{
				Position = currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MySessionComponentTrash.GetName(myTrashRemovalFlags3)
			};
			m_checkboxRevertBoulders = new MyGuiControlCheckbox(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_checkboxRevertBoulders.IsChecked = (MySession.Static.Settings.TrashFlags & myTrashRemovalFlags3) == myTrashRemovalFlags3;
			m_checkboxRevertBoulders.UserData = myTrashRemovalFlags3;
			MyGuiControlCheckbox checkboxRevertBoulders = m_checkboxRevertBoulders;
			checkboxRevertBoulders.IsCheckedChanged = (Action<MyGuiControlCheckbox>)Delegate.Combine(checkboxRevertBoulders.IsCheckedChanged, new Action<MyGuiControlCheckbox>(RevertBoulderCheckboxChanged));
			m_checkboxRevertBoulders.SetTooltip(string.Format(MyTexts.GetString(MySpaceTexts.ScreenDebugAdminMenu_RevertBoulderTooltip), MyGuiScreenAdminMenu.BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE));
			base.Control.Controls.Add(m_checkboxRevertBoulders);
			base.Control.Controls.Add(control);
			currentPosition.Y += 0.045f;
			MyTrashRemovalFlags myTrashRemovalFlags4 = MyTrashRemovalFlags.RevertWithFloatingsPresent;
			control = new MyGuiControlLabel
			{
				Position = currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MySessionComponentTrash.GetName(myTrashRemovalFlags4),
				IsAutoScaleEnabled = true,
				IsAutoEllipsisEnabled = true
			};
			control.SetMaxWidth(0.26f);
			control.DoEllipsisAndScaleAdjust(RecalculateSize: true, control.TextScale, resetEllipsis: true);
			m_checkboxRevertFloatingPreset = new MyGuiControlCheckbox(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_checkboxRevertFloatingPreset.IsChecked = (MySession.Static.Settings.TrashFlags & myTrashRemovalFlags4) == myTrashRemovalFlags4;
			m_checkboxRevertFloatingPreset.UserData = myTrashRemovalFlags4;
			base.Control.Controls.Add(m_checkboxRevertFloatingPreset);
			base.Control.Controls.Add(control);
			currentPosition.Y += 0.045f;
			MyTrashRemovalFlags myTrashRemovalFlags5 = MyTrashRemovalFlags.RevertCloseToNPCGrids;
			control = new MyGuiControlLabel
			{
				Position = currentPosition + new Vector2(0.001f, 0f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
				Text = MySessionComponentTrash.GetName(myTrashRemovalFlags5)
			};
			m_checkboxIgnoreNPCGrids = new MyGuiControlCheckbox(new Vector2(currentPosition.X + 0.293f, currentPosition.Y - 0.01f), null, null, isChecked: false, MyGuiControlCheckboxStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_checkboxIgnoreNPCGrids.IsChecked = (MySession.Static.Settings.TrashFlags & myTrashRemovalFlags5) == myTrashRemovalFlags5;
			m_checkboxIgnoreNPCGrids.UserData = myTrashRemovalFlags5;
			base.Control.Controls.Add(m_checkboxIgnoreNPCGrids);
			base.Control.Controls.Add(control);
		}

		private void RevertBoulderCheckboxChanged(MyGuiControlCheckbox obj)
		{
			if (obj.IsChecked)
			{
				PlayerDistanceTextChanged(m_textboxVoxelAgeTrash);
			}
		}

		private void PlayerDistanceTextChanged(MyGuiControlTextbox obj)
		{
			if (m_checkboxRevertBoulders.IsChecked)
			{
				int.TryParse(m_textboxVoxelPlayerDistanceTrash.Text, out var result);
				if (result < MyGuiScreenAdminMenu.BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE)
				{
<<<<<<< HEAD
					MyGuiControlTextbox textboxVoxelPlayerDistanceTrash = m_textboxVoxelPlayerDistanceTrash;
					int bOULDER_REVERT_MINIMUM_PLAYER_DISTANCE = MyGuiScreenAdminMenu.BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE;
					textboxVoxelPlayerDistanceTrash.Text = bOULDER_REVERT_MINIMUM_PLAYER_DISTANCE.ToString();
=======
					m_textboxVoxelPlayerDistanceTrash.Text = MyGuiScreenAdminMenu.BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE.ToString();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
		}

		private void OnTrashVoxelButtonClicked(MyGuiControlButton obj)
		{
			MySession.Static.Settings.VoxelTrashRemovalEnabled = !MySession.Static.Settings.VoxelTrashRemovalEnabled;
			if (!MySession.Static.Settings.VoxelTrashRemovalEnabled)
			{
				obj.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_ResumeTrashButton);
			}
			else
			{
				obj.Text = MyTexts.GetString(MyCommonTexts.ScreenDebugAdminMenu_PauseTrashButton);
			}
			MyGuiScreenAdminMenu.RecalcTrash();
		}

		internal override bool GetSettings(ref MyGuiScreenAdminMenu.AdminSettings settings)
		{
			if (m_textboxVoxelPlayerDistanceTrash == null || m_textboxVoxelGridDistanceTrash == null || m_textboxVoxelAgeTrash == null)
			{
				return false;
			}
			float.TryParse(m_textboxVoxelPlayerDistanceTrash.Text, out var result);
			if (m_checkboxRevertBoulders.IsChecked && result < (float)MyGuiScreenAdminMenu.BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE)
			{
				result = MyGuiScreenAdminMenu.BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE;
<<<<<<< HEAD
				MyGuiControlTextbox textboxVoxelPlayerDistanceTrash = m_textboxVoxelPlayerDistanceTrash;
				int bOULDER_REVERT_MINIMUM_PLAYER_DISTANCE = MyGuiScreenAdminMenu.BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE;
				textboxVoxelPlayerDistanceTrash.Text = bOULDER_REVERT_MINIMUM_PLAYER_DISTANCE.ToString();
=======
				m_textboxVoxelPlayerDistanceTrash.Text = MyGuiScreenAdminMenu.BOULDER_REVERT_MINIMUM_PLAYER_DISTANCE.ToString();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			float.TryParse(m_textboxVoxelGridDistanceTrash.Text, out var result2);
			int.TryParse(m_textboxVoxelAgeTrash.Text, out var result3);
			int num = 0 | ((MySession.Static.Settings.VoxelPlayerDistanceThreshold != result) ? 1 : 0) | ((MySession.Static.Settings.VoxelGridDistanceThreshold != result2) ? 1 : 0) | ((MySession.Static.Settings.VoxelAgeThreshold != result3) ? 1 : 0);
			settings.VoxelDistanceFromPlayer = result;
			settings.VoxelDistanceFromGrid = result2;
			settings.VoxelAge = result3;
			settings.VoxelEnable = MySession.Static.Settings.VoxelTrashRemovalEnabled;
			settings.Flags = (m_checkboxRevertMaterials.IsChecked ? (settings.Flags | (MyTrashRemovalFlags)m_checkboxRevertMaterials.UserData) : (settings.Flags & ~(MyTrashRemovalFlags)m_checkboxRevertMaterials.UserData));
			settings.Flags = (m_checkboxRevertAsteroids.IsChecked ? (settings.Flags | (MyTrashRemovalFlags)m_checkboxRevertAsteroids.UserData) : (settings.Flags & ~(MyTrashRemovalFlags)m_checkboxRevertAsteroids.UserData));
			settings.Flags = (m_checkboxRevertBoulders.IsChecked ? (settings.Flags | (MyTrashRemovalFlags)m_checkboxRevertBoulders.UserData) : (settings.Flags & ~(MyTrashRemovalFlags)m_checkboxRevertBoulders.UserData));
			settings.Flags = (m_checkboxRevertFloatingPreset.IsChecked ? (settings.Flags | (MyTrashRemovalFlags)m_checkboxRevertFloatingPreset.UserData) : (settings.Flags & ~(MyTrashRemovalFlags)m_checkboxRevertFloatingPreset.UserData));
			settings.Flags = (m_checkboxIgnoreNPCGrids.IsChecked ? (settings.Flags | (MyTrashRemovalFlags)m_checkboxIgnoreNPCGrids.UserData) : (settings.Flags & ~(MyTrashRemovalFlags)m_checkboxIgnoreNPCGrids.UserData));
			return (byte)((uint)num | ((MySession.Static.Settings.TrashFlags != settings.Flags) ? 1u : 0u)) != 0;
		}
	}
}
