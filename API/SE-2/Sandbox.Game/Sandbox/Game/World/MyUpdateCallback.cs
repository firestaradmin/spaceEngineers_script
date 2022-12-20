using System;

namespace Sandbox.Game.World
{
	public class MyUpdateCallback
	{
<<<<<<< HEAD
		/// <summary>
		/// If function returns true, then callback can be removed
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private Func<bool> m_action;

		public bool ToBeRemoved { get; private set; }

		public MyUpdateCallback(Func<bool> action)
		{
			ToBeRemoved = false;
			m_action = action;
		}

		public void Update()
		{
			if (m_action())
			{
				ToBeRemoved = true;
			}
		}
	}
}
