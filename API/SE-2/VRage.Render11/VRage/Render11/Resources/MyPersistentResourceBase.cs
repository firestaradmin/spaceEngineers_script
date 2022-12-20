using System.Diagnostics;

namespace VRage.Render11.Resources
{
	[DebuggerDisplay("Name = {Name}")]
	internal abstract class MyPersistentResourceBase<TDescription> : IMyPersistentResource<TDescription>
	{
		private string m_name;

		protected TDescription m_description;

		public string Name => m_name;

		public TDescription Description => m_description;

		internal void Init(string name, ref TDescription desc)
		{
			m_name = name;
			m_description = CloneDescription(ref desc);
		}

		public void ChangeDescription(ref TDescription desc)
		{
			TDescription description = CloneDescription(ref desc);
			ChangeDescriptionInternal(ref desc);
			m_description = description;
		}

		protected abstract void ChangeDescriptionInternal(ref TDescription desc);

		protected virtual TDescription CloneDescription(ref TDescription desc)
		{
			return desc;
		}

		internal abstract void OnDeviceInit();

		internal abstract void OnDeviceEnd();
	}
}
