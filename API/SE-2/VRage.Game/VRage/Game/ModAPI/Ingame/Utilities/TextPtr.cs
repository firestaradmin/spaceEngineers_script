using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
	/// <summary>
	/// A parser utility structure representing a pointer to a location within a string.
	/// </summary>
	[DebuggerDisplay("{ToDebugString(),nq}")]
	public struct TextPtr
	{
		/// <summary>
		/// The original content string
		/// </summary>
		public readonly string Content;

		/// <summary>
		/// The index being pointed at by this structure
		/// </summary>
		public readonly int Index;

		/// <summary>
		/// Returns the character currently being pointed at, or <c>\0</c> if out of bounds
		/// </summary>
		public char Char
		{
			get
			{
				if (!IsOutOfBounds())
				{
					return Content[Index];
				}
				return '\0';
			}
		}

		/// <summary>
		/// Determines whether this pointer is an empty pointer, i.e. not pointing at anything at all.
		/// </summary>
		public bool IsEmpty => Content == null;

		/// <summary>
		/// Implicitly return the index into the string
		/// </summary>
		/// <param name="ptr"></param>
		public static implicit operator int(TextPtr ptr)
		{
			return ptr.Index;
		}

		/// <summary>
		/// Implicitly return the original string
		/// </summary>
		/// <param name="ptr"></param>
		public static implicit operator string(TextPtr ptr)
		{
			return ptr.Content;
		}

		/// <summary>
		/// Add an offset to a pointer
		/// </summary>
		/// <param name="ptr"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		public static TextPtr operator +(TextPtr ptr, int offset)
		{
			return new TextPtr(ptr.Content, ptr.Index + offset);
		}

		/// <summary>
		/// Increment a pointer by one
		/// </summary>
		/// <param name="ptr"></param>
		/// <returns></returns>
		public static TextPtr operator ++(TextPtr ptr)
		{
			return new TextPtr(ptr.Content, ptr.Index + 1);
		}

		/// <summary>
		/// Subtract an offset from a pointer
		/// </summary>
		/// <param name="ptr"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		public static TextPtr operator -(TextPtr ptr, int offset)
		{
			return new TextPtr(ptr.Content, ptr.Index - offset);
		}

		/// <summary>
		/// Decrement a pointer by one
		/// </summary>
		/// <param name="ptr"></param>
		/// <returns></returns>
		public static TextPtr operator --(TextPtr ptr)
		{
			return new TextPtr(ptr.Content, ptr.Index - 1);
		}

		/// <summary>
		/// Create a new text pointer at the first character in the given string
		/// </summary>
		/// <param name="content"></param>
		public TextPtr(string content)
		{
			Content = content;
			Index = 0;
		}

		/// <summary>
		/// Create a new text pointer at the given index of the given string
		/// </summary>
		/// <param name="content"></param>
		/// <param name="index"></param>
		public TextPtr(string content, int index)
		{
			Content = content;
			Index = index;
		}

		/// <summary>
		/// Determines whether this pointer is currently out of bounds (before or after the string content)
		/// </summary>
		/// <returns></returns>
		public bool IsOutOfBounds()
		{
			if (Index >= 0)
			{
				return Index >= Content.Length;
			}
			return true;
		}

		/// <summary>
		/// Determines what line number this pointer is currently at.
		/// </summary>
		/// <returns></returns>
		public int FindLineNo()
		{
			string content = Content;
			int index = Index;
			int num = 1;
			for (int i = 0; i < index; i++)
			{
				if (content[i] == '\n')
				{
					num++;
				}
			}
			return num;
		}

		/// <summary>
		/// Finds the given text string
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public TextPtr Find(string str)
		{
			if (IsOutOfBounds())
			{
				return this;
			}
			int num = Content.IndexOf(str, Index, StringComparison.InvariantCulture);
			if (num == -1)
			{
				return new TextPtr(Content, Content.Length);
			}
			return new TextPtr(Content, num);
		}

		/// <summary>
		/// Finds the given character
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		public TextPtr Find(char ch)
		{
			if (IsOutOfBounds())
			{
				return this;
			}
			int num = Content.IndexOf(ch, Index);
			if (num == -1)
			{
				return new TextPtr(Content, Content.Length);
			}
			return new TextPtr(Content, num);
		}

		/// <summary>
		/// Finds one of the given characters
		/// </summary>
		/// <param name="chs"></param>
		/// <returns></returns>
		public TextPtr FindAny(char[] chs)
		{
			if (IsOutOfBounds())
			{
				return this;
			}
			int num = Content.IndexOfAny(chs, Index);
			if (num == -1)
			{
				return new TextPtr(Content, Content.Length);
			}
			return new TextPtr(Content, num);
		}

		/// <summary>
		/// Finds the given character within the current line
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		public TextPtr FindInLine(char ch)
		{
			if (IsOutOfBounds())
			{
				return this;
			}
			string content = Content;
			int length = Content.Length;
			for (int i = Index; i < length; i++)
			{
				char c = content[i];
				if (c == ch)
				{
					return new TextPtr(content, i);
				}
				if (c == '\r' || c == '\n')
				{
					break;
				}
			}
			return new TextPtr(Content, Content.Length);
		}

		/// <summary>
		/// Finds one of the given characters within the current line
		/// </summary>
		/// <param name="chs"></param>
		/// <returns></returns>
		public TextPtr FindAnyInLine(char[] chs)
		{
			if (IsOutOfBounds())
			{
				return this;
			}
			string content = Content;
			int length = Content.Length;
			for (int i = Index; i < length; i++)
			{
				char c = content[i];
				if (Array.IndexOf(chs, c) >= 0)
				{
					return new TextPtr(content, i);
				}
				if (c == '\r' || c == '\n')
				{
					break;
				}
			}
			return new TextPtr(Content, Content.Length);
		}

		/// <summary>
		/// Finds the end of the current line
		/// </summary>
		/// <param name="skipNewline">Whether the actual newline character(s) should also be skipped. Defaults to false</param>
		/// <returns></returns>
		public TextPtr FindEndOfLine(bool skipNewline = false)
		{
			int length = Content.Length;
			if (Index >= length)
			{
				return this;
			}
			TextPtr result = this;
			while (result.Index < length)
			{
				if (result.IsNewLine())
				{
					if (skipNewline)
					{
						if (result.Char == '\r')
						{
							++result;
						}
						++result;
					}
					break;
				}
				++result;
			}
			return result;
		}

		/// <summary>
		/// Determines if the current pointer location starts with the given string - in a case insensitive manner.
		/// </summary>
		/// <param name="what"></param>
		/// <returns></returns>
		public bool StartsWithCaseInsensitive(string what)
		{
			TextPtr textPtr = this;
			foreach (char c in what)
			{
				if (char.ToUpper(textPtr.Char) != char.ToUpper(c))
				{
					return false;
				}
				++textPtr;
			}
			return true;
		}

		/// <summary>
		/// Determines if the current pointer location starts with the given string - in a case sensitive manner.
		/// </summary>
		/// <param name="what"></param>
		/// <returns></returns>
		public bool StartsWith(string what)
		{
			TextPtr textPtr = this;
			foreach (char c in what)
			{
				if (textPtr.Char != c)
				{
					return false;
				}
				++textPtr;
			}
			return true;
		}

		/// <summary>
		/// Skips whitespace
		/// </summary>
		/// <param name="skipNewline">Whether to also skip newlines</param>
		/// <returns></returns>
		public TextPtr SkipWhitespace(bool skipNewline = false)
		{
			TextPtr result = this;
			int length = result.Content.Length;
			if (skipNewline)
			{
				while (true)
				{
					char @char = result.Char;
					if (result.Index >= length || !char.IsWhiteSpace(@char))
					{
						break;
					}
					++result;
				}
				return result;
			}
			while (true)
			{
				char char2 = result.Char;
				if (result.Index >= length || result.IsNewLine() || !char.IsWhiteSpace(char2))
				{
					break;
				}
				++result;
			}
			return result;
		}

		/// <summary>
		/// Determines whether the pointer is currently at the end of a line (right before a newline character set or end of the string)
		/// </summary>
		/// <returns></returns>
		public bool IsEndOfLine()
		{
			if (Index < Content.Length)
			{
				return IsNewLine();
			}
			return true;
		}

		/// <summary>
		/// Determines whether the pointer is currently at the beginning of a line (right after a newline character set or start of the string)
		/// </summary>
		/// <returns></returns>
		public bool IsStartOfLine()
		{
			if (Index > 0)
			{
				return (this - 1).IsNewLine();
			}
			return true;
		}

		/// <summary>
		/// Determines whether the pointer is currently at a newline (end of the string is not a newline)
		/// </summary>
		/// <returns></returns>
		public bool IsNewLine()
		{
			switch (Char)
			{
			case '\r':
				if (Index < Content.Length - 1)
				{
					return Content[Index + 1] == '\n';
				}
				return false;
			case '\n':
				return true;
			default:
				return false;
			}
		}

		/// <summary>
		/// Advances the pointer location until all whitespace is skipped - does not skip newlines
		/// </summary>
		/// <returns></returns>
		public TextPtr TrimStart()
		{
			string content = Content;
			int i = Index;
			for (int length = content.Length; i < length; i++)
			{
				char c = content[i];
				if (c != ' ' && c != '\t')
				{
					break;
				}
			}
			return new TextPtr(content, i);
		}

		/// <summary>
		/// Reverses the pointer location until all whitespace is skipped - does not skip newlines
		/// </summary>
		/// <returns></returns>
		public TextPtr TrimEnd()
		{
			string content = Content;
			int num;
			for (num = Index - 1; num >= 0; num--)
			{
				char c = content[num];
				if (c != ' ' && c != '\t')
				{
					break;
				}
			}
			return new TextPtr(content, num + 1);
		}

		private string ToDebugString()
		{
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Expected O, but got Unknown
			if (Index < 0)
			{
				return "<before>";
			}
			if (Index >= Content.Length)
			{
				return "<after>";
			}
			int num = Index + 37;
<<<<<<< HEAD
			string input = ((num <= Content.Length) ? (Content.Substring(Index, num - Index) + "...") : Content.Substring(Index, Content.Length - Index));
			return Regex.Replace(input, "[\\r\\t\\n]", delegate(Match match)
=======
			string text = ((num <= Content.Length) ? (Content.Substring(Index, num - Index) + "...") : Content.Substring(Index, Content.Length - Index));
			object obj = _003C_003Ec._003C_003E9__30_0;
			if (obj == null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				MatchEvaluator val = (Match match) => ((Capture)match).get_Value() switch
				{
					"\r" => "\\r", 
					"\t" => "\\t", 
					"\n" => "\\n", 
					_ => ((Capture)match).get_Value(), 
				};
				obj = (object)val;
				_003C_003Ec._003C_003E9__30_0 = val;
			}
			return Regex.Replace(text, "[\\r\\t\\n]", (MatchEvaluator)obj);
		}
	}
}
