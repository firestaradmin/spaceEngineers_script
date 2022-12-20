using System.Reflection;
using System.Threading;

namespace VRage.Platform.Windows.Sys
{
	public class MySingleProgramInstance
	{
		private readonly Mutex m_mutex;

		private bool m_weOwn;

		public bool IsSingleInstance => m_weOwn;

		public MySingleProgramInstance()
		{
			m_mutex = new Mutex(initiallyOwned: true, Assembly.GetExecutingAssembly().GetName().Name, out m_weOwn);
		}

		public MySingleProgramInstance(string identifier)
		{
			m_mutex = new Mutex(initiallyOwned: true, identifier, out m_weOwn);
		}

		public void Close()
		{
			if (m_weOwn)
			{
				m_mutex.ReleaseMutex();
				m_mutex.Close();
				m_weOwn = false;
			}
		}
	}
}
