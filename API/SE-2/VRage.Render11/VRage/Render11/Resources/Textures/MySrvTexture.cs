using VRage.Network;

namespace VRage.Render11.Resources.Textures
{
	internal class MySrvTexture : MyTextureInternal, ISrvTexture, ISrvBindable, IResource, ITexture
	{
		private class VRage_Render11_Resources_Textures_MySrvTexture_003C_003EActor : IActivator, IActivator<MySrvTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MySrvTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MySrvTexture CreateInstance()
			{
				return new MySrvTexture();
			}

			MySrvTexture IActivator<MySrvTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public void OnDeviceInit()
		{
			OnDeviceInitInternal();
		}

		public void OnDeviceEnd()
		{
			OnDeviceEndInternal();
		}
	}
}
