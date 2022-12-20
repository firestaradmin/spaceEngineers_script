namespace Sandbox.Game.Screens
{
	public class MyLoadingRuntimeCompilationNotSupportedException : MyLoadingException
	{
		public MyLoadingRuntimeCompilationNotSupportedException()
			: base(MyCommonTexts.MessageBoxTextErrorLoadingScripting)
		{
		}
	}
}
