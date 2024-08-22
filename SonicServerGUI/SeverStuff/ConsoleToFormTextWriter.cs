using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace SonicServer.GUI.SeverStuff
{
	internal class ConsoleToFormTextWriter : TextWriter
	{
		private RichTextBox box;
		private Color color;
		public ConsoleToFormTextWriter(ref RichTextBox textbox, Color color)
		{
			this.box = textbox;
			this.color = color;
		}

		public override void Write(char value)
		{
			box.SelectionStart = box.TextLength;
			box.SelectionLength = 0;

			box.SelectionColor = color;
			box.AppendText(value.ToString());
			box.SelectionColor = box.ForeColor;
		}

		public override void Write(string value)
		{
			box.SelectionStart = box.TextLength;
			box.SelectionLength = 0;

			box.SelectionColor = color;
			box.AppendText(value);
			box.SelectionColor = box.ForeColor;
		}
		public override Encoding Encoding
		{
			get { return Encoding.ASCII; }
		}
	}
}
