namespace VRage.Network
{
	public struct MyClientInfo
	{
		private readonly MyClient m_client;

		public bool IsValid => m_client != null;

		public MyClientStateBase State => m_client.State;

		public Endpoint EndpointId => m_client.State.EndpointId;

		public float PriorityMultiplier => m_client.PriorityMultiplier;

		public bool PlayerControllableUsesPredictedPhysics => m_client.PlayerControllableUsesPredictedPhysics;

		internal MyClientInfo(MyClient client)
		{
			m_client = client;
		}

		public bool HasReplicable(IMyReplicable replicable)
		{
			return m_client.Replicables.ContainsKey(replicable);
		}

		public bool IsReplicableReady(IMyReplicable replicable)
		{
			return m_client.IsReplicableReady(replicable);
		}
	}
}
