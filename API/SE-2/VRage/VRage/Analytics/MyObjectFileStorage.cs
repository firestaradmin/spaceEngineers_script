using System;
using System.Collections.Generic;
using System.IO;
using LitJson;
using VRage.Utils;

namespace VRage.Analytics
{
	public class MyObjectFileStorage
	{
		public string StoragePath { get; private set; }

		public int MaxStoredFilesPerType { get; private set; }

		public MyObjectFileStorage(string storagePath, int maxStoredFilesPerType = -1)
		{
			if (string.IsNullOrEmpty(storagePath))
			{
				throw new ArgumentNullException("storagePath can't be null");
			}
			Directory.CreateDirectory(storagePath);
			StoragePath = storagePath;
			MaxStoredFilesPerType = maxStoredFilesPerType;
		}

		/// <summary>
		/// Saves given object to a file in set storage directory and marks it by type, timestamp and a unique identifier
		/// </summary>
		public bool StoreObject<T>(T objectToStore, DateTime timestamp) where T : class
		{
<<<<<<< HEAD
			string contents = SerializeObject(objectToStore);
=======
			string text = SerializeObject(objectToStore);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			string newFilePath = GetNewFilePath<T>(timestamp);
			Exception ex = null;
			try
			{
<<<<<<< HEAD
				File.WriteAllText(newFilePath, contents);
=======
				File.WriteAllText(newFilePath, text);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch (Exception ex2)
			{
				MyLog.Default.WriteLine($"ObjectFileStorage error: {ex2}");
				ex = ex2;
			}
			if (ex != null)
			{
				return false;
			}
			PruneExcessFilesOfType<T>();
			return true;
		}

		/// <summary>
		/// Returns all stored objects of given type in set storage directory
		/// </summary>
		public List<T> RetrieveStoredObjectsByType<T>(bool shouldWipeAfter = false) where T : class
		{
			List<T> list = new List<T>();
			try
			{
				string[] files = Directory.GetFiles(StoragePath, GetFilePrefixForType<T>() + "*");
				for (int i = 0; i < files.Length; i++)
				{
					string serializedObject = File.ReadAllText(files[i]);
					T item = DeserializeObject<T>(serializedObject);
					list.Add(item);
				}
			}
			catch (Exception arg)
			{
				MyLog.Default.WriteLine($"Analytics storage error: {arg}");
				return null;
			}
			if (shouldWipeAfter)
			{
				WipeStoredObjectsByType<T>();
			}
			return list;
		}

		/// <summary>
		/// Deletes all stored objects of given type
		/// </summary>
		public int WipeStoredObjectsByType<T>() where T : class
		{
			return DeleteStoredObjectsByType<T>();
		}

		private string SerializeObject<T>(T objectToSerialize) where T : class
		{
			return JsonMapper.ToJson(objectToSerialize);
		}

		private T DeserializeObject<T>(string serializedObject) where T : class
		{
			return JsonMapper.ToObject<T>(serializedObject);
		}

		private string GetNewFilePath<T>(DateTime timestamp)
		{
			string path = GetFilePrefixForType<T>() + timestamp.ToString("o").Replace(':', '-') + "_" + Guid.NewGuid().ToString();
			return Path.Combine(StoragePath, path);
		}

		private string GetFilePrefixForType<T>()
		{
			return typeof(T).Name + "_";
		}

		private void PruneExcessFilesOfType<T>() where T : class
		{
			DeleteStoredObjectsByType<T>(MaxStoredFilesPerType);
		}

		private int DeleteStoredObjectsByType<T>(int amountToKeep = 0) where T : class
		{
			if (amountToKeep < 0)
			{
				return 0;
			}
			int num = 0;
			try
			{
				string[] files = Directory.GetFiles(StoragePath, GetFilePrefixForType<T>() + "*");
				Array.Sort(files);
				for (int i = 0; i < files.Length - amountToKeep; i++)
				{
					File.Delete(files[i]);
					num++;
				}
				return num;
			}
			catch (Exception arg)
			{
				MyLog.Default.WriteLine($"ObjectFileStorage error: {arg}");
				return num;
			}
		}
	}
}
