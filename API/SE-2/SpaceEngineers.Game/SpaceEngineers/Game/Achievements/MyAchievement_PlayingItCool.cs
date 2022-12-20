using Sandbox.Game.SessionComponents;

namespace SpaceEngineers.Game.Achievements
{
	internal class MyAchievement_PlayingItCool : MySteamAchievementBase
	{
		public override bool NeedsUpdate => false;

		protected override (string, string, float) GetAchievementInfo()
		{
			return ("MyAchievement_PlayingItCool", null, 0f);
		}
	}
}
