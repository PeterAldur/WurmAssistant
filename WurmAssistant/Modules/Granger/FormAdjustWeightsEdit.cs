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
    public partial class FormAdjustWeightsEdit : Form
    {
        public FormAdjustWeightsEdit()
        {
            InitializeComponent();
        }

        private void FormAdjustWeightsEdit_Shown(object sender, EventArgs e)
        {
            try
            {
                numericUpDown1.Select(0, numericUpDown1.Text.Length);
            }
            catch (Exception _e)
            {
                Logger.LogException(_e);
            }
        }

        private void FormAdjustWeightsEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) 
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
