using System;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Game
{
	[Serializable]
	public struct MyUIString
	{
		protected class Sandbox_Game_MyUIString_003C_003EText_003C_003EAccessor : IMemberAccessor<MyUIString, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyUIString owner, in string value)
			{
				owner.Text = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyUIString owner, out string value)
			{
				value = owner.Text;
			}
		}

		protected class Sandbox_Game_MyUIString_003C_003ENormalizedCoord_003C_003EAccessor : IMemberAccessor<MyUIString, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyUIString owner, in Vector2 value)
			{
				owner.NormalizedCoord = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyUIString owner, out Vector2 value)
			{
				value = owner.NormalizedCoord;
			}
		}

		protected class Sandbox_Game_MyUIString_003C_003EScale_003C_003EAccessor : IMemberAccessor<MyUIString, float>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyUIString owner, in float value)
			{
				owner.Scale = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyUIString owner, out float value)
			{
				value = owner.Scale;
			}
		}

		protected class Sandbox_Game_MyUIString_003C_003EFont_003C_003EAccessor : IMemberAccessor<MyUIString, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyUIString owner, in string value)
			{
				owner.Font = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyUIString owner, out string value)
			{
				value = owner.Font;
			}
		}

		protected class Sandbox_Game_MyUIString_003C_003EDrawAlign_003C_003EAccessor : IMemberAccessor<MyUIString, MyGuiDrawAlignEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyUIString owner, in MyGuiDrawAlignEnum value)
			{
				owner.DrawAlign = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyUIString owner, out MyGuiDrawAlignEnum value)
			{
				value = owner.DrawAlign;
			}
		}

		public string Text;

		public Vector2 NormalizedCoord;

		public float Scale;

		public string Font;

		public MyGuiDrawAlignEnum DrawAlign;
	}
}
