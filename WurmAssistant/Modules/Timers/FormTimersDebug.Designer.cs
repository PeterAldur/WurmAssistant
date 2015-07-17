namespace WurmAssistant
{
    partial class FormTimersDebug
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
            this.listBoxMeditHistory = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelCalculatedUptime = new System.Windows.Forms.Label();
            this.textBoxServerUptime = new System.Windows.Forms.TextBox();
            this.listBoxPrayHistory = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listBoxAlignmentHistory = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listBoxMeditHistory
            // 
            this.listBoxMeditHistory.FormattingEnabled = true;
            this.listBoxMeditHistory.ItemHeight = 16;
            this.listBoxMeditHistory.Location = new System.Drawing.Point(12, 29);
            this.listBoxMeditHistory.Name = "listBoxMeditHistory";
            this.listBoxMeditHistory.Size = new System.Drawing.Size(346, 324);
            this.listBoxMeditHistory.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "All medit since 2 days";
            // 
            // labelCalculatedUptime
            // 
            this.labelCalculatedUptime.AutoSize = true;
            this.labelCalculatedUptime.Location = new System.Drawing.Point(9, 375);
            this.labelCalculatedUptime.Name = "labelCalculatedUptime";
            this.labelCalculatedUptime.Size = new System.Drawing.Size(267, 34);
            this.labelCalculatedUptime.TabIndex = 11;
            this.labelCalculatedUptime.Text = "Calculated server uptime (if this keeps \r\nshowing wrong, it\'s a bug, please repor" +
    "t):";
            // 
            // textBoxServerUptime
            // 
            this.textBoxServerUptime.Location = new System.Drawing.Point(12, 411);
            this.textBoxServerUptime.Name = "textBoxServerUptime";
            this.textBoxServerUptime.Size = new System.Drawing.Size(320, 22);
            this.textBoxServerUptime.TabIndex = 12;
            // 
            // listBoxPrayHistory
            // 
            this.listBoxPrayHistory.FormattingEnabled = true;
            this.listBoxPrayHistory.ItemHeight = 16;
            this.listBoxPrayHistory.Location = new System.Drawing.Point(364, 29);
            this.listBoxPrayHistory.Name = "listBoxPrayHistory";
            this.listBoxPrayHistory.Size = new System.Drawing.Size(346, 324);
            this.listBoxPrayHistory.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(361, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(158, 17);
            this.label2.TabIndex = 14;
            this.label2.Text = "All prayers since 2 days";
            // 
            // listBoxAlignmentHistory
            // 
            this.listBoxAlignmentHistory.FormattingEnabled = true;
            this.listBoxAlignmentHistory.ItemHeight = 16;
            this.listBoxAlignmentHistory.Location = new System.Drawing.Point(716, 29);
            this.listBoxAlignmentHistory.Name = "listBoxAlignmentHistory";
            this.listBoxAlignmentHistory.Size = new System.Drawing.Size(346, 324);
            this.listBoxAlignmentHistory.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(713, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(223, 17);
            this.label3.TabIndex = 16;
            this.label3.Text = "All alignment triggers since 2 days";
            // 
            // FormTimersDebug
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 445);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBoxAlignmentHistory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBoxPrayHistory);
            this.Controls.Add(this.textBoxServerUptime);
            this.Controls.Add(this.labelCalculatedUptime);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxMeditHistory);
            this.Name = "FormTimersDebug";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Timers debug window";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTimersDebug_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelCalculatedUptime;
        public System.Windows.Forms.ListBox listBoxMeditHistory;
        public System.Windows.Forms.TextBox textBoxServerUptime;
        public System.Windows.Forms.ListBox listBoxPrayHistory;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.ListBox listBoxAlignmentHistory;
        private System.Windows.Forms.Label label3;
    }
}