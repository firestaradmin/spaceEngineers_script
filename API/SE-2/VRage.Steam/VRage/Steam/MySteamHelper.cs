using System;
using Steamworks;
using VRage.GameServices;

namespace VRage.Steam
{
	internal static class MySteamHelper
	{
		public static ERemoteStoragePublishedFileVisibility ToSteam(this MyPublishedFileVisibility visibility)
		{
			switch (visibility)
			{
			case MyPublishedFileVisibility.Public:
				return ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic;
			case MyPublishedFileVisibility.FriendsOnly:
				return ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityFriendsOnly;
			case MyPublishedFileVisibility.Private:
				return ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate;
			case MyPublishedFileVisibility.Unlisted:
				return ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityUnlisted;
			default:
				throw new ArgumentOutOfRangeException("visibility", visibility, null);
			}
		}

		public static MyPublishedFileVisibility ToService(this ERemoteStoragePublishedFileVisibility visibility)
		{
			switch (visibility)
			{
			case ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPublic:
				return MyPublishedFileVisibility.Public;
			case ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityFriendsOnly:
				return MyPublishedFileVisibility.FriendsOnly;
			case ERemoteStoragePublishedFileVisibility.k_ERemoteStoragePublishedFileVisibilityPrivate:
				return MyPublishedFileVisibility.Private;
			default:
				throw new ArgumentOutOfRangeException("visibility", visibility, null);
			}
		}
	}
}
