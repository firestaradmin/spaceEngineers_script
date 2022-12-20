namespace Sandbox.Game.Screens.Helpers.RadialMenuActions
{
	internal interface IMyRadialMenuSystemAction
	{
		MyRadialLabelText GetLabel(string shortcut, string name);

		bool IsEnabled();

		void ExecuteAction();

		int GetIconIndex();
	}
}
