using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

namespace WurmAssistant
{
    public partial class FormWurmAssistantChoosePlayer : Form
    {
        WurmAssistant _ParentForm;
        public bool RequiresEngineRestart = false;
        string RegistryLogsPath;
        string PreviousPath;
        bool previousIfDailyLogging;
        Dictionary<string, string> AllLogPathsForPlayers = new Dictionary<string, string>();

        public FormWurmAssistantChoosePlayer(WurmAssistant _parentForm, string currentLogsPath)
        {
            InitializeComponent();
            _ParentForm = _parentForm;
            textBoxPathToLogFiles.Text = currentLogsPath;
            PreviousPath = currentLogsPath;
            RegistryLogsPath = _parentForm.TryGetWurmLogDirFromRegistry();
            try
            {
                BuildPlayersDict();
            }
            catch
            {
                string message = "Error while building players list, perhaps found path to log files is invalid, please choose log directory manually";
                MessageBox.Show(message);
                Logger.WriteLine("!!" + message);
            }
            PopulatePlayersList();
            if (_ParentForm.DailyLoggingMode)
            {
                radioButtonDaily.Checked = true;
                previousIfDailyLogging = true;
            }
            else
            {
                radioButtonMonthly.Checked = true;
                previousIfDailyLogging = false;
            }
            if (listBoxChoosePlayer.Items.Count > 0)
            {
                listBoxChoosePlayer.SetSelected(0, true);
            }
        }

        void BuildPlayersDict()
        {
            string[] allplayers = Directory.GetDirectories(RegistryLogsPath + @"players\");
            foreach (var playerpath in allplayers)
            {
                string playername = GeneralHelper.GetLastFolderNameFromPath(playerpath);
                AllLogPathsForPlayers.Add(playername, RegistryLogsPath + @"players\" + playername + @"\logs");
            }
        }

        void PopulatePlayersList()
        {
            foreach (var keyvalue in AllLogPathsForPlayers)
            {
                listBoxChoosePlayer.Items.Add(keyvalue.Key);
            }
        }

        private void buttonChooseManually_Click(object sender, EventArgs e)
        {
            if (textBoxPathToLogFiles.Text != "") folderBrowserDialog1.SelectedPath = textBoxPathToLogFiles.Text;
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxPathToLogFiles.Text = folderBrowserDialog1.SelectedPath;
                listBoxChoosePlayer.ClearSelected();
            }
        }

        private void listBoxChoosePlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxChoosePlayer.SelectedIndex != -1)
            {
                textBoxPathToLogFiles.Text = AllLogPathsForPlayers[Convert.ToString(listBoxChoosePlayer.SelectedItem)];
            }
            else
            {
                if (listBoxChoosePlayer.Items.Count > 0)
                {
                    Color oldcolor = labelWarning.BackColor;
                    labelWarning.BackColor = Color.Yellow;
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (PreviousPath != textBoxPathToLogFiles.Text) RequiresEngineRestart = true;
            if (radioButtonMonthly.Checked == true)
            {
                _ParentForm.DailyLoggingMode = false;
                if (previousIfDailyLogging == true) RequiresEngineRestart = true;
            }
            if (radioButtonDaily.Checked == true)
            {
                _ParentForm.DailyLoggingMode = true;
                if (previousIfDailyLogging == false) RequiresEngineRestart = true;
            }
        }
    }
}
