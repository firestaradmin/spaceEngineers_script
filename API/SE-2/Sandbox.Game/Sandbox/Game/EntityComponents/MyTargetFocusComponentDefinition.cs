using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace Sandbox.Game.EntityComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_TargetFocusComponentDefinition), null)]
	public class MyTargetFocusComponentDefinition : MyComponentDefinitionBase
	{
		private class Sandbox_Game_EntityComponents_MyTargetFocusComponentDefinition_003C_003EActor : IActivator, IActivator<MyTargetFocusComponentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyTargetFocusComponentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTargetFocusComponentDefinition CreateInstance()
			{
				return new MyTargetFocusComponentDefinition();
			}

			MyTargetFocusComponentDefinition IActivator<MyTargetFocusComponentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public double FocusSearchMaxDistance;

		public double AngularToleranceFromCrosshair;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_TargetFocusComponentDefinition myObjectBuilder_TargetFocusComponentDefinition = builder as MyObjectBuilder_TargetFocusComponentDefinition;
			if (myObjectBuilder_TargetFocusComponentDefinition != null)
			{
				FocusSearchMaxDistance = myObjectBuilder_TargetFocusComponentDefinition.FocusSearchMaxDistance;
				AngularToleranceFromCrosshair = myObjectBuilder_TargetFocusComponentDefinition.AngularToleranceFromCrosshair;
			}
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_TargetFocusComponentDefinition obj = base.GetObjectBuilder() as MyObjectBuilder_TargetFocusComponentDefinition;
			obj.FocusSearchMaxDistance = FocusSearchMaxDistance;
			obj.AngularToleranceFromCrosshair = AngularToleranceFromCrosshair;
			return obj;
		}
	}
}
