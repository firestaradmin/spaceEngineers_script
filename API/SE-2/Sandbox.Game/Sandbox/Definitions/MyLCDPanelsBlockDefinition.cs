<<<<<<< HEAD
=======
using System.Collections.Generic;
using System.Linq;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_LCDPanelsBlockDefinition), null)]
	public class MyLCDPanelsBlockDefinition : MyFunctionalBlockDefinition
	{
		private class Sandbox_Definitions_MyLCDPanelsBlockDefinition_003C_003EActor : IActivator, IActivator<MyLCDPanelsBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyLCDPanelsBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyLCDPanelsBlockDefinition CreateInstance()
			{
				return new MyLCDPanelsBlockDefinition();
			}

			MyLCDPanelsBlockDefinition IActivator<MyLCDPanelsBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float RequiredPowerInput;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_LCDPanelsBlockDefinition myObjectBuilder_LCDPanelsBlockDefinition = (MyObjectBuilder_LCDPanelsBlockDefinition)builder;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_LCDPanelsBlockDefinition.ResourceSinkGroup);
			RequiredPowerInput = myObjectBuilder_LCDPanelsBlockDefinition.RequiredPowerInput;
<<<<<<< HEAD
=======
			ScreenAreas = ((myObjectBuilder_LCDPanelsBlockDefinition.ScreenAreas != null) ? Enumerable.ToList<ScreenArea>((IEnumerable<ScreenArea>)myObjectBuilder_LCDPanelsBlockDefinition.ScreenAreas) : null);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
