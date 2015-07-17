namespace WurmAssistant.Granger
{
    partial class FormHerdName
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxOld = new System.Windows.Forms.TextBox();
            this.textBoxNew = new System.Windows.Forms.TextBox();
            this.labelOld = new System.Windows.Forms.Label();
            this.labelNew = new System.Windows.Forms.Label();
            this.comboBoxTargetHerd = new System.Windows.Forms.ComboBox();
            this.labelTarget = new System.Windows.Forms.Label();
            this.labelWarn = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(200, 146);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(90, 37);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(104, 146);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(90, 37);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // textBoxOld
            // 
            this.textBoxOld.Location = new System.Drawing.Point(128, 19);
            this.textBoxOld.Name = "textBoxOld";
            this.textBoxOld.ReadOnly = true;
            this.textBoxOld.Size = new System.Drawing.Size(162, 22);
            this.textBoxOld.TabIndex = 2;
            // 
            // textBoxNew
            // 
            this.textBoxNew.Location = new System.Drawing.Point(128, 47);
            this.textBoxNew.MaxLength = 30;
            this.textBoxNew.Name = "textBoxNew";
            this.textBoxNew.Size = new System.Drawing.Size(162, 22);
            this.textBoxNew.TabIndex = 3;
            this.textBoxNew.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxNew_Validating);
            // 
            // labelOld
            // 
            this.labelOld.AutoSize = true;
            this.labelOld.Location = new System.Drawing.Point(16, 22);
            this.labelOld.Name = "labelOld";
            this.labelOld.Size = new System.Drawing.Size(106, 17);
            this.labelOld.TabIndex = 4;
            this.labelOld.Text = "Old herd name:";
            // 
            // labelNew
            // 
            this.labelNew.AutoSize = true;
            this.labelNew.Location = new System.Drawing.Point(11, 50);
            this.labelNew.Name = "labelNew";
            this.labelNew.Size = new System.Drawing.Size(111, 17);
            this.labelNew.TabIndex = 5;
            this.labelNew.Text = "New herd name:";
            // 
            // comboBoxTargetHerd
            // 
            this.comboBoxTargetHerd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTargetHerd.FormattingEnabled = true;
            this.comboBoxTargetHerd.Location = new System.Drawing.Point(128, 75);
            this.comboBoxTargetHerd.Name = "comboBoxTargetHerd";
            this.comboBoxTargetHerd.Size = new System.Drawing.Size(162, 24);
            this.comboBoxTargetHerd.TabIndex = 6;
            // 
            // labelTarget
            // 
            this.labelTarget.AutoSize = true;
            this.labelTarget.Location = new System.Drawing.Point(60, 78);
            this.labelTarget.Name = "labelTarget";
            this.labelTarget.Size = new System.Drawing.Size(62, 17);
            this.labelTarget.TabIndex = 7;
            this.labelTarget.Text = "To herd:";
            // 
            // labelWarn
            // 
            this.labelWarn.AutoSize = true;
            this.labelWarn.ForeColor = System.Drawing.Color.OrangeRed;
            this.labelWarn.Location = new System.Drawing.Point(48, 102);
            this.labelWarn.Name = "labelWarn";
            this.labelWarn.Size = new System.Drawing.Size(242, 17);
            this.labelWarn.TabIndex = 8;
            this.labelWarn.Text = "This name already exists in database";
            this.labelWarn.Visible = false;
            // 
            // FormHerdName
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(302, 195);
            this.Controls.Add(this.labelWarn);
            this.Controls.Add(this.labelTarget);
            this.Controls.Add(this.comboBoxTargetHerd);
            this.Controls.Add(this.labelNew);
            this.Controls.Add(this.labelOld);
            this.Controls.Add(this.textBoxNew);
            this.Controls.Add(this.textBoxOld);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormHerdName";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormHerdName";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelOld;
        private System.Windows.Forms.Label labelNew;
        private System.Windows.Forms.Label labelTarget;
        public System.Windows.Forms.TextBox textBoxOld;
        public System.Windows.Forms.TextBox textBoxNew;
        public System.Windows.Forms.ComboBox comboBoxTargetHerd;
        private System.Windows.Forms.Label labelWarn;
    }
}