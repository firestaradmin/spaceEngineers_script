using System;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlColor : MyGuiControlParent
	{
		private const float SLIDER_WIDTH = 0.09f;

		private Color m_color;

		private MyGuiControlLabel m_textLabel;

		private MyGuiControlSlider m_rSlider;

		private MyGuiControlSlider m_gSlider;

		private MyGuiControlSlider m_bSlider;

		private MyGuiControlLabel m_rLabel;

		private MyGuiControlLabel m_gLabel;

		private MyGuiControlLabel m_bLabel;

		private MyStringId m_caption;

		private bool m_canChangeColor = true;

		private bool m_placeSlidersVertically;

		public Color Color => m_color;

		public event Action<MyGuiControlColor> OnChange;

		public MyGuiControlColor(string text, float textScale, Vector2 position, Color color, Color defaultColor, MyStringId dialogAmountCaption, bool placeSlidersVertically = false, string font = "Blue", bool isAutoscaleEnabled = false, bool isAutoEllipsisEnabled = false, float maxTitleWidth = 1f)
			: base(position)
		{
			m_color = color;
			m_placeSlidersVertically = placeSlidersVertically;
			m_textLabel = MakeLabel(textScale, font, isAutoscaleEnabled, maxTitleWidth, isAutoEllipsisEnabled, text.ToString());
			m_caption = dialogAmountCaption;
			m_rSlider = MakeSlider(font, defaultColor.R);
			m_gSlider = MakeSlider(font, defaultColor.G);
			m_bSlider = MakeSlider(font, defaultColor.B);
			MyGuiControlSlider rSlider = m_rSlider;
			rSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(rSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider sender)
			{
				if (m_canChangeColor)
				{
					m_color.R = (byte)sender.Value;
					UpdateTexts();
					this.OnChange.InvokeIfNotNull(this);
				}
			});
			MyGuiControlSlider gSlider = m_gSlider;
			gSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(gSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider sender)
			{
				if (m_canChangeColor)
				{
					m_color.G = (byte)sender.Value;
					UpdateTexts();
					this.OnChange.InvokeIfNotNull(this);
				}
			});
			MyGuiControlSlider bSlider = m_bSlider;
			bSlider.ValueChanged = (Action<MyGuiControlSlider>)Delegate.Combine(bSlider.ValueChanged, (Action<MyGuiControlSlider>)delegate(MyGuiControlSlider sender)
			{
				if (m_canChangeColor)
				{
					m_color.B = (byte)sender.Value;
					UpdateTexts();
					this.OnChange.InvokeIfNotNull(this);
				}
			});
			m_rLabel = MakeLabel(textScale, font);
			m_gLabel = MakeLabel(textScale, font);
			m_bLabel = MakeLabel(textScale, font);
			m_rSlider.Value = (int)m_color.R;
			m_gSlider.Value = (int)m_color.G;
			m_bSlider.Value = (int)m_color.B;
			Elements.Add(m_textLabel);
			Elements.Add(m_rSlider);
			Elements.Add(m_gSlider);
			Elements.Add(m_bSlider);
			Elements.Add(m_rLabel);
			Elements.Add(m_gLabel);
			Elements.Add(m_bLabel);
			UpdateTexts();
			RefreshInternals();
			base.Size = base.MinSize;
		}

		private MyGuiControlSlider MakeSlider(string font, byte defaultVal)
		{
			Vector2? position = Vector2.Zero;
			float width = 121f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;
			Vector4? color = base.ColorMask;
			return new MyGuiControlSlider(position, 0f, 255f, width, (int)defaultVal, color, null, 1, 0.8f, 0f, font, null, MyGuiControlSliderStyleEnum.Default, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP)
			{
				SliderClicked = OnSliderClicked
			};
		}

		private bool OnSliderClicked(MyGuiControlSlider who)
		{
			if (MyInput.Static.IsAnyCtrlKeyPressed())
			{
				MyGuiScreenDialogAmount obj = new MyGuiScreenDialogAmount(0f, 255f, defaultAmount: who.Value, caption: m_caption, minMaxDecimalDigits: 3, parseAsInteger: true);
				obj.OnConfirmed += delegate(float v)
				{
					who.Value = v;
				};
				MyScreenManager.AddScreen(obj);
				return true;
			}
			return false;
		}

		private MyGuiControlLabel MakeLabel(float scale, string font, bool isAutoscaleEnabled = false, float maxTitleWidth = 1f, bool isAutoEllipsisEnabled = false, string Text = "")
		{
			Vector4? colorMask = base.ColorMask;
			float textScale = 0.8f * scale;
			bool isAutoScaleEnabled = isAutoscaleEnabled;
			return new MyGuiControlLabel(null, null, Text, colorMask, textScale, font, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, isAutoEllipsisEnabled, maxTitleWidth, isAutoScaleEnabled);
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			Vector2 positionAbsoluteTopRight = GetPositionAbsoluteTopRight();
			positionAbsoluteTopRight.Y += m_textLabel.Size.Y + MyGuiConstants.MULTILINE_LABEL_BORDER.Y * 2f;
			Vector2 vector = new Vector2(m_bSlider.Size.X, m_textLabel.Size.Y);
			MyGuiManager.DrawSpriteBatch("Textures\\GUI\\Blank.dds", positionAbsoluteTopRight, vector, MyGuiControlBase.ApplyColorMaskModifiers(m_color.ToVector4(), enabled: true, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			positionAbsoluteTopRight.X -= vector.X;
			Color white = Color.White;
			white.A = (byte)((float)(int)white.A * transitionAlpha);
			MyGuiManager.DrawBorders(positionAbsoluteTopRight, vector, white, base.BorderSize);
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = base.HandleInput();
			if (myGuiControlBase == null)
			{
				myGuiControlBase = HandleInputElements();
			}
			return myGuiControlBase;
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			RefreshInternals();
		}

		private void RefreshInternals()
		{
			Vector2 vector = -0.5f * base.Size;
			Vector2 zero = Vector2.Zero;
			if (m_placeSlidersVertically)
			{
				zero.X = Math.Max(m_textLabel.Size.X, m_gSlider.MinSize.X + 0.06f);
				zero.Y = m_textLabel.Size.Y * 2f + 3f * Math.Max(m_gSlider.Size.Y, m_gLabel.Size.Y);
			}
			else
			{
				zero.X = MathHelper.Max(m_textLabel.Size.X, 3f * (m_gSlider.MinSize.X + 0.06f));
				zero.Y = m_textLabel.Size.Y * 2f + m_rSlider.Size.Y + m_rLabel.Size.Y;
			}
			if (base.Size.X < zero.X || base.Size.Y < zero.Y)
			{
				base.Size = Vector2.Max(base.Size, zero);
				return;
			}
			vector.Y += MyGuiConstants.MULTILINE_LABEL_BORDER.Y;
			m_textLabel.Position = vector;
			vector.Y += m_textLabel.Size.Y * 2f + MyGuiConstants.MULTILINE_LABEL_BORDER.Y;
			if (m_placeSlidersVertically)
			{
<<<<<<< HEAD
				Vector2 vector2 = new Vector2(base.Size.X - 0.06f, m_rSlider.MinSize.Y);
				float num = Math.Max(m_rLabel.Size.Y, m_rSlider.Size.Y);
				MyGuiControlSlider rSlider = m_rSlider;
				MyGuiControlSlider gSlider = m_gSlider;
				Vector2 vector4 = (m_bSlider.Size = vector2);
				Vector2 vector7 = (rSlider.Size = (gSlider.Size = vector4));
				m_rLabel.Position = vector + new Vector2(0f, 0.5f) * num;
				m_rSlider.Position = new Vector2(vector.X + base.Size.X, vector.Y + 0.5f * num);
				m_rLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_rSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
=======
				Vector2 vector2 = new Vector2(base.Size.X - 0.06f, m_RSlider.MinSize.Y);
				float num = Math.Max(m_RLabel.Size.Y, m_RSlider.Size.Y);
				MyGuiControlSlider rSlider = m_RSlider;
				MyGuiControlSlider gSlider = m_GSlider;
				Vector2 vector4 = (m_BSlider.Size = vector2);
				Vector2 vector7 = (rSlider.Size = (gSlider.Size = vector4));
				m_RLabel.Position = vector + new Vector2(0f, 0.5f) * num;
				m_RSlider.Position = new Vector2(vector.X + base.Size.X, vector.Y + 0.5f * num);
				m_RLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_RSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				vector.Y += num;
				m_gLabel.Position = vector + new Vector2(0f, 0.5f) * num;
				m_gSlider.Position = new Vector2(vector.X + base.Size.X, vector.Y + 0.5f * num);
				m_gLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_gSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
				vector.Y += num;
				m_bLabel.Position = vector + new Vector2(0f, 0.5f) * num;
				m_bSlider.Position = new Vector2(vector.X + base.Size.X, vector.Y + 0.5f * num);
				m_bLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER;
				m_bSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER;
				vector.Y += num;
			}
			else
			{
<<<<<<< HEAD
				float num2 = MathHelper.Max(m_rLabel.Size.X, m_rSlider.MinSize.X, base.Size.X / 3f);
				Vector2 vector8 = new Vector2(num2, m_rSlider.Size.Y);
				MyGuiControlSlider rSlider2 = m_rSlider;
				MyGuiControlSlider gSlider2 = m_gSlider;
				Vector2 vector4 = (m_bSlider.Size = vector8);
=======
				float num2 = MathHelper.Max(m_RLabel.Size.X, m_RSlider.MinSize.X, base.Size.X / 3f);
				Vector2 vector8 = new Vector2(num2, m_RSlider.Size.Y);
				MyGuiControlSlider rSlider2 = m_RSlider;
				MyGuiControlSlider gSlider2 = m_GSlider;
				Vector2 vector4 = (m_BSlider.Size = vector8);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				Vector2 vector7 = (rSlider2.Size = (gSlider2.Size = vector4));
				Vector2 position = vector;
				m_rSlider.Position = position;
				position.X += num2;
				m_gSlider.Position = position;
				position.X += num2;
				m_bSlider.Position = position;
				vector.Y += m_rSlider.Size.Y;
				m_rLabel.Position = vector;
				vector.X += num2;
				m_gLabel.Position = vector;
				vector.X += num2;
				m_bLabel.Position = vector;
				vector.X += num2;
<<<<<<< HEAD
				MyGuiControlLabel rLabel = m_rLabel;
				MyGuiControlLabel gLabel = m_gLabel;
				MyGuiDrawAlignEnum myGuiDrawAlignEnum2 = (m_bLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				MyGuiDrawAlignEnum myGuiDrawAlignEnum5 = (rLabel.OriginAlign = (gLabel.OriginAlign = myGuiDrawAlignEnum2));
				MyGuiControlSlider rSlider3 = m_rSlider;
				MyGuiControlSlider gSlider3 = m_gSlider;
				myGuiDrawAlignEnum2 = (m_bSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
=======
				MyGuiControlLabel rLabel = m_RLabel;
				MyGuiControlLabel gLabel = m_GLabel;
				MyGuiDrawAlignEnum myGuiDrawAlignEnum2 = (m_BLabel.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
				MyGuiDrawAlignEnum myGuiDrawAlignEnum5 = (rLabel.OriginAlign = (gLabel.OriginAlign = myGuiDrawAlignEnum2));
				MyGuiControlSlider rSlider3 = m_RSlider;
				MyGuiControlSlider gSlider3 = m_GSlider;
				myGuiDrawAlignEnum2 = (m_BSlider.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myGuiDrawAlignEnum5 = (rSlider3.OriginAlign = (gSlider3.OriginAlign = myGuiDrawAlignEnum2));
			}
		}

		private void UpdateSliders()
		{
			m_canChangeColor = false;
			m_rSlider.Value = (int)m_color.R;
			m_gSlider.Value = (int)m_color.G;
			m_bSlider.Value = (int)m_color.B;
			UpdateTexts();
			m_canChangeColor = true;
		}

		private void UpdateTexts()
		{
			m_rLabel.Text = $"R: {m_color.R}";
			m_gLabel.Text = $"G: {m_color.G}";
			m_bLabel.Text = $"B: {m_color.B}";
		}

		public void SetColor(Vector3 color)
		{
			SetColor(new Color(color));
		}

		public void SetColor(Vector4 color)
		{
			SetColor(new Color(color));
		}

		public void SetColor(Color color)
		{
			bool num = m_color != color;
			m_color = color;
			UpdateSliders();
			if (num)
			{
				this.OnChange.InvokeIfNotNull(this);
			}
		}

		public Color GetColor()
		{
			return m_color;
		}
	}
}
