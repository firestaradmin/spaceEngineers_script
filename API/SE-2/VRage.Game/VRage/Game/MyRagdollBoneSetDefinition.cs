using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyRagdollBoneSetDefinition : MyBoneSetDefinition
	{
		protected class VRage_Game_MyRagdollBoneSetDefinition_003C_003ECollisionRadius_003C_003EAccessor : IMemberAccessor<MyRagdollBoneSetDefinition, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyRagdollBoneSetDefinition owner, in float value)
			{
				owner.CollisionRadius = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyRagdollBoneSetDefinition owner, out float value)
			{
				value = owner.CollisionRadius;
			}
		}

		protected class VRage_Game_MyRagdollBoneSetDefinition_003C_003EName_003C_003EAccessor : VRage_Game_MyBoneSetDefinition_003C_003EName_003C_003EAccessor, IMemberAccessor<MyRagdollBoneSetDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyRagdollBoneSetDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyRagdollBoneSetDefinition, MyBoneSetDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyRagdollBoneSetDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyRagdollBoneSetDefinition, MyBoneSetDefinition>(ref owner), out value);
			}
		}

		protected class VRage_Game_MyRagdollBoneSetDefinition_003C_003EBones_003C_003EAccessor : VRage_Game_MyBoneSetDefinition_003C_003EBones_003C_003EAccessor, IMemberAccessor<MyRagdollBoneSetDefinition, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyRagdollBoneSetDefinition owner, in string value)
			{
				Set(ref System.Runtime.CompilerServices.Unsafe.As<MyRagdollBoneSetDefinition, MyBoneSetDefinition>(ref owner), in value);
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyRagdollBoneSetDefinition owner, out string value)
			{
				Get(ref System.Runtime.CompilerServices.Unsafe.As<MyRagdollBoneSetDefinition, MyBoneSetDefinition>(ref owner), out value);
			}
		}

		private class VRage_Game_MyRagdollBoneSetDefinition_003C_003EActor : IActivator, IActivator<MyRagdollBoneSetDefinition>
		{
			private sealed override object CreateInstance()
			{
				return new MyRagdollBoneSetDefinition();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyRagdollBoneSetDefinition CreateInstance()
			{
				return new MyRagdollBoneSetDefinition();
			}

			MyRagdollBoneSetDefinition IActivator<MyRagdollBoneSetDefinition>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(11)]
		public float CollisionRadius;
	}
}
