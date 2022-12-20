using System;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiScreenPlayersWarfareTeamScoreTable : MyGuiControlBase
	{
		private float m_width;

		private float m_height;

		private float m_factionNameFontSize = 0.8f;

		private float m_uraniumCountFontSize = 1.8f;

		private string m_factionName;

		private string m_factionIcon;

		private bool m_drawOwnbackground;

		private bool m_drawBorders;

		private bool m_isLocalPlayersFaction;

		private Vector2 m_factionNameStringSize;

		private Vector2 m_iconSize = new Vector2(0.04f, 0.05f);

		private Vector2 m_topLeft;

		public long FactionId;

		public int ScorePoints;

		public string EscapePodString;

		public float ObjectiveFinishedPercentage;

		private Vector2 IconTopLeftPosition => m_topLeft + new Vector2(0.008f, 0.008f);

		private Vector2 FactionNameLeftAllignedPosition => IconTopLeftPosition + new Vector2(m_iconSize.X + 0.03f, m_iconSize.Y / 2f - MyGuiManager.MeasureString("White", m_factionName, 1f).Y / 2f - 0.007f);

		private Vector2 UraniumCounterRightAllignedPosition => FactionNameLeftAllignedPosition + new Vector2(m_factionNameStringSize.X, m_factionNameStringSize.Y + 0.005f);

		private Vector2 EscapePodEscapePodLeftAllignedPosition => IconTopLeftPosition + new Vector2(0f, m_iconSize.Y + 0.015f);

		private Vector2 EscapePodEscapePodPercentageRightAllignedPosition => new Vector2(UraniumCounterRightAllignedPosition.X, EscapePodEscapePodLeftAllignedPosition.Y);

		public MyGuiScreenPlayersWarfareTeamScoreTable(Vector2 topLeft, float width, float height, string factionName, string factionIcon, string escapePodString, long factionId, bool drawOwnBackground = false, bool drawBorders = true, bool isLocalPlayersFaction = false)
		{
			m_topLeft = topLeft;
			m_width = width;
			m_factionName = factionName;
			m_height = height;
			m_factionNameStringSize = MyGuiManager.MeasureString("White", factionName, m_factionNameFontSize);
			m_factionIcon = factionIcon;
			EscapePodString = escapePodString;
			FactionId = factionId;
			m_drawOwnbackground = drawOwnBackground;
			m_drawBorders = drawBorders;
			m_isLocalPlayersFaction = isLocalPlayersFaction;
		}

		public void SetThisAsLocalsPlayerFactionTable(bool isLocal = false)
		{
			m_isLocalPlayersFaction = isLocal;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			Color color = new Color(1f, 1f, 1f, transitionAlpha);
			if (m_factionName.StartsWith("Red"))
			{
				color = Color.Red;
			}
			if (m_factionName.StartsWith("Green"))
			{
				color = Color.Green;
			}
			if (m_factionName.StartsWith("Blue"))
			{
				color = Color.Blue;
			}
			if (m_drawOwnbackground)
			{
				MyGuiManager.DrawRectangle(m_topLeft, new Vector2(m_width, m_height), MyGuiConstants.ACTIVE_BACKGROUND_COLOR * 0.6f * transitionAlpha);
			}
			if (m_drawBorders)
			{
				MyGuiManager.DrawBorders(m_topLeft, new Vector2(m_width, m_height), Color.White * transitionAlpha, 1);
			}
			MyGuiManager.DrawRectangle(IconTopLeftPosition, m_iconSize, Color.Black * backgroundTransitionAlpha);
			MyGuiManager.DrawSpriteBatch(m_factionIcon, IconTopLeftPosition, m_iconSize, color * transitionAlpha, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP, useFullClientArea: false, waitTillLoaded: false);
			MyGuiManager.DrawString(m_isLocalPlayersFaction ? "Green" : "White", m_factionName, FactionNameLeftAllignedPosition, m_factionNameFontSize, Color.White * transitionAlpha, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			MyGuiManager.DrawString("White", ScorePoints.ToString(), UraniumCounterRightAllignedPosition, m_uraniumCountFontSize, Color.White * transitionAlpha, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
			MyGuiManager.DrawString("White", EscapePodString + ": ", EscapePodEscapePodLeftAllignedPosition, m_factionNameFontSize, Color.White * transitionAlpha, MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER);
			Color color2 = new Color(255, 70, 61);
			if (ObjectiveFinishedPercentage >= 100f)
			{
				color2 = new Color(106, 248, 0);
			}
			MyGuiManager.DrawString("White", Math.Round((double)ObjectiveFinishedPercentage * 10.0, 2) + "%", EscapePodEscapePodPercentageRightAllignedPosition, m_factionNameFontSize, color2 * transitionAlpha, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER);
		}
	}
}
