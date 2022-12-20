using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using Sandbox.Graphics;
using VRage;
using VRage.Collections;
using VRage.FileSystem;
using VRage.Game.Localization;
using VRage.Utils;

namespace Sandbox.Game.Localization
{
	public static class MyLanguage
	{
		private static string m_actualCultureName;

		private static MyLanguagesEnum m_actualLanguage;

		private static HashSet<MyLanguagesEnum> m_supportedLanguages = new HashSet<MyLanguagesEnum>();

		private static string m_currentOSCultureName;

		public static HashSetReader<MyLanguagesEnum> SupportedLanguages => m_supportedLanguages;

		public static MyLanguagesEnum CurrentLanguage
		{
			get
			{
				return m_actualLanguage;
			}
			set
			{
				LoadLanguage(value);
				if (MySandboxGame.Config.Language != m_actualLanguage)
				{
					MySandboxGame.Config.Language = m_actualLanguage;
					MySandboxGame.Config.Save();
				}
				MyGuiManager.CurrentLanguage = m_actualLanguage;
				m_actualCultureName = ConvertLangEnum(m_actualLanguage);
				MyLocalization.Static.Switch(m_actualCultureName);
			}
		}

		public static string CurrentCultureName
		{
			get
			{
				return m_actualCultureName ?? CultureInfo.CurrentCulture.Name;
			}
			set
			{
				m_actualCultureName = value;
			}
		}

		public static void Init()
		{
			MyTexts.LoadSupportedLanguages(GetLocalizationPath(), m_supportedLanguages);
			LoadLanguage(MyLanguagesEnum.English);
		}

		private static void LoadLanguage(MyLanguagesEnum value)
		{
			MyTexts.MyLanguageDescription myLanguageDescription = MyTexts.Languages[value];
			MyTexts.Clear();
			MyTexts.LoadTexts(GetLocalizationPath(), myLanguageDescription.CultureName, myLanguageDescription.SubcultureName);
			MyGuiManager.LanguageTextScale = myLanguageDescription.GuiTextScale;
			m_actualLanguage = value;
			m_actualCultureName = ConvertLangEnum(value);
		}

		private static string GetLocalizationPath()
		{
			return Path.Combine(MyFileSystem.ContentPath, "Data", "Localization");
		}

		[Conditional("DEBUG")]
		private static void GenerateCurrentLanguageCharTable()
		{
			SortedSet<char> val = new SortedSet<char>();
			foreach (MyStringId enumValue in typeof(MyStringId).GetEnumValues())
			{
				StringBuilder stringBuilder = MyTexts.Get(enumValue);
				for (int i = 0; i < stringBuilder.Length; i++)
				{
					val.Add(stringBuilder[i]);
				}
			}
			List<char> list = new List<char>((IEnumerable<char>)val);
			string userDataPath = MyFileSystem.UserDataPath;
			string path = $"character-table-{CurrentLanguage}.txt";
			using (StreamWriter streamWriter = new StreamWriter(Path.Combine(userDataPath, path)))
			{
				foreach (char item in list)
				{
					streamWriter.WriteLine($"{item}\t{(int)item:x4}");
				}
			}
			string path2 = $"character-ranges-{CurrentLanguage}.txt";
			using StreamWriter streamWriter2 = new StreamWriter(Path.Combine(userDataPath, path2));
			int j = 0;
			while (j < list.Count)
			{
				int num;
				int num2 = (num = list[j]);
				for (j++; j < list.Count && list[j] == num2 + 1; j++)
				{
<<<<<<< HEAD
					int num;
					int num2 = (num = list[j]);
					for (j++; j < list.Count && list[j] == num2 + 1; j++)
					{
						num2 = list[j];
					}
					streamWriter2.WriteLine($"-range {num:x4}-{num2:x4}");
=======
					num2 = list[j];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				streamWriter2.WriteLine($"-range {num:x4}-{num2:x4}");
			}
		}

		public static MyLanguagesEnum GetOsLanguageCurrent()
		{
			return ConvertLangEnum(m_currentOSCultureName);
		}

		public static MyLanguagesEnum GetOsLanguageCurrentOfficial()
		{
			MyLanguagesEnum myLanguagesEnum = ConvertLangEnum(m_currentOSCultureName);
			MyTexts.Languages.TryGetValue(myLanguagesEnum, out var value);
			if (value == null || value.IsCommunityLocalized)
			{
				return MyLanguagesEnum.English;
			}
			return myLanguagesEnum;
		}

		public static MyLanguagesEnum ConvertLangEnum(string cultureName)
		{
			if (cultureName == null)
			{
				cultureName = CultureInfo.CurrentCulture.Name;
			}
			return cultureName switch
			{
<<<<<<< HEAD
			case "en-US":
				return MyLanguagesEnum.English;
			case "cs-CZ":
				return MyLanguagesEnum.Czech;
			case "sk-SK":
				return MyLanguagesEnum.Slovak;
			case "de-DE":
				return MyLanguagesEnum.German;
			case "ru-RU":
				return MyLanguagesEnum.Russian;
			case "es-ES":
				return MyLanguagesEnum.Spanish_Spain;
			case "fr-FR":
				return MyLanguagesEnum.French;
			case "it-IT":
				return MyLanguagesEnum.Italian;
			case "da-DK":
				return MyLanguagesEnum.Danish;
			case "nl-NL":
				return MyLanguagesEnum.Dutch;
			case "is-IS":
				return MyLanguagesEnum.Icelandic;
			case "pl-PL":
				return MyLanguagesEnum.Polish;
			case "fi-FI":
				return MyLanguagesEnum.Finnish;
			case "hu-HU":
				return MyLanguagesEnum.Hungarian;
			case "pt-BR":
				return MyLanguagesEnum.Portuguese_Brazil;
			case "et-EE":
				return MyLanguagesEnum.Estonian;
			case "nb-NO":
				return MyLanguagesEnum.Norwegian;
			case "es":
				return MyLanguagesEnum.Spanish_HispanicAmerica;
			case "sv-SE":
				return MyLanguagesEnum.Swedish;
			case "ca-ES":
				return MyLanguagesEnum.Catalan;
			case "hr-HR":
				return MyLanguagesEnum.Croatian;
			case "ro-RO":
				return MyLanguagesEnum.Romanian;
			case "uk-UA":
				return MyLanguagesEnum.Ukrainian;
			case "tr-TR":
				return MyLanguagesEnum.Turkish;
			case "lv-LV":
				return MyLanguagesEnum.Latvian;
			case "zh-CN":
				return MyLanguagesEnum.ChineseChina;
			case "ja-JP":
				return MyLanguagesEnum.Japanese;
			default:
				return MyLanguagesEnum.English;
			}
=======
				"en-US" => MyLanguagesEnum.English, 
				"cs-CZ" => MyLanguagesEnum.Czech, 
				"sk-SK" => MyLanguagesEnum.Slovak, 
				"de-DE" => MyLanguagesEnum.German, 
				"ru-RU" => MyLanguagesEnum.Russian, 
				"es-ES" => MyLanguagesEnum.Spanish_Spain, 
				"fr-FR" => MyLanguagesEnum.French, 
				"it-IT" => MyLanguagesEnum.Italian, 
				"da-DK" => MyLanguagesEnum.Danish, 
				"nl-NL" => MyLanguagesEnum.Dutch, 
				"is-IS" => MyLanguagesEnum.Icelandic, 
				"pl-PL" => MyLanguagesEnum.Polish, 
				"fi-FI" => MyLanguagesEnum.Finnish, 
				"hu-HU" => MyLanguagesEnum.Hungarian, 
				"pt-BR" => MyLanguagesEnum.Portuguese_Brazil, 
				"et-EE" => MyLanguagesEnum.Estonian, 
				"nb-NO" => MyLanguagesEnum.Norwegian, 
				"es" => MyLanguagesEnum.Spanish_HispanicAmerica, 
				"sv-SE" => MyLanguagesEnum.Swedish, 
				"ca-ES" => MyLanguagesEnum.Catalan, 
				"hr-HR" => MyLanguagesEnum.Croatian, 
				"ro-RO" => MyLanguagesEnum.Romanian, 
				"uk-UA" => MyLanguagesEnum.Ukrainian, 
				"tr-TR" => MyLanguagesEnum.Turkish, 
				"lv-LV" => MyLanguagesEnum.Latvian, 
				"zh-CN" => MyLanguagesEnum.ChineseChina, 
				_ => MyLanguagesEnum.English, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static string ConvertLangEnum(MyLanguagesEnum enumVal)
		{
			return enumVal switch
			{
<<<<<<< HEAD
			case MyLanguagesEnum.English:
				return "en-US";
			case MyLanguagesEnum.Czech:
				return "cs-CZ";
			case MyLanguagesEnum.Slovak:
				return "sk-SK";
			case MyLanguagesEnum.German:
				return "de-DE";
			case MyLanguagesEnum.Russian:
				return "ru-RU";
			case MyLanguagesEnum.Spanish_Spain:
				return "es-ES";
			case MyLanguagesEnum.French:
				return "fr-FR";
			case MyLanguagesEnum.Italian:
				return "it-IT";
			case MyLanguagesEnum.Danish:
				return "da-DK";
			case MyLanguagesEnum.Dutch:
				return "nl-NL";
			case MyLanguagesEnum.Icelandic:
				return "is-IS";
			case MyLanguagesEnum.Polish:
				return "pl-PL";
			case MyLanguagesEnum.Finnish:
				return "fi-FI";
			case MyLanguagesEnum.Hungarian:
				return "hu-HU";
			case MyLanguagesEnum.Portuguese_Brazil:
				return "pt-BR";
			case MyLanguagesEnum.Estonian:
				return "et-EE";
			case MyLanguagesEnum.Norwegian:
				return "nb-NO";
			case MyLanguagesEnum.Spanish_HispanicAmerica:
				return "es";
			case MyLanguagesEnum.Swedish:
				return "sv-SE";
			case MyLanguagesEnum.Catalan:
				return "ca-ES";
			case MyLanguagesEnum.Croatian:
				return "hr-HR";
			case MyLanguagesEnum.Romanian:
				return "ro-RO";
			case MyLanguagesEnum.Ukrainian:
				return "uk-UA";
			case MyLanguagesEnum.Turkish:
				return "tr-TR";
			case MyLanguagesEnum.Latvian:
				return "lv-LV";
			case MyLanguagesEnum.ChineseChina:
				return "zh-CN";
			case MyLanguagesEnum.Japanese:
				return "ja-JP";
			default:
				return "en-US";
			}
=======
				MyLanguagesEnum.English => "en-US", 
				MyLanguagesEnum.Czech => "cs-CZ", 
				MyLanguagesEnum.Slovak => "sk-SK", 
				MyLanguagesEnum.German => "de-DE", 
				MyLanguagesEnum.Russian => "ru-RU", 
				MyLanguagesEnum.Spanish_Spain => "es-ES", 
				MyLanguagesEnum.French => "fr-FR", 
				MyLanguagesEnum.Italian => "it-IT", 
				MyLanguagesEnum.Danish => "da-DK", 
				MyLanguagesEnum.Dutch => "nl-NL", 
				MyLanguagesEnum.Icelandic => "is-IS", 
				MyLanguagesEnum.Polish => "pl-PL", 
				MyLanguagesEnum.Finnish => "fi-FI", 
				MyLanguagesEnum.Hungarian => "hu-HU", 
				MyLanguagesEnum.Portuguese_Brazil => "pt-BR", 
				MyLanguagesEnum.Estonian => "et-EE", 
				MyLanguagesEnum.Norwegian => "nb-NO", 
				MyLanguagesEnum.Spanish_HispanicAmerica => "es", 
				MyLanguagesEnum.Swedish => "sv-SE", 
				MyLanguagesEnum.Catalan => "ca-ES", 
				MyLanguagesEnum.Croatian => "hr-HR", 
				MyLanguagesEnum.Romanian => "ro-RO", 
				MyLanguagesEnum.Ukrainian => "uk-UA", 
				MyLanguagesEnum.Turkish => "tr-TR", 
				MyLanguagesEnum.Latvian => "lv-LV", 
				MyLanguagesEnum.ChineseChina => "zh-CN", 
				_ => "en-US", 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static void ObtainCurrentOSCulture()
		{
			m_currentOSCultureName = CultureInfo.CurrentCulture.Name;
		}

		public static string GetCurrentOSCulture()
		{
			return m_currentOSCultureName;
		}
	}
}
