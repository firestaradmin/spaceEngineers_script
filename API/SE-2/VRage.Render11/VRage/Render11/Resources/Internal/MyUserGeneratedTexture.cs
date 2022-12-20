using VRage.Network;

namespace VRage.Render11.Resources.Internal
{
	[GenerateActivator]
	internal class MyUserGeneratedTexture : MyGeneratedTexture, IUserGeneratedTexture, IGeneratedTexture, ITexture, ISrvBindable, IResource, IRtvBindable, IAsyncTexture
	{
		private class VRage_Render11_Resources_Internal_MyUserGeneratedTexture_003C_003EActor : IActivator, IActivator<MyUserGeneratedTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyUserGeneratedTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyUserGeneratedTexture CreateInstance()
			{
				return new MyUserGeneratedTexture();
			}

			MyUserGeneratedTexture IActivator<MyUserGeneratedTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public bool IsLoaded { get; set; }

		public void Reset(byte[] data)
		{
			MyGeneratedTextureManager.ResetUserTexture(this, data);
		}

		public void SetTextureReady()
		{
			IsLoaded = true;
		}
	}
}
