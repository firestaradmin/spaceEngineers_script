using VRage.Sync;
using VRageMath;

namespace Sandbox.Game.Entities.Interfaces
{
	public interface IMyLightingLogicSync
	{
		Sync<float, SyncDirection.BothWays> BlinkIntervalSecondsSync { get; }

		Sync<float, SyncDirection.BothWays> BlinkLengthSync { get; }

		Sync<float, SyncDirection.BothWays> BlinkOffsetSync { get; }

		Sync<float, SyncDirection.BothWays> IntensitySync { get; }

		Sync<Color, SyncDirection.BothWays> LightColorSync { get; }

		Sync<float, SyncDirection.BothWays> LightRadiusSync { get; }

		Sync<float, SyncDirection.BothWays> LightFalloffSync { get; }

		Sync<float, SyncDirection.BothWays> LightOffsetSync { get; }
	}
}
