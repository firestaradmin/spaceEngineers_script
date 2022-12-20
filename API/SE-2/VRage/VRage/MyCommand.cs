using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VRage
{
	public abstract class MyCommand
	{
		protected class MyCommandAction
		{
			public StringBuilder AutocompleteHint = new StringBuilder("");

			public ParserDelegate Parser;

			public ActionDelegate CallAction;
		}

		protected Dictionary<string, MyCommandAction> m_methods;

		public List<string> Methods => Enumerable.ToList<string>((IEnumerable<string>)m_methods.Keys);

		public abstract string Prefix();

		public MyCommand()
		{
			m_methods = new Dictionary<string, MyCommandAction>();
		}

		public StringBuilder Execute(string method, List<string> args)
		{
			if (m_methods.TryGetValue(method, out var value))
			{
				try
				{
					MyCommandArgs commandArgs = value.Parser(args);
					return value.CallAction(commandArgs);
				}
				catch
				{
					throw new MyConsoleInvalidArgumentsException();
				}
			}
			throw new MyConsoleMethodNotFoundException();
		}

		public StringBuilder GetHint(string method)
		{
			if (m_methods.TryGetValue(method, out var value))
			{
				return value.AutocompleteHint;
			}
			return null;
		}
	}
}
