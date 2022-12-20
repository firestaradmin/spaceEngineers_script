using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[] { true, false }, new KeyTypeEnum[]
	{
		KeyTypeEnum.EntityName,
		KeyTypeEnum.Unknown
	})]
	public delegate void DoubleKeyPlayerEvent(string entityName, long playerId, string gridName);
}
