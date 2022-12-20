using System;
using System.Collections.Generic;
using VRage.Utils;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
	/// <summary>
	/// Represents a segment of a string.
	/// </summary>
	public struct StringSegment
	{
		private static readonly char[] NEWLINE_CHARS = new char[2] { '\r', '\n' };

		/// <summary>
		/// The original text string
		/// </summary>
		public readonly string Text;

		/// <summary>
		/// Where the segment starts
		/// </summary>
		public readonly int Start;

		/// <summary>
		/// The length of the segment
		/// </summary>
		public readonly int Length;

		private string m_cache;

		/// <summary>
		/// Determines whether this is an empty/undefined string segment
		/// </summary>
		public bool IsEmpty => Text == null;

		/// <summary>
		/// Determines whether this segment has been pre-cached in such a way that no allocation will occur when using <see cref="M:VRage.Game.ModAPI.Ingame.Utilities.StringSegment.ToString" />
		/// </summary>
		public bool IsCached => m_cache != null;

		/// <summary>
		/// Gets a character at a position relative to <see cref="F:VRage.Game.ModAPI.Ingame.Utilities.StringSegment.Start" />.
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public char this[int i]
		{
			get
			{
				if (i < 0 || i >= Length)
				{
					return '\0';
				}
				return Text[Start + i];
			}
		}

		/// <summary>
		/// Creates a new <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.StringSegment" /> encompassing an entire string
		/// </summary>
		/// <param name="text"></param>
		public StringSegment(string text)
			: this(text, 0, text.Length)
		{
			m_cache = text;
		}

		/// <summary>
		/// Creates a new <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.StringSegment" /> for the given string area
		/// </summary>
		/// <param name="text">The original text string</param>
		/// <param name="start">Where the segment starts</param>
		/// <param name="length">The length of the segment</param>
		public StringSegment(string text, int start, int length)
		{
			Text = text;
			Start = start;
			Length = Math.Max(0, length);
			m_cache = null;
		}

		/// <summary>
		/// Reports the zero-based index of the first occurence of the specified character, relative to <see cref="F:VRage.Game.ModAPI.Ingame.Utilities.StringSegment.Start" />. Returns -1 if nothing was found.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		public int IndexOf(char ch)
		{
			if (Length == 0)
			{
				return -1;
			}
			int num = Text.IndexOf(ch, Start, Length);
			if (num >= 0)
			{
				return num - Start;
			}
			return -1;
		}

		/// <summary>
		/// Reports the zero-based index of the first occurence of the specified character, relative to <see cref="F:VRage.Game.ModAPI.Ingame.Utilities.StringSegment.Start" />. Returns -1 if nothing was found.
		/// </summary>
		/// <param name="ch"></param>
		/// <param name="start">Where to start the search (relative to <see cref="F:VRage.Game.ModAPI.Ingame.Utilities.StringSegment.Start" />)</param>
		/// <returns></returns>
		public int IndexOf(char ch, int start)
		{
			if (Length == 0)
			{
				return -1;
			}
			int num = Text.IndexOf(ch, Start + start, Length);
			if (num >= 0)
			{
				return num - Start;
			}
			return -1;
		}

		/// <summary>
		/// Reports the zero-based index of the first occurence of one of the provided characters, relative to <see cref="F:VRage.Game.ModAPI.Ingame.Utilities.StringSegment.Start" />. Returns -1 if nothing was found.
		/// </summary>
		/// <param name="chars"></param>
		/// <returns></returns>
		public int IndexOfAny(char[] chars)
		{
			if (Length == 0)
			{
				return -1;
			}
			int num = Text.IndexOfAny(chars, Start, Length);
			if (num >= 0)
			{
				return num;
			}
			return -1;
		}

		/// <summary>Indicates whether this instance and a specified object are equal.</summary>
		/// <param name="obj">The object to compare with the current instance.</param>
		/// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			string text = obj as string;
			if (text != null)
			{
				return Equals(text);
			}
			return false;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		public override int GetHashCode()
		{
			return MyUtils.GetHash(Text, Start, Length);
		}

		/// <summary>
		/// Compares this string segment with the given string in a case sensitive manner.
		/// </summary>
		/// <param name="other"></param>
		/// <returns><c>true</c> if the string segment equals the string; <c>false</c> otherwise.</returns>
		public bool Equals(string other)
		{
			return Equals(new StringSegment(other));
		}

		/// <summary>
		/// Compares this string segment with another in a case sensitive manner.
		/// </summary>
		/// <param name="other"></param>
		/// <returns><c>true</c> if the string segments are equal; <c>false</c> otherwise.</returns>
		public bool Equals(StringSegment other)
		{
			if (Length != other.Length)
			{
				return false;
			}
			string text = Text;
			int num = Start;
			string text2 = other.Text;
			int num2 = other.Start;
			for (int i = 0; i < Length; i++)
			{
				if (text[num] != text2[num2])
				{
					return false;
				}
				num++;
				num2++;
			}
			return true;
		}

		/// <summary>
		/// Compares this string segment with the given string in a case insensitive manner.
		/// </summary>
		/// <param name="other"></param>
		/// <returns><c>true</c> if the string segment equals the string; <c>false</c> otherwise.</returns>
		public bool EqualsIgnoreCase(string other)
		{
			return EqualsIgnoreCase(new StringSegment(other));
		}

		/// <summary>
		/// Compares this string segment with another in a case insensitive manner.
		/// </summary>
		/// <param name="other"></param>
		/// <returns><c>true</c> if the string segments are equal; <c>false</c> otherwise.</returns>
		public bool EqualsIgnoreCase(StringSegment other)
		{
			if (Length != other.Length)
			{
				return false;
			}
			string text = Text;
			int num = Start;
			string text2 = other.Text;
			int num2 = other.Start;
			for (int i = 0; i < Length; i++)
			{
				if (char.ToUpperInvariant(text[num]) != char.ToUpperInvariant(text2[num2]))
				{
					return false;
				}
				num++;
				num2++;
			}
			return true;
		}

		/// <summary>
		/// Returns a string containing just this segment.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if (Length == 0)
			{
				return "";
			}
			if (m_cache == null)
			{
				m_cache = Text.Substring(Start, Length);
			}
			return m_cache;
		}

		/// <summary>
		/// Fills a list with individual string segments representing the lines of text within this string segment, separated by newlines.
		/// </summary>
		/// <param name="lines"></param>
		public void GetLines(List<StringSegment> lines)
		{
			if (Length == 0)
			{
				return;
			}
			string text = Text;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			int num = Start;
			int num2 = num + Length;
			lines.Clear();
			while (num < num2)
			{
				int num3 = text.IndexOfAny(NEWLINE_CHARS, num, num2 - num);
				if (num3 < 0)
				{
					lines.Add(new StringSegment(text, num, text.Length - num));
					break;
				}
				lines.Add(new StringSegment(text, num, num3 - num));
				num = num3;
				if (num < text.Length && text[num] == '\r')
				{
					num++;
				}
				if (num < text.Length && text[num] == '\n')
				{
					num++;
				}
			}
		}

		/// <summary>
		/// Fills a list with individual strings representing the lines of text within this string segment, separated by newlines.
		/// </summary>
		/// <param name="lines"></param>
		public void GetLines(List<string> lines)
		{
			if (Length == 0)
			{
				return;
			}
			string text = Text;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			int num = Start;
			int num2 = num + Length;
			lines.Clear();
			while (num < num2)
			{
				int num3 = text.IndexOfAny(NEWLINE_CHARS, num, num2 - num);
				if (num3 < 0)
				{
					lines.Add(text.Substring(num, text.Length - num));
					break;
				}
				lines.Add(text.Substring(num, num3 - num));
				num = num3;
				if (num < text.Length && text[num] == '\r')
				{
					num++;
				}
				if (num < text.Length && text[num] == '\n')
				{
					num++;
				}
			}
		}
	}
}
