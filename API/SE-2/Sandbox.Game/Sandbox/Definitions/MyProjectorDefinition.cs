<<<<<<< HEAD
=======
using System.Collections.Generic;
using System.Linq;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.Utils;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ProjectorDefinition), null)]
	public class MyProjectorDefinition : MyFunctionalBlockDefinition
	{
		private class Sandbox_Definitions_MyProjectorDefinition_003C_003EActor : IActivator, IActivator<MyProjectorDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyProjectorDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyProjectorDefinition CreateInstance()
			{
				return new MyProjectorDefinition();
			}

			MyProjectorDefinition IActivator<MyProjectorDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public MyStringHash ResourceSinkGroup;

		public float RequiredPowerInput;

		public MySoundPair IdleSound;

		public bool AllowScaling;

		public bool AllowWelding;

		public bool IgnoreSize;

		public int RotationAngleStepDeg;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ProjectorDefinition myObjectBuilder_ProjectorDefinition = builder as MyObjectBuilder_ProjectorDefinition;
			ResourceSinkGroup = MyStringHash.GetOrCompute(myObjectBuilder_ProjectorDefinition.ResourceSinkGroup);
			RequiredPowerInput = myObjectBuilder_ProjectorDefinition.RequiredPowerInput;
			IdleSound = new MySoundPair(myObjectBuilder_ProjectorDefinition.IdleSound);
			AllowScaling = myObjectBuilder_ProjectorDefinition.AllowScaling;
			AllowWelding = myObjectBuilder_ProjectorDefinition.AllowWelding;
			IgnoreSize = myObjectBuilder_ProjectorDefinition.IgnoreSize;
			RotationAngleStepDeg = myObjectBuilder_ProjectorDefinition.RotationAngleStepDeg;
<<<<<<< HEAD
=======
			ScreenAreas = ((myObjectBuilder_ProjectorDefinition.ScreenAreas != null) ? Enumerable.ToList<ScreenArea>((IEnumerable<ScreenArea>)myObjectBuilder_ProjectorDefinition.ScreenAreas) : null);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
