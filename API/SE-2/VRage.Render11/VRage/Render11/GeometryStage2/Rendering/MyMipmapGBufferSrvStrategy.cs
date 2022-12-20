using System;
using System.Collections.Generic;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRage.Render11.Common;
using VRage.Render11.GeometryStage2.Materials;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;
using VRageMath;

namespace VRage.Render11.GeometryStage2.Rendering
{
	internal class MyMipmapGBufferSrvStrategy : IGBufferSrvStrategy
	{
		private readonly Dictionary<Vector2I, ISrvBindable> m_textures = new Dictionary<Vector2I, ISrvBindable>();

		private ITexture[] m_fileTextures;

		private ITexture m_blackTexture;

		private readonly FastResourceLock m_lock = new FastResourceLock();

		private readonly ISrvBindable[] m_tmpSrvs = new ISrvBindable[4];

		private void PrepareFileTextures()
		{
			if (m_fileTextures == null)
			{
				m_fileTextures = new ITexture[6];
				m_fileTextures[0] = MyManagers.Textures.GetPermanentTexture("Textures\\Debug\\Red.dds", MyFileTextureEnum.COLOR_METAL).Texture;
				m_fileTextures[1] = MyManagers.Textures.GetPermanentTexture("Textures\\Debug\\Green.dds", MyFileTextureEnum.COLOR_METAL).Texture;
				m_fileTextures[2] = MyManagers.Textures.GetPermanentTexture("Textures\\Debug\\Blue.dds", MyFileTextureEnum.COLOR_METAL).Texture;
				m_fileTextures[3] = MyManagers.Textures.GetPermanentTexture("Textures\\Debug\\Yellow.dds", MyFileTextureEnum.COLOR_METAL).Texture;
				m_fileTextures[4] = MyManagers.Textures.GetPermanentTexture("Textures\\Debug\\Cyan.dds", MyFileTextureEnum.COLOR_METAL).Texture;
				m_fileTextures[5] = MyManagers.Textures.GetPermanentTexture("Textures\\Debug\\Magenta.dds", MyFileTextureEnum.COLOR_METAL).Texture;
				m_blackTexture = MyManagers.Textures.GetPermanentTexture("Textures\\Debug\\Black.dds", MyFileTextureEnum.COLOR_METAL).Texture;
			}
		}

		private ISrvBindable CreateNewTexture(MyRenderContext rc, Vector2I resolution)
		{
			string debugName = $"Debug-mipmap-{resolution.X}x{resolution.Y}";
			int mipLevels = MyResourceUtils.GetMipLevels(Math.Max(resolution.X, resolution.Y));
			resolution.X = MyResourceUtils.GetMipStride(resolution.X, 0);
			resolution.Y = MyResourceUtils.GetMipStride(resolution.Y, 0);
			ISrvTexture srvTexture = MyManagers.RwTextures.CreateSrv(debugName, resolution.X, resolution.Y, Format.BC7_UNorm_SRgb, 1, 0, ResourceOptionFlags.None, ResourceUsage.Default, mipLevels);
			for (int i = 0; i < mipLevels; i++)
			{
				ISrvBindable srvBindable = m_fileTextures[i % m_fileTextures.Length];
				Vector2I vector2I = new Vector2I(MyResourceUtils.GetMipSize(resolution.X, i), MyResourceUtils.GetMipSize(resolution.Y, i));
				for (int j = 0; j < vector2I.X; j += srvBindable.Size.X)
				{
					for (int k = 0; k < vector2I.Y; k += srvBindable.Size.Y)
					{
						rc.CopySubresourceRegion(srvBindable, 0, null, srvTexture, i, j, k);
					}
				}
			}
			return srvTexture;
		}

		public ISrvBindable[] GetSrvs(MyRenderContext rc, MyRenderMaterialBindings part, int lodNum)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				PrepareFileTextures();
				Vector2I vector2I = new Vector2I(0, 0);
				if (part.Srvs != null && part.Srvs.Length >= 1)
				{
					vector2I = part.Srvs[0].Size;
				}
				ISrvBindable value;
				if (vector2I == new Vector2I(0, 0))
				{
					value = m_blackTexture;
				}
				else if (!m_textures.TryGetValue(vector2I, out value))
				{
					value = CreateNewTexture(rc, vector2I);
					m_textures.Add(vector2I, value);
				}
				m_tmpSrvs[0] = value;
				m_tmpSrvs[1] = MyGeneratedTextureManager.ReleaseMissingNormalGlossTex;
				m_tmpSrvs[2] = MyGeneratedTextureManager.ReleaseMissingExtensionTex;
				m_tmpSrvs[3] = MyGeneratedTextureManager.ReleaseMissingAlphamaskTex;
				return m_tmpSrvs;
			}
		}

		public ISrvBindable[] GetSrvsForTheOldPipeline(MyRenderContext rc, ISrvBindable[] srvs)
		{
			if (srvs == null || srvs.Length == 0)
			{
				return null;
			}
			if (srvs[0].Size3.Z != 1)
			{
				return srvs;
			}
			using (m_lock.AcquireExclusiveUsing())
			{
				PrepareFileTextures();
				Vector2I vector2I = new Vector2I(0, 0);
				vector2I = srvs[0].Size;
				ISrvBindable value;
				if (vector2I == new Vector2I(0, 0))
				{
					value = m_blackTexture;
				}
				else if (!m_textures.TryGetValue(vector2I, out value))
				{
					value = CreateNewTexture(rc, vector2I);
					m_textures.Add(vector2I, value);
				}
				m_tmpSrvs[0] = value;
				m_tmpSrvs[1] = MyGeneratedTextureManager.ReleaseMissingNormalGlossTex;
				m_tmpSrvs[2] = MyGeneratedTextureManager.ReleaseMissingExtensionTex;
				m_tmpSrvs[3] = MyGeneratedTextureManager.ReleaseMissingAlphamaskTex;
				return m_tmpSrvs;
			}
		}
	}
}
