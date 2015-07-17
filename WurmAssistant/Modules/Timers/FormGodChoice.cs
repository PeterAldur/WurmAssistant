using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WurmAssistant
{
    public partial class FormGodChoice : Form
    {
        public ModuleTimers.PriesthoodTimer.WurmReligions ResultWurmReligion;

        public FormGodChoice()
        {
            InitializeComponent();
        }

        private void FormGodChoice_Load(object sender, EventArgs e)
        {
            listBox1.Items.AddRange(Enum.GetNames(typeof(ModuleTimers.PriesthoodTimer.WurmReligions)));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0)
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResultWurmReligion = (ModuleTimers.PriesthoodTimer.WurmReligions)Enum.Parse(typeof(ModuleTimers.PriesthoodTimer.WurmReligions), listBox1.SelectedItem.ToString());
        }
    }
}
