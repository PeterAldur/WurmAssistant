using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WurmAssistant
{
    public enum FormSoundNotifyConfigDialogMode { Add, Edit, ChangeQueueSound }

    public partial class FormSoundNotifyConfigDialog : Form
    {
        ModuleSoundNotify ParentModule;

        public FormSoundNotifyConfigDialog()
        {
            InitializeComponent();
        }

        public FormSoundNotifyConfigDialog(ModuleSoundNotify module, FormSoundNotifyConfigDialogMode mode, PlaylistEntry parPlaylistEntry = null)
            : this()
        {
            this.ParentModule = module;
            listBoxChooseSound.Items.AddRange(SoundBank.getSoundsArray());
            listBoxChooseSound.Sorted = true;

            checkedListBoxSearchIn.Items.AddRange((object[])GameLogTypesEX.GetAllNames());

            if (mode == FormSoundNotifyConfigDialogMode.Edit)
            {
                // change title
                this.Text = "Edit entry";
                // choose sound
                if (listBoxChooseSound.Items.Contains((object)parPlaylistEntry.SoundName))
                {
                    listBoxChooseSound.SetSelected((int)(listBoxChooseSound.Items.IndexOf((object)(parPlaylistEntry.SoundName))), true);
                }
                // input condition
                if (!parPlaylistEntry.isCustomRegex) textBoxChooseCond.Text = ParentModule.ConvertRegexToCondOutput(parPlaylistEntry.Condition);
                else textBoxChooseCond.Text = parPlaylistEntry.Condition;
                // choose logs
                foreach (var cond in parPlaylistEntry.SpecialSettings)
                {
                    if (GameLogTypesEX.doesTypeExist(cond))
                    {
                        checkedListBoxSearchIn.SetItemChecked(checkedListBoxSearchIn.Items.IndexOf((object)(cond)), true);
                    }
                }
                // choose specials
                foreach (var cond in parPlaylistEntry.SpecialSettings)
                {
                    if (cond == "s:CustomRegex")
                    {
                        checkBoxUseRegexSemantics.Checked = true;
                    }
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            bool soundExists = false;
            foreach (string str in SoundBank.getSoundsArray())
            {
                if (listBoxChooseSound.Text == str)
                    soundExists = true;
            }
            if (soundExists == false)
            {
                MessageBox.Show("Choose sound");
                this.DialogResult = DialogResult.None;
            }
            else if (textBoxChooseCond.Text.Trim() == "")
            {
                if (checkBoxUseRegexSemantics.Checked == false)
                {
                    MessageBox.Show("Condition cannot be empty");
                    this.DialogResult = DialogResult.None;
                }
            }
            else if (textBoxChooseCond.Text.Contains(ParentModule.DefDelimiter[0]))
            {
                MessageBox.Show("Condition cannot contain " + ParentModule.DefDelimiter[0]+" symbol");
                this.DialogResult = DialogResult.None;
            }

            textBoxChooseCond.Text = textBoxChooseCond.Text.Trim();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            SoundBank.PlaySound(listBoxChooseSound.Text);
        }

        private void buttonCheckAll_Click(object sender, EventArgs e)
        {
            int elemnumber = checkedListBoxSearchIn.Items.Count;
            for (int i = 0; i < elemnumber; i++)
            {
                checkedListBoxSearchIn.SetItemChecked(i, true);
            }
        }

        private void listBoxChooseSound_DoubleClick(object sender, EventArgs e)
        {
            buttonPlay.PerformClick();
        }

        private void textBoxChooseCond_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBoxChooseCond.Text.Length >= 10)
                {
                    if (Regex.IsMatch(textBoxChooseCond.Text.Substring(0, 10), @"\[\d\d:\d\d:\d\d\]"))
                    {
                        textBoxChooseCond.Text = textBoxChooseCond.Text.Remove(0, 10);
                    }
                }
            }
            catch (Exception _e)
            {
                Logger.WriteLine("Exception at textBoxChooseCond_TextChanged");
                Logger.LogException(_e);
            }
        }
    }
}
