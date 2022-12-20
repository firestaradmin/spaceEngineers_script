using System;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using System.Collections.ObjectModel;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Linq;
using System.Text;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.GameSystems;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Game;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	internal class MyTerminalControlPanel : MyTerminalController
	{
		public static readonly Vector4 RED_TEXT_COLOR = new Vector4(1f, 0.25f, 0.25f, 1f);

		private static readonly MyTerminalComparer m_nameComparer = new MyTerminalComparer();

		private static bool m_showAllTerminalBlocks = false;

		private static HashSet<Type> tmpBlockTypes = new HashSet<Type>();

		private static MyGuiHighlightTexture ICON_HIDE = new MyGuiHighlightTexture
		{
			Normal = "Textures\\GUI\\Controls\\button_hide.dds",
			Highlight = "Textures\\GUI\\Controls\\button_hide.dds",
			Focus = "Textures\\GUI\\Controls\\button_hide_focus.dds",
			SizePx = new Vector2(40f, 40f)
		};

		private static MyGuiHighlightTexture ICON_UNHIDE = new MyGuiHighlightTexture
		{
			Normal = "Textures\\GUI\\Controls\\button_unhide.dds",
			Highlight = "Textures\\GUI\\Controls\\button_unhide.dds",
			Focus = "Textures\\GUI\\Controls\\button_unhide_focus.dds",
			SizePx = new Vector2(40f, 40f)
		};

		private IMyGuiControlsParent m_controlsParent;

		private MyGuiControlListbox m_blockListbox;

		private MyGuiControlLabel m_blockNameLabel;

		private MyGuiControlBase blockControl;

		private List<MyBlockGroup> m_currentGroups = new List<MyBlockGroup>();

		private MyBlockGroup m_tmpGroup;

		private MyGuiControlSearchBox m_searchBox;

		private MyGuiControlTextbox m_groupName;

		private MyGuiControlButton m_groupSave;

		private MyGuiControlButton m_showAll;

		private MyGuiControlButton m_groupDelete;

		private List<MyBlockGroup> m_oldGroups = new List<MyBlockGroup>();

		private MyTerminalBlock m_originalBlock;

		private MyGridColorHelper m_colorHelper;

		private MyPlayer m_controller;

		private ulong m_last_showInTerminalChanged;

		private MyGuiScreenTerminal m_terminalScreen;

		private MyGuiControlBase m_blockControl
		{
			get
			{
				return blockControl;
			}
			set
			{
				if (blockControl != value)
				{
					if (m_terminalScreen != null && blockControl != null)
					{
						m_terminalScreen.DetachGroups(blockControl.Elements);
					}
					if (m_terminalScreen != null && value != null)
					{
						m_terminalScreen.AttachGroups(value.Elements);
					}
					blockControl = value;
				}
			}
		}

		private HashSet<MyTerminalBlock> CurrentBlocks => m_tmpGroup.Blocks;

		public MyGridTerminalSystem TerminalSystem { get; private set; }

		public void Init(IMyGuiControlsParent controlsParent, MyPlayer controller, MyCubeGrid grid, MyTerminalBlock currentBlock, MyGridColorHelper colorHelper)
		{
			m_controlsParent = controlsParent;
			m_controller = controller;
			m_colorHelper = colorHelper;
			if (grid == null)
			{
				foreach (MyGuiControlBase control2 in controlsParent.Controls)
				{
					control2.Visible = false;
				}
				MyGuiControlLabel control = MyGuiScreenTerminal.CreateErrorLabel(MySpaceTexts.ScreenTerminalError_ShipNotConnected, "ErrorMessage");
				controlsParent.Controls.Add(control);
				return;
			}
			TerminalSystem = grid.GridSystems.TerminalSystem;
			m_tmpGroup = new MyBlockGroup();
			m_terminalScreen.GetGroupInjectableControls(ref m_blockNameLabel, ref m_groupName, ref m_groupSave, ref m_groupDelete);
			m_searchBox = (MyGuiControlSearchBox)m_controlsParent.Controls.GetControlByName("FunctionalBlockSearch");
			m_searchBox.OnTextChanged += blockSearch_TextChanged;
			m_blockListbox = (MyGuiControlListbox)m_controlsParent.Controls.GetControlByName("FunctionalBlockListbox");
<<<<<<< HEAD
			m_blockListbox.IsAutoEllipsisEnabled = true;
			m_blockListbox.IsAutoScaleEnabled = true;
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_blockNameLabel.Text = "";
			m_groupName.TextChanged += m_groupName_TextChanged;
			m_groupName.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroup));
			m_groupName.ShowTooltipWhenDisabled = true;
			m_showAll = (MyGuiControlButton)m_controlsParent.Controls.GetControlByName("ShowAll");
			m_showAll.Selected = m_showAllTerminalBlocks;
			m_showAll.ButtonClicked += showAll_Clicked;
			m_showAll.SetToolTip(MySpaceTexts.Terminal_ShowAllInTerminal);
			m_showAll.IconRotation = 0f;
			m_showAll.Icon = ICON_UNHIDE;
			m_showAll.Size = new Vector2(0f, 0f);
			m_groupSave.TextEnum = MySpaceTexts.TerminalButton_GroupSave;
			m_groupSave.TextAlignment = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER;
			m_groupSave.VisualStyle = MyGuiControlButtonStyleEnum.Rectangular;
			m_groupSave.ButtonClicked += groupSave_ButtonClicked;
			m_groupSave.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupSave));
			m_groupSave.ShowTooltipWhenDisabled = true;
			m_groupDelete.ButtonClicked += groupDelete_ButtonClicked;
			m_groupDelete.ShowTooltipWhenDisabled = true;
			m_groupDelete.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupDeleteDisabled));
			m_groupDelete.Enabled = false;
			m_blockListbox.ItemsSelected += blockListbox_ItemSelected;
			m_originalBlock = currentBlock;
			MyTerminalBlock[] selectedBlocks = null;
			if (m_originalBlock != null)
			{
				selectedBlocks = new MyTerminalBlock[1] { m_originalBlock };
			}
			RefreshBlockList(selectedBlocks);
			TerminalSystem.BlockAdded += TerminalSystem_BlockAdded;
			TerminalSystem.BlockRemoved += TerminalSystem_BlockRemoved;
			TerminalSystem.BlockManipulationFinished += TerminalSystem_BlockManipulationFinished;
			TerminalSystem.GroupAdded += TerminalSystem_GroupAdded;
			TerminalSystem.GroupRemoved += TerminalSystem_GroupRemoved;
			blockSearch_TextChanged(m_searchBox.SearchText);
			m_blockListbox.ScrollToFirstSelection();
		}

		private void m_groupName_TextChanged(MyGuiControlTextbox obj)
		{
			if (string.IsNullOrEmpty(obj.Text) || CurrentBlocks.get_Count() == 0)
			{
				m_groupSave.Enabled = false;
				m_groupSave.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupSaveDisabled));
			}
			else
			{
				m_groupSave.Enabled = true;
				m_groupSave.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupSave));
			}
		}

		private void TerminalSystem_GroupRemoved(MyBlockGroup group)
		{
			if (m_blockListbox == null)
			{
				return;
			}
			foreach (MyGuiControlListbox.Item item in m_blockListbox.Items)
			{
				if (item.UserData == group)
				{
<<<<<<< HEAD
					m_blockListbox.Items.Remove(item);
=======
					((Collection<MyGuiControlListbox.Item>)(object)m_blockListbox.Items).Remove(item);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					break;
				}
			}
		}

		private void TerminalSystem_GroupAdded(MyBlockGroup group)
		{
			if (m_blockListbox != null)
			{
				AddGroupToList(group, 0);
			}
		}

		private void groupDelete_ButtonClicked(MyGuiControlButton obj)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
			foreach (MyBlockGroup currentGroup in m_currentGroups)
			{
				Enumerator<MyTerminalBlock> enumerator2 = currentGroup.Blocks.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						if (!enumerator2.get_Current().HasLocalPlayerAccess())
						{
							flag = true;
							break;
						}
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
			}
			if (flag)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextCannotDeleteGroup)));
				return;
			}
			while (m_currentGroups.Count > 0)
			{
				TerminalSystem.RemoveGroup(m_currentGroups[0], fireEvent: true);
			}
		}

		private void showAll_Clicked(MyGuiControlButton obj)
		{
			m_showAllTerminalBlocks = !m_showAllTerminalBlocks;
			m_showAll.Selected = m_showAllTerminalBlocks;
			List<MyGuiControlListbox.Item> selectedItems = m_blockListbox.SelectedItems;
			MyTerminalBlock[] array = new MyTerminalBlock[selectedItems.Count];
			for (int i = 0; i < selectedItems.Count; i++)
			{
				if (selectedItems[i].UserData is MyTerminalBlock)
				{
					array[i] = (MyTerminalBlock)selectedItems[i].UserData;
				}
			}
			ClearBlockList();
			PopulateBlockList(array);
			m_blockListbox.ScrollToolbarToTop();
			blockSearch_TextChanged(m_searchBox.SearchText);
			UpdateShowAllTextures();
		}

		private void UpdateShowAllTextures()
		{
			if (m_showAllTerminalBlocks)
			{
				m_showAll.Icon = ICON_HIDE;
			}
			else
			{
				m_showAll.Icon = ICON_UNHIDE;
			}
		}

		private void groupSave_ButtonClicked(MyGuiControlButton obj)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
			Enumerator<MyTerminalBlock> enumerator = m_tmpGroup.Blocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.get_Current().HasLocalPlayerAccess())
					{
						flag = true;
						break;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			if (flag)
			{
				MyGuiSandbox.AddScreen(MyGuiSandbox.CreateMessageBox(MyMessageBoxStyleEnum.Error, MyMessageBoxButtonsType.OK, messageCaption: MyTexts.Get(MyCommonTexts.MessageBoxCaptionError), messageText: MyTexts.Get(MyCommonTexts.MessageBoxTextCannotCreateGroup)));
			}
			else if (m_groupName.Text != "")
			{
				m_currentGroups.Clear();
				m_tmpGroup.Name.Clear().Append(m_groupName.Text);
				m_tmpGroup = TerminalSystem.AddUpdateGroup(m_tmpGroup, fireEvent: true, modify: true);
				m_currentGroups.Add(m_tmpGroup);
				m_tmpGroup = new MyBlockGroup();
				CurrentBlocks.UnionWith((IEnumerable<MyTerminalBlock>)m_currentGroups[0].Blocks);
				SelectBlocks();
			}
		}

		internal void SetTerminalScreen(MyGuiScreenTerminal terminalScreen)
		{
			m_terminalScreen = terminalScreen;
		}

		private void blockSearch_TextChanged(string text)
		{
			if (m_blockListbox == null)
<<<<<<< HEAD
			{
				return;
			}
			if (text != "")
			{
=======
			{
				return;
			}
			if (text != "")
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				string[] array = text.Split(new char[1] { ' ' });
				foreach (MyGuiControlListbox.Item item in m_blockListbox.Items)
				{
					bool flag = true;
					if (item.UserData is MyTerminalBlock)
					{
						flag = ((MyTerminalBlock)item.UserData).ShowInTerminal || m_showAllTerminalBlocks || item.UserData == m_originalBlock;
					}
					if (!flag)
					{
						continue;
					}
					string text2 = item.Text.ToString().ToLower();
					string[] array2 = array;
					foreach (string text3 in array2)
					{
						if (!text2.Contains(text3.ToLower()))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						item.Visible = true;
					}
					else
					{
						item.Visible = false;
					}
				}
			}
			else
			{
				foreach (MyGuiControlListbox.Item item2 in m_blockListbox.Items)
				{
					if (item2.UserData is MyTerminalBlock)
					{
						MyTerminalBlock myTerminalBlock = (MyTerminalBlock)item2.UserData;
						item2.Visible = myTerminalBlock.ShowInTerminal || m_showAllTerminalBlocks || myTerminalBlock == m_originalBlock;
					}
					else
					{
						item2.Visible = true;
					}
				}
			}
			m_blockListbox.ScrollToolbarToTop();
		}

		private void TerminalSystem_BlockAdded(MyTerminalBlock obj)
		{
			AddBlockToList(obj);
		}

		private void TerminalSystem_BlockRemoved(MyTerminalBlock obj)
		{
			obj.CustomNameChanged -= block_CustomNameChanged;
			obj.PropertiesChanged -= block_CustomNameChanged;
			if (m_blockListbox != null && (obj.ShowInTerminal || m_showAllTerminalBlocks))
			{
				m_blockListbox.Remove((MyGuiControlListbox.Item item) => item.UserData == obj);
			}
		}

		private void TerminalSystem_BlockManipulationFinished()
		{
			blockSearch_TextChanged(m_searchBox.SearchText);
		}

		public void Close()
		{
			if (TerminalSystem != null)
			{
				if (m_blockListbox != null)
				{
					ClearBlockList();
					m_blockListbox.ItemsSelected -= blockListbox_ItemSelected;
				}
				TerminalSystem.BlockAdded -= TerminalSystem_BlockAdded;
				TerminalSystem.BlockRemoved -= TerminalSystem_BlockRemoved;
				TerminalSystem.BlockManipulationFinished -= TerminalSystem_BlockManipulationFinished;
				TerminalSystem.GroupAdded -= TerminalSystem_GroupAdded;
				TerminalSystem.GroupRemoved -= TerminalSystem_GroupRemoved;
			}
			if (m_tmpGroup != null)
			{
				m_tmpGroup.Blocks.Clear();
			}
			if (m_showAll != null)
			{
				m_showAll.ButtonClicked -= showAll_Clicked;
			}
			m_controlsParent = null;
			m_blockListbox = null;
			m_blockNameLabel = null;
			TerminalSystem = null;
			m_currentGroups.Clear();
		}

		public void RefreshBlockList(MyTerminalBlock[] selectedBlocks = null)
		{
			if (m_blockListbox != null)
			{
				ClearBlockList();
				PopulateBlockList(selectedBlocks);
			}
		}

		public void ClearBlockList()
		{
			if (m_blockListbox == null)
			{
				return;
			}
			foreach (MyGuiControlListbox.Item item in m_blockListbox.Items)
			{
				if (item.UserData is MyTerminalBlock)
				{
					MyTerminalBlock obj = (MyTerminalBlock)item.UserData;
					obj.CustomNameChanged -= block_CustomNameChanged;
					obj.PropertiesChanged -= block_CustomNameChanged;
					obj.ShowInTerminalChanged -= block_ShowInTerminalChanged_Delayed;
				}
			}
<<<<<<< HEAD
			m_blockListbox.Items.Clear();
=======
			((Collection<MyGuiControlListbox.Item>)(object)m_blockListbox.Items).Clear();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void PopulateBlockList(MyTerminalBlock[] selectedBlocks = null)
		{
			if (TerminalSystem == null)
			{
				return;
			}
			if (TerminalSystem.BlockGroups == null)
			{
				MySandboxGame.Log.WriteLine("m_terminalSystem.BlockGroups is null");
			}
			if (!TerminalSystem.Blocks.IsValid)
			{
				MySandboxGame.Log.WriteLine("m_terminalSystem.Blocks.IsValid is false");
			}
			if (CurrentBlocks == null)
			{
				MySandboxGame.Log.WriteLine("CurrentBlocks is null");
			}
			if (m_blockListbox == null)
			{
				MySandboxGame.Log.WriteLine("m_blockListbox is null");
			}
			MyBlockGroup[] array = TerminalSystem.BlockGroups.ToArray();
			Array.Sort(array, MyTerminalComparer.Static);
			MyBlockGroup[] array2 = array;
			foreach (MyBlockGroup group in array2)
			{
				AddGroupToList(group);
			}
			MyTerminalBlock[] array3 = TerminalSystem.Blocks.ToArray();
			Array.Sort(array3, MyTerminalComparer.Static);
			m_blockListbox.SelectedItems.Clear();
			m_blockListbox.IsInBulkInsert = true;
			MyTerminalBlock[] array4 = array3;
			foreach (MyTerminalBlock myTerminalBlock in array4)
			{
				AddBlockToList(myTerminalBlock, myTerminalBlock == m_originalBlock || myTerminalBlock.ShowInTerminal || m_showAllTerminalBlocks);
			}
			m_blockListbox.IsInBulkInsert = false;
			if (selectedBlocks == null)
			{
				if (CurrentBlocks.get_Count() > 0)
				{
					SelectBlocks();
					return;
				}
				foreach (MyGuiControlListbox.Item item in m_blockListbox.Items)
				{
					if (item.UserData is MyTerminalBlock)
					{
						SelectBlocks(new MyTerminalBlock[1] { (MyTerminalBlock)item.UserData });
						break;
					}
				}
			}
			else
			{
				SelectBlocks(selectedBlocks);
			}
		}

		private bool IsGeneric(MyBlockGroup group)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				bool flag = true;
				Enumerator<MyTerminalBlock> enumerator = group.Blocks.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						MyTerminalBlock current = enumerator.get_Current();
						flag = flag && current is MyFunctionalBlock;
						tmpBlockTypes.Add(current.GetType());
					}
				}
				finally
				{
<<<<<<< HEAD
					flag = flag && block is MyFunctionalBlock;
					tmpBlockTypes.Add(block.GetType());
=======
					((IDisposable)enumerator).Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				if (tmpBlockTypes.get_Count() == 1)
				{
					return false;
				}
				return true;
			}
			finally
			{
				tmpBlockTypes.Clear();
			}
		}

		private void AddGroupToList(MyBlockGroup group, int? position = null)
		{
			foreach (MyGuiControlListbox.Item item2 in m_blockListbox.Items)
			{
				MyBlockGroup myBlockGroup = item2.UserData as MyBlockGroup;
				if (myBlockGroup != null && myBlockGroup.Name.CompareTo(group.Name) == 0)
				{
					((Collection<MyGuiControlListbox.Item>)(object)m_blockListbox.Items).Remove(item2);
					break;
				}
			}
			MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(null, group.Name.ToString(), GetIconForGroup(group), group);
			item.Text.Clear().Append("*").AppendStringBuilder(group.Name)
				.Append("*");
			m_blockListbox.Add(item, position);
		}

		private string GetIconForBlock(MyTerminalBlock block)
		{
			if (block.BlockDefinition == null || block.BlockDefinition.Icons.IsNullOrEmpty())
			{
				return MyGuiConstants.TEXTURE_ICON_FAKE.Texture;
			}
			return block.BlockDefinition.Icons[0];
		}

		private string GetIconForGroup(MyBlockGroup group)
		{
			if (group == null || IsGeneric(group))
			{
				return MyGuiConstants.TEXTURE_TERMINAL_GROUP;
			}
			MyTerminalBlock myTerminalBlock = Enumerable.First<MyTerminalBlock>((IEnumerable<MyTerminalBlock>)group.Blocks);
			if (myTerminalBlock.BlockDefinition == null || myTerminalBlock.BlockDefinition.Icons.IsNullOrEmpty())
			{
				return MyGuiConstants.TEXTURE_TERMINAL_GROUP;
			}
			return myTerminalBlock.BlockDefinition.Icons[0];
		}

		private MyGuiControlListbox.Item AddBlockToList(MyTerminalBlock block, bool? visibility = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			block.GetTerminalName(stringBuilder);
			MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(null, stringBuilder.ToString(), GetIconForBlock(block), block);
			UpdateItemAppearance(block, item);
			block.CustomNameChanged += block_CustomNameChanged;
			block.PropertiesChanged += block_CustomNameChanged;
			block.ShowInTerminalChanged += block_ShowInTerminalChanged_Delayed;
			if (visibility.HasValue)
			{
				item.Visible = visibility.Value;
			}
			m_blockListbox.Add(item);
			return item;
		}

		private void UpdateItemAppearance(MyTerminalBlock block, MyGuiControlListbox.Item item)
		{
			item.Text.Clear();
			block.GetTerminalName(item.Text);
			MyTerminalBlock.AccessRightsResult accessRightsResult;
			if (!block.IsFunctional)
			{
				item.ColorMask = RED_TEXT_COLOR;
				item.Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Terminal_BlockIncomplete));
<<<<<<< HEAD
			}
			else if (m_controller != null && m_controller.Identity != null && (accessRightsResult = block.HasPlayerAccessReason(m_controller.Identity.IdentityId)) != 0)
=======
				return;
			}
			MyTerminalBlock.AccessRightsResult accessRightsResult;
			if (m_controller != null && m_controller.Identity != null && (accessRightsResult = block.HasPlayerAccessReason(m_controller.Identity.IdentityId)) != 0)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				item.ColorMask = RED_TEXT_COLOR;
				if (accessRightsResult == MyTerminalBlock.AccessRightsResult.MissingDLC)
				{
					if (block.BlockDefinition == null || block.BlockDefinition.DLCs == null)
					{
						return;
					}
					string[] dLCs = block.BlockDefinition.DLCs;
					for (int i = 0; i < dLCs.Length; i++)
					{
						if (MyDLCs.TryGetDLC(dLCs[i], out var _))
						{
							item.Text.Append(" (").Append((object)MyTexts.Get(MyCommonTexts.RequiresAnyDlc)).Append(")");
						}
					}
				}
				else
				{
					item.Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Terminal_BlockAccessDenied));
				}
<<<<<<< HEAD
=======
				return;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			else if (!block.ShowInTerminal)
			{
				Color? gridColor = m_colorHelper.GetGridColor(block.CubeGrid);
				item.ColorMask = 0.6f * (gridColor.HasValue ? gridColor.Value.ToVector4() : Vector4.One);
				item.FontOverride = null;
			}
<<<<<<< HEAD
			else if (block.IDModule == null && !block.HasLocalPlayerAccessToBlockWithoutOwnership())
=======
			if (m_controller != null && m_controller.Identity != null && block.IDModule == null && block.CubeGrid != null)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				item.ColorMask = RED_TEXT_COLOR;
				item.Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Terminal_BlockAccessDenied));
			}
			else
			{
				Color? gridColor2 = m_colorHelper.GetGridColor(block.CubeGrid);
				if (gridColor2.HasValue)
				{
<<<<<<< HEAD
					item.ColorMask = gridColor2.Value.ToVector4();
=======
					List<long> smallOwners = block.CubeGrid.SmallOwners;
					if ((smallOwners == null || smallOwners.Count != 0) && !block.HasLocalPlayerAdminUseTerminals() && !block.CubeGrid.SmallOwners.Contains(m_controller.Identity.IdentityId))
					{
						item.ColorMask = RED_TEXT_COLOR;
						item.Text.AppendStringBuilder(MyTexts.Get(MySpaceTexts.Terminal_BlockAccessDenied));
						return;
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				item.FontOverride = null;
			}
<<<<<<< HEAD
=======
			Color? gridColor2 = m_colorHelper.GetGridColor(block.CubeGrid);
			if (gridColor2.HasValue)
			{
				item.ColorMask = gridColor2.Value.ToVector4();
			}
			item.FontOverride = null;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void block_CustomNameChanged(MyTerminalBlock obj)
		{
			if (m_blockListbox == null)
			{
				return;
			}
			foreach (MyGuiControlListbox.Item item in m_blockListbox.Items)
			{
				if (item.UserData == obj)
				{
					UpdateItemAppearance(obj, item);
					break;
				}
			}
<<<<<<< HEAD
			if (CurrentBlocks.Count > 0 && CurrentBlocks.FirstElement() == obj)
=======
			if (CurrentBlocks.get_Count() > 0 && CurrentBlocks.FirstElement<MyTerminalBlock>() == obj)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_blockNameLabel.Text = obj.CustomName.ToString();
			}
		}

		public void SelectBlocks(MyTerminalBlock[] blocks)
		{
			m_tmpGroup.Blocks.Clear();
			m_tmpGroup.Blocks.UnionWith((IEnumerable<MyTerminalBlock>)blocks);
			m_currentGroups.Clear();
			CurrentBlocks.Clear();
			foreach (MyTerminalBlock myTerminalBlock in blocks)
			{
				if (myTerminalBlock != null)
				{
					CurrentBlocks.Add(myTerminalBlock);
				}
			}
			SelectBlocks();
		}

		private void SelectBlocks()
		{
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			if (m_blockControl != null)
			{
				m_controlsParent.Controls.Remove(m_blockControl);
				m_blockControl = null;
			}
			m_blockNameLabel.Text = "";
			m_groupName.Text = "";
			if (m_currentGroups.Count == 1)
			{
				m_blockNameLabel.Text = m_currentGroups[0].Name.ToString();
				m_groupName.Text = m_blockNameLabel.Text;
			}
			if (CurrentBlocks.get_Count() > 0)
			{
				if (CurrentBlocks.get_Count() == 1)
				{
					m_blockNameLabel.Text = CurrentBlocks.FirstElement<MyTerminalBlock>().CustomName.ToString();
				}
				m_blockControl = new MyGuiControlGenericFunctionalBlock(Enumerable.ToArray<MyTerminalBlock>((IEnumerable<MyTerminalBlock>)CurrentBlocks));
				m_controlsParent.Controls.Add(m_blockControl);
				m_blockControl.Size = new Vector2(0.595f, 0.64f);
				m_blockControl.OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP;
				m_blockControl.Position = new Vector2(-0.1415f, -0.3f);
			}
			UpdateGroupControl();
			m_blockListbox.SelectedItems.Clear();
			Enumerator<MyTerminalBlock> enumerator = CurrentBlocks.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					MyTerminalBlock current = enumerator.get_Current();
					foreach (MyGuiControlListbox.Item item in m_blockListbox.Items)
					{
						if (item.UserData == current)
						{
							m_blockListbox.SelectedItems.Add(item);
							break;
						}
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			foreach (MyBlockGroup currentGroup in m_currentGroups)
			{
				foreach (MyGuiControlListbox.Item item2 in m_blockListbox.Items)
				{
					if (item2.UserData == currentGroup)
					{
						m_blockListbox.SelectedItems.Add(item2);
						break;
					}
				}
			}
		}

		public void SelectAllBlocks()
		{
			if (m_blockListbox != null)
			{
				m_blockListbox.SelectAllVisible();
			}
		}

		private void UpdateGroupControl()
		{
			if (m_currentGroups.Count > 0)
			{
				m_groupDelete.Enabled = true;
				m_groupDelete.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupDelete));
			}
			else
			{
				m_groupDelete.Enabled = false;
				m_groupDelete.SetTooltip(MyTexts.GetString(MySpaceTexts.ControlScreen_TerminalBlockGroupDeleteDisabled));
			}
		}

		public void UpdateCubeBlock(MyTerminalBlock block)
		{
			if (block != null)
			{
				if (TerminalSystem != null)
				{
					TerminalSystem.BlockAdded -= TerminalSystem_BlockAdded;
					TerminalSystem.BlockRemoved -= TerminalSystem_BlockRemoved;
					TerminalSystem.BlockManipulationFinished -= TerminalSystem_BlockManipulationFinished;
					TerminalSystem.GroupAdded -= TerminalSystem_GroupAdded;
					TerminalSystem.GroupRemoved -= TerminalSystem_GroupRemoved;
				}
				MyCubeGrid cubeGrid = block.CubeGrid;
				TerminalSystem = cubeGrid.GridSystems.TerminalSystem;
				m_tmpGroup = new MyBlockGroup();
				TerminalSystem.BlockAdded += TerminalSystem_BlockAdded;
				TerminalSystem.BlockRemoved += TerminalSystem_BlockRemoved;
				TerminalSystem.BlockManipulationFinished += TerminalSystem_BlockManipulationFinished;
				TerminalSystem.GroupAdded += TerminalSystem_GroupAdded;
				TerminalSystem.GroupRemoved += TerminalSystem_GroupRemoved;
				SelectBlocks(new MyTerminalBlock[1] { block });
			}
		}

		private void blockListbox_ItemSelected(MyGuiControlListbox sender)
		{
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			m_oldGroups.Clear();
			m_oldGroups.AddRange(m_currentGroups);
			m_currentGroups.Clear();
			m_tmpGroup.Blocks.Clear();
			foreach (MyGuiControlListbox.Item selectedItem in sender.SelectedItems)
			{
				if (selectedItem.UserData is MyBlockGroup)
				{
					m_currentGroups.Add((MyBlockGroup)selectedItem.UserData);
				}
				else if (selectedItem.UserData is MyTerminalBlock)
				{
					CurrentBlocks.Add(selectedItem.UserData as MyTerminalBlock);
				}
			}
			for (int i = 0; i < m_currentGroups.Count; i++)
			{
<<<<<<< HEAD
				if (m_oldGroups.Contains(m_currentGroups[i]) && m_currentGroups[i].Blocks.Intersect(CurrentBlocks).Count() != 0)
				{
					continue;
				}
				foreach (MyTerminalBlock block in m_currentGroups[i].Blocks)
				{
					if (!CurrentBlocks.Contains(block))
					{
						CurrentBlocks.Add(block);
=======
				if (m_oldGroups.Contains(m_currentGroups[i]) && Enumerable.Count<MyTerminalBlock>(Enumerable.Intersect<MyTerminalBlock>((IEnumerable<MyTerminalBlock>)m_currentGroups[i].Blocks, (IEnumerable<MyTerminalBlock>)CurrentBlocks)) != 0)
				{
					continue;
				}
				Enumerator<MyTerminalBlock> enumerator2 = m_currentGroups[i].Blocks.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						MyTerminalBlock current2 = enumerator2.get_Current();
						if (!CurrentBlocks.Contains(current2))
						{
							CurrentBlocks.Add(current2);
						}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
				finally
				{
					((IDisposable)enumerator2).Dispose();
				}
			}
			SelectBlocks();
		}

		private void block_ShowInTerminalChanged_Delayed(MyTerminalBlock obj)
		{
			if (m_last_showInTerminalChanged == MySandboxGame.Static.SimulationFrameCounter)
			{
				return;
			}
			m_last_showInTerminalChanged = MySandboxGame.Static.SimulationFrameCounter;
			MySandboxGame.Static.Invoke(delegate
			{
				MyTerminalBlock[] array = null;
				if (m_blockListbox != null)
				{
					List<MyGuiControlListbox.Item> selectedItems = m_blockListbox.SelectedItems;
					array = new MyTerminalBlock[selectedItems.Count];
					for (int i = 0; i < selectedItems.Count; i++)
					{
						MyTerminalBlock myTerminalBlock;
						if ((myTerminalBlock = selectedItems[i].UserData as MyTerminalBlock) != null)
						{
							array[i] = myTerminalBlock;
						}
					}
				}
				ClearBlockList();
				PopulateBlockList(array);
				if (m_blockListbox != null)
				{
					m_blockListbox.ScrollToolbarToTop();
				}
				blockSearch_TextChanged(m_searchBox.SearchText);
			}, "ShowInTerminalChanged");
		}

		public override void HandleInput()
		{
			base.HandleInput();
			if (m_blockListbox != null && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_X))
			{
				List<MyGuiControlListbox.Item> selectedItems = m_blockListbox.SelectedItems;
				for (int i = 0; i < selectedItems.Count; i++)
				{
					MyFunctionalBlock myFunctionalBlock;
					if ((myFunctionalBlock = selectedItems[i].UserData as MyFunctionalBlock) != null)
					{
						myFunctionalBlock.Enabled = !myFunctionalBlock.Enabled;
					}
				}
			}
			if (m_blockListbox != null && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.BUTTON_Y))
			{
				List<MyGuiControlListbox.Item> selectedItems2 = m_blockListbox.SelectedItems;
				for (int j = 0; j < selectedItems2.Count; j++)
				{
					MyTerminalBlock myTerminalBlock;
					if ((myTerminalBlock = selectedItems2[j].UserData as MyTerminalBlock) != null)
					{
						myTerminalBlock.ShowInTerminal = !myTerminalBlock.ShowInTerminal;
					}
				}
			}
			if (m_blockListbox != null && MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.RIGHT_STICK_BUTTON))
			{
				List<MyGuiControlListbox.Item> selectedItems3 = m_blockListbox.SelectedItems;
				for (int k = 0; k < selectedItems3.Count; k++)
				{
					MyTerminalBlock myTerminalBlock2;
					if ((myTerminalBlock2 = selectedItems3[k].UserData as MyTerminalBlock) != null)
					{
						myTerminalBlock2.ShowOnHUD = !myTerminalBlock2.ShowOnHUD;
					}
				}
			}
			if (m_blockListbox == null || !MyControllerHelper.IsControl(MyControllerHelper.CX_GUI, MyControlsGUI.LEFT_STICK_BUTTON))
			{
				return;
			}
			List<MyGuiControlListbox.Item> selectedItems4 = m_blockListbox.SelectedItems;
			for (int l = 0; l < selectedItems4.Count; l++)
			{
				MyTerminalBlock myTerminalBlock3;
				if ((myTerminalBlock3 = selectedItems4[l].UserData as MyTerminalBlock) != null)
				{
					myTerminalBlock3.ShowInToolbarConfig = !myTerminalBlock3.ShowInToolbarConfig;
				}
			}
		}
	}
}
