using Sandbox.Game.Screens.Helpers.RadialMenuActions;
using VRage.Game;

namespace Sandbox.Game.Screens.Helpers
{
	[MyRadialMenuItemDescriptor(typeof(MyObjectBuilder_RadialMenuItemSystem))]
	internal class MyRadialMenuItemSystem : MyRadialMenuItem
	{
		private MySystemAction m_systemAction;

		private IMyRadialMenuSystemAction m_action;

		public override MyRadialLabelText Label
		{
			get
			{
				if (m_action == null)
				{
					return new MyRadialLabelText
					{
						Name = base.LabelName,
						State = string.Empty,
						Shortcut = base.LabelShortcut
					};
				}
				return m_action.GetLabel(base.LabelShortcut, base.LabelName);
			}
		}

		public override void Init(MyObjectBuilder_RadialMenuItem builder)
		{
			base.Init(builder);
			MyObjectBuilder_RadialMenuItemSystem myObjectBuilder_RadialMenuItemSystem = (MyObjectBuilder_RadialMenuItemSystem)builder;
			m_systemAction = (MySystemAction)myObjectBuilder_RadialMenuItemSystem.SystemAction;
			m_action = MyRadialMenuItemFactory.GetSystemMenuAction(m_systemAction);
		}

		public override void Activate(params object[] parameters)
		{
			if (Enabled() && m_action != null)
			{
				m_action.ExecuteAction();
			}
		}

		public override string GetIcon()
		{
			if (Icons == null || Icons.Count <= 0 || m_action == null)
			{
				return string.Empty;
			}
			int iconIndex = m_action.GetIconIndex();
			if (iconIndex < 0 || iconIndex >= Icons.Count)
			{
				return string.Empty;
			}
			return Icons[iconIndex];
		}

		public override bool Enabled()
		{
			if (m_action == null)
			{
				return false;
			}
			if (m_action.IsEnabled())
			{
				return base.Enabled();
			}
			return false;
		}
	}
}
