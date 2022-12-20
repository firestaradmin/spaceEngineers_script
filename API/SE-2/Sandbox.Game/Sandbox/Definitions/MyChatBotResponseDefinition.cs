using VRage.Game;
using VRage.Game.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_ChatBotResponseDefinition), null)]
	public class MyChatBotResponseDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyChatBotResponseDefinition_003C_003EActor : IActivator, IActivator<MyChatBotResponseDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyChatBotResponseDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyChatBotResponseDefinition CreateInstance()
			{
				return new MyChatBotResponseDefinition();
			}

			MyChatBotResponseDefinition IActivator<MyChatBotResponseDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public string Response;

		public string[] Questions;

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_ChatBotResponseDefinition myObjectBuilder_ChatBotResponseDefinition = (MyObjectBuilder_ChatBotResponseDefinition)builder;
			Response = myObjectBuilder_ChatBotResponseDefinition.Response;
			Questions = myObjectBuilder_ChatBotResponseDefinition.Questions;
		}
	}
}
