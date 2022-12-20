<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Linq;
using Sandbox.Game.Localization;
using VRage;
using VRage.Collections;
using VRage.Game.ModAPI;
using VRage.Utils;

namespace Sandbox.Game
{
	/// <summary>
	/// Class for storing DLC ids.
	/// </summary>
	public class MyDLCs
	{
		public sealed class MyDLC : IMyDLC
		{
			public static readonly string DLC_NAME_DeluxeEdition = "DeluxeEdition";

			public static readonly string DLC_NAME_PreorderPack = "PreorderPack";

			public static readonly string DLC_NAME_DecorativeBlocks = "DecorativeBlocks";

			public static readonly string DLC_NAME_Economy = "Economy";

			public static readonly string DLC_NAME_StylePack = "StylePack";

			public static readonly string DLC_NAME_DecorativeBlocks2 = "DecorativeBlocks2";

			public static readonly string DLC_NAME_Frostbite = "Frostbite";

			public static readonly string DLC_NAME_SparksOfTheFuture = "SparksOfTheFuture";

			public static readonly string DLC_NAME_ScrapRace = "ScrapRace";

			public static readonly string DLC_NAME_Warfare1 = "Warfare1";

<<<<<<< HEAD
			public static readonly string DLC_NAME_HeavyIndustry = "HeavyIndustry";

			public static readonly string DLC_NAME_Warfare2 = "Warfare2";

			public static readonly List<MyDLC> DLCList = new List<MyDLC>
			{
				new MyDLC(MyPerGameSettings.DeluxeEditionDlcId, DLC_NAME_DeluxeEdition, MySpaceTexts.DisplayName_DLC_DeluxeEdition, MySpaceTexts.Description_DLC_DeluxeEdition, "Textures\\GUI\\DLCs\\Deluxe\\DeluxeIcon.DDS", "Textures\\GUI\\DLCs\\Deluxe\\DeluxeEdition.dds", null, null),
				new MyDLC(999999990u, DLC_NAME_PreorderPack, MySpaceTexts.DisplayName_DLC_PreorderPack, MySpaceTexts.Description_DLC_PreorderPack, "Textures\\GUI\\DLCs\\PreorderPack\\PreorderPackIcon.DDS", "Textures\\GUI\\DLCs\\PreorderPack\\PreorderPack.dds", "KeenSoftwareHouse.SpaceEngineersPre-orderPack_1.0.0.0_neutral__wgp8fdpqxah6y", "9NW1WR9SM13R"),
				new MyDLC(1049790u, DLC_NAME_DecorativeBlocks, MySpaceTexts.DisplayName_DLC_DecorativeBlocks, MySpaceTexts.Description_DLC_DecorativeBlocks, "Textures\\GUI\\DLCs\\Decorative\\DecorativeBlocks.DDS", "Textures\\GUI\\DLCs\\Decorative\\DecorativeDLC_Badge.DDS", "KeenSoftwareHouse.5342003B4CC9C_1.0.0.0_neutral__wgp8fdpqxah6y", "9NSQZVRNMCCX"),
				new MyDLC(1135960u, DLC_NAME_Economy, MySpaceTexts.DisplayName_DLC_EconomyExpansion, MySpaceTexts.Description_DLC_EconomyExpansion, "Textures\\GUI\\DLCs\\Economy\\Economy.DDS", "Textures\\GUI\\DLCs\\Economy\\EconomyDLC_Badge.DDS", "KeenSoftwareHouse.42644926416AB_1.0.0.0_neutral__wgp8fdpqxah6y", "9NXSMCKTQ1NK"),
				new MyDLC(1084680u, DLC_NAME_StylePack, MySpaceTexts.DisplayName_DLC_StylePack, MySpaceTexts.Description_DLC_StylePack, "Textures\\GUI\\DLCs\\Style\\StylePackDLC.DDS", "Textures\\GUI\\DLCs\\Style\\StylePackDLC_Badge.DDS", "KeenSoftwareHouse.5859112754337_1.0.0.0_neutral__wgp8fdpqxah6y", "9NB0NQS0R8D0"),
				new MyDLC(1167910u, DLC_NAME_DecorativeBlocks2, MySpaceTexts.DisplayName_DLC_DecorativeBlocks2, MySpaceTexts.Description_DLC_DecorativeBlocks2, "Textures\\GUI\\DLCs\\Decorative2\\DecorativeBlocks.DDS", "Textures\\GUI\\DLCs\\Decorative2\\DecorativeDLC_Badge.DDS", "KeenSoftwareHouse.54238C3A3C360_1.0.0.0_neutral__wgp8fdpqxah6y", "9N0JKF6MZCVL"),
				new MyDLC(1241550u, DLC_NAME_Frostbite, MySpaceTexts.DisplayName_DLC_Frostbite, MySpaceTexts.Description_DLC_Frostbite, "Textures\\GUI\\DLCs\\Frostbite\\FrostbiteIcon.DDS", "Textures\\GUI\\DLCs\\Frostbite\\FrostbiteBadge.DDS", "KeenSoftwareHouse.SpaceEngineersFrostbitePack_1.0.0.0_neutral__wgp8fdpqxah6y", "9P3M382025Q7"),
				new MyDLC(1307680u, DLC_NAME_SparksOfTheFuture, MySpaceTexts.DisplayName_DLC_SparksOfTheFuture, MySpaceTexts.Description_DLC_SparksOfTheFuture, "Textures\\GUI\\DLCs\\SparksOfTheFuture\\SparksIcon.DDS", "Textures\\GUI\\DLCs\\SparksOfTheFuture\\SparksBadge.DDS", "KeenSoftwareHouse.SparksoftheFuture_1.0.0.0_neutral__wgp8fdpqxah6y", "9NS1RG0LXX2K"),
				new MyDLC(1374610u, DLC_NAME_ScrapRace, MySpaceTexts.DisplayName_DLC_ScrapRace, MySpaceTexts.Description_DLC_ScrapRace, "Textures\\GUI\\DLCs\\ScrapRace\\Icon.DDS", "Textures\\GUI\\DLCs\\ScrapRace\\Badge.DDS", "KeenSoftwareHouse.SpaceEngineersScrapRacePack_1.0.0.0_neutral__wgp8fdpqxah6y", "9NV5802791GM"),
				new MyDLC(1475830u, DLC_NAME_Warfare1, MySpaceTexts.DisplayName_DLC_Warfare1, MySpaceTexts.Description_DLC_Warfare1, "Textures\\GUI\\DLCs\\Warfare1\\Icon.DDS", "Textures\\GUI\\DLCs\\Warfare1\\WarfareBadge.DDS", "KeenSoftwareHouse.SpaceEngineersWarfarePack_1.0.0.0_neutral__wgp8fdpqxah6y", "9NBMLLPNCB8K"),
				new MyDLC(1676100u, DLC_NAME_HeavyIndustry, MySpaceTexts.DisplayName_DLC_HeavyIndustry, MySpaceTexts.Description_DLC_HeavyIndustry, "Textures\\GUI\\DLCs\\HeavyIndustry\\Icon.DDS", "Textures\\GUI\\DLCs\\HeavyIndustry\\Badge.DDS", "KeenSoftwareHouse.HeavyIndustryPack_1.0.0.0_neutral__wgp8fdpqxah6y", "9NC39BMT4SZ6"),
				new MyDLC(1783760u, DLC_NAME_Warfare2, MySpaceTexts.DisplayName_DLC_Warfare2, MySpaceTexts.Description_DLC_Warfare2, "Textures\\GUI\\DLCs\\Warfare2\\Icon.DDS", "Textures\\GUI\\DLCs\\Warfare2\\Badge.DDS", "KeenSoftwareHouse.Warfare2_1.0.0.0_neutral__wgp8fdpqxah6y", "9PC3RXQ6GC2Z")
			};

=======
			public static readonly List<MyDLC> DLCList = new List<MyDLC>
			{
				new MyDLC(MyPerGameSettings.DeluxeEditionDlcId, DLC_NAME_DeluxeEdition, MySpaceTexts.DisplayName_DLC_DeluxeEdition, MySpaceTexts.Description_DLC_DeluxeEdition, "Textures\\GUI\\DLCs\\Deluxe\\DeluxeIcon.DDS", "Textures\\GUI\\DLCs\\Deluxe\\DeluxeEdition.dds", null, null),
				new MyDLC(999999990u, DLC_NAME_PreorderPack, MySpaceTexts.DisplayName_DLC_PreorderPack, MySpaceTexts.Description_DLC_PreorderPack, "Textures\\GUI\\DLCs\\PreorderPack\\PreorderPackIcon.DDS", "Textures\\GUI\\DLCs\\PreorderPack\\PreorderPack.dds", "KeenSoftwareHouse.SpaceEngineersPre-orderPack_1.0.0.0_neutral__wgp8fdpqxah6y", "9NW1WR9SM13R"),
				new MyDLC(1049790u, DLC_NAME_DecorativeBlocks, MySpaceTexts.DisplayName_DLC_DecorativeBlocks, MySpaceTexts.Description_DLC_DecorativeBlocks, "Textures\\GUI\\DLCs\\Decorative\\DecorativeBlocks.DDS", "Textures\\GUI\\DLCs\\Decorative\\DecorativeDLC_Badge.DDS", "KeenSoftwareHouse.5342003B4CC9C_1.0.0.0_neutral__wgp8fdpqxah6y", "9NSQZVRNMCCX"),
				new MyDLC(1135960u, DLC_NAME_Economy, MySpaceTexts.DisplayName_DLC_EconomyExpansion, MySpaceTexts.Description_DLC_EconomyExpansion, "Textures\\GUI\\DLCs\\Economy\\Economy.DDS", "Textures\\GUI\\DLCs\\Economy\\EconomyDLC_Badge.DDS", "KeenSoftwareHouse.42644926416AB_1.0.0.0_neutral__wgp8fdpqxah6y", "9NXSMCKTQ1NK"),
				new MyDLC(1084680u, DLC_NAME_StylePack, MySpaceTexts.DisplayName_DLC_StylePack, MySpaceTexts.Description_DLC_StylePack, "Textures\\GUI\\DLCs\\Style\\StylePackDLC.DDS", "Textures\\GUI\\DLCs\\Style\\StylePackDLC_Badge.DDS", "KeenSoftwareHouse.5859112754337_1.0.0.0_neutral__wgp8fdpqxah6y", "9NB0NQS0R8D0"),
				new MyDLC(1167910u, DLC_NAME_DecorativeBlocks2, MySpaceTexts.DisplayName_DLC_DecorativeBlocks2, MySpaceTexts.Description_DLC_DecorativeBlocks2, "Textures\\GUI\\DLCs\\Decorative2\\DecorativeBlocks.DDS", "Textures\\GUI\\DLCs\\Decorative2\\DecorativeDLC_Badge.DDS", "KeenSoftwareHouse.54238C3A3C360_1.0.0.0_neutral__wgp8fdpqxah6y", "9N0JKF6MZCVL"),
				new MyDLC(1241550u, DLC_NAME_Frostbite, MySpaceTexts.DisplayName_DLC_Frostbite, MySpaceTexts.Description_DLC_Frostbite, "Textures\\GUI\\DLCs\\Frostbite\\FrostbiteIcon.DDS", "Textures\\GUI\\DLCs\\Frostbite\\FrostbiteBadge.DDS", "KeenSoftwareHouse.SpaceEngineersFrostbitePack_1.0.0.0_neutral__wgp8fdpqxah6y", "9P3M382025Q7"),
				new MyDLC(1307680u, DLC_NAME_SparksOfTheFuture, MySpaceTexts.DisplayName_DLC_SparksOfTheFuture, MySpaceTexts.Description_DLC_SparksOfTheFuture, "Textures\\GUI\\DLCs\\SparksOfTheFuture\\SparksIcon.DDS", "Textures\\GUI\\DLCs\\SparksOfTheFuture\\SparksBadge.DDS", "KeenSoftwareHouse.SparksoftheFuture_1.0.0.0_neutral__wgp8fdpqxah6y", "9NS1RG0LXX2K"),
				new MyDLC(1374610u, DLC_NAME_ScrapRace, MySpaceTexts.DisplayName_DLC_ScrapRace, MySpaceTexts.Description_DLC_ScrapRace, "Textures\\GUI\\DLCs\\ScrapRace\\Icon.DDS", "Textures\\GUI\\DLCs\\ScrapRace\\Badge.DDS", "KeenSoftwareHouse.SpaceEngineersScrapRacePack_1.0.0.0_neutral__wgp8fdpqxah6y", "9NV5802791GM"),
				new MyDLC(1475830u, DLC_NAME_Warfare1, MySpaceTexts.DisplayName_DLC_Warfare1, MySpaceTexts.Description_DLC_Warfare1, "Textures\\GUI\\DLCs\\Warfare1\\Icon.DDS", "Textures\\GUI\\DLCs\\Warfare1\\WarfareBadge.DDS", "KeenSoftwareHouse.SpaceEngineersWarfarePack_1.0.0.0_neutral__wgp8fdpqxah6y", "9NBMLLPNCB8K")
			};

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public uint AppId { get; }

			public string Name { get; }

<<<<<<< HEAD
			/// <summary>
			/// Name of the DLC, preferably a localized string
			/// </summary>
			public MyStringId DisplayName { get; }

			/// <summary>
			/// Description of the DLC, preferably a localized string
			/// </summary>
			public MyStringId Description { get; }

			/// <summary>
			/// Icon of the DLC, to be displayed in G-screen, blueprints, etc ...
			/// </summary>
			public string Icon { get; }

			/// <summary>
			/// Badge of the DLC, to be displayed in main menu
			/// </summary>
=======
			public MyStringId DisplayName { get; }

			public MyStringId Description { get; }

			public string Icon { get; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public string Badge { get; }

			public string PackageId { get; }

			public string StoreId { get; }

			private MyDLC(uint appId, string name, MyStringId displayName, MyStringId description, string icon, string badge, string packageId, string storeId)
			{
				AppId = appId;
				Name = name;
				DisplayName = displayName;
				Description = description;
				Icon = icon;
				Badge = badge;
				PackageId = packageId;
				StoreId = storeId;
			}
		}

<<<<<<< HEAD
		private static readonly Dictionary<uint, MyDLC> m_dlcs = MyDLC.DLCList.ToDictionary((MyDLC x) => x.AppId, (MyDLC x) => x);

		private static readonly Dictionary<string, MyDLC> m_dlcsByName = MyDLC.DLCList.ToDictionary((MyDLC x) => x.Name, (MyDLC x) => x);
=======
		private static readonly Dictionary<uint, MyDLC> m_dlcs = Enumerable.ToDictionary<MyDLC, uint, MyDLC>((IEnumerable<MyDLC>)MyDLC.DLCList, (Func<MyDLC, uint>)((MyDLC x) => x.AppId), (Func<MyDLC, MyDLC>)((MyDLC x) => x));

		private static readonly Dictionary<string, MyDLC> m_dlcsByName = Enumerable.ToDictionary<MyDLC, string, MyDLC>((IEnumerable<MyDLC>)MyDLC.DLCList, (Func<MyDLC, string>)((MyDLC x) => x.Name), (Func<MyDLC, MyDLC>)((MyDLC x) => x));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private static readonly HashSet<string> m_unsupportedDLCs = new HashSet<string>();

		public static DictionaryReader<uint, MyDLC> DLCs => m_dlcs;

		public static bool TryGetDLC(uint id, out MyDLC dlc)
		{
			return m_dlcs.TryGetValue(id, out dlc);
		}

		public static bool TryGetDLC(string name, out MyDLC dlc)
		{
			return m_dlcsByName.TryGetValue(name, out dlc);
		}

		public static MyDLC GetDLC(string name)
		{
			return m_dlcsByName[name];
		}

		public static string GetRequiredDLCTooltip(string name)
		{
			if (TryGetDLC(name, out var dlc))
			{
				return GetRequiredDLCTooltip(dlc.AppId);
			}
			return null;
		}

		public static string GetRequiredDLCTooltip(uint id)
		{
			if (TryGetDLC(id, out var dlc))
			{
				return string.Format(MyTexts.GetString(MyCommonTexts.RequiresDlc), MyTexts.GetString(dlc.DisplayName));
			}
			return string.Format(MyTexts.GetString(MyCommonTexts.RequiresDlc), id);
		}

		public static string GetRequiredDLCStoreHint(uint id)
		{
			if (TryGetDLC(id, out var dlc))
			{
				return string.Format(MyTexts.GetString(MyCommonTexts.ShowDlcStore), MyTexts.GetString(dlc.DisplayName));
			}
			return string.Format(MyTexts.GetString(MyCommonTexts.ShowDlcStore), id);
		}

		public static string GetDLCIcon(uint id)
		{
			if (TryGetDLC(id, out var dlc))
			{
				return dlc.Icon;
			}
			return null;
		}

		public static void SetUnsupported(string dlcIdentifier)
		{
			m_unsupportedDLCs.Add(dlcIdentifier);
		}

		public static bool IsDLCSupported(string dlcIdentifier)
		{
			if (string.IsNullOrEmpty(dlcIdentifier))
			{
				return true;
			}
			return !m_unsupportedDLCs.Contains(dlcIdentifier);
		}
	}
}
