using System.Collections.Immutable;
using VRage;

namespace Sandbox.Game
{
	public static class MyPlatformGameSettings
	{
		public struct VoxelTrashRemovalSettings
		{
			public int Age;

			public bool RevertAsteroids;

			public int MinDistanceFromGrid;

			public int MinDistanceFromPlayer;

			public bool RevertCloseToNPCGrids;
		}

		public static bool LIMITED_MAIN_MENU = false;

<<<<<<< HEAD
		public static string FEEDBACK_URL = null;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static bool PUBLIC_BETA_MP_TEST = !string.IsNullOrEmpty(FEEDBACK_URL);

		public static bool FEEDBACK_ON_EXIT = true;

<<<<<<< HEAD
=======
		public static string FEEDBACK_URL = null;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public const string CONSOLE_COMPATIBLE_SEARCH_KEY = "CONSOLE_COMPATIBLE";

		public static bool CONSOLE_COMPATIBLE = false;

		public const int CONSOLE_MAX_PLANETS = 3;

		public static bool ENABLE_LOGOS = true;

		public static bool ENABLE_LOGOS_ASAP = false;

		public static int DEFAULT_PROCEDURAL_ASTEROID_GENERATOR = 4;

		public const int CONSOLE_PROCEDURAL_ASTEROID_GENERATOR = 5;

		public static VoxelTrashRemovalSettings? FORCED_VOXEL_TRASH_REMOVAL_SETTINGS = null;

		public static bool IsConsoleModdingAllowed = true;

		public static bool SUPPORT_COMMUNITY_TRANSLATIONS = true;

		public static bool UGC_TEST_ENVIRONMENT = false;

		public static bool BLUEPRINTS_SUPPORT_LOCAL_TYPE = true;

		public static bool CLOUD_ALWAYS_ENABLED = false;

		public static bool GAME_SAVES_COMPRESSED_BY_DEFAULT = false;

		public static bool GAME_SAVES_TO_CLOUD = false;

		public static bool GAME_CONFIG_TO_CLOUD = false;

		public static bool GAME_LAST_SESSION_TO_CLOUD = false;

		public static bool ENABLE_SIMPLE_NEWGAME_SCREEN = false;

		public static bool PREFER_ONLINE = false;

		public static bool ENABLE_NEWGAME_SCREEN_ABTEST = true;

		public static uint WORKSHOP_BROWSER_ITEMS_PER_PAGE = 9u;

		public static bool? SIMPLIFIED_SIMULATION_OVERRIDE;

		public static SimulationQuality? VST_SIMULATION_QUALITY_OVERRIDE;

		public static bool ENABLE_LOW_MEM_WORLD_LOCKDOWN = false;

		public static float ITEM_TOOLTIP_SCALE = 0.7f;

		public static bool CONTROLLER_DEFAULT_ON_START = false;

		public static bool IsMultilineEditableByGamepad = false;

		public static bool SYNCHRONIZED_PLANET_LOADING = false;

		public static int LOBBY_MAX_PLAYERS = 8;

		public static int? LOBBY_TOTAL_PCU_MAX = 50000;

		public static int? SERVER_TOTAL_PCU_MAX = 600000;

		public static int? OFFLINE_TOTAL_PCU_MAX = 100000;

		public static bool IsIgnorePcuAllowed = true;

		public static ImmutableArray<(string SubTypeId, short Count)> ADDITIONAL_BLOCK_LIMITS = ImmutableArray<(string, short)>.Empty;

		public static bool VOICE_CHAT_3D_SOUND = true;

		public static bool VOICE_CHAT_AUTOMATIC_ACTIVATION = false;

		public static bool ENABLE_ANSEL = true;

		public static bool ENABLE_ANSEL_WITH_SPRITES = true;

		public static bool DYNAMIC_REPLICATION_RADIUS = false;

		public static bool VERBOSE_NETWORK_LOGGING = false;

		public const string EOS_RETAIL_URL = "https://retail.epicgames.com/";

		public const string EOS_RETAIL_PRODUCT_ID = "24b1cd652a18461fa9b3d533ac8d6b5b";

		public const string EOS_RETAIL_SANDBOX_ID = "1958fe26c66d4151a327ec162e4d49c8";

		public const string EOS_RETAIL_DEPLOYMENT_ID = "07c169b3b641401496d352cad1c905d6";

		public const string EOS_RETAIL_CLIENT_ID_XBOX = "xyza7891mVx6E5U6hFlTXNUiWhKEFK6G";

		public const string EOS_RETAIL_CLIENT_SECRET_XBOX = "dAe4SP6LIQYAep9y+a00BzEonfUjaeXQovwpBscU5KY";

		public const string EOS_RETAIL_CLIENT_ID_PC = "xyza7891964JhtVD93nm3nZp8t1MbnhC";

		public const string EOS_RETAIL_CLIENT_SECRET_PC = "AKGM16qoFtct0IIIA8RCqEIYG4d4gXPPDNpzGuvlhLA";

		public const string EOS_RETAIL_CLIENT_ID_DS = "xyza7891A4WeGrpP85BTlBa3BSfUEABN";

		public const string EOS_RETAIL_CLIENT_SECRET_DS = "ZdHZVevSVfIajebTnTmh5MVi3KPHflszD9hJB7mRkgg";

		public static bool ENABLE_BEHAVIOR_TREE_TOOL_COMMUNICATION = true;

		public const string MODIO_GAME_NAME = "spaceengineers";

		public const string MODIO_TEST_GAMEID = "331";

		public const string MODIO_TEST_APIKEY = "f2b64abe55452252b030c48adc0c1f0e";

		public const string MODIO_LIVE_GAMEID = "264";

		public const string MODIO_LIVE_APIKEY = "1fb4489996a5e8ffc6ec1135f9985b5b";

		public static bool ENABLE_TRASH_REMOVAL_SETTING => !LIMITED_MAIN_MENU;

		public static bool IsModdingAllowed
		{
			get
			{
				if (!IsConsoleModdingAllowed)
				{
					return !CONSOLE_COMPATIBLE;
				}
				return true;
			}
		}
	}
}
