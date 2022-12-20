using Sandbox.Engine.Networking;
using Sandbox.Graphics.GUI;
using VRage;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	public class MySCloudStorageQuotaBar : MyGuiControlInfoProgressBar
	{
		private int m_ticksTillNextUpdate;

		private bool m_innerControlsVisible;

		public MySCloudStorageQuotaBar(Vector2 position)
			: base(0.1758f, position, null, MyTexts.GetString(MyCommonTexts.CloudQuota_CloudUsage))
		{
			foreach (MyGuiControlBase control in base.Controls)
			{
				control.Visible = false;
			}
		}

		public override void Update()
		{
			base.Update();
			m_ticksTillNextUpdate--;
			if (m_ticksTillNextUpdate >= 0)
			{
				return;
			}
			m_ticksTillNextUpdate = 100;
			if (!MyGameService.GetRemoteStorageQuota(out var totalBytes, out var availableBytes))
			{
				return;
			}
			if ((double)availableBytes < (double)totalBytes * 0.1)
			{
				Vector4 vector3 = (m_LeftLabel.ColorMask = (m_RightLabel.ColorMask = Color.Red));
				m_BarInnerLine.ColorMask = Color.Red;
			}
			else
			{
				Vector4 vector3 = (m_LeftLabel.ColorMask = (m_RightLabel.ColorMask = Color.White));
				m_BarInnerLine.ColorMask = Color.White;
			}
			float num = (float)totalBytes / 1048576f;
			float num2 = (float)availableBytes / 1048576f;
			if (!m_innerControlsVisible)
			{
				m_innerControlsVisible = true;
				foreach (MyGuiControlBase control in base.Controls)
				{
					control.Visible = true;
				}
			}
			UpdateValues(num - num2, (long)(num + 0.5f), "{0:F1}/{1} MB");
		}

		private void UpdateValues(float current, long max, string format = null)
		{
			float textScale = 0.67f;
			m_LeftLabel.TextScale = textScale;
			m_RightLabel.TextScale = textScale;
			m_RightLabel.Text = string.Format(format ?? "{0} / {1}", current, max);
			float num = MathHelper.Clamp((float)((double)current / (double)max), 0f, 1f);
			m_BarInnerLine.Size = new Vector2(m_barSize.X * num, m_barSize.Y);
		}
	}
}
