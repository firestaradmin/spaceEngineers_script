using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Replication.ClientStates
{
	[Serializable]
	public struct MyCockpitMoveState
	{
		protected class Sandbox_Game_Replication_ClientStates_MyCockpitMoveState_003C_003EMove_003C_003EAccessor : IMemberAccessor<MyCockpitMoveState, Vector3>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCockpitMoveState owner, in Vector3 value)
			{
				owner.Move = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCockpitMoveState owner, out Vector3 value)
			{
				value = owner.Move;
			}
		}

		protected class Sandbox_Game_Replication_ClientStates_MyCockpitMoveState_003C_003ERotation_003C_003EAccessor : IMemberAccessor<MyCockpitMoveState, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCockpitMoveState owner, in Vector2 value)
			{
				owner.Rotation = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCockpitMoveState owner, out Vector2 value)
			{
				value = owner.Rotation;
			}
		}

		protected class Sandbox_Game_Replication_ClientStates_MyCockpitMoveState_003C_003ERoll_003C_003EAccessor : IMemberAccessor<MyCockpitMoveState, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCockpitMoveState owner, in float value)
			{
				owner.Roll = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCockpitMoveState owner, out float value)
			{
				value = owner.Roll;
			}
		}

		public Vector3 Move;

		public Vector2 Rotation;

		public float Roll;
	}
}
