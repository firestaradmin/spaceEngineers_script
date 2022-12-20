using System.Runtime.CompilerServices;
using Sandbox.Game;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Engine.Utils
{
	public static class MyFakes
	{
		public static MyQuickLaunchType? QUICK_LAUNCH;

		public static bool ALT_AS_DEBUG_KEY;

		public static bool ENABLE_F12_MENU;

		public const bool DETECT_LEAKS = false;

		public static bool ENABLE_LONG_DISTANCE_GIZMO_DRAWING;

		public static bool ENABLE_MENU_VIDEO_BACKGROUND;

		public static bool ENABLE_SPLASHSCREEN;

		internal static MyEntity FakeTarget;

		public static bool ENABLE_EDGES;

		public static bool ENABLE_GRAVITY_PHANTOM;

		public static bool ENABLE_INFINITE_REACTOR_FUEL;

		public static bool ENABLE_BATTERY_SELF_RECHARGE;

		public static bool MANUAL_CULL_OBJECTS;

		public static bool ENABLE_JOYSTICK_SETTINGS;

		public static bool UNLIMITED_CHARACTER_BUILDING;

		public static bool DETECT_DISCONNECTS;

		public static bool DRAW_GUI_SCREEN_BORDERS;

		public static bool ENABLE_SLOW_WINDOW_TRANSITION_ANIMATIONS;

		public static bool SHOW_DAMAGE_EFFECTS;

		public static float THRUST_FORCE_RATIO;

		/// DEFORMATIONS
		public static bool DEFORMATION_LOGGING;

		public static float DEFORMATION_MINIMUM_VELOCITY;

		public static float DEFORMATION_MIN_BREAK_IMPULSE;

		public static float DEFORMATION_FORCED_VELOCITY;

		public static float DEFORMATION_MASS_THR;

		public static float DEFORMATION_OFFSET_RATIO;

		public static float DEFORMATION_OFFSET_MAX;

		public static float DEFORMATION_PROJECTILE_OFFSET_RATIO;

		public static float DEFORMATION_DRILL_OFFSET_RATIO;

		public static float DRILL_DAMAGE;

		public static bool DEFORMATION_APPLY_IMPULSE;

		public static float DEFORMATION_IMPULSE_FACTOR;

		public static float DEFORMATION_VELOCITY_RELAY;

		public static float DEFORMATION_VELOCITY_RELAY_STATIC;

		public static bool DEFORMATION_EXPLOSIONS;

		public static float DEFORMATION_VOXEL_CUTOUT_MULTIPLIER;

		public static float DEFORMATION_VOXEL_CUTOUT_MULTIPLIER_SMALL_GRID;

		public static float DEFORMATION_DAMAGE_MULTIPLIER;

		public static float DEFORMATION_VOXEL_CUTOUT_MAX_RADIUS;

		public static bool ENABLE_AUTOSAVE;

		public static bool ENABLE_TRANSPARENT_CUBE_BUILDER;

		public static bool HIDE_ENGINEER_TOOL_HIGHLIGHT;

		public static bool ENABLE_SIMPLE_GRID_PHYSICS;

		public static bool FORCE_SOFTWARE_MOUSE_DRAW;

		public static bool ENABLE_GATLING_TURRETS;

		public static bool ENABLE_MISSILE_TURRETS;

		public static bool ENABLE_INTERIOR_TURRETS;

		public static bool ENABLE_DAMAGED_COMPONENTS;

		public static bool CHARACTER_CAN_DIE_EVEN_IN_CREATIVE_MODE;

		public static bool ENABLE_EXPORT_MTL_DIAGNOSTICS;

		public static bool SHOW_INVALID_TRIANGLES;

		public static bool ENABLE_NEW_COLLISION_AVOIDANCE;

		public static bool ENABLE_NEW_SOUNDS;

		public static bool ENABLE_NEW_SOUNDS_QUICK_UPDATE;

		public static bool ENABLE_NEW_SMALL_SHIP_SOUNDS;

		public static bool ENABLE_NEW_LARGE_SHIP_SOUNDS;

		public static bool ENABLE_MUSIC_CONTROLLER;

		public static bool ENABLE_REALISTIC_LIMITER;

		public static bool ENABLE_REALISTIC_ON_TOUCH;

		public static bool ENABLE_NON_PUBLIC_BLOCKS;

		public static bool ENABLE_NON_PUBLIC_CATEGORY_CLASSES;

		public static bool ENABLE_NON_PUBLIC_GUI_ELEMENTS;

		public static bool ENABLE_CHARACTER_AND_DEBRIS_COLLISIONS;

		public static bool SIMULATE_SLOW_UPDATE;

		public static bool SHOW_AUDIO_DEV_SCREEN;

		public static bool ENABLE_SCRAP;

		public static bool SIMULATE_NO_SOUND_CARD;

		public static bool INACTIVE_THRUSTER_DMG;

		public static bool ENABLE_CARGO_SHIPS;

		public static bool ENABLE_METEOR_SHOWERS;

		public static bool SHOW_INVENTORY_ITEM_IDS;

		public static bool SIMULATE_QUICK_TRIGGER;

		public static float SIMULATION_SPEED;

		public static bool TEST_PREFABS_FOR_INCONSISTENCIES;

		public static bool SHOW_PRODUCTION_QUEUE_ITEM_IDS;

		public static bool ENABLE_CONNECT_COMMAND_LINE;

		public static bool ENABLE_WHEEL_CONTROLS_IN_COCKPIT;

		public static bool TEST_NEWS;

		public static bool ENABLE_TRASH_REMOVAL;

		public static bool SHOW_FACTIONS_GUI;

		public static bool ENABLE_RADIO_HUD;

		public static bool ENABLE_REMOVED_VOXEL_CONTENT_HACK;

		public static bool ENABLE_AUTO_HEAL;

		public static bool ENABLE_CENTER_OF_MASS;

		public static bool ENABLE_VIDEO_PLAYER;

		public static bool ENABLE_COPY_GROUP;

		public static bool LANDING_GEAR_BREAKABLE;

		public static bool ENABLE_WORKSHOP_MODS;

		public static bool ENABLE_SHIP_BLOCKS_TOOLBAR;

		public static bool SKIP_VOXELS_DURING_LOAD;

		public static bool ENABLE_TERMINAL_PROPERTIES;

		public static bool ENABLE_GYRO_OVERRIDE;

		public static bool TEST_MODELS;

		public static bool TEST_MODELS_WRONG_TRIANGLES;

		public static bool DISABLE_SOUND_POOLING;

		public static bool ENABLE_GRAVITY_GENERATOR_SPHERE;

		public static bool ENABLE_REMOTE_CONTROL;

		public static bool ENABLE_DAMPENERS_OVERRIDE;

		public static bool ENABLE_BLOCK_PLACEMENT_ON_VOXEL;

		public static bool ENABLE_COMPOUND_BLOCKS;

		public static bool ENABLE_SCRIPTS;

		public static bool ENABLE_SCRIPTS_PDB;

		public static bool ENABLE_TURRET_CONTROL;

		public static bool ENABLE_WELDER_HELP_OTHERS;

		public static bool ENABLE_MISSION_SCREEN;

		public static bool ENABLE_OBJECTIVE_LINE;

		public static bool ENABLE_SUBBLOCKS;

		public static bool ENABLE_MULTIBLOCKS;

		public static bool ENABLE_MULTIBLOCKS_IN_SURVIVAL;

		public static bool ENABLE_MULTIBLOCK_PART_IDS;

		public static bool ENABLE_MULTIBLOCK_CONSTRUCTION;

		public static bool ENABLE_STATIC_SMALL_GRID_ON_LARGE;

		public static bool ENABLE_LARGE_OFFSET;

		public static bool ENABLE_PROJECTOR_BLOCK;

		public static bool ENABLE_ASTEROID_FIELDS;

		public static bool ENABLE_USE_NEW_OBJECT_HIGHLIGHT;

		public static float MAX_PRECALC_TIME_IN_MILLIS;

		public static bool ENABLE_YIELDING_IN_PRECALC_TASK;

		public static bool LOAD_UNCONTROLLED_CHARACTERS;

		public static bool ENABLE_PREFAB_THROWER;

		public static bool ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA;

		public static bool ENABLE_DYNAMIC_SMALL_GRID_MERGING;

		public static bool ENABLE_ENVIRONMENT_ITEMS;

		public static bool ENABLE_BARBARIANS;

		public static bool DEBUG_DRAW_NAVMESH_PROCESSED_VOXEL_CELLS;

		public static bool DEBUG_DRAW_NAVMESH_PREPARED_VOXEL_CELLS;

		public static bool DEBUG_DRAW_NAVMESH_CELLS_ON_PATHS;

		public static bool REMOVE_VOXEL_NAVMESH_CELLS;

		public static bool DEBUG_DRAW_VOXEL_CONNECTION_HELPER;

		public static bool DEBUG_DRAW_FOUND_PATH;

		public static bool DEBUG_DRAW_FUNNEL;

		public static bool DEBUG_DRAW_NAVMESH_CELL_BORDERS;

		public static bool DEBUG_DRAW_NAVMESH_HIERARCHY;

		public static bool DEBUG_DRAW_NAVMESH_HIERARCHY_LITE;

		public static bool DEBUG_DRAW_NAVMESH_EXPLORED_HL_CELLS;

		public static bool DEBUG_DRAW_NAVMESH_FRINGE_HL_CELLS;

		public static bool DEBUG_DRAW_NAVMESH_LINKS;

		public static bool SHOW_PATH_EXPANSION_ASSERTS;

		public static bool DEBUG_ONE_AI_STEP_SETTING;

		public static bool DEBUG_ONE_AI_STEP;

		public static bool DEBUG_ONE_VOXEL_PATHFINDING_STEP_SETTING;

		public static bool DEBUG_ONE_VOXEL_PATHFINDING_STEP;

		public static bool DEBUG_BEHAVIOR_TREE;

		public static bool DEBUG_BEHAVIOR_TREE_ONE_STEP;

		public static bool LOG_NAVMESH_GENERATION;

		public static bool REPLAY_NAVMESH_GENERATION;

		public static bool REPLAY_NAVMESH_GENERATION_TRIGGER;

		public static bool ENABLE_AFTER_REPLACE_BODY;

		public static bool ENABLE_BLOCK_PLACING_ON_INTERSECTED_POSITION;

		public static bool ENABLE_COMMUNICATION;

		public static bool ENABLE_GUI_HIDDEN_CUBEBLOCKS;

		public static bool ENABLE_BLOCK_STAGES;

		public static bool SHOW_REMOVE_GIZMO;

		public static bool ENABLE_PROGRAMMABLE_BLOCK;

		public static bool ENABLE_DESTRUCTION_EFFECTS;

		public static bool ENABLE_COLLISION_EFFECTS;

		public static bool ENABLE_PHYSICS_HIGH_FRICTION;

		public static float PHYSICS_HIGH_FRICTION;

		public static bool REUSE_OLD_PLAYER_IDENTITY;

		public static bool ENABLE_MOD_CATEGORIES;

		public static bool ENABLE_GENERATED_BLOCKS;

		public static bool ENABLE_HAVOK_MULTITHREADING;

		public static bool ENABLE_HAVOK_PARALLEL_SCHEDULING;

		public static bool ENABLE_HAVOK_STEP_OPTIMIZERS;

		public static bool ENABLE_HAVOK_STEP_OPTIMIZERS_TIMINGS;

		public static bool ENABLE_GPS;

		public static bool SHOW_FORBIDDEN_ENITIES_VOXEL_HAND;

		public static bool ENABLE_WEAPON_TERMINAL_CONTROL;

		public static bool ENABLE_WAIT_UNTIL_CLIPMAPS_READY;

		public static bool SHOW_MISSING_DESTRUCTION;

		public static bool ENABLE_DEBUG_DRAW_TEXTURE_NAMES;

		public static bool ENABLE_GRID_SYSTEM_UPDATE;

		public static bool ENABLE_GRID_SYSTEM_ONCE_BEFORE_FRAME_UPDATE;

		public static bool REDUCE_FRACTURES_COUNT;

		public static bool REMOVE_GENERATED_BLOCK_FRACTURES;

		public static bool ENABLE_TOOL_SHAKE;

		public static bool OVERRIDE_LANDING_GEAR_INERTIA;

		public static float LANDING_GEAR_INTERTIA;

		public static bool ASSERT_CHANGES_IN_SIMULATION;

		public static bool USE_HAVOK_MODELS;

		public static bool LAZY_LOAD_DESTRUCTION;

		public static bool ENABLE_STANDARD_AXES_ROTATION;

		public static bool ENABLE_ARMOR_HAND;

		public static bool ASSERT_NON_PUBLIC_BLOCKS;

		public static bool REMOVE_NON_PUBLIC_BLOCKS;

		public static bool ENABLE_ROTATION_HINTS;

		public static bool ENABLE_NOTIFICATION_BLOCK_NOT_AVAILABLE;

		public static bool FORCE_CLUSTER_REORDER;

		public static bool PAUSE_PHYSICS;

		public static bool STEP_PHYSICS;

		public static MyEntity VDB_ENTITY;

		public static bool ENABLE_VOXEL_PHYSICS_SHAPE_DISCARDING;

		public static bool ENABLE_SMALL_BLOCK_TO_LARGE_STATIC_CONNECTIONS;

		public static bool ENABLE_LARGE_STATIC_GROUP_COPY_FIRST;

		public static bool CLONE_SHAPES_ON_WORKER;

		public static bool FRACTURED_BLOCK_AABB_MOUNT_POINTS;

		public static bool ENABLE_BLOCK_COLORING;

		public static bool CHANGE_BLOCK_CONVEX_RADIUS;

		public static bool ENABLE_TEST_BLOCK_CONNECTIVITY_CHECK;

		public static bool ENABLE_CUSTOM_CHARACTER_IMPACT;

		public static bool ENABLE_FOOT_IK;

		public static bool ENABLE_JETPACK_IN_SURVIVAL;

		public static bool ENABLE_FOOT_IK_USE_HAVOK_RAYCAST;

		public static bool DEVELOPMENT_PRESET;

		public static bool SHOW_CURRENT_VOXEL_MAP_AABB_IN_VOXEL_HAND;

		public static bool ENABLE_OXYGEN_SOUNDS;

		public static bool ENABLE_ROPE_UNWINDING_TORQUE;

		public static bool ENABLE_BONES_AND_ANIMATIONS_DEBUG;

		public static bool ENABLE_RAGDOLL;

		public static bool ENABLE_RAGDOLL_BONES_TRANSLATION;

		public static bool ENABLE_RAGDOLL_DEFAULT_PROPERTIES;

		public static bool ENABLE_RAGDOLL_CLIENT_SYNC;

		public static bool FORCE_RAGDOLL_DEACTIVATION;

		public static bool ENABLE_RAGDOLL_DEBUG;

		public static bool ENABLE_JETPACK_RAGDOLL_COLLISIONS;

		public static float RAGDOLL_ANIMATION_WEIGHTING;

		public static float RAGDOLL_GRAVITY_MULTIPLIER;

		public static bool ENABLE_STATION_ROTATION;

		public static bool ENABLE_CONTROLLER_HINTS;

		public static bool ENABLE_PHYSICS_SETTINGS;

		public static bool ENABLE_DEFAULT_BLUEPRINTS;

		public static int VOICE_CHAT_TARGET_BIT_RATE;

		public static float VOICE_CHAT_MIC_SENSITIVITY;

		public static bool VOICE_CHAT_ECHO;

		public static float VOICE_CHAT_PLAYBACK_DELAY;

		public static bool ENABLE_VOICE_CHAT_DEBUGGING;

		public static bool ENABLE_GENERATED_INTEGRITY_FIX;

		public static bool ENABLE_VOXEL_MAP_AABB_CORNER_TEST;

		public static bool ENABLE_PERMANENT_SIMULATIONS_COMPUTATION;

		public static bool ENABLE_ADMIN_SPECTATOR_BUILDING;

		public static bool ENABLE_MEDIEVAL_INVENTORY;

		/// <summary>
		/// If true, container grid mass will be static
		/// If false, container grid mass includes the content mass
		/// </summary>
		public static bool ENABLE_STATIC_INVENTORY_MASS;

		public static bool ENABLE_PLANETS;

		public static bool ENABLE_NEW_TRIGGERS;

		public static bool ENABLE_USE_OBJECT_CORNERS;

		public static bool ENABLE_PLANETS_JETPACK_LIMIT_IN_CREATIVE;

		public static bool ENABLE_STATS_GUI;

		public static bool WELD_LANDING_GEARS;

		public static bool ENFORCE_CONTROLLER;

		public static bool ENABLE_ALL_IN_SURVIVAL;

		public static bool ENABLE_SURVIVAL_SWITCHING;

		public static bool ENABLE_DRIVING_PARTICLES;

		public static bool ENABLE_BLOCKS_IN_VOXELS_TEST;

		public static bool ENABLE_TURRET_LASERS;

		public static bool SKIP_BIOME_MAP;

		public static bool PRIORITIZE_PRECALC_JOBS;

		public static bool DISABLE_COMPOSITE_MATERIAL;

		public static bool ENABLE_PLANETARY_CLOUDS;

		public static bool ENABLE_CLOUD_FOG;

		public static bool ENVIRONMENT_ITEMS_ONE_INSTANCEBUFFER;

		public static bool ENABLE_FRACTURE_COMPONENT;

		public static bool TESTING_VEHICLES;

		public static bool ENABLE_WALKING_PARTICLES;

		public static bool ENABLE_HYDROGEN_FUEL;

		public static bool WELD_PISTONS;

		public static bool WELD_ROTORS;

		public static bool SUSPENSION_POWER_RATIO;

		public static bool TWO_STEP_SIMULATIONS;

		public static bool WHEEL_SOFTNESS;

		public static bool WHEEL_ALTERNATIVE_MODELS_ENABLED;

		public static bool FORCE_SINGLE_WORKER;

		public static bool FORCE_NO_WORKER;

		public static bool DISABLE_CLIPBOARD_PLACEMENT_TEST;

		public static bool ENABLE_LIMITED_CHARACTER_BODY;

		public static bool ENABLE_VOXEL_COMPUTED_OCCLUSION;

		public static bool ENABLE_COMPOUND_BLOCK_COLLISION_DUMMIES;

		public static bool ENABLE_EXTENDED_PLANET_OPTIONS;

		public static bool ENABLE_JOIN_SCREEN_REMAINING_TIME;

		public static bool ENABLE_INVENTORY_FIX;

		public static bool ENABLE_LAZY_VOXEL_PHYSICS;

		public static bool ENABLE_PLANET_HIERARCHY;

		public static bool ENABLE_FLORA_COMPONENT_DEBUG;

		public static bool ENABLE_DEBUG_DRAW_COORD_SYS;

		public static bool ENABLE_FRACTURE_PIECE_SHAPE_CHECK;

		public static bool ENABLE_PLANET_FIREFLIES;

		public static bool ENABLE_DURABILITY_COMPONENT;

		public static bool SPAWN_SPACE_FAUNA_IN_CREATIVE;

		public static bool ENABLE_RUN_WITHOUT_STEAM;

		public static bool PRECISE_SIM_SPEED;

		public static bool ENABLE_SIMSPEED_LOCKING;

		public static bool BACKGROUND_OXYGEN;

		public static bool ENABLE_GATHERING_SMALL_BLOCK_FROM_GRID;

		public static bool ENABLE_COMPONENT_BLOCKS;

		public static bool ENABLE_SMALL_GRID_BLOCK_INFO;

		public static bool ENABLE_SMALL_GRID_BLOCK_COMPONENT_INFO;

		public static bool ENABLE_BOUNDINGBOX_SHRINKING;

		public static bool ENABLE_HUD_PICKED_UP_ITEMS;

		public static bool ENABLE_VR_DRONE_COLLISIONS;

		public static bool ENABLE_VR_BLOCK_DEFORMATION_RATIO;

		public static bool ENABLE_VR_FORCE_BLOCK_DESTRUCTIBLE;

		public static bool ENABLE_VR_REMOTE_CONTROL_WAYPOINTS_FAST_MOVEMENT;

		public static bool ENABLE_VR_BUILDING;

		public static bool ENABLE_SEPARATE_USE_AND_PICK_UP_KEY;

		public static bool ENABLE_USE_DEFAULT_DAMAGE_DECAL;

		public static bool ENABLE_QUICK_WARDROBE;

		public static bool ENABLE_TYPES_FROM_MODS;

		public static bool ENABLE_PRELOAD_DEFINITIONS;

		public static bool ENABLE_CESTMIR_PATHFINDING;

		public static bool ENABLE_ROSLYN_SCRIPT_DIAGNOSTICS;

		/// <summary>
		/// MULTIPLAYER RELATED FAKES
		/// </summary>
		public static bool MP_ISLANDS;

		public static bool MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CHARACTER;

		public static bool MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CHARACTER_IN_JETPACK;

		public static bool MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_GRID;

		public static bool MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CAR;

		public static bool MP_SYNC_CLUSTERTREE;

		public static bool MULTIPLAYER_PREDICTION_RESET_CLIENT_FALLING_BEHIND;

		public static bool MULTIPLAYER_SMOOTH_PING;

		public static bool MULTIPLAYER_SKIP_PREDICTION;

		public static bool MULTIPLAYER_SKIP_PREDICTION_SUBGRIDS;

		public static bool MULTIPLAYER_SKIP_ANIMATION;

		public static bool MULTIPLAYER_EXTRAPOLATION_SMOOTHING;

		public static bool SNAPSHOTCACHE_HIERARCHY;

		public static bool WORLD_SNAPSHOTS;

		public static bool SNAPSHOTS_MECHANICAL_PIVOTS;

		public static bool MULTIPLAYER_CLIENT_CONSTRAINTS;

		public static bool WORLD_LOCKING_IN_CLIENTUPDATE;

		public static float SLOWDOWN_FACTOR_TORQUE_MULTIPLIER;

		public static float SLOWDOWN_FACTOR_TORQUE_MULTIPLIER_LARGE_SHIP;

		public static bool ENABLE_MP_DATA_HASHES;

		public static bool ENABLE_DOUBLED_KINEMATIC;

		public static bool ENABLE_CPU_PARTICLES;

		public static bool FORCE_UPDATE_NEWSLETTER_STATUS;

		public static bool SUN_GLARE;

		public static bool LEGACY_HUD;

		public static bool FORCE_CHARTOOLS_1ST_PERSON;

		public static bool ENABLE_TREES_IN_THE_NEW_PIPE;

		public static bool ENABLE_ANSEL_IN_MULTIPLAYER;

		public static float ANIMATION_UPDATE_DISTANCE;

		public static bool ENABLE_SOUNDS_ASYNC_PRELOAD;

		public static bool ENABLE_MAIN_MENU_INVENTORY_SCENE;

		public static bool ENABLE_IME;

		public static bool ENABLE_MULTICORE_JIT;

		public static bool ENABLE_NGEN;

		public static bool FORCE_CHARACTER_2D_SOUND;

		public static bool TESTING_JUMPDRIVE;

		public static bool TREE_MESH_FROM_MODEL;

		public static bool ENABLE_STARDUST_ON_PLANET;

		public static bool ENABLE_DEBRIS;

		public static bool DEBUG_DISPLAY_DESTROY_EFFECT_OFFSET;

		public static bool PRIORITIZED_VICINITY_ASSETS_LOADING;

		public static double PRIORITIZED_CUBE_VICINITY_RADIUS;

		public static double PRIORITIZED_VOXEL_VICINITY_RADIUS_FAR;

		public static double PRIORITIZED_VOXEL_VICINITY_RADIUS_CLOSE;

		public static bool PLANET_CRASH_ENABLED;

		public static bool AUDIO_ENABLE_REVERB;

		public static bool OPTIMIZE_GRID_UPDATES;

		public static bool TESTING_TOOL_PLUGIN;

		public static bool ENABLE_ANIMATED_KINEMATIC_UPDATE;

		public static bool ENABLE_GRID_PLACEMENT_TEST;

		public static bool USE_GOODBOT_DEV_SERVER;

		public static bool ENABLE_WAIT_UNTIL_MULTIPLAYER_READY;

		public static bool ENABLE_PRELOAD_CHARACTER_ANIMATIONS;

		public static readonly bool LOADING_STREAMING_TIMEOUT_ENABLED;

		public static bool ENABLE_MINIDUMP_SENDING;

		public static bool COLLECT_SUSPEND_DUMPS;

		public static bool USE_GPS_AS_FRIENDLY_SPAWN_LOCATIONS;

		public static readonly bool I_AM_READY_FOR_NEW_BLUEPRINT_SCREEN;

		public static readonly bool I_AM_READY_FOR_NEW_SCRIPT_SCREEN;

		public const bool DONT_TRANSLATE_BLOCK_DATA = false;

		public static readonly bool ENABLE_WORKSHOP_PUBLISH;

		public static bool ENABLE_DRILL_ROCKS;

		public static bool VOXELHAND_PARALLEL;

		public static bool OWN_ALL_DLCS;

		public static bool OWN_ALL_ITEMS;

		public static readonly uint SWITCH_DLC_FROM;

		public static readonly uint SWITCH_DLC_TO;

		public static bool FORCE_ADD_TRASH_REMOVAL_MENU;

		public static bool ENABLE_GDPR_MESSAGE;

		public static bool ENABLE_ASTEROIDS;

		public static bool ENABLE_GRID_STORAGE_TEST_FUNCTIONS;

		public static readonly bool ENABLE_STATION_GENERATOR_DEBUG_DRAW;

		public static readonly bool ENABLE_RELATION_GENERATOR_DEBUG_DRAW;

		public static readonly bool ENABLE_ZONE_CHIP_REQ;

		public static readonly bool CONTRACT_ESCORT_DEBUGDRAW;

		public static readonly bool CONV_PULL_CACL_IMMIDIATLY_STORE_SAFEZONE;

		public static bool ENABLE_AREA_INTERACTIONS;

		public static bool ENABLE_AREA_INTERACTIONS_BLOCKS;

		public static bool NETWORK_SINGLE_THREADED;

		public static bool ENABLE_SMART_UPDATER;

		public static bool ENABLE_ECONOMY_ANALYTICS;

		public static readonly bool ENABLE_BOULDER_NAME_PARSING;

		public static readonly bool ENABLE_DOOR_SAFETY;

		public static readonly bool ENABLE_DOOR_SAFETY_2;

		public static bool USE_PARALLEL_TOOL_RAYCAST;

		public static readonly bool ENABLE_MAGBOOTS_ON_GRID_SMOOTHING;

		public static bool ENABLE_PERFORMANCELOGGING;

		public static bool ForcePlayoutDelayBuffer;

		public static bool ENABLE_CHARACTER_IK_FEET_OFFSET;

		public static float CHARACTER_ANIMATION_SPEED;

		public static float CHARACTER_ANKLE_HEIGHT;

		public static bool CHARACTER_FOOTS_DEBUG_DRAW;

		public static bool RECORD_CHARACTER_FOOT_ANIMATION;

		public static bool HIDE_CROSSHAIR_OPTIONS;

		public static readonly bool SHOW_MATCH_STOP;
<<<<<<< HEAD

		public static readonly bool ENABLE_DYNAMIC_EFFECTIVE_RANGE;

		public static readonly bool ENABLE_AMMO_DETONATION;

		public static readonly bool FORCE_HARDCODED_GAMEPAD_CONTROLS;

		public static bool PROJECTILES_APPLY_RAYCAST_OPTIMIZATION;

		public static bool PROJECTILES_APPLY_GRAVITY;

		public static bool PROJECTILES_EARLY_EXIT_CHECK;

		public static bool LEAD_INDICATOR_ITERATIVE_PREDICTION;

		public static bool LEAD_INDICATOR_DEBUGDRAW_BULLET_DROP;

		public static bool LEAD_DEBUGDRAW_MISSILE_PREDICTION_SHORTCUTS_ENABLED;

		public static bool ENABLE_TARGET_LOCKING_RAYCAST_VOXEL_PHYSICS_PREFETCH;

		public static bool ENABLE_HAND_ANIMATIONS_CALCULATION_ON_DS;

		public static bool TARGETING_DISTANCE_MODIFIER_ENABLED;

		public static float TARGETING_DISTANCE_MODIFIER_POWER;

		public static float TARGETING_DISTANCE_MODIFIER_POWER_LIMIT;

		public static bool ENABLE_TARGETING_CHANCE_DRAW;

		public static bool DrawSolarOcclusionOnce;

		public static bool ENABLE_MISSILES_NATURAL_GRAVITY;

		public static bool ENABLE_COLDSTART_ASSEMBLY_LOADING;
=======

		public static readonly bool ENABLE_DYNAMIC_EFFECTIVE_RANGE;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		static MyFakes()
		{
			QUICK_LAUNCH = null;
			ALT_AS_DEBUG_KEY = true;
			ENABLE_F12_MENU = false;
			ENABLE_LONG_DISTANCE_GIZMO_DRAWING = false;
			ENABLE_MENU_VIDEO_BACKGROUND = true;
			ENABLE_SPLASHSCREEN = true;
			FakeTarget = null;
			ENABLE_EDGES = true;
			ENABLE_GRAVITY_PHANTOM = true;
			ENABLE_INFINITE_REACTOR_FUEL = false;
			ENABLE_BATTERY_SELF_RECHARGE = false;
			MANUAL_CULL_OBJECTS = true;
			ENABLE_JOYSTICK_SETTINGS = true;
			UNLIMITED_CHARACTER_BUILDING = false;
			DETECT_DISCONNECTS = true;
			DRAW_GUI_SCREEN_BORDERS = false;
			ENABLE_SLOW_WINDOW_TRANSITION_ANIMATIONS = false;
			SHOW_DAMAGE_EFFECTS = true;
			THRUST_FORCE_RATIO = 1f;
			DEFORMATION_LOGGING = false;
			DEFORMATION_MINIMUM_VELOCITY = 2f;
			DEFORMATION_MIN_BREAK_IMPULSE = 10000f;
			DEFORMATION_FORCED_VELOCITY = 40f;
			DEFORMATION_MASS_THR = 10000f;
			DEFORMATION_OFFSET_RATIO = 0.3f;
			DEFORMATION_OFFSET_MAX = 5f;
			DEFORMATION_PROJECTILE_OFFSET_RATIO = 0.00664f;
			DEFORMATION_DRILL_OFFSET_RATIO = 0.00332f;
			DRILL_DAMAGE = 60f;
			DEFORMATION_APPLY_IMPULSE = false;
			DEFORMATION_IMPULSE_FACTOR = 0.05f;
			DEFORMATION_VELOCITY_RELAY = 0.8f;
			DEFORMATION_VELOCITY_RELAY_STATIC = 0.2f;
			DEFORMATION_EXPLOSIONS = true;
			DEFORMATION_VOXEL_CUTOUT_MULTIPLIER = 2.2f;
			DEFORMATION_VOXEL_CUTOUT_MULTIPLIER_SMALL_GRID = 2.6f;
			DEFORMATION_DAMAGE_MULTIPLIER = 1.1f;
			DEFORMATION_VOXEL_CUTOUT_MAX_RADIUS = 20f;
			ENABLE_AUTOSAVE = true;
			ENABLE_TRANSPARENT_CUBE_BUILDER = true;
			HIDE_ENGINEER_TOOL_HIGHLIGHT = false;
			ENABLE_SIMPLE_GRID_PHYSICS = false;
			FORCE_SOFTWARE_MOUSE_DRAW = false;
			ENABLE_GATLING_TURRETS = true;
			ENABLE_MISSILE_TURRETS = true;
			ENABLE_INTERIOR_TURRETS = true;
			ENABLE_DAMAGED_COMPONENTS = false;
			CHARACTER_CAN_DIE_EVEN_IN_CREATIVE_MODE = false;
			ENABLE_EXPORT_MTL_DIAGNOSTICS = false;
			SHOW_INVALID_TRIANGLES = false;
			ENABLE_NEW_COLLISION_AVOIDANCE = true;
			ENABLE_NEW_SOUNDS = true;
			ENABLE_NEW_SOUNDS_QUICK_UPDATE = true;
			ENABLE_NEW_SMALL_SHIP_SOUNDS = true;
			ENABLE_NEW_LARGE_SHIP_SOUNDS = true;
			ENABLE_MUSIC_CONTROLLER = true;
			ENABLE_REALISTIC_LIMITER = false;
			ENABLE_REALISTIC_ON_TOUCH = false;
			ENABLE_NON_PUBLIC_BLOCKS = false;
			ENABLE_NON_PUBLIC_CATEGORY_CLASSES = false;
			ENABLE_NON_PUBLIC_GUI_ELEMENTS = false;
			ENABLE_CHARACTER_AND_DEBRIS_COLLISIONS = false;
			SIMULATE_SLOW_UPDATE = false;
			SHOW_AUDIO_DEV_SCREEN = false;
			ENABLE_SCRAP = true;
			SIMULATE_NO_SOUND_CARD = false;
			INACTIVE_THRUSTER_DMG = true;
			ENABLE_CARGO_SHIPS = true;
			ENABLE_METEOR_SHOWERS = true;
			SHOW_INVENTORY_ITEM_IDS = false;
			SIMULATE_QUICK_TRIGGER = false;
			SIMULATION_SPEED = 1f;
			TEST_PREFABS_FOR_INCONSISTENCIES = false;
			SHOW_PRODUCTION_QUEUE_ITEM_IDS = false;
			ENABLE_CONNECT_COMMAND_LINE = true;
			ENABLE_WHEEL_CONTROLS_IN_COCKPIT = true;
			TEST_NEWS = false;
			ENABLE_TRASH_REMOVAL = true;
			SHOW_FACTIONS_GUI = true;
			ENABLE_RADIO_HUD = true;
			ENABLE_REMOVED_VOXEL_CONTENT_HACK = true;
			ENABLE_AUTO_HEAL = false;
			ENABLE_CENTER_OF_MASS = true;
			ENABLE_VIDEO_PLAYER = true;
			ENABLE_COPY_GROUP = true;
			LANDING_GEAR_BREAKABLE = false;
			ENABLE_WORKSHOP_MODS = true;
			ENABLE_SHIP_BLOCKS_TOOLBAR = true;
			SKIP_VOXELS_DURING_LOAD = false;
			ENABLE_TERMINAL_PROPERTIES = true;
			ENABLE_GYRO_OVERRIDE = true;
			TEST_MODELS = false;
			TEST_MODELS_WRONG_TRIANGLES = false;
			DISABLE_SOUND_POOLING = false;
			ENABLE_GRAVITY_GENERATOR_SPHERE = true;
			ENABLE_REMOTE_CONTROL = true;
			ENABLE_DAMPENERS_OVERRIDE = true;
			ENABLE_BLOCK_PLACEMENT_ON_VOXEL = false;
			ENABLE_COMPOUND_BLOCKS = false;
			ENABLE_SCRIPTS = true;
			ENABLE_SCRIPTS_PDB = false;
			ENABLE_TURRET_CONTROL = true;
			ENABLE_WELDER_HELP_OTHERS = true;
			ENABLE_MISSION_SCREEN = false;
			ENABLE_OBJECTIVE_LINE = true;
			ENABLE_SUBBLOCKS = false;
			ENABLE_MULTIBLOCKS = false;
			ENABLE_MULTIBLOCKS_IN_SURVIVAL = false;
			ENABLE_MULTIBLOCK_PART_IDS = false;
			ENABLE_MULTIBLOCK_CONSTRUCTION = false;
			ENABLE_STATIC_SMALL_GRID_ON_LARGE = false;
			ENABLE_LARGE_OFFSET = false;
			ENABLE_PROJECTOR_BLOCK = true;
			ENABLE_ASTEROID_FIELDS = true;
			ENABLE_USE_NEW_OBJECT_HIGHLIGHT = true;
			MAX_PRECALC_TIME_IN_MILLIS = 20f;
			ENABLE_YIELDING_IN_PRECALC_TASK = false;
			LOAD_UNCONTROLLED_CHARACTERS = true;
			ENABLE_PREFAB_THROWER = true;
			ENABLE_BLOCK_PLACING_IN_OCCUPIED_AREA = false;
			ENABLE_DYNAMIC_SMALL_GRID_MERGING = false;
			ENABLE_ENVIRONMENT_ITEMS = true;
			ENABLE_BARBARIANS = false;
			DEBUG_DRAW_NAVMESH_PROCESSED_VOXEL_CELLS = false;
			DEBUG_DRAW_NAVMESH_PREPARED_VOXEL_CELLS = false;
			DEBUG_DRAW_NAVMESH_CELLS_ON_PATHS = false;
			REMOVE_VOXEL_NAVMESH_CELLS = true;
			DEBUG_DRAW_VOXEL_CONNECTION_HELPER = false;
			DEBUG_DRAW_FOUND_PATH = false;
			DEBUG_DRAW_FUNNEL = false;
			DEBUG_DRAW_NAVMESH_CELL_BORDERS = false;
			DEBUG_DRAW_NAVMESH_HIERARCHY = false;
			DEBUG_DRAW_NAVMESH_HIERARCHY_LITE = false;
			DEBUG_DRAW_NAVMESH_EXPLORED_HL_CELLS = false;
			DEBUG_DRAW_NAVMESH_FRINGE_HL_CELLS = false;
			DEBUG_DRAW_NAVMESH_LINKS = false;
			SHOW_PATH_EXPANSION_ASSERTS = false;
			DEBUG_ONE_AI_STEP_SETTING = false;
			DEBUG_ONE_AI_STEP = false;
			DEBUG_ONE_VOXEL_PATHFINDING_STEP_SETTING = false;
			DEBUG_ONE_VOXEL_PATHFINDING_STEP = false;
			DEBUG_BEHAVIOR_TREE = false;
			DEBUG_BEHAVIOR_TREE_ONE_STEP = false;
			LOG_NAVMESH_GENERATION = false;
			REPLAY_NAVMESH_GENERATION = false;
			REPLAY_NAVMESH_GENERATION_TRIGGER = false;
			ENABLE_AFTER_REPLACE_BODY = true;
			ENABLE_BLOCK_PLACING_ON_INTERSECTED_POSITION = false;
			ENABLE_COMMUNICATION = true;
			ENABLE_GUI_HIDDEN_CUBEBLOCKS = true;
			ENABLE_BLOCK_STAGES = true;
			SHOW_REMOVE_GIZMO = true;
			ENABLE_PROGRAMMABLE_BLOCK = true;
			ENABLE_DESTRUCTION_EFFECTS = true;
			ENABLE_COLLISION_EFFECTS = true;
			ENABLE_PHYSICS_HIGH_FRICTION = false;
			PHYSICS_HIGH_FRICTION = 0.7f;
			REUSE_OLD_PLAYER_IDENTITY = true;
			ENABLE_MOD_CATEGORIES = true;
			ENABLE_GENERATED_BLOCKS = false;
			ENABLE_HAVOK_MULTITHREADING = true;
			ENABLE_HAVOK_PARALLEL_SCHEDULING = true;
			ENABLE_HAVOK_STEP_OPTIMIZERS = true;
			ENABLE_HAVOK_STEP_OPTIMIZERS_TIMINGS = false;
			ENABLE_GPS = true;
			SHOW_FORBIDDEN_ENITIES_VOXEL_HAND = true;
			ENABLE_WEAPON_TERMINAL_CONTROL = true;
			ENABLE_WAIT_UNTIL_CLIPMAPS_READY = true;
			SHOW_MISSING_DESTRUCTION = false;
			ENABLE_DEBUG_DRAW_TEXTURE_NAMES = false;
			ENABLE_GRID_SYSTEM_UPDATE = true;
			ENABLE_GRID_SYSTEM_ONCE_BEFORE_FRAME_UPDATE = ENABLE_GRID_SYSTEM_UPDATE;
			REDUCE_FRACTURES_COUNT = false;
			REMOVE_GENERATED_BLOCK_FRACTURES = true;
			ENABLE_TOOL_SHAKE = true;
			OVERRIDE_LANDING_GEAR_INERTIA = false;
			LANDING_GEAR_INTERTIA = 0.1f;
			ASSERT_CHANGES_IN_SIMULATION = false;
			USE_HAVOK_MODELS = false;
			LAZY_LOAD_DESTRUCTION = true;
			ENABLE_STANDARD_AXES_ROTATION = false;
			ENABLE_ARMOR_HAND = false;
			ASSERT_NON_PUBLIC_BLOCKS = false;
			REMOVE_NON_PUBLIC_BLOCKS = false;
			ENABLE_ROTATION_HINTS = true;
			ENABLE_NOTIFICATION_BLOCK_NOT_AVAILABLE = true;
			FORCE_CLUSTER_REORDER = false;
			PAUSE_PHYSICS = false;
			STEP_PHYSICS = false;
			VDB_ENTITY = null;
			ENABLE_VOXEL_PHYSICS_SHAPE_DISCARDING = true;
			ENABLE_SMALL_BLOCK_TO_LARGE_STATIC_CONNECTIONS = false;
			ENABLE_LARGE_STATIC_GROUP_COPY_FIRST = false;
			CLONE_SHAPES_ON_WORKER = true;
			FRACTURED_BLOCK_AABB_MOUNT_POINTS = true;
			ENABLE_BLOCK_COLORING = true;
			CHANGE_BLOCK_CONVEX_RADIUS = true;
			ENABLE_TEST_BLOCK_CONNECTIVITY_CHECK = false;
			ENABLE_CUSTOM_CHARACTER_IMPACT = false;
			ENABLE_FOOT_IK = false;
			ENABLE_JETPACK_IN_SURVIVAL = true;
			ENABLE_FOOT_IK_USE_HAVOK_RAYCAST = true;
			DEVELOPMENT_PRESET = false;
			SHOW_CURRENT_VOXEL_MAP_AABB_IN_VOXEL_HAND = true;
			ENABLE_OXYGEN_SOUNDS = true;
			ENABLE_ROPE_UNWINDING_TORQUE = false;
			ENABLE_BONES_AND_ANIMATIONS_DEBUG = false;
			ENABLE_RAGDOLL = true;
			ENABLE_RAGDOLL_BONES_TRANSLATION = true;
			ENABLE_RAGDOLL_DEFAULT_PROPERTIES = false;
			ENABLE_RAGDOLL_CLIENT_SYNC = false;
			FORCE_RAGDOLL_DEACTIVATION = false;
			ENABLE_RAGDOLL_DEBUG = false;
			ENABLE_JETPACK_RAGDOLL_COLLISIONS = true;
			RAGDOLL_ANIMATION_WEIGHTING = 1.5f;
			RAGDOLL_GRAVITY_MULTIPLIER = 20f;
			ENABLE_STATION_ROTATION = true;
			ENABLE_CONTROLLER_HINTS = true;
			ENABLE_PHYSICS_SETTINGS = false;
			ENABLE_DEFAULT_BLUEPRINTS = false;
			VOICE_CHAT_TARGET_BIT_RATE = 0;
			VOICE_CHAT_MIC_SENSITIVITY = 0.5f;
			VOICE_CHAT_ECHO = false;
			VOICE_CHAT_PLAYBACK_DELAY = 500f;
			ENABLE_VOICE_CHAT_DEBUGGING = false;
			ENABLE_GENERATED_INTEGRITY_FIX = true;
			ENABLE_VOXEL_MAP_AABB_CORNER_TEST = false;
			ENABLE_PERMANENT_SIMULATIONS_COMPUTATION = true;
			ENABLE_ADMIN_SPECTATOR_BUILDING = false;
			ENABLE_MEDIEVAL_INVENTORY = false;
			ENABLE_STATIC_INVENTORY_MASS = false;
			ENABLE_PLANETS = true;
			ENABLE_NEW_TRIGGERS = true;
			ENABLE_USE_OBJECT_CORNERS = true;
			ENABLE_PLANETS_JETPACK_LIMIT_IN_CREATIVE = false;
			ENABLE_STATS_GUI = true;
			WELD_LANDING_GEARS = false;
			ENFORCE_CONTROLLER = false;
			ENABLE_ALL_IN_SURVIVAL = false;
			ENABLE_SURVIVAL_SWITCHING = false;
			ENABLE_DRIVING_PARTICLES = false;
			ENABLE_BLOCKS_IN_VOXELS_TEST = false;
			ENABLE_TURRET_LASERS = false;
			SKIP_BIOME_MAP = false;
			PRIORITIZE_PRECALC_JOBS = true;
			DISABLE_COMPOSITE_MATERIAL = false;
			ENABLE_PLANETARY_CLOUDS = true;
			ENABLE_CLOUD_FOG = false;
			ENVIRONMENT_ITEMS_ONE_INSTANCEBUFFER = false;
			ENABLE_FRACTURE_COMPONENT = false;
			TESTING_VEHICLES = false;
			ENABLE_WALKING_PARTICLES = true;
			ENABLE_HYDROGEN_FUEL = true;
			WELD_PISTONS = true;
			WELD_ROTORS = true;
			SUSPENSION_POWER_RATIO = false;
			TWO_STEP_SIMULATIONS = false;
			WHEEL_SOFTNESS = false;
			WHEEL_ALTERNATIVE_MODELS_ENABLED = false;
			FORCE_SINGLE_WORKER = false;
			FORCE_NO_WORKER = false;
			DISABLE_CLIPBOARD_PLACEMENT_TEST = false;
			ENABLE_LIMITED_CHARACTER_BODY = false;
			ENABLE_VOXEL_COMPUTED_OCCLUSION = false;
			ENABLE_COMPOUND_BLOCK_COLLISION_DUMMIES = false;
			ENABLE_EXTENDED_PLANET_OPTIONS = false;
			ENABLE_JOIN_SCREEN_REMAINING_TIME = false;
			ENABLE_INVENTORY_FIX = true;
			ENABLE_LAZY_VOXEL_PHYSICS = true;
			ENABLE_PLANET_HIERARCHY = true;
			ENABLE_FLORA_COMPONENT_DEBUG = false;
			ENABLE_DEBUG_DRAW_COORD_SYS = false;
			ENABLE_FRACTURE_PIECE_SHAPE_CHECK = false;
			ENABLE_PLANET_FIREFLIES = true;
			ENABLE_DURABILITY_COMPONENT = true;
			SPAWN_SPACE_FAUNA_IN_CREATIVE = true;
			ENABLE_RUN_WITHOUT_STEAM = true;
			PRECISE_SIM_SPEED = false;
			ENABLE_SIMSPEED_LOCKING = false;
			BACKGROUND_OXYGEN = true;
			ENABLE_GATHERING_SMALL_BLOCK_FROM_GRID = false;
			ENABLE_COMPONENT_BLOCKS = true;
			ENABLE_SMALL_GRID_BLOCK_INFO = true;
			ENABLE_SMALL_GRID_BLOCK_COMPONENT_INFO = true;
			ENABLE_BOUNDINGBOX_SHRINKING = true;
			ENABLE_HUD_PICKED_UP_ITEMS = false;
			ENABLE_VR_DRONE_COLLISIONS = false;
			ENABLE_VR_BLOCK_DEFORMATION_RATIO = false;
			ENABLE_VR_FORCE_BLOCK_DESTRUCTIBLE = false;
			ENABLE_VR_REMOTE_CONTROL_WAYPOINTS_FAST_MOVEMENT = false;
			ENABLE_VR_BUILDING = false;
			ENABLE_SEPARATE_USE_AND_PICK_UP_KEY = false;
			ENABLE_USE_DEFAULT_DAMAGE_DECAL = false;
			ENABLE_QUICK_WARDROBE = false;
			ENABLE_TYPES_FROM_MODS = false;
			ENABLE_PRELOAD_DEFINITIONS = true;
			ENABLE_CESTMIR_PATHFINDING = false;
			ENABLE_ROSLYN_SCRIPT_DIAGNOSTICS = false;
			MP_ISLANDS = false;
			MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CHARACTER = true;
			MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CHARACTER_IN_JETPACK = true;
			MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_GRID = true;
			MULTIPLAYER_CLIENT_SIMULATE_CONTROLLED_CAR = false;
			MP_SYNC_CLUSTERTREE = false;
			MULTIPLAYER_PREDICTION_RESET_CLIENT_FALLING_BEHIND = false;
			MULTIPLAYER_SMOOTH_PING = true;
			MULTIPLAYER_SKIP_PREDICTION = false;
			MULTIPLAYER_SKIP_PREDICTION_SUBGRIDS = false;
			MULTIPLAYER_SKIP_ANIMATION = false;
			MULTIPLAYER_EXTRAPOLATION_SMOOTHING = true;
			SNAPSHOTCACHE_HIERARCHY = true;
			WORLD_SNAPSHOTS = false;
			SNAPSHOTS_MECHANICAL_PIVOTS = true;
			MULTIPLAYER_CLIENT_CONSTRAINTS = false;
			WORLD_LOCKING_IN_CLIENTUPDATE = false;
			SLOWDOWN_FACTOR_TORQUE_MULTIPLIER = 5f;
			SLOWDOWN_FACTOR_TORQUE_MULTIPLIER_LARGE_SHIP = 2f;
			ENABLE_MP_DATA_HASHES = false;
			ENABLE_DOUBLED_KINEMATIC = true;
			ENABLE_CPU_PARTICLES = true;
			FORCE_UPDATE_NEWSLETTER_STATUS = false;
			SUN_GLARE = true;
			LEGACY_HUD = false;
			FORCE_CHARTOOLS_1ST_PERSON = false;
			ENABLE_TREES_IN_THE_NEW_PIPE = false;
			ENABLE_ANSEL_IN_MULTIPLAYER = true;
			ANIMATION_UPDATE_DISTANCE = 100f;
			ENABLE_SOUNDS_ASYNC_PRELOAD = false;
			ENABLE_MAIN_MENU_INVENTORY_SCENE = true;
			ENABLE_IME = true;
			ENABLE_MULTICORE_JIT = true;
			ENABLE_NGEN = true;
			FORCE_CHARACTER_2D_SOUND = false;
			TESTING_JUMPDRIVE = false;
			TREE_MESH_FROM_MODEL = true;
			ENABLE_STARDUST_ON_PLANET = false;
			ENABLE_DEBRIS = true;
			DEBUG_DISPLAY_DESTROY_EFFECT_OFFSET = false;
			PRIORITIZED_VICINITY_ASSETS_LOADING = true;
			PRIORITIZED_CUBE_VICINITY_RADIUS = 60.0;
			PRIORITIZED_VOXEL_VICINITY_RADIUS_FAR = 2000.0;
			PRIORITIZED_VOXEL_VICINITY_RADIUS_CLOSE = 32.0;
			PLANET_CRASH_ENABLED = true;
			AUDIO_ENABLE_REVERB = false;
			OPTIMIZE_GRID_UPDATES = true;
			TESTING_TOOL_PLUGIN = false;
			ENABLE_ANIMATED_KINEMATIC_UPDATE = true;
			ENABLE_GRID_PLACEMENT_TEST = true;
			USE_GOODBOT_DEV_SERVER = MyPlatformGameSettings.PUBLIC_BETA_MP_TEST;
			ENABLE_WAIT_UNTIL_MULTIPLAYER_READY = true;
			ENABLE_PRELOAD_CHARACTER_ANIMATIONS = true;
			LOADING_STREAMING_TIMEOUT_ENABLED = true;
			ENABLE_MINIDUMP_SENDING = true;
			COLLECT_SUSPEND_DUMPS = false;
			USE_GPS_AS_FRIENDLY_SPAWN_LOCATIONS = false;
			I_AM_READY_FOR_NEW_BLUEPRINT_SCREEN = true;
			I_AM_READY_FOR_NEW_SCRIPT_SCREEN = true;
			ENABLE_WORKSHOP_PUBLISH = true;
			ENABLE_DRILL_ROCKS = true;
			VOXELHAND_PARALLEL = true;
			OWN_ALL_DLCS = false;
			OWN_ALL_ITEMS = false;
			SWITCH_DLC_FROM = 0u;
			SWITCH_DLC_TO = 0u;
			FORCE_ADD_TRASH_REMOVAL_MENU = false;
			ENABLE_GDPR_MESSAGE = true;
			ENABLE_ASTEROIDS = true;
			ENABLE_GRID_STORAGE_TEST_FUNCTIONS = false;
			ENABLE_STATION_GENERATOR_DEBUG_DRAW = false;
			ENABLE_RELATION_GENERATOR_DEBUG_DRAW = false;
			ENABLE_ZONE_CHIP_REQ = true;
			CONTRACT_ESCORT_DEBUGDRAW = false;
			CONV_PULL_CACL_IMMIDIATLY_STORE_SAFEZONE = true;
			ENABLE_AREA_INTERACTIONS = true;
			ENABLE_AREA_INTERACTIONS_BLOCKS = false;
			NETWORK_SINGLE_THREADED = false;
			ENABLE_SMART_UPDATER = true;
			ENABLE_ECONOMY_ANALYTICS = false;
			ENABLE_BOULDER_NAME_PARSING = false;
			ENABLE_DOOR_SAFETY = true;
			ENABLE_DOOR_SAFETY_2 = false;
			USE_PARALLEL_TOOL_RAYCAST = false;
			ENABLE_MAGBOOTS_ON_GRID_SMOOTHING = true;
			ENABLE_PERFORMANCELOGGING = false;
			ForcePlayoutDelayBuffer = false;
			ENABLE_CHARACTER_IK_FEET_OFFSET = true;
			CHARACTER_ANIMATION_SPEED = 1f;
			CHARACTER_ANKLE_HEIGHT = 0.187f;
			CHARACTER_FOOTS_DEBUG_DRAW = false;
			RECORD_CHARACTER_FOOT_ANIMATION = false;
			HIDE_CROSSHAIR_OPTIONS = true;
			SHOW_MATCH_STOP = false;
			ENABLE_DYNAMIC_EFFECTIVE_RANGE = true;
<<<<<<< HEAD
			ENABLE_AMMO_DETONATION = true;
			FORCE_HARDCODED_GAMEPAD_CONTROLS = false;
			PROJECTILES_APPLY_RAYCAST_OPTIMIZATION = true;
			PROJECTILES_APPLY_GRAVITY = true;
			PROJECTILES_EARLY_EXIT_CHECK = false;
			LEAD_INDICATOR_ITERATIVE_PREDICTION = false;
			LEAD_INDICATOR_DEBUGDRAW_BULLET_DROP = false;
			LEAD_DEBUGDRAW_MISSILE_PREDICTION_SHORTCUTS_ENABLED = false;
			ENABLE_TARGET_LOCKING_RAYCAST_VOXEL_PHYSICS_PREFETCH = true;
			ENABLE_HAND_ANIMATIONS_CALCULATION_ON_DS = false;
			TARGETING_DISTANCE_MODIFIER_ENABLED = true;
			TARGETING_DISTANCE_MODIFIER_POWER = 3.5f;
			TARGETING_DISTANCE_MODIFIER_POWER_LIMIT = 20f;
			ENABLE_TARGETING_CHANCE_DRAW = false;
			DrawSolarOcclusionOnce = false;
			ENABLE_MISSILES_NATURAL_GRAVITY = true;
			ENABLE_COLDSTART_ASSEMBLY_LOADING = false;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			RuntimeHelpers.RunClassConstructor(typeof(MyFakesLocal).TypeHandle);
		}
	}
}
