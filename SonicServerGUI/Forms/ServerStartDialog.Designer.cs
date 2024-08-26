namespace SonicServer.GUI.Forms
{
	partial class ServerStartDialog
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.btnStart = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.NumPort = new System.Windows.Forms.NumericUpDown();
			this.ChBoxAdvanced = new System.Windows.Forms.CheckBox();
			this.IPcb = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.NumPort)).BeginInit();
			this.SuspendLayout();
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(467, 343);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 0;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(548, 343);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(634, 60);
			this.label1.TabIndex = 2;
			this.label1.Text = "To start the server you need to set an IP and port";
			// 
			// NumPort
			// 
			this.NumPort.Location = new System.Drawing.Point(91, 109);
			this.NumPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.NumPort.Minimum = new decimal(new int[] {
            1025,
            0,
            0,
            0});
			this.NumPort.Name = "NumPort";
			this.NumPort.Size = new System.Drawing.Size(120, 20);
			this.NumPort.TabIndex = 3;
			this.NumPort.Value = new decimal(new int[] {
            8005,
            0,
            0,
            0});
			// 
			// ChBoxAdvanced
			// 
			this.ChBoxAdvanced.AutoSize = true;
			this.ChBoxAdvanced.Location = new System.Drawing.Point(356, 347);
			this.ChBoxAdvanced.Name = "ChBoxAdvanced";
			this.ChBoxAdvanced.Size = new System.Drawing.Size(105, 17);
			this.ChBoxAdvanced.TabIndex = 4;
			this.ChBoxAdvanced.Text = "Show Advanced";
			this.ChBoxAdvanced.UseVisualStyleBackColor = true;
			this.ChBoxAdvanced.CheckedChanged += new System.EventHandler(this.ChBoxAdvanced_CheckedChanged);
			// 
			// IPcb
			// 
			this.IPcb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.IPcb.FormattingEnabled = true;
			this.IPcb.Location = new System.Drawing.Point(90, 77);
			this.IPcb.Name = "IPcb";
			this.IPcb.Size = new System.Drawing.Size(121, 21);
			this.IPcb.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(20, 80);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "IP Address: ";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(20, 111);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(26, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "Port";
			// 
			// ServerStartDialog
			// 
			this.AcceptButton = this.btnStart;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(635, 378);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.IPcb);
			this.Controls.Add(this.ChBoxAdvanced);
			this.Controls.Add(this.NumPort);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnStart);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ServerStartDialog";
			this.Text = "ServerStartDialog";
			this.Load += new System.EventHandler(this.ServerStartDialog_Load);
			((System.ComponentModel.ISupportInitialize)(this.NumPort)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown NumPort;
		private System.Windows.Forms.CheckBox ChBoxAdvanced;
		private System.Windows.Forms.ComboBox IPcb;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
	}
}