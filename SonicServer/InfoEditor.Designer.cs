using System.Drawing;
using System.Windows.Forms;

namespace ServerUI
{
    partial class InfoEditor
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
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            dataBox = new GroupBox();
            activeClientID = new Label();
            mainContent = new TabControl();
            basicSection = new TabPage();
            tableLayoutPanel3 = new TableLayoutPanel();
            panel3 = new Panel();
            label1 = new Label();
            idTxt = new TextBox();
            label3 = new Label();
            panel2 = new Panel();
            tableLayoutPanel4 = new TableLayoutPanel();
            firstnametxt = new TextBox();
            lastnametxt = new TextBox();
            label4 = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            applyProfileBtn = new Button();
            panel1 = new Panel();
            comboBox2 = new ComboBox();
            button2 = new Button();
            pfpSection = new TabPage();
            previewLargeCbx = new CheckBox();
            pfpActualImg = new PictureBox();
            spacer = new Panel();
            applyPreviewBtn = new Button();
            previewBtn = new Button();
            urlLbl = new Label();
            previewUrlTxt = new TextBox();
            downloadedPreview = new PictureBox();
            prevHeader = new Label();
            profilesSection = new TabPage();
            button3 = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            previewIdLbl = new Label();
            previewFullNameLbl = new Label();
            comboBox1 = new ComboBox();
            pictureBox1 = new PictureBox();
            updatelist = new System.Windows.Forms.Timer(components);
            dataBox.SuspendLayout();
            mainContent.SuspendLayout();
            basicSection.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel1.SuspendLayout();
            pfpSection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pfpActualImg).BeginInit();
            ((System.ComponentModel.ISupportInitialize)downloadedPreview).BeginInit();
            profilesSection.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dataBox
            // 
            dataBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dataBox.Controls.Add(activeClientID);
            dataBox.Location = new Point(12, 12);
            dataBox.Name = "dataBox";
            dataBox.Size = new Size(590, 100);
            dataBox.TabIndex = 1;
            dataBox.TabStop = false;
            dataBox.Text = "Data";
            // 
            // activeClientID
            // 
            activeClientID.Font = new Font("Segoe UI", 14F);
            activeClientID.Location = new Point(10, 15);
            activeClientID.Name = "activeClientID";
            activeClientID.Size = new Size(558, 70);
            activeClientID.TabIndex = 0;
            activeClientID.Text = "Client {N/A}";
            activeClientID.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // mainContent
            // 
            mainContent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            mainContent.Controls.Add(basicSection);
            mainContent.Controls.Add(pfpSection);
            mainContent.Controls.Add(profilesSection);
            mainContent.Location = new Point(12, 118);
            mainContent.Name = "mainContent";
            mainContent.SelectedIndex = 0;
            mainContent.Size = new Size(590, 359);
            mainContent.TabIndex = 2;
            mainContent.DrawItem += mainContent_DrawItem;
            mainContent.SelectedIndexChanged += mainContent_SelectedIndexChanged;
            // 
            // basicSection
            // 
            basicSection.Controls.Add(tableLayoutPanel3);
            basicSection.Controls.Add(tableLayoutPanel2);
            basicSection.Location = new Point(4, 24);
            basicSection.Name = "basicSection";
            basicSection.Padding = new Padding(3);
            basicSection.Size = new Size(582, 331);
            basicSection.TabIndex = 0;
            basicSection.Text = "Basic Info";
            basicSection.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(panel3, 0, 0);
            tableLayoutPanel3.Controls.Add(panel2, 0, 1);
            tableLayoutPanel3.Location = new Point(6, 10);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(570, 252);
            tableLayoutPanel3.TabIndex = 9;
            // 
            // panel3
            // 
            panel3.Controls.Add(label1);
            panel3.Controls.Add(idTxt);
            panel3.Controls.Add(label3);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(564, 120);
            panel3.TabIndex = 4;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 9F);
            label1.Location = new Point(341, 0);
            label1.Name = "label1";
            label1.Size = new Size(223, 23);
            label1.TabIndex = 1;
            label1.Text = "Apply before saving.";
            label1.TextAlign = ContentAlignment.TopRight;
            // 
            // idTxt
            // 
            idTxt.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            idTxt.Font = new Font("Segoe UI", 16F);
            idTxt.Location = new Point(3, 73);
            idTxt.Name = "idTxt";
            idTxt.Size = new Size(558, 36);
            idTxt.TabIndex = 10;
            idTxt.Text = "Error loading profile data";
            idTxt.TextChanged += idTxt_TextChanged;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label3.Font = new Font("Segoe UI", 16F);
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(561, 70);
            label3.TabIndex = 2;
            label3.Text = "ID";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.Controls.Add(tableLayoutPanel4);
            panel2.Controls.Add(label4);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 129);
            panel2.Name = "panel2";
            panel2.Size = new Size(564, 120);
            panel2.TabIndex = 3;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(firstnametxt, 0, 0);
            tableLayoutPanel4.Controls.Add(lastnametxt, 1, 0);
            tableLayoutPanel4.Location = new Point(3, 72);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(558, 45);
            tableLayoutPanel4.TabIndex = 9;
            // 
            // firstnametxt
            // 
            firstnametxt.Dock = DockStyle.Fill;
            firstnametxt.Font = new Font("Segoe UI", 16F);
            firstnametxt.Location = new Point(3, 3);
            firstnametxt.Name = "firstnametxt";
            firstnametxt.Size = new Size(273, 36);
            firstnametxt.TabIndex = 10;
            firstnametxt.Text = "Error loading profile data";
            firstnametxt.TextChanged += fullNameTxt_TextChanged;
            // 
            // lastnametxt
            // 
            lastnametxt.Dock = DockStyle.Fill;
            lastnametxt.Font = new Font("Segoe UI", 16F);
            lastnametxt.Location = new Point(282, 3);
            lastnametxt.Name = "lastnametxt";
            lastnametxt.Size = new Size(273, 36);
            lastnametxt.TabIndex = 11;
            lastnametxt.Text = "Error loading profile data";
            lastnametxt.TextChanged += lastnametxt_TextChanged;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label4.Font = new Font("Segoe UI", 16F);
            label4.Location = new Point(0, 0);
            label4.Name = "label4";
            label4.Size = new Size(561, 69);
            label4.TabIndex = 2;
            label4.Text = "First Last";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(applyProfileBtn, 0, 0);
            tableLayoutPanel2.Controls.Add(panel1, 1, 0);
            tableLayoutPanel2.Location = new Point(6, 265);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(570, 60);
            tableLayoutPanel2.TabIndex = 8;
            // 
            // applyProfileBtn
            // 
            applyProfileBtn.Dock = DockStyle.Fill;
            applyProfileBtn.Font = new Font("Segoe UI", 12F);
            applyProfileBtn.Location = new Point(3, 3);
            applyProfileBtn.Name = "applyProfileBtn";
            applyProfileBtn.Size = new Size(279, 54);
            applyProfileBtn.TabIndex = 7;
            applyProfileBtn.Text = "Apply Info*";
            applyProfileBtn.UseVisualStyleBackColor = true;
            applyProfileBtn.Click += applyProfileBtn_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(comboBox2);
            panel1.Controls.Add(button2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(288, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(279, 54);
            panel1.TabIndex = 8;
            // 
            // comboBox2
            // 
            comboBox2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(0, 3);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(279, 23);
            comboBox2.TabIndex = 9;
            comboBox2.Text = "Error loading profiles";
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button2.Font = new Font("Segoe UI", 10F);
            button2.Location = new Point(0, 25);
            button2.Name = "button2";
            button2.Size = new Size(279, 29);
            button2.TabIndex = 8;
            button2.Text = "Save Profile";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // pfpSection
            // 
            pfpSection.Controls.Add(previewLargeCbx);
            pfpSection.Controls.Add(pfpActualImg);
            pfpSection.Controls.Add(spacer);
            pfpSection.Controls.Add(applyPreviewBtn);
            pfpSection.Controls.Add(previewBtn);
            pfpSection.Controls.Add(urlLbl);
            pfpSection.Controls.Add(previewUrlTxt);
            pfpSection.Controls.Add(downloadedPreview);
            pfpSection.Controls.Add(prevHeader);
            pfpSection.Location = new Point(4, 24);
            pfpSection.Name = "pfpSection";
            pfpSection.Padding = new Padding(3);
            pfpSection.Size = new Size(582, 331);
            pfpSection.TabIndex = 1;
            pfpSection.Text = "Profile Picture";
            pfpSection.UseVisualStyleBackColor = true;
            pfpSection.Click += pfpSection_Click;
            // 
            // previewLargeCbx
            // 
            previewLargeCbx.AutoSize = true;
            previewLargeCbx.Location = new Point(290, 3);
            previewLargeCbx.Name = "previewLargeCbx";
            previewLargeCbx.Size = new Size(181, 19);
            previewLargeCbx.TabIndex = 4;
            previewLargeCbx.Text = "Project Preview to Large View";
            previewLargeCbx.UseVisualStyleBackColor = true;
            // 
            // pfpActualImg
            // 
            pfpActualImg.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pfpActualImg.BackColor = SystemColors.Control;
            pfpActualImg.Location = new Point(6, 6);
            pfpActualImg.Name = "pfpActualImg";
            pfpActualImg.Size = new Size(278, 278);
            pfpActualImg.SizeMode = PictureBoxSizeMode.StretchImage;
            pfpActualImg.TabIndex = 0;
            pfpActualImg.TabStop = false;
            // 
            // spacer
            // 
            spacer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            spacer.BackColor = SystemColors.Control;
            spacer.Location = new Point(290, 142);
            spacer.Name = "spacer";
            spacer.Size = new Size(270, 89);
            spacer.TabIndex = 7;
            // 
            // applyPreviewBtn
            // 
            applyPreviewBtn.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            applyPreviewBtn.Font = new Font("Segoe UI", 12F);
            applyPreviewBtn.Location = new Point(290, 237);
            applyPreviewBtn.Name = "applyPreviewBtn";
            applyPreviewBtn.Size = new Size(270, 47);
            applyPreviewBtn.TabIndex = 6;
            applyPreviewBtn.Text = "Apply Preview";
            applyPreviewBtn.UseVisualStyleBackColor = true;
            applyPreviewBtn.Click += applyPreviewBtn_Click;
            // 
            // previewBtn
            // 
            previewBtn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            previewBtn.Location = new Point(290, 105);
            previewBtn.Name = "previewBtn";
            previewBtn.Size = new Size(270, 31);
            previewBtn.TabIndex = 5;
            previewBtn.Text = "Download";
            previewBtn.UseVisualStyleBackColor = true;
            previewBtn.Click += previewBtn_Click;
            // 
            // urlLbl
            // 
            urlLbl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            urlLbl.Font = new Font("Segoe UI", 9F);
            urlLbl.Location = new Point(338, 58);
            urlLbl.Name = "urlLbl";
            urlLbl.Size = new Size(222, 15);
            urlLbl.TabIndex = 4;
            urlLbl.Text = "URL";
            urlLbl.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // previewUrlTxt
            // 
            previewUrlTxt.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            previewUrlTxt.Location = new Point(338, 76);
            previewUrlTxt.Name = "previewUrlTxt";
            previewUrlTxt.Size = new Size(222, 23);
            previewUrlTxt.TabIndex = 3;
            // 
            // downloadedPreview
            // 
            downloadedPreview.BackColor = SystemColors.Control;
            downloadedPreview.Location = new Point(290, 57);
            downloadedPreview.Name = "downloadedPreview";
            downloadedPreview.Size = new Size(42, 42);
            downloadedPreview.SizeMode = PictureBoxSizeMode.StretchImage;
            downloadedPreview.TabIndex = 2;
            downloadedPreview.TabStop = false;
            // 
            // prevHeader
            // 
            prevHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            prevHeader.Font = new Font("Segoe UI", 16F);
            prevHeader.Location = new Point(290, 25);
            prevHeader.Name = "prevHeader";
            prevHeader.Size = new Size(270, 29);
            prevHeader.TabIndex = 1;
            prevHeader.Text = "Online Previewer";
            prevHeader.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // profilesSection
            // 
            profilesSection.Controls.Add(button3);
            profilesSection.Controls.Add(tableLayoutPanel1);
            profilesSection.Controls.Add(comboBox1);
            profilesSection.Controls.Add(pictureBox1);
            profilesSection.Location = new Point(4, 24);
            profilesSection.Name = "profilesSection";
            profilesSection.Size = new Size(582, 331);
            profilesSection.TabIndex = 2;
            profilesSection.Text = "Saved Profiles";
            profilesSection.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button3.Font = new Font("Segoe UI", 12F);
            button3.Location = new Point(290, 233);
            button3.Name = "button3";
            button3.Size = new Size(270, 51);
            button3.TabIndex = 8;
            button3.Text = "Set As Current";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.BackColor = SystemColors.Control;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(previewIdLbl, 0, 0);
            tableLayoutPanel1.Controls.Add(previewFullNameLbl, 0, 1);
            tableLayoutPanel1.Location = new Point(290, 35);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(270, 192);
            tableLayoutPanel1.TabIndex = 5;
            // 
            // previewIdLbl
            // 
            previewIdLbl.Dock = DockStyle.Fill;
            previewIdLbl.Font = new Font("Segoe UI", 16F);
            previewIdLbl.Location = new Point(3, 0);
            previewIdLbl.Name = "previewIdLbl";
            previewIdLbl.Size = new Size(264, 96);
            previewIdLbl.TabIndex = 3;
            previewIdLbl.Text = "ID";
            previewIdLbl.TextAlign = ContentAlignment.BottomCenter;
            // 
            // previewFullNameLbl
            // 
            previewFullNameLbl.Dock = DockStyle.Fill;
            previewFullNameLbl.Font = new Font("Segoe UI", 16F);
            previewFullNameLbl.Location = new Point(3, 96);
            previewFullNameLbl.Name = "previewFullNameLbl";
            previewFullNameLbl.Size = new Size(264, 96);
            previewFullNameLbl.TabIndex = 2;
            previewFullNameLbl.Text = "First Last";
            previewFullNameLbl.TextAlign = ContentAlignment.TopCenter;
            // 
            // comboBox1
            // 
            comboBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Default" });
            comboBox1.Location = new Point(290, 6);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(270, 23);
            comboBox1.TabIndex = 4;
            comboBox1.Text = "Default";
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            pictureBox1.BackColor = SystemColors.Control;
            pictureBox1.Location = new Point(6, 6);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(278, 278);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // updatelist
            // 
            updatelist.Enabled = true;
            // 
            // InfoEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(614, 489);
            Controls.Add(mainContent);
            Controls.Add(dataBox);
            MinimumSize = new Size(614, 489);
            Name = "InfoEditor";
            Text = "Form1";
            Load += InfoEditor_Load;
            dataBox.ResumeLayout(false);
            mainContent.ResumeLayout(false);
            basicSection.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            pfpSection.ResumeLayout(false);
            pfpSection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pfpActualImg).EndInit();
            ((System.ComponentModel.ISupportInitialize)downloadedPreview).EndInit();
            profilesSection.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private GroupBox dataBox;
        private Label activeClientID;
        private TabControl mainContent;
        private TabPage pfpSection;
        private System.Windows.Forms.Timer updatelist;
        private TabPage basicSection;
        private Label urlLbl;
        private TextBox previewUrlTxt;
        private PictureBox downloadedPreview;
        private Label prevHeader;
        private PictureBox pfpActualImg;
        private Panel spacer;
        private Button applyPreviewBtn;
        private Button previewBtn;
        private CheckBox previewLargeCbx;
        private TabPage profilesSection;
        private Label previewFullNameLbl;
        private PictureBox pictureBox1;
        private Label previewIdLbl;
        private ComboBox comboBox1;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Button applyProfileBtn;
        private Button button2;
        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label4;
        private Panel panel2;
        private TextBox firstnametxt;
        private Panel panel3;
        private TextBox idTxt;
        private Label label3;
        private Button button3;
        private TextBox lastnametxt;
        private ComboBox comboBox2;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel4;
    }
}
