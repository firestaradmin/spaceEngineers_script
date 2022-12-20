<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage.Render11.Culling;

namespace VRage.Render11.Emit
{
	internal static class CullDataEmitter
	{
		private static FieldInfo[] CullDataFields;

		static CullDataEmitter()
		{
<<<<<<< HEAD
			CullDataFields = (from x in typeof(MyCullResults).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				where x.HasAttribute<CullDataAttribute>()
				select x).ToArray();
=======
			CullDataFields = Enumerable.ToArray<FieldInfo>(Enumerable.Where<FieldInfo>((IEnumerable<FieldInfo>)typeof(MyCullResults).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), (Func<FieldInfo, bool>)((FieldInfo x) => x.HasAttribute<CullDataAttribute>())));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		/// <summary>
		/// Make a field mask for cull results given the names of fields of that type.
		/// </summary>
		/// <param name="members">The list of members of <see cref="T:VRage.Render11.Culling.MyCullResults" /> to use for the mask.</param>
		/// <returns>The mask for the provided fields.</returns>
		public static byte MakeMask(params string[] members)
		{
			MyCullResults myCullResults = new MyCullResults();
			foreach (string name in members)
			{
				FieldInfo field = typeof(MyCullResults).GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				field.FieldType.GetMethod("Add").Invoke(field.GetValue(myCullResults), new object[1]);
			}
			return myCullResults.GetFlags();
		}

		public static byte GetType(MyCullResults results)
		{
			return results.GetFlags();
		}

		public static IEnumerable<string> GetFieldsFromMask(byte mask)
		{
			return MyCullResults.GetFieldsFromMask(mask);
		}
	}
}
