using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	internal class MyGuiControlRating : MyGuiControlBase
	{
		private Vector2 m_textureSize = new Vector2(32f);

		private float m_space = 8f;

		private int m_value;

		private int m_maxValue;

		public string EmptyTexture = "Textures\\GUI\\Icons\\Rating\\NoStar.png";

		public string FilledTexture = "Textures\\GUI\\Icons\\Rating\\FullStar.png";

		public string HalfFilledTexture = "Textures\\GUI\\Icons\\Rating\\HalfStar.png";

		public int MaxValue
		{
			get
			{
				return m_maxValue;
			}
			set
			{
				m_maxValue = value;
				RecalculateSize();
			}
		}

		public int Value
		{
			get
			{
				return m_value;
			}
			set
			{
				m_value = value;
			}
		}

		public MyGuiControlRating(int value = 0, int maxValue = 10)
		{
			m_value = value;
			m_maxValue = maxValue;
			BackgroundTexture = null;
			base.ColorMask = Vector4.One;
		}

		private void RecalculateSize()
		{
			Vector2 vector = MyGuiManager.GetHudNormalizedSizeFromPixelSize(m_textureSize) * new Vector2(0.75f, 1f);
			Vector2 hudNormalizedSizeFromPixelSize = MyGuiManager.GetHudNormalizedSizeFromPixelSize(new Vector2(m_space * 0.75f, 0f));
			base.Size = new Vector2((vector.X + hudNormalizedSizeFromPixelSize.X) * (float)m_maxValue, vector.Y);
		}

		public float GetWidth()
		{
			float num = MyGuiManager.GetHudNormalizedSizeFromPixelSize(m_textureSize).X * 0.75f;
<<<<<<< HEAD
			float x = MyGuiManager.GetHudNormalizedSizeFromPixelSize(new Vector2(m_space * 0.75f, 0f)).X;
			return (num + x) * (float)MaxValue / 2f;
=======
			float num2 = MyGuiManager.GetHudNormalizedSizeFromPixelSize(new Vector2(m_space * 0.75f, 0f)).X;
			return (num + num2) * (float)MaxValue / 2f;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			if (MaxValue <= 0)
			{
				return;
			}
			Vector2 normalizedSize = MyGuiManager.GetHudNormalizedSizeFromPixelSize(m_textureSize) * new Vector2(0.75f, 1f);
			Vector2 hudNormalizedSizeFromPixelSize = MyGuiManager.GetHudNormalizedSizeFromPixelSize(new Vector2(m_space * 0.75f, 0f));
			Vector2 vector = GetPositionAbsoluteTopLeft() + new Vector2(0f, (base.Size.Y - normalizedSize.Y) / 2f);
			Vector2 vector2 = new Vector2((normalizedSize.X + hudNormalizedSizeFromPixelSize.X) * 0.5f, normalizedSize.Y);
			for (int i = 0; i < MaxValue; i += 2)
			{
				Vector2 normalizedCoord = vector + new Vector2(vector2.X * (float)i, 0f);
				if (i == Value - 1)
				{
					MyGuiManager.DrawSpriteBatch(HalfFilledTexture, normalizedCoord, normalizedSize, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, useFullClientArea: false, waitTillLoaded: false);
				}
				else if (i < Value)
				{
					MyGuiManager.DrawSpriteBatch(FilledTexture, normalizedCoord, normalizedSize, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, useFullClientArea: false, waitTillLoaded: false);
				}
				else
				{
					MyGuiManager.DrawSpriteBatch(EmptyTexture, normalizedCoord, normalizedSize, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, useFullClientArea: false, waitTillLoaded: false);
				}
			}
		}
	}
}
