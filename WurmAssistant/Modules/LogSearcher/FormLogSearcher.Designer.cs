namespace WurmAssistant
{
    partial class FormLogSearcher
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxSearchKey = new System.Windows.Forms.TextBox();
            this.dateTimePickerTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerTimeTo = new System.Windows.Forms.DateTimePicker();
            this.comboBoxSearchType = new System.Windows.Forms.ComboBox();
            this.comboBoxPlayerName = new System.Windows.Forms.ComboBox();
            this.comboBoxLogType = new System.Windows.Forms.ComboBox();
            this.listBoxAllResults = new System.Windows.Forms.ListBox();
            this.labelAllResults = new System.Windows.Forms.Label();
            this.richTextBoxAllLines = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonCommitSearch = new System.Windows.Forms.Button();
            this.buttonCancelSearch = new System.Windows.Forms.Button();
            this.labelPM = new System.Windows.Forms.Label();
            this.textBoxPM = new System.Windows.Forms.TextBox();
            this.labelWorking = new System.Windows.Forms.Label();
            this.buttonForceRecache = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Logs for character:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Log type:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(152, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Time from:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(152, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Time to:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(370, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "Search type:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(370, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 17);
            this.label6.TabIndex = 11;
            this.label6.Text = "Search key:";
            // 
            // textBoxSearchKey
            // 
            this.textBoxSearchKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxSearchKey.Location = new System.Drawing.Point(373, 86);
            this.textBoxSearchKey.Name = "textBoxSearchKey";
            this.textBoxSearchKey.Size = new System.Drawing.Size(294, 27);
            this.textBoxSearchKey.TabIndex = 6;
            this.textBoxSearchKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxSearchKey_KeyDown);
            this.textBoxSearchKey.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxSearchKey_KeyUp);
            // 
            // dateTimePickerTimeFrom
            // 
            this.dateTimePickerTimeFrom.CustomFormat = "dd-MM-yyyy hh:mm";
            this.dateTimePickerTimeFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.dateTimePickerTimeFrom.Location = new System.Drawing.Point(158, 33);
            this.dateTimePickerTimeFrom.Name = "dateTimePickerTimeFrom";
            this.dateTimePickerTimeFrom.Size = new System.Drawing.Size(188, 27);
            this.dateTimePickerTimeFrom.TabIndex = 2;
            this.dateTimePickerTimeFrom.Value = new System.DateTime(2012, 12, 3, 0, 0, 0, 0);
            this.dateTimePickerTimeFrom.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dateTimePickerTimeFrom_KeyUp);
            // 
            // dateTimePickerTimeTo
            // 
            this.dateTimePickerTimeTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.dateTimePickerTimeTo.Location = new System.Drawing.Point(155, 86);
            this.dateTimePickerTimeTo.Name = "dateTimePickerTimeTo";
            this.dateTimePickerTimeTo.Size = new System.Drawing.Size(191, 27);
            this.dateTimePickerTimeTo.TabIndex = 3;
            this.dateTimePickerTimeTo.Value = new System.DateTime(2012, 12, 3, 0, 0, 0, 0);
            this.dateTimePickerTimeTo.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dateTimePickerTimeTo_KeyUp);
            // 
            // comboBoxSearchType
            // 
            this.comboBoxSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSearchType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBoxSearchType.FormattingEnabled = true;
            this.comboBoxSearchType.Location = new System.Drawing.Point(373, 31);
            this.comboBoxSearchType.Name = "comboBoxSearchType";
            this.comboBoxSearchType.Size = new System.Drawing.Size(294, 28);
            this.comboBoxSearchType.TabIndex = 4;
            this.comboBoxSearchType.KeyUp += new System.Windows.Forms.KeyEventHandler(this.comboBoxSearchType_KeyUp);
            // 
            // comboBoxPlayerName
            // 
            this.comboBoxPlayerName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPlayerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBoxPlayerName.FormattingEnabled = true;
            this.comboBoxPlayerName.Location = new System.Drawing.Point(15, 31);
            this.comboBoxPlayerName.Name = "comboBoxPlayerName";
            this.comboBoxPlayerName.Size = new System.Drawing.Size(121, 28);
            this.comboBoxPlayerName.TabIndex = 0;
            this.comboBoxPlayerName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.comboBoxPlayerName_KeyUp);
            // 
            // comboBoxLogType
            // 
            this.comboBoxLogType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLogType.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.comboBoxLogType.FormattingEnabled = true;
            this.comboBoxLogType.Location = new System.Drawing.Point(15, 86);
            this.comboBoxLogType.Name = "comboBoxLogType";
            this.comboBoxLogType.Size = new System.Drawing.Size(121, 28);
            this.comboBoxLogType.TabIndex = 1;
            this.comboBoxLogType.SelectedIndexChanged += new System.EventHandler(this.comboBoxLogType_SelectedIndexChanged);
            this.comboBoxLogType.KeyUp += new System.Windows.Forms.KeyEventHandler(this.comboBoxLogType_KeyUp);
            // 
            // listBoxAllResults
            // 
            this.listBoxAllResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxAllResults.FormattingEnabled = true;
            this.listBoxAllResults.ItemHeight = 16;
            this.listBoxAllResults.Location = new System.Drawing.Point(16, 145);
            this.listBoxAllResults.Name = "listBoxAllResults";
            this.listBoxAllResults.Size = new System.Drawing.Size(195, 468);
            this.listBoxAllResults.TabIndex = 9;
            this.listBoxAllResults.Click += new System.EventHandler(this.listBoxAllResults_Click);
            // 
            // labelAllResults
            // 
            this.labelAllResults.AutoSize = true;
            this.labelAllResults.Location = new System.Drawing.Point(13, 125);
            this.labelAllResults.Name = "labelAllResults";
            this.labelAllResults.Size = new System.Drawing.Size(73, 17);
            this.labelAllResults.TabIndex = 18;
            this.labelAllResults.Text = "All results:";
            // 
            // richTextBoxAllLines
            // 
            this.richTextBoxAllLines.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxAllLines.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.richTextBoxAllLines.HideSelection = false;
            this.richTextBoxAllLines.Location = new System.Drawing.Point(217, 145);
            this.richTextBoxAllLines.Name = "richTextBoxAllLines";
            this.richTextBoxAllLines.ReadOnly = true;
            this.richTextBoxAllLines.Size = new System.Drawing.Size(853, 532);
            this.richTextBoxAllLines.TabIndex = 8;
            this.richTextBoxAllLines.Text = "";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(214, 125);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 17);
            this.label8.TabIndex = 20;
            this.label8.Text = "All log entries:";
            // 
            // buttonCommitSearch
            // 
            this.buttonCommitSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCommitSearch.Location = new System.Drawing.Point(16, 619);
            this.buttonCommitSearch.Name = "buttonCommitSearch";
            this.buttonCommitSearch.Size = new System.Drawing.Size(195, 58);
            this.buttonCommitSearch.TabIndex = 7;
            this.buttonCommitSearch.Text = "Search";
            this.buttonCommitSearch.UseVisualStyleBackColor = true;
            this.buttonCommitSearch.Click += new System.EventHandler(this.buttonCommitSearch_Click);
            // 
            // buttonCancelSearch
            // 
            this.buttonCancelSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancelSearch.Location = new System.Drawing.Point(16, 555);
            this.buttonCancelSearch.Name = "buttonCancelSearch";
            this.buttonCancelSearch.Size = new System.Drawing.Size(196, 58);
            this.buttonCancelSearch.TabIndex = 23;
            this.buttonCancelSearch.TabStop = false;
            this.buttonCancelSearch.Text = "Cancel search";
            this.buttonCancelSearch.UseVisualStyleBackColor = true;
            this.buttonCancelSearch.Visible = false;
            this.buttonCancelSearch.Click += new System.EventHandler(this.buttonCancelSearch_Click);
            // 
            // labelPM
            // 
            this.labelPM.AutoSize = true;
            this.labelPM.Location = new System.Drawing.Point(688, 11);
            this.labelPM.Name = "labelPM";
            this.labelPM.Size = new System.Drawing.Size(75, 17);
            this.labelPM.TabIndex = 25;
            this.labelPM.Text = "PM player:";
            this.labelPM.Visible = false;
            // 
            // textBoxPM
            // 
            this.textBoxPM.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxPM.Location = new System.Drawing.Point(691, 31);
            this.textBoxPM.Name = "textBoxPM";
            this.textBoxPM.Size = new System.Drawing.Size(245, 27);
            this.textBoxPM.TabIndex = 5;
            this.textBoxPM.TabStop = false;
            this.textBoxPM.Visible = false;
            this.textBoxPM.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxPM_KeyUp);
            // 
            // labelWorking
            // 
            this.labelWorking.AutoSize = true;
            this.labelWorking.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelWorking.Location = new System.Drawing.Point(266, 195);
            this.labelWorking.Name = "labelWorking";
            this.labelWorking.Size = new System.Drawing.Size(344, 38);
            this.labelWorking.TabIndex = 26;
            this.labelWorking.Text = "Preparing log entries...";
            this.labelWorking.Visible = false;
            // 
            // buttonForceRecache
            // 
            this.buttonForceRecache.Location = new System.Drawing.Point(975, 12);
            this.buttonForceRecache.Name = "buttonForceRecache";
            this.buttonForceRecache.Size = new System.Drawing.Size(94, 116);
            this.buttonForceRecache.TabIndex = 27;
            this.buttonForceRecache.Text = "Force Refresh Cache";
            this.buttonForceRecache.UseVisualStyleBackColor = true;
            this.buttonForceRecache.Click += new System.EventHandler(this.buttonForceRecache_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(688, 77);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(248, 51);
            this.label9.TabIndex = 28;
            this.label9.Text = "If this is first run (or just updated), and\r\nsearch doesn\'t seem to work, please\r" +
    "\ntry restart the program.";
            // 
            // FormLogSearcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 699);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.buttonForceRecache);
            this.Controls.Add(this.labelWorking);
            this.Controls.Add(this.labelPM);
            this.Controls.Add(this.textBoxPM);
            this.Controls.Add(this.buttonCancelSearch);
            this.Controls.Add(this.buttonCommitSearch);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.richTextBoxAllLines);
            this.Controls.Add(this.labelAllResults);
            this.Controls.Add(this.listBoxAllResults);
            this.Controls.Add(this.comboBoxLogType);
            this.Controls.Add(this.comboBoxPlayerName);
            this.Controls.Add(this.comboBoxSearchType);
            this.Controls.Add(this.dateTimePickerTimeTo);
            this.Controls.Add(this.dateTimePickerTimeFrom);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxSearchKey);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "FormLogSearcher";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Log Searcher";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLogSearcher_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxSearchKey;
        private System.Windows.Forms.DateTimePicker dateTimePickerTimeFrom;
        private System.Windows.Forms.DateTimePicker dateTimePickerTimeTo;
        private System.Windows.Forms.ComboBox comboBoxSearchType;
        private System.Windows.Forms.ComboBox comboBoxPlayerName;
        private System.Windows.Forms.ComboBox comboBoxLogType;
        private System.Windows.Forms.ListBox listBoxAllResults;
        private System.Windows.Forms.Label labelAllResults;
        private System.Windows.Forms.RichTextBox richTextBoxAllLines;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonCommitSearch;
        private System.Windows.Forms.Button buttonCancelSearch;
        private System.Windows.Forms.Label labelPM;
        private System.Windows.Forms.TextBox textBoxPM;
        private System.Windows.Forms.Label labelWorking;
        private System.Windows.Forms.Button buttonForceRecache;
        private System.Windows.Forms.Label label9;
    }
}