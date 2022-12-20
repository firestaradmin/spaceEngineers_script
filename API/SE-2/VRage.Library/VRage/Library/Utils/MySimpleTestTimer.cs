using System;
using System.Diagnostics;
using System.IO;

namespace VRage.Library.Utils
{
	internal struct MySimpleTestTimer : IDisposable
	{
		private string m_name;

		private Stopwatch m_watch;

		public MySimpleTestTimer(string name)
		{
<<<<<<< HEAD
=======
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_name = name;
			m_watch = new Stopwatch();
			m_watch.Start();
		}

		public void Dispose()
		{
<<<<<<< HEAD
			File.AppendAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "perf.log"), $"{m_name}: {m_watch.ElapsedMilliseconds:N}ms\n");
=======
			File.AppendAllText(Path.Combine(Environment.GetFolderPath((SpecialFolder)16), "perf.log"), $"{m_name}: {m_watch.get_ElapsedMilliseconds():N}ms\n");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
