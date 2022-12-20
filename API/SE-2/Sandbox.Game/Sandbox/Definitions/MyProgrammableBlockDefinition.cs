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
	[MyDefinitionType(typeof(MyObjectBuilder_ProgrammableBlockDefinition), null)]
	public class MyProgrammableBlockDefinition : MyFunctionalBlockDefinition
	{
		private class Sandbox_Definitions_MyProgrammableBlockDefinition_003C_003EActor : IActivator, IActivator<MyProgrammableBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyProgrammableBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyProgrammableBlockDefinition CreateInstance()
			{
				return new MyProgrammableBlockDefinition();
			}

			MyProgrammableBlockDefinition IActivator<MyProgrammableBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ProgrammableBlockDefinition myObjectBuilder_ProgrammableBlockDefinition = (MyObjectBuilder_ProgrammableBlockDefinition)builder;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_ProgrammableBlockDefinition.ResourceSinkGroup);
<<<<<<< HEAD
=======
			ScreenAreas = ((myObjectBuilder_ProgrammableBlockDefinition.ScreenAreas != null) ? Enumerable.ToList<ScreenArea>((IEnumerable<ScreenArea>)myObjectBuilder_ProgrammableBlockDefinition.ScreenAreas) : null);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
