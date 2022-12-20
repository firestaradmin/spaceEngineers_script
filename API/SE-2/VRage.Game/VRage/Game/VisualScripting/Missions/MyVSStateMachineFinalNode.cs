using System;
using System.Linq;
using VRage.Collections;
using VRage.Generics;
using VRage.Utils;

namespace VRage.Game.VisualScripting.Missions
{
	public class MyVSStateMachineFinalNode : MyStateMachineNode
	{
		public static Action<string, string, bool, bool> Finished;

		private bool m_showCredits;

		private bool m_closeSession;

		public MyVSStateMachineFinalNode(string name, bool showCredits, bool closeSession)
			: base(name)
		{
			m_showCredits = showCredits;
			m_closeSession = closeSession;
		}

		protected override void ExpandInternal(MyStateMachineCursor cursor, MyConcurrentHashSet<MyStringId> enquedActions, int passThrough)
		{
			MyStateMachineTransition myStateMachineTransition = cursor.StateMachine.FindTransition(cursor.LastTransitionTakenId);
			if (myStateMachineTransition == null && cursor.StateMachine.AllTransitions.Count > 0)
			{
<<<<<<< HEAD
				myStateMachineTransition = cursor.StateMachine.AllTransitions.Values.Last().Transition;
=======
				myStateMachineTransition = Enumerable.Last<MyStateMachineTransitionWithStart>(cursor.StateMachine.AllTransitions.Values).Transition;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (Finished != null && myStateMachineTransition != null)
			{
				Finished(cursor.StateMachine.Name, myStateMachineTransition.Name.ToString(), m_showCredits, m_closeSession);
			}
			foreach (MyStateMachineCursor activeCursor in cursor.StateMachine.ActiveCursors)
			{
				cursor.StateMachine.DeleteCursor(activeCursor.Id);
			}
		}
	}
}
