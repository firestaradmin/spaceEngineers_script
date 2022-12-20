<<<<<<< HEAD
using System;
using System.Runtime.CompilerServices;
using VRage.Network;

namespace Sandbox.Game.Entities.Character
{
	[Serializable]
	public struct MyRecoilDataCollection
	{
		protected class Sandbox_Game_Entities_Character_MyRecoilDataCollection_003C_003EId_003C_003EAccessor : IMemberAccessor<MyRecoilDataCollection, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyRecoilDataCollection owner, in int value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyRecoilDataCollection owner, out int value)
			{
				value = owner.Id;
			}
		}

		protected class Sandbox_Game_Entities_Character_MyRecoilDataCollection_003C_003EVerticalValue_003C_003EAccessor : IMemberAccessor<MyRecoilDataCollection, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyRecoilDataCollection owner, in float value)
			{
				owner.VerticalValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyRecoilDataCollection owner, out float value)
			{
				value = owner.VerticalValue;
			}
		}

		protected class Sandbox_Game_Entities_Character_MyRecoilDataCollection_003C_003EHorizontalValue_003C_003EAccessor : IMemberAccessor<MyRecoilDataCollection, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyRecoilDataCollection owner, in float value)
			{
				owner.HorizontalValue = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyRecoilDataCollection owner, out float value)
			{
				value = owner.HorizontalValue;
			}
		}

=======
namespace Sandbox.Game.Entities.Character
{
	public struct MyRecoilDataCollection
	{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public int Id;

		public float VerticalValue;

		public float HorizontalValue;
	}
}
