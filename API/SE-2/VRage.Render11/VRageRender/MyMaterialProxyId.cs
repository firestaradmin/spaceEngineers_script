namespace VRageRender
{
	internal struct MyMaterialProxyId
	{
		internal int Index;

		internal static readonly MyMaterialProxyId NULL = new MyMaterialProxyId
		{
			Index = -1
		};

		public MyMaterialProxy_2 Info
		{
			get
			{
				return MyMaterials1.ProxyPool.Data[Index];
			}
			set
			{
				MyMaterials1.ProxyPool.Data[Index] = value;
			}
		}

		public static bool operator ==(MyMaterialProxyId x, MyMaterialProxyId y)
		{
			return x.Index == y.Index;
		}

		public static bool operator !=(MyMaterialProxyId x, MyMaterialProxyId y)
		{
			return x.Index != y.Index;
		}
<<<<<<< HEAD

		public override bool Equals(object obj)
		{
			object obj2;
			if ((obj2 = obj) is MyMaterialProxyId)
			{
				MyMaterialProxyId myMaterialProxyId = (MyMaterialProxyId)obj2;
				return Index == myMaterialProxyId.Index;
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
