using System;
using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.ObjectBuilders.Gui;
using VRage.GameServices;
using VRage.Serialization;
using VRageMath;

namespace Sandbox.Game.Screens
{
	public abstract class MyServerFilterOptions
	{
		protected struct MySessionSearchFilterHelper
		{
			public MySessionSearchFilter Filter;

			public Dictionary<string, MySearchConditionFlags> SupportedConditions;

			public static MySessionSearchFilterHelper Create(MySupportedPropertyFilters supportedFilters)
			{
				MySessionSearchFilterHelper result = default(MySessionSearchFilterHelper);
				result.Filter = new MySessionSearchFilter();
<<<<<<< HEAD
				result.SupportedConditions = supportedFilters.ToDictionary((MySupportedPropertyFilters.Entry x) => x.Property, (MySupportedPropertyFilters.Entry x) => x.SupportedConditions);
				return result;
			}

			/// <summary>
			/// Add a query to the filter if supported.
			/// </summary>
			/// <param name="property"></param>
			/// <param name="condition"></param>
			/// <param name="value"></param>
=======
				result.SupportedConditions = Enumerable.ToDictionary<MySupportedPropertyFilters.Entry, string, MySearchConditionFlags>((IEnumerable<MySupportedPropertyFilters.Entry>)supportedFilters, (Func<MySupportedPropertyFilters.Entry, string>)((MySupportedPropertyFilters.Entry x) => x.Property), (Func<MySupportedPropertyFilters.Entry, MySearchConditionFlags>)((MySupportedPropertyFilters.Entry x) => x.SupportedConditions));
				return result;
			}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public void AddConditional(string property, MySearchCondition condition, object value)
			{
				if (SupportedConditions.TryGetValue(property, out var value2) && value2.Contains(condition))
				{
					Filter.AddQuery(property, condition, value.ToString());
				}
			}

			public void AddCustomConditional(string property, MySearchCondition condition, object value)
			{
				if (SupportedConditions.TryGetValue("SERVER_CPROP_", out var value2) && value2.Contains(condition))
				{
					Filter.AddQueryCustom(property, condition, value.ToString());
				}
			}
		}

		public bool AllowedGroups;

		public bool SameVersion;

		public bool SameData;

		public bool? HasPassword;

		public bool CreativeMode;

		public bool SurvivalMode;

		public bool AdvancedFilter;

		public int Ping;

		public bool CheckPlayer;

		public SerializableRange PlayerCount;

		public bool CheckMod;

		public SerializableRange ModCount;

		public bool CheckDistance;

		public SerializableRange ViewDistance;

		public bool ModsExclusive;

		public HashSet<WorkshopId> Mods = new HashSet<WorkshopId>();

		private Dictionary<byte, IMyFilterOption> m_filters;

		public const int MAX_PING = 150;

		public Dictionary<byte, IMyFilterOption> Filters
		{
			get
			{
				return m_filters ?? (m_filters = CreateFilters());
			}
			set
			{
				m_filters = value;
			}
		}

		public MyServerFilterOptions()
		{
			SetDefaults();
		}

		public void SetDefaults(bool resetMods = false)
		{
			AdvancedFilter = false;
			CheckPlayer = false;
			CheckMod = false;
			CheckDistance = false;
			AllowedGroups = true;
			SameVersion = true;
			SameData = true;
			CreativeMode = true;
			SurvivalMode = true;
			HasPassword = null;
			Ping = 150;
			m_filters = CreateFilters();
			if (resetMods)
			{
				Mods.Clear();
			}
		}

		public MyServerFilterOptions(MyObjectBuilder_ServerFilterOptions ob)
		{
			Init(ob);
		}

		protected abstract Dictionary<byte, IMyFilterOption> CreateFilters();

		public abstract bool FilterServer(MyCachedServerItem server);

		public abstract bool FilterLobby(IMyLobby lobby);

		public abstract MySessionSearchFilter GetNetworkFilter(MySupportedPropertyFilters supportedFilters, string searchText);

		public MyObjectBuilder_ServerFilterOptions GetObjectBuilder()
		{
			MyObjectBuilder_ServerFilterOptions myObjectBuilder_ServerFilterOptions = new MyObjectBuilder_ServerFilterOptions();
			myObjectBuilder_ServerFilterOptions.AllowedGroups = AllowedGroups;
			myObjectBuilder_ServerFilterOptions.SameVersion = SameVersion;
			myObjectBuilder_ServerFilterOptions.SameData = SameData;
			myObjectBuilder_ServerFilterOptions.HasPassword = HasPassword;
			myObjectBuilder_ServerFilterOptions.CreativeMode = CreativeMode;
			myObjectBuilder_ServerFilterOptions.SurvivalMode = SurvivalMode;
			myObjectBuilder_ServerFilterOptions.CheckPlayer = CheckPlayer;
			myObjectBuilder_ServerFilterOptions.PlayerCount = PlayerCount;
			myObjectBuilder_ServerFilterOptions.CheckMod = CheckMod;
			myObjectBuilder_ServerFilterOptions.ModCount = ModCount;
			myObjectBuilder_ServerFilterOptions.CheckDistance = CheckDistance;
			myObjectBuilder_ServerFilterOptions.ViewDistance = ViewDistance;
			myObjectBuilder_ServerFilterOptions.Advanced = AdvancedFilter;
			myObjectBuilder_ServerFilterOptions.Ping = Ping;
<<<<<<< HEAD
			myObjectBuilder_ServerFilterOptions.WorkshopMods = Mods?.ToList();
=======
			HashSet<WorkshopId> mods = Mods;
			myObjectBuilder_ServerFilterOptions.WorkshopMods = ((mods != null) ? Enumerable.ToList<WorkshopId>((IEnumerable<WorkshopId>)mods) : null);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			myObjectBuilder_ServerFilterOptions.ModsExclusive = ModsExclusive;
			myObjectBuilder_ServerFilterOptions.Filters = new SerializableDictionary<byte, string>();
			foreach (KeyValuePair<byte, IMyFilterOption> filter in Filters)
			{
				myObjectBuilder_ServerFilterOptions.Filters[filter.Key] = filter.Value.SerializedValue;
			}
			return myObjectBuilder_ServerFilterOptions;
		}

		public void Init(MyObjectBuilder_ServerFilterOptions ob)
		{
			AllowedGroups = ob.AllowedGroups;
			SameVersion = ob.SameVersion;
			SameData = ob.SameData;
			HasPassword = ob.HasPassword;
			CreativeMode = ob.CreativeMode;
			SurvivalMode = ob.SurvivalMode;
			CheckPlayer = ob.CheckPlayer;
			PlayerCount = ob.PlayerCount;
			CheckMod = ob.CheckMod;
			ModCount = ob.ModCount;
			CheckDistance = ob.CheckDistance;
			ViewDistance = ob.ViewDistance;
			AdvancedFilter = ob.Advanced;
			Ping = ob.Ping;
			ModsExclusive = ob.ModsExclusive;
			if (ob.WorkshopMods != null)
			{
<<<<<<< HEAD
				Mods = new HashSet<WorkshopId>(ob.WorkshopMods);
=======
				Mods = new HashSet<WorkshopId>((IEnumerable<WorkshopId>)ob.WorkshopMods);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				Mods = new HashSet<WorkshopId>();
			}
			if (ob.Filters == null)
			{
				return;
			}
			foreach (KeyValuePair<byte, string> item in ob.Filters.Dictionary)
			{
				if (!Filters.TryGetValue(item.Key, out var value))
				{
					throw new Exception("Unrecognized filter key");
				}
				value.Configure(item.Value);
			}
		}
	}
}
