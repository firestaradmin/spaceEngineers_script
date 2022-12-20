using System;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlBlockProperty : MyGuiControlBase
	{
		private readonly MyGuiControlBlockPropertyLayoutEnum m_layout;

		private MyGuiControlLabel m_title;

		private MyGuiControlLabel m_extraInfo;

		private MyGuiControlBase m_propertyControl;

		private readonly float m_titleHeight;

		public MyGuiControlLabel TitleLabel => m_title;

		public MyGuiControlBase PropertyControl => m_propertyControl;

		public MyGuiControlLabel ExtraInfoLabel => m_extraInfo;

		public MyGuiControlBlockProperty(string title, string tooltip, MyGuiControlBase propertyControl, MyGuiControlBlockPropertyLayoutEnum layout = MyGuiControlBlockPropertyLayoutEnum.Vertical, bool showExtraInfo = true, bool isAutoEllipsisEnabled = false, bool isAutoScaleEnabled = false, float max_Width = float.PositiveInfinity)
			: base(null, null, null, tooltip, null, isActiveControl: false, canHaveFocus: true)
		{
			bool isAutoScaleEnabled2 = isAutoScaleEnabled;
<<<<<<< HEAD
			m_title = new MyGuiControlLabel(null, null, title, null, 0.76f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP, isAutoEllipsisEnabled, max_Width, isAutoScaleEnabled2)
			{
				Margin = new Thickness(0.005f, 0f, 0f, 0f)
			};
=======
			m_title = new MyGuiControlLabel(null, null, title, null, 0.76f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP, isAutoEllipsisEnabled, max_Width, isAutoScaleEnabled2);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (title.Length > 0)
			{
				Elements.Add(m_title);
			}
			m_title.RecalculateSize();
			m_extraInfo = new MyGuiControlLabel(null, null, null, null, 0.76f);
			if (showExtraInfo)
			{
				Elements.Add(m_extraInfo);
			}
			m_propertyControl = propertyControl;
			Elements.Add(m_propertyControl);
			m_titleHeight = ((title.Length > 0 || showExtraInfo) ? m_title.Size.Y : 0f);
			m_layout = layout;
			switch (layout)
			{
			case MyGuiControlBlockPropertyLayoutEnum.Horizontal:
				base.MinSize = new Vector2(m_propertyControl.Size.X + m_title.Size.X * 1.1f, Math.Max(m_propertyControl.Size.Y, 2.1f * m_titleHeight));
				m_title.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				m_propertyControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
				m_extraInfo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				break;
			case MyGuiControlBlockPropertyLayoutEnum.Vertical:
				base.MinSize = new Vector2(Math.Max(m_propertyControl.Size.X, m_title.Size.X), m_propertyControl.Size.Y + m_titleHeight * 1.8f);
				if (IsOnOffSwitch() && m_layout == MyGuiControlBlockPropertyLayoutEnum.Vertical)
				{
					m_title.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
				}
				else
				{
					m_title.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				}
				m_propertyControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				m_extraInfo.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP;
				break;
			}
			base.Size = base.MinSize;
			m_extraInfo.Text = "";
			m_extraInfo.Visible = false;
			base.CanFocusChildren = true;
		}

		private bool IsOnOffSwitch()
		{
			return m_propertyControl.GetType().Name == "MyGuiControlOnOffSwitch";
		}

		public override MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyDirection direction, bool page)
		{
			if (currentFocusControl == m_propertyControl)
			{
				return null;
			}
			if (m_propertyControl.CanFocusChildren)
			{
				return m_propertyControl.GetNextFocusControl(currentFocusControl, direction, page);
			}
			return m_propertyControl;
		}

		public override void OnRemoving()
		{
			ClearEvents();
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			if (myGuiControlBase == null)
			{
				myGuiControlBase = HandleInputElements();
			}
			if (myGuiControlBase == null && base.HasFocus)
			{
				myGuiControlBase = m_propertyControl.HandleInput();
			}
			return myGuiControlBase;
		}

		protected override void OnSizeChanged()
		{
			RefreshPositionsAndSizes();
			base.OnSizeChanged();
		}

		private void RefreshPositionsAndSizes()
		{
			switch (m_layout)
			{
			case MyGuiControlBlockPropertyLayoutEnum.Horizontal:
				m_title.Position = new Vector2(base.Size.X * -0.5f, base.Size.Y * -0.25f);
				m_extraInfo.Position = m_title.Position + new Vector2(0f, m_titleHeight * 1.05f);
				m_propertyControl.Position = new Vector2(base.Size.X * 0.505f, base.Size.Y * -0.5f);
				break;
			case MyGuiControlBlockPropertyLayoutEnum.Vertical:
				if (IsOnOffSwitch())
				{
					m_title.Position = new Vector2(0f, (base.Size * -0.5f).Y);
				}
				else
				{
					m_title.Position = base.Size * -0.5f;
				}
				m_extraInfo.Position = base.Size * new Vector2(0.5f, -0.5f);
				m_propertyControl.Position = base.Size * -0.5f + new Vector2(0f, m_titleHeight * 1.05f);
				break;
			}
		}

		public override void OnFocusChanged(MyGuiControlBase control, bool focus)
		{
			base.Owner?.OnFocusChanged(control, focus);
		}

		public override void CheckIsWithinScissor(RectangleF scissor, bool complete = false)
		{
			Vector2 topLeft = Vector2.Zero;
			Vector2 botRight = Vector2.Zero;
			GetScissorBounds(ref topLeft, ref botRight);
			Vector2 vector = new Vector2(Math.Max(topLeft.X, scissor.X), Math.Max(topLeft.Y, scissor.Y));
			Vector2 size = new Vector2(Math.Min(botRight.X, scissor.Right), Math.Min(botRight.Y, scissor.Bottom)) - vector;
			if (size.X <= 0f || size.Y <= 0f)
			{
				base.IsWithinScissor = false;
				foreach (MyGuiControlBase element in Elements)
				{
					element.IsWithinScissor = false;
				}
				return;
			}
<<<<<<< HEAD
			RectangleF rectangleF = default(RectangleF);
			rectangleF.Position = vector;
			rectangleF.Size = size;
			RectangleF scissor2 = rectangleF;
=======
			RectangleF scissor2 = default(RectangleF);
			scissor2.Position = vector;
			scissor2.Size = size;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			base.IsWithinScissor = true;
			foreach (MyGuiControlBase element2 in Elements)
			{
				element2.CheckIsWithinScissor(scissor2, complete);
			}
		}
	}
}
