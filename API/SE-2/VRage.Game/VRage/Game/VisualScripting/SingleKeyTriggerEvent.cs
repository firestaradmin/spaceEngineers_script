namespace VRage.Game.VisualScripting
{
	[VisualScriptingEvent(new bool[] { true, false }, new KeyTypeEnum[]
	{
		KeyTypeEnum.Trigger,
		KeyTypeEnum.Unknown
	})]
	public delegate void SingleKeyTriggerEvent(string triggerName, long playerId);
}
