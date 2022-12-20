using System;
using System.Text.RegularExpressions;

namespace VRage.Filesystem.FindFilesRegEx
{
	public static class FindFilesPatternToRegex
	{
		private static Regex HasQuestionMarkRegEx = new Regex("\\?", (RegexOptions)8);

		private static Regex IlegalCharactersRegex = new Regex("[\\/:<>|\"]", (RegexOptions)8);

		private static Regex CatchExtentionRegex = new Regex("^\\s*.+\\.([^\\.]+)\\s*$", (RegexOptions)8);

		private static string NonDotCharacters = "[^.]*";

		public static Regex Convert(string pattern)
		{
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Expected O, but got Unknown
			if (pattern == null)
			{
				throw new ArgumentNullException();
			}
			pattern = pattern.Trim();
			if (pattern.Length == 0)
			{
				throw new ArgumentException("Pattern is empty.");
			}
			if (IlegalCharactersRegex.IsMatch(pattern))
			{
				throw new ArgumentException("Patterns contains ilegal characters.");
			}
			bool flag = CatchExtentionRegex.IsMatch(pattern);
			bool flag2 = false;
			if (HasQuestionMarkRegEx.IsMatch(pattern))
			{
				flag2 = true;
			}
			else if (flag)
			{
<<<<<<< HEAD
				flag2 = CatchExtentionRegex.Match(pattern).Groups[1].Length != 3;
=======
				flag2 = ((Capture)CatchExtentionRegex.Match(pattern).get_Groups().get_Item(1)).get_Length() != 3;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			string text = Regex.Escape(pattern);
			text = "^" + Regex.Replace(text, "\\\\\\*", ".*");
			text = Regex.Replace(text, "\\\\\\?", ".");
			if (!flag2 && flag)
			{
				text += NonDotCharacters;
			}
			text += "$";
			return new Regex(text, (RegexOptions)9);
		}
	}
}
