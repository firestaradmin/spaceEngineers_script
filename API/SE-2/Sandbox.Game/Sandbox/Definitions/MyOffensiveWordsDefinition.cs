using System.Collections.Generic;
using System.Linq;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Network;

namespace Sandbox.Definitions
{
	[MyDefinitionType(typeof(MyObjectBuilder_OffensiveWords), null)]
	public class MyOffensiveWordsDefinition : MyDefinitionBase
	{
		private class Sandbox_Definitions_MyOffensiveWordsDefinition_003C_003EActor : IActivator, IActivator<MyOffensiveWordsDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyOffensiveWordsDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyOffensiveWordsDefinition CreateInstance()
			{
				return new MyOffensiveWordsDefinition();
			}

			MyOffensiveWordsDefinition IActivator<MyOffensiveWordsDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public List<string> Blacklist = new List<string>();

		public List<string> Whitelist = new List<string>();

		protected override void Init(MyObjectBuilder_DefinitionBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_OffensiveWords myObjectBuilder_OffensiveWords = (MyObjectBuilder_OffensiveWords)builder;
			if (myObjectBuilder_OffensiveWords.Blacklist != null)
			{
<<<<<<< HEAD
				Blacklist = myObjectBuilder_OffensiveWords.Blacklist.ToList();
			}
			if (myObjectBuilder_OffensiveWords.Whitelist != null)
			{
				Whitelist = myObjectBuilder_OffensiveWords.Whitelist.ToList();
=======
				Blacklist = Enumerable.ToList<string>((IEnumerable<string>)myObjectBuilder_OffensiveWords.Blacklist);
			}
			if (myObjectBuilder_OffensiveWords.Whitelist != null)
			{
				Whitelist = Enumerable.ToList<string>((IEnumerable<string>)myObjectBuilder_OffensiveWords.Whitelist);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}
	}
}
