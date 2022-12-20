using System;

namespace VRage.Library
{
	public static class MyEnvironment
	{
		public static bool Is64BitProcess => Environment.get_Is64BitProcess();

		public static string NewLine => Environment.get_NewLine();

		public static int ProcessorCount => Environment.get_ProcessorCount();

		public static int TickCount => Environment.get_TickCount();

		public static long WorkingSetForMyLog => Environment.get_WorkingSet();
	}
}
