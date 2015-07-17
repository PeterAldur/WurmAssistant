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
    public partial class FormManageHorse : Form
    {
        HorseOperationType opType;
        Horse horse;
        ModuleGranger parentModule;
        string[] allHorsesInDatabase;

        bool newNameInvalid = false;
        bool newNameEmpty = false;
        bool newNameAndParentTheSame = false;

        //in case new horse, a proper object needs to be created, then passed here to populate, then applied to herd if OK
        public FormManageHorse(Horse horse, ModuleGranger parentModule, HorseOperationType optype)
        {
            this.opType = optype;
            this.horse = horse;
            this.parentModule = parentModule;
            InitializeComponent();

            allHorsesInDatabase = parentModule.AllHerds.GetListOfAllHorseNames();
            disableAllFields();

            comboBoxFather.Items.AddRange(allHorsesInDatabase);
            comboBoxMother.Items.AddRange(allHorsesInDatabase);

            if (optype == HorseOperationType.View)
                prepareFieldsForView();
            else if (optype == HorseOperationType.Edit)
                prepareFieldsForEdit();
            else if (optype == HorseOperationType.New)
                prepareFieldsForNew();
        }

        void disableAllFields()
        {
            panel1.Enabled = false;
            dateTimePickerBreeded.Value = DateTimePicker.MinimumDateTime;
            dateTimePickerGroomed.Value = DateTimePicker.MinimumDateTime;
            dateTimePickerPregnant.Value = DateTimePicker.MinimumDateTime;
            labelValue.Text = "";
            //textBoxName.Enabled = false;
            //comboBoxMother.Enabled = false;
            //comboBoxFather.Enabled = false;
            //groupBoxGender.Enabled = false;
            //dateTimePickerBreeded.Enabled = false;
            //dateTimePickerGroomed.Enabled = false;
            //dateTimePickerPregnant.Enabled = false;
            //checkedListBoxTraits.Enabled = false;
            //checkBoxDeceased.Enabled = false;
            //checkBoxPregnant.Enabled = false;
            buttonEdit.Visible = true;
        }

        void enableAllFields()
        {
            panel1.Enabled = true;
            //textBoxName.Enabled = true;
            //comboBoxMother.Enabled = true;
            //comboBoxFather.Enabled = true;
            //groupBoxGender.Enabled = true;
            //dateTimePickerBreeded.Enabled = true;
            //dateTimePickerGroomed.Enabled = true;
            //dateTimePickerPregnant.Enabled = true;
            //checkedListBoxTraits.Enabled = true;
            //checkBoxDeceased.Enabled = true;
            //checkBoxPregnant.Enabled = true;
            buttonEdit.Visible = false;
        }

        void prepareFieldsForView()
        {
            buildTraits();

            textBoxName.Text = horse.Name;
            comboBoxMother.Text = horse.Mother;
            comboBoxFather.Text = horse.Father;
            if (horse.IsMale) radioButtonMale.Checked = true; else radioButtonFemale.Checked = true;
            try { dateTimePickerBreeded.Value = horse.NotInMood; }
            catch { dateTimePickerBreeded.Value = DateTimePicker.MinimumDateTime; }
            try { dateTimePickerGroomed.Value = horse.GroomedOn; }
            catch { dateTimePickerGroomed.Value = DateTimePicker.MinimumDateTime; }
            try { dateTimePickerPregnant.Value = horse.PregnantTo; }
            catch { dateTimePickerPregnant.Value = DateTimePicker.MinimumDateTime; }
            labelValue.Text = "This horse value is: +" + TraitValues.CalculateHorseValue(horse) + "\n Potential value: " + TraitValues.CalculatePotentialHorseValueForDisplay(horse);
            checkBoxDeceased.Checked = horse.maybeDied;
            checkBoxPregnant.Checked = horse.maybePregnant;
            int skill;
            if (horse.TraitsInspectedAtSkill > 100) skill = 100;
            else if (horse.TraitsInspectedAtSkill < 0) skill = 0;
            else skill = horse.TraitsInspectedAtSkill;
            numericUpDownAHskill.Value = skill;
            textBoxCaredForBy.Text = horse.TakenCareOfBy;
            textBoxComment.Text = horse.Comments;
        }

        void prepareFieldsForEdit()
        {
            prepareFieldsForView();
            enableAllFields();
        }

        void buildClearTraitList()
        {
            checkedListBoxTraits.Items.Clear();
            checkedListBoxTraits.Items.AddRange(HorseTraitEX.NameToEnumMap.Keys.ToArray<string>());
        }

        void buildTraits()
        {
            checkedListBoxTraits.Items.Clear();
            foreach (var keyval in HorseTraitEX.NameToEnumMap)
            {
                if (HorseTraitEX.EnumToAHSkillMap[keyval.Value] > horse.TraitsInspectedAtSkill)
                {
                    checkedListBoxTraits.Items.Add("(unknown) " + keyval.Key);
                }
                else
                {
                    checkedListBoxTraits.Items.Add(
                        keyval.Key,
                        horse.Traits.Contains(keyval.Value) ? true : false);
                }
            }
        }

        void prepareFieldsForNew()
        {
            buildClearTraitList();
            enableAllFields();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //validate fields again becase winforms is total crap
            ValidateHorseName();
            //decide what to do
            if (opType == HorseOperationType.Edit || opType == HorseOperationType.New)
            {
                if (newNameInvalid)
                {
                    MessageBox.Show("This horse name must be unique in the database");
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                }
                else if (newNameEmpty)
                {
                    MessageBox.Show("This horse name cannot be empty");
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                }
                else if (newNameAndParentTheSame)
                {
                    MessageBox.Show("This horse cannot have itself as parent (");
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                }
                else
                {
                    //apply changes to horse object

                    if (opType == HorseOperationType.New)
                    {
                        horse.Name = textBoxName.Text;
                    }
                    else if (textBoxName.Text != horse.Name)
                    {
                        if (!parentModule.AllHerds.RenameHorse(horse, textBoxName.Text))
                            MessageBox.Show("There was an error while attempting to rename this horse");
                    }

                    horse.Mother = comboBoxMother.Text.Trim();
                    horse.Father = comboBoxFather.Text.Trim();
                    horse.IsMale = radioButtonMale.Checked ? true : false;
                    horse.NotInMood = dateTimePickerBreeded.Value;
                    horse.PregnantTo = dateTimePickerPregnant.Value;
                    horse.GroomedOn = dateTimePickerGroomed.Value;
                    horse.maybeDied = checkBoxDeceased.Checked;
                    horse.maybePregnant = checkBoxPregnant.Checked;
                    //update inspected at skill
                    horse.Traits.Clear();
                    int highestAHskill = 0;
                    foreach (string strtrait in checkedListBoxTraits.CheckedItems)
                    {
                        //remove "(unknown) " if exists
                        string fixedSTRtrait = strtrait.Replace("(unknown) ", "");
                        HorseTrait trait = HorseTraitEX.NameToEnumMap[fixedSTRtrait];
                        horse.Traits.Add(trait);
                        int thisTraitAHSkill = HorseTraitEX.EnumToAHSkillMap[trait];
                        if (highestAHskill < thisTraitAHSkill) highestAHskill = thisTraitAHSkill;
                    }
                    if (highestAHskill > numericUpDownAHskill.Value) horse.TraitsInspectedAtSkill = highestAHskill;
                    else horse.TraitsInspectedAtSkill = (int)numericUpDownAHskill.Value;
                    textBoxCaredForBy.Text = horse.TakenCareOfBy.Trim();
                    horse.Comments = textBoxComment.Text;
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            // export to a html formatted document? (or anything really) txt?
        }

        private void buttonCopyToClipboard_Click(object sender, EventArgs e)
        {
            // prepare this horse data in one-liner no more than ~~200-250? chars
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            opType = HorseOperationType.Edit;
            prepareFieldsForEdit();
        }

        private void textBoxName_Validating(object sender, CancelEventArgs e)
        {
            ValidateHorseName();
        }

        private void ValidateHorseName()
        {
            newNameInvalid = labelWarnDuplicate.Visible = false;
            newNameEmpty = false;
            newNameAndParentTheSame = false;

            textBoxName.Text = RefactorHorseName(textBoxName.Text);

            if (parentModule.AllHerds.HorseExists(textBoxName.Text))
            {
                if (opType == HorseOperationType.Edit && textBoxName.Text != horse.Name)
                    newNameInvalid = labelWarnDuplicate.Visible = true;
            }

            if (textBoxName.Text == "")
            {
                newNameEmpty = true;
            }

            if (textBoxName.Text == comboBoxFather.Text || textBoxName.Text == comboBoxMother.Text)
            {
                newNameAndParentTheSame = true;
            }
        }

        private string RefactorHorseName(string input)
        {
            input = GrangerHelpers.RemoveAllPrefixes(input);
            input = input.Trim();
            string concatworker = "";
            if (input.Length > 0) concatworker = input[0].ToString().ToUpper();
            if (input.Length > 1) concatworker += input.Substring(1);
            input = concatworker;
            return input;
        }

        private void comboBoxMother_Validating(object sender, CancelEventArgs e)
        {
            comboBoxMother.Text = RefactorHorseName(comboBoxMother.Text);
        }

        private void comboBoxFather_Validating(object sender, CancelEventArgs e)
        {
            comboBoxFather.Text = RefactorHorseName(comboBoxFather.Text);
        }
    }
}
