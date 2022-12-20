using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionUnequip : MyActionBase
	{
		public override void ExecuteAction()
		{
			(MySession.Static.ControlledEntity as MyCharacter)?.SwitchToWeapon((MyToolbarItemWeapon)null);
		}
	}
}
