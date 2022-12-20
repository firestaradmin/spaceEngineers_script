using System;
using System.Text;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	internal class MyRichLabelText : MyRichLabelPart
	{
		private StringBuilder m_text;

		private string m_font;

		private float m_scale;

		private bool m_showTextShadow;

		private Vector4 m_textColor;

		public StringBuilder Text => m_text;

		public bool ShowTextShadow
		{
			get
			{
				return m_showTextShadow;
			}
			set
			{
				m_showTextShadow = value;
			}
		}

		public float Scale
		{
			get
			{
				return m_scale;
			}
			set
			{
				m_scale = value;
				RecalculateSize();
			}
		}

		public string Font
		{
			get
			{
				return m_font;
			}
			set
			{
				m_font = value;
				RecalculateSize();
			}
		}

<<<<<<< HEAD
=======
		public new Vector4 Color
		{
			get
			{
				return m_color;
			}
			set
			{
				m_color = value;
			}
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public string Tag { get; set; }

		public MyRichLabelText(StringBuilder text, string font, float scale, Vector4 color)
		{
			m_text = text;
			m_font = font;
			m_scale = scale;
			m_textColor = color;
			RecalculateSize();
		}

		public MyRichLabelText()
		{
			m_text = new StringBuilder();
			m_font = "Blue";
			m_scale = 0f;
			m_textColor = Vector4.Zero;
		}

		public void Init(string text, string font, float scale, Vector4 color)
		{
			m_text.Append(text);
			m_font = font;
			m_scale = scale;
			m_textColor = color;
			RecalculateSize();
		}

		public void Clear()
		{
			m_text.Clear();
			m_font = null;
<<<<<<< HEAD
			m_textColor = Vector4.Zero;
=======
			m_color = Vector4.Zero;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_scale = 0f;
			m_showTextShadow = false;
		}

		public void Append(string text)
		{
			m_text.Append(text);
			RecalculateSize();
		}

		public override void AppendTextTo(StringBuilder builder)
		{
			builder.Append((object)m_text);
		}

		public override bool Draw(Vector2 position, float alphamask, ref int charactersLeft)
		{
			string text = m_text.ToString(0, Math.Min(m_text.Length, charactersLeft));
			charactersLeft -= m_text.Length;
			if (ShowTextShadow && !string.IsNullOrWhiteSpace(text))
			{
				Vector2 textSize = Size;
				MyGuiTextShadows.DrawShadow(ref position, ref textSize, null, alphamask, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			}
<<<<<<< HEAD
			Vector4 textColor = m_textColor;
			textColor *= alphamask;
			MyGuiManager.DrawString(m_font, text, position, m_scale, new Color(textColor));
=======
			Vector4 color = m_color;
			color *= alphamask;
			MyGuiManager.DrawString(m_font, text, position, m_scale, new Color(color));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return true;
		}

		public override bool HandleInput(Vector2 position)
		{
			return false;
		}

		private void RecalculateSize()
		{
			Size = MyGuiManager.MeasureString(m_font, m_text, m_scale);
		}
	}
}
