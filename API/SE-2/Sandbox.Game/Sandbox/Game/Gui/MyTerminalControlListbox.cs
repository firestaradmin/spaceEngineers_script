using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Sandbox.Game.Entities.Cube;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage;
using VRage.Game;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
	public class MyTerminalControlListbox<TBlock> : MyTerminalControl<TBlock>, ITerminalControlSync, IMyTerminalControlTitleTooltip, IMyTerminalControlListbox, IMyTerminalControl where TBlock : MyTerminalBlock
	{
		public delegate void ListContentDelegate(TBlock block, ICollection<MyGuiControlListbox.Item> listBoxContent, ICollection<MyGuiControlListbox.Item> listBoxSelectedItems, ICollection<MyGuiControlListbox.Item> lastFocused);

		public delegate void SelectItemDelegate(TBlock block, List<MyGuiControlListbox.Item> items);

		public MyStringId Title;

		public MyStringId Tooltip;

		public ListContentDelegate ListContent;

		public SelectItemDelegate ItemSelected;

		public SelectItemDelegate ItemDoubleClicked;

		private MyGuiControlListbox m_listbox;

		private bool m_enableMultiSelect;

		private int m_visibleRowsCount = 8;

		private bool m_keepScrolling = true;

		private bool KeepScrolling
		{
			get
			{
				return m_keepScrolling;
			}
			set
			{
				m_keepScrolling = value;
			}
		}

		/// <summary>
		/// Implements IMyTerminalControlListBox for Mods
		/// </summary>
		MyStringId IMyTerminalControlTitleTooltip.Title
		{
			get
			{
				return Title;
			}
			set
			{
				Title = value;
			}
		}

		MyStringId IMyTerminalControlTitleTooltip.Tooltip
		{
			get
			{
				return Tooltip;
			}
			set
			{
				Tooltip = value;
			}
		}

		bool IMyTerminalControlListbox.Multiselect
		{
			get
			{
				return m_enableMultiSelect;
			}
			set
			{
				m_enableMultiSelect = value;
			}
		}

		int IMyTerminalControlListbox.VisibleRowsCount
		{
			get
			{
				return m_visibleRowsCount;
			}
			set
			{
				m_visibleRowsCount = value;
			}
		}

		Action<IMyTerminalBlock, List<MyTerminalControlListBoxItem>, List<MyTerminalControlListBoxItem>> IMyTerminalControlListbox.ListContent
		{
			set
			{
				ListContent = delegate(TBlock block, ICollection<MyGuiControlListbox.Item> contentList, ICollection<MyGuiControlListbox.Item> selectedList, ICollection<MyGuiControlListbox.Item> focusedItem)
				{
					List<MyTerminalControlListBoxItem> list = new List<MyTerminalControlListBoxItem>();
					List<MyTerminalControlListBoxItem> list2 = new List<MyTerminalControlListBoxItem>();
					value(block, list, list2);
					foreach (MyTerminalControlListBoxItem item2 in list)
					{
						MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(item2.Text.ToString()), item2.Tooltip.ToString(), null, item2.UserData);
						contentList.Add(item);
						if (list2.Contains(item2))
						{
							selectedList.Add(item);
						}
					}
				};
			}
		}

		Action<IMyTerminalBlock, List<MyTerminalControlListBoxItem>> IMyTerminalControlListbox.ItemSelected
		{
			set
			{
				ItemSelected = delegate(TBlock block, List<MyGuiControlListbox.Item> selectedList)
				{
					List<MyTerminalControlListBoxItem> list = new List<MyTerminalControlListBoxItem>();
					foreach (MyGuiControlListbox.Item selected in selectedList)
					{
<<<<<<< HEAD
						string str = ((selected.ToolTip != null && selected.ToolTip.ToolTips.Count > 0) ? selected.ToolTip.ToolTips.First().ToString() : null);
=======
						string str = ((selected.ToolTip != null && ((Collection<MyColoredText>)(object)selected.ToolTip.ToolTips).Count > 0) ? Enumerable.First<MyColoredText>((IEnumerable<MyColoredText>)selected.ToolTip.ToolTips).ToString() : null);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
						MyTerminalControlListBoxItem item = new MyTerminalControlListBoxItem(MyStringId.GetOrCompute(selected.Text.ToString()), MyStringId.GetOrCompute(str), selected.UserData);
						list.Add(item);
					}
					value(block, list);
				};
			}
		}

		public MyTerminalControlListbox(string id, MyStringId title, MyStringId tooltip, bool multiSelect = false, int visibleRowsCount = 8)
			: base(id)
		{
			Title = title;
			Tooltip = tooltip;
			m_enableMultiSelect = multiSelect;
			m_visibleRowsCount = visibleRowsCount;
		}

		protected override MyGuiControlBase CreateGui()
		{
			m_listbox = new MyGuiControlListbox
			{
				VisualStyle = MyGuiControlListboxStyleEnum.Terminal,
				OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_CENTER,
				VisibleRowsCount = m_visibleRowsCount,
				MultiSelect = m_enableMultiSelect
			};
			m_listbox.IsAutoScaleEnabled = true;
			m_listbox.IsAutoEllipsisEnabled = true;
			m_listbox.ItemsSelected += OnItemsSelected;
			m_listbox.ItemDoubleClicked += OnItemDoubleClicked;
			return new MyGuiControlBlockProperty(MyTexts.GetString(Title), MyTexts.GetString(Tooltip), m_listbox);
		}

		private void OnItemsSelected(MyGuiControlListbox obj)
		{
			if (ItemSelected == null || obj.SelectedItems.Count <= 0)
			{
				return;
			}
			foreach (TBlock targetBlock in base.TargetBlocks)
			{
<<<<<<< HEAD
				if (targetBlock.CanLocalPlayerChangeValue())
				{
					ItemSelected(targetBlock, obj.SelectedItems);
				}
=======
				ItemSelected(targetBlock, obj.SelectedItems);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private void OnItemDoubleClicked(MyGuiControlListbox obj)
		{
			if (ItemDoubleClicked == null || obj.SelectedItems.Count <= 0)
			{
				return;
			}
			foreach (TBlock targetBlock in base.TargetBlocks)
			{
<<<<<<< HEAD
				if (targetBlock.CanLocalPlayerChangeValue())
				{
					ItemDoubleClicked(targetBlock, obj.SelectedItems);
				}
=======
				ItemDoubleClicked(targetBlock, obj.SelectedItems);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		protected override void OnUpdateVisual()
		{
			base.OnUpdateVisual();
			TBlock firstBlock = base.FirstBlock;
			if (firstBlock == null)
			{
				return;
			}
			float scrollPosition = m_listbox.GetScrollPosition();
<<<<<<< HEAD
			m_listbox.Items.Clear();
=======
			((Collection<MyGuiControlListbox.Item>)(object)m_listbox.Items).Clear();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_listbox.SelectedItems.Clear();
			if (ListContent != null)
			{
				List<MyGuiControlListbox.Item> list = new List<MyGuiControlListbox.Item>();
<<<<<<< HEAD
				ListContent(firstBlock, m_listbox.Items, m_listbox.SelectedItems, list);
=======
				ListContent(firstBlock, (ICollection<MyGuiControlListbox.Item>)m_listbox.Items, m_listbox.SelectedItems, list);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				if (list.Count > 0)
				{
					m_listbox.FocusedItem = list[0];
				}
			}
<<<<<<< HEAD
			if (scrollPosition <= (float)(m_listbox.Items.Count - m_listbox.VisibleRowsCount) + 1f)
=======
			if (scrollPosition <= (float)(((Collection<MyGuiControlListbox.Item>)(object)m_listbox.Items).Count - m_listbox.VisibleRowsCount) + 1f)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				m_listbox.SetScrollPosition(scrollPosition);
			}
			else
			{
				m_listbox.SetScrollPosition(0f);
			}
		}

		public void Serialize(BitStream stream, MyTerminalBlock block)
		{
		}
	}
}
