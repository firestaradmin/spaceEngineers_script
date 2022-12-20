using System.Collections.Generic;
using Sandbox.Engine.Networking;
using Sandbox.Game;
using Sandbox.Graphics.GUI;
using VRageMath;

namespace SpaceEngineers.Game.GUI
{
	public class MyBadgeHelper
	{
		private enum MyBannerStatus
		{
			Offline,
			Installed,
			NotInstalled
		}

		private class MyBadge
		{
			public MyBannerStatus Status;

			public uint DLCId;

			public string AchievementName;

			public string Texture;
		}

		/// <summary>
		/// Collection of the round badges under the main logo.
		/// </summary>
		private List<MyBadge> m_badges;

		private void InitializeBadges()
		{
			m_badges = new List<MyBadge>
			{
				new MyBadge
				{
					Status = MyBannerStatus.Offline,
					Texture = "Textures\\GUI\\PromotedEngineer.dds",
					DLCId = 0u,
					AchievementName = "Promoted_engineer"
				}
			};
			foreach (KeyValuePair<uint, MyDLCs.MyDLC> dLC in MyDLCs.DLCs)
			{
				m_badges.Add(new MyBadge
				{
					Status = MyBannerStatus.Offline,
					Texture = dLC.Value.Badge,
					DLCId = dLC.Value.AppId,
					AchievementName = ""
				});
			}
		}

		public void DrawGameLogo(float transitionAlpha, Vector2 position)
		{
			if (m_badges == null)
			{
				InitializeBadges();
			}
			MyGuiSandbox.DrawGameLogo(transitionAlpha, position);
			position.X += 0.005f;
			position.Y += 0.19f;
			Vector2 vector = position;
			Vector2 size = new Vector2(114f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			int num = 0;
			foreach (MyBadge badge in m_badges)
			{
				if (badge.Status == MyBannerStatus.Installed)
				{
					MyGuiSandbox.DrawBadge(badge.Texture, transitionAlpha, position, size);
					position.X += size.X;
					num++;
					if (num >= 6)
					{
						vector.Y += size.Y;
						position = vector;
						num = 0;
					}
				}
			}
		}

		public void RefreshGameLogo()
		{
			if (m_badges == null)
			{
				InitializeBadges();
			}
			foreach (MyBadge badge in m_badges)
			{
				if (MyGameService.IsActive)
				{
					if (badge.DLCId != 0 && MyGameService.IsDlcInstalled(badge.DLCId))
					{
						badge.Status = MyBannerStatus.Installed;
					}
					else if (!string.IsNullOrEmpty(badge.AchievementName) && MyGameService.GetAchievement(badge.AchievementName, null, 0f).IsUnlocked)
					{
						badge.Status = MyBannerStatus.Installed;
					}
					else
					{
						badge.Status = MyBannerStatus.NotInstalled;
					}
				}
				else
				{
					badge.Status = MyBannerStatus.Offline;
				}
			}
		}
	}
}
