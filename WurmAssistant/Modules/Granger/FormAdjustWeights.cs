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
    public partial class FormAdjustWeights : Form
    {
        ModuleGranger ParentModule;

        public FormAdjustWeights(ModuleGranger parentModule)
        {
            this.ParentModule = parentModule;
            InitializeComponent();
        }

        private void FormAdjustWeights_Load(object sender, EventArgs e)
        {
            RefreshWeights();
        }

        void RefreshWeights()
        {
            dataGridView1.Rows.Clear();
            foreach (var keyval in TraitValues.TraitToValueMap)
            {
                string traitname = HorseTraitEX.EnumToNameMap[keyval.Key];
                dataGridView1.Rows.Add(traitname, keyval.Value);
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            OpenWeightEdit();
        }

        void OpenWeightEdit()
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int selectedRow = dataGridView1.SelectedCells[0].RowIndex;
                FormAdjustWeightsEdit ui = new FormAdjustWeightsEdit();
                ui.textBox1.Text = (string)dataGridView1.Rows[selectedRow].Cells[0].Value;
                ui.numericUpDown1.Value = Decimal.Parse(dataGridView1.Rows[selectedRow].Cells[1].Value.ToString());
                if (ui.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ParentModule.Values.Update(ui.textBox1.Text, ui.numericUpDown1.Value);
                    RefreshWeights();
                    dataGridView1.Rows[selectedRow].Selected = true;
                    dataGridView1.CurrentCell = dataGridView1.Rows[selectedRow].Cells[0];
                }
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                OpenWeightEdit();
            }
        }
    }
}
