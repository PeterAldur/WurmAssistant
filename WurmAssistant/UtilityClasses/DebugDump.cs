using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace WurmAssistant
{
    static class DebugDump
    {
        static string DumpDirProto = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        static string DumpDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), @"debugdumps");
        static string FileName = "default.txt";
        static TextFileObject DumpFile;

        static DebugDump()
        {
            if (!Directory.Exists(DumpDir))
            {
                Directory.CreateDirectory(DumpDir);
            }
        }

        public static void SetDumpFile(string filename)
        {
            FileName = filename;
            DumpFile = new TextFileObject(Path.Combine(DumpDir, FileName), false, false, true, false, false, false);
        }

        public static void WriteLine(string line)
        {
            if (DumpFile != null) DumpFile.WriteLine(line);
        }

        public static void ClearFile()
        {
            if (DumpFile != null) DumpFile.ClearFile();
        }

        public static void Clean()
        {
            DumpFile = null;
        }

        public static void DumpToTextFile(string filename, List<string> stringlist)
        {
            string dumpfileaddress = Path.Combine(DumpDir, filename);
            TextFileObject dumpfile = new TextFileObject(dumpfileaddress, false, false, true, false, false, false);
            dumpfile.WriteLines(stringlist);
        }

        public static void TSafeDumpToTextFile(string filename, List<string> stringlist)
        {
            WurmAssistant.ZeroRef.BeginInvoke(new WurmAssistant.DebugDumpCallback(WurmAssistant.ZeroRef.InvokeDebugDump), new object[] { filename, stringlist });
        }
    }
}
