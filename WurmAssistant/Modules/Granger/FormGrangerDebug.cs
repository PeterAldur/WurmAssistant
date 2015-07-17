using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WurmAssistant.Granger
{
    public partial class FormGrangerDebug : Form
    {
        List<string> allEntries = new List<string>();

        public FormGrangerDebug()
        {
            InitializeComponent();
        }

        public void WriteToTextbox(string message)
        {
            allEntries.Add(message);
            if (allEntries.Count > 10000) allEntries.RemoveRange(0, 1000);
            textBox1.Lines = allEntries.ToArray();
        }

        private void FormGrangerDebug_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
