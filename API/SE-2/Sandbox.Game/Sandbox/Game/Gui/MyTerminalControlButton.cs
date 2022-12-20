using System;
using System.Text;
using Sandbox.Game.Entities.Cube;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage;
using VRage.Audio;
using VRage.Game;
using VRage.Utils;

namespace Sandbox.Game.Gui
{
	public class MyTerminalControlButton<TBlock> : MyTerminalControl<TBlock>, IMyTerminalControlButton, IMyTerminalControl, IMyTerminalControlTitleTooltip where TBlock : MyTerminalBlock
	{
		private Action<TBlock> m_action;

		private Action<MyGuiControlButton> m_buttonClicked;

		public MyStringId Title;

		public MyStringId Tooltip;

		private bool m_isAutoscaleEnabled;

		public Action<TBlock> Action
		{
			get
			{
				return m_action;
			}
			set
			{
				m_action = value;
			}
		}

		/// <summary>
		/// Implement IMyTerminalControlButton for Mods
		/// </summary>
		Action<IMyTerminalBlock> IMyTerminalControlButton.Action
		{
			get
			{
				Action<TBlock> oldAction = Action;
				return delegate(IMyTerminalBlock x)
				{
					oldAction((TBlock)x);
				};
			}
			set
			{
				Action = value;
			}
		}

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

		public MyTerminalControlButton(string id, MyStringId title, MyStringId tooltip, Action<TBlock> action, bool isAutoscaleEnabled = false)
			: base(id)
		{
			Title = title;
			Tooltip = tooltip;
			m_action = action;
			m_isAutoscaleEnabled = isAutoscaleEnabled;
		}

		protected override MyGuiControlBase CreateGui()
		{
			MyGuiControlButton obj = new MyGuiControlButton(null, MyGuiControlButtonStyleEnum.Default, null, null, MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: MyTexts.Get(Title), toolTip: MyTexts.GetString(Tooltip), textScale: 0.8f, textAlignment: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, highlightType: MyGuiControlHighlightType.WHEN_CURSOR_OVER, onButtonClick: null, cueEnum: GuiSounds.MouseClick, buttonScale: 1f, buttonIndex: null, activateOnMouseRelease: false, isAutoscaleEnabled: m_isAutoscaleEnabled);
			m_buttonClicked = OnButtonClicked;
			obj.ButtonClicked += m_buttonClicked;
			return obj;
		}

		private void OnButtonClicked(MyGuiControlButton obj)
		{
			foreach (TBlock targetBlock in base.TargetBlocks)
			{
				if (targetBlock.CanLocalPlayerChangeValue() && m_action != null)
				{
					m_action(targetBlock);
				}
			}
		}

		protected override void OnUpdateVisual()
		{
			MySandboxGame.Static.Invoke(delegate
			{
				base.OnUpdateVisual();
			}, "TerminalControlButton");
		}

		public MyTerminalAction<TBlock> EnableAction(string icon, StringBuilder name, WriterDelegate writer = null)
		{
			MyTerminalAction<TBlock> myTerminalAction = new MyTerminalAction<TBlock>(Id, name, m_action, writer, icon);
			base.Actions = new MyTerminalAction<TBlock>[1] { myTerminalAction };
			return myTerminalAction;
		}
	}
}
