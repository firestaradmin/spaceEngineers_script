using System;
using System.Collections.Generic;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.GeometryStage2.SpecialPass;
using VRage.Render11.Profiler;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Scene;
using VRage.Render11.Scene.Components;
using VRage.Utils;
using VRageMath;

namespace VRageRender
{
	internal class MyHighlight : MyImmediateRC
	{
		public const byte HIGHLIGHT_STENCIL_MASK = 64;

		public const byte OVERLAPPING_STENCIL_MASK = 128;

		public const byte ALL_BITES_STENCIL_MASK = 192;

		private static readonly Dictionary<uint, HashSet<MyHighlightDesc>> m_highlights = new Dictionary<uint, HashSet<MyHighlightDesc>>();

		private static readonly HashSet<uint> m_overlappingActors = new HashSet<uint>();

		private static readonly List<uint> m_tmpIdsToRemove = new List<uint>();

		private static IBorrowedDepthStencilTexture m_depthStencilCopy;

		public static bool HasHighlights => m_highlights.Count > 0;

		internal static void CopyDepthStencil(MyRenderContext rc)
		{
			if (HasHighlights && !MyManagers.Ansel.IsSessionRunning)
			{
<<<<<<< HEAD
				MyGpuProfiler.IC_BeginBlock("CopyDepthStencil", "CopyDepthStencil", "E:\\Repo1\\Sources\\VRage.Render11\\PostprocessStage\\MyHighlight.cs");
				m_depthStencilCopy = MyGBuffer.Main.GetDepthStencilCopyRtv(rc);
				MyGpuProfiler.IC_EndBlock(0f, "CopyDepthStencil", "E:\\Repo1\\Sources\\VRage.Render11\\PostprocessStage\\MyHighlight.cs");
=======
				MyGpuProfiler.IC_BeginBlock("CopyDepthStencil", "CopyDepthStencil", "E:\\Repo3\\Sources\\VRage.Render11\\PostprocessStage\\MyHighlight.cs");
				m_depthStencilCopy = MyGBuffer.Main.GetDepthStencilCopyRtv(rc);
				MyGpuProfiler.IC_EndBlock(0f, "CopyDepthStencil", "E:\\Repo3\\Sources\\VRage.Render11\\PostprocessStage\\MyHighlight.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private static void Add(uint ID, string sectionName, Color outlineColor, float thickness, float pulseTimeInSeconds, int instanceId = -1)
		{
			if (!m_highlights.ContainsKey(ID))
			{
				m_highlights[ID] = new HashSet<MyHighlightDesc>();
			}
			m_highlights[ID].Add(new MyHighlightDesc
			{
				SectionName = sectionName,
				Color = outlineColor,
				Thickness = thickness,
				PulseTimeInSeconds = pulseTimeInSeconds,
				InstanceId = instanceId
			});
		}

		public static void WriteHighlightConstants(ref MyHighlightDesc desc)
		{
			HighlightConstantsLayout data = default(HighlightConstantsLayout);
			data.Color = desc.Color.ToVector4() * (1f - MyPostprocessSettingsWrapper.Settings.Data.SepiaStrength);
			if (desc.PulseTimeInSeconds > 0f)
			{
				data.Color.W *= (float)Math.Pow(Math.Cos(Math.PI * 2.0 * MyCommon.FrameTime.Seconds / (double)desc.PulseTimeInSeconds), 2.0);
			}
			MyMapping myMapping = MyMapping.MapDiscard(MyCommon.HighlightConstants);
			myMapping.WriteAndPosition(ref data);
			myMapping.Unmap();
		}

		private static void BlendHighlight(MyRenderContext rc, IRtvBindable target, ISrvBindable outlined, ICustomTexture fxaaTarget, IDepthStencil depthStencilCopy)
		{
<<<<<<< HEAD
			MyGpuProfiler.IC_BeginBlock("Highlight Blending", "BlendHighlight", "E:\\Repo1\\Sources\\VRage.Render11\\PostprocessStage\\MyHighlight.cs");
=======
			MyGpuProfiler.IC_BeginBlock("Highlight Blending", "BlendHighlight", "E:\\Repo3\\Sources\\VRage.Render11\\PostprocessStage\\MyHighlight.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (fxaaTarget != null)
			{
				MyBlendTargets.RunWithStencil(rc, fxaaTarget.SRgb, outlined, MyBlendStateManager.BlendAdditive, MyDepthStencilStateManager.TestHighlightOuterStencil, 0, depthStencilCopy);
				MyBlendTargets.RunWithStencil(rc, fxaaTarget.SRgb, outlined, MyBlendStateManager.BlendTransparent, MyDepthStencilStateManager.TestHighlightInnerStencil, 64, depthStencilCopy);
			}
			else if (MyRender11.MultisamplingEnabled)
			{
				MyBlendTargets.RunWithPixelStencilTest(rc, target, outlined, MyBlendStateManager.BlendAdditive, inverseTest: false, depthStencilCopy);
				MyBlendTargets.RunWithPixelStencilTest(rc, target, outlined, MyBlendStateManager.BlendTransparent, inverseTest: true, depthStencilCopy);
			}
			else
			{
				MyBlendTargets.RunWithStencil(rc, target, outlined, MyBlendStateManager.BlendAdditive, MyDepthStencilStateManager.TestHighlightOuterStencil, 0, depthStencilCopy);
				MyBlendTargets.RunWithStencil(rc, target, outlined, MyBlendStateManager.BlendTransparent, MyDepthStencilStateManager.TestHighlightInnerStencil, 64, depthStencilCopy);
			}
<<<<<<< HEAD
			MyGpuProfiler.IC_EndBlock(0f, "BlendHighlight", "E:\\Repo1\\Sources\\VRage.Render11\\PostprocessStage\\MyHighlight.cs");
=======
			MyGpuProfiler.IC_EndBlock(0f, "BlendHighlight", "E:\\Repo3\\Sources\\VRage.Render11\\PostprocessStage\\MyHighlight.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static void DrawMeshPartForGroup(MeshId model, MyActor actor, MyMergeGroupRootComponent group, MyCullProxy_2 proxy, MyHighlightDesc desc)
		{
			if (!MyMeshes.TryGetMeshSection(model, 0, desc.SectionName, out var sectionId))
			{
				return;
			}
			WriteHighlightConstants(ref desc);
			MyMeshSectionPartInfo1[] meshes = sectionId.Info.Meshes;
			for (int i = 0; i < meshes.Length; i++)
			{
				if (!group.TryGetMaterialGroup(meshes[i].Material, out var group2))
				{
					DebugRecordMeshPartCommands(model, desc.SectionName, meshes[i].Material);
					break;
				}
				if (!group2.TryGetActorIndex(actor, out var index))
				{
					break;
				}
				MyHighlightPass.Instance.RecordCommands(ref proxy.Proxies[group2.Index], index, meshes[i].PartIndex);
			}
		}

		private static void DrawMeshPartForSingle(LodMeshId lodModelId, MyRenderLod renderLod, MyHighlightDesc desc)
		{
			WriteHighlightConstants(ref desc);
			int partsNum = lodModelId.Info.PartsNum;
			for (int i = 0; i < partsNum; i++)
			{
				MyRenderableProxy obj = renderLod.RenderableProxies[i];
				MatrixD worldMatrix = obj.Parent.Owner.WorldMatrix;
				MatrixD m = worldMatrix;
				m.Translation = worldMatrix.Translation - MyRender11.Environment.Matrices.CameraPosition;
				obj.CommonObjectData.LocalMatrix = m;
				MyRenderUtils.BindShaderBundle(MyImmediateRC.RC, renderLod.RenderableProxies[i].HighlightShaders);
				MyHighlightPass.Instance.RecordCommands(renderLod.RenderableProxies[i], -1, desc.InstanceId);
			}
		}

		/// <returns>True if the section was found</returns>
		private static void DrawMeshSection(MeshId model, MyRenderableComponent rendercomp, MyRenderLod renderLod, int highlightLod, MyHighlightDesc desc)
		{
			if (!MyMeshes.TryGetMeshSection(model, highlightLod, desc.SectionName, out var sectionId))
			{
				return;
			}
			WriteHighlightConstants(ref desc);
			MyMeshSectionPartInfo1[] meshes = sectionId.Info.Meshes;
			for (int i = 0; i < meshes.Length; i++)
			{
				MyMeshSectionPartInfo1 myMeshSectionPartInfo = meshes[i];
				if (renderLod.RenderableProxies.Length <= myMeshSectionPartInfo.PartIndex)
				{
					DebugRecordMeshPartCommands(model, desc.SectionName, rendercomp, renderLod, meshes, i);
					break;
				}
				MyRenderableProxy myRenderableProxy = renderLod.RenderableProxies[myMeshSectionPartInfo.PartIndex];
				MatrixD worldMatrix = myRenderableProxy.Parent.Owner.WorldMatrix;
				MatrixD m = worldMatrix;
				m.Translation = worldMatrix.Translation - MyRender11.Environment.Matrices.CameraPosition;
				myRenderableProxy.CommonObjectData.LocalMatrix = m;
				MyRenderUtils.BindShaderBundle(MyImmediateRC.RC, myRenderableProxy.HighlightShaders);
				MyHighlightPass.Instance.RecordCommands(myRenderableProxy, myMeshSectionPartInfo.PartSubmeshIndex, desc.InstanceId);
			}
		}

		private static void DrawRenderableComponent(MyActor actor, MyRenderableComponent renderableComponent, HashSet<MyHighlightDesc> highlightDescs)
		{
<<<<<<< HEAD
=======
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (renderableComponent == null || renderableComponent.Lods == null)
			{
				return;
			}
			MyRenderLod renderLod = renderableComponent.Lods[0];
			MeshId model = renderableComponent.GetModel();
			if (!MyMeshes.TryGetLodMesh(model, 0, out var lodMeshId))
			{
				return;
			}
<<<<<<< HEAD
			foreach (MyHighlightDesc highlightDesc in highlightDescs)
			{
				if (!renderableComponent.IsRenderedStandAlone)
				{
					MyMergeGroupRootComponent mergeGroupRoot = actor.Parent.GetMergeGroupRoot();
					if (mergeGroupRoot != null)
					{
						DrawMeshPartForGroup(model, actor, mergeGroupRoot, mergeGroupRoot.m_proxy, highlightDesc);
					}
				}
				else if (!string.IsNullOrEmpty(highlightDesc.SectionName))
				{
					DrawMeshSection(model, renderableComponent, renderLod, 0, highlightDesc);
				}
				else
				{
					DrawMeshPartForSingle(lodMeshId, renderLod, highlightDesc);
				}
			}
=======
			Enumerator<MyHighlightDesc> enumerator = highlightDescs.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyHighlightDesc current = enumerator.get_Current();
					if (!renderableComponent.IsRenderedStandAlone)
					{
						MyMergeGroupRootComponent mergeGroupRoot = actor.Parent.GetMergeGroupRoot();
						if (mergeGroupRoot != null)
						{
							DrawMeshPartForGroup(model, actor, mergeGroupRoot, mergeGroupRoot.m_proxy, current);
						}
					}
					else if (!string.IsNullOrEmpty(current.SectionName))
					{
						DrawMeshSection(model, renderableComponent, renderLod, 0, current);
					}
					else
					{
						DrawMeshPartForSingle(lodMeshId, renderLod, current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static void DrawOverlappingMesh(MyRenderableProxy[] proxies)
		{
			for (int i = 0; i < proxies.Length; i++)
			{
				MyRenderUtils.BindShaderBundle(MyImmediateRC.RC, proxies[i].HighlightShaders);
				MyImmediateRC.RC.PixelShader.Set(null);
				MyHighlightPass.Instance.RecordCommands(proxies[i], -1, 0);
			}
		}

		private static void DrawHighlightedObjects()
		{
			m_tmpIdsToRemove.Clear();
			foreach (KeyValuePair<uint, HashSet<MyHighlightDesc>> highlight in m_highlights)
			{
				MyActor myActor = MyIDTracker<MyActor>.FindByID(highlight.Key);
				if (myActor == null)
				{
					continue;
				}
				MyRenderableComponent renderable = myActor.GetRenderable();
				MyInstanceComponent instance = myActor.GetInstance();
				if (renderable != null)
				{
					DrawRenderableComponent(myActor, renderable, highlight.Value);
				}
				else if (instance != null)
				{
					MyHighlightSpecialPass.DrawInstanceComponent(instance, highlight.Value);
				}
				else if (myActor.Children.Count > 0)
				{
					foreach (MyActor child in myActor.Children)
					{
						MyRenderableComponent renderable2 = child.GetRenderable();
						MyInstanceComponent instance2 = child.GetInstance();
						if (renderable2 != null)
						{
							DrawRenderableComponent(myActor, renderable2, highlight.Value);
						}
						else if (instance2 != null)
						{
							MyHighlightSpecialPass.DrawInstanceComponent(instance2, highlight.Value);
						}
					}
				}
				else
				{
					m_tmpIdsToRemove.Add(highlight.Key);
				}
			}
			foreach (uint item in m_tmpIdsToRemove)
			{
				m_highlights.Remove(item);
			}
			m_tmpIdsToRemove.Clear();
		}

		private static void DrawOverlappingObjects()
		{
<<<<<<< HEAD
			MyRenderContext rC = MyRender11.RC;
			m_tmpIdsToRemove.Clear();
			foreach (uint overlappingActor in m_overlappingActors)
			{
				MyActor myActor = MyIDTracker<MyActor>.FindByID(overlappingActor);
				if (myActor == null)
				{
					m_tmpIdsToRemove.Add(overlappingActor);
					continue;
				}
				MyRenderableComponent renderable = myActor.GetRenderable();
				MyInstanceComponent instance = myActor.GetInstance();
				if (renderable != null)
				{
					DrawOverlappingMesh(renderable.CullProxy.RenderableProxies);
				}
				else if (instance != null)
				{
					MyHighlightSpecialPass.DrawOverlappingInstance(rC, instance);
				}
				else
				{
					m_tmpIdsToRemove.Add(overlappingActor);
				}
			}
=======
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			MyRenderContext rC = MyRender11.RC;
			m_tmpIdsToRemove.Clear();
			Enumerator<uint> enumerator = m_overlappingActors.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					uint current = enumerator.get_Current();
					MyActor myActor = MyIDTracker<MyActor>.FindByID(current);
					if (myActor == null)
					{
						m_tmpIdsToRemove.Add(current);
						continue;
					}
					MyRenderableComponent renderable = myActor.GetRenderable();
					MyInstanceComponent instance = myActor.GetInstance();
					if (renderable != null)
					{
						DrawOverlappingMesh(renderable.CullProxy.RenderableProxies);
					}
					else if (instance != null)
					{
						MyHighlightSpecialPass.DrawOverlappingInstance(rC, instance);
					}
					else
					{
						m_tmpIdsToRemove.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			foreach (uint item in m_tmpIdsToRemove)
			{
				m_overlappingActors.Remove(item);
			}
			m_tmpIdsToRemove.Clear();
		}

		public static void RemoveOverlappingModel(uint overlappingModelId)
		{
			m_overlappingActors.Remove(overlappingModelId);
		}

		public static void AddOverlappingModel(uint overlappingModelId)
		{
			m_overlappingActors.Add(overlappingModelId);
		}

		public static void SetOverlappingModels(List<uint> overlappingModelIDs)
		{
			m_overlappingActors.Clear();
			foreach (uint overlappingModelID in overlappingModelIDs)
			{
				m_overlappingActors.Add(overlappingModelID);
			}
		}

<<<<<<< HEAD
		/// <summary>
		///
		/// </summary>
		/// <param name="ID"></param>
		/// <param name="sectionNames">null for all the mesh</param>
		/// <param name="outlineColor"></param>
		/// <param name="thickness">Zero or negative remove the outline</param>                
		/// <param name="pulseTimeInSeconds"></param>
		/// <param name="instanceId"></param>
=======
		/// <param name="sectionIndices">null for all the mesh</param>
		/// <param name="thickness">Zero or negative remove the outline</param>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static void AddObjects(uint ID, string[] sectionNames, Color? outlineColor, float thickness, float pulseTimeInSeconds, int instanceId)
		{
			if (!(thickness > 0f))
			{
				return;
			}
			if (sectionNames == null)
			{
				Add(ID, null, outlineColor.Value, thickness, pulseTimeInSeconds, instanceId);
				return;
			}
			foreach (string sectionName in sectionNames)
			{
				Add(ID, sectionName, outlineColor.Value, thickness, pulseTimeInSeconds, instanceId);
			}
		}

		public static void RemoveObjects(uint ID, string[] sectionNames)
		{
			if (sectionNames == null)
			{
				m_highlights.Remove(ID);
			}
			else
			{
				RemoveObjectsSections(ID, sectionNames);
			}
		}

		public static void Run(MyRenderContext rc, IRtvBindable target, ICustomTexture fxaaTarget)
		{
<<<<<<< HEAD
=======
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!HasHighlights)
			{
				return;
			}
			MyHighlightPass.Instance.ViewProjection = MyRender11.Environment.Matrices.ViewProjectionAt0;
			MyHighlightPass.Instance.Viewport = new MyViewport(MyRender11.ViewportResolution.X, MyRender11.ViewportResolution.Y);
			MyHighlightPass.Instance.Begin();
			int samplesCount = MyRender11.Settings.User.AntialiasingMode.SamplesCount();
			IBorrowedRtvTexture borrowedRtvTexture = MyManagers.RwTexturesPool.BorrowRtv("MyHighlight.Rgba8_1", Format.R8G8B8A8_UNorm_SRgb, samplesCount);
			MyImmediateRC.RC.ClearRtv(borrowedRtvTexture, default(RawColor4));
			MyImmediateRC.RC.SetRtv(m_depthStencilCopy.DsvRoDepth, borrowedRtvTexture);
			DrawHighlightedObjects();
			MyHighlightPass.Instance.End();
			MyImmediateRC.RC.SetBlendState(null);
			ISrvBindable initialResourceView = borrowedRtvTexture;
			IRtvBindable renderTarget = borrowedRtvTexture;
			float num = 0f;
			foreach (KeyValuePair<uint, HashSet<MyHighlightDesc>> highlight in m_highlights)
			{
<<<<<<< HEAD
				foreach (MyHighlightDesc item in highlight.Value)
				{
					num = Math.Max(num, item.Thickness);
=======
				Enumerator<MyHighlightDesc> enumerator2 = highlight.Value.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						num = Math.Max(num, enumerator2.get_Current().Thickness);
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
			}
			if (num > 0f)
			{
				IBorrowedRtvTexture borrowedRtvTexture2 = MyManagers.RwTexturesPool.BorrowRtv("MyHighlight.Rgba8_2", Format.R8G8B8A8_UNorm_SRgb);
				MyBlur.Run(rc, renderTarget, borrowedRtvTexture2, initialResourceView, (int)Math.Round(num), MyBlur.MyBlurDensityFunctionType.Exponential, 0.25f, MyDepthStencilStateManager.IgnoreDepthStencil);
				borrowedRtvTexture2.Release();
			}
			MyHighlightPass.Instance.Begin();
			MyRender11.RC.SetRtv(m_depthStencilCopy.DsvRoDepth);
			MyRender11.RC.SetDepthStencilState(MyDepthStencilStateManager.WriteOverlappingHighlightStencil, 128);
			DrawOverlappingObjects();
			MyHighlightPass.Instance.End();
			BlendHighlight(rc, target, borrowedRtvTexture, fxaaTarget, m_depthStencilCopy);
			m_depthStencilCopy.Release();
		}

		private static void DebugRecordMeshPartCommands(MeshId model, string sectionName, MyRenderableComponent render, MyRenderLod renderLod, MyMeshSectionPartInfo1[] meshes, int index)
		{
			MyRenderProxy.Error("DebugRecordMeshPartCommands1: Call Francesco");
			MyLog.Default.WriteLine("DebugRecordMeshPartCommands1");
			MyLog.Default.WriteLine("sectionName: " + sectionName);
			MyLog.Default.WriteLine("model.Info.Name: " + model.Info.Name);
			MyLog.Default.WriteLine("render.CurrentLod: " + render.CurrentLod);
			MyLog.Default.WriteLine("renderLod.RenderableProxies.Length: " + renderLod.RenderableProxies.Length);
			MyLog.Default.WriteLine("meshes.Length: " + meshes.Length);
			MyLog.Default.WriteLine("Mesh index: " + index);
			MyLog.Default.WriteLine("Mesh part index: " + meshes[index].PartIndex);
		}

		private static void DebugRecordMeshPartCommands(MeshId model, string sectionName, MyMeshMaterialId material)
		{
			MyRenderProxy.Error("DebugRecordMeshPartCommands2: Call Francesco");
			MyLog.Default.WriteLine("DebugRecordMeshPartCommands2");
			MyLog.Default.WriteLine("sectionName: " + sectionName);
			MyLog.Default.WriteLine("model.Info.Name: " + model.Info.Name);
			MyLog.Default.WriteLine("material.Info.Name: " + material.Info.Name);
		}

		private static void RemoveObjectsSections(uint ID, string[] sectionNames)
		{
<<<<<<< HEAD
=======
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (!m_highlights.ContainsKey(ID))
			{
				return;
			}
<<<<<<< HEAD
			HashSet<MyHighlightDesc> hashSet = m_highlights[ID];
			List<MyHighlightDesc> list = new List<MyHighlightDesc>();
			foreach (MyHighlightDesc item in hashSet)
			{
				if (sectionNames.Contains(item.SectionName))
				{
					list.Add(item);
				}
			}
			foreach (MyHighlightDesc item2 in list)
			{
				hashSet.Remove(item2);
			}
			if (hashSet.Count == 0)
=======
			HashSet<MyHighlightDesc> val = m_highlights[ID];
			List<MyHighlightDesc> list = new List<MyHighlightDesc>();
			Enumerator<MyHighlightDesc> enumerator = val.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyHighlightDesc current = enumerator.get_Current();
					if (sectionNames.Contains(current.SectionName))
					{
						list.Add(current);
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			foreach (MyHighlightDesc item in list)
			{
				val.Remove(item);
			}
			if (val.get_Count() == 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_highlights.Remove(ID);
			}
		}

		public static void Init()
		{
			MyBlur.InitShaders(MyBlur.MyBlurDensityFunctionType.Exponential, 8, 0f);
		}
	}
}
