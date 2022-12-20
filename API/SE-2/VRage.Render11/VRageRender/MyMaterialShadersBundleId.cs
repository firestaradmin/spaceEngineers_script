using SharpDX.Direct3D11;

namespace VRageRender
{
	internal struct MyMaterialShadersBundleId
	{
		internal int Index;

		internal static readonly MyMaterialShadersBundleId NULL = new MyMaterialShadersBundleId
		{
			Index = -1
		};

		internal InputLayout IL => MyMaterialShaders.Bundles[Index].IL;

		internal VertexShader VS => MyMaterialShaders.Bundles[Index].VS;

		internal PixelShader PS => MyMaterialShaders.Bundles[Index].PS;

		internal MyMaterialShadersInfo BundleInfo => MyMaterialShaders.BundleInfo.Data[Index];

		public static bool operator ==(MyMaterialShadersBundleId x, MyMaterialShadersBundleId y)
		{
			return x.Index == y.Index;
		}

		public static bool operator !=(MyMaterialShadersBundleId x, MyMaterialShadersBundleId y)
		{
			return x.Index != y.Index;
		}
<<<<<<< HEAD

		public override bool Equals(object obj)
		{
			object obj2;
			if ((obj2 = obj) is MyMaterialShadersBundleId)
			{
				MyMaterialShadersBundleId myMaterialShadersBundleId = (MyMaterialShadersBundleId)obj2;
				return Index == myMaterialShadersBundleId.Index;
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
