using System.Collections.Generic;
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Weapons;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_WeaponBlockDefinition), null)]
	public class MyWeaponBlockDefinition : MyFunctionalBlockDefinition
	{
		private class Sandbox_Definitions_MyWeaponBlockDefinition_003C_003EActor : IActivator, IActivator<MyWeaponBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyWeaponBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWeaponBlockDefinition CreateInstance()
			{
				return new MyWeaponBlockDefinition();
			}

			MyWeaponBlockDefinition IActivator<MyWeaponBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyDefinitionId WeaponDefinitionId;

		public MyStringHash ResourceSinkGroup;

		public float InventoryMaxVolume;

		public float InventoryFillFactorMin;

<<<<<<< HEAD
		public Dictionary<int, string> DummyNames;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_WeaponBlockDefinition myObjectBuilder_WeaponBlockDefinition = builder as MyObjectBuilder_WeaponBlockDefinition;
			WeaponDefinitionId = new MyDefinitionId(myObjectBuilder_WeaponBlockDefinition.WeaponDefinitionId.Type, myObjectBuilder_WeaponBlockDefinition.WeaponDefinitionId.Subtype);
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_WeaponBlockDefinition.ResourceSinkGroup);
			InventoryMaxVolume = myObjectBuilder_WeaponBlockDefinition.InventoryMaxVolume;
			InventoryFillFactorMin = myObjectBuilder_WeaponBlockDefinition.InventoryFillFactorMin;
<<<<<<< HEAD
			DummyNames = new Dictionary<int, string>();
			if (myObjectBuilder_WeaponBlockDefinition.MuzzleProjectileDummyName != null)
			{
				DummyNames.Add(MyGunBase.DUMMY_KEY_PROJECTILE, myObjectBuilder_WeaponBlockDefinition.MuzzleProjectileDummyName);
			}
			if (myObjectBuilder_WeaponBlockDefinition.MuzzleMissileDummyName != null)
			{
				DummyNames.Add(MyGunBase.DUMMY_KEY_MISSILE, myObjectBuilder_WeaponBlockDefinition.MuzzleMissileDummyName);
			}
			if (myObjectBuilder_WeaponBlockDefinition.HoldingDummyName != null)
			{
				DummyNames.Add(MyGunBase.DUMMY_KEY_HOLDING, myObjectBuilder_WeaponBlockDefinition.HoldingDummyName);
			}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
