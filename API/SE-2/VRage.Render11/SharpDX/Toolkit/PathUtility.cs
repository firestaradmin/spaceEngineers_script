using System;
using System.Collections.Generic;

namespace SharpDX.Toolkit
{
	/// <summary>
	/// Path utility methods.
	/// </summary>
	internal class PathUtility
	{
		/// <summary>
		/// Transform a path by replacing '/' by '\' and transforming relative '..' or current path '.' to an absolute path. See remarks.
		/// </summary>
		/// <param name="path">A path string</param>
		/// <returns>A normalized path.</returns>
		/// <remarks>
		/// Unlike <see cref="T:System.IO.Path" /> , this doesn't make a path absolute to the actual file system.
		/// </remarks>
		public static string GetNormalizedPath(string path)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			path = path.Replace('/', '\\');
			string[] array = path.Split(new char[1] { '\\' }, StringSplitOptions.RemoveEmptyEntries);
<<<<<<< HEAD
			Stack<string> stack = new Stack<string>();
=======
			Stack<string> val = new Stack<string>();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (text == ".")
				{
					continue;
				}
				if (text == "..")
				{
<<<<<<< HEAD
					if (stack.Count == 0)
					{
						return null;
					}
					stack.Pop();
				}
				else
				{
					stack.Push(text);
				}
			}
			string[] array3 = stack.ToArray();
=======
					if (val.get_Count() == 0)
					{
						return null;
					}
					val.Pop();
				}
				else
				{
					val.Push(text);
				}
			}
			string[] array3 = val.ToArray();
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			Array.Reverse((Array)array3);
			return Utilities.Join("\\", array3);
		}
	}
}
