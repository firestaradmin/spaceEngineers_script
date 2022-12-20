namespace VRage.GameServices
{
	public static class MySearchConditionExtensions
	{
		public static bool Contains(this MySearchConditionFlags self, MySearchCondition condition)
		{
			return ((uint)(byte)(1 << (int)condition) & (uint)self) != 0;
		}
	}
}
