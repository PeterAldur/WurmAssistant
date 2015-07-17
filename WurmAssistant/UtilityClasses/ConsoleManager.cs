using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace WurmAssistant
{
    /// <summary>
    /// Sets and disables writing console output to text file or to console.
    /// Used for debugging only.
    /// </summary>
    static class ConsoleManager
    {
        static FileStream filestream;
        static StreamWriter streamwriter_ConsoleOut;
        static string defConsoleOutFile = "consoleout.txt";
        static TextWriter textwriter_ConsoleDefaultOut = Console.Out;
        static ConsoleTraceListener debugConsoleWriter;
        static bool FileOutEnabled = false;

        static ConsoleManager()
        {
            Application.ApplicationExit += new System.EventHandler(OnApplicationExit);
        }

        /// <summary>
        /// Adds console as trace listener for debugging messages
        /// </summary>
        static public void EnableConsoleTraceOut()
        {
            debugConsoleWriter = new ConsoleTraceListener();
            Trace.Listeners.Add(debugConsoleWriter);
        }

        /// <summary>
        /// Redirects console output to text file "consoleout.txt"
        /// </summary>
        static public void EnableConsoleFileOut()
        {
            filestream = new FileStream(defConsoleOutFile, FileMode.Create);
            streamwriter_ConsoleOut = new StreamWriter(filestream);
            streamwriter_ConsoleOut.AutoFlush = true;
            //Console.SetOut(streamwriter_ConsoleOut);
            Debug.WriteLine(DateTime.Now.ToString() + ": Console output saving enabled");
            FileOutEnabled = true;
        }

        /// <summary>
        /// Redirects console output to specified text file
        /// </summary>
        /// <param name="address">valid file name including extension</param>
        static public void EnableConsoleFileOut(string address)
        {
            filestream = new FileStream(address, FileMode.Create);
            streamwriter_ConsoleOut = new StreamWriter(filestream);
            streamwriter_ConsoleOut.AutoFlush = true;
            Console.SetOut(streamwriter_ConsoleOut);
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString() + ": Console output saving enabled to specified file adress:" + address);
            FileOutEnabled = true;
        }

        /// <summary>
        /// Disables redirecting console output, restores default out, cleans the resources
        /// </summary>
        static public void DisableConsoleFileOut()
        {
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString() + ": Console output saving disabled");
            streamwriter_ConsoleOut.Dispose();
            filestream.Dispose();
            Console.SetOut(textwriter_ConsoleDefaultOut);
            FileOutEnabled = false;
        }

        static private void OnApplicationExit(object sender, EventArgs e)
        {
            if (FileOutEnabled == true) DisableConsoleFileOut();
        }
    }
}
