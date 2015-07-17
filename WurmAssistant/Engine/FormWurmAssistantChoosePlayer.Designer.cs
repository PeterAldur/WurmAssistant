namespace WurmAssistant
{
    partial class FormWurmAssistantChoosePlayer
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
            this.label1 = new System.Windows.Forms.Label();
            this.listBoxChoosePlayer = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPathToLogFiles = new System.Windows.Forms.TextBox();
            this.buttonChooseManually = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonMonthly = new System.Windows.Forms.RadioButton();
            this.radioButtonDaily = new System.Windows.Forms.RadioButton();
            this.labelWarning = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(231, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Choose your main Wurm character:";
            // 
            // listBoxChoosePlayer
            // 
            this.listBoxChoosePlayer.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listBoxChoosePlayer.FormattingEnabled = true;
            this.listBoxChoosePlayer.ItemHeight = 20;
            this.listBoxChoosePlayer.Location = new System.Drawing.Point(12, 128);
            this.listBoxChoosePlayer.Name = "listBoxChoosePlayer";
            this.listBoxChoosePlayer.Size = new System.Drawing.Size(295, 164);
            this.listBoxChoosePlayer.TabIndex = 1;
            this.listBoxChoosePlayer.SelectedIndexChanged += new System.EventHandler(this.listBoxChoosePlayer_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 303);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Detected path to Wurm log files:";
            // 
            // textBoxPathToLogFiles
            // 
            this.textBoxPathToLogFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPathToLogFiles.Location = new System.Drawing.Point(12, 323);
            this.textBoxPathToLogFiles.Name = "textBoxPathToLogFiles";
            this.textBoxPathToLogFiles.ReadOnly = true;
            this.textBoxPathToLogFiles.Size = new System.Drawing.Size(295, 22);
            this.textBoxPathToLogFiles.TabIndex = 3;
            // 
            // buttonChooseManually
            // 
            this.buttonChooseManually.Location = new System.Drawing.Point(117, 351);
            this.buttonChooseManually.Name = "buttonChooseManually";
            this.buttonChooseManually.Size = new System.Drawing.Size(190, 25);
            this.buttonChooseManually.TabIndex = 4;
            this.buttonChooseManually.Text = "Choose location manually...";
            this.buttonChooseManually.UseVisualStyleBackColor = true;
            this.buttonChooseManually.Click += new System.EventHandler(this.buttonChooseManually_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(128, 451);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(107, 37);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(15, 451);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(107, 37);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Choose Wurm Log Folder (ex. C:\\wurm\\players\\Johndoe\\logs)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonMonthly);
            this.groupBox1.Controls.Add(this.radioButtonDaily);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(297, 88);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Wurm Client logging mode";
            // 
            // radioButtonMonthly
            // 
            this.radioButtonMonthly.AutoSize = true;
            this.radioButtonMonthly.Checked = true;
            this.radioButtonMonthly.Location = new System.Drawing.Point(6, 27);
            this.radioButtonMonthly.Name = "radioButtonMonthly";
            this.radioButtonMonthly.Size = new System.Drawing.Size(78, 21);
            this.radioButtonMonthly.TabIndex = 1;
            this.radioButtonMonthly.TabStop = true;
            this.radioButtonMonthly.Text = "Monthly";
            this.radioButtonMonthly.UseVisualStyleBackColor = true;
            // 
            // radioButtonDaily
            // 
            this.radioButtonDaily.AutoSize = true;
            this.radioButtonDaily.Location = new System.Drawing.Point(6, 54);
            this.radioButtonDaily.Name = "radioButtonDaily";
            this.radioButtonDaily.Size = new System.Drawing.Size(60, 21);
            this.radioButtonDaily.TabIndex = 0;
            this.radioButtonDaily.Text = "Daily";
            this.radioButtonDaily.UseVisualStyleBackColor = true;
            // 
            // labelWarning
            // 
            this.labelWarning.AutoSize = true;
            this.labelWarning.Location = new System.Drawing.Point(9, 388);
            this.labelWarning.Name = "labelWarning";
            this.labelWarning.Size = new System.Drawing.Size(315, 51);
            this.labelWarning.TabIndex = 8;
            this.labelWarning.Text = "Please do not choose location manually, unless \r\nabove list is empty or shows wro" +
    "ng names!\r\nCorrect directory is essential for this app to work.";
            // 
            // FormWurmAssistantChoosePlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 500);
            this.Controls.Add(this.labelWarning);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonChooseManually);
            this.Controls.Add(this.textBoxPathToLogFiles);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBoxChoosePlayer);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(340, 545);
            this.Name = "FormWurmAssistantChoosePlayer";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Choose character";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBoxChoosePlayer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonChooseManually;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        public System.Windows.Forms.TextBox textBoxPathToLogFiles;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonMonthly;
        private System.Windows.Forms.RadioButton radioButtonDaily;
        private System.Windows.Forms.Label labelWarning;
    }
}