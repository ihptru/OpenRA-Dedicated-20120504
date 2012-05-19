#region Copyright & License Information
/*
 * Copyright 2007-2011 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace OpenRA
{
	static class Program
	{
		[STAThread]
		static void Main( string[] args )
		{
			// brutal hack
			Application.CurrentCulture = CultureInfo.InvariantCulture;

			if (Debugger.IsAttached || args.Contains("--just-die"))
			{
				Run(args);
				return;
			}

			try
			{
				Run( args );
			}
			catch( Exception e )
			{
				Log.AddChannel("exception", "exception.log");
				Log.Write("exception", "{0}", e.ToString());
				throw;
			}
		}

		static void Run( string[] args )
		{
			Game.Initialize( new Arguments(args) );
			GC.Collect();
			Game.Run();
		}
	}
}