using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Linq;

namespace VRage.Compression
{
	/// <summary>
	/// Class based on http://www.codeproject.com/Articles/209731/Csharp-use-Zip-archives-without-external-libraries.
	/// </summary>
	public class MyZipArchive : IDisposable
	{
		private readonly ZipArchive m_zip;

		private readonly Dictionary<string, string> m_mixedCaseHelper = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

		public string ZipPath { get; private set; }

<<<<<<< HEAD
		public IEnumerable<string> FileNames => from p in Files
			select FixName(p.FullName) into p
			orderby p
			select p;
=======
		public IEnumerable<string> FileNames => (IEnumerable<string>)Enumerable.OrderBy<string, string>(Enumerable.Select<ZipArchiveEntry, string>((IEnumerable<ZipArchiveEntry>)Files, (Func<ZipArchiveEntry, string>)((ZipArchiveEntry p) => FixName(p.FullName))), (Func<string, string>)((string p) => p));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public ReadOnlyCollection<ZipArchiveEntry> Files => m_zip.Entries;

		private MyZipArchive(ZipArchive zipObject, string path = null)
		{
			m_zip = zipObject;
			ZipPath = path;
			if (m_zip.Mode == ZipArchiveMode.Create)
			{
				return;
			}
			foreach (ZipArchiveEntry file in Files)
			{
				m_mixedCaseHelper[FixName(file.FullName)] = file.FullName;
			}
		}

		private static string FixName(string name)
		{
			return name.Replace('/', '\\');
		}

		public static MyZipArchive OpenOnFile(string path, ZipArchiveMode mode = ZipArchiveMode.Read)
		{
			return new MyZipArchive(ZipFile.Open(path, mode), path);
		}

		public MyZipFileInfo AddFile(string path, CompressionLevel level)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return new MyZipFileInfo(m_zip.CreateEntry(path, level));
		}

		public MyZipFileInfo GetFile(string name)
		{
			return new MyZipFileInfo(m_zip.GetEntry(m_mixedCaseHelper[FixName(name)]));
		}

		public bool FileExists(string name)
		{
			return m_mixedCaseHelper.ContainsKey(FixName(name));
		}

		public bool DirectoryExists(string name)
		{
			name = FixName(name);
			foreach (string key in m_mixedCaseHelper.Keys)
			{
				if (key.StartsWith(name, StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}
			return false;
		}

		public void Dispose()
		{
			m_zip.Dispose();
		}

		public static void ExtractToDirectory(string sourceArchiveFileName, string destinationDirectoryName)
		{
			ZipFile.ExtractToDirectory(sourceArchiveFileName, destinationDirectoryName);
		}
	}
}
