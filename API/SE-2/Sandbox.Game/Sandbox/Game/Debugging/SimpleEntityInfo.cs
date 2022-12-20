using System.Runtime.CompilerServices;
using ProtoBuf;
using VRage.Network;
using VRage.Serialization;

namespace Sandbox.Game.Debugging
{
	[ProtoContract]
	public struct SimpleEntityInfo
	{
		protected class Sandbox_Game_Debugging_SimpleEntityInfo_003C_003EId_003C_003EAccessor : IMemberAccessor<SimpleEntityInfo, long>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SimpleEntityInfo owner, in long value)
			{
				owner.Id = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SimpleEntityInfo owner, out long value)
			{
				value = owner.Id;
			}
		}

		protected class Sandbox_Game_Debugging_SimpleEntityInfo_003C_003EDisplayName_003C_003EAccessor : IMemberAccessor<SimpleEntityInfo, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SimpleEntityInfo owner, in string value)
			{
				owner.DisplayName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SimpleEntityInfo owner, out string value)
			{
				value = owner.DisplayName;
			}
		}

		protected class Sandbox_Game_Debugging_SimpleEntityInfo_003C_003EType_003C_003EAccessor : IMemberAccessor<SimpleEntityInfo, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SimpleEntityInfo owner, in string value)
			{
				owner.Type = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SimpleEntityInfo owner, out string value)
			{
				value = owner.Type;
			}
		}

		protected class Sandbox_Game_Debugging_SimpleEntityInfo_003C_003EName_003C_003EAccessor : IMemberAccessor<SimpleEntityInfo, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref SimpleEntityInfo owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref SimpleEntityInfo owner, out string value)
			{
				value = owner.Name;
			}
		}

		private class Sandbox_Game_Debugging_SimpleEntityInfo_003C_003EActor : IActivator, IActivator<SimpleEntityInfo>
		{
			private sealed override object CreateInstance()
			{
				return default(SimpleEntityInfo);
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override SimpleEntityInfo CreateInstance()
			{
				return (SimpleEntityInfo)(object)default(SimpleEntityInfo);
			}

			SimpleEntityInfo IActivator<SimpleEntityInfo>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(5)]
		public long Id;

		[ProtoMember(10)]
		[Nullable]
		public string DisplayName;

		[ProtoMember(15)]
		public string Type;

		[ProtoMember(20)]
		public string Name;
	}
}
