using System;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MyGuiControlPcuBar : MyGuiControlInfoProgressBar
	{
		private int m_maxPCU;

		private int m_currentPCU;

		private int m_currentDisplayedPCU = -1;

<<<<<<< HEAD
=======
		private int m_frameCounterPCU;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public MyGuiControlPcuBar(Vector2? position = null, float? width = null)
			: base(width ?? 0.32f, position, "Textures\\GUI\\PCU.png", "PCU:")
		{
		}

		/// <summary>
		/// Check PCU bar and animate it if required. Called while G-Screen is open.
		/// </summary>
		public void UpdatePCU(MyIdentity identity, bool performAnimation)
		{
			m_maxPCU = 0;
			m_currentPCU = 0;
			if (identity != null)
			{
				m_maxPCU = identity.GetMaxPCU();
				m_currentPCU = identity.BlockLimits.PCU;
			}
			if (MySession.Static.BlockLimitsEnabled == MyBlockLimitsEnabledEnum.NONE || MySession.Static.TotalPCU == 0)
			{
				m_currentDisplayedPCU = m_currentPCU;
				m_RightLabel.TextEnum = MyCommonTexts.Unlimited;
			}
			else if (m_currentDisplayedPCU != m_currentPCU)
			{
				if (performAnimation)
				{
					int num = Math.Max(1, Math.Abs((m_currentPCU - m_currentDisplayedPCU) / 20));
					m_currentDisplayedPCU = ((m_currentPCU < m_currentDisplayedPCU) ? (m_currentDisplayedPCU - num) : (m_currentDisplayedPCU + num));
				}
				else
				{
					m_currentDisplayedPCU = m_currentPCU;
				}
				m_RightLabel.Text = $"{m_currentPCU} / {m_maxPCU}";
			}
			m_BarInnerLine.Size = new Vector2((m_maxPCU != 0) ? Math.Min(m_barSize.X / (float)m_maxPCU * (float)m_currentDisplayedPCU, m_barSize.X) : 0f, m_barSize.Y);
		}
	}
}
