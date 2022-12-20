using SharpDX.Direct3D11;
using VRage.Network;
using VRage.Render11.Common;
using VRageMath;

namespace VRage.Render11.Resources.Textures
{
	internal class MyBorrowedRtvTexture : MyBorrowedTexture, IBorrowedRtvTexture, IBorrowedSrvTexture, ISrvTexture, ISrvBindable, IResource, ITexture, IRtvTexture, IRtvBindable
	{
		private class VRage_Render11_Resources_Textures_MyBorrowedRtvTexture_003C_003EActor : IActivator, IActivator<MyBorrowedRtvTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyBorrowedRtvTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBorrowedRtvTexture CreateInstance()
			{
				return new MyBorrowedRtvTexture();
			}

			MyBorrowedRtvTexture IActivator<MyBorrowedRtvTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override Vector3I Size3 => RtvTexture.Size3;

		public override Vector2I Size => RtvTexture.Size;

		public override Resource Resource => RtvTexture.Resource;

		public override ShaderResourceView Srv => RtvTexture.Srv;

		public RenderTargetView Rtv => RtvTexture.Rtv;

		public IRtvTexture RtvTexture { get; private set; }

		protected override void CreateTextureInternal(ref MyBorrowedTextureKey key, string debugName)
		{
			RtvTexture = MyManagers.RwTextures.CreateRtv(debugName, key.Width, key.Height, key.Format, key.SamplesCount, key.SamplesQuality);
		}
	}
}
