using VRage.Game.VisualScripting;

namespace Sandbox.Game
{
	[VisualScriptingEvent(new bool[] { true }, new KeyTypeEnum[] { KeyTypeEnum.EntityName })]
	public delegate void SingleKeyEntityNameEvent(string entityName);
}
