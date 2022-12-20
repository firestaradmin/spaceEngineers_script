using VRage.Render11.Common;
using VRage.Render11.GeometryStage2.Materials;
using VRage.Render11.RenderContext;
using VRage.Render11.Resources;

namespace VRage.Render11.GeometryStage2.Rendering
{
	internal class MyLodGBufferSrvStrategy : IGBufferSrvStrategy
	{
		private ITexture[] m_fileTextures;

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
			}
		}

		public ISrvBindable[] GetSrvs(MyRenderContext rc, MyRenderMaterialBindings part, int lodNum)
		{
			using (m_lock.AcquireExclusiveUsing())
			{
				PrepareFileTextures();
				ISrvBindable[] srvs = part.Srvs;
				for (int i = 0; i < srvs.Length; i++)
				{
					m_tmpSrvs[i] = srvs[i];
				}
				for (int j = srvs.Length; j < m_tmpSrvs.Length; j++)
				{
					m_tmpSrvs[j] = null;
				}
				m_tmpSrvs[0] = m_fileTextures[lodNum % m_fileTextures.Length];
				m_tmpSrvs[1] = MyGeneratedTextureManager.ReleaseMissingNormalGlossTex;
				m_tmpSrvs[2] = MyGeneratedTextureManager.ReleaseMissingExtensionTex;
				m_tmpSrvs[3] = MyGeneratedTextureManager.ReleaseMissingAlphamaskTex;
				return m_tmpSrvs;
			}
		}

		public ISrvBindable[] GetSrvsForTheOldPipeline(MyRenderContext rc, ISrvBindable[] srvs)
		{
			return srvs;
		}
	}
}
