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
    public partial class FormChooseSeasons : Form
    {
        public FormChooseSeasons(string[] items, Dictionary<string, ModuleCalendar.WurmSeasonsEnum> tracked)
        {
            InitializeComponent();
            int indexcount = 0;
            foreach (string item in items)
            {
                checkedListBox1.Items.Add(item);
                checkedListBox1.SetItemChecked(indexcount, tracked.ContainsKey(item));
                indexcount++;
            }
        }
    }
}
