using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using VRage;
using VRage.Network;

namespace Sandbox.Game.Screens.Helpers.InputRecording
{
	[Serializable]
	[Obfuscation(Feature = "cw symbol renaming", Exclude = true)]
	public class MyCameraSnapshot
	{
		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyCameraSnapshot_003C_003ECameraPosition_003C_003EAccessor : IMemberAccessor<MyCameraSnapshot, MyPositionAndOrientation?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCameraSnapshot owner, in MyPositionAndOrientation? value)
			{
				owner.CameraPosition = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCameraSnapshot owner, out MyPositionAndOrientation? value)
			{
				value = owner.CameraPosition;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyCameraSnapshot_003C_003ETakeScreenShot_003C_003EAccessor : IMemberAccessor<MyCameraSnapshot, bool>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCameraSnapshot owner, in bool value)
			{
				owner.TakeScreenShot = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCameraSnapshot owner, out bool value)
			{
				value = owner.TakeScreenShot;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyCameraSnapshot_003C_003ELOD_003C_003EAccessor : IMemberAccessor<MyCameraSnapshot, int?>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCameraSnapshot owner, in int? value)
			{
				owner.LOD = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCameraSnapshot owner, out int? value)
			{
				value = owner.LOD;
			}
		}

		protected class Sandbox_Game_Screens_Helpers_InputRecording_MyCameraSnapshot_003C_003EView_003C_003EAccessor : IMemberAccessor<MyCameraSnapshot, MyTestingToolHelper.MyViewsEnum>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyCameraSnapshot owner, in MyTestingToolHelper.MyViewsEnum value)
			{
				owner.View = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyCameraSnapshot owner, out MyTestingToolHelper.MyViewsEnum value)
			{
				value = owner.View;
			}
		}

		public MyPositionAndOrientation? CameraPosition { get; set; }

		public bool TakeScreenShot { get; set; }

		public int? LOD { get; set; }

		public MyTestingToolHelper.MyViewsEnum View { get; set; }
	}
}
