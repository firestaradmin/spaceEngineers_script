using System.Text;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Media;
using VRageMath;
using VRageRender;

namespace VRage.UserInterface.Media
{
	public class MyFont : FontBase
	{
		private VRageRender.MyFont m_font;

		private int m_fontIndex;

		public static float GlobalFontScale { get; set; } = 1f;


		public override char? DefaultCharacter => ' ';

		public override int LineSpacing => m_font.LineHeight;

		public override FontEffectType EffectType => FontEffectType.None;

		public override float Spacing
		{
			get
			{
				return m_font.Spacing;
			}
			set
			{
				m_font.Spacing = (int)value;
			}
		}

		public float Scale { get; set; }

		public override object GetNativeFont()
		{
			return m_fontIndex;
		}

		public MyFont(object nativeFont, int index, float scale)
			: base(nativeFont)
		{
			m_font = nativeFont as VRageRender.MyFont;
			m_fontIndex = index;
			Scale = scale;
		}

		public override Size MeasureString(StringBuilder text, float dpiScaleX, float dpiScaleY)
		{
			Vector2 vector = m_font.MeasureString(text, GlobalFontScale * Scale * (1f / dpiScaleX));
			return new Size(vector.X, vector.Y);
		}

		public override Size MeasureString(string text, float dpiScaleX, float dpiScaleY)
		{
			Vector2 vector = m_font.MeasureString(text, GlobalFontScale * Scale * (1f / dpiScaleX));
			return new Size(vector.X, vector.Y);
		}
	}
}
