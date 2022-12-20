using VRage.Collections;
using VRageRender.Messages;

namespace VRageRender
{
	/// <summary>
	/// TODO: This should use some better sync, it could introduce delays with current state
	/// 1) Use spin lock
	/// 2) Lock only queue, not whole dictionary
	/// 3) Test count first and when it's insufficient, create new message, both should be safe to do out of any lock
	/// 4) Custom consumer/producer non-locking (except resize) queue could be better (maybe overkill)
	/// </summary>
	public class MyMessagePool
	{
		private MyConcurrentQueue<MyRenderMessageBase>[] m_messageQueues = new MyConcurrentQueue<MyRenderMessageBase>[145];

		public MyMessagePool()
		{
			for (int i = 0; i < m_messageQueues.Length; i++)
			{
				m_messageQueues[i] = new MyConcurrentQueue<MyRenderMessageBase>();
			}
		}

		public void Clear(MyRenderMessageEnum message)
		{
			m_messageQueues[(int)message].Clear();
		}

		public T Get<T>(MyRenderMessageEnum renderMessageEnum) where T : MyRenderMessageBase, new()
		{
			if (!m_messageQueues[(int)renderMessageEnum].TryDequeue(out var instance))
			{
				instance = new T();
			}
			instance.Init();
			return (T)instance;
		}

		public void Return(MyRenderMessageBase message)
		{
			if (!message.IsPersistent)
			{
				MyConcurrentQueue<MyRenderMessageBase> obj = m_messageQueues[(int)message.MessageType];
				message.Close();
				obj.Enqueue(message);
			}
		}
	}
}
