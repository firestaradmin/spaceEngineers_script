using VRage.Game.Components.Session;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace VRage.Game.Definitions.SessionComponents
{
	[MyDefinitionType(typeof(MyObjectBuilder_SessionComponentSmartUpdaterDefinition), null)]
	public class MySessionComponentSmartUpdaterDefinition : MySessionComponentDefinition
	{
		private class VRage_Game_Definitions_SessionComponents_MySessionComponentSmartUpdaterDefinition_003C_003EActor : IActivator, IActivator<MySessionComponentSmartUpdaterDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MySessionComponentSmartUpdaterDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySessionComponentSmartUpdaterDefinition CreateInstance()
			{
				return new MySessionComponentSmartUpdaterDefinition();
			}

			MySessionComponentSmartUpdaterDefinition IActivator<MySessionComponentSmartUpdaterDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}
	}
}
