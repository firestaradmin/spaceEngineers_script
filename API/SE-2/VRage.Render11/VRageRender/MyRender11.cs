using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading;
using ParallelTasks;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using VRage;
using VRage.FileSystem;
using VRage.Library.Utils;
using VRage.Profiler;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.GBufferResolve;
using VRage.Render11.GeometryStage.Geometry;
using VRage.Render11.GeometryStage.Voxel;
using VRage.Render11.GeometryStage2.Common;
using VRage.Render11.GeometryStage2.Instancing;
using VRage.Render11.GeometryStage2.Lodding;
using VRage.Render11.GeometryStage2.Model;
using VRage.Render11.GeometryStage2.StaticGroup;
using VRage.Render11.LightingStage;
using VRage.Render11.LightingStage.EnvironmentProbe;
using VRage.Render11.PostprocessStage;
using VRage.Render11.Profiler;
using VRage.Render11.Profiler.Internal;
using VRage.Render11.Render;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRage.Render11.Scene;
using VRage.Render11.Scene.Components;
using VRage.Render11.Sprites;
using VRage.Render11.Tools;
using VRage.Utils;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender.Import;
using VRageRender.Messages;
using VRageRender.Utils;
using VRageRender.Vertex;
using VRageRender.Voxels;

namespace VRageRender
{
	internal static class MyRender11
	{
		private delegate bool MessageFilterDelegate(MyRenderMessageEnum messageType);

		private static readonly Dictionary<int, MyRenderFont> m_fontsById;

		private static MyRenderFont DummyFont;

		private static IMyUpdateBatch m_meshBatch;

		private static readonly List<MyRenderMessageBase> m_persistentDebugMessages;

		private const int MAX_PERSISTANT_MSG_COUNT = 1000;

		private static MyRenderContext m_rc;

		internal static bool LockImmediateRC;

		private static MyRenderDeviceSettings m_settings;

		[ThreadStatic]
		private static StringBuilder m_debugStringBuilder;

		private static long m_lastSkippedCount;

		private static bool m_initialized;

		private static bool m_initializedOnce;

		private static readonly List<MyRenderMessageBase> m_debugDrawMessages;

		private static bool m_drawScene;

		private static MyScreenshot? m_screenshot;

		private static List<renderColoredTextureProperties> m_texturesToRender;

		private static readonly StringBuilder m_exceptionBuilder;

		private static MySpritesRenderer m_mainSprites;

		private static readonly ConcurrentQueue<Action> m_deferredUpdate;

		private static MyFinishedContext m_mainSpritesFC;

		private static Task m_mainSpritesTask;

		private static float m_mainViewportScaleFactor;

		private static MyRenderDebugOverrides m_debugOverrides;

		internal static MyPostprocessSettings Postprocess;

		private static bool m_resetEyeAdaptation;

		private static MyPixelShaders.Id m_stretchPs;

		internal static FrameProcessStatusEnum FrameProcessStatus;

		private static bool m_deferStateChanges;

		private static bool m_deferredStateChanges;

		private static int m_messageFrameCounter;

		private static Task m_processTask;

		private static readonly Stopwatch m_processStopwatch;

		private static int m_processCtr;

		private const int DEPTH_BIAS_NEAR = 50000;

		private static readonly ShaderMacro[] m_shaderSampleFrequencyDefine;

		internal static SwapChain m_swapchain;

		internal static Vector2I m_resolution;

<<<<<<< HEAD
=======
		private static int ctr;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		internal static ObjectIDGenerator IdGenerator;

		internal static bool UseComplementaryDepthBuffer;

		/// <summary>
		/// Do not use these timers for anything affecting rendered stuff. Use MyCommon.FrameTime instead.
		/// </summary>
		internal static MyTimeSpan CurrentDrawTime;

		internal static MyTimeSpan CurrentUpdateTime;

		internal static MyTimeSpan PreviousDrawTime;

		internal static MySharedData SharedData;

		internal static MyLog Log;

		internal static string RootDirectory;

		internal static string RootDirectoryEffects;

		internal static string RootDirectoryDebug;

		internal static MyTimeSpan CPUDraw;

		internal static MyTimeSpan CPUWait;

		internal static uint GlobalMessageCounter;

		internal static MyRenderSettings Settings;

		internal static readonly MyEnvironment Environment;

		private static readonly MyRenderProfiler m_renderProfiler;

<<<<<<< HEAD
=======
		private static double m_lastGpuLoad;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private static string m_blockName;

		private static int m_blockInnerCounter;

		private static readonly HashSet<string> m_whiteList;

		private static readonly Dictionary<string, MyTimeSpan> m_lastTimings;

		public static Func<Vector3D, Vector3> CalculateGravityInPoint;

		internal const bool DebugMode = false;

<<<<<<< HEAD
=======
		private static Matrix m_proj;

		private static Matrix m_vp;

		private static Matrix m_invvp;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static bool SceneSystemsReady { get; private set; }

		internal static MyRenderFont DebugFont { get; private set; }

		public static int DebugFontId { get; set; }

		internal static SharpDX.Direct3D11.Device1 DeviceInstance { get; private set; }

		internal static DeviceDebug DebugDevice { get; private set; }

		internal static MyRenderContext RC
		{
			get
			{
				return m_rc;
			}
			private set
			{
				Log.WriteLine("Device Context change");
				m_rc = value;
			}
		}

		internal static MyRenderContext RCForQueries => m_rc;

		internal static MyRenderDeviceSettings DeviceSettings => m_settings;

		internal static Vector2 ResolutionF => new Vector2(m_resolution.X, m_resolution.Y);

		internal static Vector2I ResolutionI => m_resolution;

		private static SharpDX.Direct3D11.InfoQueue DebugInfoQueue { get; set; }

		public static Thread RenderThread { get; private set; }

		private static StringBuilder DebugStringBuilder => m_debugStringBuilder ?? (m_debugStringBuilder = new StringBuilder());

		public static bool BatchedConstantBufferMapping { get; private set; }

		public static bool ParallelVertexBufferMapping { get; private set; }

		public static bool SimpleEnvironmentProbe { get; private set; }

		public static List<MyRenderMessageBase> DebugDrawMessages => m_debugDrawMessages;

		public static IMyVoxelUpdateBatch DeferStateChangeBatch { get; private set; }

		internal static MyRenderDebugOverrides DebugOverrides => m_debugOverrides;

		internal static bool MultisamplingEnabled => Settings.User.AntialiasingMode.IsMultisampled();

		internal static int MultisamplingSampleCount => Settings.User.AntialiasingMode.SamplesCount();

		internal static bool FxaaEnabled
		{
			get
			{
				if (Settings.User.AntialiasingMode == MyAntialiasingMode.FXAA && m_debugOverrides.Postprocessing)
				{
					return m_debugOverrides.Fxaa;
				}
				return false;
			}
		}

		internal static MyBackbuffer Backbuffer { get; private set; }

		internal static Vector2I BackBufferResolution
		{
			get
			{
				return m_resolution;
			}
			private set
			{
			}
		}

		internal static Vector2I ViewportResolution => BackBufferResolution;

		internal static float DepthClearValue => (!UseComplementaryDepthBuffer) ? 1 : 0;

		public static int GameplayFrameCounter { get; private set; }

		internal static MyMessageQueue OutputQueue => SharedData.RenderOutputMessageQueue;

		private static void InitSubsystemsOnce()
		{
		}

		private static void InitSubsystems(bool initParallel)
		{
			MyShaders.Init();
			MyCommon.Init();
			MyVertexLayouts.Init();
			MyLinesRenderer.Init();
			MyPrimitivesRenderer.Init();
			MyManagers.OnDeviceInit();
			m_mainSprites = MyManagers.SpritesManager.GetSpritesRenderer();
			MyScreenPass.Init(MyImmediateRC.RC);
			MyRenderContext deferredRC = MyManagers.DeferredRCs.AcquireRC("InitSubsytems_RC");
			if (initParallel)
			{
				Parallel.Start(delegate
				{
					InitSubsystemsAsync(deferredRC);
				});
			}
			else
			{
				InitSubsystemsAsync(deferredRC);
			}
		}

		private static void InitSubsystemsConsume(MyRenderContext rc)
		{
			MyFinishedContext fc = rc.FinishDeferredContext();
<<<<<<< HEAD
			RC.ExecuteContext(ref fc, "InitSubsystemsConsume", 62, "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Content.cs");
=======
			RC.ExecuteContext(ref fc, "InitSubsystemsConsume", 62, "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Content.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			RC.ClearState();
			MyBillboardRenderer.FinalizeInit();
			OnSessionStart();
			SceneSystemsReady = true;
		}

		private static void InitSubsystemsAsync(MyRenderContext rc)
		{
			m_meshBatch = MyMeshes.OpenMeshUpdateBatch();
			InitDummyFont();
			ResetShadows(rc, MyShadowCascades.Settings.Data.CascadesCount, Settings.User.ShadowGPUQuality.ShadowCascadeResolution());
			MyMeshes.Init();
			MyMeshTableSrv.Init();
			MyLightsRendering.Init(m_meshBatch);
			MyBlur.Init();
			MyTransparentRendering.Init();
			MyBillboardRenderer.Init();
			MyDebugRenderer.Init();
			MyScreenDecals.Init();
			MyEnvProbeProcessing.Init();
			MyAtmosphereRenderer.Init(m_meshBatch);
			MyCloudRenderer.Init();
			MyAAEdgeMarking.Init();
			MyCopyToRT.Init();
			MyBlendTargets.Init();
			MyFXAA.Init();
			MyDepthResolve.Init();
			MyModernBloom.Init();
			MyLuminanceAverage.Init();
			MyEyeAdaptation.Init(rc);
			MyToneMapping.Init();
			MyChromaticAberration.Init();
			MySSAO.Init();
			MyHdrDebugTools.Init();
			MyMaterials1.Init();
			MyVoxelMaterials.Init();
			MyMeshMaterials1.Init();
			MyHighlight.Init();
			MyHBAO.Init(rc);
			MyMeshMaterials1.Init();
			try
			{
				if (m_settings.UseStereoRendering)
				{
					MyStereoStencilMask.InitUsingOpenVR();
				}
			}
			catch (Exception)
			{
				MyStereoStencilMask.InitUsingUndefinedMask();
			}
			MyScene11.Instance.Updater.CallIn(delegate
			{
				InitSubsystemsConsume(rc);
			}, MyTimeSpan.Zero);
		}

		private static void OnDeviceReset()
		{
			MyManagers.OnDeviceReset();
			MyShaders.OnDeviceReset();
			MyMaterialShaders.OnDeviceReset();
			MyTransparentRendering.OnDeviceReset();
			ResetShadows(MyImmediateRC.RC, MyShadowCascades.Settings.Data.CascadesCount, Settings.User.ShadowGPUQuality.ShadowCascadeResolution());
			MyBillboardRenderer.OnDeviceReset();
			MyScreenDecals.OnDeviceReset();
			MyMeshMaterials1.OnDeviceReset();
			MyVoxelMaterials.OnDeviceReset();
			MyRenderableComponent.MarkAllDirty();
			foreach (MyMergeGroupRootComponent item in MyComponentFactory<MyMergeGroupRootComponent>.GetAll())
			{
				item.OnDeviceReset();
			}
			MyBigMeshTable.Table.OnDeviceReset();
			MyMeshes.OnDeviceReset();
			MyInstancing.OnDeviceReset();
			MyScreenDecals.OnDeviceReset();
		}

		private static void OnDeviceEnd()
		{
			MyScreenDecals.OnDeviceEnd();
			MyShaders.OnDeviceEnd();
			MyMaterialShaders.OnDeviceEnd();
			MyVoxelMaterials.OnDeviceEnd();
			MyTransparentRendering.OnDeviceEnd();
			MyBillboardRenderer.OnDeviceEnd();
			MyAtmosphereRenderer.OnDeviceEnd();
			MyMeshes.OnDeviceEnd();
			MyMeshMaterials1.OnDeviceEnd();
			MyMaterials1.OnDeviceEnd();
			MyManagers.OnDeviceEnd();
		}

		private static void OnSessionStart()
		{
			m_meshBatch?.Commit();
			m_meshBatch = null;
			MyManagers.Buffers.OnSessionStart();
			MyManagers.RwTextures.OnSessionStart();
		}

		private static void OnSessionEnd()
		{
			Log.WriteLine("Unloading session data");
			RC.UnloadData();
			MyScene11.Instance.Updater.ForceDelayedCalls();
			MyAtmosphereRenderer.OnSessionEnd();
			MyMeshes.OnSessionEnd();
			MyCommon.UnloadData();
			MyManagers.ParticleEffectsManager.UnloadData();
			MyActorFactory.RemoveAll();
			m_debugDrawMessages.Clear();
			m_persistentDebugMessages.Clear();
			MyScene11.Instance.Clear();
			MyAlphaTransition.Unload();
			MyInstancing.OnSessionEnd();
			MyMaterials1.OnSessionEnd();
			MyVoxelMaterials.OnSessionEnd();
			MyMeshMaterials1.OnSessionEnd();
			MyScreenDecals.OnSessionEnd();
			MyBigMeshTable.Table.OnSessionEnd();
			MyPrimitivesRenderer.Unload();
			MyTransparentRendering.OnSessionEnd();
			MyBillboardRenderer.OnSessionEnd();
			MyManagers.OnUnloadData();
		}

		private static void GatherTextures()
		{
			MyMeshMaterials1.OnResourcesGathering(preloadTextures: false);
			MyVoxelMaterials.OnResourcesGather();
		}

		private static void AddFont(int id, MyRenderFont font, bool isDebugFont)
		{
			if (isDebugFont)
			{
				DebugFont = font;
				DebugFontId = id;
			}
			m_fontsById[id] = font;
		}

		private static void InitDummyFont()
		{
			DummyFont = new MyRenderFont("DummyFont", Color.Magenta, dummyFont: true);
		}

		private static void LoadFont(int fontId, bool isDebugFont, string fontPath, Color colorMask)
		{
			if (m_fontsById.TryGetValue(fontId, out var value) && value != DummyFont)
			{
				if (value.FontFilePath == fontPath)
				{
					return;
				}
				value.Unload();
			}
			AddFont(fontId, DummyFont, isDebugFont);
			MyRenderFont renderFont = null;
			Parallel.Start(delegate
			{
				renderFont = new MyRenderFont(fontPath, colorMask);
				renderFont.LoadContent();
			}, delegate
			{
				renderFont.ConsumeContent();
				AddFont(fontId, renderFont, isDebugFont);
			}, WorkPriority.VeryHigh);
		}

		internal static MyRenderFont GetDebugFont()
		{
			return DebugFont;
		}

		internal static MyRenderFont GetFont(int id)
		{
			if (m_fontsById.TryGetValue(id, out var value))
			{
				return value;
			}
			return DebugFont;
		}

		private static void ReloadFonts()
		{
			foreach (KeyValuePair<int, MyRenderFont> item in m_fontsById)
			{
				item.Value.LoadContent();
				item.Value.ConsumeContent();
			}
			DebugFont.LoadContent();
			DebugFont.ConsumeContent();
		}

		private static void CreateScreenResources()
		{
			RemoveScreenResources();
			int x = m_resolution.X;
			int y = m_resolution.Y;
			int samplesNum = Settings.User.AntialiasingMode.SamplesCount();
			MyUtils.Init(ref MyGBuffer.Main);
			HDRType hdrType = ((!Settings.HDREnabled) ? HDRType.HDR : ((!Settings.User.HqTarget) ? HDRType.HDR : HDRType.HDR_HQ));
			MyGBuffer.Main.Resize(x, y, samplesNum, 0, hdrType);
			MyLightsRendering.Resize(x, y);
			MyHBAO.InitScreenResources();
		}

		private static void RemoveScreenResources()
		{
			MyHBAO.ReleaseScreenResources();
			MyLightsRendering.Dispose();
			if (MyGBuffer.Main != null)
			{
				MyGBuffer.Main.Release();
				MyGBuffer.Main = null;
			}
		}

		private static void ProcessDebugMessages()
		{
			bool flag = ProcessDebugMessages(m_debugDrawMessages);
			ProcessDebugMessages(m_persistentDebugMessages);
			foreach (MyRenderMessageBase debugDrawMessage in m_debugDrawMessages)
			{
				if (debugDrawMessage.IsPersistent)
				{
					m_persistentDebugMessages.Add(debugDrawMessage);
				}
			}
			m_debugDrawMessages.Clear();
			if (flag)
			{
				foreach (MyRenderMessageBase persistentDebugMessage in m_persistentDebugMessages)
				{
					persistentDebugMessage.Dispose();
				}
				m_persistentDebugMessages.Clear();
			}
			else if (m_persistentDebugMessages.Count > 1000)
			{
				int num = m_persistentDebugMessages.Count - 1000;
				for (int i = 0; i < num; i++)
				{
					m_persistentDebugMessages[i].Dispose();
				}
				m_persistentDebugMessages.RemoveRange(0, num);
			}
		}

		private unsafe static bool ProcessDebugMessages(List<MyRenderMessageBase> messageList)
		{
			if (messageList.Count == 0)
			{
				return false;
			}
			bool result = false;
			MyLinesBatch myLinesBatch = MyLinesRenderer.CreateBatch();
			MyLinesBatch myLinesBatch2 = MyLinesRenderer.CreateBatch();
			myLinesBatch2.IgnoreDepth = true;
			MyLinesBatch myLinesBatch3 = MyLinesRenderer.CreateBatch();
			myLinesBatch3.IgnoreDepth = true;
			foreach (MyRenderMessageBase message in messageList)
			{
				switch (message.MessageType)
				{
				case MyRenderMessageEnum.DebugDrawLine3D:
				{
					MyRenderMessageDebugDrawLine3D myRenderMessageDebugDrawLine3D = (MyRenderMessageDebugDrawLine3D)message;
					if (myRenderMessageDebugDrawLine3D.DepthRead)
					{
<<<<<<< HEAD
						myLinesBatch.Add((Vector3)(myRenderMessageDebugDrawLine3D.PointFrom - Environment.Matrices.CameraPosition), (Vector3)(myRenderMessageDebugDrawLine3D.PointTo - Environment.Matrices.CameraPosition), myRenderMessageDebugDrawLine3D.ColorFrom, myRenderMessageDebugDrawLine3D.ColorTo);
					}
					else
					{
						myLinesBatch2.Add((Vector3)(myRenderMessageDebugDrawLine3D.PointFrom - Environment.Matrices.CameraPosition), (Vector3)(myRenderMessageDebugDrawLine3D.PointTo - Environment.Matrices.CameraPosition), myRenderMessageDebugDrawLine3D.ColorFrom, myRenderMessageDebugDrawLine3D.ColorTo);
=======
						myLinesBatch.Add(myRenderMessageDebugDrawLine3D.PointFrom - Environment.Matrices.CameraPosition, myRenderMessageDebugDrawLine3D.PointTo - Environment.Matrices.CameraPosition, myRenderMessageDebugDrawLine3D.ColorFrom, myRenderMessageDebugDrawLine3D.ColorTo);
					}
					else
					{
						myLinesBatch2.Add(myRenderMessageDebugDrawLine3D.PointFrom - Environment.Matrices.CameraPosition, myRenderMessageDebugDrawLine3D.PointTo - Environment.Matrices.CameraPosition, myRenderMessageDebugDrawLine3D.ColorFrom, myRenderMessageDebugDrawLine3D.ColorTo);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					break;
				}
				case MyRenderMessageEnum.DebugDrawLine2D:
				{
					MyRenderMessageDebugDrawLine2D myRenderMessageDebugDrawLine2D = (MyRenderMessageDebugDrawLine2D)message;
					Matrix matrix3 = myRenderMessageDebugDrawLine2D.Projection ?? Matrix.CreateOrthographicOffCenter(0f, ViewportResolution.X, ViewportResolution.Y, 0f, 0f, -1f);
					if (!myLinesBatch3.CustomViewProjection.HasValue || (myLinesBatch3.CustomViewProjection.HasValue && myLinesBatch3.CustomViewProjection.Value != matrix3))
					{
						myLinesBatch3.Commit();
						myLinesBatch3 = MyLinesRenderer.CreateBatch();
						myLinesBatch3.IgnoreDepth = true;
						myLinesBatch3.CustomViewProjection = matrix3;
					}
<<<<<<< HEAD
					Vector3 vector2 = new Vector3(myRenderMessageDebugDrawLine2D.PointFrom.X, myRenderMessageDebugDrawLine2D.PointFrom.Y, 0f);
					myLinesBatch3.Add(to: new Vector3(myRenderMessageDebugDrawLine2D.PointTo.X, myRenderMessageDebugDrawLine2D.PointTo.Y, 0f), from: vector2, colorFrom: myRenderMessageDebugDrawLine2D.ColorFrom, colorTo: myRenderMessageDebugDrawLine2D.ColorTo);
=======
					Vector3 from = new Vector3(myRenderMessageDebugDrawLine2D.PointFrom.X, myRenderMessageDebugDrawLine2D.PointFrom.Y, 0f);
					Vector3 to = new Vector3(myRenderMessageDebugDrawLine2D.PointTo.X, myRenderMessageDebugDrawLine2D.PointTo.Y, 0f);
					myLinesBatch3.Add(from, to, myRenderMessageDebugDrawLine2D.ColorFrom, myRenderMessageDebugDrawLine2D.ColorTo);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					break;
				}
				case MyRenderMessageEnum.DebugDrawPoint:
				{
					MyRenderMessageDebugDrawPoint myRenderMessageDebugDrawPoint = (MyRenderMessageDebugDrawPoint)message;
					float num2 = 0.125f;
					float num3 = (UseComplementaryDepthBuffer ? 0f : 1f);
					num3 = (myRenderMessageDebugDrawPoint.ClipDistance.HasValue ? Vector3.Transform(new Vector3(0f, 0f, 0f - myRenderMessageDebugDrawPoint.ClipDistance.Value), Environment.Matrices.Projection).Z : num3);
					Vector3D vector3D2 = myRenderMessageDebugDrawPoint.Position - Environment.Matrices.CameraPosition;
					Vector3D vector3D3 = Vector3D.Transform(vector3D2, Environment.Matrices.ViewProjectionAt0);
					vector3D3.X = vector3D3.X * 0.5 + 0.5;
					vector3D3.Y = vector3D3.Y * -0.5 + 0.5;
					if ((!UseComplementaryDepthBuffer) ? (vector3D3.Z < (double)num3 && vector3D3.Z > 0.0) : (vector3D3.Z > (double)num3 && vector3D3.Z < 1.0))
					{
						MyLinesBatch obj2 = (myRenderMessageDebugDrawPoint.DepthRead ? myLinesBatch : myLinesBatch2);
<<<<<<< HEAD
						obj2.Add((Vector3)(vector3D2 + Vector3.UnitX * num2), (Vector3)(vector3D2 - Vector3.UnitX * num2), myRenderMessageDebugDrawPoint.Color);
						obj2.Add((Vector3)(vector3D2 + Vector3.UnitY * num2), (Vector3)(vector3D2 - Vector3.UnitY * num2), myRenderMessageDebugDrawPoint.Color);
						obj2.Add((Vector3)(vector3D2 + Vector3.UnitZ * num2), (Vector3)(vector3D2 - Vector3.UnitZ * num2), myRenderMessageDebugDrawPoint.Color);
=======
						obj2.Add(vector3D2 + Vector3.UnitX * num2, vector3D2 - Vector3.UnitX * num2, myRenderMessageDebugDrawPoint.Color);
						obj2.Add(vector3D2 + Vector3.UnitY * num2, vector3D2 - Vector3.UnitY * num2, myRenderMessageDebugDrawPoint.Color);
						obj2.Add(vector3D2 + Vector3.UnitZ * num2, vector3D2 - Vector3.UnitZ * num2, myRenderMessageDebugDrawPoint.Color);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					break;
				}
				case MyRenderMessageEnum.DebugDrawSphere:
				{
					MyRenderMessageDebugDrawSphere myRenderMessageDebugDrawSphere = (MyRenderMessageDebugDrawSphere)message;
					float num4 = (UseComplementaryDepthBuffer ? 0f : 1f);
					num4 = (myRenderMessageDebugDrawSphere.ClipDistance.HasValue ? Vector3.Transform(new Vector3(0f, 0f, 0f - myRenderMessageDebugDrawSphere.ClipDistance.Value), Environment.Matrices.Projection).Z : num4);
					Vector3D vector3D4 = myRenderMessageDebugDrawSphere.Position - Environment.Matrices.CameraPosition;
					Vector3D vector3D5 = Vector3D.Transform(vector3D4, Environment.Matrices.ViewProjectionAt0);
					vector3D5.X = vector3D5.X * 0.5 + 0.5;
					vector3D5.Y = vector3D5.Y * -0.5 + 0.5;
					if ((!UseComplementaryDepthBuffer) ? (vector3D5.Z < (double)num4 && vector3D5.Z > 0.0) : (vector3D5.Z > (double)num4 && vector3D5.Z < 1.0))
					{
						if (myRenderMessageDebugDrawSphere.Smooth)
						{
							MyPrimitivesRenderer.DrawSphere(vector3D4, myRenderMessageDebugDrawSphere.Radius, myRenderMessageDebugDrawSphere.Color);
							break;
						}
						MyLinesBatch obj3 = (myRenderMessageDebugDrawSphere.DepthRead ? myLinesBatch : myLinesBatch2);
						obj3.AddSphereRing(new BoundingSphere(vector3D4, myRenderMessageDebugDrawSphere.Radius), myRenderMessageDebugDrawSphere.Color, Matrix.Identity);
						obj3.AddSphereRing(new BoundingSphere(vector3D4, myRenderMessageDebugDrawSphere.Radius), myRenderMessageDebugDrawSphere.Color, Matrix.CreateRotationX((float)Math.E * 449f / 777f));
						obj3.AddSphereRing(new BoundingSphere(vector3D4, myRenderMessageDebugDrawSphere.Radius), myRenderMessageDebugDrawSphere.Color, Matrix.CreateRotationZ((float)Math.E * 449f / 777f));
					}
					break;
				}
				case MyRenderMessageEnum.DebugDrawAABB:
				{
					MyRenderMessageDebugDrawAABB myRenderMessageDebugDrawAABB = (MyRenderMessageDebugDrawAABB)message;
					BoundingBox bb = (BoundingBox)myRenderMessageDebugDrawAABB.AABB;
					bb.Translate(-Environment.Matrices.CameraPosition);
					if (myRenderMessageDebugDrawAABB.DepthRead)
					{
						myLinesBatch.AddBoundingBox(bb, myRenderMessageDebugDrawAABB.Color);
					}
					else
					{
						myLinesBatch2.AddBoundingBox(bb, myRenderMessageDebugDrawAABB.Color);
					}
					if (myRenderMessageDebugDrawAABB.Shaded)
					{
						Vector3* ptr = stackalloc Vector3[8];
						bb.GetCornersUnsafe(ptr);
						MyPrimitivesRenderer.Draw6FacedConvexZ(ptr, myRenderMessageDebugDrawAABB.Color, myRenderMessageDebugDrawAABB.Alpha);
					}
					break;
				}
				case MyRenderMessageEnum.DebugDraw6FaceConvex:
				{
					MyRenderMessageDebugDraw6FaceConvex myRenderMessageDebugDraw6FaceConvex = (MyRenderMessageDebugDraw6FaceConvex)message;
					if (myRenderMessageDebugDraw6FaceConvex.Fill)
					{
						MyPrimitivesRenderer.Draw6FacedConvex(myRenderMessageDebugDraw6FaceConvex.Vertices, myRenderMessageDebugDraw6FaceConvex.Color, myRenderMessageDebugDraw6FaceConvex.Alpha);
					}
					else if (myRenderMessageDebugDraw6FaceConvex.DepthRead)
					{
						myLinesBatch.Add6FacedConvexWorld(myRenderMessageDebugDraw6FaceConvex.Vertices, myRenderMessageDebugDraw6FaceConvex.Color);
					}
					else
					{
						myLinesBatch2.Add6FacedConvexWorld(myRenderMessageDebugDraw6FaceConvex.Vertices, myRenderMessageDebugDraw6FaceConvex.Color);
					}
					break;
				}
				case MyRenderMessageEnum.DebugDrawCone:
				{
					MyRenderMessageDebugDrawCone myRenderMessageDebugDrawCone = (MyRenderMessageDebugDrawCone)message;
					MyLinesBatch myLinesBatch6 = (myRenderMessageDebugDrawCone.DepthRead ? myLinesBatch : myLinesBatch2);
					Vector3D directionVector = myRenderMessageDebugDrawCone.DirectionVector;
					directionVector.Normalize();
					Vector3D vector3D10 = myRenderMessageDebugDrawCone.Translation + myRenderMessageDebugDrawCone.DirectionVector;
					int num9 = 32;
					float num10 = (float)(Math.PI * 2.0 / (double)num9);
					for (int l = 0; l < 32; l++)
					{
						float num11 = (float)l * num10;
						float num12 = (float)(l + 1) * num10;
						Vector3D vector3D11 = myRenderMessageDebugDrawCone.Translation + Vector3D.Transform(myRenderMessageDebugDrawCone.BaseVector, MatrixD.CreateFromAxisAngle(directionVector, num11)) - Environment.Matrices.CameraPosition;
						Vector3D vector3D12 = myRenderMessageDebugDrawCone.Translation + Vector3D.Transform(myRenderMessageDebugDrawCone.BaseVector, MatrixD.CreateFromAxisAngle(directionVector, num12)) - Environment.Matrices.CameraPosition;
<<<<<<< HEAD
						myLinesBatch6.Add((Vector3)vector3D11, (Vector3)vector3D12, myRenderMessageDebugDrawCone.Color);
						myLinesBatch6.Add((Vector3)vector3D11, (Vector3)vector3D10, myRenderMessageDebugDrawCone.Color);
=======
						myLinesBatch6.Add(vector3D11, vector3D12, myRenderMessageDebugDrawCone.Color);
						myLinesBatch6.Add(vector3D11, vector3D10, myRenderMessageDebugDrawCone.Color);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					break;
				}
				case MyRenderMessageEnum.DebugDrawAxis:
				{
					MyRenderMessageDebugDrawAxis myRenderMessageDebugDrawAxis = (MyRenderMessageDebugDrawAxis)message;
					MyLinesBatch myLinesBatch4 = (myRenderMessageDebugDrawAxis.DepthRead ? myLinesBatch : myLinesBatch2);
					Vector3 vector = myRenderMessageDebugDrawAxis.Matrix.Translation - Environment.Matrices.CameraPosition;
					if (myRenderMessageDebugDrawAxis.SkipScale)
					{
						myLinesBatch4.Add(vector, vector + Vector3.Normalize(myRenderMessageDebugDrawAxis.Matrix.Right) * myRenderMessageDebugDrawAxis.AxisLength, myRenderMessageDebugDrawAxis.CustomColor.HasValue ? myRenderMessageDebugDrawAxis.CustomColor.Value : Color.Red);
						myLinesBatch4.Add(vector, vector + Vector3.Normalize(myRenderMessageDebugDrawAxis.Matrix.Up) * myRenderMessageDebugDrawAxis.AxisLength, myRenderMessageDebugDrawAxis.CustomColor.HasValue ? myRenderMessageDebugDrawAxis.CustomColor.Value : Color.Green);
						myLinesBatch4.Add(vector, vector + Vector3.Normalize(myRenderMessageDebugDrawAxis.Matrix.Forward) * myRenderMessageDebugDrawAxis.AxisLength, myRenderMessageDebugDrawAxis.CustomColor.HasValue ? myRenderMessageDebugDrawAxis.CustomColor.Value : Color.Blue);
					}
					else
					{
<<<<<<< HEAD
						myLinesBatch4.Add(vector, (Vector3)(vector + myRenderMessageDebugDrawAxis.Matrix.Right * myRenderMessageDebugDrawAxis.AxisLength), myRenderMessageDebugDrawAxis.CustomColor.HasValue ? myRenderMessageDebugDrawAxis.CustomColor.Value : Color.Red);
						myLinesBatch4.Add(vector, (Vector3)(vector + myRenderMessageDebugDrawAxis.Matrix.Up * myRenderMessageDebugDrawAxis.AxisLength), myRenderMessageDebugDrawAxis.CustomColor.HasValue ? myRenderMessageDebugDrawAxis.CustomColor.Value : Color.Green);
						myLinesBatch4.Add(vector, (Vector3)(vector + myRenderMessageDebugDrawAxis.Matrix.Forward * myRenderMessageDebugDrawAxis.AxisLength), myRenderMessageDebugDrawAxis.CustomColor.HasValue ? myRenderMessageDebugDrawAxis.CustomColor.Value : Color.Blue);
=======
						myLinesBatch4.Add(vector, vector + myRenderMessageDebugDrawAxis.Matrix.Right * myRenderMessageDebugDrawAxis.AxisLength, myRenderMessageDebugDrawAxis.CustomColor.HasValue ? myRenderMessageDebugDrawAxis.CustomColor.Value : Color.Red);
						myLinesBatch4.Add(vector, vector + myRenderMessageDebugDrawAxis.Matrix.Up * myRenderMessageDebugDrawAxis.AxisLength, myRenderMessageDebugDrawAxis.CustomColor.HasValue ? myRenderMessageDebugDrawAxis.CustomColor.Value : Color.Green);
						myLinesBatch4.Add(vector, vector + myRenderMessageDebugDrawAxis.Matrix.Forward * myRenderMessageDebugDrawAxis.AxisLength, myRenderMessageDebugDrawAxis.CustomColor.HasValue ? myRenderMessageDebugDrawAxis.CustomColor.Value : Color.Blue);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					break;
				}
				case MyRenderMessageEnum.DebugDrawOBB:
				{
					MyRenderMessageDebugDrawOBB myRenderMessageDebugDrawOBB = (MyRenderMessageDebugDrawOBB)message;
					Vector3D[] array = new Vector3D[8];
					new MyOrientedBoundingBoxD(myRenderMessageDebugDrawOBB.Matrix).GetCorners(array, 0);
					Vector3[] array2 = new Vector3[8];
					for (int k = 0; k < 8; k++)
					{
						array2[k] = array[k] - Environment.Matrices.CameraPosition;
					}
					if (myRenderMessageDebugDrawOBB.DepthRead)
					{
						myLinesBatch.Add6FacedConvex(array2, myRenderMessageDebugDrawOBB.Color);
					}
					else
					{
						myLinesBatch2.Add6FacedConvex(array2, myRenderMessageDebugDrawOBB.Color);
					}
					if (myRenderMessageDebugDrawOBB.Smooth)
					{
						MyPrimitivesRenderer.Draw6FacedConvexZ(array2, myRenderMessageDebugDrawOBB.Color, myRenderMessageDebugDrawOBB.Alpha);
					}
					break;
				}
				case MyRenderMessageEnum.DebugDrawFrustrum:
				{
					MyRenderMessageDebugDrawFrustrum myRenderMessageDebugDrawFrustrum = (MyRenderMessageDebugDrawFrustrum)message;
					Vector3D[] array3 = new Vector3D[8];
					myRenderMessageDebugDrawFrustrum.Frustum.GetCorners(array3);
					Vector3[] array4 = new Vector3[8];
					for (int n = 0; n < 8; n++)
					{
						array4[n] = array3[n] - Environment.Matrices.CameraPosition;
					}
					if (myRenderMessageDebugDrawFrustrum.DepthRead)
					{
						myLinesBatch.Add6FacedConvex(array4, myRenderMessageDebugDrawFrustrum.Color);
					}
					else
					{
						myLinesBatch2.Add6FacedConvex(array4, myRenderMessageDebugDrawFrustrum.Color);
					}
					MyPrimitivesRenderer.Draw6FacedConvexZ(array4, myRenderMessageDebugDrawFrustrum.Color, myRenderMessageDebugDrawFrustrum.Alpha);
					break;
				}
				case MyRenderMessageEnum.DebugDrawCylinder:
				{
					MyRenderMessageDebugDrawCylinder myRenderMessageDebugDrawCylinder = (MyRenderMessageDebugDrawCylinder)message;
					MyLinesBatch myLinesBatch5 = (myRenderMessageDebugDrawCylinder.DepthRead ? myLinesBatch : myLinesBatch2);
					int num5 = 32;
					float num6 = (float)(Math.PI * 2.0 / (double)num5);
					for (int j = 0; j < 32; j++)
					{
						float num7 = (float)j * num6;
						float num8 = (float)(j + 1) * num6;
						Vector3D vector3D8 = new Vector3D(Math.Cos(num7), 1.0, Math.Sin(num7)) * 0.5;
						Vector3D vector3D9 = new Vector3D(Math.Cos(num8), 1.0, Math.Sin(num8)) * 0.5;
						Vector3D position = vector3D8 - Vector3D.UnitY;
						Vector3D position2 = vector3D9 - Vector3D.UnitY;
						vector3D8 = Vector3D.Transform(vector3D8, myRenderMessageDebugDrawCylinder.Matrix);
						vector3D9 = Vector3D.Transform(vector3D9, myRenderMessageDebugDrawCylinder.Matrix);
						position = Vector3D.Transform(position, myRenderMessageDebugDrawCylinder.Matrix);
						position2 = Vector3D.Transform(position2, myRenderMessageDebugDrawCylinder.Matrix);
						vector3D8 -= Environment.Matrices.CameraPosition;
						vector3D9 -= Environment.Matrices.CameraPosition;
						position -= Environment.Matrices.CameraPosition;
						position2 -= Environment.Matrices.CameraPosition;
<<<<<<< HEAD
						myLinesBatch5.Add((Vector3)vector3D8, (Vector3)vector3D9, myRenderMessageDebugDrawCylinder.Color);
						myLinesBatch5.Add((Vector3)vector3D8, (Vector3)position, myRenderMessageDebugDrawCylinder.Color);
						myLinesBatch5.Add((Vector3)position, (Vector3)position2, myRenderMessageDebugDrawCylinder.Color);
=======
						myLinesBatch5.Add(vector3D8, vector3D9, myRenderMessageDebugDrawCylinder.Color);
						myLinesBatch5.Add(vector3D8, position, myRenderMessageDebugDrawCylinder.Color);
						myLinesBatch5.Add(position, position2, myRenderMessageDebugDrawCylinder.Color);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					break;
				}
				case MyRenderMessageEnum.DebugDrawTriangle:
				{
					MyRenderMessageDebugDrawTriangle myRenderMessageDebugDrawTriangle = (MyRenderMessageDebugDrawTriangle)message;
					MyPrimitivesRenderer.DrawTriangle(myRenderMessageDebugDrawTriangle.Vertex0 - Environment.Matrices.CameraPosition, myRenderMessageDebugDrawTriangle.Vertex1 - Environment.Matrices.CameraPosition, myRenderMessageDebugDrawTriangle.Vertex2 - Environment.Matrices.CameraPosition, myRenderMessageDebugDrawTriangle.Color);
					break;
				}
				case MyRenderMessageEnum.DebugDrawTriangles:
				{
					MyRenderMessageDebugDrawTriangles myRenderMessageDebugDrawTriangles = (MyRenderMessageDebugDrawTriangles)message;
					for (int i = 0; i < myRenderMessageDebugDrawTriangles.Indices.Count; i += 3)
					{
						Vector3D vector3D6 = Vector3D.Transform(myRenderMessageDebugDrawTriangles.Vertices[myRenderMessageDebugDrawTriangles.Indices[i]].Position, myRenderMessageDebugDrawTriangles.WorldMatrix) - Environment.Matrices.CameraPosition;
						Vector3D vector3D7 = Vector3D.Transform(myRenderMessageDebugDrawTriangles.Vertices[myRenderMessageDebugDrawTriangles.Indices[i + 1]].Position, myRenderMessageDebugDrawTriangles.WorldMatrix) - Environment.Matrices.CameraPosition;
						MyPrimitivesRenderer.DrawTriangle(v2: Vector3D.Transform(myRenderMessageDebugDrawTriangles.Vertices[myRenderMessageDebugDrawTriangles.Indices[i + 2]].Position, myRenderMessageDebugDrawTriangles.WorldMatrix) - Environment.Matrices.CameraPosition, v0: vector3D6, v1: vector3D7, color: myRenderMessageDebugDrawTriangles.Color);
					}
					break;
				}
				case MyRenderMessageEnum.DebugDrawMesh:
					MyPrimitivesRenderer.DebugMesh(message as MyRenderMessageDebugDrawMesh);
					break;
				case MyRenderMessageEnum.DebugDrawCapsule:
				{
					MyRenderMessageDebugDrawCapsule myRenderMessageDebugDrawCapsule = (MyRenderMessageDebugDrawCapsule)message;
					MyLinesBatch obj = (myRenderMessageDebugDrawCapsule.DepthRead ? myLinesBatch : myLinesBatch2);
					obj.AddSphereRing(new BoundingSphere(myRenderMessageDebugDrawCapsule.P0 - Environment.Matrices.CameraPosition, myRenderMessageDebugDrawCapsule.Radius), myRenderMessageDebugDrawCapsule.Color, Matrix.Identity);
					obj.AddSphereRing(new BoundingSphere(myRenderMessageDebugDrawCapsule.P0 - Environment.Matrices.CameraPosition, myRenderMessageDebugDrawCapsule.Radius), myRenderMessageDebugDrawCapsule.Color, Matrix.CreateRotationX((float)Math.E * 449f / 777f));
					obj.AddSphereRing(new BoundingSphere(myRenderMessageDebugDrawCapsule.P1 - Environment.Matrices.CameraPosition, myRenderMessageDebugDrawCapsule.Radius), myRenderMessageDebugDrawCapsule.Color, Matrix.Identity);
					obj.AddSphereRing(new BoundingSphere(myRenderMessageDebugDrawCapsule.P1 - Environment.Matrices.CameraPosition, myRenderMessageDebugDrawCapsule.Radius), myRenderMessageDebugDrawCapsule.Color, Matrix.CreateRotationX((float)Math.E * 449f / 777f));
<<<<<<< HEAD
					obj.Add((Vector3)(myRenderMessageDebugDrawCapsule.P0 - Environment.Matrices.CameraPosition), (Vector3)(myRenderMessageDebugDrawCapsule.P1 - Environment.Matrices.CameraPosition), myRenderMessageDebugDrawCapsule.Color);
=======
					obj.Add(myRenderMessageDebugDrawCapsule.P0 - Environment.Matrices.CameraPosition, myRenderMessageDebugDrawCapsule.P1 - Environment.Matrices.CameraPosition, myRenderMessageDebugDrawCapsule.Color);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					break;
				}
				case MyRenderMessageEnum.DebugDrawText2D:
				{
					MyRenderMessageDebugDrawText2D myRenderMessageDebugDrawText2D = (MyRenderMessageDebugDrawText2D)message;
					StringBuilder text = new StringBuilder(myRenderMessageDebugDrawText2D.Text);
					MyDebugTextHelpers.DrawText(myRenderMessageDebugDrawText2D.Coord, text, myRenderMessageDebugDrawText2D.Color, myRenderMessageDebugDrawText2D.Scale, myRenderMessageDebugDrawText2D.Align);
					break;
				}
				case MyRenderMessageEnum.DebugDrawText3D:
				{
					MyRenderMessageDebugDrawText3D myRenderMessageDebugDrawText3D = (MyRenderMessageDebugDrawText3D)message;
					Vector3D coord = myRenderMessageDebugDrawText3D.Coord;
					MatrixD matrix = Environment.Matrices.ViewProjectionD;
					if (myRenderMessageDebugDrawText3D.CustomViewProjection != -1)
					{
						if (!MyRenderProxy.BillboardsViewProjectionRead.ContainsKey(myRenderMessageDebugDrawText3D.CustomViewProjection))
						{
							break;
						}
						int customViewProjection = myRenderMessageDebugDrawText3D.CustomViewProjection;
						float m = MyRenderProxy.BillboardsViewProjectionRead[customViewProjection].Viewport.Width / (float)ViewportResolution.X;
						float m2 = MyRenderProxy.BillboardsViewProjectionRead[customViewProjection].Viewport.Height / (float)ViewportResolution.Y;
						float m3 = MyRenderProxy.BillboardsViewProjectionRead[customViewProjection].Viewport.OffsetX / (float)ViewportResolution.X;
						float m4 = ((float)ViewportResolution.Y - MyRenderProxy.BillboardsViewProjectionRead[customViewProjection].Viewport.OffsetY - MyRenderProxy.BillboardsViewProjectionRead[customViewProjection].Viewport.Height) / (float)ViewportResolution.Y;
						Matrix matrix2 = new Matrix(m, 0f, 0f, 0f, 0f, m2, 0f, 0f, 0f, 0f, 1f, 0f, m3, m4, 0f, 1f);
						Matrix m5 = MyRenderProxy.BillboardsViewProjectionRead[myRenderMessageDebugDrawText3D.CustomViewProjection].ViewAtZero * MyRenderProxy.BillboardsViewProjectionRead[myRenderMessageDebugDrawText3D.CustomViewProjection].Projection * matrix2;
						matrix = m5;
					}
					Vector3D vector3D = Vector3D.Transform(coord, ref matrix);
					vector3D.X = vector3D.X * 0.5 + 0.5;
					vector3D.Y = vector3D.Y * -0.5 + 0.5;
					float num = (UseComplementaryDepthBuffer ? 0f : 1f);
					num = (myRenderMessageDebugDrawText3D.ClipDistance.HasValue ? Vector3.Transform(new Vector3(0f, 0f, 0f - myRenderMessageDebugDrawText3D.ClipDistance.Value), Environment.Matrices.Projection).Z : num);
					if ((!UseComplementaryDepthBuffer) ? (vector3D.Z < (double)num && vector3D.Z > 0.0) : (vector3D.Z > (double)num && vector3D.Z < 1.0))
					{
						MyDebugTextHelpers.DrawText(new Vector2((float)vector3D.X, (float)vector3D.Y) * ViewportResolution, new StringBuilder(myRenderMessageDebugDrawText3D.Text), myRenderMessageDebugDrawText3D.Color, myRenderMessageDebugDrawText3D.Scale, myRenderMessageDebugDrawText3D.Align);
					}
					break;
				}
				case MyRenderMessageEnum.DebugDrawModel:
					_ = (MyRenderMessageDebugDrawModel)message;
					break;
				case MyRenderMessageEnum.DebugDrawPlane:
					_ = (MyRenderMessageDebugDrawPlane)message;
					break;
				case MyRenderMessageEnum.DebugWaitForPresent:
				{
					MyRenderMessageDebugWaitForPresent myRenderMessageDebugWaitForPresent = (MyRenderMessageDebugWaitForPresent)message;
					MyRenderProxy.RenderThread.DebugAddWaitingForPresent(myRenderMessageDebugWaitForPresent.WaitHandle);
					break;
				}
				case MyRenderMessageEnum.DebugClearPersistentMessages:
					_ = (MyRenderMessageDebugClearPersistentMessages)message;
					result = true;
					break;
				}
			}
			myLinesBatch.Commit();
			myLinesBatch2.Commit();
			myLinesBatch3.Commit();
			return result;
		}

		[Conditional("DEBUG")]
		public static void CheckRenderThread()
		{
		}

		private static void AddDebugQueueMessage(string message)
		{
			if (DebugInfoQueue != null)
			{
				DebugInfoQueue.AddApplicationMessage(MessageSeverity.Information, message);
			}
		}

		private static void InitDebugOutput()
		{
			if (MyVRage.Platform.Render.IsRenderOutputDebugSupported)
			{
				DebugDevice = new DeviceDebug(DeviceInstance);
			}
			DebugInfoQueue = DeviceInstance.QueryInterface<SharpDX.Direct3D11.InfoQueue>();
			DebugInfoQueue.SetBreakOnSeverity(MessageSeverity.Corruption, true);
			DebugInfoQueue.SetBreakOnSeverity(MessageSeverity.Error, true);
			DebugInfoQueue.MessageCountLimit = 4096L;
			DebugInfoQueue.ClearStorageFilter();
			SharpDX.Direct3D11.InfoQueueFilter infoQueueFilter = new SharpDX.Direct3D11.InfoQueueFilter
			{
				DenyList = new SharpDX.Direct3D11.InfoQueueFilterDescription
				{
					Severities = new MessageSeverity[1]
				},
				AllowList = new SharpDX.Direct3D11.InfoQueueFilterDescription()
			};
			infoQueueFilter.DenyList.Severities[0] = MessageSeverity.Information;
			DebugInfoQueue.AddStorageFilterEntries(infoQueueFilter);
		}

		[Conditional("__RANDOM_UNDEFINED_PROFILING_SYMBOL__")]
		internal static void ProcessDebugOutput()
		{
			if (DebugInfoQueue == null || DebugInfoQueue.NumStoredMessagesAllowedByRetrievalFilter <= 0)
			{
				return;
			}
			string debugOutput;
			lock (DebugInfoQueue)
			{
				debugOutput = GetDebugOutput();
			}
			if (debugOutput.Length == 0)
			{
				return;
			}
			if (MyCompilationSymbols.DX11DebugOutput)
			{
				string[] array = debugOutput.Split(new char[1] { '\n' });
				foreach (string text in array)
				{
					if (!string.IsNullOrEmpty(text))
					{
						MyVRage.Platform.System.LogToExternalDebugger(text);
					}
				}
			}
			if (MyCompilationSymbols.DX11LogOutput)
			{
				Log.WriteLine(debugOutput);
			}
		}

		private static string GetDebugOutput()
		{
			StringBuilder debugStringBuilder = DebugStringBuilder;
			debugStringBuilder.Clear();
			long numStoredMessagesAllowedByRetrievalFilter = DebugInfoQueue.NumStoredMessagesAllowedByRetrievalFilter;
			for (long num = 0L; num < numStoredMessagesAllowedByRetrievalFilter; num++)
			{
				Message message;
				try
				{
					message = DebugInfoQueue.GetMessage(num);
				}
				catch
				{
					Message message2 = default(Message);
					message2.Description = "";
					message = message2;
				}
				debugStringBuilder.AppendLine(string.Format("D3D11 {0}: {1} [ {2} #{3}: {4} ] {5}/{6}", message.Severity, message.Description.Replace("\0", ""), message.Category, (int)message.Id, message.Id, num, DebugInfoQueue.NumStoredMessages));
			}
			if (numStoredMessagesAllowedByRetrievalFilter > 0)
			{
<<<<<<< HEAD
				debugStringBuilder.AppendLine(System.Environment.StackTrace.Replace('\n', '|'));
=======
				debugStringBuilder.AppendLine(Environment.get_StackTrace().Replace('\n', '|'));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (DebugInfoQueue.NumMessagesDiscardedByMessageCountLimit - m_lastSkippedCount > 0)
			{
				debugStringBuilder.Append("Skipped messages: ");
				debugStringBuilder.Append(DebugInfoQueue.NumMessagesDiscardedByMessageCountLimit - m_lastSkippedCount);
				m_lastSkippedCount = DebugInfoQueue.NumMessagesDiscardedByMessageCountLimit;
			}
			DebugInfoQueue.ClearStoredMessages();
			return debugStringBuilder.ToString();
		}

		internal static MyRenderDeviceSettings CreateDevice(MyRenderDeviceSettings? settingsToTry)
		{
			MyRenderExceptionEnum exceptionType;
			bool flag = CreateDeviceInternalSafe(settingsToTry, out exceptionType);
			Log.WriteLine("CreateDevice: deviceCreated = " + flag);
			if (!settingsToTry.HasValue || !settingsToTry.Value.SettingsMandatory)
			{
				if (!flag && settingsToTry.HasValue && settingsToTry.Value.UseStereoRendering)
				{
					Log.WriteLine("CreateDevice: Attempt to create stereo renderer");
					MyRenderDeviceSettings value = settingsToTry.Value;
					value.UseStereoRendering = false;
					flag = CreateDeviceInternalSafe(value, out exceptionType);
				}
				MyRenderDeviceSettings value2;
				if (!flag)
				{
					Log.WriteLine("Primary desktop size fallback.");
					MyAdapterInfo[] adaptersList = GetAdaptersList();
					for (int i = 0; i < adaptersList.Length; i++)
					{
						Rectangle desktopBounds = adaptersList[i].DesktopBounds;
						if (adaptersList[i].IsDx11Supported)
						{
							MyDisplayMode[] supportedDisplayModes = adaptersList[i].SupportedDisplayModes;
							for (int j = 0; j < supportedDisplayModes.Length; j++)
							{
								MyDisplayMode myDisplayMode = supportedDisplayModes[j];
								if (myDisplayMode.Width == desktopBounds.Width && myDisplayMode.Height == desktopBounds.Height)
								{
									value2 = default(MyRenderDeviceSettings);
									value2.AdapterOrdinal = i;
									value2.BackBufferWidth = myDisplayMode.Width;
									value2.BackBufferHeight = myDisplayMode.Height;
									value2.WindowMode = MyWindowModeEnum.FullscreenWindow;
									value2.RefreshRate = myDisplayMode.RefreshRate;
									value2.VSync = 1;
									flag = CreateDeviceInternalSafe(value2, out exceptionType);
									if (flag)
									{
										break;
									}
								}
							}
						}
						if (flag)
						{
							break;
						}
					}
				}
				if (!flag)
				{
					Log.WriteLine("Lowest res fallback.");
					MyAdapterInfo[] adaptersList2 = GetAdaptersList();
					for (int k = 0; k < adaptersList2.Length; k++)
					{
						value2 = default(MyRenderDeviceSettings);
						value2.AdapterOrdinal = k;
						value2.BackBufferWidth = 640;
						value2.BackBufferHeight = 480;
						value2.WindowMode = MyWindowModeEnum.Window;
						value2.VSync = 1;
						flag = CreateDeviceInternalSafe(value2, out exceptionType);
						if (flag)
						{
							break;
						}
					}
				}
			}
			if (!flag)
			{
				string text = $"Please apply windows updates and update to latest graphics drivers.";
				MyMessageBox.Show("Unable to initialize game", text);
				throw new MyRenderException(text, MyRenderExceptionEnum.GpuNotSupported);
			}
			return m_settings;
		}

		[HandleProcessCorruptedStateExceptions]
		[SecurityCritical]
		private static bool CreateDeviceInternalSafe(MyRenderDeviceSettings? settings, out MyRenderExceptionEnum exceptionType)
		{
			exceptionType = MyRenderExceptionEnum.Unassigned;
			bool flag = false;
			try
			{
				CreateDeviceInternal(settings);
				flag = true;
			}
			catch (MyRenderException ex)
			{
				Log.WriteLine("CreateDevice failed: MyRenderException occurred");
				Log.IncreaseIndent();
				Log.WriteLine(ex);
				Log.DecreaseIndent();
				exceptionType = ex.Type;
			}
			catch (Exception ex2)
			{
				Log.WriteLine("CreateDevice failed: Regular exception occurred");
				Log.IncreaseIndent();
				Log.WriteLine(ex2);
				Log.DecreaseIndent();
			}
			if (!flag)
			{
				Log.WriteLine("CreateDevice failed: Disposing Device");
				DisposeDevice();
				return false;
			}
			return true;
		}

		private static void CreateDeviceInternal(MyRenderDeviceSettings? settings)
		{
<<<<<<< HEAD
			RenderThread = Thread.CurrentThread;
=======
			RenderThread = Thread.get_CurrentThread();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			DisposeDevice();
			Log.WriteLine("CreateDeviceInternal");
			MyVRage.Platform.Render.CreateRenderDevice(ref settings, out var deviceInstance, out var swapChain);
			m_settings = settings.Value;
			LogSettings(ref m_settings);
			DeviceInstance = deviceInstance as SharpDX.Direct3D11.Device1;
			m_swapchain = swapChain as SwapChain;
			MyAdapterInfo myAdapterInfo = GetAdaptersList()[m_settings.AdapterOrdinal];
			ParallelVertexBufferMapping = myAdapterInfo.ParallelVertexBufferMapping;
			BatchedConstantBufferMapping = myAdapterInfo.BatchedConstantBufferMapping;
			Log.WriteLine("CreateDeviceInternal InitDebugOutput");
			Log.WriteLine("CreateDeviceInternal RC Dispose");
			if (RC != null)
			{
				RC.Dispose();
			}
			Log.WriteLine("CreateDeviceInternal RC Create");
			RC = new MyRenderContext();
			Log.WriteLine("CreateDeviceInternal RC Initialize");
			RC.Initialize(DeviceInstance.ImmediateContext1);
			m_resolution = new Vector2I(m_settings.BackBufferWidth, m_settings.BackBufferHeight);
			Log.WriteLine("CreateDeviceInternal m_initializedOnce (" + m_initializedOnce + ")");
			if (!m_initializedOnce)
			{
				InitSubsystemsOnce();
				m_initializedOnce = true;
			}
			Log.WriteLine("CreateDeviceInternal m_initialized (" + m_initialized + ")");
			if (!m_initialized)
			{
				InitSubsystems(m_settings.InitParallel);
				m_initialized = true;
			}
			Log.WriteLine("CreateDeviceInteral Apply Settings");
			settings = m_settings;
			m_settings.WindowMode = MyWindowModeEnum.Window;
			ApplySettings(settings.Value);
			Log.WriteLine(string.Concat("CreateDeviceInteral done (", m_settings, ")"));
		}

		internal static void DisposeDevice()
		{
			ForceWindowed();
			RemoveScreenResources();
			OnDeviceEnd();
			if (Backbuffer != null)
			{
				Backbuffer.Release();
				Backbuffer = null;
			}
			if (RC != null)
			{
				RC.Dispose();
				RC = null;
			}
			m_initialized = false;
			MyVRage.Platform.Render.DisposeRenderDevice();
			if (DebugDevice != null)
			{
				DebugDevice.ReportLiveDeviceObjects(ReportingLevel.Summary | ReportingLevel.Detail);
				DebugDevice.Dispose();
			}
		}

		internal static long GetAvailableTextureMemory()
		{
			if (m_settings.AdapterOrdinal == -1)
			{
				return 0L;
			}
			return (long)GetAdaptersList()[m_settings.AdapterOrdinal].VRAM;
		}

		private static MyAdapterInfo GetCurrentAdapter()
		{
			return GetAdaptersList()[m_settings.AdapterOrdinal];
		}

		private static bool IsDeferStateChangesTaskPending()
		{
			if (m_processTask.valid)
			{
				return !m_processTask.IsComplete;
			}
			return false;
		}

		internal static void Draw(bool draw = true)
		{
			if (m_deferStateChanges || IsDeferStateChangesTaskPending())
			{
				m_deferredStateChanges = true;
				SimpleDraw();
				if (!IsDeferStateChangesTaskPending())
				{
					ProcessUpdates();
					MyRenderVoxelActor.UpdateQueued();
					MyManagers.OnFrameEnd();
					MyManagers.OnUpdate();
					m_processTask.WaitOrExecute();
					m_processTask = Parallel.Start(ProcessStateChanges, WorkPriority.VeryHigh);
				}
			}
			else
			{
				if (m_processTask.valid)
				{
					m_processTask.WaitOrExecute();
					m_processTask = default(Task);
				}
				m_deferredStateChanges = false;
				FullDraw(draw);
			}
		}

		public static void EnqueueUpdate(Action a)
		{
			m_deferredUpdate.Enqueue(a);
		}

		private static void ProcessUpdates()
		{
<<<<<<< HEAD
			Action result;
			while (m_deferredUpdate.TryDequeue(out result))
			{
				result.InvokeIfNotNull();
=======
			Action handler = default(Action);
			while (m_deferredUpdate.TryDequeue(ref handler))
			{
				handler.InvokeIfNotNull();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private static void SimpleDraw(bool draw = true)
		{
			MyCommon.UpdateTimers();
			ProcessMessageQueue();
			RenderMainSprites();
			RC.ClearRtv(Backbuffer, default(RawColor4));
			ConsumeMainSprites();
			MyManagers.SpritesManager.FrameEnd();
			RC.CleanUpCommandLists();
			MyLinesRenderer.Clear();
		}

		private static void ProcessStateChanges()
		{
<<<<<<< HEAD
			MyRenderProxy.RenderThread.ProcessStateChangesThread = Thread.CurrentThread;
=======
			MyRenderProxy.RenderThread.ProcessStateChangesThread = Thread.get_CurrentThread();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			DeferStateChangeBatch = MyMeshes.OpenMeshUpdateBatch();
			ProcessMessageQueue(draw: false);
			Parallel.RunCallbacks();
			UpdateGameScene();
			EnqueueUpdate(ProcessStateChangesCommit);
			MyRenderProxy.RenderThread.ProcessStateChangesThread = null;
		}

		private static void ProcessStateChangesCommit()
		{
			DeferStateChangeBatch?.Commit(allowEmpty: true);
			DeferStateChangeBatch = null;
		}

		private static void FullDraw(bool draw = true)
		{
			m_drawScene = false;
			FrameProcessStatus = FrameProcessStatusEnum.NoProcess;
			m_mainSprites.Clear();
			MyCommon.UpdateTimers();
<<<<<<< HEAD
			MyGpuProfiler.IC_BeginBlockAlways("GPUFrame", "FullDraw", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyGpuProfiler.IC_BeginBlock("ProcessMessageQueue", "FullDraw", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			ProcessMessageQueue();
			MyGpuProfiler.IC_EndBlock(0f, "FullDraw", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
=======
			MyGpuProfiler.IC_BeginBlockAlways("GPUFrame", "FullDraw", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyGpuProfiler.IC_BeginBlock("ProcessMessageQueue", "FullDraw", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			ProcessMessageQueue();
			MyGpuProfiler.IC_EndBlock(0f, "FullDraw", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			MyRenderVoxelActor.UpdateQueued();
			ProcessUpdates();
			Parallel.RunCallbacks();
			if (draw)
			{
				if (MyRenderProxy.DrawRenderStats != 0)
				{
					MyRendererStats.UpdateStats();
				}
				RenderMainSprites();
				if (!MyManagers.Ansel.IsCaptureRunning && SceneSystemsReady)
				{
<<<<<<< HEAD
					MyGpuProfiler.IC_BeginBlock("UpdateGameScene", "FullDraw", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
					UpdateGameScene();
					MyGpuProfiler.IC_EndBlock(0f, "FullDraw", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
=======
					MyGpuProfiler.IC_BeginBlock("UpdateGameScene", "FullDraw", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
					UpdateGameScene();
					MyGpuProfiler.IC_EndBlock(0f, "FullDraw", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				else
				{
					MyTimeSpan currentTime = new MyTimeSpan(Stopwatch.GetTimestamp());
					MyScene11.Instance.Updater.UpdateDelayedCalls(currentTime);
				}
				if (m_drawScene && SceneSystemsReady)
				{
					DrawScene();
				}
				else
				{
					RC.ClearRtv(Backbuffer, default(RawColor4));
				}
				if (m_screenshot.HasValue && m_screenshot.Value.IgnoreSprites)
				{
					TakeCustomSizedScreenshot(m_screenshot.Value.SizeMult, m_screenshot.Value.IgnoreSprites);
					m_screenshot = null;
				}
				ConsumeMainSprites();
				if (m_screenshot.HasValue && !m_screenshot.Value.IgnoreSprites)
				{
					TakeCustomSizedScreenshot(m_screenshot.Value.SizeMult, m_screenshot.Value.IgnoreSprites);
					m_screenshot = null;
				}
				MyManagers.SpritesManager.FrameEnd();
				if (m_texturesToRender.Count > 0)
				{
					MySaveExportedTextures.RenderColoredTextures(m_texturesToRender);
				}
			}
			RC.CleanUpCommandLists();
			MyLinesRenderer.Clear();
			LogMemoryForImprovedGFX();
<<<<<<< HEAD
			MyGpuProfiler.IC_EndBlockAlways(0f, "FullDraw", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
=======
			MyGpuProfiler.IC_EndBlockAlways(0f, "FullDraw", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static void DrawScene()
		{
			AddDebugQueueMessage("Frame render start");
<<<<<<< HEAD
			MyGpuProfiler.IC_BeginBlock("DrawGameScene", "DrawScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			DrawGameScene(Backbuffer, out var debugAmbientOcclusion);
			MyGpuProfiler.IC_EndBlock(0f, "DrawScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyGpuProfiler.IC_BeginBlock("DrawDebugScene", "DrawScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			DrawDebugScene(debugAmbientOcclusion);
			MyGpuProfiler.IC_EndBlock(0f, "DrawScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
=======
			MyGpuProfiler.IC_BeginBlock("DrawGameScene", "DrawScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			DrawGameScene(Backbuffer, out var debugAmbientOcclusion);
			MyGpuProfiler.IC_EndBlock(0f, "DrawScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyGpuProfiler.IC_BeginBlock("DrawDebugScene", "DrawScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			DrawDebugScene(debugAmbientOcclusion);
			MyGpuProfiler.IC_EndBlock(0f, "DrawScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static void DrawDebugScene(IBorrowedRtvTexture debugAmbientOcclusion)
		{
<<<<<<< HEAD
			MyGpuProfiler.IC_BeginBlock("Draw scene debug", "DrawDebugScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			DrawSceneDebug();
			ProcessDebugMessages();
			MyGpuProfiler.IC_EndBlock(0f, "DrawDebugScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyGpuProfiler.IC_BeginBlock("MyDebugRenderer.Draw", "DrawDebugScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyDebugRenderer.Draw(Backbuffer, debugAmbientOcclusion);
			MyGpuProfiler.IC_EndBlock(0f, "DrawDebugScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyGpuProfiler.IC_BeginBlock("MyDebugTextureDisplay.Draw", "DrawDebugScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyDebugTextureDisplay.Draw(Backbuffer);
			MyGpuProfiler.IC_EndBlock(0f, "DrawDebugScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			debugAmbientOcclusion.Release();
			MyGpuProfiler.IC_BeginBlock("MyPrimitivesRenderer.Draw", "DrawDebugScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyPrimitivesRenderer.Draw(Backbuffer, ref Environment.Matrices.ViewProjectionAt0, useDepth: true);
			MyGpuProfiler.IC_EndBlock(0f, "DrawDebugScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyGpuProfiler.IC_BeginBlock("MyLinesRenderer.Draw", "DrawDebugScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyLinesRenderer.Draw(Backbuffer);
			MyGpuProfiler.IC_EndBlock(0f, "DrawDebugScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
=======
			MyGpuProfiler.IC_BeginBlock("Draw scene debug", "DrawDebugScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			DrawSceneDebug();
			ProcessDebugMessages();
			MyGpuProfiler.IC_EndBlock(0f, "DrawDebugScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyGpuProfiler.IC_BeginBlock("MyDebugRenderer.Draw", "DrawDebugScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyDebugRenderer.Draw(Backbuffer, debugAmbientOcclusion);
			MyGpuProfiler.IC_EndBlock(0f, "DrawDebugScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyGpuProfiler.IC_BeginBlock("MyDebugTextureDisplay.Draw", "DrawDebugScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyDebugTextureDisplay.Draw(Backbuffer);
			MyGpuProfiler.IC_EndBlock(0f, "DrawDebugScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			debugAmbientOcclusion.Release();
			MyGpuProfiler.IC_BeginBlock("MyPrimitivesRenderer.Draw", "DrawDebugScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyPrimitivesRenderer.Draw(Backbuffer, ref Environment.Matrices.ViewProjectionAt0, useDepth: true);
			MyGpuProfiler.IC_EndBlock(0f, "DrawDebugScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyGpuProfiler.IC_BeginBlock("MyLinesRenderer.Draw", "DrawDebugScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
			MyLinesRenderer.Draw(Backbuffer);
			MyGpuProfiler.IC_EndBlock(0f, "DrawDebugScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			AddDebugQueueMessage("Frame render end");
		}

		internal static void ConsumeMainSprites()
		{
			if (m_mainSpritesTask.valid)
			{
<<<<<<< HEAD
				MyGpuProfiler.IC_BeginBlock("ConsumeMainSprites", "ConsumeMainSprites", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
				m_mainSpritesTask.Wait(blocking: true);
				RC.ExecuteContext(ref m_mainSpritesFC, "ConsumeMainSprites", 312, "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
				RC.ClearState();
				m_mainSpritesTask.valid = false;
				MyGpuProfiler.IC_EndBlock(0f, "ConsumeMainSprites", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
=======
				MyGpuProfiler.IC_BeginBlock("ConsumeMainSprites", "ConsumeMainSprites", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
				m_mainSpritesTask.Wait(blocking: true);
				RC.ExecuteContext(ref m_mainSpritesFC, "ConsumeMainSprites", 312, "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
				RC.ClearState();
				m_mainSpritesTask.valid = false;
				MyGpuProfiler.IC_EndBlock(0f, "ConsumeMainSprites", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public static void SetMainViewportScale(float scaleFactor)
		{
			m_mainViewportScaleFactor = scaleFactor;
		}

		public static MyViewport ScaleMainViewport(MyViewport viewport)
		{
			return new MyViewport(((float)ViewportResolution.X - (float)ViewportResolution.X * m_mainViewportScaleFactor) / 2f, ((float)ViewportResolution.Y - (float)ViewportResolution.Y * m_mainViewportScaleFactor) / 2f, (float)ViewportResolution.X * m_mainViewportScaleFactor, (float)ViewportResolution.Y * m_mainViewportScaleFactor);
		}

		private static void RenderMainSprites()
		{
			MyViewport myViewport = new MyViewport(ViewportResolution.X, ViewportResolution.Y);
			Vector2 size = ViewportResolution;
			RenderMainSprites(Backbuffer, ScaleMainViewport(myViewport), myViewport, size);
		}

		internal static void RenderMainSprites(IRtvBindable rtv, MyViewport viewportBound, MyViewport viewportFull, Vector2 size, MyViewport? targetRegion = null)
		{
			if (!Settings.OffscreenSpritesRendering || !m_drawScene)
			{
				UpdateRenderStats();
				MySpriteMessageData defaultMessages = MyManagers.SpritesManager.AcquireDrawMessages("DefaultOffscreenTarget");
				MySpriteMessageData debugMessages = MyManagers.SpritesManager.CloseDebugDrawMessages();
				m_mainSpritesTask = Parallel.Start(delegate
				{
					RenderMainSpritesWorker(rtv, viewportBound, viewportFull, size, defaultMessages, debugMessages, targetRegion);
				}, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.GUI, "RenderMainSprites"), WorkPriority.VeryHigh);
			}
		}

		private static void RenderMainSpritesWorker(IRtvBindable rtv, MyViewport viewportBound, MyViewport viewportFull, Vector2 size, MySpriteMessageData defaultMessages, MySpriteMessageData debugMessages, MyViewport? targetRegion = null)
		{
			try
			{
				MyRenderContext myRenderContext = MyManagers.DeferredRCs.AcquireRC("UpdateAndRenderMainSprites");
				if (defaultMessages != null)
				{
					m_mainSprites.ProcessDrawSpritesQueue(defaultMessages, touchTextures: true);
					MyManagers.SpritesManager.DisposeDrawMessages(defaultMessages);
				}
				if (debugMessages != null)
				{
					m_mainSprites.ProcessDrawSpritesQueue(debugMessages, touchTextures: true);
					MyManagers.SpritesManager.DisposeDrawMessages(debugMessages);
				}
				m_mainSprites.Draw(myRenderContext, rtv, ref viewportBound, ref viewportFull, ref size, targetRegion);
				m_mainSpritesFC = myRenderContext.FinishDeferredContext();
			}
			catch (Exception ex)
			{
				Log.WriteLine(ex);
				throw ex;
			}
		}

		public static IBorrowedRtvTexture DrawSpritesOffscreen(MySpriteMessageData messages, string textureName, int width, int height, Format format = Format.B8G8R8A8_UNorm, RawColor4? clearColor = null, IBlendState blendState = null)
		{
			if (string.IsNullOrEmpty(textureName))
			{
				textureName = "DefaultOffscreenTarget";
			}
			if (width == -1)
			{
				width = ViewportResolution.X;
			}
			if (height == -1)
			{
				height = ViewportResolution.Y;
			}
			IBorrowedRtvTexture borrowedRtvTexture = MyManagers.RwTexturesPool.BorrowRtv(textureName, width, height, format);
			MyImmediateRC.RC.ClearRtv(borrowedRtvTexture, clearColor ?? default(RawColor4));
			MySpritesRenderer spritesRenderer = MyManagers.SpritesManager.GetSpritesRenderer();
			if (!spritesRenderer.ProcessDrawSpritesQueue(messages, touchTextures: true))
			{
				MyManagers.SpritesManager.Return(spritesRenderer);
				return borrowedRtvTexture;
			}
			MyViewport viewportRtvBound = new MyViewport(width, height);
			Vector2 viewportSizeWrittenIntoShaders = new Vector2(width, height);
			spritesRenderer.Draw(RC, borrowedRtvTexture, ref viewportRtvBound, ref viewportRtvBound, ref viewportSizeWrittenIntoShaders, null, blendState);
			MyManagers.SpritesManager.Return(spritesRenderer);
			return borrowedRtvTexture;
		}

		public static bool DrawSpritesOffscreen(MyRenderContext rc, IRtvBindable texture, MySpriteMessageData messages, ref Vector2 aspectRatio, RawColor4? clearColor = null, IBlendState blendState = null)
		{
			rc.ClearRtv(texture, clearColor ?? default(RawColor4));
			MySpritesRenderer spritesRenderer = MyManagers.SpritesManager.GetSpritesRenderer();
			if (!spritesRenderer.ProcessDrawSpritesQueue(messages, touchTextures: false))
			{
				MyManagers.SpritesManager.Return(spritesRenderer);
				return false;
			}
			MyViewport viewportRtvBound = new MyViewport(texture.Size.X, texture.Size.Y);
			Vector2 viewportSizeWrittenIntoShaders = texture.Size * aspectRatio;
			spritesRenderer.Draw(rc, texture, ref viewportRtvBound, ref viewportRtvBound, ref viewportSizeWrittenIntoShaders, null, blendState);
			MyManagers.SpritesManager.Return(spritesRenderer);
			return true;
		}

		private static void UpdateRenderStats()
		{
			if (MyRenderProxy.DrawRenderStats == MyRenderProxy.MyStatsState.NoDraw)
			{
				return;
			}
<<<<<<< HEAD
			MyGpuProfiler.IC_BeginBlock("MyRenderStatsDraw.Draw", "UpdateRenderStats", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
=======
			MyGpuProfiler.IC_BeginBlock("MyRenderStatsDraw.Draw", "UpdateRenderStats", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			switch (MyRenderProxy.DrawRenderStats)
			{
			case MyRenderProxy.MyStatsState.SimpleTimingStats:
				MyRenderStatsDraw.Draw(MyRenderStats.m_stats, 0.6f, Color.Yellow);
				break;
			case MyRenderProxy.MyStatsState.ComplexTimingStats:
				MyRenderStatsDraw.Draw(MyRenderStats.m_stats, 0.6f, Color.Yellow);
				break;
			case MyRenderProxy.MyStatsState.Draw:
				MyStatsDisplay.Draw();
				break;
			case MyRenderProxy.MyStatsState.MoveNext:
				MyRenderProxy.DrawRenderStats = MyRenderProxy.MyStatsState.Draw;
				if (!MyStatsDisplay.MoveToNextPage())
				{
					MyRenderProxy.DrawRenderStats = MyRenderProxy.MyStatsState.Last;
				}
				break;
			}
<<<<<<< HEAD
			MyGpuProfiler.IC_EndBlock(0f, "UpdateRenderStats", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
=======
			MyGpuProfiler.IC_EndBlock(0f, "UpdateRenderStats", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-Draw.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static void LogMemoryForImprovedGFX()
		{
			long num = 0L;
			num += MyManagers.FoliageManager.FoliagePool.GetReport().TotalBytesAllocated;
			MyVRage.Platform.Render.SetMemoryUsedForImprovedGFX(num);
		}

		private static void ResetShadows(MyRenderContext rc, int cascadeCount, int cascadeResolution)
		{
			if (MyManagers.Shadows.CascadeCount == cascadeCount && MyManagers.Shadows.CascadeResolution == cascadeResolution)
			{
				return;
			}
			MyManagers.Shadows.Reset(rc, cascadeCount, cascadeResolution);
			MyRenderableComponent.MarkAllDirty();
			foreach (MyLightComponent item in MyComponentFactory<MyLightComponent>.GetAll())
			{
				item.Owner.SetWorldMatrixDirty();
			}
		}

		public static void SetupCameraMatrices(MyRenderMessageSetCameraViewMatrix message)
		{
			MyManagers.Ansel.SetCurrentCamera(message.ViewMatrix, message.FOV, ResolutionF.X / ResolutionF.Y, message.NearPlane, message.FarPlane, message.FarFarPlane, message.CameraPosition);
			MyManagers.Ansel.SetUpdateTime(message.UpdateTime);
			SetupCameraMatricesInternal(message, Environment.Matrices, MyStereoRegion.FULLSCREEN);
			if (MyStereoRender.Enable)
			{
				SetupCameraMatricesInternal(message, MyStereoRender.EnvMatricesLeftEye, MyStereoRegion.LEFT);
				SetupCameraMatricesInternal(message, MyStereoRender.EnvMatricesRightEye, MyStereoRegion.RIGHT);
			}
		}

		private static Matrix GetMatrixEyeTranslation(bool isLeftEye, Matrix view)
		{
			float num = 0.2f;
			Matrix matrix = Matrix.Transpose(view);
			return Matrix.CreateTranslation(((!isLeftEye) ? matrix.Left : matrix.Right) * num);
		}

		private static void SetupCameraMatricesInternal(MyRenderMessageSetCameraViewMatrix message, MyEnvironmentMatrices envMatrices, MyStereoRegion typeofEnv)
		{
			Matrix projectionMatrix = message.ProjectionMatrix;
			Matrix projectionFarMatrix = message.ProjectionFarMatrix;
			MatrixD viewMatrix = message.ViewMatrix;
			Vector3D cameraPosition = message.CameraPosition;
			MatrixD m = viewMatrix;
			m.M14 = 0.0;
			m.M24 = 0.0;
			m.M34 = 0.0;
			m.M41 = 0.0;
			m.M42 = 0.0;
			m.M43 = 0.0;
			m.M44 = 1.0;
			float num = ResolutionF.X / ResolutionF.Y;
			if (typeofEnv != 0)
			{
				num /= 2f;
			}
			Matrix m2 = ((message.FOV > 0f) ? Matrix.CreatePerspectiveFovRhInfiniteComplementary(message.FOV, num, message.NearPlane) : message.ProjectionMatrix);
			m2.M31 = message.ProjectionOffsetX;
			m2.M32 = message.ProjectionOffsetY;
			Matrix projectionForSkybox = ((message.FOV > 0f) ? Matrix.CreatePerspectiveFovRhInfiniteComplementary(message.FOVForSkybox, num, message.NearPlane) : message.ProjectionFarMatrix);
			projectionForSkybox.M31 = message.ProjectionOffsetX;
			projectionForSkybox.M32 = message.ProjectionOffsetY;
			MyScene11.Instance.Environment.CameraPosition = cameraPosition;
			envMatrices.ViewAt0 = m;
			envMatrices.InvViewAt0 = Matrix.Invert(m);
			MatrixD m3 = m * m2;
			envMatrices.ViewProjectionAt0 = m3;
			m3 = m * m2;
			envMatrices.InvViewProjectionAt0 = Matrix.Invert(m3);
			envMatrices.CameraPosition = cameraPosition;
			envMatrices.ViewD = viewMatrix;
			envMatrices.InvViewD = MatrixD.Invert(viewMatrix);
			envMatrices.OriginalProjection = projectionMatrix;
			envMatrices.OriginalProjectionFar = projectionFarMatrix;
			envMatrices.Projection = m2;
			envMatrices.ProjectionForSkybox = projectionForSkybox;
			envMatrices.InvProjection = Matrix.Invert(m2);
			envMatrices.ViewProjectionD = envMatrices.ViewD * (MatrixD)m2;
			Matrix m4 = Matrix.Invert(envMatrices.ViewProjectionD);
			envMatrices.InvViewProjectionD = m4;
			envMatrices.NearClipping = message.NearPlane;
			envMatrices.FarClipping = message.FarPlane;
			envMatrices.LargeDistanceFarClipping = message.FarPlane * 500f;
			int x = ViewportResolution.X;
			int y = ViewportResolution.Y;
			envMatrices.FovV = (float)(2.0 * Math.Atan(Math.Tan((double)(envMatrices.FovH = message.FOV) / 2.0) * ((double)y / (double)x)));
			MyUtils.Init(ref envMatrices.ViewFrustumClippedD);
			envMatrices.ViewFrustumClippedD.Matrix = envMatrices.ViewD * envMatrices.OriginalProjection;
			MyUtils.Init(ref envMatrices.ViewFrustumClippedFarD);
			MatrixD matrix = envMatrices.ViewD * envMatrices.OriginalProjectionFar;
			envMatrices.ViewFrustumClippedFarD.Matrix = matrix;
			envMatrices.LastUpdateWasSmooth = message.Smooth;
		}

		private static void PrepareGameScene()
		{
			MyManagers.EnvironmentProbe.UpdateProbe();
			MyCommon.UpdateFrameConstants();
			MyCommon.VoxelMaterialsConstants.FeedGPU();
			MyOffscreenRenderer.Render();
		}

		private static void DrawGameScene(IRtvBindable renderTarget, out IBorrowedRtvTexture debugAmbientOcclusion)
		{
			PrepareGameScene();
			RC.ClearState();
			if (MyStereoRender.Enable && MyStereoRender.EnableUsingStencilMask)
			{
<<<<<<< HEAD
				MyGpuProfiler.IC_BeginBlock("MyStereoStencilMask.Draw", "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
				MyStereoStencilMask.Draw();
				MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
=======
				MyGpuProfiler.IC_BeginBlock("MyStereoStencilMask.Draw", "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
				MyStereoStencilMask.Draw();
				MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			MyManagers.RenderScheduler.Init();
			MyManagers.RenderScheduler.Execute();
			MyManagers.RenderScheduler.Done();
			debugAmbientOcclusion = MyGBufferResolver.DebugAmbientOcclusion;
			if (Settings.DisplayHDRTest)
			{
				DrawImage("Textures\\Debug\\HDR.dds", MyGBuffer.Main.LBuffer);
			}
			MyManagers.Ansel.MarkHdrBufferFinished();
<<<<<<< HEAD
			MyGpuProfiler.IC_BeginBlock("PostProcess", "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			MyGpuProfiler.IC_BeginBlock("Luminance reduction", "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
=======
			MyGpuProfiler.IC_BeginBlock("PostProcess", "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			MyGpuProfiler.IC_BeginBlock("Luminance reduction", "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			RC.PixelShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			IBorrowedRtvTexture debugHistogram = null;
			if (Postprocess.EnableEyeAdaptation)
			{
				MyEyeAdaptation.Run(RC, MyGBuffer.Main.LBuffer, Settings.DisplayHistogram, out debugHistogram);
			}
			else
			{
<<<<<<< HEAD
				MyGpuProfiler.IC_BeginBlock("MyEyeAdaptation.ConstantExposure", "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
				MyEyeAdaptation.ConstantExposure(RC);
				MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			}
			MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
=======
				MyGpuProfiler.IC_BeginBlock("MyEyeAdaptation.ConstantExposure", "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
				MyEyeAdaptation.ConstantExposure(RC);
				MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			}
			MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (Settings.DisplayHdrIntensity)
			{
				MyHdrDebugTools.DisplayHdrIntensity(RC, MyGBuffer.Main.LBuffer);
			}
<<<<<<< HEAD
			MyGpuProfiler.IC_BeginBlock("Bloom", "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
=======
			MyGpuProfiler.IC_BeginBlock("Bloom", "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			IBorrowedRtvTexture borrowedRtvTexture = null;
			if (m_debugOverrides.Postprocessing && m_debugOverrides.Bloom && Postprocess.BloomEnabled)
			{
				borrowedRtvTexture = MyModernBloom.Run(RC, MyGBuffer.Main.LBuffer, MyGBuffer.Main.GBuffer2, MyGBuffer.Main.ResolvedDepthStencil.SrvDepth, MyEyeAdaptation.GetExposure());
			}
			else
			{
				borrowedRtvTexture = MyManagers.RwTexturesPool.BorrowRtv("bloom_EightScreenUavHDR", ResolutionI.X / 8, ResolutionI.Y / 8, MyGBuffer.LBufferFormat);
				RC.ClearRtv(borrowedRtvTexture, default(RawColor4));
			}
<<<<<<< HEAD
			MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			bool fxaaEnabled = FxaaEnabled;
			MyGpuProfiler.IC_BeginBlock("Tone mapping", "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			IBorrowedCustomTexture borrowedCustomTexture = MyToneMapping.Run(MyGBuffer.Main.LBuffer, MyEyeAdaptation.GetExposure(), borrowedRtvTexture, Postprocess.EnableTonemapping && m_debugOverrides.Postprocessing && m_debugOverrides.Tonemapping, Postprocess.DirtTexture, fxaaEnabled);
			borrowedRtvTexture.Release();
			MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			MyGpuProfiler.IC_BeginBlock("MyHighlight", "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
=======
			MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			bool fxaaEnabled = FxaaEnabled;
			MyGpuProfiler.IC_BeginBlock("Tone mapping", "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			IBorrowedCustomTexture borrowedCustomTexture = MyToneMapping.Run(MyGBuffer.Main.LBuffer, MyEyeAdaptation.GetExposure(), borrowedRtvTexture, Postprocess.EnableTonemapping && m_debugOverrides.Postprocessing && m_debugOverrides.Tonemapping, Postprocess.DirtTexture, fxaaEnabled);
			borrowedRtvTexture.Release();
			MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			MyGpuProfiler.IC_BeginBlock("MyHighlight", "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (MyHighlight.HasHighlights && !MyManagers.Ansel.IsSessionRunning)
			{
				MyHighlight.Run(RC, borrowedCustomTexture.Linear, null);
			}
<<<<<<< HEAD
			MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			MyGpuProfiler.IC_BeginBlock("RenderLDR", "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
=======
			MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			MyGpuProfiler.IC_BeginBlock("RenderLDR", "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (Settings.DrawBillboards && Settings.DrawBillboardsLDR)
			{
				MyBillboardRenderer.RenderLDR(RC, MyGBuffer.Main.ResolvedDepthStencil.SrvDepth, borrowedCustomTexture.SRgb);
			}
<<<<<<< HEAD
			MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			if (fxaaEnabled)
			{
				IBorrowedCustomTexture borrowedCustomTexture2 = MyManagers.RwTexturesPool.BorrowCustom("MyRender11.FXAA.Rgb8");
				MyGpuProfiler.IC_BeginBlock("FXAA", "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
				MyFXAA.Run(RC, borrowedCustomTexture2.Linear, borrowedCustomTexture.Linear);
				MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
=======
			MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			if (fxaaEnabled)
			{
				IBorrowedCustomTexture borrowedCustomTexture2 = MyManagers.RwTexturesPool.BorrowCustom("MyRender11.FXAA.Rgb8");
				MyGpuProfiler.IC_BeginBlock("FXAA", "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
				MyFXAA.Run(RC, borrowedCustomTexture2.Linear, borrowedCustomTexture.Linear);
				MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				borrowedCustomTexture.Release();
				borrowedCustomTexture = borrowedCustomTexture2;
			}
			if (Postprocess.Data.ChromaticFactor != 0f || Postprocess.Data.VignetteStart != 0f)
			{
<<<<<<< HEAD
				MyGpuProfiler.IC_BeginBlock("ChromaticAberration", "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
=======
				MyGpuProfiler.IC_BeginBlock("ChromaticAberration", "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				IBorrowedCustomTexture borrowedCustomTexture3 = MyManagers.RwTexturesPool.BorrowCustom("DrawGameScene.ChromaticAberration");
				MyChromaticAberration.Run(borrowedCustomTexture3, borrowedCustomTexture.Linear);
				borrowedCustomTexture.Release();
				borrowedCustomTexture = borrowedCustomTexture3;
<<<<<<< HEAD
				MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			}
			MyGpuProfiler.IC_BeginBlock("RenderPostPP", "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
=======
				MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			}
			MyGpuProfiler.IC_BeginBlock("RenderPostPP", "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			if (Settings.DrawBillboards && Settings.DrawBillboardsPostPP)
			{
				MyBillboardRenderer.RenderPostPP(RC, MyGBuffer.Main.ResolvedDepthStencil.SrvDepth, borrowedCustomTexture.SRgb);
			}
<<<<<<< HEAD
			MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			if (renderTarget != null)
			{
				MyGpuProfiler.IC_BeginBlock("Copy", "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
				MyCopyToRT.Run(renderTarget, borrowedCustomTexture.SRgb);
				MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
=======
			MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
			if (renderTarget != null)
			{
				MyGpuProfiler.IC_BeginBlock("Copy", "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
				MyCopyToRT.Run(renderTarget, borrowedCustomTexture.SRgb);
				MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			if (Settings.DisplayHistogram)
			{
				MyHdrDebugTools.DisplayDebugHistogram(RC, renderTarget, debugHistogram);
			}
<<<<<<< HEAD
			MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
=======
			MyGpuProfiler.IC_EndBlock(0f, "DrawGameScene", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-DrawScene.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			debugHistogram?.Release();
			borrowedCustomTexture?.Release();
			MyManagers.Cull.OnFrameEnd();
		}

		private static void DrawImage(string fileName, IRtvTexture rtv)
		{
			if (m_stretchPs == MyPixelShaders.Id.NULL)
			{
				m_stretchPs = MyPixelShaders.Create("Postprocess/PostprocessStretch.hlsl");
			}
			ITexture tempTexture = MyManagers.Textures.GetTempTexture(fileName, new MyTextureStreamingManager.QueryArgs
			{
				TextureType = MyFileTextureEnum.GUI,
				SkipQualityReduction = true,
				WaitUntilLoaded = true
			}, 100);
			RC.SetBlendState(null);
			RC.PixelShader.Set(m_stretchPs);
			RC.SetRtv(rtv);
			RC.PixelShader.SetSrv(0, tempTexture);
			MyScreenPass.DrawFullscreenQuad(RC, new MyViewport(rtv.Size.X, rtv.Size.Y));
		}

		private static void TakeCustomSizedScreenshot(Vector2 rescale, bool ignoreSprites)
		{
			IBorrowedRtvTexture borrowedRtvTexture = null;
			Vector2I resolution = new Vector2I(m_resolution * rescale);
			if (rescale.X > 1f || rescale.Y > 1f)
			{
				Vector2I resolution2 = m_resolution;
				m_resolution = resolution;
				Log.WriteLine("Resolution: " + m_resolution);
				Log.WriteLine("Value: " + rescale);
				Log.WriteLine("MaxTextureSize: " + 16384);
				if (m_resolution.X > 16384 || m_resolution.Y > 16384)
				{
					Log.WriteLine("Taking screenshot failed - too big surface " + m_resolution);
					m_resolution = resolution2;
					return;
				}
				Log.WriteLine("Condition passed, creating surfaces..");
				CreateScreenResources();
				Log.WriteLine("Taking screenshot..");
				borrowedRtvTexture = MyManagers.RwTexturesPool.BorrowRtv("TakeCustomizedScreenshot", m_swapchain.Description.ModeDescription.Format);
				if (m_drawScene)
				{
					DrawGameScene(borrowedRtvTexture, out var debugAmbientOcclusion);
					debugAmbientOcclusion.Release();
					m_resetEyeAdaptation = true;
				}
				else
				{
					RC.ClearRtv(borrowedRtvTexture, new RawColor4(0f, 0f, 0f, 1f));
				}
				if (!ignoreSprites)
				{
					MyViewport myViewport = new MyViewport(borrowedRtvTexture.Size.X, borrowedRtvTexture.Size.Y);
					Vector2 size = resolution2;
					RenderMainSprites(borrowedRtvTexture, ScaleMainViewport(myViewport), myViewport, size);
					ConsumeMainSprites();
				}
				m_resolution = resolution2;
				CreateScreenResources();
			}
			else
			{
				borrowedRtvTexture = MyManagers.RwTexturesPool.BorrowRtv("TakeCustomizedScreenshot", resolution.X, resolution.Y, m_swapchain.Description.ModeDescription.Format);
				MyCopyToRT.Run(borrowedRtvTexture, Backbuffer);
			}
			SaveScreenshotFromResource(borrowedRtvTexture);
			borrowedRtvTexture?.Release();
		}

		private static void UpdateGameScene()
		{
			MyBillboardRenderer.OnFrameStart();
			MyAtmosphereRenderer.Update();
			MyCloudRenderer.Update();
			MyManagers.ParticleEffectsManager.Update();
			MyManagers.ModelFactory.CompleteAsyncTasks();
			MyMeshes.Load();
			MyAlphaTransition.Update();
			MyManagers.TextureChangeManager.OnUpdate();
			MyScene11.Instance.Update();
			GatherTextures();
			MyBigMeshTable.Table.MoveToGPU();
			MyCommon.MoveToNextFrame();
		}

		private static void SaveScreenshotFromResource(IResource res)
		{
			MyRenderProxy.ScreenshotTaken(MyTextureData.ToFile(res, m_screenshot.Value.SavePath, m_screenshot.Value.Format), m_screenshot.Value.SavePath, m_screenshot.Value.ShowNotification);
			m_screenshot = null;
		}

		private static void ProcessMessageQueue()
		{
			NextFrame();
			if (m_deferredStateChanges)
			{
				ProcessDrawMessageQueue();
			}
			else
			{
				ProcessMessageQueue(draw: true);
			}
		}

		private static void ProcessMessageQueue(bool draw)
		{
			bool flag = true;
			FrameProcessStatus = FrameProcessStatusEnum.NoProcess;
			while (flag)
			{
				bool isPreFrame;
				MyUpdateFrame renderFrame = SharedData.GetRenderFrame(out isPreFrame);
				if (renderFrame == null)
				{
					FrameProcessStatus = FrameProcessStatusEnum.NoFrame;
					ProcessRenderFrame(SharedData.MessagesForNextFrame, draw);
					ProcessPreFrame(SharedData.MessagesForNextFrame);
					return;
				}
				CurrentUpdateTime = renderFrame.UpdateTimestamp;
				if (!renderFrame.Processed)
				{
					MyCommon.UpdateTime = MyCommon.FrameTime;
				}
				if (isPreFrame)
				{
					if (!renderFrame.Processed)
					{
						FrameProcessStatus = FrameProcessStatusEnum.Skipped;
					}
					ProcessPreFrame(renderFrame, returnShared: true);
					continue;
				}
				if (!renderFrame.Processed)
				{
					if (FrameProcessStatus == FrameProcessStatusEnum.NoProcess)
					{
						FrameProcessStatus = FrameProcessStatusEnum.Success;
					}
				}
				else
				{
					FrameProcessStatus = FrameProcessStatusEnum.AlreadyProcessed;
				}
				ProcessRenderFrame(renderFrame, draw);
				flag = false;
			}
			ProcessRenderFrame(SharedData.MessagesForNextFrame, draw);
			ProcessPreFrame(SharedData.MessagesForNextFrame);
			MyManagers.PostponedUpdate.Apply();
		}

		private static void ProcessDrawMessageQueue()
		{
			MyUpdateFrame drawRenderFrame;
			do
			{
				drawRenderFrame = SharedData.GetDrawRenderFrame();
			}
			while (!ProcessDrawFrame(drawRenderFrame));
		}

		private static bool ProcessDrawFrame(MyUpdateFrame frame)
		{
			m_messageFrameCounter++;
			lock (frame)
			{
				if (frame.Cleared)
				{
					return false;
				}
				foreach (MyRenderMessageBase item in frame.RenderInput)
				{
					if (item.MessageClass == MyRenderMessageType.Draw || item.MessageClass == MyRenderMessageType.DebugDraw)
					{
						ProcessMessage(item, m_messageFrameCounter);
					}
				}
				return true;
			}
		}

		private static void ProcessRenderFrame(MyUpdateFrame frame, bool draw)
		{
			m_messageFrameCounter++;
			for (int i = 0; i < frame.RenderInput.Count; i++)
			{
				MyRenderMessageBase myRenderMessageBase = frame.RenderInput[i];
				if ((draw || (myRenderMessageBase.MessageClass != 0 && myRenderMessageBase.MessageClass != MyRenderMessageType.DebugDraw)) && (!frame.Processed || myRenderMessageBase.MessageClass != MyRenderMessageType.StateChangeOnce))
				{
					ProcessMessage(myRenderMessageBase, m_messageFrameCounter);
				}
			}
			frame.Processed = true;
		}

		private static void ProcessPreFrame(MyUpdateFrame frame, bool returnShared = false)
		{
			m_messageFrameCounter++;
			if (!frame.Processed)
			{
				foreach (MyRenderMessageBase item in frame.RenderInput)
				{
					if (item.MessageClass != 0 && item.MessageClass != MyRenderMessageType.DebugDraw)
					{
						ProcessMessage(item, m_messageFrameCounter);
					}
				}
			}
			lock (frame)
			{
				frame.Cleared = true;
				foreach (MyRenderMessageBase item2 in frame.RenderInput)
				{
					if (!item2.IsPersistent)
					{
						item2.Dispose();
					}
				}
				frame.RenderInput.Clear();
				if (returnShared)
				{
					SharedData.ReturnPreFrame(frame);
				}
			}
		}

		internal static Vector3 ColorFromMask(Vector3 hsv)
		{
			return hsv;
		}

		private static void NextFrame()
		{
			m_processCtr = 0;
			m_processStopwatch.Restart();
		}

		private static void ProcessMessage(MyRenderMessageBase message, int frameId)
		{
			ProcessMessageInternal(message, frameId);
			m_processCtr++;
			if (m_processCtr % 1000 != 0)
			{
				return;
			}
			m_processStopwatch.Stop();
<<<<<<< HEAD
			if (m_processStopwatch.Elapsed.TotalSeconds > 0.5)
=======
			if (m_processStopwatch.get_Elapsed().TotalSeconds > 0.5)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				if (MyRenderProxy.EnableAppEventsCall)
				{
					MyRenderProxy.RenderThread.RenderWindow.DoEvents();
				}
				m_processStopwatch.Reset();
			}
			m_processStopwatch.Start();
		}

		private static void ProcessMessageInternal(MyRenderMessageBase message, int frameId)
		{
			switch (message.MessageType)
			{
			case MyRenderMessageEnum.DrawCommands:
				foreach (MyRenderMessageBase message2 in ((MyRenderMessageDrawCommands)message).Messages)
				{
					ProcessMessage(message2, frameId);
				}
				break;
			case MyRenderMessageEnum.SetCameraViewMatrix:
				SetupCameraMatrices((MyRenderMessageSetCameraViewMatrix)message);
				break;
			case MyRenderMessageEnum.DrawScene:
				m_drawScene = true;
				break;
			case MyRenderMessageEnum.RenderProfiler:
			{
				MyRenderMessageRenderProfiler myRenderMessageRenderProfiler = (MyRenderMessageRenderProfiler)message;
				MyRenderProfiler.HandleInput(myRenderMessageRenderProfiler.Command, myRenderMessageRenderProfiler.Index, myRenderMessageRenderProfiler.Value);
				break;
			}
			case MyRenderMessageEnum.CreateRenderCharacter:
			{
				MyRenderMessageCreateRenderCharacter myRenderMessageCreateRenderCharacter = (MyRenderMessageCreateRenderCharacter)message;
<<<<<<< HEAD
				MyActor myActor9 = MyActorFactory.CreateCharacter(myRenderMessageCreateRenderCharacter.DebugName);
				MyRenderableComponent renderable4 = myActor9.GetRenderable();
				renderable4.SetModel(MyMeshes.GetMeshId(MyStringId.GetOrCompute(myRenderMessageCreateRenderCharacter.Model), 1f, DeferStateChangeBatch));
				myActor9.SetMatrix(ref myRenderMessageCreateRenderCharacter.WorldMatrix);
				if (myRenderMessageCreateRenderCharacter.ColorMaskHSV.HasValue)
				{
					renderable4.SetKeyColor(ColorFromMask(myRenderMessageCreateRenderCharacter.ColorMaskHSV.Value));
				}
				myActor9.SetID(myRenderMessageCreateRenderCharacter.ID);
				renderable4.AdditionalFlags |= MyProxiesFactory.GetRenderableProxyFlags(myRenderMessageCreateRenderCharacter.Flags);
				renderable4.DrawFlags = MyDrawSubmesh.MySubmeshFlags.Gbuffer | MyDrawSubmesh.MySubmeshFlags.Depth;
				if (myRenderMessageCreateRenderCharacter.FadeIn)
				{
					renderable4.StartFadeIn();
=======
				MyActor myActor2 = MyActorFactory.CreateCharacter(myRenderMessageCreateRenderCharacter.DebugName);
				MyRenderableComponent renderable2 = myActor2.GetRenderable();
				renderable2.SetModel(MyMeshes.GetMeshId(MyStringId.GetOrCompute(myRenderMessageCreateRenderCharacter.Model), 1f, DeferStateChangeBatch));
				myActor2.SetMatrix(ref myRenderMessageCreateRenderCharacter.WorldMatrix);
				if (myRenderMessageCreateRenderCharacter.ColorMaskHSV.HasValue)
				{
					renderable2.SetKeyColor(ColorFromMask(myRenderMessageCreateRenderCharacter.ColorMaskHSV.Value));
				}
				myActor2.SetID(myRenderMessageCreateRenderCharacter.ID);
				renderable2.AdditionalFlags |= MyProxiesFactory.GetRenderableProxyFlags(myRenderMessageCreateRenderCharacter.Flags);
				renderable2.DrawFlags = MyDrawSubmesh.MySubmeshFlags.Gbuffer | MyDrawSubmesh.MySubmeshFlags.Depth;
				if (myRenderMessageCreateRenderCharacter.FadeIn)
				{
					renderable2.StartFadeIn();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				break;
			}
			case MyRenderMessageEnum.SetCharacterSkeleton:
			{
				MyRenderMessageSetCharacterSkeleton myRenderMessageSetCharacterSkeleton = (MyRenderMessageSetCharacterSkeleton)message;
				MyIDTracker<MyActor>.FindByID(myRenderMessageSetCharacterSkeleton.CharacterID)?.GetSkinning().SetSkeleton(myRenderMessageSetCharacterSkeleton.SkeletonBones, myRenderMessageSetCharacterSkeleton.SkeletonIndices);
				break;
			}
			case MyRenderMessageEnum.SetCharacterTransforms:
			{
				MyRenderMessageSetCharacterTransforms myRenderMessageSetCharacterTransforms = (MyRenderMessageSetCharacterTransforms)message;
				MyIDTracker<MyActor>.FindByID(myRenderMessageSetCharacterTransforms.CharacterID)?.GetSkinning().SetAnimationBones(myRenderMessageSetCharacterTransforms.BoneAbsoluteTransforms, myRenderMessageSetCharacterTransforms.BoneDecalUpdates);
				break;
			}
			case MyRenderMessageEnum.UpdateRenderEntity:
			{
				MyRenderMessageUpdateRenderEntity myRenderMessageUpdateRenderEntity = (MyRenderMessageUpdateRenderEntity)message;
<<<<<<< HEAD
				MyActor myActor17 = MyIDTracker<MyActor>.FindByID(myRenderMessageUpdateRenderEntity.ID);
				if (myActor17 == null)
				{
					break;
				}
				if (myActor17.GetRenderable() != null)
				{
					if (myRenderMessageUpdateRenderEntity.ColorMaskHSV.HasValue)
					{
						myActor17.GetRenderable().SetKeyColor(ColorFromMask(myRenderMessageUpdateRenderEntity.ColorMaskHSV.Value));
					}
					if (myRenderMessageUpdateRenderEntity.Dithering.HasValue)
					{
						myActor17.GetRenderable().StopFadeIn();
						myActor17.GetRenderable().SetDithering(myRenderMessageUpdateRenderEntity.Dithering.Value);
					}
					if (myRenderMessageUpdateRenderEntity.FadeIn)
					{
						myActor17.GetRenderable().StartFadeIn();
					}
				}
				if (myActor17.GetInstance() != null)
=======
				MyActor myActor16 = MyIDTracker<MyActor>.FindByID(myRenderMessageUpdateRenderEntity.ID);
				if (myActor16 == null)
				{
					break;
				}
				if (myActor16.GetRenderable() != null)
				{
					if (myRenderMessageUpdateRenderEntity.ColorMaskHSV.HasValue)
					{
						myActor16.GetRenderable().SetKeyColor(ColorFromMask(myRenderMessageUpdateRenderEntity.ColorMaskHSV.Value));
					}
					if (myRenderMessageUpdateRenderEntity.Dithering.HasValue)
					{
						myActor16.GetRenderable().StopFadeIn();
						myActor16.GetRenderable().SetDithering(myRenderMessageUpdateRenderEntity.Dithering.Value);
					}
					if (myRenderMessageUpdateRenderEntity.FadeIn)
					{
						myActor16.GetRenderable().StartFadeIn();
					}
				}
				if (myActor16.GetInstance() != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					if (myRenderMessageUpdateRenderEntity.ColorMaskHSV.HasValue)
					{
						Vector3 vector = ColorFromMask(myRenderMessageUpdateRenderEntity.ColorMaskHSV.Value);
<<<<<<< HEAD
						myActor17.GetInstance().KeyColor = new HalfVector4(vector.X, vector.Y, vector.Z, 0f);
					}
					if (myRenderMessageUpdateRenderEntity.Dithering.HasValue)
					{
						myActor17.GetInstance().SetDithered(myRenderMessageUpdateRenderEntity.Dithering.Value < 0f, Math.Abs(myRenderMessageUpdateRenderEntity.Dithering.Value));
					}
					if (myRenderMessageUpdateRenderEntity.FadeIn)
					{
						myActor17.GetInstance().StartFadeIn();
					}
				}
				if (myActor17.GetVoxel() != null && myRenderMessageUpdateRenderEntity.Dithering.HasValue)
				{
					myActor17.GetVoxel().SetDithering(myRenderMessageUpdateRenderEntity.Dithering.Value);
=======
						myActor16.GetInstance().KeyColor = new HalfVector4(vector.X, vector.Y, vector.Z, 0f);
					}
					if (myRenderMessageUpdateRenderEntity.Dithering.HasValue)
					{
						myActor16.GetInstance().SetDithered(myRenderMessageUpdateRenderEntity.Dithering.Value < 0f, Math.Abs(myRenderMessageUpdateRenderEntity.Dithering.Value));
					}
					if (myRenderMessageUpdateRenderEntity.FadeIn)
					{
						myActor16.GetInstance().StartFadeIn();
					}
				}
				if (myActor16.GetVoxel() != null && myRenderMessageUpdateRenderEntity.Dithering.HasValue)
				{
					myActor16.GetVoxel().SetDithering(myRenderMessageUpdateRenderEntity.Dithering.Value);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				break;
			}
			case MyRenderMessageEnum.SetVisibilityUpdates:
			{
				MyRenderMessageSetVisibilityUpdates myRenderMessageSetVisibilityUpdates = (MyRenderMessageSetVisibilityUpdates)message;
				MyRenderProxy.ObjectType objectType2 = MyRenderProxy.GetObjectType(myRenderMessageSetVisibilityUpdates.ID);
				if (objectType2 == MyRenderProxy.ObjectType.Entity || objectType2 == MyRenderProxy.ObjectType.Light || objectType2 == MyRenderProxy.ObjectType.ManualCull)
				{
					MyIDTracker<MyActor>.FindByID(myRenderMessageSetVisibilityUpdates.ID)?.SetVisibilityUpdates(myRenderMessageSetVisibilityUpdates.State);
				}
				break;
			}
			case MyRenderMessageEnum.ChangeModel:
			{
				MyRenderMessageChangeModel myRenderMessageChangeModel = (MyRenderMessageChangeModel)message;
<<<<<<< HEAD
				MyActor myActor2 = MyIDTracker<MyActor>.FindByID(myRenderMessageChangeModel.ID);
				if (myActor2 != null && myActor2.GetRenderable() != null)
				{
					MyRenderableComponent renderable2 = myActor2.GetRenderable();
					MeshId meshId = MyMeshes.GetMeshId(X.TEXT_(myRenderMessageChangeModel.Model), myRenderMessageChangeModel.Scale, DeferStateChangeBatch);
					if (renderable2.GetModel() != meshId)
					{
						renderable2.SetModel(meshId);
=======
				MyActor myActor18 = MyIDTracker<MyActor>.FindByID(myRenderMessageChangeModel.ID);
				if (myActor18 != null && myActor18.GetRenderable() != null)
				{
					MyRenderableComponent renderable4 = myActor18.GetRenderable();
					MeshId meshId = MyMeshes.GetMeshId(X.TEXT_(myRenderMessageChangeModel.Model), myRenderMessageChangeModel.Scale, DeferStateChangeBatch);
					if (renderable4.GetModel() != meshId)
					{
						renderable4.SetModel(meshId);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				break;
			}
			case MyRenderMessageEnum.CreateRenderVoxelDebris:
			{
				MyRenderMessageCreateRenderVoxelDebris myRenderMessageCreateRenderVoxelDebris = (MyRenderMessageCreateRenderVoxelDebris)message;
<<<<<<< HEAD
				MyActor myActor18 = MyActorFactory.CreateSceneObject(myRenderMessageCreateRenderVoxelDebris.DebugName);
				myActor18.GetRenderable().AdditionalFlags |= MyRenderableProxyFlags.SkipInForward;
				if (myRenderMessageCreateRenderVoxelDebris.Model != null)
				{
					myActor18.GetRenderable().SetModel(MyMeshes.GetMeshId(X.TEXT_(myRenderMessageCreateRenderVoxelDebris.Model), 1f, DeferStateChangeBatch));
				}
				myActor18.SetID(myRenderMessageCreateRenderVoxelDebris.ID);
				myActor18.SetMatrix(ref myRenderMessageCreateRenderVoxelDebris.WorldMatrix);
				if (myRenderMessageCreateRenderVoxelDebris.FadeIn)
				{
					myActor18.GetRenderable().StartFadeIn();
=======
				MyActor myActor14 = MyActorFactory.CreateSceneObject(myRenderMessageCreateRenderVoxelDebris.DebugName);
				myActor14.GetRenderable().AdditionalFlags |= MyRenderableProxyFlags.SkipInForward;
				if (myRenderMessageCreateRenderVoxelDebris.Model != null)
				{
					myActor14.GetRenderable().SetModel(MyMeshes.GetMeshId(X.TEXT_(myRenderMessageCreateRenderVoxelDebris.Model), 1f, DeferStateChangeBatch));
				}
				myActor14.SetID(myRenderMessageCreateRenderVoxelDebris.ID);
				myActor14.SetMatrix(ref myRenderMessageCreateRenderVoxelDebris.WorldMatrix);
				if (myRenderMessageCreateRenderVoxelDebris.FadeIn)
				{
					myActor14.GetRenderable().StartFadeIn();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				MyRenderableComponent.DebrisEntityData[myRenderMessageCreateRenderVoxelDebris.ID] = new MyRenderableComponent.MyDebrisData
				{
					VoxelMaterial = myRenderMessageCreateRenderVoxelDebris.VoxelMaterialIndex,
					ColorMultiplier = new Vector3(myRenderMessageCreateRenderVoxelDebris.TextureColorMultiplier)
				};
				break;
			}
			case MyRenderMessageEnum.CreateScreenDecal:
			{
				MyRenderMessageCreateScreenDecal myRenderMessageCreateScreenDecal = (MyRenderMessageCreateScreenDecal)message;
<<<<<<< HEAD
				MyScreenDecals.AddDecal(myRenderMessageCreateScreenDecal.ID, myRenderMessageCreateScreenDecal.ParentIDs, ref myRenderMessageCreateScreenDecal.TopoData, myRenderMessageCreateScreenDecal.Flags, myRenderMessageCreateScreenDecal.SourceTarget, myRenderMessageCreateScreenDecal.Material, myRenderMessageCreateScreenDecal.MaterialIndex, myRenderMessageCreateScreenDecal.RenderSqDistance, myRenderMessageCreateScreenDecal.IsTrail, myRenderMessageCreateScreenDecal.TimeUntilLive);
=======
				MyScreenDecals.AddDecal(myRenderMessageCreateScreenDecal.ID, myRenderMessageCreateScreenDecal.ParentIDs, ref myRenderMessageCreateScreenDecal.TopoData, myRenderMessageCreateScreenDecal.Flags, myRenderMessageCreateScreenDecal.SourceTarget, myRenderMessageCreateScreenDecal.Material, myRenderMessageCreateScreenDecal.MaterialIndex, myRenderMessageCreateScreenDecal.RenderSqDistance, myRenderMessageCreateScreenDecal.IsTrail);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				break;
			}
			case MyRenderMessageEnum.UpdateScreenDecal:
				MyScreenDecals.UpdateDecals(((MyRenderMessageUpdateScreenDecal)message).Decals);
				break;
			case MyRenderMessageEnum.CreateRenderEntity:
			{
				MyRenderMessageCreateRenderEntity myRenderMessageCreateRenderEntity = (MyRenderMessageCreateRenderEntity)message;
				MyActor myActor7 = MyActorFactory.Create(myRenderMessageCreateRenderEntity.DebugName);
				string model2 = myRenderMessageCreateRenderEntity.Model;
				MyModel myModel2 = null;
				string text2 = null;
				if (model2 != null && (myRenderMessageCreateRenderEntity.Flags & RenderFlags.ForceOldPipeline) == 0)
				{
					text2 = MyAssetsLoader.ModelRemap.Get(model2, model2);
					myModel2 = MyManagers.ModelFactory.GetOrCreateModels(text2);
				}
				if (myModel2 != null && myModel2.IsValid)
				{
					bool isVisible = (myRenderMessageCreateRenderEntity.Flags & RenderFlags.Visible) == RenderFlags.Visible;
					MyVisibilityExtFlags myVisibilityExtFlags = MyVisibilityExtFlags.None;
					if ((myRenderMessageCreateRenderEntity.Flags & RenderFlags.SkipInMainView) != RenderFlags.SkipInMainView)
					{
						myVisibilityExtFlags |= MyVisibilityExtFlags.Gbuffer;
					}
					if ((myRenderMessageCreateRenderEntity.Flags & (RenderFlags.CastShadows | RenderFlags.SkipInDepth)) == RenderFlags.CastShadows)
					{
						myVisibilityExtFlags |= MyVisibilityExtFlags.Depth;
					}
					if ((myRenderMessageCreateRenderEntity.Flags & RenderFlags.SkipInForward) != RenderFlags.SkipInForward)
					{
						myVisibilityExtFlags |= MyVisibilityExtFlags.Forward;
					}
					bool metalnessColorable = (myRenderMessageCreateRenderEntity.Flags & RenderFlags.MetalnessColorable) > (RenderFlags)0;
					MyCompatibilityDataForTheOldPipeline myCompatibilityDataForTheOldPipeline = default(MyCompatibilityDataForTheOldPipeline);
					myCompatibilityDataForTheOldPipeline.Rescale = myRenderMessageCreateRenderEntity.Rescale;
					myCompatibilityDataForTheOldPipeline.DepthBias = myRenderMessageCreateRenderEntity.DepthBias;
					myCompatibilityDataForTheOldPipeline.MwmFilepath = myRenderMessageCreateRenderEntity.Model;
					myCompatibilityDataForTheOldPipeline.RenderFlags = myRenderMessageCreateRenderEntity.Flags;
					MyCompatibilityDataForTheOldPipeline compatibilityData = myCompatibilityDataForTheOldPipeline;
					myActor7.AddComponent<MyInstanceComponent>(MyComponentFactory<MyInstanceComponent>.Create());
					myActor7.GetInstance().Init(myModel2, isVisible, myRenderMessageCreateRenderEntity.ID, myVisibilityExtFlags, compatibilityData, metalnessColorable, text2, MyManagers.ModelFactory.IsDummyModel(myModel2));
				}
				else
				{
					MeshId nULL = MeshId.NULL;
<<<<<<< HEAD
					string str = MyAssetsLoader.ModelRemap.Get(model2, model2);
					if (myRenderMessageCreateRenderEntity.ForceReload)
					{
						MyMeshes.RemoveMesh(X.TEXT_(str));
					}
					nULL = MyMeshes.GetMeshId(X.TEXT_(str), myRenderMessageCreateRenderEntity.Rescale, DeferStateChangeBatch);
=======
					nULL = MyMeshes.GetMeshId(X.TEXT_(MyAssetsLoader.ModelRemap.Get(model2, model2)), myRenderMessageCreateRenderEntity.Rescale, DeferStateChangeBatch);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					myActor7.AddComponent<MyRenderableComponent>(MyComponentFactory<MyRenderableComponent>.Create());
					MyRenderableComponent renderable3 = myActor7.GetRenderable();
					renderable3.AdditionalFlags |= MyProxiesFactory.GetRenderableProxyFlags(myRenderMessageCreateRenderEntity.Flags);
					renderable3.DepthBias = myRenderMessageCreateRenderEntity.DepthBias;
					if (nULL != MeshId.NULL)
					{
						renderable3.SetModel(nULL);
					}
				}
				myActor7.SetID(myRenderMessageCreateRenderEntity.ID);
				myActor7.SetMatrix(ref myRenderMessageCreateRenderEntity.WorldMatrix);
				break;
			}
			case MyRenderMessageEnum.CreateStaticGroup:
			{
				MyRenderMessageCreateStaticGroup myRenderMessageCreateStaticGroup = (MyRenderMessageCreateStaticGroup)message;
				MyActor myActor3 = MyActorFactory.Create("I am static group! My debug name is not implemented properly!");
				myActor3.SetID(myRenderMessageCreateStaticGroup.ID);
				string model = myRenderMessageCreateStaticGroup.Model;
				MyModel myModel = null;
				string text = null;
				if (model != null)
				{
					text = MyAssetsLoader.ModelRemap.Get(model, model);
					myModel = MyManagers.ModelFactory.GetOrCreateModels(text);
				}
				if (myModel != null && myModel.IsValid)
				{
					myActor3.AddComponent<MyStaticGroupComponent>(MyComponentFactory<MyStaticGroupComponent>.Create());
					myActor3.GetStaticGroup().Init(myModel, myRenderMessageCreateStaticGroup.Translation, myRenderMessageCreateStaticGroup.LocalMatrices, text, MyManagers.ModelFactory.IsDummyModel(myModel));
				}
				else
				{
					MyRenderProxy.Error($"Model '{myRenderMessageCreateStaticGroup.Model}' cannot be loaded by the new pipeline.");
				}
				break;
			}
			case MyRenderMessageEnum.CreateRenderEntityClouds:
				MyCloudRenderer.CreateCloudLayer(ref ((MyRenderMessageCreateRenderEntityClouds)message).Settings);
				break;
			case MyRenderMessageEnum.CreateRenderEntityAtmosphere:
			{
				MyRenderMessageCreateRenderEntityAtmosphere myRenderMessageCreateRenderEntityAtmosphere = (MyRenderMessageCreateRenderEntityAtmosphere)message;
				if (myRenderMessageCreateRenderEntityAtmosphere.Technique == MyMeshDrawTechnique.ATMOSPHERE)
				{
					float num = 6360000f;
					float num2 = 6420000f;
					float num3 = num2 / num;
					float num4 = myRenderMessageCreateRenderEntityAtmosphere.AtmosphereRadius / myRenderMessageCreateRenderEntityAtmosphere.PlanetRadius;
					_ = (num4 - 1f) / (num3 - 1f);
					num2 = num * num4;
					float planetScaleFactor = myRenderMessageCreateRenderEntityAtmosphere.PlanetRadius / num;
					float atmosphereScaleFactor = (myRenderMessageCreateRenderEntityAtmosphere.AtmosphereRadius - myRenderMessageCreateRenderEntityAtmosphere.PlanetRadius) / (myRenderMessageCreateRenderEntityAtmosphere.PlanetRadius * 0.5f);
					Vector3 rayleighScattering = new Vector3(5.8E-06f, 1.35E-05f, 3.31E-05f);
					Vector3 mieScattering = new Vector3(2E-05f, 2E-05f, 2E-05f);
					float rayleighHeightScale = 8000f;
					float mieHeightScale = 1200f;
					MyAtmosphereRenderer.CreateAtmosphere(myRenderMessageCreateRenderEntityAtmosphere.ID, myRenderMessageCreateRenderEntityAtmosphere.WorldMatrix, num, num2, rayleighScattering, rayleighHeightScale, mieScattering, mieHeightScale, planetScaleFactor, atmosphereScaleFactor, myRenderMessageCreateRenderEntityAtmosphere.FadeIn);
				}
				break;
			}
			case MyRenderMessageEnum.RemoveDecal:
<<<<<<< HEAD
			{
				MyRenderMessageRemoveDecal myRenderMessageRemoveDecal = (MyRenderMessageRemoveDecal)message;
				MyScreenDecals.RemoveDecal(myRenderMessageRemoveDecal.ID, myRenderMessageRemoveDecal.Immediately);
				break;
			}
=======
				MyScreenDecals.RemoveDecal(((MyRenderMessageRemoveDecal)message).ID);
				break;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			case MyRenderMessageEnum.SetDecalGlobals:
				MyScreenDecals.SetDecalGlobals(((MyRenderMessageSetDecalGlobals)message).Globals);
				break;
			case MyRenderMessageEnum.RegisterDecalsMaterials:
<<<<<<< HEAD
			{
				MyRenderMessageRegisterScreenDecalsMaterials obj = (MyRenderMessageRegisterScreenDecalsMaterials)message;
				MyScreenDecals.RegisterMaterials(obj.MaterialDescriptions);
				obj.MaterialDescriptions = null;
				break;
			}
=======
				MyScreenDecals.RegisterMaterials(((MyRenderMessageRegisterScreenDecalsMaterials)message).MaterialDescriptions);
				break;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			case MyRenderMessageEnum.ClearDecals:
				_ = (MyRenderMessageClearScreenDecals)message;
				MyScreenDecals.ClearDecals();
				break;
			case MyRenderMessageEnum.CreateParticleEffect:
			{
				MyRenderMessageCreateParticleEffect myRenderMessageCreateParticleEffect = (MyRenderMessageCreateParticleEffect)message;
				MyManagers.ParticleEffectsManager.Create(myRenderMessageCreateParticleEffect.ID, myRenderMessageCreateParticleEffect.Data, myRenderMessageCreateParticleEffect.DebugName);
				break;
			}
			case MyRenderMessageEnum.UpdateParticleEffect:
			{
				MyRenderMessageUpdateParticleEffect myRenderMessageUpdateParticleEffect = (MyRenderMessageUpdateParticleEffect)message;
				MyManagers.ParticleEffectsManager.Get(myRenderMessageUpdateParticleEffect.State.ID)?.UpdateState(ref myRenderMessageUpdateParticleEffect.State);
				break;
			}
			case MyRenderMessageEnum.UpdateRenderObject:
			{
				MyRenderMessageUpdateRenderObject myRenderMessageUpdateRenderObject = (MyRenderMessageUpdateRenderObject)message;
				MyRenderProxy.ObjectType objectType4 = MyRenderProxy.GetObjectType(myRenderMessageUpdateRenderObject.ID);
				if (objectType4 != 0 && objectType4 != MyRenderProxy.ObjectType.ManualCull)
				{
					_ = 10;
				}
				else
				{
					MyManagers.PostponedUpdate.SavePostponedUpdate(myRenderMessageUpdateRenderObject);
				}
				break;
			}
			case MyRenderMessageEnum.UpdateRenderComponent:
			{
				MyRenderMessageUpdateComponent myRenderMessageUpdateComponent = (MyRenderMessageUpdateComponent)message;
				MyIDTracker<MyActor>.FindByID(myRenderMessageUpdateComponent.ID)?.UpdateComponent(myRenderMessageUpdateComponent);
				break;
			}
			case MyRenderMessageEnum.RemoveRenderObject:
			{
				MyRenderMessageRemoveRenderObject myRenderMessageRemoveRenderObject = (MyRenderMessageRemoveRenderObject)message;
				MyHighlight.RemoveObjects(myRenderMessageRemoveRenderObject.ID, null);
				MyHighlight.RemoveOverlappingModel(myRenderMessageRemoveRenderObject.ID);
				MyRenderProxy.ObjectType objectType3 = MyRenderProxy.GetObjectType(myRenderMessageRemoveRenderObject.ID);
				switch (objectType3)
				{
				case MyRenderProxy.ObjectType.Entity:
				case MyRenderProxy.ObjectType.Light:
				case MyRenderProxy.ObjectType.ManualCull:
					MyIDTracker<MyActor>.FindByID(myRenderMessageRemoveRenderObject.ID)?.Destroy(myRenderMessageRemoveRenderObject.FadeOut);
					break;
				case MyRenderProxy.ObjectType.InstanceBuffer:
					if (myRenderMessageRemoveRenderObject.FadeOut)
					{
						uint id = myRenderMessageRemoveRenderObject.ID;
						MyScene11.Instance.Updater.CallIn(delegate
						{
							MyInstancing.Remove(id, uint.MaxValue, checkConsistency: false);
						}, MyTimeSpan.FromSeconds(MyCommon.LoddingSettings.Global.MaxTransitionInSeconds));
					}
					else
					{
						MyInstancing.Remove(myRenderMessageRemoveRenderObject.ID, uint.MaxValue);
					}
					break;
				case MyRenderProxy.ObjectType.ParticleEffect:
					MyManagers.ParticleEffectsManager.Remove(myRenderMessageRemoveRenderObject.ID, !myRenderMessageRemoveRenderObject.FadeOut, output: false);
					break;
				case MyRenderProxy.ObjectType.Atmosphere:
					MyAtmosphereRenderer.RemoveAtmosphere(myRenderMessageRemoveRenderObject.ID, myRenderMessageRemoveRenderObject.FadeOut);
					break;
				case MyRenderProxy.ObjectType.Cloud:
					MyCloudRenderer.RemoveCloud(myRenderMessageRemoveRenderObject.ID, myRenderMessageRemoveRenderObject.FadeOut);
					break;
				case MyRenderProxy.ObjectType.DebugDrawMesh:
					MyPrimitivesRenderer.RemoveDebugMesh(myRenderMessageRemoveRenderObject.ID);
					break;
				case MyRenderProxy.ObjectType.Video:
					MyVideoFactory.Remove(myRenderMessageRemoveRenderObject.ID);
					break;
				}
				if (objectType3 != MyRenderProxy.ObjectType.Invalid)
				{
					MyRenderProxy.RemoveMessageId(myRenderMessageRemoveRenderObject.ID, objectType3);
				}
				break;
			}
			case MyRenderMessageEnum.UpdateRenderObjectVisibility:
			{
				MyRenderMessageUpdateRenderObjectVisibility myRenderMessageUpdateRenderObjectVisibility = (MyRenderMessageUpdateRenderObjectVisibility)message;
				MyRenderProxy.ObjectType objectType = MyRenderProxy.GetObjectType(myRenderMessageUpdateRenderObjectVisibility.ID);
				if (objectType != 0 && objectType != MyRenderProxy.ObjectType.ManualCull)
				{
					_ = 10;
					break;
				}
				MyActor myActor = MyIDTracker<MyActor>.FindByID(myRenderMessageUpdateRenderObjectVisibility.ID);
				myActor?.SetVisibility(myRenderMessageUpdateRenderObjectVisibility.Visible);
				MyRenderableComponent renderable = myActor.GetRenderable();
				if (renderable != null)
				{
					if (myRenderMessageUpdateRenderObjectVisibility.NearFlag)
					{
						renderable.DepthBias = 50000;
					}
					else if (renderable.DepthBias == 50000)
					{
						renderable.DepthBias = 0;
					}
				}
				MyInstanceComponent component = myActor.GetComponent<MyInstanceComponent>();
				if (component != null && myRenderMessageUpdateRenderObjectVisibility.NearFlag)
				{
					component.DepthBias = 50000;
					MyComponentConverter.ConvertActorToTheOldPipeline(myActor);
				}
				break;
			}
			case MyRenderMessageEnum.CreateRenderInstanceBuffer:
			{
				MyRenderMessageCreateRenderInstanceBuffer myRenderMessageCreateRenderInstanceBuffer = (MyRenderMessageCreateRenderInstanceBuffer)message;
				MyInstancing.Create(myRenderMessageCreateRenderInstanceBuffer.ID, myRenderMessageCreateRenderInstanceBuffer.ParentID, myRenderMessageCreateRenderInstanceBuffer.Type, myRenderMessageCreateRenderInstanceBuffer.DebugName);
				break;
			}
			case MyRenderMessageEnum.UpdateRenderInstanceBufferRange:
			{
				MyRenderMessageUpdateRenderInstanceBufferRange myRenderMessageUpdateRenderInstanceBufferRange = (MyRenderMessageUpdateRenderInstanceBufferRange)message;
				InstancingId instancingId = MyInstancing.Get(myRenderMessageUpdateRenderInstanceBufferRange.ID);
				if (instancingId != InstancingId.NULL)
				{
					MyInstancing.UpdateGeneric(instancingId, myRenderMessageUpdateRenderInstanceBufferRange.InstanceData, myRenderMessageUpdateRenderInstanceBufferRange.InstanceData.Length);
				}
				break;
			}
			case MyRenderMessageEnum.UpdateRenderCubeInstanceBuffer:
			{
				MyRenderMessageUpdateRenderCubeInstanceBuffer myRenderMessageUpdateRenderCubeInstanceBuffer = (MyRenderMessageUpdateRenderCubeInstanceBuffer)message;
				InstancingId instancingId2 = MyInstancing.Get(myRenderMessageUpdateRenderCubeInstanceBuffer.ID);
				if (instancingId2 != InstancingId.NULL)
				{
					MyInstancing.UpdateCube(instancingId2, myRenderMessageUpdateRenderCubeInstanceBuffer.InstanceData, myRenderMessageUpdateRenderCubeInstanceBuffer.DecalsData, myRenderMessageUpdateRenderCubeInstanceBuffer.Capacity);
				}
				break;
			}
			case MyRenderMessageEnum.SetInstanceBuffer:
			{
				MyRenderMessageSetInstanceBuffer myRenderMessageSetInstanceBuffer = (MyRenderMessageSetInstanceBuffer)message;
<<<<<<< HEAD
				MyActor myActor16 = MyIDTracker<MyActor>.FindByID(myRenderMessageSetInstanceBuffer.ID);
				if (myActor16 != null)
				{
					if (myActor16.GetRenderable() != null)
					{
						myActor16.GetRenderable().SetInstancing(MyInstancing.Get(myRenderMessageSetInstanceBuffer.InstanceBufferId));
						myActor16.SetLocalAabb(myRenderMessageSetInstanceBuffer.LocalAabb);
						myActor16.GetRenderable().SetInstancingCounters(myRenderMessageSetInstanceBuffer.InstanceCount, myRenderMessageSetInstanceBuffer.InstanceStart);
					}
					else if (myActor16.GetInstance() != null)
					{
						MyComponentConverter.ConvertActorToTheOldPipeline(myActor16);
						myActor16.GetRenderable().SetInstancing(MyInstancing.Get(myRenderMessageSetInstanceBuffer.InstanceBufferId));
						myActor16.SetLocalAabb(myRenderMessageSetInstanceBuffer.LocalAabb);
						myActor16.GetRenderable().SetInstancingCounters(myRenderMessageSetInstanceBuffer.InstanceCount, myRenderMessageSetInstanceBuffer.InstanceStart);
=======
				MyActor myActor17 = MyIDTracker<MyActor>.FindByID(myRenderMessageSetInstanceBuffer.ID);
				if (myActor17 != null)
				{
					if (myActor17.GetRenderable() != null)
					{
						myActor17.GetRenderable().SetInstancing(MyInstancing.Get(myRenderMessageSetInstanceBuffer.InstanceBufferId));
						myActor17.SetLocalAabb(myRenderMessageSetInstanceBuffer.LocalAabb);
						myActor17.GetRenderable().SetInstancingCounters(myRenderMessageSetInstanceBuffer.InstanceCount, myRenderMessageSetInstanceBuffer.InstanceStart);
					}
					else if (myActor17.GetInstance() != null)
					{
						MyComponentConverter.ConvertActorToTheOldPipeline(myActor17);
						myActor17.GetRenderable().SetInstancing(MyInstancing.Get(myRenderMessageSetInstanceBuffer.InstanceBufferId));
						myActor17.SetLocalAabb(myRenderMessageSetInstanceBuffer.LocalAabb);
						myActor17.GetRenderable().SetInstancingCounters(myRenderMessageSetInstanceBuffer.InstanceCount, myRenderMessageSetInstanceBuffer.InstanceStart);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					else
					{
						MyRenderProxy.Error("Unresolved condition");
					}
				}
				break;
			}
			case MyRenderMessageEnum.UpdateLodImmediately:
				MyIDTracker<MyActor>.FindByID(((MyRenderMessageUpdateLodImmediately)message).Id)?.GetRenderable()?.SkipNextLodTransition();
				break;
			case MyRenderMessageEnum.CreateManualCullObject:
			{
				MyRenderMessageCreateManualCullObject myRenderMessageCreateManualCullObject = (MyRenderMessageCreateManualCullObject)message;
				MyActor myActor15 = MyActorFactory.CreateRoot(myRenderMessageCreateManualCullObject.DebugName);
				myActor15.SetID(myRenderMessageCreateManualCullObject.ID);
				myActor15.SetMatrix(ref myRenderMessageCreateManualCullObject.WorldMatrix);
				myActor15.SetRoot(state: true);
				break;
			}
			case MyRenderMessageEnum.SetParentCullObject:
			{
				MyRenderMessageSetParentCullObject myRenderMessageSetParentCullObject = (MyRenderMessageSetParentCullObject)message;
<<<<<<< HEAD
				MyActor myActor12 = MyIDTracker<MyActor>.FindByID(myRenderMessageSetParentCullObject.ID);
				MyActor myActor13 = MyIDTracker<MyActor>.FindByID(myRenderMessageSetParentCullObject.CullObjectID);
				if (myActor12 != null && myActor13 != null && myActor13.IsRoot())
				{
					MyScene11.Instance.SetActorParent(myActor13, myActor12, myRenderMessageSetParentCullObject.ChildToParent);
=======
				MyActor myActor11 = MyIDTracker<MyActor>.FindByID(myRenderMessageSetParentCullObject.ID);
				MyActor myActor12 = MyIDTracker<MyActor>.FindByID(myRenderMessageSetParentCullObject.CullObjectID);
				if (myActor11 != null && myActor12 != null && myActor12.IsRoot())
				{
					MyScene11.Instance.SetActorParent(myActor12, myActor11, myRenderMessageSetParentCullObject.ChildToParent);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				break;
			}
			case MyRenderMessageEnum.CreateLineBasedObject:
			{
				MyRenderMessageCreateLineBasedObject myRenderMessageCreateLineBasedObject = (MyRenderMessageCreateLineBasedObject)message;
<<<<<<< HEAD
				MyActor myActor11 = MyActorFactory.CreateSceneObject(myRenderMessageCreateLineBasedObject.DebugName);
				myActor11.SetID(myRenderMessageCreateLineBasedObject.ID);
				myActor11.SetMatrix(ref MatrixD.Identity);
				MyMeshMaterials1.GetMaterialId("__ROPE_MATERIAL", null, myRenderMessageCreateLineBasedObject.ColorMetalTexture, myRenderMessageCreateLineBasedObject.NormalGlossTexture, myRenderMessageCreateLineBasedObject.ExtensionTexture, MyMeshDrawTechnique.MESH);
				myActor11.GetRenderable().SetModel(MyMeshes.CreateRuntimeMesh(X.TEXT_("LINE" + myRenderMessageCreateLineBasedObject.ID), 1, dynamic: true));
=======
				MyActor myActor10 = MyActorFactory.CreateSceneObject(myRenderMessageCreateLineBasedObject.DebugName);
				myActor10.SetID(myRenderMessageCreateLineBasedObject.ID);
				myActor10.SetMatrix(ref MatrixD.Identity);
				MyMeshMaterials1.GetMaterialId("__ROPE_MATERIAL", null, myRenderMessageCreateLineBasedObject.ColorMetalTexture, myRenderMessageCreateLineBasedObject.NormalGlossTexture, myRenderMessageCreateLineBasedObject.ExtensionTexture, MyMeshDrawTechnique.MESH);
				myActor10.GetRenderable().SetModel(MyMeshes.CreateRuntimeMesh(X.TEXT_("LINE" + myRenderMessageCreateLineBasedObject.ID), 1, dynamic: true));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				break;
			}
			case MyRenderMessageEnum.UpdateLineBasedObject:
			{
				MyRenderMessageUpdateLineBasedObject myRenderMessageUpdateLineBasedObject = (MyRenderMessageUpdateLineBasedObject)message;
				MyActor myActor6 = MyIDTracker<MyActor>.FindByID(myRenderMessageUpdateLineBasedObject.ID);
				if (myActor6 != null)
				{
					MyLineHelpers.GenerateVertexData(ref myRenderMessageUpdateLineBasedObject.WorldPointA, ref myRenderMessageUpdateLineBasedObject.WorldPointB, out var stream, out var stream2);
					ushort[] array5 = MyLineHelpers.GenerateIndices(stream.Length);
					MyRuntimeSectionInfo[] sections = new MyRuntimeSectionInfo[1]
					{
						new MyRuntimeSectionInfo
						{
							TriCount = array5.Length / 3,
							IndexStart = 0,
							MaterialName = "__ROPE_MATERIAL"
						}
					};
					MyMeshes.UpdateRuntimeMesh(MyMeshes.GetMeshId(X.TEXT_("LINE" + myRenderMessageUpdateLineBasedObject.ID), 1f, DeferStateChangeBatch), array5, stream, stream2, sections, (BoundingBox)MyLineHelpers.GetBoundingBox(ref myRenderMessageUpdateLineBasedObject.WorldPointA, ref myRenderMessageUpdateLineBasedObject.WorldPointB));
					myActor6.GetRenderable()?.MarkDirty();
					MatrixD matrix = MatrixD.CreateTranslation((Vector3)(myRenderMessageUpdateLineBasedObject.WorldPointA + myRenderMessageUpdateLineBasedObject.WorldPointB) * 0.5f);
					myActor6.SetMatrix(ref matrix);
				}
				break;
			}
			case MyRenderMessageEnum.SetRenderEntityData:
				_ = (MyRenderMessageSetRenderEntityData)message;
				MyRenderProxy.Error("MyRenderMessageSetRenderEntityData is deprecated!");
				break;
			case MyRenderMessageEnum.AddRuntimeModel:
			{
				MyRenderMessageAddRuntimeModel myRenderMessageAddRuntimeModel = (MyRenderMessageAddRuntimeModel)message;
				if (!MyMeshes.Exists(myRenderMessageAddRuntimeModel.Name))
				{
					ushort[] array = new ushort[myRenderMessageAddRuntimeModel.ModelData.Indices.Count];
					for (int i = 0; i < myRenderMessageAddRuntimeModel.ModelData.Indices.Count; i++)
					{
						array[i] = (ushort)myRenderMessageAddRuntimeModel.ModelData.Indices[i];
					}
					int count = myRenderMessageAddRuntimeModel.ModelData.Positions.Count;
					MyVertexFormatPositionH4[] array2 = new MyVertexFormatPositionH4[count];
					MyVertexFormatTexcoordNormalTangentTexindices[] array3 = new MyVertexFormatTexcoordNormalTangentTexindices[count];
					Vector4I[] array4 = MyManagers.GeometryTextureSystem.CreateTextureIndices(myRenderMessageAddRuntimeModel.ModelData.Sections, myRenderMessageAddRuntimeModel.ModelData.Indices, myRenderMessageAddRuntimeModel.ModelData.Positions.Count);
					for (int j = 0; j < count; j++)
					{
						array2[j] = new MyVertexFormatPositionH4(myRenderMessageAddRuntimeModel.ModelData.Positions[j]);
						array3[j] = new MyVertexFormatTexcoordNormalTangentTexindices(myRenderMessageAddRuntimeModel.ModelData.TexCoords[j], myRenderMessageAddRuntimeModel.ModelData.Normals[j], myRenderMessageAddRuntimeModel.ModelData.Tangents[j], (Byte4)array4[j]);
					}
					MyMeshes.UpdateRuntimeMesh(MyMeshes.CreateRuntimeMesh(X.TEXT_(myRenderMessageAddRuntimeModel.Name), myRenderMessageAddRuntimeModel.ModelData.Sections.Count, dynamic: false), array, array2, array3, myRenderMessageAddRuntimeModel.ModelData.Sections.ToArray(), myRenderMessageAddRuntimeModel.ModelData.AABB);
					if (myRenderMessageAddRuntimeModel.ReplacedModel != null)
					{
						MyAssetsLoader.ModelRemap[myRenderMessageAddRuntimeModel.Name] = myRenderMessageAddRuntimeModel.ReplacedModel;
					}
				}
				break;
			}
			case MyRenderMessageEnum.UpdateModelProperties:
			{
				MyRenderMessageUpdateModelProperties myRenderMessageUpdateModelProperties = (MyRenderMessageUpdateModelProperties)message;
				MyActor myActor19 = MyIDTracker<MyActor>.FindByID(myRenderMessageUpdateModelProperties.ID);
				if (myActor19 == null)
				{
					break;
				}
				string materialName = myRenderMessageUpdateModelProperties.MaterialName;
				MyEntityMaterialKey myEntityMaterialKey = new MyEntityMaterialKey(materialName);
				if (myRenderMessageUpdateModelProperties.FlagsChange.HasValue)
				{
					if (myActor19.GetInstance() != null && myActor19.GetInstance().IsUsedMaterialWithinModel(materialName))
					{
						MyComponentConverter.ConvertActorToTheOldPipeline(myActor19);
					}
					MyScene11.AddMaterialRenderFlagChange(myRenderMessageUpdateModelProperties.ID, myEntityMaterialKey, myRenderMessageUpdateModelProperties.FlagsChange.Value);
				}
				if (myActor19.GetRenderable() != null)
				{
					myActor19.GetRenderable().SetModelProperties(myEntityMaterialKey, myRenderMessageUpdateModelProperties.Emissivity, myRenderMessageUpdateModelProperties.DiffuseColor);
				}
				else if (myActor19.GetInstance() != null)
				{
					myActor19.GetInstance().SetInstanceMaterial(materialName, myRenderMessageUpdateModelProperties.Emissivity, myRenderMessageUpdateModelProperties.DiffuseColor);
				}
				else
				{
					MyRenderProxy.Error("Unresolved condition");
				}
				break;
			}
			case MyRenderMessageEnum.UpdateModelHighlight:
			{
				MyRenderMessageUpdateModelHighlight myRenderMessageUpdateModelHighlight = (MyRenderMessageUpdateModelHighlight)message;
				if (MyIDTracker<MyActor>.FindByID(myRenderMessageUpdateModelHighlight.ID) == null)
				{
					break;
				}
				uint[] subpartIndices;
				if (myRenderMessageUpdateModelHighlight.Thickness > 0f)
				{
					MyHighlight.AddObjects(myRenderMessageUpdateModelHighlight.ID, myRenderMessageUpdateModelHighlight.SectionNames, myRenderMessageUpdateModelHighlight.OutlineColor, myRenderMessageUpdateModelHighlight.Thickness, myRenderMessageUpdateModelHighlight.PulseTimeInSeconds, myRenderMessageUpdateModelHighlight.InstanceIndex);
					if (myRenderMessageUpdateModelHighlight.SubpartIndices == null)
					{
						break;
					}
					subpartIndices = myRenderMessageUpdateModelHighlight.SubpartIndices;
					foreach (uint num5 in subpartIndices)
					{
						if (MyIDTracker<MyActor>.FindByID(num5) != null)
						{
							MyHighlight.AddObjects(num5, null, myRenderMessageUpdateModelHighlight.OutlineColor, myRenderMessageUpdateModelHighlight.Thickness, myRenderMessageUpdateModelHighlight.PulseTimeInSeconds, -1);
						}
					}
					break;
				}
				MyHighlight.RemoveObjects(myRenderMessageUpdateModelHighlight.ID, myRenderMessageUpdateModelHighlight.SectionNames);
				if (myRenderMessageUpdateModelHighlight.SubpartIndices == null)
				{
					break;
				}
				subpartIndices = myRenderMessageUpdateModelHighlight.SubpartIndices;
				foreach (uint num6 in subpartIndices)
				{
					if (MyIDTracker<MyActor>.FindByID(num6) != null)
					{
						MyHighlight.RemoveObjects(num6, null);
					}
				}
				break;
			}
			case MyRenderMessageEnum.UpdateOverlappingModelsForHighlight:
			{
				MyRenderMessageUpdateOverlappingModelsForHighlight myRenderMessageUpdateOverlappingModelsForHighlight = (MyRenderMessageUpdateOverlappingModelsForHighlight)message;
				if (myRenderMessageUpdateOverlappingModelsForHighlight.Enable)
				{
					MyHighlight.AddOverlappingModel(myRenderMessageUpdateOverlappingModelsForHighlight.OverlappingModelID);
				}
				else
				{
					MyHighlight.RemoveOverlappingModel(myRenderMessageUpdateOverlappingModelsForHighlight.OverlappingModelID);
				}
				break;
			}
			case MyRenderMessageEnum.UpdateColorEmissivity:
			{
				MyRenderMessageUpdateColorEmissivity myRenderMessageUpdateColorEmissivity = (MyRenderMessageUpdateColorEmissivity)message;
<<<<<<< HEAD
				MyActor myActor14 = MyIDTracker<MyActor>.FindByID(myRenderMessageUpdateColorEmissivity.ID);
				if (myActor14 != null)
				{
					if (myActor14.GetRenderable() != null)
					{
						myActor14.GetRenderable().UpdateColorEmissivity(myRenderMessageUpdateColorEmissivity.LOD, myRenderMessageUpdateColorEmissivity.MaterialName, myRenderMessageUpdateColorEmissivity.DiffuseColor, myRenderMessageUpdateColorEmissivity.Emissivity);
					}
					else if (myActor14.GetInstance() != null)
=======
				MyActor myActor13 = MyIDTracker<MyActor>.FindByID(myRenderMessageUpdateColorEmissivity.ID);
				if (myActor13 != null)
				{
					if (myActor13.GetRenderable() != null)
					{
						myActor13.GetRenderable().UpdateColorEmissivity(myRenderMessageUpdateColorEmissivity.LOD, myRenderMessageUpdateColorEmissivity.MaterialName, myRenderMessageUpdateColorEmissivity.DiffuseColor, myRenderMessageUpdateColorEmissivity.Emissivity);
					}
					else if (myActor13.GetInstance() != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						MyInstanceMaterial myInstanceMaterial = default(MyInstanceMaterial);
						myInstanceMaterial.ColorMult = myRenderMessageUpdateColorEmissivity.DiffuseColor;
						myInstanceMaterial.Emissivity = myRenderMessageUpdateColorEmissivity.Emissivity;
						MyInstanceMaterial instanceMaterial = myInstanceMaterial;
<<<<<<< HEAD
						myActor14.GetInstance().SetInstanceMaterial(myRenderMessageUpdateColorEmissivity.MaterialName, instanceMaterial);
=======
						myActor13.GetInstance().SetInstanceMaterial(myRenderMessageUpdateColorEmissivity.MaterialName, instanceMaterial);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
					else
					{
						MyRenderProxy.Error("Unresolved condition");
					}
				}
				break;
			}
			case MyRenderMessageEnum.PreloadModel:
			{
				MyRenderMessagePreloadModel myRenderMessagePreloadModel = (MyRenderMessagePreloadModel)message;
				if (myRenderMessagePreloadModel.ForceOldPipeline)
				{
					MyMeshes.GetMeshId(X.TEXT_(myRenderMessagePreloadModel.Name), myRenderMessagePreloadModel.Rescale, DeferStateChangeBatch);
					break;
				}
				string filepath = MyAssetsLoader.ModelRemap.Get(myRenderMessagePreloadModel.Name, myRenderMessagePreloadModel.Name);
				MyManagers.ModelFactory.GetOrCreateModels(filepath);
				break;
			}
			case MyRenderMessageEnum.ChangeMaterialTexture:
			{
				MyRenderMessageChangeMaterialTexture rMessage2 = (MyRenderMessageChangeMaterialTexture)message;
				MyManagers.TextureChangeManager.ChangeMaterialTexture(rMessage2);
				break;
			}
			case MyRenderMessageEnum.RenderOffscreenTexture:
				MyOffscreenRenderer.Add((MyRenderMessageRenderOffscreenTexture)message);
				break;
			case MyRenderMessageEnum.PreloadMaterials:
				MyMeshes.GetMeshId(X.TEXT_(((MyRenderMessagePreloadMaterials)message).Name), 1f, DeferStateChangeBatch);
				break;
			case MyRenderMessageEnum.VoxelCreate:
			{
				MyRenderMessageVoxelCreate myRenderMessageVoxelCreate = (MyRenderMessageVoxelCreate)message;
				MyRenderVoxelActor myRenderVoxelActor = MyComponentFactory<MyRenderVoxelActor>.Create();
<<<<<<< HEAD
				MyActor myActor10 = MyActorFactory.Create(myRenderMessageVoxelCreate.DebugName);
				myActor10.SetID(myRenderMessageVoxelCreate.Id);
				myActor10.AddComponent<MyRenderVoxelActor>(myRenderVoxelActor);
=======
				MyActor myActor9 = MyActorFactory.Create(myRenderMessageVoxelCreate.DebugName);
				myActor9.SetID(myRenderMessageVoxelCreate.Id);
				myActor9.AddComponent<MyRenderVoxelActor>(myRenderVoxelActor);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				myRenderVoxelActor.Init(myRenderMessageVoxelCreate.Size, myRenderMessageVoxelCreate.Clipmap);
				myRenderMessageVoxelCreate.Clipmap.VoxelRenderDataProcessorProvider = Singleton<MyMeshes.MyVoxelRenderDataProcessorProvider>.Instance;
				myRenderVoxelActor.SpherizeCenter = myRenderMessageVoxelCreate.SpherizePosition;
				myRenderVoxelActor.SpherizeRadius = myRenderMessageVoxelCreate.SpherizeRadius;
				myRenderVoxelActor.RenderFlags = MyProxiesFactory.GetRenderableProxyFlags(myRenderMessageVoxelCreate.RenderFlags);
				myRenderVoxelActor.SetDithering(myRenderMessageVoxelCreate.Dithering);
<<<<<<< HEAD
				myActor10.SetMatrix(ref myRenderMessageVoxelCreate.WorldMatrix);
=======
				myActor9.SetMatrix(ref myRenderMessageVoxelCreate.WorldMatrix);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				break;
			}
			case MyRenderMessageEnum.CreateRenderVoxelMaterials:
			{
<<<<<<< HEAD
				MyRenderMessageCreateRenderVoxelMaterials obj4 = (MyRenderMessageCreateRenderVoxelMaterials)message;
				MyVoxelMaterials.Set(obj4.Materials);
				obj4.Materials = null;
=======
				MyRenderMessageCreateRenderVoxelMaterials obj3 = (MyRenderMessageCreateRenderVoxelMaterials)message;
				MyVoxelMaterials.Set(obj3.Materials);
				obj3.Materials = null;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				break;
			}
			case MyRenderMessageEnum.PreloadVoxelMaterials:
				MyVoxelMaterials.Preload(((MyRenderMessagePreloadVoxelMaterials)message).Materials);
				break;
			case MyRenderMessageEnum.UpdateRenderVoxelMaterials:
			{
<<<<<<< HEAD
				MyRenderMessageUpdateRenderVoxelMaterials obj3 = (MyRenderMessageUpdateRenderVoxelMaterials)message;
				MyVoxelMaterials.Set(obj3.Materials, update: true);
				obj3.Materials = null;
=======
				MyRenderMessageUpdateRenderVoxelMaterials obj2 = (MyRenderMessageUpdateRenderVoxelMaterials)message;
				MyVoxelMaterials.Set(obj2.Materials, update: true);
				obj2.Materials = null;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				break;
			}
			case MyRenderMessageEnum.CreateRenderLight:
			{
				MyRenderMessageCreateRenderLight myRenderMessageCreateRenderLight = (MyRenderMessageCreateRenderLight)message;
				MyActorFactory.CreateLight(myRenderMessageCreateRenderLight.DebugName).SetID(myRenderMessageCreateRenderLight.ID);
				break;
			}
			case MyRenderMessageEnum.UpdateRenderLight:
			{
				MyRenderMessageUpdateRenderLight myRenderMessageUpdateRenderLight = (MyRenderMessageUpdateRenderLight)message;
				MyActor myActor8 = MyIDTracker<MyActor>.FindByID(myRenderMessageUpdateRenderLight.Data.ID);
				if (myActor8 != null && myActor8.GetLight() != null)
				{
					myActor8.GetLight().UpdateData(ref myRenderMessageUpdateRenderLight.Data);
				}
				break;
			}
			case MyRenderMessageEnum.SetLightShadowIgnore:
			{
				MyRenderMessageSetLightShadowIgnore myRenderMessageSetLightShadowIgnore = (MyRenderMessageSetLightShadowIgnore)message;
				MyActor myActor5 = MyIDTracker<MyActor>.FindByID(myRenderMessageSetLightShadowIgnore.ID);
				if (myActor5 != null && myActor5.GetLight() != null)
				{
					myActor5.GetLight().IgnoreShadowForEntity(myRenderMessageSetLightShadowIgnore.ID2);
				}
				break;
			}
			case MyRenderMessageEnum.ClearLightShadowIgnore:
			{
				MyActor myActor4 = MyIDTracker<MyActor>.FindByID(((MyRenderMessageClearLightShadowIgnore)message).ID);
				if (myActor4 != null && myActor4.GetLight() != null)
				{
					myActor4.GetLight().ClearIgnoredEntities();
				}
				break;
			}
			case MyRenderMessageEnum.UpdateShadowSettings:
			{
				MyRenderMessageUpdateShadowSettings myRenderMessageUpdateShadowSettings = (MyRenderMessageUpdateShadowSettings)message;
				MyShadowCascades.Settings.CopyFrom(myRenderMessageUpdateShadowSettings.Settings);
				break;
			}
			case MyRenderMessageEnum.UpdateNewLoddingSettings:
			{
				MyNewLoddingSettings settings = ((MyRenderMessageUpdateNewLoddingSettings)message).Settings;
				MyCommon.LoddingSettings.CopyFrom(settings);
				MyManagers.GeometryRenderer.IsLodUpdateEnabled = settings.Global.IsUpdateEnabled;
				MyManagers.GeometryRenderer.SetLoddingSetting(ref settings);
				MyLodStrategy.SetSettings(ref settings.Global);
				MyManagers.ModelFactory.OnLoddingSettingChanged();
				break;
			}
			case MyRenderMessageEnum.UpdateFogSettings:
			{
				MyRenderMessageUpdateFogSettings myRenderMessageUpdateFogSettings = (MyRenderMessageUpdateFogSettings)message;
				if (m_debugOverrides.Fog)
				{
					Environment.Fog = myRenderMessageUpdateFogSettings.Settings;
				}
				else
				{
					Environment.Fog.FogDensity = (Environment.Fog.FogSkybox = (Environment.Fog.FogAtmo = 0f));
				}
				break;
			}
			case MyRenderMessageEnum.UpdatePlanetSettings:
			{
				MyRenderMessageUpdatePlanetSettings myRenderMessageUpdatePlanetSettings = (MyRenderMessageUpdatePlanetSettings)message;
				Environment.Planet = myRenderMessageUpdatePlanetSettings.Settings;
				break;
			}
			case MyRenderMessageEnum.UpdateAtmosphereSettings:
			{
				MyRenderMessageUpdateAtmosphereSettings myRenderMessageUpdateAtmosphereSettings = (MyRenderMessageUpdateAtmosphereSettings)message;
				MyAtmosphereRenderer.UpdateSettings(myRenderMessageUpdateAtmosphereSettings.ID, myRenderMessageUpdateAtmosphereSettings.Settings);
				break;
			}
			case MyRenderMessageEnum.EnableAtmosphere:
				MyAtmosphereRenderer.Enabled = ((MyRenderMessageEnableAtmosphere)message).Enabled;
				break;
			case MyRenderMessageEnum.UpdateCloudLayerFogFlag:
				MyCloudRenderer.DrawFog = ((MyRenderMessageUpdateCloudLayerFogFlag)message).ShouldDrawFog;
				break;
			case MyRenderMessageEnum.UpdateRenderEnvironment:
			{
				MyRenderMessageUpdateRenderEnvironment myRenderMessageUpdateRenderEnvironment = (MyRenderMessageUpdateRenderEnvironment)message;
				if (!string.IsNullOrEmpty(myRenderMessageUpdateRenderEnvironment.Data.Skybox))
				{
					Environment.Data = myRenderMessageUpdateRenderEnvironment.Data;
					Environment.SkyboxIndirect = Path.GetDirectoryName(myRenderMessageUpdateRenderEnvironment.Data.Skybox) + "\\" + Path.GetFileNameWithoutExtension(myRenderMessageUpdateRenderEnvironment.Data.Skybox) + "Indirect" + Path.GetExtension(myRenderMessageUpdateRenderEnvironment.Data.Skybox);
					m_resetEyeAdaptation |= myRenderMessageUpdateRenderEnvironment.ResetEyeAdaptation;
				}
				break;
			}
			case MyRenderMessageEnum.UpdateDebugOverrides:
				m_debugOverrides = ((MyRenderMessageUpdateDebugOverrides)message).Overrides;
				break;
			case MyRenderMessageEnum.UpdatePostprocessSettings:
			{
				MyRenderMessageUpdatePostprocessSettings myRenderMessageUpdatePostprocessSettings = (MyRenderMessageUpdatePostprocessSettings)message;
				Postprocess = myRenderMessageUpdatePostprocessSettings.Settings;
				if (Postprocess.EnableEyeAdaptation != myRenderMessageUpdatePostprocessSettings.Settings.EnableEyeAdaptation)
				{
					m_resetEyeAdaptation = true;
				}
				break;
			}
			case MyRenderMessageEnum.UpdateSSAOSettings:
				MySSAO.Params = ((MyRenderMessageUpdateSSAOSettings)message).Settings;
				break;
			case MyRenderMessageEnum.UpdateHBAO:
				MyHBAO.Params = ((MyRenderMessageUpdateHBAO)message).Settings;
				break;
			case MyRenderMessageEnum.DrawSprite:
			case MyRenderMessageEnum.DrawSpriteExt:
			case MyRenderMessageEnum.DrawSpriteAtlas:
			case MyRenderMessageEnum.DrawVideo:
			case MyRenderMessageEnum.DrawString:
			case MyRenderMessageEnum.DrawStringAligned:
			case MyRenderMessageEnum.SpriteScissorPush:
			case MyRenderMessageEnum.SpriteScissorPop:
				MyManagers.SpritesManager.AddMessage(message, frameId);
				break;
			case MyRenderMessageEnum.SpriteMainViewportScale:
				SetMainViewportScale(((MyRenderMessageSpriteMainViewportScale)message).ScaleFactor);
				break;
			case MyRenderMessageEnum.CreateFont:
			{
				MyRenderMessageCreateFont myRenderMessageCreateFont = (MyRenderMessageCreateFont)message;
				Color colorMask = myRenderMessageCreateFont.ColorMask ?? Color.White;
				LoadFont(myRenderMessageCreateFont.FontId, myRenderMessageCreateFont.IsDebugFont, myRenderMessageCreateFont.FontPath, colorMask);
				break;
			}
			case MyRenderMessageEnum.PreloadTextures:
			{
				MyRenderMessagePreloadTextures preloadMsg = (MyRenderMessagePreloadTextures)message;
				MyFileTextureEnum textureType = preloadMsg.TextureType.ToFileTextureEnum();
				MyTextureStreamingManager.QueryArgs queryArgs = new MyTextureStreamingManager.QueryArgs
				{
					TextureType = textureType
				};
				Parallel.For(0, preloadMsg.Files.Count, delegate(int x)
				{
					MyManagers.Textures.GetTexture(preloadMsg.Files[x], queryArgs).Touch(100);
				}, 50, WorkPriority.VeryHigh, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.AssetLoad, "PreloadTextures"), blocking: true);
				break;
			}
			case MyRenderMessageEnum.UnloadTextures:
				foreach (string file in ((MyRenderMessageUnloadTextures)message).Files)
				{
					MyManagers.Textures.UnloadTexture(file);
				}
				break;
			case MyRenderMessageEnum.DeprioritizeTextures:
				foreach (string file2 in ((MyRenderMessageDeprioritizeTextures)message).Files)
				{
					MyManagers.Textures.DeprioritizeTexture(file2);
				}
				break;
			case MyRenderMessageEnum.AddToParticleTextureArray:
				MyGPUEmitters.AddToParticleTextureArray((message as MyRenderMessageAddToParticleTextureArray).Files);
				break;
			case MyRenderMessageEnum.UnloadTexture:
			{
				MyRenderMessageUnloadTexture myRenderMessageUnloadTexture = (MyRenderMessageUnloadTexture)message;
				MyManagers.Textures.UnloadTexture(myRenderMessageUnloadTexture.Texture);
				break;
			}
			case MyRenderMessageEnum.CreateGeneratedTexture:
			{
				MyRenderMessageCreateGeneratedTexture myRenderMessageCreateGeneratedTexture = (MyRenderMessageCreateGeneratedTexture)message;
				MyManagers.FileTextures.CreateGeneratedTexture(myRenderMessageCreateGeneratedTexture.TextureName, myRenderMessageCreateGeneratedTexture.Width, myRenderMessageCreateGeneratedTexture.Height, myRenderMessageCreateGeneratedTexture.Type, myRenderMessageCreateGeneratedTexture.GenerateMipMaps, myRenderMessageCreateGeneratedTexture.Data, myRenderMessageCreateGeneratedTexture.ImmediatelyReady);
				break;
			}
			case MyRenderMessageEnum.DestroyGeneratedTexture:
			{
				MyRenderMessageDestroyGeneratedTexture myRenderMessageDestroyGeneratedTexture = (MyRenderMessageDestroyGeneratedTexture)message;
				MyManagers.FileTextures.DestroyGeneratedTexture(myRenderMessageDestroyGeneratedTexture.TextureName);
				break;
			}
			case MyRenderMessageEnum.ResetGeneratedTexture:
			{
				MyRenderMessageResetGeneratedTexture myRenderMessageResetGeneratedTexture = (MyRenderMessageResetGeneratedTexture)message;
				MyManagers.FileTextures.ResetGeneratedTexture(myRenderMessageResetGeneratedTexture.TextureName, myRenderMessageResetGeneratedTexture.Data);
				break;
			}
			case MyRenderMessageEnum.ReloadTextures:
				_ = (MyRenderMessageReloadTextures)message;
				MyVoxelMaterials.InvalidateMaterials();
				MyMeshMaterials1.InvalidateMaterials();
				MyManagers.FileTextures.ReloadTextures(MyFileTextureManager.MyFileTextureHelper.IsAssetTextureFilter);
				MyManagers.DynamicFileArrayTextures.ReloadAll();
				MyGPUEmitters.ReloadTextures();
				ReloadFonts();
				break;
			case MyRenderMessageEnum.ReloadModels:
				_ = (MyRenderMessageReloadModels)message;
				MyAssetsLoader.ReloadMeshes();
				MyRenderableComponent.MarkAllDirty();
				MyManagers.ModelFactory.ReloadModels();
				MyManagers.Instances.OnReloadModels();
				MyManagers.StaticGroups.OnReloadModels();
				break;
			case MyRenderMessageEnum.TakeScreenshot:
			{
				MyRenderMessageTakeScreenshot myRenderMessageTakeScreenshot = (MyRenderMessageTakeScreenshot)message;
				m_screenshot = new MyScreenshot(myRenderMessageTakeScreenshot.PathToSave, myRenderMessageTakeScreenshot.SizeMultiplier, myRenderMessageTakeScreenshot.IgnoreSprites, myRenderMessageTakeScreenshot.ShowNotification);
				MyManagers.Textures.UnloadTexture(myRenderMessageTakeScreenshot.PathToSave);
				break;
			}
			case MyRenderMessageEnum.ReloadEffects:
				MyShaders.Recompile();
				MyMaterialShaders.Recompile();
				MyAtmosphereRenderer.RecomputeAtmospheres();
				MyRenderableComponent.MarkAllDirty();
				MyManagers.FoliageManager.Reset();
				break;
			case MyRenderMessageEnum.PauseTimer:
				MyCommon.IsPaused = ((MyRenderMessagePauseTimer)message).pause;
				break;
			case MyRenderMessageEnum.PlayVideo:
			{
				MyRenderMessagePlayVideo rMessage = (MyRenderMessagePlayVideo)message;
				MyVideoFactory.Create(rMessage.ID);
				Parallel.Start(delegate
				{
					MyVideoFactory.Play(rMessage.ID, rMessage.VideoFile, rMessage.Volume);
				}, Parallel.DefaultOptions.WithDebugInfo(MyProfiler.TaskType.GUI, "StartVideo"), WorkPriority.VeryHigh);
				break;
			}
			case MyRenderMessageEnum.CloseVideo:
			{
<<<<<<< HEAD
				MyRenderMessageCloseVideo obj2 = (MyRenderMessageCloseVideo)message;
				MyVideoFactory.Remove(obj2.ID);
				MyRenderProxy.RemoveMessageId(obj2.ID, MyRenderProxy.ObjectType.Video);
=======
				MyRenderMessageCloseVideo obj = (MyRenderMessageCloseVideo)message;
				MyVideoFactory.Remove(obj.ID);
				MyRenderProxy.RemoveMessageId(obj.ID, MyRenderProxy.ObjectType.Video);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				break;
			}
			case MyRenderMessageEnum.UpdateGameplayFrame:
				GameplayFrameCounter = ((MyRenderMessageUpdateGameplayFrame)message).GameplayFrame;
				break;
			case MyRenderMessageEnum.UpdateVideo:
				MyVideoFactory.GetVideo(((MyRenderMessageUpdateVideo)message).ID)?.Update();
				break;
			case MyRenderMessageEnum.SetVideoVolume:
			{
				MyRenderMessageSetVideoVolume myRenderMessageSetVideoVolume = (MyRenderMessageSetVideoVolume)message;
				MyVideoPlayer video = MyVideoFactory.GetVideo(myRenderMessageSetVideoVolume.ID);
				if (video != null)
				{
					video.Volume = myRenderMessageSetVideoVolume.Volume;
				}
				break;
			}
			case MyRenderMessageEnum.VideoAdaptersRequest:
				MyRenderProxy.SendVideoAdapters(GetAdaptersList());
				break;
			case MyRenderMessageEnum.SwitchDeviceSettings:
				MyRenderProxy.RenderThread.SwitchSettings((message as MyRenderMessageSwitchDeviceSettings).Settings);
				break;
			case MyRenderMessageEnum.SwitchRenderSettings:
				UpdateRenderSettings(((MyRenderMessageSwitchRenderSettings)message).Settings);
				break;
			case MyRenderMessageEnum.UnloadData:
				OnSessionEnd();
				OnSessionStart();
				break;
			case MyRenderMessageEnum.CollectGarbage:
				GC.Collect();
				break;
			case MyRenderMessageEnum.SetFrameTimeStep:
				MyCommon.SetConstFrameTimeDelta((message as MyRenderMessageSetFrameTimeStep).TimeStepInSeconds);
				break;
			case MyRenderMessageEnum.ResetRandomness:
				MyCommon.SetRandomSeed((message as MyRenderMessageResetRandomness).Seed);
				break;
			case MyRenderMessageEnum.RenderColoredTexture:
			{
				MyRenderMessageRenderColoredTexture myRenderMessageRenderColoredTexture = (MyRenderMessageRenderColoredTexture)message;
				m_texturesToRender.AddRange(myRenderMessageRenderColoredTexture.texturesToRender);
				break;
			}
			case MyRenderMessageEnum.PreloadModels:
			{
				MyRenderMessagePreloadModels myRenderMessagePreloadModels = message as MyRenderMessagePreloadModels;
				if (myRenderMessagePreloadModels != null)
				{
					if (myRenderMessagePreloadModels.ForInstancedComponent)
					{
						MyManagers.ModelFactory.AddModels(myRenderMessagePreloadModels.Models, preloadTextures: true);
					}
					else
					{
						MyMeshes.Preload(myRenderMessagePreloadModels.Models);
					}
				}
				break;
			}
			case MyRenderMessageEnum.DebugDrawLine3D:
			case MyRenderMessageEnum.DebugDrawLine2D:
			case MyRenderMessageEnum.DebugDrawPoint:
			case MyRenderMessageEnum.DebugDrawSphere:
			case MyRenderMessageEnum.DebugDrawAABB:
			case MyRenderMessageEnum.DebugDrawAxis:
			case MyRenderMessageEnum.DebugDrawOBB:
			case MyRenderMessageEnum.DebugDrawFrustrum:
			case MyRenderMessageEnum.DebugDrawTriangle:
			case MyRenderMessageEnum.DebugDrawCapsule:
			case MyRenderMessageEnum.DebugDrawText2D:
			case MyRenderMessageEnum.DebugDrawText3D:
			case MyRenderMessageEnum.DebugDrawModel:
			case MyRenderMessageEnum.DebugDrawTriangles:
			case MyRenderMessageEnum.DebugDrawPlane:
			case MyRenderMessageEnum.DebugDrawCylinder:
			case MyRenderMessageEnum.DebugDrawCone:
			case MyRenderMessageEnum.DebugDrawMesh:
			case MyRenderMessageEnum.DebugDraw6FaceConvex:
			case MyRenderMessageEnum.DebugWaitForPresent:
			case MyRenderMessageEnum.DebugClearPersistentMessages:
				m_debugDrawMessages.Add(message);
				break;
			case MyRenderMessageEnum.DebugCrashRenderThread:
				throw new InvalidOperationException("Forced exception");
			case MyRenderMessageEnum.DebugPrintAllFileTexturesIntoLog:
				Log.WriteLine(MyManagers.FileTextures.GetFileTexturesDesc().ToString());
				Log.WriteLine(MyManagers.FileArrayTextures.GetFileTexturesDesc().ToString());
				Log.Flush();
				break;
			case MyRenderMessageEnum.SetGravityProvider:
			{
				MyRenderMessageSetGravityProvider myRenderMessageSetGravityProvider = (MyRenderMessageSetGravityProvider)message;
				if (myRenderMessageSetGravityProvider.CalculateGravityInPoint != null)
				{
					CalculateGravityInPoint = myRenderMessageSetGravityProvider.CalculateGravityInPoint;
					break;
				}
				CalculateGravityInPoint = (Vector3D x) => Vector3.Zero;
				break;
			}
			case MyRenderMessageEnum.DeferStateChanges:
				m_deferStateChanges = ((MyRenderMessageDeferStateChanges)message).Enabled;
				break;
			case MyRenderMessageEnum.InvalidateClipmapRange:
			case MyRenderMessageEnum.ClipmapsReady:
			case MyRenderMessageEnum.RebuildCullingStructure:
			case MyRenderMessageEnum.CreateEffect:
			case MyRenderMessageEnum.UpdateNewPipelineSettings:
			case MyRenderMessageEnum.UpdateMaterialsSettings:
			case MyRenderMessageEnum.UpdateEnvironmentMap:
			case MyRenderMessageEnum.SetDecalsBlacklist:
			case MyRenderMessageEnum.ScreenshotTaken:
			case MyRenderMessageEnum.ExportToObjComplete:
			case MyRenderMessageEnum.Error:
			case MyRenderMessageEnum.DebugDrawLine3DBatch:
			case MyRenderMessageEnum.VideoAdaptersResponse:
			case MyRenderMessageEnum.CreatedDeviceSettings:
			case MyRenderMessageEnum.MainThreadCallback:
			case MyRenderMessageEnum.TasksFinished:
			case MyRenderMessageEnum.ParticleEffectRemoved:
				break;
			}
		}

		internal static ShaderMacro[] ShaderSampleFrequencyDefine()
		{
			return m_shaderSampleFrequencyDefine;
		}

		public static void LogUpdateRenderSettings()
		{
			Log.WriteLine("MyRenderSettings1 = {");
			Log.IncreaseIndent();
			Log.WriteLine("AntialiasingMode = " + Settings.User.AntialiasingMode);
			Log.WriteLine("ShadowQuality = " + Settings.User.ShadowQuality);
			Log.WriteLine("ShadowGPUQuality = " + Settings.User.ShadowGPUQuality);
			Log.WriteLine("TextureQuality = " + Settings.User.TextureQuality);
			Log.WriteLine("VoxelTextureQuality = " + Settings.User.VoxelTextureQuality);
			Log.WriteLine("AnisotropicFiltering = " + Settings.User.AnisotropicFiltering);
			Log.WriteLine("HqDepth = " + Settings.User.HqDepth);
			Log.WriteLine("GrassDrawDistance = " + Settings.User.GrassDrawDistance);
			Log.WriteLine("GrassDensityFactor = " + Settings.User.GrassDensityFactor);
			Log.WriteLine("AmbientOcclusionEnabled = " + Settings.User.AmbientOcclusionEnabled);
			Log.WriteLine("ModelQuality = " + Settings.User.ModelQuality);
			Log.WriteLine("VoxelQuality = " + Settings.User.VoxelQuality);
			Log.WriteLine("VoxelShaderQuality = " + Settings.User.VoxelShaderQuality);
			Log.WriteLine("AlphaMaskedShaderQuality = " + Settings.User.AlphaMaskedShaderQuality);
			Log.WriteLine("AtmosphereShaderQuality = " + Settings.User.AtmosphereShaderQuality);
			Log.WriteLine("DistanceFade = " + Settings.User.DistanceFade);
			Log.WriteLine("ParticleQuality = " + Settings.User.ParticleQuality);
<<<<<<< HEAD
			Log.WriteLine("LightsQuality = " + Settings.User.LightsQuality);
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Log.DecreaseIndent();
			Log.WriteLine("}");
		}

		private static bool UpdateAntialiasingMode(MyAntialiasingMode mode, MyAntialiasingMode oldMode)
		{
			MyShaders.Recompile();
			MyMaterialShaders.Recompile();
			MyRenderableComponent.MarkAllDirty();
			return mode.SamplesCount() != oldMode.SamplesCount();
		}

		internal static void UpdateRenderSettings(MyRenderSettings settings)
		{
			Parallel.Start(LogUpdateRenderSettings);
			MyRenderSettings settings2 = Settings;
			Settings = settings;
			if (settings.User.GrassDensityFactor != settings2.User.GrassDensityFactor)
			{
				MyManagers.FoliageManager.Reset();
			}
			if (settings.User.ShadowGPUQuality != settings2.User.ShadowGPUQuality)
			{
				ResetShadows(MyImmediateRC.RC, MyShadowCascades.Settings.Data.CascadesCount, settings.User.ShadowGPUQuality.ShadowCascadeResolution());
			}
			bool flag = settings.HDREnabled != settings2.HDREnabled || settings.User.HqTarget != settings2.User.HqTarget;
			if (settings.User.AntialiasingMode != settings2.User.AntialiasingMode)
			{
				flag |= UpdateAntialiasingMode(settings.User.AntialiasingMode, settings2.User.AntialiasingMode);
			}
			if (flag)
			{
				CreateScreenResources();
			}
			if (settings.User.AnisotropicFiltering != settings2.User.AnisotropicFiltering)
			{
				MySamplerStateManager.UpdateFiltering();
			}
			if (settings.User.TextureQuality != settings2.User.TextureQuality)
			{
				MyMeshMaterials1.InvalidateMaterials();
				MyManagers.GlobalResources.OnTextureQualityChanged();
				MyManagers.Textures.OnTextureQualityChanged();
			}
			if (settings.User.VoxelTextureQuality != settings2.User.VoxelTextureQuality)
			{
				MyVoxelMaterials.InvalidateMaterials();
			}
			if (settings.User.VoxelShaderQuality != settings2.User.VoxelShaderQuality || settings.User.AlphaMaskedShaderQuality != settings2.User.AlphaMaskedShaderQuality)
			{
				MyRenderableComponent.MarkAllDirty();
			}
			if (settings.User.AtmosphereShaderQuality != settings2.User.AtmosphereShaderQuality)
			{
				MyAtmosphereRenderer.ReloadShaders();
			}
			MyGPUEmitters.UpdateParticleSettings();
			MyScreenDecals.UpdateDecalsQuality();
			if (settings.DrawCheckerTexture != settings2.DrawCheckerTexture)
			{
				MyManagers.FileTextures.SetCheckerTexture(settings.DrawCheckerTexture);
			}
<<<<<<< HEAD
			MyLightsRendering.UpdateSettings();
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static void ResizeSwapchain(int width, int height)
		{
			RC.ClearState();
			RemoveScreenResources();
			if (Backbuffer != null)
			{
				Backbuffer.Release();
				m_swapchain.ResizeBuffers(m_swapchain.Description.BufferCount, width, height, m_swapchain.Description.ModeDescription.Format, SwapChainFlags.AllowModeSwitch);
			}
			Backbuffer = new MyBackbuffer(m_swapchain.GetBackBuffer<Texture2D>(0));
			m_resolution = new Vector2I(width, height);
			CreateScreenResources();
		}

		internal static void Present()
		{
			if (m_swapchain != null)
			{
				if (!m_deferredStateChanges)
				{
					MyManagers.OnFrameEnd();
				}
<<<<<<< HEAD
				MyGpuProfiler.IC_BeginBlockAlways("Waiting for present", "Present", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-SwapChain.cs");
				m_swapchain.Present(m_settings.VSync, PresentFlags.None);
				MyGpuProfiler.IC_EndBlockAlways(0f, "Present", "E:\\Repo1\\Sources\\VRage.Render11\\Render\\MyRender-SwapChain.cs");
=======
				MyGpuProfiler.IC_BeginBlockAlways("Waiting for present", "Present", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-SwapChain.cs");
				m_swapchain.Present(m_settings.VSync, PresentFlags.None);
				MyGpuProfiler.IC_EndBlockAlways(0f, "Present", "E:\\Repo3\\Sources\\VRage.Render11\\Render\\MyRender-SwapChain.cs");
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (!m_deferredStateChanges)
				{
					MyManagers.OnUpdate();
				}
				MyGpuProfiler.EndFrame();
				MyGpuProfiler.StartFrame();
				MyVRage.Platform.Render.ApplyRenderSettings(null);
			}
		}

		internal static bool SettingsChanged(MyRenderDeviceSettings settings)
		{
			return !m_settings.Equals(ref settings);
		}

		private static void ForceWindowed()
		{
			if (m_settings.WindowMode == MyWindowModeEnum.Fullscreen && m_swapchain != null)
			{
				try
				{
					m_swapchain.SetFullscreenState(false, null);
				}
				catch (Exception)
				{
				}
			}
		}

		private static void LogSettings(ref MyRenderDeviceSettings settings)
		{
			MyAdapterInfo[] adaptersList = GetAdaptersList();
			Log.WriteLine("MyRenderDeviceSettings = {");
			Log.IncreaseIndent();
			Log.WriteLine("Adapter id = " + settings.AdapterOrdinal);
			if (settings.AdapterOrdinal >= 0 && settings.AdapterOrdinal < adaptersList.Length)
			{
				Log.WriteLine("DXGIAdapter id = " + adaptersList[settings.AdapterOrdinal].AdapterDeviceId);
				Log.WriteLine("DXGIOutput id = " + adaptersList[settings.AdapterOrdinal].OutputId);
			}
			else
			{
				Log.WriteLine("DXGIAdapter id = <autodetect>");
				Log.WriteLine("DXGIOutput id = <autodetect>");
			}
			Log.WriteLine($"Resolution = {settings.BackBufferWidth} x {settings.BackBufferHeight}");
			Log.WriteLine("Window mode = " + settings.WindowMode);
			Log.DecreaseIndent();
			Log.WriteLine("}");
		}

		internal static void ApplySettings(MyRenderDeviceSettings settings)
		{
			Log.WriteLine("ApplySettings");
			Log.IncreaseIndent();
			LogSettings(ref settings);
			if (Backbuffer == null || settings.BackBufferWidth != Backbuffer.Size.X || settings.BackBufferHeight != Backbuffer.Size.Y)
			{
				ResizeSwapchain(settings.BackBufferWidth, settings.BackBufferHeight);
			}
			if (m_settings.UseStereoRendering)
			{
				settings.UseStereoRendering = true;
			}
			MyVRage.Platform.Render.ApplyRenderSettings(settings);
			m_settings = settings;
			Log.DecreaseIndent();
		}

		public static ShaderMacro GetQualityMacro(MyRenderQualityEnum quality)
		{
<<<<<<< HEAD
			switch (quality)
			{
			case MyRenderQualityEnum.LOW:
				return new ShaderMacro("LQ", null);
			case MyRenderQualityEnum.NORMAL:
				return new ShaderMacro("MQ", null);
			default:
				return new ShaderMacro("HQ", null);
			}
=======
			return quality switch
			{
				MyRenderQualityEnum.LOW => new ShaderMacro("LQ", null), 
				MyRenderQualityEnum.NORMAL => new ShaderMacro("MQ", null), 
				_ => new ShaderMacro("HQ", null), 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		static MyRender11()
		{
<<<<<<< HEAD
=======
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_fontsById = new Dictionary<int, MyRenderFont>(32);
			m_persistentDebugMessages = new List<MyRenderMessageBase>();
			LockImmediateRC = false;
			m_settings = new MyRenderDeviceSettings
			{
				AdapterOrdinal = -1
			};
			m_initialized = false;
			m_initializedOnce = false;
			m_debugDrawMessages = new List<MyRenderMessageBase>();
			m_texturesToRender = new List<renderColoredTextureProperties>();
			m_exceptionBuilder = new StringBuilder();
			m_deferredUpdate = new ConcurrentQueue<Action>();
			m_mainViewportScaleFactor = 1f;
			m_debugOverrides = new MyRenderDebugOverrides();
			Postprocess = MyPostprocessSettings.Default;
			m_resetEyeAdaptation = false;
			m_stretchPs = MyPixelShaders.Id.NULL;
			m_processStopwatch = new Stopwatch();
			m_shaderSampleFrequencyDefine = new ShaderMacro[1]
			{
				new ShaderMacro("SAMPLE_FREQ_PASS", null)
			};
			m_swapchain = null;
			IdGenerator = new ObjectIDGenerator();
			UseComplementaryDepthBuffer = true;
			SharedData = new MySharedData();
			Log = new MyLog(alwaysFlush: true);
			RootDirectory = MyFileSystem.ContentPath;
			RootDirectoryEffects = MyFileSystem.ContentPath;
			RootDirectoryDebug = MyFileSystem.ContentPath;
			GlobalMessageCounter = 1u;
			Settings = MyRenderSettings.Default;
			Environment = new MyEnvironment();
			m_renderProfiler = new MyRenderProfilerDX11();
<<<<<<< HEAD
			m_whiteList = new HashSet<string> { "GPUFrame", "Waiting for present" };
=======
			HashSet<string> obj = new HashSet<string>();
			obj.Add("GPUFrame");
			obj.Add("Waiting for present");
			m_whiteList = obj;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_lastTimings = new Dictionary<string, MyTimeSpan>();
			CalculateGravityInPoint = (Vector3D x) => Vector3.Zero;
			MyGpuProfiler.OnBlockStart += delegate
			{
			};
			MyGpuProfiler.OnBlockEnd += delegate
			{
			};
			MyGpuProfiler.OnBlockStart += delegate(string x, string member, string file)
			{
				MySimpleProfiler.BeginGPUBlock(x);
			};
			MyGpuProfiler.OnBlockEnd += delegate(MyTimeSpan x, float y, string member, string file)
			{
				MySimpleProfiler.EndGPUBlock(x);
			};
			MyGpuProfiler.OnBlockStart += delegate(string x, string member, string file)
			{
				StatsStart(x);
			};
			MyGpuProfiler.OnBlockEnd += delegate(MyTimeSpan x, float y, string member, string file)
			{
				StatsEnd(x);
			};
			MyGpuProfiler.OnCommit += StatsDone;
			MyGpuProfiler.IsPaused += () => MyRenderProfiler.Paused;
			Log.InitWithDate("VRageRender-DirectX11", new StringBuilder("Version unknown"), 3);
			Log.WriteLine("VRage renderer started");
		}

		private static void StatsStart(string blockName)
		{
			if (m_whiteList.Contains(blockName))
			{
				m_blockName = blockName;
			}
			else if (m_blockName != null)
			{
				m_blockInnerCounter++;
			}
		}

		private static void StatsEnd(MyTimeSpan time)
		{
			if (m_blockName == null)
			{
				return;
			}
			if (m_blockInnerCounter == 0)
			{
				if (m_lastTimings.ContainsKey(m_blockName))
				{
					m_lastTimings[m_blockName] = time;
				}
				else
				{
					m_lastTimings.Add(m_blockName, time);
				}
				m_blockName = null;
			}
			else
			{
				m_blockInnerCounter--;
			}
		}

		private static void StatsDone()
		{
			double seconds = m_lastTimings["GPUFrame"].Seconds;
			double seconds2 = m_lastTimings["Waiting for present"].Seconds;
			MyRenderProxy.GPULoadSmooth = MathHelper.Smooth(MyRenderProxy.GPULoad = (float)(seconds / (seconds + seconds2) * 100.0), MyRenderProxy.GPULoadSmooth);
			MyRenderProxy.GPUTimeSmooth = MathHelper.Smooth((float)seconds * 1000f, MyRenderProxy.GPUTimeSmooth);
		}

		internal static void EnqueueMessage(MyRenderMessageBase message)
		{
<<<<<<< HEAD
			if (Thread.CurrentThread == MyRenderProxy.RenderSystemThread)
=======
			if (Thread.get_CurrentThread() == MyRenderProxy.RenderSystemThread)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				SharedData.MessagesForNextFrame.Enqueue(message);
			}
			else
			{
				SharedData.CurrentUpdateFrame.Enqueue(message);
			}
		}

		internal static void EnqueueOutputMessage(MyRenderMessageBase message)
		{
			SharedData.RenderOutputMessageQueue.Enqueue(message);
		}

		internal static MyRenderProfiler GetRenderProfiler()
		{
			return m_renderProfiler;
		}

		public static MyAdapterInfo[] GetAdaptersList()
		{
			return MyVRage.Platform.Render.GetRenderAdapterList();
		}

		private static Ray ComputeIntersectionLine(ref Plane p1, ref Plane p2)
		{
			Ray result = default(Ray);
			result.Direction = Vector3.Cross(p1.Normal, p2.Normal);
			float num = result.Direction.LengthSquared();
			result.Position = Vector3.Cross((0f - p1.D) * p2.Normal + p2.D * p1.Normal, result.Direction) / num;
			return result;
		}

		private static void TransformRay(ref Ray ray, ref Matrix matrix)
		{
			ray.Direction = Vector3.Transform(ray.Position + ray.Direction, ref matrix);
			ray.Position = Vector3.Transform(ray.Position, ref matrix);
			ray.Direction -= ray.Position;
		}

		internal static void DrawSceneDebug()
		{
		}
	}
}
