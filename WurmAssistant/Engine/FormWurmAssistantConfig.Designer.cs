namespace WurmAssistant
{
    partial class FormWurmAssistantConfig
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
            this.textBoxSeconds = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownMillis = new System.Windows.Forms.NumericUpDown();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkBoxDisplayEntries = new System.Windows.Forms.CheckBox();
            this.checkBoxMiniToTray = new System.Windows.Forms.CheckBox();
            this.checkBoxStartMinimized = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMillis)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(196, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Program loop (timer) tick rate:";
            // 
            // textBoxSeconds
            // 
            this.textBoxSeconds.Location = new System.Drawing.Point(200, 35);
            this.textBoxSeconds.Name = "textBoxSeconds";
            this.textBoxSeconds.ReadOnly = true;
            this.textBoxSeconds.Size = new System.Drawing.Size(47, 22);
            this.textBoxSeconds.TabIndex = 1;
            this.textBoxSeconds.TabStop = false;
            this.textBoxSeconds.Text = "0,5";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(98, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "milliseconds =";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(253, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "seconds";
            // 
            // numericUpDownMillis
            // 
            this.numericUpDownMillis.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownMillis.Location = new System.Drawing.Point(12, 36);
            this.numericUpDownMillis.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDownMillis.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownMillis.Name = "numericUpDownMillis";
            this.numericUpDownMillis.Size = new System.Drawing.Size(80, 22);
            this.numericUpDownMillis.TabIndex = 0;
            this.numericUpDownMillis.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDownMillis.ValueChanged += new System.EventHandler(this.numericUpDownMillis_ValueChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(12, 190);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(107, 37);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(125, 190);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(107, 37);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // checkBoxDisplayEntries
            // 
            this.checkBoxDisplayEntries.AutoSize = true;
            this.checkBoxDisplayEntries.Location = new System.Drawing.Point(12, 74);
            this.checkBoxDisplayEntries.Name = "checkBoxDisplayEntries";
            this.checkBoxDisplayEntries.Size = new System.Drawing.Size(324, 21);
            this.checkBoxDisplayEntries.TabIndex = 5;
            this.checkBoxDisplayEntries.Text = "Display new log entries in program log (debug)";
            this.checkBoxDisplayEntries.UseVisualStyleBackColor = true;
            this.checkBoxDisplayEntries.CheckedChanged += new System.EventHandler(this.checkBoxDisplayEntries_CheckedChanged);
            // 
            // checkBoxMiniToTray
            // 
            this.checkBoxMiniToTray.AutoSize = true;
            this.checkBoxMiniToTray.Location = new System.Drawing.Point(12, 101);
            this.checkBoxMiniToTray.Name = "checkBoxMiniToTray";
            this.checkBoxMiniToTray.Size = new System.Drawing.Size(133, 21);
            this.checkBoxMiniToTray.TabIndex = 6;
            this.checkBoxMiniToTray.Text = "Minimize to Tray";
            this.checkBoxMiniToTray.UseVisualStyleBackColor = true;
            this.checkBoxMiniToTray.CheckedChanged += new System.EventHandler(this.checkBoxMiniToTray_CheckedChanged);
            // 
            // checkBoxStartMinimized
            // 
            this.checkBoxStartMinimized.AutoSize = true;
            this.checkBoxStartMinimized.Location = new System.Drawing.Point(12, 128);
            this.checkBoxStartMinimized.Name = "checkBoxStartMinimized";
            this.checkBoxStartMinimized.Size = new System.Drawing.Size(126, 21);
            this.checkBoxStartMinimized.TabIndex = 7;
            this.checkBoxStartMinimized.Text = "Start minimized";
            this.checkBoxStartMinimized.UseVisualStyleBackColor = true;
            // 
            // FormWurmAssistantConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 239);
            this.Controls.Add(this.checkBoxStartMinimized);
            this.Controls.Add(this.checkBoxMiniToTray);
            this.Controls.Add(this.checkBoxDisplayEntries);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.numericUpDownMillis);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxSeconds);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormWurmAssistantConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wurm Assistant Config";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMillis)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSeconds;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        public System.Windows.Forms.NumericUpDown numericUpDownMillis;
        private System.Windows.Forms.CheckBox checkBoxDisplayEntries;
        public System.Windows.Forms.CheckBox checkBoxMiniToTray;
        public System.Windows.Forms.CheckBox checkBoxStartMinimized;
    }
}