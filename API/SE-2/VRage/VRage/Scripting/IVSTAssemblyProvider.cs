using System.Collections.Generic;
using System.Reflection;

namespace VRage.Scripting
{
	public interface IVSTAssemblyProvider
	{
		bool DebugEnabled { get; set; }

		bool TryLoad(string assemblyName, bool checkFileExists);

		void Init(IEnumerable<string> fileNames, string localModPath);

		Assembly GetAssembly();
	}
}
