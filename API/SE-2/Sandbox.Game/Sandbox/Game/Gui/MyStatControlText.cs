using System.Text;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Game.GUI
{
	public class MyStatControlText : MyStatControlBase
	{
		private const string STAT_TAG = "{STAT}";
<<<<<<< HEAD
=======

		private readonly string m_text;

		private readonly StringBuilder m_textTmp = new StringBuilder(128);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private readonly string m_text;

		private readonly StringBuilder m_textTmp = new StringBuilder(128);

		private MyFont m_font;

		private MyStringHash m_fontHash;

		private readonly bool m_hasStat;

		private Vector2 m_lastDrawOffset;

		private Vector2 m_lastSize;

		private string m_lastStatString;

		private string m_currentText;

		private bool m_dirty = true;

		public float Scale { get; set; }

		public Vector4 TextColorMask { get; set; }

		public MyGuiDrawAlignEnum TextAlign { get; set; }

		public string Font
		{
			get
			{
				return m_fontHash.String;
			}
			set
			{
				m_fontHash = MyStringHash.GetOrCompute(value);
				m_font = MyGuiManager.GetFont(m_fontHash);
				m_dirty = true;
			}
		}

		public static string SubstituteTexts(string text, string context = null)
		{
			return MyTexts.SubstituteTexts(text, context);
		}

		public MyStatControlText(MyStatControls parent, string text)
			: base(parent)
		{
			TextAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			Font = "Blue";
			Scale = 1f;
			TextColorMask = Vector4.One;
			m_text = MyTexts.SubstituteTexts(text);
			m_hasStat = m_text.Contains("{STAT}");
		}

		public override void Draw(float transitionAlpha)
		{
			Vector4 sourceColorMask = TextColorMask;
			if (base.BlinkBehavior.Blink && base.BlinkBehavior.ColorMask.HasValue)
			{
				sourceColorMask = base.BlinkBehavior.ColorMask.Value;
			}
			sourceColorMask = MyGuiControlBase.ApplyColorMaskModifiers(sourceColorMask, enabled: true, transitionAlpha);
			if (m_hasStat)
			{
				if (m_lastStatString != base.StatString)
				{
					m_lastStatString = base.StatString;
					m_textTmp.Clear();
					m_textTmp.Append(m_text);
					m_textTmp.Replace("{STAT}", m_lastStatString);
					m_dirty = true;
					m_currentText = m_textTmp.ToString();
				}
			}
			else
			{
				m_currentText = m_text;
			}
			if (m_dirty)
			{
				m_lastSize = m_font.MeasureString(m_currentText, Scale);
				m_lastDrawOffset = MyUtils.GetCoordTopLeftFromAligned(Vector2.Zero, m_lastSize, TextAlign) + base.Size / 2f;
				m_dirty = false;
			}
			Vector2 screenCoord = base.Position + m_lastDrawOffset;
			MyRenderProxy.DrawString((int)m_fontHash, screenCoord, sourceColorMask, m_currentText, Scale, m_lastSize.X + 100f, ignoreBounds: false);
		}
	}
}
