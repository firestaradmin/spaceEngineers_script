using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.Entity;

namespace Sandbox.Game.Screens.Helpers
{
	[MyToolbarItemDescriptor(typeof(MyObjectBuilder_ToolbarItemConsumable))]
	public class MyToolbarItemConsumable : MyToolbarItemDefinition
	{
		public MyInventory Inventory => (MySession.Static.ControlledEntity as MyCharacter)?.GetInventory();

		public override bool Activate()
		{
			MyFixedPoint myFixedPoint = ((Inventory != null) ? Inventory.GetItemAmount(Definition.Id) : ((MyFixedPoint)0));
			if (myFixedPoint > 0)
			{
				MyCharacter myCharacter = MySession.Static.ControlledEntity as MyCharacter;
				myFixedPoint = MyFixedPoint.Min(myFixedPoint, 1);
				if (myCharacter != null && myCharacter.StatComp != null && myFixedPoint > 0)
				{
					if (myCharacter.StatComp.HasAnyComsumableEffect() || (myCharacter.SuitBattery != null && myCharacter.SuitBattery.HasAnyComsumableEffect()))
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
						Inventory.ConsumeItem(Definition.Id, myFixedPoint, myCharacter.EntityId);
					}
				}
			}
			return true;
		}

		public override bool Init(MyObjectBuilder_ToolbarItem data)
		{
			bool result = base.Init(data);
			base.ActivateOnClick = false;
			base.WantsToBeActivated = false;
			return result;
		}

		public override MyObjectBuilder_ToolbarItem GetObjectBuilder()
		{
			if (Definition == null)
			{
				return null;
			}
			MyObjectBuilder_ToolbarItemConsumable obj = (MyObjectBuilder_ToolbarItemConsumable)MyToolbarItemFactory.CreateObjectBuilder(this);
			obj.DefinitionId = Definition.Id;
			return obj;
		}

		public override bool AllowedInToolbarType(MyToolbarType type)
		{
			return type == MyToolbarType.Character;
		}

		public override ChangeInfo Update(MyEntity owner, long playerID = 0L)
		{
			bool enabled = Inventory != null && Inventory.GetItemAmount(Definition.Id) > 0;
			return SetEnabled(enabled);
		}
	}
}
