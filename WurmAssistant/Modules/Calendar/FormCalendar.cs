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
    public partial class FormCalendar : Form
    {
        ModuleCalendar ParentModule;

        public FormCalendar(ModuleCalendar parentModule)
        {
            InitializeComponent();
            this.ParentModule = parentModule;
            radioButtonWurmTime.Checked = ParentModule.UseWurmTimeForDisplay;
            radioButtonRealTime.Checked = !ParentModule.UseWurmTimeForDisplay;
            checkBoxSoundWarning.Checked = ParentModule.SoundWarning;
            checkBoxPopupWarning.Checked = ParentModule.PopupWarning;
            textBoxChosenSound.Text = ParentModule.SoundName;
        }

        public void RestoreFromMin()
        {
            if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;
        }

        public void UpdateSeasonOutput(List<ModuleCalendar.WurmSeasonOutputItem> outputList, bool wurmTime)
        {
            //ListView + double buffering
            //http://stackoverflow.com/questions/442817/c-sharp-flickering-listview-on-update
            ////
            listViewNFSeasons.Items.Clear();
            foreach (var item in outputList)
            {
                listViewNFSeasons.Items.Add(new ListViewItem(new string[] {
                    item.BuildName(), item.BuildTimeData(wurmTime), item.BuildLengthData(wurmTime) }));
            }
            //wurm date debug
            try
            {
                TimeSpan ts = ModuleTimingAssist.CurrentWurmDateTime.TimeAndDayOfYear;
                int month = (int)(ts.TotalDays / 28);
                string monthSTR = ModuleTimingAssist.WurmCalendarData.WurmMonthsNames[month];
                ts = TimeSpan.FromDays(ts.TotalDays - (double)(month * 28));
                int day = (int)(ts.Days) - 1;
                string daySTR = ModuleTimingAssist.WurmCalendarData.WurmDaysNames[day];
                ts = TimeSpan.FromDays(ts.TotalDays - (double)(day) - 1D);
                textBoxWurmDate.Text = String.Format("{0}, {1}, {2}", monthSTR, daySTR, ts.ToString());
            }
            catch
            {
                textBoxWurmDate.Text = "error";
            }
        }

        private void radioButtonWurmTime_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonWurmTime.Checked) ParentModule.UseWurmTimeForDisplay = true;
        }

        private void radioButtonRealTime_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonRealTime.Checked) ParentModule.UseWurmTimeForDisplay = false;
        }

        private void buttonChooseSeasons_Click(object sender, EventArgs e)
        {
            ParentModule.ChooseTrackedSeasons();
        }

        public void UpdateTrackedSeasonsList(string[] trackedSeasons)
        {
            if (trackedSeasons.Length > 0)
            {
                StringBuilder builder = new StringBuilder(120);
                foreach (string str in trackedSeasons)
                {
                    builder.Append(str).Append(", ");
                }
                builder.Remove(builder.Length - 2, 2);
                textBoxChosenSeasons.Text = builder.ToString();
            }
            else textBoxChosenSeasons.Text = "none";
        }

        private void buttonChooseSound_Click(object sender, EventArgs e)
        {
            FormChooseSound ChooseSoundUI = new FormChooseSound();
            if (ChooseSoundUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ParentModule.SoundName = ChooseSoundUI.ChosenSound;
                textBoxChosenSound.Text = ParentModule.SoundName;
            }
        }

        private void buttonClearSound_Click(object sender, EventArgs e)
        {
            ParentModule.SoundName = "none";
            textBoxChosenSound.Text = ParentModule.SoundName;
        }

        private void checkBoxSoundWarning_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.SoundWarning = checkBoxSoundWarning.Checked;
        }

        private void checkBoxPopupWarning_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.PopupWarning = checkBoxPopupWarning.Checked;
        }

        private void FormCalendar_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }
    }
}
