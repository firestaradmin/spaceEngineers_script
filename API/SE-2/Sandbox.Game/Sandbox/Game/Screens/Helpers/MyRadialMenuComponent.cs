using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Engine.Utils;
using Sandbox.Graphics.GUI;
using VRage.Game;
using VRage.Game.Components;
using VRage.Utils;

namespace Sandbox.Game.Screens.Helpers
{
	[MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
	internal class MyRadialMenuComponent : MySessionComponentBase
	{
		private MyRadialMenuSection m_lastUsedBlocks;

		private MyRadialMenuSection m_lastUsedVoxel;

		public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
		{
			base.Init(sessionComponent);
			m_lastUsedBlocks = new MyRadialMenuSection(new List<MyRadialMenuItem>(), MyStringId.GetOrCompute("RadialMenuGroupTitle_LastUsed"));
			MyRadialMenu radialMenuDefinition = MyDefinitionManager.Static.GetRadialMenuDefinition("Toolbar");
			radialMenuDefinition.SectionsComplete.Insert(0, m_lastUsedBlocks);
			radialMenuDefinition.SectionsSurvival.Insert(0, m_lastUsedBlocks);
			radialMenuDefinition.SectionsCreative.Insert(0, m_lastUsedBlocks);
			m_lastUsedVoxel = new MyRadialMenuSection(new List<MyRadialMenuItem>(), MyStringId.GetOrCompute("RadialMenuGroupTitle_LastUsedVoxels"));
			MyRadialMenu radialMenuDefinition2 = MyDefinitionManager.Static.GetRadialMenuDefinition("VoxelHand");
			radialMenuDefinition2.SectionsComplete.Insert(0, m_lastUsedVoxel);
			radialMenuDefinition2.SectionsSurvival.Insert(0, m_lastUsedVoxel);
			radialMenuDefinition2.SectionsCreative.Insert(0, m_lastUsedVoxel);
		}

		public void InitDefaultLastUsed(MyObjectBuilder_Toolbar toolbar)
		{
			foreach (MyObjectBuilder_Toolbar.Slot slot in toolbar.Slots)
			{
				if (m_lastUsedBlocks.Items.Count >= 8)
				{
					break;
				}
				MyObjectBuilder_ToolbarItemCubeBlock myObjectBuilder_ToolbarItemCubeBlock;
				if ((myObjectBuilder_ToolbarItemCubeBlock = slot.Data as MyObjectBuilder_ToolbarItemCubeBlock) == null)
				{
					continue;
				}
				MyCubeBlockDefinition cubeBlockDefinition = MyDefinitionManager.Static.GetCubeBlockDefinition(myObjectBuilder_ToolbarItemCubeBlock.DefinitionId);
				MyBlockVariantGroup blockVariantsGroup = cubeBlockDefinition.BlockVariantsGroup;
				int value = 0;
				int num = 0;
				MyCubeBlockDefinitionGroup[] blockGroups = blockVariantsGroup.BlockGroups;
				for (int i = 0; i < blockGroups.Length; i++)
				{
					if (blockGroups[i].Contains(cubeBlockDefinition, checkStages: false))
					{
						value = num;
						break;
					}
					num++;
				}
				MyObjectBuilder_RadialMenuItemCubeBlock builder = new MyObjectBuilder_RadialMenuItemCubeBlock
				{
					Id = blockVariantsGroup.Id
				};
				MyRadialMenuItemCubeBlock myRadialMenuItemCubeBlock = new MyRadialMenuItemCubeBlock();
				myRadialMenuItemCubeBlock.Init(builder);
				PushLastUsedBlock(myRadialMenuItemCubeBlock, value);
			}
		}

		public void PushLastUsedBlock(MyRadialMenuItemCubeBlock block, int? idx = null, bool pushOnlyNewGroups = false)
		{
			if (block != null && !(idx < 0) && (!idx.HasValue || !LastBlocksContainBlock(block, idx.Value)) && (!pushOnlyNewGroups || !idx.HasValue || !LastBlocksContainGroup(block, idx.Value)))
			{
				m_lastUsedBlocks.Items.Insert(0, block.BuildRecentMenuItem(idx));
				if (m_lastUsedBlocks.Items.Count > 8)
				{
					m_lastUsedBlocks.Items.RemoveAt(8);
				}
			}
		}

		public bool LastBlocksContainBlock(MyRadialMenuItemCubeBlock block, int id = 0)
		{
			foreach (MyRadialMenuItem item in m_lastUsedBlocks.Items)
			{
				MyRadialMenuItemCubeBlockSingle myRadialMenuItemCubeBlockSingle;
				MyRadialMenuItemCubeBlock myRadialMenuItemCubeBlock;
				if ((myRadialMenuItemCubeBlockSingle = item as MyRadialMenuItemCubeBlockSingle) != null)
				{
					if (myRadialMenuItemCubeBlockSingle.BlockVariantGroup.Id == block.BlockVariantGroup.Id && myRadialMenuItemCubeBlockSingle.CurrentIndex == id)
					{
						return true;
					}
				}
				else if ((myRadialMenuItemCubeBlock = item as MyRadialMenuItemCubeBlock) != null && myRadialMenuItemCubeBlock.BlockVariantGroup.Id == block.BlockVariantGroup.Id)
				{
					return true;
				}
			}
			return false;
		}

		public bool LastBlocksContainGroup(MyRadialMenuItemCubeBlock block, int id = 0)
		{
			foreach (MyRadialMenuItem item in m_lastUsedBlocks.Items)
			{
				MyRadialMenuItemCubeBlockSingle myRadialMenuItemCubeBlockSingle;
				MyRadialMenuItemCubeBlock myRadialMenuItemCubeBlock;
				if ((myRadialMenuItemCubeBlockSingle = item as MyRadialMenuItemCubeBlockSingle) != null)
				{
					if (myRadialMenuItemCubeBlockSingle.BlockVariantGroup.Id == block.BlockVariantGroup.Id)
					{
						return true;
					}
				}
				else if ((myRadialMenuItemCubeBlock = item as MyRadialMenuItemCubeBlock) != null && myRadialMenuItemCubeBlock.BlockVariantGroup.Id == block.BlockVariantGroup.Id)
				{
					return true;
				}
			}
			return false;
		}

		public void PushLastUsedVoxel(MyRadialMenuItemVoxelHand voxel)
		{
			if (voxel != null)
			{
				if (m_lastUsedVoxel.Items.Contains(voxel))
				{
					m_lastUsedVoxel.Items.Remove(voxel);
				}
				m_lastUsedVoxel.Items.Insert(0, voxel);
				if (m_lastUsedVoxel.Items.Count > 8)
				{
					m_lastUsedVoxel.Items.RemoveAt(8);
				}
			}
		}

		public void ShowSystemRadialMenu(MyStringId context, Func<bool> inputCallback)
		{
			string subtype = ((context == MySpaceBindingCreator.AX_ACTIONS) ? "SystemShip" : ((!(context == MySpaceBindingCreator.AX_BUILD) && !(context == MySpaceBindingCreator.AX_SYMMETRY)) ? "SystemDefault" : "SystemBuild"));
			MyGuiSandbox.AddScreen(new MyGuiControlRadialMenuSystem(MyDefinitionManager.Static.GetRadialMenuDefinition(subtype), MyControlsSpace.SYSTEM_RADIAL_MENU, inputCallback));
		}
	}
}
