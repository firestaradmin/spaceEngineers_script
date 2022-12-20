using System.Collections.Generic;
using System.Linq;
using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_FunctionalBlockDefinition), null)]
	public class MyFunctionalBlockDefinition : MyCubeBlockDefinition
	{
		private class Sandbox_Definitions_MyFunctionalBlockDefinition_003C_003EActor : IActivator, IActivator<MyFunctionalBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyFunctionalBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFunctionalBlockDefinition CreateInstance()
			{
				return new MyFunctionalBlockDefinition();
			}

			MyFunctionalBlockDefinition IActivator<MyFunctionalBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<ScreenArea> ScreenAreas;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_FunctionalBlockDefinition myObjectBuilder_FunctionalBlockDefinition = (MyObjectBuilder_FunctionalBlockDefinition)builder;
			ScreenAreas = ((myObjectBuilder_FunctionalBlockDefinition.ScreenAreas != null) ? myObjectBuilder_FunctionalBlockDefinition.ScreenAreas.ToList() : null);
		}
	}
}
