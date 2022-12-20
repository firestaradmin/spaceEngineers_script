using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Utils;

namespace VRage
{
	public static class MyTexts
	{
		public class MyLanguageDescription
		{
			public readonly MyLanguagesEnum Id;

			public readonly string Name;

			public readonly string CultureName;

			public readonly string SubcultureName;

			public readonly string FullCultureName;

			public readonly bool IsCommunityLocalized;

			public readonly float GuiTextScale;

			internal MyLanguageDescription(MyLanguagesEnum id, string displayName, string cultureName, string subcultureName, float guiTextScale, bool isCommunityLocalized)
			{
				Id = id;
				Name = displayName;
				CultureName = cultureName;
				SubcultureName = subcultureName;
				if (string.IsNullOrWhiteSpace(subcultureName))
				{
					FullCultureName = cultureName;
				}
				else
				{
					FullCultureName = $"{cultureName}-{subcultureName}";
				}
				IsCommunityLocalized = isCommunityLocalized;
				GuiTextScale = guiTextScale;
			}
		}

		private class MyGeneralEvaluator : ITextEvaluator
		{
			public string TokenEvaluate(string token, string context)
			{
				StringBuilder stringBuilder = Get(MyStringId.GetOrCompute(token));
				if (stringBuilder != null)
				{
					return stringBuilder.ToString();
				}
				return "";
			}
		}

		private static readonly string LOCALIZATION_TAG_GENERAL;

		public static readonly MyStringId GAMEPAD_VARIANT_ID;

		private static readonly Dictionary<MyLanguagesEnum, MyLanguageDescription> m_languageIdToLanguage;

		private static readonly Dictionary<string, MyLanguagesEnum> m_cultureToLanguageId;

		private static readonly bool m_checkMissingTexts;

		private static MyLocalizationPackage m_package;

		/// <summary>
		/// Current global localization variant selector.
		/// </summary>
		private static MyStringId m_selectedVariant;

		private static Regex m_textReplace;

		private static readonly Dictionary<string, ITextEvaluator> m_evaluators;

		private static MatchEvaluator m_ReplaceEvaluator;

		/// <summary>
		/// Global selector for translation variants.
		/// </summary>
		public static MyStringId GlobalVariantSelector => m_selectedVariant;

		public static DictionaryReader<MyLanguagesEnum, MyLanguageDescription> Languages => new DictionaryReader<MyLanguagesEnum, MyLanguageDescription>(m_languageIdToLanguage);

		static MyTexts()
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			LOCALIZATION_TAG_GENERAL = "LOCG";
			GAMEPAD_VARIANT_ID = MyStringId.GetOrCompute("Gamepad");
			m_languageIdToLanguage = new Dictionary<MyLanguagesEnum, MyLanguageDescription>();
			m_cultureToLanguageId = new Dictionary<string, MyLanguagesEnum>();
			m_checkMissingTexts = false;
			m_package = new MyLocalizationPackage();
			m_selectedVariant = MyStringId.NullOrEmpty;
			m_evaluators = new Dictionary<string, ITextEvaluator>();
<<<<<<< HEAD
			m_ReplaceEvaluator = ReplaceEvaluator;
=======
			m_ReplaceEvaluator = new MatchEvaluator(ReplaceEvaluator);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			AddLanguage(MyLanguagesEnum.English, "en", null, "English", 1f, isCommunityLocalized: false);
			AddLanguage(MyLanguagesEnum.Czech, "cs", "CZ", "Česky", 0.95f);
			AddLanguage(MyLanguagesEnum.Slovak, "sk", "SK", "Slovenčina", 0.95f);
			AddLanguage(MyLanguagesEnum.German, "de", null, "Deutsch", 1f, isCommunityLocalized: false);
			AddLanguage(MyLanguagesEnum.Russian, "ru", null, "Русский", 1f, isCommunityLocalized: false);
			AddLanguage(MyLanguagesEnum.Spanish_Spain, "es", null, "Español (España)", 1f, isCommunityLocalized: false);
			AddLanguage(MyLanguagesEnum.French, "fr", null, "Français", 1f, isCommunityLocalized: false);
<<<<<<< HEAD
			AddLanguage(MyLanguagesEnum.Italian, "it", null, "Italiano", 1f, isCommunityLocalized: false);
=======
			AddLanguage(MyLanguagesEnum.Italian, "it", null, "Italiano");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			AddLanguage(MyLanguagesEnum.Danish, "da", null, "Dansk");
			AddLanguage(MyLanguagesEnum.Dutch, "nl", null, "Nederlands");
			AddLanguage(MyLanguagesEnum.Icelandic, "is", "IS", "Íslenska");
			AddLanguage(MyLanguagesEnum.Polish, "pl", "PL", "Polski");
			AddLanguage(MyLanguagesEnum.Finnish, "fi", null, "Suomi");
			AddLanguage(MyLanguagesEnum.Hungarian, "hu", "HU", "Magyar", 0.85f);
			AddLanguage(MyLanguagesEnum.Portuguese_Brazil, "pt", "BR", "Português (Brasileiro)", 1f, isCommunityLocalized: false);
			AddLanguage(MyLanguagesEnum.Estonian, "et", "EE", "Eesti");
			AddLanguage(MyLanguagesEnum.Norwegian, "no", null, "Norsk");
			AddLanguage(MyLanguagesEnum.Spanish_HispanicAmerica, "es", "419", "Español (Latinoamerica)");
			AddLanguage(MyLanguagesEnum.Swedish, "sv", null, "Svenska", 0.9f);
			AddLanguage(MyLanguagesEnum.Catalan, "ca", "AD", "Català", 0.85f);
			AddLanguage(MyLanguagesEnum.Croatian, "hr", "HR", "Hrvatski", 0.9f);
			AddLanguage(MyLanguagesEnum.Romanian, "ro", null, "Română", 0.85f);
			AddLanguage(MyLanguagesEnum.Ukrainian, "uk", null, "Українська");
			AddLanguage(MyLanguagesEnum.Turkish, "tr", "TR", "Türkçe");
			AddLanguage(MyLanguagesEnum.Latvian, "lv", null, "Latviešu", 0.87f);
			AddLanguage(MyLanguagesEnum.ChineseChina, "zh", "CN", "Chinese", 1f, isCommunityLocalized: false);
			RegisterEvaluator(LOCALIZATION_TAG_GENERAL, new MyGeneralEvaluator());
		}

		private static void AddLanguage(MyLanguagesEnum id, string cultureName, string subcultureName = null, string displayName = null, float guiTextScale = 1f, bool isCommunityLocalized = true)
		{
			MyLanguageDescription myLanguageDescription = new MyLanguageDescription(id, displayName, cultureName, subcultureName, guiTextScale, isCommunityLocalized);
			m_languageIdToLanguage.Add(id, myLanguageDescription);
			m_cultureToLanguageId.Add(myLanguageDescription.FullCultureName, id);
		}

		public static MyLanguagesEnum GetBestSuitableLanguage(string culture)
		{
			MyLanguagesEnum value = MyLanguagesEnum.English;
			if (!m_cultureToLanguageId.TryGetValue(culture, out value))
			{
				string[] array = culture.Split(new char[1] { '-' });
				string text = array[0];
				_ = array[1];
				{
					foreach (KeyValuePair<MyLanguagesEnum, MyLanguageDescription> item in m_languageIdToLanguage)
					{
						if (item.Value.FullCultureName == text)
						{
							return item.Key;
						}
					}
					return value;
				}
			}
			return value;
		}

		public static string GetSystemLanguage()
		{
			return CultureInfo.InstalledUICulture.Name;
		}

		public static void LoadSupportedLanguages(string rootDirectory, HashSet<MyLanguagesEnum> outSupportedLanguages)
		{
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			outSupportedLanguages.Add(MyLanguagesEnum.English);
			IEnumerable<string> files = MyFileSystem.GetFiles(rootDirectory, "*.resx", MySearchOption.TopDirectoryOnly);
			HashSet<string> val = new HashSet<string>();
			foreach (string item in files)
			{
				string[] array = Path.GetFileNameWithoutExtension(item).Split(new char[1] { '.' });
				if (array.Length > 1)
				{
					val.Add(array[1]);
				}
			}
			Enumerator<string> enumerator2 = val.GetEnumerator();
			try
			{
<<<<<<< HEAD
				if (m_cultureToLanguageId.TryGetValue(item2, out var value))
=======
				while (enumerator2.MoveNext())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					string current = enumerator2.get_Current();
					if (m_cultureToLanguageId.TryGetValue(current, out var value))
					{
						outSupportedLanguages.Add(value);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
		}

		/// <summary>
		/// Set the global variant to be selected for each translation.
		/// </summary>
		/// <param name="variantName"></param>
		public static void SetGlobalVariantSelector(MyStringId variantName)
		{
			m_selectedVariant = variantName;
		}

		public static string SubstituteTexts(string text, string context = null)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			if (text != null)
			{
				return m_textReplace.Replace(text, (MatchEvaluator)((Match match) => ReplaceEvaluator(match, context)));
			}
			return null;
		}

		public static StringBuilder SubstituteTexts(StringBuilder text)
		{
			if (text == null)
			{
				return null;
			}
			string text2 = text.ToString();
			string text3 = m_textReplace.Replace(text2, m_ReplaceEvaluator);
			if (!(text2 == text3))
			{
				return new StringBuilder(text3);
			}
			return text;
		}

		public static StringBuilder Get(MyStringId id)
		{
			if (!m_package.TryGetStringBuilder(id, m_selectedVariant, out var messageSb))
			{
				messageSb = ((!m_checkMissingTexts) ? new StringBuilder(id.ToString()) : new StringBuilder("X_" + id.ToString()));
			}
			if (m_checkMissingTexts)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("T_");
				messageSb = stringBuilder.Append((object)messageSb);
			}
			string text = messageSb.ToString();
			string text2 = m_textReplace.Replace(text, m_ReplaceEvaluator);
			if (!(text == text2))
			{
				return new StringBuilder(text2);
			}
			return messageSb;
		}

		public static string TrySubstitute(string input)
		{
			MyStringId orCompute = MyStringId.GetOrCompute(input);
			if (!m_package.TryGet(orCompute, m_selectedVariant, out var message))
			{
				return input;
			}
			return m_textReplace.Replace(message, m_ReplaceEvaluator);
		}

		public static void RegisterEvaluator(string prefix, ITextEvaluator eval)
		{
			m_evaluators.Add(prefix, eval);
			InitReplace();
		}

		private static void InitReplace()
		{
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Expected O, but got Unknown
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			stringBuilder.Append("{(");
			foreach (KeyValuePair<string, ITextEvaluator> evaluator in m_evaluators)
			{
				if (num != 0)
				{
					stringBuilder.Append("|");
				}
				stringBuilder.AppendFormat(evaluator.Key);
				num++;
			}
			stringBuilder.Append("):((?:\\w|:)*)}");
			m_textReplace = new Regex(stringBuilder.ToString());
		}

		private static string ReplaceEvaluator(Match match)
		{
			return ReplaceEvaluator(match, null);
		}

		private static string ReplaceEvaluator(Match match, string context)
		{
			if (match.get_Groups().get_Count() != 3)
			{
				return string.Empty;
			}
<<<<<<< HEAD
			if (m_evaluators.TryGetValue(match.Groups[1].Value, out var value))
=======
			if (m_evaluators.TryGetValue(((Capture)match.get_Groups().get_Item(1)).get_Value(), out var value))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return value.TokenEvaluate(((Capture)match.get_Groups().get_Item(2)).get_Value(), context);
			}
			return string.Empty;
		}

		public static bool MatchesReplaceFormat(string str)
		{
<<<<<<< HEAD
			return m_textReplace?.IsMatch(str) ?? false;
=======
			Regex textReplace = m_textReplace;
			if (textReplace == null)
			{
				return false;
			}
			return textReplace.IsMatch(str);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static string GetString(MyStringId id)
		{
			if (!m_package.TryGet(id, m_selectedVariant, out var message))
			{
				message = ((!m_checkMissingTexts) ? id.ToString() : ("X_" + id.ToString()));
			}
			if (m_checkMissingTexts)
			{
				message = "T_" + message;
			}
			return m_textReplace.Replace(message, m_ReplaceEvaluator);
		}

		public static string GetString(string keyString)
		{
			return GetString(MyStringId.GetOrCompute(keyString));
		}

		public static bool Exists(MyStringId id)
		{
			return m_package.ContainsKey(id);
		}

		public static void Clear()
		{
			m_package.Clear();
			m_package.AddMessage("", "");
		}

		private static string GetPathWithFile(string file, List<string> allFiles)
		{
			foreach (string allFile in allFiles)
			{
				if (allFile.Contains(file))
				{
					return allFile;
				}
			}
			return null;
		}

		public static bool IsTagged(string text, int position, string tag)
		{
			for (int i = 0; i < tag.Length; i++)
			{
				if (text[position + i] != tag[i])
				{
					return false;
				}
			}
			return true;
		}

		public static void LoadTexts(string rootDirectory, string cultureName = null, string subcultureName = null)
		{
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0287: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			HashSet<string> val = new HashSet<string>();
			HashSet<string> val2 = new HashSet<string>();
			HashSet<string> val3 = new HashSet<string>();
			IEnumerable<string> files = MyFileSystem.GetFiles(rootDirectory, "*.resx", MySearchOption.AllDirectories);
			List<string> list = new List<string>();
			foreach (string item in files)
			{
				if (item.Contains("MyCommonTexts"))
				{
<<<<<<< HEAD
					hashSet.Add(Path.GetFileNameWithoutExtension(item).Split(new char[1] { '.' })[0]);
				}
				else if (item.Contains("MyTexts"))
				{
					hashSet2.Add(Path.GetFileNameWithoutExtension(item).Split(new char[1] { '.' })[0]);
=======
					val.Add(Path.GetFileNameWithoutExtension(item).Split(new char[1] { '.' })[0]);
				}
				else if (item.Contains("MyTexts"))
				{
					val2.Add(Path.GetFileNameWithoutExtension(item).Split(new char[1] { '.' })[0]);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				else
				{
					if (!item.Contains("MyCoreTexts"))
					{
						continue;
					}
<<<<<<< HEAD
					hashSet3.Add(Path.GetFileNameWithoutExtension(item).Split(new char[1] { '.' })[0]);
=======
					val3.Add(Path.GetFileNameWithoutExtension(item).Split(new char[1] { '.' })[0]);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				list.Add(item);
			}
			Enumerator<string> enumerator2 = val.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					string current2 = enumerator2.get_Current();
					PatchTexts(GetPathWithFile($"{current2}.resx", list));
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
			enumerator2 = val2.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					string current3 = enumerator2.get_Current();
					PatchTexts(GetPathWithFile($"{current3}.resx", list));
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
<<<<<<< HEAD
			if (cultureName == null)
			{
				return;
			}
			foreach (string item5 in hashSet)
			{
				PatchTexts(GetPathWithFile($"{item5}.{cultureName}.resx", list));
			}
			foreach (string item6 in hashSet2)
			{
				PatchTexts(GetPathWithFile($"{item6}.{cultureName}.resx", list));
			}
			foreach (string item7 in hashSet3)
			{
				PatchTexts(GetPathWithFile($"{item7}.{cultureName}.resx", list));
=======
			enumerator2 = val3.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					string current4 = enumerator2.get_Current();
					PatchTexts(GetPathWithFile($"{current4}.resx", list));
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
			if (cultureName == null)
			{
				return;
			}
			enumerator2 = val.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					string current5 = enumerator2.get_Current();
					PatchTexts(GetPathWithFile($"{current5}.{cultureName}.resx", list));
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
			enumerator2 = val2.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					string current6 = enumerator2.get_Current();
					PatchTexts(GetPathWithFile($"{current6}.{cultureName}.resx", list));
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
			enumerator2 = val3.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					string current7 = enumerator2.get_Current();
					PatchTexts(GetPathWithFile($"{current7}.{cultureName}.resx", list));
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (subcultureName == null)
			{
				return;
			}
<<<<<<< HEAD
			foreach (string item8 in hashSet)
			{
				PatchTexts(GetPathWithFile($"{item8}.{cultureName}-{subcultureName}.resx", list));
			}
			foreach (string item9 in hashSet2)
			{
				PatchTexts(GetPathWithFile($"{item9}.{cultureName}-{subcultureName}.resx", list));
			}
			foreach (string item10 in hashSet3)
			{
				PatchTexts(GetPathWithFile($"{item10}.{cultureName}-{subcultureName}.resx", list));
=======
			enumerator2 = val.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					string current8 = enumerator2.get_Current();
					PatchTexts(GetPathWithFile($"{current8}.{cultureName}-{subcultureName}.resx", list));
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
			enumerator2 = val2.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					string current9 = enumerator2.get_Current();
					PatchTexts(GetPathWithFile($"{current9}.{cultureName}-{subcultureName}.resx", list));
				}
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
			enumerator2 = val3.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					string current10 = enumerator2.get_Current();
					PatchTexts(GetPathWithFile($"{current10}.{cultureName}-{subcultureName}.resx", list));
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			finally
			{
				((IDisposable)enumerator2).Dispose();
			}
		}

		private static void PatchTexts(string resourceFile)
		{
<<<<<<< HEAD
=======
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Expected O, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!MyFileSystem.FileExists(resourceFile))
			{
				return;
			}
<<<<<<< HEAD
			using (Stream inStream = MyFileSystem.OpenRead(resourceFile))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(inStream);
				foreach (XmlNode item in xmlDocument.SelectNodes("/root/data"))
				{
					string value = item.Attributes["name"].Value;
					string text = null;
					foreach (XmlNode childNode in item.ChildNodes)
					{
						if (childNode.Name.Equals("value", StringComparison.InvariantCultureIgnoreCase))
						{
							XmlNodeReader xmlNodeReader = new XmlNodeReader(childNode);
							if (xmlNodeReader.Read())
							{
								text = xmlNodeReader.ReadString();
							}
						}
					}
					if (!string.IsNullOrEmpty(value) && text != null)
					{
						m_package.AddMessage(value, text, overwrite: true);
=======
			using Stream stream = MyFileSystem.OpenRead(resourceFile);
			XmlDocument val = new XmlDocument();
			val.Load(stream);
			foreach (XmlNode item in ((XmlNode)val).SelectNodes("/root/data"))
			{
				string value = ((XmlNode)item.get_Attributes().get_ItemOf("name")).get_Value();
				string text = null;
				foreach (XmlNode childNode in item.get_ChildNodes())
				{
					XmlNode val3 = childNode;
					if (val3.get_Name().Equals("value", StringComparison.InvariantCultureIgnoreCase))
					{
						XmlNodeReader val4 = new XmlNodeReader(val3);
						if (((XmlReader)val4).Read())
						{
							text = ((XmlReader)val4).ReadString();
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				if (!string.IsNullOrEmpty(value) && text != null)
				{
					m_package.AddMessage(value, text, overwrite: true);
				}
			}
		}

		public static StringBuilder AppendFormat(this StringBuilder stringBuilder, MyStringId textEnum, object arg0)
		{
			return stringBuilder.AppendFormat(GetString(textEnum), arg0);
		}

		public static StringBuilder AppendFormat(this StringBuilder stringBuilder, MyStringId textEnum, params object[] arg)
		{
			return stringBuilder.AppendFormat(GetString(textEnum), arg);
		}

		public static StringBuilder AppendFormat(this StringBuilder stringBuilder, MyStringId textEnum, MyStringId arg0)
		{
			return stringBuilder.AppendFormat(GetString(textEnum), GetString(arg0));
		}
	}
}
