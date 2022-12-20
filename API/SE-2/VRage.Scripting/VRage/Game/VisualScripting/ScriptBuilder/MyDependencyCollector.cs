using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis;
using VRage.Collections;

namespace VRage.Game.VisualScripting.ScriptBuilder
{
	public class MyDependencyCollector
	{
		private HashSet<MetadataReference> m_references;

		public HashSetReader<MetadataReference> References => new HashSetReader<MetadataReference>(m_references);

		public MyDependencyCollector(IEnumerable<Assembly> assemblies)
			: this()
		{
			foreach (Assembly assembly in assemblies)
			{
				CollectReferences(assembly);
			}
		}

		public MyDependencyCollector()
		{
			m_references = new HashSet<MetadataReference>();
			try
			{
				Assembly assembly = Assembly.Load("netstandard");
<<<<<<< HEAD
				m_references.Add(MetadataReference.CreateFromFile(assembly.Location));
=======
				m_references.Add((MetadataReference)MetadataReference.CreateFromFile(assembly.Location));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
			catch
			{
			}
<<<<<<< HEAD
			m_references.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
=======
			m_references.Add((MetadataReference)MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		}

		public void CollectReferences(Assembly assembly)
		{
			if (!(assembly == null))
			{
				AssemblyName[] referencedAssemblies = assembly.GetReferencedAssemblies();
				for (int i = 0; i < referencedAssemblies.Length; i++)
				{
					Assembly assembly2 = Assembly.Load(referencedAssemblies[i]);
<<<<<<< HEAD
					m_references.Add(MetadataReference.CreateFromFile(assembly2.Location));
				}
				m_references.Add(MetadataReference.CreateFromFile(assembly.Location));
=======
					m_references.Add((MetadataReference)MetadataReference.CreateFromFile(assembly2.Location));
				}
				m_references.Add((MetadataReference)MetadataReference.CreateFromFile(assembly.Location));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void RegisterAssembly(Assembly assembly)
		{
			if (assembly != null)
			{
<<<<<<< HEAD
				m_references.Add(MetadataReference.CreateFromFile(assembly.Location));
=======
				m_references.Add((MetadataReference)MetadataReference.CreateFromFile(assembly.Location));
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}
	}
}
