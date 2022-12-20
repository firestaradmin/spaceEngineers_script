using SharpDX.Direct3D11;
using SharpDX.DXGI;
using VRageRender;

namespace VRage.Render11.Resources.Buffers
{
	internal class MyUavBuffer : MyBufferInternal, IUavBuffer, IUavBindable, IResource, IBuffer
	{
		private UnorderedAccessView m_uav;

		public UnorderedAccessView Uav => m_uav;

		public MyUavType UavType { get; set; }

		protected override void AfterBufferInit()
		{
			if (UavType == MyUavType.Default)
			{
				m_uav = new UnorderedAccessView(MyRender11.DeviceInstance, base.Resource);
			}
			else
			{
				UnorderedAccessViewDescription unorderedAccessViewDescription = default(UnorderedAccessViewDescription);
				unorderedAccessViewDescription.Buffer = new UnorderedAccessViewDescription.BufferResource
				{
					ElementCount = base.ElementCount,
					FirstElement = 0,
					Flags = ((UavType == MyUavType.Append) ? UnorderedAccessViewBufferFlags.Append : UnorderedAccessViewBufferFlags.Counter)
				};
				unorderedAccessViewDescription.Format = Format.Unknown;
				unorderedAccessViewDescription.Dimension = UnorderedAccessViewDimension.Buffer;
				UnorderedAccessViewDescription description = unorderedAccessViewDescription;
				m_uav = new UnorderedAccessView(MyRender11.DeviceInstance, base.Resource, description);
			}
			m_uav.DebugName = base.Name + "_Uav";
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
