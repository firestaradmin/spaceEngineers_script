<<<<<<< HEAD
=======
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sandbox.Graphics;
using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Gui
{
	public class MyWikiMarkupParser
	{
		private static Regex m_splitRegex = new Regex("\\[.*?\\]{1,2}");

		private static Regex m_markupRegex = new Regex("(?<=\\[)(?!\\[).*?(?=\\])");

		private static Regex m_digitsRegex = new Regex("\\d+");

		private static StringBuilder m_stringCache = new StringBuilder();

		public static void ParseText(string text, ref MyGuiControlMultilineText label)
		{
			try
			{
				string[] array = m_splitRegex.Split(text);
				MatchCollection val = m_splitRegex.Matches(text);
				for (int i = 0; i < val.get_Count() || i < array.Length; i++)
				{
					if (i < array.Length)
					{
						label.AppendText(m_stringCache.Clear().Append(array[i]));
					}
					if (i < val.get_Count())
					{
						ParseMarkup(label, ((Capture)val.get_Item(i)).get_Value());
					}
				}
			}
			catch
			{
			}
		}

		private static void ParseMarkup(MyGuiControlMultilineText label, string markup)
		{
			Match val = m_markupRegex.Match(markup);
			if (Enumerable.Contains<char>((IEnumerable<char>)((Capture)val).get_Value(), '|'))
			{
<<<<<<< HEAD
				string[] array = match.Value.Substring(5).Split(new char[1] { '|' });
				MatchCollection matchCollection = m_digitsRegex.Matches(array[1]);
				if (int.TryParse(matchCollection[0].Value, out var result) && int.TryParse(matchCollection[1].Value, out var result2))
=======
				string[] array = ((Capture)val).get_Value().Substring(5).Split(new char[1] { '|' });
				MatchCollection val2 = m_digitsRegex.Matches(array[1]);
				if (int.TryParse(((Capture)val2.get_Item(0)).get_Value(), out var result) && int.TryParse(((Capture)val2.get_Item(1)).get_Value(), out var result2))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					label.AppendImage(array[0], MyGuiManager.GetNormalizedSizeFromScreenSize(new Vector2(result, result2)), Vector4.One);
				}
			}
			else
			{
				label.AppendLink(((Capture)val).get_Value().Substring(0, ((Capture)val).get_Value().IndexOf(' ')), ((Capture)val).get_Value().Substring(((Capture)val).get_Value().IndexOf(' ') + 1));
			}
		}
	}
}
