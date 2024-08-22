using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System;

namespace SonicServer.GUI.SeverStuff
{
	internal class ConsoleToFormTextWriter : TextWriter
	{
		public delegate void WriteToBox(string text, Color color);
		private WriteToBox ToBox;
		private Color color;
		public ConsoleToFormTextWriter(WriteToBox textbox, Color color)
		{
			ToBox = textbox;
			this.color = color;
		}

		public override void Write(char value)
		{
			ToBox.Invoke(value.ToString(), color);
		}

		public override void Write(string value)
		{
			ToBox.Invoke(value, color);
		}
		public override void WriteLine(string value)
		{
			ToBox.Invoke(value, color);
		}
		public override Encoding Encoding
		{
			get { return Encoding.ASCII; }
		}
	}
}
