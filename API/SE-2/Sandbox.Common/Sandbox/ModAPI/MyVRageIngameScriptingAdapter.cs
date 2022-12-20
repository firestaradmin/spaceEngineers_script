using System;
using VRage.Collections;
using VRage.Scripting;

namespace Sandbox.ModAPI
{
<<<<<<< HEAD
	/// <summary>
	/// Allows mods change programmable block script settings
	/// </summary>
	public class MyVRageIngameScriptingAdapter : IMyIngameScripting, IMyScriptBlacklist
	{
		/// <summary>
		/// Allows blacklist for program block script to use certain namespaces, types or methods
		/// </summary>
=======
	public class MyVRageIngameScriptingAdapter : IMyIngameScripting, IMyScriptBlacklist
	{
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		private class MyScriptBlacklistBatchAdapter : IMyScriptBlacklistBatch, IDisposable
		{
			private readonly VRage.Scripting.IMyScriptBlacklistBatch m_batch;

			public MyScriptBlacklistBatchAdapter(VRage.Scripting.IMyScriptBlacklistBatch batch)
			{
				m_batch = batch;
			}

<<<<<<< HEAD
			/// <summary>
			/// Adds to blacklist for program block script some namespaces of provided type
			/// </summary>
			/// <param name="types">Namespace names would be taken from this types</param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public void AddNamespaceOfTypes(params Type[] types)
			{
				m_batch.AddNamespaceOfTypes(types);
			}

<<<<<<< HEAD
			/// <summary>
			/// Removes from blacklist for program block script some namespaces of provided type
			/// </summary>
			/// <param name="types">Namespace names would be taken from this types</param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public void RemoveNamespaceOfTypes(params Type[] types)
			{
				m_batch.RemoveNamespaceOfTypes(types);
			}

<<<<<<< HEAD
			/// <summary>
			/// Adds to blacklist for program block script certain types
			/// </summary>
			/// <param name="types">Blacklisted types</param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public void AddTypes(params Type[] types)
			{
				m_batch.AddTypes(types);
			}

<<<<<<< HEAD
			/// <summary>
			/// Removes from blacklist for program block script certain types
			/// </summary>
			/// <param name="types">Blacklisted types</param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public void RemoveTypes(params Type[] types)
			{
				m_batch.RemoveTypes(types);
			}

<<<<<<< HEAD
			/// <summary>
			/// Adds to blacklist for program block script certain type members
			/// </summary>
			/// <param name="type">Type to blacklist</param>
			/// <param name="memberNames">Blacklisted members</param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public void AddMembers(Type type, params string[] memberNames)
			{
				m_batch.AddMembers(type, memberNames);
			}

<<<<<<< HEAD
			/// <summary>
			/// Removes from blacklist for program block script certain type members
			/// </summary>
			/// <param name="type">Type to blacklist</param>
			/// <param name="memberNames">Blacklisted members</param>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
			public void RemoveMembers(Type type, params string[] memberNames)
			{
				m_batch.RemoveMembers(type, memberNames);
			}

			public void Dispose()
			{
				m_batch.Dispose();
			}
		}

		private readonly VRage.Scripting.IMyIngameScripting m_scripting;

<<<<<<< HEAD
		/// <summary>
		/// Provides the ability for mods to add and remove items from a type and member blacklist,
		/// giving the ability to remove even more API for scripts. Intended for server admins to
		/// restrict what people are able to do with scripts to keep their simspeed up.
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public IMyScriptBlacklist ScriptBlacklist { get; }

		private VRage.Scripting.IMyScriptBlacklist BlackList => m_scripting.ScriptBlacklist;

		public MyVRageIngameScriptingAdapter(VRage.Scripting.IMyIngameScripting impl)
		{
			m_scripting = impl;
			ScriptBlacklist = this;
		}

<<<<<<< HEAD
		/// <summary>
		/// Clears all <see cref="P:Sandbox.ModAPI.MyVRageIngameScriptingAdapter.ScriptBlacklist" /> changes 
		/// </summary>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public void Clean()
		{
			m_scripting?.Clean();
		}

<<<<<<< HEAD
		/// <summary>
		/// Gets information about whitelisted classes and methods
		/// </summary>
		/// <returns>Dictionary reader, where key - name of namespace, type of method, and value - in which context it is allowed</returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public DictionaryReader<string, MyWhitelistTarget> GetWhitelist()
		{
			return BlackList.GetWhitelist();
		}

<<<<<<< HEAD
		/// <summary>
		/// Gets the entries that have been blacklisted for the ingame scripts.
		/// </summary>
		/// <returns>Hashset reader of namespaces that were whitelisted</returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public HashSetReader<string> GetBlacklistedIngameEntries()
		{
			return BlackList.GetBlacklistedIngameEntries();
		}

<<<<<<< HEAD
		/// <summary>
		/// Opens a batch to add or remove members to the blacklist.
		/// </summary>
		/// <returns>Object allowing you change blacklisted members/types/namespaces.</returns>
=======
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d
		public IMyScriptBlacklistBatch OpenIngameBlacklistBatch()
		{
			return new MyScriptBlacklistBatchAdapter(BlackList.OpenIngameBlacklistBatch());
		}
	}
}
