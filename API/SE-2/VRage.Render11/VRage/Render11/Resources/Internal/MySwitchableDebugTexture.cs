using System;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRageMath;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	internal class MySwitchableDebugTexture : IGeneratedTexture, ITexture, ISrvBindable, IResource
	{
		private string m_name;

		private IGeneratedTexture m_releaseTexture;

		private IGeneratedTexture m_debugTexture;

		public ShaderResourceView Srv => GetTexture().Srv;

		public SharpDX.Direct3D11.Resource Resource => GetTexture().Resource;

		public string Name => GetTexture().Name;

		public Vector2I Size => GetTexture().Size;

		public Vector3I Size3 => GetTexture().Size3;

		public int MipLevels => GetTexture().MipLevels;

		public Format Format => GetTexture().Format;

		public event Action<ITexture> OnFormatChanged;

		private IGeneratedTexture GetTexture()
		{
			if (MyRender11.Settings.UseDebugMissingFileTextures)
			{
				return m_debugTexture;
			}
			return m_releaseTexture;
		}

		public void Init(IGeneratedTexture releaseTex, IGeneratedTexture debugTex, string name)
		{
			m_name = name;
			m_releaseTexture = releaseTex;
			m_debugTexture = debugTex;
		}
	}
}
