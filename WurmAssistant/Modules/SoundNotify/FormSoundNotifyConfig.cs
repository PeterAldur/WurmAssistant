using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace WurmAssistant
{
    public partial class FormSoundNotifyConfig : Form
    {
        ModuleSoundNotify ParentModule;

        public FormSoundNotifyConfig()
        {
            InitializeComponent();
        }

        public FormSoundNotifyConfig(ModuleSoundNotify module)
            : this()
        {
            this.ParentModule = module;
            numericUpDownQueueDelay.Value = Convert.ToDecimal(ParentModule.QueueDefDelay);
            trackBarGlobalVolume.Value = (int)(ParentModule.GetSoundEngineVolume() * 100);
            textBoxQueSoundName.Text = ParentModule.GetQueueSoundForUI();
        }

        private void RefreshBankAndList()
        {
            SoundBank.InitSoundBank();
            RefreshList();
        }

        private void RefreshList()
        {
            listViewSounds.Items.Clear();
            List<PlaylistEntry> playlist = ParentModule.getPlaylist();
            int counter = 1;
            foreach (PlaylistEntry entry in playlist)
            {
                listViewSounds.Items.Add(counter.ToString());
                counter++;
                listViewSounds.Items[listViewSounds.Items.Count - 1].SubItems.Add(entry.SoundName);

                if (entry.isCustomRegex) listViewSounds.Items[listViewSounds.Items.Count - 1].SubItems.Add(entry.Condition);
                else listViewSounds.Items[listViewSounds.Items.Count - 1].SubItems.Add(ParentModule.ConvertRegexToCondOutput(entry.Condition));

                string allspecials = "";
                bool firstspecial = true;
                foreach (string special in entry.SpecialSettings)
                {
                    if (firstspecial)
                    {
                        allspecials = special;
                        firstspecial = false;
                    }
                    else allspecials += ", " + special;
                }
                listViewSounds.Items[listViewSounds.Items.Count - 1].SubItems.Add(allspecials);
                listViewSounds.Items[listViewSounds.Items.Count - 1].SubItems.Add(entry.isActive.ToString());
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            RefreshBankAndList();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormSoundNotifyConfigDialog dialog = new FormSoundNotifyConfigDialog(ParentModule, FormSoundNotifyConfigDialogMode.Add);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string condition = dialog.textBoxChooseCond.Text;
                List<string> specCond = new List<string>();

                if (dialog.checkBoxUseRegexSemantics.Checked)
                {
                    specCond.Add("s:CustomRegex");
                }
                else condition = ParentModule.ConvertCondOutputToRegex(condition);

                if (dialog.checkedListBoxSearchIn.CheckedItems.Count > 0)
                {
                    foreach (string line in dialog.checkedListBoxSearchIn.CheckedItems)
                    {
                        specCond.Add(line);
                    }
                }
                ParentModule.AddPlaylistEntry(dialog.listBoxChooseSound.Text, condition, specCond, true);
                RefreshBankAndList();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (listViewSounds.SelectedItems.Count > 0)
            {
                int oldEntryIndex = Convert.ToInt32(listViewSounds.SelectedItems[0].Text) - 1;
                bool oldActive = ParentModule.getPlaylistEntryAtIndex(oldEntryIndex).isActive;
                FormSoundNotifyConfigDialog dialog = new FormSoundNotifyConfigDialog(
                    ParentModule,
                    FormSoundNotifyConfigDialogMode.Edit,
                    ParentModule.getPlaylistEntryAtIndex(oldEntryIndex));
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string condition = dialog.textBoxChooseCond.Text;
                    ParentModule.RemovePlaylistEntry(oldEntryIndex);
                    List<string> specCond = new List<string>();

                    if (dialog.checkBoxUseRegexSemantics.Checked)
                    {
                        specCond.Add("s:CustomRegex");
                    }
                    else condition = ParentModule.ConvertCondOutputToRegex(condition);

                    if (dialog.checkedListBoxSearchIn.CheckedItems.Count > 0)
                    {
                        foreach (string line in dialog.checkedListBoxSearchIn.CheckedItems)
                        {
                            specCond.Add(line);
                        }
                    }
                    // this is calling insert, because its passing old index
                    ParentModule.AddPlaylistEntry(dialog.listBoxChooseSound.Text, condition, specCond, oldActive, oldEntryIndex);
                    RefreshBankAndList();
                }
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            removeEntry(true);
        }

        private void FormWarningsConfig_Load(object sender, EventArgs e)
        {
            RefreshBankAndList();
            RefreshQueueSoundUI();
        }

        private void RefreshQueueSoundUI()
        {
            if (ParentModule.QueueSoundEnabled)
            {
                checkBoxToggleQSound.Checked = true;
            }
            else
            {
                checkBoxToggleQSound.Checked = false;
            }
        }

        private void buttonOpenFolder_Click(object sender, EventArgs e) //depr
        {
            try
            {
                System.Diagnostics.Process.Start("explorer.exe", SoundBank.SoundsDirectory);
            }
            catch (Win32Exception)
            {
                MessageBox.Show("Error: could not open folder");
            }
        }

        private void numericUpDownQueueDelay_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDownQueueDelay.Value < 0) numericUpDownQueueDelay.Value = 0;
            ParentModule.QueueDefDelay = Convert.ToDouble(numericUpDownQueueDelay.Value);
        }

        private void checkBoxToggleQSound_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxToggleQSound.Checked)
            {
                ParentModule.QueueSoundEnabled = true;
            }
            else ParentModule.QueueSoundEnabled = false;
            RefreshQueueSoundUI();
        }

        void removeEntry(bool ifShowWarning)
        {
            if (listViewSounds.SelectedItems.Count > 0)
            {
                if (ifShowWarning && MessageBox.Show("Are you sure? \n\nNote: You can use DELETE key to avoid this popup", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                {
                    ParentModule.RemovePlaylistEntry(Convert.ToInt32(listViewSounds.SelectedItems[0].Text) - 1);
                    RefreshBankAndList();
                }
                else if (!ifShowWarning)
                {
                    ParentModule.RemovePlaylistEntry(Convert.ToInt32(listViewSounds.SelectedItems[0].Text) - 1);
                    RefreshBankAndList();
                }
            }
        }

        private void listViewSounds_DoubleClick(object sender, EventArgs e)
        {
            buttonEdit.PerformClick();
           
        }

        private void listViewSounds_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
                removeEntry(false);
        }

        public void RestoreFromMin()
        {
            if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;
        }

        private void buttonMute_Click(object sender, EventArgs e)
        {
            if (ParentModule.ModuleMuted)
            {
                ParentModule.ModuleMuted = false;
                buttonMute.Image = Properties.Resources.SoundEnabledSmall;
                this.Text = "SoundNotify Manager";
            }
            else
            {
                ParentModule.ModuleMuted = true;
                SoundBank.StopSounds();
                buttonMute.Image = Properties.Resources.SoundDisabledSmall;
                this.Text = "SoundNotify Manager (muted)";
            }
        }

        private void trackBarGlobalVolume_Scroll(object sender, EventArgs e)
        {
            SoundBank.ChangeGlobalVolume(((float)trackBarGlobalVolume.Value / 100));
        }

        private void buttonManageSNDBank_Click(object sender, EventArgs e)
        {
            WurmAssistant.ZeroRef.OpenSoundBank();
        }

        private void listViewSounds_MouseClick(object sender, MouseEventArgs e)
        {
            // swap active/inactive on the sound
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                ParentModule.TogglePlaylistEntryActive(Convert.ToInt32(listViewSounds.SelectedItems[0].Text) - 1);
                RefreshList();
            }
        }

        private void buttonChangeQueSound_Click(object sender, EventArgs e)
        {
            ParentModule.SetQueueSound();
        }

        public void UpdateSoundName(string name)
        {
            this.textBoxQueSoundName.Text = name;
        }

        private void buttonQueSoundHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "This is a preconfigured setting, that once set correctly, will play a sound, once your in-game character finishes doing his actions.\n\n" +
                "Queue delay setting determines, how long program will wait, before playing the sound. If your in-game character starts new action within this time window, the sound will not play. " +
                "Too low delay may cause sound to play mid-queue (due to lags and other factors). The recommended minimum value (and default) is 1.0 second. "+
                "You can also set the delay to a high value, so that sound will play only if you forget about your character. "+
                "For example, if you set it to 10 seconds, then any action started before those 10 seconds pass, will reset this timer and sound will not play.",
                "Queue sound description", MessageBoxButtons.OK);
        }

        private void buttonPlaylistHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "You can use this playlist to set a sound to play on anything, that happens in your in-game logs (this means event window, chats, skill tab and so on).\n\n" +
                "First you should manage sound bank and add some sound files, then you can add new entries. Choose a sound, condition (which is any part of the actual in-game message), then finally select appropriate log type.\n" +
                "If you copy-paste a log line, it will have it's timestamp removed, that is not a bug. Also remember, any changing elements in log messages (for example creature names) should be replaced by * (asterisk) symbol.\n\n"+
                "See examples in program guide.\n\n" +
                "Custom Regex is a special option for advanced users, allows to type a regular expression to match by using C# Regex semantics.\n\n"+ 
                "There is also a preconfigured \"queue sound\" option, which works very well as a reminder, that your character finished doing his actions.",
                "SoundNotify help", MessageBoxButtons.OK);
        }

        private void buttonClearQueSound_Click(object sender, EventArgs e)
        {
            ParentModule.QueueSoundName = null;
            textBoxQueSoundName.Text = "default";
        }
    }
}
