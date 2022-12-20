using System;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Linq;
using Sandbox.Game.Entities;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Graphics.GUI;
using VRage.Input;

namespace Sandbox.Game.SessionComponents
{
	[IngameObjective("IngameHelp_Building", 60)]
	internal class MyIngameHelpBuilding : MyIngameHelpObjective
	{
		private class MouseAndKeyboardVersion : IHelplet
		{
			private bool m_blockSelected;

			private bool m_gPressed;

			private bool m_toolbarDrop;

			public MouseAndKeyboardVersion(MyIngameHelpObjective help)
			{
				help.RequiredCondition = BlockInToolbarSelected;
				help.Details = new MyIngameHelpDetail[4]
				{
					new MyIngameHelpDetail
					{
						TextEnum = MySpaceTexts.IngameHelp_Building_Detail1
					},
					new MyIngameHelpDetail
					{
						TextEnum = MySpaceTexts.IngameHelp_Building_Detail2,
						FinishCondition = ToolbarConfigScreenCondition
					},
					new MyIngameHelpDetail
					{
						TextEnum = MySpaceTexts.IngameHelp_Building_Detail3,
						FinishCondition = ToolbarDropCondition
					},
					new MyIngameHelpDetail
					{
						TextEnum = MySpaceTexts.IngameHelp_Building_Detail4,
						FinishCondition = BlockInToolbarSelected
					}
				};
				if (MyToolbarComponent.CurrentToolbar != null)
				{
					MyToolbarComponent.CurrentToolbar.SlotActivated += CurrentToolbar_SlotActivated;
					MyToolbarComponent.CurrentToolbarChanged += MyToolbarComponent_CurrentToolbarChanged;
				}
			}

			public void OnActivated()
			{
				MyToolbarComponent.CurrentToolbar.ItemChanged += CurrentToolbar_ItemChanged;
			}

			private void MyToolbarComponent_CurrentToolbarChanged(MyToolbar old, MyToolbar current)
			{
				if (old != null)
				{
					old.SlotActivated -= CurrentToolbar_SlotActivated;
					old.ItemChanged -= CurrentToolbar_ItemChanged;
				}
				MyToolbarComponent.CurrentToolbar.SlotActivated += CurrentToolbar_SlotActivated;
				MyToolbarComponent.CurrentToolbar.ItemChanged += CurrentToolbar_ItemChanged;
			}

			public void CleanUp()
			{
				if (MyToolbarComponent.CurrentToolbar != null)
				{
					MyToolbarComponent.CurrentToolbar.SlotActivated -= CurrentToolbar_SlotActivated;
					MyToolbarComponent.CurrentToolbar.ItemChanged -= CurrentToolbar_ItemChanged;
				}
				MyToolbarComponent.CurrentToolbarChanged -= MyToolbarComponent_CurrentToolbarChanged;
			}

			private void CurrentToolbar_ItemChanged(MyToolbar arg1, MyToolbar.IndexArgs arg2, bool isGamepad)
			{
				m_toolbarDrop = true;
			}

			private void CurrentToolbar_SlotActivated(MyToolbar toolbar, MyToolbar.SlotArgs args, bool userActivated)
			{
				if (toolbar.SelectedItem is MyToolbarItemCubeBlock && userActivated)
				{
					m_blockSelected = true;
				}
			}

			private bool BlockInToolbarSelected()
			{
				return m_blockSelected;
			}

			private bool ToolbarConfigScreenCondition()
			{
				m_gPressed |= Enumerable.Any<MyGuiScreenBase>(MyScreenManager.Screens, (Func<MyGuiScreenBase, bool>)((MyGuiScreenBase x) => x is MyGuiScreenToolbarConfigBase));
				return m_gPressed;
			}

			private bool ToolbarDropCondition()
			{
				return m_toolbarDrop;
			}
		}

		private class GamepadVersion : IHelplet
		{
			private bool m_blockSelected;

			private bool m_radialMenuOpened;

			private bool m_radialMenuTabsSwitched;

			private int m_initialRadialMenuTab = -1;

			public GamepadVersion(MyIngameHelpObjective help)
			{
				help.RequiredCondition = BlockInToolbarSelected;
				help.Details = new MyIngameHelpDetail[4]
				{
					new MyIngameHelpDetail
					{
						TextEnum = MySpaceTexts.IngameHelp_Building_Detail1
					},
					new MyIngameHelpDetail
					{
						TextEnum = MySpaceTexts.IngameHelp_Building_Detail2_Gamepad,
						FinishCondition = BuildingRadialMenuCondition
					},
					new MyIngameHelpDetail
					{
						TextEnum = MySpaceTexts.IngameHelp_Building_Detail3_Gamepad,
						FinishCondition = RadialMenuTabCondition
					},
					new MyIngameHelpDetail
					{
						TextEnum = MySpaceTexts.IngameHelp_Building_Detail4_Gamepad,
						FinishCondition = BlockInToolbarSelected
					}
				};
			}

			public void OnActivated()
			{
			}

			public void CleanUp()
			{
			}

			private bool RadialMenuTabCondition()
			{
				MyGuiControlRadialMenuBlock myGuiControlRadialMenuBlock = Enumerable.FirstOrDefault<MyGuiControlRadialMenuBlock>(Enumerable.OfType<MyGuiControlRadialMenuBlock>((IEnumerable)MyScreenManager.Screens));
				if (myGuiControlRadialMenuBlock != null)
				{
					m_radialMenuTabsSwitched |= m_initialRadialMenuTab != myGuiControlRadialMenuBlock.CurrentTabIndex;
				}
				return m_radialMenuTabsSwitched;
			}

			private bool BlockInToolbarSelected()
			{
				m_blockSelected |= MyCubeBuilder.Static.CurrentBlockDefinition != null;
				return m_blockSelected;
			}

			private bool BuildingRadialMenuCondition()
			{
				MyGuiControlRadialMenuBlock myGuiControlRadialMenuBlock = Enumerable.FirstOrDefault<MyGuiControlRadialMenuBlock>(Enumerable.OfType<MyGuiControlRadialMenuBlock>((IEnumerable)MyScreenManager.Screens));
				if (myGuiControlRadialMenuBlock != null)
				{
					m_radialMenuOpened = true;
					if (m_initialRadialMenuTab != -1)
					{
						m_initialRadialMenuTab = myGuiControlRadialMenuBlock.CurrentTabIndex;
					}
				}
				return m_radialMenuOpened;
			}
		}

		private IHelplet m_helplet;

		public MyIngameHelpBuilding()
		{
			TitleEnum = MySpaceTexts.IngameHelp_Building_Title;
			RequiredIds = new string[1] { "IngameHelp_Intro" };
			DelayToHide = MySessionComponentIngameHelp.DEFAULT_OBJECTIVE_DELAY;
			FollowingId = "IngameHelp_BuildingTip";
			DelayToAppear = (float)TimeSpan.FromMinutes(3.0).TotalSeconds;
		}

		public override void OnBeforeActivate()
		{
			base.OnBeforeActivate();
			if (MyInput.Static.IsJoystickLastUsed)
			{
				m_helplet = new GamepadVersion(this);
			}
			else
			{
				m_helplet = new MouseAndKeyboardVersion(this);
			}
		}

		public override void CleanUp()
		{
			if (m_helplet != null)
			{
				m_helplet.CleanUp();
			}
		}

		public override void OnActivated()
		{
			base.OnActivated();
			m_helplet.OnActivated();
		}
	}
}
