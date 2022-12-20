using System;
using System.Text;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Screens.Terminal.Controls;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage;
using VRage.Library.Collections;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	public class MyTerminalControlTextbox<TBlock> : MyTerminalValueControl<TBlock, StringBuilder>, ITerminalControlSync, IMyTerminalControlTextbox, IMyTerminalControl, IMyTerminalValueControl<StringBuilder>, ITerminalProperty, IMyTerminalControlTitleTooltip where TBlock : MyTerminalBlock
	{
		public new delegate void SerializerDelegate(BitStream stream, StringBuilder value);

		private char[] m_tmpArray = new char[64];

		private MyGuiControlTextbox m_textbox;

		public new SerializerDelegate Serializer;

		public MyStringId Title;

		public MyStringId Tooltip;

		private StringBuilder m_tmpText = new StringBuilder(15);

		private Action<MyGuiControlTextbox> m_textChanged;

<<<<<<< HEAD
=======
		public new GetterDelegate Getter { private get; set; }

		public new SetterDelegate Setter { private get; set; }

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public SetterDelegate EnterPressed { get; set; }

		/// <summary>
		/// Implements IMyTerminalControlTextbox for ModAPI
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

		/// <summary>
		/// Implements IMyTerminalValueControl for Mods
		/// </summary>
		Func<IMyTerminalBlock, StringBuilder> IMyTerminalValueControl<StringBuilder>.Getter
		{
			get
			{
				GetterDelegate oldGetter = base.Getter;
				return (IMyTerminalBlock x) => oldGetter((TBlock)x);
			}
			set
			{
				base.Getter = value.Invoke;
			}
		}

		Action<IMyTerminalBlock, StringBuilder> IMyTerminalValueControl<StringBuilder>.Setter
		{
			get
			{
				SetterDelegate oldSetter = base.Setter;
				return delegate(IMyTerminalBlock x, StringBuilder y)
				{
					oldSetter((TBlock)x, y);
				};
			}
			set
			{
				base.Setter = value.Invoke;
			}
		}

		public MyTerminalControlTextbox(string id, MyStringId title, MyStringId tooltip)
			: base(id)
		{
			Title = title;
			Tooltip = tooltip;
			Serializer = delegate(BitStream s, StringBuilder sb)
			{
				s.Serialize(sb, ref m_tmpArray, Encoding.UTF8);
			};
		}

		protected override MyGuiControlBase CreateGui()
		{
			m_textbox = new MyGuiControlTextbox();
			m_textbox.Size = new Vector2(MyTerminalControl<TBlock>.PREFERRED_CONTROL_WIDTH, m_textbox.Size.Y);
			m_textChanged = OnTextChanged;
			m_textbox.TextChanged += m_textChanged;
			m_textbox.EnterPressed += OnEnterPressed;
			MyGuiControlBlockProperty myGuiControlBlockProperty = new MyGuiControlBlockProperty(MyTexts.GetString(Title), MyTexts.GetString(Tooltip), m_textbox);
			myGuiControlBlockProperty.Size = new Vector2(MyTerminalControl<TBlock>.PREFERRED_CONTROL_WIDTH, myGuiControlBlockProperty.Size.Y);
			return myGuiControlBlockProperty;
		}

		private void OnTextChanged(MyGuiControlTextbox obj)
		{
			m_tmpText.Clear();
			obj.GetText(m_tmpText);
			foreach (TBlock targetBlock in base.TargetBlocks)
			{
				if (targetBlock.CanLocalPlayerChangeValue())
				{
					SetValue(targetBlock, m_tmpText);
				}
			}
		}

		private void OnEnterPressed(MyGuiControlTextbox obj)
		{
			if (EnterPressed == null)
			{
				return;
			}
			foreach (TBlock targetBlock in base.TargetBlocks)
			{
				if (targetBlock.CanLocalPlayerChangeValue())
				{
					EnterPressed(targetBlock, m_tmpText);
				}
			}
		}

		private void OnEnterPressed(MyGuiControlTextbox obj)
		{
			if (EnterPressed == null)
			{
				return;
			}
			foreach (TBlock targetBlock in base.TargetBlocks)
			{
				EnterPressed(targetBlock, m_tmpText);
			}
		}

		protected override void OnUpdateVisual()
		{
			base.OnUpdateVisual();
			if (m_textbox.IsImeActive)
			{
				return;
			}
			TBlock firstBlock = base.FirstBlock;
			if (firstBlock != null)
			{
				StringBuilder value = GetValue(firstBlock);
				if (!m_textbox.TextEquals(value))
				{
					m_textbox.TextChanged -= m_textChanged;
					m_textbox.SetText(value);
					m_textbox.TextChanged += m_textChanged;
					m_textbox.ResetPosition();
				}
			}
		}

		public override StringBuilder GetValue(TBlock block)
		{
			return base.Getter(block);
		}

		public override void SetValue(TBlock block, StringBuilder value)
		{
<<<<<<< HEAD
			if (base.Setter != null)
			{
				base.Setter(block, new StringBuilder(value.ToString()));
=======
			if (Setter != null)
			{
				Setter(block, new StringBuilder(value.ToString()));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			block.NotifyTerminalValueChanged(this);
		}

		public override StringBuilder GetDefaultValue(TBlock block)
		{
			return new StringBuilder();
		}

		public override StringBuilder GetMinimum(TBlock block)
		{
			return new StringBuilder();
		}

		public override StringBuilder GetMaximum(TBlock block)
		{
			return new StringBuilder();
		}
	}
}
