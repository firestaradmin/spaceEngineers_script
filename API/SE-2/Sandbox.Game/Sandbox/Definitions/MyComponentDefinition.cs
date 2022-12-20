using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ComponentDefinition), null)]
	public class MyComponentDefinition : MyPhysicalItemDefinition
	{
		private class Sandbox_Definitions_MyComponentDefinition_003C_003EActor : IActivator, IActivator<MyComponentDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyComponentDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyComponentDefinition CreateInstance()
			{
				return new MyComponentDefinition();
			}

			MyComponentDefinition IActivator<MyComponentDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		/// <summary>
		/// HP of the component. Used when calculating overall HP of block from its components.
		/// </summary>
		public int MaxIntegrity;

		/// <summary>
		/// Chance that the damaged component will be dropped when damage is inflicted to a component stack.
		/// Percentage given as value from 0 to 1.
		/// </summary>
		public float DropProbability;

		public float DeconstructionEfficiency;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ComponentDefinition myObjectBuilder_ComponentDefinition = builder as MyObjectBuilder_ComponentDefinition;
			MaxIntegrity = myObjectBuilder_ComponentDefinition.MaxIntegrity;
			DropProbability = myObjectBuilder_ComponentDefinition.DropProbability;
			DeconstructionEfficiency = myObjectBuilder_ComponentDefinition.DeconstructionEfficiency;
		}
	}
}
