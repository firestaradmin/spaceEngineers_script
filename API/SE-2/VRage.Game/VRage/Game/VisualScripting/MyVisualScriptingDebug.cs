using System.Collections.Generic;
using VRage.Game.VisualScripting.Missions;

namespace VRage.Game.VisualScripting
{
	public static class MyVisualScriptingDebug
	{
		private static List<MyDebuggingNodeLog> m_nodes = new List<MyDebuggingNodeLog>();

		private static List<MyDebuggingStateMachine> m_stateMachines = new List<MyDebuggingStateMachine>();

		public static IReadOnlyList<MyDebuggingNodeLog> LoggedNodes => m_nodes;

		public static IReadOnlyList<MyDebuggingStateMachine> StateMachines => m_stateMachines;

		public static void LogNode(int nodeID, params object[] values)
		{
		}

		public static void LogStateMachine(MyVSStateMachine machine)
		{
		}

		public static void Clear()
		{
			m_nodes.Clear();
			m_stateMachines.Clear();
		}
	}
}
