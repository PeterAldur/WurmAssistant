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
    public partial class FormChooseSound : Form
    {
        public string ChosenSound = null;

        public FormChooseSound()
        {
            InitializeComponent();
            RefreshList();
        }

        void RefreshList()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(SoundBank.getSoundsArray());
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChosenSound = (string)listBox1.SelectedItem;
        }

        private void buttonAddSound_Click(object sender, EventArgs e)
        {
            WurmAssistant.ZeroRef.OpenSoundBank();
            RefreshList();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            PlaySound();
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            PlaySound();
        }

        void PlaySound()
        {
            SoundBank.StopSounds();
            if (listBox1.SelectedIndex > -1)
            {
                SoundBank.PlaySound((string)listBox1.SelectedItem);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ChosenSound == null)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                MessageBox.Show("Please select a sound first");
            }
        }
    }
}
