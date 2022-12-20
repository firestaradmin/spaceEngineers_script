using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VRage.FileSystem;
using VRage.Render11.Resources;

namespace VRageRender
{
	internal class MyAssetsLoader
	{
		internal const bool LOG_MESH_STATISTICS = false;

		private static Dictionary<string, MyAssetMesh> m_meshes = new Dictionary<string, MyAssetMesh>();

		internal static Dictionary<string, string> ModelRemap = new Dictionary<string, string>();

		internal static MyAssetMesh m_debugMesh;

		internal static float LoadedMeshSize = 0f;

		internal static MyAssetMesh GetModel(string assetName)
		{
			MyAssetMesh myAssetMesh = m_meshes.Get(assetName);
			if (myAssetMesh != null)
			{
				return myAssetMesh;
			}
			myAssetMesh = new MyAssetMesh(assetName);
			myAssetMesh.LoadAsset();
			m_meshes[assetName] = myAssetMesh;
			return myAssetMesh;
		}

		internal static MyAssetMesh GetDebugMesh()
		{
			return GetModel("Models\\Cubes\\large\\StoneSlope.mwm");
		}

		[Conditional("DEBUG")]
		private static void LogModel(MyAssetMesh mesh, string assetName)
		{
<<<<<<< HEAD
			string text = (Path.IsPathRooted(assetName) ? assetName : Path.Combine(MyFileSystem.ContentPath, assetName));
			FileInfo fileInfo = new FileInfo(text);
=======
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			string text = (Path.IsPathRooted(assetName) ? assetName : Path.Combine(MyFileSystem.ContentPath, assetName));
			FileInfo val = new FileInfo(text);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (MyPerformanceCounter.LogFiles)
			{
				MyPerformanceCounter.PerAppLifetime.LoadedModelFiles.Add(text);
			}
<<<<<<< HEAD
			if (!fileInfo.Exists)
			{
				return;
			}
			MyPerformanceCounter.PerAppLifetime.MyModelsFilesSize += (int)fileInfo.Length;
=======
			if (!((FileSystemInfo)val).get_Exists())
			{
				return;
			}
			MyPerformanceCounter.PerAppLifetime.MyModelsFilesSize += (int)val.get_Length();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyPerformanceCounter.PerAppLifetime.ModelsCount++;
			MyRenderLodInfo[] lODs = mesh.LODs;
			foreach (MyRenderLodInfo myRenderLodInfo in lODs)
			{
				IVertexBuffer[] vB = myRenderLodInfo.m_meshInfo.VB;
				foreach (IVertexBuffer vertexBuffer in vB)
				{
					MyPerformanceCounter.PerAppLifetime.ModelVertexBuffersSize += vertexBuffer.ByteSize;
					MyPerformanceCounter.PerAppLifetime.MyModelsVertexesCount += vertexBuffer.ElementCount;
				}
				MyPerformanceCounter.PerAppLifetime.ModelIndexBuffersSize += myRenderLodInfo.m_meshInfo.IB.ByteSize;
			}
		}

		internal static MyAssetMesh GetMaterials(string assetName)
		{
			MyAssetMesh myAssetMesh = m_meshes.Get(assetName);
			if (myAssetMesh != null)
			{
				return myAssetMesh;
			}
			MyAssetMesh.LoadMaterials(assetName);
			return myAssetMesh;
		}

		internal static void ReloadMeshes()
		{
			foreach (KeyValuePair<string, MyAssetMesh> mesh in m_meshes)
			{
				mesh.Value.LoadAsset();
			}
		}

		internal static void ClearMeshes()
		{
			foreach (KeyValuePair<string, MyAssetMesh> mesh in m_meshes)
			{
				mesh.Value.Release();
			}
			m_meshes.Clear();
		}
	}
}
