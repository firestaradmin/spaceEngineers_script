using System;

namespace VRage.Game.ModAPI
{
	public interface IMyReflection
	{
		/// <summary>
		/// <see cref="P:System.Type.BaseType" />
		/// </summary>
		Type BaseTypeOf(Type type);

		/// <summary>
		/// <see cref="M:System.Type.GetInterfaces" />
		/// </summary>
		Type[] GetInterfaces(Type type);

		/// <summary>
		/// <see cref="M:System.Type.IsInstanceOfType(System.Object)" />
		/// </summary>
		bool IsInstanceOfType(Type type, object instance);

		/// <summary>
		/// <see cref="M:System.Type.IsAssignableFrom(System.Type)" />
		/// </summary>
		bool IsAssignableFrom(Type baseType, Type derivedType);
	}
}
