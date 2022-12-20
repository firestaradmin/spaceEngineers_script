using System;
using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Screens.AdminMenu
{
	internal static class MyAdminMenuTabFactory
	{
		public static MyTabContainer CreateTab(MyGuiScreenBase parentScreen, MyTabControlEnum tabType)
		{
			MyTabContainer myTabContainer = null;
			switch (tabType)
			{
			case MyTabControlEnum.General:
				myTabContainer = new MyTrashGeneralTabContainer(parentScreen);
				break;
			case MyTabControlEnum.Voxel:
				myTabContainer = new MyTrashVoxelTabContainer(parentScreen);
				break;
			case MyTabControlEnum.Other:
				myTabContainer = new MyTrashOtherTabContainer(parentScreen);
				break;
			default:
				throw new NotImplementedException();
			}
			if (myTabContainer != null)
			{
				myTabContainer.Control.Visible = false;
			}
			return myTabContainer;
		}
	}
}
