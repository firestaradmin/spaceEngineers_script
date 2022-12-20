using System;
using System.Collections.Generic;
using System.Text;
using Sandbox.Definitions;
using Sandbox.Engine.Networking;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Game.ObjectBuilders.Components;
using VRage.Input;
using VRage.Library.Utils;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers
{
	internal class MyGuiControlRadialMenuBlock : MyGuiControlRadialMenuBase
	{
		private MyCubeSize m_currentSizeSelection;

		private MyCubeBlockDefinition m_currentBlock;

		private readonly MyGuiControlPcuBar m_pcuBar;

		private readonly MyGuiControlImage m_infoHintsBk;

		private readonly MyGuiControlImage m_blockSizeSmall;

		private readonly MyGuiControlImage m_blockSizeLarge;

		private readonly MyGuiControlBlockGroupInfo m_blockDetail;

		private readonly MyGuiControlMultilineText m_buildPlannerHint;

		private readonly MyGuiControlMultilineText m_cycleBlocksHint;

		private MyGuiControlImage[] m_missingRequirementIcons;

		public static event Action<MyGuiControlRadialMenuBlock> OnSelectionConfirmed;

		public MyGuiControlRadialMenuBlock(MyRadialMenu data, MyStringId closingControl, Func<bool> handleInputCallback)
			: base(data, closingControl, handleInputCallback)
		{
			Vector2 vector = new Vector2(90f) / MyGuiConstants.GUI_OPTIMAL_SIZE;
			MyGuiControlImage myGuiControlImage = new MyGuiControlImage(backgroundColor: new Vector4(1f, 1f, 1f, 0.8f), position: new Vector2(0f, 0.365f), size: MyGuiConstants.TEXTURE_BUTTON_DEFAULT_NORMAL.MinSizeGui, backgroundTexture: null, textures: new string[1] { "Textures\\GUI\\Controls\\button_default_outlineless.dds" });
			AddControl(myGuiControlImage);
			AddControl(m_blockSizeLarge = new MyGuiControlImage(null, vector * 0.7f));
			AddControl(m_blockSizeSmall = new MyGuiControlImage(null, vector * 0.7f));
			m_blockSizeLarge.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\GridSizeLarge.png");
			m_blockSizeSmall.SetTexture("Textures\\GUI\\Icons\\HUD 2017\\GridSizeSmall.png");
			float num = 0.003f;
			AddControl(new MyGuiControlLabel(new Vector2(0.028f, 0.365f + num), null, MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION1_MOD1), null, 0.8f, "Blue", MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER));
			m_blockDetail = new MyGuiControlBlockGroupInfo();
			m_blockDetail.Init(new MyObjectBuilder_GuiControlBlockGroupInfo
			{
				Name = string.Empty,
				Size = new Vector2(0.27f, 0.68f),
				Position = new Vector2(0.3f, -0.355f),
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP
			});
			AddControl(m_blockDetail);
			m_blockDetail.RegisterAllControls(Controls);
			m_blockDetail.UpdateArrange();
			float x = m_blockDetail.Position.X + m_blockDetail.Size.X;
			float x2 = m_blockDetail.Size.X;
			m_infoHintsBk = new MyGuiControlImage(backgroundColor: new Vector4(1f, 1f, 1f, 0.8f), position: new Vector2(x, 0.365f - myGuiControlImage.Size.Y / 2f), size: new Vector2(x2, 0.12f), backgroundTexture: null, textures: new string[1] { "Textures\\GUI\\Controls\\button_default_outlineless.dds" }, toolTip: null, originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_TOP);
			m_infoHintsBk.Visible = false;
			AddControl(m_infoHintsBk);
			m_buildPlannerHint = new MyGuiControlMultilineText();
			m_buildPlannerHint.Position = m_infoHintsBk.Position - new Vector2(m_infoHintsBk.Size.X, 0f) + new Vector2(0.01f, 0.01f);
			m_buildPlannerHint.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_buildPlannerHint.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_buildPlannerHint.Size = m_infoHintsBk.Size;
			m_buildPlannerHint.Margin = new Thickness(0.015f, 0.005f, 0.015f, 0.005f);
			Controls.Add(m_buildPlannerHint);
			m_buildPlannerHint.Parse();
			m_cycleBlocksHint = new MyGuiControlMultilineText();
			m_cycleBlocksHint.Position = m_infoHintsBk.Position - new Vector2(m_infoHintsBk.Size.X, 0f) + new Vector2(0.01f, 0.09f);
			m_cycleBlocksHint.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_cycleBlocksHint.TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
			m_cycleBlocksHint.Size = m_infoHintsBk.Size + new Vector2(0.01f, 0.01f);
			RegenerateBlockHints();
			m_cycleBlocksHint.Margin = new Thickness(0.015f, 0.005f, 0.015f, 0.005f);
			AddControl(m_cycleBlocksHint);
			m_cycleBlocksHint.Parse();
			m_pcuBar = new MyGuiControlPcuBar(new Vector2(0f, 0.459f), 0.548f)
			{
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_BOTTOM
			};
			m_pcuBar.UpdatePCU(GetIdentity(), performAnimation: false);
			AddControl(m_pcuBar);
			UpdateBuildPlanner();
			RefreshBlockSize(MyCubeBuilder.Static.CubeBuilderState.CubeSizeMode);
			MyCubeBuilder.Static.CubeBuilderState.OnBlockSizeChanged += RefreshBlockSize;
			SwitchSection(0);
		}

		protected override void RegenerateBlockHints()
		{
			if (m_currentSection < 0 || m_currentSection >= m_data.CurrentSections.Count)
			{
				m_cycleBlocksHint.Text = new StringBuilder();
				return;
			}
			List<MyRadialMenuItem> items = m_data.CurrentSections[m_currentSection].Items;
			if (m_selectedButton < 0 || m_selectedButton >= items.Count)
			{
				m_cycleBlocksHint.Text = new StringBuilder();
			}
			else if (items[m_selectedButton] is MyRadialMenuItemCubeBlockSingle)
			{
				m_cycleBlocksHint.Text = new StringBuilder();
			}
			else
			{
				m_cycleBlocksHint.Text = new StringBuilder(string.Format(MyTexts.GetString(MySpaceTexts.RadialMenu_HintCycleBlocks) + "\n", MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACCEPT)));
			}
		}

		private static MyIdentity GetIdentity()
		{
			MyCharacter localCharacter = MySession.Static.LocalCharacter;
			MyPlayer myPlayer = null;
			if (localCharacter != null)
			{
				myPlayer = MyPlayer.GetPlayerFromCharacter(localCharacter);
			}
			if (myPlayer == null)
			{
				MyShipController myShipController = MyToolbarComponent.CurrentToolbar.Owner as MyShipController;
				if (myShipController?.Pilot != null && myShipController.ControllerInfo.Controller != null)
				{
					myPlayer = myShipController.ControllerInfo.Controller.Player;
				}
			}
			return myPlayer?.Identity;
		}

		public override bool Update(bool hasFocus)
		{
			m_pcuBar.UpdatePCU(GetIdentity(), performAnimation: true);
			return base.Update(hasFocus);
		}

		protected override void ActivateItem(MyRadialMenuItem item)
		{
			item.Activate(m_currentSizeSelection);
			MyRadialMenuItemCubeBlock myRadialMenuItemCubeBlock;
			if ((myRadialMenuItemCubeBlock = item as MyRadialMenuItemCubeBlock) != null)
			{
				MySession.Static.GetComponent<MyRadialMenuComponent>().PushLastUsedBlock(myRadialMenuItemCubeBlock, myRadialMenuItemCubeBlock.GetCurrentIndex());
			}
			MyGuiControlRadialMenuBlock.OnSelectionConfirmed.InvokeIfNotNull(this);
		}

		protected override void OnClosed()
		{
			MyCubeBuilder.Static.CubeBuilderState.OnBlockSizeChanged -= RefreshBlockSize;
			base.OnClosed();
		}

		private void RefreshBlockSize(MyCubeSize size)
		{
			m_currentSizeSelection = size;
			UpdateTooltip();
			m_blockDetail.OnUserSizePreferenceChanged(size);
		}

		public void UpdateBuildPlanner()
		{
			m_blockDetail.UpdateBuildPlanner();
		}

		protected override void UpdateTooltip()
		{
			bool flag = false;
			MyDLCs.MyDLC missingDLC = null;
			int allowedCubeSizes = 0;
			MyCubeSize cubeSizeMode = MyCubeBuilder.Static.CubeBuilderState.CubeSizeMode;
			if (MyCubeBuilder.Static.IsActivated)
			{
				m_currentSizeSelection = cubeSizeMode;
			}
			List<MyRadialMenuItem> items = m_data.CurrentSections[m_currentSection].Items;
			if (m_selectedButton >= 0 && m_selectedButton < items.Count)
			{
				MyRadialMenuItem myRadialMenuItem = items[m_selectedButton];
				MyCubeBlockDefinitionGroup myCubeBlockDefinitionGroup = null;
				if ((myRadialMenuItem as MyRadialMenuItemCubeBlock)?.BlockVariantGroup?.BlockGroups != null)
				{
					myCubeBlockDefinitionGroup = (myRadialMenuItem as MyRadialMenuItemCubeBlock)?.GetFirstGroup();
<<<<<<< HEAD
					if (myCubeBlockDefinitionGroup != null)
=======
					MyCubeSize[] values = MyEnum<MyCubeSize>.Values;
					foreach (MyCubeSize myCubeSize in values)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					{
						MyCubeSize[] values = MyEnum<MyCubeSize>.Values;
						foreach (MyCubeSize myCubeSize in values)
						{
							if (myCubeBlockDefinitionGroup[myCubeSize] != null)
							{
								allowedCubeSizes |= 1 << (int)myCubeSize;
							}
						}
						if (myCubeBlockDefinitionGroup[m_currentSizeSelection] == null)
						{
							m_currentSizeSelection = myCubeBlockDefinitionGroup.Any.CubeSize;
						}
						flag = MyRadialMenuItemCubeBlock.IsBlockGroupEnabled(myCubeBlockDefinitionGroup, out missingDLC) >= MyRadialMenuItemCubeBlock.EnabledState.Research;
						m_infoHintsBk.Visible = true;
						m_blockDetail.Visible = true;
						m_blockDetail.SetBlockGroup(myCubeBlockDefinitionGroup);
						m_currentBlock = myCubeBlockDefinitionGroup[m_currentSizeSelection];
						m_buildPlannerHint.Visible = true;
						m_cycleBlocksHint.Visible = true;
					}
				}
<<<<<<< HEAD
=======
				flag = MyRadialMenuItemCubeBlock.IsBlockGroupEnabled(myCubeBlockDefinitionGroup, out missingDLC) >= MyRadialMenuItemCubeBlock.EnabledState.Research;
				m_infoHintsBk.Visible = true;
				m_blockDetail.Visible = true;
				m_blockDetail.SetBlockGroup(myCubeBlockDefinitionGroup);
				m_currentBlock = myCubeBlockDefinitionGroup[m_currentSizeSelection];
				m_buildPlannerHint.Visible = true;
				m_cycleBlocksHint.Visible = true;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else
			{
				allowedCubeSizes = int.MaxValue;
				m_currentBlock = null;
				m_blockDetail.Visible = false;
				m_infoHintsBk.Visible = false;
				m_buildPlannerHint.Visible = false;
				m_cycleBlocksHint.Visible = false;
			}
			SetCubeSizeIconVisibility(MyCubeSize.Small, m_blockSizeSmall);
			SetCubeSizeIconVisibility(MyCubeSize.Large, m_blockSizeLarge);
			StringBuilder stringBuilder = m_buildPlannerHint.Text.Clear();
			string codeForControl = MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION1);
			string codeForControl2 = MyControllerHelper.GetCodeForControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION2);
			if (flag || missingDLC != null)
			{
				if (flag)
				{
					stringBuilder.Append(codeForControl).Append(' ');
					stringBuilder.Append(MyTexts.GetString(MySpaceTexts.ControlMenuItemLabel_ShowProgressionTree));
					stringBuilder.AppendLine();
				}
				if (missingDLC != null)
				{
					stringBuilder.Append(codeForControl2).Append(' ');
					stringBuilder.AppendFormat(MyCommonTexts.ShowDlcStore, missingDLC.DisplayName);
					stringBuilder.AppendLine();
				}
			}
			else
			{
				stringBuilder.AppendFormat(MySpaceTexts.BuildPlannerHint, codeForControl, codeForControl2);
			}
			m_buildPlannerHint.RefreshText(useEnum: false);
			void SetCubeSizeIconVisibility(MyCubeSize size, MyGuiControlImage icon)
			{
				if (icon != null)
				{
					int num = 1 << (int)size;
					bool flag2 = (allowedCubeSizes & ~num) != 0;
					icon.Visible = (allowedCubeSizes & num) != 0;
					icon.ColorMask = new Vector4(1f, 1f, 1f, (m_currentSizeSelection != size && flag2) ? 0.2f : 1f);
					float num2 = -0.025f;
					if (!flag2)
					{
						float num3 = icon.Size.X / 4f;
						num2 = ((size != 0) ? (num2 + num3) : (num2 - num3));
					}
					icon.Position = new Vector2(num2, 0.365f);
				}
			}
		}

		public override void HandleInput(bool receivedFocusInThisUpdate)
		{
			bool flag = false;
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.SHIFT_RIGHT))
			{
				if (m_data == null)
				{
					MyLog.Default.Error("RadialMenuBlock m_data are null!");
				}
				if (m_data.CurrentSections.Count < m_currentSection || m_data.CurrentSections[m_currentSection] == null)
				{
					MyLog.Default.Error("Section not present!");
				}
				List<MyRadialMenuItem> items = m_data.CurrentSections[m_currentSection].Items;
				if (items != null && m_selectedButton >= 0 && m_selectedButton < items.Count)
				{
					MyObjectBuilder_ToolbarItemDefinition myObjectBuilder_ToolbarItemDefinition = (m_blockDetail.GetSelectedVariant().UserData as MyGuiScreenToolbarConfigBase.GridItemUserData)?.ItemData() as MyObjectBuilder_ToolbarItemDefinition;
					if (myObjectBuilder_ToolbarItemDefinition != null && MyDefinitionManager.Static.TryGetDefinition<MyDefinitionBase>(myObjectBuilder_ToolbarItemDefinition.DefinitionId, out var definition))
					{
						MyCharacter localCharacter = MySession.Static.LocalCharacter;
						MyDefinitionId myDefinitionId = new MyDefinitionId(typeof(MyObjectBuilder_CubePlacer));
						if (localCharacter != null && MySessionComponentSafeZones.IsActionAllowed(localCharacter, MySafeZoneAction.Building, 0L, 0uL))
						{
							if (localCharacter.CurrentWeapon == null || !(localCharacter.CurrentWeapon.DefinitionId == myDefinitionId))
							{
								localCharacter.SwitchToWeapon(myDefinitionId);
							}
							MyCubeBuilder.Static.CubeBuilderState.SetCubeSize((m_currentSizeSelection == MyCubeSize.Large) ? MyCubeSize.Small : MyCubeSize.Large);
							MyCubeBuilder.Static.ActivateFromRadialMenu(definition.Id);
							MyCubeBuilder.Static.SetToolType(MyCubeBuilderToolType.BuildTool);
							MyRadialMenuItemCubeBlock myRadialMenuItemCubeBlock;
							if ((myRadialMenuItemCubeBlock = items[m_selectedButton] as MyRadialMenuItemCubeBlock) != null)
							{
								MySession.Static.GetComponent<MyRadialMenuComponent>().PushLastUsedBlock(myRadialMenuItemCubeBlock, myRadialMenuItemCubeBlock.GetCurrentIndex(), pushOnlyNewGroups: true);
							}
							MyDefinitionId[] array = m_currentBlock?.BlockStages;
							if (array == null)
							{
								if (m_currentBlock != null && m_currentBlock.BlockVariantsGroup != null && m_currentBlock.BlockVariantsGroup.PrimaryGUIBlock != null && m_currentBlock.BlockVariantsGroup.PrimaryGUIBlock.BlockStages != null)
								{
									array = m_currentBlock.BlockVariantsGroup.PrimaryGUIBlock.BlockStages;
								}
							}
							else
							{
								MyCubeBuilder.Static.CubeBuilderState.CurrentBlockDefinitionStages.Add(m_currentBlock);
							}
							if (array != null)
							{
								MyDefinitionId[] array2 = array;
								foreach (MyDefinitionId defId in array2)
								{
									MyDefinitionManager.Static.TryGetCubeBlockDefinition(defId, out var blockDefinition);
									if (blockDefinition != null)
									{
										MyCubeBuilder.Static.CubeBuilderState.CurrentBlockDefinitionStages.Add(blockDefinition);
									}
								}
							}
						}
						base.CloseScreen();
						flag = true;
					}
				}
			}
			if (!flag)
			{
				base.HandleInput(receivedFocusInThisUpdate);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION1_MOD1))
			{
				MyCubeSize myCubeSize = m_currentSizeSelection;
				if (MyCubeBuilder.Static.IsActivated)
				{
					myCubeSize = MyCubeBuilder.Static.CubeBuilderState.CubeSizeMode;
				}
				MyCubeBuilder.Static.CubeBuilderState.SetCubeSize((myCubeSize == MyCubeSize.Large) ? MyCubeSize.Small : MyCubeSize.Large);
				MyGuiSoundManager.PlaySound(GuiSounds.MouseClick);
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.CANCEL_MOD1))
			{
				Cancel();
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION1) && m_currentBlock != null)
			{
				List<MyRadialMenuItem> items2 = m_data.CurrentSections[m_currentSection].Items;
				if (items2 != null && m_selectedButton >= 0 && m_selectedButton < items2.Count)
				{
					MyObjectBuilder_ToolbarItemDefinition myObjectBuilder_ToolbarItemDefinition2 = (m_blockDetail.GetSelectedVariant().UserData as MyGuiScreenToolbarConfigBase.GridItemUserData)?.ItemData() as MyObjectBuilder_ToolbarItemDefinition;
					if (myObjectBuilder_ToolbarItemDefinition2 != null && MyDefinitionManager.Static.TryGetDefinition<MyCubeBlockDefinition>(myObjectBuilder_ToolbarItemDefinition2.DefinitionId, out var definition2))
					{
						MyDLCs.MyDLC missingDLC;
						MyRadialMenuItemCubeBlock.EnabledState enabledState = MyRadialMenuItemCubeBlock.IsBlockEnabled(definition2, out missingDLC);
						if (enabledState == MyRadialMenuItemCubeBlock.EnabledState.Enabled)
						{
							if (MySession.Static.LocalCharacter.AddToBuildPlanner(definition2))
							{
								UpdateBuildPlanner();
							}
						}
						else if ((enabledState & MyRadialMenuItemCubeBlock.EnabledState.Research) != 0)
						{
							CloseScreen();
							OpenPrograssionTree();
						}
					}
				}
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACTION2) && m_currentBlock != null)
			{
				MyDLCs.MyDLC missingDLC2;
				MyRadialMenuItemCubeBlock.EnabledState enabledState2 = MyRadialMenuItemCubeBlock.IsBlockEnabled(m_currentBlock, out missingDLC2);
				if (enabledState2 == MyRadialMenuItemCubeBlock.EnabledState.Enabled)
				{
					MyCharacter localCharacter2 = MySession.Static.LocalCharacter;
					if (localCharacter2 != null && localCharacter2.BuildPlanner.Count > 0)
					{
						int num = -1;
						int num2 = 0;
						foreach (MyIdentity.BuildPlanItem item in MySession.Static.LocalCharacter.BuildPlanner)
						{
							if (item.BlockDefinition == m_currentBlock)
							{
								num = num2;
							}
							num2++;
						}
						if (num >= 0)
						{
							MySession.Static.LocalCharacter.RemoveAtBuildPlanner(num);
						}
						else
						{
							MySession.Static.LocalCharacter.RemoveAtBuildPlanner(MySession.Static.LocalCharacter.BuildPlanner.Count - 1);
						}
						UpdateBuildPlanner();
					}
				}
				else if ((enabledState2 & MyRadialMenuItemCubeBlock.EnabledState.DLC) != 0)
				{
					MyGameService.OpenDlcInShop(missingDLC2.AppId);
				}
			}
			if (MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.ACCEPT))
			{
				m_blockDetail.SelectNextVariant();
			}
		}

		private void OpenPrograssionTree()
		{
			MyGuiSandbox.AddScreen(MyGuiScreenGamePlay.ActiveGameplayScreen = MyGuiSandbox.CreateScreen(MyPerGameSettings.GUI.ToolbarConfigScreen, 0, MySession.Static.ControlledEntity as MyShipController, "ResearchPage", true, null));
		}

		protected override void GenerateIcons(int maxSize)
		{
			base.GenerateIcons(maxSize);
			m_missingRequirementIcons = new MyGuiControlImage[maxSize];
			for (int i = 0; i < maxSize; i++)
			{
				MyGuiControlImage myGuiControlImage = new MyGuiControlImage(null, new Vector2(40f) / MyGuiConstants.GUI_OPTIMAL_SIZE, null, null, null, null, MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_BOTTOM);
				AddControl(myGuiControlImage);
				m_missingRequirementIcons[i] = myGuiControlImage;
				MyGuiControlImage myGuiControlImage2 = m_icons[i];
				myGuiControlImage.Position = myGuiControlImage2.Position + myGuiControlImage2.Size / 2f + new Vector2(0.004f, 0.004f);
			}
		}

		protected override void SetIconTextures(MyRadialMenuSection selectedSection)
		{
			base.SetIconTextures(selectedSection);
			List<MyRadialMenuItem> items = selectedSection.Items;
			for (int i = 0; i < m_missingRequirementIcons.Length; i++)
			{
				MyGuiControlImage myGuiControlImage = m_missingRequirementIcons[i];
				string text = null;
				if (i < items.Count)
				{
					MyRadialMenuItemCubeBlock myRadialMenuItemCubeBlock = (MyRadialMenuItemCubeBlock)items[i];
					if (!myRadialMenuItemCubeBlock.Enabled())
					{
						MyCubeBlockDefinitionGroup[] blockGroups = myRadialMenuItemCubeBlock.BlockVariantGroup.BlockGroups;
						for (int j = 0; j < blockGroups.Length; j++)
						{
							MyDLCs.MyDLC missingDLC;
							MyRadialMenuItemCubeBlock.EnabledState enabledState = MyRadialMenuItemCubeBlock.IsBlockGroupEnabled(blockGroups[j], out missingDLC);
							if ((enabledState & MyRadialMenuItemCubeBlock.EnabledState.Research) != 0)
							{
								text = "Textures\\GUI\\Icons\\HUD 2017\\ProgressionTree.png";
								break;
							}
							if ((enabledState & MyRadialMenuItemCubeBlock.EnabledState.DLC) != 0 && text == null)
							{
								text = missingDLC.Icon;
							}
						}
					}
				}
				if (text == null)
				{
					myGuiControlImage.Visible = false;
					continue;
				}
				myGuiControlImage.Visible = true;
				myGuiControlImage.SetTexture(text);
			}
		}
	}
}
