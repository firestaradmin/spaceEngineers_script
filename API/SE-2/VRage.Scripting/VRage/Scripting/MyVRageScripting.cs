namespace VRage.Scripting
{
	public static class MyVRageScripting
	{
		public static IVRageScripting Create()
		{
			return new MyVRageScriptingInternal();
		}
	}
}
