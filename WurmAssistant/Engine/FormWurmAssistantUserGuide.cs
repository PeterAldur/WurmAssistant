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
    public partial class FormWurmAssistantUserGuide : Form
    {
        public FormWurmAssistantUserGuide()
        {
            InitializeComponent();
        }

        private void FormWurmAssistantUserGuide_Load(object sender, EventArgs e)
        {
            AdjustTextBoxRMargin();
            try
            {
                richTextBox1.LoadFile(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "userguide.rtf"));
            }
            catch (Exception _e)
            {
                Logger.WriteLine("! UserGuide: error while loading guide");
                Logger.LogException(_e);
            }
        }

        private void richTextBox1_Resize(object sender, EventArgs e)
        {
            AdjustTextBoxRMargin();
            
        }

        private void AdjustTextBoxRMargin()
        {
            richTextBox1.RightMargin = richTextBox1.Size.Width - 35;
        }
    }
}
