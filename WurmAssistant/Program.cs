using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Runtime.CompilerServices;

namespace WurmAssistant
{
    static class Program
    {
        static bool fileAccessFailedMessageDisplayed = false;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread] //There are more threads used by this app but STA seems to be working fine
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(new WurmAssistant());
            }
            catch (Exception _e)
            {
                Logger.LogException(_e);
                throw;
            }
            finally
            {
                Logger.CleanAndDispose();
            }
        }
    }
}
