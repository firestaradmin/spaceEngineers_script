using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Sandbox.Game.SessionComponents;
using Sandbox.Game.World;
using Sandbox.Graphics.GUI;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game.Gui
{
	[MyDebugScreen("Game", "In Game Help")]
	internal class MyGuiScreenDebugInGameHelp : MyGuiScreenDebugBase
	{
		private MyGuiControlListbox m_objectives;

		private MyStringHash m_objectiveToSet;

		private long m_nextTimeToSet;

		public MyGuiScreenDebugInGameHelp()
		{
			RecreateControls(constructor: true);
		}

		public override void RecreateControls(bool constructor)
		{
			base.RecreateControls(constructor);
			m_scale = 0.7f;
			AddCaption("Localization", Color.Yellow.ToVector4());
			AddShareFocusHint();
			m_currentPosition = -m_size.Value / 2f + new Vector2(0.02f, 0.1f);
			m_currentPosition.Y += 0.01f * m_scale;
			AddLabel("Loading Screen Texts", Color.Yellow.ToVector4(), 1.2f);
			if (MySession.Static == null)
			{
				return;
			}
			m_objectives = AddListBox(0.37f);
			m_objectives.MultiSelect = false;
			m_objectives.VisibleRowsCount = 10;
<<<<<<< HEAD
			foreach (MyStringHash item2 in MySession.Static.GetComponent<MySessionComponentIngameHelp>().AvailableObjectives.OrderBy((MyStringHash x) => x.String, StringComparer.InvariantCulture))
			{
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(item2.String), item2.String, null, item2);
				m_objectives.Items.Add(item);
=======
			foreach (MyStringHash item2 in (IEnumerable<MyStringHash>)Enumerable.OrderBy<MyStringHash, string>(MySession.Static.GetComponent<MySessionComponentIngameHelp>().AvailableObjectives, (Func<MyStringHash, string>)((MyStringHash x) => x.String), (IComparer<string>)StringComparer.InvariantCulture))
			{
				MyGuiControlListbox.Item item = new MyGuiControlListbox.Item(new StringBuilder(item2.String), item2.String, null, item2);
				((Collection<MyGuiControlListbox.Item>)(object)m_objectives.Items).Add(item);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			AddButton("Set Current Objective", delegate
			{
				MyGuiControlListbox.Item lastSelected = m_objectives.GetLastSelected();
				object userData;
				if (lastSelected != null && (userData = lastSelected.UserData) is MyStringHash)
				{
					MyStringHash myStringHash = (m_objectiveToSet = (MyStringHash)userData);
					m_nextTimeToSet = DateTime.Now.AddSeconds(1.0).Ticks;
				}
			});
		}

		public override bool Update(bool hasFocus)
		{
			if (m_nextTimeToSet < DateTime.Now.Ticks && m_objectiveToSet != MyStringHash.NullOrEmpty)
			{
				MySession.Static.GetComponent<MySessionComponentIngameHelp>().ForceObjective(m_objectiveToSet);
				m_objectiveToSet = MyStringHash.NullOrEmpty;
			}
			return base.Update(hasFocus);
		}

		public override string GetFriendlyName()
		{
			return "MyGuiScreenDebugInGameHelp";
		}
	}
}
