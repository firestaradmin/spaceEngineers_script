using Sandbox.Common.ObjectBuilders;
using VRage.ObjectBuilders;

namespace Sandbox.Game.AI.Autopilots
{
	internal static class MyAutopilotFactory
	{
		private static readonly MyObjectFactory<MyAutopilotTypeAttribute, MyAutopilotBase> m_objectFactory;

		static MyAutopilotFactory()
		{
			m_objectFactory = new MyObjectFactory<MyAutopilotTypeAttribute, MyAutopilotBase>();
			m_objectFactory.RegisterFromCreatedObjectAssembly();
		}

		public static MyAutopilotBase CreateAutopilot(MyObjectBuilder_AutopilotBase builder)
		{
			return m_objectFactory.CreateInstance(builder.TypeId);
		}

		public static MyObjectBuilder_AutopilotBase CreateObjectBuilder(MyAutopilotBase autopilot)
		{
			return m_objectFactory.CreateObjectBuilder<MyObjectBuilder_AutopilotBase>(autopilot);
		}
	}
}
