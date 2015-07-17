namespace WurmAssistant
{
    partial class FormWurmAssistantUpdate
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.buttonVisitForum = new System.Windows.Forms.Button();
            this.buttonChangelog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.richTextBox1.Location = new System.Drawing.Point(15, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(755, 488);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.TabStop = false;
            this.richTextBox1.Text = "";
            // 
            // buttonVisitForum
            // 
            this.buttonVisitForum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonVisitForum.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonVisitForum.Location = new System.Drawing.Point(570, 506);
            this.buttonVisitForum.Name = "buttonVisitForum";
            this.buttonVisitForum.Size = new System.Drawing.Size(200, 37);
            this.buttonVisitForum.TabIndex = 1;
            this.buttonVisitForum.Text = "Visit forum thread...";
            this.buttonVisitForum.UseVisualStyleBackColor = true;
            this.buttonVisitForum.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonChangelog
            // 
            this.buttonChangelog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonChangelog.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonChangelog.Location = new System.Drawing.Point(15, 506);
            this.buttonChangelog.Name = "buttonChangelog";
            this.buttonChangelog.Size = new System.Drawing.Size(200, 37);
            this.buttonChangelog.TabIndex = 0;
            this.buttonChangelog.Text = "View full changelog...";
            this.buttonChangelog.UseVisualStyleBackColor = true;
            this.buttonChangelog.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormWurmAssistantUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 555);
            this.Controls.Add(this.buttonChangelog);
            this.Controls.Add(this.buttonVisitForum);
            this.Controls.Add(this.richTextBox1);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 452);
            this.Name = "FormWurmAssistantUpdate";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wurm Assistant - What\'s new?";
            this.Load += new System.EventHandler(this.FormWurmAssistantUpdate_Load);
            this.Resize += new System.EventHandler(this.FormWurmAssistantUpdate_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button buttonVisitForum;
        private System.Windows.Forms.Button buttonChangelog;
    }
}