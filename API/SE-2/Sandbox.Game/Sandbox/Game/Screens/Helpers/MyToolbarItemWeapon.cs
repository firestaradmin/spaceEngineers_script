<<<<<<< HEAD
=======
using System;
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Text;
using Sandbox.Common.ObjectBuilders;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Weapons;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Game.ModAPI;

namespace Sandbox.Game.Screens.Helpers
{
	[MyToolbarItemDescriptor(typeof(MyObjectBuilder_ToolbarItemWeapon))]
	public class MyToolbarItemWeapon : MyToolbarItemDefinition
	{
		protected int m_lastAmmoCount = -1;

		protected int m_lastMagazineCount = -1;

		protected bool m_needsWeaponSwitching = true;

		protected string m_lastTextValue = string.Empty;

		private bool m_areValuesDirty = true;

		public int AmmoCount => m_lastAmmoCount;

		public int MagazineCount => m_lastMagazineCount;

		public override bool Init(MyObjectBuilder_ToolbarItem data)
		{
			bool result = base.Init(data);
			base.ActivateOnClick = false;
			return result;
		}

		public override MyObjectBuilder_ToolbarItem GetObjectBuilder()
		{
			return (MyObjectBuilder_ToolbarItemWeapon)base.GetObjectBuilder();
		}

		public override bool Activate()
		{
			if (Definition == null)
			{
				return false;
			}
			IMyControllableEntity controlledEntity = MySession.Static.ControlledEntity;
			if (controlledEntity != null)
			{
				if (m_needsWeaponSwitching)
				{
					controlledEntity.SwitchToWeapon(this);
					base.WantsToBeActivated = true;
				}
				else
				{
					controlledEntity.SwitchAmmoMagazine();
				}
			}
			return true;
		}

		public override bool AllowedInToolbarType(MyToolbarType type)
		{
			return true;
		}

		public override ChangeInfo Update(MyEntity owner, long playerID = 0L)
		{
			//IL_03af: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
			bool flag2 = false;
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			MyShipController myShipController = MySession.Static.ControlledEntity as MyShipController;
			bool flag3 = myShipController == null && localCharacter != null && (localCharacter.FindWeaponItemByDefinition(Definition.Id).HasValue || !localCharacter.WeaponTakesBuilderFromInventory(Definition.Id));
			ChangeInfo changeInfo = ChangeInfo.None;
			if (flag3)
			{
				IMyHandheldGunObject<MyDeviceBase> currentWeapon = localCharacter.CurrentWeapon;
				if (currentWeapon != null)
				{
					flag = MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId).Id == Definition.Id;
				}
				if (localCharacter.LeftHandItem != null)
				{
					flag |= Definition == localCharacter.LeftHandItem.PhysicalItemDefinition;
				}
				MyWeaponItemDefinition myWeaponItemDefinition2;
				if (flag && currentWeapon != null)
				{
					MyWeaponItemDefinition myWeaponItemDefinition = MyDefinitionManager.Static.GetPhysicalItemForHandItem(currentWeapon.DefinitionId) as MyWeaponItemDefinition;
					if (myWeaponItemDefinition != null && myWeaponItemDefinition.ShowAmmoCount)
					{
						int ammunitionAmount = localCharacter.CurrentWeapon.GetAmmunitionAmount();
						int magazineAmount = localCharacter.CurrentWeapon.GetMagazineAmount();
						if (m_lastAmmoCount != ammunitionAmount || m_lastMagazineCount != magazineAmount)
						{
							m_lastAmmoCount = ammunitionAmount;
							m_lastMagazineCount = magazineAmount;
							base.IconText.Clear().Append($"{ammunitionAmount} • {magazineAmount}");
							changeInfo |= ChangeInfo.IconText;
						}
					}
					m_areValuesDirty = false;
				}
				else if (m_areValuesDirty && localCharacter != null && (myWeaponItemDefinition2 = Definition as MyWeaponItemDefinition) != null && myWeaponItemDefinition2 != null && myWeaponItemDefinition2.ShowAmmoCount)
				{
					m_areValuesDirty = false;
					int num = 0;
					int num2 = 0;
					string text = string.Empty;
					MyInventory inventory = localCharacter.GetInventory();
					foreach (MyPhysicalInventoryItem item in inventory.GetItems())
					{
						MyObjectBuilder_PhysicalGunObject myObjectBuilder_PhysicalGunObject;
						MyObjectBuilder_AutomaticRifle myObjectBuilder_AutomaticRifle;
						if (item.Content != null && !(item.Content.SubtypeName != Definition.Id.SubtypeName) && (myObjectBuilder_PhysicalGunObject = item.Content as MyObjectBuilder_PhysicalGunObject) != null && (myObjectBuilder_AutomaticRifle = myObjectBuilder_PhysicalGunObject.GunEntity as MyObjectBuilder_AutomaticRifle) != null && myObjectBuilder_AutomaticRifle.GunBase != null)
						{
							num = myObjectBuilder_AutomaticRifle.GunBase.RemainingAmmo;
							text = myObjectBuilder_AutomaticRifle.GunBase.CurrentAmmoMagazineName;
							break;
						}
					}
					if (!string.IsNullOrEmpty(text))
					{
						foreach (MyPhysicalInventoryItem item2 in inventory.GetItems())
						{
							if (item2.Content != null && !(item2.Content.SubtypeName != text))
							{
								MyFixedPoint amount = item2.Amount;
								num2 = amount.ToIntSafe();
								break;
							}
						}
					}
					m_lastAmmoCount = num;
					m_lastMagazineCount = num2;
					base.IconText.Clear().Append($"{num} • {num2}");
					changeInfo |= ChangeInfo.IconText;
				}
			}
			if (myShipController != null && myShipController.GridSelectionSystem.WeaponSystem != null)
			{
				flag2 = myShipController.GridSelectionSystem.WeaponSystem.HasGunsOfId(Definition.Id);
				if (flag2)
				{
					IMyGunObject<MyDeviceBase> gun = myShipController.GridSelectionSystem.WeaponSystem.GetGun(Definition.Id);
					if (gun.GunBase is MyGunBase)
					{
						bool flag4 = gun is MySmallGatlingGun || gun is MySmallMissileLauncher;
						int num3 = 0;
<<<<<<< HEAD
						foreach (IMyGunObject<MyDeviceBase> item3 in myShipController.GridSelectionSystem.WeaponSystem.GetGunsById(Definition.Id))
						{
							num3 = ((!flag4) ? (num3 + item3.GetAmmunitionAmount()) : (num3 + item3.GetTotalAmmunitionAmount()));
=======
						Enumerator<IMyGunObject<MyDeviceBase>> enumerator2 = myShipController.GridSelectionSystem.WeaponSystem.GetGunsById(Definition.Id).GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								IMyGunObject<MyDeviceBase> current3 = enumerator2.get_Current();
								num3 = ((!flag4) ? (num3 + current3.GetAmmunitionAmount()) : (num3 + current3.GetTotalAmmunitionAmount()));
							}
						}
						finally
						{
							((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						}
						if (num3 != m_lastAmmoCount)
						{
							m_lastAmmoCount = num3;
							base.IconText.Clear().AppendInt32(num3);
							changeInfo |= ChangeInfo.IconText;
						}
					}
				}
				flag = myShipController.GridSelectionSystem.GetGunId() == Definition.Id;
<<<<<<< HEAD
			}
			bool flag5 = flag3 || flag2;
			changeInfo |= SetEnabled(flag5);
			if (!flag5)
			{
				base.IconText.Clear();
				m_lastAmmoCount = -1;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			base.WantsToBeSelected = flag;
			m_needsWeaponSwitching = !flag;
			if (m_lastTextValue != base.IconText.ToString())
			{
				changeInfo |= ChangeInfo.IconText;
			}
			m_lastTextValue = base.IconText.ToString();
			return changeInfo;
		}
	}
}
