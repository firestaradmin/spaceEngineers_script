using VRage.Network;

namespace Sandbox.Game.Replication
{
	/// <summary>
	/// Same as MyExternalReplicableEvent, but with support for event proxy.
	/// </summary>
	public abstract class MyExternalReplicableEvent<T> : MyExternalReplicable<T>, IMyProxyTarget, IMyNetObject, IMyEventOwner where T : IMyEventProxy
	{
		private IMyEventProxy m_proxy;

		IMyEventProxy IMyProxyTarget.Target => m_proxy;

		protected override void OnHook()
		{
			m_proxy = base.Instance;
		}
	}
}
