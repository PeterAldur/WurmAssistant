using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Data.SQLite;
using Microsoft.Win32;
using System.Net;
using System.Text.RegularExpressions;

namespace WurmAssistant
{
    /// <summary>
    /// Works as main class for program, init this UI and Engine.
    /// </summary>
    public partial class WurmAssistant : Form
    {
        public static WurmAssistant ZeroRef;
        public static string DefaultDir = @"Default\";
        public static string PersistentDataDir;
        public static SQLiteDB Database;

        WurmAssistantEngineV2 Engine;
        AC_SettingsDB WASettings;
        FormWurmAssistantUpdate WhatsNew = new FormWurmAssistantUpdate();
        FormWurmAssistantUserGuide UserGuide = new FormWurmAssistantUserGuide();

        bool EngineRunning = false;

        PopupManager popupManager;

        public WurmAssistant()
        {
            InitializeComponent();
            ConsoleManager.EnableConsoleTraceOut(); //debug bld only
            ZeroRef = this;
            InitDataDirectories();

            SetSaveDebugToFile();
            Logger.setOutput(textBoxAppLog, true, null); // output to textbox and text file

            StartPopupManager();

            Database = new SQLiteDB(PersistentDataDir + "WurmAssistantDB.s3db");
            WASettings = new AC_SettingsDB("WurmAssistant", Database, PersistentDataDir + "WA_Settings.xml");
            InitAllSettings();

            if (StartMinimized)
                this.WindowState = FormWindowState.Minimized;

            SoundBank.CreateSoundBank();
            SoundBank.ChangeGlobalVolume(AppDefaultVolume);

            WurmPaths.Initialize();
        }

        private void timerInit_Tick(object sender, EventArgs e)
        {
            timerInit.Enabled = false;

            timerMainLoop.Interval = TimerTickRate;
            HandleUpdatesAndInformUser();
            ShowDeployVersionInCaption();
            TryToAppendUptimeAndTimeToConfigs();
            currentWurmRunning = lastWurmRunning = IsWurmRunning();
            if (FirstLaunch)
            {
                ShowWhatsNew();
                FirstLaunch = false;
            }
            if (TrackedPlayers == null)
            {
                //chooseWurmLogLocation();
                openCharacterConfig();
            }
            if (TrackedPlayers == null)
            {
                Logger.WriteLine("Please choose correct tracking settings in engine menu, then start engine");
            }
            else TryStartEngine(false);
        }

        /// <summary>
        /// Timer loop for main engine
        /// </summary>
        private void timerMainLoop_Tick(object sender, EventArgs e)
        {
            try
            {
                Engine.Update();
                //Debug.WriteLine("tick");
            }
            catch (Exception _e)
            {
                Logger.WriteLine("!! Stopping engine due to unhandled exception");
                Logger.LogException(_e);
                timerMainLoop.Enabled = false;
                EngineRunning = false;
            }
        }

        int counter = 0;

        private void timerPollingLoop_Tick(object sender, EventArgs e)
        {
            if (Engine != null)
            {
                Engine.UpdateOnPollingLoop();
            }
            //HandlePopupQueue();
        }

        /// <summary>
        /// Timer loop only for saving settings
        /// </summary>
        private void timerSaveSettings_Tick(object sender, EventArgs e)
        {
            Engine.TryToSaveModuleSettings();
        }

        public void InitDataDirectories()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string userFilePath = Path.Combine(localAppData, @"AldurCraft\WurmAssistant\");

            if (!Directory.Exists(userFilePath))
            {
                Directory.CreateDirectory(userFilePath);
            }
            PersistentDataDir = userFilePath;

            if (!Directory.Exists(DefaultDir))
            {
                Directory.CreateDirectory(DefaultDir);
            }
        }
        
        //deprec
        public void chooseWurmLogLocation()
        {
            FormWurmAssistantChoosePlayer UIForm = new FormWurmAssistantChoosePlayer(this, LogFilesDirectory);
            if (UIForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (UIForm.RequiresEngineRestart)
                {
                    LogFilesDirectory = UIForm.textBoxPathToLogFiles.Text;
                    FirstLaunch = false;
                    stopEngine();
                    //if (ModuleLogSearcher.LogSearchMan != null) ModuleLogSearcher.LogSearchMan.UpdateWurmLogsPath(LogFilesDirectory);
                    TryStartEngine();
                }
                WASettings.SaveToDB();
            }
        }

        public void openCharacterConfig()
        {
            FormCharacterManager UIForm = new FormCharacterManager(TrackedPlayers);
            // return OK is not complete
            // req flag for logging mode and wurm path change
            // engine should always rebuild until above is fixed, then rebuild only on dialog OK
            if (UIForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FirstLaunch = false;
                stopEngine();
                if (ModuleLogSearcher.LogSearchMan != null)
                {
                    if (UIForm.RequiresLogSearchManagerRebuild)
                    {
                        ModuleLogSearcher.LogSearchMan = null;
                    }
                    //if (TrackedPlayers != null)
                    //{
                    //    //ModuleLogSearcher.LogSearchMan.UpdateWurmLogsPath(WurmPaths.GetLogsDirForPlayer(TrackedPlayers[0]));
                    //}
                    //else
                    //{
                    //    ModuleLogSearcher.LogSearchMan = null;
                    //}
                }
                TryStartEngine();
            }
            WASettings.SaveToDB();
        }

        /// <summary>
        /// NOTE: this rebuilds engine!!
        /// </summary>
        public void TryStartEngine(bool showRunningMsg = true)
        {
            if (!EngineRunning)
            {
                startEngine();
                BuildDynamicInterfaceElements();
            }
            else
            {
                if (showRunningMsg) MessageBox.Show("Engine already running", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void startEngine()
        {
            Logger.WriteLine(">> Starting engine");
            //if (LogFilesDirectory != null && LogFilesDirectory.Trim() != "" && LogFilesDirectory != "noneset")
            //{
                try
                {
                    //Engine = new WurmAssistantEngine(this, LogFilesDirectory);
                    EngineRunning = true;
                    Engine = new WurmAssistantEngineV2(TrackedPlayers);
                    timerMainLoop.Enabled = true;
                    Logger.WriteLine(">> Engine started and running...");
                    UpdateEngineSleepMode();
                    timerSaveSettings.Enabled = true;
                }
                catch (Exception _e)
                {
                    stopEngine();
                    Logger.WriteLine("!! Failed to start engine");
                    Logger.LogException(_e);
                }
            //}
            //else
            //{
            //    Logger.WriteLine("<< Engine start cancelled");
            //    Logger.WriteLine("!! Please choose wurm logs directory before starting engine");
            //}
        }

        private void stopEngine()
        {
            if (EngineRunning)
            {
                timerMainLoop.Enabled = false;
                timerSaveSettings.Enabled = false;
                if (Engine != null) Engine.KillAllModules();
                ClearDynamicInterfaceElements();
                Engine = null;
                EngineRunning = false;
                Logger.WriteLine("<< Engine stopped");
                Logger.WriteLine("-----------------------------------");
            }
        }

        private void SetSaveDebugToFile()
        {
            StreamWriter sw = new StreamWriter(PersistentDataDir + "debuglog.txt", true);
            sw.AutoFlush = true;
            Trace.Listeners.Add(new TextWriterTraceListener(sw));
        }

        private void textBoxAppLog_TextChanged(object sender, EventArgs e)
        {
            TextBoxScrollToBottom();
        }

        private void TextBoxScrollToBottom()
        {
            // autoscroll to bottom
            textBoxAppLog.SelectionStart = textBoxAppLog.Text.Length;
            textBoxAppLog.ScrollToCaret();
        }

        private void WurmAssistant_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.WASettings.SaveToDB();
                try
                {
                    Engine.OnAppClosing();
                }
                catch (NullReferenceException)
                {
                    //in this case ignore null exception, happens when engine not running
                }
                AppDefaultVolume = SoundBank.GlobalVolume;
                Logger.CleanAndDispose();
            }
            catch
            {
                //no error should block exiting the app
            }
        }

        private void WurmAssistant_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void WurmAssistant_Resize(object sender, EventArgs e)
        {
            HandleMinimizeToTray(sender, e);
            TextBoxScrollToBottom();
        }

        public bool OpenSoundBank()
        {
            FormSoundBank SoundBankUI = new FormSoundBank();
            if (SoundBankUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return true;
            }
            else return false;
        }

        static string CachedWurmDirFromRegistry = null;
        /// <summary>
        /// Valid path if found, null if not
        /// </summary>
        /// <returns></returns>
        public string TryGetWurmLogDirFromRegistry()
        {
            if (CachedWurmDirFromRegistry != null) return CachedWurmDirFromRegistry;
            else
            {
                string wurmdir = Convert.ToString(Registry.GetValue(@"HKEY_CURRENT_USER\Software\JavaSoft\Prefs\com\wurmonline\client", "wurm_dir", null));
                if (wurmdir != null)
                {
                    wurmdir = wurmdir.Replace(@"//", @"\");
                    wurmdir = wurmdir.Replace(@"/", @"");
                    wurmdir = wurmdir.Trim();
                    if (!wurmdir.EndsWith(@"\")) wurmdir += @"\";
                    CachedWurmDirFromRegistry = wurmdir;
                    return wurmdir;
                }
                else return null;
            }
        }

        void TryToAppendUptimeAndTimeToConfigs()
        {
            string wurmDir = TryGetWurmLogDirFromRegistry();
            if (wurmDir != null)
            {
                string configsDir = Path.Combine(TryGetWurmLogDirFromRegistry(), @"configs\");

                string[] allConfigs = Directory.GetDirectories(configsDir);
                foreach (string config in allConfigs)
                {
                    string autorunPath = Path.Combine(config + @"\autorun.txt");

                    bool uptimeDoesntExist = true;
                    bool timeDoesntExist = true;
                    if (File.Exists(autorunPath))
                    {
                        using (StreamReader sr = new StreamReader(autorunPath))
                        {
                            string fileContents = sr.ReadToEnd();
                            if (fileContents.Contains("say /uptime"))
                            {
                                uptimeDoesntExist = false;
                            }
                            if (fileContents.Contains("say /time"))
                            {
                                timeDoesntExist = false;
                            }
                        }
                    }
                    if (uptimeDoesntExist || timeDoesntExist)
                    {
                        using (StreamWriter sw = new StreamWriter(autorunPath, true))
                        {
                            if (uptimeDoesntExist)
                            {
                                sw.WriteLine();
                                sw.WriteLine("say /uptime");
                            }
                            if (timeDoesntExist)
                            {
                                sw.WriteLine();
                                sw.WriteLine("say /time");
                            }
                        }
                    }
                }
            }
        }

        #region THREAD SAFE HANDLERS

        public delegate void TimingAssistUptimeSearchCallback(LogSearchData logsearchdata);

        public void InvokeTimingAssistUptimeSearch(LogSearchData logsearchdata)
        {
            ModuleTimingAssist.ZeroRef.EndSearchHistoryForUptime(logsearchdata);
        }

        public delegate void TimingAssistDateTimeSearchCallback(LogSearchData logsearchdata);

        public void InvokeTimingAssistDateTimeSearch(LogSearchData logsearchdata)
        {
            ModuleTimingAssist.ZeroRef.EndSearchHistoryForDateTime(logsearchdata);
        }

        public delegate void LoggerWriteLineCallback(string message);

        public void InvokeLoggerWriteLine(string message)
        {
            Logger.WriteLine(message);
        }

        public delegate void LoggerDisplayExceptionDataCallback(Exception exception);

        public void InvokeLoggerDisplayExceptionData(Exception exception)
        {
            Logger.LogException(exception);
        }

        public delegate void LoggerCriticalExceptionCallback(Exception exception);

        public void InvokeCriticalException(Exception exception)
        {
            Logger.WriteLine("<<< STOPPING ENGINE DUE UNKNOWN EXCEPTION, please report this error");
            stopEngine();
            ModuleLogSearcher.LogSearchMan = null;
            Logger.LogException(exception);
        }

        public delegate void DebugDumpCallback(string filename, List<string> stringlist);

        public void InvokeDebugDump(string filename, List<string> stringlist)
        {
            DebugDump.DumpToTextFile(filename, stringlist);
        }

        #endregion

        #region APPLICATION SETTINGS

        string _wurmDirOverride = null;
        public string WurmDirOverride
        {
            get { return _wurmDirOverride; }
            set
            {
                _wurmDirOverride = value;
                WASettings.SetNullableStr("WurmDirOverride", value);
            }
        }

        string[] _trackerPlayers = null;
        public string[] TrackedPlayers
        {
            get { return _trackerPlayers; }
            set
            {
                _trackerPlayers = value;
                WASettings.SetStrArray("TrackedPlayers", value);
            }
        }

        //depr
        string _logFilesDirectory = "noneset";
        public string LogFilesDirectory
        {
            get { return _logFilesDirectory; }
            private set
            {
                _logFilesDirectory = value;
                WASettings.Set("LogFilesDirectory", value);
            }
        }

        bool _firstLaunch = true;
        public bool FirstLaunch
        {
            get { return _firstLaunch; }
            private set
            {
                _firstLaunch = value;
                WASettings.Set("FirstLaunch", value);
            }
        }

        // tickrate for engine main loop
        int _timerTickRate = 500;
        public int TimerTickRate
        {
            get { return _timerTickRate; }
            private set
            {
                _timerTickRate = value;
                timerMainLoop.Interval = value;
                WASettings.Set("TimerTickRate", value);
                if (Engine != null)
                    Engine.AppSettingsChanged();
            }
        }

        bool _displayAllLogEvents = false;
        public bool DisplayAllLogEvents
        {
            get { return _displayAllLogEvents; }
            set
            {
                _displayAllLogEvents = value;
                WASettings.Set("DisplayAllLogEvents", value);
                if (Engine != null)
                    Engine.AppSettingsChanged();
            }
        }

        bool _notifyIconBallonShown = false;
        public bool NotifyIconBallonShown
        {
            get { return _notifyIconBallonShown; }
            set
            {
                _notifyIconBallonShown = value;
                WASettings.Set("NotifyIconBallonShown", value);
                if (Engine != null)
                    Engine.AppSettingsChanged();
            }
        }

        bool _appSetToMinimizeToTray = false;
        public bool AppSetToMinimizeToTray
        {
            get { return _appSetToMinimizeToTray; }
            set
            {
                _appSetToMinimizeToTray = value;
                WASettings.Set("AppSetToMinimizeToTray", value);
                if (Engine != null)
                    Engine.AppSettingsChanged();
            }
        }

        float _appDefaultVolume = 1.0F;
        public float AppDefaultVolume
        {
            get { return _appDefaultVolume; }
            set
            {
                _appDefaultVolume = value;
                WASettings.Set("AppDefaultVolume", value);
            }
        }

        bool _dailyLoggingMode = false;
        public bool DailyLoggingMode
        {
            get { return _dailyLoggingMode; }
            set
            {
                _dailyLoggingMode = value;
                WASettings.Set("DailyLoggingMode", value);
            }
        }

        int _lastDeplRevision = 0;
        public int LastDeplRevision
        {
            get { return _lastDeplRevision; }
            set
            {
                _lastDeplRevision = value;
                WASettings.Set("LastDeplRevision", value);
            }
        }

        int _lastDeplBuild = 0;
        public int LastDeplBuild
        {
            get { return _lastDeplBuild; }
            set
            {
                _lastDeplBuild = value;
                WASettings.Set("LastDeplBuild", value);
            }
        }

        int _lastDeplMinor = 0;
        public int LastDeplMinor
        {
            get { return _lastDeplMinor; }
            set
            {
                _lastDeplMinor = value;
                WASettings.Set("LastDeplMinor", value);
            }
        }

        int _lastDeplMajor = 0;
        public int LastDeplMajor
        {
            get { return _lastDeplMajor; }
            set
            {
                _lastDeplMajor = value;
                WASettings.Set("LastDeplMajor", value);
            }
        }

        bool _startMinimized = false;
        public bool StartMinimized
        {
            get { return _startMinimized; }
            set
            {
                _startMinimized = value;
                WASettings.Set("StartMinimized", value);
            }
        }

        void InitAllSettings()
        {
            _wurmDirOverride = WASettings.GetNullableStr("WurmDirOverride", _wurmDirOverride);
            _trackerPlayers = WASettings.GetStrArray("TrackedPlayers", _trackerPlayers);

            _logFilesDirectory = WASettings.Get("LogFilesDirectory", _logFilesDirectory);
            _firstLaunch = WASettings.Get("FirstLaunch", _firstLaunch);
            _timerTickRate = WASettings.Get("TimerTickRate", _timerTickRate);
            _displayAllLogEvents = WASettings.Get("DisplayAllLogEvents", _displayAllLogEvents);
            _notifyIconBallonShown = WASettings.Get("NotifyIconBallonShown", _notifyIconBallonShown);
            _appSetToMinimizeToTray = WASettings.Get("AppSetToMinimizeToTray", _appSetToMinimizeToTray);
            _appDefaultVolume = WASettings.Get("AppDefaultVolume", _appDefaultVolume);
            _dailyLoggingMode = WASettings.Get("DailyLoggingMode", _dailyLoggingMode);
            _lastDeplRevision = WASettings.Get("LastDeplRevision", _lastDeplRevision);
            _lastDeplBuild = WASettings.Get("LastDeplBuild", _lastDeplBuild);
            _lastDeplMinor = WASettings.Get("LastDeplMinor", _lastDeplMinor);
            _lastDeplMajor = WASettings.Get("LastDeplMajor", _lastDeplMajor);
            _startMinimized = WASettings.Get("StartMinimized", _startMinimized);
        }

        #endregion

        #region ENGINE SLEEP MODE

        bool lastWurmRunning = false;
        bool currentWurmRunning = false;

        bool _wurmisonline;

        bool IsWurmRunning()
        {
            _wurmisonline = false;
            try
            {
                Process[] allActiveProcesses = Process.GetProcessesByName("javaw");
                foreach (var process in allActiveProcesses)
                {
                    if (process.MainWindowTitle.StartsWith("Wurm Online", StringComparison.Ordinal)) _wurmisonline = true;
                }
                return _wurmisonline;
            }
            catch (Exception _e)
            {
                Logger.WriteLine("There was an error while checking for active Wurm client: " + _e.Message);
                Logger.WriteLine("Setting has been defaulted to active, program will run correctly. Please report this bug!");
                return true;
            }
        }

        private void timerIsWurmRunning_Tick(object sender, EventArgs e)
        {
            currentWurmRunning = IsWurmRunning();
            if (currentWurmRunning != lastWurmRunning)
            {
                if (UpdateEngineSleepMode())
                {
                    if (currentWurmRunning)
                    {
                        Logger.WriteLine("Found active Wurm Online client, engine has been awakened");
                        Engine.OnEngineWakeUp();
                    }
                }
                lastWurmRunning = currentWurmRunning;
            }
        }

        private bool UpdateEngineSleepMode()
        {
            #if DEBUG
            return true; //DEBUG OVERRIDE
            #endif
            if (currentWurmRunning)
            {
                if (Engine != null)
                {
                    Engine.SleepMode = false;
                    timerIsWurmRunning.Interval = 10000;
                    return true;
                }
                else return false;
            }
            else
            {
                if (Engine != null)
                {
                    Engine.SleepMode = true;
                    timerIsWurmRunning.Interval = 5000;
                    Logger.WriteLine("No active Wurm Online client found, engine has been put into sleep mode");
                    return true;
                }
                else return false;
            }
        }

        #endregion

        #region UPDATE AND VERSION

        public void ShowWhatsNew()
        {
            try
            {
                WhatsNew.Show();
            }
            catch
            {
                WhatsNew = new FormWurmAssistantUpdate();
                WhatsNew.Show();
            }
        }

        public void ShowWhatsNewIfNotRevisionUpdate(Version curVer)
        {
            if (curVer != null)
            {
                if (curVer.Revision > LastDeplRevision
                    && curVer.Build == LastDeplBuild
                    && curVer.Minor == LastDeplMinor
                    && curVer.Major == LastDeplMajor)
                {
                    Logger.WriteLine(">>> Wurm Assistant updated to " + curVer.ToString(4) + " (revision update, you can check change log for details)");
                }
                else
                {
                    ShowWhatsNew();
                }
            }
            else
            {
                Logger.WriteLine("Error: Current publish version was null");
            }
        }

        private void ShowDeployVersionInCaption()
        {
            Version curVer = GetDeployVersion();
            if (curVer != null)
            {
                this.Text += " (" + curVer.ToString(4) + ")";
            }
            else this.Text += " (unknown version)";
        }

        public void HandleUpdatesAndInformUser()
        {
            Version curVer = GetDeployVersion();

            try
            {
                if (System.Deployment.Application.ApplicationDeployment.CurrentDeployment.IsFirstRun)
                {
                    ShowWhatsNewIfNotRevisionUpdate(curVer);
                }
            }
            catch (System.Deployment.Application.InvalidDeploymentException)
            {
                Logger.WriteLine("Minor error: Invalid deployment. Please report this bug! Program should run correctly.");
            }

            if (curVer != null)
            {
                if (curVer.Revision != LastDeplRevision
                    && curVer.Build != LastDeplBuild
                    && curVer.Minor != LastDeplMinor
                    && curVer.Major != LastDeplMajor)
                {
                    LastDeplRevision = curVer.Revision;
                    LastDeplBuild = curVer.Build;
                    LastDeplMinor = curVer.Minor;
                    LastDeplMajor = curVer.Major;
                }
            }
        }

        public Version GetDeployVersion()
        {
            try
            {
                return System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region TRAY AND NOTIFICATIONS

        private void HandleMinimizeToTray(object sender, EventArgs e)
        {
            if (AppSetToMinimizeToTray)
            {
                if (FormWindowState.Minimized == this.WindowState)
                {
                    notifyIcon1.Visible = true;
                    if (!NotifyIconBallonShown)
                    {
                        notifyIcon1.ShowBalloonTip(5000);
                        NotifyIconBallonShown = true;
                    }
                    this.Hide();
                }
                else if (FormWindowState.Normal == this.WindowState)
                {
                    notifyIcon1.Visible = false;
                }
            }
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStripTray.Show();
            }
        }

        private void TrayExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TrayRestoreMainWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
        }

        private void TraySoundNotifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            soundNotifyToolStripMenuItem.PerformClick();
        }

        private void TrayOpenLogSearcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logSearcherToolStripMenuItem.PerformClick();
        }

        private void TrayOpenTimersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timersToolStripMenuItem.PerformClick();
        }

        private void TrayOpenCalendarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            calendarToolStripMenuItem.PerformClick();
        }

        private void trayOpenGrangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grangerToolStripMenuItem.PerformClick();
        }

        #endregion

        #region POPUP HANDLING

        private void StartPopupManager()
        {
            popupManager = new PopupManager();
        }

        public void ScheduleCustomPopupNotify(string title, string content, int timeToShowMillis = 4000)
        {
            popupManager.ScheduleCustomPopupNotify(title, content, timeToShowMillis);
        }

        #endregion

        #region HTTP REQUEST (TimingAssist)

        public bool BeginHttpExtractor(ModuleTimingAssist.TimingAssistWorkerBasket basket)
        {
            if (!backgroundWorkerHttpRequest.IsBusy)
            {
                Logger.WriteLine("Debug: Starting http background worker");
                backgroundWorkerHttpRequest.RunWorkerAsync(basket);
                return true;
            }
            else return false;
        }

        private void backgroundWorkerHttpRequest_DoWork(object sender, DoWorkEventArgs e)
        {
            ModuleTimingAssist.ZeroRef.HttpExtractorWorkerToDo(sender, e);
        }

        private void backgroundWorkerHttpRequest_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ModuleTimingAssist.ZeroRef.EndHttpExtractor(e.Result);
        }

        #endregion

        #region MAIN MENU STATIC INTERFACE

        private void configureCharactersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //chooseWurmLogLocation();
            openCharacterConfig();
        }

        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormWurmAssistantConfig configdialog = new FormWurmAssistantConfig(this);
            if (configdialog.ShowDialog() == DialogResult.OK)
            {
                TimerTickRate = (int)configdialog.numericUpDownMillis.Value;
                AppSetToMinimizeToTray = configdialog.checkBoxMiniToTray.Checked;
                StartMinimized = configdialog.checkBoxStartMinimized.Checked;
                WASettings.SaveToDB();
            }
            this.BringToFront();
        }

        private void about1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WurmAssistantAbout aboutbox = new WurmAssistantAbout();
            aboutbox.ShowDialog();
        }

        private void whatsNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowWhatsNew();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopEngine();
        }

        //dynamic interface
        private void soundNotifyToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //if (Engine != null)
            //{
            //    Engine.moduleSoundNotify.ToggleUI();
            //}
            //else MessageBox.Show("Engine not running", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TryStartEngine();
        }

        private void logSearcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Engine != null)
            {
                Engine.ToggleLogSearcherUI();
            }
            else MessageBox.Show("Engine not running", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        //dynamic interface
        private void timersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (Engine != null)
            //{
            //    Engine.moduleTimers.ToggleUI();
            //}
            //else MessageBox.Show("Engine not running", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void calendarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Engine != null)
            {
                Engine.ToggleCalendarUI();
            }
            else MessageBox.Show("Engine not running", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void grangerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Engine != null)
            {
                Engine.ToggleGrangerUI();
            }
            else MessageBox.Show("Engine not running", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void findOtherCommunityToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will open a forum thread on official Wurm website, do you wish to continue?", "Confirm", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                Process.Start(@"http://forum.wurmonline.com/index.php?/topic/60914-wurm-resources/");
            }
        }

        private void guideToWurmAssistantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UserGuide.Show();
            }
            catch
            {
                UserGuide = new FormWurmAssistantUserGuide();
                UserGuide.Show();
            }
        }

        //just for testing
        private void tESTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCharacterManager CharacterUI = new FormCharacterManager(TrackedPlayers);
            if (CharacterUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                stopEngine();
                if (ModuleLogSearcher.LogSearchMan != null)
                {
                    if (TrackedPlayers != null && TrackedPlayers.Length > 0)
                    {
                        //if (ModuleLogSearcher.LogSearchMan != null) ModuleLogSearcher.LogSearchMan.UpdateWurmLogsPath(LogFilesDirectory);
                        //ModuleLogSearcher.LogSearchMan.UpdateWurmLogsPath(WurmPaths.GetLogsDirForPlayer(TrackedPlayers[0]));
                    }
                }
                TryStartEngine();
            }

            string output = "Tracked Players: ";
            if (TrackedPlayers == null) output += "NULL";
            else
            {
                foreach (string player in TrackedPlayers)
                {
                    output += player + ", ";
                }
            }
            Logger.WriteLine(output);
        }

        #endregion

        #region MENU DYNAMIC INTERFACE

        public delegate void ToggleUIDelegate(object sender, EventArgs e);

        void BuildDynamicInterfaceElements()
        {
            ClearDynamicInterfaceElements();
            try
            {
                List<DynaMenuItemData> timersMenuItems = Engine.GetTimersMenuItems();
                foreach (DynaMenuItemData item in timersMenuItems)
                {
                    timersToolStripMenuItem.DropDownItems.Add(item.DisplayName, null, new EventHandler(item.ToggleMethod));
                    TrayOpenTimersToolStripMenuItem.DropDownItems.Add(item.DisplayName, null, new EventHandler(item.ToggleMethod));

                }
                List<DynaMenuItemData> soundNotifyMenuItems = Engine.GetSoundNotifyMenuItems();
                foreach (DynaMenuItemData item in soundNotifyMenuItems)
                {
                    soundNotifyToolStripMenuItem.DropDownItems.Add(item.DisplayName, null, new EventHandler(item.ToggleMethod));
                    TrayOpenSoundNotifyToolStripMenuItem.DropDownItems.Add(item.DisplayName, null, new EventHandler(item.ToggleMethod));
                }
            }
            catch (Exception _e)
            {
                Logger.WriteLine("! error while building dynamic interfaces");
                Logger.LogException(_e);
            }
        }

        void ClearDynamicInterfaceElements()
        {
            //removes the items and eventhandlers will be disposed
            timersToolStripMenuItem.DropDownItems.Clear();
            TrayOpenTimersToolStripMenuItem.DropDownItems.Clear();

            soundNotifyToolStripMenuItem.DropDownItems.Clear();
            TrayOpenSoundNotifyToolStripMenuItem.DropDownItems.Clear();
        }

        public class DynaMenuItemData
        {
            public string DisplayName;
            public ToggleUIDelegate ToggleMethod;

            public DynaMenuItemData(string displayName, ToggleUIDelegate toggleMethod)
            {
                DisplayName = displayName;
                ToggleMethod = toggleMethod;
            }
        }

        #endregion
    }
}
