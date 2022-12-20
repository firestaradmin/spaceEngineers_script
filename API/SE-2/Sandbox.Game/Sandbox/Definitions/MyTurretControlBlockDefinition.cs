using SpaceEngineers.ObjectBuilders.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_TurretControlBlockDefinition), null)]
	public class MyTurretControlBlockDefinition : MyFunctionalBlockDefinition
	{
		private class Sandbox_Definitions_MyTurretControlBlockDefinition_003C_003EActor : IActivator, IActivator<MyTurretControlBlockDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyTurretControlBlockDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTurretControlBlockDefinition CreateInstance()
			{
				return new MyTurretControlBlockDefinition();
			}

			MyTurretControlBlockDefinition IActivator<MyTurretControlBlockDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float MaxRangeMeters;

		public float PlayerInputDivider;

		public MyStringHash ResourceSinkGroup;

		public float PowerInputIdle;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_TurretControlBlockDefinition myObjectBuilder_TurretControlBlockDefinition = (MyObjectBuilder_TurretControlBlockDefinition)builder;
			MaxRangeMeters = myObjectBuilder_TurretControlBlockDefinition.MaxRangeMeters;
			PlayerInputDivider = myObjectBuilder_TurretControlBlockDefinition.PlayerInputDivider;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_TurretControlBlockDefinition.ResourceSinkGroup);
			PowerInputIdle = myObjectBuilder_TurretControlBlockDefinition.PowerInputIdle;
		}
	}
}
