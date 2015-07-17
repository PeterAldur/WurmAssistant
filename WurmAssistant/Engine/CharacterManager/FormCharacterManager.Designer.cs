namespace WurmAssistant
{
    partial class FormCharacterManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCharacterManager));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonDaily = new System.Windows.Forms.RadioButton();
            this.radioButtonMonthly = new System.Windows.Forms.RadioButton();
            this.textBoxWurmPath = new System.Windows.Forms.TextBox();
            this.labelWurmPath = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSetManuallyWurmPath = new System.Windows.Forms.Button();
            this.listBoxMainChar = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkedListBoxAltChars = new System.Windows.Forms.CheckedListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonCloseAndApply = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radioButtonDaily);
            this.groupBox1.Controls.Add(this.radioButtonMonthly);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 137);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Wurm Client logging mode";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.ForeColor = System.Drawing.Color.DarkCyan;
            this.label1.Location = new System.Drawing.Point(2, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(254, 40);
            this.label1.TabIndex = 2;
            this.label1.Text = "Please set this setting correctly, \r\nelse some features will not work.";
            // 
            // radioButtonDaily
            // 
            this.radioButtonDaily.AutoSize = true;
            this.radioButtonDaily.Location = new System.Drawing.Point(9, 102);
            this.radioButtonDaily.Name = "radioButtonDaily";
            this.radioButtonDaily.Size = new System.Drawing.Size(60, 21);
            this.radioButtonDaily.TabIndex = 1;
            this.radioButtonDaily.TabStop = true;
            this.radioButtonDaily.Text = "Daily";
            this.radioButtonDaily.UseVisualStyleBackColor = true;
            this.radioButtonDaily.CheckedChanged += new System.EventHandler(this.radioButtonDaily_CheckedChanged);
            // 
            // radioButtonMonthly
            // 
            this.radioButtonMonthly.AutoSize = true;
            this.radioButtonMonthly.Location = new System.Drawing.Point(9, 75);
            this.radioButtonMonthly.Name = "radioButtonMonthly";
            this.radioButtonMonthly.Size = new System.Drawing.Size(78, 21);
            this.radioButtonMonthly.TabIndex = 0;
            this.radioButtonMonthly.TabStop = true;
            this.radioButtonMonthly.Text = "Monthly";
            this.radioButtonMonthly.UseVisualStyleBackColor = true;
            this.radioButtonMonthly.CheckedChanged += new System.EventHandler(this.radioButtonMonthly_CheckedChanged);
            // 
            // textBoxWurmPath
            // 
            this.textBoxWurmPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWurmPath.Location = new System.Drawing.Point(6, 44);
            this.textBoxWurmPath.Name = "textBoxWurmPath";
            this.textBoxWurmPath.ReadOnly = true;
            this.textBoxWurmPath.Size = new System.Drawing.Size(652, 22);
            this.textBoxWurmPath.TabIndex = 1;
            // 
            // labelWurmPath
            // 
            this.labelWurmPath.AutoSize = true;
            this.labelWurmPath.Location = new System.Drawing.Point(3, 24);
            this.labelWurmPath.Name = "labelWurmPath";
            this.labelWurmPath.Size = new System.Drawing.Size(236, 17);
            this.labelWurmPath.TabIndex = 2;
            this.labelWurmPath.Text = "Autodetected wurm installation path:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.buttonSetManuallyWurmPath);
            this.groupBox2.Controls.Add(this.textBoxWurmPath);
            this.groupBox2.Controls.Add(this.labelWurmPath);
            this.groupBox2.Location = new System.Drawing.Point(275, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(664, 105);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Wurm Client Path";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(438, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Note: Only adjust this option, if you suspect above path to be wrong.";
            // 
            // buttonSetManuallyWurmPath
            // 
            this.buttonSetManuallyWurmPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSetManuallyWurmPath.Location = new System.Drawing.Point(517, 68);
            this.buttonSetManuallyWurmPath.Name = "buttonSetManuallyWurmPath";
            this.buttonSetManuallyWurmPath.Size = new System.Drawing.Size(141, 27);
            this.buttonSetManuallyWurmPath.TabIndex = 3;
            this.buttonSetManuallyWurmPath.Text = "Manual override";
            this.buttonSetManuallyWurmPath.UseVisualStyleBackColor = true;
            this.buttonSetManuallyWurmPath.Click += new System.EventHandler(this.buttonSetManuallyWurmPath_Click);
            // 
            // listBoxMainChar
            // 
            this.listBoxMainChar.FormattingEnabled = true;
            this.listBoxMainChar.ItemHeight = 16;
            this.listBoxMainChar.Location = new System.Drawing.Point(6, 44);
            this.listBoxMainChar.Name = "listBoxMainChar";
            this.listBoxMainChar.Size = new System.Drawing.Size(234, 180);
            this.listBoxMainChar.TabIndex = 4;
            this.listBoxMainChar.SelectedIndexChanged += new System.EventHandler(this.listBoxMainChar_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox3.Controls.Add(this.checkedListBoxAltChars);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.listBoxMainChar);
            this.groupBox3.Location = new System.Drawing.Point(12, 208);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(257, 490);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Character Manager";
            // 
            // checkedListBoxAltChars
            // 
            this.checkedListBoxAltChars.CheckOnClick = true;
            this.checkedListBoxAltChars.FormattingEnabled = true;
            this.checkedListBoxAltChars.Location = new System.Drawing.Point(6, 254);
            this.checkedListBoxAltChars.Name = "checkedListBoxAltChars";
            this.checkedListBoxAltChars.Size = new System.Drawing.Size(234, 191);
            this.checkedListBoxAltChars.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 234);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(234, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Choose the ALTS you want to track:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(193, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Choose your MAIN character:";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.textBox2);
            this.groupBox4.Location = new System.Drawing.Point(275, 123);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(658, 575);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Additional information:";
            // 
            // textBox2
            // 
            this.textBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBox2.Location = new System.Drawing.Point(6, 21);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox2.Size = new System.Drawing.Size(646, 545);
            this.textBox2.TabIndex = 0;
            this.textBox2.Text = resources.GetString("textBox2.Text");
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.Description = "Choose Wurm Base Directory (ex. C:\\Games\\wurm\\)";
            // 
            // buttonCloseAndApply
            // 
            this.buttonCloseAndApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCloseAndApply.Location = new System.Drawing.Point(801, 704);
            this.buttonCloseAndApply.Name = "buttonCloseAndApply";
            this.buttonCloseAndApply.Size = new System.Drawing.Size(132, 28);
            this.buttonCloseAndApply.TabIndex = 8;
            this.buttonCloseAndApply.Text = "Close and Apply";
            this.buttonCloseAndApply.UseVisualStyleBackColor = true;
            this.buttonCloseAndApply.Click += new System.EventHandler(this.buttonCloseAndApply_Click);
            // 
            // FormCharacterManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 741);
            this.Controls.Add(this.buttonCloseAndApply);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCharacterManager";
            this.ShowIcon = false;
            this.Text = "Character Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCharacterManager_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonDaily;
        private System.Windows.Forms.RadioButton radioButtonMonthly;
        private System.Windows.Forms.TextBox textBoxWurmPath;
        private System.Windows.Forms.Label labelWurmPath;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonSetManuallyWurmPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBoxMainChar;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox checkedListBoxAltChars;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button buttonCloseAndApply;
        private System.Windows.Forms.Label label1;
    }
}