using System;
using System.Collections.Generic;
using SharpDX.DXGI;
using VRage.Render11.Common;
using VRage.Render11.Resources.Textures;

namespace VRage.Render11.Resources
{
	internal class MyRwTextureCatalog : IManager, IManagerDevice, IManagerFrameEnd
	{
		private class MyTextureKeyIdentity
		{
			public MyCatalogTextureType Type { get; set; }

			public MyBorrowedTextureKey Key { get; set; }
		}

		private Dictionary<string, MyTextureKeyIdentity> m_textureKeysCatalog = new Dictionary<string, MyTextureKeyIdentity>();

		private Dictionary<string, IBorrowedSrvTexture> m_texturesMap = new Dictionary<string, IBorrowedSrvTexture>();

		public void RegisterTexture(string textureName, int width, int height, Format format, MyCatalogTextureType type)
		{
			m_textureKeysCatalog.Add(textureName, new MyTextureKeyIdentity
			{
				Type = type,
				Key = new MyBorrowedTextureKey
				{
					Width = width,
					Height = height,
					Format = format
				}
			});
		}

		public ISrvTexture GetTexture(string textureName)
		{
			if (!m_texturesMap.TryGetValue(textureName, out var value))
			{
				MyTextureKeyIdentity myTextureKeyIdentity = m_textureKeysCatalog[textureName];
<<<<<<< HEAD
				switch (myTextureKeyIdentity.Type)
				{
				case MyCatalogTextureType.Rtv:
					value = MyManagers.RwTexturesPool.BorrowRtv(textureName, myTextureKeyIdentity.Key.Width, myTextureKeyIdentity.Key.Height, myTextureKeyIdentity.Key.Format);
					break;
				case MyCatalogTextureType.Uav:
					value = MyManagers.RwTexturesPool.BorrowUav(textureName, myTextureKeyIdentity.Key.Width, myTextureKeyIdentity.Key.Height, myTextureKeyIdentity.Key.Format);
					break;
				default:
					throw new Exception();
				}
=======
				value = myTextureKeyIdentity.Type switch
				{
					MyCatalogTextureType.Rtv => MyManagers.RwTexturesPool.BorrowRtv(textureName, myTextureKeyIdentity.Key.Width, myTextureKeyIdentity.Key.Height, myTextureKeyIdentity.Key.Format), 
					MyCatalogTextureType.Uav => MyManagers.RwTexturesPool.BorrowUav(textureName, myTextureKeyIdentity.Key.Width, myTextureKeyIdentity.Key.Height, myTextureKeyIdentity.Key.Format), 
					_ => throw new Exception(), 
				};
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				m_texturesMap.Add(textureName, value);
			}
			return value;
		}

		public IRtvTexture GetRtvTexture(string textureName)
		{
			return (IRtvTexture)GetTexture(textureName);
		}

		public IUavTexture GetUavTexture(string textureName)
		{
			return (IUavTexture)GetTexture(textureName);
		}

		void IManagerFrameEnd.OnFrameEnd()
		{
			foreach (IBorrowedSrvTexture value in m_texturesMap.Values)
			{
				value.Release();
			}
			m_texturesMap.Clear();
		}

		public void OnDeviceInit()
		{
		}

		public void OnDeviceReset()
		{
		}

		public void OnDeviceEnd()
		{
			m_textureKeysCatalog.Clear();
		}
	}
}
