using SharpDX.Direct3D11;
using VRage.Network;
using VRage.Render11.Common;
using VRageMath;

namespace VRage.Render11.Resources.Textures
{
	internal class MyBorrowedDepthStencilTexture : MyBorrowedTexture, IBorrowedDepthStencilTexture, IBorrowedSrvTexture, ISrvTexture, ISrvBindable, IResource, ITexture, IDepthStencil
	{
		private class VRage_Render11_Resources_Textures_MyBorrowedDepthStencilTexture_003C_003EActor : IActivator, IActivator<MyBorrowedDepthStencilTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyBorrowedDepthStencilTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBorrowedDepthStencilTexture CreateInstance()
			{
				return new MyBorrowedDepthStencilTexture();
			}

			MyBorrowedDepthStencilTexture IActivator<MyBorrowedDepthStencilTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public IDepthStencil DepthStencilTexture { get; private set; }

		public override Vector3I Size3 => DepthStencilTexture.Size3;

		public override Vector2I Size => DepthStencilTexture.Size;

		public override Resource Resource => DepthStencilTexture.Resource;

		public override ShaderResourceView Srv => SrvDepth.Srv;

		public ISrvBindable SrvDepth => DepthStencilTexture.SrvDepth;

		public ISrvBindable SrvStencil => DepthStencilTexture.SrvStencil;

		public IDsvBindable Dsv => DepthStencilTexture.Dsv;

		public IDsvBindable DsvRo => DepthStencilTexture.DsvRo;

		public IDsvBindable DsvRoStencil => DepthStencilTexture.DsvRoStencil;

		public IDsvBindable DsvRoDepth => DepthStencilTexture.DsvRoDepth;

		protected override void CreateTextureInternal(ref MyBorrowedTextureKey key, string debugName)
		{
			DepthStencilTexture = MyManagers.DepthStencils.CreateDepthStencil(debugName, key.Width, key.Height, key.HqDepth, key.SamplesCount, key.SamplesQuality);
		}
	}
}
