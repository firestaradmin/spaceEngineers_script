using SharpDX.Direct3D11;
using VRage.Network;
using VRage.Render11.Common;
using VRageMath;

namespace VRage.Render11.Resources.Textures
{
	internal class MyBorrowedCustomTexture : MyBorrowedTexture, IBorrowedCustomTexture, IBorrowedSrvTexture, ISrvTexture, ISrvBindable, IResource, ITexture, ICustomTexture, IUavBindable
	{
		private class VRage_Render11_Resources_Textures_MyBorrowedCustomTexture_003C_003EActor : IActivator, IActivator<MyBorrowedCustomTexture>
		{
			private sealed override object CreateInstance()
			{
				return new MyBorrowedCustomTexture();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyBorrowedCustomTexture CreateInstance()
			{
				return new MyBorrowedCustomTexture();
			}

			MyBorrowedCustomTexture IActivator<MyBorrowedCustomTexture>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		public override Vector3I Size3 => CustomTexture.Size3;

		public override Vector2I Size => CustomTexture.Size;

		public override Resource Resource => CustomTexture.Resource;

		public IRtvTexture Linear => CustomTexture.Linear;

		public IRtvTexture SRgb => CustomTexture.SRgb;

		public ICustomTexture CustomTexture { get; private set; }

		public override ShaderResourceView Srv => CustomTexture.Linear.Srv;

		public UnorderedAccessView Uav => CustomTexture.Uav;

		protected override void CreateTextureInternal(ref MyBorrowedTextureKey key, string debugName)
		{
			CustomTexture = MyManagers.CustomTextures.CreateTexture(debugName, key.Width, key.Height, key.SamplesCount, key.SamplesQuality);
		}
	}
}
