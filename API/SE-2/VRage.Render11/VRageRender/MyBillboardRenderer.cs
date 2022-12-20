using System;
using System.Collections.Generic;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Generics;
using VRage.Render.Scene;
using VRage.Render11.Common;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRage.Render11.Resources.Textures;
using VRage.Render11.Sprites;
using VRage.Render11.Tools;
using VRage.Utils;
using VRageMath;
using VRageMath.PackedVector;
using VRageRender.Vertex;

namespace VRageRender
{
	internal class MyBillboardRenderer
	{
		private struct MyBucketBatches
		{
			public int StartIndex;

			public int Count;
		}

		[Flags]
		private enum PixelShaderFlags
		{
			OIT = 0x1,
			LIT_PARTICLE = 0x2,
			ALPHA_CUTOUT = 0x4,
			SOFT_PARTICLE = 0x8,
			DEBUG_UNIFORM_ACCUM = 0x10,
			SINGLE_CHANNEL = 0x20,
			Max = 0x3F
		}

		private const int BILLBOARDS_INIT_SIZE = 8192;

		private const int MAX_BILLBOARDS_SIZE = 32768;

		private const int MAX_CUSTOM_PROJECTIONS_SIZE = 32;

		private static IConstantBuffer m_cbCustomProjections;

		private static MyVertexShaders.Id m_vs;

		private static MyVertexShaders.Id m_vsLit;

		private static MyVertexShaders.Id m_vsLDR;

		private static MyInputLayouts.Id m_inputLayout;

		private static IIndexBuffer m_IB;

		private static IVertexBuffer m_VB;

		private static ISrvBuffer m_SB;

		private const int BUCKETS_COUNT = 6;

		private static readonly int[] m_bucketCounts = new int[6];

		private static readonly int[] m_bucketIndices = new int[6];

		private static int m_billboardCountSafe;

		private static readonly MyBucketBatches[] m_bucketBatches = new MyBucketBatches[6];

		private static readonly Dictionary<string, IMyStreamedTexture> m_fileTextures = new Dictionary<string, IMyStreamedTexture>();

		private static readonly List<MyBillboardRendererBatch> m_batches = new List<MyBillboardRendererBatch>();

		private static MyBillboard[] m_tempBuffer = new MyBillboard[8192];

		private static MyBillboardDataArray m_arrayDataBillboards = new MyBillboardDataArray(8192);

		private static MyRendererStats.MyRenderStats m_stats = default(MyRendererStats.MyRenderStats);

		private static readonly MyObjectsPoolSimple<MyBillboard> m_billboardsOncePool = new MyObjectsPoolSimple<MyBillboard>(2048);

		private static MyTextureAtlas m_atlas;

		private static int m_lastBatchOffset;

		private static readonly MyPixelShaders.Id[] m_psBundle = new MyPixelShaders.Id[63];

		public static int Count => MyRenderProxy.BillboardsRead.Count + m_billboardsOncePool.GetAllocatedCount() + MyRenderProxy.PersistentBillboardsCount;

		private static void GeneratePS()
		{
			List<ShaderMacro> list = new List<ShaderMacro>();
			for (int i = 0; i < 63; i++)
			{
				list.Clear();
				for (int num = 1; num < 63; num <<= 1)
				{
					if ((i & num) > 0)
					{
						PixelShaderFlags pixelShaderFlags = (PixelShaderFlags)num;
						string name = pixelShaderFlags.ToString();
						list.Add(new ShaderMacro(name, null));
					}
				}
				m_psBundle[i] = MyPixelShaders.Create("Transparent/Billboards.hlsl", list.ToArray());
			}
		}

		internal unsafe static void Init()
		{
			m_cbCustomProjections = MyManagers.Buffers.CreateConstantBuffer("BilloardCustomProjections", sizeof(Matrix) * 32, null, ResourceUsage.Dynamic, isGlobal: true);
			GeneratePS();
			m_vs = MyVertexShaders.Create("Transparent/Billboards.hlsl");
			m_vsLit = MyVertexShaders.Create("Transparent/Billboards.hlsl", new ShaderMacro[1]
			{
				new ShaderMacro("LIT_PARTICLE", null)
			});
			m_vsLDR = MyVertexShaders.Create("Transparent/Billboards.hlsl", new ShaderMacro[1]
			{
				new ShaderMacro("LDR", null)
			});
			m_inputLayout = MyInputLayouts.Create(m_vs.InfoId, MyVertexLayouts.GetLayout(MyVertexInputComponentType.POSITION3, MyVertexInputComponentType.TEXCOORD0_H));
			InitBillboardsIndexBuffer();
			m_VB = MyManagers.Buffers.CreateVertexBuffer("MyBillboardRenderer", 131072, sizeof(MyVertexFormatPositionTextureH), null, ResourceUsage.Dynamic, isStreamOutput: false, isGlobal: true);
			int byteStride = sizeof(MyBillboardData);
			m_SB = MyManagers.Buffers.CreateSrv("MyBillboardRenderer", 32768, byteStride, null, ResourceUsage.Dynamic, isGlobal: true);
			m_atlas = new MyTextureAtlas("Textures\\Particles\\", "Textures\\Particles\\ParticlesAtlas.tai");
		}

		internal static void FinalizeInit()
		{
			m_atlas.LoadTextures();
		}

		internal static void ClearBillboardsOnce()
		{
			m_billboardsOncePool.ClearAllAllocated();
		}

		internal static void OnFrameStart()
		{
			ClearBillboardsOnce();
		}

		private unsafe static void InitBillboardsIndexBuffer()
		{
			if (m_IB != null)
			{
				MyManagers.Buffers.Dispose(m_IB);
			}
			uint[] array = new uint[196608];
			for (int i = 0; i < 32768; i++)
			{
				array[i * 6] = (uint)(i * 4);
				array[i * 6 + 1] = (uint)(i * 4 + 1);
				array[i * 6 + 2] = (uint)(i * 4 + 2);
				array[i * 6 + 3] = (uint)(i * 4);
				array[i * 6 + 4] = (uint)(i * 4 + 2);
				array[i * 6 + 5] = (uint)(i * 4 + 3);
			}
			fixed (uint* value = array)
			{
				m_IB = MyManagers.Buffers.CreateIndexBuffer("MyBillboardRenderer", 196608, new IntPtr(value), MyIndexBufferFormat.UInt, ResourceUsage.Immutable, isGlobal: true);
			}
		}

		internal static void OnDeviceReset()
		{
			InitBillboardsIndexBuffer();
			m_fileTextures.Clear();
		}

		internal static void OnDeviceEnd()
		{
			m_atlas?.Dispose();
			m_atlas = null;
		}

		internal static void OnSessionEnd()
		{
			m_fileTextures.Clear();
		}

		private static int GetBillboardBucket(MyBillboard billboard)
		{
			return (int)billboard.BlendType;
		}

		private static int PrepareList()
		{
			m_batches.Clear();
			for (int i = 0; i < 6; i++)
			{
				m_bucketCounts[i] = 0;
			}
			foreach (MyBillboard item in MyRenderProxy.BillboardsRead)
			{
				m_bucketCounts[GetBillboardBucket(item)]++;
			}
			int allocatedCount = m_billboardsOncePool.GetAllocatedCount();
			for (int j = 0; j < allocatedCount; j++)
			{
				MyBillboard allocatedItem = m_billboardsOncePool.GetAllocatedItem(j);
				m_bucketCounts[GetBillboardBucket(allocatedItem)]++;
			}
			MyRenderProxy.ApplyActionOnPersistentBillboards(delegate(MyBillboard x)
			{
				m_bucketCounts[GetBillboardBucket(x)]++;
			});
			int num = 0;
			for (int k = 0; k < 6; k++)
			{
				num += m_bucketCounts[k];
			}
			if (num == 0)
			{
				return 0;
			}
			int num2 = ((num > 32768) ? 32768 : num);
			int num3 = m_tempBuffer.Length;
			while (num > num3)
			{
				num3 *= 2;
			}
			Array.Resize(ref m_tempBuffer, num3);
			num3 = m_arrayDataBillboards.Length;
			while (num2 > num3)
			{
				num3 *= 2;
			}
			m_arrayDataBillboards.Resize(num3);
			for (int l = 0; l < 6; l++)
			{
				m_bucketBatches[l] = default(MyBucketBatches);
			}
			m_lastBatchOffset = 0;
			m_bucketIndices[0] = 0;
			for (int m = 1; m < 6; m++)
			{
				m_bucketIndices[m] = m_bucketIndices[m - 1] + m_bucketCounts[m - 1];
			}
			foreach (MyBillboard item2 in MyRenderProxy.BillboardsRead)
			{
				m_tempBuffer[m_bucketIndices[GetBillboardBucket(item2)]++] = item2;
			}
			for (int n = 0; n < allocatedCount; n++)
			{
				MyBillboard allocatedItem2 = m_billboardsOncePool.GetAllocatedItem(n);
				m_tempBuffer[m_bucketIndices[GetBillboardBucket(allocatedItem2)]++] = allocatedItem2;
			}
			MyRenderProxy.ApplyActionOnPersistentBillboards(delegate(MyBillboard x)
			{
				m_tempBuffer[m_bucketIndices[GetBillboardBucket(x)]++] = x;
			});
			m_bucketIndices[0] = 0;
			for (int num4 = 1; num4 < 6; num4++)
			{
				m_bucketIndices[num4] = m_bucketIndices[num4 - 1] + m_bucketCounts[num4 - 1];
			}
			for (int num5 = 0; num5 < 6; num5++)
			{
				if (num5 != 3 && num5 != 4)
				{
					Array.Sort(m_tempBuffer, m_bucketIndices[num5], m_bucketCounts[num5]);
				}
			}
			return num2;
		}

		private static void GatherInternal(MyRenderContext rc)
		{
			int offset = 0;
			ISrvBindable srvBindable = null;
			MyTransparentMaterial myTransparentMaterial = null;
			MyStringId myStringId = default(MyStringId);
			ISrvBindable srvBindable2 = null;
			MyPolyLineD polyLine = default(MyPolyLineD);
			for (int i = 0; i < m_billboardCountSafe; i++)
			{
				MyBillboard myBillboard = m_tempBuffer[i];
				if (myBillboard == null)
				{
					continue;
				}
				MyTransparentMaterial material = MyTransparentMaterials.GetMaterial(myBillboard.Material);
				ISrvBindable srvBindable3;
				if (myStringId == myBillboard.Material)
				{
					srvBindable3 = srvBindable2;
				}
				else
				{
					bool flag = true;
					if (material.UseAtlas)
					{
						srvBindable3 = m_atlas.FindElement(material.Texture).Texture.Texture;
					}
					else
					{
						switch (material.TextureType)
						{
						case MyTransparentMaterialTextureType.FileTexture:
						{
							srvBindable3 = null;
							IMyStreamedTexture value = null;
							if (material.Texture != null && !m_fileTextures.TryGetValue(material.Texture, out value))
							{
								value = MyManagers.Textures.GetTexture(material.Texture, MyFileTextureEnum.GUI);
								if (value.Texture.IsTextureLoaded())
								{
									m_fileTextures.Add(material.Texture, value);
								}
								else
								{
									value.Touch(32767);
									value = null;
								}
							}
							if (value != null)
							{
								value.Touch(32767);
								srvBindable3 = value.Texture;
							}
							else
							{
								MyManagers.FileTextures.TryGetTexture("EMPTY", out ITexture texture);
								srvBindable3 = texture;
							}
							break;
						}
						case MyTransparentMaterialTextureType.RenderTarget:
						{
							flag = false;
							MySpriteMessageData messages = MyManagers.SpritesManager.AcquireDrawMessages(material.Id.String);
							srvBindable3 = MyRender11.DrawSpritesOffscreen(messages, material.Id.String, material.TargetSize.X, material.TargetSize.Y);
							MyManagers.SpritesManager.DisposeDrawMessages(messages);
							break;
						}
						default:
							throw new Exception();
						}
					}
					if (flag)
					{
						srvBindable2 = srvBindable3;
						myStringId = myBillboard.Material;
					}
				}
				bool flag2 = IsBucketBoundary(i);
				if (i > 0 && (srvBindable3 != srvBindable || flag2))
				{
					AddBatch(i, offset, srvBindable, myTransparentMaterial);
					offset = i;
				}
				MyBillboardData myBillboardData = default(MyBillboardData);
				MyBillboardVertexData myBillboardVertexData = default(MyBillboardVertexData);
				myBillboardData.CustomProjectionID = myBillboard.CustomViewProjection;
				myBillboardData.Color = myBillboard.Color;
				myBillboardData.Color.X *= myBillboard.ColorIntensity;
				myBillboardData.Color.Y *= myBillboard.ColorIntensity;
				myBillboardData.Color.Z *= myBillboard.ColorIntensity;
				myBillboardData.AlphaCutout = myBillboard.AlphaCutout;
				myBillboardData.AlphaSaturation = material.AlphaSaturation;
				myBillboardData.SoftParticleDistanceScale = myBillboard.SoftParticleDistanceScale * material.SoftParticleDistanceScale;
				myBillboardData.Reflective = myBillboard.Reflectivity;
				MyEnvironmentMatrices myEnvironmentMatrices = MyRender11.Environment.Matrices;
				if (MyStereoRender.Enable)
				{
					if (MyStereoRender.RenderRegion == MyStereoRegion.LEFT)
					{
						myEnvironmentMatrices = MyStereoRender.EnvMatricesLeftEye;
					}
					else if (MyStereoRender.RenderRegion == MyStereoRegion.RIGHT)
					{
						myEnvironmentMatrices = MyStereoRender.EnvMatricesRightEye;
					}
				}
				Vector3D position = myBillboard.Position0;
				Vector3D normal = myBillboard.Position1;
				Vector3D position2 = myBillboard.Position2;
				Vector3D position3 = myBillboard.Position3;
				if (myBillboard.ParentID != uint.MaxValue)
				{
					MyActor myActor = MyIDTracker<MyActor>.FindByID(myBillboard.ParentID);
					if (myActor != null)
					{
						MatrixD matrix = myActor.WorldMatrix;
						if (myBillboard.LocalType == MyBillboard.LocalTypeEnum.Line)
						{
							Vector3D.Transform(ref position, ref matrix, out position);
							Vector3D.TransformNormal(ref normal, ref matrix, out normal);
						}
						else if (myBillboard.LocalType == MyBillboard.LocalTypeEnum.Point)
						{
							Vector3D.Transform(ref position, ref matrix, out position);
						}
						else
						{
							Vector3D.Transform(ref position, ref matrix, out position);
							Vector3D.Transform(ref normal, ref matrix, out normal);
							Vector3D.Transform(ref position2, ref matrix, out position2);
							Vector3D.Transform(ref position3, ref matrix, out position3);
						}
					}
				}
				if (myBillboard.LocalType == MyBillboard.LocalTypeEnum.Line)
				{
					Vector3D vector3D = position;
					Vector3D vector3D2 = normal;
					polyLine.LineDirectionNormalized = vector3D2;
					polyLine.Point0 = position;
					polyLine.Point1 = position + vector3D2 * position2.X;
					polyLine.Thickness = (float)position2.Y;
					Vector3D vector3D3 = ((myBillboard.CustomViewProjection == -1) ? myEnvironmentMatrices.CameraPosition : MyRenderProxy.BillboardsViewProjectionRead[myBillboard.CustomViewProjection].CameraPosition);
					if (Vector3D.IsZero(vector3D3 - polyLine.Point0, 1E-06))
					{
						m_billboardCountSafe--;
						i--;
						continue;
					}
					MyUtils.GetPolyLineQuad(out var retQuad, ref polyLine, vector3D3);
					position = retQuad.Point0;
					normal = retQuad.Point1;
					position2 = retQuad.Point2;
					position3 = retQuad.Point3;
					float num = 1f - Math.Abs(Vector3.Dot(MyUtils.Normalize(myEnvironmentMatrices.CameraPosition - vector3D), vector3D2));
					float num2 = (1f - (float)Math.Pow(1f - num, 30.0)) * 0.5f;
					myBillboardData.Color.X *= num2;
					myBillboardData.Color.Y *= num2;
					myBillboardData.Color.Z *= num2;
				}
				else if (myBillboard.LocalType == MyBillboard.LocalTypeEnum.Point)
				{
					Vector3D position4 = position;
					float num3 = (float)position2.X;
					float angle = (float)position2.Y;
					if (!MyUtils.GetBillboardQuadAdvancedRotated(out var quad, position4, num3, num3, angle, myEnvironmentMatrices.CameraPosition))
					{
						m_billboardCountSafe--;
						i--;
						continue;
					}
					position = quad.Point0;
					normal = quad.Point1;
					position2 = quad.Point2;
					position3 = quad.Point3;
				}
				if (myBillboard.CustomViewProjection == -1)
				{
					position -= myEnvironmentMatrices.CameraPosition;
					normal -= myEnvironmentMatrices.CameraPosition;
					position2 -= myEnvironmentMatrices.CameraPosition;
					position3 -= myEnvironmentMatrices.CameraPosition;
				}
				Vector3D vector3D4 = Vector3D.Cross(normal - position, position2 - position);
				vector3D4.Normalize();
				myBillboardData.Normal = vector3D4;
				myBillboardVertexData.V0.Position = position;
				myBillboardVertexData.V1.Position = normal;
				myBillboardVertexData.V2.Position = position2;
				myBillboardVertexData.V3.Position = position3;
				Vector2 vector = new Vector2(material.UVOffset.X + myBillboard.UVOffset.X, material.UVOffset.Y + myBillboard.UVOffset.Y);
				Vector2 vector2 = new Vector2(material.UVOffset.X + material.UVSize.X * myBillboard.UVSize.X + myBillboard.UVOffset.X, material.UVOffset.Y + myBillboard.UVOffset.Y);
				Vector2 vector3 = new Vector2(material.UVOffset.X + material.UVSize.X * myBillboard.UVSize.X + myBillboard.UVOffset.X, material.UVOffset.Y + myBillboard.UVOffset.Y + material.UVSize.Y * myBillboard.UVSize.Y);
				Vector2 vector4 = new Vector2(material.UVOffset.X + myBillboard.UVOffset.X, material.UVOffset.Y + myBillboard.UVOffset.Y + material.UVSize.Y * myBillboard.UVSize.Y);
				if (material.UseAtlas)
				{
					MyTextureAtlas.Element element = m_atlas.FindElement(material.Texture);
					vector = vector * new Vector2(element.UvOffsetScale.Z, element.UvOffsetScale.W) + new Vector2(element.UvOffsetScale.X, element.UvOffsetScale.Y);
					vector2 = vector2 * new Vector2(element.UvOffsetScale.Z, element.UvOffsetScale.W) + new Vector2(element.UvOffsetScale.X, element.UvOffsetScale.Y);
					vector3 = vector3 * new Vector2(element.UvOffsetScale.Z, element.UvOffsetScale.W) + new Vector2(element.UvOffsetScale.X, element.UvOffsetScale.Y);
					vector4 = vector4 * new Vector2(element.UvOffsetScale.Z, element.UvOffsetScale.W) + new Vector2(element.UvOffsetScale.X, element.UvOffsetScale.Y);
				}
				myBillboardVertexData.V0.Texcoord = new HalfVector2(vector);
				myBillboardVertexData.V1.Texcoord = new HalfVector2(vector2);
				myBillboardVertexData.V2.Texcoord = new HalfVector2(vector3);
				myBillboardVertexData.V3.Texcoord = new HalfVector2(vector4);
				MyTriangleBillboard myTriangleBillboard = myBillboard as MyTriangleBillboard;
				if (myTriangleBillboard != null)
				{
					myBillboardVertexData.V3.Position = position2;
					myBillboardVertexData.V0.Texcoord = new HalfVector2(myTriangleBillboard.UV0);
					myBillboardVertexData.V1.Texcoord = new HalfVector2(myTriangleBillboard.UV1);
					myBillboardVertexData.V2.Texcoord = new HalfVector2(myTriangleBillboard.UV2);
					myBillboardData.Normal = myTriangleBillboard.Normal0;
				}
				m_arrayDataBillboards.Data[i] = myBillboardData;
				m_arrayDataBillboards.Vertex[i] = myBillboardVertexData;
				srvBindable = srvBindable3;
				myTransparentMaterial = material;
			}
			if (m_billboardCountSafe > 0 && myTransparentMaterial != null)
			{
				AddBatch(m_billboardCountSafe, offset, srvBindable, myTransparentMaterial);
			}
		}

		public static void TransferData(MyRenderContext rc)
		{
			TransferDataCustomProjections(rc);
			TransferDataBillboards(rc, 0, m_billboardCountSafe, m_arrayDataBillboards.Data);
			TransferDataBillboards(rc, 0, m_billboardCountSafe, m_arrayDataBillboards.Vertex);
		}

		private static int GetBucketIndex(int i)
		{
			for (int j = 0; j < 5; j++)
			{
				if (i < m_bucketIndices[j + 1])
				{
					return j;
				}
			}
			return -1;
		}

		private static bool IsBucketBoundary(int i)
		{
			for (int j = 0; j < 5; j++)
			{
				if (i == m_bucketIndices[j + 1])
				{
					return true;
				}
			}
			return false;
		}

		private static void AddBatch(int counter, int offset, ISrvBindable prevTexture, MyTransparentMaterial prevMaterial)
		{
			MyBillboardRendererBatch item = default(MyBillboardRendererBatch);
			item.Offset = offset;
			item.Num = counter - offset;
			item.Texture = prevTexture;
			item.Lit = prevMaterial.CanBeAffectedByOtherLights;
			item.AlphaCutout = prevMaterial.AlphaCutout;
			item.SingleChannel = prevTexture.Srv.Description.Format == Format.BC4_UNorm;
			m_batches.Add(item);
			if (counter == m_billboardCountSafe || IsBucketBoundary(counter))
			{
				int count = m_batches.Count;
				m_bucketBatches[GetBucketIndex(counter - 1)] = new MyBucketBatches
				{
					StartIndex = m_lastBatchOffset,
					Count = count - m_lastBatchOffset
				};
				m_lastBatchOffset = count;
			}
		}

		private static void TransferDataCustomProjections(MyRenderContext rc)
		{
			MyMapping myMapping = MyMapping.MapDiscard(rc, m_cbCustomProjections);
			for (int i = 0; i < MyRenderProxy.BillboardsViewProjectionRead.Count; i++)
			{
				MyBillboardViewProjection myBillboardViewProjection = MyRenderProxy.BillboardsViewProjectionRead[i];
				float m = myBillboardViewProjection.Viewport.Width / (float)MyRender11.ViewportResolution.X;
				float m2 = myBillboardViewProjection.Viewport.Height / (float)MyRender11.ViewportResolution.Y;
				float m3 = myBillboardViewProjection.Viewport.OffsetX / (float)MyRender11.ViewportResolution.X;
				float m4 = ((float)MyRender11.ViewportResolution.Y - myBillboardViewProjection.Viewport.OffsetY - myBillboardViewProjection.Viewport.Height) / (float)MyRender11.ViewportResolution.Y;
				Matrix matrix = new Matrix(m, 0f, 0f, 0f, 0f, m2, 0f, 0f, 0f, 0f, 1f, 0f, m3, m4, 0f, 1f);
				Matrix data = Matrix.Transpose(myBillboardViewProjection.ViewAtZero * myBillboardViewProjection.Projection * matrix);
				myMapping.WriteAndPosition(ref data);
			}
			for (int j = MyRenderProxy.BillboardsViewProjectionRead.Count; j < 32; j++)
			{
				myMapping.WriteAndPosition(ref Matrix.Identity);
			}
			myMapping.Unmap();
		}

		private static void TransferDataBillboards(MyRenderContext rc, int offset, int billboardCount, MyBillboardData[] data)
		{
			MyMapping myMapping = MyMapping.MapDiscard(rc, m_SB);
			myMapping.WriteAndPosition(data, billboardCount, offset);
			myMapping.Unmap();
		}

		private static void TransferDataBillboards(MyRenderContext rc, int offset, int billboardCount, MyBillboardVertexData[] data)
		{
			MyMapping myMapping = MyMapping.MapDiscard(rc, m_VB);
			myMapping.WriteAndPosition(data, billboardCount, offset);
			myMapping.Unmap();
		}

<<<<<<< HEAD
		/// <summary>
		///
		/// </summary>
		/// <param name="rc"></param>
		/// <param name="immediateContext"></param>        
=======
		/// <param name="handleWindow">Handle function for window billboards: decides if
		/// keeping it in separate storage list</param>
		/// <returns>True if the transparent geometry bindings must be reset</returns>
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static void Gather(MyRenderContext rc, bool immediateContext)
		{
			m_stats = default(MyRendererStats.MyRenderStats);
			m_batches.Clear();
			m_billboardCountSafe = PrepareList();
			if (m_billboardCountSafe != 0)
			{
				GatherInternal(rc);
			}
			if (immediateContext)
			{
				TransferData(rc);
			}
		}

		public static void RenderAdditiveBottom(MyRenderContext rc, ISrvBindable depthRead)
		{
			rc.SetBlendState(MyBlendStateManager.BlendAdditive);
			if (MyStereoRender.Enable && MyStereoRender.EnableUsingStencilMask)
			{
				rc.SetDepthStencilState(MyDepthStencilStateManager.StereoDefaultDepthState);
			}
			else
			{
				rc.SetDepthStencilState(MyDepthStencilStateManager.DefaultDepthState);
			}
			rc.PixelShader.SetSrv(4, null);
			rc.SetRtv(MyGBuffer.Main.ResolvedDepthStencil.DsvRoDepth, MyGBuffer.Main.LBuffer);
			rc.SetScreenViewport();
			Render(rc, depthRead, m_bucketBatches[1], oit: false);
			rc.SetRtvNull();
		}

		public static void RenderAdditiveTop(MyRenderContext rc)
		{
			rc.SetBlendState(MyBlendStateManager.BlendAdditive);
			rc.SetDepthStencilState(MyDepthStencilStateManager.IgnoreDepthStencil);
			rc.SetRtv(MyGBuffer.Main.LBuffer);
			rc.SetScreenViewport();
			Render(rc, null, m_bucketBatches[2], oit: false);
			rc.SetRtvNull();
		}

		public static void RenderLDR(MyRenderContext rc, ISrvBindable depthRead, IRtvBindable target)
		{
			rc.SetViewport(0f, 0f, target.Size.X, target.Size.Y);
			rc.SetBlendState(MyBlendStateManager.BlendAlphaPremult);
			rc.SetDepthStencilState(MyDepthStencilStateManager.DefaultDepthState);
			rc.SetRtv(MyGBuffer.Main.ResolvedDepthStencil.DsvRoDepth, target);
			Render(rc, depthRead, m_bucketBatches[3], oit: false, ldr: true);
			rc.SetRtvNull();
		}

		public static void RenderPostPP(MyRenderContext rc, ISrvBindable depthRead, IRtvBindable target)
		{
			rc.SetViewport(0f, 0f, target.Size.X, target.Size.Y);
			rc.SetBlendState(MyBlendStateManager.BlendAlphaPremult);
			rc.SetDepthStencilState(MyDepthStencilStateManager.DefaultDepthState);
			rc.SetRtv(MyGBuffer.Main.ResolvedDepthStencil.DsvRoDepth, target);
			Render(rc, depthRead, m_bucketBatches[4], oit: false, ldr: true);
			rc.SetRtvNull();
		}

		internal static void RenderStandard(MyRenderContext rc, ISrvBindable depthRead)
		{
			if (MyStereoRender.Enable && MyStereoRender.EnableUsingStencilMask)
			{
				rc.SetDepthStencilState(MyDepthStencilStateManager.StereoDefaultDepthState);
			}
			else
			{
				rc.SetDepthStencilState(MyDepthStencilStateManager.DefaultDepthState);
			}
			rc.SetScreenViewport();
			Render(rc, depthRead, m_bucketBatches[0], oit: true);
		}

		private static void Render(MyRenderContext rc, ISrvBindable depthRead, MyBucketBatches bucketBatches, bool oit, bool ldr = false)
		{
			if ((!MyRender11.DebugOverrides.BillboardsDynamic && !MyRender11.DebugOverrides.BillboardsStatic) || m_batches.Count == 0 || bucketBatches.Count == 0)
			{
				return;
			}
			rc.PixelShader.SetSrv(1, depthRead);
			BindResourcesCommon(rc);
			rc.SetPrimitiveTopology(PrimitiveTopology.TriangleList);
			PixelShaderFlags pixelShaderFlags = (MyTransparentRendering.DisplayTransparencyHeatMap() ? PixelShaderFlags.DEBUG_UNIFORM_ACCUM : ((PixelShaderFlags)0)) | ((MyRender11.DebugOverrides.OIT && oit) ? PixelShaderFlags.OIT : ((PixelShaderFlags)0)) | ((depthRead != null) ? PixelShaderFlags.SOFT_PARTICLE : ((PixelShaderFlags)0));
			for (int i = bucketBatches.StartIndex; i < bucketBatches.StartIndex + bucketBatches.Count; i++)
			{
				MyPixelShaders.Id id = m_psBundle[(uint)pixelShaderFlags | (uint)(m_batches[i].Lit ? 2 : 0) | (uint)(m_batches[i].AlphaCutout ? 4 : 0) | (uint)(m_batches[i].SingleChannel ? 32 : 0)];
				rc.VertexShader.Set(ldr ? m_vsLDR : (m_batches[i].Lit ? m_vsLit : m_vs));
				rc.PixelShader.Set(id);
				ISrvBindable texture = m_batches[i].Texture;
				rc.PixelShader.SetSrv(0, texture);
				if (!MyStereoRender.Enable)
				{
					rc.DrawIndexed(m_batches[i].Num * 6, m_batches[i].Offset * 6, 0);
				}
				else
				{
					MyStereoRender.DrawIndexedBillboards(rc, m_batches[i].Num * 6, m_batches[i].Offset * 6, 0);
				}
				(texture as IBorrowedRtvTexture)?.Release();
				m_stats.Draws++;
				m_stats.Triangles += m_batches[i].Num * 2;
			}
			rc.SetRasterizerState(null);
			MyRendererStats.AddBillboardRenderStats(ref m_stats);
		}

		internal static MyBillboard AddBillboardOnce()
		{
			return m_billboardsOncePool.Allocate();
		}

		private static void BindResourcesCommon(MyRenderContext rc)
		{
			rc.SetRasterizerState(MyRasterizerStateManager.NocullRasterizerState);
			rc.AllShaderStages.SetSrv(30, m_SB);
			rc.AllShaderStages.SetConstantBuffer(2, m_cbCustomProjections);
			rc.VertexShader.SetConstantBuffer(0, MyCommon.FrameConstants);
			rc.AllShaderStages.SetConstantBuffer(4, MyManagers.Shadows.ShadowCascades.CascadeConstantBuffer);
			rc.VertexShader.SetSampler(15, MySamplerStateManager.Shadowmap);
			rc.VertexShader.SetSrv(15, MyManagers.Shadows.ShadowCascades.CascadeShadowmapArray);
			rc.VertexShader.SetSamplers(0, MySamplerStateManager.StandardSamplers);
			rc.VertexShader.SetSrv(2, MyEyeAdaptation.GetExposure());
			rc.PixelShader.SetSrv(11, MyManagers.EnvironmentProbe.CloseCubemapFinal);
			rc.PixelShader.SetSrv(17, MyManagers.EnvironmentProbe.FarCubemapFinal);
			rc.SetVertexBuffer(0, m_VB);
			rc.SetIndexBuffer(m_IB);
			rc.SetInputLayout(m_inputLayout);
		}
	}
}
