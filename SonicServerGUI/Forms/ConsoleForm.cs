using System;
using System.Drawing;
using System.Windows.Forms;
using SonicServer.GUI.SeverStuff;

namespace SonicServer.GUI
{
	public partial class ConsoleForm : Form
	{
		public ConsoleForm()
		{
			Console.SetOut(new ConsoleToFormTextWriter(ref richTextBox1, Color.Black));
			Console.SetError(new ConsoleToFormTextWriter(ref richTextBox1, Color.Red));
			InitializeComponent();
		}
	}
}
