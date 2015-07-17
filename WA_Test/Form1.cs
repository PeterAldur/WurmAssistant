using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace WA_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void skillParsingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] input = { 
                             "[12:43:12] Animal husbandry increased by 0,00780 to 35,8539",
                             "[2012-08-18] [17:28:19] First aid increased by 0,0124 to 23,392",
                             "[2012-08-18] [17:44:01] Digging increased by 0,0108 to 73,362",
                             "[2012-08-19] [23:53:27] Mind increased  to 27",
                             "[2012-08-21] [03:23:59] Nature increased  to 30",
                             "[2012-09-13] [19:28:37] Woodcutting increased by 0,104 to 42,337",
                             "[2013-01-22] [15:25:36] Smithing increased by 0,0102 to 30,137",
                             "[2013-03-03] [22:43:50] Nature increased by 0,00163 to 36,6656",

                             "[12:43:12] Animal husbandry increased by 0.00780 to 35.8539",
                             "[2012-08-18] [17:28:19] First aid increased by 0.0124 to 23.392",
                             "[2012-08-18] [17:44:01] Digging increased by 0.0108 to 73.362",
                             "[2012-08-19] [23:53:27] Mind increased  to 27",
                             "Nature increased  to 30",
                             "[2012-09-13] [19:28:37] Woodcutting increased by 0.104 to 42.337",
                             "[2013-01-22] [15:25:36] Smithing increased by 0.0102 to 30.137",
                             "[2013-03-03] [22:43:50] Nature increased by 0.00163 to 36.6656" };

            string[] cultureInput = {
                                        "[12:43:12] Animal husbandry increased by 0,00780 to 35,8539",
                                        "[12:43:12] Animal husbandry increased by 0.00780 to 35.8539" };

            List<string> output = new List<string>();
            output.Add("ExtractSkillGAINFromLine");
            foreach (string line in input)
            {
                output.Add(string.Format("{0} => {1}", line, WurmAssistant.GeneralHelper.ExtractSkillGAINFromLine(line)));
            }
            output.Add("");
            output.Add("ExtractSkillLEVELFromLine");
            foreach (string line in input)
            {
                output.Add(string.Format("{0} => {1}", line, WurmAssistant.GeneralHelper.ExtractSkillLEVELFromLine(line)));
            }
            textBoxOutput.Lines = output.ToArray();

            CultureInfo curCulture = Application.CurrentCulture;
            CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            output.Add("");
            output.Add("Culture verification");
            foreach (CultureInfo culture in allCultures)
            {
                Application.CurrentCulture = culture;
                output.Add(culture.Name);
                foreach (string line in cultureInput)
                {
                    float result = WurmAssistant.GeneralHelper.ExtractSkillLEVELFromLine(line);
                    output.Add(string.Format("{0} => {1}", line, result));
                    output.Add("2x: " + result * 2);
                }
            }
            textBoxOutput.Lines = output.ToArray();
            Application.CurrentCulture = curCulture;
        }

        private void cultureSwapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<string> output = new List<string>();

            CultureInfo curCulture = Application.CurrentCulture;
            try
            {
                CultureInfo[] allCultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
                output.Add("");
                output.Add("Culture number decimal point test verification");
                foreach (CultureInfo culture in allCultures)
                {
                    Application.CurrentCulture = culture;
                    float result;
                    float.TryParse("33,352", out result);
                    output.Add(result.ToString());
                }
                textBoxOutput.Lines = output.ToArray();
            }
            catch (Exception _e)
            {
                MessageBox.Show(_e.Message);
            }
            finally
            {
                Application.CurrentCulture = curCulture;
            }
        }

        private void betterNumberParserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] cultureInput = {
                                        "[12:43:12] Animal husbandry increased by 0,00780 to 35,8539",
                                        "[12:43:12] Animal husbandry increased by 0.00780 to 35.8539" };

            List<string> output = new List<string>();

            output.Add("ExtractSkillLEVELFromLine");
            foreach (string line in cultureInput)
            {
                float result = BetterNumberParser.ExtractSkillLEVELFromLine(line);
                output.Add(string.Format("{0} => {1}", line, result));
                output.Add("x2: "+ result * 2);
            }
            output.Add("ExtractSkillGAINFromLine");
            foreach (string line in cultureInput)
            {
                float result = BetterNumberParser.ExtractSkillGAINFromLine(line);
                output.Add(string.Format("{0} => {1}", line, result));
                output.Add("x2: " + result * 2);
            }

            textBoxOutput.Lines = output.ToArray();
        }
    }
}
