using System;
using System.Collections.Generic;
using System.Linq;
using ParallelTasks;
using VRage.FileSystem;
using VRage.Profiler;
using VRage.Render11.Common;
using VRage.Utils;
using VRageRender;

namespace VRage.Render11.GeometryStage2.Model
{
	internal class MyModelFactory : IManager, IManagerUnloadData, IManagerDevice
	{
		private class ModelData : WorkData
		{
			public string FilePath;

			public string FullFilePath;

			public MyModel Model;

			public bool PreloadTextures;
		}

		private static readonly bool ENABLE_MODEL_ASYNC_LOADING = true;

		private static readonly bool ENABLE_SKIP_LODS_ON_LOAD = true;

		private readonly Dictionary<string, ModelData> m_models = new Dictionary<string, ModelData>();

		private readonly List<ModelData> m_completedTasks = new List<ModelData>();

		private MyModel m_dummyModel;

		private bool m_skipOutstandingTasks;

		public int MinLoadingLod { get; private set; }

		public bool IsDummyModel(MyModel model)
		{
			return model == m_dummyModel;
		}

		public MyModel GetOrCreateModels(string filepath, bool preloadTextures = false)
		{
			m_skipOutstandingTasks = false;
			string fullMwmFilepath = MyMwmUtils.GetFullMwmFilepath(filepath);
			if (m_models.TryGetValue(fullMwmFilepath, out var value))
			{
				return value.Model;
			}
			ModelData modelData = new ModelData
			{
				FilePath = filepath,
				FullFilePath = fullMwmFilepath,
				PreloadTextures = preloadTextures,
				Model = new MyModel(),
				Priority = WorkPriority.VeryLow
			};
			if (ENABLE_MODEL_ASYNC_LOADING)
			{
				ModelData modelData2 = new ModelData
				{
					FilePath = filepath,
					FullFilePath = fullMwmFilepath,
					PreloadTextures = false,
					Model = m_dummyModel
				};
				m_models.Add(fullMwmFilepath, modelData2);
				Parallel.Start(LoadInternalAsync, null, modelData, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.AssetLoad, "MyModelFactory"), WorkPriority.VeryLow);
				return modelData2.Model;
			}
			LoadInternal(fullMwmFilepath, modelData.Model);
			m_models.Add(fullMwmFilepath, modelData);
			return modelData.Model;
		}

		internal void AddModels(List<string> models, bool preloadTextures = false)
		{
			foreach (string model in models)
			{
				GetOrCreateModels(model, preloadTextures);
			}
		}

		public void CompleteAsyncTasks()
		{
			lock (m_completedTasks)
			{
				foreach (ModelData completedTask in m_completedTasks)
				{
					MyModel model = m_models[completedTask.FullFilePath].Model;
					MyModel model2 = completedTask.Model;
					if (IsDummyModel(model))
					{
						FinalizeLoad(completedTask, ditherIn: true);
					}
					else
					{
						model2.UnloadData();
					}
				}
				m_completedTasks.Clear();
			}
		}

		private void FinalizeLoad(ModelData modelData, bool ditherIn = false)
		{
			if (modelData.PreloadTextures)
			{
				modelData.Model.PreloadTextures();
			}
			m_models[modelData.FullFilePath] = modelData;
			MyManagers.Instances.OnReloadModels(modelData.FilePath, modelData.Model, ignoreAsserts: true, ditherIn);
			MyManagers.StaticGroups.OnReloadModels(modelData.FilePath, modelData.Model, ignoreAsserts: true, ditherIn);
		}

		private void LoadInternal(string filepath, MyModel model)
		{
			model.UnloadData();
			try
			{
				model.LoadFromFile(filepath, MinLoadingLod);
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex);
				MyLog.Default.WriteLine($"model.LoadState: {model.LoadState}");
				throw;
			}
		}

		private void LoadInternalAsync(WorkData workData)
		{
			ModelData modelData = workData as ModelData;
			LoadInternal(modelData.FullFilePath, modelData.Model);
			lock (m_completedTasks)
			{
				if (m_skipOutstandingTasks)
				{
					modelData.Model.UnloadData();
				}
				else
				{
					m_completedTasks.Add(modelData);
				}
			}
		}

		public void ReloadModels()
		{
<<<<<<< HEAD
			foreach (KeyValuePair<string, ModelData> item in m_models.ToList())
=======
			foreach (KeyValuePair<string, ModelData> item in Enumerable.ToList<KeyValuePair<string, ModelData>>((IEnumerable<KeyValuePair<string, ModelData>>)m_models))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				string key = item.Key;
				ModelData value = item.Value;
				bool flag = IsDummyModel(value.Model);
				if (flag)
				{
					value.Model = new MyModel();
				}
				LoadInternal(key, value.Model);
				FinalizeLoad(value, flag);
			}
		}

		void IManagerUnloadData.OnUnloadData()
		{
			lock (m_completedTasks)
			{
				m_skipOutstandingTasks = true;
				foreach (ModelData completedTask in m_completedTasks)
				{
					m_models[completedTask.FullFilePath] = completedTask;
				}
				m_completedTasks.Clear();
			}
<<<<<<< HEAD
			string[] array = m_models.Keys.ToArray();
=======
			string[] array = Enumerable.ToArray<string>((IEnumerable<string>)m_models.Keys);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (string text in array)
			{
				if (!MyFileSystem.IsGameContent(text))
				{
					m_models[text].Model.UnloadData();
					m_models.Remove(text);
				}
			}
		}

		public void OnDeviceInit()
		{
			MinLoadingLod = (ENABLE_SKIP_LODS_ON_LOAD ? MyCommon.LoddingSettings.CommonMinLod() : 0);
			m_dummyModel = new MyModel();
			LoadInternal("Models\\Debug\\LoadingQuad.mwm", m_dummyModel);
		}

		public void OnDeviceReset()
		{
		}

		public void OnDeviceEnd()
		{
			if (m_dummyModel != null)
			{
				m_dummyModel.UnloadData();
				m_dummyModel = null;
			}
			foreach (KeyValuePair<string, ModelData> model in m_models)
			{
				model.Value.Model.UnloadData();
			}
			m_models.Clear();
		}

		public void OnLoddingSettingChanged()
		{
			if (MinLoadingLod != MyCommon.LoddingSettings.CommonMinLod() && ENABLE_SKIP_LODS_ON_LOAD)
			{
				MinLoadingLod = MyCommon.LoddingSettings.CommonMinLod();
				ReloadModels();
			}
		}
	}
}
