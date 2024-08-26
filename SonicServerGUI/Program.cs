using System;
using System.Windows.Forms;

namespace SonicServer.GUI
{
	internal static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			/*Thread d = new Thread(() =>
			{
				Thread.Sleep(2000);
				Console.WriteLine("KYS");
			});
			d.Start();*/
			Application.Run(new MainForm());
		}
	}
}
