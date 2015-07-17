namespace WA_Test
{
    partial class Form1
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
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.generalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grangerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skillParsingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cultureSwapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.experimentalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.betterNumberParserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutput.Location = new System.Drawing.Point(12, 51);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOutput.Size = new System.Drawing.Size(1168, 684);
            this.textBoxOutput.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generalToolStripMenuItem,
            this.grangerToolStripMenuItem,
            this.helpersToolStripMenuItem,
            this.experimentalToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1192, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // generalToolStripMenuItem
            // 
            this.generalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.skillParsingToolStripMenuItem});
            this.generalToolStripMenuItem.Name = "generalToolStripMenuItem";
            this.generalToolStripMenuItem.Size = new System.Drawing.Size(72, 24);
            this.generalToolStripMenuItem.Text = "General";
            // 
            // grangerToolStripMenuItem
            // 
            this.grangerToolStripMenuItem.Name = "grangerToolStripMenuItem";
            this.grangerToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.grangerToolStripMenuItem.Text = "Granger";
            // 
            // skillParsingToolStripMenuItem
            // 
            this.skillParsingToolStripMenuItem.Name = "skillParsingToolStripMenuItem";
            this.skillParsingToolStripMenuItem.Size = new System.Drawing.Size(158, 24);
            this.skillParsingToolStripMenuItem.Text = "Skill parsing";
            this.skillParsingToolStripMenuItem.Click += new System.EventHandler(this.skillParsingToolStripMenuItem_Click);
            // 
            // helpersToolStripMenuItem
            // 
            this.helpersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cultureSwapToolStripMenuItem});
            this.helpersToolStripMenuItem.Name = "helpersToolStripMenuItem";
            this.helpersToolStripMenuItem.Size = new System.Drawing.Size(72, 24);
            this.helpersToolStripMenuItem.Text = "Helpers";
            // 
            // cultureSwapToolStripMenuItem
            // 
            this.cultureSwapToolStripMenuItem.Name = "cultureSwapToolStripMenuItem";
            this.cultureSwapToolStripMenuItem.Size = new System.Drawing.Size(161, 24);
            this.cultureSwapToolStripMenuItem.Text = "CultureSwap";
            this.cultureSwapToolStripMenuItem.Click += new System.EventHandler(this.cultureSwapToolStripMenuItem_Click);
            // 
            // experimentalToolStripMenuItem
            // 
            this.experimentalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.betterNumberParserToolStripMenuItem});
            this.experimentalToolStripMenuItem.Name = "experimentalToolStripMenuItem";
            this.experimentalToolStripMenuItem.Size = new System.Drawing.Size(114, 24);
            this.experimentalToolStripMenuItem.Text = "Experimentals";
            // 
            // betterNumberParserToolStripMenuItem
            // 
            this.betterNumberParserToolStripMenuItem.Name = "betterNumberParserToolStripMenuItem";
            this.betterNumberParserToolStripMenuItem.Size = new System.Drawing.Size(212, 24);
            this.betterNumberParserToolStripMenuItem.Text = "BetterNumberParser";
            this.betterNumberParserToolStripMenuItem.Click += new System.EventHandler(this.betterNumberParserToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1192, 747);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem generalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem skillParsingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grangerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cultureSwapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem experimentalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem betterNumberParserToolStripMenuItem;
    }
}

