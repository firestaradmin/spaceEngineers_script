using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game.GameSystems.BankingAndCurrency;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Definitions.SessionComponents;
using VRage.Game.Factions.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Library.Collections;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.World.Generator
{
	internal class MyFactionGenerator
	{
		private class MyNameCollection
		{
			public MyList<string> First = new MyList<string>();

			public MyList<string> FirstTag = new MyList<string>();

			public MyList<string> Miner = new MyList<string>();

			public MyList<string> MinerTag = new MyList<string>();

			public MyList<string> Trader = new MyList<string>();

			public MyList<string> TraderTag = new MyList<string>();

			public MyList<string> Builder = new MyList<string>();

			public MyList<string> BuilderTag = new MyList<string>();
		}

		private static readonly int MAX_FACTION_COUNT = 100;

		private static readonly string DESCRIPTION_KEY_MINER = "EconomyFaction_Description_Miner";

		private static readonly string DESCRIPTION_KEY_TRADER = "EconomyFaction_Description_Trader";

		private static readonly string DESCRIPTION_KEY_BUILDER = "EconomyFaction_Description_Builder";

		private static readonly int DESCRIPTION_VARIANTS_MINER = 5;

		private static readonly int DESCRIPTION_VARIANTS_TRADER = 5;

		private static readonly int DESCRIPTION_VARIANTS_BUILDER = 5;

		private static readonly Vector3 DEFAULT_ICON_COLOR = new Vector3(224f / 255f, 224f / 255f, 224f / 255f);

		public bool GenerateFactions(MySessionComponentEconomyDefinition def)
		{
			if (!Sync.IsServer)
			{
				return false;
			}
			if (MySession.Static == null)
			{
				return false;
			}
			int num = MySession.Static.Settings.TradeFactionsCount;
			if (num > MAX_FACTION_COUNT)
			{
				num = MAX_FACTION_COUNT;
			}
			if (num == 0)
			{
				return false;
			}
			_ = MySession.Static.Factions;
			float num2 = def.FactionRatio_Miners;
			float num3 = def.FactionRatio_Traders;
			float num4 = def.FactionRatio_Builders;
			float num5 = num2 + num3 + num4;
			float num6 = (float)num / num5;
			num2 *= num6;
			int num7 = (int)num2;
			num2 -= (float)num7;
			num3 *= num6;
			int num8 = (int)num3;
			num3 -= (float)num8;
			num4 *= num6;
			int num9 = (int)num4;
			num4 -= (float)num9;
			int num10 = num - (num7 + num8 + num9);
			for (int i = 0; i < num10; i++)
			{
				if (num2 > num3)
				{
					if (num2 > num4)
					{
						num7++;
						num2 -= 1f;
					}
					else
					{
						num9++;
						num4 -= 1f;
					}
				}
				else if (num3 > num4)
				{
					num8++;
					num3 -= 1f;
				}
				else
				{
					num9++;
					num4 -= 1f;
				}
			}
			MyNameCollection namePrecursors = GetNamePrecursors();
			List<Tuple<string, string>> namesM = new List<Tuple<string, string>>();
			List<Tuple<string, string>> namesT = new List<Tuple<string, string>>();
			List<Tuple<string, string>> namesB = new List<Tuple<string, string>>();
			GenerateNamesAll(namePrecursors, num7, num8, num9, out namesM, out namesT, out namesB);
			int num11 = 0;
			int num12 = 0;
			MyFactionIconsDefinition definition = MyDefinitionManager.Static.GetDefinition<MyFactionIconsDefinition>("Miner");
			List<Vector3> range = definition.BackgroundColorRanges.GetRange(0, definition.BackgroundColorRanges.Count);
			num12 = num7 - range.Count;
			while (num12 > 0)
			{
				int num13 = Math.Min(definition.BackgroundColorRanges.Count, num12);
				range.AddRange(definition.BackgroundColorRanges.GetRange(0, num13));
				num12 -= num13;
			}
			for (int j = 0; j < num7; j++)
			{
				string @string = MyTexts.GetString(DESCRIPTION_KEY_MINER + MyRandom.Instance.Next(0, DESCRIPTION_VARIANTS_MINER));
				if (string.IsNullOrEmpty(@string))
				{
					@string = MyTexts.GetString(DESCRIPTION_KEY_MINER);
				}
				GetRandomFactionColorAndIcon(range, definition.Icons, out var color, out var iconColor, out var factionIcon);
				BuildFaction(namesM[j].Item1, @string, namesM[j].Item2, string.Format(MyTexts.GetString(MySpaceTexts.Economy_FactionLeader_Formated), namesM[j].Item1), MyFactionTypes.Miner, def.PerFactionInitialCurrency, color, iconColor, factionIcon);
				num11++;
			}
			definition = MyDefinitionManager.Static.GetDefinition<MyFactionIconsDefinition>("Trader");
			range = definition.BackgroundColorRanges.GetRange(0, definition.BackgroundColorRanges.Count);
			num12 = num8 - range.Count;
			while (num12 > 0)
			{
				int num14 = Math.Min(definition.BackgroundColorRanges.Count, num12);
				range.AddRange(definition.BackgroundColorRanges.GetRange(0, num14));
				num12 -= num14;
			}
			for (int k = 0; k < num8; k++)
			{
				string string2 = MyTexts.GetString(DESCRIPTION_KEY_TRADER + MyRandom.Instance.Next(0, DESCRIPTION_VARIANTS_TRADER));
				if (string.IsNullOrEmpty(string2))
				{
					string2 = MyTexts.GetString(DESCRIPTION_KEY_TRADER);
				}
				GetRandomFactionColorAndIcon(range, definition.Icons, out var color2, out var iconColor2, out var factionIcon2);
				BuildFaction(namesT[k].Item1, string2, namesT[k].Item2, namesT[k].Item1 + " CEO", MyFactionTypes.Trader, def.PerFactionInitialCurrency, color2, iconColor2, factionIcon2);
				num11++;
			}
			definition = MyDefinitionManager.Static.GetDefinition<MyFactionIconsDefinition>("Builder");
			range = definition.BackgroundColorRanges.GetRange(0, definition.BackgroundColorRanges.Count);
			num12 = num9 - range.Count;
			while (num12 > 0)
			{
				int num15 = Math.Min(definition.BackgroundColorRanges.Count, num12);
				range.AddRange(definition.BackgroundColorRanges.GetRange(0, num15));
				num12 -= num15;
			}
			for (int l = 0; l < num9; l++)
			{
				string string3 = MyTexts.GetString(DESCRIPTION_KEY_BUILDER + MyRandom.Instance.Next(0, DESCRIPTION_VARIANTS_BUILDER));
				if (string.IsNullOrEmpty(string3))
				{
					string3 = MyTexts.GetString(DESCRIPTION_KEY_BUILDER);
				}
				GetRandomFactionColorAndIcon(range, definition.Icons, out var color3, out var iconColor3, out var factionIcon3);
				BuildFaction(namesB[l].Item1, string3, namesB[l].Item2, namesB[l].Item1 + " CEO", MyFactionTypes.Builder, def.PerFactionInitialCurrency, color3, iconColor3, factionIcon3);
				num11++;
			}
			return true;
		}

		private static void GetRandomFactionColorAndIcon(List<Vector3> colorList, string[] icons, out Vector3 color, out Vector3 iconColor, out MyStringId factionIcon)
		{
			int index = MyRandom.Instance.Next(0, colorList.Count);
			color = colorList[index];
			colorList.RemoveAtFast(index);
			int num = MyRandom.Instance.Next(0, icons.Length);
			factionIcon = MyStringId.GetOrCompute(icons[num]);
			iconColor = MyColorPickerConstants.HSVToHSVOffset(ColorExtensions.ColorToHSV(DEFAULT_ICON_COLOR));
		}

		private Tuple<long, long> BuildFaction(string name, string description, string tag, string founderName, MyFactionTypes type, long initialCurrency, Vector3 color, Vector3 iconColor, MyStringId factionIcon)
		{
			MyFactionDefinition myFactionDefinition = new MyFactionDefinition();
			myFactionDefinition.Tag = tag;
			myFactionDefinition.DisplayNameString = name;
			myFactionDefinition.DescriptionString = description;
			myFactionDefinition.AutoAcceptMember = false;
			myFactionDefinition.AcceptHumans = false;
			myFactionDefinition.EnableFriendlyFire = false;
			myFactionDefinition.Type = type;
			myFactionDefinition.FactionIcon = factionIcon;
			MyIdentity myIdentity = Sync.Players.CreateNewIdentity(founderName, null, null, initialPlayer: false, addToNpcs: true);
			if (myIdentity == null)
			{
				return null;
			}
			long num = MyEntityIdentifier.AllocateId(MyEntityIdentifier.ID_OBJECT_TYPE.FACTION);
			if (!MyFactionCollection.CreateFactionInternal(myIdentity.IdentityId, num, myFactionDefinition, color, iconColor))
			{
				Sync.Players.RemoveIdentity(myIdentity.IdentityId);
				return null;
			}
			MyBankingSystem.Static.CreateAccount(num, initialCurrency);
			MyFactionCollection.FactionCreationFinished(num, myIdentity.IdentityId, tag, founderName, string.Empty, string.Empty, createFromDef: false, type);
			return new Tuple<long, long>(myIdentity.IdentityId, num);
		}

		private Tuple<string, string> GenerateName(MyNameCollection namePrecursors, MyFactionTypes type, bool randomFirst = false)
		{
			string text;
			string text2;
			if (!randomFirst)
			{
				int index = MyRandom.Instance.Next(0, namePrecursors.First.Count);
				text = namePrecursors.First[index];
				text2 = namePrecursors.FirstTag[index];
			}
			else
			{
				text = new string(new char[3]
				{
					(char)(65 + MyRandom.Instance.Next(0, 26)),
					(char)(65 + MyRandom.Instance.Next(0, 26)),
					(char)(65 + MyRandom.Instance.Next(0, 26))
				});
				text2 = text;
			}
			MyList<string> myList = type switch
			{
				MyFactionTypes.Trader => namePrecursors.Trader, 
				MyFactionTypes.Miner => namePrecursors.Miner, 
				_ => namePrecursors.Builder, 
			};
			MyList<string> myList2 = type switch
			{
<<<<<<< HEAD
			default:
				myList3 = namePrecursors.BuilderTag;
				break;
			case MyFactionTypes.Trader:
				myList3 = namePrecursors.TraderTag;
				break;
			case MyFactionTypes.Miner:
				myList3 = namePrecursors.MinerTag;
				break;
			}
			MyList<string> myList4 = myList3;
			int index2 = MyRandom.Instance.Next(0, myList2.Count);
			string arg = myList2[index2];
			string text3 = (randomFirst ? myList4[index2].Substring(0, 1) : myList4[index2]);
=======
				MyFactionTypes.Trader => namePrecursors.TraderTag, 
				MyFactionTypes.Miner => namePrecursors.MinerTag, 
				_ => namePrecursors.BuilderTag, 
			};
			int index2 = MyRandom.Instance.Next(0, myList.Count);
			string arg = myList[index2];
			string text3 = (randomFirst ? myList2[index2].Substring(0, 1) : myList2[index2]);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			return new Tuple<string, string>($"{text} {arg}", text2 + text3);
		}

		private bool GenerateNamesAll(MyNameCollection precursors, int countM, int countT, int countB, out List<Tuple<string, string>> namesM, out List<Tuple<string, string>> namesT, out List<Tuple<string, string>> namesB)
		{
			_ = precursors.First.Count;
			_ = precursors.Miner.Count;
			_ = precursors.Trader.Count;
			_ = precursors.Builder.Count;
			namesM = new List<Tuple<string, string>>();
			namesT = new List<Tuple<string, string>>();
			namesB = new List<Tuple<string, string>>();
			HashSet<string> alreadyGenerated = new HashSet<string>();
			return true & GenerateNames(precursors, countM, alreadyGenerated, MyFactionTypes.Miner, out namesM) & GenerateNames(precursors, countT, alreadyGenerated, MyFactionTypes.Trader, out namesT) & GenerateNames(precursors, countB, alreadyGenerated, MyFactionTypes.Builder, out namesB);
		}

		private bool GenerateNames(MyNameCollection precursors, int nameCount, HashSet<string> alreadyGenerated, MyFactionTypes type, out List<Tuple<string, string>> pairNameTag)
		{
			bool result = true;
			pairNameTag = new List<Tuple<string, string>>();
			MyList<string> first = precursors.First;
			MyList<string> firstTag = precursors.FirstTag;
			MyList<string> myList = type switch
			{
				MyFactionTypes.Trader => precursors.Trader, 
				MyFactionTypes.Miner => precursors.Miner, 
				_ => precursors.Builder, 
			};
			MyList<string> myList2 = type switch
			{
				MyFactionTypes.Trader => precursors.TraderTag, 
				MyFactionTypes.Miner => precursors.MinerTag, 
				_ => precursors.BuilderTag, 
			};
			for (int i = 0; i < nameCount; i++)
			{
				int num = MyRandom.Instance.Next(0, first.Count);
				int num2 = MyRandom.Instance.Next(0, myList.Count);
				bool flag = false;
				string text = string.Empty;
				string item = string.Empty;
				for (int j = 0; j < first.Count; j++)
				{
					string arg = first[(num + j) % first.Count];
					string arg2 = firstTag[(num + j) % firstTag.Count];
					for (int k = 0; k < myList.Count; k++)
					{
						string arg3 = myList[(num2 + j) % myList.Count];
						string arg4 = myList2[(num2 + j) % myList2.Count];
						text = $"{arg} {arg3}";
						item = $"{arg2}{arg4}";
						if (!alreadyGenerated.Contains(text))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					for (int l = 0; l < MAX_FACTION_COUNT; l++)
					{
						(text, item) = GenerateName(precursors, type, randomFirst: true);
						if (!alreadyGenerated.Contains(text))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					result = false;
				}
				pairNameTag.Add(new Tuple<string, string>(text, item));
				alreadyGenerated.Add(text);
			}
			return result;
		}

		private MyNameCollection GetNamePrecursors()
		{
			if (MyDefinitionManager.Static == null)
			{
				return null;
			}
			DictionaryReader<MyDefinitionId, MyFactionNameDefinition> factionNameDefinitions = MyDefinitionManager.Static.GetFactionNameDefinitions();
			MyNameCollection myNameCollection = new MyNameCollection();
			MyNameCollection myNameCollection2 = new MyNameCollection();
			MyLanguagesEnum myLanguagesEnum = MyLanguagesEnum.English;
			MyLanguagesEnum currentLanguage = MyLanguage.CurrentLanguage;
			foreach (MyFactionNameDefinition value in factionNameDefinitions.Values)
			{
				switch (value.Type)
				{
				case MyFactionNameTypeEnum.Miner:
					if (value.LanguageId == currentLanguage)
					{
						myNameCollection2.Miner.AddRange(value.Names);
						myNameCollection2.MinerTag.AddRange(value.Tags);
					}
					else if (value.LanguageId == myLanguagesEnum)
					{
						myNameCollection.Miner.AddRange(value.Names);
						myNameCollection.MinerTag.AddRange(value.Tags);
					}
					break;
				case MyFactionNameTypeEnum.Trader:
					if (value.LanguageId == currentLanguage)
					{
						myNameCollection2.Trader.AddRange(value.Names);
						myNameCollection2.TraderTag.AddRange(value.Tags);
					}
					else if (value.LanguageId == myLanguagesEnum)
					{
						myNameCollection.Trader.AddRange(value.Names);
						myNameCollection.TraderTag.AddRange(value.Tags);
					}
					break;
				case MyFactionNameTypeEnum.Builder:
					if (value.LanguageId == currentLanguage)
					{
						myNameCollection2.Builder.AddRange(value.Names);
						myNameCollection2.BuilderTag.AddRange(value.Tags);
					}
					else if (value.LanguageId == myLanguagesEnum)
					{
						myNameCollection.Builder.AddRange(value.Names);
						myNameCollection.BuilderTag.AddRange(value.Tags);
					}
					break;
				default:
					if (value.LanguageId == currentLanguage)
					{
						myNameCollection2.First.AddRange(value.Names);
						myNameCollection2.FirstTag.AddRange(value.Tags);
					}
					else if (value.LanguageId == myLanguagesEnum)
					{
						myNameCollection.First.AddRange(value.Names);
						myNameCollection.FirstTag.AddRange(value.Tags);
					}
					break;
				}
			}
			if (currentLanguage != myLanguagesEnum)
			{
				if (myNameCollection2.First.Count <= 0)
				{
					myNameCollection2.First.AddRange(myNameCollection.First);
				}
				if (myNameCollection2.FirstTag.Count <= 0)
				{
					myNameCollection2.FirstTag.AddRange(myNameCollection.First);
				}
				if (myNameCollection2.Miner.Count <= 0)
				{
					myNameCollection2.Miner.AddRange(myNameCollection.Miner);
				}
				if (myNameCollection2.MinerTag.Count <= 0)
				{
					myNameCollection2.MinerTag.AddRange(myNameCollection.Miner);
				}
				if (myNameCollection2.Trader.Count <= 0)
				{
					myNameCollection2.Trader.AddRange(myNameCollection.Trader);
				}
				if (myNameCollection2.TraderTag.Count <= 0)
				{
					myNameCollection2.TraderTag.AddRange(myNameCollection.Trader);
				}
				if (myNameCollection2.Builder.Count <= 0)
				{
					myNameCollection2.Builder.AddRange(myNameCollection.Builder);
				}
				if (myNameCollection2.BuilderTag.Count <= 0)
				{
					myNameCollection2.BuilderTag.AddRange(myNameCollection.Builder);
				}
			}
			if (myNameCollection2.First.Count <= 0)
			{
				MyLog.Default.WriteToLogAndAssert("Faction Name Generator - Collection of First names is empty!!!  Factions will be screwed");
			}
			if (myNameCollection2.First.Count != myNameCollection2.FirstTag.Count)
			{
				MyLog.Default.WriteToLogAndAssert("Faction Name Generator - Faction First name and tag counts does not match!!!  Factions will be screwed");
			}
			if (myNameCollection2.Miner.Count <= 0)
			{
				MyLog.Default.WriteToLogAndAssert("Faction Name Generator - Collection of Miner names is empty!!!  Factions will be screwed");
			}
			if (myNameCollection2.Miner.Count != myNameCollection2.MinerTag.Count)
			{
				MyLog.Default.WriteToLogAndAssert("Faction Name Generator - Faction Miner name and tag counts does not match!!!  Factions will be screwed");
			}
			if (myNameCollection2.Trader.Count <= 0)
			{
				MyLog.Default.WriteToLogAndAssert("Faction Name Generator - Collection of Trader names is empty!!!  Factions will be screwed");
			}
			if (myNameCollection2.Trader.Count != myNameCollection2.TraderTag.Count)
			{
				MyLog.Default.WriteToLogAndAssert("Faction Name Generator - Faction Trader name and tag counts does not match!!!  Factions will be screwed");
			}
			if (myNameCollection2.Builder.Count <= 0)
			{
				MyLog.Default.WriteToLogAndAssert("Faction Name Generator - Collection of Builders names is empty!!!  Factions will be screwed");
			}
			if (myNameCollection2.Builder.Count != myNameCollection2.BuilderTag.Count)
			{
				MyLog.Default.WriteToLogAndAssert("Faction Name Generator - Faction Builder name and tag counts does not match!!!  Factions will be screwed");
			}
			return myNameCollection2;
		}
	}
}
