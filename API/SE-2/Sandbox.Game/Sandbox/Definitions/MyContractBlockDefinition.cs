<<<<<<< HEAD
=======
using System.Collections.Generic;
using System.Linq;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ContractBlockDefinition), null)]
	public class MyContractBlockDefinition : MyFunctionalBlockDefinition
	{
		private class Sandbox_Definitions_MyContractBlockDefinition_003C_003EActor : IActivator, IActivator<MyContractBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyContractBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyContractBlockDefinition CreateInstance()
			{
				return new MyContractBlockDefinition();
			}

			MyContractBlockDefinition IActivator<MyContractBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
<<<<<<< HEAD
=======
			MyObjectBuilder_ContractBlockDefinition myObjectBuilder_ContractBlockDefinition = builder as MyObjectBuilder_ContractBlockDefinition;
			ScreenAreas = ((myObjectBuilder_ContractBlockDefinition.ScreenAreas != null) ? Enumerable.ToList<ScreenArea>((IEnumerable<ScreenArea>)myObjectBuilder_ContractBlockDefinition.ScreenAreas) : null);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
