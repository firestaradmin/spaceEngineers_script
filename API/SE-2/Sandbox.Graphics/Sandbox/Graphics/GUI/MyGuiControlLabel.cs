using System.Collections.ObjectModel;
using System.Text;
using VRage;
using VRage.Game;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlLabel))]
	public class MyGuiControlLabel : MyGuiControlAutoScaleText
	{
		public class StyleDefinition
		{
			public string Font = "Blue";

			public Vector4 ColorMask = Vector4.One;

			public float TextScale = 0.8f;
		}

		private StyleDefinition m_styleDefinition;

		private bool m_forceNewStringBuilder;

		private float m_maxScaleForAutoScale = 0.8f;

		private string m_text;

		private string m_originalText;

<<<<<<< HEAD
=======
		private bool m_originalTextSetAlready;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private MyStringId m_textEnum;

		private float m_textScale;

		public StringBuilder m_textToDraw;

		private Vector2 m_drawOffset;

		public MyGuiDrawAlignEnum DrawAlign { get; set; }

<<<<<<< HEAD
		/// <summary>
		/// Font used for drawing. Setting null will switch to default font (ie. this never returns null).
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public string Font { get; set; }

		public string Text
		{
			get
			{
				return m_text;
			}
			set
			{
				if (m_text != value)
				{
					m_text = value;
					UpdateFormatParams(null);
					if (!string.IsNullOrEmpty(m_text))
					{
						DoEllipsisAndScaleAdjust(RecalculateSize: true);
					}
				}
				if (value != null)
				{
<<<<<<< HEAD
=======
					m_originalTextSetAlready = true;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_originalText = value;
				}
			}
		}

		public MyStringId TextEnum
		{
			get
			{
				return m_textEnum;
			}
			set
			{
				if (m_textEnum != value || m_text != null)
				{
					m_textEnum = value;
					m_text = null;
					UpdateFormatParams(null);
				}
			}
		}

		public float TextScale
		{
			get
			{
				return m_textScale;
			}
			set
			{
				if (m_textScale != value)
				{
					m_maxScaleForAutoScale = value;
					m_textScale = value;
					TextScaleWithLanguage = value * MyGuiManager.LanguageTextScale;
					RecalculateSize();
				}
			}
		}

		public float TextScaleWithLanguage { get; private set; }

		public StringBuilder TextToDraw
		{
			get
			{
				return m_textToDraw;
			}
			set
			{
				m_textToDraw = value;
				RecalculateSize();
			}
		}

		private StringBuilder TextForDraw => m_textToDraw ?? MyTexts.Get(m_textEnum);

		public bool UseTextShadow { get; set; }

		public MyGuiControlLabel()
			: this(null, null, null, null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, isAutoEllipsisEnabled: false, float.PositiveInfinity, isAutoScaleEnabled: false)
		{
		}

		public MyGuiControlLabel(Vector2? position = null, Vector2? size = null, string text = null, Vector4? colorMask = null, float textScale = 0.8f, string font = "Blue", MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER, bool isAutoEllipsisEnabled = false, float maxWidth = float.PositiveInfinity, bool isAutoScaleEnabled = false)
			: base(position, size, colorMask, isActiveControl: false)
		{
			base.IsAutoEllipsisEnabled = isAutoEllipsisEnabled;
			base.IsAutoScaleEnabled = isAutoScaleEnabled;
			m_originalText = text;
			if (maxWidth != float.PositiveInfinity)
			{
				SetMaxSize(new Vector2(maxWidth, base.MaxSize.Y));
			}
			base.Name = "Label";
			Font = font;
			if (text != null)
			{
				m_text = text;
				m_textToDraw = new StringBuilder(text);
			}
			base.OriginAlign = originAlign;
			TextScale = textScale;
			if (base.IsAutoEllipsisEnabled || base.IsAutoScaleEnabled)
			{
				DoEllipsisAndScaleAdjust();
			}
			RefreshDrawOffset();
		}

		public override void Init(MyObjectBuilder_GuiControlBase objectBuilder)
		{
			base.Init(objectBuilder);
			MyObjectBuilder_GuiControlLabel myObjectBuilder_GuiControlLabel = (MyObjectBuilder_GuiControlLabel)objectBuilder;
			m_textEnum = MyStringId.GetOrCompute(myObjectBuilder_GuiControlLabel.TextEnum);
			TextScale = myObjectBuilder_GuiControlLabel.TextScale;
			m_text = (string.IsNullOrWhiteSpace(myObjectBuilder_GuiControlLabel.Text) ? null : myObjectBuilder_GuiControlLabel.Text);
			Font = myObjectBuilder_GuiControlLabel.Font;
			m_textToDraw = new StringBuilder();
			UpdateFormatParams(null);
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlLabel obj = (MyObjectBuilder_GuiControlLabel)base.GetObjectBuilder();
			obj.TextEnum = m_textEnum.ToString();
			obj.TextScale = TextScale;
			obj.Text = m_text;
			obj.Font = Font;
			return obj;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			if (base.IsAutoEllipsisEnabled)
			{
				_ = base.MaxSize;
			}
			if (TextForDraw == null)
			{
				MyLog.Default.WriteLine("text shouldn't be null! MyGuiContolLabel:" + this);
				return;
			}
			if (UseTextShadow)
			{
				Vector2 textSize = base.Size;
				Vector2 position = GetPositionAbsoluteTopLeft();
				MyGuiTextShadows.DrawShadow(ref position, ref textSize, null, 1f, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			}
			if (base.IsAutoEllipsisEnabled || base.IsAutoScaleEnabled)
			{
				DoEllipsisAndScaleAdjust();
			}
			MyGuiManager.DrawString(Font, TextForDraw.ToString(), GetPositionAbsolute() + m_drawOffset, TextScaleWithLanguage, MyGuiControlBase.ApplyColorMaskModifiers(base.ColorMask, base.Enabled, transitionAlpha), DrawAlign);
<<<<<<< HEAD
=======
		}

		public void DoEllipsisAndScaleAdjust(bool RecalculateSize = false, float maxScale = float.PositiveInfinity, bool resetEllipsis = false)
		{
			if (resetEllipsis)
			{
				TextScale = m_maxScaleForAutoScale;
				Text = m_originalText;
				TextToDraw = new StringBuilder(m_originalText);
				m_textToDraw = new StringBuilder(m_originalText);
				this.RecalculateSize();
				base.IsTextWithEllipseAlready = false;
			}
			if (base.IsAutoScaleEnabled && (RecalculateSize || TextScale != MyGuiManager.MIN_TEXT_SCALE))
			{
				float num = MyGuiControlAutoScaleText.GetScale(Font, TextToDraw, base.MaxSize, TextScale, MyGuiManager.MIN_TEXT_SCALE);
				if (TextScale != num)
				{
					if (num < MyGuiManager.MIN_TEXT_SCALE)
					{
						num = MyGuiManager.MIN_TEXT_SCALE;
					}
					if (num > m_maxScaleForAutoScale)
					{
						num = m_maxScaleForAutoScale;
					}
					if (num > maxScale)
					{
						num = maxScale;
					}
					m_textScale = num;
					TextScaleWithLanguage = num * MyGuiManager.LanguageTextScale;
					this.RecalculateSize();
				}
				if (TextScaleWithLanguage != num * MyGuiManager.LanguageTextScale)
				{
					TextScaleWithLanguage = num * MyGuiManager.LanguageTextScale;
				}
			}
			if (base.IsAutoEllipsisEnabled && !base.IsTextWithEllipseAlready)
			{
				AddTooltip(GetTextWithEllipsis(m_textToDraw, Font, TextScale, base.MaxSize));
				this.RecalculateSize();
			}
		}

		private void AddTooltip(string tooltip)
		{
			if (tooltip != null)
			{
				if (m_toolTip == null)
				{
					m_toolTip = new MyToolTips(tooltip);
				}
				else if (m_toolTip.ToolTips == null)
				{
					m_toolTip = new MyToolTips(tooltip);
				}
				else if (((Collection<MyColoredText>)(object)m_toolTip.ToolTips).Count == 0)
				{
					m_toolTip.AddToolTip(tooltip);
				}
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void DoEllipsisAndScaleAdjust(bool RecalculateSize = false, float maxScale = float.PositiveInfinity, bool resetEllipsis = false)
		{
			if (resetEllipsis)
			{
				TextScale = m_maxScaleForAutoScale;
				Text = m_originalText;
				TextToDraw = new StringBuilder(m_originalText);
				m_textToDraw = new StringBuilder(m_originalText);
				this.RecalculateSize();
				base.IsTextWithEllipseAlready = false;
			}
			if (base.IsAutoScaleEnabled && (RecalculateSize || TextScale != MyGuiManager.MIN_TEXT_SCALE))
			{
				float num = MyGuiControlAutoScaleText.GetScale(Font, TextToDraw, base.MaxSize, TextScale, MyGuiManager.MIN_TEXT_SCALE);
				if (TextScale != num)
				{
					if (num < MyGuiManager.MIN_TEXT_SCALE)
					{
						num = MyGuiManager.MIN_TEXT_SCALE;
					}
					if (num > m_maxScaleForAutoScale)
					{
						num = m_maxScaleForAutoScale;
					}
					if (num > maxScale)
					{
						num = maxScale;
					}
					m_textScale = num;
					TextScaleWithLanguage = num * MyGuiManager.LanguageTextScale;
					this.RecalculateSize();
				}
				if (TextScaleWithLanguage != num * MyGuiManager.LanguageTextScale)
				{
					TextScaleWithLanguage = num * MyGuiManager.LanguageTextScale;
				}
			}
			if (base.IsAutoEllipsisEnabled && !base.IsTextWithEllipseAlready)
			{
				AddTooltip(GetTextWithEllipsis(m_textToDraw, Font, TextScale, base.MaxSize));
				this.RecalculateSize();
			}
		}

		private void AddTooltip(string tooltip)
		{
			if (tooltip != null)
			{
				if (m_toolTip == null)
				{
					m_toolTip = new MyToolTips(tooltip);
				}
				else if (m_toolTip.ToolTips == null)
				{
					m_toolTip = new MyToolTips(tooltip);
				}
				else if (m_toolTip.ToolTips.Count == 0)
				{
					m_toolTip.AddToolTip(tooltip);
				}
			}
		}

		/// <summary>
		/// Inserts newlines into text to make it fit size.
		/// Works only on TextToDraw.
		/// </summary>
		public void Autowrap(float width)
		{
			if (m_textToDraw != null)
			{
				m_textToDraw.Autowrap(width, Font, TextScaleWithLanguage);
			}
		}

		public Vector2 GetTextSize()
		{
			return MyGuiManager.MeasureString(Font, TextForDraw, TextScaleWithLanguage);
		}

		/// <summary>
		/// If label's text contains params, we can update them here. Also, don't forget
		/// that text is defined two time: one as a definition and one that we draw.
		/// </summary>
		/// <param name="args"></param>
		public void UpdateFormatParams(params object[] args)
		{
			if (m_text == null)
			{
				if (m_textToDraw == null || m_forceNewStringBuilder)
				{
					m_textToDraw = new StringBuilder();
				}
				m_textToDraw.Clear();
				if (args != null)
				{
					m_textToDraw.AppendFormat(MyTexts.GetString(m_textEnum), args);
				}
				else
				{
					m_textToDraw.Append(MyTexts.GetString(m_textEnum));
				}
			}
			else
			{
				if (m_textToDraw == null || m_forceNewStringBuilder)
				{
					m_textToDraw = new StringBuilder();
				}
				m_textToDraw.Clear();
				if (args != null)
				{
					m_textToDraw.AppendFormat(m_text.ToString(), args);
				}
				else
				{
					m_textToDraw.Append(m_text);
				}
			}
			m_forceNewStringBuilder = false;
			RecalculateSize();
		}

		/// <summary>
		/// Prepares the text to be changed asynchronously.
		/// Important to use before changing text from parallel threads!
		/// </summary>
		public void PrepareForAsyncTextUpdate()
		{
			m_forceNewStringBuilder = true;
		}

		public void RecalculateSize()
		{
			RefreshInternals();
			base.Size = GetTextSize();
		}

		protected override void OnSizeChanged()
		{
			base.OnSizeChanged();
			RefreshDrawOffset();
		}

		protected override void OnOriginAlignChanged()
		{
			base.OnOriginAlignChanged();
			RefreshDrawOffset();
		}

		private void RefreshDrawOffset()
		{
			if (base.OriginAlign != 0)
			{
				Vector2 size = MyGuiManager.MeasureString(Font, TextForDraw.ToString(), TextScale);
				m_drawOffset = MyUtils.GetCoordTopLeftFromAligned(Vector2.Zero, size, base.OriginAlign);
			}
			else
			{
				m_drawOffset = Vector2.Zero;
			}
		}

		public void RefreshInternals()
		{
			if (m_styleDefinition != null)
			{
				Font = m_styleDefinition.Font;
				base.ColorMask = m_styleDefinition.ColorMask;
				TextScale = m_styleDefinition.TextScale;
			}
		}

		public void ApplyStyle(StyleDefinition style)
		{
			if (style != null)
			{
				m_styleDefinition = style;
				RefreshInternals();
			}
		}
	}
}
