namespace VRageRender
{
	internal struct ShaderInfoId
	{
		internal int Index;

		internal static readonly ShaderInfoId NULL = new ShaderInfoId
		{
			Index = -1
		};

		public static bool operator ==(ShaderInfoId x, ShaderInfoId y)
		{
			return x.Index == y.Index;
		}

		public static bool operator !=(ShaderInfoId x, ShaderInfoId y)
		{
			return x.Index != y.Index;
		}
<<<<<<< HEAD

		public override bool Equals(object obj)
		{
			object obj2;
			if ((obj2 = obj) is ShaderInfoId)
			{
				ShaderInfoId shaderInfoId = (ShaderInfoId)obj2;
				return Index == shaderInfoId.Index;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Index.GetHashCode();
		}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
