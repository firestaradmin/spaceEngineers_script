using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_DecoyDefinition), null)]
<<<<<<< HEAD
	public class MyDecoyDefinition : MyFunctionalBlockDefinition
=======
	public class MyDecoyDefinition : MyCubeBlockDefinition
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	{
		private class Sandbox_Definitions_MyDecoyDefinition_003C_003EActor : IActivator, IActivator<MyDecoyDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyDecoyDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyDecoyDefinition CreateInstance()
			{
				return new MyDecoyDefinition();
			}

			MyDecoyDefinition IActivator<MyDecoyDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float LightningRodRadiusLarge;

		public float LightningRodRadiusSmall;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_DecoyDefinition myObjectBuilder_DecoyDefinition = builder as MyObjectBuilder_DecoyDefinition;
			LightningRodRadiusLarge = myObjectBuilder_DecoyDefinition.LightningRodRadiusLarge;
			LightningRodRadiusSmall = myObjectBuilder_DecoyDefinition.LightningRodRadiusSmall;
		}
	}
}
