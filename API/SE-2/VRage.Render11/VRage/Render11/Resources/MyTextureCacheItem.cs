using System.Collections.Generic;
using ProtoBuf;
using VRage.Network;

namespace VRage.Render11.Resources
{
	[ProtoContract]
	public class MyTextureCacheItem
	{
		private class VRage_Render11_Resources_MyTextureCacheItem_003C_003EActor : IActivator, IActivator<MyTextureCacheItem>
		{
			private sealed override object CreateInstance()
			{
				return new MyTextureCacheItem();
			}

			object IActivator.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}

			private sealed override MyTextureCacheItem CreateInstance()
			{
				return new MyTextureCacheItem();
			}

			MyTextureCacheItem IActivator<MyTextureCacheItem>.CreateInstance()
			{
				//ILSpy generated this explicit interface implementation from .override directive in CreateInstance
				return this.CreateInstance();
			}
		}

		[ProtoMember(1)]
		public MyFileTextureParams TextureParams;

		[ProtoMember(5)]
		public MyFileTextureParams FullTextureParams;

		[ProtoMember(2)]
		public long Timestamp { get; set; }

		[ProtoMember(3)]
		public List<byte[]> Data { get; set; }

		[ProtoMember(4)]
		public List<int> RowStrides { get; set; }
	}
}
