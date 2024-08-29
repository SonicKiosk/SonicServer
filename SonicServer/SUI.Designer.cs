﻿using System.Drawing;
using System.Windows.Forms;

namespace ServerUI
{
    partial class SUI
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>

        [STAThread]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            dataBox = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            button1 = new Button();
            checkInBtn = new Button();
            infoEditorBtn = new Button();
            createOrderBtn = new Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            infoIndicatorLbl = new Label();
            activeClientID = new Label();
            mainContent = new TabControl();
            tabPage1 = new TabPage();
            button2 = new Button();
            listView1 = new ListView();
            tabPage2 = new TabPage();
            tabPage3 = new TabPage();
            updatelist = new System.Windows.Forms.Timer(components);
            clientList = new ListBox();
            checkBox1 = new CheckBox();
            IPLabel = new Label();
            openFileDialog1 = new OpenFileDialog();
            lastnametxt = new TextBox();
            dataBox.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            mainContent.SuspendLayout();
            tabPage1.SuspendLayout();
            SuspendLayout();
            // 
            // dataBox
            // 
            dataBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataBox.Controls.Add(tableLayoutPanel1);
            dataBox.Controls.Add(tableLayoutPanel2);
            dataBox.Location = new Point(214, 12);
            dataBox.Name = "dataBox";
            dataBox.Size = new Size(574, 100);
            dataBox.TabIndex = 1;
            dataBox.TabStop = false;
            dataBox.Text = "Data";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(button1, 1, 0);
            tableLayoutPanel1.Controls.Add(checkInBtn, 0, 0);
            tableLayoutPanel1.Controls.Add(infoEditorBtn, 3, 0);
            tableLayoutPanel1.Controls.Add(createOrderBtn, 2, 0);
            tableLayoutPanel1.Location = new Point(4, 52);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(566, 42);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Fill;
            button1.ForeColor = Color.FromArgb(200, 0, 0);
            button1.Location = new Point(144, 3);
            button1.Name = "button1";
            button1.Size = new Size(135, 36);
            button1.TabIndex = 3;
            button1.Text = "Check Out";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // checkInBtn
            // 
            checkInBtn.Dock = DockStyle.Fill;
            checkInBtn.ForeColor = Color.FromArgb(0, 200, 0);
            checkInBtn.Location = new Point(3, 3);
            checkInBtn.Name = "checkInBtn";
            checkInBtn.Size = new Size(135, 36);
            checkInBtn.TabIndex = 0;
            checkInBtn.Text = "Check In";
            checkInBtn.UseVisualStyleBackColor = true;
            checkInBtn.Click += checkInBtn_Click;
            // 
            // infoEditorBtn
            // 
            infoEditorBtn.Dock = DockStyle.Fill;
            infoEditorBtn.ForeColor = Color.FromArgb(150, 150, 0);
            infoEditorBtn.Location = new Point(426, 3);
            infoEditorBtn.Name = "infoEditorBtn";
            infoEditorBtn.Size = new Size(137, 36);
            infoEditorBtn.TabIndex = 2;
            infoEditorBtn.Text = "Info Editor";
            infoEditorBtn.UseVisualStyleBackColor = true;
            infoEditorBtn.Click += infoEditorBtn_Click;
            // 
            // createOrderBtn
            // 
            createOrderBtn.Dock = DockStyle.Fill;
            createOrderBtn.ForeColor = Color.FromArgb(30, 100, 200);
            createOrderBtn.Location = new Point(285, 3);
            createOrderBtn.Name = "createOrderBtn";
            createOrderBtn.Size = new Size(135, 36);
            createOrderBtn.TabIndex = 1;
            createOrderBtn.Text = "Create/Clear Order";
            createOrderBtn.UseVisualStyleBackColor = true;
            createOrderBtn.Click += createOrderBtn_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel2.Controls.Add(infoIndicatorLbl, 1, 0);
            tableLayoutPanel2.Controls.Add(activeClientID, 0, 0);
            tableLayoutPanel2.Location = new Point(7, 19);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(560, 27);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // infoIndicatorLbl
            // 
            infoIndicatorLbl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            infoIndicatorLbl.Font = new Font("Segoe UI", 12F);
            infoIndicatorLbl.ForeColor = Color.Navy;
            infoIndicatorLbl.Location = new Point(395, 0);
            infoIndicatorLbl.Name = "infoIndicatorLbl";
            infoIndicatorLbl.Size = new Size(162, 27);
            infoIndicatorLbl.TabIndex = 1;
            infoIndicatorLbl.Text = "Info: N/A";
            infoIndicatorLbl.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // activeClientID
            // 
            activeClientID.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            activeClientID.Font = new Font("Segoe UI", 12F);
            activeClientID.ForeColor = Color.Purple;
            activeClientID.Location = new Point(3, 0);
            activeClientID.Name = "activeClientID";
            activeClientID.Size = new Size(386, 27);
            activeClientID.TabIndex = 0;
            activeClientID.Text = "Client {N/A}";
            activeClientID.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // mainContent
            // 
            mainContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mainContent.Controls.Add(tabPage1);
            mainContent.Controls.Add(tabPage2);
            mainContent.Controls.Add(tabPage3);
            mainContent.Location = new Point(214, 118);
            mainContent.Name = "mainContent";
            mainContent.SelectedIndex = 0;
            mainContent.Size = new Size(574, 318);
            mainContent.TabIndex = 2;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(lastnametxt);
            tabPage1.Controls.Add(button2);
            tabPage1.Controls.Add(listView1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(566, 290);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Sonic Menu";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Font = new Font("Segoe UI", 12F);
            button2.ForeColor = Color.Black;
            button2.Location = new Point(530, 6);
            button2.Name = "button2";
            button2.Size = new Size(30, 28);
            button2.TabIndex = 2;
            button2.Text = "↻";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // listView1
            // 
            listView1.Dock = DockStyle.Fill;
            listView1.Location = new Point(3, 3);
            listView1.Name = "listView1";
            listView1.Size = new Size(560, 284);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(566, 290);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Custom Items";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(566, 290);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Custom Item Factory";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // updatelist
            // 
            updatelist.Enabled = true;
            updatelist.Tick += updatelist_Tick;
            // 
            // clientList
            // 
            clientList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            clientList.FormattingEnabled = true;
            clientList.ItemHeight = 15;
            clientList.Location = new Point(12, 27);
            clientList.Name = "clientList";
            clientList.Size = new Size(196, 409);
            clientList.TabIndex = 0;
            clientList.DoubleClick += clientList_DoubleClick;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(12, 6);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(40, 19);
            checkBox1.TabIndex = 4;
            checkBox1.Text = "All";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // IPLabel
            // 
            IPLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            IPLabel.Font = new Font("Segoe UI", 8F);
            IPLabel.ForeColor = Color.Green;
            IPLabel.Location = new Point(58, 4);
            IPLabel.Name = "IPLabel";
            IPLabel.Size = new Size(150, 20);
            IPLabel.TabIndex = 2;
            IPLabel.Text = "0:0";
            IPLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // lastnametxt
            // 
            lastnametxt.Dock = DockStyle.Bottom;
            lastnametxt.Font = new Font("Segoe UI", 16F);
            lastnametxt.Location = new Point(3, 251);
            lastnametxt.Name = "lastnametxt";
            lastnametxt.Size = new Size(560, 36);
            lastnametxt.TabIndex = 12;
            lastnametxt.Text = "0bc6bd4d820f4db382d6d1e9b01a5c8b";
            // 
            // SUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(IPLabel);
            Controls.Add(checkBox1);
            Controls.Add(mainContent);
            Controls.Add(dataBox);
            Controls.Add(clientList);
            MinimumSize = new Size(816, 489);
            Name = "SUI";
            Text = "Form1";
            FormClosing += SUI_FormClosing;
            Load += SUI_Load;
            dataBox.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            mainContent.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private GroupBox dataBox;
        private Label activeClientID;
        private TabControl mainContent;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private ListView listView1;
        private TabPage tabPage3;
        private System.Windows.Forms.Timer updatelist;
        private TableLayoutPanel tableLayoutPanel2;
        private Label infoIndicatorLbl;
        private ListBox clientList;
        private TableLayoutPanel tableLayoutPanel1;
        private Button infoEditorBtn;
        private Button createOrderBtn;
        private Button checkInBtn;
        private CheckBox checkBox1;
        private Label IPLabel;
        private Button button1;
        private Button button2;
        private OpenFileDialog openFileDialog1;
        private TextBox lastnametxt;
    }
}