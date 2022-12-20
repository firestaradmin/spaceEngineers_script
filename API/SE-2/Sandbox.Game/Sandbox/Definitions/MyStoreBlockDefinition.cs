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
	[MyDefinitionType(typeof(MyObjectBuilder_StoreBlockDefinition), null)]
	public class MyStoreBlockDefinition : MyFunctionalBlockDefinition
	{
		private class Sandbox_Definitions_MyStoreBlockDefinition_003C_003EActor : IActivator, IActivator<MyStoreBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyStoreBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyStoreBlockDefinition CreateInstance()
			{
				return new MyStoreBlockDefinition();
			}

			MyStoreBlockDefinition IActivator<MyStoreBlockDefinition>.CreateInstance()
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
			MyObjectBuilder_StoreBlockDefinition myObjectBuilder_StoreBlockDefinition = builder as MyObjectBuilder_StoreBlockDefinition;
			ScreenAreas = ((myObjectBuilder_StoreBlockDefinition.ScreenAreas != null) ? Enumerable.ToList<ScreenArea>((IEnumerable<ScreenArea>)myObjectBuilder_StoreBlockDefinition.ScreenAreas) : null);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
