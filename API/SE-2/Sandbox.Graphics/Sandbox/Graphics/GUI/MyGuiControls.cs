using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using VRage.Collections;
using VRage.Game;
using VRage.ObjectBuilders;

namespace Sandbox.Graphics.GUI
{
	public class MyGuiControls : MyGuiControlBase.Friend, IEnumerable<MyGuiControlBase>, IEnumerable
	{
		private static Random m_randomIdGenerator = new Random();

<<<<<<< HEAD
		private readonly IMyGuiControlsOwner m_owner;
=======
		private IMyGuiControlsOwner m_owner;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		private ObservableCollection<MyGuiControlBase> m_controls;

		private Dictionary<string, MyGuiControlBase> m_controlsByName;

		private List<MyGuiControlBase> m_visibleControls;

		private bool m_refreshVisibleControls;

		public MyGuiControlBase this[int index]
		{
			get
			{
				MyScreenManager.CheckThread();
<<<<<<< HEAD
				return m_controls[index];
=======
				return ((Collection<MyGuiControlBase>)(object)m_controls)[index];
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			set
			{
				MyScreenManager.CheckThread();
<<<<<<< HEAD
				MyGuiControlBase myGuiControlBase = m_controls[index];
				if (myGuiControlBase != null)
				{
					myGuiControlBase.VisibleChanged -= Control_VisibleChanged;
=======
				MyGuiControlBase myGuiControlBase = ((Collection<MyGuiControlBase>)(object)m_controls)[index];
				if (myGuiControlBase != null)
				{
					myGuiControlBase.VisibleChanged -= control_VisibleChanged;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_refreshVisibleControls = true;
				}
				if (value != null)
				{
<<<<<<< HEAD
					value.VisibleChanged -= Control_VisibleChanged;
					value.VisibleChanged += Control_VisibleChanged;
					m_controls[index] = value;
=======
					value.VisibleChanged -= control_VisibleChanged;
					value.VisibleChanged += control_VisibleChanged;
					((Collection<MyGuiControlBase>)(object)m_controls)[index] = value;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					m_refreshVisibleControls = true;
				}
			}
		}

		public int Count
		{
			get
			{
				MyScreenManager.CheckThread();
<<<<<<< HEAD
				return m_controls.Count;
=======
				return ((Collection<MyGuiControlBase>)(object)m_controls).Count;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public event Action<MyGuiControls> CollectionChanged;

		public event Action<MyGuiControls> CollectionMembersVisibleChanged;

		public MyGuiControls(IMyGuiControlsOwner owner)
		{
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Expected O, but got Unknown
			m_owner = owner;
			m_controls = new ObservableCollection<MyGuiControlBase>();
			m_controlsByName = new Dictionary<string, MyGuiControlBase>();
			m_visibleControls = new List<MyGuiControlBase>();
			((ObservableCollection<MyGuiControlBase>)m_controls).add_CollectionChanged(new NotifyCollectionChangedEventHandler(OnPrivateCollectionChanged));
			m_refreshVisibleControls = true;
		}

		public void Init(MyObjectBuilder_GuiControls objectBuilder)
		{
			Clear();
			if (objectBuilder.Controls == null)
<<<<<<< HEAD
			{
				return;
			}
			foreach (MyObjectBuilder_GuiControlBase control in objectBuilder.Controls)
			{
=======
			{
				return;
			}
			foreach (MyObjectBuilder_GuiControlBase control in objectBuilder.Controls)
			{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				MyGuiControlBase myGuiControlBase = MyGuiControlsFactory.CreateGuiControl(control);
				if (myGuiControlBase != null)
				{
					myGuiControlBase.Init(control);
					Add(myGuiControlBase);
				}
			}
		}

		public MyObjectBuilder_GuiControls GetObjectBuilder()
		{
			MyObjectBuilder_GuiControls myObjectBuilder_GuiControls = MyObjectBuilderSerializer.CreateNewObject<MyObjectBuilder_GuiControls>();
			myObjectBuilder_GuiControls.Controls = new List<MyObjectBuilder_GuiControlBase>();
			foreach (KeyValuePair<string, MyGuiControlBase> item in m_controlsByName)
			{
				MyObjectBuilder_GuiControlBase objectBuilder = item.Value.GetObjectBuilder();
				myObjectBuilder_GuiControls.Controls.Add(objectBuilder);
			}
			return myObjectBuilder_GuiControls;
		}

		private void OnPrivateCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.CollectionChanged.InvokeIfNotNull(this);
		}

		private void Control_VisibleChanged(object control, bool isVisible)
		{
			m_refreshVisibleControls = true;
			this.CollectionMembersVisibleChanged.InvokeIfNotNull(this);
		}

		private void RefreshVisibleControls()
		{
			if (!m_refreshVisibleControls)
			{
				return;
			}
			m_refreshVisibleControls = false;
			m_visibleControls = new List<MyGuiControlBase>();
			foreach (MyGuiControlBase control in m_controls)
			{
				if (control.Visible)
				{
					m_visibleControls.Add(control);
				}
			}
		}

		public List<MyGuiControlBase> GetVisibleControls()
		{
			MyScreenManager.CheckThread();
			RefreshVisibleControls();
			return m_visibleControls;
		}

		public void Add(MyGuiControlBase control)
		{
			MyScreenManager.CheckThread();
			MyGuiControlBase.Friend.SetOwner(control, m_owner);
			control.Name = ChangeToNonCollidingName(control.Name);
			m_controlsByName.Add(control.Name, control);
			if (control.Visible)
			{
				m_refreshVisibleControls = true;
			}
<<<<<<< HEAD
			m_controls.Add(control);
			control.VisibleChanged += Control_VisibleChanged;
			control.NameChanged += Control_NameChanged;
=======
			((Collection<MyGuiControlBase>)(object)m_controls).Add(control);
			control.VisibleChanged += control_VisibleChanged;
			control.NameChanged += control_NameChanged;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void AddWeak(MyGuiControlBase control)
		{
			MyScreenManager.CheckThread();
			if (control.Visible)
			{
				m_refreshVisibleControls = true;
			}
<<<<<<< HEAD
			m_controls.Add(control);
			control.VisibleChanged += Control_VisibleChanged;
			control.NameChanged += Control_NameChanged;
=======
			((Collection<MyGuiControlBase>)(object)m_controls).Add(control);
			control.VisibleChanged += control_VisibleChanged;
			control.NameChanged += control_NameChanged;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private void Control_NameChanged(MyGuiControlBase control, MyGuiControlBase.NameChangedArgs args)
		{
			MyScreenManager.CheckThread();
			m_controlsByName.Remove(args.OldName);
			control.NameChanged -= Control_NameChanged;
			control.Name = ChangeToNonCollidingName(control.Name);
			control.NameChanged += Control_NameChanged;
			m_controlsByName.Add(control.Name, control);
		}

		public void ClearWeaks()
		{
			MyScreenManager.CheckThread();
<<<<<<< HEAD
			m_controls.Clear();
=======
			((Collection<MyGuiControlBase>)(object)m_controls).Clear();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			m_controlsByName.Clear();
			m_visibleControls.Clear();
			m_refreshVisibleControls = true;
		}

		public void Clear()
		{
			MyScreenManager.CheckThread();
			foreach (MyGuiControlBase control in m_controls)
			{
				control.OnRemoving();
				control.VisibleChanged -= Control_VisibleChanged;
				control.NameChanged -= Control_NameChanged;
			}
			((Collection<MyGuiControlBase>)(object)m_controls).Clear();
			m_controlsByName.Clear();
			m_visibleControls.Clear();
			m_refreshVisibleControls = true;
		}

		public bool Remove(MyGuiControlBase control)
		{
			MyScreenManager.CheckThread();
			m_controlsByName.Remove(control.Name);
			bool num = ((Collection<MyGuiControlBase>)(object)m_controls).Remove(control);
			if (num)
			{
				m_refreshVisibleControls = true;
				control.OnRemoving();
				control.VisibleChanged -= Control_VisibleChanged;
				control.NameChanged -= Control_NameChanged;
			}
			return num;
		}

		public bool RemoveControlByName(string name)
		{
			MyScreenManager.CheckThread();
			MyGuiControlBase controlByName = GetControlByName(name);
			if (controlByName == null)
			{
				return false;
			}
			return Remove(controlByName);
		}

		public int IndexOf(MyGuiControlBase item)
		{
			MyScreenManager.CheckThread();
<<<<<<< HEAD
			return m_controls.IndexOf(item);
=======
			return ((Collection<MyGuiControlBase>)(object)m_controls).IndexOf(item);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public int FindIndex(Predicate<MyGuiControlBase> match)
		{
			MyScreenManager.CheckThread();
			return m_controls.FindIndex(match);
		}

		public MyGuiControlBase GetControlByName(string name)
		{
			MyScreenManager.CheckThread();
			MyGuiControlBase value = null;
			m_controlsByName.TryGetValue(name, out value);
			return value;
		}

		private string ChangeToNonCollidingName(string originalName)
		{
			MyScreenManager.CheckThread();
			string text = originalName;
			while (m_controlsByName.ContainsKey(text))
			{
				text = originalName + m_randomIdGenerator.Next();
			}
			return text;
		}

		public bool Contains(MyGuiControlBase control)
		{
			MyScreenManager.CheckThread();
<<<<<<< HEAD
			return m_controls.Contains(control);
=======
			return ((Collection<MyGuiControlBase>)(object)m_controls).Contains(control);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public ObservableCollection<MyGuiControlBase>.Enumerator GetEnumerator()
		{
			MyScreenManager.CheckThread();
			return m_controls.GetEnumerator();
		}

		IEnumerator<MyGuiControlBase> IEnumerable<MyGuiControlBase>.GetEnumerator()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
