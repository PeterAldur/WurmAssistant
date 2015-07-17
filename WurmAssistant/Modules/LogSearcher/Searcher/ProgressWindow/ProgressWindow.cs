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
    public partial class ProgressWindow : Form
    {
        int NumOfJobs = 0;
        int CurrentJob = 0;
        string defaultStatusText = "";

        public ProgressWindow(int numOfJobs, string jobDescription, string progressDescription)
        {
            InitializeComponent();
            this.Top = 1;
            this.Left = Screen.PrimaryScreen.Bounds.Width - this.Width;
            this.NumOfJobs = numOfJobs;
            this.textBoxDescribe.Text = jobDescription;
            this.defaultStatusText = progressDescription;
            UpdateStatusText();
            this.Show();
            this.SendToBack();
        }

        private void ProgressWindow_Load(object sender, EventArgs e)
        {
            this.Refresh();
        }

        public void SetJob(int currentJobNumber, string optionalString = null)
        {
            if (currentJobNumber > NumOfJobs) NumOfJobs = currentJobNumber;
            this.CurrentJob = currentJobNumber;
            UpdateStatusText(optionalString);
        }

        public void NextJob(string optionalString = null)
        {
            if (CurrentJob == NumOfJobs) NumOfJobs++;
            this.CurrentJob++;
            UpdateStatusText(optionalString);
        }

        void UpdateStatusText(string optionalString = null)
        {
            if (optionalString != null)
            {
                labelStatusUpdate.Text += " " + optionalString;
            }
            labelStatusUpdate.Text = "";
            labelStatusUpdate.Text += defaultStatusText + " (" + CurrentJob.ToString() + " / " + NumOfJobs.ToString() + " )";
            this.progressBar1.Value = (int)(100 * ((float)CurrentJob / (float)NumOfJobs));
            this.progressBar1.Refresh();
            this.Refresh();
        }
    }
}
