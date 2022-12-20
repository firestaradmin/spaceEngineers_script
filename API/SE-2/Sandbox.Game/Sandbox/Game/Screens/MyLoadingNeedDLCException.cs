namespace Sandbox.Game.Screens
{
	public class MyLoadingNeedDLCException : MyLoadingException
	{
		public MyDLCs.MyDLC RequiredDLC { get; }

		public MyLoadingNeedDLCException(MyDLCs.MyDLC requiredDLC)
			: base(string.Empty)
		{
			RequiredDLC = requiredDLC;
		}
	}
}
