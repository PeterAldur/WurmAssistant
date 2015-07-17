using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WurmAssistant
{
    public partial class FormTimers : Form
    {
        class NotifyHandler
        {
            string SoundName;
            bool Played = true;

            string Title;
            string Message;
            bool Shown = true;

            public NotifyHandler()
            {
                this.SoundName = "none";
                this.Title = "";
                this.Message = "";
            }

            public NotifyHandler(string soundname, string message_title, string message_content)
            {
                this.SoundName = soundname;
                this.Title = message_title;
                this.Message = message_content;
            }

            public void SetSound(string soundname)
            {
                this.SoundName = soundname;
            }

            public void SetTitle(string message_title)
            {
                this.Title = message_title;
            }

            public void SetMessage(string message_content)
            {
                this.Message = message_content;
            }

            public void TryToNotify()
            {
                if (!Played)
                {
                    SoundBank.PlaySound(SoundName);
                    Played = true;
                }
                if (!Shown)
                {
                    WurmAssistant.ZeroRef.ScheduleCustomPopupNotify(Title, Message);
                    Shown = true;
                }
            }

            public void Reset(bool ifResetNotify)
            {
                Played = false;
                if (ifResetNotify) Shown = false;
            }
        }

        ModuleTimers ParentModule;
        FormTimersDebug DebugUI = new FormTimersDebug();

        public FormTimers(ModuleTimers parentModule)
        {
            InitializeComponent();

            //because invokes require window handles and you can't reserve them
            this.Show();
            this.Hide();

            this.ParentModule = parentModule;

            FormTimersMeditInits();
            FormTimersPriestInits();
            FormTimersLockpickInits();

            UpdateServerUptimeDebugInfo();
        }

        #region MEDITATION

        bool firstUpdated_medit = false;
        bool remindSleepBonusActive = false;
        bool MeditSoundPlayed = false;
        bool MeditTrayShown = false;
        bool FsleepRemindSoundPlayed = true;
        bool FsleepTrayShown = true;

        //helpers
        float ShortMeditCooldownTimeSpanTicks = 0;
        float LongMeditCooldownTimeSpanTicks = 0;
        Color defRemindTextBoxBackColor;

        void FormTimersMeditInits()
        {
            ShortMeditCooldownTimeSpanTicks = ModuleTimers.MeditationsTimer.ShortMeditCooldown.Ticks;
            LongMeditCooldownTimeSpanTicks = ModuleTimers.MeditationsTimer.LongMeditCooldown.Ticks;

            checkBoxSleepBonusReminder.Checked = ParentModule.MeditTimer.RemindSleepBonus;
            defRemindTextBoxBackColor = textBoxRemindSleepBonus.BackColor;
            textBoxChosenFsleepSound.Text = ParentModule.MeditTimer.MeditFsleepWarningSound;
            textBoxChosenMeditSound.Text = ParentModule.MeditTimer.MeditReadySound;
            checkBoxMeditEnabled.Checked = ParentModule.MeditTimer.MeditTimerEnabled;
            checkBoxMeditCooldownTrayNotify.Checked = ParentModule.MeditTimer.TrayCooldownNotify;
            checkBoxSleepBonusTrayNotify.Checked = ParentModule.MeditTimer.TraySleepBonusNotify;
            UpdateQTimerButtonStyle();
        }

        public void UpdateMeditOutput()
        {
            textBoxMeditSkill.Text = ParentModule.MeditTimer.GetMeditSkill().ToString();

            DebugUI.UpdateMeditHistoryOutput(ParentModule.MeditTimer.GetAllMedits());

            firstUpdated_medit = true;
        }

        public void MeditJustHappened()
        {
            ResetPlayMeditSound();
            ResetMeditTrayNotify();

            if (ParentModule.MeditTimer.RemindSleepBonus)
            {
                if (ParentModule.MeditTimer.isSleepBonusActive)
                {
                    remindSleepBonusActive = true;
                }
                ResetPlayFsleepRemindSound();
                ResetTrayFsleepNotify();
            }
        }

        void PlayMeditSound()
        {
            if (!MeditSoundPlayed)
            {
                SoundBank.PlaySound(ParentModule.MeditTimer.MeditReadySound);
                MeditSoundPlayed = true;
            }
        }

        void ShowMeditPopup()
        {
            if (!MeditTrayShown)
            {
                WurmAssistant.ZeroRef.ScheduleCustomPopupNotify(ParentModule.PlayerName, "Meditation cooldown ready.");
                MeditTrayShown = true;
            }
        }

        public void ResetPlayMeditSound()
        {
            MeditSoundPlayed = false;
        }

        public void ResetMeditTrayNotify()
        {
            MeditTrayShown = false;
        }

        void PlayFsleepRemindSound()
        {
            if (!FsleepRemindSoundPlayed)
            {
                SoundBank.PlaySound(ParentModule.MeditTimer.MeditFsleepWarningSound);
                FsleepRemindSoundPlayed = true;
            }
        }

        void ShowFsleepPopup()
        {
            if (!FsleepTrayShown)
            {
                WurmAssistant.ZeroRef.ScheduleCustomPopupNotify(ParentModule.PlayerName, "Sleep Bonus can be turned off now.");
                FsleepTrayShown = true;
            }
        }

        public void ResetPlayFsleepRemindSound()
        {
            FsleepRemindSoundPlayed = false;
        }

        public void ResetTrayFsleepNotify()
        {
            FsleepTrayShown = false;
        }

        private void checkBoxSleepBonusReminder_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.MeditTimer.RemindSleepBonus = checkBoxSleepBonusReminder.Checked;
        }

        private void buttonChooseFsleepSound_Click(object sender, EventArgs e)
        {
            FormChooseSound ChooseSoundUI = new FormChooseSound();
            if (ChooseSoundUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ParentModule.MeditTimer.MeditFsleepWarningSound = ChooseSoundUI.ChosenSound;
                textBoxChosenFsleepSound.Text = ParentModule.MeditTimer.MeditFsleepWarningSound;
            }
        }

        private void buttonChooseMeditSound_Click(object sender, EventArgs e)
        {
            FormChooseSound ChooseSoundUI = new FormChooseSound();
            if (ChooseSoundUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ParentModule.MeditTimer.MeditReadySound = ChooseSoundUI.ChosenSound;
                textBoxChosenMeditSound.Text = ParentModule.MeditTimer.MeditReadySound;
            }
        }

        private void checkBoxMeditEnabled_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.MeditTimer.MeditTimerEnabled = checkBoxMeditEnabled.Checked;
        }

        private void buttonClearMeditSound_Click(object sender, EventArgs e)
        {
            ParentModule.MeditTimer.MeditReadySound = "none";
            textBoxChosenMeditSound.Text = ParentModule.MeditTimer.MeditReadySound;
        }

        private void buttonClearFsleepSound_Click(object sender, EventArgs e)
        {
            ParentModule.MeditTimer.MeditFsleepWarningSound = "none";
            textBoxChosenFsleepSound.Text = ParentModule.MeditTimer.MeditFsleepWarningSound;
        }

        private void checkBoxCooldownTrayNotify_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.MeditTimer.TrayCooldownNotify = checkBoxMeditCooldownTrayNotify.Checked;
        }

        private void checkBoxSleepBonusTrayNotify_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.MeditTimer.TraySleepBonusNotify = checkBoxSleepBonusTrayNotify.Checked;
        }

        private void buttonSetQTimerManually_Click(object sender, EventArgs e)
        {
            if (ParentModule.MeditTimer.QuestionTimerOverriden)
            {
                ParentModule.MeditTimer.QuestionTimerOverriden = false;
            }
            else
            {
                FormChooseQTimerManually ChooseUI = new FormChooseQTimerManually(ParentModule);
                if (ChooseUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ParentModule.MeditTimer.HandleQTimerOverride(ChooseUI.GetResultMeditLevel(), ChooseUI.GetResultOriginDate());
                }
            }
            UpdateQTimerButtonStyle();
        }

        private void buttonClearQTimerManually_Click(object sender, EventArgs e)
        {
            ParentModule.MeditTimer.QuestionTimerOverriden = false;
        }

        public void UpdateQTimerButtonStyle()
        {
            if (ParentModule.MeditTimer.QuestionTimerOverriden)
            {
                buttonSetQTimerManually.Text = "Clear manual timer";
            }
            else
            {
                buttonSetQTimerManually.Text = "Set question timer manually...";
            }
        }

        private void HandleMeditationUpdates()
        {
            if (firstUpdated_medit)
            {
                //medit
                TimeSpan timeToMedit = ParentModule.MeditTimer.GetNextMeditDate() - DateTime.Now;
                if (timeToMedit.Ticks < 0)
                {
                    timeToMedit = new TimeSpan(0);
                    PlayMeditSound();
                    if (ParentModule.MeditTimer.TrayCooldownNotify) ShowMeditPopup();
                }

                if (this.Visible == true)
                {
                    if (timeToMedit.Ticks == 0) textBoxMeditCooldownCounter.Text = "Ready!";
                    else textBoxMeditCooldownCounter.Text = timeToMedit.ToString("hh':'mm':'ss");

                    if (ParentModule.MeditTimer.IsLongCooldown())
                    {
                        progressBarMeditCooldown.Value = (int)(
                            (LongMeditCooldownTimeSpanTicks - (float)timeToMedit.Ticks)
                            / LongMeditCooldownTimeSpanTicks * 1000);
                    }
                    else
                    {
                        progressBarMeditCooldown.Value = (int)(
                            (ShortMeditCooldownTimeSpanTicks - (float)timeToMedit.Ticks)
                            / ShortMeditCooldownTimeSpanTicks * 1000);
                    }

                    //question counter
                    TimeSpan toNextQuestion;
                    if (ParentModule.MeditTimer.QuestionTimerOverriden)
                    {
                        toNextQuestion = ParentModule.MeditTimer.QuestionTimerOverrideDate - DateTime.Now;
                    }
                    else
                    {
                        toNextQuestion = ParentModule.MeditTimer.DateOfNextQuestionAttempt - DateTime.Now;
                    }

                    if (toNextQuestion.Ticks < 0)
                    {
                        textBoxToNextQuestion.Text = "Ready!";
                        if (ParentModule.MeditTimer.QuestionTimerOverriden)
                        {
                            ParentModule.MeditTimer.QuestionTimerOverriden = false;
                            UpdateQTimerButtonStyle();
                        }
                    }
                    else
                    {
                        string output = "";
                        if (toNextQuestion.Days > 1) output += toNextQuestion.Days + " days ";
                        else if (toNextQuestion.Days == 1) output += toNextQuestion.Days + " day ";
                        output += toNextQuestion.ToString("hh':'mm':'ss");
                        if (ParentModule.MeditTimer.QuestionTimerOverriden) output += " (manual timer)";
                        textBoxToNextQuestion.Text = output;
                    }
                }

                //sleep bonus reminder
                if (remindSleepBonusActive)
                    if (!ParentModule.MeditTimer.isSleepBonusActive)
                        remindSleepBonusActive = false;

                if (ParentModule.MeditTimer.RemindSleepBonus)
                {
                    if (ParentModule.MeditTimer.isSleepBonusActive)
                    {
                        TimeSpan ts = DateTime.Now - ParentModule.MeditTimer.SleepBonusStarted;

                        if (this.Visible == true) textBoxRemindSleepBonus.Text = ts.ToString("hh':'mm':'ss");

                        if (ts.TotalMinutes > 5.0D)
                        {
                            if (remindSleepBonusActive)
                            {
                                textBoxRemindSleepBonus.BackColor = Color.Red;
                                PlayFsleepRemindSound();
                                if (ParentModule.MeditTimer.TraySleepBonusNotify) ShowFsleepPopup();
                            }
                            else textBoxRemindSleepBonus.BackColor = Color.Yellow;
                        }
                    }
                    else
                    {
                        if (this.Visible == true)
                        {
                            textBoxRemindSleepBonus.Text = "sleep bonus off";
                            textBoxRemindSleepBonus.BackColor = defRemindTextBoxBackColor;
                        }
                    }
                }
            }
        }

        #endregion

        #region PRIESTHOOD

        bool firstUpdated_priest = false;

        NotifyHandler PrayNotifyHandler, SermonNotifyHandler, AlignmentNotifyHandler, FavorNotifyHandler;

        float PrayerCooldownTimeSpanTicks = 0;
        float SermonCooldownTimeSpanTicks = 0;
        float AlignmentCooldownTimeSpanTicks = 0;
        int i32_FaithLevel = 0;
        int i32_FavorLevel = 0;

        void FormTimersPriestInits()
        {
            checkBoxFaithEnabled.Checked = ParentModule.PriestTimer.PriesthoodTimerEnabled;

            //prayer
            checkBoxPrayerCooldownTrayNotify.Checked = ParentModule.PriestTimer.TrayPrayerNotify;
            textBoxChosenPrayerSound.Text = ParentModule.PriestTimer.PrayerReadySound;

            //favor
            checkBoxFavorTrayNotify.Checked = ParentModule.PriestTimer.TrayFavorNotify;
            textBoxChosenFavorSound.Text = ParentModule.PriestTimer.FavorReadySound;

            if (ParentModule.PriestTimer.FavorNotifyLevel > -1)
                numericUpDownNotifyFavorLevel.Value = ParentModule.PriestTimer.FavorNotifyLevel;
            else if (ParentModule.PriestTimer.FavorNotifyLevel == -1)
            {
                numericUpDownNotifyFavorLevel.Enabled = false;
                checkBoxNotifyFavorWhenMax.Checked = true;
            }

            //sermon
            checkBoxSermonTrayNotification.Checked = ParentModule.PriestTimer.TraySermonNotify;
            textBoxChosenSermonSound.Text = ParentModule.PriestTimer.SermonReadySound;

            //alignment
            checkBoxAlignmentTrayNotify.Checked = ParentModule.PriestTimer.TrayAlignmentNotify;
            textBoxChosenAlignmentSound.Text = ParentModule.PriestTimer.AlignmentReadySound;

            buttonSwapLight.Text = ParentModule.PriestTimer.IsWhiteLighter ? "WL" : "BL";
            AdjustSwapLightBackColor();

            buttonGod.Text = ParentModule.PriestTimer.PlayerReligion.ToString();

            //notifyhandlers
            InitPriestNotifyHandlers();

            PrayerCooldownTimeSpanTicks = ModuleTimers.PriesthoodTimer.PrayCooldown.Ticks;
            SermonCooldownTimeSpanTicks = ModuleTimers.PriesthoodTimer.SermonPreacherCooldown.Ticks;
            AlignmentCooldownTimeSpanTicks = ModuleTimers.PriesthoodTimer.AlignmentCooldown.Ticks;

            //player name in caption
            this.Text += " (" + ParentModule.PlayerName + ")";
        }

        private void HandlePriesthoodUpdates()
        {
            if (firstUpdated_priest)
            {
                TimeSpan timeToPrayer = ParentModule.PriestTimer.GetNextPrayerDate() - DateTime.Now;
                if (timeToPrayer.Ticks < 0)
                {
                    timeToPrayer = new TimeSpan(0);
                    PrayNotifyHandler.TryToNotify();
                }

                TimeSpan timeToSermon = ParentModule.PriestTimer.GetNextSermonDate() - DateTime.Now;
                if (timeToSermon.Ticks < 0)
                {
                    timeToSermon = new TimeSpan(0);
                    SermonNotifyHandler.TryToNotify();
                }

                TimeSpan timeToAlignment = ParentModule.PriestTimer.GetNextAlignmentDate() - DateTime.Now;
                if (timeToAlignment.Ticks < 0)
                {
                    timeToAlignment = new TimeSpan(0);
                    AlignmentNotifyHandler.TryToNotify();
                }

                if (checkBoxNotifyFavorWhenMax.Checked)
                    if (i32_FavorLevel >= i32_FaithLevel)
                        FavorNotifyHandler.TryToNotify();
                    else
                        FavorNotifyHandler.Reset(ParentModule.PriestTimer.TrayFavorNotify);
                else
                    if (i32_FavorLevel >= numericUpDownNotifyFavorLevel.Value)
                        FavorNotifyHandler.TryToNotify();
                    else
                        FavorNotifyHandler.Reset(ParentModule.PriestTimer.TrayFavorNotify);

                if (this.Visible == true)
                {
                    //prayer counter
                    if (timeToPrayer.Ticks == 0) textBoxPrayerCooldownCounter.Text = "Ready!";
                    else textBoxPrayerCooldownCounter.Text = timeToPrayer.ToString("hh':'mm':'ss");

                    if (timeToPrayer.Ticks > PrayerCooldownTimeSpanTicks)
                    {
                        timeToPrayer = new TimeSpan((long)PrayerCooldownTimeSpanTicks);
                    }

                    int newval = (int)(
                        (PrayerCooldownTimeSpanTicks - (float)timeToPrayer.Ticks)
                        / PrayerCooldownTimeSpanTicks * 1000);
                    progressBarPrayerCooldown.Value = GeneralHelper.Validate(newval, 0, 1000);

                    if (ParentModule.PriestTimer.IsPrayCountMaxed()) textBoxPrayerCooldownCounter.Text += " (maxed)";

                    //sermon counter
                    if (timeToSermon.Ticks == 0) textBoxSermonCounter.Text = "Ready!";
                    else textBoxSermonCounter.Text = timeToSermon.ToString("hh':'mm':'ss");

                    int newval2 = (int)(
                        (SermonCooldownTimeSpanTicks - (float)timeToSermon.Ticks)
                        / SermonCooldownTimeSpanTicks * 1000);
                    progressBarSermonCooldown.Value = GeneralHelper.Validate(newval2, 0, 1000);

                    //alignment counter
                    if (timeToAlignment.Ticks == 0) textBoxAlignmentCooldownCounter.Text = "Ready!";
                    else textBoxAlignmentCooldownCounter.Text = timeToAlignment.ToString("hh':'mm':'ss");

                    int newval3 = (int)(
                        (AlignmentCooldownTimeSpanTicks - (float)timeToAlignment.Ticks)
                        / AlignmentCooldownTimeSpanTicks * 1000);
                    progressBarAlignmentCooldown.Value = GeneralHelper.Validate(newval3, 0, 1000);
                }
            }
        }

        internal void UpdatePriesthoodOutput()
        {
            i32_FaithLevel = (int)ParentModule.PriestTimer.GetFaithSkill();
            textBoxFaithLevel.Text = ParentModule.PriestTimer.GetFaithSkill().ToString();

            i32_FavorLevel = (int)ParentModule.PriestTimer.GetFavorLevel();
            textBoxFavorLevel.Text = ParentModule.PriestTimer.GetFavorLevel().ToString();

            DebugUI.UpdatePrayerHistoryOutput(ParentModule.PriestTimer.GetAllPrayers());
            DebugUI.UpdateAlignmentHistoryOutput(ParentModule.PriestTimer.GetAllAlignments());

            firstUpdated_priest = true;
        }

        void InitPriestNotifyHandlers()
        {
            PrayNotifyHandler = new NotifyHandler(
                ParentModule.PriestTimer.PrayerReadySound, ParentModule.PlayerName, "Prayer cooldown finished.");
            PrayNotifyHandler.Reset(ParentModule.PriestTimer.TrayPrayerNotify);

            FavorNotifyHandler = new NotifyHandler(
                ParentModule.PriestTimer.FavorReadySound, ParentModule.PlayerName, "Enough favor is now available.");
            FavorNotifyHandler.Reset(ParentModule.PriestTimer.TrayFavorNotify);

            SermonNotifyHandler = new NotifyHandler(
                ParentModule.PriestTimer.SermonReadySound, ParentModule.PlayerName, "Sermon cooldown finished.");
            SermonNotifyHandler.Reset(ParentModule.PriestTimer.TraySermonNotify);

            AlignmentNotifyHandler = new NotifyHandler(
                ParentModule.PriestTimer.AlignmentReadySound, ParentModule.PlayerName, "Alignment cooldown finished.");
            AlignmentNotifyHandler.Reset(ParentModule.PriestTimer.TrayAlignmentNotify);
        }

        internal void PrayerJustHappened()
        {
            PrayNotifyHandler.Reset(ParentModule.PriestTimer.TrayPrayerNotify);
        }

        internal void SermonJustHappened()
        {
            SermonNotifyHandler.Reset(ParentModule.PriestTimer.TraySermonNotify);
        }

        internal void AlignmentGainJustHappened()
        {
            AlignmentNotifyHandler.Reset(ParentModule.PriestTimer.TrayAlignmentNotify);
        }

        private void checkBoxFaithEnabled_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.PriestTimer.PriesthoodTimerEnabled = checkBoxFaithEnabled.Checked;
        }

        private void checkBoxPrayerCooldownTrayNotify_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.PriestTimer.TrayPrayerNotify = checkBoxPrayerCooldownTrayNotify.Checked;
        }

        private void checkBoxSermonTrayNotification_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.PriestTimer.TraySermonNotify = checkBoxSermonTrayNotification.Checked;
        }

        private void checkBoxAlignmentTrayNotify_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.PriestTimer.TrayAlignmentNotify = checkBoxAlignmentTrayNotify.Checked;
        }

        private void buttonPrayerSound_Click(object sender, EventArgs e)
        {
            FormChooseSound ChooseSoundUI = new FormChooseSound();
            if (ChooseSoundUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ParentModule.PriestTimer.PrayerReadySound = ChooseSoundUI.ChosenSound;
                textBoxChosenPrayerSound.Text = ParentModule.PriestTimer.PrayerReadySound;
                PrayNotifyHandler.SetSound(ParentModule.PriestTimer.PrayerReadySound);
            }
        }

        private void buttonClearPrayerSound_Click(object sender, EventArgs e)
        {
            ParentModule.PriestTimer.PrayerReadySound = "none";
            textBoxChosenPrayerSound.Text = ParentModule.PriestTimer.PrayerReadySound;
            PrayNotifyHandler.SetSound(ParentModule.PriestTimer.PrayerReadySound);
        }

        private void buttonChooseSermonSound_Click(object sender, EventArgs e)
        {
            FormChooseSound ChooseSoundUI = new FormChooseSound();
            if (ChooseSoundUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ParentModule.PriestTimer.SermonReadySound = ChooseSoundUI.ChosenSound;
                textBoxChosenSermonSound.Text = ParentModule.PriestTimer.SermonReadySound;
                SermonNotifyHandler.SetSound(ParentModule.PriestTimer.SermonReadySound);
            }
        }

        private void buttonClearSermonSound_Click(object sender, EventArgs e)
        {
            ParentModule.PriestTimer.SermonReadySound = "none";
            textBoxChosenSermonSound.Text = ParentModule.PriestTimer.SermonReadySound;
            SermonNotifyHandler.SetSound(ParentModule.PriestTimer.SermonReadySound);
        }

        private void buttonChooseAlignmentSound_Click(object sender, EventArgs e)
        {
            FormChooseSound ChooseSoundUI = new FormChooseSound();
            if (ChooseSoundUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ParentModule.PriestTimer.AlignmentReadySound = ChooseSoundUI.ChosenSound;
                textBoxChosenAlignmentSound.Text = ParentModule.PriestTimer.AlignmentReadySound;
                AlignmentNotifyHandler.SetSound(ParentModule.PriestTimer.AlignmentReadySound);
            }
        }

        private void buttonClearAlignmentSound_Click(object sender, EventArgs e)
        {
            ParentModule.PriestTimer.AlignmentReadySound = "none";
            textBoxChosenAlignmentSound.Text = ParentModule.PriestTimer.AlignmentReadySound;
            AlignmentNotifyHandler.SetSound(ParentModule.PriestTimer.AlignmentReadySound);
        }

        private void buttonAlignmentHelp_Click(object sender, EventArgs e)
        {
            FormAlignmentHelp ui = new FormAlignmentHelp();
            ui.ShowDialog();
        }

        private void buttonSwapLight_Click(object sender, EventArgs e)
        {
            string newaligntype;
            if (ParentModule.PriestTimer.IsWhiteLighter) newaligntype = "BlackLighter";
            else newaligntype = "WhiteLighter";

            if (MessageBox.Show("Are you sure to change alignment type to: " + newaligntype, "Confirm change", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                if (ParentModule.PriestTimer.IsWhiteLighter) ParentModule.PriestTimer.IsWhiteLighter = false;
                else ParentModule.PriestTimer.IsWhiteLighter = true;
                buttonSwapLight.Text = (newaligntype == "BlackLighter" ? "BL" : "WL");
                AdjustSwapLightBackColor();
            }
        }

        private void buttonGod_Click(object sender, EventArgs e)
        {
            FormGodChoice ui = new FormGodChoice();
            if (ui.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ParentModule.PriestTimer.PlayerReligion = ui.ResultWurmReligion;
                buttonGod.Text = ParentModule.PriestTimer.PlayerReligion.ToString();
            }
        }

        void AdjustSwapLightBackColor()
        {
            if (ParentModule.PriestTimer.IsWhiteLighter)
            {
                buttonSwapLight.BackColor = Color.White;
            }
            else
            {
                buttonSwapLight.BackColor = Color.DarkGray;
            }
        }

        private void buttonVerifyAlignment_Click(object sender, EventArgs e)
        {
            FormVerifyAlignment ui = new FormVerifyAlignment(ParentModule.PriestTimer.GetAllAlignmentsForVerifyList());
            ui.ShowDialog();
        }

        private void buttonChooseFavorSound_Click(object sender, EventArgs e)
        {
            FormChooseSound ChooseSoundUI = new FormChooseSound();
            if (ChooseSoundUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ParentModule.PriestTimer.FavorReadySound = ChooseSoundUI.ChosenSound;
                textBoxChosenFavorSound.Text = ParentModule.PriestTimer.FavorReadySound;
                FavorNotifyHandler.SetSound(ParentModule.PriestTimer.FavorReadySound);
            }
        }

        private void buttonClearFavorSound_Click(object sender, EventArgs e)
        {
            ParentModule.PriestTimer.FavorReadySound = "none";
            textBoxChosenFavorSound.Text = ParentModule.PriestTimer.FavorReadySound;
            FavorNotifyHandler.SetSound(ParentModule.PriestTimer.FavorReadySound);
        }

        private void checkBoxFavorTrayNotify_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.PriestTimer.TrayFavorNotify = checkBoxFavorTrayNotify.Checked;
        }

        #endregion

        #region LOCKPICK

        bool firstUpdated_lockpick = false;
        NotifyHandler LockpickNotifyHandler;
        float LockpickCooldownTimeSpanTicks = 0;

        void FormTimersLockpickInits()
        {
            checkBoxLocksmithEnabled.Checked = ParentModule.LockpickTimer.LockpickingTimerEnabled;
            checkBoxLockpickingTrayNotify.Checked = ParentModule.LockpickTimer.TrayLockpickNotify;
            textBoxChosenLockpickSound.Text = ParentModule.LockpickTimer.LockpickReadySound;
            InitLockpickNotifyHandlers();

            LockpickCooldownTimeSpanTicks = ModuleTimers.LockpickingTimer.LockpickCooldown.Ticks;
        }



        private void HandleLockpickUpdates()
        {
            if (firstUpdated_lockpick)
            {
                TimeSpan timeToLockpick = ParentModule.LockpickTimer.GetNextLockpickDate() - DateTime.Now;
                if (timeToLockpick.Ticks < 0)
                {
                    timeToLockpick = new TimeSpan(0);
                    LockpickNotifyHandler.TryToNotify();
                }

                if (this.Visible == true)
                {
                    //sermon counter
                    if (timeToLockpick.Ticks == 0) textBoxLockpickCDCounter.Text = "Ready!";
                    else textBoxLockpickCDCounter.Text = timeToLockpick.ToString("hh':'mm':'ss");

                    progressBarLockpickCooldown.Value = (int)(
                        (LockpickCooldownTimeSpanTicks - (float)timeToLockpick.Ticks)
                        / LockpickCooldownTimeSpanTicks * 1000);
                }
            }
        }

        internal void UpdateLockpickOutput()
        {
            textBoxLockpickSkill.Text = ParentModule.LockpickTimer.GetLockpickSkill().ToString();
            firstUpdated_lockpick = true;
        }

        private void InitLockpickNotifyHandlers()
        {
            LockpickNotifyHandler = new NotifyHandler(
                ParentModule.LockpickTimer.LockpickReadySound, ParentModule.PlayerName, "Lockpick cooldown finished.");
            LockpickNotifyHandler.Reset(ParentModule.LockpickTimer.TrayLockpickNotify);
        }

        internal void LockpickGainJustHappened()
        {
            LockpickNotifyHandler.Reset(ParentModule.LockpickTimer.TrayLockpickNotify);
        }

        private void buttonLockpickingSound_Click(object sender, EventArgs e)
        {
            FormChooseSound ChooseSoundUI = new FormChooseSound();
            if (ChooseSoundUI.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ParentModule.LockpickTimer.LockpickReadySound = ChooseSoundUI.ChosenSound;
                textBoxChosenLockpickSound.Text = ParentModule.LockpickTimer.LockpickReadySound;
                LockpickNotifyHandler.SetSound(ParentModule.LockpickTimer.LockpickReadySound);
            }
        }

        private void buttonClearLockpickSound_Click(object sender, EventArgs e)
        {
            ParentModule.LockpickTimer.LockpickReadySound = "none";
            textBoxChosenLockpickSound.Text = ParentModule.LockpickTimer.LockpickReadySound;
            LockpickNotifyHandler.SetSound(ParentModule.LockpickTimer.LockpickReadySound);
        }

        private void checkBoxLockpickingTrayNotify_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.LockpickTimer.TrayLockpickNotify = checkBoxLockpickingTrayNotify.Checked;
        }

        private void checkBoxLocksmithEnabled_CheckedChanged(object sender, EventArgs e)
        {
            ParentModule.LockpickTimer.LockpickingTimerEnabled = checkBoxLocksmithEnabled.Checked;
        }

        #endregion

        #region CONTROL

        public void UpdateServerUptimeDebugInfo()
        {
            DebugUI.UpdateDebugOutput();
        }

        public void RestoreFromMin()
        {
            if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;
        }

        private void FormTimers_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        public void HandleSearchCallback(LogSearchData logsearchdata)
        {
            ParentModule.HandleSearchCallback(logsearchdata);
        }

        private void buttonDebug_Click(object sender, EventArgs e)
        {
            DebugUI.Show();
            if (DebugUI.WindowState == FormWindowState.Minimized) DebugUI.WindowState = FormWindowState.Normal;
        }


        //interface update handling
        private void timerUpdateCountdowns_Tick(object sender, EventArgs e)
        {
            UpdateServerUptimeDebugInfo();
            if (ParentModule.MeditTimer.MeditTimerEnabled)
            {
                HandleMeditationUpdates();
            }
            if (ParentModule.PriestTimer.PriesthoodTimerEnabled)
            {
                HandlePriesthoodUpdates();
            }
            if (ParentModule.LockpickTimer.LockpickingTimerEnabled)
            {
                HandleLockpickUpdates();
            }
        }

        #endregion

        private void checkBoxNotifyFavorWhenMax_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxNotifyFavorWhenMax.Checked)
            {
                ParentModule.PriestTimer.FavorNotifyLevel = -1;
                numericUpDownNotifyFavorLevel.Value = 0;
                numericUpDownNotifyFavorLevel.Enabled = false;
            }
            else
            {
                ParentModule.PriestTimer.FavorNotifyLevel = 0;
                numericUpDownNotifyFavorLevel.Enabled = true;
                numericUpDownNotifyFavorLevel.Value = ParentModule.PriestTimer.FavorNotifyLevel;
            }
        }
    }
}
