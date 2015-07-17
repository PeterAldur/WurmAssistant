using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WurmAssistant
{
    public partial class FormWurmAssistantUpdate : Form
    {
        public FormWurmAssistantUpdate()
        {
            InitializeComponent();
        }

        private void FormWurmAssistantUpdate_Load(object sender, EventArgs e)
        {
            AdjustTextBoxRMargin();
            try
            {
                richTextBox1.LoadFile(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "whatsnew.rtf"));
            }
            catch (Exception _e)
            {
                Logger.WriteLine("! Error while trying to load whatsnew");
                Logger.LogException(_e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", "CHANGELOG.txt");
            }
            catch { }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://forum.wurmonline.com/index.php?/topic/68031-windows-tool-wurm-assistant/");
            }
            catch { }
        }

        private void FormWurmAssistantUpdate_Resize(object sender, EventArgs e)
        {
            AdjustTextBoxRMargin();
        }

        private void AdjustTextBoxRMargin()
        {
            richTextBox1.RightMargin = richTextBox1.Size.Width - 35;
        }
    }
}
