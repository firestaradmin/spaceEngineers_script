using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using VRage.Collections;

namespace VRageRender.Messages
{
	public class MyRenderMessageUpdateComponent : MyRenderMessageBase
	{
		public enum UpdateType
		{
			Update,
			Delete
		}

		private class DataFrame
		{
			private MyConcurrentPool<UpdateData> m_messagePool;

			public DataFrame(Type type)
			{
				m_messagePool = new MyConcurrentPool<UpdateData>(5, null, 10000, ExpressionExtension.CreateActivator<UpdateData>(type));
			}

			public UpdateData Allocate()
			{
				return m_messagePool.Get();
			}

			public void Free(UpdateData instance)
			{
				m_messagePool.Return(instance);
			}
		}

		private static ConcurrentDictionary<Type, DataFrame> m_dataPool = new ConcurrentDictionary<Type, DataFrame>();

		public uint ID;

		public UpdateType Type;

		public UpdateData Data;

		public override MyRenderMessageType MessageClass => MyRenderMessageType.StateChangeOnce;

		public override MyRenderMessageEnum MessageType => MyRenderMessageEnum.UpdateRenderComponent;

		public T Initialize<T>() where T : UpdateData
		{
			Type typeFromHandle = typeof(T);
<<<<<<< HEAD
			if (!m_dataPool.TryGetValue(typeFromHandle, out var value))
=======
			DataFrame orAdd = default(DataFrame);
			if (!m_dataPool.TryGetValue(typeFromHandle, ref orAdd))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				orAdd = new DataFrame(typeFromHandle);
				orAdd = m_dataPool.GetOrAdd(typeFromHandle, orAdd);
			}
			Data = orAdd.Allocate();
			return Data.As<T>();
		}

		public override void Close()
		{
			base.Close();
			if (Data is VolatileComponentData)
			{
				Data.ComponentType = null;
			}
			m_dataPool.get_Item(Data.DataType).Free(Data);
			Data = null;
		}
	}
}
