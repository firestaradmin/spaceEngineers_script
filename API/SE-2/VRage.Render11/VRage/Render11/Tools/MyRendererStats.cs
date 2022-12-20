using System;
using System.Collections.Generic;
using VRage.Collections;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRage.Render11.Scene;
using VRageRender;

namespace VRage.Render11.Tools
{
	internal class MyRendererStats
	{
		public struct MyCullStats
		{
			public int CullProxies;

			public int Instances;

			public int RootObjects;

			public int RootInstances;

			public int Groups;

			public int PointLights;

			public int SpotLights;

			public int Foliage;

			public int All()
			{
				return CullProxies + Instances;
			}

			public int AllRoot()
			{
				return RootObjects + RootInstances;
			}
		}

		internal struct MyRenderStats
		{
			internal int Draws;

			internal int Triangles;

			internal void Gather(ref MyRenderStats other)
			{
				Draws += other.Draws;
				Triangles += other.Triangles;
			}
		}

		private const string VISIBLE_OBJECTS_GROUP = "Visible proxies";

		private const string VISIBLE_GROUPS_GROUP = "Visible groups";

		private const string VISIBLE_ROOTOBJECTS_GROUP = "Visible root proxies";

		public static readonly MyCullStats[] ViewCullStats = new MyCullStats[19];

		private static readonly MyRenderStats[] m_viewRenderStats = new MyRenderStats[19];

		private static MyRenderStats m_billboardRenderStats;

		private static MyRenderStats m_totalRenderStats;

		private static readonly MyRenderContextStatistics m_tmpRCStatistics = new MyRenderContextStatistics();

		private static void UpdateResources(string page)
		{
			MyStatsDisplay.WritePersistent("GPU", "Total memory (MBs)", (int)MyRenderProxy.GetAvailableTextureMemory() / 1024 / 1024, page);
			string group = "Resources count";
			MyStatsDisplay.Write(group, "Array textures", MyManagers.ArrayTextures.GetArrayTexturesCount(), page);
			MyStatsDisplay.Write(group, "DepthStencils", MyManagers.DepthStencils.GetDepthStencilsCount(), page);
			MyStatsDisplay.Write(group, "Custom textures", MyManagers.CustomTextures.GetTexturesCount(), page);
			MyStatsDisplay.Write(group, "Generated textures", MyManagers.FileTextures.GeneratedTexturesCount, page);
			MyStatsDisplay.Write(group, "Blend states", MyManagers.BlendStates.GetResourcesCount(), page);
			MyStatsDisplay.Write(group, "Depth stencil states", MyManagers.DepthStencilStates.GetResourcesCount(), page);
			MyStatsDisplay.Write(group, "Rasterizer states", MyManagers.RasterizerStates.GetResourcesCount(), page);
			MyStatsDisplay.Write(group, "Sampler states", MyManagers.SamplerStates.GetResourcesCount(), page);
			group = "Hardware Buffers";
			foreach (MyBufferStatistics item in MyManagers.Buffers.GetReport())
			{
				MyStatsDisplay.Write(group, item.Name + " TotalBuffers", item.ActiveBuffers, page);
				MyStatsDisplay.Write(group, item.Name + " TotalBytes", item.ActiveBytes, page);
				MyStatsDisplay.Write(group, item.Name + " TotalBuffersPeak", item.TotalBuffersAllocated, page);
				MyStatsDisplay.Write(group, item.Name + " TotalBytesPeak", item.TotalBytesAllocated, page);
			}
			MyBufferStatistics report = MyManagers.FoliageManager.FoliagePool.GetReport();
			MyStatsDisplay.Write(group, report.Name + " # used", report.ActiveBuffers, page);
			MyStatsDisplay.Write(group, report.Name + " bytes used", report.ActiveBytes, page);
			MyStatsDisplay.Write(group, report.Name + " # allocated", report.TotalBuffersAllocated, page);
			MyStatsDisplay.Write(group, report.Name + " bytes allocated", report.TotalBytesAllocated, page);
			group = "File Textures";
			MyFileTextureUsageReport report2 = MyManagers.FileTextures.GetReport();
			MyStatsDisplay.Write(group, "Total", report2.TexturesTotal, page);
			MyStatsDisplay.Write(group, "Loaded", report2.TexturesLoaded, page);
			MyStatsDisplay.Write(group, "Memory KB", (int)(report2.TotalTextureMemory / 1024), page);
			MyStatsDisplay.Write(group, "Total (peak)", report2.TexturesTotalPeak, page);
			MyStatsDisplay.Write(group, "Loaded (peak)", report2.TexturesLoadedPeak, page);
			MyStatsDisplay.Write(group, "Memory (peak) KB", (int)(report2.TotalTextureMemoryPeak / 1024), page);
			foreach (KeyValuePair<MyFileTextureEnum, MyPerTextureTypeUsageReport> texturesLoadedByTypeDatum in report2.TexturesLoadedByTypeData)
			{
				group = "Loaded File Textures: " + texturesLoadedByTypeDatum.Key;
				if (texturesLoadedByTypeDatum.Value.CompressedCount > 0)
				{
					MyStatsDisplay.Write(group, "Compressed Count", texturesLoadedByTypeDatum.Value.CompressedCount, page);
					MyStatsDisplay.Write(group, "Compressed Memory KB", (int)(texturesLoadedByTypeDatum.Value.CompressedMemory / 1024), page);
				}
				else
				{
					MyStatsDisplay.Remove(group, "Compressed Count", page);
					MyStatsDisplay.Remove(group, "Compressed Memory KB", page);
				}
				if (texturesLoadedByTypeDatum.Value.NoncompressedCount > 0)
				{
					MyStatsDisplay.Write(group, "NonCompressed Count", texturesLoadedByTypeDatum.Value.NoncompressedCount, page);
					MyStatsDisplay.Write(group, "NonCompressed Memory KB", (int)(texturesLoadedByTypeDatum.Value.NoncompressedMemory / 1024), page);
				}
				else
				{
					MyStatsDisplay.Remove(group, "NonCompressed Count", page);
					MyStatsDisplay.Remove(group, "NonCompressed Memory KB", page);
				}
			}
			group = "Texture Arrays";
			MyTextureStatistics statistics = MyManagers.FileArrayTextures.Statistics;
			MyStatsDisplay.Write(group, "Total", statistics.TexturesTotal, page);
			MyStatsDisplay.Write(group, "Memory KB", (int)(statistics.TotalTextureMemory / 1024), page);
			MyStatsDisplay.Write(group, "Total (peak)", statistics.TexturesTotalPeak, page);
			MyStatsDisplay.Write(group, "Memory (peak) KB", (int)(statistics.TotalTextureMemoryPeak / 1024), page);
			MyTextureTileStreamingStatistics statistics2 = MyManagers.Textures.Statistics;
			MyStatsDisplay.Write(group, "Streamed tiles loaded ", statistics2.LoadedTiles, page);
			MyStatsDisplay.Write(group, "Streamed tiles missed ", statistics2.MissedTiles, page);
			MyStatsDisplay.Write(group, "Streamed tiles total ", statistics2.StreamedTiles, page);
			MyStatsDisplay.Write(group, "Swaps performed ", statistics2.SwapsPerformed, page);
			MyStatsDisplay.Write(group, "Total swaps performed ", statistics2.TotalSwapsPerformed, page);
			MyStatsDisplay.Write(group, "Loaded tiles max priority ", statistics2.LoadedMaxPriority, page);
			MyStatsDisplay.Write(group, "Loaded tiles median priority ", statistics2.LoadedMedianPriority, page);
			MyStatsDisplay.Write(group, "Loaded tiles min priority ", statistics2.LoadedMinPriority, page);
			MyStatsDisplay.Write(group, "Missed avg ", statistics2.MissedAvgPriority, page);
			group = "RW Textures";
			statistics = MyManagers.RwTextures.Statistics;
			MyStatsDisplay.Write(group, "Total", statistics.TexturesTotal, page);
			MyStatsDisplay.Write(group, "Memory KB", (int)(statistics.TotalTextureMemory / 1024), page);
			MyStatsDisplay.Write(group, "Total (peak)", statistics.TexturesTotalPeak, page);
			MyStatsDisplay.Write(group, "Memory (peak) KB", (int)(statistics.TotalTextureMemoryPeak / 1024), page);
			group = "Generated Textures";
			statistics = MyManagers.GeneratedTextures.Statistics;
			MyStatsDisplay.Write(group, "Total", statistics.TexturesTotal, page);
			MyStatsDisplay.Write(group, "Memory KB", (int)(statistics.TotalTextureMemory / 1024), page);
			MyStatsDisplay.Write(group, "Total (peak)", statistics.TexturesTotalPeak, page);
			MyStatsDisplay.Write(group, "Memory (peak) KB", (int)(statistics.TotalTextureMemoryPeak / 1024), page);
			MyManagers.RwTexturesPool.UpdateStats(page);
		}

		private static int SumCounters(MyViewType viewType, Func<MyCullStats, int> valueSelector)
		{
			int viewCount = MyViewIds.GetViewCount(viewType);
			int num = 0;
			int id = MyViewIds.GetId(viewType, 0);
			for (int i = 0; i < viewCount; i++)
			{
				num += valueSelector(ViewCullStats[id + i]);
			}
			return num;
		}

		private static int SumStats(MyViewType viewType, Func<MyRenderStats, int> valueSelector)
		{
			int viewCount = MyViewIds.GetViewCount(viewType);
			int num = 0;
			int id = MyViewIds.GetId(viewType, 0);
			for (int i = 0; i < viewCount; i++)
			{
				num += valueSelector(m_viewRenderStats[id + i]);
			}
			return num;
		}

		private static void UpdateCulling(string page)
		{
			int cascadesCount = MyShadowCascades.Settings.Data.CascadesCount;
			string group = "Visible proxies";
			MyStatsDisplay.Write(group, "GBuffer", ViewCullStats[MyViewIds.GetMainId(0)].All(), page);
			for (int i = 0; i < cascadesCount; i++)
			{
				MyStatsDisplay.Write(group, "CSM" + i, ViewCullStats[MyViewIds.GetShadowCascadeId(i)].All(), page);
			}
			MyStatsDisplay.Write(group, "Shadow projection", SumCounters(MyViewType.ShadowProjection, (MyCullStats x) => x.All()), page);
			MyStatsDisplay.Write(group, "EnvProbe", SumCounters(MyViewType.EnvironmentProbe, (MyCullStats x) => x.All()), page);
			group = "Visible groups";
			MyStatsDisplay.Write(group, "GBuffer", ViewCullStats[MyViewIds.GetMainId(0)].Groups, page);
			for (int j = 0; j < cascadesCount; j++)
			{
				MyStatsDisplay.Write(group, "CSM" + j, ViewCullStats[MyViewIds.GetShadowCascadeId(j)].Groups, page);
			}
			MyStatsDisplay.Write(group, "Shadow projection", SumCounters(MyViewType.ShadowProjection, (MyCullStats x) => x.Groups), page);
			MyStatsDisplay.Write(group, "EnvProbe", SumCounters(MyViewType.EnvironmentProbe, (MyCullStats x) => x.Groups), page);
			group = "Visible root proxies";
			MyStatsDisplay.Write(group, "GBuffer", ViewCullStats[MyViewIds.GetMainId(0)].AllRoot(), page);
			for (int k = 0; k < cascadesCount; k++)
			{
				MyStatsDisplay.Write(group, "CSM" + k, ViewCullStats[MyViewIds.GetShadowCascadeId(k)].AllRoot(), page);
			}
			MyStatsDisplay.Write(group, "Shadow projection", SumCounters(MyViewType.ShadowProjection, (MyCullStats x) => x.AllRoot()), page);
			MyStatsDisplay.Write(group, "EnvProbe", SumCounters(MyViewType.EnvironmentProbe, (MyCullStats x) => x.AllRoot()), page);
			group = "Gbuffer Misc";
			MyStatsDisplay.Write(group, "PointLights", ViewCullStats[MyViewIds.GetMainId(0)].PointLights, page);
			MyStatsDisplay.Write(group, "SpotLights", ViewCullStats[MyViewIds.GetMainId(0)].SpotLights, page);
			MyStatsDisplay.Write(group, "Foliage", ViewCullStats[MyViewIds.GetMainId(0)].Foliage, page);
		}

		private static void UpdateRender(string page)
		{
			int cascadesCount = MyShadowCascades.Settings.Data.CascadesCount;
			string group = "Pixels in cascades";
			for (int i = 0; i < cascadesCount; i++)
			{
				MyStatsDisplay.Write(group, "CSM" + i, (int)MyManagers.Shadows.ShadowCascades.PixelCounts[i], page);
			}
			group = "Draw calls";
			MyStatsDisplay.Write(group, "GBuffer", m_viewRenderStats[MyViewIds.GetMainId(0)].Draws, page);
			for (int j = 0; j < cascadesCount; j++)
			{
				MyStatsDisplay.Write(group, "CSM" + j, m_viewRenderStats[MyViewIds.GetShadowCascadeId(j)].Draws, page);
			}
			MyStatsDisplay.Write(group, "Shadow projection", SumStats(MyViewType.ShadowProjection, (MyRenderStats x) => x.Draws), page);
			MyStatsDisplay.Write(group, "EnvProbe", SumStats(MyViewType.EnvironmentProbe, (MyRenderStats x) => x.Draws), page);
			MyStatsDisplay.Write(group, "Billboard", m_billboardRenderStats.Draws, page);
			group = "Triangles";
			MyStatsDisplay.Write(group, "GBuffer", m_viewRenderStats[MyViewIds.GetMainId(0)].Triangles, page);
			for (int k = 0; k < cascadesCount; k++)
			{
				MyStatsDisplay.Write(group, "CSM" + k, m_viewRenderStats[MyViewIds.GetShadowCascadeId(k)].Triangles, page);
			}
			MyStatsDisplay.Write(group, "Shadow projection", SumStats(MyViewType.ShadowProjection, (MyRenderStats x) => x.Triangles), page);
			MyStatsDisplay.Write(group, "EnvProbe", SumStats(MyViewType.EnvironmentProbe, (MyRenderStats x) => x.Triangles), page);
			MyStatsDisplay.Write(group, "Billboard", m_billboardRenderStats.Triangles, page);
		}

		private static void Cleanup()
		{
			for (int i = 0; i < ViewCullStats.Length; i++)
			{
				ViewCullStats[i] = default(MyCullStats);
				m_viewRenderStats[i] = default(MyRenderStats);
			}
			m_billboardRenderStats = default(MyRenderStats);
			m_totalRenderStats = default(MyRenderStats);
		}

		private static void UpdateRenderContextStats(string page, string group, MyRenderContextStatistics statistics)
		{
			MyStatsDisplay.Write(group, "RCs", statistics.RCs, page);
			MyStatsDisplay.Write(group, "Draws", statistics.Draws, page);
			MyStatsDisplay.Write(group, "Dispatches", statistics.Dispatches, page);
			MyStatsDisplay.Write(group, "SetInputLayout", statistics.SetInputLayout, page);
			MyStatsDisplay.Write(group, "SetPrimitiveTopologies", statistics.SetPrimitiveTopologies, page);
			MyStatsDisplay.Write(group, "SetIndexBuffers", statistics.SetIndexBuffers, page);
			MyStatsDisplay.Write(group, "SetVertexBuffers", statistics.SetVertexBuffers, page);
			MyStatsDisplay.Write(group, "SetBlendStates", statistics.SetBlendStates, page);
			MyStatsDisplay.Write(group, "SetDepthStencilStates", statistics.SetDepthStencilStates, page);
			MyStatsDisplay.Write(group, "SetRasterizerStates", statistics.SetRasterizerStates, page);
			MyStatsDisplay.Write(group, "SetViewports", statistics.SetViewports, page);
			MyStatsDisplay.Write(group, "SetTargets", statistics.SetTargets, page);
			MyStatsDisplay.Write(group, "ClearStates", statistics.ClearStates, page);
			MyStatsDisplay.Write(group, "SetConstantBuffers", statistics.SetConstantBuffers, page);
			MyStatsDisplay.Write(group, "SetSamplers", statistics.SetSamplers, page);
			MyStatsDisplay.Write(group, "SetSrvs", statistics.SetSrvs, page);
			MyStatsDisplay.Write(group, "SetVertexShaders", statistics.SetVertexShaders, page);
			MyStatsDisplay.Write(group, "SetGeometryShaders", statistics.SetGeometryShaders, page);
			MyStatsDisplay.Write(group, "SetPixelShaders", statistics.SetPixelShaders, page);
			MyStatsDisplay.Write(group, "SetComputeShaders", statistics.SetComputeShaders, page);
			MyStatsDisplay.Write(group, "SetUavs", statistics.SetUavs, page);
		}

		private static void UpdateScene(string page)
		{
			MyStatsDisplay.Write("Entities", "Actors", MyIDTracker<MyActor>.Count, page);
			MyStatsDisplay.Write("Entities", "InstanceBuffers", MyInstancing.Count, page);
			MyStatsDisplay.Write("Entities", "GPUEmitters", MyGPUEmitters.Count, page);
			MyStatsDisplay.Write("Entities", "Atmospheres", MyAtmosphereRenderer.Count, page);
			MyStatsDisplay.Write("Entities", "Clouds", MyCloudRenderer.Count, page);
			MyStatsDisplay.Write("Entities", "DebugDrawMesh", MyPrimitivesRenderer.MeshCount, page);
			MyStatsDisplay.Write("Entities", "Videos", MyVideoFactory.Count, page);
			MyStatsDisplay.Write("Entities", "Decals", MyScreenDecals.TotalCount, page);
			MyStatsDisplay.Write("Entities", "Billboards", MyBillboardRenderer.Count, page);
			MyComponentFactory.FillStats(page);
		}

		private static void UpdateStateChanges(string page)
		{
			UpdateRenderContextStats(page, "RC calls", MyRender11.RC.GetStatistics());
			MyRender11.RC.ClearStatistics();
		}

		private static void UpdateDecals(string page)
		{
			MyStatsDisplay.WritePersistent("Decals", "Draw count", MyScreenDecals.DrawCount, page);
			MyStatsDisplay.WritePersistent("Decals", "To remove", MyScreenDecals.ToRemove, page);
		}

		public static void UpdateStats()
		{
			UpdateScene("Scene");
			UpdateCulling("Culling");
			UpdateDecals("Culling");
			UpdateStateChanges("Render");
			UpdateRender("Render");
			UpdateResources("Resources");
			Cleanup();
		}

		public static void AddViewRenderStats(int viewId, ref MyRenderStats stats)
		{
			m_viewRenderStats[viewId].Gather(ref stats);
			m_totalRenderStats.Gather(ref stats);
		}

		public static void AddBillboardRenderStats(ref MyRenderStats stats)
		{
			m_billboardRenderStats.Gather(ref stats);
			m_totalRenderStats.Gather(ref stats);
		}
	}
}
