using System.Collections.Generic;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRage.Render11.Scene;
using VRageRender.Messages;

namespace VRage.Render11.Render
{
	public class MyTextureChangeManager : IManager, IManagerUpdate, IManagerUnloadData
	{
		private readonly Dictionary<uint, MyRenderMessageChangeMaterialTexture> m_textureChanges = new Dictionary<uint, MyRenderMessageChangeMaterialTexture>();

		private readonly HashSet<uint> m_notReadyIds = new HashSet<uint>();

		private readonly List<uint> m_toRemove = new List<uint>();

		public void RemovePendingChange(uint ID, string materialName)
		{
			if (m_textureChanges.TryGetValue(ID, out var value) && value.Changes.ContainsKey(materialName))
			{
				if (value.Changes.Count > 1)
				{
					value.Changes = new Dictionary<string, MyTextureChange>(value.Changes);
					value.Changes.Remove(materialName);
				}
				else
				{
					m_textureChanges.Remove(ID);
				}
			}
		}

		public void ChangeMaterialTexture(MyRenderMessageChangeMaterialTexture rMessage)
		{
			uint renderObjectID = rMessage.RenderObjectID;
			if (rMessage.Changes == null)
			{
				m_textureChanges.Remove(renderObjectID);
				MyIDTracker<MyActor>.FindByID(rMessage.RenderObjectID)?.GetRenderable()?.ClearTextureChanges();
				return;
			}
			MyTextureStreamingManager textures = MyManagers.Textures;
			foreach (KeyValuePair<string, MyTextureChange> change in rMessage.Changes)
			{
				textures.GetTexture(change.Value.ColorMetalFileName, MyFileTextureEnum.COLOR_METAL).Touch(100);
				textures.GetTexture(change.Value.NormalGlossFileName, MyFileTextureEnum.NORMALMAP_GLOSS).Touch(100);
				textures.GetTexture(change.Value.ExtensionsFileName, MyFileTextureEnum.EXTENSIONS).Touch(100);
				textures.GetTexture(change.Value.AlphamaskFileName, MyFileTextureEnum.ALPHAMASK).Touch(100);
			}
			if (m_textureChanges.TryGetValue(renderObjectID, out var value))
			{
				Dictionary<string, MyTextureChange> dictionary = new Dictionary<string, MyTextureChange>(value.Changes);
				foreach (KeyValuePair<string, MyTextureChange> change2 in rMessage.Changes)
				{
					dictionary[change2.Key] = change2.Value;
				}
				value.Changes = dictionary;
			}
			else
			{
				MyRenderMessageChangeMaterialTexture value2 = new MyRenderMessageChangeMaterialTexture
				{
					RenderObjectID = rMessage.RenderObjectID,
					Changes = rMessage.Changes
				};
				m_textureChanges[renderObjectID] = value2;
			}
		}

		private bool CheckChangesAvailable(MyRenderMessageChangeMaterialTexture rMessage)
		{
			bool result = true;
			foreach (var (_, myTextureChange2) in rMessage.Changes)
			{
				if (!MyManagers.FileTextures.IsTextureReadyForMaterialSwap(myTextureChange2.ColorMetalFileName) || !MyManagers.FileTextures.IsTextureReadyForMaterialSwap(myTextureChange2.NormalGlossFileName) || !MyManagers.FileTextures.IsTextureReadyForMaterialSwap(myTextureChange2.ExtensionsFileName) || !MyManagers.FileTextures.IsTextureReadyForMaterialSwap(myTextureChange2.AlphamaskFileName))
				{
					result = false;
				}
			}
			return result;
		}

		private void ChangeMaterialTextureInternal(MyRenderMessageChangeMaterialTexture rMessage)
		{
			MyActor myActor = MyIDTracker<MyActor>.FindByID(rMessage.RenderObjectID);
			if (myActor != null)
			{
				myActor.GetInstance()?.AddTextureChanges(rMessage.Changes);
				myActor.GetRenderable()?.AddTextureChanges(rMessage.Changes);
			}
		}

		public void OnUpdate()
		{
			m_notReadyIds.Clear();
			foreach (MyRenderMessageChangeMaterialTexture value in m_textureChanges.Values)
			{
				if (!CheckChangesAvailable(value))
				{
					m_notReadyIds.Add(value.RenderObjectID);
				}
			}
			m_toRemove.Clear();
			foreach (KeyValuePair<uint, MyRenderMessageChangeMaterialTexture> textureChange in m_textureChanges)
			{
				if (!m_notReadyIds.Contains(textureChange.Value.RenderObjectID))
				{
					if (MyIDTracker<MyActor>.FindByID(textureChange.Value.RenderObjectID) != null)
					{
						ChangeMaterialTextureInternal(textureChange.Value);
					}
					m_toRemove.Add(textureChange.Key);
				}
			}
			foreach (uint item in m_toRemove)
			{
				m_textureChanges.Remove(item);
			}
		}

		public void OnUnloadData()
		{
			m_textureChanges.Clear();
		}
	}
}
