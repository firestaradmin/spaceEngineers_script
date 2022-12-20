using System;

namespace Sandbox.Graphics.GUI
{
	[Flags]
	public enum UrlOpenMode
	{
		SteamOverlay = 0x1,
		ExternalBrowser = 0x2,
		ConfirmExternal = 0x4,
		ExternalWithConfirm = 0x6,
		SteamOrExternalWithConfirm = 0x7
	}
}
