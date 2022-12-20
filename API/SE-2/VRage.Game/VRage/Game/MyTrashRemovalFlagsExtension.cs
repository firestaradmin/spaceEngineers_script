namespace VRage.Game
{
	public static class MyTrashRemovalFlagsExtension
	{
		public static bool HasFlags(this MyTrashRemovalFlags thiz, MyTrashRemovalFlags flags)
		{
			return (thiz & flags) == flags;
		}
	}
}
