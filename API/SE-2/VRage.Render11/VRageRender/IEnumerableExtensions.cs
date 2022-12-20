using System.Collections.Generic;

namespace VRageRender
{
	public static class IEnumerableExtensions
	{
		/// <summary>
		/// Wraps this object instance into an IEnumerable&lt;T&gt;
		/// consisting of a single item.
		/// </summary>
		/// <typeparam name="T"> Type of the object. </typeparam>
		/// <param name="item"> The instance that will be wrapped. </param>
		/// <returns> An IEnumerable&lt;T&gt; consisting of a single item. </returns>
		public static IEnumerable<T> Yield_<T>(this T item)
		{
			yield return item;
		}
	}
}
