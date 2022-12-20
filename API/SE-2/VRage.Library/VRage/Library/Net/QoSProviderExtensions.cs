namespace VRage.Library.Net
{
	/// <summary>
	/// Extension methods for the QoS provider.
	/// </summary>
	public static class QoSProviderExtensions
	{
		/// <summary>
		/// Process all queues in the provider.
		/// </summary>
		/// <param name="provider"></param>
		public static void ProcessQueues(this IQoSProvider provider)
		{
			provider.ProcessReadQueues();
			provider.ProcessWriteQueues();
		}
	}
}
