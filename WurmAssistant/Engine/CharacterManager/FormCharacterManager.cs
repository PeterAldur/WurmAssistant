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
    public partial class FormCharacterManager : Form
    {
        string[] oldPlayers;
        public string[] ResultPlayers;
        string oldWurmDir;
        string oldWurmDirOverride;
        bool oldIsDailyLogging;

        public bool RequiresLogSearchManagerRebuild = false;

        public FormCharacterManager(string[] players)
        {
            InitializeComponent();

            this.oldPlayers = players;
            oldIsDailyLogging = WurmAssistant.ZeroRef.DailyLoggingMode;
            oldWurmDir = WurmPaths.WurmDir;
            oldWurmDirOverride = WurmAssistant.ZeroRef.WurmDirOverride;

            if (WurmAssistant.ZeroRef.DailyLoggingMode) radioButtonDaily.Checked = true;
            else radioButtonMonthly.Checked = true;

            textBoxWurmPath.Text = WurmPaths.WurmDir == null ? "" : WurmPaths.WurmDir;
            UpdateButtonTextSetManuallyWurmPath();

            listBoxMainChar.Items.AddRange(WurmPaths.AllPlayers);

            if (players != null)
            {
                for (int i = 0; i < listBoxMainChar.Items.Count; i++)
                {
                    if (listBoxMainChar.Items[i].ToString() == players[0])
                    {
                        listBoxMainChar.SetSelected(i, true);
                    }
                }

                for (int i = 0; i < checkedListBoxAltChars.Items.Count; i++)
                {
                    foreach (string player in players)
                    {
                        if (checkedListBoxAltChars.Items[i].ToString() == player)
                        {
                            checkedListBoxAltChars.SetItemChecked(i, true);
                        }
                    }
                }
            }
        }

        void UpdateButtonTextSetManuallyWurmPath()
        {
            if (WurmAssistant.ZeroRef.WurmDirOverride == null)
            {
                buttonSetManuallyWurmPath.Text = "Set manually...";
                labelWurmPath.Text = "Wurm client path (autodetected):";
            }
            else
            {
                buttonSetManuallyWurmPath.Text = "Clear manual";
                labelWurmPath.Text = "Wurm client path (manual override):";
            }
        }

        private void radioButtonMonthly_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonMonthly.Checked) WurmAssistant.ZeroRef.DailyLoggingMode = false;
            else WurmAssistant.ZeroRef.DailyLoggingMode = true;
        }

        private void radioButtonDaily_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDaily.Checked) WurmAssistant.ZeroRef.DailyLoggingMode = true;
            else WurmAssistant.ZeroRef.DailyLoggingMode = false;
        }

        private void buttonSetManuallyWurmPath_Click(object sender, EventArgs e)
        {
            if (WurmAssistant.ZeroRef.WurmDirOverride == null)
            {
                if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (folderBrowserDialog1.SelectedPath != null)
                    {
                        textBoxWurmPath.Text = folderBrowserDialog1.SelectedPath;
                        WurmAssistant.ZeroRef.WurmDirOverride = textBoxWurmPath.Text;
                        listBoxMainChar.Items.Clear();
                        checkedListBoxAltChars.Items.Clear();
                        WurmPaths.Initialize();
                        listBoxMainChar.Items.AddRange(WurmPaths.AllPlayers);
                    }
                }
            }
            else
            {
                WurmAssistant.ZeroRef.WurmDirOverride = null;
                WurmPaths.Initialize();
                textBoxWurmPath.Text = WurmPaths.WurmDir == null ? "" : WurmPaths.WurmDir;
            }
            UpdateButtonTextSetManuallyWurmPath();
        }

        private void listBoxMainChar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxMainChar.SelectedIndex >= 0)
            {
                checkedListBoxAltChars.Items.Clear();
                List<string> altList = new List<string>();
                foreach (string player in WurmPaths.AllPlayers)
                {
                    if (player != listBoxMainChar.SelectedItem.ToString())
                    {
                        checkedListBoxAltChars.Items.Add(player);
                    }
                }
            }
        }

        private void FormCharacterManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;

            List<string> result = new List<string>();
            if (listBoxMainChar.SelectedIndex >= 0)
            {
                result.Add(listBoxMainChar.SelectedItem.ToString());
                foreach (var item in checkedListBoxAltChars.CheckedItems)
                {
                    result.Add(item.ToString());
                }
            }

            if (result.Count == 0)
            {
                WurmAssistant.ZeroRef.TrackedPlayers = null;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                bool isChanged = false;

                if (oldPlayers != null)
                {
                    if (oldPlayers[0] == result[0])
                    {
                        if (oldPlayers.Length == result.Count)
                        {
                            foreach (string player in oldPlayers)
                            {
                                if (!result.Contains(player)) isChanged = true;
                            }
                        }
                        else isChanged = true;
                    }
                    else isChanged = true;
                }
                else isChanged = true;

                if (isChanged)
                {
                    WurmAssistant.ZeroRef.TrackedPlayers = result.ToArray<string>();
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }

            if (oldWurmDir != WurmPaths.WurmDir || oldWurmDirOverride != WurmAssistant.ZeroRef.WurmDirOverride)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.RequiresLogSearchManagerRebuild = true;
            }

            if (oldIsDailyLogging != WurmAssistant.ZeroRef.DailyLoggingMode)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void buttonCloseAndApply_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
