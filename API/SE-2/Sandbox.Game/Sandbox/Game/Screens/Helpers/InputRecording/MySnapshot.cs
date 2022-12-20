using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage.Network;
using VRageMath;

namespace Sandbox.Game.Screens.Helpers.InputRecording
{
	[Serializable]
	[Obfuscation(Feature = "cw symbol renaming", Exclude = true)]
	public class MySnapshot
	{
		protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot_003C_003EMouseSnapshot_003C_003EAccessor : IMemberAccessor<MySnapshot, MyMouseSnapshot>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySnapshot owner, in MyMouseSnapshot value)
			{
				owner.MouseSnapshot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySnapshot owner, out MyMouseSnapshot value)
			{
				value = owner.MouseSnapshot;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot_003C_003EKeyboardSnapshot_003C_003EAccessor : IMemberAccessor<MySnapshot, List<byte>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySnapshot owner, in List<byte> value)
			{
				owner.KeyboardSnapshot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySnapshot owner, out List<byte> value)
			{
				value = owner.KeyboardSnapshot;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot_003C_003EKeyboardSnapshotText_003C_003EAccessor : IMemberAccessor<MySnapshot, List<char>>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySnapshot owner, in List<char> value)
			{
				owner.KeyboardSnapshotText = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySnapshot owner, out List<char> value)
			{
				value = owner.KeyboardSnapshotText;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot_003C_003EJoystickSnapshot_003C_003EAccessor : IMemberAccessor<MySnapshot, MyJoystickStateSnapshot>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySnapshot owner, in MyJoystickStateSnapshot value)
			{
				owner.JoystickSnapshot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySnapshot owner, out MyJoystickStateSnapshot value)
			{
				value = owner.JoystickSnapshot;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot_003C_003ESnapshotTimestamp_003C_003EAccessor : IMemberAccessor<MySnapshot, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySnapshot owner, in int value)
			{
				owner.SnapshotTimestamp = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySnapshot owner, out int value)
			{
				value = owner.SnapshotTimestamp;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot_003C_003EMouseCursorPosition_003C_003EAccessor : IMemberAccessor<MySnapshot, Vector2>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySnapshot owner, in Vector2 value)
			{
				owner.MouseCursorPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySnapshot owner, out Vector2 value)
			{
				value = owner.MouseCursorPosition;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot_003C_003ECameraSnapshot_003C_003EAccessor : IMemberAccessor<MySnapshot, MyCameraSnapshot>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySnapshot owner, in MyCameraSnapshot value)
			{
				owner.CameraSnapshot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySnapshot owner, out MyCameraSnapshot value)
			{
				value = owner.CameraSnapshot;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot_003C_003EBlockSnapshot_003C_003EAccessor : IMemberAccessor<MySnapshot, MyBlockSnapshot>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySnapshot owner, in MyBlockSnapshot value)
			{
				owner.BlockSnapshot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySnapshot owner, out MyBlockSnapshot value)
			{
				value = owner.BlockSnapshot;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot_003C_003ETimerRepetitions_003C_003EAccessor : IMemberAccessor<MySnapshot, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySnapshot owner, in long value)
			{
				owner.TimerRepetitions = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySnapshot owner, out long value)
			{
				value = owner.TimerRepetitions;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MySnapshot_003C_003ETimerFrames_003C_003EAccessor : IMemberAccessor<MySnapshot, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MySnapshot owner, in int value)
			{
				owner.TimerFrames = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MySnapshot owner, out int value)
			{
				value = owner.TimerFrames;
			}
		}

		public MyMouseSnapshot MouseSnapshot { get; set; }

		public List<byte> KeyboardSnapshot { get; set; }

		public List<char> KeyboardSnapshotText { get; set; }

		public MyJoystickStateSnapshot JoystickSnapshot { get; set; }

		public int SnapshotTimestamp { get; set; }

		public Vector2 MouseCursorPosition { get; set; }

		public MyCameraSnapshot CameraSnapshot { get; set; }

		public MyBlockSnapshot BlockSnapshot { get; set; }

		public long TimerRepetitions { get; set; }

		public int TimerFrames { get; set; }
	}
}
