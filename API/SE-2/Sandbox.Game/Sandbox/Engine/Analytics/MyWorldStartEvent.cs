using VRage.Analytics;

namespace Sandbox.Engine.Analytics
{
	public class MyWorldStartEvent : MyAnalyticsEvent
	{
		[Required]
		public MySessionStartEvent SessionStartProperties { get; set; }

		[Required]
		public string WorldSessionID { get; set; }

		public string scenario_name { get; set; }

		public string active_mods { get; set; }

		public int? active_mods_count { get; set; }

		public bool? destructible_blocks { get; set; }

		public bool? destructible_voxels { get; set; }

		public bool? drones { get; set; }

		public bool? encounters { get; set; }

		public string entry { get; set; }

		public string game_mode { get; set; }

		public string hostility { get; set; }

		public bool? is_campaign_mission { get; set; }

		public bool? is_hosting_player { get; set; }

		public bool? is_official_campaign { get; set; }

		public bool? jetpack { get; set; }

		public int? level_script_count { get; set; }

		public long? loading_duration { get; set; }

		public int? max_floating_objects { get; set; }

		public string multiplayer_type { get; set; }

		public double? multiplier_assembler_efficiency { get; set; }

		public double? multiplier_assembler_speed { get; set; }

		public double? multiplier_grinding_speed { get; set; }

		public double? multiplier_inventory { get; set; }

		public double? multiplier_refinery_speed { get; set; }

		public double? multiplier_welding_speed { get; set; }

		public string online_mode { get; set; }

		public bool? oxygen { get; set; }

		public bool? pressurization { get; set; }

		public bool? realistic_sounds { get; set; }

		public int? server_current_number_of_players { get; set; }

		public bool? server_is_dedicated { get; set; }

		public string server_id { get; set; }

		public int? server_max_number_of_players { get; set; }

		public bool? server_name { get; set; }

		public bool? spiders { get; set; }

		public int? state_machine_count { get; set; }

		public bool? tool_shake { get; set; }

		public bool? voxel_support { get; set; }

		public bool? weather_system { get; set; }

		public bool? wolfs { get; set; }

		public string worldName { get; set; }

		public string world_type { get; set; }

		public string networking_type { get; set; }

		public MyWorldStartEvent(MySessionStartEvent sessionStartProperties)
		{
			SessionStartProperties = sessionStartProperties;
		}

		public override string GetEventName()
		{
			return "WorldStart";
		}

		public override MyReportTypeData GetReportTypeAndArgs()
		{
			return new MyReportTypeData(MyReportType.ProgressionStart, "Game", world_type ?? "UnknownWorld");
		}
	}
}
