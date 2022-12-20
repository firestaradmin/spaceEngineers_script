using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using ProtoBuf;
using Sandbox.Engine.Platform.VideoMode;
using Sandbox.Game;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.Game.ObjectBuilders.Gui;
using VRage.Network;
using VRage.Serialization;
using VRage.Utils;
using VRageMath;
using VRageRender;

namespace Sandbox.Engine.Utils
{
	/// <summary>
	///  This class encapsulated read/write access to our config file - xxx.cfg - stored in user's local files
	///  It assumes that config file may be non existing, or that some values may be missing or in wrong format - this class can handle it
	///  and in such case will offer default values -&gt; BUT YOU HAVE TO HELP IT... HOW? -&gt; when writing getter from a new property,
	///  you have to return default value in case it's null or empty or invalid!!
	///  IMPORTANT: Never call get/set on this class properties from real-time code (during gameplay), e.g. don't do AddCue2D(cueEnum, MyConfig.VolumeMusic)
	///  IMPORTANT: Only from loading and initialization methods.
	/// </summary>
	public class MyConfig : MyConfigBase, IMyConfig
	{
		public enum LowMemSwitch
		{
			ARMED,
			TRIGGERED,
			USER_SAID_NO
		}

		public enum NewsletterStatus
		{
			Unknown,
			NoFeedback,
			NotInterested,
			EmailNotConfirmed,
			EmailConfirmed
		}

		public enum WelcomeScreenStatus
		{
			NotSeen,
			AlreadySeen
		}

		public enum CrosshairSwitch
		{
			VisibleWithHud,
			AlwaysVisible,
			AlwaysHidden
		}

		[ProtoContract]
		public struct MyDebugInputData
		{
			protected class Sandbox_Engine_Utils_MyConfig_003C_003EMyDebugInputData_003C_003EEnabled_003C_003EAccessor : IMemberAccessor<MyDebugInputData, bool>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyDebugInputData owner, in bool value)
				{
					owner.Enabled = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyDebugInputData owner, out bool value)
				{
					value = owner.Enabled;
				}
			}

			protected class Sandbox_Engine_Utils_MyConfig_003C_003EMyDebugInputData_003C_003ESerializedData_003C_003EAccessor : IMemberAccessor<MyDebugInputData, string>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyDebugInputData owner, in string value)
				{
					owner.SerializedData = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyDebugInputData owner, out string value)
				{
					value = owner.SerializedData;
				}
			}

			protected class Sandbox_Engine_Utils_MyConfig_003C_003EMyDebugInputData_003C_003EData_003C_003EAccessor : IMemberAccessor<MyDebugInputData, object>
			{
				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Set(ref MyDebugInputData owner, in object value)
				{
					owner.Data = value;
				}

				[MethodImpl(MethodImplOptions.AggressiveInlining)]
				public sealed override void Get(ref MyDebugInputData owner, out object value)
				{
					value = owner.Data;
				}
			}

			private class Sandbox_Engine_Utils_MyConfig_003C_003EMyDebugInputData_003C_003EActor : IActivator, IActivator<MyDebugInputData>
			{
				private sealed override object CreateInstance()
				{
					return default(MyDebugInputData);
				}

				object IActivator.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}

				private sealed override MyDebugInputData CreateInstance()
				{
					return (MyDebugInputData)(object)default(MyDebugInputData);
				}

				MyDebugInputData IActivator<MyDebugInputData>.CreateInstance()
				{
					//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
					return this.CreateInstance();
				}
			}

			[ProtoMember(1)]
			public bool Enabled;

			[ProtoMember(4)]
			public string SerializedData;

			public object Data
			{
				get
				{
					return Decode64AndDeserialize(SerializedData);
				}
				set
				{
					SerializedData = SerialiazeAndEncod64(value);
				}
			}

			public bool ShouldSerializeData()
			{
				return false;
			}
		}

		private readonly string MODEL_QUALITY = "ModelQuality";

		private readonly string VOXEL_QUALITY = "VoxelQuality";

		private readonly string FIELD_OF_VIEW = "FieldOfView";

		private readonly string ENABLE_DAMAGE_EFFECTS = "EnableDamageEffects";

		private readonly string SCREEN_WIDTH = "ScreenWidth";

		private readonly string SCREEN_HEIGHT = "ScreenHeight";

		private readonly string FULL_SCREEN = "FullScreen";

		private readonly string VIDEO_ADAPTER = "VideoAdapter";

		private readonly string DISABLE_UPDATE_DRIVER_NOTIFICATION = "DisableUpdateDriverNotification";

		private readonly string VERTICAL_SYNC = "VerticalSync";

		private readonly string REFRESH_RATE = "RefreshRate";

		private readonly string FLARES_INTENSITY = "FlaresIntensity";

		private readonly string GAME_VOLUME = "GameVolume";

		private readonly string MUSIC_VOLUME_OLD = "MusicVolume";

		private readonly string MUSIC_VOLUME = "Music_Volume";

		private readonly string VOICE_CHAT_VOLUME = "VoiceChatVolume";

		private readonly string LANGUAGE = "Language";

		private readonly string SKIN = "Skin";

		private readonly string EXPERIMENTAL_MODE = "ExperimentalMode";

		private readonly string CONTROLS_HINTS = "ControlsHints";

		private readonly string GOODBOT_HINTS = "GoodBotHints";

		private readonly string NEW_NEW_GAME_SCREEN = "NewNewGameScreen";

		private readonly string ROTATION_HINTS = "RotationHints";

		private readonly string SHOW_CROSSHAIR = "ShowCrosshair2";

		private readonly string ENABLE_TRADING = "EnableTrading";

		private readonly string ENABLE_STEAM_CLOUD = "EnableSteamCloud";

		private readonly string CONTROLS_GENERAL = "ControlsGeneral";

		private readonly string CONTROLS_BUTTONS = "ControlsButtons";

		private readonly string SCREENSHOT_SIZE_MULTIPLIER = "ScreenshotSizeMultiplier";

		private readonly string FIRST_TIME_RUN = "FirstTimeRun";

		private readonly string FIRST_VT_TIME_RUN = "FirstVTTimeRun";

		private readonly string FIRST_TIME_TUTORIALS = "FirstTimeTutorials";

		private readonly string SYNC_RENDERING = "SyncRendering";

		private readonly string NEED_SHOW_BATTLE_TUTORIAL_QUESTION = "NeedShowBattleTutorialQuestion";

		private readonly string DEBUG_INPUT_COMPONENTS = "DebugInputs";

		private readonly string DEBUG_INPUT_COMPONENTS_INFO = "DebugComponentsInfo";

		private readonly string HUD_STATE = "HudState";

		private readonly string MEMORY_LIMITS = "MemoryLimits";

		private readonly string CUBE_BUILDER_USE_SYMMETRY = "CubeBuilderUseSymmetry";

		private readonly string CUBE_BUILDER_BUILDING_MODE = "CubeBuilderBuildingMode";

		private readonly string CUBE_BUILDER_ALIGN_TO_DEFAULT = "CubeBuilderAlignToDefault";

		private readonly string MULTIPLAYER_SHOWCOMPATIBLE = "MultiplayerShowCompatible";

		private readonly string ENABLE_PERFORMANCE_WARNINGS_TEMP = "EnablePerformanceWarningsTempV2";

		private readonly string LAST_CHECKED_VERSION = "LastCheckedVersion";

		private readonly string WINDOW_MODE = "WindowMode";

		private readonly string MOUSE_CAPTURE = "CaptureMouse";

		private readonly string HUD_WARNINGS = "HudWarnings";

		private readonly string DYNAMIC_MUSIC = "EnableDynamicMusic";

		private readonly string SHIP_SOUNDS_SPEED = "ShipSoundsAreBasedOnSpeed";

		private readonly string HQTARGET = "HQTarget";

		private readonly string ANTIALIASING_MODE = "AntialiasingMode";

		private readonly string SHADOW_MAP_RESOLUTION = "ShadowMapResolution";

		private readonly string SHADOW_GPU_QUALITY = "ShadowGPUQuality";

		private readonly string AMBIENT_OCCLUSION_ENABLED = "AmbientOcclusionEnabled";

		private readonly string POSTPROCESSING_ENABLED = "PostProcessingEnabled";

		private readonly string TEXTURE_QUALITY = "TextureQuality";

		private readonly string VOXEL_TEXTURE_QUALITY = "VoxelTextureQuality";

		private readonly string SHADER_QUALITY = "ShaderQuality";

		private readonly string LIGHTS_QUALITY = "LightsQuality";

		private readonly string ANISOTROPIC_FILTERING = "AnisotropicFiltering";

		private readonly string GRASS_DENSITY = "GrassDensity";

		private readonly string GRASS_DRAW_DISTANCE = "GrassDrawDistance";

		private readonly string VEGETATION_DISTANCE = "TreeViewDistance";

		private readonly string GRAPHICS_RENDERER = "GraphicsRenderer";

		private readonly string ENABLE_VOICE_CHAT = "VoiceChat";

		private readonly string ENABLE_MUTE_WHEN_NOT_IN_FOCUS = "EnableMuteWhenNotInFocus";

		private readonly string ENABLE_REVERB = "EnableReverb";

		private readonly string UI_TRANSPARENCY = "UiTransparency";

		private readonly string UI_BK_TRANSPARENCY = "UiBkTransparency";

		private readonly string HUD_BK_TRANSPARENCY = "HUDBkTransparency";

		private readonly string TUTORIALS_FINISHED = "TutorialsFinished";

		private readonly string MUTED_PLAYERS = "MutedPlayers";

		private readonly string LOW_MEM_SWITCH_TO_LOW = "LowMemSwitchToLow";

		private readonly string NEWSLETTER_CURRENT_STATUS = "NewsletterCurrentStatus";

		private readonly string SERVER_SEARCH_SETTINGS = "ServerSearchSettings";

		private readonly string ENABLE_DOPPLER = "EnableDoppler";

		private readonly string WELCOMESCREEN_CURRENT_STATUS = "WelcomeScreenCurrentStatus";

		private readonly string DEBUG_OVERRIDE_AUTOSAVE = "DebugOverrideAutosave";

		private readonly string GDPR_CONSENT = "GDPRConsent";

		private readonly string GDPR_CONSENT_SENT = "GDPRConsentSentUpdated";

		private readonly string AREA_INTERACTION = "AreaInteraction";

		private readonly string SHOW_CHAT_TIMESTAMP = "ShowChatTimestamp";

		private readonly string GAMEPAD_SCHEME = "GamepadScheme";

<<<<<<< HEAD
		private readonly string GAMEPAD_SCHEME_NAME = "GamepadSchemeName";

		public readonly string MODIO_CONSENT = "ModIoConsent";

		public readonly string STEAM_CONSENT = "SteamConsent";

		private readonly string MIC_SENSITIVITY = "MicrophoneSensitivity";

		private readonly string AUTOMATIC_VOICE_CHAT_ACTIVATION = "VoiceChatVoiceActivation";

		private readonly string SPRITE_MAIN_VIEWPORT_SCALE = "SpriteMainViewportScale";

		private readonly string CAMPAIGNS_STARTED = "CampaignsStarted";

		private readonly string NEW_NEW_GAME_SCREEN_LAST_SELECTION = "NewNewGameScreenLastSelection";

		private readonly string ZOOM_MULTIPLIER = "ZoomMultiplier";

		private readonly string CONTROLLER_DEFAULT_ON_START = "ControllerDefaultOnStart";

=======
		public readonly string MODIO_CONSENT = "ModIoConsent";

		public readonly string STEAM_CONSENT = "SteamConsent";

		private readonly string MIC_SENSITIVITY = "MicrophoneSensitivity";

		private readonly string AUTOMATIC_VOICE_CHAT_ACTIVATION = "VoiceChatVoiceActivation";

		private readonly string SPRITE_MAIN_VIEWPORT_SCALE = "SpriteMainViewportScale";

		private readonly string CAMPAIGNS_STARTED = "CampaignsStarted";

		private readonly string NEW_NEW_GAME_SCREEN_LAST_SELECTION = "NewNewGameScreenLastSelection";

		private readonly string ZOOM_MULTIPLIER = "ZoomMultiplier";

		private readonly string CONTROLLER_DEFAULT_ON_START = "ControllerDefaultOnStart";

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private readonly string IRON_SIGHT_SWITCH_STATE = "IronSightSwitchState";

		private const char m_numberSeparator = ',';

		private HashSet<ulong> m_mutedPlayers;

		private readonly string HIT_INDICATOR_COLOR_CHARACTER = "HitIndicatorColorCharacter";

		private readonly string HIT_INDICATOR_COLOR_HEADSHOT = "HitIndicatorColorHeadshot";

		private readonly string HIT_INDICATOR_COLOR_KILL = "HitIndicatorColorKill";

		private readonly string HIT_INDICATOR_COLOR_GRID = "HitIndicatorColorGrid";

		private readonly string HIT_INDICATOR_COLOR_FRIEND = "HitIndicatorColorFriend";

		private readonly string HIT_INDICATOR_TEXTURE_CHARACTER = "HitIndicatorTextureCharacter";

		private readonly string HIT_INDICATOR_TEXTURE_HEADSHOT = "HitIndicatorTextureHeadshot";

		private readonly string HIT_INDICATOR_TEXTURE_KILL = "HitIndicatorTextureKill";

		private readonly string HIT_INDICATOR_TEXTURE_GRID = "HitIndicatorTextureGrid";

		private readonly string HIT_INDICATOR_TEXTURE_FRIEND = "HitIndicatorTextureFriend";

		public bool FirstTimeRun
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(FIRST_TIME_RUN), defaultValue: true);
			}
			set
			{
				SetParameterValue(FIRST_TIME_RUN, value);
			}
		}

		public bool FirstVTTimeRun
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(FIRST_VT_TIME_RUN), defaultValue: true);
			}
			set
			{
				SetParameterValue(FIRST_VT_TIME_RUN, value);
			}
		}

		public bool FirstTimeTutorials
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(FIRST_TIME_TUTORIALS), defaultValue: true);
			}
			set
			{
				SetParameterValue(FIRST_TIME_TUTORIALS, value);
			}
		}

		public bool SyncRendering
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(SYNC_RENDERING), defaultValue: false);
			}
			set
			{
				SetParameterValue(SYNC_RENDERING, value);
			}
		}

		public bool NeedShowBattleTutorialQuestion
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(NEED_SHOW_BATTLE_TUTORIAL_QUESTION), defaultValue: true);
			}
			set
			{
				SetParameterValue(NEED_SHOW_BATTLE_TUTORIAL_QUESTION, value);
			}
		}

		public MyRenderQualityEnum? ModelQuality
		{
			get
			{
				return GetOptionalEnum<MyRenderQualityEnum>(MODEL_QUALITY);
			}
			set
			{
				SetOptionalEnum(MODEL_QUALITY, value);
			}
		}

		public MyRenderQualityEnum? VoxelQuality
		{
			get
			{
				return GetOptionalEnum<MyRenderQualityEnum>(VOXEL_QUALITY);
			}
			set
			{
				SetOptionalEnum(VOXEL_QUALITY, value);
			}
		}

		public float? GrassDensityFactor
		{
			get
			{
				float floatFromString = MyUtils.GetFloatFromString(GetParameterValue(GRASS_DENSITY), -1f);
				if (floatFromString < 0f)
				{
					return null;
				}
				return floatFromString;
			}
			set
			{
				if (value.HasValue)
				{
					SetParameterValue(GRASS_DENSITY, value.Value);
				}
				else
				{
					m_values.Dictionary.Remove(GRASS_DENSITY);
				}
			}
		}

		public float? GrassDrawDistance
		{
			get
			{
				float floatFromString = MyUtils.GetFloatFromString(GetParameterValue(GRASS_DRAW_DISTANCE), -1f);
				if (floatFromString < 0f)
				{
					return null;
				}
				return floatFromString;
			}
			set
			{
				if (value.HasValue)
				{
					SetParameterValue(GRASS_DRAW_DISTANCE, value.Value);
				}
				else
				{
					m_values.Dictionary.Remove(GRASS_DRAW_DISTANCE);
				}
			}
		}

		public float? VegetationDrawDistance
		{
			get
			{
				return MyUtils.GetFloatFromString(GetParameterValue(VEGETATION_DISTANCE));
			}
			set
			{
				if (value.HasValue)
				{
					SetParameterValue(VEGETATION_DISTANCE, value.Value);
				}
				else
				{
					m_values.Dictionary.Remove(VEGETATION_DISTANCE);
				}
			}
		}

		public float FieldOfView
		{
			get
			{
				float? floatFromString = MyUtils.GetFloatFromString(GetParameterValue(FIELD_OF_VIEW));
				if (floatFromString.HasValue)
				{
					return MathHelper.ToRadians(floatFromString.Value);
				}
				return MyConstants.FIELD_OF_VIEW_CONFIG_DEFAULT;
			}
			set
			{
				SetParameterValue(FIELD_OF_VIEW, MathHelper.ToDegrees(value));
			}
		}

		public bool? HqTarget
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(HQTARGET));
			}
			set
			{
				SetParameterValue(HQTARGET, value);
			}
		}

		public MyAntialiasingMode? AntialiasingMode
		{
			get
			{
				return GetOptionalEnum<MyAntialiasingMode>(ANTIALIASING_MODE);
			}
			set
			{
				SetOptionalEnum(ANTIALIASING_MODE, value);
			}
		}

		public MyShadowsQuality? ShadowQuality
		{
			get
			{
				return GetOptionalEnum<MyShadowsQuality>(SHADOW_MAP_RESOLUTION);
			}
			set
			{
				SetOptionalEnum(SHADOW_MAP_RESOLUTION, value);
			}
		}

		public bool? AmbientOcclusionEnabled
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(AMBIENT_OCCLUSION_ENABLED));
			}
			set
			{
				SetParameterValue(AMBIENT_OCCLUSION_ENABLED, value);
			}
		}

		public bool PostProcessingEnabled
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(POSTPROCESSING_ENABLED), defaultValue: true);
			}
			set
			{
				SetParameterValue(POSTPROCESSING_ENABLED, value);
			}
		}

		public MyTextureQuality? TextureQuality
		{
			get
			{
				return GetOptionalEnum<MyTextureQuality>(TEXTURE_QUALITY);
			}
			set
			{
				SetOptionalEnum(TEXTURE_QUALITY, value);
			}
		}

		public MyTextureQuality? VoxelTextureQuality
		{
			get
			{
				return GetOptionalEnum<MyTextureQuality>(VOXEL_TEXTURE_QUALITY);
			}
			set
			{
				SetOptionalEnum(VOXEL_TEXTURE_QUALITY, value);
			}
		}

		public MyRenderQualityEnum? ShaderQuality
		{
			get
			{
				return GetOptionalEnum<MyRenderQualityEnum>(SHADER_QUALITY);
			}
			set
			{
				SetOptionalEnum(SHADER_QUALITY, value);
			}
		}

		public MyTextureAnisoFiltering? AnisotropicFiltering
		{
			get
			{
				return GetOptionalEnum<MyTextureAnisoFiltering>(ANISOTROPIC_FILTERING);
			}
			set
			{
				SetOptionalEnum(ANISOTROPIC_FILTERING, value);
			}
		}

		public MyRenderQualityEnum? LightsQuality
		{
			get
			{
				return GetOptionalEnum<MyRenderQualityEnum>(LIGHTS_QUALITY);
			}
			set
			{
				SetOptionalEnum(LIGHTS_QUALITY, value);
			}
		}

		public int? ScreenWidth
		{
			get
			{
				return MyUtils.GetInt32FromString(GetParameterValue(SCREEN_WIDTH));
			}
			set
			{
				SetParameterValue(SCREEN_WIDTH, value);
			}
		}

		public int? ScreenHeight
		{
			get
			{
				return MyUtils.GetInt32FromString(GetParameterValue(SCREEN_HEIGHT));
			}
			set
			{
				SetParameterValue(SCREEN_HEIGHT, value);
			}
		}

		public int VideoAdapter
		{
			get
			{
				return MyUtils.GetIntFromString(GetParameterValue(VIDEO_ADAPTER), 0);
			}
			set
			{
				SetParameterValue(VIDEO_ADAPTER, value);
			}
		}

		public bool DisableUpdateDriverNotification
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(DISABLE_UPDATE_DRIVER_NOTIFICATION), defaultValue: false);
			}
			set
			{
				SetParameterValue(DISABLE_UPDATE_DRIVER_NOTIFICATION, value);
			}
		}

		public MyWindowModeEnum WindowMode
		{
			get
			{
				string parameterValue = GetParameterValue(WINDOW_MODE);
				byte? b = null;
				if (!string.IsNullOrEmpty(parameterValue))
				{
					b = MyUtils.GetByteFromString(parameterValue);
				}
				else
				{
					bool? boolFromString = MyUtils.GetBoolFromString(GetParameterValue(FULL_SCREEN));
					if (boolFromString.HasValue)
					{
						RemoveParameterValue(FULL_SCREEN);
						b = (byte)(boolFromString.Value ? 2 : 0);
						SetParameterValue(WINDOW_MODE, b.Value);
					}
				}
				if (!b.HasValue || !Enum.IsDefined(typeof(MyWindowModeEnum), b))
				{
					return MyWindowModeEnum.Fullscreen;
				}
				return (MyWindowModeEnum)b.Value;
			}
			set
			{
				SetParameterValue(WINDOW_MODE, (int)value);
			}
		}

		public bool CaptureMouse
		{
			get
			{
				if (GetParameterValue(MOUSE_CAPTURE).Equals("False"))
				{
					return false;
				}
				return true;
			}
			set
			{
				SetParameterValue(MOUSE_CAPTURE, value.ToString());
			}
		}

		public int VerticalSync
		{
			get
			{
				return MyUtils.GetIntFromString(GetParameterValue(VERTICAL_SYNC), 1);
			}
			set
			{
				SetParameterValue(VERTICAL_SYNC, value);
			}
		}

		public int RefreshRate
		{
			get
			{
				return MyUtils.GetIntFromString(GetParameterValue(REFRESH_RATE), 0);
			}
			set
			{
				SetParameterValue(REFRESH_RATE, value);
			}
		}

		public float FlaresIntensity
		{
			get
			{
				return MyUtils.GetFloatFromString(GetParameterValue(FLARES_INTENSITY), 1f);
			}
			set
			{
				SetParameterValue(FLARES_INTENSITY, value);
			}
		}

		public bool? EnableDamageEffects
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(ENABLE_DAMAGE_EFFECTS));
			}
			set
			{
				SetParameterValue(ENABLE_DAMAGE_EFFECTS, value);
			}
		}

		public float GameVolume
		{
			get
			{
				return MyUtils.GetFloatFromString(GetParameterValue(GAME_VOLUME), 1f);
			}
			set
			{
				SetParameterValue(GAME_VOLUME, value);
			}
		}

		public float MusicVolume
		{
			get
			{
				float? floatFromString = MyUtils.GetFloatFromString(GetParameterValue(MUSIC_VOLUME_OLD));
				if (floatFromString.HasValue)
				{
					if (floatFromString.Value != 1f)
					{
						SetParameterValue(MUSIC_VOLUME, floatFromString.Value);
					}
					RemoveParameterValue(MUSIC_VOLUME_OLD);
				}
				return MyUtils.GetFloatFromString(GetParameterValue(MUSIC_VOLUME), 0.5f);
			}
			set
			{
				SetParameterValue(MUSIC_VOLUME, value);
			}
		}

		public float VoiceChatVolume
		{
			get
			{
				return MyUtils.GetFloatFromString(GetParameterValue(VOICE_CHAT_VOLUME), 5f);
			}
			set
			{
				SetParameterValue(VOICE_CHAT_VOLUME, value);
			}
		}

		public bool ExperimentalMode
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(EXPERIMENTAL_MODE), defaultValue: true);
			}
			set
			{
				SetParameterValue(EXPERIMENTAL_MODE, value);
			}
		}

		public bool ControlsHints
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(CONTROLS_HINTS), defaultValue: true);
			}
			set
			{
				SetParameterValue(CONTROLS_HINTS, value);
			}
		}

		public bool GoodBotHints
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(GOODBOT_HINTS), defaultValue: true);
			}
			set
			{
				SetParameterValue(GOODBOT_HINTS, value);
			}
		}

		public bool EnableNewNewGameScreen
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(NEW_NEW_GAME_SCREEN), defaultValue: false);
			}
			set
			{
				SetParameterValue(NEW_NEW_GAME_SCREEN, value);
			}
		}

		public bool RotationHints
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(ROTATION_HINTS), defaultValue: true);
			}
			set
			{
				SetParameterValue(ROTATION_HINTS, value);
			}
		}

		public CrosshairSwitch ShowCrosshair
		{
			get
			{
				return (CrosshairSwitch)MyUtils.GetIntFromString(GetParameterValue(SHOW_CROSSHAIR), 0);
			}
			set
			{
				SetParameterValue(SHOW_CROSSHAIR, (int)value);
			}
		}

<<<<<<< HEAD
		public bool ShowCrosshairHUD
=======
		public bool ShowCrosshairHUD => ShowCrosshair switch
		{
			CrosshairSwitch.AlwaysVisible => true, 
			CrosshairSwitch.AlwaysHidden => false, 
			_ => !MyHud.IsHudMinimal, 
		};

		public bool EnableTrading
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		{
			get
			{
				switch (ShowCrosshair)
				{
				case CrosshairSwitch.AlwaysVisible:
					return true;
				case CrosshairSwitch.AlwaysHidden:
					return false;
				default:
					return !MyHud.IsHudMinimal;
				}
			}
		}

		public bool EnableTrading
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(ENABLE_TRADING), defaultValue: true);
			}
			set
			{
				SetParameterValue(ENABLE_TRADING, value);
			}
		}

<<<<<<< HEAD
		public bool ShowChatTimestamp
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(SHOW_CHAT_TIMESTAMP), defaultValue: true);
			}
			set
			{
				SetParameterValue(SHOW_CHAT_TIMESTAMP, value);
			}
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public List<string> CampaignsStarted
		{
			get
			{
				if (!m_values.Dictionary.ContainsKey(CAMPAIGNS_STARTED))
				{
					m_values.Dictionary.Add(CAMPAIGNS_STARTED, new List<string>());
				}
				List<string> list = GetParameterValueT<List<string>>(CAMPAIGNS_STARTED);
				if (list == null)
				{
					list = new List<string>();
					m_values.Dictionary[CAMPAIGNS_STARTED] = list;
				}
				return list;
<<<<<<< HEAD
=======
			}
			set
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public int NewNewGameScreenLastSelection
		{
			get
			{
				return MyUtils.GetIntFromString(GetParameterValue(NEW_NEW_GAME_SCREEN_LAST_SELECTION), -1);
			}
			set
			{
				SetParameterValue(NEW_NEW_GAME_SCREEN_LAST_SELECTION, value);
			}
		}

		public bool EnableSteamCloud
		{
			get
			{
				if (!MyUtils.GetBoolFromString(GetParameterValue(ENABLE_STEAM_CLOUD), defaultValue: false))
				{
					return MyPlatformGameSettings.CLOUD_ALWAYS_ENABLED;
				}
				return true;
			}
			set
			{
				SetParameterValue(ENABLE_STEAM_CLOUD, value);
			}
		}

		public float ScreenshotSizeMultiplier
		{
			get
			{
				if (string.IsNullOrEmpty(GetParameterValue(SCREENSHOT_SIZE_MULTIPLIER)))
				{
					SetParameterValue(SCREENSHOT_SIZE_MULTIPLIER, 1f);
					Save();
				}
				return MyUtils.GetFloatFromString(GetParameterValue(SCREENSHOT_SIZE_MULTIPLIER), 1f);
			}
			set
			{
				SetParameterValue(SCREENSHOT_SIZE_MULTIPLIER, value);
			}
		}

		public MyLanguagesEnum Language
		{
			get
			{
				byte? byteFromString = MyUtils.GetByteFromString(GetParameterValue(LANGUAGE));
				if (!byteFromString.HasValue || !Enum.IsDefined(typeof(MyLanguagesEnum), byteFromString))
				{
					return MyLanguage.GetOsLanguageCurrentOfficial();
				}
				return (MyLanguagesEnum)byteFromString.Value;
			}
			set
			{
				SetParameterValue(LANGUAGE, (int)value);
			}
		}

		public string Skin
		{
			get
			{
				if (string.IsNullOrEmpty(GetParameterValue(SKIN)))
				{
					SetParameterValue(SKIN, "Default");
					Save();
				}
				return GetParameterValue(SKIN);
			}
			set
			{
				SetParameterValue(SKIN, value);
			}
		}

		public SerializableDictionary<string, string> ControlsGeneral => GetParameterValueDictionary<string>(CONTROLS_GENERAL);

		public SerializableDictionary<string, SerializableDictionary<string, string>> ControlsButtons => GetParameterValueDictionary<SerializableDictionary<string, string>>(CONTROLS_BUTTONS);

		public SerializableDictionary<string, MyDebugInputData> DebugInputComponents
		{
			get
			{
				if (!m_values.Dictionary.ContainsKey(DEBUG_INPUT_COMPONENTS))
				{
					m_values.Dictionary.Add(DEBUG_INPUT_COMPONENTS, new SerializableDictionary<string, MyDebugInputData>());
				}
				else if (!(m_values.Dictionary[DEBUG_INPUT_COMPONENTS] is SerializableDictionary<string, MyDebugInputData>))
				{
					m_values.Dictionary[DEBUG_INPUT_COMPONENTS] = new SerializableDictionary<string, MyDebugInputData>();
				}
				return GetParameterValueT<SerializableDictionary<string, MyDebugInputData>>(DEBUG_INPUT_COMPONENTS);
			}
		}

		public MyDebugComponent.MyDebugComponentInfoState DebugComponentsInfo
		{
			get
			{
				int? intFromString = MyUtils.GetIntFromString(GetParameterValue(DEBUG_INPUT_COMPONENTS_INFO));
				if (!intFromString.HasValue || !Enum.IsDefined(typeof(MyDebugComponent.MyDebugComponentInfoState), intFromString))
				{
					return MyDebugComponent.MyDebugComponentInfoState.EnabledInfo;
				}
				return (MyDebugComponent.MyDebugComponentInfoState)intFromString.Value;
			}
			set
			{
				SetParameterValue(DEBUG_INPUT_COMPONENTS_INFO, (int)value);
			}
		}

		public bool MinimalHud
		{
			get
			{
				if (!MyHud.IsHudMinimal)
				{
					return MyHud.MinimalHud;
				}
				return true;
			}
			set
			{
				HudState = ((!value) ? 1 : 0);
			}
		}

		public int HudState
		{
			get
			{
				return MyUtils.GetIntFromString(GetParameterValue(HUD_STATE), 1);
			}
			set
			{
				SetParameterValue(HUD_STATE, value);
			}
		}

		public bool MemoryLimits
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(MEMORY_LIMITS), defaultValue: true);
			}
			set
			{
				SetParameterValue(MEMORY_LIMITS, value);
			}
		}

		public bool CubeBuilderUseSymmetry
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(CUBE_BUILDER_USE_SYMMETRY), defaultValue: true);
			}
			set
			{
				SetParameterValue(CUBE_BUILDER_USE_SYMMETRY, value);
			}
		}

		public int CubeBuilderBuildingMode
		{
			get
			{
				return MyUtils.GetIntFromString(GetParameterValue(CUBE_BUILDER_BUILDING_MODE), 0);
			}
			set
			{
				SetParameterValue(CUBE_BUILDER_BUILDING_MODE, value);
			}
		}

		public bool CubeBuilderAlignToDefault
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(CUBE_BUILDER_ALIGN_TO_DEFAULT), defaultValue: true);
			}
			set
			{
				SetParameterValue(CUBE_BUILDER_ALIGN_TO_DEFAULT, value);
			}
		}

		public bool MultiplayerShowCompatible
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(MULTIPLAYER_SHOWCOMPATIBLE), defaultValue: true);
			}
			set
			{
				SetParameterValue(MULTIPLAYER_SHOWCOMPATIBLE, value);
			}
		}

		public bool EnablePerformanceWarnings
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(ENABLE_PERFORMANCE_WARNINGS_TEMP), defaultValue: true);
			}
			set
			{
				SetParameterValue(ENABLE_PERFORMANCE_WARNINGS_TEMP, value);
			}
		}

		public int LastCheckedVersion
		{
			get
			{
				return MyUtils.GetIntFromString(GetParameterValue(LAST_CHECKED_VERSION), 0);
			}
			set
			{
				SetParameterValue(LAST_CHECKED_VERSION, value);
			}
		}

		public float UIOpacity
		{
			get
			{
				return MyUtils.GetFloatFromString(GetParameterValue(UI_TRANSPARENCY), 1f);
			}
			set
			{
				SetParameterValue(UI_TRANSPARENCY, value);
			}
		}

		public float UIBkOpacity
		{
			get
			{
				return MyUtils.GetFloatFromString(GetParameterValue(UI_BK_TRANSPARENCY), 0.8f);
			}
			set
			{
				SetParameterValue(UI_BK_TRANSPARENCY, value);
			}
		}

		public float HUDBkOpacity
		{
			get
			{
				return MyUtils.GetFloatFromString(GetParameterValue(HUD_BK_TRANSPARENCY), 0.6f);
			}
			set
			{
				SetParameterValue(HUD_BK_TRANSPARENCY, value);
			}
		}

		public List<string> TutorialsFinished
		{
			get
			{
				if (!m_values.Dictionary.ContainsKey(TUTORIALS_FINISHED))
				{
					m_values.Dictionary.Add(TUTORIALS_FINISHED, new List<string>());
				}
				if (m_values.Dictionary[TUTORIALS_FINISHED] != null && m_values.Dictionary[TUTORIALS_FINISHED].GetType() != typeof(List<string>))
				{
					m_values.Dictionary[TUTORIALS_FINISHED] = new List<string>();
				}
				return (List<string>)m_values.Dictionary[TUTORIALS_FINISHED];
<<<<<<< HEAD
=======
			}
			set
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public bool HudWarnings
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(HUD_WARNINGS), defaultValue: true);
			}
			set
			{
				SetParameterValue(HUD_WARNINGS, value);
			}
		}

		public bool EnableVoiceChat
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(ENABLE_VOICE_CHAT), defaultValue: true);
			}
			set
			{
				SetParameterValue(ENABLE_VOICE_CHAT, value);
			}
		}

		public bool EnableMuteWhenNotInFocus
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(ENABLE_MUTE_WHEN_NOT_IN_FOCUS), defaultValue: true);
			}
			set
			{
				SetParameterValue(ENABLE_MUTE_WHEN_NOT_IN_FOCUS, value);
			}
		}

		public bool EnableDynamicMusic
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(DYNAMIC_MUSIC), defaultValue: true);
			}
			set
			{
				SetParameterValue(DYNAMIC_MUSIC, value);
			}
		}

		public bool ShipSoundsAreBasedOnSpeed
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(SHIP_SOUNDS_SPEED), defaultValue: true);
			}
			set
			{
				SetParameterValue(SHIP_SOUNDS_SPEED, value);
			}
		}

		public bool EnableReverb
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(ENABLE_REVERB), defaultValue: true);
			}
			set
			{
				SetParameterValue(ENABLE_REVERB, value);
			}
		}

		public MyStringId GraphicsRenderer
		{
			get
			{
				string parameterValue = GetParameterValue(GRAPHICS_RENDERER);
				if (string.IsNullOrEmpty(parameterValue))
				{
					return MyPerGameSettings.DefaultGraphicsRenderer;
				}
				MyStringId myStringId = MyStringId.TryGet(parameterValue);
				if (myStringId == MyStringId.NullOrEmpty)
				{
					return MyPerGameSettings.DefaultGraphicsRenderer;
				}
				return myStringId;
			}
			set
			{
				SetParameterValue(GRAPHICS_RENDERER, value.ToString());
			}
		}

		public MyObjectBuilder_ServerFilterOptions ServerSearchSettings
		{
			get
			{
				m_values.Dictionary.TryGetValue(SERVER_SEARCH_SETTINGS, out var value);
				return value as MyObjectBuilder_ServerFilterOptions;
			}
			set
			{
				m_values.Dictionary[SERVER_SEARCH_SETTINGS] = value;
			}
		}

		public bool EnableDoppler
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(ENABLE_DOPPLER), defaultValue: true);
			}
			set
			{
				SetParameterValue(ENABLE_DOPPLER, value);
			}
		}

		public HashSet<ulong> MutedPlayers
		{
			get
			{
				return GetSeparatedValues(MUTED_PLAYERS, ref m_mutedPlayers);
			}
			set
			{
				SetSeparatedValues(MUTED_PLAYERS, value, ref m_mutedPlayers);
			}
		}

		public LowMemSwitch LowMemSwitchToLow
		{
			get
			{
				return (LowMemSwitch)MyUtils.GetIntFromString(GetParameterValue(LOW_MEM_SWITCH_TO_LOW), 0);
			}
			set
			{
				SetParameterValue(LOW_MEM_SWITCH_TO_LOW, (int)value);
			}
		}

		public NewsletterStatus NewsletterCurrentStatus
		{
			get
			{
				return (NewsletterStatus)MyUtils.GetIntFromString(GetParameterValue(NEWSLETTER_CURRENT_STATUS), 1);
			}
			set
			{
				SetParameterValue(NEWSLETTER_CURRENT_STATUS, (int)value);
			}
		}

		public WelcomeScreenStatus WelcomScreenCurrentStatus
		{
			get
			{
				return (WelcomeScreenStatus)MyUtils.GetIntFromString(GetParameterValue(WELCOMESCREEN_CURRENT_STATUS), 0);
			}
			set
			{
				SetParameterValue(WELCOMESCREEN_CURRENT_STATUS, (int)value);
			}
		}

		public int GamepadSchemeId
		{
			get
			{
<<<<<<< HEAD
				return MyUtils.GetIntFromString(GetParameterValue(GAMEPAD_SCHEME), -1);
=======
				return MyUtils.GetIntFromString(GetParameterValue(GAMEPAD_SCHEME), 0);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			set
			{
				SetParameterValue(GAMEPAD_SCHEME, value);
			}
		}

<<<<<<< HEAD
		public string GamepadSchemeName
		{
			get
			{
				return GetParameterValue(GAMEPAD_SCHEME_NAME);
			}
			set
			{
				SetParameterValue(GAMEPAD_SCHEME_NAME, value);
			}
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public bool ControllerDefaultOnStart
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(CONTROLLER_DEFAULT_ON_START), defaultValue: false);
			}
			set
			{
				SetParameterValue(CONTROLLER_DEFAULT_ON_START, value);
			}
		}

		public float ZoomMultiplier
		{
			get
			{
				return MyUtils.GetFloatFromString(GetParameterValue(ZOOM_MULTIPLIER), 0.2f);
			}
			set
			{
				SetParameterValue(ZOOM_MULTIPLIER, value);
			}
		}

		public bool DebugOverrideAutosave
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(DEBUG_OVERRIDE_AUTOSAVE), defaultValue: false);
			}
			set
			{
				SetParameterValue(DEBUG_OVERRIDE_AUTOSAVE, value);
			}
		}

		public bool ModIoConsent
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(MODIO_CONSENT), defaultValue: false);
			}
			set
			{
				SetParameterValue(MODIO_CONSENT, value);
			}
		}

		public bool SteamConsent
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(STEAM_CONSENT), defaultValue: false);
			}
			set
			{
				SetParameterValue(STEAM_CONSENT, value);
			}
		}

		MyTextureAnisoFiltering? IMyConfig.AnisotropicFiltering => AnisotropicFiltering;

		MyAntialiasingMode? IMyConfig.AntialiasingMode => AntialiasingMode;

		bool IMyConfig.ControlsHints => ControlsHints;

		int IMyConfig.CubeBuilderBuildingMode => CubeBuilderBuildingMode;

		bool IMyConfig.CubeBuilderUseSymmetry => CubeBuilderUseSymmetry;

		bool IMyConfig.EnableDamageEffects
		{
			get
			{
				if (EnableDamageEffects.HasValue)
				{
					return EnableDamageEffects.Value;
				}
				return true;
			}
		}

		float IMyConfig.FieldOfView => FieldOfView;

		float IMyConfig.GameVolume => GameVolume;

		bool IMyConfig.HudWarnings => HudWarnings;

		MyLanguagesEnum IMyConfig.Language => Language;

		bool IMyConfig.MemoryLimits => MemoryLimits;

		int IMyConfig.HudState => HudState;

		float IMyConfig.MusicVolume => MusicVolume;

		int IMyConfig.RefreshRate => RefreshRate;

		MyGraphicsRenderer IMyConfig.GraphicsRenderer
		{
			get
			{
				if (MyStringId.TryGet(GetParameterValue(GRAPHICS_RENDERER)) == MySandboxGame.DirectX11RendererKey)
				{
					return MyGraphicsRenderer.DX11;
				}
				return MyGraphicsRenderer.NONE;
			}
		}

		bool IMyConfig.RotationHints => RotationHints;

		int? IMyConfig.ScreenHeight => ScreenHeight;

		int? IMyConfig.ScreenWidth => ScreenWidth;

		MyShadowsQuality? IMyConfig.ShadowQuality => ShadowQuality;

		bool IMyConfig.ShowCrosshair => ShowCrosshair == CrosshairSwitch.AlwaysVisible;

		int IMyConfig.ShowCrosshair2 => (int)ShowCrosshair;

		bool IMyConfig.EnableTrading => EnableTrading;

		MyTextureQuality? IMyConfig.TextureQuality => TextureQuality;

		MyTextureQuality? IMyConfig.VoxelTextureQuality => VoxelTextureQuality;

		int IMyConfig.VerticalSync => VerticalSync;

		int IMyConfig.VideoAdapter => VideoAdapter;

		MyWindowModeEnum IMyConfig.WindowMode => WindowMode;

		bool IMyConfig.CaptureMouse => CaptureMouse;

		bool? IMyConfig.AmbientOcclusionEnabled => AmbientOcclusionEnabled;

		DictionaryReader<string, object> IMyConfig.ControlsButtons => DictionaryReader<string, object>.Empty;

		DictionaryReader<string, object> IMyConfig.ControlsGeneral => DictionaryReader<string, object>.Empty;

		bool IMyConfig.EnableDynamicMusic => EnableDynamicMusic;

		bool IMyConfig.EnableMuteWhenNotInFocus => EnableMuteWhenNotInFocus;

		bool IMyConfig.EnablePerformanceWarnings => EnablePerformanceWarnings;

		bool IMyConfig.EnableReverb => EnableReverb;

		bool IMyConfig.EnableVoiceChat => EnableVoiceChat;

		bool IMyConfig.FirstTimeRun => FirstTimeRun;

		float IMyConfig.FlaresIntensity => FlaresIntensity;

		float? IMyConfig.GrassDensityFactor => GrassDensityFactor;

		float? IMyConfig.GrassDrawDistance => GrassDrawDistance;

		float IMyConfig.HUDBkOpacity => HUDBkOpacity;

		MyRenderQualityEnum? IMyConfig.ModelQuality => ModelQuality;

		HashSetReader<ulong> IMyConfig.MutedPlayers => MutedPlayers;

		float IMyConfig.ScreenshotSizeMultiplier => ScreenshotSizeMultiplier;

		MyRenderQualityEnum? IMyConfig.ShaderQuality => ShaderQuality;

		bool IMyConfig.ShipSoundsAreBasedOnSpeed => ShipSoundsAreBasedOnSpeed;

		string IMyConfig.Skin => Skin;

		float IMyConfig.UIBkOpacity => UIBkOpacity;

		float IMyConfig.UIOpacity => UIOpacity;

		float? IMyConfig.VegetationDrawDistance => VegetationDrawDistance;

		float IMyConfig.VoiceChatVolume => VoiceChatVolume;

		MyRenderQualityEnum? IMyConfig.VoxelQuality => VoxelQuality;

		MyRenderQualityEnum? IMyConfig.LightsQuality => LightsQuality;

		public bool? GDPRConsent
		{
			get
			{
				string parameterValue = GetParameterValue(GDPR_CONSENT);
				if (string.IsNullOrWhiteSpace(parameterValue))
				{
					return null;
				}
				return MyUtils.GetBoolFromString(parameterValue, defaultValue: false);
			}
			set
			{
				SetParameterValue(GDPR_CONSENT, value.Value);
			}
		}

		public bool AreaInteraction
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(AREA_INTERACTION), defaultValue: true);
			}
			set
			{
				SetParameterValue(AREA_INTERACTION, value);
			}
		}

		public bool? GDPRConsentSent
		{
			get
			{
				string parameterValue = GetParameterValue(GDPR_CONSENT_SENT);
				if (string.IsNullOrWhiteSpace(parameterValue))
				{
					return null;
				}
				return MyUtils.GetBoolFromString(parameterValue, defaultValue: false);
			}
			set
			{
				SetParameterValue(GDPR_CONSENT_SENT, value.Value);
			}
		}

		public float MicSensitivity
		{
			get
			{
				return MyUtils.GetFloatFromString(GetParameterValue(MIC_SENSITIVITY), MyFakes.VOICE_CHAT_MIC_SENSITIVITY);
			}
			set
			{
				SetParameterValue(MIC_SENSITIVITY, value);
			}
		}

		public bool AutomaticVoiceChatActivation
		{
			get
			{
				return MyUtils.GetBoolFromString(GetParameterValue(AUTOMATIC_VOICE_CHAT_ACTIVATION), MyPlatformGameSettings.VOICE_CHAT_AUTOMATIC_ACTIVATION);
			}
			set
			{
				SetParameterValue(AUTOMATIC_VOICE_CHAT_ACTIVATION, value);
			}
		}

		public float SpriteMainViewportScale
		{
			get
			{
				return MyUtils.GetFloatFromString(GetParameterValue(SPRITE_MAIN_VIEWPORT_SCALE), 1f);
			}
			set
			{
				SetParameterValue(SPRITE_MAIN_VIEWPORT_SCALE, value);
			}
		}

		public IronSightSwitchStateType IronSightSwitchState
		{
			get
			{
				return (IronSightSwitchStateType)MyUtils.GetIntFromString(GetParameterValue(IRON_SIGHT_SWITCH_STATE), 1);
			}
			set
			{
				SetParameterValue(IRON_SIGHT_SWITCH_STATE, (int)value);
			}
		}

		public Color HitIndicatorColorCharacter
		{
			get
			{
				return new Color(MyUtils.GetUIntFromString(GetParameterValue(HIT_INDICATOR_COLOR_CHARACTER), new Color(149, 169, 179).PackedValue));
			}
			set
			{
				SetParameterValue(HIT_INDICATOR_COLOR_CHARACTER, value.PackedValue);
			}
		}

		public Color HitIndicatorColorHeadshot
		{
			get
			{
				return new Color(MyUtils.GetUIntFromString(GetParameterValue(HIT_INDICATOR_COLOR_HEADSHOT), new Color(232, 179, 35).PackedValue));
			}
			set
			{
				SetParameterValue(HIT_INDICATOR_COLOR_HEADSHOT, value.PackedValue);
			}
		}

		public Color HitIndicatorColorKill
		{
			get
			{
				return new Color(MyUtils.GetUIntFromString(GetParameterValue(HIT_INDICATOR_COLOR_KILL), new Color(228, 62, 62).PackedValue));
			}
			set
			{
				SetParameterValue(HIT_INDICATOR_COLOR_KILL, value.PackedValue);
			}
		}

		public Color HitIndicatorColorGrid
		{
			get
			{
				return new Color(MyUtils.GetUIntFromString(GetParameterValue(HIT_INDICATOR_COLOR_GRID), new Color(117, 201, 241).PackedValue));
			}
			set
			{
				SetParameterValue(HIT_INDICATOR_COLOR_GRID, value.PackedValue);
			}
		}

		public Color HitIndicatorColorFriend
		{
			get
			{
				return new Color(MyUtils.GetUIntFromString(GetParameterValue(HIT_INDICATOR_COLOR_FRIEND), new Color(101, 178, 91).PackedValue));
			}
			set
			{
				SetParameterValue(HIT_INDICATOR_COLOR_FRIEND, value.PackedValue);
			}
		}

		public string HitIndicatorTextureCharacter
		{
			get
			{
				string parameterValue = GetParameterValue(HIT_INDICATOR_TEXTURE_CHARACTER);
				if (string.IsNullOrEmpty(parameterValue))
				{
					return "Textures\\GUI\\Indicators\\HitIndicator4.png";
				}
				return parameterValue;
			}
			set
			{
				SetParameterValue(HIT_INDICATOR_TEXTURE_CHARACTER, value);
			}
		}

		public string HitIndicatorTextureHeadshot
		{
			get
			{
				string parameterValue = GetParameterValue(HIT_INDICATOR_TEXTURE_HEADSHOT);
				if (string.IsNullOrEmpty(parameterValue))
				{
					return "Textures\\GUI\\Indicators\\HitIndicator4.png";
				}
				return parameterValue;
			}
			set
			{
				SetParameterValue(HIT_INDICATOR_TEXTURE_HEADSHOT, value);
			}
		}

		public string HitIndicatorTextureKill
		{
			get
			{
				string parameterValue = GetParameterValue(HIT_INDICATOR_TEXTURE_KILL);
				if (string.IsNullOrEmpty(parameterValue))
				{
					return "Textures\\GUI\\Indicators\\HitIndicator4.png";
				}
				return parameterValue;
			}
			set
			{
				SetParameterValue(HIT_INDICATOR_TEXTURE_KILL, value);
			}
		}

		public string HitIndicatorTextureGrid
		{
			get
			{
				string parameterValue = GetParameterValue(HIT_INDICATOR_TEXTURE_GRID);
				if (string.IsNullOrEmpty(parameterValue))
				{
					return "Textures\\GUI\\Indicators\\HitIndicator4.png";
				}
				return parameterValue;
			}
			set
			{
				SetParameterValue(HIT_INDICATOR_TEXTURE_GRID, value);
			}
		}

		public string HitIndicatorTextureFriend
		{
			get
			{
				string parameterValue = GetParameterValue(HIT_INDICATOR_TEXTURE_FRIEND);
				if (string.IsNullOrEmpty(parameterValue))
				{
					return "Textures\\GUI\\Indicators\\HitIndicator4.png";
				}
				return parameterValue;
			}
			set
			{
				SetParameterValue(HIT_INDICATOR_TEXTURE_FRIEND, value);
			}
		}

		public MyConfig(string fileName)
			: base(fileName)
		{
			RedactedProperties.Add(MUTED_PLAYERS);
		}

		private static string SerialiazeAndEncod64(object p)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (p == null)
			{
				return "";
			}
			MemoryStream memoryStream = new MemoryStream();
			new BinaryFormatter().Serialize((Stream)memoryStream, p);
			return Convert.ToBase64String(memoryStream.GetBuffer());
		}

		private static object Decode64AndDeserialize(string p)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			if (p == null || p.Length == 0)
			{
				return null;
			}
			byte[] buffer = Convert.FromBase64String(p);
			BinaryFormatter val = new BinaryFormatter();
			MemoryStream memoryStream = new MemoryStream(buffer);
			return val.Deserialize((Stream)memoryStream);
		}

		private HashSet<ulong> GetSeparatedValues(string key, ref HashSet<ulong> cache)
		{
			if (cache == null)
			{
				string text = "";
				if (!m_values.Dictionary.ContainsKey(key))
				{
					m_values.Dictionary.Add(key, "");
				}
				else
				{
					text = GetParameterValue(key);
				}
				cache = new HashSet<ulong>();
				string[] array = text.Split(new char[1] { ',' });
				foreach (string text2 in array)
				{
					if (text2.Length > 0)
					{
						cache.Add(Convert.ToUInt64(text2));
					}
				}
			}
			return cache;
		}

		private void SetSeparatedValues(string key, HashSet<ulong> value, ref HashSet<ulong> cache)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			cache = value;
			string text = "";
			Enumerator<ulong> enumerator = value.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					text = text + enumerator.get_Current() + ",";
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			SetParameterValue(key, text);
		}

		public bool IsSetToLowQuality()
		{
<<<<<<< HEAD
			if (AnisotropicFiltering == MyTextureAnisoFiltering.NONE && AntialiasingMode == MyAntialiasingMode.NONE && ShadowQuality == MyShadowsQuality.LOW && TextureQuality == MyTextureQuality.LOW && VoxelTextureQuality == MyTextureQuality.LOW && ModelQuality == MyRenderQualityEnum.LOW && VoxelQuality == MyRenderQualityEnum.LOW && GrassDrawDistance == 0f && LightsQuality == MyRenderQualityEnum.LOW)
=======
			if (AnisotropicFiltering == MyTextureAnisoFiltering.NONE && AntialiasingMode == MyAntialiasingMode.NONE && ShadowQuality == MyShadowsQuality.LOW && TextureQuality == MyTextureQuality.LOW && VoxelTextureQuality == MyTextureQuality.LOW && ModelQuality == MyRenderQualityEnum.LOW && VoxelQuality == MyRenderQualityEnum.LOW && GrassDrawDistance == 0f)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				return true;
			}
			return false;
		}

		public void SetToLowQuality()
		{
			AnisotropicFiltering = MyTextureAnisoFiltering.NONE;
			AntialiasingMode = MyAntialiasingMode.NONE;
			ShadowQuality = MyShadowsQuality.LOW;
			TextureQuality = MyTextureQuality.LOW;
			VoxelTextureQuality = MyTextureQuality.LOW;
			ModelQuality = MyRenderQualityEnum.LOW;
			VoxelQuality = MyRenderQualityEnum.LOW;
			LightsQuality = MyRenderQualityEnum.LOW;
			GrassDrawDistance = 0f;
		}

		internal void SetToMediumQuality()
		{
			MyPerformanceSettings myPerformanceSettings = default(MyPerformanceSettings);
			myPerformanceSettings.RenderSettings = new MyRenderSettings1
			{
				AnisotropicFiltering = MyTextureAnisoFiltering.NONE,
				AntialiasingMode = MyAntialiasingMode.FXAA,
				ShadowQuality = MyShadowsQuality.MEDIUM,
				ShadowGPUQuality = MyRenderQualityEnum.NORMAL,
				AmbientOcclusionEnabled = true,
				TextureQuality = MyTextureQuality.MEDIUM,
				VoxelTextureQuality = MyTextureQuality.MEDIUM,
				ModelQuality = MyRenderQualityEnum.NORMAL,
				VoxelQuality = MyRenderQualityEnum.NORMAL,
				GrassDrawDistance = 160f,
				GrassDensityFactor = 1f,
				HqDepth = true,
				VoxelShaderQuality = MyRenderQualityEnum.NORMAL,
				AlphaMaskedShaderQuality = MyRenderQualityEnum.NORMAL,
				AtmosphereShaderQuality = MyRenderQualityEnum.NORMAL,
				DistanceFade = 100f,
<<<<<<< HEAD
				ParticleQuality = MyRenderQualityEnum.NORMAL,
				LightsQuality = MyRenderQualityEnum.NORMAL
=======
				ParticleQuality = MyRenderQualityEnum.NORMAL
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			};
			myPerformanceSettings.EnableDamageEffects = true;
			MyPerformanceSettings performanceSettings = myPerformanceSettings;
			MyGraphicsSettings currentGraphicsSettings = MyVideoSettingsManager.CurrentGraphicsSettings;
			currentGraphicsSettings.PerformanceSettings = performanceSettings;
			MyVideoSettingsManager.Apply(currentGraphicsSettings);
			MyVideoSettingsManager.SaveCurrentSettings();
		}

		protected override void NewConfigWasStarted()
		{
			base.NewConfigWasStarted();
			EnableNewNewGameScreen = MyPlatformGameSettings.ENABLE_SIMPLE_NEWGAME_SCREEN;
			MyNewGameScreenABTestHelper.Instance.ActivateTest();
		}
	}
}
