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
    public partial class FormHerdName : Form
    {
        private HerdOperationType herdOperationType;
        bool validated = false;

        public FormHerdName(HerdOperationType optype, string[] allherds)
        {
            InitializeComponent();

            this.herdOperationType = optype;

            comboBoxTargetHerd.Items.AddRange(allherds);

            if (optype == HerdOperationType.New)
            {
                this.Text = "Create new herd...";

                labelOld.Visible = false;
                textBoxOld.Visible = false;

                labelTarget.Visible = false;
                comboBoxTargetHerd.Visible = false;
            }
            else if (optype == HerdOperationType.Rename)
            {
                this.Text = "Rename herd...";

                labelTarget.Visible = false;
                comboBoxTargetHerd.Visible = false;
            }
            else if (optype == HerdOperationType.Merge)
            {
                this.Text = "Merge herds...";

                labelNew.Visible = false;
                textBoxNew.Visible = false;

                labelOld.Text = "From herd:";
            }
            else if (optype == HerdOperationType.Select)
            {
                this.Text = "Select herd...";

                labelNew.Visible = false;
                textBoxNew.Visible = false;

                labelOld.Visible = false;
                textBoxOld.Visible = false;
            }
        }

        public FormHerdName(HerdOperationType herdOperationType, string[] allherds, string oldherd)
            : this(herdOperationType, allherds)
        {
            // TODO: Complete member initialization
            this.herdOperationType = herdOperationType;
            textBoxOld.Text = oldherd;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

            if (!ValidateNewName())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                MessageBox.Show("New herd name must be unique");
            }

            if (herdOperationType == HerdOperationType.Merge)
            {
                if (comboBoxTargetHerd.SelectedIndex == -1)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    MessageBox.Show("Please select target herd for merging");
                }
                else if (comboBoxTargetHerd.Text == textBoxOld.Text)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    MessageBox.Show("Cannot merge this herd with itself");
                }
            }

            if (herdOperationType == HerdOperationType.Select)
            {
                if (comboBoxTargetHerd.SelectedIndex == -1)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    MessageBox.Show("Please select a herd");
                }
            }
        }

        bool ValidateNewName()
        {
            textBoxNew.Text = textBoxNew.Text.Trim();
            textBoxNew.Text = textBoxNew.Text.Replace(" ", "_");

            if (comboBoxTargetHerd.Items.Contains(textBoxNew.Text))
            {
                labelWarn.Visible = true;
                return false;
            }
            else
            {
                labelWarn.Visible = false;
                return true;
            }
        }

        private void textBoxNew_Validating(object sender, CancelEventArgs e)
        {
            ValidateNewName();
        }
    }
}
