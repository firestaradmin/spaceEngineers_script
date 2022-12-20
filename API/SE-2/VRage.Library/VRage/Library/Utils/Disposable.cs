using System;

namespace VRage.Library.Utils
{
	public class Disposable : IDisposable
	{
		public Disposable(bool collectStack = false)
		{
		}

		public virtual void Dispose()
		{
			GC.SuppressFinalize(this);
		}

		~Disposable()
		{
			string text = "Dispose not called!";
			string text2 = $"Dispose was not called for '{GetType().FullName}'";
		}
	}
}
