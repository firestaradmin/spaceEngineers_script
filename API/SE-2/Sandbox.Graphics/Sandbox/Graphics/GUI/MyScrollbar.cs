using System;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public abstract class MyScrollbar
	{
		protected enum StateEnum
		{
			Ready,
			Drag,
			Click
		}

		public const float DEFAULT_STEP = 0.015f;

		private bool m_hasHighlight;

		private float m_value;

		private MyGuiCompositeTexture m_normalTexture;

		private MyGuiCompositeTexture m_highlightTexture;

		protected MyGuiCompositeTexture m_backgroundTexture;

		protected MyGuiControlBase m_ownerControl;

		protected Vector2 m_position;

		protected Vector2 m_caretSize;

		protected Vector2 m_caretPageSize;

		protected float m_max;

		protected float m_page;

		protected StateEnum m_state;

		protected MyGuiCompositeTexture m_texture;

		protected bool m_captured;

		public float ScrollBarScale = 1f;

		public bool Visible;

		public Vector2 Size { get; protected set; }

		public bool HasHighlight
		{
			get
			{
				return m_hasHighlight;
			}
			set
			{
				if (m_hasHighlight != value)
				{
					m_hasHighlight = value;
					RefreshInternals();
				}
			}
		}

		public float MaxSize => m_max;

		public float PageSize => m_page;

		public float Value
		{
			get
			{
				return m_value;
			}
			set
			{
				value = MathHelper.Clamp(value, 0f, m_max - m_page);
				if (m_value != value)
				{
					m_captured = true;
					m_value = value;
					this.ValueChanged.InvokeIfNotNull(this);
				}
			}
		}

		public bool IsOverCaret { get; protected set; }

		public bool IsInDomainCaret { get; protected set; }

		public event Action<MyScrollbar> ValueChanged;

		protected MyScrollbar(MyGuiControlBase control, MyGuiCompositeTexture normalTexture, MyGuiCompositeTexture highlightTexture, MyGuiCompositeTexture backgroundTexture)
		{
			m_ownerControl = control;
			m_normalTexture = normalTexture;
			m_highlightTexture = highlightTexture;
			m_backgroundTexture = backgroundTexture;
			RefreshInternals();
		}

		protected bool CanScroll()
		{
			if (m_max > 0f)
			{
				return m_max > m_page;
			}
			return false;
		}

		public void Init(float max, float page)
		{
			m_max = max;
			m_page = page;
		}

		public void ChangeValue(float amount)
		{
			Value += amount;
		}

		public void PageDown()
		{
			ChangeValue(m_page);
		}

		public void PageUp()
		{
			ChangeValue(0f - m_page);
		}

		public void ScrollDown(float step = 0.015f)
		{
			ChangeValue(step);
		}

		public void ScrollUp(float step = 0.015f)
		{
			ChangeValue(0f - step);
		}

		public void SetPage(float pageNumber)
		{
			Value = pageNumber * m_page;
		}

		public float GetPage()
		{
			return Value / m_page;
		}

		public abstract void Layout(Vector2 position, float length);

		public abstract void Draw(Color colorMask);

		public void DebugDraw()
		{
			MyGuiManager.DrawBorders(m_ownerControl.GetPositionAbsoluteCenter() + m_position, Size, Color.White, 1);
		}

		public abstract bool HandleInput(bool fakeFocus = false);

		protected virtual void RefreshInternals()
		{
			m_texture = (HasHighlight ? m_highlightTexture : m_normalTexture);
			if (HasHighlight)
			{
				m_texture = m_highlightTexture;
			}
			else
			{
				m_texture = m_normalTexture;
			}
		}
	}
}
