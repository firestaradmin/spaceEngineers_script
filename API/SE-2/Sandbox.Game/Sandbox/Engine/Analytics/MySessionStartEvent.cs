using System.Collections.Generic;
using Sandbox.Engine.Utils;
using VRage.Analytics;

namespace Sandbox.Engine.Analytics
{
<<<<<<< HEAD
	/// <summary> The game was launched or a new user logged in <para />
	/// Should be the first event in an analytics session </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	public class MySessionStartEvent : MyAnalyticsEvent
	{
		public bool? audio_hud_warnings { get; set; }

		public double? audio_music_volume { get; set; }

		public bool? audio_mute_when_not_in_focus { get; set; }

		public double? audio_sound_volume { get; set; }

		public string client_branch { get; set; }

		public string client_version { get; set; }

		public string cpu_info { get; set; }

		public uint? cpu_number_of_cores { get; set; }

		public int? cpu_number_of_threads { get; set; }

		public string display_resolution { get; set; }

		public string display_window_mode { get; set; }

		public string gpu_driver_version { get; set; }

		public int? gpu_memory { get; set; }

		public string gpu_name { get; set; }

		public string graphics_alphamasked_shader_quality { get; set; }

		public string graphics_anisotropic_filtering { get; set; }

		public string graphics_antialiasing_mode { get; set; }

		public string graphics_atmosphere_shader_quality { get; set; }

		public double? graphics_distance_fade { get; set; }

		public double? graphics_flares_intensity { get; set; }

		public double? graphics_grass_density_factor { get; set; }

		public double? graphics_grass_draw_distance { get; set; }

		public string graphics_shadow_quality { get; set; }

		public string graphics_texture_quality { get; set; }

		public string graphics_voxel_quality { get; set; }

		public string graphics_voxel_shader_quality { get; set; }

		public string graphics_voxel_texture_quality { get; set; }

		public bool? is_dedicated { get; set; }

		public bool? is_first_run { get; set; }

		public string os_architecture { get; set; }

		public string os_culture { get; set; }

		public string os_info { get; set; }

		public ulong? os_memory { get; set; }

		public bool? speed_based_ship_sounds { get; set; }

		public string userLanguage { get; set; }

		public string userLocale { get; set; }

		public string region_iso2 { get; set; }

		public string region_iso3 { get; set; }

		public double[] region_location { get; set; }

		public bool? experimental_mode { get; set; }

		public int? building_mode { get; set; }

		public bool? controls_hints { get; set; }

		public bool? goodbot_hints { get; set; }

		public bool? rotation_hints { get; set; }

		public bool? enable_steam_cloud { get; set; }

		public bool? enable_trading { get; set; }

		public bool? gdpr_consent { get; set; }

		public bool? area_interaction { get; set; }

		public bool? mod_io_consent { get; set; }

		public MyConfig.CrosshairSwitch show_crosshair { get; set; }

		public Dictionary<string, MyProduct> Products { get; set; }

		public override string GetEventName()
		{
			return "SessionStart";
		}

		public override MyReportTypeData GetReportTypeAndArgs()
		{
			return new MyReportTypeData(MyReportType.ProgressionStart, "Game", null);
		}
	}
}
