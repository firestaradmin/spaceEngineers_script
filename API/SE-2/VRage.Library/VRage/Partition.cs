using System;
using System.Linq;

namespace VRage
{
	public static class Partition
	{
		private static readonly string[] m_letters = Enumerable.ToArray<string>(Enumerable.Select<int, string>(Enumerable.Range(65, 26), (Func<int, string>)((int s) => new string((char)s, 1))));

		public static T Select<T>(int num, T a, T b)
		{
			if (num % 2 != 0)
			{
				return b;
			}
			return a;
		}

		public static T Select<T>(int num, T a, T b, T c)
		{
			return ((uint)num % 3u) switch
			{
				1u => b, 
				0u => a, 
				_ => c, 
			};
		}

		public static T Select<T>(int num, T a, T b, T c, T d)
		{
			return ((uint)num % 4u) switch
			{
				2u => c, 
				1u => b, 
				0u => a, 
				_ => d, 
			};
		}

		public static T Select<T>(int num, T a, T b, T c, T d, T e)
		{
			return ((uint)num % 5u) switch
			{
				3u => d, 
				2u => c, 
				1u => b, 
				0u => a, 
				_ => e, 
			};
		}

		public static T Select<T>(int num, T a, T b, T c, T d, T e, T f)
		{
			return ((uint)num % 6u) switch
			{
				0u => a, 
				1u => b, 
				2u => c, 
				3u => d, 
				4u => e, 
				_ => f, 
			};
		}

		public static T Select<T>(int num, T a, T b, T c, T d, T e, T f, T g)
		{
			return ((uint)num % 7u) switch
			{
				0u => a, 
				1u => b, 
				2u => c, 
				3u => d, 
				4u => e, 
				5u => f, 
				_ => g, 
			};
		}

		public static T Select<T>(int num, T a, T b, T c, T d, T e, T f, T g, T h)
		{
			return ((uint)num % 8u) switch
			{
				0u => a, 
				1u => b, 
				2u => c, 
				3u => d, 
				4u => e, 
				5u => f, 
				6u => g, 
				_ => h, 
			};
		}

		public static T Select<T>(int num, T a, T b, T c, T d, T e, T f, T g, T h, T i)
		{
			return ((uint)num % 9u) switch
			{
				0u => a, 
				1u => b, 
				2u => c, 
				3u => d, 
				4u => e, 
				5u => f, 
				6u => g, 
				7u => h, 
				_ => i, 
			};
		}

		public static string SelectStringByLetter(char c)
		{
			if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
			{
				c = char.ToUpperInvariant(c);
				return m_letters[c - 65];
			}
			if (c >= '0' && c <= '9')
			{
				return "0-9";
			}
			return "Non-letter";
		}

		public static string SelectStringGroupOfTenByLetter(char c)
		{
			c = char.ToUpperInvariant(c);
			if (c >= '0' && c <= '9')
			{
				return "0-9";
			}
			switch (c)
			{
			case 'A':
			case 'B':
			case 'C':
				return "A-C";
			case 'D':
			case 'E':
			case 'F':
				return "D-F";
			case 'G':
			case 'H':
			case 'I':
				return "G-I";
			case 'J':
			case 'K':
			case 'L':
				return "J-L";
			case 'M':
			case 'N':
			case 'O':
				return "M-O";
			case 'P':
			case 'Q':
			case 'R':
				return "P-R";
			case 'S':
			case 'T':
			case 'U':
			case 'V':
				return "S-V";
			case 'W':
			case 'X':
			case 'Y':
			case 'Z':
				return "W-Z";
			default:
				return "Non-letter";
			}
		}
	}
}
