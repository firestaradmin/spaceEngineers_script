using System.Collections.Generic;
using VRage.Analytics;

namespace Sandbox.Engine.Analytics
{
	public class MyWorldEndEvent : MyAnalyticsEvent
	{
		[Required]
		public MyWorldStartEvent WorldStartProperties { get; set; }

		public uint? entire_world_duration { get; set; }

		public uint? fps_average { get; set; }

		public uint? fps_maximum { get; set; }

		public uint? fps_minimum { get; set; }

		public double? simspeed_client_average { get; set; }

		public double? simspeed_server_average { get; set; }

		public uint? time_camera_building_first_person { get; set; }

		public uint? time_camera_building_third_person { get; set; }

		public uint? time_camera_character_first_person { get; set; }

		public uint? time_camera_character_third_person { get; set; }

		public uint? time_camera_grid_first_person { get; set; }

		public uint? time_camera_grid_third_person { get; set; }

		public uint? time_camera_tool_first_person { get; set; }

		public uint? time_camera_tool_third_person { get; set; }

		public uint? time_camera_weapon_first_person { get; set; }

		public uint? time_camera_weapon_third_person { get; set; }

		public uint? time_creative_tools_enabled { get; set; }

		public uint? time_in_ship_builder_mode { get; set; }

		public uint? time_on_foot_all { get; set; }

		public uint? time_on_foot_asteroids { get; set; }

		public uint? time_on_foot_planets { get; set; }

		public uint? time_on_foot_ships { get; set; }

		public uint? time_on_foot_stations { get; set; }

		public uint? time_sprinting { get; set; }

		public uint? time_piloting_big_ships { get; set; }

		public uint? time_piloting_small_ships { get; set; }

		public uint? time_grinding_blocks { get; set; }

		public uint? time_grinding_friendly_blocks { get; set; }

		public uint? time_grinding_neutral_blocks { get; set; }

		public uint? time_grinding_enemy_blocks { get; set; }

		public uint? time_using_jetpack { get; set; }

		public uint? time_using_input_mouse { get; set; }

		public uint? time_using_input_gamepad { get; set; }

		public uint? toolbar_page_switches { get; set; }

		public uint? toolbar_used_slots { get; set; }

		public uint? total_blocks_created { get; set; }

		public uint? total_blocks_created_from_ship { get; set; }

		public uint? total_damage_dealt { get; set; }

		public Dictionary<string, int> TotalAmountMined { get; set; }

		public uint? ups_average { get; set; }

		public uint? world_session_duration { get; set; }

		public uint? server_most_concurrent_players { get; set; }

		public Dictionary<string, double> NetworkStats { get; set; }

		public string world_exit_reason { get; set; }

		public MyWorldEndEvent(MyWorldStartEvent worldStartProperties)
		{
			WorldStartProperties = worldStartProperties;
		}

		public override string GetEventName()
		{
			return "WorldEnd";
		}

		public override MyReportTypeData GetReportTypeAndArgs()
		{
			return new MyReportTypeData(MyReportType.ProgressionComplete, "Game", WorldStartProperties?.world_type ?? "UnknownWorld");
		}
	}
}
