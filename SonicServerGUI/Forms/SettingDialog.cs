using System;
using System.Windows.Forms;

namespace SonicServer.GUI.Forms
{
	public partial class SettingDialog : Form
	{
		public string UpperTip = "";
		public string OKButtonText = "OK";
		public string CancelButtonText = "Cancel";
		public SettingDialog() {
			this.btnOK.Click += new System.EventHandler((obj, e) => { this.OKBtnClicked(); });
			this.btnCancel.Click += new System.EventHandler((obj, e) => { this.CancelBtnClicked(); });
			this.ChBoxAdvanced.CheckedChanged += new System.EventHandler((obj, e) => this.AdvancedChanged(this.ChBoxAdvanced.Checked));
			InitializeComponent();
			this.btnOK.Text = OKButtonText;
			this.btnCancel.Text = CancelButtonText;
			this.label1.Text = UpperTip;
		}
		public virtual void AdvancedChanged(bool showAdvanced) { }
		public virtual void CancelBtnClicked() {
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		public virtual void OKBtnClicked() {
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
