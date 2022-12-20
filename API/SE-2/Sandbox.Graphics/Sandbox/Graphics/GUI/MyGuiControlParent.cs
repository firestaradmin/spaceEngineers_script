using System;
using System.Collections.Generic;
using VRage.Game;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	[MyGuiControlType(typeof(MyObjectBuilder_GuiControlParent))]
	public class MyGuiControlParent : MyGuiControlBase, IMyGuiControlsParent, IMyGuiControlsOwner
	{
		private MyGuiControls m_controls;

		public MyGuiControls Controls => m_controls;

		public MyGuiControlParent()
			: this(null, null, null, null)
		{
		}

		public MyGuiControlParent(Vector2? position = null, Vector2? size = null, Vector4? backgroundColor = null, string toolTip = null)
			: base(position, size, backgroundColor, toolTip, null, isActiveControl: true, canHaveFocus: true)
		{
			m_controls = new MyGuiControls(this);
			base.CanFocusChildren = true;
		}

		public override void Init(MyObjectBuilder_GuiControlBase builder)
		{
			base.Init(builder);
			MyObjectBuilder_GuiControlParent myObjectBuilder_GuiControlParent = builder as MyObjectBuilder_GuiControlParent;
			if (myObjectBuilder_GuiControlParent.Controls != null)
			{
				m_controls.Init(myObjectBuilder_GuiControlParent.Controls);
			}
		}

		public override MyObjectBuilder_GuiControlBase GetObjectBuilder()
		{
			MyObjectBuilder_GuiControlParent obj = base.GetObjectBuilder() as MyObjectBuilder_GuiControlParent;
			obj.Controls = Controls.GetObjectBuilder();
			return obj;
		}

		public override void Clear()
		{
			Controls.Clear();
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			foreach (MyGuiControlBase visibleControl in Controls.GetVisibleControls())
			{
<<<<<<< HEAD
				if (visibleControl.IsWithinDrawScissor && visibleControl.GetExclusiveInputHandler() != visibleControl && !(visibleControl is MyGuiControlGridDragAndDrop))
=======
				if (visibleControl.GetExclusiveInputHandler() != visibleControl && !(visibleControl is MyGuiControlGridDragAndDrop) && visibleControl.IsWithinDrawScissor)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					visibleControl.Draw(transitionAlpha * visibleControl.Alpha, backgroundTransitionAlpha * visibleControl.Alpha);
				}
			}
		}

		public override MyGuiControlBase HandleInput()
		{
			MyGuiControlBase myGuiControlBase = null;
			myGuiControlBase = base.HandleInput();
			base.IsMouseOver = true;
			foreach (MyGuiControlBase visibleControl in Controls.GetVisibleControls())
			{
				if (visibleControl.IsWithinDrawScissor)
				{
					myGuiControlBase = visibleControl.HandleInput();
					if (myGuiControlBase != null)
					{
						return myGuiControlBase;
					}
				}
			}
			return myGuiControlBase;
		}

		public bool CheckChildFocus()
		{
			bool result = false;
			MyGuiControlBase myGuiControlBase = null;
			MyGuiControlBase myGuiControlBase2 = GetTopMostOwnerScreen()?.FocusedControl;
			if (myGuiControlBase2 != null && myGuiControlBase != myGuiControlBase2)
			{
				myGuiControlBase = myGuiControlBase2;
				result = false;
				while (myGuiControlBase2.Owner != null)
				{
					if (myGuiControlBase2.Owner == this)
					{
						result = true;
						break;
					}
					myGuiControlBase2 = myGuiControlBase2.Owner as MyGuiControlBase;
					if (myGuiControlBase2 == null)
					{
						break;
					}
				}
			}
			return result;
		}

		public override MyGuiControlBase GetExclusiveInputHandler()
		{
			MyGuiControlBase exclusiveInputHandler = MyGuiControlBase.GetExclusiveInputHandler(Controls);
			if (exclusiveInputHandler == null)
			{
				exclusiveInputHandler = base.GetExclusiveInputHandler();
			}
			return exclusiveInputHandler;
		}

		public override bool IsMouseOverAnyControl()
		{
			List<MyGuiControlBase> visibleControls = Controls.GetVisibleControls();
			for (int num = visibleControls.Count - 1; num >= 0; num--)
			{
<<<<<<< HEAD
				if (!visibleControls[num].IsHitTestVisible && visibleControls[num].IsWithinDrawScissor && visibleControls[num].IsMouseOver)
=======
				if (!visibleControls[num].IsHitTestVisible && visibleControls[num].IsMouseOver)
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					return true;
				}
			}
			return false;
		}

		public override MyGuiControlBase GetMouseOverControl()
		{
			List<MyGuiControlBase> visibleControls = Controls.GetVisibleControls();
			for (int num = visibleControls.Count - 1; num >= 0; num--)
			{
				if (visibleControls[num].IsHitTestVisible && visibleControls[num].IsMouseOver)
				{
					return visibleControls[num];
				}
			}
			return null;
		}

		public override MyGuiControlGridDragAndDrop GetDragAndDropHandlingNow()
		{
			List<MyGuiControlBase> visibleControls = Controls.GetVisibleControls();
			for (int i = 0; i < visibleControls.Count; i++)
			{
				MyGuiControlBase myGuiControlBase = visibleControls[i];
				if (myGuiControlBase is MyGuiControlGridDragAndDrop)
				{
					MyGuiControlGridDragAndDrop myGuiControlGridDragAndDrop = (MyGuiControlGridDragAndDrop)myGuiControlBase;
					if (myGuiControlGridDragAndDrop.IsActive())
					{
						return myGuiControlGridDragAndDrop;
					}
				}
			}
			return null;
		}

		public override void HideToolTip()
		{
			foreach (MyGuiControlBase control in Controls)
			{
				control.HideToolTip();
			}
		}

		public override void ShowToolTip()
		{
			foreach (MyGuiControlBase visibleControl in Controls.GetVisibleControls())
			{
				if (visibleControl.IsWithinDrawScissor)
				{
					visibleControl.ShowToolTip();
				}
			}
		}

		public override void Update()
		{
			foreach (MyGuiControlBase visibleControl in Controls.GetVisibleControls())
			{
				if (visibleControl.IsWithinDrawScissor || visibleControl.HasFocus || visibleControl.HasHighlight || visibleControl.IsActiveControl)
				{
					visibleControl.Update();
				}
			}
			base.Update();
		}

		public override void OnRemoving()
		{
			Controls.Clear();
			base.OnRemoving();
		}

		public override MyGuiControlBase GetNextFocusControl(MyGuiControlBase currentFocusControl, MyDirection direction, bool page)
		{
			return MyGuiScreenBase.GetNextFocusControl(currentFocusControl, null, Controls.GetVisibleControls(), Elements.GetVisibleControls(), direction, page);
		}

		public override void UpdateArrange()
		{
			base.UpdateArrange();
		}

		public override void UpdateMeasure()
		{
			base.UpdateMeasure();
		}

		public override void OnFocusChanged(MyGuiControlBase control, bool focus)
		{
			base.OnFocusChanged(control, focus);
			base.Owner?.OnFocusChanged(control, focus);
		}

		public override void CheckIsWithinScissor(RectangleF scissor, bool complete = false)
		{
			Vector2 topLeft = Vector2.Zero;
			Vector2 botRight = Vector2.Zero;
			GetScissorBounds(ref topLeft, ref botRight);
			Vector2 vector = new Vector2(Math.Max(topLeft.X, scissor.X), Math.Max(topLeft.Y, scissor.Y));
			Vector2 size = new Vector2(Math.Min(botRight.X, scissor.Right), Math.Min(botRight.Y, scissor.Bottom)) - vector;
			if (size.X <= 0f || size.Y <= 0f)
			{
				base.IsWithinScissor = false;
				foreach (MyGuiControlBase control in Controls)
				{
					control.IsWithinScissor = false;
				}
				return;
			}
			RectangleF scissor2 = default(RectangleF);
			scissor2.Position = vector;
			scissor2.Size = size;
			base.IsWithinScissor = true;
			foreach (MyGuiControlBase control2 in Controls)
			{
				control2.CheckIsWithinScissor(scissor2, complete);
			}
		}
	}
}
