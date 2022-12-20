using System;
using System.Collections.Generic;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	internal class MyGuiControlRadialMenuVoxel : MyGuiControlRadialMenuBase
	{
		protected static readonly string ICON_MASK_NORMAL = "Textures\\GUI\\Icons\\RadialMenu_Voxel\\MaterialMaskUnselected.dds";

		protected static readonly string ICON_MASK_HIGHLIGHT = "Textures\\GUI\\Icons\\RadialMenu_Voxel\\MaterialMaskSelected.dds";

		public MyGuiControlRadialMenuVoxel(MyRadialMenu data, MyStringId closingControl, Func<bool> handleInputCallback)
			: base(data, closingControl, handleInputCallback)
		{
			SwitchSection(MyGuiControlRadialMenuBase.m_lastSelectedSection.GetValueOrDefault(data.Id, 0));
		}

		protected override void UpdateTooltip()
		{
			List<MyRadialMenuItem> items = m_data.CurrentSections[m_currentSection].Items;
			if (m_selectedButton >= 0 && m_selectedButton < items.Count)
			{
				MyRadialMenuItem myRadialMenuItem = items[m_selectedButton];
				MyRadialLabelText label = myRadialMenuItem.Label;
				m_tooltipName.Text = MyTexts.GetString(label.Name);
				m_tooltipState.Text = MyTexts.GetString(label.State);
				m_tooltipShortcut.Text = MyTexts.GetString(label.Shortcut);
				m_tooltipName.RecalculateSize();
				m_tooltipState.RecalculateSize();
				m_tooltipShortcut.RecalculateSize();
				m_tooltipName.Position = m_icons[m_selectedButton].Position * 2.5f;
				m_tooltipState.Position = new Vector2(-0.25f, 0f);
				m_tooltipShortcut.Position = new Vector2(-0.25f, 0.05f);
				m_tooltipName.Visible = true;
				m_tooltipState.Visible = true;
				m_tooltipShortcut.Visible = true;
				m_tooltipState.ColorMask = (myRadialMenuItem.Enabled() ? Color.White : Color.Red);
				Vector2 position = m_tooltipName.Position;
				int num = ((!((double)Math.Abs(position.X) < 0.05)) ? Math.Sign(position.X) : 0);
				int num2 = ((!((double)Math.Abs(position.Y) < 0.05)) ? Math.Sign(position.Y) : 0);
				m_tooltipName.OriginAlign = (MyGuiDrawAlignEnum)(3 * (-num + 1) + 1 + num2);
			}
			else
			{
				m_tooltipName.Visible = false;
				m_tooltipState.Visible = false;
				m_tooltipShortcut.Visible = false;
			}
		}

		protected override void GenerateIcons(int maxSize)
		{
			for (int i = 0; i < maxSize; i++)
			{
				MyGuiControlImageRotatable myGuiControlImageRotatable = new MyGuiControlImageRotatable();
				myGuiControlImageRotatable.SetTexture("Textures\\GUI\\Controls\\RadialSectorUnSelected.dds", ICON_MASK_NORMAL);
				float num2 = (myGuiControlImageRotatable.Rotation = (float)Math.PI * 2f / (float)maxSize * (float)i);
				myGuiControlImageRotatable.Size = new Vector2(288f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
				myGuiControlImageRotatable.Position = new Vector2((float)Math.Cos(num2 - (float)Math.E * 449f / 777f), (float)Math.Sin(num2 - (float)Math.E * 449f / 777f)) * 144f / MyGuiConstants.GUI_OPTIMAL_SIZE;
				m_icons.Add(myGuiControlImageRotatable);
				AddControl(myGuiControlImageRotatable);
			}
		}

		protected override void SetIconTextures(MyRadialMenuSection selectedSection)
		{
			for (int i = 0; i < m_buttons.Count; i++)
			{
				MyGuiControlImageRotatable myGuiControlImageRotatable = m_buttons[i];
				MyGuiControlImage myGuiControlImage = m_icons[i];
				if (i < selectedSection.Items.Count)
				{
					bool visible = (myGuiControlImage.Visible = true);
					myGuiControlImageRotatable.Visible = visible;
					MyRadialMenuItem myRadialMenuItem = selectedSection.Items[i];
					myGuiControlImage.SetTexture(myRadialMenuItem.GetIcon(), "Textures\\GUI\\Icons\\RadialMenu_Voxel\\MaterialMaskUnselected.dds");
					myGuiControlImage.ColorMask = (myRadialMenuItem.Enabled() ? Color.White : Color.Gray);
				}
				else
				{
					myGuiControlImage.Visible = false;
				}
			}
		}

		protected override void UpdateHighlight(int oldIndex, int newIndex)
		{
			base.UpdateHighlight(oldIndex, newIndex);
			if (oldIndex != -1)
			{
				m_icons[oldIndex].SetTexture(m_icons[oldIndex].Textures[0].Texture, ICON_MASK_NORMAL);
			}
			if (newIndex != -1)
			{
				m_icons[newIndex].SetTexture(m_icons[newIndex].Textures[0].Texture, ICON_MASK_HIGHLIGHT);
			}
		}

		public override void GetTexturesForPreload(HashSet<string> textures)
		{
			textures.Add(ICON_MASK_HIGHLIGHT);
			base.GetTexturesForPreload(textures);
		}

		protected override void ActivateItem(MyRadialMenuItem item)
		{
			item.Activate();
			MySession.Static.GetComponent<MyRadialMenuComponent>().PushLastUsedVoxel(item as MyRadialMenuItemVoxelHand);
		}
	}
}
