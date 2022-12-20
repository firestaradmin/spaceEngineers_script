using System.Threading;
using VRage.Network;

namespace VRageRender.Messages
{
	[GenerateActivator]
	public abstract class MyRenderMessageBase
	{
		private int m_ref;

		/// <summary>
		/// Get message class
		/// </summary>
		public abstract MyRenderMessageType MessageClass { get; }

		/// <summary>
		/// Gets message type
		/// </summary>
		public abstract MyRenderMessageEnum MessageType { get; }

		public virtual bool IsPersistent => false;

		public virtual void Close()
		{
			m_ref = 0;
		}

		public virtual void Init()
		{
		}

		public void AddRef()
		{
			Interlocked.Increment(ref m_ref);
		}

		public virtual void Dispose()
		{
			if (Interlocked.Decrement(ref m_ref) == -1)
			{
				MyRenderProxy.MessagePool.Return(this);
			}
		}
	}
}
