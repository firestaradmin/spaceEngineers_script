using System;
using System.Collections.Concurrent;
using System.IO;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.FileSystem;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Resources
{
	internal static class MyResourceUtils
	{
		private static ConcurrentDictionary<string, (string, Uri)> m_normalizeFileTextureResults = new ConcurrentDictionary<string, (string, Uri)>();

		public static bool IsCompressed(Format fmt)
		{
			if (fmt < Format.BC1_Typeless || fmt > Format.BC5_SNorm)
			{
				if (fmt >= Format.BC6H_Typeless)
				{
					return fmt <= Format.BC7_UNorm_SRgb;
				}
				return false;
			}
			return true;
		}

		public static bool IsRawRGBA(Format fmt)
		{
			if ((fmt < Format.R8G8B8A8_Typeless || fmt > Format.R8G8B8A8_SInt) && fmt != Format.B8G8R8A8_UNorm && fmt != Format.B8G8R8A8_Typeless)
			{
				return fmt == Format.B8G8R8A8_UNorm_SRgb;
			}
			return true;
		}

		public static Format MakeSrgb(Format fmt)
		{
<<<<<<< HEAD
			switch (fmt)
			{
			case Format.R8G8B8A8_UNorm:
				return Format.R8G8B8A8_UNorm_SRgb;
			case Format.B8G8R8A8_UNorm:
				return Format.B8G8R8A8_UNorm_SRgb;
			case Format.B8G8R8X8_UNorm:
				return Format.B8G8R8X8_UNorm_SRgb;
			case Format.BC1_UNorm:
				return Format.BC1_UNorm_SRgb;
			case Format.BC2_UNorm:
				return Format.BC2_UNorm_SRgb;
			case Format.BC3_UNorm:
				return Format.BC3_UNorm_SRgb;
			case Format.BC7_UNorm:
				return Format.BC7_UNorm_SRgb;
			default:
				return fmt;
			}
=======
			return fmt switch
			{
				Format.R8G8B8A8_UNorm => Format.R8G8B8A8_UNorm_SRgb, 
				Format.B8G8R8A8_UNorm => Format.B8G8R8A8_UNorm_SRgb, 
				Format.B8G8R8X8_UNorm => Format.B8G8R8X8_UNorm_SRgb, 
				Format.BC1_UNorm => Format.BC1_UNorm_SRgb, 
				Format.BC2_UNorm => Format.BC2_UNorm_SRgb, 
				Format.BC3_UNorm => Format.BC3_UNorm_SRgb, 
				Format.BC7_UNorm => Format.BC7_UNorm_SRgb, 
				_ => fmt, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static Format MakeLinear(Format fmt)
		{
<<<<<<< HEAD
			switch (fmt)
			{
			case Format.R8G8B8A8_UNorm_SRgb:
				return Format.R8G8B8A8_UNorm;
			case Format.B8G8R8A8_UNorm_SRgb:
				return Format.B8G8R8A8_UNorm;
			case Format.B8G8R8X8_UNorm_SRgb:
				return Format.B8G8R8X8_UNorm;
			case Format.BC1_UNorm_SRgb:
				return Format.BC1_UNorm;
			case Format.BC2_UNorm_SRgb:
				return Format.BC2_UNorm;
			case Format.BC3_UNorm_SRgb:
				return Format.BC3_UNorm;
			case Format.BC7_UNorm_SRgb:
				return Format.BC7_UNorm;
			default:
				return fmt;
			}
=======
			return fmt switch
			{
				Format.R8G8B8A8_UNorm_SRgb => Format.R8G8B8A8_UNorm, 
				Format.B8G8R8A8_UNorm_SRgb => Format.B8G8R8A8_UNorm, 
				Format.B8G8R8X8_UNorm_SRgb => Format.B8G8R8X8_UNorm, 
				Format.BC1_UNorm_SRgb => Format.BC1_UNorm, 
				Format.BC2_UNorm_SRgb => Format.BC2_UNorm, 
				Format.BC3_UNorm_SRgb => Format.BC3_UNorm, 
				Format.BC7_UNorm_SRgb => Format.BC7_UNorm, 
				_ => fmt, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static bool IsSrgb(Format fmt)
		{
<<<<<<< HEAD
			switch (fmt)
			{
			case Format.R8G8B8A8_UNorm_SRgb:
				return true;
			case Format.B8G8R8A8_UNorm_SRgb:
				return true;
			case Format.B8G8R8X8_UNorm_SRgb:
				return true;
			case Format.BC1_UNorm_SRgb:
				return true;
			case Format.BC2_UNorm_SRgb:
				return true;
			case Format.BC3_UNorm_SRgb:
				return true;
			case Format.BC7_UNorm_SRgb:
				return true;
			default:
				return false;
			}
=======
			return fmt switch
			{
				Format.R8G8B8A8_UNorm_SRgb => true, 
				Format.B8G8R8A8_UNorm_SRgb => true, 
				Format.B8G8R8X8_UNorm_SRgb => true, 
				Format.BC1_UNorm_SRgb => true, 
				Format.BC2_UNorm_SRgb => true, 
				Format.BC3_UNorm_SRgb => true, 
				Format.BC7_UNorm_SRgb => true, 
				_ => false, 
			};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static int GetTexelBitSize(Format fmt)
		{
			switch (fmt)
			{
			case Format.Unknown:
				throw new ArgumentException($"The format {fmt} cannot be used.", "fmt");
			case Format.R32G32B32A32_Typeless:
			case Format.R32G32B32A32_Float:
			case Format.R32G32B32A32_UInt:
			case Format.R32G32B32A32_SInt:
				return 128;
			case Format.R32G32B32_Typeless:
			case Format.R32G32B32_Float:
			case Format.R32G32B32_UInt:
			case Format.R32G32B32_SInt:
				return 96;
			case Format.R16G16B16A16_Typeless:
			case Format.R16G16B16A16_Float:
			case Format.R16G16B16A16_UNorm:
			case Format.R16G16B16A16_UInt:
			case Format.R16G16B16A16_SNorm:
			case Format.R16G16B16A16_SInt:
			case Format.R32G32_Typeless:
			case Format.R32G32_Float:
			case Format.R32G32_UInt:
			case Format.R32G32_SInt:
			case Format.R32G8X24_Typeless:
				return 64;
			case Format.D32_Float_S8X24_UInt:
			case Format.R32_Float_X8X24_Typeless:
			case Format.X32_Typeless_G8X24_UInt:
			case Format.R10G10B10A2_Typeless:
			case Format.R10G10B10A2_UNorm:
			case Format.R10G10B10A2_UInt:
			case Format.R11G11B10_Float:
			case Format.R8G8B8A8_Typeless:
			case Format.R8G8B8A8_UNorm:
			case Format.R8G8B8A8_UNorm_SRgb:
			case Format.R8G8B8A8_UInt:
			case Format.R8G8B8A8_SNorm:
			case Format.R8G8B8A8_SInt:
			case Format.R16G16_Typeless:
			case Format.R16G16_Float:
			case Format.R16G16_UNorm:
			case Format.R16G16_UInt:
			case Format.R16G16_SNorm:
			case Format.R16G16_SInt:
			case Format.R32_Typeless:
			case Format.D32_Float:
			case Format.R32_Float:
			case Format.R32_UInt:
			case Format.R32_SInt:
			case Format.R24G8_Typeless:
			case Format.D24_UNorm_S8_UInt:
			case Format.R24_UNorm_X8_Typeless:
			case Format.X24_Typeless_G8_UInt:
				return 32;
			case Format.R8G8_Typeless:
			case Format.R8G8_UNorm:
			case Format.R8G8_UInt:
			case Format.R8G8_SNorm:
			case Format.R8G8_SInt:
			case Format.R16_Typeless:
			case Format.R16_Float:
			case Format.D16_UNorm:
			case Format.R16_UNorm:
			case Format.R16_UInt:
			case Format.R16_SNorm:
			case Format.R16_SInt:
				return 16;
			case Format.R8_Typeless:
			case Format.R8_UNorm:
			case Format.R8_UInt:
			case Format.R8_SNorm:
			case Format.R8_SInt:
			case Format.A8_UNorm:
				return 8;
			case Format.R1_UNorm:
				throw new ArgumentException($"The one-bit format {fmt} cannot be used.", "fmt");
			case Format.R9G9B9E5_Sharedexp:
			case Format.R8G8_B8G8_UNorm:
			case Format.G8R8_G8B8_UNorm:
				return 32;
			case Format.BC1_Typeless:
			case Format.BC1_UNorm:
			case Format.BC1_UNorm_SRgb:
				return 4;
			case Format.BC2_Typeless:
			case Format.BC2_UNorm:
			case Format.BC2_UNorm_SRgb:
			case Format.BC3_Typeless:
			case Format.BC3_UNorm:
			case Format.BC3_UNorm_SRgb:
				return 8;
			case Format.BC4_Typeless:
			case Format.BC4_UNorm:
			case Format.BC4_SNorm:
				return 4;
			case Format.BC5_Typeless:
			case Format.BC5_UNorm:
			case Format.BC5_SNorm:
				return 8;
			case Format.B5G6R5_UNorm:
			case Format.B5G5R5A1_UNorm:
				return 16;
			case Format.B8G8R8A8_UNorm:
			case Format.B8G8R8X8_UNorm:
			case Format.R10G10B10_Xr_Bias_A2_UNorm:
			case Format.B8G8R8A8_Typeless:
			case Format.B8G8R8A8_UNorm_SRgb:
			case Format.B8G8R8X8_Typeless:
			case Format.B8G8R8X8_UNorm_SRgb:
				return 32;
			case Format.BC6H_Typeless:
			case Format.BC6H_Uf16:
			case Format.BC6H_Sf16:
			case Format.BC7_Typeless:
			case Format.BC7_UNorm:
			case Format.BC7_UNorm_SRgb:
				return 8;
			case Format.B4G4R4A4_UNorm:
				return 16;
			case Format.AYUV:
			case Format.Y410:
			case Format.Y416:
			case Format.NV12:
			case Format.P010:
			case Format.P016:
			case Format.Opaque420:
			case Format.YUY2:
			case Format.Y210:
			case Format.Y216:
			case Format.NV11:
			case Format.AI44:
			case Format.IA44:
			case Format.P8:
			case Format.A8P8:
				throw new ArgumentException($"The format {fmt} is a YUV video format. We don't support those.", "fmt");
			case Format.P208:
			case Format.V208:
			case Format.V408:
				throw new ArgumentException($"The format {fmt} is unsupported.", "fmt");
			default:
				throw new ArgumentOutOfRangeException("fmt", fmt, "Invalid format.");
			}
		}

		public static long GetTextureByteSize(this ITexture texture)
		{
			return GetTextureByteSize(texture.Size3, texture.MipLevels, texture.Format);
		}

		public static long GetTextureByteSize(Vector3I size, int mipLevels, Format format)
		{
			long num = 0L;
			for (int i = 0; i < mipLevels; i++)
			{
				num += (long)((double)(size.X * size.Y * GetTexelBitSize(format)) / 8.0);
				size.X = size.X / 2 + size.X % 2;
				size.Y = size.Y / 2 + size.X % 2;
			}
			return num * size.Z;
		}

		public static int GetMipLevels(int size)
		{
			int num = 0;
			int num2 = 0;
			while (true)
			{
				bool flag = (size & 1) == 1;
				if (size <= 1)
				{
					break;
				}
				if (flag)
				{
					num = 1;
				}
				num2++;
				size /= 2;
			}
			return 1 + num2 + num;
		}

		public static int GetMipSize(int mip0Size, int mip)
		{
			int num = mip0Size;
			for (int i = 0; i < mip; i++)
			{
				num = num / 2 + num % 2;
			}
			return num;
		}

		public static int GetMipStride(int mip0Size, int mip)
		{
			int mipSize = GetMipSize(mip0Size, mip);
			return (mipSize / 4 + ((mipSize % 4 != 0) ? 1 : 0)) * 4;
		}

		public static int GetMipsToSkip(MyFileTextureEnum type, bool isVoxel, Vector2I resolution, int mipLevels)
		{
			if (MyFileTextureManager.MyFileTextureHelper.IsQualityDependentFilter(type) && mipLevels > 1)
			{
				if (isVoxel)
				{
					return MyRender11.Settings.User.VoxelTextureQuality.MipLevelsToSkip(resolution.X, resolution.Y, type);
				}
				return MyRender11.Settings.User.TextureQuality.MipLevelsToSkip(resolution.X, resolution.Y, type);
			}
			return 0;
		}

		public static (Vector2I Resolution, int MipLevels) GetTextureSizeAfterMipmapSkip(MyFileTextureEnum type, bool isVoxel, Vector2I resolution, int mipLevels)
		{
			if (mipLevels <= 1)
			{
				return (resolution, mipLevels);
			}
			int mipsToSkip = GetMipsToSkip(type, isVoxel, resolution, mipLevels);
			return (new Vector2I(GetMipSize(resolution.X, mipsToSkip), GetMipSize(resolution.Y, mipsToSkip)), mipLevels - mipsToSkip);
		}

		public static bool CheckTexturesConsistency(Texture2DDescription desc1, Texture2DDescription desc2)
		{
			if (desc1.Format == desc2.Format && desc1.MipLevels == desc2.MipLevels && desc1.Width == desc2.Width)
			{
				return desc1.Height == desc2.Height;
			}
			return false;
		}

		/// <summary>Normalizes file names into lower case relative path, if possible</summary>
		/// <returns>True if it's a file texture, false if the texture is ram generated</returns>
		public static bool NormalizeFileTextureName(ref string name)
		{
			Uri uri;
			return NormalizeFileTextureName(ref name, out uri);
		}

		/// <summary>Normalizes file names into lower case relative path, if possible</summary>
		/// <returns>True if it's a file texture, false if the texture is ram generated</returns>
		public static bool NormalizeFileTextureName(ref string name, out Uri uri)
		{
<<<<<<< HEAD
			if (name != null && m_normalizeFileTextureResults.TryGetValue(name, out var value))
			{
				uri = value.Item2;
				(name, _) = value;
=======
			(string, Uri) tuple = default((string, Uri));
			if (name != null && m_normalizeFileTextureResults.TryGetValue(name, ref tuple))
			{
				uri = tuple.Item2;
				(name, _) = tuple;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				return true;
			}
			if (MyRenderProxy.IsValidGeneratedTextureName(name))
			{
				uri = null;
				return false;
			}
			string text = name;
			if (Uri.TryCreate(text, UriKind.Absolute, out uri))
			{
				name = MakeRelativePath(uri);
			}
			name = name.ToLowerInvariant().Replace('/', '\\').Replace("\\\\", "\\");
			try
			{
				uri = new Uri(Path.Combine(MyFileSystem.ContentPath, text));
<<<<<<< HEAD
				value = (name, uri);
				m_normalizeFileTextureResults.TryAdd(text, value);
=======
				tuple = (name, uri);
				m_normalizeFileTextureResults.TryAdd(text, tuple);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch
			{
				uri = null;
				return false;
			}
			return true;
		}

		public static string NormalizePath(string path)
		{
			return Path.GetFullPath(new Uri(path).LocalPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToLowerInvariant();
		}

		/// <returns>Returns resolved texture path (rooted and without . or ..)</returns>
		public static string GetTextureFullPath(string textureName, string contentPath = null)
		{
			if (string.IsNullOrEmpty(textureName))
			{
				return string.Empty;
			}
			if (MyRenderProxy.IsValidGeneratedTextureName(textureName))
			{
				return textureName;
			}
			string text = null;
			if (Path.IsPathRooted(textureName))
			{
				text = textureName;
			}
			else
			{
				if (!string.IsNullOrEmpty(contentPath))
				{
					string text2 = NormalizePath(contentPath);
					string text3 = NormalizePath(MyFileSystem.ContentPath);
					if (text2 != text3)
					{
						string text4 = Path.Combine(contentPath, textureName);
						if (MyFileSystem.FileExists(text4))
						{
							text = text4;
						}
					}
				}
				if (text == null)
				{
					text = Path.Combine(MyFileSystem.ContentPath, textureName);
				}
			}
			text = text.ToLower().Replace('/', '\\');
			text = text.ToLower().Replace("\\\\", "\\");
			return Path.GetFullPath(text);
		}

		private static string MakeRelativePath(Uri toUri)
		{
			Uri uri = new Uri(TerminatePath(MyFileSystem.ContentPath));
			if (uri.Scheme != toUri.Scheme)
			{
				return toUri.AbsoluteUri;
			}
			string text = Uri.UnescapeDataString(uri.MakeRelativeUri(toUri).ToString());
			if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
			{
				text = text.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			}
			return text;
		}

		private static string TerminatePath(string path)
		{
			if (!string.IsNullOrEmpty(path) && path[path.Length - 1] == Path.DirectorySeparatorChar)
			{
				return path;
			}
<<<<<<< HEAD
			char directorySeparatorChar = Path.DirectorySeparatorChar;
			return path + directorySeparatorChar;
=======
			return path + Path.DirectorySeparatorChar;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
