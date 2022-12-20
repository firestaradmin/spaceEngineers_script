using SharpDX.Direct3D;
using VRage.Utils;

namespace VRageRender
{
	internal struct MyShaderKey
	{
		public MyStringId FileId;

		public ShaderMacro[] Macros;

		public override int GetHashCode()
		{
			int num = FileId.GetHashCode();
			if (Macros != null)
			{
				num = num * 314159 + Macros.Length;
				for (int i = 0; i < Macros.Length; i++)
				{
					num = num * 314159 + Macros[i].GetHashCode();
				}
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			MyShaderKey myShaderKey = (MyShaderKey)obj;
			if (FileId == myShaderKey.FileId)
			{
				if (Macros == null && myShaderKey.Macros == null)
				{
					return true;
				}
				if (Macros != null && myShaderKey.Macros != null)
				{
					for (int i = 0; i < Macros.Length; i++)
					{
						if (Macros[i] != myShaderKey.Macros[i])
						{
							return false;
						}
					}
					return true;
				}
			}
			return false;
		}
	}
}
