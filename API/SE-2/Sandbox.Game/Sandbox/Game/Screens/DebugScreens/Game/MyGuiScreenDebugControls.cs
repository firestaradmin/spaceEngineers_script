using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game.Gui;
using VRage.Game;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Screens.DebugScreens.Game
{
	[MyDebugScreen("Game", "Controls")]
	[StaticEventOwner]
	public class MyGuiScreenDebugControls : MyGuiScreenDebugBase
	{
		public MyGuiScreenDebugControls()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Controls", Color.Yellow.ToVector4());
			AddButton("Reload definitions", delegate
			{
				ReloadDefinitions();
			});
		}

		private void ReloadDefinitions()
		{
			MyDefinitionManager.Static.UnloadData(clearPreloaded: true);
			List<MyObjectBuilder_Checkpoint.ModItem> mods = new List<MyObjectBuilder_Checkpoint.ModItem>();
			MyDefinitionManager.Static.LoadData(mods);
		}
	}
}
