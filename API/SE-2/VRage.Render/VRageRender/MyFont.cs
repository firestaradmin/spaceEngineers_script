using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using VRage.FileSystem;
using VRage.Utils;
using VRageMath;

namespace VRageRender
{
	public class MyFont
	{
		/// <summary>
		///  Info for each glyph in the font - where to find the glyph image and other properties
		/// </summary>
		protected class MyGlyphInfo
		{
			public ushort nBitmapID;

			public ushort pxLocX;

			public ushort pxLocY;

			public byte pxWidth;

			public byte pxHeight;

			public byte pxAdvanceWidth;

			public sbyte pxLeftSideBearing;

			public sbyte pxHeightOffset;
		}

		/// <summary>
		/// Info for each font bitmap
		/// </summary>
		protected struct MyBitmapInfo
		{
			public string strFilename;

			public int nX;

			public int nY;
		}

		protected struct KernPair
		{
			public char Left;

			public char Right;

			public KernPair(char l, char r)
			{
				Left = l;
				Right = r;
			}
		}

		protected class KernPairComparer : IComparer<KernPair>, IEqualityComparer<KernPair>
		{
			public int Compare(KernPair x, KernPair y)
			{
				if (x.Left != y.Left)
				{
					return x.Left.CompareTo(y.Left);
				}
				return x.Right.CompareTo(y.Right);
			}

			public bool Equals(KernPair x, KernPair y)
			{
				if (x.Left == y.Left)
				{
					return x.Right == y.Right;
				}
				return false;
			}

			public int GetHashCode(KernPair x)
			{
				return x.Left.GetHashCode() ^ x.Right.GetHashCode();
			}
		}

		/// <summary>
		/// Replacement character shown when we don't have something in our texture.
		/// Normally, this would be \uFFFD, but BMFontGen refuses to generate it, so I put its glyph at \u25A1 (empty square)
		/// </summary>
		protected const char REPLACEMENT_CHARACTER = '□';

		protected const char ELLIPSIS = '…';

		public const char NEW_LINE = '\n';

		private static readonly KernPairComparer m_kernPairComparer = new KernPairComparer();

		protected readonly Dictionary<int, MyBitmapInfo> m_bitmapInfoByID = new Dictionary<int, MyBitmapInfo>();

		protected readonly Dictionary<char, MyGlyphInfo> m_glyphInfoByChar = new Dictionary<char, MyGlyphInfo>();

		protected readonly Dictionary<KernPair, sbyte> m_kernByPair = new Dictionary<KernPair, sbyte>(m_kernPairComparer);

		protected readonly string m_fontDirectory;

		protected MyGlyphInfo m_replacementCharInfo;

		/// <summary>
		/// This is artificial spacing in between two characters (in pixels).
		/// Using it we can make spaces wider or narrower
		/// </summary>
		public int Spacing;

		/// <summary>
		/// Enable / disable kerning of adjacent character pairs.
		/// </summary>
		public bool KernEnabled = true;

		/// <summary>
		/// The depth at which to draw the font
		/// </summary>
		public float Depth;

		public int AverageLineHeight = 45;

		public int AverageFontLineOffset = 45;

		/// <summary>
		/// Distance from top of font to the baseline
		/// </summary>
		public int Baseline { get; private set; }

		/// <summary>
		/// Distance from top to bottom of the font
		/// </summary>
		public int LineHeight { get; private set; }

		/// <summary>
		/// Create a new font from the info in the specified font descriptor (XML) file
		/// </summary>
		public MyFont(string fontFilePath, int spacing = 1, bool dummyFont = false)
		{
			if (dummyFont)
			{
				return;
			}
			LogWriteLine("MyFont.Ctor - START");
			using (MyRenderProxy.Log?.IndentUsing(LoggingOptions.MISC_RENDER_ASSETS))
			{
				Spacing = spacing;
				LogWriteLine("Font filename: " + fontFilePath);
				string text = fontFilePath;
				if (!Path.IsPathRooted(fontFilePath))
				{
					text = Path.Combine(MyFileSystem.ContentPath, fontFilePath);
				}
				if (!MyFileSystem.FileExists(text))
				{
					throw new Exception($"Unable to find font path '{text}'.");
				}
				m_fontDirectory = Path.GetDirectoryName(text);
				LoadFontXML(text);
				m_glyphInfoByChar.TryGetValue('□', out m_replacementCharInfo);
				if (TryGetGlyphInfo('a', out var info))
				{
					AverageLineHeight = info.pxHeightOffset + info.pxHeight;
					AverageFontLineOffset = info.pxHeight - info.pxHeightOffset;
				}
				LogWriteLine("FontFilePath: " + text);
				LogWriteLine("LineHeight: " + LineHeight);
				LogWriteLine("Baseline: " + Baseline);
				LogWriteLine("KernEnabled: " + KernEnabled);
			}
			LogWriteLine("MyFont.Ctor - END");
		}

		private void LogWriteLine(string message)
		{
			MyRenderProxy.Log?.WriteLine(message);
		}

		public Vector2 MeasureString(string text, float scale, bool useMyRenderGuiConstants = true)
		{
			if (useMyRenderGuiConstants)
			{
				scale *= 144f / 185f;
			}
			float num = 0f;
			char chLeft = '\0';
			float num2 = 0f;
			int num3 = 1;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				MyGlyphInfo info;
				if (c == '\n')
				{
					num3++;
					num = 0f;
					chLeft = '\0';
					num6 = 0;
					num4 += num5;
					num5 = 0;
				}
				else if (TryGetGlyphInfo(c, out info))
				{
					int num7 = info.pxHeight + info.pxHeightOffset;
					if (num7 - LineHeight > num5)
					{
						num5 = num7 - LineHeight;
					}
					int num8 = info.pxHeight - info.pxHeightOffset - AverageFontLineOffset;
					if (num8 > num6)
					{
						num6 = num8;
					}
					if (KernEnabled)
					{
						num += (float)CalcKern(chLeft, c);
						chLeft = c;
					}
					num += (float)(int)info.pxAdvanceWidth;
					if (i < text.Length - 1)
					{
						num += (float)Spacing;
					}
					if (num > num2)
					{
						num2 = num;
					}
				}
			}
			return new Vector2(num2 * scale, (float)(num3 * LineHeight) * scale + (float)num4 / 2f * scale + (float)num6 * scale);
		}

		public Vector2 MeasureString(StringBuilder text, float scale, bool useMyRenderGuiConstants = true)
<<<<<<< HEAD
		{
			return MeasureString(text.ToString(), scale, useMyRenderGuiConstants);
		}

		protected bool TryGetGlyphInfo(char c, out MyGlyphInfo info)
		{
			if (m_glyphInfoByChar.TryGetValue(c, out info))
			{
				return true;
			}
			if (m_replacementCharInfo == null || !CanUseReplacementCharacter(c))
			{
				return false;
			}
=======
		{
			return MeasureString(text.ToString(), scale, useMyRenderGuiConstants);
		}

		protected bool TryGetGlyphInfo(char c, out MyGlyphInfo info)
		{
			if (m_glyphInfoByChar.TryGetValue(c, out info))
			{
				return true;
			}
			if (m_replacementCharInfo == null || !CanUseReplacementCharacter(c))
			{
				return false;
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			info = m_replacementCharInfo;
			return true;
		}

		protected MyGlyphInfo GetGlyphInfo(char c)
		{
			if (m_glyphInfoByChar.TryGetValue(c, out var value))
			{
				return value;
			}
			if (!CanUseReplacementCharacter(c))
			{
				return null;
			}
			return m_replacementCharInfo;
		}

		protected bool CanWriteOrReplace(ref char c)
		{
			if (m_glyphInfoByChar.ContainsKey(c))
			{
				return true;
			}
			if (!CanUseReplacementCharacter(c))
			{
				return false;
			}
			c = '□';
			return m_glyphInfoByChar.ContainsKey(c);
		}

		public int ComputeCharsThatFit(StringBuilder text, float scale, float maxTextWidth)
		{
			if (text == null || (text != null && text.Length == 0))
			{
				return 0;
			}
			scale *= 144f / 185f;
			maxTextWidth /= scale;
			float num = 0f;
			char chLeft = '\0';
			char c = text[0];
			for (int i = 0; i < text.Length; i++)
			{
				c = text[i];
				if (TryGetGlyphInfo(c, out var info))
				{
					if (KernEnabled)
					{
						num += (float)CalcKern(chLeft, c);
						chLeft = c;
					}
					num += (float)(int)info.pxAdvanceWidth;
					if (i < text.Length - 1)
					{
						num += (float)Spacing;
					}
					if (num > maxTextWidth)
					{
						return i;
					}
				}
			}
			return text.Length;
		}

		protected float ComputeScaledAdvanceWithKern(char c, char cLast, float scale)
		{
			if (!TryGetGlyphInfo(c, out var info))
			{
				return 0f;
			}
			return ComputeScaledAdvanceWithKern(c, info, cLast, scale);
		}

		protected float ComputeScaledAdvanceWithKern(char c, MyGlyphInfo ginfo, char cLast, float scale)
		{
			float num = 0f;
			if (KernEnabled)
			{
				int num2 = CalcKern(cLast, c);
				num += (float)num2 * scale;
			}
			return num + (float)(int)ginfo.pxAdvanceWidth * scale;
		}

		protected bool CanUseReplacementCharacter(char c)
		{
			if (!char.IsWhiteSpace(c))
			{
				return !char.IsControl(c);
			}
			return false;
		}

		protected int CalcKern(char chLeft, char chRight)
		{
			sbyte value = 0;
			m_kernByPair.TryGetValue(new KernPair(chLeft, chRight), out value);
			return value;
		}

		private void LoadFontXML(string path)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			XmlDocument val = new XmlDocument();
			using (Stream stream = MyFileSystem.OpenRead(path))
			{
				val.Load(stream);
			}
			LoadFontXML(((XmlNode)val).get_ChildNodes());
		}

		/// <summary>
		/// Load the font data from an XML font descriptor file
		/// </summary>
		/// <param name="xnl">XML node list containing the entire font descriptor file</param>
		private void LoadFontXML(XmlNodeList xnl)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			foreach (XmlNode item in xnl)
			{
				XmlNode val = item;
				if (val.get_Name() == "font")
				{
					Baseline = int.Parse(GetXMLAttribute(val, "base"));
					LineHeight = int.Parse(GetXMLAttribute(val, "height"));
					LoadFontXML_font(val.get_ChildNodes());
				}
			}
		}

		/// <summary>
		/// Load the data from the "font" node
		/// </summary>
		/// <param name="xnl">XML node list containing the "font" node's children</param>
		private void LoadFontXML_font(XmlNodeList xnl)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			foreach (XmlNode item in xnl)
			{
				XmlNode val = item;
				if (val.get_Name() == "bitmaps")
				{
					LoadFontXML_bitmaps(val.get_ChildNodes());
				}
				if (val.get_Name() == "glyphs")
				{
					LoadFontXML_glyphs(val.get_ChildNodes());
				}
				if (val.get_Name() == "kernpairs")
				{
					LoadFontXML_kernpairs(val.get_ChildNodes());
				}
			}
		}

		/// <summary>
		/// Load the data from the "bitmaps" node
		/// </summary>
		/// <param name="xnl">XML node list containing the "bitmaps" node's children</param>
		private void LoadFontXML_bitmaps(XmlNodeList xnl)
		{
<<<<<<< HEAD
=======
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyBitmapInfo value = default(MyBitmapInfo);
			foreach (XmlNode item in xnl)
			{
				XmlNode val = item;
				if (val.get_Name() == "bitmap")
				{
<<<<<<< HEAD
					string xMLAttribute = GetXMLAttribute(item, "id");
					string xMLAttribute2 = GetXMLAttribute(item, "name");
					string[] array = GetXMLAttribute(item, "size").Split(new char[1] { 'x' });
=======
					string xMLAttribute = GetXMLAttribute(val, "id");
					string xMLAttribute2 = GetXMLAttribute(val, "name");
					string[] array = GetXMLAttribute(val, "size").Split(new char[1] { 'x' });
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					value.strFilename = xMLAttribute2;
					value.nX = int.Parse(array[0]);
					value.nY = int.Parse(array[1]);
					m_bitmapInfoByID[int.Parse(xMLAttribute)] = value;
				}
			}
		}

		/// <summary>
		/// Load the data from the "glyphs" node
		/// </summary>
		/// <param name="xnl">XML node list containing the "glyphs" node's children</param>
		private void LoadFontXML_glyphs(XmlNodeList xnl)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			foreach (XmlNode item in xnl)
			{
				XmlNode val = item;
				if (val.get_Name() == "glyph")
				{
<<<<<<< HEAD
					string xMLAttribute = GetXMLAttribute(item, "ch");
					string xMLAttribute2 = GetXMLAttribute(item, "bm");
					string xMLAttribute3 = GetXMLAttribute(item, "loc");
					string xMLAttribute4 = GetXMLAttribute(item, "size");
					string xMLAttribute5 = GetXMLAttribute(item, "aw");
					string xMLAttribute6 = GetXMLAttribute(item, "lsb");
					string text = GetXMLAttribute(item, "ho");
					string xMLAttribute7 = GetXMLAttribute(item, "wo");
=======
					string xMLAttribute = GetXMLAttribute(val, "ch");
					string xMLAttribute2 = GetXMLAttribute(val, "bm");
					string xMLAttribute3 = GetXMLAttribute(val, "loc");
					string xMLAttribute4 = GetXMLAttribute(val, "size");
					string xMLAttribute5 = GetXMLAttribute(val, "aw");
					string xMLAttribute6 = GetXMLAttribute(val, "lsb");
					string text = GetXMLAttribute(val, "ho");
					string xMLAttribute7 = GetXMLAttribute(val, "wo");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					if (xMLAttribute3 == "")
					{
						xMLAttribute3 = GetXMLAttribute(val, "origin");
					}
					string[] array = xMLAttribute3.Split(new char[1] { ',' });
					string[] array2 = xMLAttribute4.Split(new char[1] { 'x' });
					if (text == "")
					{
						text = "0";
					}
					if (xMLAttribute7 == "")
					{
						xMLAttribute7 = "0";
					}
					MyGlyphInfo value = new MyGlyphInfo
					{
						nBitmapID = ushort.Parse(xMLAttribute2),
						pxLocX = ushort.Parse(array[0]),
						pxLocY = ushort.Parse(array[1]),
						pxWidth = byte.Parse(array2[0]),
						pxHeight = byte.Parse(array2[1]),
						pxAdvanceWidth = byte.Parse(xMLAttribute5),
						pxLeftSideBearing = sbyte.Parse(xMLAttribute6),
						pxHeightOffset = sbyte.Parse(text)
					};
					m_glyphInfoByChar[xMLAttribute[0]] = value;
				}
			}
			if (m_glyphInfoByChar.Count > 0)
			{
<<<<<<< HEAD
				var anon = (from x in (from h in m_glyphInfoByChar.Values
						select h.pxHeight into a
						group a by a into g
						select new
						{
							Height = g.Key,
							Count = g.Count()
						}).ToList()
					orderby x.Count descending
					select x).FirstOrDefault();
=======
				var anon = Enumerable.FirstOrDefault((IEnumerable<_003C_003Ef__AnonymousType0<byte, int>>)Enumerable.OrderByDescending(Enumerable.ToList(Enumerable.Select(Enumerable.GroupBy<byte, byte>(Enumerable.Select<MyGlyphInfo, byte>((IEnumerable<MyGlyphInfo>)m_glyphInfoByChar.Values, (Func<MyGlyphInfo, byte>)((MyGlyphInfo h) => h.pxHeight)), (Func<byte, byte>)((byte a) => a)), (IGrouping<byte, byte> g) => new
				{
					Height = g.get_Key(),
					Count = Enumerable.Count<byte>((IEnumerable<byte>)g)
				})), x => x.Count));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (anon != null)
				{
					AverageLineHeight = anon.Height;
				}
			}
		}

		/// <summary>
		/// Load the data from the "kernpairs" node
		/// </summary>
		/// <param name="xnl">XML node list containing the "kernpairs" node's children</param>
		private void LoadFontXML_kernpairs(XmlNodeList xnl)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			foreach (XmlNode item in xnl)
			{
				XmlNode val = item;
				if (val.get_Name() == "kernpair")
				{
					char l = GetXMLAttribute(val, "left")[0];
					char r = GetXMLAttribute(val, "right")[0];
					string xMLAttribute = GetXMLAttribute(val, "adjust");
					KernPair key = new KernPair(l, r);
					m_kernByPair[key] = sbyte.Parse(xMLAttribute);
				}
			}
		}

		/// <summary>
		/// Get the XML attribute value
		/// </summary>
		/// <param name="n">XML node</param>
		/// <param name="strAttr">Attribute name</param>
		/// <returns>Attribute value, or the empty string if the attribute doesn't exist</returns>
		private static string GetXMLAttribute(XmlNode n, string strAttr)
		{
			XmlNode namedItem = ((XmlNamedNodeMap)n.get_Attributes()).GetNamedItem(strAttr);
			XmlAttribute val = (XmlAttribute)(object)((namedItem is XmlAttribute) ? namedItem : null);
			if (val != null)
			{
				return ((XmlNode)val).get_Value();
			}
			return "";
		}
	}
}
