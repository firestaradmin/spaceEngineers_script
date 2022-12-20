using System.Collections.Generic;
using VRage.Utils;

namespace VRage.Game.ModAPI.Ingame.Utilities
{
	/// <summary>
	/// A comparer designed to compare <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.StringSegment" /> instances in a case sensitive manner. Use <see cref="F:VRage.Game.ModAPI.Ingame.Utilities.StringSegmentComparer.DEFAULT" /> for a default instance
	/// </summary>
	public class StringSegmentComparer : IEqualityComparer<StringSegment>
	{
		/// <summary>
		/// A default instance of <see cref="T:VRage.Game.ModAPI.Ingame.Utilities.StringSegmentComparer" />
		/// </summary>
		public static readonly StringSegmentComparer DEFAULT = new StringSegmentComparer();

		/// <summary>Determines whether the specified objects are equal.</summary>
		/// <param name="x">The first object of type T to compare.</param>
		/// <param name="y">The second object of type T to compare.</param>
		/// <returns>true if the specified objects are equal; otherwise, false.</returns>
		public bool Equals(StringSegment x, StringSegment y)
		{
			if (x.Length != y.Length)
			{
				return false;
			}
			string text = x.Text;
			int num = x.Start;
			string text2 = y.Text;
			int num2 = y.Start;
			for (int i = 0; i < x.Length; i++)
			{
				if (text[num] != text2[num2])
				{
					return false;
				}
				num++;
				num2++;
			}
			return true;
		}

		/// <summary>Returns a hash code for the specified object.</summary>
		/// <param name="obj">The <see cref="T:System.Object"></see> for which a hash code is to be returned.</param>
		/// <returns>A hash code for the specified object.</returns>
		/// <exception cref="T:System.ArgumentNullException">The type of <paramref name="obj">obj</paramref> is a reference type and <paramref name="obj">obj</paramref> is null.</exception>
		public int GetHashCode(StringSegment obj)
		{
			return MyUtils.GetHash(obj.Text, obj.Start, obj.Length);
		}
	}
}
