using System.Collections.Generic;

namespace VRage.GameServices
{
	public class MySessionSearchFilter
	{
		public struct Query
		{
			public string Property;

			public string Value;

			public MySearchCondition Condition;

			public Query(string property, MySearchCondition condition, string value)
			{
				Property = property;
				Condition = condition;
				Value = value;
			}

			/// <inheritdoc />
			public override string ToString()
			{
				return $"{Property}::{Condition}::{Value}";
			}
		}

		/// <summary>
		/// Property combining the server and world names.
		/// </summary>
		public const string Names = "SERVER_PROP_NAMES";

		/// <summary>
		/// Game Data property.
		/// </summary>
		/// <seealso cref="M:VRage.GameServices.IMyGameServer.SetGameData(System.String)" />
		public const string Data = "SERVER_PROP_DATA";

		/// <summary>
		/// Property equal to the number of players in the server.
		/// </summary>
		public const string PlayerCount = "SERVER_PROP_PLAYER_COUNT";

		/// <summary>
		/// Property equal to the estimated ping time to the server.
		/// </summary>
		public const string Ping = "SERVER_PROP_PING";

		/// <summary>
		/// Property equal to the game's tags.
		/// </summary>
		public const string Tags = "SERVER_PROP_TAGS";

		/// <summary>
		/// Property equal to the game's tags.
		/// </summary>
		public const string CustomPropertyPrefix = "SERVER_CPROP_";

		/// <summary>
		/// Queries contained in this filter.
		/// </summary>
		public readonly List<Query> Queries = new List<Query>();

		/// <summary>
		/// Append a query to this search filter.
		/// </summary>
		/// <param name="property"></param>
		/// <param name="condition"></param>
		/// <param name="value"></param>
		public void AddQuery(string property, MySearchCondition condition, string value)
		{
			Queries.Add(new Query(property, condition, value));
		}

		/// <summary>
		/// Append a query for a custom property to this search filter.
		/// </summary>
		/// <param name="customProperty"></param>
		/// <param name="condition"></param>
		/// <param name="value"></param>
		public void AddQueryCustom(string customProperty, MySearchCondition condition, string value)
		{
			Queries.Add(new Query("SERVER_CPROP_" + customProperty, condition, value));
		}

		/// <inheritdoc />
		public override string ToString()
		{
			return "[" + string.Join("", Queries) + "]";
		}
	}
}
