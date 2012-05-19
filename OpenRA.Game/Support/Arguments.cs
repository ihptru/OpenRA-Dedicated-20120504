#region Copyright & License Information
/*
 * Copyright 2007-2011 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace OpenRA
{
	public class Arguments
	{
		Dictionary<string, string> args = new Dictionary<string, string>();
		List<string> flags = new List<string>();
		
		public static Arguments Empty { get { return new Arguments(); } }

		public Arguments(params string[] src)
		{
			Regex regexPairs = new Regex ("([^=]+)=(.*)");
			Regex regexFlags = new Regex ("--(.+)");
			foreach (string s in src)
			{
				Match m = regexPairs.Match (s);
				if (!(m == null || !m.Success))
				{
					args [m.Groups [1].Value] = m.Groups [2].Value;
				}
				m = regexFlags.Match (s);
				if (!(m == null || !m.Success))
				{
					flags.Add (m.Groups [1].Value);
				}
			}

		}
		
		public bool ContainsFlag(string flag)
		{
			return flags.Contains (flag);
		}
		public bool Contains(string key) { return args.ContainsKey(key); }
		public string GetValue(string key, string defaultValue) { return Contains(key) ? args[key] : defaultValue; }
	}
}
