using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using VRage.Collections;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyToolTips
	{
		private MyGuiControlBase m_tooltipControl;

		public readonly ObservableCollection<MyColoredText> ToolTips;

		public bool RecalculateOnChange = true;

		public Vector2 Size;

		public string Background;

		public Color? ColorMask;

		public bool Highlight { get; set; }

		public Vector4 HighlightColor { get; set; }

		public MyGuiControlBase TooltipControl
		{
			get
			{
				return m_tooltipControl;
			}
			set
			{
				m_tooltipControl = value;
				if (m_tooltipControl != null)
				{
					Size = m_tooltipControl.Size;
				}
				else
				{
					RecalculateSize();
				}
			}
		}

		public bool HasContent
		{
			get
			{
				if (m_tooltipControl == null)
				{
					return ((Collection<MyColoredText>)(object)ToolTips).Count > 0;
				}
				return true;
			}
		}

		/// <summary>
		/// Creates new instance with empty tooltips
		/// </summary>
		public MyToolTips()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Expected O, but got Unknown
			Background = null;
			ColorMask = null;
			ToolTips = new ObservableCollection<MyColoredText>();
			((ObservableCollection<MyColoredText>)ToolTips).add_CollectionChanged(new NotifyCollectionChangedEventHandler(ToolTips_CollectionChanged));
			Size = new Vector2(-1f);
			HighlightColor = Color.Orange.ToVector4();
		}

		private void ToolTips_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (RecalculateOnChange)
			{
				RecalculateSize();
			}
		}

		/// <summary>
		/// Creates new instance with one default tooltip
		/// </summary>
		/// <param name="toolTip">Tooltip's text</param>
		public MyToolTips(string toolTip)
			: this()
		{
			AddToolTip(toolTip);
		}

		public void AddToolTip(string toolTip, float textScale = 0.7f, string font = "Blue")
		{
			if (toolTip != null)
			{
				((Collection<MyColoredText>)(object)ToolTips).Add(new MyColoredText(toolTip, Color.White, null, font, textScale));
			}
		}

		/// <summary>
		/// Recalculates size of tooltips
		/// </summary>
		public void RecalculateSize()
		{
			float x = 0f;
			float num = 0f;
			bool flag = true;
			for (int i = 0; i < ((Collection<MyColoredText>)(object)ToolTips).Count; i++)
			{
				if (((Collection<MyColoredText>)(object)ToolTips)[i].Text.Length > 0)
				{
					flag = false;
				}
				Vector2 vector = MyGuiManager.MeasureString("Blue", ((Collection<MyColoredText>)(object)ToolTips)[i].Text, ((Collection<MyColoredText>)(object)ToolTips)[i].ScaleWithLanguage);
				x = Math.Max(Size.X, vector.X);
				num += vector.Y;
			}
			if (flag)
			{
				Size.X = -1f;
				Size.Y = -1f;
			}
			else
			{
				Size.X = x;
				Size.Y = num;
			}
		}

		public void Draw(Vector2 mousePosition)
		{
			Vector2 vector = mousePosition + MyGuiConstants.TOOL_TIP_RELATIVE_DEFAULT_POSITION;
			if (!(Size.X > -1f))
			{
				return;
			}
			Vector2 vector2 = new Vector2(0.005f, 0.002f);
			Vector2 vector3 = Size + 2f * vector2;
			Vector2 vector4 = vector - new Vector2(vector2.X, 0f);
			Rectangle rectangle = (MyGuiManager.FullscreenHudEnabled ? MyGuiManager.GetFullscreenRectangle() : MyGuiManager.GetSafeFullscreenRectangle());
			Vector2 vector5 = MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate(new Vector2(rectangle.Left, rectangle.Top)) + new Vector2(MyGuiConstants.TOOLTIP_DISTANCE_FROM_BORDER);
			Vector2 vector6 = MyGuiManager.GetNormalizedCoordinateFromScreenCoordinate(new Vector2(rectangle.Right, rectangle.Bottom)) - new Vector2(MyGuiConstants.TOOLTIP_DISTANCE_FROM_BORDER);
			if (vector4.X + vector3.X > vector6.X)
			{
				vector4.X = vector6.X - vector3.X;
			}
			if (vector4.Y + vector3.Y > vector6.Y)
			{
				vector4.Y = vector6.Y - vector3.Y;
			}
			if (vector4.X < vector5.X)
			{
				vector4.X = vector5.X;
			}
			if (vector4.Y < vector5.Y)
			{
				vector4.Y = vector5.Y;
			}
			if (Highlight)
			{
				Vector2 vector7 = new Vector2(0.003f, 0.004f);
				Vector2 positionLeftTop = vector4 - vector7;
				Vector2 size = vector3 + 2f * vector7;
				MyGuiConstants.TEXTURE_RECTANGLE_NEUTRAL.Draw(positionLeftTop, size, HighlightColor);
			}
			if (TooltipControl != null)
			{
				TooltipControl.Position = vector4;
				TooltipControl.Update();
				TooltipControl.Draw(1f, 1f);
				return;
			}
			Color color = ColorMask ?? MyGuiConstants.THEMED_GUI_BACKGROUND_COLOR;
			color.A = 230;
			MyGuiManager.DrawSpriteBatch("Textures\\GUI\\TooltipBackground.dds", vector4, vector3, color, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP);
			Vector2 normalizedPosition = vector4 + new Vector2(vector2.X, vector3.Y / 2f - Size.Y / 2f);
			foreach (MyColoredText toolTip in ToolTips)
			{
				toolTip.Draw(normalizedPosition, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, 1f, isHighlight: false);
				normalizedPosition.Y += toolTip.Size.Y;
			}
		}
	}
}
