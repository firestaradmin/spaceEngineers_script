using System;

namespace VRage.Game.ModAPI
{
	public class ScriptingReflection : IMyReflection
	{
		public Type BaseTypeOf(Type type)
		{
			return type.BaseType;
		}

		public Type[] GetInterfaces(Type type)
		{
			return type.GetInterfaces();
		}

		public bool IsAssignableFrom(Type baseType, Type derivedType)
		{
			return baseType.IsAssignableFrom(derivedType);
		}

		public bool IsInstanceOfType(Type type, object instance)
		{
			return type.IsInstanceOfType(instance);
		}
	}
}
