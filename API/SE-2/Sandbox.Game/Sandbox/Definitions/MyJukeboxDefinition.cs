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
	[MyDefinitionType(typeof(MyObjectBuilder_JukeboxDefinition), null)]
	public class MyJukeboxDefinition : MySoundBlockDefinition
	{
		private class Sandbox_Definitions_MyJukeboxDefinition_003C_003EActor : IActivator, IActivator<MyJukeboxDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyJukeboxDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyJukeboxDefinition CreateInstance()
			{
				return new MyJukeboxDefinition();
			}

			MyJukeboxDefinition IActivator<MyJukeboxDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
<<<<<<< HEAD
			_ = (MyObjectBuilder_JukeboxDefinition)builder;
=======
			MyObjectBuilder_JukeboxDefinition myObjectBuilder_JukeboxDefinition = (MyObjectBuilder_JukeboxDefinition)builder;
			ScreenAreas = ((myObjectBuilder_JukeboxDefinition.ScreenAreas != null) ? Enumerable.ToList<ScreenArea>((IEnumerable<ScreenArea>)myObjectBuilder_JukeboxDefinition.ScreenAreas) : null);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
