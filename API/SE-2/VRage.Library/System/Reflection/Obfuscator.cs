using System.Collections;
using System.Linq;

namespace System.Reflection
{
	public static class Obfuscator
	{
		public const string NoRename = "cw symbol renaming";

		public static readonly bool EnableAttributeCheck = true;

		public static bool CheckAttribute(this MemberInfo member)
		{
			if (!EnableAttributeCheck)
			{
				return true;
			}
			object[] customAttributes = member.GetCustomAttributes(typeof(ObfuscationAttribute), inherit: false);
			foreach (ObfuscationAttribute item in Enumerable.OfType<ObfuscationAttribute>((IEnumerable)customAttributes))
			{
				if (item.Feature == "cw symbol renaming" && item.Exclude)
				{
					return true;
				}
			}
			return false;
		}
	}
}
