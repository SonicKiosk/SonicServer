using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SonicServer.GUI.Forms
{
	public partial class ServerStartDialog : SettingDialog
	{
		public IPAddress ChosenIP;
		public int ChosenPort;
		public ServerStartDialog()
		{
			InitializeComponent();
		}
		public override void AdvancedChanged(bool showAdvanced)
		{
			if (showAdvanced)
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
		public override void OKBtnClicked()
		{
			ChosenIP = IPAddress.Parse(IPcb.Text);
			ChosenPort = Convert.ToInt32(NumPort.Value);
		}
	}
}
