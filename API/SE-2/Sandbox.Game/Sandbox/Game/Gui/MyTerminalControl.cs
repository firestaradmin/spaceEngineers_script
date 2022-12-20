using System;
<<<<<<< HEAD
=======
using System.Collections.Generic;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Text;
using Sandbox.Game.Entities.Cube;
using Sandbox.Graphics.GUI;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Interfaces.Terminal;
using VRage.Extensions;

namespace Sandbox.Game.Gui
{
	/// <summary>
	/// Terminal control for specified block type.
	/// E.g. Torque slider for stator
	/// </summary>
	public abstract class MyTerminalControl<TBlock> : ITerminalControl, IMyTerminalControl where TBlock : MyTerminalBlock
	{
		public delegate string TooltipGetter(TBlock block);

		public delegate void WriterDelegate(TBlock block, StringBuilder writeTo);

		public delegate void AdvancedWriterDelegate(TBlock block, MyGuiControlBlockProperty control, StringBuilder writeTo);

		public static readonly float PREFERRED_CONTROL_WIDTH = 355f / MyGuiConstants.GUI_OPTIMAL_SIZE.X;

		public static readonly MyTerminalBlock[] Empty = new MyTerminalBlock[0];

		public readonly string Id;

		public Func<TBlock, bool> Enabled = (TBlock b) => true;

		public Func<TBlock, bool> Visible = (TBlock b) => true;

		private MyGuiControlBase m_control;

		public TooltipGetter DynamicTooltipGetter { get; set; }

		MyTerminalBlock[] ITerminalControl.TargetBlocks { get; set; }

		protected ArrayOfTypeEnumerator<MyTerminalBlock, ArrayEnumerator<MyTerminalBlock>, TBlock> TargetBlocks => ((ITerminalControl)this).TargetBlocks.OfTypeFast<MyTerminalBlock, TBlock>();

		protected TBlock FirstBlock
		{
			get
			{
				foreach (TBlock targetBlock in TargetBlocks)
				{
					if (targetBlock.CanLocalPlayerChangeValue())
					{
						return targetBlock;
					}
				}
				using (ArrayOfTypeEnumerator<MyTerminalBlock, ArrayEnumerator<MyTerminalBlock>, TBlock> arrayOfTypeEnumerator = TargetBlocks.GetEnumerator())
				{
					if (arrayOfTypeEnumerator.MoveNext())
					{
						return arrayOfTypeEnumerator.Current;
					}
				}
				return null;
			}
		}

		public bool SupportsMultipleBlocks { get; set; }

		public MyTerminalAction<TBlock>[] Actions { get; protected set; }

		ITerminalAction[] ITerminalControl.Actions => Actions;

		string ITerminalControl.Id => Id;

		string IMyTerminalControl.Id => Id;

		Func<IMyTerminalBlock, bool> IMyTerminalControl.Enabled
		{
			get
			{
				Func<TBlock, bool> oldEnabled = Enabled;
				return (IMyTerminalBlock x) => oldEnabled((TBlock)x);
			}
			set
			{
				Enabled = value;
			}
		}

		Func<IMyTerminalBlock, bool> IMyTerminalControl.Visible
		{
			get
			{
				Func<TBlock, bool> oldVisible = Visible;
				return (IMyTerminalBlock x) => oldVisible((TBlock)x);
			}
			set
			{
				Visible = value;
			}
		}

		public MyGuiControlBase GetGuiControl()
		{
			if (m_control == null)
			{
				m_control = CreateGui();
			}
			return m_control;
		}

		public MyTerminalControl(string id)
		{
			Id = id;
			SupportsMultipleBlocks = true;
			((ITerminalControl)this).TargetBlocks = Empty;
		}

		/// <summary>
		/// Called when app needs GUI (not on DS)
		/// </summary>
		protected abstract MyGuiControlBase CreateGui();

		/// <summary>
		/// Called when GUI needs update
		/// </summary>
		protected virtual void OnUpdateVisual()
		{
			bool flag = false;
			foreach (TBlock targetBlock in TargetBlocks)
			{
				bool flag2 = targetBlock.HasLocalPlayerAccess() && Enabled(targetBlock);
<<<<<<< HEAD
				if (flag2 && targetBlock.IDModule == null && !targetBlock.HasLocalPlayerAccessToBlockWithoutOwnership())
				{
					flag2 = false;
=======
				if (flag2 && targetBlock.CubeGrid != null)
				{
					List<long> bigOwners = targetBlock.CubeGrid.BigOwners;
					if (bigOwners == null || bigOwners.Count != 0)
					{
						List<long> smallOwners = targetBlock.CubeGrid.SmallOwners;
						if ((smallOwners == null || smallOwners.Count != 0) && !targetBlock.HasLocalPlayerAdminUseTerminals() && targetBlock.IDModule == null && !targetBlock.CubeGrid.SmallOwners.Contains(MySession.Static.LocalPlayerId))
						{
							flag2 = false;
						}
					}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				}
				flag = flag || flag2;
			}
			if (m_control.Enabled != flag)
			{
				m_control.Enabled = flag;
			}
			TBlock firstBlock = FirstBlock;
			if (firstBlock != null && DynamicTooltipGetter != null)
			{
				m_control.SetTooltip(DynamicTooltipGetter(firstBlock));
			}
		}

		public void UpdateVisual()
		{
			if (m_control != null)
			{
				OnUpdateVisual();
			}
		}

		public void RedrawControl()
		{
			if (m_control != null)
			{
				m_control = CreateGui();
			}
		}

		bool ITerminalControl.IsVisible(MyTerminalBlock block)
		{
			return Visible((TBlock)block);
		}
	}
}
