using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Gui
{
	internal abstract class MyTerminalController
	{
		protected bool m_dirtyDraw;

		public virtual void InvalidateBeforeDraw()
		{
			m_dirtyDraw = true;
		}

		public virtual void UpdateBeforeDraw(MyGuiScreenBase screen)
		{
		}

		public virtual void HandleInput()
		{
		}
	}
}
