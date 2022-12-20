using System;

namespace VRage.Render11.Resources.Textures
{
	internal struct MyStreamedTexturePin : IDisposable
	{
		private MyTextureStreamingManager.MyStreamedTexture m_texture;

		public bool IsEmpty => m_texture == null;

		public ITexture Texture => m_texture.Texture;

		public MyStreamedTexturePin(MyTextureStreamingManager.MyStreamedTexture texture)
		{
			texture.Pin();
			m_texture = texture;
		}

		public static bool operator ==(MyStreamedTexturePin a, MyStreamedTexturePin b)
		{
			return a.m_texture == b.m_texture;
		}

		public static bool operator !=(MyStreamedTexturePin a, MyStreamedTexturePin b)
		{
			return !(a == b);
		}

<<<<<<< HEAD
		public override bool Equals(object obj)
		{
			object obj2;
			if ((obj2 = obj) is MyStreamedTexturePin)
			{
				MyStreamedTexturePin myStreamedTexturePin = (MyStreamedTexturePin)obj2;
				return m_texture == myStreamedTexturePin.m_texture;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return m_texture.GetHashCode();
		}

=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public override string ToString()
		{
			return m_texture.ToString();
		}

		public void Dispose()
		{
			m_texture?.ReleasePin();
			m_texture = null;
		}
	}
}
