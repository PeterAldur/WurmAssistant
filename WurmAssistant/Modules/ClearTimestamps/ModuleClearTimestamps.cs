using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WurmAssistant
{
    /// <summary>
    /// clears timestamps from log messages for easier parsing
    /// </summary>
    public class ModuleClearTimestamps : Module
    {
        public ModuleClearTimestamps()
            : base("ClearTimestamps")
        {
        }

        public override void HandleNewLogEvents(List<string> newLogEvents, GameLogState log)
        {
            for (int i = 0; i < newLogEvents.Count; i++)
            {
                try { newLogEvents[i] = newLogEvents[i].Remove(0, 11); }
                catch { System.Diagnostics.Debug.WriteLine("error at cleartimestamps: " + newLogEvents[i]); }
                System.Diagnostics.Debug.WriteLine(newLogEvents[i]);
            }
        }
    }
}
