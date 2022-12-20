using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public class MyActionChat : MyActionBase
	{
		public override void ExecuteAction()
		{
			if (MyGuiScreenChat.Static == null)
			{
				MyGuiScreenHudSpace.Static.VisibleChanged += PostponedOpenChatScreen;
			}
		}

		public void PostponedOpenChatScreen(object sender, bool state)
		{
			if (state)
			{
				OpenChatScreen();
				MyGuiScreenHudSpace.Static.VisibleChanged -= PostponedOpenChatScreen;
			}
		}

		public void OpenChatScreen()
		{
			Vector2 hudPos = new Vector2(0.029f, 0.8f);
			hudPos = MyGuiScreenHudBase.ConvertHudToNormalizedGuiPosition(ref hudPos);
			MyGuiSandbox.AddScreen(new MyGuiScreenChat(hudPos));
		}
	}
}
