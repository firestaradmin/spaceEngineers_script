namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	public abstract class MyActionBase : IMyRadialMenuSystemAction
	{
		public abstract void ExecuteAction();

		public virtual MyRadialLabelText GetLabel(string shortcut, string name)
		{
			return new MyRadialLabelText
			{
				Name = name,
				State = string.Empty,
				Shortcut = shortcut
			};
		}

		public virtual bool IsEnabled()
		{
			return true;
		}

		protected static string AppendingConjunctionState(MyRadialLabelText label)
		{
			if (!string.IsNullOrEmpty(label.State))
			{
				return " - ";
			}
			return "";
		}

		public virtual int GetIconIndex()
		{
			return 0;
		}
	}
}
