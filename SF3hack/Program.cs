﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

static class Program
{
	public static Form2 parent;
	/// <summary>
	/// The main entry point for the application.
	/// </summary>
	[STAThread]
	static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(false);
		Application.Run(parent = new Form2());
	}
}
