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
    public partial class FormTimersDebug : Form
    {
        public FormTimersDebug()
        {
            InitializeComponent();
        }

        private void FormTimersDebug_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                this.WindowState = FormWindowState.Minimized;
            }
        }

        internal void UpdateDebugOutput()
        {
            if (this.Visible)
            {
                TimeSpan ts = (DateTime.Now - ModuleTimingAssist.ServerUpSince);
                string uptime_output = ts.ToString("%d") + " days " + ts.ToString(@"hh\:mm\:ss");
                textBoxServerUptime.Text = uptime_output;
            }
        }

        internal void UpdateMeditHistoryOutput(string[] medit_history)
        {
            listBoxMeditHistory.Items.Clear();
            listBoxMeditHistory.Items.AddRange(medit_history);
        }

        internal void UpdatePrayerHistoryOutput(string[] prayer_history)
        {
            listBoxPrayHistory.Items.Clear();
            listBoxPrayHistory.Items.AddRange(prayer_history);
        }

        internal void UpdateAlignmentHistoryOutput(string[] alignment_history)
        {
            listBoxAlignmentHistory.Items.Clear();
            listBoxAlignmentHistory.Items.AddRange(alignment_history);
        }
    }
}
