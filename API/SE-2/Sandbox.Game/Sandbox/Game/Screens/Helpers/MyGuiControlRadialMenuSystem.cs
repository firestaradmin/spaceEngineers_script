using System;
using System.Collections.Generic;
using Sandbox.Graphics;
using VRage;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	internal class MyGuiControlRadialMenuSystem : MyGuiControlRadialMenuBase
	{
		public MyGuiControlRadialMenuSystem(MyRadialMenu data, MyStringId closingControl, Func<bool> handleInputCallback)
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
				Vector2 vector = m_icons[m_selectedButton].Position * 1.92f;
				Vector2 zero = Vector2.Zero;
				Vector2 vector2 = new Vector2(0f, 0.025f);
				Vector2 vector3 = Vector2.Zero;
				int num = ((!((double)Math.Abs(vector.X) < 0.05)) ? ((Math.Sign(vector.X) >= 0) ? 1 : (-1)) : 0);
				int num2 = ((!((double)Math.Abs(vector.Y) < 0.05)) ? ((Math.Sign(vector.Y) >= 0) ? 1 : (-1)) : 0);
				MyGuiDrawAlignEnum originAlign = (MyGuiDrawAlignEnum)(3 * (-num + 1) + (-num2 + 1));
				MyGuiDrawAlignEnum originAlign2 = (MyGuiDrawAlignEnum)(-num2 + 1);
				float num3 = MyGuiManager.MeasureString(m_tooltipShortcut.Font, m_tooltipShortcut.TextToDraw, m_tooltipShortcut.TextScale).Y + 0.005f;
				if (string.IsNullOrEmpty(m_tooltipState.Text))
				{
					num3 -= vector2.Y;
				}
				Vector2 vector4 = default(Vector2);
				switch (num2)
				{
				case -1:
					if (!string.IsNullOrEmpty(label.State))
					{
						zero -= vector2;
					}
					if (!string.IsNullOrEmpty(label.Shortcut))
					{
						zero -= vector2;
					}
					vector4.Y += num3;
					break;
				case 0:
					if (!string.IsNullOrEmpty(label.State))
					{
						zero -= vector2;
					}
					vector4.Y += num3 * 0.75f;
					break;
				case 1:
					vector4.Y += num3 * 0.5f;
					break;
				}
				Vector2 vector5 = Vector2.Zero;
				_ = Vector2.Zero;
				switch (num)
				{
				case -1:
					vector3 = new Vector2(0f - m_tooltipName.Size.X, 0f);
					if (m_tooltipState.Size.X > m_tooltipName.Size.X)
					{
						vector5 = new Vector2(m_tooltipName.Size.X - m_tooltipState.Size.X, 0f);
					}
					if (m_tooltipShortcut.Size.X > m_tooltipName.Size.X)
					{
						new Vector2(m_tooltipName.Size.X - m_tooltipShortcut.Size.X, 0f);
					}
					break;
				case 0:
					vector3 = new Vector2(-0.5f * m_tooltipName.Size.X, 0f);
					break;
				case 1:
					vector3 = Vector2.Zero;
					break;
				}
				m_tooltipName.Position = vector + zero;
				m_tooltipState.Position = m_tooltipName.Position + vector2 + vector3 + vector5;
				m_tooltipShortcut.Position = m_tooltipState.Position + vector4;
				m_tooltipName.OriginAlign = originAlign;
				m_tooltipState.OriginAlign = originAlign2;
				m_tooltipShortcut.OriginAlign = originAlign2;
				m_tooltipName.Visible = true;
				m_tooltipState.Visible = true;
				m_tooltipShortcut.Visible = true;
				m_tooltipState.ColorMask = (myRadialMenuItem.Enabled() ? Color.White : Color.Red);
			}
			else
			{
				m_tooltipName.Visible = false;
				m_tooltipState.Visible = false;
				m_tooltipShortcut.Visible = false;
			}
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			base.HandleInput(receivedFocusInThisUpdate);
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL_MOD1))
			{
				Cancel();
			}
		}
	}
}
