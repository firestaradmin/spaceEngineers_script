using System;
using System.Collections;
using System.Collections.Generic;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControlElementGroup : IEnumerable<MyGuiControlBase>, IEnumerable
	{
		public const int INVALID_INDEX = -1;

		private List<MyGuiControlBase> m_controlElements;

		private int? m_selectedIndex;

		public int? SelectedIndex
		{
			get
			{
				return m_selectedIndex;
			}
			set
			{
				if (m_selectedIndex != value)
				{
					if (m_selectedIndex.HasValue)
					{
						m_controlElements[m_selectedIndex.Value].HasHighlight = false;
					}
					m_selectedIndex = value;
					if (m_selectedIndex.HasValue)
					{
						m_controlElements[m_selectedIndex.Value].HasHighlight = true;
					}
					this.HighlightChanged.InvokeIfNotNull(this);
				}
			}
		}

		public event Action<MyGuiControlElementGroup> HighlightChanged;

		public MyGuiControlElementGroup()
		{
			m_controlElements = new List<MyGuiControlBase>();
			m_selectedIndex = null;
		}

		public void Add(MyGuiControlBase controlElement)
		{
			if (controlElement.CanHaveFocus)
			{
				m_controlElements.Add(controlElement);
			}
		}

		public void Remove(MyGuiControlBase controlElement)
		{
			m_controlElements.Remove(controlElement);
		}

		public void Clear()
		{
			m_controlElements.Clear();
			m_selectedIndex = null;
		}

		private MyGuiControlBase TryGetElement(int elementIdx)
		{
			if (elementIdx >= m_controlElements.Count || elementIdx < 0)
			{
				return null;
			}
			return m_controlElements[elementIdx];
		}

		public IEnumerator<MyGuiControlBase> GetEnumerator()
		{
			return m_controlElements.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
