using System.Text;

namespace VRage.Utils
{
	public static class MyStringUtils
	{
		public const string OPEN_SQUARE_BRACKET = "U+005B";

		public const string CLOSED_SQUARE_BRACKET = "U+005D";

		/// <summary>
		/// Converts '[' and ']' into their UTF form to avoid being removed by notification processing system.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string UpdateControlsToNotificationFriendly(this string text)
		{
			return text.Replace("[", "U+005B").Replace("]", "U+005D");
		}

		/// <summary>
		/// Converts '[' and ']' UTF form to the regular characters.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string UpdateControlsFromNotificationFriendly(this string text)
		{
			return text.Replace("U+005B", "[").Replace("U+005D", "]");
		}

		/// <summary>
		/// Converts '[' and ']' UTF form to the regular characters.
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static StringBuilder UpdateControlsFromNotificationFriendly(this StringBuilder text)
		{
			return text.Replace("U+005B", "[").Replace("U+005D", "]");
		}

		/// <summary>
		/// Platform agnostic hash code (same as x64 .NET Framework)
		/// </summary>
		public unsafe static int GetUniversalHashCode(this string str)
		{
			fixed (char* ptr = str)
			{
				int num = 5381;
				int num2 = num;
				char* ptr2 = ptr;
				int num3;
				while ((num3 = *ptr2) != 0)
				{
					num = ((num << 5) + num) ^ num3;
					int num4 = ptr2[1];
					if (num4 == 0)
					{
						break;
					}
					num2 = ((num2 << 5) + num2) ^ num4;
					ptr2 += 2;
				}
				return num + num2 * 1566083941;
			}
		}
	}
}
