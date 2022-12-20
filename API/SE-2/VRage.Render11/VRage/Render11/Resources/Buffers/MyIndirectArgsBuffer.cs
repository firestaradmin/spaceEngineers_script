using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRageRender;

namespace VRage.Render11.Resources.Buffers
{
	internal class MyIndirectArgsBuffer : MyBufferInternal, IIndirectResourcesBuffer, IUavBindable, IResource, IBuffer
	{
		private UnorderedAccessView m_uav;

		public Format Format { get; set; }

		public UnorderedAccessView Uav => m_uav;

		protected override void AfterBufferInit()
		{
			UnorderedAccessViewDescription unorderedAccessViewDescription = default(UnorderedAccessViewDescription);
			unorderedAccessViewDescription.Buffer = new UnorderedAccessViewDescription.BufferResource
			{
				ElementCount = base.ElementCount,
				FirstElement = 0,
				Flags = UnorderedAccessViewBufferFlags.None
			};
			unorderedAccessViewDescription.Format = Format;
			unorderedAccessViewDescription.Dimension = UnorderedAccessViewDimension.Buffer;
			UnorderedAccessViewDescription description = unorderedAccessViewDescription;
			m_uav = new UnorderedAccessView(MyRender11.DeviceInstance, base.Resource, description)
			{
				DebugName = base.Name + "_Uav"
			};
		}

		public override void DisposeInternal()
		{
			if (m_uav != null)
			{
				m_uav.Dispose();
				m_uav = null;
			}
			base.DisposeInternal();
		}
	}
}
