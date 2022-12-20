using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using VRage;
using VRage.Game;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	[MyRadialMenuItemDescriptor(typeof(MyObjectBuilder_RadialMenuItemCubeBlock))]
	internal class MyRadialMenuItemCubeBlock : MyRadialMenuItem
	{
		[Flags]
		public enum EnabledState
		{
			Enabled = 0x0,
			DLC = 0x1,
			Research = 0x2,
			Other = 0x4
		}

		public override bool CanBeActivated
		{
			get
			{
				MyDLCs.MyDLC missingDLC;
				return IsBlockGroupEnabled(MyCubeBuilder.Static.CubeBuilderState.GetCurrentBlockForBlockVariantGroup(BlockVariantGroup), out missingDLC) == EnabledState.Enabled;
			}
		}

		public MyBlockVariantGroup BlockVariantGroup { get; protected set; }

		public override bool IsValid => MyDefinitionManager.Static.GetBlockVariantGroupDefinitions().ContainsKey(BlockVariantGroup.Id.SubtypeName);

		public override void Init(MyObjectBuilder_RadialMenuItem builder)
		{
			MyObjectBuilder_RadialMenuItemCubeBlock myObjectBuilder_RadialMenuItemCubeBlock = (MyObjectBuilder_RadialMenuItemCubeBlock)builder;
			MyDefinitionManager.Static.GetBlockVariantGroupDefinitions().TryGetValue(myObjectBuilder_RadialMenuItemCubeBlock.Id.SubtypeId, out var value);
			BlockVariantGroup = value;
			Icons = new List<string>();
			base.LabelName = string.Empty;
			if (value != null)
			{
				if (value.DisplayNameEnum.HasValue)
				{
					base.LabelName = MyTexts.GetString(value.DisplayNameEnum.Value);
				}
				else
				{
					MyDefinitionErrors.Add(MyModContext.UnknownContext, "Block " + myObjectBuilder_RadialMenuItemCubeBlock.Id.TypeIdString + "/" + myObjectBuilder_RadialMenuItemCubeBlock.Id.SubtypeId + " block variant group doesn't have `DisplayNameEnum` property", TErrorSeverity.Warning);
				}
				Icons.Add(value.Icons[0]);
			}
			else
			{
				MyDefinitionErrors.Add(MyModContext.UnknownContext, "Block " + myObjectBuilder_RadialMenuItemCubeBlock.Id.TypeIdString + "/" + myObjectBuilder_RadialMenuItemCubeBlock.Id.SubtypeId + " doesn't have block variant group!", TErrorSeverity.Warning);
			}
			CloseMenu = builder.CloseMenu;
		}

		public override bool Enabled()
		{
			MyCubeBlockDefinition[] blocks = BlockVariantGroup.Blocks;
			for (int i = 0; i < blocks.Length; i++)
			{
				if (IsBlockEnabled(blocks[i], out var _) == EnabledState.Enabled)
				{
					return true;
				}
			}
			return false;
		}

		public override void Activate(params object[] parameters)
		{
			MyCubeBlockDefinitionGroup currentBlockForBlockVariantGroup = MyCubeBuilder.Static.CubeBuilderState.GetCurrentBlockForBlockVariantGroup(BlockVariantGroup);
			MyCubeSize cubeSize = MyCubeSize.Large;
			object obj;
			if (parameters.Length != 0 && (obj = parameters[0]) is MyCubeSize)
			{
				MyCubeSize myCubeSize = (MyCubeSize)obj;
				cubeSize = myCubeSize;
			}
			ActivateInner(currentBlockForBlockVariantGroup, cubeSize, useRepresentativeIfPossible: false);
		}

		public virtual void ActivateInner(MyCubeBlockDefinitionGroup targetPair, MyCubeSize cubeSize, bool useRepresentativeIfPossible = true)
		{
			MyDefinitionId weaponDefinition = new MyDefinitionId(typeof(MyObjectBuilder_CubePlacer));
			MySession.Static.LocalCharacter?.SwitchToWeapon(weaponDefinition);
			MyCubeBuilder.Static.CubeBuilderState.SetCubeSize(cubeSize);
			MyCubeBlockDefinition myCubeBlockDefinition = targetPair[cubeSize] ?? targetPair.AnyPublic;
			MyCubeBlockDefinition myCubeBlockDefinition2 = myCubeBlockDefinition.BlockVariantsGroup?.PrimaryGUIBlock;
			if (useRepresentativeIfPossible && myCubeBlockDefinition2 != null)
			{
				myCubeBlockDefinition = myCubeBlockDefinition2;
			}
			MyCubeBuilder.Static.Activate(myCubeBlockDefinition.Id);
			MyCubeBuilder.Static.SetToolType(MyCubeBuilderToolType.BuildTool);
			if (MySessionComponentVoxelHand.Static.Enabled)
			{
				MySessionComponentVoxelHand.Static.Enabled = false;
			}
		}

		public static EnabledState IsBlockGroupEnabled(MyCubeBlockDefinitionGroup blocks, out MyDLCs.MyDLC missingDLC)
		{
			MyDLCs.MyDLC missingDLC2;
			EnabledState enabledState = IsBlockEnabled(blocks.Small, out missingDLC2);
			MyDLCs.MyDLC missingDLC3;
			EnabledState enabledState2 = IsBlockEnabled(blocks.Large, out missingDLC3);
			if (enabledState > enabledState2)
			{
				missingDLC = missingDLC2;
				return enabledState;
			}
			missingDLC = missingDLC3;
			return enabledState2;
		}

		public static EnabledState IsBlockEnabled(MyCubeBlockDefinition block, out MyDLCs.MyDLC missingDLC)
		{
			missingDLC = null;
			EnabledState enabledState = EnabledState.Enabled;
			if (block != null)
			{
				if (!block.Public)
				{
					enabledState |= EnabledState.Other;
				}
				MySessionComponentDLC component = MySession.Static.GetComponent<MySessionComponentDLC>();
				missingDLC = component.GetFirstMissingDefinitionDLC(block, Sync.MyId);
				if (!MySessionComponentResearch.Static.CanUse(MySession.Static.LocalPlayerId, block.Id) && !MySession.Static.CreativeToolsEnabled(Sync.MyId))
				{
					enabledState |= EnabledState.Research;
				}
				if (missingDLC != null)
				{
					enabledState |= EnabledState.DLC;
				}
			}
			return enabledState;
		}

		public virtual bool MoveItemIndex(int shift)
		{
			MyCubeBlockDefinitionGroup activeGroup = GetActiveGroup();
			MyCubeBlockDefinitionGroup[] blockGroups = BlockVariantGroup.BlockGroups;
			int num = Array.IndexOf(blockGroups, activeGroup);
			int num2 = num;
			do
			{
				num2 = MyMath.Mod(num2 + shift, blockGroups.Length);
				MyCubeBlockDefinitionGroup myCubeBlockDefinitionGroup = blockGroups[num2];
				if (IsBlockGroupEnabled(myCubeBlockDefinitionGroup, out var _) < EnabledState.Other)
				{
					MyCubeBuilder.Static.CubeBuilderState.SetCurrentBlockForBlockVariantGroup(myCubeBlockDefinitionGroup);
					return true;
				}
			}
			while (num2 != num);
			return false;
		}

		public virtual MyCubeBlockDefinitionGroup GetActiveGroup()
		{
			return MyCubeBuilder.Static.CubeBuilderState.GetCurrentBlockForBlockVariantGroup(BlockVariantGroup);
		}

		public virtual MyCubeBlockDefinitionGroup GetFirstGroup()
		{
			return MyCubeBuilder.Static.CubeBuilderState.GetFirstBlockForBlockVariantGroup(BlockVariantGroup);
		}

		public virtual int GetCurrentIndex()
		{
			MyCubeBlockDefinitionGroup activeGroup = GetActiveGroup();
			return Array.IndexOf(BlockVariantGroup.BlockGroups, activeGroup);
		}

		public virtual MyRadialMenuItemCubeBlockSingle BuildRecentMenuItem(int? idx = null)
		{
			return MyRadialMenuItemCubeBlockSingle.BuildMenuItem(this, idx);
		}
	}
}
