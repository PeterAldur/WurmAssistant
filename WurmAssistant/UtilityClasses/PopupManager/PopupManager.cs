using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace WurmAssistant
{
    class PopupManager
    {
        FormPopupContainer popupContainer;
        Thread popupThread;

        public PopupManager()
        {
            BuildPopupThread();
        }

        void BuildPopupThread()
        {
            try
            {
                popupThread = new Thread(PopupThreadStart);
                popupThread.Priority = ThreadPriority.BelowNormal;
                popupThread.IsBackground = true;
                popupThread.Start();
            }
            catch (Exception _e)
            {
                TSafeLogger.DisplayExceptionData(_e);
                throw;
            }
        }

        void PopupThreadStart()
        {
            try
            {
                popupContainer = new FormPopupContainer();
                Application.Run(popupContainer);
            }
            catch (Exception _e)
            {
                TSafeLogger.DisplayExceptionData(_e);
                throw;
            }
        }

        public void ScheduleCustomPopupNotify(string title, string content, int timeToShowMillis = 3000)
        {
            try
            {
                popupContainer.BeginInvoke(new Action<string, string, int>(popupContainer.ScheduleCustomPopupNotify), title, content, timeToShowMillis);
            }
            catch (Exception _e)
            {
                if (_e is NullReferenceException || _e is InvalidOperationException)
                {
                    Logger.WriteLine("! Invoke exception at ScheduleCustomPopupNotify:");
                    Logger.LogException(_e);
                    try
                    {
                        if (_e is InvalidOperationException)
                        {
                            try { popupContainer.BeginInvoke(new Action(popupContainer.CloseThisContainer)); }
                            catch { };
                        }
                        BuildPopupThread();
                        popupContainer.BeginInvoke(new Action<string, string, int>(popupContainer.ScheduleCustomPopupNotify), title, content, timeToShowMillis);
                    }
                    catch (Exception _e2)
                    {
                        Logger.WriteLine("! Fix failed: ");
                        Logger.LogException(_e2);
                    }
                }
                else
                {
                    Logger.WriteLine("! Unknown Invoke exception at ScheduleCustomPopupNotify");
                    Logger.LogException(_e);
                }
            }
        }

        ~PopupManager()
        {
            try
            {
                popupContainer.BeginInvoke(new Action(popupContainer.CloseThisContainer));
            }
            catch { }
        }
    }
}
