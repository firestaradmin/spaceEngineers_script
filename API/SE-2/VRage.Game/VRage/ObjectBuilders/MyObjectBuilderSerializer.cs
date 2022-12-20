using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using ProtoBuf.Meta;
using VRage.FileSystem;
using VRage.Library.Collections;
using VRage.Serialization;
using VRage.Utils;

namespace VRage.ObjectBuilders
{
	public class MyObjectBuilderSerializer
	{
		public enum XmlCompression
		{
			Uncompressed,
			Gzip
		}

		public const string ProtobufferExtension = "B5";

		private static readonly bool ENABLE_PROTOBUFFERS_CLONING;

		private static MyObjectFactory<MyObjectBuilderDefinitionAttribute, MyObjectBuilder_Base> m_objectFactory;

		public static TypeModel Serializer;

		public static readonly MySerializeInfo Dynamic;

		private static IProtoTypeModel m_typeModel;

		static MyObjectBuilderSerializer()
		{
			ENABLE_PROTOBUFFERS_CLONING = true;
			Dynamic = new MySerializeInfo(MyObjectFlags.Dynamic, MyPrimitiveFlags.None, 0, SerializeDynamic, null, null);
			m_typeModel = MyVRage.Platform?.GetTypeModel();
			Serializer = m_typeModel?.Model;
			m_objectFactory = new MyObjectFactory<MyObjectBuilderDefinitionAttribute, MyObjectBuilder_Base>();
		}

		public static void RegisterFromAssembly(Assembly assembly)
		{
			m_objectFactory.RegisterFromAssembly(assembly);
		}

<<<<<<< HEAD
=======
		private static ushort Get16BitHash(string s)
		{
			MD5 val = MD5.Create();
			try
			{
				return BitConverter.ToUInt16(((HashAlgorithm)val).ComputeHash(Encoding.UTF8.GetBytes(s)), 0);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public static void LoadSerializers()
		{
			m_typeModel?.RegisterTypes(Enumerable.Select<MyObjectBuilderDefinitionAttribute, Type>((IEnumerable<MyObjectBuilderDefinitionAttribute>)m_objectFactory.Attributes, (Func<MyObjectBuilderDefinitionAttribute, Type>)((MyObjectBuilderDefinitionAttribute x) => x.ProducedType)));
		}

		private static void SerializeXMLInternal(Stream writeTo, MyObjectBuilder_Base objectBuilder, Type serializeAsType = null)
		{
			MyXmlSerializerManager.GetSerializer(serializeAsType ?? objectBuilder.GetType()).Serialize(writeTo, (object)objectBuilder);
		}

		private static void SerializeGZippedXMLInternal(Stream writeTo, MyObjectBuilder_Base objectBuilder, Type serializeAsType = null)
		{
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Expected O, but got Unknown
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Expected O, but got Unknown
			GZipStream val = new GZipStream(writeTo, (CompressionMode)1, true);
			try
			{
				BufferedStream val2 = new BufferedStream((Stream)(object)val, 32768);
				try
				{
					SerializeXMLInternal((Stream)(object)val2, objectBuilder, serializeAsType);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}

		public static bool SerializeXML(Stream writeTo, MyObjectBuilder_Base objectBuilder, XmlCompression compress = XmlCompression.Uncompressed, Type serializeAsType = null)
		{
			try
			{
				switch (compress)
				{
				case XmlCompression.Gzip:
					SerializeGZippedXMLInternal(writeTo, objectBuilder, serializeAsType);
					break;
				case XmlCompression.Uncompressed:
					SerializeXMLInternal(writeTo, objectBuilder, serializeAsType);
					break;
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("Error during serialization.");
				MyLog.Default.WriteLine(ex.ToString());
				return false;
			}
			return true;
		}

		public static bool SerializeXML(string path, bool compress, MyObjectBuilder_Base objectBuilder, Type serializeAsType = null)
		{
			ulong sizeInBytes;
			return SerializeXML(path, compress, objectBuilder, out sizeInBytes, serializeAsType);
		}

		public static bool SerializeXML(string path, bool compress, MyObjectBuilder_Base objectBuilder, out ulong sizeInBytes, Type serializeAsType = null)
		{
			try
			{
<<<<<<< HEAD
				using (Stream stream = MyFileSystem.OpenWrite(path))
				{
					using (Stream stream2 = (compress ? stream.WrapGZip() : stream))
					{
						long position = stream.Position;
						MyXmlSerializerManager.GetSerializer(serializeAsType ?? objectBuilder.GetType()).Serialize(stream2, objectBuilder);
						sizeInBytes = (ulong)(stream.Position - position);
					}
				}
=======
				using Stream stream = MyFileSystem.OpenWrite(path);
				using Stream stream2 = (compress ? stream.WrapGZip() : stream);
				long position = stream.Position;
				MyXmlSerializerManager.GetSerializer(serializeAsType ?? objectBuilder.GetType()).Serialize(stream2, (object)objectBuilder);
				sizeInBytes = (ulong)(stream.Position - position);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("Error: " + path + " failed to serialize.");
				MyLog.Default.WriteLine(ex.ToString());
				sizeInBytes = 0uL;
				return false;
			}
			return true;
		}

		public static bool DeserializeXML<T>(string path, out T objectBuilder) where T : MyObjectBuilder_Base
		{
			ulong fileSize;
			return DeserializeXML(path, out objectBuilder, out fileSize);
		}

		public static bool DeserializeXML<T>(string path, out T objectBuilder, out ulong fileSize) where T : MyObjectBuilder_Base
		{
			bool flag = false;
			fileSize = 0uL;
			objectBuilder = null;
			using (Stream stream = MyFileSystem.OpenRead(path))
			{
				if (stream != null)
				{
					using Stream stream2 = stream.UnwrapGZip();
					if (stream2 != null)
					{
						fileSize = (ulong)stream.Length;
						flag = DeserializeXML(stream2, out objectBuilder);
					}
				}
			}
			if (!flag)
			{
				MyLog.Default.WriteLine($"Failed to deserialize file '{path}'");
			}
			return flag;
		}

		public static bool DeserializeXML<T>(byte[] xmlData, out T objectBuilder, out ulong fileSize) where T : MyObjectBuilder_Base
		{
			bool flag = false;
			fileSize = 0uL;
			objectBuilder = null;
			using (MemoryStream memoryStream = new MemoryStream(xmlData))
			{
				if (memoryStream != null)
				{
<<<<<<< HEAD
					using (Stream stream = memoryStream.UnwrapGZip())
					{
						if (stream != null)
						{
							fileSize = (ulong)memoryStream.Length;
							flag = DeserializeXML(stream, out objectBuilder);
						}
=======
					using Stream stream = memoryStream.UnwrapGZip();
					if (stream != null)
					{
						fileSize = (ulong)memoryStream.Length;
						flag = DeserializeXML(stream, out objectBuilder);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			if (!flag)
			{
				MyLog.Default.WriteLine("Failed to deserialize a memory file ");
			}
			return flag;
		}

		public static bool DeserializeXML<T>(Stream reader, out T objectBuilder) where T : MyObjectBuilder_Base
		{
			MyObjectBuilder_Base objectBuilder2;
			bool result = DeserializeXML(reader, out objectBuilder2, typeof(T));
			objectBuilder = (T)objectBuilder2;
			return result;
		}

		public static bool DeserializeXML(string path, out MyObjectBuilder_Base objectBuilder, Type builderType)
		{
			bool flag = false;
			objectBuilder = null;
			using (Stream stream = MyFileSystem.OpenRead(path))
			{
				if (stream != null)
				{
					using Stream stream2 = stream.UnwrapGZip();
					if (stream2 != null)
					{
						flag = DeserializeXML(stream2, out objectBuilder, builderType);
					}
				}
			}
			if (!flag)
			{
				MyLog.Default.WriteLine($"Failed to deserialize file '{path}'");
			}
			return flag;
		}

		public static bool DeserializeXML(Stream reader, out MyObjectBuilder_Base objectBuilder, Type builderType)
		{
			return DeserializeXML(reader, out objectBuilder, builderType, null);
		}

		/// <summary>
		///
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="objectBuilder"></param>
		/// <param name="builderType"></param>
		/// <param name="typeOverrideMap">Allows override of the type of the definition. Refer to MyDefinitionXmlSerializer</param>
		/// <returns></returns>
		internal static bool DeserializeXML(Stream reader, out MyObjectBuilder_Base objectBuilder, Type builderType, Dictionary<string, string> typeOverrideMap)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			objectBuilder = null;
			try
			{
				XmlSerializer serializer = MyXmlSerializerManager.GetSerializer(builderType);
				XmlReaderSettings val = new XmlReaderSettings();
				val.set_CheckCharacters(false);
				XmlReaderSettings settings = val;
				MyXmlTextReader myXmlTextReader = new MyXmlTextReader(reader, settings);
				myXmlTextReader.DefinitionTypeOverrideMap = typeOverrideMap;
				objectBuilder = (MyObjectBuilder_Base)serializer.Deserialize((XmlReader)(object)myXmlTextReader);
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("ERROR: Exception during objectbuilder read! (xml): " + builderType.Name);
				MyLog.Default.WriteLine(ex);
				return false;
			}
			return true;
		}

		public static bool DeserializeGZippedXML<T>(Stream reader, out T objectBuilder) where T : MyObjectBuilder_Base
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Expected O, but got Unknown
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			objectBuilder = null;
			try
			{
				GZipStream val = new GZipStream(reader, (CompressionMode)0);
				try
				{
					BufferedStream val2 = new BufferedStream((Stream)(object)val, 32768);
					try
					{
						XmlSerializer serializer = MyXmlSerializerManager.GetSerializer(typeof(T));
						objectBuilder = (T)serializer.Deserialize((Stream)(object)val2);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("ERROR: Exception during objectbuilder read! (xml): " + typeof(T).Name);
				MyLog.Default.WriteLine(ex);
				if (Debugger.IsAttached)
				{
					Debugger.Break();
				}
				return false;
			}
			return true;
		}

		public static void SerializeDynamic(BitStream stream, Type baseType, ref Type obj)
		{
			if (stream.Reading)
			{
				MyRuntimeObjectBuilderId myRuntimeObjectBuilderId = new MyRuntimeObjectBuilderId(stream.ReadUInt16());
				obj = (MyObjectBuilderType)myRuntimeObjectBuilderId;
			}
			else
			{
				stream.WriteUInt16(((MyRuntimeObjectBuilderId)(MyObjectBuilderType)obj).Value);
			}
		}

		/// <summary>
		/// Deserialize data by protobuf
		/// </summary>
		/// <typeparam name="T">object builder type</typeparam>
		/// <param name="path">path</param>
		/// <param name="objectBuilder">instance</param>
		/// <returns>false when fails</returns>
		public static bool DeserializePB<T>(string path, out T objectBuilder) where T : MyObjectBuilder_Base
		{
			ulong fileSize;
			return DeserializePB<T>(path, out objectBuilder, out fileSize);
		}

		/// <summary>
		/// Deserialize data by protobuf
		/// </summary>
		/// <typeparam name="T">object builder type</typeparam>
		/// <param name="path">path</param>
		/// <param name="objectBuilder">instance</param>
		/// <param name="fileSize">size of the file</param>
		/// <returns>false when fails</returns>
		public static bool DeserializePB<T>(string path, out T objectBuilder, out ulong fileSize) where T : MyObjectBuilder_Base
		{
			bool flag = false;
			fileSize = 0uL;
			objectBuilder = null;
			using (Stream stream = MyFileSystem.OpenRead(path))
			{
				if (stream != null)
				{
					using Stream stream2 = stream.UnwrapGZip();
					if (stream2 != null)
					{
						fileSize = (ulong)stream.Length;
						flag = DeserializePB(stream2, out objectBuilder);
					}
				}
			}
			if (!flag)
			{
				MyLog.Default.WriteLine($"Failed to deserialize file '{path}'");
			}
			return flag;
		}

		public static bool DeserializePB<T>(byte[] data, out T objectBuilder, out ulong fileSize) where T : MyObjectBuilder_Base
		{
			bool flag = false;
			fileSize = 0uL;
			objectBuilder = null;
			using (MemoryStream memoryStream = new MemoryStream(data))
			{
				if (memoryStream != null)
				{
<<<<<<< HEAD
					using (Stream stream = memoryStream.UnwrapGZip())
					{
						if (stream != null)
						{
							fileSize = (ulong)memoryStream.Length;
							flag = DeserializePB(stream, out objectBuilder);
						}
=======
					using Stream stream = memoryStream.UnwrapGZip();
					if (stream != null)
					{
						fileSize = (ulong)memoryStream.Length;
						flag = DeserializePB(stream, out objectBuilder);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
					}
				}
			}
			if (!flag)
			{
				MyLog.Default.WriteLine("Failed to deserialize file from memory");
			}
			return flag;
		}

		public static bool DeserializePB<T>(Stream reader, out T objectBuilder) where T : MyObjectBuilder_Base
		{
			MyObjectBuilder_Base objectBuilder2;
			bool result = DeserializePB(reader, out objectBuilder2, typeof(T));
			objectBuilder = (T)objectBuilder2;
			return result;
		}

		internal static bool DeserializePB(Stream reader, out MyObjectBuilder_Base objectBuilder, Type builderType)
		{
			objectBuilder = null;
			try
			{
				objectBuilder = Serializer.Deserialize(reader, null, builderType) as MyObjectBuilder_Base;
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("ERROR: Exception during objectbuilder read! (pb): " + builderType.Name);
				MyLog.Default.WriteLine(ex);
				return false;
			}
			return true;
		}

		public static bool SerializePB(string path, bool compress, MyObjectBuilder_Base objectBuilder)
		{
			ulong sizeInBytes;
			return SerializePB(path, compress, objectBuilder, out sizeInBytes);
		}

		public static bool SerializePB(string path, bool compress, MyObjectBuilder_Base objectBuilder, out ulong sizeInBytes)
		{
			try
			{
<<<<<<< HEAD
				using (Stream stream = MyFileSystem.OpenWrite(path))
				{
					return SerializePB(stream, compress, objectBuilder, out sizeInBytes);
				}
=======
				using Stream stream = MyFileSystem.OpenWrite(path);
				return SerializePB(stream, compress, objectBuilder, out sizeInBytes);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("Error: " + path + " failed to serialize.");
				MyLog.Default.WriteLine(ex.ToString());
				sizeInBytes = 0uL;
				return false;
			}
		}

		public static bool SerializePB(Stream stream, bool compress, MyObjectBuilder_Base objectBuilder, out ulong sizeInBytes)
		{
			try
			{
<<<<<<< HEAD
				using (Stream dest = (compress ? stream.WrapGZip() : stream))
				{
					long position = stream.Position;
					Serializer.Serialize(dest, objectBuilder);
					sizeInBytes = (ulong)(stream.Position - position);
				}
=======
				using Stream dest = (compress ? stream.WrapGZip() : stream);
				long position = stream.Position;
				Serializer.Serialize(dest, objectBuilder);
				sizeInBytes = (ulong)(stream.Position - position);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine("Error: failed to serialize.");
				MyLog.Default.WriteLine(ex.ToString());
				sizeInBytes = 0uL;
				return false;
			}
			return true;
		}

		public static bool SerializePB(Stream stream, MyObjectBuilder_Base objectBuilder)
		{
			try
			{
				Serializer.Serialize(stream, objectBuilder);
			}
			catch (Exception ex)
			{
				MyLog.Default.WriteLine(ex.ToString());
				return false;
			}
			return true;
		}

		public static MyObjectBuilder_Base CreateNewObject(SerializableDefinitionId id)
		{
			return CreateNewObject(id.TypeId, id.SubtypeId);
		}

		public static MyObjectBuilder_Base CreateNewObject(MyObjectBuilderType type, string subtypeName)
		{
			MyObjectBuilder_Base myObjectBuilder_Base = CreateNewObject(type);
			myObjectBuilder_Base.SubtypeName = subtypeName;
			return myObjectBuilder_Base;
		}

		public static MyObjectBuilder_Base CreateNewObject(MyObjectBuilderType type)
		{
			return m_objectFactory.CreateInstance(type);
		}

		public static T CreateNewObject<T>(string subtypeName) where T : MyObjectBuilder_Base, new()
		{
			T val = CreateNewObject<T>();
			val.SubtypeName = subtypeName;
			return val;
		}

		public static T CreateNewObject<T>() where T : MyObjectBuilder_Base, new()
		{
			return m_objectFactory.CreateInstance<T>();
		}

		public static MyObjectBuilder_Base Clone(MyObjectBuilder_Base toClone)
		{
			MyObjectBuilder_Base objectBuilder = null;
			using MemoryStream memoryStream = new MemoryStream();
			if (ENABLE_PROTOBUFFERS_CLONING && Serializer != null)
			{
				Serializer.Serialize(memoryStream, toClone);
				memoryStream.Position = 0L;
				DeserializePB(memoryStream, out objectBuilder, toClone.GetType());
				return objectBuilder;
			}
			SerializeXMLInternal(memoryStream, toClone);
			memoryStream.Position = 0L;
			DeserializeXML(memoryStream, out objectBuilder, toClone.GetType());
			return objectBuilder;
		}

		public static void UnregisterAssembliesAndSerializers()
		{
			m_objectFactory = new MyObjectFactory<MyObjectBuilderDefinitionAttribute, MyObjectBuilder_Base>();
			m_typeModel.FlushCaches();
		}

		public static MemoryStream SerializePB(bool compressed, MyObjectBuilder_Base objectBuilder)
		{
			MemoryStream memoryStream = new MemoryStream();
			if (SerializePB(memoryStream, compressed, objectBuilder, out var _))
			{
				return memoryStream;
			}
			return null;
		}
	}
}
