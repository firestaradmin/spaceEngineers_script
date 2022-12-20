using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using ProtoBuf;
using VRage.Network;
using VRage.Serialization;

namespace Sandbox.Common.ObjectBuilders.Definitions
{
	[ProtoContract]
	public class ScreenArea
	{
		protected class Sandbox_Common_ObjectBuilders_Definitions_ScreenArea_003C_003EName_003C_003EAccessor : IMemberAccessor<ScreenArea, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ScreenArea owner, in string value)
			{
				owner.Name = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ScreenArea owner, out string value)
			{
				value = owner.Name;
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_ScreenArea_003C_003EDisplayName_003C_003EAccessor : IMemberAccessor<ScreenArea, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ScreenArea owner, in string value)
			{
				owner.DisplayName = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ScreenArea owner, out string value)
			{
				value = owner.DisplayName;
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_ScreenArea_003C_003ETextureResolution_003C_003EAccessor : IMemberAccessor<ScreenArea, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ScreenArea owner, in int value)
			{
				owner.TextureResolution = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ScreenArea owner, out int value)
			{
				value = owner.TextureResolution;
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_ScreenArea_003C_003EScreenWidth_003C_003EAccessor : IMemberAccessor<ScreenArea, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ScreenArea owner, in int value)
			{
				owner.ScreenWidth = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ScreenArea owner, out int value)
			{
				value = owner.ScreenWidth;
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_ScreenArea_003C_003EScreenHeight_003C_003EAccessor : IMemberAccessor<ScreenArea, int>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ScreenArea owner, in int value)
			{
				owner.ScreenHeight = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ScreenArea owner, out int value)
			{
				value = owner.ScreenHeight;
			}
		}

		protected class Sandbox_Common_ObjectBuilders_Definitions_ScreenArea_003C_003EScript_003C_003EAccessor : IMemberAccessor<ScreenArea, string>
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Set(ref ScreenArea owner, in string value)
			{
				owner.Script = value;
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public sealed override void Get(ref ScreenArea owner, out string value)
			{
				value = owner.Script;
			}
		}

		private class Sandbox_Common_ObjectBuilders_Definitions_ScreenArea_003C_003EActor : IActivator, IActivator<ScreenArea>
		{
			private sealed override object CreateInstance()
			{
				return new ScreenArea();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override ScreenArea CreateInstance()
			{
				return new ScreenArea();
			}

			ScreenArea IActivator<ScreenArea>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		[XmlAttribute]
		public string Name;

		[ProtoMember(4)]
		[XmlAttribute]
		public string DisplayName;

		[ProtoMember(7)]
		[XmlAttribute]
		[DefaultValue(512)]
		public int TextureResolution = 512;

		[ProtoMember(10)]
		[XmlAttribute]
		[DefaultValue(1)]
		public int ScreenWidth = 1;

		[ProtoMember(13)]
		[XmlAttribute]
		[DefaultValue(1)]
		public int ScreenHeight = 1;

		[ProtoMember(16)]
		[XmlAttribute]
		[DefaultValue(null)]
		[Nullable]
		public string Script;
	}
}
