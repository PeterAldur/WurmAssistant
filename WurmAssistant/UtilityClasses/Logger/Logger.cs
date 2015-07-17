using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace WurmAssistant
{
    /// <summary>
    /// Logging class for the program internal log. Can be assigned to a multiline textbox, 
    /// set to save to file, or output log messages through a generic string List for custom use
    /// </summary>
    static class Logger
    {
        // reference to output textbox
        static TextBox LogTextBox;
        // stores all log messages, autocleans
        static List<string> LogMessages = new List<string>();
        // used for custom output
        static List<string> outputListMessages;
        // if save internal log to file
        static bool bool_SaveToLogFile = false;
        // address for the internal log file
        static string fileAdress;

        static bool loggingDisabled = false;

        static StreamWriter logfileWriter;

        /// <summary>
        /// Sets outputs for the logger
        /// </summary>
        /// <param name="textbox">null if not needed, textbox should be set for multiline</param>
        /// <param name="ifSaveToLogFile">true if save output to text file</param>
        /// <param name="listOutput">null if not needed, otherwise ref to list that logger will Add to</param>
        public static void setOutput(TextBox textbox, bool ifSaveToLogFile, List<string> listOutput)
        {
            fileAdress = Path.Combine(WurmAssistant.PersistentDataDir, "WurmAssistant_log.txt");

            int outputCount = 0;
            if (textbox != null)
            {
                LogTextBox = textbox;
                outputCount++;
            }
            if (ifSaveToLogFile == true)
            {
                bool_SaveToLogFile = true;
                File.WriteAllText(fileAdress, String.Empty);
                logfileWriter = File.AppendText(fileAdress);
                outputCount++;
            }
            if (listOutput != null)
            {
                outputListMessages = listOutput;
                outputCount++;
            }

            if (outputCount == 0)
            {
                loggingDisabled = true;
            }
        }

        /// <summary>
        /// Adds one text entry to logger
        /// </summary>
        /// <param name="note"></param>
        public static void WriteLine(string note)
        {
            if (!loggingDisabled)
            {
                string _note = DateTime.Now.ToString("[HH:mm:ss] ") + note;
                LogMessages.Add(_note);
                if (LogMessages.Count > 1000)
                {
                    LogMessages.RemoveRange(0, 899);
                }
                if (LogTextBox != null)
                    RefreshLogTextBox();
                if (bool_SaveToLogFile == true)
                    SaveToLogFile(note);
                if (outputListMessages != null)
                    outputListMessages.Add(note);
            }
        }

        /// <summary>
        /// Creates and sends updated string array to the TextBox
        /// </summary>
        static void RefreshLogTextBox()
        {
            int msgcounttocopy;
            int copyliststartindex = 0;
            if (LogMessages.Count > 500)
            {
                msgcounttocopy = 500;
                copyliststartindex = LogMessages.Count - 500;
            }
            else msgcounttocopy = LogMessages.Count;

            string[] tempArray = new string[msgcounttocopy];

            LogMessages.CopyTo(copyliststartindex, tempArray, 0, msgcounttocopy);
            try
            {
                LogTextBox.Lines = tempArray;
            }
            catch
            {
                Debug.WriteLine("Exception while updating logtextbox");
            }
        }

        /// <summary>
        /// Appends new log text entry to the log text file
        /// </summary>
        /// <param name="note"></param>
        static void SaveToLogFile(string note)
        {
            note = DateTime.Now.ToString("[dd/MM/yyyy HH:mm:ss] ") + note;

            try
            {
                writer(note);
            }
            catch
            {
                Logger.WriteLine("Logger: error while writing to file, disabling");
                bool_SaveToLogFile = false;
            }
        }

        static void writer(string text)
        {
            logfileWriter.WriteLine(text);
            logfileWriter.Flush();
        }

        static public void LogException(Exception _e, bool warningsound = false)
        {
            Logger.WriteLine("EXCEPTION: " + _e.Message);
            Logger.WriteLine("SOURCE: " + _e.Source);
            Logger.WriteLine("TRACE: ");
            Logger.WriteLine(_e.StackTrace);
            if (warningsound) System.Media.SystemSounds.Hand.Play();
        }

        public static void CleanAndDispose()
        {
            if (logfileWriter != null) logfileWriter.Dispose();
        }
    }
}
