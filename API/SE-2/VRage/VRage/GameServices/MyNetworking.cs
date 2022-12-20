using System;
using System.Collections.Generic;

namespace VRage.GameServices
{
	public static class MyNetworking
	{
		/// <summary>
		/// Extract a set of network parameters from an array of command line arguments.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public static List<string> CollectNetworkParameters(string[] args)
		{
			List<string> list = new List<string>();
			int num = 0;
			while ((num = Array.IndexOf(args, "-np", num)) > 0)
			{
				num++;
				if (num < args.Length)
				{
					list.Add(args[num]);
				}
			}
			return list;
		}
	}
}
