using System;
using Epic.OnlineServices;
using Epic.OnlineServices.Lobby;

namespace VRage.EOS
{
	internal static class LobbyExtensions
	{
		public static Result SetParameter(this LobbySearch self, string key, ComparisonOp comparison, AttributeDataValue value)
		{
			return self.SetParameter(new LobbySearchSetParameterOptions
			{
				ComparisonOp = comparison,
				Parameter = new AttributeData
				{
					Key = key,
					Value = value
				}
			});
		}

		public static object GetValue(this AttributeDataValue self)
		{
			switch (self.ValueType)
			{
			case AttributeType.Boolean:
				return self.AsBool;
			case AttributeType.Int64:
				return self.AsInt64;
			case AttributeType.Double:
				return self.AsDouble;
			case AttributeType.String:
				return self.AsUtf8;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}
}
