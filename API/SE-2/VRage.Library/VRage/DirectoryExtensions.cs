using System;
using System.IO;

namespace VRage
{
	public static class DirectoryExtensions
	{
		public static void CopyAll(string source, string target)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			EnsureDirectoryExists(target);
			FileInfo[] files = new DirectoryInfo(source).GetFiles();
			foreach (FileInfo val in files)
			{
				val.CopyTo(Path.Combine(target, ((FileSystemInfo)val).get_Name()), true);
			}
			DirectoryInfo[] directories = new DirectoryInfo(source).GetDirectories();
			foreach (DirectoryInfo val2 in directories)
			{
				DirectoryInfo val3 = Directory.CreateDirectory(Path.Combine(target, ((FileSystemInfo)val2).get_Name()));
				CopyAll(((FileSystemInfo)val2).get_FullName(), ((FileSystemInfo)val3).get_FullName());
			}
		}

		public static void EnsureDirectoryExists(string path)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			DirectoryInfo val = new DirectoryInfo(path);
			if (val.get_Parent() != null)
			{
				EnsureDirectoryExists(((FileSystemInfo)val.get_Parent()).get_FullName());
			}
			if (!((FileSystemInfo)val).get_Exists())
			{
				val.Create();
			}
		}

		public static bool IsParentOf(this DirectoryInfo dir, string absPath)
		{
<<<<<<< HEAD
			string value = dir.FullName.TrimEnd(new char[1] { Path.DirectorySeparatorChar });
			DirectoryInfo directoryInfo = new DirectoryInfo(absPath);
			while (directoryInfo.Exists)
			{
				if (directoryInfo.FullName.TrimEnd(new char[1] { Path.DirectorySeparatorChar }).Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				if (!directoryInfo.FullName.TrimEnd(new char[1] { Path.DirectorySeparatorChar }).StartsWith(value))
=======
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			string value = ((FileSystemInfo)dir).get_FullName().TrimEnd(new char[1] { Path.DirectorySeparatorChar });
			DirectoryInfo val = new DirectoryInfo(absPath);
			while (((FileSystemInfo)val).get_Exists())
			{
				if (((FileSystemInfo)val).get_FullName().TrimEnd(new char[1] { Path.DirectorySeparatorChar }).Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
				if (!((FileSystemInfo)val).get_FullName().TrimEnd(new char[1] { Path.DirectorySeparatorChar }).StartsWith(value))
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
				{
					return false;
				}
				if (val.get_Parent() == null)
				{
					return false;
				}
				val = val.get_Parent();
			}
			return false;
		}

		public static ulong GetStorageSize(string sessionPath)
		{
<<<<<<< HEAD
			ulong num = 0uL;
			foreach (string item in Directory.EnumerateFileSystemEntries(sessionPath))
			{
				num = ((!Directory.Exists(item)) ? (num + (ulong)new FileInfo(item).Length) : (num + GetStorageSize(item)));
=======
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			ulong num = 0uL;
			foreach (string item in Directory.EnumerateFileSystemEntries(sessionPath))
			{
				num = ((!Directory.Exists(item)) ? (num + (ulong)new FileInfo(item).get_Length()) : (num + GetStorageSize(item)));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			return num;
		}
	}
}
