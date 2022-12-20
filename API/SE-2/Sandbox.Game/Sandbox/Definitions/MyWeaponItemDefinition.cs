using System.Collections.Generic;
using Sandbox.Game.Weapons;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_WeaponItemDefinition), null)]
	public class MyWeaponItemDefinition : MyPhysicalItemDefinition
	{
		private class Sandbox_Definitions_MyWeaponItemDefinition_003C_003EActor : IActivator, IActivator<MyWeaponItemDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyWeaponItemDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWeaponItemDefinition CreateInstance()
			{
				return new MyWeaponItemDefinition();
			}

			MyWeaponItemDefinition IActivator<MyWeaponItemDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyDefinitionId WeaponDefinitionId;

		public bool ShowAmmoCount;

		public Dictionary<int, string> DummyNames;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_WeaponItemDefinition myObjectBuilder_WeaponItemDefinition = builder as MyObjectBuilder_WeaponItemDefinition;
			WeaponDefinitionId = new MyDefinitionId(myObjectBuilder_WeaponItemDefinition.WeaponDefinitionId.Type, myObjectBuilder_WeaponItemDefinition.WeaponDefinitionId.Subtype);
			ShowAmmoCount = myObjectBuilder_WeaponItemDefinition.ShowAmmoCount;
			DummyNames = new Dictionary<int, string>();
			if (myObjectBuilder_WeaponItemDefinition.MuzzleProjectileDummyName != null)
			{
				DummyNames.Add(MyGunBase.DUMMY_KEY_PROJECTILE, myObjectBuilder_WeaponItemDefinition.MuzzleProjectileDummyName);
			}
			if (myObjectBuilder_WeaponItemDefinition.MuzzleMissileDummyName != null)
			{
				DummyNames.Add(MyGunBase.DUMMY_KEY_MISSILE, myObjectBuilder_WeaponItemDefinition.MuzzleMissileDummyName);
			}
			if (myObjectBuilder_WeaponItemDefinition.HoldingDummyName != null)
			{
				DummyNames.Add(MyGunBase.DUMMY_KEY_HOLDING, myObjectBuilder_WeaponItemDefinition.HoldingDummyName);
			}
		}
	}
}
