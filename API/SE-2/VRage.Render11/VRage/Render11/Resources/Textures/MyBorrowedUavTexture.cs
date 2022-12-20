using SharpDX.Direct3D11;
using VRage.Network;
using VRage.Render11.Common;
using VRageMath;

namespace VRage.Render11.Resources.Textures
{
	internal class MyBorrowedUavTexture : MyBorrowedTexture, IBorrowedUavTexture, IBorrowedSrvTexture, ISrvTexture, ISrvBindable, IResource, ITexture, IUavTexture, IRtvTexture, IRtvBindable, IUavBindable
	{
		private class VRage_Render11_Resources_Textures_MyBorrowedUavTexture_003C_003EActor : IActivator, IActivator<MyBorrowedUavTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyBorrowedUavTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBorrowedUavTexture CreateInstance()
			{
				return new MyBorrowedUavTexture();
			}

			MyBorrowedUavTexture IActivator<MyBorrowedUavTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override Vector3I Size3 => UavTexture.Size3;

		public override Vector2I Size => UavTexture.Size;

		public override Resource Resource => UavTexture.Resource;

		public override ShaderResourceView Srv => UavTexture.Srv;

		public UnorderedAccessView Uav => UavTexture.Uav;

		public RenderTargetView Rtv => UavTexture.Rtv;

		public IUavTexture UavTexture { get; private set; }

		protected override void CreateTextureInternal(ref MyBorrowedTextureKey key, string debugName)
		{
			UavTexture = MyManagers.RwTextures.CreateUav(debugName, key.Width, key.Height, key.Format, key.SamplesCount, key.SamplesQuality);
		}
	}
}
