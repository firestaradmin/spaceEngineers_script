using System.Collections.Generic;
using System.Text;
using VRage.Library.Utils;
using VRage.Render11.Sprites;
using VRage.Stats;
using VRageMath;
using VRageRender.Utils;

namespace VRageRender
{
	/// <summary>
	/// Draws statistics
	/// </summary>
	public static class MyRenderStatsDraw
	{
		private static float m_rightGapSizeRatio = 0.1f;

		private static float m_rightColumnWidth;

		private static MyTimeSpan m_rightColumnChangeTime = MyRender11.CurrentDrawTime;

		private static StringBuilder m_tmpDrawText = new StringBuilder(4096);

		public static void Draw(Dictionary<MyRenderStats.ColumnEnum, List<MyStats>> m_stats, float scale, Color color)
		{
			if (MyRenderProxy.DrawRenderStats == MyRenderProxy.MyStatsState.SimpleTimingStats)
			{
				try
				{
					m_stats.FirstPair().Value[0].WriteTo(m_tmpDrawText);
					m_tmpDrawText.AppendLine();
					MyDebugTextHelpers.DrawText(new Vector2(10f, 10f), m_tmpDrawText, color, scale);
				}
				finally
				{
					m_tmpDrawText.Clear();
				}
				return;
			}
			foreach (KeyValuePair<MyRenderStats.ColumnEnum, List<MyStats>> m_stat in m_stats)
			{
				try
				{
					foreach (MyStats item in m_stat.Value)
					{
						item.WriteTo(m_tmpDrawText);
						m_tmpDrawText.AppendLine();
					}
					Vector2 screenCoord = new Vector2(10f, 10f);
					if (m_stat.Key == MyRenderStats.ColumnEnum.Right)
					{
						Vector2 vector = MyRender11.GetDebugFont().MeasureString(m_tmpDrawText, scale);
						if (m_rightColumnWidth < vector.X)
						{
							m_rightColumnWidth = vector.X * m_rightGapSizeRatio;
							m_rightColumnChangeTime = MyRender11.CurrentDrawTime;
						}
						else if (m_rightColumnWidth > vector.X * m_rightGapSizeRatio && (MyRender11.CurrentDrawTime - m_rightColumnChangeTime).Seconds > 3.0)
						{
							m_rightColumnWidth = vector.X * m_rightGapSizeRatio;
							m_rightColumnChangeTime = MyRender11.CurrentDrawTime;
						}
						screenCoord = new Vector2((float)MyRender11.ViewportResolution.X - m_rightColumnWidth, 0f);
					}
					MyDebugTextHelpers.DrawText(screenCoord, m_tmpDrawText, color, scale);
				}
				finally
				{
					m_tmpDrawText.Clear();
				}
			}
		}
	}
}
