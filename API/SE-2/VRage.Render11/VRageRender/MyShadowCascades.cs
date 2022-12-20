using System;
using System.Collections.Generic;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Mathematics.Interop;
using VRage;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;

namespace VRageRender
{
	internal class MyShadowCascades
	{
		private sealed class MyCascadeState
		{
			public int Index;

			public MyProjectionInfo Info;

			public Vector4 Scale;

			public Vector3D UpdatePosition;

			public Vector3D LightDirection;

			public long LastUpdateFrame;

			public int FramesSinceLightUpdate;

<<<<<<< HEAD
=======
			public MyTuple<int, int> UpdateInterval;

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public long FirstSkipFrame;

			public void ResetSkipFrame()
			{
				FirstSkipFrame = MyCommon.FrameCounter + 20;
			}
		}

		internal static readonly MyShadowsSettings Settings = new MyShadowsSettings();

		private static readonly MyTuple<int, int>[] m_updateIntervals = new MyTuple<int, int>[8]
		{
			MyTuple.Create(1, 0),
			MyTuple.Create(1, 0),
			MyTuple.Create(1, 0),
			MyTuple.Create(1, 0),
			MyTuple.Create(4, 3),
			MyTuple.Create(8, 5),
			MyTuple.Create(8, 6),
			MyTuple.Create(8, 7)
		};

		private readonly Vector3D[] m_cornersCS = new Vector3D[8]
		{
			new Vector3D(-1.0, -1.0, 0.0),
			new Vector3D(-1.0, 1.0, 0.0),
			new Vector3D(1.0, 1.0, 0.0),
			new Vector3D(1.0, -1.0, 0.0),
			new Vector3D(-1.0, -1.0, 1.0),
			new Vector3D(-1.0, 1.0, 1.0),
			new Vector3D(1.0, 1.0, 1.0),
			new Vector3D(1.0, -1.0, 1.0)
		};

		private readonly Vector3D[] m_blockOS = new Vector3D[8]
		{
			new Vector3D(-1.0, -1.0, 0.0),
			new Vector3D(-1.0, 1.0, 0.0),
			new Vector3D(1.0, 1.0, 0.0),
			new Vector3D(1.0, -1.0, 0.0),
			new Vector3D(-1.0, -1.0, 1.0),
			new Vector3D(-1.0, 1.0, 1.0),
			new Vector3D(1.0, 1.0, 1.0),
			new Vector3D(1.0, -1.0, 1.0)
		};

		private int m_initializedCascadesCount;

		private float[] m_shadowCascadeSplitDepths;

		private readonly MyCascadeState[] m_cascadeStates = new MyCascadeState[8];

		private MyShadowCascadesPostProcess m_cascadePostProcessor;

		private IConstantBuffer m_csmConstants;

		private IDepthArrayTexture m_cascadeShadowmapArray;

		private IConstantBuffer m_csmConstants2;

		private IDepthArrayTexture m_cascadeShadowmapArray2;

		private readonly MyShadowCascadesStats m_cascadeStats = new MyShadowCascadesStats();

		private static int m_texturesReferenceCount;

		private bool m_matricesInitialized;

		private MatrixD m_matricesInvViewProjection;

		private Matrix m_matricesProjection;

		private Vector3D m_matricesCameraPosition;

		private MatrixD m_matricesInvViewAt0;

		private uint m_validCascades;

		private readonly Vector3D[] m_frustumVerticesWS = new Vector3D[8];

		private readonly Vector3D[] m_tmpDebugFrozen8D = new Vector3D[8];

		private readonly Vector3[] m_tmpDebugFrozen8 = new Vector3[8];

		private readonly Color[] m_debugCascadeColor = new Color[8]
		{
			new Color(255, 0, 0),
			new Color(0, 255, 0),
			new Color(0, 0, 255),
			new Color(255, 255, 0),
			new Color(0, 255, 255),
			new Color(255, 0, 255),
			new Color(255, 0, 128),
			new Color(128, 255, 0)
		};

		/// <summary>
		/// Creates shadowmap queries and appends them to the provided list
		/// </summary>
		private readonly Vector3D[] m_tmpUntransformedVertices = new Vector3D[4];

		private readonly MatrixD[] m_tmpCascadesMatrices = new MatrixD[8];

		private readonly MatrixD[] m_tmpCascadesMatricesExtruded = new MatrixD[8];

		private const long MIN_SKIP_FRAMES = 20L;

		internal uint[] PixelCounts => m_cascadeStats.PixelCounts;

		internal IDepthArrayTexture CascadeShadowmapArrayOld => m_cascadeShadowmapArray2;

		internal IConstantBuffer CascadeConstantBufferOld => m_csmConstants2;

		internal IDepthArrayTexture CascadeShadowmapArray => m_cascadeShadowmapArray;

		internal IConstantBuffer CascadeConstantBuffer => m_csmConstants;

		internal bool Enabled
		{
			get
			{
				if (MyRender11.Settings.EnableShadows && MyRender11.DebugOverrides.Shadows)
				{
					return MyRender11.Settings.User.ShadowQuality != MyShadowsQuality.DISABLED;
				}
				return false;
			}
		}

		public int CascadeCount => m_initializedCascadesCount;

		public int CascadeResolution => m_cascadeShadowmapArray.Size.X;

		internal MyShadowCascades(MyRenderContext rc, int numberOfCascades, int cascadeResolution)
		{
			for (int i = 0; i < m_cascadeStates.Length; i++)
			{
				m_cascadeStates[i] = new MyCascadeState
				{
					Index = i,
					Info = new MyProjectionInfo()
				};
			}
			Init(rc, numberOfCascades, cascadeResolution);
		}

		private void Init(MyRenderContext rc, int numberOfCascades, int cascadeResolution)
		{
			SetNumberOfCascades(numberOfCascades);
			m_initializedCascadesCount = numberOfCascades;
			InitResources(rc, cascadeResolution);
			if (m_cascadePostProcessor == null)
			{
				m_cascadePostProcessor = new MyShadowCascadesPostProcess(numberOfCascades);
			}
			else
			{
				m_cascadePostProcessor.Reset(numberOfCascades);
			}
			for (int i = 0; i < m_cascadeStates.Length; i++)
			{
				m_cascadeStates[i].ResetSkipFrame();
			}
		}

		internal void Reset(MyRenderContext rc, int numberOfCascades, int cascadeResolution)
		{
			UnloadResources();
			Init(rc, numberOfCascades, cascadeResolution);
		}

		private void InitResources(MyRenderContext rc, int cascadeResolution)
		{
			InitConstantBuffer();
			InitCascadeTextures(rc, cascadeResolution);
		}

		internal void UnloadResources()
		{
			m_cascadePostProcessor.UnloadResources();
			DestroyConstantBuffer();
			DestroyCascadeTextures();
		}

		private unsafe void InitConstantBuffer()
		{
			DestroyConstantBuffer();
			int byteSize = (sizeof(Matrix) + sizeof(Matrix) + 2 * sizeof(Vector4) + sizeof(Vector4)) * 8 + sizeof(Vector4);
			m_csmConstants = MyManagers.Buffers.CreateConstantBuffer("MyShadowCascades", byteSize, null, ResourceUsage.Dynamic, isGlobal: true);
			m_csmConstants2 = MyManagers.Buffers.CreateConstantBuffer("MyShadowCascades2", byteSize, null, ResourceUsage.Dynamic, isGlobal: true);
		}

		private void DestroyConstantBuffer()
		{
			MyManagers.Buffers.Dispose(m_csmConstants);
			MyManagers.Buffers.Dispose(m_csmConstants2);
			m_csmConstants = null;
			m_csmConstants2 = null;
		}

		private void InitCascadeTextures(MyRenderContext rc, int cascadeResolution)
		{
			DestroyCascadeTextures();
			MyArrayTextureManager arrayTextures = MyManagers.ArrayTextures;
			MyRender11.Log.WriteLine("InitCascadeTextures: " + m_texturesReferenceCount);
			m_cascadeShadowmapArray = arrayTextures.CreateDepthArray("MyShadowCascades.CascadeShadowmapArray", cascadeResolution, cascadeResolution, m_initializedCascadesCount, Format.R32_Typeless, Format.R32_Float, Format.D32_Float);
			m_cascadeShadowmapArray2 = arrayTextures.CreateDepthArray("MyShadowCascades.CascadeShadowmapArray2", cascadeResolution, cascadeResolution, m_initializedCascadesCount, Format.R32_Typeless, Format.R32_Float, Format.D32_Float);
			for (int i = 0; i < m_initializedCascadesCount; i++)
			{
				rc.ClearDsv(m_cascadeShadowmapArray.SubresourceDsv(i), DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1f, 0);
				rc.ClearDsv(m_cascadeShadowmapArray2.SubresourceDsv(i), DepthStencilClearFlags.Depth | DepthStencilClearFlags.Stencil, 1f, 0);
			}
			m_cascadeStats.Init();
			m_texturesReferenceCount++;
		}

		private void DestroyCascadeTextures()
		{
			MyRender11.Log.WriteLine("DestroyCascadeTextures");
			MyRender11.Log.IncreaseIndent();
			m_cascadeStats.Done();
			MyArrayTextureManager arrayTextures = MyManagers.ArrayTextures;
			arrayTextures.DisposeTex(ref m_cascadeShadowmapArray);
			arrayTextures.DisposeTex(ref m_cascadeShadowmapArray2);
			m_texturesReferenceCount = Math.Max(m_texturesReferenceCount - 1, 0);
			MyRender11.Log.DecreaseIndent();
		}

		private void SetNumberOfCascades(int newCount)
		{
			Array.Resize(ref m_shadowCascadeSplitDepths, newCount + 1);
		}

		private unsafe MatrixD CreateGlobalMatrix(ref MatrixD invViewProjection)
		{
			Vector3D* ptr = stackalloc Vector3D[m_cornersCS.Length];
			Vector3D.Transform(m_cornersCS, ref invViewProjection, ptr);
			Vector3D zero = Vector3D.Zero;
			for (int i = 0; i < m_cornersCS.Length; i++)
			{
				zero += ptr[i];
			}
			zero /= 8.0;
			MatrixD matrixD = MatrixD.CreateLookAt(zero, zero - MyRender11.Environment.Data.EnvironmentLight.SunLightDirection, Vector3D.UnitY);
			MatrixD matrixD2 = MatrixD.CreateOrthographic(1.0, 1.0, 0.0, 1.0);
			return matrixD * matrixD2 * MyMatrixHelpers.ClipspaceToTexture;
		}

		internal void PrepareQueries(MyRenderContext rc, List<MyShadowmapQuery> appendShadowmapQueries)
		{
			if (!Enabled)
			{
				return;
			}
			m_cascadeStats.Update();
			if (!MyRender11.Settings.ShadowCameraFrozen || !m_matricesInitialized)
			{
				m_matricesInitialized = true;
				m_matricesInvViewProjection = MyRender11.Environment.Matrices.InvViewProjectionD;
				m_matricesProjection = MyRender11.Environment.Matrices.Projection;
				m_matricesCameraPosition = MyRender11.Environment.Matrices.CameraPosition;
				m_matricesInvViewAt0 = MyRender11.Environment.Matrices.InvViewAt0;
			}
			FillConstantBuffer(rc, m_csmConstants2);
			IDepthArrayTexture cascadeShadowmapArray = m_cascadeShadowmapArray2;
			m_cascadeShadowmapArray2 = m_cascadeShadowmapArray;
			m_cascadeShadowmapArray = cascadeShadowmapArray;
			bool flag = true;
			for (int i = 0; i < m_initializedCascadesCount; i++)
			{
				MyCascadeState myCascadeState = m_cascadeStates[i];
				myCascadeState.FramesSinceLightUpdate++;
				if ((float)myCascadeState.FramesSinceLightUpdate > (float)i * Settings.Data.LightDirectionChangeDelayMultiplier || MyRender11.Environment.Data.EnvironmentLight.SunLightDirection.Dot(myCascadeState.LightDirection) < 1f - Settings.Data.LightDirectionDifferenceThreshold)
				{
					myCascadeState.LightDirection = MyRender11.Environment.Data.EnvironmentLight.SunLightDirection;
					myCascadeState.FramesSinceLightUpdate = 0;
				}
			}
			MatrixD matrix = CreateGlobalMatrix(ref m_matricesInvViewProjection);
			float num = 1f;
			float num2 = MyRender11.Settings.User.ShadowQuality.BackOffset();
			float num3 = MyRender11.Settings.User.ShadowGPUQuality.ShadowCascadeResolution();
			for (int j = 0; j < m_shadowCascadeSplitDepths.Length; j++)
			{
				m_shadowCascadeSplitDepths[j] = MyRender11.Settings.User.ShadowQuality.ShadowCascadeSplit(j);
			}
			double num4 = 1.0 / (double)m_matricesProjection.M11;
			double num5 = 1.0 / (double)m_matricesProjection.M22;
			m_tmpUntransformedVertices[0] = new Vector3D(0.0 - num4, 0.0 - num5, -1.0);
			m_tmpUntransformedVertices[1] = new Vector3D(0.0 - num4, num5, -1.0);
			m_tmpUntransformedVertices[2] = new Vector3D(num4, num5, -1.0);
			m_tmpUntransformedVertices[3] = new Vector3D(num4, 0.0 - num5, -1.0);
			uint num6 = 0u;
			int num7 = (MyRender11.Settings.ShadowCascadeUsageBasedSkip ? 10 : 0);
			for (int k = 0; k < m_initializedCascadesCount; k++)
			{
				MyCascadeState myCascadeState2 = m_cascadeStates[k];
				if (!MyManagers.Ansel.Is360Capturing)
				{
					bool flag2 = MyCommon.FrameCounter % m_updateIntervals[k].Item1 != m_updateIntervals[k].Item2;
					if (((!(m_shadowCascadeSplitDepths[k] > 1000f) || !(Vector3D.DistanceSquared(myCascadeState2.UpdatePosition, m_matricesCameraPosition) > Math.Pow(1000.0, 2.0))) && flag2 && !Settings.Data.UpdateCascadesEveryFrame) || Settings.ShadowCascadeFrozen[k])
					{
						UpdateCascadeDoubleBuffering(rc, myCascadeState2);
						continue;
					}
				}
				myCascadeState2.UpdatePosition = m_matricesCameraPosition;
				for (int l = 0; l < 4; l++)
				{
					m_frustumVerticesWS[l] = m_tmpUntransformedVertices[l] * m_shadowCascadeSplitDepths[k];
					m_frustumVerticesWS[l + 4] = m_tmpUntransformedVertices[l] * m_shadowCascadeSplitDepths[k + 1];
				}
				Vector3D.Transform(m_frustumVerticesWS, ref m_matricesInvViewAt0, m_frustumVerticesWS);
				BoundingSphereD boundingSphereD = BoundingSphereD.CreateFromPoints(m_frustumVerticesWS);
				if (flag)
				{
					Vector3D.Fract(ref m_matricesCameraPosition, out var r);
					boundingSphereD.Center -= r;
					boundingSphereD.Radius = Math.Ceiling(boundingSphereD.Radius);
				}
				Vector3D vector3D = boundingSphereD.Center + myCascadeState2.LightDirection * (boundingSphereD.Radius + (double)num);
				MatrixD matrixD = MatrixD.CreateLookAt(vector3D, vector3D - myCascadeState2.LightDirection, (Math.Abs(Vector3.UnitY.Dot(myCascadeState2.LightDirection)) < 0.99f) ? Vector3.UnitY : Vector3.UnitX);
				double num8 = boundingSphereD.Radius + (double)num + (double)num2;
				Vector3D vector3D2 = new Vector3D(0.0 - boundingSphereD.Radius, 0.0 - boundingSphereD.Radius, num);
				Vector3D vector3D3 = new Vector3D(boundingSphereD.Radius, boundingSphereD.Radius, num8 + boundingSphereD.Radius);
				Vector3D viewOrigin = vector3D - myCascadeState2.LightDirection * vector3D3.Z;
				MatrixD m = MatrixD.CreateOrthographicOffCenter(vector3D2.X, vector3D3.X, vector3D2.Y, vector3D3.Y, vector3D3.Z, vector3D2.Z);
				m_tmpCascadesMatrices[k] = matrixD * m;
				MatrixD matrixD2 = MatrixD.CreateOrthographicOffCenter(vector3D2.X, vector3D3.X, vector3D2.Y, vector3D3.Y, vector3D3.Z, vector3D2.Z * -10000.0);
				m_tmpCascadesMatricesExtruded[k] = matrixD * matrixD2;
				if (flag)
				{
					Vector3D vector3D4 = Vector3D.Transform(Vector3D.Zero, MatrixD.CreateTranslation(-m_matricesCameraPosition) * m_tmpCascadesMatrices[k]) * num3 / 2.0;
					Vector3D vector3D5 = (vector3D4.Round() - vector3D4) * 2.0 / num3;
					m.M41 += vector3D5.X;
					m.M42 += vector3D5.Y;
					m_tmpCascadesMatrices[k] = matrixD * m;
				}
				MatrixD matrix2 = MatrixD.Invert(m_tmpCascadesMatrices[k]);
				Vector3D vector3D6 = Vector3D.Transform(Vector3D.Transform(new Vector3D(-1.0, -1.0, 0.0), matrix2), matrix);
				Vector3D vector3D7 = Vector3D.Transform(Vector3D.Transform(new Vector3D(1.0, 1.0, 1.0), matrix2), matrix) - vector3D6;
				Vector3D value = 1.0 / vector3D7;
				myCascadeState2.Scale = new Vector4D(value, 0.0);
				myCascadeState2.Info.WorldCameraOffsetPosition = m_matricesCameraPosition;
				myCascadeState2.Info.WorldToProjection = MatrixD.CreateTranslation(-m_matricesCameraPosition) * m_tmpCascadesMatrices[k];
				myCascadeState2.Info.LocalToProjection = m_tmpCascadesMatrices[k];
				myCascadeState2.Info.LocalToProjectionExtruded = m_tmpCascadesMatricesExtruded[k];
				myCascadeState2.Info.Projection = m;
				myCascadeState2.Info.ViewOrigin = viewOrigin;
				MyShadowmapQuery myShadowmapQuery = default(MyShadowmapQuery);
				myShadowmapQuery.DepthBuffer = m_cascadeShadowmapArray.SubresourceDsv(k);
				myShadowmapQuery.DepthBufferRo = m_cascadeShadowmapArray.SubresourceDsvRo(k);
				myShadowmapQuery.Viewport = new MyViewport(num3, num3);
				myShadowmapQuery.ProjectionInfo = myCascadeState2.Info;
				myShadowmapQuery.ProjectionDir = myCascadeState2.LightDirection;
				myShadowmapQuery.ProjectionFactor = (float)((double)(num3 * num3) / (boundingSphereD.Radius * boundingSphereD.Radius * 4.0));
				myShadowmapQuery.ViewType = MyViewType.ShadowCascade;
				myShadowmapQuery.ViewIndex = k;
				MyShadowmapQuery item = myShadowmapQuery;
				bool flag3 = m_cascadeStats.PixelCounts[k] < num7;
				if (flag3)
				{
					if (myCascadeState2.FirstSkipFrame < MyCommon.FrameCounter)
					{
						num6 |= (uint)(1 << k);
						UpdateCascadeDoubleBuffering(rc, myCascadeState2);
					}
					else
					{
						flag3 = false;
					}
				}
				else
				{
					myCascadeState2.ResetSkipFrame();
				}
				if (!flag3)
				{
					myCascadeState2.LastUpdateFrame = MyCommon.FrameCounter;
					appendShadowmapQueries.Add(item);
				}
			}
			m_validCascades = ~num6;
			DebugProcessFrustrums();
			FillConstantBuffer(rc, m_csmConstants);
		}

		private void UpdateCascadeDoubleBuffering(MyRenderContext rc, MyCascadeState cascadeState)
		{
			if (cascadeState.LastUpdateFrame == MyCommon.FrameCounter - 1)
			{
				rc.CopySubresourceRegion(m_cascadeShadowmapArray2, SharpDX.Direct3D11.Resource.CalculateSubResourceIndex(0, cascadeState.Index, m_cascadeShadowmapArray2.MipLevels), null, m_cascadeShadowmapArray, SharpDX.Direct3D11.Resource.CalculateSubResourceIndex(0, cascadeState.Index, m_cascadeShadowmapArray.MipLevels));
			}
		}

		private void FillConstantBuffer(MyRenderContext rc, IConstantBuffer constantBuffer)
		{
			MyMapping myMapping = MyMapping.MapDiscard(rc, constantBuffer);
			for (int i = 0; i < m_initializedCascadesCount; i++)
			{
				MatrixD m = m_cascadeStates[i].Info.CurrentLocalToProjection * MyMatrixHelpers.ClipspaceToTexture;
				Matrix data = Matrix.Transpose(m);
				myMapping.WriteAndPosition(ref data);
			}
			for (int j = m_initializedCascadesCount; j < 8; j++)
			{
				myMapping.WriteAndPosition(ref Matrix.Zero);
			}
			for (int k = 0; k < m_initializedCascadesCount; k++)
			{
				MatrixD m = m_cascadeStates[k].Info.CurrentLocalToProjectionExtruded * MyMatrixHelpers.ClipspaceToTexture;
				Matrix data2 = Matrix.Transpose(m);
				myMapping.WriteAndPosition(ref data2);
			}
			for (int l = m_initializedCascadesCount; l < 8; l++)
			{
				myMapping.WriteAndPosition(ref Matrix.Zero);
			}
			myMapping.WriteAndPosition(m_shadowCascadeSplitDepths, m_shadowCascadeSplitDepths.Length);
			float data3 = 1E+20f;
			for (int n = m_shadowCascadeSplitDepths.Length; n < 8; n++)
			{
				myMapping.WriteAndPosition(ref data3);
			}
			for (int num = 0; num < m_initializedCascadesCount; num++)
			{
				Vector4 data4 = m_cascadeStates[num].Scale / m_cascadeStates[0].Scale;
				data4 += Vector4.One * num * 0.3f;
				data4 *= ((num < 4) ? 3f : ((num < 5) ? 2f : 1f));
				myMapping.WriteAndPosition(ref data4);
			}
			for (int num2 = m_initializedCascadesCount; num2 < 8; num2++)
			{
				myMapping.WriteAndPosition(ref Vector4.Zero);
			}
			float data5 = 1f / (float)MyRender11.Settings.User.ShadowGPUQuality.ShadowCascadeResolution();
			myMapping.WriteAndPosition(ref data5);
			float data6 = Settings.Data.ZBias;
			myMapping.WriteAndPosition(ref data6);
			myMapping.WriteAndPosition(ref m_initializedCascadesCount);
			myMapping.WriteAndPosition(ref m_validCascades);
			myMapping.Unmap();
		}

		internal IBorrowedUavTexture PostProcess(MyRenderContext rc)
		{
			IBorrowedUavTexture borrowedUavTexture = MyManagers.RwTexturesPool.BorrowUav("MyShadowCascades.PostProcess", Format.R8_UNorm);
			if (!Enabled)
			{
				rc.ClearUav(borrowedUavTexture, default(RawInt4));
				return borrowedUavTexture;
			}
			m_cascadePostProcessor.GatherArray(rc, borrowedUavTexture, CascadeShadowmapArray, m_csmConstants);
			Gather(rc, ref MyCommon.FrameConstantsData.Screen, MyGBuffer.Main.ResolvedDepthStencil.SrvDepth, 0);
			return borrowedUavTexture;
		}

		public void Gather(MyRenderContext rc, ref MyCommon.MyScreenLayout layout, ISrvBindable srvDepth, int viewId)
		{
			if (Enabled)
			{
				m_cascadeStats.Gather(rc, m_csmConstants, ref layout, srvDepth, viewId);
			}
		}

		private void DebugProcessFrustrums()
		{
			if (MyRender11.Settings.DisplayShadowVolumes)
			{
				for (int i = 0; i < m_initializedCascadesCount; i++)
				{
					DebugDrawFrozenCascadeFrustrum(i);
				}
			}
		}

		private void DebugDrawFrozenViewFrustrum(int nCascade)
		{
			MatrixD matrix = MatrixD.Invert(Matrix.CreateTranslation(MyRender11.Environment.Matrices.CameraPosition) * MatrixD.Invert(m_matricesInvViewProjection));
			Vector3D.Transform(m_blockOS, ref matrix, m_tmpDebugFrozen8D);
			for (int i = 0; i < m_tmpDebugFrozen8.Length; i++)
			{
				m_tmpDebugFrozen8[i] = m_tmpDebugFrozen8D[i];
			}
			MyLinesBatch myLinesBatch = MyLinesRenderer.CreateBatch();
			myLinesBatch.Add6FacedConvex(m_tmpDebugFrozen8, Color.Blue);
			myLinesBatch.Commit();
		}

		private void DebugDrawFrozenCascadeFrustrum(int nCascade)
		{
			MyLinesBatch myLinesBatch = MyLinesRenderer.CreateBatch();
			MatrixD matrix = MatrixD.Invert(Matrix.CreateTranslation(MyRender11.Environment.Matrices.CameraPosition) * m_cascadeStates[nCascade].Info.WorldToProjection);
			Vector3D.Transform(m_blockOS, ref matrix, m_tmpDebugFrozen8D);
			for (int i = 0; i < m_tmpDebugFrozen8.Length; i++)
			{
				m_tmpDebugFrozen8[i] = m_tmpDebugFrozen8D[i];
			}
			MyPrimitivesRenderer.Draw6FacedConvexZ(m_tmpDebugFrozen8, m_debugCascadeColor[nCascade], 0.2f);
			myLinesBatch.Add6FacedConvex(m_tmpDebugFrozen8, Color.Pink);
			myLinesBatch.Commit();
		}
	}
}
