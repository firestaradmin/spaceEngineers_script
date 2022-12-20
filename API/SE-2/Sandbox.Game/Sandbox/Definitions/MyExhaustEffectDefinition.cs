using System.Collections.Generic;
using System.Linq;
using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ExhaustEffectDefinition), null)]
	public class MyExhaustEffectDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyExhaustEffectDefinition_003C_003EActor : IActivator, IActivator<MyExhaustEffectDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyExhaustEffectDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyExhaustEffectDefinition CreateInstance()
			{
				return new MyExhaustEffectDefinition();
			}

			MyExhaustEffectDefinition IActivator<MyExhaustEffectDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<MyObjectBuilder_ExhaustEffectDefinition.Pipe> ExhaustPipes;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ExhaustEffectDefinition myObjectBuilder_ExhaustEffectDefinition = (MyObjectBuilder_ExhaustEffectDefinition)builder;
<<<<<<< HEAD
			ExhaustPipes = ((myObjectBuilder_ExhaustEffectDefinition.ExhaustPipes != null) ? myObjectBuilder_ExhaustEffectDefinition.ExhaustPipes.ToList() : null);
=======
			ExhaustPipes = ((myObjectBuilder_ExhaustEffectDefinition.ExhaustPipes != null) ? Enumerable.ToList<MyObjectBuilder_ExhaustEffectDefinition.Pipe>((IEnumerable<MyObjectBuilder_ExhaustEffectDefinition.Pipe>)myObjectBuilder_ExhaustEffectDefinition.ExhaustPipes) : null);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
		{
			MyObjectBuilder_ExhaustEffectDefinition myObjectBuilder_ExhaustEffectDefinition = (MyObjectBuilder_ExhaustEffectDefinition)base.GetObjectBuilder();
			if (ExhaustPipes != null)
			{
				myObjectBuilder_ExhaustEffectDefinition.ExhaustPipes = new List<MyObjectBuilder_ExhaustEffectDefinition.Pipe>();
				myObjectBuilder_ExhaustEffectDefinition.ExhaustPipes.AddList(ExhaustPipes);
			}
			return myObjectBuilder_ExhaustEffectDefinition;
		}
	}
}
