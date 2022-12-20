using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using VRage.Game;

namespace Sandbox.Game.Screens.Helpers
{
	internal class MyRadialMenuItemCubeBlockSingle : MyRadialMenuItemCubeBlock
	{
		private int m_currentIndex;

		public override bool CanBeActivated
		{
			get
			{
				MyDLCs.MyDLC missingDLC;
				return MyRadialMenuItemCubeBlock.IsBlockGroupEnabled(MyCubeBuilder.Static.CubeBuilderState.GetCurrentBlockForBlockVariantGroup(CurrentIndex, base.BlockVariantGroup), out missingDLC) == EnabledState.Enabled;
			}
		}

		public int CurrentIndex
		{
			get
			{
				return m_currentIndex;
			}
			protected set
			{
				if (m_currentIndex != value)
				{
					if (value >= base.BlockVariantGroup.BlockGroups.Length)
					{
						m_currentIndex = 0;
					}
					else
					{
						m_currentIndex = value;
					}
				}
			}
		}

		public static MyRadialMenuItemCubeBlockSingle BuildMenuItem(MyRadialMenuItemCubeBlock menuItem, int? idx = null)
		{
			int num = (idx.HasValue ? idx.Value : MyCubeBuilder.Static.CubeBuilderState.LastSelectedStageIndexForGroup.GetValueOrDefault(menuItem.BlockVariantGroup.Id, 0));
			MyCubeBlockDefinition anyPublic = menuItem.BlockVariantGroup.BlockGroups[num].AnyPublic;
			string item = string.Empty;
			if (anyPublic != null && anyPublic.Icons.Length != 0)
			{
				item = anyPublic.Icons[0];
			}
			MyRadialMenuItemCubeBlockSingle myRadialMenuItemCubeBlockSingle = new MyRadialMenuItemCubeBlockSingle();
			myRadialMenuItemCubeBlockSingle.BlockVariantGroup = menuItem.BlockVariantGroup;
			myRadialMenuItemCubeBlockSingle.CloseMenu = menuItem.CloseMenu;
			myRadialMenuItemCubeBlockSingle.Icons = new List<string>();
			myRadialMenuItemCubeBlockSingle.LabelName = menuItem.LabelName;
			myRadialMenuItemCubeBlockSingle.LabelShortcut = menuItem.LabelShortcut;
			myRadialMenuItemCubeBlockSingle.CurrentIndex = num;
			myRadialMenuItemCubeBlockSingle.Icons.Add(item);
			return myRadialMenuItemCubeBlockSingle;
		}

		public override void Activate(params object[] parameters)
		{
			MyCubeBlockDefinitionGroup currentBlockForBlockVariantGroup = MyCubeBuilder.Static.CubeBuilderState.GetCurrentBlockForBlockVariantGroup(CurrentIndex, base.BlockVariantGroup);
			MyCubeBuilder.Static.CubeBuilderState.SetCurrentBlockForBlockVariantGroup(base.BlockVariantGroup.BlockGroups[CurrentIndex]);
			MyCubeSize cubeSize = MyCubeSize.Large;
			object obj;
			if (parameters.Length != 0 && (obj = parameters[0]) is MyCubeSize)
			{
				MyCubeSize myCubeSize = (MyCubeSize)obj;
				cubeSize = myCubeSize;
			}
			ActivateInner(currentBlockForBlockVariantGroup, cubeSize, useRepresentativeIfPossible: false);
		}

		public override MyCubeBlockDefinitionGroup GetActiveGroup()
		{
			return MyCubeBuilder.Static.CubeBuilderState.GetCurrentBlockForBlockVariantGroup(CurrentIndex, base.BlockVariantGroup);
		}

		public override bool MoveItemIndex(int shift)
		{
			return false;
		}
	}
}
