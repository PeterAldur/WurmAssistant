namespace WurmAssistant
{
    partial class FormCalendar
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonRealTime = new System.Windows.Forms.RadioButton();
            this.radioButtonWurmTime = new System.Windows.Forms.RadioButton();
            this.checkBoxSoundWarning = new System.Windows.Forms.CheckBox();
            this.checkBoxPopupWarning = new System.Windows.Forms.CheckBox();
            this.buttonChooseSeasons = new System.Windows.Forms.Button();
            this.buttonClearSound = new System.Windows.Forms.Button();
            this.buttonChooseSound = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxChosenSeasons = new System.Windows.Forms.TextBox();
            this.textBoxChosenSound = new System.Windows.Forms.TextBox();
            this.listViewNFSeasons = new ClassLibrary1.ListViewNF();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxWurmDate = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.radioButtonRealTime);
            this.groupBox1.Controls.Add(this.radioButtonWurmTime);
            this.groupBox1.Location = new System.Drawing.Point(573, 330);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(159, 77);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Time display options";
            // 
            // radioButtonRealTime
            // 
            this.radioButtonRealTime.AutoSize = true;
            this.radioButtonRealTime.Location = new System.Drawing.Point(6, 48);
            this.radioButtonRealTime.Name = "radioButtonRealTime";
            this.radioButtonRealTime.Size = new System.Drawing.Size(93, 21);
            this.radioButtonRealTime.TabIndex = 1;
            this.radioButtonRealTime.TabStop = true;
            this.radioButtonRealTime.Text = "Real Time";
            this.radioButtonRealTime.UseVisualStyleBackColor = true;
            this.radioButtonRealTime.CheckedChanged += new System.EventHandler(this.radioButtonRealTime_CheckedChanged);
            // 
            // radioButtonWurmTime
            // 
            this.radioButtonWurmTime.AutoSize = true;
            this.radioButtonWurmTime.Location = new System.Drawing.Point(6, 21);
            this.radioButtonWurmTime.Name = "radioButtonWurmTime";
            this.radioButtonWurmTime.Size = new System.Drawing.Size(101, 21);
            this.radioButtonWurmTime.TabIndex = 0;
            this.radioButtonWurmTime.TabStop = true;
            this.radioButtonWurmTime.Text = "Wurm Time";
            this.radioButtonWurmTime.UseVisualStyleBackColor = true;
            this.radioButtonWurmTime.CheckedChanged += new System.EventHandler(this.radioButtonWurmTime_CheckedChanged);
            // 
            // checkBoxSoundWarning
            // 
            this.checkBoxSoundWarning.AutoSize = true;
            this.checkBoxSoundWarning.Location = new System.Drawing.Point(6, 29);
            this.checkBoxSoundWarning.Name = "checkBoxSoundWarning";
            this.checkBoxSoundWarning.Size = new System.Drawing.Size(71, 21);
            this.checkBoxSoundWarning.TabIndex = 2;
            this.checkBoxSoundWarning.Text = "Sound";
            this.checkBoxSoundWarning.UseVisualStyleBackColor = true;
            this.checkBoxSoundWarning.CheckedChanged += new System.EventHandler(this.checkBoxSoundWarning_CheckedChanged);
            // 
            // checkBoxPopupWarning
            // 
            this.checkBoxPopupWarning.AutoSize = true;
            this.checkBoxPopupWarning.Location = new System.Drawing.Point(6, 56);
            this.checkBoxPopupWarning.Name = "checkBoxPopupWarning";
            this.checkBoxPopupWarning.Size = new System.Drawing.Size(131, 21);
            this.checkBoxPopupWarning.TabIndex = 3;
            this.checkBoxPopupWarning.Text = "Tray notification";
            this.checkBoxPopupWarning.UseVisualStyleBackColor = true;
            this.checkBoxPopupWarning.CheckedChanged += new System.EventHandler(this.checkBoxPopupWarning_CheckedChanged);
            // 
            // buttonChooseSeasons
            // 
            this.buttonChooseSeasons.Location = new System.Drawing.Point(280, 17);
            this.buttonChooseSeasons.Name = "buttonChooseSeasons";
            this.buttonChooseSeasons.Size = new System.Drawing.Size(251, 27);
            this.buttonChooseSeasons.TabIndex = 4;
            this.buttonChooseSeasons.Text = "Choose tracked seasons...";
            this.buttonChooseSeasons.UseVisualStyleBackColor = true;
            this.buttonChooseSeasons.Click += new System.EventHandler(this.buttonChooseSeasons_Click);
            // 
            // buttonClearSound
            // 
            this.buttonClearSound.Location = new System.Drawing.Point(193, 84);
            this.buttonClearSound.Name = "buttonClearSound";
            this.buttonClearSound.Size = new System.Drawing.Size(64, 28);
            this.buttonClearSound.TabIndex = 15;
            this.buttonClearSound.Text = "clear";
            this.buttonClearSound.UseVisualStyleBackColor = true;
            this.buttonClearSound.Click += new System.EventHandler(this.buttonClearSound_Click);
            // 
            // buttonChooseSound
            // 
            this.buttonChooseSound.Location = new System.Drawing.Point(6, 84);
            this.buttonChooseSound.Name = "buttonChooseSound";
            this.buttonChooseSound.Size = new System.Drawing.Size(184, 28);
            this.buttonChooseSound.TabIndex = 14;
            this.buttonChooseSound.Text = "Choose sound";
            this.buttonChooseSound.UseVisualStyleBackColor = true;
            this.buttonChooseSound.Click += new System.EventHandler(this.buttonChooseSound_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.textBoxChosenSeasons);
            this.groupBox2.Controls.Add(this.checkBoxPopupWarning);
            this.groupBox2.Controls.Add(this.textBoxChosenSound);
            this.groupBox2.Controls.Add(this.buttonChooseSeasons);
            this.groupBox2.Controls.Add(this.buttonChooseSound);
            this.groupBox2.Controls.Add(this.checkBoxSoundWarning);
            this.groupBox2.Controls.Add(this.buttonClearSound);
            this.groupBox2.Location = new System.Drawing.Point(12, 330);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(555, 150);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Notification options";
            // 
            // textBoxChosenSeasons
            // 
            this.textBoxChosenSeasons.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxChosenSeasons.Location = new System.Drawing.Point(280, 49);
            this.textBoxChosenSeasons.Multiline = true;
            this.textBoxChosenSeasons.Name = "textBoxChosenSeasons";
            this.textBoxChosenSeasons.ReadOnly = true;
            this.textBoxChosenSeasons.Size = new System.Drawing.Size(251, 95);
            this.textBoxChosenSeasons.TabIndex = 17;
            // 
            // textBoxChosenSound
            // 
            this.textBoxChosenSound.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxChosenSound.Location = new System.Drawing.Point(6, 117);
            this.textBoxChosenSound.Name = "textBoxChosenSound";
            this.textBoxChosenSound.ReadOnly = true;
            this.textBoxChosenSound.Size = new System.Drawing.Size(251, 27);
            this.textBoxChosenSound.TabIndex = 16;
            // 
            // listViewNFSeasons
            // 
            this.listViewNFSeasons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewNFSeasons.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.listViewNFSeasons.GridLines = true;
            this.listViewNFSeasons.Location = new System.Drawing.Point(12, 12);
            this.listViewNFSeasons.Name = "listViewNFSeasons";
            this.listViewNFSeasons.Size = new System.Drawing.Size(720, 312);
            this.listViewNFSeasons.TabIndex = 17;
            this.listViewNFSeasons.UseCompatibleStateImageBehavior = false;
            this.listViewNFSeasons.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Plant";
            this.columnHeader4.Width = 110;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Starts in...";
            this.columnHeader5.Width = 360;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Lasts for...";
            this.columnHeader6.Width = 220;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxWurmDate);
            this.groupBox3.Location = new System.Drawing.Point(573, 413);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(159, 67);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Wurm Date";
            // 
            // textBoxWurmDate
            // 
            this.textBoxWurmDate.Location = new System.Drawing.Point(7, 21);
            this.textBoxWurmDate.Multiline = true;
            this.textBoxWurmDate.Name = "textBoxWurmDate";
            this.textBoxWurmDate.ReadOnly = true;
            this.textBoxWurmDate.Size = new System.Drawing.Size(146, 40);
            this.textBoxWurmDate.TabIndex = 0;
            // 
            // FormCalendar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 486);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.listViewNFSeasons);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(762, 508);
            this.Name = "FormCalendar";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Season Calendar";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormCalendar_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonRealTime;
        private System.Windows.Forms.RadioButton radioButtonWurmTime;
        private System.Windows.Forms.CheckBox checkBoxSoundWarning;
        private System.Windows.Forms.CheckBox checkBoxPopupWarning;
        private System.Windows.Forms.Button buttonChooseSeasons;
        private System.Windows.Forms.Button buttonClearSound;
        private System.Windows.Forms.Button buttonChooseSound;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxChosenSeasons;
        private System.Windows.Forms.TextBox textBoxChosenSound;
        private ClassLibrary1.ListViewNF listViewNFSeasons;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxWurmDate;



    }
}