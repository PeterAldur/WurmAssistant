using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace WurmAssistant
{
    /// <summary>
    /// Text file wrapper
    /// </summary>
    class TextFileObject
    {
        // address of the wraped file
        string fileAdress;

        // current numer of lines in this file
        int LinesMaxIndex = 0;

        // list of all lines in this file
        List<string> Lines = new List<string>();

        // primitive iterator for getnextline method
        int currentReadIndex = 0;

        // used to determine if to read new lines from file
        long lastSizeOfThisFile = 0;

        // should instance be allowed to update its Lines list
        bool doUpdate = false;

        // should instance do full update regardless of anything
        bool alwaysUpdate = false;

        // does file exist on drive
        bool fileExists = false;
        public bool FileExists
        {
            get { return fileExists; }
        }

        // whether this file should be handled as read only
        bool isReadOnly = false;

        // whether this file is only appended to (like log files)
        bool isGrowingFileOnly = false;

        // if display log when file found
        bool WrapNotify = true;

        bool LogReaderMode = false;

        bool firstUpdate = true;

        bool NewLineOnCRLF = false;

        FileInfo currentFileInfo;

        /// <summary>
        /// constructs new text file wrapper
        /// </summary>
        /// <param name="fileAdress">path to the file</param>
        /// <param name="doUpdate">true to enable reading from this file</param>
        /// <param name="alwaysUpdate">true if update should happen regardless of file size changes</param>
        /// <param name="createIfNotExists">true if file should be created if not exists</param>
        /// <param name="isReadOnly">true if writing to file should be disabled, also disables creating file</param>
        /// <param name="isGrowingOnly">true if text is only appended to this file (more efficient file reading)</param>
        /// <param name="notifyWhenWrapped">true if should notify logger when file aquired</param>
        public TextFileObject(string fileAdress, bool doUpdate, bool alwaysUpdate, bool createIfNotExists, bool isReadOnly, bool isGrowingOnly, bool notifyWhenWrapped = true, bool logReaderMode = false, bool newLineOnlyOnCRLF = false)
        {
            this.doUpdate = doUpdate;
            this.fileAdress = fileAdress;
            this.alwaysUpdate = alwaysUpdate;
            this.isReadOnly = isReadOnly;
            this.isGrowingFileOnly = isGrowingOnly;
            this.WrapNotify = notifyWhenWrapped;
            this.LogReaderMode = logReaderMode;
            this.NewLineOnCRLF = newLineOnlyOnCRLF;

            this.CheckIfFileExists();

            this.currentFileInfo = new FileInfo(fileAdress);

            if (this.fileExists == false)
            {
                if (createIfNotExists && !isReadOnly) this.CreateIfNotExists();
            }

            this.Update();
        }

        private void CreateIfNotExists()
        {

            try
            {
                if (!File.Exists(fileAdress))
                {
                    File.WriteAllText(fileAdress, "");
                    fileExists = true;
                }
            }
            catch
            {
                Debug.WriteLine("file does not exist, failed to create: " + fileAdress);
            }
        }

        /// <summary>
        /// Reads the file and updates List of lines
        /// </summary>
        public void Update()
        {
            if (doUpdate)
            {
                if (!fileExists)
                {
                    CheckIfFileExists();
                }

                if (fileExists)
                {
                    if (CheckForFileChanged())
                    {
                        try
                        {
                            using (FileStream fs = new FileStream(fileAdress, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                            {
                                if (isGrowingFileOnly) fs.Seek(lastSizeOfThisFile, SeekOrigin.Begin);
                                else Lines.Clear();

                                if (LogReaderMode)
                                {
                                    Lines.Clear();
                                    resetReadPos();
                                    if (firstUpdate)
                                    {
                                        fs.Seek(0, SeekOrigin.End);
                                        firstUpdate = false;
                                    }
                                }

                                using (StreamReader sr = new StreamReader(fs))
                                {
                                    if (NewLineOnCRLF)
                                    {
                                        string thefile = sr.ReadToEnd();
                                        string[] strlist = thefile.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                                        Lines.AddRange(strlist);
                                    }
                                    else
                                    {
                                        while (!sr.EndOfStream)
                                        {
                                            Lines.Add(sr.ReadLine());
                                            //Debug.WriteLine(DateTime.Now + " line added to list at TextFileObject update");
                                        }
                                    }
                                    lastSizeOfThisFile = fs.Position;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("The file could not be updated: ");
                            Debug.WriteLine(e.Message);
                            fileExists = false;
                        }
                        LinesMaxIndex = Lines.Count() - 1;
                    }
                }
            }
        }

        private void CheckIfFileExists()
        {
            if (File.Exists(fileAdress))
            {
                fileExists = true;
                if (WrapNotify) TSafeLogger.WriteLine("Wrapped file: " + fileAdress);
            }
        }

        private bool CheckForFileChanged()
        {
            if (!alwaysUpdate)
            {
                try
                {
                    currentFileInfo.Refresh();
                    if (currentFileInfo.Length != lastSizeOfThisFile)
                        return true;
                    else return false;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("The file info could not be read:");
                    Debug.WriteLine(e.Message);
                    return false;
                }
            }
            else return true;
        }

        /// <summary>
        /// Returns one line from text file at specified index
        /// </summary>
        /// <param name="index">index of the line starting 0</param>
        /// <returns>null if any exception</returns>
        public string ReadLine(int index)
        {
            try
            {
                if (index <= LinesMaxIndex)
                    return Lines[index];
                else return null;
            }
            catch
            {
                Debug.WriteLine("exception at GetLine(index) - empty text file?");
                return null;
            }
        }

        /// <summary>
        /// Returns next line from text file, starting from the beginning and using internal counter.
        /// Can be reset with resetReadPos()
        /// </summary>
        /// <returns>null if any exception</returns>
        public string ReadNextLine()
        {
            try
            {
                if (currentReadIndex <= LinesMaxIndex)
                {
                    currentReadIndex++;
                    return Lines[currentReadIndex - 1];
                }
                else return null;
            }
            catch
            {
                Debug.WriteLine("exception at GetNextLine()");
                //Debugger.Break();
                return null;
            }
        }

        /// <summary>
        /// Returns next line from text file, with an offset
        /// </summary>
        /// <param name="offset">offset that causes line number minus offset to be treated as last line number</param>
        /// <returns>null if any exception</returns>
        public string ReadNextLineOffset(int offset)
        {
            try
            {
                if (currentReadIndex <= LinesMaxIndex - offset)
                {
                    currentReadIndex++;
                    return Lines[currentReadIndex - 1];
                }
                else return null;
            }
            catch
            {
                Debug.WriteLine("exception at GetLineOffset(offset)");
                return null;
            }
        }

        /// <summary>
        /// Returns last line in file
        /// </summary>
        /// <returns>null if any exception</returns>
        public string ReadLastLine()
        {
            try
            {
                return Lines[LinesMaxIndex];
            }
            catch
            {
                Debug.WriteLine("exception at GetLastLine()");
                return null;
            }
        }

        /// <summary>
        /// Returns last line in file with offset
        /// </summary>
        /// <param name="offset">offset that causes line number minus offset to be treated as last line number</param>
        /// <returns> null if any exception</returns>
        public string GetLastLine(int offset)
        {
            try
            {
                return Lines[LinesMaxIndex - offset];
            }
            catch
            {
                Debug.WriteLine("exception at GetLastLine()");
                return null;
            }
        }

        /// <summary>
        /// Resets internal iterator for getNextLine methods to 0
        /// </summary>
        public void resetReadPos()
        {
            currentReadIndex = 0;
        }

        /// <summary>
        /// Returns index of the last line in this file, (indexing starts at 0)
        /// </summary>
        /// <returns></returns>
        public int getLastIndex()
        {
            return LinesMaxIndex;
        }

        /// <summary>
        /// Appends a new text line to the file
        /// </summary>
        /// <param name="text"></param>
        /// <returns>false if operation failed</returns>
        public bool WriteLine(string text)
        {
            if (fileExists && !isReadOnly)
            {
                try
                {
                    using (StreamWriter sw = File.AppendText(fileAdress))
                    {
                        sw.WriteLine(text);
                    }
                    Update();
                    return true;
                }
                catch
                {
                    Debug.WriteLine("error while adding line to file: " + fileAdress);
                    return false;
                }
            }
            else return false;
        }

        /// <summary>
        /// Appends a List of new lines to the file
        /// </summary>
        /// <param name="textlist"></param>
        /// <returns>false if operation failed</returns>
        public bool WriteLines(List<string> textlist)
        {
            if (fileExists && !isReadOnly)
            {
                try
                {
                    using (StreamWriter sw = File.AppendText(fileAdress))
                    {
                        foreach (string text in textlist)
                        {
                            sw.WriteLine(text);
                        }
                    }
                    Update();
                    return true;
                }
                catch
                {
                    Debug.WriteLine("error while adding lines to file: " + fileAdress);
                    return false;
                }
            }
            else return false;
        }

        /// <summary>
        /// Clears the file and writes provided new List of lines to this file
        /// </summary>
        /// <param name="textlist"></param>
        /// <returns>false if operation failed</returns>
        public bool RewriteFile(List<string> textlist)
        {
            if (fileExists && !isReadOnly)
            {
                ClearFile();
                WriteLines(textlist);
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Clears the file
        /// </summary>
        /// <returns>false if clearing failed</returns>
        public bool ClearFile()
        {
            if (fileExists && !isReadOnly)
            {
                try
                {
                    File.WriteAllText(fileAdress, String.Empty);
                    resetReadPos();
                    Update();
                    return true;
                }
                catch
                {
                    Debug.WriteLine("error while clearing file: " + fileAdress);
                    return false;
                }
            }
            else return false;
        }

        /// <summary>
        /// Returns all lines as string array
        /// </summary>
        /// <returns>all lines in this text file</returns>
        public string[] getAllLines()
        {
            return Lines.ToArray();
        }

        /// <summary>
        /// Backups the file in the same directory with .bak extension appended to it
        /// </summary>
        public void BackupFile()
        {
            File.Copy(fileAdress, fileAdress + ".bak", true);
        }
    }
}
