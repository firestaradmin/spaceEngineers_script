namespace VRage.GameServices
{
	public class MyNullNetworkingInvite : IMyNetworkingInvite
	{
		public bool IsInviteSupported()
		{
			return false;
		}

		public void OpenInviteOverlay()
		{
		}
	}
}
