using System;
using System.Collections.Generic;
using System.Text;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	internal class MyRichLabelLine
	{
		private readonly float m_minLineHeight;

		private List<MyRichLabelPart> m_parts;

		private Vector2 m_size;

		public Vector2 Size => m_size;

		public string DebugText
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < m_parts.Count; i++)
				{
					m_parts[i].AppendTextTo(stringBuilder);
				}
				return stringBuilder.ToString();
			}
		}

		public MyRichLabelLine(float minLineHeight)
		{
			m_minLineHeight = minLineHeight;
			m_parts = new List<MyRichLabelPart>(8);
			RecalculateSize();
		}

		public void AddPart(MyRichLabelPart part)
		{
			m_parts.Add(part);
			RecalculateSize();
		}

		public void ClearParts()
		{
			m_parts.Clear();
			RecalculateSize();
		}

		public List<MyRichLabelPart> GetParts()
		{
			return m_parts;
		}

		private void RecalculateSize()
		{
			Vector2 size = new Vector2(0f, m_minLineHeight);
			for (int i = 0; i < m_parts.Count; i++)
			{
				Vector2 size2 = m_parts[i].Size;
				size.Y = Math.Max(size2.Y, size.Y);
				size.X += size2.X;
			}
			m_size = size;
		}

		public bool Draw(Vector2 position, float alphamask, ref int charactersLeft)
		{
			Vector2 position2 = position;
			float num = position.Y + m_size.Y / 2f;
			float num2 = 0f;
			for (int i = 0; i < m_parts.Count; i++)
			{
				MyRichLabelPart myRichLabelPart = m_parts[i];
				if (num2 < myRichLabelPart.Size.Y)
				{
					num2 = myRichLabelPart.Size.Y;
				}
			}
			for (int j = 0; j < m_parts.Count; j++)
			{
				MyRichLabelPart myRichLabelPart2 = m_parts[j];
				Vector2 size = myRichLabelPart2.Size;
				position2.Y = num - num2 / 2f;
<<<<<<< HEAD
				if (j > 0 && size.Y < m_parts[0].Size.Y)
				{
					position2.Y += m_parts[0].Size.Y - size.Y;
				}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!(position2.Y + m_size.Y < 0f) && !(position2.Y > 1f))
				{
					Vector2 vector = default(Vector2);
					if (j > 0)
					{
						vector.Y = m_parts[j - 1].Size.Y - m_parts[j].Size.Y;
					}
					if (!myRichLabelPart2.Draw(position2, alphamask, ref charactersLeft))
					{
						return false;
					}
					position2.X += size.X;
					if (charactersLeft <= 0)
					{
						return true;
					}
				}
			}
			return true;
		}

		public bool IsEmpty()
		{
			return m_parts.Count == 0;
		}

		public bool HandleInput(Vector2 position)
		{
			for (int i = 0; i < m_parts.Count; i++)
			{
				MyRichLabelPart myRichLabelPart = m_parts[i];
				if (myRichLabelPart.HandleInput(position))
				{
					return true;
				}
				position.X += myRichLabelPart.Size.X;
			}
			return false;
		}
	}
}
