using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WurmAssistant.Granger
{
    static class GrangerDebug
    {
        const bool Enabled = true;
        static FormGrangerDebug ui;

        static GrangerDebug()
        {
            if (Enabled)
            {
                ui = new FormGrangerDebug();
            }
        }

        public static void Log(string message)
        {
            if (Enabled)
            {
                try
                {
                    ui.WriteToTextbox(message);
                }
                catch
                {
                    ui = new FormGrangerDebug();
                }
            }
        }

        internal static void Show()
        {
            if (Enabled)
            {
                try
                {
                    if (ui.Visible == true) ui.WindowState = System.Windows.Forms.FormWindowState.Normal;
                    else ui.Show();
                }
                catch
                {
                    ui = new FormGrangerDebug();
                    ui.Show();
                }
            }
        }
    }
}
