using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Entities.Character
{
	[Serializable]
	public struct MyDynamicRangePair
	{
		protected class Sandbox_Game_Entities_Character_MyDynamicRangePair_003C_003EDistance_003C_003EAccessor : IMemberAccessor<MyDynamicRangePair, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDynamicRangePair owner, in float value)
			{
				owner.Distance = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDynamicRangePair owner, out float value)
			{
				value = owner.Distance;
			}
		}

		protected class Sandbox_Game_Entities_Character_MyDynamicRangePair_003C_003EHitPosition_003C_003EAccessor : IMemberAccessor<MyDynamicRangePair, Vector3D>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyDynamicRangePair owner, in Vector3D value)
			{
				owner.HitPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyDynamicRangePair owner, out Vector3D value)
			{
				value = owner.HitPosition;
			}
		}

		public float Distance;

		public Vector3D HitPosition;
	}
}
