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
    public partial class FormWurmAssistantConfig : Form
    {
        WurmAssistant _ParentForm;

        public FormWurmAssistantConfig(WurmAssistant parentform)
        {
            this._ParentForm = parentform;
            InitializeComponent();
            numericUpDownMillis.Value = _ParentForm.TimerTickRate;
            checkBoxDisplayEntries.Checked = _ParentForm.DisplayAllLogEvents;
            checkBoxMiniToTray.Checked = _ParentForm.AppSetToMinimizeToTray;
            checkBoxStartMinimized.Checked = _ParentForm.StartMinimized;
        }

        private void numericUpDownMillis_ValueChanged(object sender, EventArgs e)
        {
            textBoxSeconds.Text = (numericUpDownMillis.Value / 1000).ToString();
        }

        private void checkBoxDisplayEntries_CheckedChanged(object sender, EventArgs e)
        {
            _ParentForm.DisplayAllLogEvents = checkBoxDisplayEntries.Checked;
        }

        private void checkBoxMiniToTray_CheckedChanged(object sender, EventArgs e)
        {
            // handle in parent
        }

        private void checkBoxDisableSleepMode_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
