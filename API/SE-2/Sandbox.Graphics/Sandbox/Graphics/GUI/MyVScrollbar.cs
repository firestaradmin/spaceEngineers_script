using System;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyVScrollbar : MyScrollbar
	{
		private Vector2 m_dragClick;

		private float m_speedMultiplier = 1f;

		public float SpeedMultiplier
		{
			get
			{
				return m_speedMultiplier;
			}
			set
			{
				m_speedMultiplier = value;
			}
		}

		public MyVScrollbar(MyGuiControlBase control)
			: base(control, MyGuiConstants.TEXTURE_SCROLLBAR_V_THUMB, MyGuiConstants.TEXTURE_SCROLLBAR_V_THUMB_HIGHLIGHT, MyGuiConstants.TEXTURE_SCROLLBAR_V_BACKGROUND)
		{
		}

		private Vector2 GetCarretPosition()
		{
			return new Vector2(0f, base.Value * (base.Size.Y - m_caretSize.Y) / (m_max - m_page));
		}

		public override void Layout(Vector2 positionTopLeft, float length)
		{
			m_position = positionTopLeft;
			base.Size = new Vector2(m_texture.MinSizeGui.X, length);
			if (CanScroll())
			{
				m_caretSize = new Vector2(m_texture.MinSizeGui.X, MathHelper.Clamp(m_page / m_max * length, m_texture.MinSizeGui.Y, m_texture.MaxSizeGui.Y));
				m_caretPageSize = new Vector2(m_texture.MinSizeGui.X, MathHelper.Clamp(m_page, m_texture.MinSizeGui.Y, m_texture.MaxSizeGui.Y));
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
					m_texture.Draw(vector + carretPosition, m_caretSize, colorMask);
				}
			}
		}

		public override bool HandleInput(bool fakeFocus = false)
		{
			m_captured = false;
			if (!Visible || !CanScroll())
			{
				return m_captured;
			}
<<<<<<< HEAD
			Vector2 positionAbsoluteCenter = m_ownerControl.GetPositionAbsoluteCenter();
			Vector2 vector = positionAbsoluteCenter + m_position;
			bool flag = MyGuiControlBase.CheckMouseOver(base.Size, vector, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			bool flag2 = MyGuiControlBase.CheckMouseOver(m_ownerControl.Size, positionAbsoluteCenter, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			base.IsOverCaret = MyGuiControlBase.CheckMouseOver(m_caretSize, vector + GetCarretPosition(), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
=======
			Vector2 positionAbsoluteCenter = OwnerControl.GetPositionAbsoluteCenter();
			Vector2 vector = positionAbsoluteCenter + Position;
			bool flag = MyGuiControlBase.CheckMouseOver(base.Size, vector, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			bool flag2 = MyGuiControlBase.CheckMouseOver(OwnerControl.Size, positionAbsoluteCenter, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER);
			base.IsOverCaret = MyGuiControlBase.CheckMouseOver(CaretSize, vector + GetCarretPosition(), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			base.IsInDomainCaret = MyGuiControlBase.CheckMouseOver(base.Size, vector, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			base.HasHighlight = base.IsOverCaret;
			switch (m_state)
			{
			case StateEnum.Ready:
				if (MyInput.Static.IsNewPrimaryButtonPressed() && base.IsOverCaret)
				{
					m_captured = true;
					m_state = StateEnum.Drag;
					m_dragClick = MyGuiManager.MouseCursorPosition;
				}
				else if (MyInput.Static.IsNewPrimaryButtonPressed() && base.IsInDomainCaret)
				{
					m_captured = true;
					m_dragClick = MyGuiManager.MouseCursorPosition;
					m_state = StateEnum.Click;
				}
				break;
			case StateEnum.Drag:
				if (!MyInput.Static.IsPrimaryButtonPressed())
				{
					m_state = StateEnum.Ready;
				}
				else
				{
					ChangeValue((MyGuiManager.MouseCursorPosition.Y - m_dragClick.Y) * (m_max - m_page) / (base.Size.Y - m_caretSize.Y));
					m_dragClick = MyGuiManager.MouseCursorPosition;
				}
				m_captured = true;
				break;
			case StateEnum.Click:
			{
				m_dragClick = MyGuiManager.MouseCursorPosition;
				Vector2 vector2 = GetCarretPosition() + vector + m_caretSize / 2f;
				float amount = (m_dragClick.Y - vector2.Y) * (m_max - m_page) / (base.Size.Y - m_caretSize.Y);
				ChangeValue(amount);
				m_state = StateEnum.Ready;
				break;
			}
			}
			if (flag || flag2)
			{
				int num = MyInput.Static.DeltaMouseScrollWheelValue();
				if (num != 0 && num != -MyInput.Static.PreviousMouseScrollWheelValue())
				{
					ChangeValue((float)num / -120f * m_page * 0.25f);
				}
			}
			if (m_ownerControl.HasFocus || fakeFocus)
			{
				float num2 = MyControllerHelper.IsControlAnalog(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_DOWN) - MyControllerHelper.IsControlAnalog(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_UP);
				if (Math.Abs(num2) > float.Epsilon)
				{
					ChangeValue(MyControllerHelper.GAMEPAD_ANALOG_SCROLL_SPEED * num2 * SpeedMultiplier * m_page);
				}
			}
<<<<<<< HEAD
			return m_captured;
=======
			if (OwnerControl.HasFocus || fakeFocus)
			{
				float num2 = MyControllerHelper.IsControlAnalog(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_DOWN) - MyControllerHelper.IsControlAnalog(MyControllerHelper.CX_GUI, MyControlsGUI.SCROLL_UP);
				if (Math.Abs(num2) > float.Epsilon)
				{
					ChangeValue(MyControllerHelper.GAMEPAD_ANALOG_SCROLL_SPEED * num2 * SpeedMultiplier * Page);
				}
			}
			return result;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		protected override void RefreshInternals()
		{
			base.RefreshInternals();
			base.Size = new Vector2(m_texture.MinSizeGui.X, base.Size.Y);
		}
	}
}
