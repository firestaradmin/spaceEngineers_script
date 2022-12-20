using ProtoBuf;
using SharpDX.DXGI;
using SharpDX.Toolkit.Graphics;
using VRage.Network;
using VRageMath;

namespace VRage.Render11.Resources
{
	[ProtoContract]
	public struct MyFileTextureParams
	{
		private class VRage_Render11_Resources_MyFileTextureParams_003C_003EActor : IActivator, IActivator<MyFileTextureParams>
		{
			private sealed override object CreateInstance()
			{
				return default(MyFileTextureParams);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyFileTextureParams CreateInstance()
			{
				return (MyFileTextureParams)(object)default(MyFileTextureParams);
			}

			MyFileTextureParams IActivator<MyFileTextureParams>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public Vector2I Resolution;

		[ProtoMember(2)]
		public Format Format;

		[ProtoMember(3)]
		public int MipLevels;

		[ProtoMember(4)]
		public int ArraySize;

		[ProtoMember(5)]
		public TextureDimension Dimension;

		public bool IsEmpty => MipLevels == 0;

		public static bool operator ==(MyFileTextureParams params1, MyFileTextureParams params2)
		{
			return params1.Equals(params2);
		}

		public static bool operator !=(MyFileTextureParams params1, MyFileTextureParams params2)
		{
			return !params1.Equals(params2);
		}
<<<<<<< HEAD

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
	}
}
