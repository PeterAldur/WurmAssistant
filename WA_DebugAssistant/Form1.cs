using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;

namespace WA_DebugAssistant
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            textBox1.Lines = null;
            backgroundWorker1.RunWorkerAsync();
            progressBar1.Visible = true;
            label1.Visible = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> sl = new List<string>(100000);
            sl.Add(System.Environment.OSVersion.ToString());
            sl.Add("");
            string wurmdir = Convert.ToString(Registry.GetValue(@"HKEY_CURRENT_USER\Software\JavaSoft\Prefs\com\wurmonline\client", "wurm_dir", null));
            if (wurmdir != null)
            {
                wurmdir = wurmdir.Replace(@"//", @"\");
                wurmdir = wurmdir.Replace(@"/", @"");
                wurmdir = wurmdir.Trim();
                if (!wurmdir.EndsWith(@"\", StringComparison.Ordinal)) wurmdir += @"\";
                if (wurmdir.EndsWith(@"players\", StringComparison.Ordinal)) wurmdir = wurmdir.Remove(wurmdir.Length - 8);
                sl.Add("WurmDir: " + wurmdir);
                sl.Add("==DIRECTORY STRUCTURE==");
                AddDirStructure(wurmdir, sl);
            }
            else
            {
                sl.Add("WurmDir: registry retrieve failed");
            }
            e.Result = sl;
        }

        List<string> AddDirStructure(string directory, List<string> sl)
        {
            sl.Add("Directory: " + directory);
            string[] files = Directory.GetFiles(directory);
            foreach (string file in files)
            {
                sl.Add(file);
            }
            string[] directories = Directory.GetDirectories(directory);
            foreach (string dir in directories)
            {
                AddDirStructure(dir, sl);
            }
            return sl;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.Visible = false;
            label1.Visible = false;
            List<string> result = (List<string>)e.Result;
            textBox1.Lines = result.ToArray();
            button2.Enabled = true;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.Text);
        }
    }
}
