using Sandbox.Engine.Networking;
using VRage.GameServices;

namespace SpaceEngineers.Game.Achievements
{
	public static class MySpaceEngineersAchievements
	{
		public static void Initialize()
		{
			IMyGameService service = MyGameService.Service;
			service.RegisterAchievement("MyAchievment_ColorBlind", "1");
			service.RegisterAchievement("MyAchievement_IHaveGotPresentForYou", "2");
			service.RegisterAchievement("MyAchievement_LockAndLoad", "3");
			service.RegisterAchievement("MyAchievement_NumberFiveIsAlive", "4");
			service.RegisterAchievement("MyAchievement_Explorer", "5");
			service.RegisterAchievement("MyAchievement_GoingGreen", "6");
			service.RegisterAchievement("MyAchievement_LostInSpace", "7");
			service.RegisterAchievement("MyAchievement_MasterEngineer", "8");
			service.RegisterAchievement("MyAchievement_TheBiggerTheyAre", "9");
			service.RegisterAchievement("MyAchievement_TheHarderTheyFall", "10");
			service.RegisterAchievement("MyAchievement_ToTheStars", "11");
			service.RegisterAchievement("MyAchievment_DeclareWar", "12");
			service.RegisterAchievement("MyAchievement_Monolith", "13");
			service.RegisterAchievement("MyAchievement_PersonalityCrisis", "14");
			service.RegisterAchievement("MyAchievement_SmileAndWave", "15");
			service.RegisterAchievement("MyAchievement_WinWin", "16");
			service.RegisterAchievement("MyAchievement_DeathWish", "17");
			service.RegisterAchievement("MyAchievement_GiantLeapForMankind", "18");
			service.RegisterAchievement("Promoted_engineer", "19");
			service.RegisterAchievement("Engineering_degree", "20");
			service.RegisterAchievement("Planetesphobia", "21");
			service.RegisterAchievement("It_takes_but_one", "22");
			service.RegisterAchievement("I_see_dead_drones", "23");
			service.RegisterAchievement("Bring_it_on", "24");
			service.RegisterAchievement("Im_doing_my_part", "25");
			service.RegisterAchievement("Scrap_delivery", "26");
			service.RegisterAchievement("Joint_operation", "27");
			service.RegisterAchievement("MillionaireClub", "28");
			service.RegisterAchievement("FriendOfFactions", "29");
			service.RegisterAchievement("MyAchievement_PlayingItCool", "30");
		}
	}
}
