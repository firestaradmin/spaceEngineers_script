using System;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Linq;
using VRage.Render11.Resources.Textures;
using VRage.Render11.Scene.Resources;
using VRageRender.Messages;

namespace VRage.Render11.GeometryStage2.Model
{
	internal class MyModelInstance : IDisposable
	{
		public readonly MyModel Model;

		public readonly Dictionary<string, MyTextureChange> Changes;

		public int RefHashCode;

		public readonly int ContentHashCode;

		public MyLodInstance[] LodInstances;

		public IMySceneResource[] Resources;

		public MyModelInstance(MyModel model, Dictionary<string, MyTextureChange> changes, int contentHash)
		{
			Model = model;
			Changes = changes;
			if (Changes != null)
			{
				ContentHashCode = contentHash;
				RefHashCode = Changes.GetHashCode();
			}
			if (Model.IsValid)
			{
				Reload();
			}
		}

		public void Reload()
		{
			Dispose();
<<<<<<< HEAD
			HashSet<IMySceneResource> hashSet = new HashSet<IMySceneResource>();
=======
			HashSet<IMySceneResource> val = new HashSet<IMySceneResource>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			int lodsCount = Model.GetLodStrategyInfo().GetLodsCount();
			LodInstances = new MyLodInstance[lodsCount];
			for (int i = 0; i < lodsCount; i++)
			{
<<<<<<< HEAD
				LodInstances[i] = new MyLodInstance(Model.GetLod(i), Changes, hashSet);
			}
			Resources = hashSet.ToArray();
			Action<IMyStreamedTexture> value = OnTextureHandleChanged;
			foreach (IMyStreamedTexture item in Resources.OfType<IMyStreamedTexture>())
=======
				LodInstances[i] = new MyLodInstance(Model.GetLod(i), Changes, val);
			}
			Resources = Enumerable.ToArray<IMySceneResource>((IEnumerable<IMySceneResource>)val);
			Action<IMyStreamedTexture> value = OnTextureHandleChanged;
			foreach (IMyStreamedTexture item in Enumerable.OfType<IMyStreamedTexture>((IEnumerable)Resources))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				item.OnTextureHandleChanged += value;
			}
		}

		private void OnTextureHandleChanged(IMyStreamedTexture texture)
		{
			MyLodInstance[] lodInstances = LodInstances;
			for (int i = 0; i < lodInstances.Length; i++)
			{
				lodInstances[i].OnTextureHandleChanged(texture);
			}
		}

		public void Dispose()
		{
			if (Resources == null)
			{
				return;
			}
			Action<IMyStreamedTexture> value = OnTextureHandleChanged;
<<<<<<< HEAD
			foreach (IMyStreamedTexture item in Resources.OfType<IMyStreamedTexture>())
=======
			foreach (IMyStreamedTexture item in Enumerable.OfType<IMyStreamedTexture>((IEnumerable)Resources))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				item.OnTextureHandleChanged -= value;
			}
			Resources = null;
			LodInstances = null;
		}
	}
}
