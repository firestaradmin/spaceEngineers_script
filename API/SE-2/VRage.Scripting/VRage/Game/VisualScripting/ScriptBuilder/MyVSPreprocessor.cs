<<<<<<< HEAD
=======
using System;
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRage.FileSystem;
using VRage.Game.ObjectBuilders.VisualScripting;
using VRage.ObjectBuilders;
using VRage.Utils;

namespace VRage.Game.VisualScripting.ScriptBuilder
{
	public class MyVSPreprocessor
	{
		private readonly HashSet<string> m_filePaths = new HashSet<string>();

		private readonly HashSet<string> m_classNames = new HashSet<string>();

<<<<<<< HEAD
		public string[] ClassNames => m_classNames.ToArray();
=======
		public string[] ClassNames => Enumerable.ToArray<string>((IEnumerable<string>)m_classNames);
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		public string[] FileSet
		{
			get
			{
<<<<<<< HEAD
				string[] array = new string[m_filePaths.Count];
				int num = 0;
				foreach (string filePath in m_filePaths)
				{
					array[num++] = filePath;
				}
				return array;
=======
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				string[] array = new string[m_filePaths.get_Count()];
				int num = 0;
				Enumerator<string> enumerator = m_filePaths.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.get_Current();
						array[num++] = current;
					}
					return array;
				}
				finally
				{
					((IDisposable)enumerator).Dispose();
				}
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			}
		}

		public void AddFile(string filePath, string localModPath)
		{
			if (filePath == null || !m_filePaths.Add(filePath))
			{
				return;
			}
			MyObjectBuilder_VSFiles objectBuilder = null;
			new MyContentPath(filePath, localModPath);
			using (Stream stream = MyFileSystem.OpenRead(filePath))
			{
				if (stream == null)
				{
					MyLog.Default.WriteLine("VisualScripting Preprocessor: " + filePath + " is Missing.");
				}
				if (!MyObjectBuilderSerializer.DeserializeXML(stream, out objectBuilder))
				{
					m_filePaths.Remove(filePath);
					return;
				}
			}
			if (objectBuilder.VisualScript != null)
			{
				if (m_classNames.Add(objectBuilder.VisualScript.Name))
				{
					foreach (string dependencyFilePath in objectBuilder.VisualScript.DependencyFilePaths)
					{
						string exitingFilePath = new MyContentPath(dependencyFilePath, localModPath).GetExitingFilePath();
						if (string.IsNullOrEmpty(exitingFilePath))
						{
							string message = "Missing visual script dependency file " + dependencyFilePath;
							MyLog.Default.Error(message);
						}
						else
						{
							AddFile(exitingFilePath, localModPath);
						}
					}
				}
				else
				{
					m_filePaths.Remove(filePath);
				}
			}
			if (objectBuilder.StateMachine != null)
			{
				MyObjectBuilder_ScriptSMNode[] nodes = objectBuilder.StateMachine.Nodes;
				foreach (MyObjectBuilder_ScriptSMNode myObjectBuilder_ScriptSMNode in nodes)
				{
					if (!(myObjectBuilder_ScriptSMNode is MyObjectBuilder_ScriptSMSpreadNode) && !(myObjectBuilder_ScriptSMNode is MyObjectBuilder_ScriptSMBarrierNode) && !string.IsNullOrEmpty(myObjectBuilder_ScriptSMNode.ScriptFilePath))
					{
						string exitingFilePath2 = new MyContentPath(myObjectBuilder_ScriptSMNode.ScriptFilePath, localModPath).GetExitingFilePath();
						if (string.IsNullOrEmpty(exitingFilePath2))
						{
							string message2 = "Missing mission script file " + myObjectBuilder_ScriptSMNode.ScriptFilePath;
							MyLog.Default.Error(message2);
						}
						else
						{
							AddFile(exitingFilePath2, localModPath);
						}
					}
				}
				m_filePaths.Remove(filePath);
			}
			if (objectBuilder.LevelScript == null)
			{
				return;
			}
			if (m_classNames.Add(objectBuilder.LevelScript.Name))
			{
				foreach (string dependencyFilePath2 in objectBuilder.LevelScript.DependencyFilePaths)
				{
					MyContentPath myContentPath = new MyContentPath(dependencyFilePath2, localModPath);
					if (string.IsNullOrEmpty(myContentPath.GetExitingFilePath()))
					{
						string message3 = "Missing objective dependency file " + dependencyFilePath2;
						MyLog.Default.Error(message3);
					}
					else
					{
						AddFile(myContentPath.GetExitingFilePath(), localModPath);
					}
				}
			}
			else
			{
				m_filePaths.Remove(filePath);
			}
		}

		public void Clear()
		{
			m_filePaths.Clear();
			m_classNames.Clear();
		}
	}
}
