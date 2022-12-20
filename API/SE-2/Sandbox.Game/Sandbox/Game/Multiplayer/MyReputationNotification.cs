using Sandbox.Game.Gui;

namespace Sandbox.Game.Multiplayer
{
	internal class MyReputationNotification
	{
		private string m_notificationTag = string.Empty;

		private int m_notificationValue;

		private MyHudNotification m_notification;

		internal MyReputationNotification(MyHudNotification notification)
		{
			m_notification = notification;
		}

		internal void UpdateReputationNotification(in string newTag, in int valueChange)
		{
			if (!m_notification.Alive)
			{
				m_notificationTag = newTag;
				m_notificationValue = valueChange;
				m_notification.SetTextFormatArguments(m_notificationTag, m_notificationValue);
				MyHud.Notifications.Add(m_notification);
				return;
			}
			if (m_notificationTag == newTag)
			{
				m_notificationValue += valueChange;
				m_notification.ResetAliveTime();
			}
			else
			{
				m_notificationTag = newTag;
				m_notificationValue = valueChange;
			}
			m_notification.SetTextFormatArguments(m_notificationTag, m_notificationValue);
			MyHud.Notifications.Update(m_notification);
		}
	}
}
