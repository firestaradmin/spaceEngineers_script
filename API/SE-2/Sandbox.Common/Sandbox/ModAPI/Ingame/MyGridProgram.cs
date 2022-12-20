using System;
using System.Reflection;

namespace Sandbox.ModAPI.Ingame
{
	/// <summary>
	///     All programmable block scripts derive from this class, meaning that all properties in this
	///     class are directly available for use in your scripts.
	///     If you use Visual Studio or other external editors to write your scripts, you can derive
	///     directly from this class and have a compatible script template.
	/// </summary>
	/// <example>
	///     <code>
	/// public void Main()
	/// {
	///     // Print out the time elapsed since the currently running programmable block was run
	///     // the last time.
	///     Echo(Me.CustomName + " was last run " + Runtime.TimeSinceLastRun.TotalSeconds + " seconds ago.");
	/// }
	/// </code>
	/// </example>
	public abstract class MyGridProgram : IMyGridProgram
	{
		private string m_storage;

		private readonly Action<string, UpdateType> m_main;

		private readonly Action m_save;

		private Func<IMyIntergridCommunicationSystem> m_IGC_ContextGetter;

		/// <summary>
		///     Provides access to the grid terminal system as viewed from this programmable block.
		/// </summary>
		public IMyGridTerminalSystem GridTerminalSystem { get; protected set; }

		/// <summary>
		///     Gets a reference to the currently running programmable block.
		/// </summary>
		public IMyProgrammableBlock Me { get; protected set; }
<<<<<<< HEAD
=======

		/// <summary>
		///     Gets the amount of in-game time elapsed from the previous run.
		/// </summary>
		[Obsolete("Use Runtime.TimeSinceLastRun instead")]
		public TimeSpan ElapsedTime { get; protected set; }
>>>>>>> d46cf8619665219cc163a7b28984ced59ed9470d

		/// <summary>
		/// Gets runtime information for the running grid program.
		/// </summary>
		public IMyGridProgramRuntimeInfo Runtime { get; protected set; }

		/// <summary>
		///     Allows you to store data between game sessions.
		/// </summary>
		public string Storage
		{
			get
			{
				return m_storage ?? "";
			}
			protected set
			{
				m_storage = value ?? "";
			}
		}

		/// <summary>
		///     Prints out text onto the currently running programmable block's detail info area.
		/// </summary>
		public Action<string> Echo { get; protected set; }

		Func<IMyIntergridCommunicationSystem> IMyGridProgram.IGC_ContextGetter
		{
			set
			{
				m_IGC_ContextGetter = value;
			}
		}

		public IMyIntergridCommunicationSystem IGC => m_IGC_ContextGetter();

		IMyGridTerminalSystem IMyGridProgram.GridTerminalSystem
		{
			get
			{
				return GridTerminalSystem;
			}
			set
			{
				GridTerminalSystem = value;
			}
		}

		IMyProgrammableBlock IMyGridProgram.Me
		{
			get
			{
				return Me;
			}
			set
			{
				Me = value;
			}
		}

		string IMyGridProgram.Storage
		{
			get
			{
				return Storage;
			}
			set
			{
				Storage = value;
			}
		}

		Action<string> IMyGridProgram.Echo
		{
			get
			{
				return Echo;
			}
			set
			{
				Echo = value;
			}
		}

		IMyGridProgramRuntimeInfo IMyGridProgram.Runtime
		{
			get
			{
				return Runtime;
			}
			set
			{
				Runtime = value;
			}
		}

		bool IMyGridProgram.HasMainMethod => m_main != null;

		bool IMyGridProgram.HasSaveMethod => m_save != null;

		protected MyGridProgram()
		{
			Type type = GetType();
			MethodInfo method = type.GetMethod("Main", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[2]
			{
				typeof(string),
				typeof(UpdateType)
			}, null);
			if (method != null)
			{
				m_main = MethodInfoExtensions.CreateDelegate<Action<string, UpdateType>>(method, this);
			}
			else
			{
				method = type.GetMethod("Main", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[1] { typeof(string) }, null);
				if (method != null)
				{
					Action<string> main = MethodInfoExtensions.CreateDelegate<Action<string>>(method, this);
					m_main = delegate(string arg, UpdateType source)
					{
						main(arg);
					};
				}
				else
				{
					method = type.GetMethod("Main", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
					if (method != null)
					{
						Action mainWithoutArgument = MethodInfoExtensions.CreateDelegate<Action>(method, this);
						m_main = delegate
						{
							mainWithoutArgument();
						};
					}
				}
			}
			MethodInfo method2 = type.GetMethod("Save", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (method2 != null)
			{
				m_save = MethodInfoExtensions.CreateDelegate<Action>(method2, this);
			}
		}

		[Obsolete]
		void IMyGridProgram.Main(string argument)
		{
			if (m_main == null)
			{
				throw new InvalidOperationException("No Main method available");
			}
			m_main(argument ?? string.Empty, UpdateType.Mod);
		}

		void IMyGridProgram.Main(string argument, UpdateType updateSource)
		{
			if (m_main == null)
			{
				throw new InvalidOperationException("No Main method available");
			}
			m_main(argument ?? string.Empty, updateSource);
		}

		void IMyGridProgram.Save()
		{
			if (m_save != null)
			{
				m_save();
			}
		}
	}
}
