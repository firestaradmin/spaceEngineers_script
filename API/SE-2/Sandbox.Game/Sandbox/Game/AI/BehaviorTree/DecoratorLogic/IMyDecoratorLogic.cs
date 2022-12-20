using VRage.Game;

namespace Sandbox.Game.AI.BehaviorTree.DecoratorLogic
{
	public interface IMyDecoratorLogic
	{
		void Construct(MyObjectBuilder_BehaviorTreeDecoratorNode.Logic logicData);

		void Update(MyBehaviorTreeDecoratorNodeMemory.LogicMemory memory);

		bool CanRun(MyBehaviorTreeDecoratorNodeMemory.LogicMemory memory);

		MyBehaviorTreeDecoratorNodeMemory.LogicMemory GetNewMemoryObject();
	}
}
