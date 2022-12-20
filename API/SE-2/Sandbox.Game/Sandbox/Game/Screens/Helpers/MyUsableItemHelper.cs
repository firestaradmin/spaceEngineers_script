using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.GUI;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Input;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyUsableItemHelper
	{
		public static void ItemActivatedGridKeyboard(MyPhysicalInventoryItem item, MyInventory inventory, MyCharacter character, MySharedButtonsEnum button)
		{
			if (item.Content is MyObjectBuilder_ConsumableItem && button == MySharedButtonsEnum.Secondary)
			{
				MyFixedPoint amount = item.Amount;
				if (amount > 0)
				{
					amount = MyFixedPoint.Min(amount, 1);
					if (character != null && character.StatComp != null && amount > 0)
					{
						if (character.StatComp.HasAnyComsumableEffect() || (character.SuitBattery != null && character.SuitBattery.HasAnyComsumableEffect()))
						{
							if (MyHud.Notifications != null)
							{
								MyHudNotification notification = new MyHudNotification(MyCommonTexts.ConsumableCooldown);
								MyHud.Notifications.Add(notification);
								return;
							}
						}
						else
						{
							inventory.ConsumeItem(item.Content.GetId(), amount, character.EntityId);
						}
					}
				}
			}
			MyObjectBuilder_Datapad myObjectBuilder_Datapad = item.Content as MyObjectBuilder_Datapad;
			if (myObjectBuilder_Datapad != null && button == MySharedButtonsEnum.Secondary)
			{
				MyScreenManager.AddScreen(new MyGuiDatapadEditScreen(myObjectBuilder_Datapad, item, inventory, character));
			}
		}

		public static bool ItemActivatedGridGamepad(MyPhysicalInventoryItem item, MyInventory inventory, MyCharacter character, MyGridItemAction action)
		{
			if (item.Content is MyObjectBuilder_ConsumableItem && action == MyGridItemAction.Button_Y)
			{
				MyFixedPoint amount = item.Amount;
				if (amount > 0)
				{
					amount = MyFixedPoint.Min(amount, 1);
					if (character != null && character.StatComp != null && amount > 0)
					{
						if (character.StatComp.HasAnyComsumableEffect() || (character.SuitBattery != null && character.SuitBattery.HasAnyComsumableEffect()))
						{
							if (MyHud.Notifications != null)
							{
								MyHudNotification notification = new MyHudNotification(MyCommonTexts.ConsumableCooldown);
								MyHud.Notifications.Add(notification);
								return false;
							}
						}
						else
						{
							inventory.ConsumeItem(item.Content.GetId(), amount, character.EntityId);
						}
						return true;
					}
				}
			}
			MyObjectBuilder_Datapad myObjectBuilder_Datapad = item.Content as MyObjectBuilder_Datapad;
			if (myObjectBuilder_Datapad != null && action == MyGridItemAction.Button_Y)
			{
				MyScreenManager.AddScreen(new MyGuiDatapadEditScreen(myObjectBuilder_Datapad, item, inventory, character));
				return true;
			}
			return false;
		}
	}
}
