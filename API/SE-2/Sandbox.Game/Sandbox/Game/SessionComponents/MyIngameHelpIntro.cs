<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Linq;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Graphics.GUI;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Intro", 10)]
	internal class MyIngameHelpIntro : MyIngameHelpObjective
	{
		private bool m_F1pressed;

		public MyIngameHelpIntro()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Intro_Title;
			Details = new MyIngameHelpDetail[2]
			{
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Intro_Detail1
				},
				new MyIngameHelpDetail
				{
					TextEnum = MySpaceTexts.IngameHelp_Intro_Detail2,
					Args = new object[1] { MyIngameHelpObjective.GetHighlightedControl(MyControlsSpace.HELP_SCREEN) },
					FinishCondition = F1Condition
				}
			};
			FollowingId = "IngameHelp_IntroTip";
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
		}

		private bool F1Condition()
		{
			if (Enumerable.Any<MyGuiScreenBase>(MyScreenManager.Screens, (Func<MyGuiScreenBase, bool>)((MyGuiScreenBase x) => x is MyGuiScreenHelpSpace)))
			{
				m_F1pressed = true;
			}
			return m_F1pressed;
		}
	}
}
