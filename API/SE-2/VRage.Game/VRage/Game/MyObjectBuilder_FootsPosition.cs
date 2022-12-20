using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Network;
using VRageMath;

namespace VRage.Game
{
	[ProtoContract]
	public class MyObjectBuilder_FootsPosition
	{
		protected class VRage_Game_MyObjectBuilder_FootsPosition_003C_003EAnimation_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FootsPosition, MyCharacterMovementEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FootsPosition owner, in MyCharacterMovementEnum value)
			{
				owner.Animation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FootsPosition owner, out MyCharacterMovementEnum value)
			{
				value = owner.Animation;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FootsPosition_003C_003ELeftFoot_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FootsPosition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FootsPosition owner, in Vector3 value)
			{
				owner.LeftFoot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FootsPosition owner, out Vector3 value)
			{
				value = owner.LeftFoot;
			}
		}

		protected class VRage_Game_MyObjectBuilder_FootsPosition_003C_003ERightFoot_003C_003EAccessor : IMemberAccessor<MyObjectBuilder_FootsPosition, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyObjectBuilder_FootsPosition owner, in Vector3 value)
			{
				owner.RightFoot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyObjectBuilder_FootsPosition owner, out Vector3 value)
			{
				value = owner.RightFoot;
			}
		}

		private class VRage_Game_MyObjectBuilder_FootsPosition_003C_003EActor : IActivator, IActivator<MyObjectBuilder_FootsPosition>
		{
			private sealed override object CreateInstance()
			{
				return new MyObjectBuilder_FootsPosition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyObjectBuilder_FootsPosition CreateInstance()
			{
				return new MyObjectBuilder_FootsPosition();
			}

			MyObjectBuilder_FootsPosition IActivator<MyObjectBuilder_FootsPosition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public MyCharacterMovementEnum Animation;

		[ProtoMember(2)]
		public Vector3 LeftFoot;

		[ProtoMember(3)]
		public Vector3 RightFoot;
	}
}
