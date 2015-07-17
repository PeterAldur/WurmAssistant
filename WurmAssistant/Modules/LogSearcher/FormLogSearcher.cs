using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace WurmAssistant
{
    public partial class FormLogSearcher : Form
    {
        ModuleLogSearcher ParentModule;
        LogSearchData HandleToCurrentLogSearchData;

        public FormLogSearcher(ModuleLogSearcher parentModule)
        {
            InitializeComponent();
            ParentModule = parentModule;

            //init all search boxes and choose default values for them
            dateTimePickerTimeFrom.Value = DateTime.Now;
            dateTimePickerTimeTo.Value = DateTime.Now;
            comboBoxPlayerName.Items.AddRange(ParentModule.GetAllPlayerNames().ToArray());
            comboBoxPlayerName.Text = parentModule.GetCurrentPlayer();
            comboBoxLogType.Items.AddRange(GameLogTypesEX.GetAllNames());
            comboBoxLogType.Text = GameLogTypesEX.GetNameForLogType(GameLogTypes.Event);
            comboBoxSearchType.Items.AddRange(SearchTypesEX.GetAllNames());
            comboBoxSearchType.Text = SearchTypesEX.GetNameForSearchType(SearchTypes.RegexEscapedCaseIns);
        }

        public delegate void DisplaySearchResultsCallback(LogSearchData logSearchData);

        public void DisplaySearchResults(LogSearchData logSearchData)
        {
            buttonCommitSearch.Text = "Loading results...";
            this.Refresh();
            if (!logSearchData.StopSearching)
            {
                labelAllResults.Text = "All results: " + logSearchData.SearchResults.Count;

                richTextBoxAllLines.Visible = false;
                listBoxAllResults.Visible = false;
                labelWorking.Show();
                this.Refresh();

                richTextBoxAllLines.Clear();
                listBoxAllResults.Items.Clear();
                richTextBoxAllLines.Lines = logSearchData.AllLinesArray;
                bool tooManyToProcess = false;
                bool tooManyToHighlight = false;
                if (logSearchData.SearchResults.Count > 20000) tooManyToProcess = true;
                if (logSearchData.SearchResults.Count > 5000) tooManyToHighlight = true;
                if (!tooManyToProcess)
                {
                    foreach (LogSearchData.SingleSearchMatch searchmatch in logSearchData.SearchResults)
                    {
                        if (!logSearchData.StopSearching && !ParentModule.isAppClosing) //avoid app exit exceptions due to Application.DoEvents
                        {
                            string matchDesc = "";
                            matchDesc += searchmatch.MatchDate;
                            if (!tooManyToHighlight)
                            {
                                richTextBoxAllLines.Select((int)searchmatch.Begin, (int)searchmatch.Length);
                                richTextBoxAllLines.SelectionBackColor = Color.LightBlue;
                            }
                            listBoxAllResults.Items.Add(matchDesc);
                            Application.DoEvents(); //improve interface responsiveness and allow cancelling
                        }
                    }
                }
                else
                {
                    listBoxAllResults.Items.Add("too many matches");
                    listBoxAllResults.Items.Add("narrow the search");
                }

                if (!ParentModule.isAppClosing) //avoid app exit exceptions due to Application.DoEvents
                {
                    labelWorking.Hide();
                    buttonCancelSearch.Visible = false;
                    listBoxAllResults.Visible = true;
                    richTextBoxAllLines.Visible = true;
                    richTextBoxAllLines.Select(0, 0);
                    richTextBoxAllLines.ScrollToCaret();
                }
            }
            if (!ParentModule.isAppClosing) //avoid app exit exceptions due to Application.DoEvents
            {
                buttonCommitSearch.Text = "Search";
                buttonCommitSearch.Enabled = true;
            }
        }

        void PerformSearch()
        {
            try
            {
                // create new search data container
                LogSearchData logSearchData = new LogSearchData();
                HandleToCurrentLogSearchData = logSearchData;
                // enable cancel button
                buttonCancelSearch.Visible = true;
                // clear old results
                richTextBoxAllLines.Clear();
                listBoxAllResults.Items.Clear();
                // write container with return address
                logSearchData.CallerControl = this;
                // adjust timeto if necessary (monitor)
                dateTimePickerTimeFrom.Value = new DateTime(
                    dateTimePickerTimeFrom.Value.Year,
                    dateTimePickerTimeFrom.Value.Month,
                    dateTimePickerTimeFrom.Value.Day,
                    0, 0, 0);
                dateTimePickerTimeTo.Value = new DateTime(
                    dateTimePickerTimeTo.Value.Year,
                    dateTimePickerTimeTo.Value.Month,
                    dateTimePickerTimeTo.Value.Day,
                    23, 59, 59);
                Debug.WriteLine("Search begin");
                Debug.WriteLine("from " + dateTimePickerTimeFrom.Value + " to " + dateTimePickerTimeTo.Value);
                // drop all search criteria into the container
                logSearchData.BuildSearchData(
                    comboBoxPlayerName.Text,
                    GameLogTypesEX.GetLogTypeForName(comboBoxLogType.Text),
                    dateTimePickerTimeFrom.Value,
                    dateTimePickerTimeTo.Value,
                    textBoxSearchKey.Text,
                    SearchTypesEX.GetSearchTypeForName(comboBoxSearchType.Text)
                    );
                // add pm player if applies
                if (comboBoxLogType.Text == GameLogTypesEX.GetNameForLogType(GameLogTypes.PM))
                {
                    logSearchData.SetPM_Player(textBoxPM.Text);
                }
                // pass the container further
                ParentModule.PerformSearch(logSearchData);
            }
            catch (Exception _e)
            {
                MessageBox.Show("Error while starting search, this is a bug please report!");
                Logger.WriteLine("LogSearcher: Error while performing search");
                Logger.LogException(_e);
            }
        }

        public void RestoreFromMin()
        {
            if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;
        }

        private void FormLogSearcher_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void buttonCommitSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        public void UpdateUIAboutScheduledSearch(SearchStatus searchStatus)
        {
            if (searchStatus == SearchStatus.Busy)
            {
                buttonCommitSearch.Enabled = false;
                buttonCommitSearch.Text = "Busy, please wait...";
            }
            else if (searchStatus == SearchStatus.Searching)
            {
                buttonCommitSearch.Enabled = false;
                buttonCommitSearch.Text = "Searching...";
            }
        }

        private void buttonCancelSearch_Click(object sender, EventArgs e)
        {
            if (HandleToCurrentLogSearchData != null)
            {
                HandleToCurrentLogSearchData.StopSearching = true;
            }
        }

        private void listBoxAllResults_Click(object sender, EventArgs e)
        {
            try
            {
                LogSearchData.SingleSearchMatch matchdata = HandleToCurrentLogSearchData.SearchResults[listBoxAllResults.SelectedIndex];
                richTextBoxAllLines.Select((int)matchdata.Begin, (int)matchdata.Length);
                richTextBoxAllLines.Focus();
            }
            catch
            {

            }
        }

        private void textBoxSearchKey_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.ProcessTabKey(true);
            }
        }

        private void textBoxSearchKey_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
            }
            
        }

        private void dateTimePickerTimeFrom_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.ProcessTabKey(true);
            }
        }

        private void dateTimePickerTimeTo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.ProcessTabKey(true);
            }
        }

        private void comboBoxLogType_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.ProcessTabKey(true);
            }
        }

        private void comboBoxPlayerName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.ProcessTabKey(true);
            }
        }

        private void comboBoxSearchType_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.ProcessTabKey(true);
            }
        }

        private void textBoxPM_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.ProcessTabKey(true);
            }
        }

        private void comboBoxLogType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxLogType.Text == GameLogTypesEX.GetNameForLogType(GameLogTypes.PM))
            {
                labelPM.Visible = true;
                textBoxPM.Visible = true;
                textBoxPM.TabStop = true;
            }
            else
            {
                labelPM.Visible = false;
                textBoxPM.Visible = false;
                textBoxPM.TabStop = false;
            }
        }

        private void buttonForceRecache_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Confirm recache", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk) == System.Windows.Forms.DialogResult.OK)
            {
                buttonForceRecache.Text = "Working...";
                buttonForceRecache.Enabled = false;
                buttonCommitSearch.Enabled = false;
                ParentModule.ForceRecache();
            }
        }

        public delegate void OnRecacheCompleteCallback();

        public void InvokeOnRecacheComplete()
        {
            buttonCommitSearch.Enabled = true;
            buttonForceRecache.Text = "Force Refresh Cache";
            buttonForceRecache.Enabled = true;
        }
    }
}
