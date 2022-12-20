using VRage.Game;
using VRageMath;

namespace Sandbox.Game.AI.BehaviorTree
{
	[MyBehaviorTreeNodeMemoryType(typeof(MyObjectBuilder_BehaviorTreeNodeMemory))]
	public class MyBehaviorTreeNodeMemory
	{
		public MyBehaviorTreeState NodeState { get; set; }

		public Color NodeStateColor => GetColorByState(NodeState);

		public bool InitCalled { get; set; }

		public bool IsTicked => NodeState != MyBehaviorTreeState.NOT_TICKED;

		public MyBehaviorTreeNodeMemory()
		{
			InitCalled = false;
			ClearNodeState();
		}

		public virtual void Init(MyObjectBuilder_BehaviorTreeNodeMemory builder)
		{
			InitCalled = builder.InitCalled;
		}

		public virtual MyObjectBuilder_BehaviorTreeNodeMemory GetObjectBuilder()
		{
			MyObjectBuilder_BehaviorTreeNodeMemory myObjectBuilder_BehaviorTreeNodeMemory = MyBehaviorTreeNodeMemoryFactory.CreateObjectBuilder(this);
			myObjectBuilder_BehaviorTreeNodeMemory.InitCalled = InitCalled;
			return myObjectBuilder_BehaviorTreeNodeMemory;
		}

		public virtual void ClearMemory()
		{
			NodeState = MyBehaviorTreeState.NOT_TICKED;
			InitCalled = false;
		}

		public virtual void PostTickMemory()
		{
		}

		public void ClearNodeState()
		{
			NodeState = MyBehaviorTreeState.NOT_TICKED;
		}

		private static Color GetColorByState(MyBehaviorTreeState state)
		{
			return state switch
			{
				MyBehaviorTreeState.ERROR => Color.Bisque, 
				MyBehaviorTreeState.FAILURE => Color.Red, 
				MyBehaviorTreeState.NOT_TICKED => Color.White, 
				MyBehaviorTreeState.RUNNING => Color.Yellow, 
				MyBehaviorTreeState.SUCCESS => Color.Green, 
				_ => Color.Black, 
			};
		}
	}
}
