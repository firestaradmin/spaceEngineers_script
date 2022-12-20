using SharpDX.Direct3D11;
using VRageRender;

namespace VRage.Render11.Resources.Internal
{
	internal class MyArraySubresourceUav : MyArraySubresourceInternal, IUavArraySubresource, IUavBindable, IResource
	{
		public UnorderedAccessView Uav { get; private set; }

		public void OnDeviceInit()
		{
			UnorderedAccessViewDescription unorderedAccessViewDescription = default(UnorderedAccessViewDescription);
			unorderedAccessViewDescription.Format = m_format;
			unorderedAccessViewDescription.Dimension = UnorderedAccessViewDimension.Texture2DArray;
			unorderedAccessViewDescription.Texture2DArray.ArraySize = 1;
			unorderedAccessViewDescription.Texture2DArray.FirstArraySlice = m_slice;
			unorderedAccessViewDescription.Texture2DArray.MipSlice = m_mipLevel;
			UnorderedAccessViewDescription description = unorderedAccessViewDescription;
			Uav = new UnorderedAccessView(MyRender11.DeviceInstance, m_owner.Resource, description);
		}

		public void OnDeviceEnd()
		{
			if (Uav != null)
			{
				Uav.Dispose();
				Uav = null;
			}
		}
	}
}
