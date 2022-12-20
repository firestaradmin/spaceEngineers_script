namespace VRage.Render11.Scene.Components
{
	internal static class My64BitValueHelper
	{
		internal static void SetBits(ref ulong key, int from, int num, ulong value)
		{
			ulong num2 = (uint)((1 << num) - 1);
			ulong num3 = num2 << from;
			key &= ~num3;
			key |= (value & num2) << from;
		}

		internal static ulong GetValue(ref ulong key, int from, int num)
		{
			ulong num2 = (uint)((1 << num) - 1);
			return (key >> from) & num2;
		}
	}
}
