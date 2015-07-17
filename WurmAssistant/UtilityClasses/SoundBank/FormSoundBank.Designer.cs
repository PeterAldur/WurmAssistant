namespace WurmAssistant
{
    partial class FormSoundBank
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
            this.listBoxAllSounds = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonRename = new System.Windows.Forms.Button();
            this.textBoxSelectedSound = new System.Windows.Forms.TextBox();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.trackBarAdjustedVolume = new System.Windows.Forms.TrackBar();
            this.buttonPlay = new System.Windows.Forms.Button();
            this.buttonAddSounds = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAdjustedVolume)).BeginInit();
            this.SuspendLayout();
            // 
            // listBoxAllSounds
            // 
            this.listBoxAllSounds.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxAllSounds.FormattingEnabled = true;
            this.listBoxAllSounds.ItemHeight = 16;
            this.listBoxAllSounds.Location = new System.Drawing.Point(12, 36);
            this.listBoxAllSounds.Name = "listBoxAllSounds";
            this.listBoxAllSounds.Size = new System.Drawing.Size(431, 388);
            this.listBoxAllSounds.TabIndex = 0;
            this.listBoxAllSounds.SelectedIndexChanged += new System.EventHandler(this.listBoxAllSounds_SelectedIndexChanged);
            this.listBoxAllSounds.DoubleClick += new System.EventHandler(this.listBoxAllSounds_DoubleClick);
            this.listBoxAllSounds.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBoxAllSounds_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sounds (adjusted volume):";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.buttonRename);
            this.groupBox1.Controls.Add(this.textBoxSelectedSound);
            this.groupBox1.Controls.Add(this.buttonRemove);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.trackBarAdjustedVolume);
            this.groupBox1.Controls.Add(this.buttonPlay);
            this.groupBox1.Location = new System.Drawing.Point(449, 36);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(229, 388);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selected Sound";
            // 
            // buttonRename
            // 
            this.buttonRename.Location = new System.Drawing.Point(11, 49);
            this.buttonRename.Name = "buttonRename";
            this.buttonRename.Size = new System.Drawing.Size(202, 42);
            this.buttonRename.TabIndex = 16;
            this.buttonRename.Text = "Rename";
            this.buttonRename.UseVisualStyleBackColor = true;
            this.buttonRename.Click += new System.EventHandler(this.buttonRename_Click);
            // 
            // textBoxSelectedSound
            // 
            this.textBoxSelectedSound.Location = new System.Drawing.Point(11, 21);
            this.textBoxSelectedSound.Name = "textBoxSelectedSound";
            this.textBoxSelectedSound.ReadOnly = true;
            this.textBoxSelectedSound.Size = new System.Drawing.Size(202, 22);
            this.textBoxSelectedSound.TabIndex = 15;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(11, 340);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(202, 42);
            this.buttonRemove.TabIndex = 14;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 17);
            this.label2.TabIndex = 13;
            this.label2.Text = "Adjusted volume";
            // 
            // trackBarAdjustedVolume
            // 
            this.trackBarAdjustedVolume.AutoSize = false;
            this.trackBarAdjustedVolume.Location = new System.Drawing.Point(11, 138);
            this.trackBarAdjustedVolume.Maximum = 100;
            this.trackBarAdjustedVolume.Name = "trackBarAdjustedVolume";
            this.trackBarAdjustedVolume.Size = new System.Drawing.Size(202, 38);
            this.trackBarAdjustedVolume.TabIndex = 12;
            this.trackBarAdjustedVolume.Value = 100;
            this.trackBarAdjustedVolume.ValueChanged += new System.EventHandler(this.trackBarAdjustedVolume_ValueChanged);
            this.trackBarAdjustedVolume.KeyDown += new System.Windows.Forms.KeyEventHandler(this.trackBarAdjustedVolume_KeyDown);
            this.trackBarAdjustedVolume.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trackBarAdjustedVolume_MouseUp);
            // 
            // buttonPlay
            // 
            this.buttonPlay.Location = new System.Drawing.Point(11, 182);
            this.buttonPlay.Name = "buttonPlay";
            this.buttonPlay.Size = new System.Drawing.Size(202, 42);
            this.buttonPlay.TabIndex = 0;
            this.buttonPlay.Text = "Play";
            this.buttonPlay.UseVisualStyleBackColor = true;
            this.buttonPlay.Click += new System.EventHandler(this.buttonPlay_Click);
            // 
            // buttonAddSounds
            // 
            this.buttonAddSounds.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddSounds.Location = new System.Drawing.Point(12, 430);
            this.buttonAddSounds.Name = "buttonAddSounds";
            this.buttonAddSounds.Size = new System.Drawing.Size(431, 42);
            this.buttonAddSounds.TabIndex = 3;
            this.buttonAddSounds.Text = "Add more sounds...";
            this.buttonAddSounds.UseVisualStyleBackColor = true;
            this.buttonAddSounds.Click += new System.EventHandler(this.buttonAddSounds_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Supported Sound Files|*.wav;*.ogg;*.mp3;*.flac;*.mod;*.it;*.s3d;*.xm";
            this.openFileDialog1.InitialDirectory = "SoundBank";
            this.openFileDialog1.Multiselect = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(11, 230);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(202, 42);
            this.button1.TabIndex = 17;
            this.button1.Text = "Stop playing";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormSoundBank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 484);
            this.Controls.Add(this.buttonAddSounds);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxAllSounds);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(550, 529);
            this.Name = "FormSoundBank";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sound Bank";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAdjustedVolume)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxAllSounds;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonPlay;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBarAdjustedVolume;
        private System.Windows.Forms.Button buttonAddSounds;
        private System.Windows.Forms.TextBox textBoxSelectedSound;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonRename;
        private System.Windows.Forms.Button button1;

    }
}