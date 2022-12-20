using System;
using VRage.Network;

namespace VRage.Render11.Resources
{
	[GenerateActivator]
	internal abstract class MyPersistentResource<TResource, TDescription> : MyPersistentResourceBase<TDescription> where TResource : IDisposable
	{
		private TResource m_resource;

		private bool m_isInit;

		public TResource Resource => m_resource;

		protected sealed override void ChangeDescriptionInternal(ref TDescription description)
		{
			if (m_isInit)
			{
				m_resource = CreateResource(ref description);
			}
		}

		internal sealed override void OnDeviceInit()
		{
			m_resource = CreateResource(ref m_description);
			m_isInit = true;
		}

		protected abstract TResource CreateResource(ref TDescription desc);

		internal sealed override void OnDeviceEnd()
		{
			if (m_isInit)
			{
				m_resource.Dispose();
			}
			m_isInit = false;
		}
	}
}
