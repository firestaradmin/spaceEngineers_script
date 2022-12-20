using System;
using VRage.Input;
using VRageMath;

namespace Sandbox.Graphics.GUI
{
	internal class MyTreeViewItemDragAndDrop : MyGuiControlBase
	{
		/// <summary>
		/// Uggly hack, this control needs contain mouse just one more frame after actual drop
		/// because otherwise for example editor will consume mouse events and do things like
		/// unselecting added objects
		/// </summary>
		private bool m_frameBackDragging;

		public EventHandler Drop;

		public bool Dragging { get; set; }

		public Vector2 StartDragPosition { get; set; }

		public MyTreeViewItem DraggedItem { get; set; }

		public MyTreeViewItemDragAndDrop()
			: base(Vector2.Zero)
		{
		}

		public void Init(MyTreeViewItem item, Vector2 startDragPosition)
		{
			Dragging = false;
			DraggedItem = item;
			StartDragPosition = startDragPosition;
		}

		public bool HandleInput(MyTreeViewItem treeViewItem)
		{
			bool result = false;
			if (DraggedItem == null && MyGUIHelper.Contains(treeViewItem.GetPosition(), treeViewItem.GetSize(), MyGuiManager.MouseCursorPosition.X, MyGuiManager.MouseCursorPosition.Y) && treeViewItem.TreeView.Contains(MyGuiManager.MouseCursorPosition.X, MyGuiManager.MouseCursorPosition.Y) && MyInput.Static.IsNewLeftMousePressed())
			{
				Dragging = false;
				DraggedItem = treeViewItem;
				StartDragPosition = MyGuiManager.MouseCursorPosition;
				result = true;
			}
			return result;
		}

		public override void Draw(float transitionAlpha, float backgroundTransitionAlpha)
		{
			base.Draw(transitionAlpha, backgroundTransitionAlpha);
			if (Dragging)
			{
				Vector2 vector = StartDragPosition - DraggedItem.GetPosition();
				DraggedItem.DrawDraged(MyGuiManager.MouseCursorPosition - vector, transitionAlpha);
			}
			m_frameBackDragging = Dragging;
		}

		public override MyGuiControlBase HandleInput()
		{
			if (DraggedItem != null)
			{
				if (MyInput.Static.IsLeftMousePressed())
				{
					if (MyGuiManager.GetScreenSizeFromNormalizedSize(StartDragPosition - MyGuiManager.MouseCursorPosition).LengthSquared() > 16f)
					{
						Dragging = (m_frameBackDragging = true);
					}
				}
				else
				{
					if (Drop != null && Dragging)
					{
						Drop(this, EventArgs.Empty);
					}
					Dragging = false;
					DraggedItem = null;
				}
			}
			return base.HandleInput();
		}

		public override bool CheckMouseOver(bool use_IsMouseOverAll = true)
		{
			return m_frameBackDragging;
		}
	}
}
