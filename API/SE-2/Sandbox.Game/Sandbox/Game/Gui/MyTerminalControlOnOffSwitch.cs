using System;
using System.Text;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Helpers;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage;
using VRage.Library.Collections;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyTerminalControlOnOffSwitch<TBlock> : MyTerminalValueControl<TBlock, bool>, IMyTerminalControlOnOffSwitch, IMyTerminalControl, IMyTerminalValueControl<bool>, ITerminalProperty, IMyTerminalControlTitleTooltip where TBlock : MyTerminalBlock
	{
		private MyGuiControlOnOffSwitch m_onOffSwitch;

		public MyStringId Title;

		public MyStringId OnText;

		public MyStringId OffText;

		public MyStringId Tooltip;

		private float m_maxWidth = float.PositiveInfinity;

		private bool m_isAutoEllipsisEnabled;

		private bool m_isAutoScaleEnabled;

		private bool m_isButtonAutoScaleEnabled;

		private Action<MyGuiControlOnOffSwitch> m_valueChanged;

		/// <summary>
		///  Implements IMyTerminalControlOnOffSwitch for Mods
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

		MyStringId IMyTerminalControlOnOffSwitch.OnText
		{
			get
			{
				return OnText;
			}
			set
			{
				OnText = value;
			}
		}

		MyStringId IMyTerminalControlOnOffSwitch.OffText
		{
			get
			{
				return OffText;
			}
			set
			{
				OffText = value;
			}
		}

		public MyTerminalControlOnOffSwitch(string id, MyStringId title, MyStringId tooltip = default(MyStringId), MyStringId? on = null, MyStringId? off = null, float max_Width = float.PositiveInfinity, bool is_AutoEllipsisEnabled = false, bool is_AutoScaleEnabled = false)
			: base(id)
		{
			m_isAutoEllipsisEnabled = is_AutoEllipsisEnabled;
			m_isAutoScaleEnabled = is_AutoScaleEnabled;
			m_maxWidth = max_Width;
			Title = title;
			OnText = on ?? MySpaceTexts.SwitchText_On;
			OffText = off ?? MySpaceTexts.SwitchText_Off;
			Tooltip = tooltip;
			Serializer = delegate(BitStream stream, ref bool value)
			{
				stream.Serialize(ref value);
			};
		}

		protected override MyGuiControlBase CreateGui()
		{
			if (MySession.Static.Config.Language == MyLanguagesEnum.Portuguese_Brazil)
			{
				m_isButtonAutoScaleEnabled = true;
			}
			m_onOffSwitch = new MyGuiControlOnOffSwitch(initialValue: false, MyTexts.GetString(OnText), MyTexts.GetString(OffText), m_isButtonAutoScaleEnabled);
			m_onOffSwitch.SetToolTip(MyTexts.GetString(Tooltip));
			m_valueChanged = OnValueChanged;
			m_onOffSwitch.ValueChanged += m_valueChanged;
			MyGuiControlBlockProperty myGuiControlBlockProperty = new MyGuiControlBlockProperty(MyTexts.GetString(Title), MyTexts.GetString(Tooltip), m_onOffSwitch, MyGuiControlBlockPropertyLayoutEnum.Vertical, showExtraInfo: false, m_isAutoEllipsisEnabled, m_isAutoScaleEnabled, m_maxWidth);
			myGuiControlBlockProperty.Size = new Vector2(MyTerminalControl<TBlock>.PREFERRED_CONTROL_WIDTH, myGuiControlBlockProperty.Size.Y);
			return myGuiControlBlockProperty;
		}

		private void OnValueChanged(MyGuiControlOnOffSwitch obj)
		{
			bool value = obj.Value;
			foreach (TBlock targetBlock in base.TargetBlocks)
			{
				if (targetBlock.CanLocalPlayerChangeValue())
				{
					SetValue(targetBlock, value);
				}
			}
		}

		protected override void OnUpdateVisual()
		{
			base.OnUpdateVisual();
			TBlock firstBlock = base.FirstBlock;
			if (firstBlock != null)
			{
				m_onOffSwitch.ValueChanged -= m_valueChanged;
				m_onOffSwitch.Value = GetValue(firstBlock);
				m_onOffSwitch.ValueChanged += m_valueChanged;
			}
			m_onOffSwitch.Position = new Vector2(0.5f * (m_onOffSwitch.Size.X - MyTerminalControl<TBlock>.PREFERRED_CONTROL_WIDTH), m_onOffSwitch.Position.Y);
			m_onOffSwitch.RefreshInternals();
		}

		private void SwitchAction(TBlock block)
		{
			SetValue(block, !GetValue(block));
		}

		private void OnAction(TBlock block)
		{
			SetValue(block, value: true);
		}

		private void OffAction(TBlock block)
		{
			SetValue(block, value: false);
		}

		private void Writer(TBlock block, StringBuilder result, StringBuilder onText, StringBuilder offText)
		{
			result.AppendStringBuilder(GetValue(block) ? onText : offText);
		}

		private void AppendAction(MyTerminalAction<TBlock> action)
		{
			MyTerminalAction<TBlock>[] array = base.Actions ?? new MyTerminalAction<TBlock>[0];
			Array.Resize(ref array, array.Length + 1);
			array[array.Length - 1] = action;
			base.Actions = array;
		}

		public MyTerminalAction<TBlock> EnableToggleAction(string icon, StringBuilder name, StringBuilder onText, StringBuilder offText)
		{
			MyTerminalAction<TBlock> myTerminalAction = new MyTerminalAction<TBlock>(Id, name, SwitchAction, delegate(TBlock x, StringBuilder r)
			{
				Writer(x, r, onText, offText);
			}, icon);
			AppendAction(myTerminalAction);
			return myTerminalAction;
		}

		public MyTerminalAction<TBlock> EnableToggleAction(string icon, StringBuilder name, StringBuilder onText, StringBuilder offText, Func<TBlock, bool> callable)
		{
			MyTerminalAction<TBlock> myTerminalAction = new MyTerminalAction<TBlock>(Id, name, SwitchAction, delegate(TBlock x, StringBuilder r)
			{
				Writer(x, r, onText, offText);
			}, icon, callable);
			AppendAction(myTerminalAction);
			return myTerminalAction;
		}

		public MyTerminalAction<TBlock> EnableOnAction(string icon, StringBuilder name, StringBuilder onText, StringBuilder offText, Func<TBlock, bool> callable)
		{
			MyTerminalAction<TBlock> myTerminalAction = new MyTerminalAction<TBlock>(Id + "_On", name, OnAction, delegate(TBlock x, StringBuilder r)
			{
				Writer(x, r, onText, offText);
			}, icon, callable);
			AppendAction(myTerminalAction);
			return myTerminalAction;
		}

		public MyTerminalAction<TBlock> EnableOffAction(string icon, StringBuilder name, StringBuilder onText, StringBuilder offText, Func<TBlock, bool> callable)
		{
			MyTerminalAction<TBlock> myTerminalAction = new MyTerminalAction<TBlock>(Id + "_Off", name, OffAction, delegate(TBlock x, StringBuilder r)
			{
				Writer(x, r, onText, offText);
			}, icon, callable);
			AppendAction(myTerminalAction);
			return myTerminalAction;
		}

		public override bool GetDefaultValue(TBlock block)
		{
			return false;
		}

		public override bool GetMinimum(TBlock block)
		{
			return false;
		}

		public override bool GetMaximum(TBlock block)
		{
			return true;
		}

		public override void SetValue(TBlock block, bool value)
		{
			base.SetValue(block, value);
		}

		public override bool GetValue(TBlock block)
		{
			return base.GetValue(block);
		}
	}
}
