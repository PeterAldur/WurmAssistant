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
    public partial class FormChooseHerd : Form
    {
        public string Result = null;

        public FormChooseHerd(string[] herdNames)
        {
            InitializeComponent();
            listBox1.Items.AddRange(herdNames);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count > 0)
            {
                Result = listBox1.SelectedItem.ToString();
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Select target herd");
            }
        }
    }
}
