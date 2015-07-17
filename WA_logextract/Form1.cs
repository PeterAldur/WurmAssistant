using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace WA_logextract
{
    public partial class FormExtractor : Form
    {
        public FormExtractor()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            try
            {
                Process.Start(Path.Combine(appdata, @"AldurCraft\WurmAssistant\WurmAssistant_log.txt"));
            }
            catch
            {
            }
        }
    }
}
