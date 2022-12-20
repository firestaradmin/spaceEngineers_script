using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;
using VRage.ObjectBuilder;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ControllerSchemeDefinition), null)]
	public class MyControllerSchemeDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyControllerSchemeDefinition_003C_003EActor : IActivator, IActivator<MyControllerSchemeDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyControllerSchemeDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyControllerSchemeDefinition CreateInstance()
			{
				return new MyControllerSchemeDefinition();
			}

			MyControllerSchemeDefinition IActivator<MyControllerSchemeDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public bool IsSelectable;

		public bool IsDefault;

		public List<MyObjectBuilder_ControlBindingContext> Contexts = new List<MyObjectBuilder_ControlBindingContext>();

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ControllerSchemeDefinition myObjectBuilder_ControllerSchemeDefinition = builder as MyObjectBuilder_ControllerSchemeDefinition;
			if (myObjectBuilder_ControllerSchemeDefinition != null)
			{
				IsSelectable = myObjectBuilder_ControllerSchemeDefinition.IsSelectable;
				IsDefault = myObjectBuilder_ControllerSchemeDefinition.IsDefault;
				Contexts = myObjectBuilder_ControllerSchemeDefinition.Contexts;
			}
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_ControllerSchemeDefinition myObjectBuilder_ControllerSchemeDefinition = base.GetObjectBuilder() as MyObjectBuilder_ControllerSchemeDefinition;
			myObjectBuilder_ControllerSchemeDefinition.Contexts = new MySerializableList<MyObjectBuilder_ControlBindingContext>();
			foreach (MyObjectBuilder_ControlBindingContext context in Contexts)
			{
				myObjectBuilder_ControllerSchemeDefinition.Contexts.Add(context);
			}
			return myObjectBuilder_ControllerSchemeDefinition;
		}
	}
}
