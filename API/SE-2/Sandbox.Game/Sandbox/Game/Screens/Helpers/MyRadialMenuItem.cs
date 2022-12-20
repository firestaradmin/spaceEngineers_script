using System.Collections.Generic;
using VRage;
using VRage.Game;

namespace Sandbox.Game.Screens.Helpers
{
	public abstract class MyRadialMenuItem
	{
		public List<string> Icons;

		public bool CloseMenu;

		public virtual MyRadialLabelText Label => new MyRadialLabelText
		{
			Name = LabelName,
			State = string.Empty,
			Shortcut = LabelShortcut
		};

		public string LabelName { get; protected set; }

		public string LabelShortcut { get; protected set; }

		public virtual bool CanBeActivated => Enabled();

		public virtual bool IsValid => true;

		public virtual void Init(MyObjectBuilder_RadialMenuItem builder)
		{
			Icons = new List<string>();
			if (builder.Icons != null)
			{
				foreach (string icon in builder.Icons)
				{
					Icons.Add(icon);
				}
			}
			LabelName = MyTexts.GetString(builder.LabelName);
			_ = builder.LabelShortcut;
			LabelShortcut = MyTexts.GetString(builder.LabelShortcut);
			CloseMenu = builder.CloseMenu;
		}

		public virtual bool Enabled()
		{
			return true;
		}

		public abstract void Activate(params object[] parameters);

		public virtual string GetIcon()
		{
			if (Icons == null || Icons.Count <= 0)
			{
				return string.Empty;
			}
			return Icons[0];
		}
	}
}
