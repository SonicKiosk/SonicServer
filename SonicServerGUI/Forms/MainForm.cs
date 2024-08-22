using SonicServer.GUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SonicServer.GUI
{
	public partial class MainForm : Form
	{
		public ConsoleForm consoleForm1;
		public MainForm()
		{
			this.FormClosing += MainForm_FormClosing;
			InitializeComponent();
			consoleForm1 = new ConsoleForm();
			consoleForm1.FormClosed += Console_FormClosed;
			consoleForm1.Shown += Console_Shown;
#if DEBUG
			consoleForm1.Show();
#endif
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (Server.IsStarted)
			{
				DialogResult result = MessageBox.Show(
					"The Server is still running!\nAre you sure you want to exit?", 
					"Sonic Server", //title
					MessageBoxButtons.OKCancel, 
					MessageBoxIcon.Warning
				);
                if (result == DialogResult.OK)
                {
                    Server.Stop();
                }
				else
				{
					e.Cancel = true;
				}
            }
		}

		private void Console_Shown(object sender, EventArgs e)
		{
			consoleToolStripMenuItem.Checked = true;
		}

		private void Console_FormClosed(object sender, FormClosedEventArgs e)
		{
			consoleToolStripMenuItem.Checked = false;
		}

		private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			consoleToolStripMenuItem.Checked = !consoleToolStripMenuItem.Checked;
			consoleForm1.Visible = consoleToolStripMenuItem.Checked;
		}

		private void startServerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Server.IsStarted)
			{
				Server.Stop();
				startServerToolStripMenuItem.Text = "Start Server";
				return;
			}
			ServerStartDialog startDialog = new();
			DialogResult result = startDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				Server.Start(startDialog.ChosenIP, startDialog.ChosenPort);
				startServerToolStripMenuItem.Text = "Stop Server";
			}
		}
	}
}
