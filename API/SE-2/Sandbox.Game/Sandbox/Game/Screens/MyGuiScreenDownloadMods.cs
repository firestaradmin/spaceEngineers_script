using System;
using System.Text;
using Sandbox.Graphics.GUI;
using VRage;

namespace Sandbox.Game.Screens
{
	public class MyGuiScreenDownloadMods : MyGuiScreenMessageBox
	{
		public MyGuiScreenDownloadMods(Action<ResultEnum> cancelCallback)
			: base(MyMessageBoxStyleEnum.Info, MyMessageBoxButtonsType.OK, new StringBuilder(MyTexts.GetString(MyCommonTexts.ProgressTextCheckingMods)), new StringBuilder(MyTexts.GetString(MyCommonTexts.DownloadingMods)), MyCommonTexts.Cancel, MyCommonTexts.Cancel, MyCommonTexts.Yes, MyCommonTexts.No, cancelCallback, 0, ResultEnum.YES, canHideOthers: true, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
		{
		}
	}
}
