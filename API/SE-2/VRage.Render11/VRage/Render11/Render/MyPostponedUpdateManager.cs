using System.Collections.Generic;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Utils;
using VRageRender.Messages;

namespace VRage.Render11.Render
{
	internal class MyPostponedUpdateManager : IManager
	{
		private static readonly Stack<MyRenderObjectUpdateData> m_updateDataPool = new Stack<MyRenderObjectUpdateData>();

		private readonly Dictionary<uint, MyRenderObjectUpdateData> m_updateData = new Dictionary<uint, MyRenderObjectUpdateData>();

		private static void UpdateActor(uint id, MyRenderObjectUpdateData data)
		{
			MyIDTracker<MyActor>.FindByID(id)?.SetTransforms(data);
			data.Clean();
			m_updateDataPool.Push(data);
		}

		public void SavePostponedUpdate(MyRenderMessageUpdateRenderObject rMessage)
		{
			uint iD = rMessage.ID;
			if (!m_updateData.TryGetValue(iD, out var value))
			{
<<<<<<< HEAD
				value = ((m_updateDataPool.Count <= 0) ? new MyRenderObjectUpdateData() : m_updateDataPool.Pop());
=======
				value = ((m_updateDataPool.get_Count() <= 0) ? new MyRenderObjectUpdateData() : m_updateDataPool.Pop());
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				value.Clean();
			}
			MyUtils.Swap(ref rMessage.Data, ref value);
			if (value.HasWorldMatrix)
			{
				value.HasLocalMatrix = false;
			}
			else if (value.HasLocalMatrix)
			{
				value.HasWorldMatrix = false;
			}
			if (!value.HasLocalAABB && rMessage.Data.HasLocalAABB)
			{
				value.LocalAABB = rMessage.Data.LocalAABB;
			}
			m_updateData[iD] = value;
		}

		public void ApplyPostponedUpdate(uint actorID)
		{
			if (m_updateData.TryGetValue(actorID, out var value))
			{
				UpdateActor(actorID, value);
				m_updateData.Remove(actorID);
			}
		}

		public void Apply()
		{
			foreach (KeyValuePair<uint, MyRenderObjectUpdateData> updateDatum in m_updateData)
			{
				UpdateActor(updateDatum.Key, updateDatum.Value);
			}
			m_updateData.Clear();
		}
	}
}
