using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRage.Render11.Sprites;
using VRageMath;
using VRageRender.Messages;

namespace VRageRender
{
	internal class MyRenderFont : MyFont
	{
		private readonly Dictionary<int, ISrvBindable> m_bitmapTextureById = new Dictionary<int, ISrvBindable>();

		private readonly List<IMyStreamedTexture> m_streamTextures = new List<IMyStreamedTexture>();

		private bool m_premultipliedAlpha;

		private readonly Color m_colorMask;

		[ThreadStatic]
		private static List<MyGlyphInfo> m_giList;

		[ThreadStatic]
		private static List<int> m_offsets;

		internal string FontFilePath { get; private set; }

		internal MyRenderFont(string fontFilePath, Color colorMask, bool dummyFont = false)
			: base(fontFilePath, 1, dummyFont)
		{
			m_colorMask = colorMask;
			FontFilePath = fontFilePath;
		}

		internal void LoadContent()
		{
			MyTextureStreamingManager textures = MyManagers.Textures;
			m_premultipliedAlpha = false;
			foreach (KeyValuePair<int, MyBitmapInfo> item in m_bitmapInfoByID)
			{
				string name = Path.Combine(m_fontDirectory, item.Value.strFilename);
				IMyStreamedTexture texture = textures.GetTexture(name, MyFileTextureEnum.GUI);
				m_streamTextures.Add(texture);
				m_bitmapTextureById[item.Key] = texture.Texture;
				texture.Texture.OnFormatChanged += OnFormatChanged;
				m_premultipliedAlpha |= MyResourceUtils.IsRawRGBA(texture.Texture.Format) || item.Value.strFilename.Contains("FontDataPA");
			}
		}

		private void OnFormatChanged(ITexture t)
		{
			t.OnFormatChanged -= OnFormatChanged;
			m_premultipliedAlpha |= MyResourceUtils.IsRawRGBA(t.Format);
		}

		internal void ConsumeContent()
		{
			foreach (IMyStreamedTexture streamTexture in m_streamTextures)
			{
				streamTexture.Pin();
			}
			m_streamTextures.Clear();
		}

		internal void Unload()
		{
			foreach (ISrvBindable value in m_bitmapTextureById.Values)
			{
				ITexture texture;
				if ((texture = value as ITexture) != null)
				{
					texture.OnFormatChanged -= OnFormatChanged;
				}
				MyManagers.Textures.UnloadTexture(value.Name);
			}
			m_bitmapTextureById.Clear();
		}

		private float ComputeLineLength(string text, int id, char cLast, float scale, float spacingScaled)
		{
			float num = 0f;
			for (int i = id; i < text.Length; i++)
			{
				if (text[i] == '\n')
				{
					num += (float)(i - id) * spacingScaled;
					break;
				}
				num += ComputeScaledAdvanceWithKern(text[i], cLast, scale);
				if (i == text.Length - 1)
				{
					num += (float)(i - id) * spacingScaled;
				}
			}
			return num;
		}

		private float ComputeAlignmentOffset(string text, int id, char cLast, float scale, float spacingScaled, MyRenderTextAlignmentEnum alignment, int textureWidthInPx = 0)
		{
			if (alignment != 0 && (uint)(alignment - 1) <= 1u)
			{
				float num = (float)textureWidthInPx - ComputeLineLength(text, id, cLast, scale, spacingScaled);
				if (alignment == MyRenderTextAlignmentEnum.Align_Center)
				{
					return num / 2f;
				}
				return num;
			}
			return 0f;
		}

		/// <summary>
		/// Draw the given string at vOrigin using the specified color
		/// </summary>
<<<<<<< HEAD
		/// <param name="renderer"></param>
		/// <param name="position">Direction on the baseline. Text will advance from this position.</param>
		/// <param name="colorMask"></param>
		/// <param name="text"></param>
		/// <param name="scale"></param>
		/// <param name="ignoreBounds"></param>
		/// <param name="maxTextWidth">Maximum width of the text. Texts wider than this will be truncated and they will end with an ellipsis.</param>
		/// <returns>Width of the text (in pixels).</returns>                
=======
		/// <param name="position">Direction on the baseline. Text will advance from this position.</param>
		/// <param name="maxTextWidth">Maximum width of the text. Texts wider than this will be truncated and they will end with an ellipsis.</param>
		/// <returns>Width of the text (in pixels).</returns>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		internal float DrawString(MySpritesRenderer renderer, Vector2 position, Color colorMask, string text, float scale, bool ignoreBounds, float maxTextWidth = float.PositiveInfinity)
		{
			float pxWidth = 0f;
			if (string.IsNullOrEmpty(text) || scale == 0f || m_bitmapTextureById.Count == 0)
			{
				return 0f;
			}
			if (m_giList == null)
			{
				m_giList = new List<MyGlyphInfo>();
			}
			else
			{
				m_giList.Clear();
			}
			if (m_offsets == null)
			{
				m_offsets = new List<int>();
			}
			else
			{
				m_offsets.Clear();
			}
			scale *= 144f / 185f;
			char cLast = '\0';
			Vector2 vAt = position;
			float num = (float)Spacing * scale;
			colorMask *= m_colorMask;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < text.Length; i++)
			{
				if (TryGetGlyphInfo(text[i], out var info))
				{
					m_giList.Add(info);
				}
				else
				{
					m_giList.Add(null);
				}
			}
			for (int j = 0; j < text.Length; j++)
			{
				char c = text[j];
				MyGlyphInfo info = m_giList[j];
				if (c == '\n')
				{
					if (m_offsets.Count == 0)
					{
						m_offsets = GetLineOffsets(text, m_giList, scale);
					}
					num3 = 0;
					vAt.X = position.X;
					num2++;
					pxWidth = 0f;
				}
				else
				{
					if (info == null)
					{
						continue;
					}
					if (num2 >= 1 && m_offsets.Count >= num2)
					{
						num3 = m_offsets[num2 - 1];
					}
					if (j + 1 != text.Length && vAt.X - position.X + ComputeScaledAdvanceWithKern(c, info, cLast, scale) + ComputeScaledAdvanceWithKern('…', info, c, scale) >= maxTextWidth)
					{
						DrawCharGlyph(renderer, info, ref position, scale, ref colorMask, ref pxWidth, ref cLast, ref vAt, num2, '…', ignoreBounds, num3);
						pxWidth += num;
						vAt.X += num;
						for (; j < text.Length; j++)
						{
							if (c == '\n')
							{
								break;
							}
						}
						if (j < text.Length && c == '\n')
						{
							j--;
						}
					}
					else
					{
						DrawCharGlyph(renderer, info, ref position, scale, ref colorMask, ref pxWidth, ref cLast, ref vAt, num2, c, ignoreBounds, num3);
						if (j < text.Length - 1)
						{
							pxWidth += num;
							vAt.X += num;
						}
					}
				}
			}
			return pxWidth;
		}

		internal float DrawStringAligned(MySpritesRenderer renderer, Vector2 position, Color colorMask, string text, float scale, bool ignoreBounds, float maxTextWidth = float.PositiveInfinity, int textureWidthInPx = 512, MyRenderTextAlignmentEnum alignment = MyRenderTextAlignmentEnum.Align_Left)
		{
			float pxWidth = 0f;
			if (m_bitmapTextureById.Count == 0 || string.IsNullOrEmpty(text))
			{
				return 0f;
			}
			scale *= 144f / 185f;
			char cLast = '\0';
			Vector2 vAt = position;
			float num = (float)Spacing * scale;
			colorMask *= m_colorMask;
			int num2 = 0;
			int num3 = 0;
			if (m_giList == null)
			{
				m_giList = new List<MyGlyphInfo>();
			}
			else
			{
				m_giList.Clear();
			}
			if (m_offsets == null)
			{
				m_offsets = new List<int>();
			}
			else
			{
				m_offsets.Clear();
			}
			for (int i = 0; i < text.Length; i++)
			{
				if (TryGetGlyphInfo(text[i], out var info))
				{
					m_giList.Add(info);
				}
				else
				{
					m_giList.Add(null);
				}
			}
			vAt.X = position.X + ComputeAlignmentOffset(text, 0, cLast, scale, num, alignment, textureWidthInPx);
			for (int j = 0; j < text.Length; j++)
			{
				char c = text[j];
				MyGlyphInfo info = m_giList[j];
				if (c == '\n')
				{
					vAt.X = position.X + ComputeAlignmentOffset(text, j + 1, cLast, num, scale, alignment, textureWidthInPx);
					num2++;
					if (m_offsets.Count == 1)
					{
						m_offsets = GetLineOffsets(text, m_giList, scale);
					}
				}
				else
				{
					if (info == null)
					{
						continue;
					}
					if (num2 >= 1 && m_offsets.Count >= num2)
					{
						num3 = m_offsets[num2 - 1];
					}
					if (maxTextWidth < float.PositiveInfinity)
					{
						float num4 = ComputeScaledAdvanceWithKern(cLast, c, scale);
						float num5 = ComputeScaledAdvanceWithKern('…', c, scale);
						if (vAt.X - position.X + num4 + num5 >= maxTextWidth && j + 1 != text.Length)
						{
							DrawCharGlyph(renderer, info, ref position, scale, ref colorMask, ref pxWidth, ref cLast, ref vAt, num2, '…', ignoreBounds, num3);
							pxWidth += num;
							vAt.X += num;
							for (; j < text.Length && text[j] != '\n'; j++)
							{
							}
							if (j < text.Length && text[j] == '\n')
							{
								j--;
							}
							continue;
						}
					}
					DrawCharGlyph(renderer, info, ref position, scale, ref colorMask, ref pxWidth, ref cLast, ref vAt, num2, c, ignoreBounds, num3);
					if (j < text.Length - 1)
					{
						pxWidth += num;
						vAt.X += num;
					}
				}
			}
			return pxWidth;
		}

		private List<int> GetLineOffsets(string text, List<MyGlyphInfo> giList, float scale)
		{
			List<int> list = new List<int> { 0 };
			if (text == null || (text != null && text.Length == 0) || giList == null)
			{
				return list;
			}
			int averageLineHeight = AverageLineHeight;
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] != '\n')
				{
					continue;
				}
				averageLineHeight = AverageLineHeight;
				bool flag = false;
				int num = 0;
				for (int j = i + 1; j < text.Length && text[j] != '\n'; j++)
				{
					MyGlyphInfo myGlyphInfo = giList[j];
					if (myGlyphInfo == null)
					{
						continue;
					}
					num = myGlyphInfo.pxHeight + myGlyphInfo.pxHeightOffset;
					if (num > averageLineHeight)
					{
						averageLineHeight = num;
						if (averageLineHeight > AverageLineHeight)
						{
							flag = true;
						}
					}
				}
				if (list.Count == 0)
				{
					list.Add(0);
				}
				else
				{
					list.Add(list[list.Count - 1]);
				}
				if (flag)
				{
					list[list.Count - 1] += averageLineHeight - AverageLineHeight;
				}
			}
			return list;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void DrawCharGlyph(MySpritesRenderer renderer, MyGlyphInfo ginfo, ref Vector2 vAtOriginal, float scale, ref Color currentColor, ref float pxWidth, ref char cLast, ref Vector2 vAt, int line, char c, bool ignoreBounds, float lineOffset = 0f)
		{
			if (KernEnabled)
			{
				int num = CalcKern(cLast, c);
				vAt.X += (float)num * scale;
				pxWidth += (float)num * scale;
				cLast = c;
			}
			vAt.Y = vAtOriginal.Y + ((float)(ginfo.pxLeftSideBearing + ginfo.pxHeightOffset) + 3.83333325f + (float)(line * base.LineHeight)) * scale;
			vAt.Y += lineOffset * scale;
			vAt.X += (float)ginfo.pxLeftSideBearing * scale;
			if (ginfo.pxWidth != 0 && ginfo.pxHeight != 0)
			{
				Rectangle value = new Rectangle(ginfo.pxLocX, ginfo.pxLocY, ginfo.pxWidth, ginfo.pxHeight);
				Color color = currentColor;
				renderer.AddSingleSprite(m_bitmapTextureById[ginfo.nBitmapID], color, Vector2.Zero, Vector2.UnitX, value, new RectangleF(vAt.X, vAt.Y, scale * (float)value.Width, scale * (float)value.Height), ignoreBounds, m_premultipliedAlpha);
			}
			pxWidth += (float)(int)ginfo.pxAdvanceWidth * scale;
			vAt.X += (float)(ginfo.pxAdvanceWidth - ginfo.pxLeftSideBearing) * scale;
		}
	}
}
