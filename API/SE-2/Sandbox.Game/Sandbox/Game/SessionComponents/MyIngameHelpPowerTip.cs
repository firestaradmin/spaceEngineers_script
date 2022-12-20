using Sandbox.Game.Localization;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_PowerTip", 100)]
	internal class MyIngameHelpPowerTip : MyIngameHelpObjective
	{
		public MyIngameHelpPowerTip()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Power_Title;
			RequiredIds = new string[1] { "IngameHelp_Power" };
<<<<<<< HEAD
			Details = new MyIngameHelpDetail[3]
=======
			Details = new MyIngameHelpDetail[2]
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_PowerTip_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_PowerTip_Detail2
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_PowerTip_Detail3
				}
			};
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY * 8f;
		}
	}
}
