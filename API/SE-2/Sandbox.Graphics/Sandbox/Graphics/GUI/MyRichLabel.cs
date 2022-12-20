using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Gui.RichTextLabel;
using VRage.Collections;
using VRage.Utils;
using VRageMath;
using VRageRender;
using VRageRender.Messages;

namespace Sandbox.Graphics.GUI
{
	public class MyRichLabel
	{
		private struct DrawParams
		{
			public Vector2 Position;

			public float OffsetY;

			public float OffsetX;

			public Vector2 DrawSizeMax;

			public float Alphamask;

			public DrawParams(Vector2 position, float offsetY, float offsetX, Vector2 drawSizeMax, float alphamask)
			{
				Position = position;
				OffsetY = offsetY;
				OffsetX = offsetX;
				DrawSizeMax = drawSizeMax;
				Alphamask = alphamask;
			}

			public bool Equals(DrawParams other)
			{
				if (Position.Equals(other.Position) && OffsetY.Equals(other.OffsetY) && OffsetX.Equals(other.OffsetX) && DrawSizeMax.Equals(other.DrawSizeMax))
				{
					return Alphamask.Equals(other.Alphamask);
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				object obj2;
				if ((obj2 = obj) is DrawParams)
				{
					DrawParams other = (DrawParams)obj2;
					return Equals(other);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (((((((Position.GetHashCode() * 397) ^ OffsetY.GetHashCode()) * 397) ^ OffsetX.GetHashCode()) * 397) ^ DrawSizeMax.GetHashCode()) * 397) ^ Alphamask.GetHashCode();
			}

			public static bool operator ==(DrawParams left, DrawParams right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(DrawParams left, DrawParams right)
			{
				return !left.Equals(right);
			}
		}

		private static readonly string[] LINE_SEPARATORS = new string[2] { "\n", "\r\n" };

		private const char m_wordSeparator = ' ';

		private MyGuiControlBase m_parent;

		private bool m_sizeDirty;

		private Vector2 m_size;

		private float m_maxLineWidth;

		private float m_minLineHeight;

		private List<MyRichLabelLine> m_lineSeparators;

		private int m_lineSeparatorsCount;

		private int m_lineSeparatorsCapacity;

		private int m_lineSeparatorFirst;

		private MyRichLabelLine m_currentLine;

		private float m_currentLineRestFreeSpace;

		private StringBuilder m_helperSb;

		private int m_visibleLinesCount;

		private List<MyRichLabelText> m_richTextsPool;

		private int m_richTextsOffset;

		private int m_richTextsCapacity = 32;

		private MyRenderMessageDrawCommands m_lastDrawing;

		private bool m_layoutDirty;

		public MyGuiDrawAlignEnum TextAlign;

		private bool m_isScissorEnabled = true;

		private DrawParams m_lastDrawParams;

		private bool m_showTextShadow;

		private int m_charactersDisplayed;

		public bool IsScissorEnabled
		{
			get
			{
				return m_isScissorEnabled;
			}
			set
			{
				if (m_isScissorEnabled != value)
				{
					m_isScissorEnabled = value;
				}
			}
		}

		public int NumberOfRows => m_lineSeparatorsCount + 1;

		public Vector2 Size
		{
			get
			{
				if (!m_sizeDirty)
				{
					return m_size;
				}
				m_size = Vector2.Zero;
				for (int i = 0; i <= m_lineSeparatorsCount; i++)
				{
					int indexSafe = GetIndexSafe(i);
					if (indexSafe >= m_lineSeparators.Count)
					{
						break;
					}
					Vector2 size = m_lineSeparators[indexSafe].Size;
					m_size.Y += size.Y;
					m_size.X = MathHelper.Max(m_size.X, size.X);
				}
				m_sizeDirty = false;
				return m_size;
			}
		}

		public float MaxLineWidth
		{
			get
			{
				return m_maxLineWidth;
			}
			set
			{
				if (m_maxLineWidth != value)
				{
					m_layoutDirty = true;
				}
				m_maxLineWidth = value;
			}
		}

		public bool ShowTextShadow
		{
			get
			{
				return m_showTextShadow;
			}
			set
			{
				if (m_showTextShadow != value)
				{
					m_layoutDirty = true;
				}
				m_showTextShadow = value;
			}
		}

		public int CharactersDisplayed
		{
			get
			{
				return m_charactersDisplayed;
			}
			set
			{
				if (m_charactersDisplayed != value)
				{
					m_layoutDirty = true;
				}
				m_charactersDisplayed = value;
			}
		}

		public event ScissorRectangleHandler AdjustingScissorRectangle;

		public MyRichLabel(MyGuiControlBase parent, float maxLineWidth, float minLineHeight, int? linesCountMax = null)
		{
			m_parent = parent;
			m_maxLineWidth = maxLineWidth;
			m_minLineHeight = minLineHeight;
			m_helperSb = new StringBuilder(64);
			m_visibleLinesCount = ((!linesCountMax.HasValue) ? int.MaxValue : linesCountMax.Value);
			Init();
		}

		private void Init()
		{
			m_helperSb.Clear();
			m_sizeDirty = true;
			m_layoutDirty = true;
			m_size = Vector2.Zero;
			m_lineSeparatorsCount = 0;
			m_lineSeparatorsCapacity = 32;
			m_richTextsOffset = 0;
			m_currentLineRestFreeSpace = m_maxLineWidth;
			m_lineSeparatorFirst = 0;
			m_lineSeparators = new List<MyRichLabelLine>(m_lineSeparatorsCapacity);
			for (int i = 0; i < m_lineSeparatorsCapacity; i++)
			{
				m_lineSeparators.Add(new MyRichLabelLine(m_minLineHeight));
			}
			m_currentLine = m_lineSeparators[0];
			if (m_richTextsPool == null)
			{
				m_richTextsPool = new List<MyRichLabelText>(m_richTextsCapacity);
				for (int j = 0; j < m_richTextsCapacity; j++)
				{
					m_richTextsPool.Add(new MyRichLabelText
					{
						ShowTextShadow = ShowTextShadow,
						Tag = m_parent.Name
					});
				}
			}
			else
			{
				for (int k = 0; k < m_richTextsPool.Count; k++)
				{
					m_richTextsPool[k].Clear();
					m_richTextsPool[k].ShowTextShadow = ShowTextShadow;
					m_richTextsPool[k].Tag = m_parent.Name;
				}
			}
		}

		~MyRichLabel()
		{
			if (m_lastDrawing != null)
			{
				MyRenderProxy.DisposeCommandList(ref m_lastDrawing);
			}
		}

		private void ReallocateLines()
		{
			if (m_lineSeparatorsCount + 1 >= m_lineSeparatorsCapacity)
			{
				m_lineSeparatorsCapacity *= 2;
				m_lineSeparators.Capacity = m_lineSeparatorsCapacity;
				for (int i = 0; i < m_lineSeparatorsCapacity / 2; i++)
				{
					m_lineSeparators.Add(new MyRichLabelLine(m_minLineHeight));
				}
			}
		}

		private void ReallocateRichTexts()
		{
			if (m_richTextsOffset + 1 >= m_richTextsCapacity)
			{
				m_richTextsCapacity *= 2;
				m_richTextsPool.Capacity = m_richTextsCapacity;
				for (int i = 0; i < m_richTextsCapacity / 2; i++)
				{
					m_richTextsPool.Add(new MyRichLabelText
					{
						ShowTextShadow = ShowTextShadow,
						Tag = m_parent.Name
					});
				}
			}
		}

		public void Append(StringBuilder text, string font, float scale, Vector4 color)
		{
			Append(text.ToString(), font, scale, color);
		}

		public void Append(string text, string font, float scale, Vector4 color)
		{
			string[] array = text.Split(LINE_SEPARATORS, StringSplitOptions.None);
			for (int i = 0; i < array.Length; i++)
			{
				AppendParagraph(array[i], font, scale, color);
				if (i < array.Length - 1)
				{
					AppendLine();
				}
			}
		}

		public void Append(string texture, Vector2 size, Vector4 color)
		{
			MyRichLabelImage myRichLabelImage = new MyRichLabelImage(texture, size, color);
			if (myRichLabelImage.Size.X > m_currentLineRestFreeSpace)
			{
				AppendLine();
			}
			AppendPart(myRichLabelImage);
		}

		public void AppendLink(string url, string text, float scale, Action<string> onClick)
		{
			MyRichLabelLink part = new MyRichLabelLink(url, text, scale, onClick);
			AppendPart(part);
		}

		public void AppendLine()
		{
			if (m_lineSeparatorsCount == m_visibleLinesCount)
			{
				m_lineSeparatorFirst = GetIndexSafe(1);
				m_currentLine = m_lineSeparators[GetIndexSafe(m_lineSeparatorsCount)];
				m_currentLine.ClearParts();
			}
			else
			{
				ReallocateLines();
				m_lineSeparatorsCount++;
				m_currentLine = m_lineSeparators[GetIndexSafe(m_lineSeparatorsCount)];
			}
			m_currentLineRestFreeSpace = m_maxLineWidth;
			ReallocateRichTexts();
			m_sizeDirty = true;
			m_layoutDirty = true;
		}

		public void AppendLine(StringBuilder text, string font, float scale, Vector4 color)
		{
			Append(text, font, scale, color);
			AppendLine();
		}

		public void AppendLine(string texture, Vector2 size, Vector4 color)
		{
			Append(texture, size, color);
			AppendLine();
		}

		private void AppendParagraph(string paragraph, string font, float scale, Vector4 color)
		{
			m_helperSb.Clear();
			m_helperSb.Append(paragraph);
			if (MyGuiManager.MeasureString(font, m_helperSb, scale).X < m_currentLineRestFreeSpace)
			{
				ReallocateRichTexts();
				m_richTextsPool[++m_richTextsOffset].Init(m_helperSb.ToString(), font, scale, color);
				AppendPart(m_richTextsPool[m_richTextsOffset]);
				return;
			}
			ReallocateRichTexts();
			m_richTextsPool[++m_richTextsOffset].Init("", font, scale, color);
			string[] array = paragraph.Split(new char[1] { ' ' });
			for (int i = 0; i < array.Length - 1; i++)
			{
				array[i] += " ";
			}
			if (paragraph.StartsWith(" "))
			{
				array[1] = " " + array[1];
			}
			int num = 0;
			while (num < array.Length)
			{
				m_helperSb.Clear();
				m_helperSb.Append(array[num]);
				if (MyGuiManager.MeasureString(font, m_helperSb, scale).X <= m_currentLineRestFreeSpace - m_richTextsPool[m_richTextsOffset].Size.X)
				{
					m_richTextsPool[m_richTextsOffset].Append(m_helperSb.ToString());
					num++;
					continue;
				}
				if ((m_currentLine == null || m_currentLine.IsEmpty()) && m_richTextsPool[m_richTextsOffset].Text.Length == 0)
				{
					int num2 = MyGuiManager.ComputeNumCharsThatFit(font, m_helperSb, scale, m_currentLineRestFreeSpace);
					m_richTextsPool[m_richTextsOffset].Append(array[num].Substring(0, num2));
					array[num] = array[num].Substring(num2);
				}
				AppendPart(m_richTextsPool[m_richTextsOffset]);
				ReallocateRichTexts();
				m_richTextsPool[++m_richTextsOffset].Init("", font, scale, color);
				if (num < array.Length)
				{
					AppendLine();
				}
			}
			if (m_richTextsPool[m_richTextsOffset].Text.Length > 0)
			{
				AppendPart(m_richTextsPool[m_richTextsOffset]);
			}
		}

		private void AppendPart(MyRichLabelPart part)
		{
			m_currentLine = m_lineSeparators[GetIndexSafe(m_lineSeparatorsCount)];
			m_currentLine.AddPart(part);
			m_currentLineRestFreeSpace = m_maxLineWidth - m_currentLine.Size.X;
			m_sizeDirty = true;
			m_layoutDirty = true;
		}

		public bool Draw(Vector2 position, float offsetY, float offsetX, Vector2 drawSizeMax, float alphamask)
		{
			DrawParams drawParams = new DrawParams(position, offsetY, offsetX, drawSizeMax, alphamask);
			if (drawParams == m_lastDrawParams && !m_layoutDirty)
			{
				MyRenderProxy.ExecuteCommands(m_lastDrawing, disposeAfterDraw: false);
				return true;
			}
			m_lastDrawParams = drawParams;
			m_layoutDirty = false;
			if (m_lastDrawing != null)
			{
				MyRenderProxy.DisposeCommandList(ref m_lastDrawing);
			}
			MyRenderProxy.BeginRecordingDeferredMessages();
			RectangleF rectangle = new RectangleF(position, drawSizeMax);
			OnAdjustingScissorRectangle(ref rectangle);
			Vector2 textSize = Size;
			int charactersLeft = ((CharactersDisplayed == -1) ? int.MaxValue : CharactersDisplayed);
			if (IsScissorEnabled)
			{
				using (MyGuiManager.UsingScissorRectangle(ref rectangle))
				{
					ScissorableDraw();
				}
			}
			else
			{
				ScissorableDraw();
			}
			if (charactersLeft > 0)
			{
				CharactersDisplayed = -1;
			}
			m_lastDrawing = MyRenderProxy.FinishRecordingDeferredMessages();
			MyRenderProxy.ExecuteCommands(m_lastDrawing, disposeAfterDraw: false);
			return true;
			void ScissorableDraw()
			{
				Vector2 zero = Vector2.Zero;
				float num = 0f;
				switch (TextAlign)
				{
				case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP:
				case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
				case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
					zero.X = 0.5f * drawSizeMax.X - 0.5f * textSize.X;
					num = 0.5f;
					break;
				case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP:
				case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
				case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
					zero.X = drawSizeMax.X - textSize.X;
					num = 1f;
					break;
				}
				switch (TextAlign)
				{
				case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER:
				case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER:
				case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER:
					zero.Y = 0.5f * drawSizeMax.Y - 0.5f * textSize.Y;
					break;
				case MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_BOTTOM:
				case MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM:
				case MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM:
					zero.Y = drawSizeMax.Y - textSize.Y;
					break;
				}
				zero.Y -= offsetY;
				zero.X -= offsetX;
				for (int i = 0; i <= m_lineSeparatorsCount; i++)
				{
					int indexSafe = GetIndexSafe(i);
					if (indexSafe >= m_lineSeparators.Count)
					{
						break;
					}
					MyRichLabelLine myRichLabelLine = m_lineSeparators[indexSafe];
					Vector2 size = myRichLabelLine.Size;
					Vector2 position2 = position + zero;
					position2.X += num * (textSize.X - size.X);
					if (charactersLeft > 0)
					{
						myRichLabelLine.Draw(position2, alphamask, ref charactersLeft);
					}
					zero.Y += size.Y;
				}
			}
		}

		private int GetIndexSafe(int index)
		{
			return (index + m_lineSeparatorFirst) % (m_visibleLinesCount + 1);
		}

		public void Clear()
		{
			m_lineSeparators.Clear();
			Init();
		}

		private void OnAdjustingScissorRectangle(ref RectangleF rectangle)
		{
			this.AdjustingScissorRectangle?.Invoke(ref rectangle);
		}

		internal bool HandleInput(Vector2 position, float offsetV, float offsetH)
		{
			position.X -= offsetH;
			position.Y -= offsetV;
			for (int i = 0; i <= m_lineSeparatorsCount; i++)
			{
				int indexSafe = GetIndexSafe(i);
				if (indexSafe < 0 || indexSafe >= m_lineSeparators.Count)
				{
					return false;
				}
				MyRichLabelLine myRichLabelLine = m_lineSeparators[indexSafe];
				if (myRichLabelLine.HandleInput(position))
				{
					return true;
				}
				position.Y += myRichLabelLine.Size.Y;
			}
			return false;
		}

		internal ListReader<MyRichLabelLine> GetLines()
		{
			return m_lineSeparators;
		}

		public void SetColor(Color textColor)
		{
			Vector4 color = textColor;
			foreach (MyRichLabelLine line in GetLines())
			{
				foreach (MyRichLabelPart part in line.GetParts())
				{
					part.Color = color;
				}
			}
		}
	}
}
