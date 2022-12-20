using System.Text;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public abstract class MyGuiControlAutoScaleText : MyGuiControlBase
	{
		protected bool IsTextWithEllipseAlready { get; set; }

		public bool IsAutoScaleEnabled { get; set; }

		public bool IsAutoEllipsisEnabled { get; set; }

<<<<<<< HEAD
		public MyGuiControlAutoScaleText(Vector2? position = null, Vector2? size = null, Vector4? colorMask = null, bool isActiveControl = true, bool canHaveFocus = false)
			: base(position, size, colorMask, null, null, isActiveControl, canHaveFocus)
=======
		public MyGuiControlAutoScaleText(Vector2? position = null, Vector2? size = null, Vector4? colorMask = null, bool isActiveControl = true)
			: base(position, size, colorMask, null, null, isActiveControl)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
		}

		public MyGuiControlAutoScaleText(Vector2? position = null, Vector2? size = null, Vector4? colorMask = null, MyGuiDrawAlignEnum originAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, string toolTip = null, MyGuiControlHighlightType highlightType = MyGuiControlHighlightType.WHEN_CURSOR_OVER, bool canHaveFocus = false)
			: base(position ?? Vector2.Zero, size, colorMask ?? MyGuiConstants.BUTTON_BACKGROUND_COLOR, toolTip, null, isActiveControl: true, canHaveFocus: true, highlightType, originAlign)
		{
		}

		public string GetTextWithEllipsis(StringBuilder textToDraw, string font, float textScale, Vector2 maxSize)
		{
			string result = textToDraw.ToString();
			if (!IsTextWithEllipseAlready && textToDraw != null && maxSize.X != float.PositiveInfinity)
			{
				IsTextWithEllipseAlready = true;
				float scale = GetScale(font, textToDraw, maxSize, textScale, 0f);
				if (scale >= textScale)
				{
					return null;
				}
				while (scale < textScale)
				{
					if (textToDraw.Length > 3)
					{
						textToDraw.TrimEnd(2);
						textToDraw.Append("…");
						scale = GetScale(font, textToDraw, maxSize, textScale, 0f);
						continue;
					}
					return result;
				}
				return result;
			}
			return null;
		}

		public static float GetScale(string font, StringBuilder textForDraw, Vector2 maxSize, float textScale, float minTextScale)
		{
			float num = MyGuiManager.MeasureScaleNeeded(font, textForDraw, maxSize.X);
			if (num > textScale)
			{
				num = textScale;
			}
			if (num < textScale)
			{
				textScale = ((!(num < minTextScale)) ? num : minTextScale);
				return textScale;
			}
			return num;
		}
	}
}
