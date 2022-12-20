using System;
using System.Collections.Generic;
using System.IO;
using VRage.Collections;

namespace VRage.FileSystem
{
	public class MyFileProviderAggregator : IFileProvider
	{
		private HashSet<IFileProvider> m_providers = new HashSet<IFileProvider>();

		public HashSetReader<IFileProvider> Providers => new HashSetReader<IFileProvider>(m_providers);

		public MyFileProviderAggregator(params IFileProvider[] providers)
		{
			foreach (IFileProvider provider in providers)
			{
				AddProvider(provider);
			}
		}

		public void AddProvider(IFileProvider provider)
		{
			m_providers.Add(provider);
		}

		public void RemoveProvider(IFileProvider provider)
		{
			m_providers.Remove(provider);
		}

		public Stream OpenRead(string path)
		{
			return Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		public Stream OpenWrite(string path, FileMode mode = FileMode.OpenOrCreate)
		{
			return Open(path, mode, FileAccess.Write, FileShare.Read);
		}

		/// <summary>
		/// Opens file, returns null when file does not exists or cannot be opened
		/// </summary>
		public Stream Open(string path, FileMode mode, FileAccess access, FileShare share)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<IFileProvider> enumerator = m_providers.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IFileProvider current = enumerator.get_Current();
					try
					{
						Stream stream = current.Open(path, mode, access, share);
						if (stream != null)
						{
							return stream;
						}
					}
					catch
					{
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return null;
		}

		public bool DirectoryExists(string path)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<IFileProvider> enumerator = m_providers.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IFileProvider current = enumerator.get_Current();
					try
					{
						if (current.DirectoryExists(path))
						{
							return true;
						}
					}
					catch
					{
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return false;
		}

		public IEnumerable<string> GetFiles(string path, string filter, MySearchOption searchOption)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Enumerator<IFileProvider> enumerator = m_providers.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IFileProvider current = enumerator.get_Current();
					try
					{
						IEnumerable<string> files = current.GetFiles(path, filter, searchOption);
						if (files != null)
						{
							return files;
						}
					}
					catch
					{
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return null;
		}

		public bool FileExists(string path)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrWhiteSpace(path))
			{
				return false;
			}
			Enumerator<IFileProvider> enumerator = m_providers.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					IFileProvider current = enumerator.get_Current();
					try
					{
						if (current.FileExists(path))
						{
							return true;
						}
					}
					catch
					{
					}
				}
			}
			finally
			{
				((IDisposable)enumerator).Dispose();
			}
			return false;
		}
	}
}
