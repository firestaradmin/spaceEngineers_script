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
	[MyDefinitionType(typeof(MyObjectBuilder_CockpitDefinition), null)]
	public class MyCockpitDefinition : MyShipControllerDefinition
	{
		private class Sandbox_Definitions_MyCockpitDefinition_003C_003EActor : IActivator, IActivator<MyCockpitDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyCockpitDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyCockpitDefinition CreateInstance()
			{
				return new MyCockpitDefinition();
			}

			MyCockpitDefinition IActivator<MyCockpitDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float OxygenCapacity;

		public bool IsPressurized;

		public string HUD;

		public bool HasInventory;

		public bool BackpackEnabled;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_CockpitDefinition myObjectBuilder_CockpitDefinition = builder as MyObjectBuilder_CockpitDefinition;
			GlassModel = myObjectBuilder_CockpitDefinition.GlassModel;
			InteriorModel = myObjectBuilder_CockpitDefinition.InteriorModel;
			CharacterAnimation = myObjectBuilder_CockpitDefinition.CharacterAnimation ?? myObjectBuilder_CockpitDefinition.CharacterAnimationFile;
			if (!string.IsNullOrEmpty(myObjectBuilder_CockpitDefinition.CharacterAnimationFile))
			{
				MyDefinitionErrors.Add(Context, "<CharacterAnimation> tag must contain animation name (defined in Animations.sbc) not the file: " + myObjectBuilder_CockpitDefinition.CharacterAnimationFile, TErrorSeverity.Error);
			}
			OxygenCapacity = myObjectBuilder_CockpitDefinition.OxygenCapacity;
			IsPressurized = myObjectBuilder_CockpitDefinition.IsPressurized;
			HasInventory = myObjectBuilder_CockpitDefinition.HasInventory;
			HUD = myObjectBuilder_CockpitDefinition.HUD;
<<<<<<< HEAD
			BackpackEnabled = myObjectBuilder_CockpitDefinition.BackpackEnabled;
=======
			ScreenAreas = ((myObjectBuilder_CockpitDefinition.ScreenAreas != null) ? Enumerable.ToList<ScreenArea>((IEnumerable<ScreenArea>)myObjectBuilder_CockpitDefinition.ScreenAreas) : null);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
