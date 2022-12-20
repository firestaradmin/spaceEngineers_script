using System;
using System.Collections.Generic;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage;
using VRage.Library.Collections;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyTerminalControlCombobox<TBlock> : MyTerminalValueControl<TBlock, long>, IMyTerminalControlCombobox, IMyTerminalControl, IMyTerminalValueControl<long>, ITerminalProperty, IMyTerminalControlTitleTooltip where TBlock : MyTerminalBlock
	{
		public delegate void ComboBoxContentDelegate(TBlock block, ICollection<MyTerminalControlComboBoxItem> comboBoxContent);

		private static List<MyTerminalControlComboBoxItem> m_handlerItems = new List<MyTerminalControlComboBoxItem>();

		public MyStringId Title;

		public MyStringId Tooltip;

		private MyGuiControlCombobox m_comboBox;

		public ComboBoxContentDelegate ComboBoxContentWithBlock;

		public Action<List<MyTerminalControlComboBoxItem>> ComboBoxContent;

		/// <summary>
		/// Implements IMyTerminalControlCombobox for Mods
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

		Action<List<MyTerminalControlComboBoxItem>> IMyTerminalControlCombobox.ComboBoxContent
		{
			get
			{
				Action<List<MyTerminalControlComboBoxItem>> oldComboBoxContent = ComboBoxContent;
				return delegate(List<MyTerminalControlComboBoxItem> x)
				{
					oldComboBoxContent(x);
				};
			}
			set
			{
				ComboBoxContent = value;
			}
		}

		private Action<IMyTerminalBlock, List<MyTerminalControlComboBoxItem>> ComboBoxContentWithBlockAction
		{
			set
			{
				ComboBoxContentWithBlock = delegate(TBlock block, ICollection<MyTerminalControlComboBoxItem> comboBoxContent)
				{
					List<MyTerminalControlComboBoxItem> list = new List<MyTerminalControlComboBoxItem>();
					value(block, list);
					foreach (MyTerminalControlComboBoxItem item2 in list)
					{
						MyTerminalControlComboBoxItem myTerminalControlComboBoxItem = default(MyTerminalControlComboBoxItem);
						myTerminalControlComboBoxItem.Key = item2.Key;
						myTerminalControlComboBoxItem.Value = item2.Value;
						MyTerminalControlComboBoxItem item = myTerminalControlComboBoxItem;
						comboBoxContent.Add(item);
					}
				};
			}
		}

		public MyTerminalControlCombobox(string id, MyStringId title, MyStringId tooltip)
			: base(id)
		{
			Title = title;
			Tooltip = tooltip;
			SetSerializerDefault();
		}

		/// <summary>
		/// Sets default serializer which serializes always 8B
		/// </summary>
		public void SetSerializerDefault()
		{
			Serializer = delegate(BitStream stream, ref long value)
			{
				stream.Serialize(ref value);
			};
		}

		/// <summary>
		/// Serializes values as 0 or 1
		/// </summary>
		public void SetSerializerBit()
		{
			Serializer = delegate(BitStream stream, ref long value)
			{
				if (stream.Reading)
				{
					value = (stream.ReadBool() ? 1 : 0);
				}
				else
				{
					stream.WriteBool(value != 0);
				}
			};
		}

		/// <summary>
		/// Sets optimal serizalizer for range of two values (with uniform probability)
		/// </summary>
		public void SetSerializerRange(int minInclusive, int maxInclusive)
		{
			uint v = (uint)((long)maxInclusive - (long)minInclusive + 1);
			v = MathHelper.GetNearestBiggerPowerOfTwo(v);
			int bitCount = MathHelper.Log2(v);
			Serializer = delegate(BitStream stream, ref long value)
			{
				if (stream.Reading)
				{
					value = (long)stream.ReadUInt64() + (long)minInclusive;
				}
				else
				{
					stream.WriteUInt64((ulong)(value - minInclusive), bitCount);
				}
			};
		}

		/// <summary>
		/// Sets variant length serializer, smaller number takes less bytes than larger.
		/// </summary>
		public void SetSerializerVariant(bool usesNegativeValues = false)
		{
			if (usesNegativeValues)
			{
				Serializer = delegate(BitStream stream, ref long value)
				{
					stream.SerializeVariant(ref value);
				};
				return;
			}
			Serializer = delegate(BitStream stream, ref long value)
			{
				if (stream.Reading)
				{
					value = stream.ReadInt64();
				}
				else
				{
					stream.WriteInt64(value);
				}
			};
		}

		protected override MyGuiControlBase CreateGui()
		{
			m_comboBox = new MyGuiControlCombobox(null, toolTip: MyTexts.GetString(Tooltip), size: new Vector2(0.23f, 0.04f));
			m_comboBox.VisualStyle = MyGuiControlComboboxStyleEnum.Terminal;
			m_comboBox.ItemSelected += OnItemSelected;
			return new MyGuiControlBlockProperty(MyTexts.GetString(Title), MyTexts.GetString(Tooltip), m_comboBox);
		}

		public override void SetValue(TBlock block, long value)
		{
			if (base.Getter(block) != value)
			{
				base.SetValue(block, value);
			}
		}

		private void OnItemSelected()
		{
			if (m_comboBox.GetItemsCount() <= 0)
<<<<<<< HEAD
			{
				return;
			}
			long selectedKey = m_comboBox.GetSelectedKey();
			foreach (TBlock targetBlock in base.TargetBlocks)
			{
				if (targetBlock.CanLocalPlayerChangeValue())
				{
					SetValue(targetBlock, selectedKey);
				}
=======
			{
				return;
			}
			long selectedKey = m_comboBox.GetSelectedKey();
			foreach (TBlock targetBlock in base.TargetBlocks)
			{
				SetValue(targetBlock, selectedKey);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		protected override void OnUpdateVisual()
		{
			base.OnUpdateVisual();
			TBlock firstBlock = base.FirstBlock;
			if (m_comboBox.IsOpen || firstBlock == null)
			{
				return;
			}
			m_comboBox.ClearItems();
			m_handlerItems.Clear();
			if (ComboBoxContent != null)
			{
				ComboBoxContent(m_handlerItems);
				foreach (MyTerminalControlComboBoxItem handlerItem in m_handlerItems)
				{
					m_comboBox.AddItem(handlerItem.Key, handlerItem.Value);
				}
				long value = GetValue(firstBlock);
				if (m_comboBox.GetSelectedKey() != value)
				{
					m_comboBox.ItemSelected -= OnItemSelected;
					m_comboBox.SelectItemByKey(value);
					m_comboBox.ItemSelected += OnItemSelected;
				}
			}
			if (ComboBoxContentWithBlock == null)
			{
				return;
			}
			ComboBoxContentWithBlock(firstBlock, m_handlerItems);
			foreach (MyTerminalControlComboBoxItem handlerItem2 in m_handlerItems)
			{
				m_comboBox.AddItem(handlerItem2.Key, handlerItem2.Value);
			}
			long value2 = GetValue(firstBlock);
			if (m_comboBox.GetSelectedKey() != value2)
			{
				m_comboBox.SelectItemByKey(value2);
			}
		}

		public override long GetDefaultValue(TBlock block)
		{
			return GetMinimum(block);
		}

		public override long GetMinimum(TBlock block)
		{
			long result = 0L;
			if (ComboBoxContent != null)
			{
				m_handlerItems.Clear();
				ComboBoxContent(m_handlerItems);
				if (m_handlerItems.Count > 0)
				{
					result = m_handlerItems[0].Key;
				}
			}
			return result;
		}

		public override long GetMaximum(TBlock block)
		{
			long result = 0L;
			if (ComboBoxContent != null)
			{
				m_handlerItems.Clear();
				ComboBoxContent(m_handlerItems);
				if (m_handlerItems.Count > 0)
				{
					result = m_handlerItems[m_handlerItems.Count - 1].Key;
				}
			}
			return result;
		}
	}
}
