using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WurmAssistant.Granger
{
    public partial class FormHorseManager : Form
    {
        ModuleGranger ParentModule;

        public FormHorseManager(ModuleGranger parentmodule)
        {
            this.ParentModule = parentmodule;
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.Hide();
            InitControls();
            timerRefresh.Enabled = true;
        }

        void InitControls()
        {
            checkBoxEnable.Checked = ParentModule.ModuleEnabled;
            checkBoxLiveAdvisor.Checked = ParentModule.LiveAdvisor;
            checkBoxAdviseAvailable.Checked = ParentModule.AdviseOnlyAvailable;
            checkBoxIncludePotential.Checked = ParentModule.IncludePotentialValue;
            checkBoxPairingMissingTraits.Checked = ParentModule.PreferMissingTraits;
            checkBoxNegativeExclude.Checked = ParentModule.ExcludeNegatives;
            checkBoxDontExcludeInbreed.Checked = ParentModule.DontExcludeInbreeding;
            checkBoxDisableAdvisor.Checked = ParentModule.DisableAdvisor;
            labelAHSkill.Text = "AH Skill for: " + ParentModule.PlayerName;

            int ahskill = ParentModule.AHSkill;
            if (ahskill > 200) ahskill = 100;
            if (ahskill < 0) ahskill = 0;
            numericUpDownAHSkill.Value = ahskill;

            buttonChangeAHSkill.Enabled = false;
            ClearTraitsList();
        }

        public void UpdateAHSkillAndEnableEdit()
        {
            numericUpDownAHSkill.Value = ParentModule.AHSkill;
            buttonChangeAHSkill.Enabled = true;
        }

        public void UpdateContents()
        {
            comboBoxHerd.Items.Clear();
            comboBoxHerd.Items.AddRange(ParentModule.AllHerds.GetHerdIDs());

            comboBoxHerd.Text = ParentModule.AllHerds.CurrentHerd.HerdID;

            textBoxNewHorseDestination.Text = ParentModule.AllHerds.NewHorseTargetHerd.HerdID;

            if (ParentModule.AllHerds.NewHorseTargetHerd != null)
            {
                textBoxNewHorseDestination.Text = ParentModule.AllHerds.NewHorseTargetHerd.HerdID;
            }
            else
            {
                Logger.WriteLine("! New horse target herd was null");
            }
        }

        #region HERD LIST HANDLING

        //current horse is stored in local variable, so it can be reselected after sorting and other list updates
        //setting current horse to null at any point, will cause deselect in the list on next timer tick
        Horse currentlySelectedHorse;

        //refreshes trait list on tick
        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                UpdateHerdHorseList();
            }
        }

        //herd list is set to virtual, so scrolling position can be preserved
        //this means, the actual timer-updated collection is this list:
        List<ListViewItem> VirtualHerdItems = new List<ListViewItem>();

        void UpdateHerdVirtualList()
        {
            VirtualHerdItems.Clear();
            foreach (var keyval in ParentModule.AllHerds.CurrentHerd.Horses)
            {
                //build breeded
                TimeSpan? breededSince = DateTime.Now - keyval.Value.NotInMood;
                if (breededSince > TimeSpan.FromMinutes(45)) breededSince = null;
                string breededOutput = "";
                if (breededSince != null)
                {
                    breededOutput = ((int)breededSince.Value.TotalMinutes).ToString() + " min. ago";
                }
                //build groomed
                TimeSpan? groomedSince = DateTime.Now - keyval.Value.GroomedOn;
                if (groomedSince > TimeSpan.FromMinutes(60)) groomedSince = null;
                string groomedOutput = "";
                if (groomedSince != null)
                {
                    if (groomedSince.Value.TotalHours < 2.0D)
                        groomedOutput = ((int)(groomedSince.Value.TotalMinutes)).ToString() + " min. ago";
                    else
                        groomedOutput = ((int)(groomedSince.Value.TotalHours)).ToString() + " hours ago";
                }
                //build pregnant
                TimeSpan? pregnantFor = keyval.Value.PregnantTo - DateTime.Now;
                if (pregnantFor > TimeSpan.FromDays(15D) || pregnantFor.Value.Ticks < 0) pregnantFor = null;
                string pregnantOutput = "";
                if (pregnantFor != null)
                {
                    pregnantOutput = "in " + ((int)pregnantFor.Value.TotalHours).ToString() + " hours";
                }
                //build comments
                string comments = "";
                if (keyval.Value.maybeDied) comments += "(dead?) ";
                if (keyval.Value.maybePregnant) comments += "(pregnant?) ";
                if (keyval.Value.TakenCareOfBy != "") comments += "(cared by "+keyval.Value.TakenCareOfBy+") ";
                if (keyval.Value.Comments != string.Empty) comments += keyval.Value.Comments;
                //add up
                VirtualHerdItems.Add(new ListViewItem(new string[] {
                    keyval.Key,
                    keyval.Value.IsMale ? "male" : "female",
                    keyval.Value.Father,
                    keyval.Value.Mother,
                    TraitValues.CalculateHorseValue(keyval.Value).ToString(),
                    TraitValues.CalculatePotentialHorseValueForDisplay(keyval.Value),
                    breededOutput,
                    groomedOutput,
                    pregnantOutput,
                    comments}));
            }

            MarkBreedingCandidates();
        }

        //this fetches items from virtual list to the listview, 
        //all items are fetched as the only reason to use this is scroll position preservation
        private void listViewNFHerd_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            e.Item = VirtualHerdItems[e.ItemIndex];
        }

        public void UpdateHerdHorseList()
        {
            UpdateHerdVirtualList();
            listViewNFHerd.VirtualListSize = VirtualHerdItems.Count;
            listViewNFHerd.Refresh();
            textBoxNewHorseDestination.Text = ParentModule.AllHerds.NewHorseTargetHerd.HerdID;

            if (currentlySelectedHorse != null)
            {
                if (!SelectPreviousHorse()) currentlySelectedHorse = null;
            }
        }

        private void listViewNFHerd_SelectedIndexChanged(object sender, EventArgs e)
        {
            //trait list needs to show traits for currently selected horse, or nothing if none is selected
            UpdateTraitList();
        }

        //this stores last horse for which grangerAI ran calculations
        //it is kept to avoid running calculations for same horse on every refresh
        Horse lastCalculatedHorse = null;

        //this is the actual grangerAI result, storing row color for every horse
        //it is applied on every list refresh
        Dictionary<Horse, Color> CachedHorseToColorMap;

        //this iterates over virtual herd list, applying colors to rows
        //runs calculation in grangerAI only if horse changed since last one
        private void MarkBreedingCandidates()
        {
            if (!checkBoxDisableAdvisor.Checked)
            {
                Dictionary<Horse, Color> HorseToColorMap;
                if (currentlySelectedHorse != null)
                {
                    if (lastCalculatedHorse != currentlySelectedHorse || CachedHorseToColorMap == null)
                    {
                        lastCalculatedHorse = currentlySelectedHorse;
                        GrangerAI aiworker = new GrangerAI(currentlySelectedHorse, ParentModule.AllHerds.CurrentHerd);

                        Horse curhorse = currentlySelectedHorse;
                        List<Horse> horsesToConsider = new List<Horse>();
                        //get list of potential candidates
                        if (checkBoxNegativeExclude.Checked) aiworker.ExcludeNegatives();
                        if (!checkBoxDontExcludeInbreed.Checked) aiworker.ExcludeInbreed();
                        if (checkBoxAdviseAvailable.Checked) aiworker.IgnoreUnavailable();
                        //perform calculations
                        aiworker.PerformCalculations(checkBoxPairingMissingTraits.Checked, checkBoxIncludePotential.Checked);
                        //set appropriate list objects with appropriate colors
                        CachedHorseToColorMap = HorseToColorMap = aiworker.GetColoredDict();
                    }
                    else
                    {
                        HorseToColorMap = CachedHorseToColorMap;
                    }

                    //foreach (ListViewItem item in listViewNFHerd.Items)
                    foreach (ListViewItem item in VirtualHerdItems)
                    {
                        Horse horse;
                        if (ParentModule.AllHerds.CurrentHerd.Horses.TryGetValue(item.SubItems[0].Text, out horse))
                        {
                            Color color;
                            if (HorseToColorMap.TryGetValue(horse, out color))
                            {
                                item.BackColor = color;
                            }
                        }
                    }
                }
            }
        }

        bool SelectPreviousHorse()
        {
            for (int i = 0; i < listViewNFHerd.Items.Count; i++)
            {
                string currentRecord = listViewNFHerd.Items[i].SubItems[0].Text;
                if (currentRecord == currentlySelectedHorse.Name)
                {
                    listViewNFHerd.Items[i].Selected = true;
                    return true;
                }
            }
            return false;
        }
        
        //sorting
        private void listViewNFHerd_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            switch (e.Column)
            {
                case 0:
                    ParentModule.AllHerds.CurrentHerd.SortBy(HorseSortingOption.Name);
                    break;
                case 1:
                    ParentModule.AllHerds.CurrentHerd.SortBy(HorseSortingOption.Gender);
                    break;
                case 2:
                    ParentModule.AllHerds.CurrentHerd.SortBy(HorseSortingOption.Father);
                    break;
                case 3:
                    ParentModule.AllHerds.CurrentHerd.SortBy(HorseSortingOption.Mother);
                    break;
                case 4:
                    ParentModule.AllHerds.CurrentHerd.SortBy(HorseSortingOption.Value);
                    break;
                case 5:
                    ParentModule.AllHerds.CurrentHerd.SortBy(HorseSortingOption.Potential);
                    break;
                case 6:
                    ParentModule.AllHerds.CurrentHerd.SortBy(HorseSortingOption.Breeded);
                    break;
                case 7:
                    ParentModule.AllHerds.CurrentHerd.SortBy(HorseSortingOption.Groomed);
                    break;
                case 8:
                    ParentModule.AllHerds.CurrentHerd.SortBy(HorseSortingOption.Pregnant);
                    break;
                case 9:
                    ParentModule.AllHerds.CurrentHerd.SortBy(HorseSortingOption.Comment);
                    break;
                default:
                    break;
            }
            UpdateContents();
        }

        private void listViewNFHerd_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listViewNFHerd.SelectedIndices.Count > 0)
            {
                Horse horse;
                if (ParentModule.AllHerds.TryFindHorse(SelectedHorseNameInHerdListView, out horse))
                {
                    FormManageHorse ui = new FormManageHorse(horse, ParentModule, HorseOperationType.View);
                    ui.ShowDialog();
                    //no need to apply, object is modified in the form
                }
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            UpdateHerdHorseList();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            //TODO export functions
            //get list of all horses in this herd, let user choose which to export
            //optional: format choice
            //default: html document? or txt?
        }

        private void comboBoxHerd_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxHerd.SelectedIndex != -1)
            {
                ParentModule.AllHerds.SetSelectedHerds(newCurrentHerdID: comboBoxHerd.Text);
                UpdateHerdHorseList();
            }
        }

        private void addHorseManuallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //horse edit dialog: new horse
            Horse horse = new Horse();
            FormManageHorse ui = new FormManageHorse(horse, ParentModule, HorseOperationType.New);
            if (ui.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //route new horse to allherds
                ParentModule.AllHerds.CurrentHerd.AddHorse(horse);
            }

        }

        private void editTraitsManuallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //horse edit dialog: this horse
            if (listViewNFHerd.SelectedIndices.Count > 0)
            {
                Horse horse;
                if (ParentModule.AllHerds.TryFindHorse(SelectedHorseNameInHerdListView, out horse))
                {
                    FormManageHorse ui = new FormManageHorse(horse, ParentModule, HorseOperationType.Edit);
                    ui.ShowDialog();
                    //no need to apply, object is modified in the form
                }
            }
        }

        string SelectedHorseNameInHerdListView
        {
            get
            {
                return listViewNFHerd.Items[listViewNFHerd.SelectedIndices[0]].SubItems[0].Text;
            }
        }

        private void moveThisHorseToAnotherHerdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //dialog asking for target herd
            if (currentlySelectedHorse != null)
            {
                FormChooseHerd ui = new FormChooseHerd(ParentModule.AllHerds.GetHerdIDs());
                if (ui.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //route request to allherds
                    if (!ParentModule.AllHerds.MoveHorse(currentlySelectedHorse, ParentModule.AllHerds.CurrentHerd.HerdID, ui.Result))
                    {
                        MessageBox.Show("Error while trying to move horse");
                    }
                }
            }
        }

        private void deleteThisHorseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (currentlySelectedHorse != null)
            {
                string horseToDel = currentlySelectedHorse.Name;
                //dialog asking for confirmation
                if (MessageBox.Show("Are you sure to delete " + currentlySelectedHorse.Name, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                {
                    //route request to allherds
                    if (ParentModule.AllHerds.CurrentHerd.Horses.ContainsKey(horseToDel))
                    {
                        if (!ParentModule.AllHerds.CurrentHerd.Horses.Remove(horseToDel))
                        {
                            MessageBox.Show("Could not delete this horse from current herd!");
                            Logger.WriteLine("Granger: Could not delete " + horseToDel + " from current herd: " + ParentModule.AllHerds.CurrentHerd.HerdID);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Could not delete this horse from current herd, because it's not there!");
                        Logger.WriteLine("Granger: Could not delete " + horseToDel + " from current herd: " + ParentModule.AllHerds.CurrentHerd.HerdID + " because it was not in it!");
                    }
                }
            }
        }

        private void buttonNewHerd_Click(object sender, EventArgs e)
        {
            //dialog asking for name
            FormHerdName ui = new FormHerdName(HerdOperationType.New, ParentModule.AllHerds.GetHerdIDs());
            if (ui.ShowDialog() == DialogResult.OK)
            {
                //route to allherds (fail if name exists)
                //notify user
                if (ParentModule.AllHerds.CreateNewHerd(ui.textBoxNew.Text))
                {
                    ParentModule.NotifyUser(String.Format("Created new herd: {0}", ui.textBoxNew.Text));
                    this.UpdateContents();
                }
                else
                {
                    ParentModule.NotifyUser(String.Format("FAILED to create new herd: {0}", ui.textBoxNew.Text));
                }
            }
        }

        private void buttonRenameHerd_Click(object sender, EventArgs e)
        {
            //dialog asking for new name
            FormHerdName ui = new FormHerdName(HerdOperationType.Rename, ParentModule.AllHerds.GetHerdIDs(), ParentModule.AllHerds.CurrentHerd.HerdID);
            if (ui.ShowDialog() == DialogResult.OK)
            {
                //route to allherds (fail if name exists)                 //notify user
                if (ParentModule.AllHerds.RenameHerd(ui.textBoxOld.Text, ui.textBoxNew.Text))
                {
                    ParentModule.NotifyUser(String.Format("Renamed herd: {0} to: {1}", ui.textBoxOld.Text, ui.textBoxNew.Text));
                    this.UpdateContents();
                }
                else
                {
                    ParentModule.NotifyUser(String.Format("FAILED to rename herd: {0} to: {1}", ui.textBoxOld.Text, ui.textBoxNew.Text));
                }
            }
        }

        private void buttonMergeHerds_Click(object sender, EventArgs e)
        {
            //dialog asking for target herd
            FormHerdName ui = new FormHerdName(HerdOperationType.Merge, ParentModule.AllHerds.GetHerdIDs(), ParentModule.AllHerds.CurrentHerd.HerdID);
            if (ui.ShowDialog() == DialogResult.OK)
            {
                //route to allherds (fail if name doesn't exist)
                //notify user
                if (ParentModule.AllHerds.MergeHerds(ui.textBoxOld.Text, ui.comboBoxTargetHerd.SelectedItem.ToString()))
                {
                    ParentModule.NotifyUser(String.Format("Merged herd: {0} into: {1}", ui.textBoxOld.Text, ui.comboBoxTargetHerd.SelectedItem.ToString()));
                    UpdateHerdHorseList();
                    this.UpdateContents();
                }
                else
                {
                    ParentModule.NotifyUser(String.Format("FAILED to merge herd: {0} into: {1}", ui.textBoxOld.Text, ui.comboBoxTargetHerd.SelectedItem.ToString()));
                }
            }
        }

        private void buttonDeleteHerd_Click(object sender, EventArgs e)
        {
            //dialog asking for confirmation
            //route to allherds
            if (MessageBox.Show(String.Format("Are you sure to delete this herd: ({0})", ParentModule.AllHerds.CurrentHerd.HerdID),
                "Confirmation needed", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
            {
                ParentModule.AllHerds.DeleteHerd(ParentModule.AllHerds.CurrentHerd);
                this.UpdateContents();
            }
        }

        #endregion

        #region TRAIT LIST HANDLING

        void ClearTraitsList()
        {
            foreach (HorseTrait name in HorseTraitEX.EnumToNameMap.Keys)
            {
                listViewNFTraits.Items.Add(name.ToString());
            }
        }

        private void buttonSetWeights_Click(object sender, EventArgs e)
        {
            FormAdjustWeights ui = new FormAdjustWeights(ParentModule);
            ui.ShowDialog();
            UpdateContents();
        }

        void UpdateTraitList()
        {
            if (listViewNFHerd.SelectedIndices.Count > 0)
            {
                //Horse horse = ParentModule.AllHerds.CurrentHerd.Horses[listViewNFHerd.SelectedItems[0].Text];
                Horse horse = ParentModule.AllHerds.CurrentHerd.Horses[listViewNFHerd.Items[listViewNFHerd.SelectedIndices[0]].SubItems[0].Text];
                currentlySelectedHorse = horse;
                MarkBreedingCandidates();
                listViewNFTraits.Items.Clear();
                labelInspectedHorse.Text = horse.Name;
                foreach (HorseTrait trait in HorseTraitEX.EnumToNameMap.Keys)
                {
                    float thisvalue = TraitValues.TraitToValueMap[trait];
                    string traitview = "";
                    Color? adjcolor = null;
                    if (horse.Traits.Contains(trait))
                    {
                        traitview = "YES";

                        if (thisvalue > 0F) adjcolor = Color.LightGreen;
                        else if (thisvalue < 0F) adjcolor = Color.OrangeRed;
                    }
                    else
                    {
                        if (HorseTraitEX.EnumToAHSkillMap[trait] > horse.TraitsInspectedAtSkill)
                        {
                            traitview = "unknown";
                            if (thisvalue < 0F) adjcolor = Color.Yellow;
                            else if (thisvalue > 0F) adjcolor = Color.LightBlue;
                        }
                    }

                    ListViewItem item = new ListViewItem(new string[]
                        {
                            trait.ToString(),
                            traitview,
                            thisvalue.ToString()
                        });
                    if (adjcolor != null) item.BackColor = (Color)adjcolor;
                    listViewNFTraits.Items.Add(item);
                }
            }
            else
            {
                currentlySelectedHorse = null;
                listViewNFTraits.Items.Clear();
                labelInspectedHorse.Text = "";
            }
        }

        #endregion

        #region GRANGER OPTIONS

        private void checkBoxEnable_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.ModuleEnabled = checkBoxEnable.Checked;
        }

        private void buttonChangeNewHorseDestination_Click(object sender, EventArgs e)
        {
            FormHerdName ui = new FormHerdName(HerdOperationType.Select, ParentModule.AllHerds.GetHerdIDs());
            if (ui.ShowDialog() == DialogResult.OK)
            {
                ParentModule.AllHerds.SetSelectedHerds(newHorseTargetHerdID: ui.comboBoxTargetHerd.Text);
                UpdateContents();
            }
        }

        public void RestoreFromMin()
        {
            if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;
        }

        private void FormHorseManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        public void HandleSearchCallback(LogSearchData logsearchdata)
        {
            ParentModule.HandleSearchCallback(logsearchdata);
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://fnc.ucoz.net/Granger.htm");
            //FormGrangerHelp ui = new FormGrangerHelp();
            //ui.Show();
        }

        private void checkBoxLiveAdvisor_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.LiveAdvisor = checkBoxLiveAdvisor.Checked;
        }

        private void checkBoxAdviseAvailable_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.AdviseOnlyAvailable = checkBoxAdviseAvailable.Checked;
        }

        private void checkBoxIncludePotential_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.IncludePotentialValue = checkBoxIncludePotential.Checked;
        }

        private void checkBoxPairingMissingTraits_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.PreferMissingTraits = checkBoxPairingMissingTraits.Checked;
        }

        private void checkBoxNegativeExclude_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.ExcludeNegatives = checkBoxNegativeExclude.Checked;
        }

        private void checkBoxDontExcludeInbreed_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.DontExcludeInbreeding = checkBoxDontExcludeInbreed.Checked;
        }

        private void checkBoxDisableAdvisor_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.DisableAdvisor = checkBoxDisableAdvisor.Checked;
        }

        #endregion

        private void buttonDebug_Click(object sender, EventArgs e)
        {
            GrangerDebug.Show();
        }

        private void FormHorseManager_Load(object sender, EventArgs e)
        {

        }

        private void buttonChangeAHSkill_Click(object sender, EventArgs e)
        {
            numericUpDownAHSkill.ReadOnly = false;
        }

        private void numericUpDownAHSkill_Validated(object sender, EventArgs e)
        {
            ParentModule.AHSkill = (int)numericUpDownAHSkill.Value;
            numericUpDownAHSkill.ReadOnly = true;
        }

        private void numericUpDownAHSkill_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                numericUpDownAHSkill_Validated(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        private void buttonDeleteAll_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will delete ALL granger settings, herds, horses and trait values!! This will also require Wurm Assistant to be restarted. Are you sure?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
            {
                if (MessageBox.Show("Are you really, REALLY shure!?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                {
                    try 
                    {
                        ParentModule.WipeOutEverything();
                        MessageBox.Show("Fine! It's done. Program will close now.");
                        Application.Exit();
                    }
                    catch (Exception _e)
                    {
                        MessageBox.Show("Opps.. there was some error, please check Wurm Assistant log for details and report this bug");
                        Logger.WriteLine("! Granger: Error while wiping all granger DB entries");
                        Logger.LogException(_e);
                    }
                }
            }
        }

        private void checkBoxEpicCurve_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
