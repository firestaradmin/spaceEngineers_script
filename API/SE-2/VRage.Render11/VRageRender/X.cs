using VRage.Utils;

namespace VRageRender
{
	internal static class X
	{
		internal static MyStringId TEXT_(string str)
		{
			return MyStringId.GetOrCompute(str);
		}
	}
}
