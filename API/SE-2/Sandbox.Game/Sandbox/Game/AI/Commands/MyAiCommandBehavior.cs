using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Engine.Physics;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.World;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.AI.Commands
{
	public class MyAiCommandBehavior : IMyAiCommand
	{
		private static readonly List<MyPhysics.HitInfo> m_tmpHitInfos = new List<MyPhysics.HitInfo>();

		public MyAiCommandBehaviorDefinition Definition { get; private set; }

		public void InitCommand(MyAiCommandDefinition definition)
		{
			Definition = definition as MyAiCommandBehaviorDefinition;
		}

		public void ActivateCommand()
		{
			if (Definition.CommandEffect == MyAiCommandEffect.TARGET)
			{
				ChangeTarget();
			}
			else if (Definition.CommandEffect == MyAiCommandEffect.OWNED_BOTS)
			{
				ChangeAllBehaviors();
			}
		}

		private void ChangeTarget()
		{
			Vector3D vector3D;
			Vector3D forward;
			if (MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.ThirdPersonSpectator || MySession.Static.GetCameraControllerEnum() == MyCameraControllerEnum.Entity)
			{
				MatrixD headMatrix = MySession.Static.ControlledEntity.GetHeadMatrix(includeY: true);
				vector3D = headMatrix.Translation;
				forward = headMatrix.Forward;
			}
			else
			{
				vector3D = MySector.MainCamera.Position;
				forward = MySector.MainCamera.WorldMatrix.Forward;
			}
			m_tmpHitInfos.Clear();
			MyPhysics.CastRay(vector3D, vector3D + forward * 20.0, m_tmpHitInfos, 24);
			if (m_tmpHitInfos.Count == 0)
			{
				return;
			}
			foreach (MyPhysics.HitInfo tmpHitInfo in m_tmpHitInfos)
			{
				MyCharacter character;
				if ((character = tmpHitInfo.HkHitInfo.GetHitEntity() as MyCharacter) != null && TryGetBotForCharacter(character, out var bot) && bot.BotDefinition.Commandable)
				{
					ChangeBotBehavior(bot);
				}
			}
		}

		private void ChangeAllBehaviors()
		{
			foreach (KeyValuePair<int, IMyBot> allBot in MyAIComponent.Static.Bots.GetAllBots())
			{
				MyAgentBot myAgentBot;
				if ((myAgentBot = allBot.Value as MyAgentBot) != null && myAgentBot.BotDefinition.Commandable)
				{
					ChangeBotBehavior(myAgentBot);
				}
			}
		}

		private static bool TryGetBotForCharacter(MyCharacter character, out MyAgentBot bot)
		{
			bot = null;
			foreach (KeyValuePair<int, IMyBot> allBot in MyAIComponent.Static.Bots.GetAllBots())
			{
				MyAgentBot myAgentBot;
				if ((myAgentBot = allBot.Value as MyAgentBot) != null && myAgentBot.AgentEntity == character)
				{
					bot = myAgentBot;
					return true;
				}
			}
			return false;
		}

		private void ChangeBotBehavior(MyAgentBot bot)
		{
			MyAIComponent.Static.BehaviorTrees.ChangeBehaviorTree(Definition.BehaviorTreeName, bot);
		}
	}
}
