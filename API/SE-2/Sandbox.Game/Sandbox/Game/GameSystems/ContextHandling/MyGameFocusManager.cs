namespace Sandbox.Game.GameSystems.ContextHandling
{
	/// <summary>
	/// Manages game objects that can be deactivated.
	/// </summary>
	public class MyGameFocusManager
	{
		/// <summary>
		/// Current focus holder.
		/// </summary>
		private IMyFocusHolder m_currentFocusHolder;

		/// <summary>
		/// Registers holder as current one and informs the old one that it is defocused.
		/// </summary>
		/// <param name="newFocusHolder"></param>
		public void Register(IMyFocusHolder newFocusHolder)
		{
			if (m_currentFocusHolder != null && newFocusHolder != m_currentFocusHolder)
			{
				m_currentFocusHolder.OnLostFocus();
			}
			m_currentFocusHolder = newFocusHolder;
		}

		/// <summary>
		/// Unregisters focus holder.
		/// </summary>
		/// <param name="focusHolder">Focus holder to unregister.</param>
		public void Unregister(IMyFocusHolder focusHolder)
		{
			if (m_currentFocusHolder == focusHolder)
			{
				m_currentFocusHolder = null;
			}
		}

		/// <summary>
		/// Informs current focus holder and than clears focus.
		/// </summary>
		public void Clear()
		{
			if (m_currentFocusHolder != null)
			{
				m_currentFocusHolder.OnLostFocus();
			}
			m_currentFocusHolder = null;
		}
	}
}
