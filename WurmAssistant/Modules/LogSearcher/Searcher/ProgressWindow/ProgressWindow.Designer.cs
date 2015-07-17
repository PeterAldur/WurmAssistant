namespace WurmAssistant
{
    partial class ProgressWindow
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.labelWait = new System.Windows.Forms.Label();
            this.labelStatusUpdate = new System.Windows.Forms.Label();
            this.textBoxDescribe = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 95);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(437, 29);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 0;
            // 
            // labelWait
            // 
            this.labelWait.AutoSize = true;
            this.labelWait.Location = new System.Drawing.Point(183, 136);
            this.labelWait.Name = "labelWait";
            this.labelWait.Size = new System.Drawing.Size(91, 17);
            this.labelWait.TabIndex = 1;
            this.labelWait.Text = "Please wait...";
            // 
            // labelStatusUpdate
            // 
            this.labelStatusUpdate.AutoSize = true;
            this.labelStatusUpdate.Location = new System.Drawing.Point(28, 75);
            this.labelStatusUpdate.Name = "labelStatusUpdate";
            this.labelStatusUpdate.Size = new System.Drawing.Size(46, 17);
            this.labelStatusUpdate.TabIndex = 2;
            this.labelStatusUpdate.Text = "label1";
            // 
            // textBoxDescribe
            // 
            this.textBoxDescribe.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDescribe.Location = new System.Drawing.Point(12, 12);
            this.textBoxDescribe.Multiline = true;
            this.textBoxDescribe.Name = "textBoxDescribe";
            this.textBoxDescribe.ReadOnly = true;
            this.textBoxDescribe.Size = new System.Drawing.Size(437, 60);
            this.textBoxDescribe.TabIndex = 3;
            this.textBoxDescribe.TabStop = false;
            // 
            // ProgressWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 167);
            this.Controls.Add(this.textBoxDescribe);
            this.Controls.Add(this.labelStatusUpdate);
            this.Controls.Add(this.labelWait);
            this.Controls.Add(this.progressBar1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressWindow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Wurm Assistant - Log Searcher";
            this.Load += new System.EventHandler(this.ProgressWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        public System.Windows.Forms.Label labelWait;
        public System.Windows.Forms.Label labelStatusUpdate;
        public System.Windows.Forms.TextBox textBoxDescribe;
    }
}