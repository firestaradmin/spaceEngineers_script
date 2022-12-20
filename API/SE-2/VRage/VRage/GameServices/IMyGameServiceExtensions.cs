using System;

namespace VRage.GameServices
{
	public static class IMyGameServiceExtensions
	{
		public static void RequestPermissions(this IMyGameService thiz, Permissions permission, bool attemptResolution, Action<bool> onDone)
		{
			thiz.RequestPermissions(permission, attemptResolution, delegate(PermissionResult result)
			{
				onDone(result == PermissionResult.Granted);
			});
		}

		public static void RequestPermissionsWithTargetUser(this IMyGameService thiz, Permissions permission, ulong targetUserId, Action<bool> onDone)
		{
			thiz.RequestPermissionsWithTargetUser(permission, targetUserId, delegate(PermissionResult result)
			{
				onDone(result == PermissionResult.Granted);
			});
		}
	}
}
