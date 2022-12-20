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
	[MyDefinitionType(typeof(MyObjectBuilder_SurvivalKitDefinition), null)]
	public class MySurvivalKitDefinition : MyAssemblerDefinition
	{
		private class Sandbox_Definitions_MySurvivalKitDefinition_003C_003EActor : IActivator, IActivator<MySurvivalKitDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MySurvivalKitDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySurvivalKitDefinition CreateInstance()
			{
				return new MySurvivalKitDefinition();
			}

			MySurvivalKitDefinition IActivator<MySurvivalKitDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string ProgressSound = "BlockMedicalProgress";

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_SurvivalKitDefinition myObjectBuilder_SurvivalKitDefinition = (MyObjectBuilder_SurvivalKitDefinition)builder;
			ProgressSound = myObjectBuilder_SurvivalKitDefinition.ProgressSound;
<<<<<<< HEAD
=======
			ScreenAreas = ((myObjectBuilder_SurvivalKitDefinition.ScreenAreas != null) ? Enumerable.ToList<ScreenArea>((IEnumerable<ScreenArea>)myObjectBuilder_SurvivalKitDefinition.ScreenAreas) : null);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
