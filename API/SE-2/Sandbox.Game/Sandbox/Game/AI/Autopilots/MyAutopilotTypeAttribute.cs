using System;
using VRage.Game.Common;

namespace Sandbox.Game.AI.Autopilots
{
	internal class MyAutopilotTypeAttribute : MyFactoryTagAttribute
	{
		public MyAutopilotTypeAttribute(Type objectBuilderType)
			: base(objectBuilderType)
		{
		}
	}
}
