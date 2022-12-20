using System.Collections.Generic;
using Sandbox.Graphics.GUI;

namespace Sandbox.Game.Entities.Blocks
{
	public interface IMyMultiTextPanelComponentOwner : IMyTextPanelComponentOwner
	{
		MyMultiTextPanelComponent MultiTextPanel { get; }

		void SelectPanel(List<MyGuiControlListbox.Item> selectedItems);
	}
}
