using System;
using VRage.Game.Common;

namespace VRage.Game.VisualScripting.ScriptBuilder
{
	public class MyVisualScriptTag : MyFactoryTagAttribute
	{
		public MyVisualScriptTag(Type objectBuilderType)
			: base(objectBuilderType)
		{
		}
	}
}
