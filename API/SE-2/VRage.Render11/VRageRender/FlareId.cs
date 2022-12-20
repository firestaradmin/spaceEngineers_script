using System.Collections.Generic;

namespace VRageRender
{
	internal struct FlareId
	{
		internal class MyFlareIdComparerType : IEqualityComparer<FlareId>
		{
			public bool Equals(FlareId left, FlareId right)
			{
				return left == right;
			}

			public int GetHashCode(FlareId flareId)
			{
				return flareId.Index;
			}
		}

		internal int Index;

		internal static readonly FlareId NULL = new FlareId
		{
			Index = -1
		};

		internal static MyFlareIdComparerType Comparer = new MyFlareIdComparerType();

		public static bool operator ==(FlareId x, FlareId y)
		{
			return x.Index == y.Index;
		}

		public static bool operator !=(FlareId x, FlareId y)
		{
			return x.Index != y.Index;
		}
<<<<<<< HEAD

		public override bool Equals(object obj)
		{
			object obj2;
			if ((obj2 = obj) is FlareId)
			{
				FlareId right = (FlareId)obj2;
				return Comparer.Equals(this, right);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Comparer.GetHashCode();
		}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
