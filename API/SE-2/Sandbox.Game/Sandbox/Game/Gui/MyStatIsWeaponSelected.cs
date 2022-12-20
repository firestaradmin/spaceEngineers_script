using Sandbox.Game.Entities;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using VRage.Utils;

namespace Sandbox.Game.GUI
{
	public class MyStatIsWeaponSelected : MyStatBase
	{
		public MyStatIsWeaponSelected()
		{
			base.Id = MyStringHash.GetOrCompute("is_weapon_selected");
		}

		public override void Update()
		{
			MyShipController myShipController;
			if ((myShipController = MySession.Static.ControlledEntity as MyShipController) != null)
			{
				MyToolbarItemWeapon myToolbarItemWeapon;
				if (myShipController.Toolbar?.SelectedItem != null && myShipController.GridSelectionSystem?.WeaponSystem != null && (myToolbarItemWeapon = myShipController.Toolbar?.SelectedItem as MyToolbarItemWeapon) != null && myShipController.GridSelectionSystem.WeaponSystem.HasGunsOfId(myToolbarItemWeapon.Definition.Id) && myShipController.GridSelectionSystem.WeaponSystem.GetGun(myToolbarItemWeapon.Definition.Id).IsTargetLockingCapable)
				{
					base.CurrentValue = 1f;
				}
			}
			else
			{
				base.CurrentValue = 0f;
			}
		}
	}
}
