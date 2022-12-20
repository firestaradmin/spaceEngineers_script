using System;
using System.Collections.Concurrent;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using VRage.FileSystem;
using VRageRender;

namespace VRage.Render11.Shader
{
	internal class MyShaderCache
	{
		private const bool USE_COMPRESSION = true;

		private static readonly ConcurrentDictionary<string, bool> m_inProcess;

		static MyShaderCache()
		{
			m_inProcess = new ConcurrentDictionary<string, bool>();
			Directory.CreateDirectory(Path.Combine(MyFileSystem.UserDataPath, "ShaderCache2"));
		}

		private static string GetDecompressedSource(byte[] bytes, UTF8Encoding encoding)
		{
<<<<<<< HEAD
			using (MemoryStream stream = new MemoryStream(bytes))
			{
				using (GZipStream gZipStream = new GZipStream(stream, CompressionMode.Decompress))
				{
					MemoryStream memoryStream = new MemoryStream();
					gZipStream.CopyTo(memoryStream);
					return encoding.GetString(memoryStream.ToArray());
				}
=======
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Expected O, but got Unknown
			using MemoryStream memoryStream = new MemoryStream(bytes);
			GZipStream val = new GZipStream((Stream)memoryStream, (CompressionMode)0);
			try
			{
				MemoryStream memoryStream2 = new MemoryStream();
				((Stream)(object)val).CopyTo((Stream)memoryStream2);
				return encoding.GetString(memoryStream2.ToArray());
			}
			finally
			{
				((IDisposable)val)?.Dispose();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		private static byte[] GetCompressedSource(string text, UTF8Encoding encoding)
		{
<<<<<<< HEAD
			byte[] bytes = encoding.GetBytes(text);
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
				{
					gZipStream.Write(bytes, 0, bytes.Length);
				}
				return memoryStream.ToArray();
			}
=======
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			byte[] bytes = encoding.GetBytes(text);
			using MemoryStream memoryStream = new MemoryStream();
			GZipStream val = new GZipStream((Stream)memoryStream, (CompressionMode)1);
			try
			{
				((Stream)(object)val).Write(bytes, 0, bytes.Length);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			return memoryStream.ToArray();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		private static string GetHashFromString(string text, UTF8Encoding encoding, MD5 md5)
		{
			byte[] bytes = encoding.GetBytes(text);
<<<<<<< HEAD
			byte[] array = md5.ComputeHash(bytes);
=======
			byte[] array = ((HashAlgorithm)md5).ComputeHash(bytes);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			StringBuilder stringBuilder = new StringBuilder(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("X2"));
			}
			return stringBuilder.ToString();
		}

		private static string GetHashFromByteArray(byte[] bytes, MD5 md5)
		{
<<<<<<< HEAD
			byte[] array = md5.ComputeHash(bytes);
=======
			byte[] array = ((HashAlgorithm)md5).ComputeHash(bytes);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			StringBuilder stringBuilder = new StringBuilder(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("X2"));
			}
			return stringBuilder.ToString();
		}

		public static string GetPreprocessedFilepath(string basePath, string subdirName, string hashForFilename)
		{
			string path = hashForFilename + ".hlsl";
			return Path.Combine(basePath, subdirName, path);
		}

		public static string GetPdbFilepath(string basePath, string subdirName, string hashForFilename)
		{
			string path = hashForFilename + ".pdb";
			return Path.Combine(basePath, subdirName, path);
		}

		private static string GetSourceFilepath(string basePath, string subdirName, string hashForFilename, MyShaderProfile profile)
		{
			string path = hashForFilename + "." + MyShaderCompiler.ProfileToString(profile) + ".hash";
			return Path.Combine(basePath, subdirName, path);
		}

		private static string GetCacheFilepath(string basePath, string subdirName, string hashForFilename, MyShaderProfile profile)
		{
			string path = hashForFilename + "." + MyShaderCompiler.ProfileToString(profile) + ".cache";
			return Path.Combine(basePath, subdirName, path);
		}

		private static void DeleteSafe(string filepath)
		{
			try
			{
				File.Delete(filepath);
			}
			catch (Exception)
			{
			}
		}

		private static int GetHeaderLength(byte[] sourceFile)
		{
			int i;
			for (i = 0; i < sourceFile.Length && sourceFile[i] != 10; i++)
			{
			}
			return i;
		}

		private static byte[] SkipHeader(byte[] sourceFile)
		{
			int num = GetHeaderLength(sourceFile);
			if (num < sourceFile.Length)
			{
				num++;
			}
			return sourceFile.CreateSubarray(num, sourceFile.Length - num);
		}

		private static string CreateHeader(byte[] binaryShaderCache, MD5 md5)
		{
			return "CacheCrc=" + GetHashFromByteArray(binaryShaderCache, md5);
		}

		private static bool TryFetch(string basePath, string dirName, string hashForFilename, string preprocessedSource, MyShaderProfile profile, out byte[] cache, UTF8Encoding encoding, MD5 md5)
		{
			cache = null;
			string sourceFilepath = GetSourceFilepath(basePath, dirName, hashForFilename, profile);
			string cacheFilepath = GetCacheFilepath(basePath, dirName, hashForFilename, profile);
			if (!File.Exists(sourceFilepath) || !File.Exists(cacheFilepath))
			{
				if (File.Exists(sourceFilepath))
				{
					DeleteSafe(sourceFilepath);
				}
				if (File.Exists(cacheFilepath))
				{
					DeleteSafe(cacheFilepath);
				}
				return false;
			}
			try
			{
				byte[] array = File.ReadAllBytes(sourceFilepath);
				byte[] array2 = File.ReadAllBytes(cacheFilepath);
				string @string = encoding.GetString(array, 0, GetHeaderLength(array));
				byte[] bytes = SkipHeader(array);
				string string2 = encoding.GetString(array2, 0, GetHeaderLength(array2));
				byte[] array3 = SkipHeader(array2);
				string text = CreateHeader(array3, md5);
				if (GetDecompressedSource(bytes, encoding) == preprocessedSource && string2 == text && @string == text)
				{
					cache = array3;
					return true;
				}
			}
			catch (Exception ex)
			{
				MyRender11.Log.WriteLine("Corrupted shader cache files " + sourceFilepath + " / " + preprocessedSource + ", error: " + ex);
			}
			MyRender11.Log.WriteLine("Preprocessed shader " + cacheFilepath + " cannot be used. Deleting cache for the shader.");
			DeleteSafe(sourceFilepath);
			DeleteSafe(cacheFilepath);
			return false;
		}

		public static bool TryFetch(string preprocessedSource, MyShaderProfile profile, out byte[] cache, out string preprocessedSourceHash)
		{
			UTF8Encoding encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
			MD5 md = MD5.Create();
			preprocessedSourceHash = GetHashFromString(preprocessedSource, encoding, md);
<<<<<<< HEAD
			while (!m_inProcess.TryAdd(preprocessedSourceHash, value: true))
=======
			while (!m_inProcess.TryAdd(preprocessedSourceHash, true))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			{
				Thread.Sleep(10);
			}
			bool flag = TryFetch(MyFileSystem.ContentPath, "ShaderCache", preprocessedSourceHash, preprocessedSource, profile, out cache, encoding, md);
			if (!flag)
			{
				flag = TryFetch(MyFileSystem.UserDataPath, "ShaderCache2", preprocessedSourceHash, preprocessedSource, profile, out cache, encoding, md);
			}
			if (flag)
			{
				Release(preprocessedSourceHash);
			}
			return flag;
		}

		public static void Release(string hash)
		{
			if (!string.IsNullOrEmpty(hash))
			{
<<<<<<< HEAD
				m_inProcess.TryRemove(hash, out var _);
=======
				bool flag = default(bool);
				m_inProcess.TryRemove(hash, ref flag);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		internal static void Store(string preprocessedSource, MyShaderProfile profile, byte[] cache)
		{
<<<<<<< HEAD
=======
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Expected O, but got Unknown
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			UTF8Encoding uTF8Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
			MD5 md = MD5.Create();
			string hashFromString = GetHashFromString(preprocessedSource, uTF8Encoding, md);
			string s = CreateHeader(cache, md) + "\n";
			string sourceFilepath = GetSourceFilepath(MyFileSystem.UserDataPath, "ShaderCache2", hashFromString, profile);
			try
			{
				using (FileStream fileStream = new FileStream(sourceFilepath, FileMode.Create))
				{
					byte[] bytes = uTF8Encoding.GetBytes(s);
					fileStream.Write(bytes, 0, bytes.Length);
<<<<<<< HEAD
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
						{
							byte[] bytes2 = uTF8Encoding.GetBytes(preprocessedSource);
							gZipStream.Write(bytes2, 0, bytes2.Length);
						}
						byte[] array = memoryStream.ToArray();
						fileStream.Write(array, 0, array.Length);
					}
				}
				sourceFilepath = GetCacheFilepath(MyFileSystem.UserDataPath, "ShaderCache2", hashFromString, profile);
				using (FileStream fileStream2 = new FileStream(sourceFilepath, FileMode.Create))
				{
					byte[] bytes3 = uTF8Encoding.GetBytes(s);
					fileStream2.Write(bytes3, 0, bytes3.Length);
					fileStream2.Write(cache, 0, cache.Length);
				}
=======
					using MemoryStream memoryStream = new MemoryStream();
					GZipStream val = new GZipStream((Stream)memoryStream, (CompressionMode)1);
					try
					{
						byte[] bytes2 = uTF8Encoding.GetBytes(preprocessedSource);
						((Stream)(object)val).Write(bytes2, 0, bytes2.Length);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
					byte[] array = memoryStream.ToArray();
					fileStream.Write(array, 0, array.Length);
				}
				sourceFilepath = GetCacheFilepath(MyFileSystem.UserDataPath, "ShaderCache2", hashFromString, profile);
				using FileStream fileStream2 = new FileStream(sourceFilepath, FileMode.Create);
				byte[] bytes3 = uTF8Encoding.GetBytes(s);
				fileStream2.Write(bytes3, 0, bytes3.Length);
				fileStream2.Write(cache, 0, cache.Length);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch (IOException)
			{
				Thread.Sleep(10);
				Store(preprocessedSource, profile, cache);
			}
		}
	}
}
