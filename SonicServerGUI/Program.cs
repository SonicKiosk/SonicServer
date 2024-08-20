using SimpleLogs4Net;
using System;
using System.IO;
using System.Windows.Forms;

namespace SonicServer.GUI
{
	internal static class Program
	{
		[STAThread]
		static void Main()
		{
			LogConfiguration.Initialize(AppDomain.CurrentDomain.BaseDirectory + "Logs" + Path.DirectorySeparatorChar, OutputStream.Both);
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
