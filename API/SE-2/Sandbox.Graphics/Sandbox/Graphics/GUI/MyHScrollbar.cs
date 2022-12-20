using System;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyHScrollbar : MyScrollbar
	{
		private Vector2 m_dragClick;

		public bool EnableWheelScroll;

		public MyHScrollbar(MyGuiControlBase control)
			: base(control, MyGuiConstants.TEXTURE_SCROLLBAR_H_THUMB, MyGuiConstants.TEXTURE_SCROLLBAR_H_THUMB_HIGHLIGHT, MyGuiConstants.TEXTURE_SCROLLBAR_V_BACKGROUND)
		{
		}

		private Vector2 GetCarretPosition()
		{
			return new Vector2(base.Value * (base.Size.X - m_caretSize.X) / (m_max - m_page), 0f);
		}

		public override void Layout(Vector2 positionTopLeft, float length)
		{
			m_position = positionTopLeft;
			base.Size = new Vector2(length, m_texture.MinSizeGui.Y);
			if (CanScroll())
			{
				m_caretSize = new Vector2(MathHelper.Clamp(m_page / m_max * length, m_texture.MinSizeGui.X, m_texture.MaxSizeGui.X), m_texture.MinSizeGui.Y);
				if (base.Value > m_max - m_page)
				{
					base.Value = m_max - m_page;
				}
			}
		}

		public override void Draw(Color colorMask)
		{
			if (Visible)
			{
				Vector2 vector = m_ownerControl.GetPositionAbsoluteCenter() + m_position;
				m_backgroundTexture.Draw(vector, base.Size, colorMask);
				if (CanScroll())
				{
					Vector2 carretPosition = GetCarretPosition();
					m_texture.Draw(vector + carretPosition, m_caretSize, colorMask, ScrollBarScale);
				}
			}
		}

		public override bool HandleInput(bool fakeFocus = false)
		{
			bool result = false;
			if (!Visible || !CanScroll())
			{
				return result;
			}
			Vector2 vector = m_ownerControl.GetPositionAbsoluteCenter() + m_position;
			base.IsOverCaret = MyGuiControlBase.CheckMouseOver(m_caretSize, vector + GetCarretPosition(), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			switch (m_state)
			{
			case StateEnum.Ready:
				if (MyInput.Static.IsNewLeftMousePressed() && base.IsOverCaret)
				{
					result = true;
					m_state = StateEnum.Drag;
					m_dragClick = MyGuiManager.MouseCursorPosition;
				}
				break;
			case StateEnum.Drag:
				if (!MyInput.Static.IsLeftMousePressed())
				{
					m_state = StateEnum.Ready;
				}
				else
				{
					ChangeValue((MyGuiManager.MouseCursorPosition.X - m_dragClick.X) * (m_max - m_page) / (base.Size.X - m_caretSize.X));
					m_dragClick = MyGuiManager.MouseCursorPosition;
				}
				result = true;
				break;
			}
			if (EnableWheelScroll)
			{
				bool num = MyGuiControlBase.CheckMouseOver(base.Size, vector, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				bool isMouseOver = m_ownerControl.IsMouseOver;
				if (num || isMouseOver)
				{
					base.Value += (float)MyInput.Static.DeltaMouseScrollWheelValue() / -120f * m_page * 0.25f;
				}
			}
			if (m_ownerControl.HasFocus || fakeFocus)
			{
				float num2 = MyControllerHelper.IsControlAnalog(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_RIGHT) - MyControllerHelper.IsControlAnalog(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_LEFT);
				if (Math.Abs(num2) > float.Epsilon)
				{
					ChangeValue(MyControllerHelper.GAMEPAD_ANALOG_SCROLL_SPEED * num2);
				}
			}
			if (OwnerControl.HasFocus || fakeFocus)
			{
				float num2 = MyControllerHelper.IsControlAnalog(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_RIGHT) - MyControllerHelper.IsControlAnalog(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_LEFT);
				if (Math.Abs(num2) > float.Epsilon)
				{
					ChangeValue(MyControllerHelper.GAMEPAD_ANALOG_SCROLL_SPEED * num2);
				}
			}
			return result;
		}

		protected override void RefreshInternals()
		{
			base.RefreshInternals();
			base.Size = new Vector2(base.Size.X, m_texture.MinSizeGui.Y);
		}
	}
}
