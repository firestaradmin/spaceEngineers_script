using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VRage.GameServices
{
	public static class MyInventoryHelper
	{
		public static List<MyGameInventoryItem> CheckItemData(byte[] checkData, out bool checkResult)
		{
<<<<<<< HEAD
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			using (MemoryStream serializationStream = new MemoryStream(checkData))
			{
				List<MyGameInventoryItem> result = binaryFormatter.Deserialize(serializationStream) as List<MyGameInventoryItem>;
				checkResult = true;
				return result;
			}
=======
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			BinaryFormatter val = new BinaryFormatter();
			using MemoryStream memoryStream = new MemoryStream(checkData);
			List<MyGameInventoryItem> result = val.Deserialize((Stream)memoryStream) as List<MyGameInventoryItem>;
			checkResult = true;
			return result;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static byte[] GetItemsCheckData(List<MyGameInventoryItem> items)
		{
<<<<<<< HEAD
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			using (MemoryStream memoryStream = new MemoryStream())
			{
				binaryFormatter.Serialize(memoryStream, items);
				return memoryStream.ToArray();
			}
=======
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			BinaryFormatter val = new BinaryFormatter();
			using MemoryStream memoryStream = new MemoryStream();
			val.Serialize((Stream)memoryStream, (object)items);
			return memoryStream.ToArray();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public static byte[] GetItemCheckData(MyGameInventoryItem item)
		{
<<<<<<< HEAD
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			using (MemoryStream memoryStream = new MemoryStream())
			{
				List<MyGameInventoryItem> graph = new List<MyGameInventoryItem> { item };
				binaryFormatter.Serialize(memoryStream, graph);
				return memoryStream.ToArray();
			}
=======
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			BinaryFormatter val = new BinaryFormatter();
			using MemoryStream memoryStream = new MemoryStream();
			List<MyGameInventoryItem> list = new List<MyGameInventoryItem> { item };
			val.Serialize((Stream)memoryStream, (object)list);
			return memoryStream.ToArray();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}
	}
}
