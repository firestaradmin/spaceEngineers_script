using System;
using System.Collections.Concurrent;
using VRage.FileSystem;
using VRage.Utils;

namespace VRage.Game.Models
{
	public static class MyModels
	{
		private static readonly ConcurrentDictionary<string, MyModel> m_models = new ConcurrentDictionary<string, MyModel>();

		public static void UnloadData()
		{
			UnloadModelData();
			m_models.Clear();
		}

		public static void UnloadModdedModels()
		{
			UnloadModelData((MyModel models) => !MyFileSystem.IsGameContent(models.AssetName));
		}

		private static void UnloadModelData(Func<MyModel, bool> condition = null)
		{
<<<<<<< HEAD
			foreach (MyModel value in m_models.Values)
=======
			foreach (MyModel value in m_models.get_Values())
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (condition == null || condition(value))
				{
					value.UnloadData();
				}
			}
		}

		public static MyModel GetModelOnlyData(string modelAsset)
		{
			if (string.IsNullOrEmpty(modelAsset))
			{
				return null;
			}
			MyModel orAdd = GetOrAdd(modelAsset);
			if (!orAdd.LoadedData)
			{
				lock (orAdd)
				{
					if (orAdd.LoadedData)
					{
						return orAdd;
					}
					orAdd.LoadData();
					return orAdd;
				}
			}
			return orAdd;
		}

		public static MyModel GetModelOnlyAnimationData(string modelAsset, bool forceReloadMwm = false)
		{
			MyModel orAdd = GetOrAdd(modelAsset);
			try
			{
				if (!orAdd.LoadedData || forceReloadMwm)
				{
					lock (orAdd)
					{
						if (!orAdd.LoadedData)
						{
							orAdd.LoadAnimationData(forceReloadMwm);
						}
					}
				}
				return orAdd;
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
				return null;
			}
		}

		public static MyModel GetModelOnlyDummies(string modelAsset)
		{
			MyModel orAdd = GetOrAdd(modelAsset);
			orAdd.LoadOnlyDummies();
			return orAdd;
		}

		public static MyModel GetModelOnlyModelInfo(string modelAsset)
		{
			MyModel orAdd = GetOrAdd(modelAsset);
			orAdd.LoadOnlyModelInfo();
			return orAdd;
		}

		public static MyModel GetModel(string modelAsset)
		{
			if (modelAsset == null)
			{
				return null;
			}
<<<<<<< HEAD
			m_models.TryGetValue(modelAsset, out var value);
			return value;
=======
			MyModel result = default(MyModel);
			m_models.TryGetValue(modelAsset, ref result);
			return result;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static MyModel GetOrAdd(string modelAsset)
		{
<<<<<<< HEAD
			return m_models.GetOrAdd(modelAsset, (string m) => new MyModel(m));
=======
			return m_models.GetOrAdd(modelAsset, (Func<string, MyModel>)((string m) => new MyModel(m)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
