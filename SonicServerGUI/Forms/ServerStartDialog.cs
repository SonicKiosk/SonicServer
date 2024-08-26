using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace SonicServer.GUI.Forms
{
	public partial class ServerStartDialog : Form
	{
		public IPAddress ChosenIP;
		public int ChosenPort;
		public ServerStartDialog()
		{
			InitializeComponent();
		}
		private void ServerStartDialog_Load(object sender, EventArgs e)
		{
			IPAddress[] addresses = Array.FindAll(
				Dns.GetHostEntry(Dns.GetHostName()).AddressList,
				(x) => x.AddressFamily == AddressFamily.InterNetwork);
			foreach (var item in addresses)
			{
				IPcb.Items.Add(item.ToString());
			}
			IPcb.Text = IPcb.Items[0].ToString();
		}
		private void ChBoxAdvanced_CheckedChanged(object sender, EventArgs e)
		{
			if (ChBoxAdvanced.Checked)
			{
				NumPort.Minimum = 0;
				IPcb.DropDownStyle = ComboBoxStyle.Simple;
			}
			else
			{
				NumPort.Minimum = 1025;
				IPcb.DropDownStyle = ComboBoxStyle.DropDownList;
			}
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			ChosenIP = IPAddress.Parse(IPcb.Text);
			ChosenPort = Convert.ToInt32(NumPort.Value);
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}
}
