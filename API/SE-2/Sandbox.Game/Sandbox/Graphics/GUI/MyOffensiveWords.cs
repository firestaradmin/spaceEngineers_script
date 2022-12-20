using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Sandbox.Graphics.GUI
{
	public class MyOffensiveWords
	{
		private static MyOffensiveWords m_instance;

		private Regex m_blacklistRegEx;

		private Regex m_whitelistRegEx;

		public static MyOffensiveWords Instance => m_instance ?? (m_instance = new MyOffensiveWords());

		private Regex CreateFilterRegEx(List<string> list)
		{
<<<<<<< HEAD
=======
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			StringBuilder stringBuilder = new StringBuilder("\\b(");
			foreach (string item in list)
			{
				stringBuilder.Append(item.Replace("|", "[|]"));
				stringBuilder.Append("|");
			}
			if (list.Count > 0)
			{
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
			stringBuilder.Append(")\\b");
<<<<<<< HEAD
			return new Regex(stringBuilder.ToString(), RegexOptions.IgnoreCase | RegexOptions.Compiled);
=======
			return new Regex(stringBuilder.ToString(), (RegexOptions)9);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void Init(List<string> blacklist, List<string> whitelist)
		{
			m_blacklistRegEx = CreateFilterRegEx(blacklist ?? new List<string> { "" });
			m_whitelistRegEx = CreateFilterRegEx(whitelist ?? new List<string> { "" });
		}

		public string IsTextOffensive(string text)
		{
			if (m_blacklistRegEx == null)
			{
				return null;
			}
<<<<<<< HEAD
			string input = m_whitelistRegEx.Replace(text, string.Empty);
			Match match = m_blacklistRegEx.Match(input);
			if (match.Length <= 0)
			{
				return null;
			}
			return match.Value;
=======
			string text2 = m_whitelistRegEx.Replace(text, string.Empty);
			Match val = m_blacklistRegEx.Match(text2);
			if (((Capture)val).get_Length() <= 0)
			{
				return null;
			}
			return ((Capture)val).get_Value();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public string IsTextOffensive(StringBuilder sb)
		{
			if (m_blacklistRegEx == null)
			{
				return null;
			}
			return IsTextOffensive(sb.ToString());
		}

		public string FixOffensiveString(string text, string replacement = "***")
		{
			if (m_blacklistRegEx != null)
			{
				return m_blacklistRegEx.Replace(text, replacement);
			}
			return text;
		}
	}
}
