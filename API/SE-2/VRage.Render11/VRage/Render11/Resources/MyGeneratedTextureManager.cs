using System;
<<<<<<< HEAD
=======
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Generics;
using VRage.Library.Utils;
using VRage.Render11.Common;
using VRage.Render11.Resources.Internal;
using VRageMath;
using VRageRender.Messages;

namespace VRage.Render11.Resources
{
	internal class MyGeneratedTextureManager : IManager, IManagerDevice, IManagerUnloadData
	{
		public static IGeneratedTexture ZeroTex = new MyGeneratedTexture();

		public static IGeneratedTexture ReleaseMissingNormalGlossTex = new MyGeneratedTexture();

		public static IGeneratedTexture ReleaseMissingAlphamaskTex = new MyGeneratedTexture();

		public static IGeneratedTexture ReleaseMissingExtensionTex = new MyGeneratedTexture();

		public static IGeneratedTexture ReleaseMissingCubeTex = new MyGeneratedTexture();

		public static IGeneratedTexture PinkTex = new MyGeneratedTexture();

		public static IGeneratedTexture RedTex = new MyGeneratedTexture();

		public static IGeneratedTexture GreenTex = new MyGeneratedTexture();

		public static IGeneratedTexture DebugMissingNormalGlossTex = new MyGeneratedTexture();

		public static IGeneratedTexture DebugMissingAlphamaskTex = new MyGeneratedTexture();

		public static IGeneratedTexture DebugMissingExtensionTex = new MyGeneratedTexture();

		public static IGeneratedTexture DebugMissingCubeTex = new MyGeneratedTexture();

		public static IGeneratedTexture DebugCheckerCMTex = new MyGeneratedTexture();

		private static MySwitchableDebugTexture m_missingNormalGlossTex = new MySwitchableDebugTexture();

		private static MySwitchableDebugTexture m_missingAlphamaskTex = new MySwitchableDebugTexture();

		private static MySwitchableDebugTexture m_missingExtensionTex = new MySwitchableDebugTexture();

		private static MySwitchableDebugTexture m_missingCubeTex = new MySwitchableDebugTexture();

		private static MySwitchableDebugTexture m_missingColorMetal = new MySwitchableDebugTexture();

		public static IGeneratedTexture IntelFallbackCubeTex = new MyGeneratedTexture();

		public static IGeneratedTexture Dithering8x8Tex = new MyGeneratedTexture();

		public static IGeneratedTexture RandomTex = new MyGeneratedTexture();

		private static DataBox[] m_tmpDataBoxArray1 = new DataBox[1];

		private static DataBox[] m_tmpDataBoxArray6 = new DataBox[6];

		private static readonly Texture2DDescription m_descDefault = new Texture2DDescription
		{
			ArraySize = 1,
			BindFlags = BindFlags.ShaderResource,
			Format = Format.R8G8B8A8_UNorm,
			Height = 0,
			Width = 0,
			Usage = ResourceUsage.Immutable,
			MipLevels = 1,
			SampleDescription = new SampleDescription
			{
				Count = 1,
				Quality = 0
			}
		};

		private MyTextureStatistics m_statistics = new MyTextureStatistics(MyManagers.TexturesMemoryTracker.RegisterSubsystem("GeneratedTextures"));

		private MyObjectsPool<MyUserGeneratedTexture> m_objectsPoolGenerated = new MyObjectsPool<MyUserGeneratedTexture>(16);

		private MyObjectsPool<MyGeneratedTextureFromPattern> m_objectsPoolGeneratedFromPattern = new MyObjectsPool<MyGeneratedTextureFromPattern>(16);

		public static IGeneratedTexture MissingNormalGlossTex => m_missingNormalGlossTex;

		public static IGeneratedTexture MissingAlphamaskTex => m_missingAlphamaskTex;

		public static IGeneratedTexture MissingExtensionTex => m_missingExtensionTex;

		public static IGeneratedTexture MissingCubeTex => m_missingCubeTex;

		public static IGeneratedTexture MissingColorMetal => m_missingColorMetal;

		public int ActiveTexturesCount => m_objectsPoolGenerated.ActiveCount + m_objectsPoolGeneratedFromPattern.ActiveCount;

		public MyTextureStatistics Statistics => m_statistics;

		public IUserGeneratedTexture NewUserTexture(string name, int width, int height, MyGeneratedTextureType type, bool generateMipmaps, bool immediatelyReady, byte[] data = null)
		{
			m_objectsPoolGenerated.AllocateOrCreate(out var item);
			item.IsLoaded = immediatelyReady;
			switch (type)
			{
			case MyGeneratedTextureType.RGBA:
				CreateRGBA(item, name, new Vector2I(width, height), srgb: true, data, userTexture: true, generateMipmaps);
				break;
			case MyGeneratedTextureType.RGBA_Linear:
				CreateRGBA(item, name, new Vector2I(width, height), srgb: false, data, userTexture: true, generateMipmaps);
				break;
			case MyGeneratedTextureType.Alphamask:
				CreateR(item, name, new Vector2I(width, height), null, userTexture: true, generateMipmaps);
				break;
			default:
				throw new Exception();
			}
			m_statistics.Add(item);
			return item;
		}

		public IGeneratedTexture CreateFromBytePattern(string name, int width, int height, Format format, byte[] pattern)
		{
			m_objectsPoolGeneratedFromPattern.AllocateOrCreate(out var item);
			item.Init(name, new Vector2I(width, height), format, pattern);
			m_statistics.Add(item);
			return item;
		}

		internal static void ResetUserTexture(MyUserGeneratedTexture texture, byte[] data)
		{
			switch (texture.Format)
			{
			case Format.R8G8B8A8_UNorm:
			case Format.R8G8B8A8_UNorm_SRgb:
				Reset(texture, data, 4);
				break;
			case Format.R8_UNorm:
				Reset(texture, data, 1);
				break;
			default:
				throw new Exception();
			}
		}

		private void CreateR_1x1(MyGeneratedTexture tex, string name, byte data)
		{
			Texture2DDescription descDefault = m_descDefault;
			descDefault.Format = Format.R8_UNorm;
			descDefault.Height = 1;
			descDefault.Width = 1;
			tex.Init(name, descDefault, new Vector2I(1, 1), isGeneratingMipmaps: false);
			Reset(tex, data);
		}

		private unsafe static void Reset(MyGeneratedTexture tex, byte data)
		{
			void* value = &data;
			m_tmpDataBoxArray1[0].DataPointer = new IntPtr(value);
			m_tmpDataBoxArray1[0].RowPitch = 1;
			tex.Reset(m_tmpDataBoxArray1);
		}

		private void CreateRGBA_1x1(MyGeneratedTexture tex, string name, Color color)
		{
			Texture2DDescription descDefault = m_descDefault;
			descDefault.Format = Format.R8G8B8A8_UNorm;
			descDefault.Height = 1;
			descDefault.Width = 1;
			tex.Init(name, descDefault, new Vector2I(1, 1), isGeneratingMipmaps: false);
			uint packedValue = color.PackedValue;
			Reset(tex, packedValue);
		}

		private unsafe static void Reset(MyGeneratedTexture tex, uint data)
		{
			void* value = &data;
			m_tmpDataBoxArray1[0].DataPointer = new IntPtr(value);
			m_tmpDataBoxArray1[0].RowPitch = 4;
			tex.Reset(m_tmpDataBoxArray1);
		}

		private void CreateCubeRGBA_1x1(MyGeneratedTexture tex, string name, Color color)
		{
			Texture2DDescription descDefault = m_descDefault;
			descDefault.Format = Format.R8G8B8A8_UNorm;
			descDefault.Height = 1;
			descDefault.Width = 1;
			descDefault.ArraySize = 6;
			descDefault.OptionFlags = ResourceOptionFlags.TextureCube;
			tex.Init(name, descDefault, new Vector2I(1, 1), isGeneratingMipmaps: false);
			uint packedValue = color.PackedValue;
			ResetCube(tex, packedValue);
		}

		private unsafe static void ResetCube(MyGeneratedTexture tex, uint data)
		{
			void* value = &data;
			for (int i = 0; i < 6; i++)
			{
				m_tmpDataBoxArray6[i].DataPointer = new IntPtr(value);
				m_tmpDataBoxArray6[i].RowPitch = 4;
			}
			tex.Reset(m_tmpDataBoxArray6);
		}

		private void CreateR(MyGeneratedTexture tex, string name, Vector2I resolution, byte[] data, bool userTexture = false, bool generateMipmaps = false)
		{
			Texture2DDescription descDefault = m_descDefault;
			descDefault.Usage = ((!userTexture) ? ResourceUsage.Immutable : ResourceUsage.Default);
			descDefault.Format = Format.R8_UNorm;
			int x = resolution.X;
			int num = (descDefault.Height = resolution.Y);
			descDefault.Width = x;
			if (!generateMipmaps)
			{
				descDefault.MipLevels = 1;
				descDefault.OptionFlags = ResourceOptionFlags.None;
			}
			else
			{
				descDefault.MipLevels = MyResourceUtils.GetMipLevels(Math.Max(x, num));
				descDefault.OptionFlags = ResourceOptionFlags.GenerateMipMaps;
				descDefault.BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget;
			}
			tex.Init(name, descDefault, new Vector2I(x, num), generateMipmaps);
			if (data != null)
			{
				Reset(tex, data, 1);
			}
		}

		private unsafe static void Reset(MyGeneratedTexture tex, byte[] data, int nchannels)
		{
			if (data == null)
			{
				tex.Reset(null);
				return;
			}
			fixed (byte* ptr = data)
			{
				int mipLevels = tex.MipLevels;
				DataBox[] array = new DataBox[mipLevels];
				int num = tex.Size.X;
				int num2 = tex.Size.Y;
				int num3 = 0;
				for (int i = 0; i < mipLevels; i++)
				{
					array[i].DataPointer = new IntPtr(ptr + num3);
					array[i].RowPitch = num * nchannels;
					num3 += num * num2 * nchannels;
					num >>= 1;
					num2 >>= 1;
				}
				tex.Reset(array);
			}
		}

		private void CreateRGBA(MyGeneratedTexture tex, string name, Vector2I resolution, bool srgb, Color[] colors, bool generateMipmaps = false)
		{
			int x = resolution.X;
			int y = resolution.Y;
			byte[] array = new byte[x * y * 4];
			for (int i = 0; i < y; i++)
			{
				for (int j = 0; j < x; j++)
				{
					int num = j + i * x;
					Color color = colors[num];
					array[num * 4] = color.R;
					array[num * 4 + 1] = color.G;
					array[num * 4 + 2] = color.B;
					array[num * 4 + 3] = color.A;
				}
			}
			CreateRGBA(tex, name, resolution, srgb, array, generateMipmaps);
		}

		private void CreateRGBA(MyGeneratedTexture tex, string name, Vector2I resolution, bool srgb, byte[] data, bool userTexture = false, bool generateMipmaps = false)
		{
			Texture2DDescription descDefault = m_descDefault;
			descDefault.Usage = ((!userTexture) ? ResourceUsage.Immutable : ResourceUsage.Default);
			descDefault.Format = (srgb ? Format.R8G8B8A8_UNorm_SRgb : Format.B8G8R8A8_UNorm);
			int x = resolution.X;
			int y = resolution.Y;
			descDefault.Width = x;
			descDefault.Height = y;
			if (!generateMipmaps)
			{
				descDefault.MipLevels = 1;
				descDefault.OptionFlags = ResourceOptionFlags.None;
			}
			else
			{
				descDefault.MipLevels = MyResourceUtils.GetMipLevels(Math.Max(x, y));
				descDefault.OptionFlags = ResourceOptionFlags.GenerateMipMaps;
				descDefault.BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget;
			}
			tex.Init(name, descDefault, new Vector2I(x, y), generateMipmaps);
			if (data != null)
			{
				Reset(tex, data, 4);
			}
		}

		private void CreateR32G32B32A32_Float(MyGeneratedTexture tex, string name, Vector2I resolution, Vector4[] colors)
		{
			Texture2DDescription descDefault = m_descDefault;
			descDefault.Format = Format.R32G32B32A32_Float;
			int x = resolution.X;
			int y = resolution.Y;
			descDefault.Height = x;
			descDefault.Width = y;
			tex.Init(name, descDefault, new Vector2I(x, y), isGeneratingMipmaps: false);
			if (colors == null)
			{
				return;
			}
			float[] array = new float[x * y * 4];
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < y; i++)
			{
				for (int j = 0; j < x; j++)
				{
					array[num2++] = colors[num].X;
					array[num2++] = colors[num].Y;
					array[num2++] = colors[num].Z;
					array[num2++] = colors[num].W;
					num++;
				}
			}
			Reset(tex, array, x, 4);
		}

		private unsafe static void Reset(MyGeneratedTexture tex, float[] data, int rowlength, int nchannels)
		{
			fixed (float* value = data)
			{
				m_tmpDataBoxArray1[0].DataPointer = new IntPtr(value);
				m_tmpDataBoxArray1[0].RowPitch = rowlength * nchannels * 4;
				tex.Reset(m_tmpDataBoxArray1);
			}
		}

		private void CreateCheckerR(MyGeneratedTexture tex, string name, Vector2I resolution, byte v1, byte v2)
		{
			int x = resolution.X;
			int y = resolution.Y;
			byte[] array = new byte[x * y];
			for (int i = 0; i < y; i++)
			{
				for (int j = 0; j < y; j++)
				{
					byte b = v1;
					if (((i + j) & 1) == 0)
					{
						b = v2;
					}
					array[i * x + j] = b;
				}
			}
			CreateR(tex, name, resolution, array);
		}

		private void CreateCheckerRGBA(MyGeneratedTexture tex, string name, Vector2I size, Vector2I resolution, Color v1, Color v2)
		{
			Vector2I vector2I = new Vector2I(resolution.X / size.X, resolution.Y / size.Y);
			int x = resolution.X;
			int y = resolution.Y;
			Color[] array = new Color[x * y];
			for (int i = 0; i < y / vector2I.Y; i++)
			{
				for (int j = 0; j < vector2I.Y; j++)
				{
					for (int k = 0; k < x; k++)
					{
						Color color = v1;
						if (((i * vector2I.X + k) & vector2I.X) == 0)
						{
							color = v2;
						}
						array[(i * vector2I.Y + j) * x + k] = color;
					}
				}
			}
			CreateRGBA(tex, name, resolution, srgb: false, array);
		}

		private void CreateAllTextures()
		{
			CreateRGBA_1x1((MyGeneratedTexture)ZeroTex, "EMPTY", new Color(0, 0, 0, 0));
			CreateRGBA_1x1((MyGeneratedTexture)PinkTex, "PINK", new Color(255, 0, 255));
			CreateRGBA_1x1((MyGeneratedTexture)RedTex, "RED", new Color(255, 0, 0));
			CreateRGBA_1x1((MyGeneratedTexture)GreenTex, "GREEN", new Color(0, 255, 0));
			CreateRGBA_1x1((MyGeneratedTexture)ReleaseMissingNormalGlossTex, "ReleaseMissingNormalGloss", new Color(127, 127, 255, 0));
			CreateR_1x1((MyGeneratedTexture)ReleaseMissingAlphamaskTex, "ReleaseMissingAlphamask", byte.MaxValue);
			CreateRGBA_1x1((MyGeneratedTexture)ReleaseMissingExtensionTex, "ReleaseMissingExtension", new Color(255, 0, 0, 0));
			CreateCubeRGBA_1x1((MyGeneratedTexture)ReleaseMissingCubeTex, "ReleaseMissingCube", new Color(255, 0, 255, 0));
			CreateCheckerRGBA((MyGeneratedTexture)DebugMissingNormalGlossTex, "DebugMissingNormalGloss", new Vector2I(8, 8), new Vector2I(8, 8), new Color(91, 0, 217, 0), new Color(217, 0, 217, 255));
			CreateCheckerR((MyGeneratedTexture)DebugMissingAlphamaskTex, "DebugMissingAlphamask", new Vector2I(8, 8), byte.MaxValue, 0);
			CreateCheckerRGBA((MyGeneratedTexture)DebugMissingExtensionTex, "DebugMissingExtension", new Vector2I(8, 8), new Vector2I(8, 8), new Color(255, 255, 0, 0), new Color(0, 0, 0, 0));
			CreateCubeRGBA_1x1((MyGeneratedTexture)DebugMissingCubeTex, "DebubMissingCube", new Color(255, 0, 255, 0));
			CreateCheckerRGBA((MyGeneratedTexture)DebugCheckerCMTex, "DebugCheckerCMTex", new Vector2I(8, 8), new Vector2I(256, 256), new Color(255, 255, 255, 0), new Color(0, 0, 0, 0));
			m_missingNormalGlossTex.Init(ReleaseMissingNormalGlossTex, DebugMissingNormalGlossTex, "MISSING_NORMAL_GLOSS");
			m_missingAlphamaskTex.Init(ReleaseMissingAlphamaskTex, DebugMissingAlphamaskTex, "MISSING_ALPHAMASK");
			m_missingExtensionTex.Init(ReleaseMissingExtensionTex, ReleaseMissingExtensionTex, "MISSING_EXTENSIONS");
			m_missingCubeTex.Init(ReleaseMissingCubeTex, DebugMissingCubeTex, "MISSING_CUBEMAP");
			m_missingColorMetal.Init(ZeroTex, PinkTex, "MISSING_COLOR_METAL");
			CreateCubeRGBA_1x1((MyGeneratedTexture)IntelFallbackCubeTex, "INTEL_FALLBACK_CUBEMAP", new Color(10, 10, 10, 0));
			InitializeRandomTexture();
			byte[] array = new byte[64]
			{
				0, 32, 8, 40, 2, 34, 10, 42, 48, 16,
				56, 24, 50, 18, 58, 26, 12, 44, 4, 36,
				14, 46, 6, 38, 60, 28, 52, 20, 62, 30,
				54, 22, 3, 35, 11, 43, 1, 33, 9, 41,
				51, 19, 59, 27, 49, 17, 57, 25, 15, 47,
				7, 39, 13, 45, 5, 37, 63, 31, 55, 23,
				61, 29, 53, 21
			};
			for (int i = 0; i < 64; i++)
			{
				array[i] = (byte)(array[i] * 4);
			}
			CreateR((MyGeneratedTexture)Dithering8x8Tex, "DITHER_8x8", new Vector2I(8, 8), array);
		}

		public void InitializeRandomTexture(int? seed = null)
		{
			MyRandom myRandom = ((!seed.HasValue) ? new MyRandom() : new MyRandom(seed.Value));
			int num = 64;
			byte[] array = new byte[num * num];
			myRandom.NextBytes(array);
			CreateR((MyGeneratedTexture)RandomTex, "RANDOM", new Vector2I(num, num), array);
		}

		public void DisposeTex(IGeneratedTexture tex)
		{
			if (tex != null)
			{
				MyUserGeneratedTexture myUserGeneratedTexture;
				MyGeneratedTextureFromPattern myGeneratedTextureFromPattern;
				if ((myUserGeneratedTexture = tex as MyUserGeneratedTexture) != null)
				{
					myUserGeneratedTexture.Dispose();
					m_objectsPoolGenerated.Deallocate(myUserGeneratedTexture);
					m_statistics.Remove(myUserGeneratedTexture);
				}
				else if ((myGeneratedTextureFromPattern = tex as MyGeneratedTextureFromPattern) != null)
				{
					myGeneratedTextureFromPattern.Dispose();
					m_objectsPoolGeneratedFromPattern.Deallocate(myGeneratedTextureFromPattern);
					m_statistics.Remove(myGeneratedTextureFromPattern);
				}
			}
		}

		public void OnDeviceInit()
		{
			CreateAllTextures();
		}

		public void OnDeviceReset()
		{
			OnDeviceEnd();
			OnDeviceInit();
		}

		public void OnDeviceEnd()
		{
<<<<<<< HEAD
			foreach (MyUserGeneratedTexture item in m_objectsPoolGenerated.Active)
			{
				item.Dispose();
=======
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<MyUserGeneratedTexture> enumerator = m_objectsPoolGenerated.Active.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.get_Current().Dispose();
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void OnUnloadData()
		{
		}
	}
}
