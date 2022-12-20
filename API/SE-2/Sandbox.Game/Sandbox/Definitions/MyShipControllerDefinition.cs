<<<<<<< HEAD
=======
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRageMath;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ShipControllerDefinition), null)]
	public class MyShipControllerDefinition : MyFunctionalBlockDefinition
	{
		private class Sandbox_Definitions_MyShipControllerDefinition_003C_003EActor : IActivator, IActivator<MyShipControllerDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyShipControllerDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyShipControllerDefinition CreateInstance()
			{
				return new MyShipControllerDefinition();
			}

			MyShipControllerDefinition IActivator<MyShipControllerDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public bool EnableFirstPerson;

		public bool EnableShipControl;

		public bool EnableBuilderCockpit;

		public string GlassModel;

		public string InteriorModel;

		public string CharacterAnimation;

		public string GetInSound;

		public string GetOutSound;

		public bool IsDefault3rdView;

<<<<<<< HEAD
		public bool TargetLockingEnabled;
=======
		public Vector3D RaycastOffset = Vector3D.Zero;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public Vector3D RaycastOffset = Vector3D.Zero;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ShipControllerDefinition myObjectBuilder_ShipControllerDefinition = builder as MyObjectBuilder_ShipControllerDefinition;
			EnableFirstPerson = myObjectBuilder_ShipControllerDefinition.EnableFirstPerson;
			EnableShipControl = myObjectBuilder_ShipControllerDefinition.EnableShipControl;
			EnableBuilderCockpit = myObjectBuilder_ShipControllerDefinition.EnableBuilderCockpit;
			GetInSound = myObjectBuilder_ShipControllerDefinition.GetInSound;
			GetOutSound = myObjectBuilder_ShipControllerDefinition.GetOutSound;
			RaycastOffset = myObjectBuilder_ShipControllerDefinition.RaycastOffset;
			IsDefault3rdView = myObjectBuilder_ShipControllerDefinition.IsDefault3rdView;
<<<<<<< HEAD
			TargetLockingEnabled = myObjectBuilder_ShipControllerDefinition.TargetLockingEnabled;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
