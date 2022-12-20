using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace Sandbox.Game.EntityComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_TargetLockingComponentDefinition), null)]
	public class MyTargetLockingComponentDefinition : MyComponentDefinitionBase
	{
		private class Sandbox_Game_EntityComponents_MyTargetLockingComponentDefinition_003C_003EActor : IActivator, IActivator<MyTargetLockingComponentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyTargetLockingComponentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTargetLockingComponentDefinition CreateInstance()
			{
				return new MyTargetLockingComponentDefinition();
			}

			MyTargetLockingComponentDefinition IActivator<MyTargetLockingComponentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float LockingModifierSmallGrid;

		public float LockingModifierLargeGrid;

		public float LockingTimeMin;

		public float LockingTimeMax;

		public float LockingModifierDistance;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_TargetLockingComponentDefinition myObjectBuilder_TargetLockingComponentDefinition;
			if ((myObjectBuilder_TargetLockingComponentDefinition = builder as MyObjectBuilder_TargetLockingComponentDefinition) != null)
			{
				LockingModifierSmallGrid = myObjectBuilder_TargetLockingComponentDefinition.LockingModifierSmallGrid;
				LockingModifierLargeGrid = myObjectBuilder_TargetLockingComponentDefinition.LockingModifierLargeGrid;
				LockingTimeMin = myObjectBuilder_TargetLockingComponentDefinition.LockingTimeMin;
				LockingTimeMax = myObjectBuilder_TargetLockingComponentDefinition.LockingTimeMax;
				LockingModifierDistance = myObjectBuilder_TargetLockingComponentDefinition.LockingModifierDistance;
			}
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_TargetLockingComponentDefinition obj = base.GetObjectBuilder() as MyObjectBuilder_TargetLockingComponentDefinition;
			obj.LockingModifierSmallGrid = LockingModifierSmallGrid;
			obj.LockingModifierLargeGrid = LockingModifierLargeGrid;
			obj.LockingTimeMin = LockingTimeMin;
			obj.LockingTimeMax = LockingTimeMax;
			obj.LockingModifierDistance = LockingModifierDistance;
			return obj;
		}
	}
}
