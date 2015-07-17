namespace WurmAssistant
{
    partial class WurmAssistant
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WurmAssistant));
            this.timerMainLoop = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.configureCharactersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.soundNotifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logSearcherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calendarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grangerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.whatsNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guideToWurmAssistantToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.findOtherCommunityToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.about1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tESTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxAppLog = new System.Windows.Forms.TextBox();
            this.timerInit = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStripTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TrayRestoreMainWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.TrayOpenSoundNotifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayOpenLogSearcherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayOpenTimersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayOpenCalendarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trayOpenGrangerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.TrayExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerIsWurmRunning = new System.Windows.Forms.Timer(this.components);
            this.timerSaveSettings = new System.Windows.Forms.Timer(this.components);
            this.timerPollingLoop = new System.Windows.Forms.Timer(this.components);
            this.popupNotifier1 = new NotificationWindow.PopupNotifier();
            this.backgroundWorkerHttpRequest = new System.ComponentModel.BackgroundWorker();
            this.popupNotifierExt1 = new NotifyWindowExt.PopupNotifierExt();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStripTray.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerMainLoop
            // 
            this.timerMainLoop.Interval = 250;
            this.timerMainLoop.Tick += new System.EventHandler(this.timerMainLoop_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.soundNotifyToolStripMenuItem,
            this.logSearcherToolStripMenuItem,
            this.timersToolStripMenuItem,
            this.calendarToolStripMenuItem,
            this.grangerToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.tESTToolStripMenuItem});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(827, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startToolStripMenuItem,
            this.toolStripSeparator1,
            this.configureCharactersToolStripMenuItem,
            this.configureToolStripMenuItem,
            this.toolStripSeparator2,
            this.stopToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(66, 24);
            this.settingsToolStripMenuItem.Text = "Engine";
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.Size = new System.Drawing.Size(189, 24);
            this.startToolStripMenuItem.Text = "Start";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(186, 6);
            // 
            // configureCharactersToolStripMenuItem
            // 
            this.configureCharactersToolStripMenuItem.Name = "configureCharactersToolStripMenuItem";
            this.configureCharactersToolStripMenuItem.Size = new System.Drawing.Size(189, 24);
            this.configureCharactersToolStripMenuItem.Text = "Tracking settings";
            this.configureCharactersToolStripMenuItem.Click += new System.EventHandler(this.configureCharactersToolStripMenuItem_Click);
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(189, 24);
            this.configureToolStripMenuItem.Text = "Engine Settings";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(186, 6);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(189, 24);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // soundNotifyToolStripMenuItem
            // 
            this.soundNotifyToolStripMenuItem.Name = "soundNotifyToolStripMenuItem";
            this.soundNotifyToolStripMenuItem.Size = new System.Drawing.Size(104, 24);
            this.soundNotifyToolStripMenuItem.Text = "SoundNotify";
            this.soundNotifyToolStripMenuItem.Click += new System.EventHandler(this.soundNotifyToolStripMenuItem_Click_1);
            // 
            // logSearcherToolStripMenuItem
            // 
            this.logSearcherToolStripMenuItem.Name = "logSearcherToolStripMenuItem";
            this.logSearcherToolStripMenuItem.Size = new System.Drawing.Size(107, 24);
            this.logSearcherToolStripMenuItem.Text = "Log Searcher";
            this.logSearcherToolStripMenuItem.Click += new System.EventHandler(this.logSearcherToolStripMenuItem_Click);
            // 
            // timersToolStripMenuItem
            // 
            this.timersToolStripMenuItem.Name = "timersToolStripMenuItem";
            this.timersToolStripMenuItem.Size = new System.Drawing.Size(65, 24);
            this.timersToolStripMenuItem.Text = "Timers";
            this.timersToolStripMenuItem.Click += new System.EventHandler(this.timersToolStripMenuItem_Click);
            // 
            // calendarToolStripMenuItem
            // 
            this.calendarToolStripMenuItem.Name = "calendarToolStripMenuItem";
            this.calendarToolStripMenuItem.Size = new System.Drawing.Size(80, 24);
            this.calendarToolStripMenuItem.Text = "Calendar";
            this.calendarToolStripMenuItem.Click += new System.EventHandler(this.calendarToolStripMenuItem_Click);
            // 
            // grangerToolStripMenuItem
            // 
            this.grangerToolStripMenuItem.Name = "grangerToolStripMenuItem";
            this.grangerToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.grangerToolStripMenuItem.Text = "Granger";
            this.grangerToolStripMenuItem.Click += new System.EventHandler(this.grangerToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.whatsNewToolStripMenuItem,
            this.guideToWurmAssistantToolStripMenuItem,
            this.toolStripSeparator6,
            this.findOtherCommunityToolsToolStripMenuItem,
            this.toolStripSeparator5,
            this.about1ToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // whatsNewToolStripMenuItem
            // 
            this.whatsNewToolStripMenuItem.Name = "whatsNewToolStripMenuItem";
            this.whatsNewToolStripMenuItem.Size = new System.Drawing.Size(269, 24);
            this.whatsNewToolStripMenuItem.Text = "What\'s New?";
            this.whatsNewToolStripMenuItem.Click += new System.EventHandler(this.whatsNewToolStripMenuItem_Click);
            // 
            // guideToWurmAssistantToolStripMenuItem
            // 
            this.guideToWurmAssistantToolStripMenuItem.Name = "guideToWurmAssistantToolStripMenuItem";
            this.guideToWurmAssistantToolStripMenuItem.Size = new System.Drawing.Size(269, 24);
            this.guideToWurmAssistantToolStripMenuItem.Text = "Guide to Wurm Assistant";
            this.guideToWurmAssistantToolStripMenuItem.Click += new System.EventHandler(this.guideToWurmAssistantToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(266, 6);
            // 
            // findOtherCommunityToolsToolStripMenuItem
            // 
            this.findOtherCommunityToolsToolStripMenuItem.Name = "findOtherCommunityToolsToolStripMenuItem";
            this.findOtherCommunityToolsToolStripMenuItem.Size = new System.Drawing.Size(269, 24);
            this.findOtherCommunityToolsToolStripMenuItem.Text = "Find other community tools...";
            this.findOtherCommunityToolsToolStripMenuItem.Click += new System.EventHandler(this.findOtherCommunityToolsToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(266, 6);
            // 
            // about1ToolStripMenuItem
            // 
            this.about1ToolStripMenuItem.Name = "about1ToolStripMenuItem";
            this.about1ToolStripMenuItem.Size = new System.Drawing.Size(269, 24);
            this.about1ToolStripMenuItem.Text = "About Wurm Assistant...";
            this.about1ToolStripMenuItem.Click += new System.EventHandler(this.about1ToolStripMenuItem_Click);
            // 
            // tESTToolStripMenuItem
            // 
            this.tESTToolStripMenuItem.Enabled = false;
            this.tESTToolStripMenuItem.Name = "tESTToolStripMenuItem";
            this.tESTToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.tESTToolStripMenuItem.Text = "TEST";
            this.tESTToolStripMenuItem.Visible = false;
            this.tESTToolStripMenuItem.Click += new System.EventHandler(this.tESTToolStripMenuItem_Click);
            // 
            // textBoxAppLog
            // 
            this.textBoxAppLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAppLog.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxAppLog.Location = new System.Drawing.Point(12, 43);
            this.textBoxAppLog.Multiline = true;
            this.textBoxAppLog.Name = "textBoxAppLog";
            this.textBoxAppLog.ReadOnly = true;
            this.textBoxAppLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxAppLog.Size = new System.Drawing.Size(803, 436);
            this.textBoxAppLog.TabIndex = 2;
            this.textBoxAppLog.TabStop = false;
            this.textBoxAppLog.TextChanged += new System.EventHandler(this.textBoxAppLog_TextChanged);
            // 
            // timerInit
            // 
            this.timerInit.Enabled = true;
            this.timerInit.Tick += new System.EventHandler(this.timerInit_Tick);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipText = "Click this icon to show the program again.";
            this.notifyIcon1.BalloonTipTitle = "Wurm Assistant";
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStripTray;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Wurm Assistant";
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            // 
            // contextMenuStripTray
            // 
            this.contextMenuStripTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TrayRestoreMainWindowToolStripMenuItem,
            this.toolStripSeparator3,
            this.TrayOpenSoundNotifyToolStripMenuItem,
            this.TrayOpenLogSearcherToolStripMenuItem,
            this.TrayOpenTimersToolStripMenuItem,
            this.TrayOpenCalendarToolStripMenuItem,
            this.trayOpenGrangerToolStripMenuItem,
            this.toolStripSeparator4,
            this.TrayExitToolStripMenuItem});
            this.contextMenuStripTray.Name = "contextMenuStripTray";
            this.contextMenuStripTray.Size = new System.Drawing.Size(225, 206);
            // 
            // TrayRestoreMainWindowToolStripMenuItem
            // 
            this.TrayRestoreMainWindowToolStripMenuItem.Name = "TrayRestoreMainWindowToolStripMenuItem";
            this.TrayRestoreMainWindowToolStripMenuItem.Size = new System.Drawing.Size(224, 24);
            this.TrayRestoreMainWindowToolStripMenuItem.Text = "Restore Main Window";
            this.TrayRestoreMainWindowToolStripMenuItem.Click += new System.EventHandler(this.TrayRestoreMainWindowToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(221, 6);
            // 
            // TrayOpenSoundNotifyToolStripMenuItem
            // 
            this.TrayOpenSoundNotifyToolStripMenuItem.Name = "TrayOpenSoundNotifyToolStripMenuItem";
            this.TrayOpenSoundNotifyToolStripMenuItem.Size = new System.Drawing.Size(224, 24);
            this.TrayOpenSoundNotifyToolStripMenuItem.Text = "Open Sound Notify";
            this.TrayOpenSoundNotifyToolStripMenuItem.Click += new System.EventHandler(this.TraySoundNotifyToolStripMenuItem_Click);
            // 
            // TrayOpenLogSearcherToolStripMenuItem
            // 
            this.TrayOpenLogSearcherToolStripMenuItem.Name = "TrayOpenLogSearcherToolStripMenuItem";
            this.TrayOpenLogSearcherToolStripMenuItem.Size = new System.Drawing.Size(224, 24);
            this.TrayOpenLogSearcherToolStripMenuItem.Text = "Open Log Searcher";
            this.TrayOpenLogSearcherToolStripMenuItem.Click += new System.EventHandler(this.TrayOpenLogSearcherToolStripMenuItem_Click);
            // 
            // TrayOpenTimersToolStripMenuItem
            // 
            this.TrayOpenTimersToolStripMenuItem.Name = "TrayOpenTimersToolStripMenuItem";
            this.TrayOpenTimersToolStripMenuItem.Size = new System.Drawing.Size(224, 24);
            this.TrayOpenTimersToolStripMenuItem.Text = "Open Timers";
            this.TrayOpenTimersToolStripMenuItem.Click += new System.EventHandler(this.TrayOpenTimersToolStripMenuItem_Click);
            // 
            // TrayOpenCalendarToolStripMenuItem
            // 
            this.TrayOpenCalendarToolStripMenuItem.Name = "TrayOpenCalendarToolStripMenuItem";
            this.TrayOpenCalendarToolStripMenuItem.Size = new System.Drawing.Size(224, 24);
            this.TrayOpenCalendarToolStripMenuItem.Text = "Open Calendar";
            this.TrayOpenCalendarToolStripMenuItem.Click += new System.EventHandler(this.TrayOpenCalendarToolStripMenuItem_Click);
            // 
            // trayOpenGrangerToolStripMenuItem
            // 
            this.trayOpenGrangerToolStripMenuItem.Name = "trayOpenGrangerToolStripMenuItem";
            this.trayOpenGrangerToolStripMenuItem.Size = new System.Drawing.Size(224, 24);
            this.trayOpenGrangerToolStripMenuItem.Text = "Open Granger";
            this.trayOpenGrangerToolStripMenuItem.Click += new System.EventHandler(this.trayOpenGrangerToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(221, 6);
            // 
            // TrayExitToolStripMenuItem
            // 
            this.TrayExitToolStripMenuItem.Name = "TrayExitToolStripMenuItem";
            this.TrayExitToolStripMenuItem.Size = new System.Drawing.Size(224, 24);
            this.TrayExitToolStripMenuItem.Text = "Exit";
            this.TrayExitToolStripMenuItem.Click += new System.EventHandler(this.TrayExitToolStripMenuItem_Click);
            // 
            // timerIsWurmRunning
            // 
            this.timerIsWurmRunning.Enabled = true;
            this.timerIsWurmRunning.Interval = 60000;
            this.timerIsWurmRunning.Tick += new System.EventHandler(this.timerIsWurmRunning_Tick);
            // 
            // timerSaveSettings
            // 
            this.timerSaveSettings.Interval = 2000;
            this.timerSaveSettings.Tick += new System.EventHandler(this.timerSaveSettings_Tick);
            // 
            // timerPollingLoop
            // 
            this.timerPollingLoop.Enabled = true;
            this.timerPollingLoop.Tick += new System.EventHandler(this.timerPollingLoop_Tick);
            // 
            // popupNotifier1
            // 
            this.popupNotifier1.AnimationDuration = 250;
            this.popupNotifier1.ContentFont = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.popupNotifier1.ContentText = null;
            this.popupNotifier1.Delay = 5000;
            this.popupNotifier1.HeaderHeight = 12;
            this.popupNotifier1.Image = null;
            this.popupNotifier1.OptionsMenu = null;
            this.popupNotifier1.Size = new System.Drawing.Size(400, 130);
            this.popupNotifier1.TitleColor = System.Drawing.Color.ForestGreen;
            this.popupNotifier1.TitleFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.popupNotifier1.TitleText = null;
            // 
            // backgroundWorkerHttpRequest
            // 
            this.backgroundWorkerHttpRequest.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerHttpRequest_DoWork);
            this.backgroundWorkerHttpRequest.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerHttpRequest_RunWorkerCompleted);
            // 
            // popupNotifierExt1
            // 
            this.popupNotifierExt1.AnimationDuration = 250;
            this.popupNotifierExt1.ContentFont = new System.Drawing.Font("Tahoma", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.popupNotifierExt1.ContentText = null;
            this.popupNotifierExt1.Delay = 4000;
            this.popupNotifierExt1.HeaderHeight = 12;
            this.popupNotifierExt1.Image = null;
            this.popupNotifierExt1.OptionsMenu = null;
            this.popupNotifierExt1.Size = new System.Drawing.Size(400, 130);
            this.popupNotifierExt1.TitleColor = System.Drawing.Color.ForestGreen;
            this.popupNotifierExt1.TitleFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.popupNotifierExt1.TitleText = null;
            // 
            // WurmAssistant
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(827, 491);
            this.Controls.Add(this.textBoxAppLog);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "WurmAssistant";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wurm Assistant";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WurmAssistant_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.WurmAssistant_FormClosed);
            this.Resize += new System.EventHandler(this.WurmAssistant_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStripTray.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerMainLoop;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureCharactersToolStripMenuItem;
        public System.Windows.Forms.TextBox textBoxAppLog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.Timer timerInit;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTray;
        private System.Windows.Forms.ToolStripMenuItem TrayRestoreMainWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem TrayOpenSoundNotifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem TrayExitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem soundNotifyToolStripMenuItem;
        private System.Windows.Forms.Timer timerIsWurmRunning;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem whatsNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem about1ToolStripMenuItem;
        private System.Windows.Forms.Timer timerSaveSettings;
        public System.Windows.Forms.Timer timerPollingLoop;
        private System.Windows.Forms.ToolStripMenuItem logSearcherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem timersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calendarToolStripMenuItem;
        private NotificationWindow.PopupNotifier popupNotifier1;
        private System.Windows.Forms.ToolStripMenuItem TrayOpenLogSearcherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TrayOpenTimersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TrayOpenCalendarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guideToWurmAssistantToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem findOtherCommunityToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.ComponentModel.BackgroundWorker backgroundWorkerHttpRequest;
        private System.Windows.Forms.ToolStripMenuItem tESTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grangerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trayOpenGrangerToolStripMenuItem;
        private NotifyWindowExt.PopupNotifierExt popupNotifierExt1;
    }
}

