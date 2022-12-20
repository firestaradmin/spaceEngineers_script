using System;

namespace VRage.Library.Collections.__helper_namespace
{
	/// <summary>
	/// Interface describing a generic type list.
	///
	/// We use these as indices for the type dictionary.
	/// </summary>
	internal interface ITypeList
	{
		int Count { get; }

		Type this[int index] { get; }

		int HashCode { get; }

		TypeList GetSolidified();
	}
}
