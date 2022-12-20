namespace VRage.Game.VisualScripting
{
	[VisualScriptingEvent(new bool[] { true, true, true }, new KeyTypeEnum[]
	{
		KeyTypeEnum.Trigger,
		KeyTypeEnum.EntityId,
		KeyTypeEnum.EntityName
	})]
	public delegate void TriggerEventComplex(string triggerName, long entityId, string entityName);
}
