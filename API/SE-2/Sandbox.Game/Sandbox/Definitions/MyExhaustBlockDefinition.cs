<<<<<<< HEAD
using System.Collections.Generic;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ExhaustBlockDefinition), null)]
<<<<<<< HEAD
	public class MyExhaustBlockDefinition : MyFunctionalBlockDefinition
=======
	public class MyExhaustBlockDefinition : MyCubeBlockDefinition
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	{
		private class Sandbox_Definitions_MyExhaustBlockDefinition_003C_003EActor : IActivator, IActivator<MyExhaustBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyExhaustBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyExhaustBlockDefinition CreateInstance()
			{
				return new MyExhaustBlockDefinition();
			}

			MyExhaustBlockDefinition IActivator<MyExhaustBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float RequiredPowerInput;

<<<<<<< HEAD
		public List<string> AvailableEffects;

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ExhaustBlockDefinition myObjectBuilder_ExhaustBlockDefinition = (MyObjectBuilder_ExhaustBlockDefinition)builder;
			RequiredPowerInput = myObjectBuilder_ExhaustBlockDefinition.RequiredPowerInput;
<<<<<<< HEAD
			AvailableEffects = myObjectBuilder_ExhaustBlockDefinition.AvailableEffects;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
