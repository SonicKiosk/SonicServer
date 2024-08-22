using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using SonicServer.GUI.SeverStuff;

namespace SonicServer.GUI
{
	public partial class ConsoleForm : Form
	{
		private delegate void SafeCallDelegate(string text, Color color);
		private void WriteTextSafe(string text, Color color)
		{
			if (richTextBox1.InvokeRequired)
			{
				var d = new SafeCallDelegate(WriteTextSafe);
				richTextBox1.Invoke(d, new object[] { text, color });
			}
			else
			{
				richTextBox1.SelectionStart = richTextBox1.TextLength;
				richTextBox1.SelectionLength = 0;

				richTextBox1.SelectionColor = color;
				richTextBox1.AppendText($"{DateTime.Now:HH:mm:ss} - {text}");
				richTextBox1.SelectionColor = richTextBox1.ForeColor;
			}
		}
		public ConsoleForm()
		{
			Console.SetOut(new ConsoleToFormTextWriter(WriteTextSafe, Color.Black));
			Console.SetError(new ConsoleToFormTextWriter(WriteTextSafe, Color.Red));
			InitializeComponent();
			richTextBox1.Text = "";
			this.FormClosing += ConsoleForm_FormClosing;
		}

		private void ConsoleForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Visible = false;
			e.Cancel = true;
		}
	}
}
