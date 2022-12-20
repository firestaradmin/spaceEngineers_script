using System;
using System.Collections.Generic;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game.ModAPI;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens
{
	internal class MyGuiScreenWarfareFactionScore : MyGuiScreenBase
	{
		private MyGuiControlLabel m_warfare_timeRemainting_time;

		private MySessionComponentMatch m_match;

<<<<<<< HEAD
=======
		private float m_yPos = -0.45f;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private float m_warfareHeight = 0.09f;

		private float m_warfareWidth = 0.18f;

		private int m_warfareUpdate_frameCount = 30;

		private int m_warfareUpdate_frameCount_current = 30;

		public MyGuiScreenWarfareFactionScore()
		{
			base.CloseButtonEnabled = false;
			base.CanHaveFocus = false;
			m_drawEvenWithoutFocus = true;
			base.CanHideOthers = false;
			m_match = MySession.Static.GetComponent<MySessionComponentMatch>();
			RecreateControls(constructor: false);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenWarfareFactionScore";
		}

		public override bool Update(bool hasFocus)
		{
			if (m_warfareUpdate_frameCount_current >= m_warfareUpdate_frameCount)
			{
				m_warfareUpdate_frameCount_current = 0;
				foreach (MyGuiControlBase control in Controls)
				{
					if (!(control.GetType() == typeof(MyGuiScreenPlayersWarfareTeamScoreTable)))
					{
						continue;
					}
					IMyFaction myFaction = MySession.Static.Factions.TryGetFactionById(((MyGuiScreenPlayersWarfareTeamScoreTable)control).FactionId);
					if (myFaction != null && myFaction.FactionId == ((MyGuiScreenPlayersWarfareTeamScoreTable)control).FactionId)
					{
						MyMultiplayer.RaiseStaticEvent((IMyEventOwner s) => MyFactionCollection.RequestFactionScoreAndPercentageUpdate, myFaction.FactionId, MyEventContext.Current.Sender);
					}
				}
				if (m_match != null && m_match.IsEnabled)
				{
					TimeSpan timeSpan = TimeSpan.FromMinutes(m_match.RemainingMinutes);
					string text = timeSpan.ToString((timeSpan.TotalHours >= 1.0) ? "hh\\:mm\\:ss" : "mm\\:ss");
					if (m_warfare_timeRemainting_time.Text != text)
					{
						m_warfare_timeRemainting_time.Text = text;
					}
				}
				foreach (MyGuiControlBase control2 in Controls)
				{
					if (control2 is MyGuiScreenPlayersWarfareTeamScoreTable)
					{
						MyGuiScreenPlayersWarfareTeamScoreTable myGuiScreenPlayersWarfareTeamScoreTable = control2 as MyGuiScreenPlayersWarfareTeamScoreTable;
						IMyFaction myFaction2 = MySession.Static.Factions.TryGetFactionById(myGuiScreenPlayersWarfareTeamScoreTable.FactionId);
						if (myFaction2 != null && myFaction2.FactionId == myGuiScreenPlayersWarfareTeamScoreTable.FactionId)
						{
							myGuiScreenPlayersWarfareTeamScoreTable.ScorePoints = myFaction2.Score;
							myGuiScreenPlayersWarfareTeamScoreTable.ObjectiveFinishedPercentage = myFaction2.ObjectivePercentageCompleted;
							myGuiScreenPlayersWarfareTeamScoreTable.SetThisAsLocalsPlayerFactionTable(myFaction2.IsMember(MySession.Static.LocalHumanPlayer.Identity.IdentityId));
						}
					}
				}
			}
			m_warfareUpdate_frameCount_current++;
			if (!MyHud.IsVisible || MyHud.HudState == 0 || MyGuiScreenHudSpace.Static == null || (MyGuiScreenHudSpace.Static != null && MyGuiScreenHudSpace.Static.State == MyGuiScreenState.HIDDEN) || MyGuiScreenHudSpace.Static.State == MyGuiScreenState.HIDING)
			{
				if (base.State != MyGuiScreenState.HIDDEN && base.State != MyGuiScreenState.HIDING)
				{
					base.State = MyGuiScreenState.HIDING;
				}
			}
			else if (base.State != 0 && base.State != MyGuiScreenState.OPENED)
			{
				base.State = MyGuiScreenState.OPENING;
			}
			return base.Update(hasFocus);
		}

		public override void RecreateControls(bool constructor)
		{
			base.State = MyGuiScreenState.HIDDEN;
			base.RecreateControls(constructor);
			MySessionComponentMatch component = MySession.Static.GetComponent<MySessionComponentMatch>();
			if (!component.IsEnabled)
			{
				return;
			}
			List<MyFaction> list = new List<MyFaction>();
			MyFaction[] allFactions = MySession.Static.Factions.GetAllFactions();
			foreach (MyFaction myFaction in allFactions)
			{
				if ((myFaction.Name.StartsWith("Red") || myFaction.Name.StartsWith("Green") || myFaction.Name.StartsWith("Blue")) && myFaction.Name.EndsWith("Faction"))
				{
					list.Add(myFaction);
				}
			}
			int num = 0;
			foreach (MyFaction item in list)
			{
				Controls.Add(new MyGuiScreenPlayersWarfareTeamScoreTable(new Vector2(0.5f - m_warfareWidth * 1.5f + (float)num * m_warfareWidth, m_warfareHeight / 2f), m_warfareWidth, m_warfareHeight, item.Name, item.FactionIcon.Value.String, MyTexts.GetString(MySpaceTexts.WarfareCounter_EscapePod), item.FactionId, drawOwnBackground: true));
				num++;
			}
			TimeSpan timeSpan = TimeSpan.FromMinutes(component.RemainingMinutes);
			m_warfare_timeRemainting_time = new MyGuiControlLabel();
			m_warfare_timeRemainting_time.DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_warfare_timeRemainting_time.Text = timeSpan.ToString((timeSpan.TotalHours >= 1.0) ? "hh\\:mm\\:ss" : "mm\\:ss");
			MyGuiControlLabel myGuiControlLabel = new MyGuiControlLabel(new Vector2(0f - m_warfare_timeRemainting_time.Size.Y, -0.47f));
			myGuiControlLabel.DrawAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP;
			myGuiControlLabel.Text = MyTexts.GetString(MySpaceTexts.WarfareCounter_TimeRemaining).ToString() + ": ";
			m_warfare_timeRemainting_time.Position = new Vector2(myGuiControlLabel.PositionX + myGuiControlLabel.Size.X / 2f, myGuiControlLabel.PositionY);
			Controls.Add(myGuiControlLabel);
			Controls.Add(m_warfare_timeRemainting_time);
		}
	}
}
