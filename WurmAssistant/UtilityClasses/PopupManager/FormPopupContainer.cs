using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WurmAssistant
{
    public partial class FormPopupContainer : Form
    {
        struct PopupQueueItem
        {
            public string Title;
            public string Content;
            public int TimeToShowMillis;
            public PopupQueueItem(string title, string content, int timeToShowMillis)
            {
                if (title != null) Title = title; else Title = "Wurm Assistant";
                if (content != null) Content = content; else Content = "";
                TimeToShowMillis = timeToShowMillis;
            }
        }

        Queue<PopupQueueItem> PopupQueue = new Queue<PopupQueueItem>();
        int PopupQueueDelay = 0;

        public FormPopupContainer()
        {
            try
            {
                InitializeComponent();
                timer1.Enabled = true;
            }
            catch (Exception _e)
            {
                TSafeLogger.DisplayExceptionData(_e);
                throw;
            }
        }

        private void popupNotifier1_Click(object sender, EventArgs e)
        {
            try
            {
                popupNotifier1.Hide();
                PopupQueueDelay = 250;
            }
            catch (Exception _e)
            {
                TSafeLogger.DisplayExceptionData(_e);
                throw;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                HandlePopupQueue();
            }
            catch (Exception _e)
            {
                TSafeLogger.DisplayExceptionData(_e);
                throw;
            }
        }

        void HandlePopupQueue()
        {
            if (PopupQueue.Count != 0 && PopupQueueDelay <= 0)
            {
                PopupQueueItem item = PopupQueue.Dequeue();
                popupNotifier1.TitleText = item.Title;
                popupNotifier1.ContentText = item.Content;
                AppenedMoreMessagesWithSameTitle(item, 3);
                popupNotifier1.Delay = item.TimeToShowMillis;
                popupNotifier1.Popup();
                PopupQueueDelay = item.TimeToShowMillis + 250;
            }
            else PopupQueueDelay -= timer1.Interval;
        }

        void AppenedMoreMessagesWithSameTitle(PopupQueueItem item, int count)
        {
            for (int i = 1; i <= count; i++)
            {
                if (!TryAppendMessage(item)) break;
            }
        }

        bool TryAppendMessage(PopupQueueItem item)
        {
            if (PopupQueue.Count > 0 && PopupQueue.Peek().Title == item.Title)
            {
                popupNotifier1.ContentText += "\n" + PopupQueue.Dequeue().Content;
                return true;
            }
            return false;
        }

        public void ScheduleCustomPopupNotify(string title, string content, int timeToShowMillis = 4000)
        {
            try
            {
                PopupQueue.Enqueue(new PopupQueueItem(title, content, timeToShowMillis));
            }
            catch (Exception _e)
            {
                TSafeLogger.DisplayExceptionData(_e);
                throw;
            }
        }

        public void CloseThisContainer()
        {
            Application.ExitThread();
        }

        private void FormPopupContainer_Load(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
            }
            catch (Exception _e)
            {
                TSafeLogger.DisplayExceptionData(_e);
                throw;
            }
        }
    }
}
