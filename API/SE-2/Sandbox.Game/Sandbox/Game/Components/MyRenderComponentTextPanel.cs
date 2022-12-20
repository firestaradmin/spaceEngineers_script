using Sandbox.Common.ObjectBuilders.Definitions;
using Sandbox.Game.Entities.Blocks;

namespace Sandbox.Game.Components
{
	internal class MyRenderComponentTextPanel : MyRenderComponentScreenAreas
	{
		private class Sandbox_Game_Components_MyRenderComponentTextPanel_003C_003EActor
		{
		}

		private MyTextPanel m_textPanel;

		public MyRenderComponentTextPanel(MyTextPanel textPanel)
			: base(textPanel)
		{
			m_textPanel = textPanel;
		}

		public override void AddRenderObjects()
		{
			base.AddRenderObjects();
			if (m_textPanel.BlockDefinition.ScreenAreas != null && m_textPanel.BlockDefinition.ScreenAreas.Count > 0)
			{
				foreach (ScreenArea screenArea in m_textPanel.BlockDefinition.ScreenAreas)
				{
					AddScreenArea(base.RenderObjectIDs, screenArea.Name);
				}
			}
			else
			{
				AddScreenArea(base.RenderObjectIDs, m_textPanel.BlockDefinition.PanelMaterialName);
			}
		}
	}
}
