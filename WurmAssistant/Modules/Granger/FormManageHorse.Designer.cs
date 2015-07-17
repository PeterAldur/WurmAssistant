namespace WurmAssistant.Granger
{
    partial class FormManageHorse
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
            this.checkedListBoxTraits = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxGender = new System.Windows.Forms.GroupBox();
            this.radioButtonFemale = new System.Windows.Forms.RadioButton();
            this.radioButtonMale = new System.Windows.Forms.RadioButton();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.labelValue = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dateTimePickerBreeded = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerGroomed = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerPregnant = new System.Windows.Forms.DateTimePicker();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonCopyToClipboard = new System.Windows.Forms.Button();
            this.checkBoxPregnant = new System.Windows.Forms.CheckBox();
            this.checkBoxDeceased = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxComment = new System.Windows.Forms.TextBox();
            this.textBoxCaredForBy = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownAHskill = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.labelWarnDuplicate = new System.Windows.Forms.Label();
            this.comboBoxFather = new System.Windows.Forms.ComboBox();
            this.comboBoxMother = new System.Windows.Forms.ComboBox();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.groupBoxGender.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAHskill)).BeginInit();
            this.SuspendLayout();
            // 
            // checkedListBoxTraits
            // 
            this.checkedListBoxTraits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxTraits.FormattingEnabled = true;
            this.checkedListBoxTraits.Location = new System.Drawing.Point(6, 22);
            this.checkedListBoxTraits.Name = "checkedListBoxTraits";
            this.checkedListBoxTraits.Size = new System.Drawing.Size(422, 480);
            this.checkedListBoxTraits.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Traits:";
            // 
            // groupBoxGender
            // 
            this.groupBoxGender.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxGender.Controls.Add(this.radioButtonFemale);
            this.groupBoxGender.Controls.Add(this.radioButtonMale);
            this.groupBoxGender.Location = new System.Drawing.Point(451, 52);
            this.groupBoxGender.Name = "groupBoxGender";
            this.groupBoxGender.Size = new System.Drawing.Size(263, 58);
            this.groupBoxGender.TabIndex = 2;
            this.groupBoxGender.TabStop = false;
            this.groupBoxGender.Text = "Gender";
            // 
            // radioButtonFemale
            // 
            this.radioButtonFemale.AutoSize = true;
            this.radioButtonFemale.Checked = true;
            this.radioButtonFemale.Location = new System.Drawing.Point(129, 24);
            this.radioButtonFemale.Name = "radioButtonFemale";
            this.radioButtonFemale.Size = new System.Drawing.Size(71, 21);
            this.radioButtonFemale.TabIndex = 2;
            this.radioButtonFemale.TabStop = true;
            this.radioButtonFemale.Text = "female";
            this.radioButtonFemale.UseVisualStyleBackColor = true;
            // 
            // radioButtonMale
            // 
            this.radioButtonMale.AutoSize = true;
            this.radioButtonMale.Location = new System.Drawing.Point(6, 24);
            this.radioButtonMale.Name = "radioButtonMale";
            this.radioButtonMale.Size = new System.Drawing.Size(59, 21);
            this.radioButtonMale.TabIndex = 1;
            this.radioButtonMale.Text = "male";
            this.radioButtonMale.UseVisualStyleBackColor = true;
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(451, 24);
            this.textBoxName.MaxLength = 50;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(263, 22);
            this.textBoxName.TabIndex = 0;
            this.textBoxName.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxName_Validating);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(448, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Name";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(448, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Mother";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(448, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Father";
            // 
            // labelValue
            // 
            this.labelValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(7, 505);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(44, 17);
            this.labelValue.TabIndex = 9;
            this.labelValue.Text = "Value";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(448, 207);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "Last breeded on";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(448, 252);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(115, 17);
            this.label8.TabIndex = 12;
            this.label8.Text = "Last groomed on";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(448, 297);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(112, 17);
            this.label9.TabIndex = 13;
            this.label9.Text = "Will give birth on";
            // 
            // dateTimePickerBreeded
            // 
            this.dateTimePickerBreeded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerBreeded.CustomFormat = "yyyy-MM-dd   hh:mm";
            this.dateTimePickerBreeded.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerBreeded.Location = new System.Drawing.Point(451, 227);
            this.dateTimePickerBreeded.Name = "dateTimePickerBreeded";
            this.dateTimePickerBreeded.Size = new System.Drawing.Size(263, 22);
            this.dateTimePickerBreeded.TabIndex = 5;
            // 
            // dateTimePickerGroomed
            // 
            this.dateTimePickerGroomed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerGroomed.CustomFormat = "yyyy-MM-dd   hh:mm";
            this.dateTimePickerGroomed.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerGroomed.Location = new System.Drawing.Point(451, 272);
            this.dateTimePickerGroomed.Name = "dateTimePickerGroomed";
            this.dateTimePickerGroomed.Size = new System.Drawing.Size(263, 22);
            this.dateTimePickerGroomed.TabIndex = 6;
            // 
            // dateTimePickerPregnant
            // 
            this.dateTimePickerPregnant.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerPregnant.CustomFormat = "yyyy-MM-dd   hh:mm";
            this.dateTimePickerPregnant.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerPregnant.Location = new System.Drawing.Point(451, 317);
            this.dateTimePickerPregnant.Name = "dateTimePickerPregnant";
            this.dateTimePickerPregnant.Size = new System.Drawing.Size(263, 22);
            this.dateTimePickerPregnant.TabIndex = 7;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(643, 569);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(90, 39);
            this.buttonOK.TabIndex = 14;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(547, 569);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(90, 39);
            this.buttonCancel.TabIndex = 15;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonCopyToClipboard
            // 
            this.buttonCopyToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCopyToClipboard.Enabled = false;
            this.buttonCopyToClipboard.Location = new System.Drawing.Point(12, 569);
            this.buttonCopyToClipboard.Name = "buttonCopyToClipboard";
            this.buttonCopyToClipboard.Size = new System.Drawing.Size(151, 39);
            this.buttonCopyToClipboard.TabIndex = 17;
            this.buttonCopyToClipboard.Text = "Copy to clipboard";
            this.buttonCopyToClipboard.UseVisualStyleBackColor = true;
            this.buttonCopyToClipboard.Click += new System.EventHandler(this.buttonCopyToClipboard_Click);
            // 
            // checkBoxPregnant
            // 
            this.checkBoxPregnant.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxPregnant.AutoSize = true;
            this.checkBoxPregnant.Location = new System.Drawing.Point(451, 435);
            this.checkBoxPregnant.Name = "checkBoxPregnant";
            this.checkBoxPregnant.Size = new System.Drawing.Size(101, 21);
            this.checkBoxPregnant.TabIndex = 10;
            this.checkBoxPregnant.Text = "Is pregnant";
            this.checkBoxPregnant.UseVisualStyleBackColor = true;
            // 
            // checkBoxDeceased
            // 
            this.checkBoxDeceased.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxDeceased.AutoSize = true;
            this.checkBoxDeceased.Location = new System.Drawing.Point(580, 435);
            this.checkBoxDeceased.Name = "checkBoxDeceased";
            this.checkBoxDeceased.Size = new System.Drawing.Size(106, 21);
            this.checkBoxDeceased.TabIndex = 11;
            this.checkBoxDeceased.Text = "Is deceased";
            this.checkBoxDeceased.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.textBoxComment);
            this.panel1.Controls.Add(this.textBoxCaredForBy);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.numericUpDownAHskill);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.labelWarnDuplicate);
            this.panel1.Controls.Add(this.comboBoxFather);
            this.panel1.Controls.Add(this.comboBoxMother);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.checkBoxDeceased);
            this.panel1.Controls.Add(this.checkedListBoxTraits);
            this.panel1.Controls.Add(this.checkBoxPregnant);
            this.panel1.Controls.Add(this.groupBoxGender);
            this.panel1.Controls.Add(this.textBoxName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.dateTimePickerPregnant);
            this.panel1.Controls.Add(this.dateTimePickerGroomed);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.dateTimePickerBreeded);
            this.panel1.Controls.Add(this.labelValue);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(721, 551);
            this.panel1.TabIndex = 24;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(451, 470);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(78, 17);
            this.label10.TabIndex = 32;
            this.label10.Text = "Comments:";
            // 
            // textBoxComment
            // 
            this.textBoxComment.Location = new System.Drawing.Point(451, 490);
            this.textBoxComment.Multiline = true;
            this.textBoxComment.Name = "textBoxComment";
            this.textBoxComment.Size = new System.Drawing.Size(263, 58);
            this.textBoxComment.TabIndex = 12;
            // 
            // textBoxCaredForBy
            // 
            this.textBoxCaredForBy.Location = new System.Drawing.Point(451, 362);
            this.textBoxCaredForBy.Name = "textBoxCaredForBy";
            this.textBoxCaredForBy.Size = new System.Drawing.Size(263, 22);
            this.textBoxCaredForBy.TabIndex = 8;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(448, 342);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 17);
            this.label6.TabIndex = 29;
            this.label6.Text = "Cared for by";
            // 
            // numericUpDownAHskill
            // 
            this.numericUpDownAHskill.Location = new System.Drawing.Point(451, 407);
            this.numericUpDownAHskill.Name = "numericUpDownAHskill";
            this.numericUpDownAHskill.Size = new System.Drawing.Size(78, 22);
            this.numericUpDownAHskill.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(451, 387);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(231, 17);
            this.label5.TabIndex = 27;
            this.label5.Text = "Inspected at Animal Husbandry skill";
            // 
            // labelWarnDuplicate
            // 
            this.labelWarnDuplicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelWarnDuplicate.AutoSize = true;
            this.labelWarnDuplicate.ForeColor = System.Drawing.Color.Red;
            this.labelWarnDuplicate.Location = new System.Drawing.Point(501, 4);
            this.labelWarnDuplicate.Name = "labelWarnDuplicate";
            this.labelWarnDuplicate.Size = new System.Drawing.Size(217, 17);
            this.labelWarnDuplicate.TabIndex = 26;
            this.labelWarnDuplicate.Text = "This name is already in database";
            this.labelWarnDuplicate.Visible = false;
            // 
            // comboBoxFather
            // 
            this.comboBoxFather.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFather.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboBoxFather.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxFather.FormattingEnabled = true;
            this.comboBoxFather.Location = new System.Drawing.Point(451, 180);
            this.comboBoxFather.Name = "comboBoxFather";
            this.comboBoxFather.Size = new System.Drawing.Size(263, 24);
            this.comboBoxFather.TabIndex = 4;
            this.comboBoxFather.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxFather_Validating);
            // 
            // comboBoxMother
            // 
            this.comboBoxMother.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxMother.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.comboBoxMother.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxMother.FormattingEnabled = true;
            this.comboBoxMother.Location = new System.Drawing.Point(451, 133);
            this.comboBoxMother.Name = "comboBoxMother";
            this.comboBoxMother.Size = new System.Drawing.Size(263, 24);
            this.comboBoxMother.TabIndex = 3;
            this.comboBoxMother.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxMother_Validating);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEdit.Location = new System.Drawing.Point(451, 569);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(90, 39);
            this.buttonEdit.TabIndex = 16;
            this.buttonEdit.Text = "Edit";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Visible = false;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // FormManageHorse
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(748, 623);
            this.Controls.Add(this.buttonEdit);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonCopyToClipboard);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormManageHorse";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Horse";
            this.groupBoxGender.ResumeLayout(false);
            this.groupBoxGender.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAHskill)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBoxTraits;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxGender;
        private System.Windows.Forms.RadioButton radioButtonFemale;
        private System.Windows.Forms.RadioButton radioButtonMale;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dateTimePickerBreeded;
        private System.Windows.Forms.DateTimePicker dateTimePickerGroomed;
        private System.Windows.Forms.DateTimePicker dateTimePickerPregnant;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonCopyToClipboard;
        private System.Windows.Forms.CheckBox checkBoxPregnant;
        private System.Windows.Forms.CheckBox checkBoxDeceased;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.ComboBox comboBoxFather;
        private System.Windows.Forms.ComboBox comboBoxMother;
        private System.Windows.Forms.Label labelWarnDuplicate;
        private System.Windows.Forms.NumericUpDown numericUpDownAHskill;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxCaredForBy;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxComment;
    }
}