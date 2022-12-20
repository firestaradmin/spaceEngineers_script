using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Network;

namespace VRage.Game
{
	[ProtoContract]
	public class MyWeatherGeneratorVoxelSettings
	{
		protected class VRage_Game_MyWeatherGeneratorVoxelSettings_003C_003EName_003C_003EAccessor : IMemberAccessor<MyWeatherGeneratorVoxelSettings, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyWeatherGeneratorVoxelSettings owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyWeatherGeneratorVoxelSettings owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class VRage_Game_MyWeatherGeneratorVoxelSettings_003C_003EWeight_003C_003EAccessor : IMemberAccessor<MyWeatherGeneratorVoxelSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyWeatherGeneratorVoxelSettings owner, in int value)
			{
				owner.Weight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyWeatherGeneratorVoxelSettings owner, out int value)
			{
				value = owner.Weight;
			}
		}

		protected class VRage_Game_MyWeatherGeneratorVoxelSettings_003C_003EMinLength_003C_003EAccessor : IMemberAccessor<MyWeatherGeneratorVoxelSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyWeatherGeneratorVoxelSettings owner, in int value)
			{
				owner.MinLength = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyWeatherGeneratorVoxelSettings owner, out int value)
			{
				value = owner.MinLength;
			}
		}

		protected class VRage_Game_MyWeatherGeneratorVoxelSettings_003C_003EMaxLength_003C_003EAccessor : IMemberAccessor<MyWeatherGeneratorVoxelSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyWeatherGeneratorVoxelSettings owner, in int value)
			{
				owner.MaxLength = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyWeatherGeneratorVoxelSettings owner, out int value)
			{
				value = owner.MaxLength;
			}
		}

		protected class VRage_Game_MyWeatherGeneratorVoxelSettings_003C_003ESpawnOffset_003C_003EAccessor : IMemberAccessor<MyWeatherGeneratorVoxelSettings, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref MyWeatherGeneratorVoxelSettings owner, in int value)
			{
				owner.SpawnOffset = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref MyWeatherGeneratorVoxelSettings owner, out int value)
			{
				value = owner.SpawnOffset;
			}
		}

		private class VRage_Game_MyWeatherGeneratorVoxelSettings_003C_003EActor : IActivator, IActivator<MyWeatherGeneratorVoxelSettings>
		{
			private sealed override object CreateInstance()
			{
				return new MyWeatherGeneratorVoxelSettings();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyWeatherGeneratorVoxelSettings CreateInstance()
			{
				return new MyWeatherGeneratorVoxelSettings();
			}

			MyWeatherGeneratorVoxelSettings IActivator<MyWeatherGeneratorVoxelSettings>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(15)]
		public string Name;

		[ProtoMember(20)]
		public int Weight;

		[ProtoMember(25)]
		public int MinLength;

		[ProtoMember(26)]
		public int MaxLength;

		[ProtoMember(30)]
		public int SpawnOffset;
	}
}
