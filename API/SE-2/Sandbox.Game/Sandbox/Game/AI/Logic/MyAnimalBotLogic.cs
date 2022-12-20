using Sandbox.Game.AI.Navigation;
using VRageMath;

namespace Sandbox.Game.AI.Logic
{
	public class MyAnimalBotLogic : MyAgentLogic
	{
		private readonly MyCharacterAvoidanceSteering m_characterAvoidanceSteering;

		public MyAnimalBot AnimalBot => m_bot as MyAnimalBot;

		public override BotType BotType => BotType.ANIMAL;

		public MyAnimalBotLogic(MyAnimalBot bot)
			: base(bot)
		{
			MyBotNavigation navigation = AnimalBot.Navigation;
			navigation.AddSteering(new MyTreeAvoidance(navigation, 0.1f));
			m_characterAvoidanceSteering = new MyCharacterAvoidanceSteering(navigation, 1f);
			navigation.AddSteering(m_characterAvoidanceSteering);
			navigation.MaximumRotationAngle = MathHelper.ToRadians(23f);
		}

		public void EnableCharacterAvoidance(bool isTrue)
		{
			MyBotNavigation navigation = AnimalBot.Navigation;
			bool flag = navigation.HasSteeringOfType(m_characterAvoidanceSteering.GetType());
			if (isTrue && !flag)
			{
				navigation.AddSteering(m_characterAvoidanceSteering);
			}
			else if (!isTrue && flag)
			{
				navigation.RemoveSteering(m_characterAvoidanceSteering);
			}
		}
	}
}
