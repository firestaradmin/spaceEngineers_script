using System;
using VRage.Collections;
using VRageRender;

namespace VRage.Game.ModAPI
{
	/// <summary>
	/// This interface provides access to game settings
	/// </summary>
	public interface IMyConfig
	{
<<<<<<< HEAD
		/// <summary>
		/// Gets if ambient occlusion is enabled
		/// </summary>
		bool? AmbientOcclusionEnabled { get; }

		/// <summary>
		/// Gets anisotropic filtering
		/// </summary>
		MyTextureAnisoFiltering? AnisotropicFiltering { get; }

		/// <summary>
		/// Gets anti aliasing mode
		/// </summary>
		MyAntialiasingMode? AntialiasingMode { get; }

		/// <summary>
		/// Gets whether mouse should be captured by game screenshot
		/// </summary>
		bool CaptureMouse { get; }

		/// <summary>
		/// Gets whether games should show controls hints
		/// </summary>
		bool ControlsHints { get; }

		/// <summary>
		/// SingleBlock - 0
		/// Line - 1
		/// Plane - 2
		/// </summary>
		int CubeBuilderBuildingMode { get; }

		/// <summary>
		/// Gets whether cube builder should use symmetry
		/// </summary>
		bool CubeBuilderUseSymmetry { get; }

		/// <summary>
		/// Gets whether damage effects are enabled 
		/// </summary>
		bool EnableDamageEffects { get; }

		/// <summary>
		/// Gets dynamic music is enabled 
		/// </summary>
		bool EnableDynamicMusic { get; }

		/// <summary>
		/// Gets whether should mute sound when game window is not in focus
		/// </summary>
		bool EnableMuteWhenNotInFocus { get; }

		/// <summary>
		/// Gets whether should show performance warning
		/// </summary>
		bool EnablePerformanceWarnings { get; }

		/// <summary>
		/// Gets whether reverb sound feature is enabled
		/// </summary>
		bool EnableReverb { get; }

		/// <summary>
		/// Gets whether voice chat is enabled
		/// </summary>
		bool EnableVoiceChat { get; }

		/// <summary>
		/// Gets game field of view setting
		/// </summary>
		float FieldOfView { get; }

		/// <summary>
		/// Gets if game is run at first time
		/// </summary>
		bool FirstTimeRun { get; }

		/// <summary>
		/// Gets flares intensity graphics feature
		/// </summary>
		float FlaresIntensity { get; }

		/// <summary>
		/// Gets game master volume
		/// </summary>
		float GameVolume { get; }

		/// <summary>
		/// Gets graphics renderer setting
		/// </summary>
		MyGraphicsRenderer GraphicsRenderer { get; }

		/// <summary>
		/// Gets grass density factor
		/// </summary>
		float? GrassDensityFactor { get; }

		/// <summary>
		/// Gets grass draw distance
		/// </summary>
		float? GrassDrawDistance { get; }

		/// <summary>
		/// Gets HUD Background Opacity
		/// </summary>
		float HUDBkOpacity { get; }

		/// <summary>
		/// Gets whether game should show warnings
		/// </summary>
		bool HudWarnings { get; }

		/// <summary>
		/// Gets game current language
		/// </summary>
		MyLanguagesEnum Language { get; }

		/// <summary>
		/// Always true
		/// </summary>
		bool MemoryLimits { get; }

		/// <summary>
		/// Gets if hud currently hidden
		/// </summary>
		bool MinimalHud { get; }

		/// <summary>
		/// Gets hud state:
		/// 0 - hidden
		/// 1 - visible with descriptions
		/// 2 - visible without descriptions
		/// </summary>
		int HudState { get; }

		/// <summary>
		/// Gets model render quality
		/// </summary>
		MyRenderQualityEnum? ModelQuality { get; }

		/// <summary>
		/// Gets music volume
		/// </summary>
		float MusicVolume { get; }

		/// <summary>
		/// Gets set of muted players
		/// </summary>
		HashSetReader<ulong> MutedPlayers { get; }

		/// <summary>
		/// Gets game refresh rate multiplied by 1000
		/// </summary>
		int RefreshRate { get; }

		/// <summary>
		/// Gets if gabe should show rotation hints
		/// </summary>
		bool RotationHints { get; }

		/// <summary>
		/// Gets screen height 
		/// </summary>
		int? ScreenHeight { get; }

		/// <summary>
		/// Gets screenshot size multiplier
		/// </summary>
		float ScreenshotSizeMultiplier { get; }

		/// <summary>
		/// Gets grass draw distance
		/// </summary>
		int? ScreenWidth { get; }

		/// <summary>
		/// Gets shader quality setting
		/// </summary>
		MyRenderQualityEnum? ShaderQuality { get; }

		/// <summary>
		/// Gets shadow quality setting
		/// </summary>
		MyShadowsQuality? ShadowQuality { get; }

		/// <summary>
		/// Gets if ship sound are base on ship speed
		/// </summary>
		bool ShipSoundsAreBasedOnSpeed { get; }

		/// <summary>
		/// Gets whether game shows crosshair
		/// </summary>
		bool ShowCrosshair { get; }

		/// <summary>
		/// Gets game crosshair settings
		/// VisibleWithHud = 0,
		/// AlwaysVisible = 1,
		/// AlwaysHidden = 2
		/// </summary>
		int ShowCrosshair2 { get; }

		/// <summary>
		/// Gets grass draw distance
		/// </summary>
		bool EnableTrading { get; }

		/// <summary>
		/// Not used. 
		/// </summary>
		string Skin { get; }

		/// <summary>
		/// Gets game texture quality
		/// </summary>
		MyTextureQuality? TextureQuality { get; }

		/// <summary>
		/// Gets game voxel texture quality
		/// </summary>
		MyTextureQuality? VoxelTextureQuality { get; }

		/// <summary>
		/// Gets game UI background opacity
		/// </summary>
		float UIBkOpacity { get; }

		/// <summary>
		/// Gets game UI opacity
		/// </summary>
		float UIOpacity { get; }

		/// <summary>
		/// Gets tree draw distance
		/// </summary>
		float? VegetationDrawDistance { get; }

		/// <summary>
		/// Gets game vertical sync
		/// </summary>
		int VerticalSync { get; }

		/// <summary>
		/// Gets current used video adapter index. ESC-&gt;Display-&gt;VideoAdapter
		/// </summary>
		int VideoAdapter { get; }

		/// <summary>
		/// Gets game voice chat volume
		/// </summary>
		float VoiceChatVolume { get; }

		/// <summary>
		/// Gets voxel rendering quality
		/// </summary>
		MyRenderQualityEnum? VoxelQuality { get; }

		/// <summary>
		/// Gets game window mode
		/// </summary>
		MyWindowModeEnum WindowMode { get; }

		/// <summary>
		/// Gets game lights quality
		/// </summary>
		MyRenderQualityEnum? LightsQuality { get; }
=======
		bool? AmbientOcclusionEnabled { get; }

		MyTextureAnisoFiltering? AnisotropicFiltering { get; }

		MyAntialiasingMode? AntialiasingMode { get; }

		bool CaptureMouse { get; }

		bool ControlsHints { get; }

		int CubeBuilderBuildingMode { get; }

		bool CubeBuilderUseSymmetry { get; }

		bool EnableDamageEffects { get; }

		bool EnableDynamicMusic { get; }

		bool EnableMuteWhenNotInFocus { get; }

		bool EnablePerformanceWarnings { get; }

		bool EnableReverb { get; }

		bool EnableVoiceChat { get; }

		float FieldOfView { get; }

		bool FirstTimeRun { get; }

		float FlaresIntensity { get; }

		float GameVolume { get; }

		MyGraphicsRenderer GraphicsRenderer { get; }

		float? GrassDensityFactor { get; }

		float? GrassDrawDistance { get; }

		float HUDBkOpacity { get; }

		bool HudWarnings { get; }

		MyLanguagesEnum Language { get; }

		bool MemoryLimits { get; }

		bool MinimalHud { get; }

		int HudState { get; }

		MyRenderQualityEnum? ModelQuality { get; }

		float MusicVolume { get; }

		HashSetReader<ulong> MutedPlayers { get; }

		int RefreshRate { get; }

		bool RotationHints { get; }

		int? ScreenHeight { get; }

		float ScreenshotSizeMultiplier { get; }

		int? ScreenWidth { get; }

		MyRenderQualityEnum? ShaderQuality { get; }

		MyShadowsQuality? ShadowQuality { get; }

		bool ShipSoundsAreBasedOnSpeed { get; }

		bool ShowCrosshair { get; }

		int ShowCrosshair2 { get; }

		bool EnableTrading { get; }

		string Skin { get; }

		MyTextureQuality? TextureQuality { get; }

		MyTextureQuality? VoxelTextureQuality { get; }

		float UIBkOpacity { get; }

		float UIOpacity { get; }

		float? VegetationDrawDistance { get; }

		int VerticalSync { get; }

		int VideoAdapter { get; }

		float VoiceChatVolume { get; }

		MyRenderQualityEnum? VoxelQuality { get; }

		MyWindowModeEnum WindowMode { get; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		[Obsolete]
		DictionaryReader<string, object> ControlsButtons { get; }

		[Obsolete]
		DictionaryReader<string, object> ControlsGeneral { get; }
	}
}
