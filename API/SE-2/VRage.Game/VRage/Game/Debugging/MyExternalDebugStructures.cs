using System;
using System.Runtime.InteropServices;
using ProtoBuf;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace VRage.Game.Debugging
{
	public static class MyExternalDebugStructures
	{
		private class DefaultProtoSerializer<T>
		{
			public static readonly ProtoSerializer<T> Default = new ProtoSerializer<T>(MyObjectBuilderSerializer.Serializer);
		}

		public interface IExternalDebugMsg
		{
			string GetTypeStr();
		}

		public struct CommonMsgHeader
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
			public string MsgHeader;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
			public string MsgType;

			[MarshalAs(UnmanagedType.I4)]
			public int MsgSize;

			public bool IsValid => MsgHeader == "VRAGEMS";

			public static CommonMsgHeader Create(string msgType, int msgSize = 0)
			{
				CommonMsgHeader result = default(CommonMsgHeader);
				result.MsgHeader = "VRAGEMS";
				result.MsgType = msgType;
				result.MsgSize = msgSize;
				return result;
			}
		}

		public static readonly int MsgHeaderSize = Marshal.SizeOf(typeof(CommonMsgHeader));

		public static ISerializer<TMsg> GetSerializer<TMsg>() where TMsg : struct
		{
			if (Attribute.IsDefined(typeof(TMsg), typeof(ProtoContractAttribute)))
			{
				return CreateProto<TMsg>();
			}
			return null;
		}

		private static ISerializer<TMsg> CreateProto<TMsg>()
		{
			return DefaultProtoSerializer<TMsg>.Default;
		}

		/// <summary>
		/// Convert from raw data to message.
		/// Message must be struct with sequential layout having first field "Header" of type "CommonMsg".
		/// </summary>
		public static bool ReadMessageFromPtr<TMessage>(ref CommonMsgHeader header, byte[] data, out TMessage outMsg) where TMessage : struct, IExternalDebugMsg
		{
			outMsg = default(TMessage);
			if (header.MsgType != outMsg.GetTypeStr())
			{
				return false;
			}
			ISerializer<TMessage> serializer = GetSerializer<TMessage>();
			ByteStream source = new ByteStream(data, header.MsgSize);
			serializer.Deserialize(source, out outMsg);
			return true;
		}
	}
}
