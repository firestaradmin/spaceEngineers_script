using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.ComponentSystem;
using VRage.Network;

namespace Sandbox.Game.EntityComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_EntityCapacitorComponentDefinition), null)]
	public class MyEntityCapacitorComponentDefinition : MyComponentDefinitionBase
	{
		private class Sandbox_Game_EntityComponents_MyEntityCapacitorComponentDefinition_003C_003EActor : IActivator, IActivator<MyEntityCapacitorComponentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyEntityCapacitorComponentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyEntityCapacitorComponentDefinition CreateInstance()
			{
				return new MyEntityCapacitorComponentDefinition();
			}

			MyEntityCapacitorComponentDefinition IActivator<MyEntityCapacitorComponentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float Capacity;

		public float RechargeDraw;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_EntityCapacitorComponentDefinition myObjectBuilder_EntityCapacitorComponentDefinition = builder as MyObjectBuilder_EntityCapacitorComponentDefinition;
			if (myObjectBuilder_EntityCapacitorComponentDefinition != null)
			{
				Capacity = myObjectBuilder_EntityCapacitorComponentDefinition.Capacity;
				RechargeDraw = myObjectBuilder_EntityCapacitorComponentDefinition.RechargeDraw;
			}
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_EntityCapacitorComponentDefinition obj = base.GetObjectBuilder() as MyObjectBuilder_EntityCapacitorComponentDefinition;
			obj.Capacity = Capacity;
			obj.RechargeDraw = RechargeDraw;
			return obj;
		}
	}
}
