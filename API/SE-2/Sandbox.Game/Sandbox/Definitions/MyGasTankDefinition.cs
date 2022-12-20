using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_GasTankDefinition), null)]
	public class MyGasTankDefinition : MyProductionBlockDefinition
	{
		private class Sandbox_Definitions_MyGasTankDefinition_003C_003EActor : IActivator, IActivator<MyGasTankDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyGasTankDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyGasTankDefinition CreateInstance()
			{
				return new MyGasTankDefinition();
			}

			MyGasTankDefinition IActivator<MyGasTankDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float Capacity;

		public MyDefinitionId StoredGasId;

		public MyStringHash ResourceSourceGroup;

		public float LeakPercent;

		public float GasExplosionMaxRadius;

		public float GasExplosionNeededVolumeToReachMaxRadius;

		public float GasExplosionDamageMultiplier;

		public string GasExplosionSound;

		public string GasExplosionEffect;

		public string EmptyDamageEffectName;

		public MySoundPair EmptyDamagedSound;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_GasTankDefinition myObjectBuilder_GasTankDefinition = builder as MyObjectBuilder_GasTankDefinition;
			Capacity = myObjectBuilder_GasTankDefinition.Capacity;
			MyDefinitionId myDefinitionId = (StoredGasId = ((!myObjectBuilder_GasTankDefinition.StoredGasId.IsNull()) ? ((MyDefinitionId)myObjectBuilder_GasTankDefinition.StoredGasId) : new MyDefinitionId(typeof(MyObjectBuilder_GasProperties), "Oxygen")));
			ResourceSourceGroup = MyStringHash.GetOrCompute(myObjectBuilder_GasTankDefinition.ResourceSourceGroup);
			LeakPercent = myObjectBuilder_GasTankDefinition.LeakPercent;
			GasExplosionMaxRadius = myObjectBuilder_GasTankDefinition.GasExplosionMaxRadius;
			GasExplosionNeededVolumeToReachMaxRadius = myObjectBuilder_GasTankDefinition.GasExplosionNeededVolumeToReachMaxRadius;
			GasExplosionDamageMultiplier = myObjectBuilder_GasTankDefinition.GasExplosionDamageMultiplier;
			GasExplosionSound = myObjectBuilder_GasTankDefinition.GasExplosionSound;
			GasExplosionEffect = myObjectBuilder_GasTankDefinition.GasExplosionEffect;
			EmptyDamageEffectName = myObjectBuilder_GasTankDefinition.EmptyDamageEffectName;
			EmptyDamagedSound = new MySoundPair(myObjectBuilder_GasTankDefinition.EmptyDamagedSound);
		}
	}
}
