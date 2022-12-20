using System;

namespace VRage.Network
{
	[Flags]
	public enum CallSiteFlags
	{
		None = 0x0,
		Client = 0x1,
		Server = 0x2,
		Broadcast = 0x4,
		Reliable = 0x8,
		RefreshReplicable = 0x10,
		BroadcastExcept = 0x20,
		Blocking = 0x40,
		ServerInvoked = 0x80,
		DistanceRadius = 0x100
	}
}
