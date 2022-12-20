namespace VRage.Game.VisualScripting
{
	[VisualScriptingEvent(new bool[] { true, true, true, false }, new KeyTypeEnum[]
	{
		KeyTypeEnum.EntityName,
		KeyTypeEnum.Unknown,
		KeyTypeEnum.Unknown
	})]
	public delegate void InventoryChangedEvent(string entityName, string itemTypeName, string itemSubTypeName, float amount);
}
