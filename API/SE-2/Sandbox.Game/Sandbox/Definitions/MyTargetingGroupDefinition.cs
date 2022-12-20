using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;
using VRage.ObjectBuilders;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_TargetingGroupDefinition), null)]
	public class MyTargetingGroupDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyTargetingGroupDefinition_003C_003EActor : IActivator, IActivator<MyTargetingGroupDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyTargetingGroupDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTargetingGroupDefinition CreateInstance()
			{
				return new MyTargetingGroupDefinition();
			}

			MyTargetingGroupDefinition IActivator<MyTargetingGroupDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public float SortOrder;

		public string ActionIcon;

		public HashSet<MyObjectBuilderType> DefaultBlockTypes;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_TargetingGroupDefinition myObjectBuilder_TargetingGroupDefinition = builder as MyObjectBuilder_TargetingGroupDefinition;
			SortOrder = myObjectBuilder_TargetingGroupDefinition.SortOrder;
			if (myObjectBuilder_TargetingGroupDefinition.Icons != null && myObjectBuilder_TargetingGroupDefinition.Icons.Count() > 0)
			{
				ActionIcon = myObjectBuilder_TargetingGroupDefinition.Icons[0];
			}
			DefaultBlockTypes = new HashSet<MyObjectBuilderType>();
			if (myObjectBuilder_TargetingGroupDefinition.DefaultBlockTypes == null)
			{
				return;
			}
			string[] defaultBlockTypes = myObjectBuilder_TargetingGroupDefinition.DefaultBlockTypes;
			foreach (string text in defaultBlockTypes)
			{
				if (MyObjectBuilderType.TryParse("MyObjectBuilder_" + text, out var result))
				{
					DefaultBlockTypes.Add(result);
				}
			}
		}
	}
}
